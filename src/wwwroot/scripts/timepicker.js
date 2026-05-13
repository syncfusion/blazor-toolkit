'use strict';

var ROOT = 'e-timepicker';
var POPUP = 'e-popup';
var POPUP_DIMENSION = '240px';
var POPUP_WRAPPER = 'e-popup-wrapper';
var OVERFLOW = 'e-time-overflow';
var LIST_CLASS = 'e-list-item';
var SELECTED = 'e-active';
var HOVER = 'e-hover';
var NAVIGATION = 'e-navigation';
var POPUP_CONTENT = 'e-content';
var MODEL_POPUP = 'e-timepicker-mob-popup-wrap';
var ARIA_SELECT = 'aria-selected';
var HIDE_POPUP = 'HidePopup';
var TIME_MODAL = 'e-time-modal';
var RIGHT_ACTION = 'right';
var LEFT_ACTION = 'left';
var TAB_ACTION = 'tab';
var HOME_ACTION = 'home';
var END_ACTION = 'end';
var UP_ACTION = 'up';
var DOWN_ACTION = 'down';
var ENTER_ACTION = 'enter';
var MOUSE_OVER = 'mouseover';
var MOUSE_OUT = 'mouseout';
var CLOSE_POPUP = 'ClosePopupAsync';
var OFFSET_VALUE = 4;
var OPEN_DURATION = 300;
var HALF_POSITION = 2;
var ANIMATION_DURATION = 50;
var DAY = new Date().getDate();
var MONTH = new Date().getMonth();
var YEAR = new Date().getFullYear();
var SfTimePicker = /** @class */ (function () {
    /**
     * Constructor for SfTimePicker class
     * @param {string} dataId - Unique identifier for the component instance
     * @param {HTMLElement} containerElement - The container element
     * @param {HTMLElement} element - The input element
     * @param {object} dotnetRef - Reference to .NET interop object
     * @param {object} options - Configuration options
     */
    function SfTimePicker(dataId, containerElement, element, dotnetRef, options) {
        window.sfBlazorToolkit = window.sfBlazorToolkit;
        this.componentName = 'TimePicker';
        this.dataId = dataId;
        this.containerElement = containerElement;
        this.element = element;
        this.options = options;
        window.sfBlazorToolkit.base.setCompInstance(this);
        this.dotNetRef = dotnetRef;
        this.enableMask = this.options.enableMask;
        if (this.enableMask) {
            this.maskObj = new sfBlazorToolkit.MaskedDateTime();
            this.maskObj.element = this.element;
            this.maskObj.dateformat = this.options.format;
            this.maskObj.isRendered = this.options.isRendered;
            this.maskObj.isPopupOpen = this.isPopupOpen;
            this.maskObj.componentName = this.componentName;
            this.maskObj.offset = this.options.offset;
            this.maskObj.floatLabelType = this.options.floatLabelType;
            this.maskObj.maskPlaceholderDictionary = this.options.maskPlaceholderDictionary;
            this.maskObj.dayAbbreviatedName = this.options.dayAbbreviatedName;
            this.maskObj.dayName = this.options.dayName;
            this.maskObj.dayPeriod = this.options.dayPeriod;
            this.maskObj.monthAbbreviatedName = this.options.monthAbbreviatedName;
            this.maskObj.monthName = this.options.monthName;
            this.maskObj.maskDateValue = !sfBlazorToolkit.base.isNullOrUndefined(this.options.value) ? new Date(this.options.value.toString()) : new Date();
            this.maskObj.maskDateValue.setMonth(0);
            this.maskObj.maskDateValue.setHours(0);
            this.maskObj.maskDateValue.setMinutes(0);
            this.maskObj.maskDateValue.setSeconds(0);
            this.maskObj.previousDate = new Date(this.maskObj.maskDateValue.getFullYear(), this.maskObj.maskDateValue.getMonth(), this.maskObj.maskDateValue.getDate(), this.maskObj.maskDateValue.getHours(), this.maskObj.maskDateValue.getMinutes(), this.maskObj.maskDateValue.getSeconds());
            this.removeEventListener();
            this.addEventListener();
        }
    }
        /**
         * Initializes the TimePicker component with keyboard configurations
         * @returns {void}
         */
        SfTimePicker.prototype.initialize = function () {
            sfBlazorToolkit.base.EventHandler.add(this.containerElement, 'keydown', this.inputHandler, this);
            sfBlazorToolkit.base.EventHandler.add(window, 'resize', this.updateFloatLabelSize, this);
            this.updateFloatLabelSize();
        };
        /**
         * Binds or unbinds input event handlers based on mask enablement
         * @returns {void}
         */
        SfTimePicker.prototype.bindInputEvent = function () {
            if (this.enableMask) {
                sfBlazorToolkit.base.EventHandler.add(this.element, 'input', this.inputkeyHandler, this);
            }
            else {
                sfBlazorToolkit.base.EventHandler.remove(this.element, 'input', this.inputkeyHandler);
            }
        };
        /**
         * Adds event listeners for mouse and keyboard events
         * @returns {void}
         */
        SfTimePicker.prototype.addEventListener = function () {
            sfBlazorToolkit.base.EventHandler.add(this.element, 'mouseup', this.mouseUpHandler, this);
            sfBlazorToolkit.base.EventHandler.add(this.element, 'keydown', this.keydownHandler, this);
            this.bindInputEvent();
        };
        /**
         * Removes event listeners from the input element
         * @returns {void}
         */
        SfTimePicker.prototype.removeEventListener = function () {
            sfBlazorToolkit.base.EventHandler.remove(this.element, 'mouseup', this.mouseUpHandler);
            sfBlazorToolkit.base.EventHandler.remove(this.element, 'keydown', this.keydownHandler);
        };
        /**
         * Handles mouse up events for masked input
         * @param {MouseEvent} e - The mouse event
         * @returns {void}
         */
        SfTimePicker.prototype.mouseUpHandler = function (e) {
            if (this.enableMask) {
                this.maskObj.element = this.element;
                e.preventDefault();
                this.maskObj.validCharacterCheck();
            }
        };
        /**
         * Handles input key events for masked input
         * @returns {void}
         */
        SfTimePicker.prototype.inputkeyHandler = function () {
            if (this.enableMask) {
                this.maskObj.element = this.element;
                this.maskObj.maskInputHandler();
            }
        };
        /**
         * Handles keydown events for masked input
         * @param {KeyboardEvent} e - The keyboard event
         * @returns {void}
         */
        SfTimePicker.prototype.keydownHandler = function (e) {
            if (this.enableMask) {
                this.maskObj.element = this.element;
                this.maskObj.keydownHandler(e);
            }
        };
        /**
         * Handles mouse over events for list items
         * @param {MouseEvent} e - The mouse event
         * @returns {void}
         */
        SfTimePicker.prototype.mouseOverHandler = function (e) {
            if (e.target) {
                e.target.classList.add(HOVER);
            }
        };
        /**
         * Handles mouse out events for list items
         * @param {MouseEvent} e - The mouse event
         * @returns {void}
         */
        SfTimePicker.prototype.mouseOutHandler = function (e) {
            if (e.target && e.target.classList.contains(HOVER)) {
                e.target.classList.remove(HOVER);
            }
        };
        /**
         * Renders and displays the TimePicker popup with time list
         * @param {HTMLElement} popupElement - The popup element
         * @param {HTMLElement} popupHolderEle - The popup holder element
         * @param {object} openEventArgs - Open event arguments
         * @param {object} options - Configuration options
         * @returns {void}
         */
        SfTimePicker.prototype.renderPopup = function (popupElement, popupHolderEle, openEventArgs, options) {
            this.options = options;
            if (this.enableMask) this.maskObj.isPopupOpen = true;
            this.popupHolder = popupHolderEle;
            this.timeCollections = [];
            this.listWrapper = popupHolderEle.querySelector('.' + POPUP_CONTENT) || document.querySelector('.' + POPUP_CONTENT);
            this.getTimeCollection();
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.element.value)) {
                this.removeSelection();
                this.selectedElement = this.listWrapper.querySelector('li[data-value = "' + this.element.value + '"]');
                this.updateSelection(this.selectedElement);
            }
            this.popupCreation(popupElement, options);
            if (sfBlazorToolkit.base.Browser.isDevice) {
                this.mobilePopupWrapper = sfBlazorToolkit.base.createElement('div', { className: MODEL_POPUP });
                document.body.appendChild(this.mobilePopupWrapper);
            }
            var appendToElement = openEventArgs.appendTo === 'model' ? this.mobilePopupWrapper : document.body;
            appendToElement.appendChild(this.popupWrapper);
            if (this.popupWrapper) {
                var popupListItem = this.popupWrapper.querySelectorAll('.' + LIST_CLASS);
                if (popupListItem && popupListItem.length > 0) {
                    for (var i = 0; i < popupListItem.length; i++) {
                        sfBlazorToolkit.base.EventHandler.add(popupListItem[i], MOUSE_OVER, this.mouseOverHandler, this);
                        sfBlazorToolkit.base.EventHandler.add(popupListItem[i], MOUSE_OUT, this.mouseOutHandler, this);
                    }
                }
            }
            this.setScrollPosition();
            this.popupObj.refreshPosition(this.element);
            var openAnimation = {
                name: 'FadeIn',
                duration: sfBlazorToolkit.base.Browser.isDevice ? 0 : OPEN_DURATION
            };
            if (this.options.zIndex === 1000) {
                this.popupObj.show(new sfBlazorToolkit.Animation(openAnimation), this.element);
            }
            else {
                this.popupObj.show(new sfBlazorToolkit.Animation(openAnimation), null);
            }
            this.setOverlayIndex(this.mobilePopupWrapper, this.popupObj.element, this.modal, sfBlazorToolkit.base.Browser.isDevice);
            sfBlazorToolkit.base.EventHandler.add(document, 'mousedown touchstart', this.documentClickHandler, this);
        };
        /**
         * Collects all time values from list items
         * @returns {void}
         */
        SfTimePicker.prototype.getTimeCollection = function () {
            var liCollections = this.listWrapper.querySelectorAll('.' + LIST_CLASS);
            for (var index = 0; index < liCollections.length; index++) {
                this.timeCollections.push(liCollections[index].getAttribute('data-value'));
            }
        };
        /**
         * Updates selection styling for a list element
         * @param {HTMLElement} selectElement - The element to select
         * @returns {void}
         */
        SfTimePicker.prototype.updateSelection = function (selectElement) {
            if (selectElement) {
                sfBlazorToolkit.base.addClass(selectElement, SELECTED);
                selectElement.setAttribute(ARIA_SELECT, 'true');
            }
        };
        /**
         * Sets the scroll position to show selected or scrollTo time
         * @returns {void}
         */
        SfTimePicker.prototype.setScrollPosition = function () {
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.selectedElement)) {
                this.findScrollTop(this.selectedElement);
            }
            else if (this.popupWrapper && this.options.scrollTo && this.checkDateValue(new Date(this.options.scrollTo))) {
                this.setScrollTo();
            }
        };
        /**
         * Validates if value is a valid Date object
         * @param {Date} value - The value to check
         * @returns {Date|null} The date or null if invalid
         */
        SfTimePicker.prototype.checkDateValue = function (value) {
            return (!sfBlazorToolkit.base.isNullOrUndefined(value) && value instanceof Date && !isNaN(+value)) ? value : null;
        };
        /**
         * Calculates and applies scroll position for an element
         * @param {HTMLElement} element - The element to scroll to
         * @returns {void}
         */
        SfTimePicker.prototype.findScrollTop = function (element) {
            var listHeight = this.getPopupHeight();
            var nextEle = element.nextElementSibling;
            var height = nextEle ? nextEle.offsetTop : element.offsetTop;
            var liHeight = element.getBoundingClientRect().height;
            if ((height + element.offsetTop) > listHeight) {
                this.popupWrapper.scrollTop = nextEle ? (height - (listHeight / HALF_POSITION + liHeight / HALF_POSITION)) : height;
            }
            else {
                this.popupWrapper.scrollTop = 0;
            }
        };
        /**
         * Sets scroll position based on scrollTo option
         * @returns {void}
         */
        SfTimePicker.prototype.setScrollTo = function () {
            var element;
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.popupWrapper)) {
                var items = this.popupWrapper.querySelectorAll('.' + LIST_CLASS);
                if (items.length) {
                    var initialTime = !sfBlazorToolkit.base.isNullOrUndefined(this.options.value) && typeof (this.options.value) === 'string' ? this.getDateObject(new Date(new Date().toDateString() + ' ' + this.timeCollections[0])).setMilliseconds(0) : new Date(new Date().toDateString() + ' ' + this.timeCollections[0]).setMilliseconds(0);
                    var scrollTime = this.getDateObject(new Date(this.options.scrollTo)).getTime();
                    element = items[Math.round((scrollTime - initialTime) / (this.options.step * 60000))];
                }
            }
            else {
                this.popupWrapper.scrollTop = 0;
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
                this.findScrollTop(element);
            }
            else {
                this.popupWrapper.scrollTop = 0;
            }
        };
        /**
         * Converts text to Date object with current date and specified time
         * @param {Date|string} text - The date/time value
         * @returns {Date|null} The date object or null
         */
        SfTimePicker.prototype.getDateObject = function (text) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(text)) {
                var dateValue = text;
                var value = !sfBlazorToolkit.base.isNullOrUndefined(this.options.value);
                if (this.checkDateValue(dateValue)) {
                    var date = value ? new Date(this.options.value).getDate() : DAY;
                    var month = value ? new Date(this.options.value).getMonth() : MONTH;
                    var year = value ? new Date(this.options.value).getFullYear() : YEAR;
                    return new Date(year, month, date, dateValue.getHours(), dateValue.getMinutes(), dateValue.getSeconds());
                }
            }
            return null;
        };
        /**
         * Calculates the appropriate popup height
         * @returns {number} The popup height in pixels
         */
        SfTimePicker.prototype.getPopupHeight = function () {
            var height = parseInt(POPUP_DIMENSION, 10);
            var popupHeight = this.popupWrapper.getBoundingClientRect().height;
            return popupHeight > height ? height : popupHeight;
        };
        /**
         * Creates the popup component with specified configuration
         * @param {HTMLElement} popupElement - The popup element
         * @param {object} options - Configuration options
         * @returns {void}
         */
        SfTimePicker.prototype.popupCreation = function (popupElement, options) {
            var _this = this;
            this.popupWrapper = popupElement;
            this.containerStyle = this.containerElement.getBoundingClientRect();
            if (sfBlazorToolkit.base.Browser.isDevice) {
                this.modal = sfBlazorToolkit.base.createElement('div', { className: '' + ROOT + ' ' + TIME_MODAL });
                document.body.className += ' ' + OVERFLOW;
                this.modal.style.display = 'block';
                document.body.appendChild(this.modal);
            }
            this.popupObj = new sfBlazorToolkit.popups.Popup(this.popupWrapper, {
                width: this.setPopupWidth(this.options.width),
                relateTo: sfBlazorToolkit.base.Browser.isDevice ? document.body : this.containerElement,
                position: sfBlazorToolkit.base.Browser.isDevice ? { X: 'center', Y: 'center' } : { X: 'left', Y: 'bottom' },
                collision: sfBlazorToolkit.base.Browser.isDevice ? { X: 'fit', Y: 'fit' } : { X: 'flip', Y: 'flip' },
                offsetY: OFFSET_VALUE,
                targetType: 'relative',
                enableRtl: options.enableRtl,
                zIndex: options.zIndex,
                open: function () {
                    _this.popupWrapper.style.visibility = 'visible';
                }, close: function () {
                    _this.popupHolder.appendChild(_this.popupWrapper);
                    if (_this.popupObj) {
                        _this.popupObj.destroy();
                    }
                    if (!_this.isDisposed) {
                        // eslint-disable-next-line @typescript-eslint/no-empty-function
                        _this.dotNetRef.invokeMethodAsync(CLOSE_POPUP).catch(function () { });
                    }
                    _this.popupObj = null;
                }, targetExitViewport: function () {
                    if (!sfBlazorToolkit.base.Browser.isDevice && !_this.isDisposed) {
                        _this.dotNetRef.invokeMethodAsync(HIDE_POPUP, null);
                    }
                }
            });
            if (!sfBlazorToolkit.base.Browser.isDevice) {
                this.popupObj.collision = { X: 'none', Y: 'flip' };
            }
            if (this.popupWrapper.classList.contains('e-popup-expand')) {
                this.popupObj.element.style.maxHeight = '100%';
                this.popupObj.element.style.width = '100%';
            }
            else {
                this.popupObj.element.style.maxHeight = POPUP_DIMENSION;
            }
        };
        /**
         * Closes the TimePicker popup
         * @param {object} closeEventArgs - Close event arguments
         * @param {object} options - Configuration options
         * @returns {void}
         */
        SfTimePicker.prototype.closePopup = function (closeEventArgs, options) {
            this.options = options;
            if (this.enableMask) this.maskObj.isPopupOpen = false;
            sfBlazorToolkit.base.removeClass(document.body, OVERFLOW);
            this.closeEventCallback(closeEventArgs);
        };
        /**
         * Removes selection styling from all list items
         * @returns {void}
         */
        SfTimePicker.prototype.removeSelection = function () {
            this.removeHover(HOVER);
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.popupWrapper)) {
                var items = this.popupWrapper.querySelectorAll('.' + SELECTED);
                if (items.length) {
                    sfBlazorToolkit.base.removeClass(items, SELECTED);
                    items[0].removeAttribute(ARIA_SELECT);
                }
            }
        };
        /**
         * Removes hover or navigation class from list items
         * @param {string} className - The class name to remove
         * @returns {void}
         */
        SfTimePicker.prototype.removeHover = function (className) {
            var hoveredItem = this.popupWrapper ?
                this.popupWrapper.querySelectorAll('.' + className) : [];
            if (hoveredItem && hoveredItem.length) {
                sfBlazorToolkit.base.removeClass(hoveredItem, className);
                if (className === NAVIGATION) {
                    hoveredItem[0].removeAttribute(ARIA_SELECT);
                }
            }
        };
        /**
         * Normalizes width value to valid CSS width
         * @param {string|number} width - The width value
         * @returns {string} The normalized width string
         */
        SfTimePicker.prototype.setWidth = function (width) {
            if (typeof width === 'number' || typeof width === 'string') {
                width = width;
            }
            else {
                width = '100%';
            }
            return width;
        };
        /**
         * Calculates popup width based on container width and options
         * @param {string|number} width - The width value
         * @returns {string} The calculated popup width
         */
        SfTimePicker.prototype.setPopupWidth = function (width) {
            width = this.setWidth(width);
            if (width.indexOf('%') > -1) {
                var inputWidth = this.containerStyle.width * parseFloat(width) / 100;
                width = inputWidth.toString() + 'px';
            }
            return width;
        };
        /**
         * Callback handler for popup close event
         * @param {object} eventArgs - Event arguments
         * @returns {void}
         */
        SfTimePicker.prototype.closeEventCallback = function (eventArgs) {
            var preventArgs = eventArgs;
            if (this.isPopupOpen() && !preventArgs.cancel && this.popupObj) {
                var animModel = {
                    name: 'FadeOut',
                    duration: ANIMATION_DURATION,
                    delay: 0
                };
                if (this.popupWrapper) {
                    var popupListItem = this.popupWrapper.querySelectorAll('.' + LIST_CLASS);
                    if (popupListItem && popupListItem.length > 0) {
                        for (var i = 0; i < popupListItem.length; i++) {
                            sfBlazorToolkit.base.EventHandler.remove(popupListItem[i], MOUSE_OVER, this.mouseOverHandler);
                            sfBlazorToolkit.base.EventHandler.remove(popupListItem[i], MOUSE_OUT, this.mouseOutHandler);
                        }
                    }
                }
                this.popupObj.hide(new sfBlazorToolkit.Animation(animModel));
            }
            if (sfBlazorToolkit.base.Browser.isDevice) {
                if (this.modal) {
                    this.modal.style.display = 'none';
                    this.modal.outerHTML = '';
                    this.modal = null;
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.mobilePopupWrapper)) {
                    this.mobilePopupWrapper.remove();
                    this.mobilePopupWrapper = null;
                }
            }
            sfBlazorToolkit.base.EventHandler.remove(document, 'mousedown touchstart', this.documentClickHandler);
        };
        /**
         * Checks if popup is currently open
         * @returns {boolean} True if popup is open
         */
        SfTimePicker.prototype.isPopupOpen = function () {
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.popupObj) && this.popupObj.element.classList.contains(POPUP)) {
                return true;
            }
            return false;
        };
        /**
         * Handles document click events to close popup on outside click
         * @param {MouseEvent|TouchEvent} event - The event object
         * @returns {void}
         */
        SfTimePicker.prototype.documentClickHandler = function (event) {
            var target = event.target;
            if ((!sfBlazorToolkit.base.isNullOrUndefined(this.popupObj) && (this.containerElement.contains(target) ||
                (this.popupObj.element && this.popupObj.element.contains(target)))) && event.type !== 'touchstart') {
                event.preventDefault();
            }
            var clearElement = this.containerElement.querySelector('.e-close');
            var timeIconElement = this.containerElement.querySelector('.e-clock.e-toolkit-icons');
            if (!(sfBlazorToolkit.base.closest(target, '.' + POPUP_WRAPPER)) && target !== this.element
                && target !== timeIconElement && target !== clearElement && target !== this.containerElement) {
                if (this.isPopupOpen() && !this.isDisposed) {
                    this.dotNetRef.invokeMethodAsync(HIDE_POPUP, null);
                }
            }
        };
        /**
         * Sets z-index for modal overlay on mobile devices
         * @param {HTMLElement} popupWrapper - The popup wrapper element
         * @param {HTMLElement} timePopupElement - The time popup element
         * @param {HTMLElement} modal - The modal element
         * @param {boolean} isDevice - Whether on mobile device
         * @returns {void}
         */
        SfTimePicker.prototype.setOverlayIndex = function (popupWrapper, timePopupElement, modal, isDevice) {
            if (isDevice && !sfBlazorToolkit.base.isNullOrUndefined(timePopupElement) && !sfBlazorToolkit.base.isNullOrUndefined(modal) && !sfBlazorToolkit.base.isNullOrUndefined(popupWrapper)) {
                var index = parseInt(timePopupElement.style.zIndex, 10) ? parseInt(timePopupElement.style.zIndex, 10) : 1000;
                modal.style.zIndex = (index - 1).toString();
                popupWrapper.style.zIndex = index.toString();
            }
        };
        /**
         * Selects input text and updates scroll position for navigation
         * @param {HTMLElement} element - The input element
         * @param {boolean} isNavigation - Whether this is from navigation
         * @param {number} index - The index of item to scroll to
         * @returns {void}
         */
        SfTimePicker.prototype.selectInputText = function (element, isNavigation, index) {
            if (!sfBlazorToolkit.base.Browser.isDevice && (!this.enableMask || this.maskObj.isPopupOpen)) {
                if (isNavigation && this.listWrapper) {
                    this.selectedElement = this.listWrapper.querySelectorAll('.' + LIST_CLASS)[index];
                    this.setScrollPosition();
                }
            }
        };
        /**
         * Checks if running on a mobile device
         * @returns {boolean} True if mobile device
         */
        SfTimePicker.prototype.isDevice = function () {
            return sfBlazorToolkit.base.Browser.isDevice;
        };
        /**
         * Handles keyboard input events and invokes .NET handler
         * @param {KeyboardEvent} event - The keyboard event
         * @returns {void}
         */
        SfTimePicker.prototype.inputHandler = function (event) {
            var inputElement = this.element;
            if (!((event.action === RIGHT_ACTION || event.action === LEFT_ACTION || event.action === TAB_ACTION) ||
                ((event.action === HOME_ACTION || event.action === END_ACTION || event.action === UP_ACTION || event.action === DOWN_ACTION) &&
                    !this.isPopupOpen() && !this.enableMask))) {
                event.preventDefault();
            }
            if (event.action === ENTER_ACTION && this.isPopupOpen()) {
                event.stopPropagation();
            }
            var eventArgs = {
                Action: event.action,
                Key: event.key,
                KeyCode: event.keyCode,
                Events: event,
                SelectDate: null,
                FocusedDate: null,
                classList: '',
                Id: null,
                TargetClassList: null
            };
            if (this.enableMask && this.maskObj.isPopupOpen) {
                inputElement.value = null;
            }
            if (!this.isDisposed) {
                this.dotNetRef.invokeMethodAsync('KeyboardHandlerAsync', eventArgs, inputElement.value);
            }
            if ((event.action === 'enter' && !sfBlazorToolkit.base.isNullOrUndefined(this.popupObj) && document.body.contains(this.popupObj.element))) {
                event.preventDefault();
            }
        };
        /**
         * Calculates the rendered width of text
         * @param {string} text - The text to measure
         * @returns {number} The width in pixels
         */
        SfTimePicker.prototype.getTextWidth = function (text) {
            var tempSpan = sfBlazorToolkit.base.createElement('span', {
                styles: {
                    'visibility': 'hidden', 'whiteSpace': 'nowrap'
                }
            });
            tempSpan.textContent = text;
            document.body.appendChild(tempSpan);
            var textWidth = tempSpan.offsetWidth;
            document.body.removeChild(tempSpan);
            return textWidth;
        };
        /**
         * Updates float label width to fit within input
         * @returns {void}
         */
        SfTimePicker.prototype.updateFloatLabelSize = function () {
            if (this.containerElement) {
                var floatLabel = this.containerElement.querySelector('.e-float-text');
                if (!sfBlazorToolkit.base.isNullOrUndefined(floatLabel) && !sfBlazorToolkit.base.isNullOrUndefined(this.element) && this.element.clientWidth > 0) {
                    if (this.element.clientWidth < this.getTextWidth(floatLabel.textContent)) {
                        floatLabel.style.width = this.element.clientWidth + 'px';
                    }
                    else {
                        floatLabel.style.width = '';
                    }
                }
            }
        };
        /**
         * Destroys the TimePicker component and cleans up resources
         * @returns {void}
         */
        SfTimePicker.prototype.destroy = function () {
            this.maskObj = null;
            this.element = null;
            this.containerElement = null;
            this.keyConfigure = null;
            this.listWrapper = null;
            this.popupWrapper = null;
            this.popupHolder = null;
            this.popupObj.relateTo = null;
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            window.sfBlazorToolkit.base.disposeWindowsInstance(this.dataId);
        };
        /**
         * Removes custom width from float label
         * @returns {void}
         */
        SfTimePicker.prototype.removeFloatLabelSize = function () {
            if (this.containerElement) {
                var floatLabel = this.containerElement.querySelector('.e-float-text');
                if (!sfBlazorToolkit.base.isNullOrUndefined(floatLabel)) {
                    floatLabel.style.width = '';
                }
            }
        };
        return SfTimePicker;
    }());

/**
 * Initializes a new TimePicker instance
 * @param {string} dataId - Unique identifier
 * @param {HTMLElement} containerElement - Container element
 * @param {HTMLElement} element - Input element
 * @param {object} dotnetRef - .NET interop reference
 * @param {object} options - Configuration options
 * @returns {object} Mask format and input value
 */
export function initialize(dataId, containerElement, element, dotnetRef, options) {
    if (element) {
        var instance = new SfTimePicker(dataId, containerElement, element, dotnetRef, options);
        instance.initialize();
        if (!sfBlazorToolkit.base.isNullOrUndefined(sfBlazorToolkit.base.closest(element, 'fieldset')) && sfBlazorToolkit.base.closest(element, 'fieldset').disabled) {
            var disabled = options.disabled = true;
            instance.dotNetRef.invokeMethodAsync('UpdateFieldSetStatus', disabled);
        }
        if (options.enableMask) {
            return instance.maskObj.createMask(options);
        }
    }
    return { 'currentMaskFormat': null, 'inputElementValue': options.value };
}

/**
 * Creates mask for existing TimePicker instance
 * @param {string} dataId - Component instance identifier
 * @param {object} options - Configuration options
 * @returns {object} Mask format and input value
 */
export function createMask(dataId, options) {
    if (dataId && options.enableMask) {
        var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
        return instance.maskObj.createMask(options);
    }
    return { 'currentMaskFormat': null, 'inputElementValue': options.value };
}

/**
 * Updates the current value of the masked input
 * @param {string} dataId - Component instance identifier
 * @param {string} currentValueAsString - The new value
 * @returns {void}
 */
export function updateCurrentValue(dataId, currentValueAsString) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    instance.maskObj.updateCurrentValue(currentValueAsString);
}

/**
 * Renders the popup for TimePicker instance
 * @param {string} dataId - Component instance identifier
 * @param {HTMLElement} popupElement - Popup element
 * @param {HTMLElement} popupHolderEle - Popup holder
 * @param {object} openEventArgs - Open event arguments
 * @param {object} options - Configuration options
 * @returns {void}
 */
export function renderPopup(dataId, popupElement, popupHolderEle, openEventArgs, options) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && popupElement && popupHolderEle) {
        instance.renderPopup(popupElement, popupHolderEle, openEventArgs, options);
    }
}

/**
 * Closes the popup for TimePicker instance
 * @param {string} dataId - Component instance identifier
 * @param {object} closeEventArgs - Close event arguments
 * @param {object} options - Configuration options
 * @returns {void}
 */
export function closePopup(dataId, closeEventArgs, options) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && closeEventArgs) {
        instance.closePopup(closeEventArgs, options);
    }
}

/**
 * Selects input text based on navigation
 * @param {string} dataId - Component instance identifier
 * @param {boolean} isNavigation - Whether from navigation
 * @param {number} index - Item index
 * @returns {void}
 */
export function selectInputText(dataId, isNavigation, index) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.selectInputText(instance.element, isNavigation, index);
    }
}

/**
 * Sets focus to the input element
 * @param {string} dataId - Component instance identifier
 * @param {boolean} isReadOnly - Whether input is readonly
 * @returns {void}
 */
export function focusIn(dataId, isReadOnly) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        if (isReadOnly) {
            instance.element.setAttribute('readonly', 'true');
        }
        instance.element.focus();
    }
}

/**
 * Removes focus from input element
 * @param {string} dataId - Component instance identifier
 * @returns {void}
 */
export function focusOut(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.element.blur();
    }
}

/**
 * Updates the float label size
 * @param {string} dataId - Component instance identifier
 * @returns {void}
 */
export function updateFloatLabelSize(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.updateFloatLabelSize();
    }
}

/**
 * Removes float label size styling
 * @param {string} dataId - Component instance identifier
 * @returns {void}
 */
export function removeFloatLabelSize(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.removeFloatLabelSize();
    }
}

/**
 * Destroys TimePicker instance and cleans up resources
 * @param {string} dataId - Component instance identifier
 * @param {HTMLElement} popupElement - Popup element
 * @param {HTMLElement} popupHolderEle - Popup holder
 * @param {object} closeEventArgs - Close event arguments
 * @param {object} options - Configuration options
 * @returns {void}
 */
export function destroy(dataId, popupElement, popupHolderEle, closeEventArgs, options) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        if (popupElement && popupElement instanceof HTMLElement && popupHolderEle) {
            instance.isDisposed = true;
            instance.closePopup(closeEventArgs, options);
        }
        instance.destroy();
    }
}
