using System;
using System.Collections.Generic;
using I2.Loc;
using Rilisoft;

// Token: 0x0200075B RID: 1883
public sealed class SocialGunBannerView : BannerWindow
{
	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06004213 RID: 16915 RVA: 0x0015F728 File Offset: 0x0015D928
	// (remove) Token: 0x06004214 RID: 16916 RVA: 0x0015F740 File Offset: 0x0015D940
	public static event Action<bool> SocialGunBannerViewLoginCompletedWithResult;

	// Token: 0x06004215 RID: 16917 RVA: 0x0015F758 File Offset: 0x0015D958
	private void SetRewardLabelsText()
	{
		foreach (UILabel uilabel in this.rewardLabels)
		{
			uilabel.text = string.Format(LocalizationStore.Get("Key_1531"), 10);
		}
	}

	// Token: 0x06004216 RID: 16918 RVA: 0x0015F7D4 File Offset: 0x0015D9D4
	private void Awake()
	{
		this.SetRewardLabelsText();
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x0015F7F0 File Offset: 0x0015D9F0
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Social Gun");
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x0015F82C File Offset: 0x0015DA2C
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06004219 RID: 16921 RVA: 0x0015F84C File Offset: 0x0015DA4C
	private void HandleEscape()
	{
		this.HideWindow();
	}

	// Token: 0x0600421A RID: 16922 RVA: 0x0015F854 File Offset: 0x0015DA54
	public override void Show()
	{
		base.Show();
		if (FacebookController.sharedController != null)
		{
			FacebookController.sharedController.UpdateCountShownWindowByShowCondition();
		}
	}

	// Token: 0x0600421B RID: 16923 RVA: 0x0015F884 File Offset: 0x0015DA84
	public void HideWindow()
	{
		ButtonClickSound.TryPlayClick();
		if (!this.freePanelBanner)
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController != null)
			{
				sharedController.HideBannerWindow();
				return;
			}
		}
		base.Hide();
	}

	// Token: 0x0600421C RID: 16924 RVA: 0x0015F8C8 File Offset: 0x0015DAC8
	public void Continue()
	{
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
		{
			FacebookController.Login(delegate
			{
				this.FireSocialGunBannerViewLoginCompletedEvent(true);
			}, delegate
			{
				this.FireSocialGunBannerViewLoginCompletedEvent(false);
			}, "Social Gun Banner", null);
		}, delegate
		{
			FacebookController.Login(null, null, "Social Gun Banner", null);
		});
	}

	// Token: 0x0600421D RID: 16925 RVA: 0x0015F904 File Offset: 0x0015DB04
	private void FireSocialGunBannerViewLoginCompletedEvent(bool val)
	{
		Action<bool> socialGunBannerViewLoginCompletedWithResult = SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult;
		if (socialGunBannerViewLoginCompletedWithResult != null)
		{
			socialGunBannerViewLoginCompletedWithResult(val);
		}
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x0015F924 File Offset: 0x0015DB24
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x0600421F RID: 16927 RVA: 0x0015F938 File Offset: 0x0015DB38
	private void HandleLocalizationChanged()
	{
		this.SetRewardLabelsText();
	}

	// Token: 0x04003054 RID: 12372
	public bool freePanelBanner;

	// Token: 0x04003055 RID: 12373
	public List<UILabel> rewardLabels;

	// Token: 0x04003056 RID: 12374
	private IDisposable _backSubscription;
}
