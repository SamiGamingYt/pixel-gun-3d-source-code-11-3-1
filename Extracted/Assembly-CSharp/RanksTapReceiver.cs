using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class RanksTapReceiver : MonoBehaviour
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06002B31 RID: 11057 RVA: 0x000E3E28 File Offset: 0x000E2028
	// (remove) Token: 0x06002B32 RID: 11058 RVA: 0x000E3E40 File Offset: 0x000E2040
	public static event Action RanksClicked;

	// Token: 0x06002B33 RID: 11059 RVA: 0x000E3E58 File Offset: 0x000E2058
	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x000E3E6C File Offset: 0x000E206C
	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if (RanksTapReceiver.RanksClicked != null)
		{
			RanksTapReceiver.RanksClicked();
		}
	}
}
