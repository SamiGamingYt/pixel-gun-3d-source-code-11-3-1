using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class DestroyChecker : MonoBehaviour
{
	// Token: 0x06000445 RID: 1093 RVA: 0x00024820 File Offset: 0x00022A20
	private void OnDestroy()
	{
		Debug.Log("Destroy");
	}
}
