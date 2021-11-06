using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using com.amazon.device.iap.cpt;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200076B RID: 1899
public sealed class StoreKitEventListener : MonoBehaviour
{
	// Token: 0x060042B6 RID: 17078 RVA: 0x001625FC File Offset: 0x001607FC
	static StoreKitEventListener()
	{
		StoreKitEventListener.billingSupported = false;
		StoreKitEventListener.coinIds = new string[]
		{
			"coin1",
			"coin7",
			"coin2",
			"coin3.",
			"coin4",
			"coin5",
			"coin8",
			"coin9"
		};
		StoreKitEventListener._productIds = new string[]
		{
			"bigammopack",
			"Fullhealth",
			"crystalsword",
			"MinerWeapon",
			StoreKitEventListener.elixirID
		};
		StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun = new List<string>();
		StoreKitEventListener.idsForSingle = new string[]
		{
			"bigammopack",
			"Fullhealth",
			"ironSword",
			"MinerWeapon",
			"steelAxe",
			"spas",
			StoreKitEventListener.elixirID,
			"glock",
			"chainsaw",
			"scythe",
			"shovel"
		};
		StoreKitEventListener.idsForMulti = new string[]
		{
			StoreKitEventListener.idsForSingle[2],
			StoreKitEventListener.idsForSingle[3],
			"steelAxe",
			"woodenBow",
			"combatrifle",
			"spas",
			"goldeneagle",
			StoreKitEventListener.idsForSingle[7],
			StoreKitEventListener.idsForSingle[8],
			"famas"
		};
		StoreKitEventListener.idsForFull = new string[]
		{
			StoreKitEventListener.fullVersion
		};
		StoreKitEventListener.categoriesMulti = new string[][]
		{
			new string[]
			{
				StoreKitEventListener.idsForSingle[0],
				StoreKitEventListener.idsForSingle[1],
				StoreKitEventListener.armor,
				StoreKitEventListener.armor2,
				StoreKitEventListener.armor3
			},
			PotionsController.potions
		};
		StoreKitEventListener.categoriesSingle = StoreKitEventListener.categoriesMulti;
	}

	// Token: 0x060042B7 RID: 17079 RVA: 0x00162B0C File Offset: 0x00160D0C
	public static decimal GetPriceFromPriceAmountMicros(long priceAmountMicros)
	{
		decimal d = priceAmountMicros;
		decimal d2 = 1000000m;
		return decimal.Divide(d, d2);
	}

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x060042B8 RID: 17080 RVA: 0x00162B34 File Offset: 0x00160D34
	internal ICollection<IMarketProduct> Products
	{
		get
		{
			return this._products;
		}
	}

	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x060042B9 RID: 17081 RVA: 0x00162B3C File Offset: 0x00160D3C
	public Task<AmazonUserData> AmazonUser
	{
		get
		{
			return this._amazonUserPromise.Task;
		}
	}

	// Token: 0x060042BA RID: 17082 RVA: 0x00162B4C File Offset: 0x00160D4C
	private void Start()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (Application.isEditor && !this._products.Any<IMarketProduct>())
			{
				this.InitializeTestProductsAmazon();
			}
			else
			{
				List<string> skus = StoreKitEventListener.coinIds.Concat(StoreKitEventListener.gemsIds).ToList<string>();
				SkusInput skusInput = new SkusInput
				{
					Skus = skus
				};
				Debug.Log("Amazon GetProductData (StoreKitEventListener.Start): " + skusInput.ToJson());
				AmazonIapV2Impl.Instance.GetProductData(skusInput);
			}
		}
		else if (Application.isEditor)
		{
			this.InitializeTestProductsGoogle();
		}
		else
		{
			string publicKey = (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite) ? string.Empty : "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoTzMTaqsFhaywvCFKawFwL5KM+djLJfOCT/rbGQRfHmHYmOY2sBMgDWsA/67Szx6EVTZPVlFzHMgkAq1TwdL/A5aYGpGzaCX7o96cyp8R6wSF+xCuj++LAkTaDnLW0veI2bke3EVHu3At9xgM46e+VDucRUqQLvf6SQRb15nuflY5i08xKnewgX7I4U2H0RvAZDyoip+qZPmI4ZvaufAfc0jwZbw7XGiV41zibY3LU0N57mYKk51Wx+tOaJ7Tkc9Rl1qVCTjb+bwXshTqhVXVP6r4kabLWw/8OJUh0Sm69lbps6amP7vPy571XjscCTMLfXQan1959rHbNgkb2mLLQIDAQAB";
			GoogleIAB.init(publicKey);
			GoogleIAB.setAutoVerifySignatures(false);
			if (Defs.IsDeveloperBuild)
			{
				GoogleIAB.enableLogging(true);
			}
		}
	}

	// Token: 0x060042BB RID: 17083 RVA: 0x00162C24 File Offset: 0x00160E24
	private void OnEnable()
	{
		this._purchaseFailedSubscription.Dispose();
		Action<string, int> googlePurchaseFailedHandler = delegate(string error, int response)
		{
			StoreKitEventListener.purchaseInProcess = false;
			Debug.LogWarning(string.Format("googlePurchaseFailedHandler({0}): {1}", response, error));
		};
		this._purchaseFailedSubscription = new ActionDisposable(delegate()
		{
			GoogleIABManager.purchaseFailedEvent -= googlePurchaseFailedHandler;
		});
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.AddGetUserDataResponseListener(new GetUserDataResponseDelegate(this.HandleGetUserIdResponseEvent));
			AmazonIapV2Impl.Instance.AddGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
			AmazonIapV2Impl.Instance.AddPurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEventAmazon));
			AmazonIapV2Impl.Instance.AddGetPurchaseUpdatesResponseListener(new GetPurchaseUpdatesResponseDelegate(this.HandlePurchaseUpdatesRequestSuccessfulEvent));
			this.HandleAmazonSdkAvailableEvent(false);
			Debug.Log("Amazon GetUserData (StoreKitEventListener.OnEnable)");
			AmazonIapV2Impl.Instance.GetUserData();
		}
		else
		{
			GoogleIABManager.billingSupportedEvent += this.billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += this.billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent += this.queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += this.queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += this.purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent += this.HandleGooglePurchaseSucceeded;
			GoogleIABManager.purchaseFailedEvent += googlePurchaseFailedHandler;
			GoogleIABManager.consumePurchaseSucceededEvent += this.consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent += this.consumePurchaseFailedEvent;
		}
	}

	// Token: 0x060042BC RID: 17084 RVA: 0x00162D94 File Offset: 0x00160F94
	private void OnDisable()
	{
		this._purchaseFailedSubscription.Dispose();
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.RemoveGetUserDataResponseListener(new GetUserDataResponseDelegate(this.HandleGetUserIdResponseEvent));
			AmazonIapV2Impl.Instance.RemoveGetProductDataResponseListener(new GetProductDataResponseDelegate(this.HandleItemDataRequestFinishedEvent));
			AmazonIapV2Impl.Instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessfulEventAmazon));
			AmazonIapV2Impl.Instance.RemoveGetPurchaseUpdatesResponseListener(new GetPurchaseUpdatesResponseDelegate(this.HandlePurchaseUpdatesRequestSuccessfulEvent));
		}
		else
		{
			GoogleIABManager.billingSupportedEvent -= this.billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent -= this.billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent -= this.queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= this.queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= this.purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent -= this.HandleGooglePurchaseSucceeded;
			GoogleIABManager.consumePurchaseSucceededEvent -= this.consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent -= this.consumePurchaseFailedEvent;
		}
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x00162E9C File Offset: 0x0016109C
	private void billingSupportedEvent()
	{
		StoreKitEventListener.billingSupported = true;
		Debug.Log("billingSupportedEvent");
		StoreKitEventListener.RefreshProducts();
	}

	// Token: 0x060042BE RID: 17086 RVA: 0x00162EB4 File Offset: 0x001610B4
	public static void RefreshProducts()
	{
		if (!StoreKitEventListener.billingSupported && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			return;
		}
		IEnumerable<string> source = StoreKitEventListener._productIds.Concat(StoreKitEventListener.coinIds).Concat(StoreKitEventListener.gemsIds).Concat(StoreKitEventListener.starterPackIds);
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			SkusInput skusInput = new SkusInput
			{
				Skus = source.ToList<string>()
			};
			Debug.Log("Amazon GetProductData (RefreshProducts): " + skusInput.ToJson());
			AmazonIapV2Impl.Instance.GetProductData(skusInput);
		}
		else
		{
			GoogleIAB.queryInventory(source.ToArray<string>());
		}
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x00162F4C File Offset: 0x0016114C
	private void billingNotSupportedEvent(string error)
	{
		StoreKitEventListener.billingSupported = false;
		Debug.LogWarning("billingNotSupportedEvent: " + error);
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x00162F64 File Offset: 0x00161164
	private void HandleAmazonSdkAvailableEvent(bool isSandboxMode)
	{
		Debug.Log("Amazon SDK available in sandbox mode: " + isSandboxMode);
		StoreKitEventListener.billingSupported = true;
		StoreKitEventListener.RefreshProducts();
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x00162F94 File Offset: 0x00161194
	private void HandleGetUserIdResponseEvent(GetUserDataResponse response)
	{
		string message = "Amazon GetUserDataResponse: " + response.Status;
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(message);
			this._amazonUserPromise.TrySetException(new InvalidOperationException(message));
			return;
		}
		Debug.Log(message);
		this._amazonUserPromise.TrySetResult(response.AmazonUserData);
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x00162FFC File Offset: 0x001611FC
	private void AndroidAddCurrencyAndConsume(GooglePurchase purchase)
	{
		string text = null;
		try
		{
			text = InappBonuessController.Instance.GiveBonusForInapp(purchase.productId);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in giving inapp bonus in HandleGooglePurchaseSucceeded: {0}", new object[]
			{
				ex
			});
		}
		this.TryAddVirtualCrrency(purchase.productId, text == null);
		Debug.Log("StoreKitEventListener.AddCurrencyAndConsumeNextGooglePlayPurchase(): Consuming Goole purchase " + purchase.ToString());
		StoreKitEventListener.GooglePlayConsumeAndSave(purchase);
		if (StoreKitEventListener.IsSinglePurchase(purchase))
		{
			this.SendFirstTimePayment(purchase);
		}
		this.LogRealPayment(purchase, text);
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x001630A0 File Offset: 0x001612A0
	private static bool IsSinglePurchase(GooglePurchase purchase)
	{
		if (!Storager.hasKey("Android.GooglePlayOrderIdsKey"))
		{
			return false;
		}
		string @string = Storager.getString("Android.GooglePlayOrderIdsKey", false);
		if (string.IsNullOrEmpty(@string))
		{
			return false;
		}
		List<object> list = Rilisoft.MiniJson.Json.Deserialize(@string) as List<object>;
		return list != null && list.Count == 1 && list.OfType<string>().FirstOrDefault(new Func<string, bool>(purchase.productId.Equals)) != null;
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x00163120 File Offset: 0x00161320
	private void AddCurrencyAndConsumeNextGooglePlayPurchase()
	{
		try
		{
			if (this._cheatedPurchasesToConsume.Count > 0)
			{
				GooglePurchase googlePurchase = this._cheatedPurchasesToConsume.FirstOrDefault((GooglePurchase p) => StoreKitEventListener.IsVirtualCurrency(p.productId));
				if (googlePurchase != null)
				{
					Debug.Log("StoreKitEventListener.AddCurrencyAndConsumeNextGooglePlayPurchase(): Consuming Goole purchase " + googlePurchase.ToString());
					StoreKitEventListener.GooglePlayConsumeAndSave(googlePurchase);
				}
			}
			else
			{
				GooglePurchase googlePurchase = this._purchasesToConsume.FirstOrDefault((GooglePurchase p) => StoreKitEventListener.IsVirtualCurrency(p.productId));
				if (googlePurchase != null)
				{
					string text = string.Empty;
					if (Storager.hasKey("Android.GooglePlayOrderIdsKey"))
					{
						text = Storager.getString("Android.GooglePlayOrderIdsKey", false);
					}
					if (string.IsNullOrEmpty(text))
					{
						text = "[]";
					}
					List<object> source = (Rilisoft.MiniJson.Json.Deserialize(text) as List<object>) ?? new List<object>();
					HashSet<string> hashSet = new HashSet<string>(source.OfType<string>());
					if (!hashSet.Contains(googlePurchase.orderId))
					{
						if (!StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun.Contains(googlePurchase.orderId))
						{
							if (CoroutineRunner.Instance != null)
							{
								CoroutineRunner.Instance.StartCoroutine(this.WaitForX3AndGiveCurrency(googlePurchase, null));
							}
							else
							{
								Debug.LogError("AddCurrencyAndConsumeNextGooglePlayPurchase CoroutineRunner.Instance == null ");
								this.AndroidAddCurrencyAndConsume(googlePurchase);
							}
						}
					}
					else
					{
						Debug.Log("StoreKitEventListener.AddCurrencyAndConsumeNextGooglePlayPurchase(): Consuming Goole purchase " + googlePurchase.ToString());
						StoreKitEventListener.GooglePlayConsumeAndSave(googlePurchase);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"AddCurrencyAndConsumeNextGooglePlayPurchase exception: ",
				ex,
				"\nstacktrace:\n",
				Environment.StackTrace
			}));
		}
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x001632F0 File Offset: 0x001614F0
	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (purchases == null)
		{
			purchases = new List<GooglePurchase>();
		}
		if (skus == null)
		{
			skus = new List<GoogleSkuInfo>();
		}
		this._products.Clear();
		this._purchasesToConsume.Clear();
		this._cheatedPurchasesToConsume.Clear();
		try
		{
			if (!skus.Any((GoogleSkuInfo s) => s.productId == "skinsmaker"))
			{
				string[] productIds = (from sku in skus
				select sku.productId).ToArray<string>();
				string arg = string.Join(", ", productIds);
				string[] value = (from p in purchases
				select string.Format("<{0}, {1}>", p.productId, p.purchaseState)).ToArray<string>();
				string arg2 = string.Join(", ", value);
				string message = string.Format("Google billing. Query inventory succeeded, purchases: [{0}], skus: [{1}]", arg2, arg);
				Debug.Log(message);
				IEnumerable<GoogleMarketProduct> enumerable = (from s in skus
				where productIds.Contains(s.productId)
				select s).Select(new Func<GoogleSkuInfo, GoogleMarketProduct>(MarketProductFactory.CreateGoogleMarketProduct));
				foreach (GoogleMarketProduct googleMarketProduct in enumerable)
				{
					if (googleMarketProduct.Price.Contains("$0.0"))
					{
						Debug.LogWarningFormat("Unexpected price '{0}': '{1}' ('{2}')", new object[]
						{
							googleMarketProduct.Price,
							googleMarketProduct.Id,
							googleMarketProduct.Title
						});
						coinsShop.HasTamperedProducts = true;
					}
					if (!this._products.Contains(googleMarketProduct))
					{
						this._products.Add(googleMarketProduct);
					}
				}
				foreach (GooglePurchase googlePurchase in purchases)
				{
					if (!(googlePurchase.productId == "MinerWeapon") && !(googlePurchase.productId == "MinerWeapon".ToLower()))
					{
						if (!(googlePurchase.productId == "crystalsword"))
						{
							if (StoreKitEventListener.starterPackIds.Contains(googlePurchase.productId))
							{
								try
								{
									StarterPackController.Get.AddBuyAndroidStarterPack(googlePurchase.productId);
									StarterPackController.Get.TryRestoreStarterPack(googlePurchase.productId);
								}
								catch (Exception ex)
								{
									Debug.LogFormat("Exception in queryInventorySucceededEvent starter packs: {0}", new object[]
									{
										ex
									});
								}
							}
							else if (this.VerifyPurchase(googlePurchase.originalJson, googlePurchase.signature))
							{
								this._purchasesToConsume.Add(googlePurchase);
							}
							else
							{
								this._cheatedPurchasesToConsume.Add(googlePurchase);
							}
						}
					}
				}
				this.AddCurrencyAndConsumeNextGooglePlayPurchase();
			}
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
		}
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x00163654 File Offset: 0x00161854
	private void queryInventoryFailedEvent(string error)
	{
		Debug.LogWarning("Google: queryInventoryFailedEvent: " + error);
		base.StartCoroutine(this.WaitAndQueryInventory());
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x00163674 File Offset: 0x00161874
	private IEnumerator WaitAndQueryInventory()
	{
		Debug.LogWarning(string.Format("Waiting {0}s before requering inventory...", 10f));
		yield return new WaitForSeconds(10f);
		Debug.LogWarning(string.Format("Trying to repeat query inventory...", new object[0]));
		string[] products = StoreKitEventListener._productIds.Concat(StoreKitEventListener.coinIds).Concat(StoreKitEventListener.gemsIds).Concat(StoreKitEventListener.starterPackIds).ToArray<string>();
		GoogleIAB.queryInventory(products);
		yield break;
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00163688 File Offset: 0x00161888
	private void HandleItemDataRequestFinishedEvent(GetProductDataResponse response)
	{
		string message = "Amazon: GetProductDataResponse: " + response.Status;
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(message);
			return;
		}
		Debug.Log(message);
		this._products.Clear();
		try
		{
			List<string> obj = response.ProductDataMap.Keys.ToList<string>();
			string arg = Rilisoft.MiniJson.Json.Serialize(obj);
			string arg2 = Rilisoft.MiniJson.Json.Serialize(response.UnavailableSkus);
			string message2 = string.Format("Item data request finished;    Unavailable skus: {0}, Available skus: {1}", arg2, arg);
			Debug.Log(message2);
			IEnumerable<ProductData> enumerable = from item in response.ProductDataMap.Values
			where StoreKitEventListener.coinIds.Contains(item.Sku) || StoreKitEventListener.gemsIds.Contains(item.Sku)
			select item;
			IEnumerable<AmazonMarketProduct> enumerable2 = response.ProductDataMap.Values.Select(new Func<ProductData, AmazonMarketProduct>(MarketProductFactory.CreateAmazonMarketProduct));
			foreach (AmazonMarketProduct item2 in enumerable2)
			{
				if (!this._products.Contains(item2))
				{
					this._products.Add(item2);
				}
			}
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("[Rilisoft] Amazon: calling GetPurchaseUpdates()");
			}
			AmazonIapV2Impl.Instance.GetPurchaseUpdates(new ResetInput
			{
				Reset = true
			});
		}
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x00163824 File Offset: 0x00161A24
	private void HandleItemDataRequestFailedEvent()
	{
		Debug.LogWarning("Amamzon: Item data request failed.");
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x00163830 File Offset: 0x00161A30
	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x00163848 File Offset: 0x00161A48
	private static bool IsVirtualCurrency(string productId)
	{
		if (productId == null)
		{
			return false;
		}
		int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, productId);
		int num2 = Array.IndexOf<string>(StoreKitEventListener.gemsIds, productId);
		return num >= StoreKitEventListener.coinIds.GetLowerBound(0) || num2 >= StoreKitEventListener.gemsIds.GetLowerBound(0);
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x0016389C File Offset: 0x00161A9C
	private bool TryAddVirtualCrrency(string productId, bool shouldAddCurrency)
	{
		if (string.IsNullOrEmpty(productId))
		{
			Debug.LogError("TryAddVirtualCrrency string.IsNullOrEmpty(productId)");
			return false;
		}
		int? num = null;
		int num2 = Array.IndexOf<string>(StoreKitEventListener.coinIds, productId);
		int num3 = Array.IndexOf<string>(StoreKitEventListener.gemsIds, productId);
		if (num2 >= StoreKitEventListener.coinIds.GetLowerBound(0))
		{
			num = new int?(Mathf.RoundToInt((float)VirtualCurrencyHelper.GetCoinInappsQuantity(num2) * PremiumAccountController.VirtualCurrencyMultiplier));
			if (shouldAddCurrency)
			{
				int val = Storager.getInt("Coins", false) + num.Value;
				Storager.setInt("Coins", val, false);
				AnalyticsFacade.CurrencyAccrual(num.Value, "Coins", AnalyticsConstants.AccrualType.Purchased);
			}
			coinsShop.TryToFireCurrenciesAddEvent("Coins");
			try
			{
				ChestBonusController.TryTakeChestBonus(false, num2);
			}
			catch (Exception arg)
			{
				Debug.LogError("TryAddVirtualCrrency ChestBonusController.TryTakeChestBonus exception: " + arg);
			}
		}
		else if (num3 >= StoreKitEventListener.gemsIds.GetLowerBound(0))
		{
			num = new int?(Mathf.RoundToInt((float)VirtualCurrencyHelper.GetGemsInappsQuantity(num3) * PremiumAccountController.VirtualCurrencyMultiplier));
			if (shouldAddCurrency)
			{
				int val2 = Storager.getInt("GemsCurrency", false) + num.Value;
				Storager.setInt("GemsCurrency", val2, false);
				AnalyticsFacade.CurrencyAccrual(num.Value, "GemsCurrency", AnalyticsConstants.AccrualType.Purchased);
			}
			coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
			try
			{
				ChestBonusController.TryTakeChestBonus(true, num3);
			}
			catch (Exception arg2)
			{
				Debug.LogError("TryAddVirtualCrrency ChestBonusController.TryTakeChestBonus exception: " + arg2);
			}
		}
		if (num != null)
		{
			try
			{
				StoreKitEventListener.LogVirtualCurrencyPurchased(productId, num.Value, num3 >= StoreKitEventListener.gemsIds.GetLowerBound(0));
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("TryAddVirtualCrrency ANALYTICS, LogVirtualCurrencyPurchased({0}, {1}) threw exception: {2}", new object[]
				{
					productId,
					num.Value,
					ex
				});
			}
		}
		try
		{
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.SendOurData(false);
			}
		}
		catch (Exception arg3)
		{
			Debug.LogWarning("FriendsController.sharedController.SendOurData " + arg3);
		}
		return num != null;
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x00163B18 File Offset: 0x00161D18
	private bool TryAddStarterPackItem(string productId)
	{
		if (StoreKitEventListener.starterPackIds.Contains(productId))
		{
			bool flag = false;
			try
			{
				flag = StarterPackController.Get.TryTakePurchasesForCurrentPack(productId, false);
			}
			catch (Exception ex)
			{
				Debug.LogFormat("Exception in TryAddStarterPackItem starter packs: {0}", new object[]
				{
					ex
				});
			}
			if (flag)
			{
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
			}
			FriendsController.sharedController.SendOurData(false);
			return flag;
		}
		return false;
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x00163BA0 File Offset: 0x00161DA0
	private void ConsumeProductIfCheating(GooglePurchase purchase)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogError("Consuming cheated purchase: " + purchase.ToString());
		}
		StoreKitEventListener.GooglePlayConsumeAndSave(purchase);
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x00163BC8 File Offset: 0x00161DC8
	private void LogRealPayment(GooglePurchase purchase, string keyOfInappAction)
	{
		if (purchase.purchaseState != GooglePurchase.GooglePurchaseState.Purchased)
		{
			return;
		}
		try
		{
			GoogleSkuInfo googleSkuInfo = this.Products.FirstOrDefault((IMarketProduct p) => (p.PlatformProduct as GoogleSkuInfo).productId == purchase.productId).PlatformProduct as GoogleSkuInfo;
			long priceAmountMicros = googleSkuInfo.priceAmountMicros;
			decimal priceFromPriceAmountMicros = StoreKitEventListener.GetPriceFromPriceAmountMicros(priceAmountMicros);
			string text = purchase.orderId ?? string.Empty;
			AnalyticsFacade.RealPayment(text, (float)priceFromPriceAmountMicros, AnalyticsStuff.ReadableNameForInApp(googleSkuInfo.productId), googleSkuInfo.priceCurrencyCode, false, keyOfInappAction);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.SendAddPurchaseEvent(googleSkuInfo.productId, text, (float)priceFromPriceAmountMicros, googleSkuInfo.priceCurrencyCode, string.Empty);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in RealPayment: " + arg);
		}
		decimal payment;
		if (VirtualCurrencyHelper.ReferencePricesInUsd.TryGetValue(purchase.productId, out payment))
		{
			decimal d = StoreKitEventListener.IncrementAccumulatedPayments(payment);
			if (d >= 100m)
			{
				AnalyticsStuff.TrySendOnceToFacebook("paid_100", null, null);
			}
			else if (d >= 50m)
			{
				AnalyticsStuff.TrySendOnceToFacebook("paid_50", null, null);
			}
			else if (d >= 25m)
			{
				AnalyticsStuff.TrySendOnceToFacebook("paid_25", null, null);
			}
		}
		else
		{
			Debug.LogErrorFormat("Cannot find price for product {0}", new object[]
			{
				purchase.productId
			});
		}
	}

	// Token: 0x060042D0 RID: 17104 RVA: 0x00163D7C File Offset: 0x00161F7C
	private void SendFirstTimePayment(GooglePurchase purchase)
	{
		if (purchase.purchaseState != GooglePurchase.GooglePurchaseState.Purchased)
		{
			return;
		}
		try
		{
			Version v = new Version(Switcher.InitialAppVersion);
			if (v <= new Version(10, 3, 2, 891))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		GoogleSkuInfo googleSkuInfo = (from p in this.Products
		select p.PlatformProduct).OfType<GoogleSkuInfo>().FirstOrDefault(new Func<GoogleSkuInfo, bool>(purchase.productId.Equals));
		if (googleSkuInfo == null)
		{
			Debug.LogErrorFormat("SendFirstTimePayment: sku == null, productId = {0}", new object[]
			{
				purchase.productId
			});
			return;
		}
		decimal value = decimal.Divide(googleSkuInfo.priceAmountMicros, 1000000m);
		AnalyticsFacade.SendFirstTimeRealPayment(purchase.orderId, (float)value, AnalyticsStuff.ReadableNameForInApp(googleSkuInfo.productId), googleSkuInfo.priceCurrencyCode);
	}

	// Token: 0x060042D1 RID: 17105 RVA: 0x00163E8C File Offset: 0x0016208C
	private void HandleGooglePurchaseSucceeded(GooglePurchase purchase)
	{
		Debug.Log("HandleGooglePurchaseSucceeded: " + purchase);
		if (coinsShop.IsWideLayoutAvailable)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("Cheating attempt.");
			}
			this.ConsumeProductIfCheating(purchase);
			return;
		}
		if (!coinsShop.CheckHostsTimestamp())
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("Hosts tampering attempt.");
			}
			this.ConsumeProductIfCheating(purchase);
			return;
		}
		if (!this.VerifyPurchase(purchase.originalJson, purchase.signature))
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("Purchase verification failed.");
			}
			this.ConsumeProductIfCheating(purchase);
			return;
		}
		StoreKitEventListener.ContentType contentType = StoreKitEventListener.ContentType.Unknown;
		try
		{
			string text = null;
			try
			{
				text = InappBonuessController.Instance.GiveBonusForInapp(purchase.productId);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in giving inapp bonus in HandleGooglePurchaseSucceeded: {0}", new object[]
				{
					ex
				});
			}
			bool flag = this.TryAddVirtualCrrency(purchase.productId, text == null);
			if (flag)
			{
				if (Array.IndexOf<string>(StoreKitEventListener.coinIds, purchase.productId) >= 0)
				{
					contentType = StoreKitEventListener.ContentType.Coins;
				}
				else if (Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchase.productId) >= 0)
				{
					contentType = StoreKitEventListener.ContentType.Gems;
				}
				Debug.Log("StoreKitEventListener.HandleGooglePurchaseSucceeded(): Consuming Goole product " + purchase.productId);
				StoreKitEventListener.GooglePlayConsumeAndSave(purchase);
			}
			else if (this.TryAddStarterPackItem(purchase.productId))
			{
				contentType = StoreKitEventListener.ContentType.StarterPack;
			}
			if (StoreKitEventListener.IsSinglePurchase(purchase))
			{
				this.SendFirstTimePayment(purchase);
			}
			this.LogRealPayment(purchase, text);
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
			if (purchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
			{
				decimal d;
				if (VirtualCurrencyHelper.ReferencePricesInUsd.TryGetValue(purchase.productId, out d))
				{
					decimal num = Math.Round(d, 0, MidpointRounding.AwayFromZero);
					Dictionary<string, string> eventParams = new Dictionary<string, string>
					{
						{
							"af_revenue",
							num.ToString("F2")
						},
						{
							"af_content_type",
							contentType.ToString()
						},
						{
							"af_content_id",
							purchase.productId
						},
						{
							"af_currency",
							"USD"
						},
						{
							"af_validated",
							"true"
						},
						{
							"af_receipt_id",
							purchase.orderId
						}
					};
					AnalyticsFacade.SendCustomEventToAppsFlyer("af_purchase_approximate", eventParams);
				}
				else
				{
					Debug.LogErrorFormat("Cannot find price for product {0}", new object[]
					{
						purchase.productId
					});
				}
			}
		}
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x00164114 File Offset: 0x00162314
	private bool VerifyPurchase(string purchaseJson, string base64Signature)
	{
		try
		{
			byte[] signature = Convert.FromBase64String(base64Signature);
			byte[] bytes = Encoding.UTF8.GetBytes(purchaseJson);
			return this._rsa.Value.VerifyData(bytes, this._sha1.Value, signature);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return false;
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x00164188 File Offset: 0x00162388
	private void HandlePurchaseSuccessfulEventAmazon(PurchaseResponse response)
	{
		string message = "Amazon PurchaseResponse (StoreKitEventListener): " + response.Status;
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(message);
			StoreKitEventListener.purchaseInProcess = false;
			return;
		}
		Debug.Log(message);
		PurchaseReceipt purchaseReceipt = response.PurchaseReceipt;
		Debug.Log("Amazon PurchaseResponse.PurchaseReceipt: " + purchaseReceipt.ToJson());
		try
		{
			NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput
			{
				ReceiptId = purchaseReceipt.ReceiptId,
				FulfillmentResult = "FULFILLED"
			};
			string text = null;
			try
			{
				text = InappBonuessController.Instance.GiveBonusForInapp(purchaseReceipt.Sku);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in giving inapp bonus in HandlePurchaseSuccessfulEventAmazon: {0}", new object[]
				{
					ex
				});
			}
			int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, purchaseReceipt.Sku);
			if (num >= StoreKitEventListener.coinIds.GetLowerBound(0))
			{
				int num2 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetCoinInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				string message2 = string.Format("Process purchase {0}, VirtualCurrencyHelper.GetCoinInappsQuantity({1})", purchaseReceipt.Sku, num);
				Debug.Log(message2);
				if (text == null)
				{
					int val = Storager.getInt("Coins", false) + num2;
					Storager.setInt("Coins", val, false);
					AnalyticsFacade.CurrencyAccrual(num2, "Coins", AnalyticsConstants.AccrualType.Purchased);
				}
				Debug.Log("Amazon NotifyFulfillment (HandlePurchaseSuccessfulEvent): " + notifyFulfillmentInput.ToJson());
				StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput);
				ChestBonusController.TryTakeChestBonus(false, num);
				coinsShop.TryToFireCurrenciesAddEvent("Coins");
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(purchaseReceipt.Sku, num2, false);
			}
			num = Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchaseReceipt.Sku);
			if (num >= StoreKitEventListener.gemsIds.GetLowerBound(0))
			{
				int num3 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetGemsInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				string message3 = string.Format("Process purchase {0}, VirtualCurrencyHelper.GetGemsInappsQuantity({1})", purchaseReceipt.Sku, num);
				Debug.Log(message3);
				if (text == null)
				{
					int val2 = Storager.getInt("GemsCurrency", false) + num3;
					Storager.setInt("GemsCurrency", val2, false);
					AnalyticsFacade.CurrencyAccrual(num3, "GemsCurrency", AnalyticsConstants.AccrualType.Purchased);
				}
				Debug.Log("Amazon NotifyFulfillment (HandlePurchaseSuccessfulEvent): " + notifyFulfillmentInput.ToJson());
				StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput);
				ChestBonusController.TryTakeChestBonus(true, num);
				coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(purchaseReceipt.Sku, num3, true);
			}
			bool flag = this.TryAddStarterPackItem(purchaseReceipt.Sku);
			if (flag)
			{
				string message4 = string.Format("Process purchase {0}. Starter pack.", purchaseReceipt.Sku, num);
				Debug.Log(message4);
			}
			FriendsController.sharedController.SendOurData(false);
		}
		finally
		{
			StoreKitEventListener.purchaseInProcess = false;
			StoreKitEventListener.restoreInProcess = false;
		}
	}

	// Token: 0x060042D4 RID: 17108 RVA: 0x00164468 File Offset: 0x00162668
	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
		if (this._cheatedPurchasesToConsume.RemoveWhere((GooglePurchase p) => p.productId == purchase.productId) == 0)
		{
			this._purchasesToConsume.RemoveWhere((GooglePurchase p) => p.productId == purchase.productId);
		}
		this.AddCurrencyAndConsumeNextGooglePlayPurchase();
	}

	// Token: 0x060042D5 RID: 17109 RVA: 0x001644D4 File Offset: 0x001626D4
	private void consumePurchaseFailedEvent(string error)
	{
		Debug.LogWarning("consumePurchaseFailedEvent: " + error);
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x001644E8 File Offset: 0x001626E8
	private static NotifyFulfillmentInput FulfillmentInputForReceipt(PurchaseReceipt receipt)
	{
		return new NotifyFulfillmentInput
		{
			ReceiptId = receipt.ReceiptId,
			FulfillmentResult = "FULFILLED"
		};
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x00164518 File Offset: 0x00162718
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

	// Token: 0x060042D8 RID: 17112 RVA: 0x0016453C File Offset: 0x0016273C
	private IEnumerator WaitForX3AndGiveCurrency(GooglePurchase purchase, PurchaseReceipt receipt)
	{
		string idToList = (purchase == null) ? receipt.ReceiptId : purchase.orderId;
		StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun.Add(idToList);
		try
		{
			while (StoreKitEventListener.ShouldDelayCompletingTransactions())
			{
				if (CoroutineRunner.Instance != null)
				{
					yield return CoroutineRunner.Instance.StartCoroutine(this.MyWaitForSeconds(1f));
				}
				else
				{
					Debug.LogError("Amazon/Android WaitForX3AndGiveCurrency CoroutineRunner.Instance == null ");
				}
			}
		}
		finally
		{
			StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun.Remove(idToList);
		}
		try
		{
			if (PromoActionsManager.sharedManager != null)
			{
				PromoActionsManager.sharedManager.ForceCheckEventX3Active();
			}
		}
		catch (Exception ex)
		{
			Exception e = ex;
			Debug.LogError("Amazon WaitForX3AndGiveCurrency PromoActionsManager.sharedManager.ForceCheckEventX3Active() exception: " + e);
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			StoreKitEventListener.GiveCoinsOrGemsOnAmazon(receipt);
		}
		else
		{
			this.AndroidAddCurrencyAndConsume(purchase);
		}
		yield break;
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x00164574 File Offset: 0x00162774
	private static void GiveCoinsOrGemsOnAmazon(PurchaseReceipt receipt)
	{
		try
		{
			Debug.Log("[Rilisoft] Amazon: restoring purchase: " + receipt.Sku);
			string text = null;
			try
			{
				text = InappBonuessController.Instance.GiveBonusForInapp(receipt.Sku);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in giving inapp bonus in HandlePurchaseSuccessfulEventAmazon: {0}", new object[]
				{
					ex
				});
			}
			int num = Array.IndexOf<string>(StoreKitEventListener.coinIds, receipt.Sku);
			if (num >= StoreKitEventListener.coinIds.GetLowerBound(0))
			{
				int num2 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetCoinInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				if (text == null)
				{
					int val = Storager.getInt("Coins", false) + num2;
					Storager.setInt("Coins", val, false);
					AnalyticsFacade.CurrencyAccrual(num2, "Coins", AnalyticsConstants.AccrualType.Purchased);
				}
				try
				{
					ChestBonusController.TryTakeChestBonus(false, num);
				}
				catch (Exception arg)
				{
					Debug.LogError("[Rilisoft] Amazon: TryTakeChestBonus exception: " + arg);
				}
				coinsShop.TryToFireCurrenciesAddEvent("Coins");
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(receipt.Sku, num2, false);
			}
			num = Array.IndexOf<string>(StoreKitEventListener.gemsIds, receipt.Sku);
			if (num >= StoreKitEventListener.gemsIds.GetLowerBound(0))
			{
				int num3 = Mathf.RoundToInt((float)VirtualCurrencyHelper.GetGemsInappsQuantity(num) * PremiumAccountController.VirtualCurrencyMultiplier);
				string message = string.Format("Process purchase {0}, VirtualCurrencyHelper.GetGemsInappsQuantity({1})", receipt.Sku, num);
				Debug.Log(message);
				if (text == null)
				{
					int val2 = Storager.getInt("GemsCurrency", false) + num3;
					Storager.setInt("GemsCurrency", val2, false);
					AnalyticsFacade.CurrencyAccrual(num3, "GemsCurrency", AnalyticsConstants.AccrualType.Purchased);
				}
				try
				{
					ChestBonusController.TryTakeChestBonus(true, num);
				}
				catch (Exception arg2)
				{
					Debug.LogError("[Rilisoft] Amazon: TryTakeChestBonus exception: " + arg2);
				}
				coinsShop.TryToFireCurrenciesAddEvent("GemsCurrency");
				StoreKitEventListener.CheckIfFirstTimePayment();
				StoreKitEventListener.SetLastPaymentTime();
				StoreKitEventListener.LogVirtualCurrencyPurchased(receipt.Sku, num3, true);
			}
		}
		catch (Exception arg3)
		{
			Debug.LogError("Exception GiveCoinsOrGemsOnAmazon: " + arg3);
		}
		finally
		{
			NotifyFulfillmentInput notifyFulfillmentInput = StoreKitEventListener.FulfillmentInputForReceipt(receipt);
			Debug.Log("Amazon NotifyFulfillment (HandlePurchaseUpdatesRequestSuccessfulEvent): " + notifyFulfillmentInput.ToJson());
			StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput);
		}
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x001647FC File Offset: 0x001629FC
	private void HandlePurchaseUpdatesRequestSuccessfulEvent(GetPurchaseUpdatesResponse response)
	{
		string message = "[Rilisoft] Amazon GetPurchaseUpdatesResponse: " + response.ToJson();
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning(message);
			return;
		}
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(message);
		}
		string text = string.Empty;
		if (Storager.hasKey("Amazon.FulfilledReceipts"))
		{
			text = Storager.getString("Amazon.FulfilledReceipts", false);
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "[]";
		}
		List<object> source = (Rilisoft.MiniJson.Json.Deserialize(text) as List<object>) ?? new List<object>();
		HashSet<string> hashSet = new HashSet<string>(source.OfType<string>());
		List<PurchaseReceipt> receipts = response.Receipts;
		for (int num = 0; num != receipts.Count; num++)
		{
			PurchaseReceipt purchaseReceipt = receipts[num];
			string sku = purchaseReceipt.Sku;
			if (StoreKitEventListener.starterPackIds.Contains(sku))
			{
				try
				{
					StarterPackController.Get.AddBuyAndroidStarterPack(sku);
					StarterPackController.Get.TryRestoreStarterPack(sku);
				}
				catch (Exception ex)
				{
					Debug.LogFormat("Exception in HandlePurchaseUpdatesRequestSuccessfulEvent starter packs: {0}", new object[]
					{
						ex
					});
				}
			}
			else
			{
				if (!StoreKitEventListener.coinIds.Contains(sku))
				{
					if (!StoreKitEventListener.gemsIds.Contains(sku))
					{
						goto IL_1D3;
					}
				}
				try
				{
					if (!hashSet.Contains(purchaseReceipt.ReceiptId))
					{
						if (!StoreKitEventListener.listOfIdsForWhichX3WaitingCoroutinesRun.Contains(purchaseReceipt.ReceiptId))
						{
							if (CoroutineRunner.Instance != null)
							{
								CoroutineRunner.Instance.StartCoroutine(this.WaitForX3AndGiveCurrency(null, purchaseReceipt));
							}
							else
							{
								Debug.LogError("Amazon NotifyFulfillment CoroutineRunner.Instance == null ");
								StoreKitEventListener.GiveCoinsOrGemsOnAmazon(purchaseReceipt);
							}
						}
					}
					else
					{
						NotifyFulfillmentInput notifyFulfillmentInput = StoreKitEventListener.FulfillmentInputForReceipt(purchaseReceipt);
						Debug.Log("Amazon NotifyFulfillment (HandlePurchaseUpdatesRequestSuccessfulEvent): " + notifyFulfillmentInput.ToJson());
						StoreKitEventListener.AmazonNotifyFulfillmentAndSave(notifyFulfillmentInput);
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception HandlePurchaseUpdatesRequestSuccessfulEvent: " + arg);
				}
			}
			IL_1D3:;
		}
	}

	// Token: 0x060042DB RID: 17115 RVA: 0x00164A24 File Offset: 0x00162C24
	private static void AmazonNotifyFulfillmentAndSave(NotifyFulfillmentInput notifyFulfillmentInput)
	{
		string callee = (!Defs.IsDeveloperBuild) ? string.Empty : string.Format("AmazonNotifyFulfillmentAndSave('{0}')", notifyFulfillmentInput.ToJson());
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			if (notifyFulfillmentInput == null)
			{
				throw new ArgumentNullException("notifyFulfillmentInput");
			}
			string text = string.Empty;
			if (Storager.hasKey("Amazon.FulfilledReceipts"))
			{
				text = Storager.getString("Amazon.FulfilledReceipts", false);
				Debug.LogFormat("Storager has {0}: {1}", new object[]
				{
					"Amazon.FulfilledReceipts",
					text
				});
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "[]";
				Debug.LogFormat("Storager doesn't have {0}: {1}", new object[]
				{
					"Amazon.FulfilledReceipts",
					text
				});
			}
			List<object> source = (Rilisoft.MiniJson.Json.Deserialize(text) as List<object>) ?? new List<object>();
			AmazonIapV2Impl.Instance.NotifyFulfillment(notifyFulfillmentInput);
			text = Rilisoft.MiniJson.Json.Serialize(new HashSet<string>(source.OfType<string>())
			{
				notifyFulfillmentInput.ReceiptId
			}.ToList<string>());
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("[Rilisoft] Saving fulfillments: " + text);
			}
			Storager.setString("Amazon.FulfilledReceipts", text, false);
			PlayerPrefs.Save();
		}
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x00164B80 File Offset: 0x00162D80
	private static void GooglePlayConsumeAndSave(GooglePurchase purchase)
	{
		try
		{
			if (purchase == null)
			{
				Debug.LogWarning("GooglePlayConsumeAndSave: purchase == null");
			}
			else
			{
				string text = string.Empty;
				if (Storager.hasKey("Android.GooglePlayOrderIdsKey"))
				{
					text = Storager.getString("Android.GooglePlayOrderIdsKey", false);
				}
				if (string.IsNullOrEmpty(text))
				{
					text = "[]";
				}
				List<object> source = (Rilisoft.MiniJson.Json.Deserialize(text) as List<object>) ?? new List<object>();
				GoogleIAB.consumeProduct(purchase.productId);
				text = Rilisoft.MiniJson.Json.Serialize(new HashSet<string>(source.OfType<string>())
				{
					purchase.orderId
				}.ToList<string>());
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("[Rilisoft] Saving consumed order ids: " + text);
				}
				Storager.setString("Android.GooglePlayOrderIdsKey", text, false);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"GooglePlayConsumeAndSave exception: ",
				ex,
				"\nstacktrace:\n",
				Environment.StackTrace
			}));
		}
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x00164C98 File Offset: 0x00162E98
	private void HandlePurchaseUpdatesRequestFailedEvent()
	{
		Debug.LogWarning("Amazon: Purchase updates request failed.");
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x00164CA4 File Offset: 0x00162EA4
	private static RSACryptoServiceProvider InitializeRsa()
	{
		RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
		rsacryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>oTzMTaqsFhaywvCFKawFwL5KM+djLJfOCT/rbGQRfHmHYmOY2sBMgDWsA/67Szx6EVTZPVlFzHMgkAq1TwdL/A5aYGpGzaCX7o96cyp8R6wSF+xCuj++LAkTaDnLW0veI2bke3EVHu3At9xgM46e+VDucRUqQLvf6SQRb15nuflY5i08xKnewgX7I4U2H0RvAZDyoip+qZPmI4ZvaufAfc0jwZbw7XGiV41zibY3LU0N57mYKk51Wx+tOaJ7Tkc9Rl1qVCTjb+bwXshTqhVXVP6r4kabLWw/8OJUh0Sm69lbps6amP7vPy571XjscCTMLfXQan1959rHbNgkb2mLLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
		return rsacryptoServiceProvider;
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x00164CC4 File Offset: 0x00162EC4
	private void InitializeTestProductsAmazon()
	{
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin1"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin2"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin3."
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin4"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin5"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin17"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin8"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				"coin9"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of coins"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem1"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem2"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem3"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem4"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem5"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem6"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"sku",
				"gem7"
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"Small pack of gems"
			}
		})));
		Dictionary<string, object> jsonMap = new Dictionary<string, object>
		{
			{
				"description",
				"Test starter pack product for editor in Amazon edition"
			},
			{
				"productType",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"sku",
				StoreKitEventListener.starterPack1
			},
			{
				"smallIconUrl",
				"http://example.com"
			},
			{
				"title",
				"First starter pack(amazon)"
			}
		};
		this._products.Add(new AmazonMarketProduct(ProductData.CreateFromDictionary(jsonMap)));
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x00165494 File Offset: 0x00163694
	private void InitializeTestProductsGoogle()
	{
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin1"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin2"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin3."
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin4"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin5"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin7"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin8"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test coin product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"99 руб."
			},
			{
				"productId",
				"coin9"
			},
			{
				"title",
				"Average pack of coins"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem1"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem2"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem3"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem4"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem5"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem6"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(new Dictionary<string, object>
		{
			{
				"description",
				"Test gem product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				"gem7"
			},
			{
				"title",
				"Average pack of gems"
			}
		})));
		Dictionary<string, object> dict = new Dictionary<string, object>
		{
			{
				"description",
				"Test starter pack product for editor in Google edition"
			},
			{
				"type",
				"Not defined"
			},
			{
				"price",
				"33 руб."
			},
			{
				"productId",
				StoreKitEventListener.starterPack1
			},
			{
				"title",
				"First starter pack(android)"
			}
		};
		this._products.Add(new GoogleMarketProduct(new GoogleSkuInfo(dict)));
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x00165B64 File Offset: 0x00163D64
	public static bool ShouldDelayCompletingTransactions()
	{
		bool result;
		try
		{
			result = (Time.realtimeSinceStartup - PromoActionsManager.startupTime < 45f && (!PromoActionsManager.x3InfoDownloadaedOnceDuringCurrentRun || !ChestBonusController.chestBonusesObtainedOnceInCurrentRun || !coinsShop.IsStoreAvailable));
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in ShouldDelayCompletingTransactions: " + arg);
			result = false;
		}
		return result;
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x00165BF0 File Offset: 0x00163DF0
	private void Awake()
	{
		StoreKitEventListener.Instance = this;
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x00165BF8 File Offset: 0x00163DF8
	private void OnDestroy()
	{
		StoreKitEventListener.Instance = null;
	}

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x060042E4 RID: 17124 RVA: 0x00165C00 File Offset: 0x00163E00
	private static string starterPack1
	{
		get
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "starterpack1andr";
			}
			return "starterpack1";
		}
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x00165C18 File Offset: 0x00163E18
	internal static bool IsPayingUser()
	{
		return Storager.getInt("PayingUser", true) > 0;
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x00165C28 File Offset: 0x00163E28
	public void ProvideContent()
	{
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x00165C2C File Offset: 0x00163E2C
	private static IEnumerator WaitForFyberAndSetIsPaying()
	{
		while (FyberFacade.Instance == null)
		{
			yield return null;
		}
		FyberFacade.Instance.SetUserPaying("1");
		yield break;
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x00165C40 File Offset: 0x00163E40
	internal static decimal IncrementAccumulatedPayments(decimal payment)
	{
		decimal d;
		if (!Storager.hasKey("Analytics.AccumulatedPayments") || !decimal.TryParse(Storager.getString("Analytics.AccumulatedPayments", false), out d))
		{
			d = 0m;
		}
		decimal result = d + payment;
		Storager.setString("Analytics.AccumulatedPayments", result.ToString(CultureInfo.InvariantCulture), false);
		return result;
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x00165C9C File Offset: 0x00163E9C
	internal static void CheckIfFirstTimePayment()
	{
		if (!Storager.hasKey("PayingUser") || Storager.getInt("PayingUser", true) != 1)
		{
			Storager.setInt("PayingUser", 1, true);
			if (CoroutineRunner.Instance != null)
			{
				CoroutineRunner.Instance.StartCoroutine(StoreKitEventListener.WaitForFyberAndSetIsPaying());
			}
			else
			{
				Debug.LogError("CheckIfFirstTimePayment CoroutineRunner.Instance == null");
			}
		}
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x00165D04 File Offset: 0x00163F04
	public static int GetDollarsSpent()
	{
		return PlayerPrefs.GetInt("ALLCoins", 0) + PlayerPrefs.GetInt("ALLGems", 0);
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x00165D20 File Offset: 0x00163F20
	internal static void SetLastPaymentTime()
	{
		string value = DateTime.UtcNow.ToString("s");
		PlayerPrefs.SetString("Last Payment Time", value);
		Storager.setInt("PayingUser", 1, true);
		PlayerPrefs.SetString("Last Payment Time (Advertisement)", value);
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x00165D64 File Offset: 0x00163F64
	public static void LogVirtualCurrencyPurchased(string purchaseId, int virtualCurrencyCount, bool isGems)
	{
		try
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.AnyDiscountForTryGuns)
			{
				AnalyticsStuff.LogWEaponsSpecialOffers_MoneySpended(purchaseId);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("LogVirtualCurrencyPurchased exception (Weapons Special Offers): " + arg);
		}
		try
		{
			if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
			{
				AnalyticsStuff.LogDailyGiftPurchases(purchaseId);
			}
			if (BuySmileBannerController.openedFromPromoActions || (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.IsFromPromoActions))
			{
				AnalyticsStuff.LogSpecialOffersPanel((!isGems) ? "Buy Coins" : "Buy Gems", AnalyticsStuff.ReadableNameForInApp(purchaseId), null, null);
			}
			if (ShopNGUIController.sharedShop.GunThatWeUsedInPolygon != null)
			{
				AnalyticsFacade.SendCustomEvent("Polygon", new Dictionary<string, object>
				{
					{
						"Money Spended",
						AnalyticsStuff.ReadableNameForInApp(purchaseId)
					}
				});
			}
		}
		catch (Exception arg2)
		{
			Debug.LogError("LogVirtualCurrencyPurchased exception: " + arg2);
		}
		string deviceModel = SystemInfo.deviceModel;
		ShopNGUIController.AddBoughtCurrency((!isGems) ? "Coins" : "GemsCurrency", virtualCurrencyCount);
		string value = string.Format("{0} ({1})", purchaseId, virtualCurrencyCount);
		string value2 = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1).ToString();
		string value3 = (!(ExperienceController.sharedController != null)) ? "Unknown" : ExperienceController.sharedController.currentLevel.ToString();
		string eventName = (((!isGems) ? "Coins Purchased " : "Gems Purchased ") + StoreKitEventListener.State.Mode) ?? string.Empty;
		Dictionary<string, string> eventParams = new Dictionary<string, string>(StoreKitEventListener.State.Parameters)
		{
			{
				StoreKitEventListener.State.PurchaseKey,
				purchaseId
			},
			{
				"Rank",
				value3
			},
			{
				"Session number",
				value2
			},
			{
				"SKU",
				value
			},
			{
				"Device model",
				deviceModel
			}
		};
		AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, eventParams);
		int num = PlayerPrefs.GetInt("CountPaying", 0);
		int num2 = Array.IndexOf<string>(StoreKitEventListener.coinIds, purchaseId);
		bool flag = false;
		if (num2 == -1)
		{
			num2 = Array.IndexOf<string>(StoreKitEventListener.gemsIds, purchaseId);
			if (num2 == -1)
			{
				num2 = Array.IndexOf<string>(StoreKitEventListener.starterPackIds, purchaseId);
				flag = true;
			}
		}
		if (((BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon) || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64) && FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendAddPurchaseEvent(purchaseId, string.Empty, 0f, string.Empty, string.Empty);
		}
		if (num2 == -1)
		{
			string message = string.Format("Could not find “{0}” value in coinIds array.", purchaseId);
			Debug.Log(message);
		}
		else
		{
			if (ABTestController.useBuffSystem)
			{
				BuffSystem.instance.OnCurrencyBuyed(isGems, num2);
			}
			int num3;
			if (isGems)
			{
				num3 = PlayerPrefs.GetInt("ALLGems", 0);
				num3 += ((!flag) ? VirtualCurrencyHelper.gemsPriceIds[num2] : VirtualCurrencyHelper.starterPackFakePrice[num2]);
				PlayerPrefs.SetInt("ALLGems", num3);
			}
			else
			{
				num3 = PlayerPrefs.GetInt("ALLCoins", 0);
				num3 += ((!flag) ? VirtualCurrencyHelper.coinPriceIds[num2] : VirtualCurrencyHelper.starterPackFakePrice[num2]);
				PlayerPrefs.SetInt("ALLCoins", num3);
			}
			if (!flag)
			{
				Storager.setInt(Defs.AllCurrencyBought + ((!isGems) ? "Coins" : "GemsCurrency"), Storager.getInt(Defs.AllCurrencyBought + ((!isGems) ? "Coins" : "GemsCurrency"), false) + virtualCurrencyCount, false);
			}
			num++;
			PlayerPrefs.SetInt("CountPaying", num);
			if (num >= 1 && PlayerPrefs.GetInt("Paying_User", 0) == 0)
			{
				PlayerPrefs.SetInt("Paying_User", 1);
				FacebookController.LogEvent("Paying_User", null);
				Debug.Log("Paying_User detected.");
			}
			if (num > 1 && PlayerPrefs.GetInt("Paying_User_Dolphin", 0) == 0)
			{
				PlayerPrefs.SetInt("Paying_User_Dolphin", 1);
				FacebookController.LogEvent("Paying_User_Dolphin", null);
				Debug.Log("Paying_User_Dolphin detected.");
			}
			if (num > 3 && PlayerPrefs.GetInt("Paying_User_Whale", 0) == 0)
			{
				PlayerPrefs.SetInt("Paying_User_Whale", 1);
				FacebookController.LogEvent("Paying_User_Whale", null);
				Debug.Log("Paying_User_Whale detected.");
			}
			if (num3 >= 100 && PlayerPrefs.GetInt("SendKit", 0) == 0)
			{
				PlayerPrefs.SetInt("SendKit", 1);
				FacebookController.LogEvent("Whale_detected", null);
				Debug.Log("Whale detected.");
			}
			if (PlayerPrefs.GetInt("confirmed_1st_time", 0) == 0)
			{
				PlayerPrefs.SetInt("confirmed_1st_time", 1);
				FacebookController.LogEvent("Purchase_confirmed_1st_time", null);
				Debug.Log("Purchase confirmed first time.");
			}
			if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
			{
				FacebookController.LogEvent("Active_loyal_users_payed", null);
				PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
			}
		}
	}

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x060042ED RID: 17133 RVA: 0x001662C8 File Offset: 0x001644C8
	internal static StoreKitEventListener.StoreKitEventListenerState State
	{
		get
		{
			return StoreKitEventListener._state;
		}
	}

	// Token: 0x040030C5 RID: 12485
	public const string coin1 = "coin1";

	// Token: 0x040030C6 RID: 12486
	public const string coin2 = "coin2";

	// Token: 0x040030C7 RID: 12487
	public const string coin3 = "coin3.";

	// Token: 0x040030C8 RID: 12488
	public const string coin4 = "coin4";

	// Token: 0x040030C9 RID: 12489
	public const string coin5 = "coin5";

	// Token: 0x040030CA RID: 12490
	public const string coin7 = "coin7";

	// Token: 0x040030CB RID: 12491
	public const string coin8 = "coin8";

	// Token: 0x040030CC RID: 12492
	private const string AmazonFulfilledReceiptsKey = "Amazon.FulfilledReceipts";

	// Token: 0x040030CD RID: 12493
	private const string GooglePlayConsumedOrderIdsKey = "Android.GooglePlayOrderIdsKey";

	// Token: 0x040030CE RID: 12494
	public const string bigAmmoPackID = "bigammopack";

	// Token: 0x040030CF RID: 12495
	public const string crystalswordID = "crystalsword";

	// Token: 0x040030D0 RID: 12496
	public const string fullHealthID = "Fullhealth";

	// Token: 0x040030D1 RID: 12497
	public const string minerWeaponID = "MinerWeapon";

	// Token: 0x040030D2 RID: 12498
	[NonSerialized]
	internal readonly ICollection<IMarketProduct> _products = new List<IMarketProduct>();

	// Token: 0x040030D3 RID: 12499
	[NonSerialized]
	public readonly ICollection<GoogleSkuInfo> _skinProducts = new GoogleSkuInfo[0];

	// Token: 0x040030D4 RID: 12500
	[NonSerialized]
	public static bool billingSupported;

	// Token: 0x040030D5 RID: 12501
	[NonSerialized]
	public static readonly string[] coinIds;

	// Token: 0x040030D6 RID: 12502
	private static string[] _productIds;

	// Token: 0x040030D7 RID: 12503
	private readonly HashSet<GooglePurchase> _purchasesToConsume = new HashSet<GooglePurchase>();

	// Token: 0x040030D8 RID: 12504
	private readonly HashSet<GooglePurchase> _cheatedPurchasesToConsume = new HashSet<GooglePurchase>();

	// Token: 0x040030D9 RID: 12505
	private readonly TaskCompletionSource<AmazonUserData> _amazonUserPromise = new TaskCompletionSource<AmazonUserData>();

	// Token: 0x040030DA RID: 12506
	private IDisposable _purchaseFailedSubscription = new ActionDisposable(null);

	// Token: 0x040030DB RID: 12507
	private static List<string> listOfIdsForWhichX3WaitingCoroutinesRun;

	// Token: 0x040030DC RID: 12508
	private readonly Lazy<SHA1Managed> _sha1 = new Lazy<SHA1Managed>(() => new SHA1Managed());

	// Token: 0x040030DD RID: 12509
	private readonly Lazy<RSACryptoServiceProvider> _rsa = new Lazy<RSACryptoServiceProvider>(new Func<RSACryptoServiceProvider>(StoreKitEventListener.InitializeRsa));

	// Token: 0x040030DE RID: 12510
	public static StoreKitEventListener Instance = null;

	// Token: 0x040030DF RID: 12511
	private static string gem1 = "gem1";

	// Token: 0x040030E0 RID: 12512
	private static string gem2 = "gem2";

	// Token: 0x040030E1 RID: 12513
	private static string gem3 = "gem3";

	// Token: 0x040030E2 RID: 12514
	private static string gem4 = "gem4";

	// Token: 0x040030E3 RID: 12515
	private static string gem5 = "gem5";

	// Token: 0x040030E4 RID: 12516
	private static string gem6 = "gem6";

	// Token: 0x040030E5 RID: 12517
	private static string gem7 = "gem7";

	// Token: 0x040030E6 RID: 12518
	private static string starterPack2 = "starterpack2";

	// Token: 0x040030E7 RID: 12519
	private static string starterPack4 = "starterpack4";

	// Token: 0x040030E8 RID: 12520
	private static string starterPack6 = "starterpack6";

	// Token: 0x040030E9 RID: 12521
	private static string starterPack3 = "starterpack3";

	// Token: 0x040030EA RID: 12522
	private static string starterPack5 = "starterpack5";

	// Token: 0x040030EB RID: 12523
	private static string starterPack7 = "starterpack7";

	// Token: 0x040030EC RID: 12524
	private static string starterPack8 = "starterpack8";

	// Token: 0x040030ED RID: 12525
	public static readonly int[] realValue = new int[]
	{
		1,
		3,
		5,
		10,
		20,
		50,
		100
	};

	// Token: 0x040030EE RID: 12526
	public static readonly string[] gemsIds = new string[]
	{
		StoreKitEventListener.gem1,
		StoreKitEventListener.gem2,
		StoreKitEventListener.gem3,
		StoreKitEventListener.gem4,
		StoreKitEventListener.gem5,
		StoreKitEventListener.gem6,
		StoreKitEventListener.gem7
	};

	// Token: 0x040030EF RID: 12527
	public static readonly string[] starterPackIds = new string[]
	{
		StoreKitEventListener.starterPack1,
		StoreKitEventListener.starterPack2,
		StoreKitEventListener.starterPack3,
		StoreKitEventListener.starterPack4,
		StoreKitEventListener.starterPack5,
		StoreKitEventListener.starterPack6,
		StoreKitEventListener.starterPack7,
		StoreKitEventListener.starterPack8
	};

	// Token: 0x040030F0 RID: 12528
	public static Dictionary<string, string> inAppsReadableNames = new Dictionary<string, string>
	{
		{
			"coin1",
			"Small Stack of Coins"
		},
		{
			"coin7",
			"Medium Stack of Coins"
		},
		{
			"coin2",
			"Big Stack of Coins"
		},
		{
			"coin3.",
			"Huge Stack of Coins"
		},
		{
			"coin4",
			"Chest with Coins"
		},
		{
			"coin5",
			"Golden Chest with Coins"
		},
		{
			"coin8",
			"Holy Grail"
		},
		{
			StoreKitEventListener.gem1,
			"Few Gems"
		},
		{
			StoreKitEventListener.gem2,
			"Handful of Gems"
		},
		{
			StoreKitEventListener.gem3,
			"Pile of Gems"
		},
		{
			StoreKitEventListener.gem4,
			"Chest with Gems"
		},
		{
			StoreKitEventListener.gem5,
			"Treasure with Gems"
		},
		{
			StoreKitEventListener.gem6,
			"Expensive Relic"
		},
		{
			StoreKitEventListener.gem7,
			"Safe with Gems"
		},
		{
			StoreKitEventListener.starterPack1,
			"Newbie Set"
		},
		{
			"starterpack2",
			"Golden Coins Extra Pack"
		},
		{
			StoreKitEventListener.starterPack3,
			"Trooper Set"
		},
		{
			"starterpack4",
			"Gems Extra Pack"
		},
		{
			StoreKitEventListener.starterPack5,
			"Veteran Set"
		},
		{
			"starterpack6",
			"Mega Gems Pack"
		},
		{
			StoreKitEventListener.starterPack7,
			"Hero Set"
		},
		{
			StoreKitEventListener.starterPack8,
			"Winner Set"
		}
	};

	// Token: 0x040030F1 RID: 12529
	public static string elixirSettName = Defs.NumberOfElixirsSett;

	// Token: 0x040030F2 RID: 12530
	public static bool purchaseInProcess = false;

	// Token: 0x040030F3 RID: 12531
	public static bool restoreInProcess = false;

	// Token: 0x040030F4 RID: 12532
	public static string elixirID = (!GlobalGameController.isFullVersion) ? "elixirlite" : "elixir";

	// Token: 0x040030F5 RID: 12533
	public static string fullVersion = "extendedversion";

	// Token: 0x040030F6 RID: 12534
	public static string armor = "armor";

	// Token: 0x040030F7 RID: 12535
	public static string armor2 = "armor2";

	// Token: 0x040030F8 RID: 12536
	public static string armor3 = "armor3";

	// Token: 0x040030F9 RID: 12537
	public static readonly string[] idsForSingle;

	// Token: 0x040030FA RID: 12538
	public static readonly string[] idsForMulti;

	// Token: 0x040030FB RID: 12539
	public static readonly string[] idsForFull;

	// Token: 0x040030FC RID: 12540
	public static readonly string[][] categoriesSingle;

	// Token: 0x040030FD RID: 12541
	public static readonly string[][] categoriesMulti;

	// Token: 0x040030FE RID: 12542
	public GameObject messagePrefab;

	// Token: 0x040030FF RID: 12543
	public static string[] categoryNames = new string[]
	{
		"Armory",
		"Guns",
		"Melee",
		"Special",
		"Gear"
	};

	// Token: 0x04003100 RID: 12544
	public AudioClip onEarnCoinsSound;

	// Token: 0x04003101 RID: 12545
	public AudioClip onEarnGemsSound;

	// Token: 0x04003102 RID: 12546
	[NonSerialized]
	public static List<string> buyStarterPack = new List<string>();

	// Token: 0x04003103 RID: 12547
	private static readonly StoreKitEventListener.StoreKitEventListenerState _state = new StoreKitEventListener.StoreKitEventListenerState();

	// Token: 0x0200076C RID: 1900
	private enum ContentType
	{
		// Token: 0x0400310E RID: 12558
		Unknown,
		// Token: 0x0400310F RID: 12559
		Coins,
		// Token: 0x04003110 RID: 12560
		Gems,
		// Token: 0x04003111 RID: 12561
		StarterPack
	}

	// Token: 0x0200076D RID: 1901
	internal sealed class StoreKitEventListenerState
	{
		// Token: 0x060042F7 RID: 17143 RVA: 0x00166394 File Offset: 0x00164594
		public StoreKitEventListenerState()
		{
			this.Mode = string.Empty;
			this.PurchaseKey = string.Empty;
			this.Parameters = new Dictionary<string, string>();
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x060042F8 RID: 17144 RVA: 0x001663C8 File Offset: 0x001645C8
		// (set) Token: 0x060042F9 RID: 17145 RVA: 0x001663D0 File Offset: 0x001645D0
		public string Mode { get; set; }

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060042FA RID: 17146 RVA: 0x001663DC File Offset: 0x001645DC
		// (set) Token: 0x060042FB RID: 17147 RVA: 0x001663E4 File Offset: 0x001645E4
		public string PurchaseKey { get; set; }

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060042FC RID: 17148 RVA: 0x001663F0 File Offset: 0x001645F0
		// (set) Token: 0x060042FD RID: 17149 RVA: 0x001663F8 File Offset: 0x001645F8
		public IDictionary<string, string> Parameters { get; private set; }
	}
}
