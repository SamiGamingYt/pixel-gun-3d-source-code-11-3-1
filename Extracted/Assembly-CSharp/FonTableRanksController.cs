using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class FonTableRanksController : MonoBehaviour
{
	// Token: 0x06000841 RID: 2113 RVA: 0x000320AC File Offset: 0x000302AC
	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			float x = this.countKillsHead.transform.position.x;
			this.countKillsHead.transform.position = new Vector3(this.scoreHead.transform.position.x, this.countKillsHead.transform.position.y, this.countKillsHead.transform.position.z);
			this.scoreHead.transform.position = new Vector3(x, this.scoreHead.transform.position.y, this.scoreHead.transform.position.z);
			this.countKillsHead.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1006");
			if (this.likeHead != null)
			{
				this.likeHead.SetActive(false);
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			this.scoreHead.transform.position = new Vector3(this.countKillsHead.transform.position.x, this.scoreHead.transform.position.y, this.scoreHead.transform.position.z);
			this.countKillsHead.SetActive(false);
			if (this.likeHead != null)
			{
				this.likeHead.SetActive(false);
			}
		}
		if (Defs.isDaterRegim)
		{
			this.scoreHead.gameObject.SetActive(false);
			this.countKillsHead.gameObject.SetActive(false);
			if (this.likeHead != null)
			{
				this.likeHead.SetActive(true);
			}
		}
		else if (this.likeHead != null)
		{
			this.likeHead.SetActive(false);
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x000322BC File Offset: 0x000304BC
	public void SetCommand(int _command)
	{
		if (!this.isTeamTable)
		{
			return;
		}
		if (_command == 0)
		{
			this.fon.spriteName = "GreyTableHead";
			this.fonHead.spriteName = "GreyTableHead";
			this.fonUndrhead.spriteName = "GreyTable";
			foreach (UILabel uilabel in this.undrheadLabels)
			{
				uilabel.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			}
			this.headLabel.text = LocalizationStore.Get(this.nameCommand);
		}
		if (_command == 1)
		{
			this.fon.spriteName = "BlueTeamTableHead";
			this.fonHead.spriteName = "BlueTeamTableHead";
			this.fonUndrhead.spriteName = "BlueTeamTable";
			foreach (UILabel uilabel2 in this.undrheadLabels)
			{
				uilabel2.color = new Color(0.6f, 0.8f, 1f, 1f);
			}
			this.headLabel.text = LocalizationStore.Get("Key_1771");
		}
		if (_command == 2)
		{
			this.fon.spriteName = "RedTeamTableHead";
			this.fonHead.spriteName = "RedTeamTableHead";
			this.fonUndrhead.spriteName = "RedTeamTable";
			this.headLabel.text = LocalizationStore.Get("Key_1772");
			foreach (UILabel uilabel3 in this.undrheadLabels)
			{
				uilabel3.color = new Color(1f, 0.7f, 0.7f, 1f);
			}
		}
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00032488 File Offset: 0x00030688
	private void Update()
	{
		int num = (WeaponManager.sharedManager.myNetworkStartTable.myCommand > 0) ? WeaponManager.sharedManager.myNetworkStartTable.myCommand : WeaponManager.sharedManager.myNetworkStartTable.myCommandOld;
		if (NetworkStartTable.LocalOrPasswordRoom() && NetworkStartTableNGUIController.IsStartInterfaceShown())
		{
			num = 0;
		}
		this.SetCommand((num > 0) ? this.command : 0);
	}

	// Token: 0x040006DC RID: 1756
	public bool isTeamTable;

	// Token: 0x040006DD RID: 1757
	public GameObject scoreHead;

	// Token: 0x040006DE RID: 1758
	public GameObject countKillsHead;

	// Token: 0x040006DF RID: 1759
	public GameObject likeHead;

	// Token: 0x040006E0 RID: 1760
	public int command;

	// Token: 0x040006E1 RID: 1761
	public string nameCommand;

	// Token: 0x040006E2 RID: 1762
	public UISprite fon;

	// Token: 0x040006E3 RID: 1763
	public UISprite fonHead;

	// Token: 0x040006E4 RID: 1764
	public UISprite fonUndrhead;

	// Token: 0x040006E5 RID: 1765
	public UILabel headLabel;

	// Token: 0x040006E6 RID: 1766
	public UILabel[] undrheadLabels;
}
