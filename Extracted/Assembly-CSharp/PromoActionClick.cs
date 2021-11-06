using System;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class PromoActionClick : MonoBehaviour
{
	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06002A4E RID: 10830 RVA: 0x000DFEA4 File Offset: 0x000DE0A4
	// (remove) Token: 0x06002A4F RID: 10831 RVA: 0x000DFEBC File Offset: 0x000DE0BC
	public static event Action<string> Click;

	// Token: 0x06002A50 RID: 10832 RVA: 0x000DFED4 File Offset: 0x000DE0D4
	private void OnPress(bool down)
	{
		if (down)
		{
			if (base.transform.parent.GetComponent<PromoActionPreview>().pressed != null)
			{
				base.transform.parent.GetComponent<PromoActionPreview>().icon.mainTexture = base.transform.parent.GetComponent<PromoActionPreview>().pressed;
			}
		}
		else if (base.transform.parent.GetComponent<PromoActionPreview>().unpressed != null)
		{
			base.transform.parent.GetComponent<PromoActionPreview>().icon.mainTexture = base.transform.parent.GetComponent<PromoActionPreview>().unpressed;
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000DFF8C File Offset: 0x000DE18C
	private void OnClick()
	{
		if (PromoActionClick.Click != null)
		{
			PromoActionClick.Click(base.transform.parent.GetComponent<PromoActionPreview>().tg);
		}
		Debug.Log("Click");
	}
}
