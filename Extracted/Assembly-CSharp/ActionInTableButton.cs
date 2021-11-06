using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public sealed class ActionInTableButton : MonoBehaviour
{
	// Token: 0x06000006 RID: 6 RVA: 0x00002244 File Offset: 0x00000444
	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			float x = this.countKillsPlayers.transform.position.x;
			this.countKillsPlayers.transform.position = new Vector3(this.scorePlayers.transform.position.x, this.countKillsPlayers.transform.position.y, this.countKillsPlayers.transform.position.z);
			this.scorePlayers.transform.position = new Vector3(x, this.scorePlayers.transform.position.y, this.scorePlayers.transform.position.z);
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			this.scorePlayers.transform.position = new Vector3(this.countKillsPlayers.transform.position.x, this.scorePlayers.transform.position.y, this.scorePlayers.transform.position.z);
			this.countKillsPlayers.gameObject.SetActive(false);
		}
		if (Defs.isDaterRegim)
		{
			this.countKillsPlayers.gameObject.SetActive(true);
			this.scorePlayers.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000023C4 File Offset: 0x000005C4
	private void Update()
	{
		this.UpdateAddState();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000023CC File Offset: 0x000005CC
	public void UpdateAddState()
	{
		if (string.IsNullOrEmpty(this.pixelbookID) || !FriendsController.sharedController.IsShowAdd(this.pixelbookID))
		{
			if (string.IsNullOrEmpty(this.pixelbookID) || this.pixelbookID.Equals("0") || this.pixelbookID.Equals("-1") || this.pixelbookID.Equals(FriendsController.sharedController.id) || !Defs2.IsAvalibleAddFrends() || string.IsNullOrEmpty(FriendsController.sharedController.id))
			{
				if (this.buttonScript.enabled)
				{
					this.buttonScript.enabled = false;
					this.buttonScript.tweenTarget.SetActive(false);
					this.check.SetActive(false);
				}
				if (this.check.activeSelf)
				{
					this.check.SetActive(false);
				}
			}
			else
			{
				if (this.buttonScript.enabled)
				{
					this.buttonScript.enabled = false;
					this.buttonScript.tweenTarget.SetActive(false);
				}
				if (!this.check.activeSelf)
				{
					this.check.SetActive(true);
				}
			}
		}
		else
		{
			if (!this.buttonScript.enabled)
			{
				this.buttonScript.enabled = true;
				this.buttonScript.tweenTarget.SetActive(true);
				this.check.SetActive(true);
			}
			if (!this.check.activeSelf)
			{
				this.check.SetActive(true);
			}
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002570 File Offset: 0x00000770
	public void UpdateState(bool isActive, int placeIndex = 0, bool _isMine = false, int command = 0, string nick = "", string score = "", string countKills = "", int rank = 1, Texture clanLogo = null, string _pixelbookID = "")
	{
		this.pixelbookID = _pixelbookID;
		this.nickPlayer = nick;
		this.isMine = _isMine;
		if (!isActive)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			Color white = Color.white;
			if (this.isMine)
			{
				this.isMineSprite.SetActive(true);
				if (this.buttonScript.enabled)
				{
					this.buttonScript.enabled = false;
					this.buttonScript.tweenTarget.SetActive(false);
					this.check.SetActive(false);
				}
				if (this.check.activeSelf)
				{
					this.check.SetActive(false);
				}
				white = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				this.isMineSprite.SetActive(false);
				bool flag = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
				if (flag)
				{
					if (command == 0)
					{
						white = new Color(0.6f, 0.6f, 0.6f, 1f);
					}
					else if (command == 1)
					{
						white = new Color(0.6f, 0.8f, 1f, 1f);
					}
					else
					{
						white = new Color(1f, 0.7f, 0.7f, 1f);
					}
				}
			}
			base.gameObject.SetActive(true);
			this.namesPlayers.text = nick;
			if (this.defaultNameLabelPos == Vector3.zero)
			{
				this.defaultNameLabelPos = this.namesPlayers.transform.localPosition;
			}
			if (clanLogo == null)
			{
				this.namesPlayers.transform.localPosition = this.defaultNameLabelPos;
			}
			else
			{
				this.namesPlayers.transform.localPosition = this.defaultNameLabelPos + Vector3.right * 34f;
			}
			this.namesPlayers.color = white;
			this.scorePlayers.text = score;
			this.scorePlayers.color = white;
			this.countKillsPlayers.text = countKills;
			this.countKillsPlayers.color = white;
			this.rankSprite.spriteName = "Rank_" + rank;
			this.clanTexture.mainTexture = clanLogo;
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000027D8 File Offset: 0x000009D8
	private void OnClick()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		if (this.isMine)
		{
			return;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.networkStartTableNGUIController.ShowActionPanel(this.pixelbookID, this.nickPlayer);
	}

	// Token: 0x04000001 RID: 1
	public UIButton buttonScript;

	// Token: 0x04000002 RID: 2
	public UISprite backgroundSprite;

	// Token: 0x04000003 RID: 3
	public UISprite rankSprite;

	// Token: 0x04000004 RID: 4
	public GameObject check;

	// Token: 0x04000005 RID: 5
	public UILabel namesPlayers;

	// Token: 0x04000006 RID: 6
	public Vector3 defaultNameLabelPos;

	// Token: 0x04000007 RID: 7
	public UILabel scorePlayers;

	// Token: 0x04000008 RID: 8
	public UILabel countKillsPlayers;

	// Token: 0x04000009 RID: 9
	public UITexture clanTexture;

	// Token: 0x0400000A RID: 10
	public string pixelbookID;

	// Token: 0x0400000B RID: 11
	public string nickPlayer;

	// Token: 0x0400000C RID: 12
	public NetworkStartTableNGUIController networkStartTableNGUIController;

	// Token: 0x0400000D RID: 13
	public bool isMine;

	// Token: 0x0400000E RID: 14
	public GameObject isMineSprite;
}
