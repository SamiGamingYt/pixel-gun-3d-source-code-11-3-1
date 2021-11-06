using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000246 RID: 582
	internal class NativeAchievement : BaseReferenceHolder
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x0004DCB4 File Offset: 0x0004BEB4
		internal NativeAchievement(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0004DCC0 File Offset: 0x0004BEC0
		internal uint CurrentSteps()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_CurrentSteps(base.SelfPtr());
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0004DCD0 File Offset: 0x0004BED0
		internal string Description()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Description(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0004DCE4 File Offset: 0x0004BEE4
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Id(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0004DCF8 File Offset: 0x0004BEF8
		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Name(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0004DD0C File Offset: 0x0004BF0C
		internal Types.AchievementState State()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_State(base.SelfPtr());
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0004DD1C File Offset: 0x0004BF1C
		internal uint TotalSteps()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_TotalSteps(base.SelfPtr());
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0004DD2C File Offset: 0x0004BF2C
		internal Types.AchievementType Type()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Type(base.SelfPtr());
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0004DD3C File Offset: 0x0004BF3C
		internal ulong LastModifiedTime()
		{
			if (GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Valid(base.SelfPtr()))
			{
				return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_LastModifiedTime(base.SelfPtr());
			}
			return 0UL;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0004DD68 File Offset: 0x0004BF68
		internal ulong getXP()
		{
			return GooglePlayGames.Native.Cwrapper.Achievement.Achievement_XP(base.SelfPtr());
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0004DD78 File Offset: 0x0004BF78
		internal string getRevealedImageUrl()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_RevealedIconUrl(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0004DD8C File Offset: 0x0004BF8C
		internal string getUnlockedImageUrl()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Achievement.Achievement_UnlockedIconUrl(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0004DDA0 File Offset: 0x0004BFA0
		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Dispose(selfPointer);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0004DDA8 File Offset: 0x0004BFA8
		internal GooglePlayGames.BasicApi.Achievement AsAchievement()
		{
			GooglePlayGames.BasicApi.Achievement achievement = new GooglePlayGames.BasicApi.Achievement();
			achievement.Id = this.Id();
			achievement.Name = this.Name();
			achievement.Description = this.Description();
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			ulong num = this.LastModifiedTime();
			if (num == 18446744073709551615UL)
			{
				num = 0UL;
			}
			achievement.LastModifiedTime = dateTime.AddMilliseconds(num);
			achievement.Points = this.getXP();
			achievement.RevealedImageUrl = this.getRevealedImageUrl();
			achievement.UnlockedImageUrl = this.getUnlockedImageUrl();
			if (this.Type() == Types.AchievementType.INCREMENTAL)
			{
				achievement.IsIncremental = true;
				achievement.CurrentSteps = (int)this.CurrentSteps();
				achievement.TotalSteps = (int)this.TotalSteps();
			}
			achievement.IsRevealed = (this.State() == Types.AchievementState.REVEALED || this.State() == Types.AchievementState.UNLOCKED);
			achievement.IsUnlocked = (this.State() == Types.AchievementState.UNLOCKED);
			return achievement;
		}

		// Token: 0x04000C02 RID: 3074
		private const ulong MinusOne = 18446744073709551615UL;
	}
}
