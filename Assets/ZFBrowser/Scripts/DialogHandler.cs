using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/** 
 * Helper for browser dialog boxes, like alert(). You don't need to use this directly, it will 
 * automatically be added where it's needed. 
 */
[RequireComponent(typeof(Browser))]
public class DialogHandler : MonoBehaviour {
	protected static string dialogPage;

	public delegate void DialogCallback(bool affirm, string text1, string text2);
	public delegate void MenuCallback(int commandId);

	public static DialogHandler Create(Browser parent, DialogCallback dialogCallback, MenuCallback contextCallback) {
		if (dialogPage == null) {
			dialogPage = Resources.Load<TextAsset>("Browser/Dialogs").text;
		}


		var go = new GameObject("Browser Dialog for " + parent.name);
		var handler = go.AddComponent<DialogHandler>();

		handler.parentBrowser = parent;
		handler.dialogCallback = dialogCallback;
		

		var db = handler.dialogBrowser = handler.GetComponent<Browser>();
		
		db.UIHandler = parent.UIHandler;
		db.EnableRendering = false;
		db.EnableInput = false;
		db.allowContextMenuOn = BrowserNative.ContextMenuOrigin.Editable;
		//Use the parent texture. Except, we don't actually use it. So
		//mostly we just mimic the size and don't consume more texture memory.
		db.Resize(parent.Texture);
		db.LoadHTML(dialogPage, "about:dialog");
		db.UIHandler = parent.UIHandler;

		db.RegisterFunction("reportDialogResult", args => {
			dialogCallback(args[0], args[1], args[3]);
			handler.Hide();
		});			
		db.RegisterFunction("reportContextMenuResult", args => {
			contextCallback(args[0]);
			handler.Hide();
		});

		return handler;
	}

	protected Browser parentBrowser;
	protected Browser dialogBrowser;
	protected DialogCallback dialogCallback;
	protected MenuCallback contextCallback;

	public void HandleDialog(BrowserNative.DialogType type, string text, string promptDefault = null) {
		if (type == BrowserNative.DialogType.DLT_HIDE) {
			Hide();
			return;
		}

		Show();

//		Debug.Log("HandleDialog " + type + " text " + text);

		switch (type) {
			case BrowserNative.DialogType.DLT_ALERT:
				dialogBrowser.CallFunction("showAlert", text);
				break;
			case BrowserNative.DialogType.DLT_CONFIRM:
				dialogBrowser.CallFunction("showConfirm", text);
				break;
			case BrowserNative.DialogType.DLT_PROMPT:
				dialogBrowser.CallFunction("showPrompt", text, promptDefault);
				break;
			case BrowserNative.DialogType.DLT_PAGE_UNLOAD:
				dialogBrowser.CallFunction("showConfirmNav", text);
				break;
			case BrowserNative.DialogType.DLT_PAGE_RELOAD:
				dialogBrowser.CallFunction("showConfirmReload", text);
				break;			
			case BrowserNative.DialogType.DLT_GET_AUTH:
				dialogBrowser.CallFunction("showAuthPrompt", text);
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public void Show() {
		parentBrowser.SetOverlay(dialogBrowser);
		parentBrowser.EnableInput = false;
		dialogBrowser.EnableInput = true;
		dialogBrowser.UpdateCursor();
	}

	public void Hide() {
		parentBrowser.SetOverlay(null);
		parentBrowser.EnableInput = true;
		dialogBrowser.EnableInput = false;
		parentBrowser.UpdateCursor();
		if (dialogBrowser.IsLoaded) dialogBrowser.CallFunction("reset");
	}

	public void HandleContextMenu(string menuJSON, int x, int y) {
		if (menuJSON == null) {
			Hide();
			return;
		}

		Show();

		dialogBrowser.CallFunction("showContextMenu", menuJSON, x, y);
	}
}

}
