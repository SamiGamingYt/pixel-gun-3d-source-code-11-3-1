using System;
using UnityEngine;

// Token: 0x020007B8 RID: 1976
internal sealed class ShowHideRanksPanel : MonoBehaviour
{
	// Token: 0x060047A1 RID: 18337 RVA: 0x0018C098 File Offset: 0x0018A298
	private void OnEnable()
	{
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isMenu = false;
			ExperienceController.sharedController.isConnectScene = false;
			ExperienceController.sharedController.isShowRanks = false;
		}
		ActivityIndicator.IsActiveIndicator = true;
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x0018C0D4 File Offset: 0x0018A2D4
	private void OnDisable()
	{
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
			ExperienceController.sharedController.isMenu = true;
			ExperienceController.sharedController.isConnectScene = true;
			ExperienceController.sharedController.isShowRanks = true;
		}
		ActivityIndicator.IsActiveIndicator = false;
	}
}
