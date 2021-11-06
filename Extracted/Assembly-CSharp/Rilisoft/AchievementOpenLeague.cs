using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004FD RID: 1277
	public class AchievementOpenLeague : Achievement
	{
		// Token: 0x06002CDC RID: 11484 RVA: 0x000ECFD0 File Offset: 0x000EB1D0
		public AchievementOpenLeague(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base._data.League == null)
			{
				Debug.LogErrorFormat("achievement '{0}' without value", new object[]
				{
					base._data.Id
				});
				return;
			}
			AchievementsManager.Awaiter.Register(this.WaitRatingSystem());
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000ED034 File Offset: 0x000EB234
		private IEnumerator WaitRatingSystem()
		{
			while (RatingSystem.instance == null)
			{
				yield return null;
			}
			this.OnRatingUpdated();
			RatingSystem.OnRatingUpdate += this.OnRatingUpdated;
			yield break;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000ED050 File Offset: 0x000EB250
		private void OnRatingUpdated()
		{
			if (base._data.League.Value == RatingSystem.instance.currentLeague && base.Progress.Points < base.PointsLeft)
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000ED09C File Offset: 0x000EB29C
		public override void Dispose()
		{
			RatingSystem.OnRatingUpdate -= this.OnRatingUpdated;
			AchievementsManager.Awaiter.Remove(this.WaitRatingSystem());
		}
	}
}
