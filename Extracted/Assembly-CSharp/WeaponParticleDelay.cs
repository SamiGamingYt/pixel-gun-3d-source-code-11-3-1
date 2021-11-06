using System;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class WeaponParticleDelay : MonoBehaviour
{
	// Token: 0x06004E85 RID: 20101 RVA: 0x001C7528 File Offset: 0x001C5728
	private void Awake()
	{
		this.weaponAnimation = base.GetComponent<Animation>();
	}

	// Token: 0x06004E86 RID: 20102 RVA: 0x001C7538 File Offset: 0x001C5738
	private void Update()
	{
		if (this.weaponAnimation.IsPlaying(this.animationName))
		{
			if (!this.seqStarted)
			{
				this.seqStarted = true;
				this.partSystem.gameObject.SetActive(false);
				base.Invoke("TurnOnParticleSystem", this.delay);
			}
		}
		else
		{
			this.seqStarted = false;
		}
	}

	// Token: 0x06004E87 RID: 20103 RVA: 0x001C759C File Offset: 0x001C579C
	private void TurnOnParticleSystem()
	{
		this.partSystem.gameObject.SetActive(true);
	}

	// Token: 0x04003D18 RID: 15640
	private Animation weaponAnimation;

	// Token: 0x04003D19 RID: 15641
	public string animationName;

	// Token: 0x04003D1A RID: 15642
	public float delay;

	// Token: 0x04003D1B RID: 15643
	public ParticleSystem partSystem;

	// Token: 0x04003D1C RID: 15644
	private bool seqStarted;
}
