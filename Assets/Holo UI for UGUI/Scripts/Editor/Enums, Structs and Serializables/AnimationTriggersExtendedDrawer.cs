/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
	[CustomPropertyDrawer(typeof(AnimationTriggersExtended), true)]
	public class AnimationTriggersExtendedDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
		{
			Rect position = rect;
			position.height = EditorGUIUtility.singleLineHeight;
			SerializedProperty property = prop.FindPropertyRelative("m_NormalTrigger");
			SerializedProperty property2 = prop.FindPropertyRelative("m_HighlightedTrigger");
			SerializedProperty property3 = prop.FindPropertyRelative("m_PressedTrigger");
			SerializedProperty property4 = prop.FindPropertyRelative("m_ActiveTrigger");
			SerializedProperty property5 = prop.FindPropertyRelative("m_ActiveHighlightedTrigger");
			SerializedProperty property6 = prop.FindPropertyRelative("m_ActivePressedTrigger");
			SerializedProperty property7 = prop.FindPropertyRelative("m_DisabledTrigger");
			
			EditorGUI.PropertyField(position, property);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property2);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property3);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property4);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property5);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property6);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, property7);
		}
		
		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
		{
			return 7f * EditorGUIUtility.singleLineHeight + 6f * EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
