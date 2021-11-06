using System;

namespace Rilisoft
{
	// Token: 0x02000512 RID: 1298
	public class AchievementResurection : AchievementBindedToMyPlayer
	{
		// Token: 0x06002D29 RID: 11561 RVA: 0x000EDFAC File Offset: 0x000EC1AC
		public AchievementResurection(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000EDFB8 File Offset: 0x000EC1B8
		private new Player_move_c MyPlayer
		{
			get
			{
				return (!(WeaponManager.sharedManager != null)) ? null : WeaponManager.sharedManager.myPlayerMoveC;
			}
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000EDFE8 File Offset: 0x000EC1E8
		protected override void OnPlayerInstanceSetted()
		{
			if (this.MyPlayer != null)
			{
				this.MyPlayer.OnMyPlayerResurected += this.Player_move_c_OnMyPlayerResurected;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000EE020 File Offset: 0x000EC220
		private void Player_move_c_OnMyPlayerResurected()
		{
			base.Gain(1);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000EE02C File Offset: 0x000EC22C
		public override void Dispose()
		{
			base.Dispose();
			if (this.MyPlayer != null)
			{
				this.MyPlayer.OnMyPlayerResurected -= this.Player_move_c_OnMyPlayerResurected;
			}
		}

		// Token: 0x040021BC RID: 8636
		private int _lastHash;
	}
}
