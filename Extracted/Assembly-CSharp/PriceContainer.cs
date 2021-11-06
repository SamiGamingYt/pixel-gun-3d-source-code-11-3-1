using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class PriceContainer : MonoBehaviour
{
	// Token: 0x06002A18 RID: 10776 RVA: 0x000DDC58 File Offset: 0x000DBE58
	public void SetPrice(ItemPrice price)
	{
		this.priceLabel.text = price.Price.ToString();
		bool flag = price.Currency.Equals("Coins");
		this.gem.SetActive(!flag);
		this.coin.SetActive(flag);
	}

	// Token: 0x04001F14 RID: 7956
	public UITexture background;

	// Token: 0x04001F15 RID: 7957
	public UILabel priceLabel;

	// Token: 0x04001F16 RID: 7958
	public GameObject gem;

	// Token: 0x04001F17 RID: 7959
	public GameObject coin;
}
