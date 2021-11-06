using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000806 RID: 2054
public class TouchPadController : MonoBehaviour
{
	// Token: 0x06004AE5 RID: 19173 RVA: 0x001A971C File Offset: 0x001A791C
	public void MakeInactive()
	{
		this.jumpPressed = false;
		this.isShooting = false;
		this.isShootingPressure = false;
		this.Reset();
		this.HasAmmo();
		this._joyActive = false;
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x001A9754 File Offset: 0x001A7954
	public void MakeActive()
	{
		this._joyActive = true;
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x001A9760 File Offset: 0x001A7960
	private void Awake()
	{
		ChooseGadgetPanel.OnDisablePanel += this.ChooseGadgetPanel_OnDisablePanel;
		this.AdjustGadgetPanelVisibility();
		this.reloadUISprite = this.reloadSpirte.GetComponent<UISprite>();
		this.isHunger = Defs.isHunger;
		if (!Device.isPixelGunLow)
		{
			this._touchControlScheme = new CameraTouchControlScheme_CleanNGUI();
		}
		else if (Defs.isTouchControlSmoothDump)
		{
			this._touchControlScheme = new CameraTouchControlScheme_SmoothDump();
		}
		else
		{
			this._touchControlScheme = new CameraTouchControlScheme_CleanNGUI();
		}
	}

	// Token: 0x06004AE8 RID: 19176 RVA: 0x001A97E0 File Offset: 0x001A79E0
	private void ChooseGadgetPanel_OnDisablePanel()
	{
		if (this.chooseGadgetPanelShown)
		{
			this.HideGadgetsPanel();
			this.m_shouldHideGadgetPanel = false;
		}
		this.grenadePressed = false;
		this.isInvokeGrenadePress = false;
	}

	// Token: 0x06004AE9 RID: 19177 RVA: 0x001A9814 File Offset: 0x001A7A14
	private void OnEnable()
	{
		this.isShooting = false;
		this.isShootingPressure = false;
		if (this._shouldRecalcRects)
		{
			base.Invoke("ReCalcRects", 0.1f);
		}
		this._shouldRecalcRects = false;
		base.StartCoroutine(this._SetIsFirstFrame());
	}

	// Token: 0x06004AEA RID: 19178 RVA: 0x001A9854 File Offset: 0x001A7A54
	public static int GetGrenadeCount()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			return WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount;
		}
		return 0;
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x001A9898 File Offset: 0x001A7A98
	private static bool IsButtonGrenadeVisible()
	{
		return ((InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive && !Defs.isTurretWeapon) || InGameGUI.sharedInGameGUI.playerMoveC == null) && !Defs.isZooming;
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x001A98F8 File Offset: 0x001A7AF8
	private bool IsUseGrenadeActive()
	{
		return TouchPadController.IsButtonGrenadeVisible() && Defs.isGrenateFireEnable && (!this.isHunger || this.hungerGameController.isGo) && (!Defs.isDaterRegim || TouchPadController.GetGrenadeCount() > 0) && WeaponManager.sharedManager._currentFilterMap != 1 && WeaponManager.sharedManager._currentFilterMap != 2 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None);
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x001A9984 File Offset: 0x001A7B84
	public static bool IsBuyGrenadeActive()
	{
		return TouchPadController.IsButtonGrenadeVisible() && Defs.isDaterRegim && WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount <= 0;
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x001A99E4 File Offset: 0x001A7BE4
	private void SetSpritesState()
	{
		this.SetGrenadeUISpriteState();
		if (WeaponManager.sharedManager != null)
		{
			string name = WeaponManager.sharedManager.currentWeaponSounds.gameObject.name;
			if (!Defs.isTurretWeapon && !name.Contains("Weapon"))
			{
				return;
			}
		}
		this.jumpSprite.gameObject.SetActive((Defs.isJumpAndShootButtonOn || !Defs.isUseJump3DTouch) && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None || TrainingController.stepTraining >= TrainingState.GetTheGun));
		bool flag = TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled;
		this.fireSprite.gameObject.SetActive((Defs.isJumpAndShootButtonOn || !Defs.isUseShoot3DTouch) && !Defs.isTurretWeapon && flag && (!this.isHunger || this.hungerGameController.isGo) && !WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon);
		this.reloadSpirte.gameObject.SetActive(((InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive) || InGameGUI.sharedInGameGUI.playerMoveC == null) && !Defs.isTurretWeapon && flag && (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && !WeaponManager.sharedManager.currentWeaponSounds.isMelee) && !WeaponManager.sharedManager.currentWeaponSounds.isShotMelee);
		this.zoomSprite.gameObject.SetActive(((InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive) || InGameGUI.sharedInGameGUI.playerMoveC == null) && !Defs.isTurretWeapon && flag && (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null) && WeaponManager.sharedManager.currentWeaponSounds.isZooming);
		if (this.jumpIcon.activeSelf == Defs.isJetpackEnabled)
		{
			this.jumpIcon.SetActive(!Defs.isJetpackEnabled);
		}
		if (this.jetPackIcon.activeSelf != Defs.isJetpackEnabled)
		{
			this.jetPackIcon.SetActive(Defs.isJetpackEnabled);
		}
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x001A9C98 File Offset: 0x001A7E98
	private void SetGrenadeUISpriteState()
	{
		if (!Defs.isDaterRegim)
		{
			return;
		}
		this.grenadeButton.gameObject.SetActiveSafeSelf(true);
		this.chooseGadgetPanel.gameObject.SetActiveSafeSelf(false);
		bool flag = TouchPadController.IsBuyGrenadeActive();
		bool flag2 = this.IsUseGrenadeActive();
		this.grenadeButton.gameObject.SetActiveSafeSelf(flag || flag2);
		if (!this.grenadeButton.gameObject.activeSelf)
		{
			return;
		}
		this.grenadeButton.grenadeSprite.spriteName = (((!this.grenadePressed && !this._isBuyGrenadePressed) || !this.grenadeRect.Contains(UICamera.lastTouchPosition)) ? ((!Defs.isDaterRegim) ? "grenade_btn" : "grenade_like_btn") : ((!Defs.isDaterRegim) ? "grenade_btn_n" : "grenade_like_btn_n"));
		if (flag)
		{
			if (Defs.isDaterRegim)
			{
				this.grenadeButton.priceLabel.gameObject.SetActiveSafeSelf(true);
				this.grenadeButton.countLabel.gameObject.SetActiveSafeSelf(false);
				this.grenadeButton.fullLabel.gameObject.SetActiveSafeSelf(false);
			}
			else
			{
				this.grenadeButton.gameObject.SetActiveSafeSelf(false);
			}
		}
		else
		{
			this.grenadeButton.gameObject.SetActiveSafeSelf(true);
			this.grenadeButton.priceLabel.gameObject.SetActiveSafeSelf(false);
			int grenadeCount = TouchPadController.GetGrenadeCount();
			this.grenadeButton.countLabel.gameObject.SetActiveSafeSelf(true);
			this.grenadeButton.countLabel.text = grenadeCount.ToString();
			this.grenadeButton.fullLabel.gameObject.SetActiveSafeSelf(false);
		}
	}

	// Token: 0x06004AF0 RID: 19184 RVA: 0x001A9E60 File Offset: 0x001A8060
	private void SetSide()
	{
		bool flag = (base.GetComponent<UIAnchor>().side == UIAnchor.Side.BottomRight && GlobalGameController.LeftHanded) || (base.GetComponent<UIAnchor>().side == UIAnchor.Side.BottomLeft && !GlobalGameController.LeftHanded);
		base.GetComponent<UIAnchor>().side = ((!GlobalGameController.LeftHanded) ? UIAnchor.Side.BottomLeft : UIAnchor.Side.BottomRight);
		Vector3 center = base.GetComponent<BoxCollider>().center;
		center.x *= ((!flag) ? -1f : 1f);
		base.GetComponent<BoxCollider>().center = center;
		this.chooseGadgetPanel.transform.localScale = new Vector3((float)((!GlobalGameController.LeftHanded) ? -1 : 1), 1f, 1f);
		for (int i = 0; i < this.chooseGadgetPanel.objectsNoFilpXScale.Count; i++)
		{
			this.chooseGadgetPanel.objectsNoFilpXScale[i].localScale = new Vector3((float)((!GlobalGameController.LeftHanded) ? -1 : 1), this.chooseGadgetPanel.objectsNoFilpXScale[i].localScale.y, this.chooseGadgetPanel.objectsNoFilpXScale[i].localScale.z);
		}
	}

	// Token: 0x06004AF1 RID: 19185 RVA: 0x001A9FBC File Offset: 0x001A81BC
	private void SetSideAndCalcRects()
	{
		this.SetSide();
		this.SetShouldRecalcRects();
	}

	// Token: 0x06004AF2 RID: 19186 RVA: 0x001A9FCC File Offset: 0x001A81CC
	private void SetShouldRecalcRects()
	{
		this._shouldRecalcRects = true;
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x001A9FD8 File Offset: 0x001A81D8
	[Obfuscation(Exclude = true)]
	private void ReCalcRects()
	{
		this.CalcRects(true);
	}

	// Token: 0x06004AF4 RID: 19188 RVA: 0x001A9FE4 File Offset: 0x001A81E4
	private IEnumerator Start()
	{
		this.SetSide();
		PauseNGUIController.PlayerHandUpdated += this.SetSideAndCalcRects;
		ControlsSettingsBase.ControlsChanged += this.SetShouldRecalcRects;
		if (this.isHunger)
		{
			this.hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
		}
		this.SetSpritesState();
		yield return null;
		this.CalcRects(false);
		this.Reset();
		yield break;
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x001AA000 File Offset: 0x001A8200
	public void Reset()
	{
		this._touchControlScheme.Reset();
	}

	// Token: 0x06004AF6 RID: 19190 RVA: 0x001AA010 File Offset: 0x001A8210
	public void HideButtonsOnGadgetPanel()
	{
		this.AlphaUpdate();
		if (!this.gadgetPanelVisible)
		{
			this.fireAlpha = 1f;
			this.jumpAlpha = 1f;
			this.reloadAlpha = 1f;
			this.zoomAlpha = 1f;
			return;
		}
		Transform transform = NGUITools.GetRoot(base.gameObject).transform;
		Camera component = transform.GetChild(0).GetChild(0).GetComponent<Camera>();
		Transform transform2 = component.transform;
		float num = 768f;
		float num2 = num * ((float)Screen.width / (float)Screen.height);
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.chooseGadgetPanel.gadgetButtonScript.ContainerForScale, true, true);
		bounds.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		Rect rect = new Rect((bounds.center.x - bounds.size.x / 2f) * Defs.Coef, (bounds.center.y - bounds.size.y / 2f) * Defs.Coef, bounds.size.x * Defs.Coef, bounds.size.y * Defs.Coef);
		this.fireAlpha = (float)((!rect.Overlaps(this.fireRect)) ? 1 : 0);
		this.jumpAlpha = (float)((!rect.Overlaps(this.jumpRect)) ? 1 : 0);
		this.reloadAlpha = (float)((!rect.Overlaps(this.reloadRect)) ? 1 : 0);
		this.zoomAlpha = (float)((!rect.Overlaps(this.zoomRect)) ? 1 : 0);
	}

	// Token: 0x06004AF7 RID: 19191 RVA: 0x001AA1F0 File Offset: 0x001A83F0
	private void AlphaUpdate()
	{
		if (this.fireSprite.GetComponent<UISprite>().alpha != this.fireAlpha)
		{
			this.fireSprite.GetComponent<UISprite>().alpha = Mathf.MoveTowards(this.fireSprite.GetComponent<UISprite>().alpha, this.fireAlpha, Time.deltaTime * 7f);
		}
		if (this.jumpSprite.GetComponent<UISprite>().alpha != this.jumpAlpha)
		{
			this.jumpSprite.GetComponent<UISprite>().alpha = Mathf.MoveTowards(this.jumpSprite.GetComponent<UISprite>().alpha, this.jumpAlpha, Time.deltaTime * 7f);
		}
		if (this.reloadSpirte.GetComponent<UISprite>().alpha != this.reloadAlpha)
		{
			this.reloadSpirte.GetComponent<UISprite>().alpha = Mathf.MoveTowards(this.reloadSpirte.GetComponent<UISprite>().alpha, this.reloadAlpha, Time.deltaTime * 7f);
		}
		if (this.zoomSprite.GetComponent<UISprite>().alpha != this.zoomAlpha)
		{
			this.zoomSprite.GetComponent<UISprite>().alpha = Mathf.MoveTowards(this.zoomSprite.GetComponent<UISprite>().alpha, this.zoomAlpha, Time.deltaTime * 7f);
		}
	}

	// Token: 0x06004AF8 RID: 19192 RVA: 0x001AA344 File Offset: 0x001A8544
	private void CalcRects(bool forceRecalculation = false)
	{
		if (forceRecalculation)
		{
			this.rectsCalculated = false;
		}
		if (this.rectsCalculated)
		{
			return;
		}
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
		float num3 = 62f;
		if (array.Length > 3)
		{
			num3 = (float)array[3] * 0.5f;
		}
		bounds.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.fireRect = new Rect((bounds.center.x - num3) * Defs.Coef, (bounds.center.y - num3) * Defs.Coef, 2f * num3 * Defs.Coef, 2f * num3 * Defs.Coef);
		Bounds bounds2 = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.jumpSprite, true, true);
		bounds2.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		float num4 = 62f;
		if (array.Length > 2)
		{
			num4 = (float)array[2] * 0.5f;
		}
		this.jumpRect = new Rect((bounds2.center.x - num4 * 0.7f) * Defs.Coef, (bounds2.center.y - num4) * Defs.Coef, 2f * num4 * Defs.Coef, 2f * num4 * Defs.Coef);
		Bounds bounds3 = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.reloadSpirte, true, true);
		float num5 = 55f;
		if (array.Length > 1)
		{
			num5 = (float)array[1] * 0.5f;
		}
		bounds3.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.reloadRect = new Rect((bounds3.center.x - num5) * Defs.Coef, (bounds3.center.y - num5) * Defs.Coef, 2f * num5 * Defs.Coef, 2f * num5 * Defs.Coef);
		float num6 = 55f;
		if (array.Length > 0)
		{
			num6 = (float)array[0] * 0.5f;
		}
		Bounds bounds4 = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.zoomSprite, true, true);
		bounds4.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.zoomRect = new Rect((bounds4.center.x - num6) * Defs.Coef, (bounds4.center.y - num6) * Defs.Coef, 2f * num6 * Defs.Coef, 2f * num6 * Defs.Coef);
		float num7 = 45f;
		if (array.Length > 5)
		{
			num7 = (float)array[5] * 0.5f;
		}
		float num8 = num7 * 2f / 90f;
		Bounds bounds5 = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.chooseGadgetPanel.gadgetButtonScript.gadgetIcon.transform, true, true);
		bounds5.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.grenadeRect = new Rect((bounds5.center.x - num7) * Defs.Coef, (bounds5.center.y - num7) * Defs.Coef, 2f * num7 * Defs.Coef, 2f * num7 * Defs.Coef);
		this.availableGadget1Rect.Set(this.grenadeRect.x - this.chooseGadgetPanel.transform.localScale.x * (this.grenadeRect.width + 20f * num8 * Defs.Coef), this.grenadeRect.y, this.grenadeRect.width, this.grenadeRect.height);
		this.availableGadget2Rect.Set(this.grenadeRect.x - this.chooseGadgetPanel.transform.localScale.x * (this.grenadeRect.width + 20f * num8 * Defs.Coef) * 2f, this.grenadeRect.y, this.grenadeRect.width, this.grenadeRect.height);
		float num9 = (float)Screen.height * 0.81f;
		if (!GlobalGameController.LeftHanded)
		{
			this.moveRect = new Rect(0f, 0f, num9, (float)Screen.height * 0.65f);
		}
		else
		{
			this.moveRect = new Rect((float)Screen.width - num9, 0f, num9, (float)Screen.height * 0.65f);
		}
		this.rectsCalculated = (!this.fireRect.width.Equals(0f) && Screen.width > Screen.height);
		Debug.Log("Control rects calculated. Success == " + this.rectsCalculated);
	}

	// Token: 0x06004AF9 RID: 19193 RVA: 0x001AA934 File Offset: 0x001A8B34
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

	// Token: 0x06004AFA RID: 19194 RVA: 0x001AA950 File Offset: 0x001A8B50
	private void AdjustGadgetPanelVisibility()
	{
		bool flag = WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.showGadgetsPanel;
		if (flag != this.chooseGadgetPanel.gameObject.activeSelf)
		{
			this.chooseGadgetPanel.gameObject.SetActiveSafeSelf(flag);
		}
	}

	// Token: 0x06004AFB RID: 19195 RVA: 0x001AA9AC File Offset: 0x001A8BAC
	private void Update()
	{
		this.HideButtonsOnGadgetPanel();
		if (this.m_shouldHideGadgetPanel && Time.realtimeSinceStartup - 5f >= this.m_hideGadgetsPanelSettedTime)
		{
			this.HideGadgetsPanel();
			this.m_shouldHideGadgetPanel = false;
		}
		this.AdjustGadgetPanelVisibility();
		this.cantUseGadget.SetActive(WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.gadgetsDisabled);
		if (this.chooseGadgetPanelShown)
		{
			this.chooseGadgetPanel.hoverBackground1.SetActiveSafeSelf(this.grenadeRect.Contains(UICamera.lastTouchPosition));
			this.chooseGadgetPanel.hoverBackground2.SetActiveSafeSelf(this.availableGadget1Rect.Contains(UICamera.lastTouchPosition));
			this.chooseGadgetPanel.hoverBackground3.SetActiveSafeSelf(this.availableGadget2Rect.Contains(UICamera.lastTouchPosition));
		}
		this.framesCount++;
		this.CalcRects(false);
		this.SetSpritesState();
		this._isFirstFrame = false;
		if (!this._joyActive)
		{
			this.jumpPressed = false;
			this.isShooting = false;
			this.isShootingPressure = false;
			this._touchControlScheme.Reset();
			return;
		}
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (this.gadgetsTouchID == -1)
			{
				if (touch.phase == TouchPhase.Began && this.grenadeRect.Contains(touch.position))
				{
					this.gadgetsTouchID = touch.fingerId;
					this.OnGadgetPanelClick(true, touch.position);
				}
			}
			else if (touch.fingerId == this.gadgetsTouchID)
			{
				if (!Defs.isDaterRegim && this.isInvokeGrenadePress && this.chooseGadgetPanel.CanExtend() && (touch.position - this._initialGrenadePressPosition).sqrMagnitude > (float)(TouchPadController.thresholdGadgetPanel * TouchPadController.thresholdGadgetPanel) * Defs.Coef)
				{
					this.isInvokeGrenadePress = false;
					base.CancelInvoke("GrenadePressInvoke");
					this.chooseGadgetPanelShown = true;
					this.chooseGadgetPanel.Show();
				}
				if (touch.phase == TouchPhase.Ended)
				{
					this.gadgetsTouchID = -1;
					this.OnGadgetPanelClick(false, default(Vector3));
				}
			}
			if (touch.phase == TouchPhase.Began && this.chooseGadgetPanelShown)
			{
				bool flag = this.ProcessPressOnGadgetsPanel(touch.position);
				if (flag)
				{
					this.m_shouldHideGadgetPanel = false;
				}
			}
			if (touch.phase == TouchPhase.Ended && this.chooseGadgetPanelShown)
			{
				bool flag2 = this.ProcessPressOnGadgetsPanel(touch.position);
				if (this.chooseGadgetPanelShown && !this.m_shouldHideGadgetPanel)
				{
					this.m_shouldHideGadgetPanel = true;
					this.m_hideGadgetsPanelSettedTime = Time.realtimeSinceStartup;
				}
			}
		}
		this._touchControlScheme.OnUpdate();
		touchCount = Input.touchCount;
		if (touchCount > 0)
		{
			if (this.MoveTouchID == -1)
			{
				for (int j = 0; j < touchCount; j++)
				{
					Touch touch2 = Input.GetTouch(j);
					if (touch2.phase == TouchPhase.Began && this.moveRect.Contains(touch2.position))
					{
						this.MoveTouchID = touch2.fingerId;
					}
				}
			}
			if (!this.gadgetsPanelPress)
			{
				this.UpdateMoveTouch();
			}
		}
		else
		{
			this.MoveTouchID = -1;
			this.pastPos = Vector2.zero;
			this.pastDelta = Vector2.zero;
		}
	}

	// Token: 0x06004AFC RID: 19196 RVA: 0x001AAD2C File Offset: 0x001A8F2C
	private void UpdateMoveTouch()
	{
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.fingerId == this.MoveTouchID)
			{
				if (touch.phase == TouchPhase.Ended)
				{
					this.MoveTouchID = -1;
					this.pastPos = Vector2.zero;
					this.pastDelta = Vector2.zero;
					this.firstDeltaSkip = false;
					return;
				}
				if (this.pastPos == Vector2.zero)
				{
					this.pastPos = touch.position;
				}
				Vector2 vector = touch.position - this.pastPos;
				if (vector == Vector2.zero && this.pastDelta != Vector2.zero && this.IsDifferendDirections(this.pastDelta, JoystickController.leftJoystick.value) && this.pastDelta.sqrMagnitude > 15f)
				{
					vector = this.pastDelta / 2f;
					this.compDeltas += vector;
				}
				else
				{
					vector -= this.compDeltas;
					this.compDeltas = Vector2.zero;
				}
				if (this.firstDeltaSkip)
				{
					this.OnDragTouch(vector);
				}
				else if (vector.sqrMagnitude > 1E-45f)
				{
					this.firstDeltaSkip = true;
				}
				this.pastPos = touch.position;
				this.pastDelta = vector;
			}
		}
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x001AAEAC File Offset: 0x001A90AC
	private void OnGadgetPanelClick(bool pressed, [Optional] Vector3 clickPos)
	{
		if (pressed)
		{
			if (!this.chooseGadgetPanelShown)
			{
				if (TouchPadController.IsBuyGrenadeActive())
				{
					this.BuyGrenadePressInvoke();
				}
				else if (!(InGameGUI.sharedInGameGUI != null) || !(InGameGUI.sharedInGameGUI.changeWeaponScroll != null) || !InGameGUI.sharedInGameGUI.changeWeaponScroll.isDragging)
				{
					if (Defs.isDaterRegim)
					{
						if (this.IsUseGrenadeActive())
						{
							this.GrenadePressInvoke();
						}
					}
					else if (!this.grenadePressed && WeaponManager.sharedManager.myPlayerMoveC.canUseGadgets)
					{
						this._initialGrenadePressPosition = clickPos;
						this.isInvokeGrenadePress = true;
						this.gadgetsPanelPress = true;
						base.Invoke("GrenadePressInvoke", TouchPadController.timeGadgetPanel);
					}
				}
			}
		}
		else if (this.grenadePressed)
		{
			this.grenadePressed = false;
			if (Defs.isDaterRegim)
			{
				this.grenadeButton.grenadeSprite.spriteName = ((!Defs.isDaterRegim) ? "grenade_btn" : "grenade_like_btn");
				this.move.GrenadeFire();
			}
			else
			{
				Gadget gadget = null;
				if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget) && gadget.CanUse)
				{
					gadget.Use();
				}
			}
		}
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x001AB004 File Offset: 0x001A9204
	private bool IsDifferendDirections(Vector2 delta1, Vector2 delta2)
	{
		return JoystickController.leftJoystick.value != Vector2.zero && Mathf.Sign(delta1.x) != Mathf.Sign(delta2.x);
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x001AB040 File Offset: 0x001A9240
	public void HasAmmo()
	{
		BlinkReloadButton.isBlink = false;
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x001AB048 File Offset: 0x001A9248
	public void NoAmmo()
	{
		BlinkReloadButton.isBlink = true;
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x001AB050 File Offset: 0x001A9250
	[Obfuscation(Exclude = true)]
	private IEnumerator BlinkReload()
	{
		for (;;)
		{
			yield return new WaitForSeconds(0.5f);
			this.reloadUISprite.spriteName = "Reload_0";
			yield return new WaitForSeconds(0.5f);
			this.reloadUISprite.spriteName = "Reload_1";
		}
		yield break;
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x001AB06C File Offset: 0x001A926C
	private bool ProcessPressOnGadgetsPanel(Vector2 pos)
	{
		if (this.grenadeRect.Contains(pos))
		{
			this.chooseGadgetPanel.ChooseDefault();
			this.HideGadgetsPanel();
			return true;
		}
		if (this.availableGadget1Rect.Contains(pos))
		{
			this.chooseGadgetPanel.ChooseFirst();
			this.HideGadgetsPanel();
			return true;
		}
		if (this.availableGadget2Rect.Contains(pos))
		{
			this.chooseGadgetPanel.ChooseSecond();
			this.HideGadgetsPanel();
			return true;
		}
		return false;
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x001AB0E8 File Offset: 0x001A92E8
	private void HideGadgetsPanel()
	{
		this.chooseGadgetPanel.Hide();
		this.chooseGadgetPanelShown = false;
		this.chooseGadgetPanel.hoverBackground1.SetActiveSafeSelf(false);
		this.chooseGadgetPanel.hoverBackground2.SetActiveSafeSelf(false);
		this.chooseGadgetPanel.hoverBackground3.SetActiveSafeSelf(false);
	}

	// Token: 0x06004B04 RID: 19204 RVA: 0x001AB13C File Offset: 0x001A933C
	private void OnPress(bool isDown)
	{
		this._touchControlScheme.OnPress(isDown);
		if (!this.move)
		{
			if (!Defs.isMulti)
			{
				this.move = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
			}
			else
			{
				this.move = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		this.CalcRects(false);
		if (!this._joyActive)
		{
			return;
		}
		if (this._isFirstFrame)
		{
			return;
		}
		if (isDown)
		{
		}
		if (isDown && this.fireRect.Contains(UICamera.lastTouchPosition) && (Defs.isJumpAndShootButtonOn || !Defs.isUseShoot3DTouch) && !this.gadgetsPanelPress && this.fireAlpha == 1f)
		{
			this.isShooting = true;
		}
		if (isDown && this.jumpRect.Contains(UICamera.lastTouchPosition) && (Defs.isJumpAndShootButtonOn || !Defs.isUseJump3DTouch) && !this.gadgetsPanelPress && this.jumpAlpha == 1f && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None || TrainingController.stepTraining >= TrainingState.GetTheGun))
		{
			this.jumpPressed = true;
		}
		if (isDown && ((InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive) || InGameGUI.sharedInGameGUI.playerMoveC == null) && this.reloadRect.Contains(UICamera.lastTouchPosition) && !this.gadgetsPanelPress && this.reloadAlpha == 1f && this.move && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled))
		{
			this.move.ReloadPressed();
		}
		bool flag = this.zoomSprite != null && this.zoomSprite.gameObject.activeInHierarchy;
		if (isDown && ((InGameGUI.sharedInGameGUI.playerMoveC != null && !InGameGUI.sharedInGameGUI.playerMoveC.isMechActive) || InGameGUI.sharedInGameGUI.playerMoveC == null) && flag && this.zoomRect.Contains(UICamera.lastTouchPosition) && !this.gadgetsPanelPress && this.zoomAlpha == 1f && this.move && WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && WeaponManager.sharedManager.currentWeaponSounds.isZooming)
		{
			this.move.ZoomPress();
		}
		if (!isDown)
		{
			this.gadgetsPanelPress = false;
			if (this.isInvokeGrenadePress)
			{
				this.isInvokeGrenadePress = false;
				base.CancelInvoke("GrenadePressInvoke");
				Gadget gadget = null;
				if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget) && gadget.CanUse)
				{
					gadget.PreUse();
					gadget.Use();
				}
			}
			if (this._isBuyGrenadePressed)
			{
				this._isBuyGrenadePressed = false;
				this.grenadeButton.grenadeSprite.spriteName = ((!Defs.isDaterRegim) ? "grenade_btn" : "grenade_like_btn");
				if (this.grenadeRect.Contains(UICamera.lastTouchPosition))
				{
					InGameGUI.sharedInGameGUI.HandleBuyGrenadeClicked(null, EventArgs.Empty);
				}
			}
			this.isShooting = false;
			this.jumpPressed = false;
			this.isShootingPressure = false;
		}
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x001AB4F4 File Offset: 0x001A96F4
	[Obfuscation(Exclude = true)]
	private void GrenadePressInvoke()
	{
		this.gadgetsPanelPress = false;
		Gadget gadget = null;
		bool flag;
		if (Defs.isDaterRegim)
		{
			this.grenadeButton.grenadeSprite.spriteName = "grenade_like_btn_n";
			gadget = WeaponManager.sharedManager.myPlayerMoveC.daterLikeGadget;
			flag = true;
		}
		else
		{
			InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget);
			flag = (WeaponManager.sharedManager.myPlayerMoveC.canUseGadgets && gadget.CanUse);
		}
		if (gadget != null && flag)
		{
			this.isInvokeGrenadePress = false;
			this.grenadePressed = true;
			gadget.PreUse();
		}
	}

	// Token: 0x06004B06 RID: 19206 RVA: 0x001AB594 File Offset: 0x001A9794
	private void BuyGrenadePressInvoke()
	{
		this._isBuyGrenadePressed = true;
		this.grenadeButton.grenadeSprite.spriteName = "grenade_like_btn_n";
	}

	// Token: 0x06004B07 RID: 19207 RVA: 0x001AB5B4 File Offset: 0x001A97B4
	private void OnPressure(float pressure)
	{
		if (Defs.touchPressureSupported && Defs.isUseShoot3DTouch && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			bool flag = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled) && (!this.isHunger || this.hungerGameController.isGo) && this.hasAmmo && !WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon;
			if (flag && pressure > Defs.touchPressurePower)
			{
				this.isShootingPressure = true;
			}
			else
			{
				this.isShootingPressure = false;
			}
		}
	}

	// Token: 0x06004B08 RID: 19208 RVA: 0x001AB66C File Offset: 0x001A986C
	private void OnDragTouch(Vector2 delta)
	{
		if (!this._joyActive)
		{
			this.jumpPressed = false;
			this.isShooting = false;
			this._touchControlScheme.ResetDelta();
			return;
		}
		this.framesCount = 0;
		this._touchControlScheme.OnDrag(delta);
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x001AB6B4 File Offset: 0x001A98B4
	private void OnDestroy()
	{
		PauseNGUIController.PlayerHandUpdated -= this.SetSideAndCalcRects;
		ControlsSettingsBase.ControlsChanged -= this.SetShouldRecalcRects;
		ChooseGadgetPanel.OnDisablePanel -= this.ChooseGadgetPanel_OnDisablePanel;
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x001AB6EC File Offset: 0x001A98EC
	public Vector2 GrabDeltaPosition()
	{
		Vector2 result = Vector2.zero;
		if (this._touchControlScheme != null)
		{
			result = this._touchControlScheme.DeltaPosition;
			this._touchControlScheme.ResetDelta();
		}
		return result;
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x001AB724 File Offset: 0x001A9924
	public void ApplyDeltaTo(Vector2 deltaPosition, Transform yawTransform, Transform pitchTransform, float sensitivity, bool invert)
	{
		if (this._touchControlScheme != null)
		{
			this._touchControlScheme.ApplyDeltaTo(deltaPosition, yawTransform, pitchTransform, sensitivity, invert);
		}
	}

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06004B0C RID: 19212 RVA: 0x001AB744 File Offset: 0x001A9944
	// (set) Token: 0x06004B0D RID: 19213 RVA: 0x001AB74C File Offset: 0x001A994C
	public CameraTouchControlScheme touchControlScheme
	{
		get
		{
			return this._touchControlScheme;
		}
		set
		{
			this._touchControlScheme = value;
			this._touchControlScheme.Reset();
		}
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x001AB760 File Offset: 0x001A9960
	public void OnDisable()
	{
		this.jumpPressed = false;
		this.isShooting = false;
		this.isShootingPressure = false;
		this.MoveTouchID = -1;
		this.gadgetsTouchID = -1;
		this.ChooseGadgetPanel_OnDisablePanel();
	}

	// Token: 0x0400376F RID: 14191
	public const string GRENADE_BUY_NORMAL_SPRITE_NAME = "grenade_btn";

	// Token: 0x04003770 RID: 14192
	private const string GRENADE_BUY_PRESSED_SPRITE_NAME = "grenade_btn_n";

	// Token: 0x04003771 RID: 14193
	public const string LIKE_BUY_NORMAL_SPRITE_NAME = "grenade_like_btn";

	// Token: 0x04003772 RID: 14194
	private const string LIKE_BUY_PRESSED_SPRITE_NAME = "grenade_like_btn_n";

	// Token: 0x04003773 RID: 14195
	public static int thresholdGadgetPanel = 70;

	// Token: 0x04003774 RID: 14196
	public static float timeGadgetPanel = 0.5f;

	// Token: 0x04003775 RID: 14197
	public ChooseGadgetPanel chooseGadgetPanel;

	// Token: 0x04003776 RID: 14198
	public GrenadeButton grenadeButton;

	// Token: 0x04003777 RID: 14199
	public bool grenadePressed;

	// Token: 0x04003778 RID: 14200
	public bool jumpPressed;

	// Token: 0x04003779 RID: 14201
	public Transform fireSprite;

	// Token: 0x0400377A RID: 14202
	public Transform jumpSprite;

	// Token: 0x0400377B RID: 14203
	public Transform reloadSpirte;

	// Token: 0x0400377C RID: 14204
	public Transform zoomSprite;

	// Token: 0x0400377D RID: 14205
	public bool hasAmmo = true;

	// Token: 0x0400377E RID: 14206
	public bool _isFirstFrame = true;

	// Token: 0x0400377F RID: 14207
	public GameObject jetPackIcon;

	// Token: 0x04003780 RID: 14208
	public GameObject jumpIcon;

	// Token: 0x04003781 RID: 14209
	public GameObject cantUseGadget;

	// Token: 0x04003782 RID: 14210
	private Rect grenadeRect = default(Rect);

	// Token: 0x04003783 RID: 14211
	private Rect availableGadget1Rect = default(Rect);

	// Token: 0x04003784 RID: 14212
	private Rect availableGadget2Rect = default(Rect);

	// Token: 0x04003785 RID: 14213
	private bool isInvokeGrenadePress;

	// Token: 0x04003786 RID: 14214
	private bool chooseGadgetPanelShown;

	// Token: 0x04003787 RID: 14215
	private bool gadgetsPanelPress;

	// Token: 0x04003788 RID: 14216
	private Vector2 _initialGrenadePressPosition;

	// Token: 0x04003789 RID: 14217
	private UISprite reloadUISprite;

	// Token: 0x0400378A RID: 14218
	public bool isShooting;

	// Token: 0x0400378B RID: 14219
	public bool isShootingPressure;

	// Token: 0x0400378C RID: 14220
	private bool isHunger;

	// Token: 0x0400378D RID: 14221
	private Player_move_c move;

	// Token: 0x0400378E RID: 14222
	private HungerGameController hungerGameController;

	// Token: 0x0400378F RID: 14223
	private Rect fireRect = default(Rect);

	// Token: 0x04003790 RID: 14224
	private Rect jumpRect = default(Rect);

	// Token: 0x04003791 RID: 14225
	private Rect reloadRect;

	// Token: 0x04003792 RID: 14226
	private Rect zoomRect;

	// Token: 0x04003793 RID: 14227
	private Rect moveRect;

	// Token: 0x04003794 RID: 14228
	private bool _joyActive = true;

	// Token: 0x04003795 RID: 14229
	private bool _isBuyGrenadePressed;

	// Token: 0x04003796 RID: 14230
	private CameraTouchControlScheme _touchControlScheme;

	// Token: 0x04003797 RID: 14231
	private bool _shouldRecalcRects;

	// Token: 0x04003798 RID: 14232
	public bool gadgetPanelVisible;

	// Token: 0x04003799 RID: 14233
	private float zoomAlpha = 1f;

	// Token: 0x0400379A RID: 14234
	private float reloadAlpha = 1f;

	// Token: 0x0400379B RID: 14235
	private float jumpAlpha = 1f;

	// Token: 0x0400379C RID: 14236
	private float fireAlpha = 1f;

	// Token: 0x0400379D RID: 14237
	private bool rectsCalculated;

	// Token: 0x0400379E RID: 14238
	private int framesCount;

	// Token: 0x0400379F RID: 14239
	private int selectedGadget;

	// Token: 0x040037A0 RID: 14240
	private int MoveTouchID = -1;

	// Token: 0x040037A1 RID: 14241
	private int gadgetsTouchID = -1;

	// Token: 0x040037A2 RID: 14242
	private bool firstDeltaSkip;

	// Token: 0x040037A3 RID: 14243
	private Vector2 pastPos = Vector2.zero;

	// Token: 0x040037A4 RID: 14244
	private Vector2 pastDelta = Vector2.zero;

	// Token: 0x040037A5 RID: 14245
	private Vector2 compDeltas = Vector2.zero;

	// Token: 0x040037A6 RID: 14246
	private bool m_shouldHideGadgetPanel;

	// Token: 0x040037A7 RID: 14247
	private float m_hideGadgetsPanelSettedTime;
}
