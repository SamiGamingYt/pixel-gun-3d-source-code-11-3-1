using System;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class NetworkStartTableOnGUI : MonoBehaviour
{
	// Token: 0x06002327 RID: 8999 RVA: 0x000AE66C File Offset: 0x000AC86C
	private void Start()
	{
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000AE670 File Offset: 0x000AC870
	private void Update()
	{
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000AE674 File Offset: 0x000AC874
	private void OnGUI()
	{
		this.myTable.MyOnGUI();
	}

	// Token: 0x04001766 RID: 5990
	public NetworkStartTable myTable;
}
