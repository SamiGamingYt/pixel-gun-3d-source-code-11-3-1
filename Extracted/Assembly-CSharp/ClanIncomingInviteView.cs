using System;
using System.Collections;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x020005BF RID: 1471
internal sealed class ClanIncomingInviteView : MonoBehaviour
{
	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x060032C9 RID: 13001 RVA: 0x00107528 File Offset: 0x00105728
	// (set) Token: 0x060032CA RID: 13002 RVA: 0x00107530 File Offset: 0x00105730
	public string ClanId { get; set; }

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x060032CB RID: 13003 RVA: 0x0010753C File Offset: 0x0010573C
	// (set) Token: 0x060032CC RID: 13004 RVA: 0x00107544 File Offset: 0x00105744
	public string ClanCreatorId { get; set; }

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x060032CD RID: 13005 RVA: 0x00107550 File Offset: 0x00105750
	// (set) Token: 0x060032CE RID: 13006 RVA: 0x00107564 File Offset: 0x00105764
	public string ClanName
	{
		get
		{
			return this._clanName ?? string.Empty;
		}
		set
		{
			this._clanName = (value ?? string.Empty);
			this.clanName.Do(delegate(UILabel l)
			{
				l.text = this._clanName;
			});
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x060032CF RID: 13007 RVA: 0x00107594 File Offset: 0x00105794
	// (set) Token: 0x060032D0 RID: 13008 RVA: 0x001075A8 File Offset: 0x001057A8
	public string ClanLogo
	{
		get
		{
			return this._clanLogo ?? string.Empty;
		}
		set
		{
			this._clanLogo = (value ?? string.Empty);
			this.clanLogo.Do(delegate(UITexture t)
			{
				LeaderboardScript.SetClanLogo(t, this._clanLogo);
			});
		}
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x001075D8 File Offset: 0x001057D8
	public void HandleAcceptButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Accept invite to clan {0} ({1})", this.ClanName, this.ClanId);
			Debug.Log(message);
		}
		FriendsController.sharedController.Do(delegate(FriendsController f)
		{
			f.StartCoroutine(this.AcceptClanInviteCoroutine());
		});
		ClanIncomingInvitesController.SetRequestDirty();
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x00107628 File Offset: 0x00105828
	public void HandleRejectButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Reject invite to clan {0} ({1})", this.ClanName, this.ClanId);
			Debug.Log(message);
		}
		FriendsController.sharedController.Do(delegate(FriendsController f)
		{
			f.StartCoroutine(this.RejectClanInviteCoroutine());
		});
		ClanIncomingInvitesController.SetRequestDirty();
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x00107678 File Offset: 0x00105878
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x00107680 File Offset: 0x00105880
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x00107688 File Offset: 0x00105888
	internal void Refresh()
	{
		if (this.acceptButton != null && this.rejectButton != null && FriendsController.sharedController != null)
		{
			bool flag = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			Vector3[] array2;
			if (flag)
			{
				Vector3[] array = new Vector3[2];
				array[0] = this.rejectButton.transform.localPosition;
				array2 = array;
				array[1] = this.acceptButton.transform.localPosition;
			}
			else
			{
				Vector3[] array3 = new Vector3[2];
				array3[0] = this.acceptButton.transform.localPosition;
				array2 = array3;
				array3[1] = this.rejectButton.transform.localPosition;
			}
			Vector3[] array4 = array2;
			this.rejectButton.transform.localPosition = array4[0];
			this.acceptButton.transform.localPosition = array4[1];
			this.acceptButton.gameObject.SetActive(flag);
		}
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x001077A8 File Offset: 0x001059A8
	private IEnumerator AcceptClanInviteCoroutine()
	{
		if (FriendsController.sharedController == null)
		{
			yield break;
		}
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "accept_invite");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerId);
		form.AddField("id_clan", this.ClanId ?? string.Empty);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("accept_invite", null));
		this.acceptButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		this.rejectButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		try
		{
			WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (request == null)
			{
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogError(request.error);
				yield break;
			}
			string responseText = URLs.Sanitize(request);
			if ("fail".Equals(responseText, StringComparison.OrdinalIgnoreCase))
			{
				Debug.LogError("accept_invite failed.");
				yield break;
			}
			FriendsController.sharedController.clanLogo = this.ClanLogo;
			FriendsController.sharedController.ClanID = (this.ClanId ?? string.Empty);
			FriendsController.sharedController.clanName = this.ClanName;
			FriendsController.sharedController.clanLeaderID = (this.ClanCreatorId ?? string.Empty);
			if (ClansGUIController.sharedController != null)
			{
				ClansGUIController.sharedController.nameClanLabel.text = FriendsController.sharedController.clanName;
			}
			UIGrid g = base.transform.parent.GetComponent<UIGrid>();
			if (g != null)
			{
				NGUITools.Destroy(base.transform);
				yield return new WaitForEndOfFrame();
				g.Reposition();
				this.acceptButton = null;
				this.rejectButton = null;
				g.GetComponentInParent<ClanIncomingInvitesController>().Do(delegate(ClanIncomingInvitesController c)
				{
					c.Refresh();
				});
			}
			ClanIncomingInviteView[] views = g.GetComponentsInChildren<ClanIncomingInviteView>();
			foreach (ClanIncomingInviteView view in views)
			{
				view.Refresh();
			}
		}
		finally
		{
			this.acceptButton.Do(delegate(UIButton b)
			{
				b.isEnabled = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			});
			this.rejectButton.Do(delegate(UIButton b)
			{
				b.isEnabled = true;
			});
		}
		yield break;
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x001077C4 File Offset: 0x001059C4
	private IEnumerator RejectClanInviteCoroutine()
	{
		string playerId = FriendsController.sharedController.Map((FriendsController sc) => sc.id) ?? string.Empty;
		if (string.IsNullOrEmpty(playerId))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "reject_invite");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerId);
		form.AddField("id_clan", this.ClanId ?? string.Empty);
		form.AddField("id", playerId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("reject_invite", null));
		this.acceptButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		this.rejectButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		try
		{
			WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (request == null)
			{
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogError(request.error);
				yield break;
			}
			string responseText = URLs.Sanitize(request);
			if ("fail".Equals(responseText, StringComparison.OrdinalIgnoreCase))
			{
				Debug.LogError("reject_invite failed.");
				yield break;
			}
			UIGrid g = base.transform.parent.GetComponent<UIGrid>();
			if (g != null)
			{
				NGUITools.Destroy(base.transform);
				yield return new WaitForEndOfFrame();
				g.Reposition();
				this.acceptButton = null;
				this.rejectButton = null;
				g.GetComponentInParent<ClanIncomingInvitesController>().Do(delegate(ClanIncomingInvitesController c)
				{
					c.Refresh();
				});
			}
		}
		finally
		{
			this.acceptButton.Do(delegate(UIButton b)
			{
				b.isEnabled = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			});
			this.rejectButton.Do(delegate(UIButton b)
			{
				b.isEnabled = true;
			});
		}
		yield break;
	}

	// Token: 0x04002556 RID: 9558
	public UIButton acceptButton;

	// Token: 0x04002557 RID: 9559
	public UIButton rejectButton;

	// Token: 0x04002558 RID: 9560
	public UITexture clanLogo;

	// Token: 0x04002559 RID: 9561
	public UILabel clanName;

	// Token: 0x0400255A RID: 9562
	private string _clanName = string.Empty;

	// Token: 0x0400255B RID: 9563
	private string _clanLogo = string.Empty;
}
