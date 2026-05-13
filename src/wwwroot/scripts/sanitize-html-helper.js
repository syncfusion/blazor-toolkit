window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.SanitizeHtmlHelper = (function () {
    'use strict';

    var removeTags = [
        'script',
        'style',
        'iframe[src]',
        'link[href*="javascript:"]',
        'object[type="text/x-scriptlet"]',
        'object[data^="data:text/html;base64"]',
        'img[src^="data:text/html;base64"]',
        '[src^="javascript:"]',
        '[dynsrc^="javascript:"]',
        '[lowsrc^="javascript:"]',
        '[type^="application/x-shockwave-flash"]'
    ];

    var removeAttrs = [
        { attribute: 'href', selector: '[href*="javascript:"]' },
        { attribute: 'background', selector: '[background^="javascript:"]' },
        { attribute: 'style', selector: '[style*="javascript:"]' },
        { attribute: 'style', selector: '[style*="expression("]' },
        { attribute: 'href', selector: 'a[href^="data:text/html;base64"]' }
    ];

    var jsEvents = ['onchange',
        'onclick',
        'onmouseover',
        'onmouseout',
        'onkeydown',
        'onload',
        'onerror',
        'onblur',
        'onfocus',
        'onbeforeload',
        'onbeforeunload',
        'onkeyup',
        'onsubmit',
        'onafterprint',
        'onbeforeonload',
        'onbeforeprint',
        'oncanplay',
        'oncanplaythrough',
        'oncontextmenu',
        'ondblclick',
        'ondrag',
        'ondragend',
        'ondragenter',
        'ondragleave',
        'ondragover',
        'ondragstart',
        'ondrop',
        'ondurationchange',
        'onemptied',
        'onended',
        'onformchange',
        'onforminput',
        'onhaschange',
        'oninput',
        'oninvalid',
        'onkeypress',
        'onloadeddata',
        'onloadedmetadata',
        'onloadstart',
        'onmessage',
        'onmousedown',
        'onmousemove',
        'onmouseup',
        'onmousewheel',
        'onoffline',
        'onoine',
        'ononline',
        'onpagehide',
        'onpageshow',
        'onpause',
        'onplay',
        'onplaying',
        'onpopstate',
        'onprogress',
        'onratechange',
        'onreadystatechange',
        'onredo',
        'onresize',
        'onscroll',
        'onseeked',
        'onseeking',
        'onselect',
        'onstalled',
        'onstorage',
        'onsuspend',
        'ontimeupdate',
        'onundo',
        'onunload',
        'onvolumechange',
        'onwaiting',
        'onmouseenter',
        'onmouseleave',
        'onstart',
        'onpropertychange',
        'oncopy',
        'ontoggle',
        'onpointerout',
        'onpointermove',
        'onpointerleave',
        'onpointerenter',
        'onpointerrawupdate',
        'onpointerover',
        'onbeforecopy',
        'onbeforecut',
        'onbeforeinput'
    ];

    function SanitizeHtmlHelper() {
    }
    SanitizeHtmlHelper.beforeSanitize = function () {
        return {
            selectors: {
                tags: removeTags,
                attributes: removeAttrs
            }
        };
    };
    SanitizeHtmlHelper.serializeValue = function (item, value) {
        this.removeAttrs = item.selectors.attributes;
        this.removeTags = item.selectors.tags;
        this.wrapElement = document.createElement('div');
        this.wrapElement.innerHTML = value;
        this.removeXssTags();
        this.removeJsEvents();
        this.removeXssAttrs();
        var tempEleValue = this.wrapElement.innerHTML;
        this.removeElement();
        this.wrapElement = null;
        return tempEleValue.replace(/&amp;/g, '&');
    };
    SanitizeHtmlHelper.removeElement = function () {
        // Removes an element's attibute to avoid html tag validation
        var nodes = this.wrapElement.children;
        for (var j = 0; j < nodes.length; j++) {
            var attribute = nodes[parseInt(j.toString(), 10)].attributes;
            for (var i = 0; i < attribute.length; i++) {
                this.wrapElement.children[parseInt(j.toString(), 10)].removeAttribute(attribute[parseInt(i.toString(), 10)].localName);
            }
        }
    };
    SanitizeHtmlHelper.removeXssTags = function () {
        var elements = this.wrapElement.querySelectorAll(this.removeTags.join(','));
        if (elements.length > 0) {
            elements.forEach(function (element) {
                detach(element);
            });
        }
        else {
            return;
        }
    };
    SanitizeHtmlHelper.removeJsEvents = function () {
        var elements = this.wrapElement.querySelectorAll('[' + jsEvents.join('],[') + ']');
        if (elements.length > 0) {
            elements.forEach(function (element) {
                jsEvents.forEach(function (attr) {
                    if (element.hasAttribute(attr)) {
                        element.removeAttribute(attr);
                    }
                });
            });
        }
        else {
            return;
        }
    };
    SanitizeHtmlHelper.removeXssAttrs = function () {
        var _this = this;
        // eslint-disable-next-line
        this.removeAttrs.forEach(function (item, index) {
            var elements = _this.wrapElement.querySelectorAll(item.selector);
            if (elements.length > 0) {
                elements.forEach(function (element) {
                    element.removeAttribute(item.attribute);
                });
            }
        });
    };
    return SanitizeHtmlHelper;
})();