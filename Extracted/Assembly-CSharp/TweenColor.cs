using System;
using UnityEngine;

// Token: 0x02000385 RID: 901
[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	// Token: 0x06001FAE RID: 8110 RVA: 0x0009245C File Offset: 0x0009065C
	private void Cache()
	{
		this.mCached = true;
		this.mWidget = base.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			return;
		}
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mSr != null)
		{
			return;
		}
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			this.mMat = component.material;
			return;
		}
		this.mLight = base.GetComponent<Light>();
		if (this.mLight == null)
		{
			this.mWidget = base.GetComponentInChildren<UIWidget>();
		}
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06001FAF RID: 8111 RVA: 0x000924F8 File Offset: 0x000906F8
	// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x00092500 File Offset: 0x00090700
	[Obsolete("Use 'value' instead")]
	public Color color
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

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x0009250C File Offset: 0x0009070C
	// (set) Token: 0x06001FB2 RID: 8114 RVA: 0x000925A4 File Offset: 0x000907A4
	public Color value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.color;
			}
			if (this.mSr != null)
			{
				return this.mSr.color;
			}
			if (this.mLight != null)
			{
				return this.mLight.color;
			}
			return Color.black;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
			}
			else if (this.mMat != null)
			{
				this.mMat.color = value;
			}
			else if (this.mSr != null)
			{
				this.mSr.color = value;
			}
			else if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = (value.r + value.g + value.b > 0.01f);
			}
		}
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x00092670 File Offset: 0x00090870
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Color.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x0009268C File Offset: 0x0009088C
	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000926D4 File Offset: 0x000908D4
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000926E4 File Offset: 0x000908E4
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000926F4 File Offset: 0x000908F4
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x00092704 File Offset: 0x00090904
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x0400141D RID: 5149
	public Color from = Color.white;

	// Token: 0x0400141E RID: 5150
	public Color to = Color.white;

	// Token: 0x0400141F RID: 5151
	private bool mCached;

	// Token: 0x04001420 RID: 5152
	private UIWidget mWidget;

	// Token: 0x04001421 RID: 5153
	private Material mMat;

	// Token: 0x04001422 RID: 5154
	private Light mLight;

	// Token: 0x04001423 RID: 5155
	private SpriteRenderer mSr;
}
