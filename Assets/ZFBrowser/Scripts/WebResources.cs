using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Acts like a webserver for local files in Assets/../BrowserAssets.
 *
 * To override this, extend the class and call `BrowserNative.webResources = myInstance`
 * before doing anything with Browsers.
 *
 * Important:
 *  - NB: Methods in this class may be called from any thread! Most Unity APIs except Debug.Log
 *    ARE NOT thread-safe. Even concatenating numbers and strings can cause a crash as the system
 *    attempts to create a collation.
 *  - Do not call any BrowserNative.zfb_* methods (directly or indirectly). Doing so may result in deadlock
 *    in certain cases.
 */
public abstract class WebResources {

	/**
	 * Mapping of file extension => HTTP mime type
	 *
	 * Treated as immutable.
	 */
	public static readonly Dictionary<string, string> extensionMimeTypes = new Dictionary<string, string>() {
		{"css", "text/css"},
		{"gif", "image/gif"},
		{"html", "text/html"},
		{"jpeg", "image/jpeg"},
		{"jpg", "image/jpeg"},
		{"js", "application/javascript"},
		{"mp3", "audio/mpeg"},
		{"mpeg", "video/mpeg"},
		{"ogg", "application/ogg"},
		{"ogv", "video/ogg"},
		{"webm", "video/webm"},
		{"png", "image/png"},
		{"svg", "image/svg+xml"},
		{"txt", "text/plain"},
		{"xml", "application/xml"},

		//Need to add something? Look it up here: http://hul.harvard.edu/ois/systems/wax/wax-public-help/mimetypes.htm

		//Default/fallback
		{"*", "application/octet-stream"},
	};

	public struct Response {
		/** HTTP response code */
		public int responseCode;
		public string mimeType;
		public byte[] data;
	}

	private readonly Regex matchDots = new Regex(@"\.[2,]");

	/**
	 * Looks up the data for the given URL.
	 * This may be called from any thread.
	 * Do not call any BrowserNative.zfb_* methods directly or indirectly.
	 * The platform-specific prefix will be removed and you will get a string in this format:
	 * "/index.html"
	 */
	public virtual Response this[string url] {
		get {
			if (string.IsNullOrEmpty(url) || url[0] != '/') {
				return GetError("Invalid path");
			}

			//strip off query string, hash
			if (url.IndexOf('?') >= 0) url = url.Substring(0, url.IndexOf('?'));
			if (url.IndexOf('#') >= 0) url = url.Substring(0, url.IndexOf('#'));

			var path = WWW.UnEscapeURL(url);

			//make sure there's no ".." in url
			path = matchDots.Replace(path, ".");


			//Try to find the appropriate file
			try {
				var data = GetData(path);
				if (data == null) {
					Debug.LogWarning("WebResources: File not found fetching " + path);
					return GetError("Not found", 404);
				}

				var ext = Path.GetExtension(path);
				if (ext.Length > 0) ext = ext.Substring(1);

				string mimeType;
				if (!extensionMimeTypes.TryGetValue(ext, out mimeType)) {
					mimeType = extensionMimeTypes["*"];
				}

				return new Response() {
					mimeType = mimeType,
					data = data,
					responseCode = 200,//OK
				};

			} catch (Exception ex) {
				Debug.LogError("WebResources: Failed to fetch URL " + path);
				Debug.LogException(ex);
				return GetError("Internal error");
			}
		}
	}

	/**
	 * Gets the data for the given file, returning null if it does not exist.
	 * The path will be URL decoded and quasi-normalized.
	 *
	 * This may be called from any thread.
	 * Do not call any BrowserNative.zfb_* methods directly or indirectly.
	 */
	public abstract byte[] GetData(string path);

	public Response GetError(string text, int status = 500) {
		return new Response() {
			data = Encoding.UTF8.GetBytes(text),
			mimeType = "text/plain",
			responseCode = status,
		};
	}

}

}
