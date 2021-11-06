using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x02000689 RID: 1673
internal sealed class LeaderboardsExample : MonoBehaviour
{
	// Token: 0x06003A85 RID: 14981 RVA: 0x0012E89C File Offset: 0x0012CA9C
	private void Start()
	{
		base.StartCoroutine(this.PopulateLeaderboards());
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x0012E8AC File Offset: 0x0012CAAC
	private IEnumerator PopulateLeaderboards()
	{
		if (this.menuLeaderboardsView == null)
		{
			Debug.LogError("menuLeaderboardsView == null");
			yield break;
		}
		yield return null;
		System.Random prng = new System.Random();
		IList<LeaderboardItemViewModel> bestPlayersList = new List<LeaderboardItemViewModel>();
		for (int i = 0; i != 42; i++)
		{
			LeaderboardItemViewModel vm = new LeaderboardItemViewModel
			{
				Rank = (1000 - i) % 13,
				Nickname = "Player_" + prng.Next(100),
				WinCount = prng.Next(1000),
				Place = i + 1,
				Highlight = (i > 11 && i % 7 == 3)
			};
			bestPlayersList.Add(vm);
		}
		for (int j = bestPlayersList.Count; j < MenuLeaderboardsView.PageSize; j++)
		{
			bestPlayersList.Add(LeaderboardItemViewModel.Empty);
		}
		this.menuLeaderboardsView.BestPlayersList = bestPlayersList;
		IList<LeaderboardItemViewModel> friendsList = new List<LeaderboardItemViewModel>();
		for (int k = 0; k != 7; k++)
		{
			LeaderboardItemViewModel vm2 = new LeaderboardItemViewModel
			{
				Rank = (100 - k) % 13,
				Nickname = "Player_" + k * 13 + 2,
				WinCount = 190 + 3 * k,
				Place = 5 * k + 3,
				Highlight = (k % 6 == 2)
			};
			friendsList.Add(vm2);
		}
		for (int l = friendsList.Count; l < MenuLeaderboardsView.PageSize; l++)
		{
			friendsList.Add(LeaderboardItemViewModel.Empty);
		}
		this.menuLeaderboardsView.FriendsList = friendsList;
		yield break;
	}

	// Token: 0x04002B0D RID: 11021
	public LeaderboardsView leaderboardsView;

	// Token: 0x04002B0E RID: 11022
	public MenuLeaderboardsView menuLeaderboardsView;
}
