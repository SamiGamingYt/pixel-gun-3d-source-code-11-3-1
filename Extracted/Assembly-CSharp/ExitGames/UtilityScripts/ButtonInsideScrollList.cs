using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x02000463 RID: 1123
	public class ButtonInsideScrollList : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x000C4BCC File Offset: 0x000C2DCC
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.scrollRect != null)
			{
				this.scrollRect.StopMovement();
				this.scrollRect.enabled = false;
			}
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000C4C04 File Offset: 0x000C2E04
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.scrollRect != null && !this.scrollRect.enabled)
			{
				this.scrollRect.enabled = true;
			}
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000C4C34 File Offset: 0x000C2E34
		private void Start()
		{
			this.scrollRect = base.GetComponentInParent<ScrollRect>();
		}

		// Token: 0x04001B8C RID: 7052
		private ScrollRect scrollRect;
	}
}
