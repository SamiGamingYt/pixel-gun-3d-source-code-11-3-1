using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020007AC RID: 1964
public class ShopNGUIController : MonoBehaviour
{
	// Token: 0x06004603 RID: 17923 RVA: 0x0017A6C0 File Offset: 0x001788C0
	public ShopNGUIController()
	{
		this.itemIndexInCarousel = -1;
		base..ctor();
	}

	// Token: 0x140000A3 RID: 163
	// (add) Token: 0x06004605 RID: 17925 RVA: 0x0017AAF0 File Offset: 0x00178CF0
	// (remove) Token: 0x06004606 RID: 17926 RVA: 0x0017AB08 File Offset: 0x00178D08
	public static event Action<string, ShopNGUIController.CategoryNames, string> EquippedWearInCategory;

	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x06004607 RID: 17927 RVA: 0x0017AB20 File Offset: 0x00178D20
	// (remove) Token: 0x06004608 RID: 17928 RVA: 0x0017AB38 File Offset: 0x00178D38
	public static event Action<ShopNGUIController.CategoryNames, string> UnequippedWearInCategory;

	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x06004609 RID: 17929 RVA: 0x0017AB50 File Offset: 0x00178D50
	// (remove) Token: 0x0600460A RID: 17930 RVA: 0x0017AB68 File Offset: 0x00178D68
	public static event Action ShowArmorChanged;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x0600460B RID: 17931 RVA: 0x0017AB80 File Offset: 0x00178D80
	// (remove) Token: 0x0600460C RID: 17932 RVA: 0x0017AB98 File Offset: 0x00178D98
	public static event Action ShowWearChanged;

	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x0600460D RID: 17933 RVA: 0x0017ABB0 File Offset: 0x00178DB0
	// (remove) Token: 0x0600460E RID: 17934 RVA: 0x0017ABC8 File Offset: 0x00178DC8
	public static event Action<bool> ShopChangedIsActive;

	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x0600460F RID: 17935 RVA: 0x0017ABE0 File Offset: 0x00178DE0
	// (remove) Token: 0x06004610 RID: 17936 RVA: 0x0017ABF8 File Offset: 0x00178DF8
	public static event Action GunOrArmorBought;

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06004611 RID: 17937 RVA: 0x0017AC10 File Offset: 0x00178E10
	// (remove) Token: 0x06004612 RID: 17938 RVA: 0x0017AC28 File Offset: 0x00178E28
	public static event Action<string> TryGunBought;

	// Token: 0x140000AA RID: 170
	// (add) Token: 0x06004613 RID: 17939 RVA: 0x0017AC40 File Offset: 0x00178E40
	// (remove) Token: 0x06004614 RID: 17940 RVA: 0x0017AC58 File Offset: 0x00178E58
	public static event Action<string, string, GadgetInfo.GadgetCategory> EquippedGadget;

	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06004615 RID: 17941 RVA: 0x0017AC70 File Offset: 0x00178E70
	// (remove) Token: 0x06004616 RID: 17942 RVA: 0x0017AC88 File Offset: 0x00178E88
	public static event Action<string, string> EquippedPet;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06004617 RID: 17943 RVA: 0x0017ACA0 File Offset: 0x00178EA0
	// (remove) Token: 0x06004618 RID: 17944 RVA: 0x0017ACB8 File Offset: 0x00178EB8
	public static event Action<string> UnequippedPet;

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x06004619 RID: 17945 RVA: 0x0017ACD0 File Offset: 0x00178ED0
	// (remove) Token: 0x0600461A RID: 17946 RVA: 0x0017ACE8 File Offset: 0x00178EE8
	public static event Action GunBought;

	// Token: 0x0600461B RID: 17947 RVA: 0x0017AD00 File Offset: 0x00178F00
	internal void ReloadCarousel(ShopNGUIController.ShopItem item)
	{
		ShopCarouselElement[] componentsInChildren = this.wrapContent.GetComponentsInChildren<ShopCarouselElement>(true);
		foreach (ShopCarouselElement shopCarouselElement in componentsInChildren)
		{
			UnityEngine.Object.Destroy(shopCarouselElement.gameObject);
			shopCarouselElement.transform.parent = null;
		}
		this.wrapContent.Reposition();
		List<GameObject> modelsList = this.GetModelsList(this.CurrentCategory);
		if (item == null)
		{
			item = this.CurrentItem;
		}
		if (item != null)
		{
			int num = modelsList.FindIndex((GameObject go) => go.nameNoClone() == item.Id);
		}
		for (int j = 0; j < modelsList.Count; j++)
		{
			GameObject original = Resources.Load<GameObject>("ShopCarouselElement");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = this.wrapContent.transform;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = Vector3.zero;
			GameObject gameObject2 = modelsList[j];
			gameObject.name = j.ToString("D7");
			ShopCarouselElement sce = gameObject.GetComponent<ShopCarouselElement>();
			string name = gameObject2.name;
			sce.itemID = name;
			sce.SetupPriceAndDiscount(sce.itemID, ShopNGUIController.CategoryNames.ArmorCategory);
			Action<GameObject, ShopNGUIController.CategoryNames> action = delegate(GameObject loadedOBject, ShopNGUIController.CategoryNames category)
			{
				ShopNGUIController.AddModel(loadedOBject, delegate(GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float scaleCoefShop, int tier, int league)
				{
					if (sce == null)
					{
						UnityEngine.Object.Destroy(manipulateObject);
						return;
					}
					sce.readableName = (readableName ?? string.Empty);
					manipulateObject.transform.parent = sce.transform;
					sce.baseScale = new Vector3(scaleCoefShop, scaleCoefShop, scaleCoefShop);
					sce.model = manipulateObject.transform;
					sce.ourPosition = positionShop;
					sce.SetPos((!this.EnableConfigurePos) ? 0f : 1f, 0f);
					sce.model.Rotate(rotationShop, Space.World);
					if (ExpController.Instance != null && ExpController.Instance.OurTier < tier && tier < 100 && sce.itemID == WeaponManager.FirstUnboughtTag(sce.itemID) && sce.itemID != "cape_Custom" && sce.itemID != "boots_tabi" && sce.locked != null)
					{
						sce.locked.SetActive(true);
					}
					if (sce.itemID != WeaponManager.FirstUnboughtTag(sce.itemID))
					{
						if (sce.arrow != null)
						{
							sce.arrow.gameObject.SetActive(true);
						}
						Dictionary<ShopNGUIController.CategoryNames, float> dictionary = new Dictionary<ShopNGUIController.CategoryNames, float>(5, ShopNGUIController.CategoryNameComparer.Instance)
						{
							{
								ShopNGUIController.CategoryNames.HatsCategory,
								85f
							},
							{
								ShopNGUIController.CategoryNames.ArmorCategory,
								105f
							},
							{
								ShopNGUIController.CategoryNames.CapesCategory,
								75f
							},
							{
								ShopNGUIController.CategoryNames.BootsCategory,
								81f
							},
							{
								ShopNGUIController.CategoryNames.MaskCategory,
								75f
							}
						};
						if (dictionary.ContainsKey(ShopNGUIController.CategoryNames.ArmorCategory))
						{
							sce.arrnoInitialPos = new Vector3(dictionary[ShopNGUIController.CategoryNames.ArmorCategory], sce.arrnoInitialPos.y, sce.arrnoInitialPos.z);
						}
					}
				}, ShopNGUIController.MapShopCategoryToItemCategory(category), false, null);
			};
			action(ItemDb.GetWearFromResources(gameObject2.nameNoClone(), ShopNGUIController.CategoryNames.ArmorCategory), ShopNGUIController.CategoryNames.ArmorCategory);
		}
		this.wrapContent.Reposition();
		this.ChooseCarouselItem(item, true);
	}

	// Token: 0x0600461C RID: 17948 RVA: 0x0017AEE4 File Offset: 0x001790E4
	public void ChooseCarouselItem(ShopNGUIController.ShopItem itemToSet, bool moveCarousel = false)
	{
		if (itemToSet == null)
		{
			return;
		}
		ShopCarouselElement[] array = this.wrapContent.GetComponentsInChildren<ShopCarouselElement>(true);
		if (array == null)
		{
			array = new ShopCarouselElement[0];
		}
		foreach (ShopCarouselElement shopCarouselElement in array)
		{
			if (shopCarouselElement.itemID.Equals(itemToSet.Id))
			{
				if (moveCarousel)
				{
					SpringPanel component = this.scrollViewPanel.GetComponent<SpringPanel>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
					if (this.scrollViewPanel.gameObject.activeInHierarchy)
					{
						this.scrollViewPanel.GetComponent<UIScrollView>().MoveRelative(new Vector3(-shopCarouselElement.transform.localPosition.x - this.scrollViewPanel.transform.localPosition.x, this.scrollViewPanel.transform.localPosition.y, this.scrollViewPanel.transform.localPosition.z));
					}
					this.wrapContent.Reposition();
				}
				this.CurrentItem = itemToSet;
				this.UpdatePersWithNewItem(this.CurrentItem);
				this.UpdateButtons();
				this.caption.text = (shopCarouselElement.readableName ?? string.Empty);
				foreach (UILabel uilabel in this.armorNameLabels)
				{
					uilabel.text = ItemDb.GetItemNameByTag(this.CurrentItem.Id);
				}
				break;
			}
		}
	}

	// Token: 0x0600461D RID: 17949 RVA: 0x0017B0A8 File Offset: 0x001792A8
	private void HandleCarouselCentering()
	{
		this.HandleCarouselCentering(this.carouselCenter.centeredObject);
	}

	// Token: 0x0600461E RID: 17950 RVA: 0x0017B0BC File Offset: 0x001792BC
	private void HandleCarouselCentering(GameObject centeredObj)
	{
		if (centeredObj != null && centeredObj != this._lastSelectedItem)
		{
			this._lastSelectedItem = centeredObj;
			ShopCarouselElement component = centeredObj.GetComponent<ShopCarouselElement>();
			this.ChooseCarouselItem(new ShopNGUIController.ShopItem(component.itemID, ShopNGUIController.CategoryNames.ArmorCategory), false);
		}
		if (this.EnableConfigurePos && centeredObj != null)
		{
			centeredObj.GetComponent<ShopCarouselElement>().SetPos(1f, 0f);
		}
	}

	// Token: 0x0600461F RID: 17951 RVA: 0x0017B134 File Offset: 0x00179334
	private void CheckCenterItemChanging()
	{
		if (this.CurrentCategory != ShopNGUIController.CategoryNames.ArmorCategory)
		{
			return;
		}
		if (!this.scrollViewPanel.cachedGameObject.activeInHierarchy)
		{
			return;
		}
		Transform cachedTransform = this.scrollViewPanel.cachedTransform;
		this.itemIndexInCarousel = -1;
		int num = (int)this.wrapContent.cellWidth;
		int childCount = this.wrapContent.transform.childCount;
		if (cachedTransform.localPosition.x > 0f)
		{
			this.itemIndexInCarousel = 0;
		}
		else if (cachedTransform.localPosition.x < (float)(-1 * num * childCount))
		{
			this.itemIndexInCarousel = childCount - 1;
		}
		else
		{
			this.itemIndexInCarousel = -1 * Mathf.RoundToInt((cachedTransform.localPosition.x - (float)(Mathf.CeilToInt(cachedTransform.localPosition.x / (float)num / (float)childCount) * num * childCount)) / (float)num);
		}
		this.itemIndexInCarousel = Mathf.Clamp(this.itemIndexInCarousel, 0, childCount - 1);
		if (this.itemIndexInCarousel >= 0 && this.itemIndexInCarousel < this.wrapContent.transform.childCount)
		{
			GameObject gameObject = this.wrapContent.transform.GetChild(this.itemIndexInCarousel).gameObject;
			if (!this.EnableConfigurePos)
			{
				this.HandleCarouselCentering(gameObject);
			}
		}
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x0017B28C File Offset: 0x0017948C
	private List<GameObject> GetModelsList(ShopNGUIController.CategoryNames category)
	{
		List<GameObject> list;
		Func<ShopNGUIController.CategoryNames, Comparison<GameObject>> func = (ShopNGUIController.CategoryNames cn) => delegate(GameObject lh, GameObject rh)
		{
			List<string> list2 = Wear.wear[cn].FirstOrDefault((List<string> list) => list.Contains(lh.name));
			List<string> list3 = Wear.wear[cn].FirstOrDefault((List<string> list) => list.Contains(rh.name));
			if (list2 == null || list3 == null)
			{
				return 0;
			}
			if (list2 == list3)
			{
				return list2.IndexOf(lh.name) - list2.IndexOf(rh.name);
			}
			return Wear.wear[cn].IndexOf(list2) - Wear.wear[cn].IndexOf(list3);
		};
		list = new List<GameObject>();
		if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1 && !TrainingController.TrainingCompleted)
		{
			GameObject gameObject = Resources.Load<GameObject>("Armor_Info/Armor_Novice");
			if (gameObject != null)
			{
				list.Add(gameObject);
			}
			else
			{
				Debug.LogError("No novice armor when Storager.getInt(Defs.NoviceArmorUsedKey,false) == 1 && !TrainingController.TrainingCompleted");
			}
		}
		else
		{
			foreach (ShopPositionParams shopPositionParams in ShopNGUIController.armor)
			{
				this.FilterUpgradesArmor(list, shopPositionParams.gameObject, ShopNGUIController.CategoryNames.ArmorCategory, Defs.VisualArmor);
			}
		}
		list.Sort(func(ShopNGUIController.CategoryNames.ArmorCategory));
		return list;
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x0017B380 File Offset: 0x00179580
	private void FilterUpgradesArmor(List<GameObject> modelsList, GameObject prefab, ShopNGUIController.CategoryNames category, string visualDefName)
	{
		if (prefab.name.Replace("(Clone)", string.Empty) == "Armor_Novice")
		{
			return;
		}
		if (prefab != null && TempItemsController.PriceCoefs.ContainsKey(prefab.name) && TempItemsController.sharedController != null)
		{
			if (TempItemsController.sharedController.ContainsItem(prefab.name))
			{
				modelsList.Add(prefab);
			}
			return;
		}
		List<string> list = Wear.wear[category].FirstOrDefault((List<string> l) => l.Contains(prefab.name)).ToList<string>();
		if (list == null)
		{
			return;
		}
		int num = list.IndexOf(prefab.name);
		if (Storager.getInt(prefab.name, true) > 0)
		{
			if (num == list.Count - 1)
			{
				modelsList.Add(prefab);
			}
			else if (num >= 0 && num < list.Count - 1 && Storager.getInt(list[num + 1], true) == 0)
			{
				modelsList.Add(prefab);
			}
		}
		else
		{
			if (num == 0 && Wear.LeagueForWear(prefab.name, category) <= (int)RatingSystem.instance.currentLeague)
			{
				modelsList.Add(prefab);
			}
			if (num > 0 && Storager.getInt(list[num - 1], true) > 0)
			{
				modelsList.Add(prefab);
			}
		}
	}

	// Token: 0x06004622 RID: 17954 RVA: 0x0017B52C File Offset: 0x0017972C
	private ArmoryCell GetNewCell()
	{
		try
		{
			return UnityEngine.Object.Instantiate<ArmoryCell>(Resources.Load<ArmoryCell>("ArmoryCell"));
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in AddCellToPool: " + arg);
		}
		return null;
	}

	// Token: 0x06004623 RID: 17955 RVA: 0x0017B58C File Offset: 0x0017978C
	public static void AddBoughtCurrency(string currency, int count)
	{
		if (currency == null)
		{
			Debug.LogWarning("AddBoughtCurrency: currency == null");
			return;
		}
		if (Debug.isDebugBuild)
		{
			Debug.Log(string.Format("<color=#ff00ffff>AddBoughtCurrency {0} {1}</color>", currency, count));
		}
		Storager.setInt("BoughtCurrency" + currency, Storager.getInt("BoughtCurrency" + currency, false) + count, false);
	}

	// Token: 0x06004624 RID: 17956 RVA: 0x0017B5F0 File Offset: 0x001797F0
	public static void SpendBoughtCurrency(string currency, int count)
	{
		if (currency == null)
		{
			Debug.LogWarning("SpendBoughtCurrency: currency == null");
			return;
		}
		if (Debug.isDebugBuild)
		{
			Debug.Log(string.Format("<color=#ff00ffff>SpendBoughtCurrency {0} {1}</color>", currency, count));
		}
	}

	// Token: 0x06004625 RID: 17957 RVA: 0x0017B624 File Offset: 0x00179824
	public static void TryToBuy(GameObject mainPanel, ItemPrice price, Action onSuccess, Action onFailure = null, Func<bool> successAdditionalCond = null, Action onReturnFromBank = null, Action onEnterCoinsShopAction = null, Action onExitCoinsShopAction = null)
	{
		Debug.Log("Trying to buy from ShopNGUIController...");
		if (BankController.Instance == null)
		{
			Debug.LogWarning("BankController.Instance == null");
			return;
		}
		if (price == null)
		{
			Debug.LogWarning("price == null");
			return;
		}
		EventHandler handleBackFromBank = null;
		handleBackFromBank = delegate(object sender, EventArgs e)
		{
			BankController.Instance.BackRequested -= handleBackFromBank;
			BankController.Instance.InterfaceEnabled = false;
			coinsShop.thisScript.notEnoughCurrency = null;
			if (mainPanel != null)
			{
				mainPanel.SetActive(true);
			}
			if (onReturnFromBank != null)
			{
				onReturnFromBank();
			}
			if (onExitCoinsShopAction != null)
			{
				onExitCoinsShopAction();
			}
		};
		EventHandler act = null;
		act = delegate(object sender, EventArgs e)
		{
			BankController.Instance.BackRequested -= act;
			mainPanel.SetActive(true);
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int @int = Storager.getInt(price.Currency, false);
			int num = @int;
			num -= price.Price;
			bool flag = num >= 0;
			flag = ((successAdditionalCond == null) ? flag : (successAdditionalCond() || flag));
			if (flag)
			{
				Storager.setInt(price.Currency, num, false);
				ShopNGUIController.SpendBoughtCurrency(price.Currency, price.Price);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				if (ABTestController.useBuffSystem)
				{
					BuffSystem.instance.OnSomethingPurchased();
				}
				if (onSuccess != null)
				{
					onSuccess();
				}
			}
			else
			{
				if (onFailure != null)
				{
					onFailure();
				}
				coinsShop.thisScript.notEnoughCurrency = price.Currency;
				Debug.Log("Trying to display bank interface...");
				BankController.Instance.BackRequested += handleBackFromBank;
				BankController.Instance.InterfaceEnabled = true;
				mainPanel.SetActive(false);
				if (onEnterCoinsShopAction != null)
				{
					onEnterCoinsShopAction();
				}
			}
		};
		act(BankController.Instance, EventArgs.Empty);
	}

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x06004626 RID: 17958 RVA: 0x0017B6F8 File Offset: 0x001798F8
	public static bool NoviceArmorAvailable
	{
		get
		{
			return Storager.getInt("Training.NoviceArmorUsedKey", false) == 1 && (!TrainingController.TrainingCompleted || Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1);
		}
	}

	// Token: 0x06004627 RID: 17959 RVA: 0x0017B72C File Offset: 0x0017992C
	private static void FilterWearUpgrades(string item, ShopNGUIController.CategoryNames category, List<string> outputList)
	{
		if (item.Replace("(Clone)", string.Empty) == "Armor_Novice")
		{
			return;
		}
		if (item != null && TempItemsController.PriceCoefs.ContainsKey(item) && TempItemsController.sharedController != null)
		{
			if (TempItemsController.sharedController.ContainsItem(item))
			{
				outputList.Add(item);
			}
			return;
		}
		List<string> list = Wear.wear[category].FirstOrDefault((List<string> l) => l.Contains(item)).ToList<string>();
		if (list == null)
		{
			return;
		}
		int num = list.IndexOf(item);
		if (Storager.getInt(item, true) > 0)
		{
			if (num == list.Count - 1)
			{
				outputList.Add(item);
			}
			else if (num >= 0 && num < list.Count - 1 && Storager.getInt(list[num + 1], true) == 0)
			{
				outputList.Add(item);
			}
		}
		else if (num == 0 && Wear.LeagueForWear(item, category) <= (int)RatingSystem.instance.currentLeague)
		{
			outputList.Add(item);
		}
	}

	// Token: 0x06004628 RID: 17960 RVA: 0x0017B88C File Offset: 0x00179A8C
	public static bool ShowLockedFacebookSkin()
	{
		return FacebookController.FacebookSupported && (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.myPlayerMoveC != null) || Storager.getInt("super_socialman", true) > 0);
	}

	// Token: 0x06004629 RID: 17961 RVA: 0x0017B8DC File Offset: 0x00179ADC
	public static List<ShopNGUIController.ShopItem> GetItemNamesList(ShopNGUIController.CategoryNames category)
	{
		List<ShopNGUIController.ShopItem> list;
		Func<ShopNGUIController.CategoryNames, Comparison<string>> func = (ShopNGUIController.CategoryNames cn) => delegate(string leftWearItem, string rightWearItem)
		{
			List<string> list8 = Wear.wear[cn].FirstOrDefault((List<string> list) => list.Contains(leftWearItem));
			List<string> list9 = Wear.wear[cn].FirstOrDefault((List<string> list) => list.Contains(rightWearItem));
			if (list8 == null || list9 == null)
			{
				return 0;
			}
			if (list8 == list9)
			{
				return list8.IndexOf(leftWearItem) - list8.IndexOf(rightWearItem);
			}
			return Wear.wear[cn].IndexOf(list8) - Wear.wear[cn].IndexOf(list9);
		};
		list = new List<ShopNGUIController.ShopItem>();
		if (ShopNGUIController.IsBestCategory(category))
		{
			List<ShopNGUIController.CategoryNames> categoriesOfThisBestCategory = ShopNGUIController.СategoriesOfBestCategories[category];
			Func<ShopNGUIController.ShopItem, bool> isBought = delegate(ShopNGUIController.ShopItem shopItem)
			{
				if (ShopNGUIController.IsWeaponCategory(shopItem.Category))
				{
					ItemRecord byTag = ItemDb.GetByTag(shopItem.Id);
					return byTag != null && !byTag.StorageId.IsNullOrEmpty() && Storager.getInt(byTag.StorageId, true) > 0;
				}
				if (ShopNGUIController.IsGadgetsCategory(shopItem.Category))
				{
					return GadgetsInfo.IsBought(shopItem.Id);
				}
				if (shopItem.Category == ShopNGUIController.CategoryNames.SkinsCategory)
				{
					return SkinsController.IsSkinBought(shopItem.Id);
				}
				return Storager.getInt(shopItem.Id, true) > 0;
			};
			Func<List<string>, List<string>, List<ShopNGUIController.ShopItem>> func2 = delegate(List<string> src, List<string> excludeList)
			{
				List<ShopNGUIController.ShopItem> result;
				try
				{
					List<ShopNGUIController.ShopItem> list11 = new List<ShopNGUIController.ShopItem>();
					for (int i = 0; i < categoriesOfThisBestCategory.Count; i++)
					{
						List<ShopNGUIController.ShopItem> shopListOfThisCategory = ShopNGUIController.GetItemNamesList(categoriesOfThisBestCategory[i]);
						IEnumerable<ShopNGUIController.ShopItem> source = from shopItem in shopListOfThisCategory
						where src.Contains(shopItem.Id) && !excludeList.Contains(shopItem.Id) && !isBought(shopItem)
						select shopItem;
						List<ShopNGUIController.ShopItem> second4 = (from shopItem in source
						orderby shopListOfThisCategory.IndexOf(shopItem)
						select shopItem).ToList<ShopNGUIController.ShopItem>();
						list11 = list11.Concat(second4).ToList<ShopNGUIController.ShopItem>();
					}
					result = list11;
				}
				catch (Exception ex2)
				{
					Debug.LogErrorFormat("Exception in filterSource: {0}", new object[]
					{
						ex2
					});
					result = new List<ShopNGUIController.ShopItem>();
				}
				return result;
			};
			List<ShopNGUIController.ShopItem> list2 = func2(PromoActionsManager.sharedManager.news, new List<string>());
			List<ShopNGUIController.ShopItem> second = func2(PromoActionsManager.sharedManager.topSellers, (from shopItem in list2
			select shopItem.Id).ToList<string>());
			List<ShopNGUIController.ShopItem> second2 = ShopNGUIController.BestItemsToRemoveOnLeave ?? new List<ShopNGUIController.ShopItem>();
			return list2.Concat(second).Concat(second2).ToList<ShopNGUIController.ShopItem>();
		}
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			list = (from p in WeaponManager.sharedManager.FilteredShopListsNoUpgrades[(int)category]
			select new ShopNGUIController.ShopItem(ItemDb.GetByPrefabName(p.nameNoClone()).Tag, category)).ToList<ShopNGUIController.ShopItem>();
		}
		else
		{
			Dictionary<ShopNGUIController.CategoryNames, List<string>> dictionary = new Dictionary<ShopNGUIController.CategoryNames, List<string>>(4, ShopNGUIController.CategoryNameComparer.Instance);
			dictionary.Add(ShopNGUIController.CategoryNames.HatsCategory, (from item in ShopNGUIController.hats
			select item.nameNoClone()).ToList<string>());
			dictionary.Add(ShopNGUIController.CategoryNames.MaskCategory, (from item in ShopNGUIController.masks
			select item.nameNoClone()).ToList<string>());
			dictionary.Add(ShopNGUIController.CategoryNames.BootsCategory, (from item in ShopNGUIController.boots
			select item.nameNoClone()).ToList<string>());
			dictionary.Add(ShopNGUIController.CategoryNames.CapesCategory, (from item in ShopNGUIController.capes
			select item.nameNoClone()).ToList<string>());
			Dictionary<ShopNGUIController.CategoryNames, List<string>> dictionary2 = dictionary;
			ShopNGUIController.CategoryNames categoryNames = ShopNGUIController.MapShopCategoryToItemCategory(category);
			switch (categoryNames)
			{
			case ShopNGUIController.CategoryNames.HatsCategory:
			case ShopNGUIController.CategoryNames.CapesCategory:
			case ShopNGUIController.CategoryNames.BootsCategory:
			case ShopNGUIController.CategoryNames.MaskCategory:
			{
				IEnumerable<string> enumerable = Wear.LeagueItems().SelectMany((KeyValuePair<Wear.LeagueItemState, List<string>> kvp) => kvp.Value).Distinct<string>();
				bool flag = category != ShopNGUIController.MapShopCategoryToItemCategory(category);
				if (flag)
				{
					List<string> list3 = enumerable.ToList<string>();
					list3.Sort(func(ShopNGUIController.MapShopCategoryToItemCategory(category)));
					list.AddRange(from itemId in list3
					select new ShopNGUIController.ShopItem(itemId, ShopNGUIController.MapShopCategoryToItemCategory(category)));
				}
				else
				{
					IEnumerable<string> enumerable2 = dictionary2[ShopNGUIController.MapShopCategoryToItemCategory(category)].Except(enumerable);
					List<string> list4 = new List<string>();
					foreach (string item2 in enumerable2)
					{
						ShopNGUIController.FilterWearUpgrades(item2, ShopNGUIController.MapShopCategoryToItemCategory(category), list4);
					}
					list4.Sort(func(ShopNGUIController.MapShopCategoryToItemCategory(category)));
					list.AddRange(from itemId in list4
					select new ShopNGUIController.ShopItem(itemId, ShopNGUIController.MapShopCategoryToItemCategory(category)));
				}
				break;
			}
			default:
				if (categoryNames != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
				{
					if (categoryNames != ShopNGUIController.CategoryNames.ThrowingCategory && categoryNames != ShopNGUIController.CategoryNames.ToolsCategoty && categoryNames != ShopNGUIController.CategoryNames.SupportCategory)
					{
						if (categoryNames != ShopNGUIController.CategoryNames.PetsCategory)
						{
							if (categoryNames == ShopNGUIController.CategoryNames.EggsCategory)
							{
								List<Egg> list5 = new List<Egg>(Singleton<EggsManager>.Instance.GetPlayerEggsInIncubator());
								list5.Sort(delegate(Egg egg1, Egg egg2)
								{
									if (egg1.Data.Rare == EggRarity.Champion && egg2.Data.Rare == EggRarity.Champion)
									{
										return 0;
									}
									if (egg1.Data.Rare == EggRarity.Champion)
									{
										return -1;
									}
									if (egg2.Data.Rare == EggRarity.Champion)
									{
										return 1;
									}
									List<Egg> playerEggsInIncubator = Singleton<EggsManager>.Instance.GetPlayerEggsInIncubator();
									return playerEggsInIncubator.IndexOf(egg2).CompareTo(playerEggsInIncubator.IndexOf(egg1));
								});
								list.AddRange(from egg in list5
								select new ShopNGUIController.ShopItem(egg.Id.ToString(), ShopNGUIController.CategoryNames.EggsCategory));
							}
						}
						else
						{
							IEnumerable<ShopNGUIController.ShopItem> collection = from petOrEmptySlotId in Singleton<PetsManager>.Instance.PlayerPetIdsAndEmptySlots()
							select new ShopNGUIController.ShopItem(petOrEmptySlotId, ShopNGUIController.CategoryNames.PetsCategory);
							list.AddRange(collection);
						}
					}
					else
					{
						IEnumerable<List<string>> enumerable3 = from chain in GadgetsInfo.UpgradeChains
						where GadgetsInfo.info[chain[0]].Category == (GadgetInfo.GadgetCategory)category
						select chain;
						List<string> list6 = new List<string>();
						foreach (List<string> list7 in enumerable3)
						{
							string text = GadgetsInfo.LastBoughtFor(list7[0]);
							if (text != null)
							{
								list6.Add(text);
							}
							else
							{
								string text2 = GadgetsInfo.FirstForOurTier(list7[0]);
								if (GadgetsInfo.info[text2].Tier <= ExpController.OurTierForAnyPlace())
								{
									list6.Add(text2);
								}
							}
						}
						try
						{
							Func<string, int> getGadgetsTier = delegate(string gadgetId)
							{
								string key = GadgetsInfo.Upgrades[gadgetId][0];
								return GadgetsInfo.info[key].Tier;
							};
							list6 = (from gadgetId in list6
							orderby getGadgetsTier(gadgetId)
							select gadgetId).ToList<string>();
						}
						catch (Exception ex)
						{
							Debug.LogErrorFormat("Exception in sorting gadgets in shop: {0}", new object[]
							{
								ex
							});
						}
						list.AddRange(from gadgetId in list6
						select new ShopNGUIController.ShopItem(gadgetId, ShopNGUIController.MapShopCategoryToItemCategory(category)));
					}
				}
				else
				{
					list.AddRange(from skin in WeaponSkinsManager.AllSkins
					select new ShopNGUIController.ShopItem(skin.Id, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory));
				}
				break;
			case ShopNGUIController.CategoryNames.SkinsCategory:
			{
				ShopNGUIController.CategoryNames category2 = category;
				if (category2 != ShopNGUIController.CategoryNames.SkinsCategory)
				{
					if (category2 != ShopNGUIController.CategoryNames.LeagueSkinsCategory)
					{
						Debug.LogErrorFormat("Unknown category: {0}", new object[]
						{
							category
						});
					}
					else
					{
						try
						{
							IEnumerable<ShopNGUIController.ShopItem> collection2 = from skinId in SkinsController.leagueSkinsIds
							select new ShopNGUIController.ShopItem(skinId, ShopNGUIController.CategoryNames.SkinsCategory);
							list.AddRange(collection2);
						}
						catch (Exception arg)
						{
							Debug.LogError("Exception in filling league skins: " + arg);
						}
					}
				}
				else
				{
					IEnumerable<string> second3 = from item in SkinsController.sharedController.skinItems
					where item.category != SkinItem.CategoryNames.LeagueSkinsCategory
					orderby item.category
					select item.name;
					IEnumerable<ShopNGUIController.ShopItem> collection3 = from skinId in new List<string>
					{
						"CustomSkinID"
					}.Concat(ShopNGUIController.CustomSkinsReverse()).Concat(second3)
					select new ShopNGUIController.ShopItem(skinId, ShopNGUIController.CategoryNames.SkinsCategory);
					list.AddRange(collection3);
				}
				if (!ShopNGUIController.ShowLockedFacebookSkin())
				{
					list.RemoveAll((ShopNGUIController.ShopItem shopItem) => shopItem.Id == "super_socialman");
				}
				break;
			}
			}
		}
		return list;
	}

	// Token: 0x0600462A RID: 17962 RVA: 0x0017C0D8 File Offset: 0x0017A2D8
	private static IEnumerable<string> CustomSkinsReverse()
	{
		return SkinsController.CustomSkinIds().OrderByDescending(delegate(string x)
		{
			long result;
			if (long.TryParse(x, out result))
			{
				return result;
			}
			return 0L;
		});
	}

	// Token: 0x0600462B RID: 17963 RVA: 0x0017C104 File Offset: 0x0017A304
	private static string GetWeaponStatText(int currentValue, int nextValue)
	{
		if (nextValue - currentValue == 0)
		{
			return currentValue.ToString();
		}
		if (nextValue - currentValue > 0)
		{
			return string.Format("{0}[00ff00]+{1}[-]", currentValue, nextValue - currentValue);
		}
		return string.Format("{0}[FACC2E]{1}[-]", currentValue, nextValue - currentValue);
	}

	// Token: 0x0600462C RID: 17964 RVA: 0x0017C160 File Offset: 0x0017A360
	public void SetCamera()
	{
		Transform child = this.characterPoint.GetChild(0);
		HOTween.Kill(child);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 p_endVal = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(1f, 1f, 1f);
		ShopNGUIController.CategoryNames categoryNames = this.CurrentCategory;
		if (this.CurrentItem != null)
		{
			categoryNames = this.CurrentItem.Category;
		}
		ShopNGUIController.CategoryNames categoryNames2 = categoryNames;
		switch (categoryNames2)
		{
		case ShopNGUIController.CategoryNames.HatsCategory:
			vector = new Vector3(1.06f, -0.54f, 2.19f);
			p_endVal = new Vector3(0f, -9.5f, 0f);
			break;
		default:
			if (categoryNames2 != ShopNGUIController.CategoryNames.MaskCategory)
			{
				vector = new Vector3(0f, 0f, 0f);
				p_endVal = new Vector3(0f, 0f, 0f);
			}
			else
			{
				vector = new Vector3(1.06f, -0.54f, 2.19f);
				p_endVal = new Vector3(0f, -9.5f, 0f);
			}
			break;
		case ShopNGUIController.CategoryNames.CapesCategory:
			vector = new Vector3(0f, 0f, 0f);
			p_endVal = new Vector3(0f, -130.76f, 0f);
			break;
		}
		float p_duration = 0.5f;
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(child, p_duration, new TweenParms().Prop("localPosition", vector).Prop("localRotation", new PlugQuaternion(p_endVal)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(delegate()
		{
			this.idleTimerLastTime = Time.realtimeSinceStartup;
		}));
	}

	// Token: 0x0600462D RID: 17965 RVA: 0x0017C330 File Offset: 0x0017A530
	public static bool IsModeWithNormalTimeScaleInShop()
	{
		bool flag = !Defs.IsSurvival && TrainingController.TrainingCompleted && !Defs.isMulti;
		return !Defs.IsSurvival && !flag;
	}

	// Token: 0x0600462E RID: 17966 RVA: 0x0017C370 File Offset: 0x0017A570
	private void PlayPetAnimation()
	{
		try
		{
			if (this.characterInterface != null && this.characterInterface.myPet != null)
			{
				if (!ShopNGUIController.IsModeWithNormalTimeScaleInShop())
				{
					Animation component = this.characterInterface.myPet.GetComponent<PetEngine>().Model.GetComponent<Animation>();
					this.StopPetAnimation();
					if (component.GetClip("Profile") == null)
					{
						Debug.LogErrorFormat("Error: pet {0} has no Profile animation clip", new object[]
						{
							this.characterInterface.myPet.nameNoClone()
						});
					}
					else if (this.petProfileAnimationRunner.gameObject.activeInHierarchy)
					{
						this.petProfileAnimationRunner.StartPlay(component, "Profile", false, null);
					}
					else
					{
						Debug.LogErrorFormat("Coroutine couldn't be started because the the game object 'AnimationCoroutineRunner' is inactive! (Pet)", new object[0]);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in PlayPetAnimation: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x0600462F RID: 17967 RVA: 0x0017C48C File Offset: 0x0017A68C
	public void PlayWeaponAnimation()
	{
		if (this.profile != null && this.weapon != null && this.weapon.GetComponent<WeaponSounds>() != null)
		{
			Animation component = this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>();
			if (ShopNGUIController.IsModeWithNormalTimeScaleInShop())
			{
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(this.profile, "Profile");
				}
				component.Play("Profile");
			}
			else
			{
				this.animationCoroutineRunner.StopAllCoroutines();
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(this.profile, "Profile");
				}
				if (this.animationCoroutineRunner.gameObject.activeInHierarchy)
				{
					this.animationCoroutineRunner.StartPlay(component, "Profile", false, null);
				}
				else
				{
					Debug.LogWarning("Coroutine couldn't be started because the the game object 'AnimationCoroutineRunner' is inactive!");
				}
			}
		}
		this.MainMenu_Pers.GetComponent<Animation>().Stop();
		this.MainMenu_Pers.GetComponent<Animation>().Play("Idle");
	}

	// Token: 0x06004630 RID: 17968 RVA: 0x0017C5B8 File Offset: 0x0017A7B8
	public static Texture TextureForEquippedWeaponOrWear(int cc)
	{
		string text = (!ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)cc)) ? ((!ShopNGUIController.IsWearCategory((ShopNGUIController.CategoryNames)cc)) ? ((!ShopNGUIController.IsGadgetsCategory((ShopNGUIController.CategoryNames)cc)) ? ((cc != 25000) ? "potion" : Singleton<PetsManager>.Instance.GetEqipedPetId()) : GadgetsInfo.EquippedForCategory((GadgetInfo.GadgetCategory)cc)) : ShopNGUIController.sharedShop.WearForCat((ShopNGUIController.CategoryNames)cc)) : ShopNGUIController._CurrentWeaponSetIDs()[cc];
		if (text.IsNullOrEmpty())
		{
			return null;
		}
		return ItemDb.GetItemIcon(text, (ShopNGUIController.CategoryNames)cc, null, true);
	}

	// Token: 0x06004631 RID: 17969 RVA: 0x0017C650 File Offset: 0x0017A850
	public void SetIconModelsPositions(Transform t, ShopNGUIController.CategoryNames c)
	{
		switch (ShopNGUIController.MapShopCategoryToItemCategory(c))
		{
		case ShopNGUIController.CategoryNames.HatsCategory:
		{
			t.transform.localPosition = new Vector3(-0.62f, -0.04f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(-75f, -165f, -90f));
			float num = 82.5f;
			t.transform.localScale = new Vector3(num, num, num);
			break;
		}
		case ShopNGUIController.CategoryNames.ArmorCategory:
		{
			t.transform.localPosition = new Vector3(0f, 0f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			float num2 = 1f;
			t.transform.localScale = new Vector3(num2, num2, num2);
			break;
		}
		case ShopNGUIController.CategoryNames.SkinsCategory:
			SkinsController.SetTransformParamtersForSkinModel(t);
			break;
		case ShopNGUIController.CategoryNames.CapesCategory:
		{
			t.transform.localPosition = new Vector3(-0.720093f, 5.35f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(8f, -25f, 0f));
			float num3 = 60f;
			t.transform.localScale = new Vector3(num3, num3, num3);
			break;
		}
		case ShopNGUIController.CategoryNames.BootsCategory:
		{
			t.transform.localPosition = new Vector3(-0.4f, -0.1f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(13f, 149f, 0f));
			float num4 = 75f;
			t.transform.localScale = new Vector3(num4, num4, num4);
			break;
		}
		case ShopNGUIController.CategoryNames.GearCategory:
		{
			t.transform.localPosition = new Vector3(4.648193f, 2.444565f, 0f);
			t.transform.localRotation = Quaternion.Euler(new Vector3(8f, -25f, 0f));
			float num5 = 319.3023f;
			t.transform.localScale = new Vector3(num5, num5, num5);
			break;
		}
		}
	}

	// Token: 0x06004632 RID: 17970 RVA: 0x0017C884 File Offset: 0x0017AA84
	private void DisableGunflashes(GameObject root)
	{
		if (root == null)
		{
			return;
		}
		if (root.name.Equals("GunFlash"))
		{
			root.SetActive(false);
		}
		foreach (object obj in root.transform)
		{
			Transform transform = (Transform)obj;
			if (!(null == transform))
			{
				this.DisableGunflashes(transform.gameObject);
			}
		}
	}

	// Token: 0x06004633 RID: 17971 RVA: 0x0017C934 File Offset: 0x0017AB34
	public void UpdateIcons(bool animateModel = false)
	{
		Enum.GetValues(typeof(ShopNGUIController.CategoryNames)).OfType<ShopNGUIController.CategoryNames>().ForEach(delegate(ShopNGUIController.CategoryNames cat)
		{
			this.UpdateIcon(cat, animateModel);
		});
	}

	// Token: 0x06004634 RID: 17972 RVA: 0x0017C97C File Offset: 0x0017AB7C
	public void UpdateIcon(ShopNGUIController.CategoryNames category, bool animateModel = false)
	{
		if (category == ShopNGUIController.CategoryNames.GearCategory || category == ShopNGUIController.CategoryNames.EggsCategory || category == ShopNGUIController.CategoryNames.SkinsCategoryMale || category == ShopNGUIController.CategoryNames.SkinsCategoryFemale || category == ShopNGUIController.CategoryNames.SkinsCategorySpecial || category == ShopNGUIController.CategoryNames.SkinsCategoryPremium || category == ShopNGUIController.CategoryNames.SkinsCategoryEditor || category == ShopNGUIController.CategoryNames.BestWeapons || category == ShopNGUIController.CategoryNames.BestWear || category == ShopNGUIController.CategoryNames.BestGadgets)
		{
			return;
		}
		ShopCategoryButton shopCategoryButton = this.TransformOfButtonForCategory(category).GetComponent<ShopCategoryButton>();
		Action<Texture> action = delegate(Texture texture)
		{
			shopCategoryButton.icon.mainTexture = texture;
		};
		if (category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			action(shopCategoryButton.icon.mainTexture);
			return;
		}
		string capeName = string.Empty;
		try
		{
			capeName = Storager.getString(Defs.CapeEquppedSN, false);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception getting cape in UpdateIcon: " + arg);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory && shopCategoryButton.modelPoint != null)
		{
			shopCategoryButton.modelPoint.DestroyChildren();
		}
		if (category != ShopNGUIController.CategoryNames.SkinsCategory)
		{
			if (category != ShopNGUIController.CategoryNames.LeagueSkinsCategory)
			{
				if (category == ShopNGUIController.CategoryNames.CapesCategory && capeName == "cape_Custom")
				{
					if (shopCategoryButton.icon != null)
					{
						shopCategoryButton.icon.mainTexture = null;
					}
					if (shopCategoryButton.emptyIcon != null)
					{
						shopCategoryButton.emptyIcon.SetActive(false);
					}
					GameObject wearFromResources = ItemDb.GetWearFromResources("cape_Custom", ShopNGUIController.CategoryNames.CapesCategory);
					ShopNGUIController.AddModel(wearFromResources, delegate(GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float sc, int _unusedTier, int _unusedLeague)
					{
						manipulateObject.transform.parent = shopCategoryButton.modelPoint;
						float num = 0.5f;
						Transform transform = manipulateObject.transform;
						transform.localPosition = shopCategoryButton.modelPoint.localPosition + positionShop * num;
						transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
						transform.Rotate(rotationShop, Space.World);
						transform.localScale = new Vector3(sc * num, sc * num, sc * num);
						if (category == ShopNGUIController.CategoryNames.CapesCategory && capeName == "cape_Custom" && SkinsController.capeUserTexture != null)
						{
							Player_move_c.SetTextureRecursivelyFrom(manipulateObject, SkinsController.capeUserTexture, new GameObject[0]);
						}
						this.SetIconModelsPositions(transform, category);
					}, ShopNGUIController.MapShopCategoryToItemCategory(category), false, null);
					return;
				}
				Texture texture2 = ShopNGUIController.TextureForEquippedWeaponOrWear((int)ShopNGUIController.MapShopCategoryToItemCategory(category));
				if (shopCategoryButton.emptyIcon != null)
				{
					shopCategoryButton.emptyIcon.SetActive(texture2 == null);
				}
				if (texture2 != null)
				{
					action(texture2);
					return;
				}
				if (shopCategoryButton.icon != null)
				{
					shopCategoryButton.icon.mainTexture = null;
					return;
				}
				return;
			}
		}
		try
		{
			Tools.SetTextureRecursivelyFrom(shopCategoryButton.modelPoint.gameObject, SkinsController.currentSkinForPers, new GameObject[0]);
			HOTween.Kill(shopCategoryButton.modelPoint);
			shopCategoryButton.modelPoint.localScale = Vector3.one;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateIcon, updating skin icon: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06004635 RID: 17973 RVA: 0x0017CCF8 File Offset: 0x0017AEF8
	public void EquipWear(string tg)
	{
		ShopNGUIController.EquipWearInCategory(tg, this.CurrentItem.Category, this.inGame);
	}

	// Token: 0x06004636 RID: 17974 RVA: 0x0017CD14 File Offset: 0x0017AF14
	public static void EquipWearInCategoryIfNotEquiped(string tg, ShopNGUIController.CategoryNames cat, bool inGameLocal)
	{
		if (!Storager.hasKey(ShopNGUIController.SnForWearCategory(cat)))
		{
			Storager.setString(ShopNGUIController.SnForWearCategory(cat), ShopNGUIController.NoneEquippedForWearCategory(cat), false);
		}
		if (!Storager.getString(ShopNGUIController.SnForWearCategory(cat), false).Equals(tg))
		{
			ShopNGUIController.EquipWearInCategory(tg, cat, inGameLocal);
		}
	}

	// Token: 0x06004637 RID: 17975 RVA: 0x0017CD64 File Offset: 0x0017AF64
	public static void SendEquippedWearInCategory(string equippedItem, ShopNGUIController.CategoryNames cat, string previousItem)
	{
		Action<string, ShopNGUIController.CategoryNames, string> equippedWearInCategory = ShopNGUIController.EquippedWearInCategory;
		if (equippedWearInCategory != null)
		{
			equippedWearInCategory(equippedItem ?? string.Empty, cat, previousItem ?? string.Empty);
		}
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x0017CDA0 File Offset: 0x0017AFA0
	private static void EquipWearInCategory(string tg, ShopNGUIController.CategoryNames cat, bool inGameLocal)
	{
		bool flag = !Defs.isMulti;
		string @string = Storager.getString(ShopNGUIController.SnForWearCategory(cat), false);
		Player_move_c player_move_c = null;
		if (inGameLocal)
		{
			if (flag)
			{
				if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && !SceneLoader.ActiveSceneName.Equals("ChooseLevel"))
				{
					player_move_c = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		ShopNGUIController.SetAsEquippedAndSendToServer(tg, cat);
		if (ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(cat, @string ?? ShopNGUIController.NoneEquippedForWearCategory(cat), ShopNGUIController.sharedShop.WearForCat(cat));
		}
		if (cat == ShopNGUIController.CategoryNames.BootsCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.bootsMethods.ContainsKey(@string))
			{
				Wear.bootsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.bootsMethods.ContainsKey(tg))
			{
				Wear.bootsMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (cat == ShopNGUIController.CategoryNames.CapesCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.capesMethods.ContainsKey(@string))
			{
				Wear.capesMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.capesMethods.ContainsKey(tg))
			{
				Wear.capesMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (cat == ShopNGUIController.CategoryNames.HatsCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.hatsMethods.ContainsKey(@string))
			{
				Wear.hatsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.hatsMethods.ContainsKey(tg))
			{
				Wear.hatsMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (cat == ShopNGUIController.CategoryNames.ArmorCategory && inGameLocal && player_move_c != null)
		{
			if (!@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.armorMethods.ContainsKey(@string))
			{
				Wear.armorMethods[@string].Value(player_move_c, new Dictionary<string, object>());
			}
			if (Wear.armorMethods.ContainsKey(tg))
			{
				Wear.armorMethods[tg].Key(player_move_c, new Dictionary<string, object>());
			}
		}
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateButtons();
			ShopNGUIController.sharedShop.UpdateIcons(true);
		}
		ShopNGUIController.SendEquippedWearInCategory(tg, cat, @string);
	}

	// Token: 0x06004639 RID: 17977 RVA: 0x0017D0BC File Offset: 0x0017B2BC
	public static void UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames cat, bool inGameLocal)
	{
		bool flag = !Defs.isMulti;
		string @string = Storager.getString(ShopNGUIController.SnForWearCategory(cat), false);
		Player_move_c player_move_c = null;
		if (inGameLocal)
		{
			if (flag)
			{
				if (!SceneLoader.ActiveSceneName.Equals("LevelComplete") && !SceneLoader.ActiveSceneName.Equals("ChooseLevel"))
				{
					player_move_c = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
				}
			}
			else if (WeaponManager.sharedManager.myPlayer != null)
			{
				player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		Storager.setString(ShopNGUIController.SnForWearCategory(cat), ShopNGUIController.NoneEquippedForWearCategory(cat), false);
		FriendsController.sharedController.SendAccessories();
		if (ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(cat, @string ?? ShopNGUIController.NoneEquippedForWearCategory(cat), ShopNGUIController.NoneEquippedForWearCategory(cat));
		}
		if (cat == ShopNGUIController.CategoryNames.BootsCategory && inGameLocal && player_move_c != null && !@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.bootsMethods.ContainsKey(@string))
		{
			Wear.bootsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (cat == ShopNGUIController.CategoryNames.CapesCategory && inGameLocal && player_move_c != null && !@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.capesMethods.ContainsKey(@string))
		{
			Wear.capesMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (cat == ShopNGUIController.CategoryNames.HatsCategory && inGameLocal && player_move_c != null && !@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.hatsMethods.ContainsKey(@string))
		{
			Wear.hatsMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (cat == ShopNGUIController.CategoryNames.ArmorCategory && inGameLocal && player_move_c != null && !@string.Equals(ShopNGUIController.NoneEquippedForWearCategory(cat)) && Wear.armorMethods.ContainsKey(@string))
		{
			Wear.armorMethods[@string].Value(player_move_c, new Dictionary<string, object>());
		}
		if (ShopNGUIController.sharedShop.wearUnequipAction != null)
		{
			ShopNGUIController.sharedShop.wearUnequipAction(cat, @string ?? ShopNGUIController.NoneEquippedForWearCategory(cat));
		}
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateIcons(false);
		}
		Action<ShopNGUIController.CategoryNames, string> unequippedWearInCategory = ShopNGUIController.UnequippedWearInCategory;
		if (unequippedWearInCategory != null)
		{
			unequippedWearInCategory(cat, @string ?? string.Empty);
		}
	}

	// Token: 0x0600463A RID: 17978 RVA: 0x0017D368 File Offset: 0x0017B568
	public static void ShowTryGunIfPossible(bool placeForGiveNewTryGun, Transform point, string layer, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitoinsShopAdditional = null, Action<string> customEquipWearAction = null)
	{
		if (!Defs.isHunger && !Defs.isDuel && !placeForGiveNewTryGun && WeaponManager.sharedManager != null && WeaponManager.sharedManager.ExpiredTryGuns.Count > 0 && TrainingController.TrainingCompleted)
		{
			string tg;
			foreach (string tg2 in WeaponManager.sharedManager.ExpiredTryGuns)
			{
				tg = tg2;
				try
				{
					if (WeaponManager.sharedManager.weaponsInGame.FirstOrDefault((UnityEngine.Object w) => ItemDb.GetByPrefabName(w.name).Tag == tg) != null)
					{
						WeaponManager.sharedManager.ExpiredTryGuns.RemoveAll((string t) => t == tg);
						if (WeaponManager.LastBoughtTag(tg, null) == null)
						{
							ShopNGUIController.ShowAddTryGun(tg, point, layer, onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction, true);
							break;
						}
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception in foreach (var tg in WeaponManager.sharedManager.ExpiredTryGuns): " + arg);
				}
			}
		}
		else if (!Defs.isHunger && !Defs.isDuel && !Defs.isDaterRegim && WeaponManager.sharedManager._currentFilterMap == 0 && ((!ABTestController.useBuffSystem) ? KillRateCheck.instance.giveWeapon : BuffSystem.instance.giveTryGun) && TrainingController.TrainingCompleted)
		{
			try
			{
				int maximumCoinBank = ShopNGUIController.UpperCoinsBankBound();
				List<ItemRecord> source = (from prefabName in WeaponManager.tryGunsTable.SelectMany((KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> kvp) => kvp.Value[ExpController.OurTierForAnyPlace()])
				select ItemDb.GetByPrefabName(prefabName) into rec
				where rec.StorageId != null && Storager.getInt(rec.StorageId, true) == 0
				where !WeaponManager.sharedManager.IsAvailableTryGun(rec.Tag) && !WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(rec.Tag)
				select rec).ToList<ItemRecord>();
				List<ItemRecord> source2 = (from rec in source
				where rec.Price.Currency == "Coins"
				where ShopNGUIController.PriceIfGunWillBeTryGun(rec.Tag) > maximumCoinBank
				select rec).Randomize<ItemRecord>().ToList<ItemRecord>();
				string text;
				if (source2.Any<ItemRecord>())
				{
					text = source2.First<ItemRecord>().Tag;
				}
				else
				{
					int maximumGemBank = ShopNGUIController.UpperGemsBankBound();
					List<ItemRecord> source3 = (from rec in source
					where rec.Price.Currency == "GemsCurrency"
					where ShopNGUIController.PriceIfGunWillBeTryGun(rec.Tag) > maximumGemBank
					select rec).Randomize<ItemRecord>().ToList<ItemRecord>();
					if (source3.Any<ItemRecord>())
					{
						text = source3.First<ItemRecord>().Tag;
					}
					else
					{
						text = ShopNGUIController.TryGunForCategoryWithMaxUnbought();
					}
				}
				if (text != null)
				{
					ShopNGUIController.ShowAddTryGun(text, point, layer, onPurchase, onEnterCoinsShopAdditional, onExitoinsShopAdditional, customEquipWearAction, false);
				}
			}
			catch (Exception arg2)
			{
				Debug.LogError("Exception in giving: " + arg2);
			}
		}
	}

	// Token: 0x0600463B RID: 17979 RVA: 0x0017D6F8 File Offset: 0x0017B8F8
	private static void AnimateScaleForTransform(Transform t)
	{
		Vector3 localScale = t.localScale;
		t.localScale *= 1.25f;
		HOTween.To(t, 0.25f, new TweenParms().Prop("localScale", localScale).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear));
	}

	// Token: 0x0600463C RID: 17980 RVA: 0x0017D750 File Offset: 0x0017B950
	private static int UpperCoinsBankBound()
	{
		int num = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
		num = Mathf.Clamp(num, 0, ExperienceController.addCoinsFromLevels.Length - 1);
		return Storager.getInt("Coins", false) + 30 + ExperienceController.addCoinsFromLevels[num];
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x0017D7A8 File Offset: 0x0017B9A8
	private static int UpperGemsBankBound()
	{
		int num = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
		num = Mathf.Clamp(num, 0, ExperienceController.addGemsFromLevels.Length - 1);
		return Storager.getInt("GemsCurrency", false) + ExperienceController.addGemsFromLevels[num];
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x0017D7FC File Offset: 0x0017B9FC
	private static string TryGunForCategoryWithMaxUnbought()
	{
		List<ShopNGUIController.CategoryNames> list = new List<ShopNGUIController.CategoryNames>
		{
			ShopNGUIController.CategoryNames.PrimaryCategory,
			ShopNGUIController.CategoryNames.BackupCategory,
			ShopNGUIController.CategoryNames.MeleeCategory,
			ShopNGUIController.CategoryNames.SpecilCategory,
			ShopNGUIController.CategoryNames.SniperCategory,
			ShopNGUIController.CategoryNames.PremiumCategory
		}.Randomize<ShopNGUIController.CategoryNames>().OrderBy(delegate(ShopNGUIController.CategoryNames cat)
		{
			List<WeaponSounds> list2 = (from w in WeaponManager.sharedManager.weaponsInGame
			select ((GameObject)w).GetComponent<WeaponSounds>() into ws
			where ws.categoryNabor - 1 == (int)cat && ws.tier == ExpController.OurTierForAnyPlace()
			where WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(ws.name).Tag) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]], true) == 1
			select ws).ToList<WeaponSounds>();
			return list2.Count;
		}).ToList<ShopNGUIController.CategoryNames>();
		string result = null;
		for (int i = 0; i < list.Count; i++)
		{
			ShopNGUIController.CategoryNames cat = list[i];
			List<WeaponSounds> source = (from ws in (from w in WeaponManager.sharedManager.weaponsInGame
			select ((GameObject)w).GetComponent<WeaponSounds>() into ws
			where ws.categoryNabor - 1 == (int)cat && ws.tier == ExpController.OurTierForAnyPlace()
			select ws).Where(delegate(WeaponSounds ws)
			{
				List<string> list2 = WeaponUpgrades.ChainForTag(ItemDb.GetByPrefabName(ws.name).Tag);
				return list2 == null || (list2.Count > 0 && list2[0] == ItemDb.GetByPrefabName(ws.name).Tag);
			})
			where WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(ws.name).Tag) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[ItemDb.GetByPrefabName(ws.name).Tag]], true) == 0
			where WeaponManager.tryGunsTable[cat][ExpController.OurTierForAnyPlace()].Contains(ItemDb.GetByTag(ItemDb.GetByPrefabName(ws.name).Tag).PrefabName)
			where !WeaponManager.sharedManager.IsAvailableTryGun(ItemDb.GetByPrefabName(ws.name).Tag) && !WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(ItemDb.GetByPrefabName(ws.name).Tag)
			select ws).Randomize<WeaponSounds>().ToList<WeaponSounds>();
			if (source.Count<WeaponSounds>() != 0)
			{
				result = ItemDb.GetByPrefabName(source.First<WeaponSounds>().name).Tag;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x0017D98C File Offset: 0x0017BB8C
	public static bool ShowPremimAccountExpiredIfPossible(Transform point, string layer, string header = "", bool showOnlyIfExpired = true)
	{
		if (showOnlyIfExpired && (!PremiumAccountController.AccountHasExpired || !Defs2.CanShowPremiumAccountExpiredWindow))
		{
			return false;
		}
		if (point != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("PremiumAccount"));
			gameObject.transform.parent = point;
			Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer(layer ?? "Default"));
			gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			PremiumAccountScreenController component = gameObject.GetComponent<PremiumAccountScreenController>();
			component.Header = header;
			PremiumAccountController.AccountHasExpired = false;
			return true;
		}
		return false;
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x0017DA5C File Offset: 0x0017BC5C
	public static void GiveArmorArmy1OrNoviceArmor()
	{
		ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1) ? "Armor_Army_1" : "Armor_Novice", 1, false, 0, null, null, true, Storager.getInt("Training.NoviceArmorUsedKey", false) == 1, false);
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x06004641 RID: 17985 RVA: 0x0017DAA4 File Offset: 0x0017BCA4
	private static List<ShopNGUIController.ShopItem> BestItemsToRemoveOnLeave
	{
		get
		{
			return ShopNGUIController.m_bestItemsToRemoveOnLeave;
		}
	}

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x06004642 RID: 17986 RVA: 0x0017DAAC File Offset: 0x0017BCAC
	private static Dictionary<ShopNGUIController.CategoryNames, List<ShopNGUIController.CategoryNames>> СategoriesOfBestCategories
	{
		get
		{
			return ShopNGUIController.m_categoriesOfBestCategories;
		}
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x0017DAB4 File Offset: 0x0017BCB4
	public static void SetEggInfoHintViewed()
	{
		Storager.setInt("Shop.Tutorial.KEY_TUTORIAL_EGG_INFO_HINT_VIEWED", 1, false);
	}

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06004644 RID: 17988 RVA: 0x0017DAC4 File Offset: 0x0017BCC4
	// (set) Token: 0x06004645 RID: 17989 RVA: 0x0017DAD4 File Offset: 0x0017BCD4
	public ShopNGUIController.TutorialPhase TutorialPhasePassed
	{
		get
		{
			return (ShopNGUIController.TutorialPhase)PlayerPrefs.GetInt("shop_tutorial_state_passed_VER_12_1", 0);
		}
		set
		{
			PlayerPrefs.SetInt("shop_tutorial_state_passed_VER_12_1", (int)value);
		}
	}

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06004646 RID: 17990 RVA: 0x0017DAE4 File Offset: 0x0017BCE4
	// (set) Token: 0x06004647 RID: 17991 RVA: 0x0017DB18 File Offset: 0x0017BD18
	private ShopNGUIController.TutorialPhase? TutorialPhaseLastViewed
	{
		get
		{
			int @int = PlayerPrefs.GetInt("shop_tutorial_state_viewed", -1);
			ShopNGUIController.TutorialPhase? result = null;
			if (@int > -1)
			{
				result = new ShopNGUIController.TutorialPhase?((ShopNGUIController.TutorialPhase)@int);
			}
			return result;
		}
		set
		{
			PlayerPrefs.SetInt("shop_tutorial_state_viewed", (int)value.Value);
		}
	}

	// Token: 0x06004648 RID: 17992 RVA: 0x0017DB2C File Offset: 0x0017BD2C
	private void OnTrainingCompleted_4_4_Sett_Changed()
	{
		if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) > 0)
		{
			if (this._tutorialCurrentState != null)
			{
				this._tutorialCurrentState.StageAct(ShopNGUIController.TutorialStageTrigger.Exit);
				this._tutorialCurrentState = null;
			}
			this._tutorialHintsContainer.gameObject.SetActive(false);
			this.backButton.isEnabled = true;
			this.unlockButton.isEnabled = true;
		}
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x0017DB98 File Offset: 0x0017BD98
	private void UpdateTutorialState()
	{
		if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			this.UpdateExtTutorial();
			return;
		}
		if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) > 0)
		{
			this.OnTrainingCompleted_4_4_Sett_Changed();
			return;
		}
		if (!this._tutorialStates.Any<ShopNGUIController.TutorialState>())
		{
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.SelectWeaponCategory, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialSelectWeaponCategory)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.SelectSniperSection, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialSelectSniperSection)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.SelectRifle, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialSelectRifle)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.EquipRifle, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialEquipRifle)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.SelectWearCategory, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialSelectWearCategory)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.EquipArmor, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialEquipArmor)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.SelectArmorSection, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialSelectArmorSection)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.SelectPetsCategory, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialSelectPetsCategory)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.ShowEggsHint, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialShowEggsHint)));
			this._tutorialStates.Add(new ShopNGUIController.TutorialState(ShopNGUIController.TutorialPhase.LeaveArmory, new Action<ShopNGUIController.TutorialStageTrigger>(this.TutorialLeaveArmory)));
		}
		ShopNGUIController.TutorialPhase tutorialPhase = (this._tutorialCurrentState == null) ? this.TutorialPhasePassed : this._tutorialCurrentState.ForStage;
		if (tutorialPhase < ShopNGUIController.TutorialPhase.SelectWearCategory)
		{
			if (WeaponManager.sharedManager == null)
			{
				return;
			}
			for (int i = 0; i < WeaponManager.sharedManager.playerWeapons.Count; i++)
			{
				string tag = ItemDb.GetByPrefabName((WeaponManager.sharedManager.playerWeapons[i] as Weapon).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
				if (tag == WeaponTags.HunterRifleTag)
				{
					this.TutorialPhasePassed = ShopNGUIController.TutorialPhase.SelectWearCategory;
					tutorialPhase = ShopNGUIController.TutorialPhase.SelectWearCategory;
					break;
				}
			}
		}
		else if (Storager.getString(Defs.ArmorNewEquppedSN, false) != Defs.ArmorNewNoneEqupped && this.TutorialPhasePassed < ShopNGUIController.TutorialPhase.SelectPetsCategory)
		{
			this.TutorialPhasePassed = ShopNGUIController.TutorialPhase.SelectPetsCategory;
			tutorialPhase = ShopNGUIController.TutorialPhase.SelectPetsCategory;
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Equip_Armor, 0);
		}
		ShopNGUIController.TutorialPhase toPhase = tutorialPhase;
		this.backButton.isEnabled = (tutorialPhase >= ShopNGUIController.TutorialPhase.LeaveArmory);
		this.unlockButton.isEnabled = (tutorialPhase >= ShopNGUIController.TutorialPhase.LeaveArmory);
		switch (tutorialPhase)
		{
		case ShopNGUIController.TutorialPhase.SelectWeaponCategory:
			if (this.superCategoriesButtonController.currentBtnName == "Weapons")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectSniperSection;
			}
			else
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWeaponCategory;
			}
			break;
		case ShopNGUIController.TutorialPhase.SelectSniperSection:
			if (this.superCategoriesButtonController.currentBtnName != "Weapons")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWeaponCategory;
			}
			else if (this.CurrentCategory != ShopNGUIController.CategoryNames.SniperCategory)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectSniperSection;
			}
			else
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectRifle;
			}
			break;
		case ShopNGUIController.TutorialPhase.SelectRifle:
			if (this.superCategoriesButtonController.currentBtnName != "Weapons")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWeaponCategory;
			}
			else if (this.CurrentCategory != ShopNGUIController.CategoryNames.SniperCategory)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectSniperSection;
			}
			else if (this.CurrentItem.Id != WeaponTags.HunterRifleTag)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectRifle;
			}
			else
			{
				toPhase = ShopNGUIController.TutorialPhase.EquipRifle;
			}
			break;
		case ShopNGUIController.TutorialPhase.EquipRifle:
			if (this.superCategoriesButtonController.currentBtnName != "Weapons")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWeaponCategory;
			}
			else if (this.CurrentCategory != ShopNGUIController.CategoryNames.SniperCategory)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectSniperSection;
			}
			else if (this.CurrentItem.Id != WeaponTags.HunterRifleTag)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectRifle;
			}
			else
			{
				for (int j = 0; j < WeaponManager.sharedManager.playerWeapons.Count; j++)
				{
					string tag2 = ItemDb.GetByPrefabName((WeaponManager.sharedManager.playerWeapons[j] as Weapon).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
					if (tag2 == WeaponTags.HunterRifleTag)
					{
						if (this.TutorialPhasePassed < ShopNGUIController.TutorialPhase.SelectWearCategory)
						{
							this.TutorialPhasePassed = ShopNGUIController.TutorialPhase.SelectWearCategory;
						}
						toPhase = ShopNGUIController.TutorialPhase.SelectWearCategory;
						break;
					}
				}
			}
			break;
		case ShopNGUIController.TutorialPhase.SelectWearCategory:
			if (this.superCategoriesButtonController.currentBtnName == "Wear")
			{
				toPhase = ShopNGUIController.TutorialPhase.EquipArmor;
			}
			else
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWearCategory;
			}
			break;
		case ShopNGUIController.TutorialPhase.EquipArmor:
			if (this.superCategoriesButtonController.currentBtnName != "Wear")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWearCategory;
			}
			else if (this.CurrentCategory != ShopNGUIController.CategoryNames.ArmorCategory)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectArmorSection;
			}
			else
			{
				if (this.CurrentItem.Id != "Armor_Army_1")
				{
					this.ChooseCarouselItem(new ShopNGUIController.ShopItem("Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory), true);
				}
				toPhase = ShopNGUIController.TutorialPhase.EquipArmor;
			}
			break;
		case ShopNGUIController.TutorialPhase.SelectArmorSection:
			if (this.superCategoriesButtonController.currentBtnName != "Wear")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectWearCategory;
			}
			else if (this.CurrentCategory != ShopNGUIController.CategoryNames.ArmorCategory)
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectArmorSection;
			}
			else
			{
				if (this.CurrentItem.Id != "Armor_Army_1")
				{
					this.ChooseCarouselItem(new ShopNGUIController.ShopItem("Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory), true);
				}
				toPhase = ShopNGUIController.TutorialPhase.EquipArmor;
			}
			break;
		case ShopNGUIController.TutorialPhase.SelectPetsCategory:
			if (this.superCategoriesButtonController.currentBtnName != "Pets")
			{
				toPhase = ShopNGUIController.TutorialPhase.SelectPetsCategory;
			}
			else
			{
				toPhase = ShopNGUIController.TutorialPhase.ShowEggsHint;
			}
			break;
		case ShopNGUIController.TutorialPhase.ShowEggsHint:
			if (this.m_shouldMoveToLeaveState)
			{
				this.TutorialPhasePassed = ShopNGUIController.TutorialPhase.LeaveArmory;
				toPhase = ShopNGUIController.TutorialPhase.LeaveArmory;
				this.backButton.isEnabled = true;
				this.unlockButton.isEnabled = true;
			}
			else
			{
				toPhase = ShopNGUIController.TutorialPhase.ShowEggsHint;
			}
			break;
		}
		CoroutineRunner.Instance.StartCoroutine(this.ToTutorialPhaseCoroutine(toPhase));
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x0017E170 File Offset: 0x0017C370
	private IEnumerator ToTutorialPhaseCoroutine(ShopNGUIController.TutorialPhase toPhase)
	{
		this.TutorialStopBlinking();
		if (this._tutorialCurrentState != null)
		{
			this._tutorialCurrentState.StageAct(ShopNGUIController.TutorialStageTrigger.Exit);
			if (this._tutorialCurrentState.ForStage < (ShopNGUIController.TutorialPhase)this._tutorialHintsContainer.transform.childCount && this._tutorialCurrentState.ForStage != ShopNGUIController.TutorialPhase.ShowEggsHint)
			{
				this._tutorialHintsContainer.transform.GetChild((int)this._tutorialCurrentState.ForStage).gameObject.SetActive(false);
			}
			if (this._tutorialCurrentState.ForStage >= ShopNGUIController.TutorialPhase.ShowEggsHint && (this.superCategoriesButtonController.currentBtnName != ShopNGUIController.Supercategory.Pets.ToString() || this.backButton.isEnabled))
			{
				this._tutorialHintsContainer.transform.GetChild(8).gameObject.SetActive(false);
			}
		}
		this._tutorialCurrentState = this._tutorialStates.FirstOrDefault((ShopNGUIController.TutorialState d) => d.ForStage == toPhase);
		if (this._tutorialCurrentState == null)
		{
			throw new Exception(string.Format("undefined tutorial state: {0}", toPhase));
		}
		this._tutorialCurrentState.StageAct(ShopNGUIController.TutorialStageTrigger.Enter);
		AnalyticsConstants.TutorialState? state = null;
		if (this.TutorialPhaseLastViewed == null)
		{
			state = new AnalyticsConstants.TutorialState?(AnalyticsConstants.TutorialState.Open_Shop);
			this.TutorialPhaseLastViewed = new ShopNGUIController.TutorialPhase?(ShopNGUIController.TutorialPhase.SelectWeaponCategory);
		}
		else if (this.TutorialPhaseLastViewed.Value < toPhase)
		{
			this.TutorialPhaseLastViewed = new ShopNGUIController.TutorialPhase?(toPhase);
			switch (toPhase)
			{
			case ShopNGUIController.TutorialPhase.SelectRifle:
				state = new AnalyticsConstants.TutorialState?(AnalyticsConstants.TutorialState.Category_Sniper);
				break;
			case ShopNGUIController.TutorialPhase.SelectWearCategory:
				state = new AnalyticsConstants.TutorialState?(AnalyticsConstants.TutorialState.Equip_Sniper);
				break;
			case ShopNGUIController.TutorialPhase.EquipArmor:
				state = new AnalyticsConstants.TutorialState?(AnalyticsConstants.TutorialState.Category_Armor);
				break;
			case ShopNGUIController.TutorialPhase.LeaveArmory:
				state = new AnalyticsConstants.TutorialState?(AnalyticsConstants.TutorialState.Equip_Armor);
				break;
			}
		}
		if (state != null)
		{
			AnalyticsStuff.Tutorial(state.Value, 0);
		}
		if (this._tutorialCurrentState.ForStage < (ShopNGUIController.TutorialPhase)this._tutorialHintsContainer.transform.childCount && this._tutorialCurrentState.ForStage != ShopNGUIController.TutorialPhase.ShowEggsHint)
		{
			this._tutorialHintsContainer.transform.GetChild((int)this._tutorialCurrentState.ForStage).gameObject.SetActive(true);
		}
		if (this._tutorialCurrentState.ForStage < ShopNGUIController.TutorialPhase.ShowEggsHint)
		{
			yield break;
		}
		if (!(this.superCategoriesButtonController.currentBtnName == ShopNGUIController.Supercategory.Pets.ToString()))
		{
			yield break;
		}
		if (!this.backButton.isEnabled)
		{
			this._tutorialHintsContainer.transform.GetChild(8).gameObject.SetActive(true);
			yield break;
		}
		yield break;
	}

	// Token: 0x0600464B RID: 17995 RVA: 0x0017E19C File Offset: 0x0017C39C
	private void TutorialSelectWeaponCategory(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			BtnCategory btnCategory = this.superCategoriesButtonController.buttons.First((BtnCategory b) => b.btnName == "Weapons");
			UISprite component = btnCategory.gameObject.GetChildGameObject("Blink", true).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 0.004f, 0.5f));
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x0600464C RID: 17996 RVA: 0x0017E22C File Offset: 0x0017C42C
	private void TutorialSelectSniperSection(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			GameObject gameObject = this.TransformOfButtonForCategory(ShopNGUIController.CategoryNames.SniperCategory).gameObject;
			UIWidget component = gameObject.GetChildGameObject("Pressed", false).GetComponent<UIWidget>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 0.004f, 0.5f));
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x0600464D RID: 17997 RVA: 0x0017E298 File Offset: 0x0017C498
	private void TutorialSelectRifle(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			GameObject gameObject = this.TransformOfButtonForCategory(ShopNGUIController.CategoryNames.SniperCategory).gameObject;
			UIWidget component = gameObject.GetChildGameObject("Pressed", false).GetComponent<UIWidget>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 1f, 1f, 0.004f, 0.5f));
			ArmoryCell armoryCellByItemId = this.GetArmoryCellByItemId(WeaponTags.HunterRifleTag);
			UISprite component2 = armoryCellByItemId.gameObject.GetChildGameObject("Blink", false).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component2, 0.004f, 1f, 0.004f, 0.5f));
			this.gridScrollView.SetDragAmount(0f, 0f, false);
			this.gridScrollView.enabled = false;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(false);
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.gridScrollView.enabled = true;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(true);
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x0600464E RID: 17998 RVA: 0x0017E3A8 File Offset: 0x0017C5A8
	private void TutorialEquipRifle(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			GameObject gameObject = this.TransformOfButtonForCategory(ShopNGUIController.CategoryNames.SniperCategory).gameObject;
			UIWidget component = gameObject.GetChildGameObject("Pressed", false).GetComponent<UIWidget>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 1f, 1f, 0.004f, 0.5f));
			this.gridScrollView.enabled = false;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(false);
			UISprite component2 = this.equipButton.gameObject.GetChildGameObject("Blink", false).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component2, 0.004f, 1f, 0.004f, 0.5f));
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.gridScrollView.enabled = true;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(true);
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x0600464F RID: 17999 RVA: 0x0017E498 File Offset: 0x0017C698
	private void TutorialSelectWearCategory(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			BtnCategory btnCategory = this.superCategoriesButtonController.buttons.First((BtnCategory b) => b.btnName == "Wear");
			UISprite component = btnCategory.gameObject.GetChildGameObject("Blink", true).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 0.004f, 0.5f));
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x06004650 RID: 18000 RVA: 0x0017E528 File Offset: 0x0017C728
	private void TutorialEquipArmor(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			this.ChooseCarouselItem(new ShopNGUIController.ShopItem("Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory), true);
			this.scrollViewPanel.GetComponent<UIScrollView>().enabled = false;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(false);
			UISprite component = this.equipButton.gameObject.GetChildGameObject("Blink", false).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 0.004f, 0.5f));
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
			this.scrollViewPanel.GetComponent<UIScrollView>().enabled = true;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(true);
		}
	}

	// Token: 0x06004651 RID: 18001 RVA: 0x0017E5F0 File Offset: 0x0017C7F0
	private void TutorialSelectArmorSection(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			GameObject gameObject = this.TransformOfButtonForCategory(ShopNGUIController.CategoryNames.ArmorCategory).gameObject;
			UIWidget component = gameObject.GetChildGameObject("Pressed", false).GetComponent<UIWidget>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 1f, 0.5f));
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(false);
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(true);
		}
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x0017E688 File Offset: 0x0017C888
	private void TutorialSelectPetsCategory(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			BtnCategory btnCategory = this.superCategoriesButtonController.buttons.First((BtnCategory b) => b.btnName == "Pets");
			UISprite component = btnCategory.gameObject.GetChildGameObject("Blink", true).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 0.004f, 0.5f));
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(this.CurrentCategory == ShopNGUIController.CategoryNames.ArmorCategory);
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x06004653 RID: 18003 RVA: 0x0017E738 File Offset: 0x0017C938
	private void TutorialShowEggsHint(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			if (!this.m_startedLeaveArmoryStateTimer)
			{
				base.StartCoroutine(this.WaitAndSetShouldMoveToLeaveState());
				this.m_startedLeaveArmoryStateTimer = true;
			}
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
			this.scrollViewPanel.GetComponent<UIScrollView>().enabled = true;
			this.gridScrollView.verticalScrollBar.gameObject.SetActiveSafeSelf(true);
		}
	}

	// Token: 0x06004654 RID: 18004 RVA: 0x0017E7A4 File Offset: 0x0017C9A4
	private void TutorialLeaveArmory(ShopNGUIController.TutorialStageTrigger trigger)
	{
		if (trigger == ShopNGUIController.TutorialStageTrigger.Enter)
		{
			UISprite component = this.backButton.gameObject.GetChildGameObject("Blink", false).GetComponent<UISprite>();
			CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(component, 0.004f, 1f, 0.004f, 0.5f));
		}
		else if (trigger == ShopNGUIController.TutorialStageTrigger.Exit)
		{
			this.TutorialStopBlinking();
		}
	}

	// Token: 0x06004655 RID: 18005 RVA: 0x0017E80C File Offset: 0x0017CA0C
	private IEnumerator WaitAndSetShouldMoveToLeaveState()
	{
		yield return new WaitForRealSeconds(3.5f);
		this.m_shouldMoveToLeaveState = true;
		this.UpdateTutorialState();
		yield break;
	}

	// Token: 0x06004656 RID: 18006 RVA: 0x0017E828 File Offset: 0x0017CA28
	private void TutorialStopBlinking()
	{
		foreach (CancellationTokenSource cancellationTokenSource in this._tutorialTokensSources.ToArray())
		{
			cancellationTokenSource.Cancel();
		}
	}

	// Token: 0x06004657 RID: 18007 RVA: 0x0017E860 File Offset: 0x0017CA60
	private void TutorialStopBlinking_EggIfno()
	{
		foreach (CancellationTokenSource cancellationTokenSource in this._tutorialEggsInfoTokensSources.ToArray())
		{
			cancellationTokenSource.Cancel();
		}
	}

	// Token: 0x06004658 RID: 18008 RVA: 0x0017E898 File Offset: 0x0017CA98
	private void TutorialStopBlinking_PetUpgrade()
	{
		foreach (CancellationTokenSource cancellationTokenSource in this._tutorialPetUpgradeTokensSources.ToArray())
		{
			cancellationTokenSource.Cancel();
		}
	}

	// Token: 0x06004659 RID: 18009 RVA: 0x0017E8D0 File Offset: 0x0017CAD0
	private IEnumerator BlinkAlpha(UIWidget widget, float fromAlpha, float toAlpha, float defaultAlpha = 0.004f, float blinkTimeInSeconds = 0.5f)
	{
		float elapsedTime = 0f;
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		this._tutorialTokensSources.Add(tokenSource);
		while (!tokenSource.Token.IsCancellationRequested)
		{
			if (elapsedTime >= blinkTimeInSeconds)
			{
				elapsedTime = 0f;
				float tmp = fromAlpha;
				fromAlpha = toAlpha;
				toAlpha = tmp;
			}
			widget.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / blinkTimeInSeconds);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		this._tutorialTokensSources.Remove(tokenSource);
		widget.alpha = defaultAlpha;
		yield break;
		yield break;
	}

	// Token: 0x0600465A RID: 18010 RVA: 0x0017E938 File Offset: 0x0017CB38
	private IEnumerator BlinkAlpha_PetUpgrades(UIWidget widget, float fromAlpha, float toAlpha, float defaultAlpha = 0.004f, float blinkTimeInSeconds = 0.5f)
	{
		float elapsedTime = 0f;
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		this._tutorialPetUpgradeTokensSources.Add(tokenSource);
		while (!tokenSource.Token.IsCancellationRequested)
		{
			if (elapsedTime >= blinkTimeInSeconds)
			{
				elapsedTime = 0f;
				float tmp = fromAlpha;
				fromAlpha = toAlpha;
				toAlpha = tmp;
			}
			widget.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / blinkTimeInSeconds);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		this._tutorialPetUpgradeTokensSources.Remove(tokenSource);
		widget.alpha = defaultAlpha;
		yield break;
		yield break;
	}

	// Token: 0x0600465B RID: 18011 RVA: 0x0017E9A0 File Offset: 0x0017CBA0
	private IEnumerator BlinkAlpha_EggInfo(UIWidget widget, float fromAlpha, float toAlpha, float defaultAlpha = 0.004f, float blinkTimeInSeconds = 0.5f)
	{
		float elapsedTime = 0f;
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		this._tutorialEggsInfoTokensSources.Add(tokenSource);
		while (!tokenSource.Token.IsCancellationRequested)
		{
			if (elapsedTime >= blinkTimeInSeconds)
			{
				elapsedTime = 0f;
				float tmp = fromAlpha;
				fromAlpha = toAlpha;
				toAlpha = tmp;
			}
			widget.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / blinkTimeInSeconds);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		this._tutorialEggsInfoTokensSources.Remove(tokenSource);
		widget.alpha = defaultAlpha;
		yield break;
		yield break;
	}

	// Token: 0x0600465C RID: 18012 RVA: 0x0017EA08 File Offset: 0x0017CC08
	private IEnumerator BlinkColor(Func<Color> getter, Action<Color> setter, Color fromColor, Color toColor, float blinkTimeInSeconds = 0.5f)
	{
		float elapsedTime = 0f;
		Color defaultColor = getter();
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		this._tutorialTokensSources.Add(tokenSource);
		while (!tokenSource.Token.IsCancellationRequested)
		{
			if (elapsedTime >= blinkTimeInSeconds)
			{
				elapsedTime = 0f;
				Color tmp = fromColor;
				fromColor = toColor;
				toColor = tmp;
			}
			setter(Color.Lerp(fromColor, toColor, elapsedTime / blinkTimeInSeconds));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		this._tutorialTokensSources.Remove(tokenSource);
		setter(defaultColor);
		yield break;
		yield break;
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x0017EA70 File Offset: 0x0017CC70
	private void UpdateExtTutorial()
	{
		if (!ShopNGUIController.GuiActive)
		{
			return;
		}
		this.TutorialStopBlinking();
		this.TutorialStopBlinking_EggIfno();
		this.TutorialStopBlinking_PetUpgrade();
		if (PlayerPrefs.GetInt("tutorial_info_hint_viewed", 0) == 0)
		{
			if (this.superCategoriesButtonController.currentBtnName == "Weapons")
			{
				GameObject childGameObject = this.infoButton.gameObject.GetChildGameObject("Blink", false);
				if (childGameObject != null)
				{
					CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha(childGameObject.GetComponent<UISprite>(), 0.004f, 1f, 0.004f, 0.5f));
				}
				else
				{
					CoroutineRunner.Instance.StartCoroutine(this.BlinkColor(() => this.infoButton.defaultColor, delegate(Color c)
					{
						this.infoButton.defaultColor = c;
					}, this.infoButton.defaultColor, Color.green, 0.5f));
				}
				GameObject childGameObject2 = this._tutorialHintsExtContainer.GetChildGameObject("OpenInfoHint", true);
				childGameObject2.SetActive(true);
				if (this.m_tutorialInfoBtnED == null)
				{
					this.m_tutorialInfoBtnED = new EventDelegate(new EventDelegate.Callback(this.TutorialOnInfoButtonClicked));
				}
				if (!this.infoButton.onClick.Contains(this.m_tutorialInfoBtnED))
				{
					this.infoButton.onClick.Add(this.m_tutorialInfoBtnED);
				}
				if (this.infoScreen != null)
				{
					this.TutorialOnInfoButtonClicked();
				}
			}
			else
			{
				this.TutorialStopBlinking();
				GameObject childGameObject3 = this._tutorialHintsExtContainer.GetChildGameObject("OpenInfoHint", true);
				childGameObject3.SetActive(false);
				if (this.infoButton.onClick.Contains(this.m_tutorialInfoBtnED))
				{
					this.infoButton.onClick.Remove(this.m_tutorialInfoBtnED);
				}
			}
		}
		else if (Storager.getInt("tutorial_button_try_highlighted", false) == 0 && ExperienceController.sharedController.currentLevel > 1)
		{
			GameObject childGameObject4 = this._tutorialHintsExtContainer.GetChildGameObject("TryHint", true);
			if (this.tryGun.gameObject.activeInHierarchy && this.superCategoriesButtonController.currentBtnName == "Weapons" && this.infoScreen == null)
			{
				childGameObject4.SetActive(true);
				Storager.SubscribeToChanged("tutorial_button_try_highlighted", new Action(this.OnKEY_BTN_TRY_HIGHLIGHTED_Changed));
			}
			else
			{
				childGameObject4.SetActive(false);
			}
		}
		if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1 && Storager.getInt("Shop.Tutorial.KEY_TUTORIAL_EGG_INFO_HINT_VIEWED", false) == 0)
		{
			if (this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory)
			{
				GameObject childGameObject5 = this.infoButton.gameObject.GetChildGameObject("Blink", false);
				CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha_EggInfo(childGameObject5.GetComponent<UISprite>(), 0.004f, 1f, 0.004f, 0.5f));
				GameObject childGameObject6 = this._tutorialHintsExtContainer.GetChildGameObject("PetsInfo", true);
				childGameObject6.SetActive(true);
				if (this.m_tutorialInfoBtnED_Eggs == null)
				{
					this.m_tutorialInfoBtnED_Eggs = new EventDelegate(new EventDelegate.Callback(this.TutorialOnInfoEggsButtonClicked));
				}
				if (!this.infoButton.onClick.Contains(this.m_tutorialInfoBtnED_Eggs))
				{
					this.infoButton.onClick.Add(this.m_tutorialInfoBtnED_Eggs);
				}
				if (this.rentScreenPoint.FindChild("PetInfoScreen(Clone)") != null)
				{
					this.TutorialOnInfoEggsButtonClicked();
				}
			}
			else
			{
				this.TutorialStopBlinking_EggIfno();
				GameObject childGameObject7 = this._tutorialHintsExtContainer.GetChildGameObject("PetsInfo", true);
				childGameObject7.SetActive(false);
				if (this.infoButton.onClick.Contains(this.m_tutorialInfoBtnED_Eggs))
				{
					this.infoButton.onClick.Remove(this.m_tutorialInfoBtnED_Eggs);
				}
			}
		}
		try
		{
			if (Storager.getInt("Shop.Tutorial.KEY_TUTORIAL_PET_UPRADE_HINT_VIEWED", false) == 0)
			{
				if (this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory && Singleton<PetsManager>.Instance.PlayerPets.Count > 0 && this.CurrentItem.Id != null && PetsInfo.info.ContainsKey(this.CurrentItem.Id) && this.upgradeButton.gameObject.activeInHierarchy && this.upgradeButton.isEnabled)
				{
					GameObject childGameObject8 = this.upgradeButton.gameObject.GetChildGameObject("Blink", false);
					CoroutineRunner.Instance.StartCoroutine(this.BlinkAlpha_PetUpgrades(childGameObject8.GetComponent<UISprite>(), 0.004f, 1f, 0.004f, 0.5f));
					GameObject childGameObject9 = this._tutorialHintsExtContainer.GetChildGameObject("PetsUpgrade", true);
					childGameObject9.SetActive(true);
					if (!this.m_petUpgradeHideCoroutineStarted)
					{
						base.StartCoroutine(this.WaitAndRemovePetUpgradeHint());
					}
				}
				else
				{
					this.RemovePetUpgradeHint();
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateExitTutorial pet upgrade hint: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x0017EF7C File Offset: 0x0017D17C
	private IEnumerator WaitAndRemovePetUpgradeHint()
	{
		this.m_petUpgradeHideCoroutineStarted = true;
		yield return new WaitForRealSeconds(5f);
		this.RemovePetUpgradeHint();
		Storager.setInt("Shop.Tutorial.KEY_TUTORIAL_PET_UPRADE_HINT_VIEWED", 1, false);
		yield break;
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x0017EF98 File Offset: 0x0017D198
	private void RemovePetUpgradeHint()
	{
		this.TutorialStopBlinking_PetUpgrade();
		GameObject childGameObject = this._tutorialHintsExtContainer.GetChildGameObject("PetsUpgrade", true);
		childGameObject.SetActive(false);
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x0017EFC4 File Offset: 0x0017D1C4
	private void OnKEY_BTN_TRY_HIGHLIGHTED_Changed()
	{
		if (Storager.getInt("tutorial_button_try_highlighted", false) > 0)
		{
			Storager.UnSubscribeToChanged("tutorial_button_try_highlighted", new Action(this.OnKEY_BTN_TRY_HIGHLIGHTED_Changed));
			GameObject childGameObject = this._tutorialHintsExtContainer.GetChildGameObject("TryHint", true);
			childGameObject.SetActive(false);
		}
	}

	// Token: 0x06004661 RID: 18017 RVA: 0x0017F014 File Offset: 0x0017D214
	private void TutorialOnInfoButtonClicked()
	{
		this.TutorialStopBlinking();
		this.infoButton.onClick.Remove(this.m_tutorialInfoBtnED);
		GameObject childGameObject = this._tutorialHintsExtContainer.GetChildGameObject("OpenInfoHint", true);
		childGameObject.SetActive(false);
		PlayerPrefs.SetInt("tutorial_info_hint_viewed", 1);
	}

	// Token: 0x06004662 RID: 18018 RVA: 0x0017F064 File Offset: 0x0017D264
	private void TutorialOnInfoEggsButtonClicked()
	{
		this.TutorialStopBlinking_EggIfno();
		this.infoButton.onClick.Remove(this.m_tutorialInfoBtnED_Eggs);
		GameObject childGameObject = this._tutorialHintsExtContainer.GetChildGameObject("PetsInfo", true);
		childGameObject.SetActive(false);
		ShopNGUIController.SetEggInfoHintViewed();
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x0017F0AC File Offset: 0x0017D2AC
	private IEnumerator WaitCoroutine(Func<bool> condition, Action afterTrue)
	{
		while (!condition())
		{
			yield return null;
		}
		afterTrue();
		yield break;
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x0017F0DC File Offset: 0x0017D2DC
	private void TutorialDisableHints()
	{
		for (int i = 0; i < this._tutorialHintsContainer.transform.childCount; i++)
		{
			this._tutorialHintsContainer.transform.GetChild(i).gameObject.SetActive(false);
		}
		for (int j = 0; j < this._tutorialHintsExtContainer.transform.childCount; j++)
		{
			this._tutorialHintsContainer.transform.GetChild(j).gameObject.SetActive(false);
		}
	}

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06004665 RID: 18021 RVA: 0x0017F164 File Offset: 0x0017D364
	// (set) Token: 0x06004666 RID: 18022 RVA: 0x0017F16C File Offset: 0x0017D36C
	public static bool ShowArmor
	{
		get
		{
			return ShopNGUIController._showArmorValue;
		}
		private set
		{
			if (ShopNGUIController._showArmorValue == value)
			{
				return;
			}
			ShopNGUIController._showArmorValue = value;
			Action showArmorChanged = ShopNGUIController.ShowArmorChanged;
			if (showArmorChanged != null)
			{
				showArmorChanged();
			}
		}
	}

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x06004667 RID: 18023 RVA: 0x0017F1A0 File Offset: 0x0017D3A0
	// (set) Token: 0x06004668 RID: 18024 RVA: 0x0017F1A8 File Offset: 0x0017D3A8
	public static bool ShowHat
	{
		get
		{
			return ShopNGUIController._showHatValue;
		}
		private set
		{
			if (ShopNGUIController._showHatValue == value)
			{
				return;
			}
			ShopNGUIController._showHatValue = value;
			Action showArmorChanged = ShopNGUIController.ShowArmorChanged;
			if (showArmorChanged != null)
			{
				showArmorChanged();
			}
		}
	}

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x06004669 RID: 18025 RVA: 0x0017F1DC File Offset: 0x0017D3DC
	// (set) Token: 0x0600466A RID: 18026 RVA: 0x0017F214 File Offset: 0x0017D414
	public static bool ShowWear
	{
		get
		{
			if (ShopNGUIController._showWearValue == null)
			{
				ShopNGUIController._showWearValue = new bool?(PlayerPrefs.GetInt("ShowWearKeySetting", 0).ToBool());
			}
			return ShopNGUIController._showWearValue.Value;
		}
		private set
		{
			if (ShopNGUIController._showWearValue != null && ShopNGUIController._showWearValue.Value == value)
			{
				return;
			}
			ShopNGUIController._showWearValue = new bool?(value);
			PlayerPrefs.SetInt("ShowWearKeySetting", value.ToInt());
			Action showWearChanged = ShopNGUIController.ShowWearChanged;
			if (showWearChanged != null)
			{
				showWearChanged();
			}
		}
	}

	// Token: 0x0600466B RID: 18027 RVA: 0x0017F270 File Offset: 0x0017D470
	public void SetArmorVisible(bool isVisible)
	{
		ShopNGUIController.ShowArmor = isVisible;
		ShopNGUIController.SetPersArmorVisible(this.armorPoint);
		PlayerPrefs.SetInt("ShowArmorKeySetting", ShopNGUIController.ShowArmor.ToInt());
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x0017F2A4 File Offset: 0x0017D4A4
	public void SetHatVisible(bool isVisible)
	{
		ShopNGUIController.ShowHat = isVisible;
		ShopNGUIController.SetPersHatVisible(this.hatPoint);
		PlayerPrefs.SetInt("ShowHatKeySetting", ShopNGUIController.ShowWear.ToInt());
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x0017F2D8 File Offset: 0x0017D4D8
	public void SetWearVisible(bool isVisible)
	{
		if (isVisible == ShopNGUIController.ShowWear)
		{
			return;
		}
		ShopNGUIController.ShowWear = isVisible;
		PlayerPrefs.SetInt("ShowWearKeySetting", ShopNGUIController.ShowWear.ToInt());
		ShopNGUIController.SetRenderersVisibleFromPoint(this.characterInterface.hatPoint, isVisible);
		ShopNGUIController.SetRenderersVisibleFromPoint(this.characterInterface.maskPoint, isVisible);
		ShopNGUIController.SetRenderersVisibleFromPoint(this.characterInterface.leftBootPoint, isVisible);
		ShopNGUIController.SetRenderersVisibleFromPoint(this.characterInterface.rightBootPoint, isVisible);
		ShopNGUIController.SetRenderersVisibleFromPoint(this.characterInterface.capePoint, isVisible);
	}

	// Token: 0x0600466E RID: 18030 RVA: 0x0017F360 File Offset: 0x0017D560
	public static void SetPersArmorVisible(Transform armorPoint)
	{
		ShopNGUIController.SetRenderersVisibleFromPoint(armorPoint, ShopNGUIController.ShowArmor);
		if (armorPoint.childCount > 0)
		{
			Transform child = armorPoint.GetChild(0);
			ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(component.leftBone, ShopNGUIController.ShowArmor);
				}
				if (component.rightBone != null)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(component.rightBone, ShopNGUIController.ShowArmor);
				}
			}
		}
	}

	// Token: 0x0600466F RID: 18031 RVA: 0x0017F3E8 File Offset: 0x0017D5E8
	public static void SetPersHatVisible(Transform hatPoint)
	{
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x0017F3F8 File Offset: 0x0017D5F8
	public void HandleShowArmorButton(UIToggle toggle)
	{
		this.SetArmorVisible(toggle.value);
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x0017F408 File Offset: 0x0017D608
	public void HandleShowHatButton(UIToggle toggle)
	{
		this.SetHatVisible(toggle.value);
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x0017F418 File Offset: 0x0017D618
	public void HandleShowWearButton(UIToggle toggle)
	{
		this.SetWearVisible(toggle.value);
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x06004673 RID: 18035 RVA: 0x0017F428 File Offset: 0x0017D628
	// (set) Token: 0x06004674 RID: 18036 RVA: 0x0017F430 File Offset: 0x0017D630
	public ShopNGUIController.ShopItem CurrentItem
	{
		get
		{
			return this.m_currentItem;
		}
		private set
		{
			this.m_currentItem = value;
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x06004675 RID: 18037 RVA: 0x0017F43C File Offset: 0x0017D63C
	// (set) Token: 0x06004676 RID: 18038 RVA: 0x0017F444 File Offset: 0x0017D644
	public bool IsExiting { get; private set; }

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x06004677 RID: 18039 RVA: 0x0017F450 File Offset: 0x0017D650
	// (set) Token: 0x06004678 RID: 18040 RVA: 0x0017F458 File Offset: 0x0017D658
	public ShopNGUIController.CategoryNames CurrentCategory { get; private set; }

	// Token: 0x06004679 RID: 18041 RVA: 0x0017F464 File Offset: 0x0017D664
	public static ShopNGUIController.CategoryNames MapShopCategoryToItemCategory(ShopNGUIController.CategoryNames category)
	{
		if (category == ShopNGUIController.CategoryNames.BestWeapons || category == ShopNGUIController.CategoryNames.BestWear || category == ShopNGUIController.CategoryNames.BestGadgets)
		{
			Debug.LogErrorFormat("MapShopCategoryToItemCategory - best category", new object[0]);
			return category;
		}
		if (category != ShopNGUIController.CategoryNames.SkinsCategoryEditor && category != ShopNGUIController.CategoryNames.SkinsCategoryMale && category != ShopNGUIController.CategoryNames.SkinsCategoryFemale && category != ShopNGUIController.CategoryNames.SkinsCategorySpecial && category != ShopNGUIController.CategoryNames.SkinsCategoryPremium)
		{
			if (category == ShopNGUIController.CategoryNames.LeagueHatsCategory)
			{
				return ShopNGUIController.CategoryNames.HatsCategory;
			}
			if (category != ShopNGUIController.CategoryNames.LeagueSkinsCategory)
			{
				return category;
			}
		}
		return ShopNGUIController.CategoryNames.SkinsCategory;
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x0017F500 File Offset: 0x0017D700
	public static ShopNGUIController.CategoryNames RealCategoryToPseudoCategory(ShopNGUIController.CategoryNames category, string itemId)
	{
		if (Wear.LeagueForWear(itemId, category) > 0 || itemId == "league1_hat_hitman")
		{
			return ShopNGUIController.CategoryNames.LeagueHatsCategory;
		}
		return category;
	}

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x0600467B RID: 18043 RVA: 0x0017F534 File Offset: 0x0017D734
	public Camera Camera3D
	{
		get
		{
			Camera result;
			if (this.ourCameras != null)
			{
				result = this.ourCameras.FirstOrDefault((Camera c) => c.name.Equals("Camera3D"));
			}
			else
			{
				result = null;
			}
			return result;
		}
	}

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x0600467C RID: 18044 RVA: 0x0017F570 File Offset: 0x0017D770
	// (set) Token: 0x0600467D RID: 18045 RVA: 0x0017F578 File Offset: 0x0017D778
	public string GunThatWeUsedInPolygon
	{
		get
		{
			return this._gunThatWeUsedInPolygon;
		}
		set
		{
			this._gunThatWeUsedInPolygon = value;
		}
	}

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x0600467E RID: 18046 RVA: 0x0017F584 File Offset: 0x0017D784
	public bool EggWindowIsOpened
	{
		get
		{
			EggHatchingWindowController[] componentsInChildren = this.rentScreenPoint.GetComponentsInChildren<EggHatchingWindowController>();
			return componentsInChildren != null && componentsInChildren.Length > 0;
		}
	}

	// Token: 0x0600467F RID: 18047 RVA: 0x0017F5AC File Offset: 0x0017D7AC
	public void HandleBuyButton()
	{
		if (this.IsExiting)
		{
			Debug.LogErrorFormat("HandleBuyButton: IsExiting", new object[0]);
			return;
		}
		this.BuyOrUpgradeWeapon(false);
	}

	// Token: 0x06004680 RID: 18048 RVA: 0x0017F5D4 File Offset: 0x0017D7D4
	public void HandleUpgradeButton()
	{
		if (this.IsExiting)
		{
			Debug.LogErrorFormat("HandleUpgradeButton: IsExiting", new object[0]);
			return;
		}
		this.BuyOrUpgradeWeapon(true);
	}

	// Token: 0x06004681 RID: 18049 RVA: 0x0017F5FC File Offset: 0x0017D7FC
	public void HandleRenamePetButton()
	{
		if (this.rentScreenPoint.childCount == 0 && this.CurrentItem != null && this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory && this.CurrentItem.Id != null)
		{
			try
			{
				Transform transform = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("NguiWindows/PetWindows"));
				transform.parent = this.rentScreenPoint;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				EggHatchingWindowController component = transform.GetComponent<EggHatchingWindowController>();
				component.SetRenameMode();
				component.SetPetId(this.CurrentItem.Id);
				component.ReplaceEggWithPet(this.CurrentItem.Id);
				component.SetPetsNameToInput(Singleton<PetsManager>.Instance.GetPlayerPet(this.CurrentItem.Id).PetName);
				ShopNGUIController.ShopItem itemBefore = this.CurrentItem;
				component.OnCloseCustomAction = delegate()
				{
					if (this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory && itemBefore != null && !itemBefore.Id.IsNullOrEmpty())
					{
						this.ChooseItem(itemBefore, true, false);
					}
				};
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in HandleRenamePetButton: {0}", new object[]
				{
					ex
				});
				this.rentScreenPoint.DestroyChildren();
			}
		}
	}

	// Token: 0x06004682 RID: 18050 RVA: 0x0017F73C File Offset: 0x0017D93C
	private bool IsGridEmpty()
	{
		Transform child = this.itemsGrid.GetChild(0);
		if (child != null)
		{
			ArmoryCell component = child.GetComponent<ArmoryCell>();
			if (component != null)
			{
				return component.isEmpty || !component.gameObject.activeSelf;
			}
			Debug.LogErrorFormat("IsGridEmpty: armoryCell == null", new object[0]);
		}
		else
		{
			Debug.LogErrorFormat("IsGridEmpty: firstCel == null", new object[0]);
		}
		return true;
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x0017F7B8 File Offset: 0x0017D9B8
	private bool NeedToShowPropertiesInCategory(ShopNGUIController.CategoryNames category)
	{
		if (ShopNGUIController.IsBestCategory(this.CurrentCategory) && this.IsGridEmpty())
		{
			return false;
		}
		if (category == ShopNGUIController.CategoryNames.PetsCategory && Singleton<PetsManager>.Instance.PlayerPets.Count == 0)
		{
			return false;
		}
		bool result;
		if (this.propertiesShownInCategory.TryGetValue(category, out result))
		{
			return result;
		}
		Debug.LogErrorFormat("NeedToShowPropertiesInCategory: not found value for category {0}", new object[]
		{
			category
		});
		return false;
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x0017F834 File Offset: 0x0017DA34
	public void HandleSuperIncubatorButton()
	{
		if (this.rentScreenPoint.childCount == 0 && this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("NguiWindows/SuperIncubatorWindow"));
			transform.parent = this.rentScreenPoint;
			transform.localPosition = Vector3.zero;
			transform.localScale = new Vector3(1f, 1f, 1f);
			transform.GetComponent<SuperIncubatorWindowController>().BuyAction = new Action(this.HandleSuperIncubatorBuyButton);
		}
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x0017F8BC File Offset: 0x0017DABC
	public void EquipPetAndUpdate(string petId)
	{
		if (ShopNGUIController.GuiActive)
		{
			this.StopPetAnimation();
		}
		ShopNGUIController.EquipPet(petId);
		if (ShopNGUIController.GuiActive)
		{
			this.UpdateIcons(false);
		}
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x0017F8E8 File Offset: 0x0017DAE8
	public void HandleEquipButton()
	{
		if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
		{
			string text = WeaponManager.LastBoughtTag(this.CurrentItem.Id, null);
			if (text == null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(this.CurrentItem.Id))
			{
				text = this.CurrentItem.Id;
			}
			if (text == null)
			{
				return;
			}
			string prefabName = ItemDb.GetByTag(text).PrefabName;
			Weapon w = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault((Weapon weapon) => weapon.weaponPrefab.nameNoClone() == prefabName);
			WeaponManager.sharedManager.EquipWeapon(w, true, true);
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
			if (this.equipAction != null)
			{
				this.equipAction(text);
			}
			this.UpdateIcons(false);
		}
		else if (ShopNGUIController.IsWearCategory(this.CurrentItem.Category))
		{
			string text2 = WeaponManager.LastBoughtTag(this.CurrentItem.Id, null);
			if (!string.IsNullOrEmpty(text2))
			{
				this.EquipWear(text2);
			}
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
			{
				if (this.InTrainingAfterNoviceArmorRemoved)
				{
					this.InTrainingAfterNoviceArmorRemoved = false;
					this.HandleOffersUpdated();
				}
			}
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.SetSkinAsCurrent(this.CurrentItem.Id);
			this.UpdateIcons(true);
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			ShopNGUIController.EquipWeaponSkinWrapper(this.CurrentItem.Id);
			this.UpdateIcons(false);
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.EquipPetAndUpdate(this.CurrentItem.Id);
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
		{
			ShopNGUIController.EquipGadget(this.CurrentItem.Id, (GadgetInfo.GadgetCategory)this.CurrentItem.Category);
			this.UpdateIcons(false);
		}
		this.UpdateButtons();
		this.UpdateTutorialState();
	}

	// Token: 0x06004687 RID: 18055 RVA: 0x0017FB18 File Offset: 0x0017DD18
	public void HandleUnequipButton()
	{
		if (ShopNGUIController.IsWearCategory(this.CurrentItem.Category))
		{
			ShopNGUIController.UnequipCurrentWearInCategory(this.CurrentItem.Category, this.inGame);
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			WeaponSkinsManager.UnequipSkin(this.CurrentItem.Id);
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.StopPetAnimation();
			ShopNGUIController.UnequipPet(this.CurrentItem.Id);
			this.ChooseItem(this.CurrentItem, false, false);
		}
		this.UpdateButtons();
		this.UpdateIcons(false);
	}

	// Token: 0x06004688 RID: 18056 RVA: 0x0017FBC8 File Offset: 0x0017DDC8
	public void HandleEnableButton()
	{
		if (this.IsExiting)
		{
			Debug.LogErrorFormat("HandleEnableButton: IsExiting", new object[0]);
			return;
		}
		if (this.CurrentItem.Id == "cape_Custom")
		{
			this.BuyOrUpgradeWeapon(false);
		}
	}

	// Token: 0x06004689 RID: 18057 RVA: 0x0017FC14 File Offset: 0x0017DE14
	public void HandleCreateButton()
	{
		if (!this.inGame)
		{
			this.GoToSkinsEditor();
		}
	}

	// Token: 0x0600468A RID: 18058 RVA: 0x0017FC28 File Offset: 0x0017DE28
	public void HandleSuperIncubatorBuyButton()
	{
		ItemPrice itemPrice = ShopNGUIController.GetItemPrice("Eggs.SuperIncubatorId", ShopNGUIController.CategoryNames.EggsCategory, false, true, false);
		int price = itemPrice.Price;
		string currency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(this.mainPanel, itemPrice, delegate
		{
			Singleton<EggsManager>.Instance.AddEggsForSuperIncubator();
			AnalyticsStuff.LogSales("Eggs.SuperIncubatorId", "Eggs", false);
			SuperIncubatorWindowController componentInChildren = this.rentScreenPoint.GetComponentInChildren<SuperIncubatorWindowController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.HandleClose();
			}
		}, null, null, delegate
		{
			this.PlayPersAnimations();
		}, delegate
		{
			ShopNGUIController.SetBankCamerasEnabled();
		}, delegate
		{
			this.ShowGridOrArmorCarousel();
			this.SetOtherCamerasEnabled(false);
			this.ChooseCategory(ShopNGUIController.CategoryNames.EggsCategory, null, false);
		});
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x0017FCA8 File Offset: 0x0017DEA8
	public void HandleUnlockButton()
	{
		if (this.IsExiting)
		{
			Debug.LogErrorFormat("HandleUnlockButton: IsExiting", new object[0]);
			return;
		}
		ItemPrice itemPrice = ShopNGUIController.GetItemPrice("CustomSkinID", ShopNGUIController.CategoryNames.SkinsCategory, false, true, false);
		int priceAmount = itemPrice.Price;
		string priceCurrency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(this.mainPanel, itemPrice, delegate
		{
			if (Defs.isSoundFX)
			{
				UIPlaySound component = this.unlockButton.GetComponent<UIPlaySound>();
				if (component != null)
				{
					component.Play();
				}
			}
			if (ShopNGUIController.GunBought != null)
			{
				ShopNGUIController.GunBought();
			}
			string salesName = AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategory);
			AnalyticsStuff.LogSales(Defs.SkinsMakerInProfileBought, salesName, false);
			AnalyticsFacade.InAppPurchase(Defs.SkinsMakerInProfileBought, salesName, 1, priceAmount, priceCurrency);
			Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
			this.ReloadItemGrid(new ShopNGUIController.ShopItem("CustomSkinID", ShopNGUIController.CategoryNames.SkinsCategory));
			if (!this.inGame)
			{
				ShopNGUIController.SynchronizeAndroidPurchases("Custom skin");
			}
			if (!this.inGame)
			{
				this.GoToSkinsEditor();
			}
		}, null, null, delegate
		{
			this.PlayPersAnimations();
		}, delegate
		{
			ButtonClickSound.Instance.PlayClick();
			ShopNGUIController.SetBankCamerasEnabled();
		}, delegate
		{
			this.ShowGridOrArmorCarousel();
			this.SetOtherCamerasEnabled(false);
		});
		this.ShowLockOrPropertiesAndButtons();
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x0017FD5C File Offset: 0x0017DF5C
	public void HandleEditButton()
	{
		if (!this.inGame)
		{
			this.GoToSkinsEditor();
		}
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x0017FD70 File Offset: 0x0017DF70
	public void HandleDeleteButton()
	{
		string skinWhereDelteWasPressed = this.CurrentItem.Id;
		InfoWindowController.ShowDialogBox(LocalizationStore.Get("Key_1693"), delegate
		{
			ButtonClickSound.Instance.PlayClick();
			string currentSkinNameForPers = SkinsController.currentSkinNameForPers;
			if (skinWhereDelteWasPressed != null)
			{
				SkinsController.DeleteUserSkin(skinWhereDelteWasPressed);
				if (skinWhereDelteWasPressed.Equals(currentSkinNameForPers))
				{
					this.SetSkinAsCurrent("0");
					this.UpdateIcons(false);
				}
				this.UpdatePersSkin(SkinsController.currentSkinNameForPers ?? "0");
			}
			this.ReloadGridOrCarousel(new ShopNGUIController.ShopItem("CustomSkinID", ShopNGUIController.CategoryNames.SkinsCategory));
		}, null);
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x0017FDB8 File Offset: 0x0017DFB8
	public void HandleInfoButton()
	{
		if (this.rentScreenPoint.childCount == 0)
		{
			if (this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory)
			{
				Transform transform = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("NguiWindows/PetInfoScreen"));
				transform.parent = this.rentScreenPoint;
				transform.localPosition = Vector3.zero;
				transform.localScale = new Vector3(1f, 1f, 1f);
				this.UpdateTutorialState();
				ShopNGUIController.SetEggInfoHintViewed();
			}
			else if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category) || ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
			{
				this.infoScreen = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("ArmoryInfoScreen"));
				this.infoScreen.parent = this.rentScreenPoint;
				this.infoScreen.localPosition = Vector3.zero;
				this.infoScreen.localScale = new Vector3(1f, 1f, 1f);
				this.infoScreen.GetComponent<ArmoryInfoScreenController>().SetItem(ShopNGUIController.sharedShop.CurrentItem);
				this.UpdateTutorialState();
			}
		}
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x0017FED8 File Offset: 0x0017E0D8
	public void HandleFacebookButton()
	{
		this._isFromPromoActions = false;
		MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
		{
			FacebookController.Login(delegate
			{
				if (ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.UpdateButtons();
				}
			}, null, "Shop", null);
		}, delegate
		{
			FacebookController.Login(null, null, "Shop", null);
		});
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x0017FF2C File Offset: 0x0017E12C
	public void HandleProfileButton()
	{
		GameObject mainMenu = GameObject.Find("MainMenuNGUI");
		if (mainMenu)
		{
			mainMenu.SetActive(false);
		}
		GameObject inGameGui = GameObject.FindWithTag("InGameGUI");
		if (inGameGui)
		{
			inGameGui.SetActive(false);
		}
		GameObject networkTable = GameObject.FindWithTag("NetworkStartTableNGUI");
		if (networkTable)
		{
			networkTable.SetActive(false);
		}
		ShopNGUIController.GuiActive = false;
		Action action = delegate()
		{
		};
		ProfileController.Instance.DesiredWeaponTag = this._assignedWeaponTag;
		ProfileController.Instance.ShowInterface(new Action[]
		{
			action,
			delegate()
			{
				ShopNGUIController.GuiActive = true;
				if (mainMenu)
				{
					mainMenu.SetActive(true);
				}
				if (inGameGui)
				{
					inGameGui.SetActive(true);
				}
				if (networkTable)
				{
					networkTable.SetActive(true);
				}
			}
		});
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x0018001C File Offset: 0x0017E21C
	public static bool IsWeaponCategory(ShopNGUIController.CategoryNames c)
	{
		return c < ShopNGUIController.CategoryNames.HatsCategory;
	}

	// Token: 0x06004692 RID: 18066 RVA: 0x00180024 File Offset: 0x0017E224
	public static bool IsWearCategory(ShopNGUIController.CategoryNames c)
	{
		return Wear.wear.Keys.Contains(c);
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x00180038 File Offset: 0x0017E238
	private static string[] _CurrentWeaponSetIDs()
	{
		string[] array = new string[6];
		WeaponManager sharedManager = WeaponManager.sharedManager;
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (num >= sharedManager.playerWeapons.Count)
			{
				array[i] = null;
			}
			else
			{
				Weapon weapon = sharedManager.playerWeapons[num] as Weapon;
				if (weapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == i)
				{
					num++;
					array[i] = ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
				}
				else
				{
					array[i] = null;
				}
			}
		}
		return array;
	}

	// Token: 0x06004694 RID: 18068 RVA: 0x001800E4 File Offset: 0x0017E2E4
	public static void EquipGadget(string gadgetId, GadgetInfo.GadgetCategory category)
	{
		if (gadgetId == null)
		{
			Debug.LogError("EquipGadget gadgetId == null");
			return;
		}
		string arg = GadgetsInfo.EquippedForCategory(category);
		Storager.setString(GadgetsInfo.SNForCategory(category), gadgetId, false);
		Action<string, string, GadgetInfo.GadgetCategory> equippedGadget = ShopNGUIController.EquippedGadget;
		if (equippedGadget != null)
		{
			equippedGadget(gadgetId, arg, category);
		}
	}

	// Token: 0x06004695 RID: 18069 RVA: 0x0018012C File Offset: 0x0017E32C
	public static void EquipPet(string petId)
	{
		if (petId == null)
		{
			Debug.LogError("EquipPet pet == null");
			return;
		}
		string eqipedPetId = Singleton<PetsManager>.Instance.GetEqipedPetId();
		Singleton<PetsManager>.Instance.SetEquipedPet(petId);
		Action<string, string> equippedPet = ShopNGUIController.EquippedPet;
		if (equippedPet != null)
		{
			equippedPet(petId, eqipedPetId);
		}
	}

	// Token: 0x06004696 RID: 18070 RVA: 0x00180174 File Offset: 0x0017E374
	public static void UnequipPet(string petId)
	{
		if (petId == null)
		{
			Debug.LogError("UnequipPet petId == null");
			return;
		}
		Singleton<PetsManager>.Instance.SetEquipedPet(string.Empty);
		Action<string> unequippedPet = ShopNGUIController.UnequippedPet;
		if (unequippedPet != null)
		{
			unequippedPet(petId);
		}
	}

	// Token: 0x06004697 RID: 18071 RVA: 0x001801B4 File Offset: 0x0017E3B4
	public static void ShowAddTryGun(string gunTag, Transform point, string lr, Action<string> onPurchase = null, Action onEnterCoinsShopAdditional = null, Action onExitCoinsShopAdditional = null, Action<string> customEquipWearAction = null, bool expiredTryGun = false)
	{
		try
		{
			GameObject original = Resources.Load<GameObject>("TryGunScreen");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			TryGunScreenController component = gameObject.GetComponent<TryGunScreenController>();
			Player_move_c.SetLayerRecursively(component.gameObject, LayerMask.NameToLayer(lr));
			component.transform.parent = point;
			component.transform.localPosition = new Vector3(0f, 0f, -130f);
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			if (expiredTryGun)
			{
				WeaponManager.sharedManager.AddTryGunPromo(gunTag);
			}
			component.ItemTag = gunTag;
			component.onPurchaseCustomAction = onPurchase;
			component.onEnterCoinsShopAdditionalAction = onEnterCoinsShopAdditional;
			component.onExitCoinsShopAdditionalAction = onExitCoinsShopAdditional;
			component.customEquipWearAction = customEquipWearAction;
			component.ExpiredTryGun = expiredTryGun;
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in ShowAddTryGun: " + arg);
		}
	}

	// Token: 0x06004698 RID: 18072 RVA: 0x001802AC File Offset: 0x0017E4AC
	private static int NumberOfColumnsInArmoryGrid()
	{
		return ((double)((float)Screen.width / (float)Screen.height) >= 1.5) ? 4 : 3;
	}

	// Token: 0x06004699 RID: 18073 RVA: 0x001802E0 File Offset: 0x0017E4E0
	private void SetSliceShaderParams(ArmoryCell armoryCell)
	{
		Material material = armoryCell.capeRenderer.material;
		Material material2 = armoryCell.modelForSkin.GetComponent<MeshRenderer>().material;
		float y = this.topPointForShader.transform.localPosition.y;
		float y2 = this.bottomPointForShader.transform.localPosition.y + 5f;
		Transform parent = this.gridScrollView.transform.parent;
		material2.SetFloat("_TopBorder", parent.TransformPoint(new Vector3(0f, y, 0f)).y);
		material2.SetFloat("_BottomBorder", parent.TransformPoint(new Vector3(0f, y2, 0f)).y);
	}

	// Token: 0x0600469A RID: 18074 RVA: 0x001803AC File Offset: 0x0017E5AC
	private void StopPetAnimation()
	{
		this.petProfileAnimationRunner.StopAllCoroutines();
	}

	// Token: 0x0600469B RID: 18075 RVA: 0x001803BC File Offset: 0x0017E5BC
	private void UpdatePointsForSkinsShader()
	{
		this.topPointForShader.GetComponent<UIWidget>().ResetAndUpdateAnchors();
		this.bottomPointForShader.GetComponent<UIWidget>().ResetAndUpdateAnchors();
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x001803EC File Offset: 0x0017E5EC
	internal void ReloadItemGrid(ShopNGUIController.ShopItem itemToSet)
	{
		if (!ShopNGUIController._gridInitiallyRepositioned)
		{
			this.itemsGrid.Reposition();
		}
		int num = ShopNGUIController.NumberOfColumnsInArmoryGrid();
		UIWidget component = this.gridScrollView.transform.parent.GetComponent<UIWidget>();
		component.ResetAndUpdateAnchors();
		this.UpdatePointsForSkinsShader();
		float cellWidth = ((float)component.width - 12f - (float)num * 10f) / (float)num;
		float cellHeight = 1f;
		float scale = 1f;
		scale = cellWidth / 170f;
		cellHeight = 150f * scale;
		ArmoryCell[] array = this.itemsGrid.GetComponentsInChildren<ArmoryCell>(true) ?? new ArmoryCell[0];
		if (!ShopNGUIController._gridInitiallyRepositioned)
		{
			array.ForEach(delegate(ArmoryCell cell)
			{
				Transform transform = cell.transform;
				transform.localScale = Vector3.one;
				transform.GetChild(0).localScale = new Vector3(scale, scale, scale);
				cell.GetComponent<BoxCollider>().size = new Vector2(cellWidth, cellHeight);
			});
			ShopNGUIController._gridInitiallyRepositioned = true;
		}
		List<ShopNGUIController.ShopItem> itemNamesList = ShopNGUIController.GetItemNamesList(this.CurrentCategory);
		array.ForEach(delegate(ArmoryCell child)
		{
			UIToggle component2 = child.GetComponent<UIToggle>();
			if (component2 != null && component2.value)
			{
				List<EventDelegate> onChange = component2.onChange;
				component2.onChange = new List<EventDelegate>();
				bool instantTween = component2.instantTween;
				component2.instantTween = true;
				component2.Set(false);
				component2.onChange = onChange;
				component2.instantTween = instantTween;
			}
		});
		if (itemToSet == null && this.CurrentItem != null && itemNamesList.Any((ShopNGUIController.ShopItem item) => item.Id == this.CurrentItem.Id && item.Category == this.CurrentItem.Category))
		{
			itemToSet = this.CurrentItem;
		}
		bool flag = false;
		Action<ArmoryCell, int> action = delegate(ArmoryCell cell, int cellNumber)
		{
			Transform transform = cell.transform;
			transform.SetParent(this.itemsGrid.transform, false);
			transform.localScale = Vector3.one;
			transform.GetChild(0).localScale = new Vector3(scale, scale, scale);
			cell.GetComponent<BoxCollider>().size = new Vector2(cellWidth, cellHeight);
			cell.name = cellNumber.ToString("D4");
		};
		for (int i = 0; i < itemNamesList.Count; i++)
		{
			bool flag2 = i >= array.Length;
			ArmoryCell armoryCell = (!flag2) ? array[i] : this.GetNewCell();
			if (flag2)
			{
				flag = true;
				action(armoryCell, i);
			}
			armoryCell.Setup(itemNamesList[i], ShopNGUIController.IsBestCategory(this.CurrentCategory));
			if (!armoryCell.gameObject.activeSelf)
			{
				armoryCell.gameObject.SetActiveSafeSelf(true);
			}
			else
			{
				armoryCell.StopAllCoroutines();
				armoryCell.UpdateAllAndStartUpdateCoroutine();
			}
			armoryCell.ReSubscribeToEquipEvents();
			try
			{
				this.SetSliceShaderParams(armoryCell);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in setting SliceToWorldPosShader: " + arg);
			}
		}
		int num2 = (num != 3) ? 12 : 9;
		int num3;
		if (itemNamesList.Count < num2 && itemNamesList.Count > 0)
		{
			num3 = num2 - itemNamesList.Count;
		}
		else if (itemNamesList.Count % num > 0 && itemNamesList.Count > 0)
		{
			num3 = num - itemNamesList.Count % num;
		}
		else
		{
			num3 = 0;
		}
		int num4 = itemNamesList.Count + num3;
		for (int j = itemNamesList.Count; j < num4; j++)
		{
			bool flag3 = j >= array.Length;
			ArmoryCell armoryCell2 = (!flag3) ? array[j] : this.GetNewCell();
			if (flag3)
			{
				flag = true;
				action(armoryCell2, j);
			}
			armoryCell2.MakeCellEmpty();
			armoryCell2.gameObject.SetActiveSafeSelf(true);
			armoryCell2.SetupEmptyCellCategory(this.CurrentCategory);
		}
		for (int k = num4; k < array.Length; k++)
		{
			array[k].gameObject.SetActiveSafeSelf(false);
		}
		this.itemsGrid.cellWidth = cellWidth + 10f;
		this.itemsGrid.cellHeight = cellHeight + 10f;
		this.itemsGrid.maxPerLine = num;
		if (flag)
		{
			this.itemsGrid.Reposition();
		}
		this.gridScrollView.ResetPosition();
		if (!ShopNGUIController.gridScrollViewPanelUpdatedOnFirstLaunch)
		{
			ShopNGUIController.gridScrollViewPanelUpdatedOnFirstLaunch = true;
			if (this.gridScrollView.panel != null)
			{
				this.gridScrollView.panel.ResetAndUpdateAnchors();
				this.gridScrollView.panel.Refresh();
			}
			this.AdjustCategoryGridCells();
		}
		if (itemToSet == null)
		{
			return;
		}
		if (itemNamesList.Any((ShopNGUIController.ShopItem item) => item.Id == itemToSet.Id))
		{
			this.ChooseItem(itemToSet, true, true);
		}
		else if (ShopNGUIController.IsWeaponCategory(this.CurrentCategory))
		{
			string id = WeaponManager.LastBoughtTag(itemToSet.Id, null) ?? WeaponManager.FirstUnboughtOrForOurTier(this.CurrentItem.Id);
			this.ChooseItem(new ShopNGUIController.ShopItem(id, this.CurrentCategory), true, true);
		}
		else if (this.CurrentCategory == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.CurrentItem = new ShopNGUIController.ShopItem("CustomSkinID", ShopNGUIController.CategoryNames.SkinsCategory);
		}
		else
		{
			this.CurrentItem = null;
		}
	}

	// Token: 0x0600469D RID: 18077 RVA: 0x001808CC File Offset: 0x0017EACC
	public static void AddModel(GameObject pref, ShopNGUIController.Action7<GameObject, Vector3, Vector3, string, float, int, int> act, ShopNGUIController.CategoryNames category, bool isButtonInGameGui = false, WeaponSounds wsForPos = null)
	{
		float arg = 150f;
		Vector3 arg2 = Vector3.zero;
		Vector3 arg3 = Vector3.zero;
		GameObject gameObject = null;
		int arg4 = 0;
		int arg5 = 0;
		string arg6 = null;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			arg = wsForPos.scaleShop;
			arg2 = wsForPos.positionShop;
			arg3 = wsForPos.rotationShop;
			gameObject = pref.GetComponent<InnerWeaponPars>().bonusPrefab;
			try
			{
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(wsForPos.name.Replace("(Clone)", string.Empty));
				string tag = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
				ItemRecord byTag = ItemDb.GetByTag(tag);
				WeaponSounds weaponSounds = Resources.Load<WeaponSounds>(string.Format("Weapons/{0}", byTag.PrefabName));
				arg6 = weaponSounds.shopName;
			}
			catch (Exception arg7)
			{
				Debug.LogError("Error in getting shop name of first upgrade: " + arg7);
				arg6 = wsForPos.shopName;
			}
			arg4 = wsForPos.tier;
		}
		else
		{
			switch (category)
			{
			case ShopNGUIController.CategoryNames.HatsCategory:
			case ShopNGUIController.CategoryNames.ArmorCategory:
			case ShopNGUIController.CategoryNames.CapesCategory:
			case ShopNGUIController.CategoryNames.BootsCategory:
			case ShopNGUIController.CategoryNames.GearCategory:
			case ShopNGUIController.CategoryNames.MaskCategory:
			{
				gameObject = pref.transform.GetChild(0).gameObject;
				ShopPositionParams infoForNonWeaponItem = ItemDb.GetInfoForNonWeaponItem(pref.nameNoClone(), category);
				arg3 = infoForNonWeaponItem.rotationShop;
				arg = infoForNonWeaponItem.scaleShop;
				arg2 = infoForNonWeaponItem.positionShop;
				arg6 = infoForNonWeaponItem.shopName;
				arg4 = infoForNonWeaponItem.tier;
				arg5 = infoForNonWeaponItem.League;
				break;
			}
			case ShopNGUIController.CategoryNames.SkinsCategory:
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(pref);
				CharacterInterface component = gameObject.GetComponent<CharacterInterface>();
				component.SetSimpleCharacter();
				arg3 = component.rotationShop;
				arg = component.scaleShop;
				arg2 = component.positionShop;
				arg4 = component.shopTier;
				break;
			}
			default:
				if (category == ShopNGUIController.CategoryNames.ThrowingCategory || category == ShopNGUIController.CategoryNames.ToolsCategoty || category == ShopNGUIController.CategoryNames.SupportCategory)
				{
					gameObject = pref;
					arg3 = Vector3.zero;
					arg = 1f;
					arg2 = Vector3.zero;
					arg6 = string.Empty;
					arg4 = 1;
					arg5 = 1;
				}
				break;
			}
		}
		Vector3 localPosition = Vector3.zero;
		GameObject gameObject2 = null;
		if (category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			gameObject2 = gameObject;
			localPosition = new Vector3(0f, -1f, 0f);
		}
		else if (ShopNGUIController.IsGadgetsCategory(category))
		{
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		}
		else if (gameObject != null)
		{
			Material[] array = null;
			Mesh mesh = null;
			SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer == null)
			{
				SkinnedMeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				if (componentsInChildren != null && componentsInChildren.Length > 0)
				{
					skinnedMeshRenderer = componentsInChildren[0];
				}
			}
			if (skinnedMeshRenderer != null)
			{
				array = skinnedMeshRenderer.sharedMaterials;
				mesh = skinnedMeshRenderer.sharedMesh;
			}
			else
			{
				MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
				MeshRenderer component3 = gameObject.GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					mesh = component2.sharedMesh;
				}
				if (component3 != null)
				{
					array = component3.sharedMaterials;
				}
			}
			if (array != null && mesh != null)
			{
				gameObject2 = new GameObject();
				gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
				MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
				meshRenderer.materials = array;
				localPosition = -meshRenderer.bounds.center;
			}
		}
		try
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		catch (Exception arg8)
		{
			Debug.LogError("Exception DisableLightProbesRecursively: " + arg8);
		}
		GameObject gameObject3 = new GameObject();
		gameObject3.name = gameObject2.name;
		gameObject2.transform.localPosition = localPosition;
		gameObject2.transform.parent = gameObject3.transform;
		Player_move_c.SetLayerRecursively(gameObject3, LayerMask.NameToLayer("NGUIShop"));
		if (act != null)
		{
			act(gameObject3, arg2, arg3, arg6, arg, arg4, arg5);
		}
	}

	// Token: 0x0600469E RID: 18078 RVA: 0x00180CB8 File Offset: 0x0017EEB8
	public void HandlePropertiesHideShow(PropertiesHideSHow propertiesHideShowScript)
	{
		if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
		{
			foreach (ShopNGUIController.CategoryNames key in new ShopNGUIController.CategoryNames[]
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				ShopNGUIController.CategoryNames.BackupCategory,
				ShopNGUIController.CategoryNames.MeleeCategory,
				ShopNGUIController.CategoryNames.SpecilCategory,
				ShopNGUIController.CategoryNames.SniperCategory,
				ShopNGUIController.CategoryNames.PremiumCategory
			})
			{
				this.propertiesShownInCategory[key] = !propertiesHideShowScript.isHidden;
			}
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
		{
			foreach (ShopNGUIController.CategoryNames key2 in new ShopNGUIController.CategoryNames[]
			{
				ShopNGUIController.CategoryNames.ThrowingCategory,
				ShopNGUIController.CategoryNames.ToolsCategoty,
				ShopNGUIController.CategoryNames.SupportCategory
			})
			{
				this.propertiesShownInCategory[key2] = !propertiesHideShowScript.isHidden;
			}
		}
		else
		{
			this.propertiesShownInCategory[this.CurrentItem.Category] = !propertiesHideShowScript.isHidden;
		}
		CoroutineRunner.Instance.StartCoroutine(this.SetPositionAfterPropertiesHideShow(propertiesHideShowScript));
	}

	// Token: 0x0600469F RID: 18079 RVA: 0x00180DC8 File Offset: 0x0017EFC8
	private void GetArmoryCellAndAdjustShaderParams()
	{
		ArmoryCell componentInChildren = this.itemsGrid.GetComponentInChildren<ArmoryCell>(true);
		if (componentInChildren != null)
		{
			this.SetSliceShaderParams(componentInChildren);
		}
	}

	// Token: 0x060046A0 RID: 18080 RVA: 0x00180DF8 File Offset: 0x0017EFF8
	private IEnumerator SetPositionAfterPropertiesHideShow(PropertiesHideSHow propertiesHideShowScript)
	{
		CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSecondsActionEveryNFrames(propertiesHideShowScript.animTime * 1.2f, delegate
		{
			this.gridScrollView.DisableSpring();
			this.gridScrollView.RestrictWithinBounds(true);
			if (!this.gridScrollView.shouldMove)
			{
				this.gridScrollView.SetDragAmount(0f, 0f, false);
			}
			this.UpdateSkinShaderParams();
		}, 1));
		yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSecondsActionEveryNFrames(propertiesHideShowScript.animTime * 1.1f, delegate
		{
			this.RefreshGridScrollView();
		}, 3));
		if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.CapesCategory || this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.GetArmoryCellAndAdjustShaderParams();
		}
		this.RefreshGridScrollView();
		this.needRefreshInLateUpdate = 2;
		yield break;
	}

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x060046A1 RID: 18081 RVA: 0x00180E24 File Offset: 0x0017F024
	public List<ArmoryCell> AllArmoryCells
	{
		get
		{
			return (from cell in this.itemsGrid.GetComponentsInChildren<ArmoryCell>(true)
			orderby cell.name
			select cell).ToList<ArmoryCell>();
		}
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x00180E5C File Offset: 0x0017F05C
	public ArmoryCell GetArmoryCellByItemId(string itemId)
	{
		return this.AllArmoryCells.FirstOrDefault((ArmoryCell c) => c.ItemId == itemId);
	}

	// Token: 0x060046A3 RID: 18083 RVA: 0x00180E90 File Offset: 0x0017F090
	private void ClearCaption()
	{
		this.caption.text = string.Empty;
		foreach (UILabel uilabel in this.wearNameLabels)
		{
			uilabel.text = string.Empty;
		}
		foreach (UILabel uilabel2 in this.skinNameLabels)
		{
			uilabel2.text = string.Empty;
		}
		foreach (UILabel uilabel3 in this.armorNameLabels)
		{
			uilabel3.text = string.Empty;
		}
	}

	// Token: 0x060046A4 RID: 18084 RVA: 0x00180FC0 File Offset: 0x0017F1C0
	private void SetCurrentItemCaption(ShopNGUIController.ShopItem item)
	{
		string id = item.Id;
		if (item.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			Egg egg = Singleton<EggsManager>.Instance.GetPlayerEggs().FirstOrDefault((Egg e) => e.Id.ToString() == item.Id);
			if (egg != null)
			{
				id = egg.Data.Id;
			}
			else
			{
				Debug.LogErrorFormat("ShopNguiController: idForName, egg = null, item.Id = {0}", new object[]
				{
					item.Id
				});
			}
		}
		string itemName = ItemDb.GetItemName(id, item.Category);
		this.caption.text = itemName;
		foreach (UILabel uilabel in this.wearNameLabels)
		{
			uilabel.text = itemName;
		}
		if (this.skinNameLabels != null && (this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory || this.CurrentItem.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory))
		{
			foreach (UILabel uilabel2 in this.skinNameLabels)
			{
				if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory)
				{
					uilabel2.text = ((!(item.Id == "CustomSkinID")) ? ((!SkinsController.skinsNamesForPers.ContainsKey(item.Id)) ? string.Empty : SkinsController.skinsNamesForPers[item.Id]) : LocalizationStore.Get("Key_1090"));
				}
				else
				{
					try
					{
						WeaponSkin skin = WeaponSkinsManager.GetSkin(item.Id);
						if (skin != null)
						{
							uilabel2.text = LocalizationStore.Get(skin.Lkey);
						}
					}
					catch (Exception arg)
					{
						Debug.LogError("Exception in setting weapon skin caption: " + arg);
					}
				}
			}
		}
	}

	// Token: 0x060046A5 RID: 18085 RVA: 0x00181234 File Offset: 0x0017F434
	public void ChooseItem(ShopNGUIController.ShopItem item, bool moveScroll = false, bool switchToggleInstantly = false)
	{
		this.ReturnPersWearAndSkinWhenSwitching();
		if (item.Id == null)
		{
			if (ShopNGUIController.IsWeaponCategory(item.Category))
			{
				this.UpdatePersWithNewItem(item);
			}
			return;
		}
		List<ArmoryCell> allArmoryCells = this.AllArmoryCells;
		List<ArmoryCell> list = (from ac in allArmoryCells
		where ac.gameObject.activeSelf
		select ac).ToList<ArmoryCell>();
		int num = list.FindIndex((ArmoryCell cell) => cell.ItemId == item.Id);
		ArmoryCell armoryCell = list[num];
		if (moveScroll)
		{
			float num2 = this.gridScrollView.bounds.extents.y * 2f;
			UIPanel uipanel = this.gridScrollView.panel ?? this.gridScrollView.GetComponent<UIPanel>();
			float y = uipanel.GetViewSize().y;
			if (num != -1)
			{
				int num3 = ShopNGUIController.NumberOfColumnsInArmoryGrid();
				int num4 = num / num3;
				int num5 = (list.Count % num3 != 0) ? (list.Count / num3 + 1) : (list.Count / num3);
				float cellHeight = this.itemsGrid.cellHeight;
				float num6 = (float)(num4 + 1) * cellHeight;
				if (y < num6)
				{
					float num7 = num2 - y;
					if (num7 > 0f)
					{
						float num8 = (num6 - y) / num7;
						num8 += 3f / num7;
						this.gridScrollView.SetDragAmount(0f, num8, false);
						if (!this.categoryGridsRepositioned && num8 > 1f)
						{
							float y2 = (num8 - 1f) * num7;
							this.gridScrollView.MoveRelative(new Vector3(0f, y2));
						}
						if (this.categoryGridsRepositioned)
						{
							float y3 = 1f;
							this.gridScrollView.MoveRelative(new Vector3(0f, y3));
						}
					}
				}
			}
		}
		if (armoryCell != null)
		{
			UIToggle component = armoryCell.GetComponent<UIToggle>();
			if (component != null)
			{
				List<EventDelegate> onChange = component.onChange;
				component.onChange = new List<EventDelegate>();
				bool instantTween = component.instantTween;
				if (switchToggleInstantly)
				{
					component.instantTween = true;
				}
				component.Set(true);
				component.onChange = onChange;
				component.instantTween = instantTween;
				armoryCell.UpdateUpgrades();
				armoryCell.UpdateDiscountVisibility();
			}
		}
		this.CurrentItem = item;
		this.UpdatePersWithNewItem(item);
		this.UpdateButtons();
		this.SetCurrentItemCaption(item);
		this.UpdateTutorialState();
	}

	// Token: 0x060046A6 RID: 18086 RVA: 0x001814D8 File Offset: 0x0017F6D8
	private void RefreshGridScrollView()
	{
		if (this.gridScrollView.panel != null)
		{
			this.gridScrollView.panel.SetDirty();
			this.gridScrollView.panel.Refresh();
		}
	}

	// Token: 0x060046A7 RID: 18087 RVA: 0x0018151C File Offset: 0x0017F71C
	private void SetUpUpgradesAndTiers(bool bought, ref bool buyActive, ref bool upgradeActive, ref bool saleActive, ref bool needMoreTrophiesActive)
	{
		bool flag = TempItemsController.PriceCoefs.ContainsKey(this.CurrentItem.Id);
		bool flag2 = false;
		int num2;
		int num = (this.CurrentItem.Id == null) ? -1 : ShopNGUIController.CurrentNumberOfUpgradesForWear(this.CurrentItem.Id, out flag2, this.CurrentItem.Category, out num2);
		bool flag3 = flag2;
		if ((!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted) || this.InTrainingAfterNoviceArmorRemoved)
		{
			buyActive = false;
			upgradeActive = false;
		}
		else
		{
			RatingSystem.RatingLeague ratingLeague = (RatingSystem.RatingLeague)Wear.LeagueForWear(this.CurrentItem.Id, this.CurrentItem.Category);
			bool flag4 = ratingLeague > RatingSystem.instance.currentLeague;
			buyActive = (this.CurrentItem.Id != null && !flag3 && num == 0 && this.CurrentItem.Id != "cape_Custom" && !flag && !flag4);
			upgradeActive = (this.CurrentItem.Id != null && !flag3 && num != 0 && !flag);
			needMoreTrophiesActive = (num == 0 && flag4);
		}
		if (!flag3)
		{
			int num3 = Wear.TierForWear(WeaponManager.FirstUnboughtTag(this.CurrentItem.Id));
			this.upgradeButton.isEnabled = ((!upgradeActive || ExpController.OurTierForAnyPlace() >= num3) && !flag);
		}
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x00181690 File Offset: 0x0017F890
	private static void UpdateTryGunDiscountTime(PropertiesArmoryItemContainer props, string itemId)
	{
		try
		{
			if (props != null && props.tryGunDiscountTime != null)
			{
				props.tryGunDiscountTime.text = RiliExtensions.GetTimeStringDays(WeaponManager.sharedManager.StartTimeForTryGunDiscount(itemId) + (long)WeaponManager.TryGunPromoDuration() - PromoActionsManager.CurrentUnixTime);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in UpdateTryGunDiscountTime: " + arg);
		}
	}

	// Token: 0x060046A9 RID: 18089 RVA: 0x0018171C File Offset: 0x0017F91C
	public void UpdateButtons()
	{
		string text = null;
		if ((this.CurrentCategory != ShopNGUIController.CategoryNames.EggsCategory || Singleton<EggsManager>.Instance.GetPlayerEggs().Count != 0) && (this.CurrentCategory != ShopNGUIController.CategoryNames.PetsCategory || Singleton<PetsManager>.Instance.PlayerPets.Count != 0) && !this.IsEmptyBestCategory())
		{
			ShopNGUIController.CategoryNames category = this.CurrentItem.Category;
			if (ShopNGUIController.IsWeaponCategory(category))
			{
				text = (WeaponManager.FirstUnboughtTag(this.CurrentItem.Id) ?? this.CurrentItem.Id);
				string text2 = WeaponManager.FirstTagForOurTier(this.CurrentItem.Id, null);
				List<string> list = WeaponUpgrades.ChainForTag(this.CurrentItem.Id);
				if (text2 != null && list != null && list.IndexOf(text2) > list.IndexOf(text))
				{
					text = text2;
				}
			}
			if (ShopNGUIController.IsGadgetsCategory(category))
			{
				text = (GadgetsInfo.FirstUnboughtOrForOurTier(this.CurrentItem.Id) ?? this.CurrentItem.Id);
				string text3 = GadgetsInfo.FirstForOurTier(this.CurrentItem.Id);
				List<string> list2 = GadgetsInfo.UpgradesChainForGadget(this.CurrentItem.Id);
				if (text3 != null && list2 != null && list2.IndexOf(text3) > list2.IndexOf(text))
				{
					text = text3;
				}
			}
		}
		this.GetStateButtons((this.CurrentItem == null) ? null : this.CurrentItem.Id, text, this.propertiesContainer, true);
		this.ReparentButtons();
		this.UpdatePropertiesPanels();
		this.UpdateVisibilityOfPropertiesPanelAndButtons();
		this.SetCamera();
	}

	// Token: 0x060046AA RID: 18090 RVA: 0x001818B8 File Offset: 0x0017FAB8
	public void GetStateButtons(string _viewedId, string _showForId_WEAPONS_AND_GADGET_ONLY, PropertiesArmoryItemContainer _propertiesContainer, bool isFromMainWindow)
	{
		bool flag = false;
		bool state = false;
		bool state2 = false;
		bool state3 = false;
		bool flag2 = false;
		bool flag3 = false;
		bool state4 = false;
		bool flag4 = false;
		bool state5 = false;
		bool state6 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool state7 = false;
		string text = string.Empty;
		bool state8 = false;
		bool state9 = false;
		bool state10 = false;
		bool state11 = false;
		bool flag8 = false;
		bool flag9 = false;
		bool state12 = false;
		bool flag10 = false;
		string text2 = string.Empty;
		string text3 = string.Empty;
		bool flag11 = TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted;
		bool flag12 = _viewedId != null && TempItemsController.PriceCoefs.ContainsKey(_viewedId);
		if (_propertiesContainer.upgradeButton != null)
		{
			_propertiesContainer.upgradeButton.isEnabled = true;
		}
		if (_propertiesContainer.trainButton != null)
		{
			_propertiesContainer.trainButton.isEnabled = true;
		}
		if (_propertiesContainer.gadgetProperties != null)
		{
			_propertiesContainer.gadgetProperties.SetActiveSafeSelf(this.CurrentItem != null && (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category) || this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory));
		}
		if (_propertiesContainer.weaponsRarityLabel != null)
		{
			_propertiesContainer.weaponsRarityLabel.gameObject.SetActiveSafe(this.CurrentItem != null && (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category) || this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory));
		}
		if (_propertiesContainer.specialParams != null)
		{
			_propertiesContainer.specialParams.SetActiveSafeSelf(this.CurrentItem != null && (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category) || ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category) || (this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory && Singleton<PetsManager>.Instance.PlayerPets.Count > 0)));
		}
		if (_propertiesContainer.descriptionGadget != null)
		{
			_propertiesContainer.descriptionGadget.gameObject.SetActiveSafeSelf(this.CurrentItem != null && ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category));
		}
		this.petUpgradesInSpecial.SetActiveSafeSelf(this.CurrentItem != null && this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory);
		if (!this.IsGridEmpty() && this.CurrentItem != null && (this.CurrentCategory != ShopNGUIController.CategoryNames.PetsCategory || Singleton<PetsManager>.Instance.PlayerPets.Count != 0))
		{
			if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
			{
				WeaponSounds weaponSounds = null;
				WeaponSounds weaponSounds2 = null;
				weaponSounds = ItemDb.GetWeaponInfo(_viewedId);
				weaponSounds2 = ItemDb.GetWeaponInfo(_showForId_WEAPONS_AND_GADGET_ONLY);
				bool flag13 = false;
				int num = (_viewedId == null) ? -1 : ShopNGUIController.CurrentNumberOfUpgradesForWeapon(_viewedId, out flag13, this.CurrentItem.Category, true);
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(_viewedId))
				{
					num = 0;
				}
				bool flag14 = flag13 && (!(WeaponManager.sharedManager != null) || !WeaponManager.sharedManager.IsAvailableTryGun(_viewedId));
				flag4 = (_viewedId != null && !flag14 && num == 0 && !flag12 && flag11 && !this.InTrainingAfterNoviceArmorRemoved && (isFromMainWindow || _viewedId == _showForId_WEAPONS_AND_GADGET_ONLY));
				flag5 = (_viewedId != null && !flag14 && num != 0 && weaponSounds2.tier < 100 && !flag12 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
				if (WeaponManager.sharedManager != null && _viewedId != null)
				{
					bool flag15 = WeaponManager.sharedManager.IsAvailableTryGun(_viewedId);
					bool flag16 = WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(_viewedId);
					flag = (flag15 || flag16);
					if (flag)
					{
						try
						{
							if (_propertiesContainer.tryGunMatchesCount != null)
							{
								_propertiesContainer.tryGunMatchesCount.gameObject.SetActiveSafeSelf(flag15);
							}
							if (_propertiesContainer.tryGunDiscountTime != null)
							{
								_propertiesContainer.tryGunDiscountTime.gameObject.SetActiveSafeSelf(flag16);
							}
							if (flag15 && _propertiesContainer.tryGunMatchesCount != null)
							{
								_propertiesContainer.tryGunMatchesCount.text = ((SaltedInt)WeaponManager.sharedManager.TryGuns[_viewedId]["NumberOfMatchesKey"]).Value.ToString();
							}
							if (flag16)
							{
								ShopNGUIController.UpdateTryGunDiscountTime(_propertiesContainer, _viewedId);
							}
						}
						catch (Exception arg)
						{
							Debug.LogError("Exception in tryGunMatchesCount.text: " + arg);
						}
					}
				}
				List<string> list = WeaponUpgrades.ChainForTag(_viewedId);
				flag10 = (_showForId_WEAPONS_AND_GADGET_ONLY != WeaponManager.FirstTagForOurTier(_showForId_WEAPONS_AND_GADGET_ONLY, null) && weaponSounds2 != null && ExpController.OurTierForAnyPlace() >= weaponSounds2.tier && !flag12 && flag11 && !this.InTrainingAfterNoviceArmorRemoved && list != null && list.Contains(_viewedId) && list.Contains(_showForId_WEAPONS_AND_GADGET_ONLY) && list.IndexOf(_showForId_WEAPONS_AND_GADGET_ONLY) > num);
				if (flag10)
				{
					flag5 = false;
				}
				flag8 = ((!isFromMainWindow || flag5) && weaponSounds2 != null && ExpController.OurTierForAnyPlace() < weaponSounds2.tier && weaponSounds2.tier < 100 && !flag12 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
				if (flag8)
				{
					int num2 = (weaponSounds2.tier < 0 || weaponSounds2.tier >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[weaponSounds2.tier];
					text2 = string.Format("{0} {1}", LocalizationStore.Get("Key_1073"), num2);
					flag5 = false;
				}
				if (_propertiesContainer.needBuyPrevious != null)
				{
					_propertiesContainer.needBuyPrevious.SetActive(flag10);
					_propertiesContainer.needBuyPreviousLabel.text = LocalizationStore.Get("Key_2392");
				}
				string text4 = null;
				if (_viewedId != null)
				{
					text4 = WeaponManager.LastBoughtTag(_viewedId, null);
				}
				if (text4 == null && _viewedId != null && WeaponManager.sharedManager.IsAvailableTryGun(_viewedId))
				{
					text4 = _viewedId;
				}
				string text5 = ShopNGUIController._CurrentWeaponSetIDs()[(int)this.CurrentItem.Category];
				flag3 = (text4 != null && _viewedId != null && !(text5 == text4) && _viewedId != null && (num > 0 || WeaponManager.sharedManager.IsAvailableTryGun(_viewedId)) && (flag11 || _viewedId == WeaponTags.HunterRifleTag) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)) && (isFromMainWindow || _viewedId == _showForId_WEAPONS_AND_GADGET_ONLY));
				flag2 = (!isFromMainWindow && !string.IsNullOrEmpty(_viewedId) && ((text5 != null && text5.Equals(WeaponManager.LastBoughtTag(_viewedId, null) ?? string.Empty)) || (WeaponManager.sharedManager.IsAvailableTryGun(_viewedId) && text5.Equals(_viewedId))) && (flag11 || _viewedId == WeaponTags.HunterRifleTag) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)) && (isFromMainWindow || _viewedId == _showForId_WEAPONS_AND_GADGET_ONLY));
				if ((flag5 || flag12 || WeaponManager.sharedManager.IsAvailableTryGun(_viewedId)) && _viewedId != null && _showForId_WEAPONS_AND_GADGET_ONLY != null && (flag12 || !_showForId_WEAPONS_AND_GADGET_ONLY.Equals(_viewedId) || WeaponManager.sharedManager.IsAvailableTryGun(_viewedId)) && flag3)
				{
					flag3 = ((flag11 || _viewedId == WeaponTags.HunterRifleTag) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)));
				}
				if ((flag5 || flag12 || WeaponManager.sharedManager.IsAvailableTryGun(_viewedId)) && _viewedId != null && _showForId_WEAPONS_AND_GADGET_ONLY != null && (flag12 || !_showForId_WEAPONS_AND_GADGET_ONLY.Equals(_viewedId) || WeaponManager.sharedManager.IsAvailableTryGun(_viewedId)) && flag2)
				{
					flag2 = ((flag11 || _viewedId == WeaponTags.HunterRifleTag) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)));
				}
				if (!flag5 && !flag12 && !flag4 && flag3)
				{
					flag3 = ((flag11 || _viewedId == WeaponTags.HunterRifleTag) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)));
				}
				if (!flag5 && !flag12 && !flag4 && flag2)
				{
					flag2 = ((flag11 || _viewedId == WeaponTags.HunterRifleTag) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)));
				}
				if (!isFromMainWindow && (flag2 || flag3))
				{
					flag5 = false;
				}
				bool flag17;
				int num3 = ShopNGUIController.DiscountFor(_showForId_WEAPONS_AND_GADGET_ONLY, out flag17);
				if ((flag5 || flag4) && num3 > 0)
				{
					state7 = (flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					text = "-" + num3 + "%";
				}
				else
				{
					state7 = false;
				}
				if (_propertiesContainer.discountPanel != null)
				{
					_propertiesContainer.discountPanel.SetActiveSafeSelf(state7);
				}
				if (_propertiesContainer.discountLabel != null)
				{
					_propertiesContainer.discountLabel.text = text;
				}
				if ((flag5 || flag4) && _propertiesContainer.price != null)
				{
					_propertiesContainer.price.gameObject.SetActiveSafeSelf(true);
					_propertiesContainer.price.SetPrice(ShopNGUIController.GetItemPrice(_viewedId, this.CurrentItem.Category, false, true, false));
				}
				else if (_propertiesContainer.price != null)
				{
					_propertiesContainer.price.gameObject.SetActiveSafeSelf(false);
				}
				if (_showForId_WEAPONS_AND_GADGET_ONLY != null)
				{
					state10 = ((!weaponSounds.isMelee || weaponSounds.isShotMelee) && _viewedId != null);
					state11 = (weaponSounds.isMelee && !weaponSounds.isShotMelee && _viewedId != null);
					if (weaponSounds2 == null)
					{
						weaponSounds2 = weaponSounds;
					}
					int[] array;
					if (weaponSounds.isMelee && !weaponSounds.isShotMelee)
					{
						array = new int[]
						{
							(!flag12) ? weaponSounds.damageShop : ((int)weaponSounds.DPS),
							weaponSounds.fireRateShop,
							weaponSounds.mobilityShop
						};
					}
					else
					{
						array = new int[]
						{
							(!flag12) ? weaponSounds.damageShop : ((int)weaponSounds.DPS),
							weaponSounds.fireRateShop,
							weaponSounds.CapacityShop,
							weaponSounds.mobilityShop
						};
					}
					int[] array2;
					if (weaponSounds.isMelee && !weaponSounds.isShotMelee)
					{
						array2 = new int[]
						{
							(!flag12) ? weaponSounds2.damageShop : ((int)weaponSounds2.DPS),
							weaponSounds2.fireRateShop,
							weaponSounds2.mobilityShop
						};
					}
					else
					{
						array2 = new int[]
						{
							(!flag12) ? weaponSounds2.damageShop : ((int)weaponSounds2.DPS),
							weaponSounds2.fireRateShop,
							weaponSounds2.CapacityShop,
							weaponSounds2.mobilityShop
						};
					}
					int[] array3 = array2;
					if (weaponSounds.isMelee && !weaponSounds.isShotMelee)
					{
						_propertiesContainer.damageMelee.text = ShopNGUIController.GetWeaponStatText(array[0], array3[0]);
						_propertiesContainer.fireRateMElee.text = ShopNGUIController.GetWeaponStatText(array[1], array3[1]);
						_propertiesContainer.mobilityMelee.text = ShopNGUIController.GetWeaponStatText(array[2], array3[2]);
					}
					else
					{
						_propertiesContainer.damage.text = ShopNGUIController.GetWeaponStatText(array[0], array3[0]);
						_propertiesContainer.fireRate.text = ShopNGUIController.GetWeaponStatText(array[1], array3[1]);
						_propertiesContainer.capacity.text = ShopNGUIController.GetWeaponStatText(array[2], array3[2]);
						_propertiesContainer.mobility.text = ShopNGUIController.GetWeaponStatText(array[3], array3[3]);
					}
					_propertiesContainer.specialParams.SetActiveSafeSelf(true);
					WeaponSounds weaponSounds3 = weaponSounds2;
					if (weaponSounds3 != null)
					{
						if (_propertiesContainer.weaponsRarityLabel != null)
						{
							_propertiesContainer.weaponsRarityLabel.text = LocalizationStore.Get("Key_2393") + ": " + ItemDb.GetItemRarityLocalizeName(weaponSounds3.rarity);
						}
						for (int i = 0; i < _propertiesContainer.effectsLabels.Count; i++)
						{
							_propertiesContainer.effectsLabels[i].gameObject.SetActiveSafeSelf(i < weaponSounds3.InShopEffects.Count);
							if (i < weaponSounds3.InShopEffects.Count)
							{
								_propertiesContainer.effectsLabels[i].text = ((weaponSounds3.InShopEffects[i] != WeaponSounds.Effects.Zoom) ? string.Empty : (weaponSounds3.zoomShop.ToString() + "X ")) + LocalizationStore.Get(WeaponSounds.keysAndSpritesForEffects[weaponSounds3.InShopEffects[i]].Value);
								_propertiesContainer.effectsSprites[i].spriteName = WeaponSounds.keysAndSpritesForEffects[weaponSounds3.InShopEffects[i]].Key;
								_propertiesContainer.effectsSprites[i].ResetAndUpdateAnchors();
							}
						}
						if (_propertiesContainer.specialTable != null)
						{
							_propertiesContainer.specialTable.Reposition();
						}
					}
				}
			}
			else
			{
				state10 = false;
				state11 = false;
				ShopNGUIController.CategoryNames category = this.CurrentItem.Category;
				switch (category)
				{
				case ShopNGUIController.CategoryNames.HatsCategory:
				case ShopNGUIController.CategoryNames.ArmorCategory:
				case ShopNGUIController.CategoryNames.CapesCategory:
				case ShopNGUIController.CategoryNames.BootsCategory:
				case ShopNGUIController.CategoryNames.MaskCategory:
				{
					string text6 = WeaponManager.LastBoughtTag(_viewedId, null);
					bool flag18 = text6 != null;
					bool flag19 = flag18 && this.WearForCat(this.CurrentItem.Category).Equals(text6);
					string text7 = WeaponManager.FirstUnboughtTag(_viewedId);
					this.SetUpUpgradesAndTiers(flag18, ref flag4, ref flag5, ref state7, ref state12);
					flag3 = (_viewedId != "Armor_Novice" && flag18 && !flag19 && text6 != null && text6.Equals(_viewedId) && (this.TutorialPhasePassed >= ShopNGUIController.TutorialPhase.SelectWearCategory || TrainingController.TrainingCompleted) && (!this.InTrainingAfterNoviceArmorRemoved || _viewedId == WeaponManager.LastBoughtTag("Armor_Army_1", null)));
					state4 = (_viewedId != "Armor_Novice" && flag18 && flag19 && text6 != null && text6.Equals(_viewedId) && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					flag2 = (((!isFromMainWindow || this.CurrentItem.Category == ShopNGUIController.CategoryNames.ArmorCategory) && _viewedId == "Armor_Novice") || (text6 != null && this.WearForCat(this.CurrentItem.Category).Equals(text6) && !flag12));
					if (!flag18 && _viewedId != null && _viewedId.Equals("cape_Custom"))
					{
						state = (!this.inGame && !Defs.isDaterRegim && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					}
					if (!this.inGame && flag18 && _viewedId != null && _viewedId.Equals("cape_Custom"))
					{
						state2 = (!Defs.isDaterRegim && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					}
					if (flag5 || flag12)
					{
						flag3 = (flag3 && (flag12 || (_viewedId != null && text7 != null && (text6 == null || text6.Equals(text7) || !text7.Equals(_viewedId)))));
					}
					else
					{
						flag3 = (flag3 && _viewedId != null && text7 != null && (text6 == null || text6.Equals(text7) || !text7.Equals(_viewedId)));
					}
					if (!(_viewedId == "cape_Custom") || !flag18)
					{
						flag2 = (flag2 && (flag12 || (_viewedId != null && text7 != null && (text6 == null || text6.Equals(text7) || !text7.Equals(_viewedId)))));
					}
					int num4 = Wear.TierForWear(WeaponManager.FirstUnboughtTag(_viewedId));
					flag8 = (flag5 && ExpController.OurTierForAnyPlace() < num4 && !flag12 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					if (flag8)
					{
						int num5 = (num4 < 0 || num4 >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[num4];
						text2 = string.Format("{0} {1}", LocalizationStore.Get("Key_1073"), num5);
						flag5 = false;
					}
					if (_propertiesContainer.needBuyPrevious != null)
					{
						_propertiesContainer.needBuyPrevious.SetActiveSafeSelf(flag10);
						_propertiesContainer.needBuyPreviousLabel.text = LocalizationStore.Get("Key_2392");
					}
					try
					{
						if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.ArmorCategory)
						{
							float f = Wear.armorNum[_viewedId];
							_propertiesContainer.armorCountLabel.text = ((text6 == null || !(_viewedId != text6)) ? f.ToString() : ShopNGUIController.GetWeaponStatText(Mathf.RoundToInt(Wear.armorNum[text6]), Mathf.RoundToInt(f)));
							_propertiesContainer.armorCountLabel.gameObject.SetActiveSafeSelf(_viewedId != "Armor_Novice");
							_propertiesContainer.armorWearDescription.text = LocalizationStore.Get("Key_0354");
						}
						else
						{
							_propertiesContainer.nonArmorWearDEscription.text = LocalizationStore.Get(Wear.descriptionLocalizationKeys[_viewedId]);
						}
					}
					catch (Exception arg2)
					{
						Debug.LogError("Exception in setting desciption for wear: " + arg2);
					}
					break;
				}
				case ShopNGUIController.CategoryNames.SkinsCategory:
				{
					state = false;
					bool flag20 = _viewedId == "super_socialman";
					if (_viewedId != "CustomSkinID")
					{
						bool flag21 = false;
						bool flag22 = SkinsController.IsSkinBought(_viewedId, out flag21);
						bool flag23 = !SkinsController.IsLeagueSkinAvailableByLeague(_viewedId);
						flag4 = (flag21 && !flag22 && !flag20 && flag11 && !this.InTrainingAfterNoviceArmorRemoved && !flag23);
						state12 = (flag23 && (!flag21 || !flag22));
						flag5 = false;
						flag3 = ((!flag21 || flag22) && _viewedId != SkinsController.currentSkinNameForPers && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
						state4 = false;
						flag2 = (!isFromMainWindow && (!flag21 || flag22) && _viewedId.Equals(SkinsController.currentSkinNameForPers));
						state5 = (flag20 && !flag22 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
						long num6;
						bool flag24 = long.TryParse(_viewedId, out num6) && num6 >= 1000000L;
						state2 = (!this.inGame && flag24 && !Defs.isDaterRegim && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
						state3 = (flag24 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					}
					else
					{
						bool flag25 = Storager.getInt(Defs.SkinsMakerInProfileBought, true) > 0;
						state8 = (!flag25 && flag11);
						state6 = (!this.inGame && flag25 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
						state2 = false;
					}
					flag2 = (flag2 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
					flag8 = false;
					break;
				}
				default:
					if (category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
					{
						if (category != ShopNGUIController.CategoryNames.ThrowingCategory && category != ShopNGUIController.CategoryNames.ToolsCategoty && category != ShopNGUIController.CategoryNames.SupportCategory)
						{
							if (category != ShopNGUIController.CategoryNames.PetsCategory)
							{
								if (category == ShopNGUIController.CategoryNames.EggsCategory)
								{
									state = false;
									state2 = false;
									state3 = false;
									flag8 = false;
									flag4 = false;
									flag5 = false;
									flag2 = false;
									flag3 = false;
									state4 = false;
									if (_propertiesContainer.needTier != null && _propertiesContainer.needTierLabel != null)
									{
										_propertiesContainer.needTier.SetActiveSafeSelf(false);
									}
									if (_propertiesContainer.needBuyPrevious != null)
									{
										_propertiesContainer.needBuyPrevious.SetActiveSafeSelf(false);
									}
									flag7 = true;
								}
							}
							else
							{
								state = false;
								state2 = false;
								state3 = false;
								flag8 = false;
								flag9 = false;
								flag4 = false;
								bool flag26 = Singleton<PetsManager>.Instance.PlayerPets.Count > 0;
								bool flag27 = Singleton<PetsManager>.Instance.GetEqipedPetId() == _viewedId && flag11 && !this.InTrainingAfterNoviceArmorRemoved;
								flag2 = (!isFromMainWindow && flag27 && flag26);
								flag3 = (!flag27 && flag11 && !this.InTrainingAfterNoviceArmorRemoved && flag26);
								state4 = (flag27 && flag26);
								try
								{
									PetInfo petInfo = (!flag26) ? null : Singleton<PetsManager>.Instance.GetNextUp(_viewedId);
									flag6 = (petInfo != null && TrainingController.TrainingCompleted && flag26);
									flag9 = (flag6 && (!Singleton<PetsManager>.Instance.NextUpAvailable(_viewedId) || petInfo.Tier > ExpController.OurTierForAnyPlace()) && flag11 && !this.InTrainingAfterNoviceArmorRemoved && flag26);
									if (flag9)
									{
										if (!Singleton<PetsManager>.Instance.NextUpAvailable(_viewedId))
										{
											text3 = string.Format("{0}", LocalizationStore.Get("Key_2541"));
										}
										else
										{
											int tier = petInfo.Tier;
											int num7 = (tier < 0 || tier >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[tier];
											text3 = string.Format("{0} {1}", LocalizationStore.Get("Key_1073"), num7);
										}
										flag6 = false;
									}
									float num8 = 1f;
									string textProgress;
									if (petInfo != null)
									{
										int toUpPoints = PetsManager.Infos[_viewedId].ToUpPoints;
										int points = Singleton<PetsManager>.Instance.GetPlayerPet(_viewedId).Points;
										num8 = (float)points / (float)toUpPoints;
										textProgress = string.Concat(new string[]
										{
											LocalizationStore.Get("Key_2793"),
											" ",
											points.ToString(),
											"/",
											toUpPoints.ToString()
										});
									}
									else
									{
										textProgress = LocalizationStore.Get("Key_2547");
									}
									num8 = ((num8 <= 1f) ? num8 : 1f);
									this.petUpgradeIndicator.transform.localScale = new Vector3(num8, 1f, 1f);
									this.petUpgradePointsLabels.ForEach(delegate(UILabel label)
									{
										label.text = textProgress;
									});
								}
								catch (Exception ex)
								{
									Debug.LogErrorFormat("Exception in getting needTierPetActive for pets: {0}", new object[]
									{
										ex
									});
								}
								if (_propertiesContainer.gadgetsPropertiesList != null)
								{
									string key = (!flag26) ? null : Singleton<PetsManager>.Instance.GetFirstUnboughtPet(_viewedId).Id;
									for (int j = 0; j < _propertiesContainer.gadgetsPropertiesList.Count; j++)
									{
										GadgetInfo.Parameter item = (GadgetInfo.Parameter)j;
										bool flag28 = flag26 && PetsInfo.info[_viewedId].Parameters.Contains(item);
										_propertiesContainer.gadgetsPropertiesList[j].gameObject.SetActiveSafe(flag28);
										if (flag28)
										{
											int currentValue = 0;
											int nextValue = 0;
											switch (item)
											{
											case GadgetInfo.Parameter.Attack:
												currentValue = Mathf.RoundToInt((float)PetsInfo.info[_viewedId].DPS);
												nextValue = Mathf.RoundToInt((float)PetsInfo.info[key].DPS);
												break;
											case GadgetInfo.Parameter.HP:
												currentValue = Mathf.RoundToInt(PetsInfo.info[_viewedId].HP);
												nextValue = Mathf.RoundToInt(PetsInfo.info[key].HP);
												break;
											case GadgetInfo.Parameter.Speed:
												currentValue = Mathf.RoundToInt(PetsInfo.info[_viewedId].SpeedModif);
												nextValue = Mathf.RoundToInt(PetsInfo.info[key].SpeedModif);
												break;
											case GadgetInfo.Parameter.Respawn:
												currentValue = Mathf.RoundToInt(PetsInfo.info[_viewedId].RespawnTime);
												nextValue = Mathf.RoundToInt(PetsInfo.info[key].RespawnTime);
												break;
											}
											_propertiesContainer.gadgetsPropertiesList[j].propertyLabel.text = ShopNGUIController.GetWeaponStatText(currentValue, nextValue);
										}
									}
									if (_propertiesContainer.gadgetPropertyTable)
									{
										_propertiesContainer.gadgetPropertyTable.Reposition();
									}
								}
								if (_propertiesContainer.weaponsRarityLabel != null)
								{
									_propertiesContainer.weaponsRarityLabel.text = ((!flag26) ? string.Empty : (LocalizationStore.Get("Key_2393") + ": " + ItemDb.GetItemRarityLocalizeName(PetsInfo.info[_viewedId].Rarity)));
								}
								for (int k = 0; k < _propertiesContainer.effectsLabels.Count; k++)
								{
									bool flag29 = flag26 && k < PetsInfo.info[(!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : _viewedId].Effects.Count;
									_propertiesContainer.effectsLabels[k].gameObject.SetActiveSafeSelf(flag29);
									if (flag29)
									{
										_propertiesContainer.effectsLabels[k].text = LocalizationStore.Get(WeaponSounds.keysAndSpritesForEffects[PetsInfo.info[(!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : _viewedId].Effects[k]].Value);
										_propertiesContainer.effectsSprites[k].spriteName = WeaponSounds.keysAndSpritesForEffects[PetsInfo.info[(!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : _viewedId].Effects[k]].Key;
										_propertiesContainer.effectsSprites[k].ResetAndUpdateAnchors();
									}
								}
								if (_propertiesContainer.specialTable != null)
								{
									_propertiesContainer.specialTable.Reposition();
								}
								if (!flag26)
								{
									this.wearNameLabels.ForEach(delegate(UILabel label)
									{
										label.text = string.Empty;
									});
								}
								state9 = flag26;
							}
						}
						else
						{
							state = false;
							state2 = false;
							state3 = false;
							state4 = false;
							string text8 = GadgetsInfo.LastBoughtFor(_viewedId);
							List<string> list2 = GadgetsInfo.Upgrades[_viewedId];
							int num9 = (text8 != null) ? (list2.IndexOf(text8) + 1) : 0;
							bool flag30 = num9 == list2.Count;
							bool flag31 = list2.IndexOf(_showForId_WEAPONS_AND_GADGET_ONLY) < num9;
							int tier2 = GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].Tier;
							bool flag32 = GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].Tier <= ExpController.OurTierForAnyPlace();
							flag4 = (num9 == 0 && flag11 && !this.InTrainingAfterNoviceArmorRemoved && (isFromMainWindow || _viewedId == _showForId_WEAPONS_AND_GADGET_ONLY));
							flag10 = (!flag4 && list2.IndexOf(_showForId_WEAPONS_AND_GADGET_ONLY) > num9 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
							flag5 = (num9 > 0 && !flag30 && flag32 && !flag31 && !flag10 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
							bool flag33 = GadgetsInfo.EquippedForCategory((GadgetInfo.GadgetCategory)this.CurrentItem.Category) == ((!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : _viewedId) && flag11 && !this.InTrainingAfterNoviceArmorRemoved;
							flag2 = (!isFromMainWindow && flag33);
							flag3 = (!flag33 && text8 != null && text8 == ((!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : _viewedId) && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
							flag8 = (!flag10 && !flag32 && !flag31 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
							if (flag8)
							{
								int num10 = (tier2 >= ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1] : ExpController.LevelsForTiers[tier2];
								text2 = string.Format("{0} {1}", LocalizationStore.Get("Key_1073"), num10);
								flag5 = false;
							}
							if (_propertiesContainer.needBuyPrevious != null)
							{
								_propertiesContainer.needBuyPrevious.SetActiveSafeSelf(flag10);
								_propertiesContainer.needBuyPreviousLabel.text = LocalizationStore.Get("Key_2502");
							}
							bool flag34;
							int num11 = ShopNGUIController.DiscountFor(_showForId_WEAPONS_AND_GADGET_ONLY, out flag34);
							if ((flag5 || flag4) && num11 > 0)
							{
								state7 = (flag11 && !this.InTrainingAfterNoviceArmorRemoved);
								text = "-" + num11 + "%";
							}
							else
							{
								state7 = false;
							}
							if (_propertiesContainer.discountPanel != null)
							{
								_propertiesContainer.discountPanel.SetActiveSafeSelf(state7);
							}
							if (_propertiesContainer.discountLabel != null)
							{
								_propertiesContainer.discountLabel.text = text;
							}
							if ((flag5 || flag4) && _propertiesContainer.price != null)
							{
								_propertiesContainer.price.gameObject.SetActiveSafeSelf(true);
								_propertiesContainer.price.SetPrice(ShopNGUIController.GetItemPrice(_showForId_WEAPONS_AND_GADGET_ONLY, this.CurrentItem.Category, false, true, false));
							}
							else if (_propertiesContainer.price != null)
							{
								_propertiesContainer.price.gameObject.SetActiveSafeSelf(false);
							}
							if (_propertiesContainer.descriptionGadget != null)
							{
								_propertiesContainer.descriptionGadget.text = LocalizationStore.Get(GadgetsInfo.info[list2[0]].DescriptionLkey);
							}
							if (_propertiesContainer.gadgetsPropertiesList != null)
							{
								for (int l = 0; l < _propertiesContainer.gadgetsPropertiesList.Count; l++)
								{
									GadgetInfo.Parameter item2 = (GadgetInfo.Parameter)l;
									bool flag35 = GadgetsInfo.info[_viewedId].Parameters.Contains(item2);
									_propertiesContainer.gadgetsPropertiesList[l].gameObject.SetActiveSafe(flag35);
									if (flag35)
									{
										int currentValue2 = 0;
										int nextValue2 = 0;
										switch (item2)
										{
										case GadgetInfo.Parameter.Damage:
											currentValue2 = Mathf.RoundToInt(GadgetsInfo.info[_viewedId].DPS);
											nextValue2 = Mathf.RoundToInt(GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].DPS);
											break;
										case GadgetInfo.Parameter.Durability:
											currentValue2 = Mathf.RoundToInt(GadgetsInfo.info[_viewedId].Durability);
											nextValue2 = Mathf.RoundToInt(GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].Durability);
											break;
										case GadgetInfo.Parameter.Healing:
											currentValue2 = Mathf.RoundToInt(GadgetsInfo.info[_viewedId].Heal);
											nextValue2 = Mathf.RoundToInt(GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].Heal);
											break;
										case GadgetInfo.Parameter.Lifetime:
											currentValue2 = Mathf.RoundToInt(GadgetsInfo.info[_viewedId].Duration);
											nextValue2 = Mathf.RoundToInt(GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].Duration);
											break;
										case GadgetInfo.Parameter.Cooldown:
											currentValue2 = Mathf.RoundToInt(GadgetsInfo.info[_viewedId].Cooldown);
											nextValue2 = Mathf.RoundToInt(GadgetsInfo.info[_showForId_WEAPONS_AND_GADGET_ONLY].Cooldown);
											break;
										}
										_propertiesContainer.gadgetsPropertiesList[l].propertyLabel.text = ShopNGUIController.GetWeaponStatText(currentValue2, nextValue2);
									}
								}
							}
							if (_propertiesContainer.gadgetPropertyTable)
							{
								_propertiesContainer.gadgetPropertyTable.Reposition();
							}
							for (int m = 0; m < _propertiesContainer.effectsLabels.Count; m++)
							{
								bool flag36 = m < GadgetsInfo.info[(!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : this.CurrentItem.Id].Effects.Count;
								_propertiesContainer.effectsLabels[m].gameObject.SetActiveSafeSelf(flag36);
								if (flag36)
								{
									_propertiesContainer.effectsLabels[m].text = LocalizationStore.Get(WeaponSounds.keysAndSpritesForEffects[GadgetsInfo.info[(!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : this.CurrentItem.Id].Effects[m]].Value);
									_propertiesContainer.effectsSprites[m].spriteName = WeaponSounds.keysAndSpritesForEffects[GadgetsInfo.info[(!isFromMainWindow) ? _showForId_WEAPONS_AND_GADGET_ONLY : this.CurrentItem.Id].Effects[m]].Key;
									_propertiesContainer.effectsSprites[m].ResetAndUpdateAnchors();
								}
							}
							if (_propertiesContainer.specialTable != null)
							{
								_propertiesContainer.specialTable.Reposition();
							}
						}
					}
					else
					{
						state = false;
						bool flag37 = WeaponSkinsManager.IsBoughtSkin(_viewedId);
						WeaponSkin skin = WeaponSkinsManager.GetSkin(_viewedId);
						bool flag38 = !WeaponSkinsManager.IsAvailableByLeague(_viewedId);
						flag4 = (!flag37 && flag11 && !this.InTrainingAfterNoviceArmorRemoved && !flag38);
						state12 = (flag38 && !flag37);
						flag5 = false;
						bool flag39 = flag37 && WeaponSkinsManager.GetSettedSkinId(skin.ToWeapons[0]) == _viewedId && flag11 && !this.InTrainingAfterNoviceArmorRemoved;
						flag2 = (!isFromMainWindow && flag39);
						flag3 = (flag37 && !flag39 && flag11 && !this.InTrainingAfterNoviceArmorRemoved);
						state4 = (flag37 && flag39);
						state2 = false;
						state3 = false;
						flag8 = false;
					}
					break;
				}
			}
		}
		else if (!ShopNGUIController.IsBestCategory(this.CurrentCategory))
		{
			if (this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory)
			{
				flag7 = true;
			}
		}
		if (_propertiesContainer.tryGunPanel != null)
		{
			_propertiesContainer.tryGunPanel.SetActiveSafeSelf(flag);
			UIPanel component = this.gridScrollView.GetComponent<UIPanel>();
			if (flag)
			{
				component.bottomAnchor.Set(component.bottomAnchor.relative, (float)this.itemScrollBottomAnchorRent);
				this.itemsGrid.transform.localPosition = new Vector3(this.itemsGrid.transform.localPosition.x, this.itemsGrid.transform.localPosition.y + 25f, this.itemsGrid.transform.localPosition.z);
				this.updateScrollViewOnLateUpdateForTryPanel = true;
			}
			else if (component.bottomAnchor.absolute != this.itemScrollBottomAnchor)
			{
				component.bottomAnchor.Set(component.bottomAnchor.relative, (float)this.itemScrollBottomAnchor);
				this.updateScrollViewOnLateUpdateForTryPanel = true;
			}
		}
		if (_propertiesContainer.needTier != null && _propertiesContainer.needTierLabel != null)
		{
			_propertiesContainer.needTier.SetActiveSafeSelf(flag8);
			_propertiesContainer.needTierLabel.text = text2;
		}
		if (_propertiesContainer.needTierPet != null && _propertiesContainer.needTierPetLabel != null)
		{
			_propertiesContainer.needTierPet.SetActiveSafeSelf(flag9);
			_propertiesContainer.needTierPetLabel.text = text3;
		}
		if (_propertiesContainer.renamePetButton != null)
		{
			_propertiesContainer.renamePetButton.gameObject.SetActiveSafeSelf(state9);
		}
		if (_propertiesContainer.editButton != null)
		{
			_propertiesContainer.editButton.gameObject.SetActiveSafeSelf(state2);
		}
		if (_propertiesContainer.enableButton != null)
		{
			_propertiesContainer.enableButton.gameObject.SetActiveSafeSelf(state);
		}
		if (_propertiesContainer.deleteButton != null)
		{
			_propertiesContainer.deleteButton.gameObject.SetActiveSafeSelf(state3);
		}
		if (_propertiesContainer.buyButton != null)
		{
			_propertiesContainer.buyButton.gameObject.SetActiveSafeSelf(flag4);
		}
		if (_propertiesContainer.equipButton != null)
		{
			_propertiesContainer.equipButton.gameObject.SetActiveSafeSelf(flag3);
		}
		if (_propertiesContainer.unequipButton != null)
		{
			_propertiesContainer.unequipButton.gameObject.SetActiveSafeSelf(state4);
		}
		if (_propertiesContainer.upgradeButton != null)
		{
			_propertiesContainer.upgradeButton.gameObject.SetActiveSafeSelf(flag5);
		}
		if (_propertiesContainer.trainButton != null)
		{
			_propertiesContainer.trainButton.gameObject.SetActiveSafeSelf(flag6);
		}
		if (this.facebookLoginLockedSkinButton != null)
		{
			this.facebookLoginLockedSkinButton.gameObject.SetActiveSafeSelf(state5);
		}
		if (this.unlockButton != null)
		{
			this.unlockButton.gameObject.SetActiveSafeSelf(state8);
		}
		if (_propertiesContainer.needMoreTrophiesPanel != null)
		{
			_propertiesContainer.needMoreTrophiesPanel.SetActiveSafeSelf(state12);
		}
		this.superIncubatorButton.SetActiveSafeSelf(flag7 && TrainingController.TrainingCompleted);
		if (_propertiesContainer.equipped != null)
		{
			_propertiesContainer.equipped.SetActiveSafeSelf(flag2);
		}
		if (this.createButton != null)
		{
			this.createButton.gameObject.SetActiveSafeSelf(state6);
		}
		_propertiesContainer.weaponProperties.SetActiveSafeSelf(state10);
		_propertiesContainer.meleeProperties.SetActiveSafeSelf(state11);
		if (_propertiesContainer.tryGun != null)
		{
			_propertiesContainer.tryGun.gameObject.SetActiveSafeSelf(false);
		}
	}

	// Token: 0x060046AB RID: 18091 RVA: 0x001840A0 File Offset: 0x001822A0
	public static int DiscountFor(string itemTag, out bool onlyServerDiscount)
	{
		int result;
		try
		{
			if (itemTag == null)
			{
				Debug.LogError("DiscountFor: itemTag == null");
				onlyServerDiscount = false;
				result = 0;
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				float num = 100f;
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(itemTag))
				{
					long num2 = WeaponManager.sharedManager.DiscountForTryGun(itemTag);
					num -= (float)num2;
					num = Math.Max(1f, num);
					num = Math.Min(100f, num);
					flag2 = true;
				}
				num /= 100f;
				float num3 = 100f;
				if (!flag2 && PromoActionsManager.sharedManager.discounts.ContainsKey(itemTag) && PromoActionsManager.sharedManager.discounts[itemTag].Count > 0)
				{
					num3 -= (float)PromoActionsManager.sharedManager.discounts[itemTag][0].Value;
					num3 = Math.Max(10f, num3);
					num3 = Math.Min(100f, num3);
					flag = true;
				}
				num3 /= 100f;
				onlyServerDiscount = (!flag2 && flag);
				if (!flag2 && !flag)
				{
					result = 0;
				}
				else
				{
					float num4 = num * num3;
					num4 = Mathf.Clamp(num4, 0.01f, 1f);
					float f = (1f - num4) * 100f;
					int num5 = Mathf.RoundToInt(f);
					if (onlyServerDiscount && num5 % 5 != 0)
					{
						num5 = 5 * (num5 / 5 + 1);
					}
					num5 = Math.Min(num5, 99);
					result = num5;
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in DiscountFor: " + arg);
			onlyServerDiscount = false;
			result = 0;
		}
		return result;
	}

	// Token: 0x060046AC RID: 18092 RVA: 0x00184278 File Offset: 0x00182478
	public static ItemPrice GetItemPrice(string itemId, ShopNGUIController.CategoryNames category, bool upgradeNotBuyGear = false, bool useDiscounts = true, bool itemIdIsFirstUnbought = false)
	{
		ItemPrice result;
		try
		{
			if (itemId == null)
			{
				result = new ItemPrice(0, "Coins");
			}
			else
			{
				string text = string.Empty;
				if (itemIdIsFirstUnbought)
				{
					text = itemId;
				}
				else if (ShopNGUIController.IsWeaponCategory(category) || ShopNGUIController.IsWearCategory(category))
				{
					text = WeaponManager.FirstUnboughtOrForOurTier(itemId);
				}
				else if (ShopNGUIController.IsGadgetsCategory(category))
				{
					text = GadgetsInfo.FirstUnboughtOrForOurTier(itemId);
				}
				else if (category == ShopNGUIController.CategoryNames.PetsCategory)
				{
					PetInfo firstUnboughtPet = Singleton<PetsManager>.Instance.GetFirstUnboughtPet(itemId);
					text = ((firstUnboughtPet == null) ? itemId : firstUnboughtPet.Id);
				}
				string text2 = itemId;
				if (itemId != null && WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
				{
					text2 = WeaponManager.tagToStoreIDMapping[text];
				}
				if (category == ShopNGUIController.CategoryNames.GearCategory)
				{
					text2 = ((!upgradeNotBuyGear) ? GearManager.OneItemIDForGear(GearManager.HolderQuantityForID(text2), GearManager.CurrentNumberOfUphradesForGear(text2)) : GearManager.UpgradeIDForGear(GearManager.HolderQuantityForID(text2), GearManager.CurrentNumberOfUphradesForGear(text2) + 1));
				}
				if (ShopNGUIController.IsWearCategory(category) || ShopNGUIController.IsGadgetsCategory(category) || category == ShopNGUIController.CategoryNames.PetsCategory)
				{
					text2 = text;
				}
				string itemTag = (!ShopNGUIController.IsWeaponCategory(category) && !ShopNGUIController.IsWearCategory(category) && !ShopNGUIController.IsGadgetsCategory(category) && category != ShopNGUIController.CategoryNames.PetsCategory) ? itemId : text;
				ItemPrice itemPrice = ItemDb.GetPriceByShopId(text2, category) ?? new ItemPrice(10, "Coins");
				int num = itemPrice.Price;
				if (useDiscounts)
				{
					bool flag;
					int num2 = ShopNGUIController.DiscountFor(itemTag, out flag);
					if (num2 > 0)
					{
						float num3 = (float)num2;
						num = Math.Max((int)((float)num * 0.01f), Mathf.RoundToInt((float)num * (1f - num3 / 100f)));
						if (flag)
						{
							if (num % 5 < 3)
							{
								num -= num % 5;
							}
							else
							{
								num += 5 - num % 5;
							}
						}
					}
				}
				if (category == ShopNGUIController.CategoryNames.GearCategory && !upgradeNotBuyGear)
				{
					num *= GearManager.ItemsInPackForGear(GearManager.HolderQuantityForID(text2));
				}
				result = new ItemPrice(num, itemPrice.Currency);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in currentPrice: " + arg);
			result = new ItemPrice(0, "Coins");
		}
		return result;
	}

	// Token: 0x060046AD RID: 18093 RVA: 0x001844DC File Offset: 0x001826DC
	public static int PriceIfGunWillBeTryGun(string tg)
	{
		return Mathf.RoundToInt((float)ShopNGUIController.GetItemPrice(tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tg), false, false, false).Price * ((float)WeaponManager.BaseTryGunDiscount() / 100f));
	}

	// Token: 0x060046AE RID: 18094 RVA: 0x00184510 File Offset: 0x00182710
	public static int CurrentNumberOfUpgradesForWear(string id, out bool maxUpgrade, ShopNGUIController.CategoryNames c, out int totalNumberOfUpgrades)
	{
		if (id == "Armor_Novice")
		{
			maxUpgrade = ShopNGUIController.NoviceArmorAvailable;
			totalNumberOfUpgrades = 1;
			return (!ShopNGUIController.NoviceArmorAvailable) ? 0 : 1;
		}
		List<string> list = Wear.wear[c].FirstOrDefault((List<string> l) => l.Contains(id));
		if (list == null)
		{
			maxUpgrade = false;
			totalNumberOfUpgrades = 1;
			return 0;
		}
		totalNumberOfUpgrades = list.Count;
		for (int i = 0; i < list.Count; i++)
		{
			if (Storager.getInt(list[i], true) == 0)
			{
				maxUpgrade = false;
				return i;
			}
		}
		maxUpgrade = true;
		return list.Count;
	}

	// Token: 0x060046AF RID: 18095 RVA: 0x001845C8 File Offset: 0x001827C8
	private static int CurrentNumberOfUpgradesForWeapon(string weaponId, out bool haveAllUpgrades, ShopNGUIController.CategoryNames c, bool countTryGunsAsUpgrade = true)
	{
		List<string> list;
		if ((list = WeaponUpgrades.ChainForTag(weaponId)) == null)
		{
			list = new List<string>
			{
				weaponId
			};
		}
		List<string> list2 = list;
		int num = list2.Count;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(weaponId))
		{
			for (int i = list2.Count - 1; i >= 0; i--)
			{
				string defName = weaponId;
				bool flag = ItemDb.IsTemporaryGun(weaponId);
				if (!flag)
				{
					defName = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list2[i]]];
				}
				bool flag2 = ShopNGUIController.HasBoughtGood(defName, flag);
				if (!flag2 && countTryGunsAsUpgrade && WeaponManager.sharedManager != null)
				{
					flag2 = WeaponManager.sharedManager.IsAvailableTryGun(list2[i]);
				}
				if (flag2)
				{
					break;
				}
				num--;
			}
		}
		haveAllUpgrades = (num == ((list2.Count <= 0) ? 1 : list2.Count));
		return num;
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x001846C0 File Offset: 0x001828C0
	private static bool HasBoughtGood(string defName, bool tempGun = false)
	{
		bool flag = (!tempGun) ? (Storager.getInt(defName, true) == 0) : (!TempItemsController.sharedController.ContainsItem(defName));
		return !flag;
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x001846F8 File Offset: 0x001828F8
	public void UpdatePersWithNewItem(ShopNGUIController.ShopItem itemToSet)
	{
		if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
		{
			if (itemToSet == null && WeaponManager.sharedManager.playerWeapons.Count > 0)
			{
				string tag = ItemDb.GetByPrefabName((WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
				itemToSet = new ShopNGUIController.ShopItem(tag, this.CurrentItem.Category);
			}
			this.SetWeapon(itemToSet.Id, null);
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
		{
			this.UpdatePersGadget(GadgetsInfo.BaseName(itemToSet.Id));
		}
		else
		{
			ShopNGUIController.CategoryNames category = this.CurrentItem.Category;
			switch (category)
			{
			case ShopNGUIController.CategoryNames.HatsCategory:
				this.UpdatePersHat(itemToSet.Id);
				break;
			case ShopNGUIController.CategoryNames.ArmorCategory:
				this.UpdatePersArmor(itemToSet.Id);
				break;
			case ShopNGUIController.CategoryNames.SkinsCategory:
				if (itemToSet.Id != "CustomSkinID")
				{
					this.UpdatePersSkin(itemToSet.Id);
				}
				break;
			case ShopNGUIController.CategoryNames.CapesCategory:
				this.UpdatePersCape(itemToSet.Id);
				break;
			case ShopNGUIController.CategoryNames.BootsCategory:
				this.UpdatePersBoots(itemToSet.Id);
				break;
			default:
				if (category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
				{
					if (category == ShopNGUIController.CategoryNames.PetsCategory)
					{
						this.UpdatePersPet(itemToSet.Id);
					}
				}
				else if (itemToSet != null)
				{
					WeaponSkin skin = WeaponSkinsManager.GetSkin(itemToSet.Id);
					if (skin != null)
					{
						ItemRecord byPrefabName = ItemDb.GetByPrefabName(skin.ToWeapons[0]);
						if (byPrefabName != null)
						{
							this.SetWeapon(byPrefabName.Tag, itemToSet.Id);
						}
					}
				}
				else
				{
					try
					{
						string weaponTag = null;
						if (WeaponManager.sharedManager.playerWeapons.Count > 0)
						{
							weaponTag = ItemDb.GetByPrefabName((WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.nameNoClone()).Tag;
						}
						this.SetWeapon(weaponTag, null);
					}
					catch (Exception arg)
					{
						Debug.LogError("Exception in defaultWeaponAfterWeaponSkinsCategory: " + arg);
					}
				}
				break;
			case ShopNGUIController.CategoryNames.MaskCategory:
				this.UpdatePersMask(itemToSet.Id);
				break;
			}
		}
	}

	// Token: 0x060046B2 RID: 18098 RVA: 0x00184968 File Offset: 0x00182B68
	public void UpdatePersPet(string pet)
	{
		this.StopPetAnimation();
		this.characterInterface.UpdatePet(pet);
		this.PlayPetAnimation();
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x00184984 File Offset: 0x00182B84
	public void UpdatePersHat(string hat)
	{
		this.characterInterface.UpdateHat(hat, !ShopNGUIController.ShowWear);
		ShopNGUIController.SetPersHatVisible(this.hatPoint);
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x001849A8 File Offset: 0x00182BA8
	public void UpdatePersArmor(string armor)
	{
		if (this.armorPoint.childCount > 0)
		{
			Transform child = this.armorPoint.GetChild(0);
			ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					component.leftBone.parent = child.GetChild(0);
				}
				if (component.rightBone != null)
				{
					component.rightBone.parent = child.GetChild(0);
				}
				child.parent = null;
				child.position = new Vector3(0f, -10000f, 0f);
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (armor.Equals(Defs.ArmorNewNoneEqupped))
		{
			return;
		}
		string @string = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			armor = @string;
		}
		if (this.weapon != null)
		{
			GameObject gameObject = Resources.Load("Armor_Shop/" + armor) as GameObject;
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
			ArmorRefs component2 = gameObject2.transform.GetChild(0).GetComponent<ArmorRefs>();
			if (component2 != null)
			{
				WeaponSounds component3 = this.weapon.GetComponent<WeaponSounds>();
				gameObject2.transform.parent = this.armorPoint.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
				if (component2.leftBone != null && component3.LeftArmorHand != null)
				{
					component2.leftBone.parent = component3.LeftArmorHand;
					component2.leftBone.localPosition = Vector3.zero;
					component2.leftBone.localRotation = Quaternion.identity;
					component2.leftBone.localScale = new Vector3(1f, 1f, 1f);
				}
				if (component2.rightBone != null && component3.RightArmorHand != null)
				{
					component2.rightBone.parent = component3.RightArmorHand;
					component2.rightBone.localPosition = Vector3.zero;
					component2.rightBone.localRotation = Quaternion.identity;
					component2.rightBone.localScale = new Vector3(1f, 1f, 1f);
				}
			}
			ShopNGUIController.SetPersArmorVisible(this.armorPoint);
		}
	}

	// Token: 0x060046B5 RID: 18101 RVA: 0x00184CB4 File Offset: 0x00182EB4
	public void UpdatePersMask(string mask)
	{
		this.characterInterface.UpdateMask(mask, !ShopNGUIController.ShowWear);
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x00184CCC File Offset: 0x00182ECC
	public void UpdatePersCape(string cape)
	{
		this.characterInterface.UpdateCape(cape, null, !ShopNGUIController.ShowWear);
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x00184CE4 File Offset: 0x00182EE4
	public void UpdatePersSkin(string skinId)
	{
		if (skinId == null)
		{
			Debug.LogError("Skin id should not be null!");
			return;
		}
		this.SetSkinOnPers(SkinsController.skinsForPers[skinId]);
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x00184D14 File Offset: 0x00182F14
	public void SetSkinOnPers(Texture skin)
	{
		WeaponSounds x = (this.body.transform.childCount <= 0) ? null : this.body.transform.GetChild(0).GetComponent<WeaponSounds>();
		GadgetArmoryItem gadget = (!(x == null) || this.body.transform.childCount <= 0) ? null : this.body.transform.GetChild(0).GetComponent<GadgetArmoryItem>();
		this.characterInterface.SetSkin(skin, x, gadget);
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00184DA4 File Offset: 0x00182FA4
	public void UpdatePersBoots(string bs)
	{
		this.characterInterface.UpdateBoots(bs, !ShopNGUIController.ShowWear);
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x00184DBC File Offset: 0x00182FBC
	public void UpdatePersGadget(string idGadget)
	{
		this.animationCoroutineRunner.StopAllCoroutines();
		if (WeaponManager.sharedManager == null)
		{
			return;
		}
		if (idGadget == null)
		{
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>("GadgetsContent/gadget_shop_preview/" + idGadget);
		if (gameObject == null)
		{
			Debug.Log("pref==null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		GadgetArmoryItem component = gameObject2.GetComponent<GadgetArmoryItem>();
		if (this.gadgetPreview != null)
		{
			UnityEngine.Object.Destroy(this.gadgetPreview);
			this.gadgetPreview = null;
		}
		if (component.isReplaceOnlyHands)
		{
			this.characterInterface.skinCharacter.SetActive(true);
			if (this.armorPoint.childCount > 0)
			{
				ArmorRefs component2 = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
				if (component2 != null)
				{
					if (component2.leftBone != null)
					{
						Vector3 position = component2.leftBone.position;
						Quaternion rotation = component2.leftBone.rotation;
						component2.leftBone.parent = this.armorPoint.GetChild(0).GetChild(0);
						component2.leftBone.position = position;
						component2.leftBone.rotation = rotation;
					}
					if (component2.rightBone != null)
					{
						Vector3 position2 = component2.rightBone.position;
						Quaternion rotation2 = component2.rightBone.rotation;
						component2.rightBone.parent = this.armorPoint.GetChild(0).GetChild(0);
						component2.rightBone.position = position2;
						component2.rightBone.rotation = rotation2;
					}
				}
			}
			List<Transform> list = new List<Transform>();
			foreach (object obj in this.body.transform)
			{
				Transform item = (Transform)obj;
				list.Add(item);
			}
			foreach (Transform transform in list)
			{
				transform.parent = null;
				transform.position = new Vector3(0f, -10000f, 0f);
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
			gameObject2.transform.parent = this.body.transform;
			this.weapon = gameObject2;
			this.weapon.transform.localScale = new Vector3(1f, 1f, 1f);
			this.weapon.transform.position = this.body.transform.position;
			this.weapon.transform.localPosition = Vector3.zero;
			this.weapon.transform.localRotation = Quaternion.identity;
			if (this.armorPoint.childCount > 0 && component != null)
			{
				ArmorRefs component3 = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
				if (component3 != null)
				{
					if (component3.leftBone != null && component.LeftArmorHand != null)
					{
						component3.leftBone.parent = component.LeftArmorHand;
						component3.leftBone.localPosition = Vector3.zero;
						component3.leftBone.localRotation = Quaternion.identity;
						component3.leftBone.localScale = new Vector3(1f, 1f, 1f);
					}
					if (component3.rightBone != null && component.RightArmorHand != null)
					{
						component3.rightBone.parent = component.RightArmorHand;
						component3.rightBone.localPosition = Vector3.zero;
						component3.rightBone.localRotation = Quaternion.identity;
						component3.rightBone.localScale = new Vector3(1f, 1f, 1f);
					}
				}
			}
			if (SkinsController.currentSkinForPers != null)
			{
				this.SetSkinOnPers(SkinsController.currentSkinForPers);
			}
		}
		else
		{
			this.gadgetPreview = gameObject2;
			this.characterInterface.skinCharacter.SetActive(false);
			this.gadgetPreview.transform.SetParent(this.characterInterface.transform);
			this.gadgetPreview.transform.localScale = new Vector3(1f, 1f, 1f);
			this.gadgetPreview.transform.position = this.body.transform.position;
			this.gadgetPreview.transform.localPosition = Vector3.zero;
			this.gadgetPreview.transform.localRotation = Quaternion.identity;
		}
		Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
		ShopNGUIController.DisableLightProbesRecursively(gameObject2.gameObject);
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x001852EC File Offset: 0x001834EC
	public void ReloadCategoryTempItemsRemoved(List<string> expired)
	{
	}

	// Token: 0x060046BC RID: 18108 RVA: 0x001852F0 File Offset: 0x001834F0
	private void ReturnPersWearAndSkinWhenSwitching()
	{
		if (this.CurrentItem == null)
		{
			return;
		}
		string id = string.Empty;
		ShopNGUIController.CategoryNames category = this.CurrentItem.Category;
		switch (category)
		{
		case ShopNGUIController.CategoryNames.HatsCategory:
		case ShopNGUIController.CategoryNames.ArmorCategory:
		case ShopNGUIController.CategoryNames.CapesCategory:
		case ShopNGUIController.CategoryNames.BootsCategory:
		case ShopNGUIController.CategoryNames.MaskCategory:
			id = this.WearForCat(this.CurrentItem.Category);
			break;
		case ShopNGUIController.CategoryNames.SkinsCategory:
			if (SkinsController.currentSkinNameForPers != null)
			{
				id = SkinsController.currentSkinNameForPers;
			}
			else if (SkinsController.skinsForPers != null && SkinsController.skinsForPers.Keys.Count > 0)
			{
				id = SkinsController.skinsForPers.Keys.FirstOrDefault<string>();
			}
			break;
		default:
			if (category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory || category == ShopNGUIController.CategoryNames.PetsCategory)
			{
				try
				{
					id = Singleton<PetsManager>.Instance.GetEqipedPetId();
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in getting equipped pet id: {0}", new object[]
					{
						ex
					});
				}
			}
			break;
		}
		if (ShopNGUIController.IsWearCategory(this.CurrentItem.Category) || this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory || this.CurrentItem.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory || this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.UpdatePersWithNewItem(new ShopNGUIController.ShopItem(id, this.CurrentItem.Category));
		}
	}

	// Token: 0x060046BD RID: 18109 RVA: 0x00185474 File Offset: 0x00183674
	private ShopNGUIController.CategoryNames CategoryForSuperCategory(BtnCategory superCategory)
	{
		bool flag = !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted;
		if (flag && superCategory.btnName == ShopNGUIController.Supercategory.Wear.ToString())
		{
			try
			{
				foreach (ShopNGUIController.CategoryNames category in new ShopNGUIController.CategoryNames[]
				{
					ShopNGUIController.CategoryNames.MaskCategory,
					ShopNGUIController.CategoryNames.HatsCategory,
					ShopNGUIController.CategoryNames.CapesCategory,
					ShopNGUIController.CategoryNames.SkinsCategory,
					ShopNGUIController.CategoryNames.BootsCategory
				})
				{
					GameObject gameObject = this.TransformOfButtonForCategory(category).gameObject;
					UIWidget component = gameObject.GetChildGameObject("Pressed", false).GetComponent<UIWidget>();
					component.alpha = 0f;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in CategoryForSuperCategory, tutorial: {0}", new object[]
				{
					ex
				});
			}
			return ShopNGUIController.CategoryNames.ArmorCategory;
		}
		if (superCategory == null || superCategory.btnName == null)
		{
			Debug.LogError("CategoryForSuperCategory superCategory == null || superCategory.btnName == null");
			return ShopNGUIController.CategoryNames.PrimaryCategory;
		}
		return this.supercategoryLastUsedCategory[(ShopNGUIController.Supercategory)((int)Enum.Parse(typeof(ShopNGUIController.Supercategory), superCategory.btnName))];
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x001855A4 File Offset: 0x001837A4
	private IEnumerable<Transform> AllCategoryButtonTransforms()
	{
		return this.weaponCategoriesGrid.GetChildList().Concat(this.wearCategoriesGrid.GetChildList()).Concat(this.gridCategoriesLeague.GetChildList()).Concat(this.gadgetsGrid.GetChildList()).Concat(this.petsGrid.GetChildList()).Concat(this.bestCategoriesGrid.GetChildList());
	}

	// Token: 0x060046BF RID: 18111 RVA: 0x0018560C File Offset: 0x0018380C
	private Transform TransformOfButtonForCategory(ShopNGUIController.CategoryNames category)
	{
		return this.AllCategoryButtonTransforms().FirstOrDefault((Transform c) => c.name == category.ToString());
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x00185640 File Offset: 0x00183840
	private void SetToggleForCategory(ShopNGUIController.CategoryNames category)
	{
		Transform transform = this.TransformOfButtonForCategory(category);
		if (transform != null)
		{
			UIToggle component = transform.GetComponent<UIToggle>();
			if (component != null)
			{
				List<EventDelegate> onChange = component.onChange;
				component.onChange = new List<EventDelegate>();
				bool instantTween = component.instantTween;
				component.instantTween = true;
				component.value = true;
				component.onChange = onChange;
				component.instantTween = instantTween;
			}
			else
			{
				Debug.LogError("ChooseCategoryAndSuperCategory: uIToggle == null: category: " + category.ToString());
			}
		}
	}

	// Token: 0x060046C1 RID: 18113 RVA: 0x001856D0 File Offset: 0x001838D0
	public static int CurrentNumberOfUnlockedItems()
	{
		int result;
		try
		{
			Dictionary<ShopNGUIController.Supercategory, Dictionary<ShopNGUIController.CategoryNames, List<string>>> dictionary = ShopNGUIController.UnlockedItemsByCategoriesForCurrentShop();
			result = dictionary.Values.SelectMany((Dictionary<ShopNGUIController.CategoryNames, List<string>> itemsDict) => itemsDict.Values).Sum((List<string> list) => list.Count);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in CurrentNumberOfUnlockedItems: {0}", new object[]
			{
				ex
			});
			result = 0;
		}
		return result;
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x00185778 File Offset: 0x00183978
	private static Dictionary<ShopNGUIController.Supercategory, Dictionary<ShopNGUIController.CategoryNames, List<string>>> UnlockedItemsByCategoriesForCurrentShop()
	{
		if (PromoActionsManager.sharedManager == null)
		{
			Debug.LogErrorFormat("UnlockedItemsByCategoriesForCurrentShop: PromoActionsManager.sharedManager == null", new object[0]);
			return null;
		}
		Dictionary<ShopNGUIController.Supercategory, Dictionary<ShopNGUIController.CategoryNames, List<string>>> dictionary = new Dictionary<ShopNGUIController.Supercategory, Dictionary<ShopNGUIController.CategoryNames, List<string>>>();
		dictionary.Add(ShopNGUIController.Supercategory.Weapons, WeaponManager.GetWeaponTagsByCategoriesFromItems(PromoActionsManager.sharedManager.UnlockedItems).ToDictionary((KeyValuePair<ShopNGUIController.CategoryNames, List<string>> kvp) => kvp.Key, (KeyValuePair<ShopNGUIController.CategoryNames, List<string>> kvp) => kvp.Value.Intersect(from item in ShopNGUIController.GetItemNamesList(kvp.Key)
		select item.Id).ToList<string>()));
		dictionary.Add(ShopNGUIController.Supercategory.Gadgets, GadgetsInfo.GetGadgetsByCategoriesFromItems(PromoActionsManager.sharedManager.UnlockedItems).ToDictionary((KeyValuePair<GadgetInfo.GadgetCategory, List<string>> kvp) => (ShopNGUIController.CategoryNames)kvp.Key, (KeyValuePair<GadgetInfo.GadgetCategory, List<string>> kvp) => kvp.Value.Intersect(from item in ShopNGUIController.GetItemNamesList((ShopNGUIController.CategoryNames)kvp.Key)
		select item.Id).ToList<string>()));
		return dictionary;
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x0018585C File Offset: 0x00183A5C
	private void UpdateUnlockedItemsIndicators()
	{
		if (PromoActionsManager.sharedManager == null)
		{
			Debug.LogErrorFormat("UpdateUnlockedItems: PromoActionsManager.sharedManager == null", new object[0]);
			return;
		}
		try
		{
			Dictionary<ShopNGUIController.Supercategory, Dictionary<ShopNGUIController.CategoryNames, List<string>>> dictionary = ShopNGUIController.UnlockedItemsByCategoriesForCurrentShop();
			IEnumerable<ShopNGUIController.CategoryNames> enumerable = Enum.GetValues(typeof(ShopNGUIController.CategoryNames)).OfType<ShopNGUIController.CategoryNames>();
			Dictionary<ShopNGUIController.CategoryNames, List<string>> dictionary2 = dictionary.Values.SelectMany((Dictionary<ShopNGUIController.CategoryNames, List<string>> itemsDict) => itemsDict).ToDictionary((KeyValuePair<ShopNGUIController.CategoryNames, List<string>> kvp) => kvp.Key, (KeyValuePair<ShopNGUIController.CategoryNames, List<string>> kvp) => kvp.Value);
			foreach (ShopNGUIController.CategoryNames key in enumerable)
			{
				List<UILabel> list;
				if (this.m_categoriesToUnlockedItemsLabels.TryGetValue(key, out list))
				{
					List<string> list2;
					if (!dictionary2.TryGetValue(key, out list2) || list2 == null)
					{
						list2 = new List<string>();
					}
					int numberToDisplay = list2.Count<string>();
					bool flag = numberToDisplay > 0;
					list[0].transform.parent.gameObject.SetActiveSafeSelf(flag);
					if (flag)
					{
						list.ForEach(delegate(UILabel label)
						{
							label.text = numberToDisplay.ToString();
						});
					}
				}
			}
			foreach (ShopNGUIController.Supercategory key2 in dictionary.Keys)
			{
				int numOfItemsInSupercategory = dictionary[key2].Values.Sum((List<string> items) => items.Count);
				bool flag2 = numOfItemsInSupercategory > 0;
				List<UILabel> list3 = this.m_supercategoriesToUnlockedItemsLabels[key2];
				list3[0].transform.parent.gameObject.SetActiveSafeSelf(flag2);
				if (flag2)
				{
					list3.ForEach(delegate(UILabel label)
					{
						label.text = numOfItemsInSupercategory.ToString();
					});
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateUnlockedItems: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x00185B00 File Offset: 0x00183D00
	private static void RemoveViewedUnlockedItems()
	{
		try
		{
			PromoActionsManager.sharedManager.RemoveViewedUnlockedItems();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateUnlockedItems in removing unlocked items and saving: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x00185B54 File Offset: 0x00183D54
	public void SuperCategoryChoosen(BtnCategory superCategory)
	{
		ShopNGUIController.CategoryNames categoryNames = this.CategoryForSuperCategory(superCategory);
		ShopNGUIController.BestItemsToRemoveOnLeave.Clear();
		ShopNGUIController.RemoveViewedUnlockedItems();
		this.SetToggleForCategory(categoryNames);
		this.ChooseCategory(categoryNames, null, false);
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x00185B88 File Offset: 0x00183D88
	public void CategoryChoosen(UIToggle toggle)
	{
		if (!toggle.value)
		{
			return;
		}
		ShopNGUIController.CategoryNames newCategory = (ShopNGUIController.CategoryNames)((int)Enum.Parse(typeof(ShopNGUIController.CategoryNames), toggle.gameObject.name, true));
		ShopNGUIController.BestItemsToRemoveOnLeave.Clear();
		ShopNGUIController.RemoveViewedUnlockedItems();
		this.ChooseCategory(newCategory, null, false);
	}

	// Token: 0x060046C7 RID: 18119 RVA: 0x00185BDC File Offset: 0x00183DDC
	public static bool IsLeagueCategory(ShopNGUIController.CategoryNames category)
	{
		return category == ShopNGUIController.CategoryNames.LeagueHatsCategory || category == ShopNGUIController.CategoryNames.LeagueSkinsCategory || category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory;
	}

	// Token: 0x060046C8 RID: 18120 RVA: 0x00185C00 File Offset: 0x00183E00
	public static bool IsPetsOrEggsCategory(ShopNGUIController.CategoryNames category)
	{
		return category == ShopNGUIController.CategoryNames.PetsCategory || category == ShopNGUIController.CategoryNames.EggsCategory;
	}

	// Token: 0x060046C9 RID: 18121 RVA: 0x00185C18 File Offset: 0x00183E18
	public static bool IsGadgetsCategory(ShopNGUIController.CategoryNames category)
	{
		return category == ShopNGUIController.CategoryNames.ThrowingCategory || category == ShopNGUIController.CategoryNames.SupportCategory || category == ShopNGUIController.CategoryNames.ToolsCategoty;
	}

	// Token: 0x060046CA RID: 18122 RVA: 0x00185C3C File Offset: 0x00183E3C
	public static bool IsBestCategory(ShopNGUIController.CategoryNames category)
	{
		return category == ShopNGUIController.CategoryNames.BestWeapons || category == ShopNGUIController.CategoryNames.BestWear || category == ShopNGUIController.CategoryNames.BestGadgets;
	}

	// Token: 0x060046CB RID: 18123 RVA: 0x00185C60 File Offset: 0x00183E60
	public void ChooseCategoryAndSuperCategory(ShopNGUIController.CategoryNames category, ShopNGUIController.ShopItem itemToSet, bool initial)
	{
		try
		{
			this.superCategoriesButtonController.BtnClicked((!ShopNGUIController.IsWeaponCategory(category)) ? ((!ShopNGUIController.IsWearCategory(category)) ? ((!ShopNGUIController.IsLeagueCategory(category)) ? ((!ShopNGUIController.IsPetsOrEggsCategory(category)) ? ((!ShopNGUIController.IsGadgetsCategory(category)) ? ((category != ShopNGUIController.CategoryNames.SkinsCategory) ? ShopNGUIController.Supercategory.Best.ToString() : ShopNGUIController.Supercategory.Wear.ToString()) : ShopNGUIController.Supercategory.Gadgets.ToString()) : ShopNGUIController.Supercategory.Pets.ToString()) : ShopNGUIController.Supercategory.League.ToString()) : ShopNGUIController.Supercategory.Wear.ToString()) : ShopNGUIController.Supercategory.Weapons.ToString(), true);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in superCategoriesButtonController.BtnClicked: " + arg);
		}
		this.SetToggleForCategory(category);
		this.ChooseCategory(category, itemToSet, initial);
	}

	// Token: 0x060046CC RID: 18124 RVA: 0x00185D6C File Offset: 0x00183F6C
	public void ReloadGridOrCarousel(ShopNGUIController.ShopItem item)
	{
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			this.ReloadCarousel(item);
		}
		else
		{
			if (this.CurrentCategory != ShopNGUIController.CategoryNames.SniperCategory)
			{
				this.gridScrollView.enabled = true;
			}
			this.ReloadItemGrid(item);
		}
		this.UpdateTutorialState();
	}

	// Token: 0x060046CD RID: 18125 RVA: 0x00185DB8 File Offset: 0x00183FB8
	private static IEnumerable<string> CurrentWeaponSkinIds()
	{
		List<string> sortedSkinIds = (from skin in WeaponSkinsManager.AllSkins
		select skin.Id).ToList<string>();
		return from skin in WeaponSkinsManager.BoughtSkins().Union(WeaponSkinsManager.GetAvailableForBuySkins())
		orderby sortedSkinIds.IndexOf(skin.Id)
		select skin.Id;
	}

	// Token: 0x060046CE RID: 18126 RVA: 0x00185E44 File Offset: 0x00184044
	private void UpdateEggLabels()
	{
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x00185E48 File Offset: 0x00184048
	public void ShowLockOrPropertiesAndButtons()
	{
	}

	// Token: 0x060046D0 RID: 18128 RVA: 0x00185E4C File Offset: 0x0018404C
	private bool SetAppropeiateStateForPropertiesPanel()
	{
		bool state = false;
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory && Singleton<PetsManager>.Instance.PlayerPets.Count == 0)
		{
			state = false;
		}
		else if (this.CurrentItem != null)
		{
			state = this.NeedToShowPropertiesInCategory(this.CurrentItem.Category);
		}
		bool isHidden = this.hideButton.isHidden;
		this.hideButton.SetState(state);
		return this.hideButton.isHidden != isHidden;
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x00185ECC File Offset: 0x001840CC
	public void UpdatePetsCategoryIfNeeded()
	{
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.ChooseCategory(this.CurrentCategory, null, false);
		}
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x00185EF8 File Offset: 0x001840F8
	private void UpdateSkinShaderParams()
	{
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.SkinsCategory || this.CurrentCategory == ShopNGUIController.CategoryNames.CapesCategory || this.CurrentCategory == ShopNGUIController.CategoryNames.BestWear || this.CurrentCategory == ShopNGUIController.CategoryNames.LeagueSkinsCategory)
		{
			this.UpdatePointsForSkinsShader();
			this.SetArmoryCellShaderParams();
		}
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x00185F4C File Offset: 0x0018414C
	private void UpdateVisibilityOfPropertiesPanelAndButtons()
	{
		bool state = false;
		bool flag;
		if (this.CurrentItem != null)
		{
			ShopNGUIController.CategoryNames category = this.CurrentItem.Category;
			flag = ((category != ShopNGUIController.CategoryNames.ArmorCategory && category != ShopNGUIController.CategoryNames.SkinsCategory && category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory && !ShopNGUIController.IsWearCategory(category) && category != ShopNGUIController.CategoryNames.PetsCategory && (this.CurrentCategory != ShopNGUIController.CategoryNames.PetsCategory || Singleton<PetsManager>.Instance.PlayerPets.Count != 0)) || this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory);
			if (this.IsEmptyBestCategory())
			{
				flag = false;
			}
			state = (((flag && this.CurrentItem.Category != ShopNGUIController.CategoryNames.EggsCategory) || (category == ShopNGUIController.CategoryNames.PetsCategory && Singleton<PetsManager>.Instance.PlayerPets.Count > 0) || (ShopNGUIController.IsWearCategory(this.CurrentItem.Category) && this.CurrentItem.Category != ShopNGUIController.CategoryNames.ArmorCategory)) && TrainingController.TrainingCompleted && this.CurrentCategory != ShopNGUIController.CategoryNames.EggsCategory);
			if (this.IsEmptyBestCategory())
			{
				state = false;
			}
		}
		else
		{
			flag = (this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory);
		}
		if (this.infoButton != null)
		{
			this.infoButton.gameObject.SetActiveSafeSelf(flag);
		}
		if (this.hideButton != null)
		{
			this.hideButton.gameObject.SetActiveSafeSelf(state);
		}
		bool flag2 = this.SetAppropeiateStateForPropertiesPanel();
		if (this.gridScrollView.panel != null)
		{
			this.gridScrollView.panel.ResetAndUpdateAnchors();
		}
		if (flag2)
		{
			this.UpdateSkinShaderParams();
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x060046D4 RID: 18132 RVA: 0x00186104 File Offset: 0x00184304
	private ShopNGUIController.Supercategory CurrentSupercategory
	{
		get
		{
			return (ShopNGUIController.Supercategory)((int)Enum.Parse(typeof(ShopNGUIController.Supercategory), this.superCategoriesButtonController.currentBtnName));
		}
	}

	// Token: 0x060046D5 RID: 18133 RVA: 0x00186128 File Offset: 0x00184328
	private bool IsEmptyBestCategory()
	{
		return ShopNGUIController.IsBestCategory(this.CurrentCategory) && this.IsGridEmpty();
	}

	// Token: 0x060046D6 RID: 18134 RVA: 0x00186144 File Offset: 0x00184344
	public void ChooseCategory(ShopNGUIController.CategoryNames newCategory, ShopNGUIController.ShopItem itemToSet = null, bool initial = false)
	{
		ShopNGUIController.sharedShop.armorLock.SetActiveSafeSelf(Defs.isHunger && SceneManager.GetActiveScene().name != "ConnectScene" && newCategory == ShopNGUIController.CategoryNames.ArmorCategory);
		this.supercategoryLastUsedCategory[this.CurrentSupercategory] = newCategory;
		this.weaponCategoriesGrid.gameObject.SetActiveSafeSelf(ShopNGUIController.IsWeaponCategory(newCategory));
		this.wearCategoriesGrid.gameObject.SetActiveSafeSelf(ShopNGUIController.IsWearCategory(newCategory) || (newCategory == ShopNGUIController.CategoryNames.SkinsCategory && newCategory != ShopNGUIController.CategoryNames.LeagueSkinsCategory));
		this.armorCarousel.SetActiveSafeSelf(newCategory == ShopNGUIController.CategoryNames.ArmorCategory);
		this.gridCategoriesLeague.gameObject.SetActiveSafeSelf(newCategory == ShopNGUIController.CategoryNames.LeagueHatsCategory || newCategory == ShopNGUIController.CategoryNames.LeagueSkinsCategory || newCategory == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory);
		this.gadgetsGrid.gameObject.SetActiveSafeSelf(ShopNGUIController.IsGadgetsCategory(newCategory));
		this.gadgetBlocker.SetActiveSafeSelf((ShopNGUIController.IsGadgetsCategory(newCategory) || newCategory == ShopNGUIController.CategoryNames.BestGadgets) && WeaponManager.sharedManager.myPlayer != null);
		this.petsGrid.gameObject.SetActiveSafeSelf(ShopNGUIController.IsPetsOrEggsCategory(newCategory));
		this.bestCategoriesGrid.gameObject.SetActiveSafeSelf(newCategory == ShopNGUIController.CategoryNames.BestWeapons || newCategory == ShopNGUIController.CategoryNames.BestWear || newCategory == ShopNGUIController.CategoryNames.BestGadgets);
		this.gridScrollView.gameObject.SetActiveSafeSelf(newCategory != ShopNGUIController.CategoryNames.ArmorCategory);
		this.gridSlider.SetActiveSafeSelf(newCategory != ShopNGUIController.CategoryNames.ArmorCategory);
		try
		{
			int count = Singleton<EggsManager>.Instance.GetPlayerEggsInIncubator().Count;
			this.noEggs.SetActiveSafeSelf(newCategory == ShopNGUIController.CategoryNames.EggsCategory && count == 0);
			this.panelProperties.SetActiveSafeSelf(newCategory != ShopNGUIController.CategoryNames.EggsCategory);
			this.returnEveryDay.SetActiveSafeSelf(newCategory == ShopNGUIController.CategoryNames.EggsCategory);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in setting activity of noEggs: {0}", new object[]
			{
				ex
			});
		}
		WeaponManager.ClearCachedInnerPrefabs();
		if (!initial)
		{
			this.ReturnPersWearAndSkinWhenSwitching();
		}
		this.CurrentCategory = newCategory;
		if (!ShopNGUIController.IsWeaponCategory(this.CurrentCategory) && (this.weapon == null || this.gadgetPreview != null || (!ShopNGUIController.IsGadgetsCategory(this.CurrentCategory) && this.CurrentCategory != ShopNGUIController.CategoryNames.BestGadgets && this.weapon.GetComponent<WeaponSounds>() == null)) && this.CurrentCategory != ShopNGUIController.CategoryNames.BestWeapons)
		{
			this.SetWeapon(ShopNGUIController._CurrentWeaponSetIDs()[0] ?? WeaponManager._initialWeaponName, null);
		}
		if (ShopNGUIController.IsWeaponCategory(this.CurrentCategory))
		{
			string text = ShopNGUIController._CurrentWeaponSetIDs()[(int)this.CurrentCategory];
			if (itemToSet != null && text != null && itemToSet.Id == text && WeaponManager.sharedManager != null && WeaponManager.sharedManager.FilteredShopListsNoUpgrades.Count > (int)this.CurrentCategory)
			{
				ItemRecord rec = ItemDb.GetByTag(itemToSet.Id);
				if (rec != null && WeaponManager.sharedManager.FilteredShopListsNoUpgrades[(int)this.CurrentCategory].FirstOrDefault((GameObject go) => go.name == rec.PrefabName) == null)
				{
					itemToSet = null;
				}
			}
			if (itemToSet == null)
			{
				if (text != null)
				{
					itemToSet = new ShopNGUIController.ShopItem(text, this.CurrentCategory);
				}
				else
				{
					ShopNGUIController.CategoryNames categoryNames;
					string id = ShopNGUIController.HighestDPSGun(this.CurrentCategory, out categoryNames);
					if (this.CurrentCategory == categoryNames)
					{
						itemToSet = new ShopNGUIController.ShopItem(id, this.CurrentCategory);
					}
				}
			}
		}
		else
		{
			ShopNGUIController.CategoryNames currentCategory = this.CurrentCategory;
			switch (currentCategory)
			{
			case ShopNGUIController.CategoryNames.HatsCategory:
				if (itemToSet == null)
				{
					Dictionary<Wear.LeagueItemState, List<string>> dictionary = Wear.LeagueItems();
					string text2 = this.WearForCat(ShopNGUIController.CategoryNames.HatsCategory);
					string text3;
					if (text2 != ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.HatsCategory) && !dictionary[Wear.LeagueItemState.Purchased].Contains(text2) && WeaponManager.LastBoughtTag(text2, null) != null && WeaponManager.LastBoughtTag(text2, null) == text2)
					{
						text3 = text2;
					}
					else
					{
						text3 = ShopNGUIController.hats.FirstOrDefault((ShopPositionParams hat) => hat.name == "hat_Headphones").name;
					}
					string text4 = text3;
					if (!text4.IsNullOrEmpty())
					{
						itemToSet = new ShopNGUIController.ShopItem(text4, ShopNGUIController.CategoryNames.HatsCategory);
					}
				}
				break;
			case ShopNGUIController.CategoryNames.ArmorCategory:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
				{
					itemToSet = new ShopNGUIController.ShopItem(WeaponManager.LastBoughtTag("Armor_Army_1", null) ?? "Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory);
				}
				if (this.InTrainingAfterNoviceArmorRemoved)
				{
					itemToSet = new ShopNGUIController.ShopItem(WeaponManager.LastBoughtTag("Armor_Army_1", null) ?? "Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory);
				}
				else if (itemToSet == null)
				{
					string text5 = this.WearForCat(ShopNGUIController.CategoryNames.ArmorCategory);
					string text6 = (!(text5 != ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.ArmorCategory)) || WeaponManager.LastBoughtTag(text5, null) == null || !WeaponManager.LastBoughtTag(text5, null).Equals(text5)) ? WeaponManager.FirstUnboughtTag(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][0]) : text5;
					if (!text6.IsNullOrEmpty())
					{
						itemToSet = new ShopNGUIController.ShopItem(text6, ShopNGUIController.CategoryNames.ArmorCategory);
					}
				}
				this.scrollViewPanel.transform.localPosition = Vector3.zero;
				this.scrollViewPanel.clipOffset = new Vector2(0f, 0f);
				break;
			case ShopNGUIController.CategoryNames.SkinsCategory:
				if (itemToSet == null)
				{
					itemToSet = new ShopNGUIController.ShopItem("CustomSkinID", ShopNGUIController.CategoryNames.SkinsCategory);
				}
				break;
			case ShopNGUIController.CategoryNames.CapesCategory:
			case ShopNGUIController.CategoryNames.BootsCategory:
			case ShopNGUIController.CategoryNames.MaskCategory:
				if (itemToSet == null)
				{
					List<ShopPositionParams> list = (this.CurrentCategory != ShopNGUIController.CategoryNames.CapesCategory) ? ((this.CurrentCategory != ShopNGUIController.CategoryNames.BootsCategory) ? ShopNGUIController.masks : ShopNGUIController.boots) : ShopNGUIController.capes;
					int index = 0;
					string text7 = this.WearForCat(this.CurrentCategory);
					string text8 = (ShopNGUIController.NoneEquippedForWearCategory(this.CurrentCategory) == null || !(text7 != ShopNGUIController.NoneEquippedForWearCategory(this.CurrentCategory)) || WeaponManager.LastBoughtTag(text7, null) == null || !(WeaponManager.LastBoughtTag(text7, null) == text7) || this.CurrentCategory == ShopNGUIController.CategoryNames.CapesCategory) ? (WeaponManager.LastBoughtTag(list[index].name, null) ?? list[index].name) : text7;
					if (!text8.IsNullOrEmpty())
					{
						itemToSet = new ShopNGUIController.ShopItem(text8, this.CurrentCategory);
					}
				}
				break;
			default:
				if (currentCategory != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
				{
					if (currentCategory != ShopNGUIController.CategoryNames.LeagueHatsCategory)
					{
						if (currentCategory != ShopNGUIController.CategoryNames.LeagueSkinsCategory)
						{
							if (currentCategory != ShopNGUIController.CategoryNames.ThrowingCategory && currentCategory != ShopNGUIController.CategoryNames.ToolsCategoty && currentCategory != ShopNGUIController.CategoryNames.SupportCategory)
							{
								if (currentCategory != ShopNGUIController.CategoryNames.PetsCategory)
								{
									if (currentCategory != ShopNGUIController.CategoryNames.EggsCategory)
									{
										if (currentCategory == ShopNGUIController.CategoryNames.BestWeapons || currentCategory == ShopNGUIController.CategoryNames.BestWear || currentCategory == ShopNGUIController.CategoryNames.BestGadgets)
										{
											List<ShopNGUIController.ShopItem> itemNamesList = ShopNGUIController.GetItemNamesList(this.CurrentCategory);
											if (itemToSet == null || !itemNamesList.Contains(itemToSet))
											{
												ShopNGUIController.ShopItem shopItem = itemNamesList.FirstOrDefault<ShopNGUIController.ShopItem>();
												if (shopItem != null)
												{
													itemToSet = new ShopNGUIController.ShopItem(shopItem.Id, shopItem.Category);
												}
												else
												{
													this.SetWeapon(ShopNGUIController._CurrentWeaponSetIDs()[0] ?? WeaponManager._initialWeaponName, null);
												}
											}
										}
									}
									else if (itemToSet == null)
									{
										try
										{
											Egg egg = Singleton<EggsManager>.Instance.GetPlayerEggsInIncubator().FirstOrDefault<Egg>();
											if (egg != null && egg.Data != null)
											{
												itemToSet = new ShopNGUIController.ShopItem(egg.Id.ToString(), ShopNGUIController.CategoryNames.EggsCategory);
											}
										}
										catch (Exception ex2)
										{
											Debug.LogErrorFormat("Exception in setting idToSet for eggs category: {0}", new object[]
											{
												ex2
											});
										}
									}
								}
								else if (itemToSet == null)
								{
									string text9 = Singleton<PetsManager>.Instance.GetEqipedPetId();
									if (string.IsNullOrEmpty(text9))
									{
										text9 = (from playerPet in Singleton<PetsManager>.Instance.PlayerPets
										select playerPet.InfoId).FirstOrDefault<string>();
									}
									if (!text9.IsNullOrEmpty())
									{
										itemToSet = new ShopNGUIController.ShopItem(text9, ShopNGUIController.CategoryNames.PetsCategory);
									}
								}
							}
							else if (itemToSet == null)
							{
								string text10 = GadgetsInfo.EquippedForCategory((GadgetInfo.GadgetCategory)this.CurrentCategory);
								string text11 = (!text10.IsNullOrEmpty()) ? text10 : ShopNGUIController.GetItemNamesList(this.CurrentCategory).FirstOrDefault<ShopNGUIController.ShopItem>().Id;
								if (!text11.IsNullOrEmpty())
								{
									itemToSet = new ShopNGUIController.ShopItem(text11, this.CurrentCategory);
								}
							}
						}
						else if (itemToSet == null)
						{
							string text12;
							if (SkinsController.currentSkinNameForPers != null && SkinsController.leagueSkinsIds.Contains(SkinsController.currentSkinNameForPers))
							{
								text12 = SkinsController.currentSkinNameForPers;
							}
							else
							{
								text12 = SkinsController.leagueSkinsIds.FirstOrDefault<string>();
							}
							if (!text12.IsNullOrEmpty())
							{
								itemToSet = new ShopNGUIController.ShopItem(text12, ShopNGUIController.CategoryNames.SkinsCategory);
							}
						}
					}
					else
					{
						Dictionary<Wear.LeagueItemState, List<string>> dictionary2 = Wear.LeagueItems();
						string text13 = this.WearForCat(ShopNGUIController.CategoryNames.HatsCategory);
						string text14 = (!(text13 != ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.HatsCategory)) || !dictionary2[Wear.LeagueItemState.Purchased].Contains(text13) || WeaponManager.LastBoughtTag(text13, null) == null || !(WeaponManager.LastBoughtTag(text13, null) == text13)) ? (from item in dictionary2[Wear.LeagueItemState.Open].Union(dictionary2[Wear.LeagueItemState.Purchased])
						orderby Wear.LeagueForWear(item, ShopNGUIController.MapShopCategoryToItemCategory(this.CurrentCategory))
						select item).FirstOrDefault<string>() : text13;
						if (!text14.IsNullOrEmpty())
						{
							itemToSet = new ShopNGUIController.ShopItem(text14, ShopNGUIController.CategoryNames.HatsCategory);
						}
					}
				}
				else if (itemToSet == null)
				{
					IEnumerable<string> source = ShopNGUIController.CurrentWeaponSkinIds();
					string text15 = source.FirstOrDefault<string>();
					if (!text15.IsNullOrEmpty())
					{
						itemToSet = new ShopNGUIController.ShopItem(text15, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory);
					}
				}
				break;
			}
		}
		this.ReloadGridOrCarousel(itemToSet);
		this.ShowLockOrPropertiesAndButtons();
		this.needRefreshInLateUpdate = 2;
		bool flag = this.IsEmptyBestCategory();
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory || this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory || flag)
		{
			this.UpdateButtons();
		}
		if (flag || (this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory && Singleton<PetsManager>.Instance.PlayerPets.Count == 0))
		{
			this.ClearCaption();
		}
		if (this.noOffersAtThisTime != null)
		{
			this.noOffersAtThisTime.SetActiveSafeSelf(this.IsEmptyBestCategory());
		}
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x00186C3C File Offset: 0x00184E3C
	private void UpdatePropertiesPanels()
	{
		this.propertiesContainer.armorWearProperties.SetActiveSafeSelf(this.CurrentItem != null && this.CurrentItem.Category == ShopNGUIController.CategoryNames.ArmorCategory);
		this.propertiesContainer.nonArmorWearProperties.SetActiveSafeSelf(this.CurrentItem != null && ShopNGUIController.IsWearCategory(this.CurrentItem.Category) && this.CurrentItem.Category != ShopNGUIController.CategoryNames.ArmorCategory);
		if (this.propertiesContainer.skinProperties != null)
		{
			this.propertiesContainer.skinProperties.SetActiveSafeSelf(this.CurrentItem != null && (this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory || this.CurrentItem.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory));
		}
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x00186D14 File Offset: 0x00184F14
	public static void SetRenderersVisibleFromPoint(Transform pt, bool showArmor)
	{
		Player_move_c.PerformActionRecurs(pt.gameObject, delegate(Transform t)
		{
			Renderer component = t.GetComponent<Renderer>();
			if (component != null)
			{
				WearInvisbleParams component2 = t.GetComponent<WearInvisbleParams>();
				if (component2 != null)
				{
					if (component2.SkipSetInvisible)
					{
						return;
					}
					if (component2.HideIsInvisible)
					{
						t.gameObject.SetActive(showArmor);
						return;
					}
					if (showArmor)
					{
						component.material.shader = ((!component2.BaseShader.IsNullOrEmpty()) ? Shader.Find(component2.BaseShader) : Shader.Find("Mobile/Diffuse"));
					}
					else
					{
						component.material.shader = ((!component2.InvisibleShader.IsNullOrEmpty()) ? Shader.Find(component2.InvisibleShader) : Shader.Find("Mobile/Transparent-Shop"));
					}
				}
				else
				{
					component.material.shader = Shader.Find((!showArmor) ? "Mobile/Transparent-Shop" : "Mobile/Diffuse");
				}
			}
		});
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x00186D48 File Offset: 0x00184F48
	private void PetsManager_Instance_OnPlayerPetAdded(string petId)
	{
		if (ShopNGUIController.GuiActive)
		{
			EggHatchingWindowController[] componentsInChildren = this.rentScreenPoint.GetComponentsInChildren<EggHatchingWindowController>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				EggHatchingWindowController eggHatchingWindowController = componentsInChildren.FirstOrDefault<EggHatchingWindowController>();
				if (eggHatchingWindowController != null && eggHatchingWindowController.CurrentWindowMode == EggHatchingWindowController.WindowMode.Hatching)
				{
					return;
				}
			}
			if (this.CurrentCategory == ShopNGUIController.CategoryNames.PetsCategory)
			{
				this.ChooseCategory(this.CurrentCategory, null, false);
			}
			else
			{
				this.SetPetsCategoryEnable();
			}
		}
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x00186DC4 File Offset: 0x00184FC4
	private void Awake()
	{
		this.m_supercategoriesToUnlockedItemsLabels = new Dictionary<ShopNGUIController.Supercategory, List<UILabel>>
		{
			{
				ShopNGUIController.Supercategory.Weapons,
				this.weaponSupercategoryUnlockedItems
			},
			{
				ShopNGUIController.Supercategory.Gadgets,
				this.gadgetsSupercategoryUnlockedItems
			}
		};
		this.m_categoriesToUnlockedItemsLabels = new Dictionary<ShopNGUIController.CategoryNames, List<UILabel>>
		{
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				this.primaryWeaponsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.BackupCategory,
				this.backupWeaponsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.MeleeCategory,
				this.meleeWeaponsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.SpecilCategory,
				this.specialWeaponsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.SniperCategory,
				this.sniperWeaponsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.PremiumCategory,
				this.premiumWeaponsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.ThrowingCategory,
				this.throwingGadgetsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.ToolsCategoty,
				this.toolsGadgetsUnlockedItems
			},
			{
				ShopNGUIController.CategoryNames.SupportCategory,
				this.supportGadgetsUnlockedItems
			}
		};
		EggsManager.OnReadyToUse += this.EggsManager_OnReadyToUse;
		Singleton<PetsManager>.Instance.OnPlayerPetAdded += this.PetsManager_Instance_OnPlayerPetAdded;
		ShopCategoryButton.CategoryButtonClicked += delegate(ShopCategoryButton obj)
		{
			this.CategoryChoosen(obj.GetComponent<UIToggle>());
		};
		ArmoryCell.ToggleValueChanged += delegate(ArmoryCell cell)
		{
			if (cell.ItemId != this.CurrentItem.Id)
			{
				this.ChooseItem(new ShopNGUIController.ShopItem(cell.ItemId, cell.Category), false, false);
			}
		};
		ArmoryCell.Clicked += delegate(ArmoryCell cell)
		{
			if (!(cell.ItemId != this.CurrentItem.Id))
			{
				if (this.CurrentItem.Category != ShopNGUIController.CategoryNames.EggsCategory && ((!ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category) && this.CurrentCategory != ShopNGUIController.CategoryNames.BestGadgets) || !(WeaponManager.sharedManager.myPlayerMoveC != null)))
				{
					this.HandleInfoButton();
				}
			}
		};
		this.superCategoriesButtonController.actions.AddRange(new List<Action<BtnCategory>>
		{
			new Action<BtnCategory>(this.SuperCategoryChoosen),
			new Action<BtnCategory>(this.SuperCategoryChoosen),
			new Action<BtnCategory>(this.SuperCategoryChoosen),
			new Action<BtnCategory>(this.SuperCategoryChoosen),
			new Action<BtnCategory>(this.SuperCategoryChoosen),
			new Action<BtnCategory>(this.SuperCategoryChoosen),
			new Action<BtnCategory>(this.SuperCategoryChoosen)
		});
		this.CreateCharacterModel();
		this._rotationRateForCharacter = RilisoftRotator.RotationRateForCharacterInMenues;
		if (Application.isEditor && BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			this._rotationRateForCharacter *= 200f;
		}
		this._touchZoneForRotatingCharacter = new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height);
		ShopNGUIController._showArmorValue = (PlayerPrefs.GetInt("ShowArmorKeySetting", 1) == 1);
		ShopNGUIController._showWearValue = new bool?(PlayerPrefs.GetInt("ShowWearKeySetting", 1) == 1);
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		this.m_timeOfPLastStuffUpdate = Time.realtimeSinceStartup;
		if (this.tryGun != null)
		{
			this.tryGun.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				if (Storager.getInt("tutorial_button_try_highlighted", false) == 0)
				{
					Storager.setInt("tutorial_button_try_highlighted", 1, false);
				}
			};
		}
		List<EventDelegate> onChange = this.showArmorButton.onChange;
		this.showArmorButton.onChange = new List<EventDelegate>();
		bool instantTween = this.showArmorButton.instantTween;
		this.showArmorButton.instantTween = true;
		this.showArmorButton.Set(ShopNGUIController.ShowArmor);
		this.showArmorButton.onChange = onChange;
		this.showArmorButton.instantTween = instantTween;
		List<EventDelegate> onChange2 = this.showWearButton.onChange;
		this.showWearButton.onChange = new List<EventDelegate>();
		bool instantTween2 = this.showWearButton.instantTween;
		this.showWearButton.instantTween = true;
		this.showWearButton.Set(ShopNGUIController.ShowWear);
		this.showWearButton.onChange = onChange2;
		this.showWearButton.instantTween = instantTween2;
		ShopNGUIController.sharedShop = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.ActiveObject.SetActive(false);
		if (this.coinShopButton != null)
		{
			this.coinShopButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				if (Time.realtimeSinceStartup - this.timeOfEnteringShopForProtectFromPressingCoinsButton < 0.5f)
				{
					return;
				}
				if (BankController.Instance != null)
				{
					if (BankController.Instance.InterfaceEnabledCoroutineLocked)
					{
						Debug.LogWarning("InterfaceEnabledCoroutineLocked");
						return;
					}
					EventHandler handleBackFromBank = null;
					handleBackFromBank = delegate(object sender_, EventArgs e_)
					{
						if (BankController.Instance.InterfaceEnabledCoroutineLocked)
						{
							Debug.LogWarning("InterfaceEnabledCoroutineLocked");
							return;
						}
						BankController.Instance.BackRequested -= handleBackFromBank;
						BankController.Instance.InterfaceEnabled = false;
						this.m_itemToSetAfterEnter = this.CurrentItem;
						this.CategoryToChoose = this.CurrentCategory;
						ShopNGUIController.GuiActive = true;
						if (this.CurrentItem != null)
						{
							this.UpdatePersWithNewItem(this.CurrentItem);
						}
					};
					BankController.Instance.BackRequested += handleBackFromBank;
					BankController.Instance.InterfaceEnabled = true;
					ShopNGUIController.GuiActive = false;
				}
			};
		}
		if (this.backButton != null)
		{
			this.backButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
				{
					Debug.LogErrorFormat("trying to press back in shop when in bank", new object[0]);
					return;
				}
				base.StartCoroutine(this.BackAfterDelay());
			};
		}
		ShopNGUIController.hats.AddRange(Resources.LoadAll<ShopPositionParams>("Hats_Info"));
		this.sort(ShopNGUIController.hats, ShopNGUIController.CategoryNames.HatsCategory);
		ShopNGUIController.armor.AddRange(Resources.LoadAll<ShopPositionParams>("Armor_Info"));
		this.sort(ShopNGUIController.armor, ShopNGUIController.CategoryNames.ArmorCategory);
		ShopNGUIController.capes.AddRange(Resources.LoadAll<ShopPositionParams>("Capes_Info"));
		this.sort(ShopNGUIController.capes, ShopNGUIController.CategoryNames.CapesCategory);
		ShopNGUIController.masks.AddRange(Resources.LoadAll<ShopPositionParams>("Masks_Info"));
		this.sort(ShopNGUIController.masks, ShopNGUIController.CategoryNames.MaskCategory);
		ShopNGUIController.boots.AddRange(Resources.LoadAll<ShopPositionParams>("Shop_Boots_Info"));
		this.sort(ShopNGUIController.boots, ShopNGUIController.CategoryNames.BootsCategory);
		this.pixlMan = Resources.Load<GameObject>("Character_model");
		if (!Device.IsLoweMemoryDevice)
		{
			this._onPersArmorRefs = Resources.LoadAll<GameObject>("Armor_Shop");
		}
		if (Device.isPixelGunLow)
		{
			this._refOnLowPolyArmor = Resources.Load<GameObject>("Armor_Low");
			this._refsOnLowPolyArmorMaterials = Resources.LoadAll<Material>("LowPolyArmorMaterials");
		}
		Storager.SubscribeToChanged(Defs.TrainingCompleted_4_4_Sett, new Action(this.OnTrainingCompleted_4_4_Sett_Changed));
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x001872D0 File Offset: 0x001854D0
	private void EggsManager_OnReadyToUse(Egg egg)
	{
		if (ShopNGUIController.GuiActive && this.CurrentCategory == ShopNGUIController.CategoryNames.EggsCategory)
		{
			this.ChooseCategory(ShopNGUIController.CategoryNames.EggsCategory, null, false);
		}
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x001872FC File Offset: 0x001854FC
	public void GoToSkinsEditor()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("SkinEditorController"));
		SkinEditorController component = gameObject.GetComponent<SkinEditorController>();
		if (component != null)
		{
			Action<string> backHandler = null;
			backHandler = delegate(string newSkin)
			{
				SkinEditorController.ExitFromSkinEditor -= backHandler;
				MenuBackgroundMusic.sharedMusic.StopCustomMusicFrom(SkinEditorController.sharedController.gameObject);
				this.mainPanel.SetActive(true);
				this.ShowGridOrArmorCarousel();
				if (this.CurrentCategory == ShopNGUIController.CategoryNames.CapesCategory || newSkin != null)
				{
					if (this.CurrentItem.Id == "CustomSkinID")
					{
						this.SetSkinAsCurrent(newSkin);
					}
					if (this.CurrentCategory == ShopNGUIController.CategoryNames.SkinsCategory && this.CurrentItem.Id == SkinsController.currentSkinNameForPers)
					{
						this.FireOnEquipSkin(newSkin);
					}
					if (this.CurrentItem.Id == "cape_Custom")
					{
						this.EquipWear("cape_Custom");
					}
					this.ReloadAfterEditing(newSkin);
				}
				else
				{
					this.ReloadAfterEditing(newSkin);
				}
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC == null)
				{
					if (this.CurrentCategory != ShopNGUIController.CategoryNames.CapesCategory && PlayerPrefs.GetInt(Defs.ShownRewardWindowForSkin, 0) == 0)
					{
						this._shouldShowRewardWindowSkin = true;
					}
					if (this.CurrentCategory == ShopNGUIController.CategoryNames.CapesCategory && PlayerPrefs.GetInt(Defs.ShownRewardWindowForCape, 0) == 0)
					{
						this._shouldShowRewardWindowCape = true;
					}
				}
			};
			SkinEditorController.ExitFromSkinEditor += backHandler;
			SkinEditorController.currentSkinName = ((!(this.CurrentItem.Id == "CustomSkinID")) ? this.CurrentItem.Id : null);
			SkinEditorController.modeEditor = ((ShopNGUIController.MapShopCategoryToItemCategory(this.CurrentCategory) != ShopNGUIController.CategoryNames.SkinsCategory) ? SkinEditorController.ModeEditor.Cape : SkinEditorController.ModeEditor.SkinPers);
			this.mainPanel.SetActive(false);
		}
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x001873B8 File Offset: 0x001855B8
	public void ReloadAfterEditing(string n)
	{
		string id = n ?? ((this.CurrentCategory != ShopNGUIController.CategoryNames.SkinsCategory) ? "cape_Custom" : (this.CurrentItem.Id ?? "CustomSkinID"));
		this.ReloadGridOrCarousel(new ShopNGUIController.ShopItem(id, this.CurrentCategory));
		this.PlayPersAnimations();
		this.UpdateIcons(false);
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x0018741C File Offset: 0x0018561C
	private static List<Camera> BankRelatedCameras()
	{
		List<Camera> list = BankController.Instance.GetComponentsInChildren<Camera>(true).ToList<Camera>();
		if (FreeAwardController.Instance != null && FreeAwardController.Instance.renderCamera != null)
		{
			list.Add(FreeAwardController.Instance.renderCamera);
		}
		return list;
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x00187470 File Offset: 0x00185670
	public static void SetBankCamerasEnabled()
	{
		List<Camera> list = ShopNGUIController.BankRelatedCameras();
		foreach (Camera camera in list)
		{
			if (!(camera == null))
			{
				camera.enabled = true;
				if (camera.GetComponent<UICamera>() != null)
				{
					camera.GetComponent<UICamera>().enabled = true;
				}
				if (ShopNGUIController.disablesCameras.Contains(camera))
				{
					ShopNGUIController.disablesCameras.Remove(camera);
				}
			}
		}
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x00187524 File Offset: 0x00185724
	public void ShowGridOrArmorCarousel()
	{
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			if (!this.armorCarousel.activeSelf)
			{
				this.armorCarousel.SetActive(true);
			}
		}
		else if (!this.gridScrollView.gameObject.activeSelf)
		{
			this.gridScrollView.gameObject.SetActive(true);
		}
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x00187584 File Offset: 0x00185784
	private void BuyOrUpgradeWeapon(bool upgradeNotBuy = false)
	{
		string storeId = this.CurrentItem.Id;
		string itemId = this.CurrentItem.Id;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
		{
			storeId = WeaponManager.tagToStoreIDMapping[WeaponManager.FirstUnboughtOrForOurTier(itemId)];
		}
		else if (ShopNGUIController.IsWearCategory(this.CurrentItem.Category))
		{
			storeId = WeaponManager.FirstUnboughtTag(storeId);
			itemId = storeId;
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
		{
			string text = GadgetsInfo.FirstUnboughtOrForOurTier(this.CurrentItem.Id);
			storeId = text;
			itemId = text;
		}
		if (storeId == null)
		{
			return;
		}
		ItemPrice price = ShopNGUIController.GetItemPrice(this.CurrentItem.Id, this.CurrentItem.Category, upgradeNotBuy, true, false);
		ShopNGUIController.TryToBuy(this.mainPanel, price, delegate
		{
			try
			{
				if (upgradeNotBuy && Defs.isSoundFX)
				{
					NGUITools.PlaySound((this.CurrentItem.Category != ShopNGUIController.CategoryNames.PetsCategory) ? this.upgradeBtnSound : this.upgradeBtnPetSound);
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in playing sound on upgrade in shop: {0}", new object[]
				{
					ex
				});
			}
			if (Defs.isSoundFX)
			{
				UIPlaySound component = ((!upgradeNotBuy) ? this.buyButton : this.upgradeButton).GetComponent<UIPlaySound>();
				if (component != null)
				{
					component.Play();
				}
			}
			this.ActualBuy(storeId, itemId, price);
		}, delegate
		{
		}, null, delegate
		{
			this.PlayPersAnimations();
		}, delegate
		{
			ButtonClickSound.Instance.PlayClick();
			ShopNGUIController.SetBankCamerasEnabled();
		}, delegate
		{
			this.ShowGridOrArmorCarousel();
			this.SetOtherCamerasEnabled(false);
		});
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x0018770C File Offset: 0x0018590C
	private static void EquipWeaponSkinWrapper(string itemId)
	{
		try
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(itemId);
			if (skin != null)
			{
				if (!WeaponSkinsManager.SetSkinToWeapon(itemId, skin.ToWeapons[0]))
				{
					Debug.LogError("Error in setting weapon skin after giving: itemId = " + itemId);
				}
			}
			else
			{
				Debug.LogError("Error in giving weapon skin: skinInfo != null && skinInfo.ToWeapons != null, itemId = " + itemId);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Exception in giving weapon skin ",
				itemId,
				": ",
				ex
			}));
		}
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x001877B0 File Offset: 0x001859B0
	public static void ProvideItem(ShopNGUIController.CategoryNames category, string itemId, int gearCount = 1, bool buyArmorUpToSourceTg = false, int timeForRentIndexForOldTempWeapons = 0, Action<string> contextSpecificAction = null, Action<string> customEquipWearAction = null, bool equipSkin = true, bool equipWear = true, bool doAndroidCloudSync = true)
	{
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			itemId = GearManager.HolderQuantityForID(itemId);
		}
		string text = itemId;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(itemId))
		{
			text = WeaponManager.tagToStoreIDMapping[itemId];
		}
		if (text == null)
		{
			return;
		}
		ShopNGUIController.ProvideItemCore(category, text, itemId, delegate(string item)
		{
			if (customEquipWearAction != null)
			{
				customEquipWearAction(item);
			}
			else if (equipWear)
			{
				ShopNGUIController.SetAsEquippedAndSendToServer(item, category);
				ShopNGUIController.SendEquippedWearInCategory(item, category, string.Empty);
			}
		}, contextSpecificAction, delegate(string item)
		{
			if (equipSkin)
			{
				ShopNGUIController.SaveSkinAndSendToServer(item);
			}
		}, true, gearCount, buyArmorUpToSourceTg, timeForRentIndexForOldTempWeapons, doAndroidCloudSync);
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x0018784C File Offset: 0x00185A4C
	private static int AddedNumberOfGearWhenBuyingPack(string id)
	{
		int num = GearManager.ItemsInPackForGear(GearManager.HolderQuantityForID(id));
		if (Storager.getInt(id, false) + num > GearManager.MaxCountForGear(id))
		{
			num = GearManager.MaxCountForGear(id) - Storager.getInt(id, false);
		}
		return num;
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x0018788C File Offset: 0x00185A8C
	private static void ProvideItemCore(ShopNGUIController.CategoryNames category, string storeId, string itemId, Action<string> onEquipWearAction, Action<string> contextSpecificAction, Action<string> onSkinBoughtAction, bool giveOneItemOfGear = false, int gearCount = 1, bool buyArmorAndHatsUpToTg_UNUSED_NOW = false, int timeForRentIndex = 0, bool doAndroidCloudSync = true)
	{
		if (PromoActionsManager.sharedManager != null)
		{
			PromoActionsManager.sharedManager.RemoveItemFromUnlocked(itemId);
		}
		else
		{
			Debug.LogErrorFormat("SynchronizeIosWithCloud: PromoActionsManager.sharedManager == null", new object[0]);
		}
		if (ShopNGUIController.GunBought != null)
		{
			ShopNGUIController.GunBought();
		}
		if (ShopNGUIController.IsWearCategory(category))
		{
			Storager.setInt(itemId, 1, true);
			if (doAndroidCloudSync && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage >= TrainingController.NewTrainingCompletedStage.ShopCompleted))
			{
				ShopNGUIController.SynchronizeAndroidPurchases("Wear: " + itemId);
			}
			if (onEquipWearAction != null)
			{
				onEquipWearAction(itemId);
			}
		}
		if (ShopNGUIController.IsWeaponCategory(category) && WeaponManager.FirstUnboughtTag(itemId) != itemId)
		{
			List<string> list = WeaponUpgrades.ChainForTag(itemId);
			if (list != null)
			{
				int num = list.IndexOf(itemId) - 1;
				if (num >= 0)
				{
					for (int i = 0; i <= num; i++)
					{
						try
						{
							Storager.setInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list[i]]], 1, true);
						}
						catch
						{
							Debug.LogError("Error filling chain in indexOfWeaponBeforeCurrentTg");
						}
					}
				}
			}
		}
		WeaponManager.sharedManager.AddNewWeapon(storeId, timeForRentIndex);
		if (WeaponManager.sharedManager != null && ShopNGUIController.IsWeaponCategory(category))
		{
			try
			{
				string text = WeaponManager.LastBoughtTag(itemId, null);
				bool flag = WeaponManager.sharedManager.IsAvailableTryGun(text);
				bool flag2 = WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(text);
				WeaponManager.RemoveGunFromAllTryGunRelated(text);
				if (flag2)
				{
					string empty = string.Empty;
					string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(text, empty, category, null);
					AnalyticsStuff.LogWEaponsSpecialOffers_Conversion(false, itemNameNonLocalized);
				}
				if (flag2 || flag)
				{
					Action<string> tryGunBought = ShopNGUIController.TryGunBought;
					if (tryGunBought != null)
					{
						tryGunBought(text);
					}
					if (ABTestController.useBuffSystem)
					{
						BuffSystem.instance.OnTryGunBuyed(ItemDb.GetByTag(itemId).PrefabName);
					}
					else
					{
						KillRateCheck.OnTryGunBuyed();
					}
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in removeing TryGun structures: " + arg);
			}
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			if (storeId.Contains(GearManager.UpgradeSuffix))
			{
				string key = GearManager.NameForUpgrade(GearManager.HolderQuantityForID(storeId), GearManager.CurrentNumberOfUphradesForGear(GearManager.HolderQuantityForID(storeId)) + 1);
				Storager.setInt(key, 1, false);
			}
			else
			{
				int num2 = ShopNGUIController.AddedNumberOfGearWhenBuyingPack(storeId);
				Storager.setInt(storeId, Storager.getInt(storeId, false) + ((!giveOneItemOfGear) ? num2 : gearCount), false);
			}
		}
		if (category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			WeaponSkinsManager.ProvideSkin(itemId);
			ShopNGUIController.EquipWeaponSkinWrapper(itemId);
			if (doAndroidCloudSync)
			{
				ShopNGUIController.SynchronizeAndroidPurchases("Weapon skins");
			}
		}
		if (category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			PetUpdateInfo petUpdateInfo = Singleton<PetsManager>.Instance.Upgrade(itemId);
			if (petUpdateInfo != null && petUpdateInfo.PetNew != null && !petUpdateInfo.PetNew.InfoId.IsNullOrEmpty())
			{
				ShopNGUIController.EquipPet(petUpdateInfo.PetNew.InfoId);
			}
			else
			{
				Debug.LogErrorFormat("ProvideItemCore: error equipping pet after upgrading {0}", new object[]
				{
					itemId ?? "null"
				});
			}
		}
		if (ShopNGUIController.IsGadgetsCategory(category))
		{
			GadgetsInfo.ProvideGadget(itemId);
			ShopNGUIController.EquipGadget(itemId, (GadgetInfo.GadgetCategory)category);
			if (doAndroidCloudSync)
			{
				ShopNGUIController.SynchronizeAndroidPurchases("Gadgets");
			}
		}
		if (contextSpecificAction != null)
		{
			contextSpecificAction(storeId);
		}
		if (category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			if (storeId != null && SkinsController.shopKeyFromNameSkin.ContainsValue(storeId))
			{
				Storager.setInt(storeId, 1, true);
				if (doAndroidCloudSync)
				{
					ShopNGUIController.SynchronizeAndroidPurchases("Skin: " + storeId);
				}
			}
			if (onSkinBoughtAction != null)
			{
				onSkinBoughtAction(storeId);
			}
		}
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x00187C48 File Offset: 0x00185E48
	public void FireBuyAction(string item)
	{
		if (this.buyAction != null)
		{
			this.buyAction(item);
		}
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x00187C64 File Offset: 0x00185E64
	private void MarkItemAsToRemoveOnLeave(ShopNGUIController.ShopItem itemBefore)
	{
		ShopNGUIController.BestItemsToRemoveOnLeave.Add(itemBefore);
		List<string> previousUpgrades = new List<string>();
		try
		{
			List<string> list4;
			if (this.CurrentCategory == ShopNGUIController.CategoryNames.BestWeapons)
			{
				List<string> list5 = WeaponUpgrades.ChainForTag(itemBefore.Id);
				if (list5 != null)
				{
					previousUpgrades = list5.GetRange(0, list5.IndexOf(itemBefore.Id));
				}
			}
			else if (this.CurrentCategory == ShopNGUIController.CategoryNames.BestWear)
			{
				List<List<string>> list2;
				if (Wear.wear.TryGetValue(itemBefore.Category, out list2) && list2 != null)
				{
					List<string> list3 = list2.FirstOrDefault((List<string> list) => list.Contains(itemBefore.Id));
					if (list3 != null)
					{
						previousUpgrades = list3.GetRange(0, list3.IndexOf(itemBefore.Id));
					}
				}
			}
			else if (this.CurrentCategory == ShopNGUIController.CategoryNames.BestGadgets && GadgetsInfo.Upgrades.TryGetValue(itemBefore.Id, out list4) && list4 != null)
			{
				previousUpgrades = list4.GetRange(0, list4.IndexOf(itemBefore.Id));
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in MarkItemAsToRemoveOnLeave: {0}", new object[]
			{
				ex
			});
		}
		if (previousUpgrades != null && previousUpgrades.Count > 0)
		{
			ShopNGUIController.BestItemsToRemoveOnLeave.RemoveAll((ShopNGUIController.ShopItem shopItem) => previousUpgrades.Contains(shopItem.Id));
		}
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x00187E24 File Offset: 0x00186024
	private void ActualBuy(string storeId, string itemId, ItemPrice itemPrice)
	{
		if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.ArmorCategory || ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
		{
			ShopNGUIController.FireWeaponOrArmorBought();
		}
		ShopNGUIController.CategoryNames category = this.CurrentItem.Category;
		ShopNGUIController.ProvideItemCore(this.CurrentItem.Category, storeId, itemId, delegate(string item)
		{
			this.EquipWear(item);
		}, delegate(string item)
		{
			if (ShopNGUIController.IsWeaponCategory(category) || ShopNGUIController.IsWearCategory(category))
			{
				this.FireBuyAction(item);
			}
			this.purchaseSuccessful.SetActive(true);
			this._timePurchaseSuccessfulShown = Time.realtimeSinceStartup;
		}, delegate(string item)
		{
			this.SetSkinAsCurrent(item);
		}, false, 1, false, 0, !this.inGame);
		if (WeaponManager.tagToStoreIDMapping.ContainsValue(storeId))
		{
			IEnumerable<string> source = from item in WeaponManager.tagToStoreIDMapping
			where item.Value == storeId
			select item into kv
			select kv.Key;
			if (!this.inGame)
			{
				ShopNGUIController.SynchronizeAndroidPurchases("Weapon: " + (source.FirstOrDefault<string>() ?? "null"));
			}
		}
		try
		{
			string text = storeId;
			try
			{
				if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory && text != null && SkinsController.shopKeyFromNameSkin.ContainsKey(text))
				{
					text = SkinsController.shopKeyFromNameSkin[text];
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in setting shopId: " + arg);
			}
			string text2 = (!ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category)) ? (WeaponManager.LastBoughtTag(this.CurrentItem.Id, null) ?? WeaponManager.FirstUnboughtTag(this.CurrentItem.Id)) : (GadgetsInfo.LastBoughtFor(this.CurrentItem.Id) ?? GadgetsInfo.FirstUnbought(this.CurrentItem.Id));
			string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(text2, text, this.CurrentItem.Category, null);
			try
			{
				bool isDaterWeapon = false;
				if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
				{
					WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(text2);
					isDaterWeapon = (weaponInfo != null && weaponInfo.IsAvalibleFromFilter(3));
				}
				ShopNGUIController.CategoryNames categoryNames = (this.CurrentCategory != ShopNGUIController.CategoryNames.LeagueHatsCategory) ? this.CurrentItem.Category : ShopNGUIController.CategoryNames.LeagueHatsCategory;
				string text3 = AnalyticsConstants.GetSalesName(categoryNames) ?? categoryNames.ToString();
				AnalyticsStuff.LogSales(itemNameNonLocalized, text3, isDaterWeapon);
				AnalyticsFacade.InAppPurchase(itemNameNonLocalized, AnalyticsStuff.AnalyticsReadableCategoryNameFromOldCategoryName(text3), 1, itemPrice.Price, itemPrice.Currency);
				if (ShopNGUIController.IsBestCategory(this.CurrentCategory))
				{
					AnalyticsStuff.LogBestSales(itemNameNonLocalized, this.CurrentCategory);
				}
				try
				{
					if (this.GunThatWeUsedInPolygon != null && this.GunThatWeUsedInPolygon == WeaponManager.LastBoughtTag(itemId, null))
					{
						AnalyticsFacade.SendCustomEvent("Polygon", new Dictionary<string, object>
						{
							{
								"Conversion",
								"Buy"
							},
							{
								"Currency Spended",
								itemNameNonLocalized
							}
						});
						this.GunThatWeUsedInPolygon = null;
					}
				}
				catch (Exception arg2)
				{
					Debug.LogError("Exception in sending Polygon analytics: " + arg2);
				}
				if (this._isFromPromoActions && this._promoActionsIdClicked != null && text2 != null && this._promoActionsIdClicked == text2)
				{
					AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "Buy", text3 ?? "Unknown", itemNameNonLocalized);
				}
				this._isFromPromoActions = false;
			}
			catch (Exception arg3)
			{
				Debug.LogError("Exception in LogSales block in Shop: " + arg3);
			}
		}
		catch (Exception arg4)
		{
			Debug.LogError("Exception in Shop Logging: " + arg4);
		}
		string id = null;
		if (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
		{
			id = GadgetsInfo.LastBoughtFor(this.CurrentItem.Id);
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			try
			{
				PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(this.CurrentItem.Id);
				id = ((playerPet == null) ? this.CurrentItem.Id : playerPet.InfoId);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in getting actual upgrade of pet in ActualBuy: {0}", new object[]
				{
					ex
				});
				PlayerPet playerPet2 = Singleton<PetsManager>.Instance.PlayerPets.FirstOrDefault<PlayerPet>();
				if (playerPet2 != null)
				{
					id = playerPet2.InfoId;
				}
			}
		}
		else
		{
			id = WeaponManager.LastBoughtTag(this.CurrentItem.Id, null);
		}
		this.CurrentItem = new ShopNGUIController.ShopItem(id, this.CurrentItem.Category);
		this.UpdateIcons(true);
		if (ShopNGUIController.IsBestCategory(this.CurrentCategory))
		{
			this.MarkItemAsToRemoveOnLeave(this.CurrentItem);
		}
		this.ReloadGridOrCarousel(this.CurrentItem);
		Resources.UnloadUnusedAssets();
		try
		{
			this.AllArmoryCells.ForEach(delegate(ArmoryCell cell)
			{
				cell.GetComponent<UIPanel>().SetDirty();
			});
		}
		catch (Exception arg5)
		{
			Debug.LogError("Exception in AllArmoryCells.ForEach(cell => cell.GetComponent<UIPanel>().SetDirty()): " + arg5);
		}
		if (!this.inGame && this.CurrentItem.Id == "cape_Custom")
		{
			this.GoToSkinsEditor();
		}
		this.UpdateUnlockedItemsIndicators();
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x00188414 File Offset: 0x00186614
	private static void SaveSkinAndSendToServer(string id)
	{
		SkinsController.SetCurrentSkin(id);
		byte[] array = SkinsController.currentSkinForPers.EncodeToPNG();
		if (array != null)
		{
			string text = Convert.ToBase64String(array);
			if (text != null)
			{
				FriendsController.sharedController.skin = text;
				FriendsController.sharedController.SendOurData(true);
			}
		}
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x0018845C File Offset: 0x0018665C
	private void FireOnEquipSkin(string id)
	{
		if (this.onEquipSkinAction != null)
		{
			this.onEquipSkinAction(id);
		}
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x00188478 File Offset: 0x00186678
	public void SetSkinAsCurrent(string id)
	{
		ShopNGUIController.SaveSkinAndSendToServer(id);
		this.FireOnEquipSkin(id);
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x00188488 File Offset: 0x00186688
	public static void SetAsEquippedAndSendToServer(string itemId, ShopNGUIController.CategoryNames category)
	{
		if (category == ShopNGUIController.CategoryNames.BestWeapons || category == ShopNGUIController.CategoryNames.BestGadgets || category == ShopNGUIController.CategoryNames.BestWear)
		{
			Debug.LogError("Tried to pass best category to SetAsEquippedAndSendToServer: tg = " + itemId + "   c = " + category.ToString());
			return;
		}
		if (!ShopNGUIController.IsWearCategory(ShopNGUIController.MapShopCategoryToItemCategory(category)))
		{
			Debug.LogError("Tried to pass non-wear category to SetAsEquippedAndSendToServer: tg = " + itemId + "   c = " + category.ToString());
			return;
		}
		if (string.IsNullOrEmpty(itemId))
		{
			Debug.LogError("string.IsNullOrEmpty(tg) in SetAsEquippedAndSendToServer: tg = " + itemId + "   c = " + category.ToString());
			itemId = ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.MapShopCategoryToItemCategory(category));
		}
		Storager.setString(ShopNGUIController.SnForWearCategory(category), itemId, false);
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("FriendsController.sharedController == null");
			return;
		}
		FriendsController.sharedController.SendAccessories();
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x00188574 File Offset: 0x00186774
	public IEnumerator BackAfterDelay()
	{
		this.IsExiting = true;
		using (new ActionDisposable(delegate()
		{
			this.IsExiting = false;
		}))
		{
			this._isFromPromoActions = false;
			yield return null;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted && this.TutorialPhasePassed == ShopNGUIController.TutorialPhase.LeaveArmory)
			{
				TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShopCompleted;
				HintController.instance.StartShow();
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Back_Shop, 0);
				this.TutorialDisableHints();
			}
			if (this.resumeAction != null)
			{
				this.resumeAction();
			}
			else
			{
				ShopNGUIController.GuiActive = false;
			}
			if (this.wearResumeAction != null)
			{
				this.wearResumeAction();
			}
			if (this.InTrainingAfterNoviceArmorRemoved)
			{
				this.trainingColliders.SetActive(false);
				this.trainingRemoveNoviceArmorCollider.SetActive(false);
			}
			this.InTrainingAfterNoviceArmorRemoved = false;
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
			}
			this.GunThatWeUsedInPolygon = null;
		}
		yield break;
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x00188590 File Offset: 0x00186790
	public static string SnForWearCategory(ShopNGUIController.CategoryNames c)
	{
		return (c != ShopNGUIController.CategoryNames.CapesCategory) ? ((c != ShopNGUIController.CategoryNames.BootsCategory) ? ((c != ShopNGUIController.CategoryNames.ArmorCategory) ? ((c != ShopNGUIController.CategoryNames.MaskCategory) ? Defs.HatEquppedSN : "MaskEquippedSN") : Defs.ArmorNewEquppedSN) : Defs.BootsEquppedSN) : Defs.CapeEquppedSN;
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x001885EC File Offset: 0x001867EC
	public static string NoneEquippedForWearCategory(ShopNGUIController.CategoryNames c)
	{
		return (c != ShopNGUIController.CategoryNames.CapesCategory) ? ((c != ShopNGUIController.CategoryNames.BootsCategory) ? ((c != ShopNGUIController.CategoryNames.ArmorCategory) ? ((c != ShopNGUIController.CategoryNames.MaskCategory) ? Defs.HatNoneEqupped : "MaskNoneEquipped") : Defs.ArmorNewNoneEqupped) : Defs.BootsNoneEqupped) : Defs.CapeNoneEqupped;
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x00188648 File Offset: 0x00186848
	public string WearForCat(ShopNGUIController.CategoryNames c)
	{
		if (!ShopNGUIController.IsWearCategory(c))
		{
			return string.Empty;
		}
		try
		{
			return Storager.getString(ShopNGUIController.SnForWearCategory(c), false);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in WearForCat: " + arg);
		}
		return string.Empty;
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x001886B8 File Offset: 0x001868B8
	private void HandleOffersUpdated()
	{
		this.UpdateButtons();
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x001886C0 File Offset: 0x001868C0
	private void OnDestroy()
	{
		EggsManager.OnReadyToUse -= this.EggsManager_OnReadyToUse;
		Singleton<PetsManager>.Instance.OnPlayerPetAdded -= this.PetsManager_Instance_OnPlayerPetAdded;
		if (this.profile != null)
		{
			Resources.UnloadAsset(this.profile);
			this.profile = null;
		}
		Storager.UnSubscribeToChanged(Defs.TrainingCompleted_4_4_Sett, new Action(this.OnTrainingCompleted_4_4_Sett_Changed));
		PromoActionsManager.OnLockedItemsUpdated -= this.PromoActionsManager_OnLockedItemsUpdated;
	}

	// Token: 0x060046F3 RID: 18163 RVA: 0x00188740 File Offset: 0x00186940
	private void Start()
	{
		for (int i = 0; i < 100; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("ArmoryCell"));
			transform.SetParent(this.itemsGrid.transform, false);
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.name = i.ToString("D4");
			transform.localRotation = Quaternion.identity;
		}
		this.itemScrollBottomAnchor = this.gridScrollView.GetComponent<UIPanel>().bottomAnchor.absolute;
		this.itemScrollBottomAnchorRent = this.gridScrollView.GetComponent<UIPanel>().bottomAnchor.absolute + 25;
		this._skinsMakerSkinCache = Resources.Load<Texture>("skins_maker_skin");
		base.StartCoroutine(this.TryToShowExpiredBanner());
		PromoActionsManager.OnLockedItemsUpdated += this.PromoActionsManager_OnLockedItemsUpdated;
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x0018881C File Offset: 0x00186A1C
	private void PromoActionsManager_OnLockedItemsUpdated()
	{
		if (!ShopNGUIController.GuiActive)
		{
			return;
		}
		this.UpdateUnlockedItemsIndicators();
	}

	// Token: 0x060046F5 RID: 18165 RVA: 0x00188830 File Offset: 0x00186A30
	private void OnEnable()
	{
		this.UpdateTutorialState();
	}

	// Token: 0x060046F6 RID: 18166 RVA: 0x00188838 File Offset: 0x00186A38
	private void OnDisable()
	{
		if (this._tutorialCurrentState != null)
		{
			this._tutorialCurrentState.StageAct(ShopNGUIController.TutorialStageTrigger.Exit);
		}
	}

	// Token: 0x060046F7 RID: 18167 RVA: 0x00188858 File Offset: 0x00186A58
	private IEnumerator TryToShowExpiredBanner()
	{
		while (FriendsController.sharedController == null || TempItemsController.sharedController == null)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			try
			{
				if (ShopNGUIController.GuiActive)
				{
					if ((!(this.superCategoriesButtonController.currentBtnName == ShopNGUIController.Supercategory.Best.ToString()) || this.CurrentCategory != ShopNGUIController.CategoryNames.BestWear) && (!(this.superCategoriesButtonController.currentBtnName != ShopNGUIController.Supercategory.Best.ToString()) || ShopNGUIController.MapShopCategoryToItemCategory(this.CurrentCategory) != ShopNGUIController.CategoryNames.SkinsCategory) && this.superCategoriesButtonController.currentBtnName != ShopNGUIController.Supercategory.Wear.ToString() && this.rentScreenPoint.childCount == 0)
					{
						int readyEggsCount = 0;
						try
						{
							readyEggsCount = Singleton<EggsManager>.Instance.ReadyEggs().Count;
						}
						catch (Exception ex)
						{
							Exception e = ex;
							Debug.LogErrorFormat("Exception in getting number of ready eggs: {0}", new object[]
							{
								e
							});
						}
						if (readyEggsCount > 0)
						{
							Transform eggHatchingWindow = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("NguiWindows/PetWindows"));
							eggHatchingWindow.parent = this.rentScreenPoint;
							eggHatchingWindow.localPosition = Vector3.zero;
							eggHatchingWindow.localScale = new Vector3(1f, 1f, 1f);
							eggHatchingWindow.GetComponent<EggHatchingWindowController>().EggForHatching = Singleton<EggsManager>.Instance.ReadyEggs().First<Egg>();
						}
						else if (Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1)
						{
							GameObject window = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/WeRemoveNoviceArmorBanner"));
							window.transform.parent = this.rentScreenPoint;
							Player_move_c.SetLayerRecursively(window, LayerMask.NameToLayer("NGUIShop"));
							window.transform.localPosition = new Vector3(0f, 0f, -130f);
							window.transform.localRotation = Quaternion.identity;
							window.transform.localScale = new Vector3(1f, 1f, 1f);
							this.UpdatePersArmor(Defs.ArmorNewNoneEqupped);
							ShopNGUIController.sharedShop.trainingColliders.SetActive(true);
							ShopNGUIController.sharedShop.trainingRemoveNoviceArmorCollider.SetActive(true);
							this.InTrainingAfterNoviceArmorRemoved = true;
							if (HintController.instance != null)
							{
								HintController.instance.HideHintByName("shop_remove_novice_armor");
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Exception e2 = ex2;
				Debug.LogWarning("exception in Shop  TryToShowExpiredBanner: " + e2);
			}
		}
		yield break;
	}

	// Token: 0x060046F8 RID: 18168 RVA: 0x00188874 File Offset: 0x00186A74
	private void ReportCurrentVisibleCells()
	{
		try
		{
			if (!ShopNGUIController.IsBestCategory(this.CurrentCategory))
			{
				if (this.itemsGrid.gameObject.activeInHierarchy)
				{
					List<string> itemsViewed = (from cell in this.itemsGrid.GetComponentsInChildren<ArmoryCell>(false)
					where cell.IsFullyVisible
					select cell.ItemId).ToList<string>();
					if (PromoActionsManager.sharedManager != null)
					{
						int num = PromoActionsManager.sharedManager.ItemsViewed(itemsViewed);
						if (num > 0)
						{
							this.UpdateUnlockedItemsIndicators();
						}
					}
					else
					{
						Debug.LogErrorFormat("ShopNguiController.Update: PromoActionsManager.sharedManager == null", new object[0]);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ShopNguiController.Update: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x0018897C File Offset: 0x00186B7C
	private void Update()
	{
		if (!this.ActiveObject.activeInHierarchy)
		{
			return;
		}
		ExperienceController.sharedController.isShowRanks = (this.rentScreenPoint.childCount == 0 && SkinEditorController.sharedController == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled));
		if (Time.realtimeSinceStartup - this.m_timeOfPLastStuffUpdate >= 1f && this.CurrentCategory != ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.m_timeOfPLastStuffUpdate = Time.realtimeSinceStartup;
			if (ShopNGUIController.GuiActive && this.CurrentItem != null && this.CurrentItem.Id != null && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsWeaponDiscountedAsTryGun(this.CurrentItem.Id))
			{
				ShopNGUIController.UpdateTryGunDiscountTime(this.propertiesContainer, this.CurrentItem.Id);
			}
		}
		if (this.m_firstReportItemsViewedSkipped && Time.realtimeSinceStartup - this.m_timeOfLAstReportVisibleCells >= 0.2f)
		{
			this.m_timeOfLAstReportVisibleCells = Time.realtimeSinceStartup;
			this.ReportCurrentVisibleCells();
		}
		this.m_firstReportItemsViewedSkipped = true;
		bool state = this.superCategoriesButtonController.currentBtnName == ShopNGUIController.Supercategory.Wear.ToString() || this.CurrentCategory == ShopNGUIController.CategoryNames.BestWear || this.CurrentCategory == ShopNGUIController.CategoryNames.LeagueHatsCategory || this.CurrentCategory == ShopNGUIController.CategoryNames.LeagueSkinsCategory;
		this.showArmorButton.gameObject.SetActiveSafeSelf(state);
		this.showWearButton.gameObject.SetActiveSafeSelf(state);
		if (Time.realtimeSinceStartup - this._timePurchaseSuccessfulShown >= 2f)
		{
			this.purchaseSuccessful.SetActive(false);
		}
		if (this.mainPanel.activeInHierarchy && !HOTween.IsTweening(this.MainMenu_Pers) && ArmoryInfoScreenController.sharedController == null && !this.EggWindowIsOpened)
		{
			RilisoftRotator.RotateCharacter(this.MainMenu_Pers, this._rotationRateForCharacter, this._touchZoneForRotatingCharacter, ref this.idleTimerLastTime, ref this.lastTime, null);
		}
		if (Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			HOTween.Kill(this.MainMenu_Pers);
			Vector3 zero = Vector3.zero;
			this.idleTimerLastTime = Time.realtimeSinceStartup;
			HOTween.To(this.MainMenu_Pers, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(zero)).Ease(EaseType.Linear).OnComplete(delegate()
			{
				this.idleTimerLastTime = Time.realtimeSinceStartup;
			}));
		}
		if ((this.CurrentItem == null || this.CurrentItem.Category != ShopNGUIController.CategoryNames.CapesCategory) && Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			this.SetCamera();
		}
		ActivityIndicator.IsActiveIndicator = StoreKitEventListener.restoreInProcess;
		this.CheckCenterItemChanging();
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x00188C64 File Offset: 0x00186E64
	private void LateUpdate()
	{
		if (this.needRefreshInLateUpdate > 0)
		{
			if (this.gridScrollView.panel != null)
			{
				this.gridScrollView.panel.ResetAndUpdateAnchors();
				this.gridScrollView.panel.SetDirty();
				this.gridScrollView.panel.Refresh();
			}
			this.needRefreshInLateUpdate--;
			this.gridScrollView.DisableSpring();
			this.gridScrollView.RestrictWithinBounds(true);
		}
		if (ShopNGUIController.GuiActive)
		{
			if (this.updateScrollViewOnLateUpdateForTryPanel)
			{
				this.updateScrollViewOnLateUpdateForTryPanel = false;
				this.gridScrollView.DisableSpring();
				this.gridScrollView.RestrictWithinBounds(true);
				UIPanel component = this.gridScrollView.GetComponent<UIPanel>();
				component.SetDirty();
				component.Refresh();
			}
			if (!this.categoryGridsRepositioned)
			{
				this.AdjustCategoryGridCells();
				this.categoryGridsRepositioned = true;
				foreach (UIRect uirect in ShopNGUIController.sharedShop.widgetsToUpdateAnchorsOnStart)
				{
					uirect.ResetAndUpdateAnchors();
				}
				this.armoryRootPanel.Refresh();
			}
		}
		if (this.CurrentCategory == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			float num = this.scrollViewPanel.GetViewSize().x / 2f;
			ShopCarouselElement[] componentsInChildren = this.wrapContent.GetComponentsInChildren<ShopCarouselElement>(false);
			foreach (ShopCarouselElement shopCarouselElement in componentsInChildren)
			{
				Transform transform = shopCarouselElement.transform;
				float x = this.scrollViewPanel.clipOffset.x;
				float num2 = transform.localPosition.x - x - 20f;
				float num3 = Mathf.Abs(num2);
				float scaleCoef = this.scaleCoefficent + (1f - this.scaleCoefficent) * (1f - num3 / num);
				float num4 = 0.65f;
				if (num3 <= num / 3f)
				{
					scaleCoef = num4 + (1f - num4) * (1f - num3 / (num / 3f));
				}
				else
				{
					scaleCoef = this.scaleCoefficent + (num4 - this.scaleCoefficent) * (1f - (num3 - num / 3f) / (num * 3f / 3f));
				}
				if (num3 >= num * 0.75f)
				{
					scaleCoef = 0f;
				}
				float num5 = 0f;
				float num6 = (float)((num2 > 0f) ? -1 : 1);
				if (num2 != 0f)
				{
					if (num3 <= this.wrapContent.cellWidth)
					{
						num5 = this.firstOFfset * (num3 / this.wrapContent.cellWidth);
					}
					else if (num3 <= 2f * this.wrapContent.cellWidth)
					{
						num5 = this.firstOFfset + (this.secondOffset - this.firstOFfset) * ((num3 - this.wrapContent.cellWidth) / this.wrapContent.cellWidth);
					}
					else
					{
						num5 = this.secondOffset * (1f - (num3 - 2f * this.wrapContent.cellWidth) / this.wrapContent.cellWidth);
					}
				}
				num5 *= num6;
				if (!this.EnableConfigurePos || this.scrollViewPanel.GetComponent<UIScrollView>().isDragging || this.scrollViewPanel.GetComponent<UIScrollView>().currentMomentum.x > 0f)
				{
					shopCarouselElement.SetPos(scaleCoef, num5);
				}
				shopCarouselElement.topSeller.gameObject.SetActive(shopCarouselElement.showTS && num3 <= this.wrapContent.cellWidth / 10f);
				shopCarouselElement.newnew.gameObject.SetActive(shopCarouselElement.showNew && num3 <= this.wrapContent.cellWidth / 10f);
				shopCarouselElement.quantity.gameObject.SetActive(shopCarouselElement.showQuantity && num3 <= this.wrapContent.cellWidth / 10f);
			}
		}
		if (this._escapeRequested)
		{
			base.StartCoroutine(this.BackAfterDelay());
			this._escapeRequested = false;
		}
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x001890E4 File Offset: 0x001872E4
	private void HandleEscape()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage < TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			if (Application.isEditor)
			{
				Debug.Log("Ignoring [Escape] since Tutorial is not completed.");
			}
			return;
		}
		if (this.InTrainingAfterNoviceArmorRemoved)
		{
			if (Application.isEditor)
			{
				Debug.Log("Ignoring [Escape] since Tutorial after removing Novice Armor is not completed.");
			}
			return;
		}
		if (!ShopNGUIController.GuiActive)
		{
			if (Application.isEditor)
			{
				Debug.Log(base.GetType().Name + ".LateUpdate():    Ignoring Escape because Shop GUI is not active.");
			}
			return;
		}
		this._escapeRequested = true;
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x001891D4 File Offset: 0x001873D4
	public void SetInGame(bool e)
	{
		this.inGame = e;
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x001891E0 File Offset: 0x001873E0
	private IEnumerator DisableStub()
	{
		for (int i = 0; i < 3; i++)
		{
			yield return null;
		}
		this.stub.SetActive(false);
		yield break;
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x001891FC File Offset: 0x001873FC
	private void UpdateAllWearAndSkinOnPers()
	{
		this.UpdatePersHat(this.WearForCat(ShopNGUIController.CategoryNames.HatsCategory));
		this.UpdatePersCape(this.WearForCat(ShopNGUIController.CategoryNames.CapesCategory));
		this.UpdatePersArmor(this.WearForCat(ShopNGUIController.CategoryNames.ArmorCategory));
		this.UpdatePersBoots(this.WearForCat(ShopNGUIController.CategoryNames.BootsCategory));
		this.UpdatePersMask(this.WearForCat(ShopNGUIController.CategoryNames.MaskCategory));
		this.UpdatePersSkin(SkinsController.currentSkinNameForPers);
		try
		{
			this.UpdatePersPet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateAllWearAndSkinOnPers when updating pet: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x001892A4 File Offset: 0x001874A4
	private void MakeShopActive()
	{
		Light[] array = UnityEngine.Object.FindObjectsOfType<Light>() ?? new Light[0];
		foreach (Light light in array)
		{
			if (!this.mylights.Contains(light))
			{
				light.cullingMask &= ~(1 << LayerMask.NameToLayer("NGUIShop"));
				light.cullingMask &= ~(1 << LayerMask.NameToLayer("NGUIShopWorld"));
			}
		}
		ShopNGUIController.sharedShop.ActiveObject.SetActive(true);
		this.wrapContent.Reposition();
		if (ExperienceController.sharedController != null && ExpController.Instance != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
			ExpController.Instance.InterfaceEnabled = true;
		}
		this.UpdateAllWearAndSkinOnPers();
		MyCenterOnChild myCenterOnChild = this.carouselCenter;
		myCenterOnChild.onFinished = (SpringPanel.OnFinished)Delegate.Combine(myCenterOnChild.onFinished, new SpringPanel.OnFinished(this.HandleCarouselCentering));
		PromoActionsManager.ActionsUUpdated += this.HandleOffersUpdated;
		this.PlayPersAnimations();
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		ShopNGUIController.sharedShop.carouselCenter.enabled = true;
		this.AdjustCategoryButtonsForFilterMap();
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x001893E8 File Offset: 0x001875E8
	public void PlayPersAnimations()
	{
		this.PlayWeaponAnimation();
		this.PlayPetAnimation();
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x001893F8 File Offset: 0x001875F8
	private void SetArmoryCellShaderParams()
	{
		this.AllArmoryCells.ForEach(delegate(ArmoryCell armoryCell)
		{
			try
			{
				this.SetSliceShaderParams(armoryCell);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in setting OnApplicationPause SliceToWorldPosShader: " + arg);
			}
		});
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x00189414 File Offset: 0x00187614
	private IEnumerator OnApplicationPause(bool pauseStatus)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (!ShopNGUIController.GuiActive)
			{
				yield break;
			}
			yield return null;
			yield return null;
			yield return null;
			try
			{
				this.SetAppropeiateStateForPropertiesPanel();
				this.gridScrollView.MoveRelative(new Vector3(0f, 2f));
				if (this.CurrentCategory == ShopNGUIController.CategoryNames.SkinsCategory || this.CurrentCategory == ShopNGUIController.CategoryNames.CapesCategory || this.CurrentCategory == ShopNGUIController.CategoryNames.BestWear)
				{
					this.SetArmoryCellShaderParams();
				}
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Debug.LogError("Exception in ShopNGUIController OnApplicationPause: " + e);
			}
		}
		yield break;
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x00189430 File Offset: 0x00187630
	private void DisableButtonsInIndexes(List<ShopNGUIController.CategoryNames> indexesPar)
	{
		foreach (Transform transform in this.AllCategoryButtonTransforms())
		{
			transform.GetComponent<UIButton>().isEnabled = !indexesPar.Contains((ShopNGUIController.CategoryNames)((int)Enum.Parse(typeof(ShopNGUIController.CategoryNames), transform.name)));
		}
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x001894BC File Offset: 0x001876BC
	private void AdjustCategoryButtonsForFilterMap()
	{
		List<ShopNGUIController.CategoryNames> indexesPar = new List<ShopNGUIController.CategoryNames>();
		if (SceneLoader.ActiveSceneName.Equals("Sniper"))
		{
			indexesPar = new List<ShopNGUIController.CategoryNames>
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				ShopNGUIController.CategoryNames.SpecilCategory,
				ShopNGUIController.CategoryNames.PremiumCategory
			};
			this.DisableButtonsInIndexes(indexesPar);
		}
		else if (SceneLoader.ActiveSceneName.Equals("Knife"))
		{
			indexesPar = new List<ShopNGUIController.CategoryNames>
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				ShopNGUIController.CategoryNames.BackupCategory,
				ShopNGUIController.CategoryNames.SpecilCategory,
				ShopNGUIController.CategoryNames.PremiumCategory,
				ShopNGUIController.CategoryNames.SniperCategory
			};
			this.DisableButtonsInIndexes(indexesPar);
		}
		else
		{
			this.DisableButtonsInIndexes(indexesPar);
		}
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x00189560 File Offset: 0x00187760
	private void SetPetsCategoryEnable()
	{
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x00189564 File Offset: 0x00187764
	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x00189588 File Offset: 0x00187788
	private static string TemppOrHighestDPSGunInCategory(int cInt)
	{
		string text = null;
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.FilteredShopListsNoUpgrades != null && WeaponManager.sharedManager.FilteredShopListsNoUpgrades.Count > cInt)
		{
			List<GameObject> list = WeaponManager.sharedManager.FilteredShopListsNoUpgrades[cInt];
			GameObject gameObject = list.Find((GameObject w) => ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(w.name.Replace("(Clone)", string.Empty)).Tag));
			if (gameObject != null)
			{
				text = ItemDb.GetByPrefabName(gameObject.name.Replace("(Clone)", string.Empty)).Tag;
			}
			if (text == null && list.Count > 0)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					string tag = ItemDb.GetByPrefabName(list[i].name.Replace("(Clone)", string.Empty)).Tag;
					if (!ItemDb.IsTemporaryGun(tag) && ExpController.Instance != null && list[i].GetComponent<WeaponSounds>().tier <= ExpController.Instance.OurTier)
					{
						text = tag;
						break;
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06004708 RID: 18184 RVA: 0x001896C4 File Offset: 0x001878C4
	private static string HighestDPSGun(ShopNGUIController.CategoryNames desiredCategory, out ShopNGUIController.CategoryNames inFactCategory)
	{
		inFactCategory = desiredCategory;
		string text = ShopNGUIController.TemppOrHighestDPSGunInCategory((int)desiredCategory);
		if (text == null && WeaponManager.sharedManager.playerWeapons.Count > 0)
		{
			int num = (WeaponManager.sharedManager.playerWeapons[0] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
			text = ShopNGUIController.TemppOrHighestDPSGunInCategory(num);
			inFactCategory = (ShopNGUIController.CategoryNames)num;
		}
		return text;
	}

	// Token: 0x06004709 RID: 18185 RVA: 0x0018972C File Offset: 0x0018792C
	private void OnLevelWasLoaded(int level)
	{
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.SwitchAmbientLightAndFogToShop();
		}
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x00189740 File Offset: 0x00187940
	public void SetOtherCamerasEnabled(bool e)
	{
		if (e)
		{
			foreach (Camera camera in ShopNGUIController.disablesCameras)
			{
				if (!(camera == null))
				{
					camera.enabled = e;
					if (camera.GetComponent<UICamera>() != null)
					{
						camera.GetComponent<UICamera>().enabled = e;
					}
				}
			}
			ShopNGUIController.disablesCameras.Clear();
		}
		else
		{
			List<Camera> list = (Camera.allCameras ?? new Camera[0]).ToList<Camera>();
			List<Camera> collection = ProfileController.Instance.GetComponentsInChildren<Camera>(true).ToList<Camera>();
			list.AddRange(collection);
			list.AddRange(ShopNGUIController.BankRelatedCameras());
			foreach (Camera camera2 in list)
			{
				if (!(ExpController.Instance != null) || !ExpController.Instance.IsRenderedWithCamera(camera2))
				{
					if (!camera2.gameObject.CompareTag("CamTemp"))
					{
						if (!ShopNGUIController.sharedShop.ourCameras.Contains(camera2))
						{
							if (!object.ReferenceEquals(camera2, InfoWindowController.Instance.infoWindowCamera))
							{
								ShopNGUIController.disablesCameras.Add(camera2);
								camera2.enabled = e;
								if (camera2.GetComponent<UICamera>() != null)
								{
									camera2.GetComponent<UICamera>().enabled = e;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x0600470B RID: 18187 RVA: 0x0018991C File Offset: 0x00187B1C
	public bool IsFromPromoActions
	{
		get
		{
			return this._isFromPromoActions;
		}
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x00189924 File Offset: 0x00187B24
	public void IsInShopFromPromoPanel(bool isFromPromoACtions, string tg)
	{
		this._isFromPromoActions = isFromPromoACtions;
		this._promoActionsIdClicked = tg;
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x00189934 File Offset: 0x00187B34
	private static void SwitchAmbientLightAndFogToShop()
	{
		ShopNGUIController.sharedShop._storedAmbientLight = new Color?(RenderSettings.ambientLight);
		ShopNGUIController.sharedShop._storedFogEnabled = new bool?(RenderSettings.fog);
		RenderSettings.ambientLight = Defs.AmbientLightColorForShop();
		RenderSettings.fog = false;
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x0600470E RID: 18190 RVA: 0x0018997C File Offset: 0x00187B7C
	// (set) Token: 0x0600470F RID: 18191 RVA: 0x001899AC File Offset: 0x00187BAC
	public static bool GuiActive
	{
		get
		{
			return ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.ActiveObject.activeInHierarchy;
		}
		set
		{
			bool guiActive = ShopNGUIController.GuiActive;
			if (value)
			{
				if (ArmoryInfoScreenController.sharedController == null)
				{
					if (ShopNGUIController.sharedShop._backSubscription != null)
					{
						ShopNGUIController.sharedShop._backSubscription.Dispose();
					}
					ShopNGUIController.sharedShop._backSubscription = BackSystem.Instance.Register(new Action(ShopNGUIController.sharedShop.HandleEscape), "Shop");
				}
			}
			else
			{
				if (ShopNGUIController.sharedShop._tutorialCurrentState != null)
				{
					ShopNGUIController.sharedShop._tutorialCurrentState.StageAct(ShopNGUIController.TutorialStageTrigger.Exit);
				}
				if (ShopNGUIController.sharedShop._backSubscription != null)
				{
					if (ArmoryInfoScreenController.sharedController == null)
					{
						ShopNGUIController.sharedShop._backSubscription.Dispose();
						ShopNGUIController.sharedShop._backSubscription = null;
					}
					Storager.RefreshWeaponDigestIfDirty();
				}
			}
			if (value && ShopNGUIController.sharedShop.tryGun != null)
			{
				ShopNGUIController.sharedShop.tryGun.gameObject.SetActive(false);
			}
			FreeAwardShowHandler.CheckShowChest(value);
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.SetOtherCamerasEnabled(!value);
				if (value)
				{
					ShopNGUIController.sharedShop.stub.SetActive(true);
					try
					{
						ShopNGUIController.SwitchAmbientLightAndFogToShop();
						ShopNGUIController.sharedShop.timeOfEnteringShopForProtectFromPressingCoinsButton = Time.realtimeSinceStartup;
						ShopNGUIController.sharedShop.UpdateIcons(false);
						ShopNGUIController.CategoryNames categoryNames = ShopNGUIController.CategoryNames.PrimaryCategory;
						ShopNGUIController.ShopItem itemToSet;
						if (ShopNGUIController.sharedShop.m_itemToSetAfterEnter != null || ShopNGUIController.sharedShop.CategoryToChoose != ShopNGUIController.CategoryNames.PrimaryCategory)
						{
							itemToSet = ShopNGUIController.sharedShop.m_itemToSetAfterEnter;
							categoryNames = ShopNGUIController.sharedShop.CategoryToChoose;
							ShopNGUIController.sharedShop.m_itemToSetAfterEnter = null;
							ShopNGUIController.sharedShop.CategoryToChoose = ShopNGUIController.CategoryNames.PrimaryCategory;
						}
						else
						{
							ShopNGUIController.CategoryNames categoryNames2 = ShopNGUIController.CategoryNames.PrimaryCategory;
							if (WeaponManager.sharedManager != null && WeaponManager.sharedManager._currentFilterMap == 1)
							{
								categoryNames = ShopNGUIController.CategoryNames.MeleeCategory;
								categoryNames2 = ShopNGUIController.CategoryNames.MeleeCategory;
							}
							else if (WeaponManager.sharedManager != null && WeaponManager.sharedManager._currentFilterMap == 2)
							{
								categoryNames = ShopNGUIController.CategoryNames.SniperCategory;
								categoryNames2 = ShopNGUIController.CategoryNames.SniperCategory;
							}
							string text = ShopNGUIController._CurrentWeaponSetIDs()[(int)categoryNames2];
							if (text != null)
							{
								itemToSet = new ShopNGUIController.ShopItem(text, categoryNames2);
							}
							else
							{
								string id = ShopNGUIController.HighestDPSGun(categoryNames2, out categoryNames);
								itemToSet = new ShopNGUIController.ShopItem(id, categoryNames);
							}
						}
						string text2 = null;
						if (itemToSet != null)
						{
							if (ShopNGUIController.IsGadgetsCategory(categoryNames) || categoryNames == ShopNGUIController.CategoryNames.BestGadgets)
							{
								text2 = GadgetsInfo.LastBoughtFor(itemToSet.Id);
							}
							else if (categoryNames == ShopNGUIController.CategoryNames.PetsCategory)
							{
								text2 = itemToSet.Id;
								try
								{
									PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(itemToSet.Id);
									text2 = ((playerPet == null) ? itemToSet.Id : playerPet.InfoId);
								}
								catch (Exception ex)
								{
									Debug.LogErrorFormat("Exception in getting actual upgrade of pet in GuiActive: {0}", new object[]
									{
										ex
									});
								}
							}
							else
							{
								text2 = WeaponManager.LastBoughtTag(itemToSet.Id, null);
							}
							if (text2 == null)
							{
								if (ShopNGUIController.IsWearCategory(itemToSet.Category))
								{
									List<string> list3 = Wear.wear.Values.SelectMany((List<List<string>> listOfLists) => listOfLists).FirstOrDefault((List<string> list) => list.Contains(itemToSet.Id));
									if (list3 != null)
									{
										itemToSet = new ShopNGUIController.ShopItem(list3[0], itemToSet.Category);
									}
								}
							}
							else
							{
								itemToSet = new ShopNGUIController.ShopItem(text2, itemToSet.Category);
							}
						}
						ShopNGUIController.sharedShop.AllCategoryButtonTransforms().ForEach(delegate(Transform t)
						{
							t.GetComponent<UIToggle>().SetInstantlyNoHandlers(false);
						});
						ShopNGUIController.BestItemsToRemoveOnLeave.Clear();
						ShopNGUIController.sharedShop.MakeShopActive();
						ShopNGUIController.sharedShop.ChooseCategoryAndSuperCategory(categoryNames, itemToSet, true);
						ShopNGUIController.sharedShop.UpdateAllWearAndSkinOnPers();
						ShopNGUIController.sharedShop.AdjustCategoryGridCells();
						ShopNGUIController.sharedShop.SetPetsCategoryEnable();
						if (ArmoryInfoScreenController.sharedController != null)
						{
							ArmoryInfoScreenController.sharedController.DestroyWindow();
						}
						try
						{
							if (Defs.isHunger && SceneManager.GetActiveScene().name != "ConnectScene")
							{
								List<ShopNGUIController.CategoryNames> second = (from weapon in WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>()
								select (ShopNGUIController.CategoryNames)(weapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1)).Distinct<ShopNGUIController.CategoryNames>().ToList<ShopNGUIController.CategoryNames>();
								List<ShopNGUIController.CategoryNames> list2 = new List<ShopNGUIController.CategoryNames>
								{
									ShopNGUIController.CategoryNames.PrimaryCategory,
									ShopNGUIController.CategoryNames.BackupCategory,
									ShopNGUIController.CategoryNames.MeleeCategory,
									ShopNGUIController.CategoryNames.SpecilCategory,
									ShopNGUIController.CategoryNames.SniperCategory,
									ShopNGUIController.CategoryNames.PremiumCategory
								};
								list2 = list2.Except(second).ToList<ShopNGUIController.CategoryNames>();
								ShopNGUIController.sharedShop.AdjustCategoryButtonsForFilterMap();
								ShopNGUIController.sharedShop.DisableButtonsInIndexes(list2);
							}
						}
						catch (Exception arg)
						{
							Debug.LogError("Exception in disabling buttons for hunger: " + arg);
						}
					}
					catch (Exception arg2)
					{
						Debug.LogError("Exception in ShopNGUIController.GuiActive: " + arg2);
					}
					ShopNGUIController.sharedShop.StartCoroutine(ShopNGUIController.sharedShop.DisableStub());
					ShopNGUIController.sharedShop.UpdateUnlockedItemsIndicators();
					ShopNGUIController.RemoveViewedUnlockedItems();
				}
				else
				{
					Color? storedAmbientLight = ShopNGUIController.sharedShop._storedAmbientLight;
					RenderSettings.ambientLight = ((storedAmbientLight == null) ? RenderSettings.ambientLight : storedAmbientLight.Value);
					bool? storedFogEnabled = ShopNGUIController.sharedShop._storedFogEnabled;
					RenderSettings.fog = ((storedFogEnabled == null) ? RenderSettings.fog : storedFogEnabled.Value);
					MyCenterOnChild myCenterOnChild = ShopNGUIController.sharedShop.carouselCenter;
					myCenterOnChild.onFinished = (SpringPanel.OnFinished)Delegate.Remove(myCenterOnChild.onFinished, new SpringPanel.OnFinished(ShopNGUIController.sharedShop.HandleCarouselCentering));
					PromoActionsManager.ActionsUUpdated -= ShopNGUIController.sharedShop.HandleOffersUpdated;
					ShopNGUIController.sharedShop.SetWeapon(null, null);
					ShopNGUIController.sharedShop.ActiveObject.SetActive(false);
					ShopNGUIController.sharedShop.carouselCenter.enabled = false;
					WeaponManager.ClearCachedInnerPrefabs();
					ShopNGUIController.sharedShop.AllArmoryCells.ForEach(delegate(ArmoryCell cell)
					{
						if (cell.icon != null)
						{
							cell.icon.mainTexture = null;
						}
					});
				}
			}
			if (guiActive != ShopNGUIController.GuiActive)
			{
				if (!ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.characterInterface.UpdatePet(string.Empty);
				}
				Action<bool> shopChangedIsActive = ShopNGUIController.ShopChangedIsActive;
				if (shopChangedIsActive != null)
				{
					shopChangedIsActive(ShopNGUIController.GuiActive);
				}
			}
		}
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x0018A0A0 File Offset: 0x001882A0
	[ContextMenu("AdjustCategoryGridCells")]
	public void AdjustCategoryGridCells()
	{
		float a = (float)this.weaponCategoriesGrid.GetComponentInChildren<UIToggle>().activeSprite.width * 2f;
		float b = (float)this.sccrollViewBackground.width / 6f - 3f;
		this.weaponCategoriesGrid.cellWidth = Mathf.Min(a, b);
		this.wearCategoriesGrid.cellWidth = Mathf.Min((float)this.wearCategoriesGrid.GetComponentInChildren<UIToggle>().activeSprite.width * 2f, (float)this.sccrollViewBackground.width / 6f);
		this.gridCategoriesLeague.cellWidth = Mathf.Min((float)this.gridCategoriesLeague.GetComponentInChildren<UIToggle>().activeSprite.width * 2f, (float)this.sccrollViewBackground.width / 3f);
		this.gadgetsGrid.cellWidth = Mathf.Min((float)this.gadgetsGrid.GetComponentInChildren<UIToggle>().activeSprite.width * 2f, (float)this.sccrollViewBackground.width / 3f);
		this.petsGrid.cellWidth = Mathf.Min((float)this.petsGrid.GetComponentInChildren<UIToggle>().activeSprite.width * 2f, (float)this.sccrollViewBackground.width / 3f);
		this.bestCategoriesGrid.cellWidth = Mathf.Min((float)this.bestCategoriesGrid.GetComponentInChildren<UIToggle>().activeSprite.width * 2f, (float)this.sccrollViewBackground.width / 3f);
		this.weaponCategoriesGrid.Reposition();
		this.wearCategoriesGrid.Reposition();
		this.gridCategoriesLeague.Reposition();
		this.gadgetsGrid.Reposition();
		this.petsGrid.Reposition();
		this.bestCategoriesGrid.Reposition();
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x0018A270 File Offset: 0x00188470
	public static void DisableLightProbesRecursively(GameObject w)
	{
		Player_move_c.PerformActionRecurs(w, delegate(Transform t)
		{
			MeshRenderer component = t.GetComponent<MeshRenderer>();
			SkinnedMeshRenderer component2 = t.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				component.useLightProbes = false;
			}
			if (component2 != null)
			{
				component2.useLightProbes = false;
			}
		});
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x0018A298 File Offset: 0x00188498
	public void SetWeapon(string weaponTag, string weaponSkinId)
	{
		this.characterInterface.skinCharacter.SetActive(true);
		if (this.gadgetPreview != null)
		{
			UnityEngine.Object.Destroy(this.gadgetPreview);
			this.gadgetPreview = null;
		}
		this.animationCoroutineRunner.StopAllCoroutines();
		if (WeaponManager.sharedManager == null)
		{
			return;
		}
		if (this.armorPoint.childCount > 0)
		{
			ArmorRefs component = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					Vector3 position = component.leftBone.position;
					Quaternion rotation = component.leftBone.rotation;
					component.leftBone.parent = this.armorPoint.GetChild(0).GetChild(0);
					component.leftBone.position = position;
					component.leftBone.rotation = rotation;
				}
				if (component.rightBone != null)
				{
					Vector3 position2 = component.rightBone.position;
					Quaternion rotation2 = component.rightBone.rotation;
					component.rightBone.parent = this.armorPoint.GetChild(0).GetChild(0);
					component.rightBone.position = position2;
					component.rightBone.rotation = rotation2;
				}
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (object obj in this.body.transform)
		{
			Transform item = (Transform)obj;
			list.Add(item);
		}
		foreach (Transform transform in list)
		{
			transform.parent = null;
			transform.position = new Vector3(0f, -10000f, 0f);
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (weaponTag == null)
		{
			return;
		}
		if (this.profile != null)
		{
			Resources.UnloadAsset(this.profile);
			this.profile = null;
		}
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null || string.IsNullOrEmpty(byTag.PrefabName))
		{
			Debug.Log("rec == null || string.IsNullOrEmpty(rec.PrefabName)");
			return;
		}
		GameObject gameObject = Resources.Load<GameObject>("Weapons/" + byTag.PrefabName);
		if (gameObject == null)
		{
			Debug.Log("pref==null");
			return;
		}
		this.profile = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		try
		{
			GameObject gameObject3 = gameObject2.GetComponent<WeaponSounds>()._innerPars.gameObject;
			if (weaponSkinId != null)
			{
				WeaponSkinsManager.GetSkin(weaponSkinId).SetTo(gameObject3);
			}
			else
			{
				WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(byTag.PrefabName);
				if (skinForWeapon != null)
				{
					skinForWeapon.SetTo(gameObject2);
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in setting weapon skin in SetWeapon: " + arg);
		}
		Player_move_c.SetLayerRecursively(gameObject2, LayerMask.NameToLayer("NGUIShop"));
		gameObject2.transform.parent = this.body.transform;
		this.weapon = gameObject2;
		this.weapon.transform.localScale = new Vector3(1f, 1f, 1f);
		this.weapon.transform.position = this.body.transform.position;
		this.weapon.transform.localPosition = Vector3.zero;
		this.weapon.transform.localRotation = Quaternion.identity;
		WeaponSounds component2 = this.weapon.GetComponent<WeaponSounds>();
		if (this.armorPoint.childCount > 0 && component2 != null)
		{
			ArmorRefs component3 = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component3 != null)
			{
				if (component3.leftBone != null && component2.LeftArmorHand != null)
				{
					component3.leftBone.parent = component2.LeftArmorHand;
					component3.leftBone.localPosition = Vector3.zero;
					component3.leftBone.localRotation = Quaternion.identity;
					component3.leftBone.localScale = new Vector3(1f, 1f, 1f);
				}
				if (component3.rightBone != null && component2.RightArmorHand != null)
				{
					component3.rightBone.parent = component2.RightArmorHand;
					component3.rightBone.localPosition = Vector3.zero;
					component3.rightBone.localRotation = Quaternion.identity;
					component3.rightBone.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		this.PlayWeaponAnimation();
		this.DisableGunflashes(this.weapon);
		if (SkinsController.currentSkinForPers != null)
		{
			this.SetSkinOnPers(SkinsController.currentSkinForPers);
		}
		this._assignedWeaponTag = weaponTag;
		ShopNGUIController.DisableLightProbesRecursively(gameObject2.gameObject);
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x0018A834 File Offset: 0x00188A34
	internal static void SynchronizeAndroidPurchases(string comment)
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return;
		}
		Debug.LogFormat("Trying to synchronize purchases to cloud ({0})", new object[]
		{
			comment
		});
		Action ResetWeaponManager = delegate()
		{
			PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
			if (WeaponManager.sharedManager != null)
			{
				int currentWeaponIndex = WeaponManager.sharedManager.CurrentWeaponIndex;
				string name = SceneManager.GetActiveScene().name;
				int filterMap;
				if (!Defs.filterMaps.TryGetValue(name, out filterMap))
				{
					filterMap = 0;
				}
				WeaponManager.sharedManager.Reset(filterMap);
				WeaponManager.sharedManager.CurrentWeaponIndex = currentWeaponIndex;
			}
			if (ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.UpdateIcons(false);
			}
		};
		Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
		if (androidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			if (androidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				PlayerPrefs.SetInt("PendingGooglePlayGamesSync", 1);
				PurchasesSynchronizer.Instance.AuthenticateAndSynchronize(delegate(bool success)
				{
					Debug.LogFormat("[Rilisoft] ShopNguiController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", new object[]
					{
						success,
						Time.realtimeSinceStartup
					});
					try
					{
						Debug.LogFormat("Google purchases syncronized ({0}): {1}", new object[]
						{
							comment,
							success
						});
						if (success)
						{
							ResetWeaponManager();
						}
					}
					finally
					{
						Debug.LogFormat("[Rilisoft] ShopNguiController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", new object[]
						{
							success,
							Time.realtimeSinceStartup
						});
					}
				}, true);
			}
		}
		else
		{
			PurchasesSynchronizer.Instance.SynchronizeAmazonPurchases();
			ResetWeaponManager();
		}
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x0018A8F8 File Offset: 0x00188AF8
	public static void FireWeaponOrArmorBought()
	{
		Action gunOrArmorBought = ShopNGUIController.GunOrArmorBought;
		if (gunOrArmorBought != null)
		{
			gunOrArmorBought();
		}
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x0018A918 File Offset: 0x00188B18
	public void SetItemToShow(ShopNGUIController.ShopItem itemToSet)
	{
		this.m_itemToSetAfterEnter = itemToSet;
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06004716 RID: 18198 RVA: 0x0018A924 File Offset: 0x00188B24
	public CharacterInterface ShopCharacterInterface
	{
		get
		{
			return this.characterInterface;
		}
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x0018A92C File Offset: 0x00188B2C
	private void CreateCharacterModel()
	{
		GameObject original = Resources.Load("Character_model") as GameObject;
		this.characterInterface = UnityEngine.Object.Instantiate<GameObject>(original).GetComponent<CharacterInterface>();
		this.characterPoint.localPosition = new Vector3(-2.64f - ((float)Screen.width / (float)Screen.height - 1.333f) * 0.927f, this.characterPoint.localPosition.y, this.characterPoint.localPosition.z);
		this.characterInterface.transform.SetParent(this.characterPoint.GetChild(0), false);
		this.characterInterface.SetCharacterType(false, false, false);
		ShopNGUIController.DisableLightProbesRecursively(this.characterInterface.gameObject);
		Player_move_c.SetLayerRecursively(this.characterInterface.gameObject, this.characterPoint.gameObject.layer);
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06004718 RID: 18200 RVA: 0x0018AA0C File Offset: 0x00188C0C
	public Transform armorPoint
	{
		get
		{
			return this.characterInterface.armorPoint;
		}
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06004719 RID: 18201 RVA: 0x0018AA1C File Offset: 0x00188C1C
	public Transform leftBootPoint
	{
		get
		{
			return this.characterInterface.leftBootPoint;
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x0600471A RID: 18202 RVA: 0x0018AA2C File Offset: 0x00188C2C
	public Transform rightBootPoint
	{
		get
		{
			return this.characterInterface.rightBootPoint;
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x0600471B RID: 18203 RVA: 0x0018AA3C File Offset: 0x00188C3C
	public Transform capePoint
	{
		get
		{
			return this.characterInterface.capePoint;
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x0600471C RID: 18204 RVA: 0x0018AA4C File Offset: 0x00188C4C
	public Transform hatPoint
	{
		get
		{
			return this.characterInterface.hatPoint;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x0600471D RID: 18205 RVA: 0x0018AA5C File Offset: 0x00188C5C
	public Transform maskPoint
	{
		get
		{
			return this.characterInterface.maskPoint;
		}
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x0600471E RID: 18206 RVA: 0x0018AA6C File Offset: 0x00188C6C
	public GameObject body
	{
		get
		{
			return this.characterInterface.gunPoint.gameObject;
		}
	}

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x0600471F RID: 18207 RVA: 0x0018AA80 File Offset: 0x00188C80
	public Animation animation
	{
		get
		{
			return this.characterInterface.animation;
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x06004720 RID: 18208 RVA: 0x0018AA90 File Offset: 0x00188C90
	public Transform MainMenu_Pers
	{
		get
		{
			return this.characterInterface.transform;
		}
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x0018AAA0 File Offset: 0x00188CA0
	private void ReparentButtons()
	{
		if (this.CurrentItem == null)
		{
			return;
		}
		if (ShopNGUIController.IsWeaponCategory(this.CurrentItem.Category))
		{
			string text = WeaponManager.LastBoughtTag(this.CurrentItem.Id, null);
			int index = ((text == null || !(WeaponManager.FirstUnboughtOrForOurTier(this.CurrentItem.Id) != text)) && (!(WeaponManager.sharedManager != null) || !WeaponManager.sharedManager.IsAvailableTryGun(this.CurrentItem.Id))) ? 2 : 1;
			Transform parent = this.buttonContainers[index];
			this.equipped.transform.parent = parent;
			this.equipButton.transform.parent = parent;
		}
		else if (ShopNGUIController.IsWearCategory(this.CurrentItem.Category))
		{
			if (this.CurrentItem.Id == "cape_Custom")
			{
				this.equipped.transform.parent = this.buttonContainers[1];
				this.equipButton.transform.parent = this.buttonContainers[0];
				this.unequipButton.transform.parent = this.buttonContainers[0];
			}
			else
			{
				string text2 = WeaponManager.LastBoughtTag(this.CurrentItem.Id, null);
				bool flag = text2 != null && WeaponManager.FirstUnboughtTag(this.CurrentItem.Id) != text2;
				this.equipped.transform.parent = this.buttonContainers[(!flag) ? 0 : 1];
				this.unequipButton.transform.parent = this.buttonContainers[(!flag) ? 2 : 0];
				this.equipButton.transform.parent = this.buttonContainers[(!flag) ? 2 : 0];
			}
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			if (this.CurrentItem.Id == "CustomSkinID" || SkinsController.CustomSkinIds().Contains(this.CurrentItem.Id))
			{
				this.equipped.transform.parent = this.buttonContainers[1];
				this.equipButton.transform.parent = this.buttonContainers[1];
			}
			else
			{
				this.equipped.transform.parent = this.buttonContainers[2];
				this.equipButton.transform.parent = this.buttonContainers[2];
			}
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			this.equipped.transform.parent = this.buttonContainers[0];
			this.equipButton.transform.parent = this.buttonContainers[2];
			this.unequipButton.transform.parent = this.buttonContainers[2];
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			this.equipped.transform.parent = this.buttonContainers[0];
			this.equipButton.transform.parent = this.buttonContainers[2];
			this.unequipButton.transform.parent = this.buttonContainers[2];
		}
		else if (this.CurrentItem.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			bool flag2 = Singleton<PetsManager>.Instance.PlayerPets.Count > 0 && Singleton<PetsManager>.Instance.GetNextUp(this.CurrentItem.Id) != null;
			this.equipped.transform.parent = this.buttonContainers[1];
			this.equipButton.transform.parent = this.buttonContainers[(!flag2) ? 2 : 1];
			this.unequipButton.transform.parent = this.buttonContainers[(!flag2) ? 2 : 1];
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.CurrentItem.Category))
		{
			string text3 = GadgetsInfo.LastBoughtFor(this.CurrentItem.Id);
			int index2 = (text3 == null || !(text3 != GadgetsInfo.Upgrades[text3].Last<string>())) ? 2 : 1;
			Transform parent2 = this.buttonContainers[index2];
			this.equipped.transform.parent = parent2;
			this.equipButton.transform.parent = parent2;
		}
		this.equipped.transform.localPosition = Vector3.zero;
		this.equipButton.transform.localPosition = Vector3.zero;
		this.unequipButton.transform.localPosition = Vector3.zero;
	}

	// Token: 0x04003360 RID: 13152
	public const string BoughtCurrencsySettingBase = "BoughtCurrency";

	// Token: 0x04003361 RID: 13153
	public const string KEY_TUTORIAL_STATE_PASSED = "shop_tutorial_state_passed_VER_12_1";

	// Token: 0x04003362 RID: 13154
	private const string KEY_TUTORIAL_STATE_VIEWED = "shop_tutorial_state_viewed";

	// Token: 0x04003363 RID: 13155
	public const string KEY_BTN_TRY_HIGHLIGHTED = "tutorial_button_try_highlighted";

	// Token: 0x04003364 RID: 13156
	private const string KEY_TUTORIAL_INFO_HINT_VIEWED = "tutorial_info_hint_viewed";

	// Token: 0x04003365 RID: 13157
	private const string KEY_TUTORIAL_EGG_INFO_HINT_VIEWED = "Shop.Tutorial.KEY_TUTORIAL_EGG_INFO_HINT_VIEWED";

	// Token: 0x04003366 RID: 13158
	private const string KEY_TUTORIAL_PET_UPGRADE_HINT_VIEWED = "Shop.Tutorial.KEY_TUTORIAL_PET_UPRADE_HINT_VIEWED";

	// Token: 0x04003367 RID: 13159
	private const string ShowArmorKey = "ShowArmorKeySetting";

	// Token: 0x04003368 RID: 13160
	private const string ShowHatKey = "ShowHatKeySetting";

	// Token: 0x04003369 RID: 13161
	private const string ShowWearKey = "ShowWearKeySetting";

	// Token: 0x0400336A RID: 13162
	public const string CustomSkinID = "CustomSkinID";

	// Token: 0x0400336B RID: 13163
	public const string TrainingShopStageStepKey = "ShopNGUIController.TrainingShopStageStepKey";

	// Token: 0x0400336C RID: 13164
	private const float differenceOfWidthOfBackgroundAndGrid = 12f;

	// Token: 0x0400336D RID: 13165
	[Header("Shop 2016 - Armor Carousel")]
	public GameObject armorCarousel;

	// Token: 0x0400336E RID: 13166
	public UIPanel scrollViewPanel;

	// Token: 0x0400336F RID: 13167
	public UIGrid wrapContent;

	// Token: 0x04003370 RID: 13168
	public MyCenterOnChild carouselCenter;

	// Token: 0x04003371 RID: 13169
	[NonSerialized]
	public int itemIndexInCarousel;

	// Token: 0x04003372 RID: 13170
	private GameObject _lastSelectedItem;

	// Token: 0x04003373 RID: 13171
	private static List<ShopNGUIController.ShopItem> m_bestItemsToRemoveOnLeave = new List<ShopNGUIController.ShopItem>();

	// Token: 0x04003374 RID: 13172
	private static readonly Dictionary<ShopNGUIController.CategoryNames, List<ShopNGUIController.CategoryNames>> m_categoriesOfBestCategories = new Dictionary<ShopNGUIController.CategoryNames, List<ShopNGUIController.CategoryNames>>(3, ShopNGUIController.CategoryNameComparer.Instance)
	{
		{
			ShopNGUIController.CategoryNames.BestWeapons,
			new List<ShopNGUIController.CategoryNames>
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				ShopNGUIController.CategoryNames.BackupCategory,
				ShopNGUIController.CategoryNames.SpecilCategory,
				ShopNGUIController.CategoryNames.SniperCategory,
				ShopNGUIController.CategoryNames.PremiumCategory,
				ShopNGUIController.CategoryNames.MeleeCategory
			}
		},
		{
			ShopNGUIController.CategoryNames.BestWear,
			new List<ShopNGUIController.CategoryNames>
			{
				ShopNGUIController.CategoryNames.HatsCategory,
				ShopNGUIController.CategoryNames.MaskCategory,
				ShopNGUIController.CategoryNames.CapesCategory,
				ShopNGUIController.CategoryNames.BootsCategory,
				ShopNGUIController.CategoryNames.SkinsCategory
			}
		},
		{
			ShopNGUIController.CategoryNames.BestGadgets,
			new List<ShopNGUIController.CategoryNames>
			{
				ShopNGUIController.CategoryNames.ThrowingCategory,
				ShopNGUIController.CategoryNames.ToolsCategoty,
				ShopNGUIController.CategoryNames.SupportCategory
			}
		}
	};

	// Token: 0x04003375 RID: 13173
	[Header("Training After Removing Novice Armor")]
	public GameObject trainingRemoveNoviceArmorCollider;

	// Token: 0x04003376 RID: 13174
	public List<GameObject> trainingTipsRemovedNoviceArmor = new List<GameObject>();

	// Token: 0x04003377 RID: 13175
	[Header("DEPRECATED Training")]
	public GameObject trainingColliders;

	// Token: 0x04003378 RID: 13176
	public List<GameObject> trainingTips = new List<GameObject>();

	// Token: 0x04003379 RID: 13177
	[SerializeField]
	private GameObject _tutorialHintsContainer;

	// Token: 0x0400337A RID: 13178
	[SerializeField]
	private GameObject _tutorialHintsExtContainer;

	// Token: 0x0400337B RID: 13179
	private List<ShopNGUIController.TutorialState> _tutorialStates = new List<ShopNGUIController.TutorialState>();

	// Token: 0x0400337C RID: 13180
	private ShopNGUIController.TutorialState _tutorialCurrentState;

	// Token: 0x0400337D RID: 13181
	private List<CancellationTokenSource> _tutorialTokensSources = new List<CancellationTokenSource>();

	// Token: 0x0400337E RID: 13182
	private List<CancellationTokenSource> _tutorialPetUpgradeTokensSources = new List<CancellationTokenSource>();

	// Token: 0x0400337F RID: 13183
	private List<CancellationTokenSource> _tutorialEggsInfoTokensSources = new List<CancellationTokenSource>();

	// Token: 0x04003380 RID: 13184
	private EventDelegate m_tutorialInfoBtnED;

	// Token: 0x04003381 RID: 13185
	private EventDelegate m_tutorialInfoBtnED_Eggs;

	// Token: 0x04003382 RID: 13186
	private bool m_petUpgradeHideCoroutineStarted;

	// Token: 0x04003383 RID: 13187
	private bool m_startedLeaveArmoryStateTimer;

	// Token: 0x04003384 RID: 13188
	private bool m_shouldMoveToLeaveState;

	// Token: 0x04003385 RID: 13189
	private static bool _showArmorValue = true;

	// Token: 0x04003386 RID: 13190
	private static bool _showHatValue = true;

	// Token: 0x04003387 RID: 13191
	private static bool? _showWearValue;

	// Token: 0x04003388 RID: 13192
	public static ShopNGUIController sharedShop;

	// Token: 0x04003389 RID: 13193
	[Header("Shop 2016")]
	[Header("Shop 2016 - Tutorial")]
	public GameObject noOffersAtThisTime;

	// Token: 0x0400338A RID: 13194
	public List<UILabel> weaponSupercategoryUnlockedItems;

	// Token: 0x0400338B RID: 13195
	public List<UILabel> gadgetsSupercategoryUnlockedItems;

	// Token: 0x0400338C RID: 13196
	public List<UILabel> primaryWeaponsUnlockedItems;

	// Token: 0x0400338D RID: 13197
	public List<UILabel> backupWeaponsUnlockedItems;

	// Token: 0x0400338E RID: 13198
	public List<UILabel> meleeWeaponsUnlockedItems;

	// Token: 0x0400338F RID: 13199
	public List<UILabel> specialWeaponsUnlockedItems;

	// Token: 0x04003390 RID: 13200
	public List<UILabel> sniperWeaponsUnlockedItems;

	// Token: 0x04003391 RID: 13201
	public List<UILabel> premiumWeaponsUnlockedItems;

	// Token: 0x04003392 RID: 13202
	public List<UILabel> throwingGadgetsUnlockedItems;

	// Token: 0x04003393 RID: 13203
	public List<UILabel> toolsGadgetsUnlockedItems;

	// Token: 0x04003394 RID: 13204
	public List<UILabel> supportGadgetsUnlockedItems;

	// Token: 0x04003395 RID: 13205
	public UISprite sideFrameNearCategoryButtons;

	// Token: 0x04003396 RID: 13206
	public List<UILabel> petUpgradePointsLabels;

	// Token: 0x04003397 RID: 13207
	public GameObject petUpgradesInSpecial;

	// Token: 0x04003398 RID: 13208
	public UISprite petUpgradeIndicator;

	// Token: 0x04003399 RID: 13209
	public GameObject panelProperties;

	// Token: 0x0400339A RID: 13210
	public GameObject returnEveryDay;

	// Token: 0x0400339B RID: 13211
	public GameObject superIncubatorButton;

	// Token: 0x0400339C RID: 13212
	public GameObject noEggs;

	// Token: 0x0400339D RID: 13213
	public GameObject noPets;

	// Token: 0x0400339E RID: 13214
	public GameObject gridSlider;

	// Token: 0x0400339F RID: 13215
	public GameObject eggScreen;

	// Token: 0x040033A0 RID: 13216
	public List<UILabel> eggTimeLabels;

	// Token: 0x040033A1 RID: 13217
	public UIPanel armoryRootPanel;

	// Token: 0x040033A2 RID: 13218
	public List<Transform> buttonContainers;

	// Token: 0x040033A3 RID: 13219
	public PropertiesHideSHow hideButton;

	// Token: 0x040033A4 RID: 13220
	public Transform bottomPointForShader;

	// Token: 0x040033A5 RID: 13221
	public Transform topPointForShader;

	// Token: 0x040033A6 RID: 13222
	public GameObject propertiesAndButtonsPanel;

	// Token: 0x040033A7 RID: 13223
	public GameObject equipped;

	// Token: 0x040033A8 RID: 13224
	public PropertiesArmoryItemContainer propertiesContainer;

	// Token: 0x040033A9 RID: 13225
	public UIToggle showArmorButton;

	// Token: 0x040033AA RID: 13226
	public UIToggle showWearButton;

	// Token: 0x040033AB RID: 13227
	public List<UIRect> widgetsToUpdateAnchorsOnStart;

	// Token: 0x040033AC RID: 13228
	public UILabel WeaponsRarityLabel;

	// Token: 0x040033AD RID: 13229
	public List<UILabel> wearNameLabels;

	// Token: 0x040033AE RID: 13230
	public List<UILabel> skinNameLabels;

	// Token: 0x040033AF RID: 13231
	public List<UILabel> armorNameLabels;

	// Token: 0x040033B0 RID: 13232
	public GameObject stub;

	// Token: 0x040033B1 RID: 13233
	public UILabel fireRate;

	// Token: 0x040033B2 RID: 13234
	public UILabel fireRateMElee;

	// Token: 0x040033B3 RID: 13235
	public UILabel mobility;

	// Token: 0x040033B4 RID: 13236
	public UILabel mobilityMelee;

	// Token: 0x040033B5 RID: 13237
	public UILabel capacity;

	// Token: 0x040033B6 RID: 13238
	public UILabel damage;

	// Token: 0x040033B7 RID: 13239
	public UILabel damageMelee;

	// Token: 0x040033B8 RID: 13240
	public UIWidget sccrollViewBackground;

	// Token: 0x040033B9 RID: 13241
	public UIGrid itemsGrid;

	// Token: 0x040033BA RID: 13242
	public UIScrollView gridScrollView;

	// Token: 0x040033BB RID: 13243
	public CategoryButtonsController superCategoriesButtonController;

	// Token: 0x040033BC RID: 13244
	public UIGrid weaponCategoriesGrid;

	// Token: 0x040033BD RID: 13245
	public UIGrid wearCategoriesGrid;

	// Token: 0x040033BE RID: 13246
	public UIGrid gridCategoriesLeague;

	// Token: 0x040033BF RID: 13247
	public UIGrid petsGrid;

	// Token: 0x040033C0 RID: 13248
	public UIGrid gadgetsGrid;

	// Token: 0x040033C1 RID: 13249
	public UIGrid bestCategoriesGrid;

	// Token: 0x040033C2 RID: 13250
	public UIButton facebookLoginLockedSkinButton;

	// Token: 0x040033C3 RID: 13251
	public UIButton upgradeButton;

	// Token: 0x040033C4 RID: 13252
	public UIButton buyButton;

	// Token: 0x040033C5 RID: 13253
	public UIButton equipButton;

	// Token: 0x040033C6 RID: 13254
	public UIButton unequipButton;

	// Token: 0x040033C7 RID: 13255
	public UIButton infoButton;

	// Token: 0x040033C8 RID: 13256
	public UIButton editButton;

	// Token: 0x040033C9 RID: 13257
	public UIButton deleteButton;

	// Token: 0x040033CA RID: 13258
	public UIButton enableButton;

	// Token: 0x040033CB RID: 13259
	public UIButton unlockButton;

	// Token: 0x040033CC RID: 13260
	public UIButton createButton;

	// Token: 0x040033CD RID: 13261
	public GameObject weaponProperties;

	// Token: 0x040033CE RID: 13262
	public GameObject meleeProperties;

	// Token: 0x040033CF RID: 13263
	public GameObject SpecialParams;

	// Token: 0x040033D0 RID: 13264
	public List<UISprite> effectsSprites;

	// Token: 0x040033D1 RID: 13265
	public List<UILabel> effectsLabels;

	// Token: 0x040033D2 RID: 13266
	public GameObject nonArmorWearProperties;

	// Token: 0x040033D3 RID: 13267
	public GameObject armorWearProperties;

	// Token: 0x040033D4 RID: 13268
	public GameObject skinProperties;

	// Token: 0x040033D5 RID: 13269
	public UILabel nonArmorWearDEscription;

	// Token: 0x040033D6 RID: 13270
	public UILabel armorWearDescription;

	// Token: 0x040033D7 RID: 13271
	public UILabel armorCountLabel;

	// Token: 0x040033D8 RID: 13272
	public GameObject armorLock;

	// Token: 0x040033D9 RID: 13273
	public Transform rentScreenPoint;

	// Token: 0x040033DA RID: 13274
	public GameObject purchaseSuccessful;

	// Token: 0x040033DB RID: 13275
	public List<Light> mylights;

	// Token: 0x040033DC RID: 13276
	public Transform characterPoint;

	// Token: 0x040033DD RID: 13277
	public GameObject mainPanel;

	// Token: 0x040033DE RID: 13278
	public UIButton backButton;

	// Token: 0x040033DF RID: 13279
	public UIButton coinShopButton;

	// Token: 0x040033E0 RID: 13280
	public Action resumeAction;

	// Token: 0x040033E1 RID: 13281
	public Action wearResumeAction;

	// Token: 0x040033E2 RID: 13282
	public Action<ShopNGUIController.CategoryNames, string> wearUnequipAction;

	// Token: 0x040033E3 RID: 13283
	public Action<ShopNGUIController.CategoryNames, string, string> wearEquipAction;

	// Token: 0x040033E4 RID: 13284
	public Action<string> buyAction;

	// Token: 0x040033E5 RID: 13285
	public Action<string> equipAction;

	// Token: 0x040033E6 RID: 13286
	public Action<string> activatePotionAction;

	// Token: 0x040033E7 RID: 13287
	public Action<string> onEquipSkinAction;

	// Token: 0x040033E8 RID: 13288
	private GameObject weapon;

	// Token: 0x040033E9 RID: 13289
	private AnimationClip profile;

	// Token: 0x040033EA RID: 13290
	public UIButton tryGun;

	// Token: 0x040033EB RID: 13291
	public UILabel caption;

	// Token: 0x040033EC RID: 13292
	public GameObject gadgetBlocker;

	// Token: 0x040033ED RID: 13293
	public AudioClip upgradeBtnSound;

	// Token: 0x040033EE RID: 13294
	public AudioClip upgradeBtnPetSound;

	// Token: 0x040033EF RID: 13295
	public bool inGame = true;

	// Token: 0x040033F0 RID: 13296
	public List<Camera> ourCameras;

	// Token: 0x040033F1 RID: 13297
	public AnimationCoroutineRunner animationCoroutineRunner;

	// Token: 0x040033F2 RID: 13298
	public AnimationCoroutineRunner petProfileAnimationRunner;

	// Token: 0x040033F3 RID: 13299
	public GameObject ActiveObject;

	// Token: 0x040033F4 RID: 13300
	public bool EnableConfigurePos;

	// Token: 0x040033F5 RID: 13301
	public GameObject tryGunPanel;

	// Token: 0x040033F6 RID: 13302
	public UILabel tryGunMatchesCount;

	// Token: 0x040033F7 RID: 13303
	public UILabel tryGunDiscountTime;

	// Token: 0x040033F8 RID: 13304
	public static readonly Dictionary<string, string> weaponCategoryLocKeys = new Dictionary<string, string>(6)
	{
		{
			ShopNGUIController.CategoryNames.PrimaryCategory.ToString(),
			"Key_0352"
		},
		{
			ShopNGUIController.CategoryNames.BackupCategory.ToString(),
			"Key_0442"
		},
		{
			ShopNGUIController.CategoryNames.MeleeCategory.ToString(),
			"Key_0441"
		},
		{
			ShopNGUIController.CategoryNames.SpecilCategory.ToString(),
			"Key_0440"
		},
		{
			ShopNGUIController.CategoryNames.SniperCategory.ToString(),
			"Key_1669"
		},
		{
			ShopNGUIController.CategoryNames.PremiumCategory.ToString(),
			"Key_0093"
		}
	};

	// Token: 0x040033F9 RID: 13305
	private GameObject gadgetPreview;

	// Token: 0x040033FA RID: 13306
	private GameObject[] _onPersArmorRefs;

	// Token: 0x040033FB RID: 13307
	private float m_timeOfPLastStuffUpdate;

	// Token: 0x040033FC RID: 13308
	private bool _shouldShowRewardWindowSkin;

	// Token: 0x040033FD RID: 13309
	private bool _shouldShowRewardWindowCape;

	// Token: 0x040033FE RID: 13310
	private Dictionary<ShopNGUIController.Supercategory, List<UILabel>> m_supercategoriesToUnlockedItemsLabels;

	// Token: 0x040033FF RID: 13311
	private Dictionary<ShopNGUIController.CategoryNames, List<UILabel>> m_categoriesToUnlockedItemsLabels;

	// Token: 0x04003400 RID: 13312
	private GameObject _refOnLowPolyArmor;

	// Token: 0x04003401 RID: 13313
	private Material[] _refsOnLowPolyArmorMaterials;

	// Token: 0x04003402 RID: 13314
	public static string[] gearOrder = PotionsController.potions;

	// Token: 0x04003403 RID: 13315
	private float lastTime;

	// Token: 0x04003404 RID: 13316
	public static float IdleTimeoutPers = 5f;

	// Token: 0x04003405 RID: 13317
	private float idleTimerLastTime;

	// Token: 0x04003406 RID: 13318
	private float _timePurchaseSuccessfulShown;

	// Token: 0x04003407 RID: 13319
	private float _timePurchaseRentSuccessfulShown;

	// Token: 0x04003408 RID: 13320
	private bool categoryGridsRepositioned;

	// Token: 0x04003409 RID: 13321
	private IDisposable _backSubscription;

	// Token: 0x0400340A RID: 13322
	private bool _escapeRequested;

	// Token: 0x0400340B RID: 13323
	private float timeOfEnteringShopForProtectFromPressingCoinsButton;

	// Token: 0x0400340C RID: 13324
	private Color? _storedAmbientLight;

	// Token: 0x0400340D RID: 13325
	private bool? _storedFogEnabled;

	// Token: 0x0400340E RID: 13326
	private static List<Camera> disablesCameras = new List<Camera>();

	// Token: 0x0400340F RID: 13327
	private bool _isFromPromoActions;

	// Token: 0x04003410 RID: 13328
	private string _promoActionsIdClicked;

	// Token: 0x04003411 RID: 13329
	private string _assignedWeaponTag = string.Empty;

	// Token: 0x04003412 RID: 13330
	private bool InTrainingAfterNoviceArmorRemoved;

	// Token: 0x04003413 RID: 13331
	private Rect _touchZoneForRotatingCharacter;

	// Token: 0x04003414 RID: 13332
	private float _rotationRateForCharacter;

	// Token: 0x04003415 RID: 13333
	private static Comparison<string> skinIdsComparisonForShop = delegate(string kvp1, string kvp2)
	{
		int result;
		try
		{
			bool flag = SkinsController.standardSkinsIds.Contains(kvp1);
			bool flag2 = SkinsController.standardSkinsIds.Contains(kvp2);
			if (flag == flag2)
			{
				if (flag)
				{
					result = SkinsController.standardSkinsIds.IndexOf(kvp1).CompareTo(SkinsController.standardSkinsIds.IndexOf(kvp2));
				}
				else
				{
					result = long.Parse(kvp1).CompareTo(long.Parse(kvp2));
				}
			}
			else
			{
				result = ((!flag) ? 1 : -1);
			}
		}
		catch
		{
			result = 0;
		}
		return result;
	};

	// Token: 0x04003416 RID: 13334
	private readonly Dictionary<ShopNGUIController.Supercategory, ShopNGUIController.CategoryNames> supercategoryLastUsedCategory = new Dictionary<ShopNGUIController.Supercategory, ShopNGUIController.CategoryNames>(6, new ShopNGUIController.SupercategoryComparer())
	{
		{
			ShopNGUIController.Supercategory.Best,
			ShopNGUIController.CategoryNames.BestWeapons
		},
		{
			ShopNGUIController.Supercategory.Weapons,
			ShopNGUIController.CategoryNames.PrimaryCategory
		},
		{
			ShopNGUIController.Supercategory.Wear,
			ShopNGUIController.CategoryNames.HatsCategory
		},
		{
			ShopNGUIController.Supercategory.League,
			ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory
		},
		{
			ShopNGUIController.Supercategory.Gadgets,
			ShopNGUIController.CategoryNames.ThrowingCategory
		},
		{
			ShopNGUIController.Supercategory.Pets,
			ShopNGUIController.CategoryNames.EggsCategory
		}
	};

	// Token: 0x04003417 RID: 13335
	private ShopNGUIController.ShopItem m_itemToSetAfterEnter;

	// Token: 0x04003418 RID: 13336
	public ShopNGUIController.CategoryNames CategoryToChoose;

	// Token: 0x04003419 RID: 13337
	[Range(-200f, 200f)]
	public float firstOFfset = -50f;

	// Token: 0x0400341A RID: 13338
	[Range(-200f, 200f)]
	public float secondOffset = 50f;

	// Token: 0x0400341B RID: 13339
	private CharacterInterface characterInterface;

	// Token: 0x0400341C RID: 13340
	public float scaleCoefficent = 0.5f;

	// Token: 0x0400341D RID: 13341
	private static List<ShopPositionParams> hats = new List<ShopPositionParams>();

	// Token: 0x0400341E RID: 13342
	private static List<ShopPositionParams> capes = new List<ShopPositionParams>();

	// Token: 0x0400341F RID: 13343
	private static List<ShopPositionParams> boots = new List<ShopPositionParams>();

	// Token: 0x04003420 RID: 13344
	private static List<ShopPositionParams> masks = new List<ShopPositionParams>();

	// Token: 0x04003421 RID: 13345
	private static List<ShopPositionParams> armor = new List<ShopPositionParams>();

	// Token: 0x04003422 RID: 13346
	private Action<List<ShopPositionParams>, ShopNGUIController.CategoryNames> sort = delegate(List<ShopPositionParams> prefabs, ShopNGUIController.CategoryNames c)
	{
		Comparison<ShopPositionParams> comparison = delegate(ShopPositionParams go1, ShopPositionParams go2)
		{
			List<string> list = null;
			List<string> list2 = null;
			foreach (List<string> list3 in Wear.wear[c])
			{
				if (list3.Contains(go1.name))
				{
					list = list3;
				}
				if (list3.Contains(go2.name))
				{
					list2 = list3;
				}
			}
			if (list == null || list2 == null)
			{
				return 0;
			}
			if (list == list2)
			{
				return list.IndexOf(go1.name) - list.IndexOf(go2.name);
			}
			return Wear.wear[c].IndexOf(list) - Wear.wear[c].IndexOf(list2);
		};
		prefabs.Sort(comparison);
	};

	// Token: 0x04003423 RID: 13347
	private readonly Dictionary<ShopNGUIController.CategoryNames, bool> propertiesShownInCategory = new Dictionary<ShopNGUIController.CategoryNames, bool>(20, ShopNGUIController.CategoryNameComparer.Instance)
	{
		{
			ShopNGUIController.CategoryNames.PrimaryCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.BackupCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.MeleeCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.SpecilCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.SniperCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.PremiumCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.ArmorCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.BootsCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.HatsCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.CapesCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.LeagueHatsCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.MaskCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.SkinsCategory,
			false
		},
		{
			ShopNGUIController.CategoryNames.SkinsCategoryMale,
			false
		},
		{
			ShopNGUIController.CategoryNames.SkinsCategoryFemale,
			false
		},
		{
			ShopNGUIController.CategoryNames.SkinsCategoryPremium,
			false
		},
		{
			ShopNGUIController.CategoryNames.SkinsCategoryEditor,
			false
		},
		{
			ShopNGUIController.CategoryNames.LeagueSkinsCategory,
			false
		},
		{
			ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory,
			false
		},
		{
			ShopNGUIController.CategoryNames.ThrowingCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.ToolsCategoty,
			true
		},
		{
			ShopNGUIController.CategoryNames.SupportCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.EggsCategory,
			false
		},
		{
			ShopNGUIController.CategoryNames.PetsCategory,
			true
		},
		{
			ShopNGUIController.CategoryNames.GearCategory,
			false
		},
		{
			ShopNGUIController.CategoryNames.BestWeapons,
			true
		},
		{
			ShopNGUIController.CategoryNames.BestWear,
			true
		},
		{
			ShopNGUIController.CategoryNames.BestGadgets,
			true
		}
	};

	// Token: 0x04003424 RID: 13348
	private GameObject pixlMan;

	// Token: 0x04003425 RID: 13349
	private Transform infoScreen;

	// Token: 0x04003426 RID: 13350
	private int needRefreshInLateUpdate;

	// Token: 0x04003427 RID: 13351
	private Texture _skinsMakerSkinCache;

	// Token: 0x04003428 RID: 13352
	private bool updateScrollViewOnLateUpdateForTryPanel;

	// Token: 0x04003429 RID: 13353
	private static bool gridScrollViewPanelUpdatedOnFirstLaunch = false;

	// Token: 0x0400342A RID: 13354
	private static bool _gridInitiallyRepositioned = false;

	// Token: 0x0400342B RID: 13355
	private int itemScrollBottomAnchor;

	// Token: 0x0400342C RID: 13356
	private int itemScrollBottomAnchorRent;

	// Token: 0x0400342D RID: 13357
	private string _gunThatWeUsedInPolygon;

	// Token: 0x0400342E RID: 13358
	private bool m_firstReportItemsViewedSkipped;

	// Token: 0x0400342F RID: 13359
	private float m_timeOfLAstReportVisibleCells = float.MinValue;

	// Token: 0x04003430 RID: 13360
	private ShopNGUIController.ShopItem m_currentItem;

	// Token: 0x020007AD RID: 1965
	public enum TutorialPhase
	{
		// Token: 0x0400348F RID: 13455
		SelectWeaponCategory,
		// Token: 0x04003490 RID: 13456
		SelectSniperSection,
		// Token: 0x04003491 RID: 13457
		SelectRifle,
		// Token: 0x04003492 RID: 13458
		EquipRifle,
		// Token: 0x04003493 RID: 13459
		SelectWearCategory,
		// Token: 0x04003494 RID: 13460
		EquipArmor,
		// Token: 0x04003495 RID: 13461
		SelectArmorSection,
		// Token: 0x04003496 RID: 13462
		SelectPetsCategory,
		// Token: 0x04003497 RID: 13463
		ShowEggsHint,
		// Token: 0x04003498 RID: 13464
		LeaveArmory
	}

	// Token: 0x020007AE RID: 1966
	public enum TutorialStageTrigger
	{
		// Token: 0x0400349A RID: 13466
		Enter,
		// Token: 0x0400349B RID: 13467
		Exit,
		// Token: 0x0400349C RID: 13468
		Update
	}

	// Token: 0x020007AF RID: 1967
	private class TutorialState
	{
		// Token: 0x06004780 RID: 18304 RVA: 0x0018BD08 File Offset: 0x00189F08
		public TutorialState(ShopNGUIController.TutorialPhase forStage, Action<ShopNGUIController.TutorialStageTrigger> act)
		{
			this.ForStage = forStage;
			this.StageAct = act;
		}

		// Token: 0x0400349D RID: 13469
		public ShopNGUIController.TutorialPhase ForStage;

		// Token: 0x0400349E RID: 13470
		public Action<ShopNGUIController.TutorialStageTrigger> StageAct;
	}

	// Token: 0x020007B0 RID: 1968
	public enum CategoryNames
	{
		// Token: 0x040034A0 RID: 13472
		PrimaryCategory,
		// Token: 0x040034A1 RID: 13473
		BackupCategory,
		// Token: 0x040034A2 RID: 13474
		MeleeCategory,
		// Token: 0x040034A3 RID: 13475
		SpecilCategory,
		// Token: 0x040034A4 RID: 13476
		SniperCategory,
		// Token: 0x040034A5 RID: 13477
		PremiumCategory,
		// Token: 0x040034A6 RID: 13478
		HatsCategory,
		// Token: 0x040034A7 RID: 13479
		ArmorCategory,
		// Token: 0x040034A8 RID: 13480
		SkinsCategory,
		// Token: 0x040034A9 RID: 13481
		CapesCategory,
		// Token: 0x040034AA RID: 13482
		BootsCategory,
		// Token: 0x040034AB RID: 13483
		GearCategory,
		// Token: 0x040034AC RID: 13484
		MaskCategory,
		// Token: 0x040034AD RID: 13485
		SkinsCategoryEditor = 1000,
		// Token: 0x040034AE RID: 13486
		SkinsCategoryMale = 1100,
		// Token: 0x040034AF RID: 13487
		SkinsCategoryFemale = 1200,
		// Token: 0x040034B0 RID: 13488
		SkinsCategorySpecial = 1300,
		// Token: 0x040034B1 RID: 13489
		SkinsCategoryPremium = 1400,
		// Token: 0x040034B2 RID: 13490
		LeagueWeaponSkinsCategory = 2000,
		// Token: 0x040034B3 RID: 13491
		LeagueHatsCategory = 2100,
		// Token: 0x040034B4 RID: 13492
		LeagueSkinsCategory = 2200,
		// Token: 0x040034B5 RID: 13493
		ThrowingCategory = 12500,
		// Token: 0x040034B6 RID: 13494
		ToolsCategoty = 13000,
		// Token: 0x040034B7 RID: 13495
		SupportCategory = 13500,
		// Token: 0x040034B8 RID: 13496
		PetsCategory = 25000,
		// Token: 0x040034B9 RID: 13497
		EggsCategory = 30000,
		// Token: 0x040034BA RID: 13498
		BestWeapons = 35000,
		// Token: 0x040034BB RID: 13499
		BestWear = 40000,
		// Token: 0x040034BC RID: 13500
		BestGadgets = 45000
	}

	// Token: 0x020007B1 RID: 1969
	internal sealed class CategoryNameComparer : IEqualityComparer<ShopNGUIController.CategoryNames>
	{
		// Token: 0x06004781 RID: 18305 RVA: 0x0018BD20 File Offset: 0x00189F20
		private CategoryNameComparer()
		{
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004783 RID: 18307 RVA: 0x0018BD34 File Offset: 0x00189F34
		public static ShopNGUIController.CategoryNameComparer Instance
		{
			get
			{
				return ShopNGUIController.CategoryNameComparer.s_instance;
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0018BD3C File Offset: 0x00189F3C
		public bool Equals(ShopNGUIController.CategoryNames x, ShopNGUIController.CategoryNames y)
		{
			return x == y;
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0018BD44 File Offset: 0x00189F44
		public int GetHashCode(ShopNGUIController.CategoryNames obj)
		{
			return (int)obj;
		}

		// Token: 0x040034BD RID: 13501
		private static readonly ShopNGUIController.CategoryNameComparer s_instance = new ShopNGUIController.CategoryNameComparer();
	}

	// Token: 0x020007B2 RID: 1970
	public enum Supercategory
	{
		// Token: 0x040034BF RID: 13503
		Best,
		// Token: 0x040034C0 RID: 13504
		Weapons,
		// Token: 0x040034C1 RID: 13505
		Wear,
		// Token: 0x040034C2 RID: 13506
		League,
		// Token: 0x040034C3 RID: 13507
		Gadgets,
		// Token: 0x040034C4 RID: 13508
		Pets
	}

	// Token: 0x020007B3 RID: 1971
	private sealed class SupercategoryComparer : IEqualityComparer<ShopNGUIController.Supercategory>
	{
		// Token: 0x06004787 RID: 18311 RVA: 0x0018BD50 File Offset: 0x00189F50
		public bool Equals(ShopNGUIController.Supercategory x, ShopNGUIController.Supercategory y)
		{
			return x == y;
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0018BD58 File Offset: 0x00189F58
		public int GetHashCode(ShopNGUIController.Supercategory obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x020007B4 RID: 1972
	public class ShopItem : IEquatable<ShopNGUIController.ShopItem>
	{
		// Token: 0x06004789 RID: 18313 RVA: 0x0018BD5C File Offset: 0x00189F5C
		public ShopItem(string id, ShopNGUIController.CategoryNames category)
		{
			this.Id = id;
			this.Category = category;
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600478A RID: 18314 RVA: 0x0018BD74 File Offset: 0x00189F74
		// (set) Token: 0x0600478B RID: 18315 RVA: 0x0018BD7C File Offset: 0x00189F7C
		public string Id { get; private set; }

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x0600478C RID: 18316 RVA: 0x0018BD88 File Offset: 0x00189F88
		// (set) Token: 0x0600478D RID: 18317 RVA: 0x0018BD90 File Offset: 0x00189F90
		public ShopNGUIController.CategoryNames Category { get; private set; }

		// Token: 0x0600478E RID: 18318 RVA: 0x0018BD9C File Offset: 0x00189F9C
		public bool Equals(ShopNGUIController.ShopItem other)
		{
			return !object.ReferenceEquals(other, null) && (object.ReferenceEquals(this, other) || (this.Id.Equals(other.Id) && this.Category.Equals(other.Category)));
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0018BDFC File Offset: 0x00189FFC
		public override bool Equals(object obj)
		{
			if (!(obj is ShopNGUIController.ShopItem))
			{
				return false;
			}
			ShopNGUIController.ShopItem other = (ShopNGUIController.ShopItem)obj;
			return this.Equals(other);
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0018BE24 File Offset: 0x0018A024
		public override int GetHashCode()
		{
			int num = (this.Id != null) ? this.Id.GetHashCode() : 0;
			int hashCode = this.Category.GetHashCode();
			return hashCode ^ num;
		}
	}

	// Token: 0x02000928 RID: 2344
	// (Invoke) Token: 0x06005140 RID: 20800
	public delegate void Action7<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
}
