using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class DragonDamageEffect : MonoBehaviour
{
	// Token: 0x06000467 RID: 1127 RVA: 0x000252B0 File Offset: 0x000234B0
	private void Awake()
	{
		this.lifeTimer = Time.time + this.lifeTime;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x000252C4 File Offset: 0x000234C4
	private void Update()
	{
		base.transform.position += base.transform.forward * 23f * Time.deltaTime;
		if (this.lifeTimer < Time.time)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040004EA RID: 1258
	public float lifeTime = 2f;

	// Token: 0x040004EB RID: 1259
	private float lifeTimer;
}
