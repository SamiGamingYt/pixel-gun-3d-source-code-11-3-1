using System;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200025F RID: 607
	internal class NearbyConnectionsManagerBuilder : BaseReferenceHolder
	{
		// Token: 0x06001382 RID: 4994 RVA: 0x0004FB94 File Offset: 0x0004DD94
		internal NearbyConnectionsManagerBuilder() : base(NearbyConnectionsBuilder.NearbyConnections_Builder_Construct())
		{
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0004FBA4 File Offset: 0x0004DDA4
		internal NearbyConnectionsManagerBuilder SetOnInitializationFinished(Action<NearbyConnectionsStatus.InitializationStatus> callback)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_SetOnInitializationFinished(base.SelfPtr(), new NearbyConnectionsBuilder.OnInitializationFinishedCallback(NearbyConnectionsManagerBuilder.InternalOnInitializationFinishedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0004FBC4 File Offset: 0x0004DDC4
		[MonoPInvokeCallback(typeof(NearbyConnectionsBuilder.OnInitializationFinishedCallback))]
		private static void InternalOnInitializationFinishedCallback(NearbyConnectionsStatus.InitializationStatus status, IntPtr userData)
		{
			Action<NearbyConnectionsStatus.InitializationStatus> action = Callbacks.IntPtrToPermanentCallback<Action<NearbyConnectionsStatus.InitializationStatus>>(userData);
			if (action == null)
			{
				Logger.w("Callback for Initialization is null. Received status: " + status);
				return;
			}
			try
			{
				action(status);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing NearbyConnectionsManagerBuilder#InternalOnInitializationFinishedCallback. Smothering exception: " + arg);
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0004FC34 File Offset: 0x0004DE34
		internal NearbyConnectionsManagerBuilder SetLocalClientId(long localClientId)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_SetClientId(base.SelfPtr(), localClientId);
			return this;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0004FC44 File Offset: 0x0004DE44
		internal NearbyConnectionsManagerBuilder SetDefaultLogLevel(Types.LogLevel minLevel)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_SetDefaultOnLog(base.SelfPtr(), minLevel);
			return this;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0004FC54 File Offset: 0x0004DE54
		internal NearbyConnectionsManager Build(PlatformConfiguration configuration)
		{
			return new NearbyConnectionsManager(NearbyConnectionsBuilder.NearbyConnections_Builder_Create(base.SelfPtr(), configuration.AsPointer()));
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0004FC6C File Offset: 0x0004DE6C
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_Dispose(selfPointer);
		}
	}
}
