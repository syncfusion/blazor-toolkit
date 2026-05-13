const Popup = (function (exports) {
    'use strict';

    /**
     * Specifies the offset position values.
    */
    var PositionData = function () {
        this.X = 'left';
        this.Y = 'top';
    };
    // don't use space in classNames
    var CLASSNAMES = {
        ROOT: 'e-popup',
        RTL: 'e-rtl',
        OPEN: 'e-popup-open',
        CLOSE: 'e-popup-close'
    };
    // constants
    var MAX_Z_INDEX = 2147483647;
    /**
     * Position library
     */
    var elementRect;
    var popupRect;
    var element;
    var parentDocument;
    var fixedParent = false;
    /**
     * Represents the Popup Component
     */
    var Popup = /** @class */ (function () {
        function Popup(element, options) {
            this.element = element || document.createElement('div');
            options = options || {};
            // default properties (previously applied via decorators)
            this.height = 'auto';
            this.width = 'auto';
            this.content = null;
            this.targetType = 'container';
            this.viewPortElement = null;
            this.collision = { X: 'none', Y: 'none' };
            this.relateTo = '';
            this.position = new PositionData();
            this.offsetX = 0;
            this.offsetY = 0;
            this.zIndex = 1000;
            this.enableRtl = false;
            this.actionOnScroll = 'reposition';
            this.showAnimation = null;
            this.hideAnimation = null;
            // event callbacks (can be set by consumers)
            this.open = null;
            this.close = null;
            this.targetExitViewport = null;
            this.isDestroyed = false;
            // apply provided options
            for (var p in options) {
                if (Object.prototype.hasOwnProperty.call(options, p)) {
                    if (p === 'position' && typeof options[p] === 'object') {
                        this.position = this.position || new PositionData();
                        if (options[p].X !== undefined) {
                            this.position.X = options[p].X;
                        }
                        if (options[p].Y !== undefined) {
                            this.position.Y = options[p].Y;
                        }
                    }
                    else {
                        this[p] = options[p];
                    }
                }
            }
            this.render();
        }

        // Minimal setProperties replacement so external code can update properties
        Popup.prototype.setProperties = function (newProp, silent) {
            if (!newProp) {
                return;
            }
            // capture old values for changed keys
            var oldProp = {};
            for (var k in newProp) {
                if (!Object.prototype.hasOwnProperty.call(newProp, k))
                    continue;
                if (k === 'position' && typeof newProp[k] === 'object') {
                    oldProp.position = this.position ? { X: this.position.X, Y: this.position.Y } : { X: undefined, Y: undefined };
                }
                else {
                    oldProp[k] = this[k];
                }
            }
            // apply new values
            for (var k in newProp) {
                if (!Object.prototype.hasOwnProperty.call(newProp, k))
                    continue;
                if (k === 'position' && typeof newProp[k] === 'object') {
                    this.position = this.position || new PositionData();
                    if (newProp[k].X !== undefined) this.position.X = newProp[k].X;
                    if (newProp[k].Y !== undefined) this.position.Y = newProp[k].Y;
                }
                else {
                    this[k] = newProp[k];
                }
            }
            // notify if not silent
            if (silent !== true && typeof this.onPropertyChanged === 'function') {
                try {
                    this.onPropertyChanged(newProp, oldProp);
                }
                catch (e) {
                    // swallow errors to avoid breaking existing flows
                }
            }
        };

        // Minimal trigger implementation to call event callbacks
        Popup.prototype.trigger = function (eventName, args) {
            try {
                var handler = this[eventName];
                if (typeof handler === 'function') {
                    handler.call(this, args);
                }
            }
            catch (e) {
                // swallow errors to remain non-breaking
            }
        };
        /**
         * Called internally if any of the property value changed.
         *
         * @param {PopupModel} newProp - specifies the new property
         * @param {PopupModel} oldProp - specifies the old property
         * @private
         * @returns {void}
         */
        Popup.prototype.onPropertyChanged = function (newProp, oldProp) {
            for (var _i = 0, _a = Object.keys(newProp); _i < _a.length; _i++) {
                var prop = _a[_i];
                switch (prop) {
                    case 'width':
                        sfBlazorToolkit.base.setStyleAttribute(this.element, { 'width': newProp.width });
                        break;
                    case 'height':
                        sfBlazorToolkit.base.setStyleAttribute(this.element, { 'height': newProp.height });
                        break;
                    case 'zIndex':
                        sfBlazorToolkit.base.setStyleAttribute(this.element, { 'zIndex': newProp.zIndex });
                        break;
                    case 'enableRtl':
                        this.setEnableRtl();
                        break;
                    case 'position':
                    case 'relateTo':
                        this.refreshPosition();
                        break;
                    case 'offsetX': {
                        var x = newProp.offsetX - oldProp.offsetX;
                        this.element.style.left = (parseInt(this.element.style.left, 10) + (x)).toString() + 'px';
                        break;
                    }
                    case 'offsetY': {
                        var y = newProp.offsetY - oldProp.offsetY;
                        this.element.style.top = (parseInt(this.element.style.top, 10) + (y)).toString() + 'px';
                        break;
                    }
                    case 'content':
                        this.setContent();
                        break;
                    case 'actionOnScroll':
                        if (newProp.actionOnScroll !== 'none') {
                            this.wireScrollEvents();
                        }
                        else {
                            this.unwireScrollEvents();
                        }
                        break;
                }
            }
        };
        /**
         * gets the Component module name.
         *
         * @returns {void}
         * @private
         */
        Popup.prototype.getModuleName = function () {
            return 'popup';
        };
        /**
         * To destroy the control.
         *
         * @returns {void}
         */
        Popup.prototype.destroy = function () {
            if (this.element.classList.contains('e-popup-open')) {
                this.unwireEvents();
            }
            this.element.classList.remove(CLASSNAMES.ROOT, CLASSNAMES.RTL, CLASSNAMES.OPEN, CLASSNAMES.CLOSE);
            this.content = null;
            this.relateTo = null;
            destroy();
            this.isDestroyed = true;
        };
        /**
         * To Initialize the control rendering
         *
         * @returns {void}
         * @private
         */
        Popup.prototype.render = function () {
            this.element.classList.add(CLASSNAMES.ROOT);
            var styles = {};
            if (this.zIndex !== 1000) {
                styles.zIndex = this.zIndex;
            }
            if (this.width !== 'auto') {
                styles.width = this.width;
            }
            if (this.height !== 'auto') {
                styles.height = this.height;
            }
            sfBlazorToolkit.base.setStyleAttribute(this.element, styles);
            this.fixedParent = false;
            this.setEnableRtl();
            this.setContent();
        };
        Popup.prototype.wireEvents = function () {
            if (sfBlazorToolkit.base.Browser.isDevice) {
                sfBlazorToolkit.base.EventHandler.add(window, 'orientationchange', this.orientationOnChange, this);
            }
            if (this.actionOnScroll !== 'none') {
                this.wireScrollEvents();
            }
        };
        Popup.prototype.wireScrollEvents = function () {
            var relate = this.getRelateToElement();
            if (relate) {
                var parents = this.getScrollableParent(relate);
                parents.forEach(function (parentEl) {
                    sfBlazorToolkit.base.EventHandler.add(parentEl, 'scroll', this.scrollRefresh, this);
                }, this);
            }
        };
        Popup.prototype.unwireEvents = function () {
            if (sfBlazorToolkit.base.Browser.isDevice) {
                sfBlazorToolkit.base.EventHandler.remove(window, 'orientationchange', this.orientationOnChange);
            }
            if (this.actionOnScroll !== 'none') {
                this.unwireScrollEvents();
            }
        };
        Popup.prototype.unwireScrollEvents = function () {
            var relate = this.getRelateToElement();
            if (relate) {
                var parents = this.getScrollableParent(relate);
                parents.forEach(function (parentEl) {
                    sfBlazorToolkit.base.EventHandler.remove(parentEl, 'scroll', this.scrollRefresh);
                }, this);
            }
        };
        Popup.prototype.getRelateToElement = function () {
            var relateToElement = this.relateTo === '' || sfBlazorToolkit.base.isNullOrUndefined(this.relateTo) ?
                document.body : this.relateTo;
            this.setProperties({ relateTo: relateToElement }, true);
            return ((typeof this.relateTo) === 'string') ?
                document.querySelector(this.relateTo) : this.relateTo;
        };
        Popup.prototype.scrollRefresh = function (e) {
            if (this.actionOnScroll === 'reposition') {
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.element) && !(this.element.offsetParent === e.target ||
                    (this.element.offsetParent && this.element.offsetParent.tagName === 'BODY' &&
                        e.target.parentElement == null))) {
                    this.refreshPosition();
                }
            }
            else if (this.actionOnScroll === 'hide') {
                this.hide();
            }
            if (this.actionOnScroll !== 'none') {
                var relateTo = this.getRelateToElement();
                if (relateTo) {
                    var targetVisible = this.isElementOnViewport(relateTo, e.target);
                    if (!targetVisible && !this.targetInvisibleStatus) {
                        this.trigger('targetExitViewport');
                        this.targetInvisibleStatus = true;
                    }
                    else if (targetVisible) {
                        this.targetInvisibleStatus = false;
                    }
                }
            }
        };
        /**
         * This method is to get the element visibility on viewport when scroll
         * the page. This method will returns true even though 1 px of element
         * part is in visible.
         *
         * @param {HTMLElement} relateToElement - specifies the element
         * @param {HTMLElement} scrollElement - specifies the scroll element
         * @returns {boolean} - retruns the boolean
         */
        // eslint-disable-next-line
        Popup.prototype.isElementOnViewport = function (relateToElement, scrollElement) {
            var scrollParents = this.getScrollableParent(relateToElement);
            return scrollParents.every(function (sp) {
                return this.isElementVisible(relateToElement, sp);
            }, this);
        };
        Popup.prototype.isElementVisible = function (relateToElement, scrollElement) {
            var rect = this.checkGetBoundingClientRect(relateToElement);
            if (!rect.height || !rect.width) {
                return false;
            }
            var scrollRect = this.checkGetBoundingClientRect(scrollElement);
            if (!sfBlazorToolkit.base.isNullOrUndefined(scrollRect)) {
                return !(rect.bottom < scrollRect.top) &&
                    (!(rect.bottom > scrollRect.bottom) &&
                        (!(rect.right > scrollRect.right) &&
                            !(rect.left < scrollRect.left)));
            }
            var win = window;
            var windowTop = win.scrollY;
            var windowLeft = win.scrollX;
            var windowRight = windowLeft + win.outerWidth;
            var windowBottom = windowTop + win.outerHeight;
            var off = calculatePosition(relateToElement);
            var ele = {
                top: off.top,
                left: off.left,
                right: off.left + rect.width,
                bottom: off.top + rect.height
            };
            var elementViewTop = windowBottom - ele.top;
            var elementViewLeft = windowRight - ele.left;
            var elementViewBottom = ele.bottom - windowTop;
            var elementViewRight = ele.right - windowLeft;
            return elementViewTop > 0 && elementViewLeft > 0 && elementViewRight > 0 && elementViewBottom > 0;
        };
        /**
         * Initialize the event handler
         *
         * @returns {void}
         * @private
         */
        Popup.prototype.preRender = function () {
            //There is no event handler
        };
        Popup.prototype.setEnableRtl = function () {
            this.reposition();
            if (this.enableRtl) {
                this.element.classList.add(CLASSNAMES.RTL);
            }
            else {
                this.element.classList.remove(CLASSNAMES.RTL);
            }
        };
        Popup.prototype.setContent = function () {
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.content)) {
                this.element.innerHTML = '';
                if (typeof (this.content) === 'string') {
                    this.element.textContent = this.content;
                }
                else {
                    var relateToElem = this.getRelateToElement();
                    // eslint-disable-next-line
                    var props = this.content.props;
                    if (!relateToElem.classList.contains('e-dropdown-btn') || sfBlazorToolkit.base.isNullOrUndefined(props)) {
                        this.element.appendChild(this.content);
                    }
                }
            }
        };
        Popup.prototype.orientationOnChange = function () {
            setTimeout(function () { return this.refreshPosition(); }.bind(this), 200);
        };
        /**
         * Based on the `relative` element and `offset` values, `Popup` element position will refreshed.
         *
         * @param {HTMLElement} target - The target element.
         * @param {boolean} collision - Specifies whether to check for collision.
         * @returns {void}
         */
        Popup.prototype.refreshPosition = function (target, collision) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(target)) {
                this.checkFixedParent(target);
            }
            this.reposition();
            if (!collision) {
                this.checkCollision();
            }
        };
        Popup.prototype.reposition = function () {
            var pos;
            var position;
            var relateToElement = this.getRelateToElement();
            if (typeof this.position.X === 'number' && typeof this.position.Y === 'number') {
                pos = { left: this.position.X, top: this.position.Y };
            }
            else if ((typeof this.position.X === 'string' && typeof this.position.Y === 'number') ||
                (typeof this.position.X === 'number' && typeof this.position.Y === 'string')) {
                var parentDisplay = void 0;
                var display = this.element.style.display;
                this.element.style.display = 'block';
                if (this.element.classList.contains('e-dlg-modal')) {
                    parentDisplay = this.element.parentElement.style.display;
                    this.element.parentElement.style.display = 'block';
                }
                position = this.getAnchorPosition(relateToElement, this.element, this.position, this.offsetX, this.offsetY);
                if (typeof this.position.X === 'string') {
                    pos = { left: position.left, top: this.position.Y };
                }
                else {
                    pos = { left: this.position.X, top: position.top };
                }
                this.element.style.display = display;
                if (this.element.classList.contains('e-dlg-modal')) {
                    this.element.parentElement.style.display = parentDisplay;
                }
            }
            else if (relateToElement) {
                var height = this.element.clientHeight;
                var display = this.element.style.display;
                this.element.style.display = 'block';
                pos = this.getAnchorPosition(relateToElement, this.element, this.position, this.offsetX, this.offsetY, height);
                this.element.style.display = display;
            }
            else {
                pos = { left: 0, top: 0 };
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(pos)) {
                this.element.style.left = pos.left + 'px';
                this.element.style.top = pos.top + 'px';
            }
        };
        Popup.prototype.checkGetBoundingClientRect = function (ele) {
            var eleRect;
            try {
                eleRect = ele.getBoundingClientRect();
                return eleRect;
            }
            catch (error) {
                return null;
            }
        };
        Popup.prototype.getAnchorPosition = function (anchorEle, ele, position, offsetX, offsetY, height) {
            if (height === void 0) { height = 0; }
            var eleRect = this.checkGetBoundingClientRect(ele);
            var anchorRect = this.checkGetBoundingClientRect(anchorEle);
            if (sfBlazorToolkit.base.isNullOrUndefined(eleRect) || sfBlazorToolkit.base.isNullOrUndefined(anchorRect)) {
                return null;
            }
            var anchor = anchorEle;
            var anchorPos = { left: 0, top: 0 };
            if (ele.offsetParent && ele.offsetParent.tagName === 'BODY' && anchorEle.tagName === 'BODY') {
                anchorPos = calculatePosition(anchorEle);
            }
            else {
                if ((ele.classList.contains('e-dlg-modal') && anchor.tagName !== 'BODY')) {
                    ele = ele.parentElement;
                }
                anchorPos = calculateRelativeBasedPosition(anchor, ele);
            }
            switch (position.X) {
                default:
                case 'left':
                    break;
                case 'center':
                    if ((ele.classList.contains('e-dlg-modal') && anchor.tagName === 'BODY' && this.targetType === 'container')) {
                        anchorPos.left += (window.innerWidth / 2 - eleRect.width / 2);
                    }
                    else if (this.targetType === 'container') {
                        anchorPos.left += (anchorRect.width / 2 - eleRect.width / 2);
                    }
                    else {
                        anchorPos.left += (anchorRect.width / 2);
                    }
                    break;
                case 'right':
                    if ((ele.classList.contains('e-dlg-modal') && anchor.tagName === 'BODY' && this.targetType === 'container')) {
                        anchorPos.left += (window.innerWidth - eleRect.width);
                    }
                    else if (this.targetType === 'container') {
                        var scaleX = 1;
                        var tranformElement = getTransformElement(ele);
                        if (tranformElement) {
                            var transformStyle = getComputedStyle(tranformElement).transform;
                            if (transformStyle !== 'none') {
                                var matrix = new DOMMatrix(transformStyle);
                                scaleX = matrix.a;
                            }
                            var zoomStyle = getComputedStyle(tranformElement).zoom;
                            if (zoomStyle !== 'none') {
                                var bodyZoom = getZoomValue(document.body);
                                scaleX = bodyZoom * scaleX;
                            }
                        }
                        anchorPos.left += ((anchorRect.width - eleRect.width) / scaleX);
                    }
                    else {
                        anchorPos.left += (anchorRect.width);
                    }
                    break;
            }
            switch (position.Y) {
                default:
                case 'top':
                    break;
                case 'center':
                    if ((ele.classList.contains('e-dlg-modal') && anchor.tagName === 'BODY' && this.targetType === 'container')) {
                        anchorPos.top += (window.innerHeight / 2 - eleRect.height / 2);
                    }
                    else if (this.targetType === 'container') {
                        anchorPos.top += (anchorRect.height / 2 - eleRect.height / 2);
                    }
                    else {
                        anchorPos.top += (anchorRect.height / 2);
                    }
                    break;
                case 'bottom':
                    if ((ele.classList.contains('e-dlg-modal') && anchor.tagName === 'BODY' && this.targetType === 'container')) {
                        anchorPos.top += (window.innerHeight - eleRect.height);
                    }
                    else if (this.targetType === 'container' && !ele.classList.contains('e-dialog')) {
                        anchorPos.top += (anchorRect.height - eleRect.height);
                    }
                    else if (this.targetType === 'container' && ele.classList.contains('e-dialog')) {
                        anchorPos.top += (anchorRect.height - height);
                    }
                    else {
                        anchorPos.top += (anchorRect.height);
                    }
                    break;
            }
            anchorPos.left += offsetX;
            anchorPos.top += offsetY;
            return anchorPos;
        };
        Popup.prototype.callFlip = function (param) {
            var relateToElement = this.getRelateToElement();
            flip(this.element, relateToElement, this.offsetX, this.offsetY, this.position.X, this.position.Y, this.viewPortElement, param, this.fixedParent);
        };
        Popup.prototype.callFit = function (param) {
            var collisions = isCollide(this.element, this.viewPortElement);
            if (collisions.length !== 0) {
                if (sfBlazorToolkit.base.isNullOrUndefined(this.viewPortElement)) {
                    var data = fit(this.element, this.viewPortElement, param);
                    if (param.X) {
                        this.element.style.left = data.left + 'px';
                    }
                    if (param.Y) {
                        this.element.style.top = data.top + 'px';
                    }
                }
                else {
                    var elementRect = this.checkGetBoundingClientRect(this.element);
                    var viewPortRect = this.checkGetBoundingClientRect(this.viewPortElement);
                    if (sfBlazorToolkit.base.isNullOrUndefined(elementRect) || sfBlazorToolkit.base.isNullOrUndefined(viewPortRect)) {
                        return null;
                    }
                    if (param && param.Y === true) {
                        if (viewPortRect.top > elementRect.top) {
                            this.element.style.top = '0px';
                        }
                        else if (viewPortRect.bottom < elementRect.bottom) {
                            this.element.style.top = parseInt(this.element.style.top, 10) - (elementRect.bottom - viewPortRect.bottom) + 'px';
                        }
                    }
                    if (param && param.X === true) {
                        if (viewPortRect.right < elementRect.right) {
                            this.element.style.left = parseInt(this.element.style.left, 10) - (elementRect.right - viewPortRect.right) + 'px';
                        }
                        else if (viewPortRect.left > elementRect.left) {
                            this.element.style.left = parseInt(this.element.style.left, 10) + (viewPortRect.left - elementRect.left) + 'px';
                        }
                    }
                }
            }
        };
        Popup.prototype.checkCollision = function () {
            var horz = this.collision.X;
            var vert = this.collision.Y;
            if (horz === 'none' && vert === 'none') {
                return;
            }
            if (horz === 'flip' && vert === 'flip') {
                this.callFlip({ X: true, Y: true });
            }
            else if (horz === 'fit' && vert === 'fit') {
                this.callFit({ X: true, Y: true });
            }
            else {
                if (horz === 'flip') {
                    this.callFlip({ X: true, Y: false });
                }
                else if (vert === 'flip') {
                    this.callFlip({ Y: true, X: false });
                }
                if (horz === 'fit') {
                    this.callFit({ X: true, Y: false });
                }
                else if (vert === 'fit') {
                    this.callFit({ X: false, Y: true });
                }
            }
        };
        /**
         * Shows the popup element from screen.
         *
         * @returns {void}
         * @param {AnimationModel} animationOptions - specifies the model
         * @param { HTMLElement } relativeElement - To calculate the zIndex value dynamically.
         */
        Popup.prototype.show = function (animationOptions, relativeElement) {
            var _this = this;
            this.wireEvents();
            this.getRelateToElement();
            if (this.zIndex === 1000 || !sfBlazorToolkit.base.isNullOrUndefined(relativeElement)) {
                var zIndexElement = (sfBlazorToolkit.base.isNullOrUndefined(relativeElement)) ? this.element : relativeElement;
                this.zIndex = getZIndexPartial(zIndexElement);
                sfBlazorToolkit.base.setStyleAttribute(this.element, { 'zIndex': this.zIndex });
            }
            animationOptions = (!sfBlazorToolkit.base.isNullOrUndefined(animationOptions) && typeof animationOptions === 'object') ?
                animationOptions : this.showAnimation;
            if (this.collision.X !== 'none' || this.collision.Y !== 'none') {
                sfBlazorToolkit.base.removeClass(this.element, CLASSNAMES.CLOSE);
                sfBlazorToolkit.base.addClass(this.element, CLASSNAMES.OPEN);
                this.checkCollision();
                sfBlazorToolkit.base.removeClass(this.element, CLASSNAMES.OPEN);
                sfBlazorToolkit.base.addClass(this.element, CLASSNAMES.CLOSE);
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(animationOptions)) {
                animationOptions.begin = function () {
                    if (!_this.isDestroyed) {
                        sfBlazorToolkit.base.removeClass(_this.element, CLASSNAMES.CLOSE);
                        sfBlazorToolkit.base.addClass(_this.element, CLASSNAMES.OPEN);
                    }
                };
                animationOptions.end = function () { if (!_this.isDestroyed) { _this.trigger('open'); } };
                new sfBlazorToolkit.Animation(animationOptions).animate(this.element);
            }
            else {
                sfBlazorToolkit.base.removeClass(this.element, CLASSNAMES.CLOSE);
                sfBlazorToolkit.base.addClass(this.element, CLASSNAMES.OPEN);
                this.trigger('open');
            }
        };
        /**
         * Hides the popup element from screen.
         *
         * @param {AnimationModel} animationOptions - To give the animation options.
         * @returns {void}
         */
        Popup.prototype.hide = function (animationOptions) {
            var _this = this;
            animationOptions = (!sfBlazorToolkit.base.isNullOrUndefined(animationOptions) && typeof animationOptions === 'object') ?
                animationOptions : this.hideAnimation;
            if (!sfBlazorToolkit.base.isNullOrUndefined(animationOptions)) {
                animationOptions.end = function () { if (!_this.isDestroyed) { sfBlazorToolkit.base.removeClass(_this.element, CLASSNAMES.OPEN); sfBlazorToolkit.base.addClass(_this.element, CLASSNAMES.CLOSE); _this.trigger('close'); } };
                new sfBlazorToolkit.Animation(animationOptions).animate(this.element);
            }
            else {
                sfBlazorToolkit.base.removeClass(this.element, CLASSNAMES.OPEN);
                sfBlazorToolkit.base.addClass(this.element, CLASSNAMES.CLOSE);
                this.trigger('close');
            }
            this.unwireEvents();
        };
        /**
         * Gets scrollable parent elements for the given element.
         *
         * @returns {void}
         * @param { HTMLElement } element - Specify the element to get the scrollable parents of it.
         */
        Popup.prototype.getScrollableParent = function (element) {
            this.checkFixedParent(element);
            return getScrollableParent(element, this.fixedParent);
        };
        Popup.prototype.checkFixedParent = function (element) {
            var parent = element.parentElement;
            while (parent && parent.tagName !== 'HTML') {
                var parentStyle = getComputedStyle(parent);
                if ((parentStyle.position === 'fixed' || parentStyle.position === 'sticky') && !sfBlazorToolkit.base.isNullOrUndefined(this.element) && this.element.offsetParent &&
                    this.element.offsetParent.tagName === 'BODY' && getComputedStyle(this.element.offsetParent).overflow !== 'hidden') {
                    this.element.style.top = window.scrollY > parseInt(this.element.style.top, 10) ?
                        (window.scrollY - parseInt(this.element.style.top, 10)) : (parseInt(this.element.style.top, 10) - window.scrollY);
                    this.element.style.position = 'fixed';
                    this.fixedParent = true;
                }
                parent = parent.parentElement;
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.element) && sfBlazorToolkit.base.isNullOrUndefined(this.element.offsetParent) && parentStyle.position === 'fixed'
                    && this.element.style.position === 'fixed') {
                    this.fixedParent = true;
                }
            }
        };
        return Popup;
    }());
    /**
     * Gets scrollable parent elements for the given element.
     *
     * @param { HTMLElement } element - Specify the element to get the scrollable parents of it.
     * @param {boolean} fixedParent - specifies the parent element
     * @private
     * @returns {void}
     */
    function getScrollableParent(element, fixedParent) {
        var eleStyle = getComputedStyle(element);
        var scrollParents = [];
        var overflowRegex = /(auto|scroll)/;
        var parent = element.parentElement;
        while (parent && parent.tagName !== 'HTML') {
            var parentStyle = getComputedStyle(parent);
            if (!(eleStyle.position === 'absolute' && parentStyle.position === 'static')
                && overflowRegex.test(parentStyle.overflow + parentStyle.overflowY + parentStyle.overflowX)) {
                scrollParents.push(parent);
            }
            parent = parent.parentElement;
        }
        if (!fixedParent) {
            scrollParents.push(document);
        }
        return scrollParents;
    }
    /**
     * Gets the maximum z-index of the given element.
     *
     * @returns {void}
     * @param { HTMLElement } element - Specify the element to get the maximum z-index of it.
     * @private
     */
    function getZIndexPartial(element) {
        // upto body traversal
        var parent = element.parentElement;
        var parentZindex = [];
        while (parent) {
            if (parent.tagName !== 'BODY') {
                var index = document.defaultView.getComputedStyle(parent, null).getPropertyValue('z-index');
                var position = document.defaultView.getComputedStyle(parent, null).getPropertyValue('position');
                if (index !== 'auto' && position !== 'static' && parseInt(index, 10) < MAX_Z_INDEX) {
                    parentZindex.push(index);
                }
                parent = parent.parentElement;
            }
            else {
                break;
            }
        }
        var childrenZindex = [];
        Array.prototype.forEach.call(document.body.children, function (child) {
            if (!element.isEqualNode(child)) {
                var index = document.defaultView.getComputedStyle(child, null).getPropertyValue('z-index');
                var position = document.defaultView.getComputedStyle(child, null).getPropertyValue('position');
                if (index !== 'auto' && position !== 'static' && parseInt(index, 10) < MAX_Z_INDEX) {
                    childrenZindex.push(index);
                }
            }
        });
        childrenZindex.push('999');
        var siblingsZindex = [];
        if (!sfBlazorToolkit.base.isNullOrUndefined(element.parentElement) && element.parentElement.tagName !== 'BODY') {
            var childNodes = [].slice.call(element.parentElement.children);
            childNodes.forEach(function (child) {
                if (!element.isEqualNode(child)) {
                    var index = document.defaultView.getComputedStyle(child, null).getPropertyValue('z-index');
                    var position = document.defaultView.getComputedStyle(child, null).getPropertyValue('position');
                    if (index !== 'auto' && position !== 'static' && parseInt(index, 10) < MAX_Z_INDEX) {
                        siblingsZindex.push(index);
                    }
                }
            });
        }
        var finalValue = parentZindex.concat(childrenZindex, siblingsZindex);
        // eslint-disable-next-line
        var currentZindexValue = Math.max.apply(Math, finalValue) + 1;
        return currentZindexValue > MAX_Z_INDEX ? MAX_Z_INDEX : currentZindexValue;
    }
    /**
     * Gets the maximum z-index of the page.
     *
     * @returns {void}
     * @param { HTMLElement } tagName - Specify the tagName to get the maximum z-index of it.
     * @private
     */
    function getMaxZIndex(tagName) {
        if (tagName === void 0) { tagName = ['*']; }
        var maxZindex = [];
        for (var _i2 = 0; _i2 < tagName.length; _i2++) {
            var tag = tagName[_i2];
            var elements = document.getElementsByTagName(tag);
            Array.prototype.forEach.call(elements, function (el) {
                var index = document.defaultView.getComputedStyle(el, null).getPropertyValue('z-index');
                var position = document.defaultView.getComputedStyle(el, null).getPropertyValue('position');
                if (index !== 'auto' && position !== 'static') {
                    maxZindex.push(index);
                }
            });
        }
        // eslint-disable-next-line
        var currentZindexValue = Math.max.apply(Math, maxZindex) + 1;
        return currentZindexValue > MAX_Z_INDEX ? MAX_Z_INDEX : currentZindexValue;
    }

    /**
     *
     * @param {HTMLElement} anchor - specifies the element
     * @param {HTMLElement} element - specifies the element
     * @returns {OffsetPosition} - returns the value
     */
    function calculateRelativeBasedPosition(anchor, element) {
        var fixedElement = false;
        var anchorPos = { left: 0, top: 0 };
        var tempAnchor = anchor;
        if (!anchor || !element) {
            return anchorPos;
        }
        if (sfBlazorToolkit.base.isNullOrUndefined(element.offsetParent) && element.style.position === 'fixed') {
            fixedElement = true;
        }
        while ((element.offsetParent || fixedElement) && anchor && element.offsetParent !== anchor) {
            anchorPos.left += anchor.offsetLeft;
            anchorPos.top += anchor.offsetTop;
            anchor = anchor.offsetParent;
        }
        anchor = tempAnchor;
        while ((element.offsetParent || fixedElement) && anchor && element.offsetParent !== anchor) {
            anchorPos.left -= anchor.scrollLeft;
            anchorPos.top -= anchor.scrollTop;
            anchor = anchor.parentElement;
        }
        return anchorPos;
    }

    /**
     *
     * @param {Element} currentElement - specifies the element
     * @param {string} positionX - specifies the position
     * @param {string} positionY - specifies the position
     * @param {boolean} parentElement - specifies the boolean
     * @param {ClientRect} targetValues - specifies the client
     * @returns {OffsetPosition} - returns the position
     */
    function calculatePosition(currentElement, positionX, positionY, parentElement, targetValues) {
        popupRect = undefined;
        popupRect = targetValues;
        fixedParent = parentElement ? true : false;
        if (!currentElement) {
            return { left: 0, top: 0 };
        }
        if (!positionX) {
            positionX = 'left';
        }
        if (!positionY) {
            positionY = 'top';
        }
        parentDocument = currentElement.ownerDocument;
        element = currentElement;
        var pos = { left: 0, top: 0 };
        return updatePosition(positionX.toLowerCase(), positionY.toLowerCase(), pos);
    }

    /**
     *
     * @param {number} value - specifies the number
     * @param {OffsetPosition} pos - specifies the position
     * @returns {void}
     */
    function setPosX(value, pos) {
        pos.left = value;
    }
    /**
     *
     * @param {number} value - specifies the number
     * @param {OffsetPosition} pos - specifies the position
     * @returns {void}
     */
    function setPosY(value, pos) {
        pos.top = value;
    }
    /**
     *
     * @param {string} posX - specifies the position
     * @param {string} posY - specifies the position
     * @param {OffsetPosition} pos - specifies the position
     * @returns {OffsetPosition} - returns the postion
     */
    function updatePosition(posX, posY, pos) {
        elementRect = element.getBoundingClientRect();
        var key = posY + posX;
        var positionMap = {
            'topcenter': function (p) { setPosX(getElementHCenter(), p); setPosY(getElementTop(), p); },
            'topright': function (p) { setPosX(getElementRight(), p); setPosY(getElementTop(), p); },
            'centercenter': function (p) { setPosX(getElementHCenter(), p); setPosY(getElementVCenter(), p); },
            'centerright': function (p) { setPosX(getElementRight(), p); setPosY(getElementVCenter(), p); },
            'centerleft': function (p) { setPosX(getElementLeft(), p); setPosY(getElementVCenter(), p); },
            'bottomcenter': function (p) { setPosX(getElementHCenter(), p); setPosY(getElementBottom(), p); },
            'bottomright': function (p) { setPosX(getElementRight(), p); setPosY(getElementBottom(), p); },
            'bottomleft': function (p) { setPosX(getElementLeft(), p); setPosY(getElementBottom(), p); },
            'topleft': function (p) { setPosX(getElementLeft(), p); setPosY(getElementTop(), p); }
        };
        var handler = positionMap[key] || positionMap['topleft'];
        handler(pos);
        element = null;
        return pos;
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getBodyScrollTop() {
        return parentDocument.documentElement.scrollTop || parentDocument.body.scrollTop;
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getBodyScrollLeft() {
        return parentDocument.documentElement.scrollLeft || parentDocument.body.scrollLeft;
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getElementBottom() {
        return fixedParent ? elementRect.bottom : elementRect.bottom + getBodyScrollTop();
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getElementVCenter() {
        return getElementTop() + (elementRect.height / 2);
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getElementTop() {
        return fixedParent ? elementRect.top : elementRect.top + getBodyScrollTop();
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getElementLeft() {
        return elementRect.left + getBodyScrollLeft();
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getElementRight() {
        var popupWidth = (element && (((element.classList.contains('e-date-wrapper') || element.classList.contains('e-datetime-wrapper')) && element.classList.contains('e-rtl')) || (element.classList.contains('e-ddl') && element.classList.contains('e-rtl')) || element.classList.contains('e-date-range-wrapper'))) ? (popupRect ? popupRect.width : 0) :
            (popupRect && (elementRect.width >= popupRect.width) ? popupRect.width : 0);
        if (element && element.classList.contains('e-rtl') && element.classList.contains('e-multiselect')) {
            popupWidth = popupRect.width;
        }
        return elementRect.right + getBodyScrollLeft() - popupWidth;
    }
    /**
     * @returns {number} - specifies the number value
     */
    function getElementHCenter() {
        return getElementLeft() + (elementRect.width / 2);
    }

    /**
     * Collision module.
     */
    var parentDocument$1;
    var targetContainer;
    /**
     *
     * @param {HTMLElement} element - specifies the element
     * @param {HTMLElement} viewPortElement - specifies the element
     * @param {CollisionCoordinates} axis - specifies the collision coordinates
     * @param {OffsetPosition} position - specifies the position
     * @returns {void}
     */
    function fit(element, viewPortElement, axis, position) {
        if (viewPortElement === void 0) { viewPortElement = null; }
        if (axis === void 0) { axis = { X: false, Y: false }; }
        if (!axis.Y && !axis.X) {
            return { left: 0, top: 0 };
        }
        var elemData = element.getBoundingClientRect();
        targetContainer = viewPortElement;
        parentDocument$1 = element.ownerDocument;
        if (!position) {
            position = calculatePosition(element, 'left', 'top');
        }
        if (axis.X) {
                var containerWidth = targetContainer ? getTargetContainerWidth() : getViewPortWidth();
                var containerLeft = getContainerLeft();
                var containerRight = getContainerRight();
            var overLeft = containerLeft - position.left;
            var overRight = position.left + elemData.width - containerRight;
            if (elemData.width > containerWidth) {
                if (overLeft > 0 && overRight <= 0) {
                    position.left = containerRight - elemData.width;
                }
                else if (overRight > 0 && overLeft <= 0) {
                    position.left = containerLeft;
                }
                else {
                    position.left = overLeft > overRight ? (containerRight - elemData.width) : containerLeft;
                }
            }
            else if (overLeft > 0) {
                position.left += overLeft;
            }
            else if (overRight > 0) {
                position.left -= overRight;
            }
        }
        if (axis.Y) {
            var containerHeight = targetContainer ? getTargetContainerHeight() : getViewPortHeight();
            var containerTop = getContainerTop();
            var containerBottom = getContainerBottom();
            var overTop = containerTop - position.top;
            var overBottom = position.top + elemData.height - containerBottom;
            if (elemData.height > containerHeight) {
                if (overTop > 0 && overBottom <= 0) {
                    position.top = containerBottom - elemData.height;
                }
                else if (overBottom > 0 && overTop <= 0) {
                    position.top = containerTop;
                }
                else {
                    position.top = overTop > overBottom ? (containerBottom - elemData.height) : containerTop;
                }
            }
            else if (overTop > 0) {
                position.top += overTop;
            }
            else if (overBottom > 0) {
                position.top -= overBottom;
            }
        }
        return position;
    }
    /**
     *
     * @param {HTMLElement} element - specifies the html element
     * @param {HTMLElement} viewPortElement - specifies the html element
     * @param {number} x - specifies the number
     * @param {number} y - specifies the number
     * @returns {string[]} - returns the string value
     */
    function isCollide(element, viewPortElement, x, y) {
        if (viewPortElement === void 0) { viewPortElement = null; }
        var elemOffset = calculatePosition(element, 'left', 'top');
        if (x) {
            elemOffset.left = x;
        }
        if (y) {
            elemOffset.top = y;
        }
        var data = [];
        targetContainer = viewPortElement;
        parentDocument$1 = element.ownerDocument;
        var elementRect = element.getBoundingClientRect();
        var top = elemOffset.top;
        var left = elemOffset.left;
        var right = elemOffset.left + elementRect.width;
        var bottom = elemOffset.top + elementRect.height;
        var yAxis = topCollideCheck(top, bottom);
        var xAxis = leftCollideCheck(left, right);
        if (yAxis.topSide) {
            data.push('top');
        }
        if (xAxis.rightSide) {
            data.push('right');
        }
        if (xAxis.leftSide) {
            data.push('left');
        }
        if (yAxis.bottomSide) {
            data.push('bottom');
        }
        return data;
    }
    /**
     *
     * @param {HTMLElement} element - specifies the element
     * @param {HTMLElement} target - specifies the element
     * @param {number} offsetX - specifies the number
     * @param {number} offsetY - specifies the number
     * @param {string} positionX - specifies the string value
     * @param {string} positionY - specifies the string value
     * @param {HTMLElement} viewPortElement - specifies the element
     * @param {CollisionCoordinates} axis - specifies the collision axis
     * @param {boolean} fixedParent - specifies the boolean
     * @returns {void}
     */
    function flip(element, target, offsetX, offsetY, positionX, positionY, viewPortElement, 
    /* eslint-disable */
    axis, fixedParent) {
        if (viewPortElement === void 0) { viewPortElement = null; }
        if (axis === void 0) { axis = { X: true, Y: true }; }
        if (!target || !element || !positionX || !positionY || (!axis.X && !axis.Y)) {
            return;
        }
        var tEdge = { TL: null,
            TR: null,
            BL: null,
            BR: null
        }, eEdge = {
            TL: null,
            TR: null,
            BL: null,
            BR: null
            /* eslint-enable */
        };
        var elementRect;
        if (window.getComputedStyle(element).display === 'none') {
            var oldVisibility = element.style.visibility;
            element.style.visibility = 'hidden';
            element.style.display = 'block';
            elementRect = element.getBoundingClientRect();
            element.style.removeProperty('display');
            element.style.visibility = oldVisibility;
        }
        else {
            elementRect = element.getBoundingClientRect();
        }
        var pos = {
            posX: positionX, posY: positionY, offsetX: offsetX, offsetY: offsetY, position: { left: 0, top: 0 }
        };
        targetContainer = viewPortElement;
        parentDocument$1 = target.ownerDocument;
        updateElementData(target, tEdge, pos, fixedParent, elementRect);
        setPosition(eEdge, pos, elementRect);
        if (axis.X) {
            leftFlip(target, eEdge, tEdge, pos, elementRect, true);
        }
        if (axis.Y && tEdge.TL.top > -1) {
            topFlip(target, eEdge, tEdge, pos, elementRect, true);
        }
        setPopup(element, pos, elementRect);
    }
    /**
     *
     * @param {HTMLElement} element - specifies the element
     * @param {PositionLocation} pos - specifies the location
     * @param {ClientRect} elementRect - specifies the client rect
     * @returns {void}
     */
    function setPopup(element, pos, elementRect) {
        var left = 0;
        var top = 0;
        if (element.offsetParent != null
            && (getComputedStyle(element.offsetParent).position === 'absolute' ||
                getComputedStyle(element.offsetParent).position === 'relative')) {
            var data = calculatePosition(element.offsetParent, 'left', 'top', false, elementRect);
            left = data.left;
            top = data.top;
        }
        var scaleX = 1;
        var scaleY = 1;
        var tranformElement = getTransformElement(element);
        if (tranformElement) {
            var transformStyle = getComputedStyle(tranformElement).transform;
            if (transformStyle !== 'none') {
                var matrix = new DOMMatrix(transformStyle);
                scaleX = matrix.a;
                scaleY = matrix.d;
            }
            var zoomStyle = getComputedStyle(tranformElement).zoom;
            if (zoomStyle !== 'none') {
                var bodyZoom = getZoomValue(document.body);
                scaleX = bodyZoom * scaleX;
                scaleY = bodyZoom * scaleY;
            }
        }
        element.style.top = ((pos.position.top / scaleY) + pos.offsetY - (top / scaleY)) + 'px';
        element.style.left = ((pos.position.left / scaleX) + pos.offsetX - (left / scaleX)) + 'px';
    }
    function getZoomValue(element) {
        var zoomValue = getComputedStyle(element).zoom;
        return parseFloat(zoomValue) || 1; // Default zoom value is 1 (no zoom)
    }
    /**
     *
     * @param {HTMLElement} element - specifies the element
     * @returns {HTMLElement} The modified element.
     */
    function getTransformElement(element) {
        while (element) {
            var transform = window.getComputedStyle(element).transform;
            var zoom = getZoomValue(document.body);
            if ((transform && transform !== 'none') || (zoom && zoom !== 1)) {
                return element;
            }
            if (element === document.body) {
                return null;
            }
            element = (element.offsetParent || element.parentElement);
        }
        return null;
    }
    /**
     *
     * @param {HTMLElement} target - specifies the element
     * @param {EdgeOffset} edge - specifies the offset
     * @param {PositionLocation} pos - specifies theloaction
     * @param {boolean} fixedParent - specifies the boolean
     * @param {ClientRect} elementRect - specifies the client rect
     * @returns {void}
     */
    function updateElementData(target, edge, pos, fixedParent, elementRect) {
        pos.position = calculatePosition(target, pos.posX, pos.posY, fixedParent, elementRect);
        edge.TL = calculatePosition(target, 'left', 'top', fixedParent, elementRect);
        edge.TR = calculatePosition(target, 'right', 'top', fixedParent, elementRect);
        edge.BR = calculatePosition(target, 'left', 'bottom', fixedParent, elementRect);
        edge.BL = calculatePosition(target, 'right', 'bottom', fixedParent, elementRect);
    }
    /**
     *
     * @param {EdgeOffset} eStatus - specifies the status
     * @param {PositionLocation} pos - specifies the location
     * @param {ClientRect} elementRect - specifies the client
     * @returns {void}
     */
    function setPosition(eStatus, pos, elementRect) {
        eStatus.TL = { top: pos.position.top + pos.offsetY, left: pos.position.left + pos.offsetX };
        eStatus.TR = { top: eStatus.TL.top, left: eStatus.TL.left + elementRect.width };
        eStatus.BL = { top: eStatus.TL.top + elementRect.height,
            left: eStatus.TL.left };
        eStatus.BR = { top: eStatus.TL.top + elementRect.height,
            left: eStatus.TL.left + elementRect.width };
    }
    /**
     *
     * @param {number} left - specifies the  number
     * @param {number} right - specifies the number
     * @returns {LeftCorners} - returns the value
     */
    function leftCollideCheck(left, right) {
        //eslint-disable-next-line
        var leftSide = false, rightSide = false;
        if (((left - getBodyScrollLeft$1()) < getContainerLeft())) {
            leftSide = true;
        }
        if (right > getContainerRight()) {
            rightSide = true;
        }
        return { leftSide: leftSide, rightSide: rightSide };
    }
    /**
     *
     * @param {HTMLElement} target - specifies the element
     * @param {EdgeOffset} edge - specifes the element
     * @param {EdgeOffset} tEdge - specifies the edge offset
     * @param {PositionLocation} pos - specifes the location
     * @param {ClientRect} elementRect - specifies the client
     * @param {boolean} deepCheck - specifies the boolean value
     * @returns {void}
     */
    function leftFlip(target, edge, tEdge, pos, elementRect, deepCheck) {
        var collideSide = leftCollideCheck(edge.TL.left, edge.TR.left);
        if ((tEdge.TL.left - getBodyScrollLeft$1()) <= getContainerLeft()) {
            collideSide.leftSide = false;
        }
        if (tEdge.TR.left > getContainerRight()) {
            collideSide.rightSide = false;
        }
        if ((collideSide.leftSide && !collideSide.rightSide) || (!collideSide.leftSide && collideSide.rightSide)) {
            if (pos.posX === 'right') {
                pos.posX = 'left';
            }
            else {
                pos.posX = 'right';
            }
            pos.offsetX = pos.offsetX + elementRect.width;
            pos.offsetX = -1 * pos.offsetX;
            pos.position = calculatePosition(target, pos.posX, pos.posY, false);
            setPosition(edge, pos, elementRect);
            if (deepCheck) {
                leftFlip(target, edge, tEdge, pos, elementRect, false);
            }
        }
    }
    /**
     *
     * @param {HTMLElement} target - specifies the element
     * @param {EdgeOffset} edge - specifies the offset
     * @param {EdgeOffset} tEdge - specifies the offset
     * @param {PositionLocation} pos - specifies the location
     * @param {ClientRect} elementRect - specifies the client rect
     * @param {boolean} deepCheck - specifies the boolean
     * @returns {void}
     */
    function topFlip(target, edge, tEdge, pos, elementRect, deepCheck) {
        var collideSide = topCollideCheck(edge.TL.top, edge.BL.top);
        if ((tEdge.TL.top - getBodyScrollTop$1()) <= getContainerTop()) {
            collideSide.topSide = false;
        }
        if (tEdge.BL.top >= getContainerBottom() && target.getBoundingClientRect().bottom < window.innerHeight) {
            collideSide.bottomSide = false;
        }
        if ((collideSide.topSide && !collideSide.bottomSide) || (!collideSide.topSide && collideSide.bottomSide)) {
            if (pos.posY === 'top') {
                pos.posY = 'bottom';
            }
            else {
                pos.posY = 'top';
            }
            pos.offsetY = pos.offsetY + elementRect.height;
            pos.offsetY = -1 * pos.offsetY;
            pos.position = calculatePosition(target, pos.posX, pos.posY, false, elementRect);
            setPosition(edge, pos, elementRect);
            if (deepCheck) {
                topFlip(target, edge, tEdge, pos, elementRect, false);
            }
        }
    }
    /**
     *
     * @param {number} top - specifies the number
     * @param {number} bottom - specifies the number
     * @returns {TopCorners} - retyrns the value
     */
    function topCollideCheck(top, bottom) {
        //eslint-disable-next-line
        var topSide = false, bottomSide = false;
        if ((top - getBodyScrollTop$1()) < getContainerTop()) {
            topSide = true;
        }
        if (bottom > getContainerBottom()) {
            bottomSide = true;
        }
        return { topSide: topSide, bottomSide: bottomSide };
    }
    /**
     * @returns {void}
     */
    function getTargetContainerWidth() {
        return targetContainer.getBoundingClientRect().width;
    }
    /**
     * @returns {void}
     */
    function getTargetContainerHeight() {
        return targetContainer.getBoundingClientRect().height;
    }
    /**
     * @returns {void}
     */
    function getTargetContainerLeft() {
        return targetContainer.getBoundingClientRect().left;
    }
    /**
     * @returns {void}
     */
    function getTargetContainerTop() {
        return targetContainer.getBoundingClientRect().top;
    }
    // Container edge helpers (camelCase)
    function getContainerTop() {
        if (targetContainer) {
            return getTargetContainerTop();
        }
        return 0;
    }

    function getContainerLeft() {
        if (targetContainer) {
            return getTargetContainerLeft();
        }
        return 0;
    }

    function getContainerRight() {
        if (targetContainer) {
            return (getBodyScrollLeft$1() + getTargetContainerLeft() + getTargetContainerWidth());
        }
        return (getBodyScrollLeft$1() + getViewPortWidth());
    }

    function getContainerBottom() {
        if (targetContainer) {
            return (getBodyScrollTop$1() + getTargetContainerTop() + getTargetContainerHeight());
        }
        return (getBodyScrollTop$1() + getViewPortHeight());
    }
    /**
     * @returns {number} - returns the scroll top value
     */
    function getBodyScrollTop$1() {
        // if(targetContainer)
        //     return targetContainer.scrollTop;
        return parentDocument$1.documentElement.scrollTop || parentDocument$1.body.scrollTop;
    }
    /**
     * @returns {number} - returns the scroll left value
     */
    function getBodyScrollLeft$1() {
        // if(targetContainer)
        //     return targetContainer.scrollLeft;
        return parentDocument$1.documentElement.scrollLeft || parentDocument$1.body.scrollLeft;
    }
    /**
     * @returns {number} - returns the viewport height
     */
    function getViewPortHeight() {
        return window.innerHeight;
    }
    /**
     * @returns {number} - returns the viewport width
     */
    function getViewPortWidth() {
        var windowWidth = window.innerWidth;
        var documentReact = document.documentElement.getBoundingClientRect();
        var offsetWidth = (sfBlazorToolkit.base.isNullOrUndefined(document.documentElement)) ? 0 : documentReact.width;
        return windowWidth - (windowWidth - offsetWidth);
    }
    /**
     * @returns {void}
     */
    function destroy() {
        targetContainer = null;
        parentDocument$1 = null;
    }

    exports.Popup = Popup;
    exports.PositionData = PositionData;
    exports.getMaxZIndex = getMaxZIndex;
    exports.getScrollableParent = getScrollableParent;
    exports.getZIndexPartial = getZIndexPartial;
    exports.calculateRelativeBasedPosition = calculateRelativeBasedPosition;
    exports.calculatePosition = calculatePosition;
    exports.destroy = destroy;
    exports.fit = fit;
    exports.flip = flip;
    exports.getTransformElement = getTransformElement;
    exports.getZoomValue = getZoomValue;
    exports.isCollide = isCollide;
    return exports;
});

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.popups = window.sfBlazorToolkit.popups || Popup({});