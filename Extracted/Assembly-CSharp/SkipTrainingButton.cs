using System;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class SkipTrainingButton : MonoBehaviour
{
	// Token: 0x14000098 RID: 152
	// (add) Token: 0x0600420F RID: 16911 RVA: 0x0015F6D4 File Offset: 0x0015D8D4
	// (remove) Token: 0x06004210 RID: 16912 RVA: 0x0015F6EC File Offset: 0x0015D8EC
	public static event Action SkipTrClosed;

	// Token: 0x06004211 RID: 16913 RVA: 0x0015F704 File Offset: 0x0015D904
	protected virtual void OnClick()
	{
		if (SkipTrainingButton.SkipTrClosed != null)
		{
			SkipTrainingButton.SkipTrClosed();
		}
		Resources.UnloadUnusedAssets();
	}
}
