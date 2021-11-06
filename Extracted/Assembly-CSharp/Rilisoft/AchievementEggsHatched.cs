using System;

namespace Rilisoft
{
	// Token: 0x02000513 RID: 1299
	public class AchievementEggsHatched : Achievement
	{
		// Token: 0x06002D2E RID: 11566 RVA: 0x000EE068 File Offset: 0x000EC268
		public AchievementEggsHatched(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			EggsManager.OnEggHatched += this.EggsManager_OnEggHatched;
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000EE084 File Offset: 0x000EC284
		private void EggsManager_OnEggHatched(Egg egg, PetInfo petInfo)
		{
			base.Gain(1);
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000EE090 File Offset: 0x000EC290
		public override void Dispose()
		{
			EggsManager.OnEggHatched -= this.EggsManager_OnEggHatched;
		}
	}
}
