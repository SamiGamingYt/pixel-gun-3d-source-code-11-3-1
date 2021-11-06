using System;
using UnityEngine;

// Token: 0x0200085F RID: 2143
public class TimeLeftInMatch : MonoBehaviour
{
	// Token: 0x06004D7B RID: 19835 RVA: 0x001C0520 File Offset: 0x001BE720
	private void Start()
	{
		if (ConnectSceneNGUIController.curSelectMode == TypeModeGame.Deathmatch || ConnectSceneNGUIController.curSelectMode == TypeModeGame.TimeBattle)
		{
			base.transform.localPosition += Vector3.up * 53f;
		}
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x001C0568 File Offset: 0x001BE768
	private void Update()
	{
		bool flag = Initializer.players.Count > 0 && !Defs.isDaterRegim && !Defs.isHunger && TimeGameController.sharedController != null && TimeGameController.sharedController.timerToEndMatch > 0.0 && !TimeGameController.sharedController.isEndMatch && TimeGameController.sharedController.timerToEndMatch < 1200.0;
		this.waitLabel.SetActive(flag && TimeGameController.sharedController.timerToEndMatch < 16.0);
		this.timeLabel.gameObject.SetActive(flag);
		this.timeLabel.transform.parent.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		float timeDown = (float)TimeGameController.sharedController.timerToEndMatch;
		this.timeLabel.text = Player_move_c.FormatTime(timeDown);
		this.timeLabel.color = ((!this.waitLabel.activeSelf) ? Color.white : Color.red);
	}

	// Token: 0x04003BF4 RID: 15348
	public UILabel timeLabel;

	// Token: 0x04003BF5 RID: 15349
	public GameObject waitLabel;
}
