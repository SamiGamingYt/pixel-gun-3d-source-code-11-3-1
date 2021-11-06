using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Rilisoft;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class BonusController : MonoBehaviour
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000184 RID: 388 RVA: 0x0000F270 File Offset: 0x0000D470
	// (set) Token: 0x06000185 RID: 389 RVA: 0x0000F278 File Offset: 0x0000D478
	private NetworkView _networkView { get; set; }

	// Token: 0x06000186 RID: 390 RVA: 0x0000F284 File Offset: 0x0000D484
	private void InitStack()
	{
		this.bonusStack = new BonusItem[this.maxCountBonus + 6];
		for (int i = 0; i < this.bonusStack.Length; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.bonusPrefab, Vector3.down * 100f, Quaternion.identity);
			gameObject.transform.parent = base.transform;
			this.bonusStack[i] = gameObject.GetComponent<BonusItem>();
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000F304 File Offset: 0x0000D504
	private void Awake()
	{
		if (BonusController.sharedController == null)
		{
			BonusController.sharedController = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (Defs.IsSurvival)
		{
			this.creationInterval = 9f;
		}
		else if (Defs.isDuel)
		{
			this.creationInterval = 15f;
		}
		this.timerToAddBonus = this.creationInterval;
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.maxCountBonus = ((!Defs.isDuel) ? ((!Defs.IsSurvival) ? 5 : 3) : 2);
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
	private void Start()
	{
		this.photonView = PhotonView.Get(this);
		this._networkView = base.GetComponent<NetworkView>();
		if (this.photonView)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		this.bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		if (this.maxCountBonus > this.bonusCreationZones.Length)
		{
			this.maxCountBonus = this.bonusCreationZones.Length;
		}
		this.zombieCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		this._weaponManager = WeaponManager.sharedManager;
		this.SetProbability();
		this.InitStack();
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000F450 File Offset: 0x0000D650
	private void SetProbability()
	{
		this.probabilityBonusDict.Clear();
		this.probabilityBonus.Clear();
		this.sumProbabilitys = 0;
		if (Defs.isMulti)
		{
			if (Defs.isHunger)
			{
				this.probabilityBonusDict.Add(3, 100);
			}
			else if (SceneLoader.ActiveSceneName.Equals("Knife"))
			{
				this.probabilityBonusDict.Add(1, 75);
				this.probabilityBonusDict.Add(2, 25);
			}
			else if (Defs.isDaterRegim)
			{
				this.probabilityBonusDict.Add(0, 100);
			}
			else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				this.probabilityBonusDict.Add(0, 100);
			}
			else if (Defs.isCOOP)
			{
				this.probabilityBonusDict.Add(0, 55);
				this.probabilityBonusDict.Add(1, 14);
				this.probabilityBonusDict.Add(2, 12);
			}
			else if (Defs.isDuel)
			{
				this.probabilityBonusDict.Add(0, 50);
				this.probabilityBonusDict.Add(1, 20);
				this.probabilityBonusDict.Add(2, 20);
			}
			else if (SceneLoader.ActiveSceneName.Equals("WalkingFortress"))
			{
				this.probabilityBonusDict.Add(0, 50);
				this.probabilityBonusDict.Add(1, 10);
				this.probabilityBonusDict.Add(2, 5);
			}
			else
			{
				this.probabilityBonusDict.Add(0, 50);
				this.probabilityBonusDict.Add(1, 10);
				this.probabilityBonusDict.Add(2, 10);
			}
		}
		else if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.probabilityBonusDict.Add(0, 100);
		}
		else
		{
			this.probabilityBonusDict.Add(0, 55);
			this.probabilityBonusDict.Add(1, 14);
			this.probabilityBonusDict.Add(2, 12);
		}
		foreach (KeyValuePair<int, int> keyValuePair in this.probabilityBonusDict)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("min", this.sumProbabilitys);
			this.sumProbabilitys += keyValuePair.Value;
			dictionary.Add("max", this.sumProbabilitys);
			this.probabilityBonus.Add(keyValuePair.Key, dictionary);
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
	public void AddWeaponAfterKillPlayer(string _weaponName, Vector3 _pos)
	{
		this.photonView.RPC("AddWeaponAfterKillPlayerRPC", PhotonTargets.MasterClient, new object[]
		{
			_weaponName,
			_pos
		});
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000F724 File Offset: 0x0000D924
	[RPC]
	[PunRPC]
	private void AddWeaponAfterKillPlayerRPC(string _weaponName, Vector3 _pos)
	{
		PhotonNetwork.InstantiateSceneObject("Weapon_Bonuses/" + _weaponName + "_Bonus", new Vector3(_pos.x, _pos.y - 0.5f, _pos.z), Quaternion.identity, 0, null);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000F764 File Offset: 0x0000D964
	public void AddBonusAfterKillPlayer(Vector3 _pos)
	{
		if (Defs.isInet)
		{
			this.photonView.RPC("AddBonusAfterKillPlayerRPC", PhotonTargets.MasterClient, new object[]
			{
				_pos
			});
		}
		else
		{
			this._networkView.RPC("AddBonusAfterKillPlayerRPC", RPCMode.Server, new object[]
			{
				_pos
			});
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
	[RPC]
	[PunRPC]
	private void AddBonusAfterKillPlayerRPC(Vector3 _pos)
	{
		this.AddBonusAfterKillPlayerRPC(this.IndexBonusOnKill(), _pos);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000F7D0 File Offset: 0x0000D9D0
	[PunRPC]
	[RPC]
	private void AddBonusAfterKillPlayerRPC(int _type, Vector3 _pos)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet && PhotonNetwork.isMasterClient && !Defs.isHunger)
			{
				this.AddBonus(_pos, _type);
			}
			if (!Defs.isInet && Network.isServer)
			{
				this.AddBonus(_pos, _type);
			}
			return;
		}
		this.AddBonus(_pos, _type);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000F834 File Offset: 0x0000DA34
	private void AddBonus(Vector3 pos, int _type)
	{
		if (_type == 5 || _type == 8 || _type == 7 || _type == 6 || _type == 4)
		{
			return;
		}
		if (!this.isMulti)
		{
			int num = GlobalGameController.EnemiesToKill - this.zombieCreator.NumOfDeadZombies;
			if ((!Defs.IsSurvival && num <= 0 && !this.zombieCreator.bossShowm) || (Defs.IsSurvival && this.zombieCreator.stopGeneratingBonuses))
			{
				if (!Defs.IsSurvival)
				{
					this.isStopCreateBonus = true;
				}
				return;
			}
		}
		if (_type == 9)
		{
			if (!this.CanSpawnGemBonus())
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable["SpecialBonus"] = PhotonNetwork.time + 480.0;
			PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		}
		int num2 = -1;
		bool flag = pos.Equals(Vector3.zero);
		if (flag)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Chest");
			if (this.activeBonusesCount + array.Length >= this.maxCountBonus)
			{
				return;
			}
			num2 = UnityEngine.Random.Range(0, this.bonusCreationZones.Length);
			bool[] array2 = new bool[this.bonusCreationZones.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = false;
			}
			for (int j = 0; j < this.bonusStack.Length; j++)
			{
				if (this.bonusStack[j].isActive && this.bonusStack[j].mySpawnNumber != -1)
				{
					array2[this.bonusStack[j].mySpawnNumber] = true;
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k].GetComponent<SettingBonus>().numberSpawnZone != -1)
				{
					array2[array[k].GetComponent<SettingBonus>().numberSpawnZone] = true;
				}
			}
			while (array2[num2])
			{
				num2++;
				if (num2 == array2.Length)
				{
					num2 = 0;
				}
			}
			GameObject gameObject = this.bonusCreationZones[num2];
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
			Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
			pos = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		}
		if (_type != 3)
		{
			for (int l = 0; l < this.bonusStack.Length; l++)
			{
				if (!this.bonusStack[l].isActive)
				{
					this.MakeBonusRPC(l, _type, pos, (num2 != -1) ? -1f : ((float)this.GetTimeForBonus()), num2);
					if (this.isMulti)
					{
						if (this.isInet)
						{
							this.photonView.RPC("MakeBonusRPC", PhotonTargets.Others, new object[]
							{
								l,
								_type,
								pos,
								(num2 != -1) ? -1f : ((float)this.GetTimeForBonus()),
								num2
							});
						}
						else
						{
							this._networkView.RPC("MakeBonusRPC", RPCMode.Others, new object[]
							{
								l,
								_type,
								pos,
								(num2 != -1) ? -1f : ((float)this.GetTimeForBonus()),
								num2
							});
						}
					}
					break;
				}
			}
		}
		else if (!this.isMulti || !this.isInet)
		{
			GameObject original = Resources.Load("Bonuses/Bonus_" + _type) as GameObject;
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(original, pos, Quaternion.identity);
			gameObject2.GetComponent<SettingBonus>().numberSpawnZone = num2;
		}
		else
		{
			GameObject gameObject2 = PhotonNetwork.InstantiateSceneObject("Bonuses/Bonus_" + ((_type == -1) ? this.IndexBonus() : _type), pos, Quaternion.identity, 0, null);
			gameObject2.GetComponent<SettingBonus>().SetNumberSpawnZone(num2);
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000FD20 File Offset: 0x0000DF20
	public void AddBonusForHunger(Vector3 pos, int _type, int spawnZoneIndex)
	{
		if (!Defs.isHunger)
		{
			return;
		}
		for (int i = 0; i < this.bonusStack.Length; i++)
		{
			if (!this.bonusStack[i].isActive)
			{
				this.MakeBonusRPC(i, _type, pos, -1f, spawnZoneIndex);
				if (this.isMulti)
				{
					if (this.isInet)
					{
						this.photonView.RPC("MakeBonusRPC", PhotonTargets.Others, new object[]
						{
							i,
							_type,
							pos,
							-1f,
							spawnZoneIndex
						});
					}
					else
					{
						this._networkView.RPC("MakeBonusRPC", RPCMode.Others, new object[]
						{
							i,
							_type,
							pos,
							-1f,
							spawnZoneIndex
						});
					}
				}
				break;
			}
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000FE24 File Offset: 0x0000E024
	public void RemoveBonus(int index)
	{
		this.RemoveBonusRPC(index);
		if (this.isMulti)
		{
			if (this.isInet)
			{
				this.photonView.RPC("RemoveBonusRPC", PhotonTargets.Others, new object[]
				{
					index
				});
			}
			else
			{
				this._networkView.RPC("RemoveBonusRPC", RPCMode.Others, new object[]
				{
					index
				});
			}
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000FE94 File Offset: 0x0000E094
	public void GetAndRemoveBonus(int index)
	{
		if (this.isMulti && this.isInet && !NetworkStartTable.LocalOrPasswordRoom())
		{
			this.RemoveBonusWithRewardRPC(PhotonNetwork.player, index);
			this.photonView.RPC("RemoveBonusWithRewardRPC", PhotonTargets.Others, new object[]
			{
				PhotonNetwork.player,
				index
			});
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
	public void ClearBonuses()
	{
		for (int i = 0; i < this.bonusStack.Length; i++)
		{
			if (this.bonusStack[i].isActive)
			{
				this.RemoveBonusRPC(i);
			}
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000FF38 File Offset: 0x0000E138
	private void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.isMasterClient)
		{
			for (int i = 0; i < this.bonusStack.Length; i++)
			{
				if (this.bonusStack[i].isActive)
				{
					this.photonView.RPC("MakeBonusRPC", player, new object[]
					{
						i,
						(int)this.bonusStack[i].type,
						this.bonusStack[i].transform.position,
						(float)this.bonusStack[i].expireTime,
						this.bonusStack[i].mySpawnNumber
					});
				}
			}
		}
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000FFF4 File Offset: 0x0000E1F4
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			for (int i = 0; i < this.bonusStack.Length; i++)
			{
				if (this.bonusStack[i].isActive)
				{
					this._networkView.RPC("MakeBonusRPC", player, new object[]
					{
						i,
						(int)this.bonusStack[i].type,
						this.bonusStack[i].transform.position,
						(float)this.bonusStack[i].expireTime,
						this.bonusStack[i].mySpawnNumber
					});
				}
			}
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x000100B0 File Offset: 0x0000E2B0
	[RPC]
	[PunRPC]
	private void MakeBonusRPC(int index, int type, Vector3 position, float expireTime, int zoneNumber)
	{
		if (index < this.bonusStack.Length && !this.bonusStack[index].isActive)
		{
			this.bonusStack[index].ActivateBonus((BonusController.TypeBonus)type, position, (double)expireTime, zoneNumber, index);
			if (!this.bonusStack[index].isTimeBonus)
			{
				this.activeBonusesCount++;
			}
		}
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00010114 File Offset: 0x0000E314
	private void PickupBonus(int index, PhotonPlayer player)
	{
		if (index < this.bonusStack.Length && this.bonusStack[index].isActive && !this.bonusStack[index].isPickedUp)
		{
			this.bonusStack[index].PickupBonus(player);
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00010164 File Offset: 0x0000E364
	[PunRPC]
	[RPC]
	private void RemoveBonusRPC(int index)
	{
		if (index < this.bonusStack.Length && this.bonusStack[index].isActive)
		{
			if (!this.bonusStack[index].isTimeBonus)
			{
				this.activeBonusesCount--;
			}
			this.bonusStack[index].DeactivateBonus();
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x000101C0 File Offset: 0x0000E3C0
	[PunRPC]
	[RPC]
	private void RemoveBonusWithRewardRPC(PhotonPlayer sender, int index)
	{
		if (!this.isMulti || !this.isInet || NetworkStartTable.LocalOrPasswordRoom())
		{
			return;
		}
		if (index < this.bonusStack.Length && this.bonusStack[index].isActive)
		{
			this.PickupBonus(index, sender);
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00010218 File Offset: 0x0000E418
	[RPC]
	[PunRPC]
	private void GetBonusRewardRPC(int index)
	{
		if (index < this.bonusStack.Length && this.bonusStack[index].isActive && this.bonusStack[index].isPickedUp)
		{
			if (this.bonusStack[index].playerPicked.Equals(PhotonNetwork.player))
			{
				BonusController.TypeBonus type = this.bonusStack[index].type;
				if (type == BonusController.TypeBonus.Gem)
				{
					BankController.AddGems(1, true, AnalyticsConstants.AccrualType.Earned);
				}
			}
			this.RemoveBonusRPC(index);
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x000102A4 File Offset: 0x0000E4A4
	private double GetTimeForBonus()
	{
		double result;
		if (Defs.isInet)
		{
			result = PhotonNetwork.time + 15.0;
		}
		else
		{
			result = Network.time + 15.0;
		}
		return result;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x000102EC File Offset: 0x0000E4EC
	private bool CanSpawnGemBonus()
	{
		return !Defs.isHunger && Defs.isMulti && Defs.isInet && !NetworkStartTable.LocalOrPasswordRoom() && PhotonNetwork.room != null && PhotonNetwork.room.customProperties["SpecialBonus"] != null && Convert.ToDouble(PhotonNetwork.room.customProperties["SpecialBonus"]) <= PhotonNetwork.time;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0001036C File Offset: 0x0000E56C
	private int IndexBonus()
	{
		int num = UnityEngine.Random.Range(0, this.sumProbabilitys);
		foreach (KeyValuePair<int, Dictionary<string, int>> keyValuePair in this.probabilityBonus)
		{
			if (num >= keyValuePair.Value["min"] && num < keyValuePair.Value["max"])
			{
				return keyValuePair.Key;
			}
		}
		return 0;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00010418 File Offset: 0x0000E618
	private int IndexBonusOnKill()
	{
		bool flag = this.CanSpawnGemBonus();
		if (flag && UnityEngine.Random.Range(0, 100) < 5)
		{
			return 9;
		}
		int num = UnityEngine.Random.Range(0, this.sumProbabilitys);
		foreach (KeyValuePair<int, Dictionary<string, int>> keyValuePair in this.probabilityBonus)
		{
			if (num >= keyValuePair.Value["min"] && num < keyValuePair.Value["max"])
			{
				return keyValuePair.Key;
			}
		}
		return 0;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000104E4 File Offset: 0x0000E6E4
	private void Update()
	{
		bool flag;
		if (this.isMulti)
		{
			if (this.isInet)
			{
				flag = PhotonNetwork.isMasterClient;
			}
			else
			{
				flag = Network.isServer;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < this.bonusStack.Length; i++)
			{
				if (this.bonusStack[i].isActive && this.bonusStack[i].isPickedUp)
				{
					this.photonView.RPC("GetBonusRewardRPC", PhotonTargets.All, new object[]
					{
						i
					});
				}
			}
		}
		if (!this.isStopCreateBonus && flag)
		{
			this.timerToAddBonus -= Time.deltaTime;
		}
		if (this.timerToAddBonus < 0f)
		{
			this.timerToAddBonus = this.creationInterval;
			this.AddBonus(Vector3.zero, this.IndexBonus());
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000105D4 File Offset: 0x0000E7D4
	private void OnDestroy()
	{
		if (this.photonView)
		{
			PhotonObjectCacher.RemoveObject(base.gameObject);
		}
	}

	// Token: 0x04000184 RID: 388
	public static BonusController sharedController;

	// Token: 0x04000185 RID: 389
	public GameObject bonusPrefab;

	// Token: 0x04000186 RID: 390
	public BonusItem[] bonusStack;

	// Token: 0x04000187 RID: 391
	private float creationInterval = 7f;

	// Token: 0x04000188 RID: 392
	public float timerToAddBonus;

	// Token: 0x04000189 RID: 393
	private bool isMulti;

	// Token: 0x0400018A RID: 394
	private bool isInet;

	// Token: 0x0400018B RID: 395
	private bool isStopCreateBonus;

	// Token: 0x0400018C RID: 396
	public bool isBeginCreateBonuses;

	// Token: 0x0400018D RID: 397
	private WeaponManager _weaponManager;

	// Token: 0x0400018E RID: 398
	private GameObject[] bonusCreationZones;

	// Token: 0x0400018F RID: 399
	private ZombieCreator zombieCreator;

	// Token: 0x04000190 RID: 400
	private PhotonView photonView;

	// Token: 0x04000191 RID: 401
	public int maxCountBonus = 5;

	// Token: 0x04000192 RID: 402
	private int activeBonusesCount;

	// Token: 0x04000193 RID: 403
	private int sumProbabilitys;

	// Token: 0x04000194 RID: 404
	private Dictionary<int, int> probabilityBonusDict = new Dictionary<int, int>();

	// Token: 0x04000195 RID: 405
	private Dictionary<int, Dictionary<string, int>> probabilityBonus = new Dictionary<int, Dictionary<string, int>>();

	// Token: 0x0200003B RID: 59
	public enum TypeBonus
	{
		// Token: 0x04000198 RID: 408
		Ammo,
		// Token: 0x04000199 RID: 409
		Health,
		// Token: 0x0400019A RID: 410
		Armor,
		// Token: 0x0400019B RID: 411
		Chest,
		// Token: 0x0400019C RID: 412
		Grenade,
		// Token: 0x0400019D RID: 413
		Mech,
		// Token: 0x0400019E RID: 414
		JetPack,
		// Token: 0x0400019F RID: 415
		Invisible,
		// Token: 0x040001A0 RID: 416
		Turret,
		// Token: 0x040001A1 RID: 417
		Gem
	}
}
