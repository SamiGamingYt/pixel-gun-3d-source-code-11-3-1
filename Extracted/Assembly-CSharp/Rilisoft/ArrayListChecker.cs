using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000564 RID: 1380
	internal sealed class ArrayListChecker : IDisposable
	{
		// Token: 0x06002FD5 RID: 12245 RVA: 0x000F9D80 File Offset: 0x000F7F80
		public ArrayListChecker(ArrayList arrayList, string label)
		{
			this._arrayList = arrayList;
			this._label = (label ?? string.Empty);
			this.CheckOverflowIfDebug();
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000F9DB4 File Offset: 0x000F7FB4
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this.CheckOverflowIfDebug();
			this._disposed = true;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000F9DD0 File Offset: 0x000F7FD0
		private void CheckOverflowIfDebug()
		{
			if (Debug.isDebugBuild)
			{
				if (this._arrayList == null)
				{
					Debug.LogWarning(this._label + ": ArrayList is null.");
				}
				else if (this._arrayList.Count > 50 || this._arrayList.Capacity > 1000)
				{
					this.HandleOverflow();
				}
			}
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000F9E3C File Offset: 0x000F803C
		private void HandleOverflow()
		{
			string str = string.Format("{0}: Count: {1}, Capacity: {2}", this._label, this._arrayList.Count, this._arrayList.Capacity);
			string message = str + Environment.NewLine + Environment.NewLine + Environment.StackTrace;
			Debug.LogWarning(message);
		}

		// Token: 0x0400232C RID: 9004
		private const int CapacityThreshold = 1000;

		// Token: 0x0400232D RID: 9005
		private const int CountThreshold = 50;

		// Token: 0x0400232E RID: 9006
		private ArrayList _arrayList;

		// Token: 0x0400232F RID: 9007
		private bool _disposed;

		// Token: 0x04002330 RID: 9008
		private string _label;
	}
}
