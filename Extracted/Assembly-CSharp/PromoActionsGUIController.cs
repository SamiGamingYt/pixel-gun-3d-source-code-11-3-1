using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class PromoActionsGUIController : MonoBehaviour
{
	// Token: 0x06002A5C RID: 10844 RVA: 0x000E00B8 File Offset: 0x000DE2B8
	private IEnumerator Start()
	{
		PromoActionsManager.ActionsUUpdated += this.UpdateAfterDelayHandler;
		ShopNGUIController.GunBought += this.MarkUpdateOnEnable;
		WeaponManager.TryGunExpired += this.MarkUpdateOnEnable;
		StickersController.onBuyPack += this.MarkUpdateOnEnable;
		yield return null;
		this.initiallyUpdated = true;
		yield break;
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x000E00D4 File Offset: 0x000DE2D4
	private void OnEnable()
	{
		if (this.updateOnEnable || !this.initiallyUpdated)
		{
			base.StartCoroutine(this.UpdateAfterDelay());
		}
		this.updateOnEnable = false;
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x000E010C File Offset: 0x000DE30C
	private void UpdateAfterDelayHandler()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.UpdateAfterDelay());
		}
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x000E0138 File Offset: 0x000DE338
	private IEnumerator UpdateAfterDelay()
	{
		yield return null;
		this.HandleUpdated();
		yield break;
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x000E0154 File Offset: 0x000DE354
	private void OnDestroy()
	{
		PromoActionsManager.ActionsUUpdated -= this.UpdateAfterDelayHandler;
		ShopNGUIController.GunBought -= this.MarkUpdateOnEnable;
		WeaponManager.TryGunExpired -= this.MarkUpdateOnEnable;
		StickersController.onBuyPack -= this.MarkUpdateOnEnable;
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x000E01A8 File Offset: 0x000DE3A8
	private void Update()
	{
		Transform transform = this.wrapContent.transform;
		if (transform.childCount > 0)
		{
			UIPanel component = transform.parent.GetComponent<UIPanel>();
			if (transform.childCount <= 3)
			{
				float num = 0f;
				foreach (object obj in transform)
				{
					Transform transform2 = (Transform)obj;
					num += transform2.localPosition.x;
				}
				num /= (float)transform.childCount;
				if (component != null)
				{
					this.wrapContent.WrapContent();
					component.GetComponent<UIScrollView>().SetDragAmount(0.5f, 0f, false);
				}
			}
			if (this.refreshPromoPanelCntr % 10 == 0)
			{
				component.Refresh();
			}
			this.refreshPromoPanelCntr++;
		}
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x000E02B4 File Offset: 0x000DE4B4
	public void MarkUpdateOnEnable()
	{
		this.updateOnEnable = true;
		if (base.gameObject.activeInHierarchy)
		{
			this.HandleUpdated();
		}
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x000E02D4 File Offset: 0x000DE4D4
	private void HandleUpdated()
	{
		base.StartCoroutine(this.HandleUpdateCoroutine());
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x000E02E4 File Offset: 0x000DE4E4
	public static string FilterForLoadings(string tg, List<string> alreadyUsed)
	{
		if (tg == null || alreadyUsed == null)
		{
			return null;
		}
		string text = WeaponManager.FirstUnboughtTag(tg);
		string key = string.Empty;
		try
		{
			key = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text]];
		}
		catch (Exception arg)
		{
			Debug.Log("Exception in FilterForLoadings:  idefFirstUnobught = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnobught] ]:  " + arg);
			return null;
		}
		if (!Storager.hasKey(key))
		{
			Storager.setInt(key, 0, false);
		}
		bool flag = Storager.getInt(key, true) > 0;
		WeaponSounds weaponSounds = null;
		foreach (GameObject gameObject in WeaponManager.sharedManager.weaponsInGame)
		{
			if (ItemDb.GetByPrefabName(gameObject.name).Tag.Equals(text))
			{
				weaponSounds = gameObject.GetComponent<WeaponSounds>();
				break;
			}
		}
		if (weaponSounds == null)
		{
			return null;
		}
		if (!flag && weaponSounds.tier <= ExpController.Instance.OurTier && !alreadyUsed.Contains(ItemDb.GetByPrefabName(weaponSounds.name.Replace("(Clone)", string.Empty)).Tag))
		{
			return text;
		}
		WeaponSounds weaponSounds2 = weaponSounds;
		if (!flag)
		{
			string text2 = WeaponManager.LastBoughtTag(text, null);
			if (text2 != null)
			{
				List<string> list = null;
				foreach (List<string> list2 in WeaponUpgrades.upgrades)
				{
					if (list2.Contains(text2))
					{
						list = list2;
						break;
					}
				}
				for (int j = list.IndexOf(text2); j >= 0; j--)
				{
					bool flag2 = false;
					foreach (GameObject gameObject2 in WeaponManager.sharedManager.weaponsInGame)
					{
						if (ItemDb.GetByPrefabName(gameObject2.name).Tag.Equals(list[j]))
						{
							WeaponSounds component = gameObject2.GetComponent<WeaponSounds>();
							if (component.tier <= ExpController.Instance.OurTier)
							{
								flag2 = true;
								weaponSounds2 = component;
								break;
							}
						}
					}
					if (flag2)
					{
						break;
					}
				}
			}
		}
		float num = 1f;
		if (weaponSounds2 != null)
		{
			num = weaponSounds2.DamageByTier[weaponSounds2.tier] / weaponSounds2.lengthForShot;
		}
		float initialDPS = num;
		if (flag && weaponSounds.tier > ExpController.Instance.OurTier && weaponSounds2 != null)
		{
			try
			{
				initialDPS = num * (weaponSounds2.DamageByTier[ExpController.Instance.OurTier] / weaponSounds2.DamageByTier[weaponSounds2.tier]);
			}
			catch (Exception arg2)
			{
				Debug.Log("Exception in FilterForLoadings:  if (bought && ws.tier > ExpController.Instance.OurTier && lastBoughtInOurTierWS != null):  " + arg2);
			}
		}
		List<string> list3 = new List<string>
		{
			tg
		};
		foreach (List<string> list4 in WeaponUpgrades.upgrades)
		{
			if (list4.Contains(tg))
			{
				list3 = list4;
				break;
			}
		}
		List<string> list5 = new List<string>();
		List<GameObject> list6 = new List<GameObject>();
		foreach (GameObject gameObject3 in WeaponManager.sharedManager.weaponsInGame)
		{
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(gameObject3.name);
			if (!list3.Contains(byPrefabName.Tag) && gameObject3.GetComponent<WeaponSounds>().tier <= ExpController.Instance.OurTier && !gameObject3.GetComponent<WeaponSounds>().campaignOnly && !gameObject3.name.Equals(WeaponManager.AlienGunWN) && !gameObject3.name.Equals(WeaponManager.BugGunWN) && !gameObject3.name.Equals(WeaponManager.SimpleFlamethrower_WN) && !gameObject3.name.Equals(WeaponManager.CampaignRifle_WN) && !gameObject3.name.Equals(WeaponManager.Rocketnitza_WN) && !gameObject3.name.Equals(WeaponManager.PistolWN) && !gameObject3.name.Equals(WeaponManager.SocialGunWN) && !gameObject3.name.Equals(WeaponManager.DaterFreeWeaponPrefabName) && !gameObject3.name.Equals(WeaponManager.KnifeWN) && !gameObject3.name.Equals(WeaponManager.ShotgunWN) && !gameObject3.name.Equals(WeaponManager.MP5WN) && !ItemDb.IsTemporaryGun(byPrefabName.Tag) && (byPrefabName.Tag == null || !WeaponManager.GotchaGuns.Contains(byPrefabName.Tag)))
			{
				string text3 = WeaponManager.FirstUnboughtTag(byPrefabName.Tag);
				if (!alreadyUsed.Contains(text3) && !list5.Contains(text3))
				{
					bool flag3 = false;
					foreach (GameObject gameObject4 in WeaponManager.sharedManager.weaponsInGame)
					{
						if (ItemDb.GetByPrefabName(gameObject4.name).Tag.Equals(text3))
						{
							flag3 = (gameObject4.GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier);
							break;
						}
					}
					if (!flag3)
					{
						string key2 = string.Empty;
						try
						{
							key2 = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text3]];
							if (Storager.getInt(key2, true) > 0)
							{
								goto IL_654;
							}
						}
						catch (Exception arg3)
						{
							Debug.Log("Exception in FilterForLoadings:  defFirstUnobughtOther = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnboughtOthers] ]:  " + arg3);
						}
						list5.Add(text3);
						foreach (GameObject gameObject5 in WeaponManager.sharedManager.weaponsInGame)
						{
							if (ItemDb.GetByPrefabName(gameObject5.name).Tag.Equals(text3))
							{
								list6.Add(gameObject5);
								break;
							}
						}
					}
				}
			}
			IL_654:;
		}
		list6.Sort(delegate(GameObject go1, GameObject go2)
		{
			float num2 = go1.GetComponent<WeaponSounds>().DamageByTier[go1.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
			float num3 = go2.GetComponent<WeaponSounds>().DamageByTier[go2.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
			return (int)(num2 - num3);
		});
		GameObject gameObject6 = list6.Find(delegate(GameObject obj)
		{
			float num2 = obj.GetComponent<WeaponSounds>().DamageByTier[obj.GetComponent<WeaponSounds>().tier] / obj.GetComponent<WeaponSounds>().lengthForShot;
			return num2 >= initialDPS;
		});
		if (gameObject6 == null)
		{
			gameObject6 = ((list6.Count <= 0) ? null : list6[list6.Count - 1]);
		}
		return (!(gameObject6 != null)) ? null : ItemDb.GetByPrefabName(gameObject6.name).Tag;
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000E0A68 File Offset: 0x000DEC68
	public static List<string> FilterPurchases(IEnumerable<string> input, bool filterNextTierUpgrades = false, bool filterWeapons = true, bool filterRentedTempWeapons = false, bool checkWear = true)
	{
		List<string> list3 = new List<string>();
		Dictionary<string, WeaponSounds> dictionary = (from w in WeaponManager.sharedManager.weaponsInGame
		select ((GameObject)w).GetComponent<WeaponSounds>()).ToDictionary((WeaponSounds ws) => ws.name, (WeaponSounds ws) => ws);
		HashSet<string> hashSet = new HashSet<string>(WeaponManager.sharedManager.FilteredShopListsForPromos.SelectMany((List<GameObject> l) => from g in l
		select g.name.Replace("(Clone)", string.Empty)));
		foreach (string text in input)
		{
			if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(text) || text == "Armor_Novice")
			{
				list3.Add(text);
			}
			else
			{
				ItemRecord byTag = ItemDb.GetByTag(text);
				bool flag = byTag != null && byTag.TemporaryGun;
				bool flag2 = true;
				if ((byTag == null || !hashSet.Contains(byTag.PrefabName)) && WeaponManager.tagToStoreIDMapping.ContainsKey(text))
				{
					flag2 = false;
				}
				if (filterWeapons && (!flag2 || (flag2 && !flag && WeaponManager.tagToStoreIDMapping.ContainsKey(text) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[text]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text]], true) > 0) || (filterRentedTempWeapons && flag2 && flag && TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(text))))
				{
					list3.Add(text);
				}
				bool flag3 = false;
				bool flag4 = false;
				if (checkWear)
				{
					foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
					{
						foreach (List<string> list2 in keyValuePair.Value)
						{
							if (list2.Contains(text))
							{
								flag4 = true;
								if (TempItemsController.PriceCoefs.ContainsKey(text))
								{
									break;
								}
								int num = list2.IndexOf(text);
								bool flag5 = Storager.getInt(text, true) == 0;
								bool flag6 = Wear.TierForWear(text) <= ExpController.OurTierForAnyPlace();
								bool flag7 = Wear.LeagueForWear(text, keyValuePair.Key) <= (int)RatingSystem.instance.currentLeague;
								flag3 = (((num == 0 && flag5 && flag6) || (num > 0 && flag5 && Storager.getInt(list2[num - 1], true) > 0 && (!filterNextTierUpgrades || flag6))) && flag7);
								break;
							}
						}
					}
				}
				if (!flag3 && (SkinsController.skinsNamesForPers.ContainsKey(text) || text.Equals("CustomSkinID")))
				{
					flag4 = true;
					flag3 = false;
				}
				if (flag4 && !flag3 && !TempItemsController.PriceCoefs.ContainsKey(text))
				{
					list3.Add(text);
				}
				if (!(WeaponManager.sharedManager == null) && !(ExpController.Instance == null))
				{
					WeaponSounds weaponSounds;
					if (filterWeapons && byTag != null && dictionary.TryGetValue(byTag.PrefabName, out weaponSounds) && weaponSounds != null)
					{
						if (weaponSounds.tier > ExpController.Instance.OurTier)
						{
							list3.Add(text);
						}
						if (SceneLoader.ActiveSceneName.Equals("Sniper") && (!weaponSounds.IsAvalibleFromFilter(2) || weaponSounds.name == WeaponManager.PistolWN || weaponSounds.name == WeaponManager.KnifeWN))
						{
							list3.Add(text);
						}
						if (SceneLoader.ActiveSceneName.Equals("Knife") && weaponSounds.categoryNabor != 3)
						{
							list3.Add(text);
						}
						if (SceneLoader.ActiveSceneName.Equals("LoveIsland") && !weaponSounds.IsAvalibleFromFilter(3))
						{
							list3.Add(text);
						}
					}
					if (!flag4 && !WeaponManager.tagToStoreIDMapping.ContainsKey(text))
					{
						list3.Add(text);
					}
					if (TempItemsController.PriceCoefs.ContainsKey(text))
					{
						list3.Add(text);
					}
				}
			}
		}
		try
		{
			if (ShopNGUIController.NoviceArmorAvailable)
			{
				list3 = list3.Union(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory].SelectMany((List<string> list) => list)).ToList<string>();
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in FilterPurchases removing all armor: " + arg);
		}
		return list3;
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x000E101C File Offset: 0x000DF21C
	private IEnumerator HandleUpdateCoroutine()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			yield break;
		}
		foreach (PromoActionPreview pa in this.wrapContent.GetComponentsInChildren<PromoActionPreview>(true))
		{
			pa.transform.parent = null;
			UnityEngine.Object.Destroy(pa.gameObject);
		}
		this.wrapContent.SortAlphabetically();
		if (!TrainingController.TrainingCompleted)
		{
			if (this.fonPromoPanel.activeSelf)
			{
				this.fonPromoPanel.SetActive(false);
			}
			yield break;
		}
		List<string> idsToAdd = new List<string>();
		List<string> allNews = PromoActionsManager.sharedManager.news;
		List<string> allTopSellers = PromoActionsManager.sharedManager.topSellers;
		List<string> allDiscounts = PromoActionsManager.sharedManager.discounts.Keys.Union(WeaponManager.sharedManager.TryGunPromos.Keys).ToList<string>();
		List<string> news = allNews.Except(PromoActionsGUIController.FilterPurchases(allNews, false, true, false, true)).Random(5).ToList<string>();
		idsToAdd.AddRange(news);
		List<string> topSellers = allTopSellers.Except(idsToAdd).ToList<string>();
		topSellers = topSellers.Except(PromoActionsGUIController.FilterPurchases(topSellers, false, true, false, true)).Random(2).ToList<string>();
		idsToAdd.AddRange(topSellers);
		List<string> discounts = allDiscounts.Except(idsToAdd).ToList<string>();
		int maxCount = 8 - news.Count;
		discounts = discounts.Except(PromoActionsGUIController.FilterPurchases(discounts, false, true, false, true)).Random(maxCount).ToList<string>();
		idsToAdd.AddRange(discounts);
		Dictionary<string, PromoActionMenu> pas = new Dictionary<string, PromoActionMenu>();
		foreach (string tg in idsToAdd)
		{
			PromoActionMenu pam = new PromoActionMenu
			{
				tg = tg
			};
			if (allNews.Contains(pam.tg))
			{
				pam.isNew = true;
			}
			if (allTopSellers.Contains(pam.tg))
			{
				pam.isTopSeller = true;
			}
			if (allDiscounts.Contains(pam.tg))
			{
				pam.isDiscounted = true;
				try
				{
					bool unused;
					pam.discount = ShopNGUIController.DiscountFor(pam.tg, out unused);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("Exception in pam.discount = ShopNGUIController.DiscountFor(key,out unused): " + e);
				}
				pam.price = ShopNGUIController.GetItemPrice(pam.tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(pam.tg), false, true, false).Price;
			}
			pas.Add(tg, pam);
		}
		string discountLocalize = LocalizationStore.Key_0419;
		foreach (string key in pas.Keys)
		{
			GameObject f = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("PromoAction") as GameObject);
			f.transform.parent = this.wrapContent.transform;
			f.transform.localScale = new Vector3(1f, 1f, 1f);
			PromoActionPreview pap = f.GetComponent<PromoActionPreview>();
			ItemPrice price = null;
			if (!(key == "StickersPromoActionsPanelKey"))
			{
				string shopId = ItemDb.GetShopIdByTag(key) ?? key;
				if (!string.IsNullOrEmpty(shopId))
				{
					price = ItemDb.GetPriceByShopId(shopId, (ShopNGUIController.CategoryNames)(-1));
				}
			}
			if (pas[key].isDiscounted)
			{
				pap.sale.gameObject.SetActive(true);
				pap.Discount = pas[key].discount;
				pap.sale.text = string.Format("{0}\n{1}%", discountLocalize, pas[key].discount);
				pap.coins.text = pas[key].price.ToString();
			}
			else if (price != null)
			{
				pap.coins.text = price.Price.ToString();
			}
			if (price != null)
			{
				pap.currencyImage.spriteName = ((!price.Currency.Equals("Coins")) ? "gem_znachek" : "ingame_coin");
				pap.currencyImage.width = ((!price.Currency.Equals("Coins")) ? 34 : 30);
				pap.currencyImage.height = ((!price.Currency.Equals("Coins")) ? 24 : 30);
				pap.coins.color = ((!price.Currency.Equals("Coins")) ? new Color(0.3176f, 0.8117f, 1f) : new Color(1f, 0.8627f, 0f));
			}
			else
			{
				pap.coins.gameObject.SetActive(false);
				pap.currencyImage.gameObject.SetActive(false);
			}
			if (key == "StickersPromoActionsPanelKey")
			{
				pap.stickersLabel.SetActive(true);
			}
			pap.topSeller.gameObject.SetActive(pas[key].isTopSeller);
			pap.newItem.gameObject.SetActive(pas[key].isNew);
			if (key == "StickersPromoActionsPanelKey")
			{
				pap.button.tweenTarget = pap.stickerTexture.gameObject;
				pap.icon.mainTexture = null;
				pap.icon = pap.stickerTexture;
				pap.pressed = pap.stickerTexture.mainTexture;
				pap.unpressed = pap.stickerTexture.mainTexture;
			}
			else
			{
				int cat = ItemDb.GetItemCategory(key);
				Texture t = ItemDb.GetItemIcon(key, (ShopNGUIController.CategoryNames)cat, null, true);
				if (t != null)
				{
					pap.unpressed = t;
					pap.icon.mainTexture = t;
				}
				if (t != null)
				{
					pap.pressed = t;
				}
			}
			pap.tg = key;
		}
		this.noOffersLabel.gameObject.SetActive(pas.Count == 0 && PromoActionsManager.ActionsAvailable);
		this.checkInternetLabel.gameObject.SetActive(pas.Count == 0 && !PromoActionsManager.ActionsAvailable);
		if (this.fonPromoPanel.activeSelf != (pas.Count != 0))
		{
			this.fonPromoPanel.SetActive(pas.Count != 0);
		}
		yield return null;
		PromoActionPreview[] paps = this.wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
		if (paps == null)
		{
			paps = new PromoActionPreview[0];
		}
		Comparison<PromoActionPreview> comp = delegate(PromoActionPreview pap1, PromoActionPreview pap2)
		{
			int num = 0;
			int num2 = 0;
			if (pap1.tg == "StickersPromoActionsPanelKey")
			{
				num += 1000;
			}
			if (pap2.tg == "StickersPromoActionsPanelKey")
			{
				num2 += 1000;
			}
			if (pap1.newItem.gameObject.activeSelf)
			{
				num += 100;
			}
			if (pap2.newItem.gameObject.activeSelf)
			{
				num2 += 100;
			}
			if (pap1.topSeller.gameObject.activeSelf)
			{
				num += 50;
			}
			if (pap2.topSeller.gameObject.activeSelf)
			{
				num2 += 50;
			}
			if (pap1.sale.gameObject.activeSelf)
			{
				num += 10;
			}
			if (pap2.sale.gameObject.activeSelf)
			{
				num2 += 10;
			}
			return num2 - num;
		};
		Array.Sort<PromoActionPreview>(paps, comp);
		for (int i = 0; i < paps.Length; i++)
		{
			paps[i].gameObject.name = i.ToString("D7");
		}
		this.wrapContent.SortAlphabetically();
		this.wrapContent.WrapContent();
		Transform lookAtTransform = null;
		if (paps.Length > 0)
		{
			lookAtTransform = paps[0].transform;
		}
		if (lookAtTransform != null)
		{
			float x = lookAtTransform.localPosition.x - 9f;
			Transform scrollViewTr = this.wrapContent.transform.parent;
			if (scrollViewTr != null)
			{
				UIPanel scrollViewPanel = scrollViewTr.GetComponent<UIPanel>();
				if (scrollViewPanel != null)
				{
					scrollViewPanel.clipOffset = new Vector2(x, scrollViewPanel.clipOffset.y);
					scrollViewTr.localPosition = new Vector3(-x, scrollViewTr.localPosition.y, scrollViewTr.localPosition.z);
				}
			}
		}
		this.wrapContent.SortAlphabetically();
		this.wrapContent.WrapContent();
		this.wrapContent.transform.parent.GetComponent<UIScrollView>().enabled = (this.wrapContent.transform.childCount <= 0 || this.wrapContent.transform.childCount > 3);
		yield return null;
		this.wrapContent.SortAlphabetically();
		this.wrapContent.WrapContent();
		this.wrapContent.transform.GetComponent<MyCenterOnChild>().Recenter();
		yield break;
	}

	// Token: 0x04001F68 RID: 8040
	public const int ITEMS_COUNT_NEWS = 5;

	// Token: 0x04001F69 RID: 8041
	public const int ITEMS_COUNT_TOPSELLER = 2;

	// Token: 0x04001F6A RID: 8042
	public const int ITEMS_COUNT_DISCOUNT = 3;

	// Token: 0x04001F6B RID: 8043
	public const string StickersPromoActionsPanelKey = "StickersPromoActionsPanelKey";

	// Token: 0x04001F6C RID: 8044
	public UIWrapContent wrapContent;

	// Token: 0x04001F6D RID: 8045
	public UILabel noOffersLabel;

	// Token: 0x04001F6E RID: 8046
	public UILabel checkInternetLabel;

	// Token: 0x04001F6F RID: 8047
	public GameObject fonPromoPanel;

	// Token: 0x04001F70 RID: 8048
	private bool initiallyUpdated;

	// Token: 0x04001F71 RID: 8049
	private bool updateOnEnable;

	// Token: 0x04001F72 RID: 8050
	private int refreshPromoPanelCntr;
}
