using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000351 RID: 849
[AddComponentMenu("NGUI/Interaction/Toggle")]
[ExecuteInEditMode]
public class UIToggle : UIWidgetContainer
{
	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06001D37 RID: 7479 RVA: 0x0007CBB4 File Offset: 0x0007ADB4
	// (set) Token: 0x06001D38 RID: 7480 RVA: 0x0007CBD4 File Offset: 0x0007ADD4
	public bool value
	{
		get
		{
			return (!this.mStarted) ? this.startsActive : this.mIsActive;
		}
		set
		{
			if (!this.mStarted)
			{
				this.startsActive = value;
			}
			else if (this.group == 0 || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06001D39 RID: 7481 RVA: 0x0007CC28 File Offset: 0x0007AE28
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

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0007CC6C File Offset: 0x0007AE6C
	// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0007CC74 File Offset: 0x0007AE74
	[Obsolete("Use 'value' instead")]
	public bool isChecked
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x0007CC80 File Offset: 0x0007AE80
	public static UIToggle GetActiveToggle(int group)
	{
		for (int i = 0; i < UIToggle.list.size; i++)
		{
			UIToggle uitoggle = UIToggle.list[i];
			if (uitoggle != null && uitoggle.group == group && uitoggle.mIsActive)
			{
				return uitoggle;
			}
		}
		return null;
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x0007CCDC File Offset: 0x0007AEDC
	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x0007CCEC File Offset: 0x0007AEEC
	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0007CCFC File Offset: 0x0007AEFC
	private void Start()
	{
		if (this.startsChecked)
		{
			this.startsChecked = false;
			this.startsActive = true;
		}
		if (!Application.isPlaying)
		{
			if (this.checkSprite != null && this.activeSprite == null)
			{
				this.activeSprite = this.checkSprite;
				this.checkSprite = null;
			}
			if (this.checkAnimation != null && this.activeAnimation == null)
			{
				this.activeAnimation = this.checkAnimation;
				this.checkAnimation = null;
			}
			if (Application.isPlaying && this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!this.startsActive) ? 0f : 1f);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				this.eventReceiver = null;
				this.functionName = null;
			}
		}
		else
		{
			this.mIsActive = !this.startsActive;
			this.mStarted = true;
			bool flag = this.instantTween;
			this.instantTween = true;
			this.Set(this.startsActive);
			this.instantTween = flag;
		}
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0007CE30 File Offset: 0x0007B030
	private void OnClick()
	{
		if (base.enabled && this.isColliderEnabled && UICamera.currentTouchID != -2)
		{
			this.value = !this.value;
		}
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x0007CE70 File Offset: 0x0007B070
	public void Set(bool state)
	{
		if (this.validator != null && !this.validator(state))
		{
			return;
		}
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.group != 0 && state)
			{
				int i = 0;
				int size = UIToggle.list.size;
				while (i < size)
				{
					UIToggle uitoggle = UIToggle.list[i];
					if (uitoggle != this && uitoggle.group == this.group)
					{
						uitoggle.Set(false);
					}
					if (UIToggle.list.size != size)
					{
						size = UIToggle.list.size;
						i = 0;
					}
					else
					{
						i++;
					}
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween || !NGUITools.GetActive(this))
				{
					this.activeSprite.alpha = ((!this.mIsActive) ? 0f : 1f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, (!this.mIsActive) ? 0f : 1f);
				}
			}
			if (UIToggle.current == null)
			{
				UIToggle uitoggle2 = UIToggle.current;
				UIToggle.current = this;
				if (EventDelegate.IsValid(this.onChange))
				{
					EventDelegate.Execute(this.onChange);
				}
				else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
				{
					this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
				}
				UIToggle.current = uitoggle2;
			}
			if (this.animator != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.animator, null, (!state) ? Direction.Reverse : Direction.Forward, EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation.Finish();
				}
			}
			else if (this.activeAnimation != null)
			{
				ActiveAnimation activeAnimation2 = ActiveAnimation.Play(this.activeAnimation, null, (!state) ? Direction.Reverse : Direction.Forward, EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation2 != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation2.Finish();
				}
			}
			else if (this.tween != null)
			{
				bool active = NGUITools.GetActive(this);
				if (this.tween.tweenGroup != 0)
				{
					UITweener[] componentsInChildren = this.tween.GetComponentsInChildren<UITweener>();
					int j = 0;
					int num = componentsInChildren.Length;
					while (j < num)
					{
						UITweener uitweener = componentsInChildren[j];
						if (uitweener.tweenGroup == this.tween.tweenGroup)
						{
							uitweener.Play(state);
							if (this.instantTween || !active)
							{
								uitweener.tweenFactor = ((!state) ? 0f : 1f);
							}
						}
						j++;
					}
				}
				else
				{
					this.tween.Play(state);
					if (this.instantTween || !active)
					{
						this.tween.tweenFactor = ((!state) ? 0f : 1f);
					}
				}
			}
		}
	}

	// Token: 0x0400126D RID: 4717
	public static BetterList<UIToggle> list = new BetterList<UIToggle>();

	// Token: 0x0400126E RID: 4718
	public static UIToggle current;

	// Token: 0x0400126F RID: 4719
	public int group;

	// Token: 0x04001270 RID: 4720
	public UIWidget activeSprite;

	// Token: 0x04001271 RID: 4721
	public Animation activeAnimation;

	// Token: 0x04001272 RID: 4722
	public Animator animator;

	// Token: 0x04001273 RID: 4723
	public UITweener tween;

	// Token: 0x04001274 RID: 4724
	public bool startsActive;

	// Token: 0x04001275 RID: 4725
	public bool instantTween;

	// Token: 0x04001276 RID: 4726
	public bool optionCanBeNone;

	// Token: 0x04001277 RID: 4727
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x04001278 RID: 4728
	public UIToggle.Validate validator;

	// Token: 0x04001279 RID: 4729
	[SerializeField]
	[HideInInspector]
	private UISprite checkSprite;

	// Token: 0x0400127A RID: 4730
	[SerializeField]
	[HideInInspector]
	private Animation checkAnimation;

	// Token: 0x0400127B RID: 4731
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x0400127C RID: 4732
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnActivate";

	// Token: 0x0400127D RID: 4733
	[HideInInspector]
	[SerializeField]
	private bool startsChecked;

	// Token: 0x0400127E RID: 4734
	private bool mIsActive = true;

	// Token: 0x0400127F RID: 4735
	private bool mStarted;

	// Token: 0x020008F5 RID: 2293
	// (Invoke) Token: 0x06005074 RID: 20596
	public delegate bool Validate(bool choice);
}
