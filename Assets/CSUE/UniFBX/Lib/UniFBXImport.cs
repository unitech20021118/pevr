using UnityEngine;
using System.Collections;
using CSUE.UniFBX;

#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
[RequireComponent (typeof (UniFBXThread))]
#endif
public class UniFBXImport : MonoBehaviour {

    [HideInInspector]
    public FBXSetting setting;
    private bool _isDone = true;
    public bool IsDone {
        get { return this._isDone; }
        set { this._isDone = value; }
    }
    private float porcentage = 0.0f;
    private int CONNECTION_COUNT = 0;
    private int counter = 0;
    private GameObject o = null;
    public Transform parent;

    void Start ( ) {
        UniFBXStads.Init ();
    }

    void Update ( ) {
        if (CONNECTION_COUNT > 0) {
            var p = 100.0f * ((float)(counter + 1) / CONNECTION_COUNT);
            p = Mathf.Clamp (p, 0.0f, 100.0f);
            this.porcentage = p;
            UniFBXStads.SetPorcentage (this.porcentage);
        }
    }

    public Coroutine Load ( ) {
        if (this._isDone == false) return null;
        this._isDone = false;
        this.counter = 0;
        this.porcentage = 0.0f;
        this.CONNECTION_COUNT = 0;
        UniFBXStads.Init ();
        UniFBXStads.TimerStart ();

        if (this.setting.paths.urlModels.Contains ("http")) {
            return StartCoroutine(this.ILoadWWW());
        }
#if !UNITY_WEBPLAYER || !UNITY_WEBGL
        else {
            this.setting.paths.urlModels = this.setting.paths.urlModels.Replace ("file:///", "");
            return StartCoroutine(this.ILoad());
        }
        
#endif
    }

    public GameObject GetObject ( ) {
        return o;
    }

    public float GetProcentage ( ) {
        return this.porcentage;
    }

    private IEnumerator ILoadWWW()
    {
        string path = this.setting.paths.urlModels + this.setting.paths.filename + ".fbx";
        using (WWW www = new WWW(path))
        {
            this.setting.Status = FBXStatus.Connecting;
            while (!www.isDone) yield return null;
            if (www.error == null)
            {
                string s = www.text;
                if (!s.Contains("<br />"))
                {
                    using (UniFBX fbx = new UniFBX (s, this.setting, this.transform)) {
                        //Read fbx file
                        fbx.Load ();
                        while (fbx.IsRunning) yield return null;
                        int Len = (fbx.GetConnectionListCount () / 3) * 2;    //Models and Transform
                        if (this.setting.textures.textureImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3) * 2;
                        if (this.setting.lights.lightImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3);
                        if (this.setting.cameras.cameraImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3);
                        if (this.setting.animations.animationImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3);
                        CONNECTION_COUNT = Len;
                        int from = 2;
                        int skip = 3;
                        if (fbx.GetSDK () == FBX.SDK2010) {
                            from = 1;
                            skip = 1;
                        }

                        if (this.IsStatusGood () == true) {
                            int frames = 0;

                            //Read geometries and animations
                            #region "FBX Init"
                            if (this.setting.animations.animationImported == Imported.Yes) fbx.GetAnimations ();
                            if (this.setting.meshes.meshImport == Imported.Yes) {
                                fbx.GetGeometries ();
                            }
                            fbx.GetModels ();
                            #endregion

                            //Models
                            #region "Models as GameObjects"
                            for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                this.counter++;
                                if (fbx.SetModels (i) == false) break;
                                else {
                                    if (++frames == 1024) {
                                        frames = 0;
                                        yield return null;
                                    }
                                }
                            }
                            #endregion

                            //Transform
                            #region "Transforms (TRS)"
                            while (!fbx.IsLclTransformReady ()) yield return null;
                            for (int i = 0; i < fbx.GetModelCount (); i++) fbx.SetLclTransform (i);
                            fbx.ClearModels ();
                            #endregion

                            //Geometries as Meshes
                            #region "Geometries"
                            if (this.setting.meshes.meshImport == Imported.Yes) {
                                while (!fbx.IsGeometriesReady ()) yield return null;
                                yield return null;
                                yield return null;
                                frames = 0;
                                for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                    this.counter++;
                                    byte status = fbx.SetGeometries (i);
                                    if (status == 0) break;
                                }
                                fbx.ClearGeometries ();
                            }
                            #endregion

                            //Materials
                            #region "Materials"
                            if (this.setting.materials.materialImported == Imported.Yes) {
                                fbx.GetMaterials ();
                                if (this.setting.textures.textureImported == Imported.Yes) {
                                    fbx.GetLayeredTextures ();
                                    fbx.GetTextures ();
                                }
                                while (!fbx.IsMaterialsReady ()) yield return null;
                            }

                            if (this.setting.lights.lightImported == Imported.Yes) fbx.GetLights ();
                            if (this.setting.cameras.cameraImported == Imported.Yes) fbx.GetCameras ();
                            #endregion

                            //Textures
                            #region "Textures"
                            if (this.setting.textures.textureImported == Imported.Yes) {
                                while (!fbx.IsLayeredTexturesReady ()) yield return null;
                                while (!fbx.IsTexturesReady ()) yield return null;

                                frames = 0;
                                for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                    this.counter++;
                                    byte status = fbx.GetLayeredTextures (i);
                                    if (status == 0) break;
                                }
                                for (int i = 0; i < 20; i++) yield return null;

                                frames = 0;
                                for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                    this.counter++;
                                    byte status = fbx.SetTextures (i);
                                    if (status == 0) break;
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WEBGL
                        else if (status == 1) {
                            if (++frames == 2 * this.setting.paths.objectPerFrame) {
                                frames = 0;
                                yield return null;
                            }
                        }
#endif
                                }

                                fbx.ClearMaterials ();
                                fbx.ClearLayeredTextures ();
                                fbx.ClearTextures ();
                            }
                            #endregion

                            //Lights
                            #region "Lights"
                            if (this.setting.lights.lightImported == Imported.Yes) {
                                while (!fbx.IsLightsReady ()) yield return null;
                                frames = 0;
                                for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                    this.counter++;
                                    byte status = fbx.SetLights (i);
                                    if (status == 0) break;
                                    else if (status == 1) {
                                        if (++frames == 2 * this.setting.paths.objectPerFrame) {
                                            frames = 0;
                                            yield return null;
                                        }
                                    }
                                }
                                fbx.ClearLights ();
                            }
                            #endregion

                            //Cameras
                            #region "Cameras"
                            if (this.setting.cameras.cameraImported == Imported.Yes) {
                                while (!fbx.IsCamerasReady ()) yield return null;
                                frames = 0;
                                for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                    this.counter++;
                                    byte status = fbx.SetCameras (i);
                                    if (status == 0) break;
                                    else if (status == 1) {
                                        if (++frames == this.setting.paths.objectPerFrame) {
                                            frames = 0;
                                            yield return null;
                                        }
                                    }
                                }
                                fbx.ClearCameras ();
                            }
                            #endregion

                            //Animations
                            #region "Animations"
                            if (this.setting.animations.animationImported == Imported.Yes) {
                                if (this.setting.animations.animationMethode == AnimationMethode.Animation) {
                                    if (fbx.AnimationsExist ()) {
                                        while (!fbx.IsAnimationsReady ()) yield return null;

                                        frames = 0;
                                        for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                            this.counter++;
                                            byte status = fbx.SetAnimationComponent (i);
                                            if (status == 0) break;
                                        }

                                        yield return null;
                                        frames = 0;
                                        for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                            this.counter++;
                                            byte status = fbx.GetAnimations (i);
                                            if (status == 0) break;
                                            else if (status == 1) {
                                                if (++frames == 10 * this.setting.paths.objectPerFrame) {
                                                    frames = 0;
                                                    yield return null;
                                                }
                                            }
                                        }
                                        fbx.PlayAnimations ();
                                        fbx.ClearAnimations ();
                                    }
                                }
#if !UNITY_3_5
                                else if (this.setting.animations.animationMethode == AnimationMethode.Animator) {
                                    fbx.SetAnimatorComponent ();
                                }
#endif
                            }
                            #endregion


                            //Success                
                            UniFBXStads.TimerStop ();
                            this.setting.Status = FBXStatus.Success;
                            fbx.Renderize ();
                            this.o = fbx.GetRoot ();

                            //Clear all fbx variables
                            yield return new WaitForSeconds (0.3f);
                            fbx.Clear ();
                            yield return null;
                            Resources.UnloadUnusedAssets ();
                            this._isDone = true;
#if UNITY_EDITOR
                            if (this.setting.Status == FBXStatus.Success) {
                                Debug.Log ("Success!");
                            }
#endif
                        }
                        else {
                            UniFBXStads.TimerStop ();
                            yield return null;
                            Resources.UnloadUnusedAssets ();
                            this._isDone = true;
                        }
                    } 
                }
            }
            else
            {
                string e = www.error.ToUpper();
                Debug.Log(www.error);
                if (e.Contains("OPEN FILE"))
                {
                    this.setting.Status = FBXStatus.FileNotFound;
                    UniFBXStads.TimerStop();
#if UNITY_EDITOR
                    Debug.Log("Couldn't open file or file not found");
#endif
                }
                else if (e.Contains("RESOLVE HOST"))
                {
                    this.setting.Status = FBXStatus.InternetNotAvailable;
                    UniFBXStads.TimerStop();
#if UNITY_EDITOR
                    Debug.Log("Internet not available or could not resolve host");
#endif
                }
                else if (www.error.Contains("CONNECTION REFUSED"))
                {
                    this.setting.Status = FBXStatus.InternetNotAvailable;
                    UniFBXStads.TimerStop();
#if UNITY_EDITOR
                    Debug.Log("Internet not available or could not resolve host");
#endif
                }
                else
                {
                    this.setting.Status = FBXStatus.UnknownError;
                    UniFBXStads.TimerStop();
#if UNITY_EDITOR
                    Debug.Log(e);
#endif
                }
                Resources.UnloadUnusedAssets();
                this._isDone = true;
            }
        }
    }

    private IEnumerator ILoad ( ) {
        print(this.setting.paths.GetModelPath());
        using (UniFBX fbx = new UniFBX (this.setting, this.transform)) {
            //Read fbx file
            fbx.Load ();
            while (fbx.IsRunning) yield return null;            
            int Len = (fbx.GetConnectionListCount () / 3) * 2;    //Models and Transform
            if (this.setting.textures.textureImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3) * 2;
            if (this.setting.lights.lightImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3);
            if (this.setting.cameras.cameraImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3);
            if (this.setting.animations.animationImported == Imported.Yes) Len += (fbx.GetConnectionListCount () / 3);
            CONNECTION_COUNT = Len;
            int from = 2;
            int skip = 3;
            if (fbx.GetSDK () == FBX.SDK2010) {
                from = 1;
                skip = 1;
            }
            print(fbx.GetSDK().ToString());
            if (this.IsStatusGood () == true) 
            {
                int frames = 0;

                //Read geometries and animations
                #region "FBX Init"
                if (this.setting.animations.animationImported == Imported.Yes) fbx.GetAnimations ();
                if (this.setting.meshes.meshImport == Imported.Yes) {
                    fbx.GetGeometries ();
                }                
                #endregion

                //Models
                #region "Models as GameObjects"
                fbx.GetModels ();
                for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {                    
                    this.counter++;
                    if (fbx.SetModels (i) == false) break;
                    else {
                        if (++frames == 1024) {
                            frames = 0;
                            yield return null;
                        }
                    }
                }
                #endregion

                //Transform
                #region "Transforms (TRS)"
                Debug.Log(fbx.GetModelCount() + "~~~");
                while (!fbx.IsLclTransformReady ()) yield return null;
                for (int i = 0; i < fbx.GetModelCount (); i++) fbx.SetLclTransform (i);
                fbx.ClearModels ();
                #endregion

                //Geometries as Meshes
                #region "Geometries"
                if (this.setting.meshes.meshImport == Imported.Yes) {
                    while (!fbx.IsGeometriesReady ()) yield return null;
                    frames = 0;
                    for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {                        
                        this.counter++;
                        byte status = fbx.SetGeometries (i);
                        if (status == 0) break;
                    }                                        
                   //fbx.ClearGeometries ();
                }
                #endregion

                //Materials
                #region "Materials"
                if (this.setting.materials.materialImported == Imported.Yes) {
                    fbx.GetMaterials ();
                    if (this.setting.textures.textureImported == Imported.Yes) {
                        fbx.GetLayeredTextures ();
                        fbx.GetTextures ();
                    }
                    while (!fbx.IsMaterialsReady ()) yield return null;                    
                }

                if (this.setting.lights.lightImported == Imported.Yes) fbx.GetLights ();
                if (this.setting.cameras.cameraImported == Imported.Yes) fbx.GetCameras ();
                #endregion

                //Textures
                #region "Textures"
                if (this.setting.textures.textureImported == Imported.Yes) {
                    while (!fbx.IsLayeredTexturesReady ()) yield return null;
                                       
                    
                    frames = 0;
                    for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {                        
                        this.counter++;
                        byte status = fbx.GetLayeredTextures (i);
                        if (status == 0) break;
                    }
                    while (!fbx.IsTexturesReady ()) yield return null; 
                    for (int i = 0; i < 20; i++) yield return null;

                    frames = 0;
                    for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                        this.counter++;
                        byte status = fbx.SetTextures (i);
                        if (status == 0) break;
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WEBGL
                        else if (status == 1) {
                            if (++frames == 2 * this.setting.paths.objectPerFrame) {
                                frames = 0;
                                yield return null;
                            }
                        }
#endif
                    }

                    fbx.ClearMaterials ();
                    fbx.ClearLayeredTextures ();
                    fbx.ClearTextures ();
                }
                #endregion

                //Lights
                #region "Lights"
                if (this.setting.lights.lightImported == Imported.Yes) {
                    while (!fbx.IsLightsReady ()) yield return null;
                    frames = 0;
                    for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {                        
                        this.counter++;
                        byte status = fbx.SetLights (i);
                        if (status == 0) break;
                        else if (status == 1) {
                            if (++frames == 2 * this.setting.paths.objectPerFrame) {
                                frames = 0;
                                yield return null;
                            }
                        }
                    }
                    fbx.ClearLights ();
                }
                #endregion

                //Cameras
                #region "Cameras"
                if (this.setting.cameras.cameraImported == Imported.Yes) {
                    while (!fbx.IsCamerasReady ()) yield return null;                    
                    frames = 0;
                    for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                        this.counter++;
                        byte status = fbx.SetCameras (i);
                        if (status == 0) break;
                        else if (status == 1) {
                            if (++frames == this.setting.paths.objectPerFrame) {
                                frames = 0;
                                yield return null;
                            }
                        }
                    }
                    fbx.ClearCameras ();
                }
                #endregion

                //Animations
                #region "Animations"
                if (this.setting.animations.animationImported == Imported.Yes) {
                    print("有动画");
                    if (this.setting.animations.animationMethode == AnimationMethode.Animation) {
                        if (fbx.AnimationsExist ()) {
                            print("有动画1");
                            while (!fbx.IsAnimationsReady ()) yield return null;

                            frames = 0;
                            for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                this.counter++;
                                byte status = fbx.SetAnimationComponent (i);
                                if (status == 0) break;
                            }

                            yield return null;
                            frames = 0;
                            for (int i = from; i < fbx.GetListCount () - skip; i = i + skip) {
                                this.counter++;
                                byte status = fbx.GetAnimations (i);
                                if (status == 0) break;
                                else if (status == 1) {
                                    if (++frames == 10 * this.setting.paths.objectPerFrame) {
                                        frames = 0;
                                        yield return null;
                                    }
                                }
                            }
                            print("有动画2");
                            fbx.PlayAnimations ();                            
                            fbx.ClearAnimations ();
                        }
                    }
#if !UNITY_3_5
                    else if (this.setting.animations.animationMethode == AnimationMethode.Animator) {
                        fbx.SetAnimatorComponent ();
                    }
#endif
                }
                #endregion


                //Success                
                UniFBXStads.TimerStop ();
                this.setting.Status = FBXStatus.Success;
                fbx.Renderize ();
                this.o = fbx.GetRoot ();
                this.o.transform.SetParent(parent);
                //Clear all fbx variables
                
                fbx.Clear ();
                Resources.UnloadUnusedAssets ();
                this._isDone = true;
                //
#if UNITY_EDITOR
                if (this.setting.Status == FBXStatus.Success) 
                {
                    Debug.Log ("Success!");
                }
                else
                {
                    Debug.Log("NotSuccess!");
                }
                yield return null;
#endif
            }
            else 
            {
                UniFBXStads.TimerStop ();
                yield return null;
                Resources.UnloadUnusedAssets ();
                this._isDone = true;
            }
        }        
    }

    private bool IsStatusGood ( ) {
        if (this.setting.Status == FBXStatus.BinaryNotSupported) {
#if UNITY_EDITOR
            Debug.Log ("UniFBX doesn't support binary format file!");
#endif
            return false;
        }
        else if (this.setting.Status == FBXStatus.FileNotFound) {
#if UNITY_EDITOR
            Debug.Log ("Couldn't find " + this.setting.paths.filename + ".fbx");
#endif
            return false;
        }
        else if (this.setting.Status == FBXStatus.FBXSDKNotSupported) {
#if UNITY_EDITOR
            Debug.Log ("UniFBX doesn't support SDK. " +
                "Try to export your models in valid format: " +
                "FBX SDK 2011, FBX SDK 2012, FBX SDK 2013, FBX SDK 2014!");
#endif
            return false;
        }
        else if (this.setting.Status == FBXStatus.ExceedsMaximumSize) {
#if UNITY_EDITOR
            Debug.Log ("FBX File exceeds the maximum size (200 MB)");
#endif
            return false;
        }
        else {
            return true;
        }
    }

}