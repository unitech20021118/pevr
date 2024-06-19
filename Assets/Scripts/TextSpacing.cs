using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[AddComponentMenu("UI/Effects/TextSpacing")]
public class TextSpacing : BaseMeshEffect {

	public float _textSpacing=1f;
	public override void ModifyMesh (VertexHelper vh)
	{
		if (!IsActive()|| vh.currentIndexCount == 0) {
			return;
		}
		List<UIVertex> vertexs = new List<UIVertex> ();
		vh.GetUIVertexStream (vertexs);
		int indexCount = vh.currentIndexCount;
		UIVertex vt;
		for (int i = 0; i < indexCount; i++) {
			vt = vertexs [i];
			vt.position += new Vector3 (_textSpacing * (i / 6), 0, 0);
			vertexs [i] = vt;
			if (i % 6 <= 2) {
				vh.SetUIVertex (vt, (i / 6) * 4 + i % 6);
			}
			if (i % 6 == 4) {
				vh.SetUIVertex (vt, (i / 6) * 4 + i % 6 - 1);
			}
		}
	}
}
