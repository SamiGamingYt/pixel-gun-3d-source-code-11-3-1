using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class EasterEggScript : MonoBehaviour
{
	// Token: 0x060004D0 RID: 1232 RVA: 0x000274E0 File Offset: 0x000256E0
	private void Start()
	{
		if (DateTime.Now.Hour < 23 && DateTime.Now.Minute < 55)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
