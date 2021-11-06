using System;
using System.Threading.Tasks;

namespace Rilisoft
{
	// Token: 0x02000716 RID: 1814
	public class PurchasesSavingEventArgs : EventArgs
	{
		// Token: 0x06003F41 RID: 16193 RVA: 0x00152988 File Offset: 0x00150B88
		public PurchasesSavingEventArgs(Task<bool> future)
		{
			this.Future = future;
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x00152998 File Offset: 0x00150B98
		// (set) Token: 0x06003F43 RID: 16195 RVA: 0x001529A0 File Offset: 0x00150BA0
		public Task<bool> Future { get; private set; }
	}
}
