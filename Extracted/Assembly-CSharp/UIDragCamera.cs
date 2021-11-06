using System;
using UnityEngine;

// Token: 0x02000325 RID: 805
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Camera")]
public class UIDragCamera : MonoBehaviour
{
	// Token: 0x06001BDE RID: 7134 RVA: 0x00073028 File Offset: 0x00071228
	private void Awake()
	{
		if (this.draggableCamera == null)
		{
			this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(base.gameObject);
		}
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00073058 File Offset: 0x00071258
	private void OnPress(bool isPressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Press(isPressed);
		}
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000730A0 File Offset: 0x000712A0
	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Drag(delta);
		}
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x000730E8 File Offset: 0x000712E8
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Scroll(delta);
		}
	}

	// Token: 0x040010F0 RID: 4336
	public UIDraggableCamera draggableCamera;
}
