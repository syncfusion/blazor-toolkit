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
        var label = document.getElementsByClassName('e-float-text');
        if (!label || label.length === 0) return;
        var isOutline = this.container.classList.contains('e-outline');
        var isPrepend = this.container.classList.contains('e-prepend');
        for (var i = 0; i < label.length; i++) {
            if (isOutline && isPrepend && label[i].classList.contains('e-label-top')) {
                var left = this.container.clientWidth - this.element.clientWidth;
                label[i].style.left = -left.toString() + 'px';
                label[i].style.width = 'auto';
            }
            else {
                label[i].style.left = '0px';
            }
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
