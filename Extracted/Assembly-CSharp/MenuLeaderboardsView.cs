using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200068D RID: 1677
internal sealed class MenuLeaderboardsView : MonoBehaviour
{
	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06003AAF RID: 15023 RVA: 0x0012F5F4 File Offset: 0x0012D7F4
	// (set) Token: 0x06003AB0 RID: 15024 RVA: 0x0012F5FC File Offset: 0x0012D7FC
	public MenuLeaderboardsView.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			this.friendsButton.isEnabled = (value != MenuLeaderboardsView.State.Friends);
			Transform transform = this.friendsButton.transform.FindChild("IdleLabel");
			Transform transform2 = this.friendsButton.transform.FindChild("ActiveLabel");
			if (transform != null && transform2)
			{
				transform.gameObject.SetActive(value != MenuLeaderboardsView.State.Friends);
				transform2.gameObject.SetActive(value == MenuLeaderboardsView.State.Friends);
			}
			this.bestPlayersButton.isEnabled = (value != MenuLeaderboardsView.State.BestPlayers);
			Transform transform3 = this.bestPlayersButton.transform.FindChild("IdleLabel");
			Transform transform4 = this.bestPlayersButton.transform.FindChild("ActiveLabel");
			if (transform3 != null && transform4)
			{
				transform3.gameObject.SetActive(value != MenuLeaderboardsView.State.BestPlayers);
				transform4.gameObject.SetActive(value == MenuLeaderboardsView.State.BestPlayers);
			}
			this.clansButton.isEnabled = (value != MenuLeaderboardsView.State.Clans);
			Transform transform5 = this.clansButton.transform.FindChild("IdleLabel");
			Transform transform6 = this.clansButton.transform.FindChild("ActiveLabel");
			if (transform5 != null && transform6)
			{
				transform5.gameObject.SetActive(value != MenuLeaderboardsView.State.Clans);
				transform6.gameObject.SetActive(value == MenuLeaderboardsView.State.Clans);
			}
			if (this.nickOrClanName != null)
			{
				this.nickOrClanName.text = ((value != MenuLeaderboardsView.State.Clans) ? LocalizationStore.Get("Key_0071") : LocalizationStore.Get("Key_0257"));
			}
			this.friendsPanel.transform.localPosition = ((value != MenuLeaderboardsView.State.Friends) ? this._outOfScreenPosition : this._desiredPosition);
			this.bestPlayersPanel.transform.localPosition = ((value != MenuLeaderboardsView.State.BestPlayers) ? this._outOfScreenPosition : this._desiredPosition);
			this.clansPanel.transform.localPosition = ((value != MenuLeaderboardsView.State.Clans) ? this._outOfScreenPosition : this._desiredPosition);
			this._currentState = value;
		}
	}

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x0012F82C File Offset: 0x0012DA2C
	public static bool IsNeedShow
	{
		get
		{
			bool hasFriends = FriendsController.HasFriends;
			return false;
		}
	}

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x0012F840 File Offset: 0x0012DA40
	public static int PageSize
	{
		get
		{
			return 9;
		}
	}

	// Token: 0x170009A2 RID: 2466
	// (set) Token: 0x06003AB3 RID: 15027 RVA: 0x0012F844 File Offset: 0x0012DA44
	public IList<LeaderboardItemViewModel> FriendsList
	{
		set
		{
			base.StartCoroutine(MenuLeaderboardsView.SetGrid(this.friendsGrid, value, this.temporaryBackground));
		}
	}

	// Token: 0x170009A3 RID: 2467
	// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x0012F860 File Offset: 0x0012DA60
	public IList<LeaderboardItemViewModel> BestPlayersList
	{
		set
		{
			base.StartCoroutine(MenuLeaderboardsView.SetGrid(this.bestPlayersGrid, value, this.temporaryBackground));
			if (this.bestPlayersDefaultSprite != null)
			{
				this.bestPlayersDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(this.bestPlayersDefaultSprite);
				this.bestPlayersDefaultSprite = null;
			}
		}
	}

	// Token: 0x170009A4 RID: 2468
	// (set) Token: 0x06003AB5 RID: 15029 RVA: 0x0012F8BC File Offset: 0x0012DABC
	public IList<LeaderboardItemViewModel> ClansList
	{
		set
		{
			base.StartCoroutine(MenuLeaderboardsView.SetGrid(this.clansGrid, value, this.temporaryBackground));
			if (this.clansDefaultSprite != null)
			{
				this.clansDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(this.clansDefaultSprite);
				this.clansDefaultSprite = null;
			}
		}
	}

	// Token: 0x170009A5 RID: 2469
	// (set) Token: 0x06003AB6 RID: 15030 RVA: 0x0012F918 File Offset: 0x0012DB18
	public LeaderboardItemViewModel SelfStats
	{
		set
		{
			this.footer.Reset(value);
			this.footer.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	// Token: 0x170009A6 RID: 2470
	// (set) Token: 0x06003AB7 RID: 15031 RVA: 0x0012F94C File Offset: 0x0012DB4C
	public LeaderboardItemViewModel SelfClanStats
	{
		set
		{
			this.clanFooter.Reset(value);
			this.clanFooter.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x0012F980 File Offset: 0x0012DB80
	private void OnEnable()
	{
		base.StartCoroutine(this.UpdateGridsAndScrollers());
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x0012F990 File Offset: 0x0012DB90
	private void Awake()
	{
		this.footer.gameObject.SetActive(false);
		this.clanFooter.gameObject.SetActive(false);
		if (this.bestPlayersDefaultSprite != null)
		{
			this.bestPlayersDefaultSprite.gameObject.SetActive(true);
		}
		if (this.clansDefaultSprite != null)
		{
			this.clansDefaultSprite.gameObject.SetActive(true);
		}
		this.temporaryBackground.gameObject.SetActive(false);
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x0012FA14 File Offset: 0x0012DC14
	private void Start()
	{
		this._desiredPosition = this.friendsPanel.transform.localPosition;
		this._outOfScreenPosition = new Vector3(9000f, this._desiredPosition.y, this._desiredPosition.z);
		IEnumerable<UIButton> enumerable = from b in new UIButton[]
		{
			this.friendsButton,
			this.bestPlayersButton,
			this.clansButton
		}
		where b != null
		select b;
		foreach (UIButton uibutton in enumerable)
		{
			ButtonHandler component = uibutton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandleTabPressed;
			}
		}
		IEnumerable<UIScrollView> enumerable2 = from s in new UIScrollView[]
		{
			this.friendsScroll,
			this.bestPlayersScroll,
			this.clansScroll
		}
		where s != null
		select s;
		foreach (UIScrollView uiscrollView in enumerable2)
		{
			uiscrollView.ResetPosition();
		}
		this.CurrentState = MenuLeaderboardsView.State.BestPlayers;
		bool isNeedShow = MenuLeaderboardsView.IsNeedShow;
		this.Show(isNeedShow, false);
		this.btnLeaderboards.IsChecked = false;
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x0012FBCC File Offset: 0x0012DDCC
	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (gameObject == this.friendsButton.gameObject)
		{
			this.CurrentState = MenuLeaderboardsView.State.Friends;
		}
		else if (gameObject == this.bestPlayersButton.gameObject)
		{
			this.CurrentState = MenuLeaderboardsView.State.BestPlayers;
		}
		else if (gameObject == this.clansButton.gameObject)
		{
			this.CurrentState = MenuLeaderboardsView.State.Clans;
		}
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x0012FC48 File Offset: 0x0012DE48
	private static IEnumerator SetGrid(UIGrid grid, IList<LeaderboardItemViewModel> value, UISprite temporaryBackground)
	{
		temporaryBackground.gameObject.SetActive(true);
		try
		{
			if (grid == null)
			{
				yield break;
			}
			while (!grid.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			IEnumerable<LeaderboardItemViewModel> enumerable2;
			if (value == null)
			{
				IEnumerable<LeaderboardItemViewModel> enumerable = new List<LeaderboardItemViewModel>();
				enumerable2 = enumerable;
			}
			else
			{
				enumerable2 = from it in value
				where it != null
				select it;
			}
			IEnumerable<LeaderboardItemViewModel> filteredList = enumerable2;
			List<Transform> list = grid.GetChildList();
			for (int i = 0; i != list.Count; i++)
			{
				UnityEngine.Object.Destroy(list[i].gameObject);
			}
			list.Clear();
			grid.Reposition();
			foreach (LeaderboardItemViewModel item in filteredList)
			{
				GameObject o = (!item.Highlight) ? (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardItem")) as GameObject) : (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardSelectedItem")) as GameObject);
				if (o != null)
				{
					LeaderboardItemView liv = o.GetComponent<LeaderboardItemView>();
					if (liv != null)
					{
						liv.Reset(item);
						o.transform.parent = grid.transform;
						grid.AddChild(o.transform);
						o.transform.localScale = Vector3.one;
					}
				}
			}
			grid.Reposition();
			UIScrollView scrollView = grid.transform.parent.gameObject.GetComponent<UIScrollView>();
			if (scrollView != null)
			{
				scrollView.enabled = true;
				yield return null;
				scrollView.ResetPosition();
				scrollView.UpdatePosition();
				yield return null;
				scrollView.enabled = (value.Count >= 10);
			}
		}
		finally
		{
			temporaryBackground.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x0012FC88 File Offset: 0x0012DE88
	private IEnumerator UpdateGridsAndScrollers()
	{
		IEnumerable<UIGrid> grids = from g in new UIGrid[]
		{
			this.friendsGrid,
			this.bestPlayersGrid,
			this.clansGrid
		}
		where g != null
		select g;
		foreach (UIGrid g2 in grids)
		{
			g2.Reposition();
		}
		yield return null;
		IEnumerable<UIScrollView> scrolls = from s in new UIScrollView[]
		{
			this.friendsScroll,
			this.bestPlayersScroll,
			this.clansScroll
		}
		where s != null
		select s;
		foreach (UIScrollView s2 in scrolls)
		{
			s2.ResetPosition();
			s2.UpdatePosition();
		}
		yield break;
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x0012FCA4 File Offset: 0x0012DEA4
	public void Show(bool needShow, bool animate)
	{
	}

	// Token: 0x04002B45 RID: 11077
	public UIGrid friendsGrid;

	// Token: 0x04002B46 RID: 11078
	public UIGrid bestPlayersGrid;

	// Token: 0x04002B47 RID: 11079
	public UIGrid clansGrid;

	// Token: 0x04002B48 RID: 11080
	public UIButton friendsButton;

	// Token: 0x04002B49 RID: 11081
	public UIButton bestPlayersButton;

	// Token: 0x04002B4A RID: 11082
	public UIButton clansButton;

	// Token: 0x04002B4B RID: 11083
	public UIDragScrollView friendsPanel;

	// Token: 0x04002B4C RID: 11084
	public UIDragScrollView bestPlayersPanel;

	// Token: 0x04002B4D RID: 11085
	public UIDragScrollView clansPanel;

	// Token: 0x04002B4E RID: 11086
	public UIScrollView friendsScroll;

	// Token: 0x04002B4F RID: 11087
	public UIScrollView bestPlayersScroll;

	// Token: 0x04002B50 RID: 11088
	public UIScrollView clansScroll;

	// Token: 0x04002B51 RID: 11089
	public LeaderboardItemView footer;

	// Token: 0x04002B52 RID: 11090
	public LeaderboardItemView clanFooter;

	// Token: 0x04002B53 RID: 11091
	public UISprite temporaryBackground;

	// Token: 0x04002B54 RID: 11092
	public UISprite bestPlayersDefaultSprite;

	// Token: 0x04002B55 RID: 11093
	public UISprite clansDefaultSprite;

	// Token: 0x04002B56 RID: 11094
	public UILabel nickOrClanName;

	// Token: 0x04002B57 RID: 11095
	public ToggleButton btnLeaderboards;

	// Token: 0x04002B58 RID: 11096
	public GameObject opened;

	// Token: 0x04002B59 RID: 11097
	private MenuLeaderboardsView.State _currentState;

	// Token: 0x04002B5A RID: 11098
	private Vector3 _desiredPosition = Vector3.zero;

	// Token: 0x04002B5B RID: 11099
	private Vector3 _outOfScreenPosition = Vector3.zero;

	// Token: 0x0200068E RID: 1678
	public enum State
	{
		// Token: 0x04002B5F RID: 11103
		Friends,
		// Token: 0x04002B60 RID: 11104
		BestPlayers,
		// Token: 0x04002B61 RID: 11105
		Clans
	}
}
