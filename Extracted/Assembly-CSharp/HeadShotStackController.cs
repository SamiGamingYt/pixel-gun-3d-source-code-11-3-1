using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class HeadShotStackController : MonoBehaviour
{
	// Token: 0x0600150D RID: 5389 RVA: 0x00053540 File Offset: 0x00051740
	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		HeadShotStackController.sharedController = this;
		Transform transform = base.transform;
		transform.position = Vector3.zero;
		int childCount = transform.childCount;
		this.particles = new HitParticle[childCount];
		for (int i = 0; i < childCount; i++)
		{
			this.particles[i] = transform.GetChild(i).GetComponent<HitParticle>();
			if (!Defs.isMulti || Defs.isCOOP)
			{
				ParticleSystem myParticleSystem = this.particles[i].myParticleSystem;
				myParticleSystem.GetComponent<Renderer>().material = this.mobHeadShotMaterial;
			}
		}
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x000535E8 File Offset: 0x000517E8
	public HitParticle GetCurrentParticle(bool _isUseMine)
	{
		bool flag = true;
		for (;;)
		{
			this.currentIndexHole++;
			if (this.currentIndexHole >= this.particles.Length)
			{
				if (!flag)
				{
					break;
				}
				this.currentIndexHole = 0;
				flag = false;
			}
			if (!this.particles[this.currentIndexHole].isUseMine || _isUseMine)
			{
				goto IL_51;
			}
		}
		return null;
		IL_51:
		return this.particles[this.currentIndexHole];
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x00053654 File Offset: 0x00051854
	private void OnDestroy()
	{
		HeadShotStackController.sharedController = null;
	}

	// Token: 0x04000C4B RID: 3147
	public static HeadShotStackController sharedController;

	// Token: 0x04000C4C RID: 3148
	public HitParticle[] particles;

	// Token: 0x04000C4D RID: 3149
	public Material mobHeadShotMaterial;

	// Token: 0x04000C4E RID: 3150
	private int currentIndexHole;
}
