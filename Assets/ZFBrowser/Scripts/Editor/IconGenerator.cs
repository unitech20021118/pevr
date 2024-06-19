//This file is partially subject to Chromium's BSD license, read the class notes for more details.


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using CursorType = ZenFulcrum.EmbeddedBrowser.BrowserNative.CursorType;

/**
 * Utility for generating the cursor icons.
 *
 * This isn't really here for general usage, but if you're willing to read the source and
 * fiddle with things this may give you a head start from starting with nothing.
 *
 * The default icons are pulled from
 * https://chromium.googlesource.com/chromium/src.git/+/master/ui/resources/default_100_percent/common/pointers/
 * https://chromium.googlesource.com/chromium/src.git/+/master/ui/resources/ui_resources.grd
 * and
 * https://chromium.googlesource.com/chromium/src.git/+/master/ui/base/cursor/cursors_aura.cc
 * This tool is used with a local directory, {IconGenerator.path}, filled with those icons.
 *
 * You also need to add a "loading.png" to the folder.
 *
 * To use this script, update the local path to your icons, define ZF_ICON_GENERATOR, and run it in
 * from Assets->ZF Browser->Generate Icons.
 */
[ExecuteInEditMode]
public class IconGenerator {
	private const string path = @"/my/path/to/chromium/ui-resources/default_100_percent/common/pointers";
	private const string destAsset = "ZFBrowser/Resources/Browser/Cursors";

	public static bool useBig = false;

#if ZF_ICON_GENERATOR
	[MenuItem("Assets/ZF Browser/Generate Icons")]
#endif
	public static void GenerateIcons() {
		var icons = new SortedDictionary<string, Texture2D>();

		var w = -1;
		var h = -1;

		foreach (var file in Directory.GetFiles(path)) {
			if (useBig && !file.Contains("_big.png")) continue;
			if (!useBig && file.Contains("_big.png")) continue;

			var tex = new Texture2D(0, 0);
			tex.LoadImage(File.ReadAllBytes(file));

			if (w < 0) {
				w = tex.width;
				h = tex.height;
			} else if (w != tex.width || h != tex.height) {
				throw new Exception("Icons are not all the same size. This differs: " + file);
			}

			var name = Path.GetFileNameWithoutExtension(file);
			if (useBig) name = name.Substring(0, name.Length - 4);
			icons[name] = tex;
		}

		//Also add one for "cursor: none"
		icons["_none_"] = null;

		var res = new Texture2D(w * icons.Count, h, TextureFormat.ARGB32, false);

		var descData = new StringBuilder();
		var namesToPositions = new Dictionary<string, int>();
		var i = 0;
		foreach (var kvp in icons) {
			if (kvp.Value == null) {
				Fill(new Color(0, 0, 0, 0), res, i * w, 0, w, h);
			} else {
				Copy(kvp.Value, res, i * w, 0);
			}
			namesToPositions[kvp.Key] = i++;
		}

		foreach (var kvp in mapping) {
			var pos = -1;
			try {
				if (kvp.Value.name != "_custom_") pos = namesToPositions[kvp.Value.name];
			} catch (KeyNotFoundException) {
				throw new KeyNotFoundException("No file found for " + kvp.Value.name);
			}

			if (descData.Length != 0) descData.Append("\n");

			var hotspot = kvp.Value.hotspot;
			if (!useBig) {
				hotspot.x = Mathf.Round(hotspot.x * .5f) - 3;
				hotspot.y = Mathf.Round(kvp.Value.hotspot.y * .5f) - 4;
			}

			descData
				.Append(kvp.Key).Append(",")
				.Append(pos).Append(",")
				.Append(hotspot.x).Append(",")
				.Append(hotspot.y)
			;

		}

		var resName = Application.dataPath + "/" + destAsset;
		File.WriteAllBytes(
			resName + ".png",
			res.EncodeToPNG()
		);
		File.WriteAllText(
			resName + ".csv",
			descData.ToString()
		);

		AssetDatabase.Refresh();

		Debug.Log("Wrote icons files to " + resName + ".(png|csv) size: " + w + "x" + h);
	}

	private static void Fill(Color color, Texture2D dest, int sx, int sy, int w, int h) {
		for (int x = sx; x < w; ++x) {
			for (int y = sy; y < h; ++y) {
				dest.SetPixel(x, y, color);
			}
		}
	}

	private static void Copy(Texture2D src, Texture2D dest, int destX, int destY) {
		//slow, but fine for a utility
		for (int x = 0; x < src.width; ++x) {
			for (int y = 0; y < src.height; ++y) {
				dest.SetPixel(x + destX, y + destY, src.GetPixel(x, y));
			}
		}
	}

	private struct CursorInfo {
		public CursorInfo(string name, Vector2 hotspot) {
			this.name = name;
			this.hotspot = hotspot;
		}
		public string name;
		public Vector2 hotspot;
	}

	private static Dictionary<CursorType, CursorInfo> mapping = new Dictionary<CursorType, CursorInfo>() {
		//Hotspots in for the default Chromium cursors can be found in ui/base/cursor/cursors_aura.cc, this is adapted
		//from there.
		//Note that we are always using the 2x (_big) icons.
		{
			//{19, 11}, {38, 22}} alias kCursorAlias IDR_AURA_CURSOR_ALIAS CT_ALIAS
			CursorType.Alias,
			new CursorInfo("alias", new Vector2(19, 11))
		}, {
			//{30, 30}, {60, 60}} cell  kCursorCell IDR_AURA_CURSOR_CELL CT_CELL
			CursorType.Cell,
			new CursorInfo("cell", new Vector2(30, 30))
		}, {
			//{35, 29}, {70, 58}} sb_h_double_arrow kCursorColumnResize IDR_AURA_CURSOR_COL_RESIZE CT_COLUMNRESIZE
			CursorType.ColumnResize,
			new CursorInfo("sb_h_double_arrow", new Vector2(35, 29))
		}, {
			//{11, 11}, {22, 22}} context_menu kCursorContextMenu IDR_AURA_CURSOR_CONTEXT_MENU CT_CONTEXTMENU
			CursorType.ContextMenu,
			new CursorInfo("context_menu", new Vector2(11, 11))
		}, {
			//{10, 10}, {20, 20}} copy  kCursorCopy IDR_AURA_CURSOR_COPY CT_COPY
			CursorType.Copy,
			new CursorInfo("copy", new Vector2(10, 10))
		}, {
			//{31, 30}, {62, 60}} crosshair kCursorCross IDR_AURA_CURSOR_CROSSHAIR CT_CROSS
			CursorType.Cross,
			new CursorInfo("crosshair", new Vector2(31, 30))
		}, {
			//{??, ??}, {??, ??}} custom  kCursorCustom IDR_NONE CT_CUSTOM
			CursorType.Custom,
			new CursorInfo("_custom_", new Vector2(-1, -1))
		}, {
			//{??, ??}, {??, ??}} _unknown_ kCursorEastPanning IDR_NONE CT_EASTPANNING
			CursorType.EastPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{35, 29}, {70, 58}} sb_h_double_arrow  kCursorEastResize IDR_AURA_CURSOR_EAST_RESIZE CT_EASTRESIZE
			CursorType.EastResize,
			new CursorInfo("sb_h_double_arrow", new Vector2(35, 29))
		}, {
			//{35, 29}, {70, 58}} sb_h_double_arrow kCursorEastWestResize IDR_AURA_CURSOR_EAST_WEST_RESIZE CT_EASTWESTRESIZE
			CursorType.EastWestResize,
			new CursorInfo("sb_h_double_arrow", new Vector2(35, 29))
		}, {
			//{21, 11}, {42, 22}} fleur kCursorGrab IDR_AURA_CURSOR_GRAB CT_GRAB
			CursorType.Grab,
			new CursorInfo("fleur", new Vector2(21, 11))
		}, {
			//{20, 12}, {40, 24}} hand3 kCursorGrabbing IDR_AURA_CURSOR_GRABBING CT_GRABBING
			CursorType.Grabbing,
			new CursorInfo("hand3", new Vector2(20, 12))
		}, {
			//{25, 7}, {50, 14}} hand2  kCursorHand IDR_AURA_CURSOR_HAND CT_HAND
			CursorType.Hand,
			new CursorInfo("hand2", new Vector2(25, 7))
		}, {
			//{10, 11}, {20, 22}} help  kCursorHelp IDR_AURA_CURSOR_HELP CT_HELP
			CursorType.Help,
			new CursorInfo("help", new Vector2(10, 11))
		}, {
			//{30, 32}, {60, 64}} xterm kCursorIBeam IDR_AURA_CURSOR_IBEAM CT_IBEAM
			CursorType.IBeam,
			new CursorInfo("xterm", new Vector2(30, 32))
		}, {
			//{??, ??}, {??, ??}} _unknown_ kCursorMiddlePanning IDR_NONE CT_MIDDLEPANNING
			CursorType.MiddlePanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{32, 31}, {64, 62}} move  kCursorMove IDR_AURA_CURSOR_MOVE CT_MOVE
			CursorType.Move,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{10, 10}, {20, 20}} nodrop kCursorNoDrop IDR_AURA_CURSOR_NO_DROP CT_NODROP
			CursorType.NoDrop,
			new CursorInfo("nodrop", new Vector2(10, 10))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorNone IDR_NONE CT_NONE
			CursorType.None,
			new CursorInfo("_none_", new Vector2(0, 0))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorNorthEastPanning IDR_NONE CT_NORTHEASTPANNING
			CursorType.NorthEastPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{31, 28}, {62, 56}} top_right_corner  kCursorNorthEastResize IDR_AURA_CURSOR_NORTH_EAST_RESIZE CT_NORTHEASTRESIZE
			CursorType.NorthEastResize,
			new CursorInfo("top_right_corner", new Vector2(31, 28))
		}, {
			//{32, 30}, {64, 60}} top_right_corner  kCursorNorthEastSouthWestResize IDR_AURA_CURSOR_NORTH_EAST_SOUTH_WEST_RESIZE CT_NORTHEASTSOUTHWESTRESIZE
			CursorType.NorthEastSouthWestResize,
			new CursorInfo("top_right_corner", new Vector2(32, 30))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorNorthPanning IDR_NONE CT_NORTHPANNING
			CursorType.NorthPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{29, 32}, {58, 64}} sb_v_double_arrow  kCursorNorthResize IDR_AURA_CURSOR_NORTH_RESIZE CT_NORTHRESIZE
			CursorType.NorthResize,
			new CursorInfo("sb_v_double_arrow", new Vector2(29, 32))
		}, {
			//{29, 32}, {58, 64}} sb_v_double_arrow kCursorNorthSouthResize IDR_AURA_CURSOR_NORTH_SOUTH_RESIZE CT_NORTHSOUTHRESIZE
			CursorType.NorthSouthResize,
			new CursorInfo("sb_v_double_arrow", new Vector2(29, 32))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorNorthWestPanning IDR_NONE CT_NORTHWESTPANNING
			CursorType.NorthWestPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{28, 28}, {56, 56}} top_left_corner kCursorNorthWestResize IDR_AURA_CURSOR_NORTH_WEST_RESIZE CT_NORTHWESTRESIZE
			CursorType.NorthWestResize,
			new CursorInfo("top_left_corner", new Vector2(28, 28))
		}, {
			//{32, 31}, {64, 62}} top_left_corner kCursorNorthWestSouthEastResize IDR_AURA_CURSOR_NORTH_WEST_SOUTH_EAST_RESIZE CT_NORTHWESTSOUTHEASTRESIZE
			CursorType.NorthWestSouthEastResize,
			new CursorInfo("top_left_corner", new Vector2(32, 31))
		}, {
			//{10, 10}, {20, 20}} nodrop kCursorNotAllowed IDR_AURA_CURSOR_NO_DROP CT_NOTALLOWED
			CursorType.NotAllowed,
			new CursorInfo("nodrop", new Vector2(10, 10))
		}, {
			//{10, 10}, {20, 20}} left_ptr  kCursorPointer IDR_AURA_CURSOR_PTR CT_POINTER
			CursorType.Pointer,
			new CursorInfo("left_ptr", new Vector2(10, 10))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorProgress IDR_NONE CT_PROGRESS
			CursorType.Progress,
			new CursorInfo("loading", new Vector2(32, 32))
		}, {
			//{29, 32}, {58, 64}} sb_v_double_arrow  kCursorRowResize IDR_AURA_CURSOR_ROW_RESIZE CT_ROWRESIZE
			CursorType.RowResize,
			new CursorInfo("sb_v_double_arrow", new Vector2(29, 32))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorSouthEastPanning IDR_NONE CT_SOUTHEASTPANNING
			CursorType.SouthEastPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{28, 28}, {56, 56}} top_left_corner kCursorSouthEastResize IDR_AURA_CURSOR_SOUTH_EAST_RESIZE CT_SOUTHEASTRESIZE
			CursorType.SouthEastResize,
			new CursorInfo("top_left_corner", new Vector2(28, 28))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorSouthPanning IDR_NONE CT_SOUTHPANNING
			CursorType.SouthPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{29, 32}, {58, 64}} sb_v_double_arrow  kCursorSouthResize IDR_AURA_CURSOR_SOUTH_RESIZE CT_SOUTHRESIZE
			CursorType.SouthResize,
			new CursorInfo("sb_v_double_arrow", new Vector2(29, 32))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorSouthWestPanning IDR_NONE CT_SOUTHWESTPANNING
			CursorType.SouthWestPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{31, 28}, {62, 56}} top_right_corner  kCursorSouthWestResize IDR_AURA_CURSOR_SOUTH_WEST_RESIZE CT_SOUTHWESTRESIZE
			CursorType.SouthWestResize,
			new CursorInfo("top_right_corner", new Vector2(31, 28))
		}, {
			//{32, 30}, {64, 60}} xterm_horiz  kCursorVerticalText IDR_AURA_CURSOR_XTERM_HORIZ CT_VERTICALTEXT
			CursorType.VerticalText,
			new CursorInfo("xterm_horiz", new Vector2(32, 30))
		}, {
			//{??, ??}, {??, ??}} _unknown_  kCursorWait IDR_NONE CT_WAIT
			CursorType.Wait,
			new CursorInfo("loading", new Vector2(32, 32))
		}, {
			//{??, ??}, {??, ??}} _unknown_ kCursorWestPanning IDR_NONE CT_WESTPANNING
			CursorType.WestPanning,
			new CursorInfo("move", new Vector2(32, 31))
		}, {
			//{35, 29}, {70, 58}} sb_h_double_arrow  kCursorWestResize IDR_AURA_CURSOR_WEST_RESIZE CT_WESTRESIZE
			CursorType.WestResize,
			new CursorInfo("sb_h_double_arrow", new Vector2(35, 29))
		}, {
			//{25, 26}, {50, 52}} zoom_in  kCursorZoomIn IDR_AURA_CURSOR_ZOOM_IN CT_ZOOMIN
			CursorType.ZoomIn,
			new CursorInfo("zoom_in", new Vector2(25, 26))
		}, {
			//{26, 26}, {52, 52}} zoom_out kCursorZoomOut IDR_AURA_CURSOR_ZOOM_OUT CT_ZOOMOUT
			CursorType.ZoomOut,
			new CursorInfo("zoom_out", new Vector2(26, 26))
		},
	};
}
