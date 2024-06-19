using System;

namespace ZenFulcrum.EmbeddedBrowser {

public static class Util {

	/**
	 * Sometimes creating a culture in a different thread causes Mono to crash
	 * with mono_class_vtable_full.
	 *
	 * This variant of StartsWith won't try to use a culture.
	 */
	public static bool SafeStartsWith(this string check, string starter) {
		if (check == null || starter == null) return false;

		if (check.Length < starter.Length) return false;

		for (int i = 0; i < starter.Length; ++i) {
			if (check[i] != starter[i]) return false;
		}

		return true;
	}
}

public class JSException : Exception {
	public JSException(string what) : base(what) {}
}


}
