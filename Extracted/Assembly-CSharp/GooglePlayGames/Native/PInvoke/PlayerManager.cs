using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000263 RID: 611
	internal class PlayerManager
	{
		// Token: 0x0600139C RID: 5020 RVA: 0x0004FEAC File Offset: 0x0004E0AC
		internal PlayerManager(GameServices services)
		{
			this.mGameServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0004FEC0 File Offset: 0x0004E0C0
		internal void FetchSelf(Action<PlayerManager.FetchSelfResponse> callback)
		{
			PlayerManager.PlayerManager_FetchSelf(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new PlayerManager.FetchSelfCallback(PlayerManager.InternalFetchSelfCallback), Callbacks.ToIntPtr<PlayerManager.FetchSelfResponse>(callback, new Func<IntPtr, PlayerManager.FetchSelfResponse>(PlayerManager.FetchSelfResponse.FromPointer)));
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0004FEFC File Offset: 0x0004E0FC
		[MonoPInvokeCallback(typeof(PlayerManager.FetchSelfCallback))]
		private static void InternalFetchSelfCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("PlayerManager#InternalFetchSelfCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0004FF0C File Offset: 0x0004E10C
		internal void FetchList(string[] userIds, Action<NativePlayer[]> callback)
		{
			PlayerManager.FetchResponseCollector coll = new PlayerManager.FetchResponseCollector();
			coll.pendingCount = userIds.Length;
			coll.callback = callback;
			foreach (string player_id in userIds)
			{
				PlayerManager.PlayerManager_Fetch(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, player_id, new PlayerManager.FetchCallback(PlayerManager.InternalFetchCallback), Callbacks.ToIntPtr<PlayerManager.FetchResponse>(delegate(PlayerManager.FetchResponse rsp)
				{
					this.HandleFetchResponse(coll, rsp);
				}, new Func<IntPtr, PlayerManager.FetchResponse>(PlayerManager.FetchResponse.FromPointer)));
			}
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0004FFA0 File Offset: 0x0004E1A0
		[MonoPInvokeCallback(typeof(PlayerManager.FetchCallback))]
		private static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("PlayerManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0004FFB0 File Offset: 0x0004E1B0
		internal void HandleFetchResponse(PlayerManager.FetchResponseCollector collector, PlayerManager.FetchResponse resp)
		{
			if (resp.Status() == CommonErrorStatus.ResponseStatus.VALID || resp.Status() == CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				NativePlayer player = resp.GetPlayer();
				collector.results.Add(player);
			}
			collector.pendingCount--;
			if (collector.pendingCount == 0)
			{
				collector.callback(collector.results.ToArray());
			}
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00050018 File Offset: 0x0004E218
		internal void FetchFriends(Action<ResponseStatus, List<GooglePlayGames.BasicApi.Multiplayer.Player>> callback)
		{
			PlayerManager.PlayerManager_FetchConnected(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new PlayerManager.FetchListCallback(PlayerManager.InternalFetchConnectedCallback), Callbacks.ToIntPtr<PlayerManager.FetchListResponse>(delegate(PlayerManager.FetchListResponse rsp)
			{
				this.HandleFetchCollected(rsp, callback);
			}, new Func<IntPtr, PlayerManager.FetchListResponse>(PlayerManager.FetchListResponse.FromPointer)));
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00050074 File Offset: 0x0004E274
		[MonoPInvokeCallback(typeof(PlayerManager.FetchListCallback))]
		private static void InternalFetchConnectedCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("PlayerManager#InternalFetchConnectedCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00050084 File Offset: 0x0004E284
		internal void HandleFetchCollected(PlayerManager.FetchListResponse rsp, Action<ResponseStatus, List<GooglePlayGames.BasicApi.Multiplayer.Player>> callback)
		{
			List<GooglePlayGames.BasicApi.Multiplayer.Player> list = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
			if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID || rsp.Status() == CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				Logger.d("Got " + rsp.Length().ToUInt64() + " players");
				foreach (NativePlayer nativePlayer in rsp)
				{
					list.Add(nativePlayer.AsPlayer());
				}
			}
			callback((ResponseStatus)rsp.Status(), list);
		}

		// Token: 0x04000C09 RID: 3081
		private readonly GameServices mGameServices;

		// Token: 0x02000264 RID: 612
		internal class FetchListResponse : BaseReferenceHolder, IEnumerable, IEnumerable<NativePlayer>
		{
			// Token: 0x060013A5 RID: 5029 RVA: 0x0005013C File Offset: 0x0004E33C
			internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013A6 RID: 5030 RVA: 0x00050148 File Offset: 0x0004E348
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060013A7 RID: 5031 RVA: 0x00050150 File Offset: 0x0004E350
			protected override void CallDispose(HandleRef selfPointer)
			{
				PlayerManager.PlayerManager_FetchListResponse_Dispose(base.SelfPtr());
			}

			// Token: 0x060013A8 RID: 5032 RVA: 0x00050160 File Offset: 0x0004E360
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return PlayerManager.PlayerManager_FetchListResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013A9 RID: 5033 RVA: 0x00050170 File Offset: 0x0004E370
			public IEnumerator<NativePlayer> GetEnumerator()
			{
				return PInvokeUtilities.ToEnumerator<NativePlayer>(this.Length(), (UIntPtr index) => this.GetElement(index));
			}

			// Token: 0x060013AA RID: 5034 RVA: 0x0005018C File Offset: 0x0004E38C
			internal UIntPtr Length()
			{
				return PlayerManager.PlayerManager_FetchListResponse_GetData_Length(base.SelfPtr());
			}

			// Token: 0x060013AB RID: 5035 RVA: 0x0005019C File Offset: 0x0004E39C
			internal NativePlayer GetElement(UIntPtr index)
			{
				if (index.ToUInt64() >= this.Length().ToUInt64())
				{
					throw new ArgumentOutOfRangeException();
				}
				return new NativePlayer(PlayerManager.PlayerManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index));
			}

			// Token: 0x060013AC RID: 5036 RVA: 0x000501DC File Offset: 0x0004E3DC
			internal static PlayerManager.FetchListResponse FromPointer(IntPtr selfPointer)
			{
				if (PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new PlayerManager.FetchListResponse(selfPointer);
			}
		}

		// Token: 0x02000265 RID: 613
		internal class FetchResponseCollector
		{
			// Token: 0x04000C0A RID: 3082
			internal int pendingCount;

			// Token: 0x04000C0B RID: 3083
			internal List<NativePlayer> results = new List<NativePlayer>();

			// Token: 0x04000C0C RID: 3084
			internal Action<NativePlayer[]> callback;
		}

		// Token: 0x02000266 RID: 614
		internal class FetchResponse : BaseReferenceHolder
		{
			// Token: 0x060013AF RID: 5039 RVA: 0x00050214 File Offset: 0x0004E414
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013B0 RID: 5040 RVA: 0x00050220 File Offset: 0x0004E420
			protected override void CallDispose(HandleRef selfPointer)
			{
				PlayerManager.PlayerManager_FetchResponse_Dispose(base.SelfPtr());
			}

			// Token: 0x060013B1 RID: 5041 RVA: 0x00050230 File Offset: 0x0004E430
			internal NativePlayer GetPlayer()
			{
				return new NativePlayer(PlayerManager.PlayerManager_FetchResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x060013B2 RID: 5042 RVA: 0x00050244 File Offset: 0x0004E444
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return PlayerManager.PlayerManager_FetchResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013B3 RID: 5043 RVA: 0x00050254 File Offset: 0x0004E454
			internal static PlayerManager.FetchResponse FromPointer(IntPtr selfPointer)
			{
				if (PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new PlayerManager.FetchResponse(selfPointer);
			}
		}

		// Token: 0x02000267 RID: 615
		internal class FetchSelfResponse : BaseReferenceHolder
		{
			// Token: 0x060013B4 RID: 5044 RVA: 0x0005026C File Offset: 0x0004E46C
			internal FetchSelfResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013B5 RID: 5045 RVA: 0x00050278 File Offset: 0x0004E478
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return PlayerManager.PlayerManager_FetchSelfResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013B6 RID: 5046 RVA: 0x00050288 File Offset: 0x0004E488
			internal NativePlayer Self()
			{
				return new NativePlayer(PlayerManager.PlayerManager_FetchSelfResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x060013B7 RID: 5047 RVA: 0x0005029C File Offset: 0x0004E49C
			protected override void CallDispose(HandleRef selfPointer)
			{
				PlayerManager.PlayerManager_FetchSelfResponse_Dispose(base.SelfPtr());
			}

			// Token: 0x060013B8 RID: 5048 RVA: 0x000502AC File Offset: 0x0004E4AC
			internal static PlayerManager.FetchSelfResponse FromPointer(IntPtr selfPointer)
			{
				if (PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new PlayerManager.FetchSelfResponse(selfPointer);
			}
		}
	}
}
