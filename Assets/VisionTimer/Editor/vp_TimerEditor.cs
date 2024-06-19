/////////////////////////////////////////////////////////////////////////////////
//
//	vp_TimerEditor.cs
//	© VisionPunk, Minea Softworks. All Rights Reserved.
//
//	description:	vp_TimerEditor is a CustomEditor for the vp_Timer class.
//					when 'vp_Timer.cs' is compiled with the 'DEBUG' define, a
//					gameobject will appear in the Hierarchy at runtime. when
//					the gameobject is selected, this class will display debug
//					info in the Inspector, including created, active and
//					inactive events and more.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(vp_Timer))]


public class vp_TimerEditor : Editor
{

	private bool m_ShowId = false;
	private bool m_ShowCallStack = false;
	private static GUIStyle m_SmallTextStyle = null;

	public static GUIStyle SmallTextStyle
	{
		get
		{
			if (m_SmallTextStyle == null)
			{
				m_SmallTextStyle = new GUIStyle("Label");
				m_SmallTextStyle.fontSize = 9;
				m_SmallTextStyle.alignment = TextAnchor.LowerLeft;
				m_SmallTextStyle.padding = new RectOffset(0, 0, 4, 0);
			}
			return m_SmallTextStyle;
		}
	}


	/// <summary>
	/// 
	/// </summary>
	private void OnEnable()
	{

		vp_Timer timer = (vp_Timer)target;

		if(!timer.WasAddedCorrectly)
		{
			EditorUtility.DisplayDialog("Ooops!", "vp_Timer can't be added in the Inspector. It must be called from script. See the documentation for more info.", "OK");
			DestroyImmediate(timer);
		}

	}


	/// <summary>
	/// 
	/// </summary>
	public override void OnInspectorGUI()
	{

		EditorGUILayout.Space();

		vp_Timer.Stats stats = vp_Timer.EditorGetStats();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("\tCreated: " + stats.Created);
		EditorGUILayout.LabelField("\tInactive: " + stats.Inactive);
		EditorGUILayout.LabelField("\tActive: " + stats.Active);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginHorizontal();
		m_ShowId = GUILayout.Toggle(m_ShowId, "", GUILayout.MaxWidth(12));
		GUILayout.Label("Show Id", SmallTextStyle);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		m_ShowCallStack = GUILayout.Toggle(m_ShowCallStack, "", GUILayout.MaxWidth(12));
		GUILayout.Label("Show CallStack", SmallTextStyle);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();

		if (stats.Active > 100)
		{
			EditorGUILayout.HelpBox( "Lots of active timers. Displaying only the last 100 started (for editor gui performance reasons).", MessageType.Warning);
		}

		// iterate active timers backwards to display latest at the top
		for (int c = Mathf.Min(100, stats.Active - 1); c > -1; c--)
		{

			EditorGUILayout.BeginHorizontal();

			string s = "\t";

			if(m_ShowId)
			    s += vp_Timer.EditorGetMethodId(c) + ": ";

			s += vp_Timer.EditorGetMethodInfo(c);

			if (!m_ShowCallStack)
			{
				int a = s.IndexOf(" <");
				s = s.Substring(0, a);
			}

			EditorGUILayout.HelpBox(s, MessageType.None);
			EditorGUILayout.EndHorizontal();

		}

	}
	
	
}

