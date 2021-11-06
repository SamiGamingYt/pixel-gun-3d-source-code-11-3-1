using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000660 RID: 1632
public class BonusBankViewItem : AbstractBankViewItem
{
	// Token: 0x060038EE RID: 14574 RVA: 0x00126C84 File Offset: 0x00124E84
	public override void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, EventHandler clickHandler)
	{
		if (product == null)
		{
			Debug.LogErrorFormat("BonusBankViewItem.Setup: product == null", new object[0]);
			return;
		}
		if (product.Id == null)
		{
			Debug.LogErrorFormat("BonusBankViewItem.Setup: product.Id == null", new object[0]);
			return;
		}
		base.Setup(product, newPurchaseInfo, clickHandler);
		try
		{
			this.Filler.Fill(this);
			object obj;
			if (base.InappBonusParameters.TryGetValue("Type", out obj) && obj != null && obj is string)
			{
				this.IsPacksLeftShown = (obj as string == "packs");
			}
			else
			{
				this.IsPacksLeftShown = false;
			}
			if (this.IsPacksLeftShown)
			{
				this.ItemsLeft = Convert.ToInt32(base.InappBonusParameters["Pack"]);
				this.UpdateItemsLeft();
			}
			else
			{
				this.TimeLeft = Convert.ToInt64(base.InappBonusParameters["End"]);
				this.UpdateTimeLabels();
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in BonusBankViewItem.Setup: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x00126DB0 File Offset: 0x00124FB0
	protected override void Update()
	{
		base.Update();
		try
		{
			List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
			Dictionary<string, object> dictionary = InappBonuessController.FindInappBonusInBonuses(base.InappBonusParameters, currentInnapBonus);
			if (dictionary == null)
			{
				Debug.LogWarningFormat("BonusBankViewItem.Update: currentInappBonusAction == null", new object[0]);
			}
			else
			{
				object value = (!this.IsPacksLeftShown) ? dictionary["End"] : dictionary["Pack"];
				bool flag;
				if (this.IsPacksLeftShown)
				{
					flag = (Convert.ToInt32(value) != this.ItemsLeft);
				}
				else
				{
					flag = (Convert.ToInt64(value) != this.TimeLeft);
				}
				if (!this.IsAnimatingItemsLeft && flag)
				{
					if (this.IsPacksLeftShown)
					{
						this.ItemsLeft = Convert.ToInt32(value);
					}
					else
					{
						this.TimeLeft = Convert.ToInt64(value);
					}
					this.itemsLeftTweener.ResetToBeginning();
					this.itemsLeftTweener.PlayForward();
					if (this.itemsLeftColorTweener != null)
					{
						this.itemsLeftColorTweener.ResetToBeginning();
						this.itemsLeftColorTweener.PlayForward();
					}
					this.IsAnimatingItemsLeft = true;
					base.StartCoroutine(this.FinishItemsLeftAnimation());
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in BonusBankViewItem.Update: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x00126F18 File Offset: 0x00125118
	private void UpdateItemsLeft()
	{
		string localizationKey = "Key_2864";
		string text = base.InappBonusParameters["action"] as string;
		string text2 = text;
		if (text2 != null)
		{
			if (BonusBankViewItem.<>f__switch$mapE == null)
			{
				BonusBankViewItem.<>f__switch$mapE = new Dictionary<string, int>(5)
				{
					{
						"currence",
						0
					},
					{
						"gadget",
						0
					},
					{
						"pet",
						1
					},
					{
						"weapon",
						1
					},
					{
						"leprechaun",
						1
					}
				};
			}
			int num;
			if (BonusBankViewItem.<>f__switch$mapE.TryGetValue(text2, out num))
			{
				if (num == 0)
				{
					goto IL_DD;
				}
				if (num == 1)
				{
					localizationKey = "Key_2896";
					goto IL_DD;
				}
			}
		}
		Debug.LogErrorFormat("UpdateItemsLeft Unknown action type:  {0}", new object[]
		{
			text
		});
		IL_DD:
		this.itemsLeftLabels.ForEach(delegate(UILabel label)
		{
			label.text = string.Format(LocalizationStore.Get(localizationKey), this.ItemsLeft.ToString());
		});
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0012701C File Offset: 0x0012521C
	private void UpdateTimeLabels()
	{
		string formattedTimeLeft = (this.TimeLeft < 86400L) ? RiliExtensions.GetTimeString(this.TimeLeft, ":") : string.Format("{0} {1}", LocalizationStore.Get("Key_1125"), RiliExtensions.GetTimeStringDays(this.TimeLeft));
		this.itemsLeftLabels.ForEach(delegate(UILabel label)
		{
			label.text = formattedTimeLeft;
		});
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x00127094 File Offset: 0x00125294
	private IEnumerator FinishItemsLeftAnimation()
	{
		yield return new WaitForRealSeconds(0.2f);
		this.IsAnimatingItemsLeft = false;
		if (this.IsPacksLeftShown)
		{
			this.UpdateItemsLeft();
		}
		else
		{
			this.UpdateTimeLabels();
		}
		yield break;
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x001270B0 File Offset: 0x001252B0
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x001270B8 File Offset: 0x001252B8
	protected override void OnEnable()
	{
		this.IsAnimatingItemsLeft = false;
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x001270C4 File Offset: 0x001252C4
	protected override void OnDisable()
	{
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x001270C8 File Offset: 0x001252C8
	protected override void SetIcon()
	{
		this.Filler.SetIcon(this);
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x060038F7 RID: 14583 RVA: 0x001270D8 File Offset: 0x001252D8
	// (set) Token: 0x060038F8 RID: 14584 RVA: 0x001270E0 File Offset: 0x001252E0
	private int ItemsLeft { get; set; }

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x060038F9 RID: 14585 RVA: 0x001270EC File Offset: 0x001252EC
	// (set) Token: 0x060038FA RID: 14586 RVA: 0x001270F4 File Offset: 0x001252F4
	private long TimeLeft { get; set; }

	// Token: 0x060038FB RID: 14587 RVA: 0x00127100 File Offset: 0x00125300
	private static string CurrencyOfProfit(IMarketProduct product)
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			return "%";
		}
		if (product == null)
		{
			Debug.LogErrorFormat("CurrencyOfProfit: product == null", new object[0]);
			return string.Empty;
		}
		return product.Currency ?? string.Empty;
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00127158 File Offset: 0x00125358
	private static decimal CalculateProfit(IMarketProduct product, Dictionary<string, object> inappBonusAction)
	{
		decimal d = Convert.ToDecimal(inappBonusAction["Profit"], CultureInfo.InvariantCulture);
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			return decimal.Multiply(100m, d);
		}
		if (product == null)
		{
			Debug.LogErrorFormat("CalculateProfit: product == null", new object[0]);
			return 0m;
		}
		return decimal.Multiply(product.PriceValue, d);
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x060038FD RID: 14589 RVA: 0x001271CC File Offset: 0x001253CC
	// (set) Token: 0x060038FE RID: 14590 RVA: 0x001271D4 File Offset: 0x001253D4
	private bool IsPacksLeftShown { get; set; }

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x060038FF RID: 14591 RVA: 0x001271E0 File Offset: 0x001253E0
	// (set) Token: 0x06003900 RID: 14592 RVA: 0x001271E8 File Offset: 0x001253E8
	private bool IsAnimatingItemsLeft { get; set; }

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x06003901 RID: 14593 RVA: 0x001271F4 File Offset: 0x001253F4
	// (set) Token: 0x06003902 RID: 14594 RVA: 0x00127328 File Offset: 0x00125528
	private BonusBankViewItem.UIFiller Filler
	{
		get
		{
			if (this.m_filler == null)
			{
				string text = base.InappBonusParameters["action"] as string;
				string text2 = text;
				switch (text2)
				{
				case "currence":
					this.m_filler = new BonusBankViewItem.CurrencyBonusUIFiller();
					goto IL_121;
				case "pet":
					this.m_filler = new BonusBankViewItem.PetBonusUIFiller();
					goto IL_121;
				case "gadget":
					this.m_filler = new BonusBankViewItem.GadgetsBonusUIFiller();
					goto IL_121;
				case "weapon":
					this.m_filler = new BonusBankViewItem.RedWeaponBonusUIFiller();
					goto IL_121;
				case "leprechaun":
					this.m_filler = new BonusBankViewItem.LeprechaunBonusUIFiller();
					goto IL_121;
				}
				Debug.LogErrorFormat("Unknown action type:  {0}", new object[]
				{
					text
				});
				this.m_filler = new BonusBankViewItem.CurrencyBonusUIFiller();
			}
			IL_121:
			return this.m_filler;
		}
		set
		{
			this.m_filler = value;
		}
	}

	// Token: 0x040029AC RID: 10668
	public GameObject currence;

	// Token: 0x040029AD RID: 10669
	public GameObject pets;

	// Token: 0x040029AE RID: 10670
	public GameObject weapon;

	// Token: 0x040029AF RID: 10671
	public GameObject gadget;

	// Token: 0x040029B0 RID: 10672
	public GameObject leprechaun;

	// Token: 0x040029B1 RID: 10673
	public UITexture petIcon;

	// Token: 0x040029B2 RID: 10674
	public UITexture leftGadget;

	// Token: 0x040029B3 RID: 10675
	public UITexture centerGadget;

	// Token: 0x040029B4 RID: 10676
	public UITexture rightGadget;

	// Token: 0x040029B5 RID: 10677
	public UITexture redWeaponIcon;

	// Token: 0x040029B6 RID: 10678
	public UITexture leprechaunIcon;

	// Token: 0x040029B7 RID: 10679
	public GameObject plus;

	// Token: 0x040029B8 RID: 10680
	public List<UILabel> descriptionLabels;

	// Token: 0x040029B9 RID: 10681
	public TweenScale itemsLeftTweener;

	// Token: 0x040029BA RID: 10682
	public TweenColor itemsLeftColorTweener;

	// Token: 0x040029BB RID: 10683
	public List<UILabel> itemsLeftLabels;

	// Token: 0x040029BC RID: 10684
	public UITable quantitiesTable;

	// Token: 0x040029BD RID: 10685
	public List<UILabel> coinsQuantityLabels;

	// Token: 0x040029BE RID: 10686
	public List<UILabel> gemsQuantityLabels;

	// Token: 0x040029BF RID: 10687
	public List<UILabel> profitLabels;

	// Token: 0x040029C0 RID: 10688
	private BonusBankViewItem.UIFiller m_filler;

	// Token: 0x02000661 RID: 1633
	private abstract class UIFiller
	{
		// Token: 0x06003904 RID: 14596 RVA: 0x0012733C File Offset: 0x0012553C
		public void Fill(BonusBankViewItem bonusBankViewItem)
		{
			if (bonusBankViewItem == null)
			{
				Debug.LogErrorFormat("CurrencyBonusUIFiller.Fill bonusBankViewItem == null", new object[0]);
				return;
			}
			InappRememberedBonus actualBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(bonusBankViewItem.InappBonusParameters);
			bonusBankViewItem.plus.SetActiveSafeSelf(actualBonus.Coins > 0 && actualBonus.Gems > 0);
			bool flag = bonusBankViewItem.purchaseInfo.Currency == "GemsCurrency";
			if (flag)
			{
				bonusBankViewItem.coinsQuantityLabels.ForEach(delegate(UILabel label)
				{
					label.text = actualBonus.Coins.ToString();
				});
				bonusBankViewItem.gemsQuantityLabels.ForEach(delegate(UILabel label)
				{
					label.text = actualBonus.Gems.ToString();
				});
				bonusBankViewItem.gemsQuantityLabels.First<UILabel>().transform.SetAsFirstSibling();
				bonusBankViewItem.coinsQuantityLabels.First<UILabel>().transform.SetAsLastSibling();
			}
			else
			{
				bonusBankViewItem.coinsQuantityLabels.ForEach(delegate(UILabel label)
				{
					label.text = actualBonus.Coins.ToString();
				});
				bonusBankViewItem.gemsQuantityLabels.ForEach(delegate(UILabel label)
				{
					label.text = actualBonus.Gems.ToString();
				});
				bonusBankViewItem.coinsQuantityLabels.First<UILabel>().transform.SetAsFirstSibling();
				bonusBankViewItem.gemsQuantityLabels.First<UILabel>().transform.SetAsLastSibling();
			}
			bonusBankViewItem.coinsQuantityLabels.First<UILabel>().gameObject.SetActiveSafeSelf(actualBonus.Coins > 0);
			bonusBankViewItem.gemsQuantityLabels.First<UILabel>().gameObject.SetActiveSafeSelf(actualBonus.Gems > 0);
			bonusBankViewItem.quantitiesTable.Reposition();
			bonusBankViewItem.currence.SetActiveSafeSelf(false);
			bonusBankViewItem.pets.SetActiveSafeSelf(false);
			bonusBankViewItem.weapon.SetActiveSafeSelf(false);
			bonusBankViewItem.gadget.SetActiveSafeSelf(false);
			bonusBankViewItem.leprechaun.SetActiveSafeSelf(false);
			bonusBankViewItem.profitLabels.ForEach(delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(false);
			});
			this.FillProfit(bonusBankViewItem);
			this.FillCore(bonusBankViewItem);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x00127544 File Offset: 0x00125744
		public void SetIcon(BonusBankViewItem bonusBankViewItem)
		{
			if (bonusBankViewItem == null)
			{
				Debug.LogErrorFormat("CurrencyBonusUIFiller.SetIcon bonusBankViewItem == null", new object[0]);
				return;
			}
			this.SetIconCore(bonusBankViewItem);
		}

		// Token: 0x06003906 RID: 14598
		protected abstract void FillCore(BonusBankViewItem bonusBankViewItem);

		// Token: 0x06003907 RID: 14599
		protected abstract void SetIconCore(BonusBankViewItem bonusBankViewItem);

		// Token: 0x06003908 RID: 14600 RVA: 0x00127578 File Offset: 0x00125778
		protected virtual void FillProfit(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				decimal d = BonusBankViewItem.CalculateProfit(bonusBankViewItem.MarketProduct, bonusBankViewItem.InappBonusParameters);
				decimal num = Math.Round(d, 0, MidpointRounding.AwayFromZero);
				string text = BonusBankViewItem.CurrencyOfProfit(bonusBankViewItem.MarketProduct);
				string text2 = num.ToString(CultureInfo.InvariantCulture);
				bool flag = text.Contains("$") || text.IndexOf("usd", StringComparison.InvariantCultureIgnoreCase) != -1;
				string arg = string.Format("{0}{1}", (!flag) ? text2 : text, (!flag) ? text : text2);
				string profitText = string.Format(LocalizationStore.Get("Key_2865"), arg);
				bonusBankViewItem.profitLabels.ForEach(delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(true);
				});
				bonusBankViewItem.profitLabels.ForEach(delegate(UILabel label)
				{
					label.text = profitText;
				});
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in UIFiller.FillProfit: {0}", new object[]
				{
					ex
				});
				bonusBankViewItem.profitLabels.ForEach(delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
			}
		}
	}

	// Token: 0x02000662 RID: 1634
	private class CurrencyBonusUIFiller : BonusBankViewItem.UIFiller
	{
		// Token: 0x0600390D RID: 14605 RVA: 0x00127704 File Offset: 0x00125904
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.currence.SetActiveSafeSelf(true);
			bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(false);
			});
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x00127748 File Offset: 0x00125948
		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.icon.gameObject.SetActiveSafeSelf(true);
		}
	}

	// Token: 0x02000663 RID: 1635
	private class PetBonusUIFiller : BonusBankViewItem.UIFiller
	{
		// Token: 0x06003911 RID: 14609 RVA: 0x00127774 File Offset: 0x00125974
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.pets.SetActiveSafeSelf(true);
			BonusBankViewItem.PetBonusUIFiller.FillDescription(bonusBankViewItem);
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x00127788 File Offset: 0x00125988
		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				bonusBankViewItem.petIcon.gameObject.SetActiveSafeSelf(true);
				Texture itemIcon = ItemDb.GetItemIcon(bonusBankViewItem.InappBonusParameters["Pet"] as string, ShopNGUIController.CategoryNames.PetsCategory, null, true);
				bonusBankViewItem.petIcon.mainTexture = itemIcon;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in PetBonusUIFiller.SetIconCore: {0}", new object[]
				{
					ex
				});
			}
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x00127818 File Offset: 0x00125A18
		protected override void FillProfit(BonusBankViewItem bonusBankViewItem)
		{
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0012781C File Offset: 0x00125A1C
		private static void FillDescription(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
				object value;
				if (bonusBankViewItem.InappBonusParameters.TryGetValue("Quantity", out value))
				{
					int num = Convert.ToInt32(value);
					object obj;
					if (bonusBankViewItem.InappBonusParameters.TryGetValue("Pet", out obj))
					{
						string text = obj as string;
						if (text.IsNullOrEmpty())
						{
							Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no petId.IsNullOrEmpty() in parameters: {0}", new object[]
							{
								Json.Serialize(bonusBankViewItem.InappBonusParameters)
							});
						}
						else
						{
							string petName = null;
							string text2 = text;
							if (text2 == Singleton<PetsManager>.Instance.GetIdWithoutUp(text2))
							{
								text2 += "_up0";
							}
							PetInfo petInfo;
							if (PetsInfo.info.TryGetValue(text2, out petInfo) && petInfo != null)
							{
								petName = LocalizationStore.Get(petInfo.Lkey);
								if (petName.IsNullOrEmpty())
								{
									Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no petName.IsNullOrEmpty() in parameters: {0}", new object[]
									{
										Json.Serialize(bonusBankViewItem.InappBonusParameters)
									});
								}
								else
								{
									bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
									{
										label.text = string.Format(LocalizationStore.Get("Key_2903"), petName);
									});
									bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
									{
										label.gameObject.SetActiveSafeSelf(true);
									});
								}
							}
							else
							{
								Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no such pet {0}, parameters of action = {1}", new object[]
								{
									text,
									Json.Serialize(bonusBankViewItem.InappBonusParameters)
								});
							}
						}
					}
					else
					{
						Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no Pet in parameters: {0}", new object[]
						{
							Json.Serialize(bonusBankViewItem.InappBonusParameters)
						});
					}
				}
				else
				{
					Debug.LogErrorFormat("PetBonusUIFiller.FillCore: no quantity in parameters: {0}", new object[]
					{
						Json.Serialize(bonusBankViewItem.InappBonusParameters)
					});
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in FillDescription: {0}", new object[]
				{
					ex
				});
				bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
			}
		}
	}

	// Token: 0x02000664 RID: 1636
	private class RedWeaponBonusUIFiller : BonusBankViewItem.UIFiller
	{
		// Token: 0x06003919 RID: 14617 RVA: 0x00127AB4 File Offset: 0x00125CB4
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.weapon.SetActiveSafeSelf(true);
			bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
			{
				label.text = LocalizationStore.Get("Key_2891");
			});
			bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(true);
			});
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x00127B20 File Offset: 0x00125D20
		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				bonusBankViewItem.redWeaponIcon.gameObject.SetActiveSafeSelf(true);
				string tag = bonusBankViewItem.InappBonusParameters["Weapon"] as string;
				Texture itemIcon = ItemDb.GetItemIcon(tag, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tag), null, true);
				bonusBankViewItem.redWeaponIcon.mainTexture = itemIcon;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in RedWeaponBonusUIFiller.SetIconCore: {0}", new object[]
				{
					ex
				});
			}
		}
	}

	// Token: 0x02000665 RID: 1637
	private class GadgetsBonusUIFiller : BonusBankViewItem.UIFiller
	{
		// Token: 0x0600391E RID: 14622 RVA: 0x00127BE0 File Offset: 0x00125DE0
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.gadget.SetActiveSafeSelf(true);
			bool isGems = true;
			try
			{
				InappRememberedBonus actualBonusSizeForInappBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(bonusBankViewItem.InappBonusParameters);
				if (actualBonusSizeForInappBonus.Coins > 0)
				{
					isGems = false;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in GadgetsBonusUIFiller.FillCore: {0}", new object[]
				{
					ex
				});
			}
			bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
			{
				label.text = ((!isGems) ? LocalizationStore.Get("Key_2904") : LocalizationStore.Get("Key_2904"));
			});
			bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(true);
			});
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x00127CA4 File Offset: 0x00125EA4
		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
			try
			{
				List<string> list = (bonusBankViewItem.InappBonusParameters["Gadgets"] as List<string>).OfType<string>().ToList<string>();
				switch (list.Count)
				{
				case 1:
					bonusBankViewItem.leftGadget.mainTexture = null;
					bonusBankViewItem.centerGadget.mainTexture = ItemDb.GetItemIcon(list[0], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[0]), null, true);
					bonusBankViewItem.rightGadget.mainTexture = null;
					break;
				case 2:
					bonusBankViewItem.leftGadget.mainTexture = ItemDb.GetItemIcon(list[0], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[0]), null, true);
					bonusBankViewItem.centerGadget.mainTexture = null;
					bonusBankViewItem.rightGadget.mainTexture = ItemDb.GetItemIcon(list[1], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[1]), null, true);
					break;
				case 3:
					bonusBankViewItem.leftGadget.mainTexture = ItemDb.GetItemIcon(list[0], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[0]), null, true);
					bonusBankViewItem.centerGadget.mainTexture = ItemDb.GetItemIcon(list[1], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[1]), null, true);
					bonusBankViewItem.rightGadget.mainTexture = ItemDb.GetItemIcon(list[2], (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(list[2]), null, true);
					break;
				default:
					Debug.LogErrorFormat("GadgetsBonusUIFiller.FillCore: unsupported number of gadgets {0}", new object[]
					{
						list.Count
					});
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in GadgetsBonusUIFiller.FillCore: {0}", new object[]
				{
					ex
				});
			}
		}
	}

	// Token: 0x02000666 RID: 1638
	private class LeprechaunBonusUIFiller : BonusBankViewItem.UIFiller
	{
		// Token: 0x06003922 RID: 14626 RVA: 0x00127EB0 File Offset: 0x001260B0
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
			bonusBankViewItem.leprechaun.SetActiveSafeSelf(true);
			try
			{
				InappRememberedBonus actualBonusSizeForInappBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(bonusBankViewItem.InappBonusParameters);
				int perDayCurrency = actualBonusSizeForInappBonus.PerDayLeprechaun;
				if (perDayCurrency > 0)
				{
					bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
					{
						label.text = string.Format(LocalizationStore.Get("Key_2898"), perDayCurrency);
					});
					bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
					{
						label.gameObject.SetActiveSafeSelf(true);
					});
				}
				else
				{
					Debug.LogErrorFormat("Error in LeprechaunBonusUIFiller FillCore: perDayCurrency <= 0,  {0}", new object[]
					{
						Json.Serialize(bonusBankViewItem.InappBonusParameters)
					});
					bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
					{
						label.gameObject.SetActiveSafeSelf(false);
					});
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in LeprechaunBonusUIFiller FillCore: {0}", new object[]
				{
					ex
				});
				bonusBankViewItem.descriptionLabels.ForEach(delegate(UILabel label)
				{
					label.gameObject.SetActiveSafeSelf(false);
				});
			}
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x00127FE8 File Offset: 0x001261E8
		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
		}
	}

	// Token: 0x02000667 RID: 1639
	private class UniqueWeaponBonusUIFiller : BonusBankViewItem.UIFiller
	{
		// Token: 0x06003928 RID: 14632 RVA: 0x00128024 File Offset: 0x00126224
		protected override void FillCore(BonusBankViewItem bonusBankViewItem)
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x00128028 File Offset: 0x00126228
		protected override void SetIconCore(BonusBankViewItem bonusBankViewItem)
		{
		}
	}
}
