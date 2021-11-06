using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class BalanceController : MonoBehaviour
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060000ED RID: 237 RVA: 0x00008540 File Offset: 0x00006740
	// (remove) Token: 0x060000EE RID: 238 RVA: 0x00008558 File Offset: 0x00006758
	public static event Action UpdatedBankView;

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000EF RID: 239 RVA: 0x00008570 File Offset: 0x00006770
	private string EncryptedPlayerprefsKey
	{
		get
		{
			return this.encryptedPlayerprefsKey ?? string.Empty;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000F0 RID: 240 RVA: 0x00008584 File Offset: 0x00006784
	public static string balanceURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_androd.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_ios.json";
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000F1 RID: 241 RVA: 0x000085E0 File Offset: 0x000067E0
	// (set) Token: 0x060000F2 RID: 242 RVA: 0x000085E8 File Offset: 0x000067E8
	public static Dictionary<string, List<ItemPrice>> GunPricesFromServer
	{
		get
		{
			return BalanceController._gunPricesFromServerNew;
		}
		set
		{
			BalanceController._gunPricesFromServerNew = value;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x000085F0 File Offset: 0x000067F0
	// (set) Token: 0x060000F4 RID: 244 RVA: 0x000085F8 File Offset: 0x000067F8
	public static Dictionary<string, List<ItemPrice>> GadgetPricesFromServer
	{
		get
		{
			return BalanceController._gadgetPricesFromServerNew;
		}
		set
		{
			BalanceController._gadgetPricesFromServerNew = value;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000F5 RID: 245 RVA: 0x00008600 File Offset: 0x00006800
	// (set) Token: 0x060000F6 RID: 246 RVA: 0x00008608 File Offset: 0x00006808
	public static Dictionary<string, ItemPrice> pricesFromServer
	{
		get
		{
			return BalanceController._pricesFromServer;
		}
		set
		{
			BalanceController._pricesFromServer = value;
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00008610 File Offset: 0x00006810
	private void Awake()
	{
		byte[] masterKey = Convert.FromBase64String(this.EncryptedPlayerprefsKey);
		this._encryptedPlayerPrefs = new EncryptedPlayerPrefs(masterKey);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00008638 File Offset: 0x00006838
	private void Start()
	{
		BalanceController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.ParseConfig(false);
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			base.StartCoroutine(this.GetBalansFromServer());
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00008678 File Offset: 0x00006878
	[Obfuscation(Exclude = true)]
	private void UpdateBalansFromServer()
	{
		base.StopCoroutine(this.GetBalansFromServer());
		base.StartCoroutine(this.GetBalansFromServer());
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000086A0 File Offset: 0x000068A0
	private IEnumerator GetBalansFromServer()
	{
		string responseText;
		for (;;)
		{
			Task futureToWait = PersistentCacheManager.Instance.FirstResponse;
			if (this._encryptedPlayerPrefs.HasKey(BalanceController.balanceKey) || !string.IsNullOrEmpty(this._encryptedPlayerPrefs.GetString(BalanceController.balanceKey)))
			{
				yield return new WaitUntil(() => futureToWait.IsCompleted);
				string cachedResponse = PersistentCacheManager.Instance.GetValue(BalanceController.balanceURL);
				if (!string.IsNullOrEmpty(cachedResponse))
				{
					break;
				}
			}
			WWWForm form = new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(BalanceController.balanceURL);
			if (download == null)
			{
				yield return new WaitForRealSeconds(30f);
			}
			else
			{
				yield return download;
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild || Application.isEditor)
					{
						Debug.LogWarning("GetBalans error: " + download.error);
					}
					yield return new WaitForRealSeconds(30f);
				}
				else
				{
					responseText = URLs.Sanitize(download);
					if (!string.IsNullOrEmpty(responseText))
					{
						goto Block_5;
					}
				}
			}
		}
		yield break;
		Block_5:
		using (new ScopeLogger("GetBalansFromServer()", "Saving to storager", Defs.IsDeveloperBuild))
		{
			this._encryptedPlayerPrefs.SetString(BalanceController.balanceKey, responseText);
		}
		using (new ScopeLogger("GetBalansFromServer()", "Saving to cache", Defs.IsDeveloperBuild))
		{
			PersistentCacheManager.Instance.SetValue(BalanceController.balanceURL, responseText);
		}
		this.ParseConfig(false);
		if (Debug.isDebugBuild)
		{
			Debug.Log("GetConfigABtestBalans");
		}
		yield break;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x000086BC File Offset: 0x000068BC
	public void ParseConfig(bool isFirstParse = false)
	{
		Dictionary<string, object> dictionary = null;
		string @string = Storager.getString("abTestBalansConfig2Key", false);
		if (!string.IsNullOrEmpty(@string))
		{
			dictionary = (Json.Deserialize(@string) as Dictionary<string, object>);
			if (dictionary == null)
			{
				Storager.setString("abTestBalansConfig2Key", string.Empty, false);
				Debug.LogError("AB TEST BALANCE CONFIG NOT CORRECT !!!");
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("jsonConfigABTest = ' " + @string + "'");
				}
				return;
			}
			if (dictionary.ContainsKey("NameConfig"))
			{
				BalanceController.ParseABTestBalansNameConfig(dictionary["NameConfig"], isFirstParse);
			}
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.A)
			{
				if (this._encryptedPlayerPrefs.HasKey(BalanceController.balanceKey) || !string.IsNullOrEmpty(this._encryptedPlayerPrefs.GetString(BalanceController.balanceKey)))
				{
					this.jsonConfig = this._encryptedPlayerPrefs.GetString(BalanceController.balanceKey);
				}
				dictionary = (Json.Deserialize(this.jsonConfig) as Dictionary<string, object>);
			}
		}
		else
		{
			if (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE)
			{
				AnalyticsStuff.LogABTest("New Balance", Defs.abTestBalansCohortName, false);
				Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
				Defs.abTestBalansCohortName = string.Empty;
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData(false);
				}
			}
			if (this._encryptedPlayerPrefs.HasKey(BalanceController.balanceKey) || !string.IsNullOrEmpty(this._encryptedPlayerPrefs.GetString(BalanceController.balanceKey)))
			{
				this.jsonConfig = this._encryptedPlayerPrefs.GetString(BalanceController.balanceKey);
			}
			if (string.IsNullOrEmpty(this.jsonConfig))
			{
				Debug.LogError("BALANCE CONFIG EMPTY !!!");
				return;
			}
			try
			{
				dictionary = (Json.Deserialize(this.jsonConfig) as Dictionary<string, object>);
			}
			catch (Exception ex)
			{
				Debug.LogError("Balans Controller Error parse config: " + ex.Message);
			}
		}
		if (dictionary == null)
		{
			Debug.LogError("BALANCE CONFIG NOT CORRECT !!!");
			return;
		}
		BalanceController.pricesFromServer.Clear();
		if (dictionary.ContainsKey("Weapons"))
		{
			this.ParseWeaponsConfig(dictionary["Weapons"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Gadgets"))
		{
			this.ParseGadgetsConfig(dictionary["Gadgets"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Pets"))
		{
			this.ParsePetsConfig(dictionary["Pets"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Eggs"))
		{
			this.ParseEggsConfig(dictionary["Eggs"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("ItemPrices"))
		{
			this.ParseItemPricesConfig(dictionary["ItemPrices"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Levelling"))
		{
			BalanceController.ParseLevelingConfig(dictionary["Levelling"]);
		}
		bool flag = false;
		if (dictionary.ContainsKey("Inapps"))
		{
			flag = BalanceController.ParseInappsConfig(dictionary["Inapps"]);
		}
		bool flag2 = false;
		if (dictionary.ContainsKey("InappsBonus"))
		{
			flag2 = BalanceController.ParseInappsBonusConfig(dictionary["InappsBonus"]);
		}
		else
		{
			BalanceController.inappsBonus.Clear();
			BalanceController._inappObjBonus = string.Empty;
		}
		if ((flag || flag2) && BalanceController.UpdatedBankView != null)
		{
			BalanceController.UpdatedBankView();
		}
		if (dictionary.ContainsKey("Rewards"))
		{
			BalanceController.ParseAwardConfig(dictionary["Rewards"]);
		}
		if (dictionary.ContainsKey("Levelling"))
		{
			BalanceController.ParseLevelingConfig(dictionary["Levelling"]);
		}
		if (dictionary.ContainsKey("SpecialEvents"))
		{
			BalanceController.ParseSpecialEventsConfig(dictionary["SpecialEvents"]);
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00008A84 File Offset: 0x00006C84
	private static void ParseABTestBalansNameConfig(object obj, bool isFirstParse)
	{
		List<object> list = obj as List<object>;
		Dictionary<string, object> dictionary = list[0] as Dictionary<string, object>;
		if (dictionary.ContainsKey("Group"))
		{
			Defs.abTestBalansCohort = (Defs.ABTestCohortsType)((int)Enum.Parse(typeof(Defs.ABTestCohortsType), dictionary["Group"] as string));
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
			{
				Defs.isABTestBalansCohortActual = true;
			}
			if (Application.isEditor)
			{
				Debug.Log("Defs.abTestBalansCohort = " + Defs.abTestBalansCohort.ToString());
			}
		}
		if (dictionary.ContainsKey("NameGroup"))
		{
			Defs.abTestBalansCohortName = (dictionary["NameGroup"] as string);
			if (isFirstParse)
			{
				AnalyticsStuff.LogABTest("New Balance", Defs.abTestBalansCohortName, true);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData(false);
				}
			}
			if (Application.isEditor)
			{
				Debug.Log("abTestBalansCohortName = " + Defs.abTestBalansCohortName.ToString());
			}
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00008B90 File Offset: 0x00006D90
	private void ParseWeaponsConfig(Dictionary<string, object> _weaponsConfig)
	{
		BalanceController.dpsWeapons.Clear();
		BalanceController.damageWeapons.Clear();
		BalanceController.survivalDamageWeapons.Clear();
		BalanceController.GunPricesFromServer.Clear();
		foreach (KeyValuePair<string, object> keyValuePair in _weaponsConfig)
		{
			if (!string.IsNullOrEmpty(keyValuePair.Key))
			{
				Dictionary<string, object> dictionary = keyValuePair.Value as Dictionary<string, object>;
				int num = 1;
				if (dictionary.ContainsKey("D1"))
				{
					float[] array = new float[6];
					for (int i = 2; i <= 6; i++)
					{
						if (dictionary.ContainsKey("D" + i.ToString()))
						{
							num = i;
						}
					}
					float num2 = 0.1f;
					float num3 = 0.1f;
					float num4;
					if (float.TryParse(dictionary["D1"].ToString(), out num4))
					{
						num2 = num4;
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseWeaponsConfig: not parse " + keyValuePair.Key + " parametr D1");
					}
					if (num > 1)
					{
						float num5;
						if (float.TryParse(dictionary["D" + num].ToString(), out num5))
						{
							num3 = num5;
						}
						else if (Application.isEditor)
						{
							Debug.LogError(string.Concat(new object[]
							{
								"ParseWeaponsConfig: not parse ",
								keyValuePair.Key,
								" parametr D",
								num
							}));
						}
					}
					else
					{
						num3 = num2;
					}
					for (int j = 1; j <= 6; j++)
					{
						array[j - 1] = ((j >= num) ? num3 : (num2 + (num3 - num2) / (float)(num - 1) * (float)(j - 1)));
					}
					BalanceController.dpsWeapons.Add("Weapon" + keyValuePair.Key, array);
				}
				if (dictionary.ContainsKey("U1"))
				{
					float[] array2 = new float[6];
					float num6 = 0.1f;
					float num7 = 0.1f;
					float num8;
					if (float.TryParse(dictionary["U1"].ToString(), out num8))
					{
						num6 = num8;
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseWeaponsConfig: not parse " + keyValuePair.Key + " parametr U1");
					}
					if (num > 1)
					{
						float num9;
						if (float.TryParse(dictionary["U" + num].ToString(), out num9))
						{
							num7 = num9;
						}
						else if (Application.isEditor)
						{
							Debug.LogError(string.Concat(new object[]
							{
								"ParseWeaponsConfig: not parse ",
								keyValuePair.Key,
								" parametr U",
								num
							}));
						}
					}
					else
					{
						num7 = num6;
					}
					for (int k = 1; k <= 6; k++)
					{
						array2[k - 1] = ((k >= num) ? num7 : (num6 + (num7 - num6) / (float)(num - 1) * (float)(k - 1)));
					}
					BalanceController.damageWeapons.Add("Weapon" + keyValuePair.Key, array2);
				}
				if (dictionary.ContainsKey("S"))
				{
					int value;
					if (int.TryParse(dictionary["S"].ToString(), out value))
					{
						BalanceController.survivalDamageWeapons.Add(keyValuePair.Key, value);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseWeaponsConfig: not parse " + keyValuePair.Key + " parametr Survival_damage ");
					}
				}
				if (dictionary.ContainsKey("P"))
				{
					string text = dictionary["P"].ToString();
					int num10 = Convert.ToInt32(text.Substring(1));
					int price = (!dictionary.ContainsKey("oP")) ? num10 : Convert.ToInt32(dictionary["oP"]);
					string currency = (!text.Substring(0, 1).Equals("C")) ? "GemsCurrency" : "Coins";
					BalanceController.GunPricesFromServer.Add("Weapon" + keyValuePair.Key, new List<ItemPrice>
					{
						new ItemPrice(price, currency),
						new ItemPrice(num10, currency)
					});
				}
			}
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00009030 File Offset: 0x00007230
	private void ParseGadgetsConfig(Dictionary<string, object> _gadgetsConfig)
	{
		BalanceController.GadgetPricesFromServer.Clear();
		BalanceController.damageGadgetes.Clear();
		BalanceController.dpsGadgetes.Clear();
		BalanceController.survivalDamageGadgetes.Clear();
		BalanceController.cooldownGadgetes.Clear();
		BalanceController.durationGadgetes.Clear();
		BalanceController.durabilityGadgetes.Clear();
		BalanceController.healGadgetes.Clear();
		BalanceController.hpsGadgetes.Clear();
		foreach (KeyValuePair<string, object> keyValuePair in _gadgetsConfig)
		{
			if (!string.IsNullOrEmpty(keyValuePair.Key))
			{
				Dictionary<string, object> dictionary = keyValuePair.Value as Dictionary<string, object>;
				string key = "gadget_" + keyValuePair.Key;
				string text = "CD";
				if (dictionary.ContainsKey(text))
				{
					float value;
					if (float.TryParse(dictionary[text].ToString(), out value))
					{
						BalanceController.cooldownGadgetes.Add(key, value);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "T";
				if (dictionary.ContainsKey(text))
				{
					float value2;
					if (float.TryParse(dictionary[text].ToString(), out value2))
					{
						BalanceController.durationGadgetes.Add(key, value2);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "D";
				if (dictionary.ContainsKey(text))
				{
					float value3;
					if (float.TryParse(dictionary[text].ToString(), out value3))
					{
						BalanceController.damageGadgetes.Add(key, value3);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "DPS";
				if (dictionary.ContainsKey(text))
				{
					float value4;
					if (float.TryParse(dictionary[text].ToString(), out value4))
					{
						BalanceController.dpsGadgetes.Add(key, value4);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "SD";
				if (dictionary.ContainsKey(text))
				{
					int value5;
					if (int.TryParse(dictionary[text].ToString(), out value5))
					{
						BalanceController.survivalDamageGadgetes.Add(key, value5);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "Dur";
				if (dictionary.ContainsKey(text))
				{
					float value6;
					if (float.TryParse(dictionary[text].ToString(), out value6))
					{
						BalanceController.durabilityGadgetes.Add(key, value6);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "H";
				if (dictionary.ContainsKey(text))
				{
					float value7;
					if (float.TryParse(dictionary[text].ToString(), out value7))
					{
						BalanceController.healGadgetes.Add(key, value7);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "HPS";
				if (dictionary.ContainsKey(text))
				{
					float value8;
					if (float.TryParse(dictionary[text].ToString(), out value8))
					{
						BalanceController.hpsGadgetes.Add(key, value8);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "P";
				if (dictionary.ContainsKey(text))
				{
					string text2 = dictionary[text].ToString();
					int num = Convert.ToInt32(text2.Substring(1));
					string key2 = "oP";
					int price;
					if (dictionary.ContainsKey(key2))
					{
						if (!int.TryParse(dictionary[key2].ToString(), out price))
						{
							price = num;
							if (Application.isEditor)
							{
								Debug.LogError("ParseGadgetConfig: not parse " + keyValuePair.Key + " parametr " + text);
							}
						}
					}
					else
					{
						price = num;
					}
					string currency = (!text2.Substring(0, 1).Equals("C")) ? "GemsCurrency" : "Coins";
					BalanceController.GadgetPricesFromServer.Add(key, new List<ItemPrice>
					{
						new ItemPrice(price, currency),
						new ItemPrice(num, currency)
					});
				}
			}
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00009540 File Offset: 0x00007740
	private void ParsePetsConfig(Dictionary<string, object> _petsConfig)
	{
		BalanceController.damagePets.Clear();
		BalanceController.dpsPets.Clear();
		BalanceController.survivalDamagePets.Clear();
		BalanceController.respawnTimePets.Clear();
		BalanceController.hpPets.Clear();
		BalanceController.speedPets.Clear();
		BalanceController.cashbackPets.Clear();
		foreach (KeyValuePair<string, object> keyValuePair in _petsConfig)
		{
			if (!string.IsNullOrEmpty(keyValuePair.Key))
			{
				Dictionary<string, object> dictionary = keyValuePair.Value as Dictionary<string, object>;
				string key = "pet_" + keyValuePair.Key;
				string text = "R";
				if (dictionary.ContainsKey(text))
				{
					float value;
					if (float.TryParse(dictionary[text].ToString(), out value))
					{
						BalanceController.respawnTimePets.Add(key, value);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "HP";
				if (dictionary.ContainsKey(text))
				{
					float value2;
					if (float.TryParse(dictionary[text].ToString(), out value2))
					{
						BalanceController.hpPets.Add(key, value2);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "DPS";
				if (dictionary.ContainsKey(text))
				{
					int value3;
					if (int.TryParse(dictionary[text].ToString(), out value3))
					{
						BalanceController.dpsPets.Add(key, value3);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "D";
				if (dictionary.ContainsKey(text))
				{
					float value4;
					if (float.TryParse(dictionary[text].ToString(), out value4))
					{
						BalanceController.damagePets.Add(key, value4);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "SD";
				if (dictionary.ContainsKey(text))
				{
					int value5;
					if (int.TryParse(dictionary[text].ToString(), out value5))
					{
						BalanceController.survivalDamagePets.Add(key, value5);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "S";
				if (dictionary.ContainsKey(text))
				{
					float value6;
					if (float.TryParse(dictionary[text].ToString(), out value6))
					{
						BalanceController.speedPets.Add(key, value6);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "C";
				if (dictionary.ContainsKey(text))
				{
					int value7;
					if (int.TryParse(dictionary[text].ToString(), out value7))
					{
						BalanceController.cashbackPets.Add(key, value7);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParsePetsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "P";
				if (dictionary.ContainsKey(text))
				{
					string text2 = dictionary[text].ToString();
					int price = Convert.ToInt32(text2.Substring(1));
					string currency = (!text2.Substring(0, 1).Equals("C")) ? "GemsCurrency" : "Coins";
					BalanceController.pricesFromServer.Add(key, new ItemPrice(price, currency));
				}
			}
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00009950 File Offset: 0x00007B50
	private void ParseEggsConfig(Dictionary<string, object> _eggsConfig)
	{
		BalanceController.timeEggs.Clear();
		BalanceController.victoriasEggs.Clear();
		BalanceController.ratingEggs.Clear();
		BalanceController.rarityPetsInEggs.Clear();
		foreach (KeyValuePair<string, object> keyValuePair in _eggsConfig)
		{
			if (!string.IsNullOrEmpty(keyValuePair.Key))
			{
				Dictionary<string, object> dictionary = keyValuePair.Value as Dictionary<string, object>;
				string key = keyValuePair.Key;
				string text = "T";
				if (dictionary.ContainsKey(text))
				{
					int value;
					if (int.TryParse(dictionary[text].ToString(), out value))
					{
						BalanceController.timeEggs.Add(key, value);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "V";
				if (dictionary.ContainsKey(text))
				{
					int value2;
					if (int.TryParse(dictionary[text].ToString(), out value2))
					{
						BalanceController.victoriasEggs.Add(key, value2);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "Rat";
				if (dictionary.ContainsKey(text))
				{
					int value3;
					if (int.TryParse(dictionary[text].ToString(), out value3))
					{
						BalanceController.ratingEggs.Add(key, value3);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "C";
				if (dictionary.ContainsKey(text))
				{
					float chance;
					if (float.TryParse(dictionary[text].ToString(), out chance))
					{
						if (!BalanceController.rarityPetsInEggs.ContainsKey(key))
						{
							BalanceController.rarityPetsInEggs.Add(key, new List<EggPetInfo>());
						}
						EggPetInfo eggPetInfo = new EggPetInfo();
						eggPetInfo.Rarity = ItemDb.ItemRarity.Common;
						eggPetInfo.Chance = chance;
						BalanceController.rarityPetsInEggs[key].Add(eggPetInfo);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "U";
				if (dictionary.ContainsKey(text))
				{
					float chance2;
					if (float.TryParse(dictionary[text].ToString(), out chance2))
					{
						if (!BalanceController.rarityPetsInEggs.ContainsKey(key))
						{
							BalanceController.rarityPetsInEggs.Add(key, new List<EggPetInfo>());
						}
						EggPetInfo eggPetInfo2 = new EggPetInfo();
						eggPetInfo2.Rarity = ItemDb.ItemRarity.Uncommon;
						eggPetInfo2.Chance = chance2;
						BalanceController.rarityPetsInEggs[key].Add(eggPetInfo2);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "R";
				if (dictionary.ContainsKey(text))
				{
					float chance3;
					if (float.TryParse(dictionary[text].ToString(), out chance3))
					{
						if (!BalanceController.rarityPetsInEggs.ContainsKey(key))
						{
							BalanceController.rarityPetsInEggs.Add(key, new List<EggPetInfo>());
						}
						EggPetInfo eggPetInfo3 = new EggPetInfo();
						eggPetInfo3.Rarity = ItemDb.ItemRarity.Rare;
						eggPetInfo3.Chance = chance3;
						BalanceController.rarityPetsInEggs[key].Add(eggPetInfo3);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "E";
				if (dictionary.ContainsKey(text))
				{
					float chance4;
					if (float.TryParse(dictionary[text].ToString(), out chance4))
					{
						if (!BalanceController.rarityPetsInEggs.ContainsKey(key))
						{
							BalanceController.rarityPetsInEggs.Add(key, new List<EggPetInfo>());
						}
						EggPetInfo eggPetInfo4 = new EggPetInfo();
						eggPetInfo4.Rarity = ItemDb.ItemRarity.Epic;
						eggPetInfo4.Chance = chance4;
						BalanceController.rarityPetsInEggs[key].Add(eggPetInfo4);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "L";
				if (dictionary.ContainsKey(text))
				{
					float chance5;
					if (float.TryParse(dictionary[text].ToString(), out chance5))
					{
						if (!BalanceController.rarityPetsInEggs.ContainsKey(key))
						{
							BalanceController.rarityPetsInEggs.Add(key, new List<EggPetInfo>());
						}
						EggPetInfo eggPetInfo5 = new EggPetInfo();
						eggPetInfo5.Rarity = ItemDb.ItemRarity.Legendary;
						eggPetInfo5.Chance = chance5;
						BalanceController.rarityPetsInEggs[key].Add(eggPetInfo5);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "M";
				if (dictionary.ContainsKey(text))
				{
					float chance6;
					if (float.TryParse(dictionary[text].ToString(), out chance6))
					{
						if (!BalanceController.rarityPetsInEggs.ContainsKey(key))
						{
							BalanceController.rarityPetsInEggs.Add(key, new List<EggPetInfo>());
						}
						EggPetInfo eggPetInfo6 = new EggPetInfo();
						eggPetInfo6.Rarity = ItemDb.ItemRarity.Mythic;
						eggPetInfo6.Chance = chance6;
						BalanceController.rarityPetsInEggs[key].Add(eggPetInfo6);
					}
					else if (Application.isEditor)
					{
						Debug.LogError("ParseEggsConfig: not parse " + keyValuePair.Key + " parametr " + text);
					}
				}
				text = "P";
				if (dictionary.ContainsKey(text))
				{
					string text2 = dictionary[text].ToString();
					int price = Convert.ToInt32(text2.Substring(1));
					string currency = (!text2.Substring(0, 1).Equals("C")) ? "GemsCurrency" : "Coins";
					BalanceController.pricesFromServer.Add((!(key == "SI")) ? key : "Eggs.SuperIncubatorId", new ItemPrice(price, currency));
				}
			}
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00009F8C File Offset: 0x0000818C
	private void ParseItemPricesConfig(Dictionary<string, object> _itemPricesConfig)
	{
		foreach (KeyValuePair<string, object> keyValuePair in _itemPricesConfig)
		{
			string text = keyValuePair.Value.ToString();
			int price = Convert.ToInt32(text.Substring(1));
			string currency = (!text.Substring(0, 1).Equals("C")) ? "GemsCurrency" : "Coins";
			BalanceController.pricesFromServer.Add(keyValuePair.Key, new ItemPrice(price, currency));
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0000A044 File Offset: 0x00008244
	private static void ParseLevelingConfig(object obj)
	{
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			int num = Convert.ToInt32(dictionary["L"]);
			int coins = Convert.ToInt32(dictionary["C"]);
			int gems = Convert.ToInt32(dictionary["G"]);
			ExperienceController.RewriteLevelingParametersForLevel(num - 1, coins, gems);
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000A0C0 File Offset: 0x000082C0
	private static bool ParseInappsConfig(object obj)
	{
		string text = Json.Serialize(obj);
		if (text == BalanceController._inappObj)
		{
			return false;
		}
		BalanceController._inappObj = text;
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			int priceId = Convert.ToInt32(dictionary["Real"]);
			int coinQuantity = Convert.ToInt32(dictionary["C"]);
			int gemsQuantity = Convert.ToInt32(dictionary["G"]);
			int bonusCoins = Convert.ToInt32(dictionary["BC"]);
			int bonusGems = Convert.ToInt32(dictionary["BG"]);
			VirtualCurrencyHelper.RewriteInappsQuantity(priceId, coinQuantity, gemsQuantity, bonusCoins, bonusGems);
		}
		return true;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x0000A184 File Offset: 0x00008384
	private static bool ParseInappsBonusConfig(object obj)
	{
		string text = Json.Serialize(obj);
		if (text == BalanceController._inappObjBonus)
		{
			return false;
		}
		BalanceController._inappObjBonus = text;
		BalanceController.inappsBonus.Clear();
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			if (dictionary.ContainsKey("ID"))
			{
				string item = Convert.ToString(dictionary["ID"]);
				if (BalanceController.supportedInappBonusIds.Contains(item))
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					string value = Convert.ToString(dictionary["ID"]);
					int num = Convert.ToInt32(dictionary["Real"]);
					bool flag = Convert.ToString(dictionary["Cur"]) != "Coins";
					string value2 = string.Empty;
					string[] array = (!flag) ? StoreKitEventListener.coinIds : StoreKitEventListener.gemsIds;
					int num2 = 0;
					for (int j = 0; j < VirtualCurrencyHelper.coinPriceIds.Length; j++)
					{
						if (VirtualCurrencyHelper.coinPriceIds[j] == num)
						{
							value2 = array[j];
							num2 = j;
							break;
						}
					}
					if (!string.IsNullOrEmpty(value2))
					{
						dictionary2.Add("action", value);
						dictionary2.Add("isGems", flag);
						dictionary2.Add("Start", dictionary["Start"]);
						dictionary2.Add("End", dictionary["End"]);
						float num3 = (float)num / (float)(VirtualCurrencyHelper.gemsInappsQuantity[num2] * 3);
						float num4 = (float)num / (float)(VirtualCurrencyHelper.coinInappsQuantity[num2] * 3);
						dictionary2.Add("Real", dictionary["Real"]);
						dictionary2.Add("priceGems", num3);
						dictionary2.Add("priceCoins", num4);
						dictionary2.Add("id", value2);
						if (dictionary.ContainsKey("C"))
						{
							dictionary2.Add("Coins", dictionary["C"]);
						}
						if (dictionary.ContainsKey("G"))
						{
							dictionary2.Add("GemsCurrency", dictionary["G"]);
						}
						if (dictionary.ContainsKey("Type"))
						{
							dictionary2.Add("Type", dictionary["Type"]);
						}
						if (dictionary.ContainsKey("Packs"))
						{
							dictionary2.Add("Pack", dictionary["Packs"]);
						}
						if (dictionary.ContainsKey("Count"))
						{
							dictionary2.Add("Count", dictionary["Count"]);
						}
						if (dictionary.ContainsKey("Profit"))
						{
							dictionary2.Add("Profit", dictionary["Profit"]);
						}
						if (dictionary.ContainsKey("Ids"))
						{
							List<object> list2 = Json.Deserialize(Convert.ToString(dictionary["Ids"])) as List<object>;
							if (list2 != null)
							{
								dictionary2.Add("Ids", list2);
							}
						}
						if (dictionary.ContainsKey("AddBonus"))
						{
							string text2 = dictionary["AddBonus"].ToString();
							int num5 = Convert.ToInt32(text2.Substring(1));
							string value3 = (!text2.Substring(0, 1).Equals("C")) ? "GemsCurrency" : "Coins";
							dictionary2.Add("AddBonusCount", num5);
							dictionary2.Add("AddBonusCurrency", value3);
						}
						BalanceController.inappsBonus.Add(dictionary2);
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0000A548 File Offset: 0x00008748
	private static void ParseAwardConfig(object obj)
	{
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			string text = Convert.ToString(dictionary["Mode"]);
			int[] array = new int[10];
			int num = 5;
			if (dictionary.ContainsKey("Sc"))
			{
				num = Convert.ToInt32(dictionary["Sc"]);
			}
			for (int j = 1; j <= 10; j++)
			{
				if (dictionary.ContainsKey(j.ToString()))
				{
					array[j - 1] = Convert.ToInt32(dictionary[j.ToString()]);
				}
			}
			if (text.Equals("XP Team"))
			{
				AdminSettingsController.expAvardTeamFight[0] = array;
				AdminSettingsController.minScoreTeamFight = num;
			}
			if (text.Equals("Coins Team"))
			{
				AdminSettingsController.coinAvardTeamFight[0] = array;
				AdminSettingsController.minScoreTeamFight = num;
			}
			if (text.Equals("XP DM"))
			{
				AdminSettingsController.expAvardDeathMath[0] = array;
				AdminSettingsController.minScoreDeathMath = num;
			}
			if (text.Equals("Coins DM"))
			{
				AdminSettingsController.coinAvardDeathMath[0] = array;
				AdminSettingsController.minScoreDeathMath = num;
			}
			if (text.Equals("XP Coop"))
			{
				AdminSettingsController.expAvardTimeBattle = array;
				AdminSettingsController.minScoreTimeBattle = num;
			}
			if (text.Equals("Coins Coop"))
			{
				AdminSettingsController.coinAvardTimeBattle = array;
				AdminSettingsController.minScoreTimeBattle = num;
			}
			if (text.Equals("XP Flag"))
			{
				AdminSettingsController.expAvardFlagCapture[0] = array;
				AdminSettingsController.minScoreFlagCapture = num;
			}
			if (text.Equals("Coins Flag"))
			{
				AdminSettingsController.coinAvardFlagCapture[0] = array;
				AdminSettingsController.minScoreFlagCapture = num;
			}
			if (text.Equals("XP Deadly"))
			{
				AdminSettingsController.expAvardDeadlyGames = array;
			}
			if (text.Equals("Coins Deadly"))
			{
				AdminSettingsController.coinAvardDeadlyGames = array;
			}
			if (text.Equals("XP Points"))
			{
				AdminSettingsController.expAvardCapturePoint[0] = array;
				AdminSettingsController.minScoreCapturePoint = num;
			}
			if (text.Equals("Coins Points"))
			{
				AdminSettingsController.coinAvardCapturePoint[0] = array;
				AdminSettingsController.minScoreCapturePoint = num;
			}
			if (text.Equals("XP Duels"))
			{
				AdminSettingsController.expAvardDuel = array;
				AdminSettingsController.minScoreDuel = num;
			}
			if (text.Equals("Coins Duels"))
			{
				AdminSettingsController.coinAvardDuel = array;
				AdminSettingsController.minScoreDuel = num;
			}
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x0000A7A8 File Offset: 0x000089A8
	private static void ParseSpecialEventsConfig(object obj)
	{
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			string text = Convert.ToString(dictionary["Event"]);
			if (text.Equals("StartCapital"))
			{
				bool flag = Convert.ToBoolean(dictionary["Enable"]);
				if (flag)
				{
					int num = Convert.ToInt32(dictionary["Coins"]);
					int num2 = Convert.ToInt32(dictionary["Gems"]);
					BalanceController.startCapitalCoins = num;
					BalanceController.startCapitalGems = num2;
					BalanceController.startCapitalEnabled = true;
				}
				else
				{
					BalanceController.startCapitalEnabled = false;
				}
			}
			if (text.Equals("СompetitionAward"))
			{
				bool flag2 = Convert.ToBoolean(dictionary["Enable"]);
				if (flag2)
				{
					int num3 = Convert.ToInt32(dictionary["Coins"]);
					int num4 = Convert.ToInt32(dictionary["Gems"]);
					BalanceController.countPlaceAwardInCompetion = Convert.ToInt32(dictionary["XP"]);
					if (num3 > 0)
					{
						BalanceController.competitionAward = new ItemPrice(num3, "Coins");
					}
					else if (num4 > 0)
					{
						BalanceController.competitionAward = new ItemPrice(num4, "GemsCurrency");
					}
					else
					{
						BalanceController.competitionAward = new ItemPrice(0, "Coins");
					}
				}
				else
				{
					BalanceController.competitionAward = new ItemPrice(0, "Coins");
					BalanceController.countPlaceAwardInCompetion = 0;
				}
			}
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x0000A928 File Offset: 0x00008B28
	public static string GetCurrenceCurrentInnapBonus()
	{
		string result = string.Empty;
		List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
		bool flag = false;
		bool flag2 = false;
		if (currentInnapBonus != null)
		{
			foreach (Dictionary<string, object> dictionary in currentInnapBonus)
			{
				if (dictionary.ContainsKey("isGems"))
				{
					bool flag3 = Convert.ToBoolean(dictionary["isGems"]);
					if (flag3)
					{
						flag = true;
					}
					else
					{
						flag2 = true;
					}
				}
			}
		}
		if (flag)
		{
			result = "GemsCurrency";
		}
		else if (flag2)
		{
			result = "Coins";
		}
		return result;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0000A9F0 File Offset: 0x00008BF0
	public static bool isActiveInnapBonus()
	{
		if (!TrainingController.TrainingCompleted || ExperienceController.sharedController.currentLevel < 2 || FriendsController.ServerTime < 1L)
		{
			return false;
		}
		foreach (Dictionary<string, object> dictionary in BalanceController.inappsBonus)
		{
			if (!dictionary.ContainsKey("Start") || !BalanceController.keysInappBonusActionGiven.Contains(Convert.ToString(dictionary["Start"])))
			{
				DateTime t = DateTime.MinValue;
				DateTime t2 = DateTime.MinValue;
				DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime(FriendsController.ServerTime);
				if (dictionary.ContainsKey("Start") && dictionary.ContainsKey("End"))
				{
					t = Convert.ToDateTime(dictionary["Start"], CultureInfo.InvariantCulture);
					t2 = Convert.ToDateTime(dictionary["End"], CultureInfo.InvariantCulture);
				}
				if (t <= currentTimeByUnixTime && currentTimeByUnixTime <= t2)
				{
					if (dictionary.ContainsKey("action") && Convert.ToString(dictionary["action"]) == BalanceController.petActionName)
					{
						string value = string.Empty;
						if (dictionary.ContainsKey("Ids"))
						{
							value = BalanceController.GetCurentPetID(dictionary["Start"].ToString(), dictionary["Ids"] as List<object>);
						}
						if (string.IsNullOrEmpty(value))
						{
							continue;
						}
					}
					if (dictionary.ContainsKey("action") && Convert.ToString(dictionary["action"]) == BalanceController.weaponActionName)
					{
						string value2 = string.Empty;
						if (dictionary.ContainsKey("Ids"))
						{
							value2 = BalanceController.GetCurentWeaponID(dictionary["Start"].ToString(), dictionary["Ids"] as List<object>);
						}
						if (string.IsNullOrEmpty(value2))
						{
							continue;
						}
					}
					if (!dictionary.ContainsKey("action") || !(Convert.ToString(dictionary["action"]) == BalanceController.gadgetActionName) || BalanceController.GetCurentGadgetesIDs(dictionary["Start"].ToString(), dictionary["Ids"] as List<object>) != null)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000AC94 File Offset: 0x00008E94
	private static string GetCurentPetID(string _key, List<object> _ids)
	{
		string @string = Storager.getString(Defs.keyInappBonusStartActionForPresentIDPetkey, false);
		if (@string == _key)
		{
			return Storager.getString(Defs.keyInappPresentIDPetkey, false);
		}
		List<string> list = new List<string>();
		for (int i = 0; i < _ids.Count; i++)
		{
			list.Add(_ids[i].ToString());
		}
		string firstSmallestUpPet = Singleton<PetsManager>.Instance.GetFirstSmallestUpPet(list);
		if (!string.IsNullOrEmpty(firstSmallestUpPet))
		{
			Storager.setString(Defs.keyInappPresentIDPetkey, firstSmallestUpPet, false);
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDPetkey, _key, false);
		}
		return firstSmallestUpPet;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0000AD24 File Offset: 0x00008F24
	private static bool isWeaponAvalibleForBonus(string weaponTag)
	{
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		return byTag != null && byTag.StorageId != null && Storager.getInt(byTag.StorageId, true) <= 0;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000AD60 File Offset: 0x00008F60
	private static bool isGadgetAvalibleForBonus(string gadgetTag)
	{
		return GadgetsInfo.info.ContainsKey(gadgetTag) && !GadgetsInfo.IsBought(gadgetTag);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x0000AD80 File Offset: 0x00008F80
	private static string GetCurentWeaponID(string _key, List<object> _ids)
	{
		string @string = Storager.getString(Defs.keyInappBonusStartActionForPresentIDWeaponRedkey, false);
		if (@string == _key)
		{
			string string2 = Storager.getString(Defs.keyInappPresentIDWeaponRedkey, false);
			if (BalanceController.isWeaponAvalibleForBonus(string2))
			{
				return string2;
			}
		}
		string text = null;
		int num = ExpController.OurTierForAnyPlace();
		int num2 = num;
		while (num2 >= num - 1 && num2 >= 0)
		{
			List<object> list = _ids[num2] as List<object>;
			for (int i = 0; i < list.Count; i++)
			{
				if (BalanceController.isWeaponAvalibleForBonus(list[i].ToString()))
				{
					text = list[i].ToString();
					break;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				break;
			}
			num2--;
		}
		if (!string.IsNullOrEmpty(text))
		{
			Storager.setString(Defs.keyInappPresentIDWeaponRedkey, text, false);
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDWeaponRedkey, _key, false);
		}
		return text;
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600010D RID: 269 RVA: 0x0000AE74 File Offset: 0x00009074
	// (set) Token: 0x0600010E RID: 270 RVA: 0x0000AF40 File Offset: 0x00009140
	private static List<string> curentGadgetesIDs
	{
		get
		{
			if (BalanceController._curentGadgetesIDs == null)
			{
				try
				{
					string @string = Storager.getString(Defs.keyInappPresentIDGadgetkey, false);
					if (!string.IsNullOrEmpty(@string))
					{
						List<object> list = Json.Deserialize(@string) as List<object>;
						if (list != null && list.Count == 3)
						{
							BalanceController._curentGadgetesIDs = new List<string>();
							for (int i = 0; i < list.Count; i++)
							{
								BalanceController._curentGadgetesIDs.Add(list[i].ToString());
							}
						}
					}
				}
				catch (Exception arg)
				{
					Debug.Log("Parse curentGadgetesIDs: " + arg);
					return null;
				}
			}
			return BalanceController._curentGadgetesIDs;
		}
		set
		{
			BalanceController._curentGadgetesIDs = value;
			string val = Json.Serialize(BalanceController._curentGadgetesIDs);
			Storager.setString(Defs.keyInappPresentIDGadgetkey, val, false);
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000AF6C File Offset: 0x0000916C
	private static bool isAvalibleListGadgetes(List<string> _ids)
	{
		if (_ids != null)
		{
			for (int i = 0; i < _ids.Count; i++)
			{
				bool flag = GadgetsInfo.IsBought(_ids[i]);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000AFB4 File Offset: 0x000091B4
	private static List<string> GetCurentGadgetesIDs(string _key, List<object> _ids)
	{
		string @string = Storager.getString(Defs.keyInappBonusStartActionForPresentIDGadgetkey, false);
		if (@string == _key)
		{
			List<string> curentGadgetesIDs = BalanceController.curentGadgetesIDs;
			if (BalanceController.isAvalibleListGadgetes(curentGadgetesIDs))
			{
				return curentGadgetesIDs;
			}
		}
		List<string> list = new List<string>();
		int num = ExpController.OurTierForAnyPlace();
		for (int i = 0; i < _ids.Count; i++)
		{
			List<object> list2 = _ids[i] as List<object>;
			string text = null;
			for (int j = num; j >= 0; j--)
			{
				List<object> list3 = list2[j] as List<object>;
				for (int k = 0; k < list3.Count; k++)
				{
					if (!GadgetsInfo.IsBought(list3[k].ToString()))
					{
						text = list3[k].ToString();
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
					break;
				}
			}
		}
		if (list.Count == 3)
		{
			BalanceController.curentGadgetesIDs = list;
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDGadgetkey, _key, false);
			return list;
		}
		return null;
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000111 RID: 273 RVA: 0x0000B0D4 File Offset: 0x000092D4
	public static List<string> keysInappBonusActionGiven
	{
		get
		{
			if (BalanceController._keysInappBonusActionGiven == null)
			{
				BalanceController._keysInappBonusActionGiven = new List<string>();
				string @string = Storager.getString(Defs.keysInappBonusGivenkey, false);
				List<object> list = null;
				if (!string.IsNullOrEmpty(@string))
				{
					list = (Json.Deserialize(@string) as List<object>);
				}
				if (list != null)
				{
					foreach (object obj in list)
					{
						BalanceController._keysInappBonusActionGiven.Add(obj.ToString());
					}
				}
			}
			return BalanceController._keysInappBonusActionGiven;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0000B184 File Offset: 0x00009384
	public static void AddKeysInappBonusActionGiven(string _key)
	{
		if (!BalanceController.keysInappBonusActionGiven.Contains(_key))
		{
			BalanceController.keysInappBonusActionGiven.Add(_key);
			Storager.setString(Defs.keysInappBonusGivenkey, Json.Serialize(BalanceController.keysInappBonusActionGiven), false);
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0000B1C4 File Offset: 0x000093C4
	public static List<Dictionary<string, object>> GetCurrentInnapBonus()
	{
		if (!TrainingController.TrainingCompleted || ExperienceController.sharedController.currentLevel < 2 || FriendsController.ServerTime < 1L)
		{
			return null;
		}
		if (BalanceController.countFrameInCache == Time.frameCount)
		{
			return BalanceController.cacheCurrentInnapBonus;
		}
		List<Dictionary<string, object>> list = null;
		foreach (Dictionary<string, object> dictionary in BalanceController.inappsBonus)
		{
			string text = string.Empty;
			if (dictionary.ContainsKey("Start") && dictionary.ContainsKey("id"))
			{
				text = dictionary["id"].ToString() + dictionary["Start"].ToString();
			}
			if (dictionary.ContainsKey("Type") && !BalanceController.keysInappBonusActionGiven.Contains(text))
			{
				DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime(FriendsController.ServerTime);
				object obj = dictionary["Start"];
				object value = dictionary["End"];
				DateTime dateTime = Convert.ToDateTime(obj, CultureInfo.InvariantCulture);
				DateTime dateTime2 = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
				if (dateTime <= currentTimeByUnixTime && currentTimeByUnixTime <= dateTime2)
				{
					string text2 = dictionary["Type"].ToString();
					bool flag = text2 == "packs";
					float num = Convert.ToSingle(dictionary["priceGems"]);
					float num2 = Convert.ToSingle(dictionary["priceCoins"]);
					int num3 = Convert.ToInt32(dictionary["Real"]);
					float num4 = -1f * (float)num3;
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					string text3 = Convert.ToString(dictionary["action"]);
					dictionary2.Add("action", text3);
					dictionary2.Add("Key", text);
					dictionary2.Add("End", Mathf.RoundToInt((float)dateTime2.Subtract(currentTimeByUnixTime).TotalSeconds));
					dictionary2.Add("ID", dictionary["id"]);
					object value2;
					if (dictionary.TryGetValue("Coins", out value2))
					{
						int num5 = Convert.ToInt32(value2);
						dictionary2.Add("Coins", value2);
						num4 += (float)num5 * num2;
					}
					object value3;
					if (dictionary.TryGetValue("GemsCurrency", out value3))
					{
						int num6 = Convert.ToInt32(value3);
						dictionary2.Add("GemsCurrency", value3);
						num4 += (float)num6 * num2;
					}
					if (flag)
					{
						int fullPack = Convert.ToInt32(dictionary["Pack"]);
						dictionary2.Add("Pack", BalanceController.GetCurrentPack(text, dateTime, currentTimeByUnixTime, dateTime2, fullPack));
					}
					dictionary2.Add("Type", text2);
					object obj2 = null;
					if (dictionary.TryGetValue("Count", out obj2))
					{
						dictionary2.Add("Count", obj2);
					}
					dictionary2.Add("isGems", dictionary["isGems"]);
					if (text3 == BalanceController.curencyActionName)
					{
						float num7 = num4 / (float)num3;
						dictionary2.Add("Profit", num7);
					}
					else if (text3 == BalanceController.petActionName)
					{
						string value4 = string.Empty;
						if (dictionary.ContainsKey("Ids"))
						{
							value4 = BalanceController.GetCurentPetID(obj.ToString(), dictionary["Ids"] as List<object>);
						}
						int num8 = 0;
						if (!dictionary.ContainsKey("Count") || int.TryParse(obj2.ToString(), out num8))
						{
						}
						if (string.IsNullOrEmpty(value4) || num8 == 0)
						{
							continue;
						}
						dictionary2.Add("Pet", value4);
						dictionary2.Add("Quantity", num8);
					}
					else if (text3 == BalanceController.weaponActionName)
					{
						string text4 = string.Empty;
						if (dictionary.ContainsKey("Ids"))
						{
							text4 = BalanceController.GetCurentWeaponID(obj.ToString(), dictionary["Ids"] as List<object>);
						}
						if (string.IsNullOrEmpty(text4))
						{
							continue;
						}
						ItemPrice itemPrice = ShopNGUIController.GetItemPrice(text4, ShopNGUIController.CategoryNames.SpecilCategory, false, true, false);
						if (itemPrice.Currency == "Coins")
						{
							num4 += (float)itemPrice.Price * num2;
						}
						else
						{
							num4 += (float)itemPrice.Price * num;
						}
						float num9 = num4 / (float)num3;
						dictionary2.Add("Profit", num9);
						dictionary2.Add("Weapon", text4);
					}
					else if (text3 == BalanceController.leprechaunActionName)
					{
						int num10 = Convert.ToInt32(dictionary["AddBonusCount"]) * Convert.ToInt32(obj2);
						if (Convert.ToString(dictionary["AddBonusCurrency"]) == "Coins")
						{
							num4 += (float)num10 * num2;
						}
						else
						{
							num4 += (float)num10 * num;
						}
						float num11 = num4 / (float)num3;
						dictionary2.Add("Profit", num11);
						dictionary2.Add("CurrencyLeprechaun", dictionary["AddBonusCurrency"]);
						dictionary2.Add("PerDayLeprechaun", dictionary["AddBonusCount"]);
						dictionary2.Add("DaysLeprechaun", obj2);
					}
					else if (text3 == BalanceController.gadgetActionName)
					{
						List<string> curentGadgetesIDs = BalanceController.GetCurentGadgetesIDs(obj.ToString(), dictionary["Ids"] as List<object>);
						if (curentGadgetesIDs == null)
						{
							continue;
						}
						for (int i = 0; i < curentGadgetesIDs.Count; i++)
						{
							ItemPrice itemPrice2 = ShopNGUIController.GetItemPrice(curentGadgetesIDs[i], ShopNGUIController.CategoryNames.SupportCategory, false, true, false);
							if (itemPrice2.Currency == "Coins")
							{
								num4 += (float)itemPrice2.Price * num2;
							}
							else
							{
								num4 += (float)itemPrice2.Price * num;
							}
						}
						float num12 = num4 / (float)num3;
						dictionary2.Add("Profit", num12);
						dictionary2.Add("Gadgets", curentGadgetesIDs);
					}
					if (list == null)
					{
						list = new List<Dictionary<string, object>>();
					}
					list.Add(dictionary2);
				}
			}
		}
		BalanceController.cacheCurrentInnapBonus = list;
		BalanceController.countFrameInCache = Time.frameCount;
		return list;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000B854 File Offset: 0x00009A54
	private static int GetCurrentPack(string keyAction, DateTime start, DateTime now, DateTime end, int fullPack)
	{
		if (end.Subtract(now).TotalSeconds == 0.0)
		{
			return 0;
		}
		if (!BalanceController.timeNextUpdateDict.ContainsKey(keyAction))
		{
			BalanceController.timeNextUpdateDict.Add(keyAction, DateTime.MinValue);
		}
		if (!BalanceController.curPackDict.ContainsKey(keyAction))
		{
			BalanceController.curPackDict.Add(keyAction, -1);
		}
		DateTime dateTime = BalanceController.timeNextUpdateDict[keyAction];
		int num = BalanceController.curPackDict[keyAction];
		if (num == -1 || now.Subtract(dateTime).TotalSeconds > 15.0)
		{
			num = BalanceController.CurPackForDate(start, now, end, fullPack);
			BalanceController.timeNextUpdateDict[keyAction] = now + TimeSpan.FromSeconds((double)UnityEngine.Random.Range(3, 7));
			BalanceController.curPackDict[keyAction] = num;
			return num;
		}
		if (now > dateTime)
		{
			int num2 = UnityEngine.Random.Range(3, 7);
			DateTime now2 = now + TimeSpan.FromSeconds((double)num2);
			int num3 = BalanceController.CurPackForDate(start, now2, end, fullPack);
			if (num > num3)
			{
				int num4 = UnityEngine.Random.Range(0, num - num3);
				num -= num4;
			}
			else
			{
				num = num3;
			}
			if (num < 1)
			{
				num = 1;
			}
			BalanceController.curPackDict[keyAction] = num;
			BalanceController.timeNextUpdateDict[keyAction] = now + TimeSpan.FromSeconds((double)num2);
		}
		return num;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x0000B9B0 File Offset: 0x00009BB0
	private static int CurPackForDate(DateTime start, DateTime now, DateTime end, int fullPack)
	{
		double totalSeconds = end.Subtract(now).TotalSeconds;
		double totalSeconds2 = end.Subtract(start).TotalSeconds;
		double num = totalSeconds / totalSeconds2;
		return (int)(num * (double)fullPack);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000B9EC File Offset: 0x00009BEC
	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			base.Invoke("UpdateBalansFromServer", 1f);
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x0000BA04 File Offset: 0x00009C04
	private void OnDestroy()
	{
		BalanceController.sharedController = null;
	}

	// Token: 0x040000C1 RID: 193
	public static string curencyActionName = "currence";

	// Token: 0x040000C2 RID: 194
	public static string leprechaunActionName = "leprechaun";

	// Token: 0x040000C3 RID: 195
	public static string petActionName = "pet";

	// Token: 0x040000C4 RID: 196
	public static string weaponActionName = "weapon";

	// Token: 0x040000C5 RID: 197
	public static string gadgetActionName = "gadget";

	// Token: 0x040000C6 RID: 198
	public static List<string> supportedInappBonusIds = new List<string>
	{
		BalanceController.curencyActionName,
		BalanceController.petActionName,
		BalanceController.weaponActionName,
		BalanceController.gadgetActionName,
		BalanceController.leprechaunActionName
	};

	// Token: 0x040000C7 RID: 199
	[HideInInspector]
	public string jsonConfig;

	// Token: 0x040000C8 RID: 200
	public static BalanceController sharedController = null;

	// Token: 0x040000C9 RID: 201
	[SerializeField]
	private string encryptedPlayerprefsKey;

	// Token: 0x040000CA RID: 202
	public static readonly string balanceKey = "balanceKey";

	// Token: 0x040000CB RID: 203
	public static Dictionary<string, float[]> dpsWeapons = new Dictionary<string, float[]>();

	// Token: 0x040000CC RID: 204
	public static Dictionary<string, float[]> damageWeapons = new Dictionary<string, float[]>();

	// Token: 0x040000CD RID: 205
	public static Dictionary<string, int> survivalDamageWeapons = new Dictionary<string, int>();

	// Token: 0x040000CE RID: 206
	private static Dictionary<string, List<ItemPrice>> _gunPricesFromServerNew = new Dictionary<string, List<ItemPrice>>();

	// Token: 0x040000CF RID: 207
	public static Dictionary<string, float> damageGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D0 RID: 208
	public static Dictionary<string, float> amplificationGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D1 RID: 209
	public static Dictionary<string, float> dpsGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D2 RID: 210
	public static Dictionary<string, int> survivalDamageGadgetes = new Dictionary<string, int>();

	// Token: 0x040000D3 RID: 211
	public static Dictionary<string, float> cooldownGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D4 RID: 212
	public static Dictionary<string, float> durationGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D5 RID: 213
	public static Dictionary<string, float> durabilityGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D6 RID: 214
	public static Dictionary<string, float> healGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D7 RID: 215
	public static Dictionary<string, float> hpsGadgetes = new Dictionary<string, float>();

	// Token: 0x040000D8 RID: 216
	public static Dictionary<string, float> damagePets = new Dictionary<string, float>();

	// Token: 0x040000D9 RID: 217
	public static Dictionary<string, int> dpsPets = new Dictionary<string, int>();

	// Token: 0x040000DA RID: 218
	public static Dictionary<string, int> survivalDamagePets = new Dictionary<string, int>();

	// Token: 0x040000DB RID: 219
	public static Dictionary<string, float> respawnTimePets = new Dictionary<string, float>();

	// Token: 0x040000DC RID: 220
	public static Dictionary<string, float> speedPets = new Dictionary<string, float>();

	// Token: 0x040000DD RID: 221
	public static Dictionary<string, int> cashbackPets = new Dictionary<string, int>();

	// Token: 0x040000DE RID: 222
	public static Dictionary<string, float> hpPets = new Dictionary<string, float>();

	// Token: 0x040000DF RID: 223
	public static Dictionary<string, int> timeEggs = new Dictionary<string, int>();

	// Token: 0x040000E0 RID: 224
	public static Dictionary<string, int> victoriasEggs = new Dictionary<string, int>();

	// Token: 0x040000E1 RID: 225
	public static Dictionary<string, int> ratingEggs = new Dictionary<string, int>();

	// Token: 0x040000E2 RID: 226
	public static Dictionary<string, List<EggPetInfo>> rarityPetsInEggs = new Dictionary<string, List<EggPetInfo>>();

	// Token: 0x040000E3 RID: 227
	public static List<Dictionary<string, object>> inappsBonus = new List<Dictionary<string, object>>();

	// Token: 0x040000E4 RID: 228
	private static Dictionary<string, List<ItemPrice>> _gadgetPricesFromServerNew = new Dictionary<string, List<ItemPrice>>();

	// Token: 0x040000E5 RID: 229
	public static int startCapitalCoins = 0;

	// Token: 0x040000E6 RID: 230
	public static int startCapitalGems = 0;

	// Token: 0x040000E7 RID: 231
	public static bool startCapitalEnabled = false;

	// Token: 0x040000E8 RID: 232
	public static ItemPrice competitionAward = new ItemPrice(0, "Coins");

	// Token: 0x040000E9 RID: 233
	public static int countPlaceAwardInCompetion = 0;

	// Token: 0x040000EA RID: 234
	private static Dictionary<string, ItemPrice> _pricesFromServer = new Dictionary<string, ItemPrice>();

	// Token: 0x040000EB RID: 235
	private static string _inappObj = null;

	// Token: 0x040000EC RID: 236
	private static string _inappObjBonus = null;

	// Token: 0x040000ED RID: 237
	private static List<string> _curentGadgetesIDs = null;

	// Token: 0x040000EE RID: 238
	private static List<string> _keysInappBonusActionGiven = null;

	// Token: 0x040000EF RID: 239
	private static List<Dictionary<string, object>> cacheCurrentInnapBonus = null;

	// Token: 0x040000F0 RID: 240
	private static int countFrameInCache = -1;

	// Token: 0x040000F1 RID: 241
	private static Dictionary<string, int> curPackDict = new Dictionary<string, int>();

	// Token: 0x040000F2 RID: 242
	private static Dictionary<string, DateTime> timeNextUpdateDict = new Dictionary<string, DateTime>();

	// Token: 0x040000F3 RID: 243
	private EncryptedPlayerPrefs _encryptedPlayerPrefs;
}
