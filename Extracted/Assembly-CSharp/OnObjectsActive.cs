using System;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class OnObjectsActive : MonoBehaviour
{
	// Token: 0x06002374 RID: 9076 RVA: 0x000B0988 File Offset: 0x000AEB88
	private void Update()
	{
		this.activeObjectsCount = this.objects.Length;
		for (int i = 0; i < this.objects.Length; i++)
		{
			this.activeObjectsCount += ((!this.objects[i].activeInHierarchy) ? -1 : 1);
		}
		this.objectToEnable.enabled = (this.activeObjectsCount > 0);
	}

	// Token: 0x040017EF RID: 6127
	public GameObject[] objects;

	// Token: 0x040017F0 RID: 6128
	private int activeObjectsCount;

	// Token: 0x040017F1 RID: 6129
	public MonoBehaviour objectToEnable;
}
