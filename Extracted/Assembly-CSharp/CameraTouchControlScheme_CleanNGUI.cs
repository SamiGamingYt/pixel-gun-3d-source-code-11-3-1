using System;
using UnityEngine;

// Token: 0x020007DF RID: 2015
public sealed class CameraTouchControlScheme_CleanNGUI : CameraTouchControlScheme
{
	// Token: 0x060048F2 RID: 18674 RVA: 0x00195094 File Offset: 0x00193294
	public override void OnPress(bool isDown)
	{
		this._limitDragDelta = isDown;
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x001950A0 File Offset: 0x001932A0
	public override void OnDrag(Vector2 delta)
	{
		if (this._limitDragDelta)
		{
			this._limitDragDelta = false;
			this._deltaPosition = Vector2.ClampMagnitude(delta, this.firstDragClampedMax);
		}
		else
		{
			this._deltaPosition = delta;
		}
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.MoveCamera(this._deltaPosition);
		this.Reset();
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x00195100 File Offset: 0x00193300
	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._limitDragDelta = false;
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x00195114 File Offset: 0x00193314
	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		Vector2 vector = deltaPosition * sensitivity * 30f / (float)Screen.width;
		yawTransform.Rotate(0f, vector.x, 0f, Space.World);
		pitchTransform.Rotate(((!invert) ? -1f : 1f) * vector.y, 0f, 0f);
	}

	// Token: 0x0400361F RID: 13855
	public float firstDragClampedMax = 5f;

	// Token: 0x04003620 RID: 13856
	private bool _limitDragDelta;
}
