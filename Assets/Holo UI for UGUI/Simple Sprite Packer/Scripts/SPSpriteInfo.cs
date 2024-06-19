/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System;

namespace SimpleSpritePacker
{
	[System.Serializable]
	public class SPSpriteInfo : IComparable<SPSpriteInfo>
	{
		/// <summary>
		/// The source texture or sprite.
		/// </summary>
		public UnityEngine.Object source;
		
		/// <summary>
		/// The target sprite (the one in the atlas).
		/// </summary>
		public Sprite targetSprite;
		
		/// <summary>
		/// Gets the name of the sprite.
		/// </summary>
		/// <value>The name.</value>
		public string name
		{
			get
			{
				if (this.targetSprite != null)
				{
					return this.targetSprite.name;
				}
				else if (this.source != null)
				{
					return this.source.name;
				}
				
				// Default
				return string.Empty;
			}
		}
		
		/// <summary>
		/// Gets the sprite size used for comparison.
		/// </summary>
		/// <value>The size for comparison.</value>
		public Vector2 sizeForComparison
		{
			get
			{
				if (this.source != null)
				{
					if (this.source is Texture2D)
					{
						return new Vector2((this.source as Texture2D).width, (this.source as Texture2D).height);
					}
					else if (this.source is Sprite)
					{
						return (this.source as Sprite).rect.size;
					}
				}
				else if (this.targetSprite != null)
				{
					return this.targetSprite.rect.size;
				}
				
				// Default
				return Vector2.zero;
			}
		}
		
		public int CompareTo(SPSpriteInfo other)
		{
			return this.name.CompareTo(other.name);
		}
	}
}
