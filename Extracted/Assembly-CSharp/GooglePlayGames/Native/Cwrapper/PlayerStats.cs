using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001E2 RID: 482
	internal static class PlayerStats
	{
		// Token: 0x06000F77 RID: 3959
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_Valid(HandleRef self);

		// Token: 0x06000F78 RID: 3960
		[DllImport("gpg")]
		internal static extern void PlayerStats_Dispose(HandleRef self);

		// Token: 0x06000F79 RID: 3961
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasAverageSessionLength(HandleRef self);

		// Token: 0x06000F7A RID: 3962
		[DllImport("gpg")]
		internal static extern float PlayerStats_AverageSessionLength(HandleRef self);

		// Token: 0x06000F7B RID: 3963
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasChurnProbability(HandleRef self);

		// Token: 0x06000F7C RID: 3964
		[DllImport("gpg")]
		internal static extern float PlayerStats_ChurnProbability(HandleRef self);

		// Token: 0x06000F7D RID: 3965
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasDaysSinceLastPlayed(HandleRef self);

		// Token: 0x06000F7E RID: 3966
		[DllImport("gpg")]
		internal static extern int PlayerStats_DaysSinceLastPlayed(HandleRef self);

		// Token: 0x06000F7F RID: 3967
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasNumberOfPurchases(HandleRef self);

		// Token: 0x06000F80 RID: 3968
		[DllImport("gpg")]
		internal static extern int PlayerStats_NumberOfPurchases(HandleRef self);

		// Token: 0x06000F81 RID: 3969
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasNumberOfSessions(HandleRef self);

		// Token: 0x06000F82 RID: 3970
		[DllImport("gpg")]
		internal static extern int PlayerStats_NumberOfSessions(HandleRef self);

		// Token: 0x06000F83 RID: 3971
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasSessionPercentile(HandleRef self);

		// Token: 0x06000F84 RID: 3972
		[DllImport("gpg")]
		internal static extern float PlayerStats_SessionPercentile(HandleRef self);

		// Token: 0x06000F85 RID: 3973
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool PlayerStats_HasSpendPercentile(HandleRef self);

		// Token: 0x06000F86 RID: 3974
		[DllImport("gpg")]
		internal static extern float PlayerStats_SpendPercentile(HandleRef self);
	}
}
