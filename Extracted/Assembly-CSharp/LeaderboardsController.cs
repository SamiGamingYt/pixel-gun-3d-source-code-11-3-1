using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000688 RID: 1672
internal sealed class LeaderboardsController : MonoBehaviour
{
	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06003A79 RID: 14969 RVA: 0x0012E43C File Offset: 0x0012C63C
	// (set) Token: 0x06003A7A RID: 14970 RVA: 0x0012E444 File Offset: 0x0012C644
	public LeaderboardsView LeaderboardsView
	{
		private get
		{
			return this._leaderboardsView;
		}
		set
		{
			this._leaderboardsView = value;
		}
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06003A7B RID: 14971 RVA: 0x0012E450 File Offset: 0x0012C650
	// (set) Token: 0x06003A7C RID: 14972 RVA: 0x0012E458 File Offset: 0x0012C658
	public FriendsGUIController FriendsGuiController
	{
		private get
		{
			return this._friendsGuiController;
		}
		set
		{
			this._friendsGuiController = value;
		}
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x0012E464 File Offset: 0x0012C664
	public void RequestLeaderboards()
	{
		if (!string.IsNullOrEmpty(this._playerId))
		{
			base.StartCoroutine(this.GetLeaderboardsCoroutine(this._playerId));
		}
		else
		{
			Debug.Log("Player id should not be empty.");
		}
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06003A7E RID: 14974 RVA: 0x0012E4A4 File Offset: 0x0012C6A4
	// (set) Token: 0x06003A7F RID: 14975 RVA: 0x0012E4AC File Offset: 0x0012C6AC
	public string PlayerId
	{
		private get
		{
			return this._playerId;
		}
		set
		{
			this._playerId = (value ?? string.Empty);
		}
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0012E4C4 File Offset: 0x0012C6C4
	internal static List<LeaderboardItemViewModel> ParseLeaderboardEntries(string entryId, string leaderboardName, Dictionary<string, object> response)
	{
		if (string.IsNullOrEmpty(leaderboardName))
		{
			throw new ArgumentException("Leaderbord should not be empty.", "leaderboardName");
		}
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		object obj;
		if (response.TryGetValue(leaderboardName, out obj))
		{
			List<object> list2 = obj as List<object>;
			if (list2 != null)
			{
				IEnumerable<Dictionary<string, object>> enumerable = list2.OfType<Dictionary<string, object>>();
				foreach (Dictionary<string, object> dictionary in enumerable)
				{
					LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel();
					object value;
					if (dictionary.TryGetValue("id", out value))
					{
						leaderboardItemViewModel.Id = Convert.ToString(value);
						leaderboardItemViewModel.Highlight = (!string.IsNullOrEmpty(leaderboardItemViewModel.Id) && leaderboardItemViewModel.Id.Equals(entryId));
					}
					object obj2;
					int rank;
					if (dictionary.TryGetValue("rank", out obj2) && int.TryParse(obj2 as string, out rank))
					{
						leaderboardItemViewModel.Rank = rank;
					}
					else if (dictionary.TryGetValue("member_cnt", out obj2))
					{
						try
						{
							leaderboardItemViewModel.Rank = Convert.ToInt32(obj2);
						}
						catch (Exception exception)
						{
							Debug.LogException(exception);
						}
					}
					object obj3;
					if (dictionary.TryGetValue("nick", out obj3))
					{
						leaderboardItemViewModel.Nickname = ((obj3 as string) ?? string.Empty);
					}
					else if (dictionary.TryGetValue("name", out obj3))
					{
						leaderboardItemViewModel.Nickname = ((obj3 as string) ?? string.Empty);
					}
					try
					{
						object value2;
						if (dictionary.TryGetValue("trophies", out value2))
						{
							leaderboardItemViewModel.WinCount = Convert.ToInt32(value2);
						}
						else if (dictionary.TryGetValue("wins", out value2))
						{
							leaderboardItemViewModel.WinCount = Convert.ToInt32(value2);
						}
						else if (dictionary.TryGetValue("win", out value2))
						{
							leaderboardItemViewModel.WinCount = Convert.ToInt32(value2);
						}
					}
					catch (Exception exception2)
					{
						Debug.LogException(exception2);
					}
					object obj4;
					if (dictionary.TryGetValue("logo", out obj4))
					{
						leaderboardItemViewModel.ClanLogo = ((obj4 as string) ?? string.Empty);
					}
					else
					{
						leaderboardItemViewModel.ClanLogo = string.Empty;
					}
					object obj5;
					if (dictionary.TryGetValue("name", out obj5))
					{
						leaderboardItemViewModel.ClanName = ((obj5 as string) ?? string.Empty);
					}
					else if (dictionary.TryGetValue("clan_name", out obj5))
					{
						leaderboardItemViewModel.ClanName = ((obj5 as string) ?? string.Empty);
					}
					else
					{
						leaderboardItemViewModel.ClanName = string.Empty;
					}
					list.Add(leaderboardItemViewModel);
				}
			}
		}
		return list;
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x0012E7F4 File Offset: 0x0012C9F4
	private void Start()
	{
		this.RequestLeaderboards();
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0012E7FC File Offset: 0x0012C9FC
	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		Debug.Log("LeaderboardsController.GetLeaderboardsCoroutine(" + playerId + ")");
		WWWForm form = new WWWForm();
		form.AddField("action", "get_leaderboards_league");
		form.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
		form.AddField("id", playerId);
		form.AddField("league_id", LeaderboardScript.GetLeagueId());
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_leaderboards_league", null));
		if (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
		{
			Debug.Log("Waiting previous leaderboards request...");
			while (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
			{
				yield return null;
			}
		}
		FriendsController.sharedController.NumberOfBestPlayersRequests++;
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		yield return request;
		FriendsController.sharedController.NumberOfBestPlayersRequests--;
		this.HandleRequestCompleted(request);
		yield break;
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x0012E828 File Offset: 0x0012CA28
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
		LeaderboardScript.Instance.FillGrids(text);
	}

	// Token: 0x04002B0A RID: 11018
	private LeaderboardsView _leaderboardsView;

	// Token: 0x04002B0B RID: 11019
	private FriendsGUIController _friendsGuiController;

	// Token: 0x04002B0C RID: 11020
	private string _playerId = string.Empty;
}
