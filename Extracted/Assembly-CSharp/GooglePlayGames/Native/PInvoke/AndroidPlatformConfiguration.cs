using System;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000235 RID: 565
	internal sealed class AndroidPlatformConfiguration : PlatformConfiguration
	{
		// Token: 0x060011DC RID: 4572 RVA: 0x0004C6EC File Offset: 0x0004A8EC
		private AndroidPlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0004C6F8 File Offset: 0x0004A8F8
		internal void SetActivity(IntPtr activity)
		{
			AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetActivity(base.SelfPtr(), activity);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004C708 File Offset: 0x0004A908
		internal void SetOptionalIntentHandlerForUI(Action<IntPtr> intentHandler)
		{
			Misc.CheckNotNull<Action<IntPtr>>(intentHandler);
			AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(base.SelfPtr(), new AndroidPlatformConfiguration.IntentHandler(AndroidPlatformConfiguration.InternalIntentHandler), Callbacks.ToIntPtr(intentHandler));
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0004C73C File Offset: 0x0004A93C
		protected override void CallDispose(HandleRef selfPointer)
		{
			AndroidPlatformConfiguration.AndroidPlatformConfiguration_Dispose(selfPointer);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0004C744 File Offset: 0x0004A944
		[MonoPInvokeCallback(typeof(AndroidPlatformConfiguration.IntentHandlerInternal))]
		private static void InternalIntentHandler(IntPtr intent, IntPtr userData)
		{
			Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler", Callbacks.Type.Permanent, intent, userData);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0004C754 File Offset: 0x0004A954
		internal static AndroidPlatformConfiguration Create()
		{
			return new AndroidPlatformConfiguration(AndroidPlatformConfiguration.AndroidPlatformConfiguration_Construct());
		}

		// Token: 0x020008DD RID: 2269
		// (Invoke) Token: 0x06005014 RID: 20500
		private delegate void IntentHandlerInternal(IntPtr intent, IntPtr userData);
	}
}
