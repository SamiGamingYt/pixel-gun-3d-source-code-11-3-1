using System;
using UnityEngine;

// Token: 0x020007CF RID: 1999
[ExecuteInEditMode]
public class SortOrder : MonoBehaviour
{
	// Token: 0x060048A7 RID: 18599 RVA: 0x00193318 File Offset: 0x00191518
	private void Start()
	{
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x0019331C File Offset: 0x0019151C
	private void Update()
	{
		base.GetComponent<Renderer>().sortingOrder = this.sortOrder;
	}

	// Token: 0x0400358E RID: 13710
	public int sortOrder;
}
