using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.amazon.device.iap.cpt;
using Prime31;
using Rilisoft;
using UnityEngine;

// Token: 0x02000793 RID: 1939
internal sealed class coinsShop : MonoBehaviour
{
	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x0600458C RID: 17804 RVA: 0x00178350 File Offset: 0x00176550
	// (set) Token: 0x0600458D RID: 17805 RVA: 0x00178358 File Offset: 0x00176558
	public string notEnoughCurrency { get; set; }

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x0600458E RID: 17806 RVA: 0x00178364 File Offset: 0x00176564
	public bool ProductPurchasedRecently
	{
		get
		{
			return Time.realtimeSinceStartup - this._timeWhenPurchShown <= 1.25f;
		}
	}

	// Token: 0x0600458F RID: 17807 RVA: 0x0017837C File Offset: 0x0017657C
	private void HandleQueryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (skus.Any((GoogleSkuInfo s) => s.productId == "skinsmaker"))
		{
			return;
		}
		string[] value = (from sku in skus
		select sku.productId).ToArray<string>();
		string arg = string.Join(", ", value);
		string message = string.Format("Google: Query inventory succeeded;\tPurchases count: {0}, Skus: [{1}]", purchases.Count, arg);
		Debug.Log(message);
		this.productsReceived = true;
	}

	// Token: 0x06004590 RID: 17808 RVA: 0x0017840C File Offset: 0x0017660C
	private void HandleItemDataRequestFinishedEvent(GetProductDataResponse response)
	{
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning("Amazon GetProductDataResponse (CoinsShop): " + response.Status);
			return;
		}
		Debug.Log("Amazon GetProductDataResponse (CoinsShop): " + response.ToJson());
		this.productsReceived = true;
	}

	// Token: 0x06004591 RID: 17809 RVA: 0x00178464 File Offset: 0x00176664
	private void OnEnable()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.AddPurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEvent));
		}
		else
		{
			GoogleIABManager.purchaseSucceededEvent += this.HandlePurchaseSucceededEvent;
		}
		if (Application.loadedLevelName != "Loading")
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
		this.currenciesBought.Clear();
	}

	// Token: 0x06004592 RID: 17810 RVA: 0x001784D0 File Offset: 0x001766D0
	private void OnDisable()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEvent));
		}
		else
		{
			GoogleIABManager.purchaseSucceededEvent -= this.HandlePurchaseSucceededEvent;
		}
		ActivityIndicator.IsActiveIndicator = false;
		this.currenciesBought.Clear();
	}

	// Token: 0x06004593 RID: 17811 RVA: 0x00178528 File Offset: 0x00176728
	private void OnDestroy()
	{
		coinsShop.thisScript = null;
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.RemoveGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
		}
		else
		{
			GoogleIABManager.queryInventorySucceededEvent -= this.HandleQueryInventorySucceededEvent;
		}
	}

	// Token: 0x06004594 RID: 17812 RVA: 0x00178568 File Offset: 0x00176768
	private void HandlePurchaseSuccessfullCore()
	{
		this._timeWhenPurchShown = Time.realtimeSinceStartup;
	}

	// Token: 0x06004595 RID: 17813 RVA: 0x00178578 File Offset: 0x00176778
	private void HandlePurchaseSucceededEvent(GooglePurchase purchase)
	{
		this.HandlePurchaseSuccessfullCore();
	}

	// Token: 0x06004596 RID: 17814 RVA: 0x00178580 File Offset: 0x00176780
	private void HandlePurchaseSuccessfulEvent(PurchaseResponse response)
	{
		string message = "Amazon PurchaseResponse (CoinsShop): " + response.Status;
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(message);
			return;
		}
		Debug.Log(message);
		this.HandlePurchaseSuccessfullCore();
	}

	// Token: 0x06004597 RID: 17815 RVA: 0x001785C8 File Offset: 0x001767C8
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.notEnoughCurrency = null;
		if (Application.isEditor)
		{
			this.productsReceived = true;
		}
		coinsShop.thisScript = this;
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.AddGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
		}
		else
		{
			GoogleIABManager.queryInventorySucceededEvent += this.HandleQueryInventorySucceededEvent;
		}
		this.RefreshProductsIfNeed(false);
	}

	// Token: 0x06004598 RID: 17816 RVA: 0x0017863C File Offset: 0x0017683C
	public static void TryToFireCurrenciesAddEvent(string currency)
	{
		try
		{
			CoinsMessage.FireCoinsAddedEvent(currency == "GemsCurrency", 2);
		}
		catch (Exception arg)
		{
			Debug.LogError("coinsShop.TryToFireCurrenciesAddEvent: FireCoinsAddedEvent( currency == Defs.Gems ): " + arg);
		}
	}

	// Token: 0x06004599 RID: 17817 RVA: 0x00178694 File Offset: 0x00176894
	public void HandlePurchaseButton(int i, string currency, AbstractBankViewItem item)
	{
		ButtonClickSound.Instance.PlayClick();
		if ((currency.Equals("Coins") && (i >= StoreKitEventListener.coinIds.Length || i >= VirtualCurrencyHelper.coinInappsQuantity.Length)) || (currency.Equals("GemsCurrency") && (i >= StoreKitEventListener.gemsIds.Length || i >= VirtualCurrencyHelper.gemsInappsQuantity.Length)))
		{
			Debug.LogWarning("Index of purchase is out of range: " + i);
			return;
		}
		string text = string.Empty;
		if ("Coins" == currency)
		{
			text = StoreKitEventListener.coinIds[i];
		}
		else
		{
			if (!("GemsCurrency" == currency))
			{
				Debug.LogError("HandlePurchaseButton: Unknown currency: " + currency);
				return;
			}
			text = StoreKitEventListener.gemsIds[i];
		}
		if (InappBonuessController.Instance.InappBonusAlreadyBought(item.InappBonusParameters))
		{
			return;
		}
		this.currenciesBought.Add(currency);
		StoreKitEventListener.purchaseInProcess = true;
		InappBonuessController.Instance.RememberCurrentBonusForInapp(text, item.InappBonusParameters);
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			SkuInput skuInput = new SkuInput
			{
				Sku = text
			};
			Debug.Log("Amazon Purchase (HandlePurchaseButton): " + skuInput.ToJson());
			AmazonIapV2Impl.Instance.Purchase(skuInput);
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		}
		else
		{
			coinsShop._etcFileTimestamp = coinsShop.GetHostsTimestamp();
			AnalyticsFacade.SendCustomEventToAppsFlyer("af_initiated_checkout", new Dictionary<string, string>
			{
				{
					"af_content_id",
					text
				}
			});
			GoogleIAB.purchaseProduct(text);
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		}
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x00178824 File Offset: 0x00176A24
	public static void showCoinsShop()
	{
		coinsShop.thisScript.enabled = true;
		coinsPlashka.hideButtonCoins = true;
		coinsPlashka.showPlashka();
	}

	// Token: 0x0600459B RID: 17819 RVA: 0x0017883C File Offset: 0x00176A3C
	public static void hideCoinsShop()
	{
		if (coinsShop.thisScript != null)
		{
			coinsShop.thisScript.enabled = false;
			coinsShop.thisScript.notEnoughCurrency = null;
			Resources.UnloadUnusedAssets();
		}
	}

	// Token: 0x0600459C RID: 17820 RVA: 0x00178878 File Offset: 0x00176A78
	public static void ExitFromShop(bool performOnExitActs)
	{
		coinsShop.hideCoinsShop();
		if (coinsShop.showPlashkuPriExit)
		{
			coinsPlashka.hidePlashka();
		}
		coinsPlashka.hideButtonCoins = false;
		if (!performOnExitActs)
		{
			return;
		}
		if (coinsShop.thisScript.onReturnAction != null && coinsShop.thisScript.notEnoughCurrency != null && coinsShop.thisScript.currenciesBought.Contains(coinsShop.thisScript.notEnoughCurrency))
		{
			coinsShop.thisScript.currenciesBought.Clear();
			coinsShop.thisScript.onReturnAction();
		}
		else
		{
			coinsShop.thisScript.onReturnAction = null;
		}
		if (coinsShop.thisScript.onResumeFronNGUI != null)
		{
			coinsShop.thisScript.onResumeFronNGUI();
			coinsShop.thisScript.onResumeFronNGUI = null;
			coinsPlashka.hidePlashka();
		}
	}

	// Token: 0x0600459D RID: 17821 RVA: 0x00178940 File Offset: 0x00176B40
	internal static bool CheckAndroidHostsTampering()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			if (!File.Exists("/etc/hosts"))
			{
				return false;
			}
			try
			{
				string[] source = File.ReadAllLines("/etc/hosts");
				IEnumerable<string> source2 = from l in source
				where l.TrimStart(new char[0]).StartsWith("127.")
				select l;
				return source2.Any((string l) => l.Contains("android.clients.google.com") || l.Contains("mtalk.google.com "));
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				return false;
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600459E RID: 17822 RVA: 0x00178A08 File Offset: 0x00176C08
	internal static bool CheckLuckyPatcherInstalled()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		string[] source = new string[]
		{
			"Y29tLmRpbW9udmlkZW8ubHVja3lwYXRjaGVy",
			"Y29tLmNoZWxwdXMubGFja3lwYXRjaA==",
			"Y29tLmZvcnBkYS5scA=="
		};
		IEnumerable<string> source2 = from bytes in source.Select(new Func<string, byte[]>(Convert.FromBase64String))
		where bytes != null
		select Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		return source2.Any(new Func<string, bool>(coinsShop.PackageExists));
	}

	// Token: 0x0600459F RID: 17823 RVA: 0x00178AAC File Offset: 0x00176CAC
	private static bool PackageExists(string packageName)
	{
		if (packageName == null)
		{
			throw new ArgumentNullException("packageName");
		}
		if (Application.isEditor)
		{
			return false;
		}
		try
		{
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			if (currentActivity == null)
			{
				Debug.LogWarning("activity == null");
				return false;
			}
			AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				Debug.LogWarning("manager == null");
				return false;
			}
			if (androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				packageName,
				0
			}) == null)
			{
				Debug.LogWarning("packageInfo == null");
				return false;
			}
			return true;
		}
		catch (Exception arg)
		{
			if (coinsShop._loggedPackages.Contains(packageName))
			{
				return false;
			}
			string message = string.Format("Error while retrieving Android package info:    {0}", arg);
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogWarning(message);
				coinsShop._loggedPackages.Add(packageName);
			}
		}
		return false;
	}

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x060045A0 RID: 17824 RVA: 0x00178BD0 File Offset: 0x00176DD0
	public static bool IsStoreAvailable
	{
		get
		{
			return !coinsShop.IsWideLayoutAvailable && !coinsShop.IsNoConnection;
		}
	}

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x060045A1 RID: 17825 RVA: 0x00178BE8 File Offset: 0x00176DE8
	public static bool IsWideLayoutAvailable
	{
		get
		{
			return coinsShop.CheckAndroidHostsTampering() || coinsShop.CheckLuckyPatcherInstalled() || coinsShop.IsBangerrySupported() || coinsShop.HasTamperedProducts;
		}
	}

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x060045A2 RID: 17826 RVA: 0x00178C1C File Offset: 0x00176E1C
	// (set) Token: 0x060045A3 RID: 17827 RVA: 0x00178C24 File Offset: 0x00176E24
	internal static bool HasTamperedProducts { private get; set; }

	// Token: 0x060045A4 RID: 17828 RVA: 0x00178C2C File Offset: 0x00176E2C
	private static string ConvertFromBase64(string s)
	{
		byte[] array = Convert.FromBase64String(s);
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	// Token: 0x060045A5 RID: 17829 RVA: 0x00178C50 File Offset: 0x00176E50
	private static bool IsBangerrySupported()
	{
		bool result;
		try
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
			{
				result = false;
			}
			else
			{
				string path = coinsShop.ConvertFromBase64("L0xpYnJhcnkvTW9iaWxlU3Vic3RyYXRlL0R5bmFtaWNMaWJyYXJpZXM=");
				if (File.Exists(Path.Combine(path, coinsShop.ConvertFromBase64("TG9jYWxJQVBTdG9yZS5keWxpYg=="))) || File.Exists(Path.Combine(path, coinsShop.ConvertFromBase64("TG9jYWxsQVBTdG9yZS5keWxpYg=="))))
				{
					Debug.LogWarningFormat("{0}: `cetrer`", new object[]
					{
						"IsBangerrySupported"
					});
					result = true;
				}
				else if (File.Exists(Path.Combine(path, coinsShop.ConvertFromBase64("aWFwLmR5bGli"))))
				{
					Debug.LogWarningFormat("{0}: `panemer`", new object[]
					{
						"IsBangerrySupported"
					});
					result = true;
				}
				else if (File.Exists(Path.Combine(path, coinsShop.ConvertFromBase64("aWFwZnJlZS5jb3JlLmR5bGli"))) || File.Exists(Path.Combine(path, coinsShop.ConvertFromBase64("SUFQRnJlZVNlcnZpY2UuZHlsaWI="))))
				{
					Debug.LogWarningFormat("{0}: `rastat`", new object[]
					{
						"IsBangerrySupported"
					});
					result = true;
				}
				else
				{
					result = false;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarningFormat("Exception in {0}: {1}", new object[]
			{
				"IsBangerrySupported",
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x060045A6 RID: 17830 RVA: 0x00178DAC File Offset: 0x00176FAC
	private static DateTime? GetHostsTimestamp()
	{
		DateTime? result;
		try
		{
			Debug.Log("Trying to get /ets/hosts timestamp...");
			FileInfo fileInfo = new FileInfo("/etc/hosts");
			DateTime lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
			Debug.Log("/ets/hosts timestamp: " + lastWriteTimeUtc.ToString("s"));
			result = new DateTime?(lastWriteTimeUtc);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			result = null;
		}
		return result;
	}

	// Token: 0x060045A7 RID: 17831 RVA: 0x00178E3C File Offset: 0x0017703C
	internal static bool CheckHostsTimestamp()
	{
		if (coinsShop._etcFileTimestamp != null)
		{
			DateTime? hostsTimestamp = coinsShop.GetHostsTimestamp();
			if (hostsTimestamp != null && coinsShop._etcFileTimestamp.Value != hostsTimestamp.Value)
			{
				Debug.LogError(string.Format("Timestamp check failed: {0:s} expcted, but actual value is {1:s}.", coinsShop._etcFileTimestamp.Value, hostsTimestamp.Value));
				return false;
			}
		}
		return true;
	}

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x060045A8 RID: 17832 RVA: 0x00178EB4 File Offset: 0x001770B4
	public static bool IsBillingSupported
	{
		get
		{
			return Application.isEditor || StoreKitEventListener.billingSupported;
		}
	}

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x060045A9 RID: 17833 RVA: 0x00178EC8 File Offset: 0x001770C8
	public static bool IsNoConnection
	{
		get
		{
			if (coinsShop.thisScript == null)
			{
				return true;
			}
			if (!coinsShop.thisScript.productsReceived)
			{
				return true;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return StoreKitEventListener.Instance == null || StoreKitEventListener.Instance.Products.Count<IMarketProduct>() <= 0;
			}
			return !coinsShop.IsBillingSupported;
		}
	}

	// Token: 0x060045AA RID: 17834 RVA: 0x00178F34 File Offset: 0x00177134
	public void RefreshProductsIfNeed(bool force = false)
	{
		if (!this.productsReceived || force)
		{
			StoreKitEventListener.RefreshProducts();
		}
	}

	// Token: 0x060045AB RID: 17835 RVA: 0x00178F4C File Offset: 0x0017714C
	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.RefreshProductsIfNeed(false);
		}
	}

	// Token: 0x0400330C RID: 13068
	public static coinsShop thisScript;

	// Token: 0x0400330D RID: 13069
	public static bool showPlashkuPriExit = false;

	// Token: 0x0400330E RID: 13070
	public Action onReturnAction;

	// Token: 0x0400330F RID: 13071
	private float _timeWhenPurchShown = float.MinValue;

	// Token: 0x04003310 RID: 13072
	private List<string> currenciesBought = new List<string>();

	// Token: 0x04003311 RID: 13073
	private bool productsReceived;

	// Token: 0x04003312 RID: 13074
	public Action onResumeFronNGUI;

	// Token: 0x04003313 RID: 13075
	private static readonly HashSet<string> _loggedPackages = new HashSet<string>();

	// Token: 0x04003314 RID: 13076
	private static DateTime? _etcFileTimestamp;

	// Token: 0x04003315 RID: 13077
	private Action _drawInnerInterface;
}
