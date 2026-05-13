window.renderModeHelper = {
	notify: function (mode) {
		// Persist to cookie so server-side prerender can read the selected mode
		try { document.cookie = 'renderMode=' + encodeURIComponent(mode) + '; path=/; max-age=' + (60*60*24*30); } catch (e) { }
		// If the mode changed, reload the page so server prerender and static output reflect the new mode.
		setTimeout(function () { location.reload(); }, 120);
	}
	,readCookie: function () {
		try {
			var name = 'renderMode=';
			var decoded = decodeURIComponent(document.cookie || '');
			var parts = decoded.split(';');
			for (var i = 0; i < parts.length; i++) {
				var c = parts[i].trim();
				if (c.indexOf(name) === 0) return c.substring(name.length);
			}
			return null;
		} catch (e) { return null; }
	}
};
