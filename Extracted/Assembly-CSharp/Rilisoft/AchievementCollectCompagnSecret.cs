using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000500 RID: 1280
	internal sealed class AchievementCollectCompagnSecret : Achievement
	{
		// Token: 0x06002CE6 RID: 11494 RVA: 0x000ED2CC File Offset: 0x000EB4CC
		public AchievementCollectCompagnSecret(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
			int count = LevelBox.campaignBoxes.Count;
			for (int num = 0; num != count; num++)
			{
				LevelBox levelBox = LevelBox.campaignBoxes[num];
				int count2 = levelBox.levels.Count;
				for (int num2 = 0; num2 != count2; num2++)
				{
					CampaignLevel campaignLevel = levelBox.levels[num2];
					this._collectedList.Add(new AchievementCollectCompagnSecret.SecretGetttedInfo(campaignLevel.sceneName));
				}
			}
			this.Refresh();
			Storager.SubscribeToChanged(Defs.LevelsWhereGetCoinS, new Action(this.Update));
			Storager.SubscribeToChanged(Defs.LevelsWhereGotGems, new Action(this.Update));
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000ED39C File Offset: 0x000EB59C
		private void Refresh()
		{
			HashSet<string> hashSet = new HashSet<string>(CoinBonus.GetLevelsWhereGotCoins());
			HashSet<string> hashSet2 = new HashSet<string>(CoinBonus.GetLevelsWhereGotGems());
			if (Debug.isDebugBuild)
			{
			}
			foreach (AchievementCollectCompagnSecret.SecretGetttedInfo secretGetttedInfo in this._collectedList)
			{
				secretGetttedInfo.CoinsGetted = hashSet.Contains(secretGetttedInfo.Map);
				secretGetttedInfo.GemsGetted = hashSet2.Contains(secretGetttedInfo.Map);
			}
			if (Debug.isDebugBuild)
			{
			}
			if (this._collectedList.All((AchievementCollectCompagnSecret.SecretGetttedInfo si) => si.CoinsGetted && si.GemsGetted))
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000ED480 File Offset: 0x000EB680
		private void Update()
		{
			string[] levelsWhereGotCoins = CoinBonus.GetLevelsWhereGotCoins();
			IEnumerable<string> levelsWhereGotGems = CoinBonus.GetLevelsWhereGotGems();
			foreach (AchievementCollectCompagnSecret.SecretGetttedInfo secretGetttedInfo in this._collectedList)
			{
				secretGetttedInfo.CoinsGetted = levelsWhereGotCoins.Contains(secretGetttedInfo.Map);
				secretGetttedInfo.GemsGetted = levelsWhereGotGems.Contains(secretGetttedInfo.Map);
			}
			if (this._collectedList.All((AchievementCollectCompagnSecret.SecretGetttedInfo si) => si.CoinsGetted && si.GemsGetted))
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000ED544 File Offset: 0x000EB744
		public override void Dispose()
		{
			Storager.UnSubscribeToChanged(Defs.LevelsWhereGotGems, new Action(this.Update));
			Storager.UnSubscribeToChanged(Defs.LevelsWhereGetCoinS, new Action(this.Update));
		}

		// Token: 0x040021AF RID: 8623
		private readonly List<AchievementCollectCompagnSecret.SecretGetttedInfo> _collectedList = new List<AchievementCollectCompagnSecret.SecretGetttedInfo>();

		// Token: 0x02000501 RID: 1281
		internal sealed class SecretGetttedInfo
		{
			// Token: 0x06002CEC RID: 11500 RVA: 0x000ED5B0 File Offset: 0x000EB7B0
			public SecretGetttedInfo(string map)
			{
				this.Map = map;
			}

			// Token: 0x06002CED RID: 11501 RVA: 0x000ED5C0 File Offset: 0x000EB7C0
			public override string ToString()
			{
				return string.Format("'{0}'=> c:{1} g:{2}", this.Map, this.CoinsGetted, this.GemsGetted);
			}

			// Token: 0x040021B2 RID: 8626
			public string Map;

			// Token: 0x040021B3 RID: 8627
			public bool CoinsGetted;

			// Token: 0x040021B4 RID: 8628
			public bool GemsGetted;
		}
	}
}
