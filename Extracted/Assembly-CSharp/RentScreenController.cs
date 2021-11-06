using System;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public sealed class RentScreenController : PropertyInfoScreenController
{
	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000E58D0 File Offset: 0x000E3AD0
	private Func<int, int> priceFormula
	{
		get
		{
			return delegate(int ind)
			{
				int result = 10;
				if (this._itemTag != null)
				{
					ItemRecord byTag = ItemDb.GetByTag(this._itemTag);
					ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null) ? this._itemTag : byTag.ShopId, (ShopNGUIController.CategoryNames)(-1));
					if (priceByShopId != null)
					{
						int num = -1;
						if (num == -1)
						{
							result = Mathf.RoundToInt((float)priceByShopId.Price * TempItemsController.PriceCoefs[this._itemTag][ind]);
						}
					}
				}
				return result;
			};
		}
	}

	// Token: 0x1700078B RID: 1931
	// (set) Token: 0x06002BA0 RID: 11168 RVA: 0x000E58E0 File Offset: 0x000E3AE0
	public string Header
	{
		set
		{
			foreach (UILabel uilabel in this.header)
			{
				if (uilabel != null && value != null)
				{
					uilabel.text = value;
				}
			}
		}
	}

	// Token: 0x1700078C RID: 1932
	// (set) Token: 0x06002BA1 RID: 11169 RVA: 0x000E5928 File Offset: 0x000E3B28
	public string RentFor
	{
		set
		{
			foreach (UILabel uilabel in this.rentFor)
			{
				if (uilabel != null && value != null && this._itemTag != null)
				{
					uilabel.text = string.Format(value, ItemDb.GetItemNameByTag(this._itemTag));
				}
			}
		}
	}

	// Token: 0x1700078D RID: 1933
	// (set) Token: 0x06002BA2 RID: 11170 RVA: 0x000E5988 File Offset: 0x000E3B88
	public string ItemTag
	{
		set
		{
			this._itemTag = value;
			if (this._itemTag == null)
			{
				return;
			}
			int itemCategory = ItemDb.GetItemCategory(this._itemTag);
			this.category = (ShopNGUIController.CategoryNames)itemCategory;
			ItemRecord byTag = ItemDb.GetByTag(this._itemTag);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null) ? this._itemTag : byTag.ShopId, (ShopNGUIController.CategoryNames)(-1));
			bool flag = priceByShopId != null && priceByShopId.Currency != null && priceByShopId.Currency.Equals("Coins");
			foreach (UILabel uilabel in (!flag) ? this.prices : this.pricesCoins)
			{
				if (uilabel != null)
				{
					uilabel.gameObject.SetActive(true);
					uilabel.text = this.priceFormula(Array.IndexOf<UILabel>((!flag) ? this.prices : this.pricesCoins, uilabel)).ToString();
				}
			}
			foreach (UILabel uilabel2 in (!flag) ? this.pricesCoins : this.prices)
			{
				if (uilabel2 != null)
				{
					uilabel2.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x000E5AF4 File Offset: 0x000E3CF4
	public override void Hide()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000E5B10 File Offset: 0x000E3D10
	public void HandleRentButton(UIButton b)
	{
		if (Defs.isSoundFX)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		int arg = Array.IndexOf<UIButton>(this.buttons, b);
		ItemRecord byTag = ItemDb.GetByTag(this._itemTag);
		ItemPrice priceByShopId = ItemDb.GetPriceByShopId((byTag == null || byTag.ShopId == null) ? this._itemTag : byTag.ShopId, (ShopNGUIController.CategoryNames)(-1));
		ItemPrice price = new ItemPrice(this.priceFormula(arg), (priceByShopId == null) ? "GemsCurrency" : priceByShopId.Currency);
		bool flag = TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(this._itemTag);
		ShopNGUIController.TryToBuy(this.window, price, delegate
		{
			if (!Wear.armorNumTemp.ContainsKey(this._itemTag ?? string.Empty) && this._itemTag != null)
			{
				bool flag2 = this._itemTag.Equals(WeaponTags.DragonGunRent_Tag) || this._itemTag.Equals(WeaponTags.PumpkinGunRent_Tag) || this._itemTag.Equals(WeaponTags.RayMinigunRent_Tag) || this._itemTag.Equals(WeaponTags.Red_StoneRent_Tag) || this._itemTag.Equals(WeaponTags.TwoBoltersRent_Tag);
			}
			Action<string> action = this.onPurchaseCustomAction;
			if (action != null)
			{
				action(this._itemTag);
			}
			if (TempItemsController.sharedController != null)
			{
				TempItemsController.sharedController.ExpiredItems.Remove(this._itemTag);
			}
			this.Hide();
		}, null, null, null, delegate
		{
			Action action = this.onEnterCoinsShopAdditionalAction;
			if (action != null)
			{
				action();
			}
		}, delegate
		{
			Action action = this.onExitCoinsShopAdditionalAction;
			if (action != null)
			{
				action();
			}
		});
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000E5BF4 File Offset: 0x000E3DF4
	public void HandleViewButton()
	{
		this.Hide();
		if (this._itemTag != null && TempItemsController.GunsMappingFromTempToConst.ContainsKey(this._itemTag))
		{
			string text = WeaponManager.FirstUnboughtOrForOurTier(TempItemsController.GunsMappingFromTempToConst[this._itemTag]);
			if (text != null)
			{
				int itemCategory = ItemDb.GetItemCategory(text);
				if (itemCategory != -1)
				{
				}
			}
		}
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000E5C54 File Offset: 0x000E3E54
	private void Awake()
	{
		this.rentButtonsPanel.SetActive(false);
		this.viewButtonPanel.SetActive(true);
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x000E5C70 File Offset: 0x000E3E70
	public static void SetDepthForExpGUI(int newDepth)
	{
		ExpController instance = ExpController.Instance;
		if (instance != null)
		{
			instance.experienceView.experienceCamera.depth = (float)newDepth;
		}
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000E5CA4 File Offset: 0x000E3EA4
	private void Start()
	{
		RentScreenController.SetDepthForExpGUI(89);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000E5CB0 File Offset: 0x000E3EB0
	private void OnDestroy()
	{
		RentScreenController.SetDepthForExpGUI(99);
	}

	// Token: 0x04002092 RID: 8338
	public GameObject viewButtonPanel;

	// Token: 0x04002093 RID: 8339
	public GameObject rentButtonsPanel;

	// Token: 0x04002094 RID: 8340
	public UIButton viewButton;

	// Token: 0x04002095 RID: 8341
	public GameObject window;

	// Token: 0x04002096 RID: 8342
	public UILabel[] header;

	// Token: 0x04002097 RID: 8343
	public UILabel[] rentFor;

	// Token: 0x04002098 RID: 8344
	public UILabel[] prices;

	// Token: 0x04002099 RID: 8345
	public UILabel[] pricesCoins;

	// Token: 0x0400209A RID: 8346
	public UIButton[] buttons;

	// Token: 0x0400209B RID: 8347
	public UITexture itemImage;

	// Token: 0x0400209C RID: 8348
	public Action<string> onPurchaseCustomAction;

	// Token: 0x0400209D RID: 8349
	public Action onEnterCoinsShopAdditionalAction;

	// Token: 0x0400209E RID: 8350
	public Action onExitCoinsShopAdditionalAction;

	// Token: 0x0400209F RID: 8351
	public Action<string> customEquipWearAction;

	// Token: 0x040020A0 RID: 8352
	private string _itemTag;

	// Token: 0x040020A1 RID: 8353
	private ShopNGUIController.CategoryNames category;
}
