var ARROWLEFT = 'ArrowLeft';
var ARROWRIGHT = 'ArrowRight';
var ARROWUP = 'ArrowUp';
var ARROWDOWN = 'ArrowDown';
var TAB = 'Tab';
var SHIFTTAB = 'shiftTab';
var END = 'End';
var HOME = 'Home';
var DELETE = 'Delete';
var latnDigits = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
var datePartMap = { y: 'year', M: 'month', L: 'month', d: 'day', H: 'hour', h: 'hour', m: 'minute', s: 'second', a: 'designator' };
var timeSetMap = { hour: 'setHours', minute: 'setMinutes', second: 'setSeconds', month: 'setMonth', day: 'setDate' };
var isNil = function (v) { return sfBlazorToolkit.base.isNullOrUndefined(v); };
var dateParserModule = (function () {
    var reverseObj = function (obj) {
        var res = {};
        for (var key in obj) { if (obj.hasOwnProperty(key)) res[obj[key]] = key; }
        return res;
    };

    return {
        parseDate: function (value, format, culture) {
            var digits = culture.digits || latnDigits;
            var numMap = reverseObj(digits);
            var numRegex = '[' + digits[0] + '-' + digits[digits.length - 1] + ']';
            var parseOptions = { evalPos: {} }, regexStr = '', pos = 1;
            var tokens = format.match(/([a-z])\1*|'([^']|'')+'|''|./gi) || [];

            for (var i = 0; i < tokens.length; i++) {
                var token = tokens[i], char = token[0], key = datePartMap[char], isNum = false;
                if (key) {
                    if ((char === 'M' || char === 'L') && token.length > 2) {
                        var mData = culture.months[token.length > 3 ? 'wide' : 'abbreviated'];
                        parseOptions[key] = reverseObj(mData);
                        regexStr += '(' + Object.keys(parseOptions[key]).join('|') + ')';
                    } else if (char === 'a') {
                        parseOptions[key] = reverseObj(culture.dayPeriods);
                        regexStr += '(' + Object.keys(parseOptions[key]).join('|') + ')';
                    } else {
                        isNum = true;
                        regexStr += '(' + numRegex + (char === 'y' ? '{' + token.length + ',}' : '+') + ')';
                    }
                    parseOptions.evalPos[key] = { isNum: isNum, pos: pos++ };
                } else {
                    regexStr += token[0] === "'" ? token.replace(/'/g, '') : '[\\D\\s]';
                }
            }

            var matches = value.match(new RegExp('^' + regexStr + '$', 'i'));
            if (!matches) return null;

            var resDate = new Date(); resDate.setMilliseconds(0);
            var parts = { hour: 0, minute: 0, second: 0 };
            for (var p in parseOptions.evalPos) {
                var opt = parseOptions.evalPos[p], m = matches[opt.pos];
                if (opt.isNum) {
                    parts[p] = +m.replace(new RegExp('[' + digits.join('') + ']', 'g'), function (s) { return numMap[s]; });
                } else {
                    parts[p] = parseOptions[p][m];
                }
            }

            if (parts.year !== undefined) {
                if ((parts.year + '').length <= 2) parts.year += Math.floor(new Date().getFullYear() / 100) * 100;
                resDate.setFullYear(parts.year);
            }

            for (var k in timeSetMap) {
                var val = parts[k];
                if (val === undefined) {
                    if (k === 'day') resDate.setDate(1);
                    continue;
                }
                if (k === 'month') {
                    val -= 1; var d = resDate.getDate(); resDate.setDate(1); resDate.setMonth(val);
                    var last = new Date(resDate.getFullYear(), val + 1, 0).getDate();
                    resDate.setDate(d < last ? d : last);
                } else resDate[timeSetMap[k]](val);
            }
            return resDate;
        }
    };
})();
var MaskedDateTime = /** @class */ (function () {
    function MaskedDateTime() {
        this.mask = '';
        this.hiddenMask = '';
        this.validCharacters = 'dMyhmHts';
        this.isDayPart = false;
        this.isMonthPart = false;
        this.isYearPart = false;
        this.isHourPart = false;
        this.isMinutePart = false;
        this.isSecondsPart = false;
        this.isMilliSecondsPart = false;
        this.monthCharacter = '';
        this.periodCharacter = '';
        this.isHiddenMask = false;
        this.isComplete = false;
        this.isNavigate = false;
        this.navigated = false;
        this.formatRegex = /E{3,4}|d{1,4}|M{1,4}|y{1,4}|H{1,2}|h{1,2}|m{1,2}|f{1,7}|FF|t{1,2}|s{1,2}|z{1,4}|'[^']*'|'[^']*'/g;
        this.isDeletion = false;
        this.isShortYear = false;
        this.isDeleteKey = false;
        this.isDateZero = false;
        this.isMonthZero = false;
        this.isYearZero = false;
        this.isLeadingZero = false;
        this.isPastedValue = false;
        this.dayTypeCount = 0;
        this.monthTypeCount = 0;
        this.hourTypeCount = 0;
        this.minuteTypeCount = 0;
        this.secondTypeCount = 0;
    }
    //This method handles keyboard navigations.
    MaskedDateTime.prototype.keydownHandler = function (e) {
        if (!this.options.readonly) {
            switch (e.code) {
                case ARROWLEFT:
                case ARROWRIGHT:
                case ARROWUP:
                case ARROWDOWN:
                case HOME:
                case END:
                case DELETE:
                    {
                        if (e.code !== DELETE) {
                            e.preventDefault();
                        }
                        if (!this.isPopupOpen) {
                            this.maskKeydownHandler(e);
                        }
                    }
                    break;
                case TAB:
                case SHIFTTAB:
                    {
                        this.maskKeydownHandler(e);
                    }
                    break;
            }
        }
    };
    MaskedDateTime.prototype.clearHandler = function () {
        this.isDayPart = this.isMonthPart = this.isYearPart = this.isHourPart = this.isMinutePart = this.isSecondsPart = false;
        this.updateValue();
    };
    // Updates the current value.
    MaskedDateTime.prototype.updateValue = function () {
        this.monthCharacter = this.periodCharacter = '';
        var inputValue = this.dateformat.replace(this.formatRegex, this.formatCheck());
        this._updateHidden();
        this.previousHiddenMask = this.hiddenMask;
        this.previousValue = inputValue;
    };

    MaskedDateTime.prototype._updateHidden = function () {
        this.isHiddenMask = true;
        this.hiddenMask = this.dateformat.replace(this.formatRegex, this.formatCheck());
        this.isHiddenMask = false;
    };

    MaskedDateTime.prototype._computeInput = function () {
        var v = this.dateformat.replace(this.formatRegex, this.formatCheck());
        this._updateHidden();
        this.previousHiddenMask = this.hiddenMask;
        this.previousValue = v;
        return v;
    };
    // checks dates prefix 0 values
    MaskedDateTime.prototype.zeroCheck = function (isZero, isDayPart, resultValue) {
        return (isZero && !isDayPart) ? '0' : resultValue;
    };
    // checks date and time values within the limit
    MaskedDateTime.prototype.roundOff = function (val, count) {
        var s = val + '';
        return s.length >= count ? s : Array(count - s.length + 1).join('0') + s;
    };
    // creates mask format values
    MaskedDateTime.prototype.formatCheck = function () {
        var proxy = this;
        function formatValueSpecifier(formattext) {
            var result;
            var daysAbbreviated = proxy.dayAbbreviatedName;
            var dayKeyAbbreviated = Object.keys(daysAbbreviated);
            var daysWide = (proxy.dayName);
            var dayKeyWide = Object.keys(daysWide);
            var monthAbbreviated = (proxy.monthAbbreviatedName);
            var monthWide = (proxy.monthName);
            var periodString = (proxy.dayPeriod);
            var periodkeys = Object.keys(periodString);
            var milliseconds;
            switch (formattext) {
                case 'd':
                    result = proxy.isDayPart ? proxy.maskDateValue.getDate().toString() : proxy.maskPlaceholderDictionary['Day'].toString();
                    result = proxy.zeroCheck(proxy.isDateZero, proxy.isDayPart, result);
                    if (proxy.dayTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.dayTypeCount = 0;
                    }
                    break;
                case 'dd':
                    result = proxy.isDayPart ? proxy.roundOff(proxy.maskDateValue.getDate(), 2) : proxy.maskPlaceholderDictionary['Day'].toString();
                    result = proxy.zeroCheck(proxy.isDateZero, proxy.isDayPart, result);
                    if (proxy.dayTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.dayTypeCount = 0;
                    }
                    break;
                case 'ddd':
                    result = proxy.isDayPart && proxy.isMonthPart && proxy.isYearPart ?
                        daysAbbreviated[dayKeyAbbreviated[proxy.maskDateValue.getDay()]].toString() :
                        proxy.maskPlaceholderDictionary['DayOfWeek'].toString();
                    break;
                case 'dddd':
                    result = proxy.isDayPart && proxy.isMonthPart && proxy.isYearPart ?
                        daysWide[dayKeyWide[proxy.maskDateValue.getDay()]].toString() :
                        proxy.maskPlaceholderDictionary['DayOfWeek'].toString();
                    break;
                case 'M':
                    result = proxy.isMonthPart ? (proxy.maskDateValue.getMonth() + 1).toString() :
                        proxy.maskPlaceholderDictionary['Month'].toString();
                    result = proxy.zeroCheck(proxy.isMonthZero, proxy.isMonthPart, result);
                    if (proxy.monthTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.monthTypeCount = 0;
                    }
                    break;
                case 'MM':
                    result = proxy.isMonthPart ? proxy.roundOff(proxy.maskDateValue.getMonth() + 1, 2) :
                        proxy.maskPlaceholderDictionary['Month'].toString();
                    result = proxy.zeroCheck(proxy.isMonthZero, proxy.isMonthPart, result);
                    if (proxy.monthTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.monthTypeCount = 0;
                    }
                    break;
                case 'MMM':
                    result = proxy.isMonthPart ? monthAbbreviated[proxy.maskDateValue.getMonth()] :
                        proxy.maskPlaceholderDictionary['Month'].toString();
                    break;
                case 'MMMM':
                    result = proxy.isMonthPart ? monthWide[proxy.maskDateValue.getMonth()] :
                        proxy.maskPlaceholderDictionary['Month'].toString();
                    break;
                case 'yy':
                    result = proxy.isYearPart ? proxy.roundOff(proxy.maskDateValue.getFullYear() % 100, 2) :
                        proxy.maskPlaceholderDictionary['Year'].toString();
                    result = proxy.zeroCheck(proxy.isYearZero, proxy.isYearPart, result);
                    break;
                case 'y':
                case 'yyy':
                case 'yyyy':
                    result = proxy.isYearPart ? proxy.roundOff(proxy.maskDateValue.getFullYear(), 4) :
                        proxy.maskPlaceholderDictionary['Year'].toString();
                    result = proxy.zeroCheck(proxy.isYearZero, proxy.isYearPart, result);
                    break;
                case 'h':
                    result = proxy.isHourPart ? (proxy.maskDateValue.getHours() % 12 || 12).toString() :
                        proxy.maskPlaceholderDictionary['Hour'].toString();
                    if (proxy.hourTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.hourTypeCount = 0;
                    }
                    break;
                case 'hh':
                    result = proxy.isHourPart ? proxy.roundOff(proxy.maskDateValue.getHours() % 12 || 12, 2) :
                        proxy.maskPlaceholderDictionary['Hour'].toString();
                    if (proxy.hourTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.hourTypeCount = 0;
                    }
                    break;
                case 'H':
                    result = proxy.isHourPart ? proxy.maskDateValue.getHours().toString() :
                        proxy.maskPlaceholderDictionary['Hour'].toString();
                    if (proxy.hourTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.hourTypeCount = 0;
                    }
                    break;
                case 'HH':
                    result = proxy.isHourPart ? proxy.roundOff(proxy.maskDateValue.getHours(), 2) :
                        proxy.maskPlaceholderDictionary['Hour'].toString();
                    if (proxy.hourTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.hourTypeCount = 0;
                    }
                    break;
                case 'm':
                    result = proxy.isMinutePart ? proxy.maskDateValue.getMinutes().toString() :
                        proxy.maskPlaceholderDictionary['Minute'].toString();
                    if (proxy.minuteTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.minuteTypeCount = 0;
                    }
                    break;
                case 'mm':
                    result = proxy.isMinutePart ? proxy.roundOff(proxy.maskDateValue.getMinutes(), 2) :
                        proxy.maskPlaceholderDictionary['Minute'].toString();
                    if (proxy.minuteTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.minuteTypeCount = 0;
                    }
                    break;
                case 's':
                    result = proxy.isSecondsPart ? proxy.maskDateValue.getSeconds().toString() :
                        proxy.maskPlaceholderDictionary['Second'].toString();
                    if (proxy.secondTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.secondTypeCount = 0;
                    }
                    break;
                case 'ss':
                    result = proxy.isSecondsPart ? proxy.roundOff(proxy.maskDateValue.getSeconds(), 2) :
                        proxy.maskPlaceholderDictionary['Second'].toString();
                    if (proxy.secondTypeCount === 2) {
                        proxy.isNavigate = true;
                        proxy.secondTypeCount = 0;
                    }
                    break;
                case 'f':
                    result = Math.floor(proxy.maskDateValue.getMilliseconds() / 100).toString();
                    break;
                case 'FF':
                case 'ff':
                    milliseconds = proxy.maskDateValue.getMilliseconds();
                    if (proxy.maskDateValue.getMilliseconds() > 99) {
                        milliseconds = Math.floor(proxy.maskDateValue.getMilliseconds() / 10);
                    }
                    result = proxy.roundOff(milliseconds, 2);
                    break;
                case 'fff':
                    result = proxy.roundOff(proxy.maskDateValue.getMilliseconds(), 3);
                    break;
                case 'ffff':
                    result = proxy.roundOff(proxy.maskDateValue.getMilliseconds(), 4);
                    break;
                case 'fffff':
                    result = proxy.roundOff(proxy.maskDateValue.getMilliseconds(), 5);
                    break;
                case 'ffffff':
                    result = proxy.roundOff(proxy.maskDateValue.getMilliseconds(), 6);
                    break;
                case 't':
                case 'tt':
                    result = proxy.maskDateValue.getHours() < 12 ? periodString[periodkeys[0]] : periodString[periodkeys[1]];
                    break;
                case 'z':
                case 'zz':
                    result = proxy.offset.substring(0, 3);
                    break;
                case 'zzz':
                case 'zzzz':
                    result = proxy.offset;
                    break;
            }
            if (formattext === 'ddd') formattext = 'EEE';
            if (formattext === 'dddd') formattext = 'EEEE';
            result = result !== undefined ? result : formattext.slice(1, formattext.length - 1);
            if (proxy.isHiddenMask) return formattext[0].repeat(result.length);
            return result;
        }
        return formatValueSpecifier;
    };
    // checks the difference in inputvalue
    MaskedDateTime.prototype.differenceCheck = function () {
        var inputElement = this.element;
        var start = inputElement.selectionStart;
        var inputValue = inputElement.value;
        var previousVal = this.previousValue.substring(0, start + this.previousValue.length - inputValue.length);
        var newVal = inputValue.substring(0, start);
        var newDateValue = !isNil(newVal) && newVal.length > 0 ?
            new Date(+this.maskDateValue) : new Date(this.options.valueString);
        var maxDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth() + 1, 0).getDate();
        if (previousVal.indexOf(newVal) === 0 && (newVal.length === 0 ||
            this.previousHiddenMask[newVal.length - 1] !== this.previousHiddenMask[newVal.length])) {
            for (var i = newVal.length; i < previousVal.length; i++) {
                if (this.previousHiddenMask[i] !== '' &&
                    this.validCharacters.indexOf(this.previousHiddenMask[i]) >= 0) {
                    this.isDeletion = this.handleDeletion(this.previousHiddenMask[i], false);
                }
            }
            if (this.isDeletion) {
                return;
            }
        }
        if (this.componentName === 'DatePicker' && this.isPastedValue) {
            var formattedDate = this.formatMaskedDate();
            if (!isNil(formattedDate) || formattedDate === 'Invalid Date' || formattedDate === null) {
                return;
            }
        }
        switch (this.previousHiddenMask[start - 1]) {
            case 'd':
                {
                    var date = (this.isDayPart && newDateValue.getDate().toString().length < 2 && !this.isPersist() ?
                        newDateValue.getDate() * 10 : 0) + parseInt(newVal[start - 1], 10);
                    this.isDateZero = (newVal[start - 1] === '0');
                    this.isFocused = this.isFocused ? false : this.isFocused;
                    this.navigated = this.navigated ? false : this.navigated;
                    if (isNaN(date)) {
                        return;
                    }
                    for (var i = 0; date > maxDate; i++) {
                        date = parseInt(date.toString().slice(1), 10);
                    }
                    if (date >= 1) {
                        newDateValue.setDate(date);
                        this.isNavigate = date.toString().length === 2;
                        this.previousDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth(), newDateValue.getDate());
                        if (newDateValue.getMonth() !== this.maskDateValue.getMonth()) {
                            return;
                        }
                        this.isDayPart = true;
                        this.dayTypeCount = this.dayTypeCount + 1;
                    }
                    else {
                        this.isDayPart = false;
                        this.dayTypeCount = this.isDateZero ? this.dayTypeCount + 1 : this.dayTypeCount;
                    }
                    break;
                }
            case 'M':
                {
                    var month = void 0;
                    if (newDateValue.getMonth().toString().length < 2 && !this.isPersist()) {
                        month = (this.isMonthPart ? (newDateValue.getMonth() + 1) * 10 : 0) + parseInt(newVal[start - 1], 10);
                    }
                    else {
                        month = parseInt(newVal[start - 1], 10);
                    }
                    this.isFocused = this.isFocused ? false : this.isFocused;
                    this.navigated = this.navigated ? false : this.navigated;
                    this.isMonthZero = (newVal[start - 1] === '0');
                    if (!isNaN(month)) {
                        while (month > 12) {
                            var monthvalue = month;
                            month = parseInt(month.toString().slice(1), 10);
                            if (month === 0) {
                                month = parseInt(monthvalue.toString().slice(0, 1), 10);
                            }
                        }
                        if (month >= 1) {
                            newDateValue.setMonth(month - 1);
                            if (month >= 10 || month === 1) {
                                if (this.isLeadingZero && month === 1) {
                                    this.isNavigate = month.toString().length === 1;
                                    this.isLeadingZero = false;
                                }
                                else {
                                    this.isNavigate = month.toString().length === 2;
                                }
                            }
                            else {
                                this.isNavigate = month.toString().length === 1;
                            }
                            if (newDateValue.getMonth() !== month - 1) {
                                newDateValue.setDate(1);
                                newDateValue.setMonth(month - 1);
                            }
                            if (this.isDayPart) {
                                var previousMaxDate = new Date(this.previousDate.getFullYear(), this.previousDate.getMonth() + 1, 0).getDate();
                                var currentMaxDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth() + 1, 0).getDate();
                                if (this.previousDate.getDate() === previousMaxDate && currentMaxDate <= previousMaxDate) {
                                    newDateValue.setDate(currentMaxDate);
                                }
                            }
                            this.previousDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth(), newDateValue.getDate());
                            this.isMonthPart = true;
                            this.monthTypeCount = this.monthTypeCount + 1;
                        }
                        else {
                            newDateValue.setMonth(0);
                            this.isLeadingZero = true;
                            this.isMonthPart = false;
                            this.monthTypeCount = this.isMonthZero ? this.monthTypeCount + 1 : this.monthTypeCount;
                        }
                    }
                    else {
                        var monthString = (this.monthName);
                        var monthValue = Object.keys(monthString);
                        this.monthCharacter += newVal[start - 1].toLowerCase();
                        while (this.monthCharacter.length > 0) {
                            var i = 0;
                            for (var _i = 0, monthValue_1 = monthValue; _i < monthValue_1.length; _i++) {
                                var months = monthValue_1[_i];
                                if (monthString[i].toLowerCase().indexOf(this.monthCharacter) === 0) {
                                    newDateValue.setMonth(i);
                                    this.isMonthPart = true;
                                    this.maskDateValue = newDateValue;
                                    return;
                                }
                                i++;
                            }
                            this.monthCharacter = this.monthCharacter.substring(1, this.monthCharacter.length);
                        }
                    }
                    break;
                }
            case 'y':
                {
                    var year = (this.isYearPart && (newDateValue.getFullYear().toString().length < 4 && !this.isShortYear) ?
                        newDateValue.getFullYear() * 10 : 0) + parseInt(newVal[start - 1], 10);
                    var yearValue = (this.dateformat.match(/y/g) || []).length;
                    this.isShortYear = false;
                    this.isYearZero = (newVal[start - 1] === '0');
                    if (isNaN(year)) {
                        return;
                    }
                    while (year > 9999) {
                        year = parseInt(year.toString().slice(1), 10);
                    }
                    if (year < 1) {
                        this.isYearPart = false;
                    }
                    else {
                        newDateValue.setFullYear(year);
                        if (year.toString().length === yearValue) {
                            this.isNavigate = true;
                        }
                        this.previousDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth(), newDateValue.getDate());
                        this.isYearPart = true;
                    }
                    break;
                }
            case 'h':
                {
                    this.hour = (this.isHourPart && (newDateValue.getHours() % 12 || 12).toString().length < 2 && !this.isPersist() ?
                        (newDateValue.getHours() % 12 || 12) * 10 : 0) + parseInt(newVal[start - 1], 10);
                    this.isFocused = this.isFocused ? false : this.isFocused;
                    this.navigated = this.navigated ? false : this.navigated;
                    if (isNaN(this.hour)) {
                        return;
                    }
                    while (this.hour > 12) {
                        this.hour = parseInt(this.hour.toString().slice(1), 10);
                    }
                    newDateValue.setHours(Math.floor(newDateValue.getHours() / 12) * 12 + (this.hour % 12));
                    this.isNavigate = this.hour.toString().length === 2;
                    this.isHourPart = true;
                    this.hourTypeCount = this.hourTypeCount + 1;
                    break;
                }
            case 'H':
                {
                    this.hour = (this.isHourPart && newDateValue.getHours().toString().length < 2 && !this.isPersist() ?
                        newDateValue.getHours() * 10 : 0) + parseInt(newVal[start - 1], 10);
                    this.isFocused = this.isFocused ? false : this.isFocused;
                    this.navigated = this.navigated ? false : this.navigated;
                    if (isNaN(this.hour)) {
                        return;
                    }
                    for (var i = 0; this.hour > 23; i++) {
                        this.hour = parseInt(this.hour.toString().slice(1), 10);
                    }
                    newDateValue.setHours(this.hour);
                    this.isNavigate = this.hour.toString().length === 2;
                    this.isHourPart = true;
                    this.hourTypeCount = this.hourTypeCount + 1;
                    break;
                }
            case 'm':
                {
                    var minutes = (this.isMinutePart && newDateValue.getMinutes().toString().length < 2 && !this.isPersist() ?
                        newDateValue.getMinutes() * 10 : 0) + parseInt(newVal[start - 1], 10);
                    this.isFocused = this.isFocused ? false : this.isFocused;
                    this.navigated = this.navigated ? false : this.navigated;
                    if (isNaN(minutes)) {
                        return;
                    }
                    for (var i = 0; minutes > 59; i++) {
                        minutes = parseInt(minutes.toString().slice(1), 10);
                    }
                    newDateValue.setMinutes(minutes);
                    this.isNavigate = minutes.toString().length === 2;
                    this.isMinutePart = true;
                    this.minuteTypeCount = this.minuteTypeCount + 1;
                    break;
                }
            case 's':
                {
                    var seconds = (this.isSecondsPart && newDateValue.getSeconds().toString().length < 2 && !this.isPersist() ?
                        newDateValue.getSeconds() * 10 : 0) + parseInt(newVal[start - 1], 10);
                    this.isFocused = this.isFocused ? false : this.isFocused;
                    this.navigated = this.navigated ? false : this.navigated;
                    if (isNaN(seconds)) {
                        return;
                    }
                    for (var i = 0; seconds > 59; i++) {
                        seconds = parseInt(seconds.toString().slice(1), 10);
                    }
                    newDateValue.setSeconds(seconds);
                    this.isNavigate = seconds.toString().length === 2;
                    this.isSecondsPart = true;
                    this.secondTypeCount = this.secondTypeCount + 1;
                    break;
                }
            case 't':
                {
                    this.periodCharacter += newVal[start - 1].toLowerCase();
                    var periodString = (this.dayPeriod);
                    var periodkeys = Object.keys(periodString);
                    for (var i = 0; this.periodCharacter.length > 0; i++) {
                        if ((periodString[periodkeys[0]].toLowerCase().indexOf(this.periodCharacter) === 0 && newDateValue.getHours() >= 12) ||
                            (periodString[periodkeys[1]].toLowerCase().indexOf(this.periodCharacter) === 0 && newDateValue.getHours() < 12)) {
                            newDateValue.setHours((newDateValue.getHours() + 12) % 24);
                            this.maskDateValue = newDateValue;
                        }
                        this.periodCharacter = this.periodCharacter.substring(1, this.periodCharacter.length);
                    }
                    break;
                }
        }
        this.maskDateValue = newDateValue;
    };
    //This method handles keyboard navigations
    MaskedDateTime.prototype.maskKeydownHandler = function (args) {
        var inputElement = this.element;
        this.dayTypeCount = this.monthTypeCount = this.hourTypeCount = this.minuteTypeCount = this.secondTypeCount = 0;
        if (args.key === DELETE) {
            this.isDeleteKey = true;
            return;
        }
        if ((!args.altKey && !args.ctrlKey) && (args.key === ARROWLEFT || args.key === ARROWRIGHT ||
            args.key === SHIFTTAB || args.key === TAB || args.action === SHIFTTAB ||
            args.key === END || args.key === HOME)) {
            var start = inputElement.selectionStart;
            var end = inputElement.selectionEnd;
            var length_1 = inputElement.value.length;
            if ((start === 0 && end === length_1) && ((args.key === TAB && !args.shiftKey) || (args.key === TAB && args.shiftKey))) {
                var index = (args.key === TAB && args.shiftKey) ? end : 0;
                inputElement.selectionStart = inputElement.selectionEnd = index;
            }
            if (args.key === END || args.key === HOME) {
                var range = args.key === END ? length_1 : 0;
                inputElement.selectionStart = inputElement.selectionEnd = range;
            }
            if ((args.key === ARROWLEFT || args.key === ARROWRIGHT) && start === 0 && end === length_1) {
                if (args.key === ARROWLEFT) {
                    var val = inputElement.selectionEnd;
                    for (var i = val, j = val - 1; i < this.hiddenMask.length || j >= 0; i++, j--) {
                        if (i < this.hiddenMask.length && this.validCharacters.indexOf(this.hiddenMask[i]) !== -1) {
                            this.setSelection(this.hiddenMask[i]);
                            return;
                        }
                        if (j >= 0 && this.validCharacters.indexOf(this.hiddenMask[j]) !== -1) {
                            this.setSelection(this.hiddenMask[j]);
                            return;
                        }
                    }
                }
                else {
                    this.validCharacterCheck();
                }
                return;
            }
            this.navigateSelection(args.key === ARROWLEFT || (args.shiftKey && args.key === TAB) || args.key === END ? true : false);
        }
        if ((!args.altKey && !args.ctrlKey) && (args.key === ARROWUP || args.key === ARROWDOWN)) {
            var start = inputElement.selectionStart;
            var formatText = '';
            if (this.validCharacters.indexOf(this.hiddenMask[start]) !== -1) {
                formatText = this.hiddenMask[start];
            }
            this.dateAlteration(args.key === ARROWDOWN ? true : false);
            inputElement.value = this._computeInput();
            for (var i = 0; i < this.hiddenMask.length; i++) {
                if (formatText === this.hiddenMask[i]) {
                    start = i;
                    break;
                }
            }
            inputElement.selectionStart = start;
            this.validCharacterCheck();
        }
    };
    //this method handles date and time value increment and decrement
    MaskedDateTime.prototype.dateAlteration = function (isDecrement) {
        var inputElement = this.element;
        var start = inputElement.selectionStart;
        var formatText = '';
        if (this.validCharacters.indexOf(this.hiddenMask[start]) !== -1) {
            formatText = this.hiddenMask[start];
        }
        else {
            return;
        }
        var newDateValue = new Date(this.maskDateValue.getFullYear(), this.maskDateValue.getMonth(), this.maskDateValue.getDate(), this.maskDateValue.getHours(), this.maskDateValue.getMinutes(), this.maskDateValue.getSeconds());
        this.previousDate = new Date(this.maskDateValue.getFullYear(), this.maskDateValue.getMonth(), this.maskDateValue.getDate(), this.maskDateValue.getHours(), this.maskDateValue.getMinutes(), this.maskDateValue.getSeconds());
        var incrementValue = isDecrement ? -1 : 1;
        switch (formatText) {
            case 'd':
                newDateValue.setDate(newDateValue.getDate() + incrementValue);
                break;
            case 'M':
                {
                    var newMonth = newDateValue.getMonth() + incrementValue;
                    newDateValue.setDate(1);
                    newDateValue.setMonth(newMonth);
                    if (this.isDayPart) {
                        var previousMaxDate = new Date(this.previousDate.getFullYear(), this.previousDate.getMonth() + 1, 0).getDate();
                        var currentMaxDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth() + 1, 0).getDate();
                        if (this.previousDate.getDate() === previousMaxDate && currentMaxDate <= previousMaxDate) {
                            newDateValue.setDate(currentMaxDate);
                        }
                        else {
                            newDateValue.setDate(this.previousDate.getDate());
                        }
                    }
                    else {
                        newDateValue.setDate(this.previousDate.getDate());
                    }
                    this.previousDate = new Date(newDateValue.getFullYear(), newDateValue.getMonth(), newDateValue.getDate());
                    break;
                }
            case 'y':
                newDateValue.setFullYear(newDateValue.getFullYear() + incrementValue);
                break;
            case 'H':
            case 'h':
                newDateValue.setHours(newDateValue.getHours() + incrementValue);
                break;
            case 'm':
                newDateValue.setMinutes(newDateValue.getMinutes() + incrementValue);
                break;
            case 's':
                newDateValue.setSeconds(newDateValue.getSeconds() + incrementValue);
                break;
            case 't':
                newDateValue.setHours((newDateValue.getHours() + 12) % 24);
                break;
        }
        this.maskDateValue = newDateValue.getFullYear() > 0 ? newDateValue : this.maskDateValue;
        if (this.validCharacters.indexOf(this.hiddenMask[start]) !== -1) {
            this.handleDeletion(this.hiddenMask[start], true);
        }
    };
    // handle value delete operations
    MaskedDateTime.prototype.handleDeletion = function (format, isSegment) {
        switch (format) {
            case 'd':
                this.isDayPart = isSegment;
                break;
            case 'M':
                this.isMonthPart = isSegment;
                if (!isSegment) {
                    this.maskDateValue.setMonth(0);
                    this.monthCharacter = '';
                }
                break;
            case 'y':
                this.isYearPart = isSegment;
                break;
            case 'H':
            case 'h':
                this.isHourPart = isSegment;
                if (!isSegment) {
                    this.periodCharacter = '';
                }
                break;
            case 'm':
                this.isMinutePart = isSegment;
                break;
            case 's':
                this.isSecondsPart = isSegment;
                break;
            default:
                return false;
        }
        return true;
    };
    //handle persist property
    MaskedDateTime.prototype.isPersist = function () {
        var isPersist = this.isFocused || this.navigated;
        return isPersist;
    };
    //updated the dynamic value changes
    MaskedDateTime.prototype.setDynamicValue = function () {
        this.maskDateValue = new Date(this.options.valueString);
        this.isDayPart = this.isMonthPart = this.isYearPart = this.isHourPart = this.isMinutePart = this.isSecondsPart = true;
        this.updateValue();
        if (!this.options.isBlurred) {
            this.validCharacterCheck();
        }
    };
    // handles the format selection
    MaskedDateTime.prototype.validCharacterCheck = function () {
        var inputElement = this.element;
        var start = inputElement.selectionStart;
        if (this.componentName !== 'TimePicker') {
            if (start === this.hiddenMask.length && this.mask === inputElement.value) {
                start = 0;
            }
        }
        if (this.componentName === 'TimePicker') {
            if (start === this.hiddenMask.length) {
                inputElement.setSelectionRange(0, start);
                return;
            }
            else if (this.isPopupOpen) {
                inputElement.setSelectionRange(0, this.hiddenMask.length);
                return;
            }
        }
        for (var i = start, j = start - 1; i < this.hiddenMask.length || j >= 0; i++, j--) {
            if (i < this.hiddenMask.length && this.validCharacters.indexOf(this.hiddenMask[i]) !== -1) {
                this.setSelection(this.hiddenMask[i]);
                return;
            }
            if (j >= 0 && this.validCharacters.indexOf(this.hiddenMask[j]) !== -1) {
                this.setSelection(this.hiddenMask[j]);
                return;
            }
        }
    };
    //handles the format selection
    MaskedDateTime.prototype.setSelection = function (validChar) {
        var inputElement = this.element;
        var start = -1;
        var end = 0;
        for (var i = 0; i < this.hiddenMask.length; i++) {
            if (this.hiddenMask[i] === validChar) {
                end = i + 1;
                if (start === -1) {
                    start = i;
                }
            }
        }
        if (start < 0) {
            start = 0;
        }
        inputElement.setSelectionRange(start, end);
    };
    MaskedDateTime.prototype.isValidDate = function (dateString) {
        var date = new Date(dateString);
        // Return true if the date is valid, false otherwise
        return !isNaN(date.getTime());
    };
    MaskedDateTime.prototype.isValidTimeFormat = function (inputString, format) {
        if (!inputString || !format)
            return false;
        if (!/[Hh]|mm|ss|[at]/i.test(format))
            return true;
        var timePatterns = [/([0-9]{1,2}):([0-9]{2}):([0-9]{2})\s*(AM|PM|am|pm)?/i, /([0-9]{1,2}):([0-9]{2})\s*(AM|PM|am|pm)?/i,
            /([0-9]{1,2})([0-9]{2})([0-9]{2})/, /([0-9]{1,2})([0-9]{2})/];
        var match = null;
        for (var i = 0; i < timePatterns.length; i++) {
            match = inputString.match(timePatterns[i]);
            if (match) {
                break;
            }
        }
        if (!match) {
            return false;
        }
        var hours = parseInt(match[1], 10);
        var minutes = parseInt(match[2], 10);
        var seconds = match[3] ? parseInt(match[3], 10) : 0;
        var isPeriod = match[4];
        var is24HourFormat = format.indexOf('H') !== -1;
        var isAmPmRequired = format.toLowerCase().indexOf('a') !== -1 || format.toLowerCase().indexOf('tt') !== -1;
        if (is24HourFormat) {
            if (hours < 0 || hours > 23) {
                return false;
            }
        }
        else {
            if (hours < 1 || hours > 12) {
                return false;
            }
            if (isAmPmRequired && !isPeriod) {
                return false;
            }
        }
        if (minutes < 0 || minutes > 59) {
            return false;
        }
        if (seconds < 0 || seconds > 59) {
            return false;
        }
        return true;
    };
    MaskedDateTime.prototype.formatMaskedDate = function () {
        var inputElement = this.element;
        var trimmedInputValue = inputElement.value.trim();
        var hasTimeInFormat = this.dateformat && /[Hh]|mm|ss|[at]/i.test(this.dateformat);
        if (!hasTimeInFormat || trimmedInputValue.length !== this.dateformat.length) {
            return 'Invalid Date';
        }
        var isValidTime = this.isValidTimeFormat(trimmedInputValue, this.dateformat);
        if (!isValidTime && trimmedInputValue.length > 10) {
            return 'Invalid Date';
        }
        var cultureData = {
            months: {
                abbreviated: this.monthAbbreviatedName,
                wide: this.monthName
            },
            dayPeriods: this.dayPeriod
        };
        var parsedDate = dateParserModule.parseDate(trimmedInputValue, this.dateformat, cultureData);
        return !isNil(parsedDate) ? parsedDate : (isValidTime ? trimmedInputValue : 'Invalid Date');
    };
    MaskedDateTime.prototype.maskPasteInputHandler = function () {
        this.isPastedValue = true;
        var inputElement = this.element;
        var formateDateString;
        if (this.componentName === 'DatePicker' && this.isPastedValue) {
            formateDateString = this.formatMaskedDate();
        }
        var inputValue = isNil(formateDateString) ? inputElement.value : formateDateString;
        if (this.isValidDate(inputValue)) {
            this.maskDateValue = new Date(inputValue);
            this.isDayPart = this.isMonthPart = this.isYearPart = this.isHourPart = this.isMinutePart = this.isSecondsPart = true;
            this.updateValue();
            if (!this.options.isBlurred) {
                this.validCharacterCheck();
            }
            this.isPastedValue = false;
            return;
        }
        else {
            this.maskInputHandler();
        }
        this.isPastedValue = false;
    };
    // handle the input values with mask form
    MaskedDateTime.prototype.maskInputHandler = function () {
        var inputElement = this.element;
        var start = inputElement.selectionStart;
        var formatText = '';
        if (this.validCharacters.indexOf(this.hiddenMask[start]) !== -1) {
            formatText = this.hiddenMask[start];
        }
        this.differenceCheck();
        inputElement.value = this._computeInput();
        this.isDateZero = this.isMonthZero = this.isYearZero = false;
        for (var i = 0; i < this.hiddenMask.length; i++) {
            if (formatText === this.hiddenMask[i]) {
                start = i;
                break;
            }
        }
        inputElement.selectionStart = start;
        this.validCharacterCheck();
        if ((this.isNavigate || this.isDeletion) && !this.isDeleteKey) {
            var isbackward = this.isNavigate ? false : true;
            this.isNavigate = this.isDeletion = false;
            this.navigateSelection(isbackward);
        }
        if (this.isDeleteKey) {
            this.isDeletion = false;
        }
        this.isDeleteKey = false;
    };
    // handles the keyboard navigations
    MaskedDateTime.prototype.navigateSelection = function (isbackward) {
        var inputElement = this.element;
        var start = inputElement.selectionStart;
        var end = inputElement.selectionEnd;
        var formatIndex = isbackward ? start - 1 : end;
        this.navigated = true;
        while (formatIndex < this.hiddenMask.length && formatIndex >= 0) {
            if (this.validCharacters.indexOf(this.hiddenMask[formatIndex]) >= 0) {
                this.setSelection(this.hiddenMask[formatIndex]);
                break;
            }
            formatIndex = formatIndex + (isbackward ? -1 : 1);
        }
    };
    // creates mask forms and mask placeholders
    MaskedDateTime.prototype.createMask = function (options) {
        this.options = options;
        this.isDayPart = this.isMonthPart = this.isYearPart = this.isHourPart = this.isMinutePart = this.isSecondsPart = false;
        this.dateformat = this.options.format;
        var inputValue = this._computeInput();
        this.mask = this.previousValue = inputValue;
        if (this.options.value) {
            this.value = this.options.value;
            this.navigated = this.options.navigated;
            this.setDynamicValue();
        }
        inputValue = this._computeInput();
        if (this.options.minMax) {
            this.validCharacterCheck();
        }
        return { 'currentMaskFormat': this.mask, 'inputElementValue': inputValue };
    };
    MaskedDateTime.prototype.updateCurrentValue = function (currentValue) {
        var inputElement = this.element;
        inputElement.value = currentValue;
    };
    MaskedDateTime.prototype.destroy = function () {
        // 1. Remove event listeners
        if (this.element) {
            const events = ["keydown","input","paste","focus","blur"];
            for (const evt of events) {
                const ref = this["_" + evt + "HandlerRef"];
                if (ref) this.element.removeEventListener(evt, ref);
                this["_" + evt + "HandlerRef"] = null;
            }
        }

        // 2. Drop element reference
        this.element = null;

        // 3. Clear ALL instance-owned fields for perfect cleanup
        for (const key in this) {
            if (Object.prototype.hasOwnProperty.call(this, key)) {
                this[key] = null;
            }
        }
    };

    return MaskedDateTime;
}());

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.MaskedDateTime = MaskedDateTime;
