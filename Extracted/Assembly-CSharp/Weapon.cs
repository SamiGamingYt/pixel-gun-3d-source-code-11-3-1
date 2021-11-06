using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200078C RID: 1932
public class Weapon
{
	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x060044D9 RID: 17625 RVA: 0x00173298 File Offset: 0x00171498
	// (set) Token: 0x060044DA RID: 17626 RVA: 0x001732A8 File Offset: 0x001714A8
	public int currentAmmoInBackpack
	{
		get
		{
			return this._currentAmmoInBackpack.Value;
		}
		set
		{
			this._currentAmmoInBackpack.Value = value;
		}
	}

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x060044DB RID: 17627 RVA: 0x001732B8 File Offset: 0x001714B8
	// (set) Token: 0x060044DC RID: 17628 RVA: 0x001732C8 File Offset: 0x001714C8
	public int currentAmmoInClip
	{
		get
		{
			return this._currentAmmoInClip.Value;
		}
		set
		{
			this._currentAmmoInClip.Value = value;
		}
	}

	// Token: 0x0400322A RID: 12842
	public GameObject weaponPrefab;

	// Token: 0x0400322B RID: 12843
	private SaltedInt _currentAmmoInBackpack = new SaltedInt(901269156);

	// Token: 0x0400322C RID: 12844
	private SaltedInt _currentAmmoInClip = new SaltedInt(384354114);
}
