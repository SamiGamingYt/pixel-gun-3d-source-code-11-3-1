using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.amazon.device.iap.cpt;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200075E RID: 1886
internal sealed class StarterPackController : MonoBehaviour
{
	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06004230 RID: 16944 RVA: 0x0015FD0C File Offset: 0x0015DF0C
	// (remove) Token: 0x06004231 RID: 16945 RVA: 0x0015FD24 File Offset: 0x0015DF24
	public static event StarterPackController.OnStarterPackEnableDelegate OnStarterPackEnable;

	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x06004233 RID: 16947 RVA: 0x0015FD48 File Offset: 0x0015DF48
	// (set) Token: 0x06004232 RID: 16946 RVA: 0x0015FD3C File Offset: 0x0015DF3C
	public bool isEventActive { get; private set; }

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x06004235 RID: 16949 RVA: 0x0015FD58 File Offset: 0x0015DF58
	// (set) Token: 0x06004234 RID: 16948 RVA: 0x0015FD50 File Offset: 0x0015DF50
	public static StarterPackController Get { get; private set; }

	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x06004237 RID: 16951 RVA: 0x0015FD6C File Offset: 0x0015DF6C
	// (set) Token: 0x06004236 RID: 16950 RVA: 0x0015FD60 File Offset: 0x0015DF60
	private List<string> BuyAnroidStarterPack { get; set; }

	// Token: 0x06004238 RID: 16952 RVA: 0x0015FD74 File Offset: 0x0015DF74
	private void Start()
	{
		StarterPackController.Get = this;
		this._timeLiveEvent = default(TimeSpan);
		this._starterPacksData = new List<StarterPackData>();
		this._orderCurrentPack = -1;
		this._storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
		this.BuyAnroidStarterPack = new List<string>();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06004239 RID: 16953 RVA: 0x0015FDCC File Offset: 0x0015DFCC
	private void Destroy()
	{
		StarterPackController.Get = null;
	}

	// Token: 0x0600423A RID: 16954 RVA: 0x0015FDD4 File Offset: 0x0015DFD4
	private void Update()
	{
		if (!this.isEventActive)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckEventTime >= 1f)
		{
			this._timeLiveEvent = DateTime.UtcNow - this._timeStartEvent;
			this.isEventActive = (this._timeLiveEvent <= StarterPackModel.MaxLiveTimeEvent);
			if (!this.isEventActive)
			{
				this.FinishCurrentStarterPack();
			}
			this._lastCheckEventTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x0600423B RID: 16955 RVA: 0x0015FE4C File Offset: 0x0015E04C
	private void FinishCurrentStarterPack()
	{
		Storager.setString("StartTimeShowStarterPack", string.Empty, false);
		Storager.setString("TimeEndStarterPack", DateTime.UtcNow.ToString("s"), false);
		Storager.setInt("NextNumberStarterPack", this._orderCurrentPack + 1, false);
		this.isEventActive = false;
		this.CheckSendEventChangeEnabled();
	}

	// Token: 0x0600423C RID: 16956 RVA: 0x0015FEA8 File Offset: 0x0015E0A8
	private void CancelCurrentEvent()
	{
		Storager.setString("StartTimeShowStarterPack", string.Empty, false);
		Storager.setString("TimeEndStarterPack", string.Empty, false);
		Storager.setInt("NextNumberStarterPack", 0, false);
		PlayerPrefs.SetString("LastTimeShowStarterPack", string.Empty);
		PlayerPrefs.SetInt("CountShownStarterPack", 1);
		PlayerPrefs.Save();
		this.isEventActive = false;
		this.CheckSendEventChangeEnabled();
	}

	// Token: 0x0600423D RID: 16957 RVA: 0x0015FF10 File Offset: 0x0015E110
	private void ResetToDefaultStateIfNeed()
	{
		if (Storager.hasKey("StartTimeShowStarterPack"))
		{
			string @string = Storager.getString("StartTimeShowStarterPack", false);
			if (!string.IsNullOrEmpty(@string))
			{
				Storager.setString("StartTimeShowStarterPack", string.Empty, false);
			}
		}
		if (Storager.hasKey("TimeEndStarterPack"))
		{
			string string2 = Storager.getString("TimeEndStarterPack", false);
			if (!string.IsNullOrEmpty(string2))
			{
				Storager.setString("TimeEndStarterPack", string.Empty, false);
			}
		}
		int @int = Storager.getInt("NextNumberStarterPack", false);
		if (@int > 0)
		{
			Storager.setInt("NextNumberStarterPack", 0, false);
		}
		string string3 = PlayerPrefs.GetString("LastTimeShowStarterPack", string.Empty);
		if (!string.IsNullOrEmpty(string3))
		{
			PlayerPrefs.SetString("LastTimeShowStarterPack", string.Empty);
		}
		int int2 = PlayerPrefs.GetInt("CountShownStarterPack", 1);
		if (int2 != 1)
		{
			PlayerPrefs.SetInt("CountShownStarterPack", 1);
		}
	}

	// Token: 0x0600423E RID: 16958 RVA: 0x0015FFF4 File Offset: 0x0015E1F4
	private void CheckCancelCurrentStarterPack()
	{
		this.ResetToDefaultStateIfNeed();
		if (this.isEventActive)
		{
			this.isEventActive = false;
			this.CheckSendEventChangeEnabled();
		}
	}

	// Token: 0x0600423F RID: 16959 RVA: 0x00160014 File Offset: 0x0015E214
	public void CheckFindStoreKitEventListner()
	{
		if (this._storeKitEventListener != null)
		{
			return;
		}
		this._storeKitEventListener = UnityEngine.Object.FindObjectOfType<StoreKitEventListener>();
	}

	// Token: 0x06004240 RID: 16960 RVA: 0x00160034 File Offset: 0x0015E234
	private IEnumerator DownloadDataAboutEvent()
	{
		if (this._isDownloadDataRun)
		{
			yield break;
		}
		this._isDownloadDataRun = true;
		string eventDataAddress = StarterPackModel.GetUrlForDownloadEventData();
		WWW downloadData = Tools.CreateWwwIfNotConnected(eventDataAddress);
		if (downloadData == null)
		{
			this._isDownloadDataRun = false;
			yield break;
		}
		yield return downloadData;
		if (!string.IsNullOrEmpty(downloadData.error))
		{
			Debug.LogFormat("DownloadDataAboutEvent error: {0}", new object[]
			{
				downloadData.error
			});
			this._starterPacksData.Clear();
			this._isDownloadDataRun = false;
			yield break;
		}
		string responseText = URLs.Sanitize(downloadData);
		Dictionary<string, object> eventData = Rilisoft.MiniJson.Json.Deserialize(responseText) as Dictionary<string, object>;
		if (eventData == null)
		{
			Debug.Log("DownloadDataAboutEvent eventData = null");
			this._isDownloadDataRun = false;
			yield break;
		}
		this._starterPacksData.Clear();
		if (!eventData.ContainsKey("packs"))
		{
			this._isDownloadDataRun = false;
			yield break;
		}
		List<object> packsList = eventData["packs"] as List<object>;
		if (packsList != null)
		{
			for (int i = 0; i < packsList.Count; i++)
			{
				Dictionary<string, object> element = packsList[i] as Dictionary<string, object>;
				if (element != null)
				{
					StarterPackData data = new StarterPackData();
					if (element.ContainsKey("blockLevel"))
					{
						data.blockLevel = Convert.ToInt32((long)element["blockLevel"]);
					}
					if (element.ContainsKey("coinsCost"))
					{
						data.coinsCost = Convert.ToInt32((long)element["coinsCost"]);
					}
					else if (element.ContainsKey("gemsCost"))
					{
						data.gemsCost = Convert.ToInt32((long)element["gemsCost"]);
					}
					if (element.ContainsKey("enable"))
					{
						int enableFlag = Convert.ToInt32((long)element["enable"]);
						data.enable = (enableFlag == 1);
					}
					if (element.ContainsKey("items"))
					{
						List<object> itemsObjects = element["items"] as List<object>;
						if (itemsObjects != null)
						{
							data.items = new List<StarterPackItemData>();
							for (int j = 0; j < itemsObjects.Count; j++)
							{
								Dictionary<string, object> itemsElement = itemsObjects[j] as Dictionary<string, object>;
								StarterPackItemData newItem = new StarterPackItemData();
								if (itemsElement != null)
								{
									if (itemsElement.ContainsKey("tagsVariant"))
									{
										List<object> tagsVariant = itemsElement["tagsVariant"] as List<object>;
										if (tagsVariant != null)
										{
											for (int k = 0; k < tagsVariant.Count; k++)
											{
												newItem.variantTags.Add((string)tagsVariant[k]);
											}
										}
									}
									if (itemsElement.ContainsKey("count"))
									{
										newItem.count = Convert.ToInt32((long)itemsElement["count"]);
									}
									data.items.Add(newItem);
								}
							}
						}
					}
					if (element.ContainsKey("sale"))
					{
						data.sale = Convert.ToInt32((long)element["sale"]);
					}
					if (element.ContainsKey("coinsCount"))
					{
						data.coinsCount = Convert.ToInt32((long)element["coinsCount"]);
					}
					if (element.ContainsKey("gemsCount"))
					{
						data.gemsCount = Convert.ToInt32((long)element["gemsCount"]);
					}
					this._starterPacksData.Add(data);
				}
			}
		}
		this._isDownloadDataRun = false;
		yield break;
	}

	// Token: 0x06004241 RID: 16961 RVA: 0x00160050 File Offset: 0x0015E250
	private bool IsPlayerNotPayBeforeStartEvent()
	{
		bool flag = Storager.getInt("PayingUser", true) == 0;
		return flag || this.isEventActive;
	}

	// Token: 0x06004242 RID: 16962 RVA: 0x00160084 File Offset: 0x0015E284
	private int GetMaxValidOrderPack()
	{
		int result = -1;
		int currentLevel = ExperienceController.GetCurrentLevel();
		for (int i = 0; i < this._starterPacksData.Count; i++)
		{
			if (this._starterPacksData[i].blockLevel <= currentLevel)
			{
				result = i;
			}
		}
		return result;
	}

	// Token: 0x06004243 RID: 16963 RVA: 0x001600D0 File Offset: 0x0015E2D0
	private int GetOrderCurrentPack()
	{
		int @int = Storager.getInt("NextNumberStarterPack", false);
		if (@int >= this._starterPacksData.Count)
		{
			return -1;
		}
		return @int;
	}

	// Token: 0x06004244 RID: 16964 RVA: 0x00160100 File Offset: 0x0015E300
	private bool IsCurrentPackEnable()
	{
		StarterPackData currentPackData = this.GetCurrentPackData();
		return currentPackData == null || currentPackData.enable;
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x00160124 File Offset: 0x0015E324
	private bool IsStarterPackBuyByPackOrder(int packOrder)
	{
		string storageIdByPackOrder = this.GetStorageIdByPackOrder(packOrder);
		return !string.IsNullOrEmpty(storageIdByPackOrder) && this.IsStarterPackBuy(storageIdByPackOrder);
	}

	// Token: 0x06004246 RID: 16966 RVA: 0x00160150 File Offset: 0x0015E350
	private bool IsInvalidCurrentPack()
	{
		return this.IsInvalidPack(this._orderCurrentPack);
	}

	// Token: 0x06004247 RID: 16967 RVA: 0x00160160 File Offset: 0x0015E360
	private bool IsInvalidPack(int packOrder)
	{
		StarterPackModel.TypePack packType = this.GetPackType(packOrder);
		if (packType != StarterPackModel.TypePack.Items)
		{
			return false;
		}
		List<StarterPackItemData> items = this._starterPacksData[packOrder].items;
		for (int i = 0; i < items.Count; i++)
		{
			if (string.IsNullOrEmpty(items[i].validTag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004248 RID: 16968 RVA: 0x001601C0 File Offset: 0x0015E3C0
	private bool IsEventInEndState()
	{
		string @string = Storager.getString("StartTimeShowStarterPack", false);
		string string2 = Storager.getString("TimeEndStarterPack", false);
		return @string == string.Empty && !string.IsNullOrEmpty(string2);
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x00160204 File Offset: 0x0015E404
	private bool IsCooldownEventEnd()
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime timeDataEvent = StarterPackModel.GetTimeDataEvent("TimeEndStarterPack");
		return utcNow - timeDataEvent >= StarterPackModel.CooldownTimeEvent;
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x00160234 File Offset: 0x0015E434
	private IEnumerator CheckStartStarterPackEvent()
	{
		if (!TrainingController.TrainingCompleted)
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (!this.IsPlayerNotPayBeforeStartEvent() || ExpController.LobbyLevel < 3)
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (Time.time - this.timeUpdateConfig > 3600f)
		{
			yield return base.StartCoroutine(this.DownloadDataAboutEvent());
			this.timeUpdateConfig = Time.time;
		}
		if (this.IsEventInEndState() && !this.IsCooldownEventEnd())
		{
			yield break;
		}
		this._orderCurrentPack = this.GetOrderCurrentPack();
		if (this._orderCurrentPack == -1)
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		int maxValidOrder = this.GetMaxValidOrderPack();
		if (maxValidOrder == -1)
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (maxValidOrder > this._orderCurrentPack)
		{
			this._orderCurrentPack = maxValidOrder;
			Storager.setInt("NextNumberStarterPack", this._orderCurrentPack, false);
			Storager.setString("StartTimeShowStarterPack", string.Empty, false);
		}
		else if (maxValidOrder < this._orderCurrentPack)
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (!this.IsCurrentPackEnable())
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (this.IsStarterPackBuyOnAndroid(this._orderCurrentPack))
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (this.IsStarterPackBuyByPackOrder(this._orderCurrentPack))
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		if (this.IsInvalidCurrentPack())
		{
			this.CheckCancelCurrentStarterPack();
			yield break;
		}
		string timeStartSting = Storager.getString("StartTimeShowStarterPack", false);
		if (string.IsNullOrEmpty(timeStartSting))
		{
			this.InitializeEvent();
		}
		else
		{
			this._timeStartEvent = StarterPackModel.GetTimeDataEvent("StartTimeShowStarterPack");
		}
		bool previusActive = this.isEventActive;
		this.isEventActive = true;
		if (previusActive != this.isEventActive)
		{
			this.CheckSendEventChangeEnabled();
		}
		yield break;
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x00160250 File Offset: 0x0015E450
	public void CheckShowStarterPack()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			Debug.Log("Skipping CheckShowStarterPack() on WSA.");
			return;
		}
		base.StartCoroutine(this.CheckStartStarterPackEvent());
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x00160284 File Offset: 0x0015E484
	private void InitializeEvent()
	{
		this._timeStartEvent = DateTime.UtcNow;
		Storager.setString("StartTimeShowStarterPack", this._timeStartEvent.ToString("s"), false);
		Storager.setString("TimeEndStarterPack", string.Empty, false);
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x001602C8 File Offset: 0x0015E4C8
	public StarterPackData GetCurrentPackData()
	{
		if (this._orderCurrentPack == -1)
		{
			return null;
		}
		return this._starterPacksData[this._orderCurrentPack];
	}

	// Token: 0x0600424E RID: 16974 RVA: 0x001602EC File Offset: 0x0015E4EC
	public StarterPackModel.TypePack GetPackType(int packOrder)
	{
		if (packOrder == -1)
		{
			return StarterPackModel.TypePack.None;
		}
		if (this._starterPacksData[packOrder].items != null)
		{
			return StarterPackModel.TypePack.Items;
		}
		if (this._starterPacksData[packOrder].coinsCount > 0)
		{
			return StarterPackModel.TypePack.Coins;
		}
		if (this._starterPacksData[packOrder].gemsCount > 0)
		{
			return StarterPackModel.TypePack.Gems;
		}
		return StarterPackModel.TypePack.None;
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x00160350 File Offset: 0x0015E550
	public StarterPackModel.TypePack GetCurrentPackType()
	{
		return this.GetPackType(this._orderCurrentPack);
	}

	// Token: 0x06004250 RID: 16976 RVA: 0x00160360 File Offset: 0x0015E560
	public bool TryTakePurchasesForCurrentPack(string productId, bool isRestore = false)
	{
		return this.TryTakePurchases(productId, this._orderCurrentPack, isRestore);
	}

	// Token: 0x06004251 RID: 16977 RVA: 0x00160370 File Offset: 0x0015E570
	private bool IsStarterPackBuy(string storageId)
	{
		return Storager.hasKey(storageId) && Storager.getInt(storageId, false) == 1;
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x0016038C File Offset: 0x0015E58C
	private bool TryTakePurchases(string productId, int packOrder, bool isRestore = false)
	{
		if (this._starterPacksData.Count == 0)
		{
			return false;
		}
		if (packOrder == -1)
		{
			return false;
		}
		StarterPackModel.TypePack packType = this.GetPackType(packOrder);
		if (packType == StarterPackModel.TypePack.None)
		{
			return false;
		}
		if (this.IsStarterPackBuy(productId))
		{
			return false;
		}
		StarterPackData starterPackData = this._starterPacksData[packOrder];
		switch (packType)
		{
		case StarterPackModel.TypePack.Items:
			if (starterPackData.items.Count != 0)
			{
				for (int i = 0; i < starterPackData.items.Count; i++)
				{
					string validTag = starterPackData.items[i].validTag;
					int itemCategory = ItemDb.GetItemCategory(validTag);
					int count = starterPackData.items[i].count;
					if (itemCategory == 7 || ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)itemCategory))
					{
						ShopNGUIController.FireWeaponOrArmorBought();
					}
					ShopNGUIController.ProvideItem((ShopNGUIController.CategoryNames)itemCategory, validTag, count, false, 0, null, null, true, true, false);
				}
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
				{
					ShopNGUIController.sharedShop.wearEquipAction(ShopNGUIController.CategoryNames.ArmorCategory, string.Empty, string.Empty);
				}
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.ChooseCategory(ShopNGUIController.sharedShop.CurrentCategory, null, false);
				}
			}
			break;
		case StarterPackModel.TypePack.Coins:
			BankController.AddCoins(starterPackData.coinsCount, true, AnalyticsConstants.AccrualType.Purchased);
			StoreKitEventListener.LogVirtualCurrencyPurchased(productId, starterPackData.coinsCount, false);
			break;
		case StarterPackModel.TypePack.Gems:
			BankController.AddGems(starterPackData.gemsCount, true, AnalyticsConstants.AccrualType.Purchased);
			StoreKitEventListener.LogVirtualCurrencyPurchased(productId, starterPackData.gemsCount, true);
			break;
		}
		Storager.setInt(productId, 1, false);
		this.FinishCurrentStarterPack();
		if (!isRestore)
		{
			AnalyticsStuff.LogSales((!StoreKitEventListener.inAppsReadableNames.ContainsKey(productId)) ? productId : StoreKitEventListener.inAppsReadableNames[productId], "Starter Pack", false);
		}
		return true;
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x00160570 File Offset: 0x0015E770
	public void CheckSendEventChangeEnabled()
	{
		if (StarterPackController.OnStarterPackEnable == null)
		{
			return;
		}
		StarterPackController.OnStarterPackEnable(this.isEventActive);
	}

	// Token: 0x06004254 RID: 16980 RVA: 0x00160590 File Offset: 0x0015E790
	public ItemPrice GetPriceDataForItemsPack()
	{
		StarterPackData currentPackData = this.GetCurrentPackData();
		if (currentPackData == null)
		{
			return null;
		}
		if (currentPackData.coinsCost <= 0 && currentPackData.gemsCost <= 0)
		{
			return null;
		}
		string currency = string.Empty;
		int price = 0;
		if (currentPackData.coinsCost > 0)
		{
			currency = "Coins";
			price = currentPackData.coinsCost;
		}
		else if (currentPackData.gemsCost > 0)
		{
			currency = "GemsCurrency";
			price = currentPackData.gemsCost;
		}
		return new ItemPrice(price, currency);
	}

	// Token: 0x06004255 RID: 16981 RVA: 0x0016060C File Offset: 0x0015E80C
	public bool IsPackSellForGameMoney()
	{
		StarterPackModel.TypeCost typeCostPack = this.GetTypeCostPack();
		return typeCostPack == StarterPackModel.TypeCost.Gems || typeCostPack == StarterPackModel.TypeCost.Money;
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x00160630 File Offset: 0x0015E830
	public void CheckBuyPackForGameMoney(StarterPackView view)
	{
		ItemPrice priceDataForItemsPack = this.GetPriceDataForItemsPack();
		if (priceDataForItemsPack == null)
		{
			return;
		}
		ShopNGUIController.TryToBuy(view.gameObject, priceDataForItemsPack, delegate
		{
			string storageIdByPackOrder = this.GetStorageIdByPackOrder(this._orderCurrentPack);
			this.TryTakePurchasesForCurrentPack(storageIdByPackOrder, false);
			view.HideWindow();
		}, null, null, null, null, null);
	}

	// Token: 0x06004257 RID: 16983 RVA: 0x00160684 File Offset: 0x0015E884
	private StarterPackModel.TypeCost GetTypeCostPack()
	{
		StarterPackData currentPackData = this.GetCurrentPackData();
		if (currentPackData == null)
		{
			return StarterPackModel.TypeCost.None;
		}
		if (currentPackData.coinsCost > 0)
		{
			return StarterPackModel.TypeCost.Money;
		}
		if (currentPackData.gemsCost > 0)
		{
			return StarterPackModel.TypeCost.Gems;
		}
		return StarterPackModel.TypeCost.InApp;
	}

	// Token: 0x06004258 RID: 16984 RVA: 0x001606C0 File Offset: 0x0015E8C0
	public void CheckBuyRealMoney()
	{
		if (this._orderCurrentPack >= StoreKitEventListener.starterPackIds.Length)
		{
			Debug.Log("Not purchase data for starter pack number: " + this._orderCurrentPack);
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		StoreKitEventListener.purchaseInProcess = true;
		string text = StoreKitEventListener.starterPackIds[this._orderCurrentPack];
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			SkuInput skuInput = new SkuInput
			{
				Sku = text
			};
			Debug.Log("Amazon Purchase (StarterPackController.CheckBuyMoney): " + skuInput.ToJson());
			AmazonIapV2Impl.Instance.Purchase(skuInput);
		}
		else
		{
			AnalyticsFacade.SendCustomEventToAppsFlyer("af_initiated_checkout", new Dictionary<string, string>
			{
				{
					"af_content_id",
					text
				}
			});
			GoogleIAB.purchaseProduct(text);
		}
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
	}

	// Token: 0x06004259 RID: 16985 RVA: 0x00160788 File Offset: 0x0015E988
	private int GetOrderPackByProductId(string productId)
	{
		if (!StoreKitEventListener.starterPackIds.Contains(productId))
		{
			return -1;
		}
		return Array.IndexOf<string>(StoreKitEventListener.starterPackIds, productId);
	}

	// Token: 0x0600425A RID: 16986 RVA: 0x001607A8 File Offset: 0x0015E9A8
	private string GetStorageIdByPackOrder(int packOrder)
	{
		if (packOrder < 0 || packOrder >= StoreKitEventListener.starterPackIds.Length)
		{
			return string.Empty;
		}
		return StoreKitEventListener.starterPackIds[packOrder];
	}

	// Token: 0x0600425B RID: 16987 RVA: 0x001607CC File Offset: 0x0015E9CC
	private IEnumerator TryRestoreStarterPackByProductId(string productId)
	{
		if (Application.loadedLevelName == "Loading")
		{
			yield break;
		}
		if (!StoreKitEventListener.starterPackIds.Contains(productId))
		{
			yield break;
		}
		if (this.IsStarterPackBuy(productId))
		{
			yield break;
		}
		if (this._starterPacksData.Count == 0)
		{
			yield return base.StartCoroutine(this.DownloadDataAboutEvent());
		}
		int packOrder = this.GetOrderPackByProductId(productId);
		if (this.IsInvalidPack(packOrder))
		{
			yield break;
		}
		StarterPackModel.TypePack packType = this.GetPackType(packOrder);
		if (packType != StarterPackModel.TypePack.Items)
		{
			yield break;
		}
		if (this.TryTakePurchases(productId, packOrder, true))
		{
			FyberFacade.Instance.SetUserPaying("1");
			StoreKitEventListener.SetLastPaymentTime();
		}
		yield break;
	}

	// Token: 0x0600425C RID: 16988 RVA: 0x001607F8 File Offset: 0x0015E9F8
	public void TryRestoreStarterPack(string productId)
	{
		base.StartCoroutine(this.TryRestoreStarterPackByProductId(productId));
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x00160808 File Offset: 0x0015EA08
	public string GetTimeToEndEvent()
	{
		if (!this.isEventActive)
		{
			return string.Empty;
		}
		this._timeToEndEvent = StarterPackModel.MaxLiveTimeEvent - this._timeLiveEvent;
		return string.Format("{0:00}:{1:00}:{2:00}", this._timeToEndEvent.Hours, this._timeToEndEvent.Minutes, this._timeToEndEvent.Seconds);
	}

	// Token: 0x0600425E RID: 16990 RVA: 0x00160878 File Offset: 0x0015EA78
	public string GetPriceLabelForCurrentPack()
	{
		if (this._orderCurrentPack >= StoreKitEventListener.starterPackIds.Length)
		{
			return string.Empty;
		}
		if (Application.isEditor)
		{
			return string.Format("{0}$", VirtualCurrencyHelper.starterPackFakePrice[this._orderCurrentPack]);
		}
		string productId = StoreKitEventListener.starterPackIds[this._orderCurrentPack];
		IMarketProduct marketProduct = this._storeKitEventListener.Products.FirstOrDefault((IMarketProduct p) => p.Id == productId);
		if (marketProduct != null)
		{
			return marketProduct.Price;
		}
		Debug.LogWarning("marketProduct == null,    id: " + productId);
		return string.Empty;
	}

	// Token: 0x0600425F RID: 16991 RVA: 0x00160920 File Offset: 0x0015EB20
	public string GetCurrentPackName()
	{
		if (this._orderCurrentPack == -1)
		{
			return string.Empty;
		}
		if (this._orderCurrentPack >= StarterPackModel.packNameLocalizeKey.Length)
		{
			return string.Empty;
		}
		string term = StarterPackModel.packNameLocalizeKey[this._orderCurrentPack];
		return LocalizationStore.Get(term);
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x0016096C File Offset: 0x0015EB6C
	public Texture2D GetCurrentPackImage()
	{
		StarterPackModel.TypePack currentPackType = this.GetCurrentPackType();
		string text = string.Empty;
		switch (currentPackType)
		{
		case StarterPackModel.TypePack.Items:
			text = "Textures/Bank/StarterPack_Weapon";
			break;
		case StarterPackModel.TypePack.Coins:
			text = "Textures/Bank/Coins_Shop_5";
			break;
		case StarterPackModel.TypePack.Gems:
			text = "Textures/Bank/Coins_Shop_Gem_5";
			break;
		}
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return Resources.Load<Texture2D>(text);
	}

	// Token: 0x06004261 RID: 16993 RVA: 0x001609D4 File Offset: 0x0015EBD4
	public void UpdateCountShownWindowByShowCondition()
	{
		if (PlayerPrefs.GetInt("CountShownStarterPack", 1) == 0)
		{
			return;
		}
		PlayerPrefs.SetString("LastTimeShowStarterPack", DateTime.UtcNow.ToString("s"));
		int @int = PlayerPrefs.GetInt("CountShownStarterPack", 1);
		PlayerPrefs.SetInt("CountShownStarterPack", @int - 1);
		PlayerPrefs.Save();
	}

	// Token: 0x06004262 RID: 16994 RVA: 0x00160A30 File Offset: 0x0015EC30
	public bool IsNeedShowEventWindow()
	{
		int @int = PlayerPrefs.GetInt("CountShownStarterPack", 1);
		return this.isEventActive && @int > 0 && SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene);
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x00160A70 File Offset: 0x0015EC70
	public void UpdateCountShownWindowByTimeCondition()
	{
		string @string = PlayerPrefs.GetString("LastTimeShowStarterPack", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		DateTime d = default(DateTime);
		if (!DateTime.TryParse(@string, out d))
		{
			return;
		}
		if (DateTime.UtcNow - d >= StarterPackModel.TimeOutShownWindow)
		{
			PlayerPrefs.SetInt("CountShownStarterPack", 1);
		}
	}

	// Token: 0x06004264 RID: 16996 RVA: 0x00160AD4 File Offset: 0x0015ECD4
	public string GetSavingMoneyByCarrentPack()
	{
		int orderCurrentPack = this.GetOrderCurrentPack();
		if (orderCurrentPack == -1)
		{
			return string.Empty;
		}
		if (orderCurrentPack >= StarterPackModel.savingMoneyForBuyPack.Length)
		{
			return string.Empty;
		}
		if (this.IsPackSellForGameMoney())
		{
			return string.Empty;
		}
		return string.Format("{0} {1}$", LocalizationStore.Get("Key_1047"), StarterPackModel.savingMoneyForBuyPack[orderCurrentPack]);
	}

	// Token: 0x06004265 RID: 16997 RVA: 0x00160B3C File Offset: 0x0015ED3C
	public void AddBuyingStarterPack(List<string> buyingList, string starterPackId)
	{
		if (buyingList.Contains(starterPackId))
		{
			return;
		}
		buyingList.Add(starterPackId);
	}

	// Token: 0x06004266 RID: 16998 RVA: 0x00160B54 File Offset: 0x0015ED54
	public void RestoreBuyingStarterPack(List<string> buyingList)
	{
		for (int i = 0; i < buyingList.Count; i++)
		{
			string productId = buyingList[i];
			this.TryRestoreStarterPack(productId);
		}
	}

	// Token: 0x06004267 RID: 16999 RVA: 0x00160B88 File Offset: 0x0015ED88
	private bool IsStarterPackBuying(List<string> buyingList, int orderPack)
	{
		string storageIdByPackOrder = this.GetStorageIdByPackOrder(orderPack);
		return !string.IsNullOrEmpty(storageIdByPackOrder) && buyingList.Contains(storageIdByPackOrder);
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x00160BB4 File Offset: 0x0015EDB4
	public void AddBuyAndroidStarterPack(string starterPackId)
	{
		this.AddBuyingStarterPack(this.BuyAnroidStarterPack, starterPackId);
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x00160BC4 File Offset: 0x0015EDC4
	public void RestoreStarterPackForAmazon()
	{
		this.RestoreBuyingStarterPack(this.BuyAnroidStarterPack);
	}

	// Token: 0x0600426A RID: 17002 RVA: 0x00160BD4 File Offset: 0x0015EDD4
	private bool IsStarterPackBuyOnAndroid(int orderPack)
	{
		return this.IsStarterPackBuying(this.BuyAnroidStarterPack, orderPack);
	}

	// Token: 0x0600426B RID: 17003 RVA: 0x00160BE4 File Offset: 0x0015EDE4
	public void ClearAllGooglePurchases()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
		{
			return;
		}
		if (this.BuyAnroidStarterPack.Count == 0)
		{
			return;
		}
		GoogleIAB.consumeProducts(this.BuyAnroidStarterPack.ToArray());
	}

	// Token: 0x04003068 RID: 12392
	private DateTime _timeStartEvent;

	// Token: 0x04003069 RID: 12393
	private TimeSpan _timeLiveEvent;

	// Token: 0x0400306A RID: 12394
	private TimeSpan _timeToEndEvent;

	// Token: 0x0400306B RID: 12395
	private List<StarterPackData> _starterPacksData;

	// Token: 0x0400306C RID: 12396
	private int _orderCurrentPack;

	// Token: 0x0400306D RID: 12397
	private bool _isDownloadDataRun;

	// Token: 0x0400306E RID: 12398
	private StoreKitEventListener _storeKitEventListener;

	// Token: 0x0400306F RID: 12399
	private float _lastCheckEventTime;

	// Token: 0x04003070 RID: 12400
	private float timeUpdateConfig = -3600f;

	// Token: 0x02000925 RID: 2341
	// (Invoke) Token: 0x06005134 RID: 20788
	public delegate void OnStarterPackEnableDelegate(bool enable);
}
