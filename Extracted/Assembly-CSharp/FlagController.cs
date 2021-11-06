using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class FlagController : MonoBehaviour
{
	// Token: 0x06000829 RID: 2089 RVA: 0x00031444 File Offset: 0x0002F644
	private void Awake()
	{
		GameObject original = Resources.Load("FlagPedestal") as GameObject;
		if (this.isBlue)
		{
			this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
			Initializer.flag1 = this;
		}
		else
		{
			this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
			Initializer.flag2 = this;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(original, this.myBaza.transform.position, this.myBaza.transform.rotation) as GameObject;
		this.pedistal = gameObject.GetComponent<FlagPedestalController>();
		this._objBazaTexture = (UnityEngine.Object.Instantiate(Resources.Load("ObjectPictFlag") as GameObject, this.myBaza.transform.position, this.myBaza.transform.rotation) as GameObject);
		this._objFlagTexture = (UnityEngine.Object.Instantiate(Resources.Load("ObjectPictFlag") as GameObject, this.myBaza.transform.position, this.myBaza.transform.rotation) as GameObject);
		this._objBazaTexture.GetComponent<ObjectPictFlag>().target = gameObject.transform.GetChild(0);
		this._objBazaTexture.GetComponent<ObjectPictFlag>().isBaza = true;
		this._objBazaTexture.GetComponent<ObjectPictFlag>().myFlagController = this;
		this._objFlagTexture.GetComponent<ObjectPictFlag>().target = this.pointObjTexture.transform;
		this.SetColor(0);
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x000315C0 File Offset: 0x0002F7C0
	private void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.photonView.RPC("SetMasterSeverIDRPC", PhotonTargets.AllBuffered, new object[]
		{
			this.photonView.viewID
		});
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00031604 File Offset: 0x0002F804
	public void SetColor(int _color)
	{
		if (_color == this.currentColor)
		{
			return;
		}
		this.currentColor = _color;
		this.pedistal.SetColor(_color);
		this.flagModelRed.SetActive(_color == 2);
		this.flagModelBlue.SetActive(_color == 1);
		this.rayRed.SetActive(_color == 2);
		this.rayBlue.SetActive(_color == 1);
		if (_color > 0)
		{
			this._objBazaTexture.GetComponent<ObjectPictFlag>().SetTexture(Resources.Load((_color != 1) ? "red_base" : "blue_base") as Texture2D);
			this._objFlagTexture.GetComponent<ObjectPictFlag>().SetTexture(Resources.Load((_color != 1) ? "red_flag" : "blue_flag") as Texture2D);
		}
		else
		{
			this._objBazaTexture.GetComponent<ObjectPictFlag>().SetTexture(null);
			this._objFlagTexture.GetComponent<ObjectPictFlag>().SetTexture(null);
		}
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000316FC File Offset: 0x0002F8FC
	private void Update()
	{
		if (this.inGameGui == null)
		{
			this.inGameGui = InGameGUI.sharedInGameGUI;
		}
		this.SetColor((!(WeaponManager.sharedManager.myPlayerMoveC == null)) ? (((WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1 || !this.isBlue) && (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2 || this.isBlue)) ? 2 : 1) : 0);
		if (this.rayBlue.activeInHierarchy == this.isCapture)
		{
			this.rayBlue.SetActive(!this.isCapture);
		}
		if (this.rayRed.activeInHierarchy == this.isCapture)
		{
			this.rayRed.SetActive(!this.isCapture);
		}
		if (this.targetTrasform != null)
		{
			base.transform.position = this.targetTrasform.position;
			base.transform.rotation = this.targetTrasform.rotation;
		}
		else
		{
			this.isCapture = false;
		}
		int num = 0;
		int num2 = 0;
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player_move_c != null)
			{
				int myCommand = player_move_c.myCommand;
				if (myCommand == 1)
				{
					num++;
				}
				if (myCommand == 2)
				{
					num2++;
				}
			}
		}
		if ((num == 0 || num2 == 0) && this.flagModel.activeSelf)
		{
			this.flagModel.SetActive(false);
		}
		if (this.inGameGui != null && (num == 0 || num2 == 0) && !this.inGameGui.message_wait.activeSelf)
		{
			this.inGameGui.message_wait.SetActive(true);
			this.inGameGui.timerShowNow = 0f;
		}
		if (this.inGameGui != null && num != 0 && num2 != 0 && this.inGameGui.message_wait.activeSelf)
		{
			this.inGameGui.message_wait.SetActive(false);
			this.inGameGui.timerShowNow = 3f;
		}
		if (num != 0 && num2 != 0 && !this.flagModel.activeSelf)
		{
			this.flagModel.SetActive(true);
		}
		if ((num == 0 || num2 == 0) && this.isCapture)
		{
			foreach (Player_move_c player_move_c2 in Initializer.players)
			{
				if (this.idCapturePlayer == player_move_c2.mySkinName.photonView.ownerId)
				{
					player_move_c2.isCaptureFlag = false;
				}
				this.GoBaza();
			}
		}
		if (PhotonNetwork.isMasterClient && !this.isCapture && !this.isBaza)
		{
			this.timerToBaza -= Time.deltaTime;
			if (this.timerToBaza < 0f)
			{
				this.GoBaza();
				if (WeaponManager.sharedManager.myPlayer != null)
				{
					WeaponManager.sharedManager.myPlayerMoveC.SendSystemMessegeFromFlagReturned(this.isBlue);
				}
			}
		}
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00031AA4 File Offset: 0x0002FCA4
	public void GoBaza()
	{
		this.timerToBaza = this.maxTimerToBaza;
		this.photonView.RPC("GoBazaRPC", PhotonTargets.All, new object[0]);
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00031ACC File Offset: 0x0002FCCC
	[PunRPC]
	[RPC]
	public void GoBazaRPC()
	{
		Debug.Log("GoBazaRPC");
		this.isBaza = true;
		this.isCapture = false;
		this.idCapturePlayer = -1;
		this.targetTrasform = null;
		base.transform.position = this.myBaza.transform.position;
		base.transform.rotation = this.myBaza.transform.rotation;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00031B38 File Offset: 0x0002FD38
	public void SetCapture(int _viewIdCapture)
	{
		this.photonView.RPC("SetCaptureRPC", PhotonTargets.All, new object[]
		{
			_viewIdCapture
		});
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00031B68 File Offset: 0x0002FD68
	[RPC]
	[PunRPC]
	public void SetCaptureRPC(int _viewIdCapture)
	{
		this.isBaza = false;
		this.idCapturePlayer = _viewIdCapture;
		this.isCapture = true;
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player_move_c.mySkinName.photonView.ownerId == _viewIdCapture)
			{
				this.targetTrasform = player_move_c.flagPoint.transform;
				player_move_c.isCaptureFlag = true;
			}
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00031C0C File Offset: 0x0002FE0C
	public void SetNOCapture(Vector3 pos, Quaternion rot)
	{
		this.photonView.RPC("SetNOCaptureRPC", PhotonTargets.All, new object[]
		{
			pos,
			rot
		});
		this.timerToBaza = this.maxTimerToBaza;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00031C44 File Offset: 0x0002FE44
	[RPC]
	[PunRPC]
	public void SetNOCaptureRPC(Vector3 pos, Quaternion rot)
	{
		this.isCapture = false;
		this.idCapturePlayer = -1;
		if (this.targetTrasform != null)
		{
			this.targetTrasform.parent.GetComponent<SkinName>().playerMoveC.isCaptureFlag = false;
		}
		this.targetTrasform = null;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00031C94 File Offset: 0x0002FE94
	[PunRPC]
	[RPC]
	public void SetNOCaptureRPCNewPlayer(int idNewPlayer, Vector3 pos, Quaternion rot, bool _isBaza)
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
		if (this.photonView != null && this.photonView.ownerId == idNewPlayer)
		{
			this.isBaza = _isBaza;
			this.SetNOCaptureRPC(pos, rot);
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00031CF0 File Offset: 0x0002FEF0
	[PunRPC]
	[RPC]
	public void SetCaptureRPCNewPlayer(int idNewPlayer, int _viewIdCapture)
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
		if (PhotonNetwork.player.ID == idNewPlayer)
		{
			this.SetCaptureRPC(_viewIdCapture);
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00031D34 File Offset: 0x0002FF34
	[RPC]
	[PunRPC]
	public void SetMasterSeverIDRPC(int _id)
	{
		this.masterServerID = _id;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00031D40 File Offset: 0x0002FF40
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView == null)
		{
			Debug.Log("FlagController.OnPhotonPlayerConnected():    photonView == null");
			return;
		}
		if (this.isCapture)
		{
			this.photonView.RPC("SetCaptureRPCNewPlayer", player, new object[]
			{
				player.ID,
				this.idCapturePlayer
			});
		}
		else
		{
			this.photonView.RPC("SetNOCaptureRPCNewPlayer", player, new object[]
			{
				player.ID,
				base.transform.position,
				base.transform.rotation,
				this.isBaza
			});
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00031E08 File Offset: 0x00030008
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this._objBazaTexture);
		UnityEngine.Object.Destroy(this._objFlagTexture);
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x040006C1 RID: 1729
	public bool isBlue;

	// Token: 0x040006C2 RID: 1730
	public int masterServerID;

	// Token: 0x040006C3 RID: 1731
	private PhotonView photonView;

	// Token: 0x040006C4 RID: 1732
	public bool isCapture;

	// Token: 0x040006C5 RID: 1733
	private int idCapturePlayer;

	// Token: 0x040006C6 RID: 1734
	public bool isBaza;

	// Token: 0x040006C7 RID: 1735
	private GameObject myBaza;

	// Token: 0x040006C8 RID: 1736
	public GameObject rayBlue;

	// Token: 0x040006C9 RID: 1737
	public GameObject rayRed;

	// Token: 0x040006CA RID: 1738
	public float timerToBaza = 10f;

	// Token: 0x040006CB RID: 1739
	private float maxTimerToBaza = 10f;

	// Token: 0x040006CC RID: 1740
	public GameObject flagModelRed;

	// Token: 0x040006CD RID: 1741
	public GameObject flagModelBlue;

	// Token: 0x040006CE RID: 1742
	public Transform targetTrasform;

	// Token: 0x040006CF RID: 1743
	private InGameGUI inGameGui;

	// Token: 0x040006D0 RID: 1744
	public GameObject pointObjTexture;

	// Token: 0x040006D1 RID: 1745
	private GameObject _objBazaTexture;

	// Token: 0x040006D2 RID: 1746
	private GameObject _objFlagTexture;

	// Token: 0x040006D3 RID: 1747
	public GameObject flagModel;

	// Token: 0x040006D4 RID: 1748
	private FlagPedestalController pedistal;

	// Token: 0x040006D5 RID: 1749
	private int currentColor = -1;
}
