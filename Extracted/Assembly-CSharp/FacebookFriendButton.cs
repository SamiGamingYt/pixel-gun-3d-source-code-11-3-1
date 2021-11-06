using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class FacebookFriendButton : MonoBehaviour
{
	// Token: 0x0600055D RID: 1373 RVA: 0x0002AEE4 File Offset: 0x000290E4
	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0002AF10 File Offset: 0x00029110
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (FacebookController.FacebookSupported)
		{
			NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(false);
			NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().facebookFriensPanel.gameObject.SetActive(true);
			FacebookController.sharedController.InputFacebookFriends(null, false);
		}
	}
}
