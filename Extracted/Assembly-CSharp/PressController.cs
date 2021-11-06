using System;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class PressController : MonoBehaviour
{
	// Token: 0x06002A0B RID: 10763 RVA: 0x000DD74C File Offset: 0x000DB94C
	private void OnTriggerEnter(Collider col)
	{
		if (col.transform.gameObject == WeaponManager.sharedManager.myPlayer)
		{
			if (this.isPrimary)
			{
				this.firstCollision = col.transform.gameObject;
				this.CheckSmash();
			}
			else
			{
				this.primaryPress.secondCollision = col.transform.gameObject;
				this.primaryPress.CheckSmash();
			}
		}
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x000DD7C0 File Offset: 0x000DB9C0
	private void OnTriggerExit(Collider col)
	{
		if (col.transform.gameObject == WeaponManager.sharedManager.myPlayer)
		{
			if (this.isPrimary)
			{
				this.firstCollision = null;
			}
			else
			{
				this.primaryPress.secondCollision = null;
			}
		}
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x000DD810 File Offset: 0x000DBA10
	public void CheckSmash()
	{
		if (this.firstCollision == this.secondCollision)
		{
			Player_move_c playerMoveC = this.firstCollision.GetComponent<SkinName>().playerMoveC;
			playerMoveC.KillSelf();
		}
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x000DD84C File Offset: 0x000DBA4C
	private void Update()
	{
	}

	// Token: 0x04001F07 RID: 7943
	public bool isPrimary;

	// Token: 0x04001F08 RID: 7944
	public PressController primaryPress;

	// Token: 0x04001F09 RID: 7945
	private GameObject firstCollision;

	// Token: 0x04001F0A RID: 7946
	[HideInInspector]
	public GameObject secondCollision;
}
