using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000399 RID: 921
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x00096278 File Offset: 0x00094478
	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06002070 RID: 8304 RVA: 0x0009627C File Offset: 0x0009447C
	// (set) Token: 0x06002071 RID: 8305 RVA: 0x00096294 File Offset: 0x00094494
	public static bool disableController
	{
		get
		{
			return UICamera.mDisableController && !UIPopupList.isOpen;
		}
		set
		{
			UICamera.mDisableController = value;
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06002072 RID: 8306 RVA: 0x0009629C File Offset: 0x0009449C
	// (set) Token: 0x06002073 RID: 8307 RVA: 0x000962A4 File Offset: 0x000944A4
	[Obsolete("Use lastEventPosition instead. It handles controller input properly.")]
	public static Vector2 lastTouchPosition
	{
		get
		{
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06002074 RID: 8308 RVA: 0x000962AC File Offset: 0x000944AC
	// (set) Token: 0x06002075 RID: 8309 RVA: 0x00096308 File Offset: 0x00094508
	public static Vector2 lastEventPosition
	{
		get
		{
			UICamera.ControlScheme currentScheme = UICamera.currentScheme;
			if (currentScheme == UICamera.ControlScheme.Controller)
			{
				GameObject hoveredObject = UICamera.hoveredObject;
				if (hoveredObject != null)
				{
					Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(hoveredObject.transform);
					Camera camera = NGUITools.FindCameraForLayer(hoveredObject.layer);
					return camera.WorldToScreenPoint(bounds.center);
				}
			}
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06002076 RID: 8310 RVA: 0x00096310 File Offset: 0x00094510
	public static UICamera first
	{
		get
		{
			if (UICamera.list == null || UICamera.list.size == 0)
			{
				return null;
			}
			return UICamera.list[0];
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06002077 RID: 8311 RVA: 0x00096344 File Offset: 0x00094544
	// (set) Token: 0x06002078 RID: 8312 RVA: 0x000963B4 File Offset: 0x000945B4
	public static UICamera.ControlScheme currentScheme
	{
		get
		{
			if (UICamera.mCurrentKey == KeyCode.None)
			{
				return UICamera.ControlScheme.Touch;
			}
			if (UICamera.mCurrentKey >= KeyCode.JoystickButton0)
			{
				return UICamera.ControlScheme.Controller;
			}
			if (UICamera.current != null && UICamera.mLastScheme == UICamera.ControlScheme.Controller && (UICamera.mCurrentKey == UICamera.current.submitKey0 || UICamera.mCurrentKey == UICamera.current.submitKey1))
			{
				return UICamera.ControlScheme.Controller;
			}
			return UICamera.ControlScheme.Mouse;
		}
		set
		{
			if (value == UICamera.ControlScheme.Mouse)
			{
				UICamera.currentKey = KeyCode.Mouse0;
			}
			else if (value == UICamera.ControlScheme.Controller)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
			}
			else if (value == UICamera.ControlScheme.Touch)
			{
				UICamera.currentKey = KeyCode.None;
			}
			else
			{
				UICamera.currentKey = KeyCode.Alpha0;
			}
			UICamera.mLastScheme = value;
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06002079 RID: 8313 RVA: 0x0009640C File Offset: 0x0009460C
	// (set) Token: 0x0600207A RID: 8314 RVA: 0x00096414 File Offset: 0x00094614
	public static KeyCode currentKey
	{
		get
		{
			return UICamera.mCurrentKey;
		}
		set
		{
			if (UICamera.mCurrentKey != value)
			{
				UICamera.ControlScheme controlScheme = UICamera.mLastScheme;
				UICamera.mCurrentKey = value;
				UICamera.mLastScheme = UICamera.currentScheme;
				if (controlScheme != UICamera.mLastScheme)
				{
					UICamera.HideTooltip();
					if (UICamera.mLastScheme == UICamera.ControlScheme.Mouse)
					{
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}
					else if (UICamera.current != null && UICamera.current.autoHideCursor)
					{
						Cursor.visible = false;
						Cursor.lockState = CursorLockMode.Locked;
						UICamera.mMouse[0].ignoreDelta = 2;
					}
					if (UICamera.onSchemeChange != null)
					{
						UICamera.onSchemeChange();
					}
				}
			}
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x0600207B RID: 8315 RVA: 0x000964BC File Offset: 0x000946BC
	public static Ray currentRay
	{
		get
		{
			return (!(UICamera.currentCamera != null) || UICamera.currentTouch == null) ? default(Ray) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x0600207C RID: 8316 RVA: 0x0009650C File Offset: 0x0009470C
	public static bool inputHasFocus
	{
		get
		{
			if (UICamera.mInputFocus)
			{
				if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
				{
					return true;
				}
				UICamera.mInputFocus = false;
			}
			return false;
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x0600207D RID: 8317 RVA: 0x00096540 File Offset: 0x00094740
	// (set) Token: 0x0600207E RID: 8318 RVA: 0x00096548 File Offset: 0x00094748
	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x0600207F RID: 8319 RVA: 0x00096550 File Offset: 0x00094750
	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06002080 RID: 8320 RVA: 0x00096560 File Offset: 0x00094760
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06002081 RID: 8321 RVA: 0x00096588 File Offset: 0x00094788
	public static GameObject tooltipObject
	{
		get
		{
			return UICamera.mTooltip;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06002082 RID: 8322 RVA: 0x00096590 File Offset: 0x00094790
	public static bool isOverUI
	{
		get
		{
			if (UICamera.currentTouch != null)
			{
				return UICamera.currentTouch.isOverUI;
			}
			int i = 0;
			int count = UICamera.activeTouches.Count;
			while (i < count)
			{
				UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[i];
				if (mouseOrTouch.pressed != null && mouseOrTouch.pressed != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(mouseOrTouch.pressed) != null)
				{
					return true;
				}
				i++;
			}
			return (UICamera.mMouse[0].current != null && UICamera.mMouse[0].current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(UICamera.mMouse[0].current) != null) || (UICamera.controller.pressed != null && UICamera.controller.pressed != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(UICamera.controller.pressed) != null);
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06002083 RID: 8323 RVA: 0x000966B4 File Offset: 0x000948B4
	// (set) Token: 0x06002084 RID: 8324 RVA: 0x00096710 File Offset: 0x00094910
	public static GameObject hoveredObject
	{
		get
		{
			if (UICamera.currentTouch != null && UICamera.currentTouch.dragStarted)
			{
				return UICamera.currentTouch.current;
			}
			if (UICamera.mHover && UICamera.mHover.activeInHierarchy)
			{
				return UICamera.mHover;
			}
			UICamera.mHover = null;
			return null;
		}
		set
		{
			if (UICamera.mHover == value)
			{
				return;
			}
			bool flag = false;
			UICamera uicamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.ShowTooltip(null);
			if (UICamera.mSelected && UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
				UICamera.mSelected = null;
			}
			if (UICamera.mHover)
			{
				UICamera.Notify(UICamera.mHover, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, false);
				}
			}
			UICamera.mHover = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (UICamera.mHover)
			{
				if (UICamera.mHover != UICamera.controller.current && UICamera.mHover.GetComponent<UIKeyNavigation>() != null)
				{
					UICamera.controller.current = UICamera.mHover;
				}
				if (flag)
				{
					UICamera uicamera2 = (!(UICamera.mHover != null)) ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mHover.layer);
					if (uicamera2 != null)
					{
						UICamera.current = uicamera2;
						UICamera.currentCamera = uicamera2.cachedCamera;
					}
				}
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, true);
				}
				UICamera.Notify(UICamera.mHover, "OnHover", true);
			}
			if (flag)
			{
				UICamera.current = uicamera;
				UICamera.currentCamera = ((!(uicamera != null)) ? null : uicamera.cachedCamera);
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06002085 RID: 8325 RVA: 0x000968FC File Offset: 0x00094AFC
	// (set) Token: 0x06002086 RID: 8326 RVA: 0x00096A5C File Offset: 0x00094C5C
	public static GameObject controllerNavigationObject
	{
		get
		{
			if (UICamera.controller.current && UICamera.controller.current.activeInHierarchy)
			{
				return UICamera.controller.current;
			}
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.current != null && UICamera.current.useController && UIKeyNavigation.list.size > 0)
			{
				for (int i = 0; i < UIKeyNavigation.list.size; i++)
				{
					UIKeyNavigation uikeyNavigation = UIKeyNavigation.list[i];
					if (uikeyNavigation && uikeyNavigation.constraint != UIKeyNavigation.Constraint.Explicit && uikeyNavigation.startsSelected)
					{
						UICamera.hoveredObject = uikeyNavigation.gameObject;
						UICamera.controller.current = UICamera.mHover;
						return UICamera.mHover;
					}
				}
				if (UICamera.mHover == null)
				{
					for (int j = 0; j < UIKeyNavigation.list.size; j++)
					{
						UIKeyNavigation uikeyNavigation2 = UIKeyNavigation.list[j];
						if (uikeyNavigation2 && uikeyNavigation2.constraint != UIKeyNavigation.Constraint.Explicit)
						{
							UICamera.hoveredObject = uikeyNavigation2.gameObject;
							UICamera.controller.current = UICamera.mHover;
							return UICamera.mHover;
						}
					}
				}
			}
			UICamera.controller.current = null;
			return null;
		}
		set
		{
			if (UICamera.controller.current != value && UICamera.controller.current)
			{
				UICamera.Notify(UICamera.controller.current, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.controller.current, false);
				}
				UICamera.controller.current = null;
			}
			UICamera.hoveredObject = value;
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06002087 RID: 8327 RVA: 0x00096ADC File Offset: 0x00094CDC
	// (set) Token: 0x06002088 RID: 8328 RVA: 0x00096B0C File Offset: 0x00094D0C
	public static GameObject selectedObject
	{
		get
		{
			if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
			{
				return UICamera.mSelected;
			}
			UICamera.mSelected = null;
			return null;
		}
		set
		{
			if (UICamera.mSelected == value)
			{
				UICamera.hoveredObject = value;
				UICamera.controller.current = value;
				return;
			}
			UICamera.ShowTooltip(null);
			bool flag = false;
			UICamera uicamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.mInputFocus = false;
			if (UICamera.mSelected)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
			}
			UICamera.mSelected = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (value != null)
			{
				UIKeyNavigation component = value.GetComponent<UIKeyNavigation>();
				if (component != null)
				{
					UICamera.controller.current = value;
				}
			}
			if (UICamera.mSelected && flag)
			{
				UICamera uicamera2 = (!(UICamera.mSelected != null)) ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mSelected.layer);
				if (uicamera2 != null)
				{
					UICamera.current = uicamera2;
					UICamera.currentCamera = uicamera2.cachedCamera;
				}
			}
			if (UICamera.mSelected)
			{
				UICamera.mInputFocus = (UICamera.mSelected.activeInHierarchy && UICamera.mSelected.GetComponent<UIInput>() != null);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, true);
				}
				UICamera.Notify(UICamera.mSelected, "OnSelect", true);
			}
			if (flag)
			{
				UICamera.current = uicamera;
				UICamera.currentCamera = ((!(uicamera != null)) ? null : uicamera.cachedCamera);
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x00096CE4 File Offset: 0x00094EE4
	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		int j = 0;
		int count = UICamera.activeTouches.Count;
		while (j < count)
		{
			UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[j];
			if (mouseOrTouch.pressed == go)
			{
				return true;
			}
			j++;
		}
		return UICamera.controller.pressed == go;
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x0600208A RID: 8330 RVA: 0x00096D70 File Offset: 0x00094F70
	[Obsolete("Use either 'CountInputSources()' or 'activeTouches.Count'")]
	public static int touchCount
	{
		get
		{
			return UICamera.CountInputSources();
		}
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x00096D78 File Offset: 0x00094F78
	public static int CountInputSources()
	{
		int num = 0;
		int i = 0;
		int count = UICamera.activeTouches.Count;
		while (i < count)
		{
			UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[i];
			if (mouseOrTouch.pressed != null)
			{
				num++;
			}
			i++;
		}
		for (int j = 0; j < UICamera.mMouse.Length; j++)
		{
			if (UICamera.mMouse[j].pressed != null)
			{
				num++;
			}
		}
		if (UICamera.controller.pressed != null)
		{
			num++;
		}
		return num;
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x0600208C RID: 8332 RVA: 0x00096E18 File Offset: 0x00095018
	public static int dragCount
	{
		get
		{
			int num = 0;
			int i = 0;
			int count = UICamera.activeTouches.Count;
			while (i < count)
			{
				UICamera.MouseOrTouch mouseOrTouch = UICamera.activeTouches[i];
				if (mouseOrTouch.dragged != null)
				{
					num++;
				}
				i++;
			}
			for (int j = 0; j < UICamera.mMouse.Length; j++)
			{
				if (UICamera.mMouse[j].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x0600208D RID: 8333 RVA: 0x00096EB8 File Offset: 0x000950B8
	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			return (!(eventHandler != null)) ? null : eventHandler.cachedCamera;
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x0600208E RID: 8334 RVA: 0x00096EE4 File Offset: 0x000950E4
	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uicamera = UICamera.list.buffer[i];
				if (!(uicamera == null) && uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
				{
					return uicamera;
				}
			}
			return null;
		}
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x00096F48 File Offset: 0x00095148
	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x00096F90 File Offset: 0x00095190
	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody component = trans.GetComponent<Rigidbody>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x00096FE0 File Offset: 0x000951E0
	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x00097030 File Offset: 0x00095230
	public static void Raycast(UICamera.MouseOrTouch touch)
	{
		if (!UICamera.Raycast(touch.pos))
		{
			UICamera.mRayHitObject = UICamera.fallThrough;
		}
		if (UICamera.mRayHitObject == null)
		{
			UICamera.mRayHitObject = UICamera.mGenericHandler;
		}
		touch.last = touch.current;
		touch.current = UICamera.mRayHitObject;
		UICamera.mLastPos = touch.pos;
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x00097098 File Offset: 0x00095298
	public static bool Raycast(Vector3 inPos)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			if (uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
			{
				UICamera.currentCamera = uicamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y))
				{
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int layerMask = UICamera.currentCamera.cullingMask & uicamera.eventReceiverMask;
						float num = (uicamera.rangeDistance <= 0f) ? (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane) : uicamera.rangeDistance;
						if (uicamera.eventType == UICamera.EventType.World_3D)
						{
							if (Physics.Raycast(ray, out UICamera.lastHit, num, layerMask))
							{
								UICamera.lastWorldPosition = UICamera.lastHit.point;
								UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
								if (!UICamera.list[0].eventsGoToColliders)
								{
									Rigidbody rigidbody = UICamera.FindRootRigidbody(UICamera.mRayHitObject.transform);
									if (rigidbody != null)
									{
										UICamera.mRayHitObject = rigidbody.gameObject;
									}
								}
								return true;
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_3D)
						{
							RaycastHit[] array = Physics.RaycastAll(ray, num, layerMask);
							if (array.Length > 1)
							{
								int j = 0;
								while (j < array.Length)
								{
									GameObject gameObject = array[j].collider.gameObject;
									UIWidget component = gameObject.GetComponent<UIWidget>();
									if (component != null)
									{
										if (component.isVisible)
										{
											if (component.hitCheck == null || component.hitCheck(array[j].point))
											{
												goto IL_260;
											}
										}
									}
									else
									{
										UIRect uirect = NGUITools.FindInParents<UIRect>(gameObject);
										if (!(uirect != null) || uirect.finalAlpha >= 0.001f)
										{
											goto IL_260;
										}
									}
									IL_2E1:
									j++;
									continue;
									IL_260:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.hit = array[j];
										UICamera.mHit.point = array[j].point;
										UICamera.mHit.go = array[j].collider.gameObject;
										UICamera.mHits.Add(UICamera.mHit);
										goto IL_2E1;
									}
									goto IL_2E1;
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										UICamera.lastHit = UICamera.mHits[k].hit;
										UICamera.mRayHitObject = UICamera.mHits[k].go;
										UICamera.lastWorldPosition = UICamera.mHits[k].point;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (array.Length == 1)
							{
								GameObject gameObject2 = array[0].collider.gameObject;
								UIWidget component2 = gameObject2.GetComponent<UIWidget>();
								if (component2 != null)
								{
									if (!component2.isVisible)
									{
										goto IL_7E2;
									}
									if (component2.hitCheck != null && !component2.hitCheck(array[0].point))
									{
										goto IL_7E2;
									}
								}
								else
								{
									UIRect uirect2 = NGUITools.FindInParents<UIRect>(gameObject2);
									if (uirect2 != null && uirect2.finalAlpha < 0.001f)
									{
										goto IL_7E2;
									}
								}
								if (UICamera.IsVisible(array[0].point, array[0].collider.gameObject))
								{
									UICamera.lastHit = array[0];
									UICamera.lastWorldPosition = array[0].point;
									UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.World_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out num))
							{
								Vector3 point = ray.GetPoint(num);
								Collider2D collider2D = Physics2D.OverlapPoint(point, layerMask);
								if (collider2D)
								{
									UICamera.lastWorldPosition = point;
									UICamera.mRayHitObject = collider2D.gameObject;
									if (!uicamera.eventsGoToColliders)
									{
										Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.mRayHitObject.transform);
										if (rigidbody2D != null)
										{
											UICamera.mRayHitObject = rigidbody2D.gameObject;
										}
									}
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out num))
							{
								UICamera.lastWorldPosition = ray.GetPoint(num);
								Collider2D[] array2 = Physics2D.OverlapPointAll(UICamera.lastWorldPosition, layerMask);
								if (array2.Length > 1)
								{
									int l = 0;
									while (l < array2.Length)
									{
										GameObject gameObject3 = array2[l].gameObject;
										UIWidget component3 = gameObject3.GetComponent<UIWidget>();
										if (component3 != null)
										{
											if (component3.isVisible)
											{
												if (component3.hitCheck == null || component3.hitCheck(UICamera.lastWorldPosition))
												{
													goto IL_639;
												}
											}
										}
										else
										{
											UIRect uirect3 = NGUITools.FindInParents<UIRect>(gameObject3);
											if (!(uirect3 != null) || uirect3.finalAlpha >= 0.001f)
											{
												goto IL_639;
											}
										}
										IL_688:
										l++;
										continue;
										IL_639:
										UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject3);
										if (UICamera.mHit.depth != 2147483647)
										{
											UICamera.mHit.go = gameObject3;
											UICamera.mHit.point = UICamera.lastWorldPosition;
											UICamera.mHits.Add(UICamera.mHit);
											goto IL_688;
										}
										goto IL_688;
									}
									UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
									for (int m = 0; m < UICamera.mHits.size; m++)
									{
										if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
										{
											UICamera.mRayHitObject = UICamera.mHits[m].go;
											UICamera.mHits.Clear();
											return true;
										}
									}
									UICamera.mHits.Clear();
								}
								else if (array2.Length == 1)
								{
									GameObject gameObject4 = array2[0].gameObject;
									UIWidget component4 = gameObject4.GetComponent<UIWidget>();
									if (component4 != null)
									{
										if (!component4.isVisible)
										{
											goto IL_7E2;
										}
										if (component4.hitCheck != null && !component4.hitCheck(UICamera.lastWorldPosition))
										{
											goto IL_7E2;
										}
									}
									else
									{
										UIRect uirect4 = NGUITools.FindInParents<UIRect>(gameObject4);
										if (uirect4 != null && uirect4.finalAlpha < 0.001f)
										{
											goto IL_7E2;
										}
									}
									if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject4))
									{
										UICamera.mRayHitObject = gameObject4;
										return true;
									}
								}
							}
						}
					}
				}
			}
			IL_7E2:;
		}
		return false;
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x0009789C File Offset: 0x00095A9C
	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(worldPoint))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000978D8 File Offset: 0x00095AD8
	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(de.go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(de.point))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x00097920 File Offset: 0x00095B20
	public static bool IsHighlighted(GameObject go)
	{
		return UICamera.hoveredObject == go;
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x00097930 File Offset: 0x00095B30
	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			Camera cachedCamera = uicamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uicamera;
			}
		}
		return null;
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x00097990 File Offset: 0x00095B90
	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			UICamera.currentKey = up;
			return 1;
		}
		if (UICamera.GetKeyDown(down))
		{
			UICamera.currentKey = down;
			return -1;
		}
		return 0;
	}

	// Token: 0x06002099 RID: 8345 RVA: 0x000979D0 File Offset: 0x00095BD0
	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0))
		{
			UICamera.currentKey = up0;
			return 1;
		}
		if (UICamera.GetKeyDown(up1))
		{
			UICamera.currentKey = up1;
			return 1;
		}
		if (UICamera.GetKeyDown(down0))
		{
			UICamera.currentKey = down0;
			return -1;
		}
		if (UICamera.GetKeyDown(down1))
		{
			UICamera.currentKey = down1;
			return -1;
		}
		return 0;
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x00097A40 File Offset: 0x00095C40
	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time && !string.IsNullOrEmpty(axis))
		{
			float num = UICamera.GetAxis(axis);
			if (num > 0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (num < -0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x00097ABC File Offset: 0x00095CBC
	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying > 10)
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UIPopupList.isOpen && UIPopupList.current.source == go && UIPopupList.isOpen)
		{
			go = UIPopupList.current.gameObject;
		}
		if (go && go.activeInHierarchy)
		{
			UICamera.mNotifying++;
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
			UICamera.mNotifying--;
		}
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x00097B7C File Offset: 0x00095D7C
	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x00097B88 File Offset: 0x00095D88
	public static UICamera.MouseOrTouch GetTouch(int id, bool createIfMissing = false)
	{
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		int i = 0;
		int count = UICamera.mTouchIDs.Count;
		while (i < count)
		{
			if (UICamera.mTouchIDs[i] == id)
			{
				return UICamera.activeTouches[i];
			}
			i++;
		}
		if (createIfMissing)
		{
			UICamera.MouseOrTouch mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.pressTime = RealTime.time;
			mouseOrTouch.touchBegan = true;
			UICamera.activeTouches.Add(mouseOrTouch);
			UICamera.mTouchIDs.Add(id);
			return mouseOrTouch;
		}
		return null;
	}

	// Token: 0x0600209E RID: 8350 RVA: 0x00097C18 File Offset: 0x00095E18
	public static void RemoveTouch(int id)
	{
		int i = 0;
		int count = UICamera.mTouchIDs.Count;
		while (i < count)
		{
			if (UICamera.mTouchIDs[i] == id)
			{
				UICamera.mTouchIDs.RemoveAt(i);
				UICamera.activeTouches.RemoveAt(i);
				return;
			}
			i++;
		}
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x00097C6C File Offset: 0x00095E6C
	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		UICamera.mMouse[0].pos = Input.mousePosition;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.mLastPos = UICamera.mMouse[0].pos;
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x00097CFC File Offset: 0x00095EFC
	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x00097D20 File Offset: 0x00095F20
	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x00097D30 File Offset: 0x00095F30
	private void Start()
	{
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic)
		{
			this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
		}
		if (Application.isPlaying)
		{
			if (UICamera.fallThrough == null)
			{
				UIRoot uiroot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				if (uiroot != null)
				{
					UICamera.fallThrough = uiroot.gameObject;
				}
				else
				{
					Transform transform = base.transform;
					UICamera.fallThrough = ((!(transform.parent != null)) ? base.gameObject : transform.parent.gameObject);
				}
			}
			this.cachedCamera.eventMask = 0;
		}
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x00097DE8 File Offset: 0x00095FE8
	private void Update()
	{
		this.allowMultiTouch = (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive && (!(Pauser.sharedPauser != null) || !Pauser.sharedPauser.paused) && !(ChatViewrController.sharedController != null) && !WeaponManager.sharedManager.myPlayerMoveC.showRanks);
		if (!this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		NGUIDebug.debugRaycast = this.debug;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if ((this.useKeyboard || this.useController) && !UICamera.disableController)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float num = string.IsNullOrEmpty(this.scrollAxisName) ? 0f : UICamera.GetAxis(this.scrollAxisName);
			if (num != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, num);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", num);
			}
			if (UICamera.showTooltips && UICamera.mTooltipTime != 0f && !UIPopupList.isOpen && UICamera.mMouse[0].dragged == null && (UICamera.mTooltipTime < RealTime.time || UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift)))
			{
				UICamera.currentTouch = UICamera.mMouse[0];
				UICamera.currentTouchID = -1;
				UICamera.ShowTooltip(UICamera.mHover);
			}
		}
		if (UICamera.mTooltip != null && !NGUITools.GetActive(UICamera.mTooltip))
		{
			UICamera.ShowTooltip(null);
		}
		UICamera.current = null;
		UICamera.currentTouchID = -100;
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x0009804C File Offset: 0x0009624C
	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		if (this.calculateWindowSize)
		{
			int width = Screen.width;
			int height = Screen.height;
			if (width != UICamera.mWidth || height != UICamera.mHeight)
			{
				UICamera.mWidth = width;
				UICamera.mHeight = height;
				UIRoot.Broadcast("UpdateAnchors");
				if (UICamera.onScreenResize != null)
				{
					UICamera.onScreenResize();
				}
			}
		}
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x000980BC File Offset: 0x000962BC
	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			this.calculateWindowSize = false;
		}
		else
		{
			base.StartCoroutine("ReturnAccesToScreenSize");
		}
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x000980DC File Offset: 0x000962DC
	private IEnumerator ReturnAccesToScreenSize()
	{
		yield return null;
		yield return null;
		yield return null;
		this.calculateWindowSize = true;
		yield break;
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x000980F8 File Offset: 0x000962F8
	public void ProcessMouse()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				UICamera.currentKey = KeyCode.Mouse0 + i;
				flag2 = true;
				flag = true;
			}
			else if (Input.GetMouseButton(i))
			{
				UICamera.currentKey = KeyCode.Mouse0 + i;
				flag = true;
			}
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Touch)
		{
			return;
		}
		UICamera.currentTouch = UICamera.mMouse[0];
		Vector2 vector = Input.mousePosition;
		if (UICamera.currentTouch.ignoreDelta == 0)
		{
			UICamera.currentTouch.delta = vector - UICamera.currentTouch.pos;
		}
		else
		{
			UICamera.currentTouch.ignoreDelta--;
			UICamera.currentTouch.delta.x = 0f;
			UICamera.currentTouch.delta.y = 0f;
		}
		float sqrMagnitude = UICamera.currentTouch.delta.sqrMagnitude;
		UICamera.currentTouch.pos = vector;
		UICamera.mLastPos = vector;
		bool flag3 = false;
		if (UICamera.currentScheme != UICamera.ControlScheme.Mouse)
		{
			if (sqrMagnitude < 0.001f)
			{
				return;
			}
			UICamera.currentKey = KeyCode.Mouse0;
			flag3 = true;
		}
		else if (sqrMagnitude > 0.001f)
		{
			flag3 = true;
		}
		for (int j = 1; j < 3; j++)
		{
			UICamera.mMouse[j].pos = UICamera.currentTouch.pos;
			UICamera.mMouse[j].delta = UICamera.currentTouch.delta;
		}
		if (flag || flag3 || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			UICamera.Raycast(UICamera.currentTouch);
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.currentTouch.current;
			}
		}
		bool flag4 = UICamera.currentTouch.last != UICamera.currentTouch.current;
		bool flag5 = UICamera.currentTouch.pressed != null;
		if (!flag5)
		{
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouchID = -1;
		if (flag4)
		{
			UICamera.currentKey = KeyCode.Mouse0;
		}
		if (!flag && flag3 && (!this.stickyTooltip || flag4))
		{
			if (UICamera.mTooltipTime != 0f)
			{
				UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
			}
			else if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
		}
		if (flag3 && UICamera.onMouseMove != null)
		{
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if (flag4 && (flag2 || (flag5 && !flag)))
		{
			UICamera.hoveredObject = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown || mouseButtonUp)
			{
				UICamera.currentKey = KeyCode.Mouse0 + l;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = KeyCode.Mouse0 + l;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
		}
		if (!flag && flag4)
		{
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.currentTouchID = -1;
			UICamera.currentKey = KeyCode.Mouse0;
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x0009853C File Offset: 0x0009673C
	public void ProcessTouches()
	{
		int num = (UICamera.GetInputTouchCount != null) ? UICamera.GetInputTouchCount() : Input.touchCount;
		for (int i = 0; i < num; i++)
		{
			float pressure = 0f;
			float maxPressure = 1f;
			TouchPhase phase;
			int fingerId;
			Vector2 position;
			int tapCount;
			if (UICamera.GetInputTouch == null)
			{
				UnityEngine.Touch touch = Input.GetTouch(i);
				phase = touch.phase;
				fingerId = touch.fingerId;
				position = touch.position;
				tapCount = touch.tapCount;
				pressure = touch.pressure;
				maxPressure = touch.maximumPossiblePressure;
			}
			else
			{
				UICamera.Touch touch2 = UICamera.GetInputTouch(i);
				phase = touch2.phase;
				fingerId = touch2.fingerId;
				position = touch2.position;
				tapCount = touch2.tapCount;
			}
			UICamera.currentTouchID = ((!this.allowMultiTouch) ? 1 : fingerId);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID, true);
			bool flag = phase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = phase == TouchPhase.Canceled || phase == TouchPhase.Ended;
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.delta = position - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = position;
			UICamera.currentKey = KeyCode.None;
			UICamera.currentTouch.pressure = pressure;
			UICamera.currentTouch.maxPressure = maxPressure;
			UICamera.Raycast(UICamera.currentTouch);
			if (Defs.touchPressureSupported && (Defs.isUseShoot3DTouch || Defs.isUseJump3DTouch) && UICamera.currentTouch.current != null)
			{
				UICamera.Notify(UICamera.currentTouch.current, "OnPressure", (!flag2) ? (UICamera.currentTouch.pressure / UICamera.currentTouch.maxPressure) : 0f);
			}
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (num == 0)
		{
			if (UICamera.mUsingTouchEvents)
			{
				UICamera.mUsingTouchEvents = false;
				return;
			}
			if (this.useMouse)
			{
				this.ProcessMouse();
			}
		}
		else
		{
			UICamera.mUsingTouchEvents = true;
		}
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000987E0 File Offset: 0x000969E0
	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
				UICamera.activeTouches.Add(UICamera.currentTouch);
			}
			Vector2 vector = Input.mousePosition;
			UICamera.currentTouch.delta = vector - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = vector;
			UICamera.Raycast(UICamera.currentTouch);
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			UICamera.currentKey = KeyCode.None;
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.activeTouches.Remove(UICamera.currentTouch);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x00098904 File Offset: 0x00096B04
	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyDown(KeyCode.KeypadEnter))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyUp(KeyCode.KeypadEnter))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (flag)
		{
			UICamera.currentTouch.pressTime = RealTime.time;
		}
		if ((flag || flag2) && UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.last = UICamera.currentTouch.current;
		}
		KeyCode keyCode = KeyCode.None;
		if (this.useController)
		{
			if (!UICamera.disableController && UICamera.currentScheme == UICamera.ControlScheme.Controller && (UICamera.currentTouch.current == null || !UICamera.currentTouch.current.activeInHierarchy))
			{
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			}
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				int direction = UICamera.GetDirection(this.verticalAxisName);
				if (direction != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = ((direction <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				int direction2 = UICamera.GetDirection(this.horizontalAxisName);
				if (direction2 != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = ((direction2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			float num = string.IsNullOrEmpty(this.horizontalPanAxisName) ? 0f : UICamera.GetAxis(this.horizontalPanAxisName);
			float num2 = string.IsNullOrEmpty(this.verticalPanAxisName) ? 0f : UICamera.GetAxis(this.verticalPanAxisName);
			if (num != 0f || num2 != 0f)
			{
				UICamera.ShowTooltip(null);
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
				if (UICamera.currentTouch.current != null)
				{
					Vector2 vector = new Vector2(num, num2);
					vector *= Time.unscaledDeltaTime;
					if (UICamera.onPan != null)
					{
						UICamera.onPan(UICamera.currentTouch.current, vector);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnPan", vector);
				}
			}
		}
		if ((UICamera.GetAnyKeyDown == null) ? Input.anyKeyDown : UICamera.GetAnyKeyDown())
		{
			int i = 0;
			int num3 = NGUITools.keys.Length;
			while (i < num3)
			{
				KeyCode keyCode2 = NGUITools.keys[i];
				if (keyCode != keyCode2)
				{
					if (UICamera.GetKeyDown(keyCode2))
					{
						if (this.useKeyboard || keyCode2 >= KeyCode.Mouse0)
						{
							if (this.useController || keyCode2 < KeyCode.JoystickButton0)
							{
								if (this.useMouse || (keyCode2 < KeyCode.Mouse0 && keyCode2 > KeyCode.Mouse6))
								{
									UICamera.currentKey = keyCode2;
									if (UICamera.onKey != null)
									{
										UICamera.onKey(UICamera.currentTouch.current, keyCode2);
									}
									UICamera.Notify(UICamera.currentTouch.current, "OnKey", keyCode2);
								}
							}
						}
					}
				}
				i++;
			}
		}
		UICamera.currentTouch = null;
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x00098E6C File Offset: 0x0009706C
	private void ProcessPress(bool pressed, float click, float drag)
	{
		if (pressed)
		{
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == null && UICamera.currentTouch.current != null)
			{
				UICamera.hoveredObject = UICamera.currentTouch.current;
			}
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			if (UICamera.mSelected != UICamera.currentTouch.pressed)
			{
				UICamera.mInputFocus = false;
				if (UICamera.mSelected)
				{
					UICamera.Notify(UICamera.mSelected, "OnSelect", false);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, false);
					}
				}
				UICamera.mSelected = UICamera.currentTouch.pressed;
				if (UICamera.currentTouch.pressed != null)
				{
					UIKeyNavigation component = UICamera.currentTouch.pressed.GetComponent<UIKeyNavigation>();
					if (component != null)
					{
						UICamera.controller.current = UICamera.currentTouch.pressed;
					}
				}
				if (UICamera.mSelected)
				{
					UICamera.mInputFocus = (UICamera.mSelected.activeInHierarchy && UICamera.mSelected.GetComponent<UIInput>() != null);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, true);
					}
					UICamera.Notify(UICamera.mSelected, "OnSelect", true);
				}
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.sqrMagnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float sqrMagnitude = UICamera.currentTouch.totalDelta.sqrMagnitude;
			bool flag = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && drag < sqrMagnitude)
			{
				flag = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.mTooltip != null)
				{
					UICamera.ShowTooltip(null);
				}
				UICamera.isDragging = true;
				bool flag2 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragOut != null)
					{
						UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag2)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
				else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && click < sqrMagnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x00099488 File Offset: 0x00097688
	private void ProcessRelease(bool isMouse, float drag)
	{
		if (UICamera.currentTouch == null)
		{
			return;
		}
		UICamera.currentTouch.pressStarted = false;
		if (UICamera.currentTouch.pressed != null)
		{
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDragOut != null)
				{
					UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
				if (UICamera.onDragEnd != null)
				{
					UICamera.onDragEnd(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
			}
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (isMouse && this.HasCollider(UICamera.currentTouch.pressed))
			{
				if (UICamera.mHover == UICamera.currentTouch.current)
				{
					if (UICamera.onHover != null)
					{
						UICamera.onHover(UICamera.currentTouch.current, true);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
				else
				{
					UICamera.hoveredObject = UICamera.currentTouch.current;
				}
			}
			if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.sqrMagnitude < drag))
			{
				if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
				{
					UICamera.ShowTooltip(null);
					float time = RealTime.time;
					if (UICamera.onClick != null)
					{
						UICamera.onClick(UICamera.currentTouch.pressed);
					}
					UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
					if (UICamera.currentTouch.clickTime + 0.35f > time)
					{
						if (UICamera.onDoubleClick != null)
						{
							UICamera.onDoubleClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
					}
					UICamera.currentTouch.clickTime = time;
				}
			}
			else if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDrop != null)
				{
					UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
			}
		}
		UICamera.currentTouch.dragStarted = false;
		UICamera.currentTouch.pressed = null;
		UICamera.currentTouch.dragged = null;
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x0009978C File Offset: 0x0009798C
	private bool HasCollider(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		Collider component = go.GetComponent<Collider>();
		if (component != null)
		{
			return component.enabled;
		}
		Collider2D component2 = go.GetComponent<Collider2D>();
		return component2 != null && component2.enabled;
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x000997E0 File Offset: 0x000979E0
	public void ProcessTouch(bool pressed, bool released)
	{
		if (pressed)
		{
			UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
		}
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = (!flag) ? this.touchDragThreshold : this.mouseDragThreshold;
		float num2 = (!flag) ? this.touchClickThreshold : this.mouseClickThreshold;
		num *= num;
		num2 *= num2;
		if (UICamera.currentTouch.pressed != null)
		{
			if (released)
			{
				this.ProcessRelease(flag, num);
			}
			this.ProcessPress(pressed, num2, num);
			if (UICamera.currentTouch.pressed == UICamera.currentTouch.current && UICamera.mTooltipTime != 0f && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && !UICamera.currentTouch.dragStarted && UICamera.currentTouch.deltaTime > this.tooltipDelay)
			{
				UICamera.mTooltipTime = 0f;
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				if (this.longPressTooltip)
				{
					UICamera.ShowTooltip(UICamera.currentTouch.pressed);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnLongPress", null);
			}
		}
		else if (flag || pressed || released)
		{
			this.ProcessPress(pressed, num2, num);
			if (released)
			{
				this.ProcessRelease(flag, num);
			}
		}
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x00099944 File Offset: 0x00097B44
	public static bool ShowTooltip(GameObject go)
	{
		if (UICamera.mTooltip != go)
		{
			if (UICamera.mTooltip != null)
			{
				if (UICamera.onTooltip != null)
				{
					UICamera.onTooltip(UICamera.mTooltip, false);
				}
				UICamera.Notify(UICamera.mTooltip, "OnTooltip", false);
			}
			UICamera.mTooltip = go;
			UICamera.mTooltipTime = 0f;
			if (UICamera.mTooltip != null)
			{
				if (UICamera.onTooltip != null)
				{
					UICamera.onTooltip(UICamera.mTooltip, true);
				}
				UICamera.Notify(UICamera.mTooltip, "OnTooltip", true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x000999F4 File Offset: 0x00097BF4
	public static bool HideTooltip()
	{
		return UICamera.ShowTooltip(null);
	}

	// Token: 0x040014A3 RID: 5283
	private bool calculateWindowSize = true;

	// Token: 0x040014A4 RID: 5284
	public static BetterList<UICamera> list = new BetterList<UICamera>();

	// Token: 0x040014A5 RID: 5285
	public static UICamera.GetKeyStateFunc GetKeyDown = new UICamera.GetKeyStateFunc(Input.GetKeyDown);

	// Token: 0x040014A6 RID: 5286
	public static UICamera.GetKeyStateFunc GetKeyUp = new UICamera.GetKeyStateFunc(Input.GetKeyUp);

	// Token: 0x040014A7 RID: 5287
	public static UICamera.GetKeyStateFunc GetKey = new UICamera.GetKeyStateFunc(Input.GetKey);

	// Token: 0x040014A8 RID: 5288
	public static UICamera.GetAxisFunc GetAxis = new UICamera.GetAxisFunc(Input.GetAxis);

	// Token: 0x040014A9 RID: 5289
	public static UICamera.GetAnyKeyFunc GetAnyKeyDown;

	// Token: 0x040014AA RID: 5290
	public static UICamera.OnScreenResize onScreenResize;

	// Token: 0x040014AB RID: 5291
	public UICamera.EventType eventType = UICamera.EventType.UI_3D;

	// Token: 0x040014AC RID: 5292
	public bool eventsGoToColliders;

	// Token: 0x040014AD RID: 5293
	public LayerMask eventReceiverMask = -1;

	// Token: 0x040014AE RID: 5294
	public bool debug;

	// Token: 0x040014AF RID: 5295
	public bool useMouse = true;

	// Token: 0x040014B0 RID: 5296
	public bool useTouch = true;

	// Token: 0x040014B1 RID: 5297
	public bool allowMultiTouch = true;

	// Token: 0x040014B2 RID: 5298
	public bool useKeyboard = true;

	// Token: 0x040014B3 RID: 5299
	public bool useController = true;

	// Token: 0x040014B4 RID: 5300
	public bool stickyTooltip = true;

	// Token: 0x040014B5 RID: 5301
	public float tooltipDelay = 1f;

	// Token: 0x040014B6 RID: 5302
	public bool longPressTooltip;

	// Token: 0x040014B7 RID: 5303
	public float mouseDragThreshold = 4f;

	// Token: 0x040014B8 RID: 5304
	public float mouseClickThreshold = 10f;

	// Token: 0x040014B9 RID: 5305
	public float touchDragThreshold = 40f;

	// Token: 0x040014BA RID: 5306
	public float touchClickThreshold = 40f;

	// Token: 0x040014BB RID: 5307
	public float rangeDistance = -1f;

	// Token: 0x040014BC RID: 5308
	public string horizontalAxisName = "Horizontal";

	// Token: 0x040014BD RID: 5309
	public string verticalAxisName = "Vertical";

	// Token: 0x040014BE RID: 5310
	public string horizontalPanAxisName;

	// Token: 0x040014BF RID: 5311
	public string verticalPanAxisName;

	// Token: 0x040014C0 RID: 5312
	public string scrollAxisName = "Mouse ScrollWheel";

	// Token: 0x040014C1 RID: 5313
	public bool commandClick = true;

	// Token: 0x040014C2 RID: 5314
	public KeyCode submitKey0 = KeyCode.Return;

	// Token: 0x040014C3 RID: 5315
	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	// Token: 0x040014C4 RID: 5316
	public KeyCode cancelKey0 = KeyCode.Escape;

	// Token: 0x040014C5 RID: 5317
	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	// Token: 0x040014C6 RID: 5318
	public bool autoHideCursor = true;

	// Token: 0x040014C7 RID: 5319
	public static UICamera.OnCustomInput onCustomInput;

	// Token: 0x040014C8 RID: 5320
	public static bool showTooltips = true;

	// Token: 0x040014C9 RID: 5321
	private static bool mDisableController = false;

	// Token: 0x040014CA RID: 5322
	private static Vector2 mLastPos = Vector2.zero;

	// Token: 0x040014CB RID: 5323
	public static Vector3 lastWorldPosition = Vector3.zero;

	// Token: 0x040014CC RID: 5324
	public static RaycastHit lastHit;

	// Token: 0x040014CD RID: 5325
	public static UICamera current = null;

	// Token: 0x040014CE RID: 5326
	public static Camera currentCamera = null;

	// Token: 0x040014CF RID: 5327
	public static UICamera.OnSchemeChange onSchemeChange;

	// Token: 0x040014D0 RID: 5328
	private static UICamera.ControlScheme mLastScheme = UICamera.ControlScheme.Mouse;

	// Token: 0x040014D1 RID: 5329
	public static int currentTouchID = -100;

	// Token: 0x040014D2 RID: 5330
	private static KeyCode mCurrentKey = KeyCode.Alpha0;

	// Token: 0x040014D3 RID: 5331
	public static UICamera.MouseOrTouch currentTouch = null;

	// Token: 0x040014D4 RID: 5332
	private static bool mInputFocus = false;

	// Token: 0x040014D5 RID: 5333
	private static GameObject mGenericHandler;

	// Token: 0x040014D6 RID: 5334
	public static GameObject fallThrough;

	// Token: 0x040014D7 RID: 5335
	public static UICamera.VoidDelegate onClick;

	// Token: 0x040014D8 RID: 5336
	public static UICamera.VoidDelegate onDoubleClick;

	// Token: 0x040014D9 RID: 5337
	public static UICamera.BoolDelegate onHover;

	// Token: 0x040014DA RID: 5338
	public static UICamera.BoolDelegate onPress;

	// Token: 0x040014DB RID: 5339
	public static UICamera.BoolDelegate onSelect;

	// Token: 0x040014DC RID: 5340
	public static UICamera.FloatDelegate onScroll;

	// Token: 0x040014DD RID: 5341
	public static UICamera.VectorDelegate onDrag;

	// Token: 0x040014DE RID: 5342
	public static UICamera.VoidDelegate onDragStart;

	// Token: 0x040014DF RID: 5343
	public static UICamera.ObjectDelegate onDragOver;

	// Token: 0x040014E0 RID: 5344
	public static UICamera.ObjectDelegate onDragOut;

	// Token: 0x040014E1 RID: 5345
	public static UICamera.VoidDelegate onDragEnd;

	// Token: 0x040014E2 RID: 5346
	public static UICamera.ObjectDelegate onDrop;

	// Token: 0x040014E3 RID: 5347
	public static UICamera.KeyCodeDelegate onKey;

	// Token: 0x040014E4 RID: 5348
	public static UICamera.KeyCodeDelegate onNavigate;

	// Token: 0x040014E5 RID: 5349
	public static UICamera.VectorDelegate onPan;

	// Token: 0x040014E6 RID: 5350
	public static UICamera.BoolDelegate onTooltip;

	// Token: 0x040014E7 RID: 5351
	public static UICamera.MoveDelegate onMouseMove;

	// Token: 0x040014E8 RID: 5352
	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	// Token: 0x040014E9 RID: 5353
	public static UICamera.MouseOrTouch controller = new UICamera.MouseOrTouch();

	// Token: 0x040014EA RID: 5354
	public static List<UICamera.MouseOrTouch> activeTouches = new List<UICamera.MouseOrTouch>();

	// Token: 0x040014EB RID: 5355
	private static List<int> mTouchIDs = new List<int>();

	// Token: 0x040014EC RID: 5356
	private static int mWidth = 0;

	// Token: 0x040014ED RID: 5357
	private static int mHeight = 0;

	// Token: 0x040014EE RID: 5358
	private static GameObject mTooltip = null;

	// Token: 0x040014EF RID: 5359
	private Camera mCam;

	// Token: 0x040014F0 RID: 5360
	private static float mTooltipTime = 0f;

	// Token: 0x040014F1 RID: 5361
	private float mNextRaycast;

	// Token: 0x040014F2 RID: 5362
	public static bool isDragging = false;

	// Token: 0x040014F3 RID: 5363
	private static GameObject mRayHitObject;

	// Token: 0x040014F4 RID: 5364
	private static GameObject mHover;

	// Token: 0x040014F5 RID: 5365
	private static GameObject mSelected;

	// Token: 0x040014F6 RID: 5366
	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	// Token: 0x040014F7 RID: 5367
	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	// Token: 0x040014F8 RID: 5368
	private static Plane m2DPlane = new Plane(Vector3.back, 0f);

	// Token: 0x040014F9 RID: 5369
	private static float mNextEvent = 0f;

	// Token: 0x040014FA RID: 5370
	private static int mNotifying = 0;

	// Token: 0x040014FB RID: 5371
	private static bool mUsingTouchEvents = true;

	// Token: 0x040014FC RID: 5372
	public static UICamera.GetTouchCountCallback GetInputTouchCount;

	// Token: 0x040014FD RID: 5373
	public static UICamera.GetTouchCallback GetInputTouch;

	// Token: 0x0200039A RID: 922
	public enum ControlScheme
	{
		// Token: 0x04001501 RID: 5377
		Mouse,
		// Token: 0x04001502 RID: 5378
		Touch,
		// Token: 0x04001503 RID: 5379
		Controller
	}

	// Token: 0x0200039B RID: 923
	public enum ClickNotification
	{
		// Token: 0x04001505 RID: 5381
		None,
		// Token: 0x04001506 RID: 5382
		Always,
		// Token: 0x04001507 RID: 5383
		BasedOnDelta
	}

	// Token: 0x0200039C RID: 924
	public class MouseOrTouch
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x00099A44 File Offset: 0x00097C44
		public float deltaTime
		{
			get
			{
				return RealTime.time - this.pressTime;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060020B5 RID: 8373 RVA: 0x00099A54 File Offset: 0x00097C54
		public bool isOverUI
		{
			get
			{
				return this.current != null && this.current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(this.current) != null;
			}
		}

		// Token: 0x04001508 RID: 5384
		public KeyCode key;

		// Token: 0x04001509 RID: 5385
		public Vector2 pos;

		// Token: 0x0400150A RID: 5386
		public Vector2 lastPos;

		// Token: 0x0400150B RID: 5387
		public Vector2 delta;

		// Token: 0x0400150C RID: 5388
		public Vector2 totalDelta;

		// Token: 0x0400150D RID: 5389
		public Camera pressedCam;

		// Token: 0x0400150E RID: 5390
		public GameObject last;

		// Token: 0x0400150F RID: 5391
		public GameObject current;

		// Token: 0x04001510 RID: 5392
		public GameObject pressed;

		// Token: 0x04001511 RID: 5393
		public GameObject dragged;

		// Token: 0x04001512 RID: 5394
		public float pressTime;

		// Token: 0x04001513 RID: 5395
		public float clickTime;

		// Token: 0x04001514 RID: 5396
		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		// Token: 0x04001515 RID: 5397
		public bool touchBegan = true;

		// Token: 0x04001516 RID: 5398
		public bool pressStarted;

		// Token: 0x04001517 RID: 5399
		public bool dragStarted;

		// Token: 0x04001518 RID: 5400
		public int ignoreDelta;

		// Token: 0x04001519 RID: 5401
		public float pressure;

		// Token: 0x0400151A RID: 5402
		public float maxPressure;
	}

	// Token: 0x0200039D RID: 925
	public enum EventType
	{
		// Token: 0x0400151C RID: 5404
		World_3D,
		// Token: 0x0400151D RID: 5405
		UI_3D,
		// Token: 0x0400151E RID: 5406
		World_2D,
		// Token: 0x0400151F RID: 5407
		UI_2D
	}

	// Token: 0x0200039E RID: 926
	private struct DepthEntry
	{
		// Token: 0x04001520 RID: 5408
		public int depth;

		// Token: 0x04001521 RID: 5409
		public RaycastHit hit;

		// Token: 0x04001522 RID: 5410
		public Vector3 point;

		// Token: 0x04001523 RID: 5411
		public GameObject go;
	}

	// Token: 0x0200039F RID: 927
	public class Touch
	{
		// Token: 0x04001524 RID: 5412
		public int fingerId;

		// Token: 0x04001525 RID: 5413
		public TouchPhase phase;

		// Token: 0x04001526 RID: 5414
		public Vector2 position;

		// Token: 0x04001527 RID: 5415
		public int tapCount;
	}

	// Token: 0x02000904 RID: 2308
	// (Invoke) Token: 0x060050B0 RID: 20656
	public delegate bool GetKeyStateFunc(KeyCode key);

	// Token: 0x02000905 RID: 2309
	// (Invoke) Token: 0x060050B4 RID: 20660
	public delegate float GetAxisFunc(string name);

	// Token: 0x02000906 RID: 2310
	// (Invoke) Token: 0x060050B8 RID: 20664
	public delegate bool GetAnyKeyFunc();

	// Token: 0x02000907 RID: 2311
	// (Invoke) Token: 0x060050BC RID: 20668
	public delegate void OnScreenResize();

	// Token: 0x02000908 RID: 2312
	// (Invoke) Token: 0x060050C0 RID: 20672
	public delegate void OnCustomInput();

	// Token: 0x02000909 RID: 2313
	// (Invoke) Token: 0x060050C4 RID: 20676
	public delegate void OnSchemeChange();

	// Token: 0x0200090A RID: 2314
	// (Invoke) Token: 0x060050C8 RID: 20680
	public delegate void MoveDelegate(Vector2 delta);

	// Token: 0x0200090B RID: 2315
	// (Invoke) Token: 0x060050CC RID: 20684
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x0200090C RID: 2316
	// (Invoke) Token: 0x060050D0 RID: 20688
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x0200090D RID: 2317
	// (Invoke) Token: 0x060050D4 RID: 20692
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x0200090E RID: 2318
	// (Invoke) Token: 0x060050D8 RID: 20696
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x0200090F RID: 2319
	// (Invoke) Token: 0x060050DC RID: 20700
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x02000910 RID: 2320
	// (Invoke) Token: 0x060050E0 RID: 20704
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	// Token: 0x02000911 RID: 2321
	// (Invoke) Token: 0x060050E4 RID: 20708
	public delegate int GetTouchCountCallback();

	// Token: 0x02000912 RID: 2322
	// (Invoke) Token: 0x060050E8 RID: 20712
	public delegate UICamera.Touch GetTouchCallback(int index);
}
