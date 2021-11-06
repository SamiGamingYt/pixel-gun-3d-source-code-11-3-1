using System;
using System.Runtime.InteropServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000250 RID: 592
	internal class NativePlayerStats : BaseReferenceHolder
	{
		// Token: 0x060012CC RID: 4812 RVA: 0x0004E6A0 File Offset: 0x0004C8A0
		internal NativePlayerStats(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0004E6AC File Offset: 0x0004C8AC
		internal bool Valid()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Valid(base.SelfPtr());
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0004E6BC File Offset: 0x0004C8BC
		internal bool HasAverageSessionLength()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasAverageSessionLength(base.SelfPtr());
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0004E6CC File Offset: 0x0004C8CC
		internal float AverageSessionLength()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_AverageSessionLength(base.SelfPtr());
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0004E6DC File Offset: 0x0004C8DC
		internal bool HasChurnProbability()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasChurnProbability(base.SelfPtr());
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0004E6EC File Offset: 0x0004C8EC
		internal float ChurnProbability()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_ChurnProbability(base.SelfPtr());
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0004E6FC File Offset: 0x0004C8FC
		internal bool HasDaysSinceLastPlayed()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasDaysSinceLastPlayed(base.SelfPtr());
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0004E70C File Offset: 0x0004C90C
		internal int DaysSinceLastPlayed()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_DaysSinceLastPlayed(base.SelfPtr());
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0004E71C File Offset: 0x0004C91C
		internal bool HasNumberOfPurchases()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfPurchases(base.SelfPtr());
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0004E72C File Offset: 0x0004C92C
		internal int NumberOfPurchases()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfPurchases(base.SelfPtr());
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0004E73C File Offset: 0x0004C93C
		internal bool HasNumberOfSessions()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfSessions(base.SelfPtr());
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0004E74C File Offset: 0x0004C94C
		internal int NumberOfSessions()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfSessions(base.SelfPtr());
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0004E75C File Offset: 0x0004C95C
		internal bool HasSessionPercentile()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSessionPercentile(base.SelfPtr());
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0004E76C File Offset: 0x0004C96C
		internal float SessionPercentile()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SessionPercentile(base.SelfPtr());
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0004E77C File Offset: 0x0004C97C
		internal bool HasSpendPercentile()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSpendPercentile(base.SelfPtr());
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0004E78C File Offset: 0x0004C98C
		internal float SpendPercentile()
		{
			return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SpendPercentile(base.SelfPtr());
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0004E79C File Offset: 0x0004C99C
		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Dispose(selfPointer);
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0004E7A4 File Offset: 0x0004C9A4
		internal GooglePlayGames.BasicApi.PlayerStats AsPlayerStats()
		{
			GooglePlayGames.BasicApi.PlayerStats playerStats = new GooglePlayGames.BasicApi.PlayerStats();
			playerStats.Valid = this.Valid();
			if (this.Valid())
			{
				playerStats.AvgSessonLength = this.AverageSessionLength();
				playerStats.ChurnProbability = this.ChurnProbability();
				playerStats.DaysSinceLastPlayed = this.DaysSinceLastPlayed();
				playerStats.NumberOfPurchases = this.NumberOfPurchases();
				playerStats.NumberOfSessions = this.NumberOfSessions();
				playerStats.SessPercentile = this.SessionPercentile();
				playerStats.SpendPercentile = this.SpendPercentile();
				playerStats.SpendProbability = -1f;
			}
			return playerStats;
		}
	}
}
