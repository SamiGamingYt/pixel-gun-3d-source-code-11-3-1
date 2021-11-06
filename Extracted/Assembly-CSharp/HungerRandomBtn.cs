using System;
using UnityEngine;

// Token: 0x020002AE RID: 686
public class HungerRandomBtn : MonoBehaviour
{
	// Token: 0x06001576 RID: 5494 RVA: 0x00055CE4 File Offset: 0x00053EE4
	private void OnClick()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().RandomRoomClickBtnInHunger();
		}
	}
}
