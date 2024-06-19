using UnityEngine;
using System.Collections;

namespace CSUE {
    namespace UniFBX {
        [System.Serializable]
        public class FBXPath {

            public string urlModels = "file:///C:/models/";
            public string urlTextures = "file:///C:/models/tex/";
            public string filename = "mymodel";
            //public const string filephp = "fctrl.php?filename=";
            public RunnningMethode runnningMethode = RunnningMethode.MainThread;
            public int objectPerFrame = 5;
            public int maxThreads = 2;

            public string GetModelPath()
            {
                //if (this.filename.Contains (".fbx") == false)
                //    this.filename += ".fbx";
                string s = "file:///" + this.urlModels + this.filename;
                return s;
            }

            public void SetModelPath(string UrlModels, string Filename)
            {
                urlModels = UrlModels;
                filename = Filename;
            }

            public string GetTexturePath ( ) {
                return this.urlTextures;
            }
        }

        [System.Serializable]
        public class FBXMeshes {
            public float scaleFactor = 0.01f;
            public Imported meshImport = Imported.Yes;
            public ImportMethode meshMethode = ImportMethode.Unity;
            public NormalMethode normalMethode = NormalMethode.Calculated;
            public ColliderType colliderType = ColliderType.None;
            public bool rendererOnLoad = true;
//#if !UNITY_WEBGL
//            public ConverTo conevertTo = ConverTo.None;
//#endif
        }

        [System.Serializable]
        public class FBXMaterials {
            public Imported materialImported = Imported.Yes;
            public ShaderType shaderType = ShaderType.Standard;
            public Material standardMaterial = null;
            public Color colorDefault = Color.grey;            
        }

        [System.Serializable]
        public class FBXTextures {
            public Imported textureImported = Imported.Yes;
            //public TextureCompression textureCompression = TextureCompression.DTX;
            //public bool mipmaps = true;
            public bool colormaps = true;
            public bool normalmaps = false;
            public bool heightmaps = false;
            public bool occlusionmaps = false;
            public bool emissionmaps = false;
            public bool detailmasks = false;
            //public bool bumpmaps = false;
            //public bool specularmaps = false;
            //public bool texturenameLikeMaterialname = false;
        }

        [System.Serializable]
        public class FBXAnimations {
            public Imported animationImported = Imported.No;            
#if !UNITY_3_5
            public AnimationMethode animationMethode = AnimationMethode.Animation;
            public RuntimeAnimatorController animatorController;
#endif
            public WrapMode animationWrap = WrapMode.Loop;
            public float frameRate = 30.0f;
        }

        [System.Serializable]
        public class FBXLights {
            public Imported lightImported = Imported.No;
            public ImportMethode importMethode = ImportMethode.FBX;
            public LightType lightType = LightType.Point;
            public LightShadows shadows = LightShadows.None;
            public LightRenderMode renderMode = LightRenderMode.Auto;
            public float lightRange = 100.0f;
        }

        [System.Serializable]
        public class FBXCameras {
            public Imported cameraImported = Imported.No;
            public CameraClearFlags cameraClearFlags = CameraClearFlags.Skybox;
        }

        [System.Serializable]
        public class FBXSplines {
            public Imported splineImported = Imported.No;
            public SplineDrawing splineDrawing = SplineDrawing.No;
        }

        [System.Serializable]
        public class FBXSetting {
            public FBXPath paths;
            public FBXMeshes meshes;
            public FBXMaterials materials;
            public FBXTextures textures;
            public FBXAnimations animations;
            public FBXLights lights;
            public FBXCameras cameras;
            public FBXSplines splines;            
            [HideInInspector]
            public FBXStatus Status = FBXStatus.None;
        }

    }
}