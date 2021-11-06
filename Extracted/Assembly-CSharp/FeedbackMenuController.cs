using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x02000116 RID: 278
public sealed class FeedbackMenuController : MonoBehaviour
{
	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000810 RID: 2064 RVA: 0x00030BA4 File Offset: 0x0002EDA4
	// (set) Token: 0x06000811 RID: 2065 RVA: 0x00030BAC File Offset: 0x0002EDAC
	public static FeedbackMenuController Instance { get; private set; }

	// Token: 0x06000812 RID: 2066 RVA: 0x00030BB4 File Offset: 0x0002EDB4
	private void Awake()
	{
		FeedbackMenuController.Instance = this;
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00030BBC File Offset: 0x0002EDBC
	private void Start()
	{
		this.CurrentState = FeedbackMenuController.State.FAQ;
		IEnumerable<UIButton> enumerable = from b in new UIButton[]
		{
			this.faqButton,
			this.termsFuseButton
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
		if (this.sendFeedbackButton != null)
		{
			ButtonHandler component2 = this.sendFeedbackButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleSendFeedback;
			}
		}
		if (this.backButton != null)
		{
			ButtonHandler component3 = this.backButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.HandleBackButton;
			}
		}
		string text = typeof(SettingsController).Assembly.GetName().Version.ToString();
		if (this._versionLabel != null)
		{
			this._versionLabel.text = text;
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00030D34 File Offset: 0x0002EF34
	private void BackButton()
	{
		base.gameObject.SetActive(false);
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.settingsPanel != null)
		{
			MainMenuController.sharedController.settingsPanel.SetActive(true);
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00030D84 File Offset: 0x0002EF84
	private void HandleBackButton(object sender, EventArgs e)
	{
		this.BackButton();
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00030D8C File Offset: 0x0002EF8C
	public static void ShowDialogWithCompletion(Action handler)
	{
		if (handler != null)
		{
			handler();
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00030D9C File Offset: 0x0002EF9C
	private void HandleSendFeedback(object sender, EventArgs e)
	{
		Action handler = delegate()
		{
			string text = typeof(FeedbackMenuController).Assembly.GetName().Version.ToString();
			string text2 = string.Concat(new object[]
			{
				"mailto:pixelgun3D.supp0rt@gmail.com?subject=Feedback&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20",
				DateTime.Now.ToString(),
				"%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20",
				text,
				"%0D%0APlayerID:%20",
				FriendsController.sharedController.id,
				"%0D%0ACategory:%20Feedback%0D%0ADevice%20Type:%20",
				SystemInfo.deviceType,
				"%20",
				SystemInfo.deviceModel,
				"%0D%0AOS%20Version:%20",
				SystemInfo.operatingSystem,
				"%0D%0A------------------------"
			});
			text2 = text2.Replace(" ", "%20");
			Debug.Log(text2);
			Application.OpenURL(text2);
		};
		FeedbackMenuController.ShowDialogWithCompletion(handler);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00030DD0 File Offset: 0x0002EFD0
	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (this.faqButton != null && gameObject == this.faqButton.gameObject)
		{
			this.CurrentState = FeedbackMenuController.State.FAQ;
		}
		else if (this.termsFuseButton != null && gameObject == this.termsFuseButton.gameObject)
		{
			this.CurrentState = FeedbackMenuController.State.TermsFuse;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000819 RID: 2073 RVA: 0x00030E4C File Offset: 0x0002F04C
	// (set) Token: 0x0600081A RID: 2074 RVA: 0x00030E54 File Offset: 0x0002F054
	public FeedbackMenuController.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			if (this.faqButton != null)
			{
				this.faqButton.isEnabled = (value != FeedbackMenuController.State.FAQ);
				Transform transform = this.faqButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform != null)
				{
					transform.gameObject.SetActive(value != FeedbackMenuController.State.FAQ);
				}
				Transform transform2 = this.faqButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value == FeedbackMenuController.State.FAQ);
				}
				this.textFAQScroll.SetActive(value == FeedbackMenuController.State.FAQ);
			}
			if (this.termsFuseButton != null)
			{
				this.termsFuseButton.isEnabled = (value != FeedbackMenuController.State.TermsFuse);
				Transform transform3 = this.termsFuseButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(value != FeedbackMenuController.State.TermsFuse);
				}
				else
				{
					Debug.Log("_spriteLabel=null");
				}
				Transform transform4 = this.termsFuseButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(value == FeedbackMenuController.State.TermsFuse);
				}
				else
				{
					Debug.Log("_spriteLabel=null");
				}
				this.textTermsOfUse.SetActive(value == FeedbackMenuController.State.TermsFuse);
			}
			this._currentState = value;
		}
	}

	// Token: 0x040006A8 RID: 1704
	private FeedbackMenuController.State _currentState;

	// Token: 0x040006A9 RID: 1705
	public UIButton faqButton;

	// Token: 0x040006AA RID: 1706
	public UIButton termsFuseButton;

	// Token: 0x040006AB RID: 1707
	public UIButton sendFeedbackButton;

	// Token: 0x040006AC RID: 1708
	public UIButton backButton;

	// Token: 0x040006AD RID: 1709
	public GameObject textFAQScroll;

	// Token: 0x040006AE RID: 1710
	public GameObject textTermsOfUse;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	private UILabel _versionLabel;

	// Token: 0x02000117 RID: 279
	public enum State
	{
		// Token: 0x040006B4 RID: 1716
		FAQ,
		// Token: 0x040006B5 RID: 1717
		TermsFuse
	}
}
