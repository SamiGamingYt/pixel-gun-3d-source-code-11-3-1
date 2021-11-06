using System;

// Token: 0x02000712 RID: 1810
public sealed class PurchaseEventArgs : EventArgs
{
	// Token: 0x06003F2B RID: 16171 RVA: 0x00152690 File Offset: 0x00150890
	public PurchaseEventArgs(int index, int count, decimal currencyAmount, string currency = "Coins", int discount = 0)
	{
		this.Index = index;
		this.Count = count;
		this.CurrencyAmount = currencyAmount;
		this.Currency = currency;
		this.Discount = discount;
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x001526C8 File Offset: 0x001508C8
	public PurchaseEventArgs(PurchaseEventArgs other)
	{
		if (other == null)
		{
			return;
		}
		this.Index = other.Index;
		this.Count = other.Count;
		this.CurrencyAmount = other.CurrencyAmount;
		this.Currency = other.Currency;
		this.Discount = other.Discount;
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x00152720 File Offset: 0x00150920
	public override string ToString()
	{
		return string.Format("{{ Index: {0}, Count: {1}, CurrencyAmount: {2}, Currency: {3}, Discount: {4} }}", new object[]
		{
			this.Index,
			this.Count,
			this.CurrencyAmount,
			this.Currency,
			this.Discount
		});
	}

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x06003F2E RID: 16174 RVA: 0x00152780 File Offset: 0x00150980
	// (set) Token: 0x06003F2F RID: 16175 RVA: 0x00152788 File Offset: 0x00150988
	public int Index { get; private set; }

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x06003F30 RID: 16176 RVA: 0x00152794 File Offset: 0x00150994
	// (set) Token: 0x06003F31 RID: 16177 RVA: 0x0015279C File Offset: 0x0015099C
	public int Count { get; set; }

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x06003F32 RID: 16178 RVA: 0x001527A8 File Offset: 0x001509A8
	// (set) Token: 0x06003F33 RID: 16179 RVA: 0x001527B0 File Offset: 0x001509B0
	public decimal CurrencyAmount { get; set; }

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x06003F34 RID: 16180 RVA: 0x001527BC File Offset: 0x001509BC
	// (set) Token: 0x06003F35 RID: 16181 RVA: 0x001527C4 File Offset: 0x001509C4
	public string Currency { get; private set; }

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x06003F36 RID: 16182 RVA: 0x001527D0 File Offset: 0x001509D0
	// (set) Token: 0x06003F37 RID: 16183 RVA: 0x001527D8 File Offset: 0x001509D8
	public int Discount { get; private set; }
}
