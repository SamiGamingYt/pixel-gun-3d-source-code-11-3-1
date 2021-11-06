using System;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class ChooseLanguage : MonoBehaviour
{
	// Token: 0x0600163F RID: 5695 RVA: 0x00059EB8 File Offset: 0x000580B8
	private void SetSelectCurrentLanguage()
	{
		int currentLanguageIndex = LocalizationStore.GetCurrentLanguageIndex();
		if (currentLanguageIndex == -1 || currentLanguageIndex >= this.languageButtons.Length)
		{
			return;
		}
		if (this._currentLanguage != null)
		{
			this._currentLanguage.ResetDefaultColor();
		}
		this.languageButtons[currentLanguageIndex].defaultColor = Color.grey;
		this._currentLanguage = this.languageButtons[currentLanguageIndex];
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00059F20 File Offset: 0x00058120
	private void Start()
	{
		this.SetSelectCurrentLanguage();
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x00059F28 File Offset: 0x00058128
	private void SelectLanguage(string languageName)
	{
		ButtonClickSound.TryPlayClick();
		LocalizationStore.CurrentLanguage = languageName;
		this.SetSelectCurrentLanguage();
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x00059F3C File Offset: 0x0005813C
	public void SetRussianLanguage()
	{
		this.SelectLanguage("Russian");
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x00059F4C File Offset: 0x0005814C
	public void SetEnglishLanguage()
	{
		this.SelectLanguage("English");
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x00059F5C File Offset: 0x0005815C
	public void SetFrancianLanguage()
	{
		this.SelectLanguage("French");
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x00059F6C File Offset: 0x0005816C
	public void SetDeutschLanguage()
	{
		this.SelectLanguage("German");
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x00059F7C File Offset: 0x0005817C
	public void SetJapanLanguage()
	{
		this.SelectLanguage("Japanese");
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00059F8C File Offset: 0x0005818C
	public void SetEspanolaLanguage()
	{
		this.SelectLanguage("Spanish");
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x00059F9C File Offset: 0x0005819C
	public void SetChinseLanguage()
	{
		this.SelectLanguage("Chinese (Chinese)");
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x00059FAC File Offset: 0x000581AC
	public void SetKoreanLanguage()
	{
		this.SelectLanguage("Korean");
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x00059FBC File Offset: 0x000581BC
	public void SetBrazilLanguage()
	{
		this.SelectLanguage("Portuguese (Brazil)");
	}

	// Token: 0x04000D06 RID: 3334
	public UIButton[] languageButtons;

	// Token: 0x04000D07 RID: 3335
	private UIButton _currentLanguage;
}
