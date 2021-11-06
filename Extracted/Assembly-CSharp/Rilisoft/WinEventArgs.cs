using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x0200072B RID: 1835
	public sealed class WinEventArgs : EventArgs
	{
		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x001568E0 File Offset: 0x00154AE0
		// (set) Token: 0x0600401D RID: 16413 RVA: 0x001568E8 File Offset: 0x00154AE8
		public ConnectSceneNGUIController.RegimGame Mode { get; set; }

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x001568F4 File Offset: 0x00154AF4
		// (set) Token: 0x0600401F RID: 16415 RVA: 0x001568FC File Offset: 0x00154AFC
		public string Map { get; set; }

		// Token: 0x06004020 RID: 16416 RVA: 0x00156908 File Offset: 0x00154B08
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"mode",
					this.Mode
				},
				{
					"map",
					this.Map
				}
			};
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x00156948 File Offset: 0x00154B48
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
