using System;
using UnityEngine;

// Token: 0x02000387 RID: 903
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Height")]
public class TweenHeight : UITweener
{
	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x00092850 File Offset: 0x00090A50
	public UIWidget cachedWidget
	{
		get
		{
			if (this.mWidget == null)
			{
				this.mWidget = base.GetComponent<UIWidget>();
			}
			return this.mWidget;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x00092878 File Offset: 0x00090A78
	// (set) Token: 0x06001FC8 RID: 8136 RVA: 0x00092880 File Offset: 0x00090A80
	[Obsolete("Use 'value' instead")]
	public int height
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x0009288C File Offset: 0x00090A8C
	// (set) Token: 0x06001FCA RID: 8138 RVA: 0x0009289C File Offset: 0x00090A9C
	public int value
	{
		get
		{
			return this.cachedWidget.height;
		}
		set
		{
			this.cachedWidget.height = value;
		}
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x000928AC File Offset: 0x00090AAC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.RoundToInt((float)this.from * (1f - factor) + (float)this.to * factor);
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x00092930 File Offset: 0x00090B30
	public static TweenHeight Begin(UIWidget widget, float duration, int height)
	{
		TweenHeight tweenHeight = UITweener.Begin<TweenHeight>(widget.gameObject, duration);
		tweenHeight.from = widget.height;
		tweenHeight.to = height;
		if (duration <= 0f)
		{
			tweenHeight.Sample(1f, true);
			tweenHeight.enabled = false;
		}
		return tweenHeight;
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x0009297C File Offset: 0x00090B7C
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x0009298C File Offset: 0x00090B8C
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x0009299C File Offset: 0x00090B9C
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000929AC File Offset: 0x00090BAC
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001427 RID: 5159
	public int from = 100;

	// Token: 0x04001428 RID: 5160
	public int to = 100;

	// Token: 0x04001429 RID: 5161
	public bool updateTable;

	// Token: 0x0400142A RID: 5162
	private UIWidget mWidget;

	// Token: 0x0400142B RID: 5163
	private UITable mTable;
}
