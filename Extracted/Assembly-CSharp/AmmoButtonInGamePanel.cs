using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public sealed class AmmoButtonInGamePanel : MonoBehaviour
{
	// Token: 0x06000048 RID: 72 RVA: 0x00004450 File Offset: 0x00002650
	private void Start()
	{
		this.priceLabel.text = Defs.ammoInGamePanelPrice.ToString();
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004468 File Offset: 0x00002668
	private void Update()
	{
		this.UpdateState(true);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00004474 File Offset: 0x00002674
	private void UpdateState(bool isDelta = true)
	{
		Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
		int currentAmmoInBackpack = weapon.currentAmmoInBackpack;
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		int maxAmmoWithEffectApplied = component.MaxAmmoWithEffectApplied;
		bool flag = currentAmmoInBackpack == maxAmmoWithEffectApplied;
		if (flag == this.myButton.isEnabled || !isDelta)
		{
			this.fullLabel.SetActive(flag);
			this.myButton.isEnabled = !flag;
			this.priceLabel.gameObject.SetActive(!flag);
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000450C File Offset: 0x0000270C
	private void OnEnable()
	{
		this.UpdateState(false);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00004518 File Offset: 0x00002718
	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
			weapon.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
			return;
		}
		ShopNGUIController.TryToBuy(this.inGameGui.gameObject, new ItemPrice(Defs.ammoInGamePanelPrice, "Coins"), delegate
		{
			if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
			{
				InGameGUI.sharedInGameGUI.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Ammo);
			}
			Weapon weapon2 = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			WeaponSounds component2 = weapon2.weaponPrefab.GetComponent<WeaponSounds>();
			weapon2.currentAmmoInBackpack = component2.MaxAmmoWithEffectApplied;
		}, new Action(JoystickController.leftJoystick.Reset), null, null, null, null);
	}

	// Token: 0x04000057 RID: 87
	public GameObject fullLabel;

	// Token: 0x04000058 RID: 88
	public UIButton myButton;

	// Token: 0x04000059 RID: 89
	public UILabel priceLabel;

	// Token: 0x0400005A RID: 90
	public InGameGUI inGameGui;
}
