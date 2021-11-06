using System;
using UnityEngine;

// Token: 0x02000326 RID: 806
[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour
{
	// Token: 0x06001BE3 RID: 7139 RVA: 0x00073138 File Offset: 0x00071338
	protected virtual void Start()
	{
		if (this.reparentTarget == null)
		{
			this.reparentTarget = base.transform;
		}
	}

	// Token: 0x040010F1 RID: 4337
	public Transform reparentTarget;
}
