using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x0200072F RID: 1839
	public sealed class SocialInteractionEventArgs : EventArgs
	{
		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x00156B90 File Offset: 0x00154D90
		// (set) Token: 0x06004041 RID: 16449 RVA: 0x00156B98 File Offset: 0x00154D98
		public string Kind { get; set; }

		// Token: 0x06004042 RID: 16450 RVA: 0x00156BA4 File Offset: 0x00154DA4
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"kind",
					this.Kind
				}
			};
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x00156BCC File Offset: 0x00154DCC
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
