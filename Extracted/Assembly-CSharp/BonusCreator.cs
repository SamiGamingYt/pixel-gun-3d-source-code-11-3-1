using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x0200003C RID: 60
internal sealed class BonusCreator : MonoBehaviour
{
	// Token: 0x060001A2 RID: 418 RVA: 0x00010648 File Offset: 0x0000E848
	private float _DistrSum()
	{
		float num = 0f;
		foreach (object obj in this._weaponsProbDistr)
		{
			int num2 = (int)obj;
			num += (float)num2;
		}
		return num;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x000106BC File Offset: 0x0000E8BC
	private void Awake()
	{
		if (Defs.IsSurvival)
		{
			this.creationInterval = 9f;
			this.weaponCreationInterval = 15f;
		}
		if (Defs.isMulti)
		{
			this._isMultiplayer = true;
		}
		else
		{
			this._isMultiplayer = false;
		}
		if (!this._isMultiplayer)
		{
			this.weaponPrefabs = WeaponManager.sharedManager.weaponsInGame;
			foreach (GameObject gameObject in this.weaponPrefabs)
			{
				WeaponSounds component = gameObject.GetComponent<WeaponSounds>();
				this._weaponsProbDistr.Add(component.Probability);
			}
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00010764 File Offset: 0x0000E964
	private void Start()
	{
		this._bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		this._zombieCreator = base.gameObject.GetComponent<ZombieCreator>();
		this._weaponManager = WeaponManager.sharedManager;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x000107A0 File Offset: 0x0000E9A0
	public void BeginCreateBonuses()
	{
		if (Application.isEditor && Defs.IsSurvival && !SceneLoader.ActiveSceneName.Equals(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length]))
		{
			return;
		}
		if (Defs.IsSurvival)
		{
			base.StartCoroutine(this.AddWeapon());
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x000107FC File Offset: 0x0000E9FC
	public void addBonusFromPhotonRPC(int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.bonusPrefabs[this._indexForType(_type)], _pos, rot);
		gameObject.GetComponent<PhotonView>().viewID = _id;
		gameObject.GetComponent<SettingBonus>().typeOfMass = _type;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00010840 File Offset: 0x0000EA40
	private int _indexForType(int type)
	{
		int result = 0;
		if (type == 9 || type == 10)
		{
			result = 1;
		}
		else if (type == 8)
		{
			result = 2;
		}
		return result;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00010870 File Offset: 0x0000EA70
	[PunRPC]
	[RPC]
	private void delBonus(NetworkViewID id)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Bonus");
		foreach (GameObject gameObject in array)
		{
			if (id.Equals(gameObject.GetComponent<NetworkView>().viewID))
			{
				UnityEngine.Object.Destroy(gameObject);
				break;
			}
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x000108CC File Offset: 0x0000EACC
	private IEnumerator AddWeapon()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.weaponCreationInterval);
			while (this._zombieCreator.stopGeneratingBonuses)
			{
				yield return null;
			}
			int enemiesLeft = GlobalGameController.EnemiesToKill - this._zombieCreator.NumOfDeadZombies;
			if (!Defs.IsSurvival && enemiesLeft <= 0 && !this._zombieCreator.bossShowm)
			{
				break;
			}
			GameObject spawnZone = this._bonusCreationZones[UnityEngine.Random.Range(0, this._bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
			Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
			Vector3 pos = new Vector3(zoneRect.x + UnityEngine.Random.Range(0f, zoneRect.width), spawnZone.transform.position.y, zoneRect.y + UnityEngine.Random.Range(0f, zoneRect.height));
			float sum = this._DistrSum();
			int weaponNumber;
			do
			{
				weaponNumber = 0;
				float val = UnityEngine.Random.Range(0f, sum);
				float curSum = 0f;
				for (int i = 0; i < this._weaponsProbDistr.Count; i++)
				{
					if (val < curSum + (float)((int)this._weaponsProbDistr[i]))
					{
						weaponNumber = i;
						break;
					}
					curSum += (float)((int)this._weaponsProbDistr[i]);
				}
			}
			while (weaponNumber == this._lastWeapon || (Defs.IsSurvival && !ZombieCreator.survivalAvailableWeapons.Contains(this.weaponPrefabs[weaponNumber].name)) || !ItemDb.IsWeaponCanDrop(ItemDb.GetByPrefabName((this.weaponPrefabs[weaponNumber] as GameObject).name.Replace("(Clone)", string.Empty)).Tag));
			GameObject wp = (GameObject)this.weaponPrefabs[weaponNumber];
			GameObject newBonus = BonusCreator._CreateBonus(wp.name, pos);
			this._weapons.Add(newBonus);
			if (this._weapons.Count > ((!Defs.IsSurvival) ? 5 : 3))
			{
				UnityEngine.Object.Destroy((GameObject)this._weapons[0]);
				this._weapons.RemoveAt(0);
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x000108E8 File Offset: 0x0000EAE8
	public static GameObject _CreateBonusPrefab(string _weaponName)
	{
		GameObject gameObject = Resources.Load("Weapon_Bonuses/" + _weaponName + "_Bonus") as GameObject;
		if (gameObject == null)
		{
			Debug.Log("null");
			return null;
		}
		return gameObject;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0001092C File Offset: 0x0000EB2C
	public static GameObject _CreateBonus(string _weaponName, Vector3 pos)
	{
		GameObject gameObject = BonusCreator._CreateBonusPrefab(_weaponName);
		if (gameObject == null)
		{
			Debug.Log("null");
			return null;
		}
		return BonusCreator._CreateBonusFromPrefab(gameObject, pos);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00010960 File Offset: 0x0000EB60
	public static GameObject _CreateBonusFromPrefab(UnityEngine.Object bonus, Vector3 pos)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(bonus, pos, Quaternion.identity);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return gameObject;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x000109A0 File Offset: 0x0000EBA0
	private int _curLevel()
	{
		return Defs.isMulti ? GlobalGameController.currentLevel : CurrentCampaignGame.currentLevel;
	}

	// Token: 0x040001A2 RID: 418
	public GameObject[] bonusPrefabs;

	// Token: 0x040001A3 RID: 419
	public float creationInterval = 15f;

	// Token: 0x040001A4 RID: 420
	public float weaponCreationInterval = 30f;

	// Token: 0x040001A5 RID: 421
	private UnityEngine.Object[] weaponPrefabs;

	// Token: 0x040001A6 RID: 422
	private int _lastWeapon = -1;

	// Token: 0x040001A7 RID: 423
	private bool _isMultiplayer;

	// Token: 0x040001A8 RID: 424
	private ArrayList bonuses = new ArrayList();

	// Token: 0x040001A9 RID: 425
	private ArrayList _weapons = new ArrayList();

	// Token: 0x040001AA RID: 426
	public WeaponManager _weaponManager;

	// Token: 0x040001AB RID: 427
	private GameObject[] _bonusCreationZones;

	// Token: 0x040001AC RID: 428
	private ZombieCreator _zombieCreator;

	// Token: 0x040001AD RID: 429
	private ArrayList _weaponsProbDistr = new ArrayList();
}
