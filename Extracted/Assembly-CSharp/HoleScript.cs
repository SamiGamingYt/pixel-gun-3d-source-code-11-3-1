using System;
using UnityEngine;

// Token: 0x020002AC RID: 684
public sealed class HoleScript : MonoBehaviour
{
	// Token: 0x06001566 RID: 5478 RVA: 0x00055758 File Offset: 0x00053958
	public void Init()
	{
		this.myTransform = base.transform;
		this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x0005579C File Offset: 0x0005399C
	public void StartShowHole(Vector3 pos, Quaternion rot, bool _isUseMine)
	{
		this.isUseMine = _isUseMine;
		this.liveTime = this.maxliveTime;
		this.myTransform.position = pos;
		this.myTransform.rotation = rot;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x000557E0 File Offset: 0x000539E0
	private void Update()
	{
		if (this.liveTime < 0f)
		{
			return;
		}
		this.liveTime -= Time.deltaTime;
		if (this.liveTime < 0f && this.myTransform)
		{
			this.myTransform.position = new Vector3(-10000f, -10000f, -10000f);
			this.isUseMine = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000CBA RID: 3258
	public float liveTime = -1f;

	// Token: 0x04000CBB RID: 3259
	public float maxliveTime = 3f;

	// Token: 0x04000CBC RID: 3260
	public bool isUseMine;

	// Token: 0x04000CBD RID: 3261
	private Transform myTransform;
}
