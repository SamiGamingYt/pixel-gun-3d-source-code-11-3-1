using System;
using UnityEngine;

// Token: 0x020007D5 RID: 2005
public class RPG_Animation_CharacterFadeOnly : MonoBehaviour
{
	// Token: 0x060048BE RID: 18622 RVA: 0x00193810 File Offset: 0x00191A10
	private void Awake()
	{
		RPG_Animation_CharacterFadeOnly.instance = this;
	}

	// Token: 0x040035B2 RID: 13746
	public static RPG_Animation_CharacterFadeOnly instance;
}
