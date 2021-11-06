using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class BootsMaterial : MonoBehaviour
{
	// Token: 0x060001B7 RID: 439 RVA: 0x00010FFC File Offset: 0x0000F1FC
	public void SetBootsMaterial(string materialName)
	{
		for (int i = 0; i < this.materialList.Length; i++)
		{
			if (this.materialList[i].name == materialName)
			{
				this.bootRenderer.sharedMaterial = this.materialList[i];
			}
		}
	}

	// Token: 0x040001BE RID: 446
	public MeshRenderer bootRenderer;

	// Token: 0x040001BF RID: 447
	public Material[] materialList;
}
