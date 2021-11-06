using System;
using UnityEngine;

// Token: 0x02000334 RID: 820
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06001C48 RID: 7240 RVA: 0x00075980 File Offset: 0x00073B80
	// (set) Token: 0x06001C49 RID: 7241 RVA: 0x000759B0 File Offset: 0x00073BB0
	public bool isEnabled
	{
		get
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			return component && component.enabled;
		}
		set
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			if (!component)
			{
				return;
			}
			if (component.enabled != value)
			{
				component.enabled = value;
				this.UpdateImage();
			}
		}
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000759F0 File Offset: 0x00073BF0
	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x00075A18 File Offset: 0x00073C18
	private void OnValidate()
	{
		if (this.target != null)
		{
			if (string.IsNullOrEmpty(this.normalSprite))
			{
				this.normalSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.hoverSprite))
			{
				this.hoverSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.pressedSprite))
			{
				this.pressedSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.disabledSprite))
			{
				this.disabledSprite = this.target.spriteName;
			}
		}
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x00075ABC File Offset: 0x00073CBC
	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.SetSprite((!UICamera.IsHighlighted(base.gameObject)) ? this.normalSprite : this.hoverSprite);
			}
			else
			{
				this.SetSprite(this.disabledSprite);
			}
		}
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x00075B20 File Offset: 0x00073D20
	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.SetSprite((!isOver) ? this.normalSprite : this.hoverSprite);
		}
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x00075B5C File Offset: 0x00073D5C
	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.SetSprite(this.pressedSprite);
		}
		else
		{
			this.UpdateImage();
		}
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x00075B7C File Offset: 0x00073D7C
	private void SetSprite(string sprite)
	{
		if (this.target.atlas == null || this.target.atlas.GetSprite(sprite) == null)
		{
			return;
		}
		this.target.spriteName = sprite;
		if (this.pixelSnap)
		{
			this.target.MakePixelPerfect();
		}
	}

	// Token: 0x04001172 RID: 4466
	public UISprite target;

	// Token: 0x04001173 RID: 4467
	public string normalSprite;

	// Token: 0x04001174 RID: 4468
	public string hoverSprite;

	// Token: 0x04001175 RID: 4469
	public string pressedSprite;

	// Token: 0x04001176 RID: 4470
	public string disabledSprite;

	// Token: 0x04001177 RID: 4471
	public bool pixelSnap = true;
}
