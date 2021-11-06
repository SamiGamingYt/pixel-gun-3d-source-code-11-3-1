using System;
using UnityEngine;

// Token: 0x0200034B RID: 843
[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
[ExecuteInEditMode]
public class UISlider : UIProgressBar
{
	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06001D1C RID: 7452 RVA: 0x0007BFE8 File Offset: 0x0007A1E8
	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06001D1D RID: 7453 RVA: 0x0007C02C File Offset: 0x0007A22C
	// (set) Token: 0x06001D1E RID: 7454 RVA: 0x0007C034 File Offset: 0x0007A234
	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0007C040 File Offset: 0x0007A240
	// (set) Token: 0x06001D20 RID: 7456 RVA: 0x0007C048 File Offset: 0x0007A248
	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x0007C04C File Offset: 0x0007A24C
	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x0007C0DC File Offset: 0x0007A2DC
	protected override void OnStart()
	{
		GameObject go = (!(this.mBG != null) || (!(this.mBG.GetComponent<Collider>() != null) && !(this.mBG.GetComponent<Collider2D>() != null))) ? base.gameObject : this.mBG.gameObject;
		UIEventListener uieventListener = UIEventListener.Get(go);
		UIEventListener uieventListener2 = uieventListener;
		uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener uieventListener3 = uieventListener;
		uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uieventListener4 = UIEventListener.Get(this.thumb.gameObject);
			UIEventListener uieventListener5 = uieventListener4;
			uieventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener uieventListener6 = uieventListener4;
			uieventListener6.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener6.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x0007C248 File Offset: 0x0007A448
	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x0007C29C File Offset: 0x0007A49C
	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastEventPosition);
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x0007C2D4 File Offset: 0x0007A4D4
	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = ((!(this.mFG == null)) ? (base.value - base.ScreenToValue(UICamera.lastEventPosition)) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x0007C34C File Offset: 0x0007A54C
	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastEventPosition);
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x0007C380 File Offset: 0x0007A580
	public override void OnPan(Vector2 delta)
	{
		if (base.enabled && this.isColliderEnabled)
		{
			base.OnPan(delta);
		}
	}

	// Token: 0x0400124F RID: 4687
	[SerializeField]
	[HideInInspector]
	private Transform foreground;

	// Token: 0x04001250 RID: 4688
	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	// Token: 0x04001251 RID: 4689
	[SerializeField]
	[HideInInspector]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	// Token: 0x04001252 RID: 4690
	[HideInInspector]
	[SerializeField]
	protected bool mInverted;

	// Token: 0x0200034C RID: 844
	private enum Direction
	{
		// Token: 0x04001254 RID: 4692
		Horizontal,
		// Token: 0x04001255 RID: 4693
		Vertical,
		// Token: 0x04001256 RID: 4694
		Upgraded
	}
}
