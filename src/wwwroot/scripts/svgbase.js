window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.svgbase = (function (exports) {
    'use strict';

    /**
     * SVG Renderer – creates and manipulates SVG DOM elements
     * @private
     */
    class SvgRenderer {
        constructor(rootID) {
            //Internal Variables
            this.svgLink = 'http://www.w3.org/2000/svg';
            this.rootId = rootID;
        }

        // method to get the attributes value
        getOptionValue(options, key) {
            return options[key];
        }

        /**
         * To create a Html5 SVG element
         *
         * @param {SVGAttributes} options - Options to create SVG
         * @returns {Element} It returns a appropriate element
         */
        createSvg(options) {
            if (sfBlazorToolkit.base.isNullOrUndefined(options.id)) {
                options.id = this.rootId + '_svg';
            }
            this.svgObj = document.getElementById(options.id);
            if (sfBlazorToolkit.base.isNullOrUndefined(document.getElementById(options.id))) {
                this.svgObj = document.createElementNS(this.svgLink, 'svg');
            }
            this.svgObj = this.setElementAttributes(options, this.svgObj);
            this.setSVGSize(options.width, options.height);
            return this.svgObj;
        }

        // method to set the height and width for the SVG element
        setSVGSize(width, height) {
            var element = document.getElementById(this.rootId);
            var size = !sfBlazorToolkit.base.isNullOrUndefined(element) ? element.getBoundingClientRect() : null;
            if (sfBlazorToolkit.base.isNullOrUndefined(this.width) || this.width <= 0) {
                this.svgObj.setAttribute('width', width ? width.toString() : !sfBlazorToolkit.base.isNullOrUndefined(size) ? size.width.toString() : '0');
            }
            else {
                this.svgObj.setAttribute('width', this.width.toString());
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(this.height) || this.height <= 0) {
                this.svgObj.setAttribute('height', height ? height.toString() : '450');
            }
            else {
                this.svgObj.setAttribute('height', this.height.toString());
            }
        }

        /**
         * To draw a path
         *
         * @param {PathAttributes} options - Options to draw a path in SVG
         * @returns {Element} It returns a appropriate path
         */
        drawPath(options) {
            var path = document.getElementById(options.id);
            if (path === null) {
                path = document.createElementNS(this.svgLink, 'path');
            }
            path = this.setElementAttributes(options, path);
            return path;
        }

        /**
         * To draw a line
         *
         * @param {LineAttributes} options - Options to draw a line in SVG
         * @returns {Element} It returns a appropriate element
         */
        drawLine(options) {
            var line = document.getElementById(options.id);
            if (line === null) {
                line = document.createElementNS(this.svgLink, 'line');
            }
            line = this.setElementAttributes(options, line);
            return line;
        }

        /**
         * To draw a rectangle
         *
         * @param {BaseAttibutes} options - Required options to draw a rectangle in SVG
         * @returns {Element} It returns a appropriate element
         */
        drawRectangle(options) {
            var rectangle = document.getElementById(options.id);
            if (rectangle === null) {
                rectangle = document.createElementNS(this.svgLink, 'rect');
            }
            rectangle = this.setElementAttributes(options, rectangle);
            return rectangle;
        }

        /**
         * To draw a circle
         *
         * @param {CircleAttributes} options - Required options to draw a circle in SVG
         * @returns {Element} It returns a appropriate element
         */
        drawCircle(options) {
            var circle = document.getElementById(options.id);
            if (circle === null) {
                circle = document.createElementNS(this.svgLink, 'circle');
            }
            circle = this.setElementAttributes(options, circle);
            return circle;
        }

        /**
         * To draw a polyline
         *
         * @param {PolylineAttributes} options - Options required to draw a polyline
         * @returns {Element} It returns a appropriate element
         */
        drawPolyline(options) {
            var polyline = document.getElementById(options.id);
            if (polyline === null) {
                polyline = document.createElementNS(this.svgLink, 'polyline');
            }
            polyline = this.setElementAttributes(options, polyline);
            return polyline;
        }

        /**
         * To draw an ellipse
         *
         * @param {EllipseAttributes} options - Options required to draw an ellipse
         * @returns {Element} It returns a appropriate element
         */
        drawEllipse(options) {
            var ellipse = document.getElementById(options.id);
            if (ellipse === null) {
                ellipse = document.createElementNS(this.svgLink, 'ellipse');
            }
            ellipse = this.setElementAttributes(options, ellipse);
            return ellipse;
        }

        /**
         * To draw a polygon
         *
         * @param {PolylineAttributes} options - Options needed to draw a polygon in SVG
         * @returns {Element} It returns a appropriate element
         */
        drawPolygon(options) {
            var polygon = document.getElementById(options.id);
            if (polygon === null) {
                polygon = document.createElementNS(this.svgLink, 'polygon');
            }
            polygon = this.setElementAttributes(options, polygon);
            return polygon;
        }

        /**
         * To draw an image
         *
         * @param {ImageAttributes} options - Required options to draw an image in SVG
         * @returns {Element} It returns a appropriate element
         */
        drawImage(options) {
            var img = document.createElementNS(this.svgLink, 'image');
            img.setAttributeNS(null, 'height', options.height.toString());
            img.setAttributeNS(null, 'width', options.width.toString());
            img.setAttributeNS('http://www.w3.org/1999/xlink', 'href', options.href);
            img.setAttributeNS(null, 'x', options.x.toString());
            img.setAttributeNS(null, 'y', options.y.toString());
            img.setAttributeNS(null, 'id', options.id);
            img.setAttributeNS(null, 'visibility', options.visibility);
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.getOptionValue(options, 'clip-path'))) {
                img.setAttributeNS(null, 'clip-path', this.getOptionValue(options, 'clip-path'));
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(options.preserveAspectRatio)) {
                img.setAttributeNS(null, 'preserveAspectRatio', options.preserveAspectRatio);
            }
            return img;
        }

        /**
         * To draw a text
         *
         * @param {TextAttributes} options - Options needed to draw a text in SVG
         * @param {string} label - Label of the text
         * @returns {Element} It returns a appropriate element
         */
        createText(options, label) {
            var text = document.createElementNS(this.svgLink, 'text');
            text = this.setElementAttributes(options, text);
            if (!sfBlazorToolkit.base.isNullOrUndefined(label)) {
                text.textContent = label;
            }
            return text;
        }

        /**
         * To create a tSpan
         *
         * @param {TextAttributes} options - Options to create tSpan
         * @param {string} label - The text content which is to be rendered in the tSpan
         * @returns {Element} It returns a appropriate element
         */
        createTSpan(options, label) {
            var tSpanElement = document.createElementNS(this.svgLink, 'tspan');
            tSpanElement = this.setElementAttributes(options, tSpanElement);
            if (!sfBlazorToolkit.base.isNullOrUndefined(label)) {
                tSpanElement.textContent = label;
            }
            return tSpanElement;
        }

        /**
         * To create a title
         *
         * @param {string} text - The text content which is to be rendered in the title
         * @returns {Element} It returns a appropriate element
         */
        createTitle(text) {
            var titleElement = document.createElementNS(this.svgLink, 'title');
            titleElement.textContent = text;
            return titleElement;
        }

        /**
         * To create defs element in SVG
         *
         * @returns {Element} It returns a appropriate element
         */
        createDefs() {
            var defsElement = document.createElementNS(this.svgLink, 'defs');
            return defsElement;
        }

        /**
         * To create clip path in SVG
         *
         * @param {BaseAttibutes} options - Options needed to create clip path
         * @returns {Element} It returns a appropriate element
         */
        createClipPath(options) {
            var clipPathElement = document.createElementNS(this.svgLink, 'clipPath');
            clipPathElement = this.setElementAttributes(options, clipPathElement);
            return clipPathElement;
        }

        /**
         * To create foreign object in SVG
         *
         * @param {BaseAttibutes} options - Options needed to create foreign object
         * @returns {Element} It returns a appropriate element
         */
        createForeignObject(options) {
            var foreignObject = document.createElementNS(this.svgLink, 'foreignObject');
            foreignObject = this.setElementAttributes(options, foreignObject);
            return foreignObject;
        }

        /**
         * To create group element in SVG
         *
         * @param {BaseAttibutes} options - Options needed to create group
         * @returns {Element} It returns a appropriate element
         */
        createGroup(options) {
            var groupElement = document.createElementNS(this.svgLink, 'g');
            groupElement = this.setElementAttributes(options, groupElement);
            return groupElement;
        }

        /**
         * To create pattern in SVG
         *
         * @param {PatternAttributes} options - Required options to create pattern in SVG
         * @param {string} element - Specifies the name of the pattern
         * @returns {Element} It returns a appropriate element
         */
        createPattern(options, element) {
            var pattern = document.createElementNS(this.svgLink, element);
            pattern = this.setElementAttributes(options, pattern);
            return pattern;
        }

        /**
         * To create radial gradient in SVG
         *
         * @param {string[]} colors - Specifies the colors required to create radial gradient
         * @param {string} name - Specifies the name of the gradient
         * @param {RadialGradient} options - value for radial gradient
         * @returns {string} It returns color name
         */
        createRadialGradient(colors, name, options) {
            var colorName;
            if (!sfBlazorToolkit.base.isNullOrUndefined(colors[0].colorStop)) {
                var newOptions = {
                    'id': this.rootId + '_' + name + 'radialGradient',
                    'cx': options.cx + '%',
                    'cy': options.cy + '%',
                    'r': options.r + '%',
                    'fx': options.fx + '%',
                    'fy': options.fy + '%'
                };
                this.drawGradient('radialGradient', newOptions, colors);
                colorName = 'url(#' + this.rootId + '_' + name + 'radialGradient)';
            }
            else {
                colorName = colors[0].color.toString();
            }
            return colorName;
        }

        /**
         * To create linear gradient in SVG
         *
         * @param {GradientColor[]} colors - Array of string specifies the values for color
         * @param {string} name - Specifies the name of the gradient
         * @param {LinearGradient} options - Specifies the options for gradient
         * @returns {string} It returns color name
         */
        createLinearGradient(colors, name, options) {
            var colorName;
            if (!sfBlazorToolkit.base.isNullOrUndefined(colors[0].colorStop)) {
                var newOptions = {
                    'id': this.rootId + '_' + name + 'linearGradient',
                    'xValue': options.xValue + '%',
                    'yValue': options.yValue + '%',
                    'x2': options.x2 + '%',
                    'y2': options.y2 + '%'
                };
                this.drawGradient('linearGradient', newOptions, colors);
                colorName = 'url(#' + this.rootId + '_' + name + 'linearGradient)';
            }
            else {
                colorName = colors[0].color.toString();
            }
            return colorName;
        }

        /**
         * To render the gradient element in SVG
         *
         * @param {string} gradientType - Specifies the type of the gradient
         * @param {RadialGradient | LinearGradient} options - Options required to render a gradient
         * @param {string[]} colors - Array of string specifies the values for color
         * @returns {Element} It returns a appropriate element
         */
        drawGradient(gradientType, options, colors) {
            const defsElement = this.createDefs();
            let gradient = document.createElementNS(this.svgLink, gradientType);
            gradient = this.setElementAttributes(options, gradient);
            for (let i = 0; i < colors.length; i++) {
                var stopElement = document.createElementNS(this.svgLink, 'stop');
                stopElement.setAttribute('offset', colors[i].colorStop);
                stopElement.setAttribute('stop-color', colors[i].color);
                stopElement.setAttribute('stop-opacity', colors[i].opacity ? (colors[i].opacity) : '1');
                if (!sfBlazorToolkit.base.isNullOrUndefined(colors[i].style)) {
                    stopElement.style.cssText = colors[i].style;
                }
                gradient.appendChild(stopElement);
            }
            defsElement.appendChild(gradient);
            return defsElement;
        }

        /**
         * To render a clip path
         *
         * @param {BaseAttibutes} options - Options required to render a clip path
         * @returns {Element} It returns a appropriate element
         */
        drawClipPath(options) {
            const defsElement = this.createDefs();
            const clipPathElement = this.createClipPath({ 'id': options.id });
            options.id = options.id + '_Rect';
            const rectElement = this.drawRectangle(options);
            clipPathElement.appendChild(rectElement);
            defsElement.appendChild(clipPathElement);
            return defsElement;
        }

        /**
         * To create circular clip path in SVG
         *
         * @param {CircleAttributes} options - Options required to create circular clip path
         * @returns {Element} It returns a appropriate element
         */
        drawCircularClipPath(options) {
            const defsElement = this.createDefs();
            const clipPathElement = this.createClipPath({ 'id': options.id });
            options.id = options.id + '_Circle';
            const circleElement = this.drawCircle(options);
            clipPathElement.appendChild(circleElement);
            defsElement.appendChild(clipPathElement);
            return defsElement;
        }

        /**
         * To set the attributes to the element
         *
         * @param {SVGCanvasAttributes} options - Attributes to set for the element
         * @param {Element} element - The element to which the attributes need to be set
         * @returns {Element} It returns a appropriate element
         */
        setElementAttributes(options, element) {
            let keys = Object.keys(options);
            for (var i = 0; i < keys.length; i++) {
                if (keys[i] === 'style') {
                    element.style.cssText = options[keys[i]];
                }
                else {
                    element.setAttribute(keys[i], options[keys[i]]);
                }
            }
            return element;
        }

        /**
         * To create a Html5 canvas element
         * Dummy method for using canvas/svg render in the same variable name in chart control
         */
        createCanvas() {
            return null;
        }
    }

    /* eslint-disable no-case-declarations */
    /**
     * @private
     */
    class CanvasRenderer {
        constructor(rootID) {
            this.rootId = rootID;
        }

        // method to get the attributes value
        getOptionValue(options, key) {
            return options[key];
        }

        /**
         * To create a Html5 canvas element
         *
         * @param {BaseAttibutes} options - Options to create canvas
         * @returns {HTMLCanvasElement} Creating a canvas
         */
        createCanvas(options) {
            const canvasObj = document.createElement('canvas');
            canvasObj.setAttribute('id', this.rootId + '_canvas');
            this.ctx = canvasObj.getContext('2d');
            this.canvasObj = canvasObj;
            this.setCanvasSize(options.width, options.height);
            return this.canvasObj;
        }

        /**
         * To set the width and height for the Html5 canvas element
         *
         * @param {number} width - width of the canvas
         * @param {number} height - height of the canvas
         * @returns {void} Setting canvas size
         */
        setCanvasSize(width, height) {
            const element = document.getElementById(this.rootId);
            const size = !sfBlazorToolkit.base.isNullOrUndefined(element) ? element.getBoundingClientRect() : null;
            if (sfBlazorToolkit.base.isNullOrUndefined(this.width)) {
                this.canvasObj.setAttribute('width', width ? width.toString() : size.width.toString());
            }
            else {
                this.canvasObj.setAttribute('width', this.width.toString());
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(this.height)) {
                this.canvasObj.setAttribute('height', height ? height.toString() : '450');
            }
            else {
                this.canvasObj.setAttribute('height', this.height.toString());
            }
        }

        // To set the values to the attributes
        setAttributes(options) {
            this.ctx.lineWidth = this.getOptionValue(options, 'stroke-width');
            const dashArray = this.getOptionValue(options, 'stroke-dasharray');
            if (!sfBlazorToolkit.base.isNullOrUndefined(dashArray)) {
                const dashArrayString = dashArray.split(',');
                this.ctx.setLineDash([parseInt(dashArrayString[0], 10), parseInt(dashArrayString[1], 10)]);
            }
            this.ctx.strokeStyle = this.getOptionValue(options, 'stroke');
        }

        /**
         * To draw a line
         *
         * @param {LineAttributes} options - required options to draw a line on the canvas
         * @returns {void} To draw a line
         */
        drawLine(options) {
            this.ctx.save();
            this.ctx.beginPath();
            this.ctx.lineWidth = this.getOptionValue(options, 'stroke-width');
            this.ctx.strokeStyle = options.stroke;
            this.ctx.moveTo(options.xValue, options.yValue);
            this.ctx.lineTo(options.x2, options.y2);
            this.ctx.stroke();
            this.ctx.restore();
        }

        /**
         * To draw a rectangle
         *
         * @param {RectAttributes} options - required options to draw a rectangle on the canvas.
         * @param {Int32Array} canvasTranslate TO get a translate value of canvas.
         * @returns {void} To draw rectangle.
         */
        drawRectangle(options, canvasTranslate) {
            const canvasCtx = this.ctx;
            const cornerRadius = options.rx;
            this.ctx.save();
            this.ctx.beginPath();
            if (canvasTranslate) {
                this.ctx.translate(canvasTranslate[0], canvasTranslate[1]);
            }
            this.ctx.globalAlpha = this.getOptionValue(options, 'opacity');
            this.setAttributes(options);
            this.ctx.rect(options.x, options.y, options.width, options.height);
            if (cornerRadius !== null && cornerRadius >= 0) {
                this.drawCornerRadius(options);
            }
            else {
                if (options.fill === 'none') {
                    options.fill = 'transparent';
                }
                this.ctx.fillStyle = options.fill;
                this.ctx.fillRect(options.x, options.y, options.width, options.height);
                this.ctx.stroke();
            }
            this.ctx.restore();
            this.ctx = canvasCtx;
            return (this.canvasObj);
        }

        // To draw the corner of a rectangle
        drawCornerRadius(options) {
            let cornerRadius = options.rx;
            var xPosition = options.x;
            var yPosition = options.y;
            var width = options.width;
            var height = options.height;
            if (options.fill === 'none') {
                options.fill = 'transparent';
            }
            this.ctx.fillStyle = options.fill;
            if (width < 2 * cornerRadius) {
                cornerRadius = width / 2;
            }
            if (height < 2 * cornerRadius) {
                cornerRadius = height / 2;
            }
            this.ctx.beginPath();
            this.ctx.moveTo(xPosition + width - cornerRadius, yPosition);
            this.ctx.arcTo(xPosition + width, yPosition, xPosition + width, yPosition + height, cornerRadius);
            this.ctx.arcTo(xPosition + width, yPosition + height, xPosition, yPosition + height, cornerRadius);
            this.ctx.arcTo(xPosition, yPosition + height, xPosition, yPosition, cornerRadius);
            this.ctx.arcTo(xPosition, yPosition, xPosition + width, yPosition, cornerRadius);
            this.ctx.closePath();
            this.ctx.fill();
            this.ctx.stroke();
        }

        /**
         * To draw a path on the canvas
         *
         * @param {PathAttributes} options - options needed to draw path.
         * @param {Int32Array} canvasTranslate - Array of numbers to translate the canvas.
         * @returns {Element} To draw a path.
         */
        drawPath(options, canvasTranslate) {
            const path = options.d;
            const dataSplit = path.split(' ');
            const borderWidth = this.getOptionValue(options, 'stroke-width');
            const canvasCtx = this.ctx;
            let flag = true;
            this.ctx.save();
            this.ctx.beginPath();
            if (canvasTranslate) {
                this.ctx.translate(canvasTranslate[0], canvasTranslate[1]);
            }
            this.ctx.globalAlpha = options.opacity ? options.opacity : this.getOptionValue(options, 'fill-opacity');
            this.setAttributes(options);
            for (let i = 0; i < dataSplit.length; i = i + 3) {
                const xValue = parseFloat(dataSplit[i + 1]);
                const yValue = parseFloat(dataSplit[i + 2]);
                switch (dataSplit[i]) {
                    case 'M':
                        if (!options.innerR && !options.cx) {
                            this.ctx.moveTo(xValue, yValue);
                        }
                        break;
                    case 'L':
                        if (!options.innerR) {
                            this.ctx.lineTo(xValue, yValue);
                        }
                        break;
                    case 'Q':
                        var controlX = parseFloat(dataSplit[i + 3]);
                        var controlY = parseFloat(dataSplit[i + 4]);
                        this.ctx.quadraticCurveTo(xValue, yValue, controlX, controlY);
                        i = i + 2;
                        break;
                    case 'C':
                        var controlX1 = parseFloat(dataSplit[i + 3]);
                        var controlY1 = parseFloat(dataSplit[i + 4]);
                        var controlX2 = parseFloat(dataSplit[i + 5]);
                        var controlY2 = parseFloat(dataSplit[i + 6]);
                        this.ctx.bezierCurveTo(xValue, yValue, controlX1, controlY1, controlX2, controlY2);
                        i = i + 4;
                        break;
                    case 'A':
                        if (!options.innerR) {
                            if (options.cx) {
                                this.ctx.arc(options.cx, options.cy, options.radius, 0, 2 * Math.PI, options.counterClockWise);
                            }
                            else {
                                this.ctx.moveTo(options.x, options.y);
                                this.ctx.arc(options.x, options.y, options.radius, options.start, options.end, options.counterClockWise);
                                this.ctx.lineTo(options.x, options.y);
                            }
                        }
                        else if (flag) {
                            this.ctx.arc(options.x, options.y, options.radius, options.start, options.end, options.counterClockWise);
                            this.ctx.arc(options.x, options.y, options.innerR, options.end, options.start, !options.counterClockWise);
                            flag = false;
                        }
                        i = i + 5;
                        break;
                    case 'z':
                    case 'Z':
                        this.ctx.closePath();
                        //since for loop is incremented by 3, to get next value after 'z' i is decremented for 2.
                        i = i - 2;
                        break;
                }
            }
            if (options.fill !== 'none' && options.fill !== undefined) {
                this.ctx.fillStyle = options.fill;
                this.ctx.fill();
            }
            if (borderWidth > 0) {
                this.ctx.stroke();
            }
            this.ctx.restore();
            this.ctx = canvasCtx;
            return this.canvasObj;
        }

        /**
         * To draw a text
         *
         * @param {TextAttributes} options - options required to draw text
         * @param {string} label - Specifies the text which has to be drawn on the canvas
         * @param {number} transX - Specifies the text of translate X
         * @param {number} transY - Specifies the text of translate Y
         * @param {number} dy - Specifies the text of translate dy
         * @param {boolean} isTSpan - Specifies the boolean value of span value
         * @returns {void}
         */
        createText(options, label, transX, transY, dy, isTSpan) {
            var fontWeight = this.getOptionValue(options, 'font-weight');
            if (!sfBlazorToolkit.base.isNullOrUndefined(fontWeight) && fontWeight.toLowerCase() === 'regular') {
                fontWeight = 'normal';
            }
            var fontSize = this.getOptionValue(options, 'font-size');
            var fontFamily = this.getOptionValue(options, 'font-family');
            var fontStyle = this.getOptionValue(options, 'font-style').toLowerCase();
            var font = (fontStyle + ' ' + fontWeight + ' ' + fontSize + ' ' + fontFamily);
            var anchor = this.getOptionValue(options, 'text-anchor');
            var opacity = options.opacity !== undefined ? options.opacity : 1;
            if (anchor === 'middle') {
                anchor = 'center';
            }
            this.ctx.save();
            this.ctx.fillStyle = options.fill;
            this.ctx.font = font;
            this.ctx.textAlign = anchor;
            this.ctx.globalAlpha = opacity;
            if (options.baseline) {
                this.ctx.textBaseline = options.baseline;
            }
            if (!isTSpan) {
                var txtlngth = 0;
                this.ctx.translate(options.x + (txtlngth / 2) + (transX ? transX : 0), options.y + (transY ? transY : 0));
                this.ctx.rotate(options.labelRotation * Math.PI / 180);
            }
            this.ctx.fillText(label, isTSpan ? options.x : 0, isTSpan ? dy : 0);
            this.ctx.restore();
            return this.canvasObj;
        }

        /**
         * To draw circle on the canvas
         *
         * @param {CircleAttributes} options - required options to draw the circle
         * @param {Int32Array} canvasTranslate Translate value of canvas
         * @returns {void}
         */
        drawCircle(options, canvasTranslate) {
            var canvasCtx = this.ctx;
            this.ctx.save();
            this.ctx.beginPath();
            this.ctx.arc(options.cx, options.cy, options.r, 0, 2 * Math.PI);
            this.ctx.fillStyle = options.fill;
            this.ctx.globalAlpha = options.opacity;
            this.ctx.fill();
            if (canvasTranslate) {
                this.ctx.translate(canvasTranslate[0], canvasTranslate[1]);
            }
            this.setAttributes(options);
            this.ctx.stroke();
            this.ctx.restore();
            this.ctx = canvasCtx;
            return this.canvasObj;
        }

        /**
         * To draw polyline
         *
         * @param {PolylineAttributes} options - options needed to draw polyline
         * @returns {void}
         */
        drawPolyline(options) {
            this.ctx.save();
            this.ctx.beginPath();
            var points = options.points.split(' ');
            for (var i = 0; i < points.length - 1; i++) {
                var point = points[i].split(',');
                var xValue = parseFloat(point[0]);
                var yValue = parseFloat(point[1]);
                if (i === 0) {
                    this.ctx.moveTo(xValue, yValue);
                }
                else {
                    this.ctx.lineTo(xValue, yValue);
                }
            }
            this.ctx.lineWidth = this.getOptionValue(options, 'stroke-width');
            this.ctx.strokeStyle = options.stroke;
            this.ctx.stroke();
            this.ctx.restore();
        }

        /**
         * To draw an ellipse on the canvas
         *
         * @param {EllipseAttributes} options - options needed to draw ellipse
         * @param {Int32Array} canvasTranslate Translate value of canvas
         * @returns {void}
         */
        drawEllipse(options, canvasTranslate) {
            var canvasCtx = this.ctx;
            var circumference = Math.max(options.rx, options.ry);
            var scaleX = options.rx / circumference;
            var scaleY = options.ry / circumference;
            this.ctx.save();
            this.ctx.beginPath();
            this.ctx.translate(options.cx, options.cy);
            if (canvasTranslate) {
                this.ctx.translate(canvasTranslate[0], canvasTranslate[1]);
            }
            this.ctx.save();
            this.ctx.scale(scaleX, scaleY);
            this.ctx.arc(0, 0, circumference, 0, 2 * Math.PI, false);
            this.ctx.fillStyle = options.fill;
            this.ctx.fill();
            this.ctx.restore();
            this.ctx.lineWidth = this.getOptionValue(options, 'stroke-width');
            this.ctx.strokeStyle = options.stroke;
            this.ctx.stroke();
            this.ctx.restore();
            this.ctx = canvasCtx;
        }

        /**
         * To draw an image
         *
         * @param {ImageAttributes} options - options required to draw an image on the canvas
         * @returns {void}
         */
        drawImage(options) {
            this.ctx.save();
            var imageObj = new Image();
            if (!sfBlazorToolkit.base.isNullOrUndefined(options.href)) {
                imageObj.src = options.href;
                this.ctx.drawImage(imageObj, options.x, options.y, options.width, options.height);
            }
            this.ctx.restore();
        }

        /**
         * To create a linear gradient
         *
         * @param {string[]} colors - Specifies the colors required to create linear gradient
         * @returns {string} It returns color
         */
        createLinearGradient(colors) {
            var myGradient;
            if (!sfBlazorToolkit.base.isNullOrUndefined(colors[0].colorStop)) {
                myGradient = this.ctx.createLinearGradient(0, 0, 0, this.canvasObj.height);
            }
            var color = this.setGradientValues(colors, myGradient);
            return color;
        }

        /**
         * To create a radial gradient
         *
         * @param {string[]} colors - Specifies the colors required to create linear gradient
         * @returns {string} It returns gradient color
         */
        createRadialGradient(colors) {
            var myGradient;
            if (!sfBlazorToolkit.base.isNullOrUndefined(colors[0].colorStop)) {
                myGradient = this.ctx.createRadialGradient(0, 0, 0, 0, 0, this.canvasObj.height);
            }
            var colorName = this.setGradientValues(colors, myGradient);
            return colorName;
        }

        // To set the gradient values
        setGradientValues(colors, myGradient) {
            var colorName;
            if (!sfBlazorToolkit.base.isNullOrUndefined(colors[0].colorStop)) {
                for (var i = 0; i <= colors.length - 1; i++) {
                    var color = colors[i].color;
                    var newColorStop = (colors[i].colorStop).slice(0, -1);
                    var stopColor = parseInt(newColorStop, 10) / 100;
                    myGradient.addColorStop(stopColor, color);
                }
                colorName = myGradient.toString();
            }
            else {
                colorName = colors[0].color.toString();
            }
            return colorName;
        }

        /**
         * To set the attributes to the element
         *
         * @param {SVGCanvasAttributes} options - Attributes to set for the element
         * @param {HTMLElement} element - The element to which the attributes need to be set
         * @returns {HTMLElement} It returns null value
         */
        setElementAttributes(options, element) {
            var keys = Object.keys(options);
            var values = Object.keys(options).map(function (key) { return options[key]; });
            for (var i = 0; i < keys.length; i++) {
                element.setAttribute(keys[i], values[i]);
            }
            return null;
        }

        /**
         * To update the values of the canvas element attributes
         *
         * @param {SVGCanvasAttributes} options - Specifies the colors required to create gradient
         * @returns {void}
         */
        updateCanvasAttributes(options) {
            this.setElementAttributes(options, this.canvasObj);
            var context = this.ctx;
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.dataUrl)) {
                var image = new Image;
                image.onload = function () {
                    context.drawImage(image, 0, 0);
                };
                image.src = this.dataUrl;
            }
        }

        /**
         * This method clears the given rectangle region
         *
         * @param {Rect} rect The rect parameter as passed
         */
        clearRect(rect) {
            this.ctx.restore();
            this.ctx.clearRect(rect.x, rect.y, rect.width, rect.height);
        }

        /**
         * For canvas rendering in chart
         * Dummy method for using canvas/svg render in the same variable name in chart control
         */
        createGroup() {
            return null;
        }

        /**
         * To render a clip path
         *
         * Dummy method for using canvas/svg render in the same variable name in chart control
         */
        drawClipPath() {
            return null;
        }

        /**
         * To render a Circular clip path
         *
         * Dummy method for using canvas/svg render in the same variable name in chart control
         */
        drawCircularClipPath() {
            return null;
        }

        /**
         * Clip method to perform clip in canvas mode
         *
         * @param {BaseAttibutes} options The canvas clip of options
         */
        canvasClip(options) {
            this.ctx.save();
            this.ctx.fillStyle = 'transparent';
            this.ctx.rect(options.x, options.y, options.width, options.height);
            this.ctx.fill();
            this.ctx.clip();
        }

        /**
         * Tp restore the canvas
         */
        canvasRestore() {
            this.ctx.restore();
        }

        /**
         * To draw a polygon
         * Dummy method for using canvas/svg render in the same variable name in chart control
         */
        drawPolygon() {
            return null;
        }

        /**
         * To create defs element in SVG
         * Dummy method for using canvas/svg render in the same variable name in chart control
         *
         * @returns {Element} It returns null
         */
        createDefs() {
            return null;
        }

        /**
         * To create clip path in SVG
         * Dummy method for using canvas/svg render in the same variable name in chart control
         */
        createClipPath() {
            return null;
        }

        /**
         * To create a Html5 SVG element
         * Dummy method for using canvas/svg render in the same variable name in chart control
         *
         * @returns {Element} It returns null
         */
        createSvg() {
            return null;
        }
    }

    /** @private */
    function getTooltipThemeColor(theme) {
        var style;
        switch (theme) {            
            case 'Fluent':
                style = {
                    tooltipFill: '#FFFFFF',
                    tooltipBoldLabel: '#242424',
                    tooltipLightLabel: '#242424',
                    tooltipHeaderLine: '#D2D0CE',
                    textStyle: { fontFamily: 'Segoe UI', color: '#242424', fontWeight: null, size: '12px', headerTextSize: '12px', boldTextSize: '12px' }
                };
                break;
            case 'FluentDark':
                style = {
                    tooltipFill: '#292929',
                    tooltipBoldLabel: '#FFFFFF',
                    tooltipLightLabel: '#FFFFFF',
                    tooltipHeaderLine: '#3B3A39',
                    textStyle: { fontFamily: 'Segoe UI', color: '#FFFFFF', fontWeight: null, size: '12px', headerTextSize: '12px', boldTextSize: '12px' }
                };
                break;
            default:
                style = {
                    tooltipFill: '#212529',
                    tooltipBoldLabel: '#F9FAFB',
                    tooltipLightLabel: '#F9FAFB',
                    tooltipHeaderLine: '#ffffff',
                    textStyle: { fontFamily: 'Helvetica', color: '#F9FAFB', fontWeight: null, size: '12px', headerTextSize: '12px', boldTextSize: '12px' }
                };
                break;
        }
        return style;
    }

    /**
     * Function to measure the height and width of the text.
     *
     * @private
     * @param {string} text To get a text
     * @param {FontModel} font To get a font of the text
     * @returns {Size} measureText
     */
    function measureText(text, font, themeFontStyle, isHeader) {
        var breakText = text || ''; // For avoid NuLL value
        var htmlObject = document.getElementById('chartmeasuretext');
        if (htmlObject === null) {
            htmlObject = sfBlazorToolkit.base.createElement('text', { id: 'chartmeasuretext' });
            document.body.appendChild(htmlObject);
        }
        if (typeof (text) === 'string' && (text.indexOf('<') > -1 || text.indexOf('>') > -1)) {
            var textArray = text.split(' ');
            for (var i = 0; i < textArray.length; i++) {
                if (textArray[i].indexOf('<br/>') === -1) {
                    textArray[i] = textArray[i].replace(/[<>]/g, '&');
                }
            }
            text = textArray.join(' ');
        }
        htmlObject.innerHTML = (breakText.indexOf('<br>') > -1 || breakText.indexOf('<br/>') > -1) ? breakText : text;
        htmlObject.style.position = 'fixed';
        htmlObject.style.fontSize = font.size || (isHeader ? themeFontStyle.headerTextSize : themeFontStyle.size);
        htmlObject.style.fontStyle = font.fontStyle || themeFontStyle.fontStyle;
        htmlObject.style.fontFamily = font.fontFamily || themeFontStyle.fontFamily;
        htmlObject.style.visibility = 'hidden';
        htmlObject.style.top = '-100';
        htmlObject.style.left = '0';
        htmlObject.style.whiteSpace = 'nowrap';
        // For bootstrap line height issue
        htmlObject.style.lineHeight = 'normal';
        var fontWidth = htmlObject.clientWidth;
        var fontHeight = htmlObject.clientHeight;
        var fontWeight = htmlObject.style.fontWeight;
        htmlObject.style.fontWeight = font.fontWeight || themeFontStyle.fontWeight;
        return new Size(htmlObject.style.fontWeight === 'bold' && fontWeight === 'normal' ? Math.max(fontWidth, htmlObject.clientWidth) : htmlObject.clientWidth, htmlObject.style.fontWeight === 'bold' && fontWeight === 'normal' ? Math.max(fontHeight, htmlObject.clientHeight) : htmlObject.clientHeight);
    }

    /** @private */
    function withInAreaBounds(x, y, areaBounds, width, height) {
        if (width === void 0) { width = 0; }
        if (height === void 0) { height = 0; }
        return (x >= areaBounds.x - width && x <= areaBounds.x + areaBounds.width + width && y >= areaBounds.y - height
            && y <= areaBounds.y + areaBounds.height + height);
    }

    /** @private */
    function findDirection(rX, rY, rect, arrowLocation, arrowPadding, top, bottom, left, tipX, tipY, controlName) {
        if (controlName === void 0) { controlName = ''; }
        var direction = '';
        var startX = rect.x;
        var startY = rect.y;
        var width = rect.x + rect.width;
        var height = rect.y + rect.height;
        if (top) {
            direction = direction.concat('M' + ' ' + (startX) + ' ' + (startY + rY) + ' Q ' + startX + ' '
                + startY + ' ' + (startX + rX) + ' ' + startY + ' ' +
                ' L' + ' ' + (width - rX) + ' ' + (startY) + ' Q ' + width + ' '
                + startY + ' ' + (width) + ' ' + (startY + rY));
            direction = direction.concat(' L' + ' ' + (width) + ' ' + (height - rY) + ' Q ' + width + ' '
                + (height) + ' ' + (width - rX) + ' ' + (height));
            if (arrowPadding !== 0) {
                if (controlName === 'RangeNavigator') {
                    if ((arrowLocation.x - arrowPadding) > width / 2) {
                        direction = direction.concat(' L' + ' ' + (arrowLocation.x + arrowPadding) + ' ' + (height));
                        direction = direction.concat(' L' + ' ' + (tipX + arrowPadding) + ' ' + (height + arrowPadding)
                            + ' L' + ' ' + (arrowLocation.x) + ' ' + height);
                    }
                    else {
                        direction = direction.concat(' L' + ' ' + (arrowLocation.x) + ' ' + (height));
                        direction = direction.concat(' L' + ' ' + (tipX - arrowPadding) + ' ' + (height + arrowPadding)
                            + ' L' + ' ' + (arrowLocation.x - arrowPadding) + ' ' + height);
                    }
                }
                else {
                    direction = direction.concat(' L' + ' ' + (arrowLocation.x + arrowPadding) + ' ' + (height));
                    direction = direction.concat(' L' + ' ' + (tipX) + ' ' + (height + arrowPadding)
                        + ' L' + ' ' + (arrowLocation.x - arrowPadding) + ' ' + height);
                }
            }
            if ((arrowLocation.x - arrowPadding) > startX) {
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
                + (startY) + ' ' + (startX + rX) + ' ' + (startY) + ' L' + ' ' + (arrowLocation.x - arrowPadding) + ' ' + (startY));
            direction = direction.concat(' L' + ' ' + (tipX) + ' ' + (arrowLocation.y));
            direction = direction.concat(' L' + ' ' + (arrowLocation.x + arrowPadding) + ' ' + (startY));
            direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (startY)
                + ' Q ' + (width) + ' ' + (startY) + ' ' + (width) + ' ' + (startY + rY));
            direction = direction.concat(' L' + ' ' + (width) + ' ' + (height - rY) + ' Q ' + (width) + ' '
                + (height) + ' ' + (width - rX) + ' ' + (height) +
                ' L' + ' ' + (startX + rX) + ' ' + (height) + ' Q ' + (startX) + ' '
                + (height) + ' ' + (startX) + ' ' + (height - rY) + ' z');
        }
        else if (left) {
            direction = direction.concat('M' + ' ' + (startX) + ' ' + (startY + rY) + ' Q ' + startX + ' '
                + (startY) + ' ' + (startX + rX) + ' ' + (startY));
            direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (startY) + ' Q ' + (width) + ' '
                + (startY) + ' ' + (width) + ' ' + ((controlName === 'RangeNavigator' ? 0 : (startY + rY)) + ' L' + ' ' + (width) + ' ' + (controlName === 'RangeNavigator' ? 0 : (arrowLocation.y - arrowPadding))));
            direction = (controlName === 'RangeNavigator') ? direction.concat(' L' + ' ' + (width + arrowPadding) + ' ' + 0) :
                direction.concat(' L' + ' ' + (width + arrowPadding) + ' ' + (tipY));
            direction = (controlName === 'RangeNavigator') ? direction.concat(' L' + ' ' + (width) + ' ' + (arrowLocation.y - rY)) :
                direction.concat(' L' + ' ' + (width) + ' ' + (arrowLocation.y + arrowPadding));
            direction = direction.concat(' L' + ' ' + (width) + ' ' + (height - rY) + ' Q ' + width + ' ' + (height) + ' ' + (width - rX) + ' ' + (height));
            direction = direction.concat(' L' + ' ' + (startX + rX) + ' ' + (height) + ' Q ' + startX + ' '
                + (height) + ' ' + (startX) + ' ' + (height - rY) + ' z');
        }
        else {
            direction = direction.concat('M' + ' ' + (startX + rX) + ' ' + (startY) + ' Q ' + (startX) + ' '
                + (startY) + ' ' + (startX) + ' ' + ((controlName === 'RangeNavigator' ? 0 : (startY + rY)) + ' L' + ' ' + (startX) + ' ' + (controlName === 'RangeNavigator' ? 0 : (arrowLocation.y - arrowPadding))));
            direction = (controlName === 'RangeNavigator') ? direction.concat(' L' + ' ' + (startX - arrowPadding) + ' ' + 0) :
                direction.concat(' L' + ' ' + (startX - arrowPadding) + ' ' + (tipY));
            direction = (controlName === 'RangeNavigator') ? direction.concat(' L' + ' ' + (startX) + ' ' + (arrowLocation.y - rY)) :
                direction.concat(' L' + ' ' + (startX) + ' ' + (arrowLocation.y + arrowPadding));
            direction = direction.concat(' L' + ' ' + (startX) + ' ' + (height - rY) + ' Q ' + startX + ' '
                + (height) + ' ' + (startX + rX) + ' ' + (height));
            direction = direction.concat(' L' + ' ' + (width - rX) + ' ' + (height) + ' Q ' + width + ' '
                + (height) + ' ' + (width) + ' ' + (height - rY) +
                ' L' + ' ' + (width) + ' ' + (startY + rY) + ' Q ' + width + ' '
                + (startY) + ' ' + (width - rX) + ' ' + (startY) + ' z');
        }
        return direction;
    }

    /** @private */
    class Size {
        constructor(width, height) {
            this.width = width;
            this.height = height;
        }
    }


    /** @private */
    class Rect {
        constructor(x, y, width, height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }

    class Side {
        constructor(bottom, right) {
            this.isRight = right;
            this.isBottom = bottom;
        }
    }

    /** @private */
    class CustomizeOption {
        constructor(id) {
            this.id = id;
        }
    }

    /** @private */
    class TextOption extends CustomizeOption {        
        constructor(id, x, y, anchor, text, transform = '', baseLine = 'auto', labelRotation = 0) {
            super(id);
            this.x = x;
            this.y = y;
            this.anchor = anchor;
            this.text = text;
            this.transform = transform;
            this.baseLine = baseLine;
            this.labelRotation = labelRotation;
        }
    }

    /** @private */
    function getElement(id) {
        return document.getElementById(id);
    }

    /** @private */
    function removeElement(id) {
        var element = getElement(id);
        if (element) {
            sfBlazorToolkit.base.remove(element);
        }
    }

    /** @private */
    function drawSymbol(location, shape, size, url, options, role, label) {
        var renderer = new SvgRenderer('');
        var temp = calculateShapes(location, size, shape, options, url);
        var htmlObject = renderer['draw' + temp.functionName](temp.renderOption);
        htmlObject.setAttribute('role', role);
        htmlObject.setAttribute('aria-label', label);
        return htmlObject;
    }

    /** @private */
    function calculateShapes(location, size, shape, options, url) {
        let path;
        let functionName = 'Path';
        const width = size.width;
        const height = size.height;
        const centerX = location.x;
        const centerY = location.y;
        const startX = location.x + (-width / 2);
        const startY = location.y + (-height / 2);
        switch (shape) {
            case 'Circle':
            case 'Bubble':
                functionName = 'Ellipse';
                sfBlazorToolkit.base.merge(options, { 'rx': width / 2, 'ry': height / 2, 'cx': centerX, 'cy': centerY });
                break;
            case 'Plus':
                path = 'M' + ' ' + startX + ' ' + centerY + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + centerY + ' ' +
                    'M' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' + 'L' + ' ' + centerX + ' ' +
                    (centerY + (-height / 2));
                sfBlazorToolkit.base.merge(options, { 'd': path, stroke: options.fill });
                break;
            case 'Cross':
                path = 'M' + ' ' + startX + ' ' + (centerY + (-height / 2)) + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                    'M' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' +
                    (centerY + (-height / 2));
                sfBlazorToolkit.base.merge(options, { 'd': path, stroke: options.fill });
                break;
            case 'HorizontalLine':
                path = 'M' + ' ' + startX + ' ' + centerY + ' ' + 'L' + ' ' + (centerX + (width / 2)) + ' ' + centerY;
                sfBlazorToolkit.base.merge(options, { 'd': path, stroke: options.fill });
                break;
            case 'VerticalLine':
                path = 'M' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' + 'L' + ' ' + centerX + ' ' + (centerY + (-height / 2));
                sfBlazorToolkit.base.merge(options, { 'd': path, stroke: options.fill });
                break;
            case 'Diamond':
                path = 'M' + ' ' + startX + ' ' + centerY + ' ' +
                    'L' + ' ' + centerX + ' ' + (centerY + (-height / 2)) + ' ' +
                    'L' + ' ' + (centerX + (width / 2)) + ' ' + centerY + ' ' +
                    'L' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' +
                    'L' + ' ' + startX + ' ' + centerY + ' z';
                sfBlazorToolkit.base.merge(options, { 'd': path });
                break;
            case 'Rectangle':
                path = 'M' + ' ' + startX + ' ' + (centerY + (-height / 2)) + ' ' +
                    'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (-height / 2)) + ' ' +
                    'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                    'L' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' ' +
                    'L' + ' ' + startX + ' ' + (centerY + (-height / 2)) + ' z';
                sfBlazorToolkit.base.merge(options, { 'd': path });
                break;
            case 'Triangle':
                path = 'M' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' ' +
                    'L' + ' ' + centerX + ' ' + (centerY + (-height / 2)) + ' ' +
                    'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY + (height / 2)) + ' ' +
                    'L' + ' ' + startX + ' ' + (centerY + (height / 2)) + ' z';
                sfBlazorToolkit.base.merge(options, { 'd': path });
                break;
            case 'InvertedTriangle':
                path = 'M' + ' ' + (centerX + (width / 2)) + ' ' + (centerY - (height / 2)) + ' ' +
                    'L' + ' ' + centerX + ' ' + (centerY + (height / 2)) + ' ' +
                    'L' + ' ' + (centerX - (width / 2)) + ' ' + (centerY - (height / 2)) + ' ' +
                    'L' + ' ' + (centerX + (width / 2)) + ' ' + (centerY - (height / 2)) + ' z';
                sfBlazorToolkit.base.merge(options, { 'd': path });
                break;
            case 'Pentagon':
                var eq = 72;
                var xValue = void 0;
                var yValue = void 0;
                for (var i = 0; i <= 5; i++) {
                    xValue = (width / 2) * Math.cos((Math.PI / 180) * (i * eq));
                    yValue = (height / 2) * Math.sin((Math.PI / 180) * (i * eq));
                    if (i === 0) {
                        path = 'M' + ' ' + (centerX + xValue) + ' ' + (centerY + yValue) + ' ';
                    }
                    else {
                        path = path.concat('L' + ' ' + (centerX + xValue) + ' ' + (centerY + yValue) + ' ');
                    }
                }
                path = path.concat('Z');
                sfBlazorToolkit.base.merge(options, { 'd': path });
                break;
            case 'Image':
                functionName = 'Image';
                sfBlazorToolkit.base.merge(options, { 'href': url, 'height': height, 'width': width, x: startX, y: startY });
                break;
            case 'Star': {
                var cornerPoints = 5;
                var outerRadius = Math.min(width, height) / 2;
                var innerRadius = outerRadius / 2;
                var angle = Math.PI / cornerPoints;
                var starPath = '';
                for (var i = 0; i < 2 * cornerPoints; i++) {
                    var radius = (i % 2 === 0) ? outerRadius : innerRadius;
                    var currentX = centerX + radius * Math.cos(i * angle - Math.PI / 2);
                    var currentY = centerY + radius * Math.sin(i * angle - Math.PI / 2);
                    starPath += (i === 0 ? 'M' : 'L') + currentX + ',' + currentY;
                }
                starPath += 'Z';
                sfBlazorToolkit.base.merge(options, { 'd': starPath });
                break;
            }
        }
        return { renderOption: options, functionName: functionName };
    }

    /** @private */
    class PathOption extends CustomizeOption {
        constructor(id, fill, width, color, opacity, dashArray, d) {
            super(id);
            this.opacity = opacity;
            this.fill = fill;
            this.stroke = color;
            this['stroke-width'] = width;
            this['stroke-dasharray'] = dashArray;
            this.d = d;
        }
    }

    /** @private */
    function textElement(options, font, color, parent, themeStyle) {
        var renderOptions = {};
        var renderer = new SvgRenderer('');
        renderOptions = {
            'id': options.id,
            'x': options.x,
            'y': options.y,
            'fill': color,
            'font-size': font.size || themeStyle.size,
            'font-style': font.fontStyle || themeStyle.fontStyle,
            'font-family': font.fontFamily || themeStyle.fontFamily,
            'font-weight': font.fontWeight || themeStyle.fontWeight,
            'text-anchor': options.anchor,
            'transform': options.transform,
            'opacity': font.opacity,
            'dominant-baseline': options.baseLine
        };
        var text = typeof options.text === 'string' ? options.text : options.text[0];
        var htmlObject = renderer.createText(renderOptions, text);
        if (parent) {
            parent.appendChild(htmlObject);
        }
        return htmlObject;
    }

    class TooltipLocation {
        constructor(x, y) {
            this.x = x;
            this.y = y;
        }
    }

    /**
     * Configures the fonts in charts.
     *
     * @private
     */    
    class TextStyle extends window.sfBlazorToolkit.base.ChildProperty {
      constructor(...args) {
        super(...args);
      }
    }

    /**
     * Configures the borders in the chart.
     *
     * @private
     */    
    class TooltipBorder extends window.sfBlazorToolkit.base.ChildProperty {
      constructor(...args) {
        super(...args);
      }
    }

    /**
     * Configures the borders in the chart.
     *
     * @private
     */    
    class AreaBounds extends window.sfBlazorToolkit.base.ChildProperty {
      constructor(...args) {
        super(...args);
      }
    }

    /**
     * Configures the borders in the chart.
     *
     * @private
     */    
    class ToolLocation extends window.sfBlazorToolkit.base.ChildProperty {
      constructor(...args) {
        super(...args);
      }
    }

    sfBlazorToolkit.base.Property(null)(TextStyle.prototype, 'size');
    sfBlazorToolkit.base.Property('')(TextStyle.prototype, 'color');
    sfBlazorToolkit.base.Property('Segoe UI')(TextStyle.prototype, 'fontFamily');
    sfBlazorToolkit.base.Property('Normal')(TextStyle.prototype, 'fontWeight');
    sfBlazorToolkit.base.Property('Normal')(TextStyle.prototype, 'fontStyle');
    sfBlazorToolkit.base.Property(1)(TextStyle.prototype, 'opacity');
    sfBlazorToolkit.base.Property(null)(TextStyle.prototype, 'headerTextSize');
    sfBlazorToolkit.base.Property(null)(TextStyle.prototype, 'boldTextSize');
    sfBlazorToolkit.base.Property('')(TooltipBorder.prototype, 'color');
    sfBlazorToolkit.base.Property(1)(TooltipBorder.prototype, 'width');
    sfBlazorToolkit.base.Property('')(TooltipBorder.prototype, 'dashArray');
    sfBlazorToolkit.base.Property(0)(AreaBounds.prototype, 'x');
    sfBlazorToolkit.base.Property(0)(AreaBounds.prototype, 'y');
    sfBlazorToolkit.base.Property(0)(AreaBounds.prototype, 'width');
    sfBlazorToolkit.base.Property(0)(AreaBounds.prototype, 'height');
    sfBlazorToolkit.base.Property(0)(ToolLocation.prototype, 'x');
    sfBlazorToolkit.base.Property(0)(ToolLocation.prototype, 'y');

    /**
     * Represents the Tooltip control.
     * ```html
     * <div id="tooltip"/>
     * <script>
     *   var tooltipObj = new Tooltip({ isResponsive : true });
     *   tooltipObj.appendTo("#tooltip");
     * </script>
     * ```
     *
     * @private
     */
    class Tooltip extends sfBlazorToolkit.base.Component {
        /**
         * Constructor for creating the widget
         *
         * @hidden
         */
        constructor(options, element) {
            super(options, element);
        }

        /**
         * Initialize the event handler.
         *
         * @private
         */
        preRender() {
            this.allowServerDataBinding = false;
            this.initPrivateVariable();
            if (!this.isCanvas) {
                this.removeSVG();
            }
            this.createTooltipElement();
        }
        initPrivateVariable() {
            this.renderer = new SvgRenderer(this.element.id);
            this.themeStyle = getTooltipThemeColor(this.theme);
            this.formattedText = [];
            this.padding = 5;
            this.highlightPadding = 3;
            this.areaMargin = 10;
            this.isFirst = true;
            this.markerPoint = [];
        }
        removeSVG() {
            const svgObject = document.getElementById(this.element.id + '_svg');
            const templateObject = document.getElementById(this.element.id + 'parent_template');
            if (this.blazorTemplate) {
                sfBlazorToolkit.base.resetBlazorTemplate(this.element.id + 'parent_template' + '_blazorTemplate');
            }
            if (svgObject && svgObject.parentNode) {
                sfBlazorToolkit.base.remove(svgObject);
            }
            if (templateObject && templateObject.parentNode) {
                sfBlazorToolkit.base.remove(templateObject);
            }
        }
        /**
         * To Initialize the control rendering.
         */
        render() {
            this.fadeOuted = false;
            if (!this.template) {
                if (this.split) {
                    this.renderSplitTooltip();
                }
                else {
                    this.renderText(this.isFirst);
                    var argsData = {
                        cancel: false, name: 'tooltipRender', tooltip: this
                    };
                    this.trigger('tooltipRender', argsData);
                    var markerSide = this.renderTooltipElement(this.areaBounds, this.location, 0);
                    this.drawMarker(markerSide.isBottom, markerSide.isRight, this.markerSize, 0);
                }
            }
            else {
                if (this.split) {
                    for (var splitIndex = 0; splitIndex < this.data.length; splitIndex++) {
                        this.updateTemplateFn();
                        this.createTemplate(this.areaBounds, this.location, splitIndex);
                    }
                }
                else {
                    this.updateTemplateFn();
                    this.createTemplate(this.areaBounds, this.location);
                }
            }
            this.trigger('loaded', { tooltip: this });
            var element = document.getElementById('chartmeasuretext');
            if (element) {
                sfBlazorToolkit.base.remove(element);
            }
            this.allowServerDataBinding = true;
        }

        /**
         * Handles rendering when split tooltip is enabled (creates/removes per-split groups/paths,
         * renders text for each split item, computes auto positions and renders each split tooltip).
         * @private
         */
        renderSplitTooltip() {
            var svgRoot = getElement(this.element.id + '_svg');
            var prevCount = (this.previousContent && this.previousContent.length) ? this.previousContent.length : 0;
            var currentCount = this.content.length;
            if (svgRoot) {
                if (currentCount > prevCount) {
                    for (var index = prevCount; index < currentCount; index++) {
                        var tooltipGroup = document.getElementById(this.element.id + '_group_' + index);
                        if (!tooltipGroup) {
                            tooltipGroup = this.renderer.createGroup({ id: this.element.id + '_group_' + index });
                            tooltipGroup.setAttribute('transform', 'translate(0,0)');
                            svgRoot.appendChild(tooltipGroup);
                            var tooltipPath = this.renderer.drawPath({
                                'id': this.element.id + '_path_' + index,
                                'stroke-width': ((this.theme === 'Fluent') && !this.border.width) ? 1 : this.border.width,
                                'fill': this.fill || this.themeStyle.tooltipFill,
                                'opacity': ((this.theme.indexOf('Fluent') > -1) && this.opacity === 0.75) ? 1 : this.opacity,
                                'stroke': this.border.color || (this.theme === 'Fluent' ? '#D2D0CE' : this.border.color)
                            });
                            tooltipGroup.appendChild(tooltipPath);
                        }
                    }
                }
                else if (currentCount < prevCount) {
                    for (var index = currentCount; index < prevCount; index++) {
                        removeElement(this.element.id + '_group_' + index);
                        removeElement(this.element.id + '_path_' + index);
                        removeElement(this.element.id + '_trackball_group_' + index);
                        removeElement(this.element.id + '_tooltip_connector_' + index);
                    }
                }
            }
            this.splitTooltipRectCollection = [];
            this.hasHorizontalOverflow = false;
            for (var splitIndex = 0; splitIndex < currentCount; splitIndex++) {
                this.renderText(this.isFirst, splitIndex);
            }
            if (this.split) {
                this.calculateAutoPositions();
            }
            for (var splitIndex = 0; splitIndex < currentCount; splitIndex++) {
                var argsData = {
                    cancel: false, name: 'tooltipRender', tooltip: this
                };
                this.trigger('tooltipRender', argsData);
                var markerSide = this.renderTooltipElement(this.areaBounds, this.location, splitIndex);
                this.drawMarker(markerSide.isBottom, markerSide.isRight, this.markerSize, splitIndex);
            }
            this.previousContent = this.content;
        }

        /**
         * Calculates the actual clip rectangle that encompasses all split clip bounds.
         * @returns The combined clip rectangle
         * @private
         */
        getActualSplitClipRect() {
            if (!this.splitClipBounds || this.splitClipBounds.length === 0) {
                return new Rect(0, 0, 0, 0);
            }
            let minY = Infinity;
            let maxY = -Infinity;
            let minX = Infinity;
            let maxX = -Infinity;
            for (let _i = 0, _a = this.splitClipBounds; _i < _a.length; _i++) {
                const clipBound = _a[_i];
                const boundTop = clipBound.y;
                const boundBottom = clipBound.y + clipBound.height;
                const boundLeft = clipBound.x;
                const boundRight = clipBound.x + clipBound.width;
                minY = Math.min(minY, boundTop);
                maxY = Math.max(maxY, boundBottom);
                minX = Math.min(minX, boundLeft);
                maxX = Math.max(maxX, boundRight);
            }
            const actualWidth = maxX - minX;
            const actualHeight = maxY - minY;
            return new Rect(minX, minY, actualWidth, actualHeight);
        }

        /**
         * Calculates adjusted positions for split tooltips to prevent overlap
         * Centers overlapping clusters with median at the gap center
         *
         * @private
         */
        calculateAutoPositions() {
            if (!this.split || this.splitTooltipRectCollection.length < 1) {
                return;
            }
            const TOOLTIP_PADDING = 20;
            const MINIMUM_GAP = 5;
            const OVERFLOW_BUFFER = 5;
            let totalXPosition = 0;
            var areaBoundsRight = this.areaBounds.x + this.areaBounds.width;
            if (!this.inverted) {
                for (var _i = 0, _a = this.splitTooltipRectCollection; _i < _a.length; _i++) {
                    var tooltipRect = _a[_i];
                    var tooltipRightEdge = tooltipRect.x + tooltipRect.width;
                    if (tooltipRightEdge > areaBoundsRight) {
                        this.hasHorizontalOverflow = true;
                    }
                    totalXPosition += tooltipRect.x;
                }
                var averageXPosition = totalXPosition / this.splitTooltipRectCollection.length;
                for (var _b = 0, _c = this.splitTooltipRectCollection; _b < _c.length; _b++) {
                    var tooltipRect = _c[_b];
                    tooltipRect.x = this.hasHorizontalOverflow
                        ? averageXPosition - (tooltipRect.width + 2 * TOOLTIP_PADDING)
                        : averageXPosition;
                }
            }
            else {
                var horizontalItems = this.splitTooltipRectCollection
                    .map(function (rect, originalIndex) {
                        return ({
                            xPosition: rect.x,
                            yPosition: rect.y,
                            width: rect.width,
                            height: rect.height,
                            originalIndex: originalIndex
                        });
                    })
                    .sort(function (leftItem, rightItem) { return leftItem.xPosition - rightItem.xPosition; });
                for (var itemIndex = 1; itemIndex < horizontalItems.length; itemIndex++) {
                    var previousItem = horizontalItems[itemIndex - 1];
                    var currentItem = horizontalItems[itemIndex];
                    var requiredX = previousItem.xPosition + previousItem.width + MINIMUM_GAP + OVERFLOW_BUFFER;
                    if (currentItem.xPosition < requiredX) {
                        currentItem.xPosition = requiredX;
                    }
                }
                for (var _d = 0, horizontalItems_1 = horizontalItems; _d < horizontalItems_1.length; _d++) {
                    var sortedItem = horizontalItems_1[_d];
                    this.splitTooltipRectCollection[sortedItem.originalIndex].x = sortedItem.xPosition;
                }
                return;
            }
            var sortedTooltipItems = this.splitTooltipRectCollection
                .map(function (rect, originalIndex) {
                    return ({
                        xPosition: rect.x,
                        width: rect.width,
                        yPosition: rect.y,
                        height: rect.height,
                        originalIndex: originalIndex
                    });
                })
                .sort(function (firstItem, secondItem) {
                    return firstItem.yPosition - secondItem.yPosition;
                });
            var clusterStartIndex = 0;
            var previousClusterBottomY = -Infinity;
            while (clusterStartIndex < sortedTooltipItems.length) {
                var currentCluster = [sortedTooltipItems[clusterStartIndex]];
                var nextItemIndex = clusterStartIndex + 1;
                while (nextItemIndex < sortedTooltipItems.length) {
                    var previousTooltip = currentCluster[currentCluster.length - 1];
                    var currentTooltip = sortedTooltipItems[nextItemIndex];
                    var previousTooltipBottom = previousTooltip.yPosition + previousTooltip.height;
                    if (currentTooltip.yPosition < previousTooltipBottom + MINIMUM_GAP) {
                        currentCluster.push(currentTooltip);
                        nextItemIndex++;
                    }
                    else {
                        break;
                    }
                }
                if (currentCluster.length > 1) {
                    var totalCenterPoints = 0;
                    var clusterTotalHeight = 0;
                    for (var _e = 0, currentCluster_1 = currentCluster; _e < currentCluster_1.length; _e++) {
                        var tooltipItem = currentCluster_1[_e];
                        var centerY = tooltipItem.yPosition + (tooltipItem.height / 2);
                        totalCenterPoints += centerY;
                        clusterTotalHeight += tooltipItem.height;
                    }
                    var medianCenterY = totalCenterPoints / currentCluster.length;
                    var totalHeightWithGaps = clusterTotalHeight + MINIMUM_GAP * (currentCluster.length - 1);
                    var currentYPosition = Math.max(medianCenterY - (totalHeightWithGaps / 2), previousClusterBottomY + MINIMUM_GAP);
                    for (var _f = 0, currentCluster_2 = currentCluster; _f < currentCluster_2.length; _f++) {
                        var tooltipItem = currentCluster_2[_f];
                        tooltipItem.yPosition = currentYPosition;
                        currentYPosition += tooltipItem.height + MINIMUM_GAP;
                    }
                    previousClusterBottomY = currentYPosition - MINIMUM_GAP;
                }
                else {
                    var singleTooltip = currentCluster[0];
                    var requiredMinimumY = previousClusterBottomY + MINIMUM_GAP;
                    if (singleTooltip.yPosition < requiredMinimumY) {
                        singleTooltip.yPosition = requiredMinimumY;
                    }
                    previousClusterBottomY = singleTooltip.yPosition + singleTooltip.height;
                }
                clusterStartIndex = nextItemIndex;
            }
            var itemCount = sortedTooltipItems.length;
            if (itemCount === 0) {
                return;
            }
            var lastTooltip = sortedTooltipItems[itemCount - 1];
            var lastTooltipBottom = lastTooltip.yPosition + lastTooltip.height;
            var actualClipRect = this.getActualSplitClipRect();
            if (lastTooltipBottom > (actualClipRect.y + actualClipRect.height)) {
                var overflowAmount = lastTooltipBottom - actualClipRect.height + OVERFLOW_BUFFER;
                for (var currentIndex = itemCount - 1; currentIndex >= 0; currentIndex--) {
                    var currentTooltipY = sortedTooltipItems[currentIndex].yPosition;
                    var previousTooltipBottom = currentIndex === 0
                        ? actualClipRect.y
                        : sortedTooltipItems[currentIndex - 1].yPosition + sortedTooltipItems[currentIndex - 1].height;
                    var availableSpace = currentTooltipY - previousTooltipBottom;
                    if (availableSpace > overflowAmount) {
                        for (var shiftIndex = currentIndex; shiftIndex < itemCount; shiftIndex++) {
                            sortedTooltipItems[shiftIndex].yPosition -= overflowAmount;
                        }
                        break;
                    }
                }
            }
            var firstTooltip = sortedTooltipItems[0];
            if (firstTooltip.yPosition < (actualClipRect.y)) {
                var overflowAmount = actualClipRect.y - firstTooltip.yPosition + OVERFLOW_BUFFER;
                for (var currentIndex = 0; currentIndex < itemCount; currentIndex++) {
                    var currentTooltipBottom = sortedTooltipItems[currentIndex].yPosition + sortedTooltipItems[currentIndex].height;
                    var nextTooltipY = currentIndex === itemCount - 1
                        ? (actualClipRect.height)
                        : sortedTooltipItems[currentIndex + 1].yPosition;
                    var availableSpace = nextTooltipY - currentTooltipBottom;
                    if (availableSpace > overflowAmount) {
                        for (var shiftIndex = currentIndex; shiftIndex >= 0; shiftIndex--) {
                            sortedTooltipItems[shiftIndex].yPosition += overflowAmount;
                        }
                        break;
                    }
                }
            }
            for (var _g = 0, sortedTooltipItems_1 = sortedTooltipItems; _g < sortedTooltipItems_1.length; _g++) {
                var tooltipItem = sortedTooltipItems_1[_g];
                this.splitTooltipRectCollection[tooltipItem.originalIndex].y = tooltipItem.yPosition;
            }
        }

        /**
         * Draws a connector line between the default tooltip position and the actual position
         * @param splitIndex - Index of the split tooltip
         * @param pointLocationX - pointLocation X position of the tooltip
         * @param pointLocationY - pointLocation Y position of the tooltip
         * @returns The connector line element
         * @private
         */
        drawConnectorLine(splitIndex, pointLocationX, pointLocationY) {
            const startX = pointLocationX - this.splitTooltipRectCollection[splitIndex].x;
            const startY = pointLocationY - this.splitTooltipRectCollection[splitIndex].y;
            const endX = this.hasHorizontalOverflow ? this.splitTooltipRectCollection[splitIndex].width : 0;
            const endY = this.splitTooltipRectCollection[splitIndex].height / 2;
            const connectorPath = 'M ' + startX + ' ' + startY + ' L ' + endX + ' ' + endY;
            const connectorLine = this.renderer.drawPath({
                'id': this.element.id + '_tooltip_connector_' + splitIndex,
                'd': connectorPath,
                'stroke': this.palette[splitIndex],
                'stroke-width': 1,
                'opacity': this.opacity
            });
            return connectorLine;
        };
        /**
         * Handle split tooltip connector and compute initial rect and overflow.
         * Returns the rect, whether tooltip overflows and whether it should be left-aligned.
         */
        handleSplitPosition(splitIndex, groupElement, areaBounds, arrowLocation, tipLocation) {
            var pointLocationX = this.splitLocations[splitIndex].x + this.splitClipBounds[splitIndex].x;
            var pointLocationY = this.splitLocations[splitIndex].y + this.splitClipBounds[splitIndex].y;
            var isConnectorLineNeeded = pointLocationY !== (this.splitTooltipRectCollection[splitIndex].y + this.splitTooltipRectCollection[splitIndex].height / 2) && (this.seriesTypes[splitIndex] !== 'Column');
            var isLeft = false;
            if (isConnectorLineNeeded) {
                var connectorLine = this.drawConnectorLine(splitIndex, pointLocationX, pointLocationY);
                groupElement.appendChild(connectorLine);
            }
            else {
                if (document.getElementById(this.element.id + '_tooltip_connector_' + splitIndex)) {
                    removeElement(this.element.id + '_tooltip_connector_' + splitIndex);
                }
                arrowLocation.x = this.hasHorizontalOverflow ? this.splitTooltipRectCollection[splitIndex].width : 0;
                arrowLocation.y = this.splitTooltipRectCollection[splitIndex].height / 2;
                tipLocation.y = this.splitTooltipRectCollection[splitIndex].height / 2;
                if (this.hasHorizontalOverflow) {
                    isLeft = true;
                }
            }
            var rect = this.splitTooltipRectCollection[splitIndex];
            var tooltipOverflow = (rect.y < (0) || rect.y + rect.height > (areaBounds.height)) ||
                !(withInAreaBounds(pointLocationX, pointLocationY, this.splitClipBounds[splitIndex]));
            return { rect: rect, tooltipOverflow: tooltipOverflow, isLeft: isLeft, isConnectorLineNeeded: isConnectorLineNeeded };
        }

        createTooltipElement() {
            this.textElements = [];
            if (!this.template || this.shared) {
                // SVG element for tooltip
                if (this.enableRTL) {
                    this.element.setAttribute('dir', 'ltr');
                }
                var svgObject = this.renderer.createSvg({ id: this.element.id + '_svg' });
                this.element.appendChild(svgObject);
                if (this.split) {
                    this.configureSplitTooltipElements(svgObject);
                }
                else {
                    // Group to hold text and path.
                    var groupElement = document.getElementById(this.element.id + '_group');
                    if (!groupElement) {
                        groupElement = this.renderer.createGroup({ id: this.element.id + '_group' });
                        groupElement.setAttribute('transform', 'translate(0,0)');
                    }
                    svgObject.appendChild(groupElement);
                    var pathElement = this.renderer.drawPath({
                        'id': this.element.id + '_path',
                        'stroke-width': ((this.theme === 'Fluent') && !this.border.width) ? 1 : this.border.width,
                        'fill': this.fill || this.themeStyle.tooltipFill,
                        'opacity': ((this.theme.indexOf('Fluent') > -1) && this.opacity === 0.75) ? 1 : this.opacity,
                        'stroke': this.border.color || (this.theme === 'Fluent' ? '#D2D0CE' : this.border.color)
                    });
                    groupElement.appendChild(pathElement);
                }
            }
        }

        /**
         * Create groups and paths for split tooltip mode.
         * @private
         */
        configureSplitTooltipElements(svgObject) {
            for (var splitIndex = 0; splitIndex < (this.split ? this.content.length : 1); splitIndex++) {
                // Group to hold text and path for each split tooltip.
                var groupElement = document.getElementById(this.element.id + '_group_' + splitIndex);
                if (!groupElement) {
                    groupElement = this.renderer.createGroup({ id: this.element.id + '_group_' + splitIndex });
                    groupElement.setAttribute('transform', 'translate(0,0)');
                }
                svgObject.appendChild(groupElement);
                var pathElement = this.renderer.drawPath({
                    'id': this.element.id + '_path_' + splitIndex,
                    'stroke-width': ((this.theme === 'Fluent') && !this.border.width) ? 1 : this.border.width,
                    'fill': this.fill || this.themeStyle.tooltipFill,
                    'opacity': ((this.theme.indexOf('Fluent') > -1) && this.opacity === 0.75) ? 1 : this.opacity,
                    'stroke': this.border.color || (this.theme === 'Fluent' ? '#D2D0CE' : this.border.color)
                });
                groupElement.appendChild(pathElement);
            }
        }

        drawMarker(isBottom, isRight, size, splitIndex) {
            if (this.shapes.length <= 0) {
                return null;
            }
            var shapeOption;
            var count = 0;
            var markerGroup = this.renderer.createGroup({ id: (this.split ? this.element.id + '_trackball_group_' + splitIndex : this.element.id + '_trackball_group') });
            var groupElement = getElement(this.split ? this.element.id + '_group_' + splitIndex : this.element.id + '_group');
            if (!groupElement) {
                return null;
            }
            var x = ((this.enableRTL) ? (this.split ? this.splitTooltipRectCollection[splitIndex].width
                - size - this.marginX : this.elementSize.width - (size / 2)) :
                (this.marginX * 2) + (size / 2)) + (isRight ? this.arrowPadding : 0);
            for (var index = 0; index < this.shapes.length; index++) {
                var shape = this.shapes[this.split ? splitIndex : index];
                if (!(this.split && (splitIndex !== index))) {
                    if (shape !== 'None') {
                        shapeOption = new PathOption(this.element.id + ('_Trackball_' + (this.split ? splitIndex : count)), this.palette[(this.split ? splitIndex : count)], 1, '#cccccc', 1, null);
                        if (this.markerPoint[(this.split ? 0 : count)]) {
                            var padding = 0;
                            if (this.header.indexOf('<br') > -1) {
                                padding = this.header.split(/<br.*?>/g).length + (this.split ? splitIndex : count);
                            }
                            var tooltipContent = (this.formattedText && this.formattedText.length >= 2)
                                ? this.getTooltipTextContent(this.formattedText[1]) + ", " + this.getTooltipTextContent(this.formattedText[0])
                                : '';
                            markerGroup.appendChild(drawSymbol(new TooltipLocation(x, this.markerPoint[(this.split ? 0 : count)]
                                - this.padding + (isBottom ? this.arrowPadding : padding)), shape, new Size(size, size), this.markerImage, shapeOption, 'img', tooltipContent));
                        }
                        count++;
                    }
                }
            }
            groupElement.appendChild(markerGroup);
        }

        renderTooltipElement(areaBounds, location, splitIndex) {
            var tooltipDiv = getElement(this.element.id);
            var arrowLocation = new TooltipLocation(0, 0);
            var tipLocation = new TooltipLocation(0, 0);
            var svgObject = getElement(this.element.id + '_svg');
            var groupElement = getElement(this.split ? this.element.id + '_group_' + splitIndex : this.element.id + '_group');
            var pathElement = getElement(this.split ? this.element.id + '_path_' + splitIndex : this.element.id + '_path');
            var rect;
            var isConnectorLineNeeded;
            var isTop = false;
            var isLeft = false;
            var isBottom = false;
            var xValue = 0;
            var yValue = 0;
            var tooltipOverflow;
            if (!sfBlazorToolkit.base.isNullOrUndefined(groupElement)) {
                if (this.header !== '' && this.showHeaderLine) {
                    this.elementSize.height += this.marginY;
                }
                if (this.split) {
                    var splitResult = this.handleSplitPosition(splitIndex, groupElement, areaBounds, arrowLocation, tipLocation);
                    rect = splitResult.rect;
                    tooltipOverflow = splitResult.tooltipOverflow;
                    isConnectorLineNeeded = splitResult.isConnectorLineNeeded;
                    isLeft = splitResult.isLeft;
                }
                else if (this.isFixed) {
                    var width = this.elementSize.width + (2 * this.marginX);
                    var height = this.elementSize.height + (2 * this.marginY);
                    rect = new Rect(location.x, location.y, width, height);
                }
                else if (this.content.length > 1 || this.followPointer) {
                    rect = this.sharedTooltipLocation(areaBounds, this.location.x, this.location.y);
                    isTop = true;
                }
                else {
                    rect = this.tooltipLocation(areaBounds, location, arrowLocation, tipLocation);
                    if (!this.inverted) {
                        isTop = (rect.y < (location.y + this.clipBounds.y));
                        isBottom = !isTop;
                        yValue = (isTop ? 0 : this.arrowPadding);
                    }
                    else {
                        isLeft = (rect.x < (location.x + this.clipBounds.x));
                        xValue = (isLeft ? 0 : this.arrowPadding);
                        if (this.allowHighlight) {
                            rect.x += isLeft ? this.highlightPadding : -(2 * this.highlightPadding);
                        }
                    }
                }
                if (this.header !== '' && this.showHeaderLine && !this.split) {
                    var wrapPadding = 2;
                    var padding = 0;
                    var wrapHeader = this.isWrap ? this.wrappedText : this.header;
                    if (this.isWrap && typeof (wrapHeader) === 'string' && (wrapHeader.indexOf('<') > -1 || wrapHeader.indexOf('>') > -1)) {
                        var textArray = wrapHeader.split('<br>');
                        wrapPadding = textArray.length;
                    }
                    if (this.header.indexOf('<br') > -1) {
                        padding = 5 * (this.header.split(/<br.*?>/g).length - 1);
                    }
                    var key = 'properties';
                    var font = sfBlazorToolkit.base.extend({}, this.textStyle, null, true)[key];
                    var headerSize = measureText(this.isWrap ? this.wrappedText : this.header, font, this.themeStyle.textStyle).height
                        + (this.marginY * wrapPadding) + (isBottom ? this.arrowPadding : 0) + (this.isWrap ? 5 : padding); //header padding;
                    var xLength = (this.marginX * 3) + (!isLeft && !isTop && !isBottom ? this.arrowPadding : 0);
                    var direction = 'M ' + xLength + ' ' + headerSize +
                        'L ' + (rect.width + (!isLeft && !isTop && !isBottom ? this.arrowPadding : 0) - (this.marginX * 2)) +
                        ' ' + headerSize;
                    var pathElement_1 = this.renderer.drawPath({
                        'id': this.element.id + '_header_path', 'stroke-width': 1,
                        'fill': null, 'opacity': 0.8, 'stroke': this.themeStyle.tooltipHeaderLine, 'd': direction
                    });
                    groupElement.appendChild(pathElement_1);
                }
                var start = this.border.width / 2;
                var pointRect = new Rect(start + xValue, start + yValue, rect.width - start, rect.height - start);
                groupElement.setAttribute('opacity', this.split && tooltipOverflow ? '0' : '1');
                if (this.enableAnimation && !this.isFirst && !this.crosshair) {
                    this.animateTooltipDiv((this.split ? groupElement : tooltipDiv), rect);
                }
                else {
                    this.updateDiv((this.split ? groupElement : tooltipDiv), rect.x, rect.y);
                }
                // eslint-disable-next-line no-extra-boolean-cast
                svgObject.setAttribute('height', (this.split ? this.areaBounds.height : (rect.height + this.border.width + (!((!this.inverted)) ? 0 : this.arrowPadding) + 5)).toString());
                svgObject.setAttribute('width', (this.split ? this.areaBounds.width : (rect.width + this.border.width + (((!this.inverted)) ? 0 : this.arrowPadding) + 5)).toString());
                svgObject.setAttribute('opacity', '1');
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.tooltipPlacement)) {
                    isTop = this.tooltipPlacement.indexOf('Top') > -1;
                    isBottom = this.tooltipPlacement.indexOf('Bottom') > -1;
                    isLeft = this.tooltipPlacement.indexOf('Left') > -1;
                }
                pathElement.setAttribute('d', findDirection(this.rx, this.ry, pointRect, arrowLocation, isConnectorLineNeeded ? 0 : this.arrowPadding, isTop, isBottom, isLeft, tipLocation.x, tipLocation.y, this.controlName));
                if ((this.enableShadow) || this.theme.indexOf('Fluent') > -1) {
                    var shadowId = this.element.id + '_shadow';
                    pathElement.setAttribute('filter', sfBlazorToolkit.base.Browser.isIE ? '' : 'url(#' + shadowId + ')');
                    var shadow = '<filter id="' + shadowId + '" height="130%"><feGaussianBlur in="SourceAlpha" stdDeviation="3"/>';
                    if (this.theme.indexOf('Fluent') > -1) {
                        shadow += '<feOffset dx="-1" dy="3.6" result="offsetblur"/><feComponentTransfer><feFuncA type="linear" slope="0.2"/>';
                    }
                    else {
                        shadow += '<feOffset dx="3" dy="3" result="offsetblur"/><feComponentTransfer><feFuncA type="linear" slope="0.5"/>';
                    }
                    shadow += '</feComponentTransfer><feMerge><feMergeNode/><feMergeNode in="SourceGraphic"/></feMerge></filter>';
                    var defElement = this.renderer.createDefs();
                    defElement.setAttribute('id', this.element.id + 'SVG_tooltip_definition');
                    groupElement.appendChild(defElement);
                    defElement.innerHTML = shadow;
                }
                var borderColor = ((this.theme === 'Fluent') && !this.border.color) ? '#D2D0CE' : this.border.color;
                pathElement.setAttribute('stroke', borderColor);
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.border.dashArray)) {
                    pathElement.setAttribute('stroke-dasharray', this.border.dashArray);
                }
                this.changeText(new TooltipLocation(xValue, yValue), isBottom, !isLeft && !isTop && !isBottom, splitIndex);
                if (this.revert) {
                    this.inverted = !this.inverted;
                    this.revert = false;
                }
            }
            return new Side(isBottom, !isLeft && !isTop && !isBottom);
        }

        changeText(point, isBottom, isRight, splitIndex) {
            var element = document.getElementById(this.split ? this.element.id + '_text_' + splitIndex : this.element.id + '_text');
            if (isBottom) {
                element.setAttribute('transform', 'translate(0,' + this.arrowPadding + ')');
            }
            if (isRight) {
                element.setAttribute('transform', 'translate(' + this.arrowPadding + ' 0)');
            }
        }

        findFormattedText(splitIndex) {
            this.formattedText = [];
            if (this.header.replace(/<b>/g, '').replace(/<\/b>/g, '').trim() !== '' && !this.split) {
                this.formattedText = this.formattedText.concat(this.header);
            }
            this.formattedText = this.formattedText.concat(this.split ? this.content[splitIndex] : this.content);
        }

        // tslint:disable-next-line:max-func-body-length
        renderText(isRender, splitIndex) {
            var height = 0;
            var width = 0; // Padding for text;
            var subWidth = 0;
            var lines;
            var key = 'properties';
            var font = sfBlazorToolkit.base.extend({}, this.textStyle, null, true)[key];
            var groupElement = getElement(this.split ? this.element.id + '_group_' + splitIndex : this.element.id + '_group');
            var tspanElement;
            var textCollection;
            var tspanStyle = '';
            var line;
            var tspanOption;
            this.findFormattedText(splitIndex);
            this.isWrap = false;
            var isRtlEnabled = document.body.getAttribute('dir') === 'rtl';
            var anchor = isRtlEnabled && !this.enableRTL ? 'end' : 'start';
            this.leftSpace = this.areaBounds.x + this.location.x;
            this.rightSpace = (this.areaBounds.x + this.areaBounds.width) - this.leftSpace;
            var headerContent = this.split ? '' : this.header.replace(/<b>/g, '').replace(/<\/b>/g, '').trim();
            var isBoldTag = this.header.indexOf('<b>') > -1 && this.header.indexOf('</b>') > -1;
            var headerWidth = measureText(this.formattedText[0], font, this.themeStyle.textStyle).width
                + (2 * this.marginX) + this.arrowPadding;
            var isLeftSpace = (this.location.x - headerWidth) < this.location.x;
            var isRightSpace = (this.areaBounds.x + this.areaBounds.width) < (this.location.x + headerWidth);
            var header;
            var headerSpace = (headerContent !== '' && this.showHeaderLine) ? this.marginY : 0;
            var isRow = true;
            var isColumn = true;
            this.markerPoint = [];
            var markerSize = (this.shapes.length > 0) ? 10 : 0;
            var markerPadding = (this.shapes.length > 0) ? 5 : 0;
            var spaceWidth = 4;
            var subStringLength;
            var fontSize = '12px';
            var fontWeight = '400';
            var labelColor = this.themeStyle.tooltipLightLabel;
            var dy = (22 / parseFloat(fontSize)) * (parseFloat(font.size || this.themeStyle.textStyle.size));
            var contentWidth = [];
            var textHeight = 0;
            if (!isRender || this.isCanvas) {
                removeElement(this.split ? this.element.id + '_text_' + splitIndex : this.element.id + '_text');
                removeElement(this.element.id + '_header_path');
                removeElement(this.split ? this.element.id + '_trackball_group_' + splitIndex : this.element.id + '_trackball_group');
                removeElement(this.element.id + 'SVG_tooltip_definition');
            }
            // Condition to resolve the text size issue only in chart.
            if (this.controlName === 'Chart' && parseFloat(fontSize) < parseFloat(font.size || this.themeStyle.textStyle.headerTextSize)) {
                textHeight = (parseFloat(font.size || this.themeStyle.textStyle.size) - parseFloat(fontSize));
            }
            var options = new TextOption(this.split ? this.element.id + '_text_' + splitIndex : this.element.id + '_text', this.marginX * 2, (textHeight + this.marginY * 2 + this.padding * 2 + (this.marginY === 2 ? this.controlName === 'RangeNavigator' ? 5 : 3 : 0)), anchor, '');
            var parentElement = textElement(options, font, font.color || this.themeStyle.tooltipBoldLabel, groupElement, this.themeStyle.textStyle);
            var withoutHeader = this.formattedText.length === 1 && this.formattedText[0].indexOf(' : <b>') > -1;
            var isHeader = this.split ? false : this.header !== '';
            var size = isHeader && isBoldTag ? 16 : 13;
            for (var k = 0, pointsLength = this.formattedText.length; k < pointsLength; k++) {
                textCollection = this.formattedText[k].replace(/<(b|strong)>/g, '<b>')
                    .replace(/<\/(b|strong)>/g, '</b>')
                    .split(/<br.*?>/g);
                if (this.isTextWrap && this.header !== this.formattedText[k] && this.formattedText[k].indexOf('<br') === -1) {
                    subStringLength = Math.round(this.leftSpace > this.rightSpace ? (this.leftSpace / size) : (this.rightSpace / size));
                    textCollection = this.formattedText[k].match(new RegExp('.{1,' + subStringLength + '}', 'g'));
                }
                if (k === 0 && !withoutHeader && this.isTextWrap &&
                    (this.leftSpace < headerWidth || isLeftSpace) &&
                    (this.rightSpace < headerWidth || isRightSpace)) {
                    subStringLength = Math.round(this.leftSpace > this.rightSpace ? (this.leftSpace / size) : (this.rightSpace / size));
                    header = headerContent !== '' ? headerContent : this.formattedText[k];
                    textCollection = header.match(new RegExp('.{1,' + subStringLength + '}', 'g'));
                    this.wrappedText = isBoldTag ? '<b>' + textCollection.join('<br>') + '</b>' : textCollection.join('<br>');
                    this.isWrap = textCollection.length > 1;
                }
                if (textCollection[0] === '') {
                    continue;
                }
                if ((k !== 0) || (headerContent === '')) {
                    this.markerPoint.push(((headerContent !== '' && this.showHeaderLine) ? (this.marginY) : 0) + options.y + height - (textHeight !== 0 ? ((textHeight / this.markerSize) * (parseFloat(font.size || this.themeStyle.textStyle.headerTextSize) / this.markerSize)) : 0));
                }
                for (var i = 0, len = textCollection.length; i < len; i++) { // string value of unicode for LTR is \u200E
                    lines = textCollection[i].replace(/<b>/g, '<br><b>').replace(/<\/b>/g, '</b><br>').replace(/:/g, (this.enableRTL) ? '<br>\u200E: <br>' : '<br>\u200E:<br>')
                        .split('<br>');
                    if (this.enableRTL && lines.length > 0) {
                        var colonMatches = textCollection[i].match(/:/g);
                        var colonCount = colonMatches ? colonMatches.length : 0;
                        var shouldReverse = colonCount > 0 &&
                            (this.controlName === 'Sankey' ? colonCount === 1 : true);
                        if (shouldReverse) {
                            lines[0] = lines[0].trim();
                            lines.reverse();
                        }
                    }
                    subWidth = 0;
                    isColumn = true;
                    height += dy;
                    for (var j = 0, len_1 = lines.length; j < len_1; j++) {
                        line = lines[j];
                        if (this.enableRTL && line !== '' && this.isRTLText(line)) {
                            line = line.concat('\u200E');
                        }
                        if (!/\S/.test(line) && line !== '') {
                            line = ' '; //to trim multiple white spaces to single white space
                        }
                        if ((!isColumn && line === ' ') || (line.replace(/<b>/g, '').replace(/<\/b>/g, '').trim() !== '')) {
                            subWidth += line !== ' ' ? spaceWidth : 0;
                            if (isColumn && !isRow) {
                                if (this.header.indexOf('<br') > -1 && k !== 0) {
                                    headerSpace += this.header.split(/<br.*?>/g).length;
                                }
                                tspanOption = {
                                    x: (this.marginX * 2) + (markerSize + markerPadding),
                                    dy: dy + ((isColumn) ? headerSpace : 0), fill: ''
                                };
                                headerSpace = null;
                            }
                            else {
                                if (isRow && isColumn) {
                                    tspanOption = {
                                        x: (headerContent === '') ? ((this.marginX * 2) + (markerSize + markerPadding))
                                            : (this.marginX * 2) + (this.isWrap ? (markerSize + markerPadding) : 0)
                                    };
                                }
                                else {
                                    tspanOption = {};
                                }
                            }
                            isColumn = false;
                            tspanElement = this.renderer.createTSpan(tspanOption, '');
                            parentElement.appendChild(tspanElement);
                            if (line.indexOf('<b>') > -1 || ((isBoldTag && j === 0 && k === 0) && (isHeader || this.isWrap))) {
                                fontWeight = '600';
                                labelColor = this.themeStyle.tooltipBoldLabel;
                                tspanStyle = 'font-weight:' + fontWeight;
                                font.fontWeight = fontWeight;
                                (tspanElement).setAttribute('fill', this.textStyle.color || labelColor);
                            }
                            else {
                                tspanStyle = fontWeight === '600' ? 'font-weight:' + fontWeight : '';
                                font.fontWeight = fontWeight;
                                (tspanElement).setAttribute('fill', this.textStyle.color || labelColor);
                            }
                            if (line.indexOf('</b>') > -1 || ((isBoldTag && j === len_1 - 1 && k === 0) && (isHeader || this.isWrap))) {
                                fontWeight = 'Normal';
                                labelColor = this.themeStyle.tooltipLightLabel;
                            }
                            // eslint-disable-next-line no-useless-escape
                            if (tspanStyle !== '') {
                                tspanElement.style.fontWeight = tspanStyle.split('font-weight:')[1];
                                tspanElement.style.color = tspanElement.getAttribute('fill');
                            }
                            // 'inherit' will apply css style from parent element.
                            tspanElement.style.fontFamily = 'inherit';
                            tspanElement.style.fontStyle = 'inherit';
                            tspanElement.style.fontSize = (this.header === this.formattedText[k]) ? font.size || this.themeStyle.textStyle.headerTextSize : (line.indexOf('<b>') > -1 || line.indexOf('</b>') > -1) ? font.size || this.themeStyle.textStyle.boldTextSize : font.size || this.themeStyle.textStyle.size;
                            tspanElement.style.fontWeight = (this.header === this.formattedText[k] && (this.header.indexOf('<b>') === -1 || this.header.indexOf('</b>') === -1)) ? (this.textStyle.fontWeight || '600') : line.indexOf('<b>') > -1 || line.indexOf('</b>') > -1 ? 'bold' : (this.textStyle.fontWeight || font.fontWeight);
                            var textFont = sfBlazorToolkit.base.extend({}, this.textStyle, null, true)[key];
                            textFont.fontWeight = tspanElement.style.fontWeight;
                            textFont.size = tspanElement.style.fontSize;
                            isRow = false;
                            (tspanElement).textContent = line = this.getTooltipTextContent(line);
                            subWidth += measureText(line, textFont, this.themeStyle.textStyle).width;
                        }
                    }
                    subWidth -= spaceWidth;
                    width = Math.max(width, subWidth);
                    contentWidth.push(subWidth);
                }
            }
            this.elementSize = new Size(width + (width > 0 ? (2 * this.marginX) : 0), height);
            this.elementSize.width += (markerSize + markerPadding); // marker size + marker Spacing
            var element = (parentElement.childNodes[0]);
            if (headerContent !== '' && element && !this.isWrap) {
                font.fontWeight = '600';
                var width_1 = (this.elementSize.width + (2 * this.padding)) / 2 - measureText(headerContent, font, this.themeStyle.textStyle, true).width
                    / 2;
                element.setAttribute('x', width_1.toString());
            }
            this.renderContentRTL(parentElement, isHeader, markerSize + markerPadding, contentWidth);
            if (this.split) {
                var padding = 20;
                var elementWidth = this.elementSize.width + (2 * this.marginX);
                var elementHeight = this.elementSize.height + (2 * this.marginY);
                var rect = new Rect(this.splitLocations[splitIndex].x
                    + this.splitClipBounds[splitIndex].x + padding, (this.splitLocations[splitIndex].y
                        + this.splitClipBounds[splitIndex].y) - elementHeight / 2, elementWidth, elementHeight);
                this.splitTooltipRectCollection.push(rect);
            }
        }

        renderContentRTL(textElement, isHeader, markerSize, contentWidth) {
            if (this.enableRTL) {
                var tspanElement = void 0;
                var contentWidthIndex = isHeader ? 1 : 0;
                for (var i = 0; i < textElement.childNodes.length; i++) {
                    tspanElement = (textElement.childNodes[i]);
                    if ((!isHeader || i > 0) && !sfBlazorToolkit.base.isNullOrUndefined(tspanElement.getAttribute('x'))) {
                        tspanElement.setAttribute('x', (this.elementSize.width - (markerSize + contentWidth[contentWidthIndex])).toString());
                        contentWidthIndex++;
                    }
                }
            }
        }

        getTooltipTextContent(tooltipText) {
            var characterCollection = tooltipText.match(/<[a-zA-Z\/](.|\n)*?>/g);
            if (sfBlazorToolkit.base.isNullOrUndefined(characterCollection)) {
                return tooltipText;
            }
            var isRtlText = this.isRTLText(tooltipText);
            for (var i = 0; i < characterCollection.length; i++) {
                if (this.isValidHTMLElement(characterCollection[i].replace('<', '').replace('/', '').replace('>', '').trim())) {
                    tooltipText = tooltipText.replace(characterCollection[i], isRtlText ? '\u200E' : '');
                }
            }
            return tooltipText;
        }

        isValidHTMLElement(element) {
            return document.createElement(element).toString() !== '[object HTMLUnknownElement]';
        }

        isRTLText(tooltipContent) {
            return /[\u0590-\u07FF\u200F\u202B\u202E\uFB1D-\uFDFD\uFE70-\uFEFC]/.test(tooltipContent);
        }

        createTemplate(areaBounds, location, splitIndex) {
            var argsData = { cancel: false, name: 'tooltipRender', tooltip: this };

            this.trigger('tooltipRender', argsData);
            var parent = document.getElementById(this.element.id);
            if (this.isCanvas) {
                this.removeSVG();
            }
            if (this.split && !sfBlazorToolkit.base.isNullOrUndefined(splitIndex)) {
                parent.setAttribute('opacity', '1');
                parent.style.display = '';
                var splitParentId = this.element.id + 'parent_template' + splitIndex;
                var splitParentElem = document.getElementById(splitParentId);
                if (splitParentElem) {
                    sfBlazorToolkit.base.remove(splitParentElem);
                }
            }
            else {
                var firstElement = !sfBlazorToolkit.base.isNullOrUndefined(parent) ? parent.firstElementChild : null;
                if (firstElement) {
                    sfBlazorToolkit.base.remove(firstElement);
                }
            }
            if (!argsData.cancel) {
                var elem = sfBlazorToolkit.base.createElement('div', { id: this.split ? this.element.id + 'parent_template' + splitIndex : this.element.id + 'parent_template' });
                var templateElement = void 0;
                if (this.controlName === 'Chart' && this.shared) {
                    for (var i = 0; i < this.data.length; i++) {
                        var sharedTemplateElement = this.templateFn(this.data[i], this.controlInstance, elem.id, elem.id + '_blazorTemplate', '');
                        if (i === 0) {
                            templateElement = sharedTemplateElement;
                        }
                        else {
                            if (sharedTemplateElement.length > 1) {
                                templateElement[i].outerHTML = sharedTemplateElement[i].outerHTML || sharedTemplateElement[i].textContent;
                            }
                            else {
                                templateElement[templateElement.length - 1].outerHTML += sharedTemplateElement[0].outerHTML;
                            }
                        }
                    }
                }
                else {
                    templateElement = this.templateFn(this.split ? this.data[splitIndex] : this.data, this.controlInstance, elem.id, elem.id + '_blazorTemplate', '');
                }
                while (templateElement && templateElement.length > 0) {
                    if (sfBlazorToolkit.base.isBlazor() || templateElement.length === 1) {
                        elem.appendChild(templateElement[0]);
                        templateElement = null;
                    }
                    else {
                        elem.appendChild(templateElement[0]);
                    }
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(parent)) {
                    parent.appendChild(elem);
                }
                var element = this.split || this.isCanvas ? elem : this.element;
                var rect = element.getBoundingClientRect();
                this.padding = 0;
                this.elementSize = new Size(rect.width, rect.height);
                var tooltipRect = void 0;
                if (this.split && !sfBlazorToolkit.base.isNullOrUndefined(splitIndex)) {
                    var padding = 20;
                    var elementWidth = this.elementSize.width + (2 * this.marginX);
                    var elementHeight = this.elementSize.height + (2 * this.marginY);
                    var rect_1 = new Rect(this.splitLocations[splitIndex].x
                        + this.splitClipBounds[splitIndex].x + padding, (this.splitLocations[splitIndex].y
                            + this.splitClipBounds[splitIndex].y) - elementHeight / 2, elementWidth, elementHeight);
                    tooltipRect = rect_1;
                }
                else {
                    tooltipRect = this.shared ? this.sharedTooltipLocation(areaBounds, this.location.x, this.location.y)
                        : this.tooltipLocation(areaBounds, location, new TooltipLocation(0, 0), new TooltipLocation(0, 0));
                }
                if (this.enableAnimation && !this.isFirst && !this.crosshair) {
                    this.animateTooltipDiv(this.element, tooltipRect);
                }
                else {
                    this.updateDiv(element, tooltipRect.x, tooltipRect.y);
                }
                if (this.blazorTemplate) {
                    var tooltipRendered = function () {
                        var rect1 = getElement(thisObject_1.element.id).getBoundingClientRect();
                        thisObject_1.elementSize = new Size(rect1.width, rect1.height);
                        var tooltipRect1 = thisObject_1.tooltipLocation(areaBounds, location, new TooltipLocation(0, 0), new TooltipLocation(0, 0));
                        thisObject_1.updateDiv(getElement(thisObject_1.element.id), tooltipRect1.x, tooltipRect1.y);
                    };
                    // eslint-disable-next-line @typescript-eslint/no-this-alias
                    var thisObject_1 = this;
                    tooltipRendered.bind(thisObject_1, areaBounds, location);
                    sfBlazorToolkit.base.updateBlazorTemplate(this.element.id + 'parent_template' + '_blazorTemplate', this.blazorTemplate.name, this.blazorTemplate.parent, undefined, tooltipRendered);
                }
            }
            else {
                sfBlazorToolkit.base.remove(getElement(this.element.id + '_tooltip'));
            }
        }

        sharedTooltipLocation(bounds, x, y) {
            var width = this.elementSize.width + (2 * this.marginX);
            var height = this.elementSize.height + (2 * this.marginY);
            var pointerTooltipRect = new Rect((this.shared || this.inverted)
                ? (x + 2 * this.padding) : (x - width / 2), (this.shared || this.inverted) ? (y - height / 2)
                : (y - (height + this.padding)), width, height);
            var tooltipRect = this.followPointer ? pointerTooltipRect : new Rect(x + 2 * this.padding, y - height - this.padding, width, height);
            if (tooltipRect.y < bounds.y) {
                tooltipRect.y += (tooltipRect.height + 2 * this.padding);
            }
            if (tooltipRect.y + tooltipRect.height > bounds.y + bounds.height) {
                tooltipRect.y = Math.max((bounds.y + bounds.height) - (tooltipRect.height + 2 * this.padding), bounds.y);
            }
            if (tooltipRect.x + tooltipRect.width > bounds.x + bounds.width) {
                tooltipRect.x = (bounds.x + this.location.x) - (tooltipRect.width + 4 * this.padding);
            }
            if (tooltipRect.x < bounds.x) {
                tooltipRect.x = bounds.x;
            }
            return tooltipRect;
        }

        /** @private */
        getCurrentPosition(bounds, symbolLocation, arrowLocation, tipLocation) {
            var position = this.tooltipPlacement;
            var clipX = this.clipBounds.x;
            var clipY = this.clipBounds.y;
            var markerHeight = this.offset;
            var width = this.elementSize.width + (2 * this.marginX);
            var height = this.elementSize.height + (2 * this.marginY);
            var location = new TooltipLocation(symbolLocation.x, symbolLocation.y);
            if (position === 'Top' || position === 'Bottom') {
                location = new TooltipLocation(location.x + clipX - this.elementSize.width / 2 - this.padding, location.y + clipY - this.elementSize.height - (2 * this.padding) - this.arrowPadding - markerHeight);
                arrowLocation.x = tipLocation.x = width / 2;
                if (position === 'Bottom') {
                    location.y = symbolLocation.y + clipY + markerHeight;
                }
                if (bounds.x + bounds.width < location.x + width) {
                    location.x = (bounds.width > width) ? ((bounds.x + bounds.width) - width + 6) : bounds.x;
                    arrowLocation.x = tipLocation.x = (bounds.width > width) ? (bounds.x + symbolLocation.x - location.x) : symbolLocation.x;
                }
                else if (bounds.x > location.x) {
                    location.x = bounds.x;
                    arrowLocation.x = tipLocation.x = symbolLocation.x;
                }
            }
            else {
                location = new TooltipLocation(location.x + clipX + markerHeight, location.y + clipY - this.elementSize.height / 2 - (this.padding));
                arrowLocation.y = tipLocation.y = height / 2;
                if (position === 'Left') {
                    location.x = symbolLocation.x + clipX - markerHeight - (width + this.arrowPadding);
                }
                if (bounds.y + bounds.height < location.y + height) {
                    location.y = (bounds.height > height) ? ((bounds.y + bounds.height) - height + 6) : bounds.y;
                    arrowLocation.y = tipLocation.y = (bounds.height > height) ? (bounds.y + symbolLocation.y - location.y) : symbolLocation.y;
                }
                else if (bounds.y > location.y) {
                    location.y = bounds.y;
                    arrowLocation.y = tipLocation.y = symbolLocation.y;
                }
            }
            return new Rect(location.x, location.y, width, height);
        }

        // tslint:disable-next-line:max-func-body-length
        /** @private */
        tooltipLocation(bounds, symbolLocation, arrowLocation, tipLocation) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.tooltipPlacement)) {
                var tooltipRect = this.getCurrentPosition(bounds, symbolLocation, arrowLocation, tipLocation);
                return tooltipRect;
            }
            var location = new TooltipLocation(symbolLocation.x, symbolLocation.y);
            var width = this.elementSize.width + (2 * this.marginX);
            var height = this.elementSize.height + (2 * this.marginY);
            var markerHeight = this.offset;
            var clipX = this.clipBounds.x;
            var clipY = this.clipBounds.y;
            var clipWidth = this.clipBounds.width;
            var clipHeight = this.clipBounds.height;
            var boundsX = bounds.x;
            var boundsY = bounds.y;
            this.outOfBounds = false;
            if (!this.inverted) {
                location = new TooltipLocation(location.x + clipX - this.elementSize.width / 2 - this.padding, location.y + clipY - this.elementSize.height - (2 * (this.allowHighlight ? this.highlightPadding : this.padding)) -
                    this.arrowPadding - markerHeight);
                arrowLocation.x = tipLocation.x = width / 2;
                if ((location.y < boundsY || (this.isNegative)) && !(this.controlName === 'Progressbar')) {
                    location.y = (symbolLocation.y < 0 ? 0 : symbolLocation.y) + clipY + markerHeight;
                }
                if (location.y + height + this.arrowPadding > boundsY + bounds.height) {
                    location.y = Math.min(symbolLocation.y, boundsY + bounds.height) + clipY
                        - this.elementSize.height - (2 * this.padding) - this.arrowPadding - markerHeight;
                }
                if (((location.x + width > boundsX + bounds.width) && location.y < boundsY || (this.isNegative)) && !(this.controlName === 'Progressbar')) {
                    location.y = (symbolLocation.y < 0 ? 0 : symbolLocation.y) + clipY + markerHeight;
                }
                tipLocation.x = width / 2;
                if (location.x < boundsX && !(this.controlName === 'Progressbar')) {
                    arrowLocation.x -= (boundsX - location.x);
                    tipLocation.x -= (boundsX - location.x);
                    location.x = boundsX;
                }
                if (location.x + width > boundsX + bounds.width && !(this.controlName === 'Progressbar')) {
                    arrowLocation.x += ((location.x + width) - (boundsX + bounds.width));
                    tipLocation.x += ((location.x + width) - (boundsX + bounds.width));
                    location.x -= ((location.x + width) - (boundsX + bounds.width));
                }
                if (location.x < boundsX && !(this.controlName === 'Progressbar')) {
                    arrowLocation.x -= (boundsX - location.x);
                    tipLocation.x -= (boundsX - location.x);
                    location.x = boundsX;
                }
                if (arrowLocation.x + this.arrowPadding > width - this.rx) {
                    arrowLocation.x = width - this.rx - this.arrowPadding;
                    tipLocation.x = width - this.rx - this.arrowPadding;
                }
                if (arrowLocation.x - this.arrowPadding < this.rx) {
                    arrowLocation.x = tipLocation.x = this.rx + this.arrowPadding;
                }
                if (this.controlName === 'Chart') {
                    if (((bounds.x + bounds.width) - (location.x + arrowLocation.x)) < this.areaMargin + this.arrowPadding ||
                        (location.x + arrowLocation.x) < this.areaMargin + this.arrowPadding) {
                        this.outOfBounds = true;
                    }
                    if (this.template && (location.y < 0)) {
                        location.y = symbolLocation.y + clipY + markerHeight;
                    }
                    if (!withInAreaBounds(location.x, location.y, bounds) || this.outOfBounds) {
                        this.inverted = !this.inverted;
                        this.revert = true;
                        location = new TooltipLocation(symbolLocation.x + markerHeight + clipX, symbolLocation.y + clipY - this.elementSize.height / 2 - (this.padding));
                        tipLocation.x = arrowLocation.x = 0;
                        tipLocation.y = arrowLocation.y = height / 2;
                        if ((location.x + this.arrowPadding + width > boundsX + bounds.width) || (this.isNegative)) {
                            location.x = (symbolLocation.x > boundsX + bounds.width ? bounds.width : symbolLocation.x)
                                + clipX - markerHeight - (this.arrowPadding + width);
                        }
                        if (location.x < boundsX) {
                            location.x = (symbolLocation.x < 0 ? 0 : symbolLocation.x) + markerHeight + clipX;
                        }
                        if (location.y <= boundsY) {
                            tipLocation.y -= (boundsY - location.y);
                            arrowLocation.y -= (boundsY - location.y);
                            location.y = boundsY;
                        }
                        if (location.y + height >= bounds.height + boundsY) {
                            arrowLocation.y += ((location.y + height) - (bounds.height + boundsY));
                            tipLocation.y += ((location.y + height) - (bounds.height + boundsY));
                            location.y -= ((location.y + height) - (bounds.height + boundsY));
                        }
                        if ((this.arrowPadding) + arrowLocation.y > height - this.ry) {
                            arrowLocation.y = height - this.arrowPadding - this.ry;
                            tipLocation.y = height;
                        }
                        if (arrowLocation.y - this.arrowPadding < this.ry) {
                            arrowLocation.y = (this.arrowPadding) + this.ry;
                            tipLocation.y = 0;
                        }
                    }
                }
            }
            else {
                location = new TooltipLocation(location.x + clipX + markerHeight, location.y + clipY - this.elementSize.height / 2 - (this.padding));
                arrowLocation.y = tipLocation.y = height / 2;
                if ((location.x + width + this.arrowPadding > boundsX + bounds.width) || (this.isNegative)) {
                    location.x = (symbolLocation.x > bounds.width + bounds.x ? bounds.width : symbolLocation.x)
                        + clipX - markerHeight - (width + this.arrowPadding);
                }
                if (symbolLocation.x > clipWidth) {
                    location.x = clipWidth + clipX - (width + this.arrowPadding);
                }
                if (location.x < boundsX) {
                    location.x = (symbolLocation.x < 0 ? 0 : symbolLocation.x) + clipX + markerHeight;
                }
                if ((location.x + width + this.arrowPadding > boundsX + bounds.width)) {
                    location.x = (symbolLocation.x > bounds.width + bounds.x ? bounds.width : symbolLocation.x)
                        + clipX - markerHeight - (width + this.arrowPadding);
                }
                if (location.y <= boundsY) {
                    arrowLocation.y -= (boundsY - location.y);
                    tipLocation.y -= (boundsY - location.y);
                    location.y = boundsY;
                }
                if (location.y + height >= boundsY + bounds.height) {
                    arrowLocation.y += ((location.y + height) - (boundsY + bounds.height));
                    tipLocation.y += ((location.y + height) - (boundsY + bounds.height));
                    location.y -= ((location.y + height) - (boundsY + bounds.height));
                }
                if (location.x + width >= boundsX + bounds.width && this.controlName === 'Chart') {
                    arrowLocation.x += ((location.x + width) - (boundsX + bounds.width));
                    tipLocation.x += ((location.x + width) - (boundsX + bounds.width));
                    location.x -= ((location.x + width) - (boundsX + bounds.width));
                    location.x = location.x - this.arrowPadding - this.padding;
                }
                if (arrowLocation.y + this.arrowPadding > height - this.ry) {
                    arrowLocation.y = height - this.ry - this.arrowPadding;
                    tipLocation.y = height;
                }
                if (arrowLocation.y - this.arrowPadding < this.ry) {
                    arrowLocation.y = tipLocation.y = this.ry + this.arrowPadding;
                }
                if (this.controlName === 'Chart') {
                    if ((location.y + arrowLocation.y) < this.areaMargin + this.arrowPadding ||
                        ((bounds.y + bounds.height) - (location.y + arrowLocation.y)) < this.areaMargin + this.arrowPadding) {
                        this.outOfBounds = true;
                    }
                    if (!withInAreaBounds(location.x, location.y, bounds) || this.outOfBounds) {
                        this.inverted = !this.inverted;
                        location = new TooltipLocation(symbolLocation.x + clipX - this.padding - this.elementSize.width / 2, symbolLocation.y + clipY - this.elementSize.height - (2 * this.padding) - markerHeight - this.arrowPadding);
                        this.revert = true;
                        tipLocation.x = arrowLocation.x = width / 2;
                        tipLocation.y = arrowLocation.y = 0;
                        if (location.y < boundsY || (this.isNegative)) {
                            location.y = (symbolLocation.y < 0 ? 0 : symbolLocation.y) + markerHeight + clipY;
                        }
                        if (location.y + this.arrowPadding + height > boundsY + bounds.height) {
                            location.y = Math.min(symbolLocation.y, boundsY + bounds.height) + clipY
                                - this.elementSize.height - (2 * this.padding) - markerHeight - this.arrowPadding;
                        }
                        tipLocation.x = width / 2;
                        if (location.x < boundsX) {
                            tipLocation.x -= (boundsX - location.x);
                            arrowLocation.x -= (boundsX - location.x);
                            location.x = boundsX;
                        }
                        if (location.x + width > bounds.width + boundsX) {
                            arrowLocation.x += ((location.x + width) - (bounds.width + boundsX));
                            tipLocation.x += ((location.x + width) - (bounds.width + boundsX));
                            location.x -= ((location.x + width) - (bounds.width + boundsX));
                        }
                        if ((this.arrowPadding) + arrowLocation.x > width - this.rx) {
                            tipLocation.x = width - this.rx - (this.arrowPadding);
                            arrowLocation.x = width - this.rx - (this.arrowPadding);
                        }
                        if (arrowLocation.x - (this.arrowPadding) < this.rx) {
                            arrowLocation.x = tipLocation.x = this.rx + (this.arrowPadding);
                        }
                    }
                }
                if (this.enableRTL && this.controlName === 'Sankey') {
                    var relativeX = location.x - boundsX;
                    var mirroredRelativeX = bounds.width - relativeX - (this.elementSize.width + (2 * this.marginX));
                    location.x = boundsX + mirroredRelativeX;
                }
            }
            return new Rect(location.x, location.y, width, height);
        }

        animateTooltipDiv(tooltipDiv, rect) {
            var _this = this;
            let xValue = parseFloat(tooltipDiv.style.left);
            let yValue = parseFloat(tooltipDiv.style.top);
            var duration = (this.duration === 0 && sfBlazorToolkit.base.animationMode === 'Enable') ? 300 : this.duration;
            if ((this.controlName === 'Chart' && (this.shared || this.split)) && !this.enableRTL) {
                var transformValues = (this.split ? tooltipDiv : this.element).style.transform.split(/[(),\s]+/);
                xValue = parseFloat(transformValues[1]);
                yValue = parseFloat(transformValues[2]);
                tooltipDiv.style.transition = 'transform ' + duration + 'ms ease';
            }
            else if (this.controlName === 'Sankey' && this.enableRTL) {
                var width = this.elementSize.width + (2 * this.marginX);
                var containerWidth = (this.availableSize && this.availableSize.width) ?
                    this.availableSize.width : (this.areaBounds.x + this.areaBounds.width);
                var targetRight = containerWidth - (rect.x + width);
                // Ensure positioning is anchored only by 'right' in RTL
                tooltipDiv.style.left = '';
                tooltipDiv.style.transform = '';
                tooltipDiv.style.transition = "right " + duration + "ms ease, top " + duration + "ms ease";
                tooltipDiv.style.right = targetRight + "px";
                tooltipDiv.style.top = rect.y + "px";
                const animationOptions = {
                    duration: duration,
                    progress: function () { },
                    end: function () {
                        _this.updateDiv(tooltipDiv, rect.x, rect.y);
                        _this.trigger('animationComplete', { tooltip: _this });
                    }
                };
                new sfBlazorToolkit.Animation(animationOptions).animate(tooltipDiv);
                return;
            }
            var currenDiff;
            const animationOptions = {
                duration: duration,
                progress: function (args) {
                    currenDiff = (args.timeStamp / args.duration);
                    tooltipDiv.style.animation = null;
                    if ((_this.controlName === 'Chart' && (_this.shared || _this.split)) && !_this.enableRTL) {
                        tooltipDiv.style.transform = 'translate(' + (xValue + (rect.x - xValue)) + 'px,' + (yValue + rect.y - yValue) + 'px)';
                        tooltipDiv.style.left = '';
                        tooltipDiv.style.top = '';
                    }
                    else if (_this.controlName === 'Chart' && _this.showNearestTooltip) {
                        tooltipDiv.style.transition = 'left ' + args.duration + 'ms ease-out, top ' + args.duration + 'ms ease-out';
                        tooltipDiv.style.left = rect.x + 'px';
                        tooltipDiv.style.top = rect.y + 'px';
                    }
                    else {
                        tooltipDiv.style.left = (xValue + currenDiff * (rect.x - xValue)) + 'px';
                        tooltipDiv.style.top = (yValue + currenDiff * (rect.y - yValue)) + 'px';
                        tooltipDiv.style.transform = _this.controlName === 'RangeNavigator' ? tooltipDiv.style.transform : '';
                    }
                },
                end: function (model) {
                    _this.updateDiv(tooltipDiv, rect.x, rect.y);
                    _this.trigger('animationComplete', { tooltip: _this });
                }
            };
            new sfBlazorToolkit.Animation(animationOptions).animate(tooltipDiv);
        }

        updateDiv(tooltipDiv, x, y) {
            if ((this.controlName === 'Chart' && ((this.shared && !this.crosshair) || this.split)) && (!this.enableRTL || this.split)) {
                tooltipDiv.style.transform = 'translate(' + x + 'px,' + y + 'px)';
                tooltipDiv.style.left = '';
                tooltipDiv.style.top = '';
                tooltipDiv.style.right = '';
            }
            else if (this.controlName === 'Sankey' && this.enableRTL) {
                var width = this.elementSize.width + (2 * this.marginX);
                var containerWidth = (this.availableSize && this.availableSize.width) ?
                    this.availableSize.width : (this.areaBounds.x + this.areaBounds.width);
                var right = containerWidth - (x + width);
                tooltipDiv.style.right = right + 'px';
                tooltipDiv.style.left = '';
                tooltipDiv.style.top = y + 'px';
                tooltipDiv.style.transform = '';
            }
            else {
                tooltipDiv.style.left = x + 'px';
                tooltipDiv.style.top = y + 'px';
                tooltipDiv.style.transform = this.controlName === 'RangeNavigator' ? tooltipDiv.style.transform : '';
                tooltipDiv.style.right = '';
            }
        }

        updateTemplateFn() {
            if (this.template) {
                try {
                    if (typeof this.template !== 'function' && document.querySelectorAll(this.template).length) {
                        this.templateFn = sfBlazorToolkit.base.compile(document.querySelector(this.template).innerHTML.trim());
                    }
                    else {
                        this.templateFn = sfBlazorToolkit.base.compile(this.template);
                    }
                }
                catch (e) {
                    this.templateFn = sfBlazorToolkit.base.compile(this.template);
                }
            }
        }

        /** @private */
        fadeOut() {
            var _this = this;
            var tooltipElement = (this.isCanvas && !this.template) ? getElement(this.element.id + '_svg') :
                getElement(this.element.id);
            var tooltipDiv = getElement(this.element.id);
            if (tooltipElement) {
                var tooltipGroup_1 = this.split ? tooltipElement : tooltipElement.firstChild;
                if (tooltipGroup_1 && tooltipGroup_1.nodeType !== Node.ELEMENT_NODE) {
                    tooltipGroup_1 = tooltipElement.firstElementChild;
                }
                if (this.isCanvas && !this.template) {
                    tooltipGroup_1 = document.getElementById(this.element.id + '_group') ? document.getElementById(this.element.id + '_group') :
                        tooltipGroup_1;
                }
                if (!tooltipGroup_1) {
                    return null;
                }
                var opacity_1 = parseFloat(tooltipGroup_1.getAttribute('opacity'));
                opacity_1 = !sfBlazorToolkit.base.isNullOrUndefined(opacity_1) ? opacity_1 : 1;
                const animationOptions = {
                    duration: 200,
                    progress: function(args) {
                        _this.progressAnimation(tooltipGroup_1, opacity_1, (args.timeStamp / args.duration));
                    },
                    end: function() {
                        _this.fadeOuted = true;
                        _this.endAnimation(tooltipGroup_1);
                        tooltipDiv.style.transition = '';
                    }
                };
                new sfBlazorToolkit.Animation(animationOptions).animate(tooltipGroup_1);
            }
        }

        progressAnimation(tooltipGroup, opacity, timeStamp) {
            tooltipGroup.style.animation = '';
            tooltipGroup.setAttribute('opacity', (opacity - timeStamp).toString());
        }

        /*
         * @hidden
         */
        endAnimation(tooltipGroup) {
            tooltipGroup.setAttribute('opacity', '0');
            if (this.template) {
                tooltipGroup.style.display = 'none';
            }
            this.trigger('animationComplete', { tooltip: this });
        }

        /**
         * Get the properties to be maintained in the persisted state.
         *
         * @private
         */
        getPersistData() {
            var keyEntity = [];
            return this.addOnPersist(keyEntity);
        }

        /**
         * Get component name
         *
         *  @private
         */
        getModuleName() {
            return 'tooltip';
        }

        /**
         * To destroy the accumulationcharts
         *
         * @private
         */
        destroy() {
            _super.prototype.destroy.call(this);
            this.element.classList.remove('e-tooltip');
        }

        /**
         * Called internally if any of the property value changed.
         *
         * @returns {void}
         * @private
         */
        onPropertyChanged(newProp, oldProp) {
            if (this.blazorTemplate) {
                sfBlazorToolkit.base.resetBlazorTemplate(this.element.id + 'parent_template' + '_blazorTemplate');
            }
            if (newProp && newProp.theme !== undefined) {
                this.themeStyle = getTooltipThemeColor(this.theme);
            }
            this.isFirst = false;
            this.render();
        }
    }

    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'enable');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'shared');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'split');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'followPointer');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'crosshair');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'enableShadow');
    sfBlazorToolkit.base.Property(null)(Tooltip.prototype, 'fill');
    sfBlazorToolkit.base.Property('')(Tooltip.prototype, 'header');
    sfBlazorToolkit.base.Property(0.75)(Tooltip.prototype, 'opacity');
    sfBlazorToolkit.base.Complex({ size: '12px', fontWeight: null, color: null, fontStyle: 'Normal', fontFamily: null }, TextStyle)(Tooltip.prototype, 'textStyle');
    sfBlazorToolkit.base.Property(null)(Tooltip.prototype, 'template');
    sfBlazorToolkit.base.Property(true)(Tooltip.prototype, 'enableAnimation');
    sfBlazorToolkit.base.Property(300)(Tooltip.prototype, 'duration');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'inverted');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'isNegative');
    sfBlazorToolkit.base.Complex({ color: null, width: null }, TooltipBorder)(Tooltip.prototype, 'border');
    sfBlazorToolkit.base.Property([])(Tooltip.prototype, 'content');
    sfBlazorToolkit.base.Property(10)(Tooltip.prototype, 'markerSize');
    sfBlazorToolkit.base.Complex({ x: 0, y: 0 }, ToolLocation)(Tooltip.prototype, 'clipBounds');
    sfBlazorToolkit.base.Property([])(Tooltip.prototype, 'splitClipBounds');
    sfBlazorToolkit.base.Property([])(Tooltip.prototype, 'palette');
    sfBlazorToolkit.base.Property([])(Tooltip.prototype, 'shapes');
    sfBlazorToolkit.base.Property('')(Tooltip.prototype, 'markerImage');
    sfBlazorToolkit.base.Complex({ x: 0, y: 0 }, ToolLocation)(Tooltip.prototype, 'location');
    sfBlazorToolkit.base.Complex({ x: 0, y: 0 }, ToolLocation)(Tooltip.prototype, 'splitLocations');
    sfBlazorToolkit.base.Property([])(Tooltip.prototype, 'seriesTypes');
    sfBlazorToolkit.base.Property(0)(Tooltip.prototype, 'offset');
    sfBlazorToolkit.base.Property(4)(Tooltip.prototype, 'rx');
    sfBlazorToolkit.base.Property(4)(Tooltip.prototype, 'ry');
    sfBlazorToolkit.base.Property(5)(Tooltip.prototype, 'marginX');
    sfBlazorToolkit.base.Property(5)(Tooltip.prototype, 'marginY');
    sfBlazorToolkit.base.Property(7)(Tooltip.prototype, 'arrowPadding');
    sfBlazorToolkit.base.Property(null)(Tooltip.prototype, 'data');
    sfBlazorToolkit.base.Property('Fluent')(Tooltip.prototype, 'theme');
    sfBlazorToolkit.base.Complex({ x: 0, y: 0, width: 0, height: 0 }, AreaBounds)(Tooltip.prototype, 'areaBounds');
    sfBlazorToolkit.base.Property(null)(Tooltip.prototype, 'availableSize');
    sfBlazorToolkit.base.Property()(Tooltip.prototype, 'blazorTemplate');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'isCanvas');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'isTextWrap');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'isFixed');
    sfBlazorToolkit.base.Property(null)(Tooltip.prototype, 'tooltipPlacement');
    sfBlazorToolkit.base.Property(null)(Tooltip.prototype, 'controlInstance');
    sfBlazorToolkit.base.Property('')(Tooltip.prototype, 'controlName');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'showNearestTooltip');
    sfBlazorToolkit.base.Event1()(Tooltip.prototype, 'tooltipRender');
    sfBlazorToolkit.base.Event1()(Tooltip.prototype, 'loaded');
    sfBlazorToolkit.base.Event1()(Tooltip.prototype, 'animationComplete');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'enableRTL');
    sfBlazorToolkit.base.Property(false)(Tooltip.prototype, 'allowHighlight');
    sfBlazorToolkit.base.Property(true)(Tooltip.prototype, 'showHeaderLine');
    sfBlazorToolkit.base.NotifyPropertyChanges(Tooltip);
 
    exports.AreaBounds = AreaBounds;
    exports.CanvasRenderer = CanvasRenderer;
    exports.CustomizeOption = CustomizeOption;
    exports.PathOption = PathOption;
    exports.Rect = Rect;
    exports.Side = Side;
    exports.Size = Size;
    exports.SvgRenderer = SvgRenderer;
    exports.TextOption = TextOption;
    exports.TextStyle = TextStyle;
    exports.ToolLocation = ToolLocation;
    exports.Tooltip = Tooltip;
    exports.TooltipBorder = TooltipBorder;
    exports.TooltipLocation = TooltipLocation;
    exports.calculateShapes = calculateShapes;
    exports.drawSymbol = drawSymbol;
    exports.findDirection = findDirection;
    exports.getElement = getElement;
    exports.getTooltipThemeColor = getTooltipThemeColor;
    exports.measureText = measureText;
    exports.removeElement = removeElement;
    exports.textElement = textElement;
    exports.withInAreaBounds = withInAreaBounds;

    return exports;

});

svgbase = svgbase({});
