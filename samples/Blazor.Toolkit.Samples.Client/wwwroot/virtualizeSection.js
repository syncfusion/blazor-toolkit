window.virtualizeSection = {
    observers: new WeakMap(),
    observe: function(element, dotNetRef) {
        if (!element) return;
        if (this.observers.has(element)) return;
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotNetRef.invokeMethodAsync('OnVisible');
                    observer.disconnect();
                }
            });
        }, { threshold: 0.1 });
        observer.observe(element);
        this.observers.set(element, observer);
    },
    unobserve: function(element) {
        const observer = this.observers.get(element);
        if (observer) {
            observer.disconnect();
            this.observers.delete(element);
        }
    }
};
