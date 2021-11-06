using System;
using UnityEngine;

// Token: 0x0200038D RID: 909
[AddComponentMenu("NGUI/Tween/Tween Volume")]
[RequireComponent(typeof(AudioSource))]
public class TweenVolume : UITweener
{
	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06002006 RID: 8198 RVA: 0x00093278 File Offset: 0x00091478
	public AudioSource audioSource
	{
		get
		{
			if (this.mSource == null)
			{
				this.mSource = base.GetComponent<AudioSource>();
				if (this.mSource == null)
				{
					this.mSource = base.GetComponent<AudioSource>();
					if (this.mSource == null)
					{
						Debug.LogError("TweenVolume needs an AudioSource to work with", this);
						base.enabled = false;
					}
				}
			}
			return this.mSource;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06002007 RID: 8199 RVA: 0x000932E8 File Offset: 0x000914E8
	// (set) Token: 0x06002008 RID: 8200 RVA: 0x000932F0 File Offset: 0x000914F0
	[Obsolete("Use 'value' instead")]
	public float volume
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

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06002009 RID: 8201 RVA: 0x000932FC File Offset: 0x000914FC
	// (set) Token: 0x0600200A RID: 8202 RVA: 0x00093330 File Offset: 0x00091530
	public float value
	{
		get
		{
			return (!(this.audioSource != null)) ? 0f : this.mSource.volume;
		}
		set
		{
			if (this.audioSource != null)
			{
				this.mSource.volume = value;
			}
		}
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x00093350 File Offset: 0x00091550
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		this.mSource.enabled = (this.mSource.volume > 0.01f);
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x00093398 File Offset: 0x00091598
	public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
	{
		TweenVolume tweenVolume = UITweener.Begin<TweenVolume>(go, duration);
		tweenVolume.from = tweenVolume.value;
		tweenVolume.to = targetVolume;
		if (targetVolume > 0f)
		{
			tweenVolume.audioSource.enabled = true;
			tweenVolume.audioSource.Play();
		}
		return tweenVolume;
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x000933E4 File Offset: 0x000915E4
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x000933F4 File Offset: 0x000915F4
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04001444 RID: 5188
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04001445 RID: 5189
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04001446 RID: 5190
	private AudioSource mSource;
}
