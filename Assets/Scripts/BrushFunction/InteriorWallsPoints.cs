using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorWallsPoints : MonoBehaviour {

    private Vector3[] wallPoints;
	public void SetWallPoints(Vector3[] vector3s)
    {
        wallPoints = new Vector3[vector3s.Length];
        for (int i = 0; i < wallPoints.Length; i++)
        {
            wallPoints[i] = new Vector3(vector3s[i].x, 0, vector3s[i].z);
        }
    }
    public Vector3[] GetWallPoints()
    {
        return wallPoints;
    }
}
