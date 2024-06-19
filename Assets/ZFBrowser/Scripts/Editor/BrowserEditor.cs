using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace ZenFulcrum.EmbeddedBrowser {

[CustomEditor(typeof(Browser))]
[CanEditMultipleObjects]
public class BrowserEditor : Editor {

	private static string script = "document.body.style.background = 'red';\n";
	private static string html = "Hello, <i>world</i>!\n";

	private static string[] commandNames;
	private static BrowserNative.FrameCommand[] commandValues;


	static BrowserEditor() {
		var els = Enum.GetValues(typeof(BrowserNative.FrameCommand));
		commandNames = new string[els.Length];
		commandValues = new BrowserNative.FrameCommand[els.Length];
		int i = 0;
		foreach (BrowserNative.FrameCommand cmd in els) {
			commandNames[i] = cmd.ToString();
			commandValues[i] = cmd;
			++i;
		}

	}

	public override bool RequiresConstantRepaint() {
		//The buttons get stale if we don't keep repainting them.
		return Application.isPlaying;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (Application.isPlaying && !serializedObject.isEditingMultipleObjects) {
			RenderActions();
		} else if (!Application.isPlaying) {
			GUILayout.Label("Additional options available in play mode");
		}
		
	}

	private void RenderActions() {
		var browser = (Browser)target;

		if (!browser.IsReady) {
			GUILayout.Label("Starting...");
			return;
		}

		GUILayout.BeginVertical("box");
		GUILayout.Label("Apply items above:");

		GUILayout.BeginHorizontal("box");
		{
			if (GUILayout.Button("Go to URL")) browser.LoadURL(serializedObject.FindProperty("_url").stringValue, false);
			if (GUILayout.Button("Force to URL")) browser.Url = serializedObject.FindProperty("_url").stringValue;
			if (GUILayout.Button("Resize")) {
				browser.Resize(
					serializedObject.FindProperty("_width").intValue,
					serializedObject.FindProperty("_height").intValue
				);
			}

			if (GUILayout.Button("Set Zoom")) browser.Zoom = serializedObject.FindProperty("_zoom").floatValue;
		}
		GUILayout.EndHorizontal();

		GUILayout.Label("Actions:");

		GUILayout.BeginHorizontal();
		{
			GUI.enabled = browser.CanGoBack;
			if (GUILayout.Button("Go back")) browser.GoBack();
			GUI.enabled = browser.CanGoForward;
			if (GUILayout.Button("Go forward")) browser.GoForward();
			GUI.enabled = true;


			if (browser.IsLoadingRaw) {
				if (GUILayout.Button("Stop")) browser.Stop();
			} else {
				if (GUILayout.Button("Reload")) browser.Reload();
			}
			if (GUILayout.Button("Force Reload")) browser.Reload(true);
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Show Dev Tools")) browser.ShowDevTools();
			if (GUILayout.Button("Hide Dev Tools")) browser.ShowDevTools(false);
		}
		GUILayout.EndHorizontal();


		GUILayout.Label("Script:");
		script = GUILayout.TextArea(script);
		if (GUILayout.Button("Eval JavaScript")) {
			browser.EvalJS(script, "editor command");
		}

		int pVal = EditorGUILayout.Popup("Send Command:", -1, commandNames);
		if (pVal != -1) {
			browser.SendFrameCommand(commandValues[pVal]);
		}

		GUILayout.Label("HTML:");
		html = GUILayout.TextArea(html);
		if (GUILayout.Button("Load HTML")) {
			browser.LoadHTML(html);
		}
			

		GUILayout.EndVertical();
	}


}

}
