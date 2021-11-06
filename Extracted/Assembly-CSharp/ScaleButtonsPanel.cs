using System;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class ScaleButtonsPanel : MonoBehaviour
{
	// Token: 0x06002C78 RID: 11384 RVA: 0x000EBC88 File Offset: 0x000E9E88
	private void Start()
	{
		if ((double)((float)Screen.width / (float)Screen.height) > 1.5)
		{
			base.transform.localScale = new Vector3(0.89f, 0.89f, 1f);
		}
	}
}
