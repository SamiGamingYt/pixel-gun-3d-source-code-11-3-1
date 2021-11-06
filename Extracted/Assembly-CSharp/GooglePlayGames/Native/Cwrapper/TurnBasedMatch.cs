using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001FF RID: 511
	internal static class TurnBasedMatch
	{
		// Token: 0x06001049 RID: 4169
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatch_AutomatchingSlotsAvailable(HandleRef self);

		// Token: 0x0600104A RID: 4170
		[DllImport("gpg")]
		internal static extern ulong TurnBasedMatch_CreationTime(HandleRef self);

		// Token: 0x0600104B RID: 4171
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatch_Participants_Length(HandleRef self);

		// Token: 0x0600104C RID: 4172
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatch_Participants_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x0600104D RID: 4173
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatch_Version(HandleRef self);

		// Token: 0x0600104E RID: 4174
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatch_ParticipantResults(HandleRef self);

		// Token: 0x0600104F RID: 4175
		[DllImport("gpg")]
		internal static extern Types.MatchStatus TurnBasedMatch_Status(HandleRef self);

		// Token: 0x06001050 RID: 4176
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatch_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001051 RID: 4177
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatch_PendingParticipant(HandleRef self);

		// Token: 0x06001052 RID: 4178
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatch_Variant(HandleRef self);

		// Token: 0x06001053 RID: 4179
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_HasPreviousMatchData(HandleRef self);

		// Token: 0x06001054 RID: 4180
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatch_Data(HandleRef self, [In] [Out] byte[] out_arg, UIntPtr out_size);

		// Token: 0x06001055 RID: 4181
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatch_LastUpdatingParticipant(HandleRef self);

		// Token: 0x06001056 RID: 4182
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_HasData(HandleRef self);

		// Token: 0x06001057 RID: 4183
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatch_SuggestedNextParticipant(HandleRef self);

		// Token: 0x06001058 RID: 4184
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatch_PreviousMatchData(HandleRef self, [In] [Out] byte[] out_arg, UIntPtr out_size);

		// Token: 0x06001059 RID: 4185
		[DllImport("gpg")]
		internal static extern ulong TurnBasedMatch_LastUpdateTime(HandleRef self);

		// Token: 0x0600105A RID: 4186
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatch_RematchId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x0600105B RID: 4187
		[DllImport("gpg")]
		internal static extern uint TurnBasedMatch_Number(HandleRef self);

		// Token: 0x0600105C RID: 4188
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_HasRematchId(HandleRef self);

		// Token: 0x0600105D RID: 4189
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_Valid(HandleRef self);

		// Token: 0x0600105E RID: 4190
		[DllImport("gpg")]
		internal static extern IntPtr TurnBasedMatch_CreatingParticipant(HandleRef self);

		// Token: 0x0600105F RID: 4191
		[DllImport("gpg")]
		internal static extern UIntPtr TurnBasedMatch_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001060 RID: 4192
		[DllImport("gpg")]
		internal static extern void TurnBasedMatch_Dispose(HandleRef self);
	}
}
