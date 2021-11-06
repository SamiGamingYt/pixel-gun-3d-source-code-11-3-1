using System;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class WrapContent3D : MonoBehaviour
{
	// Token: 0x06004EBB RID: 20155 RVA: 0x001C8AD0 File Offset: 0x001C6CD0
	private void Update()
	{
		for (int i = 0; i < this.wrappedObjects.Length; i++)
		{
			float f = (this.wrappedObjects[i].position.x - this.center.position.x) / 0.002604167f;
			float num = Mathf.Clamp01((this.maxDistance - Mathf.Abs(f)) / this.maxDistance);
			this.wrappedObjects[i].localScale = Vector3.Lerp(Vector3.one, Vector3.zero, 0.7f - num);
			this.wrappedObjects[i].gameObject.SetActive(Mathf.Abs(f) < this.maxDistance);
		}
	}

	// Token: 0x04003D43 RID: 15683
	public Transform[] wrappedObjects;

	// Token: 0x04003D44 RID: 15684
	public float maxDistance;

	// Token: 0x04003D45 RID: 15685
	public float deltaX;

	// Token: 0x04003D46 RID: 15686
	public Transform center;
}
