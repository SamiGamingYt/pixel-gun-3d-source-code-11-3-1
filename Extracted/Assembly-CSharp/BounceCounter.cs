using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class BounceCounter : MonoBehaviour
{
	// Token: 0x060009E0 RID: 2528 RVA: 0x0003A6B0 File Offset: 0x000388B0
	private void OnCollisionEnter(Collision c)
	{
		if (this.trackBounces)
		{
			this.conn.SaveDataOnTheCloud(base.gameObject.name, c.relativeVelocity.magnitude);
		}
	}

	// Token: 0x0400081A RID: 2074
	public UnityDataConnector conn;

	// Token: 0x0400081B RID: 2075
	public bool trackBounces = true;
}
