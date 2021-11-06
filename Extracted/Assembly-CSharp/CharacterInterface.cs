using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CharacterInterface : MonoBehaviour
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000251 RID: 593 RVA: 0x000149F8 File Offset: 0x00012BF8
	// (set) Token: 0x06000252 RID: 594 RVA: 0x00014A00 File Offset: 0x00012C00
	public string CurrentCapeId { get; private set; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000253 RID: 595 RVA: 0x00014A0C File Offset: 0x00012C0C
	// (set) Token: 0x06000254 RID: 596 RVA: 0x00014A14 File Offset: 0x00012C14
	public Texture CurrentCapeTexture { get; private set; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000255 RID: 597 RVA: 0x00014A20 File Offset: 0x00012C20
	// (set) Token: 0x06000256 RID: 598 RVA: 0x00014A28 File Offset: 0x00012C28
	public string CurrentBootsId { get; private set; }

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000257 RID: 599 RVA: 0x00014A34 File Offset: 0x00012C34
	// (set) Token: 0x06000258 RID: 600 RVA: 0x00014A3C File Offset: 0x00012C3C
	public string CurrentHatId { get; private set; }

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000259 RID: 601 RVA: 0x00014A48 File Offset: 0x00012C48
	// (set) Token: 0x0600025A RID: 602 RVA: 0x00014A50 File Offset: 0x00012C50
	public string CurrentMaskId { get; private set; }

	// Token: 0x0600025B RID: 603 RVA: 0x00014A5C File Offset: 0x00012C5C
	public void SetSimpleCharacter()
	{
		this.simpleCharacter.SetActive(true);
		this.skinCharacter.SetActive(false);
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00014A78 File Offset: 0x00012C78
	public void SetCharacterType(bool haveHands, bool staticAnim, bool forSkinsMaker)
	{
		if (!haveHands)
		{
			this.SetHandsVisible(false);
		}
		this.PlayAnimation((!staticAnim) ? "Idle" : "default");
		if (forSkinsMaker)
		{
			this.SetShopMaterial();
			this.ActivateColliders();
		}
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00014AC0 File Offset: 0x00012CC0
	private void SetHandsVisible(bool show)
	{
		this.leftHand.SetActive(show);
		this.rightHand.SetActive(show);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00014ADC File Offset: 0x00012CDC
	private void PlayAnimation(string anim)
	{
		this.animation.clip = this.animation.GetClip(anim);
		this.animation.Play();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00014B04 File Offset: 0x00012D04
	private void SetShopMaterial()
	{
		foreach (Renderer renderer in this.renderers)
		{
			renderer.material = this.shopMaterial;
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00014B3C File Offset: 0x00012D3C
	private void ActivateColliders()
	{
		foreach (BoxCollider boxCollider in this.colliders)
		{
			boxCollider.enabled = true;
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00014B70 File Offset: 0x00012D70
	private void Awake()
	{
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00014B80 File Offset: 0x00012D80
	private void OnEnable()
	{
		PetsManager.OnPetsUpdated += this.PetsSynchronizer_OnPetsUpdated;
		if (this.usePetFromStorager)
		{
			this.UpdatePet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
		else if (this.isDuelInstance && this.myPet == null && !string.IsNullOrEmpty(this.idPetForSetInCoroutine))
		{
			this.UpdatePet(this.idPetForSetInCoroutine);
		}
		if (this.myPet != null)
		{
			this.myPet.SetActive(true);
		}
		if (this.weapon == null || !this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().GetClip("Profile"))
		{
			return;
		}
		this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00014C70 File Offset: 0x00012E70
	private void PetsSynchronizer_OnPetsUpdated()
	{
		if (this.usePetFromStorager)
		{
			this.UpdatePet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00014C90 File Offset: 0x00012E90
	private void Start()
	{
		if (base.transform.root.gameObject.nameNoClone() == "ShopNGUI")
		{
			this.isInArmory = true;
			this.posFlying = this.pointFlyingPetArmory.position;
			this.posGroundPet = this.pointGroundPetArmory.position;
			this.rotFlying = this.pointFlyingPetArmory.rotation;
			this.rotGroundPet = this.pointGroundPetArmory.rotation;
			this.deltaPosFromGroundPet = this.posGroundPet - base.transform.position;
		}
		else if (this.isDuelInstance)
		{
			if (this.enemyInDuel)
			{
				this.posGroundPet = this.groundPointDuelRight.position;
				this.rotGroundPet = this.groundPointDuelRight.rotation;
				this.posFlying = this.flyPointDuelRight.position;
				this.rotFlying = this.flyPointDuelRight.rotation;
			}
			else
			{
				this.posGroundPet = this.groundPointDuelLeft.position;
				this.rotGroundPet = this.groundPointDuelLeft.rotation;
				this.posFlying = this.flyPointDuelLeft.position;
				this.rotFlying = this.flyPointDuelLeft.rotation;
			}
		}
		else
		{
			this.isInArmory = false;
			if (base.transform.root.gameObject.nameNoClone() == "ProfileGui")
			{
				this.posGroundPet = this.pointGroundPetProfile.position;
				this.rotGroundPet = this.pointGroundPetProfile.rotation;
				this.posFlying = this.pointFlyingPetProfile.position;
				this.rotFlying = this.pointFlyingPetProfile.rotation;
			}
			else
			{
				this.posGroundPet = this.pointGroundPet.position;
				this.rotGroundPet = this.pointGroundPet.rotation;
				this.posFlying = this.pointFlyingPet.position;
				this.rotFlying = this.pointFlyingPet.rotation;
			}
			this.rotFlying = this.pointFlyingPet.rotation;
			this.rotGroundPet = this.pointGroundPet.rotation;
		}
		if (this.usePetFromStorager)
		{
			ShopNGUIController.EquippedPet += this.EquippedPet;
			ShopNGUIController.UnequippedPet += this.UnequipPet;
			this.UpdatePet(Singleton<PetsManager>.Instance.GetEqipedPetId());
		}
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00014EEC File Offset: 0x000130EC
	private void Update()
	{
		if (this.isInArmory && this.myPet != null && this.myPet.GetComponent<GroundPetEngine>() != null)
		{
			this.myPet.transform.position = base.transform.position + this.deltaPosFromGroundPet;
		}
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00014F54 File Offset: 0x00013154
	private void OnDisable()
	{
		PetsManager.OnPetsUpdated -= this.PetsSynchronizer_OnPetsUpdated;
		if (this.myPet != null)
		{
			this.myPet.SetActive(false);
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00014F90 File Offset: 0x00013190
	private void OnDestroy()
	{
		ShopNGUIController.EquippedPet -= this.EquippedPet;
		ShopNGUIController.UnequippedPet -= this.UnequipPet;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00014FC0 File Offset: 0x000131C0
	public void UpdateHat(string hat, bool isInvisible = false)
	{
		if (hat.IsNullOrEmpty())
		{
			hat = Defs.HatNoneEqupped;
		}
		this.CurrentHatId = hat;
		this.RemoveHat();
		if (hat.Equals(Defs.HatNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Hats/" + hat) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("hatPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		if (!this.useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		Transform transform = gameObject2.transform;
		gameObject2.transform.parent = this.hatPoint.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
		if (base.transform.parent != null)
		{
			Player_move_c.SetLayerRecursively(gameObject2, base.transform.parent.gameObject.layer);
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x000150E4 File Offset: 0x000132E4
	public void RemoveHat()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < this.hatPoint.childCount; i++)
		{
			list.Add(this.hatPoint.GetChild(i));
		}
		foreach (Transform transform in list)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00015184 File Offset: 0x00013384
	public void UpdateCape(string cape, Texture capeTex = null, bool isInvisible = false)
	{
		if (cape.IsNullOrEmpty())
		{
			cape = Defs.CapeNoneEqupped;
		}
		this.CurrentCapeId = cape;
		this.CurrentCapeTexture = capeTex;
		this.RemoveCape();
		if (cape.Equals(Defs.CapeNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load(ResPath.Combine(Defs.CapesDir, cape)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("capePrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		if (!this.useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		gameObject2.transform.parent = this.capePoint.transform;
		gameObject2.transform.localPosition = new Vector3(0f, -0.8f, 0f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		if (capeTex != null)
		{
			gameObject2.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
			Player_move_c.SetTextureRecursivelyFrom(gameObject2, capeTex, new GameObject[0]);
		}
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
	}

	// Token: 0x0600026B RID: 619 RVA: 0x000152BC File Offset: 0x000134BC
	public void RemoveCape()
	{
		for (int i = 0; i < this.capePoint.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.capePoint.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00015308 File Offset: 0x00013508
	public void UpdateMask(string mask, bool isInvisible = false)
	{
		if (mask.IsNullOrEmpty())
		{
			mask = "MaskNoneEquipped";
		}
		this.CurrentMaskId = mask;
		this.RemoveMask();
		if (mask.Equals("MaskNoneEquipped"))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Masks/" + mask) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("maskPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		if (!this.useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		gameObject2.transform.parent = this.maskPoint.transform;
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00015414 File Offset: 0x00013614
	public void RemoveMask()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < this.maskPoint.childCount; i++)
		{
			list.Add(this.maskPoint.GetChild(i));
		}
		foreach (Transform transform in list)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x000154B4 File Offset: 0x000136B4
	public void UpdateBoots(string bs, bool isInvisible = false)
	{
		if (bs.IsNullOrEmpty())
		{
			bs = Defs.BootsNoneEqupped;
		}
		this.CurrentBootsId = bs;
		this.RemoveBoots();
		if (bs.Equals(Defs.BootsNoneEqupped))
		{
			return;
		}
		GameObject gameObject = Resources.Load("Boots/BootPrefab") as GameObject;
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.SetParent(this.leftBootPoint.transform, false);
			gameObject3.transform.SetParent(this.rightBootPoint.transform, false);
			gameObject2.transform.localScale = new Vector3(-1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
			Player_move_c.SetLayerRecursively(gameObject3, base.gameObject.layer);
			gameObject2.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[bs]);
			gameObject3.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[bs]);
			if (isInvisible)
			{
				ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
				ShopNGUIController.SetRenderersVisibleFromPoint(gameObject3.transform, false);
			}
		}
	}

	// Token: 0x0600026F RID: 623 RVA: 0x000155D4 File Offset: 0x000137D4
	public void RemoveBoots()
	{
		foreach (object obj in this.leftBootPoint.transform)
		{
			Transform transform = (Transform)obj;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		foreach (object obj2 in this.rightBootPoint.transform)
		{
			Transform transform2 = (Transform)obj2;
			UnityEngine.Object.Destroy(transform2.gameObject);
		}
	}

	// Token: 0x06000270 RID: 624 RVA: 0x000156BC File Offset: 0x000138BC
	public void UpdateArmor(string armor, bool isInvisible)
	{
		this.UpdateArmor(armor, this.pointArmLeft, this.pointArmRight, isInvisible);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x000156D4 File Offset: 0x000138D4
	public void UpdateArmor(string armor, GameObject _weapon, bool isInvisible)
	{
		WeaponSounds component = _weapon.GetComponent<WeaponSounds>();
		this.UpdateArmor(armor, component.LeftArmorHand, component.RightArmorHand, isInvisible);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x000156FC File Offset: 0x000138FC
	public void UpdateArmor(string armor)
	{
		this.UpdateArmor(armor, this.pointArmLeft, this.pointArmRight, false);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00015714 File Offset: 0x00013914
	public void UpdateArmor(string armor, GameObject _weapon)
	{
		WeaponSounds component = _weapon.GetComponent<WeaponSounds>();
		this.UpdateArmor(armor, component.LeftArmorHand, component.RightArmorHand, false);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0001573C File Offset: 0x0001393C
	public void UpdateArmor(string armor, Transform leftArmorHand, Transform rightArmorHand, bool isInvisible = false)
	{
		this.RemoveArmor();
		if (armor.Equals(Defs.ArmorNewNoneEqupped))
		{
			return;
		}
		string @string = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			armor = @string;
		}
		GameObject gameObject = Resources.Load("Armor/" + armor) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("armorPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		if (!this.useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(gameObject2);
		}
		ArmorRefs component = gameObject2.transform.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null)
		{
			if (component.leftBone != null && leftArmorHand != null)
			{
				component.leftBone.parent = leftArmorHand;
				component.leftBone.localPosition = Vector3.zero;
				component.leftBone.localRotation = Quaternion.identity;
				component.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component.rightBone != null && rightArmorHand != null)
			{
				component.rightBone.parent = rightArmorHand;
				component.rightBone.localPosition = Vector3.zero;
				component.rightBone.localRotation = Quaternion.identity;
				component.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
			gameObject2.transform.parent = this.armorPoint.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		}
		if (isInvisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(gameObject2.transform, false);
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00015978 File Offset: 0x00013B78
	public void RemoveArmor()
	{
		if (this.armorPoint.childCount > 0)
		{
			Transform child = this.armorPoint.GetChild(0);
			ArmorRefs component = child.GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					component.leftBone.parent = child.GetChild(0);
				}
				if (component.rightBone != null)
				{
					component.rightBone.parent = child.GetChild(0);
				}
				child.parent = null;
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00015A14 File Offset: 0x00013C14
	public void SetSkin(Texture skin, WeaponSounds weapon = null, GadgetArmoryItem gadget = null)
	{
		if (skin == null)
		{
			return;
		}
		skin.filterMode = FilterMode.Point;
		this.skinForPers = skin;
		GameObject[] collection = new GameObject[]
		{
			this.capePoint.gameObject,
			this.hatPoint.gameObject,
			this.leftBootPoint.gameObject,
			this.rightBootPoint.gameObject,
			this.armorPoint.gameObject,
			this.maskPoint.gameObject
		};
		List<GameObject> list = new List<GameObject>(collection);
		if (weapon != null)
		{
			if (weapon.LeftArmorHand != null)
			{
				list.Add(weapon.LeftArmorHand.gameObject);
			}
			if (weapon.RightArmorHand != null)
			{
				list.Add(weapon.RightArmorHand.gameObject);
			}
			if (weapon.grenatePoint != null)
			{
				list.Add(weapon.grenatePoint.gameObject);
			}
			if (weapon.bonusPrefab != null)
			{
				list.Add(weapon.bonusPrefab);
			}
			List<GameObject> listWeaponAnimEffects = weapon.GetListWeaponAnimEffects();
			if (listWeaponAnimEffects != null)
			{
				list.AddRange(listWeaponAnimEffects);
			}
		}
		if (gadget != null)
		{
			if (gadget.gadgetPoint != null)
			{
				list.Add(gadget.gadgetPoint);
			}
			if (gadget.noFillPersSkinObjects != null)
			{
				list.AddRange(gadget.noFillPersSkinObjects);
			}
			if (gadget.LeftArmorHand != null)
			{
				list.Add(gadget.LeftArmorHand.gameObject);
			}
			if (gadget.RightArmorHand != null)
			{
				list.Add(gadget.RightArmorHand.gameObject);
			}
		}
		Player_move_c.SetTextureRecursivelyFrom(base.gameObject, skin, list.ToArray());
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00015BD8 File Offset: 0x00013DD8
	public void SetWeapon(string weaponName)
	{
		this.SetWeapon(weaponName, weaponName, string.Empty);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00015BE8 File Offset: 0x00013DE8
	public void SetWeapon(string weaponName, string alternativeWeapons)
	{
		this.SetWeapon(weaponName, alternativeWeapons, string.Empty);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00015BF8 File Offset: 0x00013DF8
	public void SetWeapon(string weaponName, string alternativeWeapons, string skinId)
	{
		if (this.armorPoint.childCount > 0)
		{
			ArmorRefs component = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
			if (component != null)
			{
				if (component.leftBone != null)
				{
					component.leftBone.parent = this.armorPoint.GetChild(0).GetChild(0);
				}
				if (component.rightBone != null)
				{
					component.rightBone.parent = this.armorPoint.GetChild(0).GetChild(0);
				}
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (object obj in this.gunPoint)
		{
			Transform item = (Transform)obj;
			list.Add(item);
		}
		foreach (Transform transform in list)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (weaponName != null && alternativeWeapons != null && WeaponManager.Removed150615_PrefabNames.Contains(weaponName))
		{
			weaponName = alternativeWeapons;
		}
		GameObject gameObject = Resources.Load("Weapons/" + weaponName) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("pref == null");
			return;
		}
		Debug.Log("ProfileAnims/" + gameObject.name + "_Profile");
		AnimationClip animationClip = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.parent = this.gunPoint.transform;
		this.weapon = gameObject2;
		this.weapon.transform.localPosition = Vector3.zero;
		this.weapon.transform.localRotation = Quaternion.identity;
		if (animationClip != null)
		{
			this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().AddClip(animationClip, "Profile");
			this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
		}
		this.gun = gameObject2.GetComponent<WeaponSounds>().bonusPrefab;
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject3 in array)
		{
			if (gameObject3.name.Equals("GunFlash"))
			{
				gameObject3.SetActive(false);
			}
		}
		this.SetSkin(this.skinForPers, this.weapon.GetComponent<WeaponSounds>(), null);
		if (!skinId.IsNullOrEmpty() && gameObject2 != null)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin != null)
			{
				skin.SetTo(gameObject2);
			}
		}
		if (this.armorPoint.childCount == 0)
		{
			return;
		}
		WeaponSounds component2 = gameObject2.GetComponent<WeaponSounds>();
		ArmorRefs component3 = this.armorPoint.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
		if (component3 != null)
		{
			if (component3.leftBone != null && component2.LeftArmorHand != null)
			{
				component3.leftBone.parent = component2.LeftArmorHand;
				component3.leftBone.localPosition = Vector3.zero;
				component3.leftBone.localRotation = Quaternion.identity;
				component3.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component3.rightBone != null && component2.RightArmorHand != null)
			{
				component3.rightBone.parent = component2.RightArmorHand;
				component3.rightBone.localPosition = Vector3.zero;
				component3.rightBone.localRotation = Quaternion.identity;
				component3.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600027A RID: 634 RVA: 0x00016060 File Offset: 0x00014260
	// (set) Token: 0x0600027B RID: 635 RVA: 0x00016068 File Offset: 0x00014268
	public GameObject myPet { get; private set; }

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600027C RID: 636 RVA: 0x00016074 File Offset: 0x00014274
	public PetEngine myPetEngine
	{
		get
		{
			return (!(this.myPet != null)) ? null : this.myPet.GetComponent<PetEngine>();
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x000160A4 File Offset: 0x000142A4
	public void EquippedPet(string newPet, string oldPet)
	{
		this.UpdatePet(newPet);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x000160B0 File Offset: 0x000142B0
	public void UnequipPet(string oldPet)
	{
		this.UpdatePet(string.Empty);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x000160C0 File Offset: 0x000142C0
	public void UpdatePet(string _petId = "")
	{
		string text = (!string.IsNullOrEmpty(_petId) || !this.usePetFromStorager) ? _petId : Singleton<PetsManager>.Instance.GetEqipedPetId();
		this.idPetForSetInCoroutine = text;
		if (!base.gameObject.activeInHierarchy)
		{
			this.myPet.Destroy(false);
			this.myPet = null;
			return;
		}
		if (_petId.IsNullOrEmpty() && this.myPet != null)
		{
			this.myPet.Destroy(false);
			this.myPet = null;
			return;
		}
		if (string.IsNullOrEmpty(text) && this.myPet != null)
		{
			this.myPet.Destroy(false);
			this.myPet = null;
			return;
		}
		if (text.IsNullOrEmpty())
		{
			return;
		}
		base.StopCoroutine("UpdatePetCoroutine");
		base.StartCoroutine("UpdatePetCoroutine");
	}

	// Token: 0x06000280 RID: 640 RVA: 0x000161A4 File Offset: 0x000143A4
	private IEnumerator UpdatePetCoroutine()
	{
		string currentPet = this.idPetForSetInCoroutine;
		if (this.myPet == null || this.myPet.nameNoClone() != Singleton<PetsManager>.Instance.GetFirstUpgrade(currentPet).Id)
		{
			string pathPet = Singleton<PetsManager>.Instance.GetRelativePrefabPath(currentPet);
			GameObject petPref = null;
			ResourceRequest request = Resources.LoadAsync<GameObject>(pathPet);
			while (!request.isDone)
			{
				yield return null;
			}
			petPref = (GameObject)request.asset;
			if (petPref == null)
			{
				Debug.LogErrorFormat("[PETS] prefab for pet '{0}' not found", new object[]
				{
					currentPet
				});
				yield break;
			}
			if (this.myPet != null)
			{
				this.myPet.Destroy(false);
				this.myPet = null;
			}
			this.myPet = UnityEngine.Object.Instantiate<GameObject>(petPref);
			Behaviour[] components = this.myPet.GetComponents<Behaviour>();
			foreach (Behaviour comp in components)
			{
				if (comp.GetType() != typeof(Transform))
				{
					comp.enabled = false;
				}
			}
			if (this.myPet.GetComponent<PetClickHandler>() == null)
			{
				this.myPet.AddComponent<PetClickHandler>();
			}
			else
			{
				this.myPet.GetComponent<PetClickHandler>().enabled = true;
			}
			if (base.gameObject.Ancestors().Any((GameObject d) => d.name == "MainMenu_Pers_MainMenu"))
			{
				PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(currentPet);
				if (playerPet != null)
				{
					PetIndicationController c = this.myPet.GetComponent<PetIndicationController>();
					c.enabled = true;
					c.isUpdateNameFromInfo = true;
					c.IconObject.SetActive(false);
					this.myPet.GetComponent<PetEngine>().IsMine = true;
					c.LabelObj.SetActive(true);
					c.TextMesh.text = playerPet.PetName;
					c.CreateFrameLabel();
				}
			}
			else
			{
				PetIndicationController c2 = this.myPet.GetComponent<PetIndicationController>();
				c2.IconObject.SetActive(false);
				c2.LabelObj.SetActive(false);
			}
		}
		this.myPet.GetComponent<PetEngine>().SetInfo(currentPet);
		this.myPet.transform.position = ((!(this.myPet.GetComponent<FlyingPetEngine>() != null)) ? this.posGroundPet : this.posFlying);
		this.myPet.transform.rotation = ((!(this.myPet.GetComponent<FlyingPetEngine>() != null)) ? this.rotGroundPet : this.rotFlying);
		Tools.SetLayerRecursively(this.myPet, base.gameObject.layer);
		if (!this.useLightprobes)
		{
			ShopNGUIController.DisableLightProbesRecursively(this.myPet);
		}
		this.myPet.SetActive(base.enabled);
		yield break;
	}

	// Token: 0x0400028A RID: 650
	public Transform armorPoint;

	// Token: 0x0400028B RID: 651
	public Transform leftBootPoint;

	// Token: 0x0400028C RID: 652
	public Transform rightBootPoint;

	// Token: 0x0400028D RID: 653
	public Transform capePoint;

	// Token: 0x0400028E RID: 654
	public Transform hatPoint;

	// Token: 0x0400028F RID: 655
	public Transform maskPoint;

	// Token: 0x04000290 RID: 656
	public Transform gunPoint;

	// Token: 0x04000291 RID: 657
	public Transform pointArmRight;

	// Token: 0x04000292 RID: 658
	public Transform pointArmLeft;

	// Token: 0x04000293 RID: 659
	public Transform pointFlyingPet;

	// Token: 0x04000294 RID: 660
	public Transform pointGroundPet;

	// Token: 0x04000295 RID: 661
	public Transform pointFlyingPetArmory;

	// Token: 0x04000296 RID: 662
	public Transform pointGroundPetArmory;

	// Token: 0x04000297 RID: 663
	public Transform pointFlyingPetProfile;

	// Token: 0x04000298 RID: 664
	public Transform pointGroundPetProfile;

	// Token: 0x04000299 RID: 665
	public Transform flyPointDuelLeft;

	// Token: 0x0400029A RID: 666
	public Transform groundPointDuelLeft;

	// Token: 0x0400029B RID: 667
	public Transform flyPointDuelRight;

	// Token: 0x0400029C RID: 668
	public Transform groundPointDuelRight;

	// Token: 0x0400029D RID: 669
	private Vector3 posGroundPet;

	// Token: 0x0400029E RID: 670
	private Vector3 posFlying;

	// Token: 0x0400029F RID: 671
	private Quaternion rotGroundPet;

	// Token: 0x040002A0 RID: 672
	private Quaternion rotFlying;

	// Token: 0x040002A1 RID: 673
	public Animation animation;

	// Token: 0x040002A2 RID: 674
	public GameObject leftHand;

	// Token: 0x040002A3 RID: 675
	public GameObject rightHand;

	// Token: 0x040002A4 RID: 676
	public GameObject simpleCharacter;

	// Token: 0x040002A5 RID: 677
	public GameObject skinCharacter;

	// Token: 0x040002A6 RID: 678
	public Material shopMaterial;

	// Token: 0x040002A7 RID: 679
	public Renderer[] renderers;

	// Token: 0x040002A8 RID: 680
	public BoxCollider[] colliders;

	// Token: 0x040002A9 RID: 681
	public float scaleShop = 150f;

	// Token: 0x040002AA RID: 682
	public Vector3 positionShop;

	// Token: 0x040002AB RID: 683
	public Vector3 rotationShop;

	// Token: 0x040002AC RID: 684
	public int shopTier = 10;

	// Token: 0x040002AD RID: 685
	public GameObject weapon;

	// Token: 0x040002AE RID: 686
	public GameObject gun;

	// Token: 0x040002AF RID: 687
	public Texture skinForPers;

	// Token: 0x040002B0 RID: 688
	public bool useLightprobes;

	// Token: 0x040002B1 RID: 689
	public bool usePetFromStorager = true;

	// Token: 0x040002B2 RID: 690
	public bool enemyInDuel;

	// Token: 0x040002B3 RID: 691
	public bool isDuelInstance;

	// Token: 0x040002B4 RID: 692
	private bool isInArmory;

	// Token: 0x040002B5 RID: 693
	private Vector3 deltaPosFromGroundPet = Vector3.zero;

	// Token: 0x040002B6 RID: 694
	private string idPetForSetInCoroutine;
}
