using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolygonTool;

public class BrushMesh : MonoBehaviour
{

    private Triangulation triangulation;

    private List<int> resultList = new List<int>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 创建地面
    /// </summary>
    /// <param name="posList"></param>
    /// <param name="reverse"></param>
    public void BrushOneMesh(List<Vector3> posList, bool reverse = false)
    {
        triangulation = new Triangulation(posList);
        triangulation.SetCompareAxle(CompareAxle.Y);
        int[] a = triangulation.GetTriangles();

        if (a != null)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Debug.Log("===" + a[i]);
                resultList.Add(a[i]);
            }
        }
        else if (a == null && !reverse)
        {
            List<Vector3> reversePosList = new List<Vector3>();
            for (int i = posList.Count - 1; i >= 0; i--)
            {
                reversePosList.Add(posList[i]);
            }
            BrushOneMesh(reversePosList, true);
            return;
        }
        else if (a == null && reverse)
        {
            Debug.LogError("这个图形可能不是简单多边形");
            return;
        }

        GameObject go = new GameObject("Ground");
        go.transform.SetParent(BrushManager.Instance.BrushParent);
        go.layer = 13;
        go.tag = "BrushGround";
        MeshFilter mf = go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BrushWallMaterials/qipange");
        Mesh m = new Mesh();
        Vector3[] vertexs = new Vector3[posList.Count];

        for (int i = 0; i < vertexs.Length; i++)
        {
            Vector3 v = posList[i];
            vertexs[i] = v;
        }
        m.vertices = vertexs;
        int[] tri = new int[a.Length];
        for (int i = 0; i < tri.Length; i ++)
        {
            tri[i] = a[i];
        }

        #region UV
        Vector2[] uvs = new Vector2[posList.Count];
        for (int i = 0; i < posList.Count; i++)
        {
            uvs[i] = new Vector2(posList[i].x, posList[i].z);
        }
        m.uv = uvs;
        #endregion
        m.triangles = tri;

        mf.mesh = m;
    }


    public Mesh GetMeshByPoints(List<Vector3> posList, bool reverse = false)
    {
        triangulation = new Triangulation(posList);
        triangulation.SetCompareAxle(CompareAxle.Y);
        int[] a = triangulation.GetTriangles();

        if (a != null)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Debug.Log("===" + a[i]);
                resultList.Add(a[i]);
            }
        }
        else if (a == null && !reverse)
        {
            List<Vector3> reversePosList = new List<Vector3>();
            for (int i = posList.Count - 1; i >= 0; i--)
            {
                reversePosList.Add(posList[i]);
            }
            return GetMeshByPoints(reversePosList, true);
        }
        else if (a == null && reverse)
        {
            Debug.LogError("这个图形可能不是简单多边形");
            return null;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = posList.ToArray();
        mesh.triangles = a;
        #region UV
        Vector2[] uvs = new Vector2[posList.Count];
        for (int i = 0; i < posList.Count; i++)
        {
            uvs[i] = new Vector2(posList[i].x, posList[i].z);
        }
        mesh.uv = uvs;
        #endregion

        return mesh;
    }
}
