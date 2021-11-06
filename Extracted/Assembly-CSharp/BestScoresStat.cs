using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class BestScoresStat : MonoBehaviour
{
	// Token: 0x06000169 RID: 361 RVA: 0x0000E96C File Offset: 0x0000CB6C
	private void Start()
	{
		base.GetComponent<UILabel>().text = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0).ToString();
	}
}
