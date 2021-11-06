using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000020 RID: 32
internal sealed class AvardPanelOkBtn : MonoBehaviour
{
	// Token: 0x060000D1 RID: 209 RVA: 0x00007F14 File Offset: 0x00006114
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Award Panel");
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00007F50 File Offset: 0x00006150
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00007F70 File Offset: 0x00006170
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideAvardPanel();
		}
	}

	// Token: 0x040000BB RID: 187
	private IDisposable _backSubscription;
}
