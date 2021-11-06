using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class RePositionPlayersButton : MonoBehaviour
{
	// Token: 0x06002B7A RID: 11130 RVA: 0x000E525C File Offset: 0x000E345C
	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			base.transform.localPosition += this.positionInCommand;
		}
	}

	// Token: 0x04002083 RID: 8323
	public Vector3 positionInCommand;
}
