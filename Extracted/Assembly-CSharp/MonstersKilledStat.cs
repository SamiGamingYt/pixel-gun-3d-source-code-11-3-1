using System;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class MonstersKilledStat : MonoBehaviour
{
	// Token: 0x06001B47 RID: 6983 RVA: 0x000700A4 File Offset: 0x0006E2A4
	private void Start()
	{
		base.GetComponent<UILabel>().text = PlayerPrefs.GetInt(Defs.KilledZombiesSett, 0).ToString();
	}
}
