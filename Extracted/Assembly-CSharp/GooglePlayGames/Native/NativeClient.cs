using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.Native
{
	// Token: 0x02000219 RID: 537
	public class NativeClient : IPlayGamesClient
	{
		// Token: 0x0600109C RID: 4252 RVA: 0x00047210 File Offset: 0x00045410
		internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
		{
			PlayGamesHelperObject.CreateObject();
			this.mConfiguration = Misc.CheckNotNull<PlayGamesClientConfiguration>(configuration);
			this.clientImpl = clientImpl;
			this.rationale = configuration.PermissionRationale;
			if (string.IsNullOrEmpty(this.rationale))
			{
				this.rationale = "Select email address to send to this game or hit cancel to not share.";
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00047284 File Offset: 0x00045484
		private GooglePlayGames.Native.PInvoke.GameServices GameServices()
		{
			object gameServicesLock = this.GameServicesLock;
			GooglePlayGames.Native.PInvoke.GameServices result;
			lock (gameServicesLock)
			{
				result = this.mServices;
			}
			return result;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x000472D4 File Offset: 0x000454D4
		public void Authenticate(Action<bool> callback, bool silent)
		{
			object authStateLock = this.AuthStateLock;
			lock (authStateLock)
			{
				if (this.mAuthState == NativeClient.AuthState.Authenticated)
				{
					NativeClient.InvokeCallbackOnGameThread<bool>(callback, true);
					return;
				}
				if (this.mSilentAuthFailed && silent)
				{
					NativeClient.InvokeCallbackOnGameThread<bool>(callback, false);
					return;
				}
				if (callback != null)
				{
					if (silent)
					{
						this.mSilentAuthCallbacks = (Action<bool>)Delegate.Combine(this.mSilentAuthCallbacks, callback);
					}
					else
					{
						this.mPendingAuthCallbacks = (Action<bool>)Delegate.Combine(this.mPendingAuthCallbacks, callback);
					}
				}
			}
			this.InitializeGameServices();
			this.friendsLoading = false;
			if (!silent)
			{
				this.GameServices().StartAuthorizationUI();
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x000473B8 File Offset: 0x000455B8
		private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
		{
			if (callback == null)
			{
				return delegate(T A_0)
				{
				};
			}
			return delegate(T result)
			{
				NativeClient.InvokeCallbackOnGameThread<T>(callback, result);
			};
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000473F8 File Offset: 0x000455F8
		private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
		{
			if (callback == null)
			{
				return;
			}
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
				callback(data);
			});
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00047438 File Offset: 0x00045638
		private void InitializeGameServices()
		{
			object gameServicesLock = this.GameServicesLock;
			lock (gameServicesLock)
			{
				if (this.mServices == null)
				{
					using (GameServicesBuilder gameServicesBuilder = GameServicesBuilder.Create())
					{
						using (PlatformConfiguration platformConfiguration = this.clientImpl.CreatePlatformConfiguration())
						{
							this.RegisterInvitationDelegate(this.mConfiguration.InvitationDelegate);
							gameServicesBuilder.SetOnAuthFinishedCallback(new GameServicesBuilder.AuthFinishedCallback(this.HandleAuthTransition));
							gameServicesBuilder.SetOnTurnBasedMatchEventCallback(delegate(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
							{
								this.mTurnBasedClient.HandleMatchEvent(eventType, matchId, match);
							});
							gameServicesBuilder.SetOnMultiplayerInvitationEventCallback(new Action<GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation>(this.HandleInvitation));
							if (this.mConfiguration.EnableSavedGames)
							{
								gameServicesBuilder.EnableSnapshots();
							}
							if (this.mConfiguration.RequireGooglePlus)
							{
								gameServicesBuilder.RequireGooglePlus();
							}
							string[] scopes = this.mConfiguration.Scopes;
							for (int i = 0; i < scopes.Length; i++)
							{
								gameServicesBuilder.AddOauthScope(scopes[i]);
							}
							Debug.Log("Building GPG services, implicitly attempts silent auth");
							this.mAuthState = NativeClient.AuthState.SilentPending;
							this.mServices = gameServicesBuilder.Build(platformConfiguration);
							this.mEventsClient = new NativeEventClient(new GooglePlayGames.Native.PInvoke.EventManager(this.mServices));
							this.mQuestsClient = new NativeQuestClient(new GooglePlayGames.Native.PInvoke.QuestManager(this.mServices));
							this.mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(this.mServices));
							this.mTurnBasedClient.RegisterMatchDelegate(this.mConfiguration.MatchDelegate);
							this.mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(this.mServices));
							if (this.mConfiguration.EnableSavedGames)
							{
								this.mSavedGameClient = new NativeSavedGameClient(new GooglePlayGames.Native.PInvoke.SnapshotManager(this.mServices));
							}
							else
							{
								this.mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
							}
							this.mAuthState = NativeClient.AuthState.SilentPending;
							this.mTokenClient = this.clientImpl.CreateTokenClient((this.mUser != null) ? this.mUser.id : null, false);
						}
					}
				}
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x000476C0 File Offset: 0x000458C0
		internal void HandleInvitation(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			Action<Invitation, bool> currentHandler = this.mInvitationDelegate;
			if (currentHandler == null)
			{
				GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
				{
					"Received ",
					eventType,
					" for invitation ",
					invitationId,
					" but no handler was registered."
				}));
				return;
			}
			if (eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
			{
				GooglePlayGames.OurUtils.Logger.d("Ignoring REMOVED for invitation " + invitationId);
				return;
			}
			bool shouldAutolaunch = eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
			Invitation invite = invitation.AsInvitation();
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				currentHandler(invite, shouldAutolaunch);
			});
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00047764 File Offset: 0x00045964
		public string GetUserEmail()
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			this.mTokenClient.SetRationale(this.rationale);
			return this.mTokenClient.GetEmail();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x000477A8 File Offset: 0x000459A8
		public void GetUserEmail(Action<CommonStatusCodes, string> callback)
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				if (callback != null)
				{
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(CommonStatusCodes.SignInRequired, null);
					});
					return;
				}
			}
			this.mTokenClient.SetRationale(this.rationale);
			this.mTokenClient.GetEmail(delegate(CommonStatusCodes status, string email)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(status, email);
				});
			});
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00047820 File Offset: 0x00045A20
		[Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public string GetAccessToken()
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			if (!GameInfo.WebClientIdInitialized())
			{
				if (this.noWebClientIdWarningCount++ % this.webclientWarningFreq == 0)
				{
					Debug.LogError("Web client ID has not been set, cannot request access token.");
					this.noWebClientIdWarningCount = this.noWebClientIdWarningCount / this.webclientWarningFreq + 1;
				}
				return null;
			}
			this.mTokenClient.SetRationale(this.rationale);
			return this.mTokenClient.GetAccessToken();
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000478AC File Offset: 0x00045AAC
		[Obsolete("Use GetServerAuthCode() then exchange it for a token")]
		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (!this.IsAuthenticated())
			{
				Debug.Log("Cannot get API client - not authenticated");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					idTokenCallback(null);
				});
			}
			if (!GameInfo.WebClientIdInitialized())
			{
				if (this.noWebClientIdWarningCount++ % this.webclientWarningFreq == 0)
				{
					Debug.LogError("Web client ID has not been set, cannot request id token.");
					this.noWebClientIdWarningCount = this.noWebClientIdWarningCount / this.webclientWarningFreq + 1;
				}
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					idTokenCallback(null);
				});
			}
			this.mTokenClient.SetRationale(this.rationale);
			this.mTokenClient.GetIdToken(string.Empty, NativeClient.AsOnGameThreadCallback<string>(idTokenCallback));
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00047974 File Offset: 0x00045B74
		public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
		{
			this.mServices.FetchServerAuthCode(serverClientId, delegate(GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse serverAuthCodeResponse)
			{
				CommonStatusCodes responseCode = ConversionUtils.ConvertResponseStatusToCommonStatus(serverAuthCodeResponse.Status());
				if (responseCode != CommonStatusCodes.Success && responseCode != CommonStatusCodes.SuccessCached)
				{
					GooglePlayGames.OurUtils.Logger.e("Error loading server auth code: " + serverAuthCodeResponse.Status().ToString());
				}
				if (callback != null)
				{
					string authCode = serverAuthCodeResponse.Code();
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(responseCode, authCode);
					});
				}
			});
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000479A8 File Offset: 0x00045BA8
		public bool IsAuthenticated()
		{
			object authStateLock = this.AuthStateLock;
			bool result;
			lock (authStateLock)
			{
				result = (this.mAuthState == NativeClient.AuthState.Authenticated);
			}
			return result;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000479FC File Offset: 0x00045BFC
		public void LoadFriends(Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(false);
				});
				return;
			}
			if (this.mFriends != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(true);
				});
				return;
			}
			this.mServices.PlayerManager().FetchFriends(delegate(ResponseStatus status, List<GooglePlayGames.BasicApi.Multiplayer.Player> players)
			{
				if (status == ResponseStatus.Success || status == ResponseStatus.SuccessWithStale)
				{
					this.mFriends = players;
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(true);
					});
				}
				else
				{
					this.mFriends = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
					GooglePlayGames.OurUtils.Logger.e("Got " + status + " loading friends");
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						callback(false);
					});
				}
			});
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00047A80 File Offset: 0x00045C80
		public IUserProfile[] GetFriends()
		{
			if (this.mFriends == null && !this.friendsLoading)
			{
				GooglePlayGames.OurUtils.Logger.w("Getting friends before they are loaded!!!");
				this.friendsLoading = true;
				this.LoadFriends(delegate(bool ok)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
					{
						"loading: ",
						ok,
						" mFriends = ",
						this.mFriends
					}));
					if (!ok)
					{
						GooglePlayGames.OurUtils.Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
					}
					this.friendsLoading = !ok;
				});
			}
			return (this.mFriends != null) ? this.mFriends.ToArray() : new IUserProfile[0];
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00047AF4 File Offset: 0x00045CF4
		private void PopulateAchievements(uint authGeneration, GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
		{
			if (authGeneration != this.mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received achievement callback after signout occurred, ignoring");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("Populating Achievements, status = " + response.Status());
			object authStateLock = this.AuthStateLock;
			lock (authStateLock)
			{
				if (response.Status() != CommonErrorStatus.ResponseStatus.VALID && response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
					Action<bool> action = this.mPendingAuthCallbacks;
					this.mPendingAuthCallbacks = null;
					if (action != null)
					{
						NativeClient.InvokeCallbackOnGameThread<bool>(action, false);
					}
					this.SignOut();
					return;
				}
				Dictionary<string, GooglePlayGames.BasicApi.Achievement> dictionary = new Dictionary<string, GooglePlayGames.BasicApi.Achievement>();
				foreach (NativeAchievement nativeAchievement in response)
				{
					using (nativeAchievement)
					{
						dictionary[nativeAchievement.Id()] = nativeAchievement.AsAchievement();
					}
				}
				GooglePlayGames.OurUtils.Logger.d("Found " + dictionary.Count + " Achievements");
				this.mAchievements = dictionary;
			}
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for Achievements");
			this.MaybeFinishAuthentication();
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00047C80 File Offset: 0x00045E80
		private void MaybeFinishAuthentication()
		{
			Action<bool> action = null;
			object authStateLock = this.AuthStateLock;
			lock (authStateLock)
			{
				if (this.mUser == null || this.mAchievements == null)
				{
					GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
					{
						"Auth not finished. User=",
						this.mUser,
						" achievements=",
						this.mAchievements
					}));
					return;
				}
				GooglePlayGames.OurUtils.Logger.d("Auth finished. Proceeding.");
				action = this.mPendingAuthCallbacks;
				this.mPendingAuthCallbacks = null;
				this.mAuthState = NativeClient.AuthState.Authenticated;
			}
			if (action != null)
			{
				GooglePlayGames.OurUtils.Logger.d("Invoking Callbacks: " + action);
				NativeClient.InvokeCallbackOnGameThread<bool>(action, true);
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00047D5C File Offset: 0x00045F5C
		private void PopulateUser(uint authGeneration, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
		{
			GooglePlayGames.OurUtils.Logger.d("Populating User");
			if (authGeneration != this.mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received user callback after signout occurred, ignoring");
				return;
			}
			object authStateLock = this.AuthStateLock;
			lock (authStateLock)
			{
				if (response.Status() != CommonErrorStatus.ResponseStatus.VALID && response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving user, signing out");
					Action<bool> action = this.mPendingAuthCallbacks;
					this.mPendingAuthCallbacks = null;
					if (action != null)
					{
						NativeClient.InvokeCallbackOnGameThread<bool>(action, false);
					}
					this.SignOut();
					return;
				}
				this.mUser = response.Self().AsPlayer();
				this.mFriends = null;
				this.mTokenClient = this.clientImpl.CreateTokenClient(this.mUser.id, true);
			}
			GooglePlayGames.OurUtils.Logger.d("Found User: " + this.mUser);
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for User");
			this.MaybeFinishAuthentication();
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00047E74 File Offset: 0x00046074
		private void HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, CommonErrorStatus.AuthStatus status)
		{
			GooglePlayGames.OurUtils.Logger.d(string.Concat(new object[]
			{
				"Starting Auth Transition. Op: ",
				operation,
				" status: ",
				status
			}));
			object authStateLock = this.AuthStateLock;
			lock (authStateLock)
			{
				if (operation != GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN)
				{
					if (operation != GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT)
					{
						GooglePlayGames.OurUtils.Logger.e("Unknown AuthOperation " + operation);
					}
					else
					{
						this.ToUnauthenticated();
					}
				}
				else if (status == CommonErrorStatus.AuthStatus.VALID)
				{
					if (this.mSilentAuthCallbacks != null)
					{
						this.mPendingAuthCallbacks = (Action<bool>)Delegate.Combine(this.mPendingAuthCallbacks, this.mSilentAuthCallbacks);
						this.mSilentAuthCallbacks = null;
					}
					uint currentAuthGeneration = this.mAuthGeneration;
					this.mServices.AchievementManager().FetchAll(delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results)
					{
						this.PopulateAchievements(currentAuthGeneration, results);
					});
					this.mServices.PlayerManager().FetchSelf(delegate(GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results)
					{
						this.PopulateUser(currentAuthGeneration, results);
					});
				}
				else if (this.mAuthState == NativeClient.AuthState.SilentPending)
				{
					this.mSilentAuthFailed = true;
					this.mAuthState = NativeClient.AuthState.Unauthenticated;
					Action<bool> callback = this.mSilentAuthCallbacks;
					this.mSilentAuthCallbacks = null;
					GooglePlayGames.OurUtils.Logger.d("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
					NativeClient.InvokeCallbackOnGameThread<bool>(callback, false);
					if (this.mPendingAuthCallbacks != null)
					{
						GooglePlayGames.OurUtils.Logger.d("there are pending auth callbacks - starting AuthUI");
						this.GameServices().StartAuthorizationUI();
					}
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.d("AuthState == " + this.mAuthState + " calling auth callbacks with failure");
					this.UnpauseUnityPlayer();
					Action<bool> callback2 = this.mPendingAuthCallbacks;
					this.mPendingAuthCallbacks = null;
					NativeClient.InvokeCallbackOnGameThread<bool>(callback2, false);
				}
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00048070 File Offset: 0x00046270
		private void UnpauseUnityPlayer()
		{
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00048074 File Offset: 0x00046274
		private void ToUnauthenticated()
		{
			object authStateLock = this.AuthStateLock;
			lock (authStateLock)
			{
				this.mUser = null;
				this.mFriends = null;
				this.mAchievements = null;
				this.mAuthState = NativeClient.AuthState.Unauthenticated;
				this.mTokenClient = this.clientImpl.CreateTokenClient(null, true);
				this.mAuthGeneration += 1U;
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00048104 File Offset: 0x00046304
		public void SignOut()
		{
			this.ToUnauthenticated();
			if (this.GameServices() == null)
			{
				return;
			}
			this.GameServices().SignOut();
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00048124 File Offset: 0x00046324
		public string GetUserId()
		{
			if (this.mUser == null)
			{
				return null;
			}
			return this.mUser.id;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00048144 File Offset: 0x00046344
		public string GetUserDisplayName()
		{
			if (this.mUser == null)
			{
				return null;
			}
			return this.mUser.userName;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00048164 File Offset: 0x00046364
		public string GetUserImageUrl()
		{
			if (this.mUser == null)
			{
				return null;
			}
			return this.mUser.AvatarURL;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00048184 File Offset: 0x00046384
		public void GetPlayerStats(Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				this.clientImpl.GetPlayerStats(this.GetApiClient(), callback);
			});
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000481B8 File Offset: 0x000463B8
		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			this.mServices.PlayerManager().FetchList(userIds, delegate(NativePlayer[] nativeUsers)
			{
				IUserProfile[] users = new IUserProfile[nativeUsers.Length];
				for (int i = 0; i < users.Length; i++)
				{
					users[i] = nativeUsers[i].AsPlayer();
				}
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(users);
				});
			});
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x000481F0 File Offset: 0x000463F0
		public GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
		{
			if (this.mAchievements == null || !this.mAchievements.ContainsKey(achId))
			{
				return null;
			}
			return this.mAchievements[achId];
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00048230 File Offset: 0x00046430
		public void LoadAchievements(Action<GooglePlayGames.BasicApi.Achievement[]> callback)
		{
			GooglePlayGames.BasicApi.Achievement[] data = new GooglePlayGames.BasicApi.Achievement[this.mAchievements.Count];
			this.mAchievements.Values.CopyTo(data, 0);
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				callback(data);
			});
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0004828C File Offset: 0x0004648C
		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			this.UpdateAchievement("Unlock", achId, callback, (GooglePlayGames.BasicApi.Achievement a) => a.IsUnlocked, delegate(GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsUnlocked = true;
				this.GameServices().AchievementManager().Unlock(achId);
			});
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x000482E8 File Offset: 0x000464E8
		public void RevealAchievement(string achId, Action<bool> callback)
		{
			this.UpdateAchievement("Reveal", achId, callback, (GooglePlayGames.BasicApi.Achievement a) => a.IsRevealed, delegate(GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsRevealed = true;
				this.GameServices().AchievementManager().Reveal(achId);
			});
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00048344 File Offset: 0x00046544
		private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<GooglePlayGames.BasicApi.Achievement> alreadyDone, Action<GooglePlayGames.BasicApi.Achievement> updateAchievment)
		{
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			Misc.CheckNotNull<string>(achId);
			this.InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Could not " + updateType + ", no achievement with ID " + achId);
				callback(false);
				return;
			}
			if (alreadyDone(achievement))
			{
				GooglePlayGames.OurUtils.Logger.d("Did not need to perform " + updateType + ": on achievement " + achId);
				callback(true);
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("Performing " + updateType + " on " + achId);
			updateAchievment(achievement);
			this.GameServices().AchievementManager().Fetch(achId, delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
				{
					this.mAchievements.Remove(achId);
					this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[]
					{
						"Cannot refresh achievement ",
						achId,
						": ",
						rsp.Status()
					}));
					callback(false);
				}
			});
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00048444 File Offset: 0x00046644
		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			Misc.CheckNotNull<string>(achId);
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			this.InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + achId);
				callback(false);
				return;
			}
			if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + achId + " was not incremental");
				callback(false);
				return;
			}
			if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				callback(false);
				return;
			}
			this.GameServices().AchievementManager().Increment(achId, Convert.ToUInt32(steps));
			this.GameServices().AchievementManager().Fetch(achId, delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
				{
					this.mAchievements.Remove(achId);
					this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[]
					{
						"Cannot refresh achievement ",
						achId,
						": ",
						rsp.Status()
					}));
					callback(false);
				}
			});
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00048554 File Offset: 0x00046754
		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			Misc.CheckNotNull<string>(achId);
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			this.InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + achId);
				callback(false);
				return;
			}
			if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + achId + " is not incremental");
				callback(false);
				return;
			}
			if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				callback(false);
				return;
			}
			this.GameServices().AchievementManager().SetStepsAtLeast(achId, Convert.ToUInt32(steps));
			this.GameServices().AchievementManager().Fetch(achId, delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
			{
				if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
				{
					this.mAchievements.Remove(achId);
					this.mAchievements.Add(achId, rsp.Achievement().AsAchievement());
					callback(true);
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.e(string.Concat(new object[]
					{
						"Cannot refresh achievement ",
						achId,
						": ",
						rsp.Status()
					}));
					callback(false);
				}
			});
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00048664 File Offset: 0x00046864
		public void ShowAchievementsUI(Action<UIStatus> cb)
		{
			if (!this.IsAuthenticated())
			{
				return;
			}
			Action<CommonErrorStatus.UIStatus> callback = Callbacks.NoopUICallback;
			if (cb != null)
			{
				callback = delegate(CommonErrorStatus.UIStatus result)
				{
					cb((UIStatus)result);
				};
			}
			callback = NativeClient.AsOnGameThreadCallback<CommonErrorStatus.UIStatus>(callback);
			this.GameServices().AchievementManager().ShowAllUI(callback);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000486C0 File Offset: 0x000468C0
		public int LeaderboardMaxResults()
		{
			return this.GameServices().LeaderboardManager().LeaderboardMaxResults;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x000486D4 File Offset: 0x000468D4
		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> cb)
		{
			if (!this.IsAuthenticated())
			{
				return;
			}
			Action<CommonErrorStatus.UIStatus> callback = Callbacks.NoopUICallback;
			if (cb != null)
			{
				callback = delegate(CommonErrorStatus.UIStatus result)
				{
					cb((UIStatus)result);
				};
			}
			callback = NativeClient.AsOnGameThreadCallback<CommonErrorStatus.UIStatus>(callback);
			if (leaderboardId == null)
			{
				this.GameServices().LeaderboardManager().ShowAllUI(callback);
			}
			else
			{
				this.GameServices().LeaderboardManager().ShowUI(leaderboardId, span, callback);
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00048750 File Offset: 0x00046950
		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<LeaderboardScoreData>(callback);
			this.GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, this.mUser.id, callback);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0004878C File Offset: 0x0004698C
		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<LeaderboardScoreData>(callback);
			this.GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x000487B8 File Offset: 0x000469B8
		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			if (!this.IsAuthenticated())
			{
				callback(false);
			}
			this.InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
			callback(true);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00048810 File Offset: 0x00046A10
		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			callback = NativeClient.AsOnGameThreadCallback<bool>(callback);
			if (!this.IsAuthenticated())
			{
				callback(false);
			}
			this.InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
			callback(true);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0004886C File Offset: 0x00046A6C
		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			if (!this.IsAuthenticated())
			{
				return null;
			}
			object gameServicesLock = this.GameServicesLock;
			IRealTimeMultiplayerClient result;
			lock (gameServicesLock)
			{
				result = this.mRealTimeClient;
			}
			return result;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000488CC File Offset: 0x00046ACC
		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			object gameServicesLock = this.GameServicesLock;
			ITurnBasedMultiplayerClient result;
			lock (gameServicesLock)
			{
				result = this.mTurnBasedClient;
			}
			return result;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00048920 File Offset: 0x00046B20
		public ISavedGameClient GetSavedGameClient()
		{
			object gameServicesLock = this.GameServicesLock;
			ISavedGameClient result;
			lock (gameServicesLock)
			{
				result = this.mSavedGameClient;
			}
			return result;
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00048974 File Offset: 0x00046B74
		public IEventsClient GetEventsClient()
		{
			object gameServicesLock = this.GameServicesLock;
			IEventsClient result;
			lock (gameServicesLock)
			{
				result = this.mEventsClient;
			}
			return result;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x000489C8 File Offset: 0x00046BC8
		public IQuestsClient GetQuestsClient()
		{
			object gameServicesLock = this.GameServicesLock;
			IQuestsClient result;
			lock (gameServicesLock)
			{
				result = this.mQuestsClient;
			}
			return result;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00048A1C File Offset: 0x00046C1C
		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			if (invitationDelegate == null)
			{
				this.mInvitationDelegate = null;
			}
			else
			{
				this.mInvitationDelegate = Callbacks.AsOnGameThreadCallback<Invitation, bool>(delegate(Invitation invitation, bool autoAccept)
				{
					invitationDelegate(invitation, autoAccept);
				});
			}
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00048A68 File Offset: 0x00046C68
		public string GetToken()
		{
			if (this.mTokenClient != null)
			{
				return this.mTokenClient.GetAccessToken();
			}
			return null;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00048A88 File Offset: 0x00046C88
		public IntPtr GetApiClient()
		{
			return InternalHooks.InternalHooks_GetApiClient(this.mServices.AsHandle());
		}

		// Token: 0x04000B97 RID: 2967
		private readonly IClientImpl clientImpl;

		// Token: 0x04000B98 RID: 2968
		private readonly object GameServicesLock = new object();

		// Token: 0x04000B99 RID: 2969
		private readonly object AuthStateLock = new object();

		// Token: 0x04000B9A RID: 2970
		private readonly PlayGamesClientConfiguration mConfiguration;

		// Token: 0x04000B9B RID: 2971
		private GooglePlayGames.Native.PInvoke.GameServices mServices;

		// Token: 0x04000B9C RID: 2972
		private volatile NativeTurnBasedMultiplayerClient mTurnBasedClient;

		// Token: 0x04000B9D RID: 2973
		private volatile NativeRealtimeMultiplayerClient mRealTimeClient;

		// Token: 0x04000B9E RID: 2974
		private volatile ISavedGameClient mSavedGameClient;

		// Token: 0x04000B9F RID: 2975
		private volatile IEventsClient mEventsClient;

		// Token: 0x04000BA0 RID: 2976
		private volatile IQuestsClient mQuestsClient;

		// Token: 0x04000BA1 RID: 2977
		private volatile TokenClient mTokenClient;

		// Token: 0x04000BA2 RID: 2978
		private volatile Action<Invitation, bool> mInvitationDelegate;

		// Token: 0x04000BA3 RID: 2979
		private volatile Dictionary<string, GooglePlayGames.BasicApi.Achievement> mAchievements;

		// Token: 0x04000BA4 RID: 2980
		private volatile GooglePlayGames.BasicApi.Multiplayer.Player mUser;

		// Token: 0x04000BA5 RID: 2981
		private volatile List<GooglePlayGames.BasicApi.Multiplayer.Player> mFriends;

		// Token: 0x04000BA6 RID: 2982
		private volatile Action<bool> mPendingAuthCallbacks;

		// Token: 0x04000BA7 RID: 2983
		private volatile Action<bool> mSilentAuthCallbacks;

		// Token: 0x04000BA8 RID: 2984
		private volatile NativeClient.AuthState mAuthState;

		// Token: 0x04000BA9 RID: 2985
		private volatile uint mAuthGeneration;

		// Token: 0x04000BAA RID: 2986
		private volatile bool mSilentAuthFailed;

		// Token: 0x04000BAB RID: 2987
		private volatile bool friendsLoading;

		// Token: 0x04000BAC RID: 2988
		private string rationale;

		// Token: 0x04000BAD RID: 2989
		private int webclientWarningFreq = 100000;

		// Token: 0x04000BAE RID: 2990
		private int noWebClientIdWarningCount;

		// Token: 0x0200021A RID: 538
		private enum AuthState
		{
			// Token: 0x04000BB2 RID: 2994
			Unauthenticated,
			// Token: 0x04000BB3 RID: 2995
			Authenticated,
			// Token: 0x04000BB4 RID: 2996
			SilentPending
		}
	}
}
