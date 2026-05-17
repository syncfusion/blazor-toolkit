const Resize = (function (exports) {
    'use strict';

    const elementClass = ['north-west', 'north', 'north-east', 'west', 'east', 'south-west', 'south', 'south-east'];
    const RESIZE_HANDLER = 'e-resize-handle';
    const FOCUSED_HANDLER = 'e-focused-handle';
    const DIALOG_RESIZABLE = 'e-dlg-resizable';
    const RESTRICT_LEFT = ['e-restrict-left'];
    const RESIZE_WITHIN_VIEWPORT = 'e-resize-viewport';
    const dialogBorderResize = ['north', 'west', 'east', 'south'];

    let targetElement; // element being resized
    let selectedHandler; // current handle
    let originalWidth = 0, originalHeight = 0, originalX = 0, originalY = 0;
    let originalMouseX = 0, originalMouseY = 0;
    let minHeight, maxHeight, minWidth, maxWidth;
    let containerElement;
    let resizeStart = null, resize = null, resizeEnd = null;
    let resizeWestWidth, resizeCount = 0, setLeft = true, previousWidth = 0, setWidth = true;

    const isNil = v => sfBlazorToolkit.base.isNullOrUndefined(v);
    const getEl = v => isNil(v) ? null : (typeof v === 'string' ? document.querySelector(v) : v);
    const rect = el => el.getBoundingClientRect();

    function createResize(args) {
        resizeStart = args.resizeBegin; resize = args.resizing; resizeEnd = args.resizeComplete;
        targetElement = getEl(args.element); containerElement = getEl(args.boundary);
        (args.direction || '').split(' ').forEach(d => {
            if (!d || !d.trim()) return;
            var iconClass = d === 'south-east' ? 'e-toolkit-icons e-resizer-right ' : 'e-toolkit-icons ';
            if (args.proxy.enableRtl && d === 'south-west') {
                iconClass += 'e-resizer-right ';
            }
            if (dialogBorderResize.indexOf(d) >= 0) setBorderResizeElm(d);
            else targetElement.appendChild(sfBlazorToolkit.base.createElement('div', { className: iconClass + RESIZE_HANDLER + ' e-' + d }));
        });
        minHeight = args.minHeight; minWidth = args.minWidth; maxWidth = args.maxWidth; maxHeight = args.maxHeight;
        resizeCount++;
        wireEvents(args.proxy && args.proxy.element && args.proxy.element.classList.contains('e-dialog') ? args.proxy : null);
    }

    function setBorderResizeElm(direction) {
        calculateValues();
        const span = sfBlazorToolkit.base.createElement('span', { className: 'e-dialog-border-resize e-' + direction });
        span.setAttribute('unselectable', 'on'); span.setAttribute('contenteditable', 'false');
        if (direction === 'south' || direction === 'north') {
            Object.assign(span.style, { height: '2px', width: '100%', left: '0px' });
            span.style[direction === 'south' ? 'bottom' : 'top'] = '0px';
        } else {
            Object.assign(span.style, { height: '100%', width: '2px', top: '0px' });
            span.style[direction === 'east' ? 'right' : 'left'] = '0px';
        }
        targetElement.appendChild(span);
    }

    function wireEvents(context) {
        const ctx = context || this;
        const resizers = targetElement.querySelectorAll('.' + RESIZE_HANDLER);
        const eventName = (sfBlazorToolkit.base.Browser.info.name === 'msie') ? 'pointerdown' : 'touchstart';
        resizers.forEach(r => { sfBlazorToolkit.base.EventHandler.add(r, 'mousedown', onMouseDown, ctx); sfBlazorToolkit.base.EventHandler.add(r, eventName, onTouchStart, ctx); });
        const borderResizers = targetElement.querySelectorAll('.e-dialog-border-resize');
        if (!isNil(borderResizers)) borderResizers.forEach(b => { sfBlazorToolkit.base.EventHandler.add(b, 'mousedown', onMouseDown, ctx); sfBlazorToolkit.base.EventHandler.add(b, eventName, onTouchStart, ctx); });
    }

    function getEventType(e) { return (e.indexOf('mouse') > -1) ? 'mouse' : 'touch'; }

    function onMouseDown(e) {
        e.preventDefault();
        targetElement = e.target.closest('.e-dlg-resizable, .e-dialog') || targetElement;
        calculateValues();
        originalMouseX = e.pageX; originalMouseY = e.pageY;
        e.target.classList.add(FOCUSED_HANDLER);
        if (resizeStart && resizeStart(e, this) === true) return;
        if (this.targetEle && targetElement && targetElement.querySelector('.' + DIALOG_RESIZABLE)) {
            containerElement = this.target === 'body' ? null : this.targetEle;
            maxWidth = this.targetEle.clientWidth; maxHeight = this.targetEle.clientHeight;
        }
        const target = isNil(containerElement) ? document : containerElement;
        sfBlazorToolkit.base.EventHandler.add(target, 'mousemove', onMouseMove, this);
        sfBlazorToolkit.base.EventHandler.add(document, 'mouseup', onMouseUp, this);
        setLeft = !RESTRICT_LEFT.some(c => targetElement.classList.contains(c));
    }

    function onMouseUp(e) {
        const touchMoveEvent = (sfBlazorToolkit.base.Browser.info.name === 'msie') ? 'pointermove' : 'touchmove';
        const touchEndEvent = (sfBlazorToolkit.base.Browser.info.name === 'msie') ? 'pointerup' : 'touchend';
        const target = isNil(containerElement) ? document : containerElement;
        const eventName = (sfBlazorToolkit.base.Browser.info.name === 'msie') ? 'pointerdown' : 'touchstart';
        sfBlazorToolkit.base.EventHandler.remove(target, 'mousemove', onMouseMove);
        sfBlazorToolkit.base.EventHandler.remove(target, touchMoveEvent, onMouseMove);
        sfBlazorToolkit.base.EventHandler.remove(target, eventName, onMouseMove);
        const focused = document.body.querySelector('.' + FOCUSED_HANDLER);
        if (!isNil(focused)) focused.classList.remove(FOCUSED_HANDLER);
        if (resizeEnd) resizeEnd(e, this);
        sfBlazorToolkit.base.EventHandler.remove(document, 'mouseup', onMouseUp);
        sfBlazorToolkit.base.EventHandler.remove(document, touchEndEvent, onMouseUp);
    }

    function calculateValues() {
        originalWidth = parseFloat(getComputedStyle(targetElement, null).getPropertyValue('width').replace('px', '')) || 0;
        originalHeight = parseFloat(getComputedStyle(targetElement, null).getPropertyValue('height').replace('px', '')) || 0;
        const r = targetElement.getBoundingClientRect(); originalX = r.left; originalY = r.top;
    }

    function onTouchStart(e) {
        targetElement = e.target.closest('.e-dlg-resizable, .e-dialog') || targetElement;
        calculateValues();
        const dialogResizeElement = targetElement.classList.contains('e-dialog');
        if ((e.target.classList.contains(RESIZE_HANDLER) || e.target.classList.contains('e-dialog-border-resize')) && dialogResizeElement) e.target.classList.add(FOCUSED_HANDLER);
        const coordinates = e.touches ? e.changedTouches[0] : e; originalMouseX = coordinates.pageX; originalMouseY = coordinates.pageY;
        if (resizeStart && resizeStart(e, this) === true) return;
        const touchMoveEvent = (sfBlazorToolkit.base.Browser.info.name === 'msie') ? 'pointermove' : 'touchmove';
        const touchEndEvent = (sfBlazorToolkit.base.Browser.info.name === 'msie') ? 'pointerup' : 'touchend';
        const target = isNil(containerElement) ? document : containerElement;
        sfBlazorToolkit.base.EventHandler.add(target, touchMoveEvent, onMouseMove, this);
        sfBlazorToolkit.base.EventHandler.add(document, touchEndEvent, onMouseUp, this);
    }

    function onMouseMove(e) {
        let focused = document.body.querySelector('.' + FOCUSED_HANDLER);
        if (focused) {
            selectedHandler = focused;
        } else if (e.target.classList && e.target.classList.contains(RESIZE_HANDLER) && e.target.classList.contains(FOCUSED_HANDLER)) {
            selectedHandler = e.target;
        }
        if (isNil(selectedHandler)) return;
        let resizeTowards = '';
        for (let i = 0; i < elementClass.length; i++) {
            if (selectedHandler.classList.contains('e-' + elementClass[i])) {
                resizeTowards = elementClass[i];
                break;
            }
        }
        if (resize) resize(e, this);
        switch (resizeTowards) {
            case 'south':
                resizeSouth(e);
                break;
            case 'north':
                resizeNorth(e);
                break;
            case 'west':
                resizeWest(e);
                break;
            case 'east':
                resizeEast(e);
                break;
            case 'south-east':
                resizeSouth(e);
                resizeEast(e);
                break;
            case 'south-west':
                resizeSouth(e);
                resizeWest(e);
                break;
            case 'north-east':
                resizeNorth(e);
                resizeEast(e);
                break;
            case 'north-west':
                resizeNorth(e);
                resizeWest(e);
                break;
        }
    }

    function getClientRectValues(element) { return element.getBoundingClientRect(); }

    function resizeSouth(e) {
        const pageY = (getEventType(e.type) === 'mouse') ? e.pageY : e.touches[0].pageY;
        const rect = getClientRectValues(targetElement);
        const diffY = pageY - originalMouseY;
        const newHeight = originalHeight + diffY;
        if (newHeight < minHeight || newHeight > maxHeight)
            return;
        if (!isNil(containerElement)) {
            const containerRect = getClientRectValues(containerElement);
            if (rect.top + newHeight > containerRect.bottom)
                return;
        }
        targetElement.style.height = newHeight + "px";
    }

    function resizeNorth(e) {
        const pageY = (getEventType(e.type) === 'mouse') ? e.pageY : e.touches[0].pageY;
        const rect = getClientRectValues(targetElement);
        const diffY = pageY - originalMouseY;
        const newHeight = originalHeight - diffY;
        const newTop = originalY + diffY;
        if (newHeight < minHeight || newHeight > maxHeight)
            return;
        if (!isNil(containerElement)) {
            const containerRect = getClientRectValues(containerElement);
            if (newTop < containerRect.top)
                return;
            if (newTop + newHeight > containerRect.bottom)
                return;
        }
        targetElement.style.height = newHeight + "px";
        targetElement.style.top = newTop + "px";
    }

    function resizeWest(e) {
        const pageX = (getEventType(e.type) === 'mouse') ? e.pageX : e.touches[0].pageX;
        const rect = getClientRectValues(targetElement);
        const diffX = pageX - originalMouseX;
        const newWidth = originalWidth - diffX;
        const newLeft = originalX + diffX;
        if (newWidth < minWidth || newWidth > maxWidth)
            return;
        if (!isNil(containerElement)) {
            const containerRect = getClientRectValues(containerElement);
            if (newLeft < containerRect.left)
                return;
            if (newLeft + newWidth > containerRect.right)
                return;
        }
        targetElement.style.width = newWidth + "px";
        targetElement.style.left = newLeft + "px";
    }

    function resizeEast(e) {
        const pageX = (getEventType(e.type) === 'mouse') ? e.pageX : e.touches[0].pageX;
        const rect = getClientRectValues(targetElement);
        const diffX = pageX - originalMouseX;
        const newWidth = originalWidth + diffX;
        if (newWidth < minWidth || newWidth > maxWidth)
            return;
        if (!isNil(containerElement)) {
            const containerRect = getClientRectValues(containerElement);
            if (rect.left + newWidth > containerRect.right)
                return;
        }
        targetElement.style.width = newWidth + "px";
    }

    function setMinHeight(minimumHeight) { minHeight = minimumHeight; }
    function setMaxWidth(value) { maxWidth = value; }
    function setMaxHeight(value) { maxHeight = value; }

    function removeResize() {
        const handlers = targetElement.querySelectorAll('.' + RESIZE_HANDLER);
        handlers.forEach(h => sfBlazorToolkit.base.detach(h));
        const borderResizers = targetElement.querySelectorAll('.e-dialog-border-resize');
        if (!isNil(borderResizers)) borderResizers.forEach(b => sfBlazorToolkit.base.detach(b));
    }

    function resizeDestroy() {
        resizeCount--;
        if (resizeCount === 0) {
            targetElement = null; selectedHandler = null; containerElement = null; resizeWestWidth = null; resizeStart = null; resize = null; resizeEnd = null;
        }
    }

    exports.createResize = createResize; exports.removeResize = removeResize; exports.resizeDestroy = resizeDestroy; exports.setMaxHeight = setMaxHeight; exports.setMaxWidth = setMaxWidth; exports.setMinHeight = setMinHeight;
    return exports;
})( {} );

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.Resize = window.sfBlazorToolkit.Resize || Resize;