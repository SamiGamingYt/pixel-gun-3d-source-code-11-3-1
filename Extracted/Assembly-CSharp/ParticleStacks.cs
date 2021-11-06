using System;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class ParticleStacks : MonoBehaviour
{
	// Token: 0x06002382 RID: 9090 RVA: 0x000B0C08 File Offset: 0x000AEE08
	private void Awake()
	{
		ParticleStacks.instance = this;
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000B0C10 File Offset: 0x000AEE10
	private void OnDestroy()
	{
		ParticleStacks.instance = null;
	}

	// Token: 0x040017FB RID: 6139
	public static ParticleStacks instance;

	// Token: 0x040017FC RID: 6140
	public ParticleStackController fireStack;

	// Token: 0x040017FD RID: 6141
	public ParticleStackController lightningStack;

	// Token: 0x040017FE RID: 6142
	public HitStackController hitStack;

	// Token: 0x040017FF RID: 6143
	public HitStackController poisonHitStack;

	// Token: 0x04001800 RID: 6144
	public HitStackController criticalHitStack;

	// Token: 0x04001801 RID: 6145
	public HitStackController bleedHitStack;

	// Token: 0x04001802 RID: 6146
	public GameObject dragonPrefab;
}
