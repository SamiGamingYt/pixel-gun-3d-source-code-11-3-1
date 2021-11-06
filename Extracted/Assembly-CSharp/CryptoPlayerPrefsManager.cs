using System;
using UnityEngine;

// Token: 0x02000890 RID: 2192
public class CryptoPlayerPrefsManager : MonoBehaviour
{
	// Token: 0x06004EE0 RID: 20192 RVA: 0x001C9998 File Offset: 0x001C7B98
	private void Awake()
	{
		CryptoPlayerPrefs.setSalt(this.salt);
		CryptoPlayerPrefs.useRijndael(this.useRijndael);
		CryptoPlayerPrefs.useXor(this.useXor);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04003D52 RID: 15698
	public int salt = int.MaxValue;

	// Token: 0x04003D53 RID: 15699
	public bool useRijndael = true;

	// Token: 0x04003D54 RID: 15700
	public bool useXor = true;
}
