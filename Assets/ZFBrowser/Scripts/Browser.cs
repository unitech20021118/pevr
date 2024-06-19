using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace ZenFulcrum.EmbeddedBrowser {

/** Represents a browser "tab". */
public class Browser : MonoBehaviour {
	private static int lastUpdateFrame;

	public static string LocalUrlPrefix { get { return BrowserNative.LocalUrlPrefix; } }

	/**
	 * List of possible actions when a new window is opened.
	 */
	[Flags]
	public enum NewWindowAction {
		/** Ignore attempts to open new windows. */
		Ignore = 1,
		/** Navigate the current window to the new window's URL. */
		Redirect,
		/**
		 * Create a new Browser instance to handle the new window.
		 * For this to be useful, you'll typically want to fill NewWindowHandler with an
		 * implementation of your choosing.
		 *
		 * (If NewWindowHandler isn't set, an exception will be given; without you setting it up for us,
		 * we don't know where in the scene to place the new Browser or what to make it look like.)
		 */
		NewBrowser,
		/**
		 * Create a new OS window, outside the game, to show the page.
		 * At present, there is no way to control or interact with the new window outside JavaScript calls
		 * from the parent.
		 */
		NewWindow,
	}

	protected IBrowserUI _uiHandler;
	protected bool uiHandlerAssigned = false;
	/**
	 * Input handler.
	 * If you don't assign anything, it will default to something useful, but you can replace
	 * it or null it as desired.
	 *
	 * If do you want to use your own or disable it, be sure to assign something (or null) before WhenReady fires.
	 */
	public IBrowserUI UIHandler {
		get { return _uiHandler; }
		set {
			uiHandlerAssigned = true;
			_uiHandler = value;
		}
	}

	[Tooltip("Initial URL to load.\n\nTo change at runtime use browser.Url to load a page.")]
	[SerializeField] private string _url = "";

	[Tooltip("Initial size.\n\nTo change at runtime use browser.Resize.")]
	[SerializeField] private int _width = 512, _height = 512;

	[Tooltip(@"Generate mipmaps?

Generating mipmaps tends to be somewhat expensive, especially when updating a large texture every frame. Instead of
generating mipmaps, try using one of the ""emulate mipmap"" shader variants.

To change at runtime modify this value and call browser.Resize.")]
	public bool generateMipmap = false;

	[Tooltip(@"Initial background color to use for pages.
You may pick any opaque color. As a special case, if alpha == 0 the entire background will be rendered transparent.
(Be sure to use a transparent-capable shader to see it.)

This value cannot be changed after the first InputUpdate() tick, create a new browser if you need a different option.")]
	public Color32 backgroundColor = new Color32(0, 0, 0, 0);//default to transparent


	[Tooltip(@"Initial browser ""zoom level"". Negative numbers are smaller, zero is normal, positive numbers are larger.
The size roughly doubles/halves for every four units added/removed.
Note that zoom level is shared by all pages on the some domain.
Also note that this zoom level may be persisted across runs.

To change at runtime use browser.Zoom.")]
//TODO: prefer deviceScale (not yet implemented) for DPI-style size changes.
	[SerializeField] private float _zoom = 0;

	/**
	 * Fired when we get a console.log/warn/error from the page.
	 * args: (message, source)
	 *
	 * (CEF's console event leaves a lot to be desired, we are unable to get the log level or additional arguments.)
	 */
	public event Action<string, string> onConsoleMessage = (s, s1) => {};

	[Tooltip(@"Allow right-clicking to show a context menu on what parts of the page?

May be changed at any time.
")]
	[FlagsField]
	public BrowserNative.ContextMenuOrigin allowContextMenuOn = BrowserNative.ContextMenuOrigin.Editable;

	[Tooltip(@"What should we do when a user/the page tries to open a new window?

For ""New Browser"" to work, you need to assign NewWindowHandler to a handler of your creation.
")]
	public NewWindowAction newWindowAction = NewWindowAction.Redirect;
	/** If newWindowAction == NewBrowser, this will be called to create the new browser object. */
	public INewWindowHandler NewWindowHandler { get; set; }

	/** If false, the texture won't be updated with new changes. */
	public bool EnableRendering { get; set; }
	/** If false, we won't process input with the UIHandler. */
	public bool EnableInput { get; set; }

	public CookieManager CookieManager { get; private set; }

	/** Handle to the native browser. */
	internal protected int browserId;
	/** Same as browserId, but will be set before the browser is ready and remain set even after it's disposed */
	private int unsafeBrowserId;
	/** Have we requested a native handle yet? (It may take a moment for the native browser to be ready.) */
	protected bool browserIdRequested = false;
	protected Texture2D texture;
	public Texture2D Texture { get { return texture; } }
	/** Called when the image canvas has changed or resized. */
    public event System.Action<Texture2D> afterResize = t => { };
	protected bool textureIsOurs = false;
	protected bool forceNextRender = true;

	/** List of tasks to execute on the main thread. May be used on any thread, but lock before touching. */
	protected List<Action> thingsToDo = new List<Action>();
	/** List of callbacks to call when the page loads next. */
	protected List<Action> onloadActions = new List<Action>();

	/**
	 * We pass delegates/closures to the native level. We must ensure that they don't get GC'd
	 * while the native object still exists and might use them, so we keep track of them here
	 * to prevent that.
	 */
	protected List<object> thingsToRemember = new List<object>();
	/**
	 * And, to make it more complicated, in some cases we can get GC'd (along with thingsToRemember and the
	 * generated trampolines) before the native browser finishes shutting down.
	 *
	 * We use this to make sure {this} stays alive until the native side is done.
	 *
	 * Used across threads, lock before touching.
	 */
	protected static Dictionary<int, List<object>> allThingsToRemember = new Dictionary<int, List<object>>();

	/** A callback. {args} is a JSON node with the top-level type of array. */
	public delegate void JSCallback(JSONNode args);
	protected delegate void JSResultFunc(JSONNode value, bool isError);

	private int nextCallbackId = 1;
	/** Registered callbacks that JS can call to us with. */
	protected Dictionary<int, JSResultFunc> registeredCallbacks = new Dictionary<int, JSResultFunc>();


	/**
	 * We can't do much (go to url, navigate, etc) until the native browser is ready.
	 * Most these actions will be queued for you and fired when we are ready.
	 *
	 * See also: WhenReady()
	 */
	protected event BrowserNative.ReadyFunc onNativeReady;

	/**
	 * Called when the page's onload fires. (Top frame only.)
	 * loadData['status'] contains the status code, loadData['url'] the url
	 */
    public event System.Action<JSONNode> onLoad = loadData => { };
	/**
	 * Called when the top-level page has been fetched (but not necessarily parsed and run).
	 * loadData['status'] contains the status code, loadData['url'] the url
	 * (Top frame only.)
	 */
    public event System.Action<JSONNode> onFetch = loadData => { };
	/**
	 * Called when a page fails to load.
	 * Use QueuePageReplacer to inject a custom error page.
	 * (Top frame only.)
	 */
    public event System.Action<JSONNode> onFetchError = errCode => { };
	/**
	 * Called when an SSL cert fails checks.
	 * Use QueuePageReplacer to inject a custom error page.
	 * (Top frame only.)
	 */
    public event System.Action<JSONNode> onCertError = errInfo => { };
	/**
	 * Called when a renderer process dies/is killed.
	 * Use QueuePageReplacer to inject a custom error page; you might also choose to try reloading once or twice.
	 */
	public event Action onSadTab = () => {};


	private BrowserInput browserInput;
	private Browser overlay;
	/** We have to load a blank page before we can inject HTML. If we load a blank page, don't count it as the "loading". */
	private bool skipNextLoad;
	/** There may be a short moment between requesting a URL and when IsLoadedRaw turns false. We use this flag to help cope. */
	private bool loadPending;

	/**
	 * This will sometimes contain an inner Browser that handles tasks such as
	 * rendering alert()s and such.
	 */
	protected DialogHandler dialogHandler;

	protected void Awake() {
		EnableRendering = true;
		EnableInput = true;
		CookieManager = new CookieManager(this);

		browserInput = new BrowserInput(this);

		onNativeReady += id => {
			if (!uiHandlerAssigned) {
                print("5454564564");
				var meshCollider = GetComponent<MeshCollider>();
				if (meshCollider) UIHandler = ClickMeshBrowserUI.Create(meshCollider);
			}

			Resize(_width, _height);

			Zoom = _zoom;
            print(_url);
			if (!string.IsNullOrEmpty(_url)) Url = _url;
		};

		onConsoleMessage += (message, source) => {
			var text = source + ": " + message;
			Debug.Log(text, this);
		};

		onFetchError += err => {
			//don't show anything if the error is a load abort
			if (err["error"] == "ERR_ABORTED") return;

			QueuePageReplacer(() => {
				LoadHTML(Resources.Load<TextAsset>("Browser/Errors").text, Url);
				CallFunction("setErrorInfo", err);
			}, -1000);
		};

		onCertError += err => {
			QueuePageReplacer(() => {
				LoadHTML(Resources.Load<TextAsset>("Browser/Errors").text, Url);
				CallFunction("setErrorInfo", err);
			}, -900);
		};

		onSadTab += () => {
			QueuePageReplacer(() => {
				LoadHTML(Resources.Load<TextAsset>("Browser/Errors").text, Url);
				CallFunction("showCrash");
			}, -1000);
		};
	}

	/** Returns true if the browser is ready to take orders. Most actions will be internally delayed until it is. */
	public bool IsReady {
		get { return browserId != 0; }
	}

	/**
	 * The given callback will be called when the browser is ready to start taking commands.
	 */
	public void WhenReady(Action callback) {
		if (IsReady) {
			//Call it later instead of now to help head off some subtle bugs that can be produced by such a scheme.
			//Call it at next update. (Since our script order is a little bit later than everyone else this usually will add no latency.)
			lock (thingsToDo) thingsToDo.Add(callback);
		} else {
			BrowserNative.ReadyFunc func = null;
			func = id => {
				callback();
				onNativeReady -= func;
			};
			onNativeReady += func;
		}
	}

	/** Fires the given callback during th next Update/LateUpdate tick on the main thread. This may be called from any thread. */
	public void RunOnMainThread(Action callback) {
		lock (thingsToDo) thingsToDo.Add(callback);
	}

	/**
	 * Calls the given callback the next time the page is loaded.
	 * This will not fire right away if IsLoaded is true, it will wait for a new page to load.
	 * Callbacks won't be fired yet if the url is some type of blank url ("", "about:blank", etc).
	 */
	public void WhenLoaded(Action callback) {
		onloadActions.Add(callback);
	}

	/**
	 * Sets up a new native browser.
	 * If newBrowserId is zero, allocates a new browser and sets it up.
	 * If newBrowserId is nonzero, takes ownership of that allocated browser and sets it up.
	 */
	private void RequestNativeBrowser(int newBrowserId = 0) {
		if (browserId != 0 || browserIdRequested) return;

		browserIdRequested = true;

		try {
			BrowserNative.LoadNative();
		} catch {
			gameObject.SetActive(false);
			throw;
		}

		int newId;
		if (newBrowserId == 0) {
			var settings = new BrowserNative.ZFBSettings() {
				bgR = backgroundColor.r,
				bgG = backgroundColor.g,
				bgB = backgroundColor.b,
				bgA = backgroundColor.a,
				offscreen = 1,
			};
			newId = BrowserNative.zfb_createBrowser(settings);
		} else {
			newId = newBrowserId;
		}

		unsafeBrowserId = newId;

		//Debug.Log("Requested browser for " + name + " " + newId);

		//We have a native browser, but it is invalid to do anything with it until it's ready.
		//Therefore, we don't set browserId until it's ready.

		//But we will put all our callbacks in place.

		//Don't let our remember list get destroyed until we are ready for that.
		lock (allThingsToRemember) allThingsToRemember[newId] = thingsToRemember;

		BrowserNative.ForwardJSCallFunc forwardCall = (bId, id, data, size) => {
			lock (thingsToDo) thingsToDo.Add(() => {

				JSResultFunc cb;
				if (!registeredCallbacks.TryGetValue(id, out cb)) {
					Debug.LogWarning("Got a JS callback for event " + id + ", but no such event is registered.");
					return;
				}

				var isError = false;
				if (data.StartsWith("fail-")) {
					isError = true;
					data = data.Substring(5);
				}

				JSONNode node;
				try {
					node = JSONNode.Parse(data);
				} catch (SerializationException) {
					Debug.LogWarning("Invalid JSON sent from browser: " + data);
					return;
				}

				try {
					cb(node, isError);
				} catch (Exception ex) {
					//user's function died, log it and carry on
					Debug.LogException(ex);
					return;
				}

			});
		};
		thingsToRemember.Add(forwardCall);
		BrowserNative.zfb_registerJSCallback(newId, forwardCall);


		BrowserNative.ChangeFunc changeCall = (id, type, arg1) => {
			//(Note: we may have been Object.Destroy'd at this point, so guard against that.)

			if (type == BrowserNative.ChangeType.CHT_BROWSER_CLOSE) {
				//We can't continue if the browser is closed, so goodbye.

				//At this point, we may or may not be destroyed, but if not, become destroyed.
				//Debug.Log("Got close notification for " + unsafeBrowserId);
				if (this) {
					//Need to be destroyed.
					lock (thingsToDo) thingsToDo.Add(() => {
						Destroy(gameObject);
					});
				} else {
					//If we are (Unity) destroyed, we won't get another update, so we can't rely on thingsToDo
					//That said, there's not anything else for us to do but step out of allThingsToRemember.
				}

				//The native side has acknowledged it's done, now we can finally let the native trampolines be GC'd
				lock (allThingsToRemember) {
					allThingsToRemember.Remove(unsafeBrowserId);
				}

				//Just in case someone tries to call something, make sure CheckSanity and such fail.
				browserId = 0;
			} else if (this) {
				lock (thingsToDo) thingsToDo.Add(() => OnItemChange(type, arg1));
			}
		};
		thingsToRemember.Add(changeCall);
		BrowserNative.zfb_registerChangeCallback(newId, changeCall);

		BrowserNative.DisplayDialogFunc dialogCall = (id, type, text, promptText, url) => {
			lock (thingsToDo) thingsToDo.Add(() => {
				CreateDialogHandler();
				dialogHandler.HandleDialog(type, text, promptText);
			});
		};
		thingsToRemember.Add(dialogCall);
		BrowserNative.zfb_registerDialogCallback(newId, dialogCall);

		BrowserNative.ShowContextMenuFunc contextCall = (id, json, x, y, origin) => {
			if (json != null && (allowContextMenuOn & origin) == 0) {
				//ignore this
				BrowserNative.zfb_sendContextMenuResults(browserId, -1);
				return;
			}

			lock (thingsToDo) thingsToDo.Add(() => {
				if (json != null) CreateDialogHandler();
				if (dialogHandler != null) dialogHandler.HandleContextMenu(json, x, y);
			});
		};
		thingsToRemember.Add(contextCall);
		BrowserNative.zfb_registerContextMenuCallback(newId, contextCall);

		BrowserNative.NewWindowFunc popupCall = (int id, IntPtr urlPtr, bool userInvoked, int possibleId, ref BrowserNative.ZFBSettings possibleSettings) => {
			if (!userInvoked) {
				return BrowserNative.NewWindowAction.NWA_IGNORE;
			}

			switch (newWindowAction) {
				default:
				case NewWindowAction.Ignore:
					return BrowserNative.NewWindowAction.NWA_IGNORE;
				case NewWindowAction.Redirect:
					return BrowserNative.NewWindowAction.NWA_REDIRECT;
				case NewWindowAction.NewBrowser:
					if (NewWindowHandler != null) {
						possibleSettings.bgR = backgroundColor.r;
						possibleSettings.bgG = backgroundColor.g;
						possibleSettings.bgB = backgroundColor.b;
						possibleSettings.bgA = backgroundColor.a;

						lock (thingsToDo) {
							thingsToDo.Add(() => {
								var newBrowser = NewWindowHandler.CreateBrowser(this);
								newBrowser.RequestNativeBrowser(possibleId);
							});
							return BrowserNative.NewWindowAction.NWA_NEW_BROWSER;
						}
					} else {
						Debug.LogError("Missing NewWindowHandler, can't open new window", this);
						return BrowserNative.NewWindowAction.NWA_IGNORE;
					}
				case NewWindowAction.NewWindow:
					return BrowserNative.NewWindowAction.NWA_NEW_WINDOW;
			}
		};
		thingsToRemember.Add(popupCall);
		BrowserNative.zfb_registerPopupCallback(newId, popupCall);

		BrowserNative.ConsoleFunc consoleCall = (id, message, source, line) => {
			lock (thingsToDo) thingsToDo.Add(() => {
				onConsoleMessage(message, source + ":" + line);
			});
		};
		thingsToRemember.Add(consoleCall);
		BrowserNative.zfb_registerConsoleCallback(newId, consoleCall);


		BrowserNative.ReadyFunc readyCall = id => {
			Assert.AreEqual(newId, id);
			//We could be on any thread at this time, so schedule the callbacks to fire during the next InputUpdate
			lock (thingsToDo) thingsToDo.Add(() => {
				browserId = newId;

				// ReSharper disable once PossibleNullReferenceException
				onNativeReady(browserId);
			});
		};
		thingsToRemember.Add(readyCall);
		BrowserNative.zfb_setReadyCallback(newId, readyCall);
	}

	protected void OnItemChange(BrowserNative.ChangeType type, string arg1) {
		//Debug.Log("ChangeType " + name + " " + type + " arg " + arg1 + " loaded " + IsLoaded);
		switch (type) {
			case BrowserNative.ChangeType.CHT_CURSOR:
				UpdateCursor();
				break;
			case BrowserNative.ChangeType.CHT_BROWSER_CLOSE:
				//handled directly on the calling thread, nothing to do here
				break;
			case BrowserNative.ChangeType.CHT_FETCH_FINISHED:
				onFetch(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_FETCH_FAILED:
				onFetchError(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_LOAD_FINISHED:
				if (skipNextLoad) {
					//deal with extra step we have to do to load HTML to an empty page
					skipNextLoad = false;
					return;
				}

				loadPending = false;

				if (onloadActions.Count != 0) {
					foreach (var action in onloadActions) action();
					onloadActions.Clear();
				}

				onLoad(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_CERT_ERROR:
				onCertError(JSONNode.Parse(arg1));
				break;
			case BrowserNative.ChangeType.CHT_SAD_TAB:
				onSadTab();
				break;
		}
	}

	protected void CreateDialogHandler() {
		if (dialogHandler != null) return;

		DialogHandler.DialogCallback dialogCallback = (affirm, text1, text2) => {
			CheckSanity();
			BrowserNative.zfb_sendDialogResults(browserId, affirm, text1, text2);
		};
		DialogHandler.MenuCallback contextCallback = commandId => {
			CheckSanity();
			BrowserNative.zfb_sendContextMenuResults(browserId, commandId);
		};

		dialogHandler = DialogHandler.Create(this, dialogCallback, contextCallback);
	}

	/**
	 * Call this before you do any native things with our browser instance.
	 * If something terribly stupid is going on this may be able to bail out with an exception instead of
	 * crashing everything.
	 */
	protected void CheckSanity() {
		if (browserId == 0) throw new InvalidOperationException("No native browser allocated");
	}

	/**
	 * If we aren't ready, queues the given action to happen later and returns true.
	 * Else calls CheckSanity and returns false.
	 */
	internal bool DeferUnready(Action ifNotReady) {
		if (browserId == 0) {
			WhenReady(ifNotReady);
			return true;
		} else {
			CheckSanity();
			return false;
		}
	}

	protected void OnDisable() {
		//note: if you want a browser to stop, load a different page or destroy it
		//The browser will continue to run until we are destroyed.
	}

	protected void OnDestroy() {
		if (browserId == 0) return;

		if (dialogHandler) DestroyImmediate(dialogHandler.gameObject);
		dialogHandler = null;

		BrowserNative.zfb_destoryBrowser(browserId);
		if (textureIsOurs) Destroy(texture);

		browserId = 0;
		texture = null;
	}

	protected void OnApplicationQuit() {
		//According to http://docs.unity3d.com/Manual/ExecutionOrder.html,
		//OnDisable will be called before this. Experience shows this to be not so much the case.
		//Therefore, we will forcefully call OnDestroy()
		OnDestroy();

		if (BrowserNative.zfb_numBrowsers() == 0) {
			//last one out, turn off the lights

			//beforeunload windows won't fully disappear without ticking the message loop
			//Ideally, we'd just keep ticking it, but we are stopping.
			for (int i = 0; i < 10; ++i) {
				BrowserNative.zfb_tick();
				System.Threading.Thread.Sleep(10);
			}


			#if UNITY_EDITOR
			//You can't re-init CEF, so if we are the editor, never shut it down.
			#else
			BrowserNative.UnloadNative();
			#endif
		}
	}

	public string Url {
		/** Gets the current browser URL. */
		get {
			if (!IsReady) return "";
			CheckSanity();

			var urlData = BrowserNative.zfb_getURL(browserId);
			var ret = Marshal.PtrToStringAnsi(urlData);
			BrowserNative.zfb_free(urlData);
			return ret;
		}
		/** Shortcut for LoadURL(value, true) */
		set {
			LoadURL(value, true);
		}
	}

	/**
	 * Navigates to the given URL. If force is true, it will go there right away.
	 * If force is false, the pages that wish to can prompt the user and possibly cancel the
	 * navigation.
	 */
	public void LoadURL(string url, bool force) {
		if (DeferUnready(() => LoadURL(url, force))) return;

		const string magicPrefix = "localGame://";

		if (url.StartsWith(magicPrefix)) {
			url = LocalUrlPrefix + url.Substring(magicPrefix.Length);
		}

		if (string.IsNullOrEmpty(url)) {
			//If we ask CEF to load "" it will crash. Try Url = "about:blank" or LoadHTML() instead.
			throw new ArgumentException("URL must be non-empty", "value");
		}

		loadPending = true;
        print(url);
		BrowserNative.zfb_goToURL(browserId, url, force);
	}

	/**
	 * Loads the given HTML string as if it were the given URL.
	 * Use http://-like porotocols or else things may not work right.
	 *
	 * Note that, instead of using this, you can also load "data:" URIs into this.Url.
	 * This allows pretty much any type of content to be loaded as the whole page.
	 */
	public void LoadHTML(string html, string url = null) {
		if (DeferUnready(() => LoadHTML(html, url))) return;

		//Debug.Log("Load HTML " + html);

		loadPending = true;

		if (string.IsNullOrEmpty(url)) {
			url = LocalUrlPrefix + "custom";
		}

		if (string.IsNullOrEmpty(this.Url)) {
			//Nothing will happen if we don't have an initial page, so cause one.
			this.Url = "about:blank";
			skipNextLoad = true;
		}

		BrowserNative.zfb_goToHTML(browserId, html, url);
	}

	/**
	 * Sends a command such as "select all", "undo", or "copy"
	 * to the currently focused frame in th browser.
	 */
	public void SendFrameCommand(BrowserNative.FrameCommand command) {
		if (DeferUnready(() => SendFrameCommand(command))) return;

		BrowserNative.zfb_sendCommandToFocusedFrame(browserId, command);
	}

	private Action pageReplacer;
	private float pageReplacerPriority;
	/**
	 * Queues a function to replace the current page.
	 *
	 * This is used mostly in error handling. Namely, the default error handler will queue an error page at a low
	 * priority, but your onLoadError callback can call this to queue its own error page.
	 *
	 * At the end of the tick, the {replacePage} callback with the highest priority will
	 * be called. Typically {replacePage} will call LoadHTML to change things around.
	 *
	 * Default error handles will have a priority of less than -100.
	 */
	public void QueuePageReplacer(Action replacePage, float priority) {
		if (pageReplacer == null || priority >= pageReplacerPriority) {
			pageReplacer = replacePage;
			pageReplacerPriority = priority;
		}
	}

	public bool CanGoBack {
		get {
			if (!IsReady) return false;
			CheckSanity();
			return BrowserNative.zfb_canNav(browserId, -1);
		}
	}

	public void GoBack() {
		if (!IsReady) return;
		CheckSanity();
		BrowserNative.zfb_doNav(browserId, -1);
	}

	public bool CanGoForward {
		get {
			if (!IsReady) return false;
			CheckSanity();
			return BrowserNative.zfb_canNav(browserId, 1);
		}
	}

	public void GoForward() {
		if (!IsReady) return;
		CheckSanity();
		BrowserNative.zfb_doNav(browserId, 1);
	}

	/**
	 * Returns true if the browser is loading a page.
	 * Unlike IsLoaded, this does not account for special case urls.
	 */
	public bool IsLoadingRaw {
		get {
			if (!IsReady) return false;
			return BrowserNative.zfb_isLoading(browserId);
		}
	}

	/**
	 * Returns true if we have a page and it's loaded.
	 * This will not return true if we haven't gone to a URL or we are on "about:blank"
	 */
	public bool IsLoaded {
		get {
			if (!IsReady || loadPending) return false;
			if (BrowserNative.zfb_isLoading(browserId)) return false;

			var url = Url;
			var urlIsBlank = string.IsNullOrEmpty(url) || url == "about:blank";

			return !urlIsBlank;
		}
	}

	public void Stop() {
		if (!IsReady) return;
		CheckSanity();
		BrowserNative.zfb_changeLoading(browserId, BrowserNative.LoadChange.LC_STOP);
	}

	public void Reload(bool force = false) {
		if (!IsReady) return;
		CheckSanity();
		if (force) BrowserNative.zfb_changeLoading(browserId, BrowserNative.LoadChange.LC_FORCE_RELOAD);
		else BrowserNative.zfb_changeLoading(browserId, BrowserNative.LoadChange.LC_RELOAD);
	}




	/**
	 * Show the development tools for the current page.
	 *
	 * If {show} is false the dev tools will be hidden, if possible.
	 */
	public void ShowDevTools(bool show = true) {
		if (DeferUnready(() => ShowDevTools(show))) return;

		BrowserNative.zfb_showDevTools(browserId, show);
	}

	public Vector2 Size {
		get { return new Vector2(_width, _height); }
	}

	protected void _Resize(Texture2D newTexture, bool newTextureIsOurs) {

		var width = newTexture.width;
		var height = newTexture.height;

		if (textureIsOurs && texture && newTexture != texture) {
			Destroy(texture);
		}

		_width = width;
		_height = height;

		if (IsReady) BrowserNative.zfb_resize(browserId, width, height);
		else WhenReady(() => BrowserNative.zfb_resize(browserId, width, height));

		texture = newTexture;
		textureIsOurs = newTextureIsOurs;

		var renderer = GetComponent<Renderer>();
		if (renderer) renderer.material.mainTexture = texture;

		afterResize(texture);

		if (overlay) overlay.Resize(Texture);
	}

	/**
	 * Creates a new texture of the given size and starts rendering to that.
	 */
	public void Resize(int width, int height) {
		var newTexture = new Texture2D(width, height, TextureFormat.ARGB32, generateMipmap);
		if (generateMipmap) newTexture.filterMode = FilterMode.Trilinear;
		newTexture.wrapMode = TextureWrapMode.Clamp;

		_Resize(newTexture, true);
	}

	/** Tells the Browser to render to the given ARGB32 texture. */
	public void Resize(Texture2D newTexture) {
		Assert.AreEqual(TextureFormat.ARGB32, newTexture.format);
		_Resize(newTexture, false);
	}

	/** Sets and gets the current zoom level/DPI scaling factor. */
	public float Zoom {
		get { return _zoom; }
		set {
			if (DeferUnready(() => Zoom = value)) return;

			BrowserNative.zfb_setZoom(browserId, value);
			_zoom = value;
		}
	}

	/**
	 * Evaluates JavaScript in the browser.
	 * NB: This is JavaScript. Not UnityScript. If you try to feed this UnityScript it will choke and die.
	 *
	 * If IsLoaded is false, the script will be deferred until IsLoaded is true.
	 *
	 * The script is asynchronously executed in a separate process. To get the result value, yield on the returned
	 * promise (in a coroutine) then take a look at promise.Value.
	 *
	 * To see script errors and debug issues, call ShowDevTools and use the inspector window to tackle
	 * your problems. Also, keep an eye on console output (which gets forwarded to Debug.Log).
	 *
	 * If desired, you can fill out scriptURL with a URL for the file you are reading from. This can help fill out errors
	 * with the correct filename and in some cases allow you to view the source in the inspector.
	 */
	public IPromise<JSONNode> EvalJS(string script, string scriptURL = "scripted command") {
		//Debug.Log("Asked to EvalJS " + script + " loaded state: " + IsLoaded);
		var promise = new Promise<JSONNode>();
		var id = nextCallbackId++;

		var jsonScript = new JSONNode(script).AsJSON;
		var resultJS = @"try {"+
			  "_zfb_event(" + id + ", JSON.stringify(eval(" + jsonScript + " )) || 'null');" +
			"} catch(ex) {" +
				"_zfb_event(" + id + ", 'fail-' + (JSON.stringify(ex.stack) || 'null'));" +
			"}"
		;

		registeredCallbacks.Add(id, (val, isError) => {
			registeredCallbacks.Remove(id);
			if (isError) promise.Reject(new JSException(val));
			else promise.Resolve(val);
		});

		if (!IsLoaded) {
			WhenLoaded(() => _EvalJS(resultJS, scriptURL));
		} else {
			_EvalJS(resultJS, scriptURL);
		}

		return promise;
	}

	protected void _EvalJS(string script, string scriptURL) {
		BrowserNative.zfb_evalJS(browserId, script, scriptURL);
	}


	/**
	 * Looks up {name} by evaluating it as JavaScript code, then calls it with the given arguments.
	 *
	 * If IsLoaded is false, the call will be deferred until IsLoaded is true.
	 *
	 * Because {name} is evaluated, you can use lookups like "MyGUI.show" or "Foo.getThing().doBob"
	 *
	 * The call itself is run asynchronously in a separate process. To get the value returned by the JS back, yield
	 * on the promise CallFunction returns (in a coroutine) then take a look at promise.Value.
	 *
	 * Note that because JSONNode is implicitly convertible from many different types, you can often just
	 * dump the values in directly when you call this:
	 *   int x = 5, y = 47;
	 *   browser.CallFunction("Menu.setPosition", x, y);
	 *   browser.CallFunction("Menu.setTitle", "Super Game");
	 *
	 */
	public IPromise<JSONNode> CallFunction(string name, params JSONNode[] arguments) {
		var js = name + "(";

		var sep = "";
		foreach (var arg in arguments) {
			js += sep + arg.AsJSON;
			sep = ", ";
		}

		js += ");";

		return EvalJS(js);
	}

	/**
	 * Registers a JavaScript function in the Browser. When called, the given Mono {callback} will be executed.
	 *
	 * If IsLoaded is false, the in-page registration will be deferred until IsLoaded is true.
	 *
	 * The callback will be executed with one argument: a JSONNode array representing the arguments to the function
	 * given in the browser. (Access the first argument with args[0], second with args[1], etc.)
	 *
	 * The arguments sent back-and forth must be JSON-able.
	 *
	 * The JavaScript process runs asynchronously. Callbacks triggered will be collected and fired during the next Update().
	 *
	 * {name} is evaluate-assigned JavaScript. You can use values like "myCallback", "MySystem.myCallback" (only if MySystem
	 * already exists), or "GetThing().bobFunc" (if GetThing() returns an object you can use later).
	 *
	 */
	public void RegisterFunction(string name, JSCallback callback) {
		var id = nextCallbackId++;
		registeredCallbacks.Add(id, (value, error) => {
			//(we shouldn't be able to get an error here)
			callback(value);
		});

		var js = name + " = function() { _zfb_event(" + id + ", JSON.stringify(Array.prototype.slice.call(arguments))); };";
		EvalJS(js);
	}

	protected List<Action> thingsToDoClone = new List<Action>();
	protected void ProcessCallbacks() {
		while (thingsToDo.Count != 0) {
			Profiler.BeginSample("Browser.ProcessCallbacks", this);

			//It's not uncommon for some callbacks to add other callbacks
			//To keep from altering thingsToDo while iterating, we'll make a quick copy and use that.
			lock (thingsToDo) {
				thingsToDoClone.AddRange(thingsToDo);
				thingsToDo.Clear();
			}

			foreach (var thingToDo in thingsToDoClone) thingToDo();

			thingsToDoClone.Clear();

			Profiler.EndSample();
		}
	}

	protected void Update() {
		ProcessCallbacks();

		if (browserId == 0) {
			RequestNativeBrowser();
			return;//not ready yet or not loaded
		}

		CheckSanity();

		HandleInput();
	}

	protected void LateUpdate() {
		//Note: we use LateUpdate here in hopes that commands issued during (anybody's) Update()
		//will have a better chance of being completed before we push the render

		if (lastUpdateFrame != Time.frameCount) {
			Profiler.BeginSample("Browser.NativeTick");
			BrowserNative.zfb_tick();
			Profiler.EndSample();
			lastUpdateFrame = Time.frameCount;
		}

		ProcessCallbacks();

		if (pageReplacer != null) {
			pageReplacer();
			pageReplacer = null;
		}

		if (browserId == 0) return;//not ready yet or not loaded
		if (EnableRendering) Render();
	}

	/** Cached working memory for pixel operations */
	private Color32[] pixelData;

	protected void Render() {
		CheckSanity();

		BrowserNative.RenderData renderData;

		Profiler.BeginSample("Browser.UpdateTexture.zfb_getImage", this);
		{
			renderData = BrowserNative.zfb_getImage(browserId, forceNextRender);
			forceNextRender = false;

			if (renderData.pixels == IntPtr.Zero) return;//no changes

			if (renderData.w != texture.width || renderData.h != texture.height) {
				//Mismatch, can happen, for example, when we resize and ask for a new image before the IPC layer gets back to us.
				return;
			}

			if (pixelData == null || pixelData.Length != renderData.w * renderData.h) {
				pixelData = new Color32[renderData.w * renderData.h];
			}

		}
		Profiler.EndSample();

		/*
		Getting the frame data from CEF to the GPU is the biggest framerate bottleneck.

		This memcpy is unfortunate, and with more work we could just upload directly to the GPU texture from C++.
		That said, the profiler tells us:
			- With mipmaps on, GPUUpload takes an order of magnitude more time than DataCopy
			- With mipmaps off, GPUUpload takes 1-2x the time DataCopy does.
			- On my machine with a quite large 2048x2048 texture constantly updating, both weigh in at about ~10ms/frame
			  without mipmap generation.
		*/

		Profiler.BeginSample("Browser.UpdateTexture.DataCopy", this);
		{
			GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
			BrowserNative.zfb_memcpy(handle.AddrOfPinnedObject(), renderData.pixels, pixelData.Length * 4);
			handle.Free();
		}
		Profiler.EndSample();

		Profiler.BeginSample("Browser.UpdateTexture.GPUUpload", this);
		{
			texture.SetPixels32(pixelData);
			texture.Apply(true);
		}
		Profiler.EndSample();
	}

	/**
	 * Adds the given browser as an overlay of this browser.
	 *
	 * The overlaid browser will appear transparently over the top of us on our texture.
	 * {overlayBrowser} must not have an overlay and must be sized exactly the same as {this}.
	 * Additionally, overlayBrowser.EnableRendering must be false. You still need to
	 * do something to handle getting input to the right places. Overlays take a notable performance
	 * hit on rendering (CPU alpha compositing).
	 *
	 * Overlays are used internally to implement context menus and pop-up dialogs (alert, onbeforeunload).
	 * If the page causes any type of dialog, the overlay will be replaced.
	 *
	 * Overlays will be resized onto our texture when we are resized. The sizes must always match exactly.
	 *
	 * Remove the overlay (SetOverlay(null)) before closing either browser.
	 *
	 * (Note: though you can't set B as an overlay to A when B has an overlay, you can set
	 * an overlay on B /while/ it is the overlay for A. For an example of this, try
	 * right-clicking on the text area inside a prompt() popup. The context menu that
	 * appears is an overlay to the overlay to the actual browser.)
	 */
	public void SetOverlay(Browser overlayBrowser) {
		if (DeferUnready(() => SetOverlay(overlayBrowser))) return;
		if (overlayBrowser && overlayBrowser.DeferUnready(() => SetOverlay(overlayBrowser))) return;

		BrowserNative.zfb_setOverlay(browserId, overlayBrowser ? overlayBrowser.browserId : 0);
		overlay = overlayBrowser;

		if (!overlay) return;

		if (
			!overlay.Texture ||
			(overlay.Texture.width != Texture.width || overlay.Texture.height != Texture.height)
		) {
			overlay.Resize(Texture);
		}
	}

	protected void HandleInput() {
		if (_uiHandler == null || !EnableInput) return;
		CheckSanity();

		browserInput.HandleInput();
	}

	/**
	 * Updates the cursor on our UIHandler.
	 * Usually you don't need to call this, but if you are sharing input with an overlay, call this any time the
	 * "focused" overlay changes.
	 */
	public void UpdateCursor() {
		if (UIHandler == null) return;
		if (DeferUnready(UpdateCursor)) return;

		int w, h;
		var cursorType = BrowserNative.zfb_getMouseCursor(browserId, out w, out h);
		if (cursorType != BrowserNative.CursorType.Custom) {
			UIHandler.BrowserCursor.SetActiveCursor(cursorType);
		} else {
			if (w == 0 && h == 0) {
				//bad cursor
				UIHandler.BrowserCursor.SetActiveCursor(BrowserNative.CursorType.None);
				return;
			}

			var buffer = new Color32[w * h];
			int hx, hy;

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			BrowserNative.zfb_getMouseCustomCursor(browserId, handle.AddrOfPinnedObject(), w, h, out hx, out hy);
			handle.Free();

			var tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
			tex.SetPixels32(buffer);
			//in-memory only, no Apply()

			UIHandler.BrowserCursor.SetCustomCursor(tex, new Vector2(hx, hy));
			DestroyImmediate(tex);
		}
	}

}

}
