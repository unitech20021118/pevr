using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CursorType = ZenFulcrum.EmbeddedBrowser.BrowserNative.CursorType;
using Object = UnityEngine.Object;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Manages finding and copying cursors for you.
 * Each instance has one active cursor and a Texture2D you can read from to use it.
 */
public class BrowserCursor {
	public class CursorInfo {
		public int atlasOffset;
		public Vector2 hotspot;
	}

	private static Dictionary<CursorType, CursorInfo> mapping = new Dictionary<CursorType, CursorInfo>();

	private static bool loaded = false;

	private static int size;
	private static Texture2D allCursors;

	/** Fired when the mouse cursor's appearance or hotspot changes. */
	public event Action cursorChange = () => {};

	private static void Load() {
		if (loaded) return;
		allCursors = Resources.Load<Texture2D>("Browser/Cursors");
		if (!allCursors) throw new Exception("Failed to find browser allCursors");

		size = allCursors.height;

		var listObj = Resources.Load<TextAsset>("Browser/Cursors");

		foreach (var row in listObj.text.Split('\n')) {
			var info = row.Split(',');

			var k = (CursorType)Enum.Parse(typeof(CursorType), info[0]);
			var v = new CursorInfo() {
				atlasOffset = int.Parse(info[1]),
				hotspot = new Vector2(int.Parse(info[2]), int.Parse(info[3])),
			};
			mapping[k] = v;
		}

		loaded = true;
	}

	/** 
	 * Texture for the current cursor.
	 * If the cursor should be hidden, this will be null.
	 */
	public virtual Texture2D Texture { get; protected set; }

	/** 
	 * Hotspot for the current cursor. (0, 0) indicates the top-left of the texture is the hotspot. 
	 */
	public virtual Vector2 Hotspot { get; protected set; }

	protected Texture2D normalTexture;
	protected Texture2D customTexture;

	public BrowserCursor() {
		Load();

		normalTexture = new Texture2D(size, size, TextureFormat.ARGB32, false);
#if UNITY_EDITOR
		normalTexture.alphaIsTransparency = true;
#endif

		SetActiveCursor(BrowserNative.CursorType.Pointer);
	}

	/** Switches the active cursor type. After calling this you can access the cursor image through this.Texture. */
	public virtual void SetActiveCursor(CursorType type) {
		if (type == CursorType.Custom) throw new ArgumentException("Use SetCustomCursor to set custom cursors.", "type");

		if (type == CursorType.None) {
			Texture = null;
			//Side note: if you copy down a bunch of transparent pixels and try to set the mouse cursor to that
			//both OS X and Windows fail to do what you'd expect.
			cursorChange();
			return;
		}

		var info = mapping[type];
		var pixelData = allCursors.GetPixels(info.atlasOffset * size, 0, size, size);

		Hotspot = info.hotspot;

		normalTexture.SetPixels(pixelData);

		normalTexture.Apply(true);

		Texture = normalTexture;

		cursorChange();
	}

	/** 
	 * Sets a custom cursor. 
	 * {cursor} should be an ARGB texture.
	 */
	public virtual void SetCustomCursor(Texture2D cursor, Vector2 hotspot) {
		if (!customTexture || customTexture.width != cursor.width || customTexture.height != cursor.height) {
			Object.Destroy(customTexture);
			customTexture = new Texture2D(cursor.width, cursor.height, TextureFormat.ARGB32, false);
#if UNITY_EDITOR
			customTexture.alphaIsTransparency = true;
#endif
			}

		customTexture.SetPixels32(cursor.GetPixels32());
		customTexture.Apply(true);

		this.Hotspot = hotspot;

		Texture = customTexture;
		cursorChange();
	}

}

}
