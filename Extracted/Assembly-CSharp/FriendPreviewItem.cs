using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x02000618 RID: 1560
[DisallowMultipleComponent]
public sealed class FriendPreviewItem : MonoBehaviour
{
	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x0600359C RID: 13724 RVA: 0x00114EC8 File Offset: 0x001130C8
	// (set) Token: 0x0600359B RID: 13723 RVA: 0x00114EBC File Offset: 0x001130BC
	public int OnlineCodeStatus { get; private set; }

	// Token: 0x0600359D RID: 13725 RVA: 0x00114ED0 File Offset: 0x001130D0
	private void Update()
	{
		if (this.invitedToBattle)
		{
			if (this.nextBlinkTime < Time.time)
			{
				this.connectToRoomButton.normalSprite = this.defaultButtonSprite;
				this.nextBlinkTime = Time.time + 1f;
			}
			else if (this.nextBlinkTime < Time.time + 0.5f)
			{
				this.connectToRoomButton.normalSprite = this.blinkButtonSprite;
			}
		}
		else
		{
			this.connectToRoomButton.normalSprite = this.defaultButtonSprite;
		}
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x00114F5C File Offset: 0x0011315C
	private void Start()
	{
		this.defaultButtonSprite = this.connectToRoomButton.normalSprite;
		FriendsWindowController.UpdateFriendsOnlineEvent = (Action)Delegate.Combine(FriendsWindowController.UpdateFriendsOnlineEvent, new Action(this.UpdateOnline));
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x00114F90 File Offset: 0x00113190
	private Color[] FlipColorsHorizontally(Color[] colors, int width, int height)
	{
		Color[] array = new Color[colors.Length];
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				array[i + width * j] = colors[width - i - 1 + width * j];
			}
		}
		return array;
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x00114FF0 File Offset: 0x001131F0
	public void SetSkin(string skinStr)
	{
		Texture mainTexture = this.avatarIcon.mainTexture;
		if (mainTexture != null && !mainTexture.name.Equals("dude") && !mainTexture.name.Equals("multi_skin_1"))
		{
			UnityEngine.Object.DestroyImmediate(mainTexture, true);
		}
		this.avatarIcon.mainTexture = Tools.GetPreviewFromSkin(skinStr, Tools.PreviewType.HeadAndBody);
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x00115058 File Offset: 0x00113258
	public void HandleCallFriend()
	{
		string text = ProfileController.GetPlayerNameOrDefault();
		text = FilterBadWorld.FilterString(text);
		WWWForm wwwform = new WWWForm();
		string value = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
		wwwform.AddField("app_version", value);
		wwwform.AddField("player", FriendsController.sharedController.id ?? string.Empty);
		wwwform.AddField("nickname", text ?? string.Empty);
		wwwform.AddField("friend", this.id ?? string.Empty);
		if (Application.isEditor)
		{
			Debug.LogFormat("`HandleCallFriend()`: `{0}`", new object[]
			{
				Encoding.UTF8.GetString(wwwform.data, 0, wwwform.data.Length)
			});
		}
		WWW request = Tools.CreateWwwIfNotConnected("https://secure.pixelgunserver.com/invite_service", wwwform, "FriendPreviewItem.HandleCallFriend()", new Dictionary<string, string>
		{
			{
				"Authorization",
				FriendsController.HashForPush(wwwform.data)
			}
		});
		CoroutineRunner.Instance.StartCoroutine(this.WaitRequestResponse(request));
		AnalyticsStuff.LogBattleInviteSent();
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendInviteFightToPlayer(this.id);
		}
		BattleInviteListener.Instance.SetOutgoingInviteTimestamp(this.id, Time.realtimeSinceStartup);
		bool enabled = BattleInviteListener.Instance.CallToFriendEnabled(this.id);
		this.RefreshCallFriendButton(true, enabled);
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x001151C4 File Offset: 0x001133C4
	private IEnumerator WaitRequestResponse(WWW request)
	{
		if (!Application.isEditor)
		{
			yield break;
		}
		while (!request.isDone)
		{
			yield return null;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogWarningFormat("Request failed: {0}", new object[]
			{
				request.error
			});
			yield break;
		}
		Debug.LogFormat("Request succeeded: {0}", new object[]
		{
			request.text
		});
		yield break;
	}

	// Token: 0x060035A3 RID: 13731 RVA: 0x001151E8 File Offset: 0x001133E8
	private string GetLevelAndNameLabel(string level, string name)
	{
		return string.Format("[b]{0} {1}[/b]", level, name);
	}

	// Token: 0x060035A4 RID: 13732 RVA: 0x001151F8 File Offset: 0x001133F8
	private void FillCommonAttrsByPlayerInfo()
	{
		Dictionary<string, object> fullPlayerDataById = FriendsController.GetFullPlayerDataById(this.id);
		Dictionary<string, object> playerData;
		if (fullPlayerDataById != null && fullPlayerDataById.TryGetValue("player", out playerData))
		{
			this.FillCommonAttrsByPlayerData(playerData);
		}
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x00115230 File Offset: 0x00113430
	private void ResetPositionElementsDetailInfo(bool isPlayerInClan)
	{
		this.clanContainer.gameObject.SetActive(isPlayerInClan);
		this.playerDetailInfoGrid.Reposition();
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x00115250 File Offset: 0x00113450
	private void FillCommonAttrsByPlayerData(Dictionary<string, object> playerData)
	{
		string text = Convert.ToString(playerData["nick"]);
		string str = Convert.ToString(playerData["rank"]);
		this.levelAndName.text = text;
		this.level.spriteName = "Rank_" + str;
		if (playerData.ContainsKey("skin"))
		{
			string skin = playerData["skin"] as string;
			this.SetSkin(skin);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (KeyValuePair<string, object> keyValuePair in playerData)
		{
			dictionary.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
		}
		bool flag = dictionary.ContainsKey("clan_name") && !string.IsNullOrEmpty(dictionary["clan_name"]);
		this.ResetPositionElementsDetailInfo(flag);
		if (flag)
		{
			this.FillClanAttrs(dictionary);
		}
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x00115378 File Offset: 0x00113578
	private void SetupFindStateButtons()
	{
		bool flag = FriendsController.IsAlreadySendInvitePlayer(this.id);
		bool flag2 = FriendsController.IsMyPlayerId(this.id);
		bool flag3 = FriendsController.IsPlayerOurFriend(this.id);
		bool active = !flag3 && !flag2 && !flag;
		this.addFriendButton.gameObject.SetActive(active);
		this.invitationAddSentContainer.gameObject.SetActive(flag);
		this.friendAddContainer.gameObject.SetActive(flag3);
		this.selfAddContainer.gameObject.SetActive(flag2);
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x00115404 File Offset: 0x00113604
	private void ShowButtonsByTypePreview(FriendItemPreviewType typePreview)
	{
		this.friendListButtonContainer.gameObject.SetActive(typePreview == FriendItemPreviewType.view);
		bool flag = typePreview == FriendItemPreviewType.find;
		this.findFrinedsButtonContainer.gameObject.SetActive(flag);
		if (flag)
		{
			this.SetupFindStateButtons();
		}
		this.inboxFriendsButtonContainer.gameObject.SetActive(typePreview == FriendItemPreviewType.inbox);
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x0011545C File Offset: 0x0011365C
	private void HideDetailInfo()
	{
		this.detailInfoConatiner.gameObject.SetActive(false);
		this.playerDetailInfoGrid.Reposition();
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x0011547C File Offset: 0x0011367C
	public void FillData(string playerId, FriendItemPreviewType typeItem)
	{
		this.id = playerId;
		this._type = typeItem;
		this.ShowButtonsByTypePreview(typeItem);
		this.FillCommonAttrsByPlayerInfo();
		this.inYourNetworkIcon.SetActive(false);
		if (typeItem == FriendItemPreviewType.find)
		{
			this.FindOrigin = (int)FriendsController.GetPossibleFriendFindOrigin(playerId);
			if (this.FindOrigin == 0)
			{
				this.HideDetailInfo();
			}
			else
			{
				this.SetStatusLabelFindOrigin((FriendsController.PossiblleOrigin)this.FindOrigin);
			}
		}
		else if (typeItem != FriendItemPreviewType.view)
		{
			this.HideDetailInfo();
		}
		this.UpdateOnline();
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x00115500 File Offset: 0x00113700
	private void FillClanAttrs(Dictionary<string, string> plDict)
	{
		if (plDict.ContainsKey("clan_logo") && !string.IsNullOrEmpty(plDict["clan_logo"]) && !plDict["clan_logo"].Equals("null"))
		{
			this.clanIcon.gameObject.SetActive(true);
			try
			{
				byte[] data = Convert.FromBase64String(plDict["clan_logo"]);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D.LoadImage(data);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				Texture mainTexture = this.clanIcon.mainTexture;
				this.clanIcon.mainTexture = texture2D;
				if (mainTexture != null)
				{
					UnityEngine.Object.DestroyImmediate(mainTexture, true);
				}
			}
			catch (Exception ex)
			{
				Texture mainTexture2 = this.clanIcon.mainTexture;
				this.clanIcon.mainTexture = null;
				if (mainTexture2 != null)
				{
					UnityEngine.Object.DestroyImmediate(mainTexture2, true);
				}
			}
		}
		else
		{
			this.clanIcon.gameObject.SetActive(false);
		}
		if (plDict.ContainsKey("clan_name") && !string.IsNullOrEmpty(plDict["clan_name"]) && !plDict["clan_name"].Equals("null"))
		{
			this.clanName.gameObject.SetActive(true);
			string text = plDict["clan_name"];
			if (text != null)
			{
				this.clanName.text = text;
			}
		}
		else
		{
			this.clanName.gameObject.SetActive(false);
		}
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x001156B4 File Offset: 0x001138B4
	private void SetStatusLabelPlayerBusy()
	{
		if (!this.detailInfoConatiner.gameObject.activeSelf)
		{
			this.detailInfoConatiner.gameObject.SetActive(true);
			this.playerDetailInfoGrid.Reposition();
		}
		this.detailInfo.text = string.Format("[ff0000]{0}[-]", LocalizationStore.Get("Key_0576"));
		this.RefreshCallFriendButton(false);
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x00115718 File Offset: 0x00113918
	private void SetStatusLabelPlayerPlaying(string gameModeName, string mapName)
	{
		if (!this.detailInfoConatiner.gameObject.activeSelf)
		{
			this.detailInfoConatiner.gameObject.SetActive(true);
			this.playerDetailInfoGrid.Reposition();
		}
		if (string.IsNullOrEmpty(mapName))
		{
			this.detailInfo.text = string.Format("[00aeff]{0}[-]", gameModeName);
		}
		else
		{
			this.detailInfo.text = string.Format("[77ef00]{0}: {1}[-]", gameModeName, mapName);
		}
		this.RefreshCallFriendButton(false);
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x0011579C File Offset: 0x0011399C
	private void SetStatusLabelFindOrigin(FriendsController.PossiblleOrigin findOrigin)
	{
		this.invitedBackground.gameObject.SetActive(false);
		this.invitedToBattle = false;
		if (!this.detailInfoConatiner.gameObject.activeSelf)
		{
			this.detailInfoConatiner.gameObject.SetActive(true);
			this.playerDetailInfoGrid.Reposition();
		}
		if (findOrigin == FriendsController.PossiblleOrigin.None)
		{
			return;
		}
		if (findOrigin == FriendsController.PossiblleOrigin.Local)
		{
			this.detailInfo.text = string.Format("[ffe400]{0}[-]", LocalizationStore.Get("Key_1569"));
			this.inYourNetworkIcon.SetActive(true);
		}
		else if (findOrigin == FriendsController.PossiblleOrigin.Facebook)
		{
			this.detailInfo.text = string.Format("[00aeff]{0}[-]", LocalizationStore.Get("Key_1570"));
		}
		else if (findOrigin == FriendsController.PossiblleOrigin.RandomPlayer)
		{
			this.detailInfo.text = string.Format("[77ef00]{0}[-]", LocalizationStore.Get("Key_1571"));
		}
		this.RefreshCallFriendButton(false);
	}

	// Token: 0x060035AF RID: 13743 RVA: 0x00115888 File Offset: 0x00113A88
	private void SetStateButtonConnectContainer(bool isCanConnect, string conditionNotConnect)
	{
		this.connectToRoomButton.gameObject.SetActive(isCanConnect);
		this.notConnectLabel.gameObject.SetActive(!isCanConnect);
		this.notConnectLabel.text = conditionNotConnect;
		bool enabled = BattleInviteListener.Instance.CallToFriendEnabled(this.id);
		this.RefreshCallFriendButton(!isCanConnect && FriendsController.sharedController.friends.Contains(this.id), enabled);
		HashSet<string> hashSet = BattleInviteListener.Instance.GetFriendIds() as HashSet<string>;
		this.invitedToBattle = hashSet.Contains(this.id);
		this.invitedBackground.gameObject.SetActive(this.invitedToBattle);
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x00115934 File Offset: 0x00113B34
	private void SetOfflineStatePreview()
	{
		this.SetStatusLabelPlayerBusy();
		this.detailInfo.text = string.Empty;
		bool flag = BattleInviteListener.Instance.CallToFriendEnabled(this.id);
		this.OnlineCodeStatus = 3;
		this.SetStateButtonConnectContainer(false, LocalizationStore.Get("Key_1577"));
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x00115980 File Offset: 0x00113B80
	private void RefreshCallFriendButton(bool active)
	{
		if (this._callFriendButtonHolder == null)
		{
			return;
		}
		this._callFriendButtonHolder.SetActive(active);
	}

	// Token: 0x060035B2 RID: 13746 RVA: 0x001159A0 File Offset: 0x00113BA0
	private void RefreshCallFriendButton(bool active, bool enabled)
	{
		if (this._callFriendButtonHolder == null)
		{
			return;
		}
		this._callFriendButtonHolder.SetActive(active);
		UIButton componentInChildren = this._callFriendButtonHolder.GetComponentInChildren<UIButton>();
		if (componentInChildren == null)
		{
			return;
		}
		this.friendCallLabel.SetActive(enabled);
		this.friendCalledLabel.SetActive(!enabled);
		componentInChildren.isEnabled = enabled;
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x00115A08 File Offset: 0x00113C08
	private void UpdateOnline()
	{
		if (this._type == FriendItemPreviewType.find)
		{
			return;
		}
		if (!FriendsController.sharedController.onlineInfo.ContainsKey(this.id))
		{
			this.SetOfflineStatePreview();
			return;
		}
		Dictionary<string, string> onlineData = FriendsController.sharedController.onlineInfo[this.id];
		FriendsController.ResultParseOnlineData resultParseOnlineData = FriendsController.ParseOnlineData(onlineData);
		if (resultParseOnlineData == null)
		{
			this.SetOfflineStatePreview();
			return;
		}
		this.SetStatusLabelPlayerPlaying(resultParseOnlineData.GetGameModeName(), resultParseOnlineData.GetMapName());
		this.SetStateButtonConnectContainer(resultParseOnlineData.IsCanConnect, resultParseOnlineData.GetNotConnectConditionShortString());
		this.OnlineCodeStatus = (int)resultParseOnlineData.GetOnlineStatus();
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x00115AA0 File Offset: 0x00113CA0
	private void OnDestroy()
	{
		FriendsWindowController.UpdateFriendsOnlineEvent = (Action)Delegate.Remove(FriendsWindowController.UpdateFriendsOnlineEvent, new Action(this.UpdateOnline));
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x00115AD0 File Offset: 0x00113CD0
	private void CallbackFriendAddRequest(bool isComplete, bool isRequestExist)
	{
		this.addFriendButton.enabled = true;
		InfoWindowController.CheckShowRequestServerInfoBox(isComplete, isRequestExist);
		if (isComplete)
		{
			this.SetupFindStateButtons();
		}
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x00115AF4 File Offset: 0x00113CF4
	public void OnClickAddFriend()
	{
		if (string.IsNullOrEmpty(this.id))
		{
			return;
		}
		ButtonClickSound.TryPlayClick();
		this.addFriendButton.enabled = false;
		bool flag = FriendsWindowController.Instance.Map((FriendsWindowController fwc) => fwc.statusBar).Map((FriendsWindowStatusBar s) => s.IsFindFriendByIdStateActivate);
		string value = (!flag) ? string.Format("Find Friends: {0}", (FriendsController.PossiblleOrigin)this.FindOrigin) : "Search";
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			{
				"Added Friends",
				value
			},
			{
				"Deleted Friends",
				"Add"
			}
		};
		if (flag)
		{
			dictionary.Add("Search Friends", "Add");
		}
		FriendsController.SendFriendshipRequest(this.id, dictionary, new Action<bool, bool>(this.CallbackFriendAddRequest));
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x00115BE8 File Offset: 0x00113DE8
	public void OnClickConnectToFriendRoom()
	{
		ButtonClickSound.TryPlayClick();
		FriendsController.JoinToFriendRoom(this.id);
		if (this.invitedToBattle)
		{
			AnalyticsStuff.LogBattleInviteAccepted();
		}
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x00115C18 File Offset: 0x00113E18
	public void OnClickGoTohatButton()
	{
		FriendsWindowController.Instance.SetActiveChatTab(this.id);
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x00115C2C File Offset: 0x00113E2C
	private void OnCompleteAcceptInviteAction(bool isComplete)
	{
		InfoWindowController.CheckShowRequestServerInfoBox(isComplete, false);
		this.acceptInviteButton.isEnabled = true;
		this.cancelInviteButton.isEnabled = true;
		if (isComplete)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>
			{
				{
					"Friend Requests",
					"Accepted"
				}
			});
			FriendsWindowController.Instance.UpdateCurrentTabState();
		}
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x00115C8C File Offset: 0x00113E8C
	private void OnCompletetRejectInviteAction(bool isComplete)
	{
		InfoWindowController.CheckShowRequestServerInfoBox(isComplete, false);
		this.acceptInviteButton.isEnabled = true;
		this.cancelInviteButton.isEnabled = true;
		if (isComplete)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>
			{
				{
					"Friend Requests",
					"Rejected"
				}
			});
			FriendsWindowController.Instance.UpdateCurrentTabState();
		}
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x00115CEC File Offset: 0x00113EEC
	public void OnClickAcceptButton()
	{
		ButtonClickSound.TryPlayClick();
		this.acceptInviteButton.isEnabled = false;
		this.cancelInviteButton.isEnabled = false;
		bool flag = FriendsController.IsFriendsMax();
		if (flag)
		{
			InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_1424"));
			return;
		}
		FriendsController.sharedController.AcceptInvite(this.id, new Action<bool>(this.OnCompleteAcceptInviteAction));
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x00115D50 File Offset: 0x00113F50
	public void OnClickDeclineButton()
	{
		this.acceptInviteButton.isEnabled = false;
		this.cancelInviteButton.isEnabled = false;
		ButtonClickSound.TryPlayClick();
		FriendsController.sharedController.RejectInvite(this.id, new Action<bool>(this.OnCompletetRejectInviteAction));
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x00115D98 File Offset: 0x00113F98
	public void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		FriendsWindowController.Instance.ShowProfileWindow(this.id, this);
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x00115DB0 File Offset: 0x00113FB0
	public void UpdateData()
	{
		if (string.IsNullOrEmpty(this.id))
		{
			return;
		}
		this.FillData(this.id, this._type);
	}

	// Token: 0x04002763 RID: 10083
	public string id;

	// Token: 0x04002764 RID: 10084
	public UIWidget friendListButtonContainer;

	// Token: 0x04002765 RID: 10085
	public UIWidget findFrinedsButtonContainer;

	// Token: 0x04002766 RID: 10086
	public UIWidget inboxFriendsButtonContainer;

	// Token: 0x04002767 RID: 10087
	public UIButton connectToRoomButton;

	// Token: 0x04002768 RID: 10088
	public UIButton goToChat;

	// Token: 0x04002769 RID: 10089
	public UILabel levelAndName;

	// Token: 0x0400276A RID: 10090
	public UISprite level;

	// Token: 0x0400276B RID: 10091
	public UIWidget detailInfoConatiner;

	// Token: 0x0400276C RID: 10092
	public UILabel detailInfo;

	// Token: 0x0400276D RID: 10093
	public UITexture avatarIcon;

	// Token: 0x0400276E RID: 10094
	public UIWidget clanContainer;

	// Token: 0x0400276F RID: 10095
	public UITexture clanIcon;

	// Token: 0x04002770 RID: 10096
	public UILabel clanName;

	// Token: 0x04002771 RID: 10097
	public UISprite playingIcon;

	// Token: 0x04002772 RID: 10098
	public UISprite inFriendsIcon;

	// Token: 0x04002773 RID: 10099
	public UISprite offlineIcon;

	// Token: 0x04002774 RID: 10100
	public UISprite invitedBackground;

	// Token: 0x04002775 RID: 10101
	public UIGrid playerDetailInfoGrid;

	// Token: 0x04002776 RID: 10102
	[Header("Not connect button")]
	public UISprite notConnectIcon;

	// Token: 0x04002777 RID: 10103
	public UILabel notConnectLabel;

	// Token: 0x04002778 RID: 10104
	[Header("add friend button")]
	public UIButton addFriendButton;

	// Token: 0x04002779 RID: 10105
	public UIWidget invitationAddSentContainer;

	// Token: 0x0400277A RID: 10106
	public UIWidget friendAddContainer;

	// Token: 0x0400277B RID: 10107
	public UIWidget selfAddContainer;

	// Token: 0x0400277C RID: 10108
	[Header("inbox button")]
	public UIButton acceptInviteButton;

	// Token: 0x0400277D RID: 10109
	public UIButton cancelInviteButton;

	// Token: 0x0400277E RID: 10110
	[NonSerialized]
	public int FindOrigin;

	// Token: 0x0400277F RID: 10111
	public int myWrapIndex;

	// Token: 0x04002780 RID: 10112
	private FriendItemPreviewType _type;

	// Token: 0x04002781 RID: 10113
	public GameObject inYourNetworkIcon;

	// Token: 0x04002782 RID: 10114
	[SerializeField]
	private GameObject _callFriendButtonHolder;

	// Token: 0x04002783 RID: 10115
	[SerializeField]
	private GameObject friendCallLabel;

	// Token: 0x04002784 RID: 10116
	[SerializeField]
	private GameObject friendCalledLabel;

	// Token: 0x04002785 RID: 10117
	public string blinkButtonSprite = "blue_btn";

	// Token: 0x04002786 RID: 10118
	private string defaultButtonSprite;

	// Token: 0x04002787 RID: 10119
	private bool invitedToBattle;

	// Token: 0x04002788 RID: 10120
	private float nextBlinkTime;
}
