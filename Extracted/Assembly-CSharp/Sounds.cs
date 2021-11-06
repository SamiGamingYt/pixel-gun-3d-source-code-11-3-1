using System;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
public class Sounds : MonoBehaviour
{
	// Token: 0x04003591 RID: 13713
	public AudioClip hurt;

	// Token: 0x04003592 RID: 13714
	public AudioClip voice;

	// Token: 0x04003593 RID: 13715
	public AudioClip bite;

	// Token: 0x04003594 RID: 13716
	public AudioClip death;

	// Token: 0x04003595 RID: 13717
	public float notAttackingSpeed = 1f;

	// Token: 0x04003596 RID: 13718
	public float attackingSpeed = 1f;

	// Token: 0x04003597 RID: 13719
	public float health = 100f;

	// Token: 0x04003598 RID: 13720
	public float attackDistance = 3f;

	// Token: 0x04003599 RID: 13721
	public float timeToHit = 2f;

	// Token: 0x0400359A RID: 13722
	public float detectRadius = 17f;

	// Token: 0x0400359B RID: 13723
	public int damagePerHit = 1;

	// Token: 0x0400359C RID: 13724
	public int scorePerKill = 50;

	// Token: 0x0400359D RID: 13725
	public float[] attackingSpeedRandomRange = new float[]
	{
		-0.5f,
		0.5f
	};
}
