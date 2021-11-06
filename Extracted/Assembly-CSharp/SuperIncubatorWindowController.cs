using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x02000825 RID: 2085
public class SuperIncubatorWindowController : GeneralBannerWindow
{
	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x06004BC1 RID: 19393 RVA: 0x001B44FC File Offset: 0x001B26FC
	// (set) Token: 0x06004BC2 RID: 19394 RVA: 0x001B4504 File Offset: 0x001B2704
	public Action BuyAction { get; set; }

	// Token: 0x06004BC3 RID: 19395 RVA: 0x001B4510 File Offset: 0x001B2710
	public virtual void HandleBuyButton()
	{
		Action buyAction = this.BuyAction;
		if (buyAction != null)
		{
			buyAction();
		}
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x001B4530 File Offset: 0x001B2730
	private void UpdateGui()
	{
		ItemPrice price = ShopNGUIController.GetItemPrice("Eggs.SuperIncubatorId", ShopNGUIController.CategoryNames.EggsCategory, false, true, false);
		this.priceLabels.ForEach(delegate(UILabel label)
		{
			label.text = price.Price.ToString();
		});
		bool flag;
		int discount = ShopNGUIController.DiscountFor("Eggs.SuperIncubatorId", out flag);
		this.offerPriceBackground.SetActiveSafeSelf(discount > 0);
		this.saleContainer.SetActiveSafeSelf(discount > 0);
		this.saleLabels.ForEach(delegate(UILabel label)
		{
			label.text = string.Format(LocalizationStore.Get("Key_2555"), discount.ToString());
		});
		this.m_lastTimeUpdated = Time.realtimeSinceStartup;
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001B45D0 File Offset: 0x001B27D0
	private void Awake()
	{
		this.UpdateGui();
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001B45D8 File Offset: 0x001B27D8
	private void Start()
	{
		this.RegisterEscapeHandler();
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x001B45E0 File Offset: 0x001B27E0
	private void Update()
	{
		if (Time.realtimeSinceStartup - 0.5f >= this.m_lastTimeUpdated)
		{
			this.UpdateGui();
		}
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001B4600 File Offset: 0x001B2800
	private void OnDestroy()
	{
		this.UnregisterEscapeHandler();
	}

	// Token: 0x04003AEA RID: 15082
	public GameObject offerPriceBackground;

	// Token: 0x04003AEB RID: 15083
	public GameObject saleContainer;

	// Token: 0x04003AEC RID: 15084
	public List<UILabel> saleLabels;

	// Token: 0x04003AED RID: 15085
	public List<UILabel> priceLabels;

	// Token: 0x04003AEE RID: 15086
	private float m_lastTimeUpdated = float.MinValue;
}
