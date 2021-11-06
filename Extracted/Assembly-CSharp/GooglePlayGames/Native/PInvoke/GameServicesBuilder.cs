using System;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200023E RID: 574
	internal class GameServicesBuilder : BaseReferenceHolder
	{
		// Token: 0x06001222 RID: 4642 RVA: 0x0004D010 File Offset: 0x0004B210
		private GameServicesBuilder(IntPtr selfPointer) : base(selfPointer)
		{
			InternalHooks.InternalHooks_ConfigureForUnityPlugin(base.SelfPtr());
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0004D024 File Offset: 0x0004B224
		internal void SetOnAuthFinishedCallback(GameServicesBuilder.AuthFinishedCallback callback)
		{
			Builder.GameServices_Builder_SetOnAuthActionFinished(base.SelfPtr(), new Builder.OnAuthActionFinishedCallback(GameServicesBuilder.InternalAuthFinishedCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0004D044 File Offset: 0x0004B244
		internal void EnableSnapshots()
		{
			Builder.GameServices_Builder_EnableSnapshots(base.SelfPtr());
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0004D054 File Offset: 0x0004B254
		internal void RequireGooglePlus()
		{
			Builder.GameServices_Builder_RequireGooglePlus(base.SelfPtr());
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0004D064 File Offset: 0x0004B264
		internal void AddOauthScope(string scope)
		{
			Builder.GameServices_Builder_AddOauthScope(base.SelfPtr(), scope);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0004D074 File Offset: 0x0004B274
		[MonoPInvokeCallback(typeof(Builder.OnAuthActionFinishedCallback))]
		private static void InternalAuthFinishedCallback(Types.AuthOperation op, CommonErrorStatus.AuthStatus status, IntPtr data)
		{
			GameServicesBuilder.AuthFinishedCallback authFinishedCallback = Callbacks.IntPtrToPermanentCallback<GameServicesBuilder.AuthFinishedCallback>(data);
			if (authFinishedCallback == null)
			{
				return;
			}
			try
			{
				authFinishedCallback(op, status);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing InternalAuthFinishedCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0004D0D0 File Offset: 0x0004B2D0
		internal void SetOnAuthStartedCallback(GameServicesBuilder.AuthStartedCallback callback)
		{
			Builder.GameServices_Builder_SetOnAuthActionStarted(base.SelfPtr(), new Builder.OnAuthActionStartedCallback(GameServicesBuilder.InternalAuthStartedCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0004D0F0 File Offset: 0x0004B2F0
		[MonoPInvokeCallback(typeof(Builder.OnAuthActionStartedCallback))]
		private static void InternalAuthStartedCallback(Types.AuthOperation op, IntPtr data)
		{
			GameServicesBuilder.AuthStartedCallback authStartedCallback = Callbacks.IntPtrToPermanentCallback<GameServicesBuilder.AuthStartedCallback>(data);
			try
			{
				if (authStartedCallback != null)
				{
					authStartedCallback(op);
				}
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing InternalAuthStartedCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0004D148 File Offset: 0x0004B348
		protected override void CallDispose(HandleRef selfPointer)
		{
			Builder.GameServices_Builder_Dispose(selfPointer);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0004D150 File Offset: 0x0004B350
		[MonoPInvokeCallback(typeof(Builder.OnTurnBasedMatchEventCallback))]
		private static void InternalOnTurnBasedMatchEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
		{
			Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> action = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch>>(userData);
			using (NativeTurnBasedMatch nativeTurnBasedMatch = NativeTurnBasedMatch.FromPointer(match))
			{
				try
				{
					if (action != null)
					{
						action(eventType, matchId, nativeTurnBasedMatch);
					}
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing InternalOnTurnBasedMatchEventCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0004D1DC File Offset: 0x0004B3DC
		internal void SetOnTurnBasedMatchEventCallback(Action<Types.MultiplayerEvent, string, NativeTurnBasedMatch> callback)
		{
			IntPtr callback_arg = Callbacks.ToIntPtr(callback);
			Builder.GameServices_Builder_SetOnTurnBasedMatchEvent(base.SelfPtr(), new Builder.OnTurnBasedMatchEventCallback(GameServicesBuilder.InternalOnTurnBasedMatchEventCallback), callback_arg);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0004D208 File Offset: 0x0004B408
		[MonoPInvokeCallback(typeof(Builder.OnMultiplayerInvitationEventCallback))]
		private static void InternalOnMultiplayerInvitationEventCallback(Types.MultiplayerEvent eventType, string matchId, IntPtr match, IntPtr userData)
		{
			Action<Types.MultiplayerEvent, string, MultiplayerInvitation> action = Callbacks.IntPtrToPermanentCallback<Action<Types.MultiplayerEvent, string, MultiplayerInvitation>>(userData);
			using (MultiplayerInvitation multiplayerInvitation = MultiplayerInvitation.FromPointer(match))
			{
				try
				{
					if (action != null)
					{
						action(eventType, matchId, multiplayerInvitation);
					}
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing InternalOnMultiplayerInvitationEventCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0004D294 File Offset: 0x0004B494
		internal void SetOnMultiplayerInvitationEventCallback(Action<Types.MultiplayerEvent, string, MultiplayerInvitation> callback)
		{
			IntPtr callback_arg = Callbacks.ToIntPtr(callback);
			Builder.GameServices_Builder_SetOnMultiplayerInvitationEvent(base.SelfPtr(), new Builder.OnMultiplayerInvitationEventCallback(GameServicesBuilder.InternalOnMultiplayerInvitationEventCallback), callback_arg);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0004D2C0 File Offset: 0x0004B4C0
		internal GameServices Build(PlatformConfiguration configRef)
		{
			IntPtr selfPointer = Builder.GameServices_Builder_Create(base.SelfPtr(), HandleRef.ToIntPtr(configRef.AsHandle()));
			if (selfPointer.Equals(IntPtr.Zero))
			{
				throw new InvalidOperationException("There was an error creating a GameServices object. Check for log errors from GamesNativeSDK");
			}
			return new GameServices(selfPointer);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0004D30C File Offset: 0x0004B50C
		internal static GameServicesBuilder Create()
		{
			return new GameServicesBuilder(Builder.GameServices_Builder_Construct());
		}

		// Token: 0x020008DF RID: 2271
		// (Invoke) Token: 0x0600501C RID: 20508
		internal delegate void AuthFinishedCallback(Types.AuthOperation operation, CommonErrorStatus.AuthStatus status);

		// Token: 0x020008E0 RID: 2272
		// (Invoke) Token: 0x06005020 RID: 20512
		internal delegate void AuthStartedCallback(Types.AuthOperation operation);
	}
}
