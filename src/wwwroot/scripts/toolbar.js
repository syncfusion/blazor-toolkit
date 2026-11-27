/* eslint-disable no-undef */
// ES module for SfToolbar overflow measurement and popup placement - named exports only (no globals).
// Browser APIs used: ResizeObserver, MutationObserver, getBoundingClientRect, clientWidth and scrollWidth.

const instances = new WeakMap();
const overflowKeyTracks = new WeakMap();
const verticalWrappedObservers = new WeakMap();

function sizeOf(element, vertical) {
    const rect = element.getBoundingClientRect();
    return vertical ? rect.height : rect.width;
}

function gapOf(root, vertical) {
    const style = getComputedStyle(root);
    const value = parseFloat(vertical ? style.rowGap : style.columnGap);
    return Number.isFinite(value) ? value : 0;
}

function availableOf(root, vertical) {
    const style = getComputedStyle(root);
    const start = parseFloat(vertical ? style.paddingBlockStart : style.paddingInlineStart);
    const end = parseFloat(vertical ? style.paddingBlockEnd : style.paddingInlineEnd);
    const padding = (Number.isFinite(start) ? start : 0) + (Number.isFinite(end) ? end : 0);
    const box = root.getBoundingClientRect();
    return (vertical ? box.height : box.width) - padding;
}

function setStyleProperty(element, name, value) {
    if (element.style.getPropertyValue(name) !== value) {
        element.style.setProperty(name, value);
    }
}

/**
 * Sizes a vertical wrapped toolbar background to its columns without changing their width.
 */
export function sizeVerticalWrapped(root, content) {
    if (!root || !content) {
        return;
    }

    const rootStyle = getComputedStyle(root);
    const rootPadding = parseFloat(rootStyle.paddingInlineStart) + parseFloat(rootStyle.paddingInlineEnd);
    const rootBorder = parseFloat(rootStyle.borderInlineStartWidth) + parseFloat(rootStyle.borderInlineEndWidth);
    const contentWidth = content.getBoundingClientRect().width;
    const contentWidthText = contentWidth + 'px';
    const rootWidth = Math.max(content.scrollWidth, content.clientWidth) + rootPadding + rootBorder;

    setStyleProperty(root, '--e-toolbar-wrapped-root-inline-size', rootWidth + 'px');
    setStyleProperty(content, '--e-toolbar-wrapped-content-inline-size', contentWidthText);
}

/** Watches a vertical wrapped toolbar for height changes and dynamic item updates. */
export function observeVerticalWrapped(root, content) {
    if (!root || !content || verticalWrappedObservers.has(root)) {
        return;
    }

    let frame = 0;
    const update = function () {
        if (frame) {
            cancelAnimationFrame(frame);
        }
        frame = requestAnimationFrame(function () {
            frame = 0;
            sizeVerticalWrapped(root, content);
        });
    };
    const resizeObserver = new ResizeObserver(update);
    const mutationObserver = new MutationObserver(update);
    resizeObserver.observe(root);
    resizeObserver.observe(content);
    mutationObserver.observe(content, { attributes: true, characterData: true, childList: true, subtree: true });
    verticalWrappedObservers.set(root, {
        cancel: function () {
            if (frame) {
                cancelAnimationFrame(frame);
                frame = 0;
            }
        },
        resizeObserver: resizeObserver,
        mutationObserver: mutationObserver
    });
}

/** Stops vertical wrapped layout observation. Safe to call more than once. */
export function unobserveVerticalWrapped(root) {
    const instance = root ? verticalWrappedObservers.get(root) : null;
    if (!instance) {
        return;
    }
    instance.cancel();
    instance.resizeObserver.disconnect();
    instance.mutationObserver.disconnect();
    verticalWrappedObservers.delete(root);
}

/** Removes the browser-measured dimensions when the toolbar leaves vertical wrapped mode. */
export function clearVerticalWrappedSize(root, content) {
    if (root) {
        root.style.removeProperty('--e-toolbar-wrapped-root-inline-size');
    }
    if (content) {
        content.style.removeProperty('--e-toolbar-wrapped-content-inline-size');
    }
}

/**
 * Reads the sizes the .NET side needs for one calculation.
 * Every measurable item carries a data-toolbar-measure attribute holding its id; the result keeps DOM order.
 */
export function measure(root, vertical) {
    if (!root) {
        return null;
    }
    const items = Array.from(root.querySelectorAll('[data-toolbar-measure]'));
    const toolbar = root.parentElement;
    const button = toolbar?.querySelector('[data-testid="toolbar-overflow-button"]');
    const firstRect = items[0] ? items[0].getBoundingClientRect() : null;
    return {
        available: availableOf(toolbar || root, vertical),
        gap: gapOf(root, vertical),
        buttonSize: button ? sizeOf(button, vertical) : 0,
        items: items.map(function (element) {
            const rect = element.getBoundingClientRect();
            const row = firstRect ? Math.abs(vertical ? rect.left - firstRect.left : rect.top - firstRect.top) : 0;
            return { id: element.getAttribute('data-toolbar-measure'), size: sizeOf(element, vertical), row: row };
        })
    };
}

/**
 * Registers the resize and keyboard handlers and reads the initial layout in one browser interop.
 * The first render stays concealed on the .NET side until this measurement is applied.
 */
export function initialize(root, toolbar, dotNetRef, vertical) {
    const observing = observe(root, dotNetRef);
    trackOverflowKeys(toolbar);
    return {
        observing: observing,
        measurement: measure(root, vertical)
    };
}

/**
 * Starts observing the toolbar root. The .NET handler is called, debounced to one animation frame, whenever the
 * container size changes. The container is observed, not the viewport (TB-OVF-080).
 */
export function observe(root, dotNetRef) {
    if (!root || !dotNetRef) {
        return false;
    }
    const instance = instances.get(root) || {
        observer: null,
        scrollListener: null,
        cancel: function () { }
    };
    if (instance.observer) {
        return true;
    }
    let frame = 0;
    let lastWidth = -1;
    let lastHeight = -1;
    const observer = new ResizeObserver(function (entries) {
        const box = entries[0].contentRect;
        if (box.width === lastWidth && box.height === lastHeight) {
            return;
        }
        lastWidth = box.width;
        lastHeight = box.height;
        if (frame) {
            cancelAnimationFrame(frame);
        }
        frame = requestAnimationFrame(function () {
            frame = 0;
            dotNetRef.invokeMethodAsync('OnContainerResizedAsync').catch(function () { /* the circuit may be gone */ });
        });
    });
    observer.observe(root);
    instance.observer = observer;
    instance.cancel = function () {
        if (frame) {
            cancelAnimationFrame(frame);
            frame = 0;
        }
    };
    instances.set(root, instance);
    return true;
}

/**
 * Reports the scroll position of a scrollable toolbar whenever it scrolls or resizes, so the navigation
 * chevrons can be enabled and disabled at the start and the end of the range.
 *
 * The listener is passive, so it never blocks the touch swipe or the wheel that moved the toolbar.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {object} dotNetRef - the .NET reference the callback is invoked on.
 * @returns {void}
 */
export function observeScroll(scroller, dotNetRef, vertical) {
    if (!scroller || !dotNetRef) {
        return;
    }
    const instance = instances.get(scroller) || {
        observer: null,
        scrollListener: null,
        cancel: function () { }
    };
    if (instance.scrollListener) {
        return;
    }
    let frame = 0;
    instance.dotNetRef = dotNetRef;
    instance.vertical = vertical;
    instance.lastScrollState = scrollState(scroller, vertical);
    const schedule = function () {
        if (frame) {
            return;
        }
        frame = requestAnimationFrame(function () {
            frame = 0;
            notifyScrollState(scroller);
        });
    };
    const resizeObserver = new ResizeObserver(schedule);
    resizeObserver.observe(scroller);
    scroller.addEventListener('scroll', schedule, { passive: true });
    instance.scrollListener = schedule;
    instance.scrollResizeObserver = resizeObserver;
    instance.scrollCancel = function () {
        if (frame) {
            cancelAnimationFrame(frame);
            frame = 0;
        }
    };
    instances.set(scroller, instance);
}

/**
 * Stops listening for scrolling without disconnecting the toolbar resize observer.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @returns {void}
 */
export function unobserveScroll(scroller) {
    const instance = scroller ? instances.get(scroller) : null;
    if (!instance || !instance.scrollListener) {
        return;
    }
    instance.scrollCancel();
    instance.scrollResizeObserver.disconnect();
    instance.scrollResizeObserver = null;
    instance.scrollCancel = null;
    scroller.removeEventListener('scroll', instance.scrollListener);
    instance.scrollListener = null;
    instance.dotNetRef = null;
    instance.lastScrollState = null;
    if (!instance.observer) {
        instances.delete(scroller);
    }
}

/** Stops observing and releases the observer. Safe to call twice. */
export function unobserve(root) {
    const instance = root ? instances.get(root) : null;
    if (!instance) {
        return;
    }
    instance.cancel();
    if (instance.observer) {
        instance.observer.disconnect();
    }
    if (instance.scrollListener) {
        root.removeEventListener('scroll', instance.scrollListener);
        instance.scrollListener = null;
    }
    instances.delete(root);
}

/**
 * Measures where a fixed overflow menu should sit so it stays inside the viewport.
 *
 * The menu is rendered by Blazor as a position:fixed layer inside the toolbar root (ADR-006). This function
 * does not create, move or remove that node. It only reads getBoundingClientRect and returns viewport
 * coordinates. The overflow popup applies those coordinates through CSSOM, while nested menus use the
 * returned placement to choose a side. Fixed positioning escapes an ancestor with overflow:auto or
 * overflow:hidden. It does not escape an ancestor that creates a containing block (transform, filter,
 * perspective, contain); that limitation is documented in ADR-006.
 *
 * @param {HTMLElement} popup - the popup element.
 * @param {HTMLElement} anchor - the overflow button the popup is anchored to.
 * @returns {{ placement: string, below: boolean, available: number, positioned: boolean, top: number,
 * bottom: number, left: number, right: number, maxBlockSize: number, rtl: boolean }}
 */
export function placePopup(popup, anchor) {
    if (!popup || !anchor || typeof anchor.getBoundingClientRect !== 'function') {
        return { placement: 'bottom', below: true, available: 0, positioned: false, top: 0, bottom: 0, left: 0, right: 0, maxBlockSize: 0, rtl: false };
    }
    const anchorRect = anchor.getBoundingClientRect();
    // A disconnected or not-yet-laid-out anchor has an empty box. Do not pin the menu to the origin.
    if (anchorRect.width < 1 && anchorRect.height < 1) {
        return { placement: 'bottom', below: true, available: 0, positioned: false, top: 0, bottom: 0, left: 0, right: 0, maxBlockSize: 0, rtl: false };
    }
    // The first open can run before the entries have layout, so the box is still 0×0.
    // Read the laid-out height from the entries when the box has not been measured yet.
    let popupRect = popup.getBoundingClientRect();
    const popupHeight = popupRect.height >= 1
        ? popupRect.height
        : Math.max(popup.scrollHeight, popup.offsetHeight, 1);
    const margin = 8;
    const viewportHeight = window.innerHeight || document.documentElement.clientHeight || 0;
    const viewportWidth = window.innerWidth || document.documentElement.clientWidth || 0;
    const below = Math.max(0, viewportHeight - anchorRect.bottom);
    const above = Math.max(0, anchorRect.top);
    // Open downwards when it fits, otherwise upwards. When neither side fits the whole popup the
    // side with more room wins and the popup scrolls inside the height that is left.
    const fitsBelow = below >= popupHeight + margin;
    const fitsAbove = above >= popupHeight + margin;
    const openBelow = fitsBelow || (!fitsAbove && below >= above);
    const available = Math.max(0, (openBelow ? below : above) - margin);
    const rtl = getComputedStyle(anchor).direction === 'rtl';
    return {
        placement: openBelow ? 'bottom' : 'top',
        below: openBelow,
        available: available,
        positioned: true,
        top: anchorRect.bottom,
        bottom: Math.max(0, viewportHeight - anchorRect.top),
        left: anchorRect.left,
        right: Math.max(0, viewportWidth - anchorRect.right),
        maxBlockSize: (!fitsBelow && !fitsAbove) ? available : 0,
        rtl: rtl
    };
}

/**
 * Measures and positions the overflow popup. CSSOM writes avoid Blazor emitting a dynamic style attribute,
 * which is rejected by strict style-src policies. Returns the same placement data for .NET state updates.
 *
 * @param {HTMLElement} popup - the popup element.
 * @param {HTMLElement} anchor - the overflow button the popup is anchored to.
 * @returns {{ placement: string, below: boolean, available: number, positioned: boolean, top: number,
 * bottom: number, left: number, right: number, maxBlockSize: number, rtl: boolean }}
 */
export function placeOverflowPopup(popup, anchor) {
    const placement = placePopup(popup, anchor);
    if (!placement.positioned) {
        return placement;
    }

    const style = popup.style;
    style.setProperty('top', placement.below ? `${placement.top}px` : 'auto');
    style.setProperty('bottom', placement.below ? 'auto' : `${placement.bottom}px`);
    style.setProperty('left', placement.rtl ? `${placement.left}px` : 'auto');
    style.setProperty('right', placement.rtl ? 'auto' : `${placement.right}px`);
    if (placement.maxBlockSize > 0) {
        style.setProperty('max-block-size', `${placement.maxBlockSize}px`);
    } else {
        style.removeProperty('max-block-size');
    }
    return placement;
}

const popupTracks = new WeakMap();

/**
 * Repositions an open overflow menu when the page, an ancestor, or the viewport moves.
 *
 * The listener is passive and coalesced to one animation frame. It never moves a DOM node; it asks
 * .NET to measure again. Safe to call twice for the same anchor.
 *
 * @param {HTMLElement} anchor - the overflow button the menu is anchored to.
 * @param {object} dotNetRef - the .NET reference the callback is invoked on.
 * @returns {void}
 */
export function trackPopup(anchor, dotNetRef) {
    if (!anchor || !dotNetRef || popupTracks.has(anchor)) {
        return;
    }
    let frame = 0;
    const schedule = function () {
        if (frame) {
            return;
        }
        frame = requestAnimationFrame(function () {
            frame = 0;
            dotNetRef.invokeMethodAsync('OnPopupAnchorMovedAsync').catch(function () { /* the circuit may be gone */ });
        });
    };
    const scrollTargets = [];
    for (let el = anchor.parentElement; el; el = el.parentElement) {
        el.addEventListener('scroll', schedule, { passive: true });
        scrollTargets.push(el);
    }
    window.addEventListener('scroll', schedule, { passive: true });
    window.addEventListener('resize', schedule);
    popupTracks.set(anchor, { schedule: schedule, scrollTargets: scrollTargets, frame: function () { return frame; }, clearFrame: function () { frame = 0; } });
}

/** Stops repositioning an overflow menu. Safe to call when tracking was never started. */
export function untrackPopup(anchor) {
    const track = anchor ? popupTracks.get(anchor) : null;
    if (!track) {
        return;
    }
    if (track.frame()) {
        cancelAnimationFrame(track.frame());
        track.clearFrame();
    }
    for (let i = 0; i < track.scrollTargets.length; i++) {
        track.scrollTargets[i].removeEventListener('scroll', track.schedule);
    }
    window.removeEventListener('scroll', track.schedule);
    window.removeEventListener('resize', track.schedule);
    popupTracks.delete(anchor);
}

/**
 * Reads the current position from logical inline start. RTL browsers expose the modern negative
 * scrollLeft range, so convert it to a non-negative logical position before comparing or moving.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @returns {number} The position measured from logical inline start or block start.
 */
function logicalScrollPosition(scroller, vertical) {
    if (vertical) {
        return scroller.scrollTop;
    }
    return getComputedStyle(scroller).direction === 'rtl' ? -scroller.scrollLeft : scroller.scrollLeft;
}

function isFullyVisible(item, scroller, vertical) {
    const itemBounds = item.getBoundingClientRect();
    const viewportBounds = scroller.getBoundingClientRect();
    const tolerance = 1;
    const left = viewportBounds.left + scroller.clientLeft;
    const top = viewportBounds.top + scroller.clientTop;
    const right = left + scroller.clientWidth;
    const bottom = top + scroller.clientHeight;
    return vertical
        ? itemBounds.top >= top - tolerance && itemBounds.bottom <= bottom + tolerance
        : itemBounds.left >= left - tolerance && itemBounds.right <= right + tolerance;
}

function measuredToolbarItems(scroller) {
    return Array.from(scroller.querySelectorAll('[data-toolbar-measure]')).filter(function (item) {
        return item.closest('.e-toolbar-content') === scroller && !item.closest('.e-toolbar-nested-menu');
    });
}

function clampToVisibleBoundary(scroller, start, target, vertical) {
    const distance = target - start;
    if (Math.abs(distance) < 1) {
        return target;
    }
    const state = scrollState(scroller, vertical);
    if ((distance > 0 && state.atEnd) || (distance < 0 && state.atStart)) {
        return start;
    }
    const items = measuredToolbarItems(scroller);
    const item = distance > 0 ? items[items.length - 1] : items[0];
    if (!item) {
        return target;
    }
    const itemBounds = item.getBoundingClientRect();
    const viewportBounds = scroller.getBoundingClientRect();
    const left = viewportBounds.left + scroller.clientLeft;
    const top = viewportBounds.top + scroller.clientTop;
    const right = left + scroller.clientWidth;
    const bottom = top + scroller.clientHeight;
    let boundaryDistance;
    if (vertical) {
        boundaryDistance = distance > 0 ? itemBounds.bottom - bottom : itemBounds.top - top;
    } else if (getComputedStyle(scroller).direction === 'rtl') {
        boundaryDistance = distance > 0 ? left - itemBounds.left : right - itemBounds.right;
    } else {
        boundaryDistance = distance > 0 ? itemBounds.right - right : itemBounds.left - left;
    }
    if (distance > 0) {
        return boundaryDistance <= 1 ? start : Math.min(target, start + boundaryDistance);
    }
    return boundaryDistance >= -1 ? start : Math.max(target, start + boundaryDistance);
}

/**
 * Writes a position measured from logical inline start, preserving the raw scrollTop behavior vertically.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {number} position - the logical position to reach.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @returns {void}
 */
function setLogicalScrollPosition(scroller, position, vertical) {
    if (vertical) {
        scroller.scrollTop = position;
    } else {
        scroller.scrollLeft = getComputedStyle(scroller).direction === 'rtl' ? -position : position;
    }
}

/**
 * Reports how far a scrollable toolbar can move along its main axis, and where it currently is.
 *
 * The navigation chevrons are driven by this: they appear only when the content is wider than the
 * bar, and they are disabled at the start and at the end of the range.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @returns {{ canScroll: boolean, atStart: boolean, atEnd: boolean, scrollSize: number, clientSize: number }}.
 */
export function scrollState(scroller, vertical) {
    if (!scroller) {
        return { canScroll: false, atStart: true, atEnd: true, scrollSize: 0, clientSize: 0 };
    }
    const scrollSize = vertical ? scroller.scrollHeight : scroller.scrollWidth;
    const clientSize = vertical ? scroller.clientHeight : scroller.clientWidth;
    const position = logicalScrollPosition(scroller, vertical);
    const max = scrollSize - clientSize;
    // A sub-pixel remainder is common with fractional device pixel ratios, so a small tolerance
    // keeps the end chevron from staying enabled at the end of the range.
    const tolerance = 1;
    const canScroll = max > tolerance;
    const items = measuredToolbarItems(scroller);
    const firstItem = items.length > 0 ? items[0] : null;
    const lastItem = items.length > 0 ? items[items.length - 1] : null;
    return {
        canScroll: canScroll,
        atStart: firstItem ? isFullyVisible(firstItem, scroller, vertical) : position <= tolerance,
        atEnd: lastItem ? isFullyVisible(lastItem, scroller, vertical) : position >= max - tolerance,
        scrollSize: scrollSize,
        clientSize: clientSize
    };
}

function notifyScrollState(scroller) {
    const instance = instances.get(scroller);
    if (!instance || !instance.dotNetRef) {
        return;
    }
    const state = scrollState(scroller, instance.vertical);
    const previous = instance.lastScrollState;
    if (previous && previous.canScroll === state.canScroll && previous.atStart === state.atStart && previous.atEnd === state.atEnd) {
        return;
    }
    instance.lastScrollState = state;
    instance.dotNetRef.invokeMethodAsync('OnScrolledAsync').catch(function () { /* the circuit may be gone */ });
}

/**
 * Scrolls the toolbar along its main axis.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {number} delta - the distance to move. Negative scrolls towards the start.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @returns {void}
 */
const scrollMotions = new WeakMap();
const ScrollAnimationMs = 1000;

/**
 * Scrolls the toolbar along its main axis.
 *
 * A click uses a 1000ms animation. A repeat step and a reduced-motion request move immediately,
 * and any new request cancels the animation already in flight so the content cannot lag.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {number} delta - the distance to move. Negative scrolls towards the start.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @param {boolean} immediate - true to skip the animation.
 * @returns {void}
 */
export function scrollBy(scroller, delta, vertical, immediate) {
    if (!scroller) {
        return;
    }
    const reduced = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const current = logicalScrollPosition(scroller, vertical);
    moveScroller(scroller, current + delta, vertical, immediate || reduced);
}

/**
 * Scrolls the toolbar by one page towards the start or the end.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {number} direction - -1 scrolls towards the start, 1 scrolls towards the end.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @returns {void}
 */
export function scrollByPage(scroller, direction, vertical) {
    if (!scroller) {
        return;
    }
    const clientSize = vertical ? scroller.clientHeight : scroller.clientWidth;
    // A page is the visible size less a small overlap, so the item that is scrolled to stays
    // partly visible and the user keeps their place.
    const page = Math.max(1, clientSize * 0.8);
    scrollBy(scroller, page * direction, vertical, false);
}

/**
 * Moves a scroller to an absolute position. Immediate movement cancels a running animation.
 * Popup, MultiRow and Extended do not call this: they do not scroll their content.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {number} target - the position to reach.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @param {boolean} immediate - true to assign the position without animating.
 * @returns {void}
 */
function moveScroller(scroller, target, vertical, immediate) {
    const running = scrollMotions.get(scroller);
    if (running) {
        cancelAnimationFrame(running);
        scrollMotions.delete(scroller);
    }
    const max = Math.max(0, (vertical ? scroller.scrollHeight : scroller.scrollWidth) - (vertical ? scroller.clientHeight : scroller.clientWidth));
    const start = logicalScrollPosition(scroller, vertical);
    const clamped = clampToVisibleBoundary(scroller, start, Math.min(max, Math.max(0, target)), vertical);
    if (immediate) {
        setLogicalScrollPosition(scroller, clamped, vertical);
        notifyScrollState(scroller);
        return;
    }
    const distance = clamped - start;
    if (Math.abs(distance) < 1) {
        return;
    }
    const started = performance.now();
    const step = function (now) {
        const progress = Math.min(1, (now - started) / ScrollAnimationMs);
        const eased = 1 - Math.pow(1 - progress, 3);
        const value = start + (distance * eased);
        setLogicalScrollPosition(scroller, value, vertical);
        if (progress < 1) {
            scrollMotions.set(scroller, requestAnimationFrame(step));
        } else {
            scrollMotions.delete(scroller);
            notifyScrollState(scroller);
        }
    };
    scrollMotions.set(scroller, requestAnimationFrame(step));
}

/**
 * Spreads a short color wash from the pointer. Keyboard and non-pointer activations do not call this.
 *
 * @param {HTMLElement} button - the toolbar button that was pressed.
 * @param {number} clientX - the horizontal pointer coordinate relative to the viewport.
 * @param {number} clientY - the vertical pointer coordinate relative to the viewport.
 * @returns {void}
 */
export function playRipple(item, clientX, clientY) {
    if (!item || (window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches)) {
        return;
    }
    const button = item.matches('.e-tbar-btn') ? item : item.querySelector('.e-tbar-btn') || item;
    const bounds = button.getBoundingClientRect();
    const wash = document.createElement('span');
    wash.className = 'e-toolbar-ripple';
    wash.style.left = (clientX - bounds.left) + 'px';
    wash.style.top = (clientY - bounds.top) + 'px';
    button.appendChild(wash);
    window.setTimeout(function () {
        wash.remove();
    }, 1000);
}

const nestedMenuTracks = new WeakMap();

/**
 * Closes an open nested menu when a click occurs outside its menu and trigger.
 *
 * Clicks on any nested-menu trigger are left to the component so it can toggle
 * the current menu or switch to another one without racing a document handler.
 *
 * @param {HTMLElement} root - the toolbar root.
 * @param {object} dotNetRef - the .NET reference the callback is invoked on.
 * @returns {void}
 */
export function trackNestedMenus(root, dotNetRef) {
    if (!root || !dotNetRef || nestedMenuTracks.has(root)) {
        return;
    }
    const listener = function (event) {
        const target = event.target;
        if (target instanceof Element && root.contains(target)) {
            if (target.closest('[data-testid="toolbar-nested-popup"]')
                || target.closest('[data-testid="toolbar-dropdown"], [data-testid="toolbar-menubutton"], [data-testid="toolbar-split-toggle"]')) {
                return;
            }
        }
        dotNetRef.invokeMethodAsync('OnNestedMenuOutsideClickAsync').catch(function () { /* the circuit may be gone */ });
    };
    document.addEventListener('click', listener);
    nestedMenuTracks.set(root, listener);
}

/**
 * Prevents native Enter and Space activation from producing a second click.
 *
 * @param {HTMLElement} root - the toolbar root.
 * @returns {void}
 */
export function trackOverflowKeys(root) {
    if (!root || overflowKeyTracks.has(root)) {
        return;
    }
    const listener = function (event) {
        if (event.key !== 'Enter' && event.key !== ' ') {
            return;
        }
        const target = event.target;
        if (!(target instanceof Element) || !root.contains(target)) {
            return;
        }
        if (target.closest('[data-testid="toolbar-overflow-button"]')) {
            event.preventDefault();
        } else if (target.closest('[data-testid="toolbar-overflow-item"]')) {
            event.preventDefault();
        }
    };
    root.addEventListener('keydown', listener);
    overflowKeyTracks.set(root, listener);
}

/** Stops delivering overflow keyboard events. Safe to call when tracking was never started. */
export function untrackOverflowKeys(root) {
    const listener = root ? overflowKeyTracks.get(root) : null;
    if (!listener) {
        return;
    }
    root.removeEventListener('keydown', listener);
    overflowKeyTracks.delete(root);
}

/** Stops listening for clicks outside nested menus. Safe to call when tracking was never started. */
export function untrackNestedMenus(root) {
    const listener = root ? nestedMenuTracks.get(root) : null;
    if (!listener) {
        return;
    }
    document.removeEventListener('click', listener);
    nestedMenuTracks.delete(root);
}
