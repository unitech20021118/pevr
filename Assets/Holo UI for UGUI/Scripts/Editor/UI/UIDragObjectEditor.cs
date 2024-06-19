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
    [CustomEditor(typeof(UIDragObject), true)]
    public class UIDragObjectEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Target"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Horizontal"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Vertical"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Inertia", EditorStyles.boldLabel);
            EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Inertia"), new GUIContent("Enable"));
            if (this.serializedObject.FindProperty("m_Inertia").boolValue)
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_DampeningRate"));
            }
            EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Constrain Within Canvas", EditorStyles.boldLabel);
            EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_ConstrainWithinCanvas"), new GUIContent("Enable"));
            if (this.serializedObject.FindProperty("m_ConstrainWithinCanvas").boolValue)
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_ConstrainDrag"), new GUIContent("Constrain Drag"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_ConstrainInertia"), new GUIContent("Constrain Inertia"));
            }
            EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
