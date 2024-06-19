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
using UnityEngine.EventSystems;
using System.Collections;

public class DeskManagement : MonoBehaviour {

	public void ToggleDesk1(bool state)
	{
		if (state)
		{
			UIWindow.GetWindowByCustomID(1).Show();
			UIWindow.GetWindowByCustomID(2).Show();
			UIWindow.GetWindowByCustomID(3).Show();
		}
		else
		{
			UIWindow.GetWindowByCustomID(1).Hide();
			UIWindow.GetWindowByCustomID(2).Hide();
			UIWindow.GetWindowByCustomID(3).Hide();
		}
	}
	
	public void ToggleDesk2(bool state)
	{
		if (state)
		{
			UIWindow.GetWindowByCustomID(4).Show();
		}
		else
		{
			UIWindow.GetWindowByCustomID(4).Hide();
		}
	}
	
	public void ToggleDesk3(bool state)
	{
		if (state)
		{
			UIWindow.GetWindowByCustomID(5).Show();
		}
		else
		{
			UIWindow.GetWindowByCustomID(5).Hide();
		}
	}
	
	public void ToggleDesk4(bool state)
	{
		if (state)
		{
			UIWindow.GetWindowByCustomID(6).Show();
		}
		else
		{
			UIWindow.GetWindowByCustomID(6).Hide();
		}
	}
}
