'use strict';

    var __assign = (undefined && undefined.__assign) || function () {
        __assign = Object.assign || function(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                    t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };
    var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
            function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
            function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    };
    var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
        var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f) throw new TypeError("Generator is already executing.");
            while (_) try {
                if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
                if (y = 0, t) op = [op[0] & 2, t.value];
                switch (op[0]) {
                    case 0: case 1: t = op; break;
                    case 4: _.label++; return { value: op[1], done: false };
                    case 5: _.label++; y = op[1]; op = [0]; continue;
                    case 7: op = _.ops.pop(); _.trys.pop(); continue;
                    default:
                        if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                        if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                        if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                        if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                        if (t[2]) _.ops.pop();
                        _.trys.pop(); continue;
                }
                op = body.call(thisArg, _);
            } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
            if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
        }
    };
    var TAP_HOLD_THRESHOLD = 500;
    var SHOW_POINTER_TIP_GAP = 0;
    var HIDE_POINTER_TIP_GAP = 8;
    var MOUSE_TRAIL_GAP = 2;
    var POINTER_ADJUST = 2;
    var ROOT = 'e-tooltip';
    var TOOLTIP_WRAP = 'e-tooltip-wrap';
    var ARROW_TIP = 'e-arrow-tip';
    var ARROW_TIP_OUTER = 'e-arrow-tip-outer';
    var ARROW_TIP_INNER = 'e-arrow-tip-inner';
    var TIP_BOTTOM = 'e-tip-bottom';
    var TIP_TOP = 'e-tip-top';
    var TIP_LEFT = 'e-tip-left';
    var TIP_RIGHT = 'e-tip-right';
    var TIP_ICON_BOTTOM = 'e-chevron-down-fill';
    var TIP_ICON_TOP = 'e-chevron-up-fill';
    var TIP_ICON_LEFT = 'e-chevron-left-fill';
    var TIP_ICON_RIGHT = 'e-chevron-right-fill';
    var POPUP_ROOT = 'e-popup';
    var POPUP_OPEN = 'e-popup-open';
    var POPUP_CLOSE = 'e-popup-close';
    var POPUP_LIB = 'e-lib';
    var HIDDEN = 'e-hidden';
    var BIGGER = 'e-bigger';
    var RIGHT = 'Right';
    var BOTTOM = 'Bottom';
    var TOP = 'Top';
    var LEFT = 'Left';
    var CENTER = 'Center';
    var END = 'End';
    var START = 'Start';
    var TOP_LEFT = 'TopLeft';
    var TOP_RIGHT = 'TopRight';
    var BOTTOM_LEFT = 'BottomLeft';
    var BOTTOM_CENTER = 'BottomCenter';
    var BOTTOM_RIGHT = 'BottomRight';
    var LEFT_TOP = 'LeftTop';
    var LEFT_CENTER = 'LeftCenter';
    var LEFT_BOTTOM = 'LeftBottom';
    var RIGHT_TOP = 'RightTop';
    var RIGHT_CENTER = 'RightCenter';
    var RIGHT_BOTTOM = 'RightBottom';
    var PLACEHOLDER = '_content_placeholder';
    var CONTENT = '_content';
    var TIP_CONTENT = 'e-tip-content';
    var POPUP_CONTAINER = 'e-tooltip-popup-container';
    var ARIA_DESCRIBEDBY = 'aria-describedby';
    var TARGET = 'target';
    var NONE = 'None';
    var ENABLE = 'Enable';
    var DISABLE = 'Disable';
    var liveRefs = new Map();

    // ---- Tooltip-local helpers (moved from sfBlazorToolkit.base — tooltip-only callers) ----

    // Returns the current global animation mode — read live from animation.js's canonical
    // window.sfBlazorToolkit.animationMode getter, so setGlobalAnimation() changes are always
    // reflected without a page reload.
    function getAnimMode() {
        return (window.sfBlazorToolkit && window.sfBlazorToolkit.animationMode != null)
            ? window.sfBlazorToolkit.animationMode : 'Default';
    }

    // Writes DOM attributes from a plain key-value map onto a DOM element.
    // Optimized: the plain-object path from the base version is dropped — tooltip always
    // passes real DOM elements.
    function setAttributes(element, attrs) {
        if (!element || !attrs) return element;
        var keys = Object.keys(attrs);
        for (var i = 0; i < keys.length; i++) {
            element.setAttribute(keys[i], attrs[keys[i]]);
        }
        return element;
    }

    // Reads an attribute from a DOM element; if absent and `fallback` is non-null, writes
    // the fallback value and returns it.
    // Optimized: the isObject/getValue branch from the base version is dropped — tooltip
    // always passes DOM elements, never plain JS objects.
    function getAttrOrDefault(element, property, fallback) {
        var val = element.getAttribute(property);
        if (val === null && fallback !== null && fallback !== undefined) {
            element.setAttribute(property, String(fallback));
            return fallback;
        }
        return val;
    }

    // Generates an XPath expression that uniquely identifies `elm` within the document.
    // Used to build the serializable DOM-reference objects sent to C# as event data.
    // Moved verbatim from sfBlazorToolkit.base.createXPathFromElement.
    function createXPath(elm) {
        var allNodes = document.getElementsByTagName('*');
        for (var segs = []; elm && elm.nodeType === 1; elm = elm.parentNode) {
            if (elm.hasAttribute('id')) {
                var uniqueIdCount = 0;
                for (var n = 0; n < allNodes.length; n++) {
                    if (allNodes[n].hasAttribute('id') && allNodes[n].id === elm.id) uniqueIdCount++;
                    if (uniqueIdCount > 1) break;
                }
                if (uniqueIdCount === 1) {
                    segs.unshift('id("' + elm.getAttribute('id') + '")');
                    return segs.join('/');
                } else {
                    segs.unshift(elm.localName.toLowerCase() + '[@id="' + elm.getAttribute('id') + '"]');
                }
            } else {
                for (var i = 1, sib = elm.previousSibling; sib; sib = sib.previousSibling) {
                    if (sib.localName === elm.localName) i++;
                }
                segs.unshift(elm.localName.toLowerCase() + '[' + i + ']');
            }
        }
        return segs.length ? '/' + segs.join('/') : null;
    }

    function safeInvokeAsync(dataId, method) {
        var args = [];
        for (var _i = 2; _i < arguments.length; _i++) {
            args[_i - 2] = arguments[_i];
        }
        return __awaiter(this, void 0, void 0, function () {
            var ref, e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        ref = liveRefs.get(dataId);
                        if (!ref || typeof ref.invokeMethodAsync !== 'function') {
                            return [2 /*return*/, Promise.resolve(false)];
                        }
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, ref.invokeMethodAsync.apply(ref, [method].concat(args))];
                    case 2:
                        _a.sent();
                        return [2 /*return*/, true];
                    case 3:
                        e_1 = _a.sent();
                        return [2 /*return*/, false];
                    case 4: return [2 /*return*/];
                }
            });
        });
    }
    var SfTooltip = /** @class */ (function () {
        function SfTooltip(dataId, element, ref, properties, eventList) {
            this.popupObj = null;
            this.tipClass = TIP_BOTTOM;
            this.tooltipPositionX = 'Center';
            this.tooltipPositionY = 'Top';
            this.isContinuousOpen = false;
            this.isRestrictUpdate = false;
            this.showTimer = 0;
            this.hideTimer = 0;
            this.contentTargetValue = null;
            this.contentEvent = null;
            this.contentAnimation = null;
            this.beforeCloseAnimation = null;
            this.isPopupHidden = true;
            this.isMultiTarget = false;
            this.mouseAction = false;
            this.mouseMoveEvent = null;
            this.mouseMoveTarget = null;
            this.containerElement = null;
            this.isBodyContainer = true;
            this.isClick = false;
            this.element = element;
            this.dataId = dataId;
            this.properties = properties;
            this.dotnetRef = ref;
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.element)) {
                this.ctrlId = this.element.id;
                window.sfBlazorToolkit.base.setCompInstance(this);
                this.eventList = eventList;
            }
        }
        SfTooltip.prototype.getTriggerList = function (trigger) {
            if (trigger === 'Auto') {
                trigger = (sfBlazorToolkit.base.Browser.isDevice) ? 'Hover' : 'Hover Focus';
            }
            return trigger.split(' ');
        };
        SfTooltip.prototype.formatPosition = function () {
            var _a, _b;
            if (this.properties.position.indexOf('Top') === 0 || this.properties.position.indexOf('Bottom') === 0) {
                _a = this.properties.position.split(/(?=[A-Z])/), this.tooltipPositionY = _a[0], this.tooltipPositionX = _a[1];
            }
            else {
                _b = this.properties.position.split(/(?=[A-Z])/), this.tooltipPositionX = _b[0], this.tooltipPositionY = _b[1];
            }
        };
        SfTooltip.prototype.getTargetList = function (target) {
            if (!target || target === '') {
                return [this.element];
            }
            var targetElements = this.element ? this.selectAll(target, this.element) : [];
            return targetElements.length > 0 ? targetElements : this.selectAll(target);
        };
        SfTooltip.prototype.wireEvents = function (trigger) {
            var _this = this;
            var triggerList = this.getTriggerList(trigger);
            var target = this.properties.targetContainer ?
               this.select(this.properties.targetContainer) || this.element : this.element;
            triggerList.forEach(function (opensOn) {
                if (opensOn === 'Custom') {
                    return;
                }
                switch (opensOn) {
                    case 'Focus':
                        sfBlazorToolkit.base.EventHandler.add(target, 'focusin', _this.targetHover, _this);
                        break;
                    case 'Click':
                        sfBlazorToolkit.base.EventHandler.add(target, sfBlazorToolkit.base.Browser.touchStartEvent, _this.targetClick, _this);
                        break;
                    case 'Hover':
                        if (sfBlazorToolkit.base.Browser.isDevice) {
                            _this.touchModule = new sfBlazorToolkit.base.Touch(target, {
                                tapHoldThreshold: TAP_HOLD_THRESHOLD,
                                tapHold: _this.tapHoldHandler.bind(_this)
                            });
                            sfBlazorToolkit.base.EventHandler.add(target, sfBlazorToolkit.base.Browser.touchEndEvent, _this.touchEndHandler, _this);
                        }
                        else {
                            sfBlazorToolkit.base.EventHandler.add(target, 'mouseover', _this.targetHover, _this);
                            if (!_this.properties.isSticky) {
                                sfBlazorToolkit.base.EventHandler.add(target, 'mouseout', _this.onMouseOut, _this);
                            }
                        }
                        break;
                }
            });
            sfBlazorToolkit.base.EventHandler.add(document, 'touchend', this.touchEnd, this);
            sfBlazorToolkit.base.EventHandler.add(document, 'scroll wheel', this.scrollHandler.bind(this), { passive: true });
            sfBlazorToolkit.base.EventHandler.add(document, 'keydown', this.keyDown, this);
            window.addEventListener('resize', this.onWindowResize.bind(this));
        };
        SfTooltip.prototype.getValidTarget = function (e) {
            var target = null;
            var eventType = e.type;
            switch (eventType) {
                case 'focusin':
                case 'focusout':
                    if (this.properties.target) {
                        // Prefer closest match, but if selector is a descendant selector
                        // (e.g., "#details .e-textbox"), `closest` may fail. Fall back
                        // to searching candidate elements and checking containment.
                        var closestMatch = sfBlazorToolkit.base.closest(e.target, this.properties.target);
                        if (closestMatch) {
                            target = closestMatch;
                        }
                        else {
                            var candidates = this.selectAll(this.properties.target);
                            for (var i = 0; i < candidates.length; i++) {
                                if (candidates[i].contains(e.target)) {
                                    target = candidates[i];
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        // If no explicit target selector provided, consider focus on any
                        // descendant element as a valid target (e.g., focusing a button/input
                        // inside the wrapper should trigger the tooltip).
                        target = (e.target === this.element || this.element.contains(e.target)) ? this.element : null;
                    }
                    break;
                case 'mouseover':
                case sfBlazorToolkit.base.Browser.touchStartEvent:
                case sfBlazorToolkit.base.Browser.touchEndEvent:
                case 'touchstart':
                case 'touchend':
                case 'mousedown':
                case 'mouseup':
                    if (this.properties.target) {
                        var closestMatch = sfBlazorToolkit.base.closest(e.target, this.properties.target);
                        if (closestMatch) {
                            target = closestMatch;
                        }
                        else {
                            var candidates = this.selectAll(this.properties.target);
                            for (var i = 0; i < candidates.length; i++) {
                                if (candidates[i].contains(e.target)) {
                                    target = candidates[i];
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        target = (e.target === this.element || this.element.contains(e.target)) ? this.element : null;
                    }
                    break;
                case 'mouseout':
                    var relatedTarget = e.relatedTarget;
                    if (this.properties.target) {
                        var closestMatch = sfBlazorToolkit.base.closest(e.target, this.properties.target);
                        if (closestMatch) {
                            target = closestMatch;
                        }
                        else {
                            var candidates = this.selectAll(this.properties.target);
                            for (var i = 0; i < candidates.length; i++) {
                                if (candidates[i].contains(e.target)) {
                                    target = candidates[i];
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        target = (e.target !== this.element || !relatedTarget || !this.element.contains(relatedTarget)) ? this.element : null;
                    }
                    if (this.properties.target && target) {
                        target = (relatedTarget === target || !target.contains(relatedTarget)) ? target : null;
                    }
                    break;
            }
            return target;
        };
        SfTooltip.prototype.onWindowResize = function () {
            if (!this.isHidden()) {
                this.reposition(this.findTarget());
            }
        };
        SfTooltip.prototype.wireMouseEvents = function (e, target) {
            if (this.tooltipEle) {
                if (!this.properties.isSticky) {
                    if (e.type === 'focusin') {
                        var eventContainer = this.properties.targetContainer ?
                            this.select(this.properties.targetContainer) || this.element : this.element;
                        sfBlazorToolkit.base.EventHandler.add(eventContainer, 'focusout', this.onMouseOut, this);
                    }
                    if (this.properties.closeDelay) {
                        sfBlazorToolkit.base.EventHandler.add(this.tooltipEle, 'mouseenter', this.tooltipHover, this);
                        sfBlazorToolkit.base.EventHandler.add(this.tooltipEle, 'mouseleave', this.tooltipMouseOut, this);
                    }
                }
                if (this.properties.mouseTrail) {
                    sfBlazorToolkit.base.EventHandler.add(target, 'mousemove touchstart mouseenter', this.onMouseMove, this);
                }
            }
        };
        SfTooltip.prototype.unWireEvents = function (trigger) {
            var _this = this;
            var triggerList = this.getTriggerList(trigger);
            var target = this.properties.targetContainer ?
               this.select(this.properties.targetContainer) || this.element : this.element;
            triggerList.forEach(function (opensOn) {
                if (opensOn === 'Custom') {
                    return;
                }
                switch (opensOn) {
                    case 'Focus':
                        sfBlazorToolkit.base.EventHandler.remove(target, 'focusin', _this.targetHover);
                        break;
                    case 'Click':
                        sfBlazorToolkit.base.EventHandler.remove(target, sfBlazorToolkit.base.Browser.touchStartEvent, _this.targetClick);
                        break;
                    case 'Hover':
                        if (sfBlazorToolkit.base.Browser.isDevice) {
                            if (_this.touchModule) {
                                _this.touchModule.destroy();
                            }
                            sfBlazorToolkit.base.EventHandler.remove(target, sfBlazorToolkit.base.Browser.touchEndEvent, _this.touchEndHandler);
                        }
                        else {
                            sfBlazorToolkit.base.EventHandler.remove(target, 'mouseover', _this.targetHover);
                            if (!_this.properties.isSticky) {
                                sfBlazorToolkit.base.EventHandler.remove(target, 'mouseout', _this.onMouseOut);
                            }
                        }
                        break;
                }
            });
            sfBlazorToolkit.base.EventHandler.remove(document, 'touchend', this.touchEnd);
            sfBlazorToolkit.base.EventHandler.remove(document, 'scroll wheel', this.scrollHandler);
            sfBlazorToolkit.base.EventHandler.remove(document, 'keydown', this.keyDown);
            window.removeEventListener('resize', this.onWindowResize.bind(this));
        };
        SfTooltip.prototype.unWireMouseEvents = function (target) {
            var _this = this;
            if (!this.properties.isSticky) {
                var triggerList = this.getTriggerList(this.properties.opensOn);
                var eventContainer_1 = this.properties.targetContainer ?
                    this.select(this.properties.targetContainer) || this.element : this.element;
                triggerList.forEach(function (opensOn) {
                    if (opensOn === 'Focus') {
                        sfBlazorToolkit.base.EventHandler.remove(eventContainer_1, 'focusout', _this.onMouseOut);
                    }
                });
                if (this.properties.closeDelay) {
                    sfBlazorToolkit.base.EventHandler.remove(target, 'mouseenter', this.tooltipHover);
                    sfBlazorToolkit.base.EventHandler.remove(target, 'mouseleave', this.tooltipMouseOut);
                }
            }
            if (this.properties.mouseTrail) {
                sfBlazorToolkit.base.EventHandler.remove(target, 'mousemove touchstart mouseenter', this.onMouseMove);
            }
        };
        SfTooltip.prototype.findTarget = function () {
            return this.select('[data-tooltip-id= "' + this.ctrlId + '_content"]');
        };
        SfTooltip.prototype.addDescribedBy = function (target, id) {
            var describedby = (getAttrOrDefault(target, ARIA_DESCRIBEDBY, null) || '').split(/\s+/);
            if (describedby.indexOf(id) < 0) {
                describedby.push(id);
            }
            setAttributes(target, { 'aria-describedby': describedby.join(' ').trim(), 'data-tooltip-id': id });
        };
        SfTooltip.prototype.removeDescribedBy = function (target) {
            var id = getAttrOrDefault(target, 'data-tooltip-id', null);
            var describedby = (getAttrOrDefault(target, ARIA_DESCRIBEDBY, null) || '').split(/\s+/);
            var index = describedby.indexOf(id);
            if (index !== -1) {
                describedby.splice(index, 1);
            }
            target.removeAttribute('data-tooltip-id');
            var orgDescribedby = describedby.join(' ').trim();
            if (orgDescribedby) {
                setAttributes(target, { 'aria-describedby': orgDescribedby });
            }
            else {
                target.removeAttribute(ARIA_DESCRIBEDBY);
            }
        };
        SfTooltip.prototype.clear = function () {
            if (this.isPopupHidden) {
                if (this.popupObj) {
                    this.popupObj.destroy();
                }
                if (this.tooltipEle) {
                    sfBlazorToolkit.base.removeClass([this.tooltipEle], POPUP_CLOSE);
                    sfBlazorToolkit.base.addClass([this.tooltipEle], POPUP_OPEN);
                    this.tooltipEle.style.display = 'none';
                    var contentElement = document.getElementById(this.ctrlId + PLACEHOLDER);
                    if (contentElement) {
                        contentElement.appendChild(this.tooltipEle);
                    }
                    if (this.tooltipEle.parentNode) {
                        this.tooltipEle.parentNode.removeChild(this.tooltipEle);
                    }
                    if (document.body.contains(this.element)) {
                        safeInvokeAsync(this.dataId, 'CreateTooltipAsync', false);
                    }
                }
                this.tooltipEle = null;
                this.popupObj = null;
            }
        };
        SfTooltip.prototype.tapHoldHandler = function (e) {
            var target = this.getValidTarget(e.originalEvent);
            if (!target) {
                return;
            }
            this.targetHover(e.originalEvent);
        };
        SfTooltip.prototype.touchEndHandler = function (e) {
            var target = this.getValidTarget(e);
            if (!target) {
                return;
            }
            if (!this.properties.isSticky) {
                this.hideTooltip(this.properties.animation.close);
            }
        };
        SfTooltip.prototype.targetClick = function (e) {
            var validTarget = this.getValidTarget(e);
            if (!validTarget) {
                return;
            }
            var target = this.properties.target ? sfBlazorToolkit.base.closest(e.target, this.properties.target) :
                this.element;
            var mouseEvent = e;
            if (target) {
                if (getAttrOrDefault(target, 'data-tooltip-id', null) === null) {
                    if (!(mouseEvent.type === 'mousedown' && mouseEvent.button === 2)) {
                        this.targetHover(e);
                    }
                }
                else if (!this.properties.isSticky) {
                    this.hideTooltip(this.properties.animation.close, e, target);
                }
            }
        };
        SfTooltip.prototype.restoreElement = function (target) {
            this.unWireMouseEvents(target);
            if (!sfBlazorToolkit.base.isNullOrUndefined(getAttrOrDefault(target, 'data-content', null))) {
                target.removeAttribute('data-content');
            }
            this.removeDescribedBy(target);
        };
        SfTooltip.prototype.checkForOpen = function (opensOn, element, e) {
            if (sfBlazorToolkit.base.isNullOrUndefined(element) || sfBlazorToolkit.base.isNullOrUndefined(e)) {
                return false;
            }
            var target = this.properties.target ? sfBlazorToolkit.base.closest(e.target, this.properties.target) : this.element;
            if (sfBlazorToolkit.base.isNullOrUndefined(target)) {
                return false;
            }
            var isOpenable = true;
            if (opensOn === 'Hover') {
                isOpenable = sfBlazorToolkit.base.matches(target, ':hover');
            }
            else if (opensOn === 'Auto') {
                // Consider the actual event target (e.g., an input inside a wrapper) as focused
                var eventTarget = e && e.target ? e.target : null;
                isOpenable = (sfBlazorToolkit.base.matches(target, ':hover') || sfBlazorToolkit.base.matches(target, ':focus') || (eventTarget && sfBlazorToolkit.base.matches(eventTarget, ':focus')));
            }
            else if (opensOn === 'Focus') {
                // When the configured target is a non-focusable wrapper, the focused element
                // may be a child (like an <input>). Also accept focus when the event target
                // itself is focused.
                var eventTarget = e && e.target ? e.target : null;
                isOpenable = (sfBlazorToolkit.base.matches(target, ':focus') || (eventTarget && sfBlazorToolkit.base.matches(eventTarget, ':focus')));
            }
            else if (opensOn === 'Click' || opensOn === 'Custom') {
                if (getAttrOrDefault(target, 'data-tooltip-id', null) === null) {
                    isOpenable = true;
                }
                else {
                    isOpenable = false;
                }
            }
            return isOpenable;
        };
        SfTooltip.prototype.targetHover = function (e) {
            var _this = this;
            var validTarget = this.getValidTarget(e);
            if (!validTarget) {
                return;
            }
            if (!this.checkForOpen(this.properties.opensOn, this.element, e)) {
                return;
            }
            var target = this.properties.target ? sfBlazorToolkit.base.closest(e.target, this.properties.target) :
                this.element;
            var title = target.getAttribute('title');
            target.title = '';
            if (title !== '' && title !== null) {
                setAttributes(target, { 'sf-tooltip': title });
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(target) || getAttrOrDefault(target, 'data-tooltip-id', null) !== null) {
                return;
            }
            this.isMultiTarget = this.getTargetList(this.properties.target).length > 1;
            if (e.type !== 'focusin' && this.properties.target && this.isMultiTarget && e && !sfBlazorToolkit.base.isNullOrUndefined(e.target) && !sfBlazorToolkit.base.isNullOrUndefined((e.relatedTarget))) {
                var currTarget = sfBlazorToolkit.base.closest(e.target, this.properties.target);
                var relatedTarget = sfBlazorToolkit.base.closest(e.relatedTarget, this.properties.target);
                if (currTarget === relatedTarget) {
                    return;
                }
            }
            var targetList = this.selectAll('[data-tooltip-id= "' + this.ctrlId + '_content"]');
            targetList.forEach(function (target) {
                _this.restoreElement(target);
            });
            this.mouseAction = false;
            this.showTooltip(target, this.properties.animation.open, e);
        };
        SfTooltip.prototype.isHidden = function () {
            return this.tooltipEle ? !this.tooltipEle.classList.contains(POPUP_OPEN) : true;
        };
        SfTooltip.prototype.mouseMoveBeforeOpen = function (e) {
            this.mouseMoveEvent = e;
        };
        SfTooltip.prototype.mouseMoveBeforeRemove = function () {
            if (this.mouseMoveTarget) {
                sfBlazorToolkit.base.EventHandler.remove(this.mouseMoveTarget, 'mousemove touchstart', this.mouseMoveBeforeOpen);
            }
        };
        SfTooltip.prototype.showTooltip = function (target, showAnimation, e) {
            var _this = this;
            this.mouseAction = false;
            this.clear(); 
            clearTimeout(this.showTimer);
            clearTimeout(this.hideTimer);
            if (this.properties.mouseTrail && this.properties.openDelay) {
                this.mouseMoveBeforeRemove();
                this.mouseMoveTarget = target;
                sfBlazorToolkit.base.EventHandler.add(this.mouseMoveTarget, 'mousemove touchstart', this.mouseMoveBeforeOpen, this);
            }
            var show = function () {
                _this.isContinuousOpen = !sfBlazorToolkit.base.isNullOrUndefined(_this.tooltipEle);
                _this.tooltipEventArgs = {
                    type: e ? e.type.toString() : null,
                    cancel: false,
                    target: _this.getDomObject(TARGET, target),
                    event: e ? e : null,
                    hasText: _this.hasText(),
                    element: _this.getDomObject('tooltipElement', _this.tooltipEle),
                    isInteracted: !sfBlazorToolkit.base.isNullOrUndefined(e),
                    name: 'beforeRender',
                    left: e ? _this.getXYValue(e, 'x') : null,
                    top: e ? _this.getXYValue(e, 'y') : null
                };
                _this.contentTargetValue = target;
                _this.contentEvent = e;
                _this.contentAnimation = showAnimation;
                _this.isRestrictUpdate = _this.eventList.beforeRender && !_this.isHidden();
                if (_this.eventList.beforeRender) {
                    _this.triggerEvent('TriggerBeforeRenderEventAsync', _this.tooltipEventArgs);
                }
                else {
                    _this.beforeRenderCallBack(false);
                }
            };
            this.showTimer = setTimeout(show, this.properties.openDelay);
        };
        SfTooltip.prototype.triggerEvent = function (eventName, args) {
            if (document.body.contains(this.element)) {
                this.dotnetRef.invokeMethodAsync(eventName, args);
            }
        };
        SfTooltip.prototype.beforeRenderCallBack = function (cancel) {
            if (cancel) {
                this.isPopupHidden = true;
                this.clear();
                this.mouseMoveBeforeRemove();
            }
            else {
                this.isPopupHidden = false;
                if (sfBlazorToolkit.base.isNullOrUndefined(this.tooltipEle)) {
                    if (document.body.contains(this.element)) {
                        this.dotnetRef.invokeMethodAsync('CreateTooltipAsync', true);
                    }
                }
                else if (this.isContinuousOpen && !this.isRestrictUpdate) {
                    this.contentUpdated();
                }
                else {
                    this.isRestrictUpdate = false;
                }
            }
        };
        SfTooltip.prototype.checkCollision = function (target, x, y) {
            var elePos = {
                left: x,
                top: y,
                position: this.properties.position,
                horizontal: this.tooltipPositionX,
                vertical: this.tooltipPositionY,
                isCollided: false
            };
            var affectedPos = sfBlazorToolkit.popups.isCollide(this.tooltipEle, this.checkCollideTarget(), x, y);
            if (affectedPos.length > 0) {
                elePos.horizontal = affectedPos.indexOf('left') >= 0 ? RIGHT : affectedPos.indexOf('right') >= 0 ? LEFT :
                    this.tooltipPositionX;
                if ((this.properties.windowCollision && this.properties.position.indexOf('Left') === 0 || this.properties.position.indexOf('Right') === 0)) {
                    elePos.vertical = (affectedPos.indexOf('top') >= 0 || affectedPos.indexOf('bottom') >= 0) ? CENTER : this.tooltipPositionY;
                    elePos.isCollided = true;
                }
                else {
                    elePos.vertical = affectedPos.indexOf('top') >= 0 ? BOTTOM : affectedPos.indexOf('bottom') >= 0 ? TOP : this.tooltipPositionY;
                }
            }
            return elePos;
        };
        SfTooltip.prototype.checkCollideTarget = function () {
            return !this.properties.windowCollision && this.properties.target ? this.element : null;
        };
        SfTooltip.prototype.calculateElementPosition = function (pos, offsetPos) {
            return [this.isBodyContainer ? pos.left + offsetPos.left :
                    (pos.left - this.containerElement.offsetLeft) + offsetPos.left + window.scrollX + this.containerElement.scrollLeft,
                this.isBodyContainer ? pos.top + offsetPos.top :
                    (pos.top - this.containerElement.offsetTop) + offsetPos.top + window.scrollY + this.containerElement.scrollTop];
        };
        SfTooltip.prototype.getScalingFactor = function (target) {
            if (!target) {
                return { x: 1, y: 1 };
            }
            var scalingFactors = { x: 1, y: 1 };
            var elementsWithTransform = target.closest('[style*="transform: scale"]');
            if (elementsWithTransform && elementsWithTransform !== this.tooltipEle && elementsWithTransform.contains(this.tooltipEle)) {
                var computedStyle = window.getComputedStyle(elementsWithTransform);
                var transformValue = computedStyle.getPropertyValue('transform');
                var matrixValues = transformValue.match(/matrix\(([^)]+)\)/)[1].split(',').map(parseFloat);
                scalingFactors.x = matrixValues[0];
                scalingFactors.y = matrixValues[3];
            }
            return scalingFactors;
        };
        SfTooltip.prototype.collisionFlipFit = function (target, x, y) {
            var elePos = this.checkCollision(target, x, y);
            var newPos = elePos.position;
            if (this.tooltipPositionY !== elePos.vertical) {
                newPos = ((this.properties.position.indexOf(BOTTOM) === 0 || this.properties.position.indexOf('Top') === 0) ?
                    elePos.vertical + this.tooltipPositionX : this.tooltipPositionX + elePos.vertical);
            }
            if (this.tooltipPositionX !== elePos.horizontal) {
                if (newPos.indexOf(LEFT) === 0) {
                    elePos.vertical = (newPos === LEFT_TOP || newPos === LEFT_CENTER) ? TOP : BOTTOM;
                    newPos = (elePos.vertical + LEFT);
                }
                if (newPos.indexOf(RIGHT) === 0) {
                    elePos.vertical = (newPos === RIGHT_TOP || newPos === RIGHT_CENTER) ? TOP : BOTTOM;
                    newPos = (elePos.vertical + RIGHT);
                }
                elePos.horizontal = this.tooltipPositionX;
            }
            this.tooltipEventArgs = {
                type: null,
                cancel: false,
                target: this.getDomObject(TARGET, target),
                event: null,
                isInteracted: false,
                hasText: this.hasText(),
                element: this.getDomObject('tooltipElement', this.tooltipEle),
                collidedPosition: newPos,
                name: 'beforeCollision',
                left: null,
                top: null
            };
            this.isRestrictUpdate = this.eventList.beforeCollision && !this.isHidden();
            if (this.eventList.beforeCollision) {
                this.triggerEvent('TriggerBeforeCollisionEventAsync', this.tooltipEventArgs);
            }
            var vertical = elePos.vertical, horizontal = elePos.horizontal, position = elePos.position;
            if (position !== newPos) {
                var pos = sfBlazorToolkit.popups.calculatePosition(target, horizontal, vertical, !this.isBodyContainer, this.isBodyContainer ? null : this.containerElement.getBoundingClientRect());
                this.adjustArrow(target, newPos, horizontal, vertical, elePos.isCollided);
                var scalingFactors = this.getScalingFactor(target);
                var offsetPos = this.calculateTooltipOffset(newPos, scalingFactors.x, scalingFactors.y);
                offsetPos.top -= this.getOffSetPosition('TopBottom', newPos, this.properties.offsetY);
                offsetPos.left -= this.getOffSetPosition('RightLeft', newPos, this.properties.offsetX);
                elePos.position = newPos;
                var _a = this.calculateElementPosition(pos, offsetPos), left = _a[0], top_1 = _a[1];
                elePos.left = left;
                elePos.top = top_1;
            }
            else {
                this.adjustArrow(target, newPos, horizontal, vertical, elePos.isCollided);
            }
            var eleOffset = { left: elePos.left, top: elePos.top };
            var tooltipPosition = sfBlazorToolkit.popups.fit(this.tooltipEle, this.checkCollideTarget(), {
                X: true,
                Y: this.properties.windowCollision
            }, eleOffset);
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'display': 'block' });
            if (this.properties.showTipPointer && (newPos.indexOf('Bottom') === 0 || newPos.indexOf('Top') === 0)) {
                var arrowEle = this.select('.' + ARROW_TIP, this.tooltipEle);
                var arrowLeft = parseInt(arrowEle.style.left, 10) - (tooltipPosition.left - elePos.left);
                if (arrowLeft < 0) {
                    arrowLeft = 0;
                }
                else if ((arrowLeft + arrowEle.offsetWidth) > this.tooltipEle.clientWidth) {
                    arrowLeft = this.tooltipEle.clientWidth - arrowEle.offsetWidth;
                }
                sfBlazorToolkit.base.setStyleAttribute(arrowEle, { 'left': (arrowLeft.toString() + 'px') });
            }
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'display': '' });
            eleOffset.left = tooltipPosition.left;
            if (window.scrollX && this.properties.position === BOTTOM_RIGHT) {
                eleOffset.left = this.element.getBoundingClientRect().left + window.scrollX - (this.element.offsetWidth);
            }
            return eleOffset;
        };
        SfTooltip.prototype.getOffSetPosition = function (positionString, newPos, offsetType) {
            return ((positionString.indexOf(this.properties.position.split(/(?=[A-Z])/)[0]) !== -1) &&
                (positionString.indexOf(newPos.split(/(?=[A-Z])/)[0]) !== -1)) ? (2 * offsetType) : 0;
        };
        SfTooltip.prototype.hideTooltip = function (hideAnimation, e, targetElement) {
            var _this = this;
            clearTimeout(this.hideTimer);
            clearTimeout(this.showTimer);
            var hide = function () {
                if (_this.checkForOpen(_this.properties.opensOn, _this.element, (!sfBlazorToolkit.base.isNullOrUndefined(e) && !e.isTrusted ? null : e))) {
                    return;
                }
                if (_this.properties.closeDelay && _this.tooltipEle && _this.isTooltipOpen) {
                    return;
                }
                var target;
                if (e) {
                    target = _this.properties.target ? (targetElement || e.target) : _this.element;
                }
                else {
                    target = _this.select('[data-tooltip-id= "' + _this.ctrlId + '_content"]', _this.document);
                }
                _this.tooltipEventArgs = {
                    type: e ? e.type.toString() : null,
                    cancel: false,
                    target: _this.getDomObject(TARGET, target),
                    event: e ? e : null,
                    element: _this.getDomObject('tooltipElement', _this.tooltipEle),
                    hasText: _this.hasText(),
                    isInteracted: !sfBlazorToolkit.base.isNullOrUndefined(e) || _this.isClick,
                    name: 'beforeClose',
                    collidedPosition: null,
                    left: e ? _this.getXYValue(e, 'x') : null,
                    top: e ? _this.getXYValue(e, 'y') : null
                };
                _this.beforeCloseTarget = target;
                _this.beforeCloseAnimation = hideAnimation;
                _this.isRestrictUpdate = _this.eventList.beforeClose && !_this.isHidden();
                if (_this.eventList.beforeClose) {
                    _this.triggerEvent('TriggerBeforeCloseEventAsync', _this.tooltipEventArgs);
                }
                else {
                    _this.beforeCloseCallBack(false);
                }
            };
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.tooltipEle) && this.properties.closeDelay <= 0) {
                hide();
            }
            else {
                this.hideTimer = setTimeout(hide, this.properties.closeDelay);
            }
        };
        SfTooltip.prototype.beforeCloseCallBack = function (cancel) {
            if (!cancel) {
                // eslint-disable-next-line
                var proxy_1 = this;
                proxy_1.mouseMoveBeforeRemove();
                var hide = function () {
                    if (!proxy_1.isMultiTarget) {
                        proxy_1.popupHide(proxy_1.beforeCloseAnimation, proxy_1.beforeCloseTarget);
                    }
                };
                if (this.popupObj) {
                    this.popupHide(this.beforeCloseAnimation, this.beforeCloseTarget);
                }
                else {
                    setTimeout(hide, 200);
                }
            }
            else {
                this.isPopupHidden = false;
            }
        };
        SfTooltip.prototype.popupHide = function (hideAnimation, target) {
            if (target) {
                this.restoreElement(target);
            }
            this.isPopupHidden = true;
            var closeAnimation = {
                name: (hideAnimation.effect === NONE && getAnimMode() === ENABLE) ? 'FadeOut' : hideAnimation.effect,
                duration: hideAnimation.duration,
                delay: hideAnimation.delay,
                timingFunction: 'easeIn'
            };
            if ((hideAnimation.effect === NONE && getAnimMode() !== ENABLE) || getAnimMode() === DISABLE) {
                closeAnimation = undefined;
            }
            if (this.popupObj) {
                this.popupObj.hide(closeAnimation);
            }
        };
        SfTooltip.prototype.calculateTooltipOffset = function (position, xScalingFactor, yScalingFactor) {
            if (xScalingFactor === void 0) { xScalingFactor = 1; }
            if (yScalingFactor === void 0) { yScalingFactor = 1; }
            var tooltipEleRect = this.tooltipEle.getBoundingClientRect();
            var arrowEle = this.select('.' + ARROW_TIP, this.tooltipEle);
            var arrowEleRect = arrowEle ? arrowEle.getBoundingClientRect() : null;
            var tipWidth = arrowEle ? arrowEleRect.width : 0;
            var tipHeight = arrowEle ? arrowEleRect.height : 0;
            var tipAdjust = (this.properties.showTipPointer ? SHOW_POINTER_TIP_GAP : HIDE_POINTER_TIP_GAP);
            var pos = { top: 0, left: 0 };
            var tipHeightAdjust = (tipHeight / 2) + POINTER_ADJUST + (tooltipEleRect.height -
                (this.tooltipEle.clientHeight * yScalingFactor));
            var tipWidthAdjust = (tipWidth / 2) + POINTER_ADJUST + (tooltipEleRect.width -
                (this.tooltipEle.clientWidth * xScalingFactor));
            if (this.properties.mouseTrail) {
                tipAdjust += MOUSE_TRAIL_GAP;
            }
            switch (position) {
                case RIGHT_TOP:
                    pos.left += tipWidth + tipAdjust;
                    pos.top -= tooltipEleRect.height - tipHeightAdjust;
                    break;
                case RIGHT_CENTER:
                    pos.left += tipWidth + tipAdjust;
                    pos.top -= (tooltipEleRect.height / 2);
                    break;
                case RIGHT_BOTTOM:
                    pos.left += tipWidth + tipAdjust;
                    pos.top -= (tipHeightAdjust);
                    break;
                case BOTTOM_RIGHT:
                    pos.top += (tipHeight + tipAdjust);
                    pos.left -= (tipWidthAdjust);
                    break;
                case BOTTOM_CENTER:
                    pos.top += (tipHeight + tipAdjust);
                    pos.left -= (tooltipEleRect.width / 2);
                    break;
                case BOTTOM_LEFT:
                    pos.top += (tipHeight + tipAdjust);
                    pos.left -= (tooltipEleRect.width - tipWidthAdjust);
                    break;
                case LEFT_BOTTOM:
                    pos.left -= (tipWidth + tooltipEleRect.width + tipAdjust);
                    pos.top -= (tipHeightAdjust);
                    break;
                case LEFT_CENTER:
                    pos.left -= (tipWidth + tooltipEleRect.width + tipAdjust);
                    pos.top -= (tooltipEleRect.height / 2);
                    break;
                case LEFT_TOP:
                    pos.left -= (tipWidth + tooltipEleRect.width + tipAdjust);
                    pos.top -= (tooltipEleRect.height - tipHeightAdjust);
                    break;
                case TOP_LEFT:
                    pos.top -= (tooltipEleRect.height + tipHeight + tipAdjust);
                    pos.left -= (tooltipEleRect.width - tipWidthAdjust);
                    break;
                case TOP_RIGHT:
                    pos.top -= (tooltipEleRect.height + tipHeight + tipAdjust);
                    pos.left -= (tipWidthAdjust);
                    break;
                default:
                    pos.top -= (tooltipEleRect.height + tipHeight + tipAdjust);
                    pos.left -= (tooltipEleRect.width / 2);
                    break;
            }
            pos.left += this.properties.offsetX;
            pos.top += this.properties.offsetY;
            return pos;
        };
        SfTooltip.prototype.setTipClass = function (position) {
            if (position.indexOf(RIGHT) === 0) {
                this.tipClass = TIP_LEFT;
                this.tipIconClass = TIP_ICON_LEFT;
            }
            else if (position.indexOf(BOTTOM) === 0) {
                this.tipClass = TIP_TOP;
                this.tipIconClass = TIP_ICON_TOP;
            }
            else if (position.indexOf(LEFT) === 0) {
                this.tipClass = TIP_RIGHT;
                this.tipIconClass = TIP_ICON_RIGHT;
            }
            else {
                this.tipClass = TIP_BOTTOM;
                this.tipIconClass = TIP_ICON_BOTTOM;
            }
        };
        SfTooltip.prototype.updateTipPosition = function (position) {
            var selEle = this.selectAll('.' + ARROW_TIP + ',.' + ARROW_TIP_OUTER + ',.' + ARROW_TIP_INNER, this.tooltipEle);
            var removeList = [TIP_BOTTOM, TIP_TOP, TIP_LEFT, TIP_RIGHT];
            sfBlazorToolkit.base.removeClass(selEle, removeList);
            var arrowInnerELe = this.select('.' + ARROW_TIP_INNER, this.tooltipEle);
            if (arrowInnerELe) {
                sfBlazorToolkit.base.removeClass([arrowInnerELe], [TIP_ICON_BOTTOM, TIP_ICON_TOP, TIP_ICON_LEFT, TIP_ICON_RIGHT]);
            }
            this.setTipClass(position);
            sfBlazorToolkit.base.addClass(selEle, this.tipClass);
            if (arrowInnerELe) {
                sfBlazorToolkit.base.addClass([arrowInnerELe], [this.tipIconClass]);
            }
        };
        SfTooltip.prototype.adjustArrow = function (target, position, tooltipPositionX, tooltipPositionY, isCollided) {
            if (isCollided === void 0) { isCollided = false; }
            if (this.properties.showTipPointer === false) {
                return;
            }
            this.updateTipPosition(position);
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'display': 'block' });
            var tooltipWidth = this.tooltipEle.clientWidth;
            var tooltipHeight = this.tooltipEle.clientHeight;
            var arrowEle = this.select('.' + ARROW_TIP, this.tooltipEle);
            var arrowInnerELe = this.select('.' + ARROW_TIP_INNER, this.tooltipEle);
            // Guard: arrow elements may not exist when showTipPointer is toggled dynamically.
            if (!arrowEle || !arrowInnerELe) { return; }
            var tipWidth = arrowEle.offsetWidth;
            var tipHeight = arrowEle.offsetHeight;
            var leftValue;
            var topValue;
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'display': '' });
            var isEndPosition = this.properties.tipPointerPosition === END;
            var isStartPosition = this.properties.tipPointerPosition === START;
            if (this.tipClass === TIP_BOTTOM || this.tipClass === TIP_TOP) {
                topValue = this.tipClass === TIP_BOTTOM ? '99.9%' : -(tipHeight - 1) + 'px';
                sfBlazorToolkit.base.setStyleAttribute(arrowInnerELe, { 'top': "-" + (this.tipClass === TIP_BOTTOM ? tipHeight - 2 : tipHeight - 6) + "px" });
                if (target) {
                    var tipPosExclude = tooltipPositionX !== 'Center' || (tooltipWidth > target.offsetWidth) ||
                        this.properties.mouseTrail;
                    if ((tipPosExclude && tooltipPositionX === 'Left') || (!tipPosExclude && isEndPosition)) {
                        leftValue = (tooltipWidth - tipWidth - POINTER_ADJUST) + 'px';
                    }
                    else if ((tipPosExclude && tooltipPositionX === 'Right') || (!tipPosExclude && isStartPosition)) {
                        leftValue = POINTER_ADJUST + 'px';
                    }
                    else if ((tipPosExclude) && (isEndPosition || isStartPosition)) {
                        leftValue = isEndPosition ? ((target.offsetWidth + ((this.tooltipEle.offsetWidth - target.offsetWidth) / 2)) - (tipWidth / 2)) - POINTER_ADJUST + 'px'
                            : ((this.tooltipEle.offsetWidth - target.offsetWidth) / 2) - (tipWidth / 2) + POINTER_ADJUST + 'px';
                    }
                    else {
                        leftValue = ((tooltipWidth / 2) - (tipWidth / 2)) + 'px';
                    }
                }
            }
            else {
                leftValue = this.tipClass === TIP_RIGHT ? '99.9%' : -(tipWidth - 1) + 'px';
                sfBlazorToolkit.base.setStyleAttribute(arrowInnerELe, { 'left': (this.tipClass === TIP_RIGHT ? -(tipWidth - 2) : -2) + "px" });
                var targetRect = target.getBoundingClientRect();
                var tooltipEleRect = this.tooltipEle.getBoundingClientRect();
                var tipPosExclude = tooltipPositionY !== CENTER || (tooltipHeight > target.offsetHeight) ||
                    this.properties.mouseTrail;
                if ((tipPosExclude && tooltipPositionY === TOP) || (!tipPosExclude && isEndPosition)) {
                    topValue = (tooltipHeight - tipHeight - POINTER_ADJUST) + 'px';
                }
                else if ((tipPosExclude && tooltipPositionY === BOTTOM) || (!tipPosExclude && isStartPosition)) {
                    topValue = POINTER_ADJUST + 'px';
                }
                else {
                    topValue = isCollided ? (targetRect.top - tooltipEleRect.top) + targetRect.height / 2 - tipHeight / 2 + 'px' : ((tooltipHeight / 2) - (tipHeight / 2)) + 'px';
                }
            }
            if (window.scrollX && this.properties.position === BOTTOM_RIGHT) {
                leftValue = this.element.offsetLeft + 'px';
            }
            sfBlazorToolkit.base.setStyleAttribute(arrowEle, { 'top': topValue, 'left': leftValue });
        };
        SfTooltip.prototype.tooltipHover = function () {
            if (this.tooltipEle) {
                this.isTooltipOpen = true;
            }
        };
        SfTooltip.prototype.tooltipMouseOut = function (e) {
            this.isTooltipOpen = false;
            this.hideTooltip(this.properties.animation.close, e, this.findTarget());
        };
        SfTooltip.prototype.onMouseOut = function (e) {
            var target = this.getValidTarget(e);
            if (!target) {
                return;
            }
            var enteredElement = e.relatedTarget;
            this.mouseAction = true;
            if (enteredElement && !this.properties.mouseTrail) {
                var checkForTooltipElement = sfBlazorToolkit.base.closest(enteredElement, "." + TOOLTIP_WRAP + "." + POPUP_LIB + "." + POPUP_ROOT);
                if (checkForTooltipElement) {
                    sfBlazorToolkit.base.EventHandler.add(checkForTooltipElement, 'mouseleave', this.tooltipElementMouseOut, this);
                }
                else {
                    this.hideTooltip(this.properties.animation.close, e, this.findTarget());
                    if (this.properties.closeDelay === 0) {
                        this.clear();
                    }
                }
            }
            else {
                this.hideTooltip(this.properties.animation.close, e, this.findTarget());
                this.clear();
            }
        };
        SfTooltip.prototype.tooltipElementMouseOut = function (e) {
            this.hideTooltip(this.properties.animation.close, e, this.findTarget());
            sfBlazorToolkit.base.EventHandler.remove(this.element, 'mouseleave', this.tooltipElementMouseOut);
            this.clear();
        };
        SfTooltip.prototype.onMouseMove = function (event) {
            if (!this.tooltipEle) {
                return;
            }
            var eventPageX = 0;
            var eventPageY = 0;
            if (event.type.indexOf('touch') > -1) {
                event.preventDefault();
                eventPageX = event.touches[0].pageX;
                eventPageY = event.touches[0].pageY;
            }
            else {
                eventPageX = event.pageX;
                eventPageY = event.pageY;
            }

            if (this.contentAnimation !== null)
            {
                sfBlazorToolkit.Animation.stop(this.tooltipEle, this.contentAnimation);
            }
           
            sfBlazorToolkit.base.removeClass([this.tooltipEle], POPUP_CLOSE);
            sfBlazorToolkit.base.addClass([this.tooltipEle], POPUP_OPEN);
            this.adjustArrow(event.target, this.properties.position, this.tooltipPositionX, this.tooltipPositionY);
            var scalingFactors = this.getScalingFactor(event.target);
            var pos = this.calculateTooltipOffset(this.properties.position, scalingFactors.x, scalingFactors.y);
            var x = eventPageX + pos.left + this.properties.offsetX;
            var y = eventPageY + pos.top + this.properties.offsetY;
            var elePos = this.checkCollision(event.target, x, y);
            if (this.tooltipPositionX !== elePos.horizontal || this.tooltipPositionY !== elePos.vertical) {
                var newPos = (this.properties.position.indexOf(BOTTOM) === 0 || this.properties.position.indexOf(TOP) === 0) ?
                    elePos.vertical + elePos.horizontal : elePos.horizontal + elePos.vertical;
                elePos.position = newPos;
                this.adjustArrow(event.target, elePos.position, elePos.horizontal, elePos.vertical);
                var colPos = this.calculateTooltipOffset(elePos.position, scalingFactors.x, scalingFactors.y);
                elePos.left = eventPageX + colPos.left - this.properties.offsetX;
                elePos.top = eventPageY + colPos.top - this.properties.offsetY;
            }
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'left': (elePos.left + 'px'), 'top': (elePos.top + 'px') });
        };
        SfTooltip.prototype.keyDown = function (event) {
            // Use event.key (standard) instead of the deprecated event.keyCode.
            if (this.tooltipEle && event.key === 'Escape') {
                this.isClick = true;
                this.hideTooltip(this.properties.animation.close);
                this.isClick = false;
            }
        };
        SfTooltip.prototype.touchEnd = function (e) {
            if (this.tooltipEle && sfBlazorToolkit.base.closest(e.target, '.' + ROOT) === null && !this.properties.isSticky) {
                this.hideTooltip(this.properties.animation.close);
            }
        };
        SfTooltip.prototype.scrollHandler = function (e) {
            if (this.tooltipEle) {
                if (!(sfBlazorToolkit.base.closest(e.target, "." + TOOLTIP_WRAP + "." + POPUP_LIB + "." + POPUP_ROOT))) {
                    this.hideTooltip(this.properties.animation.close);
                }
            }
        };
        SfTooltip.prototype.renderContent = function (target) {
            // Guard: target must exist before reading attributes.
            if (!target) { return; }
            var title = getAttrOrDefault(target, 'sf-tooltip', null);
            var dataTitle = getAttrOrDefault(target, 'data-title', null);
            if (!sfBlazorToolkit.base.isNullOrUndefined(title) && target) {
                setAttributes(target, { 'data-content': title });
                this.isMultiTarget = true;
            }
            else if (!sfBlazorToolkit.base.isNullOrUndefined(dataTitle) && target) {
                setAttributes(target, { 'data-content': dataTitle });
                this.isMultiTarget = true;
            }
            if (target.title == '') {
                target.removeAttribute('title');
            }
            if (!this.properties.content) {
                var tooltipContent = this.select('.' + TIP_CONTENT, this.tooltipEle);
                tooltipContent.innerText = target.getAttribute('data-content');
            }
        };
        SfTooltip.prototype.setHeightWidth = function (widthValue, heightValue, target) {
            if (this.tooltipEle) {
                sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'height': heightValue, 'width': widthValue });
            }
            if (target && !sfBlazorToolkit.base.isNullOrUndefined(this.tooltipEle) && this.tooltipEle.style.width !== 'auto') {
                this.tooltipEle.style.maxWidth = widthValue;
            }
        };
        SfTooltip.prototype.appendContainer = function () {
            if (typeof this.properties.container == 'string') {
                if (this.properties.container === 'body') {
                    this.containerElement = document.body;
                }
                else {
                    this.isBodyContainer = false;
                    this.containerElement = this.select(this.properties.container, this.document);
                    sfBlazorToolkit.base.addClass([this.containerElement], POPUP_CONTAINER);
                }
            }
            this.containerElement.appendChild(this.tooltipEle);
        };
        SfTooltip.prototype.updateTarget = function () {
            if (this.contentTargetValue) {
                if (sfBlazorToolkit.base.Browser.isDevice) {
                    sfBlazorToolkit.base.addClass([this.tooltipEle], BIGGER);
                }
                this.appendContainer();
                sfBlazorToolkit.base.removeClass([this.tooltipEle], HIDDEN);
                this.addDescribedBy(this.contentTargetValue, this.ctrlId + CONTENT);
                this.renderContent(this.contentTargetValue);
                sfBlazorToolkit.base.addClass([this.tooltipEle], POPUP_OPEN);
                this.renderPopup(this.contentTargetValue);
                var pos = this.properties.position;
                this.adjustArrow(this.contentTargetValue, pos, this.tooltipPositionX, this.tooltipPositionY);
                sfBlazorToolkit.Animation.stop(this.tooltipEle, null);
                this.reposition(this.contentTargetValue);
                this.afterContentRender();
            }
        };
        SfTooltip.prototype.contentUpdated = function () {
            if (!this.tooltipEle) {
                if (!this.element) {
                    return;
                }
                this.ctrlId = this.element.id;
                this.tooltipEle = this.select('#' + this.ctrlId + CONTENT, this.document);
                if (this.tooltipEle) {
                    this.setHeightWidth(this.properties.width, this.properties.height, this.contentTargetValue);
                    this.updateTarget();
                }
            }
            else {
                if (!this.isContinuousOpen || this.isRestrictUpdate) {
                    return;
                }
                this.updateTarget();
            }
        };
        SfTooltip.prototype.afterContentRender = function () {
            sfBlazorToolkit.base.removeClass([this.tooltipEle], POPUP_OPEN);
            sfBlazorToolkit.base.addClass([this.tooltipEle], POPUP_CLOSE);
            this.tooltipEventArgs = {
                type: this.contentEvent ? this.contentEvent.type.toString() : null,
                isInteracted: !sfBlazorToolkit.base.isNullOrUndefined(this.contentEvent),
                hasText: this.hasText(),
                target: this.getDomObject(TARGET, this.contentTargetValue),
                name: 'beforeOpen',
                cancel: false,
                event: this.contentEvent ? this.contentEvent : null,
                element: this.getDomObject('tooltipElement', this.tooltipEle),
                left: this.contentEvent ? this.getXYValue(this.contentEvent, 'x') : null,
                top: this.contentEvent ? this.getXYValue(this.contentEvent, 'y') : null
            };
            this.isRestrictUpdate = this.eventList.beforeOpen && !this.isHidden();
            if (this.eventList.beforeOpen) {
                this.triggerEvent('TriggerBeforeOpenEventAsync', this.tooltipEventArgs);
            }
            else {
                this.beforeOpenCallBack(false);
            }
        };
        SfTooltip.prototype.beforeOpenCallBack = function (cancel) {
            if (cancel) {
                this.isPopupHidden = true;
                if (this.contentTargetValue) {
                    this.popupHide(this.properties.animation.close, this.contentTargetValue);
                }
                this.mouseMoveBeforeRemove();
            }
            else {
                if (sfBlazorToolkit.base.isNullOrUndefined(this.contentAnimation)) {
                    return;
                }
                var openAnimation = {
                    name: (this.contentAnimation.effect === NONE && getAnimMode() === ENABLE) ? 'FadeIn' : this.contentAnimation.effect,
                    duration: this.contentAnimation.duration,
                    delay: this.contentAnimation.delay,
                    timingFunction: 'easeOut'
                };
                if ((this.contentAnimation.effect === NONE && getAnimMode() !== ENABLE) || getAnimMode() === DISABLE) {
                    openAnimation = undefined;
                }
                if (this.popupObj) {
                    this.popupObj.show(openAnimation, this.contentTargetValue);
                    if (this.properties.openDelay && this.properties.mouseTrail && this.mouseMoveEvent) {
                        this.onMouseMove(this.mouseMoveEvent);
                    }
                }
            }
            if (this.contentEvent) {
                this.wireMouseEvents(this.contentEvent, this.contentTargetValue);
            }
            this.contentTargetValue = this.contentEvent = this.contentAnimation = null;
        };
        SfTooltip.prototype.reposition = function (target) {
            if (target === null || !this.tooltipEle) {
                return;
            }
            var elePos = this.getTooltipPosition(target);
            if (this.popupObj) {
                this.popupObj.position = { X: elePos.left, Y: elePos.top };
                this.popupObj.refreshPosition();
            }
        };
        SfTooltip.prototype.renderPopup = function (target) {
            var elePos = this.properties.mouseTrail ? { top: 0, left: 0 } : this.getTooltipPosition(target);
            this.tooltipEle.classList.remove(POPUP_LIB);
            this.popupObj = new sfBlazorToolkit.popups.Popup(this.tooltipEle, {
                height: this.properties.height,
                width: this.properties.width,
                position: { X: elePos.left, Y: elePos.top },
                enableRtl: this.properties.enableRtl,
                open: this.openPopupHandler.bind(this),
                close: this.closePopupHandler.bind(this)
            });
        };
        SfTooltip.prototype.openPopupHandler = function () {
            if (this.tooltipEle && this.tooltipEle.classList.contains(POPUP_CLOSE) && this.isMultiTarget) {
                sfBlazorToolkit.base.removeClass([this.tooltipEle], POPUP_CLOSE);
                sfBlazorToolkit.base.addClass([this.tooltipEle], POPUP_OPEN);
            }
            if (!this.properties.mouseTrail) {
                this.reposition(this.contentTargetValue ? this.contentTargetValue : this.findTarget());
            }
            this.tooltipEventArgs.name = 'Opened';
            this.isRestrictUpdate = this.eventList.opened && !this.isHidden();
            if (this.eventList.opened) {
                this.triggerEvent('TriggerOpenedEventAsync', this.tooltipEventArgs);
            }
            if (this.mouseAction && !sfBlazorToolkit.base.isNullOrUndefined(this.tooltipEle)) {
                this.hideTooltip(this.properties.animation.close);
            }
        };
        SfTooltip.prototype.closePopupHandler = function () {
            this.clear();
            this.tooltipEventArgs.name = 'Closed';
            this.isRestrictUpdate = this.eventList.closed && !this.isHidden();
            if (this.eventList.closed) {
                this.triggerEvent('TriggerClosedEventAsync', this.tooltipEventArgs);
            }
        };
        SfTooltip.prototype.getTooltipPosition = function (target) {
            // Guard: non-body container must be present before calling getBoundingClientRect.
            if (!this.isBodyContainer && !this.containerElement) {
                return { left: 0, top: 0 };
            }
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'display': 'block' });
            this.formatPosition();
            var pos = sfBlazorToolkit.popups.calculatePosition(target, this.tooltipPositionX, this.tooltipPositionY, !this.isBodyContainer, this.isBodyContainer ? null : this.containerElement.getBoundingClientRect());
            var scalingFactors = this.getScalingFactor(target);
            var offsetPos = this.calculateTooltipOffset(this.properties.position, scalingFactors.x, scalingFactors.y);
            var collisionPosition = this.calculateElementPosition(pos, offsetPos);
            var elePos = this.collisionFlipFit(target, collisionPosition[0], collisionPosition[1]);
            elePos.left = elePos.left / scalingFactors.x;
            elePos.top = elePos.top / scalingFactors.y;
            sfBlazorToolkit.base.setStyleAttribute(this.tooltipEle, { 'display': '' });
            return elePos;
        };
        SfTooltip.prototype.getDomObject = function (value, element) {
            if (!element) return null;
            return {
                id: element.id,
                class: element.className,
                xPath: createXPath(element),
                domUUID: value + sfBlazorToolkit.base.getUniqueID(value)
            };
        };
        SfTooltip.prototype.hasText = function () {
            return this.tooltipEle ? (this.tooltipEle.innerText.trim() === '' ? false : true) : false;
        };
        SfTooltip.prototype.getXYValue = function (e, direction) {
            var touchList = e.changedTouches;
            var value;
            if (direction === 'x') {
                value = touchList ? touchList[0].clientX : e.clientX;
            }
            else {
                value = touchList ? touchList[0].clientY : e.clientY;
            }
            if (!value && e.type === 'focus' && e.target) {
                var rect = e.target.getBoundingClientRect();
                value = rect ? (direction === 'x' ? rect.left : rect.top) : null;
            }
            return Math.ceil(value);
        };
        SfTooltip.prototype.destroy = function () {
            // Clear pending timers before nulling the element to prevent a fired
            // timer invoking .NET interop on an already-destroyed instance.
            clearTimeout(this.showTimer);
            clearTimeout(this.hideTimer);
            if (this.tooltipEle) {
                var placeholder = this.select('#' + this.ctrlId + PLACEHOLDER, this.document);
                if (placeholder) {
                    placeholder.appendChild(this.tooltipEle);
                }
            }
            if (this.popupObj) {
                this.popupObj.destroy();
            }
            sfBlazorToolkit.base.removeClass([this.element], ROOT);
            this.unWireEvents(this.properties.opensOn);
            this.unWireMouseEvents(this.element);
            this.tooltipEle = null;
            this.popupObj = null;
            this.element = null;
        };

        SfTooltip.prototype.selectAll = function selectAll(selector, context) {
            context = context || document;
            return Array.from(context.querySelectorAll(selector));
        };

        SfTooltip.prototype.select = function select(selector, context) {
            if (context === void 0) { context = document; }
            return context.querySelector(selector);
        };

        return SfTooltip;
    }());
    // eslint-disable-next-line
   
      export function wireEvents(dataId, element, dotnetRef, properties, eventList) {
            liveRefs.set(dataId, dotnetRef);
            new SfTooltip(dataId, element, dotnetRef, properties, eventList);
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (element && !sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.formatPosition();
                instance.wireEvents(properties.opensOn);
            }
        }
      export function contentUpdated(dataId) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.contentUpdated();
            }
        }
        export function beforeRenderCallBack(dataId, cancel) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.beforeRenderCallBack(cancel);
            }
        }
       export function beforeOpenCallBack(dataId, cancel) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.beforeOpenCallBack(cancel);
            }
        }
       export function beforeCloseCallBack(dataId, cancel) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.beforeCloseCallBack(cancel);
            }
        }
        export function showTooltip(dataId, animation, targetProp) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                return;
            }
            var target = null;
            if (targetProp !== null && targetProp !== '') {
                var tp = (typeof targetProp === 'string') ? targetProp.trim() : targetProp;
                // If passed a selector with leading '#', try getElementById first for performance.
                if (typeof tp === 'string') {
                    if (tp.charAt(0) === '#') {
                        var id = tp.substring(1);
                        var elementById = document.getElementById(id);
                        if (elementById) {
                            target = elementById;
                        }
                        else {
                            var inlineTarget = instance.element ? instance.select(tp, instance.element) : null;
                            var documentTarget = instance.selectAll(tp);
                            if (inlineTarget) {
                                target = inlineTarget;
                            }
                            else if (documentTarget) {
                                target = documentTarget;
                            }
                        }
                    }
                    else {
                        // If looks like a bare id (no selector meta chars), try getElementById
                        var hasSelectorChars = /[\s.#>+~\[\]]/.test(tp);
                        if (!hasSelectorChars) {
                            var elementByIdCandidate = document.getElementById(tp);
                            if (elementByIdCandidate) {
                                target = elementByIdCandidate;
                            }
                            else {
                                var inlineTarget2 = instance.element ? instance.select(tp, instance.element) : null;
                                var documentTarget2 = instance.select(tp);
                                if (inlineTarget2) {
                                    target = inlineTarget2;
                                }
                                else if (documentTarget2) {
                                    target = documentTarget2;
                                }
                            }
                        }
                        else {
                            var inlineTarget3 = instance.element ? instance.select(tp, instance.element) : null;
                            var documentTarget3 = instance.select(tp);
                            if (inlineTarget3) {
                                target = inlineTarget3;
                            }
                            else if (documentTarget3) {
                                target = documentTarget3;
                            }
                        }
                    }
                }
            }
            else {
                target = instance.element;
            }
            // Guard: target must be resolved and visible before showing.
            if (!target) {
                return;
            }
            if (target.offsetWidth === 0) {
                return;
            }
            instance.showTooltip(target, animation, null);
        }
       export function hideTooltip(dataId, animation, isClick) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.isClick = isClick;
                instance.hideTooltip(animation);
                instance.isClick = false;
            }
        }
       export function destroy(dataId) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            liveRefs.delete(dataId);
            if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                instance.destroy();
            }
        }
        export function refresh(dataId) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                return;
            }
            if (!instance.isPopupHidden) {
                instance.hideTooltip(instance.properties.animation.close);
            }
            instance.unWireEvents(instance.properties.opensOn);
            instance.wireEvents(instance.properties.opensOn);
        }
        export function refreshPosition(dataId, targetEle, targetProp) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                return;
            }
            if (targetEle === null) {
                targetEle = targetProp !== null && targetProp !== '' ? instance.select(targetProp, instance.element) : instance.element;
            }
            instance.reposition(targetEle);
        }
       export function updateProperties(dataId, props) {
            var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
            if (sfBlazorToolkit.base.isNullOrUndefined(instance)) {
                return;
            }
            instance.isRestrictUpdate = true;
            if (props.target || props.targetContainer || props.opensOn || props.isSticky) {
                instance.unWireEvents(instance.properties.opensOn);
            }
            instance.properties = __assign({}, instance.properties, props);
            if (props.target || props.targetContainer || props.opensOn || props.isSticky) {
                instance.wireEvents(instance.properties.opensOn);
                instance.formatPosition();
            }
            else {
                var target = instance.findTarget();
                if (props.container) {
                    if (!sfBlazorToolkit.base.isNullOrUndefined(this.containerElement)) {
                        sfBlazorToolkit.base.removeClass([this.containerElement], POPUP_CONTAINER);
                    }
                    if (instance.tooltipEle && target) {
                        instance.appendContainer();
                    }
                }
                if (props.height || props.width) {
                    instance.setHeightWidth(props.width, props.height, target);
                }
                if (props.position) {
                    instance.formatPosition();
                }
                if (instance.tooltipEle && target) {
                    if (props.position && instance.properties.showTipPointer) {
                        var arrowInnerELe = instance.select('.' + ARROW_TIP_INNER, instance.tooltipEle);
                        var arrowEle = instance.select('.' + ARROW_TIP, instance.tooltipEle);
                        sfBlazorToolkit.base.removeClass([arrowEle], [instance.tipClass]);
                        if (arrowInnerELe) {
                            sfBlazorToolkit.base.removeClass([arrowInnerELe], [instance.tipIconClass]);
                        }
                        instance.setTipClass(props.position);
                        sfBlazorToolkit.base.addClass([arrowEle], [instance.tipClass]);
                        if (arrowInnerELe) {
                            sfBlazorToolkit.base.addClass([arrowInnerELe], [instance.tipIconClass]);
                        }
                        sfBlazorToolkit.base.setStyleAttribute(arrowInnerELe, { 'top': null, 'left': null });
                    }
                    if (instance.select('.' + ARROW_TIP, instance.tooltipEle)) {
                        instance.reposition(target);
                    }
                }
                instance.isRestrictUpdate = false;
            }
        }
