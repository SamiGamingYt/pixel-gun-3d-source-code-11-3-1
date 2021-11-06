using System;
using Rilisoft;

// Token: 0x020007F7 RID: 2039
public sealed class ItemPrice
{
	// Token: 0x06004A1C RID: 18972 RVA: 0x0019B638 File Offset: 0x00199838
	public ItemPrice(int price, string currency)
	{
		this._price = new SaltedInt(ItemPrice._prng.Next(), price);
		this.Currency = currency;
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x0019B674 File Offset: 0x00199874
	public int CompareTo(ItemPrice p)
	{
		if (p == null)
		{
			return 1;
		}
		if (this.Currency.Equals(p.Currency))
		{
			return this.Price - p.Price;
		}
		float num;
		if (this.Currency.Equals("Coins"))
		{
			num = (float)this.Price - (float)p.Price * 1.7f;
		}
		else
		{
			num = (float)this.Price * 1.7f - (float)p.Price;
		}
		return (num <= 0f) ? ((num >= 0f) ? 0 : -1) : 1;
	}

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x06004A1F RID: 18975 RVA: 0x0019B71C File Offset: 0x0019991C
	public int Price
	{
		get
		{
			return this._price.Value;
		}
	}

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x06004A20 RID: 18976 RVA: 0x0019B738 File Offset: 0x00199938
	// (set) Token: 0x06004A21 RID: 18977 RVA: 0x0019B740 File Offset: 0x00199940
	public string Currency { get; private set; }

	// Token: 0x040036F1 RID: 14065
	private const float CoefGemsToCoins = 1.7f;

	// Token: 0x040036F2 RID: 14066
	private readonly SaltedInt _price;

	// Token: 0x040036F3 RID: 14067
	private static readonly Random _prng = new Random(268898311);
}
