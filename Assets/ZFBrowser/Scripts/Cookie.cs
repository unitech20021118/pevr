using System;
using System.Text.RegularExpressions;
using UnityEngine;
using NativeCookie = ZenFulcrum.EmbeddedBrowser.BrowserNative.NativeCookie;

namespace ZenFulcrum.EmbeddedBrowser {
	public class Cookie {

		public static void Init() {
			//Empty function on this class to call so we can get the cctor to call on the correct thread.
			//(Regex construction tends to crash if it tries to run from certain threads.)
		}


		private CookieManager cookies;

		private NativeCookie original;

		public string name = "", value = "", domain = "", path = "";
		/** Creation/access time of the cookie. Mostly untested/unsupported at present. */
		public DateTime creation, lastAccess;
		/** Null for normal cookies, a time for cookies that expire. Mostly untested/unsupported at present. */
		public DateTime? expires;
		public bool secure, httpOnly;

		public Cookie(CookieManager cookies) {
			this.cookies = cookies;
		}

		internal Cookie(CookieManager cookies, NativeCookie cookie) {
			this.cookies = cookies;
			original = cookie;
			Copy(original, this);
		}

		/** Deletes this cookie from the browser. */
		public void Delete() {
			if (original == null) return;

			BrowserNative.zfb_editCookie(cookies.browser.browserId, original, BrowserNative.CookieAction.Delete);
			original = null;
		}

		/** Updates any changes to this cookie in the browser, creating the cookie if it's new. */
		public void Update() {
			if (original != null) Delete();

			original = new NativeCookie();
			Copy(this, original);

			BrowserNative.zfb_editCookie(cookies.browser.browserId, original, BrowserNative.CookieAction.Create);
		}

		static readonly Regex dateRegex = new Regex(@"(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2}).(\d{3})");

		public static void Copy(NativeCookie src, Cookie dest) {
			dest.name = src.name;
			dest.value = src.value;
			dest.domain = src.domain;
			dest.path = src.path;

			Func<string, DateTime> convert = s => {
				var m = dateRegex.Match(s);

				return new DateTime(
					int.Parse(m.Groups[1].ToString()),
					int.Parse(m.Groups[2].ToString()),
					int.Parse(m.Groups[3].ToString()),
					int.Parse(m.Groups[4].ToString()),
					int.Parse(m.Groups[5].ToString()),
					int.Parse(m.Groups[6].ToString()),
					int.Parse(m.Groups[7].ToString())
				);
			};

			dest.creation = convert(src.creation);
			dest.expires = src.expires == null ? (DateTime?)null : convert(src.expires);
			dest.lastAccess = convert(src.lastAccess);

			dest.secure = src.secure != 0;
			dest.httpOnly = src.httpOnly != 0;
		}

		public static void Copy(Cookie src, NativeCookie dest) {
			dest.name = src.name;
			dest.value = src.value;
			dest.domain = src.domain;
			dest.path = src.path;

			Func<DateTime, string> convert = s => s.ToString("yyyy-MM-dd hh:mm:ss.fff");

			dest.creation = convert(src.creation);
			dest.expires = src.expires == null ? null : convert(src.expires.Value);
			dest.lastAccess = convert(src.lastAccess);

			dest.secure = src.secure ? (byte)1 : (byte)0;
			dest.httpOnly = src.httpOnly ? (byte)1 : (byte)0;
		}
	}
}
