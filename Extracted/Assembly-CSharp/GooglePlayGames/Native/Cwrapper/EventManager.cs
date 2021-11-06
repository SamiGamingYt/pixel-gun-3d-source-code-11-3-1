using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001D0 RID: 464
	internal static class EventManager
	{
		// Token: 0x06000EDD RID: 3805
		[DllImport("gpg")]
		internal static extern void EventManager_FetchAll(HandleRef self, Types.DataSource data_source, EventManager.FetchAllCallback callback, IntPtr callback_arg);

		// Token: 0x06000EDE RID: 3806
		[DllImport("gpg")]
		internal static extern void EventManager_Fetch(HandleRef self, Types.DataSource data_source, string event_id, EventManager.FetchCallback callback, IntPtr callback_arg);

		// Token: 0x06000EDF RID: 3807
		[DllImport("gpg")]
		internal static extern void EventManager_Increment(HandleRef self, string event_id, uint steps);

		// Token: 0x06000EE0 RID: 3808
		[DllImport("gpg")]
		internal static extern void EventManager_FetchAllResponse_Dispose(HandleRef self);

		// Token: 0x06000EE1 RID: 3809
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus EventManager_FetchAllResponse_GetStatus(HandleRef self);

		// Token: 0x06000EE2 RID: 3810
		[DllImport("gpg")]
		internal static extern UIntPtr EventManager_FetchAllResponse_GetData(HandleRef self, IntPtr[] out_arg, UIntPtr out_size);

		// Token: 0x06000EE3 RID: 3811
		[DllImport("gpg")]
		internal static extern void EventManager_FetchResponse_Dispose(HandleRef self);

		// Token: 0x06000EE4 RID: 3812
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus EventManager_FetchResponse_GetStatus(HandleRef self);

		// Token: 0x06000EE5 RID: 3813
		[DllImport("gpg")]
		internal static extern IntPtr EventManager_FetchResponse_GetData(HandleRef self);

		// Token: 0x020008AB RID: 2219
		// (Invoke) Token: 0x06004F4C RID: 20300
		internal delegate void FetchAllCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008AC RID: 2220
		// (Invoke) Token: 0x06004F50 RID: 20304
		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);
	}
}
