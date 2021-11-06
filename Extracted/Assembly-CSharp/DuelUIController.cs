using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class DuelUIController : MonoBehaviour
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x00026DB0 File Offset: 0x00024FB0
	public DuelController duelController
	{
		get
		{
			return DuelController.instance;
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00026DB8 File Offset: 0x00024FB8
	private void Awake()
	{
		this.nextEnemyButton.Clicked += this.NextEnemyButtonClick;
		this.nextButton.Clicked += this.NextButtonClick;
		this.revengeButton.Clicked += this.RevengeButtonClick;
		this.revengeUIButton = this.revengeButton.GetComponent<UIButton>();
		this.revengeButtonSpriteNormal = this.revengeUIButton.normalSprite;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00026E2C File Offset: 0x0002502C
	public void ShowRevengePanel(bool revengeReceived, bool opponentLeft)
	{
		this.revengePanel.SetActive(true);
		this.blinkRevengeButton = revengeReceived;
		if (opponentLeft)
		{
			this.revengeLabel.text = LocalizationStore.Get("Key_2433");
			this.revengeButton.GetComponent<UIButton>().isEnabled = false;
		}
		else
		{
			this.revengeLabel.text = LocalizationStore.Get((!revengeReceived) ? "Key_2435" : "Key_2434");
		}
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00026EA4 File Offset: 0x000250A4
	public void HideRevengePanel()
	{
		this.revengePanel.SetActive(false);
		this.blinkRevengeButton = false;
		this.revengeButton.GetComponent<UIButton>().normalSprite = this.revengeButtonSpriteNormal;
		this.inBlink = false;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00026EE4 File Offset: 0x000250E4
	public void ShowStartInterface()
	{
		WeaponManager.sharedManager.myNetworkStartTable.isShowFinished = true;
		this.ShowWaitForOpponentInterface();
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00026EFC File Offset: 0x000250FC
	public void ShowChangeAreaInterface(bool opponentLeft = false)
	{
		if (this.duelController.gameStatus != DuelController.GameStatus.End)
		{
			return;
		}
		this.duelController.SendGameLeft();
		this.duelController.gameStatus = DuelController.GameStatus.ChangeArea;
		this.showCharacters = true;
		this.endInterface.SetActive(false);
		this.waitForOpponent.SetActive(false);
		this.versusInterface.SetActive(false);
		this.changeArea.SetActive(true);
		this.opponentLeftLabel.SetActive(opponentLeft);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00026F78 File Offset: 0x00025178
	public void ShowWaitForOpponentInterface()
	{
		this.showCharacters = true;
		this.waitForOpponent.SetActive(true);
		this.versusInterface.SetActive(false);
		this.myPlayerInfo.SetPointInWorld(this.duelController.myCharacter.hatPoint, this.duelController.myCharacter.transform);
		this.opponentPlayerInfo.SetPointInWorld(this.duelController.enemyCharacter.hatPoint, this.duelController.enemyCharacter.transform);
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00026FFC File Offset: 0x000251FC
	public void ShowVersusUI()
	{
		this.showCharacters = true;
		this.endInterface.SetActive(false);
		this.waitForOpponent.SetActive(false);
		this.versusInterface.SetActive(true);
		this.changeArea.SetActive(false);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00027040 File Offset: 0x00025240
	public void IngameUI()
	{
		this.HideRevengePanel();
		WeaponManager.sharedManager.myNetworkStartTable.isShowFinished = false;
		this.showCharacters = false;
		this.versusInterface.SetActive(false);
		this.waitForOpponent.SetActive(false);
		this.changeArea.SetActive(false);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00027090 File Offset: 0x00025290
	public void UpdateCharactersInfo()
	{
		this.opponentPlayerInfo.gameObject.SetActive(this.duelController.showEnemyCharacter);
		if (this.duelController.showEnemyCharacter && this.duelController.opponentNetworkTable != null)
		{
			this.opponentPlayerInfo.FillByTable(this.duelController.opponentNetworkTable);
		}
		this.myPlayerInfo.gameObject.SetActive(this.duelController.showMyCharacter);
		if (this.duelController.showMyCharacter)
		{
			this.myPlayerInfo.FillByTable(WeaponManager.sharedManager.myNetworkStartTable);
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00027134 File Offset: 0x00025334
	private void Update()
	{
		this.UpdateCharactersInfo();
		if (this.blinkRevengeButton && this.blinkTime < Time.time)
		{
			this.inBlink = !this.inBlink;
			this.blinkTime = Time.time + 0.3f;
			this.revengeUIButton.normalSprite = ((!this.inBlink) ? this.revengeButtonSpriteNormal : this.revengeButtonSpriteBlink);
		}
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x000271AC File Offset: 0x000253AC
	public void ShowFinishedInterface(RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool isWinner, bool deadHeat)
	{
		base.gameObject.SetActive(true);
		this.coinReward.text = _addCoin.ToString();
		this.expReward.text = _addExpierence.ToString();
		this.myWinLabel.gameObject.SetActive(isWinner && !deadHeat);
		this.myLooseLabel.gameObject.SetActive(!isWinner && !deadHeat);
		this.myDHLabel.gameObject.SetActive(deadHeat);
		this.enemyWinLabel.gameObject.SetActive(!isWinner && !deadHeat);
		this.enemyLooseLabel.gameObject.SetActive(isWinner && !deadHeat);
		this.enemyDHLabel.gameObject.SetActive(deadHeat);
		if (this.duelController.opponentNetworkTable != null && this.duelController.opponentNetworkTable.myPlayerMoveC != null)
		{
			this.duelController.opponentNetworkTable.myPlayerMoveC.mySkinName.gameObject.SetActive(false);
		}
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x000272DC File Offset: 0x000254DC
	public void ShowEndInterface()
	{
		if (Initializer.networkTables.Count < 2)
		{
			this.ShowChangeAreaInterface(false);
			return;
		}
		this.showCharacters = true;
		bool flag = NetworkStartTable.LocalOrPasswordRoom();
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		this.endInterface.SetActive(true);
		int oldIndexMy = myNetworkStartTable.oldIndexMy;
		int num = -1;
		for (int i = 0; i < myNetworkStartTable.oldSpisokPixelBookID.Length; i++)
		{
			if (i != myNetworkStartTable.oldIndexMy)
			{
				num = i;
				break;
			}
		}
		this.myKills.text = ((!(myNetworkStartTable.oldCountLilsSpisok[oldIndexMy] == "-1")) ? myNetworkStartTable.oldCountLilsSpisok[oldIndexMy] : "0");
		this.myPoints.text = ((!(myNetworkStartTable.oldScoreSpisok[oldIndexMy] == "-1")) ? myNetworkStartTable.oldScoreSpisok[oldIndexMy] : "0");
		if (num != -1)
		{
			this.enemyKills.text = ((!(myNetworkStartTable.oldCountLilsSpisok[num] == "-1")) ? myNetworkStartTable.oldCountLilsSpisok[num] : "0");
			this.enemyPoints.text = ((!(myNetworkStartTable.oldScoreSpisok[num] == "-1")) ? myNetworkStartTable.oldScoreSpisok[num] : "0");
		}
		ExperienceController.sharedController.isShowRanks = true;
		this.myPlayerInfo.SetPointInWorld(this.duelController.myCharacter.hatPoint, this.duelController.myCharacter.transform);
		this.opponentPlayerInfo.SetPointInWorld(this.duelController.enemyCharacter.hatPoint, this.duelController.enemyCharacter.transform);
		this.duelController.SetShopEvents();
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000274A8 File Offset: 0x000256A8
	public void NextButtonClick(object sender, EventArgs e)
	{
		this.ShowChangeAreaInterface(false);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000274B4 File Offset: 0x000256B4
	public void NextEnemyButtonClick(object sender, EventArgs e)
	{
		WeaponManager.sharedManager.myNetworkStartTable.RandomRoomClickBtnInDuel();
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x000274C8 File Offset: 0x000256C8
	public void RevengeButtonClick(object sender, EventArgs e)
	{
		this.duelController.RevengeRequest();
	}

	// Token: 0x04000522 RID: 1314
	public DuelPlayerInfo myPlayerInfo;

	// Token: 0x04000523 RID: 1315
	public DuelPlayerInfo opponentPlayerInfo;

	// Token: 0x04000524 RID: 1316
	public GameObject waitForOpponent;

	// Token: 0x04000525 RID: 1317
	public GameObject versusInterface;

	// Token: 0x04000526 RID: 1318
	public GameObject endInterface;

	// Token: 0x04000527 RID: 1319
	public GameObject changeArea;

	// Token: 0x04000528 RID: 1320
	public GameObject rewardPanel;

	// Token: 0x04000529 RID: 1321
	public GameObject opponentLeftLabel;

	// Token: 0x0400052A RID: 1322
	public UILabel myWinLabel;

	// Token: 0x0400052B RID: 1323
	public UILabel myLooseLabel;

	// Token: 0x0400052C RID: 1324
	public UILabel myDHLabel;

	// Token: 0x0400052D RID: 1325
	public UILabel enemyWinLabel;

	// Token: 0x0400052E RID: 1326
	public UILabel enemyLooseLabel;

	// Token: 0x0400052F RID: 1327
	public UILabel enemyDHLabel;

	// Token: 0x04000530 RID: 1328
	public UILabel expReward;

	// Token: 0x04000531 RID: 1329
	public UILabel coinReward;

	// Token: 0x04000532 RID: 1330
	public UILabel myPoints;

	// Token: 0x04000533 RID: 1331
	public UILabel myKills;

	// Token: 0x04000534 RID: 1332
	public UILabel enemyPoints;

	// Token: 0x04000535 RID: 1333
	public UILabel enemyKills;

	// Token: 0x04000536 RID: 1334
	public UILabel versusTimer;

	// Token: 0x04000537 RID: 1335
	public ButtonHandler nextButton;

	// Token: 0x04000538 RID: 1336
	public ButtonHandler nextEnemyButton;

	// Token: 0x04000539 RID: 1337
	public ButtonHandler revengeButton;

	// Token: 0x0400053A RID: 1338
	[HideInInspector]
	public bool showCharacters;

	// Token: 0x0400053B RID: 1339
	public GameObject revengePanel;

	// Token: 0x0400053C RID: 1340
	public UILabel revengeLabel;

	// Token: 0x0400053D RID: 1341
	public string revengeButtonSpriteBlink;

	// Token: 0x0400053E RID: 1342
	private string revengeButtonSpriteNormal;

	// Token: 0x0400053F RID: 1343
	private UIButton revengeUIButton;

	// Token: 0x04000540 RID: 1344
	private bool blinkRevengeButton;

	// Token: 0x04000541 RID: 1345
	private bool inBlink;

	// Token: 0x04000542 RID: 1346
	private float blinkTime;
}
