using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001EF RID: 495
	internal static class SnapshotManager
	{
		// Token: 0x06001015 RID: 4117
		[DllImport("gpg")]
		internal static extern void SnapshotManager_FetchAll(HandleRef self, Types.DataSource data_source, SnapshotManager.FetchAllCallback callback, IntPtr callback_arg);

		// Token: 0x06001016 RID: 4118
		[DllImport("gpg")]
		internal static extern void SnapshotManager_ShowSelectUIOperation(HandleRef self, [MarshalAs(UnmanagedType.I1)] bool allow_create, [MarshalAs(UnmanagedType.I1)] bool allow_delete, uint max_snapshots, string title, SnapshotManager.SnapshotSelectUICallback callback, IntPtr callback_arg);

		// Token: 0x06001017 RID: 4119
		[DllImport("gpg")]
		internal static extern void SnapshotManager_Read(HandleRef self, IntPtr snapshot_metadata, SnapshotManager.ReadCallback callback, IntPtr callback_arg);

		// Token: 0x06001018 RID: 4120
		[DllImport("gpg")]
		internal static extern void SnapshotManager_Commit(HandleRef self, IntPtr snapshot_metadata, IntPtr metadata_change, byte[] data, UIntPtr data_size, SnapshotManager.CommitCallback callback, IntPtr callback_arg);

		// Token: 0x06001019 RID: 4121
		[DllImport("gpg")]
		internal static extern void SnapshotManager_Open(HandleRef self, Types.DataSource data_source, string file_name, Types.SnapshotConflictPolicy conflict_policy, SnapshotManager.OpenCallback callback, IntPtr callback_arg);

		// Token: 0x0600101A RID: 4122
		[DllImport("gpg")]
		internal static extern void SnapshotManager_ResolveConflict(HandleRef self, IntPtr snapshot_metadata, IntPtr metadata_change, string conflict_id, SnapshotManager.CommitCallback callback, IntPtr callback_arg);

		// Token: 0x0600101B RID: 4123
		[DllImport("gpg")]
		internal static extern void SnapshotManager_Delete(HandleRef self, IntPtr snapshot_metadata);

		// Token: 0x0600101C RID: 4124
		[DllImport("gpg")]
		internal static extern void SnapshotManager_FetchAllResponse_Dispose(HandleRef self);

		// Token: 0x0600101D RID: 4125
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus SnapshotManager_FetchAllResponse_GetStatus(HandleRef self);

		// Token: 0x0600101E RID: 4126
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotManager_FetchAllResponse_GetData_Length(HandleRef self);

		// Token: 0x0600101F RID: 4127
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotManager_FetchAllResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06001020 RID: 4128
		[DllImport("gpg")]
		internal static extern void SnapshotManager_OpenResponse_Dispose(HandleRef self);

		// Token: 0x06001021 RID: 4129
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.SnapshotOpenStatus SnapshotManager_OpenResponse_GetStatus(HandleRef self);

		// Token: 0x06001022 RID: 4130
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotManager_OpenResponse_GetData(HandleRef self);

		// Token: 0x06001023 RID: 4131
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotManager_OpenResponse_GetConflictId(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06001024 RID: 4132
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotManager_OpenResponse_GetConflictOriginal(HandleRef self);

		// Token: 0x06001025 RID: 4133
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotManager_OpenResponse_GetConflictUnmerged(HandleRef self);

		// Token: 0x06001026 RID: 4134
		[DllImport("gpg")]
		internal static extern void SnapshotManager_CommitResponse_Dispose(HandleRef self);

		// Token: 0x06001027 RID: 4135
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus SnapshotManager_CommitResponse_GetStatus(HandleRef self);

		// Token: 0x06001028 RID: 4136
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotManager_CommitResponse_GetData(HandleRef self);

		// Token: 0x06001029 RID: 4137
		[DllImport("gpg")]
		internal static extern void SnapshotManager_ReadResponse_Dispose(HandleRef self);

		// Token: 0x0600102A RID: 4138
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus SnapshotManager_ReadResponse_GetStatus(HandleRef self);

		// Token: 0x0600102B RID: 4139
		[DllImport("gpg")]
		internal static extern UIntPtr SnapshotManager_ReadResponse_GetData(HandleRef self, [In] [Out] byte[] out_arg, UIntPtr out_size);

		// Token: 0x0600102C RID: 4140
		[DllImport("gpg")]
		internal static extern void SnapshotManager_SnapshotSelectUIResponse_Dispose(HandleRef self);

		// Token: 0x0600102D RID: 4141
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.UIStatus SnapshotManager_SnapshotSelectUIResponse_GetStatus(HandleRef self);

		// Token: 0x0600102E RID: 4142
		[DllImport("gpg")]
		internal static extern IntPtr SnapshotManager_SnapshotSelectUIResponse_GetData(HandleRef self);

		// Token: 0x020008D2 RID: 2258
		// (Invoke) Token: 0x06004FE8 RID: 20456
		internal delegate void FetchAllCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D3 RID: 2259
		// (Invoke) Token: 0x06004FEC RID: 20460
		internal delegate void OpenCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D4 RID: 2260
		// (Invoke) Token: 0x06004FF0 RID: 20464
		internal delegate void CommitCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D5 RID: 2261
		// (Invoke) Token: 0x06004FF4 RID: 20468
		internal delegate void ReadCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008D6 RID: 2262
		// (Invoke) Token: 0x06004FF8 RID: 20472
		internal delegate void SnapshotSelectUICallback(IntPtr arg0, IntPtr arg1);
	}
}
