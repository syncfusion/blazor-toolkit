window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.Draggable = (function () {
    'use strict';

    /**
     * Safe query selector helper
     * @param {string} selector
     * @param {Element|Document} [root]  
     * @returns {Element|null}
     */
    const select = (selector, root) => {
        try {
            return (root || document).querySelector(selector);
        } catch (err) {
            return null;
        }
    };

    // Module-level constants
    const DEFAULTS = {
        DISTANCE: 1,
        TAP_HOLD_THRESHOLD: 750,
        SCOPE: 'default'
    };

    const EVENT = {
        TOUCH_TYPE: 'touch',
        MOUSE_DOWN: 'mousedown',
        TOUCH_START: 'touchstart',
        MOUSE_MOVE: 'mousemove',
        TOUCH_MOVE: 'touchmove',
        MOUSE_UP: 'mouseup',
        TOUCH_END: 'touchend'
    };

    const getDocumentSize = (kind) => {
        const body = document.body || {};
        const documentElement = document.documentElement || {};
        return Math.max(
            body['scroll' + kind] || 0, documentElement['scroll' + kind] || 0,
            body['offset' + kind] || 0, documentElement['offset' + kind] || 0, documentElement['client' + kind] || 0
        );
    };

    const getCoords = (evt) => {
        if (evt && evt.type && evt.type.includes(EVENT.TOUCH_TYPE)) {
            const touchPoint = (evt.changedTouches && evt.changedTouches[0]) || (evt.touches && evt.touches[0]);
            if (!touchPoint) return { pageX: 0, pageY: 0, clientX: 0, clientY: 0 };
            return { pageX: touchPoint.pageX, pageY: touchPoint.pageY, clientX: touchPoint.clientX, clientY: touchPoint.clientY };
        }
        return { pageX: evt.pageX, pageY: evt.pageY, clientX: evt.clientX, clientY: evt.clientY };
    };

    // Small helpers to batch add/remove event listeners via sfBlazorToolkit's EventHandler
    const _addEvents = (el, names, handler, context) => {
        const list = Array.isArray(names) ? names : ('' + names).split(/\s+/);
        for (let i = 0; i < list.length; i++) {
            sfBlazorToolkit.base.EventHandler.add(el, list[i], handler, context);
        }
    };

    const _removeEvents = (el, names, handler, context) => {
        const list = Array.isArray(names) ? names : ('' + names).split(/\s+/);
        for (let i = 0; i < list.length; i++) {
            sfBlazorToolkit.base.EventHandler.remove(el, list[i], handler, context);
        }
    };

    const dragGuard = { active: false };

    /**
     * Draggable constructor
     * @param {HTMLElement} element
     * @param {object} options
     */
    function Draggable(element, options) {
        if (!element) throw new Error('Draggable: element is required');

        // Input validation for callbacks
        const callbacks = ['dragStart', 'drag', 'dragStop'];
        if (options) {
            callbacks.forEach(cb => {
                if (options[cb] && typeof options[cb] !== 'function') {
                    throw new Error(`Draggable: ${cb} must be a function`);
                }
            });
        }

        this.element = element;
        this.options = sfBlazorToolkit.base.extend({
            dragArea: null,
            isDragScroll: false,
            distance: DEFAULTS.DISTANCE,
            handle: null,
            abort: null,
            helper: null,
            scope: DEFAULTS.SCOPE,
            enableTapHold: false,
            tapHoldThreshold: DEFAULTS.TAP_HOLD_THRESHOLD,
            // Events
            dragStart: null,
            drag: null,
            dragStop: null
        }, options || {});

        // State
        this._startTarget = null; // initial event target
        this._dragHelper = null; // helper element used while dragging
        this._initialPoint = { x: 0, y: 0 };
        this._currentPosition = { left: 0, top: 0 };
        this._cursorDiff = { x: 0, y: 0 };
        this._parentBoundingRect = { left: 0, top: 0 };
        this._parentScrollOffset = { x: 0, y: 0 };
        this._margin = { left: 0, top: 0, right: 0, bottom: 0 };
        this._dragLimits = { left: 0, top: 0, right: 0, bottom: 0 };
        this._border = { left: 0, top: 0, right: 0, bottom: 0 };
        this._padding = { left: 0, top: 0, right: 0, bottom: 0 };
        this._previousPosition = { left: 0, top: 0 };
        this._dragging = false;
        this._externalInit = false;
        this._tapHoldTimer = 0;
        this._hoverElement = null;
        this._lastPageX = 0;
        this._lastPageY = 0;

        // droppables cache per scope
        this._droppablesCache = {};
        this._droppablesCache[this.options.scope] = {};

        this._bindStart();
    }

    /**
     * Destroy instance and remove all listeners/refs to prevent leaks
     */
    Draggable.prototype.destroy = function () {
        this._unbindStart();
        this._detachActive();
        // ensure any pre-drag listeners or timers are cleared
        if (typeof this._clearTapTimer === 'function') this._clearTapTimer();
        if (typeof this._detachPreDrag === 'function') this._detachPreDrag();
        sfBlazorToolkit.base.removeClass(document.body, 'e-prevent-select');
        if (this.element) this.element.setAttribute('aria-grabbed', 'false');
        this._dragHelper = null;
        this._hoverElement = null;
        this._droppablesCache = {};
        this._startHandler = null;
    };

    Draggable.prototype.setOptions = function (opts) {
        if (!opts) return;
        sfBlazorToolkit.base.extend(this.options, opts);
    };

    // Internal: bind/unbind initial start events
    Draggable.prototype._bindStart = function () {
        sfBlazorToolkit.base.addClass(this.element, 'e-draggable');
        const startHandler = this.options.enableTapHold ? this._onTapStart.bind(this) : this._onStart.bind(this);
        this._startHandler = startHandler;
        // Listen for both mouse and touchstart to support hybrid devices
        this._startEventNames = [EVENT.MOUSE_DOWN, EVENT.TOUCH_START];

        const handleElement = this.options.handle ? select(this.options.handle, this.element) : null;
        const bindElement = handleElement || this.element;
        _addEvents(bindElement, this._startEventNames, this._startHandler);
    };

    Draggable.prototype._unbindStart = function () {
        if (!this._startHandler) return;
        const handleElement = this.options.handle ? select(this.options.handle, this.element) : null;
        const bindElement = handleElement || this.element;
        _removeEvents(bindElement, this._startEventNames, this._startHandler);
        this._startHandler = null;
    };

    // Tap-hold flow
    Draggable.prototype._onTapStart = function (evt) {
        this._tapHoldTimer = window.setTimeout(() => {
            this._externalInit = true;
            this._clearTapTimer();
            this._onStart(evt);
        }, this.options.tapHoldThreshold);
        this._clearTapTimerHandler = this._clearTapTimer;
        _addEvents(document, [EVENT.TOUCH_MOVE, EVENT.TOUCH_END], this._clearTapTimerHandler, this);
    };

    Draggable.prototype._clearTapTimer = function () {
        if (this._tapHoldTimer) window.clearTimeout(this._tapHoldTimer);
        if (this._clearTapTimerHandler) {
            _removeEvents(document, [EVENT.TOUCH_MOVE, EVENT.TOUCH_END], this._clearTapTimerHandler, this);
            this._clearTapTimerHandler = null;
        }
        this._tapHoldTimer = 0;
    };

    Draggable.prototype._onStart = function (evt) {
        if (dragGuard.active) return; // another drag in progress
        const target = evt.currentTarget || evt.target;
        // Abort selectors
        if (this.options.abort) {
            const aborts = Array.isArray(this.options.abort) ? this.options.abort : [this.options.abort];
            for (let i = 0; i < aborts.length; i++) {
                if (sfBlazorToolkit.base.closest(evt.target, aborts[i])) return;
            }
        }
        if (evt.type !== EVENT.MOUSE_DOWN) {
            if (typeof evt.preventDefault === 'function') evt.preventDefault();
        }

        this.element.setAttribute('aria-grabbed', 'true');
        const coords = getCoords(evt);
        this._initialPoint = { x: coords.pageX, y: coords.pageY };
        this._startTarget = target;
        this._dragging = false;
        this._externalInit = !!this._externalInit; // set by tap-hold
        const rect = this.element.getBoundingClientRect();
        this._getScrollableValues();
        if (evt.clientX === evt.pageX) this._parentScrollOffset.x = 0;
        if (evt.clientY === evt.pageY) this._parentScrollOffset.y = 0;
        this._relativeX = coords.pageX - (rect.left + this._parentScrollOffset.x);
        this._relativeY = coords.pageY - (rect.top + this._parentScrollOffset.y);
        this._preDragHandler = (eventObj) => { this._onPreDrag(eventObj); };
        this._cancelHandler = (eventObj) => { this._onCancel(eventObj); };
        _addEvents(document, [EVENT.MOUSE_MOVE, EVENT.TOUCH_MOVE], this._preDragHandler);
        _addEvents(document, [EVENT.MOUSE_UP, EVENT.TOUCH_END], this._cancelHandler);
        this._unbindStart();
        if (evt.type !== EVENT.TOUCH_START) sfBlazorToolkit.base.addClass(document.body, 'e-prevent-select');
        this._externalInit = false;
    };

    Draggable.prototype._onPreDrag = function (evt) {
        this._clearTapTimer();
        const isTouch = !!(evt.changedTouches);
        if (isTouch && evt.changedTouches.length !== 1) return;
        const coords = getCoords(evt);
        const deltaX = this._initialPoint.x - coords.pageX;
        const deltaY = this._initialPoint.y - coords.pageY;
        const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
        if (distance >= (this.options.distance || 0) || this._externalInit) {
            if (isTouch) evt.preventDefault();
            this._beginDrag(evt);
        }
    };

    Draggable.prototype._onCancel = function (evt) {
        this._detachPreDrag();
        this._bindStart();
        sfBlazorToolkit.base.removeClass(document.body, 'e-prevent-select');
        this.element.setAttribute('aria-grabbed', 'false');
    };

    Draggable.prototype._detachPreDrag = function () {
        if (this._preDragHandler) {
            _removeEvents(document, [EVENT.MOUSE_MOVE, EVENT.TOUCH_MOVE], this._preDragHandler);
        }
        if (this._cancelHandler) {
            _removeEvents(document, [EVENT.MOUSE_UP, EVENT.TOUCH_END], this._cancelHandler);
        }
        this._preDragHandler = null;
        this._cancelHandler = null;
    };

    Draggable.prototype._beginDrag = function (evt) {
        this._detachPreDrag();
        dragGuard.active = true;
        const style = window.getComputedStyle(this.element);
        this._margin = {
            left: parseInt(style.marginLeft, 10) || 0,
            top: parseInt(style.marginTop, 10) || 0,
            right: parseInt(style.marginRight, 10) || 0,
            bottom: parseInt(style.marginBottom, 10) || 0
        };
        let dragElement = this.element;
        if (typeof this.options.helper === 'function') {
            dragElement = this.options.helper({ element: this.element, sender: this });
        }
        if (!dragElement) { this._resetAfterStop(); return; }
        this._dragHelper = dragElement;
        this._parentBoundingRect = this._calcParentRect(dragElement.offsetParent || document.body);
        if (this.options.dragArea) {
            this._computeDragArea();
        } else {
            this._dragLimits = { left: 0, top: 0, right: 0, bottom: 0 };
            this._border = { left: 0, top: 0, right: 0, bottom: 0 };
            this._padding = { left: 0, top: 0, right: 0, bottom: 0 };
        }
        // Use page coordinates to compute a stable cursor offset and initial position
        const startCoords = getCoords(evt);
        const pageX = startCoords.pageX; const pageY = startCoords.pageY;
        const elementPageLeft = this._offsetLeft(this.element);
        const elementPageTop = this._offsetTop(this.element);
        // cursor offset inside the element (page coordinates)
        this._cursorDiff.x = pageX - elementPageLeft;
        this._cursorDiff.y = pageY - elementPageTop;
        // compute initial position relative to the offset parent
        const computedLeft = elementPageLeft - this._parentBoundingRect.left;
        const computedTop = elementPageTop - this._parentBoundingRect.top;
        const processed = { left: (computedLeft) + 'px', top: (computedTop) + 'px' };
        sfBlazorToolkit.base.setStyleAttribute(dragElement, sfBlazorToolkit.base.extend({ position: 'absolute' }, processed));
        // store previous position (page coordinates) so moves don't jump
        this._previousPosition.left = elementPageLeft;
        this._previousPosition.top = elementPageTop;
        const args = {
            event: evt,
            element: this.element,
            target: this._hitTarget(evt),
            bindEvents: this._bindActiveEvents.bind(this),
            dragElement: dragElement
        };
        if (typeof this.options.dragStart === 'function') this.options.dragStart(args);
        this._bindActiveEvents(dragElement);
    };

    Draggable.prototype._bindActiveEvents = function (helper) {
        this._moveHandler = (eventObj) => this._onMove(eventObj);
        this._upHandler = (eventObj) => this._onUp(eventObj);
        _addEvents(document, [EVENT.MOUSE_MOVE, EVENT.TOUCH_MOVE], this._moveHandler);
        _addEvents(document, [EVENT.MOUSE_UP, EVENT.TOUCH_END], this._upHandler);
        this._setGlobalDroppables(false, this.element, this._dragHelper);
    };

    Draggable.prototype._detachActive = function () {
        if (this._moveHandler) {
            _removeEvents(document, [EVENT.MOUSE_MOVE, EVENT.TOUCH_MOVE], this._moveHandler);
        }
        if (this._upHandler) {
            _removeEvents(document, [EVENT.MOUSE_UP, EVENT.TOUCH_END], this._upHandler);
        }
        this._moveHandler = null;
        this._upHandler = null;
    };

    Draggable.prototype._onMove = function (evt) {
        const isTouch = !!(evt.changedTouches);
        if (isTouch && evt.changedTouches.length !== 1) return;
        if (isTouch) {
            evt.preventDefault();
        }
        const docH = getDocumentSize('Height');
        const docW = getDocumentSize('Width');
        // use raw page coordinates for pointer to avoid inconsistencies with scrolling
        const moveCoords = getCoords(evt);
        this._currentPosition = { left: moveCoords.pageX, top: moveCoords.pageY };
        if (docH < this._currentPosition.top) this._currentPosition.top = docH;
        if (docW < this._currentPosition.left) this._currentPosition.left = docW;
        if (typeof this.options.drag === 'function') {
            this.options.drag({ event: evt, element: this.element, target: this._hitTarget(evt) });
        }
        const dragElement = this._dragHelper;
        this._parentBoundingRect = this._calcParentRect(dragElement.offsetParent || document.body);
        const iLeft = this._parentBoundingRect.left + (this._border.left || 0);
        const iTop = this._parentBoundingRect.top + (this._border.top || 0);
        const dragLeftOffset = this._currentPosition.left - this._cursorDiff.x;
        const dragTopOffset = this._currentPosition.top - this._cursorDiff.y;
        let left; let top;
        if (this.options.dragArea) {
            // reuse margins measured at drag start instead of recomputing styles each move
            const dragElementWidth = dragElement.offsetWidth + (this._margin.left || 0) + (this._margin.right || 0);
            const dragElementHeight = dragElement.offsetHeight + (this._margin.top || 0) + (this._margin.bottom || 0);
            // Horizontal
            if (this._lastPageX !== this._currentPosition.left) {
                if (this._dragLimits.left > dragLeftOffset && dragLeftOffset > 0) left = this._dragLimits.left;
                else if (this._dragLimits.right < dragLeftOffset + dragElementWidth && dragLeftOffset > 0)
                    left = dragLeftOffset - (dragLeftOffset - this._dragLimits.right) - dragElementWidth;
                else left = dragLeftOffset < 0 ? this._dragLimits.left : dragLeftOffset;
            }
            // Vertical
            if (this._lastPageY !== this._currentPosition.top) {
                if (this._dragLimits.top > dragTopOffset && dragTopOffset > 0) top = this._dragLimits.top;
                else if (this._dragLimits.bottom < dragTopOffset + dragElementHeight && dragTopOffset > 0)
                    top = dragTopOffset - (dragTopOffset - this._dragLimits.bottom) - dragElementHeight;
                else top = dragTopOffset < 0 ? this._dragLimits.top : dragTopOffset;
            }
        } else {
            left = dragLeftOffset; top = dragTopOffset;
        }
        if (sfBlazorToolkit.base.isNullOrUndefined(top)) top = this._previousPosition.top;
        if (sfBlazorToolkit.base.isNullOrUndefined(left)) left = this._previousPosition.left;
        const outLeft = left - iLeft;
        const outTop = top - iTop;
        const dragVal = { left: outLeft + 'px', top: outTop + 'px' };
        sfBlazorToolkit.base.setStyleAttribute(dragElement, dragVal);
        this._dragging = true;
        this._previousPosition.left = left;
        this._previousPosition.top = top;
        this._lastPageX = this._currentPosition.left;
        this._lastPageY = this._currentPosition.top;
    };

    Draggable.prototype._onUp = function (evt) {
        this._dragging = false;
        const isTouch = !!(evt.changedTouches);
        if (isTouch && evt.changedTouches.length !== 1) return;
        if (typeof this.options.dragStop === 'function') {
            this.options.dragStop({ event: evt, element: this.element, target: this._hitTarget(evt), helper: this._dragHelper });
        }
        this._setGlobalDroppables(true);
        sfBlazorToolkit.base.removeClass(document.body, 'e-prevent-select');
        this._resetAfterStop();
    };

    Draggable.prototype._resetAfterStop = function () {
        this._detachActive();
        this._bindStart();
        this.element.setAttribute('aria-grabbed', 'false');
        dragGuard.active = false;
        this._hoverElement = null;
    };

    Draggable.prototype._isInViewport = function (element) {
        if (!element) return false;
        const rect = element.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    };

    Draggable.prototype._calcParentRect = function (element) {
        if (!element) return { left: 0, top: 0 };
        const rect = element.getBoundingClientRect();
        const style = window.getComputedStyle(element);
        return {
            left: (rect.left + window.pageXOffset) - (parseInt(style.marginLeft, 10) || 0),
            top: (rect.top + window.pageYOffset) - (parseInt(style.marginTop, 10) || 0)
        };
    };

    Draggable.prototype._getScrollableValues = function () {
        this._parentScrollOffset.x = 0; this._parentScrollOffset.y = 0;
    };

    Draggable.prototype._offsetLeft = function (element) {
        const r = element.getBoundingClientRect(); return r.left + window.pageXOffset;
    };
    Draggable.prototype._offsetTop = function (element) {
        const r = element.getBoundingClientRect(); return r.top + window.pageYOffset;
    };

    Draggable.prototype._getMousePosition = function (evt, isDragScroll) {
        const src = evt.target || evt.srcElement;
        const coords = getCoords(evt);
        let pageX; let pageY;
        const hasOffsetParent = !!(src && src.offsetParent);
        // Use page coordinates returned by the event directly (coords.pageX/Y).
        // Avoid adding window.pageXOffset or subtracting scrollingElement values
        // because coords.pageX/Y already include page scroll offset. For drag-scroll
        // mode include offsetParent scroll values when available.
        if (isDragScroll) {
            pageX = coords.pageX + (hasOffsetParent ? src.offsetParent.scrollLeft : 0) - (this._relativeX || 0);
            pageY = coords.pageY + (hasOffsetParent ? src.offsetParent.scrollTop : 0) - (this._relativeY || 0);
        } else {
            pageX = coords.pageX - (this._relativeX || 0);
            pageY = coords.pageY - (this._relativeY || 0);
        }
        return { left: pageX - ((this._margin.left || 0)), top: pageY - ((this._margin.top || 0)) };
    };

    Draggable.prototype._computeDragArea = function () {
        const area = typeof this.options.dragArea === 'string' ? select(this.options.dragArea) : this.options.dragArea;
        if (!area) return;
        const rect = area.getBoundingClientRect();
        const styles = window.getComputedStyle(area);
        // borders & padding
        const keys = ['Top', 'Left', 'Bottom', 'Right'];
        for (let i = 0; i < keys.length; i++) {
            const key = keys[i];
            const lower = key.toLowerCase();
            this._border[lower] = parseFloat(styles['border' + key + 'Width']) || 0;
            this._padding[lower] = parseFloat(styles['padding' + key]) || 0;
        }
        // convert area bounds to page coordinates so comparisons are consistent
        const width = rect.right - rect.left;
        const height = rect.bottom - rect.top;
        const top = rect.top + window.pageYOffset;
        const left = rect.left + window.pageXOffset;
        this._dragLimits.left = left + this._border.left + this._padding.left;
        this._dragLimits.top = top + this._border.top + this._padding.top;
        this._dragLimits.right = left + width - (this._border.right + this._padding.right);
        this._dragLimits.bottom = top + height - (this._border.bottom + this._padding.bottom);
    };

    Draggable.prototype._hitTarget = function (evt) {
        const coords = getCoords(evt);
        let targetElement;
        if (this._dragHelper && (this._dragHelper === evt.target || this._dragHelper.contains(evt.target))) {
            targetElement = this._elementFromPointIgnoring(this._dragHelper, coords.clientX, coords.clientY);
        } else {
            targetElement = evt.target;
        }
        return targetElement;
    };

    Draggable.prototype._elementFromPointIgnoring = function (elToIgnore, x, y) {
        if (!elToIgnore) return document.elementFromPoint(x, y);
        const style = elToIgnore.style;
        const prevPointerEvents = style.pointerEvents;
        style.pointerEvents = 'none';
        const foundElement = document.elementFromPoint(x, y);
        style.pointerEvents = prevPointerEvents;
        return foundElement;
    };

    Draggable.prototype._setGlobalDroppables = function (reset, drag, helper) {
        this._droppablesCache[this.options.scope] = reset ? null : { draggable: drag, helper: helper, draggedElement: this.element };
    };

    Draggable.prototype._checkDroppable = function (evt) {
        const target = this._hitTarget(evt);
        return { target };
    };
    return Draggable;
}());
