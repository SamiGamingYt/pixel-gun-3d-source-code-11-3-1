using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft.PixelGun3D
{
	// Token: 0x02000800 RID: 2048
	public sealed class RegenerationMode : MonoBehaviour
	{
		// Token: 0x06004A88 RID: 19080 RVA: 0x001A7780 File Offset: 0x001A5980
		private void Start()
		{
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x001A7784 File Offset: 0x001A5984
		private IEnumerator IncrementHealth()
		{
			for (;;)
			{
				yield return new WaitForSeconds(1f);
				if (this._playerController != null && this._playerController.CurHealth < this._playerController.MaxHealth)
				{
					this._playerController.CurHealth = this._playerController.CurHealth + 1f;
				}
			}
			yield break;
		}

		// Token: 0x04003724 RID: 14116
		private Player_move_c _playerController;
	}
}
