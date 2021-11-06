using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x0200086B RID: 2155
public class TryGunScreenController : MonoBehaviour
{
	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x06004DCE RID: 19918 RVA: 0x001C2D88 File Offset: 0x001C0F88
	// (set) Token: 0x06004DCF RID: 19919 RVA: 0x001C2D90 File Offset: 0x001C0F90
	public bool ExpiredTryGun
	{
		get
		{
			return this._expiredTryGun;
		}
		set
		{
			try
			{
				this._expiredTryGun = value;
				this.backButton.SetActive(value);
				this.buyPanel.SetActive(value);
				this.equipPanel.SetActive(!value);
				this.gemsPrice.SetActive(value && this.price.Currency == "GemsCurrency");
				this.gemsPriceOld.SetActive(value && this.price.Currency == "GemsCurrency");
				this.coinsPrice.SetActive(value && this.price.Currency == "Coins");
				this.coinsPriceOld.SetActive(value && this.price.Currency == "Coins");
				this.headSpecialOffer.SetActive(!value);
				this.headExpired.SetActive(value);
				if (value)
				{
					if (this.price.Currency == "GemsCurrency")
					{
						this.gemsPrice.GetComponent<UILabel>().text = this.price.Price.ToString();
						this.gemsPriceOld.GetComponent<UILabel>().text = this.priceWithoutPromo.Price.ToString();
					}
					if (this.price.Currency == "Coins")
					{
						this.coinsPrice.GetComponent<UILabel>().text = this.price.Price.ToString();
						this.coinsPriceOld.GetComponent<UILabel>().text = this.priceWithoutPromo.Price.ToString();
					}
					try
					{
						foreach (UILabel uilabel in this.discountLabels)
						{
							bool flag;
							uilabel.text = string.Format(LocalizationStore.Get("Key_1996"), Mathf.RoundToInt(WeaponManager.TryGunPromoDuration() / 60f), ShopNGUIController.DiscountFor(this.ItemTag, out flag));
						}
					}
					catch (Exception arg)
					{
						Debug.LogError("Exception in setting up discount in try gun screen: " + arg);
					}
					AnalyticsStuff.LogWEaponsSpecialOffers_Conversion(true, null);
				}
				else
				{
					int num = (!ABTestController.useBuffSystem) ? KillRateCheck.instance.GetRoundsForGun() : BuffSystem.instance.GetRoundsForGun();
					foreach (UILabel uilabel2 in this.numberOfMatchesLabels)
					{
						uilabel2.text = string.Format(LocalizationStore.Get("Key_1995"), num);
					}
				}
			}
			catch (Exception arg2)
			{
				Debug.LogError("Exception in ExpiredTryGun: " + arg2);
			}
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x001C30EC File Offset: 0x001C12EC
	// (set) Token: 0x06004DD1 RID: 19921 RVA: 0x001C30F4 File Offset: 0x001C12F4
	public string ItemTag
	{
		get
		{
			return this._itemTag;
		}
		set
		{
			try
			{
				this._itemTag = value;
				this.category = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(this._itemTag);
				this.price = ShopNGUIController.GetItemPrice(this._itemTag, this.category, false, true, false);
				this.priceWithoutPromo = ShopNGUIController.GetItemPrice(this._itemTag, this.category, false, false, false);
				Texture itemIcon = ItemDb.GetItemIcon(this._itemTag, this.category, null, true);
				if (itemIcon != null && this.itemImage != null)
				{
					this.itemImage.mainTexture = itemIcon;
				}
				string itemNameByTag = ItemDb.GetItemNameByTag(this._itemTag);
				foreach (UILabel uilabel in this.itemNameLabels)
				{
					uilabel.text = itemNameByTag;
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in ItemTag: " + arg);
			}
		}
	}

	// Token: 0x06004DD2 RID: 19922 RVA: 0x001C3230 File Offset: 0x001C1430
	public void HandleEquip()
	{
		try
		{
			WeaponManager.sharedManager.AddTryGun(this.ItemTag);
			if (ABTestController.useBuffSystem)
			{
				BuffSystem.instance.SetGetTryGun(ItemDb.GetByTag(this.ItemTag).PrefabName);
			}
			else
			{
				KillRateCheck.instance.SetGetWeapon();
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("TryGunScreenController HandleEquip exception: " + arg);
		}
	}

	// Token: 0x06004DD3 RID: 19923 RVA: 0x001C32B8 File Offset: 0x001C14B8
	public void HandleClose()
	{
		this.DestroyScreen();
	}

	// Token: 0x06004DD4 RID: 19924 RVA: 0x001C32C0 File Offset: 0x001C14C0
	private void DestroyScreen()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001C32DC File Offset: 0x001C14DC
	public void HandleBuy()
	{
		int priceAmount = this.price.Price;
		string priceCurrency = this.price.Currency;
		ShopNGUIController.TryToBuy(base.gameObject, this.price, delegate
		{
			if (Defs.isSoundFX)
			{
			}
			ShopNGUIController.FireWeaponOrArmorBought();
			ShopNGUIController.ProvideItem(this.category, this.ItemTag, 1, false, 0, delegate(string item)
			{
				if (ShopNGUIController.sharedShop != null)
				{
					ShopNGUIController.sharedShop.FireBuyAction(item);
				}
			}, null, true, true, false);
			try
			{
				string empty = string.Empty;
				string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(WeaponManager.LastBoughtTag(this.ItemTag, null) ?? WeaponManager.FirstUnboughtTag(this.ItemTag), empty, this.category, null);
				bool isDaterWeapon = false;
				if (ShopNGUIController.IsWeaponCategory(this.category))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(this.ItemTag);
					isDaterWeapon = (weaponInfo != null && weaponInfo.IsAvalibleFromFilter(3));
				}
				string categoryParameterName = AnalyticsConstants.GetSalesName(this.category) ?? this.category.ToString();
				AnalyticsStuff.LogSales(itemNameNonLocalized, categoryParameterName, isDaterWeapon);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, AnalyticsStuff.AnalyticsReadableCategoryNameFromOldCategoryName(categoryParameterName), 1, priceAmount, priceCurrency);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in loggin in Try Gun Screen Controller: " + arg);
			}
			this.DestroyScreen();
		}, null, null, null, delegate
		{
		}, delegate
		{
		});
	}

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001C3374 File Offset: 0x001C1574
	private void Start()
	{
		this._escapeSubscription = BackSystem.Instance.Register(delegate
		{
			if (!this.ExpiredTryGun)
			{
				this.HandleEquip();
			}
			this.HandleClose();
		}, "Try Gun Screen");
	}

	// Token: 0x06004DD7 RID: 19927 RVA: 0x001C3398 File Offset: 0x001C1598
	private void OnDestroy()
	{
		this._escapeSubscription.Dispose();
	}

	// Token: 0x04003C74 RID: 15476
	public GameObject buyPanel;

	// Token: 0x04003C75 RID: 15477
	public GameObject equipPanel;

	// Token: 0x04003C76 RID: 15478
	public GameObject backButton;

	// Token: 0x04003C77 RID: 15479
	public GameObject gemsPrice;

	// Token: 0x04003C78 RID: 15480
	public GameObject gemsPriceOld;

	// Token: 0x04003C79 RID: 15481
	public GameObject coinsPrice;

	// Token: 0x04003C7A RID: 15482
	public GameObject coinsPriceOld;

	// Token: 0x04003C7B RID: 15483
	public UITexture itemImage;

	// Token: 0x04003C7C RID: 15484
	public List<UILabel> itemNameLabels;

	// Token: 0x04003C7D RID: 15485
	public GameObject headSpecialOffer;

	// Token: 0x04003C7E RID: 15486
	public GameObject headExpired;

	// Token: 0x04003C7F RID: 15487
	public List<UILabel> numberOfMatchesLabels;

	// Token: 0x04003C80 RID: 15488
	public List<UILabel> discountLabels;

	// Token: 0x04003C81 RID: 15489
	public Action<string> onPurchaseCustomAction;

	// Token: 0x04003C82 RID: 15490
	public Action onEnterCoinsShopAdditionalAction;

	// Token: 0x04003C83 RID: 15491
	public Action onExitCoinsShopAdditionalAction;

	// Token: 0x04003C84 RID: 15492
	public Action<string> customEquipWearAction;

	// Token: 0x04003C85 RID: 15493
	private string _itemTag;

	// Token: 0x04003C86 RID: 15494
	private ShopNGUIController.CategoryNames category;

	// Token: 0x04003C87 RID: 15495
	private ItemPrice price;

	// Token: 0x04003C88 RID: 15496
	private ItemPrice priceWithoutPromo;

	// Token: 0x04003C89 RID: 15497
	private bool _expiredTryGun;

	// Token: 0x04003C8A RID: 15498
	private IDisposable _escapeSubscription;
}
