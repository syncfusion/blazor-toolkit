'use strict';

// Lightweight Touch helper compatible with existing callers.
// Supports options: tap, tapHold, swipe, scroll, tapHoldThreshold
class Touch {
    constructor(element, options) {
        this.element = element;
        this.options = options || {};
        this._start = this._start.bind(this);
        this._move = this._move.bind(this);
        this._end = this._end.bind(this);
        this._tapHoldTimer = null;
        this._startPoint = null;
        this._isMoved = false;
        sfBlazorToolkit.base.EventHandler.add(this.element, sfBlazorToolkit.base.Browser.touchStartEvent, this._start, this);
    }

    _getPoint(evt) {
        return (evt.changedTouches && evt.changedTouches.length) ? evt.changedTouches[0] : evt;
    }

    _start(evt) {
        this._isMoved = false;
        this._startPoint = this._getPoint(evt);
        this._timeStart = Date.now();
        if (this.options.tapHold) {
            var thr = this.options.tapHoldThreshold || 750;
            this._tapHoldTimer = setTimeout(() => {
                this.options.tapHold && this.options.tapHold({ originalEvent: evt });
            }, thr);
        }
        sfBlazorToolkit.base.EventHandler.add(this.element, sfBlazorToolkit.base.Browser.touchMoveEvent, this._move, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, sfBlazorToolkit.base.Browser.touchEndEvent, this._end, this);
    }

    _move(evt) {
        var point = this._getPoint(evt);
        var dx = point.clientX - this._startPoint.clientX;
        var dy = point.clientY - this._startPoint.clientY;
        if (Math.abs(dx) > 2 || Math.abs(dy) > 2) {
            this._isMoved = true;
            clearTimeout(this._tapHoldTimer);
        }
        if (this.options && this.options.scroll) {
            this.options.scroll({ originalEvent: evt, distanceX: dx, distanceY: dy, startEvents: this._startPoint, scrollDirection: Math.abs(dx) > Math.abs(dy) ? (dx < 0 ? 'Left' : 'Right') : (dy < 0 ? 'Up' : 'Down'), velocity: 0 });
        }
    }

    _end(evt) {
        clearTimeout(this._tapHoldTimer);
        var point = this._getPoint(evt);
        var dx = point.clientX - this._startPoint.clientX;
        var dy = point.clientY - this._startPoint.clientY;
        var duration = Date.now() - this._timeStart;
        if (!this._isMoved) {
            if (this.options.tap) {
                this.options.tap({ originalEvent: evt, tapCount: 1 });
            }
        }
        else if (this.options.swipe) {
            var dir = Math.abs(dx) > Math.abs(dy) ? (dx < 0 ? 'Left' : 'Right') : (dy < 0 ? 'Up' : 'Down');
            var velocity = Math.sqrt(dx * dx + dy * dy) / (duration || 1);
            this.options.swipe({ originalEvent: evt, startEvents: this._startPoint, distanceX: Math.abs(dx), distanceY: Math.abs(dy), swipeDirection: dir, velocity: velocity });
        }
        sfBlazorToolkit.base.EventHandler.remove(this.element, sfBlazorToolkit.base.Browser.touchMoveEvent, this._move);
        sfBlazorToolkit.base.EventHandler.remove(this.element, sfBlazorToolkit.base.Browser.touchEndEvent, this._end);
    }

    destroy() {
        clearTimeout(this._tapHoldTimer);
        sfBlazorToolkit.base.EventHandler.remove(this.element, sfBlazorToolkit.base.Browser.touchStartEvent, this._start);
        sfBlazorToolkit.base.EventHandler.remove(this.element, sfBlazorToolkit.base.Browser.touchMoveEvent, this._move);
        sfBlazorToolkit.base.EventHandler.remove(this.element, sfBlazorToolkit.base.Browser.touchEndEvent, this._end);
        this.element = null;
        this.options = null;
    }
}

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.Touch = Touch;
if (window.sfBlazorToolkit.base) {
    window.sfBlazorToolkit.base.Touch = Touch;
}