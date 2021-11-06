using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200088A RID: 2186
	public class WeaponSkinsTestComponent : MonoBehaviour
	{
		// Token: 0x06004EB1 RID: 20145 RVA: 0x001C83E8 File Offset: 0x001C65E8
		private void OnGUI()
		{
		}

		// Token: 0x04003D38 RID: 15672
		private GameObject _prevGo;

		// Token: 0x04003D39 RID: 15673
		[SerializeField]
		private GameObject _go;

		// Token: 0x04003D3A RID: 15674
		[SerializeField]
		private bool _doNotCreteBaseSkin;

		// Token: 0x04003D3B RID: 15675
		[SerializeField]
		[ReadOnly]
		private List<WeaponSkin> _skins = new List<WeaponSkin>();

		// Token: 0x04003D3C RID: 15676
		[ReadOnly]
		[SerializeField]
		private WeaponSkin _currentSkin;

		// Token: 0x04003D3D RID: 15677
		private WeaponSkin _baseSkin;
	}
}
