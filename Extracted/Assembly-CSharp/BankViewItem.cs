using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x0200065F RID: 1631
public class BankViewItem : AbstractBankViewItem
{
	// Token: 0x060038E1 RID: 14561 RVA: 0x00126808 File Offset: 0x00124A08
	public override void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, EventHandler clickHandler)
	{
		base.Setup(product, newPurchaseInfo, clickHandler);
		this.Count = this.purchaseInfo.Count;
		this.CountX3 = 3 * this.purchaseInfo.Count;
		if (this.bonusButtonView != null)
		{
			this.bonusButtonView.UpdateState(this.purchaseInfo);
		}
	}

	// Token: 0x1700095A RID: 2394
	// (set) Token: 0x060038E2 RID: 14562 RVA: 0x00126864 File Offset: 0x00124A64
	public override bool isX3Item
	{
		set
		{
			base.isX3Item = value;
			if (this.discountSprite != null)
			{
				this.discountSprite.gameObject.SetActive(!value && base.IsDiscounted());
			}
			if (!value && this.purchaseInfo != null && this.discountPercentsLabel != null && base.IsDiscounted())
			{
				this.discountPercentsLabel.text = string.Format("{0}%", this.purchaseInfo.Discount);
			}
			this.UpdateViewBestBuy();
			this.normalAmountContainer.SetActiveSafeSelf(!value);
			this.x3AmountContainer.SetActiveSafeSelf(value);
		}
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x0012691C File Offset: 0x00124B1C
	protected override void Awake()
	{
		this._bestBuyAnimator = ((!(this.bestBuy == null)) ? this.bestBuy.GetComponent<Animator>() : null);
		this._discountAnimator = ((!(this.discountSprite == null)) ? this.discountSprite.GetComponent<Animator>() : null);
		if (this.bonusButtonView != null)
		{
			this.bonusButtonView.Initialize();
			if (this.purchaseInfo != null)
			{
				this.bonusButtonView.UpdateState(this.purchaseInfo);
			}
		}
		PromoActionsManager.BestBuyStateUpdate += this.UpdateViewBestBuy;
		base.Awake();
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x001269C8 File Offset: 0x00124BC8
	protected override void OnEnable()
	{
		this.UpdateViewBestBuy();
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x001269D0 File Offset: 0x00124BD0
	protected override void OnDisable()
	{
		if (Device.IsLoweMemoryDevice)
		{
		}
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x001269DC File Offset: 0x00124BDC
	protected override void OnDestroy()
	{
		if (this.bonusButtonView != null)
		{
			this.bonusButtonView.Deinitialize();
		}
		PromoActionsManager.BestBuyStateUpdate -= this.UpdateViewBestBuy;
		base.OnDestroy();
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x00126A14 File Offset: 0x00124C14
	protected virtual string PathToIcon()
	{
		return (!(this.purchaseInfo.Currency == "GemsCurrency")) ? ("Textures/Bank/Coins_Shop_" + (this.purchaseInfo.Index + 1)) : ("Textures/Bank/Coins_Shop_Gem_" + (this.purchaseInfo.Index + 1));
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x00126A78 File Offset: 0x00124C78
	protected override void SetIcon()
	{
		if (this.purchaseInfo == null)
		{
			Debug.LogErrorFormat("BankViewItem.SetIcon: purchaseInfo == null", new object[0]);
			return;
		}
		string path = this.PathToIcon();
		this.icon.mainTexture = Resources.Load<Texture>(path);
	}

	// Token: 0x1700095B RID: 2395
	// (set) Token: 0x060038E9 RID: 14569 RVA: 0x00126ABC File Offset: 0x00124CBC
	private int Count
	{
		set
		{
			if (this.countLabel != null)
			{
				for (int i = 0; i < this.countLabel.Count; i++)
				{
					this.countLabel[i].text = value.ToString();
				}
			}
		}
	}

	// Token: 0x1700095C RID: 2396
	// (set) Token: 0x060038EA RID: 14570 RVA: 0x00126B08 File Offset: 0x00124D08
	private int CountX3
	{
		set
		{
			if (this.countX3Label != null)
			{
				for (int i = 0; i < this.countX3Label.Count; i++)
				{
					this.countX3Label[i].text = value.ToString();
				}
			}
		}
	}

	// Token: 0x060038EB RID: 14571 RVA: 0x00126B54 File Offset: 0x00124D54
	private void UpdateAnimationEventSprite(bool isEventActive)
	{
		PromoActionsManager sharedManager = PromoActionsManager.sharedManager;
		if (sharedManager != null && sharedManager.IsEventX3Active)
		{
			return;
		}
		bool flag = base.IsDiscounted();
		if (flag && this._discountAnimator != null)
		{
			if (isEventActive)
			{
				this._discountAnimator.Play("DiscountAnimation");
			}
			else
			{
				this._discountAnimator.Play("Idle");
			}
		}
		if (isEventActive && this._bestBuyAnimator != null)
		{
			if (flag)
			{
				this._bestBuyAnimator.Play("BestBuyAnimation");
			}
			else
			{
				this._bestBuyAnimator.Play("Idle");
			}
		}
	}

	// Token: 0x060038EC RID: 14572 RVA: 0x00126C0C File Offset: 0x00124E0C
	private void UpdateViewBestBuy()
	{
		if (this.purchaseInfo == null)
		{
			Debug.LogWarningFormat("UpdateViewBestBuy: purchaseInfo == null", new object[0]);
		}
		PromoActionsManager sharedManager = PromoActionsManager.sharedManager;
		bool flag = this.purchaseInfo != null && sharedManager != null && sharedManager.IsBankItemBestBuy(this.purchaseInfo);
		this.bestBuy.gameObject.SetActive(flag);
		this.UpdateAnimationEventSprite(flag);
	}

	// Token: 0x040029A0 RID: 10656
	public GameObject normalAmountContainer;

	// Token: 0x040029A1 RID: 10657
	public GameObject x3AmountContainer;

	// Token: 0x040029A2 RID: 10658
	public List<UILabel> countLabelsList;

	// Token: 0x040029A3 RID: 10659
	public List<UILabel> countLabelsX3List;

	// Token: 0x040029A4 RID: 10660
	public List<UILabel> countLabel;

	// Token: 0x040029A5 RID: 10661
	public List<UILabel> countX3Label;

	// Token: 0x040029A6 RID: 10662
	public UISprite discountSprite;

	// Token: 0x040029A7 RID: 10663
	public UILabel discountPercentsLabel;

	// Token: 0x040029A8 RID: 10664
	public UISprite bestBuy;

	// Token: 0x040029A9 RID: 10665
	public ChestBonusButtonView bonusButtonView;

	// Token: 0x040029AA RID: 10666
	private Animator _bestBuyAnimator;

	// Token: 0x040029AB RID: 10667
	private Animator _discountAnimator;
}
