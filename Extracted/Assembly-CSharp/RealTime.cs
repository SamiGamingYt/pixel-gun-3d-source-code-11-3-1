using System;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class RealTime : MonoBehaviour
{
	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0008A76C File Offset: 0x0008896C
	public static float time
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x0008A774 File Offset: 0x00088974
	public static float deltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}
}
