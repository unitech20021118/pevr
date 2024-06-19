using UnityEngine;
using System.Collections;

namespace CSUE {
    namespace UniFBX {

        #region "Methode"
        /// <summary>
        /// MeshType enumeration
        /// </summary>
        public enum ImportMethode {
            /// <summary>
            /// Fbx data mesh to unity data mesh
            /// </summary>
            Unity = 0,
            /// <summary>
            /// Fbx data mesh does not change
            /// </summary>
            FBX
        };
        #endregion

        #region "Running"

        /// <summary>
        /// RunnningMethode enumeration
        /// </summary>
        public enum RunnningMethode {
            /// <summary>
            /// MainThread
            /// </summary>
            MainThread = 0,
#if !UNITY_WEBGL
            /// <summary>
            /// AsyncThread
            /// </summary>
            AsyncThread
#endif
        };
        #endregion

        #region "Meshes"

        /// <summary>
        /// Mesh import enumeration
        /// </summary>
        public enum Imported {
            /// <summary>
            /// Import meshes if available
            /// </summary>
            Yes = 0,
            /// <summary>
            ///  Do not import meshes
            /// </summary>
            No
        }

        /// <summary>
        /// ColliderType enumeration
        /// </summary>
        public enum ColliderType {
            None = 0,
            Box,
            Sphere,
            Mesh
        };

        /// <summary>
        /// NormalType
        /// </summary>
        public enum NormalMethode {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Imported from file
            /// </summary>
            Imported,
            /// <summary>
            /// Use RecalculateNormals() methode from unity in roder to get normals
            /// </summary>
            Calculated
        };

        /// <summary>
        /// TangentType
        /// </summary>
        public enum TangentMethode {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Compute tangents
            /// </summary>
            Calculated
        };

        /// <summary>
        /// ConverTo enumeration
        /// </summary>
        public enum ConverTo {
            None = 0,
            Terrain
        };
        #endregion

        #region "Materials"

        /// <summary>
        /// Shader type
        /// </summary>
        public enum ShaderType {
            Standard = 0,
            Legacy            
        };

        #endregion

        #region "Textures"

        ///// <summary>
        ///// TextureCompression enumeration
        ///// </summary>
        //public enum TextureCompression {
        //    /// <summary>
        //    /// Texture compressed
        //    /// </summary>
        //    DTX = 0,
        //    /// <summary>
        //    /// Texture true color
        //    /// </summary>
        //    ARGB
        //};

        #endregion

        #region "Animation"

#if !UNITY_3_5
        /// <summary>
        /// AnimationType enumerator
        /// </summary>
        public enum AnimationMethode {
            /// <summary>
            /// Animation component
            /// </summary>
            Animation = 0,
            /// <summary>
            /// Animator component
            /// </summary>
            Animator
        };
#endif

        #endregion

        #region "Others"

        /// <summary>
        /// SplineDrawing enumeration
        /// </summary>
        public enum SplineDrawing {
            /// <summary>
            /// Draw spline path
            /// </summary>
            Yes = 0,
            /// <summary>
            /// Do not draw spline path
            /// </summary>
            No
        };

        #endregion

        #region "FBX SDK"
        /// <summary>
        /// FBX SDK enumeration
        /// </summary>
        public enum FBX {
            SDKNone = 0,
            SDKBinary,
            SDK2010,
            SDK2011,
            SDK2012,
            SDK2013,
            SDK2014,
            SDK2015,
            SDK2016,
            SDKUnknown
        }
        #endregion

        //#region "Objects"
        ///// <summary>
        ///// ObjectType enumeration
        ///// </summary>
        //public enum ObjectType {
        //    None = 0,
        //    GlobalSettings,
        //    NodeAttribute,
        //    Geometry,
        //    Model,
        //    Material,
        //    Pose,
        //    Deformer,
        //    SubDeformer,
        //    Video,
        //    Texture,
        //    LayeredTexture,
        //    AnimationStack,
        //    AnimationLayer,
        //    AnimationCurveNode,
        //    AnimationCurve
        //}

        ///// <summary>
        ///// ObjectProperty enumeration
        ///// </summary>
        //public enum ObjectProperty {
        //    None = 0,
        //    Scene,
        //    RootNode,
        //    Null,
        //    Light,
        //    Camera,
        //    Mesh,
        //    NurbsCurve,
        //    Skin,
        //    BindPose,
        //    LimbNode,
        //    SubDeformer,
        //    Cluster,
        //    Clip,
        //    DiffuseColor,
        //    Bump,
        //    NormalMap,
        //    AmbientColor,
        //    SpecularColor,
        //    EmissiveColor
        //    //ReflectionColor,
        //    //TransparentColor,
        //    //DisplacementColor
        //}

        //#endregion

        public enum FBXStatus {
            None = 0,
            Success,
            Wait,
            Connecting,
            DownloadingData,
            Loading,
            BinaryNotSupported,
            NGonsNotSupported,
            FBXSDKNotSupported,
            InternetNotAvailable,
            FileNotFound,
            FileEmpty,
            ExceedsMaximumSize,
            UnknownError
        };
    }
}