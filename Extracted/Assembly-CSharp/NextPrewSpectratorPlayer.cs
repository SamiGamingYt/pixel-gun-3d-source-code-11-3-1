using System;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class NextPrewSpectratorPlayer : MonoBehaviour
{
	// Token: 0x06002350 RID: 9040 RVA: 0x000AF6C0 File Offset: 0x000AD8C0
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().ResetCamPlayer((!base.gameObject.name.Equals("PrewMode")) ? 1 : -1);
		}
	}
}
