using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000709 RID: 1801
[RequireComponent(typeof(UIGrid))]
public class LeagueItemsView : MonoBehaviour
{
	// Token: 0x06003E93 RID: 16019 RVA: 0x0014F52C File Offset: 0x0014D72C
	private void Awake()
	{
		this._grid = base.GetComponent<UIGrid>();
		this._slots = base.GetComponentsInChildren<LeagueItemStot>(true);
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0014F548 File Offset: 0x0014D748
	public void Repaint(RatingSystem.RatingLeague league)
	{
		int num = 0;
		List<WeaponSkin> list = WeaponSkinsManager.SkinsForLeague(league);
		foreach (WeaponSkin weaponSkin in list)
		{
			LeagueItemStot leagueItemStot = this._slots[num];
			Texture itemIcon = ItemDb.GetItemIcon(weaponSkin.Id, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory, null, true);
			leagueItemStot.Set(itemIcon, RatingSystem.instance.currentLeague >= league, WeaponSkinsManager.IsBoughtSkin(weaponSkin.Id));
			num++;
		}
		List<string> list2 = Wear.LeagueItemsByLeagues()[league];
		foreach (string text in list2)
		{
			LeagueItemStot leagueItemStot2 = this._slots[num];
			Texture itemIcon2 = ItemDb.GetItemIcon(text, ShopNGUIController.CategoryNames.HatsCategory, null, true);
			List<Wear.LeagueItemState> statesForItem = this.GetStatesForItem(text);
			leagueItemStot2.Set(itemIcon2, statesForItem.Contains(Wear.LeagueItemState.Open), statesForItem.Contains(Wear.LeagueItemState.Purchased));
			num++;
		}
		List<SkinItem> list3 = (from kvp in SkinsController.sharedController.skinItemsDict
		where kvp.Value.currentLeague == league
		select kvp.Value).ToList<SkinItem>();
		foreach (SkinItem skinItem in list3)
		{
			LeagueItemStot leagueItemStot3 = this._slots[num];
			Texture texture = Resources.Load<Texture>(string.Format("LeagueSkinsProfileImages/league{0}_skin_profile", (int)(league + 1)));
			bool flag = false;
			leagueItemStot3.Set(texture, RatingSystem.instance.currentLeague >= league, SkinsController.IsSkinBought(skinItem.name, out flag));
			num++;
		}
		this._headerText.gameObject.SetActive(list2.Any<string>());
		this._grid.Reposition();
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0014F7CC File Offset: 0x0014D9CC
	private List<Wear.LeagueItemState> GetStatesForItem(string itemId)
	{
		List<Wear.LeagueItemState> res = new List<Wear.LeagueItemState>();
		Dictionary<Wear.LeagueItemState, List<string>> items = Wear.LeagueItems();
		RiliExtensions.ForEachEnum<Wear.LeagueItemState>(delegate(Wear.LeagueItemState val)
		{
			if (items[val].Contains(itemId))
			{
				res.Add(val);
			}
		});
		return res;
	}

	// Token: 0x04002E34 RID: 11828
	[SerializeField]
	private UILabel _headerText;

	// Token: 0x04002E35 RID: 11829
	private UIGrid _grid;

	// Token: 0x04002E36 RID: 11830
	private LeagueItemStot[] _slots;
}
