using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E3 RID: 739
[DisallowMultipleComponent]
internal sealed class KillsLabel : MonoBehaviour
{
	// Token: 0x060019CE RID: 6606 RVA: 0x00068000 File Offset: 0x00066200
	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch || Defs.isDaterRegim));
		this._label = base.GetComponent<UILabel>();
		this._inGameGUI = InGameGUI.sharedInGameGUI;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x0006805C File Offset: 0x0006625C
	private void Update()
	{
		if (this._inGameGUI && this._label)
		{
			if (Defs.isDaterRegim || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel)
			{
				this._label.text = this.GetKillCountString(GlobalGameController.CountKills);
			}
			else if (this._inGameGUI != null)
			{
				this._label.text = this._inGameGUI.killsToMaxKills();
			}
		}
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000680E8 File Offset: 0x000662E8
	private string GetKillCountString(int killCount)
	{
		if (killCount != this._killCountMemo.Key)
		{
			string value = killCount.ToString();
			this._killCountMemo = new KeyValuePair<int, string>(killCount, value);
		}
		return this._killCountMemo.Value;
	}

	// Token: 0x04000F23 RID: 3875
	private UILabel _label;

	// Token: 0x04000F24 RID: 3876
	private InGameGUI _inGameGUI;

	// Token: 0x04000F25 RID: 3877
	private KeyValuePair<int, string> _killCountMemo = new KeyValuePair<int, string>(0, "0");
}
