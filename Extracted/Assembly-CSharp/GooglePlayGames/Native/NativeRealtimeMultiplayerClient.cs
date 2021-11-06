using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x02000221 RID: 545
	public class NativeRealtimeMultiplayerClient : IRealTimeMultiplayerClient
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x00049728 File Offset: 0x00047928
		internal NativeRealtimeMultiplayerClient(NativeClient nativeClient, RealtimeManager manager)
		{
			this.mNativeClient = Misc.CheckNotNull<NativeClient>(nativeClient);
			this.mRealtimeManager = Misc.CheckNotNull<RealtimeManager>(manager);
			this.mCurrentSession = this.GetTerminatedSession();
			PlayGamesHelperObject.AddPauseCallback(new Action<bool>(this.HandleAppPausing));
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00049780 File Offset: 0x00047980
		private NativeRealtimeMultiplayerClient.RoomSession GetTerminatedSession()
		{
			NativeRealtimeMultiplayerClient.RoomSession roomSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, new NativeRealtimeMultiplayerClient.NoopListener());
			roomSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(roomSession), false);
			return roomSession;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000497AC File Offset: 0x000479AC
		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener)
		{
			this.CreateQuickGame(minOpponents, maxOpponents, variant, 0UL, listener);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000497BC File Offset: 0x000479BC
		public void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			lock (obj)
			{
				NativeRealtimeMultiplayerClient.RoomSession newSession = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (this.mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to create a new room without cleaning up the old one.");
					newSession.LeaveRoom();
				}
				else
				{
					this.mCurrentSession = newSession;
					Logger.d("QuickGame: Setting MinPlayersToStart = " + minOpponents);
					this.mCurrentSession.MinPlayersToStart = minOpponents;
					using (RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = RealtimeRoomConfigBuilder.Create())
					{
						RealtimeRoomConfig config = realtimeRoomConfigBuilder.SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetVariant(variant).SetExclusiveBitMask(exclusiveBitMask).Build();
						using (config)
						{
							using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = NativeRealtimeMultiplayerClient.HelperForSession(newSession))
							{
								newSession.StartRoomCreation(this.mNativeClient.GetUserId(), delegate
								{
									this.mRealtimeManager.CreateRoom(config, helper, new Action<RealtimeManager.RealTimeRoomResponse>(newSession.HandleRoomResponse));
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000499A0 File Offset: 0x00047BA0
		private static GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper HelperForSession(NativeRealtimeMultiplayerClient.RoomSession session)
		{
			return GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper.Create().SetOnDataReceivedCallback(delegate(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant, byte[] data, bool isReliable)
			{
				session.OnDataReceived(room, participant, data, isReliable);
			}).SetOnParticipantStatusChangedCallback(delegate(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				session.OnParticipantStatusChanged(room, participant);
			}).SetOnRoomConnectedSetChangedCallback(delegate(NativeRealTimeRoom room)
			{
				session.OnConnectedSetChanged(room);
			}).SetOnRoomStatusChangedCallback(delegate(NativeRealTimeRoom room)
			{
				session.OnRoomStatusChanged(room);
			});
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00049A04 File Offset: 0x00047C04
		private void HandleAppPausing(bool paused)
		{
			if (paused)
			{
				Logger.d("Application is pausing, which disconnects the RTMP  client.  Leaving room.");
				this.LeaveRoom();
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00049A1C File Offset: 0x00047C1C
		public void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			lock (obj)
			{
				NativeRealtimeMultiplayerClient.RoomSession newRoom = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (this.mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to create a new room without cleaning up the old one.");
					newRoom.LeaveRoom();
				}
				else
				{
					this.mCurrentSession = newRoom;
					this.mCurrentSession.ShowingUI = true;
					this.mRealtimeManager.ShowPlayerSelectUI(minOpponents, maxOppponents, true, delegate(PlayerSelectUIResponse response)
					{
						this.mCurrentSession.ShowingUI = false;
						if (response.Status() != CommonErrorStatus.UIStatus.VALID)
						{
							Logger.d("User did not complete invitation screen.");
							newRoom.LeaveRoom();
							return;
						}
						this.mCurrentSession.MinPlayersToStart = response.MinimumAutomatchingPlayers() + (uint)response.Count<string>() + 1U;
						using (RealtimeRoomConfigBuilder realtimeRoomConfigBuilder = RealtimeRoomConfigBuilder.Create())
						{
							realtimeRoomConfigBuilder.SetVariant(variant);
							realtimeRoomConfigBuilder.PopulateFromUIResponse(response);
							using (RealtimeRoomConfig config = realtimeRoomConfigBuilder.Build())
							{
								using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = NativeRealtimeMultiplayerClient.HelperForSession(newRoom))
								{
									newRoom.StartRoomCreation(this.mNativeClient.GetUserId(), delegate
									{
										this.mRealtimeManager.CreateRoom(config, helper, new Action<RealtimeManager.RealTimeRoomResponse>(newRoom.HandleRoomResponse));
									});
								}
							}
						}
					});
				}
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00049AFC File Offset: 0x00047CFC
		public void ShowWaitingRoomUI()
		{
			object obj = this.mSessionLock;
			lock (obj)
			{
				this.mCurrentSession.ShowWaitingRoomUI();
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00049B4C File Offset: 0x00047D4C
		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			this.mRealtimeManager.FetchInvitations(delegate(RealtimeManager.FetchInvitationsResponse response)
			{
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					callback(new Invitation[0]);
					return;
				}
				List<Invitation> list = new List<Invitation>();
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation multiplayerInvitation in response.Invitations())
				{
					using (multiplayerInvitation)
					{
						list.Add(multiplayerInvitation.AsInvitation());
					}
				}
				callback(list.ToArray());
			});
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00049B80 File Offset: 0x00047D80
		public void AcceptFromInbox(RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			lock (obj)
			{
				NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey297 <AcceptFromInbox>c__AnonStorey = new NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey297();
				<AcceptFromInbox>c__AnonStorey.<>f__this = this;
				<AcceptFromInbox>c__AnonStorey.newRoom = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (this.mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to accept invitation without cleaning up active session.");
					<AcceptFromInbox>c__AnonStorey.newRoom.LeaveRoom();
				}
				else
				{
					this.mCurrentSession = <AcceptFromInbox>c__AnonStorey.newRoom;
					this.mCurrentSession.ShowingUI = true;
					this.mRealtimeManager.ShowRoomInboxUI(delegate(RealtimeManager.RoomInboxUIResponse response)
					{
						NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey297.<AcceptFromInbox>c__AnonStorey298 <AcceptFromInbox>c__AnonStorey2 = new NativeRealtimeMultiplayerClient.<AcceptFromInbox>c__AnonStorey297.<AcceptFromInbox>c__AnonStorey298();
						<AcceptFromInbox>c__AnonStorey2.<>f__ref$663 = <AcceptFromInbox>c__AnonStorey;
						<AcceptFromInbox>c__AnonStorey.<>f__this.mCurrentSession.ShowingUI = false;
						if (response.ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
						{
							Logger.d("User did not complete invitation screen.");
							<AcceptFromInbox>c__AnonStorey.newRoom.LeaveRoom();
							return;
						}
						<AcceptFromInbox>c__AnonStorey2.invitation = response.Invitation();
						using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = NativeRealtimeMultiplayerClient.HelperForSession(<AcceptFromInbox>c__AnonStorey.newRoom))
						{
							Logger.d("About to accept invitation " + <AcceptFromInbox>c__AnonStorey2.invitation.Id());
							<AcceptFromInbox>c__AnonStorey.newRoom.StartRoomCreation(<AcceptFromInbox>c__AnonStorey.<>f__this.mNativeClient.GetUserId(), delegate
							{
								<AcceptFromInbox>c__AnonStorey.<>f__this.mRealtimeManager.AcceptInvitation(<AcceptFromInbox>c__AnonStorey2.invitation, helper, delegate(RealtimeManager.RealTimeRoomResponse acceptResponse)
								{
									using (<AcceptFromInbox>c__AnonStorey2.invitation)
									{
										<AcceptFromInbox>c__AnonStorey.newRoom.HandleRoomResponse(acceptResponse);
										<AcceptFromInbox>c__AnonStorey.newRoom.SetInvitation(<AcceptFromInbox>c__AnonStorey2.invitation.AsInvitation());
									}
								});
							});
						}
					});
				}
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00049C40 File Offset: 0x00047E40
		public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
		{
			object obj = this.mSessionLock;
			lock (obj)
			{
				NativeRealtimeMultiplayerClient.RoomSession newRoom = new NativeRealtimeMultiplayerClient.RoomSession(this.mRealtimeManager, listener);
				if (this.mCurrentSession.IsActive())
				{
					Logger.e("Received attempt to accept invitation without cleaning up active session.");
					newRoom.LeaveRoom();
				}
				else
				{
					this.mCurrentSession = newRoom;
					this.mRealtimeManager.FetchInvitations(delegate(RealtimeManager.FetchInvitationsResponse response)
					{
						if (!response.RequestSucceeded())
						{
							Logger.e("Couldn't load invitations.");
							newRoom.LeaveRoom();
							return;
						}
						GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation;
						foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation3 in response.Invitations())
						{
							invitation = invitation3;
							using (invitation)
							{
								if (invitation.Id().Equals(invitationId))
								{
									this.mCurrentSession.MinPlayersToStart = invitation.AutomatchingSlots() + invitation.ParticipantCount();
									Logger.d("Setting MinPlayersToStart with invitation to : " + this.mCurrentSession.MinPlayersToStart);
									using (GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper = NativeRealtimeMultiplayerClient.HelperForSession(newRoom))
									{
										newRoom.StartRoomCreation(this.mNativeClient.GetUserId(), delegate
										{
											this.mRealtimeManager.AcceptInvitation(invitation, helper, new Action<RealtimeManager.RealTimeRoomResponse>(newRoom.HandleRoomResponse));
										});
										return;
									}
								}
							}
						}
						Logger.e("Room creation failed since we could not find invitation with ID " + invitationId);
						newRoom.LeaveRoom();
					});
				}
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00049D10 File Offset: 0x00047F10
		public Invitation GetInvitation()
		{
			return this.mCurrentSession.GetInvitation();
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00049D20 File Offset: 0x00047F20
		public void LeaveRoom()
		{
			this.mCurrentSession.LeaveRoom();
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00049D30 File Offset: 0x00047F30
		public void SendMessageToAll(bool reliable, byte[] data)
		{
			this.mCurrentSession.SendMessageToAll(reliable, data);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00049D44 File Offset: 0x00047F44
		public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
		{
			this.mCurrentSession.SendMessageToAll(reliable, data, offset, length);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00049D58 File Offset: 0x00047F58
		public void SendMessage(bool reliable, string participantId, byte[] data)
		{
			this.mCurrentSession.SendMessage(reliable, participantId, data);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00049D6C File Offset: 0x00047F6C
		public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
		{
			this.mCurrentSession.SendMessage(reliable, participantId, data, offset, length);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00049D84 File Offset: 0x00047F84
		public List<Participant> GetConnectedParticipants()
		{
			return this.mCurrentSession.GetConnectedParticipants();
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00049D94 File Offset: 0x00047F94
		public Participant GetSelf()
		{
			return this.mCurrentSession.GetSelf();
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00049DA4 File Offset: 0x00047FA4
		public Participant GetParticipant(string participantId)
		{
			return this.mCurrentSession.GetParticipant(participantId);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00049DB4 File Offset: 0x00047FB4
		public bool IsRoomConnected()
		{
			return this.mCurrentSession.IsRoomConnected();
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00049DC4 File Offset: 0x00047FC4
		public void DeclineInvitation(string invitationId)
		{
			this.mRealtimeManager.FetchInvitations(delegate(RealtimeManager.FetchInvitationsResponse response)
			{
				if (!response.RequestSucceeded())
				{
					Logger.e("Couldn't load invitations.");
					return;
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation multiplayerInvitation in response.Invitations())
				{
					using (multiplayerInvitation)
					{
						if (multiplayerInvitation.Id().Equals(invitationId))
						{
							this.mRealtimeManager.DeclineInvitation(multiplayerInvitation);
						}
					}
				}
			});
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00049DFC File Offset: 0x00047FFC
		private static T WithDefault<T>(T presented, T defaultValue) where T : class
		{
			return (presented == null) ? defaultValue : presented;
		}

		// Token: 0x04000BBC RID: 3004
		private readonly object mSessionLock = new object();

		// Token: 0x04000BBD RID: 3005
		private readonly NativeClient mNativeClient;

		// Token: 0x04000BBE RID: 3006
		private readonly RealtimeManager mRealtimeManager;

		// Token: 0x04000BBF RID: 3007
		private volatile NativeRealtimeMultiplayerClient.RoomSession mCurrentSession;

		// Token: 0x02000222 RID: 546
		private class NoopListener : RealTimeMultiplayerListener
		{
			// Token: 0x0600111A RID: 4378 RVA: 0x00049E18 File Offset: 0x00048018
			public void OnRoomSetupProgress(float percent)
			{
			}

			// Token: 0x0600111B RID: 4379 RVA: 0x00049E1C File Offset: 0x0004801C
			public void OnRoomConnected(bool success)
			{
			}

			// Token: 0x0600111C RID: 4380 RVA: 0x00049E20 File Offset: 0x00048020
			public void OnLeftRoom()
			{
			}

			// Token: 0x0600111D RID: 4381 RVA: 0x00049E24 File Offset: 0x00048024
			public void OnParticipantLeft(Participant participant)
			{
			}

			// Token: 0x0600111E RID: 4382 RVA: 0x00049E28 File Offset: 0x00048028
			public void OnPeersConnected(string[] participantIds)
			{
			}

			// Token: 0x0600111F RID: 4383 RVA: 0x00049E2C File Offset: 0x0004802C
			public void OnPeersDisconnected(string[] participantIds)
			{
			}

			// Token: 0x06001120 RID: 4384 RVA: 0x00049E30 File Offset: 0x00048030
			public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
			}
		}

		// Token: 0x02000223 RID: 547
		private class RoomSession
		{
			// Token: 0x06001121 RID: 4385 RVA: 0x00049E34 File Offset: 0x00048034
			internal RoomSession(RealtimeManager manager, RealTimeMultiplayerListener listener)
			{
				this.mManager = Misc.CheckNotNull<RealtimeManager>(manager);
				this.mListener = new NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener(listener);
				this.EnterState(new NativeRealtimeMultiplayerClient.BeforeRoomCreateStartedState(this), false);
				this.mStillPreRoomCreation = true;
			}

			// Token: 0x17000239 RID: 569
			// (get) Token: 0x06001122 RID: 4386 RVA: 0x00049E80 File Offset: 0x00048080
			// (set) Token: 0x06001123 RID: 4387 RVA: 0x00049E8C File Offset: 0x0004808C
			internal bool ShowingUI
			{
				get
				{
					return this.mShowingUI;
				}
				set
				{
					this.mShowingUI = value;
				}
			}

			// Token: 0x1700023A RID: 570
			// (get) Token: 0x06001124 RID: 4388 RVA: 0x00049E98 File Offset: 0x00048098
			// (set) Token: 0x06001125 RID: 4389 RVA: 0x00049EA0 File Offset: 0x000480A0
			internal uint MinPlayersToStart
			{
				get
				{
					return this.mMinPlayersToStart;
				}
				set
				{
					this.mMinPlayersToStart = value;
				}
			}

			// Token: 0x06001126 RID: 4390 RVA: 0x00049EAC File Offset: 0x000480AC
			internal RealtimeManager Manager()
			{
				return this.mManager;
			}

			// Token: 0x06001127 RID: 4391 RVA: 0x00049EB4 File Offset: 0x000480B4
			internal bool IsActive()
			{
				return this.mState.IsActive();
			}

			// Token: 0x06001128 RID: 4392 RVA: 0x00049EC4 File Offset: 0x000480C4
			internal string SelfPlayerId()
			{
				return this.mCurrentPlayerId;
			}

			// Token: 0x06001129 RID: 4393 RVA: 0x00049ED0 File Offset: 0x000480D0
			public void SetInvitation(Invitation invitation)
			{
				this.mInvitation = invitation;
			}

			// Token: 0x0600112A RID: 4394 RVA: 0x00049EDC File Offset: 0x000480DC
			public Invitation GetInvitation()
			{
				return this.mInvitation;
			}

			// Token: 0x0600112B RID: 4395 RVA: 0x00049EE4 File Offset: 0x000480E4
			internal NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener OnGameThreadListener()
			{
				return this.mListener;
			}

			// Token: 0x0600112C RID: 4396 RVA: 0x00049EEC File Offset: 0x000480EC
			internal void EnterState(NativeRealtimeMultiplayerClient.State handler)
			{
				this.EnterState(handler, true);
			}

			// Token: 0x0600112D RID: 4397 RVA: 0x00049EF8 File Offset: 0x000480F8
			internal void EnterState(NativeRealtimeMultiplayerClient.State handler, bool fireStateEnteredEvent)
			{
				object obj = this.mLifecycleLock;
				lock (obj)
				{
					this.mState = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.State>(handler);
					if (fireStateEnteredEvent)
					{
						Logger.d("Entering state: " + handler.GetType().Name);
						this.mState.OnStateEntered();
					}
				}
			}

			// Token: 0x0600112E RID: 4398 RVA: 0x00049F78 File Offset: 0x00048178
			internal void LeaveRoom()
			{
				if (!this.ShowingUI)
				{
					object obj = this.mLifecycleLock;
					lock (obj)
					{
						this.mState.LeaveRoom();
					}
				}
				else
				{
					Logger.d("Not leaving room since showing UI");
				}
			}

			// Token: 0x0600112F RID: 4399 RVA: 0x00049FE4 File Offset: 0x000481E4
			internal void ShowWaitingRoomUI()
			{
				this.mState.ShowWaitingRoomUI(this.MinPlayersToStart);
			}

			// Token: 0x06001130 RID: 4400 RVA: 0x00049FFC File Offset: 0x000481FC
			internal void StartRoomCreation(string currentPlayerId, Action createRoom)
			{
				object obj = this.mLifecycleLock;
				lock (obj)
				{
					if (!this.mStillPreRoomCreation)
					{
						Logger.e("Room creation started more than once, this shouldn't happen!");
					}
					else if (!this.mState.IsActive())
					{
						Logger.w("Received an attempt to create a room after the session was already torn down!");
					}
					else
					{
						this.mCurrentPlayerId = Misc.CheckNotNull<string>(currentPlayerId);
						this.mStillPreRoomCreation = false;
						this.EnterState(new NativeRealtimeMultiplayerClient.RoomCreationPendingState(this));
						createRoom();
					}
				}
			}

			// Token: 0x06001131 RID: 4401 RVA: 0x0004A0A4 File Offset: 0x000482A4
			internal void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				object obj = this.mLifecycleLock;
				lock (obj)
				{
					this.mState.OnRoomStatusChanged(room);
				}
			}

			// Token: 0x06001132 RID: 4402 RVA: 0x0004A0F4 File Offset: 0x000482F4
			internal void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				object obj = this.mLifecycleLock;
				lock (obj)
				{
					this.mState.OnConnectedSetChanged(room);
				}
			}

			// Token: 0x06001133 RID: 4403 RVA: 0x0004A144 File Offset: 0x00048344
			internal void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				object obj = this.mLifecycleLock;
				lock (obj)
				{
					this.mState.OnParticipantStatusChanged(room, participant);
				}
			}

			// Token: 0x06001134 RID: 4404 RVA: 0x0004A198 File Offset: 0x00048398
			internal void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				object obj = this.mLifecycleLock;
				lock (obj)
				{
					this.mState.HandleRoomResponse(response);
				}
			}

			// Token: 0x06001135 RID: 4405 RVA: 0x0004A1E8 File Offset: 0x000483E8
			internal void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				this.mState.OnDataReceived(room, sender, data, isReliable);
			}

			// Token: 0x06001136 RID: 4406 RVA: 0x0004A1FC File Offset: 0x000483FC
			internal void SendMessageToAll(bool reliable, byte[] data)
			{
				this.SendMessageToAll(reliable, data, 0, data.Length);
			}

			// Token: 0x06001137 RID: 4407 RVA: 0x0004A20C File Offset: 0x0004840C
			internal void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
			{
				this.mState.SendToAll(data, offset, length, reliable);
			}

			// Token: 0x06001138 RID: 4408 RVA: 0x0004A220 File Offset: 0x00048420
			internal void SendMessage(bool reliable, string participantId, byte[] data)
			{
				this.SendMessage(reliable, participantId, data, 0, data.Length);
			}

			// Token: 0x06001139 RID: 4409 RVA: 0x0004A230 File Offset: 0x00048430
			internal void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
			{
				this.mState.SendToSpecificRecipient(participantId, data, offset, length, reliable);
			}

			// Token: 0x0600113A RID: 4410 RVA: 0x0004A248 File Offset: 0x00048448
			internal List<Participant> GetConnectedParticipants()
			{
				return this.mState.GetConnectedParticipants();
			}

			// Token: 0x0600113B RID: 4411 RVA: 0x0004A258 File Offset: 0x00048458
			internal virtual Participant GetSelf()
			{
				return this.mState.GetSelf();
			}

			// Token: 0x0600113C RID: 4412 RVA: 0x0004A268 File Offset: 0x00048468
			internal virtual Participant GetParticipant(string participantId)
			{
				return this.mState.GetParticipant(participantId);
			}

			// Token: 0x0600113D RID: 4413 RVA: 0x0004A278 File Offset: 0x00048478
			internal virtual bool IsRoomConnected()
			{
				return this.mState.IsRoomConnected();
			}

			// Token: 0x04000BC0 RID: 3008
			private readonly object mLifecycleLock = new object();

			// Token: 0x04000BC1 RID: 3009
			private readonly NativeRealtimeMultiplayerClient.OnGameThreadForwardingListener mListener;

			// Token: 0x04000BC2 RID: 3010
			private readonly RealtimeManager mManager;

			// Token: 0x04000BC3 RID: 3011
			private volatile string mCurrentPlayerId;

			// Token: 0x04000BC4 RID: 3012
			private volatile NativeRealtimeMultiplayerClient.State mState;

			// Token: 0x04000BC5 RID: 3013
			private volatile bool mStillPreRoomCreation;

			// Token: 0x04000BC6 RID: 3014
			private Invitation mInvitation;

			// Token: 0x04000BC7 RID: 3015
			private volatile bool mShowingUI;

			// Token: 0x04000BC8 RID: 3016
			private uint mMinPlayersToStart;
		}

		// Token: 0x02000224 RID: 548
		private class OnGameThreadForwardingListener
		{
			// Token: 0x0600113E RID: 4414 RVA: 0x0004A288 File Offset: 0x00048488
			internal OnGameThreadForwardingListener(RealTimeMultiplayerListener listener)
			{
				this.mListener = Misc.CheckNotNull<RealTimeMultiplayerListener>(listener);
			}

			// Token: 0x0600113F RID: 4415 RVA: 0x0004A29C File Offset: 0x0004849C
			public void RoomSetupProgress(float percent)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnRoomSetupProgress(percent);
				});
			}

			// Token: 0x06001140 RID: 4416 RVA: 0x0004A2D0 File Offset: 0x000484D0
			public void RoomConnected(bool success)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnRoomConnected(success);
				});
			}

			// Token: 0x06001141 RID: 4417 RVA: 0x0004A304 File Offset: 0x00048504
			public void LeftRoom()
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnLeftRoom();
				});
			}

			// Token: 0x06001142 RID: 4418 RVA: 0x0004A318 File Offset: 0x00048518
			public void PeersConnected(string[] participantIds)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnPeersConnected(participantIds);
				});
			}

			// Token: 0x06001143 RID: 4419 RVA: 0x0004A34C File Offset: 0x0004854C
			public void PeersDisconnected(string[] participantIds)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnPeersDisconnected(participantIds);
				});
			}

			// Token: 0x06001144 RID: 4420 RVA: 0x0004A380 File Offset: 0x00048580
			public void RealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnRealTimeMessageReceived(isReliable, senderId, data);
				});
			}

			// Token: 0x06001145 RID: 4421 RVA: 0x0004A3C0 File Offset: 0x000485C0
			public void ParticipantLeft(Participant participant)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnParticipantLeft(participant);
				});
			}

			// Token: 0x04000BC9 RID: 3017
			private readonly RealTimeMultiplayerListener mListener;
		}

		// Token: 0x02000225 RID: 549
		internal abstract class State
		{
			// Token: 0x06001148 RID: 4424 RVA: 0x0004A40C File Offset: 0x0004860C
			internal virtual void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				Logger.d(base.GetType().Name + ".HandleRoomResponse: Defaulting to no-op.");
			}

			// Token: 0x06001149 RID: 4425 RVA: 0x0004A428 File Offset: 0x00048628
			internal virtual bool IsActive()
			{
				Logger.d(base.GetType().Name + ".IsNonPreemptable: Is preemptable by default.");
				return true;
			}

			// Token: 0x0600114A RID: 4426 RVA: 0x0004A448 File Offset: 0x00048648
			internal virtual void LeaveRoom()
			{
				Logger.d(base.GetType().Name + ".LeaveRoom: Defaulting to no-op.");
			}

			// Token: 0x0600114B RID: 4427 RVA: 0x0004A464 File Offset: 0x00048664
			internal virtual void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				Logger.d(base.GetType().Name + ".ShowWaitingRoomUI: Defaulting to no-op.");
			}

			// Token: 0x0600114C RID: 4428 RVA: 0x0004A480 File Offset: 0x00048680
			internal virtual void OnStateEntered()
			{
				Logger.d(base.GetType().Name + ".OnStateEntered: Defaulting to no-op.");
			}

			// Token: 0x0600114D RID: 4429 RVA: 0x0004A49C File Offset: 0x0004869C
			internal virtual void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				Logger.d(base.GetType().Name + ".OnRoomStatusChanged: Defaulting to no-op.");
			}

			// Token: 0x0600114E RID: 4430 RVA: 0x0004A4B8 File Offset: 0x000486B8
			internal virtual void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				Logger.d(base.GetType().Name + ".OnConnectedSetChanged: Defaulting to no-op.");
			}

			// Token: 0x0600114F RID: 4431 RVA: 0x0004A4D4 File Offset: 0x000486D4
			internal virtual void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				Logger.d(base.GetType().Name + ".OnParticipantStatusChanged: Defaulting to no-op.");
			}

			// Token: 0x06001150 RID: 4432 RVA: 0x0004A4F0 File Offset: 0x000486F0
			internal virtual void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				Logger.d(base.GetType().Name + ".OnDataReceived: Defaulting to no-op.");
			}

			// Token: 0x06001151 RID: 4433 RVA: 0x0004A50C File Offset: 0x0004870C
			internal virtual void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				Logger.d(base.GetType().Name + ".SendToSpecificRecipient: Defaulting to no-op.");
			}

			// Token: 0x06001152 RID: 4434 RVA: 0x0004A528 File Offset: 0x00048728
			internal virtual void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				Logger.d(base.GetType().Name + ".SendToApp: Defaulting to no-op.");
			}

			// Token: 0x06001153 RID: 4435 RVA: 0x0004A544 File Offset: 0x00048744
			internal virtual List<Participant> GetConnectedParticipants()
			{
				Logger.d(base.GetType().Name + ".GetConnectedParticipants: Returning empty connected participants");
				return new List<Participant>();
			}

			// Token: 0x06001154 RID: 4436 RVA: 0x0004A568 File Offset: 0x00048768
			internal virtual Participant GetSelf()
			{
				Logger.d(base.GetType().Name + ".GetSelf: Returning null self.");
				return null;
			}

			// Token: 0x06001155 RID: 4437 RVA: 0x0004A588 File Offset: 0x00048788
			internal virtual Participant GetParticipant(string participantId)
			{
				Logger.d(base.GetType().Name + ".GetSelf: Returning null participant.");
				return null;
			}

			// Token: 0x06001156 RID: 4438 RVA: 0x0004A5A8 File Offset: 0x000487A8
			internal virtual bool IsRoomConnected()
			{
				Logger.d(base.GetType().Name + ".IsRoomConnected: Returning room not connected.");
				return false;
			}
		}

		// Token: 0x02000226 RID: 550
		private abstract class MessagingEnabledState : NativeRealtimeMultiplayerClient.State
		{
			// Token: 0x06001157 RID: 4439 RVA: 0x0004A5C8 File Offset: 0x000487C8
			internal MessagingEnabledState(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
				this.UpdateCurrentRoom(room);
			}

			// Token: 0x06001158 RID: 4440 RVA: 0x0004A5E4 File Offset: 0x000487E4
			internal void UpdateCurrentRoom(NativeRealTimeRoom room)
			{
				if (this.mRoom != null)
				{
					this.mRoom.Dispose();
				}
				this.mRoom = Misc.CheckNotNull<NativeRealTimeRoom>(room);
				this.mNativeParticipants = this.mRoom.Participants().ToDictionary((GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.Id());
				this.mParticipants = (from p in this.mNativeParticipants.Values
				select p.AsParticipant()).ToDictionary((Participant p) => p.ParticipantId);
			}

			// Token: 0x06001159 RID: 4441 RVA: 0x0004A69C File Offset: 0x0004889C
			internal sealed override void OnRoomStatusChanged(NativeRealTimeRoom room)
			{
				this.HandleRoomStatusChanged(room);
				this.UpdateCurrentRoom(room);
			}

			// Token: 0x0600115A RID: 4442 RVA: 0x0004A6AC File Offset: 0x000488AC
			internal virtual void HandleRoomStatusChanged(NativeRealTimeRoom room)
			{
			}

			// Token: 0x0600115B RID: 4443 RVA: 0x0004A6B0 File Offset: 0x000488B0
			internal sealed override void OnConnectedSetChanged(NativeRealTimeRoom room)
			{
				this.HandleConnectedSetChanged(room);
				this.UpdateCurrentRoom(room);
			}

			// Token: 0x0600115C RID: 4444 RVA: 0x0004A6C0 File Offset: 0x000488C0
			internal virtual void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
			}

			// Token: 0x0600115D RID: 4445 RVA: 0x0004A6C4 File Offset: 0x000488C4
			internal sealed override void OnParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				this.HandleParticipantStatusChanged(room, participant);
				this.UpdateCurrentRoom(room);
			}

			// Token: 0x0600115E RID: 4446 RVA: 0x0004A6D8 File Offset: 0x000488D8
			internal virtual void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
			}

			// Token: 0x0600115F RID: 4447 RVA: 0x0004A6DC File Offset: 0x000488DC
			internal sealed override List<Participant> GetConnectedParticipants()
			{
				List<Participant> list = (from p in this.mParticipants.Values
				where p.IsConnectedToRoom
				select p).ToList<Participant>();
				list.Sort();
				return list;
			}

			// Token: 0x06001160 RID: 4448 RVA: 0x0004A724 File Offset: 0x00048924
			internal override void SendToSpecificRecipient(string recipientId, byte[] data, int offset, int length, bool isReliable)
			{
				if (!this.mNativeParticipants.ContainsKey(recipientId))
				{
					Logger.e("Attempted to send message to unknown participant " + recipientId);
					return;
				}
				if (isReliable)
				{
					this.mSession.Manager().SendReliableMessage(this.mRoom, this.mNativeParticipants[recipientId], Misc.GetSubsetBytes(data, offset, length), null);
				}
				else
				{
					this.mSession.Manager().SendUnreliableMessageToSpecificParticipants(this.mRoom, new List<GooglePlayGames.Native.PInvoke.MultiplayerParticipant>
					{
						this.mNativeParticipants[recipientId]
					}, Misc.GetSubsetBytes(data, offset, length));
				}
			}

			// Token: 0x06001161 RID: 4449 RVA: 0x0004A7C4 File Offset: 0x000489C4
			internal override void SendToAll(byte[] data, int offset, int length, bool isReliable)
			{
				byte[] subsetBytes = Misc.GetSubsetBytes(data, offset, length);
				if (isReliable)
				{
					foreach (string recipientId in this.mNativeParticipants.Keys)
					{
						this.SendToSpecificRecipient(recipientId, subsetBytes, 0, subsetBytes.Length, true);
					}
				}
				else
				{
					this.mSession.Manager().SendUnreliableMessageToAll(this.mRoom, subsetBytes);
				}
			}

			// Token: 0x06001162 RID: 4450 RVA: 0x0004A864 File Offset: 0x00048A64
			internal override void OnDataReceived(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant sender, byte[] data, bool isReliable)
			{
				this.mSession.OnGameThreadListener().RealTimeMessageReceived(isReliable, sender.Id(), data);
			}

			// Token: 0x04000BCA RID: 3018
			protected readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

			// Token: 0x04000BCB RID: 3019
			protected NativeRealTimeRoom mRoom;

			// Token: 0x04000BCC RID: 3020
			protected Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> mNativeParticipants;

			// Token: 0x04000BCD RID: 3021
			protected Dictionary<string, Participant> mParticipants;
		}

		// Token: 0x02000227 RID: 551
		private class BeforeRoomCreateStartedState : NativeRealtimeMultiplayerClient.State
		{
			// Token: 0x06001167 RID: 4455 RVA: 0x0004A8AC File Offset: 0x00048AAC
			internal BeforeRoomCreateStartedState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			// Token: 0x06001168 RID: 4456 RVA: 0x0004A8C0 File Offset: 0x00048AC0
			internal override void LeaveRoom()
			{
				Logger.d("Session was torn down before room was created.");
				this.mContainingSession.OnGameThreadListener().RoomConnected(false);
				this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
			}

			// Token: 0x04000BD2 RID: 3026
			private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;
		}

		// Token: 0x02000228 RID: 552
		private class RoomCreationPendingState : NativeRealtimeMultiplayerClient.State
		{
			// Token: 0x06001169 RID: 4457 RVA: 0x0004A8F4 File Offset: 0x00048AF4
			internal RoomCreationPendingState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mContainingSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			// Token: 0x0600116A RID: 4458 RVA: 0x0004A908 File Offset: 0x00048B08
			internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mContainingSession));
					this.mContainingSession.OnGameThreadListener().RoomConnected(false);
					return;
				}
				this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.ConnectingState(response.Room(), this.mContainingSession));
			}

			// Token: 0x0600116B RID: 4459 RVA: 0x0004A964 File Offset: 0x00048B64
			internal override bool IsActive()
			{
				return true;
			}

			// Token: 0x0600116C RID: 4460 RVA: 0x0004A968 File Offset: 0x00048B68
			internal override void LeaveRoom()
			{
				Logger.d("Received request to leave room during room creation, aborting creation.");
				this.mContainingSession.EnterState(new NativeRealtimeMultiplayerClient.AbortingRoomCreationState(this.mContainingSession));
			}

			// Token: 0x04000BD3 RID: 3027
			private readonly NativeRealtimeMultiplayerClient.RoomSession mContainingSession;
		}

		// Token: 0x02000229 RID: 553
		private class ConnectingState : NativeRealtimeMultiplayerClient.MessagingEnabledState
		{
			// Token: 0x0600116D RID: 4461 RVA: 0x0004A998 File Offset: 0x00048B98
			internal ConnectingState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
			{
				this.mPercentPerParticipant = 80f / session.MinPlayersToStart;
			}

			// Token: 0x0600116F RID: 4463 RVA: 0x0004AA04 File Offset: 0x00048C04
			internal override void OnStateEntered()
			{
				this.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
			}

			// Token: 0x06001170 RID: 4464 RVA: 0x0004AA1C File Offset: 0x00048C1C
			internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
				HashSet<string> hashSet = new HashSet<string>();
				if ((room.Status() == Types.RealTimeRoomStatus.AUTO_MATCHING || room.Status() == Types.RealTimeRoomStatus.CONNECTING) && this.mSession.MinPlayersToStart <= room.ParticipantCount())
				{
					this.mSession.MinPlayersToStart = this.mSession.MinPlayersToStart + room.ParticipantCount();
					this.mPercentPerParticipant = 80f / this.mSession.MinPlayersToStart;
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant in room.Participants())
				{
					using (multiplayerParticipant)
					{
						if (multiplayerParticipant.IsConnectedToRoom())
						{
							hashSet.Add(multiplayerParticipant.Id());
						}
					}
				}
				if (this.mConnectedParticipants.Equals(hashSet))
				{
					Logger.w("Received connected set callback with unchanged connected set!");
					return;
				}
				IEnumerable<string> source = this.mConnectedParticipants.Except(hashSet);
				if (room.Status() == Types.RealTimeRoomStatus.DELETED)
				{
					Logger.e("Participants disconnected during room setup, failing. Participants were: " + string.Join(",", source.ToArray<string>()));
					this.mSession.OnGameThreadListener().RoomConnected(false);
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
					return;
				}
				IEnumerable<string> source2 = hashSet.Except(this.mConnectedParticipants);
				Logger.d("New participants connected: " + string.Join(",", source2.ToArray<string>()));
				if (room.Status() == Types.RealTimeRoomStatus.ACTIVE)
				{
					Logger.d("Fully connected! Transitioning to active state.");
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(room, this.mSession));
					this.mSession.OnGameThreadListener().RoomConnected(true);
					return;
				}
				this.mPercentComplete += this.mPercentPerParticipant * (float)source2.Count<string>();
				this.mConnectedParticipants = hashSet;
				this.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
			}

			// Token: 0x06001171 RID: 4465 RVA: 0x0004AC4C File Offset: 0x00048E4C
			internal override void HandleParticipantStatusChanged(NativeRealTimeRoom room, GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant)
			{
				if (!NativeRealtimeMultiplayerClient.ConnectingState.FailedStatuses.Contains(participant.Status()))
				{
					return;
				}
				this.mSession.OnGameThreadListener().ParticipantLeft(participant.AsParticipant());
				if (room.Status() != Types.RealTimeRoomStatus.CONNECTING && room.Status() != Types.RealTimeRoomStatus.AUTO_MATCHING)
				{
					this.LeaveRoom();
				}
			}

			// Token: 0x06001172 RID: 4466 RVA: 0x0004ACA4 File Offset: 0x00048EA4
			internal override void LeaveRoom()
			{
				this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, this.mRoom, delegate()
				{
					this.mSession.OnGameThreadListener().RoomConnected(false);
				}));
			}

			// Token: 0x06001173 RID: 4467 RVA: 0x0004ACDC File Offset: 0x00048EDC
			internal override void ShowWaitingRoomUI(uint minimumParticipantsBeforeStarting)
			{
				this.mSession.ShowingUI = true;
				this.mSession.Manager().ShowWaitingRoomUI(this.mRoom, minimumParticipantsBeforeStarting, delegate(RealtimeManager.WaitingRoomUIResponse response)
				{
					this.mSession.ShowingUI = false;
					Logger.d("ShowWaitingRoomUI Response: " + response.ResponseStatus());
					if (response.ResponseStatus() == CommonErrorStatus.UIStatus.VALID)
					{
						Logger.d(string.Concat(new object[]
						{
							"Connecting state ShowWaitingRoomUI: room pcount:",
							response.Room().ParticipantCount(),
							" status: ",
							response.Room().Status()
						}));
						if (response.Room().Status() == Types.RealTimeRoomStatus.ACTIVE)
						{
							this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ActiveState(response.Room(), this.mSession));
						}
					}
					else if (response.ResponseStatus() == CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM)
					{
						this.LeaveRoom();
					}
					else
					{
						this.mSession.OnGameThreadListener().RoomSetupProgress(this.mPercentComplete);
					}
				});
			}

			// Token: 0x04000BD4 RID: 3028
			private const float InitialPercentComplete = 20f;

			// Token: 0x04000BD5 RID: 3029
			private static readonly HashSet<Types.ParticipantStatus> FailedStatuses = new HashSet<Types.ParticipantStatus>
			{
				Types.ParticipantStatus.DECLINED,
				Types.ParticipantStatus.LEFT
			};

			// Token: 0x04000BD6 RID: 3030
			private HashSet<string> mConnectedParticipants = new HashSet<string>();

			// Token: 0x04000BD7 RID: 3031
			private float mPercentComplete = 20f;

			// Token: 0x04000BD8 RID: 3032
			private float mPercentPerParticipant;
		}

		// Token: 0x0200022A RID: 554
		private class ActiveState : NativeRealtimeMultiplayerClient.MessagingEnabledState
		{
			// Token: 0x06001176 RID: 4470 RVA: 0x0004AE14 File Offset: 0x00049014
			internal ActiveState(NativeRealTimeRoom room, NativeRealtimeMultiplayerClient.RoomSession session) : base(session, room)
			{
			}

			// Token: 0x06001177 RID: 4471 RVA: 0x0004AE20 File Offset: 0x00049020
			internal override void OnStateEntered()
			{
				if (this.GetSelf() == null)
				{
					Logger.e("Room reached active state with unknown participant for the player");
					this.LeaveRoom();
				}
			}

			// Token: 0x06001178 RID: 4472 RVA: 0x0004AE40 File Offset: 0x00049040
			internal override bool IsRoomConnected()
			{
				return true;
			}

			// Token: 0x06001179 RID: 4473 RVA: 0x0004AE44 File Offset: 0x00049044
			internal override Participant GetParticipant(string participantId)
			{
				if (!this.mParticipants.ContainsKey(participantId))
				{
					Logger.e("Attempted to retrieve unknown participant " + participantId);
					return null;
				}
				return this.mParticipants[participantId];
			}

			// Token: 0x0600117A RID: 4474 RVA: 0x0004AE78 File Offset: 0x00049078
			internal override Participant GetSelf()
			{
				foreach (Participant participant in this.mParticipants.Values)
				{
					if (participant.Player != null && participant.Player.id.Equals(this.mSession.SelfPlayerId()))
					{
						return participant;
					}
				}
				return null;
			}

			// Token: 0x0600117B RID: 4475 RVA: 0x0004AF14 File Offset: 0x00049114
			internal override void HandleConnectedSetChanged(NativeRealTimeRoom room)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				Dictionary<string, GooglePlayGames.Native.PInvoke.MultiplayerParticipant> dictionary = room.Participants().ToDictionary((GooglePlayGames.Native.PInvoke.MultiplayerParticipant p) => p.Id());
				foreach (string text in this.mNativeParticipants.Keys)
				{
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = dictionary[text];
					GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant2 = this.mNativeParticipants[text];
					if (!multiplayerParticipant.IsConnectedToRoom())
					{
						list2.Add(text);
					}
					if (!multiplayerParticipant2.IsConnectedToRoom() && multiplayerParticipant.IsConnectedToRoom())
					{
						list.Add(text);
					}
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant3 in this.mNativeParticipants.Values)
				{
					multiplayerParticipant3.Dispose();
				}
				this.mNativeParticipants = dictionary;
				this.mParticipants = (from p in this.mNativeParticipants.Values
				select p.AsParticipant()).ToDictionary((Participant p) => p.ParticipantId);
				Logger.d("Updated participant statuses: " + string.Join(",", (from p in this.mParticipants.Values
				select p.ToString()).ToArray<string>()));
				if (list2.Contains(this.GetSelf().ParticipantId))
				{
					Logger.w("Player was disconnected from the multiplayer session.");
				}
				string selfId = this.GetSelf().ParticipantId;
				list = (from peerId in list
				where !peerId.Equals(selfId)
				select peerId).ToList<string>();
				list2 = (from peerId in list2
				where !peerId.Equals(selfId)
				select peerId).ToList<string>();
				if (list.Count > 0)
				{
					list.Sort();
					this.mSession.OnGameThreadListener().PeersConnected((from peer in list
					where !peer.Equals(selfId)
					select peer).ToArray<string>());
				}
				if (list2.Count > 0)
				{
					list2.Sort();
					this.mSession.OnGameThreadListener().PeersDisconnected((from peer in list2
					where !peer.Equals(selfId)
					select peer).ToArray<string>());
				}
			}

			// Token: 0x0600117C RID: 4476 RVA: 0x0004B1DC File Offset: 0x000493DC
			internal override void LeaveRoom()
			{
				this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, this.mRoom, delegate()
				{
					this.mSession.OnGameThreadListener().LeftRoom();
				}));
			}
		}

		// Token: 0x0200022B RID: 555
		private class ShutdownState : NativeRealtimeMultiplayerClient.State
		{
			// Token: 0x06001182 RID: 4482 RVA: 0x0004B248 File Offset: 0x00049448
			internal ShutdownState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			// Token: 0x06001183 RID: 4483 RVA: 0x0004B25C File Offset: 0x0004945C
			internal override bool IsActive()
			{
				return false;
			}

			// Token: 0x06001184 RID: 4484 RVA: 0x0004B260 File Offset: 0x00049460
			internal override void LeaveRoom()
			{
				this.mSession.OnGameThreadListener().LeftRoom();
			}

			// Token: 0x04000BDD RID: 3037
			private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;
		}

		// Token: 0x0200022C RID: 556
		private class LeavingRoom : NativeRealtimeMultiplayerClient.State
		{
			// Token: 0x06001185 RID: 4485 RVA: 0x0004B274 File Offset: 0x00049474
			internal LeavingRoom(NativeRealtimeMultiplayerClient.RoomSession session, NativeRealTimeRoom room, Action leavingCompleteCallback)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
				this.mRoomToLeave = Misc.CheckNotNull<NativeRealTimeRoom>(room);
				this.mLeavingCompleteCallback = Misc.CheckNotNull<Action>(leavingCompleteCallback);
			}

			// Token: 0x06001186 RID: 4486 RVA: 0x0004B2AC File Offset: 0x000494AC
			internal override bool IsActive()
			{
				return false;
			}

			// Token: 0x06001187 RID: 4487 RVA: 0x0004B2B0 File Offset: 0x000494B0
			internal override void OnStateEntered()
			{
				this.mSession.Manager().LeaveRoom(this.mRoomToLeave, delegate(CommonErrorStatus.ResponseStatus status)
				{
					this.mLeavingCompleteCallback();
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
				});
			}

			// Token: 0x04000BDE RID: 3038
			private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;

			// Token: 0x04000BDF RID: 3039
			private readonly NativeRealTimeRoom mRoomToLeave;

			// Token: 0x04000BE0 RID: 3040
			private readonly Action mLeavingCompleteCallback;
		}

		// Token: 0x0200022D RID: 557
		private class AbortingRoomCreationState : NativeRealtimeMultiplayerClient.State
		{
			// Token: 0x06001189 RID: 4489 RVA: 0x0004B304 File Offset: 0x00049504
			internal AbortingRoomCreationState(NativeRealtimeMultiplayerClient.RoomSession session)
			{
				this.mSession = Misc.CheckNotNull<NativeRealtimeMultiplayerClient.RoomSession>(session);
			}

			// Token: 0x0600118A RID: 4490 RVA: 0x0004B318 File Offset: 0x00049518
			internal override bool IsActive()
			{
				return false;
			}

			// Token: 0x0600118B RID: 4491 RVA: 0x0004B31C File Offset: 0x0004951C
			internal override void HandleRoomResponse(RealtimeManager.RealTimeRoomResponse response)
			{
				if (!response.RequestSucceeded())
				{
					this.mSession.EnterState(new NativeRealtimeMultiplayerClient.ShutdownState(this.mSession));
					this.mSession.OnGameThreadListener().RoomConnected(false);
					return;
				}
				this.mSession.EnterState(new NativeRealtimeMultiplayerClient.LeavingRoom(this.mSession, response.Room(), delegate()
				{
					this.mSession.OnGameThreadListener().RoomConnected(false);
				}));
			}

			// Token: 0x04000BE1 RID: 3041
			private readonly NativeRealtimeMultiplayerClient.RoomSession mSession;
		}
	}
}
