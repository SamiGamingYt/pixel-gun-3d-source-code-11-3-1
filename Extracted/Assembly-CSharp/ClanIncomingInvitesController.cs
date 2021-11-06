using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
internal sealed class ClanIncomingInvitesController : MonoBehaviour
{
	// Token: 0x060032DD RID: 13021 RVA: 0x00107828 File Offset: 0x00105A28
	public void HandleInboxPressed()
	{
		ClansGUIController.State previousState = this.clansGui.Map((ClansGUIController c) => c.CurrentState);
		this.inboxPanel.Do(delegate(GameObject i)
		{
			i.SetActive(true);
		});
		this.clanPanel.Do(delegate(GameObject i)
		{
			i.SetActive(false);
		});
		this.noClanPanel.Do(delegate(GameObject i)
		{
			i.SetActive(false);
		});
		this.clansGui.Do(delegate(ClansGUIController c)
		{
			c.CurrentState = ClansGUIController.State.Inbox;
		});
		base.StartCoroutine(this.RepositionAfterPause());
		this._back = delegate()
		{
			bool inClan = !string.IsNullOrEmpty(FriendsController.sharedController.Map((FriendsController f) => f.ClanID));
			this.inboxPanel.Do(delegate(GameObject i)
			{
				i.SetActive(false);
			});
			this.clanPanel.Do(delegate(GameObject i)
			{
				i.SetActive(inClan);
			});
			this.noClanPanel.Do(delegate(GameObject i)
			{
				i.SetActive(!inClan);
			});
			this.clansGui.Do(delegate(ClansGUIController c)
			{
				c.CurrentState = previousState;
			});
		};
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x00107934 File Offset: 0x00105B34
	public void HandleBackFromInboxPressed()
	{
		if (this._back != null)
		{
			this._back();
		}
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x0010794C File Offset: 0x00105B4C
	internal static void FetchClanIncomingInvites(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("Friends controller is null.");
			return;
		}
		ClanIncomingInvitesController._cts = new CancellationTokenSource();
		ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, ClanIncomingInvitesController._cts.Token);
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x001079B0 File Offset: 0x00105BB0
	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId, float delay, CancellationToken ct)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			throw new InvalidOperationException("FriendsController instance should not be null.");
		}
		TaskCompletionSource<List<object>> taskCompletionSource = new TaskCompletionSource<List<object>>();
		FriendsController.sharedController.StartCoroutine(ClanIncomingInvitesController.RequestClanIncomingInvitesCoroutine(playerId, delay, taskCompletionSource, ct));
		return taskCompletionSource.Task;
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x00107A14 File Offset: 0x00105C14
	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId, CancellationToken ct)
	{
		return ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 0f, ct);
	}

	// Token: 0x060032E2 RID: 13026 RVA: 0x00107A24 File Offset: 0x00105C24
	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId)
	{
		return ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 0f, CancellationToken.None);
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x00107A38 File Offset: 0x00105C38
	internal void Refresh()
	{
		if (this.clanIncomingInvitesGrid != null && this.noClanIncomingInvitesLabel != null)
		{
			bool flag = this.clanIncomingInvitesGrid.transform.childCount == 0;
			GameObject gameObject = this.clansGui.Map((ClansGUIController c) => c.receivingPlashka);
			bool active = (!(gameObject == null)) ? (flag && !gameObject.activeInHierarchy) : flag;
			this.noClanIncomingInvitesLabel.gameObject.SetActive(active);
		}
		if (this.cannotAcceptClanIncomingInvitesLabel != null)
		{
			bool flag2 = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			bool active2 = (!(this.noClanIncomingInvitesLabel == null)) ? (!flag2 && !this.noClanIncomingInvitesLabel.activeInHierarchy) : (!flag2);
			this.cannotAcceptClanIncomingInvitesLabel.SetActive(active2);
		}
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x00107B3C File Offset: 0x00105D3C
	private static IEnumerator RequestClanIncomingInvitesCoroutine(string playerId, float delay, TaskCompletionSource<List<object>> promise, CancellationToken ct)
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		if (ct.IsCancellationRequested)
		{
			promise.TrySetCanceled();
			yield break;
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			Debug.LogWarning("Current player id is empty.");
			promise.TrySetException(new InvalidOperationException("Current player id is empty."));
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_clan_incoming_invites");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_clan_incoming_invites", null));
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
		if (request == null)
		{
			promise.TrySetException(new InvalidOperationException("Request was not performed because player is connected to Photon."));
			ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 10f, ct);
			yield break;
		}
		while (!request.isDone)
		{
			yield return null;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogError(request.error);
			promise.TrySetException(new InvalidOperationException(request.error));
			ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 10f, ct);
			yield break;
		}
		string responseText = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("Clan incoming invites response is empty.");
			promise.TrySetException(new InvalidOperationException("Clan incoming invites response is empty."));
			ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 10f, ct);
			yield break;
		}
		Dictionary<string, object> d = Json.Deserialize(responseText) as Dictionary<string, object>;
		object invites;
		if (d != null && d.TryGetValue("clans_invites", out invites))
		{
			List<object> result = invites as List<object>;
			if (invites == null)
			{
				promise.TrySetException(new InvalidOperationException("“clans_invites” could not be parsed."));
			}
			else
			{
				promise.TrySetResult(result);
			}
		}
		else
		{
			promise.TrySetException(new InvalidOperationException("“clans_invites” not found."));
		}
		yield break;
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x060032E5 RID: 13029 RVA: 0x00107B94 File Offset: 0x00105D94
	internal static Task<List<object>> CurrentRequest
	{
		get
		{
			return ClanIncomingInvitesController._currentRequest;
		}
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x00107B9C File Offset: 0x00105D9C
	internal static void SetRequestDirty()
	{
		if (ClanIncomingInvitesController._currentRequest == null)
		{
			return;
		}
		if (!ClanIncomingInvitesController._currentRequest.IsCompleted)
		{
			ClanIncomingInvitesController._cts.Do(delegate(CancellationTokenSource c)
			{
				c.Cancel();
			});
		}
		ClanIncomingInvitesController._cts = new CancellationTokenSource();
		ClanIncomingInvitesController._currentRequest = null;
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x00107BFC File Offset: 0x00105DFC
	private IEnumerator Start()
	{
		this.Refresh();
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("Friends controller is null.");
			yield break;
		}
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			Debug.LogError("Player id should not be null.");
			yield break;
		}
		if (ClanIncomingInvitesController.CurrentRequest == null)
		{
			ClanIncomingInvitesController._cts = new CancellationTokenSource();
			ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, ClanIncomingInvitesController._cts.Token);
		}
		else if (ClanIncomingInvitesController.CurrentRequest.IsCompleted && (ClanIncomingInvitesController.CurrentRequest.IsCanceled || ClanIncomingInvitesController.CurrentRequest.IsFaulted))
		{
			ClanIncomingInvitesController._cts = new CancellationTokenSource();
			ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, ClanIncomingInvitesController._cts.Token);
		}
		if (!ClanIncomingInvitesController.CurrentRequest.IsCompleted)
		{
		}
		while (!ClanIncomingInvitesController.CurrentRequest.IsCompleted)
		{
			yield return null;
		}
		if (ClanIncomingInvitesController.CurrentRequest.IsCanceled)
		{
			Debug.LogWarning("Request is cancelled.");
		}
		else if (ClanIncomingInvitesController.CurrentRequest.IsFaulted)
		{
			Debug.LogException(ClanIncomingInvitesController.CurrentRequest.Exception);
		}
		else if (this.clanIncomingInviteViewPrototype != null && this.clanIncomingInvitesGrid != null)
		{
			List<object> inviteList = ClanIncomingInvitesController.CurrentRequest.Result;
			if (inviteList != null)
			{
				List<Dictionary<string, object>> invites = inviteList.OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
				this.clanIncomingInviteViewPrototype.gameObject.SetActive(invites.Count > 0);
				foreach (Dictionary<string, object> invite in invites)
				{
					GameObject newItem = NGUITools.AddChild(this.clanIncomingInvitesGrid.gameObject, this.clanIncomingInviteViewPrototype.gameObject);
					ClanIncomingInviteView c = newItem.GetComponent<ClanIncomingInviteView>();
					if (c != null)
					{
						object clanId;
						if (invite.TryGetValue("id", out clanId))
						{
							c.ClanId = Convert.ToString(clanId);
						}
						else
						{
							c.ClanId = string.Empty;
						}
						object clanName;
						if (invite.TryGetValue("name", out clanName))
						{
							c.ClanName = Convert.ToString(clanName);
						}
						else
						{
							c.ClanName = string.Empty;
						}
						object clanLogo;
						if (invite.TryGetValue("logo", out clanLogo))
						{
							c.ClanLogo = Convert.ToString(clanLogo);
						}
						else
						{
							c.ClanLogo = string.Empty;
						}
						object clanCreatorId;
						if (invite.TryGetValue("creator_id", out clanCreatorId))
						{
							c.ClanCreatorId = Convert.ToString(clanLogo);
						}
						else
						{
							c.ClanCreatorId = string.Empty;
						}
					}
				}
				this.clanIncomingInvitesGrid.transform.parent.GetComponent<UIScrollView>().Do(delegate(UIScrollView s)
				{
					s.disableDragIfFits = true;
				});
			}
			this.clanIncomingInviteViewPrototype.gameObject.SetActive(false);
			yield return new WaitForEndOfFrame();
			this.clanIncomingInvitesGrid.Reposition();
		}
		this.Refresh();
		yield break;
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x00107C18 File Offset: 0x00105E18
	private IEnumerator RepositionAfterPause()
	{
		yield return new WaitForEndOfFrame();
		this.clanIncomingInvitesGrid.Do(delegate(UIGrid g)
		{
			g.Reposition();
		});
		this.Refresh();
		yield break;
	}

	// Token: 0x0400255E RID: 9566
	public ClansGUIController clansGui;

	// Token: 0x0400255F RID: 9567
	public UIGrid clanIncomingInvitesGrid;

	// Token: 0x04002560 RID: 9568
	public ClanIncomingInviteView clanIncomingInviteViewPrototype;

	// Token: 0x04002561 RID: 9569
	public GameObject clanPanel;

	// Token: 0x04002562 RID: 9570
	public GameObject noClanPanel;

	// Token: 0x04002563 RID: 9571
	public GameObject inboxPanel;

	// Token: 0x04002564 RID: 9572
	public GameObject noClanIncomingInvitesLabel;

	// Token: 0x04002565 RID: 9573
	public GameObject cannotAcceptClanIncomingInvitesLabel;

	// Token: 0x04002566 RID: 9574
	private Action _back;

	// Token: 0x04002567 RID: 9575
	private static Task<List<object>> _currentRequest;

	// Token: 0x04002568 RID: 9576
	private static CancellationTokenSource _cts;
}
