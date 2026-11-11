"use strict";

/**
 * Watches a CSS media query and reports match changes to a .NET component.
 * @param {string} query A media query such as "(max-width: 768px)".
 * @param {import('./base.js').DotNetObject} dotnetRef A component reference that exposes OnMediaQueryChanged.
 * @returns {{ dispose: () => void }} A handle that removes the listener.
 */
export function watchMedia(query, dotnetRef) {
    const media = window.matchMedia(query);
    const notify = (event) => {
        dotnetRef.invokeMethodAsync("OnMediaQueryChanged", event.matches);
    };

    if (typeof media.addEventListener === "function") {
        media.addEventListener("change", notify);
    }
    else {
        media.addListener(notify);
    }

    notify(media);
    return {
        dispose() {
            if (typeof media.removeEventListener === "function") {
                media.removeEventListener("change", notify);
            }
            else {
                media.removeListener(notify);
            }
        }
    };
}

const FOCUSABLE = [
    "a[href]",
    "area[href]",
    "button:not([disabled])",
    "input:not([disabled]):not([type='hidden'])",
    "select:not([disabled])",
    "textarea:not([disabled])",
    "iframe",
    "[contenteditable]:not([contenteditable='false'])",
    "[tabindex]:not([tabindex='-1'])"
].join(",");

const observers = new Map();
const modals = new Map();

function isVisible(element) {
    return element.getClientRects().length > 0 && getComputedStyle(element).visibility !== "hidden";
}

function getFocusable(panel) {
    return Array.from(panel.querySelectorAll(FOCUSABLE)).filter(isVisible);
}

function isBelow(threshold) {
    return threshold > 0 && window.innerWidth < threshold;
}

function createWidthQuery(threshold) {
    const range = window.matchMedia(`(width < ${threshold}px)`);
    return range.media === "not all" ? window.matchMedia(`(max-width: ${threshold - 0.02}px)`) : range;
}

/**
 * Registers the viewport, motion and direction listeners of one sidebar and returns the initial environment.
 * @param {string} key A stable key of the sidebar instance.
 * @param {{ invokeMethodAsync: (name: string, ...args: unknown[]) => Promise<unknown> }} callbacks The .NET callbacks.
 * @param {HTMLElement} host An element inside the sidebar layout host. The direction is read from the host's parent.
 * @param {number} threshold The viewport width below which the viewport counts as mobile. 0 disables it.
 * @returns {{ isMobile: boolean, reducedMotion: boolean, isRtl: boolean }} The environment at registration time.
 */
export function observe(key, callbacks, host, threshold) {
    unobserve(key);
    const state = { threshold, isMobile: isBelow(threshold), widthQuery: null, viewportTimer: null };
    const motionQuery = window.matchMedia("(prefers-reduced-motion: reduce)");
    // Read the direction from the element that contains the layout host. The panel itself is skipped because the e-rtl
    // class this component adds forces direction: rtl there, which would make the reading a self-fulfilling loop.
    const directionSource = host.closest(".e-sidebar-container")?.parentElement ?? document.documentElement;
    const readRtl = () => getComputedStyle(directionSource).direction === "rtl";
    // Watch the layout host's direction source instead of unrelated document direction changes.
    let environment = { reducedMotion: motionQuery.matches, isRtl: readRtl() };

    const send = (name, ...args) => {
        callbacks.invokeMethodAsync(name, ...args).catch(() => { });
    };
    const checkViewport = () => {
        const isMobile = isBelow(state.threshold);
        if (isMobile === state.isMobile) {
            return;
        }

        state.isMobile = isMobile;
        clearTimeout(state.viewportTimer);
        state.viewportTimer = setTimeout(() => {
            state.viewportTimer = null;
            send("OnViewportChanged", state.isMobile);
        }, 0);
    };
    const checkEnvironment = () => {
        const next = { reducedMotion: motionQuery.matches, isRtl: readRtl() };
        if (next.reducedMotion !== environment.reducedMotion || next.isRtl !== environment.isRtl) {
            environment = next;
            send("OnEnvironmentChanged", next.reducedMotion, next.isRtl);
        }
    };
    const bindWidthQuery = () => {
        if (state.widthQuery) {
            state.widthQuery.removeEventListener("change", checkViewport);
            state.widthQuery = null;
        }
        if (state.threshold > 0) {
            state.widthQuery = createWidthQuery(state.threshold);
            state.widthQuery.addEventListener("change", checkViewport);
        }
    };

    bindWidthQuery();
    if (threshold > 0) {
        window.addEventListener("resize", checkViewport);
    }
    motionQuery.addEventListener("change", checkEnvironment);
    const directionObserver = new MutationObserver(checkEnvironment);
    directionObserver.observe(directionSource, { attributes: true, attributeFilter: ["dir"] });

    observers.set(key, {
        update(next) {
            const wasWatching = state.threshold > 0;
            state.threshold = next;
            state.isMobile = isBelow(next);
            bindWidthQuery();
            if (!wasWatching && next > 0) {
                window.addEventListener("resize", checkViewport);
            }
            else if (wasWatching && next <= 0) {
                window.removeEventListener("resize", checkViewport);
            }
            return state.isMobile;
        },
        dispose() {
            if (state.viewportTimer !== null) {
                clearTimeout(state.viewportTimer);
                state.viewportTimer = null;
            }
            window.removeEventListener("resize", checkViewport);
            motionQuery.removeEventListener("change", checkEnvironment);
            directionObserver.disconnect();
            if (state.widthQuery) {
                state.widthQuery.removeEventListener("change", checkViewport);
                state.widthQuery = null;
            }
        }
    });

    return { isMobile: state.isMobile, reducedMotion: environment.reducedMotion, isRtl: environment.isRtl };
}

/**
 * Applies a new threshold to a registered sidebar and returns whether the viewport is mobile.
 * @param {string} key The instance key passed to observe.
 * @param {number} threshold The new threshold. 0 disables mobile detection.
 * @returns {boolean} True when the viewport is below the threshold.
 */
export function update(key, threshold) {
    const observer = observers.get(key);
    return observer ? observer.update(threshold) : false;
}

/**
 * Removes every listener registered for a sidebar.
 * @param {string} key The instance key passed to observe.
 */
export function unobserve(key) {
    const observer = observers.get(key);
    if (observer) {
        observer.dispose();
        observers.delete(key);
    }
}

/**
 * Moves focus to the first focusable descendant of the panel, or to the panel itself when there is none.
 * @param {HTMLElement} panel The sidebar panel.
 */
export function focusPanel(panel) {
    const first = getFocusable(panel)[0];
    (first ?? panel).focus({ preventScroll: true });
}

/**
 * Starts the modal focus behavior: remembers the opener, moves focus in, and keeps Tab inside the panel.
 * @param {string} key The instance key.
 * @param {HTMLElement} panel The sidebar panel.
 */
export function activateModal(key, panel) {
    deactivateModal(key, false);
    const active = document.activeElement;
    const opener = active && active !== document.body && !panel.contains(active) ? active : null;
    const onKeyDown = (event) => {
        if (event.key !== "Tab") {
            return;
        }
        const items = getFocusable(panel);
        if (items.length === 0) {
            event.preventDefault();
            panel.focus({ preventScroll: true });
            return;
        }
        const first = items[0];
        const last = items[items.length - 1];
        const current = document.activeElement;
        const inside = panel.contains(current);
        if (!inside) {
            event.preventDefault();
            (event.shiftKey ? last : first).focus({ preventScroll: true });
        }
        else if (event.shiftKey && (current === first || current === panel)) {
            event.preventDefault();
            last.focus({ preventScroll: true });
        }
        else if (!event.shiftKey && current === last) {
            event.preventDefault();
            first.focus({ preventScroll: true });
        }
    };
    document.addEventListener("keydown", onKeyDown, true);
    modals.set(key, { opener, onKeyDown });
    focusPanel(panel);
}

/**
 * Ends the modal focus behavior and optionally returns focus to the opener.
 * @param {string} key The instance key.
 * @param {boolean} restore True to return focus to the opener when it is still connected.
 */
export function deactivateModal(key, restore) {
    const modal = modals.get(key);
    if (!modal) {
        return;
    }
    document.removeEventListener("keydown", modal.onKeyDown, true);
    modals.delete(key);
    if (restore && modal.opener && modal.opener.isConnected) {
        modal.opener.focus({ preventScroll: true });
    }
}
