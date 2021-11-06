using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D7 RID: 471
	internal static class MultiplayerInvitation
	{
		// Token: 0x06000F19 RID: 3865
		[DllImport("gpg")]
		internal static extern uint MultiplayerInvitation_AutomatchingSlotsAvailable(HandleRef self);

		// Token: 0x06000F1A RID: 3866
		[DllImport("gpg")]
		internal static extern IntPtr MultiplayerInvitation_InvitingParticipant(HandleRef self);

		// Token: 0x06000F1B RID: 3867
		[DllImport("gpg")]
		internal static extern uint MultiplayerInvitation_Variant(HandleRef self);

		// Token: 0x06000F1C RID: 3868
		[DllImport("gpg")]
		internal static extern ulong MultiplayerInvitation_CreationTime(HandleRef self);

		// Token: 0x06000F1D RID: 3869
		[DllImport("gpg")]
		internal static extern UIntPtr MultiplayerInvitation_Participants_Length(HandleRef self);

		// Token: 0x06000F1E RID: 3870
		[DllImport("gpg")]
		internal static extern IntPtr MultiplayerInvitation_Participants_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06000F1F RID: 3871
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool MultiplayerInvitation_Valid(HandleRef self);

		// Token: 0x06000F20 RID: 3872
		[DllImport("gpg")]
		internal static extern Types.MultiplayerInvitationType MultiplayerInvitation_Type(HandleRef self);

		// Token: 0x06000F21 RID: 3873
		[DllImport("gpg")]
		internal static extern UIntPtr MultiplayerInvitation_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000F22 RID: 3874
		[DllImport("gpg")]
		internal static extern void MultiplayerInvitation_Dispose(HandleRef self);
	}
}
