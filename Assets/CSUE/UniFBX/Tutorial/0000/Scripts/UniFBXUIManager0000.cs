using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CSUE.UniFBX;

public class UniFBXUIManager0000 : MonoBehaviour {

    public UniFBXImport uimport;

    public InputField inputFieldThread;
    public Dropdown dropDownMesh;
    public InputField inputFieldMesh;
    public Toggle toggleMesh;
    public Dropdown dropDownMaterial;
    public Toggle toggleTexture;
    public Toggle[] toggleTextures;
    public Dropdown[] dropDownAnimation;
    public InputField inputFieldAnimation;
    public Dropdown[] dropDownLight;
    public InputField inputFieldLight;
    public Dropdown dropDownCamera;

    public void SetPathModel (InputField inputField) {
        if (uimport == null) return;
        if (inputField == null) return;

        uimport.setting.paths.urlModels = inputField.text;
    }

    public void SetPathTextures (InputField inputField) {
        if (uimport == null) return;
        if (inputField == null) return;

        uimport.setting.paths.urlTextures = inputField.text;
    }

    public void SetPathFilename (InputField inputField) {
        if (uimport == null) return;
        if (inputField == null) return;

        uimport.setting.paths.filename = inputField.text;
    }

    public void SetMaximumThread (InputField inputField) {
        if (uimport == null) return;
        if (inputField == null) return;

        if (inputField.text != "") {
            var n = int.Parse (inputField.text);
            n = Mathf.Clamp (n, 1, 4);
            uimport.setting.paths.maxThreads = n;
        }
    }

    public void SetRunningMethod (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0: 
                    uimport.setting.paths.runnningMethode = RunnningMethode.MainThread;
                    inputFieldThread.interactable = false;
                    break;
#if !UNITY_WEBPLAYER && !UNITY_WEBGL
                case 1: 
                    uimport.setting.paths.runnningMethode = RunnningMethode.AsyncThread;
                    inputFieldThread.interactable = true;
                    break;
#endif
                default: 
                    uimport.setting.paths.runnningMethode = RunnningMethode.MainThread;
                    inputFieldThread.interactable = false;
                    break;
            }
        }
    }

    public void SetLoad (Button button) {
        if (uimport == null) return;
        if (button == null) return;

        uimport.Load ();
    }

    public void SetReset (Button button) {
        if (uimport == null) return;
        if (button == null) return;

#if UNITY_5
        UnityEngine.SceneManagement.SceneManager.LoadScene (0);
#else
		Application.LoadLevel (0);
#endif         
    }

    public void SetToggleMeshes (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        if (toggle.isOn) {
            uimport.setting.meshes.meshImport = Imported.Yes;
            dropDownMesh.interactable = true;
            inputFieldMesh.interactable = true;
            toggleMesh.interactable = true;
        }
        else {
            uimport.setting.meshes.meshImport = Imported.No;
            dropDownMesh.interactable = false;
            inputFieldMesh.interactable = false;
            toggleMesh.interactable = false;
        }
    }

    public void SetToggleMaterials (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        if (toggle.isOn) {
            uimport.setting.materials.materialImported = Imported.Yes;
            dropDownMaterial.interactable = true;
            toggleTexture.interactable = true;            
            for (int i = 0; i < toggleTextures.Length; i++) {
                toggleTextures[i].interactable = true;
                toggleTextures[i].transform.GetChild (1).GetComponent<Text> ().color = Color.white;
            }
        }
        else {
            uimport.setting.materials.materialImported = Imported.No;
            dropDownMaterial.interactable = false;
            toggleTexture.interactable = false;
            toggleTexture.isOn = false;
            for (int i = 0; i < toggleTextures.Length; i++) {
                toggleTextures[i].interactable = false;
                toggleTextures[i].transform.GetChild (1).GetComponent<Text> ().color = new Color (1, 1, 1, 0.5f);
            }
        }
    }

    public void SetToggleTextures (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        if (toggle.isOn) {
            uimport.setting.textures.textureImported = Imported.Yes;            
            for (int i = 0; i < toggleTextures.Length; i++) {
                toggleTextures[i].interactable = true;
                toggleTextures[i].transform.GetChild (1).GetComponent<Text> ().color = Color.white;
            }
        }
        else {
            uimport.setting.textures.textureImported = Imported.No;            
            for (int i = 0; i < toggleTextures.Length; i++) {
                toggleTextures[i].interactable = false;
                toggleTextures[i].transform.GetChild (1).GetComponent<Text> ().color = new Color (1, 1, 1, 0.5f);
            }
        }
    }

    public void SetToggleAnimations (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        if (toggle.isOn) {
            uimport.setting.animations.animationImported = Imported.Yes;
            for (int i = 0; i < dropDownAnimation.Length; i++) {
                dropDownAnimation[i].interactable = true;
            }
            inputFieldAnimation.interactable = true;
        }
        else {
            uimport.setting.animations.animationImported = Imported.No;
            for (int i = 0; i < dropDownAnimation.Length; i++) {
                dropDownAnimation[i].interactable = false;
            }
            inputFieldAnimation.interactable = false;
        }
    }

    public void SetToggleLights (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        if (toggle.isOn) {
            uimport.setting.lights.lightImported = Imported.Yes;
            for (int i = 0; i < dropDownLight.Length; i++) {
                dropDownLight[i].interactable = true;
                inputFieldLight.interactable = true;
            }
        }
        else {
            uimport.setting.lights.lightImported = Imported.No;
            for (int i = 0; i < dropDownLight.Length; i++) {
                dropDownLight[i].interactable = false;
                inputFieldLight.interactable = false;
            }
        }
    }

    public void SetToggleCameras (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        if (toggle.isOn) {
            uimport.setting.cameras.cameraImported = Imported.Yes;
            dropDownCamera.interactable = true;
        }
        else {
            uimport.setting.cameras.cameraImported = Imported.No;
            dropDownCamera.interactable = false;
        }
    }

    public void SetGlobalScaling (InputField inputField) {
        if (uimport == null) return;
        if (inputField == null) return;

        if (inputField.text != "") {
            uimport.setting.meshes.scaleFactor = float.Parse (inputField.text);
        }
    }

    public void SetColliderType (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0: uimport.setting.meshes.colliderType = ColliderType.None; break;
                case 1: uimport.setting.meshes.colliderType = ColliderType.Box; break;
                case 2: uimport.setting.meshes.colliderType = ColliderType.Sphere; break;
                case 3: uimport.setting.meshes.colliderType = ColliderType.Mesh; break;
            }
        }
    }

    public void SetRenderOnLoad (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.meshes.rendererOnLoad = toggle.isOn;
    }

    public void SetMaterialType (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.materials.shaderType = ShaderType.Standard;
                for (int i = 2; i < toggleTextures.Length; i++) {
                    toggleTextures[i].interactable = true;
                    toggleTextures[i].transform.GetChild (1).GetComponent<Text> ().color = Color.white;
                }
                break;

                case 1:
                uimport.setting.materials.shaderType = ShaderType.Legacy;
                for (int i = 2; i < toggleTextures.Length; i++) {
                    toggleTextures[i].interactable = false;
                    toggleTextures[i].transform.GetChild (1).GetComponent<Text> ().color = new Color (1, 1, 1, 0.5f);
                }
                break;
            }
        }
    }

    public void SetTextureColormap (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.textures.colormaps = toggle.isOn;
    }

    public void SetTextureNormalmap (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.textures.normalmaps = toggle.isOn;
    }

    public void SetTextureHeightmap (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.textures.heightmaps = toggle.isOn;
    }

    public void SetTextureOclussionmap (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.textures.occlusionmaps = toggle.isOn;
    }

    public void SetTextureEmisionmap (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.textures.emissionmaps = toggle.isOn;
    }

    public void SetTextureDetailmask (Toggle toggle) {
        if (uimport == null) return;
        if (toggle == null) return;

        uimport.setting.textures.detailmasks = toggle.isOn;
    }

    public void SetAnimationType (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                    uimport.setting.animations.animationMethode = AnimationMethode.Animator;
                    dropDownAnimation[1].interactable = false;
                    inputFieldAnimation.interactable = false;
                    break;

                case 1:
                    uimport.setting.animations.animationMethode = AnimationMethode.Animation;
                    dropDownAnimation[1].interactable = true;
                    inputFieldAnimation.interactable = true;
                    break;
            }
        }
    }

    public void SetAnimationWrapLoop (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.animations.animationWrap = WrapMode.Default;
                break;

                case 1:
                uimport.setting.animations.animationWrap = WrapMode.Clamp;
                break;

                case 2:
                uimport.setting.animations.animationWrap = WrapMode.Once;
                break;

                case 3:
                uimport.setting.animations.animationWrap = WrapMode.Loop;
                break;

                case 4:
                uimport.setting.animations.animationWrap = WrapMode.PingPong;
                break;

                case 5:
                uimport.setting.animations.animationWrap = WrapMode.ClampForever;
                break;
            }
        }
    }

    public void SetAnimationFrameRate (InputField field) {
        if (uimport == null) return;
        if (field == null) return;

        if (field.text != "") {
            uimport.setting.animations.frameRate = float.Parse (field.text);
        }
    }

    public void SetLightMethod (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.lights.importMethode = ImportMethode.FBX;
                break;

                case 1:
                uimport.setting.lights.importMethode = ImportMethode.Unity;
                break;
            }
        }
    }

    public void SetLightType (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.lights.lightType = LightType.Spot;
                break;

                case 1:
                uimport.setting.lights.lightType = LightType.Directional;
                break;
                case 2:
                uimport.setting.lights.lightType = LightType.Point;
                break;

                case 3:
                uimport.setting.lights.lightType = LightType.Area;
                break;

            }
        }

    }

    public void SetLightShadow (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.lights.shadows = LightShadows.None;
                break;

                case 1:
                uimport.setting.lights.shadows = LightShadows.Hard;
                break;

                case 2:
                uimport.setting.lights.shadows = LightShadows.Soft;
                break;
            }
        }
    }

    public void SetLightRender (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.lights.renderMode = LightRenderMode.Auto;
                break;

                case 1:
                uimport.setting.lights.renderMode = LightRenderMode.ForcePixel;
                break;

                case 2:
                uimport.setting.lights.renderMode = LightRenderMode.ForceVertex;
                break;
            }
        }
    }

    public void SetLightRange (InputField inputField) {
        if (uimport == null) return;
        if (inputField == null) return;

        if (inputField.text != "") {
            uimport.setting.lights.lightRange = float.Parse (inputField.text);
        }
    }

    public void SetCameraFlags (Dropdown dropDown) {
        if (uimport == null) return;
        if (dropDown == null) return;

        if (dropDown) {
            switch (dropDown.value) {
                case 0:
                uimport.setting.cameras.cameraClearFlags = CameraClearFlags.Skybox;
                break;

                case 1:
                uimport.setting.cameras.cameraClearFlags = CameraClearFlags.SolidColor;
                break;

                case 2:
                uimport.setting.cameras.cameraClearFlags = CameraClearFlags.Color;
                break;

                case 3:
                uimport.setting.cameras.cameraClearFlags = CameraClearFlags.Depth;
                break;

                case 4:
                uimport.setting.cameras.cameraClearFlags = CameraClearFlags.Nothing;
                break;
            }
        }
    }

}