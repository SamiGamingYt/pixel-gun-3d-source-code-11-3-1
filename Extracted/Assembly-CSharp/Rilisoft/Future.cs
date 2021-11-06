using System;

namespace Rilisoft
{
	// Token: 0x0200012C RID: 300
	public class Future<T>
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000979 RID: 2425 RVA: 0x00039740 File Offset: 0x00037940
		// (remove) Token: 0x0600097A RID: 2426 RVA: 0x0003975C File Offset: 0x0003795C
		public event EventHandler Completed;

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00039778 File Offset: 0x00037978
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x00039780 File Offset: 0x00037980
		public T Result
		{
			get
			{
				if (!this._isCompleted)
				{
					throw new InvalidOperationException("Future is not completed.");
				}
				return this._result;
			}
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000397A0 File Offset: 0x000379A0
		protected void SetResult(T result)
		{
			this._result = result;
			this._isCompleted = true;
			EventHandler completed = this.Completed;
			if (completed != null)
			{
				completed(this, EventArgs.Empty);
			}
		}

		// Token: 0x040007C0 RID: 1984
		private bool _isCompleted;

		// Token: 0x040007C1 RID: 1985
		private T _result;
	}
}
