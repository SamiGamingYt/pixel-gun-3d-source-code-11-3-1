using System;
using UnityEngine;

// Token: 0x020007DD RID: 2013
public abstract class CameraTouchControlScheme
{
	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x060048E7 RID: 18663 RVA: 0x00195044 File Offset: 0x00193244
	public Vector2 DeltaPosition
	{
		get
		{
			return this._deltaPosition;
		}
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x0019504C File Offset: 0x0019324C
	public virtual void OnPress(bool isDown)
	{
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x00195050 File Offset: 0x00193250
	public virtual void OnDrag(Vector2 delta)
	{
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x00195054 File Offset: 0x00193254
	public virtual void OnUpdate()
	{
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x00195058 File Offset: 0x00193258
	public void ResetDelta()
	{
		this._deltaPosition = Vector2.zero;
	}

	// Token: 0x060048EC RID: 18668
	public abstract void Reset();

	// Token: 0x060048ED RID: 18669
	public abstract void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert);

	// Token: 0x04003606 RID: 13830
	protected Vector2 _deltaPosition;
}
