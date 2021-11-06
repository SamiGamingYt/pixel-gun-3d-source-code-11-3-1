using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001C4 RID: 452
	internal static class Builder
	{
		// Token: 0x06000EC2 RID: 3778
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnAuthActionStarted(HandleRef self, Builder.OnAuthActionStartedCallback callback, IntPtr callback_arg);

		// Token: 0x06000EC3 RID: 3779
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_AddOauthScope(HandleRef self, string scope);

		// Token: 0x06000EC4 RID: 3780
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetLogging(HandleRef self, Builder.OnLogCallback callback, IntPtr callback_arg, Types.LogLevel min_level);

		// Token: 0x06000EC5 RID: 3781
		[DllImport("gpg")]
		internal static extern IntPtr GameServices_Builder_Construct();

		// Token: 0x06000EC6 RID: 3782
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_EnableSnapshots(HandleRef self);

		// Token: 0x06000EC7 RID: 3783
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_RequireGooglePlus(HandleRef self);

		// Token: 0x06000EC8 RID: 3784
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnLog(HandleRef self, Builder.OnLogCallback callback, IntPtr callback_arg, Types.LogLevel min_level);

		// Token: 0x06000EC9 RID: 3785
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetDefaultOnLog(HandleRef self, Types.LogLevel min_level);

		// Token: 0x06000ECA RID: 3786
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnAuthActionFinished(HandleRef self, Builder.OnAuthActionFinishedCallback callback, IntPtr callback_arg);

		// Token: 0x06000ECB RID: 3787
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnTurnBasedMatchEvent(HandleRef self, Builder.OnTurnBasedMatchEventCallback callback, IntPtr callback_arg);

		// Token: 0x06000ECC RID: 3788
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnQuestCompleted(HandleRef self, Builder.OnQuestCompletedCallback callback, IntPtr callback_arg);

		// Token: 0x06000ECD RID: 3789
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnMultiplayerInvitationEvent(HandleRef self, Builder.OnMultiplayerInvitationEventCallback callback, IntPtr callback_arg);

		// Token: 0x06000ECE RID: 3790
		[DllImport("gpg")]
		internal static extern IntPtr GameServices_Builder_Create(HandleRef self, IntPtr platform);

		// Token: 0x06000ECF RID: 3791
		[DllImport("gpg")]
		internal static extern void GameServices_Builder_Dispose(HandleRef self);

		// Token: 0x020008A3 RID: 2211
		// (Invoke) Token: 0x06004F2C RID: 20268
		internal delegate void OnLogCallback(Types.LogLevel arg0, string arg1, IntPtr arg2);

		// Token: 0x020008A4 RID: 2212
		// (Invoke) Token: 0x06004F30 RID: 20272
		internal delegate void OnAuthActionStartedCallback(Types.AuthOperation arg0, IntPtr arg1);

		// Token: 0x020008A5 RID: 2213
		// (Invoke) Token: 0x06004F34 RID: 20276
		internal delegate void OnAuthActionFinishedCallback(Types.AuthOperation arg0, CommonErrorStatus.AuthStatus arg1, IntPtr arg2);

		// Token: 0x020008A6 RID: 2214
		// (Invoke) Token: 0x06004F38 RID: 20280
		internal delegate void OnMultiplayerInvitationEventCallback(Types.MultiplayerEvent arg0, string arg1, IntPtr arg2, IntPtr arg3);

		// Token: 0x020008A7 RID: 2215
		// (Invoke) Token: 0x06004F3C RID: 20284
		internal delegate void OnTurnBasedMatchEventCallback(Types.MultiplayerEvent arg0, string arg1, IntPtr arg2, IntPtr arg3);

		// Token: 0x020008A8 RID: 2216
		// (Invoke) Token: 0x06004F40 RID: 20288
		internal delegate void OnQuestCompletedCallback(IntPtr arg0, IntPtr arg1);
	}
}
