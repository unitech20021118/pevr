using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CSUE {
    namespace UniFBX {
        namespace Extensions {
            public static class Extensions {

                public static FBX GetSDK (this string str_line) {
                    FBX sdk = FBX.SDKNone;
                    if (str_line.Contains ("FBX Binary")) {
#if UNITY_EDITOR
                        Debug.LogWarning ("Binary format not supported");
#endif
                        sdk = FBX.SDKBinary;
                        return sdk;
                    }
                    else {
                        string s = str_line.Substring (6, 5);
                        switch (s) {
                            case "6.1.0": sdk = FBX.SDK2010; break;
                            case "7.1.0": sdk = FBX.SDK2011; break;
                            case "7.2.0": sdk = FBX.SDK2012; break;
                            case "7.3.0": sdk = FBX.SDK2013; break;
                            case "7.4.0": sdk = FBX.SDK2014; break;
                            case "7.5.0": sdk = FBX.SDK2015; break;
                            case "7.6.0": sdk = FBX.SDK2016; break;
                            default: sdk = FBX.SDKUnknown;
#if UNITY_EDITOR
                            Debug.LogWarning ("Version not supported");
#endif
                            break;
                        }
                        return sdk;
                    }
                }
            
            }
        }

        namespace Utils {

            public static class Extensions {

#if !UNITY_WEBGL
                public static void ToTerrain (this GameObject o) {
                    //// See if a valid object is selected
                    var obj = o;
                    if (obj == null) {
                        return;
                    }
                    if (obj.GetComponent<MeshFilter> () == null) {
                        return;
                    }
                    else if (obj.GetComponent<MeshFilter> ().sharedMesh == null) {
                        return;
                    }

                    Vector3 size = o.GetComponent<MeshFilter> ().mesh.bounds.size;
                    Vector3 center = size / 2;
                    TerrainData tData = new TerrainData ();
                    tData.heightmapResolution = 512;
                    tData.size = size;
                    GameObject oTerrain = Terrain.CreateTerrainGameObject (tData);
                    var terrain = oTerrain.GetComponent<Terrain> ();

                    var terrainData = terrain.terrainData;
                    terrainData.size = size;
                    terrain.transform.position = -center;

#if UNITY_5
                    terrain.materialTemplate = new Material (Shader.Find ("Diffuse"));
                    terrain.materialTemplate.name = o.GetComponent<Renderer>().material.name;
                    terrain.materialTemplate.CopyPropertiesFromMaterial (o.GetComponent<Renderer>().material);
#endif

                    if (o.GetComponent<Renderer> ().material.mainTexture != null) {
                        List<SplatPrototype> terrainTextureList = new List<SplatPrototype> ();
                        for (int i = 0; i < o.GetComponent<Renderer> ().materials.Length; i++) {
                            SplatPrototype newSplat = new SplatPrototype ();
                            Texture2D texture = (Texture2D)o.GetComponent<Renderer> ().materials[i].mainTexture;
                            newSplat.texture = texture;
                            newSplat.tileSize = new Vector2 (size.x, size.z);
                            newSplat.tileOffset = Vector2.zero;
                            terrainTextureList.Add (newSplat);
                        }
                        terrainData.splatPrototypes = terrainTextureList.ToArray ();
                    }

                    // If there's no mesh collider, add one (and then remove it later when done)
                    var addedCollider = false;
                    var addedMesh = false;
                    var objCollider = obj.GetComponent<Collider> () as MeshCollider;
                    if (objCollider == null) {
                        objCollider = obj.AddComponent<MeshCollider> ();
                        addedCollider = true;
                    }
                    else if (objCollider.sharedMesh == null) {
                        objCollider.sharedMesh = obj.GetComponent<MeshFilter> ().sharedMesh;
                        addedMesh = true;
                    }

                    var resolutionX = terrainData.heightmapWidth;
                    var resolutionZ = terrainData.heightmapHeight;
                    var heights = terrainData.GetHeights (0, 0, resolutionX, resolutionZ);

                    // Use bounds a bit smaller than the actual object; otherwise raycasting tends to miss at the edges
                    var objectBounds = objCollider.bounds;
                    var leftEdge = objectBounds.center.x - objectBounds.extents.x + 0.001f;
                    var bottomEdge = objectBounds.center.z - objectBounds.extents.z + 0.001f;
                    var stepX = (objectBounds.size.x - 0.0019f) / resolutionX;
                    var stepZ = (objectBounds.size.z - 0.0019f) / resolutionZ;

                    // Set up raycast vars
                    var y = objectBounds.center.y + objectBounds.extents.y + 0.0001f;
                    RaycastHit hit;
                    Ray ray = new Ray (Vector3.zero, -Vector3.up);
                    var rayDistance = objectBounds.size.y + 0.0002f;
                    var heightFactor = 1.0f / rayDistance;

                    // Do raycasting samples over the object to see what terrain heights should be
                    var z = bottomEdge;
                    for (int zCount = 0; zCount < resolutionZ; zCount++) {
                        var x = leftEdge;
                        for (int xCount = 0; xCount < resolutionX; xCount++) {
                            ray.origin = new Vector3 (x, y, z);
                            if (objCollider.Raycast (ray, out hit, rayDistance)) {
                                heights[zCount, xCount] = 1.0f - (y - hit.point.y) * heightFactor;
                            }
                            else {
                                heights[zCount, xCount] = 0.0f;
                            }
                            x += stepX;
                        }
                        z += stepZ;
                    }

                    terrainData.SetHeights (0, 0, heights);

                    if (addedMesh) {
                        objCollider.sharedMesh = null;
                    }
                    if (addedCollider) {
                        MonoBehaviour.DestroyImmediate (objCollider);
                    }

                    //if (obj.transform.parent) {
                    //    MonoBehaviour.DestroyImmediate (obj.transform.parent.gameObject);
                    //}
                    //else {
                    //    MonoBehaviour.DestroyImmediate (obj);
                    //}
                    MonoBehaviour.DestroyImmediate (obj);
                }
#endif

#if !UNITY_WEBGL
                public static void ToTerrain (this GameObject o, GameObject root) {
                    //Debug.Log (o.renderer.material.name);
                    //// See if a valid object is selected
                    var obj = o;
                    if (obj == null) {
                        return;
                    }
                    if (obj.GetComponent<MeshFilter> () == null) {
                        return;
                    }
                    else if (obj.GetComponent<MeshFilter> ().sharedMesh == null) {
                        return;
                    }

                    Vector3 size = o.GetComponent<MeshFilter> ().mesh.bounds.size;
                    Vector3 center = size / 2;
                    TerrainData tData = new TerrainData ();
                    tData.heightmapResolution = 512;
                    tData.size = size;
                    GameObject oTerrain = Terrain.CreateTerrainGameObject (tData);
                    var terrain = oTerrain.GetComponent<Terrain> ();

                    var terrainData = terrain.terrainData;
                    terrainData.size = size;
                    terrain.transform.position = -center;

#if UNITY_5
                    terrain.materialTemplate = new Material (Shader.Find ("Diffuse"));
                    terrain.materialTemplate.name = o.GetComponent<Renderer>().material.name;
                    terrain.materialTemplate.CopyPropertiesFromMaterial (o.GetComponent<Renderer>().material);
#endif

                    if (o.GetComponent<Renderer> ().material.mainTexture != null) {
                        List<SplatPrototype> terrainTextureList = new List<SplatPrototype> ();
                        for (int i = 0; i < o.GetComponent<Renderer> ().materials.Length; i++) {
                            SplatPrototype newSplat = new SplatPrototype ();
                            Texture2D texture = (Texture2D)o.GetComponent<Renderer> ().materials[i].mainTexture;
                            newSplat.texture = texture;
                            newSplat.tileSize = new Vector2 (size.x, size.z);
                            newSplat.tileOffset = Vector2.zero;
                            terrainTextureList.Add (newSplat);
                        }
                        terrainData.splatPrototypes = terrainTextureList.ToArray ();
                    }


                    // If there's no mesh collider, add one (and then remove it later when done)
                    var addedCollider = false;
                    var addedMesh = false;
                    var objCollider = obj.GetComponent<Collider> () as MeshCollider;
                    if (objCollider == null) {
                        objCollider = obj.AddComponent<MeshCollider> ();
                        addedCollider = true;
                    }
                    else if (objCollider.sharedMesh == null) {
                        objCollider.sharedMesh = obj.GetComponent<MeshFilter> ().sharedMesh;
                        addedMesh = true;
                    }

                    var resolutionX = terrainData.heightmapWidth;
                    var resolutionZ = terrainData.heightmapHeight;
                    var heights = terrainData.GetHeights (0, 0, resolutionX, resolutionZ);

                    // Use bounds a bit smaller than the actual object; otherwise raycasting tends to miss at the edges
                    var objectBounds = objCollider.bounds;
                    var leftEdge = objectBounds.center.x - objectBounds.extents.x + 0.001f;
                    var bottomEdge = objectBounds.center.z - objectBounds.extents.z + 0.001f;
                    var stepX = (objectBounds.size.x - 0.0019f) / resolutionX;
                    var stepZ = (objectBounds.size.z - 0.0019f) / resolutionZ;

                    // Set up raycast vars
                    var y = objectBounds.center.y + objectBounds.extents.y + 0.0001f;
                    RaycastHit hit;
                    Ray ray = new Ray (Vector3.zero, -Vector3.up);
                    var rayDistance = objectBounds.size.y + 0.0002f;
                    var heightFactor = 1.0f / rayDistance;

                    // Do raycasting samples over the object to see what terrain heights should be
                    var z = bottomEdge;
                    for (int zCount = 0; zCount < resolutionZ; zCount++) {
                        var x = leftEdge;
                        for (int xCount = 0; xCount < resolutionX; xCount++) {
                            ray.origin = new Vector3 (x, y, z);
                            if (objCollider.Raycast (ray, out hit, rayDistance)) {
                                heights[zCount, xCount] = 1.0f - (y - hit.point.y) * heightFactor;
                            }
                            else {
                                heights[zCount, xCount] = 0.0f;
                            }
                            x += stepX;
                        }
                        z += stepZ;
                    }

                    terrainData.SetHeights (0, 0, heights);

                    if (addedMesh) {
                        objCollider.sharedMesh = null;
                    }
                    if (addedCollider) {
                        MonoBehaviour.DestroyImmediate (objCollider);
                    }

                    //if (obj.transform.parent) {
                    //    MonoBehaviour.DestroyImmediate (obj.transform.parent.gameObject);
                    //}
                    //else {
                    //    MonoBehaviour.DestroyImmediate (obj);
                    //}
                    MonoBehaviour.DestroyImmediate (obj);
                    oTerrain.transform.parent = root.transform;
                    oTerrain.transform.localPosition = Vector3.zero;
                    oTerrain.transform.localRotation = Quaternion.identity;
                }
#endif

#if !UNITY_WEBGL
                public static void ToTerrain (this GameObject o, int heightMapResolution) {
                    //// See if a valid object is selected
                    var obj = o;
                    if (obj == null) {
                        return;
                    }
                    if (obj.GetComponent<MeshFilter> () == null) {
                        return;
                    }
                    else if (obj.GetComponent<MeshFilter> ().sharedMesh == null) {
                        return;
                    }

                    Vector3 tSize = o.GetComponent<MeshFilter> ().mesh.bounds.size;
                    Vector3 tCenter = tSize / 2;
                    TerrainData tData = new TerrainData ();
                    tData.heightmapResolution = heightMapResolution;
                    tData.baseMapResolution = heightMapResolution / 2;
                    tData.size = tSize;
                    GameObject oTerrain = Terrain.CreateTerrainGameObject (tData);
                    var terrain = oTerrain.GetComponent<Terrain> ();

                    var terrainData = terrain.terrainData;
                    terrainData.size = tSize;
                    terrain.transform.position = -tCenter;
#if UNITY_5
                    terrain.materialTemplate = new Material (Shader.Find ("Diffuse"));
                    terrain.materialTemplate.name = o.GetComponent<Renderer>().material.name;
                    terrain.materialTemplate.CopyPropertiesFromMaterial (o.GetComponent<Renderer>().material);
#endif

                    if (o.GetComponent<Renderer> ().material.mainTexture != null) {
                        List<SplatPrototype> terrainTextureList = new List<SplatPrototype> ();
                        for (int i = 0; i < o.GetComponent<Renderer> ().materials.Length; i++) {
                            SplatPrototype newSplat = new SplatPrototype ();
                            Texture2D texture = (Texture2D)o.GetComponent<Renderer> ().materials[i].mainTexture;
                            newSplat.texture = texture;
                            newSplat.tileSize = new Vector2 (tSize.x, tSize.z);
                            newSplat.tileOffset = Vector2.zero;
                            terrainTextureList.Add (newSplat);
                        }
                        terrainData.splatPrototypes = terrainTextureList.ToArray ();
                    }

                    // If there's no mesh collider, add one (and then remove it later when done)
                    var addedCollider = false;
                    var addedMesh = false;
                    var objCollider = obj.GetComponent<Collider> () as MeshCollider;
                    if (objCollider == null) {
                        objCollider = obj.AddComponent<MeshCollider> ();
                        addedCollider = true;
                    }
                    else if (objCollider.sharedMesh == null) {
                        objCollider.sharedMesh = obj.GetComponent<MeshFilter> ().sharedMesh;
                        addedMesh = true;
                    }

                    var resolutionX = terrainData.heightmapWidth;
                    var resolutionZ = terrainData.heightmapHeight;
                    var heights = terrainData.GetHeights (0, 0, resolutionX, resolutionZ);

                    // Use bounds a bit smaller than the actual object; otherwise raycasting tends to miss at the edges
                    var objectBounds = objCollider.bounds;
                    var leftEdge = objectBounds.center.x - objectBounds.extents.x + 0.001f;
                    var bottomEdge = objectBounds.center.z - objectBounds.extents.z + 0.001f;
                    var stepX = (objectBounds.size.x - 0.0019f) / resolutionX;
                    var stepZ = (objectBounds.size.z - 0.0019f) / resolutionZ;

                    // Set up raycast vars
                    var y = objectBounds.center.y + objectBounds.extents.y + 0.0001f;
                    RaycastHit hit;
                    Ray ray = new Ray (Vector3.zero, -Vector3.up);
                    var rayDistance = objectBounds.size.y + 0.0002f;
                    var heightFactor = 1.0f / rayDistance;

                    // Do raycasting samples over the object to see what terrain heights should be
                    var z = bottomEdge;
                    for (int zCount = 0; zCount < resolutionZ; zCount++) {
                        var x = leftEdge;
                        for (int xCount = 0; xCount < resolutionX; xCount++) {
                            ray.origin = new Vector3 (x, y, z);
                            if (objCollider.Raycast (ray, out hit, rayDistance)) {
                                heights[zCount, xCount] = 1.0f - (y - hit.point.y) * heightFactor;
                            }
                            else {
                                heights[zCount, xCount] = 0.0f;
                            }
                            x += stepX;
                        }
                        z += stepZ;
                    }

                    terrainData.SetHeights (0, 0, heights);

                    if (addedMesh) {
                        objCollider.sharedMesh = null;
                    }
                    if (addedCollider) {
                        MonoBehaviour.DestroyImmediate (objCollider);
                    }

                    if (obj.transform.parent) {
                        MonoBehaviour.DestroyImmediate (obj.transform.parent.gameObject);
                    }
                    else {
                        MonoBehaviour.DestroyImmediate (obj);
                    }
                }
#endif
            }
        }

    }

    namespace Utils {
        public static partial class IO {

            public static string GetDataPath ( ) {
                string[] data = Application.dataPath.Split ('/');
                string path = "";
                for (int i = 0; i < data.Length - 1; i++) {
                    path += data[i] + "/";
                }
                return path;
            }

        }
    }

}