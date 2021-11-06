using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005D9 RID: 1497
	[Serializable]
	internal sealed class CurrencySpecificWatchPanel : MonoBehaviour
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x0010A620 File Offset: 0x00108820
		public UILabel WatchHeader
		{
			get
			{
				return this.watchHeader;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x0010A628 File Offset: 0x00108828
		public UILabel WatchTimer
		{
			get
			{
				return this.watchTimer;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x0010A630 File Offset: 0x00108830
		public UIButton WatchButton
		{
			get
			{
				return this.watchButton;
			}
		}

		// Token: 0x040025C0 RID: 9664
		[SerializeField]
		private UILabel watchHeader;

		// Token: 0x040025C1 RID: 9665
		[SerializeField]
		private UILabel watchTimer;

		// Token: 0x040025C2 RID: 9666
		[SerializeField]
		private UIButton watchButton;
	}
}
