using System;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001F4 RID: 500
	internal static class Status
	{
		// Token: 0x020001F5 RID: 501
		internal enum ResponseStatus
		{
			// Token: 0x04000AFA RID: 2810
			VALID = 1,
			// Token: 0x04000AFB RID: 2811
			VALID_BUT_STALE,
			// Token: 0x04000AFC RID: 2812
			ERROR_LICENSE_CHECK_FAILED = -1,
			// Token: 0x04000AFD RID: 2813
			ERROR_INTERNAL = -2,
			// Token: 0x04000AFE RID: 2814
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000AFF RID: 2815
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000B00 RID: 2816
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001F6 RID: 502
		internal enum FlushStatus
		{
			// Token: 0x04000B02 RID: 2818
			FLUSHED = 4,
			// Token: 0x04000B03 RID: 2819
			ERROR_INTERNAL = -2,
			// Token: 0x04000B04 RID: 2820
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B05 RID: 2821
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000B06 RID: 2822
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001F7 RID: 503
		internal enum AuthStatus
		{
			// Token: 0x04000B08 RID: 2824
			VALID = 1,
			// Token: 0x04000B09 RID: 2825
			ERROR_INTERNAL = -2,
			// Token: 0x04000B0A RID: 2826
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B0B RID: 2827
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000B0C RID: 2828
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001F8 RID: 504
		internal enum UIStatus
		{
			// Token: 0x04000B0E RID: 2830
			VALID = 1,
			// Token: 0x04000B0F RID: 2831
			ERROR_INTERNAL = -2,
			// Token: 0x04000B10 RID: 2832
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B11 RID: 2833
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000B12 RID: 2834
			ERROR_TIMEOUT = -5,
			// Token: 0x04000B13 RID: 2835
			ERROR_CANCELED = -6,
			// Token: 0x04000B14 RID: 2836
			ERROR_UI_BUSY = -12,
			// Token: 0x04000B15 RID: 2837
			ERROR_LEFT_ROOM = -18
		}

		// Token: 0x020001F9 RID: 505
		internal enum MultiplayerStatus
		{
			// Token: 0x04000B17 RID: 2839
			VALID = 1,
			// Token: 0x04000B18 RID: 2840
			VALID_BUT_STALE,
			// Token: 0x04000B19 RID: 2841
			ERROR_INTERNAL = -2,
			// Token: 0x04000B1A RID: 2842
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B1B RID: 2843
			ERROR_VERSION_UPDATE_REQUIRED = -4,
			// Token: 0x04000B1C RID: 2844
			ERROR_TIMEOUT = -5,
			// Token: 0x04000B1D RID: 2845
			ERROR_MATCH_ALREADY_REMATCHED = -7,
			// Token: 0x04000B1E RID: 2846
			ERROR_INACTIVE_MATCH = -8,
			// Token: 0x04000B1F RID: 2847
			ERROR_INVALID_RESULTS = -9,
			// Token: 0x04000B20 RID: 2848
			ERROR_INVALID_MATCH = -10,
			// Token: 0x04000B21 RID: 2849
			ERROR_MATCH_OUT_OF_DATE = -11,
			// Token: 0x04000B22 RID: 2850
			ERROR_REAL_TIME_ROOM_NOT_JOINED = -17
		}

		// Token: 0x020001FA RID: 506
		internal enum QuestAcceptStatus
		{
			// Token: 0x04000B24 RID: 2852
			VALID = 1,
			// Token: 0x04000B25 RID: 2853
			ERROR_INTERNAL = -2,
			// Token: 0x04000B26 RID: 2854
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B27 RID: 2855
			ERROR_TIMEOUT = -5,
			// Token: 0x04000B28 RID: 2856
			ERROR_QUEST_NO_LONGER_AVAILABLE = -13,
			// Token: 0x04000B29 RID: 2857
			ERROR_QUEST_NOT_STARTED = -14
		}

		// Token: 0x020001FB RID: 507
		internal enum QuestClaimMilestoneStatus
		{
			// Token: 0x04000B2B RID: 2859
			VALID = 1,
			// Token: 0x04000B2C RID: 2860
			ERROR_INTERNAL = -2,
			// Token: 0x04000B2D RID: 2861
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B2E RID: 2862
			ERROR_TIMEOUT = -5,
			// Token: 0x04000B2F RID: 2863
			ERROR_MILESTONE_ALREADY_CLAIMED = -15,
			// Token: 0x04000B30 RID: 2864
			ERROR_MILESTONE_CLAIM_FAILED = -16
		}

		// Token: 0x020001FC RID: 508
		internal enum CommonErrorStatus
		{
			// Token: 0x04000B32 RID: 2866
			ERROR_INTERNAL = -2,
			// Token: 0x04000B33 RID: 2867
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B34 RID: 2868
			ERROR_TIMEOUT = -5
		}

		// Token: 0x020001FD RID: 509
		internal enum SnapshotOpenStatus
		{
			// Token: 0x04000B36 RID: 2870
			VALID = 1,
			// Token: 0x04000B37 RID: 2871
			VALID_WITH_CONFLICT = 3,
			// Token: 0x04000B38 RID: 2872
			ERROR_INTERNAL = -2,
			// Token: 0x04000B39 RID: 2873
			ERROR_NOT_AUTHORIZED = -3,
			// Token: 0x04000B3A RID: 2874
			ERROR_TIMEOUT = -5
		}
	}
}
