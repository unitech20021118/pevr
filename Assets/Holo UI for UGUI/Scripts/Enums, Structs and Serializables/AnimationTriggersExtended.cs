/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using UnityEngine;

namespace UnityEngine.UI
{
	[Serializable]
	public class AnimationTriggersExtended
	{
		//
		// Properties
		//
		[SerializeField]
		private string m_NormalTrigger = "Normal";
		
		[SerializeField]
		private string m_HighlightedTrigger = "Highlighted";
		
		[SerializeField]
		private string m_PressedTrigger = "Pressed";
		
		[SerializeField]
		private string m_ActiveTrigger = "Active";
		
		[SerializeField]
		private string m_ActiveHighlightedTrigger = "ActiveHighlighted";
		
		[SerializeField]
		private string m_ActivePressedTrigger = "ActivePressed";
		
		[SerializeField]
		private string m_DisabledTrigger = "Disabled";
		
		public string normalTrigger {
			get {
				return this.m_NormalTrigger;
			}
			set {
				this.m_NormalTrigger = value;
			}
		}
		
		public string highlightedTrigger {
			get {
				return this.m_HighlightedTrigger;
			}
			set {
				this.m_HighlightedTrigger = value;
			}
		}
		
		public string pressedTrigger {
			get {
				return this.m_PressedTrigger;
			}
			set {
				this.m_PressedTrigger = value;
			}
		}
		
		public string activeTrigger {
			get {
				return this.m_ActiveTrigger;
			}
			set {
				this.m_ActiveTrigger = value;
			}
		}
		
		public string activeHighlightedTrigger {
			get {
				return this.m_ActiveHighlightedTrigger;
			}
			set {
				this.m_ActiveHighlightedTrigger = value;
			}
		}
		
		public string activePressedTrigger {
			get {
				return this.m_ActivePressedTrigger;
			}
			set {
				this.m_ActivePressedTrigger = value;
			}
		}
		
		public string disabledTrigger {
			get {
				return this.m_DisabledTrigger;
			}
			set {
				this.m_DisabledTrigger = value;
			}
		}
	}
}
