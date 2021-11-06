using System;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200027D RID: 637
	internal class StatsManager
	{
		// Token: 0x0600146A RID: 5226 RVA: 0x000517CC File Offset: 0x0004F9CC
		internal StatsManager(GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x000517E0 File Offset: 0x0004F9E0
		internal void FetchForPlayer(Action<StatsManager.FetchForPlayerResponse> callback)
		{
			Misc.CheckNotNull<Action<StatsManager.FetchForPlayerResponse>>(callback);
			StatsManager.StatsManager_FetchForPlayer(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new StatsManager.FetchForPlayerCallback(StatsManager.InternalFetchForPlayerCallback), Callbacks.ToIntPtr<StatsManager.FetchForPlayerResponse>(callback, new Func<IntPtr, StatsManager.FetchForPlayerResponse>(StatsManager.FetchForPlayerResponse.FromPointer)));
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00051824 File Offset: 0x0004FA24
		[MonoPInvokeCallback(typeof(StatsManager.FetchForPlayerCallback))]
		private static void InternalFetchForPlayerCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("StatsManager#InternalFetchForPlayerCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x04000C11 RID: 3089
		private readonly GameServices mServices;

		// Token: 0x0200027E RID: 638
		internal class FetchForPlayerResponse : BaseReferenceHolder
		{
			// Token: 0x0600146D RID: 5229 RVA: 0x00051834 File Offset: 0x0004FA34
			internal FetchForPlayerResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x0600146E RID: 5230 RVA: 0x00051840 File Offset: 0x0004FA40
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return StatsManager.StatsManager_FetchForPlayerResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x0600146F RID: 5231 RVA: 0x00051850 File Offset: 0x0004FA50
			internal NativePlayerStats PlayerStats()
			{
				IntPtr selfPointer = StatsManager.StatsManager_FetchForPlayerResponse_GetData(base.SelfPtr());
				return new NativePlayerStats(selfPointer);
			}

			// Token: 0x06001470 RID: 5232 RVA: 0x00051870 File Offset: 0x0004FA70
			protected override void CallDispose(HandleRef selfPointer)
			{
				StatsManager.StatsManager_FetchForPlayerResponse_Dispose(selfPointer);
			}

			// Token: 0x06001471 RID: 5233 RVA: 0x00051878 File Offset: 0x0004FA78
			internal static StatsManager.FetchForPlayerResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new StatsManager.FetchForPlayerResponse(pointer);
			}
		}
	}
}
