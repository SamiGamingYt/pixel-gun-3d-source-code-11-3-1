using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000245 RID: 581
	internal class MultiplayerParticipant : BaseReferenceHolder
	{
		// Token: 0x0600125D RID: 4701 RVA: 0x0004DB20 File Offset: 0x0004BD20
		internal MultiplayerParticipant(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0004DB80 File Offset: 0x0004BD80
		internal Types.ParticipantStatus Status()
		{
			return MultiplayerParticipant.MultiplayerParticipant_Status(base.SelfPtr());
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0004DB90 File Offset: 0x0004BD90
		internal bool IsConnectedToRoom()
		{
			return MultiplayerParticipant.MultiplayerParticipant_IsConnectedToRoom(base.SelfPtr()) || this.Status() == Types.ParticipantStatus.JOINED;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0004DBB0 File Offset: 0x0004BDB0
		internal string DisplayName()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => MultiplayerParticipant.MultiplayerParticipant_DisplayName(base.SelfPtr(), out_string, size));
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0004DBC4 File Offset: 0x0004BDC4
		internal NativePlayer Player()
		{
			if (!MultiplayerParticipant.MultiplayerParticipant_HasPlayer(base.SelfPtr()))
			{
				return null;
			}
			return new NativePlayer(MultiplayerParticipant.MultiplayerParticipant_Player(base.SelfPtr()));
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0004DBF4 File Offset: 0x0004BDF4
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => MultiplayerParticipant.MultiplayerParticipant_Id(base.SelfPtr(), out_string, size));
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0004DC08 File Offset: 0x0004BE08
		internal bool Valid()
		{
			return MultiplayerParticipant.MultiplayerParticipant_Valid(base.SelfPtr());
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0004DC18 File Offset: 0x0004BE18
		protected override void CallDispose(HandleRef selfPointer)
		{
			MultiplayerParticipant.MultiplayerParticipant_Dispose(selfPointer);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0004DC20 File Offset: 0x0004BE20
		internal Participant AsParticipant()
		{
			NativePlayer nativePlayer = this.Player();
			return new Participant(this.DisplayName(), this.Id(), MultiplayerParticipant.StatusConversion[this.Status()], (nativePlayer != null) ? nativePlayer.AsPlayer() : null, this.IsConnectedToRoom());
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0004DC70 File Offset: 0x0004BE70
		internal static MultiplayerParticipant FromPointer(IntPtr pointer)
		{
			if (PInvokeUtilities.IsNull(pointer))
			{
				return null;
			}
			return new MultiplayerParticipant(pointer);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0004DC88 File Offset: 0x0004BE88
		internal static MultiplayerParticipant AutomatchingSentinel()
		{
			return new MultiplayerParticipant(Sentinels.Sentinels_AutomatchingParticipant());
		}

		// Token: 0x04000C01 RID: 3073
		private static readonly Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> StatusConversion = new Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus>
		{
			{
				Types.ParticipantStatus.INVITED,
				Participant.ParticipantStatus.Invited
			},
			{
				Types.ParticipantStatus.JOINED,
				Participant.ParticipantStatus.Joined
			},
			{
				Types.ParticipantStatus.DECLINED,
				Participant.ParticipantStatus.Declined
			},
			{
				Types.ParticipantStatus.LEFT,
				Participant.ParticipantStatus.Left
			},
			{
				Types.ParticipantStatus.NOT_INVITED_YET,
				Participant.ParticipantStatus.NotInvitedYet
			},
			{
				Types.ParticipantStatus.FINISHED,
				Participant.ParticipantStatus.Finished
			},
			{
				Types.ParticipantStatus.UNRESPONSIVE,
				Participant.ParticipantStatus.Unresponsive
			}
		};
	}
}
