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
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
	[SerializeField] private Image m_Target;
	[SerializeField] private int m_FPS = 30;
	[SerializeField] private bool m_Loop = true;
	[SerializeField] private bool m_UseUnscaledTime = true;
	[SerializeField] private List<Sprite> m_Sprites = new List<Sprite>();
	
	protected float m_Delta = 0f;
	protected int m_Index = 0;
	protected bool m_Active = true;
	
	/// <summary>
	/// Number of frames in the animation.
	/// </summary>
	public int frames
	{
		get { return this.m_Sprites.Count; }
	}
	
	/// <summary>
	/// Gets or sets the target image.
	/// </summary>
	/// <value>The target.</value>
	public Image target
	{
		get { return this.m_Target; }
		set { this.m_Target = value; }
	}
	
	/// <summary>
	/// Animation framerate.
	/// </summary>
	public int framesPerSecond
	{
		get { return this.m_FPS; }
		set { this.m_FPS = value; }
	}

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>
	public bool loop
	{
		get { return this.m_Loop; }
		set { this.m_Loop = value; }
	}
	
	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>
	public bool isPlaying
	{
		get { return this.m_Active; }
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UISpriteAnimation"/> uses unscaled time.
	/// </summary>
	/// <value><c>true</c> if use unscaled time; otherwise, <c>false</c>.</value>
	public bool useUnscaledTime
	{
		get { return this.m_UseUnscaledTime; }
		set { this.m_UseUnscaledTime = value; }
	}

	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>
	protected virtual void Update()
	{
		if (this.m_Active && this.m_Target != null && this.m_Sprites.Count > 1 && Application.isPlaying && this.m_FPS > 0)
		{
			this.m_Delta += (this.m_UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
			float rate = 1f / this.m_FPS;
			
			if (rate < this.m_Delta)
			{
				
				this.m_Delta = (rate > 0f) ? this.m_Delta - rate : 0f;
				
				if (++this.m_Index >= this.m_Sprites.Count)
				{
					this.m_Index = 0;
					this.m_Active = this.m_Loop;
				}
				
				if (this.m_Active)
				{
					this.m_Target.overrideSprite = this.m_Sprites[this.m_Index];
				}
			}
		}
	}
	
	/// <summary>
	/// Reset the animation to the beginning.
	/// </summary>
	public void Play()
	{
		this.m_Active = true;
	}
	
	/// <summary>
	/// Pause the animation.
	/// </summary>
	public void Pause()
	{
		this.m_Active = false;
	}
	
	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>
	public void ResetToBeginning()
	{
		this.m_Active = true;
		this.m_Index = 0;
		
		if (this.m_Target != null && this.m_Sprites.Count > 0)
		{
			this.m_Target.overrideSprite = this.m_Sprites[this.m_Index];
		}
	}
}
