using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000120 RID: 288
internal sealed class FrameStopwatchScript : MonoBehaviour
{
	// Token: 0x06000849 RID: 2121 RVA: 0x00032630 File Offset: 0x00030830
	public float GetSecondsSinceFrameStarted()
	{
		return (float)this._stopwatch.ElapsedMilliseconds / 1000f;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00032644 File Offset: 0x00030844
	internal void Start()
	{
		this._stopwatch.Start();
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00032654 File Offset: 0x00030854
	internal void Update()
	{
		this._stopwatch.Reset();
		this._stopwatch.Start();
	}

	// Token: 0x040006EC RID: 1772
	private readonly Stopwatch _stopwatch = new Stopwatch();
}
