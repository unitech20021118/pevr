using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Runtime.InteropServices;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Getting CEF running on a build result requires some fiddling to get all the files in the right place.
 */
class PostBuildStandalone {

	private static readonly List<string> rootDirDlls = new List<string>{
		"d3dcompiler_43.dll",
		"d3dcompiler_47.dll",
		"libEGL.dll",
		"libGLESv2.dll",
		"widevinecdmadapter.dll",
		"zf_cef.dll",
	};

	[PostProcessBuild(10)]
	public static void PostprocessWindowsBuild(BuildTarget target, string buildFile) {
		if (target != BuildTarget.StandaloneWindows && target != BuildTarget.StandaloneWindows64) return;

		Debug.Log("Post processing " + buildFile);

		var buildName = Regex.Match(buildFile, @"/([^/]+)\.exe$").Groups[1].Value;
		var buildPath = Directory.GetParent(buildFile);
		var dataPath = buildPath + "/" + buildName + "_Data";

		//can't use FileLocations because we may not be building the same type as the editor
		var platformPluginsSrc = ZFFolder + "/Plugins/w" + (target == BuildTarget.StandaloneWindows64 ? "64" : "32");

		//Copy stuff we need in the root dir there
		foreach (var file in rootDirDlls) {
			ForceMove(
				dataPath + "/Plugins/" + file,
				buildPath + "/" + file
			);
		}
		File.Copy(platformPluginsSrc + "/natives_blob.bin", buildPath + "/natives_blob.bin", true);
		File.Copy(platformPluginsSrc + "/snapshot_blob.bin", buildPath + "/snapshot_blob.bin", true);

		//Copy the needed resources
		var resSrcDir = ZFFolder + "/Plugins/CEFResources";
		foreach (var filePath in Directory.GetFiles(resSrcDir)) {
			var fileName = new FileInfo(filePath).Name;
			if (fileName.EndsWith(".meta")) continue;


			File.Copy(filePath, dataPath + "/Plugins/" + fileName, true);
		}

		//(Unlike locales, icudtl.dat can't be put in a different folder)
		File.Copy(platformPluginsSrc + "/icudtl.dat", buildPath + "/icudtl.dat", true);

		//Slave process (doesn't get automatically copied by Unity like the .dlls)
		File.Copy(
			platformPluginsSrc + "/" + FileLocations.SlaveExecutable + ".exe",
			dataPath + "/Plugins/" + FileLocations.SlaveExecutable + ".exe",
			true
		);

		//Locales
		var localesSrcDir = ZFFolder + "/Plugins/CEFResources/locales";
		var localesDestDir = dataPath + "/Plugins/locales";
		Directory.CreateDirectory(localesDestDir);
		foreach (var filePath in Directory.GetFiles(localesSrcDir)) {
			var fileName = new FileInfo(filePath).Name;
			if (fileName.EndsWith(".meta")) continue;
			File.Copy(filePath, localesDestDir + "/" + fileName, true);
		}

		WriteBrowserAssets(dataPath + "/" + StandaloneWebResources.DefaultPath);
	}


	[PostProcessBuild(10)]
	public static void PostprocessLinuxBuild(BuildTarget target, string buildFile) {
		if (target == BuildTarget.StandaloneLinux || target == BuildTarget.StandaloneLinuxUniversal) {
			throw new Exception("Only x86_64 is supported");
		}
		if (target != BuildTarget.StandaloneLinux64) return;

		Debug.Log("Post processing " + buildFile);

		var buildName = Regex.Match(buildFile, @"\/([^\/]+?)(\.x86(_64)?)?$").Groups[1].Value;
		var buildPath = Directory.GetParent(buildFile);
		var dataPath = buildPath + "/" + buildName + "_Data";

		//can't use FileLocations because we may not be building the same type as the editor
		var platformPluginsSrc = ZFFolder + "/Plugins/l" + (target == BuildTarget.StandaloneLinux64 ? "64" : "32");

		//Copy the needed resources
		var resSrcDir = ZFFolder + "/Plugins/CEFResources";
		foreach (var filePath in Directory.GetFiles(resSrcDir)) {
			var fileName = new FileInfo(filePath).Name;
			if (fileName.EndsWith(".meta")) continue;


			File.Copy(filePath, dataPath + "/Plugins/" + fileName, true);
		}

		var byBinFiles = new List<string>() {
			"natives_blob.bin",
			"snapshot_blob.bin",
			"icudtl.dat",
		};
		foreach (var file in byBinFiles) {
			var linkDest = buildName + "_Data/Plugins/" + file;
			var linkFile = buildPath + "/" + file;
			File.Copy(platformPluginsSrc + "/" + file, dataPath + "/Plugins/" + file, true);
			File.Delete(linkFile);
			var linkRes = symlink(linkDest, linkFile);
			if (linkRes != 0) throw new Exception("Failed to symlink " + linkFile);
		}

		File.Copy(platformPluginsSrc + "/libgcrypt.so.11", dataPath + "/Plugins/libgcrypt.so.11", true);



		//Slave process (doesn't get automatically copied by Unity like the .dlls)
		File.Copy(
			platformPluginsSrc + "/" + FileLocations.SlaveExecutable,
			dataPath + "/Plugins/" + FileLocations.SlaveExecutable,
			true
		);

		//Locales
		var localesSrcDir = ZFFolder + "/Plugins/CEFResources/locales";
		var localesDestDir = dataPath + "/Plugins/locales";
		Directory.CreateDirectory(localesDestDir);
		foreach (var filePath in Directory.GetFiles(localesSrcDir)) {
			var fileName = new FileInfo(filePath).Name;
			if (fileName.EndsWith(".meta")) continue;
			File.Copy(filePath, localesDestDir + "/" + fileName, true);
		}

		WriteBrowserAssets(dataPath + "/" + StandaloneWebResources.DefaultPath);
	}

	[PostProcessBuild(10)]
	public static void PostprocessMacBuild(BuildTarget target, string buildFile) {
		if (target == BuildTarget.StandaloneOSXIntel || target == BuildTarget.StandaloneOSXUniversal) {
			throw new Exception("Only x86_64 is supported");
		}
		if (target != BuildTarget.StandaloneOSXIntel64) return;

		Debug.Log("Post processing " + buildFile);

		//var buildName = Regex.Match(buildFile, @"\/([^\/]+?)\.app$").Groups[1].Value;
		var buildPath = buildFile;
		var platformPluginsSrc = ZFFolder + "/Plugins/m64";

		//Copy app bits
		CopyDirectory(
			platformPluginsSrc + "/BrowserLib.app/Contents/Frameworks/Chromium Embedded Framework.framework",
			buildPath + "/Contents/Frameworks/Chromium Embedded Framework.framework"
		);	
		CopyDirectory(
			platformPluginsSrc + "/BrowserLib.app/Contents/Frameworks/ZFGameBrowser.app",
			buildPath + "/Contents/Frameworks/ZFGameBrowser.app"
		);

		File.Copy(platformPluginsSrc + "/libZFEmbedWeb.dylib", buildPath + "/Contents/Plugins/libZFEmbedWeb.dylib", true);

		//BrowserAssets
		WriteBrowserAssets(buildPath + "/Contents/" + StandaloneWebResources.DefaultPath);
	}


	private static void WriteBrowserAssets(string path) {
		//Debug.Log("Writing browser assets to " + path);

		var htmlDir = Application.dataPath + "/../BrowserAssets";
		var allData = new Dictionary<string, byte[]>();
		if (Directory.Exists(htmlDir)) {
			foreach (var file in Directory.GetFiles(htmlDir, "*", SearchOption.AllDirectories)) {
				var localPath = file.Substring(htmlDir.Length).Replace("\\", "/");
				allData[localPath] = File.ReadAllBytes(file);
			}
		}

		var wr = new StandaloneWebResources(path);
		wr.WriteData(allData);
	}

	private static void ForceMove(string src, string dest) {
		if (File.Exists(dest)) File.Delete(dest);
		File.Move(src, dest);
	}

	private static string ZFFolder {
		get {
			var path = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
			path = Directory.GetParent(path).Parent.Parent.FullName;
			return path;
		}
	}

	private static void CopyDirectory(string src, string dest) {
		foreach (var dir in Directory.GetDirectories(src, "*", SearchOption.AllDirectories)) {
			Directory.CreateDirectory(dir.Replace(src, dest));
		}

		foreach (var file in Directory.GetFiles(src, "*", SearchOption.AllDirectories)) {
			if (file.EndsWith(".meta")) continue;
			File.Copy(file, file.Replace(src, dest), true);
		}
	}

	[DllImport("__Internal")] static extern int symlink(string destStr, string symFile);

}

}
