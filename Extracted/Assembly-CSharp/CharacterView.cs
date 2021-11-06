using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
public class CharacterView : MonoBehaviour
{
	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x0600490A RID: 18698 RVA: 0x0019612C File Offset: 0x0019432C
	public CharacterInterface characterInterface
	{
		get
		{
			if (this._characterInterface == null)
			{
				this.CreateCharacterModel();
			}
			return this._characterInterface;
		}
	}

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x0600490B RID: 18699 RVA: 0x0019614C File Offset: 0x0019434C
	public Transform armorPoint
	{
		get
		{
			return this.characterInterface.armorPoint;
		}
	}

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x0600490C RID: 18700 RVA: 0x0019615C File Offset: 0x0019435C
	public Transform hatPoint
	{
		get
		{
			return this.characterInterface.hatPoint;
		}
	}

	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x0600490D RID: 18701 RVA: 0x0019616C File Offset: 0x0019436C
	public Transform body
	{
		get
		{
			return this.characterInterface.gunPoint;
		}
	}

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x0600490E RID: 18702 RVA: 0x0019617C File Offset: 0x0019437C
	public Transform character
	{
		get
		{
			return this.characterInterface.transform;
		}
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x0019618C File Offset: 0x0019438C
	private void CreateCharacterModel()
	{
		GameObject original = Resources.Load("Character_model") as GameObject;
		this._characterInterface = UnityEngine.Object.Instantiate<GameObject>(original).GetComponent<CharacterInterface>();
		this._characterInterface.transform.SetParent(base.transform, false);
		this._characterInterface.SetCharacterType(false, false, false);
		Player_move_c.SetLayerRecursively(this._characterInterface.gameObject, base.gameObject.layer);
		ShopNGUIController.DisableLightProbesRecursively(this._characterInterface.gameObject);
	}

	// Token: 0x06004910 RID: 18704 RVA: 0x0019620C File Offset: 0x0019440C
	public void ShowCharacterType(CharacterView.CharacterType characterType)
	{
		this.character.gameObject.SetActive(false);
		if (this.mech != null)
		{
			this.mech.gameObject.SetActive(false);
		}
		if (this.turret != null)
		{
			this.turret.gameObject.SetActive(false);
		}
		switch (characterType)
		{
		case CharacterView.CharacterType.Player:
			this.character.gameObject.SetActive(true);
			break;
		case CharacterView.CharacterType.Mech:
			this.mech.gameObject.SetActive(true);
			break;
		case CharacterView.CharacterType.Turret:
			this.turret.gameObject.SetActive(true);
			break;
		}
	}

	// Token: 0x06004911 RID: 18705 RVA: 0x001962CC File Offset: 0x001944CC
	public void UpdateMech(int mechUpgrade)
	{
		this.mechBodyRenderer.material = this.mechBodyMaterials[mechUpgrade];
		this.mechHandRenderer.material = this.mechBodyMaterials[mechUpgrade];
		this.mechGunRenderer.material = this.mechGunMaterials[mechUpgrade];
		this.mechBodyRenderer.material.SetColor("_ColorRili", Color.white);
		this.mechHandRenderer.material.SetColor("_ColorRili", Color.white);
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x00196348 File Offset: 0x00194548
	public void UpdateTurret(int turretUpgrade)
	{
	}

	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x06004913 RID: 18707 RVA: 0x0019634C File Offset: 0x0019454C
	private AnimationCoroutineRunner AnimationRunner
	{
		get
		{
			if (this._animationRunner == null)
			{
				this._animationRunner = base.GetComponent<AnimationCoroutineRunner>();
			}
			return this._animationRunner;
		}
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x00196374 File Offset: 0x00194574
	public void SetWeaponAndSkin(string tg, Texture skinForPers, bool replaceRemovedWeapons)
	{
		this.AnimationRunner.StopAllCoroutines();
		if (WeaponManager.sharedManager == null)
		{
			return;
		}
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
		foreach (object obj in this.body)
		{
			Transform item = (Transform)obj;
			list.Add(item);
		}
		foreach (Transform transform in list)
		{
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (tg == null)
		{
			return;
		}
		if (this._profile != null)
		{
			Resources.UnloadAsset(this._profile);
			this._profile = null;
		}
		GameObject gameObject = null;
		if (tg == "WeaponGrenade")
		{
			gameObject = Resources.Load<GameObject>("WeaponGrenade");
		}
		else
		{
			try
			{
				string weaponName = ItemDb.GetByTag(tg).PrefabName;
				gameObject = WeaponManager.sharedManager.weaponsInGame.OfType<GameObject>().FirstOrDefault((GameObject wp) => wp.name == weaponName);
			}
			catch (Exception arg)
			{
				if (Application.isEditor)
				{
					Debug.LogError("Exception in var weaponName = ItemDb.GetByTag(tg).PrefabName: " + arg);
				}
			}
			if (replaceRemovedWeapons && gameObject != null)
			{
				WeaponSounds weaponSounds = gameObject.GetComponent<WeaponSounds>();
				if (weaponSounds != null && (WeaponManager.Removed150615_PrefabNames.Contains(gameObject.name) || weaponSounds.tier > 100))
				{
					gameObject = WeaponManager.sharedManager.weaponsInGame.OfType<GameObject>().FirstOrDefault((GameObject wp) => wp.name.Equals(weaponSounds.alternativeName));
				}
			}
		}
		if (gameObject == null)
		{
			Debug.Log("pref == null");
			return;
		}
		this._profile = Resources.Load<AnimationClip>("ProfileAnimClips/" + gameObject.name + "_Profile");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		Player_move_c.PerformActionRecurs(gameObject2, delegate(Transform t)
		{
			MeshRenderer component4 = t.GetComponent<MeshRenderer>();
			SkinnedMeshRenderer component5 = t.GetComponent<SkinnedMeshRenderer>();
			if (component4 != null)
			{
				component4.useLightProbes = false;
			}
			if (component5 != null)
			{
				component5.useLightProbes = false;
			}
		});
		Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		gameObject2.transform.parent = this.body;
		this._weapon = gameObject2;
		this._weapon.transform.localScale = new Vector3(1f, 1f, 1f);
		this._weapon.transform.position = this.body.transform.position;
		this._weapon.transform.localPosition = Vector3.zero;
		this._weapon.transform.localRotation = Quaternion.identity;
		WeaponSounds component2 = this._weapon.GetComponent<WeaponSounds>();
		if (this.armorPoint.childCount > 0 && component2 != null)
		{
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
		this.PlayWeaponAnimation();
		this.SetSkinTexture(skinForPers);
		if (tg == "WeaponGrenade")
		{
			this.SetupWeaponGrenade(gameObject2);
		}
		WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(gameObject2.nameNoClone());
		if (skinForWeapon != null)
		{
			skinForWeapon.SetTo(gameObject2);
		}
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x001968D0 File Offset: 0x00194AD0
	public void SetupWeaponGrenade(GameObject weaponGrenade)
	{
		GameObject original = Resources.Load<GameObject>("Rocket");
		Rocket component = (UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Rocket>();
		component.enabled = false;
		component.GetComponent<Rigidbody>().useGravity = false;
		component.GetComponent<Rigidbody>().isKinematic = true;
		component.rocketNum = 10;
		Player_move_c.SetLayerRecursively(component.gameObject, base.gameObject.layer);
		component.transform.parent = weaponGrenade.GetComponent<WeaponSounds>().grenatePoint;
		component.transform.localPosition = Vector3.zero;
		component.transform.localRotation = Quaternion.identity;
		component.transform.localScale = Vector3.one;
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x00196988 File Offset: 0x00194B88
	public void SetSkinTexture(Texture skin)
	{
		this.characterInterface.SetSkin(skin, (this.body.transform.childCount <= 0) ? null : this.body.transform.GetChild(0).GetComponent<WeaponSounds>(), null);
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x001969D4 File Offset: 0x00194BD4
	public void UpdateHat(string hat)
	{
		string @string = Storager.getString(Defs.VisualHatArmor, false);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hat) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hat) < Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(@string))
		{
			hat = @string;
		}
		this.characterInterface.UpdateHat(hat, !ShopNGUIController.ShowWear);
	}

	// Token: 0x06004918 RID: 18712 RVA: 0x00196A60 File Offset: 0x00194C60
	public void RemoveHat()
	{
		this.characterInterface.RemoveHat();
	}

	// Token: 0x06004919 RID: 18713 RVA: 0x00196A70 File Offset: 0x00194C70
	public void UpdateCape(string cape, Texture capeTex = null)
	{
		this.characterInterface.UpdateCape(cape, capeTex, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x00196A94 File Offset: 0x00194C94
	public void RemoveCape()
	{
		this.characterInterface.RemoveCape();
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x00196AA4 File Offset: 0x00194CA4
	public void UpdateMask(string mask)
	{
		this.characterInterface.UpdateMask(mask, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x00196ABC File Offset: 0x00194CBC
	public void RemoveMask()
	{
		this.characterInterface.RemoveMask();
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x00196ACC File Offset: 0x00194CCC
	public void UpdateBoots(string bs)
	{
		this.characterInterface.UpdateBoots(bs, !ShopNGUIController.ShowWear);
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x00196AE4 File Offset: 0x00194CE4
	public void RemoveBoots()
	{
		this.characterInterface.RemoveBoots();
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x00196AF4 File Offset: 0x00194CF4
	public void UpdatePet(string bs)
	{
		this.characterInterface.UpdatePet(bs);
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x00196B04 File Offset: 0x00194D04
	public void RemovePet()
	{
		this.characterInterface.UpdatePet(string.Empty);
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x00196B18 File Offset: 0x00194D18
	public void UpdateArmor(string armor)
	{
		string @string = Storager.getString(Defs.VisualArmor, false);
		if (!string.IsNullOrEmpty(@string) && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) >= 0 && Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armor) < Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(@string))
		{
			armor = @string;
		}
		this.characterInterface.UpdateArmor(armor, this._weapon);
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x00196BA4 File Offset: 0x00194DA4
	public void RemoveArmor()
	{
		this.characterInterface.RemoveArmor();
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x00196BB4 File Offset: 0x00194DB4
	private void PlayWeaponAnimation()
	{
		if (this._profile != null)
		{
			Animation component = this._weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>();
			if (Time.timeScale != 0f)
			{
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(this._profile, "Profile");
				}
				else
				{
					Debug.LogWarning("Animation clip is null.");
				}
				component.Play("Profile");
			}
			else
			{
				this.AnimationRunner.StopAllCoroutines();
				if (component.GetClip("Profile") == null)
				{
					component.AddClip(this._profile, "Profile");
				}
				else
				{
					Debug.LogWarning("Animation clip is null.");
				}
				this.AnimationRunner.StartPlay(component, "Profile", false, null);
			}
		}
		else
		{
			Debug.LogWarning("_profile == null");
		}
	}

	// Token: 0x06004924 RID: 18724 RVA: 0x00196CA4 File Offset: 0x00194EA4
	public static Texture2D GetClanLogo(string logoBase64)
	{
		if (string.IsNullOrEmpty(logoBase64))
		{
			return null;
		}
		byte[] data = Convert.FromBase64String(logoBase64);
		Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x0400364D RID: 13901
	public Transform mech;

	// Token: 0x0400364E RID: 13902
	public SkinnedMeshRenderer mechBodyRenderer;

	// Token: 0x0400364F RID: 13903
	public SkinnedMeshRenderer mechHandRenderer;

	// Token: 0x04003650 RID: 13904
	public SkinnedMeshRenderer mechGunRenderer;

	// Token: 0x04003651 RID: 13905
	public Material[] mechGunMaterials;

	// Token: 0x04003652 RID: 13906
	public Material[] mechBodyMaterials;

	// Token: 0x04003653 RID: 13907
	public Transform turret;

	// Token: 0x04003654 RID: 13908
	private CharacterInterface _characterInterface;

	// Token: 0x04003655 RID: 13909
	private AnimationCoroutineRunner _animationRunner;

	// Token: 0x04003656 RID: 13910
	private AnimationClip _profile;

	// Token: 0x04003657 RID: 13911
	private GameObject _weapon;

	// Token: 0x020007E4 RID: 2020
	public enum CharacterType
	{
		// Token: 0x0400365A RID: 13914
		Player,
		// Token: 0x0400365B RID: 13915
		Mech,
		// Token: 0x0400365C RID: 13916
		Turret
	}
}
