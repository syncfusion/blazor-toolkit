'use strict';

/**
 * Blazor textarea interop handler
 *
 * The SfTextArea JS surface mirrors SfTextBox:
 *
 *   - `initialize(dataId, element, containerEle)` registers the
 *     instance in the base registry AND caches a back-pointer on
 *     the DOM element.
 *   - `calculateWidth(dataId, element)` recomputes the floating-label
 *     position. It looks up the instance by dataId from the
 *     registry; if missing, falls back to creating one on demand
 *     (and caches it).
 *   - `focusOut(element)` blurs the element.
 *   - `destroy(dataId)` drops the instance from the registry and
 *     detaches the DOM back-reference.
 */
var SfTextArea = /** @class */ (function () {
    function SfTextArea(dataId, element, containerEle) {
        this.dataId = dataId;
        this.element = element;
        this.container = containerEle;
        if (dataId && window.sfBlazorToolkit && window.sfBlazorToolkit.base && window.sfBlazorToolkit.base.setCompInstance) {
            window.sfBlazorToolkit.base.setCompInstance(this);
        }
    }
    SfTextArea.prototype.calculateWidth = function () {
        if (!this.container) {
            return;
        }
        var label = this.container.querySelector('.e-float-text');
        if (!label) {
            return;
        }

        if (this.container.classList.contains('e-outline') && this.container.classList.contains('e-prepend') &&
            label.classList.contains('e-label-top')) {
            var left = this.container.clientWidth - this.element.clientWidth;
            label.style.left = -left.toString() + 'px';
            label.style.width = 'auto';
        }
        else {
            label.style.left = '0px';
        }
    };
    SfTextArea.prototype.dispose = function () {
        this.container = null;
        this.element = null;
    };
    return SfTextArea;
}());

/**
 * Initializes a SfTextArea JS instance and registers it in the
 * base instance registry under the supplied dataId.
 */
export function initialize(dataId, element, containerEle) {
    if (!element || !dataId) {
        return;
    }
    new SfTextArea(dataId, element, containerEle);
}

/**
 * Recomputes the floating-label position for the registered
 * instance. Falls back to creating a transient instance when none
 * is registered.
 */
export function calculateWidth(dataId, element) {
    if (!element) {
        return;
    }
    var instance = null;
    if (dataId && window.sfBlazorToolkit && window.sfBlazorToolkit.base) {
        instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    }
    if (!instance) {
        instance = new SfTextArea(dataId || '', element, null);
    }
    instance.calculateWidth();
}

/**
 * Programmatically blurs the underlying textarea element.
 */
export function focusOut(element) {
    if (element) {
        element.blur();
    }
}

/**
 * Tears down the JS-side state for a SfTextArea.
 *
 * Removes the instance from the base registry and drops any DOM
 * back-references the instance was holding.
 *
 * @param {string} dataId - The dataId used at initialize() time.
 */
export function destroy(dataId) {
    if (!dataId || !window.sfBlazorToolkit || !window.sfBlazorToolkit.base) {
        return;
    }
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (instance && typeof instance.dispose === 'function') {
        instance.dispose();
    }
    if (typeof window.sfBlazorToolkit.base.disposeWindowsInstance === 'function') {
        window.sfBlazorToolkit.base.disposeWindowsInstance(dataId);
    }
    else if (typeof window.sfBlazorToolkit.base.removeCompInstance === 'function') {
        window.sfBlazorToolkit.base.removeCompInstance(dataId);
    }
}
