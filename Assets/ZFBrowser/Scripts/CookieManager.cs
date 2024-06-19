using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

public class CookieManager {
	internal readonly Browser browser;


	public CookieManager(Browser browser) {
		this.browser = browser;
	}

	//Keep the trampoline memory alive until the promise is resolved.
	private static readonly Dictionary<IPromise<List<Cookie>>, BrowserNative.GetCookieFunc> cookieFuncs = new Dictionary<IPromise<List<Cookie>>, BrowserNative.GetCookieFunc>();

	/**
	 * Returns a list of all cookies in the browser across all domains.
	 *
	 * Note that cookies are shared between browser instances.
	 *
	 * If the browser is not ready yet (browser.IsReady or WhenReady()) this will return an empty list.
	 */
	public IPromise<List<Cookie>> GetCookies() {
		Cookie.Init();

		var ret = new List<Cookie>();
		if (!browser.IsReady || !browser.enabled) return Promise<List<Cookie>>.Resolved(ret);
		var promise = new Promise<List<Cookie>>();

		BrowserNative.GetCookieFunc cookieFunc = cookie => {
			try {
				if (cookie == null) {
					browser.RunOnMainThread(() => promise.Resolve(ret));
					cookieFuncs.Remove(promise);
					return;
				}

				ret.Add(new Cookie(this, cookie));

			} catch (Exception ex) {
				Debug.LogException(ex);
			}
		};

		BrowserNative.zfb_getCookies(browser.browserId, cookieFunc);

		cookieFuncs[promise] = cookieFunc;

		return promise;
	}

	/**
	 * Deletes all cookies in the browser.
	 */
	public void ClearAll() {
		if (browser.DeferUnready(ClearAll)) return;

		BrowserNative.zfb_clearCookies(browser.browserId);
	}




}

}
