'use strict';

const SfChart = (function () {
    function SfChart(dataId, id, element, dotnetRef, isZooming = false, isScrollbar = false, options, selectionHighlightOptions, isStripLineTooltip, isMouseMoveEvent) {
        this.sfBlazor = window.sfBlazor;
        this.mouseY = 0;
        this.mouseX = 0;
        this.isTouch = false;
        this.eventInterval = 80;
        this.mouseMoveRef = null;
        this.mouseMoveThreshold = null;
        this.chartOnMouseClickRef = null;
        this.chartRightClickRef = null;
        this.mouseLeaveRef = null;
        this.chartKeyDownRef = null;
        this.chartKeyUpRef = null;
        this.chartMouseWheelRef = null;
        this.chartOnMouseDownRef = null;
        this.mouseEndRef = null;
        this.domMouseMoveRef = null;
        this.domMouseUpRef = null;
        this.longPressBound = null;
        this.touchObject = null;
        //values to be passed
        this.pinchZoomingEnable = false;
        this.crosshairEnable = false;
        this.toggleVisibility = true;
        this.enableHighlight = true;
        this.highlightMode = 'None';
        this.selectionMode = 'None';
        this.seriesTypes = [];
        this.redraw = false;
        this.selectedDataIndexes = [];
        this.highlightDataIndexes = [];
        this.previousSelectedEle = [];
        this.highlightColor = '';
        this.highlightPattern = 'None';
        this.allowMultiSelection = false;
        this.previousHighlightedIndex = -1;
        this.tooltipBase = new ITooltipBase();
        this.striplineTooltipBase = new ITooltipBase();
        this.markerExplodeBase = new IMarkerExplodeBase();
        this.crosshairBase = new ICrosshairBase();
        this.userInteractionBase = new IUserInteractionBase();
        this.zoomBase = new IZoomBase();
        this.zoomToolkitBase = new IZoomToolkitBase();
        this.isChartPanning = false;
        this.axes = [];
        this.scrollDownRef = null;
        this.wheelEventEndTimeout = null;
        this.isWheelScrolling = false;
        this.crosshairLocationX = 0;
        this.crosshairLocationY = 0;
        this.isHighlightRemoved = false;
        this.isChartKeyDown = false;
        this.isDisposed = false;
        this.disabledLegendIndexes = [];
        this.currentToggledBackLegendIndex = -1;
        this.isTouchLegendClick = false;
        this.isLegendHighlighting = false;
        this.prevHighlightedSeriesIndex = -1;
        this.isDoubleTap = false;
        this.scaleX = 0;
        this.scaleY = 0;
        this.scrollBarsId = [];
        this.pinchStyle = 'opacity: 0; position: absolute; display: block; width: 100px; height: 100px; background: transparent; border: 2px solid blue;';
        this.pinchtarget = null;
        this.id = id;
        this.element = element;
        this.dotnetref = dotnetRef;
        this.isZooming = isZooming;
        this.isStripLineTooltip = isStripLineTooltip;
        this.isMouseMoveEvent = isMouseMoveEvent;
        this.isScrollbar = isScrollbar;
        this.options = options;
        this.currentLegendIndex = 0;
        this.currentPointIndex = 0;
        this.currentSeriesIndex = 0;
        this.currentAnnotationIndex = 0;
        this.seriesPathElement = null;
        this.previousTargetId = '';
        this.isZoomed = false;
        this.selectionHighlightOptions(selectionHighlightOptions);
        this.selectedDataIndexes = selectionHighlightOptions.selectedDataIndexes || [];
        this.unSelected = id + '_ej2_deselected';
        this.dataId = dataId;
        window.sfBlazorToolkit.base.setCompInstance(this);
        this.getTooltipData();
        this.userInteractionBase.svgRenderer = new svgbase.SvgRenderer(this.element.id);
        this.removeTooltipData();
    }
    SfChart.prototype.render = function () {
        this.unWireEvents(this.id, this.dotnetref);
        this.wireEvents(this.id, this.dotnetref);
    };
    SfChart.prototype.destroy = function () {
        this.isDisposed = true;
        this.unWireEvents(this.id, this.dotnetref);
        this.dotnetref.invokeMethodAsync('DisposeDotNetReference');
    };
    SfChart.prototype.selectionHighlightOptions = function (options) {
        this.enableHighlight = options.enableHighlight;
        this.pinchZoomingEnable = options.pinchZoomingEnable;
        this.toggleVisibility = options.toggleVisibility;
        this.highlightMode = options.highlightMode;
        this.selectionMode = options.selectionMode;
        this.seriesTypes = options.seriesTypes;
        this.highlightColor = options.highlightColor;
        this.highlightPattern = options.highlightPattern;
        this.allowMultiSelection = options.allowMultiSelection;
        this.oldMode = this.currentMode;
    };
    SfChart.prototype.getTooltipData = function (clipRects, seriesMarkers, seriesBorders, axes) {
        const element = document.getElementById(this.element.id + '_tooltip_data');
        if (element) {
            this.userInteractionBase.chartData = element.getAttributeNames().map((name) => element.getAttribute(name));
            this.userInteractionBase.chartData.shift();
            const index = this.userInteractionBase.chartData.indexOf('display: block;');
            if (index > -1) {
                this.userInteractionBase.chartData.splice(index, 1);
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.userInteractionBase.visibleSeries)) {
                this.userInteractionBase.visibleSeries.forEach((series) => {
                    const seriesElement = document.getElementById(series.id);
                    series.visible = !sfBlazorToolkit.base.isNullOrUndefined(seriesElement) ? !sfBlazorToolkit.base.isNullOrUndefined(seriesElement.getAttribute('data-point')) && seriesElement.getAttribute('data-point').split(/(?![^(]*\)),/)[8] === 'True' : false;
                });
            }
            if (this.userInteractionBase.chartData.length > 0 && clipRects && seriesMarkers && seriesBorders && axes) {
                this.getVisibleSeries(this, clipRects, seriesMarkers, seriesBorders, axes);
                this.isDataLoaded = true;
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(this.tooltipBase.tooltipElementSize) && this.tooltipBase.tooltipModule && this.tooltipBase.tooltipModule.template) {
                this.getElementSize(this.element.id + '_tooltip');
            }
        }
    };
    SfChart.prototype.removeTooltipData = function () {
        const element = document.getElementById(this.element.id + '_tooltip_data');
        if (element && element.getAttributeNames().length > 1 && this.dotnetref && !this.isDisposed) {
            this.dotnetref.invokeMethodAsync('RemoveTooltipData');
        }
    };
    SfChart.prototype.tooltipOptions = function (tooltip, options) {
        this.userInteractionBase.availableSize = options.availableSize;
        this.userInteractionBase.chartBorderWidth = options.borderWidth;
        this.userInteractionBase.disableTrackTooltip = options.disableTrackTooltip;
        this.userInteractionBase.axisClipRect = options.axisClipRect;
        this.userInteractionBase.isPointMouseDown = options.isPointMouseDown;
        this.userInteractionBase.isPointDragging = options.isPointDragging;
        this.userInteractionBase.isInverted = options.isInverted;
        this.userInteractionBase.chartAreaType = options.chartAreaType;
        this.tooltipBase.isTooltipMarker = options.isMarkerEnable;
        this.tooltipBase.tooltipModule = tooltip;
        this.tooltipBase.tooltipFormat = !sfBlazorToolkit.base.isNullOrUndefined(options.tooltipFormat) ? this.getTooltipFormat(options.tooltipFormat) : options.tooltipFormat;
        this.userInteractionBase.enableRTL = options.enableRTL;
        this.tooltipBase.enableHighlight = options.enableHighlight;
        this.crosshairBase.crosshair = options.crosshair;
        this.crosshairEnable = options.crosshair != null && options.crosshair.enable;
        this.markerExplodeBase.markerExploded = options.markerExplode;
        this.userInteractionBase.chartRadius = options.chartRadius;
        this.userInteractionBase.theme = options.theme;
        this.crosshairBase.themeStyleCrosshairLine = options.themeStyleCrosshairLine;
        this.crosshairBase.themeStyleCrosshairBackground = options.themeStyleCrosshairBackground;
        this.crosshairBase.themeStyleCrosshairFill = options.themeStyleCrosshairFill;
        this.crosshairBase.themeStyleCrosshairLabel = options.themeStyleCrosshairLabel;
        this.crosshairBase.themeStyleCrosshairTextSize = options.themeStyleCrosshairTextSize;
        this.crosshairBase.themeStyleCrosshairFontFamily = options.themeStyleCrosshairFontFamily;
        this.crosshairBase.themeStyleCrosshairFontWeight = options.themeStyleCrosshairFontWeight;
        this.tooltipBase.tooltipModule.template = options.templateString ? options.templateString : this.tooltipBase.tooltipModule.template;
        this.userInteractionBase.initialRect = options.initialRect;
        this.userInteractionBase.secondaryElementOffset = options.secondaryElementOffset;
        if (sfBlazorToolkit.base.isNullOrUndefined(this.tooltipBase.tooltipElementSize) && this.tooltipBase.tooltipModule.template) {
            this.getElementSize(this.element.id + '_tooltip');
        }
        this.tooltipBase.tooltipEventCalled = options.tooltipEventCalled;
        this.tooltipBase.sharedTooltipEventCalled = options.sharedTooltipEventCalled;
        this.tooltipBase.crosshairMouseMoveEventCalled = options.crosshairMouseMoveEventCalled;
        this.tooltipBase.seriesTooltipTop = options.seriesTooltipTop;
        this.userInteractionBase.useGrouping = options.useGrouping;
        this.userInteractionBase.focusable = options.focusable;
        this.tooltipBase.tooltipDuration = this.tooltipBase.tooltipModule.duration !== 0 ? this.tooltipBase.tooltipModule.duration : (this.tooltipBase.tooltipModule.shared ? 100 : 300);
        if (this.crosshairEnable) {
            this.togglePointerEvents(this.element.id, false);
        }
        else if (this.pinchZoomingEnable) {
            this.togglePointerEvents(this.element.id, true);
        }
    };
    SfChart.prototype.getElementSize = function (id) {
        const element = document.getElementById(id);
        if (element) {
            const tooltipTemplate = document.getElementById(this.element.id + 'tooltip_template');
            if (tooltipTemplate) {
                element.innerHTML = '';
                element.appendChild(tooltipTemplate);
                this.tooltipBase.tooltipElementSize = {
                    width: element.offsetWidth,
                    height: element.offsetHeight
                };
                element.removeChild(element.firstChild);
            }
        }
    }
    SfChart.prototype.clipPathID = function (index) {
        return this.element.id + '_ChartSeriesClipRect_' + index;
    };
    SfChart.prototype.markerClipPathId = function (index) {
        return this.element.id + '_ChartMarkerClipRect_' + index;
    };
    SfChart.prototype.getVisibleSeries = function (charts, clipRects, seriesMarkers, seriesBorders, axes) {
        charts.userInteractionBase.visibleSeries = [];
        charts.userInteractionBase.axes = axes;
        const seriesCollection = document.getElementById(charts.element.id + 'SeriesCollection').querySelectorAll('[id*="SeriesGroup"]');
        const trendlineCollection = document.getElementById(charts.element.id + 'TrendLineCollection').querySelectorAll('[id*="TrendLineSeriesGroup"]');
        let allSeries = [];
        if (seriesCollection.length > 0) {
            seriesCollection.forEach(function (item) {
                if (item.lastElementChild && item.lastElementChild.id.indexOf('NDLine') === -1) {
                    allSeries.push(item);
                }
            });
        }
        if (trendlineCollection.length > 0) {
            trendlineCollection.forEach(function (item) {
                allSeries.push(item);
            });
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(allSeries) && allSeries.length > 0) {
            allSeries = allSeries.filter((element) => {
                return !sfBlazorToolkit.base.isNullOrUndefined(element) &&
                    !sfBlazorToolkit.base.isNullOrUndefined(element.getAttribute('data-point')) &&
                    element.getAttribute('data-point').split(/(?![^(]*\)),/)[8] === 'True';
            });
        }
        for (let i = 0; i < allSeries.length; i++) {
            const elementId = allSeries[parseInt(i.toString(), 10)].id;
            const element = document.getElementById(elementId);
            const symbolGroup = elementId.indexOf('Series') > -1 ? document.getElementById(elementId.replace('Series', 'Symbol')) : null;
            const raw = !sfBlazorToolkit.base.isNullOrUndefined(element) ? element.getAttribute('data-point') : null;
            const dataPoints = !sfBlazorToolkit.base.isNullOrUndefined(raw) ? this.parseDataPoint(raw) : null;
            const symbolDataPoints = symbolGroup && symbolGroup.childNodes.length > 1 && !sfBlazorToolkit.base.isNullOrUndefined(symbolGroup.childNodes[2]) && !sfBlazorToolkit.base.isNullOrUndefined(symbolGroup.childNodes[2].getAttribute('data-point')) ? this.parseDataPoint(symbolGroup.childNodes[2].getAttribute('data-point')) : null;
            const seriesShape = symbolDataPoints && symbolDataPoints.length > 0 ? symbolDataPoints[12] : dataPoints[12];
            const series = {
                id: elementId,
                name: dataPoints[14],
                index: parseInt(dataPoints[4], 10),
                interior: dataPoints[2],
                visible: dataPoints[8] === 'True',
                category: dataPoints[10],
                seriesType: dataPoints[11],
                type: dataPoints[3],
                marker: seriesMarkers[parseInt(i.toString(), 10)],
                border: seriesBorders[parseInt(i.toString(), 10)],
                shape: seriesShape,
                points: !sfBlazorToolkit.base.isNullOrUndefined(this.userInteractionBase.chartData[parseInt(i.toString(), 10)]) && this.userInteractionBase.chartData[parseInt(i.toString(), 10)] !== '' ? JSON.parse(charts.userInteractionBase.chartData[parseInt(i.toString(), 10)]) : [],
                clipRect: clipRects[parseInt(i.toString(), 10)],
                tooltipFormat: dataPoints[13] !== '' ? this.getTooltipFormat(dataPoints[13]) : dataPoints[13],
                enableTooltip: dataPoints[15] === 'True',
                dataEditSettings: dataPoints[16] === 'True',
                chartIsTransposed: dataPoints[17] === 'True',
                opacity: parseFloat(dataPoints[18]),
                x_Axis: this.userInteractionBase.axes.filter((axis) => axis.name === dataPoints[19])[0],
                y_Axis: this.userInteractionBase.axes.filter((axis) => axis.name === dataPoints[20])[0],
                axesCount: this.userInteractionBase.axes.length,
                volume: dataPoints[21],
                markerDataLabelFormat: dataPoints[22],
                xMin: parseInt(dataPoints[23], 10),
                xMax: parseInt(dataPoints[24], 10),
                emptyPointMode: dataPoints[25],
                focusable: dataPoints[26] === 'True',
                showNearestTooltip: dataPoints[27] === 'True',
                width: Number(dataPoints[28])
            };
            charts.userInteractionBase.visibleSeries.push(series);
        }
        this.setSelectionProperties(charts, charts.userInteractionBase.visibleSeries);
    };
    SfChart.prototype.parseDataPoint = function (raw) {
        const result = [];
        let buffer = '';
        let insideSlash = false;
        const isStartSlash = (s) => s.length > 0 && s.charCodeAt(0) === 47;
        const isEndSlash = (s) => s.length > 0 && s.charCodeAt(s.length - 1) === 47;
        raw.split(/(?![^(]*\)),/).forEach((part) => {
            if (insideSlash) {
                buffer += ',' + part;
                if (isEndSlash(part)) {
                    result.push(buffer.slice(1, -1));
                    insideSlash = false;
                    buffer = '';
                }
            }
            else if (isStartSlash(part) && !isEndSlash(part)) {
                buffer = part;
                insideSlash = true;
            }
            else if (isStartSlash(part) && isEndSlash(part)) {
                result.push(part.slice(1, -1));
            }
            else {
                result.push(part);
            }
        });
        return result;
    };
    SfChart.prototype.setSelectionProperties = function (chart, seriesCollection) {
        seriesCollection.forEach(function (series) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(chart.seriesTypes) && series.index >= 0 && series.index < chart.seriesTypes.length) {
                chart.seriesTypes[series.index] = series.type;
            }
            else {
                chart.seriesTypes.push(series.type);
            }
        });
    };
    SfChart.prototype.getTooltipFormat = function (text) {
        text = text.replace('tooltip', 'tT').replace('text', 't').replace('high', 'h').replace('low', 'l').replace('open', 'o')
            .replace('close', 'c').replace('volume', 'v').replace('size', 'sI').replace('percentage', 'p').replace('minimum', 'mI')
            .replace('maximum', 'mX').replace('outliers', 'oL').replace('upperQuartile', 'uQ').replace('lowerQuartile', 'lQ').replace('median', 'm')
            .replace('average', 'a');
        return text;
    };
    SfChart.prototype.calculateSecondaryOffset = function (id) {
        const svgRect = document.getElementById(id + '_svg').getBoundingClientRect();
        const rect = document.getElementById(id).getBoundingClientRect();
        if (!this.userInteractionBase.secondaryElementOffset) {
            return;
        }
        this.userInteractionBase.secondaryElementOffset.left = Math.max(svgRect.left - rect.left, 0);
        this.userInteractionBase.secondaryElementOffset.top = Math.max(svgRect.top - rect.top, 0);
    };
    SfChart.prototype.unWireEvents = function (id, dotnetref) {
        const element = document.getElementById(id);
        this.dotnetref = dotnetref;
        dotnetrefCollection = dotnetrefCollection.filter((item) => {
            return item.id !== id;
        });
        /*! Find the Events type */
        const cancelEvent = sfBlazorToolkit.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        /*! Bind the Event handler */
        if (element) {
            element.removeEventListener('mousemove', this.mouseMoveRef);
            element.removeEventListener('touchmove', this.mouseMoveRef);
            sfBlazorToolkit.base.EventHandler.remove(element, sfBlazorToolkit.base.Browser.touchStartEvent, this.chartOnMouseDownRef);
            sfBlazorToolkit.base.EventHandler.remove(element, sfBlazorToolkit.base.Browser.touchEndEvent, this.mouseEndRef);
            sfBlazorToolkit.base.EventHandler.remove(element, 'click', this.chartOnMouseClickRef);
            sfBlazorToolkit.base.EventHandler.remove(element, 'contextmenu', this.chartRightClickRef);
            sfBlazorToolkit.base.EventHandler.remove(element, cancelEvent, this.mouseLeaveRef);
            sfBlazorToolkit.base.EventHandler.remove(element, 'keydown', this.chartKeyDownRef);
            sfBlazorToolkit.base.EventHandler.remove(document.body, 'keydown', this.documentKeyHandler);
            sfBlazorToolkit.base.EventHandler.remove(element, 'keyup', this.chartKeyUpRef);
            const wheelEvent = sfBlazorToolkit.base.Browser.info.name === 'mozilla' ? (sfBlazorToolkit.base.Browser.isPointer ? 'mousewheel' : 'DOMMouseScroll') : 'mousewheel';
            element.removeEventListener(wheelEvent, this.chartMouseWheelRef);
        }
        window.removeEventListener('mousedown', this.scrollDownRef);
        window.removeEventListener('touchstart', this.scrollDownRef);
        window.removeEventListener('mousemove', this.domMouseMoveRef);
        window.removeEventListener('touchmove', this.domMouseMoveRef);
        window.removeEventListener('mouseup', this.domMouseUpRef, false);
        window.removeEventListener('touchend', this.domMouseUpRef, false);
        if (document.getElementsByClassName('e-chart').length === 0) {
            const resize = sfBlazorToolkit.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
            sfBlazorToolkit.base.EventHandler.remove(window, resize, resizeBound);
        }
        if (this.touchObject) {
            this.touchObject.destroy();
            this.touchObject = null;
        }
        /*! Apply the style for chart */
    };
    SfChart.prototype.wireEvents = function (id, dotnetref) {
        const element = document.getElementById(id);
        if (!element) {
            return;
        }
        this.dotnetref = dotnetref;
        dotnetrefCollection.push({ id: id, dotnetref: dotnetref });
        /*! Find the Events type */
        const cancelEvent = sfBlazorToolkit.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        this.chartOnMouseDownRef = this.chartOnMouseDown.bind(this, dotnetref, id);
        this.mouseEndRef = this.mouseEnd.bind(this, dotnetref, id);
        this.mouseMoveRef = this.mouseMove.bind(this, dotnetref, id);
        this.chartOnMouseClickRef = this.chartOnMouseClick.bind(this, dotnetref, id);
        this.chartRightClickRef = this.chartRightClick.bind(this, dotnetref, id);
        this.chartKeyDownRef = this.chartOnKeyDown.bind(this, this.dotnetref, this.id);
        this.chartKeyUpRef = this.chartOnKeyUp.bind(this, this.dotnetref, this.id);
        this.mouseLeaveRef = this.mouseLeave.bind(this, dotnetref, id);
        /*! Bind the Event handler */
        element.addEventListener('mousemove', this.mouseMoveRef);
        element.addEventListener('touchmove', this.mouseMoveRef);
        sfBlazorToolkit.base.EventHandler.add(element, sfBlazorToolkit.base.Browser.touchStartEvent, this.chartOnMouseDownRef);
        sfBlazorToolkit.base.EventHandler.add(element, sfBlazorToolkit.base.Browser.touchEndEvent, this.mouseEndRef);
        sfBlazorToolkit.base.EventHandler.add(element, 'click', this.chartOnMouseClickRef);
        sfBlazorToolkit.base.EventHandler.add(element, 'contextmenu', this.chartRightClickRef);
        sfBlazorToolkit.base.EventHandler.add(element, cancelEvent, this.mouseLeaveRef);
        sfBlazorToolkit.base.EventHandler.add(element, 'keydown', this.chartKeyDownRef);
        sfBlazorToolkit.base.EventHandler.add(document.body, 'keydown', this.documentKeyHandler, this);
        sfBlazorToolkit.base.EventHandler.add(element, 'keyup', this.chartKeyUpRef);
        this.chartMouseWheelRef = this.chartMouseWheel.bind(this, dotnetref, id);
        const wheelEvent = sfBlazorToolkit.base.Browser.info.name === 'mozilla' ? (sfBlazorToolkit.base.Browser.isPointer ? 'mousewheel' : 'DOMMouseScroll') : 'mousewheel';
        element.addEventListener(wheelEvent, this.chartMouseWheelRef);
        this.scrollDownRef = this.scrollDown.bind(this, dotnetref, id);
        this.domMouseMoveRef = this.domMouseMove.bind(this, dotnetref, id);
        this.domMouseUpRef = this.domMouseUp.bind(this, dotnetref, id);
        window.addEventListener('mousedown', this.scrollDownRef);
        window.addEventListener('touchstart', this.scrollDownRef);
        window.addEventListener('mousemove', this.domMouseMoveRef);
        window.addEventListener('touchmove', this.domMouseMoveRef);
        window.addEventListener('mouseup', this.domMouseUpRef, false);
        window.addEventListener('touchend', this.domMouseUpRef, false);
        resizeBound = chartResize.bind(this, dotnetrefCollection);
        const resize = sfBlazorToolkit.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        sfBlazorToolkit.base.EventHandler.add(window, resize, resizeBound);
        this.longPressBound = this.longPress.bind(this, dotnetref, id);
        this.touchObject = new sfBlazorToolkit.Touch(element, { tapHold: this.longPressBound, tapHoldThreshold: 500 });
    };
    SfChart.prototype.getEventArgs = function (e, id) {
        const clientX = e.changedTouches ? e.changedTouches[0].clientX : e.clientX;
        const clientY = e.changedTouches ? e.changedTouches[0].clientY : e.clientY;
        this.setMouseXY(clientX, clientY, id);
        const touches = e.touches;
        const touchList = [];
        if (e.type.indexOf('touch') > -1) {
            for (let i = 0, length = touches.length; i < length; i++) {
                touchList.push({ pageX: touches[parseInt(i.toString(), 10)].clientX, pageY: touches[parseInt(i.toString(), 10)].clientY, pointerId: e.pointerId || 0 });
            }
        }
        const parentId = this.getLegendTemplateParentId(e);
        return {
            type: e.type,
            clientX: e.clientX,
            clientY: e.clientY,
            mouseX: this.mouseX,
            mouseY: this.mouseY,
            pointerType: e.pointerType,
            target: parentId && parentId !== '' ? parentId : e.target.id,
            changedTouches: {
                clientX: e.changedTouches ? e.changedTouches[0].clientX : 0,
                clientY: e.changedTouches ? e.changedTouches[0].clientY : 0
            },
            touches: touchList,
            pointerId: e.pointerId
        };
    };
    SfChart.prototype.getLegendTemplateParentId = function (event) {
        const target = event.target;
        if (target) {
            const matchingAncestor = target.closest('[id*="' + this.id + '_chart_legend_template_"]');
            return matchingAncestor ? matchingAncestor.id : '';
        }
        return '';
    };
    SfChart.prototype.getWheelArgs = function (e, id) {
        this.setMouseXY(e.clientX, e.clientY, id);
        return {
            detail: e.detail,
            wheelDelta: e['wheelDelta'],
            target: e.currentTarget ? e.currentTarget['id'] : e.srcElement ? e.srcElement['id'] : e.target ? e.target['id'] : '',
            clientX: e.clientX,
            clientY: e.clientY,
            mouseX: this.mouseX,
            mouseY: this.mouseY,
            browserName: sfBlazorToolkit.base.Browser.info.name,
            isPointer: sfBlazorToolkit.base.Browser.isPointer
        };
    };
    SfChart.prototype.setMouseXY = function (pageX, pageY, id) {
        const svgRect = document.getElementById(id + '_svg').getBoundingClientRect();
        const rect = document.getElementById(id).getBoundingClientRect();
        if (!this.userInteractionBase.secondaryElementOffset) {
            return;
        }
        this.userInteractionBase.secondaryElementOffset.left = Math.max(svgRect.left - rect.left, 0);
        this.userInteractionBase.secondaryElementOffset.top = Math.max(svgRect.top - rect.top, 0);
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.userInteractionBase.availableSize) && !sfBlazorToolkit.base.isNullOrUndefined(svgRect) &&
            (this.userInteractionBase.availableSize.width !== svgRect.width || this.userInteractionBase.availableSize.height !== svgRect.height)) {
            this.scaleX = svgRect.width / this.userInteractionBase.availableSize.width;
            this.scaleY = svgRect.height / this.userInteractionBase.availableSize.height;
        }
        this.mouseY = ((pageY - rect.top) - this.userInteractionBase.secondaryElementOffset.top) / (this.scaleX !== 0 ? this.scaleX : 1);
        this.mouseX = ((pageX - rect.left) - this.userInteractionBase.secondaryElementOffset.left) / (this.scaleY !== 0 ? this.scaleY : 1);
    };
    SfChart.prototype.chartOnMouseDown = function (dotnetref, id, e) {
        // Mouse buttons are identified by numbers:
        // 0 for the left button.
        // 1 for the middle button.
        // 2 for the right button.
        // Here, we are restricting the right and middle button click to perform the selection and zooming.
        if (e.button === 1 || e.button === 2) {
            return false;
        }
        this.dotnetref = dotnetref;
        let target = e.target;
        const parentId = this.getLegendTemplateParentId(e);
        const parentTarget = findDOMElement(parentId);
        if (!sfBlazorToolkit.base.isNullOrUndefined(parentTarget)) {
            target = parentTarget;
        }
        const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
        if (target.id.indexOf('legend') > 0) {
            removeTooltipCrosshairOnZoom(this);
            if (e.type === 'touchstart') {
                chart.isTouchLegendClick = true;
            }
            if (!chart.enableHighlight) {
                chart.previousHighlightedIndex = -1;
            }
        }
        let pageX;
        let pageY;
        let touchArg;
        if (e.type === 'touchstart') {
            this.isTouch = true;
            touchArg = e;
            pageX = touchArg.changedTouches[0].clientX;
            pageY = touchArg.changedTouches[0].clientY;
        }
        else {
            this.isTouch = e.pointerType === 'touch' || e.pointerType === '2' || this.isTouch;
            pageX = e.clientX;
            pageY = e.clientY;
        }
        if (e.type.indexOf('touch') > -1) {
            const clientX = e.changedTouches ? e.changedTouches[0].clientX : e.clientX;
            const clientY = e.changedTouches ? e.changedTouches[0].clientY : e.clientY;
            const isMultiTouch = e.touches.length > 1;
            const offset = sfBlazorToolkit.base.Browser.isDevice ? 20 : 30;
            this.prevTabMouseX = this.mouseX;
            this.prevTabMouseY = this.mouseY;
            this.isDoubleTap = (new Date().getTime() < this.touchThreshold) && Math.abs(this.mouseX - this.prevTabMouseX) <= offset && Math.abs(this.mouseY - this.prevTabMouseY) <= offset;
            if (this.isDoubleTap) {
                if ((chart.selectionMode.indexOf('Drag') > -1 && !(target.id.indexOf('ej2_drag_') > -1)) || (chart.zoomBase.zoomSettings && chart.zoomBase.zoomSettings.enableSelectionZooming)) {
                    e.preventDefault();
                }
            }
            if (chart.pinchZoomingEnable) {
                this.pinchtarget = document.getElementById(id + '_Pinch_target');
                this.pinchtarget.setAttribute('style', this.pinchStyle + ' top: ' + (clientY - 50) + 'px; left: ' + (clientX - 50) + 'px;');
                this.togglePointerEvents(id, chart.crosshairEnable && chart.userInteractionBase.startMove ? isMultiTouch : true);
                if (sfBlazorToolkit.base.Browser.info.name.toLowerCase() !== 'mozilla' && isMultiTouch) {
                    e.stopImmediatePropagation();
                }
            }
            else if (chart.crosshairEnable && chart.userInteractionBase.startMove) {
                this.togglePointerEvents(id, false);
                e.preventDefault();
            }
        }
        if (this.zoomBase.zoomingModule) {
            if (target.id.indexOf('_Zooming_') > -1) {
                zoomToolkitMouseDown(this, target);
            }
            if (!(target.id.indexOf('_Zooming_') > -1 || target.id.indexOf('_scrollBar_') > -1)) {
                onZoomingMouseDown(e, pageX, pageY, this);
            }
        }
        if (target && target.id && target.id.indexOf('scrollbar') > 0) {
            return false;
        }
        dotnetref.invokeMethodAsync('OnZoomingMouseDown', this.getEventArgs(e, id), Boolean(this.zoomBase.zoomingModule && this.zoomBase.zoomingModule.isPanning && this.zoomBase.isChartDrag));
        return false;
    };
    SfChart.prototype.togglePointerEvents = function (id, stopPointerEvents) {
        if (stopPointerEvents) {
            this.setPointerEvents(id, 'none');
        }
        else {
            this.setPointerEvents(id, 'auto');
        }
    };
    SfChart.prototype.setPointerEvents = function (id, pointEventStyle) {
        const chartRectStyle = document.createElement('style');
        chartRectStyle.id = id + '_sfchart_rect_style';
        chartRectStyle.innerHTML = '#' + id + '_ChartAreaBorder, #' + id + '_ChartBorder {' + 'pointer-events: ' + pointEventStyle + '; }';
        document.body.appendChild(chartRectStyle);
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.userInteractionBase.theme) ) {
            chartRectStyle.innerHTML += 'rect[id*="_ej2_drag_rect"], circle[id*="_ej2_drag_closecircle_"] { stroke-dasharray: 5 !important; }';
        }
    };
    SfChart.prototype.chartMouseWheel = function (dotnetref, id, e) {
        if (!(this.isZooming || this.isScrollbar)) {
            return false;
        }
        this.dotnetref = dotnetref;
        if (this.zoomBase.zoomingModule || this.isScrollbar) {
            removeTooltipCrosshair();
            zoomingMouseWheelHandler(e, this);
        }
        dotnetref.invokeMethodAsync('OnChartMouseWheel', this.getWheelArgs(e, id));
        if (this.wheelEventEndTimeout) {
            clearTimeout(this.wheelEventEndTimeout);
        }
        this.wheelEventEndTimeout = setTimeout(() => {
            dotnetref.invokeMethodAsync('UpdateChartData');
            this.isWheelScrolling = false;
            this.isMouseWheelZoom = false;
        }, 100);
        if (this.isMouseWheelZoom || this.isWheelScrolling) {
            e.preventDefault();
        }
        return false;
    };
    SfChart.prototype.mouseMove = function (dotnetref, id, e, isNavigation = false) {
        this.isChartKeyDown = false;
        const parentId = this.getLegendTemplateParentId(e);
        const parentTarget = findDOMElement(parentId);
        let target = e.target;
        if (!sfBlazorToolkit.base.isNullOrUndefined(parentTarget)) {
            target = parentTarget;
        }
        let isSkip;
        let isHighlighted = false;
        if (!sfBlazorToolkit.base.isNullOrUndefined(document.getElementById(target.id))) {
            if (this.highlightMode !== 'None' || this.enableHighlight) {
                if (!sfBlazorToolkit.base.isNullOrUndefined(target) || target.id.indexOf('Point') > -1 || target.id.indexOf('Symbol') > -1) {
                    if (target.id.indexOf('text') > -1) {
                        target = findDOMElement(target.id.replace('text', 'shape'));
                    }
                    else {
                        target = findDOMElement(target.id);
                    }
                    if (!sfBlazorToolkit.base.isNullOrUndefined(target) && ((target).hasAttribute('class') && ((target).getAttribute('class').indexOf('highlight') > -1 ||
                        (target).getAttribute('class').indexOf('selection') > -1))) {
                        isSkip = true;
                    }
                }
                if (!isSkip) {
                    if (this.highlightMode !== 'None') {
                        calculateSelectedElements(e, this.dataId);
                    }
                    let currentIndex;
                    const values = [this.id + '_chart_legend_text_', this.id + '_chart_legend_shape_marker_', this.id + '_chart_legend_shape_', this.id + '_chart_legend_g_', this.id + '_chart_legend_template_'];
                    for (let i = 0; i < values.length; i++) {
                        if (!sfBlazorToolkit.base.isNullOrUndefined(target) && !target.id.indexOf(values[parseInt(i.toString(), 10)])) {
                            currentIndex = parseInt(target.id.split(values[parseInt(i.toString(), 10)])[1], 10);
                            break;
                        }
                    }
                    const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
                    if ((!sfBlazorToolkit.base.isNullOrUndefined(target) && target.id.indexOf('legend') > 0 && (this.enableHighlight && isTargetChanged(currentIndex, chart) || this.highlightMode === 'Series') && !sfBlazorToolkit.base.Browser.isDevice && !this.isTouchLegendClick)) {
                        this.legendVisible = true;
                        legendSelection(e, chart.dataId);
                        isHighlighted = true;
                    }
                    if (!sfBlazorToolkit.base.isNullOrUndefined(target) && !(target.id.indexOf('legend') > 0)) {
                        resetPreviousHighlightIndex(chart);
                    }
                    if (!sfBlazorToolkit.base.isNullOrUndefined(target) && this.highlightMode === 'None' && !(target.id.indexOf('legend') > 0) && chart.highlightDataIndexes.length > 0) {
                        chart.isLegendHighlighting = false;
                        chart.prevHighlightedSeriesIndex = -1;
                        removeSelectionStyles(this.dataId);
                        if (this.selectionMode !== 'None') {
                            redrawSelection(this.dataId);
                        }
                    }
                    else if (!sfBlazorToolkit.base.isNullOrUndefined(target) && this.highlightMode !== 'None' && chart.previousSelectedEle.length > 0 && !isHighlighted && chart.isLegendHighlighting && chart.prevHighlightedSeriesIndex !== -1 && !isRectSeries(chart.seriesTypes[chart.prevHighlightedSeriesIndex])) {
                        const seriesStyle = generateStyle(chart.element.id + 'SeriesGroup' + chart.prevHighlightedSeriesIndex, chart);
                        const elements = chart.previousSelectedEle.filter(function (element) {
                            return element.classList.contains(seriesStyle);
                        });
                        if (chart.selectedDataIndexes.length > 0) {
                            const isSeriesSelected = chart.selectedDataIndexes.some((indexes) => indexes.series === chart.prevHighlightedSeriesIndex);
                            chart.previousSelectedEle.forEach(function (element) {
                                element.classList.remove(seriesStyle);
                                if (!isSeriesSelected) {
                                    element.classList.add(chart.unSelected);
                                }
                            });
                        }
                        else {
                            elements.forEach(function (element) {
                                element.classList.remove(seriesStyle);
                            });
                        }
                        chart.isLegendHighlighting = false;
                        chart.prevHighlightedSeriesIndex = -1;
                    }
                }
            }
        }
        if (this.isTouchLegendClick) {
            this.isTouchLegendClick = false;
        }
        if (isNavigation) {
            return;
        }
        let pageX;
        let pageY;
        let touchArg;
        if (e.type === 'touchmove') {
            this.isTouch = true;
            touchArg = e;
            pageX = touchArg.changedTouches[0].clientX;
            pageY = touchArg.changedTouches[0].clientY;
            const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
            if (this.pinchtarget && chart.pinchZoomingEnable) {
                this.pinchtarget.setAttribute('style', this.pinchStyle + ' top: ' + (pageY - 50) + 'px; left: ' + (pageX - 50) + 'px;');
                e.preventDefault();
            }
            if (chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable && chart.userInteractionBase.startMove) {
                e.preventDefault();
            }
        }
        else {
            this.isTouch = e.pointerType === 'touch' || e.pointerType === '2' || this.isTouch;
            pageX = e.clientX;
            pageY = e.clientY;
        }
        this.dotnetref = dotnetref;
        if (document.getElementById(id + '_svg')) {
            this.setMouseXY(pageX, pageY, id);
            if (this.userInteractionBase.chartStartMove) {
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.tooltipBase.tooltipModule)) {
                    tooltipMousemovehandler(this);
                }
                if (this.markerExplodeBase.markerExploded && (!this.isTouch || this.userInteractionBase.startMove)) {
                    markerMove(this, false);
                }
                if (this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable) {
                    crosshairMousemoveHandler(this);
                }
            }
            if (!this.isTouch) {
                if (!sfBlazorToolkit.base.isNullOrUndefined(target) && target.id.indexOf('_Zooming_') > -1) {
                    zoomToolkitTooltip(this, target.id, e);
                }
                else {
                    zoomToolkitRemoveTooltip(this);
                }
            }
            if (this.zoomBase.zoomingModule) {
                onZoomingMouseMove(this.getEventArgs(e, id), this);
            }
            if (target && target.id && target.id.indexOf('scrollbar') > 0) {
                return;
            }
            if (this.mouseMoveThreshold == null || (new Date().getTime() - this.mouseMoveThreshold) > this.eventInterval) {
                this.mouseMoveThreshold = new Date().getTime();
                const type = target.id.indexOf('_ChartTitle') > -1 ? 'Title' : target.id.indexOf('_ChartSubTitle') > -1 ? 'SubTitle' : '';
                const element = document.getElementById(target.id);
                const titleTooltipElement = document.getElementById(`${this.id}_EJ2_Title_Tooltip`);
                const axisTooltipElement = document.getElementById(`${this.id}_EJ2_AxisLabel_Tooltip`);
                const isTitleRemoveTooltip = type == "" && titleTooltipElement && titleTooltipElement.textContent !== '';
                const isAxisLabelTooltip = target.id.indexOf('_AxisLabel_') > -1 && element.textContent.indexOf('...') > -1;
                const isAxisLabelRemoveTooltip = !isAxisLabelTooltip && axisTooltipElement && axisTooltipElement.textContent !== '';
                const isStripLineTooltip = this.isStripLineTooltip && target.id.indexOf('_Series_') > -1 || target.id.indexOf('_stripline_Behind_') > -1;
                var isDataEditDrag = this.userInteractionBase.isPointMouseDown === true || this.userInteractionBase.isPointDragging === true;
                if (type !== '' || isAxisLabelTooltip || isStripLineTooltip || isTitleRemoveTooltip || isAxisLabelRemoveTooltip || this.isMouseMoveEvent || isDataEditDrag || this.selectionMode && this.selectionMode.indexOf('Drag') > -1) {
                    dotnetref.invokeMethodAsync('OnZoomingMouseMove', this.getEventArgs(e, id));
                }
            }
            this.isTouch = false;
        }
    };
    SfChart.prototype.mouseEnd = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        const touchArg = e.type === 'touchend' ? e : null;
        const pageX = touchArg ? touchArg.changedTouches[0].clientX : e.clientX;
        const pageY = touchArg ? touchArg.changedTouches[0].clientY : e.clientY;
        this.isTouch = e.type.indexOf('touch') > -1 ? true : (e.pointerType === 'touch' || e.pointerType === '2');
        if (document.getElementById(id + '_svg')) {
            this.setMouseXY(pageX, pageY, id);
            if (this.isTouch) {
                if ((this.selectionMode.indexOf('Drag') > -1 && !(e.target.id.indexOf('ej2_drag_') > -1)) ||
                    (this.zoomBase.zoomSettings && this.zoomBase.zoomSettings.enableSelectionZooming)) {
                    this.touchThreshold = new Date().getTime() + 300;
                }
                if (this.tooltipBase.tooltipModule.enable && !(this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable) && !isSelected() &&
                    ((svgbase.withInAreaBounds(this.mouseX, this.mouseY, this.userInteractionBase.axisClipRect) && this.tooltipBase.tooltipModule.shared) || !this.tooltipBase.tooltipModule.shared)) {
                    tooltip(this);
                    markerMove(this, false);
                }
                if (!this.tooltipBase.tooltipModule.enable && !(this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable) && this.markerExplodeBase.markerExploded) {
                    markerMove(this, false);
                }
                if (!this.userInteractionBase.startMove) {
                    if (this.clearTooltip) {
                        clearTimeout(this.clearTooltip);
                    }
                    this.clearTooltip = setTimeout(() => {
                        if (this.tooltipBase.tooltipModule.enable) {
                            removeTooltip(300, this);
                        }
                        else {
                            removeMarker(this);
                        }
                    }, 1000);
                    const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
                    if (this.clearCrosshair) {
                        clearTimeout(this.clearCrosshair);
                    }
                    this.clearCrosshair = setTimeout(function () {
                        removeCrosshair(chart, 300);
                    }, 1000);
                }
            }
            if (this.userInteractionBase.startMove) {
                if (this.tooltipBase.tooltipModule.enable) {
                    removeTooltip(2000, this);
                }
                else {
                    removeMarker(this);
                }
                removeCrosshair(this, 2000);
                this.userInteractionBase.startMove = false;
            }
            const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
            if (this.highlightMode === 'None' && this.selectionMode === 'Series' && !chart.toggleVisibility && chart.highlightDataIndexes.length > 0) {
                removeSelectionStyles(this.dataId);
            }
        }
        if (e.type === 'touchend') {
            if (this.pinchZoomingEnable) {
                const isMultiTouch = e.touches.length > 0;
                this.togglePointerEvents(id, this.crosshairEnable ? isMultiTouch : true);
            }
            else {
                this.togglePointerEvents(id, false);
            }
        }
        if (this.zoomBase.zoomingModule) {
            this.zoomBase.threshold = new Date().getTime() + 300;
            if (this.zoomBase.isSkipZooming || this.zoomBase.zoomingModule.isZooming) {
                setTimeout(() => {
                    this.zoomBase.isSkipZooming = false;
                    this.zoomBase.zoomingModule.isZooming = false;
                }, 500);
            }
            onZoomingMouseEnd(this.getEventArgs(e, id), this);
        }
        const target = e.target;
        if (target && target.id && target.id.indexOf('scrollbar') > 0) {
            return false;
        }
        dotnetref.invokeMethodAsync('OnZoomingMouseEnd', this.getEventArgs(e, id));
        return false;
    };
    SfChart.prototype.chartOnMouseClick = function (dotnetref, id, e) {
        calculateSelectedElements(e, this.dataId);
        const parentId = this.getLegendTemplateParentId(e);
        const parentTarget = findDOMElement(parentId);
        let target = e.target;
        if (!sfBlazorToolkit.base.isNullOrUndefined(parentTarget)) {
            target = parentTarget;
        }
        const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
        if (target.id.indexOf('legend') > 0) {
            if (!chart.toggleVisibility && this.selectionMode === 'Series') {
                this.legendVisible = true;
                legendSelection(e, chart.dataId);
            }
            else if (chart.toggleVisibility && chart.enableHighlight) {
                chart.isLegendHighlighting = false;
                chart.prevHighlightedSeriesIndex = -1;
                storeToggledLegendIndexes(target, chart, e);
            }
        }
        if (!(e instanceof PointerEvent) && !(e instanceof TouchEvent)) {
            return; // Prevent further invocation if x or y is null
        }
        dotnetref.invokeMethodAsync('OnChartMouseClick', this.getEventArgs(e, id));
    };
    SfChart.prototype.chartRightClick = function (dotnetref, id, event) {
        this.dotnetref = dotnetref;
        const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
        if (chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable && (event.buttons === 2 || event.pointerType === 'touch')) {
            event.preventDefault();
            event.stopPropagation();
        }
        dotnetref.invokeMethodAsync('OnChartMouseRightClick', this.getEventArgs(event, id));
        return false;
    };
    SfChart.prototype.documentKeyHandler = function (e) {
        if (e.altKey && e.key && e.key.toLowerCase() === 'j' && !sfBlazorToolkit.base.isNullOrUndefined(this.element)) {
            this.element.focus();
        }
    };
    SfChart.prototype.chartOnKeyDown = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        let actionKey = '';
        const target = e.target;
        let isAnnotation = false;
        let parentDiv = target;
        this.isChartKeyDown = true;
        while (parentDiv && parentDiv.parentNode) {
            if (parentDiv.id && parentDiv.id.indexOf('Annotation') > -1) {
                isAnnotation = true;
                break;
            }
            parentDiv = parentDiv.parentNode;
        }
        if ((this.isZoomed && e.code === 'Tab') || (e.code === 'Space' && !isAnnotation)) {
            e.preventDefault();
        }
        if ((this.options.showTooltip || (this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable)) && ((e.code === 'Tab' && this.previousTargetId.indexOf('Series') > -1) || e.code === 'Escape')) {
            actionKey = 'ESC';
        }
        if (e.ctrlKey && (e.key === '+' || e.code === 'Equal' || e.key === '-' || e.code === 'Minus')) {
            e.preventDefault();
            this.isZoomed = this.options.enableZoom;
            fadeOut(this.element);
            actionKey = this.isZoomed ? e.code : '';
        }
        else if (e.key.toLowerCase() === 'r' && this.isZoomed) {
            e.preventDefault();
            this.isZoomed = false;
            actionKey = 'R';
        }
        else if (e.code.indexOf('Arrow') > -1) {
            e.preventDefault();
            actionKey = this.isZoomed ? e.code : '';
        }
        if (e.ctrlKey && (e.key === 'p')) {
            e.preventDefault();
            actionKey = 'CtrlP';
        }
        if (actionKey !== '' && this.userInteractionBase.focusable) {
            if (this.zoomBase.zoomingModule) {
                if (actionKey === 'Equal' || actionKey === 'Minus') {
                    this.zoomBase.zoomingModule.isZoomed = this.zoomBase.zoomingModule.performedUI = true;
                    this.zoomBase.zoomingModule.isPanning = this.zoomBase.isChartDrag = false;
                    if (actionKey === 'Equal') {
                        zoomToolkitZoomIn(this, target.getAttribute('opacity'));
                    }
                    else {
                        zoomToolkitZoomOut(this, target.getAttribute('opacity'));
                    }
                    performZoomRedraw(this, true);
                }
                if (actionKey === 'ArrowUp' || actionKey === 'ArrowDown' || actionKey === 'ArrowLeft' || actionKey === 'ArrowRight') {
                    const yArrowPadding = actionKey === 'ArrowUp' ? 10 : (actionKey === 'ArrowDown' ? -10 : 0);
                    const xArrowPadding = actionKey === 'ArrowLeft' ? -10 : (actionKey === 'ArrowRight' ? 10 : 0);
                    this.zoomBase.zoomingModule.isPanning = this.zoomBase.isChartDrag = true;
                    doPan(this, this.zoomBase.axisCollections, xArrowPadding, yArrowPadding);
                    performZoomRedraw(this, true);
                }
                if (actionKey === 'R') {
                    zoomToolkitReset(this);
                }
            }
            dotnetref.invokeMethodAsync('OnChartKeyboardNavigationsAsync', actionKey, e.target['id']);
        }
        return false;
    };
    SfChart.prototype.processPointSelection = function (dotnetref, chartId, targetId, actionKey) {
        const chartElement = document.getElementById(chartId);
        if (sfBlazorToolkit.base.isNullOrUndefined(chartElement) || sfBlazorToolkit.base.isNullOrUndefined(document.getElementById(targetId))) {
            return;
        }
        if (actionKey === 'Enter') {
            this.chartOnMouseClick(dotnetref, chartId, { target: document.getElementById(targetId), type: 'click' });
        }
        else if (actionKey === 'Tab' || actionKey === 'ArrowMove') {
            this.mouseMove(dotnetref, chartId, { target: document.getElementById(targetId), type: 'mousemove' }, true);
        }
    };
    SfChart.prototype.chartOnKeyUp = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        let actionKey = '';
        const target = e.target;
        let targetId = e.target['id'];
        let groupElement;
        let markerGroup;
        let targetElement = e.target;
        let pointIndex;
        const titleElement = getElement(this.element.id + '_ChartTitle');
        const subTitleElement = getElement(this.element.id + '_ChartSubTitle');
        const seriesElement = getElement(this.element.id + 'SeriesCollection');
        const legendElement = getElement(this.element.id + '_chart_legend_translate_g');
        const pagingElement = getElement(this.element.id + '_chart_legend_pageup');
        const annotationElement = getElement(this.element.id + '_Annotation_Collections');
        const zoomInElement = getElement(this.element.id + '_Zooming_ZoomIn');
        const zoomOutElement = getElement(this.element.id + '_Zooming_ZoomOut');
        const resetElement = getElement(this.element.id + '_Zooming_Reset');
        const trendlineElement = getElement(this.element.id + 'TrendLineCollection');
        const chartZoomed = isAxisZoomed(this.userInteractionBase.axes);
        let currentSeries;
        let isNotSamePointsLength = false;
        if (!this.isZoomed && e.keyCode !== 82 && !chartZoomed && this.userInteractionBase.focusable) {
            if (titleElement) {
                titleElement.setAttribute('class', 'e-chart-focused');
            }
            if (subTitleElement) {
                subTitleElement.setAttribute('class', 'e-chart-focused');
            }
            if (seriesElement && seriesElement.firstElementChild) {
                const firstChild = seriesElement.firstElementChild.children[1];
                if (firstChild) {
                    let className = firstChild.getAttribute('class');
                    if (className && className.indexOf('e-chart-focused') === -1) {
                        className = className + ' e-chart-focused';
                    }
                    else if (!className) {
                        className = 'e-chart-focused';
                    }
                    firstChild.setAttribute('class', className + ' e-chart-focused');
                }
            }
            if (trendlineElement && trendlineElement.firstElementChild) {
                const firstChild = trendlineElement.firstElementChild.children[1];
                if (firstChild) {
                    let className = firstChild.getAttribute('class');
                    if (className && className.indexOf('e-chart-focused') === -1) {
                        className = className + ' e-chart-focused';
                    }
                    else if (!className) {
                        className = 'e-chart-focused';
                    }
                    firstChild.setAttribute('class', className + ' e-chart-focused');
                }
            }
            if (legendElement) {
                const firstChild = legendElement.firstElementChild;
                if (firstChild) {
                    let className = firstChild.getAttribute('class');
                    if (className && className.indexOf('e-chart-focused') === -1) {
                        className = className + ' e-chart-focused';
                    }
                    else if (!className) {
                        className = 'e-chart-focused';
                    }
                    firstChild.setAttribute('class', className);
                }
            }
            if (annotationElement) {
                const firstChild = annotationElement.firstElementChild;
                if (firstChild) {
                    let className = firstChild.getAttribute('class');
                    if (className && className.indexOf('e-chart-focused') === -1) {
                        className = className + ' e-chart-focused';
                    }
                    else if (!className) {
                        className = 'e-chart-focused';
                    }
                    firstChild.setAttribute('class', className);
                }
            }
            if (pagingElement) {
                pagingElement.setAttribute('class', 'e-chart-focused');
            }
            if (e.code === 'Tab') {
                if (this.previousTargetId !== '') {
                    if (this.previousTargetId.indexOf('Series') > -1 && targetId.indexOf('Series') === -1) {
                        groupElement = getElement(this.element.id + (this.previousTargetId.indexOf('TrendLine') > -1 ? 'TrendLineCollection' : 'SeriesCollection'));
                        if (!sfBlazorToolkit.base.isNullOrUndefined(groupElement.children[0])) {
                            let previousElement;
                            if (this.previousTargetId.indexOf('_PointIndex_') > -1) {
                                previousElement = getElement(this.seriesPathElement.id);
                            }
                            else {
                                previousElement = this.previousTargetId.indexOf('_Symbol') > -1 ? getElement(this.previousTargetId.indexOf('TrendLine') > -1 ? (this.element.id + '_Series_' + this.currentSeriesIndex + '_TrendLine_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex + '_Symbol') : (this.element.id + '_Series_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex + '_Symbol')) :
                                    (this.previousTargetId.indexOf('_Point_') > -1 ?
                                        getElement(this.previousTargetId.indexOf('TrendLine') > -1 ? (this.element.id + '_Series_' + this.currentSeriesIndex + '_TrendLine_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex) : (this.element.id + '_Series_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex)) :
                                        getElement(this.element.id + (this.previousTargetId.indexOf('TrendLine') > -1 ? 'TrendlineSeriesGroup' : 'SeriesGroup') + this.currentSeriesIndex));
                            }
                            const seriesId = getFocusedSeries(this, groupElement.firstElementChild.id);
                            setTabIndex(previousElement, getElement(seriesId));
                            this.currentPointIndex = 0;
                            this.currentSeriesIndex = 0;
                            this.currentLegendIndex = 0;
                        }
                    }
                    else if (this.previousTargetId.indexOf('_chart_legend_page') > -1 && targetId.indexOf('_chart_legend_page') === -1
                        && targetId.indexOf('_chart_legend_g_') === -1) {
                        setTabIndex(e.target, getElement(this.element.id + '_chart_legend_pageup'));
                    }
                    else if (this.previousTargetId.indexOf('_chart_legend_g_') > -1 && targetId.indexOf('_chart_legend_g_') === -1) {
                        groupElement = getElement(this.element.id + '_chart_legend_translate_g');
                        setTabIndex(groupElement.children[this.currentLegendIndex], groupElement.firstElementChild);
                    }
                }
                if (this.previousTargetId.indexOf('Series') === -1 && targetId.indexOf('Series') > -1) {
                    this.currentPointIndex = 0;
                    const seriesId = targetId.indexOf('TrendLine') > -1 ? trendlineElement.firstElementChild.id : seriesElement.firstElementChild.id;
                    targetId = getFocusedSeries(this, seriesId);
                    targetElement = getElement(targetId);
                }
                this.previousTargetId = targetId;
                if (targetId.indexOf('SeriesGroup') > -1) {
                    this.currentSeriesIndex = +targetId.match(/SeriesGroup(\d+)/)[1];
                    targetElement.removeAttribute('tabindex');
                    targetElement.blur();
                    if (targetElement.children[1].id.indexOf('_Point_') === -1) {
                        markerGroup = getElement(this.element.id + (targetElement.children[1].id.indexOf('_TrendLine_') > -1 ? 'TrendLineSymbolGroup' : 'SymbolGroup') + targetId.match(/SeriesGroup(\d+)/)[1]);
                    }
                    targetId = !sfBlazorToolkit.base.isNullOrUndefined(markerGroup) ? markerGroup.children[1].id : targetElement.children[1].id;
                    currentSeries = getCurrentSeries(this, targetId, this.currentSeriesIndex);
                    if (currentSeries.type !== 'MultiColoredLine') {
                        targetId = focusChild(!sfBlazorToolkit.base.isNullOrUndefined(markerGroup) ? markerGroup.children[1] : targetElement.children[1]);
                    }
                }
                currentSeries = getCurrentSeries(this, targetId, this.currentSeriesIndex);
                if (targetId.indexOf('Series') > -1) {
                    isNotSamePointsLength = checkPointsLength(currentSeries);
                }
                if (targetId.indexOf('Series') > -1 && currentSeries.type !== 'MultiColoredLine' && isNotSamePointsLength) {
                    this.currentPointIndex = parseInt(targetId.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
                }
                actionKey = this.enableHighlight || (this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable) || this.options.showTooltip || (targetId.indexOf('_Series_') > -1 && sfBlazorToolkit.base.isNullOrUndefined(markerGroup)) ? 'Tab' : '';
            }
            else if (e.code.indexOf('Arrow') > -1) {
                e.preventDefault();
                this.previousTargetId = targetId;
                if (targetId.indexOf('_chart_legend_page') > -1) {
                    if (e.code === 'ArrowLeft') {
                        getElement(this.element.id + '_chart_legend_pagedown').removeAttribute('tabindex');
                        focusChild(getElement(this.element.id + '_chart_legend_pageup'));
                    }
                    else if (e.code === 'ArrowRight') {
                        getElement(this.element.id + '_chart_legend_pageup').removeAttribute('tabindex');
                        focusChild(getElement(this.element.id + '_chart_legend_pagedown'));
                    }
                }
                else if ((targetId.indexOf('_chart_legend_') > -1)) {
                    const legendElement = targetElement.parentElement.children;
                    legendElement[this.currentLegendIndex].removeAttribute('tabindex');
                    this.currentLegendIndex += (e.code === 'ArrowUp' || e.code === 'ArrowRight') ? +1 : -1;
                    this.currentLegendIndex = getActualIndex(this.currentLegendIndex, legendElement.length);
                    const currentLegend = legendElement[this.currentLegendIndex];
                    focusChild(currentLegend);
                    targetId = currentLegend.children[1].id;
                    actionKey = this.enableHighlight || this.highlightMode !== 'None' ? 'ArrowMove' : '';
                }
                else if (targetId.indexOf('_Annotation_') > -1) {
                    const annotationElement = targetElement.parentElement.children;
                    annotationElement[this.currentAnnotationIndex].removeAttribute('tabindex');
                    this.currentAnnotationIndex += e.code === 'ArrowUp' || e.code === 'ArrowRight' ? +1 : -1;
                    this.currentAnnotationIndex = getActualIndex(this.currentAnnotationIndex, annotationElement.length);
                    const currentAnnotation = annotationElement[this.currentAnnotationIndex];
                    focusChild(currentAnnotation);
                    targetId = currentAnnotation.id;
                    actionKey = '';
                }
                else if (targetId.indexOf('_Series_') > -1) {
                    groupElement = targetElement.id.indexOf('_Series_') === -1 ? getElement(this.element.id + 'SeriesCollection') : targetElement.parentElement.parentElement;
                    let currentPoint = e.target;
                    targetElement.removeAttribute('tabindex');
                    targetElement.blur();
                    if (e.code === 'ArrowRight' || e.code === 'ArrowLeft') {
                        const seriesIndexes = [];
                        for (let i = 0; i < groupElement.children.length; i++) {
                            if (groupElement.children[parseInt(i.toString(), 10)].id.indexOf('SeriesGroup') > -1) {
                                seriesIndexes.push(+groupElement.children[parseInt(i.toString(), 10)].id.match(/SeriesGroup(\d+)/)[1]);
                            }
                        }
                        const previousSeriesIndex = seriesIndexes.indexOf(this.currentSeriesIndex);
                        this.currentSeriesIndex = seriesIndexes.indexOf(this.currentSeriesIndex) + (e.code === 'ArrowRight' ? 1 : -1);
                        this.currentSeriesIndex = seriesIndexes[getActualIndex(this.currentSeriesIndex, seriesIndexes.length)];
                        currentSeries = getCurrentSeries(this, targetId, this.currentSeriesIndex);
                        while (!currentSeries.focusable) {
                            this.currentSeriesIndex = seriesIndexes.indexOf(this.currentSeriesIndex) + (e.code === 'ArrowRight' ? 1 : -1);
                            this.currentSeriesIndex = seriesIndexes[getActualIndex(this.currentSeriesIndex, seriesIndexes.length)];
                            currentSeries = getCurrentSeries(this, targetId, this.currentSeriesIndex);
                        }
                        groupElement = getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSeriesGroup' : 'SeriesGroup') + this.currentSeriesIndex);
                        markerGroup = getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSymbolGroup' : 'SymbolGroup') + this.currentSeriesIndex);
                        const previousGroupElement = getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSeriesGroup' : 'SeriesGroup') + previousSeriesIndex);
                        const previousMarkerGroup = getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSymbolGroup' : 'SymbolGroup') + previousSeriesIndex);
                        pointIndex = parseInt(targetId.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
                        const previousIndex = this.seriesPathElement ? parseInt(this.seriesPathElement.id.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10) : 0;
                        const currentGroup = groupElement.children[1].id.indexOf('_Point_') === -1 && markerGroup ? markerGroup : groupElement;
                        const previousGroup = previousGroupElement && previousGroupElement.children[1].id.indexOf('_Point_') === -1 && previousMarkerGroup ? previousMarkerGroup : previousGroupElement;
                        isNotSamePointsLength = checkPointsLength(currentSeries);
                        if (!isNaN(pointIndex) && !isNaN(previousIndex)) {
                            if (previousGroup && currentGroup.childElementCount === previousGroup.childElementCount) {
                                this.currentPointIndex = getActualIndex(this.currentPointIndex, markerGroup ? markerGroup.childElementCount : groupElement.childElementCount);
                                currentPoint = groupElement.children[1].id.indexOf('_Point_') === -1 && markerGroup ? markerGroup.children[this.currentPointIndex + 1] : groupElement.children[this.currentPointIndex + 1];
                            }
                            else {
                                this.currentPointIndex = pointIndex;
                                currentPoint = getCurrentPointElement(this, currentGroup, e.code);
                            }
                        }
                        else {
                            currentPoint = groupElement.children[1].id.indexOf('_Point_') === -1 && markerGroup ? markerGroup.children[this.currentPointIndex + 1] : (currentSeries.type === 'MultiColoredArea' ? getElement(targetId) : groupElement.children[this.currentPointIndex + 1]);
                        }
                    }
                    else {
                        this.currentPointIndex += e.code === 'ArrowUp' ? 1 : -1;
                        pointIndex = parseInt(targetId.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
                        currentSeries = getCurrentSeries(this, targetId, this.currentSeriesIndex);
                        groupElement = getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSeriesGroup' : 'SeriesGroup') + this.currentSeriesIndex);
                        markerGroup = getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSymbolGroup' : 'SymbolGroup') + this.currentSeriesIndex);
                        const currentGroup = groupElement.children[1].id.indexOf('_Point_') === -1 && markerGroup ? markerGroup : groupElement;
                        isNotSamePointsLength = checkPointsLength(currentSeries);
                        if (!isNaN(pointIndex) && targetId.indexOf('_BoxPath') === -1 && (currentSeries.points.length !== currentGroup.children.length - 1 || isNotSamePointsLength)) {
                            currentPoint = getCurrentPointElement(this, currentGroup, e.code);
                        }
                        else {
                            if (targetId.indexOf('_Symbol') > -1) {
                                this.currentPointIndex = getActualIndex(this.currentPointIndex, getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSymbolGroup' : 'SymbolGroup') + this.currentSeriesIndex).childElementCount - 1);
                                currentPoint = targetId.indexOf('_TrendLine_') > -1 ? getElement(this.element.id + '_Series_' + this.currentSeriesIndex + '_TrendLine_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex + '_Symbol') : getElement(this.element.id + '_Series_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex + '_Symbol');
                            }
                            else if (targetId.indexOf('_Point_') > -1) {
                                this.currentPointIndex = getActualIndex(this.currentPointIndex, (targetId.indexOf('_BoxPath') > -1 ? (getElement(this.element.id + 'SeriesGroup' + this.currentSeriesIndex).childElementCount - 1) / 2 : getElement(this.element.id + 'SeriesGroup' + this.currentSeriesIndex).childElementCount - 1));
                                currentPoint = targetId.indexOf('_BoxPath') > -1 ? getElement(this.element.id + '_Series_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex + '_BoxPath') : getElement(this.element.id + '_Series_' + this.currentSeriesIndex + '_Point_' + this.currentPointIndex);
                            }
                        }
                    }
                    if (currentSeries.type === 'MultiColoredLine' || currentSeries.type === 'MultiColoredArea') {
                        targetId = !sfBlazorToolkit.base.isNullOrUndefined(currentPoint) ? currentPoint.id : targetId;
                    }
                    else {
                        targetId = !sfBlazorToolkit.base.isNullOrUndefined(currentPoint) ? focusChild(currentPoint) : (targetId.indexOf('_TrendLine_') > -1 ? focusChild(getElement(this.element.id + '_Series_' + this.currentSeriesIndex + '_TrendLine_' + this.currentSeriesIndex)) :
                            focusChild(!sfBlazorToolkit.base.isNullOrUndefined(getElement(this.element.id + '_Series_' + this.currentSeriesIndex)) ? getElement(this.element.id + '_Series_' + this.currentSeriesIndex) : getRectSeriesElement(this, currentSeries)));
                    }
                    actionKey = (this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable) || this.options.showTooltip || sfBlazorToolkit.base.isNullOrUndefined(markerGroup) ? 'ArrowMove' : '';
                }
            }
            else if ((e.code === 'Enter' || e.code === 'Space') && ((targetId.indexOf('_chart_legend_') > -1) ||
                (targetId.indexOf('_Point_') > -1))) {
                targetId = (targetId.indexOf('_chart_legend_page') > -1) ? targetId : ((targetId.indexOf('_chart_legend_') > -1) ?
                    targetElement.children[1].id : targetId);
                actionKey = 'Enter';
            }
        }
        if (zoomInElement) {
            zoomInElement.setAttribute('class', 'e-chart-focused');
        }
        if (zoomOutElement) {
            zoomOutElement.setAttribute('class', 'e-chart-focused');
        }
        if (resetElement) {
            resetElement.setAttribute('class', 'e-chart-focused');
        }
        if (targetId.indexOf('_Zooming_') > -1) {
            if (this.zoomBase.zoomingModule) {
                if (e.code === 'Enter' || e.code === 'Space') {
                    this.zoomBase.zoomingModule.isZoomed = this.zoomBase.zoomingModule.performedUI = true;
                    this.zoomBase.zoomingModule.isPanning = this.zoomBase.isChartDrag = false;
                    if (targetId.indexOf('_Zooming_ZoomIn') > -1) {
                        zoomToolkitZoomIn(this, target.getAttribute('opacity'));
                        actionKey = 'Enter';
                    }
                    else if (targetId.indexOf('_Zooming_ZoomOut') > -1) {
                        zoomToolkitZoomOut(this, target.getAttribute('opacity'));
                        actionKey = 'Enter';
                    }
                    else if (targetId.indexOf('_Zooming_Reset') > -1) {
                        zoomToolkitReset(this);
                        actionKey = 'Enter';
                    }
                    performZoomRedraw(this, false);
                }
            }
        }
        if (actionKey !== '') {
            if (actionKey === 'ArrowMove' || actionKey === 'Tab') {
                if (targetId.indexOf('_Point_') > -1 || targetId.indexOf('_Series_') > -1 || targetId.indexOf('_ChartSegmentClipRect_') > -1) {
                    const seriesIndex = targetId.indexOf('_ChartSegmentClipRect_') > -1 ? parseInt(targetId.split('_ChartSegmentClipRect_')[1], 10) : parseInt(targetId.split('_Series_')[1].split('_Point_')[0], 10);
                    pointIndex = targetId.indexOf('_ChartSegmentClipRect_') > -1 ? NaN : parseInt(targetId.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
                    currentSeries = getCurrentSeries(this, targetId, seriesIndex);
                    if (isNaN(pointIndex) || (currentSeries.type === 'MultiColoredLine' && (targetId.indexOf('_Series_') > -1))) {
                        isNotSamePointsLength = checkPointsLength(currentSeries);
                        if (isNotSamePointsLength) {
                            this.currentPointIndex = (e.code === 'ArrowUp' || e.code === 'ArrowDown') ? this.currentPointIndex : (!sfBlazorToolkit.base.isNullOrUndefined(this.seriesPathElement) ? parseInt(this.seriesPathElement.id.split('_Series_')[1].replace('_Symbol', '').split('_PointIndex_')[1], 10) : currentSeries.points[0].iX);
                            pointIndex = getCurrentPointIndex(this, currentSeries, e.code);
                        }
                        else {
                            this.currentPointIndex = getActualIndex(this.currentPointIndex, currentSeries.points.length);
                            pointIndex = this.currentPointIndex;
                        }
                        targetId = targetId.indexOf('_TrendLine_') > -1 ? this.element.id + '_Series_' + seriesIndex + '_TrendLine_' + seriesIndex + '_PointIndex_' + this.currentPointIndex : this.element.id + '_Series_' + seriesIndex + '_PointIndex_' + this.currentPointIndex;
                        if (!sfBlazorToolkit.base.isNullOrUndefined(this.seriesPathElement)) {
                            removeElement(this.seriesPathElement.id);
                            removeElement(this.element.id + '_Series_' + parseInt(this.seriesPathElement.id.split('_Series_')[1].split('_PointIndex_')[0], 10) + '_Point_' + parseInt(this.seriesPathElement.id.split('_Series_')[1].replace('_Symbol', '').split('_PointIndex_')[1], 10) + '_Trackball_1');
                            this.seriesPathElement = null;
                        }
                        if (currentSeries.points[parseInt(pointIndex.toString(), 10)]) {
                            this.seriesPathElement = this.userInteractionBase.svgRenderer.drawPath({
                                'id': targetId,
                                'stroke-width': 2,
                                'fill': 'transparent',
                                'opacity': 1,
                                'stroke': 'transparent',
                                'd': 'M' + ' ' + currentSeries.points[parseInt(pointIndex.toString(), 10)].s[0].x + ' ' + currentSeries.points[parseInt(pointIndex.toString(), 10)].s[0].y
                            });
                            getElement(this.element.id + (targetId.indexOf('_TrendLine_') > -1 ? 'TrendLineSeriesGroup' : 'SeriesGroup') + seriesIndex).appendChild(this.seriesPathElement);
                            focusTarget(targetId);
                            const firstChildElement = targetId.indexOf('_TrendLine_') > -1 ? getElement(this.element.id + '_Series_' + seriesIndex + '_TrendLine_' + seriesIndex) : getElement(this.element.id + '_Series_' + seriesIndex);
                            if (firstChildElement) {
                                const className = firstChildElement.getAttribute('class');
                                const tabIndex = firstChildElement.getAttribute('tabindex');
                                if (className && tabIndex) {
                                    firstChildElement.removeAttribute('class');
                                    firstChildElement.removeAttribute('tabindex');
                                }
                            }
                            dotnetref.invokeMethodAsync('GetAccessibilityText', seriesIndex, (currentSeries.type.indexOf('Spline') > -1 && currentSeries.emptyPointMode === 'Drop') ? pointIndex : this.currentPointIndex, targetId);
                        }
                    }
                    const index = parseInt(targetId.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
                    if ((currentSeries.emptyPointMode === 'Gap' || (currentSeries.type.indexOf('Spline') === -1 && currentSeries.emptyPointMode === 'Drop')) && isNotSamePointsLength && !isNaN(index)) {
                        for (let i = 0; i < currentSeries.points.length; i++) {
                            if (pointIndex === currentSeries.points[parseInt(i.toString(), 10)].iX) {
                                pointIndex = i;
                                i = currentSeries.points.length;
                            }
                        }
                    }
                    let pointRegion;
                    if (currentSeries.points.length > 0) {
                        pointRegion = isRectSeries(currentSeries.type) ? currentSeries.points[parseInt(pointIndex.toString(), 10)].r[0] : currentSeries.points[parseInt(pointIndex.toString(), 10)].s[0];
                        this.mouseX = isRectSeries(currentSeries.type) ? Math.ceil(pointRegion.x + currentSeries.clipRect.x) : pointRegion.x + currentSeries.clipRect.x;
                        this.mouseY = isRectSeries(currentSeries.type) ? Math.ceil(pointRegion.y + currentSeries.clipRect.y) : pointRegion.y + currentSeries.clipRect.y;
                    }
                    else if (!currentSeries.enableTooltip && this.tooltipBase.tooltipModule.shared) {
                        this.userInteractionBase.visibleSeries.forEach((series) => {
                            if (series.points.length > 0) {
                                pointRegion = isRectSeries(series.type) ? series.points[parseInt(pointIndex.toString(), 10)].r[0] : series.points[parseInt(pointIndex.toString(), 10)].s[0];
                                this.mouseX = isRectSeries(series.type) ? Math.ceil(pointRegion.x + series.clipRect.x) : pointRegion.x + series.clipRect.x;
                                this.mouseY = isRectSeries(series.type) ? Math.ceil(pointRegion.y + series.clipRect.y) : pointRegion.y + series.clipRect.y;
                                return;
                            }
                        });
                    }
                    tooltipMousemovehandler(this);
                    crosshairMousemoveHandler(this);
                    if (this.markerExplodeBase.markerExploded) {
                        markerMove(this, false);
                    }
                }
                else {
                    removeTooltip(1, this);
                    removeCrosshair(this, 1);
                    removeMarker(this);
                }
            }
            if (actionKey === 'ESC') {
                removeTooltip(1, this);
                removeCrosshair(this, 1);
                removeMarker(this);
            }
            this.processPointSelection(dotnetref, id, targetId, actionKey);
            if (targetId.indexOf('_Series_') > -1 && this.highlightDataIndexes.length > 0) {
                removeSelectionStyles(this.dataId);
            }
            dotnetref.invokeMethodAsync('OnChartKeyboardNavigationsAsync', actionKey, targetId);
        }
        if (targetId.indexOf('_Series_') > -1 && this.highlightDataIndexes.length > 0) {
            removeSelectionStyles(this.dataId);
        }
        return false;
    };
    SfChart.prototype.mouseLeave = function (dotnetref, id, e) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.tooltipBase.tooltipModule)) {
            removeTooltip(this.tooltipBase.tooltipModule.fadeOutDuration, this);
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.crosshairBase.crosshair)) {
            removeCrosshair(this, 1000);
        }
        this.dotnetref = dotnetref;
        if (this.zoomBase.zoomingModule) {
            this.zoomBase.isChartDrag = this.userInteractionBase.isPointMouseDown = false;
            mouseCancelHandler(this);
        }
        if (this.highlightDataIndexes.length > 0) {
            removeSelectionStyles(this.dataId);
        }
        dotnetref.invokeMethodAsync('OnChartMouseLeave', this.getEventArgs(e, id));
        return false;
    };
    SfChart.prototype.longPress = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        const clientX = e && e.originalEvent.changedTouches ? e.originalEvent.changedTouches[0].clientX : 0;
        const clientY = e && e.originalEvent.changedTouches ? e.originalEvent.changedTouches[0].clientY : 0;
        const target = e && e.originalEvent ? e.originalEvent.target : null;
        if (target && target.id.indexOf('_Zooming_') > -1) {
            return false;
        }
        this.setMouseXY(clientX, clientY, id);
        this.userInteractionBase.startMove = true;
        if (e && e.originalEvent.type.indexOf('touch') > -1 && this.zoomBase && this.zoomBase.zoomingModule) {
            this.zoomBase.isSkipZooming = true;
            this.zoomBase.isChartDrag = false;
            this.userInteractionBase.disableTrackTooltip = false;
        }
        if (svgbase.withInAreaBounds(this.mouseX, this.mouseY, this.userInteractionBase.axisClipRect)) {
            markerMove(this, false);
            if (this.tooltipBase.tooltipModule.enable) {
                tooltip(this);
            }
            if (this.crosshairBase.crosshair && this.crosshairBase.crosshair.enable) {
                createCrosshair(this);
            }
        }
        this.dotnetref.invokeMethodAsync('OnChartLongPress');
        return false;
    };
    SfChart.prototype.getAxisScrollbar = function (id, axes) {
        const axisNames = axes.map((axis) => axis.name);
        for (const axisName of axisNames) {
            if (typeof id === 'string' && id.indexOf(axisName) !== -1) {
                const matchedAxes = axes.filter((axis) => axis.name === axisName);
                return matchedAxes.length > 0 ? matchedAxes[0] : null;
            }
        }
        return null;
    };
    SfChart.prototype.getScrollbarId = function (chart, event) {
        let clientX;
        let clientY;
        if (event instanceof MouseEvent) {
            clientX = event.clientX;
            clientY = event.clientY;
        }
        else if (event instanceof TouchEvent) {
            const touch = event.touches[0];
            clientX = touch.clientX;
            clientY = touch.clientY;
        }
        else {
            return null;
        }
        for (const id of chart.scrollBarsId) {
            const svgElement = document.getElementById(id);
            if (!svgElement) {
                continue;
            }
            const rect = svgElement.getBoundingClientRect();
            if (clientX >= rect.left && clientX <= rect.right && clientY >= rect.top && clientY <= rect.bottom) {
                const elementAtPoint = document.elementFromPoint(clientX, clientY);
                if (elementAtPoint && svgElement.contains(elementAtPoint)) {
                    return elementAtPoint.id;
                }
            }
        }
        return null;
    };
    SfChart.prototype.scrollDown = function (dotnetref, id, event) {
        if (!(this.isZooming || this.isScrollbar)) {
            return false;
        }
        let targetId = event.target.id;
        const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
        if (!chart) {
            return false;
        }
        targetId = targetId.indexOf('scrollBar') === -1 ? this.getScrollbarId(chart, event) : targetId;
        if (targetId && targetId.indexOf('scrollBar') > -1 && !sfBlazorToolkit.base.isNullOrUndefined(chart) && !chart.isDisposed) {
            const scrollbar = chart.scrollbarBase;
            scrollbar.targetId = targetId;
            const scrollbarAxis = this.getAxisScrollbar(targetId, scrollbar.axes);
            if (!sfBlazorToolkit.base.isNullOrUndefined(scrollbarAxis)) {
                scrollbar.axis = chart.selectedScrollAxis = scrollbarAxis;
                const scrollbarOptions = scrollbar.scrollbarOptions[scrollbarAxis.name];
                scrollbarOptions.isVertical = scrollbarAxis.orientation === 'Vertical';
                const isInverse = scrollbarAxis.isAxisInverse;
                const eventArgs = getScrollEventArgs(event, [chart.id, chart.id + '_scrollBar_svg' + scrollbarAxis.name]);
                this.mouseX = eventArgs['mouseX'];
                this.mouseY = eventArgs['mouseY'];
                if (event.type.indexOf('touch') > -1) {
                    let clientX;
                    let clientY;
                    if (event instanceof TouchEvent) {
                        clientX = event.changedTouches[0].clientX;
                        clientY = event.changedTouches[0].clientY;
                    }
                    else if (event instanceof PointerEvent && event.pointerType === 'touch') {
                        clientX = event.clientX;
                        clientY = event.clientY;
                    }
                    targetId = this.getNearScrollCircleElement(clientX, clientY, [scrollbarOptions.leftCircleEle, scrollbarOptions.rightCircleEle], targetId);
                }
                scrollbar.isResizeLeft = isExist(targetId, '_leftCircle_') || isExist(targetId, '_leftArrow_');
                scrollbar.isResizeRight = isExist(targetId, '_rightCircle_') || isExist(targetId, '_rightArrow_');
                scrollbar.previousXY = (scrollbarOptions.isVertical && isInverse) ? this.mouseY : scrollbarOptions.isVertical ? scrollbarOptions.width -
                    this.mouseY : isInverse ? scrollbarOptions.width - this.mouseX : this.mouseX;
                scrollbar.previousWidth = scrollbarOptions.thumbRectWidth;
                scrollbar.previousRectX = scrollbarOptions.thumbRectX;
                scrollbar.startZoomPosition = scrollbarAxis.zoomPosition;
                scrollbar.startZoomFactor = scrollbarAxis.zoomFactor;
                scrollbar.startRange = scrollbarAxis.visibleRange;
                scrollbar.scrollStarted = true;
                if (scrollbar.isScrollEventCalled) {
                    chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('ScrollStart', scrollbarAxis.name, scrollbarAxis.zoomPosition, scrollbarAxis.zoomFactor, null));
                }
                if (isExist(targetId, 'scrollBarThumb_') || isExist(targetId, 'gripCircle')) {
                    scrollbar.isThumbDrag = true;
                    if (scrollbar.axis.scrollbarSettings.height >= 12) {
                        scrollbarOptions.svgObject.style.cursor = '-webkit-grabbing';
                    }
                }
                else if (isExist(targetId, 'scrollBarBackRect_')) {
                    const currentX = moveLength(scrollbar);
                    scrollbarOptions.thumbRectX = isWithIn(currentX, scrollbar) ? currentX : scrollbarOptions.thumbRectX;
                    positionThumb(scrollbarOptions.thumbRectX, scrollbarOptions.thumbRectWidth, scrollbar);
                    setZoomFactorPosition(scrollbar, scrollbarOptions.thumbRectX, scrollbarOptions.thumbRectWidth, false);
                    if (scrollbarOptions.isLazyLoad) {
                        const thumbMove = scrollbarOptions.thumbRectX > scrollbar.previousRectX ? 'RightMove' : 'LeftMove';
                        const args = calculateLazyRange(scrollbar, thumbMove);
                        if (args) {
                            if (scrollbar.isScrollEventCalled) {
                                chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('ScrollStart', scrollbarAxis.name, scrollbarAxis.zoomPosition, scrollbarAxis.zoomFactor, args.currentRange));
                            }
                        }
                    }
                }
            }
        }
        return false;
    };
    SfChart.prototype.getNearScrollCircleElement = function (x, y, scrollbarCircleElements, targetId, threshold = 10) {
        if (!scrollbarCircleElements || scrollbarCircleElements.length === 0) {
            return targetId;
        }
        for (const scrollbarElement of scrollbarCircleElements) {
            const rect = scrollbarElement.getBoundingClientRect();
            const centerX = rect.left + rect.width / 2;
            const centerY = rect.top + rect.height / 2;
            const distance = Math.sqrt(Math.pow(x - centerX, 2) + Math.pow(y - centerY, 2));
            if (distance <= threshold) {
                return scrollbarElement.id;
            }
        }
        return targetId;
    };
    SfChart.prototype.domMouseMove = function (dotnetref, id, event) {
        if (!(this.isZooming || this.isScrollbar)) {
            return false;
        }
        let targetId = event.target.id;
        const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
        if (!chart) {
            return false;
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(chart) && !chart.isDisposed && !sfBlazorToolkit.base.isNullOrUndefined(targetId) && targetId.indexOf(chart.id) > -1) {
            const scrollbar = chart.scrollbarBase;
            targetId = !sfBlazorToolkit.base.isNullOrUndefined(scrollbar) && scrollbar.scrollStarted ? scrollbar.targetId : targetId;
            if (targetId.indexOf('scrollBar') > -1 || !sfBlazorToolkit.base.isNullOrUndefined(chart.selectedScrollAxis)) {
                const scrollbarAxis = !sfBlazorToolkit.base.isNullOrUndefined(chart.selectedScrollAxis) ? chart.selectedScrollAxis : this.getAxisScrollbar(targetId, scrollbar.axes);
                if (sfBlazorToolkit.base.isNullOrUndefined(scrollbarAxis)) {
                    return null;
                }
                const scrollbarOptions = scrollbar.scrollbarOptions[scrollbarAxis.name];
                const isInverse = scrollbarAxis.isAxisInverse;
                if (sfBlazorToolkit.base.isNullOrUndefined(scrollbarOptions) || (scrollbarOptions && !document.getElementById(scrollbarOptions.svgObject.id))) {
                    return null;
                }
                const eventArgs = getScrollEventArgs(event, [chart.id, chart.id + '_scrollBar_svg' + scrollbarAxis.name]);
                this.mouseX = eventArgs['mouseX'];
                this.mouseY = eventArgs['mouseY'];
                if (!scrollbar.isThumbDrag) {
                    setCursor(event.target, this.dataId, scrollbarAxis.name);
                }
                scrollbarOptions.enableZoom = scrollbarAxis.scrollbarSettings.enableZoom;
                setTheme(event.target, this.dataId, scrollbarAxis.name);
                let mouseXY = (scrollbarOptions.isVertical && isInverse) ? scrollbarOptions.width - this.mouseY : scrollbarOptions.isVertical ?
                    this.mouseY : this.mouseX;
                const zoomPosition = scrollbarOptions.zoomPosition;
                const zoomFactor = scrollbarOptions.zoomFactor;
                const moveLength = scrollbar.previousRectX - scrollbarOptions.thumbRectX;
                const thumbMove = moveLength < 0 ? 'RightMove' : 'LeftMove';
                let args;
                if (scrollbarOptions.isLazyLoad && (scrollbar.isThumbDrag || scrollbar.isResizeLeft || scrollbar.isResizeRight)) {
                    args = calculateLazyRange(scrollbar, thumbMove);
                }
                const currentRange = args ? args.currentRange : null;
                if (scrollbar.isThumbDrag) {
                    scrollbar.isScrolling = scrollbar.isThumbDrag;
                    mouseXY = (scrollbarOptions.isVertical || isInverse) ? scrollbarOptions.width - mouseXY : mouseXY;
                    const currentX = scrollbarOptions.thumbRectX + (mouseXY - scrollbar.previousXY);
                    if (mouseXY >= currentX + scrollbarOptions.thumbRectWidth) {
                        setCursor(event.target, this.dataId, scrollbarAxis.name);
                    }
                    else if (scrollbar.axis.scrollbarSettings.height >= 12) {
                        scrollbarOptions.svgObject.style.cursor = '-webkit-grabbing';
                    }
                    if (mouseXY >= 0 && mouseXY <= currentX + scrollbarOptions.thumbRectWidth) {
                        scrollbarOptions.thumbRectX = isWithIn(currentX, scrollbar) ? currentX : scrollbarOptions.thumbRectX;
                        positionThumb(scrollbarOptions.thumbRectX, scrollbarOptions.thumbRectWidth, scrollbar);
                        scrollbar.previousXY = mouseXY;
                        setZoomFactorPosition(scrollbar, currentX, scrollbarOptions.thumbRectWidth, false);
                    }
                    if (scrollbar.isScrollEventCalled) {
                        chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollChanged', scrollbarAxis.name, zoomPosition, zoomFactor, currentRange));
                    }
                }
                else if (scrollbar.isResizeLeft || scrollbar.isResizeRight) {
                    resizeThumb(scrollbar, this.mouseX, this.mouseY);
                    if (scrollbar.isScrollEventCalled) {
                        chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollChanged', scrollbarAxis.name, zoomPosition, zoomFactor, currentRange));
                    }
                }
            }
        }
        return false;
    };
    SfChart.prototype.domMouseUp = function (dotnetref, id, event) {
        if (!(this.isZooming || this.isScrollbar)) {
            return false;
        }
        let targetId = event.target.id;
        const chart = window.sfBlazorToolkit.base.getCompInstance(this.dataId);
        if (!chart) {
            return false;
        }
        const scrollbar = chart.scrollbarBase;
        if (sfBlazorToolkit.base.isNullOrUndefined(scrollbar)) {
            return false;
        }
        targetId = (!sfBlazorToolkit.base.isNullOrUndefined(scrollbar) && scrollbar.scrollStarted) ? scrollbar.targetId : targetId;
        if (targetId.indexOf('scrollBar') > -1 && !sfBlazorToolkit.base.isNullOrUndefined(scrollbar.scrollbarOptions[scrollbar.axis.name])) {
            let args;
            const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
            scrollbarOptions.startX = scrollbarOptions.thumbRectX;
            const scrollbarAxis = this.getAxisScrollbar(targetId, scrollbar.axes);
            const circleRadius = scrollbarOptions.height / 2;
            const circleWidth = 1;
            const currentScrollWidth = scrollbarOptions.startX + scrollbarOptions.thumbRectWidth + circleRadius + circleWidth;
            const currentZPWidth = circleRadius + (circleWidth / 2);
            if ((scrollbar.isResizeLeft || scrollbar.isResizeRight) && !scrollbarOptions.isLazyLoad) {
                scrollbarAxis.zoomFactor = (currentScrollWidth >= scrollbarOptions.width - 1 && (scrollbarOptions.startX - currentZPWidth) <= 0) ? 1 : scrollbarOptions.zoomFactor;
            }
            if (scrollbarOptions.isLazyLoad) {
                const moveLength = scrollbar.previousRectX - scrollbarOptions.startX;
                if ((moveLength > 0 || moveLength < 0) && scrollbar.isThumbDrag) {
                    const thumbMove = moveLength < 0 ? 'RightMove' : 'LeftMove';
                    if (thumbMove === 'RightMove') {
                        scrollbarOptions.startX = (scrollbarOptions.startX + Math.abs(moveLength)) < scrollbarOptions.width - circleRadius ? scrollbarOptions.startX :
                            scrollbarOptions.width - circleRadius - scrollbarOptions.thumbRectWidth;
                    }
                    else {
                        scrollbarOptions.startX = (scrollbarOptions.startX + scrollbarOptions.thumbRectWidth - Math.abs(moveLength)) > circleRadius ?
                            scrollbarOptions.startX : circleRadius;
                    }
                    args = calculateLazyRange(scrollbar, thumbMove);
                    if (args) {
                        if (scrollbar.isScrollEventCalled) {
                            chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollEnd', scrollbarAxis.name, scrollbarAxis.zoomPosition, scrollbarAxis.zoomFactor, args.currentRange));
                        }
                        scrollbar.targetId = targetId;
                        scrollbar.scrollStarted = false;
                    }
                }
                if (scrollbar.isResizeLeft || scrollbar.isResizeRight) {
                    args = calculateLazyRange(scrollbar);
                    if (args) {
                        if (scrollbar.isScrollEventCalled) {
                            chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollEnd', scrollbarAxis.name, scrollbarAxis.zoomPosition, scrollbarAxis.zoomFactor, args.currentRange));
                        }
                        scrollbar.targetId = targetId;
                        scrollbar.scrollStarted = false;
                    }
                }
            }
            scrollbar.isThumbDrag = false;
            scrollbar.isScrollEnd = false;
            scrollbar.isScrolling = false;
            if (scrollbar.scrollStarted && !scrollbarOptions.isLazyLoad) {
                if (scrollbar.isScrollEventCalled) {
                    chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollChanged', scrollbarAxis.name, scrollbarAxis.zoomPosition, scrollbarAxis.zoomFactor, args ? args.currentRange : null));
                }
                scrollbar.targetId = targetId;
                scrollbar.scrollStarted = false;
            }
            chart.dotnetref.invokeMethodAsync('UpdateChartData');
        }
        scrollbar.isResizeLeft = false;
        scrollbar.isResizeRight = false;
        chart.selectedScrollAxis = null;
        return false;
    };
    return SfChart;
}());

class Index {
    constructor(seriesIndex, pointIndex) {
        this.series = seriesIndex;
        this.point = pointIndex;
    }
}
class IChartInternalLocation {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }
}
class IColorValue {
    constructor(r, g, b) {
        this.r = r;
        this.g = g;
        this.b = b;
    }
}

class ITooltipBase {
    constructor() {
        this.currentPoints = [];
        this.previousPoints = [];
        this.valueX = 0;
        this.valueY = 0;
        this.tooltipTempList = [];
    }
}
class IMarkerExplodeBase {
    constructor() {
        this.markerCurrentPoints = [];
        this.markerPreviousPoints = [];
        this.trackBallClass = 'EJ2-TrackBall';
    }
}
class ICrosshairBase {
    constructor() {
        this.crosshairX = 0;
        this.crosshairY = 0;
        this.rx = 2;
        this.ry = 2;
        this.crossHighlightWidth = 0;
        this.leftOverflow = 0;
        this.rightOverflow = 0;
    }
}
class IUserInteractionBase {
    constructor() {
        this.lierIndex = 0;
        this.chartStartMove = true;
        this.axes = [];
        this.isFirstRendered = true;
        this.toolbarHeight = 43;
    }
}
class IBrowser {
    constructor(name, isPointer, isDevice, isTouch, isIos) {
        this.browserName = name;
        this.isPointer = isPointer;
        this.isDevice = isDevice;
        this.isTouch = isTouch;
        this.isIos = isIos;
    }
}
class RectOption extends svgbase.PathOption {
    constructor(id, fill, border, opacity, rect, rx, ry, transform, dashArray) {
        super(id, fill, border.width, border.color, opacity, dashArray);
        this.y = rect.y;
        this.x = rect.x;
        this.height = rect.height;
        this.width = rect.width;
        this.rx = rx ? rx : 0;
        this.ry = ry ? ry : 0;
        this.transform = transform ? transform : '';
        this.stroke = (border.width !== 0 && this.stroke !== '') ? border.color : 'transparent';
    }
}
class IZoom {
    constructor(sfChart) {
        this.isZoomStart = true;
        this.touchStartList = [];
        this.touchMoveList = [];
        this.chart = sfChart;
        this.browser = new IBrowser(sfBlazorToolkit.base.Browser.info.name, sfBlazorToolkit.base.Browser.isPointer, sfBlazorToolkit.base.Browser.isDevice, sfBlazorToolkit.base.Browser.isTouch, sfBlazorToolkit.base.Browser.isIos || sfBlazorToolkit.base.Browser.isIos7);
        this.isDevice = this.browser.isDevice;
        this.zooming = sfChart.zoomBase.zoomSettings;
        this.elementId = sfChart.element.id;
        this.zoomAxes = [];
        this.zoomingRect = new svgbase.Rect(0, 0, 0, 0);
        this.isZoomed = this.performedUI = this.zooming.enablePan && this.zooming.enableSelectionZooming;
        this.isPanning = this.zooming.isPanning;
    }
}
class IZoomToolkitBase {
    constructor() {
        this.iconRectOverFill = 'transparent';
        this.iconRectSelectionFill = 'transparent';
    }
}
class IZoomBase {
    constructor() {
        this.axisCollections = [];
        this.previousMouseMoveX = 0;
        this.previousMouseMoveY = 0;
        this.mouseDownX = 0;
        this.mouseDownY = 0;
        this.clipRectId = '_ChartAreaClipRect__Rect';
        this.zoomToolkitId = '_Zooming_KitCollection';
        this.zoomToolkitZoom = '_Zooming_Zoom';
        this.zoomToolkitZoomIn = '_Zooming_ZoomIn';
        this.zoomToolkitZoomOut = '_Zooming_ZoomOut';
        this.zoomToolkitPan = '_Zooming_Pan';
        this.zoomToolkitReset = '_Zooming_Reset';
        this.chartZoomTip = 'EJ2_Chart_ZoomTip';
    }
}
class CircleOption extends svgbase.PathOption {
    constructor(id, fill, border, opacity, cx, cy, r) {
        super(id, fill, border.width, border.color, opacity);
        this.cy = cy;
        this.cx = cx;
        this.r = r;
    }
}

let dataId = '';
let dotnetref = {};
let svgId = null;
let dotnetrefCollection = [];
let resizeBound = {};
let resize = {};
let currentId = null;
let isHighlightRemoved = false;
let tooltipModule = null;
let crosshair = null;
let markerExploded = false;
let isDragSelection = false;
let isSeriesMode = false;

export function initialize(_dataId, element, dotnetRef, isZooming, isScrollbar, options, selectionHighlightOptions, disableTouchAction, isStripLineTooltip, isMouseMoveEvent, dataLabelTemplateId) {
    const instance = new SfChart(_dataId, element.id, element, dotnetRef, isZooming, isScrollbar, options, selectionHighlightOptions, isStripLineTooltip, isMouseMoveEvent);
    dataId = _dataId;
    if (element) {
        const touchValue = disableTouchAction ? 'none' : 'element';
        sfBlazorToolkit.base.setStyleAttribute(element, {
            touchAction: touchValue,
            msTouchAction: touchValue,
            msContentZooming: 'none',
            msUserSelect: 'none',
            webkitUserSelect: 'none',
            webkitTouchCallout: 'none',
            position: 'relative',
            display: 'block',
            overflow: 'hidden'
        });

        const pinchElement = document.createElement('div');
        pinchElement.id = element.id + '_Pinch_target';

        sfBlazorToolkit.base.setStyleAttribute(pinchElement, {
            opacity: '0',
            position: 'absolute',
            display: 'block',
            width: '100px',
            height: '100px',
            background: 'transparent',
            top: '-100px',
            left: '-100px'
        });

        element.prepend(pinchElement);

        const secondaryElement = document.getElementById(_dataId + "_Secondary_Element");
        if (secondaryElement) {
            sfBlazorToolkit.base.setStyleAttribute(secondaryElement, { position: 'relative' });
        }

        const ejSVGTooltipElements = element.querySelectorAll('.ejSVGTooltip');
        ejSVGTooltipElements.forEach((ejSVGTooltip) => {
            sfBlazorToolkit.base.setStyleAttribute(ejSVGTooltip, { position: 'absolute', zIndex: '1', pointerEvents: 'none' });
        });
    }
    instance.render();
}

export function destroy(dataId) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.destroy();
    }
}

export function setTooltipStyle(dataId) {
    const tooltipDataElement = document.getElementById(dataId)?.querySelector("#tooltip_template");
    if (tooltipDataElement) {
        sfBlazorToolkit.base.setStyleAttribute(tooltipDataElement, { opacity: 0 });
    }
}

export function updateAnnotationStyle(annotationId, locationY, locationX, annotationVisibility) {
    const annotationElement = document.getElementById(annotationId);
    if (annotationElement) {
        sfBlazorToolkit.base.setStyleAttribute(annotationElement, {
            transform: 'translate(-50%, -50%)',
            position: 'absolute',
            zIndex: 1,
            top: locationY + 'px',
            left: locationX + 'px',
            visibility: annotationVisibility ? 'visible' : 'hidden'
        });
    }
}

export function getArgs(eventName, axisName, zoomPosition, zoomFactor, currentRanges) {
    return {
        name: eventName,
        axisName: axisName,
        zoomPosition: isFinite(zoomPosition) ? zoomPosition : 1,
        zoomFactor: isFinite(zoomFactor) ? zoomFactor : 1,
        currentRangeMax: currentRanges ? currentRanges.maximum.toString() : '',
        currentRangeMin: currentRanges ? currentRanges.minimum.toString() : ''
    };
}

export function isExist(id, match) {
    return id.indexOf(match) > -1;
}

export function moveLength(scrollbar) {
    const mouseXY = scrollbar.previousXY;
    const thumbX = scrollbar.previousRectX;
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
    const circleRadius = scrollbarOptions.height / 2;
    let moveLength = (10 / 100) * (scrollbarOptions.width - circleRadius * 2);
    if (mouseXY < thumbX) {
        moveLength = thumbX - (thumbX - moveLength > circleRadius ? moveLength : circleRadius);
    }
    else {
        moveLength = thumbX + (thumbX + scrollbarOptions.thumbRectWidth + moveLength < scrollbarOptions.width - circleRadius ? moveLength : circleRadius);
    }
    return moveLength;
}

export function isWithIn(currentX, scrollbar) {
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
    const circleRadius = scrollbarOptions.height / 2;
    return (currentX - circleRadius >= 0 && currentX + scrollbarOptions.thumbRectWidth + circleRadius <= scrollbarOptions.width);
}

export function positionThumb(currentX, currentWidth, scrollbar) {
    const gripWidth = 14;
    const gripCircleDiameter = 2;
    const padding = gripWidth / 2 - gripCircleDiameter;
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
    const height = scrollbar.axis.scrollbarSettings.height;
    const verticalTranslateHeight = (height / 2) + (padding / 2) - 0.5;
    const horizontalTranslateHeight = (height / 2) - (padding / 2) - 0.5;
    scrollbarOptions.slider.setAttribute('x', currentX.toString());
    scrollbarOptions.slider.setAttribute('width', currentWidth.toString());
    if (scrollbarOptions.enableZoom) {
        scrollbarOptions.leftCircleEle.setAttribute('cx', currentX.toString());
        scrollbarOptions.rightCircleEle.setAttribute('cx', (currentX + currentWidth).toString());
        setArrowDirection(scrollbar);
    }
    scrollbarOptions.gripCircle.setAttribute('transform', 'translate(' + (currentX + currentWidth / 2 + ((scrollbarOptions.isVertical ? 1 : -1) * padding)) +
        ',' + (scrollbarOptions.isVertical ? verticalTranslateHeight : horizontalTranslateHeight) + ') rotate(' + (scrollbarOptions.isVertical ? '180' : '0') + ')');
}

export function setZoomFactorPosition(scrollbar, currentX, currentWidth, isRequire = true) {
    const axis = scrollbar.axis;
    const scrollbarOptions = scrollbar.scrollbarOptions[axis.name];
    scrollbarOptions.isScrollUI = true;
    const circleRadius = scrollbarOptions.height / 2;
    const circleWidth = 1;
    const currentScrollWidth = currentX + currentWidth + circleRadius + circleWidth;
    const currentZPWidth = circleRadius + (circleWidth / 2);
    const axisSize = scrollbarOptions.isVertical ? axis.rect.h : scrollbarOptions.width;
    scrollbarOptions.zoomFactor = (currentWidth + (currentScrollWidth >= scrollbarOptions.width ? circleRadius + circleWidth : 0)) / axisSize;
    axis.zoomFactor = isRequire && !scrollbarOptions.isLazyLoad ? scrollbarOptions.zoomFactor : axis.zoomFactor;
    scrollbarOptions.zoomPosition = currentScrollWidth > axisSize ? (1 - axis.zoomFactor) : currentX < (circleRadius + circleWidth) ? 0 :
        (currentX - (currentX - currentZPWidth <= 0 ? currentZPWidth : 0)) / axisSize;
    axis.zoomPosition = scrollbarOptions.zoomPosition < 0 ? 0 : scrollbarOptions.zoomPosition > 0.9 ? 1 : scrollbarOptions.zoomPosition;
    if (!scrollbarOptions.isLazyLoad) {
        dotnetref.invokeMethodAsync('ChartScrolled', axis.name, axis.zoomFactor, axis.zoomPosition);
    }
}

export function calculateLazyRange(scrollbar, thumbMove, delta) {
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
    const scrollThumbX = scrollbarOptions.thumbRectX;
    let currentScrollWidth = scrollbarOptions.thumbRectWidth;
    let zoomFactor;
    let zoomPosition;
    let currentStart;
    let currentEnd;
    let args;
    const range = scrollbarOptions.scrollRange;
    const previousRange = getStartEnd(scrollbarOptions.previousStart, scrollbarOptions.previousEnd, false, scrollbar);
    const circleRadius = scrollbarOptions.height / 2;
    const circleWidth = 16;
    if (scrollbar.isResizeRight || thumbMove === 'RightMove') {
        currentScrollWidth = scrollbar.isResizeRight ? currentScrollWidth + circleWidth : currentScrollWidth;
        zoomFactor = (currentScrollWidth) / scrollbarOptions.width;
        zoomPosition = thumbMove === 'RightMove' ? (scrollThumbX + circleRadius) / scrollbarOptions.width : scrollbar.axis.zoomPosition;
        currentStart = thumbMove === 'RightMove' ? (range.start + (zoomPosition * range.delta)) : scrollbarOptions.previousStart;
        currentEnd = currentStart + (zoomFactor * range.delta);
    }
    else if (scrollbar.isResizeLeft || thumbMove === 'LeftMove') {
        zoomPosition = (scrollThumbX - circleRadius) / scrollbarOptions.width;
        zoomFactor = currentScrollWidth / scrollbarOptions.width;
        currentStart = range.start + (zoomPosition * range.delta);
        currentStart = currentStart >= range.start ? currentStart : range.start;
        currentEnd = thumbMove === 'LeftMove' ? (currentStart + (zoomFactor * range.delta)) : scrollbarOptions.previousEnd;
    }
    else if (scrollbar.isThumbDrag || scrollbar.isScrollWheel) {
        zoomPosition = thumbMove === 'RightMove' || delta > 0 ? (scrollThumbX + circleRadius) / scrollbarOptions.width : (scrollThumbX - circleRadius) / scrollbarOptions.width;
        zoomFactor = (scrollbarOptions.thumbRectWidth) / scrollbarOptions.width;
        currentStart = range.start + (zoomPosition * range.delta);
        currentStart = currentStart >= range.start ? currentStart : range.start;
        currentEnd = currentStart + (zoomFactor * range.delta);
    }
    if (currentEnd) {
        args = {
            axis: scrollbar.axis, currentRange: getStartEnd(currentStart, currentEnd, true, scrollbar),
            previousAxisRange: previousRange
        };
    }
    return args;
}

export function getStartEnd(start, end, isCurrentStartEnd, scrollbar) {
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
    const valueType = scrollbarOptions.valueType;
    if ((valueType === 'DateTime' || valueType === 'DateTimeCategory') && isCurrentStartEnd) {
        scrollbarOptions.previousStart = start;
        scrollbarOptions.previousEnd = end;
    }
    else if (isCurrentStartEnd) {
        scrollbarOptions.previousStart = Math.round(start);
        scrollbarOptions.previousEnd = Math.ceil(end);
    }
    switch (valueType) {
        case 'Double':
        case 'Category':
        case 'Logarithmic':
            start = Math.round(start);
            end = Math.ceil(end);
            break;
        case 'DateTime':
        case 'DateTimeCategory':
            start = start;
            end = end;
            break;
    }
    return { minimum: start, maximum: end };
}

export function setCursor(target, dataId, axisName) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    const scrollbar = chart.scrollbarBase;
    if (scrollbar) {
        const id = target.id;
        if (id && scrollbar.scrollbarOptions[`${axisName}`]) {
            scrollbar.scrollbarOptions[`${axisName}`].svgObject.style.cursor = (id.indexOf('scrollBarThumb_') > -1 || id.indexOf('_gripCircle') > -1) && (scrollbar.axis.scrollbarSettings.height >= 12) ?
                '-webkit-grab' : (id.indexOf('Circle_') > -1 || id.indexOf('Arrow_') > -1) ? scrollbar.scrollbarOptions[`${axisName}`].isVertical ? 'ns-resize' :
                    'ew-resize' : 'auto';
        }
    }
}

export function setTheme(target, dataId, axisName) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    const scrollbar = chart.scrollbarBase;
    const scrollbarOptions = scrollbar.scrollbarOptions[`${axisName}`];
    if (scrollbarOptions && scrollbarOptions.enableZoom) {
        const id = target.id;
        const isLeftHover = id.indexOf('_leftCircle_') > -1 || id.indexOf('_leftArrow_') > -1;
        const isRightHover = id.indexOf('_rightCircle_') > -1 || id.indexOf('_rightArrow_') > -1;
        const style = scrollbar.scrollbarThemeStyle;
        const leftArrowEle = scrollbarOptions.leftArrowEle;
        const leftCircleEle = scrollbarOptions.leftCircleEle;
        const rightCircleEle = scrollbarOptions.rightCircleEle;
        const isAxis = target.id.split('_')[2] === leftArrowEle.id.split('_')[2];
        leftCircleEle.style.fill = isLeftHover && isAxis ? style.circleHover : style.circle;
        rightCircleEle.style.fill = isRightHover && isAxis ? style.circleHover : style.circle;
        leftCircleEle.style.stroke = isLeftHover && isAxis ? style.circleHover : style.circle;
        rightCircleEle.style.stroke = isRightHover && isAxis ? style.circleHover : style.circle;
    }
}

export function resizeThumb(scrollbar, mouseX, mouseY) {
    let currentWidth;
    const circleRadius = scrollbar.scrollbarOptions[scrollbar.axis.name].height / 2;
    const padding = 5;
    const gripWidth = 14;
    const minThumbWidth = circleRadius * 2 + padding * 2 + gripWidth;
    const thumbX = scrollbar.previousRectX;
    const isInverse = scrollbar.axis.isAxisInverse;
    mouseX = mouseX;
    mouseY = mouseY;
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbar.axis.name];
    const mouseXY = (scrollbarOptions.isVertical && isInverse) ? mouseY : scrollbarOptions.isVertical ? scrollbarOptions.width -
        mouseY : isInverse ? scrollbarOptions.width - mouseX : mouseX;
    const diff = Math.abs(scrollbar.previousXY - mouseXY);
    if (scrollbar.isResizeLeft && mouseXY >= 0) {
        let currentX = thumbX + (mouseXY > scrollbar.previousXY ? diff : -diff);
        currentWidth = currentX - circleRadius >= 0 ? scrollbar.previousWidth + (mouseXY > scrollbar.previousXY ? -diff : diff) :
            scrollbar.previousWidth;
        currentX = currentX - circleRadius >= 0 ? currentX : thumbX;
        if (currentWidth >= minThumbWidth && mouseXY < currentX + currentWidth) {
            scrollbarOptions.thumbRectX = scrollbar.previousRectX = currentX;
            scrollbarOptions.thumbRectWidth = scrollbar.previousWidth = currentWidth;
            scrollbar.previousXY = mouseXY;
            positionThumb(currentX, currentWidth, scrollbar);
            setZoomFactorPosition(scrollbar, currentX, currentWidth);
        }
    }
    else if (scrollbar.isResizeRight) {
        currentWidth = mouseXY >= minThumbWidth + scrollbarOptions.thumbRectX && mouseXY <= scrollbarOptions.width - circleRadius ?
            mouseXY - scrollbarOptions.thumbRectX : scrollbar.previousWidth;
        scrollbarOptions.thumbRectWidth = scrollbar.previousWidth = currentWidth;
        scrollbar.previousXY = mouseXY;
        positionThumb(scrollbarOptions.startX, currentWidth, scrollbar);
        setZoomFactorPosition(scrollbar, scrollbarOptions.startX, currentWidth);
        if (!scrollbarOptions.isLazyLoad) {
            setZoomFactorPosition(scrollbar, scrollbarOptions.startX, currentWidth);
        }
    }
}

export function setHighlightSelectionOptions(dataId, selectionHighlightOptions) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.selectionHighlightOptions(selectionHighlightOptions);
    }
}

export function showTooltip(x, y, isPoint = false, id) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(id);
    if (!chart) {
        return;
    }
    if (isPoint) {
        for (const series of chart.userInteractionBase.visibleSeries) {
            for (const point of series.points) {
                const pointX = series.x_Axis.valueType === 'DateTime' ? point.xV : point.x;
                let xValue = x;
                if (series.x_Axis.valueType === 'DateTime') {
                    xValue = new Date(xValue).getTime();
                }
                if (x === pointX && y === point.yV) {
                    chart.mouseX = point.r[0].x + series.clipRect.x;
                    chart.mouseY = point.r[0].y + series.clipRect.y;
                    tooltipMousemovehandler(chart);
                    markerMove(chart, false);
                    break;
                }
            }
        }
    }
    else {
        chart.mouseX = x;
        chart.mouseY = y;
        tooltipMousemovehandler(chart);
        markerMove(chart, false);
    }
}

export function hideTooltip(id) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(id);
    if (!chart) {
        return;
    }
    removeTooltip(sfBlazorToolkit.base.Browser.isDevice ? 2000 : 1000, chart);
}

export function showCrosshair(x, y, id) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(id);
    if (!chart) {
        return;
    }
    chart.mouseX = x;
    chart.mouseY = y;
    crosshairMousemoveHandler(chart);
}

export function hideCrosshair(id) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(id);
    if (!chart) {
        return;
    }
    removeCrosshair(chart, sfBlazorToolkit.base.Browser.isDevice ? 2000 : 1000);
}

export function updateZoomingOptions(id, isZoomed) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(id);
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
        chart.zoomBase.zoomingModule.isZoomed = isZoomed;
        chart.zoomBase.zoomingModule.isPanning = isZoomed;
    }
}

export function setTooltipData(dataId, clipRects, seriesMarkers, seriesBorders, axes, tooltip, dateValuePairs, numberValuePairs, axisClipRect, tooltipTemplate, theme) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (chart.tooltipBase.tooltipModule) {
        removeTooltipCrosshairOnZoom(chart);
    }
    if (tooltip) {
        tooltip.template = !sfBlazorToolkit.base.isNullOrUndefined(tooltipTemplate) && tooltipTemplate !== '' ? tooltipTemplate : null;
        chart.tooltipBase.tooltipModule = tooltip;
        chart.tooltipBase.tooltipFormat = !sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.format) ? chart.getTooltipFormat(chart.tooltipBase.tooltipModule.format) : chart.tooltipBase.tooltipFormat;
    }
    if (theme && chart.tooltip && chart.tooltip.theme) {
        chart.tooltip.theme = theme;
        chart.tooltip.themeStyle = svgbase.getTooltipThemeColor(theme);
        chart.tooltip.render();
    }
    // Ensure future tooltip instances use the updated theme
    if (theme) {
        chart.userInteractionBase.theme = theme;
    }
    chart.dateValuePairs = dateValuePairs;
    chart.numberValuePairs = numberValuePairs;
    chart.userInteractionBase.chartStartMove = false;
    chart.getTooltipData(clipRects, seriesMarkers, seriesBorders, axes);
    chart.userInteractionBase.chartStartMove = true;
    chart.zoomBase.axisCollections = axes;
    chart.userInteractionBase.axisClipRect = axisClipRect;
    if (chart.zoomBase.zoomingModule) {
        updateClipRect(getElement(chart.element.id + chart.zoomBase.clipRectId), chart);
    }
    if (chart.isDataLoaded) {
        setTimeout(() => {
            chart.removeTooltipData();
        }, 500);
    }
    applyZoomingToolkit(chart);
}

export function setTooltipOptions(dataId, tooltip, tooltipOptions, clipRects, seriesMarkers, seriesBorders, axes, dateValuePairs, numberValuePairs) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.dateValuePairs = dateValuePairs;
        currentInstance.numberValuePairs = numberValuePairs;
        currentInstance.userInteractionBase.chartStartMove = false;
        currentInstance.tooltipOptions(tooltip, tooltipOptions);
        currentInstance.zoomBase.axisCollections = axes;
        if (sfBlazorToolkit.base.isNullOrUndefined(currentInstance.userInteractionBase.visibleSeries)) {
            currentInstance.getVisibleSeries(currentInstance, clipRects, seriesMarkers, seriesBorders, axes);
        }
        currentInstance.userInteractionBase.chartStartMove = true;
        if (currentInstance.zoomBase.isResized && currentInstance.zoomBase.zoomingModule) {
            currentInstance.zoomBase.isResized = false;
            updateClipRect(getElement(currentInstance.element.id + currentInstance.zoomBase.clipRectId), currentInstance);
        }
    }
}

export function setTooltipCrosshairOptions(dataId, tooltip, tooltipOptions) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.tooltipOptions(tooltip, tooltipOptions);
    }
}

export function setZoomOptions(dataId, chartZoomSettings) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
        const theme = chart.userInteractionBase.theme;
        chart.zoomBase.zoomSettings = chartZoomSettings;
        chart.isZooming = chartZoomSettings.enableMouseWheelZooming;
        chart.zoomBase.zoomingModule = new IZoom(chart);
        chart.zoomToolkitBase.selectionColor = theme === 'Fluent' ? '#424242' : theme === 'FluentDark' ? '#D6D6D6' : chart.zoomToolkitBase.selectionColor;
        chart.zoomToolkitBase.iconRectOverFill = theme === 'Fluent' ? '#EBEBEB' : theme === 'FluentDark' ? '#383838' : chart.zoomToolkitBase.iconRectOverFill;
        chart.zoomToolkitBase.iconRectSelectionFill = theme === 'Fluent' ? '#EBEBEB' : theme === 'FluentDark' ? '#383838' : chart.zoomToolkitBase.iconRectSelectionFill;
        chart.zoomToolkitBase.selectedId = chart.zoomBase.zoomingModule.isPanning ? chart.element.id + '_Zooming_Pan_1' : chart.element.id + '_Zooming_Zoom_1';
        chart.zoomToolkitBase.fillColor = theme === 'Fluent' ? '#424242' : theme === 'FluentDark' ? '#D6D6D6' : '#737373';
        if (!getElement(chart.element.id + chart.zoomBase.clipRectId) && chart.zoomBase.zoomingModule) {
            createClipRect(chart);
        }
        setTimeout(() => {
            setChartElementStyle(chart);
        }, 100);
        applyZoomingToolkit(chart);
    }
}

export function setChartElementStyle(chart) {
    if (!chart) {
        return;
    }
    const disableTouchAction = chart.zoomBase && chart.zoomBase.zoomSettings && (chart.zoomBase.zoomSettings.enableSelectionZooming || chart.zoomBase.zoomSettings.enablePinchZooming);
    const touchAction = disableTouchAction ? 'none' : 'element';
    if (chart.element) {
        chart.element.style.touchAction = touchAction;
        chart.element.style.msTouchAction = touchAction;
    }
}

export function setUIBooleanValues(dataId, isPointMouseDown, disableTrackTooltip, isPointDragging) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.userInteractionBase.isPointDragging = isPointDragging;
        currentInstance.userInteractionBase.isPointMouseDown = isPointMouseDown;
        currentInstance.userInteractionBase.disableTrackTooltip = disableTrackTooltip;
    }
}

export function setTooltipArgsData(dataId, header, textCollection) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        if (currentInstance.tooltipBase.currentPoints.length > 0 && currentInstance.tooltipBase.previousPoints.length > 0 && currentInstance.tooltipBase.currentPoints[0].point === currentInstance.tooltipBase.previousPoints[0].point) {
            return;
        }
        currentInstance.tooltipBase.formattedText = [];
        currentInstance.tooltipBase.argsData.headerText = header;
        currentInstance.tooltipBase.argsData.text = textCollection;
        seriesTooltip(currentInstance, currentInstance.tooltipBase.currentPoints[0], true);
    }
}

export function setSharedTooltipArgsData(dataId, header, textCollection, data) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        const prevLength = currentInstance.tooltipBase.sharedArgsData.data.length;
        if (prevLength !== data.length) {
            currentInstance.tooltipBase.currentPoints = currentInstance.tooltipBase.currentPoints.filter((point) => data.some((d) => d.seriesIndex === point.series.index && d.pointIndex === point.point.iX));
        }
        currentInstance.tooltipBase.sharedArgsData.headerText = header;
        currentInstance.tooltipBase.sharedArgsData.text = textCollection;
        groupedTooltip(currentInstance, currentInstance.tooltipBase.currentPoints, true, currentInstance.tooltipBase.lastData);
    }
}

export function invokeBlurEffect(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
        blurEffect(chart.element.id, chart);
    }
}

export function invokeRemoveSelectedElements(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
        chart.selectedDataIndexes = [];
        chart.styleId = chart.element.id + '_ej2_chart_selection';
        removeSelectedElements(dataId);
    }
}

export function redrawSelection(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    chart.isSeriesMode = chart.oldMode === 'Series';
    let chartSelectedDatas = chart.selectedDataIndexes;
    const charthighlightedDatas = chart.highlightDataIndexes;
    if (chart.styleId && chart.styleId.indexOf('highlight') > -1 && chart.highlightDataIndexes.length > 0) {
        chart.highlightDataIndexes = [];
        removeSelectedElements(dataId);
        chartSelectedDatas = charthighlightedDatas;
    }
    else {
        chart.styleId = chart.element.id + '_ej2_chart_selection';
        chart.selectedDataIndexes = [];
        removeSelectedElements(dataId);
    }
    blurEffect(chart.element.id, chart);
    selectDataIndex(chart.dataId, chartSelectedDatas);
}

export function removeSelectionStyles(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    chart.highlightDataIndexes = [];
    const elementCollection = document.getElementsByClassName(chart.unSelected);
    while (elementCollection.length > 0) {
        const element = elementCollection[0];
        if (element) {
            removeSvgClass(element, element.getAttribute('class'));
        }
    }
    for (let i = 0; i < chart.seriesTypes.length; i++) {
        removeLegendSelectionClass(document.getElementsByClassName(chart.unSelected), chart);
        removeLegendSelectionClass(document.getElementsByClassName(generateStyle(chart.element.id + 'SeriesGroup' + i, chart)), chart);
        removeLegendSelectionClass(getSeriesElements(i, chart), chart);
    }
    for (let i = 0; i < chart.seriesTypes.length; i++) {
        const legendShapeElement = findDOMElement(chart.element.id + '_chart_legend_shape_' + i);
        if (legendShapeElement) {
            removeSvgClass(legendShapeElement, legendShapeElement.getAttribute('class'));
        }
    }
}

export function highlightAnimation(element, index, duration, startOpacity, strokeWidth, chart) {
    let endOpacity;
    let endWidth;
    const series = (index >= 0 && index < chart.userInteractionBase.visibleSeries.length) ? chart.userInteractionBase.visibleSeries[parseInt(index.toString(), 10)] : undefined;
    const startWidth = series ? Number(series.width.toString()) + 1 : 0;
    if (series) {
        if (strokeWidth) {
            if (element.id.indexOf('Border') !== -1 && series.border.wT) {
                endWidth = Number(series.border.wT.toString());
            }
            else if (element.id.indexOf('Symbol') !== -1 && series.marker.b.wT) {
                endWidth = Number(series.marker.b.wT.toString());
            }
            else {
                endWidth = Number(series.width.toString());
            }
        }
        else {
            if (element.id.indexOf('Border') !== -1) {
                endOpacity = 1;
            }
            else if (element.id.indexOf('Symbol') !== -1) {
                endOpacity = Number(series.marker.oP.toString());
            }
            else {
                endOpacity = Number(series.opacity.toString());
            }
        }
    }
    if (endOpacity !== undefined || (strokeWidth && endWidth !== undefined)) {
        const animationOptions = {
            duration: duration,
            progress: function (args) {
                element.style.animation = '';
                const progress = args.timeStamp / args.duration;
                if (strokeWidth && endWidth !== undefined) {
                    const currentWidth = startWidth + (endWidth - startWidth) * progress;
                    element.setAttribute('stroke-width', currentWidth.toString());
                }
                else if (endOpacity !== undefined) {
                    const currentOpacity = startOpacity + (endOpacity - startOpacity) * progress;
                    element.setAttribute('opacity', currentOpacity.toString());
                }
            },
            end: function () {
                if (strokeWidth && endWidth !== undefined) {
                    element.setAttribute('stroke-width', endWidth.toString());
                }
                else if (endOpacity !== undefined) {
                    element.setAttribute('opacity', endOpacity.toString());
                }
            }
        };
        new sfBlazorToolkit.Animation(animationOptions).animate(element);
    }
}

export function removeLegendSelectionClass(elements, chart) {
    for (const element of elements) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
            removeSvgClass(element, element.getAttribute('class'));
            if (!sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor) && chart.highlightColor !== '' && chart.highlightPattern === 'None') {
                element.setAttribute('fill', chart.highlightColor);
            }
        }
    }
}

export function removeSelectedElements(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    for (let i = 0; i < chart.seriesTypes.length; i++) {
        removeStyles(getSeriesElements(i, chart), chart);
    }
}

export function selectionChart(dataId, index) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
        chart.currentMode = chart.selectionMode;
        chart.styleId = chart.element.id + '_ej2_chart_selection';
        chart.isSeriesMode = false;
        selection(chart, index, findElements(chart, '', index));
    }
}

export function selectDataIndex(dataId, chartSelectedDatas) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        return;
    }
    isAlreadySelected({ type: 'click' }, currentInstance);
    const originalMode = currentInstance.currentMode;
    for (let i = 0; i < chartSelectedDatas.length; i++) {
        if (chartSelectedDatas[i] !== null) {
            const selectedIndex = chartSelectedDatas[i];
            const query = '#' + findElementId(currentInstance, selectedIndex);
            performSelection(selectedIndex, currentInstance, currentInstance.element.querySelector(query));
        }
    }
    if (currentInstance.currentMode !== originalMode) {
        currentInstance.currentMode = originalMode;
    }
}

export function findElementId(chart, selectedIndex) {
    const seriesType = chart.seriesTypes[selectedIndex.series];
    if (pointIdRequired(seriesType)) {
        return chart.element.id + '_Series_' + selectedIndex.series + '_Point_' + selectedIndex.point;
    }
    else {
        return chart.element.id + '_Series_' + selectedIndex.series + '_Point_' + selectedIndex.point + '_Symbol';
    }
}

export function focusTarget(id) {
    let element;
    let className;
    currentId = id.split("_")[0];
    if (id.indexOf('_chart_legend_') > -1) {
        element = getElement(id).parentElement;
        className = element.getAttribute('class');
        const clipRectElement = getElement(currentId + '_ChartAreaClipRect_');
        if (clipRectElement) {
            clipRectElement.parentNode.parentNode.appendChild(clipRectElement.parentNode);
        }
        const seriesElement = getElement(currentId + 'SeriesCollection').firstElementChild;
        const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
        const seriesId = seriesElement ? getFocusedSeries(chart, seriesElement.id) : null;
        setTabIndex(getElement(currentId + '_chart_legend_translate_g').firstElementChild, seriesId ? getElement(seriesId) : null);
        const trendLineElement = getElement(currentId + 'TrendLineCollection').firstElementChild;
        const trendlineSeriesId = trendLineElement ? getFocusedSeries(chart, trendLineElement.id) : null;
        if (trendlineSeriesId) {
            getElement(trendlineSeriesId).setAttribute('tabindex', '0');
        }
        element.setAttribute('tabindex', '0');
        if (className && className.indexOf('e-chart-focused') === -1) {
            className = className + ' e-chart-focused';
        }
        else if (!className) {
            className = 'e-chart-focused';
        }
    }
    else {
        element = getElement(id);
        className = 'e-chart-focused';
    }
    element.setAttribute('tabindex', '0');
    element.setAttribute('class', className);
    element.focus();
}

export function focusChild(element) {
    element.setAttribute('tabindex', '0');
    let className = element.getAttribute('class');
    element.setAttribute('tabindex', '0');
    if (className && className.indexOf('e-chart-focused') === -1) {
        className = className + ' e-chart-focused';
    }
    else if (!className) {
        className = 'e-chart-focused';
    }
    element.setAttribute('class', className);
    element.focus();
    return element.id;
}

export function getCurrentSeries(chart, targetId, seriesIndex) {
    let currentSeries;
    const targetSeries = [];
    chart.userInteractionBase.visibleSeries.forEach((series) => {
        if (targetId.indexOf('TrendLine') > -1 && series.category === 'TrendLine') {
            targetSeries.push(series);
        }
        else if (targetId.indexOf('TrendLine') === -1 && series.category !== 'TrendLine') {
            targetSeries.push(series);
        }
    });
    targetSeries.forEach((series) => {
        if (series.index === seriesIndex && (chart.tooltipBase.tooltipModule.enable && series.enableTooltip ? series.points.length > 0 : true)) {
            currentSeries = series;
        }
    });
    return currentSeries;
}

export function getFocusedSeries(chart, seriesId) {
    if (seriesId.indexOf('SeriesGroup') > -1) {
        const seriesIndex = +seriesId.match(/SeriesGroup(\d+)/)[1];
        const currentSeries = getCurrentSeries(chart, seriesId, seriesIndex);
        if (currentSeries && !currentSeries.focusable) {
            const targetSeries = [];
            chart.userInteractionBase.visibleSeries.forEach(function (series) {
                if (seriesId.indexOf('TrendLine') > -1 && series.category === 'TrendLine') {
                    targetSeries.push(series);
                }
                else if (seriesId.indexOf('TrendLine') === -1 && series.category !== 'TrendLine') {
                    targetSeries.push(series);
                }
            });
            for (let i = 0; i < targetSeries.length; i++) {
                if (targetSeries[parseInt(i.toString(), 10)].focusable) {
                    return targetSeries[parseInt(i.toString(), 10)].id;
                }
            }
        }
        else {
            return seriesId;
        }
    }
    return undefined;
}

export function getCurrentPointElement(chart, currentGroup, code) {
    const groupLength = currentGroup.children[currentGroup.children.length - 1].id.indexOf('_Connector_') > -1 ? currentGroup.children.length - 1 : currentGroup.children.length;
    for (let value = code !== 'ArrowDown' ? 1 : groupLength - 1; code !== 'ArrowDown' ? value < groupLength : value >= 1; code !== 'ArrowDown' ? value++ : value--) {
        const pointIndex = parseInt(currentGroup.children[parseInt(value.toString(), 10)].id.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
        if (code !== 'ArrowDown' && pointIndex >= chart.currentPointIndex) {
            chart.currentPointIndex = pointIndex;
            return currentGroup.children[parseInt(value.toString(), 10)];
        }
        if (code === 'ArrowDown' && pointIndex <= chart.currentPointIndex) {
            chart.currentPointIndex = pointIndex;
            return currentGroup.children[parseInt(value.toString(), 10)];
        }
    }
    chart.currentPointIndex = code !== 'ArrowUp' ? parseInt(currentGroup.children[groupLength - 1].id.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10) : parseInt(currentGroup.children[1].id.split('_Series_')[1].replace('_Symbol', '').split('_Point_')[1], 10);
    return currentGroup.children[code !== 'ArrowUp' ? groupLength - 1 : 1];
}

export function checkPointsLength(series) {
    for (let i = 0; i < series.points.length; i++) {
        if (i !== series.points[parseInt(i.toString(), 10)].iX) {
            return true;
        }
    }
    return false;
}

export function getCurrentPointIndex(chart, currentSeries, code) {
    for (let value = code !== 'ArrowDown' ? 0 : currentSeries.points.length - 1; code !== 'ArrowDown' ? value < currentSeries.points.length : value >= 0; code !== 'ArrowDown' ? value++ : value--) {
        if (code !== 'ArrowDown' && currentSeries.points[parseInt(value.toString(), 10)].iX >= chart.currentPointIndex) {
            chart.currentPointIndex = currentSeries.points[parseInt(value.toString(), 10)].iX;
            return value;
        }
        if (code === 'ArrowDown' && currentSeries.points[parseInt(value.toString(), 10)].iX <= chart.currentPointIndex) {
            chart.currentPointIndex = currentSeries.points[parseInt(value.toString(), 10)].iX;
            return value;
        }
    }
    chart.currentPointIndex = code !== 'ArrowUp' ? currentSeries.points[currentSeries.points.length - 1].iX : currentSeries.points[0].iX;
    return code !== 'ArrowUp' ? currentSeries.points.length - 1 : 0;
}

export function getRectSeriesElement(chart, currentSeries) {
    for (let index = 0; index < currentSeries.points.length; index++) {
        if (currentSeries.points[parseInt(index.toString(), 10)].iX >= chart.currentPointIndex) {
            chart.currentPointIndex = currentSeries.points[parseInt(index.toString(), 10)].iX;
            return getElement(currentId + '_Series_' + currentSeries.index + '_Point_' + chart.currentPointIndex);
        }
    }
    return getElement(currentId + '_Series_' + currentSeries.index + '_Point_' + (currentSeries.points.length - 1));
}

export function getActualIndex(index, totalLength) {
    return index > totalLength - 1 ? 0 : (index < 0 ? totalLength - 1 : index);
}

export function setTabIndex(previousElement, currentElement) {
    if (previousElement) {
        previousElement.removeAttribute('tabindex');
    }
    if (currentElement) {
        currentElement.setAttribute('tabindex', '0');
    }
}

export function getScrollEventArgs(e, chartIds) {
    const clientX = e.changedTouches ? e.changedTouches[0].clientX :
        e.clientX;
    const clientY = e.changedTouches ? e.changedTouches[0].clientY :
        e.clientY;
    const mouseXY = setScrollMouseXY(clientX, clientY, e.target['id'], chartIds);
    const touches = e.touches; //pointerId
    const touchList = [];
    if (e.type.indexOf('touch') > -1) {
        for (let i = 0, length = touches.length; i < length; i++) {
            touchList.push({ pageX: touches[parseInt(i.toString(), 10)].clientX, pageY: touches[parseInt(i.toString(), 10)].clientY, pointerId: e.pointerId || 0 });
        }
    }
    let id = e.target.id;
    id = id.indexOf('scrollBar') > -1 ? id : svgId;
    return {
        type: e.type,
        clientX: e.clientX,
        clientY: e.clientY,
        mouseX: mouseXY.mouseX,
        mouseY: mouseXY.mouseY,
        pointerType: e.pointerType,
        target: id,
        changedTouches: {
            clientX: e.changedTouches ? e.changedTouches[0].clientX : 0,
            clientY: e.changedTouches ? e.changedTouches[0].clientY : 0
        },
        touches: touchList,
        pointerId: e.pointerId
    };
}

export function setScrollMouseXY(pageX, pageY, id, chartIds) {
    svgId = chartIds[1];
    const chartId = chartIds[0];
    dotnetref = dotnetrefCollection.find((item) => { return chartId === item.id; }).dotnetref;
    let mouseX = pageX;
    let mouseY = pageY;
    const svgElement = getElement(svgId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(svgElement)) {
        const svgRect = svgElement.getBoundingClientRect();
        mouseX = pageX - Math.max(svgRect.left, 0);
        mouseY = pageY - Math.max(svgRect.top, 0);
    }
    return { mouseX: mouseX, mouseY: mouseY };
}

export function tooltipMousemovehandler(chart) {
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule)) {
        if (chart.userInteractionBase.isPointMouseDown) {
            removeTooltip(chart.tooltipBase.tooltipModule.fadeOutDuration, chart);
            return;
        }
        if (!chart.userInteractionBase.disableTrackTooltip && !isSelected() && chart.tooltipBase.tooltipModule.enable) {
            if ((!chart.tooltipBase.tooltipModule.shared && (!chart.isTouch || chart.userInteractionBase.startMove)) || svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect) && chart.tooltipBase.tooltipModule.shared && (!chart.isTouch || chart.userInteractionBase.startMove)) {
                tooltip(chart);
            }
            else if (chart.tooltipBase.tooltipModule.shared && chart.tooltipBase.isSharedRemove && !chart.isTouch) {
                removeTooltip(chart.tooltipBase.tooltipModule.fadeOutDuration, chart);
            }
        }
    }
}

export function dragStart() {
    isDragSelection = true;
    tooltipVisibility(false);
}
export function dragRemove() {
    isDragSelection = false;
    tooltipVisibility(true);
}
export function dragEnd() {
    setTimeout(() => {
        tooltipVisibility(true);
    }, 500);
}
export function tooltipVisibility(visibility) {
    const tooltipElement = document.querySelector('[id*="_tooltip"]');
    if (tooltipElement) {
        if (visibility) {
            tooltipElement.style.visibility = '';
        }
        else {
            tooltipElement.style.visibility = 'hidden';
        }
    }
}

export function markerMove(chart, remove) {
    if (chart.userInteractionBase.isPointMouseDown) {
        removeMarker(chart);
        return;
    }
    let data;
    let explodeSeries;
    let series;
    if (!chart.userInteractionBase.disableTrackTooltip) {
        if (!chart.tooltipBase.tooltipModule.shared || !chart.tooltipBase.tooltipModule.enable) {
            data = getData(chart);
            series = data.series;
            if (!sfBlazorToolkit.base.isNullOrUndefined(series) && !sfBlazorToolkit.base.isNullOrUndefined(series.marker) && series.marker.aH) {
                const previous = chart.markerExplodeBase.markerPreviousPoints.length > 0 ? chart.markerExplodeBase.markerPreviousPoints[0] : null;
                explodeSeries = (!sfBlazorToolkit.base.isNullOrUndefined(series)) && ((series.type === 'Bubble' || series.type === 'Scatter') ||
                    (series.marker.vS && series.marker.mW !== 0 && series.marker.mH !== 0));
                data.lierIndex = chart.userInteractionBase.lierIndex;
                if (data.point && explodeSeries && (sfBlazorToolkit.base.isNullOrUndefined(previous) || previous.point !== data.point || (previous.lierIndex > 3 && previous.lierIndex !== chart.userInteractionBase.lierIndex))) {
                    chart.markerExplodeBase.markerCurrentPoints.push(data);
                }
                if (data.point && explodeSeries && chart.userInteractionBase.isPointMouseDown) {
                    chart.markerExplodeBase.markerCurrentPoints.push(data);
                }
                if (chart.tooltipBase.tooltipModule && chart.tooltipBase.tooltipModule.showNearestTooltip && chart.markerExplodeBase.markerCurrentPoints.length === 0 &&
                    explodeSeries && svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect)) {
                    chart.markerExplodeBase.markerCurrentPoints = chart.tooltipBase.currentPoints;
                }
            }
        }
        else {
            if (!svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect)) {
                return;
            }
            if (chart.tooltipBase.tooltipModule.enable) {
                for (const series of chart.userInteractionBase.visibleSeries) {
                    if (!series.enableTooltip || !series.visible || (!sfBlazorToolkit.base.isNullOrUndefined(series.marker) && !series.marker.aH)) {
                        continue;
                    }
                    if (chart.userInteractionBase.chartAreaType === 'CartesianAxes' && series.visible) {
                        data = getClosestX(chart, series);
                    }
                    if (data && !sfBlazorToolkit.base.isNullOrUndefined(data.point)) {
                        if (isSeriesAxisZoomed(series)) {
                            const markerWidth = data.point.mK ? data.point.mK.mW : data.series.marker.mW;
                            const markerHeight = data.point.mK ? data.point.mK.mH : data.series.marker.mH;
                            if ((!data.point.s.length || isPointInRect(data.point.s, chart.userInteractionBase.axisClipRect, data.point.r, markerWidth, markerHeight))) {
                                chart.markerExplodeBase.markerCurrentPoints.push(data);
                            }
                        }
                        else {
                            chart.markerExplodeBase.markerCurrentPoints.push(data);
                        }
                    }
                }
                chart.markerExplodeBase.markerCurrentPoints = getSharedPoints(chart, chart.markerExplodeBase.markerCurrentPoints);
            }
        }
        if (chart.markerExplodeBase.markerCurrentPoints.length > 0) {
            if (chart.markerExplodeBase.markerPreviousPoints.length === 0 || chart.userInteractionBase.isPointMouseDown || (chart.markerExplodeBase.markerPreviousPoints.length > 0 && ((chart.tooltipBase.tooltipModule.showNearestPoint && chart.tooltipBase.tooltipModule.shared) || chart.markerExplodeBase.markerPreviousPoints[0].point !== chart.markerExplodeBase.markerCurrentPoints[0].point))) {
                if (chart.markerExplodeBase.markerPreviousPoints.length > 0) {
                    removeMarker(chart);
                }
                for (let i = 0; i < chart.markerExplodeBase.markerCurrentPoints.length; i++) {
                    const pointData = chart.markerExplodeBase.markerCurrentPoints[parseInt(i.toString(), 10)];
                    const pointDataSeries = pointData.series;
                    const point = pointData.point;
                    point.mK = point.mK ? point.mK : { vS: false };
                    if ((pointData && point) || (series)) {
                        window.clearTimeout(chart.markerExplodeBase.markerExplodeInterval);
                        chart.markerExplodeBase.isRemove = true;
                        for (let j = 0; j < point.s.length; j++) {
                            if (!isRectSeries(pointDataSeries.type) || point.mK.vS) {
                                trackBall(chart, pointDataSeries, point, point.s[parseInt(j.toString(), 10)], j);
                            }
                        }
                    }
                }
                chart.markerExplodeBase.markerPreviousPoints = [];
                chart.markerExplodeBase.markerPreviousPoints = chart.markerExplodeBase.markerPreviousPoints.concat(chart.markerExplodeBase.markerCurrentPoints);
            }
        }
        if (!chart.tooltipBase.tooltipModule.enable && ((chart.markerExplodeBase.markerCurrentPoints.length === 0 && chart.markerExplodeBase.isRemove) || (remove && chart.markerExplodeBase.isRemove) || (chart.userInteractionBase.axisClipRect && !svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect)))) {
            chart.markerExplodeBase.isRemove = false;
            chart.markerExplodeBase.markerExplodeInterval = +setTimeout(() => {
                removeMarker(chart);
            }, 2000);
        }
    }
    if (chart.tooltipBase.tooltipModule.enable && !explodeSeries && chart.markerExplodeBase.markerCurrentPoints.length === 0 && chart.tooltipBase.currentPoints.length > 0) {
        removeMarker(chart);
    }
    chart.markerExplodeBase.markerCurrentPoints = [];
}

export function trackBall(chart, series, point, location, index, explodeSeries) {
    const marker = point.mK;
    const seriesMarker = series.marker;
    const symbolId = chart.element.id + '_Series_' + series.index + '_Point_' + point.iX + '_Trackball' + (index && index !== 0 ? '_' + index : '');
    const size = {
        width: ((marker.mW) ? marker.mW : seriesMarker.mW) + 3,
        height: ((marker.mH) ? marker.mH : seriesMarker.mH) + 3
    };
    const border = (marker.b || series.border);
    const borderShadow = (border.cL && border.cL !== 'transparent') ? border.cL :
        marker.f ? marker.f : point.i ? point.i : series.interior;
    const borderColor = convertHexToColor(colorNameToHex(borderShadow));
    const borderWidth = marker.b ? marker.b.wT : seriesMarker.b.wT;
    const markerShadow = 'rgba(' + borderColor.r + ',' + borderColor.g + ',' + borderColor.b + ',' + '0.2)';
    const transform = chart.userInteractionBase.chartAreaType === 'CartesianAxes' ? 'translate(' + series.clipRect.x + ',' + series.clipRect.y + ')' : '';
    const clipPath = (series.type === 'Bubble' || series.type === 'Scatter') ? 'url(#' + chart.clipPathID(series.index) + ')' : 'url(#' + chart.markerClipPathId(series.index) + ')';
    const shape = marker.sH ? marker.sH : seriesMarker.sH;
    for (let i = 0; i < 2; i++) {
        const options = new svgbase.PathOption(symbolId + '_' + i, i > 0 ? (marker.f || point.i || (explodeSeries ? series.interior : '#ffffff')) : 'transparent', borderWidth + (i > 0 ? 0 : 8), i > 0 ? borderShadow : markerShadow, (marker.oP || seriesMarker.oP), '', null);
        const shapeOption = calculateShapes(location, size, shape, options, seriesMarker.iU, false);
        drawTrackBall(chart.element.id + '_svg', shapeOption.renderOption, shapeOption.functionName, chart.markerExplodeBase.trackBallClass, clipPath, transform);
    }
}

export function colorNameToHex(color) {
    color = color === 'transparent' ? 'white' : color;
    document.body.appendChild(sfBlazorToolkit.base.createElement('text', { id: 'chartmeasuretext' }));
    const element = document.getElementById('chartmeasuretext');
    element.style.color = color;
    color = window.getComputedStyle(element).color;
    sfBlazorToolkit.base.remove(element);
    const exp = /^(rgb|hsl)(a?)[(]\s*([\d.]+\s*%?)\s*,\s*([\d.]+\s*%?)\s*,\s*([\d.]+\s*%?)\s*(?:,\s*([\d.]+)\s*)?[)]$/;
    const isRGBValue = exp.exec(color);
    return convertToHexCode(new IColorValue(parseInt(isRGBValue[3], 10), parseInt(isRGBValue[4], 10), parseInt(isRGBValue[5], 10)));
}

export function convertToHexCode(value) {
    return '#' + componentToHex(value.r) + componentToHex(value.g) + componentToHex(value.b);
}

export function componentToHex(value) {
    const hex = value.toString(16);
    return hex.length === 1 ? '0' + hex : hex;
}

export function convertHexToColor(hex) {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? new IColorValue(parseInt(result[1], 16), parseInt(result[2], 16), parseInt(result[3], 16)) :
        new IColorValue(255, 255, 255);
}

export function calculateShapes(location, size, shape, options, url, isChart) {
    let pathData;
    let functionName = 'path';
    const isBulletChart = isChart;
    const width = (isBulletChart && shape === 'Circle') ? (size.width - 2) : size.width;
    const height = (isBulletChart && shape === 'Circle') ? (size.height - 2) : size.height;
    const sizeBullet = 0;
    const centerX = location.x;
    const centerY = location.y;
    const startY = location.y + (-height / 2);
    const startX = location.x + (-width / 2);
    const angleStep = 72;
    let pointX;
    let pointY;
    switch (shape) {
        case 'Bubble':
        case 'Circle':
            functionName = 'ellipse';
            sfBlazorToolkit.base.merge(options, { 'd': '', 'rx': width / 2, 'ry': height / 2, 'cx': centerX, 'cy': centerY });
            break;
        case 'Cross':
            pathData = 'M' + ' ' + startX + ' ' + (centerY + (-height / 2)) + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                'M' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (-height / 2));
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Multiply':
            pathData = 'M ' + (centerX - sizeBullet) + ' ' + (centerY - sizeBullet) + ' L ' +
                (centerX + sizeBullet) + ' ' + (centerY + sizeBullet) + ' M ' +
                (centerX - sizeBullet) + ' ' + (centerY + sizeBullet) + ' L ' + (centerX + sizeBullet) + ' ' + (centerY - sizeBullet);
            sfBlazorToolkit.base.merge(options, { 'd': pathData, stroke: options.fill });
            break;
        case 'HorizontalLine':
            pathData = 'M' + ' ' + startX + ' ' + centerY + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + centerY;
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'VerticalLine':
            pathData = 'M' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' + 'L' + ' ' + centerX + ' ' + (centerY + (-height / 2));
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Diamond':
            pathData = 'M' + ' ' + startX + ' ' + centerY + ' ' +
                'L' + ' ' + centerX + ' ' + (centerY + (-height / 2)) + ' ' +
                'L' + ' ' + (centerX + (width / 2)) + ' ' + centerY + ' ' +
                'L' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + startX + ' ' + centerY + ' z';
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'ActualRect':
            pathData = 'M' + ' ' + startX + ' ' + (centerY + (-height / 8)) + ' ' +
                'L' + ' ' + (centerX + (sizeBullet)) + ' ' + (centerY + (-height / 8)) + ' ' +
                'L' + ' ' + (centerX + (sizeBullet)) + ' ' + (centerY + (height / 8)) + ' ' +
                'L' + ' ' + startX + ' ' + (centerY + (height / 8)) + ' ' +
                'L' + ' ' + startX + ' ' + (centerY + (-height / 8)) + ' z';
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'TargetRect':
            pathData = 'M' + ' ' + (startX + (sizeBullet)) + ' ' + (centerY + (-height / 2)) + ' ' +
                'L' + ' ' + (centerX + (sizeBullet / 2)) + ' ' + (centerY + (-height / 2)) + ' ' +
                'L' + ' ' + (centerX + (sizeBullet / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + (startX + (sizeBullet)) + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + (startX + (sizeBullet)) + ' ' + (centerY + (-height / 2)) + ' z';
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Rectangle':
        case 'StepArea':
        case 'StackingStepArea':
        case 'Square':
        case 'Flag':
            pathData = 'M' + ' ' + startX + ' ' + (centerY + (-height / 2)) + ' ' +
                'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (-height / 2)) + ' ' +
                'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + startX + ' ' + (centerY + (-height / 2)) + ' z';
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Pyramid':
        case 'Triangle':
            pathData = 'M' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + centerX + ' ' + (centerY + (-height / 2)) + ' ' +
                'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' z';
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Funnel':
        case 'InvertedTriangle':
            pathData = 'M' + ' ' + (centerX + (width / 2)) + ' ' + (centerY - (height / 2)) + ' ' +
                'L' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' +
                'L' + ' ' + (centerX - (width / 2)) + ' ' + (centerY - (height / 2)) + ' ' +
                'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY - (height / 2)) + ' z';
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Pentagon':
            for (let i = 0; i <= 5; i++) {
                pointX = (width / 2) * Math.cos((Math.PI / 180) * (i * angleStep));
                pointY = (height / 2) * Math.sin((Math.PI / 180) * (i * angleStep));
                if (i === 0) {
                    pathData = 'M' + ' ' + (centerX + pointX) + ' ' + (centerY + pointY) + ' ';
                }
                else {
                    pathData = pathData.concat('L' + ' ' + (centerX + pointX) + ' ' + (centerY + pointY) + ' ');
                }
            }
            pathData = pathData.concat('Z');
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Plus':
            pathData = 'M' + ' ' + startX + ' ' + centerY + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + centerY + ' ' + 'M' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' + 'L' + ' ' + centerX + ' ' + (centerY + (-height / 2));
            sfBlazorToolkit.base.merge(options, { 'd': pathData });
            break;
        case 'Image':
            functionName = 'Image';
            sfBlazorToolkit.base.merge(options, { 'href': url, 'height': height, 'width': width, x: startX, y: startY });
            break;
    }
    return { renderOption: options, functionName: functionName };
}

export function removeMarker(chart) {
    removeHighLightedMarker(chart);
    chart.markerExplodeBase.markerPreviousPoints = [];
}

export function crosshairMousemoveHandler(chart) {
    if (!chart.userInteractionBase.disableTrackTooltip && chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable) {
        if (svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect) && (!chart.isTouch || chart.userInteractionBase.startMove)) {
            createCrosshair(chart);
        }
        else {
            removeCrosshair(chart, 1000);
        }
    }
}

export function removeCrosshair(chart, duration) {
    const crosshair = getElement(chart.element.id + '_UserInteraction');
    const axisGroup = getElement(chart.element.id + '_crosshair_axis');
    stopAnimation(chart.crosshairBase.crosshairInterval);
    if (!chart.userInteractionBase.isFirstRendered && crosshair && crosshair.getAttribute('opacity') !== '0') {
        chart.crosshairBase.crosshairInterval = +setTimeout(() => {
            if (crosshair.getAttribute('opacity') !== '0') {
                const animationOptions = {
                    duration: 200,
                    progress: function (args) {
                        crosshair.style.animation = '';
                        crosshair.setAttribute('opacity', (1 - (args.timeStamp / args.duration)).toString());
                        if (axisGroup) {
                            while (axisGroup.firstChild) {
                                axisGroup.removeChild(axisGroup.firstChild);
                            }
                        }
                    },
                    end: function () {
                        crosshair.setAttribute('opacity', '0');
                        if (chart.tooltipBase.tooltipModule) {
                            chart.tooltipBase.valueX = null;
                            chart.tooltipBase.valueY = null;
                        }
                    }
                };
                new sfBlazorToolkit.Animation(animationOptions).animate(crosshair);
            }
        }, duration);
    }
}

export function createCrosshair(chart) {
    if (chart.crosshairBase.crosshair.snapToData || chart.crosshairBase.crosshair.highlightCategory) {
        findMousePoints(chart);
    }
    const chartRect = chart.userInteractionBase.axisClipRect;
    let horizontalCross = '';
    let verticalCross = '';
    let options;
    const crosshair = chart.crosshairBase.crosshair;
    const crossGroup = document.getElementById(chart.element.id + '_UserInteraction');
    const dashArray = crosshair.dashArray;
    const isHorizontalHighlight = chart.crosshairBase.crosshair.highlightCategory && chart.crosshairBase.crossHighlightWidth !== 0 && chart.userInteractionBase.isInverted;
    const isVerticalHighlight = chart.crosshairBase.crosshair.highlightCategory && chart.crosshairBase.crossHighlightWidth !== 0 && !chart.userInteractionBase.isInverted;
    window.clearTimeout(chart.crosshairBase.crosshairInterval);
    if (chart.tooltipBase.tooltipModule.enable && !svgbase.withInAreaBounds(chart.tooltipBase.valueX, chart.tooltipBase.valueY, chartRect)) {
        return;
    }
    chart.crosshairBase.crosshairX = chart.crosshairBase.crosshair.snapToData || chart.crosshairBase.crosshair.highlightCategory ? chart.crosshairLocationX : chart.tooltipBase.tooltipModule.enable && !chart.userInteractionBase.isInverted ? chart.tooltipBase.valueX : chart.mouseX;
    chart.crosshairBase.crosshairY = chart.crosshairBase.crosshair.snapToData || chart.crosshairBase.crosshair.highlightCategory ? chart.crosshairLocationY : chart.tooltipBase.tooltipModule.enable && chart.userInteractionBase.isInverted ? chart.tooltipBase.valueY : chart.mouseY;
    if (chart.userInteractionBase.isFirstRendered) {
        crossGroup.setAttribute('opacity', '1');
        crossGroup.setAttribute('style', 'pointer-events:none');
        chart.userInteractionBase.isFirstRendered = false;
    }
    else {
        crossGroup.setAttribute('opacity', '1');
    }
    if ((crosshair.lineType === 'Both' || crosshair.lineType === 'Horizontal') && !sfBlazorToolkit.base.isNullOrUndefined(chart.crosshairBase.crosshairY)) {
        if (isHorizontalHighlight) {
            chart.crosshairBase.crosshairY = adjustCrosshairPosition(chart.crosshairBase.crosshairY, true, chart);
        }
        horizontalCross += 'M ' + chartRect.x + ' ' + chart.crosshairBase.crosshairY +
            ' L ' + (chartRect.x + chartRect.width) + ' ' + chart.crosshairBase.crosshairY;
    }
    if ((crosshair.lineType === 'Both' || crosshair.lineType === 'Vertical') && !sfBlazorToolkit.base.isNullOrUndefined(chart.crosshairBase.crosshairX)) {
        if (isVerticalHighlight) {
            chart.crosshairBase.crosshairX = adjustCrosshairPosition(chart.crosshairBase.crosshairX, false, chart);
        }
        verticalCross += 'M ' + chart.crosshairBase.crosshairX + ' ' + chartRect.y +
            ' L ' + chart.crosshairBase.crosshairX + ' ' + (chartRect.y + chartRect.height);
    }
    if (crossGroup.childNodes.length !== 0) {
        const highlightWidth = chart.crosshairBase.crossHighlightWidth;
        const highlightColor = crosshair.line.color ? crosshairLightenColor(crosshair.line.color) : chart.crosshairBase.themeStyleCrosshairBackground;
        const lineWidth = crosshair.line.width;
        const lineColor = crosshair.line.color || chart.crosshairBase.themeStyleCrosshairLine;
        // Horizontal line options
        options = new svgbase.PathOption('_HorizontalLine', 'none', isHorizontalHighlight ? highlightWidth : lineWidth, isHorizontalHighlight ? highlightColor : lineColor, null, isHorizontalHighlight ? null : dashArray, horizontalCross);
        renderCrosshairLine(options, crossGroup.childNodes[0]);
        // Vertical line options
        options = new svgbase.PathOption('_VerticalLine', 'none', isVerticalHighlight ? highlightWidth : lineWidth, isVerticalHighlight ? highlightColor : lineColor, null, isVerticalHighlight ? null : dashArray, verticalCross);
        const crosshairAxisArgs = { AxisInfo: [] };
        renderCrosshairLine(options, crossGroup.childNodes[3]);
        renderAxisTooltip(chart, chartRect, crossGroup.childNodes[6], crosshairAxisArgs);
        if (chart.tooltipBase.crosshairMouseMoveEventCalled) {
            chart.dotnetref.invokeMethodAsync('OnCrosshairMoveHandler', crosshairAxisArgs);
        }
    }
}

export function adjustCrosshairPosition(crosshairBasePosition, isHorizontal, chart) {
    if (chart.crosshairBase.leftOverflow > 0) {
        crosshairBasePosition += isHorizontal ? -chart.crosshairBase.leftOverflow / 2 : chart.crosshairBase.leftOverflow / 2;
    }
    if (chart.crosshairBase.rightOverflow > 0) {
        crosshairBasePosition += isHorizontal ? chart.crosshairBase.rightOverflow / 2 : -chart.crosshairBase.rightOverflow / 2;
    }
    return crosshairBasePosition;
}

export function crosshairLightenColor(color) {
    const rgbValue = convertHexToColor(colorNameToHex(color));
    return 'rgb(' + rgbValue.r + ',' + rgbValue.g + ',' + rgbValue.b + ',' + 0.25 + ')';
}

export function findMousePoints(chart) {
    let data = getData(chart);
    const pointLocations = [];
    let nearestDataPoint = null;
    let minDifference = Infinity; // For finding the nearest value
    const isInverted = chart.userInteractionBase.isInverted;
    const mouseCoordinate = (isInverted ? 'x' : 'y') === 'x' ? chart.mouseX : chart.mouseY;
    for (const series of chart.userInteractionBase.visibleSeries) {
        if (series.visible && series.category !== 'TrendLine') {
            // Get the closest X value and store the data point
            const closestData = getClosestX(chart, series) || data;
            if (closestData && closestData.point) {
                const symbolLocation = getSymbolLocation(closestData);
                if (!sfBlazorToolkit.base.isNullOrUndefined(symbolLocation)) {
                    const pointLocation = isInverted ? (symbolLocation.x + chart.userInteractionBase.axisClipRect.x)
                        : (symbolLocation.y + chart.userInteractionBase.axisClipRect.y);
                    if (chart.crosshairBase.crosshair.snapToData || chart.crosshairBase.crosshair.highlightCategory) {
                        pointLocations.push(pointLocation); // Store point locations for nearest calculation
                        // Calculate the nearest point to the mouse
                        const difference = Math.abs(pointLocation - mouseCoordinate);
                        if (difference < minDifference) {
                            minDifference = difference;
                            nearestDataPoint = closestData;
                        }
                    }
                }
            }
        }
    }
    // Use the nearest data point
    if ((chart.crosshairBase.crosshair.snapToData || chart.crosshairBase.crosshair.highlightCategory) && nearestDataPoint) {
        data = nearestDataPoint;
        chart.crosshairBase.crossHighlightWidth = 0;
        if (chart.crosshairBase.crosshair.highlightCategory && data.series.x_Axis.valueType === 'Category') {
            chart.crosshairBase.crossHighlightWidth = calculateCrossHighlightWidth(chart, data);
        }
    }
    if (data && data.point && (data.point.r.length || data.point.s.length)) {
        if (!chart.userInteractionBase.isInverted) {
            chart.crosshairLocationY = data.point.s[0].y + data.series.clipRect.y;
            chart.crosshairLocationX = !sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.valueX) && chart.tooltipBase.valueX !== 0 ? chart.tooltipBase.valueX : data.point.s[0].x + data.series.clipRect.x;
            if (chart.crosshairBase.crosshair.highlightCategory && chart.crosshairBase.crossHighlightWidth !== 0) {
                chart.crosshairLocationX = calculateCrosshairCoordinate(data, false);
            }
        }
        else {
            chart.crosshairLocationX = data.point.s[0].x + data.series.clipRect.x;
            chart.crosshairLocationY = !sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.valueY) && chart.tooltipBase.valueY !== 0 ? chart.tooltipBase.valueY : data.point.s[0].y + data.series.clipRect.y;
            if (chart.crosshairBase.crosshair.highlightCategory && chart.crosshairBase.crossHighlightWidth !== 0) {
                chart.crosshairLocationY = calculateCrosshairCoordinate(data, true);
            }
        }
    }
}

export function calculateCrossHighlightWidth(chart, data) {
    const visibleRange = data.series.x_Axis.visibleRange;
    const chartRect = chart.userInteractionBase.axisClipRect;
    // Adjust the width based on the calculated distance and axis range
    const rectSize = chart.userInteractionBase.isInverted ? chartRect.height : chartRect.width;
    const fullWidth = rectSize / visibleRange.delta;
    const relativePosition = (data.point.xV - visibleRange.start) / (visibleRange.end - visibleRange.start);
    chart.crosshairBase.leftOverflow = Math.max(0, fullWidth / 2 - relativePosition * rectSize);
    chart.crosshairBase.rightOverflow = Math.max(0, (relativePosition * rectSize + fullWidth / 2) - rectSize);
    const adjustedWidth = Math.max(0, fullWidth - chart.crosshairBase.leftOverflow - chart.crosshairBase.rightOverflow);
    return adjustedWidth;
}

export function calculateCrosshairCoordinate(data, isInverted) {
    const coefficient = valueToCoefficient(data.point.xV, data.series.x_Axis);
    return isInverted ? (1 - coefficient) * data.series.x_Axis.rect.h + data.series.x_Axis.rect.y : coefficient * data.series.x_Axis.rect.w + data.series.x_Axis.rect.x;
}

export function renderAxisTooltip(chart, chartRect, axisGroup, crosshairAxisArgs) {
    let axis;
    let axisInfo = { text: '', value: null };
    let rect;
    let pathElement;
    let textElem;
    let textOptions;
    const padding = 5;
    let direction;
    let axisRect;
    if (axisGroup) {
        while (axisGroup.firstChild) {
            axisGroup.removeChild(axisGroup.firstChild);
        }
    }
    for (let k = chart.userInteractionBase.axes.length - 1, length = 0; k >= length; k--) {
        axis = chart.userInteractionBase.axes[parseInt(k.toString(), 10)];
        axisRect = !axis.placeNextToAxisLine ? axis.rect : axis.updatedRect;
        if (axis.crosshairTooltip.enable) {
            if ((chart.crosshairBase.crosshairX <= (axisRect.x + axisRect.w) && axisRect.x <= chart.crosshairBase.crosshairX && axisRect.w !== 0) ||
                (chart.crosshairBase.crosshairY <= (axisRect.y + axisRect.h) && axisRect.y <= chart.crosshairBase.crosshairY && axisRect.h !== 0)) {
                pathElement = document.getElementById(chart.element.id + '_axis_tooltip_' + k);
                textElem = document.getElementById(chart.element.id + '_axis_tooltip_text_' + k);
                axisInfo = getAxisText(chart, axis);
                if (!axisInfo.text) {
                    continue;
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(crosshairAxisArgs)) {
                    crosshairAxisArgs.AxisInfo.push({ AxisName: axis.name, AxisLabel: axisInfo.text, Value: axisInfo.value });
                }
                rect = tooltipLocation(chart, axisInfo.text, axis, chartRect, new svgbase.Rect(axisRect.x, axisRect.y, axisRect.w, axisRect.h));
                if (sfBlazorToolkit.base.isNullOrUndefined(pathElement)) {
                    pathElement = chart.userInteractionBase.svgRenderer.drawPath({
                        'id': chart.element.id + '_axis_tooltip_' + k,
                        'fill': axis.crosshairTooltip.fill || chart.crosshairBase.themeStyleCrosshairFill
                    });
                }
                if (textElem) {
                    removeElement(textElem.id);
                }
                axisGroup.appendChild(pathElement);
                textOptions = new svgbase.TextOption(chart.element.id + '_axis_tooltip_text_' + k, 0, 0, 'start', axisInfo.text);
                const textStyle = {
                    size: axis.crosshairTooltip.textStyle.size || chart.crosshairBase.themeStyleCrosshairTextSize,
                    color: axis.crosshairTooltip.textStyle.color || chart.crosshairBase.themeStyleCrosshairLabel,
                    fontFamily: axis.crosshairTooltip.textStyle.fontFamily || chart.crosshairBase.themeStyleCrosshairFontFamily,
                    fontStyle: axis.crosshairTooltip.textStyle.fontStyle,
                    fontWeight: axis.crosshairTooltip.textStyle.fontWeight || chart.crosshairBase.themeStyleCrosshairFontWeight,
                    opacity: axis.crosshairTooltip.textStyle.opacity
                };
                textElem = svgbase.textElement(textOptions, textStyle, textStyle.color, axisGroup);
                direction = findCrosshairDirection(chart.crosshairBase.rx, chart.crosshairBase.ry, rect, chart.crosshairBase.arrowLocation, 8, chart.crosshairBase.isTop, chart.crosshairBase.isBottom, chart.crosshairBase.isLeft, chart.crosshairBase.crosshairX, chart.crosshairBase.crosshairY);
                pathElement.setAttribute('d', direction);
                textElem.textContent = axisInfo.text;
                textElem.setAttribute('x', (rect.x + padding + (chart.userInteractionBase.enableRTL ? chart.crosshairBase.elementSize.width : 0)).toString());
                textElem.setAttribute('y', (rect.y + padding + 3 * chart.crosshairBase.elementSize.height / 4).toString());
                if (chart.userInteractionBase.theme.indexOf('Fluent') > -1) {
                    const shadowId = chart.element.id + '_shadow';
                    pathElement.setAttribute('box-shadow', '0px 1.6px 3.6px 0px #00000021, 0px 0.3px 0.9px 0px #0000001A');
                    pathElement.setAttribute('filter', 'url(#' + shadowId + ')');
                    let shadow = '<filter id="' + shadowId + '" height="130%"><feGaussianBlur in="SourceAlpha" stdDeviation="3"/>';
                    shadow += '<feOffset dx="-1" dy="3.6" result="offsetblur"/><feComponentTransfer><feFuncA type="linear" slope="0.2"/>';
                    shadow += '</feComponentTransfer><feMerge><feMergeNode/><feMergeNode in="SourceGraphic"/></feMerge></filter>';
                    const defElement = chart.userInteractionBase.svgRenderer.createDefs();
                    defElement.setAttribute('id', chart.element.id + 'SVG_tooltip_definition');
                    pathElement.appendChild(defElement);
                    defElement.innerHTML = shadow;
                }
            }
        }
    }
}

export function getAxisText(chart, axis) {
    let pointValue;
    const axisInfo = { text: '', value: null };
    chart.crosshairBase.isBottom = false;
    chart.crosshairBase.isTop = false;
    chart.crosshairBase.isLeft = false;
    chart.crosshairBase.isRight = false;
    const labelValue = (axis.valueType === 'Category' && axis.labelPlacement === 'BetweenTicks')
        ? 0.5 : 0;
    const isOpposed = axis.isAxisOppositePosition;
    if (axis.orientation === 'Horizontal') {
        pointValue = getValueByPoint(Math.abs(chart.crosshairBase.crosshairX - axis.rect.x), axis.rect.w, axis.orientation, axis.visibleRange, axis.isAxisInverse) + labelValue;
        chart.crosshairBase.isBottom = !isOpposed;
        chart.crosshairBase.isTop = isOpposed;
    }
    else {
        pointValue = getValueByPoint(Math.abs(chart.crosshairBase.crosshairY - axis.rect.y), axis.rect.h, axis.orientation, axis.visibleRange, axis.isAxisInverse) + labelValue;
        chart.crosshairBase.isRight = isOpposed;
        chart.crosshairBase.isLeft = !isOpposed;
    }
    if (axis.valueType === 'DateTime') {
        const dateText = new sfBlazorToolkit.base.Internationalization().getDateFormat({ format: axis.format ? axis.format : axis.dateFormat, type: firstToLowerCase('DateTime') })(convertDateAndTime(new Date(pointValue)));
        axisInfo.text = getGlobalizedDate(dateText, chart.dateValuePairs);
        axisInfo.value = new Date(pointValue);
        return axisInfo;
    }
    else if (axis.valueType === 'Category') {
        axisInfo.text = (pointValue < axis.labels.length) ? axis.labels[Math.floor(pointValue)] : '';
        axisInfo.value = Math.floor(pointValue);
        return axisInfo;
    }
    else if (axis.valueType === 'DateTimeCategory') {
        axisInfo.text = getIndexedAxisLabel(axis.labels[Math.round(pointValue)], customFormat(axis, chart), chart.dateValuePairs);
        axisInfo.value = Math.floor(pointValue);
        return axisInfo;
    }
    else if (axis.valueType === 'Logarithmic') {
        const label = formatAxisValue(Math.pow(axis.logBase, pointValue), getaxisFormat(axis).indexOf('{value}') > -1, axis.labelFormat, void (0), chart);
        axisInfo.text = getGlobalizedNumber(label, chart.numberValuePairs);
        axisInfo.value = Number(Math.pow(axis.logBase, pointValue));
        return axisInfo;
    }
    else {
        const customLabelFormat = axis.labelFormat && axis.labelFormat.match('{value}') !== null;
        const label = customLabelFormat ? axis.labelFormat.replace('{value}', formatAxisValue(pointValue, customLabelFormat, axis.labelFormat, 3, chart)) : formatAxisValue(pointValue, customLabelFormat, axis.labelFormat, 3, chart);
        axisInfo.text = getGlobalizedNumber(label, chart.numberValuePairs);
        axisInfo.value = pointValue;
        return axisInfo;
    }
}

export function convertDateAndTime(date) {
    return new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);
}

export function getGlobalizedDate(resultText, dateValuePairs) {
    Object.keys(dateValuePairs).forEach((Key) => {
        resultText = resultText['replaceAll'] ? resultText['replaceAll'](Key, dateValuePairs[`${Key}`]) : resultText['replace'](Key, dateValuePairs[`${Key}`]);
    });
    return resultText;
}

export function getGlobalizedNumber(text, numberValuePairs) {
    const subStrings = text.split('');
    const numberKeys = Object.keys(numberValuePairs);
    for (let i = 0, textLength = text.length; i < textLength; i++) {
        for (let j = 0, keyLength = numberKeys.length - 1; j < keyLength; j++) {
            if (subStrings[parseInt(i.toString(), 10)] === numberKeys[parseInt(j.toString(), 10)]) {
                subStrings.splice(i, 1, numberValuePairs[numberKeys[parseInt(j.toString(), 10)]]);
                break;
            }
        }
    }
    return subStrings.join('');
}

export function firstToLowerCase(str) {
    return str.substring(0, 1).toLowerCase() + str.substring(1);
}

export function getaxisFormat(axis) {
    if (axis.labelFormat) {
        if (axis.labelFormat.indexOf('p') === 0 && !(axis.labelFormat.indexOf('{value}') > -1) && axis.isStack100) {
            return '{value}%';
        }
        return axis.labelFormat;
    }
    return axis.isStack100 ? '{value}%' : '';
}

export function getIndexedAxisLabel(value, format, dateValuePairs) {
    const texts = value.split(',');
    for (let i = 0; i < texts.length; i++) {
        texts[parseInt(i.toString(), 10)] = getGlobalizedDate(new sfBlazorToolkit.base.Internationalization().getDateFormat({ format: format })(convertDateAndTime(new Date(parseInt(texts[parseInt(i.toString(), 10)], 10)))), dateValuePairs);
    }
    return texts.join(', ');
}

export function customFormat(axis, chart) {
    return !axis.labelFormat ? axis.actualIntervalType === 'Years' ? 'yyyy' : getSkeleton(axis, chart) : axis.labelFormat;
}

export function getSkeleton(axis, chart) {
    const intervalType = axis.rangeIntervalType;
    const format = axis.format ? axis.format : axis.dateFormat;
    if (format) {
        return axis.format;
    }
    if (intervalType === 'Years' || intervalType === 'Quarter') {
        return 'y';
    }
    else if (intervalType === 'Months' || intervalType === 'Weeks') {
        return 'MMMM d';
    }
    else if (intervalType === 'Days') {
        return 'MM/dd/yyyy';
    }
    else if (intervalType === 'Hours') {
        return 'HH:mm tt';
    }
    else {
        return 'HH:mm:ss tt';
    }
}

export function tooltipLocation(chart, text, axis, bounds, axisRect) {
    const arrowPadding = 8;
    let tooltipRect;
    const boundsX = bounds.x;
    const boundsY = bounds.y;
    const islabelInside = axis.labelPosition === 'Inside';
    let scrollBarHeight = axis.scrollbarSettingsEnable
        ? axis.scrollBarHeight : 0;
    const crosshairFont = axis.crosshairTooltip.textStyle;
    crosshairFont.size = crosshairFont.size || chart.crosshairBase.themeStyleCrosshairTextSize;
    crosshairFont.fontFamily = crosshairFont.fontFamily || chart.crosshairBase.themeStyleCrosshairFontFamily;
    crosshairFont.fontWeight = crosshairFont.fontWeight || chart.crosshairBase.themeStyleCrosshairFontWeight;
    chart.crosshairBase.elementSize = svgbase.measureText(text, crosshairFont);
    const isOpposed = axis.isAxisOppositePosition;
    if (axis.orientation === 'Horizontal') {
        const yLocation = islabelInside ? axisRect.y - chart.crosshairBase.elementSize.height - 20 :
            axisRect.y + scrollBarHeight;
        const height = islabelInside ? axisRect.y - chart.crosshairBase.elementSize.height - 10 : axisRect.y + 10;
        chart.crosshairBase.arrowLocation = new IChartInternalLocation(chart.crosshairBase.crosshairX, yLocation);
        tooltipRect = new svgbase.Rect((chart.crosshairBase.crosshairX - (chart.crosshairBase.elementSize.width / 2) - 5), height + (!islabelInside ? scrollBarHeight : 0), chart.crosshairBase.elementSize.width + 10, chart.crosshairBase.elementSize.height + 10);
        if (isOpposed) {
            tooltipRect.y = islabelInside ? axisRect.y : axisRect.y -
                (chart.crosshairBase.elementSize.height + 20) - scrollBarHeight;
        }
        if (tooltipRect.x < boundsX) {
            tooltipRect.x = boundsX;
        }
        if (tooltipRect.x + tooltipRect.width > boundsX + bounds.width) {
            tooltipRect.x -= ((tooltipRect.x + tooltipRect.width) - (boundsX + bounds.width));
        }
        if (chart.crosshairBase.arrowLocation.x + arrowPadding / 2 > tooltipRect.x + tooltipRect.width - chart.crosshairBase.rx) {
            chart.crosshairBase.arrowLocation.x = tooltipRect.x + tooltipRect.width - chart.crosshairBase.rx - arrowPadding;
        }
        if (chart.crosshairBase.arrowLocation.x - arrowPadding < tooltipRect.x + chart.crosshairBase.rx) {
            chart.crosshairBase.arrowLocation.x = tooltipRect.x + chart.crosshairBase.rx + arrowPadding;
        }
    }
    else {
        scrollBarHeight = scrollBarHeight * (isOpposed ? 1 : -1);
        chart.crosshairBase.arrowLocation = new IChartInternalLocation(axisRect.x, chart.crosshairBase.crosshairY);
        const width = islabelInside ? axisRect.x - scrollBarHeight :
            axisRect.x - (chart.crosshairBase.elementSize.width) - 20;
        tooltipRect = new svgbase.Rect(width + scrollBarHeight, chart.crosshairBase.crosshairY - (chart.crosshairBase.elementSize.height / 2) - 5, chart.crosshairBase.elementSize.width + 10, chart.crosshairBase.elementSize.height + 10);
        if (isOpposed) {
            tooltipRect.x = islabelInside ? axisRect.x - chart.crosshairBase.elementSize.width - arrowPadding :
                axisRect.x + arrowPadding + scrollBarHeight;
            if ((tooltipRect.x + tooltipRect.width) > chart.userInteractionBase.availableSize.width) {
                chart.crosshairBase.arrowLocation.x -= ((tooltipRect.x + tooltipRect.width) - chart.userInteractionBase.availableSize.width);
                tooltipRect.x -= ((tooltipRect.x + tooltipRect.width) - chart.userInteractionBase.availableSize.width);
            }
        }
        else {
            if (tooltipRect.x < 0) {
                chart.crosshairBase.arrowLocation.x -= tooltipRect.x;
                tooltipRect.x = 0;
            }
        }
        if (tooltipRect.y < boundsY) {
            tooltipRect.y = boundsY;
        }
        if (tooltipRect.y + tooltipRect.height >= boundsY + bounds.height) {
            tooltipRect.y -= ((tooltipRect.y + tooltipRect.height) - (boundsY + bounds.height));
        }
        if (chart.crosshairBase.arrowLocation.y + arrowPadding / 2 > tooltipRect.y + tooltipRect.height - chart.crosshairBase.ry) {
            chart.crosshairBase.arrowLocation.y = tooltipRect.y + tooltipRect.height - chart.crosshairBase.ry - arrowPadding / 2;
        }
        if (chart.crosshairBase.arrowLocation.y - arrowPadding / 2 < tooltipRect.y + chart.crosshairBase.ry) {
            chart.crosshairBase.arrowLocation.y = tooltipRect.y + chart.crosshairBase.ry + arrowPadding / 2;
        }
    }
    return tooltipRect;
}

export function findCrosshairDirection(rX, rY, rect, arrowLocation, arrowPadding, top, bottom, left, tipX, tipY) {
    let direction = '';
    const startX = rect.x;
    const startY = rect.y;
    const width = rect.x + rect.width;
    const height = rect.y + rect.height;
    if (top) {
        direction = direction.concat('M' + ' ' + (startX) + ' ' + (startY + rY) + ' Q ' + startX + ' '
            + startY + ' ' + (startX + rX) + ' ' + startY);
        direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (startY) + ' Q ' + width + ' '
            + startY + ' ' + (width) + ' ' + (startY + rY));
        direction = direction.concat(' L' + ' ' + (width) + ' ' + (height - rY) + ' Q ' + width + ' '
            + (height) + ' ' + (width - rX) + ' ' + (height));
        if (arrowPadding !== 0) {
            direction = direction.concat(' L' + ' ' + (arrowLocation.x + arrowPadding / 2) + ' ' + (height));
            direction = direction.concat(' L' + ' ' + (tipX) + ' ' + (height + arrowPadding)
                + ' L' + ' ' + (arrowLocation.x - arrowPadding / 2) + ' ' + height);
        }
        if ((arrowLocation.x - arrowPadding / 2) > startX) {
            direction = direction.concat(' L' + ' ' + (startX + rX) + ' ' + height + ' Q ' + startX + ' '
                + height + ' ' + (startX) + ' ' + (height - rY) + ' z');
        }
        else {
            if (arrowPadding === 0) {
                direction = direction.concat(' L' + ' ' + (startX + rX) + ' ' + height + ' Q ' + startX + ' '
                    + height + ' ' + (startX) + ' ' + (height - rY) + ' z');
            }
            else {
                direction = direction.concat(' L' + ' ' + (startX) + ' ' + (height + rY) + ' z');
            }
        }
    }
    else if (bottom) {
        direction = direction.concat('M' + ' ' + (startX) + ' ' + (startY + rY) + ' Q ' + startX + ' '
            + (startY) + ' ' + (startX + rX) + ' ' + (startY) + ' L' + ' ' + (arrowLocation.x - arrowPadding / 2) + ' ' + (startY));
        direction = direction.concat(' L' + ' ' + (tipX) + ' ' + (arrowLocation.y));
        direction = direction.concat(' L' + ' ' + (arrowLocation.x + arrowPadding / 2) + ' ' + (startY));
        direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (startY)
            + ' Q ' + (width) + ' ' + (startY) + ' ' + (width) + ' ' + (startY + rY));
        direction = direction.concat(' L' + ' ' + (width) + ' ' + (height - rY) + ' Q ' + (width) + ' '
            + (height) + ' ' + (width - rX) + ' ' + (height));
        direction = direction.concat(' L' + ' ' + (startX + rX) + ' ' + (height) + ' Q ' + (startX) + ' '
            + (height) + ' ' + (startX) + ' ' + (height - rY) + ' z');
    }
    else if (left) {
        direction = direction.concat('M' + ' ' + (startX) + ' ' + (startY + rY) + ' Q ' + startX + ' '
            + (startY) + ' ' + (startX + rX) + ' ' + (startY));
        direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (startY) + ' Q ' + (width) + ' '
            + (startY) + ' ' + (width) + ' ' + (startY + rY) + ' L' + ' ' + (width) + ' ' + (arrowLocation.y - arrowPadding / 2));
        direction = direction.concat(' L' + ' ' + (width + arrowPadding) + ' ' + (tipY));
        direction = direction.concat(' L' + ' ' + (width) + ' ' + (arrowLocation.y + arrowPadding / 2));
        direction = direction.concat(' L' + ' ' + (width) + ' ' + (height - rY) + ' Q ' + width + ' ' + (height) + ' ' + (width - rX) + ' ' + (height));
        direction = direction.concat(' L' + ' ' + (startX + rX) + ' ' + (height) + ' Q ' + startX + ' '
            + (height) + ' ' + (startX) + ' ' + (height - rY) + ' z');
    }
    else {
        direction = direction.concat('M' + ' ' + (startX + rX) + ' ' + (startY) + ' Q ' + (startX) + ' '
            + (startY) + ' ' + (startX) + ' ' + (startY + rY) + ' L' + ' ' + (startX) + ' ' + (arrowLocation.y - arrowPadding / 2));
        direction = direction.concat(' L' + ' ' + (startX - arrowPadding) + ' ' + (tipY));
        direction = direction.concat(' L' + ' ' + (startX) + ' ' + (arrowLocation.y + arrowPadding / 2));
        direction = direction.concat(' L' + ' ' + (startX) + ' ' + (height - rY) + ' Q ' + startX + ' '
            + (height) + ' ' + (startX + rX) + ' ' + (height));
        direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (height) + ' Q ' + width + ' '
            + (height) + ' ' + (width) + ' ' + (height - rY));
        direction = direction.concat(' L' + ' ' + (width) + ' ' + (startY + rY) + ' Q ' + width + ' '
            + (startY) + ' ' + (width - rX) + ' ' + (startY) + ' z');
    }
    return direction;
}

export function renderCrosshairLine(options, childElement) {
    const keys = Object.keys(options);
    let key = '';
    for (let i = 0; i < keys.length; i++) {
        key = (keys[parseInt(i.toString(), 10)] === 'strokeWidth') ? 'stroke-width' : (keys[parseInt(i.toString(), 10)] === 'strokeDashArray') ?
            'stroke-dashArray' : (keys[parseInt(i.toString(), 10)] === 'direction') ? 'd' : keys[parseInt(i.toString(), 10)];
        if (key !== 'id') {
            childElement.setAttribute(key, options[keys[parseInt(i.toString(), 10)]]);
        }
    }
}

export function isSelected() {
    return false;
}

export function getTooltipElement(chart) {
    chart.tooltipBase.header = (sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.header)) ?
        ((chart.tooltipBase.tooltipModule.shared) ? '${point.x}' : '${series.name}')
        : chart.getTooltipFormat(chart.tooltipBase.tooltipModule.header);
    chart.tooltipBase.formattedText = [];
}

export function findData(data, previous, lierIndex) {
    return data.point && ((!previous || (previous.point !== data.point)) || (previous.lierIndex > 3 && previous.lierIndex !== lierIndex) || (previous.point === data.point));
}

export function renderSeriesTooltip(isFirst, chart) {
    let data = getData(chart);
    let nearestDataPoint = null;
    let minDifference = Infinity;
    data.lierIndex = chart.userInteractionBase.lierIndex;
    chart.tooltipBase.currentPoints = [];
    if (findData(data, chart.tooltipBase.previousPoints.length > 0 ? chart.tooltipBase.previousPoints[0] : null, chart.userInteractionBase.lierIndex)) {
        window.clearTimeout(chart.tooltipBase.toolTipInterval);
        if (!(data.series.dataEditSettings) && (chart.tooltipBase.previousPoints[0] &&
            data.point.iX === chart.tooltipBase.previousPoints[0].point.iX && data.series.index === chart.tooltipBase.previousPoints[0].series.index)) {
            chart.tooltipBase.isRemove = true;
            return null;
        }
        if (pushData(data, chart)) {
            triggerTooltipRender(data, isFirst, getTooltipText(data, chart), findHeader(data, chart), chart);
        }
        else {
            removeTooltip(chart.tooltipBase.tooltipModule.fadeOutDuration, chart);
        }
    }
    else {
        const tooltipWithInBounds = svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect);
        if (!data.point && chart.tooltipBase.isRemove && (!chart.tooltipBase.tooltipModule.showNearestTooltip || !tooltipWithInBounds)) {
            removeTooltip(chart.tooltipBase.tooltipModule.fadeOutDuration, chart);
            chart.tooltipBase.isRemove = false;
        }
        else {
            for (const series of chart.userInteractionBase.visibleSeries) {
                if (series.visible && !(series.category === 'TrendLine')) {
                    data = getClosestX(chart, series) || data;
                    if (chart.tooltipBase.tooltipModule.showNearestTooltip && tooltipWithInBounds && series.showNearestTooltip && data &&
                        data.point && series.enableTooltip && data.point.s[0]) {
                        const distance = getNearestDistance(data, series, chart, minDifference);
                        if (distance < minDifference) {
                            minDifference = distance;
                            nearestDataPoint = data;
                        }
                    }
                }
            }
            if (nearestDataPoint && pushData(nearestDataPoint, chart)) {
                data = nearestDataPoint;
                triggerTooltipRender(data, isFirst, getTooltipText(data, chart), findHeader(data, chart), chart);
            }
        }
    }
    if (data && data.point && (data.point.r.length || data.point.s.length)) {
        findMouseValue(data, chart);
    }
}

export function getNearestDistance(data, series, chart, currentMinDifference) {
    const symbolLocation = getSymbolLocation(data);
    if (symbolLocation) {
        const distanceToSymbol = getTooltipClosestDistance(symbolLocation.x, symbolLocation.y, chart);
        return distanceToSymbol;
    }
    return currentMinDifference;
}

export function getTooltipClosestDistance(x, y, chart) {
    const chartRect = chart.userInteractionBase.axisClipRect;
    return Math.sqrt(Math.pow(x + chartRect.x - chart.mouseX, 2) +
        Math.pow(y + chartRect.y - chart.mouseY, 2));
}

export function triggerTooltipRender(point, isFirst, textCollection, headerText, chart) {
    chart.tooltipBase.argsData = {
        data: {
            pointX: !sfBlazorToolkit.base.isNullOrUndefined(point.point.x) ? point.point.x.toString() : '',
            pointY: !sfBlazorToolkit.base.isNullOrUndefined(point.point.y) ? point.point.y.toString() : '',
            seriesIndex: point.series.index,
            seriesName: point.series.name,
            pointIndex: point.point.iX,
            pointText: point.point.t
        },
        headerText: headerText,
        point: point.point,
        series: {},
        text: textCollection
    };
    if (chart.tooltipBase.tooltipEventCalled) {
        chart.dotnetref.invokeMethodAsync('TooltipEventTriggeredAsync', chart.tooltipBase.argsData);
    }
    else {
        seriesTooltip(chart, point, isFirst);
    }
}

export function seriesTooltip(chart, point, isFirst) {
    const extraPoints = [];
    if (!sfBlazorToolkit.base.isNullOrUndefined(point)) {
        chart.tooltipBase.header = chart.tooltipBase.argsData.headerText;
        chart.tooltipBase.formattedText = chart.tooltipBase.formattedText.concat(chart.tooltipBase.argsData.text);
        const clipLocation = new IChartInternalLocation(point.series.clipRect.x, point.series.clipRect.y);
        if (!sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.template) && chart.tooltipBase.currentPoints.length > 0) {
            chart.tooltipBase.tooltipTempList = [];
            const pointsInfo = point.point;
            const tooltipTemp = {
                x: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.x) ? formatPointValue(pointsInfo, 'x', true, false, point.series, chart) : '',
                y: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.y) ? formatPointValue(pointsInfo, 'y', false, true, point.series, chart) : '',
                text: pointsInfo.tT.toString(),
                high: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.h) ? formatPointValue(pointsInfo, 'h', false, true, point.series, chart) : '',
                low: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.l) ? formatPointValue(pointsInfo, 'l', false, true, point.series, chart) : '',
                open: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.o) ? formatPointValue(pointsInfo, 'o', false, true, point.series, chart) : '',
                close: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.c) ? formatPointValue(pointsInfo, 'c', false, true, point.series, chart) : '',
                volume: !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo.v) ? formatPointValue(pointsInfo, 'v', false, true, point.series, chart) : '',
                pointX: pointsInfo.x ? pointsInfo.x.toString() : '',
                pointY: pointsInfo.y ? pointsInfo.y.toString() : '',
                seriesIndex: point.series.index,
                seriesName: point.series.name,
                pointIndex: point.point.iX,
                pointText: point.point.t
            };
            chart.tooltipBase.tooltipTempList.push(tooltipTemp);
            const symbolLocation = getSymbolLocation(point);
            const isRect = isRectSeries(point.series.type);
            const isNegative = isRect && point.point && parseInt(pointsInfo.y ? pointsInfo.y.toString() : '', 10) < 0;
            const inverted = chart.userInteractionBase.isInverted && isNegative;
            const templateLocation = getTemplateLocation(chart.userInteractionBase.axisClipRect, symbolLocation, chart.tooltipBase.tooltipElementSize.width, chart.tooltipBase.tooltipElementSize.height, findMarkerHeight(chart.tooltipBase.currentPoints[0], chart), clipLocation, inverted, isNegative);
            const padding = 40;
            templateLocation.x = templateLocation.x > chart.userInteractionBase.axisClipRect.width ? chart.userInteractionBase.axisClipRect.width - padding : templateLocation.x;
            chart.dotnetref.invokeMethodAsync('SetTooltipTemplateElementSizeAsync', templateLocation.x, templateLocation.y, chart.tooltipBase.tooltipTempList);
            isHighlightRemoved = false;
            removeHighlight(chart);
            highlightPoints(chart);
            updatePreviousPoint(chart, extraPoints);
        }
        else {
            const borderWidth = chart.userInteractionBase.chartBorderWidth;
            const padding = 3;
            createTooltipRenderer(chart, isFirst, getSymbolLocation(point), clipLocation, point.point, findShapes(chart), findMarkerHeight(chart.tooltipBase.currentPoints[0], chart), new svgbase.Rect(borderWidth, borderWidth, chart.userInteractionBase.availableSize.width - padding - borderWidth * 2, chart.userInteractionBase.availableSize.height - padding - borderWidth * 2), chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable, extraPoints, null, '');
        }
    }
    else {
        removeHighlight(chart);
    }
    chart.tooltipBase.isRemove = true;
}

export function groupedTooltip(chart, pointsInfo, isFirst, lastData) {
    const extraPoints = [];
    const data = pointsInfo[pointsInfo.length - 1];
    if (!sfBlazorToolkit.base.isNullOrUndefined(lastData) && chart.tooltipBase.currentPoints.length > 0) {
        const extraPoints = [];
        chart.tooltipBase.header = chart.tooltipBase.sharedArgsData.headerText;
        chart.tooltipBase.formattedText = chart.tooltipBase.sharedArgsData.text;
        findMouseValue(lastData, chart);
        const clipLocation = chart.tooltipBase.currentPoints.length === 1 ? new IChartInternalLocation(chart.tooltipBase.currentPoints[0].series.clipRect.x, chart.tooltipBase.currentPoints[0].series.clipRect.y) :
            new IChartInternalLocation(0, 0);
        if (!sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.template) && chart.tooltipBase.currentPoints.length > 0) {
            chart.tooltipBase.tooltipTempList = [];
            for (let i = 0; i < pointsInfo.length; i++) {
                const point = pointsInfo[parseInt(i.toString(), 10)].point;
                const tooltipTemp = {
                    x: !sfBlazorToolkit.base.isNullOrUndefined(point.x) ? formatPointValue(point, 'x', true, false, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    y: !sfBlazorToolkit.base.isNullOrUndefined(point.y) ? formatPointValue(point, 'y', false, true, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    text: point.t.toString(),
                    high: !sfBlazorToolkit.base.isNullOrUndefined(point.h) ? formatPointValue(point, 'h', false, true, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    low: !sfBlazorToolkit.base.isNullOrUndefined(point.l) ? formatPointValue(point, 'l', false, true, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    open: !sfBlazorToolkit.base.isNullOrUndefined(point.o) ? formatPointValue(point, 'o', false, true, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    close: !sfBlazorToolkit.base.isNullOrUndefined(point.c) ? formatPointValue(point, 'c', false, true, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    volume: !sfBlazorToolkit.base.isNullOrUndefined(point.v) ? formatPointValue(point, 'v', false, true, pointsInfo[parseInt(i.toString(), 10)].series, chart) : '',
                    pointX: point.x ? point.x.toString() : '',
                    pointY: point.y ? point.y.toString() : '',
                    seriesIndex: pointsInfo[parseInt(i.toString(), 10)].series.index,
                    seriesName: pointsInfo[parseInt(i.toString(), 10)].series.name,
                    pointIndex: pointsInfo[parseInt(i.toString(), 10)].point.iX,
                    pointText: pointsInfo[parseInt(i.toString(), 10)].point.t
                };
                chart.tooltipBase.tooltipTempList.push(tooltipTemp);
            }
            const symbolLocation = findSharedLocation(chart);
            const series = chart.tooltipBase.currentPoints[0].series;
            const isRect = isRectSeries(series.type);
            const isNegative = isRect && data.point && parseInt(data.point.y ? data.point.y.toString() : '', 10) < 0;
            const inverted = chart.userInteractionBase.isInverted && isNegative;
            const templateLocation = getTemplateLocation(chart.userInteractionBase.axisClipRect, symbolLocation, 0, 0, findMarkerHeight(chart.tooltipBase.currentPoints[0], chart), clipLocation, inverted, isNegative);
            chart.dotnetref.invokeMethodAsync('SetTooltipTemplateElementSizeAsync', templateLocation.x, templateLocation.y, chart.tooltipBase.tooltipTempList);
            setTooltipTemplateElementSize(chart, symbolLocation, clipLocation, inverted, isNegative);
            removeHighlight(chart);
            highlightPoints(chart);
            updatePreviousPoint(chart, extraPoints);
        }
        else {
            const padding = 3;
            const borderWidth = chart.userInteractionBase.chartBorderWidth;
            createTooltipRenderer(chart, isFirst, findSharedLocation(chart), clipLocation, lastData.point, findShapes(chart), findMarkerHeight(chart.tooltipBase.currentPoints[0], chart), new svgbase.Rect(borderWidth, borderWidth, chart.userInteractionBase.availableSize.width - padding - borderWidth * 2, chart.userInteractionBase.availableSize.height - padding - borderWidth * 2), chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable, extraPoints, null, '');
        }
    }
    else {
        extraPoints.push(data);
    }
    chart.tooltipBase.isSharedRemove = true;
}

export function setTooltipTemplateElementSize(chart, symbolLocation, clipLocation, inverted, isNegative) {
    const elementSize = getTemplateSize(chart.element.id + '_tooltip');
    if (elementSize && chart.tooltipBase.currentPoints.length > 0) {
        const templateLocation = getTemplateLocation(chart.userInteractionBase.axisClipRect, symbolLocation, elementSize.width, elementSize.height, findMarkerHeight(chart.tooltipBase.currentPoints[0], chart), clipLocation, inverted, isNegative);
        templateLocation.x += chart.userInteractionBase.secondaryElementOffset.left;
        templateLocation.y += chart.userInteractionBase.secondaryElementOffset.top;
        const tooltipElement = document.getElementById(chart.element.id + '_tooltip');
        const margin = 5;
        const padding = 5;
        const width = elementSize.width + 2 * margin;
        const tooltipX = templateLocation.x + 2 * padding;
        if (elementSize.width === 0 && symbolLocation.x + 10 > chart.userInteractionBase.axisClipRect.x + chart.userInteractionBase.axisClipRect.width) {
            templateLocation.x = chart.userInteractionBase.axisClipRect.x + tooltipX - (width + 4 * padding);
        }
        tooltipElement.setAttribute('style', 'top:' + templateLocation.y.toString() + 'px;left:' + templateLocation.x.toString() + 'px;pointer-events:none; position:absolute;z-index: 1;visibility: visible' + ';');
    }
}

export function isPointInRect(symbolLocation, axisClipRect, region, width, height) {
    let isPointIn;
    for (let index = 0; index < symbolLocation.length; index++) {
        const loc = symbolLocation[parseInt(index.toString(), 10)];
        if (svgbase.withInAreaBounds(loc.x + axisClipRect.x, loc.y + axisClipRect.y, axisClipRect, width, height)) {
            isPointIn = true;
        }
        else {
            isPointIn = false;
        }
    }
    return isPointIn;
}

export function pushData(data, chart) {
    if (data.series.enableTooltip) {
        const markerWidth = data.point.mK && data.point.mK.mW ? data.point.mK.mW : data.series.marker.mW;
        const markerHeight = data.point.mK && data.point.mK.mH ? data.point.mK.mH : data.series.marker.mH;
        if (chart.userInteractionBase.chartAreaType === 'CartesianAxes' && data.point.s.length > 0
            && !isPointInRect(data.point.s, chart.userInteractionBase.axisClipRect, data.point.r, markerWidth, markerHeight)) {
            return false;
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
            chart.tooltipBase.currentPoints.push(data);
        }
        return true;
    }
    return false;
}

export function renderGroupedTooltip(isFirst, chart) {
    let data;
    let lastData;
    stopAnimation(chart.tooltipBase.toolTipInterval);
    chart.tooltipBase.currentPoints = [];
    let pointsInfo = [];
    const text = [];
    let header;
    const argsData = [];
    let argsPoint;
    for (let i = 0; i < chart.userInteractionBase.visibleSeries.length; i++) {
        const series = chart.userInteractionBase.visibleSeries[parseInt(i.toString(), 10)];
        if (!series.enableTooltip || !series.visible || series.points.length === 0) {
            continue;
        }
        if (chart.userInteractionBase.chartAreaType === 'CartesianAxes' && series.visible && series.points.length > 0) {
            data = getClosestX(chart, series);
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(data) && data.point) {
            pointsInfo.push(data);
        }
    }
    if (pointsInfo.length === 0) {
        if (chart.tooltipBase.previousPoints.length > 0) {
            removeTooltip(chart.tooltipBase.tooltipModule.fadeOutDuration, chart);
        }
        return;
    }
    pointsInfo = getSharedPoints(chart, pointsInfo);
    pointsInfo = sortPointsInfo(pointsInfo);
    let closestValue = Number.MAX_VALUE;
    let pointValue;
    let tempData;
    pointsInfo.forEach((dataPoint) => {
        argsPoint = {
            pointX: !sfBlazorToolkit.base.isNullOrUndefined(dataPoint.point.x) ? dataPoint.point.x.toString() : '',
            pointY: !sfBlazorToolkit.base.isNullOrUndefined(dataPoint.point.y) ? dataPoint.point.y.toString() : '',
            seriesIndex: dataPoint.series.index,
            seriesName: dataPoint.series.name,
            pointIndex: dataPoint.point.iX,
            pointText: dataPoint.point.t
        };
        argsData.push(argsPoint);
        header = findHeader(dataPoint, chart);
        chart.tooltipBase.currentPoints.push(dataPoint);
        text.push(getTooltipText(dataPoint, chart));
        pointValue = (!chart.userInteractionBase.isInverted) ? chart.mouseX - dataPoint.series.clipRect.x : chart.mouseY - dataPoint.series.clipRect.y;
        if (dataPoint.point.s.length && Math.abs(pointValue - dataPoint.point.s[0].x) < closestValue) {
            closestValue = Math.abs(pointValue - dataPoint.point.s[0].x);
            tempData = dataPoint;
        }
        lastData = chart.tooltipBase.lastData = dataPoint.series.category === 'TrendLine' && chart.tooltipBase.tooltipModule.shared ? lastData : (tempData || lastData);
    });
    chart.tooltipBase.sharedArgsData = {
        headerText: header,
        text: text,
        data: argsData
    };
    if (lastData && lastData.series && isSeriesAxisZoomed(lastData.series) &&
        (!lastData.point || !lastData.point.s || lastData.point.s.length === 0 ||
            lastData.point.s[0].x < 0 || lastData.point.s[0].x > chart.userInteractionBase.axisClipRect.width)) {
        return;
    }
    if (chart.tooltipBase.sharedTooltipEventCalled) {
        chart.dotnetref.invokeMethodAsync('SharedTooltipEventTriggeredAsync', chart.tooltipBase.sharedArgsData);
        findMouseValue(lastData, chart);
    }
    else {
        groupedTooltip(chart, pointsInfo, isFirst, lastData);
    }
}

export function createTooltipRenderer(chart, isFirst, location, clipLocation, point, shapes, offset, bounds, crosshairEnabled, extraPoints, templatePoint, customTemplate) {
    const series = chart.tooltipBase.currentPoints[0].series;
    const isRect = isRectSeries(series.type);
    let svgTooltip;
    if (isFirst && !sfBlazorToolkit.base.isNullOrUndefined(location)) {
        svgTooltip = new svgbase.Tooltip({
            opacity: chart.tooltipBase.tooltipModule.opacity ? chart.tooltipBase.tooltipModule.opacity : 0.75,
            header: chart.tooltipBase.header,
            content: chart.tooltipBase.formattedText,
            fill: chart.tooltipBase.tooltipModule.fill,
            border: chart.tooltipBase.tooltipModule.border,
            enableAnimation: chart.tooltipBase.tooltipModule.enableAnimation,
            location: !sfBlazorToolkit.base.isNullOrUndefined(location) ? new IChartInternalLocation((location.x > bounds.width ? chart.userInteractionBase.axisClipRect.width : location.x) + chart.userInteractionBase.secondaryElementOffset.left, location.y + chart.userInteractionBase.secondaryElementOffset.top) : null,
            shared: chart.tooltipBase.tooltipModule.shared,
            crosshair: crosshairEnabled,
            shapes: shapes,
            clipBounds: clipLocation,
            areaBounds: new svgbase.Rect(bounds.x + chart.userInteractionBase.secondaryElementOffset.left, bounds.y + (0) + (chart.userInteractionBase.secondaryElementOffset.top), bounds.width, bounds.height),
            palette: findPalette(chart),
            controlName: 'Chart',
            controlInstance: chart,
            template: customTemplate || chart.tooltipBase.tooltipModule.template,
            data: templatePoint,
            theme: chart.userInteractionBase.theme,
            offset: offset,
            textStyle: chart.tooltipBase.tooltipModule.textStyle,
            isNegative: isRect && point && point.y ? ((series.seriesType !== 'BoxPlot') ? Number(point.y) < 0 : false) : false,
            inverted: chart.userInteractionBase.isInverted && isRect,
            arrowPadding: chart.tooltipBase.formattedText.length > 1 ? 0 : 7,
            availableSize: chart.userInteractionBase.availableSize,
            duration: chart.tooltipBase.tooltipDuration,
            isCanvas: false,
            rx: 4, ry: 4,
            isTextWrap: chart.tooltipBase.tooltipModule.enableTextWrap,
            enableRTL: chart.userInteractionBase.enableRTL,
            showHeaderLine: chart.tooltipBase.tooltipModule.showHeaderLine,
            showNearestTooltip: chart.tooltipBase.tooltipModule.showNearestTooltip
        });
    }
    if (!sfBlazorToolkit.base.isNullOrUndefined(svgTooltip)) {
        isHighlightRemoved = false;
        removeHighlight(chart);
        highlightPoints(chart);
        updatePreviousPoint(chart, extraPoints);
        renderTooltip(svgTooltip, chart.element.id + '_tooltip', chart);
    }
}

export function tooltip(chart) {
    getTooltipElement(chart);
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart.userInteractionBase.visibleSeries)) {
        if (!chart.tooltipBase.tooltipModule.shared) {
            renderSeriesTooltip(true, chart);
        }
        else {
            renderGroupedTooltip(true, chart);
        }
    }
}

export function getData(chart) {
    let pointData = null;
    let point = null;
    let series = null;
    let width;
    let height;
    let mouseX;
    let mouseY;
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart.userInteractionBase.visibleSeries)) {
        let isStackedArea = false;
        for (let i = 0; i < chart.userInteractionBase.visibleSeries.length; i++) {
            if (chart.userInteractionBase.visibleSeries[parseInt(i.toString(), 10)].type === 'StackingArea' || chart.userInteractionBase.visibleSeries[parseInt(i.toString(), 10)].type === 'StackingArea100') {
                isStackedArea = true;
                break;
            }
        }
        if (isStackedArea) {
            for (let len = chart.userInteractionBase.visibleSeries.length, i = 0; i < len; i++) {
                series = chart.userInteractionBase.visibleSeries[parseInt(i.toString(), 10)];
                width = series.type === 'Scatter' || (series.marker && series.marker.vS)
                    ? (series.marker.mH + 5) / 2 : 0;
                height = series.type === 'Scatter' || (series.marker && series.marker.vS)
                    ? (series.marker.mW + 5) / 2 : 0;
                mouseX = chart.mouseX;
                mouseY = chart.mouseY;
                if (series.dataEditSettings && isRectSeries(series.seriesType)) {
                    if (!(series.type === 'Bar' && series.chartIsTransposed) && (series.chartIsTransposed || series.type === 'Bar')) {
                        const markerWidth = series.marker.mW / 2;
                        mouseX = series.y_Axis.isAxisInverse ? mouseX + markerWidth : mouseX - markerWidth;
                    }
                    else {
                        const markerHeight = series.marker.mH / 2;
                        mouseY = series.y_Axis.isAxisInverse ? mouseY - markerHeight : mouseY + markerHeight;
                    }
                }
                if (series.visible && !sfBlazorToolkit.base.isNullOrUndefined(series.clipRect) && svgbase.withInAreaBounds(mouseX, mouseY, new svgbase.Rect(series.clipRect.x, series.clipRect.y, series.clipRect.w, series.clipRect.h), width, height)) {
                    point = getRectPoint(series, series.clipRect, mouseX, mouseY, chart);
                }
                if (point) {
                    return pointData = {
                        point: point,
                        series: series
                    };
                }
            }
        }
        else {
            for (let len = chart.userInteractionBase.visibleSeries.length, i = len - 1; i >= 0; i--) {
                series = chart.userInteractionBase.visibleSeries[parseInt(i.toString(), 10)];
                width = series.type === 'Scatter' || (series.marker && series.marker.vS)
                    ? (series.marker.mH + 5) / 2 : 0;
                height = series.type === 'Scatter' || (series.marker && series.marker.vS)
                    ? (series.marker.mW + 5) / 2 : 0;
                mouseX = chart.mouseX;
                mouseY = chart.mouseY;
                if (series.dataEditSettings && isRectSeries(series.seriesType)) {
                    if (!(series.type === 'Bar' && series.chartIsTransposed) && (series.chartIsTransposed || series.type === 'Bar')) {
                        const markerWidth = series.marker.mW / 2;
                        mouseX = series.y_Axis.isAxisInverse ? mouseX + markerWidth : mouseX - markerWidth;
                    }
                    else {
                        const markerHeight = series.marker.mH / 2;
                        mouseY = series.y_Axis.isAxisInverse ? mouseY - markerHeight : mouseY + markerHeight;
                    }
                }
                if (series.visible && !sfBlazorToolkit.base.isNullOrUndefined(series.clipRect) && svgbase.withInAreaBounds(mouseX, mouseY, new svgbase.Rect(series.clipRect.x - (!series.chartIsTransposed ? series.x_Axis.plotOffset / 2 : 0), series.clipRect.y - (series.chartIsTransposed ? series.x_Axis.plotOffset / 2 : 0), series.clipRect.w, series.clipRect.h), width, height)) {
                    point = getRectPoint(series, series.clipRect, mouseX, mouseY, chart);
                }
                if (point) {
                    return pointData = {
                        point: point,
                        series: series
                    };
                }
            }
        }
    }
    pointData = {
        point: point,
        series: series
    };
    return pointData;
}

export function getRectPoint(series, rect, x, y, chart) {
    const isRect = isRectSeries(series.seriesType);
    for (const point of series.points) {
        if (!point.rD) {
            if (!point.r || !point.r.length) {
                continue;
            }
        }
        if (series.dataEditSettings && isRect && rectRegion(x, y, point, rect, series, chart)) {
            return point;
        }
        if (checkRegionContainsPoint(point.r, rect, x, y, chart)) {
            return point;
        }
    }
    return null;
}

export function isRectSeries(seriesType) {
    if (seriesType === 'Column' || seriesType === 'StackingColumn' || seriesType === 'StackingColumn100' || seriesType === 'Bar' || seriesType === 'StackingBar' || seriesType === 'StackingBar100') {
        return true;
    }
    else {
        return false;
    }
}

export function rectRegion(x, y, point, rect, series, chart) {
    const isBar = series.type === 'Bar';
    const isInversed = series.y_Axis.isAxisInverse;
    const isTransposed = series.chartIsTransposed;
    const heightValue = 10;
    let yValue = 0;
    let xValue = 0;
    let width;
    let height = width = 2 * heightValue;
    if (isInversed && isTransposed) {
        if (isBar) {
            yValue = point.r[0].h - heightValue;
            width = point.r[0].w;
        }
        else {
            xValue = -heightValue;
            height = point.r[0].h;
        }
    }
    else if (isInversed || point.yV < 0) {
        if (isBar) {
            xValue = -heightValue;
            height = point.r[0].h;
        }
        else {
            yValue = point.r[0].h - heightValue;
            width = point.r[0].w;
        }
    }
    else if (isTransposed) {
        if (isBar) {
            yValue = -heightValue;
            width = point.r[0].w;
        }
        else {
            xValue = point.r[0].w - heightValue;
            height = point.r[0].h;
        }
    }
    else {
        if (isBar) {
            xValue = point.r[0].w - heightValue;
            height = point.r[0].h;
        }
        else {
            yValue = -heightValue;
            width = point.r[0].w;
        }
    }
    return point.r.some((region) => {
        return svgbase.withInAreaBounds(x, y, new svgbase.Rect((chart.userInteractionBase.chartAreaType === 'CartesianAxes' ? rect.x : 0) + region.x + xValue, (chart.userInteractionBase.chartAreaType === 'CartesianAxes' ? rect.y : 0) + region.y + yValue, width, height));
    });
}

export function checkRegionContainsPoint(regionRect, rect, x, y, chart) {
    return regionRect.some((region, index) => {
        chart.userInteractionBase.lierIndex = index;
        return svgbase.withInAreaBounds(x, y, new svgbase.Rect((chart.userInteractionBase.chartAreaType === 'CartesianAxes' ? rect.x : 0) + region.x, (chart.userInteractionBase.chartAreaType === 'CartesianAxes' ? rect.y : 0) + region.y, region.w, region.h));
    });
}

export function getClosestX(chart, series) {
    if (sfBlazorToolkit.base.isNullOrUndefined(series)) {
        return null;
    }
    const rect = series.clipRect;
    let pointData;
    if (rect && chart.mouseX <= rect.x + rect.w && chart.mouseX >= rect.x) {
        const pointValue = getPointValue(chart, series);
        const closestPoint = getClosest(series, pointValue);
        if (!sfBlazorToolkit.base.isNullOrUndefined(closestPoint) && sfBlazorToolkit.base.isNullOrUndefined(closestPoint.y) && series.name === 'BollingerBand') {
            return null;
        }
        else {
            pointData = {
                point: closestPoint,
                series: series
            };
            return pointData;
        }
    }
    return null;
}

export function getPointValue(chart, series) {
    if (!sfBlazorToolkit.base.isNullOrUndefined(series) && !sfBlazorToolkit.base.isNullOrUndefined(series.clipRect)) {
        const pointValue = (!chart.userInteractionBase.isInverted) ? chart.mouseX - series.clipRect.x : chart.mouseY - series.clipRect.y;
        const size = (!chart.userInteractionBase.isInverted) ? series.clipRect.w : series.clipRect.h;
        return getValueByPoint(pointValue, size, series.x_Axis.orientation, series.x_Axis.visibleRange, series.x_Axis.isAxisInverse);
    }
    return 0;
}

export function getValueByPoint(pointValue, size, orientation, visibleRange, isInversed) {
    return ((((orientation === 'Horizontal' && !isInversed) || (orientation !== 'Horizontal' && isInversed)) ? pointValue / size :
        (1 - (pointValue / size))) * visibleRange.delta) + visibleRange.start;
}

export function getClosest(series, pointValue) {
    let closest;
    if (sfBlazorToolkit.base.isNullOrUndefined(series) || sfBlazorToolkit.base.isNullOrUndefined(series.points)) {
        return closest;
    }
    const oneDayMilliseconds = 24 * 60 * 60 * 1000;
    const max = series.xMin + oneDayMilliseconds;
    if (pointValue >= series.xMin - 0.5 && pointValue <= series.xMax + 0.5 && series.points.length > 0) {
        closest = findClosest(sortPoints(series.points, ['xV']), pointValue);
    }
    else if (series.points.length === 1 && series.xMin === series.xMax && pointValue >= series.xMin - 0.5 && pointValue <= max + 0.5) {
        closest = findClosest(sortPoints(series.points, ['xV']), pointValue);
    }
    return closest;
}

export function sortPoints(data, fields, isDescending) {
    const sortData = sfBlazorToolkit.base.extend([], data, null);
    for (let i = 0; i < sortData.length; i++) {
        for (let j = 0; j < fields.length; j++) {
            if (sortData[i][fields[j]] instanceof Date) {
                sortData[i][fields[j]] = sortData[i][fields[j]].getTime();
            }
        }
    }
    sortData.sort((a, b) => {
        let first = 0;
        let second = 0;
        for (let i = 0; i < fields.length; i++) {
            first += a[fields[i]];
            second += b[fields[i]];
        }
        if ((!isDescending && first < second) || (isDescending && first > second)) {
            return -1;
        }
        else if (first === second) {
            return 0;
        }
        return 1;
    });
    return sortData;
}

export function findClosest(pointsData, targetValue) {
    const pointsCount = pointsData.length;
    if (targetValue <= pointsData[0].xV) {
        return pointsData[0];
    }
    if (targetValue >= pointsData[pointsCount - 1].xV) {
        return pointsData[pointsCount - 1];
    }
    let lowIndex = 0;
    let highIndex = pointsCount;
    let midIndex = 0;
    while (lowIndex < highIndex) {
        midIndex = Math.abs((lowIndex + highIndex) / 2);
        if (pointsData[parseInt(midIndex.toString(), 10)].xV === targetValue) {
            return pointsData[parseInt(midIndex.toString(), 10)];
        }
        if (targetValue < pointsData[parseInt(midIndex.toString(), 10)].xV) {
            if (midIndex > 0 && targetValue > pointsData[parseInt(midIndex.toString(), 10) - 1].xV) {
                return getClosestValue(pointsData[parseInt(midIndex.toString(), 10) - 1], pointsData[parseInt(midIndex.toString(), 10)], targetValue);
            }
            highIndex = midIndex;
        }
        else {
            if (midIndex < pointsCount - 1 && targetValue < pointsData[parseInt(midIndex.toString(), 10) + 1].xV) {
                return getClosestValue(pointsData[parseInt(midIndex.toString(), 10)], pointsData[parseInt(midIndex.toString(), 10) + 1], targetValue);
            }
            lowIndex = midIndex + 1;
        }
    }
    return pointsData[parseInt(midIndex.toString(), 10)];
}

export function getClosestValue(point1, point2, target) {
    if (target - point1.xV >= point2.xV - target) {
        return point2;
    }
    else {
        return point1;
    }
}

export function getSharedPoints(chart, pointsInfo) {
    const allPoints = pointsInfo;
    if (!(chart.userInteractionBase.chartAreaType === 'CartesianAxes' && !sfBlazorToolkit.base.isNullOrUndefined(pointsInfo) && pointsInfo.length > 0) || chart.tooltipBase.tooltipModule.showNearestPoint) {
        return pointsInfo;
    }
    let pointValue;
    let closest;
    for (const point of pointsInfo) {
        pointValue = getPointValue(chart, point.series);
        if (isClosest(pointValue, point, closest)) {
            closest = point.point.xV;
        }
        if (isAxisValue(point.series)) {
            return pointsInfo;
        }
    }
    pointsInfo = pointsInfo.filter((item) => closest === item.point.xV);
    if (pointsInfo.length < allPoints.length && chart.tooltipBase.tooltipModule.showNearestPoint) {
        return allPoints;
    }
    else {
        return pointsInfo;
    }
}

export function isClosest(pointValue, point, closest) {
    const x_AxisRenderer = point.series.x_Axis;
    return pointValue >= x_AxisRenderer.visibleRange.start - 0.5 && pointValue <= x_AxisRenderer.visibleRange.end + 0.5 &&
        (isNaN(closest) || Math.abs(point.point.xV - pointValue) < Math.abs(closest - pointValue));
}

export function isAxisValue(series) {
    if (!sfBlazorToolkit.base.isNullOrUndefined(series)) {
        return series.axesCount > 2 && series.x_Axis.name !== 'PrimaryXAxis';
    }
    return false;
}

export function sortPointsInfo(pointsInfo) {
    const points = [];
    pointsInfo.forEach(function (point) {
        if (point.series.category === 'Series') {
            points.push(point);
        }
    });
    pointsInfo.forEach(function (point) {
        if (point.series.category === 'TrendLine') {
            points.push(point);
        }
    });
    return points;
}

export function findHeader(data, chart) {
    let header = chart.tooltipBase.header;
    if (sfBlazorToolkit.base.isNullOrUndefined(header)) {
        return '';
    }
    header = parseTemplate(data.point, data.series, header, chart);
    if (header.replace(/<b>/g, '').replace(/<\/b>/g, '').trim() !== '') {
        return header;
    }
    return '';
}

export function getTooltipText(pointData, chart) {
    return parseTemplate(pointData.point, pointData.series, getFormat(chart, pointData.series), chart);
}

export function parseTemplate(point, series, format, chart) {
    let templateRegex;
    let textValue;
    for (const dataValue of Object.keys(point)) {
        templateRegex = new RegExp('${point' + '.' + dataValue + '}', 'gm');
        if (format.indexOf(templateRegex.source) > -1) {
            format = format.replace(templateRegex.source, formatPointValue(point, dataValue, templateRegex.source === '${point.x}', (templateRegex.source === '${point.h}' ||
                templateRegex.source === '${point.o}' ||
                templateRegex.source === '${point.c}' ||
                templateRegex.source === '${point.l}' ||
                templateRegex.source === '${point.y}'), series, chart));
        }
    }
    for (const dataValue of Object.keys(series)) {
        templateRegex = new RegExp('${series' + '.' + dataValue + '}', 'gm');
        textValue = series[`${dataValue}`];
        if (format.indexOf(templateRegex.source) > -1) {
            format = format.replace(templateRegex.source, textValue);
        }
    }
    return format;
}

export function getTemplateLocation(bounds, symbolLocation, width, height, markerHeight, clipBounds, inverted, isNegative) {
    let location = new IChartInternalLocation(symbolLocation.x, symbolLocation.y);
    const elementWidth = width + 10;
    const elementHeight = height + 10;
    const clipX = clipBounds.x;
    const clipY = clipBounds.y;
    const boundsX = bounds.x;
    const boundsY = bounds.y;
    if (!inverted) {
        location = new IChartInternalLocation(location.x + clipX - (width / 2), location.y + clipY - height - 12 - markerHeight);
        if (location.y < boundsY || isNegative) {
            location.y = (symbolLocation.y < 0 ? 0 : symbolLocation.y) + clipY + markerHeight;
        }
        if (location.y + elementHeight + 12 > boundsY + bounds.height) {
            location.y = (symbolLocation.y > bounds.height ? bounds.height : symbolLocation.y) + clipY - height - 12 - markerHeight;
        }
        if (location.x < boundsX) {
            location.x = boundsX;
        }
        if (location.x + elementWidth > boundsX + bounds.width) {
            location.x -= (location.x + elementWidth) - (boundsX + bounds.width);
        }
    }
    else {
        location = new IChartInternalLocation(location.x + clipX + markerHeight, location.y + clipY - (height / 2));
        if ((location.x + elementWidth + 12 > boundsX + bounds.width) || isNegative) {
            location.x = (symbolLocation.x > bounds.width ? bounds.width : symbolLocation.x) + clipX - markerHeight - (elementWidth + 12);
        }
        if (location.x < boundsX) {
            location.x = (symbolLocation.x < 0 ? 0 : symbolLocation.x) + clipX + markerHeight;
        }
        if (location.y <= boundsY) {
            location.y = boundsY;
        }
        if (location.y + elementHeight >= boundsY + bounds.height) {
            location.y -= (location.y + elementHeight) - (boundsY + bounds.height);
        }
    }
    return { x: location.x, y: location.y };
}

export function formatPointValue(point, dataValue, isXPoint, isYPoint, series, chart) {
    let textValue;
    let customLabelFormat;
    let value;
    const axis = isXPoint ? series.x_Axis : series.y_Axis;
    const format = axis.format ? axis.format : axis.dateFormat;
    const labelFormat = axis.labelFormat;
    if (axis.valueType === 'DateTime' && isXPoint) {
        textValue = new sfBlazorToolkit.base.Internationalization().getDateFormat({ format: format || 'MM/dd/yyyy', type: firstToLowerCase('DateTime') })(convertDateAndTime(new Date(point.xV)));
        textValue = getGlobalizedDate(textValue, chart.dateValuePairs);
    }
    else if (axis.valueType === 'DateTimeCategory' && isXPoint) {
        const date = axis.isUniversalDateTime ? convertDateAndTime(new Date(point[dataValue])) : new Date(point[dataValue]);
        textValue = new sfBlazorToolkit.base.Internationalization().getDateFormat({ format: format || labelFormat || 'MM/dd/yyyy HH:mm:ss tt', type: firstToLowerCase('DateTime') })(date);
        textValue = getGlobalizedDate(textValue, chart.dateValuePairs);
    }
    else if (axis.valueType !== 'Category' && isXPoint) {
        customLabelFormat = labelFormat && labelFormat.match('{value}') !== null;
        textValue = customLabelFormat ? labelFormat.replace('{value}', formatAxisValue(point[dataValue], customLabelFormat, labelFormat, void (0), chart)) :
            formatAxisValue(point[dataValue], customLabelFormat, labelFormat, void (0), chart);
    }
    else if (isYPoint && !sfBlazorToolkit.base.isNullOrUndefined(point[dataValue])) {
        customLabelFormat = labelFormat && labelFormat.match('{value}') !== null;
        if (dataValue === 'outliers') {
            value = formatAxisValue(point[dataValue][lierIndex - 4], customLabelFormat, labelFormat, void (0), chart);
        }
        else {
            const pointValue = point[dataValue];
            const dataLabelFormat = series.markerDataLabelFormat ? parseInt(series.markerDataLabelFormat.substring(1), 10) : axis.valueType === 'Logarithmic' ? 10 : 2;
            const fractionCount = dataLabelFormat;
            value = formatAxisValue(point[dataValue], customLabelFormat, labelFormat, fractionCount, chart);
        }
        value = getGlobalizedNumber(value, chart.numberValuePairs);
        textValue = customLabelFormat ? labelFormat.replace('{value}', value) : value;
    }
    else {
        textValue = !sfBlazorToolkit.base.isNullOrUndefined(point[dataValue]) ? getGlobalizedNumber(point[dataValue].toString(), chart.numberValuePairs) : '';
    }
    return textValue;
}

export function getFormat(chart, series) {
    const separator = (chart.tooltipBase.tooltipModule.shared && !series.name) ? '' : '<br/>';
    if (series.tooltipFormat) {
        return series.tooltipFormat;
    }
    if (chart.tooltipBase.tooltipFormat) {
        return chart.tooltipBase.tooltipFormat;
    }
    const textX = '${point.x}';
    const format = !chart.tooltipBase.tooltipModule.shared ? textX : '${series.name}';
    switch (series.seriesType) {
        case 'XY':
            return format + ' : ' + ((chart.seriesTypes[series.index] === 'Bubble') ? '<b>${point.y}</b>  Size : <b>${point.sI}</b>' : '<b>${point.y}</b>');
        case 'HighLow':
            return format + separator + ('High : <b>${point.h}</b><br/>Low : <b>${point.l}</b>');
        case 'HighLowOpenClose':
            return format + separator + ('High : <b>${point.h}</b><br/>Low : <b>${point.l}</b><br/>' +
                'Open : <b>${point.o}</b><br/>Close : <b>${point.c}</b>');
        case 'BoxPlot': {
            return format + separator + (chart.userInteractionBase.lierIndex > 3 ? 'Outliers : <b>${point.oL}</b>' :
                'Maximum : <b>${point.mX}</b><br/>Q3 : <b>${point.uQ}</b><br/>' +
                'Median : <b>${point.m}</b><br/>Q1 : <b>${point.lQ}</b><br/>Minimum : <b>${point.mI}</b>');
        }
        default: return '';
    }
}

export function formatAxisValue(point, isCustom, format, fractionCount = 2, chart) {
    const pointValue = Number(point);
    if (format.indexOf('{value}') === -1 && format.toLowerCase().indexOf('c') > -1) {
        const match = format.match(/[C](\d*)/i);
        let index;
        if (match) {
            index = match[1] ? parseInt(match[1], 10) : 2;
        }
        const options = {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: index
        };
        const tooltipValue = new Intl.NumberFormat('en-US', options).format(pointValue);
        const numberKeys = Object.keys(chart.numberValuePairs);
        const currencySymbol = tooltipValue.charAt(0);
        const numericPart = tooltipValue.substring(1);
        return numberKeys[numberKeys.length - 1] === 'back' ? `${numericPart}${currencySymbol}` : tooltipValue;
    }
    else {
        return new sfBlazorToolkit.base.Internationalization().getNumberFormat({
            format: isCustom ? '' : format,
            useGrouping: chart.userInteractionBase.useGrouping,
            minimumFractionDigits: fractionCount < 2 ? fractionCount : 2,
            maximumFractionDigits: fractionCount > 20 ? 20 : fractionCount
        })(pointValue);
    }
}

export function findShapes(chart) {
    if (!chart.tooltipBase.tooltipModule.enableMarker) {
        return [];
    }
    const marker = [];
    for (const data of chart.tooltipBase.currentPoints) {
        marker.push(data.series.shape);
    }
    return marker;
}

export function findMarkerHeight(pointData, chart) {
    const series = pointData.series;
    const element = series.category === 'TrendLine' ? getElement(chart.id + '_Series_' + series.index + '_TrendLine_' + series.index + '_PointIndex_' + pointData.point.iX) : getElement(chart.id + '_Series_' + series.index + '_PointIndex_' + pointData.point.iX);
    if (element) {
        return (!isRectSeries(series.type) || series.type === 'Scatter') ? (series.marker.mH + 5) / 2 + 2 * series.marker.b.wT : 0;
    }
    else {
        return ((series.marker.vS || (chart.tooltipBase.tooltipModule.shared &&
            (!isRectSeries(series.type) || series.marker.vS)) || series.type === 'Scatter')) ? ((series.marker.mH + 5) / 2 + (2 * series.marker.b.wT)) : 0;
    }
}

export function findSharedLocation(chart) {
    if (chart.tooltipBase.currentPoints.length > 1) {
        return new IChartInternalLocation(chart.tooltipBase.valueX, chart.tooltipBase.valueY);
    }
    else {
        return getSymbolLocation(chart.tooltipBase.currentPoints[0]);
    }
}

export function valueToCoefficient(point, axis) {
    const result = (point - axis.visibleRange.start) / axis.visibleRange.delta;
    return axis.isAxisInverse ? (1 - result) : result;
}

export function findMouseValue(data, chart) {
    if (!data) {
        return;
    }
    const x_axisRenderer = data.series.x_Axis;
    if (!chart.userInteractionBase.isInverted) {
        chart.tooltipBase.valueX = (data.series.category === 'TrendLine' && chart.tooltipBase.tooltipModule.shared) ? chart.tooltipBase.valueX :
            (valueToCoefficient(data.point.xV, x_axisRenderer) * x_axisRenderer.rect.w) + x_axisRenderer.rect.x;

        chart.tooltipBase.valueY = chart.mouseY;
    }
    else {
        chart.tooltipBase.valueY = ((1 - valueToCoefficient(data.point.xV, x_axisRenderer)) * x_axisRenderer.rect.h) + x_axisRenderer.rect.y;
        chart.tooltipBase.valueX = chart.mouseX;
    }
}

export function getSymbolLocation(data) {
    let location = new IChartInternalLocation(0, 0);
    if (data.point.s.length === 0) {
        return null;
    }
    location = new IChartInternalLocation(data.point.s[0].x, data.point.s[0].y);
    return location;
}

export function findPalette(chart) {
    const colors = [];
    for (const data of chart.tooltipBase.currentPoints) {
        colors.push(findColor(data, data.series));
    }
    return colors;
}

export function findColor(data, series) {
    return (data.point.i !== '' ? data.point.i : (series.marker.f !== '' ? series.marker.f : series.interior));
}

export function renderTooltip(tooltipOptions, elementId, chart) {
    const svgElement = document.getElementById(elementId + '_svg');
    const firstRender = svgElement && parseInt(svgElement.getAttribute('opacity'), 10) > 0 ? false : true;
    const options = tooltipOptions;
    const currentInstance = chart;
    if (firstRender && !sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.tooltip = new svgbase.Tooltip(options);
        currentInstance.tooltip.enableRTL = options.enableRTL;
        currentInstance.tooltip.appendTo('#' + elementId);
    }
    else if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance.tooltip)) {
        currentInstance.tooltip.location = new svgbase.TooltipLocation(options.location.x, options.location.y);
        currentInstance.tooltip.content = options.content;
        currentInstance.tooltip.header = options.header;
        currentInstance.tooltip.offset = options.offset;
        currentInstance.tooltip.palette = options.palette;
        currentInstance.tooltip.shapes = options.shapes;
        currentInstance.tooltip.data = options.data;
        currentInstance.tooltip.template = options.template;
        currentInstance.tooltip.textStyle.color = options.textStyle.color || currentInstance.tooltip.textStyle.color;
        currentInstance.tooltip.textStyle.fontFamily = options.textStyle.fontFamily || currentInstance.tooltip.textStyle.fontFamily;
        currentInstance.tooltip.textStyle.fontStyle = options.textStyle.fontStyle || currentInstance.tooltip.textStyle.fontStyle;
        currentInstance.tooltip.textStyle.fontWeight = options.textStyle.fontWeight || currentInstance.tooltip.textStyle.fontWeight;
        currentInstance.tooltip.textStyle.opacity = options.textStyle.opacity || currentInstance.tooltip.textStyle.opacity;
        currentInstance.tooltip.textStyle.size = options.textStyle.size || currentInstance.tooltip.textStyle.size;
        currentInstance.tooltip.isNegative = options.isNegative;
        currentInstance.tooltip.clipBounds = new svgbase.TooltipLocation(options.clipBounds.x, options.clipBounds.y);
        currentInstance.tooltip.arrowPadding = options.arrowPadding;
        currentInstance.tooltip.dataBind();
    }
}

export function removeTooltip(duration, chart) {
    const tooltipElement = getElement(chart.element.id + '_tooltip');
    stopAnimation(chart.tooltipBase.toolTipInterval);
    if (tooltipElement && sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.template) && chart.tooltipBase.previousPoints.length > 0) {
        chart.tooltipBase.toolTipInterval = +setTimeout(() => {
            if (chart.tooltipBase.tooltipModule && tooltipElement.firstChild) {
                fadeOut(chart.dataId);
            }
        }, duration);
    }
    else if (tooltipElement && !sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.template) && chart.tooltipBase.previousPoints.length > 0) {
        chart.tooltipBase.toolTipInterval = +setTimeout(() => {
            if (!sfBlazorToolkit.base.isNullOrUndefined(chart.tooltipBase.tooltipModule.template)) {
                chart.dotnetref.invokeMethodAsync('RemoveTemplateTooltip');
                chart.tooltipBase.valueX = 0;
                chart.tooltipBase.valueY = 0;
                chart.tooltipBase.currentPoints = [];
                if (!isHighlightRemoved) {
                    removeHighlight(chart);
                    isHighlightRemoved = true;
                }
                removeMarker(chart);
                chart.tooltipBase.previousPoints = [];
            }
        }, duration);
    }
    else if (chart.tooltipBase.previousPoints.length === 0 && chart.markerExplodeBase && chart.markerExplodeBase.markerPreviousPoints.length > 0) {
        chart.tooltipBase.toolTipInterval = +setTimeout(() => {
            removeMarker(chart);
        }, duration);
    }
}

export function stopAnimation(toolTipInterval) {
    stopTimer(toolTipInterval);
}

export function stopTimer(timer) {
    window.clearInterval(timer);
}

export function removeHighlight(chart) {
    for (let _i = 0; _i < chart.tooltipBase.previousPoints.length; _i++) {
        const point = (_i >= 0 && _i < chart.tooltipBase.previousPoints.length) ? chart.tooltipBase.previousPoints[parseInt(_i.toString(), 10)] : undefined;
        if (point) {
            const type = point.series.type;
            if ((isRectSeries(type) || chart.tooltipBase.enableHighlight) && !(chart.selectedDataIndexes.length > 0) && !(chart.highlightDataIndexes.length > 0)) {
                highlightPoint(chart, false, point);
            }
        }
    }
}

export function highlightPoint(chart, highlight, point) {
    const element = getElement(chart.element.id + '_Series_' + point.series.index + '_Point_' + point.point.iX + (point.series.seriesType === 'BoxPlot' ? '_BoxPath' : ''));
    const highlightElement = !isRectSeries(point.series.type) ? getElement(chart.id + '_Series_' + point.series.index) : null;
    const highlightElementBorder = !isRectSeries(point.series.type) ? getElement(chart.id + '_Series_' + point.series.index + '_Border') : null;
    const defaultStrokeWidth = point.series.width;
    const highlightedStrokeWidth = Math.max(point.series.width, point.series.border.wT) + 1;
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor) && chart.highlightColor !== '' && !sfBlazorToolkit.base.isNullOrUndefined(element)) {
        element.setAttribute('fill', (highlight && chart.highlightColor !== 'transparent' ? chart.highlightColor : point.series.interior));
    }
    else if (chart.tooltipBase.enableHighlight && !chart.tooltipBase.tooltipModule.shared) {
        const targetSeries = '_Series_' + point.series.index;
        const elements = document.querySelectorAll(`[id^="${chart.id}_Series_"]`);
        if (highlightElement) {
            animateStrokeWidth(highlightElement, Number(highlightElement.getAttribute('stroke-width')) || defaultStrokeWidth, highlight ? highlightedStrokeWidth : defaultStrokeWidth, chart.tooltipBase.tooltipModule.duration);
        }
        if (highlightElementBorder) {
            animateStrokeWidth(highlightElementBorder, Number(highlightElementBorder.getAttribute('stroke-width')) || defaultStrokeWidth, highlight ? highlightedStrokeWidth : defaultStrokeWidth, chart.tooltipBase.tooltipModule.duration);
        }
        if (!isRectSeries(point.series.type)) {
            const symbolElements = document.querySelectorAll(`[id^='${chart.id}_Series_${point.series.index}_Point_'][id*='_Symbol']`);
            symbolElements.forEach((symbol) => {
                animateStrokeWidth(symbol, Number(symbol.getAttribute('stroke-width')) || defaultStrokeWidth, highlight ? highlightedStrokeWidth : defaultStrokeWidth, chart.tooltipBase.tooltipModule.duration);
            });
        }
        const pointPrefix = `${chart.id}_Series_`;
        const boxPathSuffix = '_BoxPath';
        elements.forEach((element1) => {
            const elementId = element1.id;
            const isPointPattern = elementId.indexOf(pointPrefix) === 0 && /_Point_\d+$/.test(elementId);
            const isBoxPathPattern = elementId.indexOf(pointPrefix) === 0 && elementId.lastIndexOf(boxPathSuffix) === elementId.length - boxPathSuffix.length;
            const isSeriesPattern = elementId.indexOf(pointPrefix) === 0;
            if ((isPointPattern || isBoxPathPattern || isSeriesPattern) &&
                (elementId.indexOf(targetSeries) === -1)) {
                const seriesIndex = (elementId.match(/Series_(\d+)/) || [])[1] ? Number((elementId.match(/Series_(\d+)/) || [])[1]) : -1;
                let series;
                const visibleSeries = chart.userInteractionBase.visibleSeries;
                for (let i = 0; i < visibleSeries.length; i++) {
                    if (visibleSeries[parseInt(i.toString(), 10)].index === seriesIndex) {
                        series = visibleSeries[parseInt(i.toString(), 10)];
                        break;
                    }
                }
                if (series) {
                    const opacity = Math.round(series.opacity / 3 * 10) / 10;
                    if (highlight) {
                        element1.style.opacity = opacity.toString();
                    }
                    else {
                        element1.style.opacity = '';
                        highlightAnimation(element1, seriesIndex, chart.tooltipBase.tooltipModule.duration, opacity, false, chart);
                    }
                }
            }
        });
    }
    else {
        if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
            element.setAttribute('opacity', (highlight ? (point.series.opacity / 2) : point.series.opacity).toString());
        }
    }
}

export function highlightPoints(chart) {
    for (let _i = 0; _i < chart.tooltipBase.currentPoints.length; _i++) {
        const point = (_i >= 0 && _i < chart.tooltipBase.currentPoints.length) ? chart.tooltipBase.currentPoints[parseInt(_i.toString(), 10)] : undefined;
        const type = point.series.type;
        if ((isRectSeries(type) || chart.tooltipBase.enableHighlight) && point.series.category === 'Series' && !(chart.selectedDataIndexes.length > 0) && !(chart.highlightDataIndexes.length > 0)) {
            highlightPoint(chart, true, point);
        }
    }
}

export function animateStrokeWidth(element, startWidth, targetWidth, duration) {
    const animatedElement = element;
    if (animatedElement.animationFrame) {
        cancelAnimationFrame(animatedElement.animationFrame);
    }
    if (targetWidth >= startWidth) {
        animatedElement.setAttribute('stroke-width', targetWidth.toString());
        return;
    }
    let currentWidth = startWidth;
    const step = (startWidth - targetWidth) / (duration / 16);
    /**
        * Animates the stroke-width of the SVG element from the current width
        * down to the targetWidth using requestAnimationFrame.
        * Updates `animatedElement.animationFrame` with the active frame id.
        *
        * @returns {void}
        */
    function animate() {
        currentWidth -= step;
        if (currentWidth <= targetWidth) {
            animatedElement.setAttribute('stroke-width', targetWidth.toString());
            animatedElement.animationFrame = null;
        }
        else {
            animatedElement.setAttribute('stroke-width', currentWidth.toString());
            animatedElement.animationFrame = requestAnimationFrame(animate);
        }
    }
    animatedElement.animationFrame = requestAnimationFrame(animate);
}

export function setEnableHighlight(enableHighlight, dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    chart.tooltipBase.enableHighlight = enableHighlight;
}

export function updatePreviousPoint(chart, extraPoints) {
    if (extraPoints.length > 0) {
        chart.tooltipBase.currentPoints = chart.tooltipBase.currentPoints.concat(extraPoints);
    }
    chart.tooltipBase.previousPoints = [];
    chart.tooltipBase.previousPoints = chart.tooltipBase.previousPoints.concat(chart.tooltipBase.currentPoints);
}

export function fadeOut(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (sfBlazorToolkit.base.isNullOrUndefined(chart) ||
        (!sfBlazorToolkit.base.isNullOrUndefined(chart) && sfBlazorToolkit.base.isNullOrUndefined(chart.tooltip))) {
        return;
    }
    removeTooltipCommentElement(chart);
    chart.tooltipBase.valueX = 0;
    chart.tooltipBase.valueY = 0;
    chart.tooltipBase.currentPoints = [];
    if (!isHighlightRemoved) {
        removeHighlight(chart);
        isHighlightRemoved = true;
    }
    removeMarker(chart);
    chart.tooltipBase.previousPoints = [];
    chart.tooltip.fadeOut();
}

export function removeTooltipCommentElement(chartInstance) {
    const tooltipElementId = chartInstance.tooltip.element ? chartInstance.tooltip.element.id : null;
    const tooltipDivElement = tooltipElementId ? document.getElementById(tooltipElementId) : null;
    if (tooltipDivElement && !chartInstance.isRemoveCommentElement && tooltipDivElement.childNodes.length > 1) {
        const tooltipElements = tooltipDivElement.childNodes;
        const commentElements = [];
        for (let i = 0; i < tooltipElements.length; i++) {
            if (tooltipElements[parseInt(i.toString(), 10)].nodeName.match('#comment') || tooltipElements[parseInt(i.toString(), 10)].nodeName.match('#text')) {
                commentElements.push(tooltipElements[parseInt(i.toString(), 10)]);
            }
        }
        for (const element of commentElements) {
            sfBlazorToolkit.base.remove(element);
            chartInstance.isRemoveCommentElement = true;
        }
    }
}

export function isSeriesAxisZoomed(series) {
    let isAxisZoomed = false;
    if ((series.x_Axis.zoomFactor !== 1 || series.y_Axis.zoomFactor !== 1) || (series.y_Axis.zoomPosition !== 0 || series.x_Axis.zoomPosition !== 0)) {
        isAxisZoomed = true;
    }
    return isAxisZoomed;
}

export function getParentElementBoundsById(id) {
    const element = document.getElementById(id);
    if (element) {
        const svgElement = document.getElementById(id + '_svg');
        if (svgElement) {
            svgElement.style.display = 'none';
        }
        const dataElement = document.getElementById(id + '_tooltip_data');
        if (dataElement) {
            dataElement.style.display = 'none';
        }
        element.style.width = '100%';
        element.style.height = '100%';
        const elementRect = element.getBoundingClientRect();
        const avl_Width = elementRect.width || element.clientWidth || element.offsetWidth;
        const avl_Height = elementRect.height || element.clientHeight || element.offsetHeight;
        if (svgElement) {
            svgElement.style.display = '';
        }
        if (dataElement) {
            dataElement.style.display = 'block';
        }
        return {
            width: avl_Width,
            height: avl_Height,
            left: elementRect.left,
            top: elementRect.top,
            right: elementRect.right,
            bottom: elementRect.bottom
        };
    }
    return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
}

export function getElementBoundsById(id, isSetId = true) {
    if (isSetId) {
        currentId = id;
    }
    const element = document.getElementById(id);
    if (element) {
        const elementRect = element.getBoundingClientRect();
        return {
            width: element.clientWidth || element.offsetWidth,
            height: element.clientHeight || element.offsetHeight,
            left: elementRect.left,
            top: elementRect.top,
            right: elementRect.right,
            bottom: elementRect.bottom
        };
    }
    return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
}

export function getRefreshElementBoundsById(id) {
    const element = document.getElementById(id);
    if (element) {
        const chartCollection = document.getElementsByClassName('e-chart');
        const count = chartCollection.length;
        for (let i = 0; i < count; i++) {
            const chartElement = chartCollection[parseInt(i.toString(), 10)];
            const svgElement = chartElement.querySelector('[id*=_svg]');
            svgElement.style.display = 'none';
        }
        element.style.width = '100%';
        element.style.height = '100%';
        const elementRect = element.getBoundingClientRect();
        const avl_Width = elementRect.width || element.clientWidth || element.offsetWidth;
        const avl_Height = elementRect.height || element.clientHeight || element.offsetHeight;
        for (let i = 0; i < count; i++) {
            const chartElement = chartCollection[parseInt(i.toString(), 10)];
            const svgElement = chartElement.querySelector('[id*=_svg]');
            svgElement.style.display = '';
        }
        return {
            width: avl_Width,
            height: avl_Height,
            left: elementRect.left,
            top: elementRect.top,
            right: elementRect.right,
            bottom: elementRect.bottom
        };
    }
    return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
}

export function getBrowserDeviceInfo() {
    return {
        browserName: sfBlazorToolkit.base.Browser.info.name,
        isPointer: sfBlazorToolkit.base.Browser.isPointer,
        isDevice: sfBlazorToolkit.base.Browser.isDevice,
        isTouch: sfBlazorToolkit.base.Browser.isTouch,
        isIos: sfBlazorToolkit.base.Browser.isIos || sfBlazorToolkit.base.Browser.isIos7
    };
}

export function getAllCharacters() {
    const charCollection = [];
    for (let i = 33; i < 591; i++) {
        charCollection.push(String.fromCharCode(i));
    }
    return charCollection;
}

export function measureText(text, fontWeight, fontStyle, fontFamily) {
    let textObject = document.getElementById('sfchartmeasuretext');
    if (textObject === null) {
        textObject = sfBlazorToolkit.base.createElement('text', { id: 'sfchartmeasuretext' });
        document.body.appendChild(textObject);
    }
    if (text === ' ') {
        text = '&nbsp;';
    }
    textObject.innerHTML = text;
    textObject.style.position = 'fixed';
    textObject.style.setProperty('font-size', '100px', 'important');
    textObject.style.fontWeight = fontWeight;
    textObject.style.fontStyle = fontStyle;
    textObject.style.fontFamily = fontFamily;
    textObject.style.visibility = 'hidden';
    textObject.style.top = '-100';
    textObject.style.left = '0';
    textObject.style.whiteSpace = 'nowrap';
    textObject.style.lineHeight = 'normal';
    return {
        Width: textObject.clientWidth,
        Height: textObject.clientHeight
    };
}

export async function getCharCollectionSize(fontkeys) {
    const tempSizeList = [];
    const charList = getAllCharacters();
    const uniqueFontFamilies = [];
    const seenFamilies = {};
    for (let i = 0; i < fontkeys.length; i++) {
        const fonts = fontkeys[i].split('_');
        if (fonts && fonts.length > 2) {
            const fontFamily = fonts[2];
            if (!Object.prototype.hasOwnProperty.call(seenFamilies, fontFamily)) {
                seenFamilies[fontFamily] = true;
                uniqueFontFamilies.push(fontFamily);
            }
        }
    }
    for (const fontFamily of uniqueFontFamilies) {
        await document.fonts.load(`1em ${fontFamily}`);
    }
    for (let i = 0; i < fontkeys.length; i++) {
        const fontValues = fontkeys[i].split('_');
        const fontWeight = fontValues[0];
        const fontStyle = fontValues[1];
        const fontFamily = fontValues[2];
        for (let j = 0; j < charList.length; j++) {
            const char = charList[j];
            const size = measureText(char, fontWeight, fontStyle, fontFamily).Width;
            tempSizeList.push(size.toString());
        }
    }
    // Return the results as a JSON string
    return JSON.stringify(tempSizeList);
}

export function getCharSizeByFontKeys(fontkeys) {
    const charSizeList = {};
    const fontKeysLength = fontkeys.length;
    let fontValues = [];
    let charSize;
    for (let i = 0; i < fontKeysLength; i++) {
        fontValues = fontkeys[parseInt(i.toString(), 10)].split('_');
        charSize = measureText(fontValues[0], fontValues[1], fontValues[2], fontValues[3]);
        charSizeList[fontkeys[parseInt(i.toString(), 10)]] = { X: charSize.Width, Y: charSize.Height };
    }
    const result = JSON.stringify(charSizeList);
    return result;
}

export function getElement(id) {
    return document.getElementById(id);
}

export function drawTrackBall(svgId, option, tagName, className, clipPath, transform) {
    const parentElement = getElement(svgId);
    if (parentElement) {
        const childElement = document.createElementNS('http://www.w3.org/2000/svg', tagName);
        const keys = Object.keys(option);
        let key = '';
        for (let i = 0; i < keys.length; i++) {
            key = (keys[parseInt(i.toString(), 10)] === 'strokeWidth') ? 'stroke-width' : (keys[parseInt(i.toString(), 10)] === 'strokeDashArray') ?
                'stroke-dashArray' : (keys[parseInt(i.toString(), 10)] === 'direction') ? 'd' : keys[parseInt(i.toString(), 10)];
            childElement.setAttribute(key, option[keys[parseInt(i.toString(), 10)]]);
        }
        childElement.setAttribute('class', className);
        childElement.setAttribute('clip-path', clipPath);
        childElement.setAttribute('transform', transform);
        parentElement.appendChild(childElement);
    }
}

export function removeHighLightedMarker(chart) {
    const elements = document.getElementsByClassName(chart.markerExplodeBase.trackBallClass);
    let i = 0;
    while (i < elements.length) {
        if (elements[parseInt(i.toString(), 10)].id.indexOf(chart.id) !== -1) {
            sfBlazorToolkit.base.remove(elements[parseInt(i.toString(), 10)]);
        }
        else {
            i++;
        }
    }
}

export function setAttribute(id, attribute, value) {
    const element = getElement(id);
    if (element) {
        element.setAttribute(attribute, value);
    }
}

export function createTooltip(id, text, top, left, fontSize) {
    let tooltip = document.getElementById(id);
    const style = 'top:' + ((window.scrollY || 0) + top).toString() + 'px;' +
        'left:' + left.toString() + 'px;' +
        'color:black !important; ' +
        'background:#FFFFFF !important; ' +
        'position:absolute;border:1px solid #707070;font-size:' + fontSize + ';border-radius:2px; z-index:10';
    if (!tooltip) {
        tooltip = sfBlazorToolkit.base.createElement('div', {
            id: id, innerHTML: '&nbsp;' + text + '&nbsp;', styles: style
        });
        document.body.appendChild(tooltip);
    }
    else {
        tooltip.setAttribute('innerHTML', '&nbsp;' + text + '&nbsp;');
        tooltip.setAttribute('styles', style);
    }
}

export function removeElement(id) {
    if (!id) {
        return null;
    }
    const element = getElement(id);
    if (element) {
        sfBlazorToolkit.base.remove(element);
    }
}

export function applySelection(id, color) {
    const elements = document.getElementById(id);
    let childNodes;
    if (elements && elements.childNodes) {
        childNodes = elements.childNodes;
        for (let i = 1, length = childNodes.length; i < length; i++) {
            if (childNodes[parseInt(i.toString(), 10)] && childNodes[parseInt(i.toString(), 10)].tagName !== 'rect' && childNodes[parseInt(i.toString(), 10)].setAttribute) {
                childNodes[parseInt(i.toString(), 10)].setAttribute('fill', color);
            }
        }
    }
}

export function doProgressiveAnimation(id, clipId, duration, delay, strokeDashArray, isFinalSeries, annotations, lastDataLabels) {
    const clipElement = getElement(clipId);
    const path = getElement(id);
    const pathLength = path.getTotalLength();
    let currentTime;
    path.setAttribute('visibility', 'hidden');
    const animationOptions = {
        duration: duration + delay,
        delay: delay,
        progress: function (args) {
            clipElement.setAttribute('visibility', 'visible');
            if (args.timeStamp >= args.delay) {
                path.setAttribute('visibility', 'visible');
                currentTime = Math.abs(Math.round(((args.timeStamp - args.delay) * pathLength) / duration));
                path.setAttribute('stroke-dasharray', currentTime + ',' + pathLength);
            }
        },
        end: function () {
            path.setAttribute('stroke-dasharray', strokeDashArray);
            if (annotations && isFinalSeries) {
                annotations.forEach((annotation) => {
                    annotation.style.visibility = 'visible';
                });
            }
            if (lastDataLabels && isFinalSeries) {
                lastDataLabels.forEach((lastDataLabel) => {
                    lastDataLabel.style.visibility = 'visible';
                });
            }
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(path);
}

export function linear(currentTime, startValue, endValue, duration) {
    return -endValue * Math.cos(currentTime / duration * (Math.PI / 2)) + endValue + startValue;
}

export function doLinearAnimation(markerInfo, id, duration, delay, isInverted, isFinalSeries, annotations, lastDataLabels) {
    const clipRect = getElement(id);
    const effect = linear;
    const elementHeight = +clipRect.getAttribute('height');
    const elementWidth = +clipRect.getAttribute('width');
    const xCenter = +clipRect.getAttribute('x');
    const yCenter = isInverted ? +clipRect.getAttribute('height') + +clipRect.getAttribute('y') :
        +clipRect.getAttribute('y');
    let value;
    let markerValue;
    let xCenterMarker;
    let yCenterMarker;
    let markerClipPath;
    let markerHeight;
    let markerWidth;
    if (!sfBlazorToolkit.base.isNullOrUndefined(markerInfo)) {
        markerClipPath = getElement(markerInfo.markerClipPathId);
        markerHeight = +markerClipPath.getAttribute('height');
        markerWidth = +markerClipPath.getAttribute('width');
        xCenterMarker = +markerClipPath.getAttribute('x');
        yCenterMarker = isInverted ? +markerClipPath.getAttribute('height') + +markerClipPath.getAttribute('y') : +markerClipPath.getAttribute('y');
    }
    const animationOptions = {
        duration: duration + delay,
        delay: delay,
        progress: function (args) {
            if (args.timeStamp >= args.delay) {
                clipRect.setAttribute('visibility', 'visible');
                if (isInverted) {
                    value = effect(args.timeStamp - args.delay, 0, elementHeight, duration);
                    clipRect.setAttribute('transform', 'translate(' + xCenter + ' ' + yCenter +
                        ') scale(1,' + (value / elementHeight) + ') translate(' + (-xCenter) + ' ' + (-yCenter) + ')');
                    if (markerClipPath) {
                        markerClipPath.setAttribute('visibility', 'visible');
                        markerValue = effect(args.timeStamp - args.delay, 0, markerHeight, duration);
                        markerClipPath.setAttribute('transform', 'translate(' + xCenterMarker + ' ' + yCenterMarker + ') scale(1,' + markerValue / markerHeight + ') translate(' + -xCenterMarker + ' ' + -yCenterMarker + ')');
                    }
                }
                else {
                    value = effect(args.timeStamp - args.delay, 0, elementWidth, duration);
                    clipRect.setAttribute('transform', 'translate(' + xCenter + ' ' + yCenter +
                        ') scale(' + (value / elementWidth) + ', 1) translate(' + (-xCenter) + ' ' + (-yCenter) + ')');
                    if (markerClipPath) {
                        markerClipPath.setAttribute('visibility', 'visible');
                        markerValue = effect(args.timeStamp - args.delay, 0, markerWidth, duration);
                        markerClipPath.setAttribute('transform', 'translate(' + xCenterMarker + ' ' + yCenterMarker + ') scale(' + markerValue / markerWidth + ', 1) translate(' + -xCenterMarker + ' ' + -yCenterMarker + ')');
                    }
                }
            }
        },
        end: function () {
            clipRect.setAttribute('transform', 'translate(0,0)');
            if (markerClipPath) {
                markerClipPath.setAttribute('transform', 'translate(0,0)');
            }
            if (annotations && isFinalSeries) {
                annotations.forEach((annotation) => {
                    annotation.style.visibility = 'visible';
                });
            }
            if (lastDataLabels && isFinalSeries) {
                lastDataLabels.forEach((lastDataLabel) => {
                    lastDataLabel.style.visibility = 'visible';
                });
            }
            doStackLabelAnimation(delay);
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(clipRect);
}

export function doStackLabelAnimation(delay) {
    document.querySelectorAll('[id*="_StackLabelCollection"]')
        .forEach((stackLabel) => {
            if (!stackLabel) {
                return;
            }
            const rectElements = stackLabel.querySelectorAll('rect[id*="_StackLabel_"]');
            const textElements = stackLabel.querySelectorAll('text[id*="_StackLabel_"]');
            rectElements.forEach((rect) => {
                animateStackLabelElement(rect, delay);
            });
            textElements.forEach((text) => {
                animateStackLabelElement(text, delay);
            });
            stackLabel.style.visibility = 'visible';
        });
}

export function animateStackLabelElement(element, delay) {
    if (!element) {
        return;
    }
    let scale = 0;
    const isRect = element.tagName.toLowerCase() === 'rect';
    const elementX = parseFloat(element.getAttribute('x') || '0');
    const elementY = parseFloat(element.getAttribute('y') || '0');
    const width = parseFloat(element.getAttribute('width') || '0');
    const height = parseFloat(element.getAttribute('height') || '0');
    const centerX = isRect ? elementX + (width / 2) : elementX;
    const centerY = isRect ? elementY + (height / 2) : elementY;
    element.setAttribute('visibility', 'hidden');
    const animationOptions = {
        duration: 200,
        delay: delay,
        progress: function (args) {
            if (args.timeStamp >= args.delay) {
                element.setAttribute('visibility', 'visible');
                scale = ((args.timeStamp - args.delay) / args.duration);
                element.setAttribute('transform', 'translate(' + centerX
                    + ' ' + centerY + ') scale(' + scale + ') translate(' + (-centerX) + ' ' + (-centerY) + ')');
            }
        },
        end: function () {
            element.setAttribute('visibility', 'visible');
            element.removeAttribute('transform');
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(element);
}

export function filterCommentElement(id) {
    const element = getElement(id);
    if (!element) {
        return null;
    }
    const groupElement = element.childNodes;
    const pathElement = [];
    for (let i = 0; i < groupElement.length; i++) {
        if (!groupElement[parseInt(i.toString(), 10)].nodeName.match('#comment')) {
            pathElement.push(groupElement[parseInt(i.toString(), 10)]);
        }
    }
    return pathElement;
}

export function doInitialAnimation(info, lastAnimationIndex) {
    const annotations = document.querySelectorAll('[id*="_Annotation_"]:not([id*="_Annotation_Collections"])');
    const lastDataLabels = document.querySelectorAll('[id*="LastDataLabelGroup"]:not([id*="LastDataLabelCollection"])');
    for (let infoIndex = 0; infoIndex < info.length; infoIndex++) {
        switch (info[parseInt(infoIndex.toString(), 10)].type) {
            case 'Rect':
                const seriesClipPath = getElement(info[parseInt(infoIndex.toString(), 10)].clipPathId);
                const seriesPathElement = filterCommentElement(info[parseInt(infoIndex.toString(), 10)].elementId);
                for (let i = 0; i < info[parseInt(infoIndex.toString(), 10)].pointIndex.length; i++) {
                    doRectAnimation(seriesPathElement[info[parseInt(infoIndex.toString(), 10)].pointIndex[parseInt(i.toString(), 10)]], seriesClipPath, info[parseInt(infoIndex.toString(), 10)].duration, info[parseInt(infoIndex.toString(), 10)].delay, info[parseInt(infoIndex.toString(), 10)].pointX[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].pointY[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].pointWidth[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].pointHeight[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].isInvertedAxis, infoIndex === lastAnimationIndex, annotations, lastDataLabels);
                }
                break;
            case 'Progressive':
                doProgressiveAnimation(info[parseInt(infoIndex.toString(), 10)].elementId, info[parseInt(infoIndex.toString(), 10)].clipPathId, info[parseInt(infoIndex.toString(), 10)].duration, info[parseInt(infoIndex.toString(), 10)].delay, info[parseInt(infoIndex.toString(), 10)].strokeDashArray, infoIndex === lastAnimationIndex, annotations, lastDataLabels);
                break;
            case 'Linear':
                doLinearAnimation(info[parseInt(infoIndex.toString(), 10)].markerInfo, info[parseInt(infoIndex.toString(), 10)].elementId, info[parseInt(infoIndex.toString(), 10)].duration, info[parseInt(infoIndex.toString(), 10)].delay, info[parseInt(infoIndex.toString(), 10)].isInvertedAxis, infoIndex === lastAnimationIndex, annotations, lastDataLabels);
                break;
            case 'Marker':
                const markerClipPath = getElement(info[parseInt(infoIndex.toString(), 10)].clipPathId);
                const markerElement = filterCommentElement(info[parseInt(infoIndex.toString(), 10)].elementId);
                for (let i = 0; i < info[parseInt(infoIndex.toString(), 10)].pointIndex.length; i++) {
                    doMarkerAnimate(markerElement[info[parseInt(infoIndex.toString(), 10)].pointIndex[parseInt(i.toString(), 10)]], markerClipPath, info[parseInt(infoIndex.toString(), 10)].duration, info[parseInt(infoIndex.toString(), 10)].delay, info[parseInt(infoIndex.toString(), 10)].pointX[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].pointY[parseInt(i.toString(), 10)], infoIndex === lastAnimationIndex, annotations, lastDataLabels);
                }
                break;
        }
        if (info[parseInt(infoIndex.toString(), 10)].type !== 'Linear' && !sfBlazorToolkit.base.isNullOrUndefined(info[parseInt(infoIndex.toString(), 10)].markerInfo)) {
            const markerClipPath = getElement(info[parseInt(infoIndex.toString(), 10)].markerInfo.markerClipPathId);
            const markerElement = filterCommentElement(info[parseInt(infoIndex.toString(), 10)].markerInfo.markerElementId);
            for (let i = 0; i < info[parseInt(infoIndex.toString(), 10)].markerInfo.pointIndex.length; i++) {
                doMarkerAnimate(markerElement[info[parseInt(infoIndex.toString(), 10)].markerInfo.pointIndex[parseInt(i.toString(), 10)]], markerClipPath, 200, info[parseInt(infoIndex.toString(), 10)].duration + info[parseInt(infoIndex.toString(), 10)].delay, info[parseInt(infoIndex.toString(), 10)].markerInfo.pointX[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].markerInfo.pointY[parseInt(i.toString(), 10)]);
                if (info[parseInt(infoIndex.toString(), 10)].markerInfo.lowPointIndex.length > 0) {
                    doMarkerAnimate(markerElement[info[parseInt(infoIndex.toString(), 10)].markerInfo.lowPointIndex[parseInt(i.toString(), 10)]], markerClipPath, 200, info[parseInt(infoIndex.toString(), 10)].duration + info[parseInt(infoIndex.toString(), 10)].delay, info[parseInt(infoIndex.toString(), 10)].markerInfo.lowPointX[parseInt(i.toString(), 10)], info[parseInt(infoIndex.toString(), 10)].markerInfo.lowPointY[parseInt(i.toString(), 10)]);
                }
            }
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(info[parseInt(infoIndex.toString(), 10)].dataLabelInfo)) {
            if (info[parseInt(infoIndex.toString(), 10)].dataLabelInfo.templateId.length === 0) {
                doDataLabelAnimation(info[parseInt(infoIndex.toString(), 10)].dataLabelInfo.shapeGroupId, info[parseInt(infoIndex.toString(), 10)].dataLabelInfo.textGroupId, '', info[parseInt(infoIndex.toString(), 10)].clipPathId, 200, info[parseInt(infoIndex.toString(), 10)].duration + info[parseInt(infoIndex.toString(), 10)].delay);
            }
            else {
                for (let i = 0; i < info[parseInt(infoIndex.toString(), 10)].dataLabelInfo.templateId.length; i++) {
                    doDataLabelAnimation('', '', info[parseInt(infoIndex.toString(), 10)].dataLabelInfo.templateId[parseInt(i.toString(), 10)], '', 200, info[parseInt(infoIndex.toString(), 10)].duration + info[parseInt(infoIndex.toString(), 10)].delay);
                }
            }
        }
    }
}

export function doDynamicAnimation(pathInfo, rectInfo, textInfo, lastlabelInfo) {
    for (let pathIndex = 0; pathIndex < pathInfo.length; pathIndex++) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(pathInfo[parseInt(pathIndex.toString(), 10)].previousDir) && pathInfo[parseInt(pathIndex.toString(), 10)].previousDir !== '' && !sfBlazorToolkit.base.isNullOrUndefined(pathInfo[parseInt(pathIndex.toString(), 10)].currentDir) && pathInfo[parseInt(pathIndex.toString(), 10)].currentDir !== '') {
            pathAnimation(pathInfo[parseInt(pathIndex.toString(), 10)].id, pathInfo[parseInt(pathIndex.toString(), 10)].currentDir, true, pathInfo[parseInt(pathIndex.toString(), 10)].previousDir, 300);
        }
    }
    for (let rectIndex = 0; rectIndex < rectInfo.length; rectIndex++) {
        animateRectElement(rectInfo[parseInt(rectIndex.toString(), 10)].id, 0, 300, rectInfo[parseInt(rectIndex.toString(), 10)].currentRect, rectInfo[parseInt(rectIndex.toString(), 10)].previousRect);
    }
    for (let textIndex = 0; textIndex < textInfo.length; textIndex++) {
        animateRedrawElement(textInfo[parseInt(textIndex.toString(), 10)].id, 300, textInfo[parseInt(textIndex.toString(), 10)].preLocationX, textInfo[parseInt(textIndex.toString(), 10)].preLocationY, textInfo[parseInt(textIndex.toString(), 10)].curLocationX, textInfo[parseInt(textIndex.toString(), 10)].curLocationY, textInfo[parseInt(textIndex.toString(), 10)].x, textInfo[parseInt(textIndex.toString(), 10)].y);
    }
    for (let labelIndex = 0; labelIndex < lastlabelInfo.length; labelIndex++) {
        doAnimateLastDataLabel(lastlabelInfo[parseInt(labelIndex.toString(), 10)].id, lastlabelInfo[parseInt(labelIndex.toString(), 10)].previousTransform, lastlabelInfo[parseInt(labelIndex.toString(), 10)].currentX, lastlabelInfo[parseInt(labelIndex.toString(), 10)].currentY, lastlabelInfo[parseInt(labelIndex.toString(), 10)].duration);
    }
}

export function doRectAnimation(element, clipPathElement, duration, delay, centerX, centerY, elementWidth, elementHeight, isInverted, isFinalSeries, annotations, lastDataLabels) {
    const effect = linear;
    let value;
    if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
        element.setAttribute('visibility', 'hidden');
        const animationOptions = {
            duration: duration,
            delay: delay,
            progress: function (args) {
                clipPathElement.setAttribute('visibility', 'visible');
                if (args.timeStamp >= args.delay) {
                    element.setAttribute('visibility', 'visible');
                    if (!isInverted) {
                        elementHeight = elementHeight ? elementHeight : 1;
                        value = effect(args.timeStamp - args.delay, 0, elementHeight, args.duration);
                        element.setAttribute('transform', 'translate(' + centerX + ' ' + centerY + ') scale(1,' + (value / elementHeight) + ') translate(' + (-centerX) + ' ' + (-centerY) + ')');
                    }
                    else {
                        elementWidth = elementWidth ? elementWidth : 1;
                        value = effect(args.timeStamp - args.delay, 0, elementWidth, args.duration);
                        element.setAttribute('transform', 'translate(' + centerX + ' ' + centerY + ') scale(' + (value / elementWidth) + ', 1) translate(' + (-centerX) + ' ' + (-centerY) + ')');
                    }
                }
            },
            end: function () {
                element.setAttribute('transform', 'translate(0,0)');
                if (annotations && isFinalSeries) {
                    annotations.forEach((annotation) => {
                        annotation.style.visibility = 'visible';
                    });
                }
                if (lastDataLabels && isFinalSeries) {
                    lastDataLabels.forEach((lastDataLabel) => {
                        lastDataLabel.style.visibility = 'visible';
                    });
                }
                doStackLabelAnimation(delay);
            }
        };
        new sfBlazorToolkit.Animation(animationOptions).animate(element);
    }
}

export function doMarkerAnimate(element, clipPathElement, duration, delay, centerX, centerY, isFinalSeries = false, annotations, lastDataLabels) {
    let height = 0;
    if (!element) {
        return;
    }
    element.setAttribute('visibility', 'hidden');
    const animationOptions = {
        duration: duration,
        delay: delay,
        progress: function (args) {
            clipPathElement.setAttribute('visibility', 'visible');
            if (args.timeStamp > args.delay) {
                element.setAttribute('visibility', 'visible');
                height = ((args.timeStamp - args.delay) / args.duration);
                element.setAttribute('transform', 'translate(' + centerX + ' ' + centerY + ') scale(' + height + ') translate(' + (-centerX) + ' ' + (-centerY) + ')');
            }
        },
        end: function () {
            element.setAttribute('visibility', '');
            const dataLabelElement = element.parentElement.parentElement;
            if (dataLabelElement && dataLabelElement.id.indexOf('_DataLabelCollection') === -1) {
                element.removeAttribute('transform');
            }
            if (annotations && isFinalSeries) {
                annotations.forEach((annotation) => {
                    annotation.style.visibility = 'visible';
                });
            }
            if (lastDataLabels && isFinalSeries) {
                lastDataLabels.forEach((lastDataLabel) => {
                    lastDataLabel.style.visibility = 'visible';
                });
            }
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(element);
}

export function templateAnimate(element, delay, duration, name, isRemove, clipElement) {
    const animationOptions = {
        duration: duration,
        delay: delay,
        name: name,
        progress: function (args) {
            if (clipElement) {
                clipElement.setAttribute('visibility', 'visible');
            }
            args.element.style.visibility = 'visible';
        },
        end: (args) => {
            if (isRemove) {
                sfBlazorToolkit.base.remove(args.element);
            }
            else {
                args.element.style.visibility = 'visible';
            }
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(element);
}

export function doDataLabelAnimation(shapeId, textId, tempId, clipId, duration, delay) {
    const shapeElements = filterCommentElement(shapeId);
    const textElements = filterCommentElement(textId);
    const clipPathElement = getElement(clipId);
    const tempElement = getElement(tempId);
    let centerX;
    let centerY;
    const length = tempElement ? 1 : textElements ? textElements.length : 0;
    let element;
    for (let i = 0; i < length; i++) {
        if (tempElement) {
            tempElement.style.visibility = 'hidden';
            templateAnimate(tempElement, delay, duration, 'ZoomIn');
        }
        else {
            element = textElements[parseInt(i.toString(), 10)];
            centerX = (+element.getAttribute('x')) + ((+element.getAttribute('width')) / 2);
            centerY = (+element.getAttribute('y')) + ((+element.getAttribute('height')) / 2);
            doMarkerAnimate(textElements[parseInt(i.toString(), 10)], clipPathElement, duration, delay, centerX, centerY);
            if (shapeElements[parseInt(i.toString(), 10)]) {
                element = shapeElements[parseInt(i.toString(), 10)];
                centerX = (+element.getAttribute('x')) + ((+element.getAttribute('width')) / 2);
                centerY = (+element.getAttribute('y')) + ((+element.getAttribute('height')) / 2);
                doMarkerAnimate(shapeElements[parseInt(i.toString(), 10)], clipPathElement, duration, delay, centerX, centerY);
            }
        }
    }
}

export function doAnimateLastDataLabel(id, previousTransform, targetX, targetY, animateDuration) {
    const element = getElement(id);
    if (sfBlazorToolkit.base.isNullOrUndefined(element)) {
        return null;
    }
    const transformValues = previousTransform.split(/[(),\s]+/);
    const existingTranslateX = parseFloat(transformValues[1]) || 0;
    const existingTranslateY = parseFloat(transformValues[2]) || 0;
    const duration = !sfBlazorToolkit.base.isNullOrUndefined(animateDuration) ? animateDuration : 500;
    const animationOptions = {
        duration: duration,
        progress: function (args) {
            const calculatedTranslateX = linear(args.timeStamp, existingTranslateX, targetX - existingTranslateX, args.duration);
            const calculatedTranslateY = linear(args.timeStamp, existingTranslateY, targetY - existingTranslateY, args.duration);
            const transformValue = `translate(${calculatedTranslateX},${calculatedTranslateY})`;
            element.setAttribute('transform', transformValue);
        },
        end: function () {
            const transformValue = `translate(${targetX},${targetY})`;
            element.setAttribute('transform', transformValue);
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(sfBlazorToolkit.base.createElement('div'));
}

export function pathAnimation(id, direction, redraw, previousDirection, animateDuration) {
    const element = getElement(id);
    if (!redraw || sfBlazorToolkit.base.isNullOrUndefined(element)) {
        return null;
    }
    let duration = 300;
    if (animateDuration) {
        duration = animateDuration;
    }
    const startDirections = previousDirection || element.getAttribute('d');
    const splitDirections = startDirections.split(/(?=[LMCZAQ])/);
    const endDirections = direction.split(/(?=[LMCZAQ])/);
    let currentDireciton;
    let startPath = [];
    let endPath = [];
    let controlIndex;
    let controlEnd;
    element.setAttribute('d', startDirections);
    const animationOptions = {
        duration: duration,
        progress: function (args) {
            currentDireciton = '';
            splitDirections.map((directions, index) => {
                startPath = directions.split(' ');
                endPath = endDirections[parseInt(index.toString(), 10)] ? endDirections[parseInt(index.toString(), 10)].split(' ') : startPath;
                if (startPath[0] === 'Z') {
                    currentDireciton += 'Z' + ' ';
                }
                else {
                    currentDireciton += startPath[0] + ' ' +
                        linear(args.timeStamp, +startPath[1], (+endPath[1] - +startPath[1]), args.duration) + ' ' +
                        linear(args.timeStamp, +startPath[2], (+endPath[2] - +startPath[2]), args.duration) + ' ';
                }
                if (startPath[0] === 'C' || startPath[0] === 'Q') {
                    controlIndex = 3;
                    controlEnd = startPath[0] === 'Q' ? 4 : 6;
                    while (controlIndex < controlEnd) {
                        currentDireciton += linear(args.timeStamp, +startPath[parseInt(controlIndex.toString(), 10)], (+endPath[parseInt(controlIndex.toString(), 10)] - +startPath[parseInt(controlIndex.toString(), 10)]), args.duration)
                            + ' ' + linear(args.timeStamp, +startPath[++controlIndex], (+endPath[parseInt(controlIndex.toString(), 10)] - +startPath[parseInt(controlIndex.toString(), 10)]), args.duration) + ' ';
                        ++controlIndex;
                    }
                }
                if (startPath[0] === 'A') {
                    currentDireciton += 0 + ' ' + 0 + ' ' + 1 + ' ' +
                        linear(args.timeStamp, +startPath[6], (+endPath[6] - +startPath[6]), args.duration) + ' ' +
                        linear(args.timeStamp, +startPath[7], (+endPath[7] - +startPath[7]), args.duration) + ' ';
                }
            });
            element.setAttribute('d', currentDireciton);
        },
        end: function () {
            element.setAttribute('d', direction);
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(sfBlazorToolkit.base.createElement('div'));
}

export function animateRectElement(element, delay, duration, currentRect, previousRect) {
    const setStyle = (rect) => {
        element.setAttribute('x', rect.x + '');
        element.setAttribute('y', rect.y + '');
        element.setAttribute('width', rect.width + '');
        element.setAttribute('height', rect.height + '');
    };
    const animationOptions = {
        duration: duration,
        delay: delay,
        progress: function (args) {
            setStyle(new svgbase.Rect(linear(args.timeStamp, previousRect.x, currentRect.x - previousRect.x, args.duration), linear(args.timeStamp, previousRect.y, currentRect.y - previousRect.y, args.duration), linear(args.timeStamp, previousRect.width, currentRect.width - previousRect.width, args.duration), linear(args.timeStamp, previousRect.height, currentRect.height - previousRect.height, args.duration)));
        },
        end: function () {
            setStyle(currentRect);
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(sfBlazorToolkit.base.createElement('div'));
}

export function animateRedrawElement(elementId, duration, startX, startY, endX, endY, x = 'x', y = 'y') {
    const element = getElement(elementId);
    if (!element) {
        return null;
    }
    const isDiv = element.tagName === 'DIV';
    const setStyle = (xValue, yValue) => {
        if (isDiv) {
            element.style[`${x}`] = xValue + 'px';
            element.style[`${y}`] = yValue + 'px';
        }
        else {
            element.setAttribute(x, xValue + '');
            element.setAttribute(y, yValue + '');
        }
    };
    setStyle(startX, startY);
    const animationOptions = {
        duration: duration,
        progress: function (args) {
            setStyle(linear(args.timeStamp, startX, endX - startX, args.duration), linear(args.timeStamp, startY, endY - startY, args.duration));
        },
        end: function () {
            setStyle(endX, endY);
        }
    };
    new sfBlazorToolkit.Animation(animationOptions).animate(sfBlazorToolkit.base.createElement('div'));
}

export function appendChildElement(parent, childElement, redraw, isAnimate = false, x = 'x', y = 'y', start, direction, forceAnimate = false, isRect = false, previousRect = null, animateDuration) {
    const existChild = parent.querySelector('#' + childElement.id);
    const element = existChild || getElement(childElement.id);
    const child = childElement;
    const duration = animateDuration ? animateDuration : 300;
    if (redraw && isAnimate && element) {
        start = start || (element.tagName === 'DIV' ?
            new svgbase.TooltipLocation(+(element.style[`${x}`].split('px')[0]), +(element.style[`${y}`].split('px')[0])) :
            new svgbase.TooltipLocation(+element.getAttribute(x), +element.getAttribute(y)));
        if (direction !== '' && !sfBlazorToolkit.base.isNullOrUndefined(direction)) {
            pathAnimation(childElement, childElement.getAttribute('d'), redraw, direction, duration);
        }
        else if (isRect && previousRect) {
            animateRectElement(child, 0, duration, new svgbase.Rect(+element.getAttribute('x'), +element.getAttribute('y'), +element.getAttribute('width'), +element.getAttribute('height')), previousRect);
        }
        else {
            const end = child.tagName === 'DIV' ?
                new svgbase.TooltipLocation(+(child.style[`${x}`].split('px')[0]), +(child.style[`${y}`].split('px')[0])) :
                new svgbase.TooltipLocation(+child.getAttribute(x), +child.getAttribute(y));
            animateRedrawElement(child, duration, start, end, x, y);
        }
    }
    else if (redraw && isAnimate && !element && forceAnimate) {
        templateAnimate(child, 0, 600, 'FadeIn');
    }
}

export function isLassoId(x, y) {
    const lassoEle = document.elementFromPoint(x, y);
    return lassoEle ? lassoEle.id : '';
}

export function getTemplateSize(id) {
    const element = getElement(id);
    if (element) {
        return {
            width: element.offsetWidth,
            height: element.offsetHeight
        };
    }
    return null;
}

export function chartResize(dotnetrefCollection) {
    if (tooltipModule) {
        removeTooltip(100, this);
        removeCrosshair(this, 100);
    }
    tooltipModule = null;
    crosshair = null;
    markerExploded = false;
    if (resize) {
        clearTimeout(resize);
    }
    if (!this.isDisposed) {
        resize = setTimeout(() => {
            const count = dotnetrefCollection.length;
            let tempDotnetref;
            let id;
            let element;
            const offsetSizeCollection = {};
            for (let i = 0; i < count; i++) {
                tempDotnetref = dotnetrefCollection[parseInt(i.toString(), 10)].dotnetref;
                id = dotnetrefCollection[parseInt(i.toString(), 10)].id;
                if (document.getElementById(id + '_svg')) {
                    element = document.getElementById(id + '_svg');
                    element.style.display = 'none';
                }
            }
            for (let i = 0; i < count; i++) {
                tempDotnetref = dotnetrefCollection[parseInt(i.toString(), 10)].dotnetref;
                id = dotnetrefCollection[parseInt(i.toString(), 10)].id;
                if (document.getElementById(id)) {
                    element = document.getElementById(id);
                    offsetSizeCollection[`${id}`] = { Dotnetref: tempDotnetref, Width: element.clientWidth || element.offsetWidth, Height: element.clientHeight || element.offsetHeight };
                }
            }
            for (const key in offsetSizeCollection) {
                if (offsetSizeCollection[`${key}`]) {
                    tempDotnetref = offsetSizeCollection[`${key}`].Dotnetref;
                    element = document.getElementById(key + '_svg');
                    element.style.display = '';
                    tempDotnetref.invokeMethodAsync('OnChartResize', JSON.stringify({ Width: offsetSizeCollection[`${key}`].Width, Height: offsetSizeCollection[`${key}`].Height }));
                }
            }
            clearTimeout(resize);
        }, 500);
    }
    return false;
}

export function getDatalabelTemplateSize(templateIdCollection) {
    setDatalabelAnnotationCollections(currentId);
    const templateSizeList = [];
    let templateSize;
    const templateIdLength = templateIdCollection.length;
    for (let i = 0; i < templateIdLength; i++) {
        templateSize = getElementBoundsById(templateIdCollection[parseInt(i.toString(), 10)], false);
        if (templateSize != null) {
            templateSizeList.push({ X: templateSize.width, Y: templateSize.height });
        }
        else {
            templateSizeList.push({ X: 0, Y: 0 });
        }
    }
    return JSON.stringify(templateSizeList);
}

export function getAxisLabelTemplatesSize(axisTemplateIdCollection) {
    setDatalabelAnnotationCollections(currentId);
    const axisTemplateSizeList = [];
    const templateIdLength = axisTemplateIdCollection.length;
    let axisTemplateSize;
    for (let i = 0; i < templateIdLength; i++) {
        const templateId = axisTemplateIdCollection[i];
        const element = document.getElementById(templateId);
        if (!element || !element.parentNode) {
            axisTemplateSizeList.push({ Width: 0, Height: 0 });
            continue;
        }
        const bounds = element.getBoundingClientRect();
        axisTemplateSize = { width: bounds.width, height: bounds.height };
        axisTemplateSizeList.push({ Width: axisTemplateSize.width, Height: axisTemplateSize.height });
    }
    return JSON.stringify(axisTemplateSizeList);
}

export function setSvgDimensions(chartSVG, width, height, focusable, isFocused) {
    if (!chartSVG) {
        return;
    }
    chartSVG.setAttribute('width', width);
    chartSVG.setAttribute('height', height);
    if (focusable && !isFocused) {
        let className = chartSVG.parentElement.getAttribute('class');
        if (className && className.indexOf('e-chart-focused') === -1) {
            className = className + ' e-chart-focused';
        }
        else if (!className) {
            className = 'e-chart-focused';
        }
        chartSVG.parentElement.setAttribute('class', className + ' e-chart-focused');
    }
    setDatalabelAnnotationCollections(chartSVG.id.replace('_svg', ''));
}

export function setDatalabelAnnotationCollections(id) {
    const svgRect = document.getElementById(id + '_svg').getBoundingClientRect();
    const rect = document.getElementById(id).getBoundingClientRect();    
    const left = Math.max(svgRect.left - rect.left, 0) + 'px';
    const top = Math.max(svgRect.top - rect.top, 0) + 'px';

    const dataLabels = document.querySelectorAll('[id*="_DataLabelCollections"]');
    const annotations = document.querySelectorAll('[id="' + id + '_Annotation_Collections"]');
    const axisLabels = document.querySelectorAll('[id*="_AxisLabelTemplate_Collections"]');
    const legends = document.querySelectorAll('[id="' + id + '_ChartSeries_LegendTemplateCollection"]');

    if (dataLabels && dataLabels.length > 0) {
        dataLabels.forEach(function (element) {
            sfBlazorToolkit.base.setStyleAttribute(element, { position: 'relative', left: left, top: top });
        });
    }
    if (annotations && annotations.length > 0) {
        annotations.forEach(function (element) {
            sfBlazorToolkit.base.setStyleAttribute(element, { position: 'relative', left: left, top: top });
        });
    }
    if (axisLabels && axisLabels.length > 0) {
        axisLabels.forEach(function (element) {
            sfBlazorToolkit.base.setStyleAttribute(element, { position: 'relative', left: left, top: top });
        });
    }
    if (legends && legends.length > 0) {
        legends.forEach(function (element) {
            sfBlazorToolkit.base.setStyleAttribute(element, { position: 'relative', left: left, top: top });
        });
    }
}

export function findDOMElement(id) {
    return document.getElementById(id);
}

export function isChartPanning(chart) {
    return chart && chart.zoomBase && chart.zoomBase.zoomingModule && chart.zoomBase.zoomingModule.isPanning;
}

export function calculateSelectedElements(e, dataId) {
    if (sfBlazorToolkit.base.isNullOrUndefined(e.target)) {
        return;
    }
    const charts = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (isChartPanning(charts)) {
        return;
    }
    const parentId = charts.getLegendTemplateParentId(e);
    const parentTarget = findDOMElement(parentId);
    let target = (e.target);
    if (!sfBlazorToolkit.base.isNullOrUndefined(parentTarget)) {
        target = parentTarget;
    }
    target = findDOMElement(findDOMElementFromDataLabel(target.id, charts));
    if ((charts.selectionMode === 'None' && charts.highlightMode === 'None') || (!sfBlazorToolkit.base.isNullOrUndefined(target) && target.id.indexOf(charts.element.id + '_') === -1)) {
        return;
    }
    if (e.type === 'mousemove' || e.type === 'pointermove') {
        if (sfBlazorToolkit.base.isNullOrUndefined(target) && target.id.indexOf('Trackball') > -1) {
            target = findDOMElement(target.id.split('_Trackball')[0]);
        }
        if (target.hasAttribute('class') && (target.getAttribute('class').indexOf('highlight') > -1 ||
            target.getAttribute('class').indexOf('selection') > -1)) {
            return;
        }
    }
    else if (e.type === 'click' && !sfBlazorToolkit.base.isNullOrUndefined(target) && target.id.indexOf('legend') > -1 && !charts.toggleVisibility && charts.selectionMode !== 'Series') {
        return;
    }
    isAlreadySelected(e, charts);
    if (!sfBlazorToolkit.base.isNullOrUndefined(target) && target.id.indexOf('_Series_') > -1) {
        let element;
        if (target.id.indexOf('_Trackball_1') > -1) {
            element = findDOMElement(target.id.split('_Trackball_')[0] + '_Symbol');
            element = sfBlazorToolkit.base.isNullOrUndefined(element) ? findDOMElement(target.id.split('_Trackball_')[0]) : element;
        }
        else if (target.id.indexOf('_Trackball_0') > -1 || target.id.indexOf('_ErrorBarGroup_') > -1 || target.id.indexOf('_ErrorBarCap_') > -1) {
            return null;
        }
        performSelection(indexFinder(target.id), charts, element || target);
    }
}

export function generateStyle(targetId, chart) {
    let dataPoint = !sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(targetId)) ? findDOMElement(targetId).getAttribute('data-point') : null;
    let parentElement;
    if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(targetId))) {
        if (targetId.indexOf('SeriesGroup') > -1 || targetId.indexOf('SymbolGroup') > -1) {
            parentElement = document.getElementById(targetId).querySelectorAll('path,ellipse');
            dataPoint = parentElement.length > 0 ? parentElement[0].getAttribute('data-point') : null;
        }
    }
    if (!dataPoint) {
        return 'undefined';
    }
    const dataPointArray = dataPoint.split(',');
    const seriesType = dataPointArray[3];
    if (seriesType) {
        if (chart.styleId && chart.styleId.indexOf('selection') > 1 && chart.selectionMode !== 'None') {
            chart.unSelected = dataPointArray[7] || chart.unSelected;
        }
        if (chart.styleId && chart.styleId.indexOf('highlight') > 0 && (chart.highlightMode !== 'None' || chart.enableHighlight)) {
            chart.unSelected = dataPointArray[5] || chart.unSelected;
        }
        return (dataPointArray[6] || chart.styleId + '_series' + dataPointArray[4]);
    }
    return 'undefined';
}

export function findDOMElementFromDataLabel(id, charts) {
    if ((id.indexOf('Text') > -1) && (id.indexOf('Series') > -1)) {
        const primId = id.split('_Text_')[0];
        if (isRectSeries(charts.seriesTypes[id.split('_Series_')[1].split('_')[0]]) || charts.seriesTypes[0] === 'Bubble' || charts.seriesTypes[0] === 'Scatter') {
            return primId;
        }
        else {
            return primId.slice(0, primId.length) + '_Symbol ';
        }
    }
    return id;
}

export function indexFinder(id) {
    let parsedIndices = ['NaN', 'NaN'];
    if (id.indexOf('_Point_') > -1) {
        parsedIndices = id.split('_Series_')[1].split('_Point_');
        if (parsedIndices[1].indexOf('Text') > -1) {
            parsedIndices[1] = parsedIndices[1].split('_Text_')[0];
        }
    }
    else if (id.indexOf('_shape_') > -1 && !id.indexOf('_legend_')) {
        parsedIndices = id.split('_shape_');
        parsedIndices[0] = parsedIndices[1];
    }
    else if (id.indexOf('_text_') > -1 && !id.indexOf('_legend_')) {
        parsedIndices = id.split('_text_');
        if (id.indexOf('datalabel') > -1) {
            parsedIndices[0] = parsedIndices[0].split('_Series_')[1];
        }
        else {
            parsedIndices[0] = parsedIndices[1];
        }
    }
    else if (id.indexOf('Series') > -1 && !(id.indexOf('points') > -1) && !id.indexOf('_legend_')) {
        parsedIndices[0] = id.split('_Series_')[1].split('_')[0];
    }
    return new Index(parseInt(parsedIndices[0], 10), parseInt(parsedIndices[1], 10));
}

export function addOrRemoveIndex(indexes, index, charts, isAdd) {
    for (let i = 0; i < indexes.length; i++) {
        if (toEquals(indexes[parseInt(i.toString(), 10)], index, charts.isSeriesMode, charts)) {
            indexes.splice(i, 1);
            i--;
        }
    }
    if (isAdd) {
        indexes.push(index);
    }
}

export function toEquals(first, second, checkSeriesOnly, chart) {
    return ((first.series === second.series || (chart.currentMode === 'Cluster' && !checkSeriesOnly))
        && (checkSeriesOnly || (first.point === second.point)));
}

export function removeSvgClass(element, className) {
    const elementClassName = !sfBlazorToolkit.base.isNullOrUndefined(element) && !sfBlazorToolkit.base.isNullOrUndefined(element.getAttribute('class')) ? element.getAttribute('class') : '';
    if (elementClassName.indexOf(className) > -1) {
        element.setAttribute('class', elementClassName.replace(className, ''));
    }
}

export function getSelectionClass(id, chart) {
    return generateStyle(id, chart);
}

export function removeStyles(elements, chart) {
    for (const element of elements) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
            const targetElement = element;
            let id = element.id;
            const dataPoint = findDOMElement(targetElement.id).getAttribute('data-point');
            const dataPointArray = dataPoint ? dataPoint.split(',') : [];
            if (targetElement.id.indexOf('_chart_legend_shape_') > -1) {
                id = chart.element.id + 'SeriesGroup' + chart.currentSeriesIndex;
            }
            removeSvgClass(element, getSelectionClass(id, chart));
            if (chart.highlightPattern === 'None' && chart.highlightColor !== '' && !sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor)) {
                if (element.id.indexOf('Group') > 0) {
                    for (let i = 0; i < element.children.length; i++) {
                        element.children[parseInt(i.toString(), 10)].setAttribute('fill', dataPointArray[2]);
                    }
                }
                else {
                    element.setAttribute('fill', dataPointArray[2]);
                }
            }
        }
    }
}

export function findTrackballElements(selectedElements, className, chart) {
    className = className.trim();
    let trackballElements;
    let elements;
    for (let i = 0; i < selectedElements.length; i++) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(selectedElements[parseInt(i.toString(), 10)])) {
            trackballElements = !sfBlazorToolkit.base.isNullOrUndefined(selectedElements[parseInt(i.toString(), 10)].parentNode) ?
                [].slice.call(selectedElements[0].parentNode.querySelectorAll('.' + className)) : [];
            if (trackballElements.length > 0) {
                elements = [];
                for (let i = 0; i < trackballElements.length; i++) {
                    if (trackballElements[parseInt(i.toString(), 10)].id.indexOf('Trackball') > -1) {
                        elements.push(trackballElements[parseInt(i.toString(), 10)]);
                    }
                }
                removeStyles(elements, chart);
            }
        }
    }
}

export function blurEffect(chartId, charts, isLegend = false) {
    const visibility = (checkVisibility(charts.highlightDataIndexes, charts) ||
        checkVisibility(charts.selectedDataIndexes, charts));
    const seriesCollection = document.getElementById(charts.element.id + 'SeriesCollection').querySelectorAll('[id*="SeriesGroup"]');
    for (let i = 0; i < seriesCollection.length; i++) {
        const index = parseInt(seriesCollection[parseInt(i.toString(), 10)].id.split('SeriesGroup')[1], 10);
        checkSelectionElements(findDOMElement(charts.element.id + 'SeriesGroup' + index), generateStyle(charts.element.id + 'SeriesGroup' + index, charts), visibility, isLegend, index, charts);
        if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(charts.element.id + 'SymbolGroup' + index))) {
            checkSelectionElements(findDOMElement(charts.element.id + 'SymbolGroup' + index), generateStyle(charts.element.id + 'SymbolGroup' + index, charts), visibility, isLegend, index, charts);
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(charts.element.id + 'TextGroup' + index)) && isLegend) {
            checkSelectionElements(findDOMElement(charts.element.id + 'TextGroup' + index), generateStyle(charts.element.id + 'TextGroup' + index, charts), visibility, isLegend, index, charts);
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(charts.element.id + 'ShapeGroup' + index)) && isLegend) {
            checkSelectionElements(findDOMElement(charts.element.id + 'ShapeGroup' + index), generateStyle(charts.element.id + 'ShapeGroup' + index, charts), visibility, isLegend, index, charts);
        }
    }
}

export function checkVisibility(selectedIndexes, chart) {
    if (!selectedIndexes) {
        return false;
    }
    let visible = false;
    const uniqueSeries = [];
    for (const index of selectedIndexes) {
        if (uniqueSeries.indexOf(index.series) === -1) {
            uniqueSeries.push(index.series);
        }
    }
    for (const index of uniqueSeries) {
        const seriesElement = document.getElementById(chart.element.id + 'SeriesGroup' + index);
        const isSeriesVisible = chart.seriesTypes[parseInt(index.toString(), 10)].indexOf('Stacking') > -1 ? !sfBlazorToolkit.base.isNullOrUndefined(seriesElement) && !sfBlazorToolkit.base.isNullOrUndefined(seriesElement.getAttribute('data-point')) && seriesElement.getAttribute('data-point').split(/(?![^(]*\)),/)[8] === 'True' : !sfBlazorToolkit.base.isNullOrUndefined(seriesElement);
        if (!sfBlazorToolkit.base.isNullOrUndefined(seriesElement) && isSeriesVisible) {
            visible = true;
            break;
        }
    }
    return visible;
}

export function findElements(chart, series, index, suffix = '', marker) {
    if (chart.isSeriesMode) {
        return getSeriesElements(index.series.toString(), chart);
    }
    else if (chart.currentMode === 'Cluster') {
        return getClusterElements(chart, index);
    }
    else {
        return findDOMElementByIndex(chart, index, suffix, marker);
    }
}

export function removeMultiSelectElements(chart, index, currentIndex) {
    let series;
    for (let i = 0; i < index.length; i++) {
        series = chart.seriesTypes[index[parseInt(i.toString(), 10)].series];
        if (!sfBlazorToolkit.base.isNullOrUndefined(series) && ((chart.isSeriesMode && !toEquals(index[parseInt(i.toString(), 10)], currentIndex, chart.isSeriesMode, chart)) ||
            (chart.currentMode === 'Cluster' && !toEquals(index[parseInt(i.toString(), 10)], currentIndex, false, chart)) ||
            (!chart.isSeriesMode && toEquals(index[parseInt(i.toString(), 10)], currentIndex, true, chart) && !toEquals(index[parseInt(i.toString(), 10)], currentIndex, false, chart)))) {
            removeStyles(findElements(chart, series, index[parseInt(i.toString(), 10)], '', false), chart);
            if (chart.element.id + 'SymbolGroup' + index[parseInt(i.toString(), 10)].series) {
                removeStyles(findElements(chart, series, index[parseInt(i.toString(), 10)], '', true), chart);
            }
            index.splice(i, 1);
            i--;
        }
    }
}

export function checkSelectionElements(element, className, visibility, isLegend = false, series, chart) {
    let children = [];
    className = className.trim();
    if (element.id.indexOf('Series') > -1) {
        if (pointIdRequired(chart.seriesTypes[parseInt(series.toString(), 10)])) {
            children = (chart.isSeriesMode ? [element] : element.querySelectorAll('[id*="_Series"]'));
        }
        else {
            children = (chart.isSeriesMode ? [element] : element.querySelectorAll('[id*="_Series"]'));
        }
    }
    if (chart.selectionMode !== 'None' || (chart.highlightMode !== 'None' || chart.enableHighlight)) {
        children = element.querySelectorAll('[id*="_Series"]');
    }
    let elementClassName;
    let parentClassName;
    let selectElement = element;
    const parentElement = !isRectSeries(chart.seriesTypes[parseInt(series.toString(), 10)]) ? findDOMElement(chart.element.id + '_Series_' + series) : !sfBlazorToolkit.base.isNullOrUndefined(chart.element.id + 'TextGroup' + series) && isLegend ? findDOMElement(chart.element.id + '_Series_' + series + '_Point_' + series) : null;
    for (let i = 0; i < children.length; i++) {
        elementClassName = children[parseInt(i.toString(), 10)].getAttribute('class') || '';
        parentClassName = !sfBlazorToolkit.base.isNullOrUndefined(parentElement) ? (parentElement.getAttribute('class') ? parentElement.getAttribute('class') : '') : '';
        if (chart.selectionMode !== 'None' || (chart.highlightMode !== 'None' || chart.enableHighlight)) {
            className = (!sfBlazorToolkit.base.isNullOrUndefined(elementClassName) && (elementClassName.indexOf('selection') > 0 ||
                elementClassName.indexOf('highlight') > 0)) ? elementClassName : className;
            className = (!sfBlazorToolkit.base.isNullOrUndefined(elementClassName) && (parentClassName.indexOf('selection') > 0 ||
                parentClassName.indexOf('highlight') > 0)) ? parentClassName : className;
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(elementClassName) && !sfBlazorToolkit.base.isNullOrUndefined(parentClassName) && elementClassName.indexOf(className) === -1 &&
            parentClassName.indexOf(className) === -1 && visibility) {
            addSvgClass(children[parseInt(i.toString(), 10)], chart.unSelected);
        }
        else {
            selectElement = children[parseInt(i.toString(), 10)];
            removeSvgClass(children[parseInt(i.toString(), 10)], chart.unSelected);
            removeSvgClass(parentElement, chart.unSelected);
            if (chart.tooltipBase.enableHighlight && isLegend) {
                const element = children[parseInt(i.toString(), 10)];
                element.style.opacity = '';
            }
        }
        if (children[parseInt(i.toString(), 10)].id.indexOf('Trackball') > 0 && selectElement.classList[0] === className) {
            removeSvgClass(children[parseInt(i.toString(), 10)], chart.unSelected);
            removeSvgClass(parentElement, chart.unSelected);
            addSvgClass(children[parseInt(i.toString(), 10)], className);
        }
    }
    if (element.id.indexOf('Symbol') > -1) {
        const validClassName = className.trim().split(/\s+/).map((c) => `.${c}`).join('');
        const elements = element.querySelectorAll(validClassName);
        if (elements.length > 0 && elements[0].getAttribute('class') === className) {
            const symbolEle = findDOMElement(chart.element.id + '_Series_' + element.id[element.id.length - 1]);
            const seriesClassName = symbolEle && symbolEle.hasAttribute('class') ? symbolEle.getAttribute('class') : '';
            if (seriesClassName.indexOf(chart.unSelected) > -1) {
                removeSvgClass(symbolEle, chart.unSelected);
            }
        }
    }
    let dataPoint = findDOMElement(element.id).getAttribute('data-point');
    let parentElementDp;
    if (element.id.indexOf('SeriesGroup') > -1 || element.id.indexOf('SymbolGroup') > -1) {
        parentElementDp = document.getElementById(element.id).querySelectorAll('path,ellipse');
        dataPoint = parentElementDp.length > 0 ? parentElementDp[0].getAttribute('data-point') : null;
    }
    if (!dataPoint) {
        return;
    }
    const dataPointArray = dataPoint.split(',');
    const legendShape = findDOMElement(chart.id + '_chart_legend_shape_' + series);
    if (legendShape) {
        if (legendShape.hasAttribute('class')) {
            removeSvgClass(legendShape, legendShape.getAttribute('class'));
            if (!sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor && chart.highlightColor !== '')) {
                legendShape.setAttribute('stroke', dataPointArray[2]);
                if (chart.highlightPattern === 'None') {
                    legendShape.setAttribute('fill', dataPointArray[2]);
                }
            }
        }
        elementClassName = selectElement.getAttribute('class') || '';
        parentClassName = selectElement.parentNode.getAttribute('class') || '';
        if (elementClassName.indexOf(className) === -1 && parentClassName.indexOf(className) === -1 && visibility) {
            addSvgClass(legendShape, chart.unSelected);
            removeSvgClass(legendShape, className);
            if (chart.highlightColor !== '' && !sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor)) {
                legendShape.setAttribute('stroke', dataPointArray[2]);
                if (chart.highlightPattern === 'None') {
                    legendShape.setAttribute('fill', dataPointArray[2]);
                }
            }
        }
        else {
            removeSvgClass(legendShape, chart.unSelected);
            if (!sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor) && chart.highlightColor !== '') {
                legendShape.setAttribute('stroke', dataPointArray[2]);
                if (chart.highlightPattern === 'None') {
                    legendShape.setAttribute('fill', dataPointArray[2]);
                }
            }
            if (elementClassName.trim() === 'e-chart-focused') {
                elementClassName = '';
            }
            if ((elementClassName === '' && parentClassName === '') || elementClassName.trim() === 'EJ2-Trackball') {
                removeSvgClass(legendShape, className);
            }
            else {
                addSvgClass(legendShape, className);
                if (className.indexOf('highlight') > 0 && chart.highlightColor !== '' && !sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor)) {
                    legendShape.setAttribute('stroke', chart.highlightColor);
                    if (chart.styleId && chart.styleId.indexOf('highlight') > 0 && chart.highlightPattern === 'None') {
                        legendShape.setAttribute('fill', chart.highlightColor);
                    }
                }
            }
        }
        if (isLegend && parentClassName.indexOf(className) > -1) {
            addSvgClass(legendShape, className);
        }
    }
}

export function addSvgClass(element, className) {
    let elementClassName = element.getAttribute('class') || '';
    elementClassName += ((elementClassName !== '') ? ' ' : '');
    if (elementClassName.indexOf(className) === -1) {
        element.setAttribute('class', elementClassName + className);
    }
}

export function pointIdRequired(seriesType) {
    if (isRectSeries(seriesType) || seriesType === 'Bubble' || seriesType === 'Scatter') {
        return true;
    }
    else {
        return false;
    }
}

export function getSeriesElements(index, chart, isLegend = false) {
    let seriesElements;
    let seriesElementsArray = [];
    const isMarker = !sfBlazorToolkit.base.isNullOrUndefined(document.getElementById(chart.element.id + 'SymbolGroup' + index)) && chart.seriesTypes[parseInt(index.toString(), 10)] !== 'Scatter' && chart.seriesTypes[parseInt(index.toString(), 10)] !== 'Bubble' && !isRectSeries(chart.seriesTypes[parseInt(index.toString(), 10)]);
    if (isMarker) {
        seriesElements = findDOMElement(chart.element.id + 'SymbolGroup' + index).querySelectorAll('#' + chart.element.id + 'SymbolGroup' + index + ' path');
        seriesElementsArray = addSeriesElements(seriesElementsArray, seriesElements);
        if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(chart.element.id + 'SymbolGroup' + index).querySelectorAll('#' + chart.element.id + 'SymbolGroup' + index + ' ellipse'))) {
            seriesElements = findDOMElement(chart.element.id + 'SymbolGroup' + index).querySelectorAll('#' + chart.element.id + 'SymbolGroup' + index + ' ellipse');
            seriesElementsArray = addSeriesElements(seriesElementsArray, seriesElements);
        }
        seriesElementsArray = isLegend ? addTextAndShapeGroups(seriesElementsArray, chart, index) : seriesElementsArray;
    }
    else if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(chart.element.id + 'SeriesGroup' + index))) {
        seriesElements = findDOMElement(chart.element.id + 'SeriesGroup' + index).querySelectorAll('#' + chart.element.id + 'SeriesGroup' + index + ' path');
        seriesElementsArray = addSeriesElements(seriesElementsArray, seriesElements);
        if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(chart.element.id + 'SeriesGroup' + index).querySelectorAll('#' + chart.element.id + 'SeriesGroup' + index + ' ellipse'))) {
            seriesElements = findDOMElement(chart.element.id + 'SeriesGroup' + index).querySelectorAll('#' + chart.element.id + 'SeriesGroup' + index + ' ellipse');
            seriesElementsArray = addSeriesElements(seriesElementsArray, seriesElements);
        }
        seriesElementsArray = isLegend ? addTextAndShapeGroups(seriesElementsArray, chart, index) : seriesElementsArray;
    }
    const symbolGroup = document.getElementById(chart.element.id + 'SymbolGroup' + index);
    if (symbolGroup && isRectSeries(chart.seriesTypes[parseInt(index.toString(), 10)])) {
        seriesElements = symbolGroup.querySelectorAll('path');
        seriesElementsArray = addSeriesElements(seriesElementsArray, seriesElements);
        const ellipseElements = symbolGroup.querySelectorAll('ellipse');
        if (ellipseElements.length > 0) {
            seriesElementsArray = addSeriesElements(seriesElementsArray, ellipseElements);
        }
    }
    if (chart.seriesTypes[parseInt(index.toString(), 10)].indexOf('Area') > -1 || chart.seriesTypes[parseInt(index.toString(), 10)].toLowerCase().indexOf('line') > -1) {
        if (isMarker) {
            seriesElements = findDOMElement(chart.element.id + 'SeriesGroup' + index).querySelectorAll('#' + chart.element.id + 'SeriesGroup' + index + ' path');
            seriesElementsArray.push(...addSeriesElements(seriesElementsArray, seriesElements));
        }
        else if (!sfBlazorToolkit.base.isNullOrUndefined(findDOMElement(chart.element.id + 'SeriesGroup' + index))) {
            seriesElements = findDOMElement(chart.element.id + 'SeriesGroup' + index).querySelectorAll('#' + chart.element.id + 'SeriesGroup' + index + ' path');
            seriesElementsArray = addSeriesElements(seriesElementsArray, seriesElements);
        }
        seriesElementsArray = isLegend ? addTextAndShapeGroups(seriesElementsArray, chart, index) : seriesElementsArray;
    }
    return seriesElementsArray;
}

export function addSeriesElements(seriesElements, markerElements) {
    for (let i = 0; i < markerElements.length; i++) {
        const element = document.getElementById(markerElements[parseInt(i.toString(), 10)].id);
        if (!sfBlazorToolkit.base.isNullOrUndefined(element)) {
            seriesElements.push(element);
        }
        seriesElements = seriesElements.filter(function (elem, index, self) {
            return index === self.indexOf(elem);
        });
    }
    return seriesElements;
}

export function addTextAndShapeGroups(seriesElementsArray, chart, index) {
    const textGroup = document.getElementById(chart.element.id + 'TextGroup' + index);
    if (!sfBlazorToolkit.base.isNullOrUndefined(textGroup)) {
        seriesElementsArray.push(textGroup);
    }
    const shapeGroup = document.getElementById(chart.element.id + 'ShapeGroup' + index);
    if (!sfBlazorToolkit.base.isNullOrUndefined(shapeGroup)) {
        seriesElementsArray.push(shapeGroup);
    }
    return seriesElementsArray;
}

export function findDOMElementByIndex(chart, index, suffix = '', marker) {
    let elementId = chart.element.id + '_Series_' + index.series + '_Point' + '_' + index.point;
    const series = chart.seriesTypes[`${index.series}`];
    elementId = (!isRectSeries(series) && series !== 'Scatter' && series !== 'Bubble' && marker) ? (elementId + '_Symbol' + suffix) : elementId;
    return [findDOMElement(elementId), null];
}

export function applyStyles(elements, chart) {
    for (const element of elements) {
        if (element) {
            removeSvgClass(element, chart.unSelected);
            addSvgClass(element, getSelectionClass(element.id, chart));
            if (chart.styleId && chart.styleId.indexOf('highlight') > 0 && chart.highlightColor !== '' && !sfBlazorToolkit.base.isNullOrUndefined(chart.highlightColor) && chart.highlightPattern === 'None') {
                if (element.id.indexOf('Group') > 0) {
                    for (let i = 0; i < element.children.length; i++) {
                        element.children[parseInt(i.toString(), 10)].setAttribute('fill', chart.highlightColor);
                    }
                }
                else {
                    element.setAttribute('fill', chart.highlightColor);
                }
            }
        }
    }
}

export function getClusterElements(chart, index) {
    const clusters = [];
    let seriesStyle;
    let selectedElements;
    const seriesCollection = document.getElementById(chart.element.id + 'SeriesCollection').querySelectorAll('[id*="SeriesGroup"]');
    let seriesElements;
    let seriesElementsArray;
    for (let seriesG = 0; seriesG < seriesCollection.length; seriesG++) {
        seriesElements = seriesCollection[parseInt(seriesG.toString(), 10)].querySelectorAll('#' + seriesCollection[parseInt(seriesG.toString(), 10)].id + ' path');
        seriesElementsArray = Array.prototype.slice.call(seriesElements);
        for (let series = 0; series < seriesElementsArray.length; series++) {
            const seriesData = seriesElementsArray[parseInt(seriesG.toString(), 10)].getAttribute('data-point').split(',');
            index = new Index(+seriesData[4], index.point);
            if (isRectSeries(seriesData[3])) {
                clusters.push(findDOMElementByIndex(chart, index)[0]);
            }
            clusters.push(findDOMElementByIndex(chart, index, '', seriesData[9] === 'True')[0]);
            seriesStyle = generateStyle(seriesElementsArray[parseInt(seriesG.toString(), 10)].id, chart);
            selectedElements = document.querySelectorAll('.' + seriesStyle);
            findTrackballElements(selectedElements, seriesStyle);
            const clusterIndex = seriesData[9] === 'True' && isRectSeries(seriesData[3]) ? 2 : 1;
            if (!chart.allowMultiSelection && selectedElements.length > 0 &&
                selectedElements[0].id !== clusters[clusters.length - clusterIndex].id) {
                removeSelection(chart, index.series, selectedElements, seriesStyle, true);
            }
        }
    }
    return clusters;
}

export function clusterSelection(chart, index) {
    if (isChartPanning(chart)) {
        return;
    }
    selection(chart, index, getClusterElements(chart, new Index(index.series, index.point)));
}

export function removeSelection(chart, series, selectedElements, seriesStyle, isBlurEffectNeeded, legendClick) {
    if (isChartPanning(chart)) {
        return;
    }
    let elementId;
    if (selectedElements.length > 0) {
        const elements = [];
        for (let i = 0; i < selectedElements.length; i++) {
            elements.push(selectedElements[parseInt(i.toString(), 10)]);
        }
        removeStyles(elements, chart);
        chart.isSeriesMode = true;
        addOrRemoveIndex(chart.selectedDataIndexes, new Index(series, NaN), chart);
        for (let value = 0; value < selectedElements.length; value++) {
            elementId = selectedElements[parseInt(value.toString(), 10)].id;
            seriesStyle = generateStyle(elementId, chart);
            if (document.querySelectorAll('.' + seriesStyle).length > 0) {
                for (const element of elements) {
                    checkSelectionElements(element, seriesStyle, true, true, series, chart);
                }
                isBlurEffectNeeded = false;
                break;
            }
        }
        if (isBlurEffectNeeded) {
            chart.isSeriesMode = chart.selectionMode === 'Series';
            blurEffect(elementId, chart, legendClick);
        }
    }
}

export function isAlreadySelected(event, charts) {
    if (isChartPanning(charts)) {
        return false;
    }
    let targetElement = event.target;
    const parentId = charts.getLegendTemplateParentId(event);
    const parentTarget = findDOMElement(parentId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(parentTarget)) {
        targetElement = parentTarget;
    }
    const isLegend = targetElement ? targetElement.id.indexOf('legend') > -1 : false;
    if (event.type === 'click' && (!isLegend || (isLegend && !charts.toggleVisibility))) {
        charts.currentMode = charts.selectionMode;
        charts.styleId = charts.element.id + '_ej2_chart_selection';
    }
    else if (event.type === 'mousemove' || event.type === 'pointermove' || (event.type === 'click' && isLegend && charts.enableHighlight)) {
        charts.currentMode = charts.highlightMode;
        charts.highlightDataIndexes = [];
        charts.styleId = charts.element.id + '_ej2_chart_highlight';
    }
    if ((charts.highlightMode !== 'None' || charts.enableHighlight) && charts.selectionMode === 'None') {
        if (event.type === 'click' && (!isLegend || (isLegend && !charts.enableHighlight))) {
            return false;
        }
    }
    if ((charts.highlightMode !== 'None' || charts.enableHighlight) && charts.previousSelectedEle.length !== 0 && !sfBlazorToolkit.base.isNullOrUndefined(charts.previousSelectedEle[0])) {
        charts.previousSelectedEle.filter((element) => {
            return !sfBlazorToolkit.base.isNullOrUndefined(element);
        });
        const parentNodeId = !sfBlazorToolkit.base.isNullOrUndefined(targetElement) ? targetElement.parentNode.id : '';
        let isElement;
        if (!sfBlazorToolkit.base.isNullOrUndefined(targetElement) && targetElement.parentNode) {
            isElement = (parentNodeId.indexOf('Point') > 0 || parentNodeId.indexOf('Symbol') > 0) ? true : false;
        }
        for (let i = 0; i < charts.previousSelectedEle.length; i++) {
            if (charts.previousSelectedEle[parseInt(i.toString(), 10)].hasAttribute('class')) {
                if (charts.previousSelectedEle[parseInt(i.toString(), 10)].getAttribute('class').indexOf('highlight') > -1 && (isElement || event.type === 'click')) {
                    charts.previousSelectedEle[parseInt(i.toString(), 10)].removeAttribute('class');
                    if (charts.highlightColor !== '' && !sfBlazorToolkit.base.isNullOrUndefined(charts.highlightColor) && charts.highlightPattern === 'None') {
                        if (charts.previousSelectedEle[parseInt(i.toString(), 10)].id.indexOf('Group') > 0) {
                            for (let j = 0; j < charts.previousSelectedEle[parseInt(i.toString(), 10)].children.length; j++) {
                                const child = charts.previousSelectedEle[parseInt(i.toString(), 10)].children[parseInt(j.toString(), 10)];
                                child.setAttribute('fill', child.getAttribute('data-point').split(',')[2]);
                            }
                        }
                        else {
                            charts.previousSelectedEle[parseInt(i.toString(), 10)].setAttribute('fill', charts.previousSelectedEle[parseInt(i.toString(), 10)].getAttribute('data-point').split(',')[2]);
                        }
                    }
                    addOrRemoveIndex(charts.highlightDataIndexes, indexFinder(charts.previousSelectedEle[parseInt(i.toString(), 10)].id), charts);
                }
                else if (!isElement && charts.previousSelectedEle[parseInt(i.toString(), 10)].getAttribute('class').indexOf('highlight') > -1) {
                    performSelection(indexFinder(charts.previousSelectedEle[parseInt(i.toString(), 10)].id), charts, charts.previousSelectedEle[parseInt(i.toString(), 10)]);
                }
            }
        }
    }
    return true;
}

export function performSelection(index, chart, element) {
    if (isChartPanning(chart)) {
        return;
    }
    chart.isSeriesMode = chart.currentMode === 'Series';
    if (chart.seriesTypes[index.series] === 'Area' && (chart.currentMode === 'Point' || chart.currentMode === 'Cluster') && element &&
        (element.id === chart.element.id + '_Series_' + index.series)) {
        const className = generateStyle(element.id, chart);
        const selectionEle = document.querySelectorAll('.' + className);
        if (!sfBlazorToolkit.base.isNullOrUndefined(selectionEle)) {
            findTrackballElements(selectionEle, className);
            blurEffect(element.id, chart, element.getAttribute('data-point').split(',')[8] === 'True');
        }
    }
    switch (chart.currentMode) {
        case 'Series':
            removeHighlightELements(chart);
            selection(chart, index, getSeriesElements(index.series.toString(), chart));
            selectionComplete(chart, index, chart.selectionMode);
            blurEffect(!sfBlazorToolkit.base.isNullOrUndefined(element) ? element.id : '', chart);
            break;
        case 'Point':
            removeHighlightELements(chart);
            if (!isNaN(index.point) && element) {
                const pointElements = [];
                const isPointClick = element.id.indexOf('_Symbol') > -1;
                pointElements.push(element);
                if (isPointClick && (chart.seriesTypes[index.series] === 'Column' || chart.seriesTypes[index.series] === 'Bar')) {
                    pointElements.push(getElement(element.id.replace('_Symbol', '')));
                }
                selection(chart, index, pointElements);
                selectionComplete(chart, index, chart.selectionMode);
                blurEffect(element.id, chart);
            }
            break;
        case 'Cluster':
            removeHighlightELements(chart);
            if (!isNaN(index.point)) {
                clusterSelection(chart, index);
                selectionComplete(chart, index, chart.selectionMode);
                blurEffect(element.id, chart);
            }
            break;
    }
}

export function removeHighlightELements(chart) {
    if (isChartPanning(chart)) {
        return;
    }
    const elements = document.querySelectorAll(`[id^="${chart.id}_Series_"]`);
    elements.forEach(function (element1) {
        element1.style.opacity = '';
    });
}

export function selection(chart, index, selectedElements) {
    if (isChartPanning(chart)) {
        return;
    }
    selectedElements = selectedElements.filter((element) => {
        return !sfBlazorToolkit.base.isNullOrUndefined(element);
    });
    if (!(chart.currentMode === 'Lasso')) {
        if (!(chart.allowMultiSelection) && (chart.currentMode.indexOf('Drag') === -1 && chart.styleId.indexOf('highlight') === -1 &&
            chart.selectionMode !== 'None')) {
            removeMultiSelectElements(chart, chart.selectedDataIndexes, index, selectedElements);
        }
    }
    if (!sfBlazorToolkit.base.isNullOrUndefined(selectedElements[0])) {
        if (isRectSeries(chart.seriesTypes[index.series])) {
            if (selectedElements[0].id) {
                if (document.getElementById(selectedElements[0].id + '_Symbol')) {
                    selectedElements.push(findDOMElement(selectedElements[0].id + '_Symbol'));
                }
                else if (selectedElements[0].id.indexOf('SeriesGroup') !== -1) {
                    if (document.getElementById(selectedElements[0].id.replace('SeriesGroup', 'SymbolGroup'))) {
                        selectedElements.push(findDOMElement(selectedElements[0].id.replace('SeriesGroup', 'SymbolGroup')));
                    }
                }
            }
        }
        let isAdd;
        const className = selectedElements[0] && (selectedElements[0].getAttribute('class') || '');
        const pClassName = selectedElements[0].parentNode &&
            (selectedElements[0].parentNode.getAttribute('class') || '');
        if (className !== '' && chart.currentMode !== 'Cluster') {
            findTrackballElements(selectedElements, className);
        }
        if (selectedElements[0] && className.indexOf(getSelectionClass(selectedElements[0].id, chart)) > -1) {
            removeStyles(selectedElements, chart);
        }
        else if (selectedElements[0].parentNode && pClassName.indexOf(getSelectionClass(selectedElements[0].id, chart)) > -1) {
            removeStyles([selectedElements[0].parentNode], chart);
        }
        else {
            chart.previousSelectedEle = chart.highlightMode !== 'None' || chart.enableHighlight ? selectedElements : [];
            applyStyles(selectedElements, chart);
            isAdd = true;
        }
        if (chart.styleId && chart.styleId.indexOf('highlight') > 0 && (chart.highlightMode !== 'None' || chart.enableHighlight)) {
            addOrRemoveIndex(chart.highlightDataIndexes, index, chart, isAdd);
        }
        else {
            addOrRemoveIndex(chart.selectedDataIndexes, index, chart, isAdd);
        }
    }
}

export function resetPreviousHighlightIndex(chart) {
    chart.previousHighlightedIndex = -1;
}

export function isTargetChanged(currentSeriesIndex, chart) {
    let isStart = false;
    let isEquals = false;
    if (chart.previousHighlightedIndex === -1) {
        isStart = true;
    }
    if (chart.previousHighlightedIndex === currentSeriesIndex) {
        isEquals = true;
    }
    chart.previousHighlightedIndex = currentSeriesIndex;
    return (!isEquals || isStart);
}

export function legendSelection(e, dataId) {
    let targetId = e.target['id'];
    const charts = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (isChartPanning(charts)) {
        return;
    }
    const parentId = charts.getLegendTemplateParentId(e);
    if (parentId != '') {
        targetId = parentId;
    }
    const values = [charts.id + '_chart_legend_text_', charts.id + '_chart_legend_shape_marker_', charts.id + '_chart_legend_shape_', charts.id + '_chart_legend_g_', charts.id + '_chart_legend_template_'];
    let currentMode = 'None';
    let selectedElements;
    let currentIndex;
    let targetElement;
    if (isDragSelection) {
        return;
    }
    if (targetId.indexOf('legend') !== -1 && targetId.indexOf('legend_page') === -1 && !charts.isTouch) {
        for (let i = 0; i < values.length; i++) {
            if (!targetId.indexOf(values[parseInt(i.toString(), 10)])) {
                currentIndex = parseInt(targetId.split(values[parseInt(i.toString(), 10)])[1], 10);
                break;
            }
        }
        charts.currentSeriesIndex = currentIndex;
        if (targetId.indexOf('text') <= 0) {
            targetElement = findDOMElement(targetId);
        }
        else {
            targetElement = null;
        }
        if (e.type === 'mousemove' || (e.type === 'click' && charts.enableHighlight)) {
            if (targetId.indexOf('text')) {
                targetElement = findDOMElement(targetId.replace('text', 'shape'));
            }
            if (targetElement.hasAttribute('class') && !charts.enableHighlight) {
                if (charts.styleId.indexOf('highlight') > 0 && (charts.highlightMode !== 'None' || charts.enableHighlight) && charts.styleId.indexOf('selection') > 1 && charts.selectionMode !== 'None') {
                    return;
                }
            }
            currentMode = charts.highlightMode;
            if (currentMode === 'None' && charts.enableHighlight) {
                currentMode = 'Series';
            }
            if (charts.tooltipBase && charts.tooltipBase.enableHighlight && !isHighlightRemoved) {
                removeHighlight(charts);
                isHighlightRemoved = true;
            }
        }
        const isPreSelected = isAlreadySelected(e, charts);
        const seriesElement = findDOMElement(charts.element.id + 'SeriesGroup' + currentIndex);
        const isSeriesVisible = charts.seriesTypes[parseInt(currentIndex.toString(), 10)].indexOf('Stacking') > -1 ? !sfBlazorToolkit.base.isNullOrUndefined(seriesElement) && !sfBlazorToolkit.base.isNullOrUndefined(seriesElement.getAttribute('data-point')) && seriesElement.getAttribute('data-point').split(/(?![^(]*\)),/)[8] === 'True' : !sfBlazorToolkit.base.isNullOrUndefined(seriesElement);
        if (isPreSelected && !sfBlazorToolkit.base.isNullOrUndefined(seriesElement) && isSeriesVisible) {
            let seriesStyle = generateStyle(charts.element.id + 'SeriesGroup' + currentIndex, charts);
            selectedElements = document.querySelectorAll('.' + seriesStyle);
            isSeriesMode = currentMode === 'Series';
            const isBlurEffectNeeded = true;
            if (selectedElements.length > 0) {
                removeSelection(charts, currentIndex, selectedElements, seriesStyle, isBlurEffectNeeded, true);
            }
            else {
                const seriesCollection = document.getElementById(charts.element.id + 'SeriesCollection').querySelectorAll('[id*="SeriesGroup"]');
                for (let seriesG = 0; seriesG < seriesCollection.length; seriesG++) {
                    const seriesIndex = seriesCollection[parseInt(seriesG.toString(), 10)].id.indexOf('TrendLineSeriesGroup') > -1 ? +seriesCollection[parseInt(seriesG.toString(), 10)].id.split('TrendLineSeriesGroup')[1] : +seriesCollection[parseInt(seriesG.toString(), 10)].id.split('SeriesGroup')[1];
                    if (!charts.allowMultiSelection && seriesIndex !== currentIndex) {
                        seriesStyle = generateStyle(seriesCollection[parseInt(seriesG.toString(), 10)].id, charts);
                        selectedElements = document.querySelectorAll('.' + seriesStyle);
                        removeSelection(charts, currentIndex, selectedElements, seriesStyle, isBlurEffectNeeded);
                    }
                }
                const seriesElements = getSeriesElements(currentIndex.toString(), charts, true);
                for (let i = 0; i < seriesElements.length; i++) {
                    checkSelectionElements(seriesElements[parseInt(i.toString(), 10)], seriesStyle, false, true, currentIndex, charts);
                }
                charts.isSeriesMode = true;
                const indexAndPoint = new Index(currentIndex, 0);
                selection(charts, indexAndPoint, seriesElements);
                isSeriesMode = charts.selectionMode === 'Series';
                blurEffect(charts.element.id, charts, true);
                charts.isLegendHighlighting = true;
                charts.prevHighlightedSeriesIndex = currentIndex;
            }
        }
        else {
            if (!sfBlazorToolkit.base.isNullOrUndefined(targetElement) && (targetId.indexOf('legend') > -1) && !isSeriesVisible && !(charts.enableHighlight && charts.disabledLegendIndexes.indexOf(currentIndex) > -1)) {
                resetPreviousHighlightIndex(charts);
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(targetElement) && charts.highlightMode === 'None' && targetId.indexOf('legend') > -1) {
                charts.isLegendHighlighting = false;
                charts.prevHighlightedSeriesIndex = -1;
                removeSelectionStyles(charts.dataId);
            }
        }
    }
}

export function selectionComplete(chart, index, selectionMode) {
    let pointX;
    let pointY;
    let regionY;
    let pointIndex;
    let seriesIndex;
    let pointId;
    const selectedPointValues = [];
    let yValue;
    let selectedPointX;
    let seriesType;
    let SeriesCategory;
    const seriesArray = getSeriesElements(index.series.toString(), chart);
    if (selectionMode === 'Cluster') {
        for (let i = 0; i < seriesArray.length; i++) {
            seriesType = findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('data-point').split(',')[3];
            SeriesCategory = findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('data-point').split(',')[11];
            regionY = +findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('data-point').split(',')[10];
            if (findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('visibility') === 'visible') {
                for (let j = 0; j < chart.selectedDataIndexes.length; j++) {
                    pointIndex = chart.allowMultiSelection ? chart.selectedDataIndexes[parseInt(j.toString(), 10)].point : index.point;
                    seriesIndex = +findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('data-point').split(',')[4];
                    pointX = +findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('data-point').split(',')[0];
                    pointY = +findDOMElement(seriesArray[parseInt(i.toString(), 10)].id).getAttribute('data-point').split(',')[1];
                    if (!isNaN(pointIndex)) {
                        yValue = pointY;
                        selectedPointX = pointX;
                    }
                    if (chart.primaryXAxis === 'Category') {
                        selectedPointX = pointX.toLocaleString();
                    }
                    else if (chart.primaryXAxis === 'DateTime') {
                        selectedPointX = new Date(pointX);
                    }
                    if (SeriesCategory !== 'Indicator') {
                        selectedPointValues.push({
                            x: selectedPointX, y: yValue, seriesIndex: seriesIndex,
                            pointIndex: pointIndex
                        });
                    }
                }
            }
        }
    }
    else if (selectionMode === 'Series') {
        if (chart.allowMultiSelection) {
            for (let i = 0; i < chart.selectedDataIndexes.length; i++) {
                seriesIndex = chart.selectedDataIndexes[parseInt(i.toString(), 10)].series;
                selectedPointValues.push({
                    seriesIndex: seriesIndex
                });
            }
        }
        else {
            seriesIndex = (chart.selectedDataIndexes.length > 0) ? chart.selectedDataIndexes[0].series : 0;
            selectedPointValues.push({
                seriesIndex: seriesIndex
            });
        }
    }
    else if (selectionMode === 'Point') {
        for (let i = 0; i < chart.selectedDataIndexes.length; i++) {
            const selectDataIndex = chart.selectedDataIndexes[parseInt(i.toString(), 10)];
            pointIndex = selectDataIndex.point;
            seriesIndex = selectDataIndex.series;
            pointId = findElementId(chart, selectDataIndex);
            pointX = +findDOMElement(pointId).getAttribute('data-point').split(',')[0];
            pointY = +findDOMElement(pointId).getAttribute('data-point').split(',')[1];
            regionY = +findDOMElement(pointId).getAttribute('data-point').split(',')[10];
            if (!isNaN(pointIndex)) {
                selectedPointX = pointX;
                yValue = pointY;
                if (chart.primaryXAxis === 'Category') {
                    selectedPointX = pointX.toLocaleString();
                }
                else if (chart.primaryXAxis === 'DateTime') {
                    selectedPointX = new Date(pointX);
                }
                selectedPointValues.push({
                    x: selectedPointX, y: yValue, seriesIndex: seriesIndex,
                    pointIndex: pointIndex
                });
            }
        }
    }
    chart.dotnetref.invokeMethodAsync('OnSelectionChange', selectedPointValues);
}

export function storeToggledLegendIndexes(target, chart, e) {
    let currentIndex;
    const values = [currentId + '_chart_legend_text_', currentId + '_chart_legend_shape_marker_', currentId + '_chart_legend_shape_', currentId + '_chart_legend_g_', currentId + '_chart_legend_template_'];
    for (const value of values) {
        if (typeof target.id === 'string' && target.id.indexOf(value) !== -1) {
            const splitResult = target.id.split(value);
            if (splitResult.length > 1) {
                currentIndex = parseInt(splitResult[1], 10);
                break;
            }
        }
    }
    if (currentIndex !== undefined) {
        if (chart.disabledLegendIndexes.indexOf(currentIndex) !== -1) {
            chart.currentToggledBackLegendIndex = currentIndex;
            chart.currentLegendClickEvent = e;
        }
        else {
            chart.currentToggledBackLegendIndex = -1;
            chart.currentLegendClickEvent = null;
            chart.disabledLegendIndexes.push(currentIndex);
        }
    }
}

export function performLegendClickHighlight(dataId) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (chart.disabledLegendIndexes.length > 0 && chart.currentToggledBackLegendIndex !== -1) {
        legendSelection(chart.currentLegendClickEvent, dataId);
        chart.disabledLegendIndexes.splice(chart.disabledLegendIndexes.indexOf(chart.currentToggledBackLegendIndex), 1);
        chart.currentToggledBackLegendIndex = -1;
        chart.currentLegendClickEvent = null;
    }
}

export function addTouchPointer(touchList, e, touches) {
    if (touches.length > 0) {
        touchList = touches;
    }
    else {
        touchList = touchList.length > 0 ? touchList : [];
        if (touchList.length === 0) {
            touchList.push({ pageX: e.mouseX, pageY: e.mouseY, pointerId: e.pointerId });
        }
        else {
            for (let i = 0; i < touchList.length; i++) {
                if (touchList[parseInt(i.toString(), 10)].pointerId === e.pointerId) {
                    touchList[parseInt(i.toString(), 10)] = { pageX: e.mouseX, pageY: e.mouseY, pointerId: e.pointerId };
                }
                else {
                    touchList.push({ pageX: e.mouseX, pageY: e.mouseY, pointerId: e.pointerId });
                }
            }
        }
    }
    return touchList;
}

export function onZoomingMouseMove(e, chart) {
    if (chart && chart.zoomBase && chart.zoomBase.isSkipZooming) {
        return;
    }
    let touches = [];
    if (e.type === 'touchmove') {
        touches = e.touches;
    }
    if (chart.zoomBase.isChartDrag) {
        if (chart.zoomBase.zoomSettings.isOnZoomStartCalled) {
            chart.dotnetref.invokeMethodAsync('TriggerZoomingEvents', 'OnZoomStart', !chart.zoomBase.zoomingModule.isZoomStart);
            chart.zoomBase.zoomingModule.isZoomStart = false;
        }
        if (chart.isTouch) {
            chart.zoomBase.zoomingModule.touchMoveList = addTouchPointer(chart.zoomBase.zoomingModule.touchMoveList, e, touches);
            if (chart.zoomBase.zoomSettings.enablePinchZooming && chart.zoomBase.zoomingModule.touchMoveList.length > 1 && chart.zoomBase.zoomingModule.touchStartList.length > 1) {
                performPinchZooming(chart);
            }
        }
        renderZooming(e, chart, chart.isTouch);
        if (chart.isTouch && chart.zoomBase.isDoubleTap && (chart.selectionMode.indexOf('Drag') > -1 || chart.selectionMode.indexOf('Lasso') > -1)) {
            chart.zoomBase.isDoubleTap = false;
        }
    }
}

export function onZoomingMouseEnd(e, chart) {
    if (chart && chart.zoomBase && chart.zoomBase.isSkipZooming) {
        return;
    }
    chart.zoomBase.isChartDrag = false;
    const isperformZoomRedraw = !(e.target.indexOf(chart.element.id + '_ZoomOut') > -1) || (e.target.indexOf(chart.element.id + '_ZoomIn') > -1);
    if (chart.zoomBase.isChartDrag || isperformZoomRedraw) {
        clearSelectionRect(chart.element.id + '_ZoomArea');
        performZoomRedraw(chart);
        if (chart.zoomBase.zoomSettings.isOnZoomEndCalled && !chart.zoomToolkitBase.isReset && chart.zoomBase.zoomingModule.zoomAxes.length > 0) {
            chart.dotnetref.invokeMethodAsync('TriggerZoomingEvents', 'OnZoomEnd', false);
        }
        chart.zoomBase.zoomingModule.isZoomStart = true;
    }
    if (chart.isTouch) {
        if (chart.zoomBase.isDoubleTap && svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect) && chart.zoomBase.zoomingModule.touchStartList.length === 1 && chart.zoomBase.zoomingModule.isZoomed) {
            zoomToolkitReset(chart);
            chart.zoomBase.zoomingModule.isZoomStart = true;
        }
        chart.zoomBase.zoomingModule.touchStartList = [];
        chart.zoomBase.isDoubleTap = false;
    }
    chart.userInteractionBase.disableTrackTooltip = false;
    chart.zoomToolkitBase.isReset = false;
    chart.zoomBase.zoomingModule.zoomAxes = [];
    const zoomKitElement = getElement(chart.element.id + chart.zoomBase.zoomToolkitId);
    if (zoomKitElement) {
        zoomKitElement.style.visibility = 'visible';
    }
}

export function onZoomingMouseDown(e, pageX, pageY, chart) {
    if (chart && chart.zoomBase && chart.zoomBase.isSkipZooming) {
        return;
    }
    const target = e.target;
    const offset = sfBlazorToolkit.base.Browser.isDevice ? 20 : 30;
    const svgRect = document.getElementById(chart.element.id + '_svg').getBoundingClientRect();
    const rect = document.getElementById(chart.element.id).getBoundingClientRect();
    chart.zoomBase.mouseDownX = chart.zoomBase.previousMouseMoveX = (pageX - rect.left) - Math.max(svgRect.left - rect.left, 0);
    chart.zoomBase.mouseDownY = chart.zoomBase.previousMouseMoveY = (pageY - rect.top) - Math.max(svgRect.top - rect.top, 0);
    if (chart.isTouch) {
        chart.zoomBase.isDoubleTap = (new Date().getTime() < chart.zoomBase.threshold && target.id.indexOf(chart.element.id + '_Zooming') === -1 &&
            (chart.zoomBase.mouseDownX - offset >= chart.mouseX || chart.zoomBase.mouseDownX + offset >= chart.mouseX) &&
            (chart.zoomBase.mouseDownY - offset >= chart.mouseY || chart.zoomBase.mouseDownY + offset >= chart.mouseY) &&
            (chart.mouseX - offset >= chart.zoomBase.mouseDownX || chart.mouseX + offset >= chart.zoomBase.mouseDownX) &&
            (chart.mouseY - offset >= chart.zoomBase.mouseDownY || chart.mouseY + offset >= chart.zoomBase.mouseDownY));
    }
    zoomingMouseDownHandler(chart.getEventArgs(e, chart.id), chart);
}

export function zoomingMouseDownHandler(args, chart) {
    if (!((args.target.indexOf(chart.element.id + '_Zooming') > -1) || (args.target.indexOf(chart.element.id + '_scrollBar') > -1))
        && (chart.zoomBase.zoomSettings.enablePinchZooming || chart.zoomBase.zoomSettings.enableSelectionZooming || chart.zoomBase.zoomingModule.isPanning)
        && svgbase.withInAreaBounds(chart.zoomBase.previousMouseMoveX, chart.zoomBase.previousMouseMoveY, chart.userInteractionBase.axisClipRect)) {
        chart.zoomBase.isChartDrag = true;
    }
    if (chart.isTouch) {
        chart.zoomBase.zoomingModule.touchStartList = addTouchPointer(chart.zoomBase.zoomingModule.touchStartList, args, args.touches);
    }
}

export function zoomingMouseWheelHandler(e, chart) {
    const target = e.target;
    if (chart.zoomBase.zoomSettings && chart.zoomBase.zoomSettings.enableMouseWheelZooming && svgbase.withInAreaBounds(chart.mouseX, chart.mouseY, chart.userInteractionBase.axisClipRect) && !(target.id.indexOf('scrollbar') > -1)) {
        performMouseWheelZooming(e, chart.mouseX, chart.mouseY, chart, chart.userInteractionBase.axes);
    }
    if (isMouseWheelScroll(target.id)) {
        chart.scrollbarBase.isScrollWheel = chart.scrollbarBase.isScrollWheel ? chart.scrollbarBase.isScrollWheel : true;
        performMouseWheelScrolling(e, chart.mouseX, chart.mouseY, chart, chart.userInteractionBase.axes);
    }
}

export function renderZooming(e, chart, isTouch) {
    calculateZoomAxesRange(chart);
    if (chart.zoomBase.zoomSettings.enableSelectionZooming && !(chart.zoomBase.zoomSettings.enablePan) && (!isTouch || (chart.zoomBase.isDoubleTap && chart.zoomBase.zoomingModule.touchStartList.length === 1)) && (!chart.zoomBase.zoomingModule.isPanning || chart.zoomBase.isDoubleTap)) {
        chart.zoomBase.zoomingModule.isPanning = chart.zoomBase.zoomingModule.isDevice ? true : chart.zoomBase.zoomingModule.isPanning;
        chart.zoomBase.zoomingModule.performedUI = chart.zoomBase.zoomingModule.isZooming = true;
        drawZoomingRectangle(chart);
    }
    else if (chart.zoomBase.zoomingModule.isPanning && !chart.zoomBase.zoomingModule.isZooming && chart.zoomBase.isChartDrag) {
        if (!isTouch || (isTouch && chart.zoomBase.zoomingModule.touchStartList.length === 1)) {
            chart.zoomBase.zoomingModule.pinchTarget = isTouch ? e.target : null;
            doPan(chart, chart.zoomBase.axisCollections);
        }
    }
}

export function calculateZoomAxesRange(chart) {
    let range;
    let axisRange;
    for (let index = 0; index < chart.zoomBase.axisCollections.length; index++) {
        const axis = chart.zoomBase.axisCollections[index];
        axisRange = axis.visibleRange;
        if (chart.zoomBase.zoomingModule.zoomAxes.length > index && chart.zoomBase.zoomingModule.zoomAxes[index] && !chart.zoomBase.delayRedraw) {
            chart.zoomBase.zoomingModule.zoomAxes[index].min = axisRange.start;
            chart.zoomBase.zoomingModule.zoomAxes[index].delta = axisRange.delta;
        }
        else {
            range = {
                actualMin: axis.actualRange.start,
                actualDelta: axis.actualRange.delta,
                min: axisRange.start,
                delta: axisRange.delta
            };
            chart.zoomBase.zoomingModule.zoomAxes[index] = range;
        }
    }
}

export function drawZoomingRectangle(chart) {
    const areaBounds = chart.userInteractionBase.axisClipRect;
    const startLocation = new IChartInternalLocation(chart.zoomBase.previousMouseMoveX, chart.zoomBase.previousMouseMoveY);
    const endLocation = new IChartInternalLocation(chart.mouseX, chart.mouseY);
    const svgElement = getElement(chart.element.id + '_svg');
    const rect = chart.zoomBase.zoomingModule.zoomingRect = getRectLocation(startLocation, endLocation, areaBounds);
    if (rect.width > 0 && rect.height > 0) {
        chart.zoomBase.zoomingModule.isZoomed = true;
        chart.userInteractionBase.disableTrackTooltip = true;
        setAttribute((svgElement.id), 'cursor', 'crosshair');
        if (chart.zoomBase.zoomingModule.zooming.mode === 'X') {
            rect.height = areaBounds.height;
            rect.y = areaBounds.y;
        }
        else if (chart.zoomBase.zoomingModule.zooming.mode === 'Y') {
            rect.width = areaBounds.width;
            rect.x = areaBounds.x;
        }
        removeTooltipCrosshairOnZoom(chart);
        const zoomKitElement = getElement(chart.element.id + chart.zoomBase.zoomToolkitId);
        if (zoomKitElement) {
            zoomKitElement.style.visibility = 'hidden';
        }
        if (svgElement) {
            svgElement.appendChild(chart.userInteractionBase.svgRenderer.drawRectangle(new RectOption(chart.element.id + '_ZoomArea', chart.zoomBase.zoomingModule.zooming.themeStyleSelectionRectFill, { color: chart.zoomBase.zoomingModule.zooming.themeStyleSelectionRectStroke, width: 1 }, 1, rect, 0, 0, '', '3')));
        }
        else {
            const chartElement = document.getElementById(chart.element.id);
            chartElement.appendChild(chart.userInteractionBase.svgRenderer.drawRectangle(new RectOption(chart.element.id + '_ZoomArea', chart.zoomBase.zoomingModule.zooming.themeStyleSelectionRectFill, { color: chart.zoomBase.zoomingModule.zooming.themeStyleSelectionRectStroke, width: 1 }, 1, rect, 0, 0, '', '3')));
        }
    }
}

export function getRectLocation(startLocation, endLocation, outerRect) {
    const x = (endLocation.x < outerRect.x) ? outerRect.x :
        (endLocation.x > (outerRect.x + outerRect.width)) ? outerRect.x + outerRect.width : endLocation.x;
    const y = (endLocation.y < outerRect.y) ? outerRect.y :
        (endLocation.y > (outerRect.y + outerRect.height)) ? outerRect.y + outerRect.height : endLocation.y;
    return new svgbase.Rect((x > startLocation.x ? startLocation.x : x), (y > startLocation.y ? startLocation.y : y), Math.abs(x - startLocation.x), Math.abs(y - startLocation.y));
}

export function getVisibleRangeModel(doubleRange, interval) {
    return { min: doubleRange.start, max: doubleRange.end, interval: interval, delta: doubleRange.delta };
}

export function minMax(factor, min, max) {
    return isFinite(factor) ? factor > max ? max : (factor < min ? min : factor) : 0;
}

export function doPan(chart, axes, xDiff = 0, yDiff = 0) {
    if (chart.userInteractionBase.startMove && chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable) {
        chart.userInteractionBase.disableTrackTooltip = false;
        return;
    }
    removeTooltipCrosshairOnZoom(chart);
    chart.isChartPanning = true;
    chart.zoomBase.zoomingModule.isZoomed = true;
    chart.zoomBase.zoomingModule.offset = !chart.zoomBase.delayRedraw ? chart.userInteractionBase.axisClipRect : chart.zoomBase.zoomingModule.offset;
    chart.zoomBase.delayRedraw = chart.userInteractionBase.disableTrackTooltip = true;
    const zoomedAxisCollection = [];
    let zoomFactor;
    let zoomPosition;
    let currentScale;
    const axisNames = [];
    const zoomFactors = [];
    const zoomPositions = [];
    for (const axis of axes) {
        zoomFactor = axis.zoomFactor;
        zoomPosition = axis.zoomPosition;
        currentScale = Math.max(1 / minMax(zoomFactor, 0, 1), 1);
        if (axis.orientation === 'Horizontal') {
            zoomPosition = minMax(axis.zoomPosition + ((xDiff !== 0 ? xDiff : (chart.zoomBase.previousMouseMoveX - chart.mouseX)) / axis.rect.w / currentScale), 0, 1 - axis.zoomFactor);
        }
        else {
            zoomPosition = minMax(axis.zoomPosition - ((yDiff !== 0 ? yDiff : (chart.zoomBase.previousMouseMoveY - chart.mouseY)) / axis.rect.h / currentScale), 0, 1 - axis.zoomFactor);
        }
        if (axis.isZoomingScrollBar) {
            axis.isScrollUI = false;
        }
        zoomedAxisCollection.push({
            axisName: axis.name,
            zoomFactor: zoomFactor,
            zoomPosition: zoomPosition,
            axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
        });
    }
    const zoomingEventArgs = {
        name: 'OnZooming', axisCollection: zoomedAxisCollection
    };
    chart.zoomBase.zoomingEventArgs = zoomingEventArgs;
    if (!zoomingEventArgs.cancel) {
        for (const axisData of zoomedAxisCollection) {
            axes.filter((axis) => {
                if (axis.name === axisData.axisName) {
                    axisNames.push(axis.name);
                    zoomFactors.push(axisData.zoomFactor);
                    zoomPositions.push(axisData.zoomPosition);
                    axis.zoomFactor = axisData.zoomFactor;
                    axis.zoomPosition = axisData.zoomPosition;
                }
            });
        }
        performDeferredZoom(chart, axisNames, zoomFactors, zoomPositions);
    }
}

export function doZoom(chart, axes, bounds) {
    const zoomRect = chart.zoomBase.zoomingModule.zoomingRect;
    chart.zoomBase.zoomingModule.isPanning = chart.zoomBase.zoomSettings.enablePan || chart.zoomBase.zoomingModule.isPanning;
    const zoomedAxisCollection = [];
    let zoomFactor;
    let zoomPosition;
    for (const axis of axes) {
        zoomFactor = axis.zoomFactor;
        zoomPosition = axis.zoomPosition;
        if (axis.orientation === 'Horizontal' && chart.zoomBase.zoomingModule.zooming.mode !== 'Y') {
            zoomPosition += Math.abs((zoomRect.x - bounds.x) / bounds.width) * axis.zoomFactor;
            zoomFactor *= zoomRect.width / bounds.width;
        }
        else if (axis.orientation === 'Vertical' && chart.zoomBase.zoomingModule.zooming.mode !== 'X') {
            zoomPosition += (1 - Math.abs((zoomRect.height + (zoomRect.y - bounds.y)) / bounds.height)) * axis.zoomFactor;
            zoomFactor *= zoomRect.height / bounds.height;
        }
        zoomedAxisCollection.push({
            axisName: axis.name,
            zoomFactor: zoomFactor < 0.001 ? axis.zoomFactor : zoomFactor,
            zoomPosition: zoomFactor < 0.001 ? axis.zoomPosition : zoomPosition,
            axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
        });
    }
    const onZoomingEventArgs = {
        name: 'OnZooming', axisCollection: zoomedAxisCollection
    };
    chart.zoomBase.zoomingEventArgs = onZoomingEventArgs;
    if (!onZoomingEventArgs.cancel) {
        for (const axisData of zoomedAxisCollection) {
            axes.filter((axis) => {
                if (axis.name === axisData.axisName) {
                    axis.zoomFactor = axisData.zoomFactor;
                    axis.zoomPosition = axisData.zoomPosition;
                }
            });
        }
        chart.zoomBase.zoomingModule.zoomingRect = new svgbase.Rect(0, 0, 0, 0);
        performZoomRedraw(chart);
    }
}

export function performZoomRedraw(chart, isKeyboardFocus = false) {
    if (chart.zoomBase.zoomingModule.isZoomed) {
        if (chart.zoomBase.zoomingModule.zoomingRect.width > 0 && chart.zoomBase.zoomingModule.zoomingRect.height > 0) {
            chart.zoomBase.zoomingModule.performedUI = true;
            doZoom(chart, chart.userInteractionBase.axes, chart.userInteractionBase.axisClipRect);
            chart.zoomBase.isDoubleTap = false;
            setAttribute(chart.element.id + '_svg', 'cursor', 'auto');
        }
        else if (chart.userInteractionBase.disableTrackTooltip) {
            chart.userInteractionBase.disableTrackTooltip = chart.zoomBase.delayRedraw = false;
            const zoomingStates = {
                isZoomed: chart.zoomBase.zoomingModule.isZoomed,
                isPanning: chart.zoomBase.zoomingModule.isPanning,
                isDoubleTap: chart.zoomBase.isDoubleTap,
                isWheelZoom: chart.zoomBase.zoomingModule.isWheelZoom,
                performedUI: chart.zoomBase.zoomingModule.performedUI,
                delayRedraw: chart.zoomBase.delayRedraw,
                isChartDrag: chart.zoomBase.isChartDrag,
                isChartPanning: chart.isChartPanning
            };
            //need to invoke processlayoutchange.
            chart.dotnetref.invokeMethodAsync('ZoomingComplete', chart.zoomBase.zoomingEventArgs, zoomingStates);
            if (isKeyboardFocus) {
                focusTarget(chart.element.id);
            }
        }
        chart.isChartPanning = false;
    }
}

export function clearSelectionRect(id) {
    const selectionRect = document.getElementById(id);
    if (selectionRect) {
        selectionRect.setAttribute('x', '0');
        selectionRect.setAttribute('y', '0');
        selectionRect.setAttribute('width', '0');
        selectionRect.setAttribute('height', '0');
    }
}

export function isMouseWheelScroll(targetId) {
    if (targetId.indexOf('scrollBarThumb_') !== -1 || targetId.indexOf('scrollBarBackRect_') !== -1
        || targetId.indexOf('scrollBar_leftCircle_') !== -1 || targetId.indexOf('scrollBar_rightCircle_') !== -1
        || targetId.indexOf('scrollBar_gripCircle_') !== -1 || targetId.indexOf('scrollBar_leftArrow_') !== -1
        || targetId.indexOf('scrollBar_rightArrow_') !== -1 || targetId.indexOf('scrollBar_gripCircle') !== -1) {
        return true;
    }
    else {
        return false;
    }
}

export function getAxisName(targetId, axes) {
    const axisNames = axes.map((axis) => axis.name);
    for (const axisName of axisNames) {
        if (typeof targetId === 'string' && targetId.indexOf(axisName) !== -1) {
            const matchedAxes = axes.filter((axis) => axis.name === axisName);
            return matchedAxes.length > 0 ? matchedAxes[0].name : null;
        }
    }
    return null;
}

export function calculateDelta(scrollbarAxis, scrollbar, chart, e) {
    for (let i = 0; i < scrollbar.axes.length; i++) {
        if (scrollbarAxis.name === scrollbar.axes[parseInt(i.toString(), 10)].name) {
            scrollbar.axis = scrollbar.axes[parseInt(i.toString(), 10)];
        }
    }
    const scrollbarOptions = scrollbar.scrollbarOptions[scrollbarAxis.name];
    scrollbarOptions.isVertical = scrollbarAxis.orientation === 'Vertical';
    const isInverse = scrollbarAxis.isAxisInverse;
    if (sfBlazorToolkit.base.isNullOrUndefined(scrollbarOptions) || (scrollbarOptions && !document.getElementById(scrollbarOptions.svgObject.id))) {
        return null;
    }
    const eventArgs = getScrollEventArgs(e, [chart.id, chart.id + '_scrollBar_svg' + scrollbarAxis.name]);
    mouseX = eventArgs['mouseX'];
    mouseY = eventArgs['mouseY'];
    scrollbar.previousXY = scrollbarOptions.isVertical && isInverse ? mouseY : scrollbarOptions.isVertical ? scrollbarOptions.width - mouseY : isInverse ? scrollbarOptions.width - mouseX : mouseX;
    const defaultDelta = 5;
    delta = Math.max(-1, Math.min(1, (e['wheelDelta'] || -e.detail)));
    if (delta > 0) {
        scrollbar.previousXY = scrollbar.previousXY + defaultDelta;
    }
    else {
        scrollbar.previousXY = scrollbar.previousXY - defaultDelta;
    }
    return scrollbarOptions;
}

export function lazyLoadScrollChanged(scrollbarAxis, scrollbar, scrollbarOptions, chart) {
    const isInverse = scrollbarAxis.isAxisInverse;
    let mouseXY = (scrollbarOptions.isVertical && isInverse) ? scrollbarOptions.width - mouseY : scrollbarOptions.isVertical ?
        mouseY : mouseX;
    const zoomPosition = scrollbarOptions.zoomPosition;
    const zoomFactor = scrollbarOptions.zoomFactor;
    const circleRadius = scrollbarOptions.height / 2;
    let args;
    if (scrollbarOptions.isLazyLoad && scrollbar.isScrollWheel) {
        if (scrollbarOptions.thumbRectX !== circleRadius && (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + circleRadius < scrollbarOptions.width) && delta !== 0) {
            args = calculateLazyRange(scrollbar, null, delta);
        }
    }
    const currentRange = args ? args.currentRange : null;
    if (scrollbar.isScrollWheel) {
        mouseXY = (scrollbarOptions.isVertical || isInverse) ? scrollbarOptions.width - mouseXY : mouseXY;
        const currentX = scrollbarOptions.thumbRectX + (mouseXY - scrollbar.previousXY);
        scrollbarOptions.svgObject.style.cursor = 'default';
        if (mouseXY >= 0 || mouseXY <= currentX + scrollbarOptions.thumbRectWidth) {
            scrollbarOptions.thumbRectX = isWithIn(currentX, scrollbar) ? currentX : scrollbarOptions.thumbRectX;
            positionThumb(scrollbarOptions.thumbRectX, scrollbarOptions.thumbRectWidth, scrollbar);
            scrollbar.previousXY = mouseXY;
            setZoomFactorPosition(scrollbar, currentX, scrollbarOptions.thumbRectWidth, false);
        }
        if (args) {
            if (scrollbar.isScrollEventCalled) {
                chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollChanged', scrollbarAxis.name, zoomPosition, zoomFactor, currentRange));
            }
        }
    }
}

export function lazyLoadScrollEnd(scrollbarAxis, scrollbar, scrollbarOptions, chart) {
    let args;
    scrollbarOptions.startX = scrollbarOptions.thumbRectX;
    const circleRadius = scrollbarOptions.height / 2;
    const circleWidth = 1;
    const currentScrollWidth = scrollbarOptions.startX + scrollbarOptions.thumbRectWidth + circleRadius + circleWidth;
    const currentZPWidth = circleRadius + (circleWidth / 2);
    if (!scrollbarOptions.isLazyLoad) {
        scrollbarAxis.zoomFactor = (currentScrollWidth >= scrollbarOptions.width - 1 && (scrollbarOptions.startX - currentZPWidth) <= 0) ? 1 : scrollbarOptions.zoomFactor;
    }
    if (scrollbarOptions.isLazyLoad) {
        if (scrollbarOptions.thumbRectX !== circleRadius && (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + circleRadius < scrollbarOptions.width) && delta !== 0) {
            args = calculateLazyRange(scrollbar, null, delta);
        }
        if (args) {
            if (scrollbar.isScrollEventCalled) {
                chart.dotnetref.invokeMethodAsync('TriggerScrollEvents', getArgs('OnScrollEnd', scrollbarAxis.name, scrollbarAxis.zoomPosition, scrollbarAxis.zoomFactor, args.currentRange));
            }
        }
        scrollbar.isScrollWheel = false;
    }
}

export function performMouseWheelScrolling(e, mouseX, mouseY, chart, axes) {
    chart.isWheelScrolling = true;
    const scrollbar = chart.scrollbarBase;
    const targetId = e.target.id;
    const axisName = getAxisName(targetId, scrollbar.axes);
    const scrollbarAxis = axes.filter((axis) => { return axis.name === axisName; })[0];
    const scrollbarOptions = calculateDelta(scrollbarAxis, scrollbar, chart, e);
    lazyLoadScrollChanged(scrollbarAxis, scrollbar, scrollbarOptions, chart);
    lazyLoadScrollEnd(scrollbarAxis, scrollbar, scrollbarOptions, chart);
}

export function performMouseWheelZooming(e, mouseX, mouseY, chart, axes) {
    const direction = (chart.zoomBase.zoomingModule.browser.browserName === 'mozilla' && !chart.zoomBase.zoomingModule.browser.isPointer) ?
        -(e.detail) / 3 > 0 ? 1 : -1 : (e['wheelDelta'] > 0 ? 1 : -1);
    chart.zoomBase.zoomingModule.isZoomed = true;
    removeTooltipCrosshairOnZoom(chart);
    calculateZoomAxesRange(chart);
    chart.userInteractionBase.disableTrackTooltip = true;
    chart.zoomBase.zoomingModule.performedUI = true;
    chart.zoomBase.zoomingModule.isPanning = chart.zoomBase.zoomSettings.enablePan || chart.zoomBase.zoomingModule.isPanning;
    const zoomedAxisCollection = [];
    let origin;
    let cumulative;
    chart.zoomBase.zoomingModule.isWheelZoom = true;
    chart.isMouseWheelZoom = true;
    let zoomFactor;
    let zoomPosition;
    for (const axis of axes) {
        zoomFactor = axis.zoomFactor;
        zoomPosition = axis.zoomPosition;
        if ((axis.orientation === 'Vertical' && chart.zoomBase.zoomingModule.zooming.mode !== 'X') || (axis.orientation === 'Horizontal' && chart.zoomBase.zoomingModule.zooming.mode !== 'Y')) {
            cumulative = Math.max(Math.max(1 / minMax(axis.zoomFactor, 0, 1), 1) + (0.25 * direction), 1);
            if (cumulative >= 1) {
                origin = (axis.orientation === 'Horizontal') ? mouseX / axis.rect.w : 1 - (mouseY / axis.rect.h);
                origin = origin > 1 ? 1 : origin < 0 ? 0 : origin;
                zoomFactor = (cumulative === 1) ? 1 : minMax(1 / cumulative, 0, 1);
                zoomPosition = (cumulative === 1) ? 0 : axis.zoomPosition + ((axis.zoomFactor - zoomFactor) * origin);
                if (axis.zoomPosition !== zoomPosition || axis.zoomFactor !== zoomFactor) {
                    zoomFactor = (zoomPosition + zoomFactor) > 1 ? 1 - zoomPosition : zoomFactor;
                }
            }
        }
        zoomedAxisCollection.push({
            axisName: axis.name,
            zoomFactor: zoomFactor < 0.001 ? axis.zoomFactor : zoomFactor,
            zoomPosition: zoomFactor < 0.001 ? axis.zoomPosition : zoomPosition,
            axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
        });
    }
    const onZoomingEventArgs = {
        name: 'OnZooming', axisCollection: zoomedAxisCollection
    };
    chart.zoomBase.zoomingEventArgs = onZoomingEventArgs;
    if (!onZoomingEventArgs.cancel) {
        for (const axisData of zoomedAxisCollection) {
            axes.filter((axis) => {
                if (axis.name === axisData.axisName) {
                    axis.zoomFactor = axisData.zoomFactor;
                    axis.zoomPosition = axisData.zoomPosition;
                }
            });
        }
        performZoomRedraw(chart);
    }
    chart.zoomBase.zoomingModule.isWheelZoom = false;
}

export function performPinchZooming(chart) {
    if ((chart.zoomBase.zoomingModule.zoomingRect.width > 0 && chart.zoomBase.zoomingModule.zoomingRect.height > 0) ||
        (chart.userInteractionBase.startMove && chart.crosshairBase.crosshair && chart.crosshairBase.crosshair.enable)) {
        return false;
    }
    calculateZoomAxesRange(chart);
    removeTooltipCrosshairOnZoom(chart);
    chart.zoomBase.zoomingModule.isZoomed = chart.zoomBase.zoomingModule.isPanning = chart.zoomBase.zoomingModule.performedUI = true;
    chart.zoomBase.zoomingModule.offset = (!chart.zoomBase.delayRedraw || sfBlazorToolkit.base.isNullOrUndefined(chart.zoomBase.zoomingModule.offset)) ? chart.userInteractionBase.axisClipRect : chart.zoomBase.zoomingModule.offset;
    chart.zoomBase.delayRedraw = chart.userInteractionBase.disableTrackTooltip = true;
    const elementOffset = chart.element.getBoundingClientRect();
    const touchDown = chart.zoomBase.zoomingModule.touchStartList;
    const touchMove = chart.zoomBase.zoomingModule.touchMoveList;
    const touch0StartX = touchDown[0].pageX - elementOffset.left;
    const touch0StartY = touchDown[0].pageY - elementOffset.top;
    const touch0EndX = touchMove[0].pageX - elementOffset.left;
    const touch0EndY = touchMove[0].pageY - elementOffset.top;
    const touch1StartX = touchDown[1].pageX - elementOffset.left;
    const touch1StartY = touchDown[1].pageY - elementOffset.top;
    const touch1EndX = touchMove[1].pageX - elementOffset.left;
    const touch1EndY = touchMove[1].pageY - elementOffset.top;
    const scaleX = Math.abs(touch0EndX - touch1EndX) / Math.abs(touch0StartX - touch1StartX);
    const scaleY = Math.abs(touch0EndY - touch1EndY) / Math.abs(touch0StartY - touch1StartY);
    const clipX = ((chart.zoomBase.zoomingModule.offset.x - touch0EndX) / scaleX) + touch0StartX;
    const clipY = ((chart.zoomBase.zoomingModule.offset.y - touch0EndY) / scaleY) + touch0StartY;
    const translateXValue = (touch0EndX - (scaleX * touch0StartX));
    const translateYValue = (touch0EndY - (scaleY * touch0StartY));
    const pinchRect = new svgbase.Rect(clipX, clipY, chart.zoomBase.zoomingModule.offset.width / scaleX, chart.zoomBase.zoomingModule.offset.height / scaleY);
    if (!isNaN(scaleX - scaleX) && !isNaN(scaleY - scaleY)) {
        switch (chart.zoomBase.zoomSettings.mode) {
            case 'XY':
                setTransform(translateXValue, translateYValue, scaleX, scaleY, chart, true);
                break;
            case 'X':
                setTransform(translateXValue, 0, scaleX, 1, chart, true);
                break;
            case 'Y':
                setTransform(0, translateYValue, 1, scaleY, chart, true);
                break;
        }
    }
    calculatePinchZoomFactor(chart, pinchRect);
    return true;
}

export function setTransform(translateXValue, translateYValue, scaleX, scaleY, chart, isPinch) {
    const seriesCollection = document.getElementById(chart.element.id + 'SeriesCollection');
    if (seriesCollection) {
        seriesCollection.setAttribute('clip-path', 'url(#' + chart.element.id + '_ChartAreaClipRect_)');
    }
    let translate;
    if (!sfBlazorToolkit.base.isNullOrUndefined(translateXValue) && !sfBlazorToolkit.base.isNullOrUndefined(translateYValue)) {
        for (let i = 0; i < chart.userInteractionBase.visibleSeries.length; i++) {
            const seriesVal = chart.userInteractionBase.visibleSeries[parseInt(i.toString(), 10)];
            const xAxisLoc = chart.userInteractionBase.isInverted ? seriesVal.y_Axis.rect.x : seriesVal.x_Axis.rect.x;
            const yAxisLoc = chart.userInteractionBase.isInverted ? seriesVal.x_Axis.rect.y : seriesVal.y_Axis.rect.y;
            translate = 'translate(' + (translateXValue + (isPinch ? (scaleX * xAxisLoc) : xAxisLoc)) +
                ',' + (translateYValue + (isPinch ? (scaleY * yAxisLoc) : yAxisLoc)) + ')';
            translate = (scaleX || scaleY) ? translate + ' scale(' + scaleX + ' ' + scaleY + ')' : translate;
            if (seriesVal.visible) {
                const seriesElement = getElement(chart.element.id + 'SeriesGroup' + seriesVal.index);
                const errorBarElement = getElement(chart.element.id + 'ErrorBarGroup' + seriesVal.index);
                const symbolElement = getElement(chart.element.id + 'SymbolGroup' + seriesVal.index);
                const textElement = getElement(chart.element.id + 'TextGroup' + seriesVal.index);
                const shapeElement = getElement(chart.element.id + 'ShapeGroup' + seriesVal.index);
                if (seriesElement) {
                    seriesElement.setAttribute('transform', translate);
                }
                const element = getElement(chart.element.id + '_Series_' + seriesVal.index + '_DataLabelCollections');
                if (errorBarElement) {
                    errorBarElement.setAttribute('transform', translate);
                }
                if (symbolElement) {
                    symbolElement.setAttribute('transform', translate);
                }
                if (textElement) {
                    textElement.setAttribute('visibility', 'hidden');
                    shapeElement.setAttribute('visibility', 'hidden');
                }
                if (element) {
                    element.style.visibility = 'hidden';
                }
            }
        }
    }
}

export function performDeferredZoom(chart, axisNames, zoomFactors, zoomPositions) {
    let translateX;
    let translateY;
    if (chart.zoomBase.zoomSettings.enableDeferredZooming) {
        translateX = chart.mouseX - chart.zoomBase.mouseDownX;
        translateY = chart.mouseY - chart.zoomBase.mouseDownY;
        switch (chart.zoomBase.zoomSettings.mode) {
            case 'X':
                translateY = 0;
                break;
            case 'Y':
                translateX = 0;
                break;
        }
        setTransform(translateX, translateY, 0, 0, chart, false);
    }
    else {
        chart.dotnetref.invokeMethodAsync('ChartPan', axisNames, zoomFactors, zoomPositions);
    }
    chart.zoomBase.previousMouseMoveX = chart.mouseX;
    chart.zoomBase.previousMouseMoveY = chart.mouseY;
}

export function calculatePinchZoomFactor(chart, pinchRect) {
    const zoomMode = chart.zoomBase.zoomingModule.zooming.mode;
    let rangeMin;
    let rangeMax;
    let pinchValue;
    let axisTrans;
    const zoomedAxisCollection = [];
    let zoomFactor;
    let zoomPosition;
    for (let i = 0; i < chart.userInteractionBase.axes.length; i++) {
        const axis = chart.userInteractionBase.axes[parseInt(i.toString(), 10)];
        zoomFactor = axis.zoomFactor;
        zoomPosition = axis.zoomPosition;
        if ((axis.orientation === 'Horizontal' && zoomMode !== 'Y') || (axis.orientation === 'Vertical' && zoomMode !== 'X')) {
            if (axis.orientation === 'Horizontal') {
                pinchValue = pinchRect.x - chart.zoomBase.zoomingModule.offset.x;
                axisTrans = axis.rect.w / chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].delta;
                rangeMin = (pinchValue / axisTrans) + chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].min;
                pinchValue = pinchRect.x + pinchRect.width - chart.zoomBase.zoomingModule.offset.x;
                rangeMax = (pinchValue / axisTrans) + chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].min;
            }
            else {
                pinchValue = pinchRect.y - chart.zoomBase.zoomingModule.offset.y;
                axisTrans = axis.rect.h / chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].delta;
                rangeMin = (((pinchValue * -1) + axis.rect.h) / axisTrans) + chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].min;
                pinchValue = pinchRect.y + pinchRect.height - chart.zoomBase.zoomingModule.offset.y;
                rangeMax = (((pinchValue * -1) + axis.rect.h) / axisTrans) + chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].min;
            }
            const selectionMin = Math.min(rangeMin, rangeMax);
            const selectionMax = Math.max(rangeMin, rangeMax);
            const currentZP = (selectionMin - chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].actualMin) / chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].actualDelta;
            const currentZF = (selectionMax - selectionMin) / chart.zoomBase.zoomingModule.zoomAxes[parseInt(i.toString(), 10)].actualDelta;
            zoomPosition = currentZP < 0 ? 0 : currentZP;
            zoomFactor = currentZF > 1 ? 1 : (currentZF < 0.03) ? 0.03 : currentZF;
        }
        zoomedAxisCollection.push({
            axisName: axis.name,
            zoomFactor: zoomFactor,
            zoomPosition: zoomPosition,
            axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
        });
    }
    const onZoomingEventArgs = {
        name: 'OnZooming', axisCollection: zoomedAxisCollection
    };
    chart.zoomBase.zoomingEventArgs = onZoomingEventArgs;
    if (!onZoomingEventArgs.cancel) {
        for (const axisData of zoomedAxisCollection) {
            chart.userInteractionBase.axes.filter((axis) => {
                if (axis.name === axisData.axisName) {
                    axis.zoomFactor = axisData.zoomFactor;
                    axis.zoomPosition = axisData.zoomPosition;
                }
            });
        }
    }
}

export function mouseCancelHandler(chart) {
    if (chart.zoomBase.zoomingModule && chart.zoomBase.zoomingModule.isZoomed) {
        clearSelectionRect(chart.element.id + '_ZoomArea');
        performZoomRedraw(chart);
        if (chart.zoomBase.zoomingModule.isPanning && chart.zoomBase.isChartDrag) {
            chart.zoomBase.zoomingModule.isPanning = chart.zoomBase.isChartDrag = false;
        }
        if (chart.zoomBase.zoomSettings.isOnZoomEndCalled && !chart.zoomToolkitBase.isReset && chart.zoomBase.zoomingModule.zoomAxes.length > 0) {
            chart.dotnetref.invokeMethodAsync('TriggerZoomingEvents', 'OnZoomEnd', false);
        }
    }
    chart.zoomBase.zoomingModule.pinchTarget = null;
    chart.zoomBase.zoomingModule.touchStartList = [];
    chart.zoomBase.zoomingModule.touchMoveList = [];
}

export function createClipRect(chart) {
    const options = {
        'id': chart.element.id + '_ChartAreaClipRect_',
        'x': chart.userInteractionBase.axisClipRect.x,
        'y': chart.userInteractionBase.axisClipRect.y,
        'width': chart.userInteractionBase.axisClipRect.width,
        'height': chart.userInteractionBase.axisClipRect.height,
        'fill': 'transparent',
        'stroke-width': 1,
        'stroke': 'Gray'
    };
    const clipPath = chart.userInteractionBase.svgRenderer.drawClipPath(options);
    const seriesElement = getElement(chart.element.id + 'SeriesCollection');
    if (seriesElement) {
        seriesElement.appendChild(clipPath);
    }
}

export function updateClipRect(clipRect, chart) {
    if (clipRect) {
        const axisClipRect = chart.userInteractionBase.axisClipRect;
        clipRect.setAttribute('x', axisClipRect.x.toString());
        clipRect.setAttribute('y', axisClipRect.y.toString());
        clipRect.setAttribute('width', axisClipRect.width.toString());
        clipRect.setAttribute('height', axisClipRect.height.toString());
        clipRect.parentNode.parentNode.parentNode.appendChild(clipRect.parentNode.parentNode);
    }
}

export function isAxisZoomed(axes) {
    let isAxisZoomed = false;
    for (const axis of axes) {
        if (axis.zoomFactor !== 1 || axis.zoomPosition !== 0) {
            isAxisZoomed = true;
        }
    }
    return isAxisZoomed;
}

export function applyZoomingToolkit(chart) {
    if (getElement(chart.element.id + chart.zoomBase.zoomToolkitId)) {
        if (isAxisZoomed(chart.userInteractionBase.axes)) {
            chart.zoomBase.zoomingModule.isZoomed = true;
            if (chart.zoomBase.zoomingModule.isPanning) {
                zoomToolkitPan(chart);
            }
        }
        else {
            chart.zoomBase.zoomingModule.isZoomed = chart.zoomBase.zoomingModule.isPanning = false;
            setAttribute(chart.element.id + '_svg', 'cursor', 'auto');
        }
    }
    else if (chart.zoomBase && chart.zoomBase.zoomingModule) {
        chart.zoomBase.zoomingModule.isPanning = isAxisZoomed(chart.userInteractionBase.axes) && chart.zoomBase.zoomSettings.enablePan;
        setAttribute(chart.element.id + '_svg', 'cursor', chart.zoomBase.zoomingModule.isPanning ? 'pointer' : 'auto');
    }
}

export function zoomToolkitZoom(chart) {
    chart.zoomBase.zoomingModule.isPanning = false;
    const elementOpacity = '1';
    setAttribute(chart.element.id + '_svg', 'cursor', 'auto');
    setAttribute(chart.element.id + chart.zoomBase.zoomToolkitZoomIn, 'opacity', elementOpacity);
    setAttribute(chart.element.id + chart.zoomBase.zoomToolkitZoomOut, 'opacity', chart.zoomBase.zoomSettings.toolbarDisplayMode === 'Always' && !chart.zoomBase.zoomingModule.isZoomed ? '0.2' : elementOpacity);
    applySelection(chart.element.id + chart.zoomBase.zoomToolkitZoom, chart.zoomToolkitBase.selectionColor);
    applySelection(chart.element.id + chart.zoomBase.zoomToolkitPan, chart.zoomToolkitBase.selectionColor);
    if (chart.zoomToolkitBase.selectedId) {
        setAttribute(chart.zoomToolkitBase.selectedId, 'fill', 'transparent');
    }
    chart.zoomToolkitBase.selectedId = chart.element.id + chart.zoomBase.zoomToolkitZoom + '_1';
    setAttribute(chart.zoomToolkitBase.selectedId, 'fill', chart.zoomToolkitBase.iconRectSelectionFill);
}

export function zoomToolkitPan(chart) {
    if (chart.zoomBase.zoomingModule.isZoomed) {
        chart.zoomBase.zoomingModule.isPanning = !(chart.zoomBase.zoomSettings.toolbarDisplayMode === 'Always') || chart.zoomBase.zoomingModule.isZoomed;
        setAttribute(chart.element.id + '_svg', 'cursor', 'pointer');
        const elementOpacity = '0.2';
        setAttribute(chart.element.id + chart.zoomBase.zoomToolkitZoomIn, 'opacity', chart.zoomBase.zoomSettings.toolbarDisplayMode === 'Always' && !chart.zoomBase.zoomingModule.isZoomed ? '1' : elementOpacity);
        setAttribute(chart.element.id + chart.zoomBase.zoomToolkitZoomOut, 'opacity', elementOpacity);
        applySelection(chart.element.id + chart.zoomBase.zoomToolkitZoom, chart.zoomToolkitBase.selectionColor);
        applySelection(chart.element.id + chart.zoomBase.zoomToolkitPan, chart.zoomToolkitBase.selectionColor);
        if (chart.zoomToolkitBase.selectedId) {
            setAttribute(chart.zoomToolkitBase.selectedId, 'fill', 'transparent');
        }
        chart.zoomToolkitBase.selectedId = chart.element.id + chart.zoomBase.zoomToolkitPan + '_1';
        setAttribute(chart.zoomToolkitBase.selectedId, 'fill', chart.zoomToolkitBase.iconRectSelectionFill);
    }
}

export function zoomToolkitZoomIn(chart, elementOpacity) {
    chart.zoomBase.zoomingModule.isZoomed = true;
    chart.zoomToolkitBase.selectedId = chart.element.id + chart.zoomBase.zoomToolkitZoom + '_1';
    zoomInOutCalculation(1, chart, elementOpacity);
}

export function zoomToolkitZoomOut(chart, elementOpacity) {
    zoomInOutCalculation(-1, chart, elementOpacity);
}

export function zoomToolkitReset(chart) {
    if (chart.clearToolkitReset) {
        clearTimeout(chart.clearToolkitReset);
    }
    chart.clearToolkitReset = setTimeout(() => {
        const zoomedAxisCollection = [];
        for (const axis of chart.userInteractionBase.axes) {
            axis.zoomFactor = 1;
            axis.zoomPosition = 0;
            zoomedAxisCollection.push({
                axisName: axis.name,
                zoomFactor: axis.zoomFactor,
                zoomPosition: axis.zoomPosition,
                axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
            });
        }
        const onZoomingEventArgs = {
            name: 'OnZoomEnd',
            axisCollection: zoomedAxisCollection
        };
        if (!onZoomingEventArgs.cancel) {
            chart.zoomToolkitBase.isReset = true;
            removeTooltipCrosshairOnZoom(chart);
            zoomToolkitSetDeferredZoom(chart, onZoomingEventArgs);
            zoomToolkitRemoveTooltip(chart);
            const seriesCollection = document.getElementById(chart.element.id + 'SeriesCollection');
            if (seriesCollection && seriesCollection.hasAttribute('clip-path')) {
                seriesCollection.removeAttribute('clip-path');
            }
            if (!chart.zoomBase.zoomingModule.isDevice && !(chart.zoomBase.zoomSettings.toolbarDisplayMode === 'Always')) {
                const zoomKitElement = getElement(chart.element.id + chart.zoomBase.zoomToolkitId);
                if (zoomKitElement) {
                    zoomKitElement.style.visibility = 'hidden';
                }
            }
        }
    }, 100);
}

export function removeTooltipCrosshairOnZoom(chart) {
    const tooltip = getElement(chart.element.id + '_tooltip_svg');
    const crosshair = getElement(chart.element.id + '_UserInteraction');
    const axisGroup = getElement(chart.element.id + '_crosshair_axis');
    if (tooltip) {
        tooltip.remove();
        removeHighlight(chart);
    }
    removeMarker(chart);
    if (crosshair && crosshair.getAttribute('opacity') !== '0') {
        crosshair.setAttribute('opacity', '0');
        if (axisGroup) {
            while (axisGroup.firstChild) {
                axisGroup.removeChild(axisGroup.firstChild);
            }
        }
    }
    chart.tooltipBase.valueX = null;
    chart.tooltipBase.valueY = null;
    chart.tooltipBase.currentPoints = [];
    chart.tooltipBase.previousPoints = [];
    chart.markerExplodeBase.markerPreviousPoints = [];
}

export function zoomInOutCalculation(scale, chart, elementOpacity) {
    if (!chart.zoomBase.zoomingModule.isPanning && (elementOpacity !== '0.2' || chart.zoomBase.zoomSettings.toolbarDisplayMode === 'Always')) {
        if (chart.zoomBase.zoomSettings.isOnZoomStartCalled) {
            chart.dotnetref.invokeMethodAsync('TriggerZoomingEvents', 'OnZoomStart', false);
            chart.zoomBase.zoomingModule.isZoomStart = false;
        }
        const mode = chart.zoomBase.zoomingModule.zooming.mode;
        let cumulative;
        let zoomFactor;
        let zoomPosition;
        chart.userInteractionBase.disableTrackTooltip = chart.zoomBase.delayRedraw = true;
        const zoomedAxisCollection = [];
        for (const axis of chart.userInteractionBase.axes) {
            if ((axis.orientation === 'Horizontal' && mode !== 'Y') || (axis.orientation === 'Vertical' && mode !== 'X')) {
                cumulative = Math.max(Math.max(1 / minMax(axis.zoomFactor, 0, 1), 1) + (0.25 * scale), 1);
                zoomFactor = cumulative === 1 ? 1 : minMax(1 / cumulative, 0, 1);
                zoomPosition = cumulative === 1 ? 0 : axis.zoomPosition + ((axis.zoomFactor - zoomFactor) * 0.5);
                if (axis.zoomPosition !== zoomPosition || axis.zoomFactor !== zoomFactor) {
                    zoomFactor = (zoomPosition + zoomFactor) > 1 ? (1 - zoomPosition) : zoomFactor;
                }
                zoomedAxisCollection.push({
                    axisName: axis.name,
                    zoomFactor: zoomFactor,
                    zoomPosition: zoomPosition,
                    axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
                });
            }
            else {
                zoomedAxisCollection.push({
                    axisName: axis.name,
                    zoomFactor: axis.zoomFactor,
                    zoomPosition: axis.zoomPosition,
                    axisRange: getVisibleRangeModel(axis.visibleRange, axis.visibleInterval)
                });
            }
        }
        const onZoomingEventArgs = {
            name: 'OnZooming', axisCollection: zoomedAxisCollection
        };
        chart.zoomBase.zoomingEventArgs = onZoomingEventArgs;
        if (!onZoomingEventArgs.cancel) {
            for (const axisData of zoomedAxisCollection) {
                chart.userInteractionBase.axes.filter((axis) => {
                    if (axis.name === axisData.axisName) {
                        axis.zoomFactor = axisData.zoomFactor;
                        axis.zoomPosition = axisData.zoomPosition;
                    }
                });
            }
        }
    }
}

export function zoomToolkitRemoveTooltip(chart) {
    const hoverId = chart.zoomToolkitBase.hoverId;
    if (hoverId) {
        setAttribute(hoverId, 'fill', chart.zoomBase.zoomingModule.isPanning ? hoverId.indexOf('_Pan_') > -1 ? chart.zoomToolkitBase.iconRectSelectionFill : 'transparent' : hoverId.indexOf('_Zoom_') > -1 && chart.zoomBase.zoomingModule.isZoomed ? chart.zoomToolkitBase.iconRectSelectionFill : 'transparent');
        setAttribute(hoverId.replace('_1', '_2'), 'fill', chart.zoomBase.zoomingModule.isPanning ? hoverId.indexOf('_Pan_') > -1 ? chart.zoomToolkitBase.selectionColor : chart.zoomToolkitBase.fillColor : hoverId.indexOf('_Zoom_') > -1 ? chart.zoomToolkitBase.selectionColor : chart.zoomToolkitBase.fillColor);
        setAttribute(hoverId.replace('_1', '_3'), 'fill', chart.zoomBase.zoomingModule.isPanning ? chart.zoomToolkitBase.fillColor : hoverId.indexOf('_Zoom_') > -1 ? chart.zoomToolkitBase.selectionColor : chart.zoomToolkitBase.fillColor);
        DotNet.invokeMethodAsync('Syncfusion.Blazor.Toolkit', 'SetSelectedIcon', hoverId, false);
    }
    removeElement(chart.zoomBase.chartZoomTip);
}

export function zoomToolkitShowTooltip(chart, currentTarget, text, args) {
    zoomToolkitRemoveTooltip(chart);
    const textStyle = { size: '10px', fontWeight: 'Normal', fontStyle: 'Normal', fontFamily: 'Segoe UI' };
    const left = args.clientX - (svgbase.measureText(text, textStyle).width + 5);
    const rect = currentTarget + '_1';
    const currentElement = document.getElementById(currentTarget);
    
    chart.zoomToolkitBase.hoverId = rect;
    setAttribute(rect, 'fill', chart.zoomToolkitBase.iconRectOverFill);
    setAttribute(currentTarget + '_2', 'fill', chart.zoomToolkitBase.selectionColor);
    setAttribute(currentTarget + '_3', 'fill', chart.zoomToolkitBase.selectionColor);
    
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentElement) && currentElement.getAttribute('opacity') === '1' && currentElement.getAttribute('fill') !== chart.zoomToolkitBase.unSelectedFill) {
        document.getElementById(currentTarget).setAttribute('cursor', 'pointer');
    }
    else {
        document.getElementById(currentTarget).setAttribute('cursor', 'auto');
    }
    if (!chart.isTouch) {
        createTooltip(chart.zoomBase.chartZoomTip, text, args.clientY + 10, left, '10px');
    }
    DotNet.invokeMethodAsync('Syncfusion.Blazor.Toolkit', 'SetSelectedIcon', currentTarget, true);
}

export function zoomToolkitSetDeferredZoom(chart, onZoomingEventArgs) {
    chart.userInteractionBase.disableTrackTooltip = false;
    chart.zoomBase.zoomingModule.isZoomed = chart.zoomBase.zoomingModule.isPanning = chart.zoomBase.isChartDrag = chart.zoomBase.delayRedraw = false;
    chart.zoomBase.zoomingModule.touchMoveList = chart.zoomBase.zoomingModule.touchStartList = [];
    chart.zoomBase.zoomingModule.pinchTarget = null;
    chart.dotnetref.invokeMethodAsync('ZoomToolkitSetDeferredZoom', onZoomingEventArgs);
}

export function zoomToolkitTooltip(chart, targetId, e) {
    let currentTarget;
    const zoomBase = chart.zoomBase;
    if (targetId.indexOf(zoomBase.zoomToolkitZoom) > -1 && !(targetId.indexOf(zoomBase.zoomToolkitZoomIn) > -1 || targetId.indexOf(zoomBase.zoomToolkitZoomOut) > -1)) {
        currentTarget = chart.element.id + zoomBase.zoomToolkitZoom;
    }
    else if (targetId.indexOf(zoomBase.zoomToolkitZoomIn) > -1) {
        currentTarget = chart.element.id + zoomBase.zoomToolkitZoomIn;
    }
    else if (targetId.indexOf(zoomBase.zoomToolkitZoomOut) > -1) {
        currentTarget = chart.element.id + zoomBase.zoomToolkitZoomOut;
    }
    else if (targetId.indexOf(zoomBase.zoomToolkitPan) > -1) {
        currentTarget = chart.element.id + zoomBase.zoomToolkitPan;
    }
    else if (targetId.indexOf(zoomBase.zoomToolkitReset) > -1) {
        currentTarget = chart.element.id + zoomBase.zoomToolkitReset;
    }
    else {
        return;
    }
    if (currentTarget) {
        zoomToolkitShowTooltip(chart, currentTarget, getElement(currentTarget).getAttribute('data-text'), e);
    }
}

export function zoomToolkitMouseDown(chart, target) {
    const zoomBase = chart.zoomBase;
    if (target.id.indexOf(zoomBase.zoomToolkitZoom) > -1 && !(target.id.indexOf(zoomBase.zoomToolkitZoomIn) > -1 || target.id.indexOf(zoomBase.zoomToolkitZoomOut) > -1)) {
        zoomToolkitZoom(chart);
    }
    else if (target.id.indexOf(zoomBase.zoomToolkitZoomIn) > -1) {
        zoomToolkitZoomIn(chart, target.getAttribute('opacity'));
    }
    else if (target.id.indexOf(zoomBase.zoomToolkitZoomOut) > -1) {
        zoomToolkitZoomOut(chart, target.getAttribute('opacity'));
    }
    else if (target.id.indexOf(zoomBase.zoomToolkitPan) > -1) {
        zoomToolkitPan(chart);
    }
    else if (target.id.indexOf(zoomBase.zoomToolkitReset) > -1 || withInZoomToolkitRect(chart, zoomBase, target)) {
        zoomToolkitReset(chart);
    }
    else {
        return;
    }
}

export function withInZoomToolkitRect(chart, zoomBase, target) {
    let isWithinZoomToolkit = false;
    if (zoomBase.zoomingModule && zoomBase.zoomingModule.browser.isDevice && target.closest('#' + chart.element.id + zoomBase.zoomToolkitId)) {
        isWithinZoomToolkit = true;
    }
    return isWithinZoomToolkit;
}

export function setScrollbarOptions(dataId, scrollbarOptions) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(currentInstance)) {
        currentInstance.scrollbarBase = currentInstance.scrollbarBase ? currentInstance.scrollbarBase : {};
        currentInstance.scrollbarBase.isResize = scrollbarOptions.isResize;
        currentInstance.scrollbarBase.axes = scrollbarOptions.axes;
        currentInstance.scrollbarBase.scrollbarThemeStyle = scrollbarOptions.scrollbarThemeStyle;
        currentInstance.scrollbarBase.chartAreaType = scrollbarOptions.chartAreaType;
        currentInstance.scrollbarBase.isScrollExist = scrollbarOptions.isScrollExist;
        currentInstance.scrollbarBase.isScrollEventCalled = scrollbarOptions.isScrollEventCalled;
        currentInstance.scrollbarBase.chartTitleHeight = scrollbarOptions.chartTitleHeight;
        currentInstance.scrollbarBase.chartTitlePosition = scrollbarOptions.chartTitlePosition;
        currentInstance.scrollbarBase.chartSubTitleHeight = scrollbarOptions.chartSubTitleHeight;
        currentInstance.scrollbarBase.margin = scrollbarOptions.margin;
        currentInstance.scrollbarBase.isLegendVisible = scrollbarOptions.isLegendVisible;
        currentInstance.scrollbarBase.markerHeight = scrollbarOptions.markerHeight;
        currentInstance.scrollbarBase.enablePadding = scrollbarOptions.enablePadding;
        // Initialize scrollbar axis based values
        currentInstance.scrollbarBase.scrollbarOptions = currentInstance.scrollbarBase.scrollbarOptions ? currentInstance.scrollbarBase.scrollbarOptions : {};
    }
}

export function renderScrollbar(dataId, axes) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    chart.scrollBarsId = [];
    chart.axes = chart.scrollbarBase.axes = axes;
    chart.calculateSecondaryOffset(chart.element.id);
    let axis;
    let zoomModule;
    let isZoomed;
    chart.scrollbarBase.topScrollBarCount = 0;
    chart.scrollbarBase.bottomScrollBarCount = 0;
    chart.scrollbarBase.leftScrollBarCount = 0;
    chart.scrollbarBase.rightScrollBarCount = 0;
    if (sfBlazorToolkit.base.isNullOrUndefined(chart.zoomBase.zoomingModule)) {
        zoomModule = false;
    }
    else {
        zoomModule = chart.zoomBase.zoomingModule;
        isZoomed = zoomModule.isZoomed ? zoomModule.isZoomed : isAxisZoomed(chart.userInteractionBase.axes);
    }
    chart.scrollbarBase.chartId = chart.id;
    for (let i = 0; i < chart.axes.length; i++) {
        axis = chart.axes[i];
        chart.scrollbarBase.axis = axis;
        if (axis && !axis.visible) {
            removeScrollSvg(chart, axis);
            continue;
        }
        if (((isZoomed && (axis.zoomFactor < 1 || axis.zoomPosition > 0)) || (axis.scrollbarSettings.enable &&
            (axis.zoomFactor <= 1 || axis.zoomPosition >= 0)))) {
            if (!chart.scrollElement && axis.visible) {
                chart.scrollElement = document.getElementById(chart.id + '_scrollElement');
            }
            if (zoomModule || (chart.scrollbarBase.isScrollExist && axis.scrollbarSettings.enable)) {
                getDefaults(chart, axis);
            }
            removeScrollSvg(chart, axis);
            createScrollSvg(axis, chart, chart.userInteractionBase.svgRenderer);
            chart.isScrollbar = true;
            chart.scrollbarBase.scrollbarOptions[axis.name].svgObject.appendChild(renderElements(chart, axis));
            chart.scrollElement.appendChild(chart.scrollbarBase.scrollbarOptions[axis.name].svgObject);
        }
        else {
            const scrollEle = getElement(chart.id + '_scrollElement');
            if (scrollEle.childNodes.length > 0) {
                const scrollSvg = document.getElementById(chart.id + '_scrollBar_svg' + axis.name);
                if (scrollSvg) {
                    scrollSvg.style.display = 'none';
                }
            }
        }
    }
}

export function renderElements(chart, axis) {
    const isInverse = axis.isAxisInverse;
    const scrollbar = chart.scrollbarBase;
    const scrollbarOptions = scrollbar.scrollbarOptions[axis.name];
    const renderer = chart.userInteractionBase.svgRenderer;
    const scrollGroup = renderer.createGroup({
        id: chart.id + 'scrollBar_' + axis.name,
        transform: 'translate(' + ((scrollbarOptions.isVertical && isInverse) ? scrollbarOptions.height : isInverse ?
            scrollbarOptions.width : '0') + ',' + (scrollbarOptions.isVertical && isInverse ? '0' : isInverse ?
                scrollbarOptions.height : scrollbarOptions.isVertical ? scrollbarOptions.width : '0') + ') rotate(' + (scrollbarOptions.isVertical && isInverse ?
                    '90' : scrollbarOptions.isVertical ? '270' : isInverse ? '180' : '0') + ')'
    });
    const backRectGroup = renderer.createGroup({
        id: chart.id + 'scrollBar_backRect_' + axis.name
    });
    const thumbGroup = renderer.createGroup({
        id: chart.id + 'scrollBar_thumb_' + axis.name,
        transform: 'translate(0,0)'
    });
    backRect(scrollbar, renderer, backRectGroup);
    thumb(scrollbar, renderer, thumbGroup);
    if (scrollbar.axis.scrollbarSettings.enableZoom) {
        renderCircle(scrollbar, renderer, thumbGroup, chart.userInteractionBase.theme);
        arrows(scrollbar, renderer, thumbGroup);
    }
    thumbGrip(scrollbar, renderer, thumbGroup, chart.userInteractionBase.theme.indexOf('Fluent') > -1);
    scrollGroup.appendChild(backRectGroup);
    scrollGroup.appendChild(thumbGroup);
    return scrollGroup;
}

export function backRect(scroll, renderer, parent) {
    const style = scroll.scrollbarThemeStyle;
    const height = scroll.axis.scrollbarSettings.height;
    const trackColor = !sfBlazorToolkit.base.isNullOrUndefined(scroll.axis.scrollbarSettings.trackColor) && scroll.axis.scrollbarSettings.trackColor !== '' ? scroll.axis.scrollbarSettings.trackColor : style.backRect;
    const backRectEle = renderer.drawRectangle(new RectOption(scroll.chartId + 'scrollBarBackRect_' + scroll.axis.name, trackColor, { width: 1, color: trackColor }, 1, new svgbase.Rect(0, 0, scroll.scrollbarOptions[scroll.axis.name].width, height), scroll.axis.scrollbarSettings.trackRadius, scroll.axis.scrollbarSettings.trackRadius));
    backRectEle.setAttribute('vector-effect', 'non-scaling-stroke');
    parent.appendChild(backRectEle);
}

export function thumb(scroll, renderer, parent) {
    scroll.scrollbarOptions[scroll.axis.name].startX = scroll.scrollbarOptions[scroll.axis.name].thumbRectX;
    const style = scroll.scrollbarThemeStyle;
    const height = scroll.axis.scrollbarSettings.height;
    const scrollbarColor = !sfBlazorToolkit.base.isNullOrUndefined(scroll.axis.scrollbarSettings.scrollbarColor) && scroll.axis.scrollbarSettings.scrollbarColor !== '' ? scroll.axis.scrollbarSettings.scrollbarColor : style.thumb;
    scroll.scrollbarOptions[scroll.axis.name].slider = renderer.drawRectangle(new RectOption(scroll.chartId + 'scrollBarThumb_' + scroll.axis.name, scrollbarColor, { width: 1, color: '' }, 1, new svgbase.Rect(scroll.scrollbarOptions[scroll.axis.name].thumbRectX, 0, scroll.scrollbarOptions[scroll.axis.name].thumbRectWidth, height), scroll.axis.scrollbarSettings.scrollbarRadius, scroll.axis.scrollbarSettings.scrollbarRadius));
    parent.appendChild(scroll.scrollbarOptions[scroll.axis.name].slider);
}

export function renderCircle(scroll, renderer, parent, theme) {
    const style = scroll.scrollbarThemeStyle;
    const height = scroll.axis.scrollbarSettings.height;
    const circleSize = height / 2;
    const option = new CircleOption(scroll.chartId + 'scrollBar_leftCircle_' + scroll.axis.name, style.circle, { width: 1, color: style.circle }, 1, scroll.scrollbarOptions[scroll.axis.name].thumbRectX, circleSize, circleSize);
    const scrollShadowEle = '<filter x="-25.0%" y="-20.0%" width="150.0%" height="150.0%" filterUnits="objectBoundingBox"' +
        'id="scrollbar_shadow"><feOffset dx="0" dy="1" in="SourceAlpha" result="shadowOffsetOuter1"></feOffset>' +
        '<feGaussianBlur stdDeviation="1.5" in="shadowOffsetOuter1" result="shadowBlurOuter1"></feGaussianBlur>' +
        '<feComposite in="shadowBlurOuter1" in2="SourceAlpha" operator="out" result="shadowBlurOuter1"></feComposite>' +
        '<feColorMatrix values="0 0 0 0 0   0 0 0 0 0   0 0 0 0 0  0 0 0 0.16 0" type="matrix" in="shadowBlurOuter1">' +
        '</feColorMatrix></filter>';
    const defElement = renderer.createDefs();
    const shadowGroup = renderer.createGroup({
        id: scroll.chartId + scroll.axis.name + '_thumb_shadow'
    });
    defElement.innerText = scrollShadowEle;
    shadowGroup.innerText = '<use fill="black" fill-opacity="1" filter="url(#scrollbar_shadow)" xlink:href="#' +
        scroll.chartId + 'scrollBar_leftCircle_' +
        scroll.axis.name + '"></use><use fill="black" fill-opacity="1" filter="url(#scrollbar_shadow)" xlink:href="#' +
        scroll.chartId + 'scrollBar_rightCircle_' + scroll.axis.name + '"></use>';
    scroll.scrollbarOptions[scroll.axis.name].leftCircleEle = renderer.drawCircle(option);
    option.id = scroll.chartId + 'scrollBar_rightCircle_' + scroll.axis.name;
    option.cx = scroll.scrollbarOptions[scroll.axis.name].thumbRectX + scroll.scrollbarOptions[scroll.axis.name].thumbRectWidth;
    scroll.scrollbarOptions[scroll.axis.name].rightCircleEle = renderer.drawCircle(option);
    parent.appendChild(defElement);
    parent.appendChild(scroll.scrollbarOptions[scroll.axis.name].leftCircleEle);
    parent.appendChild(scroll.scrollbarOptions[scroll.axis.name].rightCircleEle);
    parent.appendChild(shadowGroup);
}

export function arrows(scroll, renderer, parent) {
    const style = scroll.scrollbarThemeStyle;
    const option = new svgbase.PathOption(scroll.chartId + 'scrollBar_leftArrow_' + scroll.axis.name, style.arrow, 1, style.arrow, 1, '', '');
    const scrollbarOptions = scroll.scrollbarOptions[scroll.axis.name];
    scrollbarOptions.leftArrowEle = renderer.drawPath(option);
    option.id = scroll.chartId + 'scrollBar_rightArrow_' + scroll.axis.name;
    scrollbarOptions.rightArrowEle = renderer.drawPath(option);
    setArrowDirection(scroll);
    const height = scroll.axis.scrollbarSettings.height;
    const circleRadius = height / 2;
    const arrowHeight = circleRadius / 2;
    const leftDirection = 'M ' + (scrollbarOptions.thumbRectX - arrowHeight) + ' ' + circleRadius + ' ' +
        'L ' + (scrollbarOptions.thumbRectX - arrowHeight + (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius - arrowHeight) + ' ' +
        'L ' + (scrollbarOptions.thumbRectX - arrowHeight + (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius + arrowHeight) + ' Z';
    const rightDirection = 'M ' + (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + arrowHeight) + ' ' + circleRadius + ' ' +
        'L ' + (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + arrowHeight - (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius - arrowHeight) + ' ' +
        'L ' + (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + arrowHeight - (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius + arrowHeight) + ' Z';
    scrollbarOptions.leftArrowEle.setAttribute('d', leftDirection);
    scrollbarOptions.rightArrowEle.setAttribute('d', rightDirection);
    parent.appendChild(scrollbarOptions.leftArrowEle);
    parent.appendChild(scrollbarOptions.rightArrowEle);
}

export function setArrowDirection(scroll) {
    const scrollbarOptions = scroll.scrollbarOptions[scroll.axis.name];
    const height = scroll.axis.scrollbarSettings.height;
    const circleRadius = height / 2;
    const arrowHeight = circleRadius / 2;
    const leftDirection = 'M ' + (scrollbarOptions.thumbRectX - arrowHeight) + ' ' + circleRadius + ' ' +
        'L ' + (scrollbarOptions.thumbRectX - arrowHeight + (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius - arrowHeight) + ' ' +
        'L ' + (scrollbarOptions.thumbRectX - arrowHeight + (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius + arrowHeight) + ' Z';
    const rightDirection = 'M ' + (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + arrowHeight) + ' ' + circleRadius + ' ' +
        'L ' + (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + arrowHeight - (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius - arrowHeight) + ' ' +
        'L ' + (scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth + arrowHeight - (arrowHeight * Math.sqrt(3))) + ' ' + (circleRadius + arrowHeight) + ' Z';
    scrollbarOptions.leftArrowEle.setAttribute('d', leftDirection);
    scrollbarOptions.rightArrowEle.setAttribute('d', rightDirection);
}

export function thumbGrip(scroll, renderer, parent, isFiveGripStyle) {
    let sidePadding = 0;
    let topPadding = 0;
    const gripWidth = 14;
    const gripCircleDiameter = 2;
    const padding = gripWidth / 2 - gripCircleDiameter;
    const style = scroll.scrollbarThemeStyle;
    const gripColor = !sfBlazorToolkit.base.isNullOrUndefined(scroll.axis.scrollbarSettings.gripColor) && scroll.axis.scrollbarSettings.gripColor ? scroll.axis.scrollbarSettings.gripColor : style.grip;
    const option = new CircleOption(scroll.chartId + 'scrollBar_gripCircle0' + '_' + scroll.axis.name, gripColor, { width: 1, color: gripColor }, 1, 0, 0, 1);
    const scrollbarOptions = scroll.scrollbarOptions[scroll.axis.name];
    const height = scroll.axis.scrollbarSettings.height;
    const verticalTranslateHeight = (height / 2) + (padding / 2) - 0.5;
    const horizontalTranslateHeight = (height / 2) - (padding / 2) - 0.5;
    scrollbarOptions.gripCircle = renderer.createGroup({
        id: scroll.chartId + 'scrollBar_gripCircle_' + scroll.axis.name,
        transform: 'translate(' + ((scrollbarOptions.thumbRectX + scrollbarOptions.thumbRectWidth / 2) + ((scrollbarOptions.isVertical ? 1 : -1) * padding) - (isFiveGripStyle ? (scrollbarOptions.isVertical ? -4 : 4) : 0)) +
            ',' + (scrollbarOptions.isVertical ? verticalTranslateHeight : horizontalTranslateHeight) + ') rotate(' + (scrollbarOptions.isVertical ? '180' : '0') + ')'
    });
    for (let i = 1; i <= (isFiveGripStyle ? 10 : 6); i++) {
        option.id = scroll.chartId + 'scrollBar_gripCircle' + i + '_' + scroll.axis.name;
        option.cx = sidePadding;
        option.cy = topPadding;
        scrollbarOptions.gripCircle.appendChild(renderer.drawCircle(option));
        sidePadding = i === (isFiveGripStyle ? 5 : 3) ? 0 : (sidePadding + 5);
        topPadding = i >= (isFiveGripStyle ? 5 : 3) ? 5 : 0;
    }
    if (scroll.axis.scrollbarSettings.height >= 12) {
        parent.appendChild(scrollbarOptions.gripCircle);
    }
}

export function getDefaults(chart, axis) {
    const circleRadius = chart.scrollbarBase.axis.scrollbarSettings.height / 2;
    const padding = 5;
    const gripWidth = 14;
    const minThumbWidth = circleRadius * 2 + padding * 2 + gripWidth;
    if (!chart.scrollbarBase.scrollbarOptions[axis.name]) {
        chart.scrollbarBase.scrollbarOptions[axis.name] = {};
    }
    const currentScrollbarOptions = chart.scrollbarBase.scrollbarOptions[axis.name];
    if (axis.scrollbarSettings.enable) {
        currentScrollbarOptions.isLazyLoad = true;
        getLazyDefaults(chart, axis);
    }
    currentScrollbarOptions.isVertical = axis.orientation === 'Vertical';
    currentScrollbarOptions.zoomFactor = currentScrollbarOptions.isLazyLoad ? currentScrollbarOptions.zoomFactor : axis.zoomFactor;
    currentScrollbarOptions.zoomPosition = currentScrollbarOptions.isLazyLoad ? currentScrollbarOptions.zoomPosition : axis.zoomPosition;
    let currentWidth = currentScrollbarOptions.zoomFactor * (currentScrollbarOptions.isVertical ? axis.rect.h : axis.rect.w);
    currentWidth = currentWidth > minThumbWidth ? currentWidth : minThumbWidth;
    currentScrollbarOptions.scrollX = axis.rect.x;
    currentScrollbarOptions.scrollY = axis.rect.y;
    currentScrollbarOptions.width = currentScrollbarOptions.isVertical ? axis.rect.h : axis.rect.w;
    currentScrollbarOptions.height = axis.scrollbarSettings.height;
    let currentX = currentScrollbarOptions.zoomPosition * (currentScrollbarOptions.isVertical ? axis.rect.h : currentScrollbarOptions.width);
    currentX = currentScrollbarOptions.isLazyLoad && currentX === 0 && currentScrollbarOptions.startX ? currentScrollbarOptions.startX : currentX;
    const maxThumbX = currentScrollbarOptions.width - currentWidth - circleRadius;
    currentScrollbarOptions.thumbRectX = Math.min(Math.max(currentX, circleRadius), maxThumbX);
    currentScrollbarOptions.thumbRectWidth = chart.scrollbarBase.isThumbDrag || chart.isWheelScrolling ? currentScrollbarOptions.thumbRectWidth : currentWidth;
}

export function getLazyDefaults(chart, axis) {
    let start;
    let end;
    let valueType = axis.valueType;
    const scrollbarSettings = axis.scrollbarSettings;
    const range = axis.scrollbarSettings.range;
    const visibleRange = axis.visibleRange;
    const pointsLength = axis.scrollbarSettings.pointsLength;
    const currentScrollbarOptions = chart.scrollbarBase.scrollbarOptions[axis.name];
    currentScrollbarOptions.valueType = valueType = (!sfBlazorToolkit.base.isNullOrUndefined(scrollbarSettings.range.minimum) || !sfBlazorToolkit.base.isNullOrUndefined(scrollbarSettings.range.maximum)) &&
        scrollbarSettings.pointsLength ? 'Double' : valueType;
    switch (valueType) {
        case 'Double':
        case 'Category':
        case 'Logarithmic':
            start = range.minimum ? range.minimum : pointsLength ? 0 : visibleRange.start;
            end = range.maximum ? range.maximum : pointsLength ? (pointsLength - 1) : visibleRange.end;
            break;
        case 'DateTime':
        case 'DateTimeCategory':
            start = range.minimum ? Date.parse(range.minimum) : visibleRange.start;
            end = range.maximum ? Date.parse(range.maximum) : visibleRange.end;
            break;
    }
    if (axis.valueType !== 'Category') {
        start = Math.min(start, visibleRange.start);
        end = Math.max(end, visibleRange.end);
    }
    const zoomFactor = (visibleRange.end - visibleRange.start) / (end - start);
    const zoomPosition = (visibleRange.start - start) / (end - start);
    currentScrollbarOptions.zoomFactor = range.minimum || range.maximum ? zoomFactor :
        currentScrollbarOptions.zoomFactor ? currentScrollbarOptions.zoomFactor : (axis.maxPointLength / axis.scrollbarSettings.pointsLength);
    currentScrollbarOptions.zoomPosition = range.minimum || range.maximum ? zoomPosition :
        currentScrollbarOptions.zoomPosition ? currentScrollbarOptions.zoomPosition : axis.zoomPosition;
    currentScrollbarOptions.scrollRange = {
        start: start,
        end: end,
        delta: axis.valueType === 'Category' && !range.maximum ? pointsLength : end - start
    };
    currentScrollbarOptions.previousStart = visibleRange.start;
    currentScrollbarOptions.previousEnd = visibleRange.end;
}

export function createScrollSvg(axis, chart, renderer) {
    const rect = axis.rect;
    const isHorizontalAxis = axis.orientation === 'Horizontal';
    const shouldAddPadding = chart.scrollbarBase.enablePadding && isHorizontalAxis && (axis.scrollbarSettings.position === 'Top' || axis.scrollbarSettings.position === 'Bottom');
    const currentScrollbarOptions = chart.scrollbarBase.scrollbarOptions[axis.name];
    let topOffset = (axis.isAxisOppositePosition && isHorizontalAxis ? -16 :
        (shouldAddPadding ? chart.scrollbarBase.markerHeight : 0)) + rect.y + Math.max(0.5, axis.axisLineStyleWidth / 2) + chart.userInteractionBase.secondaryElementOffset.top;
    let leftOffset = (((axis.isAxisOppositePosition && !isHorizontalAxis ? 16 : 0) + rect.x) -
        (currentScrollbarOptions.isVertical ? currentScrollbarOptions.height : 0)) + chart.userInteractionBase.secondaryElementOffset.left;
    if (!isHorizontalAxis && (axis.scrollbarSettings.position === 'Left' || axis.scrollbarSettings.position === 'Right')) {
        leftOffset = calculateScrollbarOffset(currentScrollbarOptions, chart);
    }
    else if (isHorizontalAxis && (axis.scrollbarSettings.position === 'Top' || axis.scrollbarSettings.position === 'Bottom')) {
        topOffset = calculateScrollbarOffset(currentScrollbarOptions, chart);
    }
    const cursorType = chart.scrollbarBase.isThumbDrag && currentScrollbarOptions.isLazyLoad && axis.scrollbarSettings.height >= 12 ? '-webkit-grabbing' : 'auto';
    const scrollSvgId = chart.element.id + '_' + 'scrollBar_svg' + axis.name;
    chart.scrollBarsId.push(scrollSvgId);
    currentScrollbarOptions.svgObject = renderer.createSvg({
        id: scrollSvgId,
        width: currentScrollbarOptions.isVertical ? currentScrollbarOptions.height : currentScrollbarOptions.width,
        height: currentScrollbarOptions.isVertical ? currentScrollbarOptions.width : currentScrollbarOptions.height,
        style: 'position: absolute;top: ' + topOffset + 'px;left: ' + leftOffset + 'px;cursor:' + cursorType + ';'
    });
}

export function calculateScrollbarOffset(currentScrollbarOptions, chart) {
    const scrollbarPadding = 5;
    const scrollbarPosition = chart.scrollbarBase.axis.scrollbarSettings.position;
    const titlePosition = chart.scrollbarBase.chartTitlePosition;
    const titlePadding = titlePosition === 'Top' || (titlePosition === 'Bottom' && !chart.scrollbarBase.isLegendVisible) ? 15 : 10;
    const titleOffset = (titlePosition === scrollbarPosition) ? chart.scrollbarBase.chartTitleHeight + chart.scrollbarBase.chartSubTitleHeight + titlePadding : 0;
    let scrollbarOffsetValue;
    switch (scrollbarPosition) {
        case 'Left':
            scrollbarOffsetValue = chart.scrollbarBase.margin.left + scrollbarPadding + ((currentScrollbarOptions.height + scrollbarPadding) * chart.scrollbarBase.leftScrollBarCount) + titleOffset;
            chart.scrollbarBase.leftScrollBarCount++;
            break;
        case 'Right':
            scrollbarOffsetValue = chart.userInteractionBase.availableSize.width - (((currentScrollbarOptions.height + scrollbarPadding) * chart.scrollbarBase.rightScrollBarCount)
                + currentScrollbarOptions.height + scrollbarPadding + chart.scrollbarBase.margin.right + titleOffset);
            chart.scrollbarBase.rightScrollBarCount++;
            break;
        case 'Top':
            scrollbarOffsetValue = chart.scrollbarBase.margin.top + scrollbarPadding + ((currentScrollbarOptions.height + scrollbarPadding) * chart.scrollbarBase.topScrollBarCount) + titleOffset;
            chart.scrollbarBase.topScrollBarCount++;
            break;
        case 'Bottom':
            scrollbarOffsetValue = chart.userInteractionBase.availableSize.height - (((currentScrollbarOptions.height + scrollbarPadding) * chart.scrollbarBase.bottomScrollBarCount)
                + currentScrollbarOptions.height + scrollbarPadding + chart.scrollbarBase.margin.bottom + titleOffset);
            chart.scrollbarBase.bottomScrollBarCount++;
            break;
    }
    return scrollbarOffsetValue;
}

export function removeScrollSvg(component, axis) {
    if (!sfBlazorToolkit.base.isNullOrUndefined(component) && document.getElementById(component.element.id + '_scrollBar_svg' + axis.name)) {
        sfBlazorToolkit.base.remove(document.getElementById(component.element.id + '_scrollBar_svg' + axis.name));
    }
}

export function removeTooltipCrosshair() {
    for (const key in window.sfBlazorToolkit.instances) {
        if (window.sfBlazorToolkit.instances[`${key}`]) {
            const chartInstance = window.sfBlazorToolkit.instances[`${key}`];
            if (!sfBlazorToolkit.base.isNullOrUndefined(chartInstance) && !sfBlazorToolkit.base.isNullOrUndefined(document.getElementById(chartInstance.id)) && chartInstance.dataId.indexOf('sfChart-') !== -1 && chartInstance.tooltipBase.tooltipModule && chartInstance.crosshairBase.crosshair) {
                removeTooltip(10, chartInstance);
                removeCrosshair(chartInstance, 10);
            }
        }
    }
}

export function removeScrollbarSvg(dataId, axes) {
    const chart = window.sfBlazorToolkit.base.getCompInstance(dataId);
    for (let i = 0; i < axes.length; i++) {
        const axis = axes[i];
        removeScrollSvg(chart, axis);
    }
    if (!sfBlazorToolkit.base.isNullOrUndefined(chart)) {
        chart.isScrollbar = false;
    }
}

export function renderStriplineTooltip(tooltipOptions, showHeaderLine, elementId, duration) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    const tooltipElement = document.getElementById(currentId + '_tooltip_svg');
    if (!currentInstance) {
        return false;
    }
    else if (currentInstance.tooltipBase.isRemove && isSeriesTooltipVisible(tooltipElement)) {
        removeStriplineTooltip(elementId, duration);
        return false;
    }
    if (currentInstance.striplineTooltipBase.toolTipInterval) {
        clearTimeout(currentInstance.striplineTooltipBase.toolTipInterval);
        currentInstance.striplineTooltipBase.toolTipInterval = null;
    }
    currentInstance.striplineTooltip = new svgbase.Tooltip(tooltipOptions);
    currentInstance.striplineTooltip.enableRTL = tooltipOptions.enableRTL;
    currentInstance.striplineTooltip.showHeaderLine = showHeaderLine;
    currentInstance.striplineTooltip.appendTo('#' + elementId);
    return true;
}

export function removeStriplineTooltip(elementId, duration) {
    const currentInstance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    const hostElement = getElement(elementId);
    const tooltipSvg = hostElement ? hostElement.querySelector('svg') : null;
    if (hostElement && currentInstance.striplineTooltip && tooltipSvg) {
        if (currentInstance.striplineTooltipBase.toolTipInterval) {
            clearTimeout(currentInstance.striplineTooltipBase.toolTipInterval);
            currentInstance.striplineTooltipBase.toolTipInterval = null;
        }
        if (tooltipSvg) {
            tooltipSvg.style.transition = `opacity ${duration / 1000}s`;
            tooltipSvg.style.opacity = '0';
        }
        currentInstance.striplineTooltipBase.toolTipInterval = +setTimeout(() => {
            if (hostElement) {
                while (hostElement.firstChild) {
                    hostElement.removeChild(hostElement.firstChild);
                }
                currentInstance.striplineTooltip = null;
                currentInstance.striplineTooltipBase.toolTipInterval = null;
            }
        }, duration);
    }
}

export function isSeriesTooltipVisible(element) {
    if (!element) {
        return false;
    }
    const cs = window.getComputedStyle(element);
    if (!cs) {
        return false;
    }
    return parseFloat(cs.opacity) > 0;
}
