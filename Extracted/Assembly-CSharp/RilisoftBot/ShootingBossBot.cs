using System;

namespace RilisoftBot
{
	// Token: 0x02000599 RID: 1433
	public class ShootingBossBot : ShootingBot
	{
		// Token: 0x060031BB RID: 12731 RVA: 0x00102C5C File Offset: 0x00100E5C
		protected override void Initialize()
		{
			this.isMobChampion = true;
			base.Initialize();
		}
	}
}
