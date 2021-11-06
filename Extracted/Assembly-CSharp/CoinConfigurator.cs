using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000072 RID: 114
internal sealed class CoinConfigurator : MonoBehaviour
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000341 RID: 833 RVA: 0x0001BD7C File Offset: 0x00019F7C
	public VirtualCurrencyBonusType BonusType
	{
		get
		{
			return this.bonusType;
		}
	}

	// Token: 0x0400037F RID: 895
	[SerializeField]
	private VirtualCurrencyBonusType bonusType;

	// Token: 0x04000380 RID: 896
	public bool CoinIsPresent = true;

	// Token: 0x04000381 RID: 897
	public Vector3 pos = default(Vector3);

	// Token: 0x04000382 RID: 898
	public Transform coinCreatePoint;
}
