using System;
using UnityEngine;

// Token: 0x02000808 RID: 2056
public class UIJoystick : MonoBehaviour
{
	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06004B1E RID: 19230 RVA: 0x001ABCA4 File Offset: 0x001A9EA4
	// (set) Token: 0x06004B1F RID: 19231 RVA: 0x001ABCD8 File Offset: 0x001A9ED8
	public float ActualRadius
	{
		get
		{
			return (this._actualRadius == null) ? this.radius : this._actualRadius.Value;
		}
		set
		{
			this._actualRadius = new float?(value);
		}
	}

	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06004B20 RID: 19232 RVA: 0x001ABCE8 File Offset: 0x001A9EE8
	public float ActualRadiusSq
	{
		get
		{
			float actualRadius = this.ActualRadius;
			return actualRadius * actualRadius;
		}
	}

	// Token: 0x06004B21 RID: 19233 RVA: 0x001ABD00 File Offset: 0x001A9F00
	private void Awake()
	{
		this._joystickWidget = base.GetComponent<UIWidget>();
		this.touchPadInJoystick = base.GetComponent<TouchPadInJoystick>();
		this._isHunger = Defs.isHunger;
	}

	// Token: 0x06004B22 RID: 19234 RVA: 0x001ABD28 File Offset: 0x001A9F28
	private void Start()
	{
		this.ChangeSide();
		PauseNGUIController.PlayerHandUpdated += this.ChangeSide;
		if (this._isHunger)
		{
			this._hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
		}
		this.UpdateVisibility();
		this.Reset();
	}

	// Token: 0x06004B23 RID: 19235 RVA: 0x001ABD78 File Offset: 0x001A9F78
	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= this.ChangeSide;
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x001ABD8C File Offset: 0x001A9F8C
	private void Update()
	{
		this.ProcessInput();
		this.UpdateVisibility();
	}

	// Token: 0x06004B25 RID: 19237 RVA: 0x001ABD9C File Offset: 0x001A9F9C
	private void ProcessInput()
	{
		if (this._grabTouches)
		{
			this.ProcessTouches();
		}
		else if (!Defs.isMouseControl)
		{
			this.Reset();
		}
	}

	// Token: 0x06004B26 RID: 19238 RVA: 0x001ABDD0 File Offset: 0x001A9FD0
	private void ProcessTouches()
	{
		if (this._touchId != -100)
		{
			Touch? touchById = UIJoystick.GetTouchById(this._touchId);
			if (touchById != null)
			{
				this._touchWorldPos = touchById.Value.position;
			}
		}
		if (this._touchId != -100)
		{
			Vector2 delta = this._touchPrevWorldPos - this._touchWorldPos;
			this._touchPrevWorldPos = this._touchWorldPos;
			this.OnDrag2(delta);
			if (this.touchPadInJoystick.isShooting)
			{
				this.value = Vector2.zero;
				this.target.localPosition = Vector3.zero;
			}
		}
		else
		{
			this.Reset();
		}
	}

	// Token: 0x06004B27 RID: 19239 RVA: 0x001ABE80 File Offset: 0x001AA080
	public static Touch? GetTouchById(int touchId)
	{
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.fingerId == touchId && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
			{
				return new Touch?(touch);
			}
		}
		return null;
	}

	// Token: 0x06004B28 RID: 19240 RVA: 0x001ABEF0 File Offset: 0x001AA0F0
	private void OnPress(bool isDown)
	{
		if ((isDown && this._touchId == -100) || (!isDown && this._touchId != -100))
		{
			this._grabTouches = isDown;
			this._touchId = ((!isDown) ? -100 : UICamera.currentTouchID);
			this._touchWorldPos = ((!isDown) ? Vector2.zero : UICamera.currentTouch.pos);
			this._touchPrevWorldPos = this._touchWorldPos;
		}
	}

	// Token: 0x06004B29 RID: 19241 RVA: 0x001ABF70 File Offset: 0x001AA170
	public void SetJoystickActive(bool joyActive)
	{
		base.enabled = joyActive;
		if (!joyActive)
		{
			this.Reset();
		}
	}

	// Token: 0x06004B2A RID: 19242 RVA: 0x001ABF88 File Offset: 0x001AA188
	private void UpdateVisibility()
	{
		bool flag = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None || TrainingController.stepTraining >= TrainingState.TapToMove) && (!this._isHunger || this._hungerGameController.isGo);
		this._joystickWidget.alpha = ((!flag) ? 0f : 1f);
	}

	// Token: 0x06004B2B RID: 19243 RVA: 0x001ABFF8 File Offset: 0x001AA1F8
	private void ChangeSide()
	{
		base.transform.parent.GetComponent<UIAnchor>().side = ((!GlobalGameController.LeftHanded) ? UIAnchor.Side.BottomRight : UIAnchor.Side.BottomLeft);
		this.Reset();
	}

	// Token: 0x06004B2C RID: 19244 RVA: 0x001AC034 File Offset: 0x001AA234
	public void Reset()
	{
		this.value = Vector2.zero;
		this.target.localPosition = Vector3.zero;
		this._grabTouches = false;
		this._touchId = -100;
		this._touchWorldPos = Vector2.zero;
		this._touchPrevWorldPos = this._touchWorldPos;
	}

	// Token: 0x06004B2D RID: 19245 RVA: 0x001AC084 File Offset: 0x001AA284
	private void OnDrag2(Vector2 delta)
	{
		this.target.position = UICamera.currentCamera.ScreenToWorldPoint(this._touchWorldPos);
		this.target.localPosition = new Vector3(this.target.localPosition.x, this.target.localPosition.y, 0f);
		float magnitude = this.target.localPosition.magnitude;
		if (magnitude > this.ActualRadius)
		{
			this.target.localPosition = Vector3.ClampMagnitude(this.target.localPosition, this.ActualRadius);
		}
		this.value = this.target.localPosition;
		this.value = this.value / this.ActualRadius * Mathf.InverseLerp(this.ActualRadius, 2f, 1f);
	}

	// Token: 0x040037B4 RID: 14260
	public const int NO_TOUCH_ID = -100;

	// Token: 0x040037B5 RID: 14261
	public Transform target;

	// Token: 0x040037B6 RID: 14262
	public float radius;

	// Token: 0x040037B7 RID: 14263
	public Vector2 value;

	// Token: 0x040037B8 RID: 14264
	private HungerGameController _hungerGameController;

	// Token: 0x040037B9 RID: 14265
	private bool _isHunger;

	// Token: 0x040037BA RID: 14266
	private float? _actualRadius;

	// Token: 0x040037BB RID: 14267
	private TouchPadInJoystick touchPadInJoystick;

	// Token: 0x040037BC RID: 14268
	private UIWidget _joystickWidget;

	// Token: 0x040037BD RID: 14269
	private bool _grabTouches;

	// Token: 0x040037BE RID: 14270
	private Vector2 _touchWorldPos;

	// Token: 0x040037BF RID: 14271
	private Vector2 _touchPrevWorldPos;

	// Token: 0x040037C0 RID: 14272
	private int _touchId;
}
