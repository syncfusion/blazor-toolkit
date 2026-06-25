'use strict';

/**
    * Blazor texbox interop handler
    */
var SfTextArea = /** @class */ (function () {
    function SfTextArea(dataId, element, containerEle) {
        this.dataId = dataId;
        this.element = element;
        this.container = containerEle;
        window.sfBlazorToolkit.base.setCompInstance(this);
    }
    SfTextArea.prototype.calculateWidth = function () {
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
    return SfTextArea;
}());
export function initialize(dataId, element, containerEle) {
    if (element) {
        new SfTextArea(dataId, element, containerEle);
    }
}
export function calculateWidth(dataId, element) {
    if (element) {
        var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
        if (instance) {
            instance.calculateWidth();
        }
    }
}
export function focusOut(element) {
    element.blur();
}
