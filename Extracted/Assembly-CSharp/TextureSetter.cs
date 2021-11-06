using System;
using UnityEngine;

// Token: 0x0200085A RID: 2138
public sealed class TextureSetter : MonoBehaviour
{
	// Token: 0x06004D5D RID: 19805 RVA: 0x001BEDE8 File Offset: 0x001BCFE8
	private void Awake()
	{
		SkipPresser.SkipPressed += this.SetTexture;
		SkipTrainingButton.SkipTrClosed += this.UnsetTexture;
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001BEE18 File Offset: 0x001BD018
	private void OnDestroy()
	{
		SkipPresser.SkipPressed -= this.SetTexture;
		SkipTrainingButton.SkipTrClosed -= this.UnsetTexture;
	}

	// Token: 0x06004D5F RID: 19807 RVA: 0x001BEE48 File Offset: 0x001BD048
	private void SetTexture()
	{
		if (string.IsNullOrEmpty(this.TextureName))
		{
			return;
		}
		string path = ResPath.Combine("SkipTraining", this.TextureName);
		base.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(path);
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x001BEE88 File Offset: 0x001BD088
	private void UnsetTexture()
	{
		base.GetComponent<UITexture>().mainTexture = null;
	}

	// Token: 0x04003BC3 RID: 15299
	public string TextureName;
}
