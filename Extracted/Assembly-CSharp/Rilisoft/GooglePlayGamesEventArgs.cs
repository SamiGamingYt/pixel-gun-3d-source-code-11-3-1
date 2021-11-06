using System;

namespace Rilisoft
{
	// Token: 0x02000713 RID: 1811
	internal sealed class GooglePlayGamesEventArgs : EventArgs
	{
		// Token: 0x06003F38 RID: 16184 RVA: 0x001527E4 File Offset: 0x001509E4
		public GooglePlayGamesEventArgs(bool succeeded, int slot, string data)
		{
			this._succeeded = succeeded;
			this._slot = slot;
			this._data = (data ?? string.Empty);
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003F39 RID: 16185 RVA: 0x00152810 File Offset: 0x00150A10
		public string Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x00152818 File Offset: 0x00150A18
		public int Slot
		{
			get
			{
				return this._slot;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003F3B RID: 16187 RVA: 0x00152820 File Offset: 0x00150A20
		public bool Succeeded
		{
			get
			{
				return this._succeeded;
			}
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x00152828 File Offset: 0x00150A28
		public override string ToString()
		{
			return (!this._succeeded) ? "<Failed>" : string.Format("Slot: {0}, Data: “{1}”", this._slot, this._data);
		}

		// Token: 0x04002E8C RID: 11916
		private string _data;

		// Token: 0x04002E8D RID: 11917
		private int _slot;

		// Token: 0x04002E8E RID: 11918
		private bool _succeeded;
	}
}
