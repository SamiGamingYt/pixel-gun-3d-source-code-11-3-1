using System;
using System.Collections;

namespace Rilisoft
{
	// Token: 0x02000511 RID: 1297
	public abstract class AchievementBindedToMyPlayer : Achievement
	{
		// Token: 0x06002D24 RID: 11556 RVA: 0x000EDF30 File Offset: 0x000EC130
		public AchievementBindedToMyPlayer(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			AchievementsManager.Awaiter.Register(this.Tick());
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x000EDF4C File Offset: 0x000EC14C
		protected Player_move_c MyPlayer
		{
			get
			{
				return (!(WeaponManager.sharedManager != null)) ? null : WeaponManager.sharedManager.myPlayerMoveC;
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000EDF7C File Offset: 0x000EC17C
		private IEnumerator Tick()
		{
			for (;;)
			{
				if (this.MyPlayer == null)
				{
					this._lastHash = 0;
				}
				if (this._lastHash == 0 && this.MyPlayer != null)
				{
					this._lastHash = this.MyPlayer.GetHashCode();
					this.OnPlayerInstanceSetted();
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002D27 RID: 11559
		protected abstract void OnPlayerInstanceSetted();

		// Token: 0x06002D28 RID: 11560 RVA: 0x000EDF98 File Offset: 0x000EC198
		public override void Dispose()
		{
			AchievementsManager.Awaiter.Remove(this.Tick());
		}

		// Token: 0x040021BB RID: 8635
		private int _lastHash;
	}
}
