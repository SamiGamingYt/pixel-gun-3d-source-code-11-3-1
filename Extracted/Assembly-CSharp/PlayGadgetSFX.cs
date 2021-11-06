using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class PlayGadgetSFX : MonoBehaviour
{
	// Token: 0x060027DE RID: 10206 RVA: 0x000C7170 File Offset: 0x000C5370
	private void Start()
	{
		this.isLow = Device.isPixelGunLow;
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x000C7180 File Offset: 0x000C5380
	public void Play()
	{
		if (!this.isLow || (this.isLow && this.ifLowDevice != PlayGadgetSFX.IfDefsLow.both))
		{
			this.timer = Time.time + this.time;
			this.isPlaying = true;
			this.sfx.SetActive(true);
		}
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x000C71D4 File Offset: 0x000C53D4
	public void Play(float playTime)
	{
		this.time = playTime;
		this.Play();
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x000C71E4 File Offset: 0x000C53E4
	public void Play(bool inZone)
	{
		this.timer = Time.time + this.time;
		this.isZone = true;
		this.isInZone = inZone;
		this.isPlaying = true;
		this.sfx.SetActive(true);
	}

	// Token: 0x060027E2 RID: 10210 RVA: 0x000C721C File Offset: 0x000C541C
	public void Stop()
	{
		this.isPlaying = false;
		this.sfx.SetActive(false);
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x000C7234 File Offset: 0x000C5434
	private void Update()
	{
		if (this.isPlaying)
		{
			if (this.isLow)
			{
				this.sfxSpriteContainer.gameObject.SetActive(this.ifLowDevice != PlayGadgetSFX.IfDefsLow.noSpriteSfx);
				if (this.particles.Length > 0)
				{
					for (int i = 0; i < this.particles.Length; i++)
					{
						this.particles[i].SetActive(this.ifLowDevice != PlayGadgetSFX.IfDefsLow.noParticles);
					}
				}
			}
			if (!this.isZone)
			{
				float num = Mathf.Max(0f, this.timer - Time.time);
				this.sfxSpriteContainer.alpha = this.alphaChange.Evaluate(1f - num / this.time);
				if (this.timer < Time.time)
				{
					this.Stop();
				}
			}
			else if (this.isInZone)
			{
				float num2 = Mathf.Max(0f, this.timer - this.tempZoneTime - Time.time);
				this.sfxSpriteContainer.alpha = this.alphaChange.Evaluate(1f - num2 / this.time);
			}
			else
			{
				float num3 = Mathf.Max(0f, this.timer - Time.time);
				this.tempZoneTime = num3;
				this.sfxSpriteContainer.alpha = this.alphaChange.Evaluate(num3 / this.time);
				if (num3 == 0f)
				{
					this.isZone = false;
					this.Stop();
				}
			}
		}
	}

	// Token: 0x04001C1E RID: 7198
	public PlayGadgetSFX.IfDefsLow ifLowDevice;

	// Token: 0x04001C1F RID: 7199
	public float time = 1f;

	// Token: 0x04001C20 RID: 7200
	public AnimationCurve alphaChange = new AnimationCurve
	{
		keys = new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		},
		postWrapMode = WrapMode.Loop,
		preWrapMode = WrapMode.Loop
	};

	// Token: 0x04001C21 RID: 7201
	public UIWidget sfxSpriteContainer;

	// Token: 0x04001C22 RID: 7202
	public GameObject[] particles;

	// Token: 0x04001C23 RID: 7203
	private float timer;

	// Token: 0x04001C24 RID: 7204
	private bool isPlaying;

	// Token: 0x04001C25 RID: 7205
	private bool isLow;

	// Token: 0x04001C26 RID: 7206
	private bool isZone;

	// Token: 0x04001C27 RID: 7207
	private bool isInZone;

	// Token: 0x04001C28 RID: 7208
	private float tempZoneTime;

	// Token: 0x04001C29 RID: 7209
	[Header("Включается для проигрывания анимации")]
	public GameObject sfx;

	// Token: 0x02000479 RID: 1145
	public enum IfDefsLow
	{
		// Token: 0x04001C2B RID: 7211
		noParticles,
		// Token: 0x04001C2C RID: 7212
		noSpriteSfx,
		// Token: 0x04001C2D RID: 7213
		both,
		// Token: 0x04001C2E RID: 7214
		doNothing
	}
}
