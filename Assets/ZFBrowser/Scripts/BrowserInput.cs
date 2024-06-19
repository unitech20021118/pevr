#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
#define ZF_OSX
#endif
#if UNITY_EDITOR_LINX || UNITY_STANDALONE_LINUX
#define ZF_LINUX
#endif

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/** Helper class for reading data from an IUIHandler, converting it, and feeding it to the native backend. */
internal class BrowserInput {

	private readonly Browser browser;

	public BrowserInput(Browser browser) {
		this.browser = browser;
	}

	private bool kbWasFocused = false;
	private bool mouseWasFocused = false;

	public void HandleInput() {
		browser.UIHandler.InputUpdate();

		if (browser.UIHandler.MouseHasFocus || mouseWasFocused) {
			HandleMouseInput();
		}
		mouseWasFocused = browser.UIHandler.MouseHasFocus;

		if (browser.UIHandler.KeyboardHasFocus) {
			if (!kbWasFocused) {
				BrowserNative.zfb_setFocused(browser.browserId, kbWasFocused = true);
			}
			HandleKeyInput();
		} else {
			if (kbWasFocused) {
				BrowserNative.zfb_setFocused(browser.browserId, kbWasFocused = false);
			}
		}
	}

	private MouseButton prevButtons = 0;
	private Vector2 prevPos;

	private class ButtonHistory {
		public float lastPressTime;
		public int repeatCount;
		public Vector3 lastPosition;

		public void ButtonPress(Vector3 mousePos, IBrowserUI uiHandler, Vector2 browserSize) {
			var now = Time.realtimeSinceStartup;

			if (now - lastPressTime > uiHandler.InputSettings.multiclickSpeed) {
				//too long ago? forget the past
				repeatCount = 0;
			}

			if (repeatCount > 0) {
				//close enough to be a multiclick?
				var p1 = Vector2.Scale(mousePos, browserSize);
				var p2 = Vector2.Scale(lastPosition, browserSize);
				if (Vector2.Distance(p1, p2) > uiHandler.InputSettings.multiclickTolerance) {
					repeatCount = 0;
				}
			}

			repeatCount++;

			lastPressTime = now;
			lastPosition = mousePos;
		}
	}

	private readonly ButtonHistory leftClickHistory = new ButtonHistory();

	private void HandleMouseInput() {
		var handler = browser.UIHandler;
		var mousePos = handler.MousePosition;

		// ReSharper disable CompareOfFloatsByEqualityOperator
		var currentButtons = handler.MouseButtons;
		var mouseScroll = handler.MouseScroll;

		if (mousePos != prevPos) {
			BrowserNative.zfb_mouseMove(browser.browserId, mousePos.x, 1 - mousePos.y);
		}
		if (mouseScroll.sqrMagnitude != 0) {
			BrowserNative.zfb_mouseScroll(
				browser.browserId,
				(int)mouseScroll.x * handler.InputSettings.scrollSpeed, (int)mouseScroll.y * handler.InputSettings.scrollSpeed
			);
		}
		// ReSharper restore CompareOfFloatsByEqualityOperator

		var leftChange = (prevButtons & MouseButton.Left) != (currentButtons & MouseButton.Left);
		var leftDown = (currentButtons & MouseButton.Left) == MouseButton.Left;
		var middleChange = (prevButtons & MouseButton.Middle) != (currentButtons & MouseButton.Middle);
		var middleDown = (currentButtons & MouseButton.Middle) == MouseButton.Middle;
		var rightChange = (prevButtons & MouseButton.Right) != (currentButtons & MouseButton.Right);
		var rightDown = (currentButtons & MouseButton.Right) == MouseButton.Right;

		if (leftChange) {
			if (leftDown) leftClickHistory.ButtonPress(mousePos, handler, browser.Size);
			BrowserNative.zfb_mouseButton(
				browser.browserId, BrowserNative.MouseButton.MBT_LEFT, leftDown,
				leftDown ? leftClickHistory.repeatCount : 0
			);
		}
		if (middleChange) {
			//no double-clicks, to be consistent with other browsers
			BrowserNative.zfb_mouseButton(
				browser.browserId, BrowserNative.MouseButton.MBT_MIDDLE, middleDown, 1
			);
		}
		if (rightChange) {
			//no double-clicks, to be consistent with other browsers
			BrowserNative.zfb_mouseButton(
				browser.browserId, BrowserNative.MouseButton.MBT_RIGHT, rightDown, 1
			);
		}

		prevPos = mousePos;
		prevButtons = currentButtons;
	}

	private void HandleKeyInput() {
		var keyEvents = browser.UIHandler.KeyEvents;
		if (keyEvents.Count == 0) return;

#if ZF_OSX
		ReconstructInputs(keyEvents);
#endif

		foreach (var ev in keyEvents) {
			var keyCode = KeyMappings.GetWindowsKeyCode(ev);
			if (ev.character == '\n') ev.character = '\r';//'cuz that's what Chromium expects

//			if (false) {
//				if (ev.character != 0) Debug.Log("k >>> " + ev.character);
//				else if (ev.type == EventType.KeyUp) Debug.Log("k ^^^ " + ev.keyCode);
//				else if (ev.type == EventType.KeyDown) Debug.Log("k vvv " + ev.keyCode);
//			}

#if ZF_OSX
			FireCommands(ev);
#endif

			if (ev.character != 0 && ev.type == EventType.KeyDown) {
#if ZF_LINUX
				//It seems, on Linux, we don't get keydown, keypress, keyup, we just get a keypress, keyup.
				//So, fire the keydown just before the keypress.
				BrowserNative.zfb_keyEvent(browser.browserId, true, keyCode);
				//Thanks for being consistent, Unity.
#endif

				BrowserNative.zfb_characterEvent(browser.browserId, ev.character, keyCode);
			} else {
				BrowserNative.zfb_keyEvent(browser.browserId, ev.type == EventType.KeyDown, keyCode);
			}
		}
	}


#if ZF_OSX
	/** Used by ReconstructInputs */
	protected HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();

	/**
	 * OS X + Unity has issues.
	 *
	 * Mac editor: If I press cmd+A: The "keydown A" event doesn't get sent,
	 *   though we do get a keypress A and a keyup A.
	 * Mac player: We get duplicate keyUPs normally. If cmd is down we get duplicate keyDOWNs instead.
	 */
	protected void ReconstructInputs(List<Event> keyEvents) {
		for (int i = 0; i < keyEvents.Count; ++i) {//int loop, not iterator, we mutate during iteration
			var ev = keyEvents[i];

			if (ev.type == EventType.KeyDown && ev.character == 0) {
				pressedKeys.Add(ev.keyCode);

				//Repeated keydown events sent in the same frame are always bogus (but valid if in different
				//frames for held key repeats)
				//Remove duplicated key down events in this tick.
				for (int j = i + 1; j < keyEvents.Count; ++j) {
					if (keyEvents[j].Equals(ev)) keyEvents.RemoveAt(j--);
				}
			} else if (ev.type == EventType.KeyDown) {
				//key down with character.
				//...did the key actually get pressed, though?
				if (ev.keyCode != KeyCode.None && !pressedKeys.Contains(ev.keyCode)) {
					//no. insert a keydown before the press
					var downEv = new Event(ev) {
						type = EventType.KeyDown,
						character = (char)0
					};
					keyEvents.Insert(i++, downEv);
					pressedKeys.Add(ev.keyCode);
				}
			} else if (ev.type == EventType.KeyUp) {
				if (!pressedKeys.Contains(ev.keyCode)) {
					//Ignore duplicate key up events
					keyEvents.RemoveAt(i--);
				}

				pressedKeys.Remove(ev.keyCode);
			}

		}
	}

	/**
	 * OS X + Unity has issues.
	 * Commands won't be run if the command is not in the application menu.
	 * Here we trap keystrokes and manually fire the relevant events in the browser.
	 */
	protected void FireCommands(Event ev) {
		if (ev.type != EventType.KeyDown || ev.character != 0 || !ev.command) return;

		switch (ev.keyCode) {
			case KeyCode.C:
				browser.SendFrameCommand(BrowserNative.FrameCommand.Copy);
				break;
			case KeyCode.X:
				browser.SendFrameCommand(BrowserNative.FrameCommand.Cut);
				break;
			case KeyCode.V:
				browser.SendFrameCommand(BrowserNative.FrameCommand.Paste);
				break;
			case KeyCode.A:
				browser.SendFrameCommand(BrowserNative.FrameCommand.SelectAll);
				break;
			case KeyCode.Z:
				if (ev.shift) browser.SendFrameCommand(BrowserNative.FrameCommand.Redo);
				else browser.SendFrameCommand(BrowserNative.FrameCommand.Undo);
				break;
			case KeyCode.Y:
				//I, for one, prefer Y for redo, but shift+Z is more idiomatic on OS X
				//Support both.
				browser.SendFrameCommand(BrowserNative.FrameCommand.Redo);
				break;
		}

	}
#endif

}

}
