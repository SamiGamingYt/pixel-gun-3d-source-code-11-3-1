using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class CoinsLabel : MonoBehaviour
{
	// Token: 0x0600034E RID: 846 RVA: 0x0001C060 File Offset: 0x0001A260
	private void Start()
	{
		this.SetCountCoins();
	}

	// Token: 0x0600034F RID: 847 RVA: 0x0001C068 File Offset: 0x0001A268
	private void Update()
	{
		this.SetCountCoins();
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0001C070 File Offset: 0x0001A270
	private void SetCountCoins()
	{
		this.mylabel.text = "1234";
	}

	// Token: 0x04000393 RID: 915
	public UILabel mylabel;
}
