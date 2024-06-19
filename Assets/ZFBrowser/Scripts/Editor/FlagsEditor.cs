using UnityEditor;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {

[CustomPropertyDrawer(typeof(FlagsFieldAttribute))]
public class FlagsEditor : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
	}
}

}
