using System;

namespace Rilisoft
{
	// Token: 0x020004F1 RID: 1265
	[Flags]
	internal enum AbuseMetod
	{
		// Token: 0x040021A1 RID: 8609
		None = 0,
		// Token: 0x040021A2 RID: 8610
		UpgradeFromVulnerableVersion = 1,
		// Token: 0x040021A3 RID: 8611
		Coins = 2,
		// Token: 0x040021A4 RID: 8612
		Gems = 4,
		// Token: 0x040021A5 RID: 8613
		Expendables = 8,
		// Token: 0x040021A6 RID: 8614
		Weapons = 16,
		// Token: 0x040021A7 RID: 8615
		AndroidPackageSignature = 32,
		// Token: 0x040021A8 RID: 8616
		health = 64
	}
}
