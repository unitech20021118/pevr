using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using CSUE.UniFBX;

[CanEditMultipleObjects, CustomEditor (typeof (UniFBXImport))]
public class UniFBXLoaderEditor : Editor {

    public override void OnInspectorGUI ( ) {

        UniFBXImport ufc = (UniFBXImport)target;

        base.OnInspectorGUI ();
        //EditorGUIUtility.LookLikeInspector ();
        //Color32 backup = GUI.backgroundColor;
        Color32 colorGUI = GUI.color;
        GUIStyle style = new GUIStyle ();
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = new Color (0.8f, 0.8f, 1.0f, 1.0f);
#if UNITY_PRO_LICENSE
        Color colorTitle = Color.yellow;
#else
		Color colorTitle = new Color (0.2f, 0.2f, 0.8f, 1.0f);
#endif


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Running Mode", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            ufc.setting.paths.runnningMethode = (RunnningMethode)EditorGUILayout.EnumPopup ("Loading Methode", ufc.setting.paths.runnningMethode);
            ufc.setting.paths.objectPerFrame = Mathf.Clamp (ufc.setting.paths.objectPerFrame, 1, 10);
            ufc.setting.paths.objectPerFrame = EditorGUILayout.IntField ("Objects per Frame", ufc.setting.paths.objectPerFrame);
#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            if (ufc.setting.paths.runnningMethode == RunnningMethode.AsyncThread) {
                ufc.setting.paths.maxThreads = Mathf.Clamp (ufc.setting.paths.maxThreads, 1, 4);
                ufc.setting.paths.maxThreads = EditorGUILayout.IntField ("Maximum Threads", ufc.setting.paths.maxThreads);  
            }
#endif
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Paths", style);
        GUI.color = colorGUI;

        if (ufc.setting != null) {
            ufc.setting.paths.urlModels = EditorGUILayout.TextField ("URL", ufc.setting.paths.urlModels);
            ufc.setting.paths.urlTextures = EditorGUILayout.TextField ("Texture Folder", ufc.setting.paths.urlTextures);
            ufc.setting.paths.filename = EditorGUILayout.TextField ("Filename (FBX)", ufc.setting.paths.filename);
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Meshes", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            ufc.setting.meshes.meshImport = (Imported)EditorGUILayout.EnumPopup ("Import Meshes", ufc.setting.meshes.meshImport);
            if (ufc.setting.meshes.meshImport == Imported.Yes) {
                ufc.setting.meshes.scaleFactor = EditorGUILayout.FloatField ("Gloabl Scale", ufc.setting.meshes.scaleFactor);
                ufc.setting.meshes.meshMethode = (ImportMethode)EditorGUILayout.EnumPopup ("Mesh Methode", ufc.setting.meshes.meshMethode);
                ufc.setting.meshes.normalMethode = (NormalMethode)EditorGUILayout.EnumPopup ("Normal Methode", ufc.setting.meshes.normalMethode);                
                ufc.setting.meshes.colliderType = (ColliderType)EditorGUILayout.EnumPopup ("Collider Type", ufc.setting.meshes.colliderType);
                ufc.setting.meshes.rendererOnLoad = EditorGUILayout.Toggle ("Render on load", ufc.setting.meshes.rendererOnLoad);
//#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
//                ufc.setting.meshes.conevertTo = (ConverTo)EditorGUILayout.EnumPopup ("Convert to", ufc.setting.meshes.conevertTo);
//#endif
            }
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Materials", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            ufc.setting.materials.materialImported = (Imported)EditorGUILayout.EnumPopup ("Import Materials", ufc.setting.materials.materialImported);
            if (ufc.setting.materials.materialImported == Imported.Yes) {
                ufc.setting.materials.shaderType = (ShaderType)EditorGUILayout.EnumPopup ("Shader Type", ufc.setting.materials.shaderType);
                if (ufc.setting.materials.shaderType == ShaderType.Standard) {
                    //ufc.setting.materials.standardMaterial = (Material)EditorGUILayout.ObjectField("Standard Material", ufc.setting.materials.standardMaterial, typeof(Material));
                }
            }
            else {
                ufc.setting.materials.colorDefault = (Color)EditorGUILayout.ColorField ("Color Default", ufc.setting.materials.colorDefault);
            }
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Textures", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            if (ufc.setting.materials.materialImported == Imported.Yes) {
                ufc.setting.textures.textureImported = (Imported)EditorGUILayout.EnumPopup ("Import Textures", ufc.setting.textures.textureImported);                
                if (ufc.setting.textures.textureImported == Imported.Yes) {
                    //ufc.setting.textures.textureCompression = (TextureCompression)EditorGUILayout.EnumPopup ("Texture Compression", ufc.setting.textures.textureCompression);
//#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
//                    if (ufc.setting.meshes.conevertTo == ConverTo.Terrain) {
//                        ufc.setting.textures.mipmaps = true;
//                    }
//#endif                   
                    //ufc.setting.textures.mipmaps = EditorGUILayout.Toggle ("Texture Mipmaps", ufc.setting.textures.mipmaps);
                    ufc.setting.textures.colormaps = EditorGUILayout.Toggle ("Color Maps", ufc.setting.textures.colormaps);
                    if (ufc.setting.materials.shaderType == ShaderType.Standard) {
                        ufc.setting.textures.normalmaps = EditorGUILayout.Toggle ("Normal Maps", ufc.setting.textures.normalmaps);
                        ufc.setting.textures.heightmaps = EditorGUILayout.Toggle ("Height Maps", ufc.setting.textures.heightmaps);
                        ufc.setting.textures.occlusionmaps = EditorGUILayout.Toggle ("Occlusion Maps", ufc.setting.textures.occlusionmaps);
                        ufc.setting.textures.emissionmaps = EditorGUILayout.Toggle ("Emission Maps", ufc.setting.textures.emissionmaps);
                        ufc.setting.textures.detailmasks = EditorGUILayout.Toggle ("Detail Masks", ufc.setting.textures.detailmasks);
                    }
                    else if (ufc.setting.materials.shaderType == ShaderType.Legacy) {
                        ufc.setting.textures.normalmaps = EditorGUILayout.Toggle ("Normal Maps", ufc.setting.textures.normalmaps);
                    }
                }
            }
            else {
                ufc.setting.textures.textureImported = (Imported)EditorGUILayout.EnumPopup ("Import Textures", Imported.No);
            }
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Animations", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            ufc.setting.animations.animationImported = (Imported)EditorGUILayout.EnumPopup ("Import Animations", ufc.setting.animations.animationImported);
            if (ufc.setting.animations.animationImported == Imported.Yes) {
#if !UNITY_3_5
                ufc.setting.animations.animationMethode = (AnimationMethode)EditorGUILayout.EnumPopup ("Animation Methode", ufc.setting.animations.animationMethode);
                if (ufc.setting.animations.animationMethode == AnimationMethode.Animation) {
                    ufc.setting.animations.animationWrap = (WrapMode)EditorGUILayout.EnumPopup ("Animation Wrap", ufc.setting.animations.animationWrap);
                    ufc.setting.animations.frameRate = Mathf.Clamp (ufc.setting.animations.frameRate, 1, 600);
                    ufc.setting.animations.frameRate = EditorGUILayout.FloatField ("Frame Rate", ufc.setting.animations.frameRate);
                }
                else {
                    //ufc.setting.animations.animatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField ("Animator Controller", ufc.setting.animations.animatorController, typeof (RuntimeAnimatorController));
                }
#endif
            }
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Lights", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            ufc.setting.lights.lightImported = (Imported)EditorGUILayout.EnumPopup ("Import Lights", ufc.setting.lights.lightImported);
            if (ufc.setting.lights.lightImported == Imported.Yes) {
                ufc.setting.lights.importMethode = (ImportMethode)EditorGUILayout.EnumPopup ("Import Methode", ufc.setting.lights.importMethode);
                if (ufc.setting.lights.importMethode == ImportMethode.Unity) {
                    ufc.setting.lights.lightType = (LightType)EditorGUILayout.EnumPopup ("Light Type", ufc.setting.lights.lightType);
                }
                ufc.setting.lights.shadows = (LightShadows)EditorGUILayout.EnumPopup ("Shadows", ufc.setting.lights.shadows);
                ufc.setting.lights.renderMode = (LightRenderMode)EditorGUILayout.EnumPopup ("Render Mode", ufc.setting.lights.renderMode);
                ufc.setting.lights.lightRange = EditorGUILayout.FloatField ("Light Range", ufc.setting.lights.lightRange);
            }
        }
        EditorGUILayout.Space ();


        GUI.color = colorTitle;
        EditorGUILayout.LabelField ("Cameras", style);
        GUI.color = colorGUI;
        if (ufc.setting != null) {
            ufc.setting.cameras.cameraImported = (Imported)EditorGUILayout.EnumPopup ("Import Cameras", ufc.setting.cameras.cameraImported);
            if (ufc.setting.cameras.cameraImported == Imported.Yes) {
                ufc.setting.cameras.cameraClearFlags = (CameraClearFlags)EditorGUILayout.EnumPopup ("Clear Flags Methode", ufc.setting.cameras.cameraClearFlags);
            }
        }
        EditorGUILayout.Space ();


        //GUI.color = colorTitle;
        //EditorGUILayout.LabelField ("Splines", style);
        //GUI.color = colorGUI;
        //if (ufc.setting != null) {
        //    ufc.setting.splines.splineImported = (Imported)EditorGUILayout.EnumPopup ("Import Splines", ufc.setting.splines.splineImported);
        //    if (ufc.setting.splines.splineImported == Imported.Yes) {
        //        ufc.setting.splines.splineDrawing = (SplineDrawing)EditorGUILayout.EnumPopup ("Draw Splines", ufc.setting.splines.splineDrawing);
        //    }
        //}
        //EditorGUILayout.Space ();

    }

}