using System;
using UnityEngine;

// Token: 0x0200031E RID: 798
[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour
{
	// Token: 0x06001BB2 RID: 7090 RVA: 0x00071F0C File Offset: 0x0007010C
	private void Start()
	{
		this.mStarted = true;
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x00071F18 File Offset: 0x00070118
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x00071F38 File Offset: 0x00070138
	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x00071F70 File Offset: 0x00070170
	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x00071FA8 File Offset: 0x000701A8
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00071FD0 File Offset: 0x000701D0
	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x00071FF0 File Offset: 0x000701F0
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00072010 File Offset: 0x00070210
	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Transform transform = componentsInChildren[i];
				transform.gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
				i++;
			}
		}
		else
		{
			this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x040010CB RID: 4299
	public GameObject target;

	// Token: 0x040010CC RID: 4300
	public string functionName;

	// Token: 0x040010CD RID: 4301
	public UIButtonMessage.Trigger trigger;

	// Token: 0x040010CE RID: 4302
	public bool includeChildren;

	// Token: 0x040010CF RID: 4303
	private bool mStarted;

	// Token: 0x0200031F RID: 799
	public enum Trigger
	{
		// Token: 0x040010D1 RID: 4305
		OnClick,
		// Token: 0x040010D2 RID: 4306
		OnMouseOver,
		// Token: 0x040010D3 RID: 4307
		OnMouseOut,
		// Token: 0x040010D4 RID: 4308
		OnPress,
		// Token: 0x040010D5 RID: 4309
		OnRelease,
		// Token: 0x040010D6 RID: 4310
		OnDoubleClick
	}
}
