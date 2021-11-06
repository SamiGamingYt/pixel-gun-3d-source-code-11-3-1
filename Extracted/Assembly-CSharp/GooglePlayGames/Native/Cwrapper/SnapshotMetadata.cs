using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001F0 RID: 496
	internal static class SnapshotMetadata
	{
		// Token: 0x0600102F RID: 4143
		[DllImport("gpg")]
		internal static extern void SnapshotMetadata_Dispose(HandleRef self);

		// Token: 0x06001030 RID: 4144
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotMetadata_CoverImageURL(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001031 RID: 4145
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotMetadata_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001032 RID: 4146
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool SnapshotMetadata_IsOpen(HandleRef self);

		// Token: 0x06001033 RID: 4147
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotMetadata_FileName(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001034 RID: 4148
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool SnapshotMetadata_Valid(HandleRef self);

		// Token: 0x06001035 RID: 4149
		[DllImport("gpg")]
		internal static extern long SnapshotMetadata_PlayedTime(HandleRef self);

		// Token: 0x06001036 RID: 4150
		[DllImport("gpg")]
		internal static extern long SnapshotMetadata_LastModifiedTime(HandleRef self);
	}
}
