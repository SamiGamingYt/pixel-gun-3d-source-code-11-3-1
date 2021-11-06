using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020000AF RID: 175
internal sealed class EnderButton : MonoBehaviour
{
	// Token: 0x0600052B RID: 1323 RVA: 0x00029FF0 File Offset: 0x000281F0
	private void Start()
	{
		bool flag = BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer || (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon);
		if (!flag || !Defs.EnderManAvailable)
		{
			base.gameObject.SetActive(false);
		}
	}
}
