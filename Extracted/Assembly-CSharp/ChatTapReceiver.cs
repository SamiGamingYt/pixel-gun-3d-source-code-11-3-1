using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class ChatTapReceiver : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000292 RID: 658 RVA: 0x00016E18 File Offset: 0x00015018
	// (remove) Token: 0x06000293 RID: 659 RVA: 0x00016E30 File Offset: 0x00015030
	public static event Action ChatClicked;

	// Token: 0x06000294 RID: 660 RVA: 0x00016E48 File Offset: 0x00015048
	private void Start()
	{
		this.HandleChatSettUpdated();
		PauseNGUIController.ChatSettUpdated += this.HandleChatSettUpdated;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00016E64 File Offset: 0x00015064
	private void HandleChatSettUpdated()
	{
		base.gameObject.SetActive(Defs.isMulti && Defs.IsChatOn);
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00016E84 File Offset: 0x00015084
	private void OnDestroy()
	{
		PauseNGUIController.ChatSettUpdated -= this.HandleChatSettUpdated;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00016E98 File Offset: 0x00015098
	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if (ChatTapReceiver.ChatClicked != null)
		{
			ChatTapReceiver.ChatClicked();
		}
	}
}
