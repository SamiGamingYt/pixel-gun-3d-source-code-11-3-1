using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001F1 RID: 497
	internal static class SnapshotMetadataChange
	{
		// Token: 0x06001037 RID: 4151
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotMetadataChange_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001038 RID: 4152
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotMetadataChange_Image(HandleRef self);

		// Token: 0x06001039 RID: 4153
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_PlayedTimeIsChanged(HandleRef self);

		// Token: 0x0600103A RID: 4154
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_Valid(HandleRef self);

		// Token: 0x0600103B RID: 4155
		[DllImport("gpg")]
		internal static extern ulong SnapshotMetadataChange_PlayedTime(HandleRef self);

		// Token: 0x0600103C RID: 4156
		[DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Dispose(HandleRef self);

		// Token: 0x0600103D RID: 4157
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_ImageIsChanged(HandleRef self);

		// Token: 0x0600103E RID: 4158
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_DescriptionIsChanged(HandleRef self);
	}
}
