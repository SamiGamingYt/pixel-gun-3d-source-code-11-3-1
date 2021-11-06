using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000566 RID: 1382
internal sealed class NicknameInput : MonoBehaviour
{
	// Token: 0x06002FE4 RID: 12260 RVA: 0x000FA1FC File Offset: 0x000F83FC
	private void HandleOkClicked(object sender, EventArgs e)
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		PlayerPrefs.SetString("NicknameRequested", "1");
		if (this.input != null)
		{
			if (this.input.value != null)
			{
				string text = this.input.value.Trim();
				string value = (!string.IsNullOrEmpty(text)) ? text : "Unnamed";
				PlayerPrefs.SetString("NamePlayer", value);
				this.input.value = value;
			}
			if (this._okButton != null)
			{
				this._okButton.isEnabled = false;
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene, LoadSceneMode.Single);
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000FA2C0 File Offset: 0x000F84C0
	private void Start()
	{
		ButtonHandler componentInChildren = base.gameObject.GetComponentInChildren<ButtonHandler>();
		if (componentInChildren != null)
		{
			componentInChildren.Clicked += this.HandleOkClicked;
			this._okButton = componentInChildren.GetComponent<UIButton>();
		}
		if (ExperienceController.sharedController != null && ExpController.Instance != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
			ExpController.Instance.InterfaceEnabled = false;
		}
		if (this.input != null)
		{
			string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
			this.input.value = playerNameOrDefault;
		}
	}

	// Token: 0x04002332 RID: 9010
	private const string PlayerNameKey = "NamePlayer";

	// Token: 0x04002333 RID: 9011
	public UIInput input;

	// Token: 0x04002334 RID: 9012
	private UIButton _okButton;
}
