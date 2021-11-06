using System;
using UnityEngine;

// Token: 0x0200032C RID: 812
[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	// Token: 0x06001C09 RID: 7177 RVA: 0x00074368 File Offset: 0x00072568
	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				this.mRayPos = currentRay.GetPoint(distance);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x00074428 File Offset: 0x00072628
	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 b = currentRay.GetPoint(distance) - this.mRayPos;
				cachedTransform.position += b;
				Vector3 vector = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
				cachedTransform.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector.x, vector.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
				if (this.updateAnchors)
				{
					this.target.BroadcastMessage("UpdateAnchors");
				}
			}
		}
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x0007454C File Offset: 0x0007274C
	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	// Token: 0x04001120 RID: 4384
	public UIWidget target;

	// Token: 0x04001121 RID: 4385
	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	// Token: 0x04001122 RID: 4386
	public int minWidth = 100;

	// Token: 0x04001123 RID: 4387
	public int minHeight = 100;

	// Token: 0x04001124 RID: 4388
	public int maxWidth = 100000;

	// Token: 0x04001125 RID: 4389
	public int maxHeight = 100000;

	// Token: 0x04001126 RID: 4390
	public bool updateAnchors;

	// Token: 0x04001127 RID: 4391
	private Plane mPlane;

	// Token: 0x04001128 RID: 4392
	private Vector3 mRayPos;

	// Token: 0x04001129 RID: 4393
	private Vector3 mLocalPos;

	// Token: 0x0400112A RID: 4394
	private int mWidth;

	// Token: 0x0400112B RID: 4395
	private int mHeight;

	// Token: 0x0400112C RID: 4396
	private bool mDragging;
}
