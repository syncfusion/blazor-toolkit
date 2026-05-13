/* eslint-disable no-undef */
// ES module for SfCheckBox interop - named exports only (no globals)
// EventHandler available via global namespace from sf-toolkit-base
const EventHandler = window.sfBlazorToolkit?.base?.EventHandler;

const FOCUS = 'focus';
const BLUR = 'blur';
const KEY_DOWN = 'keydown';
const MOUSE_DOWN = 'mousedown';
const FOCUS_CLASS = 'e-focus';

class SfCheckBoxInstance {
    constructor(element, container) {
        this.element = element;
        this.container = container;
        this.isMouseClick = false;
        // attach instance to element for deterministic cleanup
        element._blazorCheckBoxInstance = this;
    }

    initialize() {
        if (!this.element) return;
        EventHandler.add(this.element, FOCUS, this._focusHandler, this);
        EventHandler.add(this.element, BLUR, this._blurHandler, this);
        if (this.container) {
            EventHandler.add(this.container, MOUSE_DOWN, this._mouseDownHandler, this);
            EventHandler.add(this.container, KEY_DOWN, this._keyDownHandler, this);
        }
    }

    _focusHandler() {
        if (this.container && !this.isMouseClick) {
            this.container.classList.add(FOCUS_CLASS);
        }
        this.isMouseClick = false;
    }

    _mouseDownHandler() {
        this.isMouseClick = true;
    }

    _keyDownHandler(e) {
        if (this.container && (e.code === 'Space' || e.key === ' ')) {
            this.container.classList.add(FOCUS_CLASS);
        }
        this.isMouseClick = true;
    }

    _blurHandler() {
        if (this.container) {
            this.container.classList.remove(FOCUS_CLASS);
        }
        this.isMouseClick = false;
    }

    destroy() {
        if (!this.element) return;
        EventHandler.remove(this.element, FOCUS, this._focusHandler);
        EventHandler.remove(this.element, BLUR, this._blurHandler);
        if (this.container) {
            EventHandler.remove(this.container, MOUSE_DOWN, this._mouseDownHandler);
            EventHandler.remove(this.container, KEY_DOWN, this._keyDownHandler);
        }
        try {
            delete this.element._blazorCheckBoxInstance;
        }
        catch (e) {
            // ignore on IE where delete may throw in some cases
        }
    }
}

export function initialize(element, container) {
    if (!element) return;
    // create instance and initialize
    const instance = new SfCheckBoxInstance(element, container);
    instance.initialize();
}

export function destroy(element) {
    if (!element) return;
    const instance = element._blazorCheckBoxInstance;
    if (instance && typeof instance.destroy === 'function') {
        instance.destroy();
    }
}

