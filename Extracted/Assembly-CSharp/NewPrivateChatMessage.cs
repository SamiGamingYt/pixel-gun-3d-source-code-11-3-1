using System;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class NewPrivateChatMessage : MonoBehaviour
{
	// Token: 0x06002336 RID: 9014 RVA: 0x000AEA4C File Offset: 0x000ACC4C
	private void Start()
	{
		this.newMessageSprite = base.gameObject.transform.GetChild(0).gameObject;
		this.UpdateStateNewMessage();
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000AEA7C File Offset: 0x000ACC7C
	private void UpdateStateNewMessage()
	{
		if (this.newMessageSprite.activeSelf != ChatController.countNewPrivateMessage > 0)
		{
			this.newMessageSprite.SetActive(ChatController.countNewPrivateMessage > 0);
		}
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000AEAAC File Offset: 0x000ACCAC
	private void Update()
	{
		this.UpdateStateNewMessage();
	}

	// Token: 0x0400177C RID: 6012
	private GameObject newMessageSprite;
}
