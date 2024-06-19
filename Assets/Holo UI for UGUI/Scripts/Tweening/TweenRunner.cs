/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections;

namespace UnityEngine.UI.Tweens
{
	// Tween runner, executes the given tween.
	// The coroutine will live within the given 
	// behaviour container.
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		protected MonoBehaviour m_CoroutineContainer;
		protected IEnumerator m_Tween;
		
		// utility function for starting the tween
		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
				yield break;
			
			float elapsedTime = 0.0f;
			while (elapsedTime < tweenInfo.duration)
			{
				elapsedTime += (tweenInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
				
				float percentage = TweenEasingHandler.Apply(tweenInfo.easing, elapsedTime, 0.0f, 1.0f, tweenInfo.duration);
				tweenInfo.TweenValue(percentage);
				
				yield return null;
			}
			tweenInfo.TweenValue(1.0f);
			tweenInfo.Finished();
		}
		
		public void Init(MonoBehaviour coroutineContainer)
		{
			m_CoroutineContainer = coroutineContainer;
		}
		
		public void StartTween(T info)
		{
			if (m_CoroutineContainer == null)
			{
				Debug.LogWarning ("Coroutine container not configured... did you forget to call Init?");
				return;
			}
			
			if (m_Tween != null)
			{
				m_CoroutineContainer.StopCoroutine (m_Tween);
				m_Tween = null;
			}
			
			if (!m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				info.TweenValue(1.0f);
				return;
			}
			
			m_Tween = Start (info);
			m_CoroutineContainer.StartCoroutine (m_Tween);
		}
	}
}
