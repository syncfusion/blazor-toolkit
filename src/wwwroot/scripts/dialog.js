"use strict";

var TAB = 9;
var ENTER = 13;
var ESCAPE = 27;
var BTN = 'e-btn';
var HIDE = 'e-hide';
var FADE = 'e-fade';
var ICON = 'e-toolkit-icons';
var POPUP = 'e-popup';
var DIALOG = 'e-dialog';
var DEVICE = 'e-device';
var PRIMARY = 'e-primary';
var DLG_MODAL = 'e-dlg-modal';
var DRAGGABLE = 'e-draggable';
var POPUP_OPEN = 'e-popup-open';
var DLG_TARGET = 'e-dlg-target';
var DLG_CONTENT = 'e-dlg-content';
var DLG_OVERLAY = 'e-dlg-overlay';
var DLG_RESIZABLE = 'e-dlg-resizable';
var DLG_FULLSCREEN = 'e-dlg-fullscreen';
var FOOTER_CONTENT = 'e-footer-content';
var SCROLL_DISABLED = 'e-scroll-disabled';
var DLG_REF_ELEMENT = 'e-dlg-ref-element';
var DLG_RESTRICT_LEFT = 'e-restrict-left';
var DLG_RESIZE_HANDLE = 'e-resize-handle';
var DLG_RESIZE_VIEWPORT = 'e-resize-viewport';
var DLG_CLOSE_ICON_BTN = 'e-dlg-closeicon-btn';
var DLG_HEADER_CONTENT = 'e-dlg-header-content';

var SfDialog = (function () {
    /**
     * Represents a Dialog component used by Blazor via JS interop.
     * @class SfDialog
     */
    /**
     * Constructor for SfDialog
     * @param {{dataId?: string, element?: Element, target?: string|Element, isInitial?: boolean, [key:string]: any}} options - Component options from Blazor
     */
    function SfDialog(options) {
        /* Component variables */
        this.popupObj = null;
        /* Number variables */
        this.zIndex = 1000;
        this.height = 'auto';
        this.width = '100%';
        this.resizeIconDirection = 'SouthEast';
        this.visible = true;
        this.isModal = false;
        this.enableRtl = false;
        this.enableResize = false;
        this.closeOnEscape = true;
        this.allowDragging = false;
        this.allowMaxHeight = true;
        this.allowPrerender = false;
        this.hasFocusableNode = false;
        this.openedEnabled = false;
        this.closedEnabled = false;
        this.onDragEnabled = false;
        this.onDragStartEnabled = false;
        this.onDragStopEnabled = false;
        this.resizingEnabled = false;
        this.onResizeStartEnabled = false;
        this.onResizeStopEnabled = false;
        this.isOpenFullScreen = false;
        this.enablePersistence = false;
        this.isModelResize = false;
        this.dlgContainer = undefined;
        this.position = { X: 'center', Y: 'center' };
        window.sfBlazorToolkit = window.sfBlazorToolkit;
        this.preventFocus = false;
        this.updateContext(options);
        this.isInitialLoad = options.isInitial;
        window.sfBlazorToolkit.base.setCompInstance(this);
        this.initialize();
    }
    /**
     * Update internal context from Blazor-provided object
     * @param {object} dlgObj - Partial dialog properties to merge into this instance
     */
    SfDialog.prototype.updateContext = function (dlgObj) {
        sfBlazorToolkit.base.extend(this, dlgObj);
    };
    /**
     * Initialize the dialog instance: render and bind events when element is present
     */
    SfDialog.prototype.initialize = function () {
        this.calculatezIndex = (this.zIndex === 1000);
        if (this.element instanceof Element || this.element instanceof HTMLElement) {
            this.render();
            this.element.classList.remove('e-blazor-hidden');
            this.setWidth();
            this.setMinHeight();
            if (this.enableResize) {
                this.setResize(true);
                if (this.isModal) {
                    this.isModelResize = true;
                }
                if (this.animationSettings.effect === 'None') {
                    this.getMinHeight();
                }
            }
            this.bindEvent(this.element);
        }
    };
    /**
     * Apply configured width to the dialog element
     */
    SfDialog.prototype.setWidth = function () {
        if (this.width === '100%') {
            this.element.style.width = '';
        }
        else {
            sfBlazorToolkit.base.setStyleAttribute(this.element, { 'width': this.width });
            if (this.width === 'auto') {
                this.refreshPosition();
            }
        }
    };
    /**
     * Apply configured height to the dialog element
     */
    SfDialog.prototype.setHeight = function () {
        sfBlazorToolkit.base.setStyleAttribute(this.element, { 'height': this.height });
    };
    /**
     * Apply configured minHeight to the dialog element
     */
    SfDialog.prototype.setMinHeight = function () {
        if (this.minHeight !== '') {
            sfBlazorToolkit.base.setStyleAttribute(this.element, { 'minHeight': this.minHeight });
        }
    };
    /**
     * Render/setup the dialog DOM and popup wrapper
     */
    SfDialog.prototype.render = function () {
        var _this = this;
        this.checkPositionData();
        this.targetEle = this.getTargetEle(this.target);
        if (sfBlazorToolkit.base.Browser.isDevice) {
            sfBlazorToolkit.base.addClass(this.element, DEVICE);
        }
        if (this.element && sfBlazorToolkit.base.isNullOrUndefined(this.headerContent)) {
            this.headerContent = this.element.querySelector('.' + DLG_HEADER_CONTENT);
        }
        if (this.element && sfBlazorToolkit.base.isNullOrUndefined(this.contentEle)) {
            this.contentEle = this.element.querySelector('.' + DLG_CONTENT);
        }
        this.setMaxHeight();
        if (this.zIndex === 1000) {
            this.setZIndex(this.element, false);
        }
        if (this.allowDragging && (!sfBlazorToolkit.base.isNullOrUndefined(this.headerContent))) {
            this.setAllowDragging();
        }
        if (this.isModal && sfBlazorToolkit.base.isNullOrUndefined(this.dlgContainer)) {
            this.dlgContainer = this.element.parentElement;
            this.dlgOverlay = this.element.parentElement.querySelector('#' + this.dataId + '_overlay');
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.element.parentElement) && (this.allowPrerender ||
            (!this.allowPrerender && (!this.isInitialLoad || (this.visible && this.isInitialLoad))))) {
            var parentEle = this.isModal ? this.dlgContainer.parentElement : this.element.parentElement;
            this.refElement = sfBlazorToolkit.base.createElement('div', { className: DLG_REF_ELEMENT });
            this.refElement.style.display = 'none';
            parentEle.insertBefore(this.refElement, (this.isModal ? this.dlgContainer : this.element));
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.targetEle)) {
            if (this.isModal) {
                this.targetEle.appendChild(this.dlgContainer);
            }
            else {
                this.targetEle.appendChild(this.element);
            }
        }
        this.popupObj = new sfBlazorToolkit.popups.Popup(this.element, {
            height: this.height,
            width: this.width,
            zIndex: this.zIndex,
            relateTo: this.targetEle,
            actionOnScroll: 'none',
            enableRtl: this.enableRtl,
            position: { X: this.position.X, Y: this.position.Y },
            open: function () {
                if (_this.enableResize) {
                    _this.resetResizeIcon();
                }
                if (_this.openedEnabled) {
                    _this.dotNetRef.invokeMethodAsync('OpenEventAsync', _this.element.classList.toString());
                }
                else {
                    _this.focusContent();
                }
            },
            close: function () {
                if (_this.isModal) {
                    sfBlazorToolkit.base.addClass(_this.dlgOverlay, FADE);
                    _this.dlgContainer.style.display = 'none';
                }
                _this.hasFocusableNode = false;
                if (_this.closedEnabled) {
                    _this.dotNetRef.invokeMethodAsync('CloseEventAsync', _this.element.classList.toString());
                }
                _this.popupCloseHandler();
            }
        });
        if (this.isModal) {
            this.positionChange();
        }
    };
    SfDialog.prototype.checkPositionData = function () {
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.position)) {
            if (this.isNumberString(this.position.X)) {
                this.position.X = parseFloat(this.position.X);
            }
            if (this.isNumberString(this.position.Y)) {
                this.position.Y = parseFloat(this.position.Y);
            }
        }
    };
    SfDialog.prototype.isNumberString = function (val) {
        return !sfBlazorToolkit.base.isNullOrUndefined(val) && (typeof (val) !== 'number') && /^[-+]?\d*\.?\d+$/.test(val);
    };
    SfDialog.prototype.getTargetEle = function (target) {
        var targetEle;
        if (!sfBlazorToolkit.base.isNullOrUndefined(target) && (typeof target) === 'string') {
            targetEle = document.querySelector(target);
        }
        return (sfBlazorToolkit.base.isNullOrUndefined(targetEle) ? document.body : targetEle);
    };
    /**
     * Compute and set maxHeight based on target or viewport
     */
    SfDialog.prototype.setMaxHeight = function () {
        if (!this.allowMaxHeight || sfBlazorToolkit.base.isNullOrUndefined(this.element)) {
            return;
        }
        var display = this.element.style.display;
        this.element.style.display = 'none';
        this.element.style.maxHeight = (!sfBlazorToolkit.base.isNullOrUndefined(this.target)) && (this.targetEle.offsetHeight < window.innerHeight) ?
            (this.targetEle.offsetHeight - 20) + 'px' : (window.innerHeight - 20) + 'px';
        this.element.style.display = display;
        if (sfBlazorToolkit.base.Browser.isIE && this.height === 'auto' && !sfBlazorToolkit.base.isNullOrUndefined(this.contentEle)
            && this.element.offsetHeight < this.contentEle.offsetHeight) {
            this.element.style.height = 'inherit';
        }
    };
    /**
     * Calculate and (optionally) set popup z-index
     * @param {Element} zIndexElement - Element to base z-index calculation on
     * @param {boolean} setPopupZindex - When true, update popup object zIndex
     */
    SfDialog.prototype.setZIndex = function (zIndexElement, setPopupZindex) {
        this.zIndex = sfBlazorToolkit.popups.getZIndexPartial(zIndexElement);
        if (setPopupZindex) {
            if (this.popupObj.zIndex !== this.zIndex) {
                this.popupObj.setProperties({ 'zIndex': this.zIndex });
            }
        }
    };
    SfDialog.prototype.updateZIndex = function () {
        this.popupObj.setProperties({ 'zIndex': this.zIndex });
        if (this.isModal) {
            this.setOverlayZindex(this.zIndex);
        }
        this.calculatezIndex = (this.element.style.zIndex !== this.zIndex.toString()) ? false : true;
    };
    /**
     * Update the dialog's target element and adjust related settings
     */
    SfDialog.prototype.updateTarget = function () {
        this.targetEle = this.getTargetEle(this.target);
        this.popupObj.setProperties({ 'relateTo': this.targetEle });
        if (this.dragObj) {
            this.dragObj.dragArea = this.targetEle;
        }
        this.setMaxHeight();
        if (this.isModal) {
            this.targetEle.appendChild(this.dlgContainer);
        }
        if (this.enableResize) {
            this.setResize(false);
        }
    };
    SfDialog.prototype.resetResizeIcon = function () {
        var dialogConHeight = this.getMinHeight();
        if (this.targetEle.offsetHeight < dialogConHeight) {
            var resizeIcon = this.element.querySelector('.' + this.resizeIconDirection);
            if (!sfBlazorToolkit.base.isNullOrUndefined(resizeIcon)) {
                resizeIcon.style.bottom = '-' + dialogConHeight.toString() + 'px';
            }
        }
    };
    SfDialog.prototype.getMouseEvtArgs = function (e) {
        return {
            altKey: e.altKey, button: e.button, buttons: e.buttons, clientX: e.clientX, clientY: e.clientY, ctrlKey: e.ctrlKey,
            detail: e.detail, metaKey: e.metaKey, screenX: e.screenX, screenY: e.screenY, shiftKey: e.shiftKey, type: e.type
        };
    };
    /**
     * Enable draggable behavior using shared Draggable utility
     */
    SfDialog.prototype.setAllowDragging = function () {
        var _this = this;
        var proxy =  this;
        this.dragObj = new sfBlazorToolkit.Draggable(this.element, {
            isDragScroll: true,
            dragArea: sfBlazorToolkit.base.isNullOrUndefined(this.targetEle) ? null : this.targetEle,
            abort: '.' + DLG_CLOSE_ICON_BTN,
            handle: '.' + DLG_HEADER_CONTENT,
            dragStart: function (e) {
                if (_this.onDragStartEnabled) {
                    proxy.dotNetRef.invokeMethodAsync('DragStartEventAsync', { event: proxy.getMouseEvtArgs(e.event) });
                }
            },
            drag: function (e) {
                if (_this.onDragEnabled) {
                    proxy.dotNetRef.invokeMethodAsync('DragEventAsync', { event: proxy.getMouseEvtArgs(e.event) });
                }
            },
            dragStop: function (e) {
                if (proxy.isModal) {
                    if (!sfBlazorToolkit.base.isNullOrUndefined(proxy.position)) {
                        proxy.dlgContainer.classList.remove('e-dlg-' + proxy.position.X + '-' + proxy.position.Y);
                    }
                    var htmlElement = document.querySelector('html');
                    var dirValue = htmlElement.getAttribute('dir');
                    proxy.element.style.position = (!sfBlazorToolkit.base.isNullOrUndefined(dirValue) && dirValue === 'rtl') ? proxy.element.style.position : 'relative';
                }
                if (_this.onDragStopEnabled) {
                    proxy.dotNetRef.invokeMethodAsync('DragStopEventAsync', { event: proxy.getMouseEvtArgs(e.event) });
                }
                _this.isModelResize = false;
                proxy.element.classList.remove(DLG_RESTRICT_LEFT);
                _this.updatePersistData();
            }
        });
    };
    SfDialog.prototype.positionChange = function () {
        if (this.isModal) {
            if (!isNaN(parseFloat(this.position.X)) && !isNaN(parseFloat(this.position.Y))) {
                this.setPopupPosition();
            }
            else if ((!isNaN(parseFloat(this.position.X)) && isNaN(parseFloat(this.position.Y)))
                || (isNaN(parseFloat(this.position.X)) && !isNaN(parseFloat(this.position.Y)))) {
                this.setPopupPosition();
            }
            else {
                this.element.style.top = '0px';
                this.element.style.left = '0px';
                this.dlgContainer.classList.add('e-dlg-' + this.position.X + '-' + this.position.Y);
            }
        }
        else {
            this.setPopupPosition();
        }
    };
    SfDialog.prototype.setPopupPosition = function () {
        this.popupObj.setProperties({ position: { X: this.position.X, Y: this.position.Y } });
    };
    SfDialog.prototype.setEnableRTL = function () {
        if (this.element) {
            var resizeElement = this.element.querySelector('.' + DLG_RESIZE_HANDLE);
            if (!sfBlazorToolkit.base.isNullOrUndefined(resizeElement) && resizeElement.parentElement === this.element) {
                sfBlazorToolkit.Resize.removeResize();
                this.setResize(false);
            }
        }
    };
    /**
     * Enable or disable resize handles and behavior
     * @param {boolean} initialLoad - True when called during initialization
     */
    SfDialog.prototype.setResize = function (initialLoad) {
        if (this.enableResize) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.element.querySelector('.' + ICON + '.' + DLG_RESIZE_HANDLE))) {
                return;
            }
            var computedHeight = getComputedStyle(this.element).minHeight;
            var computedWidth = getComputedStyle(this.element).minWidth;
            if (this.isModal && this.enableRtl) {
                this.element.classList.add(DLG_RESTRICT_LEFT);
            }
            else if (this.isModal && (this.target === document.body || this.target === 'body')) {
                this.element.classList.add(DLG_RESIZE_VIEWPORT);
            }
            this._boundOnResizeStart = this._boundOnResizeStart || this.onResizeStart.bind(this);
            this._boundOnResizeComplete = this._boundOnResizeComplete || this.onResizeComplete.bind(this);
            this._boundOnResizing = this._boundOnResizing || this.onResizing.bind(this);
            sfBlazorToolkit.Resize.createResize({
                element: this.element,
                direction: this.resizeIconDirection,
                minHeight: parseInt(computedHeight.slice(0, computedWidth.indexOf('p')), 10),
                maxHeight: this.targetEle.clientHeight,
                minWidth: parseInt(computedWidth.slice(0, computedWidth.indexOf('p')), 10),
                maxWidth: this.targetEle.clientWidth,
                boundary: (this.target === 'body' || this.target === 'document.body') ? null : this.targetEle,
                resizeBegin: this._boundOnResizeStart,
                resizeComplete: this._boundOnResizeComplete,
                resizing: this._boundOnResizing,
                proxy: this
            });
            this.wireWindowResizeEvent();
        }
        else {
            if (initialLoad) {
                return;
            }
            sfBlazorToolkit.Resize.removeResize();
            this.unWireWindowResizeEvent();
            this._boundOnResizeStart = null;
            this._boundOnResizeComplete = null;
            this._boundOnResizing = null;
            if (this.isModal) {
                this.element.classList.remove(DLG_RESTRICT_LEFT);
            }
            else {
                this.element.classList.remove(DLG_RESIZE_VIEWPORT);
            }
        }
    };
    /**
     * Compute minimum content height (header + footer + padding)
     * @returns {number} Minimum height in pixels
     */
    SfDialog.prototype.getMinHeight = function () {
        var computedHeaderHeight = '0px';
        var computedFooterHeight = '0px';
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.element.querySelector('.' + DLG_HEADER_CONTENT))) {
            computedHeaderHeight = getComputedStyle(this.headerContent).height;
        }
        var footerEle = this.element.querySelector('.' + FOOTER_CONTENT);
        if (!sfBlazorToolkit.base.isNullOrUndefined(footerEle)) {
            computedFooterHeight = getComputedStyle(footerEle).height;
        }
        var headerHeight = parseInt(computedHeaderHeight.slice(0, computedHeaderHeight.indexOf('p')), 10);
        var footerHeight = parseInt(computedFooterHeight.slice(0, computedFooterHeight.indexOf('p')), 10);
        sfBlazorToolkit.Resize.setMinHeight(headerHeight + 30 + (isNaN(footerHeight) ? 0 : footerHeight));
        return (headerHeight + 30 + footerHeight);
    };
    /**
     * Change dialog position and persist the change
     * @param {object} dlgObj - New positioning properties
     */
    SfDialog.prototype.changePosition = function (dlgObj) {
        if (this.isModal && this.dlgContainer.classList.contains('e-dlg-' + this.position.X + '-' + this.position.Y)) {
            this.dlgContainer.classList.remove('e-dlg-' + this.position.X + '-' + this.position.Y);
        }
        this.updateContext(dlgObj);
        this.checkPositionData();
        this.positionChange();
        this.updatePersistData();
    };
    SfDialog.prototype.setOverlayZindex = function (zIndexValue) {
        var zIndex;
        if (sfBlazorToolkit.base.isNullOrUndefined(zIndexValue)) {
            zIndex = parseInt(this.element.style.zIndex, 10) ? parseInt(this.element.style.zIndex, 10) : this.zIndex;
        }
        else {
            zIndex = zIndexValue;
        }
        this.dlgOverlay.style.zIndex = (zIndex - 1).toString();
        this.dlgContainer.style.zIndex = zIndex.toString();
    };
    /**
     * Move focus into the dialog content according to autofocus rules
     */
    SfDialog.prototype.focusContent = function () {
        var element = this.getAutoFocusNode(this.element);
        var node = !sfBlazorToolkit.base.isNullOrUndefined(element) ? element : this.element;
        node.focus();
        this.hasFocusableNode = true;
    };
    /**
     * Find the best candidate node to autofocus inside the dialog
     * @param {Element} container - Dialog container element
     * @returns {Element|null}
     */
    SfDialog.prototype.getAutoFocusNode = function (container) {
        var node = container.querySelector('.' + DLG_CLOSE_ICON_BTN);
        var value = '[autofocus]';
        var items = container.querySelectorAll(value);
        var validNode = this.getValidFocusNode(items);
        this.primaryButtonEle = this.element.getElementsByClassName(PRIMARY)[0];
        if (!sfBlazorToolkit.base.isNullOrUndefined(validNode)) {
            node = validNode;
        }
        else {
            validNode = this.focusableElements(this.contentEle);
            if (!sfBlazorToolkit.base.isNullOrUndefined(validNode)) {
                return node = validNode;
            }
            else if (!sfBlazorToolkit.base.isNullOrUndefined(this.primaryButtonEle)) {
                return this.element.querySelector('.' + PRIMARY);
            }
        }
        return node;
    };
    SfDialog.prototype.getValidFocusNode = function (items) {
        var node;
        for (var u = 0; u < items.length; u++) {
            node = items[u];
            if ((node.clientHeight > 0 || (node.tagName.toLowerCase() === 'a' && node.hasAttribute('href'))) && node.tabIndex > -1 &&
                !node.disabled && !this.disableElement(node, '[disabled],[aria-disabled="true"],[type="hidden"]')) {
                return node;
            }
            else {
                node = null;
            }
        }
        return node;
    };
    SfDialog.prototype.disableElement = function (element, t) {
        var elementMatch = element ? element.matches : null;
        if (elementMatch) {
            for (; element; element = element.parentNode) {
                if (element instanceof Element && elementMatch.call(element, t)) {
                    return element;
                }
            }
        }
        return null;
    };
    SfDialog.prototype.focusableElements = function (content) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(content)) {
            var value = 'input,select,textarea,button,a,[contenteditable="true"],[tabindex]';
            var items = content.querySelectorAll(value);
            return this.getValidFocusNode(items);
        }
        return null;
    };
    /**
     * Return the element's maxHeight style value
     * @returns {string|null}
     */
    SfDialog.prototype.getMaxHeight = function () {
        this.storeActiveElement = document.activeElement;
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.element) && !sfBlazorToolkit.base.isNullOrUndefined(this.element.style)) {
            return this.element.style.maxHeight;
        }
        else {
            return null;
        }
    };
    /**
     * Handle property updates coming from Blazor
     * @param {object} props - Changed properties map
     */
    SfDialog.prototype.onPropertyChanged = function (props) {
        this.updateContext(props);
        for (var _i = 0, _a = Object.keys(props); _i < _a.length; _i++) {
            var key = _a[_i];
            switch (key) {
                case 'width':
                    sfBlazorToolkit.base.setStyleAttribute(this.element, { 'width': this.width });
                    this.refreshPosition();
                    this.updatePersistData();
                    break;
                case 'height':
                    this.setHeight();
                    this.refreshPosition();
                    this.updatePersistData();
                    break;
                case 'minHeight':
                    this.setMinHeight();
                    break;
                case 'target':
                    this.updateTarget();
                    break;
                case 'zIndex':
                    this.updateZIndex();
                    break;
                    break;
                case 'allowDragging':
                    if (this.allowDragging) {
                        this.setAllowDragging();
                    }
                    else {
                        this.destroyDraggable();
                    }
                    break;
                case 'enableRtl':
                    this.setEnableRTL();
                    break;
                case 'enableResize':
                    this.setResize(false);
                    break;
            }
        }
    };
    /**
     * Toggle fullscreen mode for the dialog
     * @param {boolean} enable - True to enter fullscreen, false to exit
     */
    SfDialog.prototype.fullScreen = function (enable) {
        if (enable) {
            sfBlazorToolkit.base.addClass(this.element, DLG_FULLSCREEN);
            if (!this.isModal) {
                this.element.style.top = document.scrollingElement.scrollTop + 'px';
            }
            var display = this.element.style.display;
            this.element.style.display = 'none';
            this.element.style.maxHeight = (!sfBlazorToolkit.base.isNullOrUndefined(this.target)) ? (this.targetEle.offsetHeight) + 'px' : (window.innerHeight) + 'px';
            this.element.style.display = display;
            sfBlazorToolkit.base.addClass(document.body, [DLG_TARGET, SCROLL_DISABLED]);
            if (this.allowDragging && !sfBlazorToolkit.base.isNullOrUndefined(this.dragObj)) {
                this.dragObj.destroy();
                this.dragObj = undefined;
            }
        }
        else {
            sfBlazorToolkit.base.removeClass(this.element, DLG_FULLSCREEN);
            sfBlazorToolkit.base.removeClass(document.body, [DLG_TARGET, SCROLL_DISABLED]);
            if (this.allowDragging && (!sfBlazorToolkit.base.isNullOrUndefined(this.headerContent)) && sfBlazorToolkit.base.isNullOrUndefined(this.dragObj)) {
                this.setAllowDragging();
            }
        }
    };
    /**
     * Show the dialog (opens popup and applies animation)
     * @param {boolean} [isFullScreen] - Whether to open in fullscreen mode
     * @param {string} [maxHeight] - Optional max-height CSS value
     */
    SfDialog.prototype.show = function (isFullScreen, maxHeight) {
        this.isOpenFullScreen = isFullScreen;
        if (!this.element.classList.contains(POPUP_OPEN) || !sfBlazorToolkit.base.isNullOrUndefined(isFullScreen)) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(isFullScreen)) {
                this.fullScreen(isFullScreen);
            }
            if (this.element.style.maxHeight !== maxHeight) {
                this.allowMaxHeight = false;
                this.element.style.maxHeight = maxHeight;
            }
            this.element.tabIndex = -1;
            if (this.isModal && sfBlazorToolkit.base.isNullOrUndefined(this.dlgOverlay)) {
                this.dlgOverlay = this.element.parentElement.querySelector('.' + DLG_OVERLAY);
            }
            if (this.isModal && !sfBlazorToolkit.base.isNullOrUndefined(this.dlgOverlay)) {
                this.dlgOverlay.style.display = 'block';
                this.dlgContainer.style.display = 'flex';
                sfBlazorToolkit.base.removeClass(this.dlgOverlay, FADE);
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.targetEle)) {
                    if (this.targetEle === document.body) {
                        this.dlgContainer.style.position = 'fixed';
                    }
                    else {
                        this.dlgContainer.style.position = 'absolute';
                    }
                    this.dlgOverlay.style.position = 'absolute';
                    this.element.style.position = 'relative';
                    sfBlazorToolkit.base.addClass(this.targetEle, [DLG_TARGET, SCROLL_DISABLED]);
                }
                else {
                    sfBlazorToolkit.base.addClass(document.body, [DLG_TARGET, SCROLL_DISABLED]);
                }
            }
            var openAnimation = sfBlazorToolkit.base.isNullOrUndefined(this.animationSettings) ? null : {
                name: this.animationSettings.effect === 'None' ? 'Fade' : this.animationSettings.effect + 'In',
                duration: this.animationSettings.duration,
                delay: this.animationSettings.delay
            };
            var zIndexElement = (this.isModal) ? this.element.parentElement : this.element;
            if (this.calculatezIndex) {
                this.setZIndex(zIndexElement, true);
                sfBlazorToolkit.base.setStyleAttribute(this.element, { 'zIndex': this.zIndex });
                if (this.isModal) {
                    this.setOverlayZindex(this.zIndex);
                }
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(this.animationSettings) || this.animationSettings.effect === 'None') {
                this.popupObj.show();
            }
            else {
                this.popupObj.show(openAnimation);
            }
        }
    };
    /**
     * Hide the dialog and cleanup transient DOM state
     * @returns {string} element classList string after hiding
     */
    SfDialog.prototype.hide = function () {
        if (this.isModal || this.isOpenFullScreen) {
            sfBlazorToolkit.base.removeClass(this.targetEle, [DLG_TARGET, SCROLL_DISABLED]);
            sfBlazorToolkit.base.removeClass(document.body, [DLG_TARGET, SCROLL_DISABLED]);
        }
        var closeAnimation = sfBlazorToolkit.base.isNullOrUndefined(this.animationSettings) ? null : {
            name: this.animationSettings.effect === 'None' ? 'Fade' : this.animationSettings.effect + 'Out',
            duration: this.animationSettings.duration,
            delay: this.animationSettings.delay
        };
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.popupObj)) {
            if (sfBlazorToolkit.base.isNullOrUndefined(this.animationSettings) || this.animationSettings.effect === 'None') {
                this.popupObj.hide();
            }
            else {
                this.popupObj.hide(closeAnimation);
            }
        }
        if (!this.allowPrerender) {
            this.destroyRefElement(this.isModal);
        }
        return this.element.classList.toString();
    };
    SfDialog.prototype.refreshPosition = function () {
        if (this.isModal && isNaN(parseFloat(this.position.X)) && isNaN(parseFloat(this.position.Y))) {
            this.element.style.top = this.element.style.left = '0px';
            this.dlgContainer.classList.add('e-dlg-' + this.position.X + '-' + this.position.Y);
        }
        else {
            this.popupObj.refreshPosition();
        }
    };
    SfDialog.prototype.getDimension = function () {
        var dialogWidth = this.element.offsetWidth;
        var dialogHeight = this.element.offsetHeight;
        return { width: dialogWidth, height: dialogHeight };
    };
    SfDialog.prototype.destroyDraggable = function () {
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.dragObj)) {
            this.dragObj.destroy();
            this.dragObj = undefined;
        }
    };
    SfDialog.prototype.destroyRefElement = function (isModel) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.refElement) && !sfBlazorToolkit.base.isNullOrUndefined(this.refElement.parentElement)) {
            if (isModel && this.dlgContainer && this.dlgContainer.querySelector('.e-dlg-overlay')) {
                this.dlgContainer.querySelector('.e-dlg-overlay').style.display = 'none';
            }
            this.refElement.parentElement.insertBefore((this.isModal ? this.dlgContainer : this.element), this.refElement);
            sfBlazorToolkit.base.detach(this.refElement);
            this.refElement = undefined;
        }
    };
    /**
     * Destroy the dialog instance and remove event listeners/DOM references
     * @param {object} dlgObj - Optional object with instance details
     * @param {boolean} isClientApp - Whether running in client app mode
     */
    SfDialog.prototype.destroy = function (dlgObj, isClientApp) {
        if (this.element instanceof Element || this.element instanceof HTMLElement) {
            this.updateContext(dlgObj);
            var attrs = ['role', 'aria-modal', 'aria-labelledby', 'aria-describedby', 'aria-grabbed', 'tabindex', 'style'];
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.cssClass) && this.cssClass !== '') {
                var classes = this.cssClass.split(' ');
                sfBlazorToolkit.base.removeClass(this.element, classes);
            }
            if (sfBlazorToolkit.base.Browser.isDevice) {
                sfBlazorToolkit.base.removeClass(this.element, DEVICE);
            }
            sfBlazorToolkit.base.removeClass(this.getTargetEle(this.target), [DLG_TARGET, SCROLL_DISABLED]);
            this.unBindEvent(this.element);
            if (this.element.classList.contains(DLG_FULLSCREEN)) {
                sfBlazorToolkit.base.removeClass(document.body, [DLG_TARGET, SCROLL_DISABLED]);
            }
            if (this.isModal) {
                sfBlazorToolkit.base.removeClass((!sfBlazorToolkit.base.isNullOrUndefined(this.targetEle) ? this.targetEle : document.body), SCROLL_DISABLED);
            }
            if (this.element.classList.contains(DLG_RESIZABLE)) {
                this.element.classList.remove(DLG_RESIZABLE);
            }
            if (this.element.classList.contains(DRAGGABLE)) {
                this.dragObj.destroy();
                this.dragObj = undefined;
            }
            if (this.element.classList.contains(POPUP)) {
                this.popupObj.destroy();
                this.popupObj = undefined;
            }
            this.destroyRefElement(this.isModal);
            if (this.dlgOverlay) {
                sfBlazorToolkit.base.detach(this.dlgOverlay);
                this.dlgOverlay = null;
            }
            if (this.enableResize) {
                this.unWireWindowResizeEvent();
            }
            if (this.contentEle) {
                sfBlazorToolkit.base.detach(this.contentEle);
                this.contentEle = null;
            }
            if (this.headerContent) {
                sfBlazorToolkit.base.detach(this.headerContent);
                this.headerContent = null;
            }
            this.dlgContainer = null;
            if (isClientApp) {
                for (var i = 0; i < this.element.children.length; i++) {
                    this.element.children[i].classList.add(HIDE);
                }
            }
            else {
                while (this.element.firstChild) {
                    sfBlazorToolkit.base.detach(this.element.firstChild);
                }
            }
            for (var i = 0; i < attrs.length; i++) {
                this.element.removeAttribute(attrs[i]);
            }
            if (this.isModal) {
                if (this.element && this.element.nextElementSibling) {
                    sfBlazorToolkit.base.detach(this.element.nextElementSibling);
                    var parent_1 = this.element.parentElement;
                    parent_1.removeAttribute('class');
                    parent_1.removeAttribute('style');
                }
                this.element.classList.remove(DLG_MODAL);
            }
            this.element.classList.remove(DIALOG);
            if (this.primaryButtonEle) {
                sfBlazorToolkit.base.detach(this.primaryButtonEle);
                this.primaryButtonEle = null;
            }
            sfBlazorToolkit.Resize.resizeDestroy();
            if (this.element) {
                this.element = null;
            }
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        window.sfBlazorToolkit.base.disposeWindowsInstance(this.dataId);
    };
    SfDialog.prototype.bindEvent = function (element) {
        sfBlazorToolkit.base.EventHandler.add(element, 'keydown', this.keyDown, this);
        if (sfBlazorToolkit.base.Browser.isDevice) {
            sfBlazorToolkit.base.EventHandler.add(window, 'orientationchange', this.orientationOnChange, this);
        }
    };
    SfDialog.prototype.unBindEvent = function (element) {
        sfBlazorToolkit.base.EventHandler.remove(element, 'keydown', this.keyDown);
        if (sfBlazorToolkit.base.Browser.isDevice) {
            sfBlazorToolkit.base.EventHandler.remove(window, 'orientationchange', this.orientationOnChange);
        }
    };
    SfDialog.prototype.wireWindowResizeEvent = function () {
        this._boundWindowResizeHandler = this.windowResizeHandler.bind(this);
        window.addEventListener('resize', this._boundWindowResizeHandler);
    };
    SfDialog.prototype.unWireWindowResizeEvent = function () {
        if (this._boundWindowResizeHandler) {
            window.removeEventListener('resize', this._boundWindowResizeHandler);
            this._boundWindowResizeHandler = null;
        }
    };
    SfDialog.prototype.orientationOnChange = function () {
        var proxy =  this;
        this.element.style.display = 'none';
        setTimeout(function () {
            sfBlazorToolkit.Resize.setMaxWidth(proxy.targetEle.clientWidth);
            sfBlazorToolkit.Resize.setMaxHeight(proxy.targetEle.clientHeight);
            proxy.setMaxHeight();
            proxy.refreshPosition();
            proxy.element.style.display = '';
        }, 250);
    };
    /* Event handlers begin */
    SfDialog.prototype.popupCloseHandler = function () {
        var activeEle = document.activeElement;
        if (!this.preventFocus && !sfBlazorToolkit.base.isNullOrUndefined(activeEle) && !sfBlazorToolkit.base.isNullOrUndefined(activeEle.blur)) {
            activeEle.blur();
        }
        if (!this.preventFocus && !sfBlazorToolkit.base.isNullOrUndefined(this.storeActiveElement) && !sfBlazorToolkit.base.isNullOrUndefined(this.storeActiveElement.focus)) {
            this.storeActiveElement.focus();
        }
    };
    SfDialog.prototype.windowResizeHandler = function () {
        sfBlazorToolkit.Resize.setMaxWidth(this.targetEle.clientWidth);
        sfBlazorToolkit.Resize.setMaxHeight(this.targetEle.clientHeight);
        this.setMaxHeight();
    };
    SfDialog.prototype.onResizeStart = function (args, dialogObj) {
        var evtArgs = dialogObj.getMouseEvtArgs(args);
        if (dialogObj.onResizeStartEnabled) {
            dialogObj.dotNetRef.invokeMethodAsync('ResizeStartEventAsync', evtArgs);
        }
        if (this.isModelResize && !sfBlazorToolkit.base.isNullOrUndefined(this.dlgContainer) && this.dlgContainer.classList.contains('e-dlg-' + this.position.X + '-' + this.position.Y)) {
            this.setPopupPosition();
            this.dlgContainer.classList.remove('e-dlg-' + this.position.X + '-' + this.position.Y);
            var htmlElement = document.querySelector('html');
            var dirValue = htmlElement.getAttribute('dir');
            this.element.style.position = (!sfBlazorToolkit.base.isNullOrUndefined(dirValue) && dirValue === 'rtl') ? this.element.style.position : 'relative';
            if (this.element.classList.contains(DLG_RESTRICT_LEFT)) {
                this.element.classList.remove(DLG_RESTRICT_LEFT);
            }
            this.isModelResize = false;
        }
    };
    SfDialog.prototype.onResizing = function (args, dialogObj) {
        if (dialogObj.resizingEnabled) {
            dialogObj.dotNetRef.invokeMethodAsync('ResizingEventAsync', dialogObj.getMouseEvtArgs(args));
        }
    };
    SfDialog.prototype.onResizeComplete = function (args, dialogObj) {
        if (dialogObj.onResizeStopEnabled) {
            dialogObj.dotNetRef.invokeMethodAsync('ResizeStopEventAsync', dialogObj.getMouseEvtArgs(args));
        }
        dialogObj.updatePersistData();
    };
    SfDialog.prototype.updatePersistData = function () {
        if (this.enablePersistence) {
            window.localStorage.setItem(this.dataId, JSON.stringify({
                width: this.element.style.width,
                height: this.element.style.height, X: parseFloat(this.element.style.left), Y: parseFloat(this.element.style.top)
            }));
        }
    };
    SfDialog.prototype.getFocusElement = function (target) {
        var value = 'input,select,textarea,button:enabled,a,[contenteditable="true"],[tabindex]';
        var items = target.querySelectorAll(value);
        return items[items.length - 1];
    };
    SfDialog.prototype.keyDown = function (e) {
        var _this = this;
        if (e.keyCode === TAB && this.isModal) {
            var btn = void 0;
            var btns = void 0;
            var footer = this.element.querySelector('.' + FOOTER_CONTENT);
            if (!sfBlazorToolkit.base.isNullOrUndefined(footer)) {
                btns = footer.querySelectorAll('button');
                if (!sfBlazorToolkit.base.isNullOrUndefined(btn) && btns.length > 0) {
                    btn = btns[btns.length - 1];
                }
                if (sfBlazorToolkit.base.isNullOrUndefined(btn) && footer.childNodes.length > 0) {
                    btn = this.getFocusElement(footer);
                }
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(footer) && !sfBlazorToolkit.base.isNullOrUndefined(this.contentEle)) {
                btn = this.getFocusElement(this.contentEle);
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(btn) && document.activeElement === btn && !e.shiftKey) {
                e.preventDefault();
                this.focusableElements(this.element).focus();
            }
            if (document.activeElement === this.focusableElements(this.element) && e.shiftKey) {
                e.preventDefault();
                if (!sfBlazorToolkit.base.isNullOrUndefined(btn)) {
                    btn.focus();
                }
            }
        }
        if (e.keyCode === ESCAPE && this.closeOnEscape) {
            // 'document.querySelector' is used to find the elements rendered based on body
            if (!document.querySelector('.e-popup-open:not(.e-dialog, .e-tooltip-wrap, .e-toolbar-pop)')) {
                this.dotNetRef.invokeMethodAsync('CloseDialogAsync', {
                    altKey: e.altKey, ctrlKey: e.ctrlKey, code: e.code, key: e.key, location: e.location,
                    repeat: e.repeat, shiftKey: e.shiftKey, metaKey: e.metaKey, type: e.type
                });
            }
        }
        if (this.hasFocusableNode) {
            var element = document.activeElement;
            var isTagName = (['input', 'textarea'].indexOf(element.tagName.toLowerCase()) > -1);
            var isContentEdit = false;
            if (!isTagName) {
                isContentEdit = element.hasAttribute('contenteditable') && element.getAttribute('contenteditable') === 'true';
            }
            var isCalendarInput = (element.classList.contains('e-datepicker') || element.classList.contains('e-datetimepicker'));
            var timeout = isCalendarInput ? 100 : 0;
            if ((e.keyCode === ENTER && !e.ctrlKey && element.tagName.toLowerCase() !== 'textarea' &&
                isTagName && !sfBlazorToolkit.base.isNullOrUndefined(this.primaryButtonEle)) || (e.keyCode === ENTER && e.ctrlKey &&
                (element.tagName.toLowerCase() === 'textarea' || isContentEdit)) && !sfBlazorToolkit.base.isNullOrUndefined(this.primaryButtonEle)) {
                setTimeout(function () {
                    var primaryButton = _this.element.querySelector('.' + FOOTER_CONTENT + ' button.' + BTN + '.' + PRIMARY);
                    if (primaryButton !== null) {
                        primaryButton.click();
                    }
                }, timeout);
            }
        }
    };
    return SfDialog;
}());

// Exported module functions (use window.sfBlazorToolkit.base for instance storage)
/**
 * Initialize a Dialog instance.
 * @param {{dataId: string, [key: string]: any}} options - Dialog options object containing at least `dataId`.
 */
export function initialize(options) {
    if (options && options.dataId) {
        new SfDialog(options);
    }
}

/**
 * Return class list string for a DOM element.
 * @param {Element} element - Target DOM element.
 * @returns {string|undefined} Space-separated class list or undefined.
 */
export function getClassList(element) {
    if (element && element.classList) {
        return element && element.classList.toString();
    }
    return "";
}

/**
 * Get the maxHeight style value for a dialog instance.
 * @param {string} dataId - Component instance id.
 * @returns {string|null} maxHeight CSS value or null.
 */
export function getMaxHeight(dataId) {
    const instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.getMaxHeight === 'function') {
        return instance.getMaxHeight();
    }
    return null;
}

/**
 * Update dialog position from outside (interop)
 * @param {{dataId: string, [key: string]: any}} dlgObj - Object containing `dataId` and new position data.
 */
export function changePosition(dlgObj) {
    if (dlgObj && dlgObj.dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(dlgObj.dataId);
        if (instance && typeof instance.changePosition === 'function') {
            instance.changePosition(dlgObj);
        }
    }
}

/**
 * Update dialog animation or related properties.
 * @param {{dataId: string, [key: string]: any}} dlgObj - Changed properties.
 */
export function updateAnimation(dlgObj) {
    if (dlgObj && dlgObj.dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(dlgObj.dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.updateContext === 'function') {
            instance.updateContext(dlgObj);
        }
    }
}

/**
 * Focus the dialog's primary focusable content.
 * @param {string} dataId - Component instance id.
 */
export function focusContent(dataId) {
    if (dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.focusContent === 'function') {
            instance.focusContent();
        }
    }
}

/**
 * Refresh dialog popup position.
 * @param {string} dataId - Component instance id.
 */
export function refreshPosition(dataId) {
    if (dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.refreshPosition === 'function') {
            instance.refreshPosition();
        }
    }
}

/**
 * Get current dialog dimensions.
 * @param {string} dataId - Component instance id.
 * @returns {{width:number,height:number}|null}
 */
export function getDimension(dataId) {
    const instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.getDimension === 'function') {
        return instance.getDimension();
    }
    return null;
}

/**
 * Invoke the dialog's popup close handler (useful for interop).
 * @param {string} dataId - Component instance id.
 */
export function popupCloseHandler(dataId) {
    if (dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.popupCloseHandler === 'function') {
            instance.popupCloseHandler();
        }
    }
}

/**
 * Notify an instance about changed properties.
 * @param {{dataId:string, [key:string]: any}} changedProps - Changed properties object including dataId.
 */
export function propertyChanged(changedProps) {
    if (changedProps && changedProps.dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(changedProps.dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.onPropertyChanged === 'function') {
            instance.onPropertyChanged(changedProps);
        }
    }
}

// Alias to match maskedtextbox naming
/**
 * Alias for `propertyChanged` to match other modules' naming.
 * @param {{dataId:string, [key:string]: any}} changedProps
 */
export function propertyChange(changedProps) {
    return propertyChanged(changedProps);
}

/**
 * Show the dialog instance.
 * @param {{dataId:string, fullScreen?:boolean, maxHeight?:string}} options
 */
export function show(options) {
    if (options && options.dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(options.dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.show === 'function') {
            instance.show(options.fullScreen, options.maxHeight);
        }
    }
}

/**
 * Hide the dialog instance.
 * @param {string} dataId - Component instance id.
 * @param {boolean} [preventFocus=false] - When true, do not restore focus to the previously focused element.
 * @returns {string|null} The element's classList string after hiding, or null.
 */
export function hide(dataId, preventFocus) {
    const instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.hide === 'function') {
        instance.preventFocus = preventFocus;
        return instance.hide();
    }
    return null;
}

/**
 * Destroy/cleanup a dialog instance.
 * @param {{dataId:string, isClient?:boolean}} dlgObj
 */
export function destroy(dlgObj) {
    if (dlgObj && dlgObj.dataId) {
        const instance = window.sfBlazorToolkit.base.getCompInstance(dlgObj.dataId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(instance) && typeof instance.destroy === 'function') {
            instance.destroy(dlgObj, dlgObj.isClient);
        }
    }
}

