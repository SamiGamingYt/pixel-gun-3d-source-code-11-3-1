using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000261 RID: 609
	internal class ParticipantResults : BaseReferenceHolder
	{
		// Token: 0x06001394 RID: 5012 RVA: 0x0004FE3C File Offset: 0x0004E03C
		internal ParticipantResults(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0004FE48 File Offset: 0x0004E048
		internal bool HasResultsForParticipant(string participantId)
		{
			return ParticipantResults.ParticipantResults_HasResultsForParticipant(base.SelfPtr(), participantId);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0004FE58 File Offset: 0x0004E058
		internal uint PlacingForParticipant(string participantId)
		{
			return ParticipantResults.ParticipantResults_PlaceForParticipant(base.SelfPtr(), participantId);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0004FE68 File Offset: 0x0004E068
		internal Types.MatchResult ResultsForParticipant(string participantId)
		{
			return ParticipantResults.ParticipantResults_MatchResultForParticipant(base.SelfPtr(), participantId);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0004FE78 File Offset: 0x0004E078
		internal ParticipantResults WithResult(string participantId, uint placing, Types.MatchResult result)
		{
			return new ParticipantResults(ParticipantResults.ParticipantResults_WithResult(base.SelfPtr(), participantId, placing, result));
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0004FE90 File Offset: 0x0004E090
		protected override void CallDispose(HandleRef selfPointer)
		{
			ParticipantResults.ParticipantResults_Dispose(selfPointer);
		}
	}
}
