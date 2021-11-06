using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000509 RID: 1289
	public class AchievementJetPackFlying : Achievement
	{
		// Token: 0x06002D04 RID: 11524 RVA: 0x000ED970 File Offset: 0x000EBB70
		public AchievementJetPackFlying(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(this.Check());
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000ED98C File Offset: 0x000EBB8C
		private IEnumerator Check()
		{
			float _timeElapsed = 0f;
			for (;;)
			{
				if (Defs.isJetpackEnabled && Defs.isJump)
				{
					_timeElapsed += Time.deltaTime;
				}
				else if (_timeElapsed > 0f)
				{
					float currentProgress = 0f;
					object rawProgress = base.Progress.CustomDataGet("ftime");
					if (rawProgress != null)
					{
						float.TryParse((string)rawProgress, out currentProgress);
					}
					currentProgress += _timeElapsed;
					base.Progress.CustomDataSet("ftime", currentProgress.ToString());
					int flyingMinutes = (int)(currentProgress / 60f);
					if (flyingMinutes != base.Progress.Points)
					{
						base.SetProgress(flyingMinutes);
					}
					_timeElapsed = 0f;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000ED9A8 File Offset: 0x000EBBA8
		public override void Dispose()
		{
			AchievementsManager.Awaiter.Remove(this.Check());
		}

		// Token: 0x040021B6 RID: 8630
		private const string DATA_KEY = "ftime";
	}
}
