using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public sealed class PlayerPanel : MonoBehaviour
{
	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06002808 RID: 10248 RVA: 0x000C8130 File Offset: 0x000C6330
	private int maxRating
	{
		get
		{
			return RatingSystem.instance.MaxRatingInLeague(RatingSystem.instance.currentLeague);
		}
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06002809 RID: 10249 RVA: 0x000C8148 File Offset: 0x000C6348
	private int league
	{
		get
		{
			return (int)RatingSystem.instance.currentLeague;
		}
	}

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x0600280A RID: 10250 RVA: 0x000C8154 File Offset: 0x000C6354
	// (set) Token: 0x0600280B RID: 10251 RVA: 0x000C8188 File Offset: 0x000C6388
	public string ExperienceLabel
	{
		get
		{
			return (!(this.experienceLabel != null)) ? string.Empty : this.experienceLabel.text;
		}
		set
		{
			if (this.experienceLabel != null)
			{
				this.experienceLabel.text = (value ?? string.Empty);
			}
		}
	}

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x0600280C RID: 10252 RVA: 0x000C81B4 File Offset: 0x000C63B4
	// (set) Token: 0x0600280D RID: 10253 RVA: 0x000C81F4 File Offset: 0x000C63F4
	public float CurrentProgress
	{
		get
		{
			return (!(this.currentExp != null)) ? 0f : this.currentExp.transform.localScale.x;
		}
		set
		{
			if (this.currentExp != null)
			{
				this.currentExp.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), 1f, 1f);
			}
		}
	}

	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x0600280E RID: 10254 RVA: 0x000C8244 File Offset: 0x000C6444
	// (set) Token: 0x0600280F RID: 10255 RVA: 0x000C8284 File Offset: 0x000C6484
	public float OldProgress
	{
		get
		{
			return (!(this.oldExp != null)) ? 0f : this.oldExp.transform.localScale.x;
		}
		set
		{
			if (this.oldExp != null)
			{
				this.oldExp.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), base.transform.localScale.y, base.transform.localScale.z);
			}
		}
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x06002810 RID: 10256 RVA: 0x000C82F0 File Offset: 0x000C64F0
	// (set) Token: 0x06002811 RID: 10257 RVA: 0x000C82F8 File Offset: 0x000C64F8
	public int RankSprite
	{
		get
		{
			return this.currentLevel;
		}
		set
		{
			if (this.rankSprite != null)
			{
				string spriteName = string.Format("Rank_{0}", value);
				this.rankSprite.spriteName = spriteName;
				this.currentLevel = value;
			}
		}
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x000C833C File Offset: 0x000C653C
	private void Awake()
	{
		PlayerPanel.instance = this;
		RatingSystem.OnRatingUpdate += this.OnRatingUpdated;
		this.playerNameStartPos = this.playerName.transform.localPosition;
		this.panelContainer = base.transform.GetChild(0).gameObject;
		this.panelContainer.SetActive(AskNameManager.isComplete);
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x000C83A0 File Offset: 0x000C65A0
	private void OnEnable()
	{
		if (FriendsController.sharedController.clanName != string.Empty)
		{
			string text = FriendsController.sharedController.clanName;
			this.clanName.text = text;
			int num = 0;
			while (this.clanName.width > 168)
			{
				num++;
				this.clanName.text = text.Remove(text.Length - num);
				this.clanName.ProcessText();
			}
			byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(data);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			this.clanIcon.mainTexture = texture2D;
			this.playerName.transform.localPosition = this.playerNameStartPos;
		}
		else
		{
			this.clanName.enabled = false;
			this.clanIcon.enabled = false;
			this.playerName.transform.localPosition = this.playerNameStartPos - Vector3.down * -16f;
		}
		this.UpdateNickPlayer();
		this.UpdateRating();
		this.UpdateExp();
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x000C84D4 File Offset: 0x000C66D4
	public void UpdateRating()
	{
		this.raitingText = RatingSystem.instance.currentRating.ToString();
		this.raitingLabel.text = this.raitingText;
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x000C850C File Offset: 0x000C670C
	public void UpdateNickPlayer()
	{
		string text = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
		this.playerName.text = text;
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000C8530 File Offset: 0x000C6730
	public void UpdateExp()
	{
		int num = ExperienceController.sharedController.currentLevel;
		this.curentExp = ExperienceController.sharedController.CurrentExperience;
		int num2 = ExperienceController.MaxExpLevelsDefault[num];
		this.RankSprite = num;
		if (num != 31)
		{
			this.OldProgress = (float)this.curentExp / (float)num2;
			this.CurrentProgress = (float)this.curentExp / (float)num2;
			this.ExperienceLabel = this.curentExp.ToString() + "/" + num2.ToString();
		}
		else
		{
			this.ExperienceLabel = LocalizationStore.Get("Key_0928");
			this.CurrentProgress = 1f;
		}
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000C85D4 File Offset: 0x000C67D4
	private void OnDisable()
	{
		this.oldExp.enabled = true;
		this.oldExp.transform.localScale = this.currentExp.transform.localScale;
		base.StopAllCoroutines();
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000C8614 File Offset: 0x000C6814
	private void Update()
	{
		if (this.curentExp != ExperienceController.sharedController.CurrentExperience || this.currentLevel != ExperienceController.sharedController.currentLevel)
		{
			this.OnExpUpdate();
		}
		if (this.panelContainer.activeSelf != AskNameManager.isComplete)
		{
			this.panelContainer.SetActive(AskNameManager.isComplete);
		}
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000C8678 File Offset: 0x000C6878
	private void OnExpUpdate()
	{
		int num = ExperienceController.sharedController.currentLevel;
		this.curentExp = ExperienceController.sharedController.CurrentExperience;
		int num2 = ExperienceController.MaxExpLevelsDefault[num];
		this.RankSprite = num;
		if (num != 31)
		{
			this.CurrentProgress = (float)this.curentExp / (float)num2;
			this.ExperienceLabel = this.curentExp.ToString() + "/" + num2.ToString();
			if (this.oldExp.transform.localScale.x > this.currentExp.transform.localScale.x)
			{
				this.oldExp.transform.localScale = Vector3.zero;
			}
			base.StartCoroutine(this.StartExpAnim());
		}
		else
		{
			this.ExperienceLabel = LocalizationStore.Get("Key_0928");
			this.CurrentProgress = 1f;
		}
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x000C8760 File Offset: 0x000C6960
	private IEnumerator StartExpAnim()
	{
		for (int i = 0; i != 4; i++)
		{
			this.currentExp.enabled = false;
			yield return new WaitForSeconds(0.15f);
			this.currentExp.enabled = true;
			yield return new WaitForSeconds(0.15f);
		}
		yield return null;
		this.oldExp.transform.localScale = this.currentExp.transform.localScale;
		yield break;
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x000C877C File Offset: 0x000C697C
	public void HandleOpenProfile()
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.GoToProfile();
		}
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SetStaticticTab(ProfileStatTabType.Leagues);
		}
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x000C87C0 File Offset: 0x000C69C0
	private void OnRatingUpdated()
	{
		this.raitingText = RatingSystem.instance.currentRating.ToString();
		this.raitingLabel.text = this.raitingText;
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000C87F8 File Offset: 0x000C69F8
	private void OnDestroy()
	{
		PlayerPanel.instance = null;
		RatingSystem.OnRatingUpdate -= this.OnRatingUpdated;
	}

	// Token: 0x04001C6C RID: 7276
	public static PlayerPanel instance;

	// Token: 0x04001C6D RID: 7277
	[SerializeField]
	private UILabel raitingLabel;

	// Token: 0x04001C6E RID: 7278
	private string raitingText;

	// Token: 0x04001C6F RID: 7279
	[SerializeField]
	public UILabel experienceLabel;

	// Token: 0x04001C70 RID: 7280
	[SerializeField]
	public UISprite currentExp;

	// Token: 0x04001C71 RID: 7281
	[SerializeField]
	public UISprite oldExp;

	// Token: 0x04001C72 RID: 7282
	[SerializeField]
	public UISprite rankSprite;

	// Token: 0x04001C73 RID: 7283
	private int curentExp;

	// Token: 0x04001C74 RID: 7284
	private int currentLevel = 1;

	// Token: 0x04001C75 RID: 7285
	[SerializeField]
	private UILabel playerName;

	// Token: 0x04001C76 RID: 7286
	[SerializeField]
	private UITexture clanIcon;

	// Token: 0x04001C77 RID: 7287
	[SerializeField]
	private UILabel clanName;

	// Token: 0x04001C78 RID: 7288
	private Vector3 playerNameStartPos;

	// Token: 0x04001C79 RID: 7289
	private GameObject panelContainer;
}
