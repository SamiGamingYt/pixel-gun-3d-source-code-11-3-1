using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x02000683 RID: 1667
internal sealed class LeaderboardScript : MonoBehaviour
{
	// Token: 0x060039F1 RID: 14833 RVA: 0x0012C240 File Offset: 0x0012A440
	public LeaderboardScript()
	{
		this._clansList = new List<LeaderboardItemViewModel>(101);
		this._friendsList = new List<LeaderboardItemViewModel>(101);
		this._playersList = new List<LeaderboardItemViewModel>(101);
		this._tournamentList = new List<LeaderboardItemViewModel>(201);
		base..ctor();
	}

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x060039F2 RID: 14834 RVA: 0x0012C298 File Offset: 0x0012A498
	// (remove) Token: 0x060039F3 RID: 14835 RVA: 0x0012C2B0 File Offset: 0x0012A4B0
	public static event EventHandler<ClickedEventArgs> PlayerClicked;

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x060039F4 RID: 14836 RVA: 0x0012C2C8 File Offset: 0x0012A4C8
	private LeaderboardsView LeaderboardView
	{
		get
		{
			return this._view.Value;
		}
	}

	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x060039F5 RID: 14837 RVA: 0x0012C2D8 File Offset: 0x0012A4D8
	public UILabel ExpirationLabel
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.expirationLabel : null;
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x060039F6 RID: 14838 RVA: 0x0012C308 File Offset: 0x0012A508
	public GameObject ExpirationIconObject
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.expirationIconObj : null;
		}
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x060039F7 RID: 14839 RVA: 0x0012C338 File Offset: 0x0012A538
	private GameObject TopFriendsGrid
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.friendsGrid.gameObject : null;
		}
	}

	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x060039F8 RID: 14840 RVA: 0x0012C36C File Offset: 0x0012A56C
	private GameObject TopPlayersGrid
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.bestPlayersGrid.gameObject : null;
		}
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x060039F9 RID: 14841 RVA: 0x0012C3A0 File Offset: 0x0012A5A0
	private GameObject TopClansGrid
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.clansGrid.gameObject : null;
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x060039FA RID: 14842 RVA: 0x0012C3D4 File Offset: 0x0012A5D4
	private GameObject TournamentGrid
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.tournamentGrid.gameObject : null;
		}
	}

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x060039FB RID: 14843 RVA: 0x0012C408 File Offset: 0x0012A608
	private GameObject ClansTableFooter
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.clansTableFooter : null;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x060039FC RID: 14844 RVA: 0x0012C438 File Offset: 0x0012A638
	private GameObject Headerleaderboard
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.leaderboardHeader : null;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x060039FD RID: 14845 RVA: 0x0012C468 File Offset: 0x0012A668
	private GameObject FooterLeaderboard
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.leaderboardFooter : null;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x060039FE RID: 14846 RVA: 0x0012C498 File Offset: 0x0012A698
	private GameObject FooterTournament
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.tournamentFooter : null;
		}
	}

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x060039FF RID: 14847 RVA: 0x0012C4C8 File Offset: 0x0012A6C8
	private GameObject FooterTableTournament
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.tournamentTableFooter : null;
		}
	}

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x06003A00 RID: 14848 RVA: 0x0012C4F8 File Offset: 0x0012A6F8
	private GameObject HeaderTournament
	{
		get
		{
			return (!(this.LeaderboardView == null)) ? this.LeaderboardView.tournamentHeader : null;
		}
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x0012C528 File Offset: 0x0012A728
	private void OnLeaderboardViewStateChanged(LeaderboardsView.State fromState, LeaderboardsView.State toState)
	{
		if (fromState != toState && toState == LeaderboardsView.State.BestPlayers)
		{
			base.StartCoroutine(this.ScrollTopPlayersGridTo(FriendsController.sharedController.id));
		}
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x0012C550 File Offset: 0x0012A750
	private void UpdateLocs()
	{
		if (this.ClansTableFooter != null)
		{
			this.ClansTableFooter.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
			{
				n.text = LocalizationStore.Get("Key_0053");
			});
		}
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x0012C5C8 File Offset: 0x0012A7C8
	private IEnumerator FillGrids(string response, string playerId, LeaderboardScript.GridState state)
	{
		while (this._fillLock)
		{
			yield return null;
		}
		this._fillLock = true;
		try
		{
			if (string.IsNullOrEmpty(playerId))
			{
				throw new ArgumentException("Player id should not be empty", "playerId");
			}
			Dictionary<string, object> d = Json.Deserialize(response ?? string.Empty) as Dictionary<string, object>;
			if (d == null)
			{
				Debug.LogWarning("Leaderboards response is ill-formed.");
				d = new Dictionary<string, object>();
			}
			else if (d.Count == 0)
			{
				Debug.LogWarning("Leaderboards response contains no elements.");
			}
			LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel();
			leaderboardItemViewModel.Id = playerId;
			leaderboardItemViewModel.Nickname = ProfileController.GetPlayerNameOrDefault();
			leaderboardItemViewModel.Rank = ExperienceController.sharedController.currentLevel;
			leaderboardItemViewModel.WinCount = RatingSystem.instance.currentRating;
			leaderboardItemViewModel.Highlight = true;
			leaderboardItemViewModel.ClanName = FriendsController.sharedController.Map((FriendsController s) => s.clanName, string.Empty);
			leaderboardItemViewModel.ClanLogo = FriendsController.sharedController.Map((FriendsController s) => s.clanLogo, string.Empty);
			LeaderboardItemViewModel me = leaderboardItemViewModel;
			object meObject;
			if (d.TryGetValue("me", out meObject))
			{
				Dictionary<string, object> meDictionary = meObject as Dictionary<string, object>;
				object myWinCount;
				if (meDictionary.TryGetValue("wins", out myWinCount))
				{
					int wins = Convert.ToInt32(myWinCount);
					PlayerPrefs.SetInt("TotalWinsForLeaderboards", wins);
				}
			}
			int _competitionId;
			if (d.ContainsKey("competition_id") && int.TryParse(d["competition_id"].ToString(), out _competitionId) && FriendsController.sharedController != null && _competitionId > FriendsController.sharedController.currentCompetition)
			{
				FriendsController.sharedController.SendRequestGetCurrentcompetition();
			}
			List<LeaderboardItemViewModel> rawFriendsList = LeaderboardsController.ParseLeaderboardEntries(playerId, "friends", d);
			HashSet<string> friendIds = new HashSet<string>(FriendsController.sharedController.friends);
			if (FriendsController.sharedController != null)
			{
				for (int j = rawFriendsList.Count - 1; j >= 0; j--)
				{
					LeaderboardItemViewModel item = rawFriendsList[j];
					Dictionary<string, object> info;
					if (friendIds.Contains(item.Id) && FriendsController.sharedController.friendsInfo.TryGetValue(item.Id, out info))
					{
						try
						{
							Dictionary<string, object> playerDict = info["player"] as Dictionary<string, object>;
							item.Nickname = Convert.ToString(playerDict["nick"]);
							item.Rank = Convert.ToInt32(playerDict["rank"]);
							object clanName;
							if (playerDict.TryGetValue("clan_name", out clanName))
							{
								item.ClanName = Convert.ToString(clanName);
							}
							object clanLogo;
							if (playerDict.TryGetValue("clan_logo", out clanLogo))
							{
								item.ClanLogo = Convert.ToString(clanLogo);
							}
						}
						catch (KeyNotFoundException)
						{
							Debug.LogError("Failed to process cached friend: " + item.Id);
						}
					}
					else
					{
						rawFriendsList.RemoveAt(j);
					}
				}
			}
			rawFriendsList.Add(me);
			List<LeaderboardItemViewModel> orderedFriendsList = LeaderboardScript.GroupAndOrder(rawFriendsList);
			Coroutine fillFriendsCoroutine = base.StartCoroutine(this.FillFriendsGrid(this.TopFriendsGrid, orderedFriendsList, state));
			yield return fillFriendsCoroutine;
			List<LeaderboardItemViewModel> rawTopPlayersList = LeaderboardsController.ParseLeaderboardEntries(playerId, "best_players", d);
			if (rawTopPlayersList.Any<LeaderboardItemViewModel>())
			{
				if (rawTopPlayersList.All((LeaderboardItemViewModel i) => i.Id != me.Id))
				{
					LeaderboardItemViewModel min = (from i in rawTopPlayersList
					orderby i.WinCount descending
					select i).Last<LeaderboardItemViewModel>();
					rawTopPlayersList.Remove(min);
					rawTopPlayersList.Add(me);
				}
				else
				{
					LeaderboardItemViewModel myitem = rawTopPlayersList.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == me.Id);
					if (myitem != null)
					{
						myitem.Nickname = me.Nickname;
						myitem.Rank = me.Rank;
						myitem.WinCount = me.WinCount;
						myitem.Highlight = me.Highlight;
						myitem.ClanName = me.ClanName;
						myitem.ClanLogo = me.ClanLogo;
					}
				}
			}
			List<LeaderboardItemViewModel> orderedTopPlayersList = LeaderboardScript.GroupAndOrder(rawTopPlayersList);
			this.AddCacheInProfileInfo(rawTopPlayersList);
			Coroutine fillPlayersCoroutine = base.StartCoroutine(this.FillPlayersGrid(this.TopPlayersGrid, orderedTopPlayersList, state));
			rawTopPlayersList.Clear();
			List<LeaderboardItemViewModel> rawTournamentList = LeaderboardsController.ParseLeaderboardEntries(playerId, "competition", d);
			if (rawTournamentList.Any<LeaderboardItemViewModel>())
			{
				if (rawTournamentList.All((LeaderboardItemViewModel i) => i.Id != me.Id))
				{
					if (rawTournamentList.Any((LeaderboardItemViewModel i) => i.WinCount <= me.WinCount))
					{
						LeaderboardItemViewModel min2 = (from i in rawTournamentList
						orderby i.WinCount descending
						select i).Last<LeaderboardItemViewModel>();
						rawTournamentList.Remove(min2);
						rawTournamentList.Add(me);
					}
				}
				else
				{
					LeaderboardItemViewModel myitem2 = rawTournamentList.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == me.Id);
					if (myitem2 != null)
					{
						myitem2.Nickname = me.Nickname;
						myitem2.Rank = me.Rank;
						myitem2.WinCount = me.WinCount;
						myitem2.Highlight = me.Highlight;
						myitem2.ClanName = me.ClanName;
						myitem2.ClanLogo = me.ClanLogo;
					}
				}
			}
			this.LeaderboardView.InTournamentTop = (rawTournamentList.Any<LeaderboardItemViewModel>() && rawTournamentList.All((LeaderboardItemViewModel i) => i.Id != me.Id));
			if (this.FooterTableTournament != null)
			{
				if (this.LeaderboardView.InTournamentTop)
				{
					this.FooterTableTournament.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = LocalizationStore.Get("Key_0053");
					});
					this.FooterTableTournament.transform.FindChild("LabelNick").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = me.Nickname;
					});
					this.FooterTableTournament.transform.FindChild("LabelWins").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = RatingSystem.instance.currentRating.ToString();
					});
					this.FooterTableTournament.SetActiveSafe(true);
				}
				else
				{
					this.FooterTableTournament.SetActiveSafe(false);
				}
			}
			List<LeaderboardItemViewModel> orderedTournamentList = LeaderboardScript.GroupAndOrder(rawTournamentList);
			Coroutine fillTournamentCoroutine = base.StartCoroutine(this.FillTournamentGrid(this.TournamentGrid, orderedTournamentList, state));
			rawTournamentList.Clear();
			List<LeaderboardItemViewModel> rawClansList = LeaderboardsController.ParseLeaderboardEntries(playerId, "top_clans", d);
			List<LeaderboardItemViewModel> orderedClansList = LeaderboardScript.GroupAndOrder(rawClansList);
			Coroutine fillClansCoroutine = base.StartCoroutine(this.FillClansGrid(this.TopClansGrid, orderedClansList, state));
			if (this.ClansTableFooter != null)
			{
				string clanId = FriendsController.sharedController.Map((FriendsController s) => s.ClanID);
				if (string.IsNullOrEmpty(clanId))
				{
					this.LeaderboardView.CanShowClanTableFooter = false;
					this.ClansTableFooter.SetActive(false);
				}
				else
				{
					LeaderboardItemViewModel myClanInTop = rawClansList.FirstOrDefault((LeaderboardItemViewModel c) => c.Id == clanId);
					this.LeaderboardView.CanShowClanTableFooter = (myClanInTop == null);
					this.ClansTableFooter.transform.FindChild("LabelPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = LocalizationStore.Get("Key_0053");
					});
					this.ClansTableFooter.transform.FindChild("LabelMembers").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel n)
					{
						n.text = string.Empty;
					});
					this.ClansTableFooter.transform.FindChild("LabelWins").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel w)
					{
						w.text = string.Empty;
					});
					UILabel clanLabel = this.ClansTableFooter.transform.FindChild("LabelNick").Map((Transform t) => t.gameObject.GetComponent<UILabel>());
					clanLabel.Do(delegate(UILabel cl)
					{
						cl.text = FriendsController.sharedController.Map((FriendsController s) => s.clanName, string.Empty);
					});
					clanLabel.Map((UILabel cl) => cl.GetComponentsInChildren<UITexture>(true).FirstOrDefault<UITexture>()).Do(delegate(UITexture t)
					{
						LeaderboardScript.SetClanLogo(t, FriendsController.sharedController.Map((FriendsController s) => s.clanLogo, string.Empty));
					});
				}
			}
			yield return fillPlayersCoroutine;
			yield return fillClansCoroutine;
			yield return fillTournamentCoroutine;
		}
		finally
		{
			this._fillLock = false;
		}
		yield break;
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x0012C610 File Offset: 0x0012A810
	private void AddCacheInProfileInfo(List<LeaderboardItemViewModel> _list)
	{
		foreach (LeaderboardItemViewModel leaderboardItemViewModel in _list)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("nick", leaderboardItemViewModel.Nickname);
			dictionary.Add("rank", leaderboardItemViewModel.Rank);
			dictionary.Add("clan_name", leaderboardItemViewModel.ClanName);
			dictionary.Add("clan_logo", leaderboardItemViewModel.ClanLogo);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("player", dictionary);
			if (!FriendsController.sharedController.profileInfo.ContainsKey(leaderboardItemViewModel.Id))
			{
				FriendsController.sharedController.profileInfo.Add(leaderboardItemViewModel.Id, dictionary2);
			}
			else
			{
				FriendsController.sharedController.profileInfo[leaderboardItemViewModel.Id] = dictionary2;
			}
		}
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x0012C718 File Offset: 0x0012A918
	private IEnumerator FillClansGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		UIWrapContent uiwrapContent = wrap;
		uiwrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uiwrapContent.onInitializeItem, new UIWrapContent.OnInitializeItem(delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			this.FillClanItem(gridGo, this._clansList, state, index, go);
		}));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		this._clansList.Clear();
		this._clansList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(this._clansList.Count > 0);
		int bound = Math.Min(15, this._clansList.Count);
		for (int i = 0; i != bound; i++)
		{
			LeaderboardItemViewModel item = this._clansList[i];
			GameObject newItem;
			if (i < childCount)
			{
				newItem = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = i.ToString(CultureInfo.InvariantCulture);
			}
			this.FillClanItem(gridGo, this._clansList, state, i, newItem);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
		yield break;
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x0012C760 File Offset: 0x0012A960
	private IEnumerator FillPlayersGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		UIWrapContent uiwrapContent = wrap;
		uiwrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uiwrapContent.onInitializeItem, new UIWrapContent.OnInitializeItem(delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			this.FillIndividualItem(gridGo, this._playersList, state, index, go);
		}));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		this._playersList.Clear();
		this._playersList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(this._playersList.Count > 0);
		int bound = Math.Min(15, this._playersList.Count);
		for (int i = 0; i != bound; i++)
		{
			LeaderboardItemViewModel item = this._playersList[i];
			GameObject newItem;
			if (i < childCount)
			{
				newItem = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = i.ToString(CultureInfo.InvariantCulture);
			}
			this.FillIndividualItem(gridGo, this._playersList, state, i, newItem);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
			while (!base.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			base.StartCoroutine(this.ScrollTopPlayersGridTo(FriendsController.sharedController.id));
		}
		yield break;
	}

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x06003A07 RID: 14855 RVA: 0x0012C7A8 File Offset: 0x0012A9A8
	// (set) Token: 0x06003A08 RID: 14856 RVA: 0x0012C7B0 File Offset: 0x0012A9B0
	private bool ScrollToPlayerRunning
	{
		get
		{
			return this._scrollToPlayerRunningVal;
		}
		set
		{
			this._scrollToPlayerRunningVal = value;
		}
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0012C7BC File Offset: 0x0012A9BC
	private IEnumerator ScrollTopPlayersGridTo(string viewId = null)
	{
		if (this.ScrollToPlayerRunning)
		{
			yield break;
		}
		this.ScrollToPlayerRunning = true;
		UIScrollView scroll = this.TopPlayersGrid.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scroll == null)
		{
			this.ScrollToPlayerRunning = false;
			yield break;
		}
		UIWrapContent wrapContent = this.TopPlayersGrid.GetComponent<UIWrapContent>();
		if (wrapContent == null)
		{
			this.ScrollToPlayerRunning = false;
			yield break;
		}
		int itemHeight = wrapContent.itemSize;
		float panelHeight = scroll.gameObject.GetComponent<UIPanel>().height;
		int visibleItemsCount = (int)(panelHeight / (float)itemHeight);
		Debug.Log("=>>> visible items count: " + visibleItemsCount);
		if (this._playersList.Count <= visibleItemsCount)
		{
			this.ScrollToPlayerRunning = false;
			yield break;
		}
		yield return null;
		if (viewId.IsNullOrEmpty())
		{
			scroll.MoveRelative(scroll.panel.clipOffset);
			this.ScrollToPlayerRunning = false;
			yield break;
		}
		LeaderboardItemViewModel me = this._playersList.FirstOrDefault((LeaderboardItemViewModel i) => i.Id == viewId);
		if (me == null)
		{
			this.ScrollToPlayerRunning = false;
			yield break;
		}
		int idx = this._playersList.IndexOf(me);
		int lastposibleIdxToScroll = this._playersList.Count - visibleItemsCount;
		idx = Mathf.Clamp(idx, 0, lastposibleIdxToScroll);
		if (idx > lastposibleIdxToScroll)
		{
			idx = lastposibleIdxToScroll;
		}
		float scrollFactor = (float)(itemHeight * idx);
		if (this._startScrollPos == null)
		{
			this._startScrollPos = new Vector3?(scroll.panel.gameObject.transform.localPosition);
		}
		scroll.panel.clipOffset = Vector2.zero;
		scroll.panel.gameObject.transform.localPosition = this._startScrollPos.Value;
		float scrollLeft = Mathf.Abs(scrollFactor);
		while (scrollLeft > 0f)
		{
			float s = (float)(itemHeight * visibleItemsCount - 1);
			if (scrollLeft > 0f)
			{
				if (scrollLeft - s < 0f)
				{
					scroll.MoveRelative(new Vector3(0f, scrollLeft));
					scrollLeft = 0f;
				}
				else
				{
					scrollLeft -= s;
					if (scrollFactor < 0f)
					{
						s *= -1f;
					}
					scroll.MoveRelative(new Vector3(0f, s));
				}
			}
		}
		if (idx + visibleItemsCount >= this._playersList.Count)
		{
			scroll.MoveRelative(new Vector3(0f, -60f));
		}
		this.ScrollToPlayerRunning = false;
		yield break;
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x0012C7E8 File Offset: 0x0012A9E8
	private IEnumerator FillFriendsGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		UIWrapContent uiwrapContent = wrap;
		uiwrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uiwrapContent.onInitializeItem, new UIWrapContent.OnInitializeItem(delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			this.FillIndividualItem(gridGo, this._friendsList, state, index, go);
		}));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		this._friendsList.Clear();
		this._friendsList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(this._friendsList.Count > 0);
		int bound = Math.Min(15, this._friendsList.Count);
		for (int i = 0; i != bound; i++)
		{
			LeaderboardItemViewModel item = this._friendsList[i];
			GameObject newItem;
			if (i < childCount)
			{
				newItem = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = i.ToString(CultureInfo.InvariantCulture);
			}
			this.FillIndividualItem(gridGo, this._friendsList, state, i, newItem);
		}
		int newChildCount = gridGo.transform.childCount;
		List<Transform> oddItemsToRemove = new List<Transform>(Math.Max(0, newChildCount - bound));
		for (int j = list.Count; j < newChildCount; j++)
		{
			oddItemsToRemove.Add(gridGo.transform.GetChild(j));
		}
		foreach (Transform item2 in oddItemsToRemove)
		{
			NGUITools.Destroy(item2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
		yield break;
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x0012C830 File Offset: 0x0012AA30
	private IEnumerator FillTournamentGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		UIWrapContent uiwrapContent = wrap;
		uiwrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uiwrapContent.onInitializeItem, new UIWrapContent.OnInitializeItem(delegate(GameObject go, int wrapIndex, int realIndex)
		{
			int index = -realIndex;
			this.FillIndividualItem(gridGo, this._tournamentList, state, index, go);
		}));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		this._tournamentList.Clear();
		this._tournamentList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(this._tournamentList.Count > 0);
		int bound = Math.Min(15, this._tournamentList.Count);
		for (int i = 0; i != bound; i++)
		{
			LeaderboardItemViewModel item = this._tournamentList[i];
			GameObject newItem;
			if (i < childCount)
			{
				newItem = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = i.ToString(CultureInfo.InvariantCulture);
			}
			this.FillIndividualItem(gridGo, this._tournamentList, state, i, newItem);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
		yield break;
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0012C878 File Offset: 0x0012AA78
	internal void RefreshMyLeaderboardEntries()
	{
		foreach (LeaderboardItemViewModel leaderboardItemViewModel in this._friendsList)
		{
			if (leaderboardItemViewModel != null && leaderboardItemViewModel.Id == FriendsController.sharedController.id)
			{
				leaderboardItemViewModel.Nickname = ProfileController.GetPlayerNameOrDefault();
				leaderboardItemViewModel.ClanName = (FriendsController.sharedController.clanName ?? string.Empty);
				break;
			}
		}
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x0012C924 File Offset: 0x0012AB24
	private void FillIndividualItem(GameObject grid, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state, int index, GameObject newItem)
	{
		LeaderboardScript.<FillIndividualItem>c__AnonStorey338 <FillIndividualItem>c__AnonStorey = new LeaderboardScript.<FillIndividualItem>c__AnonStorey338();
		<FillIndividualItem>c__AnonStorey.<>f__this = this;
		if (index >= list.Count)
		{
			return;
		}
		if (index < 0)
		{
			return;
		}
		<FillIndividualItem>c__AnonStorey.item = list[index];
		int num = index + 1;
		<FillIndividualItem>c__AnonStorey.item.Place = num;
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(<FillIndividualItem>c__AnonStorey.item);
			if (component.background != null)
			{
				if ((float)num % 2f > 0f)
				{
					Color color = new Color(0.8f, 0.9f, 1f);
					component.GetComponent<UIButton>().defaultColor = color;
					component.background.color = color;
				}
				else
				{
					Color color2 = new Color(1f, 1f, 1f);
					component.GetComponent<UIButton>().defaultColor = color2;
					component.background.color = color2;
				}
			}
		}
		component.Clicked += delegate(object sender, ClickedEventArgs e)
		{
			LeaderboardScript.PlayerClicked.Do(delegate(EventHandler<ClickedEventArgs> handler)
			{
				handler(<FillIndividualItem>c__AnonStorey.<>f__this, e);
			});
			if (Application.isEditor && Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Format("Clicked: {0}", e.Id));
			}
		};
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] array = new Transform[3];
		array[0] = componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsFirstPlace")).Map((UILabel l) => l.transform);
		array[1] = componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsSecondPlace")).Map((UILabel l) => l.transform);
		array[2] = componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsThirdPlace")).Map((UILabel l) => l.transform);
		Transform[] array2 = array;
		int p;
		for (p = 0; p != array2.Length; p++)
		{
			array2[p].Do(delegate(Transform l)
			{
				l.gameObject.SetActive(p + 1 == <FillIndividualItem>c__AnonStorey.item.Place && <FillIndividualItem>c__AnonStorey.item.WinCount > 0);
			});
		}
		newItem.transform.FindChild("LabelsPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel p)
		{
			p.text = ((<FillIndividualItem>c__AnonStorey.item.Place <= 3) ? string.Empty : <FillIndividualItem>c__AnonStorey.item.Place.ToString(CultureInfo.InvariantCulture));
		});
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x0012CBC0 File Offset: 0x0012ADC0
	private void FillClanItem(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state, int index, GameObject newItem)
	{
		LeaderboardScript.<FillClanItem>c__AnonStorey33A <FillClanItem>c__AnonStorey33A = new LeaderboardScript.<FillClanItem>c__AnonStorey33A();
		if (index >= list.Count)
		{
			return;
		}
		<FillClanItem>c__AnonStorey33A.item = list[index];
		<FillClanItem>c__AnonStorey33A.item.Place = index + 1;
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(<FillClanItem>c__AnonStorey33A.item);
		}
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] array = new Transform[3];
		array[0] = componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsFirstPlace")).Map((UILabel l) => l.transform);
		array[1] = componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsSecondPlace")).Map((UILabel l) => l.transform);
		array[2] = componentsInChildren.FirstOrDefault((UILabel l) => l.gameObject.name.Equals("LabelsThirdPlace")).Map((UILabel l) => l.transform);
		Transform[] array2 = array;
		int p;
		for (p = 0; p != array2.Length; p++)
		{
			array2[p].Do(delegate(Transform l)
			{
				l.gameObject.SetActive(p + 1 == <FillClanItem>c__AnonStorey33A.item.Place);
			});
		}
		newItem.transform.FindChild("LabelsPlace").Map((Transform t) => t.gameObject.GetComponent<UILabel>()).Do(delegate(UILabel p)
		{
			p.text = ((<FillClanItem>c__AnonStorey33A.item.Place <= 3) ? string.Empty : <FillClanItem>c__AnonStorey33A.item.Place.ToString(CultureInfo.InvariantCulture));
		});
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x0012CDA4 File Offset: 0x0012AFA4
	internal static void SetClanLogo(UITexture s, Texture2D clanLogoTexture)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture mainTexture = s.mainTexture;
		s.mainTexture = ((!(clanLogoTexture != null)) ? null : UnityEngine.Object.Instantiate<Texture2D>(clanLogoTexture));
		mainTexture.Do(new Action<Texture>(UnityEngine.Object.Destroy));
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0012CE00 File Offset: 0x0012B000
	internal static void SetClanLogo(UITexture s, string clanLogo)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture mainTexture = s.mainTexture;
		if (string.IsNullOrEmpty(clanLogo))
		{
			s.mainTexture = null;
		}
		else
		{
			s.mainTexture = LeaderboardItemViewModel.CreateLogoFromBase64String(clanLogo);
		}
		mainTexture.Do(new Action<Texture>(UnityEngine.Object.Destroy));
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0012CE64 File Offset: 0x0012B064
	private static List<LeaderboardItemViewModel> GroupAndOrder(List<LeaderboardItemViewModel> items)
	{
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		IOrderedEnumerable<IGrouping<int, LeaderboardItemViewModel>> orderedEnumerable = from vm in items
		group vm by vm.WinCount into g
		orderby g.Key descending
		select g;
		int num = 1;
		foreach (IGrouping<int, LeaderboardItemViewModel> source in orderedEnumerable)
		{
			IOrderedEnumerable<LeaderboardItemViewModel> orderedEnumerable2 = from vm in source
			orderby vm.Rank descending
			select vm;
			foreach (LeaderboardItemViewModel leaderboardItemViewModel in orderedEnumerable2)
			{
				leaderboardItemViewModel.Place = num;
				list.Add(leaderboardItemViewModel);
			}
			num += source.Count<LeaderboardItemViewModel>();
		}
		return list;
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06003A12 RID: 14866 RVA: 0x0012CF9C File Offset: 0x0012B19C
	// (set) Token: 0x06003A13 RID: 14867 RVA: 0x0012CFA4 File Offset: 0x0012B1A4
	public static LeaderboardScript Instance { get; private set; }

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06003A14 RID: 14868 RVA: 0x0012CFAC File Offset: 0x0012B1AC
	public UIPanel Panel
	{
		get
		{
			if (this._panelVal == null)
			{
				this._panelVal = ((this._view == null || !this._view.ObjectIsLoaded) ? null : this._view.Value.gameObject.GetComponent<UIPanel>());
			}
			return this._panelVal;
		}
	}

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06003A15 RID: 14869 RVA: 0x0012D00C File Offset: 0x0012B20C
	public bool UIEnabled
	{
		get
		{
			return this._view != null && this._view.ObjectIsActive;
		}
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x0012D028 File Offset: 0x0012B228
	public static int GetLeagueId()
	{
		return (int)RatingSystem.instance.currentLeague;
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x0012D034 File Offset: 0x0012B234
	internal static void RequestLeaderboards(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("Friends controller is null.");
			return;
		}
		if (LeaderboardScript._currentRequestPromise != null)
		{
			LeaderboardScript._currentRequestPromise.TrySetCanceled();
		}
		PlayerPrefs.SetString(LeaderboardScript.LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(2.0)).ToString("s", CultureInfo.InvariantCulture));
		LeaderboardScript._currentRequestPromise = new TaskCompletionSource<string>();
		FriendsController.sharedController.StartCoroutine(LeaderboardScript.LoadLeaderboardsCoroutine(playerId, LeaderboardScript._currentRequestPromise));
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0012D0E4 File Offset: 0x0012B2E4
	private void HandlePlayerClicked(object sender, ClickedEventArgs e)
	{
		if (FriendsController.sharedController != null && e.Id == FriendsController.sharedController.id)
		{
			return;
		}
		if (this.Panel == null)
		{
			Debug.LogError("Leaderboards panel not found.");
			return;
		}
		this.Panel.alpha = float.Epsilon;
		this.Panel.gameObject.SetActive(false);
		Action<bool> onCloseEvent = delegate(bool needUpdateFriendList)
		{
			this.Panel.gameObject.SetActive(true);
			this.TopFriendsGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TopClansGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TournamentGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
			{
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TopFriendsGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.TopClansGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.TournamentGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
			{
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.Panel.alpha = 1f;
			this._profileIsOpened = false;
		};
		this._profileIsOpened = true;
		FriendsController.ShowProfile(e.Id, ProfileWindowType.other, onCloseEvent);
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x0012D17C File Offset: 0x0012B37C
	private void Awake()
	{
		LeaderboardScript.Instance = this;
		this._view = new LazyObject<LeaderboardsView>(this._viewPrefab.ResourcePath, this._viewHandler);
		this._mainMenuController = new Lazy<MainMenuController>(() => MainMenuController.sharedController);
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0012D1D4 File Offset: 0x0012B3D4
	private void OnDestroy()
	{
		if (LeaderboardScript._currentRequestPromise != null)
		{
			LeaderboardScript._currentRequestPromise.TrySetCanceled();
		}
		LeaderboardScript._currentRequestPromise = null;
		LeaderboardScript.PlayerClicked = null;
		FriendsController.DisposeProfile();
		this._mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed -= this.ReturnBack;
		});
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLocs));
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x0012D238 File Offset: 0x0012B438
	public void Show()
	{
		base.StartCoroutine(this.ShowCoroutine());
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x0012D248 File Offset: 0x0012B448
	private IEnumerator ShowCoroutine()
	{
		if (!this._isInit)
		{
			if (this.LeaderboardView != null)
			{
				if (this.LeaderboardView.backButton != null)
				{
					this.LeaderboardView.backButton.Clicked += this.ReturnBack;
				}
				this.LeaderboardView.OnStateChanged += this.OnLeaderboardViewStateChanged;
			}
			LeaderboardScript.PlayerClicked = (EventHandler<ClickedEventArgs>)Delegate.Combine(LeaderboardScript.PlayerClicked, new EventHandler<ClickedEventArgs>(this.HandlePlayerClicked));
			LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLocs));
			this._isInit = true;
		}
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("Friends controller is null.");
			yield break;
		}
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			Debug.LogError("Player id should not be null.");
			yield break;
		}
		if (LeaderboardScript._currentRequestPromise == null)
		{
			LeaderboardScript._currentRequestPromise = new TaskCompletionSource<string>();
			FriendsController.sharedController.StartCoroutine(LeaderboardScript.LoadLeaderboardsCoroutine(playerId, LeaderboardScript._currentRequestPromise));
		}
		if (!this.CurrentRequest.IsCompleted)
		{
			string response = PlayerPrefs.GetString(LeaderboardScript.LeaderboardsResponseCache, string.Empty);
			if (string.IsNullOrEmpty(response))
			{
				yield return base.StartCoroutine(this.FillGrids(string.Empty, playerId, this._state));
			}
			else
			{
				this._state = LeaderboardScript.GridState.FillingWithCache;
				yield return base.StartCoroutine(this.FillGrids(response, playerId, this._state));
				this._state = LeaderboardScript.GridState.Cache;
			}
		}
		while (!this.CurrentRequest.IsCompleted)
		{
			yield return null;
		}
		if (this.CurrentRequest.IsCanceled)
		{
			Debug.LogWarning("Request is cancelled.");
		}
		else if (this.CurrentRequest.IsFaulted)
		{
			Debug.LogException(this.CurrentRequest.Exception);
		}
		else
		{
			string response2 = this.CurrentRequest.Result;
			this._state = LeaderboardScript.GridState.FillingWithResponse;
			yield return base.StartCoroutine(this.FillGrids(response2, playerId, this._state));
			this._state = LeaderboardScript.GridState.Response;
		}
		yield break;
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x0012D264 File Offset: 0x0012B464
	public void FillGrids(string rawDara)
	{
		base.StartCoroutine(this.FillGrids(rawDara, FriendsController.sharedController.id, LeaderboardScript.GridState.Response));
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x0012D280 File Offset: 0x0012B480
	private static string FormatExpirationLabel(float expirationTimespanSeconds)
	{
		if (expirationTimespanSeconds < 0f)
		{
			throw new ArgumentOutOfRangeException("expirationTimespanSeconds");
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)expirationTimespanSeconds);
		string result;
		try
		{
			string text = string.Format(LocalizationStore.Get("Key_2857"), Convert.ToInt32(Math.Floor(timeSpan.TotalDays)), timeSpan.Hours, timeSpan.Minutes);
			result = text;
		}
		catch
		{
			if (timeSpan.TotalHours < 1.0)
			{
				string text2 = string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
				result = text2;
			}
			else if (timeSpan.TotalDays < 1.0)
			{
				string text3 = string.Format("{0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				result = text3;
			}
			else
			{
				string text4 = string.Format("{0}d {1}:{2:00}:{3:00}", new object[]
				{
					Convert.ToInt32(Math.Floor(timeSpan.TotalDays)),
					timeSpan.Hours,
					timeSpan.Minutes,
					timeSpan.Seconds
				});
				result = text4;
			}
		}
		return result;
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0012D408 File Offset: 0x0012B608
	private void Update()
	{
		if (!this._isInit)
		{
			return;
		}
		if (Time.realtimeSinceStartup > this._expirationNextUpateTimeSeconds)
		{
			this._expirationNextUpateTimeSeconds = Time.realtimeSinceStartup + 5f;
			if (this.ExpirationLabel != null)
			{
				if (this._state == LeaderboardScript.GridState.Empty)
				{
					this.ExpirationLabel.text = LocalizationStore.Key_0348;
				}
				else
				{
					float num = FriendsController.sharedController.expirationTimeCompetition - Time.realtimeSinceStartup;
					if (num <= 0f)
					{
						this.ExpirationLabel.text = string.Empty;
						this.ExpirationIconObject.SetActiveSafe(false);
					}
					else
					{
						this.ExpirationLabel.text = LeaderboardScript.FormatExpirationLabel(num);
						this.ExpirationIconObject.SetActiveSafe(true);
					}
				}
			}
		}
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x0012D4D0 File Offset: 0x0012B6D0
	private void OnDisable()
	{
		this.ScrollToPlayerRunning = false;
	}

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x06003A21 RID: 14881 RVA: 0x0012D4DC File Offset: 0x0012B6DC
	private Task<string> CurrentRequest
	{
		get
		{
			return LeaderboardScript._currentRequestPromise.Map((TaskCompletionSource<string> p) => p.Task);
		}
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x0012D508 File Offset: 0x0012B708
	private static IEnumerator LoadLeaderboardsCoroutine(string playerId, TaskCompletionSource<string> requestPromise)
	{
		if (requestPromise == null)
		{
			throw new ArgumentNullException("requestPromise");
		}
		if (requestPromise.Task.IsCanceled)
		{
			yield break;
		}
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be null.", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			throw new InvalidOperationException("Friends controller should not be null.");
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			Debug.LogWarning("Current player id is empty.");
			requestPromise.TrySetException(new InvalidOperationException("Current player id is empty."));
			yield break;
		}
		string leaderboardsResponseCacheTimestampString = PlayerPrefs.GetString(LeaderboardScript.LeaderboardsResponseCacheTimestamp, string.Empty);
		DateTime leaderboardsResponseCacheTimestamp;
		if (DateTime.TryParse(leaderboardsResponseCacheTimestampString, out leaderboardsResponseCacheTimestamp))
		{
			DateTime timeOfNnextRequest = leaderboardsResponseCacheTimestamp + TimeSpan.FromMinutes((double)Defs.pauseUpdateLeaderboard);
			float secondsTillNextRequest = (float)(timeOfNnextRequest - DateTime.UtcNow).TotalSeconds;
			if (secondsTillNextRequest > 3600f)
			{
				secondsTillNextRequest = 0f;
			}
			yield return new WaitForSeconds(secondsTillNextRequest);
		}
		if (requestPromise.Task.IsCanceled)
		{
			yield break;
		}
		int leagueId = LeaderboardScript.GetLeagueId();
		WWWForm form = new WWWForm();
		form.AddField("action", "get_leaderboards_wins_tiers");
		form.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
		form.AddField("uniq_id", FriendsController.sharedController.id);
		int _tier = ExpController.OurTierForAnyPlace();
		form.AddField("tier", _tier);
		form.AddField("auth", FriendsController.Hash("get_leaderboards_wins_tiers", null));
		form.AddField("league_id", leagueId);
		form.AddField("competition_id", FriendsController.sharedController.currentCompetition);
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (request == null)
		{
			requestPromise.TrySetException(new InvalidOperationException("Request forbidden while connected."));
			TaskCompletionSource<string> newRequestPromise = new TaskCompletionSource<string>();
			LeaderboardScript._currentRequestPromise = newRequestPromise;
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LeaderboardScript.LoadLeaderboardsCoroutine(playerId, newRequestPromise));
			yield break;
		}
		while (!request.isDone)
		{
			if (requestPromise.Task.IsCanceled)
			{
				yield break;
			}
			yield return null;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			requestPromise.TrySetException(new InvalidOperationException(request.error));
			Debug.LogError(request.error);
			TaskCompletionSource<string> newRequestPromise2 = new TaskCompletionSource<string>();
			LeaderboardScript._currentRequestPromise = newRequestPromise2;
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LeaderboardScript.LoadLeaderboardsCoroutine(playerId, newRequestPromise2));
			yield break;
		}
		string responseText = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(responseText) || responseText == "fail")
		{
			string message = string.Format("Leaderboars response: '{0}'", responseText);
			requestPromise.TrySetException(new InvalidOperationException(message));
			Debug.LogWarning(message);
			TaskCompletionSource<string> newRequestPromise3 = new TaskCompletionSource<string>();
			LeaderboardScript._currentRequestPromise = newRequestPromise3;
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LeaderboardScript.LoadLeaderboardsCoroutine(playerId, newRequestPromise3));
			yield break;
		}
		requestPromise.TrySetResult(responseText);
		PlayerPrefs.SetString(LeaderboardScript.LeaderboardsResponseCache, responseText);
		PlayerPrefs.SetString(LeaderboardScript.LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture));
		yield break;
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x0012D538 File Offset: 0x0012B738
	public Task GetReturnFuture()
	{
		if (this._returnPromise.Task.IsCompleted)
		{
			this._returnPromise = new TaskCompletionSource<bool>();
		}
		this._mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed -= this.ReturnBack;
		});
		this._mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed += this.ReturnBack;
		});
		return this._returnPromise.Task;
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x0012D5AC File Offset: 0x0012B7AC
	public void ReturnBack(object sender, EventArgs e)
	{
		if (this._profileIsOpened)
		{
			return;
		}
		this.TopFriendsGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TopClansGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TournamentGrid.Map((GameObject go) => go.GetComponent<UIWrapContent>()).Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TopFriendsGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
		{
			s.ResetPosition();
			s.UpdatePosition();
		});
		this.TopClansGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
		{
			s.ResetPosition();
			s.UpdatePosition();
		});
		this.TournamentGrid.Map((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do(delegate(UIScrollView s)
		{
			s.ResetPosition();
			s.UpdatePosition();
		});
		this._returnPromise.TrySetResult(true);
		this._mainMenuController.Value.Do(delegate(MainMenuController m)
		{
			m.BackPressed -= this.ReturnBack;
		});
	}

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x06003A25 RID: 14885 RVA: 0x0012D7B4 File Offset: 0x0012B9B4
	private static string LeaderboardsResponseCache
	{
		get
		{
			return "Leaderboards.Tier.ResponseCache";
		}
	}

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06003A26 RID: 14886 RVA: 0x0012D7BC File Offset: 0x0012B9BC
	public static string LeaderboardsResponseCacheTimestamp
	{
		get
		{
			return "Leaderboards.New.ResponseCacheTimestamp";
		}
	}

	// Token: 0x04002AA6 RID: 10918
	private const int VisibleItemMaxCount = 15;

	// Token: 0x04002AA7 RID: 10919
	private float _expirationTimeSeconds;

	// Token: 0x04002AA8 RID: 10920
	private float _expirationNextUpateTimeSeconds;

	// Token: 0x04002AA9 RID: 10921
	private bool _fillLock;

	// Token: 0x04002AAA RID: 10922
	private readonly List<LeaderboardItemViewModel> _clansList;

	// Token: 0x04002AAB RID: 10923
	private readonly List<LeaderboardItemViewModel> _friendsList;

	// Token: 0x04002AAC RID: 10924
	private readonly List<LeaderboardItemViewModel> _playersList;

	// Token: 0x04002AAD RID: 10925
	private readonly List<LeaderboardItemViewModel> _tournamentList;

	// Token: 0x04002AAE RID: 10926
	private Vector3? _startScrollPos;

	// Token: 0x04002AAF RID: 10927
	private bool _scrollToPlayerRunningVal;

	// Token: 0x04002AB0 RID: 10928
	[SerializeField]
	private GameObject _viewHandler;

	// Token: 0x04002AB1 RID: 10929
	[SerializeField]
	private PrefabHandler _viewPrefab;

	// Token: 0x04002AB2 RID: 10930
	private LazyObject<LeaderboardsView> _view;

	// Token: 0x04002AB3 RID: 10931
	private UIPanel _panelVal;

	// Token: 0x04002AB4 RID: 10932
	private bool _isInit;

	// Token: 0x04002AB5 RID: 10933
	private Lazy<MainMenuController> _mainMenuController;

	// Token: 0x04002AB6 RID: 10934
	private TaskCompletionSource<bool> _returnPromise = new TaskCompletionSource<bool>();

	// Token: 0x04002AB7 RID: 10935
	private bool _profileIsOpened;

	// Token: 0x04002AB8 RID: 10936
	private static TaskCompletionSource<string> _currentRequestPromise;

	// Token: 0x04002AB9 RID: 10937
	private LeaderboardScript.GridState _state;

	// Token: 0x02000684 RID: 1668
	private enum GridState
	{
		// Token: 0x04002AEA RID: 10986
		Empty,
		// Token: 0x04002AEB RID: 10987
		FillingWithCache,
		// Token: 0x04002AEC RID: 10988
		Cache,
		// Token: 0x04002AED RID: 10989
		FillingWithResponse,
		// Token: 0x04002AEE RID: 10990
		Response
	}
}
