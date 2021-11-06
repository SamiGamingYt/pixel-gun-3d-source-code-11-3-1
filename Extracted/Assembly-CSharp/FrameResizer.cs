using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class FrameResizer : MonoBehaviour
{
	// Token: 0x06000847 RID: 2119 RVA: 0x0003253C File Offset: 0x0003073C
	public void ResizeFrame()
	{
		this.activeObjectsCounter = 0;
		for (int i = 0; i < this.objects.Length; i++)
		{
			if (this.objects[i].activeSelf)
			{
				this.activeObjectsCounter++;
			}
		}
		if (this.activeObjectsCounter > 0)
		{
			this.frame.width = Mathf.RoundToInt(this.frameSize[this.activeObjectsCounter - 1].x);
			this.frame.height = Mathf.RoundToInt(this.frameSize[this.activeObjectsCounter - 1].y);
		}
		if (this.table != null)
		{
			this.table.sorting = UITable.Sorting.Alphabetic;
			this.table.Reposition();
			this.table.repositionNow = true;
		}
	}

	// Token: 0x040006E7 RID: 1767
	public GameObject[] objects;

	// Token: 0x040006E8 RID: 1768
	public UISprite frame;

	// Token: 0x040006E9 RID: 1769
	public UITable table;

	// Token: 0x040006EA RID: 1770
	public Vector2[] frameSize;

	// Token: 0x040006EB RID: 1771
	private int activeObjectsCounter;
}
