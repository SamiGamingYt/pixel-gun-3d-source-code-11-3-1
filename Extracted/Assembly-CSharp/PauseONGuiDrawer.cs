using System;
using UnityEngine;

// Token: 0x020003DC RID: 988
internal sealed class PauseONGuiDrawer : MonoBehaviour
{
	// Token: 0x060023A8 RID: 9128 RVA: 0x000B1788 File Offset: 0x000AF988
	private void OnGUI()
	{
		if (this.act != null)
		{
			this.act();
		}
	}

	// Token: 0x04001826 RID: 6182
	public Action act;
}
