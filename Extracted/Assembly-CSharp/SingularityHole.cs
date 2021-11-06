using System;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public class SingularityHole : MonoBehaviour
{
	// Token: 0x060047BF RID: 18367 RVA: 0x0018C358 File Offset: 0x0018A558
	private void OnEnable()
	{
		Initializer.singularities.Add(this);
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x0018C368 File Offset: 0x0018A568
	private void DestroyByNetwork()
	{
		PhotonNetwork.Destroy(base.gameObject);
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x0018C378 File Offset: 0x0018A578
	private void OnDisable()
	{
		Initializer.singularities.Remove(this);
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x0018C388 File Offset: 0x0018A588
	public float GetForce(float distance)
	{
		if (distance < this.outRadius * this.outRadius && distance > this.inRadius * this.inRadius)
		{
			return Mathf.Clamp(this.minForce + (this.maxForce - this.minForce) * (1f - distance / (this.outRadius * this.outRadius)), this.minForce, this.maxForce);
		}
		if (distance < this.inRadius * this.inRadius)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x040034DA RID: 13530
	public Player_move_c owner;

	// Token: 0x040034DB RID: 13531
	public float outRadius = 15f;

	// Token: 0x040034DC RID: 13532
	public float inRadius = 2f;

	// Token: 0x040034DD RID: 13533
	public float maxForce = 10f;

	// Token: 0x040034DE RID: 13534
	public float minForce = 1f;
}
