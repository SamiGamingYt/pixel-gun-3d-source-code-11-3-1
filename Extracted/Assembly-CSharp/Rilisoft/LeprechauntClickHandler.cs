using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000691 RID: 1681
	public class LeprechauntClickHandler : MonoBehaviour
	{
		// Token: 0x06003AC6 RID: 15046 RVA: 0x0012FDF4 File Offset: 0x0012DFF4
		private void OnClick()
		{
			if (Singleton<LeprechauntManager>.Instance != null)
			{
				LeprechauntLobbyView.Instance.Tap();
			}
		}
	}
}
