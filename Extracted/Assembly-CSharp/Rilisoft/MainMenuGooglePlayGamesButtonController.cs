using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006B2 RID: 1714
	[DisallowMultipleComponent]
	[Serializable]
	internal sealed class MainMenuGooglePlayGamesButtonController : MonoBehaviour
	{
		// Token: 0x06003BD0 RID: 15312 RVA: 0x00136638 File Offset: 0x00134838
		private MainMenuGooglePlayGamesButtonController()
		{
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00136640 File Offset: 0x00134840
		private void Start()
		{
			GpgFacade.Instance.SignedOut += this.HandleSignOut;
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00136658 File Offset: 0x00134858
		private void OnDestroy()
		{
			GpgFacade.Instance.SignedOut -= this.HandleSignOut;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00136670 File Offset: 0x00134870
		private void OnEnable()
		{
			this.RefreshButtonsVisibility();
			this.RefreshSignOutButton();
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00136680 File Offset: 0x00134880
		private void RefreshButtonsVisibility()
		{
			if (this._buttonsHolder == null)
			{
				return;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				this._buttonsHolder.SetActive(false);
				return;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				this._buttonsHolder.SetActive(false);
				return;
			}
			this._buttonsHolder.SetActive(true);
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x001366DC File Offset: 0x001348DC
		private void RefreshSignOutButton()
		{
			if (this._signOutButton == null)
			{
				return;
			}
			if (Application.isEditor)
			{
				this._signOutButton.SetActive(true);
				return;
			}
			bool active = GpgFacade.Instance.IsAuthenticated();
			this._signOutButton.SetActive(active);
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0013672C File Offset: 0x0013492C
		private void HandleSignOut(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("{0}.HandleSignOut(); isAuthenticated: {1}", new object[]
				{
					base.GetType().Name,
					GpgFacade.Instance.IsAuthenticated()
				});
			}
			this.RefreshSignOutButton();
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0013677C File Offset: 0x0013497C
		public void HandlePlayGamesButton()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return;
			}
			if (TrainingController.TrainingCompletedFlagForLogging != null)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			if (this._runningAuthentication)
			{
				Debug.Log("Ignoring Play Games button since authentication is currently running.");
				return;
			}
			this.RefreshSignOutButton();
			if (GpgFacade.Instance.IsAuthenticated())
			{
				Social.ShowAchievementsUI();
				return;
			}
			this._runningAuthentication = true;
			GpgFacade.Instance.Authenticate(delegate(bool succeeded)
			{
				this.RefreshSignOutButton();
				if (succeeded)
				{
					Social.ShowAchievementsUI();
				}
				else
				{
					PlayerPrefs.SetInt("GoogleSignInDenied", 1);
				}
				this._runningAuthentication = false;
			}, false);
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0013681C File Offset: 0x00134A1C
		public void HandleSignOutGooglePlayGamesButton()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return;
			}
			ButtonClickSound.TryPlayClick();
			PlayerPrefs.SetInt("GoogleSignInDenied", 1);
			GpgFacade.Instance.SignOut();
			string text = LocalizationStore.Get("Key_2103") ?? "Signed out.";
			InfoWindowController.ShowInfoBox(text);
		}

		// Token: 0x04002C36 RID: 11318
		private bool _runningAuthentication;

		// Token: 0x04002C37 RID: 11319
		[SerializeField]
		private GameObject _buttonsHolder;

		// Token: 0x04002C38 RID: 11320
		[SerializeField]
		private GameObject _signOutButton;
	}
}
