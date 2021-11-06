using System;
using Photon;
using UnityEngine;

// Token: 0x020002AD RID: 685
internal sealed class HungerGameController : Photon.MonoBehaviour
{
	// Token: 0x17000264 RID: 612
	// (get) Token: 0x0600156A RID: 5482 RVA: 0x000558C0 File Offset: 0x00053AC0
	public static HungerGameController Instance
	{
		get
		{
			return HungerGameController._instance;
		}
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x000558C8 File Offset: 0x00053AC8
	private void Start()
	{
		this.maxCountPlayers = PhotonNetwork.room.maxPlayers;
		this.gameTimer = (float)(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString()) * 60);
		HungerGameController._instance = this;
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x00055914 File Offset: 0x00053B14
	private void OnDestroy()
	{
		HungerGameController._instance = null;
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x0005591C File Offset: 0x00053B1C
	private void Update()
	{
		if (this.isStartTimer && this.startTimer > 0f)
		{
			this.startTimer -= Time.deltaTime;
		}
		if (this.isStartGame && this.goTimer > 0f)
		{
			this.goTimer -= Time.deltaTime;
		}
		if (this.goTimer < 0f)
		{
			this.goTimer = 0f;
		}
		if (this.isShowGo && this.timerShowGo >= 0f)
		{
			this.timerShowGo -= Time.deltaTime;
		}
		if (this.isShowGo && this.timerShowGo < 0f)
		{
			this.isShowGo = false;
		}
		if (this.isGo && this.gameTimer > 0f && Initializer.players.Count > 0)
		{
			this.gameTimer -= Time.deltaTime;
		}
		if (base.photonView.isMine)
		{
			if (this.gameTimer <= 0f && !this.theEnd)
			{
				this.theEnd = true;
				base.photonView.RPC("Draw", PhotonTargets.AllBuffered, new object[0]);
			}
			this.timeToSynchTimer -= Time.deltaTime;
			if (this.isGo && this.timeToSynchTimer < 0f)
			{
				this.timeToSynchTimer = 0.5f;
				base.photonView.RPC("SynchGameTimer", PhotonTargets.Others, new object[]
				{
					this.gameTimer
				});
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
			if (!this.isStartGame)
			{
				if (!this.isStartTimer && array.Length >= this.minCountPlayer)
				{
					base.photonView.RPC("StartTimer", PhotonTargets.AllBuffered, new object[]
					{
						true
					});
				}
				if (this.timeToSynchTimer < 0f)
				{
					this.timeToSynchTimer = 0.5f;
					base.photonView.RPC("SynchStartTimer", PhotonTargets.Others, new object[]
					{
						this.startTimer
					});
				}
				if ((!this.isStartGame && this.isStartTimer && this.startTimer < 0.1f && array.Length >= this.minCountPlayer) || (!this.isStartGame && this.isStartTimer && array.Length == PhotonNetwork.room.maxPlayers))
				{
					base.photonView.RPC("StartGame", PhotonTargets.AllBuffered, new object[0]);
					PhotonNetwork.room.visible = false;
				}
			}
			else
			{
				if (this.timeToSynchTimer < 0f)
				{
					this.timeToSynchTimer = 0.5f;
					base.photonView.RPC("SynchTimerGo", PhotonTargets.Others, new object[]
					{
						this.goTimer
					});
				}
				if (!this.isGo && this.goTimer < 0.1f)
				{
					base.photonView.RPC("Go", PhotonTargets.AllBuffered, new object[0]);
				}
			}
		}
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x00055C54 File Offset: 0x00053E54
	[PunRPC]
	[RPC]
	private void Draw()
	{
		Debug.Log("Draw!!!");
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		if (myNetworkStartTable != null)
		{
			base.StartCoroutine(myNetworkStartTable.DrawInHanger());
		}
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x00055C90 File Offset: 0x00053E90
	[RPC]
	[PunRPC]
	private void StartTimer(bool _isStartTimer)
	{
		this.isStartTimer = _isStartTimer;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00055C9C File Offset: 0x00053E9C
	[PunRPC]
	[RPC]
	private void SynchStartTimer(float _startTimer)
	{
		this.startTimer = _startTimer;
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x00055CA8 File Offset: 0x00053EA8
	[RPC]
	[PunRPC]
	private void SynchTimerGo(float _goTimer)
	{
		this.goTimer = _goTimer;
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x00055CB4 File Offset: 0x00053EB4
	[PunRPC]
	[RPC]
	private void SynchGameTimer(float _gameTimer)
	{
		this.gameTimer = _gameTimer;
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x00055CC0 File Offset: 0x00053EC0
	[RPC]
	[PunRPC]
	private void StartGame()
	{
		this.isStartGame = true;
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x00055CCC File Offset: 0x00053ECC
	[PunRPC]
	[RPC]
	private void Go()
	{
		this.isGo = true;
		this.isShowGo = true;
	}

	// Token: 0x04000CBE RID: 3262
	public bool isStartGame;

	// Token: 0x04000CBF RID: 3263
	public bool isStartTimer;

	// Token: 0x04000CC0 RID: 3264
	public float startTimer = 30f;

	// Token: 0x04000CC1 RID: 3265
	public int countPlayers;

	// Token: 0x04000CC2 RID: 3266
	public int maxCountPlayers = 10;

	// Token: 0x04000CC3 RID: 3267
	public bool isRunPlayer;

	// Token: 0x04000CC4 RID: 3268
	public float goTimer = 10.5f;

	// Token: 0x04000CC5 RID: 3269
	public bool isGo;

	// Token: 0x04000CC6 RID: 3270
	private float timeToSynchTimer = 2f;

	// Token: 0x04000CC7 RID: 3271
	public int minCountPlayer = 2;

	// Token: 0x04000CC8 RID: 3272
	public bool isShowGo;

	// Token: 0x04000CC9 RID: 3273
	private float timerShowGo = 1f;

	// Token: 0x04000CCA RID: 3274
	public float gameTimer = 600f;

	// Token: 0x04000CCB RID: 3275
	public bool theEnd;

	// Token: 0x04000CCC RID: 3276
	private static HungerGameController _instance;
}
