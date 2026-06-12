'use strict';

var uid = 0;

const exports = {};

// Internal storage for component instances
const _instances = new Map();

const REGX_MOBILE = /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i;
const REGX_IE = /msie|trident/i;
const REGX_IOS = /(ipad|iphone|ipod touch)/i;

const REGX_BROWSER = {
    OPERA: /(opera|opr)(?:.*version|)[ /]([\w.]+)/i,
    EDGE: /(edge)(?:.*version|)[ /]([\w.]+)/i,
    CHROME: /(chrome|crios)[ /]([\w.]+)/i,
    PANTHOMEJS: /(phantomjs)[ /]([\w.]+)/i,
    SAFARI: /(safari)[ /]([\w.]+)/i,
    WEBKIT: /(webkit)[ /]([\w.]+)/i,
    MSIE: /(msie|trident) ([\w.]+)/i,
    MOZILLA: /(mozilla)(?:.*? rv:([\w.]+)|)/i
};

// Lightweight cache for browser details to minimize repeated UA parsing
window.browserDetails = window.browserDetails || {};

/**
 * To return the generated unique name
 *
 * @param {string} definedName - To concatenate the unique id to provided name
 * @returns {string}
 * @private
 */
function getUniqueID(definedName) {
    return definedName + '_' + uid++;
}

/**
 * Function to generate the unique id.
 */
function uniqueID() {
    if ((typeof window) === 'undefined') {
        return;
    }
    var num = new Uint16Array(5);
    var intCrypto = window.msCrypto || window.crypto;
    return intCrypto.getRandomValues(num);
}

/**
 * To get nameSpace value from the desired object.
 */
function getValue(nameSpace, obj) {
    if (!nameSpace || !obj || typeof nameSpace !== 'string') {
        return undefined;
    }
    var value = obj;
    var splits = nameSpace.replace(/\[/g, '.').replace(/\]/g, '').split('.');
    for (var i = 0; i < splits.length && !isUndefined(value); i++) {
        value = value[splits[parseInt(i.toString(), 10)]];
    }
    return value;
}

/**
 * Store a component instance by its dataId key.
 * @param {{dataId: string}} instance
 */
function setCompInstance(instance) {
    if (!instance || !instance.dataId) return;
    _instances.set(instance.dataId, instance);
}

/**
 * Retrieve a stored component instance.
 * @param {string} id
 * @returns {object|undefined}
 */
function getCompInstance(id) {
    return _instances.get(id);
}

/**
 * Remove and dispose a stored component instance.
 * @param {string} id
 */
function disposeWindowsInstance(id) {
    if (!id) return;
    _instances.delete(id);
}

/**
 * Null/undefined guard.
 * @param {*} value
 * @returns {boolean}
 */
const isNullOrUndefined = (value) => value === undefined || value === null;

/**
 * Return device flag object for compatibility.
 * @returns {{ IsDevice: boolean }}
 */
function isDevice() {
    return { IsDevice: Browser.isDevice };
}

/**
 * Determines whether a DOM element matches a given selector,
 * providing fallbacks for older browsers.
 * @param {Element} element
 * @param {string} selector
 * @returns {boolean}
 */
function matches(element, selector) {
    var matches = element.matches || element.msMatchesSelector || element.webkitMatchesSelector;
    return matches ? matches.call(element, selector) : [].indexOf.call(document.querySelectorAll(selector), element) !== -1;
}

/**
 * Closest ancestor with selector, with polyfill fallback.
 * @param {Element} element
 * @param {string} selector
 * @returns {Element|null}
 */
function closest(element, selector) {
    let el = element;
    if (!el) return null;
    if (typeof el.closest === 'function') {
        return el.closest(selector);
    }
    while (el && el.nodeType === 1) {
        if (matches(el, selector)) return el;
        el = el.parentNode;
    }
    return null;
}

/**
 * Create Instance from constructor function with desired parameters.
 */
function createInstance(classFunction, params) {
    var arrayParam = params;
    arrayParam.unshift(undefined);
    return new (Function.prototype.bind.apply(classFunction, arrayParam));
}

/**
 * Returns parent options for the object
 */
function getParentContext(context, prefix) {
    if (context.hasOwnProperty('parentObj') === false) {
        return { context: context, prefix: prefix };
    }
    var curText = getValue('propName', context);
    if (curText) {
        prefix = curText + '-' + prefix;
    }
    return getParentContext(getValue('parentObj', context), prefix);
}

/**
 * Browser utilities collection.
 */
class Browser {
    static getEvent(event) {
        const events = {
            start: { isPointer: 'pointerdown', isTouch: 'touchstart', isDevice: 'mousedown' },
            move: { isPointer: 'pointermove', isTouch: 'touchmove', isDevice: 'mousemove' },
            end: { isPointer: 'pointerup', isTouch: 'touchend', isDevice: 'mouseup' },
            cancel: { isPointer: 'pointercancel', isTouch: 'touchcancel', isDevice: 'mouseleave' }
        };
        if (Browser.isPointer) {
            return events[event].isPointer;
        }
        if (Browser.isTouch) {
            return events[event].isTouch + (!Browser.isDevice ? ' ' + events[event].isDevice : '');
        }
        return events[event].isDevice;
    }

    static getTouchStartEvent() { return Browser.getEvent('start'); }
    static getTouchEndEvent() { return Browser.getEvent('end'); }
    static getTouchMoveEvent() { return Browser.getEvent('move'); }
    static getTouchCancelEvent() { return Browser.getEvent('cancel'); };

    static extractBrowserDetail() {
        const browserInfo = { name: '', version: '' };
        const keys = Object.keys(REGX_BROWSER);
        for (const key of keys) {
            const clientInfo = Browser.userAgent.match(REGX_BROWSER[key]);
            if (clientInfo) {
                browserInfo.version = clientInfo[2];
                browserInfo.name = clientInfo[1]?.toLowerCase() === 'opr' ? 'opera' : clientInfo[1]?.toLowerCase() === 'crios' ? 'chrome' : clientInfo[1]?.toLowerCase();
                break;
            }
        }
        return browserInfo;
    }

    static getValue(key, regX) {
        const details = window.browserDetails;
        if (typeof details[key] === 'undefined') {
            details[key] = regX.test(Browser.userAgent);
        }
        return details[key];
    }

    static get info() {
        if (typeof window.browserDetails.info === 'undefined') {
            window.browserDetails.info = Browser.extractBrowserDetail();
        }
        return window.browserDetails.info;
    }

    static get isIE() { return Browser.getValue('isIE', REGX_IE); }
    static get isDevice() { return Browser.getValue('isDevice', REGX_MOBILE); }
    static get userAgent() { return typeof navigator !== 'undefined' ? navigator.userAgent : ''; }
    static get isIos() { return Browser.getValue('isIos', REGX_IOS); }

    static get touchStartEvent() {
        if (typeof window.browserDetails.touchStartEvent === 'undefined') {
            window.browserDetails.touchStartEvent = Browser.getTouchStartEvent();
        }
        return window.browserDetails.touchStartEvent;
    }
    static get touchMoveEvent() {
        if (typeof window.browserDetails.touchMoveEvent === 'undefined') {
            window.browserDetails.touchMoveEvent = Browser.getTouchMoveEvent();
        }
        return window.browserDetails.touchMoveEvent;
    }
    static get touchEndEvent() {
        if (typeof window.browserDetails.touchEndEvent === 'undefined') {
            window.browserDetails.touchEndEvent = Browser.getTouchEndEvent();
        }
        return window.browserDetails.touchEndEvent;
    }
    static get touchCancelEvent() {
        if (typeof window.browserDetails.touchCancelEvent === 'undefined') {
            window.browserDetails.touchCancelEvent = Browser.getTouchCancelEvent();
        }
        return window.browserDetails.touchCancelEvent;
    }
    static get isTouch() {
        if (typeof window.browserDetails.isTouch === 'undefined') {
            window.browserDetails.isTouch = ('ontouchstart' in window.navigator) || (window.navigator && window.navigator.maxTouchPoints > 0) || ('ontouchstart' in window);
        }
        return window.browserDetails.isTouch;
    }
    static get isPointer() {
        if (typeof window.browserDetails.isPointer === 'undefined') {
            window.browserDetails.isPointer = ('pointerEnabled' in window.navigator);
        }
        return window.browserDetails.isPointer;
    }
}

/**
 * EventHandler provides add/remove helpers.
*/
class EventHandler {
    /**
     * Get or create event list for an element.
     * @param {Element} element
     * @returns {Array<{name:string,listener:Function,bound:Function}>}
     */
    static addOrGetEventData(element) {
        if ('__eventList' in element) {
            return element.__eventList.events;
        }
        else {
            element.__eventList = {};
            return element.__eventList.events = [];
        }
    }

    /**
     * Add event listener(s) to element. Supports space-separated event names.
     * @param {Element} element
     * @param {string} eventName
     * @param {Function} listener
     * @param {object} [bindTo]
     */
    static add(element, eventName, listener, bindTo) {
        var eventData = EventHandler.addOrGetEventData(element);
        var boundListener;
        if (bindTo) {
            boundListener = listener.bind(bindTo);
        }
        else {
            boundListener = listener
        }
        var event = eventName.split(' ');
        for (var i = 0; i < event.length; i++) {
            eventData.push({
                name: event[i],
                listener: listener,
                bound: boundListener
            });
            if (Browser.isIE) {
                element.addEventListener(event[i], boundListener);
            }
            else {
                element.addEventListener(event[i], boundListener, { passive: false });
            }
        }
    }

    /**
     * Remove a previously added event listener.
     * @param {Element} element
     * @param {string} eventName
     * @param {Function} listener
     */
    static remove(element, eventName, listener) {
        const eventData = EventHandler.addOrGetEventData(element);
        if (!eventData || eventData.length === 0) return;
        const events = eventName.split(' ');
        for (const name of events) {
            // Find the index of the first matching listener
            const index = eventData.findIndex(x => x.name === name && x.listener === listener);
            if (index !== -1) {
                const { bound } = eventData[index];
                eventData.splice(index, 1);
                if (bound) {
                    element.removeEventListener(name, bound);
                }
            }
        }
    }

    /**
     * Clear all the event listeners that has been previously attached to the element.
     *
     * @param {any} element - Specifies the target html element to clear the events
     * @returns {void} ?
     */
    static clearEvents(element) {
        var eventData;
        var copyData;
        eventData = EventHandler.addOrGetEventData(element);
        copyData = extend([], copyData, eventData);
        for (var i = 0; i < copyData.length; i++) {
            var parseValue = copyData[parseInt(i.toString(), 10)];
            element.removeEventListener(parseValue.name, parseValue.debounce);
            eventData.shift();
        }
    }
}

/**
 * KeyboardEvents class enables you to bind key actions to desired key combinations
 * (e.g., Ctrl+A, Delete, Alt+Space).
 */
class KeyboardEvents {
    /**
     * Initializes the KeyboardEvents
     * @param {HTMLElement} element - Target element
     * @param {object} options - Configuration options { keyConfigs, keyAction, eventName }
     */
    constructor(element, options) {
        this.element = element;
        this.options = options || {};
        this.keyConfigs = this.options.keyConfigs || {};
        this.keyAction = this.options.keyAction;
        this.eventName = this.options.eventName || 'keyup'; // default to keyup
        this.keyPressHandler = this.keyPressHandler.bind(this);
        this.bind();
    }

    /**
     * Handles a key press event and maps to configured actions
     * @param {KeyboardEvent} e
     */
    keyPressHandler(e) {
        const isAltKey = e.altKey;
        const isCtrlKey = e.ctrlKey;
        const isShiftKey = e.shiftKey;
        const curkeyCode = e.which;
        const keys = Object.keys(this.keyConfigs);

        for (const key of keys) {
            const configCollection = this.keyConfigs[key].split(',');
            for (const rconfig of configCollection) {
                const rKeyObj = KeyboardEvents.getKeyConfigData(rconfig.trim());
                if (isAltKey === rKeyObj.altKey &&
                    isCtrlKey === rKeyObj.ctrlKey &&
                    isShiftKey === rKeyObj.shiftKey &&
                    curkeyCode === rKeyObj.keyCode) {
                    e.action = key;
                    if (this.keyAction) {
                        this.keyAction(e);
                    }
                }
            }
        }
    }

    /**
     * Wire event handlers
     */
    wireEvents() {
        this.element.addEventListener(this.eventName, this.keyPressHandler);
    }

    /**
     * Unwire event handlers
     */
    unwireEvents() {
        this.element.removeEventListener(this.eventName, this.keyPressHandler);
    }

    /**
     * Bind events
     */
    bind() {
        this.wireEvents();
    }

    /**
     * Destroy the instance
     */
    destroy() {
        this.unwireEvents();
    }

    /**
     * Module name
     */
    getModuleName() {
        return 'keyboard';
    }

    /**
     * To get the key configuration data
     * @param {string} config - configuration string (e.g., "ctrl+a")
     * @returns {object} KeyData
     */
    static getKeyConfigData(config) {
        if (config in this.configCache) {
            return this.configCache[config];
        }
        const keys = config.toLowerCase().split('+');
        const keyData = {
            altKey: keys.indexOf('alt') !== -1,
            ctrlKey: keys.indexOf('ctrl') !== -1,
            shiftKey: keys.indexOf('shift') !== -1,
            keyCode: null
        };
        if (keys[keys.length - 1].length > 1 && !!Number(keys[keys.length - 1])) {
            keyData.keyCode = Number(keys[keys.length - 1]);
        } else {
            keyData.keyCode = KeyboardEvents.getKeyCode(keys[keys.length - 1]);
        }
        this.configCache[config] = keyData;
        return keyData;
    }

    /**
     * Return the keycode value
     * @param {string} keyVal
     * @returns {number}
     */
    static getKeyCode(keyVal) {
        return this.keyCodeMap[keyVal] || keyVal.toUpperCase().charCodeAt(0);
    }
}

// Static cache for configs
KeyboardEvents.configCache = {};

// Basic keyCode map (extend as needed)
KeyboardEvents.keyCodeMap = {
    enter: 13,
    escape: 27,
    tab: 9,
    home: 36,
    end: 35,
    left: 37,
    right: 39,
    up: 38,
    down: 40,
    delete: 46,
    backspace: 8,
    space: 32
};

/**
 * Specifies the CLDR data loaded for internationalization functionalities.
 */
var cldrData = {};

/**
 * Specifies the default culture value to be considered.
 */
exports.defaultCulture = 'en-US';

/**
 * Specifies default currency code to be considered
 */
exports.defaultCurrencyCode = 'USD';

/**
 * Internationalization class provides support to parse and format the number and date object to the desired format.
 */
class Internationalization {

    constructor(cultureName) {
        if (cultureName) {
            this.culture = cultureName;
        }
    }

    /**
     * Returns the format function for given options.
     *
     * @param {DateFormatOptions} options - Specifies the format options in which the format function will return.
     * @returns {Function} ?
     */
    getDateFormat(options) {
        return DateFormat.dateFormat(this.getCulture(), options || { type: 'date', skeleton: 'short' }, cldrData);
    }

    /**
     * Returns the format function for given options.
     *
     * @param {NumberFormatOptions} options - Specifies the format options in which the format function will return.
     * @returns {Function} ?
     */
    getNumberFormat(options) {
        if (options && !options.currency) {
            options.currency = exports.defaultCurrencyCode;
        }
        if (options && !options.format) {
            options.minimumFractionDigits = 0;
        }
        return NumberFormat.numberFormatter(this.getCulture(), options || {}, cldrData);
    }

    /**
     * Returns the culture
     *
     * @returns {string} ?
     */
    getCulture() {
        return this.culture || exports.defaultCulture;
    }
}

const ABBREVIATE_REGEX_GLOBAL = /\/MMMMM|MMMM|MMM|a|LLLL|LLL|EEEEE|EEEE|E|K|cccc|ccc|WW|W|G+|z+/gi;
const WEEKDAY_KEY = ['sun', 'mon', 'tue', 'wed', 'thu', 'fri', 'sat'];

const TIME_SETTER = {
    m: 'getMinutes',
    h: 'getHours',
    H: 'getHours',
    s: 'getSeconds',
    d: 'getDate',
    f: 'getMilliseconds'
};

const TIME_SEPARATOR = 'timeSeparator';

/**
 * Date Format is a framework that provides support for date formatting.
 * @private
 */
class DateFormat {
    constructor() {
    }

    /**
     * Returns the formatter function for given skeleton.
     *
     * @param {string} culture -  Specifies the culture name to be which formatting.
     * @param {DateFormatOptions} option - Specific the format in which date  will format.
     * @param {Object} cldr - Specifies the global cldr data collection.
     * @returns {Function} ?
     */
    static dateFormat(culture, option, cldr) {
        const dependable = exports.IntlBase.getDependables(cldr, culture, option.calendar);
        const numObject = getValue('parserObject.numbers', dependable);
        const dateObject = dependable.dateObject;
        const formatOptions = { isIslamic: exports.IntlBase.islamicRegex.test(option.calendar) };
        if (option.isServerRendered) {
            option = exports.IntlBase.compareBlazorDateFormats(option, culture);
        }
        let resPattern = option.format ||
            exports.IntlBase.getResultantPattern(option.skeleton, dependable.dateObject, option.type, false, culture);
        formatOptions.dateSeperator = getValue('dateSeperator', dateObject);
        if (isUndefined(resPattern)) {
            throwError('Format options or type given must be invalid');
        }
        else {
            resPattern = exports.IntlBase.ConvertDateToWeekFormat(resPattern);
            resPattern = resPattern.replace(/tt/, 'a');
            formatOptions.pattern = resPattern;
            formatOptions.numMapper = extend({}, numObject);
            const patternMatch = resPattern.match(ABBREVIATE_REGEX_GLOBAL) || [];
            for (let _i = 0, patternMatch_1 = patternMatch; _i < patternMatch_1.length; _i++) {
                const str = patternMatch_1[_i];
                const len = str.length;
                const char = str[0];
                if (char === 'K') {
                    char = 'h';
                }
                switch (char) {
                    case 'E':
                    case 'c':
                        formatOptions.weekday = getValue('days.' + exports.IntlBase.monthIndex[len], dateObject);
                        break;
                    case 'M':
                    case 'L':
                        formatOptions.month = getValue('months.' + exports.IntlBase.monthIndex[len], dateObject);
                        break;
                    case 'a':
                        formatOptions.designator = getValue('dayPeriods', dateObject);
                        break;
                    case 'G':
                        const eText = (len <= 3) ? 'eraAbbr' : (len === 4) ? 'eraNames' : 'eraNarrow';
                        formatOptions.era = getValue('eras', dateObject);
                        break;
                    case 'z':
                        formatOptions.timeZone = getValue('dates.timeZoneNames', dependable.parserObject);
                        break;
                }
            }
        }
        return function (value) {
            if (isNaN(value.getDate())) {
                return null;
            }
            return DateFormat.intDateFormatter(value, formatOptions);
        };
    }

    /**
     * Returns formatted date string based on options passed.
     *
     * @param {Date} value ?
     * @param {FormatOptions} options ?
     * @returns {string} ?
     */
    static intDateFormatter(value, options) {
        const pattern = options.pattern;
        let ret = '';
        const matches = pattern.match(exports.IntlBase.dateParseRegex);
        const dObject = this.getCurrentDateValue(value, options.isIslamic);
        for (let _i = 0, matches_1 = matches; _i < matches_1.length; _i++) {
            const match = matches_1[_i];
            const length_1 = match.length;
            let char = match[0];
            if (char === 'K') {
                char = 'h';
            }
            let curval = void 0;
            let curvalstr = '';
            let isNumber = void 0;
            let processNumber = void 0;
            let curstr = '';
            switch (char) {
                case 'M':
                case 'L':
                    curval = dObject.month;
                    if (length_1 > 2) {
                        ret += options.month[curval];
                    }
                    else {
                        isNumber = true;
                    }
                    break;
                case 'E':
                case 'c':
                    ret += options.weekday[WEEKDAY_KEY[value.getDay()]];
                    break;
                case 'H':
                case 'h':
                case 'm':
                case 's':
                case 'd':
                case 'f':
                    isNumber = true;
                    if (char === 'd') {
                        curval = dObject.date;
                    }
                    else if (char === 'f') {
                        isNumber = false;
                        processNumber = true;
                        curvalstr = value[TIME_SETTER[char]]().toString();
                        curvalstr = curvalstr.substring(0, length_1);
                        const curlength = curvalstr.length;
                        if (length_1 !== curlength) {
                            if (length_1 > 3) {
                                continue;
                            }
                            for (const i = 0; i < length_1 - curlength; i++) {
                                curvalstr = '0' + curvalstr.toString();
                            }
                        }
                        curstr += curvalstr;
                    }
                    else {
                        curval = value[TIME_SETTER[char]]();
                    }
                    if (char === 'h') {
                        curval = curval % 12 || 12;
                    }
                    break;
                case 'y':
                    processNumber = true;
                    curstr += dObject.year;
                    if (length_1 === 2) {
                        curstr = curstr.substring(curstr.length - 2);
                    }
                    break;
                case 'a':
                    const desig = value.getHours() < 12 ? 'am' : 'pm';
                    ret += options.designator[desig];
                    break;
                case 'G':
                    const dec = value.getFullYear() < 0 ? 0 : 1;
                    let retu = options.era[dec];
                    if (isNullOrUndefined(retu)) {
                        retu = options.era[dec ? 0 : 1];
                    }
                    ret += retu || '';
                    break;
                case '\'':
                    ret += (match === '\'\'') ? '\'' : match.replace(/'/g, '');
                    break;
                case 'z':
                    const timezone = value.getTimezoneOffset();
                    let pattern_1 = (length_1 < 4) ? '+H;-H' : options.timeZone.hourFormat;
                    pattern_1 = pattern_1.replace(/:/g, options.numMapper.timeSeparator);
                    if (timezone === 0) {
                        ret += options.timeZone.gmtZeroFormat;
                    }
                    else {
                        processNumber = true;
                        curstr = this.getTimeZoneValue(timezone, pattern_1);
                    }
                    curstr = options.timeZone.gmtFormat.replace(/\{0\}/, curstr);
                    break;
                case ':':
                    ret += options.numMapper.numberSymbols[TIME_SEPARATOR];
                    break;
                case '/':
                    ret += options.dateSeperator;
                    break;
                case 'W':
                    isNumber = true;
                    curval = exports.IntlBase.getWeekOfYear(value);
                    break;
                default:
                    ret += match;
            }
            if (isNumber) {
                processNumber = true;
                curstr = this.checkTwodigitNumber(curval, length_1);
            }
            if (processNumber) {
                ret += ParserBase.convertValueParts(curstr, exports.IntlBase.latnParseRegex, options.numMapper.mapper);
            }
        }
        return ret;
    }

    static getCurrentDateValue(value, isIslamic) {
        if (isIslamic) {
            return exports.HijriParser.getHijriDate(value);
        }
        return { year: value.getFullYear(), month: value.getMonth() + 1, date: value.getDate() };
    }

    /**
     * Returns two digit numbers for given value and length
     *
     * @param {number} val ?
     * @param {number} len ?
     * @returns {string} ?
     */
    static checkTwodigitNumber(val, len) {
        const ret = val + '';
        if (len === 2 && ret.length !== 2) {
            return '0' + ret;
        }
        return ret;
    }

    /**
     * Returns the value of the Time Zone.
     *
     * @param {number} tVal ?
     * @param {string} pattern ?
     * @returns {string} ?
     * @private
     */
    static getTimeZoneValue(tVal, pattern) {
        const splt = pattern.split(';');
        const curPattern = splt[tVal > 0 ? 1 : 0];
        const no = Math.abs(tVal);
        return curPattern.replace(/HH?|mm/g, function (str) {
            const len = str.length;
            const ishour = str.indexOf('H') !== -1;
            return this.checkTwodigitNumber(Math.floor(ishour ? (no / 60) : (no % 60)), len);
        });
    }
}

const ERROR_TEXT = {
    'ms': 'minimumSignificantDigits',
    'ls': 'maximumSignificantDigits',
    'mf': 'minimumFractionDigits',
    'lf': 'maximumFractionDigits'
};

const MAPPER_$1 = ['infinity', 'nan', 'group', 'decimal', 'exponential'];

/**
 * Module for number formatting.
 * @private
 */
class NumberFormat {
    constructor() {
    }

    /**
     * Returns the formatter function for given skeleton.
     *
     * @param {string} culture -  Specifies the culture name to be which formatting.
     * @param {NumberFormatOptions} option - Specific the format in which number  will format.
     * @param {Object} cldr - Specifies the global cldr data collection.
     * @returns {Function} ?
     */
    static numberFormatter(culture, option, cldr) {
        const fOptions = extend({}, option);
        const cOptions = {};
        const dOptions = {};

        const dependable = exports.IntlBase.getDependables(cldr, culture, '', true);
        const numObject = dependable.numericObject;

        dOptions.numberMapper = extend({}, numObject);
        dOptions.currencySymbol = getValue('currencySymbol', numObject);
        dOptions.percentSymbol = getValue('numberSymbols.percentSign', numObject);
        dOptions.minusSymbol = getValue('numberSymbols.minusSign', numObject);

        const symbols = dOptions.numberMapper.numberSymbols;

        if ((option.format) && !(exports.IntlBase.formatRegex.test(option.format))) {
            cOptions = exports.IntlBase.customFormat(option.format, dOptions, dependable.numericObject);
        }
        else {
            extend(fOptions, exports.IntlBase.getProperNumericSkeleton(option.format || 'N'));
            fOptions.isCurrency = fOptions.type === 'currency';
            fOptions.isPercent = fOptions.type === 'percent';
            fOptions.groupOne = this.checkValueRange(fOptions.maximumSignificantDigits, fOptions.minimumSignificantDigits, true);
            this.checkValueRange(fOptions.maximumFractionDigits, fOptions.minimumFractionDigits, false, true);

            if (!isUndefined(fOptions.fractionDigits)) {
                fOptions.minimumFractionDigits = fOptions.maximumFractionDigits = fOptions.fractionDigits;
            }
            if (isUndefined(fOptions.useGrouping)) {
                fOptions.useGrouping = true;
            }

            cOptions.nData = extend({}, {}, getValue(fOptions.type + 'nData', numObject));
            cOptions.pData = extend({}, {}, getValue(fOptions.type + 'pData', numObject));
            if (fOptions.type === 'currency' && option.currency) {
                exports.IntlBase.replaceBlazorCurrency([cOptions.pData, cOptions.nData], dOptions.currencySymbol, option.currency);
            }
            const minFrac = isUndefined(fOptions.minimumFractionDigits);
            if (minFrac) {
                fOptions.minimumFractionDigits = cOptions.nData.minimumFraction;
            }
            if (isUndefined(fOptions.maximumFractionDigits)) {
                const mval = cOptions.nData.maximumFraction;
                fOptions.maximumFractionDigits = isUndefined(mval) && fOptions.isPercent ? 0 : mval;
            }
            const mfrac = fOptions.minimumFractionDigits;
            const lfrac = fOptions.maximumFractionDigits;

            if (!isUndefined(mfrac) && !isUndefined(lfrac)) {
                if (mfrac > lfrac) {
                    fOptions.maximumFractionDigits = mfrac;
                }
            }
        }
        extend(cOptions.nData, fOptions);
        extend(cOptions.pData, fOptions);
        return function (value) {
            if (isNaN(value)) {
                return symbols[MAPPER_$1[1]];
            }
            else if (!isFinite(value)) {
                return symbols[MAPPER_$1[0]];
            }
            return NumberFormat.intNumberFormatter(value, cOptions, dOptions, option);
        };
    }

    /**
     * Returns grouping details for the pattern provided
     *
     * @param {string} pattern ?
     * @returns {GroupDetails} ?
     */
    static getGroupingDetails(pattern) {
        const ret = {};
        const match = pattern.match(exports.IntlBase.negativeDataRegex);
        if (match && match[4]) {
            const pattern_1 = match[4];
            const p = pattern_1.lastIndexOf(',');
            if (p !== -1) {
                const temp = pattern_1.split('.')[0];
                ret.primary = (temp.length - p) - 1;
                const s = pattern_1.lastIndexOf(',', p - 1);
                if (s !== -1) {
                    ret.secondary = p - 1 - s;
                }
            }
        }
        return ret;
    }

    /**
     * Returns if the provided integer range is valid.
     *
     * @param {number} val1 ?
     * @param {number} val2 ?
     * @param {boolean} checkbothExist ?
     * @param {boolean} isFraction ?
     * @returns {boolean} ?
     */
    static checkValueRange(val1, val2, checkbothExist, isFraction) {
        const decide = isFraction ? 'f' : 's';
        let dint = 0;
        const str1 = ERROR_TEXT['l' + decide];
        const str2 = ERROR_TEXT['m' + decide];

        if (!isUndefined(val1)) {
            this.checkRange(val1, str1, isFraction);
            dint++;
        }
        if (!isUndefined(val2)) {
            this.checkRange(val2, str2, isFraction);
            dint++;
        }
        if (dint === 2) {
            if (val1 < val2) {
                throwError(str2 + 'specified must be less than the' + str1);
            }
            else {
                return true;
            }
        }
        else if (checkbothExist && dint === 1) {
            throwError('Both' + str2 + 'and' + str2 + 'must be present');
        }
        return false;
    }

    /**
     * Check if the provided fraction range is valid
     *
     * @param {number} val ?
     * @param {string} text ?
     * @param {boolean} isFraction ?
     * @returns {void} ?
     */
    static checkRange(val, text, isFraction) {
        const range = isFraction ? [0, 20] : [1, 21];
        if (val < range[0] || val > range[1]) {
            throwError(text + 'value must be within the range' + range[0] + 'to' + range[1]);
        }
    }

    /**
     * Returns formatted numeric string for provided formatting options
     * @private
     */
    static intNumberFormatter(value, fOptions, dOptions, option) {
        let curData;
        if (isUndefined(fOptions.nData.type)) {
            return undefined;
        }
        else {
            if (value < 0) {
                value = value * -1;
                curData = fOptions.nData;
            }
            else if (value === 0) {
                curData = fOptions.zeroData || fOptions.pData;
            }
            else {
                curData = fOptions.pData;
            }
            let fValue = '';
            if (curData.isPercent) {
                value = value * 100;
            }
            if (curData.groupOne) {
                fValue = this.processSignificantDigits(value, curData.minimumSignificantDigits, curData.maximumSignificantDigits);
            }
            else {
                fValue = this.processFraction(value, curData.minimumFractionDigits, curData.maximumFractionDigits, option);
                if (curData.minimumIntegerDigits) {
                    fValue = this.processMinimumIntegers(fValue, curData.minimumIntegerDigits);
                }
                if (dOptions.isCustomFormat && curData.minimumFractionDigits < curData.maximumFractionDigits
                    && /\d+\.\d+/.test(fValue)) {
                    const temp = fValue.split('.');
                    let decimalPart = temp[1];
                    const len = decimalPart.length;
                    for (let i = len - 1; i >= 0; i--) {
                        if (decimalPart["" + i] === '0' && i >= curData.minimumFractionDigits) {
                            decimalPart = decimalPart.slice(0, i);
                        }
                        else {
                            break;
                        }
                    }
                    fValue = temp[0] + '.' + decimalPart;
                }
            }
            if (curData.type === 'scientific') {
                fValue = value.toExponential(curData.maximumFractionDigits);
                fValue = fValue.replace('e', dOptions.numberMapper.numberSymbols[MAPPER_$1[4]]);
            }
            fValue = fValue.replace('.', dOptions.numberMapper.numberSymbols[MAPPER_$1[3]]);
            fValue = curData.format === '#,###,,;(#,###,,)' ? this.customPivotFormat(parseInt(fValue, 10)) : fValue;
            if (curData.useGrouping) {
                
                fValue = this.groupNumbers(fValue, curData.groupData.primary, curData.groupSeparator || ',', dOptions.numberMapper.numberSymbols[MAPPER_$1[3]] || '.', curData.groupData.secondary);
            }
            fValue = ParserBase.convertValueParts(fValue, exports.IntlBase.latnParseRegex, dOptions.numberMapper.mapper);
            if (curData.nlead === 'N/A') {
                return curData.nlead;
            }
            else {
                if (fValue === '0' && option && option.format === '0') {
                    return fValue + curData.nend;
                }
                return curData.nlead + fValue + curData.nend;
            }
        }
    }

    /**
     * Returns significant digits processed numeric string
     *
     * @param {number} value ?
     * @param {number} min ?
     * @param {number} max ?
     * @returns {string} ?
     */
    static processSignificantDigits(value, min, max) {
        let temp = value + '';
        let tn;
        const length = temp.length;
        if (length < min) {
            return value.toPrecision(min);
        }
        else {
            temp = value.toPrecision(max);
            tn = +temp;
            return tn + '';
        }
    }

    /**
     * Returns grouped numeric string
     *
     * @param {string} val ?
     * @param {number} level1 ?
     * @param {string} sep ?
     * @param {string} decimalSymbol ?
     * @param {number} level2 ?
     * @returns {string} ?
     */
    static groupNumbers(val, level1, sep, decimalSymbol, level2) {
        const flag = !isNullOrUndefined(level2) && level2 !== 0;
        const split = val.split(decimalSymbol);
        let prefix = split[0];
        let length = prefix.length;
        let str = '';

        while (length > level1) {
            str = prefix.slice(length - level1, length) + (str.length ?
                (sep + str) : '');
            length -= level1;
            if (flag) {
                level1 = level2;
                flag = false;
            }
        }
        split[0] = prefix.slice(0, length) + (str.length ? sep : '') + str;
        return split.join(decimalSymbol);
    }

    /**
     * Returns fraction processed numeric string
     *
     * @param {number} value ?
     * @param {number} min ?
     * @param {number} max ?
     * @param {NumberFormatOptions} [option] ?
     * @returns {string} ?
     */
    static processFraction(value, min, max, option) {
        const temp = (value + '').split('.')[1];
        const length = temp ? temp.length : 0;
        if (min && length < min) {
            let ret = '';
            if (length === 0) {
                ret = value.toFixed(min);
            }
            else {
                ret += value;
                for (let j = 0; j < min - length; j++) {
                    ret += '0';
                }
                return ret;
            }
            return value.toFixed(min);
        }
        else if (!isNullOrUndefined(max) && (length > max || max === 0)) {
            return value.toFixed(max);
        }
        let str = value + '';
        if (str[0] === '0' && option && option.format === '###.00') {
            str = str.slice(1);
        }
        return str;
    }

    /**
     * Returns integer processed numeric string
     *
     * @param {string} value ?
     * @param {number} min ?
     * @returns {string} ?
     */
    static processMinimumIntegers(value, min) {
        let temp = value.split('.');
        let lead = temp[0];
        const len = lead.length;
        if (len < min) {
            for (let i = 0; i < min - len; i++) {
                lead = '0' + lead;
            }
            temp[0] = lead;
        }
        return temp.join('.');
    }

    /**
     * Returns custom format for pivot table
     *
     * @param {number} value ?
     * @returns {string} ?
     */
    static customPivotFormat(value) {
        if (value >= 500000) {
            value /= 1000000;
            const _a = value.toString().split('.'), integer = _a[0], decimal = _a[1];
            return decimal && +decimal.substring(0, 1) >= 5 ? Math.ceil(value).toString() : Math.floor(value).toString();
        }
        return '';
    }
}

/**
 * Creates an HTML element with optional properties.
 *
 * @param {string} tagName - The HTML tag name.
 * @param {Object} properties - Optional properties (className, id, styles).
 * @returns {Element} The created HTML element.
 */
function createElement(tagName, properties) {
    let element = document.createElement(tagName);
    if (typeof (properties) === 'undefined') {
        return element;
    }
    if (properties.className !== undefined) {
        element.className = properties.className;
    }
    if (properties.id !== undefined) {
        element.id = properties.id;
    }
    if (properties.styles !== undefined) {
        element.style.cssText = properties.styles;
    }
    return element;
}

/**
 * Adds one or more classes to an element or collection of elements.
 *
 * @param {Element|Element[]|NodeList} elements - The element(s) to add classes to.
 * @param {string|string[]} classes - Class name(s) to add (space-separated string or array).
 * @returns {Element|Element[]|NodeList} The original elements input.
 */
function addClass(elements, classes) {
    if (!elements || !classes) {
        return elements;
    }
    const cls = (Array.isArray(classes) ? classes : classes.split(' '))
        .map(c => c.trim()).filter(Boolean);
    // Detect if single element or collection
    const isSingleElement = elements.nodeType === 1;
    const elemArray = isSingleElement ? [elements] : Array.from(elements);
    elemArray.forEach(ele => {
        // Check if element has classList support and className property exists
        if (ele && ele.classList && (ele.className !== undefined)) {
            cls.forEach(c => !ele.classList.contains(c) && ele.classList.add(c));
        }
    });
	// Return original type: single element or collection
    return elements;
}

/**
    * The function used to add the classes to array of elements
    *
    * @param  {Element[]|NodeList} elements - An array of elements that need to remove a list of classes
    * @param  {string|string[]} classes - String or array of string that need to add an individual element as a class
    * @returns {any} .
    * @private
    */
function removeClass(elements, classes) {
    if (!elements || !classes) {
        return elements;
    }
    const cls = (Array.isArray(classes) ? classes : classes.split(' '))
        .map(c => c.trim()).filter(Boolean);
    // Detect if single element or collection
    const isSingleElement = elements.nodeType === 1;
    const elemArray = isSingleElement ? [elements] : Array.from(elements);
    elemArray.forEach(ele => {
        // Check if element has classList support and className property exists
        if (ele && ele.classList && (ele.className !== undefined)) {
            cls.forEach(c => ele.classList.remove(c));
        }
    });
    // Return original type: single element or collection
    return elements;
}

/**
    * Sets style attributes on an HTML element.
    *
    * @param {HTMLElement} element - The element to apply styles to.
    * @param {Object} attrs - Object containing style properties and values.
    */
function setStyleAttribute(element, attrs) {
    if (attrs !== undefined) {
        Object.keys(attrs).forEach(function (key) {
            if (attrs[key] != null) {
                element.style[key] = attrs[key];
            }
        });
    }
}

/**
    * To run a callback function immediately after the browser has completed other operations.
    *
    * @param {Function} handler - callback function to be triggered.
    * @returns {Function} ?
    * @private
    */
function setImmediate(handler) {
    var unbind;
    var num = new Uint16Array(5);
    var intCrypto = window.msCrypto || window.crypto;
    intCrypto.getRandomValues(num);
    var secret = 'ej2' + combineArray(num);
    var messageHandler = function (event) {
        if (event.source === window && typeof event.data === 'string' && event.data.length <= 32 && event.data === secret) {
            handler();
            unbind();
        }
    };
    window.addEventListener('message', messageHandler, false);
    window.postMessage(secret, '*');
    return unbind = function () {
        window.removeEventListener('message', messageHandler);
        handler = messageHandler = secret = undefined;
    };
}

/**
    *
    * @param {Int16Array} num ?
    * @returns {string} ?
    */
function combineArray(num) {
    var ret = '';
    for (var i = 0; i < 5; i++) {
        ret += (i ? ',' : '') + num[parseInt(i.toString(), 10)];
    }
    return ret;
}

/**
    * The function used to get classlist.
    *
    * @param  {string | string[]} classes - An element the need to check visibility
    * @returns {string[]} ?
    * @private
    */
function getClassList(classes) {
    var classList = [];

    if (typeof classes === 'string') {
        if (classes && classes.trim().length > 0) {
            classList.push(classes.trim());
        }
    }
    else if (Array.isArray(classes)) {
        for (var i = 0; i < classes.length; i++) {
            var cls = classes[i];
            if (typeof cls === 'string' && cls.trim().length > 0) {
                classList.push(cls.trim());
            }
        }
    }

    return classList;
}

/**
    * Check weather the given argument is only object.
    *
    * @param {any} obj - Object which is need to check.
    * @returns {boolean} ?
    * @private
    */
function isObject(obj) {
    var objCon = {};
    return (!isNullOrUndefined(obj) && obj.constructor === objCon.constructor);
}

/**
 * Function to normalize the units applied to the element.
 *
 * @param {number|string} value ?
 * @returns {string} result
 * @private
 */
function formatUnit(value) {
    var result = value + '';
    if (result.match(/auto|cm|mm|in|px|pt|pc|%|em|ex|ch|rem|vw|vh|vmin|vmax/)) {
        return result;
    }
    return result + 'px';
}

/**
 * Removes an element from its parent node.
 *
 * @param {Element|Node|HTMLElement} element - The element to remove from the DOM.
 */
function detach(element) {
    if (element || element.parentNode) {
        element.parentNode.removeChild(element);
    }
}

/**
    * The function used to check element is visible or not.
    *
    * @param  {Element|Node} element - An element the need to check visibility
    * @returns {boolean} ?
    * @private
    */
function isVisible(element) {
    var ele = element;
    return (ele.style.visibility === '' && ele.offsetWidth > 0);
}
/**
    * Method used to create property. General syntax below.
    *
    * @param {Object} defaultValue - Specifies the default value of property.
    * @returns {PropertyDecorator} ?
    * ```
    * @Property('TypeScript')
    * propertyName: Type;
    * ```
    * @private
    */
function Property(defaultValue) {
    return function (target, key) {
        var propertyDescriptor = {
            set: propertySetter(defaultValue, key),
            get: propertyGetter(defaultValue, key),
            enumerable: true,
            configurable: true
        };
        //new property creation
        Object.defineProperty(target, key, propertyDescriptor);
        addPropertyCollection(target, key, 'prop', defaultValue);
    };
}

/**
    * Returns the properties of the object
    *
    * @param {Object} defaultValue ?
    * @param {string} curKey ?
    * @returns {void} ?
    */
function propertyGetter(defaultValue, curKey) {
    return function () {
        if (!this.properties.hasOwnProperty(curKey)) {
            this.properties["" + curKey] = defaultValue;
        }
        return this.properties["" + curKey];
    };
}
/**
    * Set the properties for the object
    *
    * @param {Object} defaultValue ?
    * @param {string} curKey ?
    * @returns {void} ?
    */
function propertySetter(defaultValue, curKey) {
    return function (newValue) {
        if (this.properties["" + curKey] !== newValue) {
            var oldVal = this.properties.hasOwnProperty(curKey) ? this.properties[curKey] : defaultValue;
            this.saveChanges(curKey, newValue, oldVal);
            this.properties["" + curKey] = newValue;
        }
    };
}

/**
    * Method  used to create the builderObject for the target component.
    *
    * @param {BuildInfo} target ?
    * @param {string} key ?
    * @param {string} propertyType ?
    * @param {Object} defaultValue ?
    * @param {Function} type ?
    * @returns {void} ?
    * @private
    */
function addPropertyCollection(target, key, propertyType, defaultValue, type) {
    if (isUndefined(target.propList)) {
        target.propList = {
            props: [],
            complexProps: [],
            colProps: [],
            events: [],
            propNames: [],
            complexPropNames: [],
            colPropNames: [],
            eventNames: []
        };
    }
    target.propList[propertyType + 's'].push({
        propertyName: key,
        defaultValue: defaultValue,
        type: type
    });
    target.propList[propertyType + 'Names'].push(key);
}

/**
    * To check whether the object is undefined.
    *
    * @param {Object} value - To check the object is undefined
    * @returns {boolean} ?
    * @private
    */
function isUndefined(value) {
    return ('undefined' === typeof value);
}

/**
    * Method used to create complex property. General syntax below.
    *
    * @param  {any} defaultValue - Specifies the default value of property.
    * @param  {Function} type - Specifies the class type of complex object.
    * @returns {PropertyDecorator} ?
    * ```
    * @Complex<Type>({},Type)
    * propertyName: Type;
    * ```
    * @private
    */
function Complex(defaultValue, type) {
    return function (target, key) {
        var propertyDescriptor = {
            set: complexSetter(defaultValue, key, type),
            get: complexGetter(defaultValue, key, type),
            enumerable: true,
            configurable: true
        };
        //new property creation
        Object.defineProperty(target, key, propertyDescriptor);
        addPropertyCollection(target, key, 'complexProp', defaultValue, type);
    };
}

/**
    * Returns complex objects
    *
    * @param {Object} defaultValue ?
    * @param {string} curKey ?
    * @param {Object[]} type ?
    * @returns {void} ?
    */
function complexGetter(defaultValue, curKey, type) {
    return function () {
        return getObject(this, curKey, defaultValue, type);
    };
}
/**
    * Sets complex objects
    *
    * @param {Object} defaultValue ?
    * @param {string} curKey ?
    * @param {Object[]} type ?
    * @returns {void} ?
    */
function complexSetter(defaultValue, curKey, type) {
    return function (newValue) {
        getObject(this, curKey, defaultValue, type).setProperties(newValue);
    };
}

/**
    * Method used to create event property. General syntax below.
    *
    * @returns {PropertyDecorator} ?
    * ```
    * @Event(()=>{return true;})
    * ```
    * @private
    */
function Event1() {
    return function (target, key) {
        var eventDescriptor = {
            set: function (newValue) {
                var oldValue = this.properties["" + key];
                if (oldValue !== newValue) {
                    var finalContext = getParentContext(this, key);
                    if (isUndefined(oldValue) === false) {
                        finalContext.context.removeEventListener(finalContext.prefix, oldValue);
                    }
                    finalContext.context.addEventListener(finalContext.prefix, newValue);
                    this.properties["" + key] = newValue;
                }
            },
            get: propertyGetter(undefined, key),
            enumerable: true,
            configurable: true
        };
        Object.defineProperty(target, key, eventDescriptor);
        addPropertyCollection(target, key, 'event');
    };
}

/**
    * NotifyPropertyChanges is triggers the call back when the property has been changed.
    *
    * @param {Function} classConstructor ?
    * @returns {void} ?
    * ```
    *  @NotifyPropertyChanges
    * class DemoClass implements INotifyPropertyChanged {
    *
    *     @Property()
    *     property1: string;
    *
    *     dataBind: () => void;
    *
    *     constructor() { }
    *
    *     onPropertyChanged(newProp: any, oldProp: any) {
    *         // Called when property changed
    *     }
    * }
    * ```
    * @private
    */
function NotifyPropertyChanges(classConstructor) {
    /** Need to code */
}

/**
    * Merge the source object into destination object.
    *
    * @param {any} source - source object which is going to merge with destination object
    * @param {any} destination - object need to be merged
    * @returns {void} ?
    * @private
    */
function merge(source, destination) {
    if (!isNullOrUndefined(destination)) {
        var temrObj = source;
        var tempProp = destination;
        var keys = Object.keys(destination);
        var deepmerge = 'deepMerge';
        for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
            var key = keys_1[_i];
            if (!isNullOrUndefined(temrObj["" + deepmerge]) && (temrObj["" + deepmerge].indexOf(key) !== -1) &&
                (isObject(tempProp["" + key]) || Array.isArray(tempProp["" + key]))) {
                extend(temrObj["" + key], temrObj["" + key], tempProp["" + key], true);
            }
            else {
                temrObj["" + key] = tempProp["" + key];
            }
        }
    }
}

/**
    * Extend the two object with newer one.
    *
    * @param {any} copied - Resultant object after merged
    * @param {Object} first - First object need to merge
    * @param {Object} second - Second object need to merge
    * @param {boolean} deep ?
    * @returns {Object} ?
    * @private
    */
function extend(copied, first, second, deep) {
    var result = copied && typeof copied === 'object' ? copied : {};
    var length = arguments.length;
    if (deep) {
        length = length - 1;
    }
    var _loop_1 = function (i) {
        if (!arguments_1[i]) {
            return "continue";
        }
        var obj1 = arguments_1[i];
        Object.keys(obj1).forEach(function (key) {
            var src = result["" + key];
            var copy = obj1["" + key];
            var clone;
            var isArrayChanged = Array.isArray(copy) && Array.isArray(src) && (copy.length !== src.length);
            var blazorEventExtend = (!(src instanceof Event) && !isArrayChanged);
            if (deep && blazorEventExtend && (isObject(copy) || Array.isArray(copy))) {
                if (isObject(copy)) {
                    clone = src ? src : {};
                    if (Array.isArray(clone) && clone.hasOwnProperty('isComplexArray')) {
                        extend(clone, {}, copy, deep);
                    }
                    else {
                        result["" + key] = extend(clone, {}, copy, deep);
                    }
                }
                else {
                    clone = src && Object.keys(copy).length;
                    result["" + key] = extend([], clone, copy, (clone && clone.length) || (copy && copy.length));
                }
            }
            else {
                result["" + key] = copy;
            }
        });
    };
    var arguments_1 = arguments;
    for (var i = 1; i < length; i++) {
        _loop_1(i);
    }
    return result;
}

/**
    * Returns the Class Object
    *
    * @param {ClassObject} instance - instance of ClassObject
    * @param {string} curKey - key of the current instance
    * @param {Object} defaultValue - default Value
    * @param {Object[]} type ?
    * @returns {ClassObject} ?
    */
function getObject(instance, curKey, defaultValue, type) {
    if (!instance.properties.hasOwnProperty(curKey) || !(instance.properties[curKey] instanceof type)) {
        instance.properties["" + curKey] = createInstance(type, [instance, curKey, defaultValue]);
    }
    return instance.properties["" + curKey];
}


/**
    * The function selects an id of element from the given context.
    *
    * @param  {string} selector - Selector string need fetch element
    * @returns {string} ?
    * @private
    */
function querySelectId(selector) {
    var charRegex = /(!|"|\$|%|&|'|\(|\)|\*|\/|:|;|<|=|\?|@|\]|\^|`|{|}|\||\+|~)/g;
    if (selector.match(/#[0-9]/g) || selector.match(charRegex)) {
        var idList = selector.split(',');
        for (var i = 0; i < idList.length; i++) {
            var list = idList[parseInt(i.toString(), 10)].split(' ');
            for (var j = 0; j < list.length; j++) {
                if (list[parseInt(j.toString(), 10)].indexOf('#') > -1) {
                    if (!list[parseInt(j.toString(), 10)].match(/\[.*\]/)) {
                        var splitId = list[parseInt(j.toString(), 10)].split('#');
                        if (splitId[1].match(/^\d/) || splitId[1].match(charRegex)) {
                            var setId = list[parseInt(j.toString(), 10)].split('.');
                            setId[0] = setId[0].replace(/#/, '[id=\'') + '\']';
                            list[parseInt(j.toString(), 10)] = setId.join('.');
                        }
                    }
                }
            }
            idList[parseInt(i.toString(), 10)] = list.join(' ');
        }
        return idList.join(',');
    }
    return selector;
}

/**
    * To set value for the nameSpace in desired object.
    *
    * @param {string} nameSpace - String value to the get the inner object
    * @param {any} value - Value that you need to set.
    * @param {any} obj - Object to get the inner object value.
    * @returns {any} ?
    * @private
    */
function setValue(nameSpace, value, obj) {
    if (!nameSpace || typeof nameSpace !== 'string') {
        return obj || {};
    }
    var keys = nameSpace.replace(/\[/g, '.').replace(/\]/g, '').split('.');
    var start = obj || {};
    var fromObj = start;
    var i;
    var length = keys.length;
    var key;
    for (i = 0; i < length; i++) {
        key = keys[parseInt(i.toString(), 10)];
        if (i + 1 === length) {
            fromObj["" + key] = value === undefined ? {} : value;
        }
        else if (isNullOrUndefined(fromObj["" + key])) {
            fromObj["" + key] = {};
        }
        fromObj = fromObj["" + key];
    }
    return start;
}


var isColEName = new RegExp(']');

/**
 * Base library module is common module for Framework modules like touch,keyboard and etc.,
 *
 */
class Base {
    /**
     * Base constructor accept options and element
     *
     * @param {Object} options ?
     * @param {string} element ?
     */
    constructor(options, element) {
        this.isRendered = false;
        this.isComplexArraySetter = false;
        this.isServerRendered = false;
        this.allowServerDataBinding = true;
        this.isProtectedOnChange = true;
        this.properties = {};
        this.changedProperties = {};
        this.oldProperties = {};
        this.bulkChanges = {};
        this.refreshing = false;
        this.ignoreCollectionWatch = false;
        this.finalUpdate = function () { };
        this.childChangedProperties = {};
        this.modelObserver = new Observer(this);

        if (!isUndefined(element)) {
            this.element = 'string' === typeof (element) ? document.querySelector(element) : element;
            if (!isNullOrUndefined(this.element)) {
                this.isProtectedOnChange = false;
                this.addInstance();
            }
        }
        if (!isUndefined(options)) {
            this.setProperties(options, true);
        }
        this.isDestroyed = false;
    }

    /** Property base section */
    /**
     * Function used to set bunch of property at a time.
     *
     * @private
     * @param  {Object} prop - JSON object which holds components properties.
     * @param  {boolean} muteOnChange ? - Specifies to true when we set properties.
     * @returns {void} ?
     */
    setProperties(prop, muteOnChange) {
        var prevDetection = this.isProtectedOnChange;
        this.isProtectedOnChange = !!muteOnChange;
        merge(this, prop);
        if (muteOnChange !== true) {
            merge(this.changedProperties, prop);
            this.dataBind();
        }
        else if (this.isRendered) {
            this.serverDataBind(prop);
        }
        this.finalUpdate();
        this.changedProperties = {};
        this.oldProperties = {};
        this.isProtectedOnChange = prevDetection;
    }

    /**
        * Calls for child element data bind
        *
        * @param {Object} obj ?
        * @param {Object} parent ?
        * @returns {void} ?
        */
    // tslint:disable-next-line:no-any
    callChildDataBind(obj, parent) {
        var keys = Object.keys(obj);
        for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
            var key = keys_1[_i];
            if (parent["" + key] instanceof Array) {
                for (var _a = 0, _b = parent["" + key]; _a < _b.length; _a++) {
                    var obj_1 = _b[_a];
                    if (obj_1.dataBind !== undefined) {
                        obj_1.dataBind();
                    }
                }
            }
            else {
                parent["" + key].dataBind();
            }
        }
    }

    clearChanges() {
        this.finalUpdate();
        this.changedProperties = {};
        this.oldProperties = {};
        this.childChangedProperties = {};
    }

    /**
        * Bind property changes immediately to components
        *
        * @returns {void} ?
        */
    dataBind() {
        this.callChildDataBind(this.childChangedProperties, this);
        if (Object.getOwnPropertyNames(this.changedProperties).length) {
            var prevDetection = this.isProtectedOnChange;
            var newChanges = this.changedProperties;
            var oldChanges = this.oldProperties;
            this.clearChanges();
            this.isProtectedOnChange = true;
            this.onPropertyChanged(newChanges, oldChanges);
            this.isProtectedOnChange = prevDetection;
        }
    }

    /* tslint:disable:no-any */
    serverDataBind(newChanges) {
        newChanges = newChanges ? newChanges : {};
        extend(this.bulkChanges, {}, newChanges, true);
        const sfBlazorToolkit = 'sfBlazorToolkit';
        if (this.allowServerDataBinding && window["" + sfBlazorToolkit].updateModel) {
            window["" + sfBlazorToolkit].updateModel(this);
            this.bulkChanges = {};
        }
    }

    /* tslint:enable:no-any */
    saveChanges(key, newValue, oldValue) {
        var newChanges = {};
        newChanges["" + key] = newValue;
        this.serverDataBind(newChanges);
        if (this.isProtectedOnChange) {
            return;
        }
        this.oldProperties["" + key] = oldValue;
        this.changedProperties["" + key] = newValue;
        this.finalUpdate();
        this.finalUpdate = setImmediate(this.dataBind.bind(this));
    }

    /** Event Base Section */
    /**
        * Adds the handler to the given event listener.
        *
        * @param {string} eventName - A String that specifies the name of the event
        * @param {Function} handler - Specifies the call to run when the event occurs.
        * @returns {void} ?
        */
    addEventListener(eventName, handler) {
        this.modelObserver.on(eventName, handler);
    }

    /**
        * Removes the handler from the given event listener.
        *
        * @param {string} eventName - A String that specifies the name of the event to remove
        * @param {Function} handler - Specifies the function to remove
        * @returns {void} ?
        */
    removeEventListener(eventName, handler) {
        this.modelObserver.off(eventName, handler);
    }

    /**
        * Triggers the handlers in the specified event.
        *
        * @private
        * @param {string} eventName - Specifies the event to trigger for the specified component properties.
        * Can be a custom event, or any of the standard events.
        * @param {Event} eventProp - Additional parameters to pass on to the event properties
        * @param {Function} successHandler - this function will invoke after event successfully triggered
        * @param {Function} errorHandler - this function will invoke after event if it failured to call.
        * @returns {void} ?
        */
    trigger(eventName, eventProp, successHandler, errorHandler) {
        var _this = this;
        if (this.isDestroyed !== true) {
            var prevDetection = this.isProtectedOnChange;
            this.isProtectedOnChange = false;
            var data = this.modelObserver.notify(eventName, eventProp, successHandler, errorHandler);
            if (isColEName.test(eventName)) {
                var handler = getValue(eventName, this);
                if (handler) {
                    var blazor = 'Blazor';
                    if (window["" + blazor]) {
                        var promise = handler.call(this, eventProp);
                        if (promise && typeof promise.then === 'function') {
                            if (!successHandler) {
                                data = promise;
                            }
                            else {
                                promise.then(function (data) {
                                    if (successHandler) {
                                        data = typeof data === 'string' && _this.modelObserver.isJson(data) ?
                                            JSON.parse(data) : data;
                                        successHandler.call(_this, data);
                                    }
                                }).catch(function (data) {
                                    if (errorHandler) {
                                        data = typeof data === 'string' && _this.modelObserver.isJson(data) ? JSON.parse(data) : data;
                                        errorHandler.call(_this, data);
                                    }
                                });
                            }
                        }
                        else if (successHandler) {
                            successHandler.call(this, eventProp);
                        }
                    }
                    else {
                        handler.call(this, eventProp);
                        if (successHandler) {
                            successHandler.call(this, eventProp);
                        }
                    }
                }
                else if (successHandler) {
                    successHandler.call(this, eventProp);
                }
            }
            this.isProtectedOnChange = prevDetection;
            return data;
        }
    }

    /**
        * To maintain instance in base class
        *
        * @returns {void} ?
        */
    addInstance() {
        // Add module class to the root element
        var moduleClass = 'e-' + this.getModuleName().toLowerCase();
        addClass([this.element], ['e-lib', moduleClass]);
        if (!isNullOrUndefined(this.element.ej2_instances)) {
            this.element.ej2_instances.push(this);
        }
        else {
            setValue('ej2_instances', [this], this.element);
        }
    }

    /**
        * To remove the instance from the element
        *
        * @returns {void} ?
        */
    destroy() {
        var _this = this;
        this.element.ej2_instances =
            this.element.ej2_instances ?
                this.element.ej2_instances.filter(function (i) {
                    if (exports.proxyToRaw) {
                        return exports.proxyToRaw(i) !== exports.proxyToRaw(_this);
                    }
                    return i !== _this;
                })
                : [];
        removeClass([this.element], ['e-' + this.getModuleName()]);
        if (this.element.ej2_instances.length === 0) {
            // Remove module class from the root element
            removeClass([this.element], ['e-lib']);
        }
        this.clearChanges();
        this.modelObserver.destroy();
        this.isDestroyed = true;
    }    
}

var regExp = RegExp;
var blazorCultureFormats = {
    'en-US': {
        'd': 'M/d/y',
        'D': 'EEEE, MMMM d, y',
        'f': 'EEEE, MMMM d, y h:mm a',
        'F': 'EEEE, MMMM d, y h:mm:s a',
        'g': 'M/d/y h:mm a',
        'G': 'M/d/yyyy h:mm:ss tt',
        'm': 'MMMM d',
        'M': 'MMMM d',
        'r': 'ddd, dd MMM yyyy HH\':\'mm\':\'ss \'GMT\'',
        'R': 'ddd, dd MMM yyyy HH\':\'mm\':\'ss \'GMT\'',
        's': 'yyyy\'-\'MM\'-\'dd\'T\'HH\':\'mm\':\'ss',
        't': 'h:mm tt',
        'T': 'h:m:s tt',
        'u': 'yyyy\'-\'MM\'-\'dd HH\':\'mm\':\'ss\'Z\'',
        'U': 'dddd, MMMM d, yyyy h:mm:ss tt',
        'y': 'MMMM yyyy',
        'Y': 'MMMM yyyy'
    }
};

/**
 * Date base common constants and function for date parser and formatter.
 */

(function (IntlBase) {
    /* eslint-disable */
    // tslint:disable-next-line:max-line-length.
    IntlBase.negativeDataRegex = /^(('[^']+'|''|[^*#@0,.E])*)(\*.)?((([#,]*[0,]*0+)(\.0*[0-9]*#*)?)|([#,]*@+#*))(E\+?0+)?(('[^']+'|''|[^*#@0,.E])*)$/;
    IntlBase.customRegex = /^(('[^']+'|''|[^*#@0,.])*)(\*.)?((([0#,]*[0,]*[0#]*[0#\ ]*)(\.[0#]*)?)|([#,]*@+#*))(E\+?0+)?(('[^']+'|''|[^*#@0,.E])*)$/;
    IntlBase.latnParseRegex = /0|1|2|3|4|5|6|7|8|9/g;
    var fractionRegex = /[0-9]/g;
    IntlBase.defaultCurrency = '$';
    var mapper = ['infinity', 'nan', 'group', 'decimal'];
    var patternRegex = /G|M|L|H|c|'| a|yy|y|EEEE|E/g;
    var patternMatch = {
        'G': '',
        'M': 'm',
        'L': 'm',
        'H': 'h',
        'c': 'd',
        '\'': '"',
        ' a': ' AM/PM',
        'yy': 'yy',
        'y': 'yyyy',
        'EEEE': 'dddd',
        'E': 'ddd'
    };
    IntlBase.dateConverterMapper = /dddd|ddd/ig;
    var defaultFirstDay = 'sun';
    IntlBase.islamicRegex = /^islamic/;
    var firstDayMapper = {
        'sun': 0,
        'mon': 1,
        'tue': 2,
        'wed': 3,
        'thu': 4,
        'fri': 5,
        'sat': 6
    };
    IntlBase.formatRegex = new regExp("(^[ncpae]{1})([0-1]?[0-9]|20)?$", "i");
    IntlBase.currencyFormatRegex = new regExp("(^[ca]{1})([0-1]?[0-9]|20)?$", "i");
    var typeMapper = {
        '$': 'isCurrency',
        '%': 'isPercent',
        '-': 'isNegative',
        0: 'nlead',
        1: 'nend'
    };
    IntlBase.dateParseRegex = /([a-z])\1*|'([^']|'')+'|''|./gi;
    IntlBase.basicPatterns = ['short', 'medium', 'long', 'full'];
    /* tslint:disable:quotemark */
    IntlBase.defaultObject = {
        'dates': {
            'calendars': {
                'gregorian': {
                    'months': {
                        'stand-alone': {
                            'abbreviated': {
                                '1': 'Jan',
                                '2': 'Feb',
                                '3': 'Mar',
                                '4': 'Apr',
                                '5': 'May',
                                '6': 'Jun',
                                '7': 'Jul',
                                '8': 'Aug',
                                '9': 'Sep',
                                '10': 'Oct',
                                '11': 'Nov',
                                '12': 'Dec'
                            },
                            'narrow': {
                                '1': 'J',
                                '2': 'F',
                                '3': 'M',
                                '4': 'A',
                                '5': 'M',
                                '6': 'J',
                                '7': 'J',
                                '8': 'A',
                                '9': 'S',
                                '10': 'O',
                                '11': 'N',
                                '12': 'D'
                            },
                            'wide': {
                                '1': 'January',
                                '2': 'February',
                                '3': 'March',
                                '4': 'April',
                                '5': 'May',
                                '6': 'June',
                                '7': 'July',
                                '8': 'August',
                                '9': 'September',
                                '10': 'October',
                                '11': 'November',
                                '12': 'December'
                            }
                        }
                    },
                    'days': {
                        'stand-alone': {
                            'abbreviated': {
                                'sun': 'Sun',
                                'mon': 'Mon',
                                'tue': 'Tue',
                                'wed': 'Wed',
                                'thu': 'Thu',
                                'fri': 'Fri',
                                'sat': 'Sat'
                            },
                            'narrow': {
                                'sun': 'S',
                                'mon': 'M',
                                'tue': 'T',
                                'wed': 'W',
                                'thu': 'T',
                                'fri': 'F',
                                'sat': 'S'
                            },
                            'short': {
                                'sun': 'Su',
                                'mon': 'Mo',
                                'tue': 'Tu',
                                'wed': 'We',
                                'thu': 'Th',
                                'fri': 'Fr',
                                'sat': 'Sa'
                            },
                            'wide': {
                                'sun': 'Sunday',
                                'mon': 'Monday',
                                'tue': 'Tuesday',
                                'wed': 'Wednesday',
                                'thu': 'Thursday',
                                'fri': 'Friday',
                                'sat': 'Saturday'
                            }
                        }
                    },
                    'dayPeriods': {
                        'format': {
                            'wide': {
                                'am': 'AM',
                                'pm': 'PM'
                            }
                        }
                    },
                    'eras': {
                        'eraNames': {
                            '0': 'Before Christ',
                            '0-alt-variant': 'Before Common Era',
                            '1': 'Anno Domini',
                            '1-alt-variant': 'Common Era'
                        },
                        'eraAbbr': {
                            '0': 'BC',
                            '0-alt-variant': 'BCE',
                            '1': 'AD',
                            '1-alt-variant': 'CE'
                        },
                        'eraNarrow': {
                            '0': 'B',
                            '0-alt-variant': 'BCE',
                            '1': 'A',
                            '1-alt-variant': 'CE'
                        }
                    },
                    'dateFormats': {
                        'full': 'EEEE, MMMM d, y',
                        'long': 'MMMM d, y',
                        'medium': 'MMM d, y',
                        'short': 'M/d/yy'
                    },
                    'timeFormats': {
                        'full': 'h:mm:ss a zzzz',
                        'long': 'h:mm:ss a z',
                        'medium': 'h:mm:ss a',
                        'short': 'h:mm a'
                    },
                    'dateTimeFormats': {
                        'full': '{1} \'at\' {0}',
                        'long': '{1} \'at\' {0}',
                        'medium': '{1}, {0}',
                        'short': '{1}, {0}',
                        'availableFormats': {
                            'd': 'd',
                            'E': 'ccc',
                            'Ed': 'd E',
                            'Ehm': 'E h:mm a',
                            'EHm': 'E HH:mm',
                            'Ehms': 'E h:mm:ss a',
                            'EHms': 'E HH:mm:ss',
                            'Gy': 'y G',
                            'GyMMM': 'MMM y G',
                            'GyMMMd': 'MMM d, y G',
                            'GyMMMEd': 'E, MMM d, y G',
                            'h': 'h a',
                            'H': 'HH',
                            'hm': 'h:mm a',
                            'Hm': 'HH:mm',
                            'hms': 'h:mm:ss a',
                            'Hms': 'HH:mm:ss',
                            'hmsv': 'h:mm:ss a v',
                            'Hmsv': 'HH:mm:ss v',
                            'hmv': 'h:mm a v',
                            'Hmv': 'HH:mm v',
                            'M': 'L',
                            'Md': 'M/d',
                            'MEd': 'E, M/d',
                            'MMM': 'LLL',
                            'MMMd': 'MMM d',
                            'MMMEd': 'E, MMM d',
                            'MMMMd': 'MMMM d',
                            'ms': 'mm:ss',
                            'y': 'y',
                            'yM': 'M/y',
                            'yMd': 'M/d/y',
                            'yMEd': 'E, M/d/y',
                            'yMMM': 'MMM y',
                            'yMMMd': 'MMM d, y',
                            'yMMMEd': 'E, MMM d, y',
                            'yMMMM': 'MMMM y'
                        }
                    }
                },
                'islamic': {
                    'months': {
                        'stand-alone': {
                            'abbreviated': {
                                '1': 'Muh.',
                                '2': 'Saf.',
                                '3': 'Rab. I',
                                '4': 'Rab. II',
                                '5': 'Jum. I',
                                '6': 'Jum. II',
                                '7': 'Raj.',
                                '8': 'Sha.',
                                '9': 'Ram.',
                                '10': 'Shaw.',
                                '11': 'Dhu?l-Q.',
                                '12': 'Dhu?l-H.'
                            },
                            'narrow': {
                                '1': '1',
                                '2': '2',
                                '3': '3',
                                '4': '4',
                                '5': '5',
                                '6': '6',
                                '7': '7',
                                '8': '8',
                                '9': '9',
                                '10': '10',
                                '11': '11',
                                '12': '12'
                            },
                            'wide': {
                                '1': 'Muharram',
                                '2': 'Safar',
                                '3': 'Rabi? I',
                                '4': 'Rabi? II',
                                '5': 'Jumada I',
                                '6': 'Jumada II',
                                '7': 'Rajab',
                                '8': 'Sha?ban',
                                '9': 'Ramadan',
                                '10': 'Shawwal',
                                '11': 'Dhu?l-Qi?dah',
                                '12': 'Dhu?l-Hijjah'
                            }
                        }
                    },
                    'days': {
                        'stand-alone': {
                            'abbreviated': {
                                'sun': 'Sun',
                                'mon': 'Mon',
                                'tue': 'Tue',
                                'wed': 'Wed',
                                'thu': 'Thu',
                                'fri': 'Fri',
                                'sat': 'Sat'
                            },
                            'narrow': {
                                'sun': 'S',
                                'mon': 'M',
                                'tue': 'T',
                                'wed': 'W',
                                'thu': 'T',
                                'fri': 'F',
                                'sat': 'S'
                            },
                            'short': {
                                'sun': 'Su',
                                'mon': 'Mo',
                                'tue': 'Tu',
                                'wed': 'We',
                                'thu': 'Th',
                                'fri': 'Fr',
                                'sat': 'Sa'
                            },
                            'wide': {
                                'sun': 'Sunday',
                                'mon': 'Monday',
                                'tue': 'Tuesday',
                                'wed': 'Wednesday',
                                'thu': 'Thursday',
                                'fri': 'Friday',
                                'sat': 'Saturday'
                            }
                        }
                    },
                    'dayPeriods': {
                        'format': {
                            'wide': {
                                'am': 'AM',
                                'pm': 'PM'
                            }
                        }
                    },
                    'eras': {
                        'eraNames': {
                            '0': 'AH'
                        },
                        'eraAbbr': {
                            '0': 'AH'
                        },
                        'eraNarrow': {
                            '0': 'AH'
                        }
                    },
                    'dateFormats': {
                        'full': 'EEEE, MMMM d, y G',
                        'long': 'MMMM d, y G',
                        'medium': 'MMM d, y G',
                        'short': 'M/d/y GGGGG'
                    },
                    'timeFormats': {
                        'full': 'h:mm:ss a zzzz',
                        'long': 'h:mm:ss a z',
                        'medium': 'h:mm:ss a',
                        'short': 'h:mm a'
                    },
                    'dateTimeFormats': {
                        'full': '{1} \'at\' {0}',
                        'long': '{1} \'at\' {0}',
                        'medium': '{1}, {0}',
                        'short': '{1}, {0}',
                        'availableFormats': {
                            'd': 'd',
                            'E': 'ccc',
                            'Ed': 'd E',
                            'Ehm': 'E h:mm a',
                            'EHm': 'E HH:mm',
                            'Ehms': 'E h:mm:ss a',
                            'EHms': 'E HH:mm:ss',
                            'Gy': 'y G',
                            'GyMMM': 'MMM y G',
                            'GyMMMd': 'MMM d, y G',
                            'GyMMMEd': 'E, MMM d, y G',
                            'h': 'h a',
                            'H': 'HH',
                            'hm': 'h:mm a',
                            'Hm': 'HH:mm',
                            'hms': 'h:mm:ss a',
                            'Hms': 'HH:mm:ss',
                            'M': 'L',
                            'Md': 'M/d',
                            'MEd': 'E, M/d',
                            'MMM': 'LLL',
                            'MMMd': 'MMM d',
                            'MMMEd': 'E, MMM d',
                            'MMMMd': 'MMMM d',
                            'ms': 'mm:ss',
                            'y': 'y G',
                            'yyyy': 'y G',
                            'yyyyM': 'M/y GGGGG',
                            'yyyyMd': 'M/d/y GGGGG',
                            'yyyyMEd': 'E, M/d/y GGGGG',
                            'yyyyMMM': 'MMM y G',
                            'yyyyMMMd': 'MMM d, y G',
                            'yyyyMMMEd': 'E, MMM d, y G',
                            'yyyyMMMM': 'MMMM y G',
                            'yyyyQQQ': 'QQQ y G',
                            'yyyyQQQQ': 'QQQQ y G'
                        }
                    }
                }
            },
            'timeZoneNames': {
                'hourFormat': '+HH:mm;-HH:mm',
                'gmtFormat': 'GMT{0}',
                'gmtZeroFormat': 'GMT'
            }
        },
        'numbers': {
            'currencies': {
                'USD': {
                    'displayName': 'US Dollar',
                    'symbol': '$',
                    'symbol-alt-narrow': '$'
                },
                'EUR': {
                    'displayName': 'Euro',
                    'symbol': '�',
                    'symbol-alt-narrow': '�'
                },
                'GBP': {
                    'displayName': 'British Pound',
                    'symbol-alt-narrow': '�'
                }
            },
            'defaultNumberingSystem': 'latn',
            'minimumGroupingDigits': '1',
            'symbols-numberSystem-latn': {
                'decimal': '.',
                'group': ',',
                'list': ';',
                'percentSign': '%',
                'plusSign': '+',
                'minusSign': '-',
                'exponential': 'E',
                'superscriptingExponent': '�',
                'perMille': '�',
                'infinity': '?',
                'nan': 'NaN',
                'timeSeparator': ':'
            },
            'decimalFormats-numberSystem-latn': {
                'standard': '#,##0.###'
            },
            'percentFormats-numberSystem-latn': {
                'standard': '#,##0%'
            },
            'currencyFormats-numberSystem-latn': {
                'standard': '�#,##0.00',
                'accounting': '�#,##0.00;(�#,##0.00)'
            },
            'scientificFormats-numberSystem-latn': {
                'standard': '#E0'
            }
        }
    };
    IntlBase.blazorDefaultObject = {
        'numbers': {
            'mapper': {
                '0': '0',
                '1': '1',
                '2': '2',
                '3': '3',
                '4': '4',
                '5': '5',
                '6': '6',
                '7': '7',
                '8': '8',
                '9': '9'
            },
            'mapperDigits': '0123456789',
            'numberSymbols': {
                'decimal': '.',
                'group': ',',
                'plusSign': '+',
                'minusSign': '-',
                'percentSign': '%',
                'nan': 'NaN',
                'timeSeparator': ':',
                'infinity': '?'
            },
            'timeSeparator': ':',
            'currencySymbol': '$',
            'currencypData': {
                'nlead': '$',
                'nend': '',
                'groupSeparator': ',',
                'groupData': {
                    'primary': 3
                },
                'maximumFraction': 2,
                'minimumFraction': 2
            },
            'percentpData': {
                'nlead': '',
                'nend': '%',
                'groupSeparator': ',',
                'groupData': {
                    'primary': 3
                },
                'maximumFraction': 2,
                'minimumFraction': 2
            },
            'percentnData': {
                'nlead': '-',
                'nend': '%',
                'groupSeparator': ',',
                'groupData': {
                    'primary': 3
                },
                'maximumFraction': 2,
                'minimumFraction': 2
            },
            'currencynData': {
                'nlead': '($',
                'nend': ')',
                'groupSeparator': ',',
                'groupData': {
                    'primary': 3
                },
                'maximumFraction': 2,
                'minimumFraction': 2
            },
            'decimalnData': {
                'nlead': '-',
                'nend': '',
                'groupData': {
                    'primary': 3
                },
                'maximumFraction': 2,
                'minimumFraction': 2
            },
            'decimalpData': {
                'nlead': '',
                'nend': '',
                'groupData': {
                    'primary': 3
                },
                'maximumFraction': 2,
                'minimumFraction': 2
            },
            'numberSystem': 'latn',
            'decimalFormats-numberSystem-latn': {
                'standard': '#,##0.###'
            },
            'percentFormats-numberSystem-latn': {
                'standard': '#,##0%'
            },
            'currencyFormats-numberSystem-latn': {
                'standard': '�#,##0.00',
                'accounting': '�#,##0.00;(�#,##0.00)'
            },
            'scientificFormats-numberSystem-latn': {
                'standard': '#E0'
            }
        },
        'dates': {
            'dayPeriods': {
                'am': 'AM',
                'pm': 'PM'
            },
            'dateSeperator': '/',
            'days': {
                'abbreviated': {
                    'sun': 'Sun',
                    'mon': 'Mon',
                    'tue': 'Tue',
                    'wed': 'Wed',
                    'thu': 'Thu',
                    'fri': 'Fri',
                    'sat': 'Sat'
                },
                'short': {
                    'sun': 'Su',
                    'mon': 'Mo',
                    'tue': 'Tu',
                    'wed': 'We',
                    'thu': 'Th',
                    'fri': 'Fr',
                    'sat': 'Sa'
                },
                'wide': {
                    'sun': 'Sunday',
                    'mon': 'Monday',
                    'tue': 'Tuesday',
                    'wed': 'Wednesday',
                    'thu': 'Thursday',
                    'fri': 'Friday',
                    'sat': 'Saturday'
                }
            },
            'months': {
                'abbreviated': {
                    '1': 'Jan',
                    '2': 'Feb',
                    '3': 'Mar',
                    '4': 'Apr',
                    '5': 'May',
                    '6': 'Jun',
                    '7': 'Jul',
                    '8': 'Aug',
                    '9': 'Sep',
                    '10': 'Oct',
                    '11': 'Nov',
                    '12': 'Dec'
                },
                'wide': {
                    '1': 'January',
                    '2': 'February',
                    '3': 'March',
                    '4': 'April',
                    '5': 'May',
                    '6': 'June',
                    '7': 'July',
                    '8': 'August',
                    '9': 'September',
                    '10': 'October',
                    '11': 'November',
                    '12': 'December'
                }
            },
            'eras': {
                '1': 'AD'
            }
        }
    };
    /* tslint:enable:quotemark */
    IntlBase.monthIndex = {
        3: 'abbreviated',
        4: 'wide',
        5: 'narrow',
        1: 'abbreviated'
    };
    /**
     *
     */
    IntlBase.month = 'months';
    IntlBase.days = 'days';
    /**
     * Default numerber Object
     */
    IntlBase.patternMatcher = {
        C: 'currency',
        P: 'percent',
        N: 'decimal',
        A: 'currency',
        E: 'scientific'
    };
    /**
     * Returns the resultant pattern based on the skeleton, dateObject and the type provided
     *
     * @private
     * @param {string} skeleton ?
     * @param {Object} dateObject ?
     * @param {string} type ?
     * @param {boolean} isIslamic ?
     * @param {string} blazorCulture ?
     * @returns {string} ?
     */
    function getResultantPattern(skeleton, dateObject, type, isIslamic, blazorCulture) {
        var resPattern;
        var iType = type || 'date';
        if (blazorCulture) {
            resPattern = compareBlazorDateFormats({ skeleton: skeleton }, blazorCulture).format ||
                compareBlazorDateFormats({ skeleton: 'd' }, 'en-US').format;
        }
        else {
            if (IntlBase.basicPatterns.indexOf(skeleton) !== -1) {
                resPattern = getValue(iType + 'Formats.' + skeleton, dateObject);
                if (iType === 'dateTime') {
                    var dPattern = getValue('dateFormats.' + skeleton, dateObject);
                    var tPattern = getValue('timeFormats.' + skeleton, dateObject);
                    resPattern = resPattern.replace('{1}', dPattern).replace('{0}', tPattern);
                }
            }
            else {
                resPattern = getValue('dateTimeFormats.availableFormats.' + skeleton, dateObject);
            }
            if (isUndefined(resPattern) && skeleton === 'yMd') {
                resPattern = 'M/d/y';
            }
        }
        return resPattern;
    }
    IntlBase.getResultantPattern = getResultantPattern;
    /**
     * Returns the dependable object for provided cldr data and culture
     *
     * @private
     * @param {Object} cldr ?
     * @param {string} culture ?
     * @param {string} mode ?
     * @param {boolean} isNumber ?
     * @returns {any} ?
     */
    function getDependables(cldr, culture, mode, isNumber) {
        var ret = {};
        var calendartype = mode || 'gregorian';
        ret.parserObject = ParserBase.getMainObject(cldr, culture) || (IntlBase.blazorDefaultObject);
        if (isNumber) {
            ret.numericObject = getValue('numbers', ret.parserObject);
        }
        else {
            var dateString = 'dates';
            ret.dateObject = getValue(dateString, ret.parserObject);
        }
        return ret;
    }
    IntlBase.getDependables = getDependables;
    /**
     * Returns the symbol pattern for provided parameters
     *
     * @private
     * @param {string} type ?
     * @param {string} numSystem ?
     * @param {Object} obj ?
     * @param {boolean} isAccount ?
     * @returns {string} ?
     */
    function getSymbolPattern(type, numSystem, obj, isAccount) {
        return getValue(type + 'Formats-numberSystem-' +
            numSystem + (isAccount ? '.accounting' : '.standard'), obj) || (isAccount ? getValue(type + 'Formats-numberSystem-' +
                numSystem + '.standard', obj) : '');
    }
    IntlBase.getSymbolPattern = getSymbolPattern;
    /**
     *
     * @param {string} format ?
     * @returns {string} ?
     */
    function ConvertDateToWeekFormat(format) {
        var convertMapper = format.match(IntlBase.dateConverterMapper);
        if (convertMapper) {
            var tempString = convertMapper[0].length === 3 ? 'EEE' : 'EEEE';
            return format.replace(IntlBase.dateConverterMapper, tempString);
        }
        return format;
    }
    IntlBase.ConvertDateToWeekFormat = ConvertDateToWeekFormat;
    /**
     *
     * @param {DateFormatOptions} formatOptions ?
     * @param {string} culture ?
     * @returns {DateFormatOptions} ?
     */
    function compareBlazorDateFormats(formatOptions, culture) {
        var format = formatOptions.format || formatOptions.skeleton;
        var curFormatMapper = getValue((culture || 'en-US') + '.' + format, blazorCultureFormats);
        if (!curFormatMapper) {
            curFormatMapper = getValue('en-US.' + format, blazorCultureFormats);
        }
        if (curFormatMapper) {
            curFormatMapper = ConvertDateToWeekFormat(curFormatMapper);
            formatOptions.format = curFormatMapper.replace(/tt/, 'a');
        }
        return formatOptions;
    }
    IntlBase.compareBlazorDateFormats = compareBlazorDateFormats;
    /**
     * Returns proper numeric skeleton
     *
     * @private
     * @param {string} skeleton ?
     * @returns {any} ?
     */
    function getProperNumericSkeleton(skeleton) {
        var matches = skeleton.match(IntlBase.formatRegex);
        var ret = {};
        var pattern = matches[1].toUpperCase();
        ret.isAccount = (pattern === 'A');
        ret.type = IntlBase.patternMatcher[pattern];
        if (skeleton.length > 1) {
            ret.fractionDigits = parseInt(matches[2], 10);
        }
        return ret;
    }
    IntlBase.getProperNumericSkeleton = getProperNumericSkeleton;
    /**
     * Returns format data for number formatting like minimum fraction, maximum fraction, etc..,
     *
     * @private
     * @param {string} pattern ?
     * @param {boolean} needFraction ?
     * @param {string} cSymbol ?
     * @param {boolean} fractionOnly ?
     * @returns {any} ?
     */
    function getFormatData(pattern, needFraction, cSymbol, fractionOnly) {
        var nData = fractionOnly ? {} : { nlead: '', nend: '' };
        var match = pattern.match(IntlBase.customRegex);
        if (match) {
            if (!fractionOnly) {
                nData.nlead = changeCurrencySymbol(match[1], cSymbol);
                nData.nend = changeCurrencySymbol(match[10], cSymbol);
                nData.groupPattern = match[4];
            }
            var fraction = match[7];
            if (fraction && needFraction) {
                var fmatch = fraction.match(fractionRegex);
                if (!isNullOrUndefined(fmatch)) {
                    nData.minimumFraction = fmatch.length;
                }
                else {
                    nData.minimumFraction = 0;
                }
                nData.maximumFraction = fraction.length - 1;
            }
        }
        return nData;
    }
    IntlBase.getFormatData = getFormatData;
    /**
     * Changes currency symbol
     *
     * @private
     * @param {string} val ?
     * @param {string} sym ?
     * @returns {string} ?
     */
    function changeCurrencySymbol(val, sym) {
        if (val) {
            val = val.replace(IntlBase.defaultCurrency, sym);
            return (sym === '') ? val.trim() : val;
        }
        return '';
    }
    IntlBase.changeCurrencySymbol = changeCurrencySymbol;
    /**
     * Returns formatting options for custom number format
     *
     * @private
     * @param {string} format ?
     * @param {CommonOptions} dOptions ?
     * @param {any} obj ?
     * @returns {any} ?
     */
    function customFormat(format, dOptions, obj) {
        var options = {};
        var formatSplit = format.split(';');
        var data = ['pData', 'nData', 'zeroData'];
        for (var i = 0; i < formatSplit.length; i++) {
            options[data[i]] = customNumberFormat(formatSplit[i], dOptions, obj);
        }
        if (isNullOrUndefined(options.nData)) {
            options.nData = extend({}, options.pData);
            options.nData.nlead = isNullOrUndefined(dOptions) ? '-' + options.nData.nlead : dOptions.minusSymbol + options.nData.nlead;
        }
        return options;
    }
    IntlBase.customFormat = customFormat;
    /**
     * Returns custom formatting options
     *
     * @private
     * @param {string} format ?
     * @param {CommonOptions} dOptions ?
     * @param {Object} numObject ?
     * @returns {any} ?
     */
    function customNumberFormat(format, dOptions, numObject) {
        var cOptions = { type: 'decimal', minimumFractionDigits: 0, maximumFractionDigits: 0 };
        var pattern = format.match(IntlBase.customRegex);
        if (isNullOrUndefined(pattern) || (pattern[5] === '' && format !== 'N/A')) {
            cOptions.type = undefined;
            return cOptions;
        }
        cOptions.nlead = pattern[1];
        cOptions.nend = pattern[10];
        var integerPart = pattern[6];
        var spaceCapture = integerPart.match(/\ $/g) ? true : false;
        var spaceGrouping = integerPart.replace(/\ $/g, '').indexOf(' ') !== -1;
        cOptions.useGrouping = integerPart.indexOf(',') !== -1 || spaceGrouping;
        integerPart = integerPart.replace(/,/g, '');
        var fractionPart = pattern[7];
        if (integerPart.indexOf('0') !== -1) {
            cOptions.minimumIntegerDigits = integerPart.length - integerPart.indexOf('0');
        }
        if (!isNullOrUndefined(fractionPart)) {
            cOptions.minimumFractionDigits = fractionPart.lastIndexOf('0');
            cOptions.maximumFractionDigits = fractionPart.lastIndexOf('#');
            if (cOptions.minimumFractionDigits === -1) {
                cOptions.minimumFractionDigits = 0;
            }
            if (cOptions.maximumFractionDigits === -1 || cOptions.maximumFractionDigits < cOptions.minimumFractionDigits) {
                cOptions.maximumFractionDigits = cOptions.minimumFractionDigits;
            }
        }
        if (!isNullOrUndefined(dOptions)) {
            dOptions.isCustomFormat = true;
            extend(cOptions, isCurrencyPercent([cOptions.nlead, cOptions.nend], '$', dOptions.currencySymbol));
            if (!cOptions.isCurrency) {
                extend(cOptions, isCurrencyPercent([cOptions.nlead, cOptions.nend], '%', dOptions.percentSymbol));
            }
        }
        else {
            extend(cOptions, isCurrencyPercent([cOptions.nlead, cOptions.nend], '%', '%'));
        }
        if (!isNullOrUndefined(numObject)) {
            var symbolPattern = getSymbolPattern(cOptions.type, dOptions.numberMapper.numberSystem, numObject, false);
            if (cOptions.useGrouping) {
                cOptions.groupSeparator = spaceGrouping ? ' ' : dOptions.numberMapper.numberSymbols[mapper[2]];
                cOptions.groupData = NumberFormat.getGroupingDetails(symbolPattern.split(';')[0]);
            }
            cOptions.nlead = cOptions.nlead.replace(/'/g, '');
            cOptions.nend = spaceCapture ? ' ' + cOptions.nend.replace(/'/g, '') : cOptions.nend.replace(/'/g, '');
        }
        return cOptions;
    }
    IntlBase.customNumberFormat = customNumberFormat;
    /**
     * Returns formatting options for currency or percent type
     *
     * @private
     * @param {string[]} parts ?
     * @param {string} actual ?
     * @param {string} symbol ?
     * @returns {any} ?
     */
    function isCurrencyPercent(parts, actual, symbol) {
        var options = { nlead: parts[0], nend: parts[1] };
        for (var i = 0; i < 2; i++) {
            var part = parts[parseInt(i.toString(), 10)];
            var loc = part.indexOf(actual);
            if ((loc !== -1) && ((loc < part.indexOf('\'')) || (loc > part.lastIndexOf('\'')))) {
                options[typeMapper[i]] = part.substring(0, loc) + symbol + part.substring(loc + 1);
                options[typeMapper[actual]] = true;
                options.type = options.isCurrency ? 'currency' : 'percent';
                break;
            }
        }
        return options;
    }
    IntlBase.isCurrencyPercent = isCurrencyPercent;

    /**
     *
     * @param {string} actual ?
     * @param {any} option ?
     * @returns {any} ?
     */
    function processSymbol(actual, option) {
        if (actual.indexOf(',') !== -1) {
            var split = actual.split(',');
            actual = (split[0] + getValue('numberMapper.numberSymbols.group', option) +
                split[1].replace('.', getValue('numberMapper.numberSymbols.decimal', option)));
        }
        else {
            actual = actual.replace('.', getValue('numberMapper.numberSymbols.decimal', option));
        }
        return actual;
    }
    /**
     * Returns Native Number pattern
     *
     * @private
     * @param {string} culture ?
     * @param {NumberFormatOptions} options ?
     * @param {Object} cldr ?
     * @param {boolean} isExcel ?
     * @returns {string} ?
     */
    function getActualNumberFormat(culture, options, cldr, isExcel) {
        var dependable = getDependables(cldr, culture, '', true);
        var parseOptions = { custom: true };
        var numrericObject = dependable.numericObject;
        var minFrac;
        var curObj = {};
        var curMatch = (options.format || '').match(IntlBase.currencyFormatRegex);
        var type = IntlBase.formatRegex.test(options.format) ? getProperNumericSkeleton(options.format || 'N') : {};
        var dOptions = {};
        if (curMatch) {
            dOptions.numberMapper = extend({}, dependable.numericObject);
            var curCode = getValue('currencySymbol', dependable.numericObject);
            var symbolPattern = getSymbolPattern('currency', dOptions.numberMapper.numberSystem, dependable.numericObject, (/a/i).test(options.format));
            symbolPattern = symbolPattern.replace(/\u00A4/g, curCode);
            var split = symbolPattern.split(';');
            curObj.hasNegativePattern = true;
            curObj.nData = getValue(type.type + 'nData', numrericObject);
            curObj.pData = getValue(type.type + 'pData', numrericObject);
            if (!curMatch[2] && !options.minimumFractionDigits && !options.maximumFractionDigits) {
                minFrac = getFormatData(symbolPattern.split(';')[0], true, '', true).minimumFraction;
            }
        }
        var actualPattern;
        if ((IntlBase.formatRegex.test(options.format)) || !(options.format)) {
            extend(parseOptions, getProperNumericSkeleton(options.format || 'N'));
            parseOptions.custom = false;
            actualPattern = '###0';
            if (parseOptions.fractionDigits || options.minimumFractionDigits || options.maximumFractionDigits || minFrac) {
                var defaultMinimum = 0;
                if (parseOptions.fractionDigits) {
                    options.minimumFractionDigits = options.maximumFractionDigits = parseOptions.fractionDigits;
                }
                actualPattern = fractionDigitsPattern(actualPattern, minFrac || parseOptions.fractionDigits ||
                    options.minimumFractionDigits || defaultMinimum, options.maximumFractionDigits || defaultMinimum);
            }
            if (options.minimumIntegerDigits) {
                actualPattern = minimumIntegerPattern(actualPattern, options.minimumIntegerDigits);
            }
            if (options.useGrouping) {
                actualPattern = groupingPattern(actualPattern);
            }
            if (parseOptions.type === 'currency' || (parseOptions.type)) {
                if (parseOptions.type !== 'currency') {
                    curObj.pData = getValue(parseOptions.type + 'pData', numrericObject);
                    curObj.nData = getValue(parseOptions.type + 'nData', numrericObject);
                }
                var cPattern = actualPattern;
                actualPattern = curObj.pData.nlead + cPattern + curObj.pData.nend;
                if (curObj.hasNegativePattern) {
                    actualPattern += ';' + curObj.nData.nlead + cPattern + curObj.nData.nend;
                }
            }
        }
        else {
            actualPattern = options.format.replace(/'/g, '"');
        }
        if (Object.keys(dOptions).length > 0) {
            actualPattern = !isExcel ? processSymbol(actualPattern, dOptions) : actualPattern;
        }
        return actualPattern;
    }
    IntlBase.getActualNumberFormat = getActualNumberFormat;
    /**
     *
     * @param {string} pattern ?
     * @param {number} minDigits ?
     * @param {number} maxDigits ?
     * @returns {string} ?
     */
    function fractionDigitsPattern(pattern, minDigits, maxDigits) {
        pattern += '.';
        for (var a = 0; a < minDigits; a++) {
            pattern += '0';
        }
        if (minDigits < maxDigits) {
            var diff = maxDigits - minDigits;
            for (var b = 0; b < diff; b++) {
                pattern += '#';
            }
        }
        return pattern;
    }
    IntlBase.fractionDigitsPattern = fractionDigitsPattern;
    /**
     *
     * @param {string} pattern ?
     * @param {number} digits ?
     * @returns {string} ?
     */
    function minimumIntegerPattern(pattern, digits) {
        var temp = pattern.split('.');
        var integer = '';
        for (var x = 0; x < digits; x++) {
            integer += '0';
        }
        return temp[1] ? (integer + '.' + temp[1]) : integer;
    }
    IntlBase.minimumIntegerPattern = minimumIntegerPattern;
    /**
     *
     * @param {string} pattern ?
     * @returns {string} ?
     */
    function groupingPattern(pattern) {
        var temp = pattern.split('.');
        var integer = temp[0];
        var no = 3 - integer.length % 3;
        var hash = (no && no === 1) ? '#' : (no === 2 ? '##' : '');
        integer = hash + integer;
        pattern = '';
        for (var x = integer.length - 1; x > 0; x = x - 3) {
            pattern = ',' + integer[x - 2] + integer[x - 1] + integer[parseInt(x.toString(), 10)] + pattern;
        }
        pattern = pattern.slice(1);
        return temp[1] ? (pattern + '.' + temp[1]) : pattern;
    }
    IntlBase.groupingPattern = groupingPattern;
    /**
     *
     * @param {string} culture ?
     * @param {Object} cldr ?
     * @returns {number} ?
     */
    function getWeekData(culture, cldr) {
        var firstDay = defaultFirstDay;
        var mapper = getValue('supplemental.weekData.firstDay', cldr);
        var iCulture = culture;
        if ((/en-/).test(iCulture)) {
            iCulture = iCulture.slice(3);
        }
        iCulture = iCulture.slice(0, 2).toUpperCase() + iCulture.substring(2);
        if (mapper) {
            firstDay = mapper["" + iCulture] || mapper[iCulture.slice(0, 2)] || defaultFirstDay;
        }
        return firstDayMapper["" + firstDay];
    }
    IntlBase.getWeekData = getWeekData;
    /**
     * @private
     * @param {any} pData ?
     * @param {string} aCurrency ?
     * @param {string} rCurrency ?
     * @returns {void} ?
     */
    function replaceBlazorCurrency(pData, aCurrency, rCurrency) {
        var iCurrency = getBlazorCurrencySymbol(rCurrency);
        if (aCurrency !== iCurrency) {
            for (var _i = 0, pData_1 = pData; _i < pData_1.length; _i++) {
                var data = pData_1[_i];
                data.nend = data.nend.replace(aCurrency, iCurrency);
                data.nlead = data.nlead.replace(aCurrency, iCurrency);
            }
        }
    }
    IntlBase.replaceBlazorCurrency = replaceBlazorCurrency;
    /**
     * @private
     * @param {Date} date ?
     * @returns {number} ?
     */
    function getWeekOfYear(date) {
        var newYear = new Date(date.getFullYear(), 0, 1);
        var day = newYear.getDay();
        var weeknum;
        day = (day >= 0 ? day : day + 7);
        var daynum = Math.floor((date.getTime() - newYear.getTime() -
            (date.getTimezoneOffset() - newYear.getTimezoneOffset()) * 60000) / 86400000) + 1;
        if (day < 4) {
            weeknum = Math.floor((daynum + day - 1) / 7) + 1;
            if (weeknum > 52) {
                var nYear = new Date(date.getFullYear() + 1, 0, 1);
                var nday = nYear.getDay();
                nday = nday >= 0 ? nday : nday + 7;
                weeknum = nday < 4 ? 1 : 53;
            }
        }
        else {
            weeknum = Math.floor((daynum + day - 1) / 7);
        }
        return weeknum;
    }
    IntlBase.getWeekOfYear = getWeekOfYear;
})(exports.IntlBase || (exports.IntlBase = {}));

const DEFAULT_NUMBERING_SYSTEM = {
    'latn': {
        '_digits': '0123456789',
        '_type': 'numeric'
    }
};
var LATN_NUMBER_SYSTEM = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

/**
 * Interface for parser base
 *
 * @private
 */
class ParserBase {
    constructor() {
    }

    /**
     * Returns the cldr object for the culture specifies
     *
     * @param {Object} obj - Specifies the object from which culture object to be acquired.
     * @param {string} cName - Specifies the culture name.
     * @returns {Object} ?
     */
    static getMainObject(obj, cName) {
        const value = cName;
        return getValue(value, obj);
    }

    /**
     * Returns the numbering system object from given cldr data.
     *
     * @param {Object} obj - Specifies the object from which number system is acquired.
     * @returns {Object} ?
     */
    static getNumberingSystem(obj) {
        return getValue('supplemental.numberingSystems', obj) || this.numberingSystems;
    }

    /**
     * Returns the replaced value of matching regex and obj mapper.
     *
     * @param {string} value - Specifies the  values to be replaced.
     * @param {RegExp} regex - Specifies the  regex to search.
     * @param {Object} obj - Specifies the  object matcher to be replace value parts.
     * @returns {string} ?
     */
    static convertValueParts(value, regex, obj) {
        return value.replace(regex, function (str) {
            return obj[str];
        });
    }

    /**
     * Returns default numbering system object for formatting from cldr data
     *
     * @param {Object} obj ?
     * @returns {NumericObject} ?
     */
    static getDefaultNumberingSystem(obj) {
        const ret = {};
        ret.obj = getValue('numbers', obj);
        ret.nSystem = getValue('defaultNumberingSystem', ret.obj);
        return ret;
    };
    /**
     * Returns number mapper object for the provided cldr data
     *
     * @param {Object} curObj ?
     * @param {Object} numberSystem ?
     * @param {boolean} isNumber ?
     * @returns {NumberMapper} ?
     */
    static getNumberMapper(curObj, numberSystem, isNumber) {
        const ret = { mapper: {} };
        const cur = this.getDefaultNumberingSystem(curObj);
        if (!isUndefined(cur.nSystem)) {
            ret.numberSystem = cur.nSystem;
            ret.numberSymbols = getValue('symbols-numberSystem-' + cur.nSystem, cur.obj);
            ret.timeSeparator = getValue('timeSeparator', ret.numberSymbols);
            const digits = getValue(cur.nSystem + '._digits', numberSystem);
            if (!isUndefined(digits)) {
                for (let _i = 0, latnNumberSystem_1 = LATN_NUMBER_SYSTEM; _i < latnNumberSystem_1.length; _i++) {
                    let i = latnNumberSystem_1[_i];
                    ret.mapper[i] = digits[i];
                }
            }
        }
        return ret;
    }

    static nPair = 'numericPair';
    static nRegex = 'numericRegex';
    static numberingSystems = DEFAULT_NUMBERING_SYSTEM;
}

/**
 * To detect the changes for inner properties.
 *
 * @private
 */
class ChildProperty {
    constructor(parent, propName, defaultValue, isArray) {
        this.isComplexArraySetter = false;
        this.properties = {};
        this.changedProperties = {};
        this.childChangedProperties = {};
        this.oldProperties = {};
        this.finalUpdate = function () { };
        this.callChildDataBind = getValue('callChildDataBind', Base);
        this.parentObj = parent;
        this.controlParent = this.parentObj.controlParent || this.parentObj;
        this.propName = propName;
        this.isParentArray = isArray;
        this.setProperties(defaultValue, true);
    }

    /**
     * Updates the property changes
     *
     * @param {boolean} val ?
     * @param {string} propName ?
     * @returns {void} ?
     */
    updateChange(val, propName) {
        if (val === true) {
            this.parentObj.childChangedProperties["" + propName] = val;
        }
        else {
            delete this.parentObj.childChangedProperties["" + propName];
        }
        if (this.parentObj.updateChange) {
            this.parentObj.updateChange(val, this.parentObj.propName);
        }
    }

    /**
     * Updates time out duration
     *
     * @returns {void} ?
     */
    updateTimeOut() {
        if (this.parentObj.updateTimeOut) {
            this.parentObj.finalUpdate();
            this.parentObj.updateTimeOut();
        }
        else {
            const changeTime_1 = setTimeout(this.parentObj.dataBind.bind(this.parentObj));
            const clearUpdate = function () {
                clearTimeout(changeTime_1);
            };
            this.finalUpdate = clearUpdate;
        }
    }

    /**
     * Clears changed properties
     *
     * @returns {void} ?
     */
    clearChanges() {
        this.finalUpdate();
        this.updateChange(false, this.propName);
        this.oldProperties = {};
        this.changedProperties = {};
    }

    /**
     * Set property changes
     *
     * @param {Object} prop ?
     * @param {boolean} muteOnChange ?
     * @returns {void} ?
     */
    setProperties(prop, muteOnChange) {
        if (muteOnChange === true) {
            merge(this, prop);
            this.updateChange(false, this.propName);
            this.clearChanges();
        }
        else {
            merge(this, prop);
        }
    }

    /**
     * Saves changes to newer values
     *
     * @param {string} key ?
     * @param {Object} newValue ?
     * @param {Object} oldValue ?
     * @param {boolean} restrictServerDataBind ?
     * @returns {void} ?
     */
    saveChanges(key, newValue, oldValue, restrictServerDataBind) {
        if (this.controlParent.isProtectedOnChange) {
            return;
        }
        if (!restrictServerDataBind) {
            this.serverDataBind(key, newValue, true);
        }
        this.oldProperties["" + key] = oldValue;
        this.changedProperties["" + key] = newValue;
        this.updateChange(true, this.propName);
        this.finalUpdate();
        this.updateTimeOut();
    }

    serverDataBind(key, value, isSaveChanges, action) {
        if (!this.parentObj.isComplexArraySetter) {
            let parentRef;
            let newChanges = {};
            let parentKey = isSaveChanges ? this.getParentKey(true) + '.' + key : key;
            /* istanbul ignore else  */
            if (parentKey.indexOf('.') !== -1) {
                const complexKeys = parentKey.split('.');
                parentRef = newChanges;
                for (let i = 0; i < complexKeys.length; i++) {
                    const isFinal = i === complexKeys.length - 1;
                    parentRef[complexKeys[parseInt(i.toString(), 10)]] = isFinal ? value : {};
                    parentRef = isFinal ? parentRef : parentRef[complexKeys[parseInt(i.toString(), 10)]];
                }
            }
            else {
                newChanges["" + parentKey] = {};
                parentRef = newChanges["" + parentKey];
                newChanges["" + parentKey]["" + key] = value;
            }
            /* istanbul ignore next */
            if (this.isParentArray) {
                const actionProperty = 'ejsAction';
                parentRef["" + actionProperty] = action ? action : 'none';
            }
            this.controlParent.serverDataBind(newChanges);
        }
    }

    getParentKey(isSaveChanges) {
        let index = '';
        let propName = this.propName;
        if (this.isParentArray) {
            index = this.parentObj[this.propName].indexOf(this);
            let valueLength = this.parentObj[this.propName].length;
            valueLength = isSaveChanges ? valueLength : (valueLength > 0 ? valueLength - 1 : 0);
            index = index !== -1 ? '-' + index : '-' + valueLength;
            propName = propName + index;
        }
        if (this.controlParent !== this.parentObj) {
            propName = this.parentObj.getParentKey() + '.' + this.propName + index;
        }
        return propName;
    }
}

/**
 * Base class for all Essential JavaScript components
 */
class Component extends Base {
    /**
     * Initialize the constructor for component base
     *
     * @param {Object} options
     * @param {string|HTMLElement} selector
     */
    constructor(options, selector) {
        super(options, selector);
        this.randomId = uniqueID();
        /**
         * string template option for Blazor template rendering
         *
         * @private
         */
        this.isStringTemplate = false;
        this.needsID = false;
        this.isReactHybrid = false;

        if (isNullOrUndefined(this.enableRtl)) {
            this.setProperties({ 'enableRtl': exports.rightToLeft }, true);
        }
        if (isNullOrUndefined(this.locale)) {
            this.setProperties({ 'locale': exports.defaultCulture }, true);
        }

        this.moduleLoader = new ModuleLoader(this);
        this.localObserver = new Observer(this);

        onIntlChange.on('notifyExternalChange', this.detectFunction, this, this.randomId);

        if (!isUndefined(selector)) {
            this.appendTo();
        }
    }

    requiredModules() {
        return [];
    }

    /**
     * Destroys the sub modules while destroying the widget
     *
     * @returns {void} ?
     */
    destroy() {
        if (this.isDestroyed) {
            return;
        }
        if (this.enablePersistence) {
            this.setPersistData();
            this.detachUnloadEvent();
        }
        this.localObserver.destroy();
        if (this.refreshing) {
            return;
        }
        removeClass([this.element], ['e-control']);
        this.trigger('destroyed', { cancel: false });
        super.destroy();
        this.moduleLoader.clean();
        onIntlChange.off('notifyExternalChange', this.detectFunction, this.randomId);
    }

    /**
     * This is a instance method to create an element.
     *
     * @param {string} tagName ?
     * @param {ElementProperties} prop ?
     * @param {boolean} isVDOM ?
     * @returns {any} ?
     * @private
     */
    createElement(tagName, prop, isVDOM) {
        return createElement(tagName, prop);
    }

    /**
     * Appends the control within the given HTML element
     *
     * @param {string | HTMLElement} selector - Target element where control needs to be appended
     * @returns {void} ?
     */
    appendTo(selector) {
        if (!isNullOrUndefined(selector) && typeof (selector) === 'string') {
            this.element = select(selector, document);
        }
        else if (!isNullOrUndefined(selector)) {
            this.element = selector;
        }
        if (!isNullOrUndefined(this.element)) {
            var moduleClass = 'e-' + this.getModuleName().toLowerCase();
            addClass([this.element], ['e-control', moduleClass]);
            this.isProtectedOnChange = false;
            if (this.needsID && !this.element.id) {
                this.element.id = this.getUniqueID(this.getModuleName());
            }
            if (this.enablePersistence) {
                this.mergePersistData();
                this.attachUnloadEvent();
            }
            var inst = getValue('ej2_instances', this.element);
            if (!inst || inst.indexOf(this) === -1) {
                super.addInstance();
            }
            this.preRender();
            this.injectModules();
            // Throw a warning for the required modules to be injected.
            var ignoredComponents = {
                schedule: 'all',
                diagram: 'all',
                PdfViewer: 'all',
                grid: ['logger'],
                richtexteditor: ['link', 'table', 'image', 'audio', 'video', 'formatPainter', 'emojiPicker', 'pasteCleanup', 'htmlEditor', 'toolbar'],
                treegrid: ['filter'],
                gantt: ['tooltip'],
                chart: ['Export', 'Zoom']
            };
            var component = this.getModuleName();
            if (this.requiredModules && (!ignoredComponents["" + component] || ignoredComponents["" + component] !== 'all')) {
                var modulesRequired = this.requiredModules();
                for (var _i = 0, _a = this.moduleLoader.getNonInjectedModules(modulesRequired); _i < _a.length; _i++) {
                    var module = _a[_i];
                    var moduleName = module.name ? module.name : module.member;
                    if (ignoredComponents["" + component] && ignoredComponents["" + component].indexOf(module.member) !== -1) {
                        continue;
                    }
                    var componentName = component.charAt(0).toUpperCase() + component.slice(1); // To capitalize the component name
                    console.warn("[WARNING] :: Module \"" + moduleName + "\" is not available in " + componentName + " component! You either misspelled the module name or forgot to load it.");
                }
            }
            this.render();
            if (!this.mount) {
                this.trigger('created');
            }
            else {
                this.accessMount();
            }
        }
    }

    injectModules() {
        if (this.injectedModules && this.injectedModules.length) {
            this.moduleLoader.inject(this.requiredModules(), this.injectedModules);
        }
    }

    detectFunction(args) {
        var prop = Object.keys(args);
        if (prop.length) {
            this[prop[0]] = args[prop[0]];
        }
    }

    setPersistData() {
        if (!this.isDestroyed) {
            if (exports.versionBasedStatePersistence) {
                window.localStorage.setItem(this.getModuleName() +
                    this.element.id + this.ej2StatePersistenceVersion, this.getPersistData());
            }
            else {
                window.localStorage.setItem(this.getModuleName() + this.element.id, this.getPersistData());
            }
        }
    }

    addOnPersist(options) {
        var _this = this;
        var persistObj = {};
        for (var _i = 0, options_1 = options; _i < options_1.length; _i++) {
            var key = options_1[_i];
            var objValue = void 0;
            objValue = getValue(key, this);
            if (!isUndefined(objValue)) {
                setValue(key, this.getActualProperties(objValue), persistObj);
            }
        }
        return JSON.stringify(persistObj, function (key, value) {
            return _this.getActualProperties(value);
        });
    }

    getActualProperties(obj) {
        if (obj instanceof ChildProperty) {
            return getValue('properties', obj);
        }
        else {
            return obj;
        }
    }
}

/**
 * Module loading operations
 */
const MODULE_SUFFIX = 'Module';

class ModuleLoader {
    constructor(parent) {
        this.loadedModules = [];
        this.parent = parent;
    }

    /**
     * Inject required modules in component library
     *
     * @returns {void} ?
     * @param {ModuleDeclaration[]} requiredModules - Array of modules to be required
     * @param {Function[]} moduleList - Array of modules to be injected from sample side
     */
    inject(requiredModules, moduleList) {
        const reqLength = requiredModules.length;
        if (reqLength === 0) {
            this.clean();
            return;
        }
        if (this.loadedModules.length) {
            this.clearUnusedModule(requiredModules);
        }
        for (let i = 0; i < reqLength; i++) {
            const modl = requiredModules[parseInt(i.toString(), 10)];
            for (let _i = 0, moduleList_1 = moduleList; _i < moduleList_1.length; _i++) {
                const module = moduleList_1[_i];
                const modName = modl.member;
                if (module && module.prototype.getModuleName() === modl.member && !this.isModuleLoaded(modName)) {
                    const moduleObject = createInstance(module, modl.args);
                    const memberName = this.getMemberName(modName);
                    if (modl.isProperty) {
                        setValue(memberName, module, this.parent);
                    }
                    else {
                        setValue(memberName, moduleObject, this.parent);
                    }
                    let loadedModule = modl;
                    loadedModule.member = memberName;
                    this.loadedModules.push(loadedModule);
                }
            }
        }
    }

    /**
     * To remove the created object while destroying the control
     *
     * @returns {void}
     */
    clean() {
        for (let _i = 0, _a = this.loadedModules; _i < _a.length; _i++) {
            const modules = _a[_i];
            if (!modules.isProperty) {
                getValue(modules.member, this.parent).destroy();
            }
        }
        this.loadedModules = [];
    }

    /**
     * Returns the array of modules that are not loaded in the component library.
     *
     * @param {ModuleDeclaration[]} requiredModules - Array of modules to be required
     * @returns {ModuleDeclaration[]} ?
     * @private
     */
    getNonInjectedModules(requiredModules) {
        var _this = this;
        return requiredModules.filter(function (module) { return !_this.isModuleLoaded(module.member); });
    };
    /**
     * Removes all unused modules
     *
     * @param {ModuleDeclaration[]} moduleList ?
     * @returns {void} ?
     */
    clearUnusedModule(moduleList) {
        var _this = this;
        const usedModules = moduleList.map(function (arg) { return _this.getMemberName(arg.member); });
        const removableModule = this.loadedModules.filter(function (module) {
            return usedModules.indexOf(module.member) === -1;
        });
        for (let _i = 0, removableModule_1 = removableModule; _i < removableModule_1.length; _i++) {
            const mod = removableModule_1[_i];
            if (!mod.isProperty) {
                getValue(mod.member, this.parent).destroy();
            }
            this.loadedModules.splice(this.loadedModules.indexOf(mod), 1);
            deleteObject(this.parent, mod.member);
        }
    }

    /**
     * To get the name of the member.
     *
     * @param {string} name ?
     * @returns {string} ?
     */
    getMemberName(name) {
        return name[0].toLowerCase() + name.substring(1) + MODULE_SUFFIX;
    }

    /**
     * Returns boolean based on whether the module specified is loaded or not
     *
     * @param {string} modName ?
     * @returns {boolean} ?
     */
    isModuleLoaded(modName) {
        for (let _i = 0, _a = this.loadedModules; _i < _a.length; _i++) {
            const mod = _a[_i];
            if (mod.member === this.getMemberName(modName)) {
                return true;
            }
        }
        return false;
    }
}

class Observer {
    constructor(context) {
        this.ranArray = [];
        this.boundedEvents = {};
        if (isNullOrUndefined(context)) {
            return;
        }
        this.context = context;
    }

    /**
     * To attach handler for given property in current context.
     *
     * @param {string} property - specifies the name of the event.
     * @param {Function} handler - Specifies the handler function to be called while event notified.
     * @param {Object} context - Specifies the context binded to the handler.
     * @param {string} id - specifies the random generated id.
     * @returns {void}
     */
    on(property, handler, context, id) {
        if (isNullOrUndefined(handler)) {
            return;
        }
        const cntxt = context || this.context;
        if (this.notExist(property)) {
            this.boundedEvents["" + property] = [{ handler: handler, context: cntxt, id: id }];
            return;
        }
        if (!isNullOrUndefined(id)) {
            if (this.ranArray.indexOf(id) === -1) {
                this.ranArray.push(id);
                this.boundedEvents["" + property].push({ handler: handler, context: cntxt, id: id });
            }
        }
        else if (!this.isHandlerPresent(this.boundedEvents["" + property], handler)) {
            this.boundedEvents["" + property].push({ handler: handler, context: cntxt });
        }
    }

    /**
     * To remove handlers from a event attached using on() function.
     *
     * @param {string} property - specifies the name of the event.
     * @param {Function} handler - Optional argument specifies the handler function to be called while event notified.
     * @param {string} id - specifies the random generated id.
     * @returns {void} ?
     */
    off(property, handler, id) {
        if (this.notExist(property)) {
            return;
        }
        const curObject = getValue(property, this.boundedEvents);
        if (handler) {
            for (var i = 0; i < curObject.length; i++) {
                if (id) {
                    if (curObject[parseInt(i.toString(), 10)].id === id) {
                        curObject.splice(i, 1);
                        var indexLocation = this.ranArray.indexOf(id);
                        if (indexLocation !== -1) {
                            this.ranArray.splice(indexLocation, 1);
                        }
                        break;
                    }
                }
                else if (handler === curObject[parseInt(i.toString(), 10)].handler) {
                    curObject.splice(i, 1);
                    break;
                }
            }
        }
        else {
            delete this.boundedEvents["" + property];
        }
    }

    /**
     * To notify the handlers in the specified event.
     *
     * @param {string} property - Specifies the event to be notify.
     * @param {Object} argument - Additional parameters to pass while calling the handler.
     * @param {Function} successHandler - this function will invoke after event successfully triggered
     * @param {Function} errorHandler - this function will invoke after event if it was failure to call.
     * @returns {void} ?
     */
    notify(property, argument, successHandler, errorHandler) {
        if (this.notExist(property)) {
            if (successHandler) {
                successHandler.call(this, argument);
            }
            return;
        }
        if (argument) {
            argument.name = property;
        }
        const blazor = 'Blazor';
        const curObject = getValue(property, this.boundedEvents).slice(0);
        if (window["" + blazor]) {
            return this.blazorCallback(curObject, argument, successHandler, errorHandler, 0);
        }
        else {
            for (let _i = 0, curObject_1 = curObject; _i < curObject_1.length; _i++) {
                const cur = curObject_1[_i];
                cur.handler.call(cur.context, argument);
            }
            if (successHandler) {
                successHandler.call(this, argument);
            }
        }
    }

    blazorCallback(objs, argument, successHandler, errorHandler, index) {
        const isTrigger = index === objs.length - 1;
        if (index < objs.length) {
            const obj = objs[parseInt(index.toString(), 10)];
            const promise = obj.handler.call(obj.context, argument);
            if (promise && typeof promise.then === 'function') {
                if (!successHandler) {
                    return promise;
                }
                promise.then(function (data) {
                    data = typeof data === 'string' && this.isJson(data) ? JSON.parse(data, this.dateReviver) : data;
                    extend(argument, argument, data, true);
                    if (successHandler && isTrigger) {
                        successHandler.call(obj.context, argument);
                    }
                    else {
                        return this.blazorCallback(objs, argument, successHandler, errorHandler, index + 1);
                    }
                }).catch(function (data) {
                    if (errorHandler) {
                        errorHandler.call(obj.context, typeof data === 'string' &&
                            this.isJson(data) ? JSON.parse(data, this.dateReviver) : data);
                    }
                });
            }
            else if (successHandler && isTrigger) {
                successHandler.call(obj.context, argument);
            }
            else {
                return this.blazorCallback(objs, argument, successHandler, errorHandler, index + 1);
            }
        }
    }

    dateReviver(key, value) {
        const dPattern = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/;
        if (typeof value === 'string' && value.match(dPattern) !== null) {
            return (new Date(value));
        }
        return (value);
    }

    isJson(value) {
        try {
            JSON.parse(value);
        }
        catch (e) {
            return false;
        }
        return true;
    }

    /**
     * To destroy handlers in the event
     *
     * @returns {void} ?
     */
    destroy() {
        this.boundedEvents = this.context = undefined;
    }

    /**
     * Returns if the property exists.
     *
     * @param {string} prop ?
     * @returns {boolean} ?
     */
    notExist(prop) {
        return this.boundedEvents.hasOwnProperty(prop) === false || this.boundedEvents[prop].length <= 0;
    }

    /**
     * Returns if the handler is present.
     *
     * @param {BoundOptions[]} boundedEvents ?
     * @param {Function} handler ?
     * @returns {boolean} ?
     */
    isHandlerPresent(boundedEvents, handler) {
        for (let _i = 0, boundedEvents_1 = boundedEvents; _i < boundedEvents_1.length; _i++) {
            const cur = boundedEvents_1[_i];
            if (cur.handler === handler) {
                return true;
            }
        }
        return false;
    }
}

/**
    * Specifies the observer used for external change detection.
    */
var onIntlChange = new Observer();

/**
 * Checks if a specific javascript namespace is available globally.
 *
 * @param {string} namespace - The namespace to check (e.g. 'sfBlazorToolkit.Toolbar').
 * @returns {boolean} True if available, otherwise false.
 */
function isAvailable(namespace) {
    if (!namespace) return false;
    var parts = namespace.split('.');
    var current = window;
    for (var i = 0; i < parts.length; i++) {
        if (current === null || typeof current[parts[i]] === 'undefined') {
            return false;
        }
        current = current[parts[i]];
    }
    return true;
}

/**
 * Set an item in localStorage with safety checks.
 *
 * @param {string} key - The storage key to set.
 * @param {string} value - The string value to store.
 * @returns {void}
 */
function setLocalStorageItem(key, value) {
    if (!key) return;
    window.localStorage.setItem(key, value);
}

/**
 * Get an item from localStorage with safety checks.
 *
 * @param {string} key - The storage key to retrieve.
 * @returns {string|null} The stored string value, or null if `key` is falsy.
 */
function getLocalStorageItem(key) {
    if (!key) return null;
    return window.localStorage.getItem(key);
}

// Expose API

function select(selector, context) {
    return (context || document).querySelector(selector);
}

function selectAll(selector, context) {
    return (context || document).querySelectorAll(selector);
}

function attributes(element, attrs) {
    for (var key in attrs) {
        element.setAttribute(key, attrs[key]);
    }
}

function remove(element) {
    detach(element);
}



const baseExports = {
    select,
    selectAll,
    attributes,
    remove,

    EventHandler,
    KeyboardEvents,
    setCompInstance,
    getCompInstance,
    disposeWindowsInstance,
    isDevice,
    isNullOrUndefined,
    Browser,
    closest,
    matches,
    extend,
    createElement,
    addClass,
    removeClass,
    setStyleAttribute,
    formatUnit,
    detach,
    getUniqueID,
    isVisible,
    getLocalStorageItem,
    setLocalStorageItem,
    isAvailable,
    Property,
    ChildProperty,
    Component,
    Complex,
    Event1,
    merge,
    remove,
    NotifyPropertyChanges,    
    Internationalization,
    Base
};

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.base = baseExports;

export {
    select,
    selectAll,
    attributes,
    remove,

    EventHandler,
    KeyboardEvents,
    setCompInstance,
    getCompInstance,
    disposeWindowsInstance,
    isDevice,
    isNullOrUndefined,
    Browser,
    closest,
    matches,
    extend,
    createElement,
    addClass,
    removeClass,
    setStyleAttribute,
    formatUnit,
    detach,
    getUniqueID,
    isVisible,
    getLocalStorageItem,
    setLocalStorageItem,
    isAvailable
};
