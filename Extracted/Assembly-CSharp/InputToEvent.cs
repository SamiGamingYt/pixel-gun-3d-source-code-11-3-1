using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class InputToEvent : MonoBehaviour
{
	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x060026B8 RID: 9912 RVA: 0x000C1FA8 File Offset: 0x000C01A8
	// (set) Token: 0x060026B9 RID: 9913 RVA: 0x000C1FB0 File Offset: 0x000C01B0
	public static GameObject goPointedAt { get; private set; }

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x060026BA RID: 9914 RVA: 0x000C1FB8 File Offset: 0x000C01B8
	public Vector2 DragVector
	{
		get
		{
			return (!this.Dragging) ? Vector2.zero : (this.currentPos - this.pressedPosition);
		}
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x000C1FEC File Offset: 0x000C01EC
	private void Start()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x000C1FFC File Offset: 0x000C01FC
	private void Update()
	{
		if (this.DetectPointedAtGameObject)
		{
			InputToEvent.goPointedAt = this.RaycastObject(Input.mousePosition);
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			this.currentPos = touch.position;
			if (touch.phase == TouchPhase.Began)
			{
				this.Press(touch.position);
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				this.Release(touch.position);
			}
			return;
		}
		this.currentPos = Input.mousePosition;
		if (Input.GetMouseButtonDown(0))
		{
			this.Press(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.Release(Input.mousePosition);
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.pressedPosition = Input.mousePosition;
			this.lastGo = this.RaycastObject(this.pressedPosition);
			if (this.lastGo != null)
			{
				this.lastGo.SendMessage("OnPressRight", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x000C2118 File Offset: 0x000C0318
	private void Press(Vector2 screenPos)
	{
		this.pressedPosition = screenPos;
		this.Dragging = true;
		this.lastGo = this.RaycastObject(screenPos);
		if (this.lastGo != null)
		{
			this.lastGo.SendMessage("OnPress", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x000C2158 File Offset: 0x000C0358
	private void Release(Vector2 screenPos)
	{
		if (this.lastGo != null)
		{
			GameObject x = this.RaycastObject(screenPos);
			if (x == this.lastGo)
			{
				this.lastGo.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
			this.lastGo.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
			this.lastGo = null;
		}
		this.pressedPosition = Vector2.zero;
		this.Dragging = false;
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x000C21CC File Offset: 0x000C03CC
	private GameObject RaycastObject(Vector2 screenPos)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.m_Camera.ScreenPointToRay(screenPos), out raycastHit, 200f))
		{
			InputToEvent.inputHitPos = raycastHit.point;
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	// Token: 0x04001B34 RID: 6964
	private GameObject lastGo;

	// Token: 0x04001B35 RID: 6965
	public static Vector3 inputHitPos;

	// Token: 0x04001B36 RID: 6966
	public bool DetectPointedAtGameObject;

	// Token: 0x04001B37 RID: 6967
	private Vector2 pressedPosition = Vector2.zero;

	// Token: 0x04001B38 RID: 6968
	private Vector2 currentPos = Vector2.zero;

	// Token: 0x04001B39 RID: 6969
	public bool Dragging;

	// Token: 0x04001B3A RID: 6970
	private Camera m_Camera;
}
