using System;
using UnityEngine;

// Token: 0x0200038A RID: 906
[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06001FEA RID: 8170 RVA: 0x00092CE8 File Offset: 0x00090EE8
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

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06001FEB RID: 8171 RVA: 0x00092D10 File Offset: 0x00090F10
	// (set) Token: 0x06001FEC RID: 8172 RVA: 0x00092D18 File Offset: 0x00090F18
	[Obsolete("Use 'value' instead")]
	public Quaternion rotation
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

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06001FED RID: 8173 RVA: 0x00092D24 File Offset: 0x00090F24
	// (set) Token: 0x06001FEE RID: 8174 RVA: 0x00092D34 File Offset: 0x00090F34
	public Quaternion value
	{
		get
		{
			return this.cachedTransform.localRotation;
		}
		set
		{
			this.cachedTransform.localRotation = value;
		}
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x00092D44 File Offset: 0x00090F44
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = ((!this.quaternionLerp) ? Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor))) : Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor));
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x00092DE4 File Offset: 0x00090FE4
	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.value.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		if (duration <= 0f)
		{
			tweenRotation.Sample(1f, true);
			tweenRotation.enabled = false;
		}
		return tweenRotation;
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x00092E3C File Offset: 0x0009103C
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value.eulerAngles;
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x00092E60 File Offset: 0x00091060
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value.eulerAngles;
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x00092E84 File Offset: 0x00091084
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = Quaternion.Euler(this.from);
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x00092E98 File Offset: 0x00091098
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = Quaternion.Euler(this.to);
	}

	// Token: 0x04001434 RID: 5172
	public Vector3 from;

	// Token: 0x04001435 RID: 5173
	public Vector3 to;

	// Token: 0x04001436 RID: 5174
	public bool quaternionLerp;

	// Token: 0x04001437 RID: 5175
	private Transform mTrans;
}
