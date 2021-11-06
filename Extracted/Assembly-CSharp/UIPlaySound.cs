using System;
using UnityEngine;

// Token: 0x0200033B RID: 827
[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06001C82 RID: 7298 RVA: 0x00076D84 File Offset: 0x00074F84
	protected bool canPlay
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			UIButton component = base.GetComponent<UIButton>();
			return component == null || component.isEnabled;
		}
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00076DBC File Offset: 0x00074FBC
	private void OnEnable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnEnable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00076DF0 File Offset: 0x00074FF0
	private void OnDisable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnDisable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00076E24 File Offset: 0x00075024
	private void OnHover(bool isOver)
	{
		if (this.trigger == UIPlaySound.Trigger.OnMouseOver)
		{
			if (this.mIsOver == isOver)
			{
				return;
			}
			this.mIsOver = isOver;
		}
		if (this.canPlay && ((isOver && this.trigger == UIPlaySound.Trigger.OnMouseOver) || (!isOver && this.trigger == UIPlaySound.Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00076E98 File Offset: 0x00075098
	private void OnPress(bool isPressed)
	{
		if (this.trigger == UIPlaySound.Trigger.OnPress)
		{
			if (this.mIsOver == isPressed)
			{
				return;
			}
			this.mIsOver = isPressed;
		}
		if (this.canPlay && ((isPressed && this.trigger == UIPlaySound.Trigger.OnPress) || (!isPressed && this.trigger == UIPlaySound.Trigger.OnRelease)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x00076F0C File Offset: 0x0007510C
	private void OnClick()
	{
		if (this.canPlay && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00076F48 File Offset: 0x00075148
	private void OnSelect(bool isSelected)
	{
		if (this.canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x00076F70 File Offset: 0x00075170
	public void Play()
	{
		NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
	}

	// Token: 0x040011A9 RID: 4521
	public AudioClip audioClip;

	// Token: 0x040011AA RID: 4522
	public UIPlaySound.Trigger trigger;

	// Token: 0x040011AB RID: 4523
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x040011AC RID: 4524
	[Range(0f, 2f)]
	public float pitch = 1f;

	// Token: 0x040011AD RID: 4525
	private bool mIsOver;

	// Token: 0x0200033C RID: 828
	public enum Trigger
	{
		// Token: 0x040011AF RID: 4527
		OnClick,
		// Token: 0x040011B0 RID: 4528
		OnMouseOver,
		// Token: 0x040011B1 RID: 4529
		OnMouseOut,
		// Token: 0x040011B2 RID: 4530
		OnPress,
		// Token: 0x040011B3 RID: 4531
		OnRelease,
		// Token: 0x040011B4 RID: 4532
		Custom,
		// Token: 0x040011B5 RID: 4533
		OnEnable,
		// Token: 0x040011B6 RID: 4534
		OnDisable
	}
}
