using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	// Token: 0x020006CD RID: 1741
	public class OnClickAction : MonoBehaviour
	{
		// Token: 0x06003C94 RID: 15508 RVA: 0x0013AEFC File Offset: 0x001390FC
		private void OnClick()
		{
			if (this._events != null)
			{
				this._events.Invoke();
			}
		}

		// Token: 0x04002CCE RID: 11470
		[SerializeField]
		private UnityEvent _events;
	}
}
