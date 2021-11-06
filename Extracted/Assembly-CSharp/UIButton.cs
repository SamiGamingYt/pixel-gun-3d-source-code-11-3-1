using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000319 RID: 793
[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00071134 File Offset: 0x0006F334
	// (set) Token: 0x06001B8C RID: 7052 RVA: 0x00071190 File Offset: 0x0006F390
	public override bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider component = base.gameObject.GetComponent<Collider>();
			if (component && component.enabled)
			{
				return true;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 && component2.enabled;
		}
		set
		{
			if (this.isEnabled != value)
			{
				Collider component = base.gameObject.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = value;
					UIButton[] components = base.GetComponents<UIButton>();
					foreach (UIButton uibutton in components)
					{
						uibutton.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
					}
				}
				else
				{
					Collider2D component2 = base.GetComponent<Collider2D>();
					if (component2 != null)
					{
						component2.enabled = value;
						UIButton[] components2 = base.GetComponents<UIButton>();
						foreach (UIButton uibutton2 in components2)
						{
							uibutton2.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
						}
					}
					else
					{
						base.enabled = value;
					}
				}
			}
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06001B8D RID: 7053 RVA: 0x00071274 File Offset: 0x0006F474
	// (set) Token: 0x06001B8E RID: 7054 RVA: 0x00071290 File Offset: 0x0006F490
	public string normalSprite
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite != null && !string.IsNullOrEmpty(this.mNormalSprite) && this.mNormalSprite == this.mSprite.spriteName)
			{
				this.mNormalSprite = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06001B8F RID: 7055 RVA: 0x00071324 File Offset: 0x0006F524
	// (set) Token: 0x06001B90 RID: 7056 RVA: 0x00071340 File Offset: 0x0006F540
	public Sprite normalSprite2D
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite2D;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite2D != null && this.mNormalSprite2D == this.mSprite2D.sprite2D)
			{
				this.mNormalSprite2D = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite2D = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000713C4 File Offset: 0x0006F5C4
	protected override void OnInit()
	{
		base.OnInit();
		this.mSprite = (this.mWidget as UISprite);
		this.mSprite2D = (this.mWidget as UI2DSprite);
		if (this.mSprite != null)
		{
			this.mNormalSprite = this.mSprite.spriteName;
		}
		if (this.mSprite2D != null)
		{
			this.mNormalSprite2D = this.mSprite2D.sprite2D;
		}
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x00071440 File Offset: 0x0006F640
	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mInitDone)
			{
				this.OnHover(UICamera.hoveredObject == base.gameObject);
			}
		}
		else
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x00071488 File Offset: 0x0006F688
	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOver();
		}
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x000714CC File Offset: 0x0006F6CC
	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOut();
		}
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x00071510 File Offset: 0x0006F710
	protected virtual void OnClick()
	{
		if (UIButton.current == null && this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x00071550 File Offset: 0x0006F750
	public override void SetState(UIButtonColor.State state, bool immediate)
	{
		base.SetState(state, immediate);
		if (this.mSprite != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!string.IsNullOrEmpty(this.hoverSprite)) ? this.hoverSprite : this.mNormalSprite);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite);
				break;
			}
		}
		else if (this.mSprite2D != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!(this.hoverSprite2D == null)) ? this.hoverSprite2D : this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite2D);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite2D);
				break;
			}
		}
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x00071688 File Offset: 0x0006F888
	protected void SetSprite(string sp)
	{
		if (this.mSprite != null && !string.IsNullOrEmpty(sp) && this.mSprite.spriteName != sp)
		{
			this.mSprite.spriteName = sp;
			if (this.pixelSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000716EC File Offset: 0x0006F8EC
	protected void SetSprite(Sprite sp)
	{
		if (sp != null && this.mSprite2D != null && this.mSprite2D.sprite2D != sp)
		{
			this.mSprite2D.sprite2D = sp;
			if (this.pixelSnap)
			{
				this.mSprite2D.MakePixelPerfect();
			}
		}
	}

	// Token: 0x040010A7 RID: 4263
	public static UIButton current;

	// Token: 0x040010A8 RID: 4264
	public bool dragHighlight;

	// Token: 0x040010A9 RID: 4265
	public string hoverSprite;

	// Token: 0x040010AA RID: 4266
	public string pressedSprite;

	// Token: 0x040010AB RID: 4267
	public string disabledSprite;

	// Token: 0x040010AC RID: 4268
	public Sprite hoverSprite2D;

	// Token: 0x040010AD RID: 4269
	public Sprite pressedSprite2D;

	// Token: 0x040010AE RID: 4270
	public Sprite disabledSprite2D;

	// Token: 0x040010AF RID: 4271
	public bool pixelSnap;

	// Token: 0x040010B0 RID: 4272
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x040010B1 RID: 4273
	[NonSerialized]
	private UISprite mSprite;

	// Token: 0x040010B2 RID: 4274
	[NonSerialized]
	private UI2DSprite mSprite2D;

	// Token: 0x040010B3 RID: 4275
	[NonSerialized]
	private string mNormalSprite;

	// Token: 0x040010B4 RID: 4276
	[NonSerialized]
	private Sprite mNormalSprite2D;
}
