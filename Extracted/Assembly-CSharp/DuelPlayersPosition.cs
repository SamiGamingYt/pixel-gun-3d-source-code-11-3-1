using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class DuelPlayersPosition : MonoBehaviour
{
	// Token: 0x060004BC RID: 1212 RVA: 0x00026D18 File Offset: 0x00024F18
	private void Start()
	{
		this.firstPlayerStartPosition = this.firstPlayer.localPosition;
		this.secondPlayerStartPosition = this.secondPlayer.localPosition;
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00026D48 File Offset: 0x00024F48
	private void Update()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = num - 1.3333334f;
		this.firstPlayer.localPosition = this.firstPlayerStartPosition * (1f + num2);
		this.secondPlayer.localPosition = this.secondPlayerStartPosition * (1f + num2);
	}

	// Token: 0x0400051E RID: 1310
	public Transform firstPlayer;

	// Token: 0x0400051F RID: 1311
	public Transform secondPlayer;

	// Token: 0x04000520 RID: 1312
	private Vector3 firstPlayerStartPosition;

	// Token: 0x04000521 RID: 1313
	private Vector3 secondPlayerStartPosition;
}
