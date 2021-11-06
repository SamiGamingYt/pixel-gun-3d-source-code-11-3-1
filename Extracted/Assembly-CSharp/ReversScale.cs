using System;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class ReversScale : MonoBehaviour
{
	// Token: 0x06002BBE RID: 11198 RVA: 0x000E6274 File Offset: 0x000E4474
	private void Update()
	{
		if (this.x && base.transform.lossyScale.x < 0f)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, base.transform.localScale.y, base.transform.localScale.z);
		}
		if (this.y && base.transform.lossyScale.y < 0f)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y * -1f, base.transform.localScale.z);
		}
		if (this.z && base.transform.lossyScale.z < 0f)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z * -1f);
		}
	}

	// Token: 0x040020AD RID: 8365
	public bool x;

	// Token: 0x040020AE RID: 8366
	public bool y;

	// Token: 0x040020AF RID: 8367
	public bool z;
}
