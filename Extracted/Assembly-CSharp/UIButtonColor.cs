using System;
using UnityEngine;

// Token: 0x0200031B RID: 795
[AddComponentMenu("NGUI/Interaction/Button Color")]
[ExecuteInEditMode]
public class UIButtonColor : UIWidgetContainer
{
	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000717F8 File Offset: 0x0006F9F8
	// (set) Token: 0x06001B9D RID: 7069 RVA: 0x00071800 File Offset: 0x0006FA00
	public UIButtonColor.State state
	{
		get
		{
			return this.mState;
		}
		set
		{
			this.SetState(value, false);
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06001B9E RID: 7070 RVA: 0x0007180C File Offset: 0x0006FA0C
	// (set) Token: 0x06001B9F RID: 7071 RVA: 0x00071828 File Offset: 0x0006FA28
	public Color defaultColor
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mDefaultColor;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			this.mDefaultColor = value;
			UIButtonColor.State state = this.mState;
			this.mState = UIButtonColor.State.Disabled;
			this.SetState(state, false);
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x00071864 File Offset: 0x0006FA64
	// (set) Token: 0x06001BA1 RID: 7073 RVA: 0x0007186C File Offset: 0x0006FA6C
	public virtual bool isEnabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x00071878 File Offset: 0x0006FA78
	public void ResetDefaultColor()
	{
		this.defaultColor = this.mStartingColor;
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00071888 File Offset: 0x0006FA88
	public void CacheDefaultColor()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x0007189C File Offset: 0x0006FA9C
	private void Start()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
		if (!this.isEnabled)
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x000718D0 File Offset: 0x0006FAD0
	protected virtual void OnInit()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null && !Application.isPlaying)
		{
			this.tweenTarget = base.gameObject;
		}
		if (this.tweenTarget != null)
		{
			this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
		}
		if (this.mWidget != null)
		{
			this.mDefaultColor = this.mWidget.color;
			this.mStartingColor = this.mDefaultColor;
		}
		else if (this.tweenTarget != null)
		{
			Renderer component = this.tweenTarget.GetComponent<Renderer>();
			if (component != null)
			{
				this.mDefaultColor = ((!Application.isPlaying) ? component.sharedMaterial.color : component.material.color);
				this.mStartingColor = this.mDefaultColor;
			}
			else
			{
				Light component2 = this.tweenTarget.GetComponent<Light>();
				if (component2 != null)
				{
					this.mDefaultColor = component2.color;
					this.mStartingColor = this.mDefaultColor;
				}
				else
				{
					this.tweenTarget = null;
					this.mInitDone = false;
				}
			}
		}
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x00071A08 File Offset: 0x0006FC08
	protected virtual void OnEnable()
	{
		if (this.mInitDone)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (UICamera.currentTouch.pressed == base.gameObject)
			{
				this.OnPress(true);
			}
			else if (UICamera.currentTouch.current == base.gameObject)
			{
				this.OnHover(true);
			}
		}
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x00071A84 File Offset: 0x0006FC84
	protected virtual void OnDisable()
	{
		if (this.mInitDone && this.tweenTarget != null)
		{
			this.SetState(UIButtonColor.State.Normal, true);
			TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.value = this.mDefaultColor;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x00071AE0 File Offset: 0x0006FCE0
	protected virtual void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState((!isOver) ? UIButtonColor.State.Normal : UIButtonColor.State.Hover, false);
			}
		}
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x00071B30 File Offset: 0x0006FD30
	protected virtual void OnPress(bool isPressed)
	{
		if (this.isEnabled && UICamera.currentTouch != null)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				if (isPressed)
				{
					this.SetState(UIButtonColor.State.Pressed, false);
				}
				else if (UICamera.currentTouch.current == base.gameObject)
				{
					if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
					{
						this.SetState(UIButtonColor.State.Hover, false);
					}
					else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == base.gameObject)
					{
						this.SetState(UIButtonColor.State.Hover, false);
					}
					else
					{
						this.SetState(UIButtonColor.State.Normal, false);
					}
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
	}

	// Token: 0x06001BAA RID: 7082 RVA: 0x00071BFC File Offset: 0x0006FDFC
	protected virtual void OnDragOver()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Pressed, false);
			}
		}
	}

	// Token: 0x06001BAB RID: 7083 RVA: 0x00071C40 File Offset: 0x0006FE40
	protected virtual void OnDragOut()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Normal, false);
			}
		}
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x00071C84 File Offset: 0x0006FE84
	public virtual void SetState(UIButtonColor.State state, bool instant)
	{
		if (!this.mInitDone)
		{
			this.mInitDone = true;
			this.OnInit();
		}
		if (this.mState != state)
		{
			this.mState = state;
			this.UpdateColor(instant);
		}
	}

	// Token: 0x06001BAD RID: 7085 RVA: 0x00071CC4 File Offset: 0x0006FEC4
	public void UpdateColor(bool instant)
	{
		if (this.tweenTarget != null)
		{
			TweenColor tweenColor;
			switch (this.mState)
			{
			case UIButtonColor.State.Hover:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
				break;
			case UIButtonColor.State.Pressed:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
				break;
			case UIButtonColor.State.Disabled:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.disabledColor);
				break;
			default:
				tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.mDefaultColor);
				break;
			}
			if (instant && tweenColor != null)
			{
				tweenColor.value = tweenColor.to;
				tweenColor.enabled = false;
			}
		}
	}

	// Token: 0x040010B7 RID: 4279
	public GameObject tweenTarget;

	// Token: 0x040010B8 RID: 4280
	public Color hover = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x040010B9 RID: 4281
	public Color pressed = new Color(0.7176471f, 0.6392157f, 0.48235294f, 1f);

	// Token: 0x040010BA RID: 4282
	public Color disabledColor = Color.grey;

	// Token: 0x040010BB RID: 4283
	public float duration = 0.2f;

	// Token: 0x040010BC RID: 4284
	[NonSerialized]
	protected Color mStartingColor;

	// Token: 0x040010BD RID: 4285
	[NonSerialized]
	protected Color mDefaultColor;

	// Token: 0x040010BE RID: 4286
	[NonSerialized]
	protected bool mInitDone;

	// Token: 0x040010BF RID: 4287
	[NonSerialized]
	protected UIWidget mWidget;

	// Token: 0x040010C0 RID: 4288
	[NonSerialized]
	protected UIButtonColor.State mState;

	// Token: 0x0200031C RID: 796
	public enum State
	{
		// Token: 0x040010C2 RID: 4290
		Normal,
		// Token: 0x040010C3 RID: 4291
		Hover,
		// Token: 0x040010C4 RID: 4292
		Pressed,
		// Token: 0x040010C5 RID: 4293
		Disabled
	}
}
