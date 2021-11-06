using System;
using UnityEngine;

// Token: 0x02000330 RID: 816
[AddComponentMenu("NGUI/Interaction/Forward Events (Legacy)")]
public class UIForwardEvents : MonoBehaviour
{
	// Token: 0x06001C2B RID: 7211 RVA: 0x000750C8 File Offset: 0x000732C8
	private void OnHover(bool isOver)
	{
		if (this.onHover && this.target != null)
		{
			this.target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x00075100 File Offset: 0x00073300
	private void OnPress(bool pressed)
	{
		if (this.onPress && this.target != null)
		{
			this.target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x00075138 File Offset: 0x00073338
	private void OnClick()
	{
		if (this.onClick && this.target != null)
		{
			this.target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x00075168 File Offset: 0x00073368
	private void OnDoubleClick()
	{
		if (this.onDoubleClick && this.target != null)
		{
			this.target.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x00075198 File Offset: 0x00073398
	private void OnSelect(bool selected)
	{
		if (this.onSelect && this.target != null)
		{
			this.target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000751D0 File Offset: 0x000733D0
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag && this.target != null)
		{
			this.target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x00075208 File Offset: 0x00073408
	private void OnDrop(GameObject go)
	{
		if (this.onDrop && this.target != null)
		{
			this.target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x00075244 File Offset: 0x00073444
	private void OnSubmit()
	{
		if (this.onSubmit && this.target != null)
		{
			this.target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x00075274 File Offset: 0x00073474
	private void OnScroll(float delta)
	{
		if (this.onScroll && this.target != null)
		{
			this.target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0400114F RID: 4431
	public GameObject target;

	// Token: 0x04001150 RID: 4432
	public bool onHover;

	// Token: 0x04001151 RID: 4433
	public bool onPress;

	// Token: 0x04001152 RID: 4434
	public bool onClick;

	// Token: 0x04001153 RID: 4435
	public bool onDoubleClick;

	// Token: 0x04001154 RID: 4436
	public bool onSelect;

	// Token: 0x04001155 RID: 4437
	public bool onDrag;

	// Token: 0x04001156 RID: 4438
	public bool onDrop;

	// Token: 0x04001157 RID: 4439
	public bool onSubmit;

	// Token: 0x04001158 RID: 4440
	public bool onScroll;
}
