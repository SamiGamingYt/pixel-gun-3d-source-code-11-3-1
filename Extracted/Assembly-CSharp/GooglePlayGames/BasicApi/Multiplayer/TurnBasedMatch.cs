using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x0200017B RID: 379
	public class TurnBasedMatch
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x000424A8 File Offset: 0x000406A8
		internal TurnBasedMatch(string matchId, byte[] data, bool canRematch, string selfParticipantId, List<Participant> participants, uint availableAutomatchSlots, string pendingParticipantId, TurnBasedMatch.MatchTurnStatus turnStatus, TurnBasedMatch.MatchStatus matchStatus, uint variant, uint version)
		{
			this.mMatchId = matchId;
			this.mData = data;
			this.mCanRematch = canRematch;
			this.mSelfParticipantId = selfParticipantId;
			this.mParticipants = participants;
			this.mParticipants.Sort();
			this.mAvailableAutomatchSlots = availableAutomatchSlots;
			this.mPendingParticipantId = pendingParticipantId;
			this.mTurnStatus = turnStatus;
			this.mMatchStatus = matchStatus;
			this.mVariant = variant;
			this.mVersion = version;
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0004251C File Offset: 0x0004071C
		public string MatchId
		{
			get
			{
				return this.mMatchId;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00042524 File Offset: 0x00040724
		public byte[] Data
		{
			get
			{
				return this.mData;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0004252C File Offset: 0x0004072C
		public bool CanRematch
		{
			get
			{
				return this.mCanRematch;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00042534 File Offset: 0x00040734
		public string SelfParticipantId
		{
			get
			{
				return this.mSelfParticipantId;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0004253C File Offset: 0x0004073C
		public Participant Self
		{
			get
			{
				return this.GetParticipant(this.mSelfParticipantId);
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0004254C File Offset: 0x0004074C
		public Participant GetParticipant(string participantId)
		{
			foreach (Participant participant in this.mParticipants)
			{
				if (participant.ParticipantId.Equals(participantId))
				{
					return participant;
				}
			}
			Logger.w("Participant not found in turn-based match: " + participantId);
			return null;
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x000425D8 File Offset: 0x000407D8
		public List<Participant> Participants
		{
			get
			{
				return this.mParticipants;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x000425E0 File Offset: 0x000407E0
		public string PendingParticipantId
		{
			get
			{
				return this.mPendingParticipantId;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x000425E8 File Offset: 0x000407E8
		public Participant PendingParticipant
		{
			get
			{
				return (this.mPendingParticipantId != null) ? this.GetParticipant(this.mPendingParticipantId) : null;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00042608 File Offset: 0x00040808
		public TurnBasedMatch.MatchTurnStatus TurnStatus
		{
			get
			{
				return this.mTurnStatus;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00042610 File Offset: 0x00040810
		public TurnBasedMatch.MatchStatus Status
		{
			get
			{
				return this.mMatchStatus;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00042618 File Offset: 0x00040818
		public uint Variant
		{
			get
			{
				return this.mVariant;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00042620 File Offset: 0x00040820
		public uint Version
		{
			get
			{
				return this.mVersion;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00042628 File Offset: 0x00040828
		public uint AvailableAutomatchSlots
		{
			get
			{
				return this.mAvailableAutomatchSlots;
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00042630 File Offset: 0x00040830
		public override string ToString()
		{
			string format = "[TurnBasedMatch: mMatchId={0}, mData={1}, mCanRematch={2}, mSelfParticipantId={3}, mParticipants={4}, mPendingParticipantId={5}, mTurnStatus={6}, mMatchStatus={7}, mVariant={8}, mVersion={9}]";
			object[] array = new object[10];
			array[0] = this.mMatchId;
			array[1] = this.mData;
			array[2] = this.mCanRematch;
			array[3] = this.mSelfParticipantId;
			array[4] = string.Join(",", (from p in this.mParticipants
			select p.ToString()).ToArray<string>());
			array[5] = this.mPendingParticipantId;
			array[6] = this.mTurnStatus;
			array[7] = this.mMatchStatus;
			array[8] = this.mVariant;
			array[9] = this.mVersion;
			return string.Format(format, array);
		}

		// Token: 0x040009AA RID: 2474
		private string mMatchId;

		// Token: 0x040009AB RID: 2475
		private byte[] mData;

		// Token: 0x040009AC RID: 2476
		private bool mCanRematch;

		// Token: 0x040009AD RID: 2477
		private uint mAvailableAutomatchSlots;

		// Token: 0x040009AE RID: 2478
		private string mSelfParticipantId;

		// Token: 0x040009AF RID: 2479
		private List<Participant> mParticipants;

		// Token: 0x040009B0 RID: 2480
		private string mPendingParticipantId;

		// Token: 0x040009B1 RID: 2481
		private TurnBasedMatch.MatchTurnStatus mTurnStatus;

		// Token: 0x040009B2 RID: 2482
		private TurnBasedMatch.MatchStatus mMatchStatus;

		// Token: 0x040009B3 RID: 2483
		private uint mVariant;

		// Token: 0x040009B4 RID: 2484
		private uint mVersion;

		// Token: 0x0200017C RID: 380
		public enum MatchStatus
		{
			// Token: 0x040009B7 RID: 2487
			Active,
			// Token: 0x040009B8 RID: 2488
			AutoMatching,
			// Token: 0x040009B9 RID: 2489
			Cancelled,
			// Token: 0x040009BA RID: 2490
			Complete,
			// Token: 0x040009BB RID: 2491
			Expired,
			// Token: 0x040009BC RID: 2492
			Unknown,
			// Token: 0x040009BD RID: 2493
			Deleted
		}

		// Token: 0x0200017D RID: 381
		public enum MatchTurnStatus
		{
			// Token: 0x040009BF RID: 2495
			Complete,
			// Token: 0x040009C0 RID: 2496
			Invited,
			// Token: 0x040009C1 RID: 2497
			MyTurn,
			// Token: 0x040009C2 RID: 2498
			TheirTurn,
			// Token: 0x040009C3 RID: 2499
			Unknown
		}
	}
}
