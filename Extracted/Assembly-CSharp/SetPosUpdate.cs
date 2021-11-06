using System;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
public class SetPosUpdate : MonoBehaviour
{
	// Token: 0x060045DF RID: 17887 RVA: 0x00179D88 File Offset: 0x00177F88
	private void Start()
	{
		this.SetPos();
	}

	// Token: 0x060045E0 RID: 17888 RVA: 0x00179D90 File Offset: 0x00177F90
	private void Update()
	{
		this.SetPos();
	}

	// Token: 0x060045E1 RID: 17889 RVA: 0x00179D98 File Offset: 0x00177F98
	private void SetPos()
	{
		int num = this.index;
		if (num != 0)
		{
			if (num == 1)
			{
				base.transform.localPosition = new Vector3(-424f - (768f * (float)Screen.width / (float)Screen.height - 912f) / 2f, 44f, 0f);
			}
		}
		else if (MainMenu.SkinsMakerSupproted())
		{
			base.transform.localPosition = new Vector3(-385f - (768f * (float)Screen.width / (float)Screen.height - 976f) / 3f, 64f, 1f);
		}
		else
		{
			base.transform.localPosition = new Vector3(-124f, 64f, 0f);
		}
	}

	// Token: 0x0400333A RID: 13114
	public int index;
}
