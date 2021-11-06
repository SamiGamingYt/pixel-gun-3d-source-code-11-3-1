using System;
using UnityEngine;

// Token: 0x02000386 RID: 902
[AddComponentMenu("NGUI/Tween/Tween Field of View")]
[RequireComponent(typeof(Camera))]
public class TweenFOV : UITweener
{
	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06001FBA RID: 8122 RVA: 0x00092734 File Offset: 0x00090934
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06001FBB RID: 8123 RVA: 0x0009275C File Offset: 0x0009095C
	// (set) Token: 0x06001FBC RID: 8124 RVA: 0x00092764 File Offset: 0x00090964
	[Obsolete("Use 'value' instead")]
	public float fov
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

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06001FBD RID: 8125 RVA: 0x00092770 File Offset: 0x00090970
	// (set) Token: 0x06001FBE RID: 8126 RVA: 0x00092780 File Offset: 0x00090980
	public float value
	{
		get
		{
			return this.cachedCamera.fieldOfView;
		}
		set
		{
			this.cachedCamera.fieldOfView = value;
		}
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x00092790 File Offset: 0x00090990
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000927B0 File Offset: 0x000909B0
	public static TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration);
		tweenFOV.from = tweenFOV.value;
		tweenFOV.to = to;
		if (duration <= 0f)
		{
			tweenFOV.Sample(1f, true);
			tweenFOV.enabled = false;
		}
		return tweenFOV;
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000927F8 File Offset: 0x000909F8
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x00092808 File Offset: 0x00090A08
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x00092818 File Offset: 0x00090A18
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x00092828 File Offset: 0x00090A28
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04001424 RID: 5156
	public float from = 45f;

	// Token: 0x04001425 RID: 5157
	public float to = 45f;

	// Token: 0x04001426 RID: 5158
	private Camera mCam;
}
