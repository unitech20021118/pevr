/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Raycast Filters/Rectangular Raycast Filter"), RequireComponent(typeof(RectTransform))]
	public class UIRectangularRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
	{
		[SerializeField] private Vector2 m_Offset = Vector2.zero;
		
		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public Vector2 offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
			}
		}
		
		[SerializeField] private RectOffset m_Borders = new RectOffset();
		
		/// <summary>
		/// Gets or sets the borders.
		/// </summary>
		/// <value>The borders.</value>
		public RectOffset borders
		{
			get
			{
				return this.m_Borders;
			}
			set
			{
				this.m_Borders = value;
			}
		}
		
		[Range(0f, 1f)]
		[SerializeField] private float m_ScaleX = 1f;
		
		/// <summary>
		/// Gets or sets the X scale.
		/// </summary>
		/// <value>The X scale.</value>
		public float scaleX
		{
			get
			{
				return this.m_ScaleX;
			}
			set
			{
				this.m_ScaleX = value;
			}
		}
		
		[Range(0f, 1f)]
		[SerializeField] private float m_ScaleY = 1f;
		
		/// <summary>
		/// Gets or sets the Y scale.
		/// </summary>
		/// <value>The Y scale.</value>
		public float scaleY
		{
			get
			{
				return this.m_ScaleY;
			}
			set
			{
				this.m_ScaleY = value;
			}
		}
		
		/// <summary>
		/// Gets the scaled rect including the offset.
		/// </summary>
		/// <value>The scaled rect.</value>
		public Rect scaledRect
		{
			get
			{
				RectTransform rt = (RectTransform)this.transform;
				return new Rect(
					(this.offset.x + this.borders.left + (rt.rect.x + ((rt.rect.width - (rt.rect.width * this.m_ScaleX)) / 2f))), 
					(this.offset.y + this.borders.bottom + (rt.rect.y + ((rt.rect.height - (rt.rect.height * this.m_ScaleY)) / 2f))), 
					((rt.rect.width * this.m_ScaleX) - this.borders.left - this.borders.right), 
					((rt.rect.height * this.m_ScaleY) - this.borders.top - borders.bottom)
					);
			}
		}
		
		/// <summary>
		/// Gets the scaled world corners including the offset.
		/// </summary>
		/// <value>The scaled world corners.</value>
		public Vector3[] scaledWorldCorners
		{
			get
			{
				RectTransform rt = (RectTransform)this.transform;
				Vector3[] corners = new Vector3[4];
				corners[0] = new Vector3(rt.position.x + this.scaledRect.x, rt.position.y + this.scaledRect.y, rt.position.z);
				corners[1] = new Vector3(rt.position.x + this.scaledRect.x + this.scaledRect.width, rt.position.y + this.scaledRect.y, rt.position.z);
				corners[2] = new Vector3(rt.position.x + this.scaledRect.x + this.scaledRect.width, rt.position.y + this.scaledRect.y + this.scaledRect.height, rt.position.z);
				corners[3] = new Vector3(rt.position.x + this.scaledRect.x, rt.position.y + this.scaledRect.y + this.scaledRect.height, rt.position.z);
				return corners;
			}
		}
		
		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (!this.enabled)
				return true;
			
			Vector2 localPositionPivotRelative;
			RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)this.transform, screenPoint, eventCamera, out localPositionPivotRelative);
			return this.scaledRect.Contains(localPositionPivotRelative);
		}
	}
}
