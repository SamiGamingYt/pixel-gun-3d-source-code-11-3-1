using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class BombsCratorNucl : MonoBehaviour
{
	// Token: 0x06000182 RID: 386 RVA: 0x0000F218 File Offset: 0x0000D418
	private IEnumerator Start()
	{
		if (Defs.isMulti)
		{
			this.oldBombPeriod = (int)PhotonNetwork.time / (int)this.time;
			for (;;)
			{
				if (this.oldBombPeriod < (int)PhotonNetwork.time / (int)this.time)
				{
					this.oldBombPeriod = (int)PhotonNetwork.time / (int)this.time;
					UnityEngine.Object.Instantiate<GameObject>(this.bmb);
				}
				yield return null;
			}
		}
		else
		{
			for (;;)
			{
				yield return new WaitForSeconds(this.time);
				if (this.bmb != null)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.bmb);
				}
			}
		}
		yield break;
	}

	// Token: 0x04000181 RID: 385
	public float time = 120f;

	// Token: 0x04000182 RID: 386
	public GameObject bmb;

	// Token: 0x04000183 RID: 387
	public int oldBombPeriod;
}
