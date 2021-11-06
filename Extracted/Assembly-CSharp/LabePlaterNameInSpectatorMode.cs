using System;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class LabePlaterNameInSpectatorMode : MonoBehaviour
{
	// Token: 0x06001A1B RID: 6683 RVA: 0x000696E0 File Offset: 0x000678E0
	private void Start()
	{
		this.label = base.GetComponent<UILabel>();
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000696F0 File Offset: 0x000678F0
	private void Update()
	{
		if (this.label != null && WeaponManager.sharedManager.myTable != null)
		{
			this.label.text = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().playerVidosNick;
			this.clanNameLabel.text = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().playerVidosClanName;
			this.clanTexture.mainTexture = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().playerVidosClanTexture;
		}
	}

	// Token: 0x04000F37 RID: 3895
	private UILabel label;

	// Token: 0x04000F38 RID: 3896
	public UILabel clanNameLabel;

	// Token: 0x04000F39 RID: 3897
	public UITexture clanTexture;
}
