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
using System.Collections;

namespace UnityEditor.UI
{
	public class NavigationTools {
		
		[MenuItem ("Tools/Navigation/Disable All Automatic")]
		static void DisableAutomaticNavigations()
		{
			Selectable[] selectables = Resources.FindObjectsOfTypeAll<Selectable>();
			
			int count = 0;
			foreach (Selectable s in selectables)
			{
				if (s.navigation.mode == Navigation.Mode.Automatic)
				{
					Navigation n = s.navigation;
					n.mode = Navigation.Mode.None;
					s.navigation = n;
					
					if (!s.gameObject.activeInHierarchy)
						EditorUtility.SetDirty(s);
					
					++count;
				}
			}
			
			Debug.Log("Affected objects: " + count.ToString());
		}
		
		[MenuItem ("Tools/Navigation/Disable All")]
		static void DisableAllNavigations()
		{
			Selectable[] selectables = Resources.FindObjectsOfTypeAll<Selectable>();
			
			int count = 0;
			foreach (Selectable s in selectables)
			{
				if (s.navigation.mode != Navigation.Mode.None)
				{
					Navigation n = s.navigation;
					n.mode = Navigation.Mode.None;
					s.navigation = n;
					
					if (!s.gameObject.activeInHierarchy)
						EditorUtility.SetDirty(s);
					
					++count;
				}
			}
			
			Debug.Log("Affected objects: " + count.ToString());
		}
	}
}
