using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXLights {
    
    public class Properties70 {
        public string id;
        public string name;
        public int lighType = 0;
        public int castShadow = 0;
        public float range = 100.0f;    //intensity in fbx
        public float intensity = 1.0f;
        public float outerAngle = 30.0f;
        public Color color = Color.white;
    }

    private FBXSetting setting;
    private Dictionary<string, Properties70> properties70 = null;
    private bool _isDone = false;
    public bool IsDone {
        get { return this._isDone; }
        set { this._isDone = value; }
    }

    public void Init (FBXSetting setting) {
        this.setting = setting;

        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread: this.Run (); break;
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread: UniFBXThread.RunAsync (this.Run); break;
#endif
            default: this.Run (); break;
        }
    }

    private void Run ( ) {
        this.IsDone = false;
        this.properties70 = new Dictionary<string, Properties70> ();
        var indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("\"NodeAttribute::\", \"Light\" {")).ToList ();
        string[] data = new string[0];
        for (int i = 0; i < indexes.Count; i++) {
            Properties70 prop70 = new Properties70 ();
            var j = indexes[i];
            data = UniFBX.list[j].Split (new char[] { ':', ',', '\"' });
            prop70.id = data[1].Trim ();
            prop70.name = data[5].Trim ();

            for (int k = j; k < (j + 5); k++) {
                if (UniFBX.list[k].Contains ("\"Color\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.color.r = float.Parse (data[data.Length - 3]);
                    prop70.color.g = float.Parse (data[data.Length - 2]);
                    prop70.color.b = float.Parse (data[data.Length - 1]);
                    prop70.color.a = 1.0f;
                }
                else if (UniFBX.list[k].Contains ("\"LightType\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.lighType = int.Parse (data[data.Length - 1]);
                }
                else if (UniFBX.list[k].Contains ("\"Intensity\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.range = float.Parse (data[data.Length - 1]);
                }
                else if (UniFBX.list[k].Contains ("\"CastShadows\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.castShadow = int.Parse (data[data.Length - 1]);
                }
                else if (UniFBX.list[k].Contains ("\"OuterAngle\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.outerAngle = float.Parse (data[data.Length - 1]);
                    break;
                }
            }

            this.properties70.Add (prop70.id, prop70);
        }
        this.IsDone = true;
    }

    public Properties70 GetProperty (string nodeAttributeID) {
        if (this.properties70 == null) return null;
        if (!this.properties70.ContainsKey (nodeAttributeID)) return null;
        return this.properties70[nodeAttributeID];
    }

    public void Clear ( ) {        
        if (this.properties70 != null) this.properties70.Clear (); ;
        this.properties70 = null;        
    }

}