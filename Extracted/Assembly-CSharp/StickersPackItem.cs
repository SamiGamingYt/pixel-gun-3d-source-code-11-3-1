using System;
using UnityEngine;

// Token: 0x02000574 RID: 1396
public class StickersPackItem : MonoBehaviour
{
	// Token: 0x06003066 RID: 12390 RVA: 0x000FC6D0 File Offset: 0x000FA8D0
	private void Start()
	{
		if (this.priceLabel)
		{
			this.priceLabel.text = StickersController.GetPricePack(this.typePack).Price.ToString();
		}
		this.buyPackController = base.GetComponentInParent<BuySmileBannerController>();
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000FC71C File Offset: 0x000FA91C
	private void OnEnable()
	{
		this.CheckStateBtn();
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x06003068 RID: 12392 RVA: 0x000FC724 File Offset: 0x000FA924
	public string KeyForBuy
	{
		get
		{
			return StickersController.KeyForBuyPack(this.typePack);
		}
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x000FC734 File Offset: 0x000FA934
	public void CheckStateBtn()
	{
		bool flag = StickersController.IsBuyPack(this.typePack);
		if (flag)
		{
			if (this.btnForBuyPack)
			{
				this.btnForBuyPack.SetActive(false);
			}
			if (this.btnAvaliablePack)
			{
				this.btnAvaliablePack.SetActive(true);
			}
		}
		else
		{
			if (this.btnForBuyPack)
			{
				this.btnForBuyPack.SetActive(true);
			}
			if (this.btnAvaliablePack)
			{
				this.btnAvaliablePack.SetActive(false);
			}
		}
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000FC7C8 File Offset: 0x000FA9C8
	public void TryBuyPack()
	{
		if (this.buyPackController != null)
		{
			ButtonClickSound.Instance.PlayClick();
			this.buyPackController.BuyStickersPack(this);
		}
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000FC7F4 File Offset: 0x000FA9F4
	public void OnBuy()
	{
		this.CheckStateBtn();
		StickersController.EventPackBuy();
	}

	// Token: 0x04002392 RID: 9106
	public TypePackSticker typePack;

	// Token: 0x04002393 RID: 9107
	public UILabel priceLabel;

	// Token: 0x04002394 RID: 9108
	public GameObject btnForBuyPack;

	// Token: 0x04002395 RID: 9109
	public GameObject btnAvaliablePack;

	// Token: 0x04002396 RID: 9110
	private BuySmileBannerController buyPackController;
}
