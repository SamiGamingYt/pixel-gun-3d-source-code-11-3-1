using System;
using System.Collections;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x0200051F RID: 1311
	public sealed class Awaiter
	{
		// Token: 0x06002DB1 RID: 11697 RVA: 0x000F0558 File Offset: 0x000EE758
		public void Register(IEnumerator iter)
		{
			if (iter != null && !this._iters.Contains(iter))
			{
				this._iters.Add(iter);
			}
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000F0580 File Offset: 0x000EE780
		public void Remove(IEnumerator iter)
		{
			if (iter != null && this._iters.Contains(iter))
			{
				this._iters.Remove(iter);
			}
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000F05B4 File Offset: 0x000EE7B4
		public void Tick()
		{
			int count = this._iters.Count;
			for (int i = 0; i < count; i++)
			{
				IEnumerator enumerator = this._iters[i];
				if (!enumerator.MoveNext())
				{
					this._itersToRemove.Add(enumerator);
				}
			}
			int count2 = this._itersToRemove.Count;
			if (count2 > 0)
			{
				for (int j = 0; j < count2; j++)
				{
					IEnumerator item = this._itersToRemove[j];
					this._iters.Remove(item);
				}
				this._itersToRemove.Clear();
			}
		}

		// Token: 0x04002226 RID: 8742
		private readonly List<IEnumerator> _iters = new List<IEnumerator>();

		// Token: 0x04002227 RID: 8743
		private readonly List<IEnumerator> _itersToRemove = new List<IEnumerator>();
	}
}
