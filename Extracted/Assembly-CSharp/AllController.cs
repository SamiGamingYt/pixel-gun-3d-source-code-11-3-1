using System;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class AllController : MonoBehaviour
{
	// Token: 0x06002F0D RID: 12045 RVA: 0x000F5CE0 File Offset: 0x000F3EE0
	private void Awake()
	{
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		AllController.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000F5D1C File Offset: 0x000F3F1C
	private void OnDestroy()
	{
		AllController.instance = null;
	}

	// Token: 0x040022C2 RID: 8898
	public static AllController instance;
}
