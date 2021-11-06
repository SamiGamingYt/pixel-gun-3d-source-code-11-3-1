using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class FacebookFriendsGUIController : MonoBehaviour
{
	// Token: 0x06000560 RID: 1376 RVA: 0x0002AF88 File Offset: 0x00029188
	private void Start()
	{
		FacebookFriendsGUIController.sharedController = this;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0002AF90 File Offset: 0x00029190
	private void Update()
	{
		if (!FacebookController.FacebookSupported)
		{
			return;
		}
		if (FacebookController.sharedController.friendsList == null || FacebookController.sharedController.friendsList.Count == 0)
		{
			return;
		}
		if (FriendsController.sharedController.facebookFriendsInfo.Count == 0 && !this._infoRequested && FriendsController.sharedController.GetFacebookFriendsCallback == null)
		{
			FriendsController.sharedController.GetFacebookFriendsInfo(new Action(this.GetFacebookFriendsCallback));
			this._infoRequested = true;
		}
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0002B018 File Offset: 0x00029218
	private void GetFacebookFriendsCallback()
	{
		if (FriendsController.sharedController == null || FriendsController.sharedController.facebookFriendsInfo == null)
		{
			return;
		}
		Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
		foreach (string text in FriendsController.sharedController.facebookFriendsInfo.Keys)
		{
			bool flag = false;
			if (FriendsController.sharedController.friends != null)
			{
				foreach (string text2 in FriendsController.sharedController.friends)
				{
					if (text2.Equals(text))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				dictionary.Add(text, FriendsController.sharedController.facebookFriendsInfo[text]);
			}
		}
		UIGrid componentInChildren = base.GetComponentInChildren<UIGrid>();
		if (componentInChildren == null)
		{
			return;
		}
		FriendPreview[] array = base.GetComponentsInChildren<FriendPreview>(true);
		if (array == null)
		{
			array = new FriendPreview[0];
		}
		Dictionary<string, FriendPreview> dictionary2 = new Dictionary<string, FriendPreview>();
		foreach (FriendPreview friendPreview in array)
		{
			if (friendPreview.id != null && dictionary.ContainsKey(friendPreview.id))
			{
				dictionary2.Add(friendPreview.id, friendPreview);
			}
			else
			{
				friendPreview.transform.parent = null;
				UnityEngine.Object.Destroy(friendPreview.gameObject);
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in dictionary)
		{
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> keyValuePair2 in keyValuePair.Value)
			{
				dictionary3.Add(keyValuePair2.Key, keyValuePair2.Value as string);
			}
			if (dictionary2.ContainsKey(keyValuePair.Key))
			{
				GameObject gameObject = dictionary2[keyValuePair.Key].gameObject;
				gameObject.GetComponent<FriendPreview>().facebookFriend = true;
				gameObject.GetComponent<FriendPreview>().id = keyValuePair.Key;
				if (keyValuePair.Value.ContainsKey("nick"))
				{
					gameObject.GetComponent<FriendPreview>().nm.text = (keyValuePair.Value["nick"] as string);
				}
				if (keyValuePair.Value.ContainsKey("rank"))
				{
					string text3 = keyValuePair.Value["rank"] as string;
					if (text3.Equals("0"))
					{
						text3 = "1";
					}
					gameObject.GetComponent<FriendPreview>().rank.spriteName = "Rank_" + text3;
				}
				if (keyValuePair.Value.ContainsKey("skin"))
				{
					gameObject.GetComponent<FriendPreview>().SetSkin(keyValuePair.Value["skin"] as string);
				}
				gameObject.GetComponent<FriendPreview>().FillClanAttrs(dictionary3);
			}
			else
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Friend") as GameObject);
				gameObject2.transform.parent = componentInChildren.transform;
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject2.GetComponent<FriendPreview>().facebookFriend = true;
				gameObject2.GetComponent<FriendPreview>().id = keyValuePair.Key;
				if (keyValuePair.Value.ContainsKey("nick"))
				{
					gameObject2.GetComponent<FriendPreview>().nm.text = (keyValuePair.Value["nick"] as string);
				}
				if (keyValuePair.Value.ContainsKey("rank"))
				{
					string text4 = keyValuePair.Value["rank"] as string;
					if (text4.Equals("0"))
					{
						text4 = "1";
					}
					gameObject2.GetComponent<FriendPreview>().rank.spriteName = "Rank_" + text4;
				}
				if (keyValuePair.Value.ContainsKey("skin"))
				{
					gameObject2.GetComponent<FriendPreview>().SetSkin(keyValuePair.Value["skin"] as string);
				}
				gameObject2.GetComponent<FriendPreview>().FillClanAttrs(dictionary3);
			}
		}
		base.StartCoroutine(this.Repos(componentInChildren));
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0002B534 File Offset: 0x00029734
	private IEnumerator Repos(UIGrid grid)
	{
		yield return null;
		grid.Reposition();
		yield break;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0002B558 File Offset: 0x00029758
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			FriendsController.sharedController.facebookFriendsInfo.Clear();
			this._infoRequested = false;
		}
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0002B578 File Offset: 0x00029778
	private void OnDestroy()
	{
		FriendsController.sharedController.GetFacebookFriendsCallback = null;
		FacebookFriendsGUIController.sharedController = null;
	}

	// Token: 0x040005D3 RID: 1491
	public static FacebookFriendsGUIController sharedController;

	// Token: 0x040005D4 RID: 1492
	public bool _infoRequested;
}
