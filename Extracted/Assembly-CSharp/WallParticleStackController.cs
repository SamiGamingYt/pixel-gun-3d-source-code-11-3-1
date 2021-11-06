using System;
using UnityEngine;

// Token: 0x0200087F RID: 2175
public class WallParticleStackController : MonoBehaviour
{
	// Token: 0x06004E77 RID: 20087 RVA: 0x001C6EC0 File Offset: 0x001C50C0
	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		WallParticleStackController.sharedController = this;
		Transform transform = base.transform;
		transform.position = Vector3.zero;
		int childCount = transform.childCount;
		this.particles = new WallBloodParticle[childCount];
		for (int i = 0; i < childCount; i++)
		{
			this.particles[i] = transform.GetChild(i).GetComponent<WallBloodParticle>();
		}
	}

	// Token: 0x06004E78 RID: 20088 RVA: 0x001C6F34 File Offset: 0x001C5134
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

	// Token: 0x06004E79 RID: 20089 RVA: 0x001C6FA0 File Offset: 0x001C51A0
	private void OnDestroy()
	{
		WallParticleStackController.sharedController = null;
	}

	// Token: 0x04003D0B RID: 15627
	public static WallParticleStackController sharedController;

	// Token: 0x04003D0C RID: 15628
	public WallBloodParticle[] particles;

	// Token: 0x04003D0D RID: 15629
	private int currentIndexHole;
}
