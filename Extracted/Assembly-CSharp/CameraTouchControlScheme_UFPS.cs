using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020007E2 RID: 2018
public sealed class CameraTouchControlScheme_UFPS : CameraTouchControlScheme
{
	// Token: 0x06004902 RID: 18690 RVA: 0x00195AA8 File Offset: 0x00193CA8
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

	// Token: 0x06004903 RID: 18691 RVA: 0x00195B2C File Offset: 0x00193D2C
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

	// Token: 0x06004904 RID: 18692 RVA: 0x00195C48 File Offset: 0x00193E48
	public override void Reset()
	{
		this._deltaPosition = Vector2.zero;
		this._grabTouches = false;
		this._touchId = -100;
		this._isTouchInputValid = false;
		this._isTouchMoving = false;
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x00195C80 File Offset: 0x00193E80
	public override void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		deltaPosition *= sensitivity * 0.01f;
		deltaPosition = new Vector2(deltaPosition.x, deltaPosition.y);
		Vector2 mouseLook = this.GetMouseLook(deltaPosition);
		if (this._isTouchInputValid)
		{
			if (this._pitchYaw == null)
			{
				this._originalRotationPitch = pitchTransform.localRotation;
				this._originalRotationYaw = yawTransform.rotation;
				this._pitchYaw = new Vector2?(Vector2.zero);
			}
			Vector2 value = this._pitchYaw.Value;
			value.x += mouseLook.y;
			value.y += mouseLook.x;
			if (value.x > 180f)
			{
				value.x -= 360f;
			}
			if (value.y > 180f)
			{
				value.y -= 360f;
			}
			if (value.x < -180f)
			{
				value.x += 360f;
			}
			if (value.y < -180f)
			{
				value.y += 360f;
			}
			value.x = Mathf.Clamp(value.x, -89.5f, 89.5f);
			value.y = Mathf.Clamp(value.y, -360f, 360f);
			this._pitchYaw = new Vector2?(value);
			yawTransform.rotation = this._originalRotationYaw;
			pitchTransform.localRotation = this._originalRotationPitch;
			yawTransform.rotation = this._originalRotationYaw * Quaternion.Euler(0f, this._pitchYaw.Value.y, 0f);
			pitchTransform.localRotation = this._originalRotationPitch * Quaternion.Euler(this._pitchYaw.Value.x * ((!invert) ? -1f : 1f), 0f, 0f);
		}
		else
		{
			this._pitchYaw = null;
		}
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x00195EB4 File Offset: 0x001940B4
	public Vector2 GetMouseLook(Vector2 touchDeltaPosition)
	{
		if (this.m_LastMouseLookFrame == Time.frameCount)
		{
			return this.m_CurrentMouseLook;
		}
		this.m_LastMouseLookFrame = Time.frameCount;
		this.m_MouseLookSmoothMove.x = touchDeltaPosition.x * Time.timeScale;
		this.m_MouseLookSmoothMove.y = touchDeltaPosition.y * Time.timeScale;
		this.mouseLookSmoothSteps = Mathf.Clamp(this.mouseLookSmoothSteps, 1, 20);
		this.mouseLookSmoothWeight = Mathf.Clamp01(this.mouseLookSmoothWeight);
		while (this.m_MouseLookSmoothBuffer.Count > this.mouseLookSmoothSteps)
		{
			this.m_MouseLookSmoothBuffer.RemoveAt(0);
		}
		this.m_MouseLookSmoothBuffer.Add(this.m_MouseLookSmoothMove);
		float num = 1f;
		Vector2 a = Vector2.zero;
		float num2 = 0f;
		for (int i = this.m_MouseLookSmoothBuffer.Count - 1; i > 0; i--)
		{
			a += this.m_MouseLookSmoothBuffer[i] * num;
			num2 += 1f * num;
			num *= this.mouseLookSmoothWeight / this.Delta;
		}
		num2 = Mathf.Max(1f, num2);
		this.m_CurrentMouseLook = CameraTouchControlScheme_UFPS.NaNSafeVector2(a / num2, default(Vector2));
		float num3 = 0f;
		float num4 = Mathf.Abs(this.m_CurrentMouseLook.x);
		float num5 = Mathf.Abs(this.m_CurrentMouseLook.y);
		if (this.mouseLookAcceleration)
		{
			num3 = Mathf.Sqrt(num4 * num4 + num5 * num5) / this.Delta;
			num3 = ((num3 > this.mouseLookAccelerationThreshold) ? num3 : 0f);
		}
		this.m_CurrentMouseLook.x = this.m_CurrentMouseLook.x * (this.mouseLookSensitivity.x + num3);
		this.m_CurrentMouseLook.y = this.m_CurrentMouseLook.y * (this.mouseLookSensitivity.y + num3);
		return this.m_CurrentMouseLook;
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x001960AC File Offset: 0x001942AC
	private static Vector2 NaNSafeVector2(Vector2 vector, [Optional] Vector2 prevVector)
	{
		vector.x = ((!double.IsNaN((double)vector.x)) ? vector.x : prevVector.x);
		vector.y = ((!double.IsNaN((double)vector.y)) ? vector.y : prevVector.y);
		return vector;
	}

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x06004908 RID: 18696 RVA: 0x00196114 File Offset: 0x00194314
	private float Delta
	{
		get
		{
			return Time.deltaTime * 30f;
		}
	}

	// Token: 0x04003639 RID: 13881
	public float startMovingThresholdSq = 4f;

	// Token: 0x0400363A RID: 13882
	public Vector2 mouseLookSensitivity = new Vector2(2.8f, 1.876f);

	// Token: 0x0400363B RID: 13883
	public int mouseLookSmoothSteps = 10;

	// Token: 0x0400363C RID: 13884
	public float mouseLookSmoothWeight = 0.15f;

	// Token: 0x0400363D RID: 13885
	public bool mouseLookAcceleration;

	// Token: 0x0400363E RID: 13886
	public float mouseLookAccelerationThreshold = 0.4f;

	// Token: 0x0400363F RID: 13887
	private bool _grabTouches;

	// Token: 0x04003640 RID: 13888
	private int _touchId;

	// Token: 0x04003641 RID: 13889
	private Vector2 _firstTouchPosition;

	// Token: 0x04003642 RID: 13890
	private Vector2 _previousTouchPosition;

	// Token: 0x04003643 RID: 13891
	private Vector2 _currentTouchPosition;

	// Token: 0x04003644 RID: 13892
	private bool _isTouchInputValid;

	// Token: 0x04003645 RID: 13893
	private bool _isTouchMoving;

	// Token: 0x04003646 RID: 13894
	private Quaternion _originalRotationPitch;

	// Token: 0x04003647 RID: 13895
	private Quaternion _originalRotationYaw;

	// Token: 0x04003648 RID: 13896
	private Vector2? _pitchYaw;

	// Token: 0x04003649 RID: 13897
	private Vector2 m_MouseLookSmoothMove = Vector2.zero;

	// Token: 0x0400364A RID: 13898
	private List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	// Token: 0x0400364B RID: 13899
	private int m_LastMouseLookFrame = -1;

	// Token: 0x0400364C RID: 13900
	private Vector2 m_CurrentMouseLook = Vector2.zero;
}
