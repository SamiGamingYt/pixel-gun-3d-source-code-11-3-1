using System;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public class CoinsShopButton : MonoBehaviour
{
	// Token: 0x060032F9 RID: 13049 RVA: 0x00107DBC File Offset: 0x00105FBC
	private void Start()
	{
		PromoActionsManager.EventX3Updated += this.OnEventX3Updated;
		this.OnEventX3Updated();
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x00107DD8 File Offset: 0x00105FD8
	private void OnEnable()
	{
		this.OnEventX3Updated();
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x00107DE0 File Offset: 0x00105FE0
	private void OnDestroy()
	{
		PromoActionsManager.EventX3Updated -= this.OnEventX3Updated;
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x00107DF4 File Offset: 0x00105FF4
	private void OnEventX3Updated()
	{
		bool flag = PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active;
		if (this.eventX3 != null && this.eventX3.activeSelf != flag)
		{
			this.eventX3.SetActive(flag);
		}
	}

	// Token: 0x0400257F RID: 9599
	public GameObject eventX3;
}
