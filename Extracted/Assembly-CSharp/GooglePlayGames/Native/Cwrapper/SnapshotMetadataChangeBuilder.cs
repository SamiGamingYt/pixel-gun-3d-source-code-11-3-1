using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001F2 RID: 498
	internal static class SnapshotMetadataChangeBuilder
	{
		// Token: 0x0600103F RID: 4159
		[DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_SetDescription(HandleRef self, string description);

		// Token: 0x06001040 RID: 4160
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotMetadataChange_Builder_Construct();

		// Token: 0x06001041 RID: 4161
		[DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_SetPlayedTime(HandleRef self, ulong played_time);

		// Token: 0x06001042 RID: 4162
		[DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_SetCoverImageFromPngData(HandleRef self, byte[] png_data, UIntPtr png_data_size);

		// Token: 0x06001043 RID: 4163
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotMetadataChange_Builder_Create(HandleRef self);

		// Token: 0x06001044 RID: 4164
		[DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_Dispose(HandleRef self);
	}
}
