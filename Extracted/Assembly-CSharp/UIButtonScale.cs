using System;
using UnityEngine;

// Token: 0x02000322 RID: 802
[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	// Token: 0x06001BCB RID: 7115 RVA: 0x000725B8 File Offset: 0x000707B8
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mScale = this.tweenTarget.localScale;
		}
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x00072608 File Offset: 0x00070808
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x00072628 File Offset: 0x00070828
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.value = this.mScale;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x0007267C File Offset: 0x0007087C
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mScale : Vector3.Scale(this.mScale, this.hover)) : Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x0007270C File Offset: 0x0007090C
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mScale : Vector3.Scale(this.mScale, this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00072774 File Offset: 0x00070974
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040010E4 RID: 4324
	public Transform tweenTarget;

	// Token: 0x040010E5 RID: 4325
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	// Token: 0x040010E6 RID: 4326
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	// Token: 0x040010E7 RID: 4327
	public float duration = 0.2f;

	// Token: 0x040010E8 RID: 4328
	private Vector3 mScale;

	// Token: 0x040010E9 RID: 4329
	private bool mStarted;
}
