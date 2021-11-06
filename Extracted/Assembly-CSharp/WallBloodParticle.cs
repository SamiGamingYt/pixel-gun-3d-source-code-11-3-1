using System;
using UnityEngine;

// Token: 0x0200087E RID: 2174
public class WallBloodParticle : MonoBehaviour
{
	// Token: 0x06004E73 RID: 20083 RVA: 0x001C6D98 File Offset: 0x001C4F98
	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myParticleSystem.enableEmission = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06004E74 RID: 20084 RVA: 0x001C6DE8 File Offset: 0x001C4FE8
	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		this.myParticleSystem.enableEmission = true;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x001C6E38 File Offset: 0x001C5038
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

	// Token: 0x04003D06 RID: 15622
	private float liveTime = -1f;

	// Token: 0x04003D07 RID: 15623
	private float maxliveTime = 0.1f;

	// Token: 0x04003D08 RID: 15624
	public bool isUseMine;

	// Token: 0x04003D09 RID: 15625
	private Transform myTransform;

	// Token: 0x04003D0A RID: 15626
	public ParticleSystem myParticleSystem;
}
