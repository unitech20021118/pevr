/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Effects/Gradient")]
#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
    public class Gradient : BaseMeshEffect
#else
    public class Gradient : BaseVertexEffect
#endif
    {
        [SerializeField] private Color topColor = Color.white;
		[SerializeField] private Color bottomColor = Color.black;

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (!this.IsActive())
                return;

            List<UIVertex> list = new List<UIVertex>();
            vertexHelper.GetUIVertexStream(list);

            ModifyVertices(list);  // calls the old ModifyVertices which was used on pre 5.2

			vertexHelper.Clear();
            vertexHelper.AddUIVertexTriangleStream(list);
        }
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
        public void ModifyVertices(List<UIVertex> vertexList)
#else
        public override void ModifyVertices(List<UIVertex> vertexList)
#endif
        {
            if (!this.IsActive())
				return;
			
			int count = vertexList.Count;
			float bottomY = vertexList[0].position.y;
			float topY = vertexList[0].position.y;
			
			for (int i = 1; i < count; i++)
			{
				float y = vertexList[i].position.y;
				if (y > topY)
				{
					topY = y;
				}
				else if (y < bottomY)
				{
					bottomY = y;
				}
			}
			
			float uiElementHeight = topY - bottomY;
			
			for (int i = 0; i < count; i++)
			{
				UIVertex uiVertex = vertexList[i];
				uiVertex.color = uiVertex.color * Color.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
				vertexList[i] = uiVertex;
			}
		}
	}
}
