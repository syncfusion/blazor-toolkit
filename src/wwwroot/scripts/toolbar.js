/* eslint-disable no-undef */
// ES module for SfToolbar overflow measurement and popup placement - named exports only (no globals).
// Browser APIs used: ResizeObserver and getBoundingClientRect. Nothing else is measured, and no focus library is used.

const instances = new WeakMap();

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
    return {
        available: availableOf(toolbar || root, vertical),
        gap: gapOf(root, vertical),
        buttonSize: button ? sizeOf(button, vertical) : 0,
        items: items.map(function (element) {
            return { id: element.getAttribute('data-toolbar-measure'), size: sizeOf(element, vertical) };
        })
    };
}

/**
 * Starts observing the toolbar root. The .NET handler is called, debounced to one animation frame, whenever the
 * container size changes. The container is observed, not the viewport (TB-OVF-080).
 */
export function observe(root, dotNetRef) {
    if (!root || !dotNetRef || instances.has(root)) {
        return;
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
    instances.set(root, {
        observer: observer,
        scrollListener: null,
        cancel: function () {
            if (frame) {
                cancelAnimationFrame(frame);
                frame = 0;
            }
        }
    });
}

/**
 * Reports the scroll position of a scrollable toolbar whenever it changes, so the navigation chevrons
 * can be enabled and disabled at the start and the end of the range.
 *
 * The listener is passive, so it never blocks the touch swipe or the wheel that moved the toolbar.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {object} dotNetRef - the .NET reference the callback is invoked on.
 * @returns {void}
 */
export function observeScroll(scroller, dotNetRef) {
    if (!scroller || !dotNetRef) {
        return;
    }
    const instance = instances.get(scroller);
    if (!instance || instance.scrollListener) {
        return;
    }
    let frame = 0;
    const listener = function () {
        if (frame) {
            return;
        }
        frame = requestAnimationFrame(function () {
            frame = 0;
            dotNetRef.invokeMethodAsync('OnScrolledAsync').catch(function () { /* the circuit may be gone */ });
        });
    };
    scroller.addEventListener('scroll', listener, { passive: true });
    instance.scrollListener = listener;
}

/** Stops observing and releases the observer. Safe to call twice. */
export function unobserve(root) {
    const instance = root ? instances.get(root) : null;
    if (!instance) {
        return;
    }
    instance.cancel();
    instance.observer.disconnect();
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
    const position = vertical ? scroller.scrollTop : scroller.scrollLeft;
    const max = scrollSize - clientSize;
    // A sub-pixel remainder is common with fractional device pixel ratios, so a small tolerance
    // keeps the end chevron from staying enabled at the end of the range.
    const tolerance = 1;
    const canScroll = max > tolerance;
    return {
        canScroll: canScroll,
        atStart: position <= tolerance,
        atEnd: position >= max - tolerance,
        scrollSize: scrollSize,
        clientSize: clientSize
    };
}

/**
 * Scrolls the toolbar along its main axis.
 *
 * @param {HTMLElement} scroller - the element that scrolls.
 * @param {number} delta - the distance to move. Negative scrolls towards the start.
 * @param {boolean} vertical - true when the toolbar scrolls along the block axis.
 * @returns {void}
 */
export function scrollBy(scroller, delta, vertical) {
    if (!scroller) {
        return;
    }
    if (vertical) {
        scroller.scrollTop += delta;
    } else {
        scroller.scrollLeft += delta;
    }
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
    scrollBy(scroller, page * direction, vertical);
}
