using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007D9 RID: 2009
public class Dragon : MonoBehaviour
{
	// Token: 0x060048D6 RID: 18646 RVA: 0x00194A68 File Offset: 0x00192C68
	private void Start()
	{
		if (this.child == null || this.wingsFirst == null || this.wingsSecond == null)
		{
			return;
		}
		this.childSound = this.child.GetComponent<AudioSource>();
		base.StartCoroutine(this.dragonfly());
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x00194AC8 File Offset: 0x00192CC8
	private IEnumerator dragonfly()
	{
		for (;;)
		{
			yield return new WaitForSeconds(6.6666665f);
			if (Defs.isSoundFX)
			{
				this.wingsFirst.SetActive(true);
			}
			yield return new WaitForSeconds(3.2333333f);
			this.wingsFirst.SetActive(false);
			yield return new WaitForSeconds(6.6666665f);
			this.child.SetActive(true);
			this.childSound.enabled = Defs.isSoundFX;
			yield return new WaitForSeconds(5f);
			this.child.SetActive(false);
			if (Defs.isSoundFX)
			{
				this.wingsSecond.SetActive(true);
			}
			yield return new WaitForSeconds(4f);
			this.wingsSecond.SetActive(false);
			yield return new WaitForSeconds(23.71f);
		}
		yield break;
	}

	// Token: 0x040035ED RID: 13805
	public GameObject child;

	// Token: 0x040035EE RID: 13806
	private AudioSource childSound;

	// Token: 0x040035EF RID: 13807
	public GameObject wingsFirst;

	// Token: 0x040035F0 RID: 13808
	public GameObject wingsSecond;
}
