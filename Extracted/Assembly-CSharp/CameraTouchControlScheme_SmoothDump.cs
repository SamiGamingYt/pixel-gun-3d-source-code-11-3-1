using System;
using UnityEngine;

// Token: 0x020007E1 RID: 2017
public sealed class CameraTouchControlScheme_SmoothDump : CameraTouchControlScheme
{
	// Token: 0x060048FD RID: 18685 RVA: 0x00195548 File Offset: 0x00193748
	public override void OnPress(bool isDown)
	{
		if ((isDown && this._touchId == -100) || (!isDown && this._touchId != -100))
		{
			this._grabTouches = isDown;
			this._touchId = ((!isDown) ? -100 : UICamera.currentTouchID);
			this._firstTouchPosition = UICamera.currentTouch.pos;
			this._previousTouchPosition = this._firstTouchPosition;
			this._currentTouchPosition = this._firstTouchPosition;
			this._isTouchMoving = false;
		}
	}

	// Token: 0x060048FE RID: 18686 RVA: 0x001955CC File Offset: 0x001937CC
	public override void OnUpdate()
	{
		this._isTouchInputValid = false;
		Touch? touch = null;
		if (this._grabTouches)
		{
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch2 = Input.GetTouch(i);
				if (touch2.fingerId == this._touchId && (touch2.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Stationary))
				{
					this._isTouchInputValid = true;
					this._previousTouchPosition = this._currentTouchPosition;
					this._currentTouchPosition = touch2.position;
					Touch? touch3 = new Touch?(touch2);
					break;
				}
			}
		}
		this._deltaPosition = Vector2.zero;
		if (this._isTouchInputValid)
		{
			if (this._isTouchMoving || (this._currentTouchPosition - this._firstTouchPosition).sqrMagnitude >= this.startMovingThresholdSq)
			{
				if (!this._isTouchMoving)
				{
					this._isTouchMoving = true;
				}
				else
				{
					this._deltaPosition = this._currentTouchPosition - this._previousTouchPosition;
				}
			}
		}
	}

	// Token: 0x060048FF RID: 18687 RVA: 0x001956E8 File Offset: 0x001938E8
	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._grabTouches = false;
		this._touchId = -100;
		this._isTouchInputValid = false;
		this._isTouchMoving = false;
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x00195720 File Offset: 0x00193920
	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (this._isTouchInputValid)
		{
			if (this._followPitchYaw == null)
			{
				this._originalRotationPitch = pitchTransform.localRotation;
				this._originalRotationYaw = yawTransform.rotation;
				this._followPitchYaw = new Vector2?(Vector2.zero);
				this._targetPitchYaw = new Vector2?(Vector2.zero);
			}
			Vector2 value = this._followPitchYaw.Value;
			Vector2 value2 = this._targetPitchYaw.Value;
			if (value2.x > 180f)
			{
				value2.x -= 360f;
				value.x -= 360f;
			}
			if (value2.y > 180f)
			{
				value2.y -= 360f;
				value.y -= 360f;
			}
			if (value2.x < -180f)
			{
				value2.x += 360f;
				value.x += 360f;
			}
			if (value2.y < -180f)
			{
				value2.y += 360f;
				value.y += 360f;
			}
			value2.x += deltaPosition.y * sensitivity * this.senseModifier * this.senseModifierByAxis.y;
			value2.y += deltaPosition.x * sensitivity * this.senseModifier * this.senseModifierByAxis.x;
			value2.x = Mathf.Clamp(value2.x, -65f, 80f);
			value.x = Mathf.SmoothDamp(value.x, value2.x, ref this._followPitchYawVelocity.x, this.dampingTime);
			value.y = Mathf.SmoothDamp(value.y, value2.y, ref this._followPitchYawVelocity.y, this.dampingTime);
			this._followPitchYaw = new Vector2?(value);
			this._targetPitchYaw = new Vector2?(value2);
		}
		else
		{
			this._followPitchYaw = this._targetPitchYaw;
			this._followPitchYawVelocity = Vector2.zero;
			this._targetPitchYaw = null;
		}
		if (this._followPitchYaw != null)
		{
			yawTransform.rotation = this._originalRotationYaw * Quaternion.Euler(0f, this._followPitchYaw.Value.y, 0f);
			pitchTransform.localRotation = this._originalRotationPitch * Quaternion.Euler(this._followPitchYaw.Value.x * ((!invert) ? -1f : 1f), 0f, 0f);
		}
		if (!this._isTouchInputValid)
		{
			this._followPitchYaw = null;
		}
	}

	// Token: 0x04003629 RID: 13865
	public float startMovingThresholdSq = 4f;

	// Token: 0x0400362A RID: 13866
	public float senseModifier = 0.03f;

	// Token: 0x0400362B RID: 13867
	public Vector2 senseModifierByAxis = new Vector2(1f, 0.8f);

	// Token: 0x0400362C RID: 13868
	public float dampingTime = 0.05f;

	// Token: 0x0400362D RID: 13869
	private bool _grabTouches;

	// Token: 0x0400362E RID: 13870
	private int _touchId;

	// Token: 0x0400362F RID: 13871
	private Vector2 _firstTouchPosition;

	// Token: 0x04003630 RID: 13872
	private Vector2 _previousTouchPosition;

	// Token: 0x04003631 RID: 13873
	private Vector2 _currentTouchPosition;

	// Token: 0x04003632 RID: 13874
	private bool _isTouchInputValid;

	// Token: 0x04003633 RID: 13875
	private bool _isTouchMoving;

	// Token: 0x04003634 RID: 13876
	private Quaternion _originalRotationPitch;

	// Token: 0x04003635 RID: 13877
	private Quaternion _originalRotationYaw;

	// Token: 0x04003636 RID: 13878
	private Vector2? _followPitchYaw;

	// Token: 0x04003637 RID: 13879
	private Vector2 _followPitchYawVelocity;

	// Token: 0x04003638 RID: 13880
	private Vector2? _targetPitchYaw;
}
