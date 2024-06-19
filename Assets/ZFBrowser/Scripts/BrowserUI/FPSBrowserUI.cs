using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {
/**
 * Works like ClickMeshBrowserUI, but assumes you are pointing at buttons with your nose
 * (a camera or object's transform.forward) instead of with a visible mouse pointer.
 * 
 * This relies on the given FPSCursorRenderer to render the cursor.
 *
 * Unlike MeshColliderBrowserUI, this won't be used by default. If you'd like to use it,
 * attach a UseFPSBrowserUI or call CursorCrosshair.SetUpBrowserInput.
 *
 * As with MeshColliderBrowserUI, pass in the mesh we interact on to {meshCollider}.
 *
 * {worldPointer} is the object we are pointing with. Usually you can use Camera.main.transsform.
 * Its world-space forward direction will be used to get the user's interaction ray.
 */
[RequireComponent(typeof(Browser))]
[RequireComponent(typeof(MeshCollider))]
public class FPSBrowserUI : ClickMeshBrowserUI {
	protected Transform worldPointer;
	protected FPSCursorRenderer cursorRenderer;

	public void Start() {
		FPSCursorRenderer.SetUpBrowserInput(GetComponent<Browser>(), GetComponent<MeshCollider>());
	}
	
	public static FPSBrowserUI Create(MeshCollider meshCollider, Transform worldPointer, FPSCursorRenderer cursorRenderer) {
		var ui = meshCollider.gameObject.GetComponent<FPSBrowserUI>();
		if (!ui) ui = meshCollider.gameObject.AddComponent<FPSBrowserUI>();
		ui.meshCollider = meshCollider;
		ui.worldPointer = worldPointer;
		ui.cursorRenderer = cursorRenderer;

		return ui;
	}

	protected override Ray LookRay {
		get { return new Ray(worldPointer.position, worldPointer.forward); }
	}

	protected override void SetCursor(BrowserCursor newCursor) {
		if (newCursor != null && !MouseHasFocus) return;

		cursorRenderer.SetCursor(newCursor, this);
	}

	public override void InputUpdate() {
		if (!cursorRenderer.EnableInput) {
			MouseHasFocus = false;
			KeyboardHasFocus = false;
			return;
		}

		base.InputUpdate();
	}
}


}
