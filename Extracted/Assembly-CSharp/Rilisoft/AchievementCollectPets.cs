using System;
using System.Collections;
using System.Linq;

namespace Rilisoft
{
	// Token: 0x0200050E RID: 1294
	public class AchievementCollectPets : Achievement
	{
		// Token: 0x06002D17 RID: 11543 RVA: 0x000EDD4C File Offset: 0x000EBF4C
		public AchievementCollectPets(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(this.WaitPetsManager());
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000EDD68 File Offset: 0x000EBF68
		private IEnumerator WaitPetsManager()
		{
			while (Singleton<PetsManager>.Instance == null)
			{
				yield return null;
			}
			this.UpdateProgress();
			Singleton<PetsManager>.Instance.OnPlayerPetAdded += this.PetsManager_Instance_OnPlayerPetAdded;
			yield break;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000EDD84 File Offset: 0x000EBF84
		private void PetsManager_Instance_OnPlayerPetAdded(string petId)
		{
			this.UpdateProgress();
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000EDD8C File Offset: 0x000EBF8C
		private void UpdateProgress()
		{
			if (Singleton<PetsManager>.Instance == null)
			{
				return;
			}
			int num = Singleton<PetsManager>.Instance.PlayerPets.Count<PlayerPet>();
			if (base.Points < num)
			{
				base.SetProgress(num);
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000EDDD0 File Offset: 0x000EBFD0
		public override void Dispose()
		{
			if (Singleton<PetsManager>.Instance != null)
			{
				Singleton<PetsManager>.Instance.OnPlayerPetAdded -= this.PetsManager_Instance_OnPlayerPetAdded;
			}
		}
	}
}
