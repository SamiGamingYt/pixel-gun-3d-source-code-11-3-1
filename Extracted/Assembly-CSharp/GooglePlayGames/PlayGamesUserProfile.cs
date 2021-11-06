using System;
using System.Collections;
using GooglePlayGames.OurUtils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	// Token: 0x020001A4 RID: 420
	public class PlayGamesUserProfile : IUserProfile
	{
		// Token: 0x06000D9E RID: 3486 RVA: 0x00044780 File Offset: 0x00042980
		internal PlayGamesUserProfile(string displayName, string playerId, string avatarUrl)
		{
			this.mDisplayName = displayName;
			this.mPlayerId = playerId;
			this.mAvatarUrl = avatarUrl;
			this.mImageLoading = false;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000447B4 File Offset: 0x000429B4
		protected void ResetIdentity(string displayName, string playerId, string avatarUrl)
		{
			this.mDisplayName = displayName;
			this.mPlayerId = playerId;
			if (this.mAvatarUrl != avatarUrl)
			{
				this.mImage = null;
				this.mAvatarUrl = avatarUrl;
			}
			this.mImageLoading = false;
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000447F8 File Offset: 0x000429F8
		public string userName
		{
			get
			{
				return this.mDisplayName;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00044800 File Offset: 0x00042A00
		public string id
		{
			get
			{
				return this.mPlayerId;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00044808 File Offset: 0x00042A08
		public bool isFriend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0004480C File Offset: 0x00042A0C
		public UserState state
		{
			get
			{
				return UserState.Online;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00044810 File Offset: 0x00042A10
		public Texture2D image
		{
			get
			{
				if (!this.mImageLoading && this.mImage == null && !string.IsNullOrEmpty(this.AvatarURL))
				{
					Debug.Log("Starting to load image: " + this.AvatarURL);
					this.mImageLoading = true;
					PlayGamesHelperObject.RunCoroutine(this.LoadImage());
				}
				return this.mImage;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0004487C File Offset: 0x00042A7C
		public string AvatarURL
		{
			get
			{
				return this.mAvatarUrl;
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00044884 File Offset: 0x00042A84
		internal IEnumerator LoadImage()
		{
			if (!string.IsNullOrEmpty(this.AvatarURL))
			{
				WWW www = new WWW(this.AvatarURL);
				while (!www.isDone)
				{
					yield return null;
				}
				if (www.error == null)
				{
					this.mImage = www.texture;
				}
				else
				{
					this.mImage = Texture2D.blackTexture;
					Debug.Log("Error downloading image: " + www.error);
				}
				this.mImageLoading = false;
			}
			else
			{
				Debug.Log("No URL found.");
				this.mImage = Texture2D.blackTexture;
				this.mImageLoading = false;
			}
			yield break;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000448A0 File Offset: 0x00042AA0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			PlayGamesUserProfile playGamesUserProfile = obj as PlayGamesUserProfile;
			return playGamesUserProfile != null && StringComparer.Ordinal.Equals(this.mPlayerId, playGamesUserProfile.mPlayerId);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000448E8 File Offset: 0x00042AE8
		public override int GetHashCode()
		{
			return typeof(PlayGamesUserProfile).GetHashCode() ^ this.mPlayerId.GetHashCode();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00044908 File Offset: 0x00042B08
		public override string ToString()
		{
			return string.Format("[Player: '{0}' (id {1})]", this.mDisplayName, this.mPlayerId);
		}

		// Token: 0x04000A7D RID: 2685
		private string mDisplayName;

		// Token: 0x04000A7E RID: 2686
		private string mPlayerId;

		// Token: 0x04000A7F RID: 2687
		private string mAvatarUrl;

		// Token: 0x04000A80 RID: 2688
		private volatile bool mImageLoading;

		// Token: 0x04000A81 RID: 2689
		private Texture2D mImage;
	}
}
