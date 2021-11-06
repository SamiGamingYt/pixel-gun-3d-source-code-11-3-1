using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class PlaySoundRepeatedly : MonoBehaviour
{
	// Token: 0x06003E3C RID: 15932 RVA: 0x0014DC94 File Offset: 0x0014BE94
	private void OnEnable()
	{
		base.StartCoroutine(this.SoundCoroutine());
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x0014DCA4 File Offset: 0x0014BEA4
	private IEnumerator SoundCoroutine()
	{
		yield return new WaitForSeconds(this.Delay);
		for (;;)
		{
			for (int i = 0; i < this.Repeats; i++)
			{
				if (Defs.isSoundFX)
				{
					base.GetComponent<AudioSource>().Play();
				}
				yield return new WaitForSeconds(this.Between);
			}
			yield return new WaitForSeconds(Mathf.Max(0f, this.Interval - this.Between * (float)this.Repeats));
		}
		yield break;
	}

	// Token: 0x04002E01 RID: 11777
	public float Delay;

	// Token: 0x04002E02 RID: 11778
	public int Repeats = 3;

	// Token: 0x04002E03 RID: 11779
	public float Between = 1f;

	// Token: 0x04002E04 RID: 11780
	public float Interval = 60f;
}
