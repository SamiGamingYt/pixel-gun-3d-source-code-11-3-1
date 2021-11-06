using System;
using UnityEngine;

// Token: 0x0200038B RID: 907
[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x00092ECC File Offset: 0x000910CC
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x00092EF4 File Offset: 0x000910F4
	// (set) Token: 0x06001FF8 RID: 8184 RVA: 0x00092F04 File Offset: 0x00091104
	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x00092F14 File Offset: 0x00091114
	// (set) Token: 0x06001FFA RID: 8186 RVA: 0x00092F1C File Offset: 0x0009111C
	[Obsolete("Use 'value' instead")]
	public Vector3 scale
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

	// Token: 0x06001FFB RID: 8187 RVA: 0x00092F28 File Offset: 0x00091128
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
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

	// Token: 0x06001FFC RID: 8188 RVA: 0x00092FB0 File Offset: 0x000911B0
	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x00092FF8 File Offset: 0x000911F8
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x00093008 File Offset: 0x00091208
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x00093018 File Offset: 0x00091218
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x00093028 File Offset: 0x00091228
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001438 RID: 5176
	public Vector3 from = Vector3.one;

	// Token: 0x04001439 RID: 5177
	public Vector3 to = Vector3.one;

	// Token: 0x0400143A RID: 5178
	public bool updateTable;

	// Token: 0x0400143B RID: 5179
	private Transform mTrans;

	// Token: 0x0400143C RID: 5180
	private UITable mTable;
}
