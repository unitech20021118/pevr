/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI.Tweens
{
	public struct FloatTween : ITweenValue
	{
		public class FloatTweenCallback : UnityEvent<float> {}
		public class FloatFinishCallback : UnityEvent {}
		
		private float m_StartFloat;
		private float m_TargetFloat;
		private float m_Duration;
		private bool m_IgnoreTimeScale;
		private TweenEasing m_Easing;
		private FloatTweenCallback m_Target;
		private FloatFinishCallback m_Finish;
		
		/// <summary>
		/// Gets or sets the starting float.
		/// </summary>
		/// <value>The start float.</value>
		public float startFloat
		{
			get { return m_StartFloat; }
			set { m_StartFloat = value; }
		}
		
		/// <summary>
		/// Gets or sets the target float.
		/// </summary>
		/// <value>The target float.</value>
		public float targetFloat
		{
			get { return m_TargetFloat; }
			set { m_TargetFloat = value; }
		}
		
		/// <summary>
		/// Gets or sets the duration of the tween.
		/// </summary>
		/// <value>The duration.</value>
		public float duration
		{
			get { return m_Duration; }
			set { m_Duration = value; }
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.Tweens.ColorTween"/> should ignore time scale.
		/// </summary>
		/// <value><c>true</c> if ignore time scale; otherwise, <c>false</c>.</value>
		public bool ignoreTimeScale
		{
			get { return m_IgnoreTimeScale; }
			set { m_IgnoreTimeScale = value; }
		}
		
		/// <summary>
		/// Gets or sets the tween easing.
		/// </summary>
		/// <value>The easing.</value>
		public TweenEasing easing
		{
			get { return m_Easing; }
			set { m_Easing = value; }
		}
		
		/// <summary>
		/// Tweens the float based on percentage.
		/// </summary>
		/// <param name="floatPercentage">Float percentage.</param>
		public void TweenValue(float floatPercentage)
		{
			if (!ValidTarget())
				return;
			
			m_Target.Invoke( Mathf.Lerp (m_StartFloat, m_TargetFloat, floatPercentage) );
		}
		
		/// <summary>
		/// Adds a on changed callback.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (m_Target == null)
				m_Target = new FloatTweenCallback();
			
			m_Target.AddListener(callback);
		}
		
		/// <summary>
		/// Adds a on finish callback.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (m_Finish == null)
				m_Finish = new FloatFinishCallback();
			
			m_Finish.AddListener(callback);
		}
		
		public bool GetIgnoreTimescale()
		{
			return m_IgnoreTimeScale;
		}
		
		public float GetDuration()
		{
			return m_Duration;
		}
		
		public bool ValidTarget()
		{
			return m_Target != null;
		}
		
		/// <summary>
		/// Invokes the on finish callback.
		/// </summary>
		public void Finished()
		{
			if (m_Finish != null)
				m_Finish.Invoke();
		}
	}
}
