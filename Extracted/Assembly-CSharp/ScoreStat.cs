using System;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class ScoreStat : MonoBehaviour
{
	// Token: 0x06002C7E RID: 11390 RVA: 0x000EBF1C File Offset: 0x000EA11C
	private void Start()
	{
		base.GetComponent<UILabel>().text = GlobalGameController.Score.ToString();
	}
}
