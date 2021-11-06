using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class RanksTable : MonoBehaviour
{
	// Token: 0x06002B26 RID: 11046 RVA: 0x000E3194 File Offset: 0x000E1394
	private void Awake()
	{
		this.othersStr = LocalizationStore.Get("Key_1224");
		this.isTeamMode = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x000E31D0 File Offset: 0x000E13D0
	private void Start()
	{
		if (this.isTeamMode)
		{
			this.panelRanksTeam.SetActive(true);
			this.panelRanks.SetActive(false);
			this.modePC1.SetActive(ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
			this.modeFC1.SetActive(ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture);
			this.modeTDM1.SetActive(ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight);
		}
		else
		{
			this.panelRanksTeam.SetActive(false);
			this.panelRanks.SetActive(true);
		}
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x000E3258 File Offset: 0x000E1458
	private void Update()
	{
		if (this.isShowRanks || this.isShowTableStart)
		{
			this.ReloadTabsFromReal();
			this.UpdateRanksFromTabs();
		}
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x000E3288 File Offset: 0x000E1488
	private void ReloadTabsFromReal()
	{
		this.tabsBlue.Clear();
		this.tabsRed.Clear();
		this.tabsWhite.Clear();
		this.tabs.Clear();
		this.tabs.AddRange(Initializer.networkTables);
		for (int i = 1; i < this.tabs.Count; i++)
		{
			NetworkStartTable networkStartTable = this.tabs[i];
			for (int j = 0; j < i; j++)
			{
				NetworkStartTable networkStartTable2 = this.tabs[j];
				if ((!Defs.isDuel && !Defs.isFlag && !Defs.isCapturePoints && (networkStartTable.score > networkStartTable2.score || (networkStartTable.score == networkStartTable2.score && networkStartTable.CountKills > networkStartTable2.CountKills))) || ((Defs.isDuel || Defs.isFlag || Defs.isCapturePoints) && (networkStartTable.CountKills > networkStartTable2.CountKills || (networkStartTable.CountKills == networkStartTable2.CountKills && networkStartTable.score > networkStartTable2.score))))
				{
					NetworkStartTable value = this.tabs[i];
					for (int k = i - 1; k >= j; k--)
					{
						this.tabs[k + 1] = this.tabs[k];
					}
					this.tabs[j] = value;
					break;
				}
			}
		}
		if (this.isTeamMode)
		{
			for (int l = 0; l < this.tabs.Count; l++)
			{
				if (this.tabs[l].myCommand == 1)
				{
					this.tabsBlue.Add(this.tabs[l]);
				}
				else if (this.tabs[l].myCommand == 2)
				{
					this.tabsRed.Add(this.tabs[l]);
				}
				else
				{
					this.tabsWhite.Add(this.tabs[l]);
				}
			}
		}
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x000E34C0 File Offset: 0x000E16C0
	private void FillButtonFromOldState(ActionInTableButton button, int tableIndex, bool isBlueTable = true, int team = 0)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		string text = string.Empty;
		string text2 = string.Empty;
		string pixelbookID = string.Empty;
		string nick = string.Empty;
		bool isMine;
		int rank;
		Texture clanLogo;
		if (!this.isTeamMode)
		{
			isMine = (myNetworkStartTable.oldIndexMy == tableIndex);
			text = myNetworkStartTable.oldCountLilsSpisok[tableIndex];
			text2 = myNetworkStartTable.oldScoreSpisok[tableIndex];
			pixelbookID = myNetworkStartTable.oldSpisokPixelBookID[tableIndex].ToString();
			nick = myNetworkStartTable.oldSpisokName[tableIndex];
			rank = myNetworkStartTable.oldSpisokRanks[tableIndex];
			clanLogo = myNetworkStartTable.oldSpisokMyClanLogo[tableIndex];
		}
		else
		{
			if (isBlueTable)
			{
				isMine = (myNetworkStartTable.oldIndexMy == tableIndex && myNetworkStartTable.myCommandOld == 1);
			}
			else
			{
				isMine = (myNetworkStartTable.oldIndexMy == tableIndex && myNetworkStartTable.myCommandOld == 2);
			}
			text = ((!isBlueTable) ? myNetworkStartTable.oldCountLilsSpisokRed[tableIndex] : myNetworkStartTable.oldCountLilsSpisokBlue[tableIndex]);
			text2 = ((!isBlueTable) ? myNetworkStartTable.oldScoreSpisokRed[tableIndex] : myNetworkStartTable.oldScoreSpisokBlue[tableIndex]);
			pixelbookID = ((!isBlueTable) ? myNetworkStartTable.oldSpisokPixelBookIDRed[tableIndex] : myNetworkStartTable.oldSpisokPixelBookIDBlue[tableIndex].ToString());
			nick = ((!isBlueTable) ? myNetworkStartTable.oldSpisokNameRed[tableIndex] : myNetworkStartTable.oldSpisokNameBlue[tableIndex]);
			rank = ((!isBlueTable) ? myNetworkStartTable.oldSpisokRanksRed[tableIndex] : myNetworkStartTable.oldSpisokRanksBlue[tableIndex]);
			clanLogo = ((!isBlueTable) ? myNetworkStartTable.oldSpisokMyClanLogoRed[tableIndex] : myNetworkStartTable.oldSpisokMyClanLogoBlue[tableIndex]);
		}
		if (text == "-1")
		{
			text = "0";
		}
		if (text2 == "-1")
		{
			text2 = "0";
		}
		button.UpdateState(true, tableIndex, isMine, team, nick, text2, text, rank, clanLogo, pixelbookID);
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000E3684 File Offset: 0x000E1884
	private void FillButtonFromTable(ActionInTableButton button, NetworkStartTable table, int tableIndex, int team = 0)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		string text = string.Empty;
		string text2 = string.Empty;
		string pixelbookID = string.Empty;
		string nick = string.Empty;
		bool isMine = table.Equals(myNetworkStartTable);
		text = table.CountKills.ToString();
		text2 = table.score.ToString();
		pixelbookID = table.pixelBookID.ToString();
		nick = table.NamePlayer;
		int myRanks = table.myRanks;
		Texture myClanTexture = table.myClanTexture;
		if (text == "-1")
		{
			text = "0";
		}
		if (text2 == "-1")
		{
			text2 = "0";
		}
		button.UpdateState(true, tableIndex, isMine, team, nick, text2, text, myRanks, myClanTexture, pixelbookID);
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000E3748 File Offset: 0x000E1948
	private void FillDeathmatchButtons(bool oldState = false)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		for (int i = 0; i < this.playersButtonsDeathmatch.Length; i++)
		{
			if (!oldState && i < this.tabs.Count)
			{
				this.FillButtonFromTable(this.playersButtonsDeathmatch[i], this.tabs[i], i, 0);
			}
			else if (oldState && i < myNetworkStartTable.oldSpisokName.Length)
			{
				this.FillButtonFromOldState(this.playersButtonsDeathmatch[i], i, true, 0);
			}
			else
			{
				this.playersButtonsDeathmatch[i].UpdateState(false, 0, false, 0, string.Empty, string.Empty, string.Empty, 1, null, string.Empty);
			}
		}
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000E3804 File Offset: 0x000E1A04
	private void FillTeamButtons(bool oldState = false)
	{
		NetworkStartTable myNetworkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		int num = Mathf.Max(0, (!oldState) ? myNetworkStartTable.myCommand : myNetworkStartTable.myCommandOld);
		this.sumRed = 0;
		this.sumBlue = 0;
		for (int i = 0; i < this.playersButtonsTeamFight.Length / 2; i++)
		{
			if (!oldState && i < Mathf.Min(this.tabsBlue.Count, 5))
			{
				this.sumBlue += ((this.tabsBlue[i].CountKills == -1) ? 0 : this.tabsBlue[i].CountKills);
				this.FillButtonFromTable(this.playersButtonsTeamFight[i + ((num != 2) ? 0 : 6)], this.tabsBlue[i], i, num);
			}
			else if (oldState && i < Mathf.Min(myNetworkStartTable.oldSpisokNameBlue.Length, 5))
			{
				this.sumBlue += int.Parse((!(myNetworkStartTable.oldCountLilsSpisokBlue[i] != "-1")) ? "0" : myNetworkStartTable.oldCountLilsSpisokBlue[i]);
				this.FillButtonFromOldState(this.playersButtonsTeamFight[i + ((num != 2) ? 0 : 6)], i, true, num);
			}
			else if (this.totalBlue - this.sumBlue > 0 && i == 5 && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				this.playersButtonsTeamFight[i + ((num != 2) ? 0 : 6)].UpdateState(true, i, false, num, this.othersStr, string.Empty, (this.totalBlue - this.sumBlue).ToString(), -1, null, string.Empty);
			}
			else
			{
				this.playersButtonsTeamFight[i + ((num != 2) ? 0 : 6)].UpdateState(false, 0, false, 0, string.Empty, string.Empty, string.Empty, 1, null, string.Empty);
			}
			if (!oldState && i < Mathf.Min(this.tabsRed.Count, 5))
			{
				this.sumRed += ((this.tabsRed[i].CountKills == -1) ? 0 : this.tabsRed[i].CountKills);
				this.FillButtonFromTable(this.playersButtonsTeamFight[i + ((num == 2) ? 0 : 6)], this.tabsRed[i], i, (num != 0) ? ((num != 2) ? 2 : 1) : 0);
			}
			else if (oldState && i < Mathf.Min(myNetworkStartTable.oldSpisokNameRed.Length, 5))
			{
				this.sumRed += int.Parse((!(myNetworkStartTable.oldCountLilsSpisokRed[i] != "-1")) ? "0" : myNetworkStartTable.oldCountLilsSpisokRed[i]);
				this.FillButtonFromOldState(this.playersButtonsTeamFight[i + ((num == 2) ? 0 : 6)], i, false, (num != 0) ? ((num != 2) ? 2 : 1) : 0);
			}
			else if (this.totalRed - this.sumRed > 0 && i == 5 && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				this.playersButtonsTeamFight[i + ((num == 2) ? 0 : 6)].UpdateState(true, i, false, (num != 0) ? ((num != 2) ? 2 : 1) : 0, this.othersStr, string.Empty, (this.totalRed - this.sumRed).ToString(), -1, null, string.Empty);
			}
			else
			{
				this.playersButtonsTeamFight[i + ((num == 2) ? 0 : 6)].UpdateState(false, 0, false, 0, string.Empty, string.Empty, string.Empty, 1, null, string.Empty);
			}
		}
		if (oldState && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			if (this.totalBlue < this.sumBlue)
			{
				this.totalBlue = this.sumBlue;
			}
			if (this.totalRed < this.sumRed)
			{
				this.totalRed = this.sumRed;
			}
		}
		for (int j = 0; j < NetworkStartTableNGUIController.sharedController.totalBlue.Length; j++)
		{
			NetworkStartTableNGUIController.sharedController.totalBlue[j].text = ((num == 2) ? this.totalRed.ToString() : this.totalBlue.ToString());
		}
		for (int k = 0; k < NetworkStartTableNGUIController.sharedController.totalRed.Length; k++)
		{
			NetworkStartTableNGUIController.sharedController.totalRed[k].text = ((num == 2) ? this.totalBlue.ToString() : this.totalRed.ToString());
		}
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x000E3CF4 File Offset: 0x000E1EF4
	private void UpdateRanksFromTabs()
	{
		if (Defs.isCompany)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				this.totalBlue = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue;
				this.totalRed = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed;
			}
			else
			{
				this.totalBlue = GlobalGameController.countKillsBlue;
				this.totalRed = GlobalGameController.countKillsRed;
			}
		}
		if (Defs.isFlag && WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			this.totalBlue = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1;
			this.totalRed = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2;
		}
		if (Defs.isCapturePoints)
		{
			this.totalBlue = Mathf.RoundToInt(CapturePointController.sharedController.scoreBlue);
			this.totalRed = Mathf.RoundToInt(CapturePointController.sharedController.scoreRed);
		}
		if (this.isTeamMode)
		{
			this.FillTeamButtons(false);
		}
		else
		{
			this.FillDeathmatchButtons(false);
		}
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000E3E00 File Offset: 0x000E2000
	public void UpdateRanksFromOldSpisok()
	{
		if (this.isTeamMode)
		{
			this.FillTeamButtons(true);
		}
		else
		{
			this.FillDeathmatchButtons(true);
		}
	}

	// Token: 0x04002034 RID: 8244
	private const int maxCountInCommandPlusOther = 6;

	// Token: 0x04002035 RID: 8245
	private const int maxCountInTeam = 5;

	// Token: 0x04002036 RID: 8246
	public GameObject panelRanks;

	// Token: 0x04002037 RID: 8247
	public GameObject panelRanksTeam;

	// Token: 0x04002038 RID: 8248
	public GameObject tekPanel;

	// Token: 0x04002039 RID: 8249
	public GameObject modePC1;

	// Token: 0x0400203A RID: 8250
	public GameObject modeFC1;

	// Token: 0x0400203B RID: 8251
	public GameObject modeTDM1;

	// Token: 0x0400203C RID: 8252
	public ActionInTableButton[] playersButtonsDeathmatch;

	// Token: 0x0400203D RID: 8253
	public ActionInTableButton[] playersButtonsTeamFight;

	// Token: 0x0400203E RID: 8254
	private List<NetworkStartTable> tabs = new List<NetworkStartTable>();

	// Token: 0x0400203F RID: 8255
	private List<NetworkStartTable> tabsBlue = new List<NetworkStartTable>();

	// Token: 0x04002040 RID: 8256
	private List<NetworkStartTable> tabsRed = new List<NetworkStartTable>();

	// Token: 0x04002041 RID: 8257
	private List<NetworkStartTable> tabsWhite = new List<NetworkStartTable>();

	// Token: 0x04002042 RID: 8258
	public bool isShowRanks;

	// Token: 0x04002043 RID: 8259
	public bool isShowTableStart;

	// Token: 0x04002044 RID: 8260
	public bool isShowTableWin;

	// Token: 0x04002045 RID: 8261
	private bool isTeamMode;

	// Token: 0x04002046 RID: 8262
	private string othersStr = "Others";

	// Token: 0x04002047 RID: 8263
	public int totalBlue;

	// Token: 0x04002048 RID: 8264
	public int totalRed;

	// Token: 0x04002049 RID: 8265
	public int sumBlue;

	// Token: 0x0400204A RID: 8266
	public int sumRed;
}
