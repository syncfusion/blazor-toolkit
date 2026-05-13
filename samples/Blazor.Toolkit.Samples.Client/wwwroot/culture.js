window.blazorCulture = {
  get: function () {
    try {
      return sessionStorage.getItem('BlazorCulture');
    } catch (e) {
      return null;
    }
  },
  set: function (value) {
    try {
      sessionStorage.setItem('BlazorCulture', value);
    } catch (e) { }
    try {
      // Also set the cookie used by ASP.NET Core RequestLocalization
      var cookieName = '.AspNetCore.Culture';
      var cookieValue = 'c=' + value + '|uic=' + value;
      // set cookie for entire app
      document.cookie = cookieName + '=' + encodeURIComponent(cookieValue) + '; path=/';
    } catch (e) { }
  }
};
