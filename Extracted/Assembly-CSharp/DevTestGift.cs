using System;
using UnityEngine;

// Token: 0x020007EA RID: 2026
public class DevTestGift : MonoBehaviour
{
	// Token: 0x020007EB RID: 2027
	private class DroppedSlotInfo
	{
		// Token: 0x06004945 RID: 18757 RVA: 0x001971FC File Offset: 0x001953FC
		public DroppedSlotInfo(SlotInfo slot)
		{
			this.Category = slot.category.Type;
			this.GiftId = ((slot.gift.RootInfo == null) ? slot.gift.Id : slot.gift.RootInfo.Id);
			this.GiftCount = slot.gift.Count.Value;
			this.PosInScroll = slot.positionInScroll;
			this.DropCount = 1;
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x00197280 File Offset: 0x00195480
		public bool Attach(DevTestGift.DroppedSlotInfo droppedSlotInfo)
		{
			if (!this.Equals(droppedSlotInfo))
			{
				return false;
			}
			this.DropCount += droppedSlotInfo.DropCount;
			return true;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x001972B0 File Offset: 0x001954B0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			DevTestGift.DroppedSlotInfo droppedSlotInfo = obj as DevTestGift.DroppedSlotInfo;
			return droppedSlotInfo != null && (this.Category == droppedSlotInfo.Category && this.GiftId == droppedSlotInfo.GiftId) && this.GiftCount == droppedSlotInfo.GiftCount;
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x0019730C File Offset: 0x0019550C
		public override string ToString()
		{
			return string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", new object[]
			{
				this.PosInScroll,
				this.Category,
				this.GiftId,
				this.GiftCount,
				this.DropCount
			});
		}

		// Token: 0x0400366D RID: 13933
		public GiftCategoryType Category;

		// Token: 0x0400366E RID: 13934
		public string GiftId;

		// Token: 0x0400366F RID: 13935
		public int GiftCount;

		// Token: 0x04003670 RID: 13936
		public int PosInScroll;

		// Token: 0x04003671 RID: 13937
		public int DropCount;
	}
}
