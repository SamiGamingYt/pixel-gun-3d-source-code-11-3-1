using System;
using UnityEngine;

// Token: 0x02000865 RID: 2149
public class TopPanelsTapReceiver : MonoBehaviour
{
	// Token: 0x06004DA2 RID: 19874 RVA: 0x001C0FF0 File Offset: 0x001BF1F0
	// Note: this type is marked as 'beforefieldinit'.
	static TopPanelsTapReceiver()
	{
		TopPanelsTapReceiver.OnClicked = delegate()
		{
		};
	}

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06004DA3 RID: 19875 RVA: 0x001C1020 File Offset: 0x001BF220
	// (remove) Token: 0x06004DA4 RID: 19876 RVA: 0x001C1038 File Offset: 0x001BF238
	public static event Action OnClicked;

	// Token: 0x06004DA5 RID: 19877 RVA: 0x001C1050 File Offset: 0x001BF250
	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
	}

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001C1064 File Offset: 0x001BF264
	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		TopPanelsTapReceiver.OnClicked();
	}
}
