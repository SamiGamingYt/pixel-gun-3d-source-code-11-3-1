using System;
using UnityEngine;

// Token: 0x02000384 RID: 900
[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x000921E0 File Offset: 0x000903E0
	// (set) Token: 0x06001FA5 RID: 8101 RVA: 0x000921E8 File Offset: 0x000903E8
	[Obsolete("Use 'value' instead")]
	public float alpha
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

	// Token: 0x06001FA6 RID: 8102 RVA: 0x000921F4 File Offset: 0x000903F4
	private void Cache()
	{
		this.mCached = true;
		this.mRect = base.GetComponent<UIRect>();
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mRect == null && this.mSr == null)
		{
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				this.mMat = component.material;
			}
			if (this.mMat == null)
			{
				this.mRect = base.GetComponentInChildren<UIRect>();
			}
		}
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x00092280 File Offset: 0x00090480
	// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x00092310 File Offset: 0x00090510
	public float value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				return this.mRect.alpha;
			}
			if (this.mSr != null)
			{
				return this.mSr.color.a;
			}
			return (!(this.mMat != null)) ? 1f : this.mMat.color.a;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				this.mRect.alpha = value;
			}
			else if (this.mSr != null)
			{
				Color color = this.mSr.color;
				color.a = value;
				this.mSr.color = color;
			}
			else if (this.mMat != null)
			{
				Color color2 = this.mMat.color;
				color2.a = value;
				this.mMat.color = color2;
			}
		}
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x000923B8 File Offset: 0x000905B8
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x000923D4 File Offset: 0x000905D4
	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x0009241C File Offset: 0x0009061C
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x0009242C File Offset: 0x0009062C
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04001417 RID: 5143
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04001418 RID: 5144
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04001419 RID: 5145
	private bool mCached;

	// Token: 0x0400141A RID: 5146
	private UIRect mRect;

	// Token: 0x0400141B RID: 5147
	private Material mMat;

	// Token: 0x0400141C RID: 5148
	private SpriteRenderer mSr;
}
