'use strict';

/**
 * Blazor textbox interop handler
 */
var SfTextBox = /** @class */ (function () {
function SfTextBox(element, dotnetRef, containerElement) {
    this.element = element;
    this.container = containerElement;
    this.element.blazor_input_instance = this;
    this.dotNetRef = dotnetRef;
}
SfTextBox.prototype.calculateWidth = function () {
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
return SfTextBox;
}());

export function initialize(element, dotnetRef, containerElement) {
    if (element) {
        var instance = new SfTextBox(element, dotnetRef, containerElement);
        if (element && element.blazor_input_instance) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(sfBlazorToolkit.base.closest(element, 'fieldset')) && sfBlazorToolkit.base.closest(element, 'fieldset').disabled) {
                var disabled = true;
                instance.dotNetRef.invokeMethodAsync('UpdateFieldSetStatus', disabled);
            }
        }
    }
}
export function calculateWidth(element, dotnetRef, containerElement) {
    if (element) {
        new SfTextBox(element, dotnetRef, containerElement, false);
    }
    if (element && element.blazor_input_instance) {
        element.blazor_input_instance.calculateWidth();
    }
}
export function focusOut(element) {
    element.blur();
}
