using System;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public class RemovePanelAuth : MonoBehaviour
{
	// Token: 0x06002B98 RID: 11160 RVA: 0x000E5780 File Offset: 0x000E3980
	private void Start()
	{
		this.startTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000E5790 File Offset: 0x000E3990
	private void Update()
	{
		if (!this.isDestroyed && Time.realtimeSinceStartup - this.startTime >= this.lifetime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			this.isDestroyed = true;
		}
	}

	// Token: 0x0400208D RID: 8333
	public float lifetime = 0.7f;

	// Token: 0x0400208E RID: 8334
	private float startTime;

	// Token: 0x0400208F RID: 8335
	private bool isDestroyed;
}
