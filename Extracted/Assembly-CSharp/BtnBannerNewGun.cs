using System;
using UnityEngine;

// Token: 0x0200059D RID: 1437
public class BtnBannerNewGun : ButtonBannerBase
{
	// Token: 0x060031D8 RID: 12760 RVA: 0x00103510 File Offset: 0x00101710
	public override bool BannerIsActive()
	{
		return PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.news.Count > 0;
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x00103548 File Offset: 0x00101748
	public override void OnClickButton()
	{
		if (!string.IsNullOrEmpty(this.tagForClick))
		{
			MainMenuController.sharedController.HandlePromoActionClicked(this.tagForClick);
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x00103578 File Offset: 0x00101778
	public override void OnHide()
	{
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x0010357C File Offset: 0x0010177C
	public override void OnShow()
	{
		if (string.IsNullOrEmpty(this.tagForClick))
		{
			this.OnUpdateParameter();
		}
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x00103594 File Offset: 0x00101794
	public override void OnUpdateParameter()
	{
		if (PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.news.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, PromoActionsManager.sharedManager.news.Count);
			this.tagForClick = PromoActionsManager.sharedManager.news[index];
		}
		if (!string.IsNullOrEmpty(this.tagForClick))
		{
			string empty = string.Empty;
			int itemCategory = ItemDb.GetItemCategory(this.tagForClick);
			Texture mainTexture = Resources.Load<Texture>(empty);
			this.txGun.mainTexture = mainTexture;
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(this.tagForClick, (ShopNGUIController.CategoryNames)(-1));
			this.lbSale.text = string.Format("{0}\n{1}%", LocalizationStore.Key_0419, PromoActionsManager.sharedManager.discounts[this.tagForClick][0].Value);
			this.lbPrice.text = PromoActionsManager.sharedManager.discounts[this.tagForClick][1].Value.ToString();
			this.lbPrice.color = ((!priceByShopId.Currency.Equals("Coins")) ? new Color(0.3176f, 0.8117f, 1f) : new Color(1f, 0.8627f, 0f));
			this.sprCoinImg.spriteName = ((!priceByShopId.Currency.Equals("Coins")) ? "gem_znachek" : "ingame_coin");
		}
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x00103730 File Offset: 0x00101930
	public override void OnChangeLocalize()
	{
	}

	// Token: 0x040024C6 RID: 9414
	public string tagForClick = string.Empty;

	// Token: 0x040024C7 RID: 9415
	public UILabel lbSale;

	// Token: 0x040024C8 RID: 9416
	public UILabel lbPrice;

	// Token: 0x040024C9 RID: 9417
	public UITexture txGun;

	// Token: 0x040024CA RID: 9418
	public UISprite sprCoinImg;
}
