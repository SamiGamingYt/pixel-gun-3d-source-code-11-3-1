using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x0200072D RID: 1837
	public sealed class CaptureEventArgs : EventArgs
	{
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x00156AB4 File Offset: 0x00154CB4
		// (set) Token: 0x06004035 RID: 16437 RVA: 0x00156ABC File Offset: 0x00154CBC
		public ConnectSceneNGUIController.RegimGame Mode { get; set; }

		// Token: 0x06004036 RID: 16438 RVA: 0x00156AC8 File Offset: 0x00154CC8
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"mode",
					this.Mode
				}
			};
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x00156AF4 File Offset: 0x00154CF4
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
