using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020003CD RID: 973
public sealed class NickLabelController : MonoBehaviour
{
	// Token: 0x06002353 RID: 9043 RVA: 0x000AF7BC File Offset: 0x000AD9BC
	private void Awake()
	{
		this.thisTransform = base.transform;
		this.HideLabel();
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000AF7D0 File Offset: 0x000AD9D0
	public void StartShow(NickLabelController.TypeNickLabel _type, Transform _target)
	{
		this.thisTransform = base.transform;
		for (int i = 0; i < this.thisTransform.childCount; i++)
		{
			this.thisTransform.GetChild(i).gameObject.SetActive(false);
		}
		this.currentType = _type;
		this.target = _target;
		base.gameObject.SetActive(true);
		this.placeMarker.SetActive(false);
		this.nickLabel.color = Color.white;
		this.healthSprite.enabled = true;
		this.offset = new Vector3(0f, 0.6f, 0f);
		float num = 1f;
		this.SetCommandColor(0);
		if (this.currentType == NickLabelController.TypeNickLabel.Player)
		{
			this.nickLabel.gameObject.SetActive(true);
			this.playerScript = this.target.GetComponent<Player_move_c>();
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			num = 1f;
			this.expFrameLobby.SetActive(true);
			this.nickLabel.gameObject.SetActive(true);
			this.clanName.gameObject.SetActive(true);
			this.clanTexture.gameObject.SetActive(true);
			this.offset = new Vector3(0f, 2.26f, 0f);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Point)
		{
			this.pointScript = this.target.GetComponent<BasePointController>();
			this.pointSprite.spriteName = "Point_" + this.pointScript.nameBase;
			this.pointFillSprite.spriteName = this.pointSprite.spriteName;
			this.pointSprite.gameObject.SetActive(true);
			this.offset = new Vector3(0f, 2.25f, 0f);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Turret)
		{
			this.turretScript = this.target.GetComponent<TurretController>();
			this.playerScript = ((!(this.turretScript.myPlayer != null)) ? null : this.turretScript.myPlayer.GetComponent<SkinName>().playerMoveC);
			this.nickLabel.gameObject.SetActive(true);
			if (!Defs.isDaterRegim)
			{
				this.healthObj.SetActive(true);
			}
			this.offset = new Vector3(0f, 1.76f, 0f);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.FreeCoins)
		{
			this.thisTransform.localScale = new Vector3(num, num, num);
			this.freeAwardTitle.gameObject.SetActive(true);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.GetGift)
		{
			this.GiftLabelPos = this.GiftObject.transform.localPosition;
			this.thisTransform.localScale = new Vector3(num, num, num);
			this.GiftObject.SetActiveSafe(true);
			CoroutineRunner.Instance.StartCoroutine(this.UpdateGachaNickLabel());
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Nest)
		{
			this.NestLabelPos = this.NestGO.transform.localPosition;
			this.thisTransform.localScale = new Vector3(num, num, num);
			this.NestGO.SetActive(true);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.InAppBonus)
		{
			this.InAppBonusLabelPos = this.NestGO.transform.localPosition;
			this.thisTransform.localScale = new Vector3(num, num, num);
			this.InAppBonusGO.SetActive(true);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Leprechaunt)
		{
			this.thisTransform.localScale = new Vector3(num, num, num);
			this.LeprechauntGO.SetActive(true);
		}
		this.thisTransform.localScale = new Vector3(num, num, num);
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			this.UpdateNickInLobby();
		}
		this.UpdateInfo();
		this.HideLabel();
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000AFB94 File Offset: 0x000ADD94
	private IEnumerator UpdateGachaNickLabel()
	{
		UIPanel panel = base.GetComponent<UIPanel>();
		for (;;)
		{
			if (this.GiftObject.activeInHierarchy && GiftController.Instance != null)
			{
				if (GiftController.Instance.CanGetTimerGift)
				{
					this.GiftObject.transform.localPosition = this.GiftLabelPosWithoutTimer;
					this.GiftTimerObject.SetActiveSafe(false);
				}
				else
				{
					this.GiftObject.transform.localPosition = this.GiftLabelPos;
					string timeStr = RiliExtensions.GetTimeString((long)GiftController.Instance.TimeLeft, ":");
					if (timeStr.IsNullOrEmpty())
					{
						this.GiftTimerObject.SetActiveSafe(false);
						this.GiftObject.transform.localPosition = this.GiftLabelPosWithoutTimer;
						panel.SetDirty();
						panel.Refresh();
					}
					else
					{
						this.GiftTimerObject.SetActiveSafe(true);
						this.GiftTimerLabel.text = timeStr;
					}
				}
				panel.SetDirty();
				panel.Refresh();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000AFBB0 File Offset: 0x000ADDB0
	public void UpdateInfo()
	{
		if (this.currentType == NickLabelController.TypeNickLabel.Player)
		{
			this.nickLabel.text = FilterBadWorld.FilterString(this.playerScript.mySkinName.NickName);
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Turret && this.playerScript != null)
		{
			if (Defs.isMulti)
			{
				this.nickLabel.text = FilterBadWorld.FilterString(this.playerScript.mySkinName.NickName);
			}
			else
			{
				this.nickLabel.text = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
			{
				this.SetCommandColor((!(WeaponManager.sharedManager.myPlayerMoveC == null)) ? ((WeaponManager.sharedManager.myPlayerMoveC.myCommand != this.playerScript.myCommand) ? 2 : 1) : 0);
			}
			else
			{
				this.SetCommandColor((!this.playerScript.Equals(WeaponManager.sharedManager.myPlayerMoveC) && !Defs.isDaterRegim && !Defs.isCOOP) ? 2 : 0);
			}
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
		{
			this.nickLabel.text = this.myLobbyNickname;
			this.clanName.text = FriendsController.sharedController.clanName;
			if (!string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				if (this.clanTexture.mainTexture == null)
				{
					byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
					Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
					texture2D.LoadImage(data);
					texture2D.filterMode = FilterMode.Point;
					texture2D.Apply();
					this.clanTexture.mainTexture = texture2D;
					Transform transform = this.clanTexture.transform;
					transform.localPosition = new Vector3((float)(-(float)this.clanName.width) * 0.5f - 16f, transform.localPosition.y, transform.localPosition.z);
				}
			}
			else
			{
				this.clanTexture.mainTexture = null;
			}
			this.UpdateExpFrameInLobby();
		}
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000AFDF4 File Offset: 0x000ADFF4
	public void UpdateNickInLobby()
	{
		this.myLobbyNickname = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000AFE08 File Offset: 0x000AE008
	private void UpdateExpFrameInLobby()
	{
		int currentLevel = ExperienceController.sharedController.currentLevel;
		this.ranksSpriteForLobby.spriteName = "Rank_" + currentLevel.ToString();
		int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
		int num2 = Mathf.Clamp(ExperienceController.sharedController.CurrentExperience, 0, num);
		string text = string.Format("{0} {1}/{2}", LocalizationStore.Get("Key_0204"), num2, num);
		if (ExperienceController.sharedController.currentLevel == 31)
		{
			text = LocalizationStore.Get("Key_0928");
		}
		this.expLabel.text = text;
		this.expProgressSprite.width = Mathf.RoundToInt(146f * ((ExperienceController.sharedController.currentLevel != 31) ? ((float)num2 / (float)num) : 1f));
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000AFEE0 File Offset: 0x000AE0E0
	public void SetCommandColor(int _command = 0)
	{
		if (_command == 1)
		{
			this.nickLabel.color = Color.blue;
			return;
		}
		if (_command == 2)
		{
			this.nickLabel.color = Color.red;
			return;
		}
		this.nickLabel.color = Color.white;
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000AFF30 File Offset: 0x000AE130
	public void ResetTimeShow(float _time = 0.1f)
	{
		this.timeShow = _time;
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000AFF3C File Offset: 0x000AE13C
	private void StopShow()
	{
		this.currentType = NickLabelController.TypeNickLabel.None;
		this.HideLabel();
		base.gameObject.SetActive(false);
		this.playerScript = null;
		this.pointScript = null;
		this.turretScript = null;
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000AFF78 File Offset: 0x000AE178
	private void CheckShow()
	{
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			this.ResetTimeShow(-1f);
			return;
		}
		if (Defs.isDaterRegim)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
		if (this.currentType == NickLabelController.TypeNickLabel.Point)
		{
			if (this.pointScript != null && this.pointScript.isBaseActive)
			{
				this.ResetTimeShow(0.1f);
			}
			else
			{
				this.ResetTimeShow(-1f);
			}
			return;
		}
		if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby || this.currentType == NickLabelController.TypeNickLabel.FreeCoins || this.currentType == NickLabelController.TypeNickLabel.GetGift || this.currentType == NickLabelController.TypeNickLabel.Nest || this.currentType == NickLabelController.TypeNickLabel.InAppBonus || this.currentType == NickLabelController.TypeNickLabel.Leprechaunt)
		{
			if (AskNameManager.isComplete)
			{
				this.ResetTimeShow(1f);
			}
			return;
		}
		if ((Defs.isHunger && HungerGameController.Instance != null && !HungerGameController.Instance.isGo) || WeaponManager.sharedManager.myPlayer == null)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.isKilled)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
		if ((ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints) && WeaponManager.sharedManager.myPlayer != null && WeaponManager.sharedManager.myPlayerMoveC != null && this.playerScript != null && WeaponManager.sharedManager.myPlayerMoveC.myCommand == this.playerScript.myCommand)
		{
			this.ResetTimeShow(0.1f);
			return;
		}
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000B0170 File Offset: 0x000AE370
	private void UpdateTurrethealthSprite()
	{
		float num = (float)Mathf.RoundToInt((float)this.maxHeathWidth * (((this.turretScript.health >= 0f) ? this.turretScript.health : 0f) / this.turretScript.maxHealth));
		if (num < 0.1f)
		{
			num = 0f;
			this.healthSprite.enabled = false;
		}
		else if (!this.healthSprite.enabled)
		{
			this.healthSprite.enabled = true;
		}
		this.healthSprite.width = Mathf.RoundToInt(num);
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000B0214 File Offset: 0x000AE414
	public void LateUpdate()
	{
		if (this.target == null)
		{
			this.StopShow();
			return;
		}
		this.CheckShow();
		if (this.timeShow > 0f)
		{
			this.timeShow -= Time.deltaTime;
		}
		if (this.timeShow <= 0f || this.target.position.y <= -1000f || !(NickLabelController.currentCamera != null))
		{
			this.HideLabel();
			return;
		}
		this.posLabel = NickLabelController.currentCamera.WorldToViewportPoint(this.target.position + this.offset + ((this.currentType != NickLabelController.TypeNickLabel.Player || !(this.playerScript != null) || !this.playerScript.isMechActive) ? Vector3.zero : this.offsetMech));
		if (this.posLabel.z >= 0f)
		{
			if (this.currentType == NickLabelController.TypeNickLabel.Turret)
			{
				this.UpdateTurrethealthSprite();
			}
			if (this.currentType == NickLabelController.TypeNickLabel.PlayerLobby)
			{
				this.UpdateInfo();
			}
			if (this.currentType == NickLabelController.TypeNickLabel.Player || this.currentType == NickLabelController.TypeNickLabel.Turret)
			{
				if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
				{
					if (WeaponManager.sharedManager.myPlayerMoveC == null)
					{
						this.SetCommandColor(0);
					}
					else
					{
						this.SetCommandColor((WeaponManager.sharedManager.myPlayerMoveC.myCommand != this.playerScript.myCommand) ? 2 : 1);
					}
				}
				else
				{
					this.SetCommandColor((!Defs.isDaterRegim && !Defs.isCOOP) ? 2 : 0);
				}
			}
			this.thisTransform.localPosition = new Vector3((this.posLabel.x - 0.5f) * this.coefScreen.x, (this.posLabel.y - 0.5f) * this.coefScreen.y, 0f);
			this.isHideLabel = false;
			return;
		}
		this.HideLabel();
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000B045C File Offset: 0x000AE65C
	private void HideLabel()
	{
		if (!this.isHideLabel)
		{
			this.isHideLabel = true;
			this.thisTransform.localPosition = new Vector3(0f, -10000f, 0f);
		}
	}

	// Token: 0x04001798 RID: 6040
	public static Camera currentCamera;

	// Token: 0x04001799 RID: 6041
	public Transform target;

	// Token: 0x0400179A RID: 6042
	public NickLabelController.TypeNickLabel currentType;

	// Token: 0x0400179B RID: 6043
	[Header("Player label")]
	public UILabel nickLabel;

	// Token: 0x0400179C RID: 6044
	public UISprite rankTexture;

	// Token: 0x0400179D RID: 6045
	public UILabel clanName;

	// Token: 0x0400179E RID: 6046
	public UITexture clanTexture;

	// Token: 0x0400179F RID: 6047
	public GameObject placeMarker;

	// Token: 0x040017A0 RID: 6048
	public UISprite multyKill;

	// Token: 0x040017A1 RID: 6049
	[Header("Lobby Exp.")]
	public GameObject expFrameLobby;

	// Token: 0x040017A2 RID: 6050
	public UISprite expProgressSprite;

	// Token: 0x040017A3 RID: 6051
	public UILabel expLabel;

	// Token: 0x040017A4 RID: 6052
	public UISprite ranksSpriteForLobby;

	// Token: 0x040017A5 RID: 6053
	[Header("Point")]
	public UISprite pointSprite;

	// Token: 0x040017A6 RID: 6054
	public UISprite pointFillSprite;

	// Token: 0x040017A7 RID: 6055
	[Header("Turret")]
	public GameObject isEnemySprite;

	// Token: 0x040017A8 RID: 6056
	public GameObject healthObj;

	// Token: 0x040017A9 RID: 6057
	public UISprite healthSprite;

	// Token: 0x040017AA RID: 6058
	[Header("Free award")]
	public GameObject freeAwardTitle;

	// Token: 0x040017AB RID: 6059
	public GameObject freeAwardGemsLabel;

	// Token: 0x040017AC RID: 6060
	public GameObject freeAwardCoinsLabel;

	// Token: 0x040017AD RID: 6061
	[Header("Gacha")]
	public GameObject GiftObject;

	// Token: 0x040017AE RID: 6062
	public GameObject GiftLabelObject;

	// Token: 0x040017AF RID: 6063
	public GameObject GiftTimerObject;

	// Token: 0x040017B0 RID: 6064
	public UILabel GiftTimerLabel;

	// Token: 0x040017B1 RID: 6065
	public Vector3 GiftLabelPosWithoutTimer;

	// Token: 0x040017B2 RID: 6066
	[ReadOnly]
	public Vector3 GiftLabelPos;

	// Token: 0x040017B3 RID: 6067
	[Header("Nest")]
	public GameObject NestGO;

	// Token: 0x040017B4 RID: 6068
	public UILabel NestTimerLabel;

	// Token: 0x040017B5 RID: 6069
	public Vector3 NestLabelPosWithoutTimer;

	// Token: 0x040017B6 RID: 6070
	[ReadOnly]
	public Vector3 NestLabelPos;

	// Token: 0x040017B7 RID: 6071
	[Header("InAppBonus")]
	public GameObject InAppBonusGO;

	// Token: 0x040017B8 RID: 6072
	public UILabel InAppBonusTimerLabel;

	// Token: 0x040017B9 RID: 6073
	[ReadOnly]
	public Vector3 InAppBonusLabelPos;

	// Token: 0x040017BA RID: 6074
	[Header("Leprechaunt")]
	public GameObject LeprechauntGO;

	// Token: 0x040017BB RID: 6075
	public UILabel LeprechauntDaysLeft;

	// Token: 0x040017BC RID: 6076
	public GameObject LeprechauntGemsRewardAvailableGO;

	// Token: 0x040017BD RID: 6077
	public UILabel LeprechauntGemsRewardAvailable;

	// Token: 0x040017BE RID: 6078
	public UILabel LeprechauntRewardTimeLeft;

	// Token: 0x040017BF RID: 6079
	public GameObject LeprechauntGemsIcon;

	// Token: 0x040017C0 RID: 6080
	public GameObject LeprechauntCoinsIcon;

	// Token: 0x040017C1 RID: 6081
	[NonSerialized]
	public Player_move_c playerScript;

	// Token: 0x040017C2 RID: 6082
	private Transform thisTransform;

	// Token: 0x040017C3 RID: 6083
	private BasePointController pointScript;

	// Token: 0x040017C4 RID: 6084
	private TurretController turretScript;

	// Token: 0x040017C5 RID: 6085
	private Vector3 offset = Vector3.up;

	// Token: 0x040017C6 RID: 6086
	private Vector3 offsetMech = new Vector3(0f, 0.5f, 0f);

	// Token: 0x040017C7 RID: 6087
	private float timeShow;

	// Token: 0x040017C8 RID: 6088
	private Vector3 posLabel;

	// Token: 0x040017C9 RID: 6089
	private int maxHeathWidth = 134;

	// Token: 0x040017CA RID: 6090
	private float timerShowMyltyKill;

	// Token: 0x040017CB RID: 6091
	private float maxTimerShowMultyKill = 5f;

	// Token: 0x040017CC RID: 6092
	private float minScale = 0.5f;

	// Token: 0x040017CD RID: 6093
	private float minDist = 10f;

	// Token: 0x040017CE RID: 6094
	private float maxDist = 30f;

	// Token: 0x040017CF RID: 6095
	private Vector2 coefScreen = new Vector2((float)Screen.width * 768f / (float)Screen.height, 768f);

	// Token: 0x040017D0 RID: 6096
	private string myLobbyNickname;

	// Token: 0x040017D1 RID: 6097
	private bool isHideLabel;

	// Token: 0x020003CE RID: 974
	public enum TypeNickLabel
	{
		// Token: 0x040017D3 RID: 6099
		None,
		// Token: 0x040017D4 RID: 6100
		Player,
		// Token: 0x040017D5 RID: 6101
		Turret,
		// Token: 0x040017D6 RID: 6102
		Point,
		// Token: 0x040017D7 RID: 6103
		PlayerLobby,
		// Token: 0x040017D8 RID: 6104
		FreeCoins,
		// Token: 0x040017D9 RID: 6105
		GetGift,
		// Token: 0x040017DA RID: 6106
		Nest,
		// Token: 0x040017DB RID: 6107
		InAppBonus,
		// Token: 0x040017DC RID: 6108
		Leprechaunt
	}
}
