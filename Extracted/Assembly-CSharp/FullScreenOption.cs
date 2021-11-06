using System;
using UnityEngine;

// Token: 0x0200084F RID: 2127
[AddComponentMenu("Common/Full Screen Option")]
public class FullScreenOption : MonoBehaviour
{
	// Token: 0x06004D1E RID: 19742 RVA: 0x001BD128 File Offset: 0x001BB328
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			if (Screen.fullScreen)
			{
				Screen.SetResolution(1280, 720, false);
			}
			else
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
		}
	}
}
