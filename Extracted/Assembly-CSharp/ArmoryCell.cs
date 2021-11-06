using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class ArmoryCell : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000086 RID: 134 RVA: 0x000051E4 File Offset: 0x000033E4
	// (remove) Token: 0x06000087 RID: 135 RVA: 0x000051FC File Offset: 0x000033FC
	public static event Action<ArmoryCell> ToggleValueChanged;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000088 RID: 136 RVA: 0x00005214 File Offset: 0x00003414
	// (remove) Token: 0x06000089 RID: 137 RVA: 0x0000522C File Offset: 0x0000342C
	public static event Action<ArmoryCell> Clicked;

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00005244 File Offset: 0x00003444
	// (set) Token: 0x0600008B RID: 139 RVA: 0x0000524C File Offset: 0x0000344C
	public string ItemId { get; private set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600008C RID: 140 RVA: 0x00005258 File Offset: 0x00003458
	// (set) Token: 0x0600008D RID: 141 RVA: 0x00005260 File Offset: 0x00003460
	public ShopNGUIController.CategoryNames Category { get; private set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600008E RID: 142 RVA: 0x0000526C File Offset: 0x0000346C
	public bool IsFullyVisible
	{
		get
		{
			return this.bottomBorderIndicatorSprite.isVisible && this.topBorderIndicatorSprite.isVisible;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x0000528C File Offset: 0x0000348C
	public void MakeCellEmpty()
	{
		this.IsBest = false;
		this.isEmpty = true;
		base.StopAllCoroutines();
		this.selectionIndicator.SetActiveSafeSelf(false);
		this.newLabel.SetActiveSafeSelf(false);
		this.topSeller.SetActiveSafeSelf(false);
		this.newSkinLabel.SetActiveSafeSelf(false);
		this.rented.SetActiveSafeSelf(false);
		this.isPriceForUpgrade.SetActiveSafeSelf(false);
		this.equipped.SetActiveSafeSelf(false);
		this.modelForSkin.SetActiveSafeSelf(false);
		this.upgradesContainer.SetActiveSafeSelf(false);
		this.priceLabel.gameObject.SetActiveSafeSelf(false);
		this.gemSprite.SetActiveSafeSelf(false);
		this.coinSprite.SetActiveSafeSelf(false);
		this.discountSprite.gameObject.SetActiveSafeSelf(false);
		this.boughtIndicator.alpha = 0f;
		this.icon.gameObject.SetActiveSafeSelf(false);
		this.toggle.enabled = false;
		this.petName.ForEach(delegate(UILabel lab)
		{
			lab.gameObject.SetActiveSafeSelf(false);
		});
		this.petLevel.ForEach(delegate(UILabel lab)
		{
			lab.gameObject.SetActiveSafeSelf(false);
		});
		this.eggCondition.ForEach(delegate(UILabel lab)
		{
			lab.gameObject.SetActiveSafeSelf(false);
		});
		this.leagueRating.ForEach(delegate(UILabel lab)
		{
			lab.gameObject.SetActiveSafeSelf(false);
		});
		this.lockSprite.SetActiveSafeSelf(false);
		this.darkForeground.SetActiveSafeSelf(false);
		this.unlocked.SetActiveSafeSelf(false);
		this.UnsubscribeEquipEvents();
	}

	// Token: 0x06000090 RID: 144 RVA: 0x0000544C File Offset: 0x0000364C
	public void SetupEmptyCellCategory(ShopNGUIController.CategoryNames category)
	{
		this.Category = category;
		ShopNGUIController.CategoryNames category2 = this.Category;
		switch (category2)
		{
		case ShopNGUIController.CategoryNames.PrimaryCategory:
		case ShopNGUIController.CategoryNames.BackupCategory:
		case ShopNGUIController.CategoryNames.MeleeCategory:
		case ShopNGUIController.CategoryNames.SpecilCategory:
		case ShopNGUIController.CategoryNames.SniperCategory:
		case ShopNGUIController.CategoryNames.PremiumCategory:
			break;
		case ShopNGUIController.CategoryNames.HatsCategory:
		case ShopNGUIController.CategoryNames.CapesCategory:
		case ShopNGUIController.CategoryNames.BootsCategory:
		case ShopNGUIController.CategoryNames.MaskCategory:
			goto IL_167;
		default:
			if (category2 != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
			{
				if (category2 == ShopNGUIController.CategoryNames.LeagueHatsCategory)
				{
					goto IL_167;
				}
				if (category2 != ShopNGUIController.CategoryNames.LeagueSkinsCategory)
				{
					if (category2 != ShopNGUIController.CategoryNames.ThrowingCategory && category2 != ShopNGUIController.CategoryNames.ToolsCategoty && category2 != ShopNGUIController.CategoryNames.SupportCategory)
					{
						if (category2 == ShopNGUIController.CategoryNames.EggsCategory)
						{
							this.icon.gameObject.SetActiveSafeSelf(true);
							this.icon.mainTexture = Resources.Load<Texture>("OfferIcons/Empty_Egg_Icon");
							return;
						}
						if (category2 == ShopNGUIController.CategoryNames.BestWeapons)
						{
							break;
						}
						if (category2 == ShopNGUIController.CategoryNames.BestWear)
						{
							goto IL_167;
						}
						if (category2 != ShopNGUIController.CategoryNames.BestGadgets)
						{
							return;
						}
					}
					this.icon.gameObject.SetActiveSafeSelf(true);
					this.icon.mainTexture = Resources.Load<Texture>("OfferIcons/Empty_Gadget_Icon");
					return;
				}
				goto IL_111;
			}
			break;
		case ShopNGUIController.CategoryNames.SkinsCategory:
			goto IL_111;
		}
		this.icon.gameObject.SetActiveSafeSelf(true);
		this.icon.mainTexture = Resources.Load<Texture>("OfferIcons/Empty_Weapon_Icon");
		return;
		IL_111:
		this.icon.gameObject.SetActiveSafeSelf(true);
		this.icon.mainTexture = Resources.Load<Texture>("OfferIcons/Empty_Skin_Icon");
		return;
		IL_167:
		this.icon.gameObject.SetActiveSafeSelf(true);
		this.icon.mainTexture = Resources.Load<Texture>("OfferIcons/Empty_Wear_Icon");
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000055EC File Offset: 0x000037EC
	public void ReSubscribeToEquipEvents()
	{
		this.UnsubscribeEquipEvents();
		this.SubscribeToEquipEvents();
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000092 RID: 146 RVA: 0x000055FC File Offset: 0x000037FC
	// (set) Token: 0x06000093 RID: 147 RVA: 0x00005604 File Offset: 0x00003804
	public bool IsBest { get; private set; }

	// Token: 0x06000094 RID: 148 RVA: 0x00005610 File Offset: 0x00003810
	internal void Setup(ShopNGUIController.ShopItem item, bool isBest)
	{
		this.IsBest = isBest;
		this.isEmpty = false;
		this.ItemId = item.Id;
		this.Category = item.Category;
		this.lastBoughtTag = string.Empty;
		this.firstTagForOurTier = string.Empty;
		this.UpdateFirstUnbought();
		if (ShopNGUIController.IsWeaponCategory(this.Category) || ShopNGUIController.IsWearCategory(this.Category))
		{
			this.lastBoughtTag = WeaponManager.LastBoughtTag(this.ItemId, null);
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.Category))
		{
			this.lastBoughtTag = GadgetsInfo.LastBoughtFor(this.ItemId);
		}
		else if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.lastBoughtTag = ((!PetsManager.IsEmptySlotId(this.ItemId)) ? this.ItemId : string.Empty);
		}
		if (ShopNGUIController.IsWeaponCategory(this.Category))
		{
			this.upgradesChain = WeaponUpgrades.ChainForTag(this.ItemId);
			if (this.upgradesChain != null)
			{
				this.firstTagForOurTier = WeaponManager.FirstTagForOurTier(this.ItemId, null);
			}
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.Category))
		{
			this.upgradesChain = GadgetsInfo.UpgradesChainForGadget(this.ItemId);
			if (this.upgradesChain != null)
			{
				this.firstTagForOurTier = GadgetsInfo.FirstForOurTier(this.ItemId);
			}
		}
		this.lastBoughtUpdateCounter = 0;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0000577C File Offset: 0x0000397C
	public void UpdateAllAndStartUpdateCoroutine()
	{
		if (this.isEmpty)
		{
			return;
		}
		this.toggle.enabled = (this.Category != ShopNGUIController.CategoryNames.PetsCategory || !PetsManager.IsEmptySlotId(this.ItemId));
		this.selectionIndicator.SetActiveSafeSelf(this.Category != ShopNGUIController.CategoryNames.EggsCategory && (this.Category != ShopNGUIController.CategoryNames.PetsCategory || !PetsManager.IsEmptySlotId(this.ItemId)));
		bool state = this.ItemId == "cape_Custom" && Storager.getInt("cape_Custom", true) > 0;
		this.capeRenderer.gameObject.SetActiveSafeSelf(state);
		bool state2 = this.ItemId == "CustomSkinID" && !this.IsUnboughtSkinsEditor();
		this.newSkinLabel.SetActiveSafeSelf(state2);
		this.petName.ForEach(delegate(UILabel lab)
		{
			lab.gameObject.SetActiveSafeSelf(this.Category == ShopNGUIController.CategoryNames.EggsCategory);
		});
		this.petLevel.ForEach(delegate(UILabel lab)
		{
			lab.gameObject.SetActiveSafeSelf(this.Category == ShopNGUIController.CategoryNames.PetsCategory && !PetsManager.IsEmptySlotId(this.ItemId));
		});
		if (this.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			Egg egg = Singleton<EggsManager>.Instance.GetPlayerEggs().FirstOrDefault((Egg e) => e.Id.ToString() == this.ItemId);
			this.eggCondition.ForEach(delegate(UILabel lab)
			{
				lab.gameObject.SetActiveSafeSelf(egg != null && egg.Data.Rare != EggRarity.Champion);
			});
			if (egg == null)
			{
				Debug.LogErrorFormat("UpdateAllAndStartUpdateCoroutine: egg == null, ItemId = {0}", new object[]
				{
					this.ItemId ?? "null"
				});
			}
		}
		else
		{
			this.eggCondition.ForEach(delegate(UILabel lab)
			{
				lab.gameObject.SetActiveSafeSelf(false);
			});
		}
		if (this.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			Egg egg = Singleton<EggsManager>.Instance.GetPlayerEggs().FirstOrDefault((Egg e) => e.Id.ToString() == this.ItemId);
			this.leagueRating.ForEach(delegate(UILabel lab)
			{
				lab.gameObject.SetActiveSafeSelf(egg != null && egg.Data.Rare == EggRarity.Champion);
			});
			if (egg == null)
			{
				Debug.LogErrorFormat("UpdateAllAndStartUpdateCoroutine: egg == null, ItemId = {0}", new object[]
				{
					this.ItemId ?? "null"
				});
			}
		}
		else
		{
			this.leagueRating.ForEach(delegate(UILabel lab)
			{
				lab.gameObject.SetActiveSafeSelf((this.Category == ShopNGUIController.CategoryNames.SkinsCategory && !SkinsController.IsLeagueSkinAvailableByLeague(this.ItemId) && !SkinsController.IsSkinBought(this.ItemId)) || (this.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory && !WeaponSkinsManager.IsAvailableByLeague(this.ItemId) && !WeaponSkinsManager.IsBoughtSkin(this.ItemId)) || (this.Category == ShopNGUIController.CategoryNames.HatsCategory && Wear.LeagueForWear(this.ItemId, ShopNGUIController.CategoryNames.HatsCategory) > (int)RatingSystem.instance.currentLeague && Storager.getInt(this.ItemId, true) == 0));
			});
		}
		bool state3 = this.Category == ShopNGUIController.CategoryNames.SkinsCategory && !this.IsUnboughtSkinsEditor();
		this.modelForSkin.SetActiveSafeSelf(state3);
		bool state4 = (this.ItemId != "cape_Custom" || Storager.getInt("cape_Custom", true) == 0) && (this.Category != ShopNGUIController.CategoryNames.SkinsCategory || this.IsUnboughtSkinsEditor());
		this.icon.gameObject.SetActiveSafeSelf(state4);
		if (this.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			if (this.IsUnboughtSkinsEditor())
			{
				this.SetIcon();
			}
			else
			{
				try
				{
					this.modelForSkin.GetComponent<MeshRenderer>().material.mainTexture = ((!(this.ItemId == "CustomSkinID")) ? SkinsController.skinsForPers[this.ItemId] : Resources.Load<Texture>("skins_maker_skin"));
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception in ArmoryCell SetTextureRecursivelyFrom: " + arg);
				}
			}
		}
		else if (this.ItemId == "cape_Custom" && Storager.getInt("cape_Custom", true) > 0)
		{
			try
			{
				if (SkinsController.capeUserTexture != null)
				{
					this.capeRenderer.material.mainTexture = SkinsController.capeUserTexture;
				}
			}
			catch (Exception arg2)
			{
				Debug.LogError("Exception in ArmoryCell capeRenderer.material.mainTexture = : " + arg2);
			}
		}
		else
		{
			this.SetIcon();
		}
		if (ShopNGUIController.IsWeaponCategory(this.Category))
		{
			WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(this.ItemId);
			if (weaponInfo != null)
			{
				Color color = this.ColorForRarity(weaponInfo.rarity);
				this.coloredBorder.color = color;
				string prefabName = ItemDb.GetByTag(this.ItemId).PrefabName;
				this.SetEquipped(WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().Any((Weapon w) => w.weaponPrefab.nameNoClone() == prefabName));
			}
		}
		else
		{
			this.coloredBorder.color = this.ColorForRarity(ItemDb.ItemRarity.Common);
		}
		if (this.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			try
			{
				WeaponSkin skin = WeaponSkinsManager.GetSkin(this.ItemId);
				if (skin != null)
				{
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(skin.ToWeapons[0]);
					if (byPrefabName != null)
					{
						this.HandleWeaponSkinsManager_EquippedSkinForWeapon(byPrefabName, WeaponSkinsManager.GetSettedSkinId(byPrefabName.PrefabName));
					}
					else
					{
						this.SetEquipped(false);
					}
				}
				else
				{
					this.SetEquipped(false);
				}
			}
			catch (Exception arg3)
			{
				Debug.LogError("Exception in initial setting equipped weapon skin in ArmoryCell: " + arg3);
			}
		}
		if (ShopNGUIController.IsWearCategory(this.Category))
		{
			this.SetEquipped(Storager.getString(ShopNGUIController.SnForWearCategory(this.Category), false) == this.ItemId);
		}
		if (this.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.HandleSkinsController_EquippedSkin(SkinsController.currentSkinNameForPers ?? string.Empty, string.Empty);
			this.upgradesContainer.SetActiveSafeSelf(false);
		}
		if (this.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory || this.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			this.upgradesContainer.SetActiveSafeSelf(false);
		}
		if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			this.upgradesContainer.SetActiveSafeSelf(false);
			this.SetEquipped(Singleton<PetsManager>.Instance.GetEqipedPetId() == this.ItemId);
		}
		if (ShopNGUIController.IsGadgetsCategory(this.Category))
		{
			this.SetEquipped(this.ItemId == GadgetsInfo.EquippedForCategory((GadgetInfo.GadgetCategory)this.Category));
		}
		if (this.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			this.SetEquipped(false);
		}
		this.StartUpdateInfo();
		this.UpdateRented();
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00005DF4 File Offset: 0x00003FF4
	public void ToggleClicked()
	{
		bool value = this.toggle.value;
		this.UpdatePriceAndDiscount();
		this.UpdateUpgrades();
		if (!value)
		{
			return;
		}
		Action<ArmoryCell> toggleValueChanged = ArmoryCell.ToggleValueChanged;
		if (toggleValueChanged != null)
		{
			toggleValueChanged(this);
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00005E34 File Offset: 0x00004034
	public void UpdateDiscountVisibility()
	{
		this.discountSprite.alpha = ((!this.toggle.value) ? 0f : 1f);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00005E6C File Offset: 0x0000406C
	private void HandleWeaponSkinsManager_EquippedSkinForWeapon(ItemRecord weaponRecord, string skinId)
	{
		if (weaponRecord == null || skinId == null || this.Category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			if (weaponRecord == null || skinId == null)
			{
				this.SetEquipped(false);
			}
			return;
		}
		try
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(this.ItemId);
			if (skin != null && skin.IsForWeapon(weaponRecord.PrefabName))
			{
				this.SetEquipped(skinId == this.ItemId);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in HandleWeaponSkinsManager_EquippedSkinForWeapon: " + arg);
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00005F18 File Offset: 0x00004118
	private void HandleSkinsController_EquippedSkin(string newSkin, string oldSkin)
	{
		if (this.ItemId == "CustomSkinID" && this.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.SetEquipped(false);
			return;
		}
		if (this.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			this.SetEquipped(newSkin == this.ItemId);
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00005F6C File Offset: 0x0000416C
	private void StartUpdateInfo()
	{
		if (this.ItemId != null)
		{
			base.StartCoroutine(this.UpdateInfo());
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00005F88 File Offset: 0x00004188
	private void SetIcon()
	{
		string tag = this.ItemId;
		if (this.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			Egg egg = Singleton<EggsManager>.Instance.GetPlayerEggs().FirstOrDefault((Egg e) => e.Id.ToString() == this.ItemId);
			if (egg != null)
			{
				tag = egg.Data.Id;
			}
			else
			{
				Debug.LogErrorFormat("ArmoryCell: SetIcon, egg = null, ItemId = {0}", new object[]
				{
					this.ItemId
				});
			}
		}
		Texture itemIcon = ItemDb.GetItemIcon(tag, this.Category, null, true);
		if (itemIcon != null)
		{
			this.icon.mainTexture = itemIcon;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00006028 File Offset: 0x00004228
	private void OnEnable()
	{
		if (this.isEmpty)
		{
			return;
		}
		this.ReSubscribeToEquipEvents();
		this.UpdateAllAndStartUpdateCoroutine();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00006044 File Offset: 0x00004244
	private void OnDisable()
	{
		this.UnsubscribeEquipEvents();
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000604C File Offset: 0x0000424C
	private void SubscribeToEquipEvents()
	{
		WeaponManager.WeaponEquipped_AllCases += this.HandleWeaponManager_WeaponEquipped_AllCases;
		ShopNGUIController.EquippedWearInCategory += this.HandleEquippedWearInCategory;
		ShopNGUIController.UnequippedWearInCategory += this.Handle_UnequippedWearInCategory;
		SkinsController.EquippedSkin += this.HandleSkinsController_EquippedSkin;
		WeaponSkinsManager.EquippedSkinForWeapon += this.HandleWeaponSkinsManager_EquippedSkinForWeapon;
		ShopNGUIController.EquippedPet += this.ShopNGUIController_EquippedPet;
		ShopNGUIController.UnequippedPet += this.ShopNGUIController_UnequippedPet;
		ShopNGUIController.EquippedGadget += this.ShopNGUIController_EquippedGadget;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000060E4 File Offset: 0x000042E4
	private void UnsubscribeEquipEvents()
	{
		WeaponManager.WeaponEquipped_AllCases -= this.HandleWeaponManager_WeaponEquipped_AllCases;
		ShopNGUIController.EquippedWearInCategory -= this.HandleEquippedWearInCategory;
		ShopNGUIController.UnequippedWearInCategory -= this.Handle_UnequippedWearInCategory;
		SkinsController.EquippedSkin -= this.HandleSkinsController_EquippedSkin;
		WeaponSkinsManager.EquippedSkinForWeapon -= this.HandleWeaponSkinsManager_EquippedSkinForWeapon;
		ShopNGUIController.EquippedPet -= this.ShopNGUIController_EquippedPet;
		ShopNGUIController.UnequippedPet -= this.ShopNGUIController_UnequippedPet;
		ShopNGUIController.EquippedGadget -= this.ShopNGUIController_EquippedGadget;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000617C File Offset: 0x0000437C
	private void ShopNGUIController_UnequippedPet(string obj)
	{
		if (this.Category != ShopNGUIController.CategoryNames.PetsCategory)
		{
			return;
		}
		this.SetEquipped(false);
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00006198 File Offset: 0x00004398
	private void ShopNGUIController_EquippedGadget(string newGadget, string oldGadget, GadgetInfo.GadgetCategory gadgetCategory)
	{
		if (!ShopNGUIController.IsGadgetsCategory((ShopNGUIController.CategoryNames)gadgetCategory) || gadgetCategory != (GadgetInfo.GadgetCategory)this.Category)
		{
			return;
		}
		this.SetEquipped(GadgetsInfo.EquippedForCategory(gadgetCategory) == this.ItemId);
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000061D4 File Offset: 0x000043D4
	private void ShopNGUIController_EquippedPet(string newPet, string oldPet)
	{
		if (this.Category != ShopNGUIController.CategoryNames.PetsCategory)
		{
			return;
		}
		this.SetEquipped(this.ItemId == newPet);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x000061FC File Offset: 0x000043FC
	private void UpdateSkins()
	{
		if (this.Category != ShopNGUIController.CategoryNames.SkinsCategory)
		{
			return;
		}
		try
		{
			bool flag = !this.IsUnboughtSkinsEditor() && !SkinsController.IsLeagueSkinAvailableByLeague(this.ItemId) && !SkinsController.IsSkinBought(this.ItemId);
			this.lockSprite.SetActiveSafeSelf(flag);
			this.darkForeground.SetActiveSafeSelf(flag);
			if (flag)
			{
				SkinItem skinItem = SkinsController.sharedController.skinItemsDict[this.ItemId];
				this.leagueRating.ForEach(delegate(UILabel lab)
				{
					lab.text = RatingSystem.instance.RatingNeededForLeague(skinItem.currentLeague).ToString();
				});
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ArmoryCell.UpdateSkins: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000062D4 File Offset: 0x000044D4
	private void UpdateWeaponSkins()
	{
		if (this.Category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			return;
		}
		try
		{
			bool flag = !WeaponSkinsManager.IsAvailableByLeague(this.ItemId) && !WeaponSkinsManager.IsBoughtSkin(this.ItemId);
			this.lockSprite.SetActiveSafeSelf(flag);
			this.darkForeground.SetActiveSafeSelf(flag);
			if (flag)
			{
				WeaponSkin skinItem = WeaponSkinsManager.GetSkin(this.ItemId);
				this.leagueRating.ForEach(delegate(UILabel lab)
				{
					lab.text = RatingSystem.instance.RatingNeededForLeague(skinItem.ForLeague).ToString();
				});
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ArmoryCell.UpdateWeaponSkins: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000639C File Offset: 0x0000459C
	private void UpdateLeagueHats()
	{
		if (this.Category != ShopNGUIController.CategoryNames.HatsCategory)
		{
			return;
		}
		try
		{
			RatingSystem.RatingLeague leagueOfCurrentItem = (RatingSystem.RatingLeague)Wear.LeagueForWear(this.ItemId, ShopNGUIController.CategoryNames.HatsCategory);
			bool flag = leagueOfCurrentItem > RatingSystem.instance.currentLeague && Storager.getInt(this.ItemId, true) == 0;
			this.lockSprite.SetActiveSafeSelf(flag);
			this.darkForeground.SetActiveSafeSelf(flag);
			if (flag)
			{
				this.leagueRating.ForEach(delegate(UILabel lab)
				{
					lab.text = RatingSystem.instance.RatingNeededForLeague(leagueOfCurrentItem).ToString();
				});
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ArmoryCell.UpdateLeagueHats: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00006468 File Offset: 0x00004668
	private void UpdatePetsAndEggs()
	{
		if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			try
			{
				if (!PetsManager.IsEmptySlotId(this.ItemId))
				{
					this.petLevel.ForEach(delegate(UILabel lab)
					{
						lab.text = string.Format(LocalizationStore.Get("Key_2496"), Singleton<PetsManager>.Instance.GetInfo(this.lastBoughtTag).Up + 1);
					});
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in setting pet cell gui: {0}", new object[]
				{
					ex
				});
			}
		}
		else if (this.Category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			try
			{
				Egg ourEgg = Singleton<EggsManager>.Instance.GetPlayerEggs().FirstOrDefault((Egg egg) => egg.Id.ToString() == this.ItemId);
				this.eggCondition.ForEach(delegate(UILabel lab)
				{
					lab.text = EggHatchingConditionFormatter.TextForConditionOfEgg(ourEgg);
				});
				this.leagueRating.ForEach(delegate(UILabel lab)
				{
					lab.text = EggHatchingConditionFormatter.TextForConditionOfEgg(ourEgg);
				});
				this.petName.ForEach(delegate(UILabel lab)
				{
					lab.text = LocalizationStore.Get(EggData.LkeyForRarity(ourEgg.Data.Rare)).ToUpperInvariant();
				});
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in setting egg cell gui: {0}", new object[]
				{
					ex2
				});
			}
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x000065A8 File Offset: 0x000047A8
	private IEnumerator UpdateInfo()
	{
		for (;;)
		{
			try
			{
				bool thisItemContainedInUnlocked = PromoActionsManager.sharedManager != null && this.ItemId != null && this.Category != ShopNGUIController.CategoryNames.EggsCategory && PromoActionsManager.sharedManager.UnlockedItems.Union(PromoActionsManager.sharedManager.ItemsToRemoveFromUnlocked).Contains(this.ItemId);
				this.unlocked.SetActiveSafeSelf(thisItemContainedInUnlocked && this.lastBoughtTag.IsNullOrEmpty() && !this.IsBest);
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				Debug.LogErrorFormat("Exception in UpdateInfo, unlocked: {0}", new object[]
				{
					ex
				});
			}
			this.lockSprite.SetActiveSafeSelf(false);
			this.darkForeground.SetActiveSafeSelf(false);
			this.UpdatePriceAndDiscount();
			this.UpdateUpgrades();
			this.UpdateNewAndTopSeller();
			this.UpdatePetsAndEggs();
			this.UpdateSkins();
			this.UpdateWeaponSkins();
			this.UpdateLeagueHats();
			yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(0.5f));
			if (this == null || base.gameObject == null)
			{
				break;
			}
			try
			{
				this.lastBoughtUpdateCounter++;
				if (this.lastBoughtUpdateCounter % 10 == 0)
				{
					if (ShopNGUIController.IsWeaponCategory(this.Category) || ShopNGUIController.IsWearCategory(this.Category))
					{
						string newLastBoughtTag = WeaponManager.LastBoughtTag(this.ItemId, null);
						if (newLastBoughtTag != this.lastBoughtTag)
						{
							this.lastBoughtTag = newLastBoughtTag;
							this.UpdateFirstUnbought();
						}
					}
					else if (ShopNGUIController.IsGadgetsCategory(this.Category))
					{
						string newLastBought = GadgetsInfo.LastBoughtFor(this.ItemId);
						if (newLastBought != this.lastBoughtTag)
						{
							this.lastBoughtTag = newLastBought;
							this.UpdateFirstUnbought();
						}
					}
					else if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
					{
						string petId = this.lastBoughtTag;
						if (petId.IsNullOrEmpty())
						{
							petId = PetsManager.PetIdWithoutSuffixes(this.ItemId);
						}
						PlayerPet newLastBoughtPetInfo = Singleton<PetsManager>.Instance.GetPlayerPet(petId);
						string newLastBought2 = (newLastBoughtPetInfo == null) ? this.lastBoughtTag : newLastBoughtPetInfo.InfoId;
						if (newLastBought2 != this.lastBoughtTag)
						{
							this.lastBoughtTag = newLastBought2;
							this.UpdateFirstUnbought();
						}
					}
				}
			}
			catch (Exception ex3)
			{
				Exception e = ex3;
				Debug.LogErrorFormat("Exception in UpdateInfo: {0}", new object[]
				{
					e
				});
			}
		}
		yield break;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000065C4 File Offset: 0x000047C4
	private void HandleEquippedWearInCategory(string newEquipped, ShopNGUIController.CategoryNames unused, string equippedBefore)
	{
		if (!ShopNGUIController.IsWearCategory(this.Category))
		{
			return;
		}
		if (equippedBefore == this.ItemId)
		{
			this.SetEquipped(false);
		}
		if (newEquipped == this.ItemId)
		{
			this.SetEquipped(true);
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00006614 File Offset: 0x00004814
	private void Handle_UnequippedWearInCategory(ShopNGUIController.CategoryNames unused, string equippedBefore)
	{
		if (!ShopNGUIController.IsWearCategory(this.Category))
		{
			return;
		}
		if (equippedBefore == this.ItemId)
		{
			this.SetEquipped(false);
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000664C File Offset: 0x0000484C
	private void HandleWeaponManager_WeaponEquipped_AllCases(WeaponSounds ws)
	{
		if (this.Category == (ShopNGUIController.CategoryNames)(ws.categoryNabor - 1))
		{
			this.SetEquipped(ws.nameNoClone() == ItemDb.GetByTag(this.ItemId).PrefabName);
		}
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00006690 File Offset: 0x00004890
	public void UpdateNewAndTopSeller()
	{
		bool flag = ShopNGUIController.IsWeaponCategory(this.Category) || ShopNGUIController.IsWearCategory(this.Category) || ShopNGUIController.IsGadgetsCategory(this.Category) || this.Category == ShopNGUIController.CategoryNames.SkinsCategory;
		bool flag2 = !this.equipped.activeSelf && this.lastBoughtTag.IsNullOrEmpty() && (this.Category != ShopNGUIController.CategoryNames.SkinsCategory || !SkinsController.IsSkinBought(this.ItemId));
		bool flag3 = !this.unlocked.activeSelf && this.Category != ShopNGUIController.CategoryNames.EggsCategory && flag2 && PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.news.Contains(this.ItemId);
		bool flag4 = !this.unlocked.activeSelf && this.Category != ShopNGUIController.CategoryNames.EggsCategory && !flag3 && flag2 && PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.topSellers.Contains(this.ItemId);
		this.newLabel.SetActiveSafeSelf(flag3 && flag);
		this.topSeller.SetActiveSafeSelf(flag4 && flag);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000067EC File Offset: 0x000049EC
	private void UpdateBoughtIndicator()
	{
		bool flag = this.Category == ShopNGUIController.CategoryNames.SkinsCategory && !this.IsUnboughtSkinsEditor() && !SkinsController.IsSkinBought(this.ItemId) && SkinsController.leagueSkinsIds.Contains(this.ItemId);
		bool flag2 = this.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory && !WeaponSkinsManager.IsBoughtSkin(this.ItemId);
		bool flag3 = false;
		if (this.Category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			IEnumerable<string> source = Wear.UnboughtLeagueItemsByLeagues().SelectMany((KeyValuePair<RatingSystem.RatingLeague, List<string>> kvp) => kvp.Value);
			flag3 = source.Contains(this.ItemId);
		}
		this.boughtIndicator.alpha = ((this.Category != ShopNGUIController.CategoryNames.EggsCategory && (!this.lastBoughtTag.IsNullOrEmpty() || !this.HasPrice()) && (this.Category != ShopNGUIController.CategoryNames.PetsCategory || !PetsManager.IsEmptySlotId(this.ItemId)) && (!(this.ItemId == "super_socialman") || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) != 0) && this.Category != ShopNGUIController.CategoryNames.EggsCategory && !flag && !flag2 && !flag3) ? ((!this.toggle.value) ? 1f : 0f) : 0f);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00006960 File Offset: 0x00004B60
	public void UpdateUpgrades()
	{
		this.UpdateBoughtIndicator();
		if (ShopNGUIController.IsWeaponCategory(this.Category))
		{
			if ((this.lastBoughtTag.IsNullOrEmpty() && !this.toggle.value) || (WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(this.ItemId)))
			{
				this.upgradesContainer.SetActiveSafeSelf(false);
			}
			else
			{
				this.upgradesContainer.SetActiveSafeSelf(true);
				List<string> actualUpgrades = null;
				if (this.upgradesChain != null)
				{
					int num = this.upgradesChain.IndexOf(this.firstTagForOurTier);
					actualUpgrades = this.upgradesChain.GetRange(num, this.upgradesChain.Count - num);
				}
				else
				{
					actualUpgrades = new List<string>
					{
						this.ItemId
					};
				}
				this.upgrades.ForEach(delegate(UISprite sprite)
				{
					int num5 = this.upgrades.IndexOf(sprite);
					bool state = num5 < actualUpgrades.Count;
					sprite.gameObject.SetActiveSafeSelf(state);
				});
				for (int i = 0; i < actualUpgrades.Count; i++)
				{
					ItemRecord byTag = ItemDb.GetByTag(actualUpgrades[i]);
					bool haveUpgrade = byTag.StorageId == null || Storager.getInt(byTag.StorageId, true) > 0;
					this.upgrades[i].spriteName = ArmoryCell.SpriteNameForUpgradeState(haveUpgrade);
				}
			}
		}
		else if (ShopNGUIController.IsWearCategory(this.Category))
		{
			bool flag;
			int num3;
			int num2 = ShopNGUIController.CurrentNumberOfUpgradesForWear(this.ItemId, out flag, this.Category, out num3);
			bool flag2 = num2 > 0 || (num3 > 1 && this.toggle.value);
			this.upgradesContainer.SetActiveSafeSelf(flag2);
			if (flag2)
			{
				for (int j = 0; j < this.upgrades.Count; j++)
				{
					this.upgrades[j].gameObject.SetActiveSafeSelf(j < num3);
					bool haveUpgrade2 = j < num2;
					this.upgrades[j].spriteName = ArmoryCell.SpriteNameForUpgradeState(haveUpgrade2);
				}
			}
		}
		else if (ShopNGUIController.IsGadgetsCategory(this.Category))
		{
			if (this.lastBoughtTag.IsNullOrEmpty() && !this.toggle.value)
			{
				this.upgradesContainer.SetActiveSafeSelf(false);
			}
			else
			{
				this.upgradesContainer.SetActiveSafeSelf(true);
				List<string> actualUpgrades = null;
				if (this.upgradesChain != null)
				{
					int num4 = this.upgradesChain.IndexOf(this.firstTagForOurTier);
					actualUpgrades = this.upgradesChain.GetRange(num4, this.upgradesChain.Count - num4);
				}
				else
				{
					actualUpgrades = new List<string>
					{
						this.ItemId
					};
				}
				this.upgrades.ForEach(delegate(UISprite sprite)
				{
					int num5 = this.upgrades.IndexOf(sprite);
					bool state = num5 < actualUpgrades.Count;
					sprite.gameObject.SetActiveSafeSelf(state);
				});
				for (int k = 0; k < actualUpgrades.Count; k++)
				{
					bool haveUpgrade3 = GadgetsInfo.IsBought(actualUpgrades[k]);
					this.upgrades[k].spriteName = ArmoryCell.SpriteNameForUpgradeState(haveUpgrade3);
				}
			}
		}
		else if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00006CE0 File Offset: 0x00004EE0
	private void UpdateRented()
	{
		bool state = ShopNGUIController.IsWeaponCategory(this.Category) && WeaponManager.sharedManager != null && WeaponManager.sharedManager.IsAvailableTryGun(this.ItemId);
		this.rented.SetActiveSafeSelf(state);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00006D30 File Offset: 0x00004F30
	private static string SpriteNameForUpgradeState(bool haveUpgrade)
	{
		return (!haveUpgrade) ? "Lev_comp_gray_star" : "Lev_comp_gold_star";
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00006D48 File Offset: 0x00004F48
	private bool HasPrice()
	{
		bool result = false;
		if (ShopNGUIController.IsWeaponCategory(this.Category) || ShopNGUIController.IsGadgetsCategory(this.Category))
		{
			result = ((!ShopNGUIController.IsWeaponCategory(this.Category) || ItemDb.IsCanBuy(this.ItemId)) && this.firstUnboughtOrForOurTier != this.lastBoughtTag);
		}
		else if (ShopNGUIController.IsWearCategory(this.Category))
		{
			Dictionary<Wear.LeagueItemState, List<string>> dictionary = Wear.LeagueItems();
			bool flag = dictionary[Wear.LeagueItemState.Closed].Contains(this.ItemId) && !dictionary[Wear.LeagueItemState.Purchased].Contains(this.ItemId);
			result = (this.firstUnboughtOrForOurTier != this.lastBoughtTag && !flag);
		}
		else if (this.Category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			if (this.IsUnboughtSkinsEditor())
			{
				result = true;
			}
			else if (!SkinsController.IsCustomSkinId(this.ItemId))
			{
				try
				{
					bool flag3;
					bool flag2 = SkinsController.IsSkinBought(this.ItemId, out flag3);
					result = (this.ItemId != "super_socialman" && flag3 && !flag2 && SkinsController.IsLeagueSkinAvailableByLeague(this.ItemId));
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception in hasPrice for skin: " + arg);
				}
			}
		}
		else if (this.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			try
			{
				result = WeaponSkinsManager.AvailableForBuy(WeaponSkinsManager.GetSkin(this.ItemId));
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in determining has price for league weapon skin(ArmoryCell): skinId = {0}\n, {1}", new object[]
				{
					this.ItemId ?? "null",
					ex
				});
			}
		}
		else if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			try
			{
				result = (!PetsManager.IsEmptySlotId(this.ItemId) && Singleton<PetsManager>.Instance.GetNextUp(this.ItemId) != null);
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in getting hasPrice for pets: {0}", new object[]
				{
					ex2
				});
			}
		}
		return result;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00006FB0 File Offset: 0x000051B0
	private void UpdatePriceAndDiscount()
	{
		bool flag = this.HasPrice();
		if (flag)
		{
			ItemPrice itemPrice = ShopNGUIController.GetItemPrice((this.Category != ShopNGUIController.CategoryNames.SkinsCategory && this.Category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory) ? this.firstUnboughtOrForOurTier : this.ItemId, this.Category, false, true, true);
			this.priceLabel.text = itemPrice.Price.ToString();
			GameObject go = (!(itemPrice.Currency == "GemsCurrency")) ? this.coinSprite : this.gemSprite;
			go.SetActiveSafeSelf(true);
			GameObject go2 = (!(itemPrice.Currency == "Coins")) ? this.coinSprite : this.gemSprite;
			go2.SetActiveSafeSelf(false);
			bool state = this.Category != ShopNGUIController.CategoryNames.SkinsCategory && this.Category != ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory && !this.lastBoughtTag.IsNullOrEmpty();
			this.isPriceForUpgrade.SetActiveSafeSelf(state);
		}
		this.priceLabel.gameObject.SetActiveSafeSelf(flag);
		if ((this.Category == ShopNGUIController.CategoryNames.SkinsCategory && this.ItemId == "CustomSkinID" && !this.IsUnboughtSkinsEditor()) || (this.Category == ShopNGUIController.CategoryNames.EggsCategory || (this.Category == ShopNGUIController.CategoryNames.SkinsCategory && !SkinsController.IsLeagueSkinAvailableByLeague(this.ItemId))) || (this.Category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory && !WeaponSkinsManager.IsAvailableByLeague(this.ItemId)) || (this.Category == ShopNGUIController.CategoryNames.PetsCategory && PetsManager.IsEmptySlotId(this.ItemId)))
		{
			this.discountSprite.gameObject.SetActiveSafeSelf(false);
			return;
		}
		bool flag2;
		int num = ShopNGUIController.DiscountFor((this.Category != ShopNGUIController.CategoryNames.SkinsCategory) ? this.firstUnboughtOrForOurTier : this.ItemId, out flag2);
		bool flag3 = num > 0 && flag;
		this.discountSprite.gameObject.SetActiveSafeSelf(flag3);
		if (flag3)
		{
			this.discountLabel.text = "-" + num + "%";
			this.UpdateDiscountVisibility();
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000071E8 File Offset: 0x000053E8
	private bool IsUnboughtSkinsEditor()
	{
		return this.ItemId == "CustomSkinID" && Storager.getInt(Defs.SkinsMakerInProfileBought, true) == 0;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000721C File Offset: 0x0000541C
	private void SetEquipped(bool e)
	{
		this.equipped.SetActiveSafeSelf(e);
		this.UpdateNewAndTopSeller();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00007230 File Offset: 0x00005430
	private Color ColorForRarity(ItemDb.ItemRarity rarity)
	{
		switch (rarity)
		{
		case ItemDb.ItemRarity.Common:
			return new Color(0.8902f, 0.8902f, 0.8902f);
		case ItemDb.ItemRarity.Uncommon:
			return new Color(0.55294f, 1f, 0.01176f);
		case ItemDb.ItemRarity.Rare:
			return new Color(0.01176f, 0.80392f, 1f);
		case ItemDb.ItemRarity.Epic:
			return new Color(1f, 0.86667f, 0.01176f);
		case ItemDb.ItemRarity.Legendary:
			return new Color(1f, 0.36471f, 0.36471f);
		default:
			return new Color(0.77647f, 0.00784f, 0.80392f);
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000072DC File Offset: 0x000054DC
	private void OnClick()
	{
		if (this.toggle.value)
		{
			Action<ArmoryCell> clicked = ArmoryCell.Clicked;
			if (clicked != null)
			{
				clicked(this);
			}
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000730C File Offset: 0x0000550C
	private void UpdateFirstUnbought()
	{
		try
		{
			if (ShopNGUIController.IsWeaponCategory(this.Category))
			{
				this.firstUnboughtOrForOurTier = WeaponManager.FirstUnboughtOrForOurTier(this.ItemId);
			}
			else if (ShopNGUIController.IsWearCategory(this.Category))
			{
				this.firstUnboughtOrForOurTier = WeaponManager.FirstUnboughtTag(this.ItemId);
			}
			else if (ShopNGUIController.IsGadgetsCategory(this.Category))
			{
				this.firstUnboughtOrForOurTier = GadgetsInfo.FirstUnboughtOrForOurTier(this.ItemId);
			}
			else if (this.Category == ShopNGUIController.CategoryNames.PetsCategory)
			{
				if (!PetsManager.IsEmptySlotId(this.ItemId))
				{
					PetInfo firstUnboughtPet = Singleton<PetsManager>.Instance.GetFirstUnboughtPet(this.ItemId);
					this.firstUnboughtOrForOurTier = ((firstUnboughtPet == null) ? this.firstUnboughtOrForOurTier : firstUnboughtPet.Id);
				}
			}
			else
			{
				this.firstUnboughtOrForOurTier = this.ItemId;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ArmoryCell.UpdateFirstUnbought: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x04000078 RID: 120
	public GameObject unlocked;

	// Token: 0x04000079 RID: 121
	public UISprite bottomBorderIndicatorSprite;

	// Token: 0x0400007A RID: 122
	public UISprite topBorderIndicatorSprite;

	// Token: 0x0400007B RID: 123
	public GameObject darkForeground;

	// Token: 0x0400007C RID: 124
	public GameObject lockSprite;

	// Token: 0x0400007D RID: 125
	public GameObject championEggImage;

	// Token: 0x0400007E RID: 126
	public List<UILabel> leagueRating;

	// Token: 0x0400007F RID: 127
	public GameObject selectionIndicator;

	// Token: 0x04000080 RID: 128
	public List<UILabel> petName;

	// Token: 0x04000081 RID: 129
	public List<UILabel> petLevel;

	// Token: 0x04000082 RID: 130
	public List<UILabel> eggCondition;

	// Token: 0x04000083 RID: 131
	public UIToggle toggle;

	// Token: 0x04000084 RID: 132
	public GameObject newLabel;

	// Token: 0x04000085 RID: 133
	public GameObject topSeller;

	// Token: 0x04000086 RID: 134
	public MeshRenderer capeRenderer;

	// Token: 0x04000087 RID: 135
	public GameObject newSkinLabel;

	// Token: 0x04000088 RID: 136
	public GameObject rented;

	// Token: 0x04000089 RID: 137
	public GameObject isPriceForUpgrade;

	// Token: 0x0400008A RID: 138
	public GameObject equipped;

	// Token: 0x0400008B RID: 139
	public GameObject modelForSkin;

	// Token: 0x0400008C RID: 140
	public GameObject upgradesContainer;

	// Token: 0x0400008D RID: 141
	public List<UISprite> upgrades;

	// Token: 0x0400008E RID: 142
	public UILabel priceLabel;

	// Token: 0x0400008F RID: 143
	public GameObject gemSprite;

	// Token: 0x04000090 RID: 144
	public GameObject coinSprite;

	// Token: 0x04000091 RID: 145
	public UILabel discountLabel;

	// Token: 0x04000092 RID: 146
	public UISprite discountSprite;

	// Token: 0x04000093 RID: 147
	public UISprite coloredBorder;

	// Token: 0x04000094 RID: 148
	public UISprite boughtIndicator;

	// Token: 0x04000095 RID: 149
	public UITexture icon;

	// Token: 0x04000096 RID: 150
	public bool isEmpty;

	// Token: 0x04000097 RID: 151
	private string firstUnboughtOrForOurTier = string.Empty;

	// Token: 0x04000098 RID: 152
	private string lastBoughtTag = string.Empty;

	// Token: 0x04000099 RID: 153
	private List<string> upgradesChain;

	// Token: 0x0400009A RID: 154
	private string firstTagForOurTier = string.Empty;

	// Token: 0x0400009B RID: 155
	private int lastBoughtUpdateCounter;
}
