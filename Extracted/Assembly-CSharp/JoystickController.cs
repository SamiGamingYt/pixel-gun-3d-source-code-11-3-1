using System;
using UnityEngine;

// Token: 0x020002DC RID: 732
internal sealed class JoystickController : MonoBehaviour
{
	// Token: 0x0600199D RID: 6557 RVA: 0x00066534 File Offset: 0x00064734
	private void Awake()
	{
		JoystickController.leftJoystick = this._leftJoystick;
		JoystickController.rightJoystick = this._rightJoystick;
		JoystickController.leftTouchPad = this._leftTouchPad;
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x00066558 File Offset: 0x00064758
	private void OnDestroy()
	{
		JoystickController.leftJoystick = null;
		JoystickController.rightJoystick = null;
		JoystickController.leftTouchPad = null;
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x0006656C File Offset: 0x0006476C
	public static bool IsButtonFireUp()
	{
		return !JoystickController.leftTouchPad.isShooting && !JoystickController.rightJoystick.isShooting;
	}

	// Token: 0x04000EB5 RID: 3765
	public static UIJoystick leftJoystick;

	// Token: 0x04000EB6 RID: 3766
	public static TouchPadController rightJoystick;

	// Token: 0x04000EB7 RID: 3767
	public static TouchPadInJoystick leftTouchPad;

	// Token: 0x04000EB8 RID: 3768
	public UIJoystick _leftJoystick;

	// Token: 0x04000EB9 RID: 3769
	public TouchPadController _rightJoystick;

	// Token: 0x04000EBA RID: 3770
	public TouchPadInJoystick _leftTouchPad;
}
