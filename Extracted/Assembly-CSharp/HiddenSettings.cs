using System;
using UnityEngine;

// Token: 0x02000653 RID: 1619
[Serializable]
public sealed class HiddenSettings : ScriptableObject
{
	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x0600384C RID: 14412 RVA: 0x00122614 File Offset: 0x00120814
	public string PersistentCacheManagerKey
	{
		get
		{
			return this.persistentCacheManagerKey;
		}
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x0600384D RID: 14413 RVA: 0x0012261C File Offset: 0x0012081C
	public string PlayerPrefsKey
	{
		get
		{
			return this.playerPrefsKey;
		}
	}

	// Token: 0x0400292E RID: 10542
	public string PhotonAppIdEncoded;

	// Token: 0x0400292F RID: 10543
	public string PhotonAppIdPad;

	// Token: 0x04002930 RID: 10544
	public string PhotonAppIdSignatureEncoded;

	// Token: 0x04002931 RID: 10545
	public string PhotonAppIdSignaturePad;

	// Token: 0x04002932 RID: 10546
	public string devtodevSecretGoogle;

	// Token: 0x04002933 RID: 10547
	public string devtodevSecretAmazon;

	// Token: 0x04002934 RID: 10548
	public string devtodevSecretIos;

	// Token: 0x04002935 RID: 10549
	public string devtodevSecretWsa;

	// Token: 0x04002936 RID: 10550
	public string appsFlyerAppKey;

	// Token: 0x04002937 RID: 10551
	public string flurryAmazonApiKey;

	// Token: 0x04002938 RID: 10552
	public string flurryAmazonDevApiKey;

	// Token: 0x04002939 RID: 10553
	public string flurryAndroidApiKey;

	// Token: 0x0400293A RID: 10554
	public string flurryAndroidDevApiKey;

	// Token: 0x0400293B RID: 10555
	public string flurryIosApiKey;

	// Token: 0x0400293C RID: 10556
	public string flurryIosDevApiKey;

	// Token: 0x0400293D RID: 10557
	[SerializeField]
	private string persistentCacheManagerKey;

	// Token: 0x0400293E RID: 10558
	[SerializeField]
	private string playerPrefsKey;
}
