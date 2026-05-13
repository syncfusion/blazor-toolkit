const DatePickerInternal = (function () {
    'use strict';

    var ROOT = 'e-datepicker';
    var POPUPDIMENSION = '240px';
    var HALFPOSITION = 2;
    var POPUP_CONTAINER = 'e-popup-wrapper';
    var POPUP = 'e-popup';
    var OVERFLOW = 'e-date-overflow';
    var TIME_OVERFLOW = 'e-time-overflow';
    var CONENT = 'e-content';
    var CONTENT = CONENT;
    var FOOTER_CONTAINER = 'e-footer-container';
    var TBODY = 'tbody';
    var TABLE = 'table';
    var HIDE_POPUP = 'HidePopupElementAsync';
    var CLOSE_POPUP = 'ClosePopupAsync';
    var MOUSE_TOUCH_EVENT = 'mousedown touchstart';
    var MOUSEOVER = 'mouseover';
    var SELECTED = 'e-selected';
    var DAY = 'e-day';
    var TODAY = 'e-today';
    var BTN = 'e-btn';
    var OFFSETVALUE = 4;
    var OPENDURATION = 300;
    var INPUTCONTAINER = 'e-input-group';
    var ARIA_ACTIVE_DESCENDANT = 'aria-activedescendant';
    var LISTCLASS = 'e-list-item';
    var HOVER = 'e-hover';
    var DATE = new Date().getDate();
    var MONTH = new Date().getMonth();
    var YEAR = new Date().getFullYear();
    var CSS_CLASSES = {
        DATE_MODAL: 'e-date-modal',
        MOBILE_WRAP: 'e-datepick-mob-popup-wrap',
        POPUP_EXPAND: 'e-popup-expand',
        NAVIGATION: 'e-navigation',
        ACTIVE: 'e-active',
        CLEAR_ICON: 'e-clear-icon',
        DATETIMEPICKER: 'e-datetimepicker'
    };
    var SfDatePicker = /** @class */ (function () {
        function SfDatePicker(dataId, containerElement, element, dotnetRef, options) {
            window.sfBlazorToolkit.base = window.sfBlazorToolkit.base;
            this.componentName = 'DatePicker';
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
        SfDatePicker.prototype.initialize = function () {
            var _this = this;
            this.index = this.options.showClearButton ? 2 : 1;
            sfBlazorToolkit.base.EventHandler.add(this.element, 'blur', this.inputBlurHandler, this);
            sfBlazorToolkit.base.EventHandler.add(window, 'resize', this.updateFloatLabelSize, this);
            setTimeout(function () {
                _this.updateFloatLabelSize();
            });
        };
        SfDatePicker.prototype.bindInputEvent = function () {
            if (this.enableMask) {
                sfBlazorToolkit.base.EventHandler.add(this.element, 'input', this.inputHandler, this);
            }
            else {
                sfBlazorToolkit.base.EventHandler.remove(this.element, 'input', this.inputHandler);
            }
        };
        SfDatePicker.prototype.addEventListener = function () {
            sfBlazorToolkit.base.EventHandler.add(this.element, 'mouseup', this.mouseUpHandler, this);
            sfBlazorToolkit.base.EventHandler.add(this.element, 'keydown', this.keydownHandler, this);
            sfBlazorToolkit.base.EventHandler.add(this.element, 'focus', this.focusHandler, this);
            this.bindInputEvent();
        };
        SfDatePicker.prototype.removeEventListener = function () {
            sfBlazorToolkit.base.EventHandler.remove(this.element, 'mouseup', this.mouseUpHandler);
            sfBlazorToolkit.base.EventHandler.remove(this.element, 'keydown', this.keydownHandler);
            sfBlazorToolkit.base.EventHandler.remove(this.element, 'focus', this.focusHandler);
        };
        SfDatePicker.prototype.mouseUpHandler = function (e) {
            if (this.enableMask) {
                var isbackward = this.maskObj.isNavigate ? false : true;
                this.maskObj.navigateSelection(isbackward);
                this.maskObj.element = this.element;
                e.preventDefault();
                this.maskObj.validCharacterCheck();
            }
        };
        SfDatePicker.prototype.focusHandler = function () {
            if (this.enableMask) {
                var inputElement = this.element;
                if (!this.options.value && this.options.placeholder && (this.maskObj.mask === inputElement.value || inputElement.value === '')) {
                    if (this.options.floatLabelType === 'Auto' || this.options.floatLabelType === 'Never' || this.options.placeholder) {
                        this.maskObj.updateCurrentValue(this.maskObj.mask);
                        inputElement.selectionStart = 0;
                        inputElement.selectionEnd = inputElement.value.length;
                    }
                }
            }
        };
        SfDatePicker.prototype.inputHandler = function (e) {
            if (this.enableMask) {
                this.maskObj.element = this.element;
                if (!sfBlazorToolkit.base.isNullOrUndefined(e) && !sfBlazorToolkit.base.isNullOrUndefined(e.inputType) && e.inputType === 'insertFromPaste') {
                    this.maskObj.maskPasteInputHandler();
                }
                else {
                    this.maskObj.maskInputHandler();
                }
            }
        };
        SfDatePicker.prototype.keydownHandler = function (e) {
            if (this.enableMask) {
                this.maskObj.element = this.element;
                this.maskObj.keydownHandler(e);
            }
        };
        SfDatePicker.prototype.inputBlurHandler = function (e) {
            if (this.isCalendar() && document.activeElement === this.element) {
                this.dotNetRef.invokeMethodAsync(HIDE_POPUP, e);
            }
        };
        SfDatePicker.prototype.mouseOverHandler = function (e) {
            if (this.popupContainer) {
                var popupListItem = this.popupContainer.querySelectorAll('.e-list-item.e-hover');
                if (popupListItem && popupListItem.length > 0) {
                    for (var i = 0; i < popupListItem.length; i++) {
                        if (popupListItem[i].classList.contains(HOVER)) {
                            popupListItem[i].classList.remove(HOVER);
                        }
                    }
                }
            }
            if (e.target) {
                e.target.classList.add(HOVER);
            }
        };
        // tslint:disable
        SfDatePicker.prototype.renderPopup = function (popupElement, popupHolderEle, openEventArgs, options, step, scrollTo) {
            this.options = options;
            if (this.options.enableMask) this.maskObj.isPopupOpen = true;
            this.popupHolder = popupHolderEle;
            if (popupElement) {
                var oldPopupEle = document.getElementById(popupElement.id);
                if (oldPopupEle) {
                    sfBlazorToolkit.base.detach(oldPopupEle);
                }
            }
            this.createCalendar(popupElement, options);
            if (sfBlazorToolkit.base.Browser.isDevice && sfBlazorToolkit.base.isNullOrUndefined(this.mobilePopupContainer)) {
                this.mobilePopupContainer = sfBlazorToolkit.base.createElement('div', { className: CSS_CLASSES.MOBILE_WRAP });
                sfBlazorToolkit.base.setStyleAttribute(this.mobilePopupContainer, { '-ms-flex-align': 'center', 'align-items': 'center', 'display': '-ms-flexbox',
                    'display': 'flex', '-ms-flex-direction': 'column', 'flex-direction': 'column', 'height': '100%', '-ms-flex-pack': 'center', 'justify-content': 'center',
                    'left': '0', 'max-height': '100%', 'position': 'fixed', 'top': '0', 'width': '100%', 'z-index': '1002' });
                document.body.appendChild(this.mobilePopupContainer);
            }
            const appendToElement = openEventArgs.appendTo === 'model' && this.mobilePopupContainer ? this.mobilePopupContainer : document.body;
            appendToElement.appendChild(this.popupContainer);
            if (!this.options.isDatePopup && this.popupContainer) {
                var popupListItem = this.popupContainer.querySelectorAll('.e-list-item');
                if (popupListItem && popupListItem.length > 0) {
                    for (var i = 0; i < popupListItem.length; i++) {
                        sfBlazorToolkit.base.EventHandler.add(popupListItem[i], MOUSEOVER, this.mouseOverHandler, this);
                    }
                }
            }
            this.popupObj.refreshPosition(this.element);
            if (!options.isDatePopup) {
                this.setScrollPosition();
                if (!sfBlazorToolkit.base.isNullOrUndefined(scrollTo) && this.checkDateValue(new Date(scrollTo)) && !sfBlazorToolkit.base.isNullOrUndefined(this.popupContainer) && !(this.popupContainer.querySelector('.e-navigation') || this.popupContainer.querySelector('.e-active'))) {
                    this.setScrollTo(step, scrollTo);
                }
            }
            var openAnimation = {
                name: 'FadeIn',
                duration: sfBlazorToolkit.base.Browser.isDevice ? 0 : OPENDURATION
            };
            const anim = new sfBlazorToolkit.Animation(openAnimation);
            this.popupObj.show(anim, this.options.zIndex === 1000 ? this.element : null);
            this.setOverlayIndex(this.mobilePopupContainer, this.popupObj.element, this.modal, sfBlazorToolkit.base.Browser.isDevice);
            sfBlazorToolkit.base.EventHandler.add(document, MOUSE_TOUCH_EVENT, this.documentHandler, this);
            if (this.popupContainer.classList.contains(CSS_CLASSES.POPUP_EXPAND)) {
                var modelHeader = this.popupContainer.querySelector('.e-model-header');
                var modelTodayButton = this.popupContainer.querySelector('button.e-today');
                modelTodayButton.classList.add('e-outline');
                modelHeader.insertBefore(modelTodayButton, modelHeader.firstChild);
            }
        };
        SfDatePicker.prototype.setOverlayIndex = function (popupContainer, popupElement, modal, isDevice) {
            if (isDevice && !sfBlazorToolkit.base.isNullOrUndefined(popupElement) && !sfBlazorToolkit.base.isNullOrUndefined(modal) && !sfBlazorToolkit.base.isNullOrUndefined(popupContainer)) {
                const parsed = parseInt(popupElement.style.zIndex, 10);
                const index = !isNaN(parsed) && parsed ? parsed : 1000;
                modal.style.zIndex = String(index - 1);
                popupContainer.style.zIndex = String(index);
            }
        };
        SfDatePicker.prototype.closePopup = function (closeEventArgs, options) {
            this.options = options;
            if (this.options.enableMask) this.maskObj.isPopupOpen = false;
            this.closeEventCallback(closeEventArgs);
        };
        SfDatePicker.prototype.calendarScrollHandler = function (e) {
            var direction = 0;
            if (this.iconRight) {
                switch (e.swipeDirection) {
                    case 'Left':
                        direction = 1;
                        break;
                    case 'Right':
                        direction = -1;
                        break;
                }
            }
            else {
                switch (e.swipeDirection) {
                    case 'Up':
                        direction = 1;
                        break;
                    case 'Down':
                        direction = -1;
                        break;
                }
            }
            if (this.isTouchstart && direction !== 0) {
                this.dotNetRef.invokeMethodAsync('ScrollToNextSection', direction === 1);
                this.isTouchstart = false;
            }
        };
        SfDatePicker.prototype.touchStartHandler = function (e) {
            this.isTouchstart = true;
        };
        SfDatePicker.prototype.createCalendar = function (popupElement, options) {
            var _this = this;
            this.popupContainer = popupElement;
            this.calendarElement = this.popupContainer.firstElementChild;
            this.tableBodyElement = this.calendarElement.querySelector(TBODY);
            var modelClassName = '' + ROOT + ' ' + CSS_CLASSES.DATE_MODAL;
            var modelOverflow = OVERFLOW;
            if (!options.isDatePopup) {
                modelClassName = CSS_CLASSES.DATETIMEPICKER + ' e-time-modal';
                modelOverflow = TIME_OVERFLOW;
            }
            else {
                this.calendarElement.querySelector(TABLE + ' ' + TBODY).className = '';
            }
            if (sfBlazorToolkit.base.Browser.isDevice && sfBlazorToolkit.base.isNullOrUndefined(this.modal)) {
                this.modal = sfBlazorToolkit.base.createElement('div', { className: modelClassName });
                document.body.style.overflow = "hidden";
                sfBlazorToolkit.base.setStyleAttribute(this.modal, { 'background-color': 'rgba(0, 0, 0, .4)', 'height': '100%', 'left': '0',
                    'opacity': '.5', 'pointer-events': 'auto', 'position': 'fixed', 'top': '0', 'width': '100%' });
                this.modal.style.display = 'block';
                document.body.appendChild(this.modal);
            }
            this.popupObj = new sfBlazorToolkit.popups.Popup(this.popupContainer, {
                width: options.isDatePopup ? 'auto' : this.setPopupWidth(this.options.width),
                relateTo: sfBlazorToolkit.base.Browser.isDevice ? document.body : this.containerElement,
                position: sfBlazorToolkit.base.Browser.isDevice ? { X: 'center', Y: 'center' } : { X: 'left', Y: 'bottom' },
                offsetY: OFFSETVALUE,
                targetType: 'container',
                enableRtl: options.enableRtl,
                zIndex: options.zIndex,
                collision: sfBlazorToolkit.base.Browser.isDevice ? { X: 'fit', Y: 'fit' } : { X: 'flip', Y: 'flip' },
                open: function () {
                    if (sfBlazorToolkit.base.Browser.isDevice) {
                        _this.iconRight = parseInt(window.getComputedStyle(_this.calendarElement.querySelector('.e-header .e-prev')).marginRight, 10) > 16 ? true : false;
                        _this.touchModule = new sfBlazorToolkit.Touch(_this.calendarElement.children[1], {
                            swipe: _this.calendarScrollHandler.bind(_this)
                        });
                        sfBlazorToolkit.base.EventHandler.add(_this.calendarElement.children[1], 'touchstart', _this.touchStartHandler, _this);
                    }
                    if (document.activeElement === _this.element && options.isDatePopup) {
                        _this.calendarElement.querySelector('.e-calendar-content-table').addEventListener('focus', function (e) {
                            var focusedDate = e.target.querySelector('tr td.e-focused-date');
                            var selectedDate = e.target.querySelector('tr td.e-selected');
                            if (focusedDate && !focusedDate.classList.contains('e-focused-cell')) {
                                focusedDate.classList.add('e-focused-cell');
                            }
                            else if (selectedDate && !selectedDate.classList.contains('e-focused-cell')) {
                                selectedDate.classList.add('e-focused-cell');
                            }
                        });
                        _this.calendarElement.querySelector('.e-calendar-content-table').addEventListener('blur', function (e) {
                            var focusedDate = e.target.querySelector('tr td.e-focused-date');
                            var selectedDate = e.target.querySelector('tr td.e-selected');
                            if (focusedDate && focusedDate.classList.contains('e-focused-cell')) {
                                focusedDate.classList.remove('e-focused-cell');
                            }
                            else if (selectedDate && selectedDate.classList.contains('e-focused-cell')) {
                                selectedDate.classList.remove('e-focused-cell');
                            }
                        });
                    }
                }, close: function () {
                    _this.popupHolder.appendChild(_this.popupContainer);
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
            if (!options.isDatePopup) {
                if (this.popupContainer.classList.contains(CSS_CLASSES.POPUP_EXPAND)) {
                    this.popupObj.element.style.maxHeight = '100%';
                    this.popupObj.element.style.width = '100%';
                    this.popupObj.element.style.display = 'flex';
                }
                else {
                    this.popupObj.element.style.maxHeight = POPUPDIMENSION;
                }
            }
        };
        SfDatePicker.prototype.getPopupHeight = function () {
            const height = parseInt(POPUPDIMENSION, 10);
            const popupHeight = this.popupContainer.getBoundingClientRect().height;
            return popupHeight > height ? height : popupHeight;
        };
        SfDatePicker.prototype.setScrollPosition = function () {
                if ((this.popupContainer && this.popupContainer.querySelector('.' + CSS_CLASSES.NAVIGATION) || this.popupContainer.querySelector('.' + CSS_CLASSES.ACTIVE))
                && !this.options.isDatePopup) {
                var selectElement = this.popupContainer.querySelector('.' + CSS_CLASSES.NAVIGATION) || this.popupContainer.querySelector('.' + CSS_CLASSES.ACTIVE);
                this.findScrollTop(selectElement);
            }
        };
        SfDatePicker.prototype.setScrollTo = function (step, scrollTo) {
            let element;
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.popupContainer)) {
                const items = this.popupContainer.querySelectorAll('.' + LISTCLASS);
                if (items && items.length) {
                    var timeCollections = [];
                    for (let index = 0; index < items.length; index++) {
                        timeCollections.push(items[index].getAttribute('data-value'));
                    }
                    var initialTime = !sfBlazorToolkit.base.isNullOrUndefined(this.options.value) && typeof (this.options.value) === 'string' ? this.getDateObject(new Date(new Date().toDateString() + ' ' + timeCollections[0])).setMilliseconds(0) : new Date(new Date().toDateString() + ' ' + timeCollections[0]).setMilliseconds(0);
                    var scrollTime = this.getDateObject(new Date(scrollTo)).getTime();
                    element = items[Math.round((scrollTime - initialTime) / (step * 60000))];
                }
            }
            else {
                this.popupContainer.scrollTop = 0;
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
                this.findScrollTop(element);
            }
            else {
                this.popupContainer.scrollTop = 0;
            }
        };
        SfDatePicker.prototype.getDateObject = function (text) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(text)) {
                var dateValue = text;
                var isValue = !sfBlazorToolkit.base.isNullOrUndefined(this.options.value);
                var value = isValue ? typeof (this.options.value) === 'string' ? this.options.value : this.options.value.toString() : null;
                if (this.checkDateValue(dateValue)) {
                    var date = isValue ? new Date(value).getDate() : DATE;
                    var month = isValue ? new Date(value).getMonth() : MONTH;
                    var year = isValue ? new Date(value).getFullYear() : YEAR;
                    return new Date(year, month, date, dateValue.getHours(), dateValue.getMinutes(), dateValue.getSeconds());
                }
            }
            return null;
        };
        SfDatePicker.prototype.checkDateValue = function (value) {
            return (!sfBlazorToolkit.base.isNullOrUndefined(value) && value instanceof Date && !isNaN(+value)) ? value : null;
        };
        SfDatePicker.prototype.findScrollTop = function (element) {
            var listHeight = this.getPopupHeight();
            var nextEle = element.nextElementSibling;
            var height = nextEle ? nextEle.offsetTop : element.offsetTop;
            var liHeight = element.getBoundingClientRect().height;
            if ((height + element.offsetTop) > listHeight) {
                this.popupContainer.scrollTop = nextEle ? (height - (listHeight / HALFPOSITION + liHeight / HALFPOSITION)) : height;
            }
            else {
                this.popupContainer.scrollTop = 0;
            }
        };
        SfDatePicker.prototype.closeEventCallback = function (eventArgs) {
            var preventArgs = eventArgs;
            if (this.isCalendar() && !preventArgs.cancel && this.popupObj) {
                if (this.popupContainer) {
                    var popupListItem = this.popupContainer.querySelectorAll('.e-list-item');
                    if (popupListItem && popupListItem.length > 0) {
                        for (var i = 0; i < popupListItem.length; i++) {
                            sfBlazorToolkit.base.EventHandler.remove(popupListItem[i], MOUSEOVER, this.mouseOverHandler);
                        }
                    }
                }
                this.popupObj.hide();
            }
            if (sfBlazorToolkit.base.Browser.isDevice && this.modal) {
                this.modal.style.display = 'none';
                this.modal.outerHTML = '';
                this.modal = null;
            }
            if (sfBlazorToolkit.base.Browser.isDevice) {
                sfBlazorToolkit.base.removeClass(document.body, OVERFLOW);
                sfBlazorToolkit.base.removeClass(document.body, TIME_OVERFLOW);
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.mobilePopupContainer)) {
                    this.mobilePopupContainer.remove();
                    this.mobilePopupContainer = null;
                }
            }
            sfBlazorToolkit.base.EventHandler.remove(document, MOUSE_TOUCH_EVENT, this.documentHandler);
            if (sfBlazorToolkit.base.Browser.isDevice && this.options.allowEdit && !this.options.readonly) {
                this.element.removeAttribute('readonly');
            }
        };
        SfDatePicker.prototype.documentHandler = function (e) {
            const target = e.target;
            const popupObjExists = !sfBlazorToolkit.base.isNullOrUndefined(this.popupObj) && this.popupObj.element;
            if (popupObjExists && (this.containerElement.contains(target) || (this.popupObj.element && this.popupObj.element.contains(target))) && e.type !== 'touchstart') {
                e.preventDefault();
            }
            const clearElement = this.containerElement ? this.containerElement.querySelector('.' + CSS_CLASSES.CLEAR_ICON) : null;
            const selectedElement = this.tableBodyElement ? this.tableBodyElement.querySelector('.e-selected') : null;
            const dateValue = this.options.value ? this.options.value.toString() : null;
            if (target === clearElement && selectedElement) {
                sfBlazorToolkit.base.removeClass(this.tableBodyElement.querySelector('.e-selected'), SELECTED);
            }
            const isInsideDatePopup = sfBlazorToolkit.base.closest(target, '.' + ROOT + '.' + POPUP_CONTAINER);
            const isInsideDateTimePopup = sfBlazorToolkit.base.closest(target, '.' + CSS_CLASSES.DATETIMEPICKER + '.' + POPUP_CONTAINER);
            const isInsideInputContainer = sfBlazorToolkit.base.closest(target, '.' + INPUTCONTAINER) === this.containerElement;
            if (!isInsideDatePopup && !isInsideDateTimePopup && !isInsideInputContainer && !target.classList.contains(DAY) && !this.isDisposed) {
                this.dotNetRef.invokeMethodAsync(HIDE_POPUP, e);
                this.element.focus();
            }
            else if (isInsideDatePopup) {
                if (target.classList.contains(DAY)
                    && !sfBlazorToolkit.base.isNullOrUndefined(e.target.parentElement)
                    && e.target.parentElement.classList.contains(SELECTED)
                    && sfBlazorToolkit.base.closest(target, '.' + CONENT)
                    && sfBlazorToolkit.base.closest(target, '.' + CONENT).classList.contains('e-' + this.options.depth.toLowerCase()) && !this.isDisposed) {
                    this.dotNetRef.invokeMethodAsync(HIDE_POPUP, e);
                }
                else if (sfBlazorToolkit.base.closest(target, '.' + FOOTER_CONTAINER)
                    && target.classList.contains(TODAY)
                    && target.classList.contains(BTN)
                    && (+new Date(dateValue) === +this.generateTodayVal(new Date(dateValue))) && !this.isDisposed) {
                    this.dotNetRef.invokeMethodAsync(HIDE_POPUP, e);
                }
            }
        };
        SfDatePicker.prototype.generateTodayVal = function (value) {
            var tempValue = new Date();
            if (value) {
                tempValue.setHours(value.getHours());
                tempValue.setMinutes(value.getMinutes());
                tempValue.setSeconds(value.getSeconds());
                tempValue.setMilliseconds(value.getMilliseconds());
            }
            else {
                tempValue = new Date(tempValue.getFullYear(), tempValue.getMonth(), tempValue.getDate(), 0, 0, 0, 0);
            }
            return tempValue;
        };
        SfDatePicker.prototype.isCalendar = function () {
            return this.popupContainer && this.popupContainer.classList.contains('' + POPUP_CONTAINER);
        };
        SfDatePicker.prototype.setWidth = function (width) {
            if (typeof width === 'number' || typeof width === 'string') {
                width = width;
            }
            else {
                width = '100%';
            }
            return width;
        };
        SfDatePicker.prototype.setPopupWidth = function (width) {
            width = this.setWidth(width);
            if (width.indexOf('%') > -1) {
                var containerStyle = this.containerElement.getBoundingClientRect();
                var inputWidth = containerStyle.width * parseFloat(width) / 100;
                width = inputWidth.toString() + 'px';
            }
            return width;
        };
        SfDatePicker.prototype.updateAriaActiveDescendant = function (cellId) {
            this.element.setAttribute(ARIA_ACTIVE_DESCENDANT, cellId);
        };
        SfDatePicker.prototype.getTextWidth = function (text) {
            var tempSpan = sfBlazorToolkit.base.createElement('span', { styles: {
                'visibility': 'hidden', 'whiteSpace': 'nowrap'
            } });
            tempSpan.textContent = text;
            document.body.appendChild(tempSpan);
            var textWidth = tempSpan.offsetWidth;
            document.body.removeChild(tempSpan);
            return textWidth;
        };
        SfDatePicker.prototype.updateFloatLabelSize = function () {
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
        SfDatePicker.prototype.destroy = function () {
            this.containerElement = null;
            this.element = null;
            if (this.enableMask) {
                this.maskObj.destroy();
                this.maskObj = null;
            }
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            window.sfBlazorToolkit.base.disposeWindowsInstance(this.dataId);
        };
        SfDatePicker.prototype.removeFloatLabelSize = function () {
            if (this.containerElement) {
                var floatLabel = this.containerElement.querySelector('.e-float-text');
                if (!sfBlazorToolkit.base.isNullOrUndefined(floatLabel)) {
                    floatLabel.style.width = '';
                }
            }
        };
        return SfDatePicker;
    }());
    // tslint:disable

    // Expose internal constructor so exported functions can create and use instances
    return { SfDatePicker: SfDatePicker };

}());

/**
 * Initializes a DatePicker instance.
 * @param {string} dataId - Unique instance id
 * @param {HTMLElement} containerElement - The container element
 * @param {HTMLElement} element - The input element
 * @param {any} dotnetRef - DotNet reference for callbacks
 * @param {Object} options - Initialization options
 * @returns {Object|{currentMaskFormat:null,inputElementValue:any}} - Mask info or defaults
 */
export function initialize(dataId, containerElement, element, dotnetRef, options) {
    if (element && dataId) {
        var instance = new DatePickerInternal.SfDatePicker(dataId, containerElement, element, dotnetRef, options);
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
 * Create mask for an existing instance.
 */
export function createMask(dataId, options) {
    if (dataId && options.enableMask) {
        var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
            return instance.maskObj.createMask(options);
        }
    }
    return { 'currentMaskFormat': null, 'inputElementValue': options.value };
}

/**
 * Update current value on the instance.
 */
export function updateCurrentValue(dataId, currentValueAsString) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.maskObj.updateCurrentValue(currentValueAsString);
    }
}

/**
 * Render popup for the instance.
 */
export function renderPopup(dataId, popupElement, popupHolderEle, openEventArgs, options, step, scrollTo) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && popupElement && popupHolderEle) {
        instance.renderPopup(popupElement, popupHolderEle, openEventArgs, options, step, scrollTo);
    }
}

/**
 * Close popup for the instance.
 */
export function closePopup(dataId, closeEventArgs, options) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && closeEventArgs) {
        instance.closePopup(closeEventArgs, options);
    }
}

/**
 * Focus management helpers
 */
export function focusIn(dataId, isReadOnly) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        if (document.activeElement !== instance.element) {
            setTimeout(function () {
                if (isReadOnly) {
                    instance.element.setAttribute('readonly', 'true');
                }
                if (instance && instance.element) {
                    instance.element.focus();
                }
            }, 100);
        }
        else {
            if (isReadOnly) {
                instance.element.setAttribute('readonly', 'true');
            }
            instance.element.focus();
        }
    }
}

export function updateScrollPosition(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.setScrollPosition();
    }
}

export function updateAriaActiveDescendant(dataId, cellId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.updateAriaActiveDescendant(cellId);
    }
}

export function focusOut(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.element.blur();
    }
}

export function moveFocusToPopup(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.element.blur();
        instance.popupContainer.firstElementChild.firstElementChild.firstChild.focus();
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

export function destroy(dataId, popupElement, popupHolderEle, closeEventArgs, options) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        if (popupElement && popupElement instanceof HTMLElement && (popupHolderEle || instance.popupObj)) {
            instance.isDisposed = true;
            instance.closePopup(closeEventArgs, options);
        }
        instance.destroy();
    }
}
