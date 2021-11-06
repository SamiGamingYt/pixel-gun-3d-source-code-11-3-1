using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007FE RID: 2046
public class Lightning : MonoBehaviour
{
	// Token: 0x06004A6D RID: 19053 RVA: 0x001A726C File Offset: 0x001A546C
	private void Start()
	{
		if (this.child == null)
		{
			return;
		}
		if (this.sound != null)
		{
			this.sound.SetActive(false);
		}
		base.StartCoroutine(this.lightning());
	}

	// Token: 0x06004A6E RID: 19054 RVA: 0x001A72B8 File Offset: 0x001A54B8
	private IEnumerator lightning()
	{
		for (;;)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 90f));
			this.child.SetActive(true);
			this.sound.SetActive(false);
			yield return new WaitForSeconds(0.1f);
			this.child.SetActive(false);
			if (Defs.isSoundFX)
			{
				this.sound.SetActive(true);
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.1f));
			this.child.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			this.child.SetActive(false);
		}
		yield break;
	}

	// Token: 0x0400371B RID: 14107
	public GameObject child;

	// Token: 0x0400371C RID: 14108
	public GameObject sound;
}
