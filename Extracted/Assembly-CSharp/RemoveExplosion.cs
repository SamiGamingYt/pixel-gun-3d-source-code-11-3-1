using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
internal sealed class RemoveExplosion : MonoBehaviour
{
	// Token: 0x06002B90 RID: 11152 RVA: 0x000E5650 File Offset: 0x000E3850
	private void Start()
	{
		float num = (!(base.GetComponent<ParticleSystem>() != null)) ? 0.1f : base.GetComponent<ParticleSystem>().duration;
		if (base.GetComponent<AudioSource>() && base.GetComponent<AudioSource>().enabled && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().Play();
		}
		base.Invoke("Remove", 7f);
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x000E56CC File Offset: 0x000E38CC
	[Obfuscation(Exclude = true)]
	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
