using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class DaysOfValorBanner : BannerWindow
{
	// Token: 0x06003035 RID: 12341 RVA: 0x000FB980 File Offset: 0x000F9B80
	private void SetButtonApplyText()
	{
		if (SceneLoader.ActiveSceneName.Equals("ConnectScene") || SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox"))
		{
			this.buttonApplyLabel.text = LocalizationStore.Get("Key_0012");
		}
		else
		{
			this.buttonApplyLabel.text = LocalizationStore.Get("Key_0085");
		}
	}

	// Token: 0x06003036 RID: 12342 RVA: 0x000FB9E4 File Offset: 0x000F9BE4
	private string GetNameSpriteForMultyplayer(int multiplyer)
	{
		return string.Format("{0}x", multiplyer);
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x000FB9F8 File Offset: 0x000F9BF8
	private void SetSettingMultiplyerContainer()
	{
		PromoActionsManager sharedManager = PromoActionsManager.sharedManager;
		if (sharedManager == null)
		{
			return;
		}
		Transform transform = this.expContainer.gameObject.transform;
		Transform transform2 = this.coinsContainer.gameObject.transform;
		transform.localPosition = Vector3.zero;
		transform2.localPosition = Vector3.zero;
		int num = this.expContainer.width / 2;
		int dayOfValorMultiplyerForExp = sharedManager.DayOfValorMultiplyerForExp;
		int dayOfValorMultiplyerForMoney = sharedManager.DayOfValorMultiplyerForMoney;
		if (dayOfValorMultiplyerForExp > 1 && dayOfValorMultiplyerForMoney > 1)
		{
			transform.gameObject.SetActive(true);
			transform2.gameObject.SetActive(true);
			transform.localPosition = new Vector3((float)(-(float)num), 0f, 0f);
			transform2.localPosition = new Vector3((float)num, 0f, 0f);
			this.expMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForExp);
			this.moneyMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForMoney);
		}
		else if (dayOfValorMultiplyerForExp > 1)
		{
			transform.gameObject.SetActive(true);
			transform2.gameObject.SetActive(false);
			this.expMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForExp);
		}
		else if (dayOfValorMultiplyerForMoney > 1)
		{
			transform.gameObject.SetActive(false);
			transform2.gameObject.SetActive(true);
			this.moneyMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForMoney);
		}
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x000FBB5C File Offset: 0x000F9D5C
	private void OnEnable()
	{
		this.SetButtonApplyText();
		this.SetSettingMultiplyerContainer();
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x000FBB6C File Offset: 0x000F9D6C
	public void OnClickApplyButton()
	{
		if (SceneLoader.ActiveSceneName.Equals("ConnectScene") || SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox"))
		{
			this.HideWindow();
		}
		else
		{
			this.HideWindow();
			MainMenuController sharedController = MainMenuController.sharedController;
			if (sharedController != null)
			{
				sharedController.OnClickMultiplyerButton();
			}
		}
	}

	// Token: 0x0600303A RID: 12346 RVA: 0x000FBBCC File Offset: 0x000F9DCC
	public override void Show()
	{
		base.Show();
		PlayerPrefs.SetString("LastTimeShowDaysOfValor", DateTime.UtcNow.ToString("s"));
		PlayerPrefs.Save();
	}

	// Token: 0x0600303B RID: 12347 RVA: 0x000FBC00 File Offset: 0x000F9E00
	public void HideWindow()
	{
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
		this.UpdateShownCount();
	}

	// Token: 0x0600303C RID: 12348 RVA: 0x000FBC30 File Offset: 0x000F9E30
	private void UpdateShownCount()
	{
		int @int = PlayerPrefs.GetInt("DaysOfValorShownCount", 1);
		PlayerPrefs.SetInt("DaysOfValorShownCount", @int - 1);
		PlayerPrefs.Save();
	}

	// Token: 0x04002368 RID: 9064
	public UILabel buttonApplyLabel;

	// Token: 0x04002369 RID: 9065
	public UIWidget multiplyerContainer;

	// Token: 0x0400236A RID: 9066
	public UIWidget expContainer;

	// Token: 0x0400236B RID: 9067
	public UIWidget coinsContainer;

	// Token: 0x0400236C RID: 9068
	public UISprite expMultiplyerSprite;

	// Token: 0x0400236D RID: 9069
	public UISprite moneyMultiplyerSprite;
}
