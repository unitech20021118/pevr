
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {
static class KeyMappings {
	private static Dictionary<KeyCode, int> mappings = new Dictionary<KeyCode, int>() {
		//I'm not gonna lie. I just opened http://www.w3.org/2002/09/tests/keys.html
		//and copied down the values my keyboard produced. >_<
		{KeyCode.Escape, 27},
		{KeyCode.F1, 112},
		{KeyCode.F2, 113},
		{KeyCode.F3, 114},
		{KeyCode.F4, 115},
		{KeyCode.F5, 116},
		{KeyCode.F6, 117},
		{KeyCode.F7, 118},
		{KeyCode.F8, 119},
		{KeyCode.F9, 120},
		{KeyCode.F10, 121},
		{KeyCode.F11, 122},
		{KeyCode.F12, 123},
		{KeyCode.SysReq, 44}, {KeyCode.Print, 44},
		{KeyCode.ScrollLock, 145},
		{KeyCode.Pause, 19},
		{KeyCode.BackQuote, 192},


		{KeyCode.Alpha0, 48},
		{KeyCode.Alpha1, 49},
		{KeyCode.Alpha2, 50},
		{KeyCode.Alpha3, 51},
		{KeyCode.Alpha4, 52},
		{KeyCode.Alpha5, 53},
		{KeyCode.Alpha6, 54},
		{KeyCode.Alpha7, 55},
		{KeyCode.Alpha8, 56},
		{KeyCode.Alpha9, 57},
		{KeyCode.Minus, 189},
		{KeyCode.Equals, 187},
		{KeyCode.Backspace, 8},

		{KeyCode.Tab, 9},
		//char keys
		{KeyCode.LeftBracket, 219},
		{KeyCode.RightBracket, 221},
		{KeyCode.Backslash, 220},

		{KeyCode.CapsLock, 20},
		//char keys
		{KeyCode.Semicolon, 186},
		{KeyCode.Quote, 222},
		{KeyCode.Return, 13},

		{KeyCode.LeftShift, 16},
		//char keys
		{KeyCode.Comma, 188},
		{KeyCode.Period, 190},
		{KeyCode.Slash, 191},
		{KeyCode.RightShift, 16},

		{KeyCode.LeftControl, 17},
		{KeyCode.LeftCommand, 91}, {KeyCode.LeftWindows, 91},
		{KeyCode.LeftAlt, 18},
		{KeyCode.Space, 32},
		{KeyCode.RightAlt, 18},
		{KeyCode.RightCommand, 92}, {KeyCode.RightWindows, 92},
		{KeyCode.Menu, 93},
		{KeyCode.RightControl, 17},


		{KeyCode.Insert, 45},
		{KeyCode.Home, 36},
		{KeyCode.PageUp, 33},

		{KeyCode.Delete, 46},
		{KeyCode.End, 35},
		{KeyCode.PageDown, 34},

		{KeyCode.UpArrow, 38},
		{KeyCode.LeftArrow, 37},
		{KeyCode.DownArrow, 40},
		{KeyCode.RightArrow, 39},


		{KeyCode.Numlock, 144},
		{KeyCode.KeypadDivide, 111},
		{KeyCode.KeypadMultiply, 106},
		{KeyCode.KeypadMinus, 109},		
		
		{KeyCode.Keypad7, 103},
		{KeyCode.Keypad8, 104},
		{KeyCode.Keypad9, 105},
		{KeyCode.KeypadPlus, 107},

		{KeyCode.Keypad4, 100},
		{KeyCode.Keypad5, 101},
		{KeyCode.Keypad6, 102},

		{KeyCode.Keypad1, 97},
		{KeyCode.Keypad2, 98},
		{KeyCode.Keypad3, 99},
		{KeyCode.KeypadEnter, 13},

		{KeyCode.Keypad0, 96},
		{KeyCode.KeypadPeriod, 110},
	};

	public static int GetWindowsKeyCode(Event ev) {
		int ukc = (int)ev.keyCode;//unity key code

		//When dealing with characters return the Unicode char as the keycode.
		if (ukc == 0) {
			//enter is special.
			if (ev.character == 10) return 13;

			return ev.character;
		}


		if (ukc >= (int)KeyCode.A && ukc <= (int)KeyCode.Z) {
			return ukc - (int)KeyCode.A + 65;
		}

		int ret;
		if (mappings.TryGetValue(ev.keyCode, out ret)) {
			return ret;
		}


		//Don't recognize it, we'll just have to return something, but it's almost sure to be wrong.
		Debug.LogWarning("Unknown key mapping: " + ev);
		return ukc;
	}
}
}
