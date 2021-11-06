using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F8 RID: 2040
public class ItemRecord
{
	// Token: 0x06004A22 RID: 18978 RVA: 0x0019B74C File Offset: 0x0019994C
	public ItemRecord(int id, string tag, string storageId, string prefabName, string shopId, string shopDisplayName, bool canBuy, bool deactivated, List<ItemPrice> UNUSED_pricesForDiffTiers, bool useImageOfFirstUpgrade = false)
	{
		this.SetMainFields(id, tag, storageId, prefabName, shopId, shopDisplayName, canBuy, deactivated, useImageOfFirstUpgrade);
	}

	// Token: 0x06004A23 RID: 18979 RVA: 0x0019B774 File Offset: 0x00199974
	public ItemRecord(int id, string tag, string storageId, string prefabName, string shopId, string shopDisplayName, int UNUSED_price, bool canBuy, bool deactivated, string UNUSED_currency = "Coins", int UNUSED_secondCurrencyPrice = -1, bool useImageOfFirstUpgrade = false)
	{
		this.SetMainFields(id, tag, storageId, prefabName, shopId, shopDisplayName, canBuy, deactivated, useImageOfFirstUpgrade);
	}

	// Token: 0x06004A24 RID: 18980 RVA: 0x0019B79C File Offset: 0x0019999C
	private void SetMainFields(int id, string tag, string storageId, string prefabName, string shopId, string shopDisplayName, bool canBuy, bool deactivated, bool useImageOfFirstUpgrade)
	{
		this.Id = id;
		this.Tag = tag;
		this.StorageId = storageId;
		this.PrefabName = prefabName;
		this.ShopId = shopId;
		this.ShopDisplayName = shopDisplayName;
		this.CanBuy = canBuy;
		this.Deactivated = deactivated;
		this.UseImagesFromFirstUpgrade = useImageOfFirstUpgrade;
	}

	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x06004A25 RID: 18981 RVA: 0x0019B7F0 File Offset: 0x001999F0
	// (set) Token: 0x06004A26 RID: 18982 RVA: 0x0019B7F8 File Offset: 0x001999F8
	public int Id { get; private set; }

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x06004A27 RID: 18983 RVA: 0x0019B804 File Offset: 0x00199A04
	// (set) Token: 0x06004A28 RID: 18984 RVA: 0x0019B80C File Offset: 0x00199A0C
	public string Tag { get; private set; }

	// Token: 0x17000C2D RID: 3117
	// (get) Token: 0x06004A29 RID: 18985 RVA: 0x0019B818 File Offset: 0x00199A18
	// (set) Token: 0x06004A2A RID: 18986 RVA: 0x0019B820 File Offset: 0x00199A20
	public string StorageId { get; private set; }

	// Token: 0x17000C2E RID: 3118
	// (get) Token: 0x06004A2B RID: 18987 RVA: 0x0019B82C File Offset: 0x00199A2C
	// (set) Token: 0x06004A2C RID: 18988 RVA: 0x0019B834 File Offset: 0x00199A34
	public string PrefabName { get; private set; }

	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x06004A2D RID: 18989 RVA: 0x0019B840 File Offset: 0x00199A40
	// (set) Token: 0x06004A2E RID: 18990 RVA: 0x0019B848 File Offset: 0x00199A48
	public string ShopId { get; private set; }

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x06004A2F RID: 18991 RVA: 0x0019B854 File Offset: 0x00199A54
	// (set) Token: 0x06004A30 RID: 18992 RVA: 0x0019B85C File Offset: 0x00199A5C
	public string ShopDisplayName { get; private set; }

	// Token: 0x17000C31 RID: 3121
	// (get) Token: 0x06004A31 RID: 18993 RVA: 0x0019B868 File Offset: 0x00199A68
	// (set) Token: 0x06004A32 RID: 18994 RVA: 0x0019B870 File Offset: 0x00199A70
	public bool CanBuy { get; private set; }

	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x06004A33 RID: 18995 RVA: 0x0019B87C File Offset: 0x00199A7C
	// (set) Token: 0x06004A34 RID: 18996 RVA: 0x0019B884 File Offset: 0x00199A84
	public bool Deactivated { get; private set; }

	// Token: 0x17000C33 RID: 3123
	// (get) Token: 0x06004A35 RID: 18997 RVA: 0x0019B890 File Offset: 0x00199A90
	// (set) Token: 0x06004A36 RID: 18998 RVA: 0x0019B898 File Offset: 0x00199A98
	public bool UseImagesFromFirstUpgrade { get; private set; }

	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06004A37 RID: 18999 RVA: 0x0019B8A4 File Offset: 0x00199AA4
	public ItemPrice Price
	{
		get
		{
			try
			{
				List<ItemPrice> list;
				if (BalanceController.GunPricesFromServer.TryGetValue(this.PrefabName, out list) && list != null)
				{
					string a = WeaponManager.FirstTagForOurTier(this.Tag, null);
					int num = (!(a == this.Tag)) ? 1 : 0;
					if (list.Count > num && list[num] != null)
					{
						return list[num];
					}
					Debug.LogError("listServerPrices.Count > index && listServerPrices [index] != null: Tag = " + this.Tag);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Exception in ItemRecord.Price: ",
					this.PrefabName ?? "null",
					" exception: ",
					ex
				}));
			}
			return new ItemPrice(200, "Coins");
		}
	}

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x06004A38 RID: 19000 RVA: 0x0019B9A4 File Offset: 0x00199BA4
	public bool TemporaryGun
	{
		get
		{
			return this.ShopId != null && this.StorageId == null;
		}
	}
}
