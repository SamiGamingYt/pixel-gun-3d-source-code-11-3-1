using System;
using UnityEngine;

// Token: 0x02000320 RID: 800
[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	// Token: 0x06001BBB RID: 7099 RVA: 0x000720E8 File Offset: 0x000702E8
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mPos = this.tweenTarget.localPosition;
		}
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x00072138 File Offset: 0x00070338
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x00072158 File Offset: 0x00070358
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x000721AC File Offset: 0x000703AC
	private void OnPress(bool isPressed)
	{
		this.mPressed = isPressed;
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mPos : (this.mPos + this.hover)) : (this.mPos + this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x00072240 File Offset: 0x00070440
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mPos : (this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x000722A8 File Offset: 0x000704A8
	private void OnDragOver()
	{
		if (this.mPressed)
		{
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos + this.hover).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x000722F0 File Offset: 0x000704F0
	private void OnDragOut()
	{
		if (this.mPressed)
		{
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, this.mPos).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00072320 File Offset: 0x00070520
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040010D7 RID: 4311
	public Transform tweenTarget;

	// Token: 0x040010D8 RID: 4312
	public Vector3 hover = Vector3.zero;

	// Token: 0x040010D9 RID: 4313
	public Vector3 pressed = new Vector3(2f, -2f);

	// Token: 0x040010DA RID: 4314
	public float duration = 0.2f;

	// Token: 0x040010DB RID: 4315
	[NonSerialized]
	private Vector3 mPos;

	// Token: 0x040010DC RID: 4316
	[NonSerialized]
	private bool mStarted;

	// Token: 0x040010DD RID: 4317
	[NonSerialized]
	private bool mPressed;
}
