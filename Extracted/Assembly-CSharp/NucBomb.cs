using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class NucBomb : MonoBehaviour
{
	// Token: 0x06002367 RID: 9063 RVA: 0x000B065C File Offset: 0x000AE85C
	private IEnumerator Start()
	{
		base.GetComponent<AudioSource>().Play();
		base.GetComponent<AudioSource>().mute = !Defs.isSoundFX;
		yield return new WaitForSeconds(this.BeforeActivate);
		base.transform.GetChild(0).gameObject.SetActive(true);
		yield return new WaitForSeconds(Mathf.Max(0f, this.BeforeDestroy - this.BeforeActivate));
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000B0678 File Offset: 0x000AE878
	private void FixedUpdate()
	{
		base.GetComponent<AudioSource>().mute = !Defs.isSoundFX;
	}

	// Token: 0x040017E1 RID: 6113
	public float BeforeActivate = 12f;

	// Token: 0x040017E2 RID: 6114
	public float BeforeDestroy = 90f;
}
