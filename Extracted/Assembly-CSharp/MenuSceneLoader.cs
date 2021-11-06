using System;
using UnityEngine;

// Token: 0x020004DA RID: 1242
public class MenuSceneLoader : MonoBehaviour
{
	// Token: 0x06002C51 RID: 11345 RVA: 0x000EB328 File Offset: 0x000E9528
	private void Awake()
	{
		if (this.m_Go == null)
		{
			this.m_Go = UnityEngine.Object.Instantiate<GameObject>(this.menuUI);
		}
	}

	// Token: 0x04002157 RID: 8535
	public GameObject menuUI;

	// Token: 0x04002158 RID: 8536
	private GameObject m_Go;
}
