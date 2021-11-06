using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000022 RID: 34
internal sealed class BackFacebookFriens : MonoBehaviour
{
	// Token: 0x060000D8 RID: 216 RVA: 0x00007FD4 File Offset: 0x000061D4
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Facebook Friends");
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00008010 File Offset: 0x00006210
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00008030 File Offset: 0x00006230
	private void OnClick()
	{
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(true);
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().facebookFriensPanel.gameObject.SetActive(false);
		FriendsController.sharedController.facebookFriendsInfo.Clear();
		FacebookFriendsGUIController.sharedController._infoRequested = false;
		ButtonClickSound.Instance.PlayClick();
	}

	// Token: 0x040000BC RID: 188
	private IDisposable _backSubscription;
}
