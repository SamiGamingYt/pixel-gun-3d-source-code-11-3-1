using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class SynchNetworkAnimation : MonoBehaviour
{
	// Token: 0x06004BF0 RID: 19440 RVA: 0x001B57A4 File Offset: 0x001B39A4
	private void Start()
	{
		if (!Defs.isMulti || !Defs.isInet)
		{
			return;
		}
		this.anim = base.GetComponent<Animation>();
		this.currState = this.anim[this.anim.clip.name];
		this.currState.normalizedTime = (float)(PhotonNetwork.time % (double)this.anim.clip.length) / this.anim.clip.length;
		this.anim.Play();
		base.StartCoroutine(this.UpdateState());
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x001B5840 File Offset: 0x001B3A40
	private IEnumerator UpdateState()
	{
		for (;;)
		{
			this.currState.normalizedTime = (float)(PhotonNetwork.time % (double)this.anim.clip.length) / this.anim.clip.length;
			yield return new WaitForSeconds(10f);
		}
		yield break;
	}

	// Token: 0x04003B1D RID: 15133
	private Animation anim;

	// Token: 0x04003B1E RID: 15134
	private AnimationState currState;
}
