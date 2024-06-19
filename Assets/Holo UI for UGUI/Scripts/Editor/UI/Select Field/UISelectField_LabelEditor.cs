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
	[CanEditMultipleObjects, CustomEditor(typeof(UISelectField_Label), true)]
	public class UISelectField_LabelEditor : Editor {

		private SerializedProperty m_TextComponentProperty;
		private SerializedProperty m_TransitionTypeProperty;
		private SerializedProperty m_ColorsProperty;
		
		public void OnEnable()
		{
			this.m_TextComponentProperty = this.serializedObject.FindProperty("textComponent");
			this.m_TransitionTypeProperty = this.serializedObject.FindProperty("transitionType");
			this.m_ColorsProperty = this.serializedObject.FindProperty("colors");
		}
		
		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.m_TextComponentProperty, new GUIContent("Text Component"));
			
			if (this.m_TextComponentProperty.objectReferenceValue != null)
			{
				UISelectField_Label.TransitionType transition = (UISelectField_Label.TransitionType)this.m_TransitionTypeProperty.enumValueIndex;
				
				EditorGUILayout.PropertyField(this.m_TransitionTypeProperty, new GUIContent("Transition"));
				
				if (transition == UISelectField_Label.TransitionType.CrossFade)
				{
					EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(this.m_ColorsProperty, new GUIContent("Colors"), true);
					if (EditorGUI.EndChangeCheck())
						(this.m_TextComponentProperty.objectReferenceValue as UnityEngine.UI.Text).canvasRenderer.SetColor(this.m_ColorsProperty.FindPropertyRelative("m_NormalColor").colorValue);
					
					EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
				}
			}
			
			this.serializedObject.ApplyModifiedProperties();
		}
	}
}
