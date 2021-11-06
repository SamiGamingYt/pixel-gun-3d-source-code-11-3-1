using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class PrivateChatController : MonoBehaviour
{
	// Token: 0x06002A1A RID: 10778 RVA: 0x000DDD30 File Offset: 0x000DBF30
	private void Awake()
	{
		this.heightFriends = (float)this.friendsWrap.itemSize;
		this.scrollTransform = this.scrollMessages.transform;
		this.scrollFriendsTransform = this.scrollFriends.transform;
		this.scrollPanel = this.scrollMessages.GetComponent<UIPanel>();
		this.scrollFriensPanel = this.scrollFriends.GetComponent<UIPanel>();
		PrivateChatController.sharedController = this;
		if (this.sendMessageInput != null)
		{
			MyUIInput myUIInput = this.sendMessageInput;
			myUIInput.onKeyboardInter = (Action)Delegate.Combine(myUIInput.onKeyboardInter, new Action(this.SendMessageFromInput));
			MyUIInput myUIInput2 = this.sendMessageInput;
			myUIInput2.onKeyboardCancel = (Action)Delegate.Combine(myUIInput2.onKeyboardCancel, new Action(this.CancelSendPrivateMessage));
			MyUIInput myUIInput3 = this.sendMessageInput;
			myUIInput3.onKeyboardVisible = (Action)Delegate.Combine(myUIInput3.onKeyboardVisible, new Action(this.OnKeyboardVisible));
			MyUIInput myUIInput4 = this.sendMessageInput;
			myUIInput4.onKeyboardHide = (Action)Delegate.Combine(myUIInput4.onKeyboardHide, new Action(this.OnKeyboardHide));
		}
		this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.stickerPosHide, this.smilePanelTransform.localPosition.z);
		this.isShowSmilePanel = false;
		this.isBuySmile = StickersController.IsBuyAnyPack();
		if (this.isBuySmile)
		{
			this.showSmileButton.SetActive(true);
			this.buySmileButton.SetActive(false);
		}
		else
		{
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(true);
		}
		this.hideSmileButton.SetActive(false);
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000DDEE0 File Offset: 0x000DC0E0
	private void OnEnable()
	{
		FriendsController.FriendsUpdated += this.Start_UpdateFriendList;
		this.Start_UpdateFriendListCore(true);
		this.sendMessageInput.value = string.Empty;
		if (string.IsNullOrEmpty(this.selectedPlayerID) && this._friends.Count > 0)
		{
			this.selectedPlayerID = this._friends[0];
		}
		base.StartCoroutine(this.SetSelectedPlayerWithPause(this.selectedPlayerID, true));
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x000DDF5C File Offset: 0x000DC15C
	private IEnumerator SetSelectedPlayerWithPause(string _playerId, bool updateToogleState = true)
	{
		yield return null;
		this.SetSelectedPlayer(this.selectedPlayerID, updateToogleState);
		yield break;
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x000DDF88 File Offset: 0x000DC188
	private void OnDisable()
	{
		this.OnKeyboardHide();
		this.HideSmilePannelOnClick();
		this.sendMessageInput.DeselectInput();
		FriendsController.FriendsUpdated -= this.Start_UpdateFriendList;
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x000DDFC0 File Offset: 0x000DC1C0
	private void Start_UpdateFriendListCore(bool isUpdatePos)
	{
		base.StartCoroutine(this.UpdateFriendList(isUpdatePos));
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x000DDFD0 File Offset: 0x000DC1D0
	private void Start_UpdateFriendList()
	{
		this.Start_UpdateFriendListCore(false);
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x000DDFDC File Offset: 0x000DC1DC
	private void OnDestroy()
	{
		FriendsController.FriendsUpdated -= this.Start_UpdateFriendList;
		PrivateChatController.sharedController = null;
		if (this.sendMessageInput != null)
		{
			MyUIInput myUIInput = this.sendMessageInput;
			myUIInput.onKeyboardInter = (Action)Delegate.Remove(myUIInput.onKeyboardInter, new Action(this.SendPrivateMessage));
			MyUIInput myUIInput2 = this.sendMessageInput;
			myUIInput2.onKeyboardCancel = (Action)Delegate.Remove(myUIInput2.onKeyboardCancel, new Action(this.CancelSendPrivateMessage));
			MyUIInput myUIInput3 = this.sendMessageInput;
			myUIInput3.onKeyboardVisible = (Action)Delegate.Remove(myUIInput3.onKeyboardVisible, new Action(this.OnKeyboardVisible));
			MyUIInput myUIInput4 = this.sendMessageInput;
			myUIInput4.onKeyboardHide = (Action)Delegate.Remove(myUIInput4.onKeyboardHide, new Action(this.OnKeyboardHide));
		}
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000DE0B0 File Offset: 0x000DC2B0
	private void onFriendItemWrap(GameObject go, int wrapInd, int realInd)
	{
		go.GetComponent<FriendPrevInChatItem>().myWrapIndex = Mathf.Abs(realInd);
		this.UpdateItemInfo(go.GetComponent<FriendPrevInChatItem>());
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x000DE0D0 File Offset: 0x000DC2D0
	private void UpdateItemInfo(FriendPrevInChatItem previewItem)
	{
		if (this._friends.Count > previewItem.myWrapIndex)
		{
			if (!previewItem.gameObject.activeSelf)
			{
				previewItem.gameObject.SetActive(true);
			}
			string text = this._friends[previewItem.myWrapIndex];
			string text2 = text;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, object> dictionary2;
			if (FriendsController.sharedController.friendsInfo.ContainsKey(text2) && FriendsController.sharedController.friendsInfo[text2].TryGetValue("player", out dictionary2))
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					dictionary.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
				}
				previewItem.playerID = text2;
				previewItem.UpdateCountNewMessage();
				previewItem.nickLabel.text = dictionary["nick"];
				previewItem.rank.spriteName = "Rank_" + dictionary["rank"];
				previewItem.previewTexture.mainTexture = Tools.GetPreviewFromSkin(dictionary["skin"], Tools.PreviewType.Head);
				previewItem.GetComponent<UIToggle>().Set(this.selectedPlayerID == text2);
			}
		}
		else if (previewItem.gameObject.activeSelf)
		{
			previewItem.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x000DE260 File Offset: 0x000DC460
	private int SortByMessagesCount(string x, string y)
	{
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		if (ChatController.privateMessages.ContainsKey(x))
		{
			for (int i = 0; i < ChatController.privateMessages[x].Count; i++)
			{
				double timeStamp = ChatController.privateMessages[x][i].timeStamp;
				if (timeStamp > num)
				{
					num = timeStamp;
				}
				if (!ChatController.privateMessages[x][i].isRead)
				{
					num3++;
				}
			}
		}
		if (ChatController.privateMessages.ContainsKey(y))
		{
			for (int j = 0; j < ChatController.privateMessages[y].Count; j++)
			{
				double timeStamp2 = ChatController.privateMessages[y][j].timeStamp;
				if (timeStamp2 > num2)
				{
					num2 = timeStamp2;
				}
				if (!ChatController.privateMessages[y][j].isRead)
				{
					num4++;
				}
			}
		}
		if (ChatController.privateMessagesForSend.ContainsKey(x))
		{
			for (int k = 0; k < ChatController.privateMessagesForSend[x].Count; k++)
			{
				double timeStamp3 = ChatController.privateMessagesForSend[x][k].timeStamp;
				if (timeStamp3 > num)
				{
					num = timeStamp3;
				}
			}
		}
		if (ChatController.privateMessagesForSend.ContainsKey(y))
		{
			for (int l = 0; l < ChatController.privateMessagesForSend[y].Count; l++)
			{
				double timeStamp4 = ChatController.privateMessagesForSend[y][l].timeStamp;
				if (timeStamp4 > num2)
				{
					num2 = timeStamp4;
				}
			}
		}
		return (num3 != num4) ? ((num3 >= num4) ? -1 : 1) : ((num > num2) ? -1 : 1);
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x000DE47C File Offset: 0x000DC67C
	public void UpdateFriendItemsInfoAndSort()
	{
		this._friends = new List<string>(this.friendsWithInfo);
		this._friends.Sort(new Comparison<string>(this.SortByMessagesCount));
		FriendPrevInChatItem[] componentsInChildren = this.friendsWrap.GetComponentsInChildren<FriendPrevInChatItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.UpdateItemInfo(componentsInChildren[i]);
		}
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x000DE4DC File Offset: 0x000DC6DC
	public IEnumerator UpdateFriendList(bool isUpdatePos = false)
	{
		this.friendsWithInfo.Clear();
		for (int i = 0; i < FriendsController.sharedController.friends.Count; i++)
		{
			string _friend = FriendsController.sharedController.friends[i];
			if (FriendsController.sharedController.friendsInfo.ContainsKey(_friend))
			{
				this.friendsWithInfo.Add(_friend);
			}
		}
		if (!this.wrapsInit)
		{
			this.friendsWrap.onInitializeItem = new UIWrapContent.OnInitializeItem(this.onFriendItemWrap);
			this.friendPreviewPrefab.transform.GetComponent<UIDragScrollView>().scrollView = this.friendsWrap.GetComponent<UIScrollView>();
			for (int f = 0; f < 16; f++)
			{
				GameObject friendPreviewItem = NGUITools.AddChild(this.friendsWrap.gameObject, this.friendPreviewPrefab);
				friendPreviewItem.name = "FriendPreviewItem_" + f;
				friendPreviewItem.GetComponent<UIToggle>().group = 3;
			}
			this.scrollFriends.ResetPosition();
			this.friendsWrap.SortAlphabetically();
			this.wrapsInit = true;
		}
		this._friends = new List<string>(this.friendsWithInfo);
		this._friends.Sort(new Comparison<string>(this.SortByMessagesCount));
		this.friendsWrap.minIndex = this._friends.Count * -1;
		FriendPrevInChatItem[] previewItems = this.friendsWrap.GetComponentsInChildren<FriendPrevInChatItem>(true);
		for (int j = 0; j < previewItems.Length; j++)
		{
			this.UpdateItemInfo(previewItems[j]);
		}
		if (!string.IsNullOrEmpty(this.selectedPlayerID) && !FriendsController.sharedController.friends.Contains(this.selectedPlayerID))
		{
			this.selectedPlayerID = string.Empty;
			this.SetSelectedPlayer(this.selectedPlayerID, true);
			this.OnKeyboardHide();
			this.sendMessageInput.DeselectInput();
		}
		if (this.wrapsInit)
		{
			this.friendsWrap.SortAlphabetically();
			this.scrollFriends.ResetPosition();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x000DE4F8 File Offset: 0x000DC6F8
	public void SetSelectedPlayer(string _playerId, bool updateToogleState = true)
	{
		this.selectedPlayerItem = null;
		if (string.IsNullOrEmpty(_playerId))
		{
			this.sendMessageInput.gameObject.SetActive(false);
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(false);
			this.hideSmileButton.SetActive(false);
		}
		else
		{
			if (!this.sendMessageInput.gameObject.activeSelf)
			{
				this.sendMessageInput.gameObject.SetActive(true);
			}
			this.showSmileButton.SetActive(this.isBuySmile && !this.isShowSmilePanel);
			this.buySmileButton.SetActive(!this.isBuySmile);
			this.hideSmileButton.SetActive(this.isBuySmile && this.isShowSmilePanel);
		}
		FriendPrevInChatItem[] componentsInChildren = this.friendsWrap.GetComponentsInChildren<FriendPrevInChatItem>(true);
		for (int i = 0; i < this._friends.Count; i++)
		{
			if (this._friends[i].Equals(_playerId))
			{
				float num = (float)(42 * i);
				if (Mathf.Abs(this.scrollFriensPanel.clipOffset.y + 291f) > num)
				{
					float yPosition = num - (this.scrollFriends.transform.localPosition.y - 291f);
					this.MoveFriendWrapToPosition(yPosition);
				}
				if (Mathf.Abs(this.scrollFriensPanel.clipOffset.y + 291f) + this.scrollFriensPanel.baseClipRegion.w - 42f < num)
				{
					float yPosition2 = num - (this.scrollFriends.transform.localPosition.y - 291f + this.scrollFriensPanel.baseClipRegion.w - 50f);
					this.MoveFriendWrapToPosition(yPosition2);
				}
			}
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].playerID.Equals(_playerId))
			{
				this.selectedPlayerItem = componentsInChildren[j];
			}
		}
		this.selectedPlayerID = _playerId;
		this.UpdateMessageForSelectedUsers(true);
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000DE72C File Offset: 0x000DC92C
	private void MoveFriendWrapToPosition(float yPosition)
	{
		bool flag = yPosition < 0f;
		float num = Mathf.Abs(yPosition);
		int num2 = (int)Mathf.Floor(num / 42f);
		float num3 = num - (float)(42 * num2);
		for (int i = 0; i < num2; i++)
		{
			this.scrollFriends.MoveRelative(new Vector3(0f, (float)((!flag) ? 42 : -42), 0f));
		}
		this.scrollFriends.MoveRelative(new Vector3(0f, (!flag) ? num3 : (-num3), 0f));
		SpringPanel.Begin(this.scrollFriensPanel.gameObject, this.scrollFriensPanel.transform.localPosition, 100000f);
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000DE7F0 File Offset: 0x000DC9F0
	public void UpdateMessageForSelectedUsers(bool resetPosition = false)
	{
		this.UpdateMessageForSelectedUsersCoroutine(resetPosition);
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x000DE7FC File Offset: 0x000DC9FC
	private void UpdateMessageItemInfo(PrivateMessageItem messageItem)
	{
		if (this.curListMessages.Count > messageItem.myWrapIndex)
		{
			if (!messageItem.gameObject.activeSelf)
			{
				messageItem.gameObject.SetActive(true);
			}
			messageItem.transform.localPosition = new Vector3(0f, messageItem.transform.localPosition.y, messageItem.transform.localPosition.z);
			messageItem.SetWidth(Mathf.RoundToInt(this.scrollPanel.baseClipRegion.z));
			ChatController.PrivateMessage privateMessage = this.curListMessages[messageItem.myWrapIndex];
			messageItem.isRead = privateMessage.isRead;
			messageItem.timeStamp = privateMessage.timeStamp.ToString("F8", CultureInfo.InvariantCulture);
			if (privateMessage.playerIDFrom.Equals(FriendsController.sharedController.id))
			{
				messageItem.SetFon(true);
				messageItem.otherMessageLabel.text = string.Empty;
				if (privateMessage.message.Contains(Defs.SmileMessageSuffix))
				{
					messageItem.yourSmileSprite.spriteName = privateMessage.message.Substring(Defs.SmileMessageSuffix.Length);
					messageItem.yourMessageLabel.text = string.Empty;
					messageItem.yourMessageLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
					messageItem.yourMessageLabel.height = 80;
					messageItem.yourMessageLabel.width = 80;
					messageItem.yourSmileSprite.gameObject.SetActive(true);
				}
				else
				{
					messageItem.yourMessageLabel.text = privateMessage.message;
					messageItem.yourMessageLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
					messageItem.yourMessageLabel.width = Mathf.CeilToInt((float)messageItem.yourWidget.width * 0.8f);
					messageItem.yourSmileSprite.gameObject.SetActive(false);
				}
				if (privateMessage.isSending)
				{
					DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime((long)((int)privateMessage.timeStamp + DateTimeOffset.Now.Offset.Hours * 3600));
					messageItem.yourTimeLabel.text = string.Concat(new object[]
					{
						currentTimeByUnixTime.Day.ToString("D2"),
						".",
						currentTimeByUnixTime.Month.ToString("D2"),
						".",
						currentTimeByUnixTime.Year,
						" ",
						currentTimeByUnixTime.Hour,
						":",
						currentTimeByUnixTime.Minute.ToString("D2")
					});
				}
				else
				{
					messageItem.yourTimeLabel.text = LocalizationStore.Get("Key_1556");
				}
			}
			else
			{
				messageItem.SetFon(false);
				messageItem.yourMessageLabel.text = string.Empty;
				if (privateMessage.message.Contains(Defs.SmileMessageSuffix))
				{
					messageItem.otherSmileSprite.spriteName = privateMessage.message.Substring(Defs.SmileMessageSuffix.Length);
					messageItem.otherMessageLabel.text = string.Empty;
					messageItem.otherMessageLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
					messageItem.otherMessageLabel.height = 80;
					messageItem.otherMessageLabel.width = 80;
					messageItem.otherSmileSprite.gameObject.SetActive(true);
				}
				else
				{
					messageItem.otherMessageLabel.text = privateMessage.message;
					messageItem.otherMessageLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
					messageItem.otherMessageLabel.width = Mathf.CeilToInt((float)messageItem.otherWidget.width * 0.8f);
					messageItem.otherSmileSprite.gameObject.SetActive(false);
				}
				DateTime currentTimeByUnixTime2 = Tools.GetCurrentTimeByUnixTime((long)((int)privateMessage.timeStamp + DateTimeOffset.Now.Offset.Hours * 3600));
				messageItem.otherTimeLabel.text = string.Concat(new object[]
				{
					currentTimeByUnixTime2.Day.ToString("D2"),
					".",
					currentTimeByUnixTime2.Month.ToString("D2"),
					".",
					currentTimeByUnixTime2.Year,
					" ",
					currentTimeByUnixTime2.Hour,
					":",
					currentTimeByUnixTime2.Minute.ToString("D2")
				});
			}
		}
		else if (messageItem.gameObject.activeSelf)
		{
			messageItem.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x000DECA4 File Offset: 0x000DCEA4
	private void UpdateMessageForSelectedUsersCoroutine(bool resetPosition)
	{
		this.curListMessages.Clear();
		if (!string.IsNullOrEmpty(this.selectedPlayerID))
		{
			if (ChatController.privateMessages.ContainsKey(this.selectedPlayerID))
			{
				this.curListMessages.AddRange(ChatController.privateMessages[this.selectedPlayerID]);
			}
			if (ChatController.privateMessagesForSend.ContainsKey(this.selectedPlayerID))
			{
				this.curListMessages.AddRange(ChatController.privateMessagesForSend[this.selectedPlayerID]);
			}
		}
		this.curListMessages.Sort((ChatController.PrivateMessage x, ChatController.PrivateMessage y) => (x.timeStamp > y.timeStamp) ? -1 : 1);
		if (this.selectedPlayerItem != null)
		{
			this.UpdateItemInfo(this.selectedPlayerItem);
		}
		while (this.privateMessageItems.Count < this.curListMessages.Count)
		{
			GameObject gameObject = NGUITools.AddChild(this.messageTable.gameObject, this.messagePrefab);
			if (this.privateMessageItems.Count > 0)
			{
				gameObject.transform.position = this.privateMessageItems[0].transform.position;
			}
			gameObject.name = this.privateMessageItems.Count.ToString();
			gameObject.GetComponent<PrivateMessageItem>().myWrapIndex = this.privateMessageItems.Count;
			this.privateMessageItems.Add(gameObject.GetComponent<PrivateMessageItem>());
		}
		while (this.privateMessageItems.Count > this.curListMessages.Count)
		{
			UnityEngine.Object.Destroy(this.privateMessageItems[this.privateMessageItems.Count - 1].gameObject);
			this.privateMessageItems.RemoveAt(this.privateMessageItems.Count - 1);
			for (int i = 0; i < this.privateMessageItems.Count; i++)
			{
				this.privateMessageItems[i].gameObject.name = i.ToString();
				this.privateMessageItems[i].myWrapIndex = i;
			}
		}
		this.messageTable.onCustomSort = ((Transform x, Transform y) => (int.Parse(x.name) <= int.Parse(y.name)) ? -1 : 1);
		for (int j = 0; j < this.privateMessageItems.Count; j++)
		{
			this.UpdateMessageItemInfo(this.privateMessageItems[j]);
		}
		this.messageTable.transform.localPosition = new Vector3(this.scrollPanel.baseClipRegion.x, this.messageTable.transform.localPosition.y, this.messageTable.transform.localPosition.z);
		this.messageTable.repositionNow = true;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.RepositionNextFrame(resetPosition));
		}
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x000DEF9C File Offset: 0x000DD19C
	private IEnumerator RepositionNextFrame(bool resetPosition)
	{
		yield return new WaitForEndOfFrame();
		this.messageTable.repositionNow = true;
		this.scrollMessages.ResetPosition();
		yield break;
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x000DEFB8 File Offset: 0x000DD1B8
	private void Update()
	{
		if (!this.isKeyboardVisible)
		{
			this.panelSmiles.UpdateAnchors();
			this.smilePanelTransform.GetComponent<UISprite>().UpdateAnchors();
			this.leftInputAnchor.GetComponent<UIWidget>().UpdateAnchors();
		}
		if (this.isShowSmilePanel && this.smilePanelTransform.localPosition.y < this.stickerPosShow)
		{
			this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.smilePanelTransform.localPosition.y + Time.deltaTime * this.speedHideOrShowStiker, this.smilePanelTransform.localPosition.z);
			this.scrollMessages.MoveRelative(Vector3.up * Time.deltaTime * this.speedHideOrShowStiker);
			if (this.smilePanelTransform.localPosition.y > this.stickerPosShow)
			{
				this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.stickerPosShow, this.smilePanelTransform.localPosition.z);
				this.scrollMessages.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (!this.isShowSmilePanel && this.smilePanelTransform.localPosition.y > this.stickerPosHide)
		{
			this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.smilePanelTransform.localPosition.y - Time.deltaTime * this.speedHideOrShowStiker, this.smilePanelTransform.localPosition.z);
			this.scrollMessages.MoveRelative(Vector3.down * Time.deltaTime * this.speedHideOrShowStiker);
			if (this.smilePanelTransform.localPosition.y < this.stickerPosHide)
			{
				this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.stickerPosHide, this.smilePanelTransform.localPosition.z);
				this.scrollMessages.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		bool flag = string.IsNullOrEmpty(this.sendMessageInput.value);
		if (this.sendMessageButton.isEnabled == flag)
		{
			this.sendMessageButton.isEnabled = !flag;
		}
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x000DF290 File Offset: 0x000DD490
	public void CancelSendPrivateMessage()
	{
		this.sendMessageInput.value = string.Empty;
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000DF2A4 File Offset: 0x000DD4A4
	public void OnKeyboardVisible()
	{
		if (this.isKeyboardVisible)
		{
			return;
		}
		this.isKeyboardVisible = true;
		this.keyboardSize = this.sendMessageInput.heightKeyboard;
		if (Application.isEditor)
		{
			this.keyboardSize = 200f;
		}
		this.bottomAnchor.localPosition = new Vector3(this.bottomAnchor.localPosition.x, this.bottomAnchor.localPosition.y + this.keyboardSize / Defs.Coef, this.bottomAnchor.localPosition.z);
		this.sendMessageButton.gameObject.SetActive(true);
		this.smilesBtnContainer.localPosition = new Vector3(-1f * this.smilesBtnContainer.localPosition.x, this.smilesBtnContainer.localPosition.y, this.smilesBtnContainer.localPosition.z);
		this.leftInputAnchor.localPosition = new Vector3(this.leftInputAnchor.localPosition.x - 162f, this.leftInputAnchor.localPosition.y, this.leftInputAnchor.localPosition.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x000DF404 File Offset: 0x000DD604
	private IEnumerator ResetpositionCoroutine()
	{
		yield return null;
		this.scrollMessages.ResetPosition();
		this.smilePanelTransform.gameObject.SetActive(false);
		this.smilePanelTransform.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x000DF420 File Offset: 0x000DD620
	public void OnKeyboardHide()
	{
		if (!this.isKeyboardVisible)
		{
			return;
		}
		this.isKeyboardVisible = false;
		this.bottomAnchor.localPosition = new Vector3(this.bottomAnchor.localPosition.x, this.bottomAnchor.localPosition.y - this.keyboardSize / Defs.Coef, this.bottomAnchor.localPosition.z);
		this.sendMessageButton.gameObject.SetActive(false);
		this.smilesBtnContainer.localPosition = new Vector3(-1f * this.smilesBtnContainer.localPosition.x, this.smilesBtnContainer.localPosition.y, this.smilesBtnContainer.localPosition.z);
		this.leftInputAnchor.localPosition = new Vector3(this.leftInputAnchor.localPosition.x + 162f, this.leftInputAnchor.localPosition.y, this.leftInputAnchor.localPosition.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
		this.smilePanelTransform.gameObject.SetActive(false);
		this.smilePanelTransform.gameObject.SetActive(true);
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000DF57C File Offset: 0x000DD77C
	public void BuySmileOnClick()
	{
		ButtonClickSound.TryPlayClick();
		this.buySmileBannerPrefab.SetActive(true);
		this.sendMessageInput.DeselectInput();
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x000DF59C File Offset: 0x000DD79C
	public void ShowSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.isShowSmilePanel = true;
		this.showSmileButton.SetActive(false);
		this.hideSmileButton.SetActive(true);
		this.scrollMessages.ResetPosition();
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000DF5F0 File Offset: 0x000DD7F0
	public void HideSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.HideSmilePannel();
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x000DF620 File Offset: 0x000DD820
	public void HideSmilePannel()
	{
		this.isShowSmilePanel = false;
		if (this.isBuySmile)
		{
			this.showSmileButton.SetActive(true);
			this.buySmileButton.SetActive(false);
		}
		else
		{
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(true);
		}
		this.hideSmileButton.SetActive(false);
	}

	// Token: 0x06002A35 RID: 10805 RVA: 0x000DF680 File Offset: 0x000DD880
	public void SendSmile(string smile)
	{
		this.SendPrivateMessageCore(Defs.SmileMessageSuffix + smile);
		this.HideSmilePannel();
	}

	// Token: 0x06002A36 RID: 10806 RVA: 0x000DF69C File Offset: 0x000DD89C
	public void SendMessageFromInput()
	{
		this.SendPrivateMessage();
		if (this.isShowSmilePanel)
		{
			this.HideSmilePannel();
		}
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x000DF6B8 File Offset: 0x000DD8B8
	public void SendPrivateMessage()
	{
		this.SendPrivateMessageCore(string.Empty);
	}

	// Token: 0x06002A38 RID: 10808 RVA: 0x000DF6C8 File Offset: 0x000DD8C8
	public void SendPrivateMessageCore(string customMessage)
	{
		if (string.IsNullOrEmpty(customMessage) && (string.IsNullOrEmpty(this.sendMessageInput.value) || this.sendMessageInput.value.Contains(Defs.SmileMessageSuffix)))
		{
			return;
		}
		bool flag = !string.IsNullOrEmpty(customMessage);
		ChatController.PrivateMessage item = new ChatController.PrivateMessage(FriendsController.sharedController.id, (!flag) ? FilterBadWorld.FilterString(this.sendMessageInput.value) : customMessage, (double)(Tools.CurrentUnixTime + 10000000L), false, true);
		if (!ChatController.privateMessagesForSend.ContainsKey(this.selectedPlayerID))
		{
			ChatController.privateMessagesForSend.Add(this.selectedPlayerID, new List<ChatController.PrivateMessage>());
		}
		ChatController.privateMessagesForSend[this.selectedPlayerID].Add(item);
		ChatController.SavePrivatMessageInPrefs();
		if (!flag)
		{
			this.sendMessageInput.value = string.Empty;
		}
		this.UpdateMessageForSelectedUsers(true);
		FriendsController.sharedController.GetFriendsData(false);
		if (this.selectedPlayerItem.myWrapIndex != 0)
		{
			this._friends = new List<string>(this.friendsWithInfo);
			this._friends.Sort(new Comparison<string>(this.SortByMessagesCount));
			this.friendsWrap.SortAlphabetically();
			this.scrollFriends.ResetPosition();
		}
	}

	// Token: 0x04001F18 RID: 7960
	public static PrivateChatController sharedController;

	// Token: 0x04001F19 RID: 7961
	public string selectedPlayerID = string.Empty;

	// Token: 0x04001F1A RID: 7962
	public FriendPrevInChatItem selectedPlayerItem;

	// Token: 0x04001F1B RID: 7963
	public GameObject friendPreviewPrefab;

	// Token: 0x04001F1C RID: 7964
	public GameObject messagePrefab;

	// Token: 0x04001F1D RID: 7965
	public MyUIInput sendMessageInput;

	// Token: 0x04001F1E RID: 7966
	public UIButton sendMessageButton;

	// Token: 0x04001F1F RID: 7967
	public UIWrapContent friendsWrap;

	// Token: 0x04001F20 RID: 7968
	public UITable messageTable;

	// Token: 0x04001F21 RID: 7969
	public UIScrollView scrollMessages;

	// Token: 0x04001F22 RID: 7970
	public UIScrollView scrollFriends;

	// Token: 0x04001F23 RID: 7971
	public Transform smilesBtnContainer;

	// Token: 0x04001F24 RID: 7972
	public Transform leftInputAnchor;

	// Token: 0x04001F25 RID: 7973
	private UIPanel scrollFriensPanel;

	// Token: 0x04001F26 RID: 7974
	private UIPanel scrollPanel;

	// Token: 0x04001F27 RID: 7975
	private Transform scrollFriendsTransform;

	// Token: 0x04001F28 RID: 7976
	private Transform scrollTransform;

	// Token: 0x04001F29 RID: 7977
	private float heightMessage = 134f;

	// Token: 0x04001F2A RID: 7978
	private float heightFriends = 100f;

	// Token: 0x04001F2B RID: 7979
	private float stickerPosShow = -150f;

	// Token: 0x04001F2C RID: 7980
	private float stickerPosHide = -314f;

	// Token: 0x04001F2D RID: 7981
	private float speedHideOrShowStiker = 500f;

	// Token: 0x04001F2E RID: 7982
	public bool isShowSmilePanel;

	// Token: 0x04001F2F RID: 7983
	public Transform smilePanelTransform;

	// Token: 0x04001F30 RID: 7984
	public GameObject showSmileButton;

	// Token: 0x04001F31 RID: 7985
	public GameObject hideSmileButton;

	// Token: 0x04001F32 RID: 7986
	public GameObject buySmileButton;

	// Token: 0x04001F33 RID: 7987
	public bool isBuySmile;

	// Token: 0x04001F34 RID: 7988
	public GameObject buySmileBannerPrefab;

	// Token: 0x04001F35 RID: 7989
	private bool wrapsInit;

	// Token: 0x04001F36 RID: 7990
	private List<string> _friends = new List<string>();

	// Token: 0x04001F37 RID: 7991
	private List<string> friendsWithInfo = new List<string>();

	// Token: 0x04001F38 RID: 7992
	private List<ChatController.PrivateMessage> curListMessages = new List<ChatController.PrivateMessage>();

	// Token: 0x04001F39 RID: 7993
	private float keyboardSize;

	// Token: 0x04001F3A RID: 7994
	public Transform bottomAnchor;

	// Token: 0x04001F3B RID: 7995
	public UIPanel panelSmiles;

	// Token: 0x04001F3C RID: 7996
	private List<PrivateMessageItem> privateMessageItems = new List<PrivateMessageItem>();

	// Token: 0x04001F3D RID: 7997
	private bool isKeyboardVisible;
}
