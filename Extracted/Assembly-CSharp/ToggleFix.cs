using System;
using UnityEngine;

// Token: 0x02000863 RID: 2147
public class ToggleFix : MonoBehaviour
{
	// Token: 0x06004D94 RID: 19860 RVA: 0x001C0B38 File Offset: 0x001BED38
	private void Start()
	{
		this.button = base.GetComponent<UIButton>();
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001C0B48 File Offset: 0x001BED48
	private void Update()
	{
		if (this.button.state != UIButtonColor.State.Pressed)
		{
			this.checkmark.color = new Color(this.checkmark.color.r, this.checkmark.color.g, this.checkmark.color.b, (!this.toggle.value) ? 0f : 1f);
			this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, (!this.toggle.value) ? 1f : 0f);
		}
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001C0C38 File Offset: 0x001BEE38
	private void OnPress()
	{
		this.checkmark.color = new Color(this.checkmark.color.r, this.checkmark.color.g, this.checkmark.color.b, 0f);
		this.background.color = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 1f);
	}

	// Token: 0x04003C05 RID: 15365
	public UIToggle toggle;

	// Token: 0x04003C06 RID: 15366
	public UIButton button;

	// Token: 0x04003C07 RID: 15367
	public UISprite background;

	// Token: 0x04003C08 RID: 15368
	public UISprite checkmark;

	// Token: 0x04003C09 RID: 15369
	public bool oldState;

	// Token: 0x04003C0A RID: 15370
	public bool firstUpdate = true;
}
