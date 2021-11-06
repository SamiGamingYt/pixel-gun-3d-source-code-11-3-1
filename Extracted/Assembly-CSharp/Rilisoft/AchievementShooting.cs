using System;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200050C RID: 1292
	public class AchievementShooting : Achievement
	{
		// Token: 0x06002D11 RID: 11537 RVA: 0x000EDB68 File Offset: 0x000EBD68
		public AchievementShooting(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base._data.WeaponCategories == null && !base._data.WeaponCategories.Any<ShopNGUIController.CategoryNames?>())
			{
				Debug.LogErrorFormat("achievement '{0}' without value", new object[]
				{
					base._data.Id
				});
				return;
			}
			Player_move_c.OnMyShootingStateSchanged += this.Player_move_c_OnMyShootingStateSchanged;
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000EDBD8 File Offset: 0x000EBDD8
		private void Player_move_c_OnMyShootingStateSchanged(bool obj)
		{
			if (!obj)
			{
				return;
			}
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null)
			{
				ShopNGUIController.CategoryNames value = (ShopNGUIController.CategoryNames)(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1);
				if (base._data.WeaponCategories.Contains(new ShopNGUIController.CategoryNames?(value)))
				{
					base.Gain(1);
				}
			}
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000EDC48 File Offset: 0x000EBE48
		public override void Dispose()
		{
			Player_move_c.OnMyShootingStateSchanged -= this.Player_move_c_OnMyShootingStateSchanged;
		}
	}
}
