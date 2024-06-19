using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXLayeredTextures {

    public class Properties70 {
        public string id;
        public string name;
        public string property;
        public Material material = null;     
    }

    private FBXSetting setting;
    private Dictionary<string, Properties70> properties70 = null;
    private Dictionary<string, Material> materials = null;
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
        this.layeredTextureMaterials = new Dictionary<string, string> ();
        var indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => UniFBX.list[x].Contains ("\"LayeredTexture::")).ToList ();
        string[] data = new string[0];
        for (int i = 0; i < indexes.Count; i++) {
            Properties70 prop70 = new Properties70 ();
            var j = indexes[i];
            data = UniFBX.list[j].Split (new char[] { ':', ',', '\"' });
            prop70.id = data[1].Trim ();
            prop70.name = data[5].Trim ();
            prop70.property = data[6].Trim ();

            this.properties70.Add (prop70.id, prop70);
        }
        this.IsDone = true;
    }

    public void CreateMaterial (string layeredTextureID, UniFBXMaterials.Properties70 mProp70, string textureName) {
        if (this.properties70[layeredTextureID].material == null) {            
#if UNITY_5
            this.properties70[layeredTextureID].material = new Material (Shader.Find ("Standard"));
#else
            this.properties70[layeredTextureID].material = new Material (Shader.Find ("Diffuse"));            
#endif
            this.properties70[layeredTextureID].material.name = STR_TEXTURES;
            if (!this.materials.ContainsKey (mProp70.id)) this.materials.Add (mProp70.id, this.properties70[layeredTextureID].material);
            if (!this.layeredTextureMaterials.ContainsKey (layeredTextureID)) this.layeredTextureMaterials.Add (layeredTextureID, mProp70.id);      
        }
        else {
            string property = this.properties70[layeredTextureID].property;
            if (property == "") property = this.properties70[layeredTextureID].name;
            string id = mProp70.id;
            if (!this.materials.ContainsKey (mProp70.id)) this.materials.Add (mProp70.id, this.properties70[layeredTextureID].material);

            if (property.Contains ("DiffuseColor")) {
                if (this.setting.textures.colormaps) this.materials[id].name = this.materials[id].name.Replace ("texDiffuseMap", textureName);
            }
            else if (property.Contains ("NormalMap")) {
                if (this.setting.textures.normalmaps) this.materials[id].name = this.materials[id].name.Replace ("texNormalMap", textureName);
            }
            if (this.setting.materials.shaderType == ShaderType.Standard) {
                if (property.Contains ("DisplacementColor")) {
                    if (this.setting.textures.heightmaps) this.materials[id].name = this.materials[id].name.Replace ("texHeightMap", textureName);
                }
                else if (property.Contains ("OcclusionColor")) {
                    if (this.setting.textures.occlusionmaps) this.materials[id].name = this.materials[id].name.Replace ("texOcclusionColorMap", textureName);
                }
                else if (property.Contains ("AmbientColor")) {
                    if (this.setting.textures.occlusionmaps) this.materials[id].name = this.materials[id].name.Replace ("texOcclusionColorMap", textureName);
                }
                else if (property.Contains ("EmissiveColor")) {
                    if (this.setting.textures.emissionmaps) this.materials[id].name = this.materials[id].name.Replace ("texEmissiveColorMap", textureName);
                }
                else if (property.Contains ("Bump")) {
                    if (this.setting.textures.detailmasks) this.materials[id].name = this.materials[id].name.Replace ("texBumpMap", textureName);
                }
            }
            
            if (!this.layeredTextureMaterials.ContainsKey (layeredTextureID)) this.layeredTextureMaterials.Add (layeredTextureID, mProp70.id); 
        }  
    }

    public void CreateTexture (string layeredTextureID, UniFBXTextures.Properties70 prop70) {
        if (this.properties70[layeredTextureID].material == null) {
#if UNITY_5
            this.properties70[layeredTextureID].material = new Material (Shader.Find ("Standard"));
#else
            this.properties70[layeredTextureID].material = new Material (Shader.Find ("Diffuse"));
#endif
            this.properties70[layeredTextureID].material.name = STR_TEXTURES;
        }

        string id = "";
        string property = "";        

        if (this.layeredTextureMaterials.ContainsKey (layeredTextureID)) {
            id = this.layeredTextureMaterials[layeredTextureID];
            property = this.properties70[layeredTextureID].property;
            if (property == "") property = this.properties70[layeredTextureID].name;
            
        }
        else {
            if (!this.materials.ContainsKey (layeredTextureID)) {
                this.materials.Add (layeredTextureID, this.properties70[layeredTextureID].material);
            }
            property = this.properties70[layeredTextureID].property;
            id = layeredTextureID;
            if (!this.materials.ContainsKey (layeredTextureID)) this.materials.Add (layeredTextureID, this.properties70[layeredTextureID].material);            
        }

        
        if (property.Contains ("DiffuseColor")) {
            if (this.setting.textures.colormaps) this.materials[id].name = this.materials[id].name.Replace ("texDiffuseMap", prop70.filename);
        }
        else if (property.Contains ("NormalMap")) {
            if (this.setting.textures.normalmaps) this.materials[id].name = this.materials[id].name.Replace ("texNormalMap", prop70.filename);
        }
        if (this.setting.materials.shaderType == ShaderType.Standard) {
            if (property.Contains ("DisplacementColor")) {
                if (this.setting.textures.heightmaps) this.materials[id].name = this.materials[id].name.Replace ("texHeightMap", prop70.filename);
            }
            else if (property.Contains ("OcclusionColor")) {
                if (this.setting.textures.occlusionmaps) this.materials[id].name = this.materials[id].name.Replace ("texOcclusionColorMap", prop70.filename);
            }
            else if (property.Contains ("AmbientColor")) {
                if (this.setting.textures.occlusionmaps) this.materials[id].name = this.materials[id].name.Replace ("texOcclusionColorMap", prop70.filename);
            }
            else if (property.Contains ("EmissiveColor")) {
                if (this.setting.textures.emissionmaps) this.materials[id].name = this.materials[id].name.Replace ("texEmissiveColorMap", prop70.filename);
            }
            else if (property.Contains ("Bump")) {
                if (this.setting.textures.detailmasks) this.materials[id].name = this.materials[id].name.Replace ("texBumpMap", prop70.filename);
            }
        }        

        this.properties70[layeredTextureID].material.SetTextureScale ("_MainTex", prop70.scaling);
        this.properties70[layeredTextureID].material.SetTextureOffset ("_MainTex", prop70.offset);
    }

    public string GetTextures (string materialID, UniFBXTextures utextures) {
        if (this.materials == null) return null;

        if (this.materials.ContainsKey (materialID)) {
            return this.materials[materialID].name;
        } {
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
        if (this.properties70 != null) this.properties70.Clear (); ;
        if (this.layeredTextureMaterials != null) this.layeredTextureMaterials.Clear ();        
        this.properties70 = null;        
        this.layeredTextureMaterials = null;
    }

}