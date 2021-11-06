using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000756 RID: 1878
	[DisallowMultipleComponent]
	[Serializable]
	internal sealed class SettingsGameCenterButtonController : MonoBehaviour
	{
		// Token: 0x060041F5 RID: 16885 RVA: 0x0015F0F0 File Offset: 0x0015D2F0
		private SettingsGameCenterButtonController()
		{
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x0015F0F8 File Offset: 0x0015D2F8
		private void OnEnable()
		{
			SettingsGameCenterButtonController.RefreshRateAndroidVisibility(this._rateGameAndroidButton);
			SettingsGameCenterButtonController.RefreshRateIosVisibility(this._rateGameIosButton);
			SettingsGameCenterButtonController.RefreshGameCenterButton(this._buttonsHolder);
			SettingsGameCenterButtonController.RefreshSignOutButton(this._signOutButton);
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x0015F134 File Offset: 0x0015D334
		private static void RefreshRateAndroidVisibility(GameObject rateGameAndroidButton)
		{
			if (rateGameAndroidButton == null)
			{
				return;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				rateGameAndroidButton.SetActive(false);
				return;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				rateGameAndroidButton.SetActive(false);
				return;
			}
			rateGameAndroidButton.SetActive(true);
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x0015F17C File Offset: 0x0015D37C
		private static void RefreshRateIosVisibility(GameObject rateGameIosButton)
		{
			if (rateGameIosButton == null)
			{
				return;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
			{
				rateGameIosButton.SetActive(false);
				return;
			}
			rateGameIosButton.SetActive(true);
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x0015F1A8 File Offset: 0x0015D3A8
		private static void RefreshGameCenterButton(GameObject buttonsHolder)
		{
			if (buttonsHolder == null)
			{
				return;
			}
			RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
			if (buildTargetPlatform != RuntimePlatform.IPhonePlayer)
			{
				buttonsHolder.SetActive(false);
				return;
			}
			if (Application.isEditor)
			{
				buttonsHolder.SetActive(true);
				return;
			}
			if (Social.localUser == null)
			{
				buttonsHolder.SetActive(false);
				return;
			}
			bool authenticated = Social.localUser.authenticated;
			buttonsHolder.SetActive(authenticated);
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x0015F214 File Offset: 0x0015D414
		private static void RefreshSignOutButton(GameObject signOutButton)
		{
			if (signOutButton == null)
			{
				return;
			}
			signOutButton.SetActive(false);
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x0015F22C File Offset: 0x0015D42C
		public void HandleGameCenterButton()
		{
			ButtonClickSound.Instance.PlayClick();
			if (Application.isEditor)
			{
				Debug.Log("[Game Center] pressed.");
				return;
			}
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.IPhonePlayer:
			{
				GameCenterSingleton instance = GameCenterSingleton.Instance;
				if (instance.IsUserAuthenticated())
				{
					instance.ShowLeaderboardUI();
				}
				else
				{
					instance.updateGameCenter();
				}
				return;
			}
			case RuntimePlatform.Android:
				if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
				{
					return;
				}
				if (GameCircleSocial.Instance.localUser == null || !GameCircleSocial.Instance.localUser.authenticated)
				{
					AGSClient.ShowSignInPage();
					return;
				}
				AGSAchievementsClient.ShowAchievementsOverlay();
				return;
			}
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x0015F2DC File Offset: 0x0015D4DC
		public void HandleSignOutGameCenterButton()
		{
			ButtonClickSound.Instance.PlayClick();
			if (Application.isEditor)
			{
				Debug.Log("[Sign Out] pressed.");
				return;
			}
		}

		// Token: 0x04003044 RID: 12356
		[SerializeField]
		private GameObject _buttonsHolder;

		// Token: 0x04003045 RID: 12357
		[SerializeField]
		private GameObject _signOutButton;

		// Token: 0x04003046 RID: 12358
		[SerializeField]
		private GameObject _rateGameAndroidButton;

		// Token: 0x04003047 RID: 12359
		[SerializeField]
		private GameObject _rateGameIosButton;
	}
}
