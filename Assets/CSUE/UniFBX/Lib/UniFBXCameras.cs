using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXCameras {

    public class Properties70 {
        public string id;
        public string name;
        public float fieldOfView = 60.0f;
        public Vector3 up = Vector3.up;
        public Vector3 lookAt = Vector3.up;
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
        var indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("\"NodeAttribute::\", \"Camera\" {")).ToList ();
        string[] data = new string[0];
        for (int i = 0; i < indexes.Count; i++) {
            Properties70 prop70 = new Properties70 ();
            var j = indexes[i];
            data = UniFBX.list[j].Split (new char[] { ':', ',', '\"' });
            prop70.id = data[1].Trim ();
            prop70.name = data[5].Trim ();

            for (int k = j; k < (j + 12); k++) {
                if (UniFBX.list[k].Contains ("Up")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.up.x = float.Parse (data[data.Length - 3]);
                    prop70.up.y = float.Parse (data[data.Length - 2]);
                    prop70.up.z = float.Parse (data[data.Length - 1]);
                }
                else if (UniFBX.list[k].Contains ("UpVector")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.lookAt.x = float.Parse (data[data.Length - 3]);
                    prop70.lookAt.y = float.Parse (data[data.Length - 2]);
                    prop70.lookAt.z = float.Parse (data[data.Length - 1]);
                }
                else if (UniFBX.list[k].Contains ("FieldOfView")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.fieldOfView = float.Parse (data[data.Length - 1]);
                    break;
                }
            }

            this.properties70.Add (prop70.id, prop70);
        }
        this.IsDone = true;

        //this.IsDone = false;

        //int f = UniFBX.list.FindIndex (x => x.Contains ("\"NodeAttribute::") && x.Contains ("\"Camera\""));
        //if (f == -1) {
        //    this.IsDone = true;
        //    return;
        //}
        //this.properties70 = new Dictionary<string, Properties70> ();

        //string[] data = new string[0];
        //for (int i = f; i < list.Count; i++) {
        //    if (list[i].Contains ("\"NodeAttribute::") && list[i].Contains("\"Camera\"")) {
        //        Properties70 prop70 = new Properties70 ();
        //        data = list[i].Split (new char[] { ':', ',', '\"' });
        //        prop70.id = data[1].Trim ();
        //        prop70.name = data[5].Trim ();

        //        for (int j = i; j < (i + 12); j++) {
        //            if (UniFBX.list[j].Contains ("Up")) {
        //                data = UniFBX.list[j].Split (',');
        //                prop70.up.x = float.Parse (data[data.Length - 3]);
        //                prop70.up.y = float.Parse (data[data.Length - 2]);
        //                prop70.up.z = float.Parse (data[data.Length - 1]);
        //            }
        //            else if (UniFBX.list[j].Contains ("UpVector")) {
        //                data = UniFBX.list[j].Split (',');
        //                prop70.lookAt.x = float.Parse (data[data.Length - 3]);
        //                prop70.lookAt.y = float.Parse (data[data.Length - 2]);
        //                prop70.lookAt.z = float.Parse (data[data.Length - 1]);
        //            }
        //            else if (UniFBX.list[j].Contains ("FieldOfView")) {
        //                data = UniFBX.list[j].Split (',');
        //                prop70.fieldOfView = float.Parse (data[data.Length - 1]);
        //                break;
        //            }
        //        }
        //        this.properties70.Add (prop70.id, prop70);                
        //        i = i + 12;
        //    }
        //    else if (
        //        list[i].Contains ("\"Model::") ||
        //        list[i].Contains ("\"Material::") ||
        //        list[i].Contains ("\"AnimStack::") ||                
        //        list[i].Contains ("Connections:  {")
        //        ) {                    
        //        this.IsDone = true;
        //        return;
        //    }
        //}
    }

    public Properties70 GetProperty (string nodeAttributeID) {
        if (this.properties70 == null) return null;
        if (!this.properties70.ContainsKey (nodeAttributeID)) return null;
        return this.properties70[nodeAttributeID];
    }

    public void Clear ( ) {        
        if (this.properties70 != null) this.properties70.Clear ();        
    }

}