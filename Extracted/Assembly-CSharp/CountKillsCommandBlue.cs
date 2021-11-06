using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class CountKillsCommandBlue : MonoBehaviour
{
	// Token: 0x06000401 RID: 1025 RVA: 0x00023154 File Offset: 0x00021354
	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		InGameGUI sharedInGameGUI = InGameGUI.sharedInGameGUI;
		this._label = base.GetComponent<UILabel>();
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00023180 File Offset: 0x00021380
	private void Update()
	{
		if (this._weaponManager && this._weaponManager.myPlayer)
		{
			string text = "0";
			bool flag = false;
			if (Defs.isFlag)
			{
				if (WeaponManager.sharedManager.myTable != null)
				{
					if (this.isEnemyCommandLabel == (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2))
					{
						text = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1.ToString();
						flag = (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2);
					}
					else
					{
						text = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2.ToString();
						flag = (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1);
					}
				}
			}
			else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				if (this.isEnemyCommandLabel == (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2))
				{
					text = Mathf.RoundToInt(CapturePointController.sharedController.scoreBlue).ToString();
					flag = (CapturePointController.sharedController.scoreBlue > CapturePointController.sharedController.scoreRed);
				}
				else
				{
					text = Mathf.RoundToInt(CapturePointController.sharedController.scoreRed).ToString();
					flag = (CapturePointController.sharedController.scoreRed > CapturePointController.sharedController.scoreBlue);
				}
			}
			else if (this.isEnemyCommandLabel == (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2))
			{
				text = this._weaponManager.myPlayerMoveC.countKillsCommandBlue.ToString();
				flag = (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed);
			}
			else
			{
				text = this._weaponManager.myPlayerMoveC.countKillsCommandRed.ToString();
				flag = (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue);
			}
			this._label.text = text;
			this._label.color = ((!flag) ? Color.white : this.goldColor);
		}
	}

	// Token: 0x04000494 RID: 1172
	public static float localScaleForLabels = 1.25f;

	// Token: 0x04000495 RID: 1173
	private UILabel _label;

	// Token: 0x04000496 RID: 1174
	public bool isEnemyCommandLabel;

	// Token: 0x04000497 RID: 1175
	private WeaponManager _weaponManager;

	// Token: 0x04000498 RID: 1176
	public GameObject myBackground;

	// Token: 0x04000499 RID: 1177
	private Color goldColor = new Color(1f, 1f, 0f);
}
