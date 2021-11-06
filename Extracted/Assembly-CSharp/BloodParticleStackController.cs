using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class BloodParticleStackController : MonoBehaviour
{
	// Token: 0x0600017B RID: 379 RVA: 0x0000EF8C File Offset: 0x0000D18C
	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		BloodParticleStackController.sharedController = this;
		Transform transform = base.transform;
		transform.position = Vector3.zero;
		int childCount = transform.childCount;
		this.particles = new WallBloodParticle[childCount];
		for (int i = 0; i < childCount; i++)
		{
			this.particles[i] = transform.GetChild(i).GetComponent<WallBloodParticle>();
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000F000 File Offset: 0x0000D200
	public WallBloodParticle GetCurrentParticle(bool _isUseMine)
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

	// Token: 0x0600017D RID: 381 RVA: 0x0000F06C File Offset: 0x0000D26C
	private void OnDestroy()
	{
		BloodParticleStackController.sharedController = null;
	}

	// Token: 0x04000178 RID: 376
	public static BloodParticleStackController sharedController;

	// Token: 0x04000179 RID: 377
	public WallBloodParticle[] particles;

	// Token: 0x0400017A RID: 378
	private int currentIndexHole;
}
