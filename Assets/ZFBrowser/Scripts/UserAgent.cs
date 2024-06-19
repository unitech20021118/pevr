using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Cooks up the user agent to use for the browser.
 *
 * Ideally, we'd just say a little bit and let websites feature-detect, but many websites (sadly)
 * still use UA sniffing to decide what version of the page to give us.
 *
 * Notably, **Google** does this (for example, maps.google.com), which I find rather strange considering
 * that I'd expect them to be among those telling us to feature-detect.
 *
 * So, use this class to generate a pile of turd that looks like every other browser out there acting like
 * every browser that came before it so we get the "real" version of pages when we browse.
 */
public class UserAgent {

	private static string agentOverride;

	/**
	 * Returns a user agent that, hopefully, tricks legacy, stupid, non-feature-detection websites
	 * into giving us their actual content.
	 *
	 * If you change this, the Editor will usually need to be restarted for changes to take effect.
	 */
	public static string GetUserAgent() {
		if (agentOverride != null) return agentOverride;

		var chromeVersion = Marshal.PtrToStringAnsi(BrowserNative.zfb_getVersion());

		//(Note: I don't care what version of the OS you have, we're telling the website you have this
		//version so you get a good site.)
		string osStr = "Windows NT 6.1";
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		osStr = "Macintosh; Intel Mac OS X 10_7_0";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
		osStr = "X11; Linux x86_64";
#endif

		var userAgent =
			"Mozilla/5.0 " +
			"(" + osStr + "; Unity 3D; ZFBrowser 1.1.0; " + Application.productName + " " + Application.version + ") " +
			"AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + chromeVersion + " Safari/537.36"
		;

		//Chromium has issues with non-ASCII user agents.
		userAgent = Regex.Replace(userAgent, @"[^\u0020-\u007E]", "?");

		return userAgent;
	}

	public static void SetUserAgent(string userAgent) {
		if (BrowserNative.NativeLoaded) {
			throw new InvalidOperationException("User Agent can only be changed before native backend is initialized.");
		}

		agentOverride = userAgent;
	}
}
}
