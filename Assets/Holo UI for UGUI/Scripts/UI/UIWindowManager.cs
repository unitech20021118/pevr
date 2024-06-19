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
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class UIWindowManager : MonoBehaviour {
	
		protected virtual void Update()
		{
			// Check for escape key press
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				bool EligibleForShow = true;
				
				// Get the windows list
				List<UIWindow> windows = UIWindow.GetWindows();
				
				// Loop through the windows and hide if required
				foreach (UIWindow window in windows)
				{
					// Check if the window has escape key action
					if (window.escapeKeyAction != UIWindow.EscapeKeyAction.None)
					{
						// Check if the window should be hidden on escape
						if (window.IsOpen && (window.escapeKeyAction == UIWindow.EscapeKeyAction.Hide || window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle || (window.escapeKeyAction == UIWindow.EscapeKeyAction.HideIfFocused && window.IsFocused)))
						{
							// Hide the window
							window.Hide();
							
							// Dont allow a window to be shown after a window has been closed
							EligibleForShow = false;
						}
					}
				}
				
				// If we didnt hide any windows with this key press check if we should show a window
				if (EligibleForShow)
				{
					// Loop through the windows again and show if required
					foreach (UIWindow window in windows)
					{
						// Check if the window has escape key action toggle and is not shown
						if (!window.IsOpen && window.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle)
						{
							// Show the window
							window.Show();
						}
					}
				}
			}
		}
	}
}
