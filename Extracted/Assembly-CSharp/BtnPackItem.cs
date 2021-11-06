using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class BtnPackItem : MonoBehaviour
{
	// Token: 0x0600304D RID: 12365 RVA: 0x000FC1A8 File Offset: 0x000FA3A8
	private void Awake()
	{
		this.scrollPack = base.GetComponentInParent<StickerPackScroll>();
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x000FC1B8 File Offset: 0x000FA3B8
	private void OnClick()
	{
		if (this.scrollPack)
		{
			ButtonClickSound.Instance.PlayClick();
			this.scrollPack.ShowPack(this.typePack);
		}
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x000FC1E8 File Offset: 0x000FA3E8
	public void ShowPack()
	{
		if (this.objListSticker)
		{
			this.objListSticker.SetActive(true);
			UIGrid component = this.objListSticker.GetComponent<UIGrid>();
			if (component != null)
			{
				component.Reposition();
			}
		}
		if (this.activeState)
		{
			this.activeState.SetActive(true);
		}
		if (this.noActiveState)
		{
			this.noActiveState.SetActive(false);
		}
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x000FC268 File Offset: 0x000FA468
	public void HidePack()
	{
		if (this.objListSticker)
		{
			this.objListSticker.SetActive(false);
		}
		if (this.activeState)
		{
			this.activeState.SetActive(false);
		}
		if (this.noActiveState)
		{
			this.noActiveState.SetActive(true);
		}
	}

	// Token: 0x0400237F RID: 9087
	public TypePackSticker typePack;

	// Token: 0x04002380 RID: 9088
	public GameObject objListSticker;

	// Token: 0x04002381 RID: 9089
	public GameObject activeState;

	// Token: 0x04002382 RID: 9090
	public GameObject noActiveState;

	// Token: 0x04002383 RID: 9091
	private StickerPackScroll scrollPack;
}
