/* eslint-disable no-undef */
// ES module for SfSwitch interop - named exports only (no globals)
// EventHandler available via global namespace from sf-toolkit-base
const EventHandler = window.sfBlazorToolkit?.base?.EventHandler;

const FOCUS = 'focus';
const BLUR = 'blur';
const KEY_UP = 'keyup';
const MOUSE_DOWN = 'mousedown';
const FOCUS_CLASS = 'e-focus';

class SfSwitchInstance {
    constructor(element, container) {
        this.element = element;
        this.container = container;
        // attach instance to element for deterministic cleanup
        element._blazorSwitchInstance = this;
    }

    initialize() {
        if (!this.element) return;
        EventHandler.add(this.element, FOCUS, this._focusHandler, this);
        EventHandler.add(this.element, BLUR, this._blurHandler, this);
        if (this.container) {
            EventHandler.add(this.container, KEY_UP, this._keyUpHandler, this);
            EventHandler.add(this.container, MOUSE_DOWN, this._mouseDownHandler, this);
        }
    }

    _focusHandler() {
        if (this.container) {
            this.container.classList.add(FOCUS_CLASS);
        }
    }

    _keyUpHandler(e) {
        if (this.container && (e.code === 'Space' || e.key === ' ')) {
            this.container.classList.remove(FOCUS_CLASS);
        }
    }

    _mouseDownHandler() {
        if (this.container) {
            this.container.classList.remove(FOCUS_CLASS);
        }
    }

    _blurHandler() {
        if (this.container) {
            this.container.classList.remove(FOCUS_CLASS);
        }
    }

    destroy() {
        if (!this.element) return;
        EventHandler.remove(this.element, FOCUS, this._focusHandler);
        EventHandler.remove(this.element, BLUR, this._blurHandler);
        if (this.container) {
            EventHandler.remove(this.container, KEY_UP, this._keyUpHandler);
            EventHandler.remove(this.container, MOUSE_DOWN, this._mouseDownHandler);
        }
        try {
            delete this.element._blazorSwitchInstance;
        }
        catch (e) {
            // ignore on IE where delete may throw in some cases
        }
    }
}

export function initialize(element, container) {
    if (!element) return;
    // create instance and initialize
    const instance = new SfSwitchInstance(element, container);
    instance.initialize();
}

export function destroy(element) {
    if (!element) return;
    const instance = element._blazorSwitchInstance;
    if (instance && typeof instance.destroy === 'function') {
        instance.destroy();
    }
}
