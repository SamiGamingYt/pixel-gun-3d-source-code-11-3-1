using System;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
internal sealed class ControlSize : MonoBehaviour
{
	// Token: 0x06003310 RID: 13072 RVA: 0x00108514 File Offset: 0x00106714
	private void Update()
	{
		if (this.maxValue < this.minValue)
		{
			this.maxValue = this.minValue;
		}
		if (this.defaultValue < this.minValue)
		{
			this.defaultValue = this.minValue;
		}
		if (this.defaultValue > this.maxValue)
		{
			this.defaultValue = this.maxValue;
		}
	}

	// Token: 0x0400258F RID: 9615
	public int minValue;

	// Token: 0x04002590 RID: 9616
	public int maxValue;

	// Token: 0x04002591 RID: 9617
	public int defaultValue;
}
