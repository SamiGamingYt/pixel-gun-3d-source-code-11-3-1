using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000025 RID: 37
public sealed class BackRanksTapReceiver : MonoBehaviour
{
	// Token: 0x060000E2 RID: 226 RVA: 0x00008238 File Offset: 0x00006438
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Back Ranks");
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00008274 File Offset: 0x00006474
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00008294 File Offset: 0x00006494
	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		this.networkStartTableNGUIController.BackPressFromRanksTable(true);
	}

	// Token: 0x040000BF RID: 191
	public NetworkStartTableNGUIController networkStartTableNGUIController;

	// Token: 0x040000C0 RID: 192
	private IDisposable _backSubscription;
}
