
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
#define ZF_WINDOWS
#endif

#if UNITY_EDITOR_LINUX || UNITY_EDITOR_OSX //|| UNITY_EDITOR_WIN
#define PROXY_BROWSER_API
#endif

#if PROXY_BROWSER_API || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
#define HAND_LOAD_SYMBOLS
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
#endif

#if HAND_LOAD_SYMBOLS
using System.Reflection;
#endif


// ReSharper disable InconsistentNaming

namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Wrapper/native callbacks for CEF browser implementation.
 * When changing code in this file you may have to restart the Unity Editor for things to get working again.
 *
 * Note that callbacks given to the native side may be invoked on any thread.
 *
 * Make sure IntPtrs are pinned and callbacks are kept alive from GC while their object lives.
 */
public static class BrowserNative {
#if UNITY_EDITOR
	public const int DebugPort = 9848;
#else
	public const int DebugPort = 9849;
#endif

	public static bool NativeLoaded { get; private set; }

	/**
	 * List of command-line switches given to Chromium.
	 * http://peter.sh/experiments/chromium-command-line-switches/
	 *
	 * If you want to change this, be sure to change it before LoadNative gets called.
	 * Also, restart the Editor apply changes.
	 *
	 * Adding or removing flags may lead to instability and/or insecurity.
	 * Make sure you understand what a flag does before you use it.
	 * Be sure to test your use cases thoroughly after changing any flags as
	 * things are more likely to crash or break if you aren't using the default
	 * configuration.
	 */
	public static List<string> commandLineSwitches = new List<string>() {
		"--enable-system-flash",
		//"--zf-browser-log-verbose",//if set, we'll write a lot more Chromium logging to your editor/player log file
	};

	/**
	 * WebResources used to resolve local requests.
	 *
	 * This may be replaced with an implementation of your choice, but be sure to set it up before requesting
	 * any URLs.
	 */
	public static WebResources webResources;
	public static string LocalUrlPrefix { get { return "https://game.local/"; } }

	private static void LogCallback(string message) {
		Debug.Log("ZFWeb: " + message);
	}


	public static void LoadNative() {
		if (NativeLoaded) return;

		if (webResources == null) {
			//if the user hasn't given us a WebResources to use, use the default
#if UNITY_EDITOR
			webResources = new EditorWebResources();
#else
			var swr = new StandaloneWebResources(Application.dataPath + "/" + StandaloneWebResources.DefaultPath);
			swr.LoadIndex();
			webResources = swr;
#endif
		}


		//For Editor/debug builds, we'll open a port you can just http:// to inspect pages.
		//Don't do this for real builds, though. It makes it really, really easy for the end user to call
		//random JS in the page, potentially affecting or bypassing game logic.
		var debugPort = Debug.isDebugBuild ? DebugPort : 0;


		var dirs = FileLocations.Dirs;

#if UNITY_EDITOR_OSX
		FixProcessPermissions(dirs);
#endif

//		Debug.Log("Browser dirs " + dirs.cefPath + " " + dirs.localesPath);

//		//We need a certain CWD on some platforms so we can load the DLL (now) and Loading our DLL implies loading a ton of other things, plus init() needs to
//		//load various other files
//		//So, change the cwd while setting up (this is more an issue in builds than the editor)
//		var loadDir = (string)null;
//		if (!Application.isEditor) {
//			//We need the CWD to be the folder of the .exe in standalone builds
//			loadDir = Directory.GetParent(Application.dataPath).FullName;
//		}

#if UNITY_STANDALONE_WIN
		//make sure the child processes can be started (their dependent .dlls are next to the main .exe, not in the Plugins folder)
		var loadDir = Directory.GetParent(Application.dataPath).FullName;
		var path = Environment.GetEnvironmentVariable("PATH");
		path += ";" + loadDir;
		Environment.SetEnvironmentVariable("PATH", path);
#endif

#if UNITY_EDITOR_LINUX
	Environment.SetEnvironmentVariable("ZF_CEF_FORCE_EXE_DIR", Directory.GetParent(dirs.subprocessFile).FullName);
#endif

#if HAND_LOAD_SYMBOLS
		HandLoadSymbols(dirs.binariesPath);
#endif

#if !UNITY_EDITOR
		StandaloneShutdown.Create();
#endif

//		foreach (DictionaryEntry env in Environment.GetEnvironmentVariables()) {
//			Debug.Log("Env var " + env.Key + "=" + env.Value);
//		}
//
// #if true || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
// 		//Add library load paths so we can load our bits
// 		var libDir = dirs.binariesPath;
//
////		var path = Environment.GetEnvironmentVariable("DYLD_FRAMEWORK_PATH");
//// 		if (!string.IsNullOrEmpty(path)) path += ":" + libDir;
//// 		else path = libDir;
////		Environment.SetEnvironmentVariable("DYLD_FRAMEWORK_PATH", path);
//
//	var	path = Environment.GetEnvironmentVariable("DYLD_LIBRARY_PATH");
//Debug.Log("Start DYLD_LIBRARY_PATH as " + path);
//// 		if (!string.IsNullOrEmpty(path)) path += ":" + libDir;
//// 		else path = libDir;
////		Environment.SetEnvironmentVariable("DYLD_LIBRARY_PATH", path);
////
////		Debug.Log("Set DYLD_LIBRARY_PATH to " + path);
//
// #endif

//		var oldCWD = Directory.GetCurrentDirectory();
		try {
//			if (loadDir != null) Directory.SetCurrentDirectory(loadDir);
			// Debug.Log("Current dir " + Directory.GetCurrentDirectory());

			//There never should be any, but just in case, destroy any existing browsers on a re-init
			zfb_destroyAllBrowsers();

			//Caution: Careful with these functions you pass to native. The Unity Editor will
			//reload assemblies, leaving the function pointers dangling. If any native calls try to use them
			//before we load back up and re-register them... *boom*.
			//To prevent this, we call zfb_setCallbacksEnabled to disable callbacks before we get unloaded.
			zfb_setDebugFunc(LogCallback);
			zfb_localRequestFuncs(HeaderCallback, DataCallback);
			zfb_setCallbacksEnabled(true);

			var settings = new ZFBInitialSettings() {
				cefPath = dirs.resourcesPath,
				localePath = dirs.localesPath,
				subprocessFile = dirs.subprocessFile,
				userAgent = UserAgent.GetUserAgent(),
				logFile = dirs.logFile,
				debugPort = debugPort,
				/*
				 * This is complicated.
				 * Depending on how this is set, you get to deal with a whole slew of bugs.
				 *
				 * Enabled:
				 *   DCHECK on exit if extensions enabled
				 *   Multithreaded support for various API funcs
				 *
				 * Disabled:
				 *   Have to tick the backend, which tends to munch more CPU than we'd like
				 *   Backend tick eats input events
				 *   Backend tick eats crashes Unity on exit betimes
				 */
				#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				//CEF doesn't have the multithreaded loop implemented on 'cept Windows
				multiThreadedMessageLoop = 1,
				#else
				multiThreadedMessageLoop = 0,
				#endif
			};

			foreach (var arg in commandLineSwitches) zfb_addCLISwitch(arg);

//			Debug.Log("Start CEF on thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);

			var initRes = zfb_init(settings);
			if (!initRes) throw new Exception("Failed to initialize browser system.");


			NativeLoaded = true;

			//System.Diagnostics.Process.Start(dirs.subprocessFile);
		} finally {
//			Directory.SetCurrentDirectory(oldCWD);
		}


		AppDomain.CurrentDomain.DomainUnload += (sender, args) => {
			zfb_destroyAllBrowsers();
			zfb_setCallbacksEnabled(false);
		};
	}

	private static void FixProcessPermissions(FileLocations.CEFDirs dirs) {
		/*
		 * The package we get from the Asset Store probably won't have the right executable permissions for
		 * ZFGameBrowser for OS X, so let's fix that for the user right now.
		 */

		var attrs = (uint)File.GetAttributes(dirs.subprocessFile);

		//From https://github.com/mono/mono/blob/master/mono/io-layer/io.c under SetFileAttributes() (also noted in FileAttributes.cs):
		//"Currently we only handle one *internal* case [...]: 0x80000000, which means `set executable bit'"
		//Let's use that now.
		attrs |= 0x80000000;

		//Make it executable.
		File.SetAttributes(dirs.subprocessFile, unchecked((FileAttributes)attrs));
	}

#if HAND_LOAD_SYMBOLS

	private static IntPtr moduleHandle;
	private static void HandLoadSymbols(string binariesPath) {
		/*
		Unity uses CEF. We use CEF. The versions are not the same and even if they were, we would
		be setting up the library differently. Therefore, we need to load two different copies of
		CEF.

		If we load ZFEmbedWeb with the normal DllImport method, the CEF calls in ZFEmbedWeb end
		up going to Unity's copy of CEF, which, naturally, dies a terrible death.

		In short, we need to load libZFEmbedWeb.so with the RTLD_DEEPBIND flag so it uses only
		its copy of CEF. There's not a way to do this with DllImport and, looking at the Mono
		source code, there's not really a practical way to have Mono do it for us.

		Therefore, we'll load the .so and peck out all the functions by hand so we can call them
		in the editor.
		*/


#if UNITY_EDITOR_OSX
		var libFile = binariesPath + "/libZFEmbedWebEditor.dylib";
#elif UNITY_STANDALONE_OSX
		var libFile = binariesPath + "/libZFEmbedWeb.dylib";
#elif UNITY_EDITOR_LINUX
		/**
		If we make a custom build of CEF+Chromium with tcmalloc disabled and the
		current executable overridden this almost works, but we die from allocator mixing in
		certain places, most notably when using storage backed by sqlite3.
		Also, it runs dirt slow. Not sure why.
		var libFile = binariesPath + "/libZFEmbedWeb.so";
		*/

		/** Start a child process and communicate with it. */
		var libFile = binariesPath + "/libZFEmbedWebEditor.so";
#elif UNITY_STANDALONE_LINUX
		var libFile = binariesPath + "/libZFEmbedWeb.so";
#elif UNITY_EDITOR_WIN
		var libFile = binariesPath + "/ZFEmbedWebEditor.dll";
#else
	#error Unknown OS.
#endif

		moduleHandle = OpenLib(libFile);

		//Now go through and fill our functions with life.
		int i = 0;
		var fields = typeof(BrowserNative).GetFields(BindingFlags.Static | BindingFlags.Public);
		foreach (var field in fields) {
			if (!field.Name.StartsWith("zfb_")) continue;

			var fp = GetFunc(moduleHandle, field.Name);

			var func = Marshal.GetDelegateForFunctionPointer(fp, field.FieldType);
			field.SetValue(null, func);
			++i;
		}

		//Debug.Log("Loaded " + i + " symbols");
	}

	private static IntPtr OpenLib(string name) {
#if ZF_WINDOWS
		var handle = LoadLibrary(name);
		if (handle == IntPtr.Zero) {
//			throw new DllNotFoundException("ZFBrowser failed to load " + name + ": " + Marshal.GetLastWin32Error());
			throw new DllNotFoundException("ZFBrowser failed to load " + name + ": " +
				new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()).ShopClientMessage
			);
		}
		return handle;
#else
		//Call this now because running a DllImport method the first time will end up calling dlerror
		//which will clear the error we were trying to get from dlerror.
		dlerror();


		/*
		Linux:
		In the editor, we need to use RTLD_DEEPBIND so libZFEmbedWeb.so gets the symbols from zf_cef.so
		instead of the ones that are already loaded in the Unity editor.
		However, for standalone, we need to NOT use RTLD_DEEPBIND because the standlaone appers to override
		{operator new} and we get nasty memory crashes if we have two allocaros in use. (Also, we don't need
		deep binding because there's no other copy of CEF flaoting around.)
		*/

		#if UNITY_EDITOR
		var handle = dlopen(name, (int)(DLFlags.RTLD_LAZY | DLFlags.RTLD_DEEPBIND));
		#else
		var handle = dlopen(name, (int)(DLFlags.RTLD_LAZY));
		#endif
		if (handle == IntPtr.Zero) {
			throw new DllNotFoundException("ZFBrowser failed to load " + name + ": " + getDlError());
		}
		return handle;
#endif
	}

	private static IntPtr GetFunc(IntPtr libHandle, string fnName) {
#if ZF_WINDOWS
		var addr = GetProcAddress(libHandle, fnName);
		if (addr == IntPtr.Zero) {
			throw new DllNotFoundException("ZFBrowser failed to load method " + fnName + ": " + Marshal.GetLastWin32Error());
		}
		return addr;
#else
		var fp = dlsym(libHandle, fnName);
		if (fp == IntPtr.Zero) {
			throw new DllNotFoundException("ZFBrowser failed to load method " + fnName + ": " + getDlError());
		}
		return fp;
#endif
	}

	[Flags]
	public enum DLFlags {
		RTLD_LAZY = 1,
		RTLD_NOW = 2,
		RTLD_DEEPBIND = 8,
	}
	[DllImport("__Internal")] static extern IntPtr dlopen(string filename, int flags);
	[DllImport("__Internal")] static extern IntPtr dlsym(IntPtr handle, string symbol);
//	[DllImport("__Internal")] static extern int dlclose(IntPtr handle);
	[DllImport("__Internal")] static extern IntPtr dlerror();
	private static string getDlError() {
		var err = dlerror();
		return Marshal.PtrToStringAnsi(err);
	}

	[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
	static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
	[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
#endif

	private static Dictionary<int, WebResources.Response> requests = new Dictionary<int, WebResources.Response>();
	private static int nextRequestId = 1;

	private static int HeaderCallback(string url, IntPtr mimeTypeDest, out int size, out int responseCode) {
		//may be called on any thread

		//Chop up the URL so it's easy to digest.
		if (url.SafeStartsWith(LocalUrlPrefix)) {
			url = "/" + url.Substring(LocalUrlPrefix.Length);
		}

		WebResources.Response response;
		if (webResources == null) {
			response = new WebResources.Response() {
				data = Encoding.UTF8.GetBytes("No WebResources handler!"),
				mimeType = "text/plain",
				responseCode = 500,
			};
		} else {
			response = webResources[url];
		}

		var data = Encoding.UTF8.GetBytes(response.mimeType);
		if (data.Length > 99) {
			Debug.LogWarning("mime type is too long " + response.mimeType);
			data = new byte[0];
		}

		Marshal.Copy(data, 0, mimeTypeDest, data.Length);
		Marshal.WriteByte(mimeTypeDest, data.Length, 0);//null-terminate

		responseCode = response.responseCode;
		size = response.data.Length;

		int id;
		lock (requests) {
			id = nextRequestId++;
			requests[id] = response;
		}

		return id;
	}

	private static void DataCallback(int reqId, IntPtr data, int size) {
		//may be called on any thread

		WebResources.Response response;
		lock (requests) {
			if (!requests.TryGetValue(reqId, out response)) {
				response = new WebResources.Response() {
					data = Encoding.UTF8.GetBytes("No response for request!"),
					mimeType = "text/plain",
					responseCode = 500,
				};
			}
			requests.Remove(reqId);
		}

		Assert.AreEqual(response.data.Length, size);

		if (size != 0) Marshal.Copy(response.data, 0, data, size);
	}

	/** Shuts down the native browser library and CEF. Once shut down, the system cannot be restarted. */
	public static void UnloadNative() {
		if (!NativeLoaded) return;

		Debug.Log("Stop CEF");

		zfb_setCallbacksEnabled(false);
		zfb_shutdown();
		NativeLoaded = false;
	}


	/** Call this with a message to debug it to a console somewhere. */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MessageFunc(string message);

	/**
	 * Callback for getting headers on a local request.
	 * url is the url requested
	 * mimeType will have 100 bytes for you to write a null-terminated string to
	 * set size to the size of the result, in bytes
	 * set responseCode to the http status code.
	 * Return whatever you'd like, the value will be included when GetRequestDataFunc is called.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int GetRequestHeadersFunc(string url, IntPtr mimeType, out int size, out int responseCode);

	/**
	 * Fetches data for a request.
	 * requestId is what you returned from GetRequestHeadersFunc
	 * Fill data with the data for the request which is {size} size.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GetRequestDataFunc(int reqId, IntPtr data, int size);




	/** Called when the native backend is ready to start receiving orders. */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyFunc(int browserId);

	/** Called on console.log, console.err, etc. */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ConsoleFunc(int browserId, string message, string source, int line);

	/**
	 * Called when JS calls back to us.
	 * callbackId is the first argument,
	 * data (UTF-8 null-terminated string) (and it's included size) are the second argument.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ForwardJSCallFunc(int browserId, int callbackId, string data, int size);

	/**
	 * Called when a browser attempts to open a new window.
	 * newURL - The URL we're trying to open.
	 * userInvoked - True if the window is opening form a user's action (false if its the type of pop-up that
	 *   ought to be blocked)
	 * possibleBrowserId - If you return NWA_NEW_BROWSER, this will be the browser id of the newly created window.
	 *   Otherwise, ignore.
	 * possibleSettings - If you return NWA_NEW_BROWSER, these settings will be used for the new browser, so alter them as desired.
	 *
	 * result - Return the NewWindowAction for what you would like to have happen.
	 *
	 * May be called on any thread.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate NewWindowAction NewWindowFunc(
		int browserId, IntPtr /* string */ newURL, bool userInvoked,
		int possibleBrowserId, ref ZFBSettings possibleSettings
	);

	/**
	 * Called when an item from ChangeType happens.
	 * See the documentation for the given ChangeType for info on what the args mean or how to get more information.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ChangeFunc(int browserId, ChangeType changeType, string arg1);

	/**
	 * This is called when the browser wants to display a dialog of some sort.
	 * dialogType - the type, or DLT_HIDE to hide any existing dialogs.
	 * dialogText - main text for the dialog, usually from in-page JavaScript
	 * initialPromptText - if we are doing a JavaScript prompt(), the default text to display
	 * sourceURL - the URL of the page that is causing the dialog
	 *
	 * Once the user has responded to the dialog (if we were showing one), call zfb_sendDialogResults
	 * with the user's input.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DisplayDialogFunc(
		int browserId, DialogType dialogType, string dialogText,
		string initialPromptText, string sourceURL
	);

	/**
	 * Called by the backend when a context menu should be shown or hidden.
	 * If menuJSON is null, hide the context menu.
	 * If it's not, show the given menu and eventually call zfb_sendContextMenuResults.
	 * For more information on the menu format, look at BrowserDialogs.html
	 *
	 * x and y report the position the menu was summoned, relative to the top-left of the view.
	 * origin indicates on what type of item the context menu was created.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ShowContextMenuFunc(int browserId, string menuJSON, int x, int y, ContextMenuOrigin origin);

	/**
	 * Used with zfb_getCookies, this will be called once for each cookie.
	 */
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GetCookieFunc(NativeCookie cookie);



	public enum LoadChange : int {
		LC_STOP = 1,
		LC_RELOAD,
		LC_FORCE_RELOAD,
	}

	public enum MouseButton : int {
		MBT_LEFT = 0,
		MBT_MIDDLE,
		MBT_RIGHT,
	}

	public enum ChangeType : int {
		/** The cursor has changed. Use zfb_getMouseCursor/zfb_getMouseCustomCursor to see what it is now. */
		CHT_CURSOR = 0,
		/** The browser has been closed and can no longer receive commands. */
		CHT_BROWSER_CLOSE,
		/**
		 * We have the HTML for the top-level page.
		 * arg1 (JSON) contains HTTP {status} code and the {url}
		 * Note that successfully fetching errors from a server (404, 500) are treated
		 * as successful, CHT_FETCH_FAILED won't be triggered.
		 */
		CHT_FETCH_FINISHED,
		/**
		 * Failed to fetch a page (timeout, network issues, etc)
		 * arg1 (JSON) contains an {error} code and the {url}
		 */
		CHT_FETCH_FAILED,
		/**
		 * The page has reached onload
		 * arg1 (JSON) contains HTTP {status} code and the {url}
		 */
		CHT_LOAD_FINISHED,
		/** SSL certificate error. arg1 has some JSON about the issue. Often followed by a CHT_FETCH_FAILED */
		CHT_CERT_ERROR,
		/** Renderer process crashed/was killed/etc. */
		CHT_SAD_TAB,
	}

	/** @see cef_cursor_type_t in cef_types.h */
	public enum CursorType : int {
		Pointer = 0,
		Cross,
		Hand,
		IBeam,
		Wait,
		Help,
		EastResize,
		NorthResize,
		NorthEastResize,
		NorthWestResize,
		SouthResize,
		SouthEastResize,
		SouthWestResize,
		WestResize,
		NorthSouthResize,
		EastWestResize,
		NorthEastSouthWestResize,
		NorthWestSouthEastResize,
		ColumnResize,
		RowResize,
		MiddlePanning,
		EastPanning,
		NorthPanning,
		NorthEastPanning,
		NorthWestPanning,
		SouthPanning,
		SouthEastPanning,
		SouthWestPanning,
		WestPanning,
		Move,
		VerticalText,
		Cell,
		ContextMenu,
		Alias,
		Progress,
		NoDrop,
		Copy,
		None,
		NotAllowed,
		ZoomIn,
		ZoomOut,
		Grab,
		Grabbing,
		Custom,
	}

	public enum DialogType {
		DLT_HIDE = 0,
		DLT_ALERT,
		DLT_CONFIRM,
		DLT_PROMPT,
		DLT_PAGE_UNLOAD,
		DLT_PAGE_RELOAD,//like unload, but the user is just refreshing the page
		DLT_GET_AUTH,
	};

	public enum NewWindowAction {
		NWA_IGNORE = 1,
		NWA_REDIRECT,
		NWA_NEW_BROWSER,
		NWA_NEW_WINDOW,
	};

	[Flags]
	public enum ContextMenuOrigin {
		Editable = 1 << 1,
		Image = 1 << 2,
		Selection = 1 << 3,
		Other = 1 << 0,
	}

	public enum FrameCommand {
		Undo,
		Redo,
		Cut,
		Copy,
		Paste,
		Delete,
		SelectAll,
		ViewSource,
	};

	public enum CookieAction {
		Delete,
		Create,
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ZFBInitialSettings {
		public string cefPath, localePath, subprocessFile, userAgent, logFile;
		public int debugPort, multiThreadedMessageLoop;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ZFBSettings {
		public int bgR, bgG, bgB, bgA;
		public int offscreen;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct RenderData {
		public IntPtr pixels;
		public int w, h;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class NativeCookie {
		public string name, value, domain, path;
		public string creation, lastAccess, expires;
		public byte secure, httpOnly;
	}

#if !HAND_LOAD_SYMBOLS
	/** Does nothing. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_noop();

	/**
	 * Some functions (getURL) allocates memory to give you a response. Call this to free it.
	 * (Using this instead of the Mono-standard shared alloc functions lets us shed a dependency.)
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_free(IntPtr memory);

	/** Plain old memcpy. Because sometimes Marshal.Copy falls short of our needs. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_memcpy(IntPtr dst, IntPtr src, int size);

	/** Returns the Chrome(ium) version as a static C string. May be called before initialization. */
	[DllImport("ZFEmbedWeb")]
	public static extern IntPtr zfb_getVersion();

	/** Sets a function to call for Debug.Log-style messages. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_setDebugFunc(MessageFunc debugFunc);

	/** Sets callbacks for local (https://game.local/) requests. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_localRequestFuncs(GetRequestHeadersFunc headerFunc, GetRequestDataFunc dataFunc);

	/** Enabled/disables user callbacks. Useful for disabling all callbacks when mono assemblies reload. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_setCallbacksEnabled(bool enabled);

	/** Destroys all browser instances. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_destroyAllBrowsers();

	/** Adds a command-line switch to Chromium, must call before zfb_init */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_addCLISwitch(string value);

	/** Initializes the system so we can start making browsers. */
	[DllImport("ZFEmbedWeb")]
	public static extern bool zfb_init(ZFBInitialSettings settings);

	/** Shuts down the system. It cannot be re-initialized. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_shutdown();

	/**
	 * Creates a new browser, returning the id.
	 * Call zfb_setReadyCallback and wait for it to fire before doing anything else.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern int zfb_createBrowser(ZFBSettings settings);

	/** Reports the number of un-destroyed browsers. */
	[DllImport("ZFEmbedWeb")]
	public static extern int zfb_numBrowsers();

	/** Closes and cleans up a browser instance. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_destoryBrowser(int id);

	/** Call once per frame if the multi-threaded message loop isn't enabled. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_tick();

	/**
	 * Registers a function to call when the browser instance is ready to start taking orders.
	 * {cb} may be executed immediately or on any thread.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_setReadyCallback(int id, ReadyFunc cb);

	/** Resizes the browser. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_resize(int id, int w, int h);

	/**
	 * Adds the given browser {overlayBrowserId} as an overlay of this browser {browserId}.
	 * The overlaid browser will appear transparently over the top of {browser}.
	 * {overlayBrowser} must not have an overlay and must be sized exactly the same as {browser}.
	 * Remove the overlay before closing either browser.
	 *
	 * While {overlayBrowser} is overlaying another browser, do not call zfb_getImage on it.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_setOverlay(int browserId, int overlayBrowserId);

	/**
	 * Gets the image data for the current frame.
	 * Do not hang onto the returned data across frames or resizes.
	 *
	 * If there are no changes since last call, the pixel data will be null (unless you specify forceDirty).
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern RenderData zfb_getImage(int id, bool forceDirty);

	/**
	 * Navigates to the given URL. If force it ture, it will go there right away.
	 * If force is false, the pages that wish to can prompt the user and possibly cancel the
	 * navigation.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_goToURL(int id, string url, bool force);

	/**
	 * Loads the given HTML string as if it were the given URL.
	 * Use http://-like porotocols or else things may not work right.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_goToHTML(int id, string html, string url);

	/**
	 * Gets the current url, returned as a UTF-8 null-terminated string.
	 * Call zfb_free on the result when you are done with it.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern IntPtr zfb_getURL(int id);

	/** Can we go back (-1) or forward (1)? */
	[DllImport("ZFEmbedWeb")]
	public static extern bool zfb_canNav(int id, int direction);

	/** Go back (-1) or forward (1) */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_doNav(int id, int direction);

	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_setZoom(int id, double zoom);

	[DllImport("ZFEmbedWeb")]
	public static extern bool zfb_isLoading(int id);

	/** Stop, refresh, or force-refresh */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_changeLoading(int id, LoadChange what);

	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_showDevTools(int id, bool show);

	/**
	 * Informs the browser if it's focused for keyboard input.
	 * Among other things, this controls if the blinking text cursor appears in an active text field.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_setFocused(int id, bool focused);

	/**
	 * Reports the mouse's current location.
	 * x and y are in the range [0,1]. (0, 0) is top-left, (1, 1) is bottom-right
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_mouseMove(int id, float x, float y);

	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_mouseButton(int id, MouseButton button, bool down, int clickCount);

	/** Reports a mouse scroll. One "tick" of a scroll wheel is generally around 120 units. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_mouseScroll(int id, int deltaX, int deltaY);

	/**
	 * Report a key down/up event. Repeated "virtual" keystrokes are simulated by repeating the down event without
	 * an interveneing up event.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_keyEvent(int id, bool down, int windowsKeyCode);

	/**
	 * Report a typed character. This typically interleaves with calls to zfb_keyEvent
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_characterEvent(int id, int character, int windowsKeyCode);

	/** Register a function to call when console.log etc. is called in the browser. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_registerConsoleCallback(int id, ConsoleFunc callback);

	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_evalJS(int id, string script, string scriptURL);

	/** Registers a callback to call when window._zfb_event(int, string) is called in the browser. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_registerJSCallback(int id, ForwardJSCallFunc cb);

	/** Registers a callback that is called when something from ChangeType happens. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_registerChangeCallback(int id, ChangeFunc cb);

	/**
	 * Gets the current mouse cursor. If the type is CursorType.Custom, width and height will be filled with
	 * the width and height of the custom cursor.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern CursorType zfb_getMouseCursor(int id, out int width, out int height);

	/**
	 * Call this if zfb_getMouseCursor tells you there's a custom cursor.
	 * This will fill buffer (RGBA bottom-top, 4 bytes * width * height) with the contents of the cursor.
	 * Use the size you got from zfb_getMouseCursor.
	 * If width or height don't match the results from zfb_getMouseCursor, does nothing.
	 *
	 * {hotX} and {hoyY} will be filled with the cursor's hotspot.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_getMouseCustomCursor(int id, IntPtr buffer, int width, int height, out int hotX, out int hotY);

	/** Registers a DisplayDialogFunc for this browser. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_registerDialogCallback(int id, DisplayDialogFunc cb);

	/** Callback for a dialog. See the docs on DisplayDialogFunc. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_sendDialogResults(int id, bool affirmed, string text1, string text2);

	/** Registers a NewWindowFunc for pop ups. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_registerPopupCallback(int id, NewWindowFunc cb);

	/** Registers a ShowContextMenuFunc for the context menu. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_registerContextMenuCallback(int id, ShowContextMenuFunc cb);

	/**
	 * After your ShowContextMenuFunc has been called,
	 * call this to report what item the user selected.
	 * If the menu was canceled, send -1.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_sendContextMenuResults(int id, int commandId);

	/**
	 * Sends a command, such as copy, paste, or select to the focused frame in the given browser.
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_sendCommandToFocusedFrame(int id, FrameCommand command);

	/** Fetches all the cookies, calling the given callback for every cookie. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_getCookies(int id, GetCookieFunc cb);

	/** Alters the given cookie as specified. */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_editCookie(int id, NativeCookie cookie, CookieAction action);

	/**
	 * Deletes all the cookies.
	 * (Though it takes a browser, this will typically clear all cookies for all browsers.)
	 */
	[DllImport("ZFEmbedWeb")]
	public static extern void zfb_clearCookies(int id);

#else //HAND_LOAD_SYMBOLS is true:
	//See HandLoadSymbols() for an explanation of what's going on here
	public delegate void Calltype_zfb_noop();
	public static Calltype_zfb_noop zfb_noop;

	public delegate void Calltype_zfb_free(IntPtr mem);
	public static Calltype_zfb_free zfb_free;

	public delegate void Calltype_zfb_memcpy(IntPtr dst, IntPtr src, int size);
	public static Calltype_zfb_memcpy zfb_memcpy;

	public delegate IntPtr Calltype_zfb_getVersion();
	public static Calltype_zfb_getVersion zfb_getVersion;

	public delegate void Calltype_zfb_setDebugFunc(MessageFunc debugFunc);
	public static Calltype_zfb_setDebugFunc zfb_setDebugFunc;

	public delegate void Calltype_zfb_localRequestFuncs(GetRequestHeadersFunc headerFunc, GetRequestDataFunc dataFunc);
	public static Calltype_zfb_localRequestFuncs zfb_localRequestFuncs;

	public delegate void Calltype_zfb_setCallbacksEnabled(bool enabled);
	public static Calltype_zfb_setCallbacksEnabled zfb_setCallbacksEnabled;

	public delegate void Calltype_zfb_destroyAllBrowsers();
	public static Calltype_zfb_destroyAllBrowsers zfb_destroyAllBrowsers;

	public delegate void Calltype_zfb_addCLISwitch(string value);
	public static Calltype_zfb_addCLISwitch zfb_addCLISwitch;

	public delegate bool Calltype_zfb_init(ZFBInitialSettings settings);
	public static Calltype_zfb_init zfb_init;

	public delegate void Calltype_zfb_shutdown();
	public static Calltype_zfb_shutdown zfb_shutdown;

	public delegate int Calltype_zfb_createBrowser(ZFBSettings settings);
	public static Calltype_zfb_createBrowser zfb_createBrowser;

	public delegate int Calltype_zfb_numBrowsers();
	public static Calltype_zfb_numBrowsers zfb_numBrowsers;

	public delegate void Calltype_zfb_destoryBrowser(int id);
	public static Calltype_zfb_destoryBrowser zfb_destoryBrowser;

	public delegate void Calltype_zfb_tick();
	public static Calltype_zfb_tick zfb_tick;

	public delegate void Calltype_zfb_setReadyCallback(int id, ReadyFunc cb);
	public static Calltype_zfb_setReadyCallback zfb_setReadyCallback;

	public delegate void Calltype_zfb_resize(int id, int w, int h);
	public static Calltype_zfb_resize zfb_resize;

	public delegate void Calltype_zfb_setOverlay(int browserId, int overlayBrowserId);
	public static Calltype_zfb_setOverlay zfb_setOverlay;

	public delegate RenderData Calltype_zfb_getImage(int id, bool forceDirty);
	public static Calltype_zfb_getImage zfb_getImage;

	public delegate void Calltype_zfb_goToURL(int id, string url, bool force);
	public static Calltype_zfb_goToURL zfb_goToURL;

	public delegate void Calltype_zfb_goToHTML(int id, string html, string url);
	public static Calltype_zfb_goToHTML zfb_goToHTML;

	public delegate IntPtr Calltype_zfb_getURL(int id);
	public static Calltype_zfb_getURL zfb_getURL;

	public delegate bool Calltype_zfb_canNav(int id, int direction);
	public static Calltype_zfb_canNav zfb_canNav;

	public delegate void Calltype_zfb_doNav(int id, int direction);
	public static Calltype_zfb_doNav zfb_doNav;

	public delegate void Calltype_zfb_setZoom(int id, double zoom);
	public static Calltype_zfb_setZoom zfb_setZoom;

	public delegate bool Calltype_zfb_isLoading(int id);
	public static Calltype_zfb_isLoading zfb_isLoading;

	public delegate void Calltype_zfb_changeLoading(int id, LoadChange what);
	public static Calltype_zfb_changeLoading zfb_changeLoading;

	public delegate void Calltype_zfb_showDevTools(int id, bool show);
	public static Calltype_zfb_showDevTools zfb_showDevTools;

	public delegate void Calltype_zfb_setFocused(int id, bool focused);
	public static Calltype_zfb_setFocused zfb_setFocused;

	public delegate void Calltype_zfb_mouseMove(int id, float x, float y);
	public static Calltype_zfb_mouseMove zfb_mouseMove;

	public delegate void Calltype_zfb_mouseButton(int id, MouseButton button, bool down, int clickCount);
	public static Calltype_zfb_mouseButton zfb_mouseButton;

	public delegate void Calltype_zfb_mouseScroll(int id, int deltaX, int deltaY);
	public static Calltype_zfb_mouseScroll zfb_mouseScroll;

	public delegate void Calltype_zfb_keyEvent(int id, bool down, int windowsKeyCode);
	public static Calltype_zfb_keyEvent zfb_keyEvent;

	public delegate void Calltype_zfb_characterEvent(int id, int character, int windowsKeyCode);
	public static Calltype_zfb_characterEvent zfb_characterEvent;

	public delegate void Calltype_zfb_registerConsoleCallback(int id, ConsoleFunc callback);
	public static Calltype_zfb_registerConsoleCallback zfb_registerConsoleCallback;

	public delegate void Calltype_zfb_evalJS(int id, string script, string scriptURL);
	public static Calltype_zfb_evalJS zfb_evalJS;

	public delegate void Calltype_zfb_registerJSCallback(int id, ForwardJSCallFunc cb);
	public static Calltype_zfb_registerJSCallback zfb_registerJSCallback;

	public delegate void Calltype_zfb_registerChangeCallback(int id, ChangeFunc cb);
	public static Calltype_zfb_registerChangeCallback zfb_registerChangeCallback;

	public delegate CursorType Calltype_zfb_getMouseCursor(int id, out int width, out int height);
	public static Calltype_zfb_getMouseCursor zfb_getMouseCursor;

	public delegate void Calltype_zfb_getMouseCustomCursor(int id, IntPtr buffer, int width, int height, out int hotX, out int hotY);
	public static Calltype_zfb_getMouseCustomCursor zfb_getMouseCustomCursor;

	public delegate void Calltype_zfb_registerDialogCallback(int id, DisplayDialogFunc cb);
	public static Calltype_zfb_registerDialogCallback zfb_registerDialogCallback;

	public delegate void Calltype_zfb_sendDialogResults(int id, bool affirmed, string text1, string text2);
	public static Calltype_zfb_sendDialogResults zfb_sendDialogResults;

	public delegate void Calltype_zfb_registerPopupCallback(int id, NewWindowFunc cb);
	public static Calltype_zfb_registerPopupCallback zfb_registerPopupCallback;

	public delegate void Calltype_zfb_registerContextMenuCallback(int id, ShowContextMenuFunc cb);
	public static Calltype_zfb_registerContextMenuCallback zfb_registerContextMenuCallback;

	public delegate void Calltype_zfb_sendContextMenuResults(int id, int commandId);
	public static Calltype_zfb_sendContextMenuResults zfb_sendContextMenuResults;

	public delegate void Calltype_zfb_sendCommandToFocusedFrame(int id, FrameCommand command);
	public static Calltype_zfb_sendCommandToFocusedFrame zfb_sendCommandToFocusedFrame;

	public delegate void Calltype_zfb_getCookies(int id, GetCookieFunc cb);
	public static Calltype_zfb_getCookies zfb_getCookies;

	public delegate void Calltype_zfb_editCookie(int id, NativeCookie cookie, CookieAction action);
	public static Calltype_zfb_editCookie zfb_editCookie;

	public delegate void Calltype_zfb_clearCookies(int id);
	public static Calltype_zfb_clearCookies zfb_clearCookies;


#endif
}

}
