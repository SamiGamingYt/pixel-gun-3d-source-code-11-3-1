using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200025D RID: 605
	internal class NativeTurnBasedMatch : BaseReferenceHolder
	{
		// Token: 0x0600134F RID: 4943 RVA: 0x0004F324 File Offset: 0x0004D524
		internal NativeTurnBasedMatch(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0004F330 File Offset: 0x0004D530
		internal uint AvailableAutomatchSlots()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_AutomatchingSlotsAvailable(base.SelfPtr());
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0004F340 File Offset: 0x0004D540
		internal ulong CreationTime()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_CreationTime(base.SelfPtr());
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0004F350 File Offset: 0x0004D550
		internal IEnumerable<MultiplayerParticipant> Participants()
		{
			return PInvokeUtilities.ToEnumerable<MultiplayerParticipant>(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_Length(base.SelfPtr()), (UIntPtr index) => new MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_GetElement(base.SelfPtr(), index)));
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0004F370 File Offset: 0x0004D570
		internal uint Version()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Version(base.SelfPtr());
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0004F380 File Offset: 0x0004D580
		internal uint Variant()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Variant(base.SelfPtr());
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0004F390 File Offset: 0x0004D590
		internal ParticipantResults Results()
		{
			return new ParticipantResults(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_ParticipantResults(base.SelfPtr()));
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0004F3A4 File Offset: 0x0004D5A4
		internal MultiplayerParticipant ParticipantWithId(string participantId)
		{
			foreach (MultiplayerParticipant multiplayerParticipant in this.Participants())
			{
				if (multiplayerParticipant.Id().Equals(participantId))
				{
					return multiplayerParticipant;
				}
				multiplayerParticipant.Dispose();
			}
			return null;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0004F424 File Offset: 0x0004D624
		internal MultiplayerParticipant PendingParticipant()
		{
			MultiplayerParticipant multiplayerParticipant = new MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_PendingParticipant(base.SelfPtr()));
			if (!multiplayerParticipant.Valid())
			{
				multiplayerParticipant.Dispose();
				return null;
			}
			return multiplayerParticipant;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0004F458 File Offset: 0x0004D658
		internal Types.MatchStatus MatchStatus()
		{
			return GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Status(base.SelfPtr());
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0004F468 File Offset: 0x0004D668
		internal string Description()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Description(base.SelfPtr(), out_string, size));
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0004F47C File Offset: 0x0004D67C
		internal bool HasRematchId()
		{
			string text = this.RematchId();
			return string.IsNullOrEmpty(text) || !text.Equals("(null)");
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0004F4AC File Offset: 0x0004D6AC
		internal string RematchId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_RematchId(base.SelfPtr(), out_string, size));
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0004F4C0 File Offset: 0x0004D6C0
		internal byte[] Data()
		{
			if (!GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_HasData(base.SelfPtr()))
			{
				Logger.d("Match has no data.");
				return null;
			}
			return PInvokeUtilities.OutParamsToArray<byte>((byte[] bytes, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Data(base.SelfPtr(), bytes, size));
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0004F4FC File Offset: 0x0004D6FC
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Id(base.SelfPtr(), out_string, size));
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0004F510 File Offset: 0x0004D710
		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Dispose(selfPointer);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0004F518 File Offset: 0x0004D718
		internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch AsTurnBasedMatch(string selfPlayerId)
		{
			List<Participant> list = new List<Participant>();
			string selfParticipantId = null;
			string pendingParticipantId = null;
			using (MultiplayerParticipant multiplayerParticipant = this.PendingParticipant())
			{
				if (multiplayerParticipant != null)
				{
					pendingParticipantId = multiplayerParticipant.Id();
				}
			}
			foreach (MultiplayerParticipant multiplayerParticipant2 in this.Participants())
			{
				using (multiplayerParticipant2)
				{
					using (NativePlayer nativePlayer = multiplayerParticipant2.Player())
					{
						if (nativePlayer != null && nativePlayer.Id().Equals(selfPlayerId))
						{
							selfParticipantId = multiplayerParticipant2.Id();
						}
					}
					list.Add(multiplayerParticipant2.AsParticipant());
				}
			}
			bool canRematch = this.MatchStatus() == Types.MatchStatus.COMPLETED && !this.HasRematchId();
			return new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch(this.Id(), this.Data(), canRematch, selfParticipantId, list, this.AvailableAutomatchSlots(), pendingParticipantId, NativeTurnBasedMatch.ToTurnStatus(this.MatchStatus()), NativeTurnBasedMatch.ToMatchStatus(pendingParticipantId, this.MatchStatus()), this.Variant(), this.Version());
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0004F6B0 File Offset: 0x0004D8B0
		private static GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus ToTurnStatus(Types.MatchStatus status)
		{
			switch (status)
			{
			case Types.MatchStatus.INVITED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Invited;
			case Types.MatchStatus.THEIR_TURN:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.TheirTurn;
			case Types.MatchStatus.MY_TURN:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.MyTurn;
			case Types.MatchStatus.PENDING_COMPLETION:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case Types.MatchStatus.COMPLETED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case Types.MatchStatus.CANCELED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case Types.MatchStatus.EXPIRED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			default:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Unknown;
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0004F6F8 File Offset: 0x0004D8F8
		private static GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus ToMatchStatus(string pendingParticipantId, Types.MatchStatus status)
		{
			switch (status)
			{
			case Types.MatchStatus.INVITED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
			case Types.MatchStatus.THEIR_TURN:
				return (pendingParticipantId != null) ? GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active : GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.AutoMatching;
			case Types.MatchStatus.MY_TURN:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
			case Types.MatchStatus.PENDING_COMPLETION:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;
			case Types.MatchStatus.COMPLETED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;
			case Types.MatchStatus.CANCELED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Cancelled;
			case Types.MatchStatus.EXPIRED:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Expired;
			default:
				return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Unknown;
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0004F74C File Offset: 0x0004D94C
		internal static NativeTurnBasedMatch FromPointer(IntPtr selfPointer)
		{
			if (PInvokeUtilities.IsNull(selfPointer))
			{
				return null;
			}
			return new NativeTurnBasedMatch(selfPointer);
		}
	}
}
