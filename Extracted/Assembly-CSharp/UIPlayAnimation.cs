using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x0200033A RID: 826
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Animation")]
public class UIPlayAnimation : MonoBehaviour
{
	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06001C6E RID: 7278 RVA: 0x00076660 File Offset: 0x00074860
	private bool dualState
	{
		get
		{
			return this.trigger == Trigger.OnPress || this.trigger == Trigger.OnHover;
		}
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x0007667C File Offset: 0x0007487C
	private void Awake()
	{
		UIButton component = base.GetComponent<UIButton>();
		if (component != null)
		{
			this.dragHighlight = component.dragHighlight;
		}
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x000766D8 File Offset: 0x000748D8
	private void Start()
	{
		this.mStarted = true;
		if (this.target == null && this.animator == null)
		{
			this.animator = base.GetComponentInChildren<Animator>();
		}
		if (this.animator != null)
		{
			if (this.animator.enabled)
			{
				this.animator.enabled = false;
			}
			return;
		}
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<Animation>();
		}
		if (this.target != null && this.target.enabled)
		{
			this.target.enabled = false;
		}
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x00076794 File Offset: 0x00074994
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (this.trigger == Trigger.OnPress || this.trigger == Trigger.OnPressTrue)
			{
				this.mActivated = (UICamera.currentTouch.pressed == base.gameObject);
			}
			if (this.trigger == Trigger.OnHover || this.trigger == Trigger.OnHoverTrue)
			{
				this.mActivated = (UICamera.currentTouch.current == base.gameObject);
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x00076858 File Offset: 0x00074A58
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x00076890 File Offset: 0x00074A90
	private void OnHover(bool isOver)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
		{
			this.Play(isOver, this.dualState);
		}
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x000768E8 File Offset: 0x00074AE8
	private void OnPress(bool isPressed)
	{
		if (!base.enabled)
		{
			return;
		}
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed))
		{
			this.Play(isPressed, this.dualState);
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x00076958 File Offset: 0x00074B58
	private void OnClick()
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x00076994 File Offset: 0x00074B94
	private void OnDoubleClick()
	{
		if (UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000769D0 File Offset: 0x00074BD0
	private void OnSelect(bool isSelected)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected))
		{
			this.Play(isSelected, this.dualState);
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x00076A2C File Offset: 0x00074C2C
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value, this.dualState);
		}
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x00076AB0 File Offset: 0x00074CB0
	private void OnDragOver()
	{
		if (base.enabled && this.dualState)
		{
			if (UICamera.currentTouch.dragged == base.gameObject)
			{
				this.Play(true, true);
			}
			else if (this.dragHighlight && this.trigger == Trigger.OnPress)
			{
				this.Play(true, true);
			}
		}
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x00076B1C File Offset: 0x00074D1C
	private void OnDragOut()
	{
		if (base.enabled && this.dualState && UICamera.hoveredObject != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x00076B5C File Offset: 0x00074D5C
	private void OnDrop(GameObject go)
	{
		if (base.enabled && this.trigger == Trigger.OnPress && UICamera.currentTouch.dragged != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00076BA4 File Offset: 0x00074DA4
	public void Play(bool forward)
	{
		this.Play(forward, true);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x00076BB0 File Offset: 0x00074DB0
	public void Play(bool forward, bool onlyIfDifferent)
	{
		if (this.target || this.animator)
		{
			if (onlyIfDifferent)
			{
				if (this.mActivated == forward)
				{
					return;
				}
				this.mActivated = forward;
			}
			if (this.clearSelection && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			int num = (int)(-(int)this.playDirection);
			Direction direction = (Direction)((!forward) ? num : ((int)this.playDirection));
			ActiveAnimation activeAnimation = (!this.target) ? ActiveAnimation.Play(this.animator, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
			if (activeAnimation != null)
			{
				if (this.resetOnPlay)
				{
					activeAnimation.Reset();
				}
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
			}
		}
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x00076CDC File Offset: 0x00074EDC
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00076CE8 File Offset: 0x00074EE8
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00076CF4 File Offset: 0x00074EF4
	private void OnFinished()
	{
		if (UIPlayAnimation.current == null)
		{
			UIPlayAnimation.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			this.eventReceiver = null;
			UIPlayAnimation.current = null;
		}
	}

	// Token: 0x04001199 RID: 4505
	public static UIPlayAnimation current;

	// Token: 0x0400119A RID: 4506
	public Animation target;

	// Token: 0x0400119B RID: 4507
	public Animator animator;

	// Token: 0x0400119C RID: 4508
	public string clipName;

	// Token: 0x0400119D RID: 4509
	public Trigger trigger;

	// Token: 0x0400119E RID: 4510
	public Direction playDirection = Direction.Forward;

	// Token: 0x0400119F RID: 4511
	public bool resetOnPlay;

	// Token: 0x040011A0 RID: 4512
	public bool clearSelection;

	// Token: 0x040011A1 RID: 4513
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x040011A2 RID: 4514
	public DisableCondition disableWhenFinished;

	// Token: 0x040011A3 RID: 4515
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x040011A4 RID: 4516
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x040011A5 RID: 4517
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x040011A6 RID: 4518
	private bool mStarted;

	// Token: 0x040011A7 RID: 4519
	private bool mActivated;

	// Token: 0x040011A8 RID: 4520
	private bool dragHighlight;
}
