using System;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000589 RID: 1417
	public class BotAnimationEventHandler : MonoBehaviour
	{
		// Token: 0x14000045 RID: 69
		// (add) Token: 0x0600316A RID: 12650 RVA: 0x00101B08 File Offset: 0x000FFD08
		// (remove) Token: 0x0600316B RID: 12651 RVA: 0x00101B24 File Offset: 0x000FFD24
		public event BotAnimationEventHandler.OnDamageEventDelegate OnDamageEvent;

		// Token: 0x0600316C RID: 12652 RVA: 0x00101B40 File Offset: 0x000FFD40
		private void OnApplyShootEffect()
		{
			if (this.OnDamageEvent == null)
			{
				return;
			}
			this.OnDamageEvent();
		}

		// Token: 0x0200091D RID: 2333
		// (Invoke) Token: 0x06005114 RID: 20756
		public delegate void OnDamageEventDelegate();
	}
}
