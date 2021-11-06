using System;

namespace Rilisoft
{
	// Token: 0x0200012D RID: 301
	public class Promise<T>
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x000397E8 File Offset: 0x000379E8
		public Future<T> Future
		{
			get
			{
				return this._future;
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x000397F0 File Offset: 0x000379F0
		public void SetResult(T result)
		{
			this._future.SetResult(result);
		}

		// Token: 0x040007C3 RID: 1987
		private readonly Promise<T>.FutureImpl<T> _future = new Promise<T>.FutureImpl<T>();

		// Token: 0x0200012E RID: 302
		private class FutureImpl<U> : Future<U>
		{
			// Token: 0x06000982 RID: 2434 RVA: 0x00039808 File Offset: 0x00037A08
			internal new void SetResult(U result)
			{
				base.SetResult(result);
			}
		}
	}
}
