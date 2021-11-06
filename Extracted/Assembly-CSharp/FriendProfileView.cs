using System;
using System.Collections;
using System.Reflection;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x02000615 RID: 1557
internal sealed class FriendProfileView : MonoBehaviour
{
	// Token: 0x14000053 RID: 83
	// (add) Token: 0x0600353D RID: 13629 RVA: 0x00113D88 File Offset: 0x00111F88
	// (remove) Token: 0x0600353E RID: 13630 RVA: 0x00113DA4 File Offset: 0x00111FA4
	public event Action BackButtonClickEvent;

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x0600353F RID: 13631 RVA: 0x00113DC0 File Offset: 0x00111FC0
	// (remove) Token: 0x06003540 RID: 13632 RVA: 0x00113DDC File Offset: 0x00111FDC
	public event Action JoinButtonClickEvent;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06003541 RID: 13633 RVA: 0x00113DF8 File Offset: 0x00111FF8
	// (remove) Token: 0x06003542 RID: 13634 RVA: 0x00113E14 File Offset: 0x00112014
	public event Action CopyMyIdButtonClickEvent;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06003543 RID: 13635 RVA: 0x00113E30 File Offset: 0x00112030
	// (remove) Token: 0x06003544 RID: 13636 RVA: 0x00113E4C File Offset: 0x0011204C
	public event Action ChatButtonClickEvent;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06003545 RID: 13637 RVA: 0x00113E68 File Offset: 0x00112068
	// (remove) Token: 0x06003546 RID: 13638 RVA: 0x00113E84 File Offset: 0x00112084
	public event Action AddButtonClickEvent;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06003547 RID: 13639 RVA: 0x00113EA0 File Offset: 0x001120A0
	// (remove) Token: 0x06003548 RID: 13640 RVA: 0x00113EBC File Offset: 0x001120BC
	public event Action RemoveButtonClickEvent;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06003549 RID: 13641 RVA: 0x00113ED8 File Offset: 0x001120D8
	// (remove) Token: 0x0600354A RID: 13642 RVA: 0x00113EF4 File Offset: 0x001120F4
	public event Action InviteToClanButtonClickEvent;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x0600354B RID: 13643 RVA: 0x00113F10 File Offset: 0x00112110
	// (remove) Token: 0x0600354C RID: 13644 RVA: 0x00113F2C File Offset: 0x0011212C
	public event Action UpdateRequested;

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x0600354D RID: 13645 RVA: 0x00113F48 File Offset: 0x00112148
	public GameObject characterModel
	{
		get
		{
			return this.characterInterface.gameObject;
		}
	}

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x0600354E RID: 13646 RVA: 0x00113F58 File Offset: 0x00112158
	// (set) Token: 0x0600354F RID: 13647 RVA: 0x00113F60 File Offset: 0x00112160
	public bool IsCanConnectToFriend { get; set; }

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x06003550 RID: 13648 RVA: 0x00113F6C File Offset: 0x0011216C
	// (set) Token: 0x06003551 RID: 13649 RVA: 0x00113F74 File Offset: 0x00112174
	public string FriendLocation { get; set; }

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x06003552 RID: 13650 RVA: 0x00113F80 File Offset: 0x00112180
	// (set) Token: 0x06003553 RID: 13651 RVA: 0x00113F88 File Offset: 0x00112188
	public int FriendCount { get; set; }

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x06003554 RID: 13652 RVA: 0x00113F94 File Offset: 0x00112194
	// (set) Token: 0x06003555 RID: 13653 RVA: 0x00113F9C File Offset: 0x0011219C
	public string FriendName { get; set; }

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x06003556 RID: 13654 RVA: 0x00113FA8 File Offset: 0x001121A8
	// (set) Token: 0x06003557 RID: 13655 RVA: 0x00113FB0 File Offset: 0x001121B0
	public OnlineState Online { get; set; }

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x06003558 RID: 13656 RVA: 0x00113FBC File Offset: 0x001121BC
	// (set) Token: 0x06003559 RID: 13657 RVA: 0x00113FC4 File Offset: 0x001121C4
	public int Rank { get; set; }

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x0600355A RID: 13658 RVA: 0x00113FD0 File Offset: 0x001121D0
	// (set) Token: 0x0600355B RID: 13659 RVA: 0x00113FD8 File Offset: 0x001121D8
	public int SurvivalScore { get; set; }

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x0600355C RID: 13660 RVA: 0x00113FE4 File Offset: 0x001121E4
	// (set) Token: 0x0600355D RID: 13661 RVA: 0x00113FEC File Offset: 0x001121EC
	public string Username { get; set; }

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x0600355E RID: 13662 RVA: 0x00113FF8 File Offset: 0x001121F8
	// (set) Token: 0x0600355F RID: 13663 RVA: 0x00114000 File Offset: 0x00112200
	public int WinCount { get; set; }

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x06003560 RID: 13664 RVA: 0x0011400C File Offset: 0x0011220C
	// (set) Token: 0x06003561 RID: 13665 RVA: 0x00114014 File Offset: 0x00112214
	public int TotalWinCount { get; set; }

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x06003562 RID: 13666 RVA: 0x00114020 File Offset: 0x00112220
	// (set) Token: 0x06003563 RID: 13667 RVA: 0x00114028 File Offset: 0x00112228
	public string FriendGameMode { get; set; }

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x06003564 RID: 13668 RVA: 0x00114034 File Offset: 0x00112234
	// (set) Token: 0x06003565 RID: 13669 RVA: 0x0011403C File Offset: 0x0011223C
	public string FriendId { get; set; }

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x06003566 RID: 13670 RVA: 0x00114048 File Offset: 0x00112248
	// (set) Token: 0x06003567 RID: 13671 RVA: 0x00114050 File Offset: 0x00112250
	public string NotConnectCondition { get; set; }

	// Token: 0x06003568 RID: 13672 RVA: 0x0011405C File Offset: 0x0011225C
	public void Reset()
	{
		this.IsCanConnectToFriend = false;
		this.FriendLocation = string.Empty;
		this.FriendCount = 0;
		this.FriendName = string.Empty;
		this.Online = ((!FriendsController.IsPlayerOurFriend(this.FriendId)) ? OnlineState.none : OnlineState.offline);
		this.Rank = 0;
		this.SurvivalScore = 0;
		this.Username = string.Empty;
		this.WinCount = 0;
		if (this.characterModel != null)
		{
			Texture texture = Resources.Load<Texture>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1"));
			if (texture != null)
			{
				this.characterInterface.SetSkin(texture, null, null);
			}
		}
		this.SetOnlineState(this.Online);
		this.characterInterface.RemoveBoots();
		this.characterInterface.RemoveHat();
		this.characterInterface.RemoveMask();
		this.characterInterface.RemoveCape();
		this.characterInterface.RemoveArmor();
		this.SetEnableAddButton(true);
		this.SetEnableInviteClanButton(true);
	}

	// Token: 0x06003569 RID: 13673 RVA: 0x0011415C File Offset: 0x0011235C
	public void SetBoots(string name)
	{
		this.characterInterface.UpdateBoots(name, false);
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x0011416C File Offset: 0x0011236C
	private void SetOnlineState(OnlineState onlineState)
	{
		bool isStateOffline = onlineState == OnlineState.offline;
		bool isStateInFriends = onlineState == OnlineState.inFriends;
		bool isStatePlaying = onlineState == OnlineState.playing;
		this.offlineStateLabel.Do(delegate(UILabel l)
		{
			l.gameObject.SetActive(isStateOffline);
		});
		this.inFriendStateLabel.Do(delegate(UILabel l)
		{
			l.gameObject.SetActive(isStateInFriends);
		});
		this.playingStateLabel.Do(delegate(UILabel l)
		{
			l.gameObject.SetActive(isStatePlaying);
		});
		this.offlineState.Do(delegate(UISprite l)
		{
			l.gameObject.SetActive(isStateOffline);
		});
		this.inFriendState.Do(delegate(UISprite l)
		{
			l.gameObject.SetActive(isStateInFriends);
		});
		this.playingState.Do(delegate(UISprite l)
		{
			l.gameObject.SetActive(isStatePlaying);
		});
		if (this.playingStateInfoContainer != null)
		{
			this.playingStateInfoContainer.SetActive(isStatePlaying);
		}
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x00114250 File Offset: 0x00112450
	public void SetStockCape(string capeName)
	{
		if (string.IsNullOrEmpty(capeName))
		{
			Debug.LogWarning("Name of cape should not be empty.");
			return;
		}
		this.characterInterface.UpdateCape(capeName, null, false);
	}

	// Token: 0x0600356C RID: 13676 RVA: 0x00114284 File Offset: 0x00112484
	public void SetCustomCape(byte[] capeBytes)
	{
		capeBytes = (capeBytes ?? new byte[0]);
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(capeBytes);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.characterInterface.UpdateCape("cape_Custom", texture2D, false);
	}

	// Token: 0x0600356D RID: 13677 RVA: 0x001142D4 File Offset: 0x001124D4
	public void SetArmor(string armorName)
	{
		if (string.IsNullOrEmpty(armorName))
		{
			Debug.LogWarning("Name of armor should not be empty.");
			return;
		}
		this.characterInterface.UpdateArmor(armorName);
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x00114304 File Offset: 0x00112504
	public void SetHat(string hatName)
	{
		if (string.IsNullOrEmpty(hatName))
		{
			Debug.LogWarning("Name of hat should not be empty.");
			return;
		}
		this.characterInterface.UpdateHat(hatName, false);
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x0011432C File Offset: 0x0011252C
	public void SetMask(string maskName)
	{
		if (string.IsNullOrEmpty(maskName))
		{
			Debug.LogWarning("Name of mask should not be empty.");
			return;
		}
		this.characterInterface.UpdateMask(maskName, false);
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x00114354 File Offset: 0x00112554
	public void SetSkin(byte[] skinBytes)
	{
		skinBytes = (skinBytes ?? new byte[0]);
		if (this.characterModel != null)
		{
			Func<byte[], Texture2D> func = delegate(byte[] bytes)
			{
				Texture2D texture2D = new Texture2D(64, 32)
				{
					filterMode = FilterMode.Point
				};
				texture2D.LoadImage(bytes);
				texture2D.Apply();
				return texture2D;
			};
			Texture2D skin = (skinBytes.Length <= 0) ? Resources.Load<Texture2D>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1")) : func(skinBytes);
			this.characterInterface.SetSkin(skin, null, null);
		}
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x001143D8 File Offset: 0x001125D8
	private void Awake()
	{
		GameObject original = Resources.Load("Character_model") as GameObject;
		this.characterInterface = UnityEngine.Object.Instantiate<GameObject>(original).GetComponent<CharacterInterface>();
		this.characterInterface.GetComponent<CharacterInterface>().usePetFromStorager = false;
		this.characterInterface.transform.SetParent(this.pers, false);
		this.characterInterface.SetCharacterType(true, true, false);
		Player_move_c.SetLayerRecursively(this.characterInterface.gameObject, this.pers.gameObject.layer);
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		this.Reset();
		if (this.backButton != null)
		{
			EventDelegate.Add(this.backButton.onClick, new EventDelegate.Callback(this.OnBackButtonClick));
		}
		if (this.joinButton != null)
		{
			EventDelegate.Add(this.joinButton.onClick, new EventDelegate.Callback(this.OnJoinButtonClick));
		}
		if (this.sendMyIdButton != null)
		{
			EventDelegate.Add(this.sendMyIdButton.onClick, new EventDelegate.Callback(this.OnSendMyIdButtonClick));
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				this.sendMyIdButton.gameObject.SetActive(false);
			}
		}
		if (this.chatButton != null)
		{
			EventDelegate.Add(this.chatButton.onClick, new EventDelegate.Callback(this.OnChatButtonClick));
		}
		if (this.addFriendButton != null)
		{
			EventDelegate.Add(this.addFriendButton.onClick, new EventDelegate.Callback(this.OnAddButtonClick));
		}
		if (this.removeFriendButton != null)
		{
			EventDelegate.Add(this.removeFriendButton.onClick, new EventDelegate.Callback(this.OnRemoveButtonClick));
		}
		if (this.inviteToClanButton != null)
		{
			EventDelegate.Add(this.inviteToClanButton.onClick, new EventDelegate.Callback(this.OnInviteToClanButtonClick));
		}
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x001145D0 File Offset: 0x001127D0
	private void OnDisable()
	{
		base.StopCoroutine("RequestUpdate");
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x00114608 File Offset: 0x00112808
	private void OnEnable()
	{
		base.StartCoroutine("RequestUpdate");
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Friend Profile");
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x00114664 File Offset: 0x00112864
	private void HandleEscape()
	{
		if (!InfoWindowController.IsActive)
		{
			this._escapePressed = true;
		}
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x00114678 File Offset: 0x00112878
	private void OnBackButtonClick()
	{
		if (this.BackButtonClickEvent != null)
		{
			this.BackButtonClickEvent();
		}
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x00114690 File Offset: 0x00112890
	private void OnJoinButtonClick()
	{
		if (this.JoinButtonClickEvent != null)
		{
			this.JoinButtonClickEvent();
		}
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x001146A8 File Offset: 0x001128A8
	private void OnSendMyIdButtonClick()
	{
		if (this.CopyMyIdButtonClickEvent != null)
		{
			this.CopyMyIdButtonClickEvent();
		}
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x001146C0 File Offset: 0x001128C0
	private void OnChatButtonClick()
	{
		if (this.ChatButtonClickEvent != null)
		{
			this.ChatButtonClickEvent();
		}
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x001146D8 File Offset: 0x001128D8
	private void OnAddButtonClick()
	{
		if (this.AddButtonClickEvent != null)
		{
			this.AddButtonClickEvent();
		}
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x001146F0 File Offset: 0x001128F0
	private void OnRemoveButtonClick()
	{
		if (this.RemoveButtonClickEvent != null)
		{
			this.RemoveButtonClickEvent();
		}
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x00114708 File Offset: 0x00112908
	private void OnInviteToClanButtonClick()
	{
		if (this.InviteToClanButtonClickEvent != null)
		{
			this.InviteToClanButtonClickEvent();
		}
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x00114720 File Offset: 0x00112920
	[Obfuscation(Exclude = true)]
	private IEnumerator RequestUpdate()
	{
		for (;;)
		{
			if (this.UpdateRequested != null)
			{
				this.UpdateRequested();
			}
			yield return new WaitForSeconds(5f);
		}
		yield break;
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x0011473C File Offset: 0x0011293C
	private void Update()
	{
		if (this._escapePressed)
		{
			this._escapePressed = false;
			this.OnBackButtonClick();
			return;
		}
		this.UpdateLightweight();
		float rotationRateForCharacterInMenues = RilisoftRotator.RotationRateForCharacterInMenues;
		Rect value = this._touchZone.Value;
		RilisoftRotator.RotateCharacter(this.pers, rotationRateForCharacterInMenues, value, ref this.idleTimerLastTime, ref this.lastTime, null);
		if (Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			this.ReturnPersTonNormState();
		}
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x001147B0 File Offset: 0x001129B0
	private void ReturnPersTonNormState()
	{
		HOTween.Kill(this.pers);
		Vector3 p_endVal = new Vector3(0f, -180f, 0f);
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(this.pers, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(p_endVal)).Ease(EaseType.Linear).OnComplete(delegate()
		{
			this.idleTimerLastTime = Time.realtimeSinceStartup;
		}));
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x00114828 File Offset: 0x00112A28
	private void UpdateLightweight()
	{
		if (this.friendLocationLabel != null)
		{
			this.friendLocationLabel.text = (this.FriendLocation ?? string.Empty);
		}
		if (this.friendCountLabel != null)
		{
			this.friendCountLabel.text = ((this.FriendCount >= 0) ? this.FriendCount.ToString() : "-");
		}
		if (this.friendNameLabel != null)
		{
			this.friendNameLabel.text = (this.FriendName ?? string.Empty);
		}
		this.SetOnlineState(this.Online);
		this.notConnectJoinButtonSprite.alpha = ((!this.IsCanConnectToFriend) ? 1f : 0f);
		if (this.rankSprite != null)
		{
			string text = "Rank_" + this.Rank;
			if (!this.rankSprite.spriteName.Equals(text))
			{
				this.rankSprite.spriteName = text;
			}
		}
		if (this.survivalScoreLabel != null)
		{
			this.survivalScoreLabel.text = ((this.SurvivalScore >= 0) ? this.SurvivalScore.ToString() : "-");
		}
		if (this.winCountLabel != null)
		{
			this.winCountLabel.text = ((this.WinCount >= 0) ? this.WinCount.ToString() : "-");
		}
		if (this.totalWinCountLabel != null)
		{
			this.totalWinCountLabel.text = ((this.TotalWinCount >= 0) ? this.TotalWinCount.ToString() : "-");
		}
		if (this.friendGameModeLabel != null)
		{
			this.friendGameModeLabel.text = this.FriendGameMode;
		}
		if (this.friendIdLabel != null)
		{
			this.friendIdLabel.text = this.FriendId;
		}
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x00114A50 File Offset: 0x00112C50
	public void SetTitle(string titleText)
	{
		for (int i = 0; i < this.titlesLabel.Length; i++)
		{
			this.titlesLabel[i].text = titleText;
		}
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x00114A84 File Offset: 0x00112C84
	private void SetActiveAndRepositionButtons(GameObject button, bool isActive)
	{
		bool activeSelf = button.activeSelf;
		button.SetActive(isActive);
		if (activeSelf != isActive)
		{
			this.buttonAlignContainer.Reposition();
			this.buttonAlignContainer.repositionNow = true;
		}
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x00114AC0 File Offset: 0x00112CC0
	public void SetActiveChatButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.chatButton.gameObject, isActive);
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x00114AD4 File Offset: 0x00112CD4
	public void SetActiveInviteButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.inviteToClanButton.gameObject, isActive);
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x00114AE8 File Offset: 0x00112CE8
	public void SetActiveAddButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.addFriendButton.gameObject, isActive);
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x00114AFC File Offset: 0x00112CFC
	public void SetActiveAddButtonSent(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.addFrienButtonSentState.gameObject, isActive);
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x00114B10 File Offset: 0x00112D10
	public void SetActiveAddClanButtonSent(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.addClanButtonSentState.gameObject, isActive);
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x00114B24 File Offset: 0x00112D24
	public void SetActiveRemoveButton(bool isActive)
	{
		this.SetActiveAndRepositionButtons(this.removeFriendButton.gameObject, isActive);
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x00114B38 File Offset: 0x00112D38
	public void SetEnableAddButton(bool enable)
	{
		if (this.addFriendButton != null)
		{
			this.addFriendButton.isEnabled = enable;
		}
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x00114B58 File Offset: 0x00112D58
	public void SetEnableRemoveButton(bool enable)
	{
		if (this.removeFriendButton != null)
		{
			this.removeFriendButton.isEnabled = enable;
		}
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x00114B78 File Offset: 0x00112D78
	public void SetEnableInviteClanButton(bool enable)
	{
		if (this.inviteToClanButton != null)
		{
			this.inviteToClanButton.isEnabled = enable;
		}
	}

	// Token: 0x04002718 RID: 10008
	private const string DefaultStatisticString = "-";

	// Token: 0x04002719 RID: 10009
	public Transform pers;

	// Token: 0x0400271A RID: 10010
	public GameObject[] bootsPoint;

	// Token: 0x0400271B RID: 10011
	private CharacterInterface characterInterface;

	// Token: 0x0400271C RID: 10012
	public UISprite rankSprite;

	// Token: 0x0400271D RID: 10013
	public UILabel friendCountLabel;

	// Token: 0x0400271E RID: 10014
	public UILabel friendLocationLabel;

	// Token: 0x0400271F RID: 10015
	public UILabel friendGameModeLabel;

	// Token: 0x04002720 RID: 10016
	public UILabel friendNameLabel;

	// Token: 0x04002721 RID: 10017
	public UILabel survivalScoreLabel;

	// Token: 0x04002722 RID: 10018
	public UILabel winCountLabel;

	// Token: 0x04002723 RID: 10019
	public UILabel totalWinCountLabel;

	// Token: 0x04002724 RID: 10020
	public UILabel clanName;

	// Token: 0x04002725 RID: 10021
	public UILabel friendIdLabel;

	// Token: 0x04002726 RID: 10022
	public UILabel[] titlesLabel;

	// Token: 0x04002727 RID: 10023
	public UITexture clanLogo;

	// Token: 0x04002728 RID: 10024
	[Header("Online state settings")]
	public UILabel inFriendStateLabel;

	// Token: 0x04002729 RID: 10025
	[Header("Online state settings")]
	public UILabel offlineStateLabel;

	// Token: 0x0400272A RID: 10026
	[Header("Online state settings")]
	public UILabel playingStateLabel;

	// Token: 0x0400272B RID: 10027
	public UISprite inFriendState;

	// Token: 0x0400272C RID: 10028
	public UISprite offlineState;

	// Token: 0x0400272D RID: 10029
	public UISprite playingState;

	// Token: 0x0400272E RID: 10030
	public GameObject playingStateInfoContainer;

	// Token: 0x0400272F RID: 10031
	[Header("Buttons settings")]
	public UIButton backButton;

	// Token: 0x04002730 RID: 10032
	public UIButton joinButton;

	// Token: 0x04002731 RID: 10033
	public UIButton sendMyIdButton;

	// Token: 0x04002732 RID: 10034
	public UIButton chatButton;

	// Token: 0x04002733 RID: 10035
	public UIButton inviteToClanButton;

	// Token: 0x04002734 RID: 10036
	public UIButton addFriendButton;

	// Token: 0x04002735 RID: 10037
	public UIButton removeFriendButton;

	// Token: 0x04002736 RID: 10038
	public UITable buttonAlignContainer;

	// Token: 0x04002737 RID: 10039
	public UILabel addOrRemoveButtonLabel;

	// Token: 0x04002738 RID: 10040
	public UISprite notConnectJoinButtonSprite;

	// Token: 0x04002739 RID: 10041
	public UISprite addFrienButtonSentState;

	// Token: 0x0400273A RID: 10042
	public UISprite addClanButtonSentState;

	// Token: 0x0400273B RID: 10043
	private IDisposable _backSubscription;

	// Token: 0x0400273C RID: 10044
	private bool _escapePressed;

	// Token: 0x0400273D RID: 10045
	private float lastTime;

	// Token: 0x0400273E RID: 10046
	private float idleTimerLastTime;

	// Token: 0x0400273F RID: 10047
	private readonly Lazy<Rect> _touchZone = new Lazy<Rect>(() => new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height));
}
