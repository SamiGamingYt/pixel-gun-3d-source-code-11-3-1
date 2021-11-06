using System;

namespace RilisoftBot
{
	// Token: 0x02000592 RID: 1426
	public class FiringBossBot : FiringShotBot
	{
		// Token: 0x060031A1 RID: 12705 RVA: 0x00102500 File Offset: 0x00100700
		protected override void Initialize()
		{
			this.isMobChampion = true;
			base.Initialize();
		}
	}
}
