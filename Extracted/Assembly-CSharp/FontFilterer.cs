using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
[ExecuteInEditMode]
public class FontFilterer : MonoBehaviour
{
	// Token: 0x06000845 RID: 2117 RVA: 0x00032508 File Offset: 0x00030708
	private void Start()
	{
		TextMesh component = base.GetComponent<TextMesh>();
		component.font.material.mainTexture.filterMode = FilterMode.Point;
	}
}
