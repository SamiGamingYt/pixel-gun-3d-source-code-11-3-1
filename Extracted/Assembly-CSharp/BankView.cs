using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200065D RID: 1629
internal class BankView : AbstractBankView
{
	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x060038D1 RID: 14545 RVA: 0x00125DC4 File Offset: 0x00123FC4
	// (set) Token: 0x060038D2 RID: 14546 RVA: 0x00125DCC File Offset: 0x00123FCC
	public override bool AreBankContentsEnabled
	{
		get
		{
			return this.m_areBankContentsEnabled;
		}
		set
		{
			bool areBankContentsEnabled = this.m_areBankContentsEnabled;
			this.m_areBankContentsEnabled = value;
			this.btnTabContainer.SetActiveSafeSelf(value);
			if (value)
			{
				if (!areBankContentsEnabled)
				{
					bool isEnabled = this.btnTabGold.isEnabled;
					this.goldScrollView.gameObject.SetActiveSafeSelf(!isEnabled);
					this.gemsScrollView.gameObject.SetActiveSafeSelf(isEnabled);
					this.UpdateUi();
					this.ResetScrollView(isEnabled);
				}
			}
			else
			{
				this.goldScrollView.gameObject.SetActiveSafeSelf(value);
				this.gemsScrollView.gameObject.SetActiveSafeSelf(value);
			}
		}
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x00125E64 File Offset: 0x00124064
	protected override void HandleNoStoreKitEventListener()
	{
		if (this.goldItemPrefab != null)
		{
			this.goldItemPrefab.gameObject.SetActive(false);
		}
		if (this.gemsItemPrefab != null)
		{
			this.gemsItemPrefab.gameObject.SetActive(false);
		}
		if (this.inappBonusItemPrefab != null)
		{
			this.inappBonusItemPrefab.gameObject.SetActive(false);
		}
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x00125ED8 File Offset: 0x001240D8
	protected override void OnEnable()
	{
		this.UpdateUi();
		UIButton btnTab = this.btnTabGems;
		if (coinsShop.thisScript != null && coinsShop.thisScript.notEnoughCurrency == "Coins")
		{
			btnTab = this.btnTabGold;
		}
		else if (coinsShop.thisScript != null && coinsShop.thisScript.notEnoughCurrency == "GemsCurrency")
		{
			btnTab = this.btnTabGems;
		}
		else if (base.DesiredCurrency != null)
		{
			btnTab = ((!(base.DesiredCurrency == "GemsCurrency")) ? this.btnTabGold : this.btnTabGems);
		}
		else
		{
			try
			{
				List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
				bool flag;
				if (currentInnapBonus != null && currentInnapBonus.Count<Dictionary<string, object>>() > 0)
				{
					flag = currentInnapBonus.All(delegate(Dictionary<string, object> inappBonus)
					{
						InappRememberedBonus actualBonusSizeForInappBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(inappBonus);
						return actualBonusSizeForInappBonus != null && !actualBonusSizeForInappBonus.InappId.IsNullOrEmpty() && StoreKitEventListener.coinIds.Contains(actualBonusSizeForInappBonus.InappId);
					});
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					btnTab = this.btnTabGold;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in BankView.OnEnable GetCurrenceCurrentInnapBonus: {0}", new object[]
				{
					ex
				});
			}
		}
		base.DesiredCurrency = null;
		this.OnBtnTabClick(btnTab);
		if (this.connectionProblemLabel != null)
		{
			this.connectionProblemLabel.text = LocalizationStore.Get("Key_0278");
		}
		base.OnEnable();
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x00126054 File Offset: 0x00124254
	protected override void Start()
	{
		UIPanel component = this.goldScrollView.GetComponent<UIPanel>();
		component.UpdateAnchors();
		UIPanel component2 = this.gemsScrollView.GetComponent<UIPanel>();
		component2.UpdateAnchors();
		this.ResetScrollView(false);
		this.ResetScrollView(true);
		base.Start();
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0012609C File Offset: 0x0012429C
	private static void ClearGrid(UIGrid itemGrid)
	{
		while (itemGrid.transform.childCount > 0)
		{
			Transform child = itemGrid.transform.GetChild(0);
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x001260E0 File Offset: 0x001242E0
	private void PopulateItemGrid(bool isGems, List<Dictionary<string, object>> inappBonusActions)
	{
		IList<PurchaseEventArgs> list;
		if (isGems)
		{
			IList<PurchaseEventArgs> gemsPurchasesInfo = AbstractBankView.gemsPurchasesInfo;
			list = gemsPurchasesInfo;
		}
		else
		{
			list = AbstractBankView.goldPurchasesInfo;
		}
		IList<PurchaseEventArgs> list2 = list;
		UIGrid uigrid = (!isGems) ? this.goldItemGrid : this.gemsItemGrid;
		AbstractBankViewItem abstractBankViewItem = (!isGems) ? this.goldItemPrefab : this.gemsItemPrefab;
		abstractBankViewItem.gameObject.SetActiveSafeSelf(true);
		for (int i = 0; i < list2.Count; i++)
		{
			AbstractBankViewItem abstractBankViewItem2 = UnityEngine.Object.Instantiate<AbstractBankViewItem>(abstractBankViewItem);
			abstractBankViewItem2.transform.SetParent(uigrid.transform);
			abstractBankViewItem2.transform.localScale = Vector3.one;
			abstractBankViewItem2.transform.localPosition = Vector3.zero;
			abstractBankViewItem2.transform.localRotation = Quaternion.identity;
			this.UpdateItem(abstractBankViewItem2, list2[i]);
		}
		abstractBankViewItem.gameObject.SetActiveSafeSelf(false);
		if (inappBonusActions != null)
		{
			for (int j = 0; j < inappBonusActions.Count; j++)
			{
				Dictionary<string, object> dictionary = inappBonusActions[j];
				PurchaseEventArgs purchaseEventArgs = null;
				InappRememberedBonus actualBonusSizeForInappBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(dictionary);
				if (actualBonusSizeForInappBonus != null)
				{
					if (!actualBonusSizeForInappBonus.InappId.IsNullOrEmpty())
					{
						int indexOfInappWithBonus = Array.IndexOf<string>((!isGems) ? StoreKitEventListener.coinIds : StoreKitEventListener.gemsIds, actualBonusSizeForInappBonus.InappId);
						if (indexOfInappWithBonus != -1)
						{
							purchaseEventArgs = list2.FirstOrDefault((PurchaseEventArgs purchaseInfo) => purchaseInfo.Index == indexOfInappWithBonus);
							if (purchaseEventArgs == null)
							{
								Debug.LogErrorFormat("PopulateItemGrid inappBonusPurchaseInfo == null isGems = {0} , inappBonusAction = {1}, bonus.InappId = {2}", new object[]
								{
									isGems,
									Json.Serialize(dictionary),
									actualBonusSizeForInappBonus.InappId
								});
							}
						}
					}
					else
					{
						Debug.LogErrorFormat("PopulateItemGrid: bonus.InappId.IsNullOrEmpty() isGems = {0} , inappBonusAction = {1}", new object[]
						{
							isGems,
							Json.Serialize(dictionary)
						});
					}
				}
				else
				{
					Debug.LogErrorFormat("PopulateItemGrid: bonus == null isGems = {0} , inappBonusAction = {1}", new object[]
					{
						isGems,
						Json.Serialize(dictionary)
					});
				}
				if (purchaseEventArgs != null)
				{
					this.inappBonusItemPrefab.gameObject.SetActiveSafeSelf(true);
					AbstractBankViewItem abstractBankViewItem3 = UnityEngine.Object.Instantiate<AbstractBankViewItem>(this.inappBonusItemPrefab);
					abstractBankViewItem3.transform.SetParent(uigrid.transform);
					abstractBankViewItem3.transform.localScale = Vector3.one;
					abstractBankViewItem3.transform.localPosition = Vector3.zero;
					abstractBankViewItem3.transform.localRotation = Quaternion.identity;
					this.UpdateItem(abstractBankViewItem3, purchaseEventArgs);
				}
			}
		}
		this.inappBonusItemPrefab.gameObject.SetActiveSafeSelf(false);
		this.ResetScrollView(isGems);
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x00126378 File Offset: 0x00124578
	public void OnBtnTabClick(UIButton btnTab)
	{
		bool flag = btnTab == this.btnTabGems;
		this.btnTabGold.isEnabled = flag;
		this.btnTabGems.isEnabled = !flag;
		this.goldScrollView.gameObject.SetActiveSafeSelf(!flag);
		this.gemsScrollView.gameObject.SetActiveSafeSelf(flag);
		this.ResetScrollView(flag);
		if (btnTab != this.btnTabGold && btnTab != this.btnTabGems)
		{
			Debug.LogErrorFormat("Unknown btnTab", new object[0]);
		}
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x0012640C File Offset: 0x0012460C
	public override void UpdateUi()
	{
		BankView.ClearGrid(this.goldItemGrid);
		BankView.ClearGrid(this.gemsItemGrid);
		if (!this.AreBankContentsEnabled)
		{
			this.IsX3Bank = false;
			return;
		}
		List<Dictionary<string, object>> inappBonusActions = null;
		try
		{
			inappBonusActions = BalanceController.GetCurrentInnapBonus();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateUi BalanceController.GetCurrentInnapBonus: {0}", new object[]
			{
				ex
			});
		}
		this.PopulateItemGrid(false, inappBonusActions);
		this.PopulateItemGrid(true, inappBonusActions);
		this.SortItemGrid(false, inappBonusActions);
		this.SortItemGrid(true, inappBonusActions);
		try
		{
			this.IsX3Bank = (PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active);
		}
		catch (Exception ex2)
		{
			Debug.LogErrorFormat("Exception in UpdateUi: {0}", new object[]
			{
				ex2
			});
		}
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x00126500 File Offset: 0x00124700
	private void SortItemGrid(bool isGems, List<Dictionary<string, object>> inappBonusActions)
	{
		UIGrid uigrid = (!isGems) ? this.goldItemGrid : this.gemsItemGrid;
		Transform transform = uigrid.transform;
		List<AbstractBankViewItem> list = new List<AbstractBankViewItem>();
		for (int i = 0; i < transform.childCount; i++)
		{
			AbstractBankViewItem component = transform.GetChild(i).GetComponent<AbstractBankViewItem>();
			list.Add(component);
		}
		list.Sort(new BankView.ItemsComparer(inappBonusActions));
		for (int j = 0; j < list.Count; j++)
		{
			list[j].gameObject.name = string.Format("{0:00}", j);
		}
		this.ResetScrollView(isGems);
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x001265B0 File Offset: 0x001247B0
	private void ResetScrollView(bool isGems)
	{
		UIScrollView uiscrollView = (!isGems) ? this.goldScrollView : this.gemsScrollView;
		UIGrid uigrid = (!isGems) ? this.goldItemGrid : this.gemsItemGrid;
		uigrid.Reposition();
		uiscrollView.ResetPosition();
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x001265FC File Offset: 0x001247FC
	protected override IEnumerable<AbstractBankViewItem> AllItems()
	{
		return (this.gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).Concat(this.goldItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]);
	}

	// Token: 0x04002993 RID: 10643
	public GameObject btnTabContainer;

	// Token: 0x04002994 RID: 10644
	public UIButton btnTabGold;

	// Token: 0x04002995 RID: 10645
	public UIButton btnTabGems;

	// Token: 0x04002996 RID: 10646
	public UIScrollView goldScrollView;

	// Token: 0x04002997 RID: 10647
	public UIGrid goldItemGrid;

	// Token: 0x04002998 RID: 10648
	public AbstractBankViewItem goldItemPrefab;

	// Token: 0x04002999 RID: 10649
	public UIScrollView gemsScrollView;

	// Token: 0x0400299A RID: 10650
	public UIGrid gemsItemGrid;

	// Token: 0x0400299B RID: 10651
	public AbstractBankViewItem gemsItemPrefab;

	// Token: 0x0400299C RID: 10652
	public AbstractBankViewItem inappBonusItemPrefab;

	// Token: 0x0400299D RID: 10653
	private bool m_areBankContentsEnabled;

	// Token: 0x0200065E RID: 1630
	public sealed class ItemsComparer : IComparer<AbstractBankViewItem>
	{
		// Token: 0x060038DE RID: 14558 RVA: 0x00126678 File Offset: 0x00124878
		public ItemsComparer(List<Dictionary<string, object>> inappBonusActions)
		{
			this.m_inappBonusActions = inappBonusActions;
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x00126688 File Offset: 0x00124888
		public int Compare(AbstractBankViewItem x, AbstractBankViewItem y)
		{
			if (x is BonusBankViewItem && y is BonusBankViewItem)
			{
				if (this.m_inappBonusActions == null)
				{
					return 0;
				}
				try
				{
					BonusBankViewItem bonusBankViewItem = x as BonusBankViewItem;
					BonusBankViewItem bonusBankViewItem2 = y as BonusBankViewItem;
					string xUniqueId = bonusBankViewItem.InappBonusParameters["Key"] as string;
					string yUniqueId = bonusBankViewItem2.InappBonusParameters["Key"] as string;
					return this.m_inappBonusActions.FindIndex((Dictionary<string, object> bonus) => bonus["Key"] as string == xUniqueId).CompareTo(this.m_inappBonusActions.FindIndex((Dictionary<string, object> bonus) => bonus["Key"] as string == yUniqueId));
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in ItemsComparer.Compare: {0}", new object[]
					{
						ex
					});
					return 0;
				}
			}
			if (x is BonusBankViewItem)
			{
				return -1;
			}
			if (y is BonusBankViewItem)
			{
				return 1;
			}
			int value = (!(y != null)) ? 0 : y.purchaseInfo.Count;
			return (!StoreKitEventListener.IsPayingUser()) ? x.purchaseInfo.Count.CompareTo(value) : value.CompareTo(x.purchaseInfo.Count);
		}

		// Token: 0x0400299F RID: 10655
		private List<Dictionary<string, object>> m_inappBonusActions;
	}
}
