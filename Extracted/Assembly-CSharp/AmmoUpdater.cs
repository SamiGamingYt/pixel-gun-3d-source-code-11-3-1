using System;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft;
using UnityEngine;

// Token: 0x02000016 RID: 22
internal sealed class AmmoUpdater : MonoBehaviour
{
	// Token: 0x0600004F RID: 79 RVA: 0x00004694 File Offset: 0x00002894
	private void Start()
	{
		this._label = base.GetComponent<UILabel>();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000046A4 File Offset: 0x000028A4
	private void Update()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null)
		{
			Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
			if ((!component.isMelee || component.isShotMelee) && this._label != null)
			{
				this._label.text = ((!component.isShotMelee) ? this.FormatShootingAmmoLabel(weapon.currentAmmoInClip, weapon.currentAmmoInBackpack) : this.FormatMeleeAmmoLabel(weapon.currentAmmoInClip, weapon.currentAmmoInBackpack));
				if (this.ammoSprite != null && !this.ammoSprite.activeSelf)
				{
					this.ammoSprite.SetActive(true);
				}
				return;
			}
		}
		this._label.text = string.Empty;
		if (this.ammoSprite != null && this.ammoSprite.activeSelf)
		{
			this.ammoSprite.SetActive(false);
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000047D4 File Offset: 0x000029D4
	private string FormatMeleeAmmoLabel(int currentAmmoInClip, int currentAmmoInBackpack)
	{
		int num = currentAmmoInClip + currentAmmoInBackpack;
		if (num != this._formatMeleeAmmoMemo.Key)
		{
			string value = num.ToString(CultureInfo.InvariantCulture);
			this._formatMeleeAmmoMemo = new KeyValuePair<int, string>(num, value);
		}
		return this._formatMeleeAmmoMemo.Value;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000481C File Offset: 0x00002A1C
	private string FormatShootingAmmoLabel(int currentAmmoInClip, int currentAmmoInBackpack)
	{
		Ammo key = new Ammo(currentAmmoInClip, currentAmmoInBackpack);
		if (!key.Equals(this._formatShootingAmmoMemo.Key))
		{
			string value = currentAmmoInClip + "/" + currentAmmoInBackpack;
			this._formatShootingAmmoMemo = new KeyValuePair<Ammo, string>(key, value);
		}
		return this._formatShootingAmmoMemo.Value ?? "0/0";
	}

	// Token: 0x0400005C RID: 92
	private UILabel _label;

	// Token: 0x0400005D RID: 93
	public GameObject ammoSprite;

	// Token: 0x0400005E RID: 94
	private KeyValuePair<int, string> _formatMeleeAmmoMemo = new KeyValuePair<int, string>(0, "0");

	// Token: 0x0400005F RID: 95
	private KeyValuePair<Ammo, string> _formatShootingAmmoMemo = new KeyValuePair<Ammo, string>(new Ammo(0, 0), "0/0");
}
