using System;
using UnityEngine;

// Token: 0x0200032D RID: 813
[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
	// Token: 0x06001C0D RID: 7181 RVA: 0x00074560 File Offset: 0x00072760
	private void OnEnable()
	{
		this.mTrans = base.transform;
		if (this.scrollView == null && this.draggablePanel != null)
		{
			this.scrollView = this.draggablePanel;
			this.draggablePanel = null;
		}
		if (this.mStarted && (this.mAutoFind || this.mScroll == null))
		{
			this.FindScrollView();
		}
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000745DC File Offset: 0x000727DC
	private void Start()
	{
		this.mStarted = true;
		this.FindScrollView();
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x000745EC File Offset: 0x000727EC
	private void FindScrollView()
	{
		UIScrollView uiscrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
		if (this.scrollView == null || (this.mAutoFind && uiscrollView != this.scrollView))
		{
			this.scrollView = uiscrollView;
			this.mAutoFind = true;
		}
		else if (this.scrollView == uiscrollView)
		{
			this.mAutoFind = true;
		}
		this.mScroll = this.scrollView;
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x0007466C File Offset: 0x0007286C
	private void OnPress(bool pressed)
	{
		if (this.mAutoFind && this.mScroll != this.scrollView)
		{
			this.mScroll = this.scrollView;
			this.mAutoFind = false;
		}
		if (this.scrollView && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.scrollView.Press(pressed);
			if (!pressed && this.mAutoFind)
			{
				this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
				this.mScroll = this.scrollView;
			}
		}
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x00074714 File Offset: 0x00072914
	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Drag();
		}
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x00074748 File Offset: 0x00072948
	private void OnScroll(float delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x00074774 File Offset: 0x00072974
	public void OnPan(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.OnPan(delta);
		}
	}

	// Token: 0x0400112D RID: 4397
	public UIScrollView scrollView;

	// Token: 0x0400112E RID: 4398
	[SerializeField]
	[HideInInspector]
	private UIScrollView draggablePanel;

	// Token: 0x0400112F RID: 4399
	private Transform mTrans;

	// Token: 0x04001130 RID: 4400
	private UIScrollView mScroll;

	// Token: 0x04001131 RID: 4401
	private bool mAutoFind;

	// Token: 0x04001132 RID: 4402
	private bool mStarted;
}
