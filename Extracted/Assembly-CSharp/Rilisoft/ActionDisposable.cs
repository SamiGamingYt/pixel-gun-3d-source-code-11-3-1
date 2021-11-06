using System;

namespace Rilisoft
{
	// Token: 0x02000768 RID: 1896
	internal sealed class ActionDisposable : IDisposable
	{
		// Token: 0x06004299 RID: 17049 RVA: 0x00161D6C File Offset: 0x0015FF6C
		public ActionDisposable(Action action)
		{
			this._action = action;
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x00161D7C File Offset: 0x0015FF7C
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			if (this._action != null)
			{
				this._action();
			}
			this._disposed = true;
		}

		// Token: 0x040030B3 RID: 12467
		private readonly Action _action;

		// Token: 0x040030B4 RID: 12468
		private bool _disposed;
	}
}
