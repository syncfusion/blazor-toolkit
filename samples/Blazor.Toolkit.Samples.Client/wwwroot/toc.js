// Minimal scroll-spy and smooth-scroll for the right-sidebar TOC treeview.
// Pure behavior shim: receives heading IDs from .NET and observes them with
// an IntersectionObserver. Does NOT parse DOM or mutate heading IDs.
//
// Layout note: the page uses a fixed-height flex layout (see
// Layout/MainLayout.razor.css) where .page { height: 100vh; overflow: hidden }
// and .main-area { overflow: auto }. Scroll position lives on the .main-area
// element, NOT on window. All scroll helpers below therefore target whichever
// element is the actual scroll container, falling back to window for pages
// that scroll normally.

(function (ns) {
    if (!ns) return;

    function getHeadings(ids) {
        if (!Array.isArray(ids)) return [];
        var found = [];
        for (var i = 0; i < ids.length; i++) {
            var el = document.getElementById(ids[i]);
            if (el) found.push(el);
        }
        return found;
    }

    function pickVisible(entries) {
        var best = null;
        for (var i = 0; i < entries.length; i++) {
            var e = entries[i];
            if (e && e.isIntersecting) {
                if (!best || e.intersectionRatio > best.ratio) {
                    best = e;
                }
            }
        }
        return best;
    }

    // Identify the element that actually owns the scroll for the main
    // page content. The .main-area container is the scroll container in
    // the default layout. If it can't be found we fall back to window.
    function getScrollContainer() {
        var el = document.querySelector('.main-area');
        return el || window;
    }

    // Small page-level helpers. Lives in the same file to avoid
    // adding a new static asset for a one-line call.
    ns.scrollHelpers = {
        initialize: function () {
            try {
                if (history && 'scrollRestoration' in history) {
                    history.scrollRestoration = 'manual';
                }
                var loaderElement = document.querySelector('.loader-class');
                if (loaderElement) {
                    loaderElement.classList.remove("loader-class");
                }
            } catch (e) { }
        },
        // Reset scroll on the actual container that owns it. The Blazor
        // side triggers this from OnAfterRenderAsync once the new page
        // is mounted, so the scroll lands on the new page. The container
        // is resolved each call because the router may swap DOM nodes
        // between navigations.
        scrollToTop: function () {
            try {
                var container = getScrollContainer();
                if (container === window) {
                    window.scrollTo(0, 0);
                }
                else {
                    container.scrollTop = 0;
                }
            } catch (e) { }
        }
    };

    ns.tocScrollspy = {
        _observer: null,
        _dotNetRef: null,

        observe: function (ids, dotNetRef) {
            try {
                if (this._observer) {
                    try { this._observer.disconnect(); } catch (e) { }
                    this._observer = null;
                }
                this._dotNetRef = dotNetRef || null;

                var headings = getHeadings(ids || []);
                if (headings.length === 0) return;

                var self = this;
                // Observe against the scroll container so headings that
                // scroll inside .main-area are picked up correctly.
                this._observer = new IntersectionObserver(function (entries) {
                    try {
                        var visible = pickVisible(entries);
                        if (visible && self._dotNetRef) {
                            self._dotNetRef.invokeMethodAsync('NotifyActiveId', visible.target.id);
                        }
                    } catch (e) { }
                }, { root: null, rootMargin: '0px 0px -60% 0px', threshold: [0, 0.1, 0.25, 0.5, 0.75, 1] });

                for (var i = 0; i < headings.length; i++) {
                    this._observer.observe(headings[i]);
                }
            } catch (e) { }
        },

        // Scroll smoothly to a heading inside the .main-area container.
        // Uses container.scrollTop so the scroll stays in sync with the
        // page layout's actual scrollable element.
        scrollToId: function (id) {
            try {
                var el = document.getElementById(id);
                if (!el) return;
                var container = getScrollContainer();
                if (container === window) {
                    el.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
                else {
                    // Compute the heading's offset relative to the
                    // scroll container and animate to it.
                    var top = el.getBoundingClientRect().top - container.getBoundingClientRect().top + container.scrollTop;
                    container.scrollTo({ top: top, behavior: 'smooth' });
                }
                if (history && history.replaceState) {
                    var path = (location.pathname || '') + (location.search || '');
                    history.replaceState(null, '', path + '#' + id);
                }
            } catch (e) { }
        },

        disconnect: function () {
            try { if (this._observer) { this._observer.disconnect(); } } catch (e) { }
            this._observer = null;
            this._dotNetRef = null;
        }
    };
})(window);
