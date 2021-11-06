using System;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	// Token: 0x020001A1 RID: 417
	public class PlayGamesLocalUser : PlayGamesUserProfile, IUserProfile, ILocalUser
	{
		// Token: 0x06000D47 RID: 3399 RVA: 0x000434C0 File Offset: 0x000416C0
		internal PlayGamesLocalUser(PlayGamesPlatform plaf) : base("localUser", string.Empty, string.Empty)
		{
			this.mPlatform = plaf;
			this.emailAddress = null;
			this.mStats = null;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000434F8 File Offset: 0x000416F8
		public void Authenticate(Action<bool> callback)
		{
			this.mPlatform.Authenticate(callback);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00043508 File Offset: 0x00041708
		public void Authenticate(Action<bool> callback, bool silent)
		{
			this.mPlatform.Authenticate(callback, silent);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00043518 File Offset: 0x00041718
		public void LoadFriends(Action<bool> callback)
		{
			this.mPlatform.LoadFriends(this, callback);
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00043528 File Offset: 0x00041728
		public IUserProfile[] friends
		{
			get
			{
				return this.mPlatform.GetFriends();
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00043538 File Offset: 0x00041738
		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(Action<string> idTokenCallback)
		{
			if (this.authenticated)
			{
				this.mPlatform.GetIdToken(idTokenCallback);
			}
			else
			{
				idTokenCallback(null);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00043560 File Offset: 0x00041760
		public bool authenticated
		{
			get
			{
				return this.mPlatform.IsAuthenticated();
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00043570 File Offset: 0x00041770
		public bool underage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00043574 File Offset: 0x00041774
		public new string userName
		{
			get
			{
				string text = string.Empty;
				if (this.authenticated)
				{
					text = this.mPlatform.GetUserDisplayName();
					if (!base.userName.Equals(text))
					{
						base.ResetIdentity(text, this.mPlatform.GetUserId(), this.mPlatform.GetUserImageUrl());
					}
				}
				return text;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x000435D0 File Offset: 0x000417D0
		public new string id
		{
			get
			{
				string text = string.Empty;
				if (this.authenticated)
				{
					text = this.mPlatform.GetUserId();
					if (!base.id.Equals(text))
					{
						base.ResetIdentity(this.mPlatform.GetUserDisplayName(), text, this.mPlatform.GetUserImageUrl());
					}
				}
				return text;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x0004362C File Offset: 0x0004182C
		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string accessToken
		{
			get
			{
				return (!this.authenticated) ? string.Empty : this.mPlatform.GetAccessToken();
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0004365C File Offset: 0x0004185C
		public new bool isFriend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x00043660 File Offset: 0x00041860
		public new UserState state
		{
			get
			{
				return UserState.Online;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00043664 File Offset: 0x00041864
		public new string AvatarURL
		{
			get
			{
				string text = string.Empty;
				if (this.authenticated)
				{
					text = this.mPlatform.GetUserImageUrl();
					if (!base.id.Equals(text))
					{
						base.ResetIdentity(this.mPlatform.GetUserDisplayName(), this.mPlatform.GetUserId(), text);
					}
				}
				return text;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x000436C0 File Offset: 0x000418C0
		public string Email
		{
			get
			{
				if (this.authenticated && string.IsNullOrEmpty(this.emailAddress))
				{
					this.emailAddress = this.mPlatform.GetUserEmail();
					this.emailAddress = (this.emailAddress ?? string.Empty);
				}
				return (!this.authenticated) ? string.Empty : this.emailAddress;
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0004372C File Offset: 0x0004192C
		public void GetStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			if (this.mStats == null || !this.mStats.Valid)
			{
				this.mPlatform.GetPlayerStats(delegate(CommonStatusCodes rc, PlayerStats stats)
				{
					this.mStats = stats;
					callback(rc, stats);
				});
			}
			else
			{
				callback(CommonStatusCodes.Success, this.mStats);
			}
		}

		// Token: 0x04000A6C RID: 2668
		internal PlayGamesPlatform mPlatform;

		// Token: 0x04000A6D RID: 2669
		private string emailAddress;

		// Token: 0x04000A6E RID: 2670
		private PlayerStats mStats;
	}
}
