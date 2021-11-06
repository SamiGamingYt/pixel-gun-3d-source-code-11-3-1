using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200064A RID: 1610
internal sealed class GiftController : MonoBehaviour
{
	// Token: 0x060037C6 RID: 14278 RVA: 0x0011F77C File Offset: 0x0011D97C
	// Note: this type is marked as 'beforefieldinit'.
	static GiftController()
	{
		GiftController.OnChangeSlots = delegate()
		{
		};
		GiftController.OnTimerEnded = delegate()
		{
		};
		GiftController.OnUpdateTimer = delegate(string s)
		{
		};
	}

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x060037C7 RID: 14279 RVA: 0x0011F7F0 File Offset: 0x0011D9F0
	// (remove) Token: 0x060037C8 RID: 14280 RVA: 0x0011F808 File Offset: 0x0011DA08
	public static event Action OnChangeSlots;

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x060037C9 RID: 14281 RVA: 0x0011F820 File Offset: 0x0011DA20
	// (remove) Token: 0x060037CA RID: 14282 RVA: 0x0011F838 File Offset: 0x0011DA38
	public static event Action OnTimerEnded;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x060037CB RID: 14283 RVA: 0x0011F850 File Offset: 0x0011DA50
	// (remove) Token: 0x060037CC RID: 14284 RVA: 0x0011F868 File Offset: 0x0011DA68
	public static event Action<string> OnUpdateTimer;

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x060037CD RID: 14285 RVA: 0x0011F880 File Offset: 0x0011DA80
	private static string UrlForLoadData
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x060037CE RID: 14286 RVA: 0x0011F8FC File Offset: 0x0011DAFC
	public float TimeLeft
	{
		get
		{
			return this._localTimer;
		}
	}

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x060037CF RID: 14287 RVA: 0x0011F904 File Offset: 0x0011DB04
	public List<SlotInfo> Slots
	{
		get
		{
			return this._slots;
		}
	}

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x060037D0 RID: 14288 RVA: 0x0011F90C File Offset: 0x0011DB0C
	public bool CanGetTimerGift
	{
		get
		{
			return this.ActiveGift && this._canGetTimerGift;
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x060037D1 RID: 14289 RVA: 0x0011F924 File Offset: 0x0011DB24
	public bool CanGetFreeSpinGift
	{
		get
		{
			return this.ActiveGift && this.FreeSpins > 0;
		}
	}

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x060037D2 RID: 14290 RVA: 0x0011F940 File Offset: 0x0011DB40
	public bool CanGetGift
	{
		get
		{
			return this.CanGetTimerGift || this.CanGetFreeSpinGift;
		}
	}

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x060037D3 RID: 14291 RVA: 0x0011F958 File Offset: 0x0011DB58
	public bool ActiveGift
	{
		get
		{
			return this._cfgGachaIsActive && this.DataIsLoaded && FriendsController.ServerTime >= 0L;
		}
	}

	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x060037D4 RID: 14292 RVA: 0x0011F980 File Offset: 0x0011DB80
	public bool DataIsLoaded
	{
		get
		{
			return this._slots != null && this._slots.Count != 0;
		}
	}

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x060037D5 RID: 14293 RVA: 0x0011F9B0 File Offset: 0x0011DBB0
	public static Dictionary<int, List<ItemRecord>> GrayCategoryWeapons
	{
		get
		{
			if (GiftController._grayCategoryWeapons == null)
			{
				GiftController._grayCategoryWeapons = new Dictionary<int, List<ItemRecord>>();
				GiftController._grayCategoryWeapons.Add(0, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon10"),
					ItemDb.GetByPrefabName("Weapon44"),
					ItemDb.GetByPrefabName("Weapon79")
				});
				GiftController._grayCategoryWeapons.Add(1, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon278"),
					ItemDb.GetByPrefabName("Weapon65"),
					ItemDb.GetByPrefabName("Weapon286")
				});
				GiftController._grayCategoryWeapons.Add(2, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon252"),
					ItemDb.GetByPrefabName("Weapon258"),
					ItemDb.GetByPrefabName("Weapon48"),
					ItemDb.GetByPrefabName("Weapon253")
				});
				GiftController._grayCategoryWeapons.Add(3, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon257"),
					ItemDb.GetByPrefabName("Weapon262"),
					ItemDb.GetByPrefabName("Weapon251")
				});
				GiftController._grayCategoryWeapons.Add(4, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon330"),
					ItemDb.GetByPrefabName("Weapon308")
				});
				GiftController._grayCategoryWeapons.Add(5, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon222")
				});
			}
			return GiftController._grayCategoryWeapons;
		}
	}

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x060037D6 RID: 14294 RVA: 0x0011FB44 File Offset: 0x0011DD44
	public int FreeSpins
	{
		get
		{
			return this._freeSpins.Value;
		}
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0011FB54 File Offset: 0x0011DD54
	internal int IncrementFreeSpins()
	{
		int num = this.FreeSpins + 1;
		this._freeSpins.Value = num;
		Storager.setInt("freeSpinsCount", this._freeSpins.Value, false);
		return num;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x0011FB90 File Offset: 0x0011DD90
	private void Awake()
	{
		GiftController.Instance = this;
		this._freeSpins.Value = Storager.getInt("freeSpinsCount", false);
		this._localTimer = -1f;
		this._categories.Clear();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Storager.hasKey("SaveServerTime"))
		{
			Storager.setInt("keyCountGiftNewPlayer", 2, false);
		}
		if (!Storager.hasKey("keyCountGiftNewPlayer"))
		{
			Storager.setInt("keyCountGiftNewPlayer", 0, false);
		}
		if (!Storager.hasKey("keyIsGetArmorNewPlayer"))
		{
			Storager.setInt("keyIsGetArmorNewPlayer", 0, false);
		}
		if (!Storager.hasKey("keyIsGetSkinNewPlayer"))
		{
			Storager.setInt("keyIsGetSkinNewPlayer", 0, false);
		}
		Storager.getInt("keyCountGiftNewPlayer", false);
		base.StartCoroutine(this.GetDataFromServerLoop());
		FriendsController.ServerTimeUpdated += this.OnUpdateTimeFromServer;
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0011FC70 File Offset: 0x0011DE70
	private void OnDestroy()
	{
		FriendsController.ServerTimeUpdated -= this.OnUpdateTimeFromServer;
		GiftController.Instance = null;
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x0011FC8C File Offset: 0x0011DE8C
	private void Update()
	{
		if (this._localTimer > 0f)
		{
			this._localTimer -= Time.deltaTime;
			if (this._localTimer < 0f)
			{
				this._localTimer = 0f;
			}
			this._canGetTimerGift = false;
			if (this._oldTime == (int)this._localTimer)
			{
				return;
			}
			this._oldTime = (int)this._localTimer;
			if (GiftController.OnUpdateTimer != null)
			{
				GiftController.OnUpdateTimer(this.GetStringTimer());
			}
		}
		else
		{
			if (this._canGetTimerGift || (int)this._localTimer != 0)
			{
				return;
			}
			this._localTimer = -1f;
			this._canGetTimerGift = true;
			if (GiftController.OnUpdateTimer != null)
			{
				GiftController.OnUpdateTimer(this.GetStringTimer());
			}
			if (GiftController.OnTimerEnded != null)
			{
				GiftController.OnTimerEnded();
			}
		}
	}

	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x060037DB RID: 14299 RVA: 0x0011FD74 File Offset: 0x0011DF74
	// (set) Token: 0x060037DC RID: 14300 RVA: 0x0011FD84 File Offset: 0x0011DF84
	public static int CountGetGiftForNewPlayer
	{
		get
		{
			return Storager.getInt("keyCountGiftNewPlayer", false);
		}
		set
		{
			if (value >= 0 && value < GiftController.CountGetGiftForNewPlayer)
			{
				Storager.setInt("keyCountGiftNewPlayer", value, false);
			}
		}
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0011FDA4 File Offset: 0x0011DFA4
	public void SetTimer(int val)
	{
		if (val > 14400)
		{
			val = 14400;
		}
		if (val == 0)
		{
			this.LastTimeGetGift = FriendsController.ServerTime - 14400L + 1L;
		}
		else
		{
			long lastTimeGetGift = FriendsController.ServerTime - (long)(14400 - val);
			this.LastTimeGetGift = lastTimeGetGift;
		}
		this.OnUpdateTimeFromServer();
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0011FE00 File Offset: 0x0011E000
	private GiftCategoryType ParseToEnum(string typeCat)
	{
		GiftCategoryType? giftCategoryType = typeCat.ToEnum(null);
		return (giftCategoryType == null) ? GiftCategoryType.none : giftCategoryType.Value;
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0011FE38 File Offset: 0x0011E038
	public void SetGifts()
	{
		if (this._cfgGachaIsActive)
		{
			if (this._categories != null && this._categories.Count > 0)
			{
				base.StartCoroutine(this.CheckAvailableGifts());
			}
		}
		else
		{
			this._categories.Clear();
			this._slots.Clear();
			if (GiftController.OnChangeSlots != null)
			{
				GiftController.OnChangeSlots();
			}
		}
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0011FEA8 File Offset: 0x0011E0A8
	private void RecreateSlots()
	{
		if (!this._kAlreadyGenerateSlot && this._cfgGachaIsActive)
		{
			this._kAlreadyGenerateSlot = true;
			this._slots.Clear();
			List<GiftCategory> categories = this._categories;
			foreach (GiftCategory giftCategory in categories)
			{
				giftCategory.CheckGifts();
				if (giftCategory.AvaliableGiftsCount >= 1)
				{
					SlotInfo slotInfo = new SlotInfo();
					slotInfo.category = giftCategory;
					slotInfo.gift = giftCategory.GetRandomGift();
					if (slotInfo.gift != null && !string.IsNullOrEmpty(slotInfo.gift.Id))
					{
						slotInfo.percentGetSlot = giftCategory.PercentChance;
						slotInfo.positionInScroll = giftCategory.ScrollPosition;
						slotInfo.isActiveEvent = false;
						if (GiftController.CountGetGiftForNewPlayer > 0)
						{
							this.SetPerGetGiftForNewPlayer(slotInfo);
						}
						this._slots.Add(slotInfo);
					}
				}
			}
			GiftCategoryType? prevDroppedCategoryType = this._prevDroppedCategoryType;
			if (prevDroppedCategoryType != null)
			{
				SlotInfo slotInfo2 = this._slots.FirstOrDefault((SlotInfo s) => s.category.Type == this._prevDroppedCategoryType);
				if (slotInfo2 != null)
				{
					slotInfo2.NoDropped = true;
				}
			}
			this._slots.Sort(delegate(SlotInfo left, SlotInfo right)
			{
				if (left == null && right == null)
				{
					return 0;
				}
				if (left == null)
				{
					return -1;
				}
				if (right == null)
				{
					return 1;
				}
				return left.positionInScroll.CompareTo(right.positionInScroll);
			});
			if (GiftController.OnChangeSlots != null)
			{
				GiftController.OnChangeSlots();
			}
			this.OnUpdateTimeFromServer();
		}
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x00120044 File Offset: 0x0011E244
	private IEnumerator WaitDrop(GiftCategory cat, string id, bool isContains = false)
	{
		bool lk = true;
		int iter = 0;
		GiftInfo gift = null;
		while (lk)
		{
			iter++;
			gift = cat.GetRandomGift();
			bool finish = (!isContains) ? (gift.Id == id) : gift.Id.Contains(id);
			if (finish)
			{
				lk = false;
				Debug.Log(string.Format("[TTT] found '{0}' iterations count: {1}", gift.Id, iter));
			}
			if (iter > 100)
			{
				Debug.Log("[TTT] stop waiting");
				lk = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x00120084 File Offset: 0x0011E284
	public GiftNewPlayerInfo GetInfoNewPlayer(GiftCategoryType needCat)
	{
		return this._forNewPlayer.Find((GiftNewPlayerInfo val) => val.TypeCategory == needCat);
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x001200B8 File Offset: 0x0011E2B8
	private void SetPerGetGiftForNewPlayer(SlotInfo curSlot)
	{
		float percentGetSlot = 0f;
		int value = curSlot.gift.Count.Value;
		curSlot.isActiveEvent = true;
		GiftNewPlayerInfo infoNewPlayer = this.GetInfoNewPlayer(curSlot.category.Type);
		if (infoNewPlayer != null)
		{
			value = infoNewPlayer.Count.Value;
			if (curSlot.category.Type == GiftCategoryType.ArmorAndHat && Storager.getInt("keyIsGetArmorNewPlayer", false) == 0)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Skins && Storager.getInt("keyIsGetSkinNewPlayer", false) == 0)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Coins)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Gems)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
		}
		curSlot.percentGetSlot = percentGetSlot;
		curSlot.CountGift = value;
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x00120198 File Offset: 0x0011E398
	public void UpdateSlot(SlotInfo curSlot)
	{
		curSlot.category.CheckGifts();
		curSlot.gift = curSlot.category.GetRandomGift();
		if (curSlot.gift == null)
		{
			this._slots.Remove(curSlot);
		}
		else
		{
			curSlot.percentGetSlot = curSlot.category.PercentChance;
			curSlot.positionInScroll = curSlot.category.ScrollPosition;
		}
		SlotInfo slot;
		foreach (SlotInfo slot2 in this._slots)
		{
			slot = slot2;
			GiftCategory giftCategory = this._categories.FirstOrDefault((GiftCategory c) => c == slot.category);
			if (GiftController.CountGetGiftForNewPlayer > 0)
			{
				this.SetPerGetGiftForNewPlayer(slot);
			}
			else
			{
				slot.percentGetSlot = slot.category.PercentChance;
			}
		}
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x001202AC File Offset: 0x0011E4AC
	public void ReCreateSlots()
	{
		this._kAlreadyGenerateSlot = false;
		this.SetGifts();
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x001202BC File Offset: 0x0011E4BC
	public SlotInfo GetRandomSlot()
	{
		return null;
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x001202C0 File Offset: 0x0011E4C0
	private IEnumerator GetDataFromServerLoop()
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.DownloadDataFormServer());
			yield return new WaitForSeconds(870f);
		}
		yield break;
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x001202DC File Offset: 0x0011E4DC
	private IEnumerator DownloadDataFormServer()
	{
		if (this._kDataLoading)
		{
			yield break;
		}
		this._kDataLoading = true;
		string urlDataAddress = GiftController.UrlForLoadData;
		WWW downloadData = null;
		for (int iter = 3; iter > 0; iter--)
		{
			downloadData = Tools.CreateWwwIfNotConnected(urlDataAddress);
			if (downloadData == null)
			{
				yield break;
			}
			while (!downloadData.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(downloadData.error))
			{
				break;
			}
			yield return new WaitForSeconds(5f);
		}
		if (downloadData == null || !string.IsNullOrEmpty(downloadData.error))
		{
			if (Defs.IsDeveloperBuild && downloadData != null)
			{
				Debug.LogWarningFormat("Request to {0} failed: {1}", new object[]
				{
					urlDataAddress,
					downloadData.error
				});
			}
			this._kDataLoading = false;
			yield break;
		}
		string responseText = URLs.Sanitize(downloadData);
		Dictionary<string, object> allData = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (allData == null)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("Bad response: " + responseText);
			}
			this._kDataLoading = false;
			yield break;
		}
		if (allData.ContainsKey("isActive"))
		{
			this._cfgGachaIsActive = Convert.ToBoolean(allData["isActive"], CultureInfo.InvariantCulture);
			if (!this._cfgGachaIsActive)
			{
				this._kDataLoading = false;
				this.OnDataLoaded();
				yield break;
			}
		}
		if (allData.ContainsKey("price"))
		{
			this.CostBuyCanGetGift.Value = Convert.ToInt32(allData["price"], CultureInfo.InvariantCulture);
		}
		this._forNewPlayer.Clear();
		if (allData.ContainsKey("newPlayerEvent"))
		{
			List<object> listAllParametrNewPlayer = allData["newPlayerEvent"] as List<object>;
			if (listAllParametrNewPlayer != null)
			{
				for (int iTG = 0; iTG < listAllParametrNewPlayer.Count; iTG++)
				{
					Dictionary<string, object> curParametr = listAllParametrNewPlayer[iTG] as Dictionary<string, object>;
					GiftNewPlayerInfo curAddInfo = new GiftNewPlayerInfo();
					if (curParametr.ContainsKey("typeCategory"))
					{
						curAddInfo.TypeCategory = this.ParseToEnum(curParametr["typeCategory"].ToString());
						if (curParametr.ContainsKey("count"))
						{
							curAddInfo.Count.Value = int.Parse(curParametr["count"].ToString());
						}
						if (curParametr.ContainsKey("percent"))
						{
							object curPercentObject = curParametr["percent"];
							curAddInfo.Percent = (float)Convert.ToDouble(curPercentObject, CultureInfo.InvariantCulture);
						}
						this._forNewPlayer.Add(curAddInfo);
					}
				}
			}
		}
		this._categories.Clear();
		if (allData.ContainsKey("categories"))
		{
			List<object> listCategories = allData["categories"] as List<object>;
			if (listCategories != null)
			{
				for (int iC = 0; iC < listCategories.Count; iC++)
				{
					Dictionary<string, object> infoCategory = listCategories[iC] as Dictionary<string, object>;
					if (infoCategory != null)
					{
						GiftCategory newCategory = new GiftCategory();
						if (infoCategory.ContainsKey("typeCategory"))
						{
							newCategory.Type = this.ParseToEnum(infoCategory["typeCategory"].ToString());
							if (infoCategory.ContainsKey("posInScroll"))
							{
								newCategory.ScrollPosition = int.Parse(infoCategory["posInScroll"].ToString());
							}
							if (infoCategory.ContainsKey("gifts"))
							{
								if (infoCategory.ContainsKey("keyTransInfo"))
								{
									newCategory.KeyTranslateInfoCommon = infoCategory["keyTransInfo"].ToString();
								}
								List<object> gifts = infoCategory["gifts"] as List<object>;
								if (gifts != null)
								{
									for (int iG = 0; iG < gifts.Count; iG++)
									{
										Dictionary<string, object> infoGift = gifts[iG] as Dictionary<string, object>;
										if (infoGift != null)
										{
											GiftInfo newGiftInfo = new GiftInfo();
											GiftCategoryType type = newCategory.Type;
											if (type != GiftCategoryType.Coins)
											{
												if (type != GiftCategoryType.Gems)
												{
													if (infoGift.ContainsKey("idGift"))
													{
														newGiftInfo.Id = infoGift["idGift"].ToString();
													}
												}
												else
												{
													newGiftInfo.Id = "Gems";
												}
											}
											else
											{
												newGiftInfo.Id = "Coins";
											}
											if (infoGift.ContainsKey("count"))
											{
												newGiftInfo.Count.Value = int.Parse(infoGift["count"].ToString());
											}
											if (infoGift.ContainsKey("percent"))
											{
												object percentObject = infoGift["percent"];
												newGiftInfo.PercentAddInSlot = (float)Convert.ToDouble(percentObject, CultureInfo.InvariantCulture);
											}
											if (infoGift.ContainsKey("keyTransInfo"))
											{
												newGiftInfo.KeyTranslateInfo = infoGift["keyTransInfo"].ToString();
											}
											if (newGiftInfo.Count.Value == 0)
											{
												newGiftInfo.Count.Value = 1;
											}
											newCategory.AddGift(newGiftInfo);
										}
									}
								}
								if (newCategory.AnyGifts)
								{
									this._categories.Add(newCategory);
								}
							}
						}
					}
				}
			}
		}
		this.OnDataLoaded();
		this._kDataLoading = false;
		yield break;
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x001202F8 File Offset: 0x0011E4F8
	private void OnDataLoaded()
	{
		this.SetGifts();
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x00120300 File Offset: 0x0011E500
	public SlotInfo GetGift(bool ignoreAvailabilityCheck = false)
	{
		if (!ignoreAvailabilityCheck)
		{
			if (this.CanGetTimerGift)
			{
				this._canGetTimerGift = false;
				this._localTimer = -1f;
				this.ReSaveLastTimeSever();
			}
			else
			{
				if (this.FreeSpins <= 0)
				{
					return null;
				}
				this._freeSpins.Value = this._freeSpins.Value - 1;
				Storager.setInt("freeSpinsCount", this._freeSpins.Value, false);
			}
		}
		List<SlotInfo> list = (from s in this._slots
		where !s.NoDropped
		select s).ToList<SlotInfo>();
		float max = list.Sum((SlotInfo s) => s.percentGetSlot);
		float num = UnityEngine.Random.Range(0f, max);
		float num2 = 0f;
		SlotInfo slotInfo = null;
		for (int i = 0; i < list.Count; i++)
		{
			SlotInfo slotInfo2 = list[i];
			num2 += slotInfo2.percentGetSlot;
			if (num <= num2)
			{
				slotInfo = slotInfo2;
				slotInfo.numInScroll = this._slots.IndexOf(slotInfo2);
				break;
			}
		}
		if (slotInfo != null)
		{
			GiftController.CountGetGiftForNewPlayer--;
			this.GiveProductForSlot(slotInfo);
		}
		slotInfo.NoDropped = true;
		this._prevDroppedCategoryType = new GiftCategoryType?(slotInfo.category.Type);
		return slotInfo;
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x00120470 File Offset: 0x0011E670
	public void CheckAvaliableSlots()
	{
		bool flag = false;
		for (int i = this._slots.Count - 1; i >= 0; i--)
		{
			SlotInfo slotInfo = this._slots[i];
			if (slotInfo == null)
			{
				this._slots.RemoveAt(i);
			}
			else
			{
				bool flag2 = slotInfo.CheckAvaliableGift();
				if (flag2)
				{
					flag = true;
				}
				if (slotInfo.gift == null)
				{
					this._slots.RemoveAt(i);
				}
			}
		}
		if (flag && GiftController.OnChangeSlots != null)
		{
			GiftController.OnChangeSlots();
		}
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00120504 File Offset: 0x0011E704
	public void GiveProductForSlot(SlotInfo curSlot)
	{
		if (curSlot != null)
		{
			switch (curSlot.category.Type)
			{
			case GiftCategoryType.Coins:
				BankController.AddCoins(curSlot.CountGift, false, AnalyticsConstants.AccrualType.Earned);
				base.StartCoroutine(BankController.WaitForIndicationGems(false));
				break;
			case GiftCategoryType.Gems:
				BankController.AddGems(curSlot.CountGift, false, AnalyticsConstants.AccrualType.Earned);
				base.StartCoroutine(BankController.WaitForIndicationGems(true));
				break;
			case GiftCategoryType.Grenades:
			{
				int @int = Storager.getInt(curSlot.gift.Id, false);
				Storager.setInt(curSlot.gift.Id, @int + curSlot.gift.Count.Value, false);
				break;
			}
			case GiftCategoryType.Gear:
			{
				int int2 = Storager.getInt(curSlot.gift.Id, false);
				Storager.setInt(curSlot.gift.Id, int2 + curSlot.gift.Count.Value, false);
				break;
			}
			case GiftCategoryType.Skins:
				Storager.setInt("keyIsGetSkinNewPlayer", 1, false);
				ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.SkinsCategory, curSlot.gift.Id, 1, false, 0, null, null, false, true, false);
				break;
			case GiftCategoryType.ArmorAndHat:
				Storager.setInt("keyIsGetArmorNewPlayer", 1, false);
				if (curSlot.gift.TypeShopCat == ShopNGUIController.CategoryNames.ArmorCategory)
				{
					ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, curSlot.gift.Id, 1, false, 0, null, null, true, true, false);
					if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
					{
						ShopNGUIController.sharedShop.wearEquipAction(ShopNGUIController.CategoryNames.ArmorCategory, string.Empty, string.Empty);
					}
				}
				break;
			case GiftCategoryType.Wear:
				ShopNGUIController.ProvideItem(curSlot.gift.TypeShopCat.Value, curSlot.gift.Id, 1, false, 0, null, null, true, true, false);
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
				{
					ShopNGUIController.sharedShop.wearEquipAction(curSlot.gift.TypeShopCat.Value, string.Empty, string.Empty);
				}
				break;
			case GiftCategoryType.Editor:
				if (curSlot.gift.Id == "editor_Cape")
				{
					this.GiveProduct(ShopNGUIController.CategoryNames.CapesCategory, "cape_Custom", true);
				}
				else if (curSlot.gift.Id == "editor_Skin")
				{
					Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
				}
				else
				{
					Debug.LogError(string.Format("[GIFT] unknown editor id: '{0}'", curSlot.gift.Id));
				}
				break;
			case GiftCategoryType.Masks:
				this.GiveProduct(ShopNGUIController.CategoryNames.MaskCategory, curSlot.gift.Id, true);
				break;
			case GiftCategoryType.Capes:
				this.GiveProduct(ShopNGUIController.CategoryNames.CapesCategory, curSlot.gift.Id, true);
				break;
			case GiftCategoryType.Boots:
				this.GiveProduct(ShopNGUIController.CategoryNames.BootsCategory, curSlot.gift.Id, true);
				break;
			case GiftCategoryType.Hats_random:
				this.GiveProduct(ShopNGUIController.CategoryNames.HatsCategory, curSlot.gift.Id, true);
				break;
			case GiftCategoryType.Gun1:
			case GiftCategoryType.Gun2:
			case GiftCategoryType.Gun3:
			case GiftCategoryType.Gun4:
			case GiftCategoryType.Guns_gray:
				if (WeaponManager.IsExclusiveWeapon(curSlot.gift.Id))
				{
					WeaponManager.ProvideExclusiveWeaponByTag(curSlot.gift.Id);
				}
				else
				{
					this.GiveProduct(curSlot.gift.TypeShopCat.Value, curSlot.gift.Id, true);
				}
				break;
			case GiftCategoryType.Stickers:
			{
				TypePackSticker? typePackSticker = curSlot.gift.Id.ToEnum(null);
				if (typePackSticker == null)
				{
					throw new Exception("sticker id type parse error");
				}
				StickersController.BuyStickersPack(typePackSticker.Value);
				break;
			}
			case GiftCategoryType.Freespins:
				this._freeSpins.Value = this._freeSpins.Value + curSlot.gift.Count.Value;
				Storager.setInt("freeSpinsCount", this._freeSpins.Value, false);
				break;
			case GiftCategoryType.WeaponSkin:
				this.GiveProduct(ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory, curSlot.gift.Id, true);
				break;
			case GiftCategoryType.Gadgets:
				if (GadgetsInfo.info.ContainsKey(curSlot.gift.Id))
				{
					GadgetInfo gadgetInfo = GadgetsInfo.info[curSlot.gift.Id];
					this.GiveProduct((ShopNGUIController.CategoryNames)gadgetInfo.Category, gadgetInfo.Id, true);
				}
				else
				{
					Debug.LogErrorFormat("not found gadget: '{0}'", new object[]
					{
						curSlot.gift.Id
					});
				}
				break;
			}
		}
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x001209A4 File Offset: 0x0011EBA4
	private void GiveProduct(ShopNGUIController.CategoryNames category, string itemId, bool autoEquip = true)
	{
		ShopNGUIController.ProvideItem(category, itemId, 1, false, 0, null, null, true, true, false);
		if (autoEquip && ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(category, string.Empty, string.Empty);
		}
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x00120A00 File Offset: 0x0011EC00
	public static List<string> GetAvailableGrayWeaponsTags()
	{
		int key = ExpController.OurTierForAnyPlace();
		List<ItemRecord> source = GiftController.GrayCategoryWeapons[key];
		return (from w in source
		where Storager.getInt(w.StorageId, true) == 0
		select w.Tag).ToList<string>();
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x00120A6C File Offset: 0x0011EC6C
	private string GetRandomGrayWeapon()
	{
		List<string> availableGrayWeaponsTags = GiftController.GetAvailableGrayWeaponsTags();
		if (!availableGrayWeaponsTags.Any<string>())
		{
			return string.Empty;
		}
		int index = UnityEngine.Random.Range(0, availableGrayWeaponsTags.Count);
		return availableGrayWeaponsTags[index];
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x00120AA4 File Offset: 0x0011ECA4
	private IEnumerator CheckAvailableGifts()
	{
		while (!(WeaponManager.sharedManager != null))
		{
			yield return null;
		}
		this.RecreateSlots();
		yield break;
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x00120AC0 File Offset: 0x0011ECC0
	public void ReSaveLastTimeSever()
	{
		this.LastTimeGetGift = FriendsController.ServerTime;
		this.OnUpdateTimeFromServer();
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x00120AD4 File Offset: 0x0011ECD4
	public string GetStringTimer()
	{
		return RiliExtensions.GetTimeString((long)this._localTimer, ":");
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x060037F3 RID: 14323 RVA: 0x00120AE8 File Offset: 0x0011ECE8
	// (set) Token: 0x060037F4 RID: 14324 RVA: 0x00120B04 File Offset: 0x0011ED04
	private long LastTimeGetGift
	{
		get
		{
			return (long)Storager.getInt("SaveServerTime", false);
		}
		set
		{
			int val = (int)value;
			Storager.setInt("SaveServerTime", val, false);
		}
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x00120B20 File Offset: 0x0011ED20
	private void OnUpdateTimeFromServer()
	{
		if (this._slots.Count == 0)
		{
			base.StartCoroutine(this.DownloadDataFormServer());
			return;
		}
		if (FriendsController.ServerTime < 0L)
		{
			return;
		}
		this._localTimer = -1f;
		this._canGetTimerGift = false;
		if (!Storager.hasKey("SaveServerTime"))
		{
			this.LastTimeGetGift = FriendsController.ServerTime - 14400L + 1L;
		}
		int num = (int)(FriendsController.ServerTime - this.LastTimeGetGift);
		if (num >= 14400)
		{
			this._canGetTimerGift = true;
			if (GiftController.OnTimerEnded != null)
			{
				GiftController.OnTimerEnded();
			}
		}
		else
		{
			this._canGetTimerGift = false;
			this._localTimer = (float)(14400 - num);
		}
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x060037F6 RID: 14326 RVA: 0x00120BDC File Offset: 0x0011EDDC
	internal TimeSpan FreeGachaAvailableIn
	{
		get
		{
			if (!Storager.hasKey("SaveServerTime"))
			{
				this.LastTimeGetGift = FriendsController.ServerTime - 14400L + 1L;
			}
			long num = FriendsController.ServerTime - this.LastTimeGetGift;
			return TimeSpan.FromSeconds((double)(14400L - num));
		}
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x00120C28 File Offset: 0x0011EE28
	public void TryGetData()
	{
		if (!this.DataIsLoaded)
		{
			base.StartCoroutine(this.DownloadDataFormServer());
		}
	}

	// Token: 0x040028A4 RID: 10404
	private const string FREE_SPINS_STORAGER_KEY = "freeSpinsCount";

	// Token: 0x040028A5 RID: 10405
	public const string KEY_COUNT_GIFT_FOR_NEW_PLAYER = "keyCountGiftNewPlayer";

	// Token: 0x040028A6 RID: 10406
	public const string KEY_EDITOR_SKIN = "editor_Skin";

	// Token: 0x040028A7 RID: 10407
	public const string KEY_EDITOR_CAPE = "editor_Cape";

	// Token: 0x040028A8 RID: 10408
	public const string KEY_COLLECTION_GUNS_GRAY = "guns_gray";

	// Token: 0x040028A9 RID: 10409
	public const string KEY_COLLECTION_MASK = "equip_Mask";

	// Token: 0x040028AA RID: 10410
	public const string KEY_COLLECTION_CAPE = "equip_Cape";

	// Token: 0x040028AB RID: 10411
	public const string KEY_COLLECTION_BOOTS = "equip_Boots";

	// Token: 0x040028AC RID: 10412
	public const string KEY_COLLECTION_HAT = "equip_Hat";

	// Token: 0x040028AD RID: 10413
	public const string KEY_COLLECTION_WEAPONSKIN_RANDOM = "random";

	// Token: 0x040028AE RID: 10414
	public const string KEY_COLLECTION_GADGETS_RANDOM = "gadget_random";

	// Token: 0x040028AF RID: 10415
	private const string KEY_FOR_SAVE_SERVER_TIME = "SaveServerTime";

	// Token: 0x040028B0 RID: 10416
	private const string KEY_NEWPLAYER_ARMOR_GETTED = "keyIsGetArmorNewPlayer";

	// Token: 0x040028B1 RID: 10417
	private const string KEY_NEWPLAYER_SKIN_GETTED = "keyIsGetSkinNewPlayer";

	// Token: 0x040028B2 RID: 10418
	private const float UPDATE_DATA_FROM_SERVER_INTERVAL = 870f;

	// Token: 0x040028B3 RID: 10419
	private const int TIME_TO_NEXT_GIFT = 14400;

	// Token: 0x040028B4 RID: 10420
	public static GiftController Instance;

	// Token: 0x040028B5 RID: 10421
	public SaltedInt CostBuyCanGetGift = new SaltedInt(15461355, 0);

	// Token: 0x040028B6 RID: 10422
	private bool _canGetTimerGift;

	// Token: 0x040028B7 RID: 10423
	private float _localTimer = -1f;

	// Token: 0x040028B8 RID: 10424
	private int _oldTime = -1;

	// Token: 0x040028B9 RID: 10425
	[SerializeField]
	[ReadOnly]
	private readonly List<GiftCategory> _categories = new List<GiftCategory>();

	// Token: 0x040028BA RID: 10426
	[ReadOnly]
	[SerializeField]
	private readonly List<SlotInfo> _slots = new List<SlotInfo>();

	// Token: 0x040028BB RID: 10427
	[ReadOnly]
	[SerializeField]
	private readonly List<GiftNewPlayerInfo> _forNewPlayer = new List<GiftNewPlayerInfo>();

	// Token: 0x040028BC RID: 10428
	private bool _cfgGachaIsActive;

	// Token: 0x040028BD RID: 10429
	private static Dictionary<int, List<ItemRecord>> _grayCategoryWeapons;

	// Token: 0x040028BE RID: 10430
	private SaltedInt _freeSpins = new SaltedInt(15461355, 0);

	// Token: 0x040028BF RID: 10431
	private GiftCategoryType? _prevDroppedCategoryType;

	// Token: 0x040028C0 RID: 10432
	private bool _kAlreadyGenerateSlot;

	// Token: 0x040028C1 RID: 10433
	private bool _kDataLoading;
}
