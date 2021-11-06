using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x02000732 RID: 1842
	public sealed class SurviveWaveInArenaEventArgs : EventArgs
	{
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x00156D10 File Offset: 0x00154F10
		// (set) Token: 0x06004056 RID: 16470 RVA: 0x00156D18 File Offset: 0x00154F18
		public int WaveNumber { get; set; }

		// Token: 0x06004057 RID: 16471 RVA: 0x00156D24 File Offset: 0x00154F24
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"waveNumber",
					this.WaveNumber
				}
			};
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00156D50 File Offset: 0x00154F50
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
