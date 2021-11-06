using System;
using UnityEngine;

// Token: 0x02000765 RID: 1893
public class StarterPackView : BannerWindow
{
	// Token: 0x06004278 RID: 17016 RVA: 0x001610C0 File Offset: 0x0015F2C0
	private void SetupItemsData()
	{
		if (this.items.Length == 0)
		{
			return;
		}
		StarterPackData currentPackData = StarterPackController.Get.GetCurrentPackData();
		if (currentPackData == null)
		{
			return;
		}
		if (currentPackData.items == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < this.items.Length; i++)
		{
			if (i >= currentPackData.items.Count)
			{
				this.items[i].gameObject.SetActive(false);
				num++;
			}
			else
			{
				this.items[i].SetData(currentPackData.items[i]);
			}
		}
		this.CenterItems(num);
	}

	// Token: 0x06004279 RID: 17017 RVA: 0x00161164 File Offset: 0x0015F364
	private void CenterItems(int countHideElements)
	{
		if (this.items.Length < 2 || countHideElements == 0)
		{
			return;
		}
		float num = (float)countHideElements / 2f;
		float num2 = this.items[1].transform.localPosition.x - this.items[0].transform.localPosition.x;
		float num3 = num2 * num;
		int num4 = this.items.Length - countHideElements;
		for (int i = 0; i < num4; i++)
		{
			Vector3 localPosition = this.items[i].transform.localPosition;
			this.items[i].transform.localPosition = new Vector3(localPosition.x + num3, localPosition.y, localPosition.z);
		}
	}

	// Token: 0x0600427A RID: 17018 RVA: 0x00161230 File Offset: 0x0015F430
	private void Update()
	{
		string timeToEndEvent = StarterPackController.Get.GetTimeToEndEvent();
		for (int i = 0; i < this.timerEvent.Length; i++)
		{
			this.timerEvent[i].text = timeToEndEvent;
		}
	}

	// Token: 0x0600427B RID: 17019 RVA: 0x00161270 File Offset: 0x0015F470
	public void OnButtonBuyClick()
	{
		StarterPackController get = StarterPackController.Get;
		if (get == null)
		{
			return;
		}
		if (get.IsPackSellForGameMoney())
		{
			get.CheckBuyPackForGameMoney(this);
		}
		else
		{
			get.CheckBuyRealMoney();
			this.HideWindow();
		}
	}

	// Token: 0x0600427C RID: 17020 RVA: 0x001612B4 File Offset: 0x0015F4B4
	private void SetTitleText(string text)
	{
		for (int i = 0; i < this.title.Length; i++)
		{
			this.title[i].text = text;
		}
	}

	// Token: 0x0600427D RID: 17021 RVA: 0x001612E8 File Offset: 0x0015F4E8
	private void SetSaleSaveText(string text)
	{
		for (int i = 0; i < this.moneySaveSaleLabel.Length; i++)
		{
			this.moneySaveSaleLabel[i].text = text;
		}
	}

	// Token: 0x0600427E RID: 17022 RVA: 0x0016131C File Offset: 0x0015F51C
	private void SetButtonText()
	{
		if (StarterPackController.Get.IsPackSellForGameMoney())
		{
			ItemPrice priceDataForItemsPack = StarterPackController.Get.GetPriceDataForItemsPack();
			if (priceDataForItemsPack == null)
			{
				return;
			}
			this.buttonMoneyContainer.gameObject.SetActive(true);
			this.buttonLabel.gameObject.SetActive(false);
			this.buttonMoneyDescription.text = LocalizationStore.Get("Key_1043");
			this.buttonMoneyIcon.spriteName = ((!(priceDataForItemsPack.Currency == "GemsCurrency")) ? "coin_znachek" : "gem_znachek");
			this.buttonMoneyIcon.MakePixelPerfect();
			this.buttonMoneyCount.text = priceDataForItemsPack.Price.ToString();
		}
		else
		{
			this.buttonMoneyContainer.gameObject.SetActive(false);
			this.buttonLabel.gameObject.SetActive(true);
			string priceLabelForCurrentPack = StarterPackController.Get.GetPriceLabelForCurrentPack();
			this.buttonLabel.text = string.Format("{0} {1}", LocalizationStore.Get("Key_1043"), priceLabelForCurrentPack);
		}
	}

	// Token: 0x0600427F RID: 17023 RVA: 0x00161428 File Offset: 0x0015F628
	private void SetCountMoneyLabel(int count, bool isCoins)
	{
		string arg = string.Empty;
		if (isCoins)
		{
			arg = LocalizationStore.Get("Key_0936");
		}
		else
		{
			arg = LocalizationStore.Get("Key_0771");
		}
		for (int i = 0; i < this.moneyCountLabel.Length; i++)
		{
			this.moneyCountLabel[i].text = string.Format("{0}\n{1}", count, arg);
		}
	}

	// Token: 0x06004280 RID: 17024 RVA: 0x00161494 File Offset: 0x0015F694
	private void SetSaleLabel(int sale)
	{
		for (int i = 0; i < this.moneySaleLabel.Length; i++)
		{
			this.moneySaleLabel[i].text = string.Format("{0}% {1}", sale, LocalizationStore.Get("Key_0276"));
		}
	}

	// Token: 0x06004281 RID: 17025 RVA: 0x001614E4 File Offset: 0x0015F6E4
	private void SetMoneyData(bool isCoins)
	{
		StarterPackData currentPackData = StarterPackController.Get.GetCurrentPackData();
		string path = string.Empty;
		int count;
		if (isCoins)
		{
			path = "Textures/Bank/Coins_Shop_5";
			count = currentPackData.coinsCount;
		}
		else
		{
			path = "Textures/Bank/Coins_Shop_Gem_5";
			count = currentPackData.gemsCount;
		}
		Texture mainTexture = Resources.Load<Texture>(path);
		this.moneyLeftSprite.mainTexture = mainTexture;
		this.moneyRightSprite.mainTexture = mainTexture;
		this.SetCountMoneyLabel(count, isCoins);
		this.SetSaleLabel(currentPackData.sale);
	}

	// Token: 0x06004282 RID: 17026 RVA: 0x0016155C File Offset: 0x0015F75C
	private void ShowCustomInterface()
	{
		StarterPackModel.TypePack currentPackType = StarterPackController.Get.GetCurrentPackType();
		bool flag = currentPackType == StarterPackModel.TypePack.Items;
		bool moneyData = currentPackType == StarterPackModel.TypePack.Coins;
		this.itemsCentralPanel.gameObject.SetActive(flag);
		this.backgroundItems.gameObject.SetActive(flag);
		this.moneyCentralPanel.gameObject.SetActive(!flag);
		this.backgroundMoney.gameObject.SetActive(!flag);
		if (flag)
		{
			this.SetupItemsData();
		}
		else
		{
			this.SetMoneyData(moneyData);
		}
		this.SetSaleSaveText(StarterPackController.Get.GetSavingMoneyByCarrentPack());
		this.SetTitleText(StarterPackController.Get.GetCurrentPackName());
		this.SetButtonText();
	}

	// Token: 0x06004283 RID: 17027 RVA: 0x00161608 File Offset: 0x0015F808
	public override void Show()
	{
		base.Show();
		StarterPackController.Get.CheckFindStoreKitEventListner();
		StarterPackController.Get.UpdateCountShownWindowByShowCondition();
		this.ShowCustomInterface();
	}

	// Token: 0x06004284 RID: 17028 RVA: 0x00161638 File Offset: 0x0015F838
	public void HideWindow()
	{
		StarterPackController.Get.CheckSendEventChangeEnabled();
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController != null)
		{
			sharedController.HideBannerWindow();
			return;
		}
		base.Hide();
	}

	// Token: 0x04003099 RID: 12441
	public UILabel[] timerEvent;

	// Token: 0x0400309A RID: 12442
	public StarterPackItem[] items;

	// Token: 0x0400309B RID: 12443
	public UILabel buttonLabel;

	// Token: 0x0400309C RID: 12444
	public UILabel[] title;

	// Token: 0x0400309D RID: 12445
	public UIWidget backgroundItems;

	// Token: 0x0400309E RID: 12446
	public UIWidget backgroundMoney;

	// Token: 0x0400309F RID: 12447
	public UIWidget itemsCentralPanel;

	// Token: 0x040030A0 RID: 12448
	public UIWidget moneyCentralPanel;

	// Token: 0x040030A1 RID: 12449
	public UITexture moneyLeftSprite;

	// Token: 0x040030A2 RID: 12450
	public UITexture moneyRightSprite;

	// Token: 0x040030A3 RID: 12451
	public UILabel[] moneyCountLabel;

	// Token: 0x040030A4 RID: 12452
	public UILabel[] moneySaleLabel;

	// Token: 0x040030A5 RID: 12453
	public UILabel[] moneySaveSaleLabel;

	// Token: 0x040030A6 RID: 12454
	public UIWidget buttonMoneyContainer;

	// Token: 0x040030A7 RID: 12455
	public UILabel buttonMoneyDescription;

	// Token: 0x040030A8 RID: 12456
	public UISprite buttonMoneyIcon;

	// Token: 0x040030A9 RID: 12457
	public UILabel buttonMoneyCount;
}
