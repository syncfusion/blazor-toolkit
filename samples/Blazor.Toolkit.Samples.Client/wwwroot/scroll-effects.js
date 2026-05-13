window.scrollEffects = window.scrollEffects || {};
(function(){
  let observer = null;

  // initReveal: shows elements when they intersect. By default it will
  // toggle visibility when elements enter/exit the viewport. To keep the
  // original "reveal once" behavior, pass { once: true } as the second arg.
  // Usage examples:
  //   scrollEffects.initReveal('.component-tile') // toggle on enter/exit
  //   scrollEffects.initReveal('.component-tile', { once: true }) // reveal once
  window.scrollEffects.initReveal = function(selector = '.component-tile', options) {
    const opts = options || {};
    const threshold = (typeof opts.threshold === 'number') ? opts.threshold : 0.12;
    const once = !!opts.once;

    const items = document.querySelectorAll(selector);
    if (!items || items.length === 0) return;
    items.forEach(i => i.classList.add('reveal'));

    // clean up previous observer if any
    if (observer) { observer.disconnect(); observer = null; }

    observer = new IntersectionObserver((entries, obs) => {
      entries.forEach(e => {
        if (e.isIntersecting) {
          e.target.classList.add('visible');
          if (once) obs.unobserve(e.target);
        } else {
          // when not 'once', remove the visible class so it can animate again
          if (!once) e.target.classList.remove('visible');
        }
      });
    }, { threshold });

    items.forEach(i => observer.observe(i));
    window.scrollEffects._observer = observer;
  };

  window.scrollEffects.dispose = function() {
    if (observer) { observer.disconnect(); observer = null; }
  };
})();
