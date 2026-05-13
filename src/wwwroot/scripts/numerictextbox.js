
'use strict';

/**
 * Constants for key codes.
 * @constant
 */
var KEY_CODES = {
    ENTER: 13,
    ARROW_UP: 38,
    ARROW_DOWN: 40,
    BACK_SPACE: 8,
    LEFT_BUTTON: 0,
    RIGHT_BUTTON: 3,
    MOUSE_BUTTON: 2,
    MINUS: 45 // '-' character code
};

/**
 * Constants for event names.
 * @constant
 */
var EVENTS = {
    MOUSE_LEAVE: 'mouseleave',
    MOUSE_UP: 'mouseup',
    FOCUS: 'focus',
    BLUR: 'blur',
    KEY_PRESS: 'keypress',
    KEY_DOWN: 'keydown',
    DROP: 'drop',
    PASTE: 'paste',
    SCROLL: 'scroll',
    RESIZE: 'resize',
    TOUCH_START: 'touchstart',
    TOUCH_END: 'touchend',
    TOUCH_MOVE: 'touchmove'
};

/**
 * Constants for CSS classes.
 * @constant
 */
var CLASSES = {
    ROOT: 'e-input-group-icon',
    SPIN_UP: 'e-chevron-up',
    SPIN_DOWN: 'e-chevron-down',
    FLOAT_TEXT: 'e-float-text'
};

/**
 * Constants for internal actions.
 * @constant
 */
var ACTIONS = {
    INCREMENT: 'increment',
    DECREMENT: 'decrement',
    SERVER_ACTION: 'ServerActionAsync',
    FOCUS_HANDLER_KEY: 'UpdateFocusInputAsync',
    INVOKE_PASTE_HANDLER: 'InvokePasteHandlerAsync'
};

/**
 * Configuration constants.
 * @constant
 */
var CONFIG = {
    WHEEL_DELTA: 120,
    DELTA: 3,
    MOBILE_INTERVAL_TIME: 600,
    INTERVAL_TIME: 30,
    TIMEOUT: 150,
    IE_VERSION: '11.0'
};

var INT_REGEXP = new RegExp('^(-)?(\\d*)$');
var selectionTimeOut = 0;


/**
 * Blazor numeric textbox interop handler
 * @class
 * @param {string} dataId - Unique ID
 * @param {HTMLElement} wrapperElement - Wrapper DOM element
 * @param {HTMLInputElement} element - Input DOM element
 * @param {object} dotnetRef - .NET reference
 * @param {object} options - Component options
 */
var SfNumericTextBox = /** @class */ (function () {
    function SfNumericTextBox(dataId, wrapperElement, element, dotnetRef, options) {
        var _this = this;
        window.sfBlazorToolkit.base = window.sfBlazorToolkit.base;
        this.keyCharCodeHelper = function (event) {
            return _this.element.value.substring(0, _this.element.selectionStart) +
                String.fromCharCode(event.which) +
                _this.element.value.substring(_this.element.selectionEnd);
        };

        this.keyHelper = function (event) {
            return _this.element.value.substring(0, _this.element.selectionStart) + event.key +
                _this.element.value.substring(_this.element.selectionEnd);
        };

        this.dataId = dataId;
        this.wrapperElement = wrapperElement;
        this.element = element;
        this.options = options;
        this.dotNetRef = dotnetRef;

        window.sfBlazorToolkit.base.setCompInstance(this);
    }

    /**
     * Initializes the component events.
     */
    SfNumericTextBox.prototype.initialize = function () {
        this.spinButtonEvents();
        sfBlazorToolkit.base.EventHandler.add(this.element, EVENTS.FOCUS, this.focusHandler, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, EVENTS.BLUR, this.focusOutHandler, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, EVENTS.KEY_PRESS, this.keyPressHandler, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, EVENTS.KEY_DOWN, this.keyDownHandler, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, EVENTS.DROP, this.dropHandler, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, EVENTS.PASTE, this.pasteHandler, this);

        if (sfBlazorToolkit.base.isDevice().IsDevice) {
            sfBlazorToolkit.base.EventHandler.add(document, EVENTS.SCROLL, this.scrollHandler);
        }
        sfBlazorToolkit.base.EventHandler.add(window, EVENTS.RESIZE, this.updateFloatLabelSize, this);
        this.updateFloatLabelSize();
    };

    /**
     * Clears the invalid value.
     * @param {string} clearValue - Value to set
     */
    SfNumericTextBox.prototype.clearInvalid = function (clearValue) {
        this.element.value = clearValue;
    };

    /**
     * Handles key press events.
     * @param {KeyboardEvent} event 
     */
    SfNumericTextBox.prototype.keyPressHandler = function (event) {
        if (this.options.disabled || this.options.readonly) {
            return true;
        }
        var action = event.keyCode;
        if (event.which === KEY_CODES.LEFT_BUTTON || event.metaKey || event.ctrlKey || action === KEY_CODES.BACK_SPACE || action === KEY_CODES.ENTER) {
            return true;
        }

        var decimalSeparator = this.options.decimalSeparator;
        var currentChar = String.fromCharCode(event.which);
        var isAlterNumPadDecimalChar = event.code === 'NumpadDecimal' && this.keyCharCodeHelper(event) !== decimalSeparator;

        if (isAlterNumPadDecimalChar) {
            currentChar = decimalSeparator;
        }

        var text = this.element.value;
        text = text.substring(0, this.element.selectionStart) +
            currentChar + text.substring(this.element.selectionEnd);

        if (!this.numericRegex().test(text)) {
            event.preventDefault();
            event.stopPropagation();
            return false;
        } else {
            if (isAlterNumPadDecimalChar) {
                var start = this.element.selectionStart + 1;
                this.element.value = text;
                this.element.setSelectionRange(start, start);
                event.preventDefault();
                event.stopPropagation();
            }
            return true;
        }
    };

    /**
     * Handles drop events.
     * @param {DragEvent} event 
     */
    SfNumericTextBox.prototype.dropHandler = function (event) {
        if (!(this.options.disabled || this.options.readonly)) {
            var dropValue = event.dataTransfer.getData('text/plain').trim();
            if (!this.numericRegex().test(dropValue.trim())) {
                dropValue = dropValue.replace(/[^-0-9.,]/g, '');
                this.dotNetRef.invokeMethodAsync(ACTIONS.INVOKE_PASTE_HANDLER, dropValue, event.type);
            }
        }
    };

    /**
     * Handles paste events.
     * @param {ClipboardEvent} event 
     */
    SfNumericTextBox.prototype.pasteHandler = function (event) {
        if (!(this.options.disabled || this.options.readonly)) {
            var pasteValue = event.clipboardData.getData('text/plain');
            if (!this.numericRegex().test(pasteValue.trim())) {
                pasteValue = pasteValue.replace(/[^-0-9.,]/g, '');
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.element) && this.element.value === pasteValue) {
                    event.preventDefault();
                } else {
                    this.dotNetRef.invokeMethodAsync(ACTIONS.INVOKE_PASTE_HANDLER, pasteValue, event.type);
                }
            } else {
                this.dotNetRef.invokeMethodAsync(ACTIONS.INVOKE_PASTE_HANDLER, pasteValue, event.type);
            }
        }
    };

    /**
     * Logic to prevent invalid input on specific devices (iOS/Android).
     * Handles cursor positioning and character validation.
     */
    SfNumericTextBox.prototype.preventHandler = function () {
        var _this = this;
        var iOS = /ipad|iphone|ipod|mac/.test(navigator.userAgent.toLowerCase());

        setTimeout(function () {
            if (_this.element.selectionStart > 0) {
                var currentPos = _this.element.selectionStart;
                var prevPos = _this.element.selectionStart - 1;
                var start = 0;
                var valArray = _this.element.value.split('');
                var decimalSeparator = _this.options.decimalSeparator;
                var ignoreKeyCode = decimalSeparator.charCodeAt(0);
                var inputElement = _this.element;

                if (_this.element.value[prevPos] === ' ' && _this.element.selectionStart > 0 && !iOS) {
                    if (sfBlazorToolkit.base.isNullOrUndefined(_this.prevVal)) {
                        _this.element.value = _this.element.value.trim();
                    } else if (prevPos !== 0) {
                        _this.element.value = _this.prevVal;
                    } else if (prevPos === 0) {
                        _this.element.value = _this.element.value.trim();
                    }
                    _this.element.setSelectionRange(prevPos, prevPos);
                } else if (isNaN(parseFloat(inputElement.value[inputElement.selectionStart - 1])) &&
                    inputElement.value[inputElement.selectionStart - 1].charCodeAt(0) !== KEY_CODES.MINUS) {
                    if ((valArray.indexOf(inputElement.value[inputElement.selectionStart - 1]) !==
                        valArray.lastIndexOf(inputElement.value[inputElement.selectionStart - 1]) &&
                        inputElement.value[inputElement.selectionStart - 1].charCodeAt(0) === ignoreKeyCode) ||
                        inputElement.value[inputElement.selectionStart - 1].charCodeAt(0) !== ignoreKeyCode) {
                        inputElement.value = inputElement.value.substring(0, prevPos) +
                            inputElement.value.substring(currentPos, inputElement.value.length);
                        inputElement.setSelectionRange(prevPos, prevPos);
                        if (isNaN(parseFloat(inputElement.value[inputElement.selectionStart - 1])) && inputElement.selectionStart > 0
                            && inputElement.value.length) {
                            _this.preventHandler();
                        }
                    }
                } else if (isNaN(parseFloat(inputElement.value[inputElement.selectionStart - 2])) && inputElement.selectionStart > 1 &&
                    inputElement.value[inputElement.selectionStart - 2].charCodeAt(0) !== KEY_CODES.MINUS) {
                    if ((valArray.indexOf(inputElement.value[inputElement.selectionStart - 2]) !==
                        valArray.lastIndexOf(inputElement.value[inputElement.selectionStart - 2]) &&
                        inputElement.value[inputElement.selectionStart - 2].charCodeAt(0) === ignoreKeyCode) ||
                        inputElement.value[inputElement.selectionStart - 2].charCodeAt(0) !== ignoreKeyCode) {
                        inputElement.setSelectionRange(prevPos, prevPos);
                        _this.nextEle = inputElement.value[inputElement.selectionStart];
                        _this.cursorPosChanged = true;
                        _this.preventHandler();
                    }
                }

                if (_this.cursorPosChanged === true && inputElement.value[inputElement.selectionStart] === _this.nextEle &&
                    isNaN(parseFloat(inputElement.value[inputElement.selectionStart - 1]))) {
                    inputElement.setSelectionRange(inputElement.selectionStart + 1, inputElement.selectionStart + 1);
                    _this.cursorPosChanged = false;
                    _this.nextEle = null;
                }

                if (inputElement.value.trim() === '') {
                    inputElement.setSelectionRange(start, start);
                }

                if (inputElement.selectionStart > 0) {
                    if ((inputElement.value[inputElement.selectionStart - 1].charCodeAt(0) === KEY_CODES.MINUS) && inputElement.selectionStart > 1) {
                        if (sfBlazorToolkit.base.isNullOrUndefined(_this.prevVal)) {
                            // eslint-disable-next-line no-self-assign
                            inputElement.value = inputElement.value;
                        } else {
                            inputElement.value = _this.prevVal;
                        }
                        inputElement.setSelectionRange(inputElement.selectionStart, inputElement.selectionStart);
                    }
                }
                _this.prevVal = inputElement.value;
            }
        });
    };

    /**
     * Handles key down events.
     * @param {KeyboardEvent} event 
     */
    SfNumericTextBox.prototype.keyDownHandler = function (event) {
        var iOS = /ipad|iphone|ipod|mac/.test(navigator.userAgent.toLowerCase());
        if (!this.options.readonly) {
            if (sfBlazorToolkit.base.isDevice().IsDevice && !(event.keyCode === KEY_CODES.ARROW_UP || event.keyCode === KEY_CODES.ARROW_DOWN) && !(event.keyCode === KEY_CODES.BACK_SPACE)) {
                if (!(this.numericRegex().test(this.keyCharCodeHelper(event)) || this.numericRegex().test(this.keyHelper(event)))) {
                    if (iOS && sfBlazorToolkit.base.isDevice().IsDevice) {
                        this.preventHandler();
                    } else {
                        event.preventDefault();
                    }
                }
            }
            if (!iOS && sfBlazorToolkit.base.isDevice().IsDevice) {
                this.preventHandler();
            }
            if (event.keyCode === KEY_CODES.ARROW_UP) {
                event.preventDefault();
                this.dotNetRef.invokeMethodAsync(ACTIONS.SERVER_ACTION, ACTIONS.INCREMENT, event, this.element.value);
            } else if (event.keyCode === KEY_CODES.ARROW_DOWN) {
                event.preventDefault();
                this.dotNetRef.invokeMethodAsync(ACTIONS.SERVER_ACTION, ACTIONS.DECREMENT, event, this.element.value);
            }
        }
    };

    /**
     * Creates regex for numeric validation.
     * @returns {RegExp}
     */
    SfNumericTextBox.prototype.numericRegex = function () {
        var decimalSeparator = this.options.decimalSeparator;
        var fractionRule = '*';
        if (decimalSeparator === '.') {
            decimalSeparator = '\\' + decimalSeparator;
        }
        if (this.options.decimals === 0 && this.options.validateDecimalOnType) {
            return INT_REGEXP;
        }
        if (this.options.decimals && this.options.validateDecimalOnType) {
            fractionRule = '{0,' + this.options.decimals + '}';
        }
        // eslint-disable-next-line security/detect-non-literal-regexp
        return new RegExp('^(-)?(((\\d+(' + decimalSeparator + '\\d' + fractionRule +
            ')?)|(' + decimalSeparator + '\\d' + fractionRule + ')))?$');
    };

    /**
     * Handles focus events.
     * @param {FocusEvent} event 
     */
    SfNumericTextBox.prototype.focusHandler = function (event) {
        var _this = this;
        this.isFocused = true;
        /* eslint-disable */
        // @ts-ignore-start
        // tslint:disable-next-line:no-any
        this.dotNetRef.invokeMethodAsync(ACTIONS.FOCUS_HANDLER_KEY).then(function (args) {
            // @ts-ignore-end
            /* eslint-enable */
            if (args && args.isRendered) {
                _this.selectRange(args.formatValue);
            }
        });
    };

    /**
     * Handles blur/focus out events.
     * @param {FocusEvent} event 
     */
    SfNumericTextBox.prototype.focusOutHandler = function (event) {
        this.isFocused = false;
        event.preventDefault();
    };

    /**
     * Handles mouse down on spinner.
     * @param {MouseEvent} event 
     */
    SfNumericTextBox.prototype.mouseDownOnSpinner = function (event) {
        var _this = this;
        if (!this.options.disabled && !this.options.readonly) {
            if (this.isFocused) {
                this.isPrevFocused = true;
                if (event.cancelable) {
                    event.preventDefault();
                }
            }
            var target = event.currentTarget;
            var action_1 = (target.classList.contains(CLASSES.SPIN_UP)) ? ACTIONS.INCREMENT : ACTIONS.DECREMENT;
            sfBlazorToolkit.base.EventHandler.add(target, EVENTS.MOUSE_LEAVE, this.mouseUpClick, this);
            // tslint:disable
            this.timeOut = setInterval(function () {
                _this.isCalled = true;
                _this.dotNetRef.invokeMethodAsync(ACTIONS.SERVER_ACTION, action_1, event, _this.element.value);
            }, CONFIG.TIMEOUT);
            sfBlazorToolkit.base.EventHandler.add(document, EVENTS.MOUSE_UP, this.mouseUpClick, this);
        }
    };

    /**
     * Handles mouse up on spinner.
     * @param {MouseEvent} event 
     */
    SfNumericTextBox.prototype.mouseUpOnSpinner = function (event) {
        if (!this.options.disabled && !this.options.readonly) {
            if (this.isPrevFocused) {
                this.element.focus();
                if (!sfBlazorToolkit.base.isDevice().IsDevice && event.cancelable) {
                    this.isPrevFocused = false;
                }
            }
            if (!sfBlazorToolkit.base.isDevice().IsDevice) {
                event.preventDefault();
            }
            if (!this.getElementData(event)) {
                return;
            }
            var target = event.currentTarget;
            var action = (target.classList.contains(CLASSES.SPIN_UP)) ? ACTIONS.INCREMENT : ACTIONS.DECREMENT;
            sfBlazorToolkit.base.EventHandler.remove(target, EVENTS.MOUSE_LEAVE, this.mouseUpClick);
            if (!this.isCalled) {
                this.dotNetRef.invokeMethodAsync(ACTIONS.SERVER_ACTION, action, event, this.element.value);
            }
            this.isCalled = false;
            sfBlazorToolkit.base.EventHandler.remove(document, EVENTS.MOUSE_UP, this.mouseUpClick);
        }
    };

    /**
     * Handles touch move on spinner.
     * @param {TouchEvent} event 
     */
    SfNumericTextBox.prototype.touchMoveOnSpinner = function (event) {
        if (this.options.disabled || this.options.readonly) {
            var target = document.elementFromPoint(event.clientX, event.clientY);
            if (!(target.classList.contains(CLASSES.ROOT))) {
                clearInterval(this.timeOut);
            }
        } else {
            clearInterval(this.timeOut);
        }
    };

    SfNumericTextBox.prototype.scrollHandler = function (event) {
        clearInterval(this.timeOut);
    };

    SfNumericTextBox.prototype.getElementData = function (event) {
        if ((event.which && event.which === KEY_CODES.RIGHT_BUTTON) || (event.button && event.button === KEY_CODES.MOUSE_BUTTON)
            || this.options.disabled || this.options.readonly) {
            return false;
        }
        clearInterval(this.timeOut);
        return true;
    };

    SfNumericTextBox.prototype.mouseUpClick = function (event) {
        event.stopPropagation();
        clearInterval(this.timeOut);
        this.isCalled = false;
        sfBlazorToolkit.base.EventHandler.remove(this.spinUp, EVENTS.MOUSE_LEAVE, this.mouseUpClick);
        sfBlazorToolkit.base.EventHandler.remove(this.spinDown, EVENTS.MOUSE_LEAVE, this.mouseUpClick);
    };

    SfNumericTextBox.prototype.selectRange = function (formatValue) {
        var _this = this;
        clearTimeout(selectionTimeOut);
        if (!sfBlazorToolkit.base.isDevice().IsDevice && sfBlazorToolkit.base.Browser.info.version === CONFIG.IE_VERSION) {
            this.element.setSelectionRange(0, formatValue.length);
        } else {
            var delay = (sfBlazorToolkit.base.isDevice().IsDevice && sfBlazorToolkit.base.Browser.isIos) ? CONFIG.MOBILE_INTERVAL_TIME : CONFIG.INTERVAL_TIME;
            selectionTimeOut = setTimeout(function () {
                if (!sfBlazorToolkit.base.isNullOrUndefined(_this.element) && _this.element.type !== 'number') {
                    _this.element.setSelectionRange(0, formatValue.length);
                }
            }, delay);
        }
    };

    SfNumericTextBox.prototype.isDevice = function () {
        return sfBlazorToolkit.base.isDevice().IsDevice;
    };

    SfNumericTextBox.prototype.spinButtonEvents = function () {
        this.spinDown = this.wrapperElement ? this.wrapperElement.querySelector('.' + CLASSES.SPIN_DOWN) : null;
        this.spinUp = this.wrapperElement ? this.wrapperElement.querySelector('.' + CLASSES.SPIN_UP) : null;
        if (this.spinDown && this.spinUp) {
            this.unBindSpinButton();
            this.bindSpinButton();
        }
    };

    SfNumericTextBox.prototype.bindSpinButton = function () {
        sfBlazorToolkit.base.EventHandler.add(this.spinUp, sfBlazorToolkit.base.Browser.touchStartEvent, this.mouseDownOnSpinner, this);
        sfBlazorToolkit.base.EventHandler.add(this.spinDown, sfBlazorToolkit.base.Browser.touchStartEvent, this.mouseDownOnSpinner, this);
        sfBlazorToolkit.base.EventHandler.add(this.spinUp, sfBlazorToolkit.base.Browser.touchEndEvent, this.mouseUpOnSpinner, this);
        sfBlazorToolkit.base.EventHandler.add(this.spinDown, sfBlazorToolkit.base.Browser.touchEndEvent, this.mouseUpOnSpinner, this);
        sfBlazorToolkit.base.EventHandler.add(this.spinUp, sfBlazorToolkit.base.Browser.touchMoveEvent, this.touchMoveOnSpinner, this);
        sfBlazorToolkit.base.EventHandler.add(this.spinDown, sfBlazorToolkit.base.Browser.touchMoveEvent, this.touchMoveOnSpinner, this);
    };

    SfNumericTextBox.prototype.unBindSpinButton = function () {
        sfBlazorToolkit.base.EventHandler.remove(this.spinUp, sfBlazorToolkit.base.Browser.touchStartEvent, this.mouseDownOnSpinner);
        sfBlazorToolkit.base.EventHandler.remove(this.spinDown, sfBlazorToolkit.base.Browser.touchStartEvent, this.mouseDownOnSpinner);
        sfBlazorToolkit.base.EventHandler.remove(this.spinUp, sfBlazorToolkit.base.Browser.touchEndEvent, this.mouseUpOnSpinner);
        sfBlazorToolkit.base.EventHandler.remove(this.spinDown, sfBlazorToolkit.base.Browser.touchEndEvent, this.mouseUpOnSpinner);
        sfBlazorToolkit.base.EventHandler.remove(this.spinUp, sfBlazorToolkit.base.Browser.touchMoveEvent, this.touchMoveOnSpinner);
        sfBlazorToolkit.base.EventHandler.remove(this.spinDown, sfBlazorToolkit.base.Browser.touchMoveEvent, this.touchMoveOnSpinner);
    };

    SfNumericTextBox.prototype.destroy = function () {
        sfBlazorToolkit.base.EventHandler.remove(this.element, EVENTS.FOCUS, this.focusHandler);
        sfBlazorToolkit.base.EventHandler.remove(this.element, EVENTS.BLUR, this.focusOutHandler);
        sfBlazorToolkit.base.EventHandler.remove(this.element, EVENTS.KEY_PRESS, this.keyPressHandler);
        sfBlazorToolkit.base.EventHandler.remove(this.element, EVENTS.KEY_DOWN, this.keyDownHandler);
        sfBlazorToolkit.base.EventHandler.remove(this.element, EVENTS.DROP, this.dropHandler);
        sfBlazorToolkit.base.EventHandler.remove(this.element, EVENTS.PASTE, this.pasteHandler);
        if (sfBlazorToolkit.base.isDevice().IsDevice) {
            sfBlazorToolkit.base.EventHandler.remove(document, EVENTS.SCROLL, this.scrollHandler);
        }
        this.spinDown = null;
        this.spinUp = null;
        this.element = null;
        this.wrapperElement = null;
        this.options = null;
        this.keyHelper = null;
        this.keyCharCodeHelper = null;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        window.sfBlazorToolkit.base.disposeWindowsInstance(this.dataId);
    };

    SfNumericTextBox.prototype.getTextWidth = function (text) {
        var tempSpan = document.createElement('span');
        tempSpan.style.visibility = 'hidden';
        tempSpan.style.whiteSpace = 'nowrap';
        tempSpan.textContent = text;
        document.body.appendChild(tempSpan);
        var textWidth = tempSpan.offsetWidth;
        document.body.removeChild(tempSpan);
        return textWidth;
    };

    SfNumericTextBox.prototype.updateFloatLabelSize = function () {
        if (this.wrapperElement) {
            var floatLabel = this.wrapperElement.querySelector('.' + CLASSES.FLOAT_TEXT);
            if (!sfBlazorToolkit.base.isNullOrUndefined(floatLabel) && !sfBlazorToolkit.base.isNullOrUndefined(this.element) && this.element.clientWidth > 0) {
                if (this.element.clientWidth < this.getTextWidth(floatLabel.textContent)) {
                    floatLabel.style.width = this.element.clientWidth + 'px';
                } else {
                    floatLabel.style.width = '';
                }
            }
        }
    };

    SfNumericTextBox.prototype.removeFloatLabelSize = function () {
        if (this.wrapperElement) {
            var floatLabel = this.wrapperElement.querySelector('.' + CLASSES.FLOAT_TEXT);
            if (!sfBlazorToolkit.base.isNullOrUndefined(floatLabel)) {
                floatLabel.style.width = '';
            }
        }
    };
    return SfNumericTextBox;
}());

export function initialize(dataId, wrapperElement, element, dotnetRef, options) {
    if (element && dataId) {
        var instance = new SfNumericTextBox(dataId, wrapperElement, element, dotnetRef, options);
        instance.initialize();
    }
}
export function isFieldsetDisabled(element) {
    if (!element) return false;
    var fieldset = element.closest('fieldset');
    return !sfBlazorToolkit.base.isNullOrUndefined(fieldset) && fieldset.disabled;
}
export function selectRange(dataId, formatValue) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.selectRange(formatValue);
    }
}
export function propertyChanges(dataId, options) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.options = options;
    } 
}
export function focusOut(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.element.blur();
    }
}
export function focusHandler(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.focusHandler();
    }
}
export function spinButtonEvents(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.spinButtonEvents();
    }
}
export function clearInvalid(dataId, clearValue) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.clearInvalid === 'function') {
        instance.clearInvalid(clearValue);
    }
}
export function updateFloatLabelSize(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.updateFloatLabelSize();
    }
}
export function removeFloatLabelSize(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.removeFloatLabelSize();
    }
}
export function destroy(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.destroy();
    }
}