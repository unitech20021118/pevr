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
	public class UIToggleWindowAndTab : MonoBehaviour {
		
		public UIWindow window;
		public UITab tab;
		
		public void Toggle()
		{
			if (this.window == null || this.tab == null)
				return;
			
			// Check if the window is open
			if (this.window.IsOpen)
			{
				// Check if the tab is active
				if (this.tab.isOn)
				{
					// Close the window since everything was already opened
					this.window.Hide();
					return;
				}
			}
			
			// If we have reached this part of the code, that means the we should open up things
			this.window.Show();
			this.tab.Activate();
		}
	}
}
