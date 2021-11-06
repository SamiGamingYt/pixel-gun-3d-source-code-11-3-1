using System;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class HitStackController : MonoBehaviour
{
	// Token: 0x0600155F RID: 5471 RVA: 0x0005555C File Offset: 0x0005375C
	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Transform transform = base.transform;
		transform.position = Vector3.zero;
		int childCount = transform.childCount;
		this.particles = new HitParticle[childCount];
		for (int i = 0; i < childCount; i++)
		{
			this.particles[i] = transform.GetChild(i).GetComponent<HitParticle>();
		}
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x000555CC File Offset: 0x000537CC
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

	// Token: 0x04000CB5 RID: 3253
	public HitParticle[] particles;

	// Token: 0x04000CB6 RID: 3254
	private int currentIndexHole;
}
