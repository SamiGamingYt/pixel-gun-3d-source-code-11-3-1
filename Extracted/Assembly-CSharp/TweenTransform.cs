using System;
using UnityEngine;

// Token: 0x0200038C RID: 908
[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
	// Token: 0x06002002 RID: 8194 RVA: 0x00093040 File Offset: 0x00091240
	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
				this.mPos = this.mTrans.position;
				this.mRot = this.mTrans.rotation;
				this.mScale = this.mTrans.localScale;
			}
			if (this.from != null)
			{
				this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
			}
			else
			{
				this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
			}
			if (this.parentWhenFinished && isFinished)
			{
				this.mTrans.parent = this.to;
			}
		}
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x00093208 File Offset: 0x00091408
	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x00093214 File Offset: 0x00091414
	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.enabled = false;
		}
		return tweenTransform;
	}

	// Token: 0x0400143D RID: 5181
	public Transform from;

	// Token: 0x0400143E RID: 5182
	public Transform to;

	// Token: 0x0400143F RID: 5183
	public bool parentWhenFinished;

	// Token: 0x04001440 RID: 5184
	private Transform mTrans;

	// Token: 0x04001441 RID: 5185
	private Vector3 mPos;

	// Token: 0x04001442 RID: 5186
	private Quaternion mRot;

	// Token: 0x04001443 RID: 5187
	private Vector3 mScale;
}
