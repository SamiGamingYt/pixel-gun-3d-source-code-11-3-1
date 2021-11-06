using System;
using UnityEngine;

// Token: 0x02000321 RID: 801
[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	// Token: 0x06001BC4 RID: 7108 RVA: 0x00072374 File Offset: 0x00070574
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mRot = this.tweenTarget.localRotation;
		}
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x000723C4 File Offset: 0x000705C4
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000723E4 File Offset: 0x000705E4
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.value = this.mRot;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x00072438 File Offset: 0x00070638
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))) : (this.mRot * Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000724D0 File Offset: 0x000706D0
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x0007253C File Offset: 0x0007073C
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040010DE RID: 4318
	public Transform tweenTarget;

	// Token: 0x040010DF RID: 4319
	public Vector3 hover = Vector3.zero;

	// Token: 0x040010E0 RID: 4320
	public Vector3 pressed = Vector3.zero;

	// Token: 0x040010E1 RID: 4321
	public float duration = 0.2f;

	// Token: 0x040010E2 RID: 4322
	private Quaternion mRot;

	// Token: 0x040010E3 RID: 4323
	private bool mStarted;
}
