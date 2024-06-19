using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZenFulcrum.EmbeddedBrowser {

[Flags]
public enum MouseButton {
	Left = 0x1,
	Middle = 0x2,
	Right = 0x4,
}

public class BrowserInputSettings {
	/**
	 * How fast do we scroll?
	 */
	public int scrollSpeed = 120;
	
	/**
	 * How far can the cursor wander from its position before won't consider another click as a double/triple click?
	 * Value is number of pixels in browser space.
	 */
	public float multiclickTolerance = 6;

	/**
	 * How long must we wait between clicks before we don't consider it a double/triple/etc. click? 
	 * Measured in seconds.
	 */
	public float multiclickSpeed = .7f;

}

/** Proxy for browser input (and current mouse cursor). */
public interface IBrowserUI {

	/** Called once per frame by the browser before fetching properties. */
	void InputUpdate();

	/**
	 * Returns true if the browser will be getting mouse events. Typically this is true when the mouse if over the browser.
	 * 
	 * If this is false, the Mouse* properties will be ignored.
	 */
	bool MouseHasFocus { get; }

	/**
	 * Current mouse position.
	 * 
	 * Returns the current position of the mouse with (0, 0) in the bottom-left corner and (1, 1) in the 
	 * top-right corner.
	 */
	Vector2 MousePosition { get; }

	/** Bitmask of currently depressed mouse buttons */
	MouseButton MouseButtons { get; }

	/**
	 * Delta X and Y scroll values since the last time InputUpdate() was called.
	 * 
	 * Return 1 for every "click" of the scroll wheel.
	 * 
	 * Return only integers.
	 */
	Vector2 MouseScroll { get; }

	/**
	 * Returns true when the browser will receive keyboard events.
	 * 
	 * In the simplest case, return the same value as MouseHasFocus, but you can track focus yourself if desired.
	 * 
	 * If this is false, the Key* properties will be ignored.
	 */
	bool KeyboardHasFocus { get; }

	/**
	 * List of key up/down events that have happened since the last InputUpdate() call.
	 * 
	 * The returned list is not to be altered or retained.
	 */
	List<Event> KeyEvents { get; }

	/**
	 * Returns a BrowserCursor instance. The Browser will update the current cursor to reflect the
	 * mouse's position on the page.
	 * 
	 * The IBrowserUI is responsible for changing the actual cursor, be it the mouse cursor or some in-game display.
	 */
	BrowserCursor BrowserCursor { get; }

	/**
	 * These settings are used to interpret the input data.
	 */
	BrowserInputSettings InputSettings { get; }
	
}

}
