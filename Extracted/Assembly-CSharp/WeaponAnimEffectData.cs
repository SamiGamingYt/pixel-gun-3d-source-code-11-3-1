using System;
using UnityEngine;

// Token: 0x02000785 RID: 1925
[Serializable]
public class WeaponAnimEffectData
{
	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x060043B1 RID: 17329 RVA: 0x0016A998 File Offset: 0x00168B98
	// (set) Token: 0x060043B2 RID: 17330 RVA: 0x0016A9A0 File Offset: 0x00168BA0
	public float animationLength { get; set; }

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x060043B3 RID: 17331 RVA: 0x0016A9AC File Offset: 0x00168BAC
	// (set) Token: 0x060043B4 RID: 17332 RVA: 0x0016A9B4 File Offset: 0x00168BB4
	public bool isPlaying { get; set; }

	// Token: 0x04003168 RID: 12648
	public string animationName;

	// Token: 0x04003169 RID: 12649
	public bool isLoop = true;

	// Token: 0x0400316A RID: 12650
	[Tooltip("Запрещает перезапуск эффекта пока от играет")]
	public bool blockAtPlay = true;

	// Token: 0x0400316B RID: 12651
	[Tooltip("Количество испускаемых частиц при старте анимации. Если количество частиц не указано, то испольхуется Play а не Emit")]
	public int EmitCount = -1;

	// Token: 0x0400316C RID: 12652
	public ParticleSystem[] particleSystems;
}
