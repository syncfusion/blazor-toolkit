'use strict';

const EASING = {
    ease: 'cubic-bezier(0.250, 0.100, 0.250, 1.000)',
    linear: 'cubic-bezier(0.250, 0.250, 0.750, 0.750)',
    easeIn: 'cubic-bezier(0.420, 0.000, 1.000, 1.000)',
    easeOut: 'cubic-bezier(0.000, 0.000, 0.580, 1.000)',
    easeInOut: 'cubic-bezier(0.420, 0.000, 0.580, 1.000)',
    elasticInOut: 'cubic-bezier(0.5,-0.58,0.38,1.81)',
    elasticIn: 'cubic-bezier(0.17,0.67,0.59,1.81)',
    elasticOut: 'cubic-bezier(0.7,-0.75,0.99,1.01)'
};

/**
 * Animation class
 * @example
 * const anim = new Animation({ name: 'FadeIn', duration: 300 });
 * anim.animate(element);
 */
class Animation {

    constructor(options = {}) {
        this.name = options.name || 'FadeIn';
        this.duration = Number.isFinite(options.duration) ? options.duration : 400;
        this.timingFunction = options.timingFunction || 'ease';
        this.delay = Number.isFinite(options.delay) ? options.delay : 0;
        this.begin = typeof options.begin === 'function' ? options.begin : null;
        this.progress = typeof options.progress === 'function' ? options.progress : null;
        this.end = typeof options.end === 'function' ? options.end : null;
        this.fail = typeof options.fail === 'function' ? options.fail : null;
        this.easing = { ...EASING };
    }

    /**
     * Applies animation to the current element.
     *
     * @param {string | HTMLElement} element
     * @param {Object} options
     */
    animate(element, options = {}) {
        const model = this.getModel(options);
        if (typeof element === 'string') {
            const elements = Array.prototype.slice.call(document.querySelectorAll(element));
            for (let i = 0; i < elements.length; i++) {
                model.element = elements[i];
                Animation.delayAnimation(model);
            }
        }
        else {
            model.element = element;
            Animation.delayAnimation(model);
        }
    }

    /**
     * Stop the animation effect on animated element.
     *
     * @param {HTMLElement} element
     * @param {Object} model
     */
    static stop(element, model) {
        element.style.animation = '';
        element.removeAttribute('e-animate');
        const animationId = element.getAttribute('e-animation-id');
        if (animationId) {
            cancelAnimationFrame(parseInt(animationId, 10));
            element.removeAttribute('e-animation-id');
        }
        if (model && model.end) { model.end.call(this, model); }
    }

    /**
     * Set delay to animation element
     *
     * @param {Object} model
     */
    static delayAnimation(model) {
        if (window.exports?.animationMode === 'Disable') {
            if (model.begin) { model.begin.call(this, model); }
            if (model.end) { model.end.call(this, model); }
        }
        else {
            if (model.delay) { setTimeout(() => Animation.applyAnimation(model), model.delay); }
            else { Animation.applyAnimation(model); }
        }
    }

    /**
     * Triggers animation
     *
     * @param {Object} model
     */
    static applyAnimation(model) {
        let step = 0;
        let timerId = 0;
        let prevTimeStamp = 0;
        model.timeStamp = 0;
        const duration = model.duration;
        if (model && model.element) { model.element.setAttribute('e-animate', 'true'); }

        const startAnimation = (timeStamp) => {
            try {
                if (timeStamp) {
                    prevTimeStamp = prevTimeStamp === 0 ? timeStamp : prevTimeStamp;
                    model.timeStamp = (timeStamp + model.timeStamp) - prevTimeStamp;
                    prevTimeStamp = timeStamp;
                    if (!step && model.begin) { model.begin.call(this, model); }
                    step++;
                    const avg = model.timeStamp / step;
                    if (model.timeStamp < duration && model.timeStamp + avg < duration && model.element.getAttribute('e-animate')) {
                        model.element.style.animation = `${model.name} ${model.duration}ms ${model.timingFunction}`;
                        if (model.progress) { model.progress.call(this, model); }
                        timerId = requestAnimationFrame(startAnimation);
                        model.element.setAttribute('e-animation-id', timerId.toString());
                    } else {
                        cancelAnimationFrame(timerId);
                        model.element.removeAttribute('e-animation-id');
                        model.element.removeAttribute('e-animate');
                        model.element.style.animation = '';
                        if (model.end) { model.end.call(this, model); }
                    }
                } else {
                    timerId = requestAnimationFrame(startAnimation);
                    model.element.setAttribute('e-animation-id', timerId.toString());
                }
            } catch (e) {
                cancelAnimationFrame(timerId);
                model.element?.removeAttribute('e-animation-id');
                if (model && model.fail) {model.fail.call(this, e); }
            }
        };
        startAnimation();
    }

    /**
     * Returns Animation Model
     *
     * @param {Object} options
     */
    getModel(options = {}) {
        return {
            name: options.name || this.name,
            delay: options.delay || this.delay,
            duration: options.duration !== undefined ? options.duration : this.duration,
            begin: options.begin || this.begin,
            end: options.end || this.end,
            fail: options.fail || this.fail,
            progress: options.progress || this.progress,
            timingFunction: this.easing[options.timingFunction] ? this.easing[options.timingFunction] : options.timingFunction || this.easing[this.timingFunction]
        };
    }
}

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.Animation = Animation;

/**
 * Animates a calendar element using the toolkit's Animation utility.
 * @param {HTMLElement} elementRef - The element to animate.
 * @param {Object} animationSettings - Configuration for the animation.
 */
export function animate(elementRef, animationSettings) {
    const animationObj = new Animation(animationSettings);
    animationObj.animate(elementRef);
}