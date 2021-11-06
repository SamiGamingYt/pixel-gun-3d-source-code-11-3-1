using System;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
[AddComponentMenu("NGUI/Interaction/My Center Scroll View on Child")]
public class MyCenterOnChild : MonoBehaviour
{
	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x06003C73 RID: 15475 RVA: 0x00139C98 File Offset: 0x00137E98
	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	// Token: 0x06003C74 RID: 15476 RVA: 0x00139CA0 File Offset: 0x00137EA0
	private void OnEnable()
	{
		this.Recenter();
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x00139CA8 File Offset: 0x00137EA8
	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	// Token: 0x06003C76 RID: 15478 RVA: 0x00139CBC File Offset: 0x00137EBC
	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x00139CD0 File Offset: 0x00137ED0
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.enabled = false;
				return;
			}
			this.mScrollView.onDragFinished = new UIScrollView.OnDragNotification(this.OnDragFinished);
			if (this.mScrollView.horizontalScrollBar != null)
			{
				this.mScrollView.horizontalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				this.mScrollView.verticalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 b = vector - this.mScrollView.currentMomentum * (this.mScrollView.momentumAmount * 0.1f);
		this.mScrollView.currentMomentum = Vector3.zero;
		float num = float.MaxValue;
		Transform target = null;
		Transform transform = base.transform;
		int num2 = 0;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			float num3 = Vector3.SqrMagnitude(child.position - b);
			if (num3 < num)
			{
				num = num3;
				target = child;
				num2 = i;
			}
			i++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == transform.GetChild(num2))
		{
			Vector2 totalDelta = UICamera.currentTouch.totalDelta;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num4;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num4 = totalDelta.magnitude;
				}
				else
				{
					num4 = totalDelta.y;
				}
			}
			else
			{
				num4 = totalDelta.x;
			}
			if (num4 > this.nextPageThreshold)
			{
				if (num2 > 0)
				{
					target = transform.GetChild(num2 - 1);
				}
				else if (this.endlessScroll && transform.childCount > 0 && num2 == 0)
				{
					target = transform.GetChild(transform.childCount - 1);
				}
			}
			else if (num4 < -this.nextPageThreshold)
			{
				if (num2 < transform.childCount - 1)
				{
					target = transform.GetChild(num2 + 1);
				}
				else if (this.endlessScroll && transform.childCount > 0)
				{
					target = transform.GetChild(0);
				}
			}
		}
		this.CenterOn(target, vector);
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x0013A018 File Offset: 0x00138218
	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 a = cachedTransform.InverseTransformPoint(target.position);
			Vector3 b = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 b2 = a - b;
			if (!this.mScrollView.canMoveHorizontally)
			{
				b2.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				b2.y = 0f;
			}
			b2.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - b2, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x0013A114 File Offset: 0x00138314
	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}

	// Token: 0x04002CA0 RID: 11424
	public bool endlessScroll;

	// Token: 0x04002CA1 RID: 11425
	public float springStrength = 8f;

	// Token: 0x04002CA2 RID: 11426
	public float nextPageThreshold;

	// Token: 0x04002CA3 RID: 11427
	public SpringPanel.OnFinished onFinished;

	// Token: 0x04002CA4 RID: 11428
	private UIScrollView mScrollView;

	// Token: 0x04002CA5 RID: 11429
	private GameObject mCenteredObject;
}
