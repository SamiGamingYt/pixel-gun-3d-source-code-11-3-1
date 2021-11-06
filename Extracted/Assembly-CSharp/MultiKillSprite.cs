using System;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class MultiKillSprite : MonoBehaviour
{
	// Token: 0x06001B57 RID: 6999 RVA: 0x00070368 File Offset: 0x0006E568
	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, -195f, base.transform.localPosition.z);
		}
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000703BC File Offset: 0x0006E5BC
	private void Update()
	{
	}
}
