using System;
using UnityEngine;

// Token: 0x020007CD RID: 1997
public sealed class SkipPresser : MonoBehaviour
{
	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x060048A1 RID: 18593 RVA: 0x00193234 File Offset: 0x00191434
	// (remove) Token: 0x060048A2 RID: 18594 RVA: 0x0019324C File Offset: 0x0019144C
	public static event Action SkipPressed;

	// Token: 0x060048A3 RID: 18595 RVA: 0x00193264 File Offset: 0x00191464
	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400358B RID: 13707
	public GameObject windowAnchor;
}
