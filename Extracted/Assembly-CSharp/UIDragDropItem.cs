using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000327 RID: 807
[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	// Token: 0x06001BE6 RID: 7142 RVA: 0x00073180 File Offset: 0x00071380
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
		this.mCollider = base.gameObject.GetComponent<Collider>();
		this.mCollider2D = base.gameObject.GetComponent<Collider2D>();
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x000731BC File Offset: 0x000713BC
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000731C0 File Offset: 0x000713C0
	protected virtual void OnDisable()
	{
		if (this.mDragging)
		{
			this.StopDragging(UICamera.hoveredObject);
		}
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x000731D8 File Offset: 0x000713D8
	protected virtual void Start()
	{
		this.mButton = base.GetComponent<UIButton>();
		this.mDragScrollView = base.GetComponent<UIDragScrollView>();
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x000731F4 File Offset: 0x000713F4
	protected virtual void OnPress(bool isPressed)
	{
		if (!this.interactable || UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (isPressed)
		{
			if (!this.mPressed)
			{
				this.mTouch = UICamera.currentTouch;
				this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
				this.mPressed = true;
			}
		}
		else if (this.mPressed && this.mTouch == UICamera.currentTouch)
		{
			this.mPressed = false;
			this.mTouch = null;
		}
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00073288 File Offset: 0x00071488
	protected virtual void Update()
	{
		if (this.restriction == UIDragDropItem.Restriction.PressAndHold && this.mPressed && !this.mDragging && this.mDragStartTime < RealTime.time)
		{
			this.StartDragging();
		}
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x000732D0 File Offset: 0x000714D0
	protected virtual void OnDragStart()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		if (this.restriction != UIDragDropItem.Restriction.None)
		{
			if (this.restriction == UIDragDropItem.Restriction.Horizontal)
			{
				Vector2 totalDelta = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.Vertical)
			{
				Vector2 totalDelta2 = this.mTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (this.restriction == UIDragDropItem.Restriction.PressAndHold)
			{
				return;
			}
		}
		this.StartDragging();
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0007339C File Offset: 0x0007159C
	public virtual void StartDragging()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging)
		{
			if (this.cloneOnDrag)
			{
				this.mPressed = false;
				GameObject gameObject = NGUITools.AddChild(base.transform.parent.gameObject, base.gameObject);
				gameObject.transform.localPosition = base.transform.localPosition;
				gameObject.transform.localRotation = base.transform.localRotation;
				gameObject.transform.localScale = base.transform.localScale;
				UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
				if (component != null)
				{
					component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
				}
				if (this.mTouch != null && this.mTouch.pressed == base.gameObject)
				{
					this.mTouch.current = gameObject;
					this.mTouch.pressed = gameObject;
					this.mTouch.dragged = gameObject;
					this.mTouch.last = gameObject;
				}
				UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
				component2.mTouch = this.mTouch;
				component2.mPressed = true;
				component2.mDragging = true;
				component2.Start();
				component2.OnClone(base.gameObject);
				component2.OnDragDropStart();
				if (UICamera.currentTouch == null)
				{
					UICamera.currentTouch = this.mTouch;
				}
				this.mTouch = null;
				UICamera.Notify(base.gameObject, "OnPress", false);
				UICamera.Notify(base.gameObject, "OnHover", false);
			}
			else
			{
				this.mDragging = true;
				this.OnDragDropStart();
			}
		}
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x00073540 File Offset: 0x00071740
	protected virtual void OnClone(GameObject original)
	{
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x00073544 File Offset: 0x00071744
	protected virtual void OnDrag(Vector2 delta)
	{
		if (!this.interactable)
		{
			return;
		}
		if (!this.mDragging || !base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.OnDragDropMove(delta * this.mRoot.pixelSizeAdjustment);
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x0007359C File Offset: 0x0007179C
	protected virtual void OnDragEnd()
	{
		if (!this.interactable)
		{
			return;
		}
		if (!base.enabled || this.mTouch != UICamera.currentTouch)
		{
			return;
		}
		this.StopDragging(UICamera.hoveredObject);
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x000735D4 File Offset: 0x000717D4
	public void StopDragging(GameObject go)
	{
		if (this.mDragging)
		{
			this.mDragging = false;
			this.OnDragDropRelease(go);
		}
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000735F0 File Offset: 0x000717F0
	protected virtual void OnDragDropStart()
	{
		if (!UIDragDropItem.draggedItems.Contains(this))
		{
			UIDragDropItem.draggedItems.Add(this);
		}
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = false;
		}
		if (this.mButton != null)
		{
			this.mButton.isEnabled = false;
		}
		else if (this.mCollider != null)
		{
			this.mCollider.enabled = false;
		}
		else if (this.mCollider2D != null)
		{
			this.mCollider2D.enabled = false;
		}
		this.mParent = this.mTrans.parent;
		this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
		this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
		this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
		if (UIDragDropRoot.root != null)
		{
			this.mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = this.mTrans.localPosition;
		localPosition.z = 0f;
		this.mTrans.localPosition = localPosition;
		TweenPosition component = base.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
		SpringPosition component2 = base.GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		if (this.mTable != null)
		{
			this.mTable.repositionNow = true;
		}
		if (this.mGrid != null)
		{
			this.mGrid.repositionNow = true;
		}
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x00073798 File Offset: 0x00071998
	protected virtual void OnDragDropMove(Vector2 delta)
	{
		this.mTrans.localPosition += delta;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000737B8 File Offset: 0x000719B8
	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!this.cloneOnDrag)
		{
			if (this.mButton != null)
			{
				this.mButton.isEnabled = true;
			}
			else if (this.mCollider != null)
			{
				this.mCollider.enabled = true;
			}
			else if (this.mCollider2D != null)
			{
				this.mCollider2D.enabled = true;
			}
			UIDragDropContainer uidragDropContainer = (!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface);
			if (uidragDropContainer != null)
			{
				this.mTrans.parent = ((!(uidragDropContainer.reparentTarget != null)) ? uidragDropContainer.transform : uidragDropContainer.reparentTarget);
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.z = 0f;
				this.mTrans.localPosition = localPosition;
			}
			else
			{
				this.mTrans.parent = this.mParent;
			}
			this.mParent = this.mTrans.parent;
			this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
			this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
			if (this.mDragScrollView != null)
			{
				base.StartCoroutine(this.EnableDragScrollView());
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			if (this.mTable != null)
			{
				this.mTable.repositionNow = true;
			}
			if (this.mGrid != null)
			{
				this.mGrid.repositionNow = true;
			}
		}
		else
		{
			NGUITools.Destroy(base.gameObject);
		}
		this.OnDragDropEnd();
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00073968 File Offset: 0x00071B68
	protected virtual void OnDragDropEnd()
	{
		UIDragDropItem.draggedItems.Remove(this);
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x00073978 File Offset: 0x00071B78
	protected IEnumerator EnableDragScrollView()
	{
		yield return new WaitForEndOfFrame();
		if (this.mDragScrollView != null)
		{
			this.mDragScrollView.enabled = true;
		}
		yield break;
	}

	// Token: 0x040010F2 RID: 4338
	public UIDragDropItem.Restriction restriction;

	// Token: 0x040010F3 RID: 4339
	public bool cloneOnDrag;

	// Token: 0x040010F4 RID: 4340
	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	// Token: 0x040010F5 RID: 4341
	public bool interactable = true;

	// Token: 0x040010F6 RID: 4342
	[NonSerialized]
	protected Transform mTrans;

	// Token: 0x040010F7 RID: 4343
	[NonSerialized]
	protected Transform mParent;

	// Token: 0x040010F8 RID: 4344
	[NonSerialized]
	protected Collider mCollider;

	// Token: 0x040010F9 RID: 4345
	[NonSerialized]
	protected Collider2D mCollider2D;

	// Token: 0x040010FA RID: 4346
	[NonSerialized]
	protected UIButton mButton;

	// Token: 0x040010FB RID: 4347
	[NonSerialized]
	protected UIRoot mRoot;

	// Token: 0x040010FC RID: 4348
	[NonSerialized]
	protected UIGrid mGrid;

	// Token: 0x040010FD RID: 4349
	[NonSerialized]
	protected UITable mTable;

	// Token: 0x040010FE RID: 4350
	[NonSerialized]
	protected float mDragStartTime;

	// Token: 0x040010FF RID: 4351
	[NonSerialized]
	protected UIDragScrollView mDragScrollView;

	// Token: 0x04001100 RID: 4352
	[NonSerialized]
	protected bool mPressed;

	// Token: 0x04001101 RID: 4353
	[NonSerialized]
	protected bool mDragging;

	// Token: 0x04001102 RID: 4354
	[NonSerialized]
	protected UICamera.MouseOrTouch mTouch;

	// Token: 0x04001103 RID: 4355
	public static List<UIDragDropItem> draggedItems = new List<UIDragDropItem>();

	// Token: 0x02000328 RID: 808
	public enum Restriction
	{
		// Token: 0x04001105 RID: 4357
		None,
		// Token: 0x04001106 RID: 4358
		Horizontal,
		// Token: 0x04001107 RID: 4359
		Vertical,
		// Token: 0x04001108 RID: 4360
		PressAndHold
	}
}
