using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {
/** 
 * Hooks to ensure that zfb gets shut down on standalone app exit. 
 * (Which are different from when we'd want to shut down in the editor.)
 */
class StandaloneShutdown : MonoBehaviour {
	public static void Create() {
		var go = new GameObject("ZFB Shutdown");
		go.AddComponent<StandaloneShutdown>();
		DontDestroyOnLoad(go);
	}

	public void OnApplicationQuit() {
		BrowserNative.UnloadNative();
	}
}
}
