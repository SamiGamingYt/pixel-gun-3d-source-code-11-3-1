using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	// Token: 0x02000175 RID: 373
	public class MatchOutcome
	{
		// Token: 0x06000C3A RID: 3130 RVA: 0x000421C4 File Offset: 0x000403C4
		public void SetParticipantResult(string participantId, MatchOutcome.ParticipantResult result, uint placement)
		{
			if (!this.mParticipantIds.Contains(participantId))
			{
				this.mParticipantIds.Add(participantId);
			}
			this.mPlacements[participantId] = placement;
			this.mResults[participantId] = result;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00042208 File Offset: 0x00040408
		public void SetParticipantResult(string participantId, MatchOutcome.ParticipantResult result)
		{
			this.SetParticipantResult(participantId, result, 0U);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00042214 File Offset: 0x00040414
		public void SetParticipantResult(string participantId, uint placement)
		{
			this.SetParticipantResult(participantId, MatchOutcome.ParticipantResult.Unset, placement);
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00042220 File Offset: 0x00040420
		public List<string> ParticipantIds
		{
			get
			{
				return this.mParticipantIds;
			}
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00042228 File Offset: 0x00040428
		public MatchOutcome.ParticipantResult GetResultFor(string participantId)
		{
			return (!this.mResults.ContainsKey(participantId)) ? MatchOutcome.ParticipantResult.Unset : this.mResults[participantId];
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00042250 File Offset: 0x00040450
		public uint GetPlacementFor(string participantId)
		{
			return (!this.mPlacements.ContainsKey(participantId)) ? 0U : this.mPlacements[participantId];
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00042278 File Offset: 0x00040478
		public override string ToString()
		{
			string str = "[MatchOutcome";
			foreach (string text in this.mParticipantIds)
			{
				str += string.Format(" {0}->({1},{2})", text, this.GetResultFor(text), this.GetPlacementFor(text));
			}
			return str + "]";
		}

		// Token: 0x04000992 RID: 2450
		public const uint PlacementUnset = 0U;

		// Token: 0x04000993 RID: 2451
		private List<string> mParticipantIds = new List<string>();

		// Token: 0x04000994 RID: 2452
		private Dictionary<string, uint> mPlacements = new Dictionary<string, uint>();

		// Token: 0x04000995 RID: 2453
		private Dictionary<string, MatchOutcome.ParticipantResult> mResults = new Dictionary<string, MatchOutcome.ParticipantResult>();

		// Token: 0x02000176 RID: 374
		public enum ParticipantResult
		{
			// Token: 0x04000997 RID: 2455
			Unset = -1,
			// Token: 0x04000998 RID: 2456
			None,
			// Token: 0x04000999 RID: 2457
			Win,
			// Token: 0x0400099A RID: 2458
			Loss,
			// Token: 0x0400099B RID: 2459
			Tie
		}
	}
}
