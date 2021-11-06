using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200077F RID: 1919
public class PreloadTexture : MonoBehaviour
{
	// Token: 0x06004364 RID: 17252 RVA: 0x00167FDC File Offset: 0x001661DC
	private void OnEnable()
	{
		if (Device.IsLoweMemoryDevice)
		{
			if (this.nguiTexture == null)
			{
				this.nguiTexture = base.GetComponent<UITexture>();
			}
			if (this.nguiTexture != null)
			{
				base.StartCoroutine(this.Crt_LoadTexture());
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x0016803C File Offset: 0x0016623C
	private IEnumerator Crt_LoadTexture()
	{
		while (string.IsNullOrEmpty(this.pathTexture))
		{
			yield return null;
		}
		Texture needTx = Resources.Load<Texture>(this.pathTexture);
		if (this.nguiTexture != null)
		{
			this.nguiTexture.mainTexture = needTx;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x00168058 File Offset: 0x00166258
	private void OnDisable()
	{
		if (Device.IsLoweMemoryDevice)
		{
			if (this.nguiTexture != null)
			{
				this.nguiTexture.mainTexture = null;
			}
			ActivityIndicator.ClearMemory();
		}
	}

	// Token: 0x04003147 RID: 12615
	public string pathTexture;

	// Token: 0x04003148 RID: 12616
	public bool clearMemoryOnUnload = true;

	// Token: 0x04003149 RID: 12617
	private UITexture nguiTexture;
}
