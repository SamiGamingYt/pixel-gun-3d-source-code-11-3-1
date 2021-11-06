using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200065A RID: 1626
public abstract class AbstractBankViewItem : MonoBehaviour
{
	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06003881 RID: 14465 RVA: 0x0012488C File Offset: 0x00122A8C
	// (set) Token: 0x06003882 RID: 14466 RVA: 0x00124894 File Offset: 0x00122A94
	public Dictionary<string, object> InappBonusParameters
	{
		get
		{
			return this.m_inappBonusParameters;
		}
		protected set
		{
			this.m_inappBonusParameters = value;
		}
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x001248A0 File Offset: 0x00122AA0
	public virtual void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, EventHandler clickHandler)
	{
		if (product == null)
		{
			Debug.LogErrorFormat("AbstractBankViewItem.Setup: product == null", new object[0]);
		}
		this.MarketProduct = product;
		List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
		if (currentInnapBonus != null)
		{
			this.InappBonusParameters = currentInnapBonus.FirstOrDefault((Dictionary<string, object> bonus) => Convert.ToString(bonus["ID"]) == product.Id);
		}
		else
		{
			this.InappBonusParameters = null;
		}
		this.purchaseInfo = newPurchaseInfo;
		this.RemovePurchaseButtonHandler();
		this.PurchaseButtonHandler = clickHandler;
		this.AddPurchaseButtonHandler();
		this.SetIcon();
	}

	// Token: 0x1700094D RID: 2381
	// (set) Token: 0x06003884 RID: 14468 RVA: 0x00124934 File Offset: 0x00122B34
	public virtual string Price
	{
		set
		{
			if (this.priceLabel != null)
			{
				this.priceLabel.text = (value ?? string.Empty);
			}
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x06003886 RID: 14470 RVA: 0x001249EC File Offset: 0x00122BEC
	// (set) Token: 0x06003885 RID: 14469 RVA: 0x00124960 File Offset: 0x00122B60
	public virtual bool isX3Item
	{
		get
		{
			return this.m_isX3Item;
		}
		set
		{
			this.m_isX3Item = value;
			for (int i = 0; i < this.x3Elements.Length; i++)
			{
				if (this.x3Elements[i] != null)
				{
					this.x3Elements[i].SetActive(value);
				}
			}
			for (int j = 0; j < this.usualElements.Length; j++)
			{
				if (this.usualElements[j] != null)
				{
					this.usualElements[j].SetActive(!value);
				}
			}
		}
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x001249F4 File Offset: 0x00122BF4
	protected static bool PaymentOccursInLastTwoWeeks()
	{
		string @string = PlayerPrefs.GetString("Last Payment Time", string.Empty);
		DateTime d;
		if (!string.IsNullOrEmpty(@string) && DateTime.TryParse(@string, out d))
		{
			TimeSpan t = DateTime.UtcNow - d;
			return t <= TimeSpan.FromDays(14.0);
		}
		return false;
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x00124A4C File Offset: 0x00122C4C
	protected virtual void Awake()
	{
		this.UpdateAdFree();
	}

	// Token: 0x06003889 RID: 14473
	protected abstract void OnEnable();

	// Token: 0x0600388A RID: 14474
	protected abstract void OnDisable();

	// Token: 0x0600388B RID: 14475 RVA: 0x00124A54 File Offset: 0x00122C54
	protected virtual void Update()
	{
		this.UpdateAdFree();
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x00124A5C File Offset: 0x00122C5C
	protected virtual void OnDestroy()
	{
		this.RemovePurchaseButtonHandler();
	}

	// Token: 0x0600388D RID: 14477 RVA: 0x00124A64 File Offset: 0x00122C64
	protected bool IsDiscounted()
	{
		return this.purchaseInfo != null && this.purchaseInfo.Discount > 0;
	}

	// Token: 0x0600388E RID: 14478
	protected abstract void SetIcon();

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x0600388F RID: 14479 RVA: 0x00124A84 File Offset: 0x00122C84
	// (set) Token: 0x06003890 RID: 14480 RVA: 0x00124A8C File Offset: 0x00122C8C
	protected IMarketProduct MarketProduct
	{
		get
		{
			return this.m_marketProduct;
		}
		set
		{
			this.m_marketProduct = value;
		}
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x00124A98 File Offset: 0x00122C98
	private void AddPurchaseButtonHandler()
	{
		try
		{
			if (this.PurchaseButtonHandler != null)
			{
				this.PurchaseButtonScript.Clicked += this.PurchaseButtonHandler;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in AddPurchaseButtonHandler: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x00124AFC File Offset: 0x00122CFC
	private void RemovePurchaseButtonHandler()
	{
		try
		{
			if (this.PurchaseButtonHandler != null)
			{
				this.PurchaseButtonScript.Clicked -= this.PurchaseButtonHandler;
				this.PurchaseButtonHandler = null;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in RemovePurchaseButtonHandler: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x06003893 RID: 14483 RVA: 0x00124B68 File Offset: 0x00122D68
	// (set) Token: 0x06003894 RID: 14484 RVA: 0x00124B70 File Offset: 0x00122D70
	private EventHandler PurchaseButtonHandler { get; set; }

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x06003895 RID: 14485 RVA: 0x00124B7C File Offset: 0x00122D7C
	private ButtonHandler PurchaseButtonScript
	{
		get
		{
			if (this.m_purchaseButtonScript == null)
			{
				this.m_purchaseButtonScript = this.btnBuy.GetComponent<ButtonHandler>();
				if (this.m_purchaseButtonScript == null)
				{
					Debug.LogErrorFormat("BankViewItem.PurchaseButtonScript: m_purchaseButtonScript == null", new object[0]);
				}
			}
			return this.m_purchaseButtonScript;
		}
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x00124BD4 File Offset: 0x00122DD4
	private void UpdateAdFree()
	{
		if (this.aDFree != null)
		{
			try
			{
				int reasonCodeToDismissInterstitialConnectScene = ConnectSceneNGUIController.GetReasonCodeToDismissInterstitialConnectScene();
				this.aDFree.SetActiveSafeSelf(reasonCodeToDismissInterstitialConnectScene == 0);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in UpdateAdFree: : {0}", new object[]
				{
					ex
				});
			}
		}
	}

	// Token: 0x04002962 RID: 10594
	public List<UILabel> inappNameLabels;

	// Token: 0x04002963 RID: 10595
	public UITexture icon;

	// Token: 0x04002964 RID: 10596
	public UILabel priceLabel;

	// Token: 0x04002965 RID: 10597
	public UIButton btnBuy;

	// Token: 0x04002966 RID: 10598
	[NonSerialized]
	public PurchaseEventArgs purchaseInfo;

	// Token: 0x04002967 RID: 10599
	public GameObject aDFree;

	// Token: 0x04002968 RID: 10600
	public GameObject[] x3Elements;

	// Token: 0x04002969 RID: 10601
	public GameObject[] usualElements;

	// Token: 0x0400296A RID: 10602
	private IMarketProduct m_marketProduct;

	// Token: 0x0400296B RID: 10603
	private Dictionary<string, object> m_inappBonusParameters;

	// Token: 0x0400296C RID: 10604
	private bool m_isX3Item;

	// Token: 0x0400296D RID: 10605
	private ButtonHandler m_purchaseButtonScript;
}
