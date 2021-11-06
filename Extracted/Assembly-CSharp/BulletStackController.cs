using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class BulletStackController : MonoBehaviour
{
	// Token: 0x06000203 RID: 515 RVA: 0x00013248 File Offset: 0x00011448
	private void Start()
	{
		BulletStackController.sharedController = this;
		base.transform.position = Vector3.zero;
		for (int i = 0; i < 6; i++)
		{
			this.currentIndexBullet[i] = 0;
		}
		this.bullets = new GameObject[6][];
		this.bullets[0] = new GameObject[this.standartBulletStack.childCount];
		for (int j = 0; j < this.bullets[0].Length; j++)
		{
			this.bullets[0][j] = this.standartBulletStack.GetChild(j).gameObject;
		}
		this.bullets[1] = new GameObject[this.redBulletStack.childCount];
		for (int k = 0; k < this.bullets[1].Length; k++)
		{
			this.bullets[1][k] = this.redBulletStack.GetChild(k).gameObject;
		}
		this.bullets[2] = new GameObject[this.for252BulletStack.childCount];
		for (int l = 0; l < this.bullets[2].Length; l++)
		{
			this.bullets[2][l] = this.for252BulletStack.GetChild(l).gameObject;
		}
		this.bullets[3] = new GameObject[this.turquoiseBulletStack.childCount];
		for (int m = 0; m < this.bullets[3].Length; m++)
		{
			this.bullets[3][m] = this.turquoiseBulletStack.GetChild(m).gameObject;
		}
		this.bullets[4] = new GameObject[this.greenBulletStack.childCount];
		for (int n = 0; n < this.bullets[4].Length; n++)
		{
			this.bullets[4][n] = this.greenBulletStack.GetChild(n).gameObject;
		}
		this.bullets[5] = new GameObject[this.violetBulletStack.childCount];
		for (int num = 0; num < this.bullets[5].Length; num++)
		{
			this.bullets[5][num] = this.violetBulletStack.GetChild(num).gameObject;
		}
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00013478 File Offset: 0x00011678
	public GameObject GetCurrentBullet(int type = 0)
	{
		if (type < 0)
		{
			return null;
		}
		this.currentIndexBullet[type]++;
		if (this.currentIndexBullet[type] >= this.bullets[type].Length)
		{
			this.currentIndexBullet[type] = 0;
		}
		return this.bullets[type][this.currentIndexBullet[type]];
	}

	// Token: 0x06000205 RID: 517 RVA: 0x000134D0 File Offset: 0x000116D0
	private void OnDestroy()
	{
		BulletStackController.sharedController = null;
	}

	// Token: 0x04000230 RID: 560
	public static BulletStackController sharedController;

	// Token: 0x04000231 RID: 561
	public Transform standartBulletStack;

	// Token: 0x04000232 RID: 562
	public Transform redBulletStack;

	// Token: 0x04000233 RID: 563
	public Transform for252BulletStack;

	// Token: 0x04000234 RID: 564
	public Transform turquoiseBulletStack;

	// Token: 0x04000235 RID: 565
	public Transform greenBulletStack;

	// Token: 0x04000236 RID: 566
	public Transform violetBulletStack;

	// Token: 0x04000237 RID: 567
	public GameObject[][] bullets;

	// Token: 0x04000238 RID: 568
	private int[] currentIndexBullet = new int[6];
}
