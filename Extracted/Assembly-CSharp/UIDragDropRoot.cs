using System;
using UnityEngine;

// Token: 0x02000329 RID: 809
[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	// Token: 0x06001BF8 RID: 7160 RVA: 0x0007399C File Offset: 0x00071B9C
	private void OnEnable()
	{
		UIDragDropRoot.root = base.transform;
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000739AC File Offset: 0x00071BAC
	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			UIDragDropRoot.root = null;
		}
	}

	// Token: 0x04001109 RID: 4361
	public static Transform root;
}
