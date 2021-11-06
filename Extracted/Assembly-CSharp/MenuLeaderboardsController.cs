using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200068C RID: 1676
internal sealed class MenuLeaderboardsController : MonoBehaviour
{
	// Token: 0x06003AA1 RID: 15009 RVA: 0x0012F2DC File Offset: 0x0012D4DC
	public void RefreshWithCache()
	{
		if (PlayerPrefs.HasKey("MenuLeaderboardsFriendsCache"))
		{
			string @string = PlayerPrefs.GetString("MenuLeaderboardsFriendsCache");
			this.FillListsWithResponseText(@string);
		}
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x0012F30C File Offset: 0x0012D50C
	private IEnumerator Start()
	{
		MenuLeaderboardsController.sharedController = this;
		using (new StopwatchLogger("MenuLeaderboardsController.Start()"))
		{
			this._menuLeaderboardsView = base.GetComponent<MenuLeaderboardsView>();
			this._playerId = Storager.getString("AccountCreated", false);
			if (PlayerPrefs.HasKey("MenuLeaderboardsFriendsCache"))
			{
				string responseText = PlayerPrefs.GetString("MenuLeaderboardsFriendsCache");
				foreach (float num in this.FillListsWithResponseTextAsync(responseText))
				{
					float _ = num;
					yield return null;
				}
			}
			else
			{
				this.TransitToFallbackState();
			}
		}
		yield break;
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x0012F328 File Offset: 0x0012D528
	private void OnDestroy()
	{
		MenuLeaderboardsController.sharedController = null;
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x0012F330 File Offset: 0x0012D530
	private void TransitToFallbackState()
	{
		LeaderboardItemViewModel item = new LeaderboardItemViewModel
		{
			Id = this._playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			WinCount = RatingSystem.instance.currentRating,
			Place = 1,
			Highlight = true
		};
		IList<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>(MenuLeaderboardsView.PageSize)
		{
			item
		};
		for (int i = 0; i < MenuLeaderboardsView.PageSize - 1; i++)
		{
			list.Add(LeaderboardItemViewModel.Empty);
		}
		this._menuLeaderboardsView.FriendsList = list;
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x0012F3C0 File Offset: 0x0012D5C0
	private void FillListsWithResponseText(string responseText)
	{
		foreach (float num in this.FillListsWithResponseTextAsync(responseText))
		{
			float num2 = num;
		}
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x0012F420 File Offset: 0x0012D620
	private IEnumerable<float> FillListsWithResponseTextAsync(string responseText)
	{
		Dictionary<string, object> response = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (response == null)
		{
			Debug.LogWarning("Leaderboards response is ill-formed.");
			yield break;
		}
		if (!response.Any<KeyValuePair<string, object>>())
		{
			Debug.LogWarning("Leaderboards response contains no elements.");
			yield break;
		}
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Menu Leaderboards response:    " + responseText);
		}
		LeaderboardItemViewModel selfStats = new LeaderboardItemViewModel
		{
			Id = this._playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			WinCount = RatingSystem.instance.currentRating,
			Highlight = true
		};
		Func<IList<LeaderboardItemViewModel>, IList<LeaderboardItemViewModel>> groupAndOrder = delegate(IList<LeaderboardItemViewModel> items)
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
		};
		if (string.IsNullOrEmpty(this._playerId))
		{
			Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		List<LeaderboardItemViewModel> rawFriendsList = LeaderboardsController.ParseLeaderboardEntries(this._playerId, "friends", response);
		rawFriendsList.Add(selfStats);
		IList<LeaderboardItemViewModel> orderedFriendsList = groupAndOrder(rawFriendsList);
		for (int i = orderedFriendsList.Count; i < MenuLeaderboardsView.PageSize; i++)
		{
			orderedFriendsList.Add(LeaderboardItemViewModel.Empty);
		}
		yield return 0.2f;
		List<LeaderboardItemViewModel> rawBestPlayersList = LeaderboardsController.ParseLeaderboardEntries(this._playerId, "best_players", response);
		IList<LeaderboardItemViewModel> orderedBestPlayersList = groupAndOrder(rawBestPlayersList);
		for (int j = orderedBestPlayersList.Count; j < MenuLeaderboardsView.PageSize; j++)
		{
			orderedBestPlayersList.Add(LeaderboardItemViewModel.Empty);
		}
		yield return 0.4f;
		List<LeaderboardItemViewModel> rawClansList = LeaderboardsController.ParseLeaderboardEntries(FriendsController.sharedController.ClanID, "top_clans", response);
		IList<LeaderboardItemViewModel> orderedClansList = groupAndOrder(rawClansList);
		for (int k = orderedClansList.Count; k < MenuLeaderboardsView.PageSize; k++)
		{
			orderedClansList.Add(LeaderboardItemViewModel.Empty);
		}
		yield return 0.6f;
		if (this._menuLeaderboardsView != null)
		{
			LeaderboardItemViewModel selfInTop = orderedBestPlayersList.FirstOrDefault((LeaderboardItemViewModel item) => item.Id == this._playerId);
			bool weAreInTop = selfInTop != null;
			if (Application.isEditor)
			{
				Debug.Log("We are in top: " + weAreInTop);
			}
			this._menuLeaderboardsView.FriendsList = orderedFriendsList;
			this._menuLeaderboardsView.BestPlayersList = orderedBestPlayersList;
			this._menuLeaderboardsView.ClansList = orderedClansList;
			this._menuLeaderboardsView.SelfStats = ((!weAreInTop) ? MenuLeaderboardsController.FulfillSelfStats(selfStats, response) : LeaderboardItemViewModel.Empty);
			object myClanObject;
			if (response.TryGetValue("my_clan", out myClanObject))
			{
				Dictionary<string, object> myClanDictionary = myClanObject as Dictionary<string, object>;
				if (Application.isEditor)
				{
					Debug.Log("My Clan: " + Json.Serialize(myClanObject));
				}
				if (myClanDictionary == null)
				{
					Debug.Log("myClanDictionary == null    Result type: " + myClanObject.GetType());
				}
				else
				{
					bool myClanInTop = myClanDictionary.ContainsKey("place");
					LeaderboardItemViewModel selfClanStats;
					if (myClanInTop)
					{
						selfClanStats = LeaderboardItemViewModel.Empty;
					}
					else
					{
						selfClanStats = new LeaderboardItemViewModel
						{
							Id = (FriendsController.sharedController.ClanID ?? string.Empty),
							Nickname = (FriendsController.sharedController.clanName ?? string.Empty),
							WinCount = int.MinValue,
							Place = int.MinValue,
							Highlight = true
						};
						object clanNameObject;
						if (myClanDictionary.TryGetValue("name", out clanNameObject))
						{
							selfClanStats.Nickname = Convert.ToString(clanNameObject);
						}
						object clanPlace;
						if (myClanDictionary.TryGetValue("place", out clanPlace))
						{
							selfClanStats.Place = Convert.ToInt32(clanPlace);
						}
						object clanWinCount;
						if (myClanDictionary.TryGetValue("wins", out clanWinCount))
						{
							selfClanStats.WinCount = Convert.ToInt32(clanWinCount);
						}
					}
					this._menuLeaderboardsView.SelfClanStats = selfClanStats;
				}
			}
			else
			{
				this._menuLeaderboardsView.SelfClanStats = LeaderboardItemViewModel.Empty;
			}
		}
		else
		{
			Debug.LogError("_menuLeaderboardsView == null");
		}
		yield return 1f;
		yield break;
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x0012F454 File Offset: 0x0012D654
	private static LeaderboardItemViewModel FulfillSelfStats(LeaderboardItemViewModel selfStats, Dictionary<string, object> response)
	{
		LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel
		{
			Id = selfStats.Id,
			Nickname = selfStats.Nickname,
			WinCount = selfStats.WinCount,
			Place = int.MinValue,
			Highlight = true
		};
		object obj;
		if (response.TryGetValue("me", out obj))
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				try
				{
					leaderboardItemViewModel.WinCount = Convert.ToInt32(dictionary["wins"]);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}
		return leaderboardItemViewModel;
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x0012F50C File Offset: 0x0012D70C
	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		Debug.Log("MenuLeaderboardsController.GetLeaderboardsCoroutine(" + playerId + ")");
		WWWForm form = new WWWForm();
		form.AddField("action", "get_menu_leaderboards");
		form.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
		form.AddField("id", playerId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_menu_leaderboards", null));
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.NumberOfBestPlayersRequests++;
		}
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		yield return request;
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.NumberOfBestPlayersRequests--;
		}
		this.HandleRequestCompleted(request);
		yield break;
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x0012F538 File Offset: 0x0012D738
	private void HandleRequestCompleted(WWW request)
	{
		if (Application.isEditor)
		{
			Debug.Log("HandleRequestCompleted()");
		}
		if (request == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogWarning(request.error);
			return;
		}
		string text = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogWarning("Leaderboars response is empty.");
			return;
		}
		PlayerPrefs.SetString("MenuLeaderboardsFriendsCache", text);
		LeaderboardScript.Instance.FillGrids(text);
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06003AAA RID: 15018 RVA: 0x0012F5B0 File Offset: 0x0012D7B0
	public bool IsOpened
	{
		get
		{
			return this.menuLeaderboardsView.opened.activeSelf;
		}
	}

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x06003AAB RID: 15019 RVA: 0x0012F5C4 File Offset: 0x0012D7C4
	public MenuLeaderboardsView menuLeaderboardsView
	{
		get
		{
			return this._menuLeaderboardsView;
		}
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x0012F5CC File Offset: 0x0012D7CC
	public void OnBtnLeaderboardsOnClick()
	{
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x0012F5D0 File Offset: 0x0012D7D0
	public void OnBtnLeaderboardsOffClick()
	{
	}

	// Token: 0x04002B40 RID: 11072
	private const string MenuLeaderboardsResponseCache = "MenuLeaderboardsFriendsCache";

	// Token: 0x04002B41 RID: 11073
	internal const bool NewLeaderboards = true;

	// Token: 0x04002B42 RID: 11074
	public static MenuLeaderboardsController sharedController;

	// Token: 0x04002B43 RID: 11075
	private MenuLeaderboardsView _menuLeaderboardsView;

	// Token: 0x04002B44 RID: 11076
	private string _playerId = string.Empty;
}
