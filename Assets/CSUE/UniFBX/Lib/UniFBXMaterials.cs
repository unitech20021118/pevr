using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXMaterials {

    public class Properties70 {
        public string id;
        public string name;
        public Color diffuseColor = Color.white;
        public Color emissiveColor = Color.black;
        public Material material;
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

        var indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("\"Material::")).ToList ();
        string[] data = new string[0];
        for (int i = 0; i < indexes.Count; i++) {
            Properties70 prop70 = new Properties70 ();
            var j = indexes[i];
            data = UniFBX.list[j].Split (new char[] { ':', ',', '\"' });
            prop70.id = data[1].Trim ();
            prop70.name = data[5].Trim ();

            for (int k = j; k < (j + 10); k++) {
                if (UniFBX.list[k].Contains ("\"EmissiveColor\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.emissiveColor.r = float.Parse (data[data.Length - 3]);
                    prop70.emissiveColor.g = float.Parse (data[data.Length - 2]);
                    prop70.emissiveColor.b = float.Parse (data[data.Length - 1]);
                    prop70.emissiveColor.a = 1.0f;
                }
                else if (UniFBX.list[k].Contains ("\"DiffuseColor\"")) {
                    data = UniFBX.list[k].Split (',');
                    prop70.diffuseColor.r = float.Parse (data[data.Length - 3]);
                    prop70.diffuseColor.g = float.Parse (data[data.Length - 2]);
                    prop70.diffuseColor.b = float.Parse (data[data.Length - 1]);
                    prop70.diffuseColor.a = 1.0f;
                    break;
                }
            }

            this.properties70.Add (prop70.id, prop70);
            UniFBXStads.AddMaterial ();
        }
        this.IsDone = true;
    }

    public Properties70 GetProperty (string materialID) {
        return this.properties70[materialID];
    }

    public string GetName (string materialID) {
        return this.properties70[materialID].name;
    }

    public Color GetColor (string materialID) {
        return this.properties70[materialID].diffuseColor;
    }
    
    public Color GetEmissiveColor (string materialID) {
        return this.properties70[materialID].emissiveColor;
    }

    public void Clear ( ) {        
        if (this.properties70 != null) this.properties70.Clear ();        
        this.properties70 = null;        
    }

}