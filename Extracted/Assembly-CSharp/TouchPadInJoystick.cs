using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000807 RID: 2055
public class TouchPadInJoystick : MonoBehaviour
{
	// Token: 0x06004B10 RID: 19216 RVA: 0x001AB7BC File Offset: 0x001A99BC
	private IEnumerator ReCalcRects()
	{
		yield return null;
		yield return null;
		this.CalcRects();
		yield break;
	}

	// Token: 0x06004B11 RID: 19217 RVA: 0x001AB7D8 File Offset: 0x001A99D8
	public void SetJoystickActive(bool active)
	{
		this._joyActive = active;
		if (!active)
		{
			this.isShooting = false;
			this.isJumpPressed = false;
		}
	}

	// Token: 0x06004B12 RID: 19218 RVA: 0x001AB7F8 File Offset: 0x001A99F8
	private void OnEnable()
	{
		this.isShooting = false;
		if (this._shouldRecalcRects)
		{
			base.StartCoroutine(this.ReCalcRects());
		}
		this._shouldRecalcRects = false;
		base.StartCoroutine(this._SetIsFirstFrame());
	}

	// Token: 0x06004B13 RID: 19219 RVA: 0x001AB838 File Offset: 0x001A9A38
	private IEnumerator _SetIsFirstFrame()
	{
		float tm = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - tm < 0.1f);
		this._isFirstFrame = false;
		yield break;
	}

	// Token: 0x06004B14 RID: 19220 RVA: 0x001AB854 File Offset: 0x001A9A54
	private IEnumerator Start()
	{
		if (Defs.isHunger)
		{
			this._hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
		}
		PauseNGUIController.PlayerHandUpdated += this.SetSideAndCalcRects;
		ControlsSettingsBase.ControlsChanged += this.SetShouldRecalcRects;
		yield return null;
		yield return null;
		this.CalcRects();
		yield break;
	}

	// Token: 0x06004B15 RID: 19221 RVA: 0x001AB870 File Offset: 0x001A9A70
	private void SetSideAndCalcRects()
	{
		this.SetShouldRecalcRects();
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x001AB878 File Offset: 0x001A9A78
	private void SetShouldRecalcRects()
	{
		this._shouldRecalcRects = true;
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x001AB884 File Offset: 0x001A9A84
	private bool IsActiveFireButton()
	{
		return (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None) && !Defs.isTurretWeapon && (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.On || (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Sniper && this._playerMoveC != null && this._playerMoveC.isZooming));
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x001AB8EC File Offset: 0x001A9AEC
	private void Update()
	{
		if (this._playerMoveC == null)
		{
			if (Defs.isMulti && WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayer != null)
			{
				this._playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
			}
			else
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this._playerMoveC = gameObject.GetComponent<SkinName>().playerMoveC;
				}
			}
		}
		if (!this._joyActive)
		{
			this.isShooting = false;
			this.isJumpPressed = false;
			return;
		}
		this.isActiveFireButton = this.IsActiveFireButton();
		bool flag = this.isActiveFireButton && (!Defs.isHunger || this._hungerGameController.isGo);
		if (flag != this.fireSprite.gameObject.activeSelf)
		{
			this.fireSprite.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x001AB9EC File Offset: 0x001A9BEC
	private void OnPressure(float pressure)
	{
		if (Defs.touchPressureSupported && Defs.isUseJump3DTouch && pressure > Defs.touchPressurePower)
		{
			if (this.pressured)
			{
				return;
			}
			this.pressured = true;
			this.isJumpPressed = true;
			if (TrainingController.sharedController != null)
			{
				TrainingController.sharedController.Hide3dTouchJump();
			}
		}
		else
		{
			this.pressured = false;
			this.isJumpPressed = false;
		}
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x001ABA60 File Offset: 0x001A9C60
	private void OnPress(bool isDown)
	{
		if (!this._joyActive)
		{
			return;
		}
		if (this.inGameGUI.playerMoveC == null)
		{
			return;
		}
		if (this._fireRect.width.Equals(0f))
		{
			this.CalcRects();
		}
		if (this._isFirstFrame)
		{
			return;
		}
		if (isDown && this._fireRect.Contains(UICamera.lastTouchPosition) && this.fireSprite.gameObject.activeSelf)
		{
			this.isShooting = true;
		}
		if (!isDown)
		{
			this.isShooting = false;
			this.pressured = false;
			this.isJumpPressed = false;
		}
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001ABB14 File Offset: 0x001A9D14
	private void CalcRects()
	{
		Transform transform = NGUITools.GetRoot(base.gameObject).transform;
		Camera component = transform.GetChild(0).GetChild(0).GetComponent<Camera>();
		Transform transform2 = component.transform;
		float num = 768f;
		float num2 = num * ((float)Screen.width / (float)Screen.height);
		List<object> list = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]")) as List<object>;
		if (list == null)
		{
			list = new List<object>();
			Debug.LogWarning(list.GetType().FullName);
		}
		int[] array = list.Select(new Func<object, int>(Convert.ToInt32)).ToArray<int>();
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.fireSprite, true, true);
		float num3 = 60f;
		if (array.Length > 6)
		{
			num3 = (float)array[6] * 0.5f;
		}
		bounds.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this._fireRect = new Rect((bounds.center.x - num3) * Defs.Coef, (bounds.center.y - num3) * Defs.Coef, 2f * num3 * Defs.Coef, 2f * num3 * Defs.Coef);
	}

	// Token: 0x06004B1C RID: 19228 RVA: 0x001ABC6C File Offset: 0x001A9E6C
	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= this.SetSideAndCalcRects;
		ControlsSettingsBase.ControlsChanged -= this.SetShouldRecalcRects;
	}

	// Token: 0x040037A8 RID: 14248
	public Transform fireSprite;

	// Token: 0x040037A9 RID: 14249
	public bool isShooting;

	// Token: 0x040037AA RID: 14250
	public bool isJumpPressed;

	// Token: 0x040037AB RID: 14251
	public InGameGUI inGameGUI;

	// Token: 0x040037AC RID: 14252
	public bool isActiveFireButton;

	// Token: 0x040037AD RID: 14253
	private Rect _fireRect = default(Rect);

	// Token: 0x040037AE RID: 14254
	private bool _shouldRecalcRects;

	// Token: 0x040037AF RID: 14255
	private bool _isFirstFrame = true;

	// Token: 0x040037B0 RID: 14256
	private HungerGameController _hungerGameController;

	// Token: 0x040037B1 RID: 14257
	private bool _joyActive = true;

	// Token: 0x040037B2 RID: 14258
	private Player_move_c _playerMoveC;

	// Token: 0x040037B3 RID: 14259
	private bool pressured;
}
