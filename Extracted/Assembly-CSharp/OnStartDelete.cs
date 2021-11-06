using System;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class OnStartDelete : MonoBehaviour
{
	// Token: 0x060026DF RID: 9951 RVA: 0x000C2E34 File Offset: 0x000C1034
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
