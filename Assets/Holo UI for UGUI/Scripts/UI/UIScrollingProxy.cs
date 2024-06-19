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

public class UIScrollingProxy : MonoBehaviour, IEventSystemHandler, IScrollHandler, IInitializePotentialDragHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
	
	protected ScrollRect m_ScrollRect;
	
	protected virtual void OnEnable()
	{
		this.m_ScrollRect = this.gameObject.GetComponentInParent<ScrollRect>();
	}
	
	protected virtual void OnDisable()
	{
		this.m_ScrollRect = null;
	}
	
	public virtual void OnScroll(PointerEventData data)
	{
		if (this.m_ScrollRect != null)
			this.m_ScrollRect.OnScroll(data);
	}
	
	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		if (this.m_ScrollRect != null)
			this.m_ScrollRect.OnInitializePotentialDrag(eventData);
	}
	
	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		if (this.m_ScrollRect != null)
			this.m_ScrollRect.OnBeginDrag(eventData);
	}
	
	public virtual void OnEndDrag(PointerEventData eventData)
	{
		if (this.m_ScrollRect != null)
			this.m_ScrollRect.OnEndDrag(eventData);
	}
	
	public virtual void OnDrag(PointerEventData eventData)
	{
		if (this.m_ScrollRect != null)
			this.m_ScrollRect.OnDrag(eventData);
	}
}
