/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Button Extended", 58)]
	public class UIButtonExtended : Button {
		
		public enum VisualState
		{
			Normal,
			Highlighted,
			Pressed,
			Disabled
		}
		
		[Serializable]
		public class StateChangeEvent : UnityEvent<VisualState, bool> { }
		
		/// <summary>
		/// The on state change event.
		/// </summary>
		public StateChangeEvent onStateChange = new StateChangeEvent();
		
		protected override void OnDisable()
		{
			base.OnDisable();
			
			// Invoke the state change event
			if (this.onStateChange != null)
				this.onStateChange.Invoke(VisualState.Disabled, true);
		}
		
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			
			// Invoke the state change event
			if (this.onStateChange != null)
				this.onStateChange.Invoke((VisualState)state, instant);
		}
	}
}
