using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000724 RID: 1828
	public sealed class WeaponSlotAccumulativeQuest : AccumulativeQuestBase
	{
		// Token: 0x06003FB9 RID: 16313 RVA: 0x001551CC File Offset: 0x001533CC
		public WeaponSlotAccumulativeQuest(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded, int requiredCount, ShopNGUIController.CategoryNames weaponSlot, int initialCount = 0) : base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
			this._weaponSlot = weaponSlot;
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003FBA RID: 16314 RVA: 0x001551F8 File Offset: 0x001533F8
		public ShopNGUIController.CategoryNames WeaponSlot
		{
			get
			{
				return this._weaponSlot;
			}
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x00155200 File Offset: 0x00153400
		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			base.AppendProperties(properties);
			properties["weaponSlot"] = this._weaponSlot;
		}

		// Token: 0x04002EE0 RID: 12000
		private readonly ShopNGUIController.CategoryNames _weaponSlot;
	}
}
