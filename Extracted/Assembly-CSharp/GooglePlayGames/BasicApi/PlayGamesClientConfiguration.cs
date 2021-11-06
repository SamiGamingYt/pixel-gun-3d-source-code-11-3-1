using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000189 RID: 393
	public struct PlayGamesClientConfiguration
	{
		// Token: 0x06000CA7 RID: 3239 RVA: 0x0004299C File Offset: 0x00040B9C
		private PlayGamesClientConfiguration(PlayGamesClientConfiguration.Builder builder)
		{
			this.mEnableSavedGames = builder.HasEnableSaveGames();
			this.mInvitationDelegate = builder.GetInvitationDelegate();
			this.mMatchDelegate = builder.GetMatchDelegate();
			this.mPermissionRationale = builder.GetPermissionRationale();
			this.mRequireGooglePlus = builder.HasRequireGooglePlus();
			this.mScopes = builder.getScopes();
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00042A10 File Offset: 0x00040C10
		public bool EnableSavedGames
		{
			get
			{
				return this.mEnableSavedGames;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00042A18 File Offset: 0x00040C18
		public bool RequireGooglePlus
		{
			get
			{
				return this.mRequireGooglePlus;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00042A20 File Offset: 0x00040C20
		public string[] Scopes
		{
			get
			{
				return this.mScopes;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00042A28 File Offset: 0x00040C28
		public InvitationReceivedDelegate InvitationDelegate
		{
			get
			{
				return this.mInvitationDelegate;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00042A30 File Offset: 0x00040C30
		public MatchDelegate MatchDelegate
		{
			get
			{
				return this.mMatchDelegate;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00042A38 File Offset: 0x00040C38
		public string PermissionRationale
		{
			get
			{
				return this.mPermissionRationale;
			}
		}

		// Token: 0x040009E0 RID: 2528
		public static readonly PlayGamesClientConfiguration DefaultConfiguration = new PlayGamesClientConfiguration.Builder().WithPermissionRationale("Select email address to send to this game or hit cancel to not share.").Build();

		// Token: 0x040009E1 RID: 2529
		private readonly bool mEnableSavedGames;

		// Token: 0x040009E2 RID: 2530
		private readonly bool mRequireGooglePlus;

		// Token: 0x040009E3 RID: 2531
		private readonly string[] mScopes;

		// Token: 0x040009E4 RID: 2532
		private readonly InvitationReceivedDelegate mInvitationDelegate;

		// Token: 0x040009E5 RID: 2533
		private readonly MatchDelegate mMatchDelegate;

		// Token: 0x040009E6 RID: 2534
		private readonly string mPermissionRationale;

		// Token: 0x0200018A RID: 394
		public class Builder
		{
			// Token: 0x06000CB0 RID: 3248 RVA: 0x00042A9C File Offset: 0x00040C9C
			public PlayGamesClientConfiguration.Builder EnableSavedGames()
			{
				this.mEnableSaveGames = true;
				return this;
			}

			// Token: 0x06000CB1 RID: 3249 RVA: 0x00042AA8 File Offset: 0x00040CA8
			public PlayGamesClientConfiguration.Builder RequireGooglePlus()
			{
				this.mRequireGooglePlus = true;
				return this;
			}

			// Token: 0x06000CB2 RID: 3250 RVA: 0x00042AB4 File Offset: 0x00040CB4
			public PlayGamesClientConfiguration.Builder AddOauthScope(string scope)
			{
				if (this.mScopes == null)
				{
					this.mScopes = new List<string>();
				}
				this.mScopes.Add(scope);
				return this;
			}

			// Token: 0x06000CB3 RID: 3251 RVA: 0x00042ADC File Offset: 0x00040CDC
			public PlayGamesClientConfiguration.Builder WithInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
			{
				this.mInvitationDelegate = Misc.CheckNotNull<InvitationReceivedDelegate>(invitationDelegate);
				return this;
			}

			// Token: 0x06000CB4 RID: 3252 RVA: 0x00042AEC File Offset: 0x00040CEC
			public PlayGamesClientConfiguration.Builder WithMatchDelegate(MatchDelegate matchDelegate)
			{
				this.mMatchDelegate = Misc.CheckNotNull<MatchDelegate>(matchDelegate);
				return this;
			}

			// Token: 0x06000CB5 RID: 3253 RVA: 0x00042AFC File Offset: 0x00040CFC
			public PlayGamesClientConfiguration.Builder WithPermissionRationale(string rationale)
			{
				this.mRationale = rationale;
				return this;
			}

			// Token: 0x06000CB6 RID: 3254 RVA: 0x00042B08 File Offset: 0x00040D08
			public PlayGamesClientConfiguration Build()
			{
				this.mRequireGooglePlus = GameInfo.RequireGooglePlus();
				return new PlayGamesClientConfiguration(this);
			}

			// Token: 0x06000CB7 RID: 3255 RVA: 0x00042B1C File Offset: 0x00040D1C
			internal bool HasEnableSaveGames()
			{
				return this.mEnableSaveGames;
			}

			// Token: 0x06000CB8 RID: 3256 RVA: 0x00042B24 File Offset: 0x00040D24
			internal bool HasRequireGooglePlus()
			{
				return this.mRequireGooglePlus;
			}

			// Token: 0x06000CB9 RID: 3257 RVA: 0x00042B2C File Offset: 0x00040D2C
			internal string[] getScopes()
			{
				return (this.mScopes != null) ? this.mScopes.ToArray() : new string[0];
			}

			// Token: 0x06000CBA RID: 3258 RVA: 0x00042B50 File Offset: 0x00040D50
			internal MatchDelegate GetMatchDelegate()
			{
				return this.mMatchDelegate;
			}

			// Token: 0x06000CBB RID: 3259 RVA: 0x00042B58 File Offset: 0x00040D58
			internal InvitationReceivedDelegate GetInvitationDelegate()
			{
				return this.mInvitationDelegate;
			}

			// Token: 0x06000CBC RID: 3260 RVA: 0x00042B60 File Offset: 0x00040D60
			internal string GetPermissionRationale()
			{
				return this.mRationale;
			}

			// Token: 0x040009E7 RID: 2535
			private bool mEnableSaveGames;

			// Token: 0x040009E8 RID: 2536
			private bool mRequireGooglePlus;

			// Token: 0x040009E9 RID: 2537
			private List<string> mScopes;

			// Token: 0x040009EA RID: 2538
			private InvitationReceivedDelegate mInvitationDelegate = delegate(Invitation A_0, bool A_1)
			{
			};

			// Token: 0x040009EB RID: 2539
			private MatchDelegate mMatchDelegate = delegate(TurnBasedMatch A_0, bool A_1)
			{
			};

			// Token: 0x040009EC RID: 2540
			private string mRationale;
		}
	}
}
