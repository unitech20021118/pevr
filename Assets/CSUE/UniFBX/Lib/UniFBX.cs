using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using CSUE.UniFBX;
using CSUE.UniFBX.Extensions;

public partial class UniFBX : IDisposable {

    private int foo = -1;
    private int geometryIndex = 0;
    private string content = null;
    private Transform tr;
    private FBXSetting setting;
    private FBX fbxsdk = FBX.SDKNone;
    private Dictionary<string, GameObject> models = null;
    private List<UniFBXMaterials.Properties70> materialProp70 = null;

    private UniFBXGeometries ugeometries1;
    private UniFBXGeometries ugeometries2;
    private UniFBXGeometries ugeometries3;
    private UniFBXModel umodels;
    private UniFBXMaterials umaterials;
    private UniFBXTextures utextures;
    private UniFBXAnimations uanimations;
    private UniFBXLayeredTextures ulayeredTextures;
    private UniFBXLights ulights;
    private UniFBXCameras ucameras;
    public static List<string> list = null;
    public static Dictionary<int, Mesh> meshes = null;

    private int connectionListCount = 0;
    private bool _isRunning = true;
    public bool IsRunning {
        get { return this._isRunning; }
        set { this._isRunning = value; }
    }

    private static bool _isDone = false;
    public static bool IsDone {
        get { return UniFBX._isDone; }
        set { UniFBX._isDone = value; }
    }

    #region Disposable

    private Boolean disposed;

    public UniFBX ( ) {

    }

    public UniFBX (FBXSetting setting, Transform tr) {
        this.setting = setting;
        this.tr = tr;
        this.tr.position = tr.position;
        this.tr.rotation = tr.rotation;
        this.tr.localScale = tr.localScale;
        this.setting.Status = FBXStatus.Loading;
    }

    public UniFBX (string text, FBXSetting setting, Transform tr) {
        this.content = text;
        this.setting = setting;
        this.tr = tr;
        this.tr.position = tr.position;
        this.tr.rotation = tr.rotation;
        this.tr.localScale = tr.localScale;
        this.setting.Status = FBXStatus.Loading;
    }

    ~UniFBX ( ) {
        this.Dispose (false);
    }

    public void Dispose ( ) {
        this.content = null;
        if (UniFBX.list != null) UniFBX.list.Clear ();
        if (UniFBX.meshes != null) UniFBX.meshes.Clear ();
        if (this.models != null) this.models.Clear ();
        if (this.materialProp70 != null) this.materialProp70.Clear ();
        if (this.uanimations != null) this.uanimations.Finish ();
        UniFBX.list = null;
        UniFBX.meshes = null;
        this.models = null;
        this.materialProp70 = null;
        this.ulayeredTextures = null;
        this.ugeometries1 = null;
        this.ugeometries2 = null;
        this.ugeometries3 = null;
        this.umodels = null;
        this.umaterials = null;
        this.utextures = null;
        this.ulayeredTextures = null;
        this.uanimations = null;
        this.ulights = null;
        this.ucameras = null;
    }

    protected virtual void Dispose (bool disposing) {
        if (!this.disposed) {
            if (disposing) {
                this.content = null;
                if (UniFBX.list != null) UniFBX.list.Clear ();
                if (UniFBX.meshes != null) UniFBX.meshes.Clear ();
                if (this.models != null) this.models.Clear ();
                if (this.materialProp70 != null) this.materialProp70.Clear ();
                if (this.uanimations != null) this.uanimations.Finish ();
                UniFBX.list = null;
                UniFBX.meshes = null;
                this.models = null;
                this.materialProp70 = null;
                this.ulayeredTextures = null;
                this.ugeometries1 = null;
                this.ugeometries2 = null;
                this.ugeometries3 = null;
                this.umodels = null;
                this.umaterials = null;
                this.utextures = null;
                this.ulayeredTextures = null;
                this.uanimations = null;
                this.ulights = null;
                this.ucameras = null;
            }
        }
        this.disposed = true;
    }

    #endregion

    #region "File"

    private void Init ( ) {
        this.geometryIndex = 0;
        this._isRunning = true;
        UniFBX._isDone = false;
        UniFBXGeometry.Count = 0;
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
        UniFBXThread.maxThreads = this.setting.paths.maxThreads;
#endif
        list = new List<string> ();
        meshes = new Dictionary<int, Mesh> ();
        models = new Dictionary<string, GameObject> ();
        materialProp70 = new List<UniFBXMaterials.Properties70> ();
    }

    public void Clear ( ) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        if (!models.ContainsKey (rootID)) {
            this.setting.Status = FBXStatus.FileEmpty;
            return;
        }
        if (models[rootID].GetComponent<UniFBXTexture2D> ()) {
            if (utextures.IsEmbedded == true) {
                models[rootID].GetComponent<UniFBXTexture2D> ().Apply2 (utextures);
            }
            else {
                models[rootID].GetComponent<UniFBXTexture2D> ().Apply ();
            }            
        }
    }

    public void Load ( ) {
        this.Init ();
        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread: this.Read (); break;
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread: UniFBXThread.RunAsync (this.Read); break;
#endif
            default: this.
                Read (); break;
        }
    }

    public int GetListCount ( ) {
        return UniFBX.list.Count;
    }

    public int GetConnectionListCount ( ) {
        return this.connectionListCount;
    }

    public FBX GetSDK ( ) {
        return this.fbxsdk;
    }

    private void Read ( ) {
        string[] data = new string[0];

        #region "File reading"
        if (this.content != null) {
            data = this.content.Split (new char[] { '\n' });
        }
        else {
            string path = this.setting.paths.urlModels + this.setting.paths.filename + ".fbx";
            if (System.IO.File.Exists (path)) {
                this.setting.Status = FBXStatus.Loading;
                data = System.IO.File.ReadAllLines (path);
            }
            else {
                this.setting.Status = FBXStatus.FileNotFound;
                _isDone = true;
                this._isRunning = false;
                return;
            }
        }

        list.AddRange (data);
        #endregion

        #region "FBX SDK"
        this.fbxsdk = list[0].GetSDK ();
        if (this.fbxsdk == FBX.SDKNone) {
            this.setting.Status = FBXStatus.FBXSDKNotSupported;
            _isDone = true;
            this._isRunning = false;
            return;
        }
        else if (this.fbxsdk == FBX.SDKBinary) {
            this.setting.Status = FBXStatus.BinaryNotSupported;
            _isDone = true;
            this._isRunning = false;
            return;
        }
        #endregion

        foo = list.FindIndex (x => x == "Connections:  {");
        this.connectionListCount = this.GetListCount () - foo;
        this.IsRunning = false;
    }

    #endregion

    #region "Models"

    public void GetModels ( ) {
        this.umodels = new UniFBXModel ();
        this.umodels.Init (this.setting);
    }

    public bool SetModels (int i) {
        Debug.Log(i + "set");
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return false;

        if (list[n].Contains (";Model::") && list[n].Contains (", Model::")) {
            #region "Model To Model"
            string[] data = list[n].Split (',');
            string ncc = data[0].Split (':')[2];
            string npp = data[1].Split (':')[2];

            GameObject p = null;
            GameObject c = null;
            if (!models.ContainsKey (npp)) {
                p = new GameObject (npp);
                if (npp == rootID) {
                    p.name = this.setting.paths.filename;
                    p.transform.position = this.tr.position;
                    p.transform.rotation = this.tr.rotation;
                    p.transform.localScale = this.tr.localScale;
                }
                models.Add (npp, p);
                UniFBXStads.AddObject ();
            }
            else {
                p = models[npp];
            }
            if (!models.ContainsKey (ncc)) {
                c = new GameObject (ncc);
                c.transform.parent = models[npp].transform;
                models.Add (ncc, c);
                UniFBXStads.AddObject ();
            }
            else {
                c = models[ncc];
                c.transform.parent = models[npp].transform;
            }
            #endregion
        }

        return true;
    }

    public int GetModelCount ( ) {
        return (this.models.Count - 1);
    }

    public void SetLclTransform (int i) {
        
        string id = this.umodels.GetName (i);// nameObjects[i];
        Debug.Log(i + "get" + id);
        this.models[id].transform.localPosition = this.umodels.GetPosition (i);// this.LclTransform[i].LclPosition;
        this.models[id].transform.localRotation = this.umodels.GetRotation (i);// this.LclTransform[i].LclRotation;
        this.models[id].transform.localScale = this.umodels.GetScale (i);// this.LclTransform[i].LclScaling;
    }

    public bool IsLclTransformReady ( ) {
        return this.umodels.IsDone;
    }

    public void ClearModels ( ) {
        if (umodels != null) {
            umodels.Clear ();
        }
    }
    #endregion

    #region "Geometries"


    public void GetGeometries ( ) {
        if (this.fbxsdk == FBX.SDK2010) {
            UniFBXGeometry.indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => list[x].Contains (", \"Mesh\" {")).ToList ();
        }
        else {
            UniFBXGeometry.indexes = Enumerable.Range (0, UniFBX.list.Count).Where (x => list[x].Contains ("\"Geometry::") && list[x].Contains ("Mesh")).ToList ();
        }

        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread:
            ugeometries1 = new UniFBXGeometries ();
            Debug.Log(this.setting + "**********************");
            ugeometries1.Init (this.setting, 1);
            break;

#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread:
            if (this.setting.paths.maxThreads > 3) {
                ugeometries1 = new UniFBXGeometries ();
                ugeometries1.Init (this.setting, 1);
                ugeometries2 = new UniFBXGeometries ();
                ugeometries2.Init (this.setting, 2);
                ugeometries3 = new UniFBXGeometries ();
                ugeometries3.Init (this.setting, 3);
            }
            else if (this.setting.paths.maxThreads == 3) {
                ugeometries1 = new UniFBXGeometries ();
                ugeometries1.Init (this.setting, 1);
                ugeometries2 = new UniFBXGeometries ();
                ugeometries2.Init (this.setting, 2);
            }
            else {
                ugeometries1 = new UniFBXGeometries ();
                ugeometries1.Init (this.setting, 1);
            }

            break;
#endif

            default:
            ugeometries1 = new UniFBXGeometries ();
            ugeometries1.Init (this.setting, 1);
            break;
        }

    }

    public byte SetGeometries (int i) {
        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";Geometry::") && list[n].Contains (", Model::")) {
            #region "Geometry To Model"
            string[] data = list[n].Split (',');
            string npp = data[1].Split (':')[2];
            models[npp].AddComponent<UniFBXGeometry> ().Add (models[npp], ugeometries1, ugeometries2, ugeometries3, setting, geometryIndex++);
            UniFBXStads.AddMesh ();
            return 1;
            #endregion
        }

        return 2;
    }

    public bool IsGeometriesReady ( ) {
        bool result = false;
        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread:
            result = (ugeometries1.IsDone);
            break;

#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread:
            if (this.setting.paths.maxThreads > 3) {
                result = (ugeometries1.IsDone & ugeometries2.IsDone & ugeometries3.IsDone);
            }
            else if (this.setting.paths.maxThreads == 3) {
                result = (ugeometries1.IsDone & ugeometries2.IsDone);
            }
            else {
                result = (ugeometries1.IsDone);
            }
            break;
#endif

            default:
            result = (ugeometries1.IsDone);
            break;
        }

        return result;
    }

    public void ClearGeometries ( ) {
        if (ugeometries1 != null) {
            ugeometries1.Clear ();
        }
    }

    #endregion

    #region "Materials"

    public void GetMaterials ( ) {
        umaterials = new UniFBXMaterials ();
        //backupBaseMaterials = new List<string> ();
        umaterials.Init (this.setting);
    }

    public bool IsMaterialsReady ( ) {
        return umaterials.IsDone;
    }

    public void GetLayeredTextures ( ) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";
        if (!models.ContainsKey (rootID)) return;

        if (models[rootID].GetComponent<UniFBXTexture2D> () == null) {
            models[rootID].AddComponent<UniFBXTexture2D> ().Init (this.setting);
        }
        ulayeredTextures = new UniFBXLayeredTextures ();
        ulayeredTextures.Init (this.setting);
    }

    public byte GetLayeredTextures (int i) {
        //string rootID = "RootNode";
        //if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";Texture::") && list[n].Contains (", LayeredTexture::")) {
            #region "Texture To LayeredTexture"
            string[] data = list[n + 1].Split (',');
            string textureID = data[1];
            string layeredTextureID = data[2];
            this.utextures.CreateTexture (layeredTextureID, textureID);
            this.ulayeredTextures.CreateTexture (layeredTextureID, utextures.GetProperty (textureID));
            //this.ulayeredTextures.SetTexture (models[rootID], layeredTextureID, utextures.GetProperty (textureID));
            return 1;
            #endregion
        }
        else if (list[n].Contains (";LayeredTexture::") && list[n].Contains (", Material::")) {
            #region "LayeredTexture To Material"
            string[] data = list[n + 1].Split (',');
            string layeredTextureID = data[1];
            string materialID = data[2];
            this.ulayeredTextures.CreateMaterial (layeredTextureID, umaterials.GetProperty (materialID), utextures.GetTexture (layeredTextureID));
            return 1;
            #endregion
        }
        else if (list[n].Contains (";Texture::") && list[n].Contains (", Material::")) {
            #region "Texture To Material"
            string[] data = list[n + 1].Split (',');
            string textureID = data[1];
            string materialID = data[2];
            this.utextures.CreateMaterial (textureID, materialID);
            //this.utextures.SetTexture (models[rootID], textureID);
            return 1;
            #endregion
        }

        return 2;
    }

    public bool IsLayeredTexturesReady ( ) {
        if (ulayeredTextures == null) return true;
        return ulayeredTextures.IsDone;
    }

    public void ClearMaterials ( ) {
        if (umaterials != null) {
            umaterials.Clear ();
        }
    }

    public void ClearLayeredTextures ( ) {
        if (ulayeredTextures != null) {
            ulayeredTextures.Clear ();
        }
    }
    #endregion

    #region "Textures

    public void GetTextures ( ) {
        utextures = new UniFBXTextures ();
        utextures.Init (this.setting);
    }

    public byte SetTextures (int i) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";
        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";Material::") && list[n].Contains (", Model::")) {
            #region "Material To Model"
            string[] data = list[n].Split (',');
            string npp = data[1].Split (':')[2];
            data = list[n + 1].Split (',');
            string materialID = data[1];
            string textureNames = ulayeredTextures.GetTextures (materialID, utextures);

            if (textureNames != null) {
                MeshRenderer mr = models[npp].GetComponent<MeshRenderer> ();
                if (mr != null) {
                    for (int m = 0; m < mr.materials.Length; m++) {
                        if (mr.materials[m].name == "0") {
                            mr.materials[m].name = umaterials.GetName (materialID);
                            mr.materials[m].color = umaterials.GetColor (materialID);
                            if (this.setting.textures.emissionmaps) {
                                mr.materials[m].SetColor ("_EmissionColor", umaterials.GetEmissiveColor (materialID));
                            }
                            this.ulayeredTextures.SetScaleOffset (materialID, mr.materials[m]);
                            if (utextures.IsEmbedded) {
                                models[rootID].GetComponent<UniFBXTexture2D> ().SetTexture (mr.materials[m], utextures, textureNames);
                            }
                            else {
                                models[rootID].GetComponent<UniFBXTexture2D> ().SetTexture (mr.materials[m], textureNames);
                            }
                            break;
                        }
                    }
                }
            }
            else {
                textureNames = utextures.GetTextures (materialID);
                MeshRenderer mr = models[npp].GetComponent<MeshRenderer> ();
                if (mr != null) {
                    for (int m = 0; m < mr.materials.Length; m++) {
                        if (mr.materials[m].name == "0") {
                            mr.materials[m].name = umaterials.GetName (materialID);
                            mr.materials[m].color = umaterials.GetColor (materialID);
                            if (this.setting.textures.emissionmaps) {
                                mr.materials[m].SetColor ("_EmissionColor", umaterials.GetEmissiveColor (materialID));
                            }
                            this.utextures.SetScaleOffset (materialID, mr.materials[m]);
                            if (utextures.IsEmbedded) {
                                models[rootID].GetComponent<UniFBXTexture2D> ().SetTexture (mr.materials[m], utextures, textureNames);
                            }
                            else {
                                models[rootID].GetComponent<UniFBXTexture2D> ().SetTexture (mr.materials[m], textureNames);
                            }
                            break;
                        }
                    }
                }
            }
            return 1;
            #endregion
        }

        return 2;
    }

    public bool IsTexturesReady ( ) {
        return utextures.IsDone;
    }

    public void ClearTextures ( ) {
        if (utextures != null) {
            utextures.Clear ();
        }
    }
    #endregion

    #region "Animations"

    public void GetAnimations ( ) {
        uanimations = new UniFBXAnimations ();
        uanimations.Init (UniFBX.list, this.setting, this.foo);
    }

    public bool AnimationsExist ( ) {
        return uanimations.AnimationExists ();
    }

    public byte GetAnimations (int i) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";AnimCurveNode::") && list[n].Contains (", Model::")) {
            #region "AnimCurveNode To Model"
            string[] data = list[n].Split (new char[] { ':', ',' });
            string ncc = data[2];   //T,R,S
            string npp = data[5];   //ModelName
            data = list[n + 1].Split (new char[] { ',', '\"' });
            string animCurveNodeId = data[3];   //AnimCurveNode ID
            string relativePath = "";           //Get Relative path
            Transform tr = models[npp].transform;
            while (tr != null) {
                relativePath += tr.name + "/";
                tr = tr.parent;
            }
            data = relativePath.Split ('/');
            relativePath = ""; for (int j = (data.Length - 3); j > -1; j--) relativePath += "/" + data[j];
            relativePath = relativePath.TrimStart ('/');

            this.uanimations.SetCurve (models[rootID], models[npp], relativePath, ncc, animCurveNodeId);
            return 1;
            #endregion
        }

        return 2;
    }

    public byte SetAnimationComponent (int i) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";
        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";AnimLayer::") && list[n].Contains (", AnimStack::")) {
            #region "AnimLayer To AnimStack"
            string[] data = list[n].Split (',');
            //string ncc = data[0].Split (':')[2];
            string npp = data[1].Split (':')[2];
            AnimationClip animationClip = new AnimationClip ();
            animationClip.name = npp;
            animationClip.frameRate = this.setting.animations.frameRate;
#if UNITY_5
            animationClip.legacy = true;
#endif
            models[rootID].AddComponent<Animation> ();
            models[rootID].GetComponent<Animation> ().clip = animationClip;
            models[rootID].GetComponent<Animation> ().AddClip (animationClip, npp);
            return 0;
            #endregion
        }
        return 1;
    }

    public void SetAnimatorComponent ( ) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        models[rootID].AddComponent<Animator> ().runtimeAnimatorController = this.setting.animations.animatorController;
    }

    public bool IsAnimationsReady ( ) {
        return uanimations.IsDone;
    }

    public void PlayAnimations ( ) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        models[rootID].GetComponent<Animation> ().Stop ();
        models[rootID].GetComponent<Animation> ().clip.EnsureQuaternionContinuity ();
        models[rootID].GetComponent<Animation> ().clip.wrapMode = this.setting.animations.animationWrap;
        models[rootID].GetComponent<Animation> ().Rewind ();
        models[rootID].GetComponent<Animation> ().Play ();
    }

    public void ClearAnimations ( ) {
        if (uanimations != null) {
            uanimations.Clear ();
        }
    }
    #endregion

    #region "Lights"

    public void GetLights ( ) {
        ulights = new UniFBXLights ();
        ulights.Init (this.setting);
    }

    public byte SetLights (int i) {
        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";NodeAttribute::") && list[n].Contains (", Model::")) {
            #region "NodeAttribute To Model"
            string[] data = list[n].Split (',');
            string npp = data[1].Split (':')[2];
            data = list[n + 1].Split (',');
            string nodeAttributeID = data[1];

            UniFBXLights.Properties70 prop70 = ulights.GetProperty (nodeAttributeID);
            if (prop70 != null) {
                models[npp].AddComponent<Light> ();
                if (this.setting.lights.importMethode == ImportMethode.FBX) {
                    switch (prop70.lighType) {
                        case 1: models[npp].GetComponent<Light> ().type = LightType.Directional; break;
                        case 2: models[npp].GetComponent<Light> ().type = LightType.Spot; break;
                        case 3: models[npp].GetComponent<Light> ().type = LightType.Area; break;
                        default: models[npp].GetComponent<Light> ().type = LightType.Point; break;
                    }
                }
                else if (this.setting.lights.importMethode == ImportMethode.Unity) {
                    models[npp].GetComponent<Light> ().type = this.setting.lights.lightType;
                }

                if (models[npp].GetComponent<Light> ().type == LightType.Spot) {
#if UNITY_5
                    models[npp].GetComponent<Light> ().bounceIntensity = 0.0f;
#endif
                    models[npp].GetComponent<Light> ().spotAngle = prop70.outerAngle;
                    models[npp].transform.localRotation *= Quaternion.Euler (90.0f * Vector3.right);
                }
                else if (models[npp].GetComponent<Light> ().type == LightType.Point) {
#if UNITY_5
                    models[npp].GetComponent<Light> ().bounceIntensity = 0.0f;
#endif
                }
                models[npp].GetComponent<Light> ().range = prop70.range;
                models[npp].GetComponent<Light> ().intensity = prop70.intensity;
                models[npp].GetComponent<Light> ().color = prop70.color;
                models[npp].GetComponent<Light> ().renderMode = this.setting.lights.renderMode;
                models[npp].GetComponent<Light> ().shadows = this.setting.lights.shadows;
                models[npp].GetComponent<Light> ().cullingMask = 1;
            }
            return 1;
            #endregion
        }

        return 2;
    }

    public bool IsLightsReady ( ) {
        return this.ulights.IsDone;
    }

    public void ClearLights ( ) {
        if (ulights != null) {
            ulights.Clear ();
        }
    }
    #endregion

    #region "Cameras"

    public void GetCameras ( ) {
        ucameras = new UniFBXCameras ();
        ucameras.Init (this.setting);
    }

    public byte SetCameras (int i) {
        int n = foo + i;
        if (n >= (list.Count - 1) || list[n].Contains (";Takes section")) return 0;

        if (list[n].Contains (";NodeAttribute::") && list[n].Contains (", Model::")) {
            #region "NodeAttribute To Model"
            string[] data = list[n].Split (',');
            string npp = data[1].Split (':')[2];
            data = list[n + 1].Split (',');
            string nodeAttributeID = data[1];

            UniFBXCameras.Properties70 prop70 = ucameras.GetProperty (nodeAttributeID);
            if (prop70 != null) {
                models[npp].AddComponent<Camera> ().enabled = false;
                models[npp].GetComponent<Camera> ().fieldOfView = prop70.fieldOfView;
                models[npp].GetComponent<Camera> ().transform.LookAt (prop70.lookAt);
                models[npp].GetComponent<Camera> ().backgroundColor = Camera.main.backgroundColor;
                models[npp].GetComponent<Camera> ().clearFlags = this.setting.cameras.cameraClearFlags;
            }
            return 1;
            #endregion
        }

        return 2;
    }

    public bool IsCamerasReady ( ) {
        return this.ucameras.IsDone;
    }

    public void ClearCameras ( ) {
        if (ucameras != null) {
            ucameras.Clear ();
        }
    }
    #endregion

    public void Renderize ( ) {
        if (this.setting.meshes.rendererOnLoad) return;
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";

        var mrs = models[rootID].GetComponentsInChildren<MeshRenderer> ();
        foreach (var mr in mrs) {
            mr.enabled = true;
        }


    }

    public GameObject GetRoot ( ) {
        string rootID = "RootNode";
        if (this.fbxsdk == FBX.SDK2010) rootID = " \"Model::Scene\"";
        return models[rootID];
    }

}