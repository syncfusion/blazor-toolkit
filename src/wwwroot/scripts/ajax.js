'use strict';

var headerRegex = /^(.*?):[ \t]*([^\r\n]*)$/gm;
var defaultType = 'GET';
/**
 * Ajax - modernized XMLHttpRequest wrapper preserving original hooks and API.
 *
 * - Keeps compatibility with existing callers that use `new Ajax(...)`.
 * - Uses `const`/`let`, clearer control flow and added JSDoc comments.
 */
class Ajax {
    /**
     * Constructor for Ajax class
     * @param {string|Object} options - URL string or options object
     * @param {string} [type]
     * @param {boolean} [async]
     * @param {string} [contentType]
     */
    constructor(options, type, async, contentType) {
        /** @type {boolean} */
        this.mode = true;
        /** @type {boolean} */
        this.emitError = true;
        this.options = {};

        if (typeof options === 'string') {
            this.url = options;
            this.type = type ? type.toUpperCase() : defaultType;
            this.mode = !sfBlazorToolkit.base.isNullOrUndefined(async) ? async : true;
        }
        else if (typeof options === 'object' && options !== null) {
            this.options = options;
            sfBlazorToolkit.base.extend(this, this.options);
        }

        this.type = this.type ? this.type.toUpperCase() : defaultType;
        this.contentType = (this.contentType !== undefined) ? this.contentType : contentType;
    }

    /**
     * Send the request to server.
     * @param {any} [data]
     * @returns {Promise<any>}
     */
    send(data) {
        this.data = sfBlazorToolkit.base.isNullOrUndefined(data) ? this.data : data;
        const eventArgs = { cancel: false, httpRequest: null };

        return new Promise((resolve, reject) => {
            try {
                this.httpRequest = new XMLHttpRequest();
                this.httpRequest.onreadystatechange = () => this.stateChange(resolve, reject);

                if (!sfBlazorToolkit.base.isNullOrUndefined(this.onLoad)) {
                    this.httpRequest.onload = this.onLoad;
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.onProgress)) {
                    this.httpRequest.onprogress = this.onProgress;
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.onAbort)) {
                    this.httpRequest.onabort = this.onAbort;
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.onError)) {
                    this.httpRequest.onerror = this.onError;
                }
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.onUploadProgress) && this.httpRequest.upload) {
                    this.httpRequest.upload.onprogress = this.onUploadProgress;
                }

                this.httpRequest.open(this.type, this.url, this.mode);

                // Set default headers
                if (!sfBlazorToolkit.base.isNullOrUndefined(this.data) && this.contentType !== null) {
                    this.httpRequest.setRequestHeader('Content-Type', this.contentType || 'application/json; charset=utf-8');
                }

                if (this.beforeSend) {
                    eventArgs.httpRequest = this.httpRequest;
                    this.beforeSend(eventArgs);
                }

                if (!eventArgs.cancel) {
                    this.httpRequest.send(!sfBlazorToolkit.base.isNullOrUndefined(this.data) ? this.data : null);
                }
                else {
                    // resolve as undefined when canceled before sending
                    resolve();
                }
            }
            catch (err) {
                reject(err);
            }
        });
    }

    /**
     * Success hook
     * @param {any} data
     * @returns {any}
     */
    successHandler(data) {
        if (typeof this.onSuccess === 'function') {
            this.onSuccess(data, this);
        }
        return data;
    }

    /**
     * Failure hook
     * @param {any} reason
     * @returns {any}
     */
    failureHandler(reason) {
        if (typeof this.onFailure === 'function') {
            this.onFailure(this.httpRequest);
        }
        return reason;
    }

    /**
     * Internal readyState handler
     * @param {Function} resolve
     * @param {Function} reject
     */
    stateChange(resolve, reject) {
        let data = this.httpRequest.responseText;

        if (this.dataType && this.dataType.toLowerCase() === 'json') {
            if (data === '') {
                data = undefined;
            }
            else {
                try {
                    data = JSON.parse(data);
                }
                catch (error) {
                    // keep original string if JSON.parse fails
                }
            }
        }

        if (this.httpRequest.readyState === 4) {
            const status = this.httpRequest.status;
            if ((status >= 200 && status <= 299) || status === 304) {
                resolve(this.successHandler(data));
            }
            else {
                if (this.emitError) {
                    reject(new Error(this.failureHandler(this.httpRequest.statusText)));
                }
                else {
                    resolve();
                }
            }
        }
    }

    /**
     * To get the response header from XMLHttpRequest
     * @param {string} key
     * @returns {string|null}
     */
    getResponseHeader(key) {
        const responseHeaders = {};
        // headerRegex is expected to be defined elsewhere in this file/environment
        let headers = headerRegex.exec(this.httpRequest.getAllResponseHeaders());
        while (headers) {
            responseHeaders[headers[1].toLowerCase()] = headers[2];
            headers = headerRegex.exec(this.httpRequest.getAllResponseHeaders());
        }
        const header = responseHeaders[key.toLowerCase()];
        return sfBlazorToolkit.base.isNullOrUndefined(header) ? null : header;
    }
}

window.sfBlazorToolkit = window.sfBlazorToolkit || {};
window.sfBlazorToolkit.Ajax = Ajax;