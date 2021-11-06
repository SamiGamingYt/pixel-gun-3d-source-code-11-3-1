using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x02000173 RID: 371
	public class Invitation
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x00042104 File Offset: 0x00040304
		internal Invitation(Invitation.InvType invType, string invId, Participant inviter, int variant)
		{
			this.mInvitationType = invType;
			this.mInvitationId = invId;
			this.mInviter = inviter;
			this.mVariant = variant;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0004212C File Offset: 0x0004032C
		public Invitation.InvType InvitationType
		{
			get
			{
				return this.mInvitationType;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00042134 File Offset: 0x00040334
		public string InvitationId
		{
			get
			{
				return this.mInvitationId;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0004213C File Offset: 0x0004033C
		public Participant Inviter
		{
			get
			{
				return this.mInviter;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00042144 File Offset: 0x00040344
		public int Variant
		{
			get
			{
				return this.mVariant;
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0004214C File Offset: 0x0004034C
		public override string ToString()
		{
			return string.Format("[Invitation: InvitationType={0}, InvitationId={1}, Inviter={2}, Variant={3}]", new object[]
			{
				this.InvitationType,
				this.InvitationId,
				this.Inviter,
				this.Variant
			});
		}

		// Token: 0x0400098A RID: 2442
		private Invitation.InvType mInvitationType;

		// Token: 0x0400098B RID: 2443
		private string mInvitationId;

		// Token: 0x0400098C RID: 2444
		private Participant mInviter;

		// Token: 0x0400098D RID: 2445
		private int mVariant;

		// Token: 0x02000174 RID: 372
		public enum InvType
		{
			// Token: 0x0400098F RID: 2447
			RealTime,
			// Token: 0x04000990 RID: 2448
			TurnBased,
			// Token: 0x04000991 RID: 2449
			Unknown
		}
	}
}
