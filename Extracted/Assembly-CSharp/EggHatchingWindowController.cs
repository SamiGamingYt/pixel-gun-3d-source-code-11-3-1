using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Rilisoft;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class EggHatchingWindowController : GeneralBannerWindow
{
	// Token: 0x060004E3 RID: 1251 RVA: 0x00028350 File Offset: 0x00026550
	public void SetRenameMode()
	{
		this.m_windowMode = EggHatchingWindowController.WindowMode.Rename;
		this.Destroy3dModels();
		this.initialState.SetActiveSafeSelf(false);
		this.upgradePetState.SetActiveSafeSelf(false);
		this.petUpgradeRotationCollider.gameObject.SetActiveSafeSelf(false);
		this.newPetState.SetActiveSafeSelf(true);
		this.petAddedRotationCollider.gameObject.SetActiveSafeSelf(true);
		this.newPetState.GetComponent<UIPanel>().alpha = 1f;
		HatchingEndedCallback.HatchingEnded -= this.HatchingEndedCallback_HatchingEnded;
		this.getPetButton.SetActiveSafeSelf(false);
		this.confirmButton.SetActiveSafeSelf(true);
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060004E4 RID: 1252 RVA: 0x000283F0 File Offset: 0x000265F0
	internal EggHatchingWindowController.WindowMode CurrentWindowMode
	{
		get
		{
			return this.m_windowMode;
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x000283F8 File Offset: 0x000265F8
	public void TouchEgg()
	{
		if (!this.hatchButton.activeSelf)
		{
			return;
		}
		this.touchesToHatch--;
		if (this.touchesToHatch == 0)
		{
			this.tweenHatcWindow.ResetToBeginning();
			this.tweenHatcWindow.PlayForward();
			this.HandleHatchButton();
		}
		else if (this.touchesToHatch > 0)
		{
			this.egg.GetComponent<Animator>().SetTrigger("Touch");
			this.tweenHatcWindow.ResetToBeginning();
			this.tweenHatcWindow.PlayForward();
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00028488 File Offset: 0x00026688
	// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00028490 File Offset: 0x00026690
	public Egg EggForHatching
	{
		get
		{
			return this.m_eggForHatching;
		}
		set
		{
			this.m_eggForHatching = value;
			if (this._3dModelsParent != null && this.m_eggForHatching != null)
			{
				try
				{
					Player_move_c.SetTextureRecursivelyFrom(this._3dModelsParent.gameObject, Resources.Load<Texture>(this.m_eggForHatching.GetRelativeMeshTexturePath()), new GameObject[0]);
					this.egg.localPosition += this.EGG_HIDE_OFFSET;
					CoroutineRunner.Instance.StartCoroutine(this.ReturnEggToIsPosition());
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in setting egg's texture: {0}", new object[]
					{
						ex
					});
				}
			}
		}
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00028550 File Offset: 0x00026750
	public void SetPetId(string petId)
	{
		this.m_petId = petId;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0002855C File Offset: 0x0002675C
	public Transform ReplaceEggWithPet(string petId)
	{
		Transform 3dModelsParent = this._3dModelsParent;
		PetEngine petEngine = Resources.Load<PetEngine>(Singleton<PetsManager>.Instance.GetRelativePrefabPath(petId));
		Transform component = UnityEngine.Object.Instantiate<GameObject>(petEngine.Model).GetComponent<Transform>();
		component.parent = 3dModelsParent;
		component.localPosition = Singleton<PetsManager>.Instance.GetInfo(petId).PositionInBanners;
		component.localRotation = Quaternion.Euler(Singleton<PetsManager>.Instance.GetInfo(petId).RotationInBanners);
		component.localScale = Vector3.one;
		Tools.SetLayerRecursively(component.gameObject, 3dModelsParent.gameObject.layer);
		this.PlayPetAnimation(component.gameObject);
		this.petAddedRotationCollider.characterView = component;
		this.petAddedRotationCollider.SetDefaultRotationFromCharacterView();
		this.petUpgradeRotationCollider.characterView = component;
		this.petUpgradeRotationCollider.SetDefaultRotationFromCharacterView();
		ShopNGUIController.DisableLightProbesRecursively(component.gameObject);
		return component;
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00028634 File Offset: 0x00026834
	public void SetPetsNameToInput(string petsName)
	{
		this.petsNameInput.value = petsName;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00028644 File Offset: 0x00026844
	public virtual void HandleHatchButton()
	{
		this.hatchButton.SetActiveSafeSelf(false);
		Animator componentInChildren = this._3dModelsParent.GetComponentInChildren<Animator>(true);
		componentInChildren.SetTrigger("Open");
		float num = 1.22f;
		float p_delay = 2f;
		try
		{
			num = componentInChildren.runtimeAnimatorController.animationClips.FirstOrDefault((AnimationClip clip) => clip.name == "Open").length / 4f;
			p_delay = componentInChildren.runtimeAnimatorController.animationClips.FirstOrDefault((AnimationClip clip) => clip.name == "Open").length - num * 2f;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in getting eggOpenDuraition: {0}", new object[]
			{
				ex
			});
		}
		try
		{
			Renderer eggRenderer = componentInChildren.GetComponent<HatchingEggRefsHolder>().eggRenderer;
			eggRenderer.material.color = new Color(1f, 1f, 1f, 1f);
			TweenParms p_parms = new TweenParms().Prop("color", new PlugSetColor(new Color(1f, 1f, 1f, 0f)).Property("_Color")).Ease(EaseType.Linear).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(p_delay);
			HOTween.To(eggRenderer.material, num, p_parms);
		}
		catch (Exception ex2)
		{
			Debug.LogErrorFormat("Exception in starting fading out egg: {0}", new object[]
			{
				ex2
			});
		}
		string petId = Singleton<EggsManager>.Instance.Use(this.EggForHatching);
		AnalyticsStuff.LogHatching(petId, this.EggForHatching);
		PetUpdateInfo petUpdateInfo = Singleton<PetsManager>.Instance.AddOrUpdatePet(petId);
		string infoId = petUpdateInfo.PetNew.InfoId;
		try
		{
			this.ShouldEquipPet = (Singleton<PetsManager>.Instance.PlayerPets.Count == 1 && petUpdateInfo.PetAdded);
		}
		catch (Exception ex3)
		{
			Debug.LogErrorFormat("Exception in calculating shouldEquipPet: {0}", new object[]
			{
				ex3
			});
		}
		this.SetPetId(infoId);
		PetInfo info = petUpdateInfo.PetNew.Info;
		Transform transform = this.ReplaceEggWithPet(info.Id);
		transform.localScale = Vector3.zero;
		TweenParms p_parms2 = new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.Linear).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(p_delay).OnComplete(delegate()
		{
		});
		HOTween.To(transform, num / 2f, p_parms2);
		try
		{
			if (petUpdateInfo.PetAdded)
			{
				this.petAddedRotationCollider.gameObject.SetActiveSafeSelf(true);
				this.petUpgradeRotationCollider.gameObject.SetActiveSafeSelf(false);
				string petsNameToInput = LocalizationStore.Get(info.Lkey);
				this.SetPetsNameToInput(petsNameToInput);
			}
			else
			{
				this.petAddedRotationCollider.gameObject.SetActiveSafeSelf(false);
				this.petUpgradeRotationCollider.gameObject.SetActiveSafeSelf(true);
				bool isPetOfMaxUpgrade = this.IsPetOfMaxUpgrade;
				foreach (UILabel uilabel in this.petNameUpgrade)
				{
					uilabel.text = string.Format(LocalizationStore.Get("Key_2596"), LocalizationStore.Get(info.Lkey));
				}
				foreach (UILabel uilabel2 in this.rarity)
				{
					uilabel2.text = ItemDb.GetItemRarityLocalizeName(info.Rarity);
				}
				if (!isPetOfMaxUpgrade)
				{
					foreach (UILabel uilabel3 in this.currentGrade)
					{
						uilabel3.text = string.Format(LocalizationStore.Get("Key_2496"), info.Up + 1);
					}
					foreach (UILabel uilabel4 in this.nextGrade)
					{
						uilabel4.text = string.Format(LocalizationStore.Get("Key_2496"), info.Up + 2);
					}
				}
				else
				{
					this.currentGrade.ForEach(delegate(UILabel lab)
					{
						lab.gameObject.SetActiveSafeSelf(false);
					});
					this.nextGrade.ForEach(delegate(UILabel lab)
					{
						lab.gameObject.SetActiveSafeSelf(false);
					});
				}
				int num2 = petUpdateInfo.PetNew.Points;
				int num3 = this.PointsToNextUpOfPet();
				if (!isPetOfMaxUpgrade)
				{
					foreach (UILabel uilabel5 in this.points)
					{
						uilabel5.text = string.Format("{0} {1} / {2}", LocalizationStore.Get("Key_2793"), num2, num3);
					}
				}
				else
				{
					this.points.ForEach(delegate(UILabel lab)
					{
						lab.gameObject.SetActiveSafeSelf(false);
					});
				}
				if (!isPetOfMaxUpgrade)
				{
					float nextUpPointsSafe = EggHatchingWindowController.GetNextUpPointsSafe(num3);
					this.oldUpgradeIndicator.fillAmount = (float)Mathf.Min(petUpdateInfo.PetOld.Points, num3) / nextUpPointsSafe;
					this.newUpgradeIndicator.fillAmount = (float)Mathf.Min(petUpdateInfo.PetOld.Points, num3) / nextUpPointsSafe;
				}
				else
				{
					this.oldUpgradeIndicator.fillAmount = 1f;
					this.newUpgradeIndicator.fillAmount = 0f;
				}
				this.noUpgrade.SetActiveSafeSelf(num2 < num3 && !isPetOfMaxUpgrade);
				this.upgradeAvailable.SetActiveSafeSelf(num2 >= num3 && !isPetOfMaxUpgrade);
				this.specialPrize.SetActiveSafeSelf(isPetOfMaxUpgrade);
				if (isPetOfMaxUpgrade)
				{
					int count = this.CoinsForPetMaxUpggrade(this.m_petId);
					BankController.AddCoins(count, true, AnalyticsConstants.AccrualType.Earned);
					this.specialPrizeCoinCount.text = count.ToString();
				}
				this.maximumLevel.ForEach(delegate(UILabel lab)
				{
					lab.gameObject.SetActiveSafeSelf(isPetOfMaxUpgrade);
				});
				this.AnimateUpgradeIndicator();
			}
		}
		catch (Exception ex4)
		{
			Debug.LogErrorFormat("Exception in HandleHatchButton in hatching banner: {0}", new object[]
			{
				ex4
			});
		}
		TweenParms p_parms3 = new TweenParms().Prop("alpha", 1f).Ease(EaseType.Linear).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(p_delay).OnComplete(delegate()
		{
		});
		UIPanel component = ((!petUpdateInfo.PetAdded) ? this.upgradePetState : this.newPetState).GetComponent<UIPanel>();
		HOTween.To(component, 0.4f, p_parms3);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00028E6C File Offset: 0x0002706C
	public void SavePetsName()
	{
		try
		{
			if (this.m_petId.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("SavePetsName m_petId.IsNullOrEmpty()", new object[0]);
			}
			else
			{
				bool flag = this.newPetState.activeInHierarchy && this.newPetState.GetComponent<UIPanel>().alpha > 0.05f;
				if (this.m_windowMode == EggHatchingWindowController.WindowMode.Rename || flag)
				{
					Singleton<PetsManager>.Instance.SetPetName(this.m_petId, this.petsNameInput.value ?? string.Empty);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in setting pets name after hatching: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00028F40 File Offset: 0x00027140
	public override void HandleClose()
	{
		this.EquipPetIfNeeded();
		if (!this.isClosing)
		{
			this.isClosing = true;
			this.SavePetsName();
			this.EquipPetIfNeeded();
			TweenParms p_parms = new TweenParms().Prop("alpha", 0f).Ease(EaseType.EaseInCubic).UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(delegate()
			{
			});
			HOTween.To(this.darkenSprite, 0.1f, p_parms);
			TweenParms p_parms2 = new TweenParms().Prop("localScale", Vector3.one * 0.02f).Ease(EaseType.EaseInCubic).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(0.1f).OnComplete(delegate()
			{
				base.HandleClose();
			});
			HOTween.To(base.transform, 0.2f, p_parms2);
			if (this.CurrentWindowMode != EggHatchingWindowController.WindowMode.Rename)
			{
				ShopNGUIController.sharedShop.UpdatePetsCategoryIfNeeded();
			}
		}
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00029040 File Offset: 0x00027240
	public void HandleDArkBackgroundPressed()
	{
		this.OnHardwareBackPressed();
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00029048 File Offset: 0x00027248
	protected override void OnHardwareBackPressed()
	{
		if (this.initialState.activeInHierarchy)
		{
			if (this.hatchButton.activeInHierarchy)
			{
				this.HandleHatchButton();
			}
		}
		else
		{
			this.HandleClose();
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0002907C File Offset: 0x0002727C
	private int PointsToNextUpOfPet()
	{
		if (this.m_petId.IsNullOrEmpty())
		{
			return 999999;
		}
		PetInfo info = Singleton<PetsManager>.Instance.GetInfo(this.m_petId);
		return (info == null) ? 999999 : info.ToUpPoints;
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060004F1 RID: 1265 RVA: 0x000290C8 File Offset: 0x000272C8
	// (set) Token: 0x060004F2 RID: 1266 RVA: 0x000290D0 File Offset: 0x000272D0
	private string HatchedPetName { get; set; }

	// Token: 0x060004F3 RID: 1267 RVA: 0x000290DC File Offset: 0x000272DC
	private static float GetNextUpPointsSafe(int nextUpPoints)
	{
		return (float)((nextUpPoints == 0) ? 1 : nextUpPoints);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x000290EC File Offset: 0x000272EC
	private void Start()
	{
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		this.RegisterEscapeHandler();
		HatchingEndedCallback.HatchingEnded += this.HatchingEndedCallback_HatchingEnded;
		this.btnUpgrade.GetComponent<ButtonHandler>().Clicked += this.OnUpgradeButtonClicked;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0002913C File Offset: 0x0002733C
	private void HatchingEndedCallback_HatchingEnded()
	{
		this.m_moveToSecondState = 5;
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00029148 File Offset: 0x00027348
	private void OnUpgradeButtonClicked(object sender, EventArgs eventArgs)
	{
		if (ShopNGUIController.sharedShop == null || this.m_petId.IsNullOrEmpty())
		{
			return;
		}
		ItemPrice itemPrice = ShopNGUIController.GetItemPrice(this.m_petId, ShopNGUIController.CategoryNames.PetsCategory, false, true, false);
		if (itemPrice == null)
		{
			return;
		}
		ShopNGUIController.TryToBuy(ShopNGUIController.sharedShop.mainPanel, itemPrice, new Action(this.OnBuyPetSuccess), new Action(this.OnBuyPetFailure), null, delegate
		{
			ShopNGUIController.sharedShop.PlayPersAnimations();
		}, delegate
		{
			ShopNGUIController.SetBankCamerasEnabled();
		}, delegate
		{
			ShopNGUIController.sharedShop.ShowGridOrArmorCarousel();
			ShopNGUIController.sharedShop.SetOtherCamerasEnabled(false);
			if (this != null)
			{
				this.EnablePetCamera();
			}
		});
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00029200 File Offset: 0x00027400
	private void OnBuyPetSuccess()
	{
		if (this.m_petId.IsNullOrEmpty())
		{
			return;
		}
		ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.PetsCategory, this.m_petId, 1, false, 0, null, null, true, true, true);
		ShopNGUIController.sharedShop.UpdatePetsCategoryIfNeeded();
		string itemNameNonLocalized = ItemDb.GetItemNameNonLocalized(this.m_petId, this.m_petId, ShopNGUIController.CategoryNames.PetsCategory, null);
		string categoryParameterName = AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.PetsCategory) ?? ShopNGUIController.CategoryNames.PetsCategory.ToString();
		AnalyticsStuff.LogSales(itemNameNonLocalized, categoryParameterName, false);
		ItemPrice itemPrice = ShopNGUIController.GetItemPrice(this.m_petId, ShopNGUIController.CategoryNames.PetsCategory, false, true, false);
		AnalyticsFacade.InAppPurchase(itemNameNonLocalized, AnalyticsStuff.AnalyticsReadableCategoryNameFromOldCategoryName(categoryParameterName), 1, itemPrice.Price, itemPrice.Currency);
		this.btnUpgrade.GetComponent<Collider>().enabled = false;
		this.btnClose.GetComponent<Collider>().enabled = false;
		this.upgradeParticles.SetActive(true);
		this.tweenUpgradeWindow.ResetToBeginning();
		this.tweenUpgradeWindow.PlayForward();
		CoroutineRunner.DeferredAction(1.4f, delegate
		{
			PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(this.m_petId);
			if (playerPet != null)
			{
				ShopNGUIController.sharedShop.ChooseCategoryAndSuperCategory(ShopNGUIController.CategoryNames.PetsCategory, new ShopNGUIController.ShopItem(playerPet.InfoId, ShopNGUIController.CategoryNames.PetsCategory), false);
			}
			this.HandleClose();
		});
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00029308 File Offset: 0x00027508
	private void OnBuyPetFailure()
	{
		Debug.Log(">>>> OnBuyPetFailure");
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00029314 File Offset: 0x00027514
	private void Destroy3dModels()
	{
		this._3dModelsParent.DestroyChildren();
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00029324 File Offset: 0x00027524
	private void AnimateUpgradeIndicator()
	{
		if (this.IsPetOfMaxUpgrade)
		{
			return;
		}
		this.btnClose.SetActiveSafe(false);
		this.btnUpgrade.SetActive(false);
		PlayerPet pet = Singleton<PetsManager>.Instance.GetPlayerPet(this.m_petId);
		int num = this.PointsToNextUpOfPet();
		float num2 = (float)Mathf.Min(pet.Points, num) / EggHatchingWindowController.GetNextUpPointsSafe(num);
		if (this.newUpgradeIndicator.fillAmount < 1f)
		{
			TweenParms p_parms = new TweenParms().Prop("fillAmount", num2).Ease(EaseType.Linear).UpdateType(UpdateType.TimeScaleIndependentUpdate).Delay(2f).OnComplete(delegate()
			{
				this.SetUpgradeWindowButtonsState(pet);
			});
			HOTween.To(this.newUpgradeIndicator, 0.7f, p_parms);
		}
		else
		{
			this.SetUpgradeWindowButtonsState(pet);
		}
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00029410 File Offset: 0x00027610
	private void SetUpgradeWindowButtonsState(PlayerPet currentPet)
	{
		int num = this.PointsToNextUpOfPet();
		PetInfo nextUp = Singleton<PetsManager>.Instance.GetNextUp(currentPet.InfoId);
		if ((float)Mathf.Min(currentPet.Points, num) >= EggHatchingWindowController.GetNextUpPointsSafe(num) && nextUp != null && ExpController.OurTierForAnyPlace() >= nextUp.Tier)
		{
			this.btnClose.SetActiveSafe(true);
			this.btnUpgrade.SetActive(true);
			this.btnClose.transform.localPosition = new Vector3(this.upgradeBtnCloseOffset, this.btnClose.transform.localPosition.y, this.btnClose.transform.localPosition.z);
			ItemPrice itemPrice = ShopNGUIController.GetItemPrice(nextUp.Id, ShopNGUIController.CategoryNames.PetsCategory, false, true, false);
			this._upgradePriceText.Text = itemPrice.Price.ToString();
			bool flag = itemPrice.Currency == "Coins";
			this._upgradePriceGoldSprite.gameObject.SetActiveSafe(flag);
			this._upgradePriceGemSprite.gameObject.SetActiveSafe(!flag);
		}
		else
		{
			this.btnClose.SetActiveSafe(true);
			this.btnUpgrade.SetActive(false);
			this.btnClose.transform.localPosition = new Vector3(0f, this.btnClose.transform.localPosition.y, this.btnClose.transform.localPosition.z);
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00029598 File Offset: 0x00027798
	private void MoveToSecondState()
	{
		this.initialState.SetActiveSafeSelf(false);
		this.egg.parent = null;
		UnityEngine.Object.Destroy(this.egg.gameObject);
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000295D0 File Offset: 0x000277D0
	private void OnDestroy()
	{
		HatchingEndedCallback.HatchingEnded -= this.HatchingEndedCallback_HatchingEnded;
		this.UnregisterEscapeHandler();
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000295EC File Offset: 0x000277EC
	private void EnablePetCamera()
	{
		if (this.petCamera != null && !this.petCamera.enabled)
		{
			this.petCamera.enabled = true;
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0002961C File Offset: 0x0002781C
	private void Update()
	{
		if (this.m_moveToSecondState > 0)
		{
			if (this.m_moveToSecondState == 1)
			{
				this.MoveToSecondState();
			}
			this.m_moveToSecondState--;
		}
		this.EnablePetCamera();
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0002965C File Offset: 0x0002785C
	private void PlayPetAnimation(GameObject objectWithAnimation)
	{
		if (objectWithAnimation == null)
		{
			return;
		}
		try
		{
			if (!ShopNGUIController.IsModeWithNormalTimeScaleInShop())
			{
				Animation component = objectWithAnimation.GetComponent<Animation>();
				this.petProfileAnimationRunner.StopAllCoroutines();
				if (component.GetClip("Profile") == null)
				{
					Debug.LogErrorFormat("Error: pet {0} has no Profile animation clip (EggHatchingWindowController)", new object[]
					{
						objectWithAnimation.nameNoClone()
					});
				}
				else if (this.petProfileAnimationRunner.gameObject.activeInHierarchy)
				{
					this.petProfileAnimationRunner.StartPlay(component, "Profile", false, null);
				}
				else
				{
					Debug.LogErrorFormat("Coroutine couldn't be started because the the game object 'AnimationCoroutineRunner' is inactive! (Pet)", new object[0]);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in PlayPetAnimation: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000501 RID: 1281 RVA: 0x00029744 File Offset: 0x00027944
	protected bool IsPetOfMaxUpgrade
	{
		get
		{
			if (this.m_petId.IsNullOrEmpty())
			{
				return false;
			}
			try
			{
				return Singleton<PetsManager>.Instance.GetAllUpgrades(this.m_petId).Length == PetsInfo.info[this.m_petId].Up + 1;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in IsPetOfMaxUpgrade: {0}", new object[]
				{
					ex
				});
			}
			return false;
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x000297D4 File Offset: 0x000279D4
	private IEnumerator ReturnEggToIsPosition()
	{
		yield return null;
		this.egg.localPosition -= this.EGG_HIDE_OFFSET;
		yield break;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x000297F0 File Offset: 0x000279F0
	private int CoinsForPetMaxUpggrade(string petId)
	{
		try
		{
			int result;
			if (BalanceController.cashbackPets.TryGetValue(petId, out result))
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in CoinsForPetMaxUpggrade: {0}", new object[]
			{
				ex
			});
		}
		return 5;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00029854 File Offset: 0x00027A54
	private void EquipPetIfNeeded()
	{
		if (this.m_petId == null)
		{
			Debug.LogErrorFormat("EquipPetIfNeeded: m_petId == null", new object[0]);
			this.ShouldEquipPet = false;
			return;
		}
		if (this.ShouldEquipPet)
		{
			try
			{
				ShopNGUIController.sharedShop.EquipPetAndUpdate(this.m_petId);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in equipping pet in EggHatchingWindowController: {0}", new object[]
				{
					ex
				});
			}
		}
		this.ShouldEquipPet = false;
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000505 RID: 1285 RVA: 0x000298E4 File Offset: 0x00027AE4
	// (set) Token: 0x06000506 RID: 1286 RVA: 0x000298EC File Offset: 0x00027AEC
	private bool ShouldEquipPet
	{
		get
		{
			return this.m_shouldEquipPet;
		}
		set
		{
			this.m_shouldEquipPet = value;
		}
	}

	// Token: 0x04000554 RID: 1364
	public Camera petCamera;

	// Token: 0x04000555 RID: 1365
	public CharacterViewRotator petUpgradeRotationCollider;

	// Token: 0x04000556 RID: 1366
	public CharacterViewRotator petAddedRotationCollider;

	// Token: 0x04000557 RID: 1367
	public Transform _3dModelsParent;

	// Token: 0x04000558 RID: 1368
	public Transform egg;

	// Token: 0x04000559 RID: 1369
	public GameObject getPetButton;

	// Token: 0x0400055A RID: 1370
	public GameObject confirmButton;

	// Token: 0x0400055B RID: 1371
	public GameObject initialState;

	// Token: 0x0400055C RID: 1372
	public GameObject newPetState;

	// Token: 0x0400055D RID: 1373
	public GameObject upgradePetState;

	// Token: 0x0400055E RID: 1374
	public GameObject hatchButton;

	// Token: 0x0400055F RID: 1375
	public List<UILabel> currentGrade;

	// Token: 0x04000560 RID: 1376
	public List<UILabel> nextGrade;

	// Token: 0x04000561 RID: 1377
	public List<UILabel> points;

	// Token: 0x04000562 RID: 1378
	public List<UILabel> rarity;

	// Token: 0x04000563 RID: 1379
	public List<UILabel> petNameUpgrade;

	// Token: 0x04000564 RID: 1380
	public List<UILabel> maximumLevel;

	// Token: 0x04000565 RID: 1381
	public UISprite oldUpgradeIndicator;

	// Token: 0x04000566 RID: 1382
	public UISprite newUpgradeIndicator;

	// Token: 0x04000567 RID: 1383
	public UILabel specialPrizeCoinCount;

	// Token: 0x04000568 RID: 1384
	public GameObject noUpgrade;

	// Token: 0x04000569 RID: 1385
	public GameObject upgradeAvailable;

	// Token: 0x0400056A RID: 1386
	public GameObject specialPrize;

	// Token: 0x0400056B RID: 1387
	public UIInput petsNameInput;

	// Token: 0x0400056C RID: 1388
	public UISprite darkenSprite;

	// Token: 0x0400056D RID: 1389
	private bool isClosing;

	// Token: 0x0400056E RID: 1390
	public AnimationCoroutineRunner petProfileAnimationRunner;

	// Token: 0x0400056F RID: 1391
	[Header("panel upgrade settings")]
	public GameObject btnClose;

	// Token: 0x04000570 RID: 1392
	public GameObject btnUpgrade;

	// Token: 0x04000571 RID: 1393
	public float upgradeBtnCloseOffset = 175f;

	// Token: 0x04000572 RID: 1394
	public TextGroup _upgradePriceText;

	// Token: 0x04000573 RID: 1395
	public UISprite _upgradePriceGoldSprite;

	// Token: 0x04000574 RID: 1396
	public UISprite _upgradePriceGemSprite;

	// Token: 0x04000575 RID: 1397
	public GameObject upgradeParticles;

	// Token: 0x04000576 RID: 1398
	public UITweener tweenHatcWindow;

	// Token: 0x04000577 RID: 1399
	public UITweener tweenUpgradeWindow;

	// Token: 0x04000578 RID: 1400
	private int touchesToHatch = 3;

	// Token: 0x04000579 RID: 1401
	private bool m_shouldEquipPet;

	// Token: 0x0400057A RID: 1402
	private readonly Vector3 EGG_HIDE_OFFSET = new Vector3(-10000f, 0f, 0f);

	// Token: 0x0400057B RID: 1403
	private EggHatchingWindowController.WindowMode m_windowMode;

	// Token: 0x0400057C RID: 1404
	private int m_moveToSecondState;

	// Token: 0x0400057D RID: 1405
	private Egg m_eggForHatching;

	// Token: 0x0400057E RID: 1406
	private string m_petId = string.Empty;

	// Token: 0x020000A8 RID: 168
	internal enum WindowMode
	{
		// Token: 0x0400058B RID: 1419
		Hatching,
		// Token: 0x0400058C RID: 1420
		Rename
	}
}
