using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200066B RID: 1643
	public class InAppBonusLobbyClickHandler : MonoBehaviour
	{
		// Token: 0x06003930 RID: 14640 RVA: 0x00128608 File Offset: 0x00126808
		private void OnClick()
		{
			if (InAppBonusLobbyController.Instance != null)
			{
				InAppBonusLobbyController.Instance.Click();
			}
		}
	}
}
