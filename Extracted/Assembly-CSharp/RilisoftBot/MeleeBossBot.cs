using System;

namespace RilisoftBot
{
	// Token: 0x02000594 RID: 1428
	public class MeleeBossBot : MeleeBot
	{
		// Token: 0x060031AA RID: 12714 RVA: 0x001026EC File Offset: 0x001008EC
		protected override void Initialize()
		{
			this.isMobChampion = true;
			base.Initialize();
		}
	}
}
