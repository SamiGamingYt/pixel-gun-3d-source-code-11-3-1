using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x02000730 RID: 1840
	public sealed class CurrencyChangedEventArgs : EventArgs
	{
		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x00156BE4 File Offset: 0x00154DE4
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x00156BEC File Offset: 0x00154DEC
		public string Currency { get; set; }

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x00156BF8 File Offset: 0x00154DF8
		// (set) Token: 0x06004048 RID: 16456 RVA: 0x00156C00 File Offset: 0x00154E00
		public int NewValue { get; set; }

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x00156C0C File Offset: 0x00154E0C
		// (set) Token: 0x0600404A RID: 16458 RVA: 0x00156C14 File Offset: 0x00154E14
		public int AddedValue { get; set; }

		// Token: 0x0600404B RID: 16459 RVA: 0x00156C20 File Offset: 0x00154E20
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"currency",
					this.Currency
				},
				{
					"newValue",
					this.NewValue
				},
				{
					"addedValue",
					this.AddedValue
				}
			};
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x00156C74 File Offset: 0x00154E74
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
