/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace UnityEditor.UI
{
	[CanEditMultipleObjects, CustomEditor(typeof(UICircularRaycastFilter))]
	public class UICircularRaycastFilterEditor : Editor {
		
		public const string PREFS_KEY = "UICircleRaycastFilter_DG";
		private bool m_DisplayGeometry = true;
		
		protected void OnEnable()
		{
			this.m_DisplayGeometry = EditorPrefs.GetBool(PREFS_KEY, true);
		}
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			GUI.changed = false;
			
			this.m_DisplayGeometry = EditorGUILayout.Toggle("Display Geometry", this.m_DisplayGeometry);
			
			if (GUI.changed)
			{
				EditorPrefs.SetBool(PREFS_KEY, this.m_DisplayGeometry);
				EditorUtility.SetDirty(target);
			}
		}
		
		protected void OnSceneGUI()
		{
			if (!this.m_DisplayGeometry)
				return;
			
			UICircularRaycastFilter filter = this.target as UICircularRaycastFilter;
			RectTransform rt = filter.transform as RectTransform;
			
			if (filter.operationalRadius == 0f)
				return;
			
			float radius = filter.operationalRadius;
			Vector3 offset = new Vector3(rt.rect.center.x, rt.rect.center.y, 0f) + new Vector3(filter.offset.x, filter.offset.y, 0f);
			
			Canvas canvas = UIUtility.FindInParents<Canvas>(filter.gameObject);
			if (canvas != null)
			{
				radius *= canvas.scaleFactor;
				offset *= canvas.scaleFactor;
			}
			
			Handles.color = Color.green;
			Handles.CircleCap(0,
			                  filter.transform.position + offset,
			                  filter.transform.rotation,
			                  radius);
		}
	}
}
