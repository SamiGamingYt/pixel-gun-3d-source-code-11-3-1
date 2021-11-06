using System;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x02000203 RID: 515
	internal static class Types
	{
		// Token: 0x02000204 RID: 516
		internal enum DataSource
		{
			// Token: 0x04000B3D RID: 2877
			CACHE_OR_NETWORK = 1,
			// Token: 0x04000B3E RID: 2878
			NETWORK_ONLY
		}

		// Token: 0x02000205 RID: 517
		internal enum LogLevel
		{
			// Token: 0x04000B40 RID: 2880
			VERBOSE = 1,
			// Token: 0x04000B41 RID: 2881
			INFO,
			// Token: 0x04000B42 RID: 2882
			WARNING,
			// Token: 0x04000B43 RID: 2883
			ERROR
		}

		// Token: 0x02000206 RID: 518
		internal enum AuthOperation
		{
			// Token: 0x04000B45 RID: 2885
			SIGN_IN = 1,
			// Token: 0x04000B46 RID: 2886
			SIGN_OUT
		}

		// Token: 0x02000207 RID: 519
		internal enum ImageResolution
		{
			// Token: 0x04000B48 RID: 2888
			ICON = 1,
			// Token: 0x04000B49 RID: 2889
			HI_RES
		}

		// Token: 0x02000208 RID: 520
		internal enum AchievementType
		{
			// Token: 0x04000B4B RID: 2891
			STANDARD = 1,
			// Token: 0x04000B4C RID: 2892
			INCREMENTAL
		}

		// Token: 0x02000209 RID: 521
		internal enum AchievementState
		{
			// Token: 0x04000B4E RID: 2894
			HIDDEN = 1,
			// Token: 0x04000B4F RID: 2895
			REVEALED,
			// Token: 0x04000B50 RID: 2896
			UNLOCKED
		}

		// Token: 0x0200020A RID: 522
		internal enum EventVisibility
		{
			// Token: 0x04000B52 RID: 2898
			HIDDEN = 1,
			// Token: 0x04000B53 RID: 2899
			REVEALED
		}

		// Token: 0x0200020B RID: 523
		internal enum LeaderboardOrder
		{
			// Token: 0x04000B55 RID: 2901
			LARGER_IS_BETTER = 1,
			// Token: 0x04000B56 RID: 2902
			SMALLER_IS_BETTER
		}

		// Token: 0x0200020C RID: 524
		internal enum LeaderboardStart
		{
			// Token: 0x04000B58 RID: 2904
			TOP_SCORES = 1,
			// Token: 0x04000B59 RID: 2905
			PLAYER_CENTERED
		}

		// Token: 0x0200020D RID: 525
		internal enum LeaderboardTimeSpan
		{
			// Token: 0x04000B5B RID: 2907
			DAILY = 1,
			// Token: 0x04000B5C RID: 2908
			WEEKLY,
			// Token: 0x04000B5D RID: 2909
			ALL_TIME
		}

		// Token: 0x0200020E RID: 526
		internal enum LeaderboardCollection
		{
			// Token: 0x04000B5F RID: 2911
			PUBLIC = 1,
			// Token: 0x04000B60 RID: 2912
			SOCIAL
		}

		// Token: 0x0200020F RID: 527
		internal enum ParticipantStatus
		{
			// Token: 0x04000B62 RID: 2914
			INVITED = 1,
			// Token: 0x04000B63 RID: 2915
			JOINED,
			// Token: 0x04000B64 RID: 2916
			DECLINED,
			// Token: 0x04000B65 RID: 2917
			LEFT,
			// Token: 0x04000B66 RID: 2918
			NOT_INVITED_YET,
			// Token: 0x04000B67 RID: 2919
			FINISHED,
			// Token: 0x04000B68 RID: 2920
			UNRESPONSIVE
		}

		// Token: 0x02000210 RID: 528
		internal enum MatchResult
		{
			// Token: 0x04000B6A RID: 2922
			DISAGREED = 1,
			// Token: 0x04000B6B RID: 2923
			DISCONNECTED,
			// Token: 0x04000B6C RID: 2924
			LOSS,
			// Token: 0x04000B6D RID: 2925
			NONE,
			// Token: 0x04000B6E RID: 2926
			TIE,
			// Token: 0x04000B6F RID: 2927
			WIN
		}

		// Token: 0x02000211 RID: 529
		internal enum MatchStatus
		{
			// Token: 0x04000B71 RID: 2929
			INVITED = 1,
			// Token: 0x04000B72 RID: 2930
			THEIR_TURN,
			// Token: 0x04000B73 RID: 2931
			MY_TURN,
			// Token: 0x04000B74 RID: 2932
			PENDING_COMPLETION,
			// Token: 0x04000B75 RID: 2933
			COMPLETED,
			// Token: 0x04000B76 RID: 2934
			CANCELED,
			// Token: 0x04000B77 RID: 2935
			EXPIRED
		}

		// Token: 0x02000212 RID: 530
		internal enum QuestState
		{
			// Token: 0x04000B79 RID: 2937
			UPCOMING = 1,
			// Token: 0x04000B7A RID: 2938
			OPEN,
			// Token: 0x04000B7B RID: 2939
			ACCEPTED,
			// Token: 0x04000B7C RID: 2940
			COMPLETED,
			// Token: 0x04000B7D RID: 2941
			EXPIRED,
			// Token: 0x04000B7E RID: 2942
			FAILED
		}

		// Token: 0x02000213 RID: 531
		internal enum QuestMilestoneState
		{
			// Token: 0x04000B80 RID: 2944
			NOT_STARTED = 1,
			// Token: 0x04000B81 RID: 2945
			NOT_COMPLETED,
			// Token: 0x04000B82 RID: 2946
			COMPLETED_NOT_CLAIMED,
			// Token: 0x04000B83 RID: 2947
			CLAIMED
		}

		// Token: 0x02000214 RID: 532
		internal enum MultiplayerEvent
		{
			// Token: 0x04000B85 RID: 2949
			UPDATED = 1,
			// Token: 0x04000B86 RID: 2950
			UPDATED_FROM_APP_LAUNCH,
			// Token: 0x04000B87 RID: 2951
			REMOVED
		}

		// Token: 0x02000215 RID: 533
		internal enum MultiplayerInvitationType
		{
			// Token: 0x04000B89 RID: 2953
			TURN_BASED = 1,
			// Token: 0x04000B8A RID: 2954
			REAL_TIME
		}

		// Token: 0x02000216 RID: 534
		internal enum RealTimeRoomStatus
		{
			// Token: 0x04000B8C RID: 2956
			INVITING = 1,
			// Token: 0x04000B8D RID: 2957
			CONNECTING,
			// Token: 0x04000B8E RID: 2958
			AUTO_MATCHING,
			// Token: 0x04000B8F RID: 2959
			ACTIVE,
			// Token: 0x04000B90 RID: 2960
			DELETED
		}

		// Token: 0x02000217 RID: 535
		internal enum SnapshotConflictPolicy
		{
			// Token: 0x04000B92 RID: 2962
			MANUAL = 1,
			// Token: 0x04000B93 RID: 2963
			LONGEST_PLAYTIME,
			// Token: 0x04000B94 RID: 2964
			LAST_KNOWN_GOOD,
			// Token: 0x04000B95 RID: 2965
			MOST_RECENTLY_MODIFIED
		}
	}
}
