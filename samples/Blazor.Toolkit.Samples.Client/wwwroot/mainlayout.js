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
                tabs.innerHTML = code;
                tabs.classList.add('blazor');
                hljs.highlightBlock(tabs);
                clearInterval(highlightCodeInterval);
            }
        }
    };

})(window);
