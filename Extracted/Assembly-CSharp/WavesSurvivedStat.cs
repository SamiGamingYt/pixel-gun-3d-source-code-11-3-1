using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000880 RID: 2176
[DisallowMultipleComponent]
internal sealed class WavesSurvivedStat : MonoBehaviour
{
	// Token: 0x06004E7A RID: 20090 RVA: 0x001C6FA8 File Offset: 0x001C51A8
	private WavesSurvivedStat()
	{
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x06004E7B RID: 20091 RVA: 0x001C6FB0 File Offset: 0x001C51B0
	// (set) Token: 0x06004E7C RID: 20092 RVA: 0x001C6FB8 File Offset: 0x001C51B8
	internal static int SurvivedWaveCount { get; set; }

	// Token: 0x06004E7D RID: 20093 RVA: 0x001C6FC0 File Offset: 0x001C51C0
	private void Start()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component != null)
		{
			component.text = WavesSurvivedStat.SurvivedWaveCount.ToString(CultureInfo.InvariantCulture);
		}
	}
}
