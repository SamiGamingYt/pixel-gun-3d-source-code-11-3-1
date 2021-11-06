using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x0200072E RID: 1838
	public sealed class KillMonsterEventArgs : EventArgs
	{
		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06004039 RID: 16441 RVA: 0x00156B0C File Offset: 0x00154D0C
		// (set) Token: 0x0600403A RID: 16442 RVA: 0x00156B14 File Offset: 0x00154D14
		public ShopNGUIController.CategoryNames WeaponSlot { get; set; }

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x0600403B RID: 16443 RVA: 0x00156B20 File Offset: 0x00154D20
		// (set) Token: 0x0600403C RID: 16444 RVA: 0x00156B28 File Offset: 0x00154D28
		public bool Campaign { get; set; }

		// Token: 0x0600403D RID: 16445 RVA: 0x00156B34 File Offset: 0x00154D34
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"weaponSlot",
					this.WeaponSlot
				},
				{
					"campaign",
					this.Campaign
				}
			};
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x00156B78 File Offset: 0x00154D78
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
