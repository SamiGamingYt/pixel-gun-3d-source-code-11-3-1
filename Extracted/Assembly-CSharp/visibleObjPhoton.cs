using System;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class visibleObjPhoton : MonoBehaviour
{
	// Token: 0x06004B7E RID: 19326 RVA: 0x001B2768 File Offset: 0x001B0968
	private void Awake()
	{
		if (!Defs.isMulti || !Defs.isInet)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x001B2788 File Offset: 0x001B0988
	private void Start()
	{
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x001B278C File Offset: 0x001B098C
	private void OnBecameVisible()
	{
		this.isVisible = true;
		if (this.lerpScript != null)
		{
			this.lerpScript.sglajEnabled = true;
		}
	}

	// Token: 0x06004B81 RID: 19329 RVA: 0x001B27C0 File Offset: 0x001B09C0
	private void OnBecameInvisible()
	{
		this.isVisible = false;
		if (this.lerpScript != null)
		{
			this.lerpScript.sglajEnabled = false;
		}
	}

	// Token: 0x04003A97 RID: 14999
	public ThirdPersonNetwork1 lerpScript;

	// Token: 0x04003A98 RID: 15000
	public bool isVisible;
}
