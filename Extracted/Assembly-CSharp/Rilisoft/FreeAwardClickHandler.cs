using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005FE RID: 1534
	public class FreeAwardClickHandler : MonoBehaviour
	{
		// Token: 0x060034AC RID: 13484 RVA: 0x001101B4 File Offset: 0x0010E3B4
		private void OnClick()
		{
			if (FreeAwardShowHandler.Instance != null)
			{
				FreeAwardShowHandler.Instance.OnClick();
			}
		}
	}
}
