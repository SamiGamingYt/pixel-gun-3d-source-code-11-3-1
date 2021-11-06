using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class DamageFromMegabullet : MonoBehaviour
{
	// Token: 0x06000413 RID: 1043 RVA: 0x00023648 File Offset: 0x00021848
	private void Start()
	{
		this.myRocketScript = base.transform.root.GetComponent<Rocket>();
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00023660 File Offset: 0x00021860
	private void Update()
	{
		if (this.myRocketScript == null)
		{
			this.myRocketScript = base.transform.root.GetComponent<Rocket>();
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00023694 File Offset: 0x00021894
	private void OnTriggerEnter(Collider other)
	{
		if (this.myRocketScript == null)
		{
			return;
		}
		this.myRocketScript.OnMegabulletTriggerEnter(other);
	}

	// Token: 0x040004A3 RID: 1187
	private Rocket myRocketScript;
}
