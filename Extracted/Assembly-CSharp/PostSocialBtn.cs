using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000495 RID: 1173
internal sealed class PostSocialBtn : MonoBehaviour
{
	// Token: 0x060029E6 RID: 10726 RVA: 0x000DD150 File Offset: 0x000DB350
	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000DD16C File Offset: 0x000DB36C
	private void OnClick()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Debug.Log("PostSocialBtn");
		ButtonClickSound.Instance.PlayClick();
		if (this.isFacebook)
		{
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().PostFacebookBtnClick();
			}
		}
		else if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().PostTwitterBtnClick();
		}
	}

	// Token: 0x04001EFF RID: 7935
	public bool isFacebook;
}
