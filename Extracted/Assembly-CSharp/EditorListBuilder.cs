using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020005E1 RID: 1505
public class EditorListBuilder : MonoBehaviour
{
	// Token: 0x0600338C RID: 13196 RVA: 0x0010AC80 File Offset: 0x00108E80
	private void Start()
	{
		string currentLanguage = LocalizationStore.CurrentLanguage;
		this._currentFilter = EditorShopItemsType.All;
		this._isStart = true;
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x0010ACA4 File Offset: 0x00108EA4
	private EditorShopItemData GetEditorItemDataByTag(string tag)
	{
		EditorShopItemData editorShopItemData = new EditorShopItemData();
		if (this._discountsData.ContainsKey(tag))
		{
			editorShopItemData.discount = this._discountsData[tag];
		}
		editorShopItemData.tag = tag;
		editorShopItemData.isTop = this._topSellersData.Contains(tag);
		editorShopItemData.isNew = this._newsData.Contains(tag);
		return editorShopItemData;
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x0010AD08 File Offset: 0x00108F08
	private List<EditorShopItemData> GetWeaponsData()
	{
		WeaponSounds[] array = Resources.LoadAll<WeaponSounds>("Weapons/");
		List<EditorShopItemData> list = new List<EditorShopItemData>();
		for (int i = 0; i < array.Length; i++)
		{
			EditorShopItemData editorItemDataByTag = this.GetEditorItemDataByTag(ItemDb.GetByPrefabName(array[i].name).Tag);
			editorItemDataByTag.localizeKey = array[i].localizeWeaponKey;
			editorItemDataByTag.type = EditorShopItemsType.Weapon;
			editorItemDataByTag.prefabName = array[i].name;
			list.Add(editorItemDataByTag);
		}
		return list;
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x0010AD80 File Offset: 0x00108F80
	private List<EditorShopItemData> GetWearData(EditorShopItemsType type)
	{
		string path = string.Empty;
		switch (type)
		{
		case EditorShopItemsType.Hats:
			path = "Hats_Info/";
			break;
		case EditorShopItemsType.Armor:
			path = "Armor_Info/";
			break;
		case EditorShopItemsType.Capes:
			path = "Capes_Info/";
			break;
		case EditorShopItemsType.Boots:
			path = "Shop_Boots_Info/";
			break;
		}
		ShopPositionParams[] array = Resources.LoadAll<ShopPositionParams>(path);
		List<EditorShopItemData> list = new List<EditorShopItemData>();
		for (int i = 0; i < array.Length; i++)
		{
			EditorShopItemData editorItemDataByTag = this.GetEditorItemDataByTag(array[i].name);
			editorItemDataByTag.localizeKey = array[i].localizeKey;
			editorItemDataByTag.type = type;
			editorItemDataByTag.prefabName = array[i].name;
			list.Add(editorItemDataByTag);
		}
		return list;
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x0010AE40 File Offset: 0x00109040
	private List<EditorShopItemData> GetSkinsData()
	{
		List<EditorShopItemData> list = new List<EditorShopItemData>();
		foreach (KeyValuePair<string, string> keyValuePair in SkinsController.shopKeyFromNameSkin)
		{
			EditorShopItemData editorItemDataByTag = this.GetEditorItemDataByTag(keyValuePair.Key);
			editorItemDataByTag.localizeKey = SkinsController.skinsLocalizeKey[keyValuePair.Key];
			editorItemDataByTag.type = EditorShopItemsType.Skins;
			list.Add(editorItemDataByTag);
		}
		return list;
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x0010AED8 File Offset: 0x001090D8
	public List<EditorShopItemData> GetItemsList(EditorShopItemsType filter)
	{
		switch (filter)
		{
		case EditorShopItemsType.Weapon:
			return this.GetWeaponsData();
		case EditorShopItemsType.Skins:
			return this.GetSkinsData();
		case EditorShopItemsType.Hats:
			return this.GetWearData(EditorShopItemsType.Hats);
		case EditorShopItemsType.Armor:
			return this.GetWearData(EditorShopItemsType.Armor);
		case EditorShopItemsType.Capes:
			return this.GetWearData(EditorShopItemsType.Capes);
		case EditorShopItemsType.Boots:
			return this.GetWearData(EditorShopItemsType.Boots);
		case EditorShopItemsType.All:
		{
			List<EditorShopItemData> wearData = this.GetWearData(EditorShopItemsType.Hats);
			wearData.AddRange(this.GetWearData(EditorShopItemsType.Armor));
			wearData.AddRange(this.GetWearData(EditorShopItemsType.Capes));
			wearData.AddRange(this.GetWearData(EditorShopItemsType.Boots));
			wearData.AddRange(this.GetWeaponsData());
			wearData.AddRange(this.GetSkinsData());
			return wearData;
		}
		default:
			return null;
		}
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x0010AF88 File Offset: 0x00109188
	private void ClearShopItemList()
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			NGUITools.Destroy(componentsInChildren[i].gameObject);
		}
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x0010AFC4 File Offset: 0x001091C4
	private bool IsItemEqualFilter(EditorShopItemData itemData)
	{
		return this._currentFilter == EditorShopItemsType.All || (this._currentFilter == EditorShopItemsType.OnlyNew && itemData.isNew) || (this._currentFilter == EditorShopItemsType.OnlyTop && itemData.isTop) || (this._currentFilter == EditorShopItemsType.OnlyDiscount && itemData.discount > 0) || this._currentFilter == itemData.type;
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x0010B040 File Offset: 0x00109240
	private int SortingWeaponByOrder(Transform left, Transform right)
	{
		EditorShopItem component = left.GetComponent<EditorShopItem>();
		EditorShopItem component2 = right.GetComponent<EditorShopItem>();
		string s = component.prefabName.Replace("Weapon", string.Empty);
		int num = 0;
		int.TryParse(s, out num);
		string s2 = component2.prefabName.Replace("Weapon", string.Empty);
		int num2 = 0;
		int.TryParse(s2, out num2);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x0010B0B8 File Offset: 0x001092B8
	private void FillShopItemList()
	{
		if (this._shopItemsData == null || this._isStart)
		{
			this._shopItemsData = this.GetItemsList(this._currentFilter);
			this.applyWindow.itemsData = this._shopItemsData;
		}
		this.ClearShopItemList();
		for (int i = 0; i < this._shopItemsData.Count; i++)
		{
			if (this.IsItemEqualFilter(this._shopItemsData[i]))
			{
				GameObject gameObject = NGUITools.AddChild(this.grid.gameObject, this.itemPrefab);
				gameObject.name = string.Format("{0:00}", i);
				EditorShopItem component = gameObject.GetComponent<EditorShopItem>();
				if (component != null)
				{
					component.SetData(this._shopItemsData[i]);
				}
				gameObject.gameObject.SetActive(true);
			}
		}
		if (this._currentFilter == EditorShopItemsType.Weapon)
		{
			this.grid.onCustomSort = new Comparison<Transform>(this.SortingWeaponByOrder);
			this.grid.sorting = UIGrid.Sorting.Custom;
		}
		else
		{
			this.grid.sorting = UIGrid.Sorting.Alphabetic;
		}
		this.grid.Reposition();
		this.scrollView.ResetPosition();
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x0010B1F4 File Offset: 0x001093F4
	private void ClearPromoActionsData()
	{
		this._discountsData.Clear();
		this._topSellersData.Clear();
		this._newsData.Clear();
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x0010B218 File Offset: 0x00109418
	public IEnumerator GetPromoActionsData()
	{
		WWWForm webForm = new WWWForm();
		string appVersion = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
		webForm.AddField("app_version", appVersion);
		string promoActionAddress = this.applyWindow.GetPromoActionUrl();
		WWW downloadData = Tools.CreateWwwIfNotConnected(promoActionAddress, webForm, string.Empty, null);
		if (downloadData == null)
		{
			yield break;
		}
		yield return downloadData;
		if (!string.IsNullOrEmpty(downloadData.error))
		{
			Debug.LogWarning("GetPromoActionsData error: " + downloadData.error);
			this.ClearPromoActionsData();
			yield break;
		}
		string responseText = URLs.Sanitize(downloadData);
		Dictionary<string, object> promoActionsData = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (promoActionsData == null)
		{
			Debug.LogWarning("GetPromoActionsData promoActionsData = null");
			yield break;
		}
		this.ClearPromoActionsData();
		if (promoActionsData.ContainsKey("news_up"))
		{
			object newsObject = promoActionsData["news_up"];
			if (newsObject != null)
			{
				List<object> newsList = newsObject as List<object>;
				if (newsList != null)
				{
					foreach (object obj in newsList)
					{
						string element = (string)obj;
						this._newsData.Add(element);
					}
				}
			}
		}
		if (promoActionsData.ContainsKey("topSellers_up"))
		{
			object topsObject = promoActionsData["topSellers_up"];
			if (topsObject != null)
			{
				List<object> topsList = topsObject as List<object>;
				if (topsList != null)
				{
					foreach (object obj2 in topsList)
					{
						string element2 = (string)obj2;
						this._topSellersData.Add(element2);
					}
				}
			}
		}
		if (promoActionsData.ContainsKey("discounts_up"))
		{
			object discountObject = promoActionsData["discounts_up"];
			if (discountObject != null)
			{
				List<object> discountListObjects = discountObject as List<object>;
				if (discountListObjects != null)
				{
					for (int i = 0; i < discountListObjects.Count; i++)
					{
						List<object> element3 = discountListObjects[i] as List<object>;
						string key = (string)element3[0];
						int value = Convert.ToInt32((long)element3[1]);
						this._discountsData.Add(key, value);
					}
				}
			}
		}
		this.FillShopItemList();
		this._isStart = false;
		yield break;
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x0010B234 File Offset: 0x00109434
	public void ChangeCurrentFilter(UIToggle toggle)
	{
		if (toggle == null || !toggle.value)
		{
			return;
		}
		this.ClearShopItemList();
		string name = toggle.name;
		switch (name)
		{
		case "OnlyWeaponCheckbox":
			this._currentFilter = EditorShopItemsType.Weapon;
			break;
		case "OnlySkinsCheckbox":
			this._currentFilter = EditorShopItemsType.Skins;
			break;
		case "OnlyHatsCheckbox":
			this._currentFilter = EditorShopItemsType.Hats;
			break;
		case "OnlyArmorCheckbox":
			this._currentFilter = EditorShopItemsType.Armor;
			break;
		case "OnlyCapesCheckbox":
			this._currentFilter = EditorShopItemsType.Capes;
			break;
		case "OnlyBootsCheckbox":
			this._currentFilter = EditorShopItemsType.Boots;
			break;
		case "AllCheckbox":
			this._currentFilter = EditorShopItemsType.All;
			break;
		case "OnlyNewCheckbox":
			this._currentFilter = EditorShopItemsType.OnlyNew;
			break;
		case "OnlyTopsCheckbox":
			this._currentFilter = EditorShopItemsType.OnlyTop;
			break;
		case "OnlyDiscountCheckbox":
			this._currentFilter = EditorShopItemsType.OnlyDiscount;
			break;
		}
		if (this._isStart)
		{
			base.StartCoroutine(this.GetPromoActionsData());
		}
		else
		{
			this.FillShopItemList();
		}
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x0010B3E0 File Offset: 0x001095E0
	public void SendDataToServerClick()
	{
		this.applyWindow.Show(UploadShopItemDataToServer.TypeWindow.UploadFileToServer);
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x0010B3F0 File Offset: 0x001095F0
	public void CheckAllTopState(UIToggle topAllCheck)
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].topCheckbox.value = topAllCheck.value;
			componentsInChildren[i].SetTopState();
		}
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x0010B43C File Offset: 0x0010963C
	public void CheckAllNewState(UIToggle newAllCheck)
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].newCheckbox.value = newAllCheck.value;
			componentsInChildren[i].SetNewState();
		}
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x0010B488 File Offset: 0x00109688
	public void SetAllDiscounts(UIInput inputAllDiscount)
	{
		EditorShopItem[] componentsInChildren = this.grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].discountInput.label.text = inputAllDiscount.label.text;
			componentsInChildren[i].SetDiscount();
		}
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x0010B4DC File Offset: 0x001096DC
	public static void CopyTextInClipboard(string text)
	{
		TextEditor textEditor = new TextEditor();
		textEditor.content = new GUIContent(text);
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x0010B508 File Offset: 0x00109708
	public void GenerateUploadTextButtonClick()
	{
		this.generateTextFile.value = this.applyWindow.GenerateTextForUploadFile();
		EditorListBuilder.CopyTextInClipboard(this.generateTextFile.value);
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0010B53C File Offset: 0x0010973C
	public void DownloadDataClick()
	{
		this._isStart = true;
		this.applyWindow.Show(UploadShopItemDataToServer.TypeWindow.ChangePlatform);
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x0010B554 File Offset: 0x00109754
	public void ShowPromoActionsPanel()
	{
		this.promoActionPanel.alpha = 1f;
		this.x3Panel.alpha = 0f;
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x0010B584 File Offset: 0x00109784
	public void ShowX3ActionsPanel()
	{
		this.promoActionPanel.alpha = 0f;
		this.x3Panel.alpha = 1f;
	}

	// Token: 0x040025E3 RID: 9699
	public GameObject itemPrefab;

	// Token: 0x040025E4 RID: 9700
	public UIScrollView scrollView;

	// Token: 0x040025E5 RID: 9701
	public UIGrid grid;

	// Token: 0x040025E6 RID: 9702
	public UIToggle defaultFilter;

	// Token: 0x040025E7 RID: 9703
	public UploadShopItemDataToServer applyWindow;

	// Token: 0x040025E8 RID: 9704
	public UIInput generateTextFile;

	// Token: 0x040025E9 RID: 9705
	public UIWidget promoActionPanel;

	// Token: 0x040025EA RID: 9706
	public UIWidget x3Panel;

	// Token: 0x040025EB RID: 9707
	private Dictionary<string, int> _discountsData = new Dictionary<string, int>();

	// Token: 0x040025EC RID: 9708
	private List<string> _topSellersData = new List<string>();

	// Token: 0x040025ED RID: 9709
	private List<string> _newsData = new List<string>();

	// Token: 0x040025EE RID: 9710
	private EditorShopItemsType _currentFilter;

	// Token: 0x040025EF RID: 9711
	private bool _isStart;

	// Token: 0x040025F0 RID: 9712
	private List<EditorShopItemData> _shopItemsData;
}
