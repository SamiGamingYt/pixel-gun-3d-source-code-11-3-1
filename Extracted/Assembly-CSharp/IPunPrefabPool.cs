using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public interface IPunPrefabPool
{
	// Token: 0x060024DD RID: 9437
	GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);

	// Token: 0x060024DE RID: 9438
	void Destroy(GameObject gameObject);
}
