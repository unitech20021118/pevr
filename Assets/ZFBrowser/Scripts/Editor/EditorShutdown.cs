#if UNITY_EDITOR
using System;
using System.CodeDom;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * The Unity editor doesn't give us a good way to detect when it's closed.
 * ...but it will hang on close if we don't shut down CEF.
 *
 * So we do some voodoo to try to deal with that.
 */
[ExecuteInEditMode]
[InitializeOnLoad]
public class EditorShutdown {
	private static string logLocation;

	static EditorShutdown() {
		logLocation = FileLocations.Dirs.logFile;

		if (!File.Exists(logLocation)) throw new Exception("Failed to find editor log. Editor may hang on exit.");

		AppDomain.CurrentDomain.DomainUnload += (sender, args) => {
			CheckForEditorExit();
		};
	}

	private static void CheckForEditorExit() {
		//Read off the last bit of log to see if we are going to reload or exit.
		//NB: Doing Debug.Log in this function before the read will result in a read of that log instead.
		const string exitString = "Cleanup mono";
		const int readBack = 500;
		var buffer = new byte[readBack];

		try {
			using (var file = File.Open(logLocation, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) {
				file.Seek(-readBack, SeekOrigin.End);

				var len = file.Read(buffer, 0, readBack);

				var readStr = System.Text.Encoding.UTF8.GetString(buffer, 0, len);
//				Debug.Log("len " + len + " readstr " + readStr);

				if (readStr.Contains(exitString)) {
					Debug.Log("Editor shutting down, also stopping ZFBrowser");


					BrowserNative.UnloadNative();
				}
			}
		} catch (Exception ex) {
			Debug.LogError("Failed to check for shutdown: " + ex);
		}
	}
}

}

#endif
