using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D4 RID: 468
	internal static class Leaderboard
	{
		// Token: 0x06000EF5 RID: 3829
		[DllImport("gpg")]
		internal static extern UIntPtr Leaderboard_Name(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EF6 RID: 3830
		[DllImport("gpg")]
		internal static extern UIntPtr Leaderboard_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EF7 RID: 3831
		[DllImport("gpg")]
		internal static extern UIntPtr Leaderboard_IconUrl(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000EF8 RID: 3832
		[DllImport("gpg")]
		internal static extern void Leaderboard_Dispose(HandleRef self);

		// Token: 0x06000EF9 RID: 3833
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Leaderboard_Valid(HandleRef self);

		// Token: 0x06000EFA RID: 3834
		[DllImport("gpg")]
		internal static extern Types.LeaderboardOrder Leaderboard_Order(HandleRef self);
	}
}
