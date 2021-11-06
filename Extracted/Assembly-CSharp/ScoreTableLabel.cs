using System;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class ScoreTableLabel : MonoBehaviour
{
	// Token: 0x06002C80 RID: 11392 RVA: 0x000EBF4C File Offset: 0x000EA14C
	private void Start()
	{
		if (Defs.isCOOP)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0190");
		}
		else if (Defs.isFlag)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1006");
		}
		else
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0191");
		}
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000EBFB8 File Offset: 0x000EA1B8
	private void Update()
	{
	}
}
