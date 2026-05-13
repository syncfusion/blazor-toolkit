'use strict';

var globalTimeOut = {};
var DEFAULT_FLUENT_WIDTH = 30;
var CLS_SHOWSPIN = 'e-spin-show';
var CLS_HIDESPIN = 'e-spin-hide';
var CLS_FLUENTSPIN = 'e-spin-fabric';
var CLS_SPINWRAP = 'e-spinner-pane';
var CLS_SPINNER_INNER_WRAP = 'e-spinner-inner';
var CLS_SPINCIRCLE = 'e-path-circle';
var CLS_SPINARC = 'e-path-arc';
var CLS_SPINLABEL = 'e-spin-label';
var CLS_SPINTEMPLATE = 'e-spin-template';
var SPINTEMPLATE = null;
var SPINCSSCLASS = null;
/**
 * Create a spinner for the specified target element.
 * ```
 * E.g : renderSpinner({ target: targetElement, width: '34px', label: 'Loading..' });
 * ```
 *
 * @param {SpinnerArgs} args - specifies the args
 * @param {CreateElementArgs} internalCreateElement - specifis the element args
 * @returns {void}
 * @private
 */
function renderSpinner(args, internalCreateElement) {
    if (!args.target) {
        return;
    }
    var radius;
    var makeElement = !sfBlazorToolkit.base.isNullOrUndefined(internalCreateElement) ? internalCreateElement : sfBlazorToolkit.base.createElement;
    // eslint-disable-next-line
    var container = createSpinnerContainer(args.target, makeElement);
    if (!sfBlazorToolkit.base.isNullOrUndefined(args.cssClass)) {
        container.wrap.classList.add(args.cssClass);
    }
    if (!sfBlazorToolkit.base.isNullOrUndefined(args.template) || !sfBlazorToolkit.base.isNullOrUndefined(SPINTEMPLATE)) {
        var template = !sfBlazorToolkit.base.isNullOrUndefined(args.template) ? args.template : SPINTEMPLATE;
        container.wrap.classList.add(CLS_SPINTEMPLATE);
        replaceContent(container.wrap, template, SPINCSSCLASS);
    }
    else {
        var theme = !sfBlazorToolkit.base.isNullOrUndefined(args.type) ? args.type : getTheme(container.wrap);
        var width = !sfBlazorToolkit.base.isNullOrUndefined(args.width) ? args.width : undefined;
        radius = calculateRadius(width, theme);
        setTheme(theme, container.wrap, radius, makeElement);
        if (!sfBlazorToolkit.base.isNullOrUndefined(args.label)) {
            createLabel(container.inner_wrap, args.label, makeElement);
        }
    }
    container.wrap.classList.add(CLS_HIDESPIN);
    container = null;
}
/**
 *
 * @param {HTMLElement} target - specifies the element
 * @param {createElementParams} makeElement - specifies the element
 * @returns {void}
 */
// eslint-disable-next-line
function createSpinnerContainer(target, makeElement) {
    var spinnerContainer = makeElement('div', {});
    var spinnerInnerContainer = makeElement('div', {});
    spinnerContainer.classList.add(CLS_SPINWRAP);
    spinnerInnerContainer.classList.add(CLS_SPINNER_INNER_WRAP);
    target.appendChild(spinnerContainer);
    spinnerContainer.appendChild(spinnerInnerContainer);
    // eslint-disable-next-line
    return { wrap: spinnerContainer, inner_wrap: spinnerInnerContainer };
}
/**
 *
 * @param {HTMLElement} container - specifies the element
 * @param {string} label - specifies the string
 * @param {createElementParams} makeElement - specifies the element
 * @returns {HTMLElement} - returns the element
 */
function createLabel(container, label, makeElement) {
    var labelEle = makeElement('div', {});
    labelEle.classList.add(CLS_SPINLABEL);
    labelEle.innerHTML = label;
    container.appendChild(labelEle);
    return labelEle;
}
/**
 *
 * @param {HTMLElement} container - specifies the element
 * @param {number} radius - specifies the radius
 * @param {createElementParams} makeElement - specifies the element
 * @returns {void}
 */
function createFluentSpinner(container, radius, makeElement) {
    var uniqueID = generateUniqueId();
    globalTimeOut[uniqueID] = { timeOut: 0, type: 'Fabric', radius: radius };
    createFluentElement(container, uniqueID, CLS_FLUENTSPIN);
    calculateFluentAttributes(radius, container, CLS_FLUENTSPIN);
}
/**
 *
 * @param {HTMLElement} container - specifies the element
 * @returns {string} - returns the string
 */
function getTheme(container) {
    var theme = window.getComputedStyle(container, ':after').getPropertyValue('content');
    return theme.replace(/['"]+/g, '');
}
/**
 *
 * @param {string} theme - specifies the theme
 * @param {HTMLElement} container - specifies the element
 * @param {number} radius - specifies the radius
 * @param {createElementParams} makeElement - specifies the element
 * @returns {void}
 */
function setTheme(theme, container, radius, makeElement) {
    var innerContainer = container.querySelector('.' + CLS_SPINNER_INNER_WRAP);
    var svg = innerContainer.querySelector('svg');
    if (!sfBlazorToolkit.base.isNullOrUndefined(svg)) {
        innerContainer.removeChild(svg);
    }
    createFluentSpinner(innerContainer, radius);
}
/**
 *
 * @param {HTMLElement} container - specifies the element
 * @param {string} template - specifies the template
 * @param {string} cssClass - specifies the css class.
 * @returns {void}
 */
function replaceContent(container, template, cssClass) {
    if (!sfBlazorToolkit.base.isNullOrUndefined(cssClass)) {
        container.classList.add(cssClass);
    }
    var inner = container.querySelector('.e-spinner-inner');
    inner.innerHTML = template;
}
/**
 *
 * @param {string} width - specifies the width
 * @param {string} theme - specifies the string
 * @returns {number} - returns the number
 */
function calculateRadius(width, theme) {
    var defaultSize;
    defaultSize = DEFAULT_FLUENT_WIDTH;
    width = width ? parseFloat(width + '') : defaultSize;
    return theme === 'Bootstrap' ? width : width / 2;
}
/**
 * @returns {string} - returns the string
 */
// eslint-disable-next-line
function generateUniqueId() {
    var random = '';
    var combine = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    for (var i = 0; i < 5; i++) {
        random += combine.charAt(Math.floor(Math.random() * combine.length));
    }
    return random;
}
/**
 *
 * @param {HTMLElement} innerCon - specifies the element
 * @param {string} uniqueID - specifies the unique id
 * @param {string} themeClass - specifies the string
 * @param {createElementParams} makeElement - specifies the element
 * @returns {void}
 */
// eslint-disable-next-line
function createFluentElement(innerCon, uniqueID, themeClass, makeElement) {
    var svgFabric = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    svgFabric.setAttribute('id', uniqueID);
    svgFabric.setAttribute('class', themeClass);
    var fabricCirclePath = document.createElementNS('http://www.w3.org/2000/svg', 'path');
    fabricCirclePath.setAttribute('class', CLS_SPINCIRCLE);
    var fabricCircleArc = document.createElementNS('http://www.w3.org/2000/svg', 'path');
    fabricCircleArc.setAttribute('class', CLS_SPINARC);
    innerCon.insertBefore(svgFabric, innerCon.firstChild);
    svgFabric.appendChild(fabricCirclePath);
    svgFabric.appendChild(fabricCircleArc);
}
/**
 *
 * @param {number} radius - specifies the number
 * @param {HTMLElement} innerConainer - specifies the element
 * @param {string} trgClass - specifies the class
 * @returns {void}
 */
// eslint-disable-next-line
function calculateFluentAttributes(radius, innerConainer, trgClass) {
    var centerX = radius;
    var centerY = radius;
    var diameter = radius * 2;
    // eslint-disable-next-line
    var startArc = 315, endArc = 45;
    var svg = innerConainer.querySelector('.' + trgClass);
    var circle = svg.querySelector('.e-path-circle');
    var path = svg.querySelector('.e-path-arc');
    var transformOrigin = (diameter / 2) + 'px';
    circle.setAttribute('d', defineCircle(centerX, centerY, radius));
    path.setAttribute('d', defineArc(centerX, centerY, radius, startArc, endArc));
    svg.setAttribute('viewBox', '0 0 ' + diameter + ' ' + diameter);
    svg.style.transformOrigin = transformOrigin + ' ' + transformOrigin + ' ' + transformOrigin;
    svg.style.width = svg.style.height = diameter + 'px';
}
/**
 *
 * @param {number} centerX - specifies the number
 * @param {number} centerY - specifies the stroke size
 * @param {number} radius - specifies the value
 * @param {number} angle - specifies the max number
 * @returns {number} - returns the number
 */
function defineArcPoints(centerX, centerY, radius, angle) {
    var radians = (angle - 90) * Math.PI / 180.0;
    return {
        x: centerX + (radius * Math.cos(radians)),
        y: centerY + (radius * Math.sin(radians))
    };
}
/**
 *
 * @param {number} x - specifies the number
 * @param {number} y - specifies the stroke size
 * @param {number} radius - specifies the radius
 * @param {number} startArc - specifies the value
 * @param {number} endArc - specifies the max number
 * @returns {number} - returns the number
 */
function defineArc(x, y, radius, startArc, endArc) {
    var start = defineArcPoints(x, y, radius, endArc);
    var end = defineArcPoints(x, y, radius, startArc);
    var d = [
        'M', start.x, start.y,
        'A', radius, radius, 0, 0, 0, end.x, end.y
    ].join(' ');
    return d;
}
/**
 *
 * @param {number} x - specifies the number
 * @param {number} y - specifies the stroke size
 * @param {number} radius - specifies the value
 * @returns {string} - returns the string
 */
function defineCircle(x, y, radius) {
    var d = [
        'M', x, y,
        'm', -radius, 0,
        'a', radius, radius, 0, 1, 0, radius * 2, 0,
        'a', radius, radius, 0, 1, 0, -radius * 2, 0
    ].join(' ');
    return d;
}
/**
 * Function to show the Spinner.
 *
 * @param {HTMLElement} container - Specify the target of the Spinner.
 * @returns {void}
 * @private
 */
function enableSpinner(container) {
    showHideSpinner(container, false);
    container = null;
}
/**
 *
 * @param {HTMLElement} container - specifies the element
 * @param {boolean} isHide - specifies the boolean
 * @returns {void}
 */
function showHideSpinner(container, isHide) {
    var spinnerWrap;
    if (container) {
        spinnerWrap = container.classList.contains(CLS_SPINWRAP) ? container :
            container.querySelector('.' + CLS_SPINWRAP);
    }
    if (container && spinnerWrap) {
        var inner = spinnerWrap.querySelector('.' + CLS_SPINNER_INNER_WRAP);
        var spinCheck = void 0;
        // eslint-disable-next-line
        spinCheck = isHide ? !spinnerWrap.classList.contains(CLS_SPINTEMPLATE) && !spinnerWrap.classList.contains(CLS_HIDESPIN) :
            !spinnerWrap.classList.contains(CLS_SPINTEMPLATE) && !spinnerWrap.classList.contains(CLS_SHOWSPIN);
        if (spinCheck) {
            var svgEle = spinnerWrap.querySelector('svg');
            if (sfBlazorToolkit.base.isNullOrUndefined(svgEle)) {
                return;
            }
            var id = svgEle.getAttribute('id');
            globalTimeOut[id].isAnimate = !isHide;

        }
        // eslint-disable-next-line
        if (isHide) {
            sfBlazorToolkit.base.addClass(spinnerWrap, [CLS_HIDESPIN]);
            sfBlazorToolkit.base.removeClass(spinnerWrap, [CLS_SHOWSPIN]);
        }
        else {
            sfBlazorToolkit.base.addClass(spinnerWrap, [CLS_SHOWSPIN]);
            sfBlazorToolkit.base.removeClass(spinnerWrap, [CLS_HIDESPIN]);
        }
        container = null;
    }
}
/**
 * Function to hide the Spinner.
 *
 * @param {HTMLElement} container - Specify the target of the Spinner.
 * @returns {void}
 * @private
 */
function disableSpinner(container) {
    showHideSpinner(container, true);
    container = null;
}

var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function () { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function () { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};

/**
 * Blazor uploader interop handler
 */
var INPUT_WRAPPER = 'e-file-select';
var DROP_WRAPPER = 'e-file-select-wrap';
var LIST_PARENT = 'e-upload-files';
var FILE = 'e-upload-file-list';
var STATUS = 'e-file-status';
var ACTION_BUTTONS = 'e-upload-actions';
var UPLOAD_BUTTONS = 'e-file-upload-btn e-css e-btn e-flat e-primary';
var CLEAR_BUTTONS = 'e-file-clear-btn e-css e-btn e-flat';
var FILE_NAME = 'e-file-name';
var FILE_TYPE = 'e-file-type';
var FILE_SIZE = 'e-file-size';
var REMOVE_ICON = 'e-close';
var DELETE_ICON = 'e-trash';
var SPINNER_PANE = 'e-spinner-pane';
var ABORT_ICON = 'e-circle-remove';
var RETRY_ICON = 'e-refresh';
var DRAG_HOVER = 'e-upload-drag-hover';
var PROGRESS_WRAPPER = 'e-upload-progress-wrap';
var PROGRESSBAR = 'e-upload-progress-bar';
var PROGRESSBAR_TEXT = 'e-progress-bar-text';
var UPLOAD_INPROGRESS = 'e-upload-progress';
var UPLOAD_SUCCESS = 'e-upload-success';
var UPLOAD_FAILED = 'e-upload-fails';
var TEXT_CONTAINER = 'e-file-container';
var VALIDATION_FAILS = 'e-validation-fails';
var RTL_CONTAINER = 'e-rtl-container';
var ICON_FOCUSED = 'e-clear-icon-focus';
var PROGRESS_INNER_WRAPPER = 'e-progress-inner-wrap';
var PAUSE_UPLOAD = 'e-pause';
var RESUME_UPLOAD = 'e-play';
var RESTRICT_RETRY = 'e-restrict-retry';
var FORM_UPLOAD = 'e-form-upload';
var HIDDEN_INPUT = 'e-hidden-file-input';
var INVALID_FILE = 'e-file-invalid';
var INFORMATION = 'e-file-information';
var SfUploader = /** @class */ (function () {
    function SfUploader(dataId, element, dotnetRef, options) {
        this.initialAttr = { accept: null, multiple: false, disabled: false };
        this.uploadedFilesData = [];
        this.base64String = [];
        this.isForm = false;
        this.allTypes = false;
        this.pausedData = [];
        this.uploadMetaData = [];
        this.btnTabIndex = '0';
        this.disableKeyboardNavigation = false;
        this.count = -1;
        this.actionCompleteCount = 0;
        this.flag = true;
        this.selectedFiles = [];
        this.uploaderName = 'UploadFiles';
        this.fileStreams = [];
        this.newFileRef = 0;
        this.isFirstFileOnSelection = false;
        this.dragCounter = 0;
        this.fileList = [];
        this.filesData = [];
        window.sfBlazorToolkit = window.sfBlazorToolkit;
        this.dataId = dataId;
        this.element = element.element;
        this.updateProperty(options);
        window.sfBlazorToolkit.base.setCompInstance(this);
        this.dotNetRef = dotnetRef;
        this.withCredentials = false;
    }
    SfUploader.prototype.initialize = function () {
        this.preRender();
        this.render();
    };
    //#region Internal methods
    SfUploader.prototype.updateProperty = function (uploadObj) {
        sfBlazorToolkit.base.extend(this, uploadObj);
    };
    SfUploader.prototype.reRenderFileList = function () {
        if (this.listParent) {
            sfBlazorToolkit.base.detach(this.listParent);
            this.listParent = null;
            this.fileList = [];
            this.createFileList(this.filesData);
            if (this.actionButtons) {
                this.removeActionButtons();
                this.renderActionButtons();
                this.checkActionButtonStatus();
            }
        }
    };
    SfUploader.prototype.updateDropArea = function () {
        if (this.dropArea) {
            this.setDropArea();
        }
        else {
            this.dropZoneElement = null;
            var dropTextArea = this.dropAreaWrapper.querySelector('.e-file-drop');
            if (dropTextArea) {
                sfBlazorToolkit.base.remove(dropTextArea);
            }
        }
    };
    SfUploader.prototype.propertyChanges = function (uploadObj, newProp) {
        this.updateProperty(uploadObj);
        this.isSaveUrlNotConfigured = (this.asyncSettings.saveUrl === '' || sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.saveUrl));
        for (var fileIndex = 0, props = Object.keys(newProp); fileIndex < props.length; fileIndex++) {
            var prop = props[fileIndex];
            switch (prop) {
                case 'AllowedExtensions':
                    this.clearAll();
                    break;
                case 'EnableRtl':
                    this.reRenderFileList();
                    break;
                case 'Buttons':
                    this.buttons.browse = sfBlazorToolkit.base.isNullOrUndefined(this.buttons.browse) ? '' : this.buttons.browse;
                    this.buttons.clear = sfBlazorToolkit.base.isNullOrUndefined(this.buttons.clear) ? '' : this.buttons.clear;
                    this.buttons.upload = sfBlazorToolkit.base.isNullOrUndefined(this.buttons.upload) ? '' : this.buttons.upload;
                    this.renderButtonTemplates();
                    break;
                case 'DropArea':
                    this.unBindDropEvents();
                    this.updateDropArea();
                    break;
                case 'Files':
                    this.renderPreLoadFiles();
                    break;
                case 'MinFileSize':
                case 'MaxFileSize':
                case 'AutoUpload':
                    this.clearAll();
                    break;
                case 'SequentialUpload':
                    this.clearAll();
                    break;
            }
        }
    };
    SfUploader.prototype.preRender = function () {
        this.isValidTemplate = this.template !== '' && !sfBlazorToolkit.base.isNullOrUndefined(this.template);
        this.isSaveUrlNotConfigured = (this.asyncSettings.saveUrl === '' || sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.saveUrl));
        if (this.isSaveUrlNotConfigured && this.sequentialUpload) {
            this.sequentialUpload = false;
        }
        if (!this.isSaveUrlNotConfigured) {
            this.formRendered();
        }
        this.keyConfigs = {
            enter: 'enter'
        };
        this.browserName = sfBlazorToolkit.base.Browser.info.name;
        this.uploaderName = this.element.getAttribute('name');
    };
    SfUploader.prototype.formRendered = function () {
        var parentEle = sfBlazorToolkit.base.closest(this.element, 'form');
        if (!sfBlazorToolkit.base.isNullOrUndefined(parentEle)) {
            for (; parentEle && parentEle !== document.documentElement; parentEle = parentEle.parentElement) {
                if (parentEle.tagName === 'FORM') {
                    this.isForm = true;
                    this.formElement = parentEle;
                    parentEle.setAttribute('enctype', 'multipart/form-data');
                    parentEle.setAttribute('encoding', 'multipart/form-data');
                }
            }
        }
    };
    /**
     * Initializes the control rendering.
     *
     * @private
     * @returns {void} Nothing.
     */
    SfUploader.prototype.render = function () {
        this.dropAreaWrapper = sfBlazorToolkit.base.closest(this.element, '.' + DROP_WRAPPER);
        this.uploadWrapper = sfBlazorToolkit.base.closest(this.element, '.e-upload.e-control-wrapper');
        this.browseButton = this.dropAreaWrapper.querySelector('button.e-upload-browse-btn');
        this.setDropArea();
        this.renderPreLoadFiles();
        this.wireEvents();
    };
    SfUploader.prototype.wireEvents = function () {
        sfBlazorToolkit.base.EventHandler.add(this.browseButton, 'click', this.browseButtonClick, this);
        sfBlazorToolkit.base.EventHandler.add(this.element, 'change', this.onSelectFiles, this);
        sfBlazorToolkit.base.EventHandler.add(document, 'click', this.removeFocus, this);
        sfBlazorToolkit.base.EventHandler.add(this.uploadWrapper, 'keydown', this.keyActionHandler, this);
        if (this.isForm) {
            sfBlazorToolkit.base.EventHandler.add(this.formElement, 'reset', this.resetForm, this);
        }
    };
    SfUploader.prototype.renderPreLoadFiles = function () {
        if (this.files && this.files.length) {
            if (this.enablePersistence && this.filesData.length) {
                this.createFileList(this.filesData);
                return;
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(this.files[0].size)) {
                return;
            }
            var files = [].slice.call(this.files);
            var filesData = [];
            if (!this.multiple) {
                this.clearData();
                files = [files[0]];
            }
            for (var index = 0, filesList = files; index < filesList.length; index++) {
                var data = filesList[index];
                var fileData = {
                    name: data.name + '.' + data.type.split('.')[data.type.split('.').length - 1],
                    rawFile: '',
                    size: data.size,
                    status: this.localizedTexts('uploadSuccessMessage'),
                    type: data.type,
                    validationMessages: { minSize: '', maxSize: '' },
                    statusCode: '2'
                };
                filesData.push(fileData);
                this.filesData.push(fileData);
            }
            if (this.isValidTemplate) {
                return;
            }
            this.createFileList(filesData);
            if (!this.autoUpload && this.listParent && !this.actionButtons && (!this.isForm || this.allowUpload()) && this.showFileList) {
                this.renderActionButtons();
            }
            this.checkActionButtonStatus();
            if (this.sequentialUpload) {
                this.count = this.filesData.length - 1;
            }
        }
    };
    SfUploader.prototype.renderActionButtons = function () {
        this.element.setAttribute('tabindex', '-1');
        if (!(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
            this.actionButtons = sfBlazorToolkit.base.createElement('div', { className: ACTION_BUTTONS });
            this.uploadButton = sfBlazorToolkit.base.createElement('button', {
                className: UPLOAD_BUTTONS,
                attrs: { 'type': 'button', 'tabindex': this.btnTabIndex }
            });
            this.clearButton = sfBlazorToolkit.base.createElement('button', {
                className: CLEAR_BUTTONS,
                attrs: { 'type': 'button', 'tabindex': this.btnTabIndex }
            });
            this.actionButtons.appendChild(this.clearButton);
            this.actionButtons.appendChild(this.uploadButton);
            this.renderButtonTemplates();
            this.uploadWrapper.appendChild(this.actionButtons);
            this.browseButton.blur();
            this.uploadButton.focus();
            this.wireActionButtonEvents();
        }
    };
    SfUploader.prototype.setDropArea = function () {
        var dropTextArea = this.dropAreaWrapper.querySelector('.e-file-drop');
        if (this.dropArea) {
            this.dropZoneElement = (typeof (this.dropArea) !== 'string') ? this.dropArea :
                document.querySelector(this.dropArea);
            var element = this.element;
            var enableDropText = false;
            while (element.parentNode) {
                element = element.parentNode;
                if (element === this.dropZoneElement) {
                    enableDropText = true;
                }
            }
            if (!enableDropText && dropTextArea) {
                sfBlazorToolkit.base.remove(dropTextArea);
            }
        }
        else if (sfBlazorToolkit.base.isNullOrUndefined(this.dropArea)) {
            this.dropZoneElement = this.uploadWrapper;
            //this.setProperties({dropArea: this.uploadWrapper }, true);
        }
        this.bindDropEvents();
    };
    SfUploader.prototype.serverActionButtonsEventBind = function (element) {
        if (element && !this.isForm) {
            this.browseButton.blur();
            this.actionButtons = element;
            this.uploadButton = this.actionButtons.querySelector('.e-file-upload-btn');
            this.clearButton = this.actionButtons.querySelector('.e-file-clear-btn');
            this.uploadButton.focus();
            this.unwireActionButtonEvents();
            this.wireActionButtonEvents();
            this.checkActionButtonStatus();
        }
    };
    SfUploader.prototype.serverUlElement = function (element) {
        if (element && (this.isSaveUrlNotConfigured || this.isValidTemplate)) {
            this.listParent = element;
            this.fileList = [].slice.call(this.listParent.querySelectorAll('li'));
            this.serverRemoveIconBindEvent();
            if (!this.isForm) {
                this.checkAutoUpload(this.filesData);
            }
        }
    };
    SfUploader.prototype.wireActionButtonEvents = function () {
        sfBlazorToolkit.base.EventHandler.add(this.uploadButton, 'click', this.uploadButtonClick, this);
        sfBlazorToolkit.base.EventHandler.add(this.clearButton, 'click', this.clearButtonClick, this);
    };
    SfUploader.prototype.unwireActionButtonEvents = function () {
        sfBlazorToolkit.base.EventHandler.remove(this.uploadButton, 'click', this.uploadButtonClick);
        sfBlazorToolkit.base.EventHandler.remove(this.clearButton, 'click', this.clearButtonClick);
    };
    SfUploader.prototype.checkActionButtonStatus = function () {
        if (this.actionButtons && !this.isValidTemplate) {
            var length_1 = this.uploadWrapper.querySelectorAll('.' + VALIDATION_FAILS).length +
                this.uploadWrapper.querySelectorAll('.e-upload-fails:not(.e-upload-progress)').length +
                this.uploadWrapper.querySelectorAll('span.' + UPLOAD_SUCCESS).length +
                this.uploadWrapper.querySelectorAll('span.' + UPLOAD_INPROGRESS).length;
            if (length_1 > 0 && length_1 === this.uploadWrapper.querySelectorAll('li').length) {
                this.uploadButton.setAttribute('disabled', 'disabled');
            }
            else {
                this.uploadButton.removeAttribute('disabled');
            }
        }
    };
    SfUploader.prototype.renderButtonTemplates = function () {
        if (typeof (this.buttons.browse) === 'string') {
            this.browseButton.textContent = (this.buttons.browse === 'Browse...') ?
                this.localizedTexts('browse') : this.buttons.browse;
            this.browseButton.setAttribute('title', this.browseButton.textContent);
        }
        else {
            this.browseButton.innerHTML = '';
            this.browseButton.appendChild(this.buttons.browse);
        }
        if (this.uploadButton) {
            var uploadText = sfBlazorToolkit.base.isNullOrUndefined(this.buttons.upload) ? 'Upload' : this.buttons.upload;
            this.buttons.upload = uploadText;
            if (typeof (this.buttons.upload) === 'string') {
                this.uploadButton.textContent = (this.buttons.upload === 'Upload') ?
                    this.localizedTexts('upload') : this.buttons.upload;
                this.uploadButton.setAttribute('title', this.uploadButton.textContent);
            }
            else {
                this.uploadButton.innerHTML = '';
                this.uploadButton.appendChild(this.buttons.upload);
            }
        }
        if (this.clearButton) {
            var clearText = sfBlazorToolkit.base.isNullOrUndefined(this.buttons.clear) ? 'Clear' : this.buttons.clear;
            this.buttons.clear = clearText;
            if (typeof (this.buttons.clear) === 'string') {
                this.clearButton.textContent = (this.buttons.clear === 'Clear') ?
                    this.localizedTexts('clear') : this.buttons.clear;
                this.clearButton.setAttribute('title', this.clearButton.textContent);
            }
            else {
                this.clearButton.innerHTML = '';
                this.clearButton.appendChild(this.buttons.clear);
            }
        }
    };
    SfUploader.prototype.checkAutoUpload = function (fileData) {
        if (this.autoUpload) {
            if (this.sequentialUpload) {
                this.sequenceUpload(fileData);
            }
            else if (this.cssClass === 'e-fm-upload') {
                for (var i = 0; i < fileData.length; i++) {
                    this.upload(fileData[i]);
                }
            }
            else {
                this.upload(fileData);
            }
            this.removeActionButtons();
        }
        else if (!this.actionButtons) {
            this.renderActionButtons();
        }
        this.checkActionButtonStatus();
    };
    SfUploader.prototype.removeActionButtons = function () {
        if (this.actionButtons) {
            this.unwireActionButtonEvents();
            if (!(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
                sfBlazorToolkit.base.detach(this.actionButtons);
            }
            this.actionButtons = null;
        }
    };
    SfUploader.prototype.sequenceUpload = function (fileData) {
        var fileCount = fileData && fileData.length ? fileData.length : fileData ? 1 : 0;
        if (this.filesData.length - fileCount === 0 ||
            this.filesData[(this.filesData.length - fileCount - 1)].statusCode !== '1') {
            if (this.multiple || this.count < 0) {
                ++this.count;
            }
            var isFileListCreated = this.showFileList ? false : true;
            if (typeof this.filesData[this.count] === 'object') {
                this.isFirstFileOnSelection = false;
                if (this.filesData[this.count].statusCode !== '2') {
                    this.upload(this.filesData[this.count], isFileListCreated);
                }
                if (this.filesData[this.count].statusCode === '0') {
                    this.sequenceUpload(fileData);
                }
            }
            else {
                --this.count;
            }
        }
    };
    SfUploader.prototype.unWireEvents = function () {
        sfBlazorToolkit.base.EventHandler.remove(this.browseButton, 'click', this.browseButtonClick);
        sfBlazorToolkit.base.EventHandler.remove(this.element, 'change', this.onSelectFiles);
        sfBlazorToolkit.base.EventHandler.remove(document, 'click', this.removeFocus);
        sfBlazorToolkit.base.EventHandler.remove(this.uploadWrapper, 'keydown', this.keyActionHandler);
        if (this.isForm) {
            sfBlazorToolkit.base.EventHandler.remove(this.formElement, 'reset', this.resetForm);
        }
    };
    SfUploader.prototype.resetForm = function () {
        this.clearAll();
    };
    SfUploader.prototype.keyActionHandler = function (e) {
        var targetElement = e.target;
        switch (e.code) {
            case 'Enter':
                if (e.target === this.clearButton) {
                    this.clearButtonClick();
                }
                else if (e.target === this.uploadButton) {
                    this.uploadButtonClick();
                }
                else if (e.target === this.browseButton) {
                    this.browseButtonClick();
                }
                else if (targetElement.classList.contains(PAUSE_UPLOAD)) {
                    var metaData = this.getCurrentMetaData(null, e);
                    metaData.file.statusCode = '4';
                    metaData.file.status = this.localizedTexts('pauseUpload');
                    this.abortUpload(metaData, false);
                }
                else if (targetElement.classList.contains(RESUME_UPLOAD)) {
                    this.resumeUpload(this.getCurrentMetaData(null, e), e);
                }
                else if (targetElement.classList.contains(RETRY_ICON)) {
                    var metaData = this.getCurrentMetaData(null, e);
                    if (!sfBlazorToolkit.base.isNullOrUndefined(metaData)) {
                        metaData.file.statusCode = '1';
                        metaData.file.status = this.localizedTexts('readyToUploadMessage');
                        this.chunkUpload(metaData.file);
                    }
                    else {
                        var target = e.target.parentElement;
                        var fileData = this.filesData[this.fileList.indexOf(target)];
                        this.retry(fileData);
                    }
                }
                else {
                    if (!this.isSaveUrlNotConfigured) {
                        this.removeFiles(e);
                        if (!targetElement.classList.contains(ABORT_ICON)) {
                            this.browseButton.focus();
                        }
                    }
                    else {
                        break;
                    }
                }
                e.preventDefault();
                e.stopPropagation();
                break;
        }
    };
    SfUploader.prototype.getCurrentMetaData = function (fileInfo, e) {
        var fileData;
        var targetMetaData;
        if (sfBlazorToolkit.base.isNullOrUndefined(fileInfo)) {
            var target = e.target.parentElement;
            fileData = this.filesData[this.fileList.indexOf(target)];
        }
        else {
            fileData = fileInfo;
        }
        for (var i = 0; i < this.uploadMetaData.length; i++) {
            if (this.uploadMetaData[i].file.name === fileData.name) {
                targetMetaData = this.uploadMetaData[i];
            }
        }
        return targetMetaData;
    };
    SfUploader.prototype.removeFocus = function () {
        if (this.uploadWrapper && this.listParent && this.listParent.querySelector('.' + ICON_FOCUSED)) {
            document.activeElement.blur();
            this.listParent.querySelector('.' + ICON_FOCUSED).classList.remove(ICON_FOCUSED);
        }
    };
    SfUploader.prototype.browseButtonClick = function () {
        this.element.click();
    };
    SfUploader.prototype.uploadButtonClick = function () {
        if (this.sequentialUpload) {
            this.sequenceUpload(this.filesData);
        }
        else {
            this.upload(this.filesData);
        }
    };
    SfUploader.prototype.clearButtonClick = function () {
        this.clearAll();
        if (this.sequentialUpload) {
            this.count = -1;
        }
        this.actionCompleteCount = 0;
    };
    SfUploader.prototype.bindDropEvents = function () {
        if (this.dropZoneElement) {
            sfBlazorToolkit.base.EventHandler.add(this.dropZoneElement, 'drop', this.dropElement, this);
            sfBlazorToolkit.base.EventHandler.add(this.dropZoneElement, 'dragover', this.dragHover, this);
            sfBlazorToolkit.base.EventHandler.add(this.dropZoneElement, 'dragleave', this.onDragLeave, this);
            sfBlazorToolkit.base.EventHandler.add(this.dropZoneElement, 'paste', this.onPasteFile, this);
            sfBlazorToolkit.base.EventHandler.add(this.dropZoneElement, 'dragenter', this.onDragEnter, this);
        }
    };
    SfUploader.prototype.unBindDropEvents = function () {
        if (this.dropZoneElement) {
            sfBlazorToolkit.base.EventHandler.remove(this.dropZoneElement, 'drop', this.dropElement);
            sfBlazorToolkit.base.EventHandler.remove(this.dropZoneElement, 'dragover', this.dragHover);
            sfBlazorToolkit.base.EventHandler.remove(this.dropZoneElement, 'dragleave', this.onDragLeave);
            sfBlazorToolkit.base.EventHandler.remove(this.dropZoneElement, 'dragenter', this.onDragEnter);
        }
    };
    SfUploader.prototype.onDragEnter = function (e) {
        if (this.disabled) {
            return;
        }
        this.dropZoneElement.classList.add(DRAG_HOVER);
        this.dragCounter = this.dragCounter + 1;
        e.preventDefault();
        e.stopPropagation();
    };
    SfUploader.prototype.onDragLeave = function (e) {
        if (this.disabled) {
            return;
        }
        this.dragCounter = this.dragCounter - 1;
        if (!this.dragCounter) {
            this.dropZoneElement.classList.remove(DRAG_HOVER);
        }
    };
    SfUploader.prototype.dragHover = function (e) {
        if (this.disabled) {
            return;
        }
        if (this.dropEffect !== 'Default') {
            e.dataTransfer.dropEffect = this.dropEffect.toLowerCase();
        }
        if (e.dataTransfer.types.indexOf('Files') !== -1) {
            e.preventDefault();
            e.stopPropagation();
        }
    };
    SfUploader.prototype.getFileFromEntry = function (fileEntry) {
        // eslint-disable-next-line @typescript-eslint/tslint/config
        return new Promise(function (resolve, reject) {
            fileEntry.file(resolve, reject);
        });
    };
    // Recursive function to handle directory traversal
    SfUploader.prototype.traverseDirectory = function (directoryEntry) {
        return __awaiter(this, void 0, void 0, function () {
            var directoryReader;
            var _this = this;
            return __generator(this, function (_a) {
                directoryReader = directoryEntry.createReader();
                // eslint-disable-next-line @typescript-eslint/tslint/config
                return [2 /*return*/, new Promise(function (resolve, reject) {
                    directoryReader.readEntries(function (entries) {
                        return __awaiter(_this, void 0, void 0, function () {
                            var i, entry, file, error_1;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        i = 0;
                                        _a.label = 1;
                                    case 1:
                                        if (!(i < entries.length)) return [3 /*break*/, 9];
                                        entry = entries[i];
                                        if (!entry.isFile) return [3 /*break*/, 6];
                                        _a.label = 2;
                                    case 2:
                                        _a.trys.push([2, 4, , 5]);
                                        return [4 /*yield*/, this.getFileFromEntry(entry)];
                                    case 3:
                                        file = _a.sent();
                                        this.FileDirectoryDetails.push({
                                            'path': entry.fullPath,
                                            'file': file
                                        });
                                        // Add the File object to the DataTransferItemList
                                        this.DropDirectoryFiles.items.add(file);
                                        return [3 /*break*/, 5];
                                    case 4:
                                        error_1 = _a.sent();
                                        console.error('Failed to convert FileEntry to File object:', error_1);
                                        return [3 /*break*/, 5];
                                    case 5: return [3 /*break*/, 8];
                                    case 6:
                                        if (!entry.isDirectory) return [3 /*break*/, 8];
                                        return [4 /*yield*/, this.traverseDirectory(entry)];
                                    case 7:
                                        _a.sent();
                                        _a.label = 8;
                                    case 8:
                                        i++;
                                        return [3 /*break*/, 1];
                                    case 9:
                                        resolve();
                                        return [2 /*return*/];
                                }
                            });
                        });
                    }, reject);
                })];
            });
        });
    };
    SfUploader.prototype.dropElement = function (e) {
        var _this = this;
        this.dragCounter = 0;
        e.preventDefault();
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        if (this.isSaveUrlNotConfigured && this.directoryUpload) {
            this.FileDirectoryDetails = [];
            this.DropDirectoryFiles = new DataTransfer();
            var items = e.dataTransfer.items;
            // Iterate over the dropped items
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var entry = item.webkitGetAsEntry();
                if (entry) {
                    if (entry.isDirectory) {
                        this.traverseDirectory(entry)
                            .then(function () {
                                _this.element.files = _this.DropDirectoryFiles.files;
                                var event = new CustomEvent('change', {
                                    bubbles: true,
                                    detail: {
                                        DropEvent: e
                                    }
                                });
                                _this.element.dispatchEvent(event);
                                _this.dropZoneElement.classList.remove(DRAG_HOVER);
                                if (e.dataTransfer.types.indexOf('Files') !== -1) {
                                    e.preventDefault();
                                    e.stopPropagation();
                                }
                            })
                            .catch(function (error) {
                                console.error('Failed to traverse directory:', error);
                            });
                    }
                }
            }
        }
        else {
            this.element.files = e.dataTransfer.files;
            var event_1 = new CustomEvent('change', {
                bubbles: true, detail: {
                    DropEvent: e
                }
            });
            this.element.dispatchEvent(event_1);
            this.dropZoneElement.classList.remove(DRAG_HOVER);
            if (e.dataTransfer.types.indexOf('Files') !== -1) {
                e.preventDefault();
                e.stopPropagation();
            }
        }
    };
    SfUploader.prototype.onPasteFile = function (event) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.dropArea) && this.dropArea.includes('.e-rte-content')) {
            return;
        }
        var item = event.clipboardData.items;
        if (item.length !== 1 && !this.multiple) {
            return;
        }
        else {
            var files = [];
            if (item.length !== event.clipboardData.files.length) {
                for (var fileData = 0; fileData < event.clipboardData.files.length; fileData++) {
                    var file = event.clipboardData.files[fileData];
                    if (file.type.match('^image/')) {
                        files.push(file);
                    }
                }
            }
            else {
                for (var fileData = 0; fileData < event.clipboardData.files.length; fileData++) {
                    var pasteFile = [].slice.call(item)[fileData];
                    var file = event.clipboardData.files[fileData];
                    if ((pasteFile.kind === 'file') || pasteFile.type.match('^image/')) {
                        files.push(file);
                    }
                }
            }
            var updatedFileListObject_1 = new DataTransfer();
            files.forEach(function (file) {
                updatedFileListObject_1.items.add(file);
            });
            this.element.files = updatedFileListObject_1.files;
            var e = new CustomEvent('change', {
                bubbles: true,
                detail: {
                    isDirectory: false,
                    isPaste: true
                }
            });
            this.element.dispatchEvent(e);
        }
    };
    SfUploader.prototype.getSelectedFiles = function (index) {
        var data = [];
        var liElement = this.fileList[index];
        var allFiles = this.getFilesData();
        var nameElements = +liElement.getAttribute('data-files-count');
        var startIndex = 0;
        for (var i = 0; i < index; i++) {
            startIndex += (+this.fileList[i].getAttribute('data-files-count'));
        }
        for (var j = startIndex; j < (startIndex + nameElements); j++) {
            data.push(allFiles[j]);
        }
        return data;
    };
    SfUploader.prototype.removeFiles = function (args) {
        if (this.disabled) {
            return;
        }
        var selectedElement = args.target.parentElement;
        if (this.isSaveUrlNotConfigured) {
            this.fileList = [].slice.call(this.uploadWrapper.querySelectorAll('li'));
        }
        var index = this.fileList.indexOf(selectedElement);
        var liElement = this.fileList[index];
        var formUpload = this.isFormUpload();
        var fileData = formUpload ? this.getSelectedFiles(index) : this.getFilesInArray(this.filesData[index]);
        if (sfBlazorToolkit.base.isNullOrUndefined(fileData)) {
            return;
        }
        if (args.target.classList.contains(ABORT_ICON) && !formUpload) {
            fileData[0].statusCode = '5';
            if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
                var spinnerTarget = liElement.querySelector('.' + ABORT_ICON);
                renderSpinner({ target: spinnerTarget, width: '20px' });
                enableSpinner(spinnerTarget);
            }
            if (this.sequentialUpload) {
                this.uploadSequential();
            }
            if (!(liElement.classList.contains(RESTRICT_RETRY))) {
                this.checkActionComplete(true);
            }
        }
        else if (!sfBlazorToolkit.base.closest(args.target, '.' + SPINNER_PANE)) {
            this.remove(fileData, false, false, true, args);
        }
        this.element.value = '';
        this.checkActionButtonStatus();
    };
    SfUploader.prototype.removeFilesData = function (file, customTemplate) {
        var index;
        if (customTemplate) {
            if (!this.showFileList) {
                index = this.filesData.indexOf(file);
                this.filesData.splice(index, 1);
            }
            return;
        }
        var selectedElement = this.getLiElement(file);
        if (sfBlazorToolkit.base.isNullOrUndefined(selectedElement)) {
            return;
        }
        if (!this.isSaveUrlNotConfigured && !this.isValidTemplate) {
            sfBlazorToolkit.base.detach(selectedElement);
        }
        var spinnerEle = selectedElement.querySelector('.' + SPINNER_PANE);
        if (!sfBlazorToolkit.base.isNullOrUndefined(spinnerEle)) {
            disableSpinner(spinnerEle);
        }
        index = this.fileList.indexOf(selectedElement);
        this.fileList.splice(index, 1);
        this.filesData.splice(index, 1);
        if (!this.isSaveUrlNotConfigured && !this.isValidTemplate && (this.fileList.length === 0 && !sfBlazorToolkit.base.isNullOrUndefined(this.listParent))) {
            sfBlazorToolkit.base.detach(this.listParent);
            this.listParent = null;
            this.removeActionButtons();
            if (this.sequentialUpload) {
                if (index <= this.count) {
                    --this.count;
                }
            }
        }
        else {
            // tslint:disable-next-line
            this.dotNetRef.invokeMethodAsync('RemoveFileDataAsync', index);
        }
    };
    SfUploader.prototype.removeUploadedFile = function (file, eventArgs, removeDirectly, custom) {
        var _this = this;
        var selectedFiles = file;
        var ajax = new sfBlazorToolkit.Ajax(this.asyncSettings.removeUrl, 'POST', true, null);
        ajax.emitError = false;
        var formData = new FormData();
        ajax.beforeSend = function (e) {
            eventArgs.currentRequest = ajax.httpRequest;
            if (_this.currentRequestHeader) {
                _this.updateCustomheader(ajax.httpRequest, _this.currentRequestHeader);
            }
            if (_this.customFormDatas) {
                _this.updateFormData(formData, _this.customFormDatas);
            }
            if (!removeDirectly && _this.removingEnabled) {
                /* eslint-disable */
                // @ts-ignore-start
                _this.dotNetRef.invokeMethodAsync('RemovingEventAsync', eventArgs).then(function (eventArgs) {
                    // @ts-ignore-end
                    /* eslint-enable */
                    if (eventArgs.cancel) {
                        e.cancel = true;
                    }
                    else {
                        _this.removingEventCallback(file);
                    }
                });
            }
            else {
                _this.removingEventCallback(file);
            }
            if (_this.sequentialUpload) {
                --_this.count;
            }
        };
        var name = this.element.getAttribute('name');
        if (eventArgs.postRawFile && !sfBlazorToolkit.base.isNullOrUndefined(selectedFiles.rawFile) && selectedFiles.rawFile !== '') {
            formData.append(name, selectedFiles.rawFile, selectedFiles.name);
        }
        else {
            formData.append(name, selectedFiles.name);
        }
        ajax.onLoad = function (e) { _this.removeCompleted(e, selectedFiles, custom); return {}; };
        ajax.onError = function (e) { _this.removeFailed(e, selectedFiles, custom); return {}; };
        ajax.send(formData);
    };
    SfUploader.prototype.removeCompleted = function (e, files, customTemplate) {
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var status = e.target;
        if (status.readyState === 4 && status.status >= 200 && status.status <= 299) {
            var args = {
                e: e, response: response, operation: 'remove',
                file: this.updateStatus(files, this.localizedTexts('removedSuccessMessage'), '2')
            };
            if (this.successEnabled) {
                this.dotNetRef.invokeMethodAsync('SuccessEventAsync', args);
            }
            this.removeFilesData(files, customTemplate);
            var index = this.uploadedFilesData.indexOf(files);
            this.uploadedFilesData.splice(index, 1);
            if (this.changeEnabled) {
                this.dotNetRef.invokeMethodAsync('ChangeEventAsync', { files: this.uploadedFilesData });
            }
        }
        else {
            this.removeFailed(e, files, customTemplate);
        }
    };
    SfUploader.prototype.removeFailed = function (e, files, customTemplate) {
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var args = {
            e: e, response: response, operation: 'remove', file: this.updateStatus(files, this.localizedTexts('removedFailedMessage'), '0')
        };
        if (!customTemplate) {
            var index = this.filesData.indexOf(files);
            var rootElement = this.fileList[index];
            if (rootElement) {
                rootElement.classList.remove(UPLOAD_SUCCESS);
                rootElement.classList.add(UPLOAD_FAILED);
                var statusElement = rootElement.querySelector('.' + STATUS);
                if (statusElement) {
                    statusElement.classList.remove(UPLOAD_SUCCESS);
                    statusElement.classList.add(UPLOAD_FAILED);
                }
            }
            this.checkActionButtonStatus();
        }
        if (this.failuredEnabled) {
            this.dotNetRef.invokeMethodAsync('FailureEventAsync', args);
        }
        var liElement = this.getLiElement(files);
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement) && !sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + DELETE_ICON))) {
            var spinnerTarget = liElement.querySelector('.' + DELETE_ICON);
            disableSpinner(spinnerTarget);
            sfBlazorToolkit.base.detach(liElement.querySelector('.e-spinner-pane'));
        }
    };
    SfUploader.prototype.removingEventCallback = function (file) {
        var liElement = this.getLiElement(file);
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement) && (!sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + DELETE_ICON)) ||
            !sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + REMOVE_ICON)))) {
            var spinnerTarget = liElement.querySelector('.' + DELETE_ICON) ? liElement.querySelector('.' + DELETE_ICON) :
                liElement.querySelector('.' + REMOVE_ICON);
            renderSpinner({ target: spinnerTarget, width: '20px' });
            enableSpinner(spinnerTarget);
        }
    };
    SfUploader.prototype.updateFormData = function (formData, customData) {
        if (customData && customData.length > 0 && customData[0]) {
            var _loop_1 = function (i) {
                var data = customData[i];
                // tslint:disable-next-line
                var value = Object.keys(data).map(function (e) {
                    return data[e];
                });
                formData.append(Object.keys(data)[0], value);
            };
            for (var i = 0; i < customData.length; i++) {
                _loop_1(i);
            }
        }
    };
    SfUploader.prototype.updateCustomheader = function (request, currentRequest) {
        if (currentRequest.length > 0 && currentRequest[0]) {
            var _loop_2 = function (i) {
                var data = currentRequest[i];
                // tslint:disable-next-line
                var value = Object.keys(data).map(function (e) {
                    return data[e];
                });
                request.setRequestHeader(Object.keys(data)[0], value);
            };
            for (var i = 0; i < currentRequest.length; i++) {
                _loop_2(i);
            }
        }
    };
    SfUploader.prototype.getFilesFromFolder = function (event) {
        this.filesEntries = [];
        var items = this.multiple ? event.dataTransfer.items :
            [event.dataTransfer.items[0]];
        var validDirectoryUpload = this.checkDirectoryUpload(items);
        if (!validDirectoryUpload) {
            return;
        }
        var _loop_3 = function (i) {
            // tslint:disable-next-line
            var item = items[i].webkitGetAsEntry();
            if (item.isFile) {
                var files_2 = [];
                // tslint:disable-next-line
                (item).file(function (fileObj) {
                    var path = item.fullPath;
                    files_2.push({ 'path': path, 'file': fileObj });
                });
                this_1.renderSelectedFiles(event, files_2, true);
            }
            else if (item.isDirectory) {
                this_1.traverseFileTree(item, event);
            }
        };
        var this_1 = this;
        for (var i = 0; i < items.length; i++) {
            _loop_3(i);
        }
    };
    SfUploader.prototype.checkDirectoryUpload = function (items) {
        for (var i = 0; items && i < items.length; i++) {
            // tslint:disable-next-line
            var item = items[i].webkitGetAsEntry();
            if (item.isDirectory) {
                return true;
            }
        }
        return false;
    };
    // tslint:disable
    SfUploader.prototype.traverseFileTree = function (item, event) {
        if (item.isFile) {
            this.filesEntries.push(item);
        }
        else if (item.isDirectory) {
            // tslint:disable-next-line
            var directoryReader = item.createReader();
            // tslint:disable-next-line
            this.readFileFromDirectory(directoryReader, event);
        }
    };
    // tslint:disable
    SfUploader.prototype.readFileFromDirectory = function (directoryReader, event) {
        var _this = this;
        // tslint:disable-next-line
        directoryReader.readEntries(function (entries) {
            for (var i = 0; i < entries.length; i++) {
                _this.traverseFileTree(entries[i], event);
                // tslint:disable-next-line
            }
            _this.pushFilesEntries(event);
            if (entries.length) {
                _this.readFileFromDirectory(directoryReader);
            }
        });
    };
    SfUploader.prototype.pushFilesEntries = function (event) {
        var _this = this;
        var files = [];
        var _loop_4 = function (i) {
            // tslint:disable-next-line
            this_2.filesEntries[i].file(function (fileObj) {
                if (_this.filesEntries.length) {
                    var path = _this.filesEntries[i].fullPath;
                    files.push({ 'path': path, 'file': fileObj });
                    if (i === _this.filesEntries.length - 1) {
                        _this.filesEntries = [];
                        _this.renderSelectedFiles(event, files, true);
                    }
                }
            });
        };
        var this_2 = this;
        for (var i = 0; i < this.filesEntries.length; i++) {
            _loop_4(i);
        }
    };
    // tslint:enable
    SfUploader.prototype.onSelectFiles = function (args) {
        if (this.disabled) {
            return;
        }
        var targetFiles;
        if (args && args.detail && args.detail.DropEvent && args.detail.DropEvent.type === 'drop') {
            if (this.directoryUpload) {
                if (!this.isSaveUrlNotConfigured) {
                    this.getFilesFromFolder(args.detail.DropEvent);
                }
                else {
                    this.renderSelectedFiles(args.detail.DropEvent, this.FileDirectoryDetails, true);
                }
            }
            else {
                var files = this.sortFilesList = args.detail.DropEvent.dataTransfer.files;
                var newFiles = [];
                var items = args.detail.DropEvent.dataTransfer.items;
                for (var i = 0; i < files.length; i++) {
                    for (var j = 0; j < items.length; j++) {
                        var item = items[j];
                        var entry = item.webkitGetAsEntry();
                        if (entry) {
                            if (entry.isFile && entry.name === files[i].name) {
                                newFiles.push(files[i]);
                            }
                        }
                    }
                }
                // Create a new DataTransfer object and assign the newFiles array to its files property
                var newDataTransfer = new DataTransfer();
                for (var i = 0; i < newFiles.length; i++) {
                    newDataTransfer.items.add(newFiles[i]);
                }
                var fileList = newDataTransfer.files;
                if (this.browserName !== 'msie' && this.browserName !== 'edge' && this.browserName !== 'safari') {
                    this.element.files = fileList;
                }
                if (fileList.length > 0) {
                    targetFiles = this.multiple ? this.sortFileList(fileList) : [fileList[0]];
                    this.renderSelectedFiles(args, targetFiles);
                }
            }
        }
        else {
            targetFiles = [].slice.call(args.target.files);
            /* eslint-disable @typescript-eslint/no-explicit-any */
            if (args.detail) {
                this.renderSelectedFiles(args, targetFiles, args.detail.isDirectory, args.detail.isPaste);
                /* eslint-enable @typescript-eslint/no-explicit-any */
            }
            else {
                this.renderSelectedFiles(args, targetFiles);
            }
        }
    };
    SfUploader.prototype.getBase64 = function (file) {
        return new Promise(function (resolve, reject) {
            var fileReader = new FileReader();
            fileReader.readAsDataURL(file);
            fileReader.onload = function () { return resolve(fileReader.result); };
            fileReader.onerror = function (error) { return reject(error); };
        });
    };
    /* tslint:ignore */
    SfUploader.prototype.renderSelectedFiles = function (args,
        // tslint:disable-next-line
        targetFiles, directory, paste) {
        var _this = this;
        this.base64String = [];
        // tslint:disable-next-line
        var eventArgs = {
            event: args,
            cancel: false,
            filesData: [],
            isModified: false,
            modifiedFilesData: [],
            progressInterval: '',
            isCanceled: false,
            currentRequest: null,
            customFormData: null,
            type: args && args.type
        };
        if (targetFiles.length < 1) {
            eventArgs.isCanceled = true;
            if (this.selectedEnabled) {
                /* eslint-disable */
                // @ts-ignore-start
                this.dotNetRef.invokeMethodAsync('SelectedEventAsync', eventArgs).then(function (eventArgs) {
                    // @ts-ignore-end
                    /* eslint-enable */
                    _this.UpdateCurrentRequest(eventArgs);
                });
            }
            return;
        }
        this.flag = true;
        var fileData = [];
        if (!this.multiple) {
            this.clearData(true);
            this.actionCompleteCount = 0;
            targetFiles = [targetFiles[0]];
        }
        for (var i = 0; i < targetFiles.length; i++) {
            var file = directory ? targetFiles[i].file : targetFiles[i];
            this.updateInitialFileDetails(args, targetFiles, file, i, fileData, directory, paste);
        }
        eventArgs.filesData = fileData;
        if (this.allowedExtensions.indexOf('*') > -1) {
            this.allTypes = true;
        }
        if (this.enableHtmlSanitizer) {
            for (var i = 0; i < fileData.length; i++) {
                var sanitizeFile = sfBlazorToolkit.SanitizeHtmlHelper.beforeSanitize();
                var sanitizeFileName = sfBlazorToolkit.SanitizeHtmlHelper.serializeValue(sanitizeFile, fileData[i].name);
                if (sanitizeFileName !== fileData[i].name) {
                    // eslint-disable-next-line no-useless-escape
                    var encodedFileName = targetFiles[i].name.replace(/[\u00A0-\u9999<>\&]/g, function (i) {
                        return '&#' + i.charCodeAt(0) + ';';
                    });
                    fileData[i].name = encodedFileName;
                    fileData[i].status = this.localizedTexts('invalidFileName');
                    fileData[i].statusCode = '0';
                }
            }
        }
        if (!this.allTypes) {
            fileData = this.checkExtension(fileData);
        }
        if (this.selectedEnabled) {
            /* eslint-disable */
            // @ts-ignore-start
            this.dotNetRef.invokeMethodAsync('SelectedEventAsync', eventArgs).then(function (eventArgs) {
                // @ts-ignore-end
                /* eslint-enable */
                _this.UpdateCurrentRequest(eventArgs);
                var fileDataObject = {};
                for (var index = 0, fileDataList = fileData; index < fileDataList.length; index++) {
                    var file = fileDataList[index];
                    fileDataObject[file.id] = file;
                }
                if (eventArgs.isModified && eventArgs.filesData.length > 0) {
                    for (var fileIndex = 0, eventFilesData = eventArgs.filesData; fileIndex < eventFilesData.length; fileIndex++) {
                        var eventFile = eventFilesData[fileIndex];
                        var file = fileDataObject[eventFile.id];
                        if (file) {
                            file.name = eventFile.name;
                        }
                    }
                }
                _this._internalRenderSelect(eventArgs, fileData);
            });
        }
        else {
            this._internalRenderSelect(eventArgs, fileData);
        }
    };
    SfUploader.prototype.updateInitialFileDetails = function (args, targetFiles, file, i, fileData, directory, paste) {
        var fileName = directory ? targetFiles[i].path.substring(1, targetFiles[i].path.length) : paste ?
            sfBlazorToolkit.base.getUniqueID(file.name.substring(0, file.name.lastIndexOf('.'))) + '.' + this.getFileType(file.name) :
            this.directoryUpload ? targetFiles[i].webkitRelativePath : file.name;
        var fileDetails = {
            name: fileName,
            rawFile: file,
            size: file.size,
            status: this.localizedTexts('readyToUploadMessage'),
            type: this.getFileType(file.name),
            mimeContentType: file.type,
            lastModifiedDate: new Date(file.lastModified),
            validationMessages: this.validatedFileSize(file.size),
            statusCode: '1',
            id: sfBlazorToolkit.base.getUniqueID(file.name.substring(0, file.name.lastIndexOf('.'))) + '.' + this.getFileType(file.name)
        };
        if (paste) {
            fileDetails.fileSource = 'paste';
        }
        fileDetails.status = fileDetails.validationMessages.minSize !== '' ? this.localizedTexts('invalidMinFileSize') :
            fileDetails.validationMessages.maxSize !== '' ? this.localizedTexts('invalidMaxFileSize') : fileDetails.status;
        if (fileDetails.validationMessages.minSize !== '' || fileDetails.validationMessages.maxSize !== '') {
            fileDetails.statusCode = '0';
        }
        fileData.push(fileDetails);
    };
    SfUploader.prototype.UpdateCurrentRequest = function (eventArgs) {
        this.currentRequestHeader = [];
        this.currentRequestHeader = eventArgs.currentRequest ? eventArgs.currentRequest : this.currentRequestHeader;
    };
    SfUploader.prototype._internalRenderSelect = function (eventArgs, fileData) {
        if (!eventArgs.cancel) {
            this.currentRequestHeader = eventArgs.currentRequest;
            this.customFormDatas = eventArgs.customFormData;
            this.selectedFiles = this.selectedFiles.concat(fileData);
            this.btnTabIndex = this.disableKeyboardNavigation ? '-1' : '0';
            if (this.showFileList) {
                if (eventArgs.isModified && eventArgs.modifiedFilesData.length > 0) {
                    for (var j = 0; j < eventArgs.modifiedFilesData.length; j++) {
                        for (var k = 0; k < fileData.length; k++) {
                            if (eventArgs.modifiedFilesData[j].id === fileData[k].id) {
                                eventArgs.modifiedFilesData[j].rawFile = fileData[k].rawFile;
                            }
                        }
                    }
                    var dataFiles = this.allTypes ? eventArgs.modifiedFilesData :
                        this.checkExtension(eventArgs.modifiedFilesData);
                    this.updateSortedFileList(dataFiles);
                    this.filesData = this.filesData.concat(dataFiles);
                    if ((!this.isForm || this.allowUpload()) && !(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
                        this.checkAutoUpload(dataFiles);
                    }
                }
                else {
                    this.createFileList(fileData, true);
                    if (this.autoUpload && this.sequenceUpload && this.sequentialUpload && !this.isSaveUrlNotConfigured
                        && !this.isValidTemplate && this.filesData.length > 0 &&
                        this.filesData[this.filesData.length - 1].statusCode !== '2' &&
                        this.filesData[this.filesData.length - 1].statusCode !== '0') {
                        this.filesData = this.filesData.concat(fileData);
                        return;
                    }
                    if (!(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
                        this.filesData = this.filesData.concat(fileData);
                    }
                    if ((!this.isForm || this.allowUpload()) && !(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
                        this.checkAutoUpload(fileData);
                    }
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(eventArgs.progressInterval) && eventArgs.progressInterval !== '') {
                    this.progressInterval = eventArgs.progressInterval;
                }
            }
            else {
                this.filesData = this.filesData.concat(fileData);
                if (this.isSaveUrlNotConfigured) {
                    // tslint:disable-next-line
                    this.dotNetRef.invokeMethodAsync('UpdateServerFileDataAsync', fileData, this.isForm);
                    return;
                }
                if (this.autoUpload) {
                    if (this.sequentialUpload) {
                        this.sequenceUpload(this.filesData);
                    }
                    else {
                        this.upload(this.filesData, true);
                    }
                }
            }
            if (!this.isSaveUrlNotConfigured) {
                for (var item = 0; item < this.filesData.length; item++) {
                    if (this.filesData[item].statusCode === '0') {
                        this.checkActionComplete(true);
                    }
                }
            }
            this.isFirstFileOnSelection = true;
        }
    };
    SfUploader.prototype.allowUpload = function () {
        var allowFormUpload = false;
        if (this.isForm && (!sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.saveUrl) && this.asyncSettings.saveUrl !== '')) {
            allowFormUpload = true;
        }
        return allowFormUpload;
    };
    SfUploader.prototype.isFormUpload = function () {
        var isFormUpload = false;
        if (this.isForm && ((sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.saveUrl) || this.asyncSettings.saveUrl === '')
            && (sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.removeUrl) || this.asyncSettings.removeUrl === ''))) {
            isFormUpload = true;
        }
        return isFormUpload;
    };
    SfUploader.prototype.clearData = function (singleUpload) {
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.listParent) && !(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
            sfBlazorToolkit.base.detach(this.listParent);
            this.listParent = null;
        }
        if (this.browserName !== 'msie' && !singleUpload) {
            this.element.value = '';
        }
        this.fileList = [];
        this.filesData = [];
        if (!this.isSaveUrlNotConfigured) {
            this.removeActionButtons();
        }
    };
    SfUploader.prototype.updateSortedFileList = function (filesData) {
        var previousListClone = sfBlazorToolkit.base.createElement('div', { id: 'clonewrapper' });
        var added = -1;
        if (this.listParent) {
            for (var i = 0; i < this.listParent.querySelectorAll('li').length; i++) {
                var liElement = this.listParent.querySelectorAll('li')[i];
                previousListClone.appendChild(liElement.cloneNode(true));
            }
            this.removeActionButtons();
            var oldList = [].slice.call(previousListClone.childNodes);
            this.createParentList();
            for (var index = 0; index < filesData.length; index++) {
                for (var j = 0; j < this.filesData.length; j++) {
                    if (this.filesData[j].name === filesData[index].name) {
                        this.listParent.appendChild(oldList[j]);
                        sfBlazorToolkit.base.EventHandler.add(oldList[j].querySelector('.e-toolkit-icons'), 'click', this.removeFiles, this);
                        this.fileList.push(oldList[j]);
                        added = index;
                    }
                }
                if (added !== index) {
                    this.createFileList([filesData[index]]);
                }
            }
        }
        else {
            this.createFileList(filesData);
        }
    };
    SfUploader.prototype.isBlank = function (str) {
        return (!str || /^\s*$/.test(str));
    };
    SfUploader.prototype.checkExtension = function (files) {
        var dropFiles = files;
        if (!this.isBlank(this.allowedExtensions)) {
            var allowedExtensions = [];
            var extensions = this.allowedExtensions.split(',');
            for (var index = 0, extensionList = extensions; index < extensionList.length; index++) {
                var extension = extensionList[index];
                allowedExtensions.push(extension.trim().toLocaleLowerCase());
            }
            for (var i = 0; i < files.length; i++) {
                if (allowedExtensions.indexOf(('.' + files[i].type).toLocaleLowerCase()) === -1) {
                    files[i].status = this.localizedTexts('invalidFileType');
                    files[i].statusCode = '0';
                }
            }
        }
        return dropFiles;
    };
    SfUploader.prototype.validatedFileSize = function (fileSize) {
        var minSizeError = '';
        var maxSizeError = '';
        if (fileSize < this.minFileSize) {
            minSizeError = this.localizedTexts('invalidMinFileSize');
        }
        else if (fileSize > this.maxFileSize) {
            maxSizeError = this.localizedTexts('invalidMaxFileSize');
        }
        else {
            minSizeError = '';
            maxSizeError = '';
        }
        var errorMessage = { minSize: minSizeError, maxSize: maxSizeError };
        return errorMessage;
    };
    SfUploader.prototype.isPreLoadFile = function (fileData) {
        var isPreload = false;
        if (this.files) {
            for (var i = 0; i < this.files.length; i++) {
                if (this.files[i].name === fileData.name.slice(0, fileData.name.lastIndexOf('.')) && this.files[i].type === fileData.type) {
                    isPreload = true;
                    break;
                }
            }
        }
        return isPreload;
    };
    SfUploader.prototype.createParentList = function () {
        if (sfBlazorToolkit.base.isNullOrUndefined(this.listParent)) {
            this.listParent = sfBlazorToolkit.base.createElement('ul', { className: LIST_PARENT });
            this.uploadWrapper.appendChild(this.listParent);
        }
    };
    SfUploader.prototype.createFileListElement = function (fileData, files) {
        var fileList = sfBlazorToolkit.base.createElement('li', { className: FILE });
        fileList.setAttribute('data-files-count', fileData.length + '');
        var fileContainer = sfBlazorToolkit.base.createElement('span', { className: TEXT_CONTAINER });
        var statusMessage;
        for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
            var listItem = fileDataList[fileIndex];
            var fileNameEle = sfBlazorToolkit.base.createElement('span', { className: FILE_NAME });
            fileNameEle.innerHTML = this.getFileNameOnly(listItem.name);
            var fileTypeEle = sfBlazorToolkit.base.createElement('span', { className: FILE_TYPE });
            var fileType = this.getFileType(listItem.name);
            fileTypeEle.innerHTML = '.' + fileType;
            if (!fileType) {
                fileTypeEle.classList.add('e-hidden');
            }
            if (!this.enableRtl) {
                fileContainer.appendChild(fileNameEle);
                fileContainer.appendChild(fileTypeEle);
            }
            else {
                var rtlContainer = sfBlazorToolkit.base.createElement('span', { className: RTL_CONTAINER });
                rtlContainer.appendChild(fileTypeEle);
                rtlContainer.appendChild(fileNameEle);
                fileContainer.appendChild(rtlContainer);
            }
            this.truncateName(fileNameEle);
            statusMessage = this.formValidateFileInfo(listItem, fileList);
        }
        fileList.appendChild(fileContainer);
        this.setListToFileInfo(fileData, fileList);
        var index = this.listParent.querySelectorAll('li').length;
        var infoEle = sfBlazorToolkit.base.createElement('span');
        if (fileList.classList.contains(INVALID_FILE)) {
            infoEle.classList.add(STATUS);
            infoEle.classList.add(INVALID_FILE);
            infoEle.innerText = fileData.length > 1 ? this.localizedTexts('invalidFileSelection') : statusMessage;
        }
        else {
            infoEle.classList.add(fileData.length > 1 ? INFORMATION : FILE_SIZE);
            infoEle.innerText = fileData.length > 1 ? this.localizedTexts('totalFiles') + ': ' + fileData.length + ' , '
                + this.localizedTexts('size') + ': ' +
                this.bytesToSize(this.getFileSize(fileData)) : this.bytesToSize(fileData[0].size);
            this.createFormInput(fileData);
        }
        fileContainer.appendChild(infoEle);
        if (sfBlazorToolkit.base.isNullOrUndefined(fileList.querySelector('.e-toolkit-icons'))) {
            var iconElement = sfBlazorToolkit.base.createElement('span', { className: 'e-toolkit-icons', attrs: { 'tabindex': this.btnTabIndex } });
            if (this.browserName === 'msie') {
                iconElement.classList.add('e-msie');
            }
            iconElement.setAttribute('title', this.localizedTexts('remove'));
            iconElement.setAttribute('aria-label', this.localizedTexts('remove'));
            fileList.appendChild(fileContainer);
            fileList.appendChild(iconElement);
            sfBlazorToolkit.base.EventHandler.add(iconElement, 'click', this.removeFiles, this);
            iconElement.classList.add(REMOVE_ICON);
        }
        var eventsArgs = {
            element: fileList,
            fileInfo: this.mergeFileInfo(fileData, fileList),
            index: index,
            isPreload: this.isPreLoadFile(this.mergeFileInfo(fileData, fileList))
        };
        if (this.fileListRenderEnabled) {
            this.dotNetRef.invokeMethodAsync('FileListRenderingEventAsync', eventsArgs);
        }
        this.listParent.appendChild(fileList);
        this.fileList.push(fileList);
    };
    SfUploader.prototype.formValidateFileInfo = function (listItem, fileList) {
        var statusMessage = listItem.status;
        var validationMessages = this.validatedFileSize(listItem.size);
        if (validationMessages.minSize !== '' || validationMessages.maxSize !== '') {
            this.addInvalidClass(fileList);
            statusMessage = validationMessages.minSize !== '' ? this.localizedTexts('invalidMinFileSize') :
                validationMessages.maxSize !== '' ? this.localizedTexts('invalidMaxFileSize') : statusMessage;
        }
        var typeValidationMessage = this.checkExtension(this.getFilesInArray(listItem))[0].status;
        if (typeValidationMessage === this.localizedTexts('invalidFileType')) {
            this.addInvalidClass(fileList);
            statusMessage = typeValidationMessage;
        }
        return statusMessage;
    };
    SfUploader.prototype.addInvalidClass = function (fileList) {
        fileList.classList.add(INVALID_FILE);
    };
    SfUploader.prototype.createFormInput = function (fileData) {
        var inputElement = this.element.cloneNode(true);
        inputElement.classList.add(HIDDEN_INPUT);
        for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
            var listItem = fileDataList[fileIndex];
            listItem.input = inputElement;
        }
        inputElement.setAttribute('name', this.uploaderName);
        this.uploadWrapper.querySelector('.' + INPUT_WRAPPER).appendChild(inputElement);
        if (this.browserName !== 'msie' && this.browserName !== 'edge') {
            this.element.value = '';
        }
    };
    SfUploader.prototype.getFileSize = function (fileData) {
        var fileSize = 0;
        for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
            var file = fileDataList[fileIndex];
            fileSize += file.size;
        }
        return fileSize;
    };
    SfUploader.prototype.mergeFileInfo = function (fileData, fileList) {
        var result = {
            name: '',
            rawFile: '',
            size: 0,
            status: '',
            type: '',
            validationMessages: { minSize: '', maxSize: '' },
            statusCode: '1',
            list: fileList
        };
        var fileNames = [];
        var type = '';
        for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
            var listItem = fileDataList[fileIndex];
            fileNames.push(listItem.name);
            type = listItem.type;
        }
        result.name = fileNames.join(', ');
        result.size = this.getFileSize(fileData);
        result.type = type;
        result.status = this.statusForFormUpload(fileData, fileList);
        return result;
    };
    SfUploader.prototype.statusForFormUpload = function (fileData, fileList) {
        var isValid = true;
        var statusMessage;
        for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
            var listItem = fileDataList[fileIndex];
            statusMessage = listItem.status;
            var validationMessages = this.validatedFileSize(listItem.size);
            if (validationMessages.minSize !== '' || validationMessages.maxSize !== '') {
                isValid = false;
                statusMessage = validationMessages.minSize !== '' ? this.localizedTexts('invalidMinFileSize') :
                    validationMessages.maxSize !== '' ? this.localizedTexts('invalidMaxFileSize') : statusMessage;
            }
            var typeValidationMessage = this.checkExtension(this.getFilesInArray(listItem))[0].status;
            if (typeValidationMessage === this.localizedTexts('invalidFileType')) {
                isValid = false;
                statusMessage = typeValidationMessage;
            }
        }
        if (!isValid) {
            fileList.classList.add(INVALID_FILE);
            statusMessage = fileData.length > 1 ? this.localizedTexts('invalidFileSelection') : statusMessage;
        }
        else {
            statusMessage = this.localizedTexts('totalFiles') + ': ' + fileData.length + ' , '
                + this.localizedTexts('size') + ': ' +
                this.bytesToSize(this.getFileSize(fileData));
        }
        return statusMessage;
    };
    /**
     * Create the file list for specified files data.
     *
     * @param { FileInfo[] } fileData - specifies the files data for file list creation.
     * @param {boolean} isSelectedFile - specifies whether the file is selected or not.
     * @returns {void}
     */
    SfUploader.prototype.createFileList = function (fileData, isSelectedFile) {
        if (this.isSaveUrlNotConfigured || this.isValidTemplate) {
            if (!this.isSaveUrlNotConfigured && this.isValidTemplate) {
                var fileListData = (isSelectedFile) ? this.filesData = this.filesData.concat(fileData) : fileData;
                // tslint:disable-next-line
                this.dotNetRef.invokeMethodAsync('CreateFileListAsync', fileListData, this.isForm);
            }
            else {
                // tslint:disable-next-line
                this.dotNetRef.invokeMethodAsync('CreateFileListAsync', fileData, this.isForm);
            }
        }
        else {
            this.createParentList();
            if (this.isFormUpload()) {
                this.uploadWrapper.classList.add(FORM_UPLOAD);
                this.createFileListElement(fileData, this.element.files);
            }
            else {
                for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
                    var listItem = fileDataList[fileIndex];
                    var liElement = sfBlazorToolkit.base.createElement('li', {
                        className: FILE,
                        attrs: { 'data-file-name': listItem.name, 'data-files-count': '1' }
                    });
                    var textContainer = sfBlazorToolkit.base.createElement('span', { className: TEXT_CONTAINER });
                    var textElement = sfBlazorToolkit.base.createElement('span', { className: FILE_NAME, attrs: { 'title': listItem.name } });
                    textElement.innerHTML = this.getFileNameOnly(listItem.name);
                    var fileExtension = sfBlazorToolkit.base.createElement('span', { className: FILE_TYPE });
                    var fileType = this.getFileType(listItem.name);
                    fileExtension.innerHTML = '.' + fileType;
                    if (!fileType) {
                        fileExtension.classList.add('e-hidden');
                    }
                    if (!this.enableRtl) {
                        textContainer.appendChild(textElement);
                        textContainer.appendChild(fileExtension);
                    }
                    else {
                        var rtlContainer = sfBlazorToolkit.base.createElement('span', { className: RTL_CONTAINER });
                        rtlContainer.appendChild(fileExtension);
                        rtlContainer.appendChild(textElement);
                        textContainer.appendChild(rtlContainer);
                    }
                    var fileSize = sfBlazorToolkit.base.createElement('span', { className: FILE_SIZE });
                    fileSize.innerHTML = this.bytesToSize(listItem.size);
                    textContainer.appendChild(fileSize);
                    var statusElement = sfBlazorToolkit.base.createElement('span', { className: STATUS });
                    textContainer.appendChild(statusElement);
                    statusElement.innerHTML = listItem.status;
                    liElement.appendChild(textContainer);
                    var iconElement = sfBlazorToolkit.base.createElement('span', {
                        className: ' e-toolkit-icons',
                        attrs: { 'tabindex': this.btnTabIndex }
                    });
                    if (this.browserName === 'msie') {
                        iconElement.classList.add('e-msie');
                    }
                    iconElement.setAttribute('title', this.localizedTexts('remove'));
                    iconElement.setAttribute('aria-label', this.localizedTexts('remove'));
                    iconElement.setAttribute('role', 'button');
                    liElement.appendChild(iconElement);
                    sfBlazorToolkit.base.EventHandler.add(iconElement, 'click', this.removeFiles, this);
                    if (listItem.statusCode === '2') {
                        statusElement.classList.add(UPLOAD_SUCCESS);
                        iconElement.classList.add(DELETE_ICON);
                        iconElement.setAttribute('title', this.localizedTexts('delete'));
                        iconElement.setAttribute('aria-label', this.localizedTexts('delete'));
                        iconElement.setAttribute('role', 'button');
                    }
                    else if (listItem.statusCode !== '1') {
                        statusElement.classList.remove(UPLOAD_SUCCESS);
                        statusElement.classList.add(VALIDATION_FAILS);
                    }
                    if (this.autoUpload && listItem.statusCode === '1' && this.asyncSettings.saveUrl !== '') {
                        statusElement.innerHTML = '';
                    }
                    if (!iconElement.classList.contains(DELETE_ICON)) {
                        iconElement.classList.add(REMOVE_ICON);
                    }
                    var index = fileData.indexOf(listItem);
                    var eventsArgs = {
                        element: liElement,
                        fileInfo: listItem,
                        index: index,
                        isPreload: this.isPreLoadFile(listItem)
                    };
                    if (this.fileListRenderEnabled) {
                        this.dotNetRef.invokeMethodAsync('FileListRenderingEventAsync', eventsArgs);
                    }
                    this.listParent.appendChild(liElement);
                    this.fileList.push(liElement);
                    this.truncateName(textElement);
                    var preventActionComplete = this.flag;
                    if (this.isPreLoadFile(listItem)) {
                        this.flag = false;
                        this.checkActionComplete(true);
                        this.flag = preventActionComplete;
                    }
                }
            }
        }
    };
    SfUploader.prototype.getSlicedName = function (nameElement) {
        var text = nameElement.textContent;
        nameElement.dataset.tail = text.slice(text.length - 10);
    };
    SfUploader.prototype.setListToFileInfo = function (fileData, fileList) {
        for (var fileIndex = 0, fileDataList = fileData; fileIndex < fileDataList.length; fileIndex++) {
            var listItem = fileDataList[fileIndex];
            listItem.list = fileList;
        }
    };
    SfUploader.prototype.truncateName = function (name) {
        var nameElement = name;
        if (this.browserName !== 'edge' && nameElement.offsetWidth < nameElement.scrollWidth) {
            this.getSlicedName(nameElement);
        }
        else if (nameElement.offsetWidth + 1 < nameElement.scrollWidth) {
            this.getSlicedName(nameElement);
        }
    };
    SfUploader.prototype.getFileType = function (name) {
        var extension;
        var index = name.lastIndexOf('.');
        if (index >= 0) {
            extension = name.substring(index + 1);
        }
        return extension ? extension : '';
    };
    SfUploader.prototype.getFileNameOnly = function (name) {
        var type = this.getFileType(name);
        var names = name.split('.' + type);
        return type = names[0];
    };
    SfUploader.prototype.setInitialAttributes = function () {
        if (this.initialAttr.accept) {
            this.element.setAttribute('accept', this.initialAttr.accept);
        }
        if (this.initialAttr.disabled) {
            this.element.setAttribute('disabled', 'disabled');
        }
        if (this.initialAttr.multiple) {
            var newAttr = document.createAttribute('multiple');
            this.element.setAttributeNode(newAttr);
        }
    };
    SfUploader.prototype.filterfileList = function (files) {
        var filterFiles = [];
        var li;
        for (var i = 0; i < files.length; i++) {
            li = this.getLiElement(files[i]);
            if (!sfBlazorToolkit.base.isNullOrUndefined(li) && !li.classList.contains(UPLOAD_SUCCESS)) {
                filterFiles.push(files[i]);
            }
        }
        return filterFiles;
    };
    SfUploader.prototype.updateStatus = function (files, status, statusCode, updateLiStatus) {
        if (updateLiStatus === void 0) { updateLiStatus = true; }
        if (!(status === '' || sfBlazorToolkit.base.isNullOrUndefined(status)) && !(statusCode === '' || sfBlazorToolkit.base.isNullOrUndefined(statusCode))) {
            if (this.isSaveUrlNotConfigured) {
                for (var i = 0; i < this.filesData.length; i++) {
                    if (this.filesData[i].name === files.name) {
                        this.filesData[i].status = status;
                        this.filesData[i].statusCode = statusCode;
                    }
                }
            }
            else {
                files.status = status;
                files.statusCode = statusCode;
            }
        }
        if (updateLiStatus) {
            var li = this.getLiElement(files);
            if (!sfBlazorToolkit.base.isNullOrUndefined(li)) {
                if (!sfBlazorToolkit.base.isNullOrUndefined(li.querySelector('.' + STATUS)) && !((status === '' || sfBlazorToolkit.base.isNullOrUndefined(status)))) {
                    li.querySelector('.' + STATUS).textContent = status;
                }
            }
        }
        return files;
    };
    SfUploader.prototype.getLiElement = function (files) {
        var index;
        for (var i = 0; i < this.filesData.length; i++) {
            if ((!sfBlazorToolkit.base.isNullOrUndefined(this.filesData[i].id) && !sfBlazorToolkit.base.isNullOrUndefined(files.id)) ?
                (this.filesData[i].name === files.name && this.filesData[i].id === files.id) :
                this.filesData[i].name === files.name) {
                index = i;
            }
        }
        return this.fileList[index];
    };
    SfUploader.prototype.createProgressBar = function (liElement) {
        var progressbarWrapper = sfBlazorToolkit.base.createElement('span', { className: PROGRESS_WRAPPER });
        var progressBar = sfBlazorToolkit.base.createElement('progressbar', { className: PROGRESSBAR, attrs: { value: '0', max: '100' } });
        var progressbarInnerWrapper = sfBlazorToolkit.base.createElement('span', { className: PROGRESS_INNER_WRAPPER });
        progressBar.setAttribute('style', 'width: 0%');
        var progressbarText = sfBlazorToolkit.base.createElement('span', { className: PROGRESSBAR_TEXT });
        progressbarText.textContent = '0%';
        progressbarInnerWrapper.appendChild(progressBar);
        progressbarWrapper.appendChild(progressbarInnerWrapper);
        progressbarWrapper.appendChild(progressbarText);
        if (liElement.querySelector('.' + TEXT_CONTAINER)) {
            liElement.querySelector('.' + TEXT_CONTAINER).appendChild(progressbarWrapper);
        }
    };
    SfUploader.prototype.updateProgressbar = function (e, li) {
        if (!isNaN(Math.round((e.loaded / e.total) * 100)) && !sfBlazorToolkit.base.isNullOrUndefined(li.querySelector('.' + PROGRESSBAR))) {
            if (!sfBlazorToolkit.base.isNullOrUndefined(this.progressInterval) && this.progressInterval !== '') {
                var value = (Math.round((e.loaded / e.total) * 100)) % parseInt(this.progressInterval, 10);
                if (value === 0 || value === 100) {
                    this.changeProgressValue(li, Math.round((e.loaded / e.total) * 100).toString() + '%');
                }
            }
            else {
                this.changeProgressValue(li, Math.round((e.loaded / e.total) * 100).toString() + '%');
            }
        }
    };
    SfUploader.prototype.changeProgressValue = function (li, progressValue) {
        li.querySelector('.' + PROGRESSBAR).setAttribute('style', 'width:' + progressValue);
        li.querySelector('.' + PROGRESSBAR_TEXT).textContent = progressValue;
    };
    SfUploader.prototype.uploadInProgress = function (e, files, customUI, request) {
        var li = this.getLiElement(files);
        if (sfBlazorToolkit.base.isNullOrUndefined(li) && (!customUI)) {
            return;
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(li)) {
            if (files.statusCode === '5') {
                this.cancelUploadingFile(files, e, request, li);
            }
            if (!(li.querySelectorAll('.' + PROGRESS_WRAPPER).length > 0) && li.querySelector('.' + STATUS)) {
                li.querySelector('.' + STATUS).classList.add(UPLOAD_INPROGRESS);
                this.createProgressBar(li);
                this.updateProgressBarClasses(li, UPLOAD_INPROGRESS);
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_FAILED);
            }
            this.updateProgressbar(e, li);
            var iconEle = li.querySelector('.' + REMOVE_ICON);
            if (!sfBlazorToolkit.base.isNullOrUndefined(iconEle)) {
                iconEle.classList.add(ABORT_ICON, UPLOAD_INPROGRESS);
                iconEle.setAttribute('title', this.localizedTexts('abort'));
                iconEle.setAttribute('aria-label', this.localizedTexts('abort'));
                iconEle.classList.remove(REMOVE_ICON);
            }
        }
        else {
            this.cancelUploadingFile(files, e, request);
        }
        var args = {
            e: e,
            operation: 'upload',
            file: this.updateStatus(files, this.localizedTexts('inProgress'), '3'),
            lengthComputable: e.lengthComputable,
            loaded: e.loaded,
            total: e.total
        };
        if (this.progressingEnabled) {
            this.dotNetRef.invokeMethodAsync('ProgressEventAsync', args);
        }
    };
    SfUploader.prototype.cancelEventCallback = function (files, request, li, eventArgs) {
        var _this = this;
        if (eventArgs.cancel) {
            files.statusCode = '3';
            if (!sfBlazorToolkit.base.isNullOrUndefined(li)) {
                var spinnerTarget = li.querySelector('.' + ABORT_ICON);
                if (!sfBlazorToolkit.base.isNullOrUndefined(spinnerTarget)) {
                    disableSpinner(spinnerTarget);
                    sfBlazorToolkit.base.detach(li.querySelector('.e-spinner-pane'));
                }
            }
        }
        else {
            request.emitError = false;
            request.httpRequest.abort();
            var formData = new FormData();
            if (files.statusCode === '5') {
                var name_1 = this.element.getAttribute('name');
                formData.append(name_1, files.name);
                formData.append('cancel-uploading', files.name);
                var ajax = new sfBlazorToolkit.Ajax(this.asyncSettings.removeUrl, 'POST', true, null);
                ajax.emitError = false;
                ajax.onLoad = function (e) { _this.removecanceledFile(e, files); return {}; };
                ajax.send(formData);
            }
        }
    };
    SfUploader.prototype.cancelUploadingFile = function (files, e, request, li) {
        var _this = this;
        if (files.statusCode === '5') {
            var eventArgs = {
                event: e,
                fileData: files,
                cancel: false
            };
            if (this.cancelEnabled) {
                /* eslint-disable */
                // @ts-ignore-start
                this.dotNetRef.invokeMethodAsync('CancelingEventAsync', eventArgs).then(function (eventArgs) {
                    // @ts-ignore-end
                    /* eslint-enable */
                    _this.cancelEventCallback(files, request, li, eventArgs);
                });
            }
            else {
                this.cancelEventCallback(files, request, li, eventArgs);
            }
        }
    };
    SfUploader.prototype.removecanceledFile = function (e, file) {
        var liElement = this.getLiElement(file);
        if (sfBlazorToolkit.base.isNullOrUndefined(liElement) || liElement.querySelector('.' + RETRY_ICON) || sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + ABORT_ICON))) {
            return;
        }
        this.updateStatus(file, this.localizedTexts('fileUploadCancel'), '5');
        this.renderFailureState(e, file, liElement);
        var spinnerTarget = liElement.querySelector('.' + REMOVE_ICON);
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
            disableSpinner(spinnerTarget);
            sfBlazorToolkit.base.detach(liElement.querySelector('.e-spinner-pane'));
        }
        var requestResponse = e && e.currentTarget ? this.getResponse(e) : null;
        var args = { event: e, response: requestResponse, operation: 'cancel', file: file };
        if (this.successEnabled) {
            this.dotNetRef.invokeMethodAsync('SuccessEventAsync', args);
        }
    };
    SfUploader.prototype.renderFailureState = function (e, file, liElement) {
        var _this = this;
        this.updateProgressBarClasses(liElement, UPLOAD_FAILED);
        this.removeProgressbar(liElement, 'failure');
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.e-file-status'))) {
            liElement.querySelector('.e-file-status').classList.add(UPLOAD_FAILED);
        }
        var deleteIcon = liElement.querySelector('.' + ABORT_ICON);
        if (sfBlazorToolkit.base.isNullOrUndefined(deleteIcon)) {
            return;
        }
        deleteIcon.classList.remove(ABORT_ICON, UPLOAD_INPROGRESS);
        deleteIcon.classList.add(REMOVE_ICON);
        deleteIcon.setAttribute('title', this.localizedTexts('remove'));
        deleteIcon.setAttribute('aria-label', this.localizedTexts('remove'));
        this.pauseButton = sfBlazorToolkit.base.createElement('span', { className: 'e-toolkit-icons e-refresh', attrs: { 'tabindex': this.btnTabIndex } });
        deleteIcon.parentElement.insertBefore(this.pauseButton, deleteIcon);
        this.pauseButton.setAttribute('title', this.localizedTexts('retry'));
        this.pauseButton.setAttribute('aria-label', this.localizedTexts('retry'));
        var retryElement = liElement.querySelector('.' + RETRY_ICON);
        retryElement.addEventListener('click', function (e) { _this.reloadcanceledFile(e, file, liElement, false); }, false);
    };
    SfUploader.prototype.reloadcanceledFile = function (e, file, liElement, custom) {
        file.statusCode = '1';
        file.status = this.localizedTexts('readyToUploadMessage');
        if (!custom) {
            liElement.querySelector('.' + STATUS).classList.remove(UPLOAD_FAILED);
            if (!sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + RETRY_ICON))) {
                sfBlazorToolkit.base.detach(liElement.querySelector('.' + RETRY_ICON));
            }
            this.pauseButton = null;
        }
        liElement.classList.add(RESTRICT_RETRY);
        this.upload([file]);
    };
    SfUploader.prototype.uploadComplete = function (e, file, customUI) {
        var status = e.target;
        if (status.readyState === 4 && status.status >= 200 && status.status <= 299) {
            var li = this.getLiElement(file);
            if (sfBlazorToolkit.base.isNullOrUndefined(li) && (!customUI || sfBlazorToolkit.base.isNullOrUndefined(customUI))) {
                return;
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(li)) {
                this.updateProgressBarClasses(li, UPLOAD_SUCCESS);
                this.removeProgressbar(li, 'success');
                var iconEle = li.querySelector('.' + ABORT_ICON);
                if (!sfBlazorToolkit.base.isNullOrUndefined(iconEle)) {
                    iconEle.classList.add(DELETE_ICON);
                    iconEle.setAttribute('title', this.localizedTexts('delete'));
                    iconEle.setAttribute('aria-label', this.localizedTexts('delete'));
                    iconEle.classList.remove(ABORT_ICON);
                    iconEle.classList.remove(UPLOAD_INPROGRESS);
                }
            }
            this.raiseSuccessEvent(e, file);
        }
        else {
            this.uploadFailed(e, file);
        }
    };
    SfUploader.prototype.getResponse = function (e) {
        // tslint:disable-next-line
        var target = e.currentTarget;
        var response = {
            readyState: target.readyState,
            statusCode: target.status,
            statusText: target.statusText,
            responseText: target.responseText,
            headers: target.getAllResponseHeaders(),
            withCredentials: target.withCredentials
        };
        return response;
    };
    SfUploader.prototype.serverRemoveIconBindEvent = function () {
        if (this.uploadWrapper && this.isSaveUrlNotConfigured) {
            var iconElement = [].slice.call(this.uploadWrapper.querySelectorAll('ul li'));
            for (var i = 0; i < iconElement.length; i++) {
                var removeIconEle = (iconElement[i]) ? iconElement[i].querySelector('.e-toolkit-icons') : null;
                if (removeIconEle) {
                    sfBlazorToolkit.base.EventHandler.remove(removeIconEle, 'click', this.removeFiles);
                    sfBlazorToolkit.base.EventHandler.add(removeIconEle, 'click', this.removeFiles, this);
                }
            }
        }
    };
    SfUploader.prototype.raiseSuccessEvent = function (e, file) {
        var _this = this;
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var statusMessage = this.localizedTexts('uploadSuccessMessage');
        var args = {
            e: e, response: response, operation: 'upload', file: this.updateStatus(file, statusMessage, '2', false), statusText: statusMessage
        };
        if (!this.isSaveUrlNotConfigured) {
            var liElement = this.getLiElement(file);
            if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
                var spinnerEle = liElement.querySelector('.' + SPINNER_PANE);
                if (!sfBlazorToolkit.base.isNullOrUndefined(spinnerEle)) {
                    disableSpinner(liElement);
                    sfBlazorToolkit.base.detach(spinnerEle);
                }
            }
        }
        if (this.successEnabled) {
            /* eslint-disable */
            // @ts-ignore-start
            this.dotNetRef.invokeMethodAsync('SuccessEventAsync', args).then(function (args) {
                // @ts-ignore-end
                /* eslint-enable */
                _this.successEventCallback(file, args);
            });
        }
        else {
            this.successEventCallback(file, args);
        }
    };
    SfUploader.prototype.successEventCallback = function (file, args) {
        // tslint:disable-next-line
        this.updateStatus(file, args.statusText, '2');
        this.uploadedFilesData.push(file);
        if (!this.isSaveUrlNotConfigured && this.changeEnabled) {
            this.dotNetRef.invokeMethodAsync('ChangeEventAsync', { files: this.uploadedFilesData });
        }
        this.checkActionButtonStatus();
        if (this.sequentialUpload && !this.showFileList) {
            this.uploadSequential();
        }
        if (this.fileList.length > 0) {
            if ((!(this.getLiElement(file)).classList.contains(RESTRICT_RETRY))) {
                this.uploadSequential();
                this.checkActionComplete(true);
            }
            else {
                (this.getLiElement(file)).classList.remove(RESTRICT_RETRY);
            }
        }
        else if (!this.showFileList) {
            this.checkActionComplete(true);
        }
    };
    SfUploader.prototype.uploadFailed = function (e, file) {
        var _this = this;
        var li = this.getLiElement(file);
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var statusMessage = this.localizedTexts('uploadFailedMessage');
        var args = {
            e: e, response: response, operation: 'upload', file: this.updateStatus(file, statusMessage, '0', false), statusText: statusMessage,
            retryFiles: null
        };
        if (!sfBlazorToolkit.base.isNullOrUndefined(li)) {
            this.renderFailureState(e, file, li);
        }
        if (this.failuredEnabled) {
            /* eslint-disable */
            // @ts-ignore-start
            this.dotNetRef.invokeMethodAsync('FailureEventAsync', args).then(function (args) {
                // @ts-ignore-end
                /* eslint-enable */
                _this.failureEventCallback(file, args);
            });
        }
        else {
            this.failureEventCallback(file, args);
        }
    };
    SfUploader.prototype.failureEventCallback = function (file, args) {
        // tslint:disable-next-line
        this.updateStatus(file, args.statusText, '0');
        this.checkActionButtonStatus();
        this.uploadSequential();
        this.checkActionComplete(true);
        // tslint:disable-next-line
        var retryFile = args.retryFiles;
        if (retryFile != null) {
            this.retry(retryFile);
        }
    };
    SfUploader.prototype.uploadSequential = function () {
        if (this.sequentialUpload) {
            if (this.autoUpload) {
                this.checkAutoUpload(this.filesData);
            }
            else {
                this.uploadButtonClick();
            }
        }
    };
    SfUploader.prototype.checkActionComplete = function (increment) {
        if (increment) {
            ++this.actionCompleteCount;
        }
        else {
            --this.actionCompleteCount;
        }
        this.raiseActionComplete();
    };
    SfUploader.prototype.raiseActionComplete = function () {
        if ((this.filesData.length === this.actionCompleteCount) && this.flag) {
            this.flag = false;
            // tslint:disable-next-line
            var eventArgs = {
                fileData: []
            };
            eventArgs.fileData = this.getSelectedFileStatus(this.selectedFiles);
            if (this.actionCompleteEnabled) {
                this.dotNetRef.invokeMethodAsync('ActionCompleteEventAsync', eventArgs);
            }
        }
    };
    SfUploader.prototype.getSelectedFileStatus = function (selectedFiles) {
        var matchFiles = [];
        var matchFilesIndex = 0;
        for (var selectFileIndex = 0; selectFileIndex < selectedFiles.length; selectFileIndex++) {
            var selectedFileData = selectedFiles[selectFileIndex];
            for (var fileDataIndex = 0; fileDataIndex < this.filesData.length; fileDataIndex++) {
                if (this.filesData[fileDataIndex].name === selectedFileData.name &&
                    this.filesData[fileDataIndex].status === selectedFileData.status) {
                    matchFiles[matchFilesIndex] = this.filesData[fileDataIndex];
                    ++matchFilesIndex;
                }
            }
        }
        return matchFiles;
    };
    SfUploader.prototype.updateProgressBarClasses = function (li, className) {
        var progressBar = li.querySelector('.' + PROGRESSBAR);
        if (!sfBlazorToolkit.base.isNullOrUndefined(progressBar)) {
            progressBar.classList.add(className);
        }
    };
    SfUploader.prototype.removeProgressbar = function (li, callType) {
        var _this = this;
        if (!sfBlazorToolkit.base.isNullOrUndefined(li.querySelector('.' + PROGRESS_WRAPPER))) {
            this.progressAnimation = new sfBlazorToolkit.Animation({ duration: 1250 });
            this.progressAnimation.animate(li.querySelector('.' + PROGRESS_WRAPPER), { name: 'FadeOut' });
            this.progressAnimation.animate(li.querySelector('.' + PROGRESSBAR_TEXT), { name: 'FadeOut' });
            setTimeout(function () { _this.animateProgressBar(li, callType); }, 750);
        }
    };
    SfUploader.prototype.animateProgressBar = function (li, callType) {
        if (callType === 'success') {
            li.classList.add(UPLOAD_SUCCESS);
            if (!sfBlazorToolkit.base.isNullOrUndefined(li.querySelector('.' + STATUS))) {
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_INPROGRESS);
                this.progressAnimation.animate(li.querySelector('.' + STATUS), { name: 'FadeIn' });
                li.querySelector('.' + STATUS).classList.add(UPLOAD_SUCCESS);
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_FAILED);
            }
        }
        else {
            if (!sfBlazorToolkit.base.isNullOrUndefined(li.querySelector('.' + STATUS))) {
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_INPROGRESS);
                this.progressAnimation.animate(li.querySelector('.' + STATUS), { name: 'FadeIn' });
                li.querySelector('.' + STATUS).classList.add(UPLOAD_FAILED);
            }
        }
        if (li.querySelector('.' + PROGRESS_WRAPPER)) {
            sfBlazorToolkit.base.detach(li.querySelector('.' + PROGRESS_WRAPPER));
        }
    };
    SfUploader.prototype.localizedTexts = function (localeText) {
        return this.localeText[localeText];
    };
    SfUploader.prototype.chunkUpload = function (file, custom, fileIndex) {
        var start = 0;
        var end = Math.min(this.asyncSettings.chunkSize, file.size);
        var index = 0;
        var blob = file.rawFile.slice(start, end);
        this.preventInterval = false;
        var metaData = { chunkIndex: index, blob: blob, file: file, start: start, end: end, retryCount: 0, request: null };
        this.sendRequest(file, metaData, custom, fileIndex);
    };
    SfUploader.prototype.sendRequest = function (file, metaData, custom, fileIndex) {
        var _this = this;
        var formData = new FormData();
        if (!file.rawFile) {
            for (var k = 0; k < this.selectedFiles.length; k++) {
                if (file.id === this.selectedFiles[k].id) {
                    file.rawFile = this.selectedFiles[k].rawFile;
                }
            }
        }
        var blob = file.rawFile.slice(metaData.start, metaData.end);
        formData.append(this.uploaderName, blob, file.name);
        formData.append('chunk-index', metaData.chunkIndex.toString());
        formData.append('chunkIndex', metaData.chunkIndex.toString());
        var totalChunk = Math.max(Math.ceil(file.size / this.asyncSettings.chunkSize), 1);
        formData.append('total-chunk', totalChunk.toString());
        formData.append('totalChunk', totalChunk.toString());
        var isUploadingEventTriggered = false;
        var ajax = new sfBlazorToolkit.Ajax({ url: this.asyncSettings.saveUrl, type: 'POST', async: true, contentType: null });
        ajax.emitError = false;
        ajax.onLoad = function (e) { _this.chunkUploadComplete(e, metaData, custom); return {}; };
        ajax.onUploadProgress = function (e) {
            var isFileSucceed = _this.getLiElement(metaData.file) && _this.getLiElement(metaData.file).querySelector('.e-file-status') ? _this.getLiElement(metaData.file).querySelector('.e-file-status').classList.contains('e-upload-success') : null;
            if (!isFileSucceed) {
                _this.chunkUploadInProgress(e, metaData, custom);
            }
            return {};
        };
        var eventArgs = {
            fileData: file,
            customFormData: [],
            cancel: false,
            chunkSize: this.asyncSettings.chunkSize === 0 ? null : this.asyncSettings.chunkSize
        };
        ajax.beforeSend = function (e) {
            eventArgs.currentRequest = ajax.httpRequest;
            eventArgs.currentChunkIndex = metaData.chunkIndex;
            if (_this.currentRequestHeader) {
                _this.updateCustomheader(ajax.httpRequest, _this.currentRequestHeader);
            }
            if (_this.customFormDatas) {
                _this.updateFormData(formData, _this.customFormDatas);
            }
            if (eventArgs.currentChunkIndex === 0) {
                // This event is currently not required but to avoid breaking changes for previous customer, we have included.
                if (_this.uploadingEnabled) {
                    if (!_this.preventInterval) {
                        _this.isUploadingEventDone = true;
                    }
                    if (!isUploadingEventTriggered) {
                        /* eslint-disable */
                        // @ts-ignore-start
                        _this.dotNetRef.invokeMethodAsync('UploadingEventAsync', eventArgs).then(function (eventArgs) {
                            if (eventArgs.currentRequest && eventArgs.currentRequest.length > 0) {
                                _this.currentRequestHeader = eventArgs.currentRequest;
                                _this.UpdateHttpHeaders();
                            }
                            if (eventArgs.customFormData && eventArgs.customFormData.length > 0) {
                                _this.customFormDatas = eventArgs.customFormData;
                            }
                            _this.preventInterval = true;
                            _this.isRetryUpload = false;
                            _this.isUploadingEventDone = false;
                            // @ts-ignore-end
                            /* eslint-enable */
                            _this.uploadingEventCallback(formData, eventArgs, e, file);
                        });
                        isUploadingEventTriggered = true;
                    }
                }
                else {
                    _this.uploadingEventCallback(formData, eventArgs, e, file);
                }
            }
            else {
                if (_this.chunkUploadingEnabled) {
                    if (eventArgs.currentChunkIndex === 1 && !_this.preventInterval) {
                        _this.isUploadingEventDone = true;
                    }
                    /* eslint-disable */
                    // @ts-ignore-start
                    _this.dotNetRef.invokeMethodAsync('ChunkUploadingEventAsync', eventArgs).then(function (eventArgs) {
                        if (eventArgs.currentRequest && eventArgs.currentRequest.length > 0) {
                            _this.currentRequestHeader = eventArgs.currentRequest;
                            _this.UpdateHttpHeaders();
                        }
                        if (eventArgs.customFormData && eventArgs.customFormData.length > 0) {
                            _this.customFormDatas = eventArgs.customFormData;
                        }
                        _this.preventInterval = true;
                        _this.isUploadingEventDone = false;
                        _this.uploadingEventCallback(formData, eventArgs, e, file);
                        // @ts-ignore-end
                        /* eslint-enable */
                    });
                }
                else {
                    _this.uploadingEventCallback(formData, eventArgs, e, file);
                }
            }
            if (_this.isUploadingEventDone) {
                e.cancel = true;
            }
            var maxTime = 5000; // Maximum wait time of 5 seconds
            var intervalTime = 10; // Interval check every 100ms
            var timePassed = 0;
            if (_this.isUploadingEventDone) {
                var interval_1 = setInterval(function () {
                    if (!_this.isUploadingEventDone) {
                        // Stop the interval if the response is received or max time has passed
                        clearInterval(interval_1);
                        ajax.send(formData);
                    }
                    else if (timePassed >= maxTime) {
                        clearInterval(interval_1);
                        ajax.send(formData);
                    }
                    else {
                        e.cancel = true;
                        // Increment the time passed
                        timePassed += intervalTime;
                    }
                }, intervalTime);
            }
        };
        ajax.onError = function (e) { _this.chunkUploadFailed(e, metaData, custom); return {}; };
        if (this.isRetryUpload) {
            this.customFormDatas = [];
            this.isRetryUpload = false;
        }
        ajax.send(formData);
        metaData.request = ajax;
    };
    SfUploader.prototype.uploadingEventCallback = function (formData, eventArgs, e, file) {
        if (eventArgs.cancel) {
            this.eventCancelByArgs(e, eventArgs, file);
        }
        else if (eventArgs.currentChunkIndex !== 0 && eventArgs.currentChunkIndex !== 1) {
            this.updateFormData(formData, eventArgs.customFormData);
        }
    };
    SfUploader.prototype.eventCancelByArgs = function (e, eventArgs, file) {
        var _this = this;
        e.cancel = true;
        if (eventArgs.fileData.statusCode === '5') {
            return;
        }
        var liElement = this.getLiElement(eventArgs.fileData);
        liElement.querySelector('.' + STATUS).innerHTML = this.localizedTexts('fileUploadCancel');
        liElement.querySelector('.' + STATUS).classList.add(UPLOAD_FAILED);
        eventArgs.fileData.statusCode = '5';
        eventArgs.fileData.status = this.localizedTexts('fileUploadCancel');
        this.pauseButton = sfBlazorToolkit.base.createElement('span', { className: 'e-toolkit-icons e-refresh', attrs: { 'tabindex': this.btnTabIndex } });
        var removeIcon = liElement.querySelector('.' + REMOVE_ICON);
        removeIcon.parentElement.insertBefore(this.pauseButton, removeIcon);
        this.pauseButton.setAttribute('title', this.localizedTexts('retry'));
        this.pauseButton.setAttribute('aria-label', this.localizedTexts('retry'));
        this.pauseButton.addEventListener('click', function (e) { _this.reloadcanceledFile(e, file, liElement); }, false);
        this.checkActionButtonStatus();
    };
    SfUploader.prototype.checkChunkUpload = function () {
        return (this.asyncSettings.chunkSize <= 0 || sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.chunkSize)) ? false : true;
    };
    SfUploader.prototype.chunkUploadComplete = function (e, metaData, custom) {
        var _this = this;
        var response = e.target;
        var liElement;
        if (response.readyState === 4 && response.status >= 200 && response.status < 300) {
            var requestResponse = e && e.currentTarget ? this.getResponse(e) : null;
            var totalChunk = Math.max(Math.ceil(metaData.file.size / this.asyncSettings.chunkSize), 1);
            var eventArgs = {
                event: e,
                file: metaData.file,
                chunkIndex: metaData.chunkIndex,
                totalChunk: totalChunk,
                chunkSize: this.asyncSettings.chunkSize,
                response: requestResponse
            };
            if (this.chunkSuccessEnabled) {
                this.dotNetRef.invokeMethodAsync('ChunkSuccessEventAsync', eventArgs);
            }
            if (sfBlazorToolkit.base.isNullOrUndefined(custom) || !custom) {
                liElement = this.getLiElement(metaData.file);
            }
            this.updateMetaData(metaData);
            if (metaData.end === metaData.file.size) {
                metaData.file.statusCode = '3';
            }
            if (metaData.file.statusCode === '5') {
                var eventArgs_1 = { event: e, fileData: metaData.file, cancel: false };
                if (this.cancelEnabled) {
                    /* eslint-disable */
                    // @ts-ignore-start
                    this.dotNetRef.invokeMethodAsync('CancelingEventAsync', eventArgs_1).then(function (eventArgs) {
                        _this.cancelingEventCallback(metaData, liElement, response, custom, eventArgs);
                        // @ts-ignore-end
                        /* eslint-enable */
                    });
                }
                else {
                    this.cancelingEventCallback(metaData, liElement, response, custom, eventArgs_1);
                }
            }
            else {
                if ((totalChunk - 1) === metaData.chunkIndex && totalChunk > metaData.chunkIndex) {
                    var index = this.pausedData.indexOf(metaData);
                    if (index >= 0) {
                        this.pausedData.splice(index, 1);
                    }
                    if (sfBlazorToolkit.base.isNullOrUndefined(this.template) && (sfBlazorToolkit.base.isNullOrUndefined(custom) || !custom) && liElement) {
                        if (liElement && !sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + PAUSE_UPLOAD))) {
                            sfBlazorToolkit.base.detach(liElement.querySelector('.' + PAUSE_UPLOAD));
                        }
                        this.removeChunkProgressBar(metaData);
                    }
                    this.raiseSuccessEvent(e, metaData.file);
                    return;
                }
                if (metaData.file.statusCode !== '4') {
                    this.sendNextRequest(metaData);
                }
            }
        }
        else {
            this.chunkUploadFailed(e, metaData);
        }
    };
    SfUploader.prototype.cancelingEventCallback = function (metaData, liElement, response, custom, eventArgs) {
        var _this = this;
        if (eventArgs.cancel) {
            metaData.file.statusCode = '3';
            var spinnerTarget = liElement.querySelector('.' + ABORT_ICON);
            if (!sfBlazorToolkit.base.isNullOrUndefined(liElement) && !sfBlazorToolkit.base.isNullOrUndefined(spinnerTarget)) {
                disableSpinner(spinnerTarget);
                sfBlazorToolkit.base.detach(liElement.querySelector('.e-spinner-pane'));
            }
            this.sendNextRequest(metaData);
        }
        else {
            metaData.request.emitError = false;
            response.abort();
            var formData = new FormData();
            var name_2 = this.element.getAttribute('name');
            formData.append(name_2, metaData.file.name);
            formData.append('cancel-uploading', metaData.file.name);
            formData.append('cancelUploading', metaData.file.name);
            var ajax = new sfBlazorToolkit.Ajax(this.asyncSettings.removeUrl, 'POST', true, null);
            ajax.emitError = false;
            ajax.onLoad = function (e) { _this.removeChunkFile(e, metaData, custom); return {}; };
            ajax.send(formData);
        }
    };
    SfUploader.prototype.sendNextRequest = function (metaData) {
        metaData.start = metaData.end;
        metaData.end += this.asyncSettings.chunkSize;
        metaData.end = Math.min(metaData.end, metaData.file.size);
        metaData.chunkIndex += 1;
        this.sendRequest(metaData.file, metaData);
    };
    SfUploader.prototype.removeChunkFile = function (e, metaData, custom) {
        if (sfBlazorToolkit.base.isNullOrUndefined(this.template) && (sfBlazorToolkit.base.isNullOrUndefined(custom) && !custom)) {
            var liElement = this.getLiElement(metaData.file);
            var deleteIcon = liElement.querySelector('.' + ABORT_ICON);
            var spinnerTarget = deleteIcon;
            this.updateStatus(metaData.file, this.localizedTexts('fileUploadCancel'), '5');
            this.updateProgressBarClasses(liElement, UPLOAD_FAILED);
            this.removeProgressbar(liElement, 'failure');
            if (deleteIcon) {
                deleteIcon.classList.remove(ABORT_ICON);
                deleteIcon.classList.add(REMOVE_ICON);
                deleteIcon.setAttribute('title', this.localizedTexts('remove'));
                deleteIcon.setAttribute('aria-label', this.localizedTexts('remove'));
            }
            var pauseIcon = liElement.querySelector('.' + PAUSE_UPLOAD);
            if (pauseIcon) {
                pauseIcon.classList.add(RETRY_ICON);
                pauseIcon.classList.remove(PAUSE_UPLOAD);
                pauseIcon.setAttribute('title', this.localizedTexts('retry'));
                pauseIcon.setAttribute('aria-label', this.localizedTexts('retry'));
            }
            if (!sfBlazorToolkit.base.isNullOrUndefined(liElement) && !sfBlazorToolkit.base.isNullOrUndefined(deleteIcon)
                && !sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.e-spinner-pane'))) {
                disableSpinner(spinnerTarget);
                sfBlazorToolkit.base.detach(liElement.querySelector('.e-spinner-pane'));
            }
        }
    };
    SfUploader.prototype.pauseUpload = function (metaData, e, custom) {
        metaData.file.statusCode = '4';
        metaData.file.status = this.localizedTexts('pause');
        this.updateMetaData(metaData);
        var eventArgs = {
            event: e ? e : null,
            file: metaData.file,
            chunkIndex: metaData.chunkIndex,
            chunkCount: Math.round(metaData.file.size / this.asyncSettings.chunkSize),
            chunkSize: this.asyncSettings.chunkSize
        };
        this.abortUpload(metaData, custom, eventArgs);
        if (this.sequentialUpload) {
            this.uploadSequential();
        }
    };
    SfUploader.prototype.abortUpload = function (metaData, custom, eventArgs) {
        if (metaData.file.statusCode !== '4') {
            metaData.request.emitError = false;
            metaData.request.httpRequest.abort();
        }
        var liElement = this.getLiElement(metaData.file);
        if (sfBlazorToolkit.base.isNullOrUndefined(this.template) && (sfBlazorToolkit.base.isNullOrUndefined(custom) || !custom)) {
            var targetElement = liElement.querySelector('.' + PAUSE_UPLOAD);
            targetElement.classList.remove(PAUSE_UPLOAD);
            targetElement.classList.add(RESUME_UPLOAD);
            targetElement.setAttribute('title', this.localizedTexts('resume'));
            targetElement.setAttribute('aria-label', this.localizedTexts('resume'));
            targetElement.nextElementSibling.classList.add(REMOVE_ICON);
            targetElement.nextElementSibling.classList.remove(ABORT_ICON);
            targetElement.nextElementSibling.setAttribute('title', this.localizedTexts('remove'));
            targetElement.nextElementSibling.setAttribute('aria-label', this.localizedTexts('remove'));
        }
        for (var i = 0; i < this.pausedData.length; i++) {
            if (this.pausedData[i].file.name === metaData.file.name) {
                this.pausedData.splice(i, 1);
            }
        }
        this.pausedData.push(metaData);
        if (this.pausedEnabled) {
            this.dotNetRef.invokeMethodAsync('PausingEventAsync', eventArgs);
        }
    };
    SfUploader.prototype.resumeUpload = function (metaData, e, custom) {
        var liElement = this.getLiElement(metaData.file);
        var targetElement;
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
            targetElement = liElement.querySelector('.' + RESUME_UPLOAD);
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(targetElement) && (sfBlazorToolkit.base.isNullOrUndefined(custom) || !custom)) {
            targetElement.classList.remove(RESUME_UPLOAD);
            targetElement.classList.add(PAUSE_UPLOAD);
            targetElement.setAttribute('title', this.localizedTexts('pause'));
            targetElement.setAttribute('aria-label', this.localizedTexts('pause'));
            targetElement.nextElementSibling.classList.remove(REMOVE_ICON);
            targetElement.nextElementSibling.classList.add(ABORT_ICON);
            targetElement.nextElementSibling.setAttribute('title', this.localizedTexts('abort'));
            targetElement.nextElementSibling.setAttribute('aria-label', this.localizedTexts('abort'));
        }
        metaData.file.status = this.localizedTexts('inProgress');
        metaData.file.statusCode = '3';
        this.updateMetaData(metaData);
        var eventArgs = {
            event: e ? e : null,
            file: metaData.file,
            chunkIndex: metaData.chunkIndex,
            chunkCount: Math.round(metaData.file.size / this.asyncSettings.chunkSize),
            chunkSize: this.asyncSettings.chunkSize
        };
        if (this.resumeEnabled) {
            this.dotNetRef.invokeMethodAsync('ResumingEventAsync', eventArgs);
        }
        for (var i = 0; i < this.pausedData.length; i++) {
            if (this.pausedData[i].end === this.pausedData[i].file.size) {
                this.chunkUploadComplete(e, metaData, custom);
            }
            else {
                if (this.pausedData[i].file.name === metaData.file.name) {
                    this.pausedData[i].end = Math.min(this.pausedData[i].end, this.pausedData[i].file.size);
                    this.sendRequest(this.pausedData[i].file, this.pausedData[i], custom);
                }
            }
        }
    };
    SfUploader.prototype.updateMetaData = function (metaData) {
        if (this.uploadMetaData.indexOf(metaData) === -1) {
            this.uploadMetaData.push(metaData);
        }
        else {
            this.uploadMetaData.splice(this.uploadMetaData.indexOf(metaData), 1);
            this.uploadMetaData.push(metaData);
        }
    };
    SfUploader.prototype.removeChunkProgressBar = function (metaData) {
        var liElement = this.getLiElement(metaData.file);
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
            this.updateProgressBarClasses(liElement, UPLOAD_SUCCESS);
            this.removeProgressbar(liElement, 'success');
            var cancelButton = liElement.querySelector('.' + ABORT_ICON);
            if (!sfBlazorToolkit.base.isNullOrUndefined(cancelButton)) {
                cancelButton.classList.add(DELETE_ICON);
                cancelButton.setAttribute('title', this.localizedTexts('delete'));
                cancelButton.setAttribute('aria-label', this.localizedTexts('delete'));
                cancelButton.classList.remove(ABORT_ICON, UPLOAD_INPROGRESS);
            }
        }
    };
    SfUploader.prototype.chunkUploadFailed = function (e, metaData, custom) {
        var _this = this;
        var chunkCount = Math.max(Math.ceil(metaData.file.size / this.asyncSettings.chunkSize), 1);
        var liElement;
        if (sfBlazorToolkit.base.isNullOrUndefined(this.template) && (sfBlazorToolkit.base.isNullOrUndefined(custom) || !custom)) {
            liElement = this.getLiElement(metaData.file);
        }
        var requestResponse = e && e.currentTarget ? this.getResponse(e) : null;
        var eventArgs = {
            event: e,
            file: metaData.file,
            chunkIndex: metaData.chunkIndex,
            totalChunk: chunkCount,
            chunkSize: this.asyncSettings.chunkSize,
            cancel: false,
            response: requestResponse
        };
        if (this.chunkFailuredEnabled) {
            /* eslint-disable */
            // @ts-ignore-start
            this.dotNetRef.invokeMethodAsync('ChunkFailureEventAsync', eventArgs).then(function (eventArgs) {
                _this.chunkFailureCallback(e, metaData, liElement, requestResponse, custom, eventArgs);
                // @ts-ignore-end
                /* eslint-enable */
            });
        }
        else {
            this.chunkFailureCallback(e, metaData, liElement, requestResponse, custom, eventArgs);
        }
    };
    SfUploader.prototype.chunkFailureCallback = function (e, metaData, liElement, requestResponse, custom, eventArgs) {
        var _this = this;
        // To prevent triggering of failure event
        // tslint:disable-next-line
        if (!eventArgs.cancel) {
            if (metaData.retryCount < this.asyncSettings.retryCount) {
                setTimeout(function () { _this.retryRequest(liElement, metaData, custom); }, this.asyncSettings.retryAfterDelay);
            }
            else {
                if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
                    var pauseButton = liElement.querySelector('.' + PAUSE_UPLOAD) ?
                        liElement.querySelector('.' + PAUSE_UPLOAD) : liElement.querySelector('.' + RESUME_UPLOAD);
                    if (!sfBlazorToolkit.base.isNullOrUndefined(pauseButton)) {
                        pauseButton.classList.add(RETRY_ICON);
                        pauseButton.classList.remove(PAUSE_UPLOAD, RESUME_UPLOAD);
                    }
                    this.updateProgressBarClasses(liElement, UPLOAD_FAILED);
                    this.removeProgressbar(liElement, 'failure');
                    liElement.querySelector('.e-toolkit-icons').classList.remove(UPLOAD_INPROGRESS);
                    var iconElement = liElement.querySelector('.' + ABORT_ICON) ?
                        liElement.querySelector('.' + ABORT_ICON) : liElement.querySelector('.' + REMOVE_ICON);
                    iconElement.classList.remove(ABORT_ICON);
                    if (!sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + PAUSE_UPLOAD))) {
                        sfBlazorToolkit.base.detach(liElement.querySelector('.' + PAUSE_UPLOAD));
                    }
                    if (metaData.start > 0) {
                        iconElement.classList.add(DELETE_ICON);
                        iconElement.setAttribute('title', this.localizedTexts('delete'));
                        iconElement.setAttribute('aria-label', this.localizedTexts('delete'));
                    }
                    else {
                        iconElement.classList.add(REMOVE_ICON);
                        iconElement.setAttribute('title', this.localizedTexts('remove'));
                        iconElement.setAttribute('aria-label', this.localizedTexts('remove'));
                    }
                }
                metaData.retryCount = 0;
                var file_1 = metaData.file;
                var failureMessage = this.localizedTexts('uploadFailedMessage');
                var args = {
                    e: e, response: requestResponse,
                    operation: 'upload',
                    file: this.updateStatus(file_1, failureMessage, '0', false),
                    statusText: failureMessage,
                    retryFiles: null
                };
                if (this.failuredEnabled) {
                    /* eslint-disable */
                    // @ts-ignore-start
                    this.dotNetRef.invokeMethodAsync('FailureEventAsync', args).then(function (args) {
                        // @ts-ignore-end
                        /* eslint-enable */
                        _this.failureCallback(file_1, args);
                    });
                }
                else {
                    this.failureCallback(file_1, args);
                }
            }
        }
    };
    SfUploader.prototype.failureCallback = function (file, args) {
        // tslint:disable-next-line
        this.updateStatus(file, args.statusText, '0');
        this.uploadSequential();
        this.checkActionComplete(true);
        // tslint:disable-next-line
        var retryFile = args.retryFiles;
        if (retryFile != null) {
            this.retry(retryFile);
        }
    };
    SfUploader.prototype.retryRequest = function (liElement, metaData, custom) {
        if (sfBlazorToolkit.base.isNullOrUndefined(this.template) && (sfBlazorToolkit.base.isNullOrUndefined(custom) || !custom) && liElement) {
            this.updateProgressBarClasses(liElement, UPLOAD_FAILED);
        }
        metaData.retryCount += 1;
        this.sendRequest(metaData.file, metaData);
    };
    SfUploader.prototype.checkPausePlayAction = function (e) {
        var targetElement = e.target;
        var selectedElement = e.target.parentElement;
        var index = this.fileList.indexOf(selectedElement);
        var fileData = this.filesData[index];
        var metaData = this.getCurrentMetaData(fileData);
        if (targetElement.classList.contains(PAUSE_UPLOAD)) {
            this.pauseUpload(metaData, e);
        }
        else if (targetElement.classList.contains(RESUME_UPLOAD)) {
            this.resumeUpload(metaData, e);
        }
        else if (targetElement.classList.contains(RETRY_ICON)) {
            if (metaData.file.status === this.localizedTexts('fileUploadCancel')) {
                this.retryUpload(metaData, false);
            }
            else {
                this.retryUpload(metaData, true);
            }
        }
    };
    SfUploader.prototype.retryUpload = function (metaData, fromcanceledStage) {
        if (fromcanceledStage) {
            metaData.end = metaData.end + this.asyncSettings.chunkSize;
            metaData.start = metaData.start + this.asyncSettings.chunkSize;
            this.isRetryUpload = true;
            this.preventInterval = false;
            this.sendRequest(metaData.file, metaData);
        }
        else {
            this.isRetryUpload = true;
            metaData.file.statusCode = '1';
            metaData.file.status = this.localizedTexts('readyToUploadMessage');
            this.chunkUpload(metaData.file);
        }
        (this.getLiElement(metaData.file)).classList.add(RESTRICT_RETRY);
    };
    SfUploader.prototype.chunkUploadInProgress = function (e, metaData, custom) {
        var _this = this;
        if (metaData.file.statusCode === '4') {
            return;
        }
        if (metaData.file.statusCode !== '4' && metaData.file.statusCode !== '5') {
            metaData.file.statusCode = '3';
            metaData.file.status = this.localizedTexts('inProgress');
        }
        this.updateMetaData(metaData);
        var liElement = this.getLiElement(metaData.file);
        if (sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
            return;
        }
        var retryElement = liElement.querySelector('.' + RETRY_ICON);
        if (!sfBlazorToolkit.base.isNullOrUndefined(retryElement)) {
            retryElement.classList.add(PAUSE_UPLOAD);
            retryElement.setAttribute('title', this.localizedTexts('pause'));
            retryElement.setAttribute('aria-label', this.localizedTexts('pause'));
            retryElement.classList.remove(RETRY_ICON);
        }
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement)) {
            if (!(liElement.querySelectorAll('.' + PROGRESS_WRAPPER).length > 0)) {
                var statusElement = liElement.querySelector('.' + STATUS);
                if (sfBlazorToolkit.base.isNullOrUndefined(this.template)) {
                    statusElement.classList.add(UPLOAD_INPROGRESS);
                    statusElement.classList.remove(UPLOAD_FAILED);
                    this.createProgressBar(liElement);
                    this.updateProgressBarClasses(liElement, UPLOAD_INPROGRESS);
                }
                var clearIcon = liElement.querySelector('.' + REMOVE_ICON) ? liElement.querySelector('.' + REMOVE_ICON) :
                    liElement.querySelector('.' + DELETE_ICON);
                if (!sfBlazorToolkit.base.isNullOrUndefined(clearIcon)) {
                    clearIcon.classList.add(ABORT_ICON);
                    clearIcon.setAttribute('title', this.localizedTexts('abort'));
                    clearIcon.setAttribute('aria-label', this.localizedTexts('abort'));
                    clearIcon.classList.remove(REMOVE_ICON);
                }
            }
            if (!isNaN(Math.round((e.loaded / e.total) * 100)) && sfBlazorToolkit.base.isNullOrUndefined(this.template) && metaData.file.statusCode !== '4') {
                var progressVal = void 0;
                var totalChunks = Math.ceil(metaData.file.size / this.asyncSettings.chunkSize) - 1;
                if (this.asyncSettings.chunkSize && totalChunks) {
                    progressVal = Math.round(metaData.chunkIndex / totalChunks * 100);
                    this.changeProgressValue(liElement, progressVal.toString() + '%');
                }
            }
            if (metaData.chunkIndex === 0) {
                this.checkActionButtonStatus();
            }
        }
        if (sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + PAUSE_UPLOAD)) && sfBlazorToolkit.base.isNullOrUndefined(this.template)
            && sfBlazorToolkit.base.isNullOrUndefined(liElement.querySelector('.' + DELETE_ICON))) {
            this.pauseButton = sfBlazorToolkit.base.createElement('span', { className: 'e-toolkit-icons e-pause', attrs: { 'tabindex': this.btnTabIndex } });
            if (this.browserName === 'msie') {
                this.pauseButton.classList.add('e-msie');
            }
            var abortIcon = liElement.querySelector('.' + ABORT_ICON);
            abortIcon.parentElement.insertBefore(this.pauseButton, abortIcon);
            this.pauseButton.setAttribute('title', this.localizedTexts('pause'));
            this.pauseButton.setAttribute('aria-label', this.localizedTexts('pause'));
            this.pauseButton.addEventListener('click', function (e) { _this.checkPausePlayAction(e); }, false);
        }
    };
    /**
     * It is used to convert bytes value into kilobytes or megabytes depending on the size based
     * on [binary prefix](https://en.wikipedia.org/wiki/Binary_prefix).
     *
     * @param { number } bytes - specifies the file size in bytes.
     * @returns { string } - returns the file size in KB or MB format.
     */
    SfUploader.prototype.bytesToSize = function (bytes) {
        var i = -1;
        var units = [' KB', ' MB'];
        if (!bytes) {
            return '0.0 KB';
        }
        do {
            bytes = bytes / 1024;
            i++;
        } while (bytes > 99);
        if (i >= 2) {
            bytes = bytes * 1024;
            i = 1;
        }
        return Math.max(bytes, 0).toFixed(1) + ' ' + units[i];
    };
    /**
     * Allows you to sort the file data alphabetically based on its file name clearly.
     *
     * @param { FileList } filesData - specifies the files data for upload.
     * @returns File[]
     */
    SfUploader.prototype.sortFileList = function (filesData) {
        filesData = filesData ? filesData : this.sortFilesList;
        var files = filesData;
        var fileNames = [];
        for (var i = 0; i < files.length; i++) {
            fileNames.push(files[i].name);
        }
        var sortedFileNames = fileNames.sort();
        var sortedFilesData = [];
        for (var index = 0, sortedFileNamesList = sortedFileNames; index < sortedFileNamesList.length; index++) {
            var name_3 = sortedFileNamesList[index];
            for (var i = 0; i < files.length; i++) {
                if (name_3 === files[i].name) {
                    sortedFilesData.push(files[i]);
                }
            }
        }
        return sortedFilesData;
    };
    /**
     * Removes the component from the DOM and detaches all its related event handlers. Also it removes the attributes and classes.
     *
     * @method destroy
     * @returns {void}.
     */
    SfUploader.prototype.destroy = function () {
        this.element.value = null;
        if (!(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
            this.clearAll();
        }
        this.unWireEvents();
        this.unBindDropEvents();
        this.element.removeAttribute('accept');
        this.setInitialAttributes();
        var attributes = ['aria-label', 'directory', 'webkitdirectory', 'tabindex'];
        for (var index = 0, attributesList = attributes; index < attributesList.length; index++) {
            var key = attributesList[index];
            this.element.removeAttribute(key);
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
    };
    /**
     * Allows you to call the upload process manually by calling save URL action.
     * To process the selected files (added in upload queue), pass an empty argument otherwise
     * upload the specific file based on its argument.
     *
     * @param { FileInfo | FileInfo[] } files - specifies the files data for upload.
     * @param { boolean } custom - specifies whether the file is custom or not.
     * @returns { void } - returns nothing.
     */
    SfUploader.prototype.upload = function (files, custom) {
        var _this = this;
        files = files ? files : this.filesData;
        if (this.sequentialUpload && (this.isFirstFileOnSelection || custom) && this.showFileList) {
            this.sequenceUpload(files);
        }
        else {
            var uploadFiles_1 = this.getFilesInArray(files);
            var eventArgs = {
                customFormData: [],
                currentRequest: null,
                cancel: false,
                withCredentials: false,
                filesData: uploadFiles_1
            };
            if (this.beforeUploadEnabled) {
                /* eslint-disable */
                // @ts-ignore-start
                this.dotNetRef.invokeMethodAsync('BeforeUploadEventAsync', eventArgs).then(function (eventArgs) {
                    _this.beforeUploadCallback(uploadFiles_1, custom, eventArgs);
                    // @ts-ignore-end
                    /* eslint-enable */
                });
            }
            else {
                this.beforeUploadCallback(uploadFiles_1, custom, eventArgs);
            }
        }
    };
    SfUploader.prototype.beforeUploadCallback = function (uploadFiles, custom, eventArgs) {
        if (!eventArgs.cancel) {
            this.currentRequestHeader = eventArgs.currentRequest ? eventArgs.currentRequest : this.currentRequestHeader;
            this.UpdateHttpHeaders();
            this.customFormDatas = [];
            this.customFormDatas = eventArgs.customFormData ? eventArgs.customFormData : this.customFormDatas;
            this.withCredentials = eventArgs.withCredentials ? eventArgs.withCredentials : this.withCredentials;
            this.uploadFiles(uploadFiles, custom);
        }
    };
    SfUploader.prototype.UpdateHttpHeaders = function () {
        var _a;
        this.currentRequestHeader = this.currentRequestHeader || [];
        if (this.httpClientHeaders && this.httpClientHeaders.length > 0) {
            (_a = this.currentRequestHeader).push.apply(_a, this.httpClientHeaders);
        }
    };
    SfUploader.prototype.getFilesInArray = function (files) {
        var uploadFiles = [];
        if (files instanceof Array) {
            uploadFiles = this.getFileListData(files);
        }
        else {
            uploadFiles.push(files);
        }
        return uploadFiles;
    };
    SfUploader.prototype.getFileListData = function (files) {
        var uploadFiles = [];
        if (!sfBlazorToolkit.base.isNullOrUndefined(this.filesData) && !sfBlazorToolkit.base.isNullOrUndefined(files)) {
            var _loop_5 = function (i) {
                uploadFiles.push(this_3.filesData.filter(function (e) {
                    return e.id ? e.id === files[i].id :
                        e.name === files[i].name;
                })[0]);
            };
            var this_3 = this;
            for (var i = 0; i < files.length; i++) {
                _loop_5(i);
            }
        }
        return !sfBlazorToolkit.base.isNullOrUndefined(uploadFiles) ? uploadFiles : [];
    };
    SfUploader.prototype.uploadFiles = function (files, custom) {
        var _this = this;
        var selectedFiles = [];
        if (this.asyncSettings.saveUrl === '' || sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.saveUrl)) {
            // tslint:disable-next-line
            this.dotNetRef.invokeMethodAsync('GetFileDetailsAsync', files);
            return;
        }
        if (!custom || sfBlazorToolkit.base.isNullOrUndefined(custom)) {
            if (!this.multiple) {
                var file = [];
                file.push(files[0]);
                selectedFiles = this.filterfileList(file);
            }
            else {
                selectedFiles = this.filterfileList(files);
            }
        }
        else {
            selectedFiles = files;
        }
        var _loop_6 = function (i) {
            if (!this_4.checkChunkUpload()) {
                /* tslint:disable */
                if (selectedFiles[i] && selectedFiles[i].rawFile instanceof File) {
                    this_4.getBase64(selectedFiles[i].rawFile).then(function (data) {
                        _this.base64String.push(data);
                        _this.uploadFilesRequest(selectedFiles, i, custom);
                    });
                }
                /* tslint:disable */
            }
            else {
                this_4.uploadFilesRequest(selectedFiles, i, custom);
            }
        };
        var this_4 = this;
        for (var i = 0; i < selectedFiles.length; i++) {
            _loop_6(i);
        }
    };
    SfUploader.prototype.uploadFilesRequest = function (selectedFiles, i, custom) {
        var _this = this;
        var cloneFiles = [];
        var chunkEnabled = this.checkChunkUpload();
        var ajax = new sfBlazorToolkit.Ajax(this.asyncSettings.saveUrl, 'POST', true, null);
        ajax.emitError = false;
        var getFileData = selectedFiles.slice(0);
        cloneFiles.push(getFileData[i].rawFile);
        var eventArgs = {
            cancel: false,
            currentRequest: null,
            customFormData: [],
            fileData: getFileData[i]
        };
        var formData = new FormData();
        ajax.beforeSend = function (e) {
            eventArgs.currentRequest = ajax.httpRequest;
            eventArgs.currentRequest.withCredentials = _this.withCredentials;
            eventArgs.fileData.rawFile = !chunkEnabled ? _this.base64String[i] : eventArgs.fileData.rawFile;
            if (_this.currentRequestHeader) {
                _this.updateCustomheader(ajax.httpRequest, _this.currentRequestHeader);
            }
            if (_this.customFormDatas) {
                _this.updateFormData(formData, _this.customFormDatas);
            }
            if (_this.uploadingEnabled) {
                /* eslint-disable */
                // @ts-ignore-start
                _this.dotNetRef.invokeMethodAsync('UploadingEventAsync', eventArgs).then(function (args) {
                    // @ts-ignore-end
                    /* eslint-enable */
                    eventArgs = args;
                    _this.uploadCallback(e, chunkEnabled, selectedFiles, formData, i, cloneFiles, args);
                });
            }
            else {
                _this.uploadCallback(e, chunkEnabled, selectedFiles, formData, i, cloneFiles, eventArgs);
            }
        };
        if (selectedFiles[i].statusCode === '1') {
            var name_4 = this.element.getAttribute('name');
            formData.append(name_4, selectedFiles[i].rawFile, selectedFiles[i].name);
            if (chunkEnabled && selectedFiles[i].size > this.asyncSettings.chunkSize) {
                this.chunkUpload(selectedFiles[i], custom, i);
            }
            else {
                ajax.onLoad = function (e) {
                    if (eventArgs.cancel) {
                        return {};
                    }
                    else {
                        _this.uploadComplete(e, selectedFiles[i], custom);
                        return {};
                    }
                };
                ajax.onUploadProgress = function (e) {
                    if (eventArgs.cancel) {
                        return {};
                    }
                    else {
                        _this.uploadInProgress(e, selectedFiles[i], custom, ajax);
                        return {};
                    }
                };
                ajax.onError = function (e) { _this.uploadFailed(e, selectedFiles[i]); return {}; };
                ajax.send(formData);
            }
        }
    };
    SfUploader.prototype.uploadCallback = function (e, chunkEnabled, selectedFiles, formData, i, cloneFiles, args) {
        if (!chunkEnabled) {
            selectedFiles[i].rawFile = args.fileData.rawFile = cloneFiles[0];
        }
        if (args.cancel) {
            this.eventCancelByArgs(e, args, selectedFiles[i]);
        }
        this.updateFormData(formData, args.customFormData);
    };
    SfUploader.prototype.spliceFiles = function (liIndex) {
        var liElement = this.fileList[liIndex];
        var allFiles = this.getFilesData();
        var nameElements = +liElement.getAttribute('data-files-count');
        var startIndex = 0;
        for (var i = 0; i < liIndex; i++) {
            startIndex += (+this.fileList[i].getAttribute('data-files-count'));
        }
        var endIndex = (startIndex + nameElements) - 1;
        for (var j = endIndex; j >= startIndex; j--) {
            allFiles.splice(j, 1);
        }
    };
    /**
     * Remove the uploaded file from server manually by calling the remove URL action.
     * If you pass an empty argument to this method, the complete file list can be cleared,
     * otherwise remove the specific file based on its argument (“file_data”).
     *
     * @param { FileInfo | FileInfo[] } fileData - specifies the files data to remove from file list/server.
     * @param { boolean } customTemplate - Set true if the component rendering with customize template.
     * @param { boolean } removeDirectly - Set true if files remove without removing event.
     * @param { boolean } postRawFile - Set false, to post file name only to the remove action.
     * @param {MouseEvent | TouchEvent | KeyboardEventArgs} args - specifies the event arguments.
     * @returns { void } - returns nothing.
     */
    SfUploader.prototype.remove = function (fileData, customTemplate, removeDirectly, postRawFile, args) {
        var _this = this;
        if (sfBlazorToolkit.base.isNullOrUndefined(postRawFile)) {
            postRawFile = true;
        }
        var removeFiles = this.getFilesInArray(fileData);
        var beforeEventArgs = {
            cancel: false,
            customFormData: [],
            currentRequest: null,
            filesData: removeFiles,
            postRawFile: postRawFile
        };
        if (this.beforeRemoveEnabled) {
            /* eslint-disable */
            // @ts-ignore-start
            this.dotNetRef.invokeMethodAsync('BeforeRemoveEventAsync', beforeEventArgs).then(function (beforeEventArgs) {
                _this.beforeRemoveCallback(fileData, customTemplate, removeDirectly, args, beforeEventArgs);
                // @ts-ignore-end
                /* eslint-enable */
            });
        }
        else {
            this.beforeRemoveCallback(fileData, customTemplate, removeDirectly, args, beforeEventArgs);
        }
    };
    SfUploader.prototype.beforeRemoveCallback = function (fileData, customTemplate, removeDirectly, args, beforeEventArgs) {
        var _this = this;
        var eventArgs = {
            event: args,
            cancel: false,
            filesData: [],
            customFormData: [],
            postRawFile: beforeEventArgs.postRawFile,
            currentRequest: null
        };
        if (!beforeEventArgs.cancel) {
            this.currentRequestHeader = beforeEventArgs.currentRequest;
            this.customFormDatas = beforeEventArgs.customFormData;
            var index = void 0;
            this.UpdateHttpHeaders();
            if (this.isFormUpload() && !this.isSaveUrlNotConfigured) {
                eventArgs.filesData = fileData;
                /* eslint-disable */
                // @ts-ignore-start
                this.dotNetRef.invokeMethodAsync('RemovingEventAsync', eventArgs).then(function (eventArgs) {
                    // @ts-ignore-end
                    /* eslint-enable */
                    _this.removeCallback(fileData, eventArgs);
                });
            }
            else if (this.isForm && (sfBlazorToolkit.base.isNullOrUndefined(this.asyncSettings.removeUrl) || this.asyncSettings.removeUrl === '')
                && !this.isSaveUrlNotConfigured) {
                eventArgs.filesData = this.getFilesData();
                if (this.removingEnabled) {
                    /* eslint-disable */
                    // @ts-ignore-start
                    this.dotNetRef.invokeMethodAsync('RemovingEventAsync', eventArgs).then(function (eventArgs) {
                        // @ts-ignore-end
                        /* eslint-enable */
                        if (!eventArgs.cancel) {
                            _this.clearAll();
                        }
                    });
                }
                else {
                    this.clearAll();
                }
            }
            else {
                var removeFiles = [];
                fileData = !sfBlazorToolkit.base.isNullOrUndefined(fileData) ? fileData : this.filesData;
                if (fileData instanceof Array) {
                    removeFiles = fileData;
                }
                else {
                    removeFiles.push(fileData);
                }
                eventArgs.filesData = removeFiles;
                var removeUrl = this.asyncSettings.removeUrl;
                var validUrl = (removeUrl === '' || sfBlazorToolkit.base.isNullOrUndefined(removeUrl)) ? false : true;
                var _loop_7 = function (files) {
                    index = this_5.filesData.indexOf(files);
                    var fileUploadedIndex = this_5.uploadedFilesData.indexOf(files);
                    if ((files.statusCode === '2' || files.statusCode === '4' || (files.statusCode === '0' &&
                        fileUploadedIndex !== -1)) && validUrl) {
                        this_5.removeUploadedFile(files, eventArgs, removeDirectly, customTemplate);
                    }
                    else {
                        if (!removeDirectly && this_5.removingEnabled) {
                            /* eslint-disable */
                            // @ts-ignore-start
                            this_5.dotNetRef.invokeMethodAsync('RemovingEventAsync', eventArgs).then(function (eventArgs) {
                                // @ts-ignore-end
                                /* eslint-enable */
                                if (!eventArgs.cancel) {
                                    _this.removeFilesData(files, customTemplate);
                                }
                            });
                        }
                        else {
                            this_5.removeFilesData(files, customTemplate);
                        }
                    }
                    if (args && !args.target.classList.contains(REMOVE_ICON)) {
                        this_5.checkActionComplete(false);
                    }
                };
                var this_5 = this;
                for (var index = 0, removeFilesList = removeFiles; index < removeFilesList.length; index++) {
                    var files = removeFilesList[index];
                    _loop_7(files);
                }
            }
        }
    };
    SfUploader.prototype.removeCallback = function (fileData, eventArgs) {
        if (!eventArgs.cancel) {
            var removingFiles = this.getFilesInArray(fileData);
            var isLiRemoved = false;
            var liIndex = void 0;
            for (var index = 0, removingFilesList = removingFiles; index < removingFilesList.length; index++) {
                var data = removingFilesList[index];
                if (!isLiRemoved) {
                    liIndex = this.fileList.indexOf(data.list);
                }
                if (liIndex > -1) {
                    var inputElement = !sfBlazorToolkit.base.isNullOrUndefined(data.input) ? data.input : null;
                    if (inputElement) {
                        sfBlazorToolkit.base.detach(inputElement);
                    }
                    this.spliceFiles(liIndex);
                    sfBlazorToolkit.base.detach(this.fileList[liIndex]);
                    this.fileList.splice(liIndex, 1);
                    isLiRemoved = true;
                    liIndex = -1;
                }
            }
        }
    };
    /**
     * Clear all the file entries from list that can be uploaded files or added in upload queue.
     *
     * @returns { void } - returns nothing.
     */
    SfUploader.prototype.clearAll = function () {
        var _this = this;
        if (sfBlazorToolkit.base.isNullOrUndefined(this.listParent) && !(this.isSaveUrlNotConfigured || this.isValidTemplate)) {
            if (this.browserName !== 'msie') {
                this.element.value = '';
            }
            this.filesData = [];
            return;
        }
        var eventArgs = {
            cancel: false,
            filesData: this.filesData
        };
        if (this.clearEnabled) {
            /* eslint-disable */
            // @ts-ignore-start
            this.dotNetRef.invokeMethodAsync('ClearingEventAsync', eventArgs).then(function (eventArgs) {
                // @ts-ignore-end
                /* eslint-enable */
                if (!eventArgs.cancel) {
                    _this.clearData();
                    _this.actionCompleteCount = 0;
                    _this.count = -1;
                }
            });
        }
        else {
            this.clearData();
            this.actionCompleteCount = 0;
            this.count = -1;
        }
    };
    /**
     * Get the data of files which are shown in file list.
     *
     * @param { number } index - specifies the file list item(li) index.
     * @returns { FileInfo[] } - returns the files data.
     */
    SfUploader.prototype.getFilesData = function (index) {
        if (sfBlazorToolkit.base.isNullOrUndefined(index)) {
            return this.filesData;
        }
        else {
            return this.getSelectedFiles(index);
        }
    };
    /**
     * Pauses the in-progress chunked upload based on the file data.
     *
     * @param { FileInfo | FileInfo[] } fileData - specifies the files data to pause from uploading.
     * @param { boolean } custom - Set true if used custom UI.
     * @returns { void } - returns nothing.
     */
    SfUploader.prototype.pause = function (fileData, custom) {
        fileData = fileData ? fileData : this.filesData;
        var fileDataFiles = this.getFilesInArray(fileData);
        this.pauseUploading(fileDataFiles, custom);
    };
    SfUploader.prototype.pauseUploading = function (fileData, custom) {
        var files = this.getFiles(fileData);
        for (var i = 0; i < files.length; i++) {
            if (files[i].statusCode === '3') {
                this.pauseUpload(this.getCurrentMetaData(files[i], null), null, custom);
            }
        }
    };
    SfUploader.prototype.getFiles = function (fileData) {
        var files = [];
        if (!sfBlazorToolkit.base.isNullOrUndefined(fileData) && !(fileData instanceof Array)) {
            files.push(fileData);
        }
        else {
            files = fileData;
        }
        return files;
    };
    /**
     * Resumes the chunked upload that is previously paused based on the file data.
     *
     * @param { FileInfo | FileInfo[] } fileData - specifies the files data to resume the paused file.
     * @param { boolean } custom - Set true if used custom UI.
     * @returns { void } - returns nothing.
     */
    SfUploader.prototype.resume = function (fileData, custom) {
        fileData = fileData ? fileData : this.filesData;
        var fileDataFiles = this.getFilesInArray(fileData);
        this.resumeFiles(fileDataFiles, custom);
    };
    SfUploader.prototype.resumeFiles = function (fileData, custom) {
        var files = this.getFiles(fileData);
        for (var i = 0; i < files.length; i++) {
            if (files[i].statusCode === '4') {
                this.resumeUpload(this.getCurrentMetaData(files[i], null), null, custom);
            }
        }
    };
    /**
     * Retries the canceled or failed file upload based on the file data.
     *
     * @param { FileInfo | FileInfo[] } fileData - specifies the files data to retry the canceled or failed file.
     * @param { boolean } fromcanceledStage - Set true to retry from canceled stage and set false to retry from initial stage.
     * @param { boolean } custom - Set true if used custom UI.
     * @returns { void } - returns nothing.
     */
    SfUploader.prototype.retry = function (fileData, fromcanceledStage, custom) {
        fileData = fileData ? fileData : this.filesData;
        var fileDataFiles = this.getFilesInArray(fileData);
        this.retryFailedFiles(fileDataFiles, fromcanceledStage, custom);
    };
    SfUploader.prototype.retryFailedFiles = function (fileData, fromcanceledStage, custom) {
        var files = this.getFiles(fileData);
        for (var i = 0; i < files.length; i++) {
            if (files[i].statusCode === '5' || files[i].statusCode === '0') {
                if (this.asyncSettings.chunkSize > 0 && this.getCurrentMetaData(files[i])) {
                    this.retryUpload(this.getCurrentMetaData(files[i], null), fromcanceledStage);
                }
                else {
                    var liElement = void 0;
                    if (!custom) {
                        liElement = this.fileList[this.filesData.indexOf(files[i])];
                    }
                    this.reloadcanceledFile(null, files[i], liElement, custom);
                }
            }
        }
    };
    /**
     * Stops the in-progress chunked upload based on the file data.
     * When the file upload is canceled, the partially uploaded file is removed from server.
     *
     * @param { FileInfo | FileInfo[] } fileData - specifies the files data to cancel the progressing file.
     * @returns { void}  - returns nothing.
     */
    SfUploader.prototype.cancel = function (fileData) {
        fileData = fileData ? fileData : this.filesData;
        var cancelingFiles = this.getFilesInArray(fileData);
        this.cancelUpload(cancelingFiles);
    };
    SfUploader.prototype.cancelUpload = function (fileData) {
        var files = this.getFiles(fileData);
        if (this.asyncSettings.chunkSize > 0) {
            for (var i = 0; i < files.length; i++) {
                if (files[i].statusCode === '3') {
                    var metaData = this.getCurrentMetaData(files[i], null);
                    metaData.file.statusCode = '5';
                    metaData.file.status = this.localizedTexts('fileUploadCancel');
                    this.updateMetaData(metaData);
                    this.showHideUploadSpinner(files[i]);
                }
            }
        }
        else {
            for (var i = 0; i < files.length; i++) {
                if (files[i].statusCode === '3') {
                    files[i].statusCode = '5';
                    files[i].status = this.localizedTexts('fileUploadCancel');
                    this.showHideUploadSpinner(files[i]);
                }
            }
        }
    };
    SfUploader.prototype.showHideUploadSpinner = function (files) {
        var liElement = this.getLiElement(files);
        if (!sfBlazorToolkit.base.isNullOrUndefined(liElement) && sfBlazorToolkit.base.isNullOrUndefined(this.template)) {
            var spinnerTarget = liElement.querySelector('.' + ABORT_ICON);
            renderSpinner({ target: spinnerTarget, width: '20px' });
            enableSpinner(spinnerTarget);
        }
    };
    return SfUploader;
}());


export function initialize(dataId, element, dotnetRef, options) {
    if (element && dataId) {
        var instance = new SfUploader(dataId, element, dotnetRef, options);
        instance.initialize();
    }
}

export function raiseSuccessEvent(dataId, file) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.raiseSuccessEvent(null, file);
    }
}

export function serverRemoveIconBindEvent(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.serverRemoveIconBindEvent();
    }
}

export function serverFileListElement(dataId, ulElement, btnElement, autoUpload) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.serverUlElement(ulElement);
        if (!autoUpload) {
            instance.serverActionButtonsEventBind(btnElement);
        }
    }
}

export function bytesToSize(dataId, bytes) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        return instance.bytesToSize(bytes);
    }
    return '';
}

export function cancel(dataId, fileData) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.cancel(fileData);
    }
}

export function clearAll(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.clearAll();
    }
}

export function createFileList(dataId, fileData, isSelectedFile) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.createFileList(fileData, isSelectedFile);
    }
}

export function getFilesData(dataId, index) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        return instance.getFilesData(index);
    }
    return null;
}

export function pause(dataId, fileData, custom) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.pause(fileData, custom);
    }
}

export function remove(dataId, fileData, customTemplate, removeDirectly, postRawFile, args) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    for (var i = 0; i < instance.filesData.length; i++) {
        if (fileData && instance.filesData[i].id === fileData[0].id) {
            fileData[0] = instance.filesData[i];
        }
    }
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.remove(fileData, customTemplate, removeDirectly, postRawFile, args);
    }
}

export function resume(dataId, fileData, custom) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.resume(fileData, custom);
    }
}

export function retry(dataId, fileData, fromcanceledStage, custom) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.retry(fileData, fromcanceledStage, custom);
    }
}

export function sortFileList(dataId, fileData) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        return instance.sortFileList(fileData);
    }
    return null;
}

export function upload(dataId, fileData, custom) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.upload(fileData, custom);
    }
}

export function destroy(dataId) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.destroy();
    }
}

export function propertyChanges(dataId, options, newProps) {
    var instance = window.sfBlazorToolkit.base.getCompInstance(dataId);
    if (!sfBlazorToolkit.base.isNullOrUndefined(instance)) {
        instance.propertyChanges(options, newProps);
    }
}
