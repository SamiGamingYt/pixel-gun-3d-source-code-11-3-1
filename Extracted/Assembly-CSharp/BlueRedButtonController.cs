using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class BlueRedButtonController : MonoBehaviour
{
	// Token: 0x0600017F RID: 383 RVA: 0x0000F08C File Offset: 0x0000D28C
	private void Start()
	{
		if (!Defs.isFlag && !Defs.isCompany && !Defs.isCapturePoints)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000F0B4 File Offset: 0x0000D2B4
	private void Update()
	{
		this.countBlue = 0;
		this.countRed = 0;
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			if (Initializer.networkTables[i].myCommand == 1)
			{
				this.countBlue++;
			}
			if (Initializer.networkTables[i].myCommand == 2)
			{
				this.countRed++;
			}
		}
		this.isBlueAvalible = true;
		this.isRedAvalible = true;
		if (PhotonNetwork.room != null && (this.countBlue >= PhotonNetwork.room.maxPlayers / 2 || this.countBlue - this.countRed > 1))
		{
			this.isBlueAvalible = false;
		}
		if (PhotonNetwork.room != null && (this.countRed >= PhotonNetwork.room.maxPlayers / 2 || this.countRed - this.countBlue > 1))
		{
			this.isRedAvalible = false;
		}
		if (this.isBlueAvalible != this.blueButton.isEnabled)
		{
			this.blueButton.isEnabled = this.isBlueAvalible;
		}
		if (this.isRedAvalible != this.redButton.isEnabled)
		{
			this.redButton.isEnabled = this.isRedAvalible;
		}
	}

	// Token: 0x0400017B RID: 379
	public UIButton blueButton;

	// Token: 0x0400017C RID: 380
	public UIButton redButton;

	// Token: 0x0400017D RID: 381
	public bool isBlueAvalible = true;

	// Token: 0x0400017E RID: 382
	public bool isRedAvalible = true;

	// Token: 0x0400017F RID: 383
	public int countBlue;

	// Token: 0x04000180 RID: 384
	public int countRed;
}
