using System;
using Facebook.Unity;
using UnityEngine;

// Token: 0x0200061D RID: 1565
public class FriendsWindowStatusBar : MonoBehaviour
{
	// Token: 0x06003608 RID: 13832 RVA: 0x0011726C File Offset: 0x0011546C
	private void Start()
	{
		if (this.findFriendInput != null)
		{
			MyUIInput myUIInput = this.findFriendInput;
			myUIInput.onKeyboardInter = (Action)Delegate.Combine(myUIInput.onKeyboardInter, new Action(this.OnClickFindFriendButton));
			MyUIInput myUIInput2 = this.findFriendInput;
			myUIInput2.onKeyboardCancel = (Action)Delegate.Combine(myUIInput2.onKeyboardCancel, new Action(this.OnClickCancelFindFriendButton));
		}
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x001172D8 File Offset: 0x001154D8
	private void OnEnable()
	{
		string text = string.Format(LocalizationStore.Get("Key_1416"), Defs.maxCountFriend, Defs.maxCountFriend);
		this.warningMessageAboutLimit.text = text;
		this.findFriendInput.defaultText = LocalizationStore.Get("Key_1422");
		this.InitFacebookRewardButtonText();
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x00117330 File Offset: 0x00115530
	private void OnDestroy()
	{
		if (this.findFriendInput != null)
		{
			MyUIInput myUIInput = this.findFriendInput;
			myUIInput.onKeyboardInter = (Action)Delegate.Remove(myUIInput.onKeyboardInter, new Action(this.OnClickFindFriendButton));
			MyUIInput myUIInput2 = this.findFriendInput;
			myUIInput2.onKeyboardCancel = (Action)Delegate.Remove(myUIInput2.onKeyboardCancel, new Action(this.OnClickCancelFindFriendButton));
		}
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x0011739C File Offset: 0x0011559C
	private void InitFacebookRewardButtonText()
	{
		if (this.facebookButtonLabels == null || this.facebookButtonLabels.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.facebookButtonLabels.Length; i++)
		{
			this.facebookButtonLabels[i].text = LocalizationStore.Get("Key_1582");
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x001173F4 File Offset: 0x001155F4
	private void SetFacebookNotRewardButtonText(string text)
	{
		if (this.facebookInviteLabels == null || this.facebookInviteLabels.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.facebookInviteLabels.Length; i++)
		{
			this.facebookInviteLabels[i].text = text;
		}
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x00117444 File Offset: 0x00115644
	private void SetVisibleFacebookButtonTextByState(bool needFullLabel)
	{
		if (this.facebookButtonLabels.Length == 0 || this.facebookInviteLabels.Length == 0)
		{
			return;
		}
		this.facebookButtonLabels[0].gameObject.SetActive(!needFullLabel);
		this.facebookInviteLabels[0].gameObject.SetActive(needFullLabel);
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x00117498 File Offset: 0x00115698
	private void SetTextFacebookButton(bool isLogin, bool isRewardTake)
	{
		this.SetVisibleFacebookButtonTextByState(isLogin || isRewardTake);
		if (isRewardTake)
		{
			this.facebookRewardLabelContainer.gameObject.SetActive(false);
			string facebookNotRewardButtonText = isLogin ? LocalizationStore.Get("Key_1437") : LocalizationStore.Get("Key_1582");
			this.SetFacebookNotRewardButtonText(facebookNotRewardButtonText);
		}
		else
		{
			this.facebookRewardLabelContainer.gameObject.SetActive(true);
			for (int i = 0; i < this.facebookRewardLabels.Length; i++)
			{
				this.facebookRewardLabels[i].text = string.Format("+{0}", 10);
			}
		}
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x00117540 File Offset: 0x00115740
	private void SetTextFacebookDescription()
	{
		if (FB.IsLoggedIn)
		{
			this.messageFacebookLabel.text = LocalizationStore.Get("Key_1413");
			return;
		}
		this.messageFacebookLabel.text = LocalizationStore.Get("Key_1415");
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x00117578 File Offset: 0x00115778
	private void SetupStateFacebookContainer(bool needHide)
	{
		if (!FacebookController.FacebookSupported)
		{
			needHide = true;
		}
		if (needHide)
		{
			this.facebookContainer.gameObject.SetActive(false);
			return;
		}
		bool isRewardTake = Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1;
		this.facebookContainer.gameObject.SetActive(true);
		this.SetTextFacebookButton(FB.IsLoggedIn, isRewardTake);
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x001175D8 File Offset: 0x001157D8
	public void OnClickBackButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowGUI.Instance.HideInterface();
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x001175EC File Offset: 0x001157EC
	private void OnSuccsessGetFacebookFriendList()
	{
		FriendsController sharedController = FriendsController.sharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.DownloadDataAboutPossibleFriends();
		FriendsWindowController.Instance.NeedUpdateCurrentFriendsList = true;
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x00117620 File Offset: 0x00115820
	private void OnFacebookLoginComplete()
	{
		this.BlockClickInWindow(false);
		this.SetupStateFacebookContainer(false);
		FacebookController sharedController = FacebookController.sharedController;
		if (sharedController != null)
		{
			sharedController.InputFacebookFriends(new Action(this.OnSuccsessGetFacebookFriendList), false);
		}
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x00117660 File Offset: 0x00115860
	private void OnFacebookLoginCancel()
	{
		this.BlockClickInWindow(false);
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x0011766C File Offset: 0x0011586C
	private void BlockClickInWindow(bool enable)
	{
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x00117670 File Offset: 0x00115870
	public void OnClickFacebookButton()
	{
		ButtonClickSound.TryPlayClick();
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		FacebookController sharedController = FacebookController.sharedController;
		if (sharedController == null)
		{
			return;
		}
		this.BlockClickInWindow(true);
		if (FB.IsLoggedIn)
		{
			sharedController.InvitePlayer(null);
		}
		else
		{
			MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
			{
				FacebookController.Login(new Action(this.OnFacebookLoginComplete), new Action(this.OnFacebookLoginCancel), "Friends", null);
			}, delegate
			{
				FacebookController.Login(null, null, "Friends", null);
			});
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06003618 RID: 13848 RVA: 0x001176F8 File Offset: 0x001158F8
	// (set) Token: 0x06003617 RID: 13847 RVA: 0x001176EC File Offset: 0x001158EC
	public bool IsFindFriendByIdStateActivate { get; set; }

	// Token: 0x06003619 RID: 13849 RVA: 0x00117700 File Offset: 0x00115900
	public void OnClickFindFriendButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowController instance = FriendsWindowController.Instance;
		if (instance == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.findFriendInput.value))
		{
			return;
		}
		this.IsFindFriendByIdStateActivate = true;
		base.StartCoroutine(instance.ShowResultFindPlayer(this.findFriendInput.value));
		this.findFriendInput.value = string.Empty;
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x0011776C File Offset: 0x0011596C
	public void OnClickCancelFindFriendButton()
	{
		ButtonClickSound.TryPlayClick();
		this.findFriendInput.value = string.Empty;
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x00117784 File Offset: 0x00115984
	public void OnClickClearAllButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowController instance = FriendsWindowController.Instance;
		if (instance == null)
		{
			return;
		}
		instance.OnClickClearAllInboxButton();
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x001177B0 File Offset: 0x001159B0
	public void OnClickSendMyIdButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsController.SendMyIdByEmail();
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x001177BC File Offset: 0x001159BC
	public void OnClickRefreshButton()
	{
		ButtonClickSound.TryPlayClick();
		FriendsController sharedController = FriendsController.sharedController;
		if (sharedController == null)
		{
			return;
		}
		FriendsWindowController instance = FriendsWindowController.Instance;
		if (instance == null)
		{
			return;
		}
		instance.ShowMessageBoxProcessingData();
		this.refreshButton.isEnabled = false;
		base.Invoke("SetEnableRefreshButton", Defs.timeBlockRefreshFriendDate);
		sharedController.GetFriendsData(true);
	}

	// Token: 0x0600361E RID: 13854 RVA: 0x00117820 File Offset: 0x00115A20
	private void SetEnableRefreshButton()
	{
		this.refreshButton.isEnabled = true;
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x00117830 File Offset: 0x00115A30
	public void UpdateFriendListState(bool isFriendsMax)
	{
		this.SetupStateFacebookContainer(isFriendsMax);
		this.warningMessageAboutLimit.gameObject.SetActive(isFriendsMax);
		this.findContainer.gameObject.SetActive(false);
		this.inboxContainer.gameObject.SetActive(false);
		this.refreshButton.gameObject.SetActive(true);
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x00117888 File Offset: 0x00115A88
	public void UpdateFindFriendsState(bool isFriendsMax)
	{
		this.IsFindFriendByIdStateActivate = false;
		this.SetupStateFacebookContainer(true);
		this.warningMessageAboutLimit.gameObject.SetActive(isFriendsMax);
		this.findContainer.gameObject.SetActive(!isFriendsMax);
		this.inboxContainer.gameObject.SetActive(false);
		this.refreshButton.gameObject.SetActive(false);
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x001178EC File Offset: 0x00115AEC
	public void OnClickInboxFriendsTab(bool isInboxListFound, bool isFriendsMax)
	{
		this.SetupStateFacebookContainer(isFriendsMax || isInboxListFound);
		this.findContainer.gameObject.SetActive(false);
		bool flag = !isFriendsMax && isInboxListFound;
		this.inboxContainer.gameObject.SetActive(flag);
		if (flag && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			this.sendMyIdButton.gameObject.SetActive(false);
			this.clearAllInviteButton.transform.position = this.sendMyIdButton.transform.position;
		}
		this.refreshButton.gameObject.SetActive(false);
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x0011798C File Offset: 0x00115B8C
	public void OnClickChatTab()
	{
		this.SetupStateFacebookContainer(true);
		this.findContainer.gameObject.SetActive(false);
		this.inboxContainer.gameObject.SetActive(false);
		this.warningMessageAboutLimit.gameObject.SetActive(false);
		this.refreshButton.gameObject.SetActive(false);
	}

	// Token: 0x040027B6 RID: 10166
	public UIWidget findContainer;

	// Token: 0x040027B7 RID: 10167
	public UIWidget inboxContainer;

	// Token: 0x040027B8 RID: 10168
	public UILabel warningMessageAboutLimit;

	// Token: 0x040027B9 RID: 10169
	public MyUIInput findFriendInput;

	// Token: 0x040027BA RID: 10170
	public UIWidget facebookContainer;

	// Token: 0x040027BB RID: 10171
	public UILabel messageFacebookLabel;

	// Token: 0x040027BC RID: 10172
	public UILabel[] facebookButtonLabels;

	// Token: 0x040027BD RID: 10173
	public UILabel[] facebookRewardLabels;

	// Token: 0x040027BE RID: 10174
	public UILabel[] facebookInviteLabels;

	// Token: 0x040027BF RID: 10175
	public UIWidget facebookRewardLabelContainer;

	// Token: 0x040027C0 RID: 10176
	public UIButton refreshButton;

	// Token: 0x040027C1 RID: 10177
	public UIButton sendMyIdButton;

	// Token: 0x040027C2 RID: 10178
	public UIButton clearAllInviteButton;
}
