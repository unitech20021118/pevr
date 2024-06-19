/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Color))]
public class ColorPropertyDrawer : PropertyDrawer
{
	private const float textFieldWidth = 86f;
	private const float spacing = 5f;

	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
	{
		label = EditorGUI.BeginProperty(pos, label, prop);

		// Draw label
		pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
		
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		Color32 color = prop.colorValue;
		Color32 color2 = EditorGUI.ColorField(new Rect(pos.x, pos.y, (pos.width - textFieldWidth - spacing), pos.height), prop.colorValue);

		if (!color2.Equals(color))
		{
			prop.colorValue = color = color2;
		}

		string colorString = EditorGUI.TextField(new Rect((pos.x + (pos.width - textFieldWidth) + spacing), pos.y, (textFieldWidth - spacing - 5f), pos.height), CommonColorBuffer.ColorToString(color));
		try
		{
			color2 = CommonColorBuffer.StringToColor(colorString);
			if (!color2.Equals(color))
			{
				prop.colorValue = color = color2;
			}
		}
		catch
		{
		}

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
