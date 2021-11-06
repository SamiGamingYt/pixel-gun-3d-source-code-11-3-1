using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	// Token: 0x0200004F RID: 79
	[RequireComponent(typeof(UIButton))]
	public class ButtonEventsHandler : MonoBehaviour
	{
		// Token: 0x0600020F RID: 527 RVA: 0x000135DC File Offset: 0x000117DC
		private void OnClick()
		{
			if (this.OnClickEvent != null)
			{
				this.OnClickEvent.Invoke();
			}
		}

		// Token: 0x0400023E RID: 574
		public UnityEvent OnClickEvent;
	}
}
