using System;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class HitParticle : MonoBehaviour
{
	// Token: 0x0600155A RID: 5466 RVA: 0x000553F8 File Offset: 0x000535F8
	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myParticleSystem.enableEmission = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x00055448 File Offset: 0x00053648
	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		base.gameObject.SetActive(true);
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		this.myParticleSystem.enableEmission = true;
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x00055498 File Offset: 0x00053698
	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine, Vector3 flyOutPos)
	{
		this.StartShowParticle(pos, rot, _isUseMine);
		if (this.myTransform.childCount > 0)
		{
			this.myParticleSystem.transform.position = flyOutPos;
		}
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x000554D4 File Offset: 0x000536D4
	private void Update()
	{
		if (this.liveTime < 0f)
		{
			return;
		}
		this.liveTime -= Time.deltaTime;
		if (this.liveTime < 0f)
		{
			this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
			this.myParticleSystem.enableEmission = false;
			this.isUseMine = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000CAF RID: 3247
	public const float DefaultHeightFlyOutEffect = 1.75f;

	// Token: 0x04000CB0 RID: 3248
	private float liveTime = -1f;

	// Token: 0x04000CB1 RID: 3249
	public float maxliveTime = 0.3f;

	// Token: 0x04000CB2 RID: 3250
	public bool isUseMine;

	// Token: 0x04000CB3 RID: 3251
	private Transform myTransform;

	// Token: 0x04000CB4 RID: 3252
	public ParticleSystem myParticleSystem;
}
