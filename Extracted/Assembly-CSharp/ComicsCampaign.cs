using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020005C4 RID: 1476
internal sealed class ComicsCampaign : MonoBehaviour
{
	// Token: 0x060032FE RID: 13054 RVA: 0x00107EA4 File Offset: 0x001060A4
	public void HandleSkipPressed()
	{
		ButtonClickSound.TryPlayClick();
		Debug.Log("[Skip] pressed.");
		if (this._skipHandler != null)
		{
			this._skipHandler();
		}
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x00107ECC File Offset: 0x001060CC
	public void HandleBackPressed()
	{
		ButtonClickSound.TryPlayClick();
		Singleton<SceneLoader>.Instance.LoadScene("ChooseLevel", LoadSceneMode.Single);
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x00107EE4 File Offset: 0x001060E4
	private bool DetermineIfFirstLaunch()
	{
		if (LevelArt.endOfBox)
		{
			string[] source = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			return !source.Any(new Func<string, bool>(CurrentCampaignGame.boXName.Equals));
		}
		string[] source2 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
		return !source2.Any(new Func<string, bool>(CurrentCampaignGame.levelSceneName.Equals));
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x00107F60 File Offset: 0x00106160
	private void Awake()
	{
		if (this.subtitlesText != null)
		{
			this.subtitlesText.transform.parent.gameObject.SetActive(LocalizationStore.CurrentLanguage != "English");
		}
		this._frameCount = Math.Min(4, this.comicFrames.Length);
		this._isFirstLaunch = this.DetermineIfFirstLaunch();
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x00107FC8 File Offset: 0x001061C8
	private IEnumerator Start()
	{
		Texture nextTexture = Resources.Load<Texture>(ComicsCampaign.GetNameForIndex(this._frameCount + 1, LevelArt.endOfBox));
		this._hasSecondPage = (nextTexture != null);
		if (this._isFirstLaunch)
		{
			this.SetSkipHandler(null);
			this.backButton.gameObject.SetActive(false);
		}
		else if (this._hasSecondPage)
		{
			this.SetSkipHandler(new Action(this.GotoNextPage));
		}
		else
		{
			this.SetSkipHandler(new Action(this.GotoLevelOrBoxmap));
		}
		if (this.background != null)
		{
			this.background.texture = Resources.Load<Texture>("Arts_background_" + CurrentCampaignGame.boXName);
		}
		for (int i = 0; i != this._frameCount; i++)
		{
			string pathToComicTexture = ComicsCampaign.GetNameForIndex(i + 1, LevelArt.endOfBox);
			Texture texture = Resources.Load<Texture>(pathToComicTexture);
			if (texture == null)
			{
				Debug.LogWarning("Texture is null: " + pathToComicTexture);
				break;
			}
			this.comicFrames[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)texture.width);
			this.comicFrames[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)texture.height);
			this.comicFrames[i].texture = texture;
			this.comicFrames[i].color = new Color(1f, 1f, 1f, 0f);
			string localizationKey = (!LevelArt.endOfBox) ? string.Format("{0}_{1}", CurrentCampaignGame.levelSceneName, i) : string.Format("{0}_{1}", CurrentCampaignGame.boXName, i);
			this._subtitles[i] = (LocalizationStore.Get(localizationKey) ?? string.Empty);
		}
		this._coroutine = base.StartCoroutine(this.FadeInCoroutine(null));
		yield return this._coroutine;
		if (this._hasSecondPage)
		{
			this.SetSkipHandler(new Action(this.GotoNextPage));
		}
		else
		{
			this.SetSkipHandler(new Action(this.GotoLevelOrBoxmap));
		}
		yield break;
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x00107FE4 File Offset: 0x001061E4
	private void GotoNextPage()
	{
		if (this._isFirstLaunch)
		{
			this.SetSkipHandler(null);
		}
		else
		{
			this.SetSkipHandler(new Action(this.GotoLevelOrBoxmap));
		}
		if (this._coroutine != null)
		{
			base.StopCoroutine(this._coroutine);
		}
		for (int num = 0; num != this.comicFrames.Length; num++)
		{
			if (!(this.comicFrames[num] == null))
			{
				this.comicFrames[num].texture = null;
				this.comicFrames[num].color = new Color(1f, 1f, 1f, 0f);
				this._subtitles[num] = string.Empty;
			}
		}
		Resources.UnloadUnusedAssets();
		for (int num2 = 0; num2 != this._frameCount; num2++)
		{
			string nameForIndex = ComicsCampaign.GetNameForIndex(this._frameCount + num2 + 1, LevelArt.endOfBox);
			Texture texture = Resources.Load<Texture>(nameForIndex);
			if (texture == null)
			{
				break;
			}
			this.comicFrames[num2].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)texture.width);
			this.comicFrames[num2].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)texture.height);
			this.comicFrames[num2].texture = texture;
			string term = (!LevelArt.endOfBox) ? string.Format("{0}_{1}", CurrentCampaignGame.levelSceneName, this._frameCount + num2) : string.Format("{0}_{1}", CurrentCampaignGame.boXName, this._frameCount + num2);
			this._subtitles[num2] = (LocalizationStore.Get(term) ?? string.Empty);
		}
		this._coroutine = base.StartCoroutine(this.FadeInCoroutine(new Action(this.GotoLevelOrBoxmap)));
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x001081B4 File Offset: 0x001063B4
	private void GotoLevelOrBoxmap()
	{
		if (this._coroutine != null)
		{
			base.StopCoroutine(this._coroutine);
		}
		if (LevelArt.endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			if (Array.IndexOf<string>(array, CurrentCampaignGame.boXName) == -1)
			{
				List<string> list = new List<string>(array);
				list.Add(CurrentCampaignGame.boXName);
				Save.SaveStringArray(Defs.ArtBoxS, list.ToArray());
			}
		}
		else
		{
			string[] array2 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			if (Array.IndexOf<string>(array2, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> list2 = new List<string>(array2);
				list2.Add(CurrentCampaignGame.levelSceneName);
				Save.SaveStringArray(Defs.ArtLevsS, list2.ToArray());
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.endOfBox) ? "CampaignLoading" : "ChooseLevel", LoadSceneMode.Single);
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x001082A4 File Offset: 0x001064A4
	private void SetSkipHandler(Action skipHandler)
	{
		this._skipHandler = skipHandler;
		if (this.skipButton != null)
		{
			this.skipButton.gameObject.SetActive(skipHandler != null);
		}
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x001082D8 File Offset: 0x001064D8
	private IEnumerator FadeInCoroutine(Action skipHandler = null)
	{
		for (int comicFrameIndex = 0; comicFrameIndex != this.comicFrames.Length; comicFrameIndex++)
		{
			RawImage f = this.comicFrames[comicFrameIndex];
			if (!(f == null))
			{
				if (this.subtitlesText != null)
				{
					this.subtitlesText.text = this._subtitles[comicFrameIndex];
				}
				for (int i = 0; i != 30; i++)
				{
					float newAlpha = Mathf.InverseLerp(0f, 30f, (float)i);
					f.color = new Color(1f, 1f, 1f, newAlpha);
					yield return new WaitForRealSeconds(0.033333335f);
				}
				f.color = new Color(1f, 1f, 1f, 1f);
				yield return new WaitForRealSeconds(2f);
			}
		}
		if (skipHandler != null)
		{
			this.SetSkipHandler(skipHandler);
		}
		yield break;
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x00108304 File Offset: 0x00106504
	private static string GetNameForIndex(int num, bool endOfBox)
	{
		return ResPath.Combine("Arts", ResPath.Combine((!endOfBox) ? CurrentCampaignGame.levelSceneName : CurrentCampaignGame.boXName, num.ToString()));
	}

	// Token: 0x04002580 RID: 9600
	public RawImage background;

	// Token: 0x04002581 RID: 9601
	public RawImage[] comicFrames = new RawImage[4];

	// Token: 0x04002582 RID: 9602
	public Button skipButton;

	// Token: 0x04002583 RID: 9603
	public Button backButton;

	// Token: 0x04002584 RID: 9604
	public Text subtitlesText;

	// Token: 0x04002585 RID: 9605
	private string[] _subtitles = new string[]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	// Token: 0x04002586 RID: 9606
	private int _frameCount;

	// Token: 0x04002587 RID: 9607
	private bool _hasSecondPage;

	// Token: 0x04002588 RID: 9608
	private bool _isFirstLaunch = true;

	// Token: 0x04002589 RID: 9609
	private Coroutine _coroutine;

	// Token: 0x0400258A RID: 9610
	private Action _skipHandler;
}
