using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class CountKillsDuelLabel : MonoBehaviour
{
	// Token: 0x06000404 RID: 1028 RVA: 0x000233D4 File Offset: 0x000215D4
	private void Awake()
	{
		this.label = base.GetComponent<UILabel>();
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x000233E4 File Offset: 0x000215E4
	private void Update()
	{
		int b = 0;
		if (this.enemyKills)
		{
			if (DuelController.instance != null && DuelController.instance.opponentNetworkTable != null)
			{
				b = ((DuelController.instance.opponentNetworkTable.CountKills < 0) ? DuelController.instance.opponentNetworkTable.oldCountKills : DuelController.instance.opponentNetworkTable.CountKills);
			}
		}
		else
		{
			b = GlobalGameController.CountKills;
		}
		this.label.text = Mathf.Max(0, b).ToString();
	}

	// Token: 0x0400049A RID: 1178
	public bool enemyKills;

	// Token: 0x0400049B RID: 1179
	private UILabel label;
}
