using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class QuitOnEscapeOrBack : MonoBehaviour
{
	// Token: 0x06002732 RID: 10034 RVA: 0x000C4358 File Offset: 0x000C2558
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
