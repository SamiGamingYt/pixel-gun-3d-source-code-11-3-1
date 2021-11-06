using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class ColliderCollisions : MonoBehaviour
{
	// Token: 0x0600035C RID: 860 RVA: 0x0001C264 File Offset: 0x0001A464
	private void Awake()
	{
		this.thisTransform = base.transform;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0001C274 File Offset: 0x0001A474
	private void Update()
	{
		this.isCollision = (this.countColision > 0);
		this.countColision = 0;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0001C28C File Offset: 0x0001A48C
	private void OnTriggerStay(Collider other)
	{
		this.countColision++;
	}

	// Token: 0x04000398 RID: 920
	public bool isCollision;

	// Token: 0x04000399 RID: 921
	public Transform thisTransform;

	// Token: 0x0400039A RID: 922
	private int countColision;
}
