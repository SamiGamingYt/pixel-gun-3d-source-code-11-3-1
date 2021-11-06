using System;
using UnityEngine;

// Token: 0x02000796 RID: 1942
public class ScrollWeaponEnableNotifier : MonoBehaviour
{
	// Token: 0x060045B7 RID: 17847 RVA: 0x00179168 File Offset: 0x00177368
	private void OnEnable()
	{
		this.inGameGui.StartCoroutine(this.inGameGui._DisableSwiping(0.5f));
	}

	// Token: 0x04003323 RID: 13091
	public InGameGUI inGameGui;
}
