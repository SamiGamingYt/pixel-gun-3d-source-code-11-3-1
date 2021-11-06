using System;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class RotatorKillCam : MonoBehaviour
{
	// Token: 0x06002C3F RID: 11327 RVA: 0x000EAF94 File Offset: 0x000E9194
	private void Start()
	{
		RotatorKillCam.isDraggin = false;
		this.ReturnCameraToDefaultOrientation();
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x000EAFA4 File Offset: 0x000E91A4
	private void OnEnable()
	{
		RotatorKillCam.isDraggin = false;
		this.ReturnCameraToDefaultOrientation();
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x000EAFB4 File Offset: 0x000E91B4
	private void OnPress(bool isDown)
	{
		RotatorKillCam.isDraggin = isDown;
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x000EAFBC File Offset: 0x000E91BC
	private void OnDrag(Vector2 delta)
	{
		if (RPG_Camera.instance == null)
		{
			return;
		}
		RPG_Camera.instance.deltaMouseX += delta.x;
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x000EAFE8 File Offset: 0x000E91E8
	private void ReturnCameraToDefaultOrientation()
	{
		if (RPG_Camera.instance == null)
		{
			return;
		}
		RPG_Camera.instance.deltaMouseX = 0f;
		RPG_Camera.instance.mouseY = 15f;
	}

	// Token: 0x0400214C RID: 8524
	public static bool isDraggin;
}
