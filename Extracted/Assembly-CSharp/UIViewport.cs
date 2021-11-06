using System;
using UnityEngine;

// Token: 0x020003BD RID: 957
[AddComponentMenu("NGUI/UI/Viewport Camera")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class UIViewport : MonoBehaviour
{
	// Token: 0x06002259 RID: 8793 RVA: 0x000A5B18 File Offset: 0x000A3D18
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000A5B50 File Offset: 0x000A3D50
	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			if (this.topLeft.gameObject.activeInHierarchy)
			{
				Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
				Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
				Rect rect = new Rect(vector.x / (float)Screen.width, vector2.y / (float)Screen.height, (vector2.x - vector.x) / (float)Screen.width, (vector.y - vector2.y) / (float)Screen.height);
				float num = this.fullSize * rect.height;
				if (rect != this.mCam.rect)
				{
					this.mCam.rect = rect;
				}
				if (this.mCam.orthographicSize != num)
				{
					this.mCam.orthographicSize = num;
				}
				this.mCam.enabled = true;
			}
			else
			{
				this.mCam.enabled = false;
			}
		}
	}

	// Token: 0x0400165C RID: 5724
	public Camera sourceCamera;

	// Token: 0x0400165D RID: 5725
	public Transform topLeft;

	// Token: 0x0400165E RID: 5726
	public Transform bottomRight;

	// Token: 0x0400165F RID: 5727
	public float fullSize = 1f;

	// Token: 0x04001660 RID: 5728
	private Camera mCam;
}
