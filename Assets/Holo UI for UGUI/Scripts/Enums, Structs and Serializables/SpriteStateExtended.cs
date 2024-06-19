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
	public struct SpriteStateExtended
	{
		//
		// Properties
		//
		[SerializeField] private Sprite m_HighlightedSprite;
		[SerializeField] private Sprite m_PressedSprite;
		[SerializeField] private Sprite m_ActiveSprite;
		[SerializeField] private Sprite m_ActiveHighlightedSprite;
		[SerializeField] private Sprite m_ActivePressedSprite;
		[SerializeField] private Sprite m_DisabledSprite;
		
		public Sprite highlightedSprite {
			get {
				return this.m_HighlightedSprite;
			}
			set {
				this.m_HighlightedSprite = value;
			}
		}
		
		public Sprite pressedSprite {
			get {
				return this.m_PressedSprite;
			}
			set {
				this.m_PressedSprite = value;
			}
		}
		
		public Sprite activeSprite {
			get {
				return this.m_ActiveSprite;
			}
			set {
				this.m_ActiveSprite = value;
			}
		}
		
		public Sprite activeHighlightedSprite {
			get {
				return this.m_ActiveHighlightedSprite;
			}
			set {
				this.m_ActiveHighlightedSprite = value;
			}
		}
		
		public Sprite activePressedSprite {
			get {
				return this.m_ActivePressedSprite;
			}
			set {
				this.m_ActivePressedSprite = value;
			}
		}
		
		public Sprite disabledSprite {
			get {
				return this.m_DisabledSprite;
			}
			set {
				this.m_DisabledSprite = value;
			}
		}
	}
}
