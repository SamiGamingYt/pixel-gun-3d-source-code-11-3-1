using System;
using UnityEngine;

// Token: 0x020002AB RID: 683
public class HoleBulletStackController : MonoBehaviour
{
	// Token: 0x06001562 RID: 5474 RVA: 0x00055640 File Offset: 0x00053840
	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		HoleBulletStackController.sharedController = this;
		base.transform.position = Vector3.zero;
		GameObject[] array = GameObject.FindGameObjectsWithTag("HoleBullet");
		this.holes = new HoleScript[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			this.holes[i] = array[i].GetComponent<HoleScript>();
			this.holes[i].Init();
		}
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000556C4 File Offset: 0x000538C4
	public HoleScript GetCurrentHole(bool _isUseMine)
	{
		bool flag = true;
		for (;;)
		{
			this.currentIndexHole++;
			if (this.currentIndexHole >= this.holes.Length)
			{
				if (!flag)
				{
					break;
				}
				this.currentIndexHole = 0;
				flag = false;
			}
			if (!this.holes[this.currentIndexHole].isUseMine || _isUseMine)
			{
				goto IL_51;
			}
		}
		return null;
		IL_51:
		return this.holes[this.currentIndexHole];
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x00055730 File Offset: 0x00053930
	private void OnDestroy()
	{
		HoleBulletStackController.sharedController = null;
	}

	// Token: 0x04000CB7 RID: 3255
	public static HoleBulletStackController sharedController;

	// Token: 0x04000CB8 RID: 3256
	public HoleScript[] holes;

	// Token: 0x04000CB9 RID: 3257
	private int currentIndexHole;
}
