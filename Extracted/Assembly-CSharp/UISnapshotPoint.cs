using System;
using UnityEngine;

// Token: 0x0200037C RID: 892
[AddComponentMenu("NGUI/Internal/Snapshot Point")]
[ExecuteInEditMode]
public class UISnapshotPoint : MonoBehaviour
{
	// Token: 0x06001F4A RID: 8010 RVA: 0x0008FC40 File Offset: 0x0008DE40
	private void Start()
	{
		if (base.tag != "EditorOnly")
		{
			base.tag = "EditorOnly";
		}
	}

	// Token: 0x040013D1 RID: 5073
	public bool isOrthographic = true;

	// Token: 0x040013D2 RID: 5074
	public float nearClip = -100f;

	// Token: 0x040013D3 RID: 5075
	public float farClip = 100f;

	// Token: 0x040013D4 RID: 5076
	[Range(10f, 80f)]
	public int fieldOfView = 35;

	// Token: 0x040013D5 RID: 5077
	public float orthoSize = 30f;

	// Token: 0x040013D6 RID: 5078
	public Texture2D thumbnail;
}
