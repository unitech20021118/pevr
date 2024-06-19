using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZenFulcrum.EmbeddedBrowser {

/** Attach this script to a GUI Image to use a browser on it. */
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(Browser))]
public class GUIBrowserUI :
	MonoBehaviour,
	IBrowserUI,
	ISelectHandler, IDeselectHandler,
	IPointerEnterHandler, IPointerExitHandler,
	IPointerDownHandler
{
	protected RawImage myImage;
	protected Browser browser;

	public bool enableInput = true;

	protected void Awake() {
		BrowserCursor = new BrowserCursor();
		InputSettings = new BrowserInputSettings();

		browser = GetComponent<Browser>();
		myImage = GetComponent<RawImage>();

		browser.afterResize += UpdateTexture;
		browser.UIHandler = this;
		BrowserCursor.cursorChange += () => {
			SetCursor(BrowserCursor);
		};

		rTransform = GetComponent<RectTransform>();
	}

	protected void OnEnable() {
		StartCoroutine(WatchResize());
	}

	/** Automatically resizes the browser to match the size of this object. */
	private IEnumerator WatchResize() {
		Rect currentSize = new Rect();

		while (enabled) {
			var rect = rTransform.rect;

			if (rect.size.x <= 0 || rect.size.y <= 0) rect.size = new Vector2(512, 512);
			if (rect.size != currentSize.size) {
				browser.Resize((int)rect.size.x, (int)rect.size.y);
				currentSize = rect;
			}

			//yield return new WaitForSeconds(.5f); won't work if you pause the game, which, BTW, is a great time to resize the screen ;-)
			yield return null;
		}
	}

	protected void UpdateTexture(Texture2D texture) {
		myImage.texture = texture;
		myImage.uvRect = new Rect(0, 0, 1, 1);
	}

	protected List<Event> keyEvents = new List<Event>();
	protected List<Event> keyEventsLast = new List<Event>();
	protected BaseRaycaster raycaster;
	protected RectTransform rTransform;
//	protected List<RaycastResult> raycastResults = new List<RaycastResult>();

	public virtual void InputUpdate() {
		var tmp = keyEvents;
		keyEvents = keyEventsLast;
		keyEventsLast = tmp;
		keyEvents.Clear();

		if (MouseHasFocus) {
			if (!raycaster) raycaster = GetComponentInParent<BaseRaycaster>();

//			raycastResults.Clear();

//			raycaster.Raycast(data, raycastResults);

//			if (raycastResults.Count != 0) {
//				Vector2 pos = raycastResults[0].stuff
				Vector2 pos;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(
					(RectTransform)transform, Input.mousePosition, raycaster.eventCamera, out pos
				);
				pos.x = pos.x / rTransform.rect.width + .5f;
				pos.y = pos.y / rTransform.rect.height + .5f;
				MousePosition = pos;


				var buttons = (MouseButton)0;
				if (Input.GetMouseButton(0)) buttons |= MouseButton.Left;
				if (Input.GetMouseButton(1)) buttons |= MouseButton.Right;
				if (Input.GetMouseButton(2)) buttons |= MouseButton.Middle;
				MouseButtons = buttons;



				MouseScroll = Input.mouseScrollDelta;
		} else {
			MouseButtons = 0;
		}


		//Unity doesn't include events for some keys, so fake it.
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
			//Note: doesn't matter if left or right shift, the browser can't tell.
			//(Prepend event. We don't know what happened first, but pressing shift usually precedes other key presses)
			keyEventsLast.Insert(0, new Event() { type = EventType.KeyDown, keyCode = KeyCode.LeftShift });
		}

		if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
			//Note: doesn't matter if left or right shift, the browser can't tell.
			keyEventsLast.Add(new Event() { type = EventType.KeyUp, keyCode = KeyCode.LeftShift });
		}
	}

	public void OnGUI() {
		var ev = Event.current;
		if (ev.type != EventType.KeyDown && ev.type != EventType.KeyUp) return;

		keyEvents.Add(new Event(ev));
	}

	protected virtual void SetCursor(BrowserCursor newCursor) {
		if (!_mouseHasFocus && newCursor != null) return;

		if (newCursor == null) {
			Cursor.visible = true;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		} else {
			if (newCursor.Texture != null) {
				Cursor.visible = true;
				Cursor.SetCursor(newCursor.Texture, newCursor.Hotspot, CursorMode.Auto);
			} else {
				Cursor.visible = false;
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
		}
	}

	protected bool _mouseHasFocus;
	public bool MouseHasFocus { get { return _mouseHasFocus && enableInput; } }
	public Vector2 MousePosition { get; private set; }
	public MouseButton MouseButtons { get; private set; }
	public Vector2 MouseScroll { get; private set; }
	protected bool _keyboardHasFocus;
	public bool KeyboardHasFocus { get { return _keyboardHasFocus && enableInput; } }
	public List<Event> KeyEvents { get { return keyEventsLast; } }
	public BrowserCursor BrowserCursor { get; private set; }
	public BrowserInputSettings InputSettings { get; private set; }

	public void OnSelect(BaseEventData eventData) {
		_keyboardHasFocus = true;
	}

	public void OnDeselect(BaseEventData eventData) {
		_keyboardHasFocus = false;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		_mouseHasFocus = true;
		SetCursor(BrowserCursor);
	}

	public void OnPointerExit(PointerEventData eventData) {
		_mouseHasFocus = false;
		SetCursor(null);
	}


	public void OnPointerDown(PointerEventData eventData) {
		EventSystem.current.SetSelectedGameObject(gameObject);
	}
}

}
