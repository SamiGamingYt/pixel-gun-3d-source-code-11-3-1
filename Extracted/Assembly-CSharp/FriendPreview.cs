using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x02000122 RID: 290
public sealed class FriendPreview : MonoBehaviour
{
	// Token: 0x06000851 RID: 2129 RVA: 0x0003277C File Offset: 0x0003097C
	public FriendPreview()
	{
		this._clansGuiController = new Lazy<ClansGUIController>(new Func<ClansGUIController>(base.GetComponentInParent<ClansGUIController>));
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003279C File Offset: 0x0003099C
	public void HandleAvatarClick()
	{
		Action<bool> onCloseEvent = delegate(bool needUpdate)
		{
		};
		FriendsController.ShowProfile(this.id, ProfileWindowType.other, onCloseEvent);
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x000327D4 File Offset: 0x000309D4
	private void Start()
	{
		if (!this.facebookFriend && !this.ClanMember && !this.ClanInvite)
		{
			FriendsGUIController.UpdaeOnlineEvent = (Action)Delegate.Combine(FriendsGUIController.UpdaeOnlineEvent, new Action(this.UpdateOnline));
		}
		if (this.isInviteFromUs && this.preview != null)
		{
			this.preview.alpha = 0.4f;
		}
		this.join.SetActive(this.join.activeSelf && (!this.facebookFriend || this.ClanMember) && !this.ClanInvite);
		this.delete.SetActive(this.delete.activeSelf && !this.facebookFriend && !this.ClanInvite);
		this.join.GetComponent<UIButton>().isEnabled = false;
		if (this.onlineStateContainer != null)
		{
			this.onlineStateContainer.SetActive(!this.facebookFriend);
		}
		this.addFacebookFriend.SetActive(this.facebookFriend || this.ClanInvite);
		if (this.ClanInvite)
		{
			this.addFacebookFriend.GetComponent<UIButton>().isEnabled = !FriendsController.sharedController.ClanLimitReached;
			UIButton component = this.avatarButton.GetComponent<UIButton>();
			if (component != null)
			{
				EventDelegate.Callback call = delegate()
				{
					if (this._clansGuiController.Value != null)
					{
						ClansGUIController.State previousState = this._clansGuiController.Value.CurrentState;
						Action<bool> onCloseEvent = delegate(bool needUpdateFriendList)
						{
							if (this._clansGuiController.Value != null)
							{
								this._clansGuiController.Value.CurrentState = previousState;
								this._clansGuiController.Value.ShowAddMembersScreen();
							}
						};
						FriendsController.ShowProfile(this.id, ProfileWindowType.other, onCloseEvent);
						this._clansGuiController.Value.CurrentState = ClansGUIController.State.ProfileDetails;
					}
				};
				EventDelegate item = new EventDelegate(call);
				component.onClick.Add(item);
			}
		}
		bool flag = true;
		this.cancel.SetActive(this.ClanInvite && flag);
		this.avatarButton.GetComponent<UIButton>().enabled = !this.facebookFriend;
		if (this.facebookFriend || this.ClanInvite)
		{
			UnityEngine.Object.Destroy(this.avatarButton.GetComponent<FriendPreviewClicker>());
		}
		this.UpdateInfo();
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000329D4 File Offset: 0x00030BD4
	public void SetSkin(string skinStr)
	{
		bool flag = true;
		if (!string.IsNullOrEmpty(skinStr) && !skinStr.Equals("empty"))
		{
			byte[] data = Convert.FromBase64String(skinStr);
			Texture2D texture2D = new Texture2D(64, 32);
			texture2D.LoadImage(data);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			this.mySkin = texture2D;
		}
		else
		{
			this.mySkin = (Resources.Load(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1")) as Texture2D);
			flag = false;
		}
		Texture2D texture2D2 = new Texture2D(20, 20, TextureFormat.ARGB32, false);
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < 20; j++)
			{
				texture2D2.SetPixel(i, j, Color.clear);
			}
		}
		texture2D2.SetPixels(6, 6, 8, 8, this.GetPixelsByRect(this.mySkin, new Rect(8f, 16f, 8f, 8f)));
		texture2D2.SetPixels(6, 0, 8, 6, this.GetPixelsByRect(this.mySkin, new Rect(20f, 6f, 8f, 6f)));
		texture2D2.SetPixels(2, 0, 4, 6, this.GetPixelsByRect(this.mySkin, new Rect(44f, 6f, 4f, 6f)));
		texture2D2.SetPixels(14, 0, 4, 6, this.GetPixelsByRect(this.mySkin, new Rect(44f, 6f, 4f, 6f)));
		texture2D2.anisoLevel = 1;
		texture2D2.mipMapBias = -0.5f;
		texture2D2.Apply();
		texture2D2.filterMode = FilterMode.Point;
		if (flag)
		{
			UnityEngine.Object.Destroy(this.mySkin);
		}
		Texture mainTexture = this.preview.mainTexture;
		this.preview.mainTexture = texture2D2;
		if (mainTexture != null && !mainTexture.name.Equals("dude") && !mainTexture.name.Equals("multi_skin_1"))
		{
			UnityEngine.Object.Destroy(mainTexture);
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00032BE0 File Offset: 0x00030DE0
	private void UpdateInfo()
	{
		if (this.facebookFriend)
		{
			return;
		}
		if (this.id != null)
		{
			if (!this.ClanMember)
			{
				Dictionary<string, object> dictionary;
				object obj;
				if (FriendsController.sharedController.playersInfo.TryGetValue(this.id, out dictionary) && dictionary.TryGetValue("player", out obj))
				{
					Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
					this.nm.text = (dictionary2["nick"] as string);
					this.rank.spriteName = "Rank_" + Convert.ToString(dictionary2["rank"]);
					string skin = dictionary2["skin"] as string;
					this.SetSkin(skin);
					Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
					foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
					{
						dictionary3.Add(keyValuePair.Key, Convert.ToString(keyValuePair.Value));
					}
					this.FillClanAttrs(dictionary3);
				}
			}
			else
			{
				foreach (Dictionary<string, string> dictionary4 in FriendsController.sharedController.clanMembers)
				{
					string text;
					if (dictionary4.TryGetValue("id", out text) && this.id.Equals(text))
					{
						if (dictionary4.ContainsKey("nick"))
						{
							this.nm.text = dictionary4["nick"];
						}
						if (dictionary4.ContainsKey("rank"))
						{
							this.rank.spriteName = "Rank_" + dictionary4["rank"];
						}
						if (dictionary4.ContainsKey("skin"))
						{
							string skin2 = dictionary4["skin"];
							this.SetSkin(skin2);
						}
						Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
						object obj2;
						if (FriendsController.sharedController.playersInfo.ContainsKey(text) && FriendsController.sharedController.playersInfo[text].TryGetValue("player", out obj2))
						{
							Dictionary<string, object> dictionary6 = obj2 as Dictionary<string, object>;
							foreach (KeyValuePair<string, object> keyValuePair2 in dictionary6)
							{
								dictionary5.Add(keyValuePair2.Key, Convert.ToString(keyValuePair2.Value));
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(FriendsController.sharedController.clanName))
							{
								dictionary5.Add("clan_name", FriendsController.sharedController.clanName);
							}
							if (!string.IsNullOrEmpty(FriendsController.sharedController.clanLeaderID))
							{
								dictionary5.Add("clan_creator_id", FriendsController.sharedController.clanLeaderID);
							}
						}
						this.FillClanAttrs(dictionary5);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00032F2C File Offset: 0x0003112C
	public void FillClanAttrs(Dictionary<string, string> plDict)
	{
		if (plDict == null)
		{
			return;
		}
		if (this.ClanMember)
		{
			string text = null;
			if (!string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				text = FriendsController.sharedController.clanLogo;
			}
			else if (plDict.ContainsKey("clan_logo") && plDict["clan_logo"] != null && plDict["clan_logo"] != null && !plDict["clan_logo"].Equals("null"))
			{
				text = plDict["clan_logo"];
			}
			if (text != null)
			{
				this.ClanLogo.gameObject.SetActive(true);
				try
				{
					byte[] data = Convert.FromBase64String(text);
					Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
					texture2D.LoadImage(data);
					texture2D.filterMode = FilterMode.Point;
					texture2D.Apply();
					Texture mainTexture = this.ClanLogo.mainTexture;
					this.ClanLogo.mainTexture = texture2D;
					if (mainTexture != null)
					{
						UnityEngine.Object.Destroy(mainTexture);
					}
				}
				catch (Exception ex)
				{
					Texture mainTexture2 = this.ClanLogo.mainTexture;
					this.ClanLogo.mainTexture = null;
					if (mainTexture2 != null)
					{
						UnityEngine.Object.Destroy(mainTexture2);
					}
				}
			}
		}
		else if (plDict.ContainsKey("clan_logo") && !string.IsNullOrEmpty(plDict["clan_logo"]) && !plDict["clan_logo"].Equals("null"))
		{
			this.ClanLogo.gameObject.SetActive(true);
			try
			{
				byte[] data2 = Convert.FromBase64String(plDict["clan_logo"]);
				Texture2D texture2D2 = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
				texture2D2.LoadImage(data2);
				texture2D2.filterMode = FilterMode.Point;
				texture2D2.Apply();
				Texture mainTexture3 = this.ClanLogo.mainTexture;
				this.ClanLogo.mainTexture = texture2D2;
				if (mainTexture3 != null)
				{
					UnityEngine.Object.Destroy(mainTexture3);
				}
			}
			catch (Exception ex2)
			{
				Texture mainTexture4 = this.ClanLogo.mainTexture;
				this.ClanLogo.mainTexture = null;
				if (mainTexture4 != null)
				{
					UnityEngine.Object.Destroy(mainTexture4);
				}
			}
		}
		else
		{
			this.ClanLogo.gameObject.SetActive(false);
		}
		if (plDict.ContainsKey("clan_name") && plDict["clan_name"] != null && !plDict["clan_name"].Equals("null"))
		{
			this.clanName.gameObject.SetActive(true);
			string text2 = plDict["clan_name"];
			int num = 12;
			if (text2 != null && text2.Length > num)
			{
				text2 = string.Format("{0}..{1}", text2.Substring(0, (num - 2) / 2), text2.Substring(text2.Length - (num - 2) / 2, (num - 2) / 2));
			}
			if (text2 != null)
			{
				this.clanName.text = text2;
			}
		}
		else
		{
			this.clanName.gameObject.SetActive(false);
		}
		if (plDict.ContainsKey("clan_creator_id") && plDict["clan_creator_id"] != null && this.id != null)
		{
			bool flag = plDict["clan_creator_id"].Equals(this.id);
			this.leader.SetActive(flag);
			this.avatarButton.GetComponent<UIButton>().normalSprite = ((!flag) ? "avatar_frame" : "avatar_leader_frame");
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x000332FC File Offset: 0x000314FC
	private Texture2D getTexFromTexByRect(Texture2D texForCut, Rect rectForCut)
	{
		Color[] pixels = texForCut.GetPixels((int)rectForCut.x, (int)rectForCut.y, (int)rectForCut.width, (int)rectForCut.height);
		Texture2D texture2D = new Texture2D((int)rectForCut.width, (int)rectForCut.height);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0003335C File Offset: 0x0003155C
	private Color[] GetPixelsByRect(Texture2D texture, Rect rect)
	{
		return texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00033390 File Offset: 0x00031590
	private void UpdateOnline()
	{
		if (this.facebookFriend)
		{
			return;
		}
		if (FriendsController.sharedController.onlineInfo.ContainsKey(this.id))
		{
			string text = FriendsController.sharedController.onlineInfo[this.id]["game_mode"];
			string s = FriendsController.sharedController.onlineInfo[this.id]["delta"];
			string text2 = FriendsController.sharedController.onlineInfo[this.id]["protocol"];
			int num = int.Parse(text);
			if (num > 99)
			{
				num /= 100;
			}
			else
			{
				num = -1;
			}
			int num2;
			if (!int.TryParse(text, out num2))
			{
				num2 = -1;
			}
			else
			{
				if (num2 > 99)
				{
					num2 -= num * 100;
				}
				num2 /= 10;
			}
			int num3;
			bool flag = int.TryParse(s, out num3);
			if (flag)
			{
				if ((float)num3 > FriendsController.onlineDelta || (num != 3 && ((num != (int)ConnectSceneNGUIController.myPlatformConnect && num != -1) || ExpController.GetOurTier() != num2)))
				{
					this.offline.gameObject.SetActive(true);
					this.onlineLab.gameObject.SetActive(false);
					this.playing.gameObject.SetActive(false);
					this.join.GetComponent<UIButton>().isEnabled = false;
				}
				else
				{
					string b = text2;
					string multiplayerProtocolVersion = GlobalGameController.MultiplayerProtocolVersion;
					int num4;
					if (text != null && int.TryParse(text, out num4))
					{
						if (num4 == -1)
						{
							this.offline.gameObject.SetActive(false);
							this.onlineLab.gameObject.SetActive(true);
							this.playing.gameObject.SetActive(false);
							this.join.GetComponent<UIButton>().isEnabled = false;
						}
						else
						{
							this.offline.gameObject.SetActive(false);
							this.onlineLab.gameObject.SetActive(false);
							this.playing.gameObject.SetActive(true);
							this.join.GetComponent<UIButton>().isEnabled = (!this._disableButtons && multiplayerProtocolVersion == b);
							string s2;
							if (FriendsController.sharedController.onlineInfo[this.id].TryGetValue("map", out s2))
							{
								SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(s2));
								if (infoScene == null)
								{
									this.join.GetComponent<UIButton>().isEnabled = false;
								}
							}
						}
					}
				}
			}
		}
		else
		{
			this.offline.gameObject.SetActive(true);
			this.onlineLab.gameObject.SetActive(false);
			this.playing.gameObject.SetActive(false);
			this.join.GetComponent<UIButton>().isEnabled = false;
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00033668 File Offset: 0x00031868
	public void DisableButtons()
	{
		this._disableButtons = true;
		this.delete.SetActive(false);
		if (this.facebookFriend)
		{
			this.addFacebookFriend.SetActive(false);
		}
		this.inactivityStartTm = Time.realtimeSinceStartup;
		this.UpdateOnline();
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000336A8 File Offset: 0x000318A8
	private bool IsFriendInClan()
	{
		Dictionary<string, Dictionary<string, object>> playersInfo = FriendsController.sharedController.playersInfo;
		if (playersInfo == null)
		{
			return false;
		}
		if (!playersInfo.ContainsKey(this.id))
		{
			return false;
		}
		if (!playersInfo[this.id].ContainsKey("player"))
		{
			return false;
		}
		Dictionary<string, object> dictionary = playersInfo[this.id]["player"] as Dictionary<string, object>;
		if (dictionary == null)
		{
			return false;
		}
		if (!dictionary.ContainsKey("clan_creator_id"))
		{
			return false;
		}
		string value = Convert.ToString(dictionary["clan_creator_id"]);
		return !string.IsNullOrEmpty(value);
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00033748 File Offset: 0x00031948
	private void Update()
	{
		if (this.ClanInvite)
		{
			this.addFacebookFriend.SetActive(!FriendsController.sharedController.ClanSentInvites.Contains(this.id) && !FriendsController.sharedController.clanSentInvitesLocal.Contains(this.id) && !FriendsController.sharedController.friendsDeletedLocal.Contains(this.id));
			this.addFacebookFriend.GetComponent<UIButton>().isEnabled = !FriendsController.sharedController.ClanLimitReached;
			this.cancel.SetActive(FriendsController.sharedController.ClanSentInvites.Contains(this.id) && !FriendsController.sharedController.clanCancelledInvitesLocal.Contains(this.id) && !FriendsController.sharedController.friendsDeletedLocal.Contains(this.id));
		}
		if (this.ClanMember)
		{
			bool flag = false;
			foreach (Dictionary<string, string> dictionary in FriendsController.sharedController.clanMembers)
			{
				if (dictionary.ContainsKey("id") && dictionary["id"].Equals(this.id))
				{
					flag = true;
					break;
				}
			}
			this.delete.SetActive(flag && !FriendsController.sharedController.clanDeletedLocal.Contains(this.id) && FriendsController.sharedController.id != null && FriendsController.sharedController.id.Equals(FriendsController.sharedController.clanLeaderID));
		}
		if (Time.realtimeSinceStartup - this.inactivityStartTm > 25f)
		{
			this.inactivityStartTm = float.PositiveInfinity;
			if (!this.isInviteFromUs)
			{
				this._disableButtons = false;
				this.UpdateOnline();
			}
			this.delete.SetActive(!this.facebookFriend && !this.ClanInvite);
		}
		if (Time.realtimeSinceStartup - this.timeLastCheck > 1f)
		{
			this.timeLastCheck = Time.realtimeSinceStartup;
			this.UpdateOnline();
			this.UpdateInfo();
		}
		if (this.facebookFriend && !this._disableButtons)
		{
			bool flag2 = false;
			if (FriendsController.sharedController.friends != null)
			{
				foreach (string text in FriendsController.sharedController.friends)
				{
					if (text.Equals(this.id))
					{
						flag2 = true;
						break;
					}
				}
			}
			this.addFacebookFriend.SetActive(!flag2);
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00033A48 File Offset: 0x00031C48
	private void OnDestroy()
	{
		FriendsGUIController.UpdaeOnlineEvent = (Action)Delegate.Remove(FriendsGUIController.UpdaeOnlineEvent, new Action(this.UpdateOnline));
	}

	// Token: 0x040006F5 RID: 1781
	public UILabel nm;

	// Token: 0x040006F6 RID: 1782
	public UITexture preview;

	// Token: 0x040006F7 RID: 1783
	public Texture2D mySkin;

	// Token: 0x040006F8 RID: 1784
	public UISprite rank;

	// Token: 0x040006F9 RID: 1785
	public bool facebookFriend;

	// Token: 0x040006FA RID: 1786
	public bool ClanMember;

	// Token: 0x040006FB RID: 1787
	public bool ClanInvite;

	// Token: 0x040006FC RID: 1788
	public GameObject avatarButton;

	// Token: 0x040006FD RID: 1789
	public GameObject join;

	// Token: 0x040006FE RID: 1790
	public GameObject delete;

	// Token: 0x040006FF RID: 1791
	public GameObject addFacebookFriend;

	// Token: 0x04000700 RID: 1792
	public GameObject cancel;

	// Token: 0x04000701 RID: 1793
	public GameObject leader;

	// Token: 0x04000702 RID: 1794
	public string id;

	// Token: 0x04000703 RID: 1795
	public bool isInviteFromUs;

	// Token: 0x04000704 RID: 1796
	public bool IsClanLeader;

	// Token: 0x04000705 RID: 1797
	public UITexture ClanLogo;

	// Token: 0x04000706 RID: 1798
	public UILabel clanName;

	// Token: 0x04000707 RID: 1799
	public GameObject onlineStateContainer;

	// Token: 0x04000708 RID: 1800
	public UILabel offline;

	// Token: 0x04000709 RID: 1801
	public UILabel onlineLab;

	// Token: 0x0400070A RID: 1802
	public UILabel playing;

	// Token: 0x0400070B RID: 1803
	private float timeLastCheck;

	// Token: 0x0400070C RID: 1804
	private readonly Lazy<ClansGUIController> _clansGuiController;

	// Token: 0x0400070D RID: 1805
	private float inactivityStartTm;

	// Token: 0x0400070E RID: 1806
	private bool _disableButtons;
}
