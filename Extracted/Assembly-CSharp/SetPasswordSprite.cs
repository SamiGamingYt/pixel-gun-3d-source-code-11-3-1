using System;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class SetPasswordSprite : MonoBehaviour
{
	// Token: 0x060045D9 RID: 17881 RVA: 0x00179B8C File Offset: 0x00177D8C
	private void Update()
	{
		if (string.IsNullOrEmpty(this.input.value))
		{
			if (!this.openSprite.activeSelf)
			{
				this.openSprite.SetActive(true);
				this.closeSprite.SetActive(false);
			}
		}
		else if (this.openSprite.activeSelf)
		{
			this.openSprite.SetActive(false);
			this.closeSprite.SetActive(true);
		}
	}

	// Token: 0x04003335 RID: 13109
	public UIInput input;

	// Token: 0x04003336 RID: 13110
	public GameObject openSprite;

	// Token: 0x04003337 RID: 13111
	public GameObject closeSprite;
}
