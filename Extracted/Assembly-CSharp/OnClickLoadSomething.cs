using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000448 RID: 1096
public class OnClickLoadSomething : MonoBehaviour
{
	// Token: 0x060026DB RID: 9947 RVA: 0x000C2D1C File Offset: 0x000C0F1C
	public void OnClick()
	{
		OnClickLoadSomething.ResourceTypeOption resourceTypeToLoad = this.ResourceTypeToLoad;
		if (resourceTypeToLoad != OnClickLoadSomething.ResourceTypeOption.Scene)
		{
			if (resourceTypeToLoad == OnClickLoadSomething.ResourceTypeOption.Web)
			{
				Application.OpenURL(this.ResourceToLoad);
			}
		}
		else
		{
			SceneManager.LoadScene(this.ResourceToLoad);
		}
	}

	// Token: 0x04001B50 RID: 6992
	public OnClickLoadSomething.ResourceTypeOption ResourceTypeToLoad;

	// Token: 0x04001B51 RID: 6993
	public string ResourceToLoad;

	// Token: 0x02000449 RID: 1097
	public enum ResourceTypeOption : byte
	{
		// Token: 0x04001B53 RID: 6995
		Scene,
		// Token: 0x04001B54 RID: 6996
		Web
	}
}
