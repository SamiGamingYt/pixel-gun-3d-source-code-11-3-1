using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class ArmoryInfoScreenController : MonoBehaviour
{
	// Token: 0x060000C3 RID: 195 RVA: 0x00007608 File Offset: 0x00005808
	public void Close()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.DestroyWindow();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00007638 File Offset: 0x00005838
	public void DestroyWindow()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00007654 File Offset: 0x00005854
	private void Awake()
	{
		this.DisposeInfoScreenBackSubscription();
		this._infoScreenBackSubscription = BackSystem.Instance.Register(new Action(this.Close), "Info screen in Armory");
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00007680 File Offset: 0x00005880
	private void Start()
	{
		ArmoryInfoScreenController.sharedController = this;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00007688 File Offset: 0x00005888
	private void OnDestroy()
	{
		ArmoryInfoScreenController.sharedController = null;
		this.DisposeInfoScreenBackSubscription();
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00007698 File Offset: 0x00005898
	private void DisposeInfoScreenBackSubscription()
	{
		if (this._infoScreenBackSubscription != null)
		{
			this._infoScreenBackSubscription.Dispose();
			this._infoScreenBackSubscription = null;
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x000076B8 File Offset: 0x000058B8
	public void OnSelectItem(ItemPreviewInArmoryInfoScreen selectedItem, ShopNGUIController.CategoryNames category)
	{
		foreach (ItemPreviewInArmoryInfoScreen itemPreviewInArmoryInfoScreen in this.listItems)
		{
			itemPreviewInArmoryInfoScreen.SetSelected(itemPreviewInArmoryInfoScreen.Equals(selectedItem), false);
		}
		this.itemNameLabel.SetText(selectedItem.headName);
		this.selectedItemController = selectedItem;
		string text = null;
		string viewedId = selectedItem.id;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			text = WeaponManager.LastBoughtTag(selectedItem.id, null);
			viewedId = (text ?? WeaponManager.FirstUnboughtOrForOurTier(selectedItem.id));
		}
		else if (ShopNGUIController.IsGadgetsCategory(category))
		{
			text = GadgetsInfo.LastBoughtFor(selectedItem.id);
			viewedId = (text ?? GadgetsInfo.FirstUnboughtOrForOurTier(selectedItem.id));
		}
		if (this.currentUpgradeWindows != null)
		{
			this.currentUpgradeWindows.SetUpgrade(selectedItem.numUpgrade, (text == null) ? -1 : this.actualUpgrades.IndexOf(text));
		}
		ShopNGUIController.sharedShop.GetStateButtons(viewedId, selectedItem.id, this.propertiesContainer, false);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x000077F4 File Offset: 0x000059F4
	public void SetItem(ShopNGUIController.ShopItem item)
	{
		List<string> list = new List<string>();
		int num = 0;
		if (ShopNGUIController.IsWeaponCategory(item.Category))
		{
			this.chainForTag = WeaponUpgrades.ChainForTag(item.Id);
			string item2 = WeaponManager.FirstTagForOurTier(item.Id, null);
			if (this.chainForTag != null)
			{
				int num2 = this.chainForTag.IndexOf(item2);
				this.actualUpgrades = this.chainForTag.GetRange(num2, this.chainForTag.Count - num2);
			}
			else
			{
				this.actualUpgrades = new List<string>
				{
					item.Id
				};
			}
			this.curCountUpgrades = this.actualUpgrades.Count;
			for (int i = 0; i < this.upgradesWindows.Length; i++)
			{
				this.upgradesWindows[i].gameObject.SetActive(i == this.curCountUpgrades - 1);
			}
			this.currentUpgradeWindows = this.upgradesWindows[this.curCountUpgrades - 1];
			string text = WeaponManager.FirstUnboughtOrForOurTier(item.Id);
			num = this.actualUpgrades.IndexOf(text);
			if (num > 0 && !text.Equals(item.Id))
			{
				num--;
			}
			for (int j = num; j < this.actualUpgrades.Count; j++)
			{
				list.Add(this.actualUpgrades[j]);
			}
		}
		else if (ShopNGUIController.IsGadgetsCategory(item.Category))
		{
			List<string> list2 = GadgetsInfo.Upgrades[item.Id];
			this.chainForTag = GadgetsInfo.Upgrades[item.Id];
			string item3 = GadgetsInfo.FirstForOurTier(item.Id);
			if (this.chainForTag != null)
			{
				int num3 = this.chainForTag.IndexOf(item3);
				this.actualUpgrades = this.chainForTag.GetRange(num3, this.chainForTag.Count - num3);
			}
			else
			{
				this.actualUpgrades = new List<string>
				{
					item.Id
				};
			}
			this.curCountUpgrades = this.actualUpgrades.Count;
			for (int k = 0; k < this.upgradesWindows.Length; k++)
			{
				this.upgradesWindows[k].gameObject.SetActive(k == this.curCountUpgrades - 1);
			}
			this.currentUpgradeWindows = this.upgradesWindows[this.curCountUpgrades - 1];
			string text2 = GadgetsInfo.FirstUnboughtOrForOurTier(item.Id);
			num = this.actualUpgrades.IndexOf(text2);
			if (num > 0 && !text2.Equals(item.Id))
			{
				num--;
			}
			for (int l = num; l < this.actualUpgrades.Count; l++)
			{
				list.Add(this.actualUpgrades[l]);
			}
		}
		this.LoadPreviewCarousel(item.Category, item.Id, list, num, (!(this.selectedItemController != null)) ? string.Empty : this.selectedItemController.id);
		if (this.selectedItemController != null)
		{
			this.selectedItemController.SetSelected(true, true);
			this.OnSelectItem(this.selectedItemController, item.Category);
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00007B3C File Offset: 0x00005D3C
	public void LoadPreviewCarousel(ShopNGUIController.CategoryNames category, string itemId, List<string> _availableTags, int startIndexUpgrade, string selectedItemID)
	{
		while (this.itemsGrid.transform.childCount > 0)
		{
			Transform child = this.itemsGrid.transform.GetChild(0);
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
		this.listItems.Clear();
		bool flag = ShopNGUIController.IsWeaponCategory(category);
		bool flag2 = ShopNGUIController.IsWearCategory(category);
		bool flag3 = ShopNGUIController.IsGadgetsCategory(category);
		for (int i = 0; i < _availableTags.Count; i++)
		{
			string currentId = _availableTags[i];
			WeaponSounds wsForPos = null;
			GameObject pref = null;
			string headName = string.Empty;
			if (flag)
			{
				GameObject prefabByTag = WeaponManager.sharedManager.GetPrefabByTag(currentId);
				wsForPos = prefabByTag.GetComponent<WeaponSounds>();
				pref = WeaponManager.InnerPrefabForWeaponSync(prefabByTag.nameNoClone());
				headName = ItemDb.GetItemName(itemId, category);
			}
			if (flag2)
			{
				pref = ItemDb.GetWearFromResources(currentId, category);
			}
			if (flag3)
			{
				pref = GadgetsInfo.GetArmoryInfoPrefabFromName(currentId);
				headName = ItemDb.GetItemName(itemId, category);
			}
			GameObject _preview = UnityEngine.Object.Instantiate<GameObject>(this.previewPrefab);
			_preview.SetActive(true);
			_preview.name = "item_" + i.ToString();
			_preview.transform.SetParent(this.itemsGrid.transform);
			float num = (!(currentId == selectedItemID)) ? ItemPreviewInArmoryInfoScreen.minScale : ItemPreviewInArmoryInfoScreen.maxScale;
			_preview.transform.localScale = new Vector3(num, num, num);
			if (currentId.Equals("cape_Custom"))
			{
				Tools.SetTextureRecursivelyFrom(_preview, SkinsController.capeUserTexture, new GameObject[0]);
			}
			ItemPreviewInArmoryInfoScreen itemController = _preview.GetComponent<ItemPreviewInArmoryInfoScreen>();
			itemController.id = currentId;
			itemController.category = category;
			itemController.headName = headName;
			itemController.id = currentId;
			itemController.numUpgrade = startIndexUpgrade + i;
			if (currentId.Equals(itemId))
			{
				this.selectedItemController = itemController;
			}
			itemController.OnSelect += this.OnSelectItem;
			this.listItems.Add(itemController);
			ShopNGUIController.AddModel(pref, delegate(GameObject manipulateObject, Vector3 positionShop, Vector3 rotationShop, string readableName, float scaleCoefShop, int tier, int league)
			{
				if (_preview == null)
				{
					UnityEngine.Object.Destroy(manipulateObject);
					return;
				}
				manipulateObject.transform.SetParent(_preview.transform);
				manipulateObject.transform.localScale = new Vector3(scaleCoefShop, scaleCoefShop, scaleCoefShop);
				if (currentId == "Eagle_3")
				{
					manipulateObject.transform.GetChild(0).localPosition = Vector3.zero;
					manipulateObject.transform.localPosition = new Vector3(0f, -10f, -1000f);
				}
				else if (currentId == "Fighter_1" || currentId == "Fighter_2")
				{
					manipulateObject.transform.GetChild(0).localPosition = Vector3.zero;
					manipulateObject.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
					manipulateObject.transform.localPosition = new Vector3(-21.5f, 12.5f, -1000f);
				}
				else
				{
					manipulateObject.transform.localPosition = new Vector3(positionShop.x, positionShop.y, -1000f);
				}
				manipulateObject.transform.localRotation = Quaternion.Euler(rotationShop);
				itemController.model = manipulateObject.transform;
				itemController.baseRotation = rotationShop;
			}, category, false, wsForPos);
		}
		this.itemsGrid.Reposition();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00007DE8 File Offset: 0x00005FE8
	public void UpgradeButtonOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		ShopNGUIController.sharedShop.HandleUpgradeButton();
		if (base.gameObject.activeInHierarchy)
		{
			this.SetItem(ShopNGUIController.sharedShop.CurrentItem);
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00007E3C File Offset: 0x0000603C
	public void BuyButtonButtonOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		ShopNGUIController.sharedShop.HandleBuyButton();
		if (base.gameObject.activeInHierarchy)
		{
			this.SetItem(ShopNGUIController.sharedShop.CurrentItem);
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00007E90 File Offset: 0x00006090
	public void EquipButtonOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		ShopNGUIController.sharedShop.HandleEquipButton();
		string viewedId = WeaponManager.LastBoughtTag(this.selectedItemController.id, null) ?? WeaponManager.FirstUnboughtOrForOurTier(this.selectedItemController.id);
		ShopNGUIController.sharedShop.GetStateButtons(viewedId, this.selectedItemController.id, this.propertiesContainer, false);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00007F08 File Offset: 0x00006108
	public void TryGunOnClick()
	{
	}

	// Token: 0x040000A7 RID: 167
	public static ArmoryInfoScreenController sharedController;

	// Token: 0x040000A8 RID: 168
	public PropertiesArmoryItemContainer propertiesContainer;

	// Token: 0x040000A9 RID: 169
	public SetHeadLabelText itemNameLabel;

	// Token: 0x040000AA RID: 170
	public UILabel rarityLabel;

	// Token: 0x040000AB RID: 171
	public UIScrollView itemsScroll;

	// Token: 0x040000AC RID: 172
	public UIGrid itemsGrid;

	// Token: 0x040000AD RID: 173
	public UpgradeWindow[] upgradesWindows;

	// Token: 0x040000AE RID: 174
	private UpgradeWindow currentUpgradeWindows;

	// Token: 0x040000AF RID: 175
	public GameObject labelFirstBuy;

	// Token: 0x040000B0 RID: 176
	public PriceContainer priceContainer;

	// Token: 0x040000B1 RID: 177
	public GameObject discountContainer;

	// Token: 0x040000B2 RID: 178
	public UILabel discountLabel;

	// Token: 0x040000B3 RID: 179
	public GameObject previewPrefab;

	// Token: 0x040000B4 RID: 180
	[SerializeField]
	private GameObject hintsContainer;

	// Token: 0x040000B5 RID: 181
	private List<ItemPreviewInArmoryInfoScreen> listItems = new List<ItemPreviewInArmoryInfoScreen>();

	// Token: 0x040000B6 RID: 182
	private ItemPreviewInArmoryInfoScreen selectedItemController;

	// Token: 0x040000B7 RID: 183
	private int curCountUpgrades;

	// Token: 0x040000B8 RID: 184
	private IDisposable _infoScreenBackSubscription;

	// Token: 0x040000B9 RID: 185
	private List<string> chainForTag;

	// Token: 0x040000BA RID: 186
	private List<string> actualUpgrades;
}
