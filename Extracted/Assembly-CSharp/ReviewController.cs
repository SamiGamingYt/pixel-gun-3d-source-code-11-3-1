using System;
using UnityEngine;

// Token: 0x0200073C RID: 1852
public class ReviewController : MonoBehaviour
{
	// Token: 0x0600411B RID: 16667 RVA: 0x0015B868 File Offset: 0x00159A68
	private void Awake()
	{
		ReviewController.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x0015B87C File Offset: 0x00159A7C
	private void OnDestroy()
	{
		ReviewController.instance = null;
	}

	// Token: 0x0600411D RID: 16669 RVA: 0x0015B884 File Offset: 0x00159A84
	public static void CheckActiveReview()
	{
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		if (GlobalGameController.CountDaySessionInCurrentVersion > 2 && ExperienceController.sharedController.currentLevel >= 4 && !ReviewController.IsSendReview)
		{
			ReviewController.IsNeedActive = true;
		}
	}

	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x0600411E RID: 16670 RVA: 0x0015B8D0 File Offset: 0x00159AD0
	// (set) Token: 0x0600411F RID: 16671 RVA: 0x0015B90C File Offset: 0x00159B0C
	public static bool ExistReviewForSend
	{
		get
		{
			if (ReviewController._ExistReviewForSend < 0)
			{
				ReviewController._ExistReviewForSend = ((!Load.LoadBool("keyNeedSendMsgReview")) ? 0 : 1);
			}
			return ReviewController._ExistReviewForSend != 0;
		}
		set
		{
			Save.SaveBool("keyNeedSendMsgReview", value);
			ReviewController._ExistReviewForSend = ((!value) ? 0 : 1);
		}
	}

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x06004120 RID: 16672 RVA: 0x0015B92C File Offset: 0x00159B2C
	// (set) Token: 0x06004121 RID: 16673 RVA: 0x0015B938 File Offset: 0x00159B38
	public static int ReviewRating
	{
		get
		{
			return Load.LoadInt("keyReviewSaveRating");
		}
		set
		{
			Save.SaveInt("keyReviewSaveRating", value);
		}
	}

	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x06004122 RID: 16674 RVA: 0x0015B948 File Offset: 0x00159B48
	// (set) Token: 0x06004123 RID: 16675 RVA: 0x0015B954 File Offset: 0x00159B54
	public static string ReviewMsg
	{
		get
		{
			return Load.LoadString("keyReviewSaveMsg");
		}
		set
		{
			Save.SaveString("keyReviewSaveMsg", value);
		}
	}

	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x06004124 RID: 16676 RVA: 0x0015B964 File Offset: 0x00159B64
	// (set) Token: 0x06004125 RID: 16677 RVA: 0x0015B970 File Offset: 0x00159B70
	public static bool IsSendReview
	{
		get
		{
			return Load.LoadBool("keyAlreadySendReview");
		}
		set
		{
			Save.SaveBool("keyAlreadySendReview", value);
		}
	}

	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x06004126 RID: 16678 RVA: 0x0015B980 File Offset: 0x00159B80
	// (set) Token: 0x06004127 RID: 16679 RVA: 0x0015B9BC File Offset: 0x00159BBC
	public static bool IsNeedActive
	{
		get
		{
			if (ReviewController._IsNeedActive < 0)
			{
				ReviewController._IsNeedActive = ((!Load.LoadBool("keyNeedActiveReview")) ? 0 : 1);
			}
			return ReviewController._IsNeedActive != 0;
		}
		set
		{
			if (ReviewController.ExistReviewForSend && value)
			{
				return;
			}
			if (ReviewController.IsNeedActive != value)
			{
				Save.SaveBool("keyNeedActiveReview", value);
			}
			ReviewController._IsNeedActive = ((!value) ? 0 : 1);
		}
	}

	// Token: 0x06004128 RID: 16680 RVA: 0x0015B9F8 File Offset: 0x00159BF8
	public static void SendReview(int rating, string msgReview)
	{
		ReviewController.ReviewRating = rating;
		ReviewController.ReviewMsg = msgReview;
		ReviewController.ExistReviewForSend = true;
		if (rating == 5)
		{
			Application.OpenURL(Defs2.ApplicationUrl);
		}
		FriendsController.StartSendReview();
	}

	// Token: 0x04002F90 RID: 12176
	public const string keyNeedActiveReview = "keyNeedActiveReview";

	// Token: 0x04002F91 RID: 12177
	public const string keyAlreadySendReview = "keyAlreadySendReview";

	// Token: 0x04002F92 RID: 12178
	public const string keyOldVersionForReview = "keyOldVersionForReview";

	// Token: 0x04002F93 RID: 12179
	public const string keyNeedSendMsgReview = "keyNeedSendMsgReview";

	// Token: 0x04002F94 RID: 12180
	public const string keyReviewSaveRating = "keyReviewSaveRating";

	// Token: 0x04002F95 RID: 12181
	public const string keyReviewSaveMsg = "keyReviewSaveMsg";

	// Token: 0x04002F96 RID: 12182
	public static ReviewController instance;

	// Token: 0x04002F97 RID: 12183
	public static int _IsNeedActive = -1;

	// Token: 0x04002F98 RID: 12184
	public static int _ExistReviewForSend = -1;

	// Token: 0x04002F99 RID: 12185
	public static bool isSending;
}
