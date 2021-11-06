using System;
using System.Diagnostics;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000767 RID: 1895
	internal sealed class StopwatchLogger : IDisposable
	{
		// Token: 0x06004296 RID: 17046 RVA: 0x00161CC8 File Offset: 0x0015FEC8
		public StopwatchLogger(string text, bool verbose)
		{
			this._verbose = verbose;
			this._text = (text ?? string.Empty);
			if (this._verbose)
			{
				UnityEngine.Debug.Log(string.Format("{0}: started.", this._text));
			}
			this._stopwatch = Stopwatch.StartNew();
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00161D20 File Offset: 0x0015FF20
		public StopwatchLogger(string text) : this(text, true)
		{
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00161D2C File Offset: 0x0015FF2C
		public void Dispose()
		{
			this._stopwatch.Stop();
			if (this._verbose)
			{
				UnityEngine.Debug.Log(string.Format("{0}: finished at {1:0.00}", this._text, this._stopwatch.ElapsedMilliseconds));
			}
		}

		// Token: 0x040030B0 RID: 12464
		private readonly Stopwatch _stopwatch;

		// Token: 0x040030B1 RID: 12465
		private readonly string _text;

		// Token: 0x040030B2 RID: 12466
		private readonly bool _verbose;
	}
}
