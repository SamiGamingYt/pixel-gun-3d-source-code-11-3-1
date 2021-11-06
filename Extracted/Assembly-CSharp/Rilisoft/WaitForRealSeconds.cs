using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000781 RID: 1921
	public class WaitForRealSeconds : CustomYieldInstruction
	{
		// Token: 0x06004396 RID: 17302 RVA: 0x00168F48 File Offset: 0x00167148
		public WaitForRealSeconds(float seconds)
		{
			this._prevRealTime = Time.realtimeSinceStartup;
			this._waitSeconds = Mathf.Abs(seconds);
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06004397 RID: 17303 RVA: 0x00168F68 File Offset: 0x00167168
		public override bool keepWaiting
		{
			get
			{
				this._elapsed += Time.realtimeSinceStartup - this._prevRealTime;
				this._prevRealTime = Time.realtimeSinceStartup;
				return this._elapsed < this._waitSeconds;
			}
		}

		// Token: 0x0400314C RID: 12620
		private float _waitSeconds;

		// Token: 0x0400314D RID: 12621
		private float _elapsed;

		// Token: 0x0400314E RID: 12622
		private float _prevRealTime;
	}
}
