using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSUE.UniFBX;

public class UniFBXGeometries {

    private FBXSetting setting;
    private List<int> indexes = null;    
    private List<UniFBXGeometry.KFbxMesh> kFBXMeshes = null;
    private List<string> verticesRecord = null;
    private int geometryIndex = 0;
    private bool _isDone = false;
    public bool IsDone {
        get { return this._isDone; }
        set { this._isDone = value; }
    }
    private int thread = 1;


    public void Init (FBXSetting setting, int thread) {
        this.setting = setting;
        this.thread = thread;

        this.geometryIndex = 0;
        verticesRecord = new List<string> ();
        this.indexes = new List<int> ();
        this.kFBXMeshes = new List<UniFBXGeometry.KFbxMesh> ();
        if (UniFBX.meshes == null) UniFBX.meshes = new Dictionary<int, Mesh> ();

        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread:
            this.Run ();
            break;

#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread:
            UniFBXThread.RunAsync (this.Run);
            break;
#endif
            default:
            this.Run ();
            break;
        }
    }

    private void Run ( ) {
        this._isDone = false;

        int Len = UniFBXGeometry.indexes.Count;

        switch (this.setting.paths.runnningMethode) {
            case RunnningMethode.MainThread:
            for (int i = (this.thread - 1); i < Len; i = i + 1) {
                Debug.Log(UniFBXGeometry.indexes[i]);
                this.GetMesh (UniFBXGeometry.indexes[i] + 1);
            }
            this.SetGeometries ();
            break;

#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
            case RunnningMethode.AsyncThread:
            int step = 1;
            if (this.setting.paths.maxThreads > 3) {
                step = 3;
            }
            else if (this.setting.paths.maxThreads == 3) {
                step = 2;
            }
            else {
                step = 1;
            }

            for (int i = (this.thread - 1); i < Len; i = i + step) {
                this.GetMesh (UniFBXGeometry.indexes[i] + 1);
            }
            UniFBXThread.QueueOnMainThread (this.SetGeometries);
            break;
#endif

            default:
            for (int i = (this.thread - 1); i < Len; i = i + 1) {
                this.GetMesh (UniFBXGeometry.indexes[i] + 1);
            }
            this.SetGeometries ();
            break;
        }
        //Debug.Log ("Run " + this.thread.ToString ());
        this._isDone = true;
    }

    private void SetGeometries ( ) {
        int j = 0;
        int key = 1;
        if (this.thread == 1) key = 0;
        else if (this.thread == 2) key = 1;
        else if (this.thread == 3) key = 2;
        for (int i = 0; i < this.kFBXMeshes.Count; i++) {
            j = this.indexes[i];
            UniFBX.meshes.Add (key, this.kFBXMeshes[j].ToUnityMesh (this.setting));
            switch (this.setting.paths.runnningMethode) {
                case RunnningMethode.MainThread:
                key = key + 1;
                break;

#if (!UNITY_WEBPLAYER && !UNITY_WEBGL)
                case RunnningMethode.AsyncThread:
                int step = 1;
                if (this.setting.paths.maxThreads > 3) {
                    step = 3;
                }
                else if (this.setting.paths.maxThreads == 3) {
                    step = 2;
                }
                else {
                    step = 1;
                }
                key = key + step;
                break;
#endif

                default:
                key = key + 1;
                break;
            }

        }
        //Debug.Log ("Run " + this.thread.ToString () + ": " + UniFBX.meshes.Count.ToString ());
    }

    public void Clear ( ) {
        if (this.indexes != null) this.indexes.Clear ();
        if (UniFBX.meshes != null) UniFBX.meshes.Clear ();
        if (this.kFBXMeshes != null) this.kFBXMeshes.Clear ();
        this.indexes = null;
        UniFBX.meshes = null;
        this.kFBXMeshes = null;
    }

    #region "Mesh Processing"

    private UniFBXGeometry.KFbxMesh GetMesh2010 (List<string> list, int n) {
        UniFBXGeometry.KFbxMesh kFBXMesh = new UniFBXGeometry.KFbxMesh ();
        string[] data = new string[0];
        string group = "";
        int dnVertice = 0;
        int dnTriangle = 0;
        int dnNormal = 0;
        int dnUV = 0;
        int dnUVIndex = 0;
        int dnMaterial = 0;
        for (int i = 0; i < list.Count; i++) {
            string s = list[n + i];

            if (s.Contains ("Vertices: ")) {
                dnVertice = i + 0;
            }
            else if (s.Contains ("PolygonVertexIndex: ")) {
                dnTriangle = i + 0;
            }
            else if (s.Contains ("Normals: ")) {
                dnNormal = i + 0;
            }
            else if (s.Contains ("UV: ")) {
                dnUV = i + 0;
            }
            else if (s.Contains ("UVIndex: ")) {
                dnUVIndex = i + 0;
            }
            else if (s.Contains ("Materials: ")) {
                dnMaterial = i + 0;
            }
            else if (s.Contains ("Layer:") && !s.Contains ("MultiLayer:")) {
                break;
            }
        }

        #region "Vertices"
        List<Vector3> vertices = new List<Vector3> ();
        list[n + dnVertice] = list[n + dnVertice].Split (':')[1];
        group = "";
        for (int i = (n + dnVertice); i < list.Count; i++) {
            if (list[i].Contains ("PolygonVertexIndex")) break;
            group += list[i];
        }

        int f = verticesRecord.FindIndex (x => x == group);
        verticesRecord.Add (group);
        if (f != -1) {
            kFBXMesh.id = f.ToString ();
            return kFBXMesh;
        }
        data = group.Split (',');
        for (int j = 0; j < data.Length; j = j + 3) {
            float x = float.Parse (data[j + 0]);
            float y = float.Parse (data[j + 1]);
            float z = float.Parse (data[j + 2]);
            vertices.Add (new Vector3 (-x, y, z) * this.setting.meshes.scaleFactor);
        }
        #endregion

        #region "Triangles"
        List<int> triangles = new List<int> ();
        List<int> kfbxt = new List<int> ();
        List<int> quads = new List<int> ();
        List<int> del = new List<int> ();
        int q = 0;
        list[n + dnTriangle] = list[n + dnTriangle].Split (':')[1];
        group = "";
        for (int i = (n + dnTriangle); i < list.Count; i++) {
            if (list[i].Contains ("GeometryVersion")) break;
            group += list[i];
        }

        data = group.Split (',');
        for (int i = 0; i < data.Length; i++) {
            q++;
            int t = int.Parse (data[i]);
            triangles.Add (t);

            if (t < 0 && q == 3) {
                q = 0;
                t = -t - 1;
                triangles[i] = t;
            }
            else if (t < 0 && q == 4) {
                q = 0;
                t = -t - 1;
                triangles[i] = t;
                kfbxt.Add (triangles[i - 3]);
                kfbxt.Add (triangles[i - 2]);
                kfbxt.Add (triangles[i - 0]);
                quads.Add (i - 3);
                quads.Add (i - 2);
                quads.Add (i - 0);
                del.Add (i - 3);
            }
            else if (q > 4) {
                this.setting.Status = FBXStatus.NGonsNotSupported;
                return new UniFBXGeometry.KFbxMesh ();
            }
        }
        triangles.AddRange (kfbxt);
        int k = 0;
        for (int i = 0; i < del.Count; i++) {
            if (del[i] < triangles.Count) triangles.RemoveAt (del[i] - (k++)); else break;
        }
        #endregion

        #region "Normales"
        List<Vector3> normals = new List<Vector3> ();
        list[n + dnNormal] = list[n + dnNormal].Split (':')[1];
        group = "";
        for (int i = (n + dnNormal); i < list.Count; i++) {
            if (list[i].Contains ("}")) break;
            group += list[i];
        }

        data = group.Split (',');
        for (int j = 0; j < data.Length; j = j + 3) {
            float x = float.Parse (data[j + 0]);
            float y = float.Parse (data[j + 1]);
            float z = float.Parse (data[j + 2]);
            normals.Add (new Vector3 (-x, y, z));
        }
        bool isByVertice = list[n + dnNormal - 1].Contains ("ByVertice");
        if (del.Count > 0 && isByVertice == false) {
            k = 0;
            for (int i = 0; i < quads.Count; i++) {
                if (quads[i] < normals.Count) normals.Add (normals[quads[i]]); else break;
            }
            for (int i = 0; i < del.Count; i++) {
                if (del[i] < normals.Count) normals.RemoveAt (del[i] - (k++)); else break;
            }
        }
        #endregion

        #region "UV"
        List<Vector2> uv = new List<Vector2> ();
        list[n + dnUV] = list[n + dnUV].Split (':')[1];
        group = "";
        for (int i = (n + dnUV); i < list.Count; i++) {
            if (list[i].Contains ("UVIndex")) break;
            group += list[i];
        }

        data = group.Split (',');
        for (int j = 0; j < data.Length; j = j + 2) {
            float x = float.Parse (data[j + 0]);
            float y = float.Parse (data[j + 1]);
            uv.Add (new Vector2 (x, y));
        }
        group = "";
        List<Vector2> uvTemp = new List<Vector2> ();
        group = "";
        list[n + dnUVIndex] = list[n + dnUVIndex].Split (':')[1];
        for (int i = (n + dnUVIndex); i < list.Count; i++) {
            if (list[i].Contains ("}")) break;
            group += list[i];
        }

        data = group.Split (',');
        for (int i = 0; i < data.Length; i++) {
            uvTemp.Add (uv[int.Parse (data[i])]);
        }
        if (del.Count > 0) {
            k = 0;
            for (int i = 0; i < quads.Count; i++) {
                if (quads[i] < uvTemp.Count) uvTemp.Add (uvTemp[quads[i]]); else break;
            }
            for (int i = 0; i < del.Count; i++) {
                if (del[i] < uvTemp.Count) uvTemp.RemoveAt (del[i] - (k++)); else break;
            }
        }
        uv = uvTemp;
        #endregion

        #region "Materials"
        List<int> materials = new List<int> ();
        list[n + dnMaterial] = list[n + dnMaterial].Split (':')[1];
        group = "";
        for (int i = (n + dnMaterial); i < list.Count; i++) {
            if (list[i].Contains ("}")) break;
            group += list[i];
        }

        data = group.Split (',');
        if (data.Length == 1) {
            int mtl = 0;
            int.TryParse (data[0], out mtl);
            for (int i = 0; i < triangles.Count / 3; i++) materials.Add (mtl);
        }
        else {
            for (int i = 0; i < data.Length; i++) {
                materials.Add (int.Parse (data[i]));
            }
            if (del.Count > 0) {
                int Len = (materials.Count - del.Count) / 2;
                for (int i = 0; i < Len; i++) materials.RemoveAt (materials.Count - 1);
                materials.AddRange (materials);
            }
        }
        if (materials.Count == 0) {
            for (int i = 0; i < triangles.Count / 3; i++) materials.Add (0);
        }
        #endregion

        #region "Swap"
        for (int i = 0; i < triangles.Count; i = i + 3) {
            int[] sw = new int[2];
            sw[0] = triangles[i + 0];
            sw[1] = triangles[i + 2];
            triangles[i + 2] = sw[0];
            triangles[i + 0] = sw[1];
        }
        if (normals.Count == triangles.Count) {
            for (int i = 0; i < normals.Count; i = i + 3) {
                Vector3[] sw = new Vector3[2];
                sw[0] = normals[i + 0];
                sw[1] = normals[i + 2];
                normals[i + 2] = sw[0];
                normals[i + 0] = sw[1];
            }
        }
        if (uv.Count == triangles.Count) {
            for (int i = 0; i < uv.Count; i = i + 3) {
                Vector2[] sw = new Vector2[2];
                sw[0] = uv[i + 0];
                sw[1] = uv[i + 2];
                uv[i + 2] = sw[0];
                uv[i + 0] = sw[1];
            }
        }
        #endregion

        if (normals.Count == triangles.Count) {
            if (uv.Count > 0) {
                kFBXMesh = this.ProcessingByNormals (vertices, triangles, normals, uv, materials);
            }
            else {
                kFBXMesh = this.ProcessingByNormals (vertices, triangles, normals);
            }
        }
        else {
            kFBXMesh = this.ProcessingByUVs (vertices, triangles, normals, uv, materials);
        }


        return kFBXMesh;
    }

    private void GetMesh2010 (int n) {
        if (n == -1) {
            this.kFBXMeshes.Add (new UniFBXGeometry.KFbxMesh ());
            this.indexes.Add (this.geometryIndex++);
            return;
        }

        UniFBXGeometry.KFbxMesh kFBXMesh = new UniFBXGeometry.KFbxMesh ();
        List<Vector3> vertices = new List<Vector3> ();
        List<int> triangles = new List<int> ();
        List<int> kfbxt = new List<int> ();
        List<int> quads = new List<int> ();
        List<int> del = new List<int> ();
        List<Vector3> normals = new List<Vector3> ();
        List<Vector2> uv = new List<Vector2> ();
        List<Vector2> uvTemp = new List<Vector2> ();
        List<int> materials = new List<int> ();

        string[] data = new string[0];
        int dnVertice = 0;
        int dnTriangle = 0;
        int dnNormal = 0;
        int dnUV = 0;
        int dnUVIndex = 0;
        int dnMaterial = 0;
        int k = 0;
        bool verticesReady = false;
        bool trianglesReady = false;
        bool normalsReady = false;
        bool uvReady = false;
        bool uvIndexReady = false;
        bool materialsReady = false;

        for (int i = 0; i < UniFBX.list.Count; i++) {
            string s = UniFBX.list[n + i];

            if (s.Contains ("Vertices:") && !verticesReady) {
                #region "Vertices"
                dnVertice = i + 1;
                UniFBX.list[n + dnVertice] = UniFBX.list[n + dnVertice].Replace ("Vertices:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnVertice); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("PolygonVertexIndex:")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                int f = verticesRecord.FindIndex (x => x == group);
                verticesRecord.Add (group);
                if (f != -1) {
                    this.geometryIndex++;
                    kFBXMesh.id = this.geometryIndex.ToString ();
                    kFBXMesh.name = f.ToString ();
                    kFBXMeshes.Add (kFBXMeshes[f]);
                    this.indexes.Add (f);
                    return;
                }
                data = group.Split (',');
                for (int j = 0; j < data.Length; j = j + 3) {
                    Vector3 v = new Vector3 ();
                    v.x = -(float.Parse (data[j + 0]));
                    v.y = (float.Parse (data[j + 1]));
                    v.z = (float.Parse (data[j + 2]));
                    vertices.Add (v * this.setting.meshes.scaleFactor);
                }
                verticesReady = true;
                #endregion
            }
            else if (s.Contains ("PolygonVertexIndex:") && !trianglesReady) {
                #region "Triangles"
                dnTriangle = i + 1;
                int q = 0;
                UniFBX.list[n + dnTriangle] = UniFBX.list[n + dnTriangle].Replace ("PolygonVertexIndex:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnTriangle); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("GeometryVersion:")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j++) {
                    q++;
                    int t = int.Parse (data[j]);
                    triangles.Add (t);

                    if (t < 0 && q == 3) {
                        q = 0;
                        t = -t - 1;
                        triangles[j] = t;
                    }
                    else if (t < 0 && q == 4) {
                        q = 0;
                        t = -t - 1;
                        triangles[j] = t;
                        kfbxt.Add (triangles[j - 3]);
                        kfbxt.Add (triangles[j - 2]);
                        kfbxt.Add (triangles[j - 0]);
                        quads.Add (j - 3);
                        quads.Add (j - 2);
                        quads.Add (j - 0);
                        del.Add (j - 3);
                    }
                    else if (q > 4) {
                        this.setting.Status = FBXStatus.NGonsNotSupported;
                        return;
                    }
                }
                triangles.AddRange (kfbxt);
                for (int j = 0; j < del.Count; j++) {
                    if (del[j] < triangles.Count) triangles.RemoveAt (del[j] - (k++)); else break;
                }
                trianglesReady = true;
                #endregion
            }
            else if (s.Contains ("Normals:") && !normalsReady) {
                #region "Normales"
                dnNormal = i + 1;
                UniFBX.list[n + dnNormal] = UniFBX.list[n + dnNormal].Replace ("Normals:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnNormal); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j = j + 3) {
                    float x = float.Parse (data[j + 0]);
                    float y = float.Parse (data[j + 1]);
                    float z = float.Parse (data[j + 2]);
                    normals.Add (new Vector3 (-x, y, z));
                }
                bool isByVertice = UniFBX.list[n + dnNormal - 1].Contains ("ByVertice");
                if (del.Count > 0 && isByVertice == false) {
                    k = 0;
                    for (int j = 0; j < quads.Count; j++) {
                        if (quads[j] < normals.Count) normals.Add (normals[quads[j]]); else break;
                    }
                    for (int j = 0; j < del.Count; j++) {
                        if (del[j] < normals.Count) normals.RemoveAt (del[j] - (k++)); else break;
                    }
                }
                normalsReady = true;
                #endregion
            }
            else if (s.Contains ("UV:") && !uvReady) {
                #region "UV"
                dnUV = i + 1;
                UniFBX.list[n + dnUV] = UniFBX.list[n + dnUV].Replace ("UV:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnUV); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("UVIndex:")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j = j + 2) {
                    float x = float.Parse (data[j + 0]);
                    float y = float.Parse (data[j + 1]);
                    uv.Add (new Vector2 (x, y));
                }
                uvReady = true;
                #endregion
            }
            else if (s.Contains ("UVIndex:") && !uvIndexReady) {
                #region "UV Index"
                dnUVIndex = i + 1;
                UniFBX.list[n + dnUVIndex] = UniFBX.list[n + dnUVIndex].Replace ("UVIndex:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnUVIndex); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j++) {
                    uvTemp.Add (uv[int.Parse (data[j])]);
                }
                if (del.Count > 0) {
                    k = 0;
                    for (int j = 0; j < quads.Count; j++) {
                        if (quads[j] < uvTemp.Count) uvTemp.Add (uvTemp[quads[j]]); else break;
                    }
                    for (int j = 0; j < del.Count; j++) {
                        if (del[j] < uvTemp.Count) uvTemp.RemoveAt (del[j] - (k++)); else break;
                    }
                }
                uv = uvTemp;
                uvIndexReady = true;
                #endregion
            }
            else if (s.Contains ("Materials:") && !materialsReady) {
                #region "Materials"
                dnMaterial = i + 1;
                UniFBX.list[n + dnMaterial] = UniFBX.list[n + dnMaterial].Replace ("Materials:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnMaterial); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                if (data.Length == 1) {
                    int mtl = 0;
                    int.TryParse (data[0], out mtl);
                    for (int j = 0; j < triangles.Count / 3; j++) materials.Add (mtl);
                }
                else {
                    for (int j = 0; j < data.Length; j++) {
                        materials.Add (int.Parse (data[j]));
                    }
                    if (del.Count > 0) {
                        int Len = (materials.Count - del.Count) / 2;
                        for (int j = 0; j < Len; j++) materials.RemoveAt (materials.Count - 1);
                        materials.AddRange (materials);
                    }
                }
                if (materials.Count == 0) {
                    for (int j = 0; j < triangles.Count / 3; j++) materials.Add (0);
                }
                materialsReady = true;
                #endregion
            }
            else if (s.Contains ("Layer:")) {
                break;
            }
        }

        #region "Swap"
        for (int i = 0; i < triangles.Count; i = i + 3) {
            int[] sw = new int[2];
            sw[0] = triangles[i + 0];
            sw[1] = triangles[i + 2];
            triangles[i + 2] = sw[0];
            triangles[i + 0] = sw[1];
        }
        if (normals.Count == triangles.Count) {
            for (int i = 0; i < normals.Count; i = i + 3) {
                Vector3[] sw = new Vector3[2];
                sw[0] = normals[i + 0];
                sw[1] = normals[i + 2];
                normals[i + 2] = sw[0];
                normals[i + 0] = sw[1];
            }
        }
        if (uv.Count == triangles.Count) {
            for (int i = 0; i < uv.Count; i = i + 3) {
                Vector2[] sw = new Vector2[2];
                sw[0] = uv[i + 0];
                sw[1] = uv[i + 2];
                uv[i + 2] = sw[0];
                uv[i + 0] = sw[1];
            }
        }
        #endregion


        if (normals.Count == triangles.Count) {
            if (uv.Count > 0) {
                kFBXMesh = this.ProcessingByNormals (vertices, triangles, normals, uv, materials);
            }
            else {
                kFBXMesh = this.ProcessingByNormals (vertices, triangles, normals);
            }
        }
        else {
            kFBXMesh = this.ProcessingByUVs (vertices, triangles, normals, uv, materials);
        }


        kFBXMesh.id = this.geometryIndex.ToString ();
        this.kFBXMeshes.Add (kFBXMesh);
        this.indexes.Add (this.geometryIndex++);
    }

    private void GetMesh (int n) {
        if (n == -1) {
            this.kFBXMeshes.Add (new UniFBXGeometry.KFbxMesh ());
            this.indexes.Add (this.geometryIndex++);
            return;
        }

        UniFBXGeometry.KFbxMesh kFBXMesh = new UniFBXGeometry.KFbxMesh ();
        List<Vector3> vertices = new List<Vector3> ();
        List<int> triangles = new List<int> ();
        List<int> kfbxt = new List<int> ();
        List<int> quads = new List<int> ();
        List<int> del = new List<int> ();
        List<Vector3> normals = new List<Vector3> ();
        List<Vector2> uv = new List<Vector2> ();
        List<Vector2> uvTemp = new List<Vector2> ();
        List<int> materials = new List<int> ();

        string[] data = new string[0];
        int dnVertice = 0;
        int dnTriangle = 0;
        int dnNormal = 0;
        int dnUV = 0;
        int dnUVIndex = 0;
        int dnMaterial = 0;
        int k = 0;
        bool verticesReady = false;
        bool trianglesReady = false;
        bool normalsReady = false;
        bool uvReady = false;
        bool uvIndexReady = false;
        bool materialsReady = false;

        for (int i = 0; i < UniFBX.list.Count; i++) {
            string s = UniFBX.list[n + i];
            //Debug.Log(s);
            if (s.Contains ("Vertices: *") && !verticesReady) {
                #region "Vertices"
                dnVertice = i + 1;
                UniFBX.list[n + dnVertice] = UniFBX.list[n + dnVertice].Replace ("a:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnVertice); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                Debug.Log(group);
                sb = null;
                int f = verticesRecord.FindIndex (x => x == group);
                
                verticesRecord.Add (group);
                if (f != -1) {
                    this.geometryIndex++;
                    kFBXMesh.id = this.geometryIndex.ToString ();
                    kFBXMesh.name = f.ToString ();
                    Debug.Log(f + "wwwwwwwwwwwwwwwww" + kFBXMeshes.Count);
                    kFBXMeshes.Add (kFBXMeshes[f]);
                    this.indexes.Add (f);
                    return;
                }
                data = group.Split (',');
                for (int j = 0; j < data.Length; j = j + 3) {
                    Vector3 v = new Vector3 ();
                    v.x = -(float.Parse (data[j + 0]));
                    v.y = (float.Parse (data[j + 1]));
                    v.z = (float.Parse (data[j + 2]));
                    vertices.Add (v * this.setting.meshes.scaleFactor);
                }
                verticesReady = true;
                #endregion
            }
            else if (s.Contains ("PolygonVertexIndex: *") && !trianglesReady) {
                #region "Triangles"                
                dnTriangle = i + 1;
                int q = 0;
                UniFBX.list[n + dnTriangle] = UniFBX.list[n + dnTriangle].Replace ("a:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnTriangle); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;                
                data = group.Split (',');
                for (int j = 0; j < data.Length; j++) {
                    q++;
                    int t = int.Parse (data[j]);
                    triangles.Add (t);

                    if (t < 0 && q == 3) {
                        q = 0;
                        t = -t - 1;
                        triangles[j] = t;
                    }
                    else if (t < 0 && q == 4) {
                        q = 0;
                        t = -t - 1;
                        triangles[j] = t;
                        kfbxt.Add (triangles[j - 3]);
                        kfbxt.Add (triangles[j - 2]);
                        kfbxt.Add (triangles[j - 0]);
                        quads.Add (j - 3);
                        quads.Add (j - 2);
                        quads.Add (j - 0);
                        del.Add (j - 3);
                    }
                    else if (q > 4) {                        
                        this.setting.Status = FBXStatus.NGonsNotSupported;
                        return;
                    }
                }
                triangles.AddRange (kfbxt);                
                for (int j = 0; j < del.Count; j++) {
                    if (del[j] < triangles.Count) triangles.RemoveAt (del[j] - (k++)); else break;
                }
                trianglesReady = true;                
                #endregion
            }
            else if (s.Contains ("Normals: *") && !normalsReady) {
                #region "Normales"
                dnNormal = i + 1;
                UniFBX.list[n + dnNormal] = UniFBX.list[n + dnNormal].Replace ("a:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnNormal); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j = j + 3) {
                    float x = float.Parse (data[j + 0]);
                    float y = float.Parse (data[j + 1]);
                    float z = float.Parse (data[j + 2]);
                    normals.Add (new Vector3 (-x, y, z));
                }
                bool isByVertice = UniFBX.list[n + dnNormal - 1].Contains ("ByVertice");
                if (del.Count > 0 && isByVertice == false) {
                    k = 0;
                    for (int j = 0; j < quads.Count; j++) {
                        if (quads[j] < normals.Count) normals.Add (normals[quads[j]]); else break;
                    }
                    for (int j = 0; j < del.Count; j++) {
                        if (del[j] < normals.Count) normals.RemoveAt (del[j] - (k++)); else break;
                    }
                }
                normalsReady = true;
                #endregion
            }
            else if (s.Contains ("UV: *") && !uvReady) {
                #region "UV"
                dnUV = i + 1;
                UniFBX.list[n + dnUV] = UniFBX.list[n + dnUV].Replace ("a:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnUV); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j = j + 2) {
                    float x = float.Parse (data[j + 0]);
                    float y = float.Parse (data[j + 1]);
                    uv.Add (new Vector2 (x, y));
                }
                uvReady = true;
                #endregion
            }
            else if (s.Contains ("UVIndex: *") && !uvIndexReady) {
                #region "UV Index"
                dnUVIndex = i + 1;
                UniFBX.list[n + dnUVIndex] = UniFBX.list[n + dnUVIndex].Replace ("a:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnUVIndex); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                for (int j = 0; j < data.Length; j++) {
                    uvTemp.Add (uv[int.Parse (data[j])]);
                }
                if (del.Count > 0) {
                    k = 0;
                    for (int j = 0; j < quads.Count; j++) {
                        if (quads[j] < uvTemp.Count) uvTemp.Add (uvTemp[quads[j]]); else break;
                    }
                    for (int j = 0; j < del.Count; j++) {
                        if (del[j] < uvTemp.Count) uvTemp.RemoveAt (del[j] - (k++)); else break;
                    }
                }
                uv = uvTemp;
                uvIndexReady = true;
                #endregion
            }
            else if (s.Contains ("Materials: *") && !materialsReady) {
                #region "Materials"
                dnMaterial = i + 1;
                UniFBX.list[n + dnMaterial] = UniFBX.list[n + dnMaterial].Replace ("a:", "").Trim ();
                System.Text.StringBuilder sb = new System.Text.StringBuilder ();
                for (int j = (n + dnMaterial); j < UniFBX.list.Count; j++) {
                    if (UniFBX.list[j].Contains ("}")) break;
                    sb.Append (UniFBX.list[j]);
                }

                string group = sb.ToString ();
                sb = null;
                data = group.Split (',');
                if (data.Length == 1) {
                    int mtl = 0;
                    int.TryParse (data[0], out mtl);
                    for (int j = 0; j < triangles.Count / 3; j++) materials.Add (mtl);
                }
                else {
                    for (int j = 0; j < data.Length; j++) {
                        materials.Add (int.Parse (data[j]));
                    }
                    if (del.Count > 0) {
                        int Len = (materials.Count - del.Count) / 2;
                        for (int j = 0; j < Len; j++) materials.RemoveAt (materials.Count - 1);
                        materials.AddRange (materials);
                    }
                }
                if (materials.Count == 0) {
                    for (int j = 0; j < triangles.Count / 3; j++) materials.Add (0);
                }
                materialsReady = true;
                #endregion
            }
            else if (s.Contains ("Layer:")) {
                break;
            }
        }

        #region "Swap"
        for (int i = 0; i < triangles.Count; i = i + 3) {
            int[] sw = new int[2];
            sw[0] = triangles[i + 0];
            sw[1] = triangles[i + 2];
            triangles[i + 2] = sw[0];
            triangles[i + 0] = sw[1];
        }
        if (normals.Count == triangles.Count) {
            for (int i = 0; i < normals.Count; i = i + 3) {
                Vector3[] sw = new Vector3[2];
                sw[0] = normals[i + 0];
                sw[1] = normals[i + 2];
                normals[i + 2] = sw[0];
                normals[i + 0] = sw[1];
            }
        }
        if (uv.Count == triangles.Count) {
            for (int i = 0; i < uv.Count; i = i + 3) {
                Vector2[] sw = new Vector2[2];
                sw[0] = uv[i + 0];
                sw[1] = uv[i + 2];
                uv[i + 2] = sw[0];
                uv[i + 0] = sw[1];
            }
        }
        #endregion


        if (normals.Count == triangles.Count) {
            if (uv.Count > 0) {
                kFBXMesh = this.ProcessingByNormals (vertices, triangles, normals, uv, materials);
            }
            else {
                kFBXMesh = this.ProcessingByNormals (vertices, triangles, normals);
            }
        }
        else {
            if (uv.Count > 0) {
                kFBXMesh = this.ProcessingByUVs (vertices, triangles, normals, uv, materials);
            }
            else {
                kFBXMesh = this.ProcessingByUVs (vertices, triangles, normals, materials);
            }
        }
        
        kFBXMesh.id = this.geometryIndex.ToString ();
        this.kFBXMeshes.Add (kFBXMesh);
        Debug.Log(kFBXMeshes.Count);
        this.indexes.Add (this.geometryIndex++);
    }

    private UniFBXGeometry.KFbxMesh ProcessingByNormals (List<Vector3> vs, List<int> ts, List<Vector3> ns, List<Vector2> us, List<int> ms) {
        List<Vector3> nn = new List<Vector3> ();
        List<Vector2> uv = new List<Vector2> ();
        //List<int> newVertexIndexes = new List<int> ();
        //List<int> newVertexWeights = new List<int> ();

        int MAX = Mathf.Max (ts.ToArray ());
        int TRIANGLE_SIZE = MAX + 1;
        for (int i = 0; i < TRIANGLE_SIZE; i++) {
            int j = 0;
            for (int k = 0; k < ts.Count; k++) {
                if (ts[k] == i) {
                    j = k;
                    break;
                }
            }
            nn.Add (ns[j]);
            uv.Add (us[j]);
        }

        if (this.setting.meshes.meshMethode == ImportMethode.Unity) {
            //int MAX_VERTICES = vs.Count - 1;
            //int skip = 1;
            for (int i = 0; i < ts.Count; i++) {
                if (ns[i] != nn[ts[i]] || us[i] != uv[ts[i]]) {
                    //if (vs.Count > (skip * MAX_VERTICES)) skip++;
                    //newVertexIndexes.Add (ts[i] * skip);
                    //newVertexWeights.Add (ts[i]);
                    vs.Add (vs[ts[i]]);
                    nn.Add (ns[i]);
                    uv.Add (us[i]);
                    ns[i] = nn[ts[i]];
                    us[i] = uv[ts[i]];
                    int inc = MAX + 1;
                    ts[i] = inc;
                    MAX = inc;
                    if (vs.Count >= 64000) break;
                }
            }
        }
        ns = nn;
        us = uv;

        UniFBXGeometry.KFbxMesh temp = new UniFBXGeometry.KFbxMesh ();
        temp.vertices = vs;
        //temp.newVeretexIndexes = newVertexIndexes;
        //temp.newVeretexWeights = newVertexWeights;
        temp.normals = ns;
        temp.uvs = us;

        //Debug.Log (vs.Count + " :: " + ns.Count + " :: " + us.Count);
        if (this.setting.textures.normalmaps) {
            if (vs.Count == us.Count) temp.tangents = this.GetTangents (vs, ts, ns, us);
        }
        temp.submeshes = this.GetSubmeshes (ts, ms);
        return temp;
    }

    private UniFBXGeometry.KFbxMesh ProcessingByNormals (List<Vector3> vs, List<int> ts, List<Vector3> ns) {
        List<Vector3> nn = new List<Vector3> ();

        int MAX = Mathf.Max (ts.ToArray ());
        int TRIANGLE_SIZE = MAX + 1;
        for (int i = 0; i < TRIANGLE_SIZE; i++) {
            int j = ts.IndexOf (i);
            nn.Add (ns[j]);
        }

        for (int i = 0; i < ts.Count; i++) {
            if (ns[i] != nn[ts[i]]) {
                vs.Add (vs[ts[i]]);
                nn.Add (ns[i]);
                int inc = MAX + 1;
                ts[i] = inc;
                MAX = inc;
                if (vs.Count >= 64000) break;
            }
        }
        ns = nn;

        UniFBXGeometry.KFbxMesh temp = new UniFBXGeometry.KFbxMesh ();
        temp.vertices = vs;
        temp.triangles = ts;
        temp.normals = ns;
        if (this.setting.textures.normalmaps) {
            if (vs.Count == ns.Count) temp.tangents = this.GetTangents (vs, ts, ns);
        }
        return temp;
    }

    private UniFBXGeometry.KFbxMesh ProcessingByUVs (List<Vector3> vs, List<int> ts, List<Vector3> ns, List<Vector2> us, List<int> ms) {
        List<Vector2> uv = new List<Vector2> ();

        int MAX = Mathf.Max (ts.ToArray ());
        int TRIANGLE_SIZE = MAX + 1;
        if (us.Count > 0) {
            for (int i = 0; i < TRIANGLE_SIZE; i++) {
                int j = Mathf.Abs (ts.IndexOf (i));
                uv.Add (us[j]);
            }

            if (this.setting.meshes.meshMethode == ImportMethode.Unity) {
                for (int i = 0; i < ts.Count; i++) {
                    if (us[i] != uv[ts[i]]) {
                        vs.Add (vs[ts[i]]);
                        if (ns.Count > 0) ns.Add (ns[ts[i]]);
                        uv.Add (us[i]);
                        int inc = MAX + 1;
                        ts[i] = inc;
                        MAX = inc;
                        if (vs.Count >= 64000) break;
                    }
                }
            }
        }
        us = uv;

        UniFBXGeometry.KFbxMesh temp = new UniFBXGeometry.KFbxMesh ();
        temp.vertices = vs;
        temp.normals = ns;
        temp.uvs = us;
        if (this.setting.textures.normalmaps) {
            if (vs.Count == us.Count) temp.tangents = this.GetTangents (vs, ts, ns, us);
        }
        temp.submeshes = this.GetSubmeshes (ts, ms);

        return temp;
    }

    private UniFBXGeometry.KFbxMesh ProcessingByUVs (List<Vector3> vs, List<int> ts, List<Vector3> ns, List<int> ms) {
        UniFBXGeometry.KFbxMesh temp = new UniFBXGeometry.KFbxMesh ();
        temp.vertices = vs;
        temp.normals = ns;
        if (this.setting.textures.normalmaps) {
            if (vs.Count == ns.Count) temp.tangents = this.GetTangents (vs, ts, ns);
        }
        temp.submeshes = this.GetSubmeshes (ts, ms);
        return temp;
    }

    private List<Vector4> GetTangents (List<Vector3> vs, List<int> ts, List<Vector3> ns, List<Vector2> us) {
        List<Vector4> tangents = new List<Vector4> ();
        Vector3[] tan1 = new Vector3[vs.Count];
        Vector3[] tan2 = new Vector3[vs.Count];

        for (int i = 0; i < ts.Count; i = i + 3) {
            int i1 = ts[i + 0];
            int i2 = ts[i + 1];
            int i3 = ts[i + 2];

            Vector3 v1 = vs[i1];
            Vector3 v2 = vs[i2];
            Vector3 v3 = vs[i3];

            Vector2 w1 = us[i1];
            Vector2 w2 = us[i2];
            Vector2 w3 = us[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;

            float r = 1.0f / (s1 * t2 - s2 * t1);
            Vector3 sdir = new Vector3 ((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3 ((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }

        for (int i = 0; i < vs.Count; i++) {
            Vector3 n = ns[i];
            Vector3 t = tan1[i];
            Vector3.OrthoNormalize (ref n, ref t);

            Vector4 temp = new Vector4 ();
            temp.x = t.x;
            temp.y = t.y;
            temp.z = t.z;
            temp.w = (Vector3.Dot (Vector3.Cross (n, t), tan2[i]) < 0) ? 0 : 1;
            tangents.Add (temp);
        }

        return tangents;
    }

    private List<Vector4> GetTangents (List<Vector3> vs, List<int> ts, List<Vector3> ns) {
        List<Vector4> tangents = new List<Vector4> ();
        Vector3[] tan1 = new Vector3[vs.Count];
        Vector3[] tan2 = new Vector3[vs.Count];
        Vector2[] us = new Vector2[vs.Count];

        for (int i = 0; i < ts.Count; i = i + 3) {
            int i1 = ts[i + 0];
            int i2 = ts[i + 1];
            int i3 = ts[i + 2];

            Vector3 v1 = vs[i1];
            Vector3 v2 = vs[i2];
            Vector3 v3 = vs[i3];

            Vector2 w1 = us[i1];
            Vector2 w2 = us[i2];
            Vector2 w3 = us[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;

            float r = 1.0f / (s1 * t2 - s2 * t1);
            Vector3 sdir = new Vector3 ((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3 ((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }


        for (int i = 0; i < vs.Count; i++) {
            Vector3 n = ns[i];
            Vector3 t = tan1[i];
            Vector3.OrthoNormalize (ref n, ref t);

            Vector4 temp = new Vector4 ();
            temp.x = t.x;
            temp.y = t.y;
            temp.z = t.z;
            temp.w = (Vector3.Dot (Vector3.Cross (n, t), tan2[i]) < 0) ? 0 : 1;
            tangents.Add (temp);
        }

        return tangents;
    }

    private UniFBXGeometry.Submeshes[] GetSubmeshes (List<int> ts, List<int> ms) {
        if (ms.Count == 0) {
            UniFBXGeometry.Submeshes[] submesh = new UniFBXGeometry.Submeshes[1];
            submesh[0].triangles = new List<int> ();
            submesh[0].triangles.AddRange (ts);
            return submesh;
        }

        int MAX_SUBMEHSES = Mathf.Max (ms.ToArray ()) + 1;
        if (MAX_SUBMEHSES == 0) {
            UniFBXGeometry.Submeshes[] submesh = new UniFBXGeometry.Submeshes[1];
            submesh[0].triangles = new List<int> ();
            submesh[0].triangles.AddRange (ts);
            return submesh;
        }
        else {
            UniFBXGeometry.Submeshes[] submeshes = new UniFBXGeometry.Submeshes[MAX_SUBMEHSES];
            for (int i = 0; i < submeshes.Length; i++) submeshes[i].triangles = new List<int> ();
            int skip = 0;
            for (int j = 0; j < ts.Count; j = j + 3) {
                int k = ms[skip++];
                k = Mathf.Abs (k);
                if (k < submeshes.Length) {
                    submeshes[k].triangles.Add (ts[j + 0]);
                    submeshes[k].triangles.Add (ts[j + 1]);
                    submeshes[k].triangles.Add (ts[j + 2]);
                }
            }

            return submeshes;
        }
    }

    #endregion

}