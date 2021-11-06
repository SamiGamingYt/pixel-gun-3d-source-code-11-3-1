using System;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class LookAtScript : MonoBehaviour
{
	// Token: 0x06001A79 RID: 6777 RVA: 0x0006B558 File Offset: 0x00069758
	private void Update()
	{
		base.transform.LookAt(this.t_target);
	}

	// Token: 0x04000F97 RID: 3991
	public Transform t_target;
}
