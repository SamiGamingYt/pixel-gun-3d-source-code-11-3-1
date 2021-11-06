using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020007C4 RID: 1988
	internal sealed class DialogEscape : MonoBehaviour
	{
		// Token: 0x060047F7 RID: 18423 RVA: 0x0018F33C File Offset: 0x0018D53C
		public DialogEscape()
		{
			this._buttonHandler = new Lazy<ButtonHandler>(new Func<ButtonHandler>(base.GetComponent<ButtonHandler>));
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060047F8 RID: 18424 RVA: 0x0018F35C File Offset: 0x0018D55C
		// (set) Token: 0x060047F9 RID: 18425 RVA: 0x0018F364 File Offset: 0x0018D564
		public string Context { get; set; }

		// Token: 0x060047FA RID: 18426 RVA: 0x0018F370 File Offset: 0x0018D570
		private void OnEnable()
		{
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
			}
			this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), this.Context ?? "Dialog");
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0018F3C4 File Offset: 0x0018D5C4
		private void OnDisable()
		{
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
				this._escapeSubscription = null;
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0018F3E4 File Offset: 0x0018D5E4
		private void HandleEscape()
		{
			if (this._buttonHandler.Value != null)
			{
				this._buttonHandler.Value.DoClick();
			}
		}

		// Token: 0x04003548 RID: 13640
		private readonly Lazy<ButtonHandler> _buttonHandler;

		// Token: 0x04003549 RID: 13641
		private IDisposable _escapeSubscription;
	}
}
