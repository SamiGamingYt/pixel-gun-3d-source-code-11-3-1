using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001C3 RID: 451
	internal static class AndroidPlatformConfiguration
	{
		// Token: 0x06000EBB RID: 3771
		[DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithSnapshot(HandleRef self, AndroidPlatformConfiguration.OnLaunchedWithSnapshotCallback callback, IntPtr callback_arg);

		// Token: 0x06000EBC RID: 3772
		[DllImport("gpg")]
		internal static extern IntPtr AndroidPlatformConfiguration_Construct();

		// Token: 0x06000EBD RID: 3773
		[DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(HandleRef self, AndroidPlatformConfiguration.IntentHandler intent_handler, IntPtr intent_handler_arg);

		// Token: 0x06000EBE RID: 3774
		[DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_Dispose(HandleRef self);

		// Token: 0x06000EBF RID: 3775
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool AndroidPlatformConfiguration_Valid(HandleRef self);

		// Token: 0x06000EC0 RID: 3776
		[DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetActivity(HandleRef self, IntPtr android_app_activity);

		// Token: 0x06000EC1 RID: 3777
		[DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithQuest(HandleRef self, AndroidPlatformConfiguration.OnLaunchedWithQuestCallback callback, IntPtr callback_arg);

		// Token: 0x020008A0 RID: 2208
		// (Invoke) Token: 0x06004F20 RID: 20256
		internal delegate void IntentHandler(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008A1 RID: 2209
		// (Invoke) Token: 0x06004F24 RID: 20260
		internal delegate void OnLaunchedWithSnapshotCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x020008A2 RID: 2210
		// (Invoke) Token: 0x06004F28 RID: 20264
		internal delegate void OnLaunchedWithQuestCallback(IntPtr arg0, IntPtr arg1);
	}
}
