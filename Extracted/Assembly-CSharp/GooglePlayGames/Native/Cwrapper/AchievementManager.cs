using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001C2 RID: 450
	internal static class AchievementManager
	{
		// Token: 0x06000EAD RID: 3757
		[DllImport("gpg")]
		internal static extern void AchievementManager_FetchAll(HandleRef self, Types.DataSource data_source, AchievementManager.FetchAllCallback callback, IntPtr callback_arg);

		// Token: 0x06000EAE RID: 3758
		[DllImport("gpg")]
		internal static extern void AchievementManager_Reveal(HandleRef self, string achievement_id);

		// Token: 0x06000EAF RID: 3759
		[DllImport("gpg")]
		internal static extern void AchievementManager_Unlock(HandleRef self, string achievement_id);

		// Token: 0x06000EB0 RID: 3760
		[DllImport("gpg")]
		internal static extern void AchievementManager_ShowAllUI(HandleRef self, AchievementManager.ShowAllUICallback callback, IntPtr callback_arg);

		// Token: 0x06000EB1 RID: 3761
		[DllImport("gpg")]
		internal static extern void AchievementManager_SetStepsAtLeast(HandleRef self, string achievement_id, uint steps);

		// Token: 0x06000EB2 RID: 3762
		[DllImport("gpg")]
		internal static extern void AchievementManager_Increment(HandleRef self, string achievement_id, uint steps);

		// Token: 0x06000EB3 RID: 3763
		[DllImport("gpg")]
		internal static extern void AchievementManager_Fetch(HandleRef self, Types.DataSource data_source, string achievement_id, AchievementManager.FetchCallback callback, IntPtr callback_arg);

		// Token: 0x06000EB4 RID: 3764
		[DllImport("gpg")]
		internal static extern void AchievementManager_FetchAllResponse_Dispose(HandleRef self);

		// Token: 0x06000EB5 RID: 3765
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus AchievementManager_FetchAllResponse_GetStatus(HandleRef self);

		// Token: 0x06000EB6 RID: 3766
		[DllImport("gpg")]
		internal static extern UIntPtr AchievementManager_FetchAllResponse_GetData_Length(HandleRef self);

		// Token: 0x06000EB7 RID: 3767
		[DllImport("gpg")]
		internal static extern IntPtr AchievementManager_FetchAllResponse_GetData_GetElement(HandleRef self, UIntPtr index);

		// Token: 0x06000EB8 RID: 3768
		[DllImport("gpg")]
		internal static extern void AchievementManager_FetchResponse_Dispose(HandleRef self);

		// Token: 0x06000EB9 RID: 3769
		[DllImport("gpg")]
		internal static extern CommonErrorStatus.ResponseStatus AchievementManager_FetchResponse_GetStatus(HandleRef self);

		// Token: 0x06000EBA RID: 3770
		[DllImport("gpg")]
		internal static extern IntPtr AchievementManager_FetchResponse_GetData(HandleRef self);

		// Token: 0x0200089D RID: 2205
		// (Invoke) Token: 0x06004F14 RID: 20244
		internal delegate void FetchAllCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x0200089E RID: 2206
		// (Invoke) Token: 0x06004F18 RID: 20248
		internal delegate void FetchCallback(IntPtr arg0, IntPtr arg1);

		// Token: 0x0200089F RID: 2207
		// (Invoke) Token: 0x06004F1C RID: 20252
		internal delegate void ShowAllUICallback(CommonErrorStatus.UIStatus arg0, IntPtr arg1);
	}
}
