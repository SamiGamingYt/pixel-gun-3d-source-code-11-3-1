using System;
using UnityEngine;

// Token: 0x0200038E RID: 910
[AddComponentMenu("NGUI/Tween/Tween Width")]
[RequireComponent(typeof(UIWidget))]
public class TweenWidth : UITweener
{
	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06002010 RID: 8208 RVA: 0x0009341C File Offset: 0x0009161C
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

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06002011 RID: 8209 RVA: 0x00093444 File Offset: 0x00091644
	// (set) Token: 0x06002012 RID: 8210 RVA: 0x0009344C File Offset: 0x0009164C
	[Obsolete("Use 'value' instead")]
	public int width
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

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06002013 RID: 8211 RVA: 0x00093458 File Offset: 0x00091658
	// (set) Token: 0x06002014 RID: 8212 RVA: 0x00093468 File Offset: 0x00091668
	public int value
	{
		get
		{
			return this.cachedWidget.width;
		}
		set
		{
			this.cachedWidget.width = value;
		}
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x00093478 File Offset: 0x00091678
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

	// Token: 0x06002016 RID: 8214 RVA: 0x000934FC File Offset: 0x000916FC
	public static TweenWidth Begin(UIWidget widget, float duration, int width)
	{
		TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
		tweenWidth.from = widget.width;
		tweenWidth.to = width;
		if (duration <= 0f)
		{
			tweenWidth.Sample(1f, true);
			tweenWidth.enabled = false;
		}
		return tweenWidth;
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x00093548 File Offset: 0x00091748
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x00093558 File Offset: 0x00091758
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x00093568 File Offset: 0x00091768
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x00093578 File Offset: 0x00091778
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001447 RID: 5191
	public int from = 100;

	// Token: 0x04001448 RID: 5192
	public int to = 100;

	// Token: 0x04001449 RID: 5193
	public bool updateTable;

	// Token: 0x0400144A RID: 5194
	private UIWidget mWidget;

	// Token: 0x0400144B RID: 5195
	private UITable mTable;
}
