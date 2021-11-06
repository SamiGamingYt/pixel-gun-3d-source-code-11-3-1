using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class ExplosionDestroyer : MonoBehaviour
{
	// Token: 0x0600055A RID: 1370 RVA: 0x0002AEB0 File Offset: 0x000290B0
	private void OnEnable()
	{
		base.StartCoroutine(this.Remove());
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0002AEC0 File Offset: 0x000290C0
	private IEnumerator Remove()
	{
		yield return new WaitForSeconds(this.Time);
		ParticleEmitter pe = base.GetComponent<ParticleEmitter>();
		if (pe != null)
		{
			pe.emit = false;
		}
		yield break;
	}

	// Token: 0x040005D2 RID: 1490
	public float Time = 30f;
}
