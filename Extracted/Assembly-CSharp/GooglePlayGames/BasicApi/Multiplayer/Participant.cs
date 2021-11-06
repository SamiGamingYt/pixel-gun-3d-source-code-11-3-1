using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x02000177 RID: 375
	public class Participant : IComparable<Participant>
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x00042314 File Offset: 0x00040514
		internal Participant(string displayName, string participantId, Participant.ParticipantStatus status, Player player, bool connectedToRoom)
		{
			this.mDisplayName = displayName;
			this.mParticipantId = participantId;
			this.mStatus = status;
			this.mPlayer = player;
			this.mIsConnectedToRoom = connectedToRoom;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0004236C File Offset: 0x0004056C
		public string DisplayName
		{
			get
			{
				return this.mDisplayName;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00042374 File Offset: 0x00040574
		public string ParticipantId
		{
			get
			{
				return this.mParticipantId;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0004237C File Offset: 0x0004057C
		public Participant.ParticipantStatus Status
		{
			get
			{
				return this.mStatus;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00042384 File Offset: 0x00040584
		public Player Player
		{
			get
			{
				return this.mPlayer;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0004238C File Offset: 0x0004058C
		public bool IsConnectedToRoom
		{
			get
			{
				return this.mIsConnectedToRoom;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00042394 File Offset: 0x00040594
		public bool IsAutomatch
		{
			get
			{
				return this.mPlayer == null;
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000423A0 File Offset: 0x000405A0
		public override string ToString()
		{
			return string.Format("[Participant: '{0}' (id {1}), status={2}, player={3}, connected={4}]", new object[]
			{
				this.mDisplayName,
				this.mParticipantId,
				this.mStatus.ToString(),
				(this.mPlayer != null) ? this.mPlayer.ToString() : "NULL",
				this.mIsConnectedToRoom
			});
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00042414 File Offset: 0x00040614
		public int CompareTo(Participant other)
		{
			return this.mParticipantId.CompareTo(other.mParticipantId);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00042428 File Offset: 0x00040628
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != typeof(Participant))
			{
				return false;
			}
			Participant participant = (Participant)obj;
			return this.mParticipantId.Equals(participant.mParticipantId);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0004247C File Offset: 0x0004067C
		public override int GetHashCode()
		{
			return (this.mParticipantId == null) ? 0 : this.mParticipantId.GetHashCode();
		}

		// Token: 0x0400099C RID: 2460
		private string mDisplayName = string.Empty;

		// Token: 0x0400099D RID: 2461
		private string mParticipantId = string.Empty;

		// Token: 0x0400099E RID: 2462
		private Participant.ParticipantStatus mStatus = Participant.ParticipantStatus.Unknown;

		// Token: 0x0400099F RID: 2463
		private Player mPlayer;

		// Token: 0x040009A0 RID: 2464
		private bool mIsConnectedToRoom;

		// Token: 0x02000178 RID: 376
		public enum ParticipantStatus
		{
			// Token: 0x040009A2 RID: 2466
			NotInvitedYet,
			// Token: 0x040009A3 RID: 2467
			Invited,
			// Token: 0x040009A4 RID: 2468
			Joined,
			// Token: 0x040009A5 RID: 2469
			Declined,
			// Token: 0x040009A6 RID: 2470
			Left,
			// Token: 0x040009A7 RID: 2471
			Finished,
			// Token: 0x040009A8 RID: 2472
			Unresponsive,
			// Token: 0x040009A9 RID: 2473
			Unknown
		}
	}
}
