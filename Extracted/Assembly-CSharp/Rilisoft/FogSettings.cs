using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000560 RID: 1376
	[Serializable]
	public class FogSettings
	{
		// Token: 0x06002FC8 RID: 12232 RVA: 0x000F9B2C File Offset: 0x000F7D2C
		public FogSettings FromCurrent()
		{
			this.Active = RenderSettings.fog;
			this.Mode = RenderSettings.fogMode;
			this.Color = RenderSettings.fogColor;
			this.Start = RenderSettings.fogStartDistance;
			this.End = RenderSettings.fogEndDistance;
			return this;
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000F9B74 File Offset: 0x000F7D74
		public void SetToCurrent()
		{
			RenderSettings.fog = this.Active;
			RenderSettings.fogMode = this.Mode;
			RenderSettings.fogColor = this.Color;
			RenderSettings.fogStartDistance = this.Start;
			RenderSettings.fogEndDistance = this.End;
		}

		// Token: 0x0400231E RID: 8990
		public bool Active;

		// Token: 0x0400231F RID: 8991
		public FogMode Mode;

		// Token: 0x04002320 RID: 8992
		public Color Color;

		// Token: 0x04002321 RID: 8993
		public float Start;

		// Token: 0x04002322 RID: 8994
		public float End;
	}
}
