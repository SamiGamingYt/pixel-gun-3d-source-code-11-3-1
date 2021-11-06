using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class FriendPreviewClicker : MonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000861 RID: 2145 RVA: 0x00033AF8 File Offset: 0x00031CF8
	// (remove) Token: 0x06000862 RID: 2146 RVA: 0x00033B10 File Offset: 0x00031D10
	public static event Action<string> FriendPreviewClicked;

	// Token: 0x06000863 RID: 2147 RVA: 0x00033B28 File Offset: 0x00031D28
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (FriendPreviewClicker.FriendPreviewClicked != null)
		{
			string id = base.transform.parent.GetComponent<FriendPreview>().id;
			FriendPreviewClicker.FriendPreviewClicked(id);
		}
	}
}
