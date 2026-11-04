// ES module for SfCard keyboard activation - named exports only (no globals).
// Blazor cannot cancel a key event conditionally, and its key events cannot tell whether the key was pressed on the
// card itself or on a control inside it. Both are handled here, and activation is forwarded as a native click so the
// card's single .NET click handler stays the only place that raises OnClick.

const KEY_DOWN = 'keydown';
const KEY_UP = 'keyup';
const BLUR = 'blur';
const ENTER = 'Enter';
const CLICKABLE_CLASS = 'e-card-clickable';

function isSpace(e) {
    return e.key === ' ' || e.code === 'Space';
}

class SfCardInstance {
    constructor(element) {
        this.element = element;
        this.spaceArmed = false;
        this.onKeyDown = this.onKeyDown.bind(this);
        this.onKeyUp = this.onKeyUp.bind(this);
        this.onBlur = this.onBlur.bind(this);
        element._blazorCardInstance = this;
    }

    initialize() {
        this.element.addEventListener(KEY_DOWN, this.onKeyDown);
        this.element.addEventListener(KEY_UP, this.onKeyUp);
        this.element.addEventListener(BLUR, this.onBlur);
    }

    // Only keys pressed while the card itself has focus activate it. Keys from a nested input, link or button are
    // left alone so they keep their own behaviour. Tab and every other key are never canceled, so focus is not trapped.
    isOwnKey(e) {
        return e.target === this.element && this.element.classList.contains(CLICKABLE_CLASS);
    }

    // Enter activates on key down; Space is armed here and activates on key up, as a native button does.
    onKeyDown(e) {
        if (!this.isOwnKey(e)) {
            return;
        }

        if (e.key === ENTER) {
            e.preventDefault();
            if (!e.repeat) {
                this.element.click();
            }
        } else if (isSpace(e)) {
            // Cancel the page scroll on every Space key down, including repeats.
            e.preventDefault();
            if (!e.repeat) {
                this.spaceArmed = true;
            }
        }
    }

    onKeyUp(e) {
        if (!isSpace(e) || !this.isOwnKey(e)) {
            return;
        }

        e.preventDefault();
        if (this.spaceArmed) {
            this.spaceArmed = false;
            this.element.click();
        }
    }

    onBlur() {
        this.spaceArmed = false;
    }

    destroy() {
        this.element.removeEventListener(KEY_DOWN, this.onKeyDown);
        this.element.removeEventListener(KEY_UP, this.onKeyUp);
        this.element.removeEventListener(BLUR, this.onBlur);
        delete this.element._blazorCardInstance;
    }
}

export function initialize(element) {
    if (!element || element._blazorCardInstance) {
        return;
    }

    new SfCardInstance(element).initialize();
}

export function destroy(element) {
    element?._blazorCardInstance?.destroy();
}
