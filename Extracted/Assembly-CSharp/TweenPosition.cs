using System;
using UnityEngine;

// Token: 0x02000389 RID: 905
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00092AC8 File Offset: 0x00090CC8
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

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06001FDD RID: 8157 RVA: 0x00092AF0 File Offset: 0x00090CF0
	// (set) Token: 0x06001FDE RID: 8158 RVA: 0x00092AF8 File Offset: 0x00090CF8
	[Obsolete("Use 'value' instead")]
	public Vector3 position
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

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06001FDF RID: 8159 RVA: 0x00092B04 File Offset: 0x00090D04
	// (set) Token: 0x06001FE0 RID: 8160 RVA: 0x00092B38 File Offset: 0x00090D38
	public Vector3 value
	{
		get
		{
			return (!this.worldSpace) ? this.cachedTransform.localPosition : this.cachedTransform.position;
		}
		set
		{
			if (this.mRect == null || !this.mRect.isAnchored || this.worldSpace)
			{
				if (this.worldSpace)
				{
					this.cachedTransform.position = value;
				}
				else
				{
					this.cachedTransform.localPosition = value;
				}
			}
			else
			{
				value -= this.cachedTransform.localPosition;
				NGUIMath.MoveRect(this.mRect, value.x, value.y);
			}
		}
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x00092BCC File Offset: 0x00090DCC
	private void Awake()
	{
		this.mRect = base.GetComponent<UIRect>();
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x00092BDC File Offset: 0x00090DDC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x00092C08 File Offset: 0x00090E08
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x00092C50 File Offset: 0x00090E50
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.worldSpace = worldSpace;
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x00092CA0 File Offset: 0x00090EA0
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x00092CB0 File Offset: 0x00090EB0
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x00092CC0 File Offset: 0x00090EC0
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x00092CD0 File Offset: 0x00090ED0
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x0400142F RID: 5167
	public Vector3 from;

	// Token: 0x04001430 RID: 5168
	public Vector3 to;

	// Token: 0x04001431 RID: 5169
	[HideInInspector]
	public bool worldSpace;

	// Token: 0x04001432 RID: 5170
	private Transform mTrans;

	// Token: 0x04001433 RID: 5171
	private UIRect mRect;
}
