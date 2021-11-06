using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
public sealed class ZombiManager : MonoBehaviour
{
	// Token: 0x0600321E RID: 12830 RVA: 0x00104040 File Offset: 0x00102240
	private void Awake()
	{
		try
		{
			this.isPizzaMap = (SceneLoader.ActiveSceneName.Equals("Pizza") || SceneLoader.ActiveSceneName.Equals("Paradise_Night"));
			string[] array;
			if (this.isPizzaMap)
			{
				array = new string[]
				{
					"86",
					"90",
					"88",
					"91",
					"84",
					"87",
					"82",
					"81",
					"92",
					"80",
					"83"
				};
			}
			else
			{
				array = new string[]
				{
					"1",
					"79",
					"2",
					"3",
					"57",
					"16",
					"56",
					"27",
					"73",
					"9",
					"39"
				};
			}
			foreach (string str in array)
			{
				string item = "Enemies/Enemy" + str + "_go";
				this.zombiePrefabs.Add(item);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x001041C4 File Offset: 0x001023C4
	private void Start()
	{
		if (!Defs.isMulti || !Defs.isCOOP)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		ZombiManager.sharedManager = this;
		try
		{
			this.nextAddZombi = 5f;
			this._enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			this.photonView = PhotonView.Get(this);
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x00104258 File Offset: 0x00102458
	[PunRPC]
	[RPC]
	private void synchTime(float _time)
	{
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x0010425C File Offset: 0x0010245C
	public void EndMatch()
	{
		if (this.photonView.isMine)
		{
			if (TimeGameController.sharedController.isEndMatch)
			{
				return;
			}
			TimeGameController.sharedController.isEndMatch = true;
			this.startGame = false;
			this.timeGame = 0.0;
			GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
			float num = -100f;
			string text = string.Empty;
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if ((float)array[i].GetComponent<NetworkStartTable>().score > num)
				{
					num = (float)array[i].GetComponent<NetworkStartTable>().score;
					text = array[i].GetComponent<NetworkStartTable>().NamePlayer;
					num2 = i;
				}
			}
			this.photonView.RPC("win", PhotonTargets.All, new object[]
			{
				text
			});
			this.photonView.RPC("WinID", PhotonTargets.All, new object[]
			{
				array[num2].GetComponent<PhotonView>().ownerId
			});
		}
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x0010435C File Offset: 0x0010255C
	private void Update()
	{
		try
		{
			int count = Initializer.players.Count;
			if (!this.startGame && count > 0)
			{
				this.startGame = true;
				this.timeGame = 0.0;
				this.nextTimeSynch = 0f;
				this.nextAddZombi = 0f;
			}
			if (this.startGame && count == 0)
			{
				this.startGame = false;
				this.timeGame = 0.0;
				this.nextTimeSynch = 0f;
				this.nextAddZombi = 0f;
			}
			if (this.startGame)
			{
				this.timeGame = this.maxTimeGame - TimeGameController.sharedController.timerToEndMatch;
				if (this.timeGame > (double)this.nextAddZombi && this.photonView.isMine && Initializer.enemiesObj.Count < 15)
				{
					float num = 4f;
					if (this.timeGame > this.maxTimeGame * 0.4000000059604645)
					{
						num = 3f;
					}
					if (this.timeGame > this.maxTimeGame * 0.800000011920929)
					{
						num = 2f;
					}
					this.nextAddZombi += num;
					int num2 = Initializer.players.Count - ((!(Application.loadedLevelName == "Arena")) ? 1 : 2);
					num2 = Mathf.Clamp(num2, 1, 15);
					Debug.LogWarning(">>> ZOMBIE COUNT " + num2);
					this.addZombies(num2);
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x00104528 File Offset: 0x00102728
	[PunRPC]
	[RPC]
	private void win(string _winer)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win(_winer, 0, 0, 0);
		}
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x00104560 File Offset: 0x00102760
	private void addZombies(int count)
	{
		for (int i = 0; i < count; i++)
		{
			this.addZombi();
		}
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x00104588 File Offset: 0x00102788
	private void addZombi()
	{
		GameObject gameObject = this._enemyCreationZones[UnityEngine.Random.Range(0, this._enemyCreationZones.Length)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
		Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 position = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		int index = 0;
		double num = this.timeGame / this.maxTimeGame * 100.0;
		if (this.isPizzaMap)
		{
			if (num < 15.0)
			{
				index = UnityEngine.Random.Range(0, 4);
			}
			if (num >= 15.0 && num < 30.0)
			{
				index = UnityEngine.Random.Range(0, 5);
			}
			if (num >= 30.0 && num < 45.0)
			{
				index = UnityEngine.Random.Range(0, 6);
			}
			if (num >= 45.0 && num < 60.0)
			{
				index = UnityEngine.Random.Range(1, 7);
			}
			if (num >= 60.0 && num < 75.0)
			{
				index = UnityEngine.Random.Range(1, 9);
			}
			if (num >= 75.0)
			{
				index = UnityEngine.Random.Range(3, 11);
			}
		}
		else
		{
			if (num < 15.0)
			{
				index = UnityEngine.Random.Range(0, 3);
			}
			if (num >= 15.0 && num < 30.0)
			{
				index = UnityEngine.Random.Range(0, 5);
			}
			if (num >= 30.0 && num < 45.0)
			{
				index = UnityEngine.Random.Range(0, 6);
			}
			if (num >= 45.0 && num < 60.0)
			{
				index = UnityEngine.Random.Range(1, 8);
			}
			if (num >= 60.0 && num < 75.0)
			{
				index = UnityEngine.Random.Range(3, 10);
			}
			if (num >= 75.0)
			{
				index = UnityEngine.Random.Range(3, 11);
			}
		}
		PhotonNetwork.InstantiateSceneObject(this.zombiePrefabs[index], position, Quaternion.identity, 0, null);
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x001048A0 File Offset: 0x00102AA0
	[PunRPC]
	[RPC]
	private void WinID(int winID)
	{
		WeaponManager weaponManager = WeaponManager.sharedManager;
		if (weaponManager.myTable != null && weaponManager.myTable.GetComponent<PhotonView>().ownerId == winID)
		{
			int val = Storager.getInt("Rating", false) + 1;
			Storager.setInt("Rating", val, false);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.TryIncrementWinCountTimestamp();
			}
		}
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x00104910 File Offset: 0x00102B10
	private void OnDestroy()
	{
		ZombiManager.sharedManager = null;
	}

	// Token: 0x040024DE RID: 9438
	public static ZombiManager sharedManager;

	// Token: 0x040024DF RID: 9439
	public double timeGame;

	// Token: 0x040024E0 RID: 9440
	public float nextTimeSynch;

	// Token: 0x040024E1 RID: 9441
	public float nextAddZombi;

	// Token: 0x040024E2 RID: 9442
	public List<string> zombiePrefabs = new List<string>();

	// Token: 0x040024E3 RID: 9443
	private List<string[]> _enemies = new List<string[]>();

	// Token: 0x040024E4 RID: 9444
	private GameObject[] _enemyCreationZones;

	// Token: 0x040024E5 RID: 9445
	public bool startGame;

	// Token: 0x040024E6 RID: 9446
	public double maxTimeGame = 240.0;

	// Token: 0x040024E7 RID: 9447
	public PhotonView photonView;

	// Token: 0x040024E8 RID: 9448
	public bool isPizzaMap;
}
