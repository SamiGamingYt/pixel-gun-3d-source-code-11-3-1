using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000244 RID: 580
	internal class MultiplayerInvitation : BaseReferenceHolder
	{
		// Token: 0x06001251 RID: 4689 RVA: 0x0004D990 File Offset: 0x0004BB90
		internal MultiplayerInvitation(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0004D99C File Offset: 0x0004BB9C
		internal MultiplayerParticipant Inviter()
		{
			MultiplayerParticipant multiplayerParticipant = new MultiplayerParticipant(MultiplayerInvitation.MultiplayerInvitation_InvitingParticipant(base.SelfPtr()));
			if (!multiplayerParticipant.Valid())
			{
				multiplayerParticipant.Dispose();
				return null;
			}
			return multiplayerParticipant;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0004D9D0 File Offset: 0x0004BBD0
		internal uint Variant()
		{
			return MultiplayerInvitation.MultiplayerInvitation_Variant(base.SelfPtr());
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0004D9E0 File Offset: 0x0004BBE0
		internal Types.MultiplayerInvitationType Type()
		{
			return MultiplayerInvitation.MultiplayerInvitation_Type(base.SelfPtr());
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0004D9F0 File Offset: 0x0004BBF0
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => MultiplayerInvitation.MultiplayerInvitation_Id(base.SelfPtr(), out_string, size));
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0004DA04 File Offset: 0x0004BC04
		protected override void CallDispose(HandleRef selfPointer)
		{
			MultiplayerInvitation.MultiplayerInvitation_Dispose(selfPointer);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0004DA0C File Offset: 0x0004BC0C
		internal uint AutomatchingSlots()
		{
			return MultiplayerInvitation.MultiplayerInvitation_AutomatchingSlotsAvailable(base.SelfPtr());
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0004DA1C File Offset: 0x0004BC1C
		internal uint ParticipantCount()
		{
			return MultiplayerInvitation.MultiplayerInvitation_Participants_Length(base.SelfPtr()).ToUInt32();
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0004DA3C File Offset: 0x0004BC3C
		private static Invitation.InvType ToInvType(Types.MultiplayerInvitationType invitationType)
		{
			if (invitationType == Types.MultiplayerInvitationType.TURN_BASED)
			{
				return Invitation.InvType.TurnBased;
			}
			if (invitationType != Types.MultiplayerInvitationType.REAL_TIME)
			{
				Logger.d("Found unknown invitation type: " + invitationType);
				return Invitation.InvType.Unknown;
			}
			return Invitation.InvType.RealTime;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0004DA78 File Offset: 0x0004BC78
		internal Invitation AsInvitation()
		{
			Invitation.InvType invType = MultiplayerInvitation.ToInvType(this.Type());
			string invId = this.Id();
			int variant = (int)this.Variant();
			Participant inviter;
			using (MultiplayerParticipant multiplayerParticipant = this.Inviter())
			{
				inviter = ((multiplayerParticipant != null) ? multiplayerParticipant.AsParticipant() : null);
			}
			return new Invitation(invType, invId, inviter, variant);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0004DAF8 File Offset: 0x0004BCF8
		internal static MultiplayerInvitation FromPointer(IntPtr selfPointer)
		{
			if (PInvokeUtilities.IsNull(selfPointer))
			{
				return null;
			}
			return new MultiplayerInvitation(selfPointer);
		}
	}
}
