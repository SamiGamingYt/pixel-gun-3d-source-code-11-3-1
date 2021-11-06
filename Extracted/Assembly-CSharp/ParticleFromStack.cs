using System;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class ParticleFromStack : MonoBehaviour
{
	// Token: 0x0600237B RID: 9083 RVA: 0x000B0A94 File Offset: 0x000AEC94
	private void OnEnable()
	{
		this.lifeTimer = this.lifeTime + Time.time;
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x000B0AA8 File Offset: 0x000AECA8
	private void Update()
	{
		if (this.fromStack == null)
		{
			return;
		}
		if (this.lifeTimer < Time.time)
		{
			this.fromStack.ReturnParticle(base.gameObject);
		}
	}

	// Token: 0x040017F4 RID: 6132
	public ParticleStackController fromStack;

	// Token: 0x040017F5 RID: 6133
	public float lifeTime;

	// Token: 0x040017F6 RID: 6134
	private float lifeTimer;
}
