using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000805 RID: 2053
public sealed class TempItemsController : MonoBehaviour
{
	// Token: 0x06004AC6 RID: 19142 RVA: 0x001A8B34 File Offset: 0x001A6D34
	static TempItemsController()
	{
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.Assault_Machine_Gun_Tag, WeaponTags.Assault_Machine_GunBuy_Tag);
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.Impulse_Sniper_Rifle_Tag, WeaponTags.Impulse_Sniper_RifleBuy_Tag);
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.RailRevolver_1_Tag, WeaponTags.RailRevolverBuy_Tag);
		TempItemsController.GunsMappingFromTempToConst.Add(WeaponTags.Autoaim_Rocketlauncher_Tag, WeaponTags.Autoaim_RocketlauncherBuy_Tag);
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x001A8DE0 File Offset: 0x001A6FE0
	public static int RentIndexFromDays(int days)
	{
		int result = 0;
		if (days == 1)
		{
			result = 0;
		}
		else if (days == 2)
		{
			result = 3;
		}
		else if (days == 3)
		{
			result = 1;
		}
		else if (days == 5)
		{
			result = 4;
		}
		else if (days == 7)
		{
			result = 2;
		}
		return result;
	}

	// Token: 0x06004AC8 RID: 19144 RVA: 0x001A8E34 File Offset: 0x001A7034
	public static bool IsCategoryContainsTempItems(ShopNGUIController.CategoryNames cat)
	{
		return ShopNGUIController.IsWeaponCategory(cat) || cat == ShopNGUIController.CategoryNames.ArmorCategory || cat == ShopNGUIController.CategoryNames.HatsCategory;
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x001A8E50 File Offset: 0x001A7050
	public void AddTemporaryItem(string tg, int tm)
	{
		this.AddTimeForItem(tg, (tm < 0) ? 0 : tm);
	}

	// Token: 0x06004ACA RID: 19146 RVA: 0x001A8E68 File Offset: 0x001A7068
	public static int RentTimeForIndex(int timeForRentIndex)
	{
		if (TempItemsController.rentTms == null)
		{
			TempItemsController.rentTms = new List<int>
			{
				86400,
				259200,
				604800,
				172800,
				432000
			};
		}
		int result = 86400;
		if (timeForRentIndex < TempItemsController.rentTms.Count && timeForRentIndex >= 0)
		{
			result = TempItemsController.rentTms[timeForRentIndex];
		}
		return result;
	}

	// Token: 0x06004ACB RID: 19147 RVA: 0x001A8EEC File Offset: 0x001A70EC
	public bool CanShowExpiredBannerForTag(string tg)
	{
		return false;
	}

	// Token: 0x06004ACC RID: 19148 RVA: 0x001A8EFC File Offset: 0x001A70FC
	public long TimeRemainingForItems(string tg)
	{
		return 0L;
	}

	// Token: 0x06004ACD RID: 19149 RVA: 0x001A8F0C File Offset: 0x001A710C
	public string TimeRemainingForItemString(string tg)
	{
		return RiliExtensions.GetTimeStringDays(this.TimeRemainingForItems(tg));
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x001A8F1C File Offset: 0x001A711C
	public void AddTimeForItem(string item, int time)
	{
	}

	// Token: 0x06004ACF RID: 19151 RVA: 0x001A8F2C File Offset: 0x001A712C
	public bool ContainsItem(string item)
	{
		return false;
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x001A8F30 File Offset: 0x001A7130
	private static void PrepareKeyForItemsJson()
	{
		if (!Storager.hasKey(Defs.TempItemsDictionaryKey))
		{
			Storager.setString(Defs.TempItemsDictionaryKey, "{}", false);
		}
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x001A8F54 File Offset: 0x001A7154
	private static bool ItemIsArmorOrHat(string tg)
	{
		int itemCategory = ItemDb.GetItemCategory(tg);
		return itemCategory != -1 && (itemCategory == 7 || itemCategory == 6);
	}

	// Token: 0x06004AD2 RID: 19154 RVA: 0x001A8F84 File Offset: 0x001A7184
	private void Awake()
	{
		TempItemsController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.DeserializeItems();
		this.DeserializeExpiredObjects();
		this.CheckForTimeHack();
		this.RemoveExpiredItems();
	}

	// Token: 0x06004AD3 RID: 19155 RVA: 0x001A8FBC File Offset: 0x001A71BC
	private void Start()
	{
		base.StartCoroutine(this.Step());
	}

	// Token: 0x06004AD4 RID: 19156 RVA: 0x001A8FCC File Offset: 0x001A71CC
	private void RemoveExpiredItems()
	{
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x001A8FDC File Offset: 0x001A71DC
	private void RemoveTemporaryItem(string key)
	{
		bool flag = TempItemsController.ItemIsArmorOrHat(key);
		if (flag)
		{
			Wear.RemoveTemporaryWear(key);
		}
		else
		{
			WeaponManager.sharedManager.RemoveTemporaryItem(key);
		}
	}

	// Token: 0x06004AD6 RID: 19158 RVA: 0x001A900C File Offset: 0x001A720C
	private IEnumerator Step()
	{
		yield break;
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x001A9020 File Offset: 0x001A7220
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

	// Token: 0x06004AD8 RID: 19160 RVA: 0x001A9044 File Offset: 0x001A7244
	private void CheckForTimeHack()
	{
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x001A9054 File Offset: 0x001A7254
	private static Dictionary<string, Dictionary<string, SaltedLong>> ToSaltedDictionary(Dictionary<string, Dictionary<string, long>> normalDict)
	{
		if (normalDict == null)
		{
			return null;
		}
		Dictionary<string, Dictionary<string, SaltedLong>> dictionary = new Dictionary<string, Dictionary<string, SaltedLong>>();
		foreach (KeyValuePair<string, Dictionary<string, long>> keyValuePair in normalDict)
		{
			Dictionary<string, SaltedLong> dictionary2 = new Dictionary<string, SaltedLong>();
			if (keyValuePair.Value != null)
			{
				foreach (KeyValuePair<string, long> keyValuePair2 in keyValuePair.Value)
				{
					if (keyValuePair2.Key != null)
					{
						dictionary2.Add(keyValuePair2.Key, new SaltedLong(1002855644958404316L, keyValuePair2.Value));
					}
				}
			}
			dictionary.Add(keyValuePair.Key, dictionary2);
		}
		return dictionary;
	}

	// Token: 0x06004ADA RID: 19162 RVA: 0x001A9160 File Offset: 0x001A7360
	private static Dictionary<string, Dictionary<string, long>> ToNormalDictionary(Dictionary<string, Dictionary<string, SaltedLong>> saltedDict_)
	{
		if (saltedDict_ == null)
		{
			return null;
		}
		Dictionary<string, Dictionary<string, long>> dictionary = new Dictionary<string, Dictionary<string, long>>();
		foreach (KeyValuePair<string, Dictionary<string, SaltedLong>> keyValuePair in saltedDict_)
		{
			Dictionary<string, long> dictionary2 = new Dictionary<string, long>();
			if (keyValuePair.Value != null)
			{
				foreach (KeyValuePair<string, SaltedLong> keyValuePair2 in keyValuePair.Value)
				{
					if (keyValuePair2.Key != null)
					{
						dictionary2.Add(keyValuePair2.Key, keyValuePair2.Value.Value);
					}
				}
			}
			dictionary.Add(keyValuePair.Key, dictionary2);
		}
		return dictionary;
	}

	// Token: 0x06004ADB RID: 19163 RVA: 0x001A9264 File Offset: 0x001A7464
	private void DeserializeItems()
	{
		TempItemsController.PrepareKeyForItemsJson();
		object obj = Json.Deserialize(Storager.getString(Defs.TempItemsDictionaryKey, false));
		if (obj == null)
		{
			Debug.LogWarning("Error Deserializing temp items JSON");
			return;
		}
		Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
		if (dictionary == null)
		{
			Debug.LogWarning("Error casting to dict in deserializing temp items JSON");
			return;
		}
		Dictionary<string, Dictionary<string, long>> dictionary2 = new Dictionary<string, Dictionary<string, long>>();
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			if (keyValuePair.Value == null)
			{
				Debug.LogWarning("Error kvp.Value == null kvp.Key = " + keyValuePair.Key + " in deserializing temp items JSON");
			}
			else
			{
				Dictionary<string, object> dictionary3 = keyValuePair.Value as Dictionary<string, object>;
				object obj2;
				if (dictionary3 == null)
				{
					Debug.LogWarning("Error innerDict == null kvp.Key = " + keyValuePair.Key + " in deserializing temp items JSON");
				}
				else if (dictionary3.TryGetValue("Duration", out obj2) && obj2 != null)
				{
					long value;
					try
					{
						value = (long)obj2;
					}
					catch (Exception ex)
					{
						Debug.LogWarning("Error unboxing DurationValue in deserializing temp items JSON: " + ex.Message);
						continue;
					}
					object obj3;
					if (dictionary3.TryGetValue("Start", out obj3) && obj3 != null)
					{
						long value2;
						try
						{
							value2 = (long)obj3;
						}
						catch (Exception ex2)
						{
							Debug.LogWarning("Error unboxing StartValue in deserializing temp items JSON: " + ex2.Message);
							continue;
						}
						dictionary2.Add(keyValuePair.Key, new Dictionary<string, long>
						{
							{
								"Start",
								value2
							},
							{
								"Duration",
								value
							}
						});
					}
					else
					{
						Debug.LogWarning(" ! (innerDict.TryGetValue(StartKey,out StartValueObj) && StartValueObj != null) in deserializing temp items JSON");
					}
				}
				else
				{
					Debug.LogWarning(" ! (innerDict.TryGetValue(DurationKey,out DurationValueObj) && DurationValueObj != null) in deserializing temp items JSON");
				}
			}
		}
		this.Items = TempItemsController.ToSaltedDictionary(dictionary2);
	}

	// Token: 0x06004ADC RID: 19164 RVA: 0x001A9490 File Offset: 0x001A7690
	private void SerializeItems()
	{
		Dictionary<string, Dictionary<string, long>> obj = TempItemsController.ToNormalDictionary(this.Items ?? new Dictionary<string, Dictionary<string, SaltedLong>>());
		Storager.setString(Defs.TempItemsDictionaryKey, Json.Serialize(obj), false);
	}

	// Token: 0x06004ADD RID: 19165 RVA: 0x001A94C8 File Offset: 0x001A76C8
	private void DeserializeExpiredObjects()
	{
		if (!Storager.hasKey("ExpiredITemptemsControllerKey"))
		{
			Storager.setString("ExpiredITemptemsControllerKey", "[]", false);
		}
		string @string = Storager.getString("ExpiredITemptemsControllerKey", false);
		object obj = Json.Deserialize(@string);
		if (obj == null)
		{
			Debug.LogWarning("Error Deserializing expired items JSON");
			return;
		}
		List<object> list = obj as List<object>;
		if (list == null)
		{
			Debug.LogWarning("Error casting expired items obj to list");
			return;
		}
		try
		{
			this.ExpiredItems.Clear();
			foreach (object obj2 in list)
			{
				string item = (string)obj2;
				this.ExpiredItems.Add(item);
			}
		}
		catch (Exception arg)
		{
			Debug.LogWarning("Exception when iterating expired items list: " + arg);
		}
	}

	// Token: 0x06004ADE RID: 19166 RVA: 0x001A95D8 File Offset: 0x001A77D8
	private void SerializeExpiredItems()
	{
		Storager.setString("ExpiredITemptemsControllerKey", Json.Serialize(this.ExpiredItems), false);
	}

	// Token: 0x06004ADF RID: 19167 RVA: 0x001A95F0 File Offset: 0x001A77F0
	private static long GetLastSuspendTime()
	{
		return PromoActionsManager.GetUnixTimeFromStorage(Defs.LastTimeTempItemsSuspended);
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x001A95FC File Offset: 0x001A77FC
	private static void SaveSuspendTime()
	{
		Storager.setString(Defs.LastTimeTempItemsSuspended, PromoActionsManager.CurrentUnixTime.ToString(), false);
	}

	// Token: 0x06004AE1 RID: 19169 RVA: 0x001A9624 File Offset: 0x001A7824
	private void OnDestroy()
	{
	}

	// Token: 0x06004AE2 RID: 19170 RVA: 0x001A9628 File Offset: 0x001A7828
	public void TakeTemporaryItemToPlayer(ShopNGUIController.CategoryNames categoryName, string tag, int indexTimeLife)
	{
		this.ExpiredItems.Remove(tag);
	}

	// Token: 0x04003765 RID: 14181
	private const long _salt = 1002855644958404316L;

	// Token: 0x04003766 RID: 14182
	private const string DurationKey = "Duration";

	// Token: 0x04003767 RID: 14183
	private const string StartKey = "Start";

	// Token: 0x04003768 RID: 14184
	private const string ExpiredItemsKey = "ExpiredITemptemsControllerKey";

	// Token: 0x04003769 RID: 14185
	public static TempItemsController sharedController;

	// Token: 0x0400376A RID: 14186
	public List<string> ExpiredItems = new List<string>();

	// Token: 0x0400376B RID: 14187
	public static Dictionary<string, List<float>> PriceCoefs = new Dictionary<string, List<float>>
	{
		{
			WeaponTags.Assault_Machine_Gun_Tag,
			new List<float>
			{
				1f,
				2f,
				4f
			}
		},
		{
			WeaponTags.Impulse_Sniper_Rifle_Tag,
			new List<float>
			{
				1f,
				2.3333333f,
				3.6666667f
			}
		},
		{
			"Armor_Adamant_3",
			new List<float>
			{
				1f,
				2.6666667f,
				5.3333335f
			}
		},
		{
			"hat_Adamant_3",
			new List<float>
			{
				1f,
				2.6666667f,
				5.3333335f
			}
		},
		{
			WeaponTags.RailRevolver_1_Tag,
			new List<float>
			{
				1f,
				2f,
				4f
			}
		},
		{
			WeaponTags.Autoaim_Rocketlauncher_Tag,
			new List<float>
			{
				1f,
				2f,
				3.125f
			}
		},
		{
			WeaponTags.TwoBoltersRent_Tag,
			new List<float>
			{
				1f,
				2f,
				3.125f
			}
		},
		{
			WeaponTags.Red_StoneRent_Tag,
			new List<float>
			{
				1f,
				2f,
				3.125f
			}
		},
		{
			WeaponTags.DragonGunRent_Tag,
			new List<float>
			{
				1f,
				2f,
				3.125f
			}
		},
		{
			WeaponTags.PumpkinGunRent_Tag,
			new List<float>
			{
				1f,
				2f,
				3.125f
			}
		},
		{
			WeaponTags.RayMinigunRent_Tag,
			new List<float>
			{
				1f,
				2f,
				3.125f
			}
		}
	};

	// Token: 0x0400376C RID: 14188
	public static Dictionary<string, string> GunsMappingFromTempToConst = new Dictionary<string, string>();

	// Token: 0x0400376D RID: 14189
	private Dictionary<string, Dictionary<string, SaltedLong>> Items = new Dictionary<string, Dictionary<string, SaltedLong>>();

	// Token: 0x0400376E RID: 14190
	private static List<int> rentTms = null;
}
