using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class FacebookInNetworkTableBtn : MonoBehaviour
{
	// Token: 0x06000567 RID: 1383 RVA: 0x0002B594 File Offset: 0x00029794
	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0002B5B0 File Offset: 0x000297B0
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().PostFacebookBtnClick();
		}
	}
}
