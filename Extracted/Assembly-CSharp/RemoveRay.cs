using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class RemoveRay : MonoBehaviour
{
	// Token: 0x06002B9B RID: 11163 RVA: 0x000E57F4 File Offset: 0x000E39F4
	private IEnumerator Start()
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < this.lifetime)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002090 RID: 8336
	public float lifetime = 0.7f;

	// Token: 0x04002091 RID: 8337
	public float length = 100f;
}
