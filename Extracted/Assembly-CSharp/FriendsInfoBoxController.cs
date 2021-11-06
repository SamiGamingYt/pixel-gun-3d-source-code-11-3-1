using System;
using I2.Loc;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class FriendsInfoBoxController : MonoBehaviour
{
	// Token: 0x060035C2 RID: 13762 RVA: 0x00115DF8 File Offset: 0x00113FF8
	private void Start()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x00115E1C File Offset: 0x0011401C
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x00115E30 File Offset: 0x00114030
	private void HandleLocalizationChanged()
	{
		this.processingDataBoxLabel.text = LocalizationStore.Key_0348;
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x00115E44 File Offset: 0x00114044
	public void ShowInfoBox(string text)
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.infoWindow;
		base.gameObject.SetActive(true);
		this.processindDataBoxContainer.gameObject.SetActive(false);
		this.infoBoxLabel.text = text;
		this.infoBoxContainer.gameObject.SetActive(true);
		this.background.gameObject.SetActive(true);
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x00115EA4 File Offset: 0x001140A4
	public void ShowProcessingDataBox()
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.processDataWindow;
		base.gameObject.SetActive(true);
		this.processindDataBoxContainer.gameObject.SetActive(true);
		this.infoBoxContainer.gameObject.SetActive(false);
		this.background.gameObject.SetActive(false);
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x00115EF8 File Offset: 0x001140F8
	public void Hide()
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.None;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x00115F10 File Offset: 0x00114110
	public void OnClickExitButton()
	{
		if (this._currentTypeBox == FriendsInfoBoxController.BoxType.processDataWindow || this._currentTypeBox == FriendsInfoBoxController.BoxType.blockClick)
		{
			return;
		}
		this.Hide();
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x00115F34 File Offset: 0x00114134
	public void SetBlockClickState()
	{
		this._currentTypeBox = FriendsInfoBoxController.BoxType.blockClick;
		base.gameObject.SetActive(true);
		this.processindDataBoxContainer.gameObject.SetActive(false);
		this.infoBoxContainer.gameObject.SetActive(false);
		this.background.gameObject.SetActive(false);
	}

	// Token: 0x0400278C RID: 10124
	public UIWidget background;

	// Token: 0x0400278D RID: 10125
	[Header("Processing data box")]
	public UIWidget processindDataBoxContainer;

	// Token: 0x0400278E RID: 10126
	public UILabel processingDataBoxLabel;

	// Token: 0x0400278F RID: 10127
	[Header("Info box")]
	public UIWidget infoBoxContainer;

	// Token: 0x04002790 RID: 10128
	public UILabel infoBoxLabel;

	// Token: 0x04002791 RID: 10129
	private FriendsInfoBoxController.BoxType _currentTypeBox = FriendsInfoBoxController.BoxType.None;

	// Token: 0x0200061A RID: 1562
	private enum BoxType
	{
		// Token: 0x04002793 RID: 10131
		infoWindow,
		// Token: 0x04002794 RID: 10132
		processDataWindow,
		// Token: 0x04002795 RID: 10133
		blockClick,
		// Token: 0x04002796 RID: 10134
		None
	}
}
