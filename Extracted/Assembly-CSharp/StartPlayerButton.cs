using System;
using UnityEngine;

// Token: 0x02000820 RID: 2080
public class StartPlayerButton : MonoBehaviour
{
	// Token: 0x06004BB3 RID: 19379 RVA: 0x001B3CC4 File Offset: 0x001B1EC4
	private void Awake()
	{
		bool flag = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
		bool flag2 = NetworkStartTable.LocalOrPasswordRoom();
		if ((!flag && this.command != StartPlayerButton.TypeButton.Start) || (flag && (this.command == StartPlayerButton.TypeButton.Start || ((this.command == StartPlayerButton.TypeButton.RandomBtn || this.command == StartPlayerButton.TypeButton.Team2 || this.command == StartPlayerButton.TypeButton.Team1) && !flag2) || (this.command == StartPlayerButton.TypeButton.TeamBattle && flag2))) || Defs.isHunger)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x001B3D70 File Offset: 0x001B1F70
	private void Start()
	{
		if ((this.command == StartPlayerButton.TypeButton.Start || this.command == StartPlayerButton.TypeButton.TeamBattle) && Defs.isRegimVidosDebug)
		{
			base.gameObject.SetActive(false);
			base.GetComponent<UIButton>().enabled = false;
		}
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x001B3DB8 File Offset: 0x001B1FB8
	private void OnEnable()
	{
		this.timeEnable = Time.realtimeSinceStartup;
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x001B3DC8 File Offset: 0x001B1FC8
	private void Update()
	{
		bool flag = Initializer.players.Count == 0 || Defs.isDaterRegim || Defs.isHunger || TimeGameController.sharedController == null || TimeGameController.sharedController.timerToEndMatch <= 0.0 || TimeGameController.sharedController.timerToEndMatch > 16.0;
		base.GetComponent<UIButton>().isEnabled = (flag && (!Defs.isFlag || Initializer.flag1 != null));
	}

	// Token: 0x06004BB7 RID: 19383 RVA: 0x001B3E68 File Offset: 0x001B2068
	private void OnClick()
	{
		if (Time.time - NotificationController.timeStartApp < 3f || (Defs.isCapturePoints && Time.realtimeSinceStartup - this.timeEnable < 1.5f))
		{
			return;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			int num = WeaponManager.sharedManager.myNetworkStartTable.myCommand;
			if (num <= 0)
			{
				num = (int)this.command;
				if ((this.command == StartPlayerButton.TypeButton.RandomBtn || this.command == StartPlayerButton.TypeButton.TeamBattle) && this.buttonController != null)
				{
					if (this.buttonController.countRed < this.buttonController.countBlue)
					{
						num = 2;
					}
					else if (this.buttonController.countRed > this.buttonController.countBlue)
					{
						num = 1;
					}
					else
					{
						num = UnityEngine.Random.Range(1, 3);
					}
				}
			}
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().StartPlayerButtonClick(num);
		}
	}

	// Token: 0x04003AD2 RID: 15058
	public StartPlayerButton.TypeButton command;

	// Token: 0x04003AD3 RID: 15059
	public BlueRedButtonController buttonController;

	// Token: 0x04003AD4 RID: 15060
	private float timeEnable;

	// Token: 0x02000821 RID: 2081
	public enum TypeButton
	{
		// Token: 0x04003AD6 RID: 15062
		Start,
		// Token: 0x04003AD7 RID: 15063
		Team1,
		// Token: 0x04003AD8 RID: 15064
		Team2,
		// Token: 0x04003AD9 RID: 15065
		RandomBtn,
		// Token: 0x04003ADA RID: 15066
		TeamBattle
	}
}
