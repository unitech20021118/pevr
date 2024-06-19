using UnityEngine;
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
#endif

public class UniFBXThread : MonoBehaviour {
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
    public struct DelayedQueueItem {
        public float time;
        public Action action;
    }

    private static int numThreads;
    public static int maxThreads = 4;
    private List<Action> _actions = new List<Action> ();
    private List<Action> _currentActions = new List<Action> ();
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem> ();
    private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem> ();

    private int _count;
    private static bool initialized;
    private static UniFBXThread _current;
    public static UniFBXThread Current {
        get {
            Initialize ();
            return _current;
        }
    }

    private static void Initialize ( ) {
        if (!initialized) {
            if (!Application.isPlaying)
                return;
            initialized = true;
            var g = new GameObject ("UniFBXThread");
            _current = g.AddComponent<UniFBXThread> ();
        }
    }

    public static void QueueOnMainThread (Action action) {
        QueueOnMainThread (action, 0f);
    }

    public static void QueueOnMainThread (Action action, float time) {
        if (time != 0) {
            lock (Current._delayed) {
                Current._delayed.Add (new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else {
            lock (Current._actions) {
                Current._actions.Add (action);
            }
        }
    }

    public static Thread RunAsync (Action a) {
        Initialize ();
        while (numThreads >= maxThreads) Thread.Sleep (4);
        Interlocked.Increment (ref numThreads);
        ThreadPool.QueueUserWorkItem (RunAction, a);
        return null;
    }

    private static void RunAction (object action) {
        try {
            ((Action)action) ();
        }
        catch {
        }
        finally {
            Interlocked.Decrement (ref numThreads);
        }
    }

    void Awake ( ) {
        _current = this;
        initialized = true;
    }

    void Update ( ) {
        lock (_actions) {
            _currentActions.Clear ();
            _currentActions.AddRange (_actions);
            _actions.Clear ();
        }
        foreach (var a in _currentActions) {
            a ();
        }
        lock (_delayed) {
            _currentDelayed.Clear ();
            foreach (DelayedQueueItem d in _delayed) {
                if (d.time <= Time.time) _currentDelayed.Add (d);
            }
            foreach (var item in _currentDelayed) {
                _delayed.Remove (item);
            }
        }
        foreach (var delayed in _currentDelayed) {
            delayed.action ();
        }
    }

    void OnDisable ( ) {
        if (_current == this) _current = null;
    }

#endif
}