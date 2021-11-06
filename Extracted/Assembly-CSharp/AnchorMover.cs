using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class AnchorMover : MonoBehaviour
{
	// Token: 0x06000054 RID: 84 RVA: 0x0000488C File Offset: 0x00002A8C
	private void SetSide()
	{
		this.SetSideCoroutine();
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004894 File Offset: 0x00002A94
	private void SetSideCoroutine()
	{
		this.flag1.localPosition = new Vector3((float)((!GlobalGameController.LeftHanded) ? 1 : -1) * ((float)Screen.width * 768f / (float)Screen.height / 2f - 30f), this.flag1.localPosition.y, this.flag1.localPosition.z);
		this.flag2.localPosition = new Vector3((float)((!GlobalGameController.LeftHanded) ? 1 : -1) * ((float)Screen.width * 768f / (float)Screen.height / 2f - 30f), this.flag2.localPosition.y, this.flag2.localPosition.z);
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004974 File Offset: 0x00002B74
	private void Start()
	{
		this.SetSide();
		PauseNGUIController.PlayerHandUpdated += this.SetSide;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004990 File Offset: 0x00002B90
	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= this.SetSide;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000049A4 File Offset: 0x00002BA4
	private void OnEnable()
	{
		this.SetSideCoroutine();
	}

	// Token: 0x04000060 RID: 96
	public Transform flag1;

	// Token: 0x04000061 RID: 97
	public Transform flag2;
}
