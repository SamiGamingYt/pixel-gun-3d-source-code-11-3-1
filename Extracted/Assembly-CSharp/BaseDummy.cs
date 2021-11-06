using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class BaseDummy : MonoBehaviour
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000119 RID: 281 RVA: 0x0000BA44 File Offset: 0x00009C44
	public bool isDead
	{
		get
		{
			return this.isDown;
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0000BA4C File Offset: 0x00009C4C
	private void OnPolygonEnter()
	{
		if (this.jetpackSounds != null)
		{
			this.jetpackSounds.SetActive(Defs.isSoundFX);
		}
		if (this.jetpackPoint != null)
		{
			this.jetpackPoint.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000BA9C File Offset: 0x00009C9C
	private void OnPolygonExit()
	{
		if (this.jetpackSounds != null)
		{
			this.jetpackSounds.SetActive(false);
		}
		if (this.jetpackPoint != null)
		{
			this.jetpackPoint.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000BAE8 File Offset: 0x00009CE8
	public void GetDamage(float dm, Player_move_c.TypeKills damageType)
	{
		if (this.isDown)
		{
			return;
		}
		this.health -= dm;
		this.DamageEffects((int)damageType);
		if (Defs.isSoundFX)
		{
			this.audioSource.PlayOneShot(this.hitSound);
		}
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000BB48 File Offset: 0x00009D48
	private void Die()
	{
		if (!this.flyTarget)
		{
			this.LayDown();
		}
		else
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.explosionEffect.gameObject, this.explosionEffect.position, Quaternion.identity) as GameObject;
			gameObject.SetActive(true);
			gameObject.GetComponent<AudioSource>().enabled = Defs.isSoundFX;
			UnityEngine.Object.Destroy(gameObject, 2f);
			base.GetComponent<MovingDummy>().ResetPath();
			this.health = this.startHealth;
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000BBCC File Offset: 0x00009DCC
	private void DamageEffects(int _typeKills)
	{
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000BBD0 File Offset: 0x00009DD0
	private IEnumerator HitMaterial(bool poison)
	{
		this.SetTextureForBody((!poison) ? SkinsController.damageHitTexture : SkinsController.poisonHitTexture);
		yield return new WaitForSeconds(0.125f);
		this.SetTextureForBody(this.myTexture);
		yield break;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000BBFC File Offset: 0x00009DFC
	public void SetTextureForBody(Texture texture)
	{
		if (this.bodyMaterial != null)
		{
			this.bodyMaterial.mainTexture = texture;
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000BC1C File Offset: 0x00009E1C
	private string GetArmorForTier(bool topArmor)
	{
		List<string> list = Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0];
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			if (Wear.TierForWear(list[i]) <= ExpController.OurTierForAnyPlace())
			{
				list2.Add(list[i]);
			}
		}
		return list2[(!topArmor) ? (list2.Count - 3) : (list2.Count - 1)];
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0000BC9C File Offset: 0x00009E9C
	private void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		this._animation = base.GetComponent<Animation>();
		this._animation.Play("Dummy_Idle", PlayMode.StopAll);
		int num = 1;
		switch (this.healthType)
		{
		case BaseDummy.HealthType.AverageOnTier:
		{
			int num2 = ExpController.LevelsForTiers[ExpController.OurTierForAnyPlace()];
			int num3 = (ExpController.LevelsForTiers.Length > ExpController.OurTierForAnyPlace() + 1) ? ExpController.LevelsForTiers[ExpController.OurTierForAnyPlace() + 1] : 31;
			num = Mathf.RoundToInt((float)(num3 + num2) / 2f);
			break;
		}
		case BaseDummy.HealthType.LowestOnTier:
			num = ExpController.LevelsForTiers[ExpController.OurTierForAnyPlace()];
			break;
		case BaseDummy.HealthType.HighestOnTier:
			num = ((ExpController.LevelsForTiers.Length > ExpController.OurTierForAnyPlace() + 1) ? ExpController.LevelsForTiers[ExpController.OurTierForAnyPlace() + 1] : 31);
			break;
		}
		this.health = ExperienceController.HealthByLevel[num];
		if (this.armorType != BaseDummy.ArmorType.None)
		{
			string armorForTier = this.GetArmorForTier(this.armorType == BaseDummy.ArmorType.TopArmorOnTier);
			this.SetArmor(armorForTier);
			this.health += Wear.armorNum[armorForTier];
		}
		this.startHealth = this.health;
		this.bodyMaterial = this.skinnedMesh.material;
		this.skinnedMesh.sharedMaterial = this.bodyMaterial;
		this.myTexture = this.bodyMaterial.mainTexture;
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000BE04 File Offset: 0x0000A004
	private void Start()
	{
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000BE08 File Offset: 0x0000A008
	private void LayDown()
	{
		if (Defs.isSoundFX)
		{
			this.audioSource.clip = this.downSound;
			this.audioSource.Play();
		}
		this.isDown = true;
		foreach (Collider collider in this.colliders)
		{
			collider.enabled = false;
		}
		this._animation.Play("Dead", PlayMode.StopAll);
		this.nextUpTime = Time.time + this.awakeTime;
		if (this.hidingTarget)
		{
			this.nextHideTime = Time.time + this.hideTime + this._animation.GetClip("Dummy_Up").length + this._animation.GetClip("Dead").length;
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000BED8 File Offset: 0x0000A0D8
	private IEnumerator GetUp()
	{
		if (Defs.isSoundFX)
		{
			this.audioSource.clip = this.upSound;
			this.audioSource.Play();
		}
		this.isDown = false;
		this.isHiding = false;
		if (!this.flyTarget)
		{
			this._animation.Play("Dummy_Up", PlayMode.StopAll);
			yield return new WaitForSeconds(this._animation.GetClip("Dummy_Up").length * 0.55f);
		}
		yield return null;
		foreach (Collider collider in this.colliders)
		{
			collider.enabled = true;
		}
		yield break;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
	private void OnDestroy()
	{
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000BEF8 File Offset: 0x0000A0F8
	private void Update()
	{
		if (this.hidingTarget && !this.isDown && this.nextHideTime < Time.time)
		{
			this.isHiding = true;
			this.LayDown();
		}
		if (this.isDown && this.nextUpTime < Time.time)
		{
			if (!this.isHiding)
			{
				this.health = this.startHealth;
			}
			base.StartCoroutine(this.GetUp());
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0000BF78 File Offset: 0x0000A178
	public void SetArmor(string armor)
	{
		GameObject gameObject = Resources.Load("Armor/" + armor) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("armorPrefab == null");
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		ArmorRefs component = gameObject2.transform.GetChild(0).GetComponent<ArmorRefs>();
		if (component != null)
		{
			if (component.leftBone != null)
			{
				component.leftBone.parent = this.armorArmLeft;
				component.leftBone.localPosition = Vector3.zero;
				component.leftBone.localRotation = Quaternion.identity;
				component.leftBone.localScale = new Vector3(1f, 1f, 1f);
			}
			if (component.rightBone != null)
			{
				component.rightBone.parent = this.armorArmRight;
				component.rightBone.localPosition = Vector3.zero;
				component.rightBone.localRotation = Quaternion.identity;
				component.rightBone.localScale = new Vector3(1f, 1f, 1f);
			}
			gameObject2.transform.parent = this.armorPoint;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			Player_move_c.SetLayerRecursively(gameObject2, base.gameObject.layer);
		}
	}

	// Token: 0x040000F5 RID: 245
	public float health;

	// Token: 0x040000F6 RID: 246
	private float startHealth;

	// Token: 0x040000F7 RID: 247
	public float awakeTime = 2f;

	// Token: 0x040000F8 RID: 248
	public float hideTime = 2f;

	// Token: 0x040000F9 RID: 249
	private bool isDown = true;

	// Token: 0x040000FA RID: 250
	private bool isHiding;

	// Token: 0x040000FB RID: 251
	public bool flyTarget;

	// Token: 0x040000FC RID: 252
	public bool hidingTarget;

	// Token: 0x040000FD RID: 253
	private Animation _animation;

	// Token: 0x040000FE RID: 254
	private AudioSource audioSource;

	// Token: 0x040000FF RID: 255
	private float nextHideTime;

	// Token: 0x04000100 RID: 256
	private float nextUpTime;

	// Token: 0x04000101 RID: 257
	public Collider[] colliders;

	// Token: 0x04000102 RID: 258
	public Transform armorArmLeft;

	// Token: 0x04000103 RID: 259
	public Transform armorArmRight;

	// Token: 0x04000104 RID: 260
	public Transform armorPoint;

	// Token: 0x04000105 RID: 261
	public BaseDummy.HealthType healthType = BaseDummy.HealthType.LowestOnTier;

	// Token: 0x04000106 RID: 262
	public BaseDummy.ArmorType armorType;

	// Token: 0x04000107 RID: 263
	public SkinnedMeshRenderer skinnedMesh;

	// Token: 0x04000108 RID: 264
	public Texture hitTexture;

	// Token: 0x04000109 RID: 265
	private Texture myTexture;

	// Token: 0x0400010A RID: 266
	private Material bodyMaterial;

	// Token: 0x0400010B RID: 267
	public AudioClip hitSound;

	// Token: 0x0400010C RID: 268
	public AudioClip upSound;

	// Token: 0x0400010D RID: 269
	public AudioClip downSound;

	// Token: 0x0400010E RID: 270
	public Transform jetpackPoint;

	// Token: 0x0400010F RID: 271
	public GameObject jetpackSounds;

	// Token: 0x04000110 RID: 272
	public Transform explosionEffect;

	// Token: 0x0200002A RID: 42
	public enum HealthType
	{
		// Token: 0x04000112 RID: 274
		AverageOnTier,
		// Token: 0x04000113 RID: 275
		LowestOnTier,
		// Token: 0x04000114 RID: 276
		HighestOnTier
	}

	// Token: 0x0200002B RID: 43
	public enum ArmorType
	{
		// Token: 0x04000116 RID: 278
		None,
		// Token: 0x04000117 RID: 279
		LowArmorOnTier,
		// Token: 0x04000118 RID: 280
		TopArmorOnTier
	}
}
