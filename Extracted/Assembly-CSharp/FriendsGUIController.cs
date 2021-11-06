using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200012A RID: 298
public sealed class FriendsGUIController : MonoBehaviour, IFriendsGUIController
{
	// Token: 0x0600095E RID: 2398 RVA: 0x00038B4C File Offset: 0x00036D4C
	void IFriendsGUIController.Hide(bool h)
	{
		this.friendsPanel.gameObject.SetActive(!h);
		this.fon.SetActive(!h);
		FriendsGUIController.ShowProfile = h;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00038B78 File Offset: 0x00036D78
	public static void RaiseUpdaeOnlineEvent()
	{
		if (FriendsGUIController.UpdaeOnlineEvent != null)
		{
			FriendsGUIController.UpdaeOnlineEvent();
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00038B90 File Offset: 0x00036D90
	public void HandleProfileButton()
	{
		if (ProfileController.Instance != null)
		{
			this.Hide(true);
			ProfileController.Instance.ShowInterface(new Action[]
			{
				delegate()
				{
					if (ExperienceController.sharedController != null && ExpController.Instance != null)
					{
						ExperienceController.sharedController.isShowRanks = false;
						ExpController.Instance.InterfaceEnabled = false;
					}
					this.Hide(false);
				}
			});
		}
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00038BE8 File Offset: 0x00036DE8
	public void ShowBestPlayers(bool h)
	{
		this.friendsPanel.gameObject.SetActive(!h);
		this.leaderboardsView.gameObject.SetActive(h);
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00038C1C File Offset: 0x00036E1C
	public void RequestLeaderboards()
	{
		if (this._leaderboardsController != null)
		{
			this._leaderboardsController.RequestLeaderboards();
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00038C3C File Offset: 0x00036E3C
	public void MultyButtonHandler(object sender, EventArgs e)
	{
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ConnectScene";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00038CA0 File Offset: 0x00036EA0
	private void Start()
	{
		StoreKitEventListener.State.Mode = "Friends";
		StoreKitEventListener.State.PurchaseKey = "In friends";
		StoreKitEventListener.State.Parameters.Clear();
		if (this.multyButton != null)
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				this.multyButton.gameObject.SetActive(false);
			}
			else
			{
				ButtonHandler component = this.multyButton.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += this.MultyButtonHandler;
				}
			}
		}
		this.timeOfLastSort = Time.realtimeSinceStartup;
		Defs.ProfileFromFriends = 0;
		this._friendProfileController = new FriendProfileController(this, true);
		if (this.leaderboardsView != null && this._leaderboardsController == null)
		{
			this._leaderboardsController = this.leaderboardsView.gameObject.AddComponent<LeaderboardsController>();
			this._leaderboardsController.LeaderboardsView = this.leaderboardsView;
			this._leaderboardsController.FriendsGuiController = this;
			this._leaderboardsController.PlayerId = Storager.getString("AccountCreated", false);
		}
		FriendsController.sharedController.StartRefreshingOnline();
		base.StartCoroutine(this.SortFriendPreviewsAfterDelay());
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00038DD8 File Offset: 0x00036FD8
	private void OnEnable()
	{
		FriendsController.FriendsUpdated += this.UpdateGUI;
		base.StartCoroutine(this.__UpdateGUI());
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00038DF8 File Offset: 0x00036FF8
	public void UpdateGUI()
	{
		base.StartCoroutine(this.__UpdateGUI());
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00038E08 File Offset: 0x00037008
	private IEnumerator SortFriendPreviewsAfterDelay()
	{
		yield return null;
		yield return null;
		this._SortFriendPreviews();
		yield break;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00038E24 File Offset: 0x00037024
	private void _SortFriendPreviews()
	{
		FriendPreview[] componentsInChildren = this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		FriendPreview[] array = this.friendsGrid.GetComponentsInChildren<FriendPreview>(false);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		Array.Sort<FriendPreview>(array, (FriendPreview fp1, FriendPreview fp2) => fp1.name.CompareTo(fp2.name));
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

	// Token: 0x06000969 RID: 2409 RVA: 0x00039094 File Offset: 0x00037294
	private IEnumerator __UpdateGUI()
	{
		FriendPreview[] fps = this.friendsGrid.GetComponentsInChildren<FriendPreview>(true);
		Invitation[] invs = this.invitationsGrid.GetComponentsInChildren<Invitation>(true);
		Invitation[] sentInvs = this.sentInvitationsGrid.GetComponentsInChildren<Invitation>(true);
		Invitation[] clanInvs = this.ClanInvitationsGrid.GetComponentsInChildren<Invitation>(true);
		List<Invitation> clanInvtoRemove = new List<Invitation>();
		List<string> existingClanInvs = new List<string>();
		foreach (Invitation i in clanInvs)
		{
			bool found = false;
			foreach (Dictionary<string, string> ClanInv in FriendsController.sharedController.ClanInvites)
			{
			}
			if (!found)
			{
				clanInvtoRemove.Add(i);
			}
			else if (i.id != null)
			{
				existingClanInvs.Add(i.id);
			}
		}
		foreach (Invitation inv in clanInvtoRemove)
		{
			inv.transform.parent = null;
			UnityEngine.Object.Destroy(inv.gameObject);
		}
		foreach (Dictionary<string, string> ClanInv2 in FriendsController.sharedController.ClanInvites)
		{
			if (!existingClanInvs.Contains(ClanInv2["id"]))
			{
				GameObject f = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Invitation") as GameObject);
				f.transform.parent = this.ClanInvitationsGrid.transform;
				f.transform.localScale = new Vector3(1f, 1f, 1f);
				f.GetComponent<Invitation>().IsClanInv = true;
				if (ClanInv2.ContainsKey("id"))
				{
					f.GetComponent<Invitation>().id = ClanInv2["id"];
					f.GetComponent<Invitation>().recordId = ClanInv2["id"];
				}
				if (ClanInv2.ContainsKey("name"))
				{
					f.GetComponent<Invitation>().nm.text = ClanInv2["name"];
				}
				string clanLogo;
				if (ClanInv2.TryGetValue("logo", out clanLogo) && !string.IsNullOrEmpty(clanLogo))
				{
					f.GetComponent<Invitation>().clanLogoString = clanLogo;
				}
			}
		}
		yield return null;
		this.invitationsGrid.Reposition();
		this.sentInvitationsGrid.Reposition();
		this.ClanInvitationsGrid.Reposition();
		this.timeOfLastSort = Time.realtimeSinceStartup;
		this._SortFriendPreviews();
		yield break;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x000390B0 File Offset: 0x000372B0
	private void Update()
	{
		if (this.receivingPlashka != null && FriendsController.sharedController != null)
		{
			if ((this.friendsPanel != null && this.friendsPanel.gameObject.activeInHierarchy) || (this.inboxPanel != null && this.inboxPanel.gameObject.activeInHierarchy))
			{
				this.receivingPlashka.SetActive(FriendsController.sharedController.NumberOfFriendsRequests > 0);
				this.receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (this._friendProfileController != null && this._friendProfileController.FriendProfileGo != null && this._friendProfileController.FriendProfileGo.activeInHierarchy)
			{
				this.receivingPlashka.SetActive(FriendsController.sharedController.NumberOffFullInfoRequests > 0);
				this.receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else if (this.leaderboardsView != null && this.leaderboardsView.gameObject.activeInHierarchy)
			{
				this.receivingPlashka.SetActive(FriendsController.sharedController.NumberOfBestPlayersRequests > 0);
				this.receivingPlashka.GetComponent<UILabel>().text = LocalizationStore.Key_0348;
			}
			else
			{
				this.receivingPlashka.SetActive(false);
			}
		}
		this.friendsGrid.transform.parent.GetComponent<UIScrollView>().enabled = (this.friendsGrid.transform.childCount > 4);
		if (this.friendsGrid.transform.childCount > 0 && this.friendsGrid.transform.childCount <= 4 && Time.realtimeSinceStartup - this._timeLastFriendsScrollUpdate > 0.5f)
		{
			this._timeLastFriendsScrollUpdate = Time.realtimeSinceStartup;
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
			if (FriendsGUIController.UpdaeOnlineEvent != null)
			{
				FriendsGUIController.UpdaeOnlineEvent();
			}
			this.timeOfLastSort = Time.realtimeSinceStartup;
			this._SortFriendPreviews();
		}
		this.newMEssage.SetActive(FriendsController.sharedController.invitesToUs.Count > 0 || FriendsController.sharedController.ClanInvites.Count > 0);
		this.canAddLAbel.SetActive(FriendsController.sharedController.friends.Count == 0);
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00039438 File Offset: 0x00037638
	private void OnDisable()
	{
		FriendsController.FriendsUpdated -= this.UpdateGUI;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003944C File Offset: 0x0003764C
	private void OnDestroy()
	{
		FriendsController.sharedController.StopRefreshingOnline();
		this._friendProfileController.Dispose();
		FriendsGUIController.ShowProfile = false;
	}

	// Token: 0x040007A3 RID: 1955
	public static Action UpdaeOnlineEvent;

	// Token: 0x040007A4 RID: 1956
	public GameObject multyButton;

	// Token: 0x040007A5 RID: 1957
	public GameObject receivingPlashka;

	// Token: 0x040007A6 RID: 1958
	public UIWrapContent friendsGrid;

	// Token: 0x040007A7 RID: 1959
	public UIGrid invitationsGrid;

	// Token: 0x040007A8 RID: 1960
	public UIGrid sentInvitationsGrid;

	// Token: 0x040007A9 RID: 1961
	public UIGrid ClanInvitationsGrid;

	// Token: 0x040007AA RID: 1962
	public LeaderboardsView leaderboardsView;

	// Token: 0x040007AB RID: 1963
	public UIPanel friendsPanel;

	// Token: 0x040007AC RID: 1964
	public UIPanel inboxPanel;

	// Token: 0x040007AD RID: 1965
	public UIPanel friendProfilePanel;

	// Token: 0x040007AE RID: 1966
	public UIPanel facebookFriensPanel;

	// Token: 0x040007AF RID: 1967
	public UIPanel bestPlayersPanel;

	// Token: 0x040007B0 RID: 1968
	public GameObject fon;

	// Token: 0x040007B1 RID: 1969
	public GameObject newMEssage;

	// Token: 0x040007B2 RID: 1970
	public GameObject canAddLAbel;

	// Token: 0x040007B3 RID: 1971
	private float timeOfLastSort;

	// Token: 0x040007B4 RID: 1972
	public static bool ShowProfile;

	// Token: 0x040007B5 RID: 1973
	private bool invitationsInitialized;

	// Token: 0x040007B6 RID: 1974
	private float _timeLastFriendsScrollUpdate;

	// Token: 0x040007B7 RID: 1975
	private FriendProfileController _friendProfileController;

	// Token: 0x040007B8 RID: 1976
	private LeaderboardsController _leaderboardsController;
}
