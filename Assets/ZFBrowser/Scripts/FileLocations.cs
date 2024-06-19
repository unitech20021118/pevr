using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZenFulcrum.EmbeddedBrowser {

public static class FileLocations {
	public const string SlaveExecutable = "ZFGameBrowser";

	private static CEFDirs _dirs;
	public static CEFDirs Dirs {
		get { return _dirs ?? (_dirs = GetCEFDirs()); }
	}

	public class CEFDirs {
		/** Where to find cef.pak, et al */
		public string resourcesPath;
		/** Where to find .dll, .so, natives_blob.bin, etc */
		public string binariesPath;
		/** Where to find en-US.pak et al */
		public string localesPath;
		/** The executable to run for browser processes. */
		public string subprocessFile;
		/** Editor/application log file */
		public string logFile;
	}

	private static CEFDirs GetCEFDirs() {
#if UNITY_EDITOR
		//In the editor we don't know exactly where we are at, but we can look up one of our scripts and move from there
		var guids = AssetDatabase.FindAssets("EditorWebResources");
		if (guids.Length != 1) throw new FileNotFoundException("Failed to locate a single EditorWebResources file");
		string scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);

		// ReSharper disable once PossibleNullReferenceException
		var baseDir = Directory.GetParent(scriptPath).Parent.FullName + "/Plugins";
		var resourcesPath = baseDir + "/CEFResources";
		var localesDir = resourcesPath + "/locales";
		var platformDir = baseDir;

		#if UNITY_EDITOR_WIN
			#if UNITY_EDITOR_64
				platformDir += "/w64";
			#else
				platformDir += "/w32";
			#endif

			//Silly MS.
			resourcesPath = resourcesPath.Replace("/", "\\");
			localesDir = localesDir.Replace("/", "\\");
			platformDir = platformDir.Replace("/", "\\");

			var subprocessFile = platformDir + "/" + SlaveExecutable + ".exe";
			var logFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Unity/Editor/Editor.log";

		#elif UNITY_EDITOR_LINUX
			#if UNITY_EDITOR_64
				platformDir += "/l64";
			#else
				platformDir += "/w32";
			#endif

			var subprocessFile = platformDir + "/" + SlaveExecutable;
			var logFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/unity3d/Editor.log";

		#elif UNITY_EDITOR_OSX
			platformDir += "/m64";

			resourcesPath = platformDir + "/BrowserLib.app/Contents/Frameworks/Chromium Embedded Framework.framework/Resources";
			localesDir = resourcesPath;

			//Chromium's base::mac::GetAppBundlePath will walk up the tree until it finds an ".app" folder and start
			//looking for pieces from there. That's why everything is hidden in a fake "BrowserLib.app"
			//folder that's not actually an app.
			var subprocessFile = platformDir + "/BrowserLib.app/Contents/Frameworks/" + SlaveExecutable + ".app/Contents/MacOS/" + SlaveExecutable;

			var logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Library/Logs/Unity/Editor.log";
		#else
			#error Web textures are not supported on this platform
		#endif


		return new CEFDirs() {
			resourcesPath = resourcesPath,
			binariesPath = platformDir,
			localesPath = localesDir,
			subprocessFile = subprocessFile,
			logFile = logFile,
		};

#elif UNITY_STANDALONE_WIN
		var resourcesPath = Application.dataPath + "/Plugins";
		return new CEFDirs() {
			resourcesPath = resourcesPath,
			binariesPath = Application.dataPath + "/../",
			localesPath = resourcesPath + "/locales",
			subprocessFile = resourcesPath + "/" + SlaveExecutable + ".exe",
			logFile = Application.dataPath + "/output_log.txt",
		};
#elif UNITY_STANDALONE_LINUX
		var resourcesPath = Application.dataPath + "/Plugins";
		return new CEFDirs() {
			resourcesPath = resourcesPath,
			binariesPath = resourcesPath,
			localesPath = resourcesPath + "/locales",
			subprocessFile = resourcesPath + "/" + SlaveExecutable,
			logFile = "/dev/null",
				// Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/unity3d/" +
				// Application.companyName + "/" + Application.productName + "/Player.log",
		};
#elif UNITY_STANDALONE_OSX
		return new CEFDirs() {
			resourcesPath = Application.dataPath + "/Frameworks/Chromium Embedded Framework.framework/Resources",
			binariesPath = Application.dataPath + "/Plugins",
			localesPath = Application.dataPath + "/Frameworks/Chromium Embedded Framework.framework/Resources",
			subprocessFile = Application.dataPath + "/Frameworks/ZFGameBrowser.app/Contents/MacOS/" + SlaveExecutable,
			logFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Library/Logs/Unity/Player.log",
		};
#else
		#error Web textures are not supported on this platform
#endif
	}
}

}
