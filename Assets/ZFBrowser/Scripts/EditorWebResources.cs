using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * WebResources implementation that grabs resources directly from Assets/../BrowserAssets.
 */
class EditorWebResources : WebResources {
	protected string basePath;

	public EditorWebResources() {
		//NB: If you try to read Application.dataPath later you may not be on the main thread and it won't work.
		basePath = Path.GetDirectoryName(Application.dataPath) + "/BrowserAssets"; 
	}

	public override byte[] GetData(string path) {
		try{
			return File.ReadAllBytes(basePath + path);
		} catch (Exception ex) {
			if (ex is FileNotFoundException || ex is DirectoryNotFoundException) {
				return null;
			} else {
				throw;
			}
		}
	}

}
}
