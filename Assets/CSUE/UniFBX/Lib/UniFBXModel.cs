using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXModel {

    public class Properties70 {
        public string id;
        public string name;
        public Vector3 LclPosition = Vector3.zero;
        public Quaternion LclRotation = Quaternion.identity;
        public Vector3 LclScaling = Vector3.one;

        public void Reset ( ) {
            this.LclPosition = Vector3.zero;
            this.LclRotation = Quaternion.identity;
            this.LclScaling = Vector3.one;
        }
    }

    private FBXSetting setting;
    private Dictionary<string, Properties70> properties70 = null;
    private List<string> nameObjects = null;    
    private List<UniFBXModel.Properties70> LclTransform = null;
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
        this.nameObjects = new List<string> ();        
        this.LclTransform = new List<Properties70> ();

        var indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("\"Model::")).ToList ();
        string[] data = new string[0];
        for (int i = 0; i < indexes.Count; i++) {
            Properties70 prop70 = new Properties70 ();
            var j = indexes[i];
            data = UniFBX.list[j].Split (new char[] { ':', ',', '\"' });
            prop70.id = data[1].Trim ();
            prop70.name = data[5].Trim ();
            Vector3 p = Vector3.zero;
            Vector3 r = Vector3.zero;
            Quaternion q = Quaternion.identity;

            for (int k = (j + 1); k < (j + 25); k++) {
                if (UniFBX.list[k].Contains ("Lcl Translation")) {
                    data = UniFBX.list[k].Split (',');
                    p.x = -float.Parse (data[data.Length - 3]);
                    p.y = float.Parse (data[data.Length - 2]);
                    p.z = float.Parse (data[data.Length - 1]);
                    p *= this.setting.meshes.scaleFactor;
                    prop70.LclPosition = p;
                }
                else if (UniFBX.list[k].Contains ("Lcl Rotation")) {
                    data = UniFBX.list[k].Split (',');
                    r.x = float.Parse (data[data.Length - 3]);
                    r.y = -float.Parse (data[data.Length - 2]);
                    r.z = -float.Parse (data[data.Length - 1]);
                    if (r.x == 0) {
                        //YZX sequence
                        q = Quaternion.Euler (Vector3.up * r.y);
                        q *= Quaternion.Euler (Vector3.forward * r.z);
                        q *= Quaternion.Euler (Vector3.right * r.x);
                    }
                    else {
                        //ZYX sequence
                        q = Quaternion.Euler (Vector3.forward * r.z);
                        q *= Quaternion.Euler (Vector3.up * r.y);
                        q *= Quaternion.Euler (Vector3.right * r.x);
                    }
                    prop70.LclRotation = q;
                }
                else if (UniFBX.list[k].Contains ("Lcl Scaling")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.LclScaling.x = float.Parse (data[data.Length - 3]);
                    prop70.LclScaling.y = float.Parse (data[data.Length - 2]);
                    prop70.LclScaling.z = float.Parse (data[data.Length - 1]);
                }
                else if (UniFBX.list[k].Contains ("Shading:")) {
                    break;
                }
                else if (UniFBX.list[k].Contains ("Connections:  {")) {
                    this.IsDone = true;
                    return;
                }
            }
            
            this.properties70.Add (prop70.id, prop70);
            this.LclTransform.Add (prop70);
            this.nameObjects.Add (prop70.name);            
        }
        this.IsDone = true;
    }

    public string GetName (int index ) {
        return this.nameObjects[index];
    }

    public Vector3 GetPosition (int index) {
        return this.LclTransform[index].LclPosition;
    }

    public Quaternion GetRotation (int index) {
        return this.LclTransform[index].LclRotation;
    }

    public Vector3 GetScale (int index) {
        return this.LclTransform[index].LclScaling;
    }

    public void Clear ( ) {
        if (this.properties70 != null) this.properties70.Clear ();
        if (this.nameObjects != null) this.nameObjects.Clear ();        
        if (this.LclTransform != null) this.LclTransform.Clear ();
        this.properties70 = null;
        this.nameObjects = null;
        this.LclTransform = null;
    }

}