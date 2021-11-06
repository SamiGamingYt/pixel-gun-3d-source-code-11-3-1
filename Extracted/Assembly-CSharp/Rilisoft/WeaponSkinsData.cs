using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000888 RID: 2184
	[CreateAssetMenu(fileName = "weapon_skins_data", menuName = "Rilisoft/SO/WeaponSkins", order = 1)]
	public class WeaponSkinsData : ScriptableObject
	{
		// Token: 0x04003D31 RID: 15665
		public List<WeaponSkin> Data = new List<WeaponSkin>();
	}
}
