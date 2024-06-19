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

namespace UnityEngine.UI
{
	public static class UIUtility {
		
		/// <summary>
		/// Brings the game object to the front.
		/// </summary>
		/// <param name="go">Game Object.</param>
		public static void BringToFront(GameObject go)
		{
			Canvas canvas = UIUtility.FindInParents<Canvas>(go);
			
			// If the object has a parent canvas
			if (canvas != null)
				go.transform.SetParent(canvas.transform, true);
			
			// Set as last sibling
			go.transform.SetAsLastSibling();
		}
		
		/// <summary>
		/// Finds the component in the game object's parents.
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="go">Game Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T FindInParents<T>(GameObject go) where T : Component
		{
			if (go == null)
				return null;
			
			var comp = go.GetComponent<T>();
			
			if (comp != null)
				return comp;
			
			Transform t = go.transform.parent;
			
			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
			
			return comp;
		}
	}
}
