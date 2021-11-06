using System;
using System.Collections;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x02000231 RID: 561
	public class NativeTurnBasedMultiplayerClient : ITurnBasedMultiplayerClient
	{
		// Token: 0x060011A9 RID: 4521 RVA: 0x0004BD20 File Offset: 0x00049F20
		internal NativeTurnBasedMultiplayerClient(NativeClient nativeClient, TurnBasedManager manager)
		{
			this.mTurnBasedManager = manager;
			this.mNativeClient = nativeClient;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004BD38 File Offset: 0x00049F38
		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			this.CreateQuickMatch(minOpponents, maxOpponents, variant, 0UL, callback);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0004BD48 File Offset: 0x00049F48
		public void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(callback);
			using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
			{
				turnBasedMatchConfigBuilder.SetVariant(variant).SetMinimumAutomatchingPlayers(minOpponents).SetMaximumAutomatchingPlayers(maxOpponents).SetExclusiveBitMask(exclusiveBitmask);
				using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig turnBasedMatchConfig = turnBasedMatchConfigBuilder.Build())
				{
					this.mTurnBasedManager.CreateMatch(turnBasedMatchConfig, this.BridgeMatchToUserCallback(delegate(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
					{
						callback(status == UIStatus.Valid, match);
					}));
				}
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0004BE14 File Offset: 0x0004A014
		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			this.CreateWithInvitationScreen(minOpponents, maxOpponents, variant, delegate(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
			{
				callback(status == UIStatus.Valid, match);
			});
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0004BE44 File Offset: 0x0004A044
		public void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(callback);
			this.mTurnBasedManager.ShowPlayerSelectUI(minOpponents, maxOpponents, true, delegate(PlayerSelectUIResponse result)
			{
				if (result.Status() != CommonErrorStatus.UIStatus.VALID)
				{
					callback((UIStatus)result.Status(), null);
					return;
				}
				using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder turnBasedMatchConfigBuilder = GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder.Create())
				{
					turnBasedMatchConfigBuilder.PopulateFromUIResponse(result).SetVariant(variant);
					using (GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig turnBasedMatchConfig = turnBasedMatchConfigBuilder.Build())
					{
						this.mTurnBasedManager.CreateMatch(turnBasedMatchConfig, this.BridgeMatchToUserCallback(callback));
					}
				}
			});
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0004BE98 File Offset: 0x0004A098
		public void GetAllInvitations(Action<Invitation[]> callback)
		{
			this.mTurnBasedManager.GetAllTurnbasedMatches(delegate(TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				Invitation[] array = new Invitation[allMatches.InvitationCount()];
				int num = 0;
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation multiplayerInvitation in allMatches.Invitations())
				{
					array[num++] = multiplayerInvitation.AsInvitation();
				}
				callback(array);
			});
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0004BECC File Offset: 0x0004A0CC
		public void GetAllMatches(Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback)
		{
			this.mTurnBasedManager.GetAllTurnbasedMatches(delegate(TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				int num = allMatches.MyTurnMatchesCount() + allMatches.TheirTurnMatchesCount() + allMatches.CompletedMatchesCount();
				GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[] array = new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[num];
				int num2 = 0;
				foreach (NativeTurnBasedMatch nativeTurnBasedMatch in allMatches.MyTurnMatches())
				{
					array[num2++] = nativeTurnBasedMatch.AsTurnBasedMatch(this.mNativeClient.GetUserId());
				}
				foreach (NativeTurnBasedMatch nativeTurnBasedMatch2 in allMatches.TheirTurnMatches())
				{
					array[num2++] = nativeTurnBasedMatch2.AsTurnBasedMatch(this.mNativeClient.GetUserId());
				}
				foreach (NativeTurnBasedMatch nativeTurnBasedMatch3 in allMatches.CompletedMatches())
				{
					array[num2++] = nativeTurnBasedMatch3.AsTurnBasedMatch(this.mNativeClient.GetUserId());
				}
				callback(array);
			});
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0004BF04 File Offset: 0x0004A104
		private Action<TurnBasedManager.TurnBasedMatchResponse> BridgeMatchToUserCallback(Action<UIStatus, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> userCallback)
		{
			return delegate(TurnBasedManager.TurnBasedMatchResponse callbackResult)
			{
				using (NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						UIStatus arg = UIStatus.InternalError;
						CommonErrorStatus.MultiplayerStatus multiplayerStatus = callbackResult.ResponseStatus();
						switch (multiplayerStatus + 5)
						{
						case (CommonErrorStatus.MultiplayerStatus)0:
							arg = UIStatus.Timeout;
							break;
						case CommonErrorStatus.MultiplayerStatus.VALID:
							arg = UIStatus.VersionUpdateRequired;
							break;
						case CommonErrorStatus.MultiplayerStatus.VALID_BUT_STALE:
							arg = UIStatus.NotAuthorized;
							break;
						case (CommonErrorStatus.MultiplayerStatus)3:
							arg = UIStatus.InternalError;
							break;
						case (CommonErrorStatus.MultiplayerStatus)6:
							arg = UIStatus.Valid;
							break;
						case (CommonErrorStatus.MultiplayerStatus)7:
							arg = UIStatus.Valid;
							break;
						}
						userCallback(arg, null);
					}
					else
					{
						GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(this.mNativeClient.GetUserId());
						Logger.d("Passing converted match to user callback:" + turnBasedMatch);
						userCallback(UIStatus.Valid, turnBasedMatch);
					}
				}
			};
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0004BF34 File Offset: 0x0004A134
		public void AcceptFromInbox(Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(callback);
			this.mTurnBasedManager.ShowInboxUI(delegate(TurnBasedManager.MatchInboxUIResponse callbackResult)
			{
				using (NativeTurnBasedMatch nativeTurnBasedMatch = callbackResult.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						callback(false, null);
					}
					else
					{
						GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch turnBasedMatch = nativeTurnBasedMatch.AsTurnBasedMatch(this.mNativeClient.GetUserId());
						Logger.d("Passing converted match to user callback:" + turnBasedMatch);
						callback(true, turnBasedMatch);
					}
				}
			});
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0004BF80 File Offset: 0x0004A180
		public void AcceptInvitation(string invitationId, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(callback);
			this.FindInvitationWithId(invitationId, delegate(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
			{
				if (invitation == null)
				{
					Logger.e("Could not find invitation with id " + invitationId);
					callback(false, null);
					return;
				}
				this.mTurnBasedManager.AcceptInvitation(invitation, this.BridgeMatchToUserCallback(delegate(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match)
				{
					callback(status == UIStatus.Valid, match);
				}));
			});
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0004BFD4 File Offset: 0x0004A1D4
		private void FindInvitationWithId(string invitationId, Action<GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
		{
			this.mTurnBasedManager.GetAllTurnbasedMatches(delegate(TurnBasedManager.TurnBasedMatchesResponse allMatches)
			{
				if (allMatches.Status() <= (CommonErrorStatus.MultiplayerStatus)0)
				{
					callback(null);
					return;
				}
				foreach (GooglePlayGames.Native.PInvoke.MultiplayerInvitation multiplayerInvitation in allMatches.Invitations())
				{
					using (multiplayerInvitation)
					{
						if (multiplayerInvitation.Id().Equals(invitationId))
						{
							callback(multiplayerInvitation);
							return;
						}
					}
				}
				callback(null);
			});
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0004C00C File Offset: 0x0004A20C
		public void RegisterMatchDelegate(MatchDelegate del)
		{
			if (del == null)
			{
				this.mMatchDelegate = null;
			}
			else
			{
				this.mMatchDelegate = Callbacks.AsOnGameThreadCallback<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool>(delegate(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool autoLaunch)
				{
					del(match, autoLaunch);
				});
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004C058 File Offset: 0x0004A258
		internal void HandleMatchEvent(Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
		{
			Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> currentDelegate = this.mMatchDelegate;
			if (currentDelegate == null)
			{
				return;
			}
			if (eventType == Types.MultiplayerEvent.REMOVED)
			{
				Logger.d("Ignoring REMOVE event for match " + matchId);
				return;
			}
			bool shouldAutolaunch = eventType == Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
			match.ReferToMe();
			Callbacks.AsCoroutine(this.WaitForLogin(delegate
			{
				currentDelegate(match.AsTurnBasedMatch(this.mNativeClient.GetUserId()), shouldAutolaunch);
				match.ForgetMe();
			}));
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004C0D8 File Offset: 0x0004A2D8
		private IEnumerator WaitForLogin(Action method)
		{
			if (string.IsNullOrEmpty(this.mNativeClient.GetUserId()))
			{
				yield return null;
			}
			method();
			yield break;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004C104 File Offset: 0x0004A304
		public void TakeTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, string pendingParticipantId, Action<bool> callback)
		{
			Logger.describe(data);
			callback = Callbacks.AsOnGameThreadCallback<bool>(callback);
			this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, callback, delegate(GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
			{
				this.mTurnBasedManager.TakeTurn(foundMatch, data, pendingParticipant, delegate(TurnBasedManager.TurnBasedMatchResponse result)
				{
					if (result.RequestSucceeded())
					{
						callback(true);
					}
					else
					{
						Logger.d("Taking turn failed: " + result.ResponseStatus());
						callback(false);
					}
				});
			});
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004C164 File Offset: 0x0004A364
		private void FindEqualVersionMatch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> onFailure, Action<NativeTurnBasedMatch> onVersionMatch)
		{
			this.mTurnBasedManager.GetMatch(match.MatchId, delegate(TurnBasedManager.TurnBasedMatchResponse response)
			{
				using (NativeTurnBasedMatch nativeTurnBasedMatch = response.Match())
				{
					if (nativeTurnBasedMatch == null)
					{
						Logger.e(string.Format("Could not find match {0}", match.MatchId));
						onFailure(false);
					}
					else if (nativeTurnBasedMatch.Version() != match.Version)
					{
						Logger.e(string.Format("Attempted to update a stale version of the match. Expected version was {0} but current version is {1}.", match.Version, nativeTurnBasedMatch.Version()));
						onFailure(false);
					}
					else
					{
						onVersionMatch(nativeTurnBasedMatch);
					}
				}
			});
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004C1B0 File Offset: 0x0004A3B0
		private void FindEqualVersionMatchWithParticipant(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string participantId, Action<bool> onFailure, Action<GooglePlayGames.Native.PInvoke.MultiplayerParticipant, NativeTurnBasedMatch> onFoundParticipantAndMatch)
		{
			this.FindEqualVersionMatch(match, onFailure, delegate(NativeTurnBasedMatch foundMatch)
			{
				if (participantId == null)
				{
					using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = GooglePlayGames.Native.PInvoke.MultiplayerParticipant.AutomatchingSentinel())
					{
						onFoundParticipantAndMatch(multiplayerParticipant, foundMatch);
						return;
					}
				}
				using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant2 = foundMatch.ParticipantWithId(participantId))
				{
					if (multiplayerParticipant2 == null)
					{
						Logger.e(string.Format("Located match {0} but desired participant with ID {1} could not be found", match.MatchId, participantId));
						onFailure(false);
					}
					else
					{
						onFoundParticipantAndMatch(multiplayerParticipant2, foundMatch);
					}
				}
			});
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0004C200 File Offset: 0x0004A400
		public int GetMaxMatchDataSize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004C208 File Offset: 0x0004A408
		public void Finish(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, MatchOutcome outcome, Action<bool> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool>(callback);
			this.FindEqualVersionMatch(match, callback, delegate(NativeTurnBasedMatch foundMatch)
			{
				GooglePlayGames.Native.PInvoke.ParticipantResults participantResults = foundMatch.Results();
				foreach (string text in outcome.ParticipantIds)
				{
					Types.MatchResult matchResult = NativeTurnBasedMultiplayerClient.ResultToMatchResult(outcome.GetResultFor(text));
					uint placementFor = outcome.GetPlacementFor(text);
					if (participantResults.HasResultsForParticipant(text))
					{
						Types.MatchResult matchResult2 = participantResults.ResultsForParticipant(text);
						uint num = participantResults.PlacingForParticipant(text);
						if (matchResult != matchResult2 || placementFor != num)
						{
							Logger.e(string.Format("Attempted to override existing results for participant {0}: Placing {1}, Result {2}", text, num, matchResult2));
							callback(false);
							return;
						}
					}
					else
					{
						GooglePlayGames.Native.PInvoke.ParticipantResults participantResults2 = participantResults;
						participantResults = participantResults2.WithResult(text, placementFor, matchResult);
						participantResults2.Dispose();
					}
				}
				this.mTurnBasedManager.FinishMatchDuringMyTurn(foundMatch, data, participantResults, delegate(TurnBasedManager.TurnBasedMatchResponse response)
				{
					callback(response.RequestSucceeded());
				});
			});
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0004C264 File Offset: 0x0004A464
		private static Types.MatchResult ResultToMatchResult(MatchOutcome.ParticipantResult result)
		{
			switch (result)
			{
			case MatchOutcome.ParticipantResult.None:
				return Types.MatchResult.NONE;
			case MatchOutcome.ParticipantResult.Win:
				return Types.MatchResult.WIN;
			case MatchOutcome.ParticipantResult.Loss:
				return Types.MatchResult.LOSS;
			case MatchOutcome.ParticipantResult.Tie:
				return Types.MatchResult.TIE;
			default:
				Logger.e("Received unknown ParticipantResult " + result);
				return Types.MatchResult.NONE;
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004C2AC File Offset: 0x0004A4AC
		public void AcknowledgeFinished(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool>(callback);
			this.FindEqualVersionMatch(match, callback, delegate(NativeTurnBasedMatch foundMatch)
			{
				this.mTurnBasedManager.ConfirmPendingCompletion(foundMatch, delegate(TurnBasedManager.TurnBasedMatchResponse response)
				{
					callback(response.RequestSucceeded());
				});
			});
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0004C2F8 File Offset: 0x0004A4F8
		public void Leave(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool>(callback);
			this.FindEqualVersionMatch(match, callback, delegate(NativeTurnBasedMatch foundMatch)
			{
				this.mTurnBasedManager.LeaveMatchDuringTheirTurn(foundMatch, delegate(CommonErrorStatus.MultiplayerStatus status)
				{
					callback(status > (CommonErrorStatus.MultiplayerStatus)0);
				});
			});
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004C344 File Offset: 0x0004A544
		public void LeaveDuringTurn(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string pendingParticipantId, Action<bool> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool>(callback);
			this.FindEqualVersionMatchWithParticipant(match, pendingParticipantId, callback, delegate(GooglePlayGames.Native.PInvoke.MultiplayerParticipant pendingParticipant, NativeTurnBasedMatch foundMatch)
			{
				this.mTurnBasedManager.LeaveDuringMyTurn(foundMatch, pendingParticipant, delegate(CommonErrorStatus.MultiplayerStatus status)
				{
					callback(status > (CommonErrorStatus.MultiplayerStatus)0);
				});
			});
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004C390 File Offset: 0x0004A590
		public void Cancel(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool>(callback);
			this.FindEqualVersionMatch(match, callback, delegate(NativeTurnBasedMatch foundMatch)
			{
				this.mTurnBasedManager.CancelMatch(foundMatch, delegate(CommonErrorStatus.MultiplayerStatus status)
				{
					callback(status > (CommonErrorStatus.MultiplayerStatus)0);
				});
			});
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004C3DC File Offset: 0x0004A5DC
		public void Rematch(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, Action<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback)
		{
			callback = Callbacks.AsOnGameThreadCallback<bool, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch>(callback);
			this.FindEqualVersionMatch(match, delegate(bool failed)
			{
				callback(false, null);
			}, delegate(NativeTurnBasedMatch foundMatch)
			{
				this.mTurnBasedManager.Rematch(foundMatch, this.BridgeMatchToUserCallback(delegate(UIStatus status, GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch m)
				{
					callback(status == UIStatus.Valid, m);
				}));
			});
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004C430 File Offset: 0x0004A630
		public void DeclineInvitation(string invitationId)
		{
			this.FindInvitationWithId(invitationId, delegate(GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
			{
				if (invitation == null)
				{
					return;
				}
				this.mTurnBasedManager.DeclineInvitation(invitation);
			});
		}

		// Token: 0x04000BF3 RID: 3059
		private readonly TurnBasedManager mTurnBasedManager;

		// Token: 0x04000BF4 RID: 3060
		private readonly NativeClient mNativeClient;

		// Token: 0x04000BF5 RID: 3061
		private volatile Action<GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch, bool> mMatchDelegate;
	}
}
