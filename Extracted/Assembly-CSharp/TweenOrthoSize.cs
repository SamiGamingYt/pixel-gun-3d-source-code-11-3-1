using System;
using UnityEngine;

// Token: 0x02000388 RID: 904
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Orthographic Size")]
public class TweenOrthoSize : UITweener
{
	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x000929DC File Offset: 0x00090BDC
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

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x00092A04 File Offset: 0x00090C04
	// (set) Token: 0x06001FD4 RID: 8148 RVA: 0x00092A0C File Offset: 0x00090C0C
	[Obsolete("Use 'value' instead")]
	public float orthoSize
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

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x00092A18 File Offset: 0x00090C18
	// (set) Token: 0x06001FD6 RID: 8150 RVA: 0x00092A28 File Offset: 0x00090C28
	public float value
	{
		get
		{
			return this.cachedCamera.orthographicSize;
		}
		set
		{
			this.cachedCamera.orthographicSize = value;
		}
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x00092A38 File Offset: 0x00090C38
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x00092A58 File Offset: 0x00090C58
	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
		tweenOrthoSize.from = tweenOrthoSize.value;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x00092AA0 File Offset: 0x00090CA0
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x00092AB0 File Offset: 0x00090CB0
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0400142C RID: 5164
	public float from = 1f;

	// Token: 0x0400142D RID: 5165
	public float to = 1f;

	// Token: 0x0400142E RID: 5166
	private Camera mCam;
}
