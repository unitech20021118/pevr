using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSUE.UniFBX;

public class UniFBXGeometry : MonoBehaviour {

    public struct KFbxMesh {
        public string id;
        public string name;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector2> uvs;
        public List<int> triangles;        
        public List<int> materials;
        public List<Vector4> tangents;
        public Submeshes[] submeshes;

        public Mesh ToUnityMesh (FBXSetting setting ) {
            Mesh mesh = new Mesh ();
            if (this.vertices == null) return mesh;

            if (this.vertices.Count >= 65000) {
#if UNITY_EDITOR    
                Debug.LogWarning ("Mesh may not have more then 65000 vertices");
#endif
                return mesh;
            }
            mesh.vertices = this.vertices.ToArray ();
            UniFBXStads.AddVertices (mesh.vertexCount);

            if (this.submeshes != null) {
                mesh.subMeshCount = this.submeshes.Length;
                for (int i = 0; i < mesh.subMeshCount; i++) {
                    mesh.SetTriangles (this.submeshes[i].triangles.ToArray (), i);                    
                }
                UniFBXStads.AddTriangles (mesh.triangles.Length / 3);
            }
            else {
                if (this.triangles != null) {
                    mesh.triangles = this.triangles.ToArray ();
                    UniFBXStads.AddTriangles (mesh.triangles.Length / 3);
                }
            }

            if (this.uvs != null) {
                if (this.uvs.Count == this.vertices.Count) {
                    mesh.uv = this.uvs.ToArray ();
                }
                else {
#if UNITY_EDITOR
                    Debug.LogWarning ("Don't have uvs or the mesh was subdivided in editor");
#endif
                }
            }

            switch (setting.meshes.normalMethode) {
                case NormalMethode.Imported: mesh.normals = this.normals.ToArray(); break;
                case NormalMethode.Calculated: mesh.RecalculateNormals (); break;
            }

            if (setting.textures.normalmaps) {
                if (this.tangents != null) mesh.tangents = this.tangents.ToArray ();
            }
            mesh.RecalculateBounds ();
            ;

            return mesh;
        }
    }

    public struct Submeshes {
        public List<int> triangles;
    }

    public static int Count = 0;
    //public static Dictionary<int, Mesh> meshes = new Dictionary<int,Mesh>();    
    public static List<int> indexes = null;

    public void Add (GameObject o, UniFBXGeometries ugeometries1, UniFBXGeometries ugeometries2, UniFBXGeometries ugeometries3, FBXSetting setting, int geometryIndex) {
        StartCoroutine (this.IAdd (o, ugeometries1, ugeometries2, ugeometries3, setting, geometryIndex));
    }

    private IEnumerator IAdd (GameObject o, UniFBXGeometries ugeometries1, UniFBXGeometries ugeometries2, UniFBXGeometries ugeometries3, FBXSetting setting, int geometryIndex) {
        if (ugeometries1 != null) while (!ugeometries1.IsDone) yield return null;
        if (ugeometries2 != null) while (!ugeometries2.IsDone) yield return null;
        if (ugeometries3 != null) while (!ugeometries3.IsDone) yield return null;

        Mesh mesh = null;
        if (UniFBX.meshes.ContainsKey (geometryIndex)) mesh = UniFBX.meshes[geometryIndex];

        if (mesh != null) {
            MeshFilter mf = o.AddComponent<MeshFilter> ();
            mf.mesh = mesh;
            mf.mesh.name = o.name;

            MeshRenderer mr = o.AddComponent<MeshRenderer> ();
            mr.enabled = setting.meshes.rendererOnLoad;
            mr.materials = new Material[mf.mesh.subMeshCount];

            for (int i = 0; i < mr.materials.Length; i++) {
#if UNITY_5
                if (setting.materials.shaderType == ShaderType.Standard) {
                    if (setting.materials.standardMaterial) {
                        mr.materials[i].CopyPropertiesFromMaterial (setting.materials.standardMaterial);
                    }
                }

                else if (setting.materials.shaderType == ShaderType.Legacy) {
#else
                if (setting.materials.shaderType == ShaderType.Legacy) {
#endif
                    if (setting.textures.normalmaps) {
#if UNITY_5
                        mr.materials[i].shader = Shader.Find ("Legacy Shaders/Bumped Diffuse");
#else
                        mr.materials[i].shader = Shader.Find ("Bumped Diffuse");
#endif
                    }
                    else {
#if UNITY_5
                        mr.materials[i].shader = Shader.Find ("Legacy Shaders/Diffuse");
#else
                        mr.materials[i].shader = Shader.Find ("Diffuse");
#endif
                    }
                    
                }
                mr.materials[i].name = "0";
                if (setting.materials.materialImported == Imported.No) {
                    mr.materials[i].color = setting.materials.colorDefault;
                }
                yield return null;
            }

            switch (setting.meshes.colliderType) {
                case ColliderType.Box: o.AddComponent<BoxCollider> (); break;
                case ColliderType.Mesh: o.AddComponent<MeshCollider> (); break;
                case ColliderType.Sphere: o.AddComponent<SphereCollider> (); break;
            }        
        }

        UniFBXGeometry.Count++;
        DestroyImmediate (this);
    }

}