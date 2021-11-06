using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000651 RID: 1617
	internal sealed class GpgFacade
	{
		// Token: 0x0600383D RID: 14397 RVA: 0x0012235C File Offset: 0x0012055C
		private GpgFacade()
		{
			try
			{
				PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
				PlayGamesPlatform.InitializeInstance(configuration);
				PlayGamesPlatform.DebugLogEnabled = (Defs.IsDeveloperBuild && BuildSettings.BuildTargetPlatform == RuntimePlatform.Android);
				PlayGamesPlatform.Activate();
				this._playGamesPlatformInstance = PlayGamesPlatform.Instance;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x0600383F RID: 14399 RVA: 0x0012240C File Offset: 0x0012060C
		// (remove) Token: 0x06003840 RID: 14400 RVA: 0x00122428 File Offset: 0x00120628
		public event EventHandler SignedOut;

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003841 RID: 14401 RVA: 0x00122444 File Offset: 0x00120644
		public static GpgFacade Instance
		{
			get
			{
				return GpgFacade.s_instance.Value;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x00122450 File Offset: 0x00120650
		public PlayGamesPlatform PlayGamesPlatform
		{
			get
			{
				return this._playGamesPlatformInstance;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x00122458 File Offset: 0x00120658
		public ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient result;
				try
				{
					result = this.PlayGamesPlatform.SavedGame;
				}
				catch (NullReferenceException)
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x001224A8 File Offset: 0x001206A8
		public void Initialize()
		{
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x001224AC File Offset: 0x001206AC
		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this.PlayGamesPlatform.Authenticate(callback, silent);
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x001224CC File Offset: 0x001206CC
		public void IncrementAchievement(string achievementId, int steps, Action<bool> callback)
		{
			if (achievementId == null)
			{
				throw new ArgumentNullException("achievementId");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this.PlayGamesPlatform.IncrementAchievement(achievementId, steps, callback);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x0012250C File Offset: 0x0012070C
		public bool IsAuthenticated()
		{
			return this.PlayGamesPlatform.IsAuthenticated();
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x0012251C File Offset: 0x0012071C
		public void SignOut()
		{
			this.PlayGamesPlatform.SignOut();
			EventHandler signedOut = this.SignedOut;
			if (signedOut != null)
			{
				signedOut(this, EventArgs.Empty);
			}
		}

		// Token: 0x0400291A RID: 10522
		private readonly PlayGamesPlatform _playGamesPlatformInstance;

		// Token: 0x0400291B RID: 10523
		private static readonly Lazy<GpgFacade> s_instance = new Lazy<GpgFacade>(() => new GpgFacade());
	}
}
