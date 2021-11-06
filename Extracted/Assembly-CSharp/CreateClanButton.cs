using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class CreateClanButton : MonoBehaviour
{
	// Token: 0x06000407 RID: 1031 RVA: 0x0002348C File Offset: 0x0002168C
	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		NGUITools.GetRoot(base.gameObject).GetComponent<ClansGUIController>().CreateClanPanel.SetActive(true);
	}
}
