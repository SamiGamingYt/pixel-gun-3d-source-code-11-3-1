using System;
using UnityEngine;

// Token: 0x020006CF RID: 1743
internal sealed class Pauser : MonoBehaviour
{
	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06003CAB RID: 15531 RVA: 0x0013B484 File Offset: 0x00139684
	// (set) Token: 0x06003CAC RID: 15532 RVA: 0x0013B48C File Offset: 0x0013968C
	public bool paused
	{
		get
		{
			return this.pausedVar;
		}
		set
		{
			this.pausedVar = value;
			if (JoystickController.leftJoystick == null || JoystickController.rightJoystick == null)
			{
				return;
			}
			if (this.pausedVar)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
				JoystickController.rightJoystick.gameObject.SetActive(false);
			}
			else
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
				JoystickController.rightJoystick.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x0013B528 File Offset: 0x00139728
	private void Start()
	{
		Pauser.sharedPauser = this;
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x0013B530 File Offset: 0x00139730
	private void OnDestroy()
	{
		Pauser.sharedPauser = null;
	}

	// Token: 0x04002CDE RID: 11486
	public static Pauser sharedPauser;

	// Token: 0x04002CDF RID: 11487
	private Action OnPlayerAddedAction;

	// Token: 0x04002CE0 RID: 11488
	public bool pausedVar;
}
