using System;
using Facebook.Unity;
using Rilisoft;
using UnityEngine;

// Token: 0x020006B0 RID: 1712
internal sealed class MainMenuFreePanel : MonoBehaviour
{
	// Token: 0x06003BBE RID: 15294 RVA: 0x00135EA8 File Offset: 0x001340A8
	private void Start()
	{
		if (this._socialGunPanel != null)
		{
			this._socialGunPanel.SetActive(FacebookController.FacebookSupported);
		}
		this._postNewsLabel.SetActive(false);
		if (this._youtubeButton != null)
		{
			this._youtubeButton.Clicked += this.HandleYoutubeClicked;
		}
		if (this._enderManButton != null)
		{
			this._enderManButton.Clicked += this.HandleEnderClicked;
		}
		if (this._postFacebookButton != null)
		{
			this._postFacebookButton.Clicked += this.HandlePostFacebookClicked;
		}
		if (this._postTwitterButton != null)
		{
			this._postTwitterButton.Clicked += this.HandlePostTwittwerClicked;
		}
		if (this._rateUsButton != null)
		{
			this._rateUsButton.Clicked += this.HandleRateAsClicked;
		}
		if (this._twitterSubcribeButton != null)
		{
			this._twitterSubcribeButton.Clicked += this.HandleTwitterSubscribeClicked;
		}
		if (this._facebookSubcribeButton != null)
		{
			this._facebookSubcribeButton.Clicked += this.HandleFacebookSubscribeClicked;
		}
		if (this._instagramSubcribeButton != null)
		{
			this._instagramSubcribeButton.Clicked += this.HandleInstagramSubscribeClicked;
		}
		if (this._backButton != null)
		{
			this._backButton.Clicked += delegate(object sender, EventArgs e)
			{
				MainMenuController.sharedController._isCancellationRequested = true;
			};
		}
		FacebookController.SocialGunEventStateChanged += this.HandleSocialGunEventStateChanged;
		if (FacebookController.sharedController != null)
		{
			this.HandleSocialGunEventStateChanged(FacebookController.sharedController.SocialGunEventActive);
		}
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x0013608C File Offset: 0x0013428C
	private void Update()
	{
		bool flag = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive;
		if (this._starParticleSocialGunButton != null && this._starParticleSocialGunButton.activeInHierarchy != flag)
		{
			this._starParticleSocialGunButton.SetActive(flag);
		}
		if (this._postFacebookButton.gameObject.activeSelf != (FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn))
		{
			this._postFacebookButton.gameObject.SetActive(FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn);
		}
		if (this._postTwitterButton.gameObject.activeSelf != (TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn))
		{
			this._postTwitterButton.gameObject.SetActive(TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		}
		if (FacebookController.sharedController != null && FacebookController.sharedController.SocialGunEventActive)
		{
			this._socialGunEventTimerLabel.text = string.Empty;
		}
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x001361DC File Offset: 0x001343DC
	private void OnDestroy()
	{
		FacebookController.SocialGunEventStateChanged -= this.HandleSocialGunEventStateChanged;
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x001361F0 File Offset: 0x001343F0
	public void SetVisible(bool visible)
	{
		if (base.gameObject.activeSelf != visible)
		{
			base.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0013621C File Offset: 0x0013441C
	public void OnSocialGunButtonClicked()
	{
		MainMenuController.sharedController.OnSocialGunEventButtonClick();
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x00136228 File Offset: 0x00134428
	private void HandleSocialGunEventStateChanged(bool enable)
	{
		this._socialGunPanel.gameObject.SetActive(enable);
		base.GetComponentsInChildren<RewardedLikeButton>(true).ForEach(delegate(RewardedLikeButton b)
		{
			b.gameObject.SetActive(!enable);
		});
		if (FacebookController.sharedController != null)
		{
			this._socialGunEventTimerLabel.text = string.Empty;
		}
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x00136290 File Offset: 0x00134490
	private void HandleYoutubeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("http://www.youtube.com/channel/UCsClw1gnMrmF6ssIB_166_Q");
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x001362E8 File Offset: 0x001344E8
	private void HandleEnderClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (Application.isEditor)
		{
			Debug.Log(MainMenu.GetEndermanUrl());
		}
		else
		{
			Application.OpenURL(MainMenu.GetEndermanUrl());
		}
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x00136344 File Offset: 0x00134544
	private void HandlePostFacebookClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		FacebookController.ShowPostDialog();
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x001363A4 File Offset: 0x001345A4
	private void HandlePostTwittwerClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (!Application.isEditor)
		{
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.PostStatusUpdate("Come and play with me in epic multiplayer shooter - Pixel Gun 3D! http://goo.gl/dQMf4n", null);
			}
		}
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x00136428 File Offset: 0x00134628
	private void HandleRateAsClicked(object sender, EventArgs e)
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.RateUs();
		}
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x00136444 File Offset: 0x00134644
	private void HandleTwitterSubscribeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("https://twitter.com/PixelGun3D");
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x0013649C File Offset: 0x0013469C
	private void HandleFacebookSubscribeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("http://pixelgun3d.com/facebook.html");
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x001364F4 File Offset: 0x001346F4
	private void HandleInstagramSubscribeClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL("http://www.instagram.com/pixelgun3d_official");
	}

	// Token: 0x04002C26 RID: 11302
	[SerializeField]
	private GameObject _postNewsLabel;

	// Token: 0x04002C27 RID: 11303
	[SerializeField]
	private GameObject _starParticleSocialGunButton;

	// Token: 0x04002C28 RID: 11304
	[SerializeField]
	private GameObject _socialGunPanel;

	// Token: 0x04002C29 RID: 11305
	[SerializeField]
	private ButtonHandler _youtubeButton;

	// Token: 0x04002C2A RID: 11306
	[SerializeField]
	private ButtonHandler _enderManButton;

	// Token: 0x04002C2B RID: 11307
	[SerializeField]
	private ButtonHandler _postFacebookButton;

	// Token: 0x04002C2C RID: 11308
	[SerializeField]
	private ButtonHandler _postTwitterButton;

	// Token: 0x04002C2D RID: 11309
	[SerializeField]
	private ButtonHandler _rateUsButton;

	// Token: 0x04002C2E RID: 11310
	[SerializeField]
	private ButtonHandler _backButton;

	// Token: 0x04002C2F RID: 11311
	[SerializeField]
	private ButtonHandler _twitterSubcribeButton;

	// Token: 0x04002C30 RID: 11312
	[SerializeField]
	private ButtonHandler _facebookSubcribeButton;

	// Token: 0x04002C31 RID: 11313
	[SerializeField]
	private ButtonHandler _instagramSubcribeButton;

	// Token: 0x04002C32 RID: 11314
	[SerializeField]
	private UILabel _socialGunEventTimerLabel;
}
