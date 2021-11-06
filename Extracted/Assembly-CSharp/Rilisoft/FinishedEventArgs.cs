using System;

namespace Rilisoft
{
	// Token: 0x0200053C RID: 1340
	internal class FinishedEventArgs : EventArgs
	{
		// Token: 0x06002EA9 RID: 11945 RVA: 0x000F4028 File Offset: 0x000F2228
		public FinishedEventArgs(bool succeeded)
		{
			this._succeeded = succeeded;
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002EAB RID: 11947 RVA: 0x000F4050 File Offset: 0x000F2250
		public bool Succeeded
		{
			get
			{
				return this._succeeded;
			}
		}

		// Token: 0x0400228A RID: 8842
		public static readonly FinishedEventArgs Success = new FinishedEventArgs(true);

		// Token: 0x0400228B RID: 8843
		public static readonly FinishedEventArgs Failure = new FinishedEventArgs(false);

		// Token: 0x0400228C RID: 8844
		private readonly bool _succeeded;
	}
}
