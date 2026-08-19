'use strict';

/**
 * Blazor textbox interop handler
 *
 * The SfTextBox uses a very small JS surface:
 *
 *   - `initialize(element, dotnetRef, containerElement)` is exposed
 *     for symmetry with the other components but is not currently
 *     invoked from C#.
 *   - `calculateWidth(element, dotnetRef, containerElement)`
 *     repositions the floating label and is called on every render.
 *     It caches the SfTextBox instance on the underlying input
 *     element via the `blazor_input_instance` back-pointer.
 *   - `focusOut(element)` blurs the input.
 *   - `destroy(element)` clears the back-pointer and is called from
 *     the C# `Destroyed` event so the JS state is released together
 *     with the Blazor component.
 */
var SfTextBox = /** @class */ (function () {
    function SfTextBox(element, dotnetRef, containerElement) {
        this.element = element;
        this.container = containerElement;
        this.dotNetRef = dotnetRef;
    }
    SfTextBox.prototype.calculateWidth = function () {
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
    SfTextBox.prototype.dispose = function () {
        // Drop the DOM-element back-reference so it can be
        // garbage-collected even if the JS module outlives the
        // Blazor component.
        if (this.element) {
            this.element.blazor_input_instance = null;
        }
        this.dotNetRef = null;
        this.container = null;
        this.element = null;
    };
    return SfTextBox;
}());

/**
 * Initializes a SfTextBox JS instance.
 *
 * Kept for symmetry with the other components; the current SfTextBox
 * C# lifecycle invokes `calculateWidth` directly without an
 * `initialize` call, so this entry-point is a defensive convenience.
 */
export function initialize(element, dotnetRef, containerElement) {
    if (!element) {
        return;
    }
    var instance = new SfTextBox(element, dotnetRef, containerElement);
    element.blazor_input_instance = instance;
    if (!window.sfBlazorToolkit || !window.sfBlazorToolkit.base) {
        return;
    }
    var fieldset = window.sfBlazorToolkit.base.closest(element, 'fieldset');
    if (!window.sfBlazorToolkit.base.isNullOrUndefined(fieldset) && fieldset.disabled) {
        if (instance.dotNetRef && typeof instance.dotNetRef.invokeMethodAsync === 'function') {
            instance.dotNetRef.invokeMethodAsync('UpdateFieldSetStatus', true);
        }
    }
}

/**
 * Recomputes the floating-label position.
 *
 * The instance is created on demand and cached via
 * `element.blazor_input_instance`. The C# `Destroyed` event then
 * forwards to `destroy(element)`, which disposes the cached
 * instance.
 */
export function calculateWidth(element, dotnetRef, containerElement) {
    if (!element) {
        return;
    }
    var instance = element.blazor_input_instance;
    if (!instance) {
        instance = new SfTextBox(element, dotnetRef, containerElement);
        element.blazor_input_instance = instance;
    }
    instance.calculateWidth();
}

/**
 * Programmatically blurs the underlying input.
 */
export function focusOut(element) {
    if (element) {
        element.blur();
    }
}

/**
 * Tears down the JS-side state for a SfTextBox.
 *
 * Disposes the cached instance and drops the DOM back-reference so
 * the input element can be garbage-collected.
 *
 * @param {HTMLElement} element - The underlying <input> element.
 */
export function destroy(element) {
    if (!element) {
        return;
    }
    var instance = element.blazor_input_instance;
    if (instance && typeof instance.dispose === 'function') {
        instance.dispose();
    }
    else {
        element.blazor_input_instance = null;
    }
}
