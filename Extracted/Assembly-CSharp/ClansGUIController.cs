using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prime31;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200006B RID: 107
public sealed class ClansGUIController : MonoBehaviour, IFriendsGUIController
{
	// Token: 0x060002F4 RID: 756 RVA: 0x000195A4 File Offset: 0x000177A4
	public ClansGUIController()
	{
		this._clanIncomingInvitesController = new Lazy<ClanIncomingInvitesController>(() => base.gameObject.GetComponent<ClanIncomingInvitesController>());
		this._newMessagesOverlays = new Lazy<UISprite[]>(delegate()
		{
			UISprite[] first = this.clanPanel.Map((GameObject c) => c.GetComponentsInChildren<UISprite>(true), new UISprite[0]);
			UISprite[] second = this.NoClanPanel.Map((GameObject c) => c.GetComponentsInChildren<UISprite>(true), new UISprite[0]);
			IEnumerable<UISprite> source = from s in first.Concat(second)
			where "NewMessages".Equals(s.name)
			select s;
			return source.ToArray<UISprite>();
		});
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x000195F4 File Offset: 0x000177F4
	void IFriendsGUIController.Hide(bool h)
	{
		this.topLevelObject.SetActive(!h);
		ClansGUIController.ShowProfile = h;
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060002F7 RID: 759 RVA: 0x0001960C File Offset: 0x0001780C
	// (set) Token: 0x060002F8 RID: 760 RVA: 0x00019614 File Offset: 0x00017814
	internal ClansGUIController.State CurrentState { get; set; }

	// Token: 0x060002F9 RID: 761 RVA: 0x00019620 File Offset: 0x00017820
	public void HideRewardWindow()
	{
		this.rewardCreateClanWindow.SetActive(false);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00019630 File Offset: 0x00017830
	private IEnumerator SetName(string nm)
	{
		yield return null;
		this.inputNameClanLabel.text = nm;
		this.inputNameClanLabel.parent.GetComponent<UIInput>().value = nm;
		yield break;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0001965C File Offset: 0x0001785C
	private void OnChangeClanName(string newName)
	{
		if (this.nameClanLabel.text.Equals(newName))
		{
			return;
		}
		if (this.changeClanNameInput.isSelected)
		{
			return;
		}
		this.nameClanLabel.text = newName;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x000196A0 File Offset: 0x000178A0
	private void Start()
	{
		ClansGUIController.sharedController = this;
		RewardWindowBase component = this.rewardCreateClanWindow.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority priority = FacebookController.StoryPriority.Green;
		component.shareAction = delegate()
		{
			FacebookController.PostOpenGraphStory("create", "clan", priority, new Dictionary<string, string>
			{
				{
					"mode",
					"create"
				}
			});
		};
		component.priority = priority;
		component.twitterStatus = (() => "I’ve created a CLAN in @PixelGun3D! Join my team and get ready to fight! #pixelgun3d #pixelgun #pg3d #mobile #fps http://goo.gl/8fzL9u");
		component.EventTitle = "Created Clan";
		component.HasReward = false;
		UIInputRilisoft uiinputRilisoft = (!(this.ClanName != null)) ? null : this.ClanName.GetComponent<UIInputRilisoft>();
		if (uiinputRilisoft != null)
		{
			uiinputRilisoft.value = LocalizationStore.Key_0589;
			UIInputRilisoft uiinputRilisoft2 = uiinputRilisoft;
			uiinputRilisoft2.onFocus = (UIInputRilisoft.OnFocus)Delegate.Combine(uiinputRilisoft2.onFocus, new UIInputRilisoft.OnFocus(this.OnFocusCreateClanName));
			UIInputRilisoft uiinputRilisoft3 = uiinputRilisoft;
			uiinputRilisoft3.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Combine(uiinputRilisoft3.onFocusLost, new UIInputRilisoft.OnFocusLost(this.onFocusLostCreateClanName));
		}
		this._friendProfileController = new FriendProfileController(this, true);
		this.InClan = !string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
		FriendsController friendsController = FriendsController.sharedController;
		friendsController.onChangeClanName = (FriendsController.OnChangeClanName)Delegate.Combine(friendsController.onChangeClanName, new FriendsController.OnChangeClanName(this.OnChangeClanName));
		if (this.InClan && !string.IsNullOrEmpty(FriendsController.sharedController.clanName))
		{
			this.nameClanLabel.text = FriendsController.sharedController.clanName;
			this.changeClanNameInput.value = this.nameClanLabel.text;
		}
		ClansGUIController.AtAddPanel = false;
		ClansGUIController.AtStatsPanel = false;
		this.timeOfLastSort = Time.realtimeSinceStartup;
		FriendsController.sharedController.StartRefreshingClanOnline();
		this.startPanel.SetActive(!FriendsController.readyToOperate);
		this.NoClanPanel.SetActive(FriendsController.readyToOperate && !this.InClan);
		this.clanPanel.SetActive(FriendsController.readyToOperate && this.InClan);
		if (GlobalGameController.Logos == null)
		{
			Texture2D[] array = Resources.LoadAll<Texture2D>("Clan_Previews/");
			if (array == null)
			{
				array = new Texture2D[0];
			}
			this._logos.AddRange(array);
			StringComparer nameComparer = StringComparer.OrdinalIgnoreCase;
			this._logos.Sort((Texture2D a, Texture2D b) => nameComparer.Compare(a.name, b.name));
			this._currentLogoInd = 0;
		}
		else if (this.InClan)
		{
			if (GlobalGameController.LogoToEdit != null)
			{
				byte[] inArray = GlobalGameController.LogoToEdit.EncodeToPNG();
				FriendsController.sharedController.clanLogo = Convert.ToBase64String(inArray);
				FriendsController.sharedController.ChangeClanLogo();
			}
		}
		else
		{
			this.CreateClanPanel.SetActive(FriendsController.readyToOperate);
			this._logos = GlobalGameController.Logos;
			base.StartCoroutine(this.SetName(GlobalGameController.TempClanName));
			if (GlobalGameController.LogoToEdit != null)
			{
				this._logos.Add(GlobalGameController.LogoToEdit);
				this._currentLogoInd = this._logos.Count - 1;
			}
			else
			{
				this._currentLogoInd = 0;
			}
		}
		if (this.InClan && !string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
		{
			try
			{
				byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D.LoadImage(data);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				Texture mainTexture = this.previewLogo.mainTexture;
				this.previewLogo.mainTexture = texture2D;
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.previewLogo.mainTexture);
			}
			catch
			{
			}
		}
		GlobalGameController.Logos = null;
		GlobalGameController.LogoToEdit = null;
		GlobalGameController.TempClanName = null;
		if (this._logos.Count > this._currentLogoInd)
		{
			this.logo.mainTexture = this._logos[this._currentLogoInd];
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.logo.mainTexture);
		}
		if (this.CreateClanButton != null)
		{
			ButtonHandler component2 = this.CreateClanButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleCreateClanClicked;
			}
		}
		if (this.EditLogoBut != null)
		{
			ButtonHandler component3 = this.EditLogoBut.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.HandleEditClicked;
			}
		}
		if (this.BackBut != null)
		{
			ButtonHandler component4 = this.BackBut.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += this.HandleBackClicked;
			}
		}
		if (this.Left != null)
		{
			ButtonHandler component5 = this.Left.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += this.HandleArrowClicked;
			}
		}
		if (this.Right != null)
		{
			ButtonHandler component6 = this.Right.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += this.HandleArrowClicked;
			}
		}
		if (this.addMembersButton != null)
		{
			ButtonHandler component7 = this.addMembersButton.GetComponent<ButtonHandler>();
			if (component7 != null)
			{
				component7.Clicked += this.HandleAddMembersClicked;
			}
		}
		if (this.deleteClanButton != null)
		{
			ButtonHandler component8 = this.deleteClanButton.GetComponent<ButtonHandler>();
			if (component8 != null)
			{
				component8.Clicked += this.HandleDeleteClanClicked;
			}
		}
		if (this.leaveButton != null)
		{
			ButtonHandler component9 = this.leaveButton.GetComponent<ButtonHandler>();
			if (component9 != null)
			{
				component9.Clicked += this.HandleLeaveClicked;
			}
		}
		if (this.editLogoInPreviewButton != null)
		{
			ButtonHandler component10 = this.editLogoInPreviewButton.GetComponent<ButtonHandler>();
			if (component10 != null)
			{
				component10.Clicked += this.HandleEditLogoInPreviewClicked;
			}
		}
		if (this.statisticsButton != null)
		{
			ButtonHandler component11 = this.statisticsButton.GetComponent<ButtonHandler>();
			if (component11 != null)
			{
				component11.Clicked += this.HandleStatisticsButtonClicked;
			}
		}
		if (this.yesDelteClan != null)
		{
			ButtonHandler component12 = this.yesDelteClan.GetComponent<ButtonHandler>();
			if (component12 != null)
			{
				component12.Clicked += this.HandleYesDelClanClicked;
			}
		}
		if (this.noDeleteClan != null)
		{
			ButtonHandler component13 = this.noDeleteClan.GetComponent<ButtonHandler>();
			if (component13 != null)
			{
				component13.Clicked += this.HandleNoDelClanClicked;
			}
		}
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00019DB4 File Offset: 0x00017FB4
	private void OnEnable()
	{
		FriendsController.ClanUpdated += this.UpdateGUI;
		this.UpdateGUI();
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			this._isCancellationRequested = true;
		}, "Clans");
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00019E10 File Offset: 0x00018010
	private void OnDisable()
	{
		FriendsController.ClanUpdated -= this.UpdateGUI;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00019E4C File Offset: 0x0001804C
	public void UpdateGUI()
	{
		base.StartCoroutine(this.__UpdateGUI());
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00019E5C File Offset: 0x0001805C
	public void ChangeClanName()
	{
		string oldText = FriendsController.sharedController.clanName ?? string.Empty;
		if (string.IsNullOrEmpty(this.nameClanLabel.text))
		{
			this.nameClanLabel.text = oldText;
			base.StartCoroutine(this.ShowThisNameInUse());
		}
		else
		{
			FriendsController.sharedController.ChangeClanName(this.nameClanLabel.text, delegate
			{
				FriendsController.sharedController.clanName = this.nameClanLabel.text;
				this.BlockGUI = false;
			}, delegate(string error)
			{
				this.nameClanLabel.text = oldText;
				Debug.Log("error " + error);
				if (!string.IsNullOrEmpty(error))
				{
					if (error.Equals("fail"))
					{
						this.StartCoroutine(this.ShowThisNameInUse());
					}
					else
					{
						this.StartCoroutine(this.ShowCheckConnection());
					}
				}
				else
				{
					this.BlockGUI = false;
				}
			});
		}
		this.BlockGUI = true;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00019F00 File Offset: 0x00018100
	private void _SortFriendPreviews()
	{
		FriendPreview[] componentsInChildren = this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] array = this.friendsGrid.GetComponentsInChildren<FriendPreview>(false);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		StringComparer nameComparer = StringComparer.Ordinal;
		Array.Sort<FriendPreview>(array, (FriendPreview fp1, FriendPreview fp2) => nameComparer.Compare(fp1.name, fp2.name));
		string text = null;
		float num = 0f;
		if (array.Length > 0)
		{
			text = array[0].gameObject.name;
			Transform parent = this.friendsGrid.transform.parent;
			if (parent != null)
			{
				UIPanel component = parent.GetComponent<UIPanel>();
				if (component != null)
				{
					num = array[0].transform.localPosition.x - component.clipOffset.x;
				}
			}
		}
		Array.Sort<FriendPreview>(componentsInChildren, delegate(FriendPreview fp1, FriendPreview fp2)
		{
			if (fp1.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp1.id))
			{
				return 1;
			}
			if (fp2.id == null || !FriendsController.sharedController.onlineInfo.ContainsKey(fp2.id))
			{
				return -1;
			}
			string s = FriendsController.sharedController.onlineInfo[fp1.id]["delta"];
			string s2 = FriendsController.sharedController.onlineInfo[fp1.id]["game_mode"];
			int num3 = int.Parse(s);
			int num4 = int.Parse(s2);
			int num5;
			if ((float)num3 > FriendsController.onlineDelta || (num4 > 99 && num4 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num4 / 100 != 3))
			{
				num5 = 2;
			}
			else if (num4 == -1)
			{
				num5 = 1;
			}
			else
			{
				num5 = 0;
			}
			if (FriendsController.sharedController.clanLeaderID != null && fp1.id.Equals(FriendsController.sharedController.clanLeaderID))
			{
				num5 = -1;
			}
			string s3 = FriendsController.sharedController.onlineInfo[fp2.id]["delta"];
			string s4 = FriendsController.sharedController.onlineInfo[fp2.id]["game_mode"];
			int num6 = int.Parse(s3);
			int num7 = int.Parse(s4);
			int num8;
			if ((float)num6 > FriendsController.onlineDelta || (num7 > 99 && num7 / 100 != (int)ConnectSceneNGUIController.myPlatformConnect && num7 / 100 != 3))
			{
				num8 = 2;
			}
			else if (num7 > -1)
			{
				num8 = 0;
			}
			else
			{
				num8 = 1;
			}
			if (FriendsController.sharedController.clanLeaderID != null && fp2.id.Equals(FriendsController.sharedController.clanLeaderID))
			{
				num8 = -1;
			}
			int num9;
			int num10;
			if (num5 == num8 && int.TryParse(fp1.id, out num9) && int.TryParse(fp2.id, out num10))
			{
				return num9 - num10;
			}
			return num5 - num8;
		});
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.name = i.ToString("D7");
		}
		this.friendsGrid.SortAlphabetically();
		this.friendsGrid.WrapContent();
		Transform transform = null;
		if (text != null)
		{
			foreach (FriendPreview friendPreview in componentsInChildren)
			{
				if (friendPreview.name.Equals(text))
				{
					transform = friendPreview.transform;
					break;
				}
			}
		}
		if (transform == null && componentsInChildren.Length > 0 && this.friendsGrid.gameObject.activeInHierarchy)
		{
			transform = componentsInChildren[0].transform;
		}
		if (transform != null)
		{
			float num2 = transform.localPosition.x - num;
			Transform parent2 = this.friendsGrid.transform.parent;
			if (parent2 != null)
			{
				UIPanel component2 = parent2.GetComponent<UIPanel>();
				if (component2 != null)
				{
					component2.clipOffset = new Vector2(num2, component2.clipOffset.y);
					parent2.localPosition = new Vector3(-num2, parent2.localPosition.y, parent2.localPosition.z);
				}
			}
		}
		this.friendsGrid.WrapContent();
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0001A170 File Offset: 0x00018370
	private IEnumerator __UpdateGUI()
	{
		try
		{
			byte[] _skinByte = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
			Texture2D _skinNew = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
			_skinNew.LoadImage(_skinByte);
			_skinNew.filterMode = FilterMode.Point;
			_skinNew.Apply();
			Texture oldTexture = this.previewLogo.mainTexture;
			this.previewLogo.mainTexture = _skinNew;
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.previewLogo.mainTexture);
		}
		catch (Exception ex)
		{
			Exception e = ex;
			Debug.LogWarning(e);
		}
		FriendPreview[] fps = this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		List<FriendPreview> toRemove = new List<FriendPreview>();
		List<string> existingPreviews = new List<string>();
		foreach (FriendPreview fp in fps)
		{
			bool found = false;
			foreach (Dictionary<string, string> member in FriendsController.sharedController.clanMembers)
			{
				string _id;
				if (member.TryGetValue("id", out _id))
				{
					if (_id.Equals(fp.id))
					{
						found = true;
						fp.nm.text = member["nick"];
						break;
					}
				}
			}
			if (!found)
			{
				toRemove.Add(fp);
			}
			else if (fp.id != null)
			{
				existingPreviews.Add(fp.id);
			}
		}
		foreach (FriendPreview fp2 in toRemove)
		{
			fp2.transform.parent = null;
			UnityEngine.Object.Destroy(fp2.gameObject);
		}
		foreach (Dictionary<string, string> member2 in FriendsController.sharedController.clanMembers)
		{
			if (member2.ContainsKey("id") && !existingPreviews.Contains(member2["id"]) && !member2["id"].Equals(FriendsController.sharedController.id))
			{
				GameObject f = UnityEngine.Object.Instantiate(Resources.Load("Friend")) as GameObject;
				f.transform.parent = this.friendsGrid.transform;
				f.transform.localScale = new Vector3(1f, 1f, 1f);
				f.GetComponent<FriendPreview>().id = member2["id"];
				f.GetComponent<FriendPreview>().ClanMember = true;
				f.GetComponent<FriendPreview>().join.GetComponent<JoinRoomFromFrendsButton>().joinRoomFromFrends = this.joinRoomFromFrends;
				if (member2.ContainsKey("nick"))
				{
					f.GetComponent<FriendPreview>().nm.text = member2["nick"];
				}
			}
		}
		yield return null;
		this.timeOfLastSort = Time.realtimeSinceStartup;
		this._SortFriendPreviews();
		yield break;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0001A18C File Offset: 0x0001838C
	private void HandleArrowClicked(object sender, EventArgs e)
	{
		if ((sender as ButtonHandler).gameObject == this.Left)
		{
			this._currentLogoInd--;
			if (this._currentLogoInd < 0)
			{
				this._currentLogoInd = this._logos.Count - 1;
				if (this._currentLogoInd < 0)
				{
					this._currentLogoInd = 0;
				}
			}
		}
		else
		{
			this._currentLogoInd++;
			if (this._currentLogoInd >= this._logos.Count)
			{
				this._currentLogoInd = 0;
			}
		}
		this.logo.mainTexture = this._logos[this._currentLogoInd];
		SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.logo.mainTexture);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0001A258 File Offset: 0x00018458
	private void HandleBackClicked(object sender, EventArgs e)
	{
		this._isCancellationRequested = true;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0001A264 File Offset: 0x00018464
	private void HandleCancellation()
	{
		if (this._clanIncomingInvitesController.Value.Map((ClanIncomingInvitesController c) => c.inboxPanel).Map((GameObject p) => p.activeInHierarchy))
		{
			this._clanIncomingInvitesController.Value.Do(delegate(ClanIncomingInvitesController c)
			{
				c.HandleBackFromInboxPressed();
			});
			return;
		}
		if (this.deleteClanDialog.activeSelf)
		{
			this.deleteClanDialog.SetActive(false);
			this.DisableStatisticsPanel(false);
			return;
		}
		if (this._defendTime > 0f)
		{
			return;
		}
		if (this.CreateClanPanel.activeInHierarchy)
		{
			this.CreateClanPanel.SetActive(false);
			this.NoClanPanel.SetActive(true);
			return;
		}
		if (this.statisiticPanel.activeInHierarchy)
		{
			this.statisiticPanel.SetActive(false);
			ClansGUIController.AtStatsPanel = false;
			this.clanPanel.SetActive(true);
			return;
		}
		if (this.addInClanPanel.activeInHierarchy)
		{
			this.HideAddPanel();
			this.clanPanel.SetActive(true);
			return;
		}
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0001A3D0 File Offset: 0x000185D0
	private void HideAddPanel()
	{
		this.addInClanPanel.SetActive(false);
		FriendsController.sharedController.StopRefreshingOnlineWithClanInfo();
		FriendsController.sharedController.StartRefreshingClanOnline();
		ClansGUIController.AtAddPanel = false;
		foreach (object obj in this.addFriendsGrid.transform)
		{
			Transform transform = (Transform)obj;
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0001A478 File Offset: 0x00018678
	private void HandleEditClicked(object sender, EventArgs e)
	{
		this.GoToSM();
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0001A480 File Offset: 0x00018680
	public void GoToSM()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("SkinEditorController"));
		SkinEditorController component = gameObject.GetComponent<SkinEditorController>();
		if (component != null)
		{
			Action<string> backHandler = null;
			backHandler = delegate(string name)
			{
				MenuBackgroundMusic.sharedMusic.StopCustomMusicFrom(SkinEditorController.sharedController.gameObject);
				SkinEditorController.ExitFromSkinEditor -= backHandler;
				this.logo.mainTexture = EditorTextures.CreateCopyTexture(SkinsController.logoClanUserTexture);
				if (this.InClan)
				{
					Debug.Log("InClan");
					byte[] inArray = SkinsController.logoClanUserTexture.EncodeToPNG();
					FriendsController.sharedController.clanLogo = Convert.ToBase64String(inArray);
					FriendsController.sharedController.ChangeClanLogo();
					this.previewLogo.mainTexture = EditorTextures.CreateCopyTexture(SkinsController.logoClanUserTexture);
				}
				else if (!string.IsNullOrEmpty(name))
				{
					this._logos.Add(this.logo.mainTexture as Texture2D);
					this._currentLogoInd = this._logos.Count - 1;
				}
				this.gameObject.SetActive(true);
			};
			SkinEditorController.ExitFromSkinEditor += backHandler;
			if (this.InClan)
			{
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.previewLogo.mainTexture);
			}
			else
			{
				SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.logo.mainTexture);
			}
			SkinEditorController.modeEditor = SkinEditorController.ModeEditor.LogoClan;
			SkinEditorController.currentSkin = EditorTextures.CreateCopyTexture((Texture2D)this.logo.mainTexture);
			gameObject.transform.parent = null;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001A560 File Offset: 0x00018760
	public void FailedSendBuyClan()
	{
		FriendsController.sharedController.FailedSendNewClan -= this.FailedSendBuyClan;
		FriendsController.sharedController.ReturnNewIDClan -= this.ReturnIDNewClan;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001A59C File Offset: 0x0001879C
	public void ReturnIDNewClan(int _idNewClan)
	{
		FriendsController.sharedController.FailedSendNewClan -= this.FailedSendBuyClan;
		FriendsController.sharedController.ReturnNewIDClan -= this.ReturnIDNewClan;
		if (_idNewClan > 0)
		{
			this.BlockGUI = false;
			FriendsController.sharedController.ClanID = _idNewClan.ToString();
			FriendsController.sharedController.clanLeaderID = FriendsController.sharedController.id;
			Texture2D texture2D = this.logo.mainTexture as Texture2D;
			byte[] inArray = texture2D.EncodeToPNG();
			string clanLogo = Convert.ToBase64String(inArray);
			FriendsController.sharedController.clanLogo = clanLogo;
			Texture mainTexture = this.previewLogo.mainTexture;
			this.previewLogo.mainTexture = this.logo.mainTexture;
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture(this.previewLogo.mainTexture);
			if (mainTexture != null)
			{
			}
			FriendsController.sharedController.clanName = this.inputNameClanLabel.text;
			this.nameClanLabel.text = FriendsController.sharedController.clanName;
			this.BuyNewClan();
			if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && Storager.getInt("ShownCreateClanRewardWindowKey", false) == 0 && !Device.isPixelGunLow)
			{
				this.rewardCreateClanWindow.SetActive(true);
				Storager.setInt("ShownCreateClanRewardWindowKey", 1, false);
			}
		}
		else
		{
			base.StartCoroutine(this.ShowThisNameInUse());
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001A700 File Offset: 0x00018900
	public IEnumerator ShowThisNameInUse()
	{
		this.NameIsUsedPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		this.NameIsUsedPanel.SetActive(false);
		this.BlockGUI = false;
		yield break;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0001A71C File Offset: 0x0001891C
	public IEnumerator ShowCheckConnection()
	{
		this.CheckConnectionPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		this.CheckConnectionPanel.SetActive(false);
		this.BlockGUI = false;
		yield break;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0001A738 File Offset: 0x00018938
	public void BuyNewClan()
	{
		int clansPrice = Defs.ClansPrice;
		int @int = Storager.getInt("Coins", false);
		int val = @int - clansPrice;
		Storager.setInt("Coins", val, false);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		this.CreateClanPanel.SetActive(false);
		this.InClan = true;
		this.ShowClanPanel();
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0001A790 File Offset: 0x00018990
	private void HandleCreateClanClicked(object sender, EventArgs e)
	{
		Action act = null;
		act = delegate()
		{
			this.CreateClanPanel.SetActive(true);
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int clansPrice = Defs.ClansPrice;
			int @int = Storager.getInt("Coins", false);
			int num = @int - clansPrice;
			Action<string> showShop = null;
			showShop = delegate(string pressedbutton)
			{
				EtceteraAndroidManager.alertButtonClickedEvent -= showShop;
				if (pressedbutton.Equals(Defs.CancelButtonTitle))
				{
					return;
				}
				coinsShop.thisScript.notEnoughCurrency = "Coins";
				coinsShop.thisScript.onReturnAction = act;
				coinsShop.showCoinsShop();
			};
			Texture2D texture2D = this.logo.mainTexture as Texture2D;
			byte[] inArray = texture2D.EncodeToPNG();
			string skinClan = Convert.ToBase64String(inArray);
			if (num >= 0)
			{
				if (this.inputNameClanLabel.text.Equals(string.Empty))
				{
					this.StartCoroutine(this.ShowThisNameInUse());
				}
				else
				{
					FriendsController.sharedController.SendCreateClan(FriendsController.sharedController.id, this.inputNameClanLabel.text, skinClan, new Action<string>(this.ErrorHandler));
					FriendsController.sharedController.FailedSendNewClan += this.FailedSendBuyClan;
					FriendsController.sharedController.ReturnNewIDClan += this.ReturnIDNewClan;
				}
				this.BlockGUI = true;
			}
			else
			{
				showShop("Yes!");
			}
		};
		act();
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0001A7D0 File Offset: 0x000189D0
	private void ErrorHandler(string error)
	{
		FriendsController.sharedController.FailedSendNewClan -= this.FailedSendBuyClan;
		FriendsController.sharedController.ReturnNewIDClan -= this.ReturnIDNewClan;
		this.BlockGUI = false;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0001A808 File Offset: 0x00018A08
	private void HandleEditLogoInPreviewClicked(object sender, EventArgs e)
	{
		this.GoToSM();
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0001A810 File Offset: 0x00018A10
	private void HandleLeaveClicked(object sender, EventArgs e)
	{
		this.InClan = false;
		this.NoClanPanel.SetActive(true);
		FriendsController.sharedController.ExitClan(null);
		FriendsController.sharedController.ClearClanData();
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0001A848 File Offset: 0x00018A48
	private void HandleAddMembersClicked(object sender, EventArgs e)
	{
		this.ShowAddMembersScreen();
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0001A850 File Offset: 0x00018A50
	internal void ShowAddMembersScreen()
	{
		this.clanPanel.SetActive(false);
		this.addInClanPanel.SetActive(true);
		FriendsController.sharedController.StartRefreshingOnlineWithClanInfo();
		FriendsController.sharedController.StopRefreshingClanOnline();
		ClansGUIController.AtAddPanel = true;
		base.StartCoroutine(this.FillAddMembers());
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0001A89C File Offset: 0x00018A9C
	private void HandleDeleteClanClicked(object sender, EventArgs e)
	{
		this.deleteClanDialog.SetActive(true);
		this.DisableStatisticsPanel(true);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0001A8B4 File Offset: 0x00018AB4
	private void HandleYesDelClanClicked(object sender, EventArgs e)
	{
		this.deleteClanDialog.SetActive(false);
		this.DisableStatisticsPanel(false);
		this.InClan = false;
		this.statisiticPanel.SetActive(false);
		FriendsController.sharedController.DeleteClan();
		FriendsController.sharedController.ClearClanData();
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0001A8FC File Offset: 0x00018AFC
	private void HandleNoDelClanClicked(object sender, EventArgs e)
	{
		this._isCancellationRequested = true;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0001A908 File Offset: 0x00018B08
	private void DisableStatisticsPanel(bool disable)
	{
		this.BackBut.GetComponent<UIButton>().isEnabled = !disable;
		this.deleteClanButton.GetComponent<UIButton>().isEnabled = !disable;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0001A940 File Offset: 0x00018B40
	private void HandleStatisticsButtonClicked(object sender, EventArgs e)
	{
		this.clanPanel.SetActive(false);
		this.statisiticPanel.SetActive(true);
		ClansGUIController.AtStatsPanel = true;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0001A960 File Offset: 0x00018B60
	public void ShowClanPanel()
	{
		this.clanPanel.SetActive(true);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001A970 File Offset: 0x00018B70
	private IEnumerator FillAddMembers()
	{
		foreach (object obj in this.addFriendsGrid.transform)
		{
			Transform child = (Transform)obj;
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
		foreach (string friend in FriendsController.sharedController.friends)
		{
			Dictionary<string, object> playerInfo;
			if (FriendsController.sharedController.playersInfo.TryGetValue(friend, out playerInfo))
			{
				object playerNode;
				if (playerInfo.TryGetValue("player", out playerNode))
				{
					Dictionary<string, string> playerDictionary = (playerNode as Dictionary<string, object>).Map((Dictionary<string, object> d) => d.ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => Convert.ToString(kv.Value)));
					string clanCreatorId;
					if (playerDictionary.TryGetValue("clan_creator_id", out clanCreatorId) && clanCreatorId == FriendsController.sharedController.id)
					{
						continue;
					}
				}
				GameObject f = UnityEngine.Object.Instantiate(Resources.Load("Friend")) as GameObject;
				FriendPreview fp = f.GetComponent<FriendPreview>();
				f.transform.parent = this.addFriendsGrid.transform;
				f.transform.localScale = new Vector3(1f, 1f, 1f);
				fp.ClanInvite = true;
				fp.id = friend;
				fp.join.GetComponent<JoinRoomFromFrendsButton>().joinRoomFromFrends = this.joinRoomFromFrends;
				Dictionary<string, string> plDict = playerInfo.ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => Convert.ToString(kv.Value));
				if (playerInfo.ContainsKey("nick"))
				{
					fp.nm.text = plDict["nick"];
				}
				if (playerInfo.ContainsKey("rank"))
				{
					string r = plDict["rank"];
					if (r.Equals("0"))
					{
						r = "1";
					}
					fp.rank.spriteName = "Rank_" + r;
				}
				if (playerInfo.ContainsKey("skin"))
				{
					fp.SetSkin(plDict["skin"]);
				}
				fp.FillClanAttrs(plDict);
			}
		}
		yield return null;
		this.addFriendsGrid.Reposition();
		yield break;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0001A98C File Offset: 0x00018B8C
	private void Update()
	{
		if (this.startPanel.activeSelf != !FriendsController.readyToOperate)
		{
			this.startPanel.SetActive(!FriendsController.readyToOperate);
		}
		if (this._isCancellationRequested)
		{
			this.HandleCancellation();
			this._isCancellationRequested = false;
		}
		this.addMembersButton.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID) && !this.BlockGUI && FriendsController.ClanDataSettted);
		this.previewLogo.transform.parent.GetComponent<UIButton>().isEnabled = this.addMembersButton.activeInHierarchy;
		this.tapToEdit.gameObject.SetActive(this.addMembersButton.activeInHierarchy);
		this.leaveButton.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && !FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID));
		this.deleteClanButton.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID));
		this.changeClanNameInput.gameObject.SetActive(!string.IsNullOrEmpty(FriendsController.sharedController.id) && FriendsController.sharedController.clanLeaderID != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID) && !this.BlockGUI);
		this.InClan = !string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
		this.NoClanPanel.SetActive(FriendsController.readyToOperate && !this.InClan && !this.CreateClanPanel.activeInHierarchy && SkinEditorController.sharedController == null && this.CurrentState != ClansGUIController.State.Inbox && this.CurrentState != ClansGUIController.State.ProfileDetails);
		this.clanPanel.SetActive(FriendsController.readyToOperate && this.InClan && !ClansGUIController.AtAddPanel && !ClansGUIController.AtStatsPanel && !ClansGUIController.ShowProfile && this.CurrentState != ClansGUIController.State.Inbox && this.CurrentState != ClansGUIController.State.ProfileDetails);
		this.statisiticPanel.SetActive(FriendsController.readyToOperate && this.InClan && !ClansGUIController.AtAddPanel && ClansGUIController.AtStatsPanel);
		bool activeInHierarchy = this.addInClanPanel.activeInHierarchy;
		this.addInClanPanel.SetActive(FriendsController.readyToOperate && this.InClan && ClansGUIController.AtAddPanel && !ClansGUIController.AtStatsPanel && this.CurrentState != ClansGUIController.State.ProfileDetails);
		if (!this.InClan)
		{
			this.deleteClanDialog.SetActive(false);
			this.DisableStatisticsPanel(false);
		}
		if (this.clanPanel.activeInHierarchy)
		{
			this.statisticsButton.SetActive(!this.BlockGUI);
			this.friendsGrid.gameObject.SetActive(!this.BlockGUI);
		}
		if (!this.addInClanPanel.activeInHierarchy && activeInHierarchy)
		{
			this.HideAddPanel();
		}
		if (!this.statisiticPanel.activeInHierarchy)
		{
			ClansGUIController.AtStatsPanel = false;
		}
		if (ClansGUIController.ShowProfile && (!this.InClan || !FriendsController.readyToOperate))
		{
			this._friendProfileController.HandleBackClicked();
		}
		if (ClansGUIController.AtAddPanel)
		{
			this.clanIsFull.gameObject.SetActive(FriendsController.sharedController.ClanLimitReached);
		}
		this.SetScreenMessage();
		if (this.InClan)
		{
			this.countMembersLabel.text = string.Format("{0}\n{1}", LocalizationStore.Get("Key_0983"), FriendsController.sharedController.clanMembers.Count);
		}
		this.noMembersLabel.SetActive(FriendsController.sharedController.clanMembers != null && FriendsController.sharedController.clanMembers.Count < 2);
		this.ClanName.SetActive(!this.BlockGUI);
		if (!this.statisiticPanel.activeInHierarchy)
		{
			this.BackBut.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		}
		this.CreateClanButton.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		this.Left.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		this.Right.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		this.EditLogoBut.GetComponent<UIButton>().isEnabled = !this.BlockGUI;
		if (this._defendTime > 0f)
		{
			this._defendTime -= Time.deltaTime;
		}
		this.friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = (this.friendsGrid.transform.childCount > 4);
		if (this.friendsGrid.transform.childCount > 0 && this.friendsGrid.transform.childCount <= 4)
		{
			float num = 0f;
			foreach (object obj in this.friendsGrid.transform)
			{
				Transform transform = (Transform)obj;
				num += transform.localPosition.x;
			}
			num /= (float)this.friendsGrid.transform.childCount;
			Transform parent = this.friendsGrid.transform.parent;
			if (parent != null)
			{
				UIPanel component = parent.GetComponent<UIPanel>();
				if (component != null)
				{
					component.clipOffset = new Vector2(num, component.clipOffset.y);
					parent.localPosition = new Vector3(-num, parent.localPosition.y, parent.localPosition.z);
				}
			}
		}
		if (Time.realtimeSinceStartup - this.timeOfLastSort > 10f)
		{
			FriendsGUIController.RaiseUpdaeOnlineEvent();
			this.timeOfLastSort = Time.realtimeSinceStartup;
			this._SortFriendPreviews();
		}
		UISprite[] value = this._newMessagesOverlays.Value;
		foreach (UISprite uisprite in value)
		{
			if (ClanIncomingInvitesController.CurrentRequest == null || !ClanIncomingInvitesController.CurrentRequest.IsCompleted)
			{
				uisprite.gameObject.SetActive(false);
			}
			else if (ClanIncomingInvitesController.CurrentRequest.IsCanceled || ClanIncomingInvitesController.CurrentRequest.IsFaulted)
			{
				uisprite.gameObject.SetActive(false);
			}
			else
			{
				uisprite.gameObject.SetActive(ClanIncomingInvitesController.CurrentRequest.Result.Count > 0);
			}
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0001B10C File Offset: 0x0001930C
	private void SetScreenMessage()
	{
		if (this.receivingPlashka == null)
		{
			return;
		}
		string text = string.Empty;
		if (!FriendsController.ClanDataSettted && this.InClan)
		{
			text = LocalizationStore.Key_0348;
		}
		else if (FriendsController.sharedController != null && this.InClan)
		{
			if (this._friendProfileController != null && this._friendProfileController.FriendProfileGo != null && this._friendProfileController.FriendProfileGo.activeInHierarchy)
			{
				if (FriendsController.sharedController.NumberOffFullInfoRequests > 0)
				{
					text = LocalizationStore.Key_0348;
				}
			}
			else if (this.CreateClanPanel.activeInHierarchy && FriendsController.sharedController.NumberOfCreateClanRequests > 0)
			{
				text = LocalizationStore.Key_0348;
			}
		}
		else if ((!this.InClan || !FriendsController.readyToOperate) && !this.NoClanPanel.activeInHierarchy && !this.CreateClanPanel.activeInHierarchy && !this.clanPanel.activeInHierarchy && !this.statisiticPanel.activeInHierarchy && !this.addInClanPanel.activeInHierarchy)
		{
			if (this.CurrentState == ClansGUIController.State.Inbox)
			{
				if (this.CurrentState != ClansGUIController.State.Inbox)
				{
					goto IL_17E;
				}
				if (ClanIncomingInvitesController.CurrentRequest.Filter((Task<List<object>> t) => t.IsCompleted) != null)
				{
					goto IL_17E;
				}
			}
			text = LocalizationStore.Key_0348;
		}
		IL_17E:
		if (!string.IsNullOrEmpty(text))
		{
			this.receivingPlashka.GetComponent<UILabel>().text = text;
			this.receivingPlashka.SetActive(true);
		}
		else
		{
			this.receivingPlashka.SetActive(false);
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0001B2D0 File Offset: 0x000194D0
	private void OnDestroy()
	{
		ClansGUIController.sharedController = null;
		this._friendProfileController.Dispose();
		FriendsController.sharedController.StopRefreshingClanOnline();
		FriendsController friendsController = FriendsController.sharedController;
		friendsController.onChangeClanName = (FriendsController.OnChangeClanName)Delegate.Remove(friendsController.onChangeClanName, new FriendsController.OnChangeClanName(this.OnChangeClanName));
		ClansGUIController.AtAddPanel = false;
		ClansGUIController.AtStatsPanel = false;
		ClansGUIController.ShowProfile = false;
		UIInputRilisoft uiinputRilisoft = (!(this.ClanName != null)) ? null : this.ClanName.GetComponent<UIInputRilisoft>();
		if (uiinputRilisoft != null)
		{
			UIInputRilisoft uiinputRilisoft2 = uiinputRilisoft;
			uiinputRilisoft2.onFocus = (UIInputRilisoft.OnFocus)Delegate.Remove(uiinputRilisoft2.onFocus, new UIInputRilisoft.OnFocus(this.OnFocusCreateClanName));
			UIInputRilisoft uiinputRilisoft3 = uiinputRilisoft;
			uiinputRilisoft3.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Remove(uiinputRilisoft3.onFocusLost, new UIInputRilisoft.OnFocusLost(this.onFocusLostCreateClanName));
		}
		FriendsController.DisposeProfile();
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0001B3A8 File Offset: 0x000195A8
	private void OnFocusCreateClanName()
	{
		if (this.ClanName != null)
		{
			this.ClanName.GetComponent<UIInputRilisoft>().value = string.Empty;
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0001B3DC File Offset: 0x000195DC
	private void onFocusLostCreateClanName()
	{
		if (this.ClanName != null)
		{
			UIInputRilisoft component = this.ClanName.GetComponent<UIInputRilisoft>();
			if (string.IsNullOrEmpty(component.value))
			{
				component.value = LocalizationStore.Key_0589;
			}
		}
	}

	// Token: 0x04000327 RID: 807
	private const string ShownCreateClanRewardWindow = "ShownCreateClanRewardWindowKey";

	// Token: 0x04000328 RID: 808
	public static ClansGUIController sharedController;

	// Token: 0x04000329 RID: 809
	public static bool AtAddPanel;

	// Token: 0x0400032A RID: 810
	public static bool AtStatsPanel;

	// Token: 0x0400032B RID: 811
	public GameObject rewardCreateClanWindow;

	// Token: 0x0400032C RID: 812
	public UIWrapContent friendsGrid;

	// Token: 0x0400032D RID: 813
	public UIGrid addFriendsGrid;

	// Token: 0x0400032E RID: 814
	public JoinRoomFromFrends joinRoomFromFrends;

	// Token: 0x0400032F RID: 815
	public bool InClan;

	// Token: 0x04000330 RID: 816
	public GameObject NoClanPanel;

	// Token: 0x04000331 RID: 817
	public GameObject CreateClanPanel;

	// Token: 0x04000332 RID: 818
	public GameObject CreateClanButton;

	// Token: 0x04000333 RID: 819
	public GameObject EditLogoBut;

	// Token: 0x04000334 RID: 820
	public GameObject BackBut;

	// Token: 0x04000335 RID: 821
	public GameObject Left;

	// Token: 0x04000336 RID: 822
	public GameObject Right;

	// Token: 0x04000337 RID: 823
	public GameObject ClanName;

	// Token: 0x04000338 RID: 824
	public GameObject clanPanel;

	// Token: 0x04000339 RID: 825
	public GameObject editLogoInPreviewButton;

	// Token: 0x0400033A RID: 826
	public GameObject leaveButton;

	// Token: 0x0400033B RID: 827
	public GameObject addMembersButton;

	// Token: 0x0400033C RID: 828
	public GameObject noMembersLabel;

	// Token: 0x0400033D RID: 829
	public GameObject statisticsButton;

	// Token: 0x0400033E RID: 830
	public GameObject statisiticPanel;

	// Token: 0x0400033F RID: 831
	public GameObject deleteClanButton;

	// Token: 0x04000340 RID: 832
	public GameObject startPanel;

	// Token: 0x04000341 RID: 833
	public GameObject addInClanPanel;

	// Token: 0x04000342 RID: 834
	public GameObject NameIsUsedPanel;

	// Token: 0x04000343 RID: 835
	public GameObject CheckConnectionPanel;

	// Token: 0x04000344 RID: 836
	public GameObject topLevelObject;

	// Token: 0x04000345 RID: 837
	public UITexture logo;

	// Token: 0x04000346 RID: 838
	public UITexture previewLogo;

	// Token: 0x04000347 RID: 839
	public UILabel nameClanLabel;

	// Token: 0x04000348 RID: 840
	public UILabel countMembersLabel;

	// Token: 0x04000349 RID: 841
	public UILabel inputNameClanLabel;

	// Token: 0x0400034A RID: 842
	public UILabel tapToEdit;

	// Token: 0x0400034B RID: 843
	public UILabel clanIsFull;

	// Token: 0x0400034C RID: 844
	public UILabel changeClanResult;

	// Token: 0x0400034D RID: 845
	public GameObject receivingPlashka;

	// Token: 0x0400034E RID: 846
	public GameObject deleteClanDialog;

	// Token: 0x0400034F RID: 847
	public UIButton yesDelteClan;

	// Token: 0x04000350 RID: 848
	public UIButton noDeleteClan;

	// Token: 0x04000351 RID: 849
	public UIInput changeClanNameInput;

	// Token: 0x04000352 RID: 850
	private bool BlockGUI;

	// Token: 0x04000353 RID: 851
	private List<Texture2D> _logos = new List<Texture2D>();

	// Token: 0x04000354 RID: 852
	private int _currentLogoInd;

	// Token: 0x04000355 RID: 853
	private bool _inCoinsShop;

	// Token: 0x04000356 RID: 854
	private float timeOfLastSort;

	// Token: 0x04000357 RID: 855
	private readonly Lazy<UISprite[]> _newMessagesOverlays;

	// Token: 0x04000358 RID: 856
	private readonly Lazy<ClanIncomingInvitesController> _clanIncomingInvitesController;

	// Token: 0x04000359 RID: 857
	private FriendProfileController _friendProfileController;

	// Token: 0x0400035A RID: 858
	private IDisposable _backSubscription;

	// Token: 0x0400035B RID: 859
	public static bool ShowProfile;

	// Token: 0x0400035C RID: 860
	private bool _isCancellationRequested;

	// Token: 0x0400035D RID: 861
	private float _defendTime;

	// Token: 0x0200006C RID: 108
	internal enum State
	{
		// Token: 0x04000369 RID: 873
		Default,
		// Token: 0x0400036A RID: 874
		Inbox,
		// Token: 0x0400036B RID: 875
		ProfileDetails
	}
}
