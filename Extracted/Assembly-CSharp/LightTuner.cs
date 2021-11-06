using System;
using UnityEngine;

// Token: 0x020002F4 RID: 756
[ExecuteInEditMode]
public class LightTuner : MonoBehaviour
{
	// Token: 0x06001A50 RID: 6736 RVA: 0x0006A6A4 File Offset: 0x000688A4
	private void Start()
	{
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x0006A6A8 File Offset: 0x000688A8
	private void Update()
	{
		if (this.apply)
		{
			foreach (Light light in this.lighters)
			{
				light.intensity *= this.value;
			}
			this.apply = false;
		}
	}

	// Token: 0x04000F6C RID: 3948
	public Light[] lighters;

	// Token: 0x04000F6D RID: 3949
	public float value;

	// Token: 0x04000F6E RID: 3950
	public bool apply;
}
