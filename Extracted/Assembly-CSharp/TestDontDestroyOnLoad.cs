using System;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class TestDontDestroyOnLoad : MonoBehaviour
{
	// Token: 0x0600274F RID: 10063 RVA: 0x000C4B08 File Offset: 0x000C2D08
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
