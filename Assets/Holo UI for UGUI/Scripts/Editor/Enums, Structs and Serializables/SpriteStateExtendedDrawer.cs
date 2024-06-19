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
	[CustomPropertyDrawer(typeof(SpriteStateExtended), true)]
	public class SpriteStateExtendedDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
		{
			Rect position = rect;
			position.height = EditorGUIUtility.singleLineHeight;
			SerializedProperty property = prop.FindPropertyRelative("m_HighlightedSprite");
			SerializedProperty property2 = prop.FindPropertyRelative("m_PressedSprite");
			SerializedProperty property3 = prop.FindPropertyRelative("m_ActiveSprite");
			SerializedProperty property4 = prop.FindPropertyRelative("m_ActiveHighlightedSprite");
			SerializedProperty property5 = prop.FindPropertyRelative("m_ActivePressedSprite");
			SerializedProperty property6 = prop.FindPropertyRelative("m_DisabledSprite");
			
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
		}
		
		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
		{
			return 6f * EditorGUIUtility.singleLineHeight + 5f * EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
