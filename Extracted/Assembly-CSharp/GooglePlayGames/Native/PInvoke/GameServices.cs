using System;
using System.Runtime.InteropServices;
using System.Text;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200023C RID: 572
	internal class GameServices : BaseReferenceHolder
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x0004CEE4 File Offset: 0x0004B0E4
		internal GameServices(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0004CEF0 File Offset: 0x0004B0F0
		internal bool IsAuthenticated()
		{
			return GameServices.GameServices_IsAuthorized(base.SelfPtr());
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0004CF00 File Offset: 0x0004B100
		internal void SignOut()
		{
			GameServices.GameServices_SignOut(base.SelfPtr());
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0004CF10 File Offset: 0x0004B110
		internal void StartAuthorizationUI()
		{
			GameServices.GameServices_StartAuthorizationUI(base.SelfPtr());
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0004CF20 File Offset: 0x0004B120
		public AchievementManager AchievementManager()
		{
			return new AchievementManager(this);
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0004CF28 File Offset: 0x0004B128
		public LeaderboardManager LeaderboardManager()
		{
			return new LeaderboardManager(this);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0004CF30 File Offset: 0x0004B130
		public PlayerManager PlayerManager()
		{
			return new PlayerManager(this);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0004CF38 File Offset: 0x0004B138
		public StatsManager StatsManager()
		{
			return new StatsManager(this);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0004CF40 File Offset: 0x0004B140
		internal HandleRef AsHandle()
		{
			return base.SelfPtr();
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0004CF48 File Offset: 0x0004B148
		protected override void CallDispose(HandleRef selfPointer)
		{
			GameServices.GameServices_Dispose(selfPointer);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0004CF50 File Offset: 0x0004B150
		internal void FetchServerAuthCode(string server_client_id, Action<GameServices.FetchServerAuthCodeResponse> callback)
		{
			Misc.CheckNotNull<Action<GameServices.FetchServerAuthCodeResponse>>(callback);
			Misc.CheckNotNull<string>(server_client_id);
			GameServices.GameServices_FetchServerAuthCode(this.AsHandle(), server_client_id, new GameServices.FetchServerAuthCodeCallback(GameServices.InternalFetchServerAuthCodeCallback), Callbacks.ToIntPtr<GameServices.FetchServerAuthCodeResponse>(callback, new Func<IntPtr, GameServices.FetchServerAuthCodeResponse>(GameServices.FetchServerAuthCodeResponse.FromPointer)));
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0004CF98 File Offset: 0x0004B198
		[MonoPInvokeCallback(typeof(GameServices.FetchServerAuthCodeCallback))]
		private static void InternalFetchServerAuthCodeCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("GameServices#InternalFetchServerAuthCodeCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0200023D RID: 573
		internal class FetchServerAuthCodeResponse : BaseReferenceHolder
		{
			// Token: 0x0600121C RID: 4636 RVA: 0x0004CFA8 File Offset: 0x0004B1A8
			internal FetchServerAuthCodeResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x0600121D RID: 4637 RVA: 0x0004CFB4 File Offset: 0x0004B1B4
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return GameServices.GameServices_FetchServerAuthCodeResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x0600121E RID: 4638 RVA: 0x0004CFC4 File Offset: 0x0004B1C4
			internal string Code()
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GameServices.GameServices_FetchServerAuthCodeResponse_GetCode(base.SelfPtr(), out_string, out_size));
			}

			// Token: 0x0600121F RID: 4639 RVA: 0x0004CFD8 File Offset: 0x0004B1D8
			protected override void CallDispose(HandleRef selfPointer)
			{
				GameServices.GameServices_FetchServerAuthCodeResponse_Dispose(selfPointer);
			}

			// Token: 0x06001220 RID: 4640 RVA: 0x0004CFE0 File Offset: 0x0004B1E0
			internal static GameServices.FetchServerAuthCodeResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new GameServices.FetchServerAuthCodeResponse(pointer);
			}
		}
	}
}
