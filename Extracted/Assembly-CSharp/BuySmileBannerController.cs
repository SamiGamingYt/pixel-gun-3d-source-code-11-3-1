using System;
using Rilisoft;
using Rilisoft.NullExtensions;

// Token: 0x02000052 RID: 82
public sealed class BuySmileBannerController : BannerWindow
{
	// Token: 0x0600021F RID: 543 RVA: 0x00013804 File Offset: 0x00011A04
	public static string GetCurrentBuySmileContextName()
	{
		return (!(FriendsWindowGUI.Instance != null) || !FriendsWindowGUI.Instance.InterfaceEnabled) ? ((!(ChatViewrController.sharedController != null)) ? "Lobby" : "Sandbox") : "Friends";
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0001385C File Offset: 0x00011A5C
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Buy Smiley Banner");
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00013898 File Offset: 0x00011A98
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06000222 RID: 546 RVA: 0x000138B8 File Offset: 0x00011AB8
	private void HandleEscape()
	{
		if (FriendsWindowGUI.Instance.Map((FriendsWindowGUI f) => f.InterfaceEnabled) || ChatViewrController.sharedController != null)
		{
			this.OnCloseClick();
		}
		else if (BannerWindowController.SharedController.Map((BannerWindowController b) => b.IsBannerShow(BannerWindowType.buySmiles)))
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	// Token: 0x06000223 RID: 547 RVA: 0x00013944 File Offset: 0x00011B44
	public void OnCloseClick()
	{
		ButtonClickSound.TryPlayClick();
		BuySmileBannerController.openedFromPromoActions = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00013960 File Offset: 0x00011B60
	public void BuyStickersPack(StickersPackItem curStickPack)
	{
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(curStickPack.KeyForBuy);
		int priceAmount = itemPrice.Price;
		string priceCurrency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, itemPrice, delegate
		{
			Storager.setInt(curStickPack.KeyForBuy, 1, true);
			try
			{
				string text = "Stickers";
				AnalyticsStuff.LogSales(curStickPack.KeyForBuy, text, false);
				AnalyticsFacade.InAppPurchase(curStickPack.KeyForBuy, text, 1, priceAmount, priceCurrency);
				if (BuySmileBannerController.openedFromPromoActions)
				{
					AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "Buy", "Stickers", curStickPack.KeyForBuy);
				}
				BuySmileBannerController.openedFromPromoActions = false;
			}
			catch
			{
			}
			if (PrivateChatController.sharedController != null && PrivateChatController.sharedController.gameObject.activeInHierarchy)
			{
				PrivateChatController.sharedController.isBuySmile = true;
				if (!PrivateChatController.sharedController.isShowSmilePanel)
				{
					PrivateChatController.sharedController.showSmileButton.SetActive(true);
				}
				PrivateChatController.sharedController.buySmileButton.SetActive(false);
				this.OnCloseClick();
			}
			if (ChatViewrController.sharedController != null && ChatViewrController.sharedController.gameObject.activeInHierarchy)
			{
				ChatViewrController.sharedController.buySmileButton.SetActive(false);
				if (!ChatViewrController.sharedController.isShowSmilePanel)
				{
					ChatViewrController.sharedController.showSmileButton.SetActive(true);
				}
				this.OnCloseClick();
			}
			curStickPack.OnBuy();
			ButtonBannerHUD.OnUpdateBanners();
		}, null, null, null, null, null);
	}

	// Token: 0x04000247 RID: 583
	public static bool openedFromPromoActions;

	// Token: 0x04000248 RID: 584
	private IDisposable _backSubscription;
}
