using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class CapturePointController : MonoBehaviour
{
	// Token: 0x06000243 RID: 579 RVA: 0x000142C4 File Offset: 0x000124C4
	private void Start()
	{
		if (!Defs.isCapturePoints || !Defs.isMulti)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (PhotonNetwork.room != null)
		{
			int num = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty];
			if (num == 3)
			{
				this.speedScoreAdd = 3f;
			}
			if (num == 5)
			{
				this.speedScoreAdd = 2f;
			}
			if (num == 7)
			{
				this.speedScoreAdd = 1.5f;
			}
		}
		CapturePointController.sharedController = this;
		this.myTransform = base.transform;
		this.basePointControllers = new BasePointController[this.myTransform.childCount];
		for (int i = 0; i < this.myTransform.childCount; i++)
		{
			this.basePointControllers[i] = this.myTransform.GetChild(i).GetComponent<BasePointController>();
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x000143A8 File Offset: 0x000125A8
	private void Update()
	{
		if (this.isStartUpdateFromMasterClient && !PhotonNetwork.connected)
		{
			this.isStartUpdateFromMasterClient = false;
		}
		if (Initializer.bluePlayers.Count == 0 || Initializer.redPlayers.Count == 0)
		{
			if (InGameGUI.sharedInGameGUI != null && !InGameGUI.sharedInGameGUI.message_wait.activeSelf)
			{
				InGameGUI.sharedInGameGUI.message_wait.SetActive(true);
			}
			for (int i = 0; i < this.basePointControllers.Length; i++)
			{
				this.basePointControllers[i].isBaseActive = false;
				if (this.basePointControllers[i].baseRender != null && this.basePointControllers[i].baseRender.activeSelf)
				{
					this.basePointControllers[i].baseRender.SetActive(false);
				}
			}
			return;
		}
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.message_wait.activeSelf)
		{
			InGameGUI.sharedInGameGUI.message_wait.SetActive(false);
		}
		for (int j = 0; j < this.basePointControllers.Length; j++)
		{
			this.basePointControllers[j].isBaseActive = true;
			if (this.basePointControllers[j].baseRender != null && !this.basePointControllers[j].baseRender.activeSelf)
			{
				this.basePointControllers[j].baseRender.SetActive(true);
			}
		}
		int num = 0;
		int num2 = 0;
		for (int k = 0; k < this.basePointControllers.Length; k++)
		{
			if (this.basePointControllers[k].captureConmmand == BasePointController.TypeCapture.blue)
			{
				num++;
			}
			if (this.basePointControllers[k].captureConmmand == BasePointController.TypeCapture.red)
			{
				num2++;
			}
		}
		this.scoreBlue += Time.deltaTime * this.speedScoreAdd * (float)num;
		this.scoreRed += Time.deltaTime * this.speedScoreAdd * (float)num2;
		if (this.scoreBlue > this.maxScoreCommands)
		{
			this.scoreBlue = this.maxScoreCommands;
			this.EndMatch();
		}
		if (this.scoreRed > this.maxScoreCommands)
		{
			this.scoreRed = this.maxScoreCommands;
			this.EndMatch();
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.timerToSynch -= Time.deltaTime;
			if (this.timerToSynch <= 0f)
			{
				this.timerToSynch = this.periodToSynch;
				this.photonView.RPC("SynchScoresCommandsRPC", PhotonTargets.All, new object[]
				{
					this.scoreBlue,
					this.scoreRed
				});
			}
		}
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00014664 File Offset: 0x00012864
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		this.photonView.RPC("SynchScoresCommandsNewPlayerRPC", player, new object[]
		{
			player.ID,
			PhotonNetwork.isMasterClient,
			this.scoreBlue,
			this.scoreRed
		});
	}

	// Token: 0x06000246 RID: 582 RVA: 0x000146C0 File Offset: 0x000128C0
	[PunRPC]
	[RPC]
	public void SynchScoresCommandsNewPlayerRPC(int _viewId, bool isMaster, float _scoreBlue, float _scoreRed)
	{
		if (this.isStartUpdateFromMasterClient || PhotonNetwork.player.ID != _viewId)
		{
			return;
		}
		this.SynchScoresCommandsRPC(_scoreBlue, _scoreRed);
		this.isStartUpdateFromMasterClient = isMaster;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000146FC File Offset: 0x000128FC
	[PunRPC]
	[RPC]
	public void SynchScoresCommandsRPC(float _scoreBlue, float _scoreRed)
	{
		this.scoreBlue = _scoreBlue;
		this.scoreRed = _scoreRed;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0001470C File Offset: 0x0001290C
	public void EndMatch()
	{
		Debug.Log("EndMatch");
		int commandWin = 0;
		if (this.scoreBlue > this.scoreRed)
		{
			commandWin = 1;
		}
		if (this.scoreRed > this.scoreBlue)
		{
			commandWin = 2;
		}
		if (!TimeGameController.sharedController.isEndMatch && !this.isEndMatch)
		{
			this.isEndMatch = true;
			if (WeaponManager.sharedManager.myTable != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, commandWin, Mathf.RoundToInt(this.scoreBlue), Mathf.RoundToInt(this.scoreRed));
			}
		}
		else
		{
			Debug.Log("EndMatch in table!");
		}
		this.scoreRed = 0f;
		this.scoreBlue = 0f;
		for (int i = 0; i < this.basePointControllers.Length; i++)
		{
			this.basePointControllers[i].OnEndMatch();
		}
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0001480C File Offset: 0x00012A0C
	private void Awake()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0001481C File Offset: 0x00012A1C
	private void OnDestroy()
	{
		CapturePointController.sharedController = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x04000278 RID: 632
	public static CapturePointController sharedController;

	// Token: 0x04000279 RID: 633
	public PhotonView photonView;

	// Token: 0x0400027A RID: 634
	public float scoreBlue;

	// Token: 0x0400027B RID: 635
	public float scoreRed;

	// Token: 0x0400027C RID: 636
	private float speedScoreAdd = 3f;

	// Token: 0x0400027D RID: 637
	private float maxScoreCommands = 1000f;

	// Token: 0x0400027E RID: 638
	private bool isStartUpdateFromMasterClient;

	// Token: 0x0400027F RID: 639
	private float periodToSynch = 1f;

	// Token: 0x04000280 RID: 640
	private float timerToSynch;

	// Token: 0x04000281 RID: 641
	public BasePointController[] basePointControllers;

	// Token: 0x04000282 RID: 642
	private Transform myTransform;

	// Token: 0x04000283 RID: 643
	public bool isEndMatch;
}
