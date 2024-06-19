using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXTextures {

    [System.Serializable]
    public class Properties70 {
        public string id;
        public string filename;
        public Material material = null;
        public Vector2 scaling = Vector2.one;
        public Vector2 offset = Vector2.zero;
        public string property;
        public byte[] bytes;
    }

    private FBXSetting setting;
    private Dictionary<string, Properties70> properties70 = null;
    private Dictionary<string, byte[]> embeddedTextures = null;
    private Dictionary<string, Material> materials = null;
    private Dictionary<string, string> textureMaterials = null;
    private Dictionary<string, string> layeredTextureMaterials = null;
    private const string STR_TEXTURES =
            "DiffuseColor,texDiffuseMap," +
            "NormalMap,texNormalMap," +
            "DisplacementColor,texHeightMap," +
            "OcclusionColor,texOcclusionColorMap," +
            "EmissiveColor,texEmissiveColorMap," +
            "Bump,texBumpMap";

    private bool _isDone = false;
    public bool IsDone {
        get { return this._isDone; }
        set { this._isDone = value; }
    }

    private bool _isEmbedded = false;
    public bool IsEmbedded {
        get { return this._isEmbedded; }
        set { this._isEmbedded = value; }
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
        this.materials = new Dictionary<string, Material> ();
        this.textureMaterials = new Dictionary<string, string> ();
        this.layeredTextureMaterials = new Dictionary<string, string> ();

        var indexesTextures = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("\"Texture::")).ToList ();

        string[] data = new string[0];

        for (int i = 0; i < indexesTextures.Count; i++) {
            Properties70 prop70 = new Properties70 ();
            var j = indexesTextures[i];
            data = UniFBX.list[j].Split (new char[] { ':', ',', '\"' });
            prop70.id = data[1].Trim ();
            prop70.filename = data[5].Trim ();

            Vector2 v = Vector2.zero;
            for (int k = j; k < (j + 16); k++) {
                if (UniFBX.list[k].Contains ("\"Translation\"")) {
                    data = UniFBX.list[k].Split (',');
                    v.x = float.Parse (data[data.Length - 3]);
                    v.y = float.Parse (data[data.Length - 2]);
                    prop70.offset = v;
                }
                else if (UniFBX.list[k].Contains ("\"Scaling\"")) {
                    data = UniFBX.list[k].Split (',');
                    v.x = float.Parse (data[data.Length - 3]);
                    v.y = float.Parse (data[data.Length - 2]);
                    prop70.scaling = v;
                }
                else if (UniFBX.list[k].Contains ("FileName: ")) {
                    data = UniFBX.list[k].Split (':');
                    data = data[data.Length -1].Split ('\\');
                    prop70.filename = data[data.Length - 1].Replace ("\"", "").Trim ();
                }
            }

            if (!this.properties70.ContainsKey (prop70.id))
                this.properties70.Add (prop70.id, prop70);
        }

        var indexesVideos = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("Content: ,")).ToList ();
        if (indexesVideos.Count > 0) {
            this.embeddedTextures = new Dictionary<string, byte[]> ();
            for (int i = 0; i < indexesVideos.Count; i++) {
                int j = indexesVideos[i];
                data = UniFBX.list[j - 1].Split (new char[] { '\"', '\\' });
                string filename = data[data.Length - 2];
                if (!this.embeddedTextures.ContainsKey (filename)) {
                    string s = "";
                    List<string> str = new List<string> ();
                    j = j + 1;
                    while (!UniFBX.list[j].Contains ("}")) {
                        s += UniFBX.list[j].Trim ();
                        j++;
                    }
                    if (s != "") {
                        s = s.Replace ("\"", "");
                        s = s.Replace (",", "");
                        this.embeddedTextures.Add (filename, System.Convert.FromBase64String (s));                        
                    }
                }
            }
            this.IsEmbedded = true;
#if UNITY_EDITOR
            Debug.Log ("Embedded textures!");
#endif
        }
        else {
            this.IsEmbedded = false;
        }
        
        this.IsDone = true;       
    }

    public Properties70 GetProperty (string textureID) {
        return this.properties70[textureID];
    }

    public void CreateMaterial (string textureID, string materialID) {
        if (this.properties70[textureID].material == null) {
            this.properties70[textureID].material = new Material (Shader.Find ("Standard"));
            string str = STR_TEXTURES;            
            this.properties70[textureID].material.name = str.Replace ("texDiffuseMap", this.properties70[textureID].filename);            

            this.properties70[textureID].material.mainTextureScale = this.properties70[textureID].scaling;
            this.properties70[textureID].material.mainTextureOffset = this.properties70[textureID].offset;
        }

        if (!this.materials.ContainsKey (materialID)) {
            this.materials.Add (materialID, this.properties70[textureID].material);
        }
        if (!this.textureMaterials.ContainsKey (textureID)) this.textureMaterials.Add (textureID, materialID);
    }

    public void CreateTexture (string layeredTextureID, string textureID) {
        if (!this.layeredTextureMaterials.ContainsKey (layeredTextureID)) {
            this.layeredTextureMaterials.Add (layeredTextureID, textureID);
        }
    }

    public string GetTexture (string layeredTextureID) {
        if (!this.layeredTextureMaterials.ContainsKey (layeredTextureID)) return null;
        string id = this.layeredTextureMaterials[layeredTextureID];
        return this.properties70[id].filename;
    }

    public string GetTextures (string materialID) {
        if (this.materials == null) return null;

        if (this.materials.ContainsKey (materialID)) {
            return this.materials[materialID].name;
        }
        else {
            return null;
        }
        
    }

    public byte[] GetEmbeddedTexture (string filename) {
        if (this.embeddedTextures.ContainsKey (filename)) {
            return this.embeddedTextures[filename];
        }
        else {
            return null;
        }
    }

    public void SetScaleOffset (string materialID, Material material) {
        if (this.materials == null) return;
        if (this.materials.ContainsKey (materialID)) {
            Vector2 s = this.materials[materialID].GetTextureScale ("_MainTex");
            Vector2 o = this.materials[materialID].GetTextureOffset ("_MainTex");
            material.SetTextureScale ("_MainTex", s);
            material.SetTextureOffset ("_MainTex", o);
            if (setting.materials.shaderType == ShaderType.Legacy) {
                if (setting.textures.normalmaps) {
                    material.SetTextureScale ("_BumpMap", s);
                    material.SetTextureOffset ("_BumpMap", o);
                }
            }
        }
    }

    public void Clear ( ) {        
        if (this.properties70 != null) this.properties70.Clear ();
        if (this.textureMaterials != null) this.textureMaterials.Clear ();
        if (this.layeredTextureMaterials != null) this.layeredTextureMaterials.Clear ();        
        this.properties70 = null;
        this.textureMaterials = null;
        this.layeredTextureMaterials = null;
    }

}