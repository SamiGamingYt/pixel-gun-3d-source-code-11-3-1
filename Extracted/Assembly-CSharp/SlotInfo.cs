using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200064F RID: 1615
[Serializable]
public class SlotInfo
{
	// Token: 0x0600381E RID: 14366 RVA: 0x00121F18 File Offset: 0x00120118
	public bool CheckAvaliableGift()
	{
		if ((GiftController.Instance != null && this.gift == null) || this.category == null || !this.category.AvailableGift(this.gift.Id, this.category.Type))
		{
			GiftController.Instance.UpdateSlot(this);
			return true;
		}
		return false;
	}

	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x0600381F RID: 14367 RVA: 0x00121F80 File Offset: 0x00120180
	// (set) Token: 0x06003820 RID: 14368 RVA: 0x00121FAC File Offset: 0x001201AC
	public int CountGift
	{
		get
		{
			if (this.isActiveEvent)
			{
				return this._countGift.Value;
			}
			return this.gift.Count.Value;
		}
		set
		{
			this._countGift.Value = value;
		}
	}

	// Token: 0x040028EC RID: 10476
	public GiftInfo gift;

	// Token: 0x040028ED RID: 10477
	public int positionInScroll;

	// Token: 0x040028EE RID: 10478
	public float percentGetSlot;

	// Token: 0x040028EF RID: 10479
	public GiftCategory category;

	// Token: 0x040028F0 RID: 10480
	public bool NoDropped;

	// Token: 0x040028F1 RID: 10481
	[HideInInspector]
	public bool isActiveEvent;

	// Token: 0x040028F2 RID: 10482
	private SaltedInt _countGift = new SaltedInt(15645675, 0);

	// Token: 0x040028F3 RID: 10483
	[HideInInspector]
	public int numInScroll;
}
