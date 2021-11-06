using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class RayAndExplosionsStackItem : MonoBehaviour
{
	// Token: 0x06002B76 RID: 11126 RVA: 0x000E5164 File Offset: 0x000E3364
	private void Start()
	{
		this.isNotAutoEnd = (base.GetComponent<FreezerRay>() == null);
		if (this.isNotAutoEnd)
		{
			base.Invoke("Deactivate", this.timeDeactivate);
		}
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x000E51A0 File Offset: 0x000E33A0
	private void OnEnable()
	{
		this.isNotAutoEnd = (base.GetComponent<FreezerRay>() == null);
		if (base.GetComponent<AudioSource>() && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().Play();
		}
		base.Invoke("Deactivate", this.timeDeactivate);
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x000E51F8 File Offset: 0x000E33F8
	[Obfuscation(Exclude = true)]
	public void Deactivate()
	{
		base.CancelInvoke("Deactivate");
		if (RayAndExplosionsStackController.sharedController != null)
		{
			if (base.GetComponent<AudioSource>())
			{
				base.GetComponent<AudioSource>().Stop();
			}
			RayAndExplosionsStackController.sharedController.ReturnObjectFromName(base.gameObject, this.myName);
		}
	}

	// Token: 0x04002080 RID: 8320
	public string myName;

	// Token: 0x04002081 RID: 8321
	public float timeDeactivate = 1f;

	// Token: 0x04002082 RID: 8322
	private bool isNotAutoEnd;
}
