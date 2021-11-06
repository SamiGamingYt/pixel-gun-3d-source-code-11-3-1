using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x0200068A RID: 1674
public sealed class LeaderboardsView : MonoBehaviour
{
	// Token: 0x06003A87 RID: 14983 RVA: 0x0012E8C8 File Offset: 0x0012CAC8
	public LeaderboardsView()
	{
		this._leaderboardsPanel = new Lazy<UIPanel>(new Func<UIPanel>(base.GetComponent<UIPanel>));
	}

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06003A88 RID: 14984 RVA: 0x0012E900 File Offset: 0x0012CB00
	// (remove) Token: 0x06003A89 RID: 14985 RVA: 0x0012E91C File Offset: 0x0012CB1C
	public event Action<LeaderboardsView.State, LeaderboardsView.State> OnStateChanged;

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06003A8A RID: 14986 RVA: 0x0012E938 File Offset: 0x0012CB38
	// (set) Token: 0x06003A8B RID: 14987 RVA: 0x0012E940 File Offset: 0x0012CB40
	public bool InTournamentTop { get; set; }

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x06003A8C RID: 14988 RVA: 0x0012E94C File Offset: 0x0012CB4C
	// (set) Token: 0x06003A8D RID: 14989 RVA: 0x0012E954 File Offset: 0x0012CB54
	public bool CanShowClanTableFooter { get; set; }

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06003A8E RID: 14990 RVA: 0x0012E960 File Offset: 0x0012CB60
	private bool IsTournamentMember
	{
		get
		{
			return RatingSystem.instance != null && RatingSystem.instance.currentLeague >= RatingSystem.RatingLeague.Adamant;
		}
	}

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06003A8F RID: 14991 RVA: 0x0012E988 File Offset: 0x0012CB88
	// (set) Token: 0x06003A90 RID: 14992 RVA: 0x0012E990 File Offset: 0x0012CB90
	public LeaderboardsView.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			PlayerPrefs.SetInt("Leaderboards.TabCache", (int)value);
			if (this.clansButton != null)
			{
				this.clansButton.isEnabled = (value != LeaderboardsView.State.Clans);
				Transform transform = this.clansButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform != null)
				{
					transform.gameObject.SetActive(value != LeaderboardsView.State.Clans);
				}
				Transform transform2 = this.clansButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value == LeaderboardsView.State.Clans);
				}
				this.clansFooter.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == LeaderboardsView.State.Clans);
				});
				this.clansTableFooter.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == LeaderboardsView.State.Clans && this.CanShowClanTableFooter);
				});
			}
			if (this.friendsButton != null)
			{
				this.friendsButton.isEnabled = (value != LeaderboardsView.State.Friends);
				Transform transform3 = this.friendsButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(value != LeaderboardsView.State.Friends);
				}
				Transform transform4 = this.friendsButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(value == LeaderboardsView.State.Friends);
				}
			}
			if (this.bestPlayersButton != null)
			{
				this.bestPlayersButton.isEnabled = (value != LeaderboardsView.State.BestPlayers);
				Transform transform5 = this.bestPlayersButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(value != LeaderboardsView.State.BestPlayers);
				}
				Transform transform6 = this.bestPlayersButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(value == LeaderboardsView.State.BestPlayers);
				}
				this.leaderboardFooter.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == LeaderboardsView.State.BestPlayers);
				});
				this.leaderboardHeader.Do(delegate(GameObject g)
				{
					g.SetActiveSafe(value == LeaderboardsView.State.BestPlayers);
				});
			}
			if (this.tournamentButton != null)
			{
				this.tournamentButton.isEnabled = (value != LeaderboardsView.State.Tournament);
				Transform transform7 = this.tournamentButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(value != LeaderboardsView.State.Tournament);
				}
				Transform transform8 = this.tournamentButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform8 != null)
				{
					transform8.gameObject.SetActive(value == LeaderboardsView.State.Tournament);
				}
			}
			if (this.clansTableHeader != null)
			{
				bool active = value == LeaderboardsView.State.Clans;
				this.clansTableHeader.SetActive(active);
			}
			this.bestPlayersGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition = g.transform.localPosition;
				localPosition.x = ((value != LeaderboardsView.State.BestPlayers) ? 9000f : 0f);
				g.gameObject.transform.localPosition = localPosition;
			});
			this.friendsGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition = g.transform.localPosition;
				localPosition.x = ((value != LeaderboardsView.State.Friends) ? 9000f : 0f);
				g.gameObject.transform.localPosition = localPosition;
			});
			this.clansGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition = g.transform.localPosition;
				localPosition.x = ((value != LeaderboardsView.State.Clans) ? 9000f : 0f);
				g.gameObject.transform.localPosition = localPosition;
			});
			this.tournamentJoinInfo.Do(delegate(GameObject o)
			{
				o.SetActiveSafe(value == LeaderboardsView.State.Tournament && !this.IsTournamentMember);
			});
			bool canShowTournamentGrid = value == LeaderboardsView.State.Tournament && this.IsTournamentMember;
			this.tournamentFooter.Do(delegate(GameObject g)
			{
				g.SetActiveSafe(canShowTournamentGrid);
			});
			this.tournamentHeader.Do(delegate(GameObject g)
			{
				g.SetActiveSafe(canShowTournamentGrid);
			});
			this.tournamentTableFooter.Do(delegate(GameObject g)
			{
				g.SetActiveSafe(canShowTournamentGrid && this.InTournamentTop);
			});
			this.tournamentGrid.Do(delegate(UIWrapContent g)
			{
				Vector3 localPosition = g.transform.localPosition;
				localPosition.x = ((!canShowTournamentGrid) ? 9000f : 0f);
				g.gameObject.transform.localPosition = localPosition;
			});
			this.UpdateScrollSize(this.tournamentGrid.gameObject, this.tournamentTableFooter);
			this.UpdateScrollSize(this.clansGrid.gameObject, this.clansTableFooter);
			this.bestPlayersTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == LeaderboardsView.State.BestPlayers);
			});
			this.clansTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == LeaderboardsView.State.Clans);
			});
			this.friendsTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == LeaderboardsView.State.Friends);
			});
			this.tournamentTableHeader.Do(delegate(GameObject o)
			{
				o.SetActive(value == LeaderboardsView.State.Tournament && !this.tournamentJoinInfo.activeInHierarchy);
			});
			LeaderboardsView.State currentState = this._currentState;
			this._currentState = value;
			if (this.OnStateChanged != null)
			{
				this.OnStateChanged(currentState, this._currentState);
			}
		}
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0012EE9C File Offset: 0x0012D09C
	public void SetOverallTopFooterActive()
	{
		this._overallTopFooterActive = true;
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x0012EEA8 File Offset: 0x0012D0A8
	public void SetLeagueTopFooterActive()
	{
		this._leagueTopFooterActive = true;
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x0012EEB4 File Offset: 0x0012D0B4
	private void RefreshGrid(UIGrid grid)
	{
		grid.Reposition();
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x0012EEBC File Offset: 0x0012D0BC
	private IEnumerator SkipFrameAndExecuteCoroutine(Action a)
	{
		if (a == null)
		{
			yield break;
		}
		yield return new WaitForEndOfFrame();
		a();
		yield break;
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x0012EEE0 File Offset: 0x0012D0E0
	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (this.clansButton != null && gameObject == this.clansButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.Clans;
		}
		else if (this.friendsButton != null && gameObject == this.friendsButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.Friends;
		}
		else if (this.bestPlayersButton != null && gameObject == this.bestPlayersButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.BestPlayers;
		}
		else if (this.tournamentButton != null && gameObject == this.tournamentButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.Tournament;
		}
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x0012EFC0 File Offset: 0x0012D1C0
	private IEnumerator UpdateGridsAndScrollers()
	{
		this._prepared = false;
		yield return new WaitForEndOfFrame();
		IEnumerable<UIWrapContent> wraps = from g in new UIWrapContent[]
		{
			this.friendsGrid,
			this.clansGrid,
			this.tournamentGrid
		}
		where g != null
		select g;
		foreach (UIWrapContent w in wraps)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		}
		yield return null;
		IEnumerable<UIScrollView> scrolls = from s in new UIScrollView[]
		{
			this.clansScroll,
			this.friendsScroll,
			this.LeagueScroll
		}
		where s != null
		select s;
		foreach (UIScrollView s2 in scrolls)
		{
			s2.ResetPosition();
			s2.UpdatePosition();
		}
		this._prepared = true;
		yield break;
	}

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x06003A97 RID: 14999 RVA: 0x0012EFDC File Offset: 0x0012D1DC
	internal bool Prepared
	{
		get
		{
			return this._prepared;
		}
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x0012EFE4 File Offset: 0x0012D1E4
	private void OnEnable()
	{
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			LeaderboardScript.Instance.ReturnBack(this, null);
		}, "Leaderboards");
		base.StartCoroutine(this.UpdateGridsAndScrollers());
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x0012F020 File Offset: 0x0012D220
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._prepared = false;
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x0012F040 File Offset: 0x0012D240
	private void UpdateScrollSize(GameObject scrollChildObj, GameObject widgetObject)
	{
		UIPanel componentInParent = scrollChildObj.GetComponentInParent<UIPanel>();
		if (widgetObject == null)
		{
			return;
		}
		UIWidget component = widgetObject.GetComponent<UIWidget>();
		if (componentInParent == null || component == null)
		{
			return;
		}
		int num = (!this._scrollsDefHeights.Keys.Contains(scrollChildObj)) ? -1 : this._scrollsDefHeights[scrollChildObj];
		if (num < 0)
		{
			return;
		}
		componentInParent.bottomAnchor.absolute = ((!widgetObject.activeInHierarchy) ? num : (num + component.height));
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x0012F0D8 File Offset: 0x0012D2D8
	private void Awake()
	{
		List<UIWrapContent> list = (from g in new UIWrapContent[]
		{
			this.friendsGrid,
			this.bestPlayersGrid,
			this.clansGrid,
			this.tournamentGrid
		}
		where g != null
		select g).ToList<UIWrapContent>();
		foreach (UIWrapContent uiwrapContent in list)
		{
			uiwrapContent.gameObject.SetActive(true);
			Vector3 localPosition = uiwrapContent.transform.localPosition;
			localPosition.x = 9000f;
			uiwrapContent.gameObject.transform.localPosition = localPosition;
		}
		UIPanel componentInParent = this.clansGrid.gameObject.GetComponentInParent<UIPanel>();
		this._scrollsDefHeights.Add(this.clansGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = this.friendsGrid.gameObject.GetComponentInParent<UIPanel>();
		this._scrollsDefHeights.Add(this.friendsGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = this.bestPlayersGrid.gameObject.GetComponentInParent<UIPanel>();
		this._scrollsDefHeights.Add(this.bestPlayersGrid.gameObject, componentInParent.bottomAnchor.absolute);
		componentInParent = this.tournamentGrid.gameObject.GetComponentInParent<UIPanel>();
		this._scrollsDefHeights.Add(this.tournamentGrid.gameObject, componentInParent.bottomAnchor.absolute);
	}

	// Token: 0x06003A9C RID: 15004 RVA: 0x0012F28C File Offset: 0x0012D48C
	private IEnumerator Start()
	{
		IEnumerable<UIButton> buttons = from b in new UIButton[]
		{
			this.clansButton,
			this.friendsButton,
			this.bestPlayersButton,
			this.tournamentButton
		}
		where b != null
		select b;
		foreach (UIButton b2 in buttons)
		{
			ButtonHandler bh = b2.GetComponent<ButtonHandler>();
			if (bh != null)
			{
				bh.Clicked += this.HandleTabPressed;
			}
		}
		IEnumerable<UIScrollView> scrollViews = from s in new UIScrollView[]
		{
			this.clansScroll,
			this.friendsScroll,
			this.bestPlayersScroll,
			this.LeagueScroll
		}
		where s != null
		select s;
		foreach (UIScrollView scrollView in scrollViews)
		{
			scrollView.ResetPosition();
		}
		yield return null;
		this.friendsGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.bestPlayersGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.clansGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.tournamentGrid.Do(delegate(UIWrapContent w)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		yield return null;
		int stateInt = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
		LeaderboardsView.State state = (LeaderboardsView.State)((!Enum.IsDefined(typeof(LeaderboardsView.State), stateInt)) ? 3 : stateInt);
		this.CurrentState = ((state == LeaderboardsView.State.None) ? LeaderboardsView.State.BestPlayers : state);
		yield break;
	}

	// Token: 0x04002B0F RID: 11023
	public const float FarAwayX = 9000f;

	// Token: 0x04002B10 RID: 11024
	internal const string LeaderboardsTabCache = "Leaderboards.TabCache";

	// Token: 0x04002B11 RID: 11025
	[SerializeField]
	public UIWrapContent clansGrid;

	// Token: 0x04002B12 RID: 11026
	[SerializeField]
	public UIWrapContent friendsGrid;

	// Token: 0x04002B13 RID: 11027
	[SerializeField]
	public UIWrapContent bestPlayersGrid;

	// Token: 0x04002B14 RID: 11028
	[SerializeField]
	public UIWrapContent tournamentGrid;

	// Token: 0x04002B15 RID: 11029
	[SerializeField]
	public ButtonHandler backButton;

	// Token: 0x04002B16 RID: 11030
	[SerializeField]
	private UIButton clansButton;

	// Token: 0x04002B17 RID: 11031
	[SerializeField]
	private UIButton friendsButton;

	// Token: 0x04002B18 RID: 11032
	[SerializeField]
	private UIButton bestPlayersButton;

	// Token: 0x04002B19 RID: 11033
	[SerializeField]
	private UIButton tournamentButton;

	// Token: 0x04002B1A RID: 11034
	[SerializeField]
	private UIScrollView clansScroll;

	// Token: 0x04002B1B RID: 11035
	[SerializeField]
	private UIScrollView friendsScroll;

	// Token: 0x04002B1C RID: 11036
	[SerializeField]
	private UIScrollView bestPlayersScroll;

	// Token: 0x04002B1D RID: 11037
	[SerializeField]
	private UIScrollView tournamentScroll;

	// Token: 0x04002B1E RID: 11038
	[SerializeField]
	private UIScrollView LeagueScroll;

	// Token: 0x04002B1F RID: 11039
	[SerializeField]
	private GameObject friendsTableHeader;

	// Token: 0x04002B20 RID: 11040
	[SerializeField]
	private GameObject bestPlayersTableHeader;

	// Token: 0x04002B21 RID: 11041
	[SerializeField]
	private GameObject clansTableHeader;

	// Token: 0x04002B22 RID: 11042
	[SerializeField]
	private GameObject tournamentTableHeader;

	// Token: 0x04002B23 RID: 11043
	[SerializeField]
	public GameObject leaderboardHeader;

	// Token: 0x04002B24 RID: 11044
	[SerializeField]
	public GameObject leaderboardFooter;

	// Token: 0x04002B25 RID: 11045
	[SerializeField]
	public GameObject tournamentHeader;

	// Token: 0x04002B26 RID: 11046
	[SerializeField]
	public GameObject tournamentFooter;

	// Token: 0x04002B27 RID: 11047
	[SerializeField]
	public GameObject tournamentTableFooter;

	// Token: 0x04002B28 RID: 11048
	[SerializeField]
	public GameObject clansTableFooter;

	// Token: 0x04002B29 RID: 11049
	[SerializeField]
	public GameObject clansFooter;

	// Token: 0x04002B2A RID: 11050
	[SerializeField]
	public GameObject tournamentJoinInfo;

	// Token: 0x04002B2B RID: 11051
	[SerializeField]
	public UILabel expirationLabel;

	// Token: 0x04002B2C RID: 11052
	[SerializeField]
	public GameObject expirationIconObj;

	// Token: 0x04002B2D RID: 11053
	[SerializeField]
	public GameObject touchBlocker;

	// Token: 0x04002B2E RID: 11054
	private IDisposable _backSubscription;

	// Token: 0x04002B2F RID: 11055
	private bool _overallTopFooterActive;

	// Token: 0x04002B30 RID: 11056
	private bool _leagueTopFooterActive;

	// Token: 0x04002B31 RID: 11057
	private readonly Lazy<UIPanel> _leaderboardsPanel;

	// Token: 0x04002B32 RID: 11058
	private bool _prepared;

	// Token: 0x04002B33 RID: 11059
	private Dictionary<GameObject, int> _scrollsDefHeights = new Dictionary<GameObject, int>();

	// Token: 0x04002B34 RID: 11060
	private LeaderboardsView.State _currentState;

	// Token: 0x0200068B RID: 1675
	public enum State
	{
		// Token: 0x04002B3A RID: 11066
		None,
		// Token: 0x04002B3B RID: 11067
		Clans,
		// Token: 0x04002B3C RID: 11068
		Friends,
		// Token: 0x04002B3D RID: 11069
		BestPlayers,
		// Token: 0x04002B3E RID: 11070
		Tournament,
		// Token: 0x04002B3F RID: 11071
		Default = 3
	}
}
