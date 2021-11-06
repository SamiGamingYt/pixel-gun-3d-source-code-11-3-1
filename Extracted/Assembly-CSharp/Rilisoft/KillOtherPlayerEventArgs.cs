using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x0200072C RID: 1836
	public sealed class KillOtherPlayerEventArgs : EventArgs
	{
		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x00156960 File Offset: 0x00154B60
		// (set) Token: 0x06004024 RID: 16420 RVA: 0x00156968 File Offset: 0x00154B68
		public ConnectSceneNGUIController.RegimGame Mode { get; set; }

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06004025 RID: 16421 RVA: 0x00156974 File Offset: 0x00154B74
		// (set) Token: 0x06004026 RID: 16422 RVA: 0x0015697C File Offset: 0x00154B7C
		public ShopNGUIController.CategoryNames WeaponSlot { get; set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x00156988 File Offset: 0x00154B88
		// (set) Token: 0x06004028 RID: 16424 RVA: 0x00156990 File Offset: 0x00154B90
		public bool Headshot { get; set; }

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06004029 RID: 16425 RVA: 0x0015699C File Offset: 0x00154B9C
		// (set) Token: 0x0600402A RID: 16426 RVA: 0x001569A4 File Offset: 0x00154BA4
		public bool Grenade { get; set; }

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x001569B0 File Offset: 0x00154BB0
		// (set) Token: 0x0600402C RID: 16428 RVA: 0x001569B8 File Offset: 0x00154BB8
		public bool Revenge { get; set; }

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x001569C4 File Offset: 0x00154BC4
		// (set) Token: 0x0600402E RID: 16430 RVA: 0x001569CC File Offset: 0x00154BCC
		public bool VictimIsFlagCarrier { get; set; }

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x001569D8 File Offset: 0x00154BD8
		// (set) Token: 0x06004030 RID: 16432 RVA: 0x001569E0 File Offset: 0x00154BE0
		public bool IsInvisible { get; set; }

		// Token: 0x06004031 RID: 16433 RVA: 0x001569EC File Offset: 0x00154BEC
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"mode",
					this.Mode
				},
				{
					"weaponSlot",
					this.WeaponSlot
				},
				{
					"headshot",
					this.Headshot
				},
				{
					"grenade",
					this.Grenade
				},
				{
					"revenge",
					this.Revenge
				},
				{
					"victimIsFlagCarrier",
					this.VictimIsFlagCarrier
				},
				{
					"isInvisible",
					this.IsInvisible
				}
			};
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00156A9C File Offset: 0x00154C9C
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
