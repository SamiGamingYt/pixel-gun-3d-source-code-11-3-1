using System;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class ParticleBonuse : MonoBehaviour
{
	// Token: 0x06002378 RID: 9080 RVA: 0x000B0A24 File Offset: 0x000AEC24
	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x000B0A70 File Offset: 0x000AEC70
	public void ShowParticle()
	{
		base.gameObject.SetActive(true);
		this.timer = this.maxTimer;
	}

	// Token: 0x040017F2 RID: 6130
	public float maxTimer = 2f;

	// Token: 0x040017F3 RID: 6131
	public float timer = -1f;
}
