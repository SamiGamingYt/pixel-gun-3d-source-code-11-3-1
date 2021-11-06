using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x02000497 RID: 1175
internal sealed class PremiumAccountScreenController : MonoBehaviour
{
	// Token: 0x06002A01 RID: 10753 RVA: 0x000DD3E8 File Offset: 0x000DB5E8
	private void Start()
	{
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && ExperienceController.sharedController != null)
		{
			this.ranksBefore = ExperienceController.sharedController.isShowRanks;
			ExperienceController.sharedController.isShowRanks = false;
		}
		this.UpdateFreeButtons();
		for (int i = 0; i < this.rentButtons.Length; i++)
		{
			foreach (object obj in this.rentButtons[i].transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name.Equals("GemsIcon"))
				{
					PremiumAccountController.AccountType accountType = (PremiumAccountController.AccountType)i;
					string key = accountType.ToString();
					ItemPrice itemPrice = VirtualCurrencyHelper.Price(key);
					UILabel component = transform.GetChild(0).GetComponent<UILabel>();
					component.text = itemPrice.Price.ToString();
					break;
				}
			}
		}
		PremiumAccountScreenController.Instance = this;
	}

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x06002A02 RID: 10754 RVA: 0x000DD518 File Offset: 0x000DB718
	// (set) Token: 0x06002A03 RID: 10755 RVA: 0x000DD520 File Offset: 0x000DB720
	public string Header { get; set; }

	// Token: 0x06002A04 RID: 10756 RVA: 0x000DD52C File Offset: 0x000DB72C
	public void HandleRentButtonPressed(UIButton button)
	{
		PremiumAccountController.AccountType accType = (PremiumAccountController.AccountType)Array.IndexOf<UIButton>(this.rentButtons, button);
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(accType.ToString());
		Action<PremiumAccountController.AccountType> provideAcc = delegate(PremiumAccountController.AccountType at)
		{
			if (PremiumAccountController.Instance != null)
			{
				PremiumAccountController.Instance.BuyAccount(at);
			}
			this.UpdateFreeButtons();
		};
		if (this.InitialFreeAvailable && accType == PremiumAccountController.AccountType.OneDay)
		{
			this.SetInitialFreeUsed();
			provideAcc(accType);
			this.Hide();
		}
		else
		{
			int priceAmount = itemPrice.Price;
			string priceCurrency = itemPrice.Currency;
			ShopNGUIController.TryToBuy(this.window, itemPrice, delegate
			{
				provideAcc(accType);
				AnalyticsStuff.LogSales(accType.ToString(), "Premium Account", false);
				AnalyticsFacade.InAppPurchase(accType.ToString(), "Premium Account", 1, priceAmount, priceCurrency);
				if (this.InitialFreeAvailable)
				{
					this.SetInitialFreeUsed();
					provideAcc(PremiumAccountController.AccountType.OneDay);
				}
				this.Hide();
			}, null, null, null, null, null);
		}
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x000DD604 File Offset: 0x000DB804
	public void Hide()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x000DD614 File Offset: 0x000DB814
	private void UpdateFreeButtons()
	{
		bool initialFreeAvailable = this.InitialFreeAvailable;
		foreach (object obj in this.rentButtons[0].transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.Equals("Free"))
			{
				transform.gameObject.SetActive(initialFreeAvailable);
			}
			if (transform.name.Equals("GemsIcon"))
			{
				transform.gameObject.SetActive(!initialFreeAvailable);
			}
		}
		this.tapToActivate.SetActive(initialFreeAvailable);
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06002A07 RID: 10759 RVA: 0x000DD6DC File Offset: 0x000DB8DC
	private bool InitialFreeAvailable
	{
		get
		{
			return Storager.getInt("PremiumInitialFree1Day", false) == 0;
		}
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x000DD6EC File Offset: 0x000DB8EC
	private void SetInitialFreeUsed()
	{
		Storager.setInt("PremiumInitialFree1Day", 1, false);
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x000DD6FC File Offset: 0x000DB8FC
	private void OnDestroy()
	{
		PremiumAccountScreenController.Instance = null;
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = this.ranksBefore;
		}
	}

	// Token: 0x04001F00 RID: 7936
	public GameObject tapToActivate;

	// Token: 0x04001F01 RID: 7937
	public GameObject window;

	// Token: 0x04001F02 RID: 7938
	public UIButton[] rentButtons;

	// Token: 0x04001F03 RID: 7939
	public List<UILabel> headerLabels;

	// Token: 0x04001F04 RID: 7940
	public static PremiumAccountScreenController Instance;

	// Token: 0x04001F05 RID: 7941
	private bool ranksBefore;
}
