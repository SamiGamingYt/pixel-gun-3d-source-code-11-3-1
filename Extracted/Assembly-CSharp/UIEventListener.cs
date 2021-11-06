using System;
using UnityEngine;

// Token: 0x02000377 RID: 887
[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06001F01 RID: 7937 RVA: 0x0008E748 File Offset: 0x0008C948
	private bool isColliderEnabled
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

	// Token: 0x06001F02 RID: 7938 RVA: 0x0008E78C File Offset: 0x0008C98C
	private void OnSubmit()
	{
		if (this.isColliderEnabled && this.onSubmit != null)
		{
			this.onSubmit(base.gameObject);
		}
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x0008E7B8 File Offset: 0x0008C9B8
	private void OnClick()
	{
		if (this.isColliderEnabled && this.onClick != null)
		{
			this.onClick(base.gameObject);
		}
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x0008E7E4 File Offset: 0x0008C9E4
	private void OnDoubleClick()
	{
		if (this.isColliderEnabled && this.onDoubleClick != null)
		{
			this.onDoubleClick(base.gameObject);
		}
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x0008E810 File Offset: 0x0008CA10
	private void OnHover(bool isOver)
	{
		if (this.isColliderEnabled && this.onHover != null)
		{
			this.onHover(base.gameObject, isOver);
		}
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x0008E848 File Offset: 0x0008CA48
	private void OnPress(bool isPressed)
	{
		if (this.isColliderEnabled && this.onPress != null)
		{
			this.onPress(base.gameObject, isPressed);
		}
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x0008E880 File Offset: 0x0008CA80
	private void OnSelect(bool selected)
	{
		if (this.isColliderEnabled && this.onSelect != null)
		{
			this.onSelect(base.gameObject, selected);
		}
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x0008E8B8 File Offset: 0x0008CAB8
	private void OnScroll(float delta)
	{
		if (this.isColliderEnabled && this.onScroll != null)
		{
			this.onScroll(base.gameObject, delta);
		}
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x0008E8F0 File Offset: 0x0008CAF0
	private void OnDragStart()
	{
		if (this.onDragStart != null)
		{
			this.onDragStart(base.gameObject);
		}
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x0008E910 File Offset: 0x0008CB10
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.gameObject, delta);
		}
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x0008E930 File Offset: 0x0008CB30
	private void OnDragOver()
	{
		if (this.isColliderEnabled && this.onDragOver != null)
		{
			this.onDragOver(base.gameObject);
		}
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x0008E95C File Offset: 0x0008CB5C
	private void OnDragOut()
	{
		if (this.isColliderEnabled && this.onDragOut != null)
		{
			this.onDragOut(base.gameObject);
		}
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x0008E988 File Offset: 0x0008CB88
	private void OnDragEnd()
	{
		if (this.onDragEnd != null)
		{
			this.onDragEnd(base.gameObject);
		}
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x0008E9A8 File Offset: 0x0008CBA8
	private void OnDrop(GameObject go)
	{
		if (this.isColliderEnabled && this.onDrop != null)
		{
			this.onDrop(base.gameObject, go);
		}
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x0008E9E0 File Offset: 0x0008CBE0
	private void OnKey(KeyCode key)
	{
		if (this.isColliderEnabled && this.onKey != null)
		{
			this.onKey(base.gameObject, key);
		}
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x0008EA18 File Offset: 0x0008CC18
	private void OnTooltip(bool show)
	{
		if (this.isColliderEnabled && this.onTooltip != null)
		{
			this.onTooltip(base.gameObject, show);
		}
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x0008EA50 File Offset: 0x0008CC50
	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uieventListener = go.GetComponent<UIEventListener>();
		if (uieventListener == null)
		{
			uieventListener = go.AddComponent<UIEventListener>();
		}
		return uieventListener;
	}

	// Token: 0x0400139E RID: 5022
	public object parameter;

	// Token: 0x0400139F RID: 5023
	public UIEventListener.VoidDelegate onSubmit;

	// Token: 0x040013A0 RID: 5024
	public UIEventListener.VoidDelegate onClick;

	// Token: 0x040013A1 RID: 5025
	public UIEventListener.VoidDelegate onDoubleClick;

	// Token: 0x040013A2 RID: 5026
	public UIEventListener.BoolDelegate onHover;

	// Token: 0x040013A3 RID: 5027
	public UIEventListener.BoolDelegate onPress;

	// Token: 0x040013A4 RID: 5028
	public UIEventListener.BoolDelegate onSelect;

	// Token: 0x040013A5 RID: 5029
	public UIEventListener.FloatDelegate onScroll;

	// Token: 0x040013A6 RID: 5030
	public UIEventListener.VoidDelegate onDragStart;

	// Token: 0x040013A7 RID: 5031
	public UIEventListener.VectorDelegate onDrag;

	// Token: 0x040013A8 RID: 5032
	public UIEventListener.VoidDelegate onDragOver;

	// Token: 0x040013A9 RID: 5033
	public UIEventListener.VoidDelegate onDragOut;

	// Token: 0x040013AA RID: 5034
	public UIEventListener.VoidDelegate onDragEnd;

	// Token: 0x040013AB RID: 5035
	public UIEventListener.ObjectDelegate onDrop;

	// Token: 0x040013AC RID: 5036
	public UIEventListener.KeyCodeDelegate onKey;

	// Token: 0x040013AD RID: 5037
	public UIEventListener.BoolDelegate onTooltip;

	// Token: 0x020008FD RID: 2301
	// (Invoke) Token: 0x06005094 RID: 20628
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x020008FE RID: 2302
	// (Invoke) Token: 0x06005098 RID: 20632
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x020008FF RID: 2303
	// (Invoke) Token: 0x0600509C RID: 20636
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x02000900 RID: 2304
	// (Invoke) Token: 0x060050A0 RID: 20640
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x02000901 RID: 2305
	// (Invoke) Token: 0x060050A4 RID: 20644
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x02000902 RID: 2306
	// (Invoke) Token: 0x060050A8 RID: 20648
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
