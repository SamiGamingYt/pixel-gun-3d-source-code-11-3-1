using System;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
public class ShopPositionParams : MonoBehaviour
{
	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x06004793 RID: 18323 RVA: 0x0018BEC4 File Offset: 0x0018A0C4
	public int League
	{
		get
		{
			return this.league - 1;
		}
	}

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x06004794 RID: 18324 RVA: 0x0018BED0 File Offset: 0x0018A0D0
	public string shopName
	{
		get
		{
			return LocalizationStore.Get(this.localizeKey);
		}
	}

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x06004795 RID: 18325 RVA: 0x0018BEE0 File Offset: 0x0018A0E0
	public string shopNameNonLocalized
	{
		get
		{
			return LocalizationStore.GetByDefault(this.localizeKey).ToUpper();
		}
	}

	// Token: 0x040034C7 RID: 13511
	public int tier = 10;

	// Token: 0x040034C8 RID: 13512
	public int league = 1;

	// Token: 0x040034C9 RID: 13513
	public float scaleShop = 150f;

	// Token: 0x040034CA RID: 13514
	public Vector3 positionShop;

	// Token: 0x040034CB RID: 13515
	public Vector3 rotationShop;

	// Token: 0x040034CC RID: 13516
	public string localizeKey;
}
