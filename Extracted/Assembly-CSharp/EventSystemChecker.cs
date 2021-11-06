using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020004D9 RID: 1241
public class EventSystemChecker : MonoBehaviour
{
	// Token: 0x06002C4F RID: 11343 RVA: 0x000EB304 File Offset: 0x000E9504
	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		if (!UnityEngine.Object.FindObjectOfType<EventSystem>())
		{
			UnityEngine.Object.Instantiate<GameObject>(this.eventSystem);
		}
		yield break;
	}

	// Token: 0x04002156 RID: 8534
	public GameObject eventSystem;
}
