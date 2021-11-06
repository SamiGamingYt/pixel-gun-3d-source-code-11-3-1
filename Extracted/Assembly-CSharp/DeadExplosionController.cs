using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class DeadExplosionController : MonoBehaviour
{
	// Token: 0x0600043F RID: 1087 RVA: 0x000244F0 File Offset: 0x000226F0
	public void StartAnim()
	{
		this.timeAnim = this.startTimerAnim + this.timeAfteAnim;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00024508 File Offset: 0x00022708
	private void Update()
	{
		if (this.timeAnim > 0f)
		{
			float value = 1.25f;
			this.timeAnim -= Time.deltaTime;
			if (this.timeAnim < this.startTimerAnim)
			{
				value = -0.25f + 1.5f * this.timeAnim / this.startTimerAnim;
			}
			this.mySkinRenderer.material.SetFloat("_Burn", value);
		}
	}

	// Token: 0x040004CE RID: 1230
	public SkinnedMeshRenderer mySkinRenderer;

	// Token: 0x040004CF RID: 1231
	public float timeAfteAnim = 0.5f;

	// Token: 0x040004D0 RID: 1232
	public float startTimerAnim = 0.5f;

	// Token: 0x040004D1 RID: 1233
	private float timeAnim = -1f;
}
