using System;
using System.Diagnostics;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000860 RID: 2144
	internal sealed class TimeMeasurement
	{
		// Token: 0x06004D7D RID: 19837 RVA: 0x001C068C File Offset: 0x001BE88C
		public TimeMeasurement(string context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this._context = context;
			this._startFrame = Time.frameCount;
			this._startTime = Time.realtimeSinceStartup;
			this._timeNetto = new Stopwatch();
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004D7E RID: 19838 RVA: 0x001C06D8 File Offset: 0x001BE8D8
		public string Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004D7F RID: 19839 RVA: 0x001C06E0 File Offset: 0x001BE8E0
		public int FrameCount
		{
			get
			{
				return this._frameCount;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004D80 RID: 19840 RVA: 0x001C06E8 File Offset: 0x001BE8E8
		public float TimeBrutto
		{
			get
			{
				return this._timeBrutto;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004D81 RID: 19841 RVA: 0x001C06F0 File Offset: 0x001BE8F0
		public TimeSpan TimeNetto
		{
			get
			{
				return this._timeNetto.Elapsed;
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x001C0700 File Offset: 0x001BE900
		public void Start()
		{
			this._timeNetto.Start();
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x001C0710 File Offset: 0x001BE910
		public void Stop()
		{
			this._timeNetto.Stop();
			this._frameCount = Time.frameCount - this._startFrame;
			this._timeBrutto = Time.realtimeSinceStartup - this._startTime;
		}

		// Token: 0x04003BF6 RID: 15350
		private readonly string _context;

		// Token: 0x04003BF7 RID: 15351
		private readonly int _startFrame;

		// Token: 0x04003BF8 RID: 15352
		private readonly float _startTime;

		// Token: 0x04003BF9 RID: 15353
		private int _frameCount;

		// Token: 0x04003BFA RID: 15354
		private float _timeBrutto;

		// Token: 0x04003BFB RID: 15355
		private readonly Stopwatch _timeNetto;
	}
}
