using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

// Token: 0x020007E0 RID: 2016
public sealed class CameraTouchControlScheme_LowPassFilter : CameraTouchControlScheme
{
	// Token: 0x060048F7 RID: 18679 RVA: 0x001951B4 File Offset: 0x001933B4
	public override void OnPress(bool isDown)
	{
		if (isDown)
		{
			this._accumulatedDrag = new Vector2?(Vector2.zero);
			this._unfilteredAccumulatedDrag = new Vector2?(Vector2.zero);
		}
		else
		{
			this._accumulatedDrag = null;
			this._unfilteredAccumulatedDrag = null;
		}
		this.firstDrag = isDown;
		this.limitDrag = isDown;
		if (isDown)
		{
			if (JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.StartCoroutine(this.CancelLimitDrag());
			}
		}
		else if (JoystickController.rightJoystick)
		{
			JoystickController.rightJoystick.StopCoroutine(this.CancelLimitDrag());
		}
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x00195264 File Offset: 0x00193464
	[Obfuscation(Exclude = true)]
	private IEnumerator CancelLimitDrag()
	{
		yield return new WaitForSeconds(this.dragClampInterval);
		this.limitDrag = false;
		yield break;
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x00195280 File Offset: 0x00193480
	public override void OnDrag(Vector2 delta)
	{
		if (!this.firstDrag)
		{
			this._deltaPosition = delta;
		}
		this.firstDrag = false;
		if (this.limitDrag)
		{
			this.limitDrag = false;
			if (JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.StopCoroutine(this.CancelLimitDrag());
			}
			this._deltaPosition = Vector2.ClampMagnitude(delta, this.dragClamp);
		}
		if (this._accumulatedDrag != null && this._unfilteredAccumulatedDrag != null)
		{
			Vector2 deltaPosition = this._deltaPosition;
			Vector2 vector = this._unfilteredAccumulatedDrag.Value + deltaPosition;
			Vector2 value = this._accumulatedDrag.Value;
			Vector2 value2 = Vector2.Lerp(value, vector, this.lerpCoeff);
			this._accumulatedDrag = new Vector2?(value2);
			this._unfilteredAccumulatedDrag = new Vector2?(vector);
		}
		WeaponManager.sharedManager.myPlayerMoveC.mySkinName.MoveCamera(this._deltaPosition);
		this.Reset();
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x00195378 File Offset: 0x00193578
	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._accumulatedDrag = null;
		this._unfilteredAccumulatedDrag = null;
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x001953B0 File Offset: 0x001935B0
	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (this._accumulatedDrag != null)
		{
			if (this._azimuthTilt != null)
			{
				Vector2 value = this._accumulatedDrag.Value;
				float num = sensitivity / 30f;
				yawTransform.rotation = Quaternion.Euler(0f, this._azimuthTilt.Value.x + value.x * num, 0f);
				float num2 = this._azimuthTilt.Value.y;
				if (num2 > 180f)
				{
					num2 -= 360f;
				}
				float num3 = num2 + value.y * ((!invert) ? -1f : 1f) * num;
				if (num3 > 80f)
				{
					num3 = 80f;
				}
				if (num3 < -65f)
				{
					num3 = -65f;
				}
				pitchTransform.localRotation = Quaternion.Euler(num3, 0f, 0f);
			}
			else
			{
				this._azimuthTilt = new Vector2?(new Vector2(yawTransform.rotation.eulerAngles.y, pitchTransform.localEulerAngles.x));
			}
		}
		else
		{
			this._azimuthTilt = null;
		}
	}

	// Token: 0x04003621 RID: 13857
	public float dragClampInterval = 1.5f;

	// Token: 0x04003622 RID: 13858
	public float dragClamp = 1f;

	// Token: 0x04003623 RID: 13859
	public float lerpCoeff = 0.25f;

	// Token: 0x04003624 RID: 13860
	private Vector2? _accumulatedDrag;

	// Token: 0x04003625 RID: 13861
	private Vector2? _unfilteredAccumulatedDrag;

	// Token: 0x04003626 RID: 13862
	private bool limitDrag;

	// Token: 0x04003627 RID: 13863
	private bool firstDrag;

	// Token: 0x04003628 RID: 13864
	private Vector2? _azimuthTilt;
}
