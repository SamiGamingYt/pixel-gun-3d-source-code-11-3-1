using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x0200073D RID: 1853
public class ReviewHUDWindow : MonoBehaviour
{
	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x0600412B RID: 16683 RVA: 0x0015BA3C File Offset: 0x00159C3C
	public static ReviewHUDWindow Instance
	{
		get
		{
			if (ReviewHUDWindow._instance == null)
			{
				GameObject gameObject = InfoWindowController.Instance.gameObject;
				ReviewHUDWindow._instance = gameObject.GetComponentInChildren<ReviewHUDWindow>();
				return ReviewHUDWindow._instance;
			}
			return ReviewHUDWindow._instance;
		}
	}

	// Token: 0x0600412C RID: 16684 RVA: 0x0015BA7C File Offset: 0x00159C7C
	private void Awake()
	{
		ReviewHUDWindow._instance = this;
		if (this.arrStarByOrder != null)
		{
			for (int i = 0; i < this.arrStarByOrder.Length; i++)
			{
				this.arrStarByOrder[i].numOrderStar = i;
				this.arrStarByOrder[i].lbNumStar.text = (i + 1).ToString();
			}
		}
	}

	// Token: 0x0600412D RID: 16685 RVA: 0x0015BAE0 File Offset: 0x00159CE0
	private void OnEnable()
	{
		this.OnShowThanks();
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x0015BAE8 File Offset: 0x00159CE8
	private void OnDestroy()
	{
		ReviewHUDWindow._instance = null;
	}

	// Token: 0x0600412F RID: 16687 RVA: 0x0015BAF0 File Offset: 0x00159CF0
	public void ShowWindowRating()
	{
		ReviewController.CheckActiveReview();
		if (ReviewController.IsNeedActive)
		{
			this.OnShowWidowRating();
		}
	}

	// Token: 0x06004130 RID: 16688 RVA: 0x0015BB08 File Offset: 0x00159D08
	public void SelectStar(StarReview curStar)
	{
		if (curStar != null)
		{
			this.countStarForReview = curStar.numOrderStar + 1;
		}
		if (this.arrStarByOrder != null)
		{
			for (int i = 0; i < this.arrStarByOrder.Length; i++)
			{
				if (curStar != null && i <= curStar.numOrderStar)
				{
					this.arrStarByOrder[i].SetActiveStar(true);
				}
				else
				{
					this.arrStarByOrder[i].SetActiveStar(false);
				}
			}
		}
	}

	// Token: 0x06004131 RID: 16689 RVA: 0x0015BB8C File Offset: 0x00159D8C
	public void OnChangeMsgReview()
	{
		this.UpdateStateBtnSendMsg(true);
	}

	// Token: 0x06004132 RID: 16690 RVA: 0x0015BB98 File Offset: 0x00159D98
	public void OnClickStarRating()
	{
		if (this.countStarForReview > 0 && this.countStarForReview <= 4)
		{
			this.OnShowWindowEnterMessage();
		}
		else
		{
			this.SendMsgReview(true);
		}
	}

	// Token: 0x06004133 RID: 16691 RVA: 0x0015BBD0 File Offset: 0x00159DD0
	public void OnSendMsgWithRating()
	{
		this.SendMsgReview(true);
	}

	// Token: 0x06004134 RID: 16692 RVA: 0x0015BBDC File Offset: 0x00159DDC
	private void SendMsgReview(bool isClickSend = true)
	{
		this.OnCloseAllWindow();
		if (this.countStarForReview > 0)
		{
			string msgReview = string.Empty;
			if (this.isInputMsgForReview)
			{
				msgReview = this.inputMsg.value;
			}
			if (this.countStarForReview == 5)
			{
				AnalyticsFacade.SendCustomEventToFacebook("5star_rating", null);
			}
			AnalyticsStuff.RateUsFake(true, this.countStarForReview, this.isInputMsgForReview && this.countStarForReview != 5);
			ReviewController.SendReview(this.countStarForReview, msgReview);
			if (isClickSend)
			{
				this.NeedShowThanks = true;
				ReviewHUDWindow.isShow = true;
				this.OnShowThanks();
			}
		}
	}

	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x06004135 RID: 16693 RVA: 0x0015BC7C File Offset: 0x00159E7C
	public string TitleTextTranslate
	{
		get
		{
			return string.Empty;
		}
	}

	// Token: 0x06004136 RID: 16694 RVA: 0x0015BC84 File Offset: 0x00159E84
	public void OnClickClose()
	{
		ReviewHUDWindow.isShow = false;
		AnalyticsStuff.RateUsFake(this.countStarForReview != 0, this.countStarForReview, false);
		this.SendMsgReview(false);
	}

	// Token: 0x06004137 RID: 16695 RVA: 0x0015BCAC File Offset: 0x00159EAC
	private void OnCloseAllWindow()
	{
		ReviewHUDWindow.isShow = false;
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(false);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowEnterMsg.SetActive(false);
		}
		if (this.objWindowGoToStore)
		{
			this.objWindowGoToStore.SetActive(false);
		}
		if (this.objThanks)
		{
			this.objThanks.SetActive(false);
		}
		this.RemoveBackSubscription();
	}

	// Token: 0x06004138 RID: 16696 RVA: 0x0015BD38 File Offset: 0x00159F38
	private void OnShowWindowEnterMessage()
	{
		this.UpdateStateBtnSendMsg(false);
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(false);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowEnterMsg.SetActive(true);
		}
	}

	// Token: 0x06004139 RID: 16697 RVA: 0x0015BD84 File Offset: 0x00159F84
	private void AddBackSubscription()
	{
		if (this._backSubscription == null)
		{
			this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClickClose), "Review HUD (Window with 5 stars)");
		}
	}

	// Token: 0x0600413A RID: 16698 RVA: 0x0015BDC0 File Offset: 0x00159FC0
	private void OnShowWidowRating()
	{
		ReviewHUDWindow.isShow = true;
		this.countStarForReview = 0;
		this.SelectStar(null);
		ReviewController.IsSendReview = true;
		ReviewController.IsNeedActive = false;
		if (this.lbTitle5Stars)
		{
			this.lbTitle5Stars.text = this.TitleTextTranslate;
		}
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(true);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowEnterMsg.SetActive(false);
		}
		if (this.objWindowGoToStore)
		{
			this.objWindowGoToStore.SetActive(false);
		}
		this.AddBackSubscription();
	}

	// Token: 0x0600413B RID: 16699 RVA: 0x0015BE68 File Offset: 0x0015A068
	private void OnShowWindowGoToStore()
	{
		if (this.objWindowRating)
		{
			this.objWindowRating.SetActive(false);
		}
		if (this.objWindowEnterMsg)
		{
			this.objWindowGoToStore.SetActive(true);
		}
	}

	// Token: 0x0600413C RID: 16700 RVA: 0x0015BEB0 File Offset: 0x0015A0B0
	private void OnShowThanks()
	{
		if (this.NeedShowThanks)
		{
			base.StartCoroutine(this.Crt_OnShowThanks());
		}
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x0015BECC File Offset: 0x0015A0CC
	private IEnumerator Crt_OnShowThanks()
	{
		yield return new WaitForEndOfFrame();
		if (this.NeedShowThanks)
		{
			this.NeedShowThanks = false;
			if (this.objThanks != null)
			{
				this.objThanks.SetActive(true);
			}
			this.AddBackSubscription();
			yield return new WaitForSeconds(3f);
			this.OnCloseThanks();
		}
		yield break;
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x0015BEE8 File Offset: 0x0015A0E8
	public void OnCloseThanks()
	{
		if (this.objThanks != null)
		{
			this.objThanks.SetActive(false);
		}
		BannerWindowController.firstScreen = false;
		ReviewHUDWindow.isShow = false;
		this.RemoveBackSubscription();
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x0015BF1C File Offset: 0x0015A11C
	private void UpdateStateBtnSendMsg(bool val)
	{
		this.isInputMsgForReview = val;
		if (this.isInputMsgForReview)
		{
			this.btnSendMsg.enabled = true;
			this.btnSendMsg.state = UIButtonColor.State.Normal;
		}
		else
		{
			this.btnSendMsg.enabled = false;
			this.btnSendMsg.state = UIButtonColor.State.Disabled;
		}
	}

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x06004140 RID: 16704 RVA: 0x0015BF70 File Offset: 0x0015A170
	// (set) Token: 0x06004141 RID: 16705 RVA: 0x0015BF78 File Offset: 0x0015A178
	public bool NeedShowThanks
	{
		get
		{
			return this._NeedShowThanks;
		}
		set
		{
			this._NeedShowThanks = value;
		}
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x0015BF84 File Offset: 0x0015A184
	[ContextMenu("Find all stars")]
	private void FindStars()
	{
		this.arrStarByOrder = base.GetComponentsInChildren<StarReview>(true);
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x0015BF94 File Offset: 0x0015A194
	[ContextMenu("Show window")]
	public void TestShow()
	{
		ReviewController.IsNeedActive = true;
		this.ShowWindowRating();
	}

	// Token: 0x06004144 RID: 16708 RVA: 0x0015BFA4 File Offset: 0x0015A1A4
	private void RemoveBackSubscription()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x04002F9A RID: 12186
	private static ReviewHUDWindow _instance;

	// Token: 0x04002F9B RID: 12187
	[Header("Добавить все звезды в список в их порядке активации при нажатии")]
	public StarReview[] arrStarByOrder;

	// Token: 0x04002F9C RID: 12188
	[Header("Окна")]
	public GameObject objWindowRating;

	// Token: 0x04002F9D RID: 12189
	public GameObject objWindowGoToStore;

	// Token: 0x04002F9E RID: 12190
	public GameObject objWindowEnterMsg;

	// Token: 0x04002F9F RID: 12191
	public GameObject objThanks;

	// Token: 0x04002FA0 RID: 12192
	[Header("Другое")]
	public UIInput inputMsg;

	// Token: 0x04002FA1 RID: 12193
	public UIButton btnSendMsg;

	// Token: 0x04002FA2 RID: 12194
	public UILabel lbTitle5Stars;

	// Token: 0x04002FA3 RID: 12195
	public static bool isShow;

	// Token: 0x04002FA4 RID: 12196
	private bool _NeedShowThanks;

	// Token: 0x04002FA5 RID: 12197
	public int countStarForReview;

	// Token: 0x04002FA6 RID: 12198
	private bool isInputMsgForReview;

	// Token: 0x04002FA7 RID: 12199
	private IDisposable _backSubscription;
}
