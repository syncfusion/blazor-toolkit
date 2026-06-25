window.toc = window.toc || {};
(function(ns){
    ns.getHeadings = function(containerSelector){
        try {
            setTimeout( function() {
                var loaderElement = document.querySelector('.loader-class');
                if (loaderElement) {
                    loaderElement.classList.remove("loader-class");
                }
            }, 100)
            var container = document.querySelector(containerSelector) || document;
            var nodes = container.querySelectorAll('h5,h6');
            var arr = [];
            nodes.forEach(function(n){
                var id = n.id;
                if (!id || id.trim() === ''){
                    var base = n.textContent.trim().toLowerCase().replace(/[^a-z0-9]+/g,'-').replace(/^\-+|\-+$/g,'');
                    if (!base) base = 'heading';
                    id = base;
                    var i = 1;
                    while(document.getElementById(id)){
                        id = base + '-' + (i++);
                    }
                    n.id = id;
                }
                arr.push({ id: n.id, text: n.textContent.trim(), tag: n.tagName, level: parseInt(n.tagName.substring(1)) });
            });
            const el = document.querySelector('main');
            if (el) el.scrollTop = 0;
            return arr;
        } catch(e){
            return [];
        }
    };

    ns.scrollToId = function(id){
        try {
            var el = document.getElementById(id);
            if (el){
                el.scrollIntoView({ behavior: 'smooth', block: 'start' });
                const url = location.pathname + location.search + '#' + id;
                if(history && history.replaceState){
                    history.replaceState(null, '', url);
                }
            }
        } catch(e){}
    };

    // Return the id of the heading currently visible in the viewport (or null)
    ns.getVisibleHeading = function(containerSelector){
        try {
            var container = document.querySelector(containerSelector) || document;
            var nodes = container.querySelectorAll('h5,h6');
            if (!nodes || nodes.length === 0) return null;
            var winH = window.innerHeight || document.documentElement.clientHeight;
            var best = null;
            var bestDist = Infinity;
            nodes.forEach(function(n){
                var rect = n.getBoundingClientRect();
                // consider headings that are at or near the top of viewport
                var dist = Math.abs(rect.top);
                if (rect.top >= 0 && rect.top < winH) {
                    if (dist < bestDist) { bestDist = dist; best = n; }
                }
            });
            if (best) return best.id || null;
            // fallback: return first heading whose top is below viewport top
            for (var i=0;i<nodes.length;i++){
                var r = nodes[i].getBoundingClientRect();
                if (r.top > 0) return nodes[i].id || null;
            }
            return nodes[0] ? (nodes[0].id || null) : null;
        } catch(e){ return null; }
    };

    // Observe headings visibility and notify .NET via DotNetObjectReference
    ns.observeVisibleHeading = function(containerSelector, dotNetRef){
        try {
            var container = document.querySelector(containerSelector) || document;
            var nodes = container.querySelectorAll('h5,h6');
            if (!nodes || nodes.length === 0) return;
            if (ns._observer) ns._observer.disconnect();
            var options = { root: null, rootMargin: '0px 0px -60% 0px', threshold: [0,0.1,0.25,0.5,0.75,1] };
            ns._observer = new IntersectionObserver(function(entries){
                var visible = null;
                entries.forEach(function(e){
                    if (e.isIntersecting) {
                        if (!visible || e.intersectionRatio > visible.ratio) {
                            visible = { id: e.target.id, ratio: e.intersectionRatio };
                        }
                    }
                });
                if (visible && dotNetRef) {
                    try { dotNetRef.invokeMethodAsync('NotifyVisibleHeading', visible.id); } catch(e){}
                } else if (dotNetRef) {
                    // fallback: compute visible heading synchronously
                    var vid = ns.getVisibleHeading(containerSelector);
                    if (vid) { try { dotNetRef.invokeMethodAsync('NotifyVisibleHeading', vid); } catch(e){} }
                }
            }, options);
            nodes.forEach(function(n){ if (n.id) ns._observer.observe(n); });
        } catch(e){}
    };

    ns.disconnectObserver = function(){ if (ns._observer){ ns._observer.disconnect(); ns._observer = null; } };

    // Get h2/h3 headings (used by Release Notes page)
    ns.getHeadingsH2H3 = function(containerSelector){
        try {
            var container = document.querySelector(containerSelector) || document;
            var nodes = container.querySelectorAll('h2,h3');
            var arr = [];
            nodes.forEach(function(n){
                var id = n.id;
                if (!id || id.trim() === ''){
                    var base = n.textContent.trim().toLowerCase().replace(/[^a-z0-9]+/g,'-').replace(/^\-+|\-+$/g,'');
                    if (!base) base = 'heading';
                    id = base;
                    var i = 1;
                    while(document.getElementById(id)){
                        id = base + '-' + (i++);
                    }
                    n.id = id;
                }
                arr.push({ id: n.id, text: n.textContent.trim(), tag: n.tagName, level: parseInt(n.tagName.substring(1)) });
            });
            return arr;
        } catch(e){
            return [];
        }
    };

    // Get visible h2/h3 heading
    ns.getVisibleHeadingH2H3 = function(containerSelector){
        try {
            var container = document.querySelector(containerSelector) || document;
            var nodes = container.querySelectorAll('h2,h3');
            if (!nodes || nodes.length === 0) return null;
            var winH = window.innerHeight || document.documentElement.clientHeight;
            var best = null;
            var bestDist = Infinity;
            nodes.forEach(function(n){
                var rect = n.getBoundingClientRect();
                var dist = Math.abs(rect.top);
                if (rect.top >= 0 && rect.top < winH) {
                    if (dist < bestDist) { bestDist = dist; best = n; }
                }
            });
            if (best) return best.id || null;
            for (var i=0;i<nodes.length;i++){
                var r = nodes[i].getBoundingClientRect();
                if (r.top > 0) return nodes[i].id || null;
            }
            return nodes[0] ? (nodes[0].id || null) : null;
        } catch(e){ return null; }
    };

    // Observe h2/h3 headings for release notes
    ns.observeHeadingsH2H3 = function(containerSelector, dotNetRef){
        try {
            var container = document.querySelector(containerSelector) || document;
            var nodes = container.querySelectorAll('h2,h3');
            if (!nodes || nodes.length === 0) return;
            if (ns._observerH2H3) ns._observerH2H3.disconnect();
            var options = { root: null, rootMargin: '-20% 0px -60% 0px', threshold: [0,0.1,0.25,0.5,0.75,1] };
            ns._observerH2H3 = new IntersectionObserver(function(entries){
                var visible = null;
                entries.forEach(function(e){
                    if (e.isIntersecting) {
                        if (!visible || e.intersectionRatio > visible.ratio) {
                            visible = { id: e.target.id, ratio: e.intersectionRatio };
                        }
                    }
                });
                if (visible && dotNetRef) {
                    try { dotNetRef.invokeMethodAsync('NotifyVisibleHeading', visible.id); } catch(e){}
                }
            }, options);
            nodes.forEach(function(n){ if (n.id) ns._observerH2H3.observe(n); });
        } catch(e){}
    };

    ns.disconnectObserverH2H3 = function(){ if (ns._observerH2H3){ ns._observerH2H3.disconnect(); ns._observerH2H3 = null; } };

    // Refresh TOC - re-reads headings and re-registers observer
    ns.refreshToc = function(dotNetRef){
        try {
            var container = '.page-content';
            // Re-read headings from the updated HTML
            var headings = ns.getHeadingsH2H3(container);
            // Re-register observer for the new content
            ns.disconnectObserverH2H3();
            if (dotNetRef) {
                ns.observeHeadingsH2H3(container, dotNetRef);
            }
            return headings;
        } catch(e){ return []; }
    };
})(window.toc);
