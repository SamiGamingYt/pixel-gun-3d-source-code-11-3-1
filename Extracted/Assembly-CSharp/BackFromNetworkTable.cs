using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000024 RID: 36
public sealed class BackFromNetworkTable : MonoBehaviour
{
	// Token: 0x060000DE RID: 222 RVA: 0x0000810C File Offset: 0x0000630C
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Back From Network Table");
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00008148 File Offset: 0x00006348
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00008168 File Offset: 0x00006368
	private void OnClick()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().BackButtonPress();
		}
		else if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.CheckHideInternalPanel();
		}
	}

	// Token: 0x040000BD RID: 189
	private IDisposable _backSubscription;

	// Token: 0x040000BE RID: 190
	private bool offFriendsController;
}
