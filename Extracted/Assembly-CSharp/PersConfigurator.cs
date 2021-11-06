using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020003DE RID: 990
public sealed class PersConfigurator : MonoBehaviour
{
	// Token: 0x060023AE RID: 9134 RVA: 0x000B183C File Offset: 0x000AFA3C
	private void Awake()
	{
		PersConfigurator.currentConfigurator = this;
		GameObject original = Resources.Load("Character_model") as GameObject;
		this.characterInterface = UnityEngine.Object.Instantiate<GameObject>(original).GetComponent<CharacterInterface>();
		this.characterInterface.transform.SetParent(base.transform, false);
		this.characterInterface.SetCharacterType(false, false, false);
		ShopNGUIController.DisableLightProbesRecursively(this.characterInterface.gameObject);
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000B18A8 File Offset: 0x000AFAA8
	private IEnumerator Start()
	{
		this.ResetWeapon();
		this.SetCurrentSkin();
		ShopNGUIController.sharedShop.onEquipSkinAction = delegate(string id)
		{
			this.SetCurrentSkin();
		};
		yield return new WaitForEndOfFrame();
		this.UpdateWear();
		ShopNGUIController.sharedShop.wearEquipAction = delegate(ShopNGUIController.CategoryNames c, string _1, string _2)
		{
			this.UpdateWear();
		};
		ShopNGUIController.sharedShop.wearUnequipAction = delegate(ShopNGUIController.CategoryNames c, string _)
		{
			this.UpdateWear();
		};
		ShopNGUIController.ShowArmorChanged += this.UpdateWear;
		ShopNGUIController.ShowWearChanged += this.UpdateWear;
		while (NickLabelStack.sharedStack == null)
		{
			yield return null;
		}
		NickLabelController.currentCamera = Camera.main;
		yield break;
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000B18C4 File Offset: 0x000AFAC4
	public void ResetWeapon()
	{
		if (this.weapon != null)
		{
			UnityEngine.Object.Destroy(this.weapon);
		}
		this.weapon = null;
		WeaponManager sharedManager = WeaponManager.sharedManager;
		GameObject gameObject = null;
		List<Weapon> list = new List<Weapon>();
		foreach (object obj in sharedManager.allAvailablePlayerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag))
			{
				list.Add(weapon);
			}
		}
		if (list.Count == 0)
		{
			foreach (object obj2 in sharedManager.allAvailablePlayerWeapons)
			{
				Weapon weapon2 = (Weapon)obj2;
				if (weapon2.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.PistolWN)
				{
					gameObject = weapon2.weaponPrefab;
					break;
				}
			}
		}
		else
		{
			gameObject = list[UnityEngine.Random.Range(0, list.Count)].weaponPrefab;
		}
		if (gameObject == null)
		{
			Debug.LogWarning("pref == null");
		}
		else
		{
			Debug.Log("ProfileAnims/" + gameObject.name + "_Profile");
			this.profile = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.parent = this.characterInterface.gunPoint.transform;
			this.weapon = gameObject2;
			this.weapon.transform.localPosition = Vector3.zero;
			this.weapon.transform.localRotation = Quaternion.identity;
			if (this.profile != null)
			{
				this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().AddClip(this.profile, "Profile");
				this.weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
			}
			this.gun = gameObject2.GetComponent<WeaponSounds>().bonusPrefab;
			WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(gameObject2.nameNoClone());
			if (skinForWeapon != null)
			{
				skinForWeapon.SetTo(gameObject2);
			}
			this.SetCurrentSkin();
		}
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject3 in array)
		{
			if (gameObject3.name.Equals("GunFlash"))
			{
				gameObject3.SetActive(false);
			}
		}
		this.ResetArmor();
		ShopNGUIController.DisableLightProbesRecursively(this.characterInterface.gameObject);
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000B1BF8 File Offset: 0x000AFDF8
	private void SetCurrentSkin()
	{
		this.characterInterface.SetSkin(SkinsController.currentSkinForPers, (!(this.weapon != null)) ? null : this.weapon.GetComponent<WeaponSounds>(), null);
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000B1C38 File Offset: 0x000AFE38
	public void UpdateWear()
	{
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		this.characterInterface.UpdateCape(@string, null, !ShopNGUIController.ShowWear);
		string string2 = Storager.getString("MaskEquippedSN", false);
		this.characterInterface.UpdateMask(string2, !ShopNGUIController.ShowWear);
		string text = Storager.getString(Defs.HatEquppedSN, false);
		string string3 = Storager.getString(Defs.VisualHatArmor, false);
		if (!string.IsNullOrEmpty(string3) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(text) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(text) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(string3))
		{
			text = string3;
		}
		this.characterInterface.UpdateHat(text, !ShopNGUIController.ShowWear);
		string string4 = Storager.getString(Defs.BootsEquppedSN, false);
		this.characterInterface.UpdateBoots(string4, !ShopNGUIController.ShowWear);
		ShopNGUIController.SetPersHatVisible(this.characterInterface.hatPoint);
		this.ResetArmor();
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x000B1D48 File Offset: 0x000AFF48
	private void ResetArmor()
	{
		string text = Storager.getString(Defs.ArmorNewEquppedSN, false);
		string @string = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(text) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(text) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			text = @string;
		}
		this.characterInterface.UpdateArmor(text, this.weapon);
		ShopNGUIController.SetPersArmorVisible(this.characterInterface.armorPoint);
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x000B1DEC File Offset: 0x000AFFEC
	private void Update()
	{
		if (Camera.main != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				RaycastHit raycastHit;
				if (Input.GetTouch(i).phase == TouchPhase.Began && Physics.Raycast(ray, out raycastHit, 1000f, -5) && raycastHit.collider.gameObject.name.Equals("MainMenu_Pers"))
				{
					PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 1);
					ConnectSceneNGUIController.GoToProfile();
					return;
				}
			}
		}
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000B1EA0 File Offset: 0x000B00A0
	private void OnDestroy()
	{
		if (ShopNGUIController.sharedShop != null)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
			ShopNGUIController.sharedShop.wearEquipAction = null;
			ShopNGUIController.sharedShop.wearUnequipAction = null;
		}
		if (this.profile != null)
		{
			Resources.UnloadAsset(this.profile);
		}
		ShopNGUIController.ShowArmorChanged -= this.UpdateWear;
		ShopNGUIController.ShowWearChanged -= this.UpdateWear;
		PersConfigurator.currentConfigurator = null;
	}

	// Token: 0x04001828 RID: 6184
	public static PersConfigurator currentConfigurator;

	// Token: 0x04001829 RID: 6185
	private CharacterInterface characterInterface;

	// Token: 0x0400182A RID: 6186
	public GameObject gun;

	// Token: 0x0400182B RID: 6187
	private GameObject weapon;

	// Token: 0x0400182C RID: 6188
	private NickLabelController _label;

	// Token: 0x0400182D RID: 6189
	private GameObject shadow;

	// Token: 0x0400182E RID: 6190
	private AnimationClip profile;
}
