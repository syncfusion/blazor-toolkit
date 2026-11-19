// MainLayout JS helpers -- centralize inline scripts for Blazor interop
(function(window){
    window.mainLayout = window.mainLayout || {};

    // Ensure a responsive helper is available on window.responsiveBlazor
    window.mainLayout.ensureResponsive = function(){
        if (!window.responsiveBlazor) {
            window.responsiveBlazor = {
                _handler: null,
                _dotNetRef: null,
                register: function(dotNetRef){
                    this._dotNetRef = dotNetRef;
                    var timer = null;
                    var self = this;
                    function handler(){
                        clearTimeout(timer);
                        timer = setTimeout(function(){
                            try { self._dotNetRef.invokeMethodAsync('NotifyResize', window.innerWidth); } catch(e){}
                        }, 120);
                    }
                    window.addEventListener('resize', handler);
                    this._handler = handler;
                    handler();
                },
                dispose: function(){
                    if (this._handler) { window.removeEventListener('resize', this._handler); this._handler = null; }
                    this._dotNetRef = null;
                }
            };
        }
    };

    // Theme init: returns 'dark' or 'light'
    window.mainLayout.initTheme = function(){
        try{
            var t = localStorage.getItem('theme') || 'light';
            if (t === 'dark'){
                document.documentElement.classList.add('dark');
                document.body.classList.add('e-dark-mode');
            } else {
                document.documentElement.classList.remove('dark');
                document.body.classList.remove('e-dark-mode');
            }
            return t;
        } catch(e){ return 'light'; }
    };

    // Dir init: returns 'rtl' or 'ltr'
    window.mainLayout.initDir = function(){
        try{
            var d = localStorage.getItem('dir') || 'ltr';
            document.documentElement.dir = d;
            if (d === 'rtl'){
                document.documentElement.classList.add('rtl');
                document.documentElement.classList.add('page-rtl');
            } else {
                document.documentElement.classList.remove('rtl');
                document.documentElement.classList.remove('page-rtl');
            }
            return d;
        } catch(e){ return 'ltr'; }
    };

    // Set direction and persist
    window.mainLayout.setDir = function(dir){
        try{
            localStorage.setItem('dir', dir);
            document.documentElement.dir = dir;
            if (dir === 'rtl'){
                document.documentElement.classList.add('rtl');
                document.documentElement.classList.add('page-rtl');
            } else {
                document.documentElement.classList.remove('rtl');
                document.documentElement.classList.remove('page-rtl');
            }
        } catch(e){}
    };

    var preferencesListener = null;
    var preferencesRef = null;

    window.mainLayout.openPreferences = function (dotNetRef) {
        window.mainLayout.closePreferences();
        preferencesRef = dotNetRef;
        var anchor = document.querySelector('.top-layout-inner [aria-label="Settings"]');
        var style = 'top: 80px; inset-inline-end: 16px;';
        if (anchor && anchor.getBoundingClientRect) {
            var rect = anchor.getBoundingClientRect();
            var rtl = getComputedStyle(document.documentElement).direction === 'rtl'
                || document.documentElement.dir === 'rtl';
            var top = Math.round(rect.bottom + 8);
            // Same inset the previous absolute popup used: the gear's end edge, shifted 12px outward.
            var end = rtl ? rect.left : window.innerWidth - rect.right;
            var inset = Math.max(8, Math.round(end - 12));
            style = 'top: ' + top + 'px; inset-inline-end: ' + inset + 'px;';
        }

        preferencesListener = function (event) {
            var flyout = document.querySelector('.preferences-flyout');
            if (event.key === 'Escape' || (flyout && !flyout.contains(event.target) && !(anchor && anchor.contains(event.target)))) {
                if (event.type === 'keydown' && event.key !== 'Escape') {
                    return;
                }
                window.mainLayout.closePreferences();
                if (preferencesRef) {
                    preferencesRef.invokeMethodAsync('ClosePreferencesFromScript').catch(function () { });
                }
            }
        };
        document.addEventListener('pointerdown', preferencesListener, true);
        document.addEventListener('keydown', preferencesListener);
        return style;
    };

    window.mainLayout.closePreferences = function () {
        if (preferencesListener) {
            document.removeEventListener('pointerdown', preferencesListener, true);
            document.removeEventListener('keydown', preferencesListener);
            preferencesListener = null;
        }
    };

    // Set theme and persist
    window.mainLayout.setTheme = function(theme){
        try{
            localStorage.setItem('theme', theme);
            if (theme === 'dark'){
                document.documentElement.classList.add('dark');
                document.body.classList.add('e-dark-mode');
            } else {
                document.documentElement.classList.remove('dark');
                document.body.classList.remove('e-dark-mode');
            }

            window.dispatchEvent(new CustomEvent('mainlayout-themechanged', { detail: theme }));
        } catch(e){}
    };

    window.mainLayout.observeTheme = function(dotNetRef){
        try{
            window.mainLayout._themeDotNetRef = dotNetRef;

            if (!window.mainLayout._themeHandler) {
                window.mainLayout._themeHandler = function (e) {
                    try {
                        var theme = (e && e.detail) ? e.detail : (document.documentElement.classList.contains('dark') ? 'dark' : 'light');
                        window.mainLayout._themeDotNetRef.invokeMethodAsync('NotifyThemeChanged', theme);
                    } catch(err){}
                };

                window.addEventListener('mainlayout-themechanged', window.mainLayout._themeHandler);
            }

            var currentTheme = document.documentElement.classList.contains('dark') ? 'dark' : 'light';
            dotNetRef.invokeMethodAsync('NotifyThemeChanged', currentTheme);
        } catch(e){}
    };

    window.mainLayout.disposeThemeObserver = function(){
        try{
            if (window.mainLayout._themeHandler) {
                window.removeEventListener('mainlayout-themechanged', window.mainLayout._themeHandler);
                window.mainLayout._themeHandler = null;
            }
            window.mainLayout._themeDotNetRef = null;
        } catch(e){}
    };

    // Navigate helpers
    window.mainLayout.assignRoot = function(){ try { location.assign('/'); } catch(e){} };
    window.mainLayout.reload = function(){ try { location.reload(); } catch(e){} };

    // Set culture fallback used by ModeSwitcher: persist to localStorage and cookie
    window.mainLayout.setCulture = function(culture){
        try{
            localStorage.setItem('BlazorCulture', culture);
            var cookieName = '.' + 'AspNetCore.Culture';
            var cookieValue = 'c=' + culture + '|uic=' + culture;
            document.cookie = cookieName + '=' + encodeURIComponent(cookieValue) + '; path=/';
        } catch(e){}
    };

    window.copyCode = (text) => {
        navigator.clipboard.writeText(text);
    };


    window.refreshTab = (code, element) => {
        var highlightCodeInterval = setInterval(highlightSource, 0);
        function highlightSource() {
            var tabs = element;
            if (tabs) {
                tabs.textContent = code;
                tabs.classList.add('blazor');
                hljs.highlightBlock(tabs);
                clearInterval(highlightCodeInterval);
            }
        }
    };

    // Plain-text variant of refreshTab. Renders the snippet without
    // applying any syntax highlighting (no hljs spans, no colour
    // classes). Used for descriptive prose inside a code block.
    window.setPlainText = (element, code) => {
        if (!element) return;
        element.textContent = code;
        // Strip any hljs-added classes/attributes from a previous
        // render so the element is fully unstyled.
        element.removeAttribute('class');
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
        element.appendChild(document.createTextNode(code));
    };

})(window);
