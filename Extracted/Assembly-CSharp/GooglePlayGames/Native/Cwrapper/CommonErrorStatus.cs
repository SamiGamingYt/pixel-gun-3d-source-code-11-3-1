using System;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001C5 RID: 453
	internal static class CommonErrorStatus
	{
		// Token: 0x020001C6 RID: 454
		internal enum ResponseStatus
		{
			// Token: 0x04000AB1 RID: 2737
			VALID = 1,
			// Token: 0x04000AB2 RID: 2738
			VALID_BUT_STALE,
			// Token: 0x04000AB3 RID: 2739
			ERROR_LICENSE_CHECK_FAILED = -1,
			// Token: 0x04000AB4 RID: 2740
			ERROR_INTERNAL = -2,
			// Token: 0x04000AB5 RID: 2741
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AB6 RID: 2742
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000AB7 RID: 2743
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001C7 RID: 455
		internal enum FlushStatus
		{
			// Token: 0x04000AB9 RID: 2745
			FLUSHED = 4,
			// Token: 0x04000ABA RID: 2746
			ERROR_INTERNAL = -2,
			// Token: 0x04000ABB RID: 2747
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000ABC RID: 2748
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000ABD RID: 2749
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001C8 RID: 456
		internal enum AuthStatus
		{
			// Token: 0x04000ABF RID: 2751
			VALID = 1,
			// Token: 0x04000AC0 RID: 2752
			ERROR_INTERNAL = -2,
			// Token: 0x04000AC1 RID: 2753
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AC2 RID: 2754
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000AC3 RID: 2755
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001C9 RID: 457
		internal enum UIStatus
		{
			// Token: 0x04000AC5 RID: 2757
			VALID = 1,
			// Token: 0x04000AC6 RID: 2758
			ERROR_INTERNAL = -2,
			// Token: 0x04000AC7 RID: 2759
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AC8 RID: 2760
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000AC9 RID: 2761
			ERROR_TIMEOUT = -5,
			// Token: 0x04000ACA RID: 2762
			ERROR_CANCELED = -6,
			// Token: 0x04000ACB RID: 2763
			ERROR_UI_BUSY = -12,
			// Token: 0x04000ACC RID: 2764
			ERROR_LEFT_ROOM = -18
		}

		// Token: 0x020001CA RID: 458
		internal enum MultiplayerStatus
		{
			// Token: 0x04000ACE RID: 2766
			VALID = 1,
			// Token: 0x04000ACF RID: 2767
			VALID_BUT_STALE,
			// Token: 0x04000AD0 RID: 2768
			ERROR_INTERNAL = -2,
			// Token: 0x04000AD1 RID: 2769
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AD2 RID: 2770
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000AD3 RID: 2771
			ERROR_TIMEOUT = -5,
			// Token: 0x04000AD4 RID: 2772
			ERROR_MATCH_ALREADY_REMATCHED = -7,
			// Token: 0x04000AD5 RID: 2773
			ERROR_INACTIVE_MATCH = -8,
			// Token: 0x04000AD6 RID: 2774
			ERROR_INVALID_RESULTS = -9,
			// Token: 0x04000AD7 RID: 2775
			ERROR_INVALID_MATCH = -10,
			// Token: 0x04000AD8 RID: 2776
			ERROR_MATCH_OUT_OF_DATE = -11,
			// Token: 0x04000AD9 RID: 2777
			ERROR_REAL_TIME_ROOM_NOT_JOINED = -17
		}

		// Token: 0x020001CB RID: 459
		internal enum QuestAcceptStatus
		{
			// Token: 0x04000ADB RID: 2779
			VALID = 1,
			// Token: 0x04000ADC RID: 2780
			ERROR_INTERNAL = -2,
			// Token: 0x04000ADD RID: 2781
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000ADE RID: 2782
			ERROR_TIMEOUT = -5,
			// Token: 0x04000ADF RID: 2783
			ERROR_QUEST_NO_LONGER_AVAILABLE = -13,
			// Token: 0x04000AE0 RID: 2784
			ERROR_QUEST_NOT_STARTED = -14
		}

		// Token: 0x020001CC RID: 460
		internal enum QuestClaimMilestoneStatus
		{
			// Token: 0x04000AE2 RID: 2786
			VALID = 1,
			// Token: 0x04000AE3 RID: 2787
			ERROR_INTERNAL = -2,
			// Token: 0x04000AE4 RID: 2788
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AE5 RID: 2789
			ERROR_TIMEOUT = -5,
			// Token: 0x04000AE6 RID: 2790
			ERROR_MILESTONE_ALREADY_CLAIMED = -15,
			// Token: 0x04000AE7 RID: 2791
			ERROR_MILESTONE_CLAIM_FAILED = -16
		}

		// Token: 0x020001CD RID: 461
		internal enum SnapshotOpenStatus
		{
			// Token: 0x04000AE9 RID: 2793
			VALID = 1,
			// Token: 0x04000AEA RID: 2794
			VALID_WITH_CONFLICT = 3,
			// Token: 0x04000AEB RID: 2795
			ERROR_INTERNAL = -2,
			// Token: 0x04000AEC RID: 2796
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AED RID: 2797
			ERROR_TIMEOUT = -5
		}
	}
}
