using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x02000731 RID: 1841
	public sealed class KillOtherPlayerOnFlyEventArgs : EventArgs
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x00156C8C File Offset: 0x00154E8C
		// (set) Token: 0x0600404F RID: 16463 RVA: 0x00156C94 File Offset: 0x00154E94
		public bool IamFly { get; set; }

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x00156CA0 File Offset: 0x00154EA0
		// (set) Token: 0x06004051 RID: 16465 RVA: 0x00156CA8 File Offset: 0x00154EA8
		public bool KilledPlayerFly { get; set; }

		// Token: 0x06004052 RID: 16466 RVA: 0x00156CB4 File Offset: 0x00154EB4
		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{
					"iamFly",
					this.IamFly
				},
				{
					"killedPlayerFly",
					this.KilledPlayerFly
				}
			};
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x00156CF8 File Offset: 0x00154EF8
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}
