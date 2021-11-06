using System;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class HeadShotParticle : MonoBehaviour
{
	// Token: 0x06001509 RID: 5385 RVA: 0x0005344C File Offset: 0x0005164C
	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myParticleSystem.emit = false;
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x00053488 File Offset: 0x00051688
	public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		this.myParticleSystem.emit = true;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x000534C4 File Offset: 0x000516C4
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
			this.myParticleSystem.emit = false;
			this.isUseMine = false;
		}
	}

	// Token: 0x04000C46 RID: 3142
	private float liveTime = -1f;

	// Token: 0x04000C47 RID: 3143
	public float maxliveTime = 1.5f;

	// Token: 0x04000C48 RID: 3144
	public bool isUseMine;

	// Token: 0x04000C49 RID: 3145
	private Transform myTransform;

	// Token: 0x04000C4A RID: 3146
	public ParticleEmitter myParticleSystem;
}
