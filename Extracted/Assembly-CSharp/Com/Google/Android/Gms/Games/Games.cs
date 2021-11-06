using System;
using Com.Google.Android.Gms.Common.Api;
using Com.Google.Android.Gms.Games.Stats;
using Google.Developers;

namespace Com.Google.Android.Gms.Games
{
	// Token: 0x020001B6 RID: 438
	public class Games : JavaObjWrapper
	{
		// Token: 0x06000E58 RID: 3672 RVA: 0x00046A24 File Offset: 0x00044C24
		public Games(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00046A30 File Offset: 0x00044C30
		public static string EXTRA_PLAYER_IDS
		{
			get
			{
				return JavaObjWrapper.GetStaticStringField("com/google/android/gms/games/Games", "EXTRA_PLAYER_IDS");
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00046A44 File Offset: 0x00044C44
		public static string EXTRA_STATUS
		{
			get
			{
				return JavaObjWrapper.GetStaticStringField("com/google/android/gms/games/Games", "EXTRA_STATUS");
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00046A58 File Offset: 0x00044C58
		public static object SCOPE_GAMES
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "SCOPE_GAMES", "Lcom/google/android/gms/common/api/Scope;");
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x00046A70 File Offset: 0x00044C70
		public static object API
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "API", "Lcom/google/android/gms/common/api/Api;");
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x00046A88 File Offset: 0x00044C88
		public static object GamesMetadata
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "GamesMetadata", "Lcom/google/android/gms/games/GamesMetadata;");
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x00046AA0 File Offset: 0x00044CA0
		public static object Achievements
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Achievements", "Lcom/google/android/gms/games/achievement/Achievements;");
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00046AB8 File Offset: 0x00044CB8
		public static object Events
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Events", "Lcom/google/android/gms/games/event/Events;");
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00046AD0 File Offset: 0x00044CD0
		public static object Leaderboards
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Leaderboards", "Lcom/google/android/gms/games/leaderboard/Leaderboards;");
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00046AE8 File Offset: 0x00044CE8
		public static object Invitations
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Invitations", "Lcom/google/android/gms/games/multiplayer/Invitations;");
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x00046B00 File Offset: 0x00044D00
		public static object TurnBasedMultiplayer
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "TurnBasedMultiplayer", "Lcom/google/android/gms/games/multiplayer/turnbased/TurnBasedMultiplayer;");
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00046B18 File Offset: 0x00044D18
		public static object RealTimeMultiplayer
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "RealTimeMultiplayer", "Lcom/google/android/gms/games/multiplayer/realtime/RealTimeMultiplayer;");
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x00046B30 File Offset: 0x00044D30
		public static object Players
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Players", "Lcom/google/android/gms/games/Players;");
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00046B48 File Offset: 0x00044D48
		public static object Notifications
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Notifications", "Lcom/google/android/gms/games/Notifications;");
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00046B60 File Offset: 0x00044D60
		public static object Quests
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Quests", "Lcom/google/android/gms/games/quest/Quests;");
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00046B78 File Offset: 0x00044D78
		public static object Requests
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Requests", "Lcom/google/android/gms/games/request/Requests;");
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00046B90 File Offset: 0x00044D90
		public static object Snapshots
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Snapshots", "Lcom/google/android/gms/games/snapshot/Snapshots;");
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00046BA8 File Offset: 0x00044DA8
		public static StatsObject Stats
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<StatsObject>("com/google/android/gms/games/Games", "Stats", "Lcom/google/android/gms/games/stats/Stats;");
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00046BC0 File Offset: 0x00044DC0
		public static string getAppId(GoogleApiClient arg_GoogleApiClient_1)
		{
			return JavaObjWrapper.StaticInvokeCall<string>("com/google/android/gms/games/Games", "getAppId", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Ljava/lang/String;", new object[]
			{
				arg_GoogleApiClient_1
			});
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00046BE0 File Offset: 0x00044DE0
		public static string getCurrentAccountName(GoogleApiClient arg_GoogleApiClient_1)
		{
			return JavaObjWrapper.StaticInvokeCall<string>("com/google/android/gms/games/Games", "getCurrentAccountName", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Ljava/lang/String;", new object[]
			{
				arg_GoogleApiClient_1
			});
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00046C00 File Offset: 0x00044E00
		public static int getSdkVariant(GoogleApiClient arg_GoogleApiClient_1)
		{
			return JavaObjWrapper.StaticInvokeCall<int>("com/google/android/gms/games/Games", "getSdkVariant", "(Lcom/google/android/gms/common/api/GoogleApiClient;)I", new object[]
			{
				arg_GoogleApiClient_1
			});
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00046C20 File Offset: 0x00044E20
		public static object getSettingsIntent(GoogleApiClient arg_GoogleApiClient_1)
		{
			return JavaObjWrapper.StaticInvokeCall<object>("com/google/android/gms/games/Games", "getSettingsIntent", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Landroid/content/Intent;", new object[]
			{
				arg_GoogleApiClient_1
			});
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00046C40 File Offset: 0x00044E40
		public static void setGravityForPopups(GoogleApiClient arg_GoogleApiClient_1, int arg_int_2)
		{
			JavaObjWrapper.StaticInvokeCallVoid("com/google/android/gms/games/Games", "setGravityForPopups", "(Lcom/google/android/gms/common/api/GoogleApiClient;I)V", new object[]
			{
				arg_GoogleApiClient_1,
				arg_int_2
			});
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00046C6C File Offset: 0x00044E6C
		public static void setViewForPopups(GoogleApiClient arg_GoogleApiClient_1, object arg_object_2)
		{
			JavaObjWrapper.StaticInvokeCallVoid("com/google/android/gms/games/Games", "setViewForPopups", "(Lcom/google/android/gms/common/api/GoogleApiClient;Landroid/view/View;)V", new object[]
			{
				arg_GoogleApiClient_1,
				arg_object_2
			});
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00046C9C File Offset: 0x00044E9C
		public static PendingResult<Status> signOut(GoogleApiClient arg_GoogleApiClient_1)
		{
			return JavaObjWrapper.StaticInvokeCall<PendingResult<Status>>("com/google/android/gms/games/Games", "signOut", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Lcom/google/android/gms/common/api/PendingResult;", new object[]
			{
				arg_GoogleApiClient_1
			});
		}

		// Token: 0x04000AAB RID: 2731
		private const string CLASS_NAME = "com/google/android/gms/games/Games";
	}
}
