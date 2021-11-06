using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200032F RID: 815
[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06001C1F RID: 7199 RVA: 0x00074DEC File Offset: 0x00072FEC
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

	// Token: 0x06001C20 RID: 7200 RVA: 0x00074E30 File Offset: 0x00073030
	private void OnHover(bool isOver)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (isOver)
		{
			EventDelegate.Execute(this.onHoverOver);
		}
		else
		{
			EventDelegate.Execute(this.onHoverOut);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x00074E88 File Offset: 0x00073088
	private void OnPress(bool pressed)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (pressed)
		{
			EventDelegate.Execute(this.onPress);
		}
		else
		{
			EventDelegate.Execute(this.onRelease);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x00074EE0 File Offset: 0x000730E0
	private void OnSelect(bool selected)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (selected)
		{
			EventDelegate.Execute(this.onSelect);
		}
		else
		{
			EventDelegate.Execute(this.onDeselect);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x00074F38 File Offset: 0x00073138
	private void OnClick()
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x00074F70 File Offset: 0x00073170
	private void OnDoubleClick()
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDoubleClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x00074FA8 File Offset: 0x000731A8
	private void OnDragStart()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragStart);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x00074FE0 File Offset: 0x000731E0
	private void OnDragEnd()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragEnd);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x00075018 File Offset: 0x00073218
	private void OnDragOver(GameObject go)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOver);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x00075050 File Offset: 0x00073250
	private void OnDragOut(GameObject go)
	{
		if (UIEventTrigger.current != null || !this.isColliderEnabled)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x00075088 File Offset: 0x00073288
	private void OnDrag(Vector2 delta)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDrag);
		UIEventTrigger.current = null;
	}

	// Token: 0x04001141 RID: 4417
	public static UIEventTrigger current;

	// Token: 0x04001142 RID: 4418
	public List<EventDelegate> onHoverOver = new List<EventDelegate>();

	// Token: 0x04001143 RID: 4419
	public List<EventDelegate> onHoverOut = new List<EventDelegate>();

	// Token: 0x04001144 RID: 4420
	public List<EventDelegate> onPress = new List<EventDelegate>();

	// Token: 0x04001145 RID: 4421
	public List<EventDelegate> onRelease = new List<EventDelegate>();

	// Token: 0x04001146 RID: 4422
	public List<EventDelegate> onSelect = new List<EventDelegate>();

	// Token: 0x04001147 RID: 4423
	public List<EventDelegate> onDeselect = new List<EventDelegate>();

	// Token: 0x04001148 RID: 4424
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x04001149 RID: 4425
	public List<EventDelegate> onDoubleClick = new List<EventDelegate>();

	// Token: 0x0400114A RID: 4426
	public List<EventDelegate> onDragStart = new List<EventDelegate>();

	// Token: 0x0400114B RID: 4427
	public List<EventDelegate> onDragEnd = new List<EventDelegate>();

	// Token: 0x0400114C RID: 4428
	public List<EventDelegate> onDragOver = new List<EventDelegate>();

	// Token: 0x0400114D RID: 4429
	public List<EventDelegate> onDragOut = new List<EventDelegate>();

	// Token: 0x0400114E RID: 4430
	public List<EventDelegate> onDrag = new List<EventDelegate>();
}
