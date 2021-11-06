using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
public sealed class ChestBonusController : MonoBehaviour
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x0600328D RID: 12941 RVA: 0x00106214 File Offset: 0x00104414
	// (remove) Token: 0x0600328E RID: 12942 RVA: 0x0010622C File Offset: 0x0010442C
	public static event ChestBonusController.OnChestBonusEnabledDelegate OnChestBonusChange;

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x06003290 RID: 12944 RVA: 0x0010624C File Offset: 0x0010444C
	// (set) Token: 0x0600328F RID: 12943 RVA: 0x00106244 File Offset: 0x00104444
	public static ChestBonusController Get { get; private set; }

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x06003292 RID: 12946 RVA: 0x00106260 File Offset: 0x00104460
	// (set) Token: 0x06003291 RID: 12945 RVA: 0x00106254 File Offset: 0x00104454
	public bool IsBonusActive { get; private set; }

	// Token: 0x06003293 RID: 12947 RVA: 0x00106268 File Offset: 0x00104468
	private void Start()
	{
		ChestBonusController.Get = this;
		this._bonusesData = new ChestBonusesData();
		this._timeStartBonus = default(DateTime);
		this._timeEndBonus = default(DateTime);
		Task firstResponse = PersistentCacheManager.Instance.FirstResponse;
		base.StartCoroutine(this.GetEventBonusInfoLoop(firstResponse));
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x001062C0 File Offset: 0x001044C0
	private void OnDestroy()
	{
		ChestBonusController.Get = null;
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x001062C8 File Offset: 0x001044C8
	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			base.StartCoroutine(this.DownloadDataAboutBonuses());
		}
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x001062E0 File Offset: 0x001044E0
	private IEnumerator GetEventBonusInfoLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		for (;;)
		{
			yield return base.StartCoroutine(this.DownloadDataAboutBonuses());
			while (Time.realtimeSinceStartup - this._eventGetBonusInfoStartTime < 870f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x0010630C File Offset: 0x0010450C
	private void Update()
	{
		if (!this.IsBonusActive)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckEventTime >= 1f)
		{
			this.IsBonusActive = this.IsBonusActivate();
			if (this._lastBonusActive != this.IsBonusActive && ChestBonusController.OnChestBonusChange != null)
			{
				ChestBonusController.OnChestBonusChange();
				this._lastBonusActive = this.IsBonusActive;
			}
			this._lastCheckEventTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x00106384 File Offset: 0x00104584
	private IEnumerator DownloadDataAboutBonuses()
	{
		if (this._isGetBonusInfoRunning)
		{
			yield break;
		}
		this._eventGetBonusInfoStartTime = Time.realtimeSinceStartup;
		this._isGetBonusInfoRunning = true;
		string bonusesDataAddress = ChestBonusModel.GetUrlForDownloadBonusesData();
		string cachedResponse = PersistentCacheManager.Instance.GetValue(bonusesDataAddress);
		string responseText;
		if (!string.IsNullOrEmpty(cachedResponse))
		{
			responseText = cachedResponse;
		}
		else
		{
			WWW downloadData = Tools.CreateWwwIfNotConnected(bonusesDataAddress);
			if (downloadData == null)
			{
				this._isGetBonusInfoRunning = false;
				yield break;
			}
			yield return downloadData;
			if (!string.IsNullOrEmpty(downloadData.error))
			{
				Debug.LogError("DownloadDataAboutBonuses error: " + downloadData.error);
				this._bonusesData.Clear();
				this._isGetBonusInfoRunning = false;
				yield break;
			}
			responseText = URLs.Sanitize(downloadData);
			PersistentCacheManager.Instance.SetValue(bonusesDataAddress, responseText);
		}
		Dictionary<string, object> bonusesData = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (bonusesData == null)
		{
			Debug.LogWarning("DownloadDataAboutBonuses bonusesData = null");
			this._isGetBonusInfoRunning = false;
			yield break;
		}
		ChestBonusController.chestBonusesObtainedOnceInCurrentRun = true;
		this._bonusesData.Clear();
		if (bonusesData.ContainsKey("start"))
		{
			this._bonusesData.timeStart = Convert.ToInt32((long)bonusesData["start"]);
		}
		if (bonusesData.ContainsKey("duration"))
		{
			long duration = Math.Min(Convert.ToInt64(bonusesData["duration"]), 2147483647L);
			duration = Math.Max(duration, -2147483648L);
			this._bonusesData.duration = Convert.ToInt32(duration);
		}
		if (this._bonusesData.timeStart == 0 || this._bonusesData.duration == 0)
		{
			this._isGetBonusInfoRunning = false;
			yield break;
		}
		if (!bonusesData.ContainsKey("bonuses"))
		{
			this._isGetBonusInfoRunning = false;
			yield break;
		}
		List<object> bonusesList = bonusesData["bonuses"] as List<object>;
		if (bonusesList != null)
		{
			this._bonusesData.bonuses = new List<ChestBonusData>();
			for (int i = 0; i < bonusesList.Count; i++)
			{
				Dictionary<string, object> bonusElement = bonusesList[i] as Dictionary<string, object>;
				if (bonusElement != null)
				{
					ChestBonusData newBonus = new ChestBonusData();
					if (bonusElement.ContainsKey("linkKey"))
					{
						newBonus.linkKey = (string)bonusElement["linkKey"];
					}
					if (bonusElement.ContainsKey("isVisible"))
					{
						int value = Convert.ToInt32((long)bonusElement["isVisible"]);
						newBonus.isVisible = (value == 1);
					}
					if (bonusElement.ContainsKey("items"))
					{
						List<object> bonusItemsList = bonusElement["items"] as List<object>;
						if (bonusItemsList != null)
						{
							newBonus.items = new List<ChestBonusItemData>();
							for (int j = 0; j < bonusItemsList.Count; j++)
							{
								Dictionary<string, object> bonusItemData = bonusItemsList[j] as Dictionary<string, object>;
								if (bonusItemData != null)
								{
									ChestBonusItemData newItem = new ChestBonusItemData();
									if (bonusItemData.ContainsKey("tag"))
									{
										newItem.tag = (string)bonusItemData["tag"];
									}
									if (bonusItemData.ContainsKey("count"))
									{
										newItem.count = Convert.ToInt32((long)bonusItemData["count"]);
									}
									if (bonusItemData.ContainsKey("timeLife"))
									{
										newItem.timeLife = Convert.ToInt32((long)bonusItemData["timeLife"]);
									}
									newBonus.items.Add(newItem);
								}
							}
						}
					}
					this._bonusesData.bonuses.Add(newBonus);
				}
			}
		}
		this._timeStartBonus = StarterPackModel.GetCurrentTimeByUnixTime(this._bonusesData.timeStart);
		int timeEnd = this._bonusesData.timeStart + this._bonusesData.duration;
		this._timeEndBonus = StarterPackModel.GetCurrentTimeByUnixTime(timeEnd);
		this.IsBonusActive = this.IsBonusActivate();
		if (ChestBonusController.OnChestBonusChange != null)
		{
			ChestBonusController.OnChestBonusChange();
		}
		this._isGetBonusInfoRunning = false;
		yield break;
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x001063A0 File Offset: 0x001045A0
	private bool IsBonusActivate()
	{
		if (this._bonusesData.timeStart == 0 || this._bonusesData.duration == 0)
		{
			return false;
		}
		DateTime utcNow = DateTime.UtcNow;
		return utcNow >= this._timeStartBonus && utcNow <= this._timeEndBonus;
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x001063F8 File Offset: 0x001045F8
	public void ShowBonusWindowForItem(PurchaseEventArgs purchaseInfo)
	{
		ChestBonusData bonusData = this.GetBonusData(purchaseInfo);
		BankController instance = BankController.Instance;
		if (bonusData != null && instance != null)
		{
			instance.bonusDetailView.Show(bonusData);
		}
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x00106434 File Offset: 0x00104634
	public bool IsBonusActiveForItem(PurchaseEventArgs purchaseInfo)
	{
		if (!this.IsBonusActive)
		{
			return false;
		}
		ChestBonusData bonusData = this.GetBonusData(purchaseInfo);
		return bonusData != null && bonusData.isVisible;
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x00106468 File Offset: 0x00104668
	public ChestBonusData GetBonusData(PurchaseEventArgs purchaseInfo)
	{
		bool isGemsPack = purchaseInfo.Currency == "GemsCurrency";
		return this.GetBonusData(isGemsPack, purchaseInfo.Index);
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x00106494 File Offset: 0x00104694
	private ChestBonusData GetBonusData(bool isGemsPack, int packOrder)
	{
		if (this._bonusesData == null || this._bonusesData.bonuses == null)
		{
			return null;
		}
		string arg = (!isGemsPack) ? "coins" : "gems";
		string b = string.Format("{0}_{1}", arg, packOrder + 1);
		for (int i = 0; i < this._bonusesData.bonuses.Count; i++)
		{
			ChestBonusData chestBonusData = this._bonusesData.bonuses[i];
			if (chestBonusData.linkKey == b)
			{
				return chestBonusData;
			}
		}
		return null;
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x00106530 File Offset: 0x00104730
	public static bool TryTakeChestBonus(bool isGemsPack, int packOrder)
	{
		ChestBonusController get = ChestBonusController.Get;
		if (get == null)
		{
			return false;
		}
		if (!get.IsBonusActive)
		{
			return false;
		}
		ChestBonusData bonusData = get.GetBonusData(isGemsPack, packOrder);
		if (bonusData == null)
		{
			return false;
		}
		if (bonusData.items == null || bonusData.items.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < bonusData.items.Count; i++)
		{
			ChestBonusItemData chestBonusItemData = bonusData.items[i];
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(chestBonusItemData.tag);
			ShopNGUIController.CategoryNames category = itemCategory;
			string tag = chestBonusItemData.tag;
			int count = chestBonusItemData.count;
			int timeLife = chestBonusItemData.timeLife;
			int timeForRentIndexForOldTempWeapons = 0;
			if (timeLife != -1)
			{
				int days = timeLife / 24;
				timeForRentIndexForOldTempWeapons = TempItemsController.RentIndexFromDays(days);
			}
			ShopNGUIController.ProvideItem(category, tag, count, false, timeForRentIndexForOldTempWeapons, null, delegate(string wearId)
			{
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.weaponsInGame != null)
				{
					if (ShopNGUIController.GuiActive && ShopNGUIController.sharedShop != null)
					{
						int itemCategory2 = ItemDb.GetItemCategory(wearId);
						if (itemCategory2 != -1)
						{
							ShopNGUIController.EquipWearInCategoryIfNotEquiped(wearId, (ShopNGUIController.CategoryNames)itemCategory2, WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null);
						}
					}
					else
					{
						int itemCategory3 = ItemDb.GetItemCategory(wearId);
						if (itemCategory3 != -1)
						{
							ShopNGUIController.SetAsEquippedAndSendToServer(wearId, (ShopNGUIController.CategoryNames)itemCategory3);
							if (ShopNGUIController.IsWearCategory((ShopNGUIController.CategoryNames)itemCategory3))
							{
								ShopNGUIController.SendEquippedWearInCategory(wearId, (ShopNGUIController.CategoryNames)itemCategory3, string.Empty);
							}
						}
					}
				}
			}, true, true, false);
			TempItemsController.sharedController.ExpiredItems.Remove(tag);
		}
		return true;
	}

	// Token: 0x0400251D RID: 9501
	private const float BonusUpdateTimeout = 870f;

	// Token: 0x0400251E RID: 9502
	public static bool chestBonusesObtainedOnceInCurrentRun;

	// Token: 0x0400251F RID: 9503
	private ChestBonusesData _bonusesData;

	// Token: 0x04002520 RID: 9504
	private float _lastCheckEventTime;

	// Token: 0x04002521 RID: 9505
	private bool _lastBonusActive;

	// Token: 0x04002522 RID: 9506
	private DateTime _timeStartBonus;

	// Token: 0x04002523 RID: 9507
	private DateTime _timeEndBonus;

	// Token: 0x04002524 RID: 9508
	private bool _isGetBonusInfoRunning;

	// Token: 0x04002525 RID: 9509
	private float _eventGetBonusInfoStartTime;

	// Token: 0x02000920 RID: 2336
	// (Invoke) Token: 0x06005120 RID: 20768
	public delegate void OnChestBonusEnabledDelegate();
}
