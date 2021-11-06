using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class BasePointController : MonoBehaviour
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600012A RID: 298 RVA: 0x0000C17C File Offset: 0x0000A37C
	// (set) Token: 0x0600012B RID: 299 RVA: 0x0000C184 File Offset: 0x0000A384
	public float captureCounter
	{
		get
		{
			return this._captureCounter;
		}
		set
		{
			this._captureCounter = value;
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000C190 File Offset: 0x0000A390
	private void Awake()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.timerToSynch = this.periodToSynch;
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
	private void OnDestroy()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
	private void Start()
	{
		this.rayPoint.SetColors(Color.gray, Color.gray);
		this.myLabelController = NickLabelStack.sharedStack.GetNextCurrentLabel();
		this.myLabelController.StartShow(NickLabelController.TypeNickLabel.Point, base.transform);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000C20C File Offset: 0x0000A40C
	private void Update()
	{
		if (this.isStartUpdateFromMasterClient && !PhotonNetwork.connected)
		{
			this.isStartUpdateFromMasterClient = false;
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		if (this.isBaseActive)
		{
			for (int i = 0; i < this.capturePlayers.Count; i++)
			{
				if (this.capturePlayers[i].myCommand == 1)
				{
					num++;
				}
				else if (this.capturePlayers[i].myCommand == 2)
				{
					num2++;
				}
				if (this.capturePlayers[i].Equals(WeaponManager.sharedManager.myPlayerMoveC))
				{
					flag = true;
				}
			}
			if (num2 == 0 && num > 0)
			{
				float num3 = (num != 1) ? ((num != 2) ? ((num != 3) ? 1.3f : 1.2f) : 1.1f) : 1f;
				this.captureCounter += Time.deltaTime * num3 * 100f / 5f;
				if (this.captureCounter > 100f)
				{
					this.captureCounter = 100f;
				}
			}
			if (num == 0 && num2 > 0)
			{
				float num4 = (num2 != 1) ? ((num2 != 2) ? ((num2 != 3) ? 1.3f : 1.2f) : 1.1f) : 1f;
				this.captureCounter -= Time.deltaTime * num4 * 100f / 5f;
				if (this.captureCounter < -100f)
				{
					this.captureCounter = -100f;
				}
			}
		}
		if (WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			Color color = (WeaponManager.sharedManager.myNetworkStartTable.myCommand != 0) ? (((this.captureCounter <= 0f || WeaponManager.sharedManager.myNetworkStartTable.myCommand != 1) && (this.captureCounter >= 0f || WeaponManager.sharedManager.myNetworkStartTable.myCommand != 2)) ? Color.red : Color.blue) : Color.gray;
			this.myLabelController.pointFillSprite.color = color;
		}
		this.myLabelController.pointFillSprite.fillAmount = Mathf.Abs(this.captureCounter) / 100f;
		if (this.captureCounter > 0f && this.captureConmmand == BasePointController.TypeCapture.red && !this.isSendMessageRaiderBlue)
		{
			this.isSendMessageRaiderBlue = true;
			this.isSendMessageRaiderRed = false;
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1783") + " " + this.nameBase);
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1784") + " " + this.nameBase);
				}
			}
		}
		if (this.captureCounter < 0f && this.captureConmmand == BasePointController.TypeCapture.blue && !this.isSendMessageRaiderRed)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1783") + " " + this.nameBase);
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1784") + " " + this.nameBase);
				}
			}
			this.isSendMessageRaiderBlue = false;
			this.isSendMessageRaiderRed = true;
		}
		if (this.captureCounter > 99.9f)
		{
			if (this.captureConmmand != BasePointController.TypeCapture.blue)
			{
				this.photonView.RPC("SinchCapture", PhotonTargets.Others, new object[]
				{
					1
				});
			}
			if (WeaponManager.sharedManager.myPlayerMoveC != null && !this.isSendMessageCaptureBlue)
			{
				this.isSendMessageCaptureRed = false;
				this.isSendMessageCaptureBlue = true;
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1781") + " " + this.nameBase);
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1785") + " " + this.nameBase);
				}
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1 && flag)
				{
					QuestMediator.NotifyCapture(ConnectSceneNGUIController.RegimGame.CapturePoints);
					if (num == 1)
					{
						WeaponManager.sharedManager.myPlayerMoveC.SendMySpotEvent();
					}
					else
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.teamCapturePoint, 1f);
					}
				}
			}
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				if (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 1)
				{
					this.rayPoint.SetColors(Color.blue, Color.blue);
				}
				else
				{
					this.rayPoint.SetColors(Color.red, Color.red);
				}
			}
			else
			{
				this.rayPoint.SetColors(Color.white, Color.white);
			}
			this.captureConmmand = BasePointController.TypeCapture.blue;
		}
		if (this.captureCounter < -99.9f)
		{
			if (this.captureConmmand != BasePointController.TypeCapture.red)
			{
				this.photonView.RPC("SinchCapture", PhotonTargets.Others, new object[]
				{
					2
				});
			}
			if (WeaponManager.sharedManager.myPlayerMoveC != null && !this.isSendMessageCaptureRed)
			{
				this.isSendMessageCaptureRed = true;
				this.isSendMessageCaptureBlue = false;
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1781") + " " + this.nameBase);
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1785") + " " + this.nameBase);
				}
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2 && flag)
				{
					QuestMediator.NotifyCapture(ConnectSceneNGUIController.RegimGame.CapturePoints);
					if (num2 == 1)
					{
						WeaponManager.sharedManager.myPlayerMoveC.SendMySpotEvent();
					}
					else
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.teamCapturePoint, 1f);
					}
				}
			}
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				if (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2)
				{
					this.rayPoint.SetColors(Color.blue, Color.blue);
				}
				else
				{
					this.rayPoint.SetColors(Color.red, Color.red);
				}
			}
			else
			{
				this.rayPoint.SetColors(Color.white, Color.white);
			}
			this.captureConmmand = BasePointController.TypeCapture.red;
		}
		if (this.myPlayerOnPoint && InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.pointCaptureBar.SetActive(this.isBaseActive);
			InGameGUI.sharedInGameGUI.pointCaptureName.text = this.nameBase;
			InGameGUI.sharedInGameGUI.captureBarSprite.fillAmount = Mathf.Abs(this.captureCounter) / 100f;
			bool flag2 = (this.captureCounter > 0f && WeaponManager.sharedManager.myNetworkStartTable.myCommand == 1) || (this.captureCounter < 0f && WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2);
			InGameGUI.sharedInGameGUI.captureBarSprite.color = ((!flag2) ? this.redTeamColor : this.blueTeamColor);
			InGameGUI.sharedInGameGUI.teamColorSprite.color = ((!flag2) ? this.redCaptureColor : this.blueCaptureColor);
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.timerToSynch -= Time.deltaTime;
			if (this.timerToSynch <= 0f)
			{
				this.timerToSynch = this.periodToSynch;
				this.photonView.RPC("SynchCaptureCounter", PhotonTargets.Others, new object[]
				{
					this.captureCounter
				});
			}
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000CAA0 File Offset: 0x0000ACA0
	private void MyPlayerEnterPoint()
	{
		this.myPlayerOnPoint = true;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.pointCaptureBar.SetActive(this.isBaseActive);
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000CADC File Offset: 0x0000ACDC
	private void MyPlayerLeavePoint()
	{
		this.myPlayerOnPoint = false;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.pointCaptureBar.SetActive(false);
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000CB08 File Offset: 0x0000AD08
	private void AddPlayerInList(GameObject _player)
	{
		Player_move_c playerMoveC = _player.GetComponent<SkinName>().playerMoveC;
		if (!this.capturePlayers.Contains(playerMoveC))
		{
			this.capturePlayers.Add(playerMoveC);
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000CB40 File Offset: 0x0000AD40
	private void RemoveFromList(GameObject _player)
	{
		Player_move_c playerMoveC = _player.GetComponent<SkinName>().playerMoveC;
		if (this.capturePlayers.Contains(playerMoveC))
		{
			this.capturePlayers.Remove(playerMoveC);
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000CB78 File Offset: 0x0000AD78
	[PunRPC]
	[RPC]
	public void SinchCapture(int command)
	{
		if (command == 1)
		{
			this.captureCounter = 200f;
			this.captureConmmand = BasePointController.TypeCapture.blue;
		}
		else
		{
			this.captureCounter = -200f;
			this.captureConmmand = BasePointController.TypeCapture.red;
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
	[PunRPC]
	[RPC]
	private void AddPlayerInCapturePoint(int _viewId, float _time)
	{
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (Initializer.players[i].photonView.ownerId == _viewId)
			{
				if (!this.capturePlayers.Contains(Initializer.players[i]))
				{
					this.capturePlayers.Add(Initializer.players[i]);
					if (this.isBaseActive && PhotonNetwork.time > (double)_time)
					{
						int num = 0;
						int num2 = 0;
						for (int j = 0; j < this.capturePlayers.Count; j++)
						{
							if (this.capturePlayers[j].myCommand == 1)
							{
								num++;
							}
							else if (this.capturePlayers[j].myCommand == 2)
							{
								num2++;
							}
						}
						if (num2 == 0 && num == 1)
						{
							this.captureCounter += ((float)PhotonNetwork.time - _time) * 100f / 5f;
							if (this.captureCounter > 100f)
							{
								this.captureCounter = 100f;
							}
						}
						if (num == 0 && num2 == 1)
						{
							this.captureCounter -= ((float)PhotonNetwork.time - _time) * 100f / 5f;
							if (this.captureCounter < -100f)
							{
								this.captureCounter = -100f;
							}
						}
					}
				}
				break;
			}
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000CD38 File Offset: 0x0000AF38
	[PunRPC]
	[RPC]
	private void RemovePlayerInCapturePoint(int _viewId, float _time)
	{
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (Initializer.players[i].photonView.ownerId == _viewId)
			{
				if (this.capturePlayers.Contains(Initializer.players[i]))
				{
					this.capturePlayers.Remove(Initializer.players[i]);
				}
				break;
			}
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000CDB4 File Offset: 0x0000AFB4
	public void OnEndMatch()
	{
		this.captureCounter = 0f;
		this.capturePlayers.Clear();
		this.rayPoint.SetColors(Color.gray, Color.gray);
		this.captureConmmand = BasePointController.TypeCapture.none;
		this.isSendMessageCaptureRed = false;
		this.isSendMessageCaptureBlue = false;
		this.isSendMessageRaiderBlue = false;
		this.isSendMessageRaiderRed = false;
		this.MyPlayerLeavePoint();
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000CE18 File Offset: 0x0000B018
	private void OnTriggerEnter(Collider other)
	{
		if (!Defs.isMulti || !Defs.isCapturePoints)
		{
			return;
		}
		if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Player"))
		{
			this.AddPlayerInList(other.transform.parent.gameObject);
			if (other.transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
			{
				this.MyPlayerEnterPoint();
				this.photonView.RPC("AddPlayerInCapturePoint", PhotonTargets.Others, new object[]
				{
					WeaponManager.sharedManager.myPlayerMoveC.photonView.ownerId,
					(float)PhotonNetwork.time
				});
			}
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
	private void OnTriggerExit(Collider other)
	{
		if (!Defs.isMulti || !Defs.isCapturePoints)
		{
			return;
		}
		if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Player"))
		{
			this.RemoveFromList(other.transform.parent.gameObject);
			if (other.transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
			{
				this.MyPlayerLeavePoint();
				this.photonView.RPC("RemovePlayerInCapturePoint", PhotonTargets.Others, new object[]
				{
					WeaponManager.sharedManager.myPlayerMoveC.photonView.ownerId,
					(float)PhotonNetwork.time
				});
			}
		}
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000CFC8 File Offset: 0x0000B1C8
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (!Defs.isMulti || !Defs.isCapturePoints)
		{
			return;
		}
		this.photonView.RPC("SynchCaptureCounterNewPlayer", player, new object[]
		{
			player.ID,
			PhotonNetwork.isMasterClient,
			this.captureCounter,
			(int)this.captureConmmand
		});
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000D038 File Offset: 0x0000B238
	[PunRPC]
	[RPC]
	public void SynchCaptureCounterNewPlayer(int _viewId, bool isMaster, float _captureCounter, int _captureCommand)
	{
		if (this.isStartUpdateFromMasterClient || PhotonNetwork.player.ID != _viewId)
		{
			return;
		}
		this.SynchCaptureCounter(this.captureCounter);
		this.captureConmmand = (BasePointController.TypeCapture)_captureCommand;
		this.isStartUpdateFromMasterClient = isMaster;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000D07C File Offset: 0x0000B27C
	[RPC]
	[PunRPC]
	private void SynchCaptureCounter(float _captureCounter)
	{
		this.captureCounter = _captureCounter;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000D088 File Offset: 0x0000B288
	public void OnDisconnectedFromPhoton()
	{
		this.isStartUpdateFromMasterClient = false;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000D094 File Offset: 0x0000B294
	private void OnFailedToConnectToPhoton(object parameters)
	{
		this.OnEndMatch();
	}

	// Token: 0x04000119 RID: 281
	private const float timeCaptureCounter = 5f;

	// Token: 0x0400011A RID: 282
	private const float maxCaptureCounter = 100f;

	// Token: 0x0400011B RID: 283
	public string nameBase;

	// Token: 0x0400011C RID: 284
	private float _captureCounter;

	// Token: 0x0400011D RID: 285
	private Color redTeamColor = Color.red;

	// Token: 0x0400011E RID: 286
	private Color blueTeamColor = Color.blue;

	// Token: 0x0400011F RID: 287
	private Color redCaptureColor = new Color32(212, 0, 0, 130);

	// Token: 0x04000120 RID: 288
	private Color blueCaptureColor = new Color32(0, 0, 225, 130);

	// Token: 0x04000121 RID: 289
	public NickLabelController myLabelController;

	// Token: 0x04000122 RID: 290
	public PhotonView photonView;

	// Token: 0x04000123 RID: 291
	private bool isStartUpdateFromMasterClient;

	// Token: 0x04000124 RID: 292
	private float periodToSynch = 1f;

	// Token: 0x04000125 RID: 293
	private float timerToSynch;

	// Token: 0x04000126 RID: 294
	public List<Player_move_c> capturePlayers = new List<Player_move_c>();

	// Token: 0x04000127 RID: 295
	public bool isBaseActive = true;

	// Token: 0x04000128 RID: 296
	private bool myPlayerOnPoint;

	// Token: 0x04000129 RID: 297
	public GameObject baseRender;

	// Token: 0x0400012A RID: 298
	public LineRenderer rayPoint;

	// Token: 0x0400012B RID: 299
	private bool isSendMessageCaptureBlue;

	// Token: 0x0400012C RID: 300
	private bool isSendMessageCaptureRed;

	// Token: 0x0400012D RID: 301
	private bool isSendMessageRaiderBlue;

	// Token: 0x0400012E RID: 302
	private bool isSendMessageRaiderRed;

	// Token: 0x0400012F RID: 303
	private bool sendScoreEventBlue;

	// Token: 0x04000130 RID: 304
	private bool sendScoreEventRed;

	// Token: 0x04000131 RID: 305
	public BasePointController.TypeCapture captureConmmand;

	// Token: 0x0200002D RID: 45
	public enum TypeCapture
	{
		// Token: 0x04000133 RID: 307
		none,
		// Token: 0x04000134 RID: 308
		blue,
		// Token: 0x04000135 RID: 309
		red
	}
}
