using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002EF RID: 751
internal sealed class LevelArt : MonoBehaviour
{
	// Token: 0x06001A2F RID: 6703 RVA: 0x00069BD0 File Offset: 0x00067DD0
	[Obsolete("Use ComicsCampaign via uGUI instead of this class.")]
	private void Start()
	{
		this._needShowSubtitle = (LocalizationStore.CurrentLanguage != "English");
		this.labelsStyle.font = LocalizationStore.GetFontByLocalize("Key_04B_03");
		this.labelsStyle.fontSize = Mathf.RoundToInt(20f * Defs.Coef);
		if (Resources.Load<Texture>(this._NameForNumber(5)) != null)
		{
			this._countOfComics *= 2;
		}
		base.StartCoroutine("ShowArts");
		this._backgroundComics = Resources.Load<Texture>("Arts_background_" + CurrentCampaignGame.boXName);
		if (LevelArt.endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			foreach (string text in array)
			{
				if (text.Equals(CurrentCampaignGame.boXName))
				{
					this._isFirstLaunch = false;
					break;
				}
			}
		}
		else
		{
			string[] array3 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			foreach (string text2 in array3)
			{
				if (text2.Equals(CurrentCampaignGame.levelSceneName))
				{
					this._isFirstLaunch = false;
					break;
				}
			}
		}
		this._isShowButton = !this._isFirstLaunch;
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x00069D34 File Offset: 0x00067F34
	private void GoToLevel()
	{
		if (LevelArt.endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS) ?? new string[0];
			if (Array.IndexOf<string>(array, CurrentCampaignGame.boXName) == -1)
			{
				List<string> list = new List<string>();
				foreach (string item in array)
				{
					list.Add(item);
				}
				list.Add(CurrentCampaignGame.boXName);
				Save.SaveStringArray(Defs.ArtBoxS, list.ToArray());
			}
		}
		else
		{
			string[] array3 = Load.LoadStringArray(Defs.ArtLevsS) ?? new string[0];
			if (!LevelArt.endOfBox && Array.IndexOf<string>(array3, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> list2 = new List<string>();
				foreach (string item2 in array3)
				{
					list2.Add(item2);
				}
				list2.Add(CurrentCampaignGame.levelSceneName);
				Save.SaveStringArray(Defs.ArtLevsS, list2.ToArray());
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.endOfBox) ? "CampaignLoading" : "ChooseLevel", LoadSceneMode.Single);
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x00069E6C File Offset: 0x0006806C
	private string _NameForNumber(int num)
	{
		return ResPath.Combine("Arts", ResPath.Combine((!LevelArt.endOfBox) ? CurrentCampaignGame.levelSceneName : CurrentCampaignGame.boXName, num.ToString()));
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x00069EA0 File Offset: 0x000680A0
	[Obfuscation(Exclude = true)]
	private IEnumerator ShowArts()
	{
		string pathToComicsTexture = string.Empty;
		Texture newComicsTexture = null;
		for (;;)
		{
			newComicsTexture = null;
			this._currentComicsImageIndex++;
			pathToComicsTexture = this._NameForNumber(this._currentComicsImageIndex);
			newComicsTexture = Resources.Load<Texture>(pathToComicsTexture);
			if (!(newComicsTexture != null))
			{
				break;
			}
			if (this._comicsTextures.Count == 4)
			{
				this._comicsTextures.Clear();
			}
			this._comicsTextures.Add(newComicsTexture);
			string localizationKey = (!LevelArt.endOfBox) ? string.Format("{0}_{1}", CurrentCampaignGame.levelSceneName, this._currentComicsImageIndex - 1) : string.Format("{0}_{1}", CurrentCampaignGame.boXName, this._currentComicsImageIndex - 1);
			this._currentSubtitle = (LocalizationStore.Get(localizationKey) ?? string.Empty);
			if (localizationKey.Equals(this._currentSubtitle))
			{
				this._currentSubtitle = string.Empty;
			}
			Resources.UnloadUnusedAssets();
			this._alphaForComics = 0f;
			float prevTime = Time.time;
			float startTime = Time.time;
			do
			{
				yield return new WaitForEndOfFrame();
				this._alphaForComics += (Time.time - prevTime) / this._delayShowComics;
				prevTime = Time.time;
			}
			while (Time.time - startTime < this._delayShowComics && !this._isSkipComics);
			this._isSkipComics = false;
			this._alphaForComics = 1f;
			if (!(newComicsTexture != null) || this._currentComicsImageIndex % 4 == 0)
			{
				goto IL_267;
			}
		}
		this.GoToLevel();
		yield break;
		IL_267:
		yield return new WaitForSeconds(this._delayShowComics);
		this._isShowButton = true;
		yield break;
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x00069EBC File Offset: 0x000680BC
	[Obsolete("Use ComicsCampaign via uGUI instead of this class.")]
	private void OnGUI()
	{
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x00069ECC File Offset: 0x000680CC
	public static string WrappedText(string text)
	{
		int num = 30;
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		int num2 = 0;
		while (i < text.Length)
		{
			stringBuilder.Append(text[i]);
			if (text[i] == '\n')
			{
				stringBuilder.Append('\n');
			}
			if (num2 >= num && text[i] == ' ')
			{
				stringBuilder.Append("\n\n");
				num2 = 0;
			}
			i++;
			num2++;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04000F45 RID: 3909
	private const int ComicsOnScreen = 4;

	// Token: 0x04000F46 RID: 3910
	public static readonly bool ShouldShowArts = true;

	// Token: 0x04000F47 RID: 3911
	public GUIStyle startButton;

	// Token: 0x04000F48 RID: 3912
	public static bool endOfBox;

	// Token: 0x04000F49 RID: 3913
	public GUIStyle labelsStyle;

	// Token: 0x04000F4A RID: 3914
	public float widthBackLabel = 770f;

	// Token: 0x04000F4B RID: 3915
	public float heightBackLabel = 100f;

	// Token: 0x04000F4C RID: 3916
	private float _alphaForComics;

	// Token: 0x04000F4D RID: 3917
	private int _currentComicsImageIndex;

	// Token: 0x04000F4E RID: 3918
	private bool _isFirstLaunch = true;

	// Token: 0x04000F4F RID: 3919
	public float _delayShowComics = 3f;

	// Token: 0x04000F50 RID: 3920
	private bool _isSkipComics;

	// Token: 0x04000F51 RID: 3921
	private int _countOfComics = 4;

	// Token: 0x04000F52 RID: 3922
	private Texture _backgroundComics;

	// Token: 0x04000F53 RID: 3923
	private List<Texture> _comicsTextures = new List<Texture>();

	// Token: 0x04000F54 RID: 3924
	private bool _isShowButton;

	// Token: 0x04000F55 RID: 3925
	private string _currentSubtitle;

	// Token: 0x04000F56 RID: 3926
	private bool _needShowSubtitle;
}
