using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSUE.UniFBX;

public class UniFBXTexture2D : MonoBehaviour {

    private List<Material> listMaterials = null;
    private List<Material> listEmbeddedMaterials = null;
    private List<string> listTextures = null;
    private List<string> listEmbeddedTextures = null;
    private Dictionary<string, Texture2D> textures = null;
    private FBXSetting setting;
    private int Count = 0;
    private bool normalMapDone = false;
    private bool otherMapDone = false;

    public void Init (FBXSetting setting) {
        this.setting = setting;
        if (this.listMaterials == null) this.listMaterials = new List<Material> ();
        if (this.listTextures == null) this.listTextures = new List<string> ();
        if (this.listEmbeddedMaterials == null) this.listEmbeddedMaterials = new List<Material> ();
        if (this.listEmbeddedTextures == null) this.listEmbeddedTextures = new List<string> ();
        if (this.textures == null) this.textures = new Dictionary<string, Texture2D> ();
    }

    private IEnumerator WaitForApplyAllTextures ( ) {
        while (Count < this.listMaterials.Count) yield return null;
        yield return null;

        if (setting.textures.normalmaps) {
            while (normalMapDone == false) yield return null;
        }

        if (!setting.textures.heightmaps || !setting.textures.occlusionmaps ||
            !setting.textures.emissionmaps || !setting.textures.detailmasks) {
            while (otherMapDone == false) yield return null;
        }

        yield return new WaitForSeconds (0.1f);
        if (this.listMaterials != null) this.listMaterials.Clear ();
        if (this.listEmbeddedMaterials != null) this.listEmbeddedMaterials.Clear ();
        if (this.listTextures != null) this.listTextures.Clear ();
        if (this.listEmbeddedTextures != null) this.listEmbeddedTextures.Clear ();
        if (this.textures != null) this.textures.Clear ();

        this.listMaterials = null;
        this.listEmbeddedMaterials = null;
        this.listTextures = null;
        this.listEmbeddedTextures = null;
        this.textures = null;

        DestroyImmediate (this);
    }

    private void Finish ( ) {        
        this.listMaterials.Clear ();
        this.listEmbeddedMaterials.Clear ();
        this.listTextures.Clear ();
        this.listEmbeddedTextures.Clear ();

        this.textures.Clear ();

        this.listMaterials = null;
        this.listEmbeddedMaterials = null;
        this.listTextures = null;
        this.listEmbeddedTextures = null;

        this.textures = null;   
     
        DestroyImmediate (this);
    }

    #region "Folder Textures"

    public void Apply ( ) {
        //StartCoroutine (this.WaitForApplyAllTextures ());

        for (int i = 0; i < listMaterials.Count; i++) {           
            string[] data = this.listTextures[i].Split (',');
            normalMapDone = false;            

            //Colormap
            if (setting.textures.colormaps) {
                if (data[1] != "texDiffuseMap") StartCoroutine (this.IColormap (data[1], this.listMaterials[i]));
            }

            //Normalmap
            if (setting.textures.normalmaps) {
                if (data[3] != "texNormalMap") StartCoroutine (this.INormalmap (data[3], this.listMaterials[i]));
            }

            //If legacy this.listMaterials[i]s don't read more texture channels
            if (setting.materials.shaderType == ShaderType.Legacy) continue;

            //Heightmap
            if (setting.textures.heightmaps) {
                if (data[5] != "texHeightMap") StartCoroutine (this.ITexturemap (data[5], this.listMaterials[i], "_ParallaxMap"));
            }

            //Oclussionmap
            if (setting.textures.occlusionmaps) {
                if (data[7] != "texOcclusionColorMap") StartCoroutine (this.ITexturemap (data[7], this.listMaterials[i], "_OcclusionMap"));
            }

            //Emissionmap
            if (setting.textures.emissionmaps) {
                if (data[9] != "texEmissiveColorMap") StartCoroutine (this.ITexturemap (data[9], this.listMaterials[i], "_EmissionMap"));
            }

            //Detailmask
            if (setting.textures.detailmasks) {
                if (data[11] != "texBumpMap") StartCoroutine (this.ITexturemap (data[11], this.listMaterials[i], "_DetailMask"));
            }
        }
    }

    public void SetTexture (Material material, string textureNames) {
        if (textureNames == null) return;
        this.listMaterials.Add (material);
        this.listTextures.Add (textureNames);
    }       

    IEnumerator IColormap (string filename, Material material ) {
        string path = setting.paths.urlTextures + filename;

        using (WWW www = new WWW (path)) {
            while (!www.isDone) yield return null;

            if (www.error == null) {
                if (!textures.ContainsKey (filename)) {
                    string ext = filename.ToUpper ();
                    if (ext.Contains (".DDS")) {
                        this.LoadTextures_DDS (filename, www.bytes, material, "_MainTex");
                    }
                    else if (ext.Contains (".TGA")) {
                        this.LoadTextures_TGA (filename, www.bytes, material, "_MainTex");
                    }
                    else if (ext.Contains (".JPG") || ext.Contains ("JPEG")) {
                        this.LoadTextures_JPG (filename, www.bytes, material, "_MainTex");
                    }
                    else if (ext.Contains (".PNG")) {
                        this.LoadTextures_PNG (filename, www.bytes, material, "_MainTex");
                    }
                    else {
                        this.LoadTextures_Default (filename, www.bytes, material, "_MainTex");
                    }
                }
                else {
                    material.SetTexture ("_MainTex", textures[filename]);
                }
            }
        }
        Count++;
    }

    IEnumerator INormalmap (string filename, Material material) {
        string path = setting.paths.urlTextures + filename;
        using (WWW www = new WWW (path)) {
            while (!www.isDone) yield return null;

            if (www.error == null) {
                if (!textures.ContainsKey (filename)) {
                    string ext = filename.ToUpper ();
                    if (ext.Contains (".DDS")) {
                        this.LoadTextures_DDS (filename, www.bytes, material, "_BumpMap");
                    }
                    else if (ext.Contains (".TGA")) {
                        this.LoadTextures_TGA (filename, www.bytes, material, "_BumpMap");
                    }
                    else if (ext.Contains (".JPG") || ext.Contains ("JPEG")) {
                        this.LoadTextures_JPG (filename, www.bytes, material, "_BumpMap");
                    }
                    else if (ext.Contains (".PNG")) {
                        this.LoadTextures_PNG (filename, www.bytes, material, "_BumpMap");
                    }
                    else {
                        this.LoadTextures_Default (filename, www.bytes, material, "_BumpMap");
                    }
                }
                else {
                    material.SetTexture ("_BumpMap", textures[filename]);
                }
            }
        }
        normalMapDone = true;
    }

    IEnumerator ITexturemap (string filename, Material material, string channelMap) {
        otherMapDone = false;
        string path = setting.paths.urlTextures + filename;
        using (WWW www = new WWW (path)) {
            while (!www.isDone) yield return null;

            if (www.error == null) {
                if (!textures.ContainsKey (filename)) {
                    string ext = filename.ToUpper ();
                    if (ext.Contains (".DDS")) {
                        this.LoadTextures_DDS (filename, www.bytes, material, channelMap);
                    }
                    else if (ext.Contains (".TGA")) {
                        this.LoadTextures_TGA (filename, www.bytes, material, channelMap);
                    }
                    else if (ext.Contains (".JPG") || ext.Contains ("JPEG")) {
                        this.LoadTextures_JPG (filename, www.bytes, material, channelMap);
                    }
                    else if (ext.Contains (".PNG")) {
                        this.LoadTextures_PNG (filename, www.bytes, material, channelMap);
                    }
                    else {
                        this.LoadTextures_Default (filename, www.bytes, material, channelMap);
                    }
                }
                else {
                    material.SetTexture (channelMap, textures[filename]);
                }
            }
        }
        otherMapDone = true;
    }

    #endregion

    #region "Embedded Textures"

    public void Apply2 (UniFBXTextures utextures) {
        for (int i = 0; i < listEmbeddedMaterials.Count; i++) {
            string[] data = this.listEmbeddedTextures[i].Split (',');

            //Colormap
            if (setting.textures.colormaps) {
                if (data[1] != "texDiffuseMap") StartCoroutine (this.IColormap2 (data[1], utextures, this.listEmbeddedMaterials[i]));
            }

            //Normalmap
            if (setting.textures.normalmaps) {
                if (data[3] != "texNormalMap") StartCoroutine (this.INormalmap2 (data[3], utextures, this.listEmbeddedMaterials[i]));
            }

            //If legacy listEmbeddedMaterialss don't read more texture channels
            if (setting.materials.shaderType == ShaderType.Legacy) continue;

            //Heightmap
            if (setting.textures.heightmaps) {
                if (data[5] != "texHeightMap") StartCoroutine (this.ITexturemap2 (data[5], utextures, this.listEmbeddedMaterials[i], "_ParallaxMap"));
            }

            //Oclussionmap
            if (setting.textures.occlusionmaps) {
                if (data[7] != "texOcclusionColorMap") StartCoroutine (this.ITexturemap2 (data[7], utextures, this.listEmbeddedMaterials[i], "_OcclusionMap"));
            }

            //Emissionmap
            if (setting.textures.emissionmaps) {
                if (data[9] != "texEmissiveColorMap") StartCoroutine (this.ITexturemap2 (data[9], utextures, this.listEmbeddedMaterials[i], "_EmissionMap"));
            }

            //Detailmask
            if (setting.textures.detailmasks) {
                if (data[11] != "texBumpMap") StartCoroutine (this.ITexturemap2 (data[11], utextures, this.listEmbeddedMaterials[i], "_DetailMask"));
            }
        }
    }

    public void SetTexture (Material material, UniFBXTextures utextures, string textureNames) {
        if (textureNames == null) return;        
        this.listEmbeddedMaterials.Add (material);
        this.listEmbeddedTextures.Add (textureNames);
    }

    IEnumerator IColormap2 (string filename, UniFBXTextures utextures, Material material) {
        byte[] bytes = utextures.GetEmbeddedTexture (filename);
        yield return null;

        if (!textures.ContainsKey (filename)) {
            string ext = filename.ToUpper ();
            if (ext.Contains (".DDS")) {
                this.LoadTextures_DDS (filename, bytes, material, "_MainTex");
            }
            else if (ext.Contains (".TGA")) {
                this.LoadTextures_TGA (filename, bytes, material, "_MainTex");
            }
            else if (ext.Contains (".JPG") || ext.Contains ("JPEG")) {
                this.LoadTextures_JPG (filename, bytes, material, "_MainTex");
            }
            else if (ext.Contains (".PNG")) {
                this.LoadTextures_PNG (filename, bytes, material, "_MainTex");
            }
            else {
                this.LoadTextures_Default (filename, bytes, material, "_MainTex");
            }
        }
        else {
            material.SetTexture ("_MainTex", textures[filename]);
        }
    }

    IEnumerator INormalmap2 (string filename, UniFBXTextures utextures, Material material) {
        byte[] bytes = utextures.GetEmbeddedTexture (filename);
        yield return null;

        if (!textures.ContainsKey (filename)) {
            if (filename.ToUpper ().Contains (".DDS")) {
                string ext = filename.ToUpper ();
                if (ext.Contains (".DDS")) {
                    this.LoadTextures_DDS (filename, bytes, material, "_BumpMap");
                }
                else if (ext.Contains (".TGA")) {
                    this.LoadTextures_TGA (filename, bytes, material, "_BumpMap");
                }
                else if (ext.Contains (".JPG") || ext.Contains ("JPEG")) {
                    this.LoadTextures_JPG (filename, bytes, material, "_BumpMap");
                }
                else if (ext.Contains (".PNG")) {
                    this.LoadTextures_PNG (filename, bytes, material, "_BumpMap");
                }
                else {
                    this.LoadTextures_Default (filename, bytes, material, "_BumpMap");
                }
            }
        }
        else {
            material.SetTexture ("_BumpMap", textures[filename]);
        }
    }

    IEnumerator ITexturemap2 (string filename, UniFBXTextures utextures, Material material, string channelMap) {
        byte[] bytes = utextures.GetEmbeddedTexture (filename);
        yield return null;

        if (!textures.ContainsKey (filename)) {
            string ext = filename.ToUpper ();
            if (ext.Contains (".DDS")) {
                this.LoadTextures_DDS (filename, bytes, material, channelMap);
            }
            else if (ext.Contains (".TGA")) {
                this.LoadTextures_TGA (filename, bytes, material, channelMap);
            }
            else if (ext.Contains (".JPG") || ext.Contains ("JPEG")) {
                this.LoadTextures_JPG (filename, bytes, material, channelMap);
            }
            else if (ext.Contains (".PNG")) {
                this.LoadTextures_PNG (filename, bytes, material, channelMap);
            }
            else {
                this.LoadTextures_Default (filename, bytes, material, channelMap);
            }
        }
        else {
            material.SetTexture (channelMap, textures[filename]);
        }
    }

    #endregion

    #region "Texture Formats"

    private void LoadTextures_Default (string filename, byte[] bytes, Material material, string channel) {
        var tex = new Texture2D (32, 32, TextureFormat.ARGB32, true);
        tex.LoadImage (bytes);
        tex.name = filename;
        tex.Compress (false);
        textures.Add (filename, tex);
        material.SetTexture (channel, tex);
        UniFBXStads.AddTexture ();
    }

    private void LoadTextures_JPG (string filename, byte[] bytes, Material material, string channel) {
        var tex = new Texture2D (32, 32, TextureFormat.RGB24, true);
        tex.LoadImage (bytes);

        if (channel == "_BumpMap") {
            var nm = new Texture2D (tex.width, tex.height, TextureFormat.RGB24, true);
            Color c = new Color ();
            for (int x = 0; x < tex.width; x++) {
                for (int y = 0; y < tex.height; y++) {
                    c.r = tex.GetPixel (x, y).g;
                    c.g = c.r;
                    c.b = c.r;
                    //c.a = tex.GetPixel (x, y).r;
                    nm.SetPixel (x, y, c);
                }
            }
            nm.Apply ();
            nm.name = filename;
            nm.Compress (false);
            textures.Add (filename, nm);
            material.SetTexture (channel, nm);
            UniFBXStads.AddTexture ();
        }
        else {
            tex.name = filename;
            tex.Compress (false);
            textures.Add (filename, tex);
            material.SetTexture (channel, tex);
            UniFBXStads.AddTexture ();

            #region "Transparence"
            if (setting.materials.shaderType == ShaderType.Standard) {
                material.SetFloat ("_Mode", 0.0f);
            }
            else if (setting.materials.shaderType == ShaderType.Legacy) {
#if UNITY_5
                material.shader = Shader.Find ("Legacy Shaders/Transparent/Diffuse");
#else
                        material.shader = Shader.Find ("Transparent/Diffuse");
#endif
            }
            #endregion        
        }
        System.Array.Clear (bytes, 0, bytes.Length - 1);
        bytes = null;
    }

    private void LoadTextures_PNG (string filename, byte[] bytes, Material material, string channel) {
        var tex = new Texture2D (32, 32, TextureFormat.ARGB32, true);
        tex.LoadImage (bytes);

        if (channel == "_BumpMap") {
            var nm = new Texture2D (tex.width, tex.height, TextureFormat.ARGB32, true);
            Color c = new Color ();
            for (int x = 0; x < tex.width; x++) {
                for (int y = 0; y < tex.height; y++) {
                    c.r = tex.GetPixel (x, y).g;
                    c.g = c.r;
                    c.b = c.r;
                    c.a = tex.GetPixel (x, y).r;
                    nm.SetPixel (x, y, c);
                }
            }
            nm.Apply ();
            nm.name = filename;
            nm.Compress (false);
            textures.Add (filename, nm);
            material.SetTexture (channel, nm);
            UniFBXStads.AddTexture ();
        }
        else {
            tex.name = filename;
            tex.Compress (false);
            textures.Add (filename, tex);
            material.SetTexture (channel, tex);
            UniFBXStads.AddTexture ();

            #region "Transparence"
            if (setting.materials.shaderType == ShaderType.Legacy) {
#if UNITY_5
                material.shader = Shader.Find ("Legacy Shaders/Transparent/Diffuse");
#else
                        material.shader = Shader.Find ("Transparent/Diffuse");
#endif
            }
            #endregion        
        }
        System.Array.Clear (bytes, 0, bytes.Length - 1);
        bytes = null;
    }

    private void LoadTextures_DDS (string filename, byte[] bytes, Material material, string channel) {
        Texture2D tex = null;
        byte ddsSizeCheck = bytes[4];
        if (ddsSizeCheck != 124) {
            Debug.LogWarning ("Invalid DDS DXTn texture. Unable to read");
            tex = new Texture2D (16, 16);
        }

        int height = bytes[13] * 256 + bytes[12];
        int width = bytes[17] * 256 + bytes[16];
        int depth = bytes[87];

        int DDS_HEADER_SIZE = 128;
        byte[] dxtBytes = new byte[bytes.Length - DDS_HEADER_SIZE];
        System.Buffer.BlockCopy (bytes, DDS_HEADER_SIZE, dxtBytes, 0, bytes.Length - DDS_HEADER_SIZE);

        if (depth == 53) {
            tex = new Texture2D (width, height, TextureFormat.DXT5, false);
        }
        else {
            tex = new Texture2D (width, height, TextureFormat.DXT1, false);
        }
        tex.LoadRawTextureData (dxtBytes);

        if (channel == "_BumpMap") {
            var nm = new Texture2D (tex.width, tex.height, TextureFormat.ARGB32, true);
            Color c = new Color ();
            for (int x = 0; x < tex.width; x++) {
                for (int y = 0; y < tex.height; y++) {
                    c.r = tex.GetPixel (x, tex.height - y - 1).g;
                    c.g = c.r;
                    c.b = c.r;
                    c.a = tex.GetPixel (x, tex.height - y - 1).r;
                    nm.SetPixel (x, y, c);
                }
            }
            nm.Apply ();

            nm.name = filename;
            nm.Compress (false);
            textures.Add (filename, nm);
            material.SetTexture (channel, nm);
            UniFBXStads.AddTexture ();
        }
        else {
            var cm = new Texture2D (tex.width, tex.height, TextureFormat.ARGB32, true);
            Color c = new Color ();
            for (int x = 0; x < tex.width; x++) {
                for (int y = 0; y < tex.height; y++) {
                    c = tex.GetPixel (x, tex.height - y - 1);
                    cm.SetPixel (x, y, c);
                }
            }
            cm.Apply ();
            cm.name = filename;
            cm.Compress (false);
            textures.Add (filename, cm);
            material.SetTexture (channel, cm);
            UniFBXStads.AddTexture ();

            if (depth == 53) {
                #region "Transparence"
                if (setting.materials.shaderType == ShaderType.Legacy) {
#if UNITY_5
                    material.shader = Shader.Find ("Legacy Shaders/Transparent/Diffuse");
#else
                        material.shader = Shader.Find ("Transparent/Diffuse");
#endif
                }
                #endregion
            }
        }
        System.Array.Clear (bytes, 0, bytes.Length - 1);
        System.Array.Clear (dxtBytes, 0, bytes.Length - 1);
        bytes = null;
        dxtBytes = null;
    }

    private void LoadTextures_TGA (string filename, byte[] bytes, Material material, string channel) {        
        int width = 256 * bytes[13] + bytes[12];
        int height = 256 * bytes[15] + bytes[14];
        int depth = bytes[16];
        int SIZE = width * height;

        Color32[] colors = new Color32[SIZE];
        int SKIP = 18;
        Texture2D tex = null;

        if (depth == 32) {
            tex = new Texture2D (width, height, TextureFormat.ARGB32, true);
            for (int i = 0; i < SIZE; i++) {
                byte r = bytes[SKIP++];
                byte g = bytes[SKIP++];
                byte b = bytes[SKIP++];
                byte a = bytes[SKIP++];
                colors[i] = new Color32 (b, g, r, a);
            }
        }
        else if (depth == 24) {
            tex = new Texture2D (width, height, TextureFormat.RGB24, true);
            for (int i = 0; i < SIZE; i++) {
                byte r = bytes[SKIP++];
                byte g = bytes[SKIP++];
                byte b = bytes[SKIP++];
                colors[i] = new Color32 (b, g, r, 255);
            }
        }
        else {
            Debug.Log ("TGA texture don't have 32 or 24 bit depth");
        }
        tex.SetPixels32 (colors);
        tex.Apply ();

        if (channel == "_BumpMap") {
            var nm = new Texture2D (tex.width, tex.height, TextureFormat.ARGB32, true);
            Color c = new Color ();
            for (int x = 0; x < tex.width; x++) {
                for (int y = 0; y < tex.height; y++) {
                    c.r = tex.GetPixel (x, y).g;
                    c.g = c.r;
                    c.b = c.r;
                    c.a = tex.GetPixel (x, y).r;
                    nm.SetPixel (x, y, c);
                }
            }
            nm.Apply ();
            nm.name = filename;
            nm.Compress (false);
            textures.Add (filename, nm);
            material.SetTexture (channel, nm);
            UniFBXStads.AddTexture ();
        }
        else {
            tex.name = filename;
            tex.Compress (false);
            textures.Add (filename, tex);
            material.SetTexture (channel, tex);
            UniFBXStads.AddTexture ();

            if (depth == 32) {
                #region "Transparence"
                if (setting.materials.shaderType == ShaderType.Legacy) {
#if UNITY_5
                    material.shader = Shader.Find ("Legacy Shaders/Transparent/Diffuse");
#else
                        material.shader = Shader.Find ("Transparent/Diffuse");
#endif
                }
                #endregion        
            }
        }

        System.Array.Clear (bytes, 0, bytes.Length - 1);
        bytes = null;
    }

    #endregion

}