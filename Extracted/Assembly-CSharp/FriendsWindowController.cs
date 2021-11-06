using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x0200061B RID: 1563
public class FriendsWindowController : MonoBehaviour
{
	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x060035CB RID: 13771 RVA: 0x00115F98 File Offset: 0x00114198
	// (set) Token: 0x060035CC RID: 13772 RVA: 0x00115FA0 File Offset: 0x001141A0
	public bool NeedUpdateCurrentFriendsList { get; set; }

	// Token: 0x060035CD RID: 13773 RVA: 0x00115FAC File Offset: 0x001141AC
	public void OnClickGoInBattleButton()
	{
		ButtonClickSound.TryPlayClick();
		GlobalGameController.GoInBattle();
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x00115FB8 File Offset: 0x001141B8
	private void Awake()
	{
		FriendsWindowController.Instance = this;
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x00115FC0 File Offset: 0x001141C0
	private void Start()
	{
		this.currentWindowState = FriendsWindowController.WindowState.friendList;
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x00115FCC File Offset: 0x001141CC
	private void OnEnable()
	{
		FriendsController.FriendsUpdated += this.UpdateFriendsListInterface;
		FriendsController.OnShowBoxProcessFriendsData = (Action)Delegate.Combine(FriendsController.OnShowBoxProcessFriendsData, new Action(this.ShowMessageBoxProcessingData));
		FriendsController.OnHideBoxProcessFriendsData = (Action)Delegate.Combine(FriendsController.OnHideBoxProcessFriendsData, new Action(this.HideInfoBox));
		FriendsController.UpdateFriendsInfoAction = (Action)Delegate.Combine(FriendsController.UpdateFriendsInfoAction, new Action(this.EventUpdateFriendsOnlineAndSorting));
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x0011604C File Offset: 0x0011424C
	public void ShowMessageBoxProcessingData()
	{
		InfoWindowController.ShowProcessingDataBox();
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x00116054 File Offset: 0x00114254
	public void HideInfoBox()
	{
		InfoWindowController.HideProcessing();
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x0011605C File Offset: 0x0011425C
	private void ShowMessageByEmptyStateTab(string text)
	{
		this.emptyStateTabLabel.gameObject.SetActive(true);
		this.emptyStateTabLabel.text = text;
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x0011607C File Offset: 0x0011427C
	private void HideMessageByEmptyStateTab()
	{
		this.emptyStateTabLabel.gameObject.SetActive(false);
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x00116090 File Offset: 0x00114290
	private void CheckShowEmptyStateTabLabel(bool isListNotEmpty, bool isFriendsMax)
	{
		if (isListNotEmpty)
		{
			if (this.currentWindowState == FriendsWindowController.WindowState.chat)
			{
				this.chatContainer.SetActive(true);
			}
			this.HideMessageByEmptyStateTab();
			return;
		}
		if (this.currentWindowState == FriendsWindowController.WindowState.chat)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1557"));
			this.chatContainer.SetActive(false);
		}
		else if (isFriendsMax)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1424"));
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1425"));
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_0223"));
		}
		else
		{
			this.ShowMessageByEmptyStateTab(LocalizationStore.Get("Key_1423"));
		}
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x0011615C File Offset: 0x0011435C
	private void UpdateFriendsListState()
	{
		if (this.myFriendsList != null)
		{
			this.UpdateList(this.friendsListWrap, FriendItemPreviewType.view);
		}
		this._isAnyFriendsDataExists = FriendsController.IsFriendsOrLocalDataExist();
		this._isFriendsMax = FriendsController.IsFriendsMax();
		if (this.currentWindowState == FriendsWindowController.WindowState.chat)
		{
			this.CheckShowEmptyStateTabLabel(FriendsController.IsFriendsDataExist(), this._isFriendsMax);
		}
		else
		{
			this.CheckShowEmptyStateTabLabel(this._isAnyFriendsDataExists, this._isFriendsMax);
		}
		bool active = !this._isAnyFriendsDataExists && ProtocolListGetter.currentVersionIsSupported;
		this.goInBattleButton.gameObject.SetActive(active);
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.statusBar.UpdateFriendListState(this._isFriendsMax);
		}
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x0011620C File Offset: 0x0011440C
	private void SetActiveFriendsListContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.friendList;
		this.UpdateFriendsListState();
		if (!this._isAnyFriendsDataExists)
		{
			this._isWindowInStartState = false;
			return;
		}
		if (this.NeedUpdateCurrentFriendsList)
		{
			base.StartCoroutine(this.UpdateCurrentFriendsList());
		}
		this._isWindowInStartState = false;
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00116258 File Offset: 0x00114458
	private string GetItemFromCurrentState(int index)
	{
		switch (this.currentWindowState)
		{
		case FriendsWindowController.WindowState.friendList:
			if (index < this.localFriendsList.Length)
			{
				return this.localFriendsList[index];
			}
			if (index - this.localFriendsList.Length < this.myFriendsList.Length)
			{
				return this.myFriendsList[index - this.localFriendsList.Length];
			}
			return string.Empty;
		case FriendsWindowController.WindowState.findFriends:
			if (index < this.findFriendsList.Length && !this._isFriendsMax)
			{
				return this.findFriendsList[index];
			}
			return string.Empty;
		case FriendsWindowController.WindowState.chat:
			if (index < this.myFriendsList.Length)
			{
				return this.myFriendsList[index];
			}
			return string.Empty;
		case FriendsWindowController.WindowState.inbox:
			if (index < this.inviteFriendsList.Length)
			{
				return this.inviteFriendsList[index];
			}
			return string.Empty;
		default:
			return null;
		}
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x00116330 File Offset: 0x00114530
	private string GetLocalPlayerItem(int index)
	{
		if (index < this.localFriendsList.Length)
		{
			return this.localFriendsList[index];
		}
		return string.Empty;
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x00116350 File Offset: 0x00114550
	private int GetLenghtFromCurrentList()
	{
		switch (this.currentWindowState)
		{
		case FriendsWindowController.WindowState.friendList:
			return ((this.localFriendsList == null) ? 0 : this.localFriendsList.Length) + this.myFriendsList.Length;
		case FriendsWindowController.WindowState.findFriends:
			return (this.findFriendsList == null) ? 0 : this.findFriendsList.Length;
		case FriendsWindowController.WindowState.inbox:
			return (this.inviteFriendsList == null) ? 0 : this.inviteFriendsList.Length;
		}
		return 0;
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x001163D8 File Offset: 0x001145D8
	private FriendItemPreviewType GetPreviewTypeForCurrentWindowState()
	{
		switch (this.currentWindowState)
		{
		case FriendsWindowController.WindowState.friendList:
			return FriendItemPreviewType.view;
		case FriendsWindowController.WindowState.findFriends:
			return FriendItemPreviewType.find;
		case FriendsWindowController.WindowState.inbox:
			return FriendItemPreviewType.inbox;
		}
		return FriendItemPreviewType.none;
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x00116410 File Offset: 0x00114610
	private UIWrapContent GetCurrentWrap()
	{
		switch (this.currentWindowState)
		{
		case FriendsWindowController.WindowState.friendList:
			return this.friendsListWrap;
		case FriendsWindowController.WindowState.findFriends:
			return this.findFriendsWrap;
		case FriendsWindowController.WindowState.chat:
			return null;
		case FriendsWindowController.WindowState.inbox:
			return this.inviteFriendsWrap;
		default:
			return null;
		}
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x00116458 File Offset: 0x00114658
	public void UpdateCurrentFriendsArrayAndItems()
	{
		this.UpdateFriendsArray(this.currentWindowState, false);
		UIWrapContent currentWrap = this.GetCurrentWrap();
		if (currentWrap == null)
		{
			return;
		}
		this.UpdateList(currentWrap, this.GetPreviewTypeForCurrentWindowState());
		UIScrollView component = currentWrap.transform.parent.GetComponent<UIScrollView>();
		int minForCurrentState = this.GetMinForCurrentState();
		int lenghtFromCurrentList = this.GetLenghtFromCurrentList();
		if (lenghtFromCurrentList > minForCurrentState && component.transform.localPosition.y + (float)(currentWrap.itemSize * minForCurrentState) > (float)(lenghtFromCurrentList * currentWrap.itemSize))
		{
			component.MoveRelative(Vector3.down * (float)currentWrap.itemSize);
		}
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x001164FC File Offset: 0x001146FC
	private void ResetWrapPosition(UIWrapContent wrap)
	{
		wrap.SortAlphabetically();
		wrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x00116524 File Offset: 0x00114724
	private void UpdateFriendsArray(FriendsWindowController.WindowState state, bool resetScroll = false)
	{
		switch (state)
		{
		case FriendsWindowController.WindowState.friendList:
		{
			List<string> list = new List<string>();
			for (int i = 0; i < FriendsController.sharedController.friends.Count; i++)
			{
				string text = FriendsController.sharedController.friends[i];
				if (FriendsController.sharedController.friendsInfo.ContainsKey(text))
				{
					list.Add(text);
				}
			}
			list.Sort(new Comparison<string>(this.SortFriendsByOnlineStatusAndClickJoin));
			this.myFriendsList = list.ToArray();
			if (this.localFriendsList == null)
			{
				this.friendsListWrap.minIndex = this.myFriendsList.Length * -1;
			}
			else
			{
				this.friendsListWrap.minIndex = (this.myFriendsList.Length + this.localFriendsList.Length) * -1;
			}
			if (resetScroll)
			{
				this.friendsListWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
			}
			break;
		}
		case FriendsWindowController.WindowState.findFriends:
		{
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, FriendsController.PossiblleOrigin> keyValuePair in FriendsController.sharedController.getPossibleFriendsResult)
			{
				string key = keyValuePair.Key;
				if (FriendsController.sharedController.profileInfo.ContainsKey(key))
				{
					list2.Add(key);
				}
			}
			this.findFriendsList = list2.ToArray();
			Array.Sort<string>(this.findFriendsList, new Comparison<string>(this.SortFriendsByFindOrigin));
			this.findFriendsWrap.minIndex = this.findFriendsList.Length * -1;
			if (resetScroll)
			{
				this.findFriendsWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
			}
			break;
		}
		case FriendsWindowController.WindowState.inbox:
		{
			List<string> list3 = new List<string>();
			for (int j = 0; j < FriendsController.sharedController.invitesToUs.Count; j++)
			{
				string text2 = FriendsController.sharedController.invitesToUs[j];
				if (FriendsController.sharedController.profileInfo.ContainsKey(text2))
				{
					list3.Add(text2);
				}
			}
			this.inviteFriendsList = list3.ToArray();
			this.inviteFriendsWrap.minIndex = this.inviteFriendsList.Length * -1;
			if (resetScroll)
			{
				this.inviteFriendsWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
			}
			break;
		}
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x001167B0 File Offset: 0x001149B0
	private void UpdateLocalFriendsArray(bool resetScroll)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FriendsController.PossiblleOrigin> keyValuePair in FriendsController.sharedController.getPossibleFriendsResult)
		{
			if (FriendsController.sharedController.profileInfo.ContainsKey(keyValuePair.Key) && keyValuePair.Value == FriendsController.PossiblleOrigin.Local)
			{
				list.Add(keyValuePair.Key);
			}
		}
		this.localFriendsList = list.ToArray();
		this.friendsListWrap.minIndex = (this.myFriendsList.Length + this.localFriendsList.Length) * -1;
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x00116878 File Offset: 0x00114A78
	private IEnumerator UpdateCurrentFriendsList()
	{
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController == null)
		{
			yield break;
		}
		this.UpdateFriendsArray(FriendsWindowController.WindowState.friendList, false);
		this.UpdateFriendsArray(FriendsWindowController.WindowState.findFriends, false);
		this.UpdateFriendsArray(FriendsWindowController.WindowState.inbox, false);
		this.UpdateLocalFriendsArray(false);
		if (!this.wrapsInit)
		{
			this.inviteFriendsWrap.onInitializeItem = new UIWrapContent.OnInitializeItem(this.OnIniviteItemWrap);
			this.friendsListWrap.onInitializeItem = new UIWrapContent.OnInitializeItem(this.OnFriendsItemWrap);
			this.findFriendsWrap.onInitializeItem = new UIWrapContent.OnInitializeItem(this.OnFindFriendsItemWrap);
			this.friendPreviewPrefab.transform.GetComponent<UIDragScrollView>().scrollView = this.inviteFriendsWrap.GetComponent<UIScrollView>();
			for (int f = 0; f < 6; f++)
			{
				GameObject friendPreviewItem = NGUITools.AddChild(this.inviteFriendsWrap.gameObject, this.friendPreviewPrefab);
				friendPreviewItem.name = "FriendPreviewItem_" + f;
			}
			this.inviteFriendsWrap.SortAlphabetically();
			this.friendPreviewPrefab.transform.GetComponent<UIDragScrollView>().scrollView = this.friendsListWrap.GetComponent<UIScrollView>();
			for (int f2 = 0; f2 < 6; f2++)
			{
				GameObject friendPreviewItem2 = NGUITools.AddChild(this.friendsListWrap.gameObject, this.friendPreviewPrefab);
				friendPreviewItem2.name = "FriendPreviewItem_" + f2;
			}
			this.friendsListWrap.SortAlphabetically();
			this.friendPreviewPrefab.transform.GetComponent<UIDragScrollView>().scrollView = this.findFriendsWrap.GetComponent<UIScrollView>();
			for (int f3 = 0; f3 < 6; f3++)
			{
				GameObject friendPreviewItem3 = NGUITools.AddChild(this.findFriendsWrap.gameObject, this.friendPreviewPrefab);
				friendPreviewItem3.name = "FriendPreviewItem_" + f3;
			}
			this.findFriendsWrap.SortAlphabetically();
			this.wrapsInit = true;
		}
		if (this.currentWindowState != FriendsWindowController.WindowState.chat)
		{
			this.UpdateList(this.GetCurrentWrap(), this.GetPreviewTypeForCurrentWindowState());
		}
		this.NeedUpdateCurrentFriendsList = false;
		yield break;
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x00116894 File Offset: 0x00114A94
	private void OnIniviteItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.inbox);
		}
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x001168CC File Offset: 0x00114ACC
	private void OnFriendsItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.view);
		}
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x00116904 File Offset: 0x00114B04
	private void OnFindFriendsItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.findFriends)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.find);
		}
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x0011693C File Offset: 0x00114B3C
	private void OnLocalFriendsItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPreviewItem>().myWrapIndex = Mathf.Abs(realInd);
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.updateItemInfo(go.GetComponent<FriendPreviewItem>(), FriendItemPreviewType.find);
		}
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x00116974 File Offset: 0x00114B74
	private void updateItemInfo(FriendPreviewItem previewItem, FriendItemPreviewType _typeItems)
	{
		string itemFromCurrentState = this.GetItemFromCurrentState(previewItem.myWrapIndex);
		if (!string.IsNullOrEmpty(itemFromCurrentState))
		{
			if (_typeItems == FriendItemPreviewType.view && previewItem.myWrapIndex < this.localFriendsList.Length)
			{
				previewItem.FillData(itemFromCurrentState, FriendItemPreviewType.find);
			}
			else
			{
				previewItem.FillData(itemFromCurrentState, _typeItems);
			}
			previewItem.gameObject.SetActive(false);
			previewItem.gameObject.SetActive(true);
		}
		else if (previewItem.gameObject.activeSelf)
		{
			previewItem.gameObject.SetActive(false);
		}
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x00116A04 File Offset: 0x00114C04
	private int GetMinForCurrentState()
	{
		return 4;
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x00116A08 File Offset: 0x00114C08
	private void UpdateList(UIWrapContent _wrap, FriendItemPreviewType _typeItems)
	{
		if (_wrap == null)
		{
			return;
		}
		FriendPreviewItem[] componentsInChildren = _wrap.GetComponentsInChildren<FriendPreviewItem>(true);
		bool flag = false;
		int minForCurrentState = this.GetMinForCurrentState();
		int lenghtFromCurrentList = this.GetLenghtFromCurrentList();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (lenghtFromCurrentList != 0)
			{
				if (lenghtFromCurrentList > minForCurrentState)
				{
					componentsInChildren[i].GetComponent<UIDragScrollView>().enabled = true;
				}
				else
				{
					componentsInChildren[i].GetComponent<UIDragScrollView>().enabled = false;
					flag = true;
				}
			}
			this.updateItemInfo(componentsInChildren[i], _typeItems);
		}
		if (flag)
		{
			this.ResetWrapPosition(_wrap);
		}
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x00116A9C File Offset: 0x00114C9C
	private void UpdateFriendsListInterface()
	{
		this.UpdateFriendsListState();
		this.NeedUpdateCurrentFriendsList = true;
		if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.NeedUpdateCurrentFriendsList = true;
			this.UpdateFriendsInboxState();
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.findFriends)
		{
			this.NeedUpdateCurrentFriendsList = true;
			if (!this.statusBar.IsFindFriendByIdStateActivate)
			{
				this.UpdateFindFriendsState();
			}
		}
		if (this.NeedUpdateCurrentFriendsList && (this.currentWindowState != FriendsWindowController.WindowState.findFriends || !this.statusBar.IsFindFriendByIdStateActivate))
		{
			base.StartCoroutine(this.UpdateCurrentFriendsList());
		}
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x00116B34 File Offset: 0x00114D34
	public void OnClickFriendListTab(UIToggle toggle)
	{
		if (toggle.value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.value)
		{
			return;
		}
		this.HideInfoBox();
		this.NeedUpdateCurrentFriendsList = true;
		this.SetActiveFriendsListContainer();
	}

	// Token: 0x060035EB RID: 13803 RVA: 0x00116B70 File Offset: 0x00114D70
	private void UpdateFindFriendsState()
	{
		FriendsController sharedController = FriendsController.sharedController;
		this._isFriendsMax = FriendsController.IsFriendsMax();
		if (this.statusBar.IsFindFriendByIdStateActivate)
		{
			this._isAnyFriendsDataExists = !this._isFriendsMax;
		}
		else
		{
			this._isAnyFriendsDataExists = (FriendsController.IsPossibleFriendsDataExist() && !this._isFriendsMax);
		}
		this.statusBar.UpdateFindFriendsState(this._isFriendsMax);
		this.CheckShowEmptyStateTabLabel(this._isAnyFriendsDataExists, this._isFriendsMax);
		this.UpdateList(this.findFriendsWrap, FriendItemPreviewType.find);
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x00116C00 File Offset: 0x00114E00
	private void SetActiveFindFriendsContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.findFriends;
		this.UpdateFindFriendsState();
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x00116C10 File Offset: 0x00114E10
	public void OnClickFindFriendsTab(UIToggle toggle)
	{
		if (toggle.value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.value)
		{
			return;
		}
		this.HideInfoBox();
		this.NeedUpdateCurrentFriendsList = true;
		if (this.statusBar.IsFindFriendByIdStateActivate)
		{
			this.UpdateFriendsArray(FriendsWindowController.WindowState.findFriends, false);
			this.ResetWrapPosition(this.findFriendsWrap);
		}
		this.SetActiveFindFriendsContainer();
		this.statusBar.findFriendInput.value = string.Empty;
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x00116C88 File Offset: 0x00114E88
	private void SetActiveChatFriendsContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.chat;
		bool flag = FriendsController.IsFriendsDataExist();
		this.CheckShowEmptyStateTabLabel(flag, false);
		this.chatContainer.SetActive(flag);
		this.statusBar.OnClickChatTab();
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x00116CC4 File Offset: 0x00114EC4
	public void OnClickFriendsChatTab(UIToggle toggle)
	{
		if (toggle.value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.value)
		{
			return;
		}
		this.HideInfoBox();
		this.SetActiveChatFriendsContainer();
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x00116CFC File Offset: 0x00114EFC
	private void UpdateFriendsInboxState()
	{
		this.UpdateFriendsArray(FriendsWindowController.WindowState.inbox, false);
		this.UpdateList(this.inviteFriendsWrap, FriendItemPreviewType.inbox);
		this._isAnyFriendsDataExists = FriendsController.IsFriendInvitesDataExist();
		this._isFriendsMax = FriendsController.IsFriendsMax();
		this.statusBar.OnClickInboxFriendsTab(this._isAnyFriendsDataExists, this._isFriendsMax);
		this.CheckShowEmptyStateTabLabel(this._isAnyFriendsDataExists, false);
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x00116D58 File Offset: 0x00114F58
	private void SetActiveInboxFriendsContainer()
	{
		this.currentWindowState = FriendsWindowController.WindowState.inbox;
		this.UpdateFriendsInboxState();
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x00116D68 File Offset: 0x00114F68
	public void OnClickInboxFriendsTab(UIToggle toggle)
	{
		if (toggle.value)
		{
			ButtonClickSound.TryPlayClick();
		}
		if (!toggle.value)
		{
			return;
		}
		this.HideInfoBox();
		this.SetActiveInboxFriendsContainer();
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x00116DA0 File Offset: 0x00114FA0
	public void UpdateCurrentTabState()
	{
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.SetActiveFriendsListContainer();
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.inbox)
		{
			this.SetActiveInboxFriendsContainer();
		}
		else if (this.currentWindowState == FriendsWindowController.WindowState.findFriends)
		{
			this.SetActiveFindFriendsContainer();
		}
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x00116DEC File Offset: 0x00114FEC
	private int GetModeByInfo(Dictionary<string, string> onlineData)
	{
		int num = Convert.ToInt32(onlineData["game_mode"]);
		if (num == 6)
		{
			return 1;
		}
		if (num == 7)
		{
			return 2;
		}
		return 0;
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x00116E20 File Offset: 0x00115020
	private int SortFriendsByOnlineStatusAndClickJoin(string friend1, string friend2)
	{
		int num;
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(friend1))
		{
			num = 3;
		}
		else
		{
			Dictionary<string, string> dictionary = FriendsController.sharedController.onlineInfo[friend1];
			if (dictionary == null)
			{
				num = 3;
			}
			else
			{
				num = this.GetModeByInfo(dictionary);
			}
		}
		int num2;
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(friend2))
		{
			num2 = 3;
		}
		else
		{
			Dictionary<string, string> dictionary2 = FriendsController.sharedController.onlineInfo[friend2];
			if (dictionary2 == null)
			{
				num2 = 3;
			}
			else
			{
				num2 = this.GetModeByInfo(dictionary2);
			}
		}
		HashSet<string> hashSet = BattleInviteListener.Instance.GetFriendIds() as HashSet<string>;
		if (hashSet != null)
		{
			bool flag = hashSet.Contains(friend1);
			bool flag2 = hashSet.Contains(friend2);
			if (flag && !flag2)
			{
				return -1;
			}
			if (!flag && flag2)
			{
				return 1;
			}
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		FriendsController sharedController = FriendsController.sharedController;
		if (sharedController == null)
		{
			return 0;
		}
		DateTime dateLastClickJoinFriend = sharedController.GetDateLastClickJoinFriend(friend1);
		DateTime dateLastClickJoinFriend2 = sharedController.GetDateLastClickJoinFriend(friend2);
		if (dateLastClickJoinFriend < dateLastClickJoinFriend2)
		{
			return -1;
		}
		if (dateLastClickJoinFriend > dateLastClickJoinFriend2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x00116F5C File Offset: 0x0011515C
	private int SortFriendsByFindOrigin(string player1, string player2)
	{
		int possibleFriendFindOrigin = (int)FriendsController.GetPossibleFriendFindOrigin(player1);
		int possibleFriendFindOrigin2 = (int)FriendsController.GetPossibleFriendFindOrigin(player2);
		if (possibleFriendFindOrigin < possibleFriendFindOrigin2)
		{
			return -1;
		}
		if (possibleFriendFindOrigin > possibleFriendFindOrigin2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x00116F8C File Offset: 0x0011518C
	private void EventUpdateFriendsOnlineAndSorting()
	{
		if (this.currentWindowState != FriendsWindowController.WindowState.friendList)
		{
			return;
		}
		this.UpdateFriendsOnlineAndSorting(false);
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x00116FA4 File Offset: 0x001151A4
	private void UpdateFriendsOnlineAndSorting(bool isNeedReposition)
	{
		if (FriendsWindowController.UpdateFriendsOnlineEvent != null)
		{
			FriendsWindowController.UpdateFriendsOnlineEvent();
		}
		if (this.currentWindowState == FriendsWindowController.WindowState.friendList)
		{
			this.UpdateFriendsArray(FriendsWindowController.WindowState.friendList, false);
			this.UpdateList(this.GetCurrentWrap(), this.GetPreviewTypeForCurrentWindowState());
		}
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x00116FEC File Offset: 0x001151EC
	private void OnDisable()
	{
		FriendsController.FriendsUpdated -= this.UpdateFriendsListInterface;
		FriendsController.OnShowBoxProcessFriendsData = (Action)Delegate.Remove(FriendsController.OnShowBoxProcessFriendsData, new Action(this.ShowMessageBoxProcessingData));
		FriendsController.OnHideBoxProcessFriendsData = (Action)Delegate.Remove(FriendsController.OnHideBoxProcessFriendsData, new Action(this.HideInfoBox));
		FriendsController.UpdateFriendsInfoAction = (Action)Delegate.Remove(FriendsController.UpdateFriendsInfoAction, new Action(this.EventUpdateFriendsOnlineAndSorting));
		InfoWindowController.HideCurrentWindow();
		BattleInviteListener.Instance.ClearIncomingInvites();
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x0011707C File Offset: 0x0011527C
	private void OnDestroy()
	{
		FriendsController.sharedController.StopRefreshingOnline();
		FriendsController.DisposeProfile();
		FriendsWindowController.Instance = null;
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x00117094 File Offset: 0x00115294
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x001170A4 File Offset: 0x001152A4
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x001170B4 File Offset: 0x001152B4
	private void OnCloseProfileWindow(bool needUpdateFriendList)
	{
		if (needUpdateFriendList)
		{
			this.NeedUpdateCurrentFriendsList = true;
		}
		base.gameObject.SetActive(true);
		this.joinToFriendRoomPhoton.SetActive(true);
		if (this._selectProfileItem != null)
		{
			this._selectProfileItem.UpdateData();
		}
		this.UpdateCurrentTabState();
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x00117108 File Offset: 0x00115308
	public void ShowProfileWindow(string friendId, FriendPreviewItem selectedItem)
	{
		this._selectProfileItem = selectedItem;
		base.gameObject.SetActive(false);
		this.joinToFriendRoomPhoton.SetActive(false);
		FriendsController.ShowProfile(friendId, ProfileWindowType.friend, new Action<bool>(this.OnCloseProfileWindow));
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x00117148 File Offset: 0x00115348
	public void SetActiveChatTab(string id)
	{
		if (PrivateChatController.sharedController != null)
		{
			PrivateChatController.sharedController.selectedPlayerID = id;
		}
		this.chatTab.Set(true);
		this.SetActiveChatFriendsContainer();
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x00117178 File Offset: 0x00115378
	public IEnumerator ShowResultFindPlayer(string param)
	{
		this.HideMessageByEmptyStateTab();
		this.isNeedRebuildFindFriendsList = true;
		this.findFriendsList = new string[]
		{
			string.Empty
		};
		this.UpdateList(this.findFriendsWrap, FriendItemPreviewType.find);
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController != null)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>
			{
				{
					"Search Friends",
					"Search"
				}
			});
			yield return base.StartCoroutine(friendsController.GetInfoByParamCoroutine(param));
		}
		if (!this.statusBar.IsFindFriendByIdStateActivate)
		{
			yield break;
		}
		List<string> findResultIds = friendsController.findPlayersByParamResult;
		if (findResultIds == null)
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1426"));
			this.CheckShowEmptyStateTabLabel(false, false);
			yield break;
		}
		this.findFriendsList = findResultIds.ToArray();
		this.findFriendsWrap.minIndex = this.findFriendsList.Length * -1;
		this.findFriendsWrap.SortAlphabetically();
		this.findFriendsWrap.transform.parent.GetComponent<UIScrollView>().ResetPosition();
		this.UpdateList(this.findFriendsWrap, FriendItemPreviewType.find);
		yield break;
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x001171A4 File Offset: 0x001153A4
	public void OnClickClearAllInboxButton()
	{
		ButtonClickSound.TryPlayClick();
		this.statusBar.clearAllInviteButton.isEnabled = false;
		FriendsController sharedController = FriendsController.sharedController;
		if (sharedController != null)
		{
			sharedController.ClearAllFriendsInvites();
		}
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x001171E0 File Offset: 0x001153E0
	public static bool IsActiveFriendListTab()
	{
		return !(FriendsWindowController.Instance == null) && FriendsWindowController.Instance.currentWindowState == FriendsWindowController.WindowState.friendList;
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x00117204 File Offset: 0x00115404
	public static bool IsActiveFindFriendTab()
	{
		return !(FriendsWindowController.Instance == null) && FriendsWindowController.Instance.currentWindowState == FriendsWindowController.WindowState.findFriends;
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x00117228 File Offset: 0x00115428
	public void SetStartState()
	{
		base.StartCoroutine(this.SetStartStateCoroutine());
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x00117238 File Offset: 0x00115438
	public void SetCancelState()
	{
		this.friendsTab.Set(false);
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x00117248 File Offset: 0x00115448
	private IEnumerator SetStartStateCoroutine()
	{
		this._isWindowInStartState = true;
		UIPanel windowPanel = base.gameObject.GetComponent<UIPanel>();
		windowPanel.alpha = 0.001f;
		yield return null;
		this.chatContainer.SetActive(false);
		this.friendsTab.Set(true);
		windowPanel.alpha = 1f;
		yield break;
	}

	// Token: 0x04002797 RID: 10135
	public static Action UpdateFriendsOnlineEvent;

	// Token: 0x04002798 RID: 10136
	public UIButton goInBattleButton;

	// Token: 0x04002799 RID: 10137
	public UIWrapContent friendsListWrap;

	// Token: 0x0400279A RID: 10138
	public UIWrapContent inviteFriendsWrap;

	// Token: 0x0400279B RID: 10139
	public UIWrapContent findFriendsWrap;

	// Token: 0x0400279C RID: 10140
	private string[] inviteFriendsList;

	// Token: 0x0400279D RID: 10141
	private string[] findFriendsList;

	// Token: 0x0400279E RID: 10142
	private string[] myFriendsList;

	// Token: 0x0400279F RID: 10143
	private string[] localFriendsList;

	// Token: 0x040027A0 RID: 10144
	public GameObject friendPreviewPrefab;

	// Token: 0x040027A1 RID: 10145
	public UIToggle chatTab;

	// Token: 0x040027A2 RID: 10146
	public UIToggle friendsTab;

	// Token: 0x040027A3 RID: 10147
	public FriendsWindowStatusBar statusBar;

	// Token: 0x040027A4 RID: 10148
	public UILabel emptyStateTabLabel;

	// Token: 0x040027A5 RID: 10149
	public GameObject emptyStateLocalPlayersTabLabel;

	// Token: 0x040027A6 RID: 10150
	public GameObject chatContainer;

	// Token: 0x040027A7 RID: 10151
	public GameObject joinToFriendRoomPhoton;

	// Token: 0x040027A8 RID: 10152
	public static FriendsWindowController Instance;

	// Token: 0x040027A9 RID: 10153
	private FriendsWindowController.WindowState currentWindowState;

	// Token: 0x040027AA RID: 10154
	private bool _isAnyFriendsDataExists;

	// Token: 0x040027AB RID: 10155
	private bool _isFriendsMax;

	// Token: 0x040027AC RID: 10156
	private bool wrapsInit;

	// Token: 0x040027AD RID: 10157
	[NonSerialized]
	public bool isNeedRebuildFindFriendsList = true;

	// Token: 0x040027AE RID: 10158
	private FriendPreviewItem _selectProfileItem;

	// Token: 0x040027AF RID: 10159
	private bool _isWindowInStartState;

	// Token: 0x0200061C RID: 1564
	private enum WindowState
	{
		// Token: 0x040027B2 RID: 10162
		friendList,
		// Token: 0x040027B3 RID: 10163
		findFriends,
		// Token: 0x040027B4 RID: 10164
		chat,
		// Token: 0x040027B5 RID: 10165
		inbox
	}
}
