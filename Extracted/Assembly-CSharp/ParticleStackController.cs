using System;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class ParticleStackController : MonoBehaviour
{
	// Token: 0x0600237E RID: 9086 RVA: 0x000B0AF0 File Offset: 0x000AECF0
	private void Awake()
	{
		this.particleBuffer = new GameObject[this.capacity];
		for (int i = 0; i < this.particleBuffer.Length; i++)
		{
			this.particleBuffer[i] = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			this.particleBuffer[i].transform.parent = base.transform;
		}
		this.count = this.particleBuffer.Length;
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000B0B60 File Offset: 0x000AED60
	public void ReturnParticle(GameObject particle)
	{
		if (this.count < this.particleBuffer.Length)
		{
			particle.transform.parent = base.transform;
			particle.transform.localScale = Vector3.one;
			particle.SetActive(false);
			this.particleBuffer[this.count++] = particle;
		}
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000B0BC4 File Offset: 0x000AEDC4
	public GameObject GetParticle()
	{
		if (this.count <= 0)
		{
			return null;
		}
		GameObject gameObject = this.particleBuffer[--this.count];
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x040017F7 RID: 6135
	public GameObject prefab;

	// Token: 0x040017F8 RID: 6136
	public int capacity = 20;

	// Token: 0x040017F9 RID: 6137
	private int count;

	// Token: 0x040017FA RID: 6138
	private GameObject[] particleBuffer;
}
