using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class DeadEnergyController : MonoBehaviour
{
	// Token: 0x0600043C RID: 1084 RVA: 0x000243F4 File Offset: 0x000225F4
	public void StartAnim(Color _color, Texture _skin)
	{
		this.timeAnim = this.startTimerAnim + this.timeAfteAnim;
		this.mySkinRenderer.material.SetColor("_BurnColor", _color);
		this.mySkinRenderer.material.SetTexture("_MainTex", _skin);
		this.myParticle.startColor = _color;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0002444C File Offset: 0x0002264C
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

	// Token: 0x040004C9 RID: 1225
	public SkinnedMeshRenderer mySkinRenderer;

	// Token: 0x040004CA RID: 1226
	public ParticleSystem myParticle;

	// Token: 0x040004CB RID: 1227
	public float timeAfteAnim;

	// Token: 0x040004CC RID: 1228
	public float startTimerAnim = 1f;

	// Token: 0x040004CD RID: 1229
	private float timeAnim = -1f;
}
