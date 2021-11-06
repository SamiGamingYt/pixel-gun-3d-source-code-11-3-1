using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020007BE RID: 1982
public class SkinEditorController : MonoBehaviour
{
	// Token: 0x060047C7 RID: 18375 RVA: 0x0018C454 File Offset: 0x0018A654
	// Note: this type is marked as 'beforefieldinit'.
	static SkinEditorController()
	{
		SkinEditorController.ExitFromSkinEditor = null;
	}

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x060047C8 RID: 18376 RVA: 0x0018C52C File Offset: 0x0018A72C
	// (remove) Token: 0x060047C9 RID: 18377 RVA: 0x0018C544 File Offset: 0x0018A744
	public static event Action<string> ExitFromSkinEditor;

	// Token: 0x060047CA RID: 18378 RVA: 0x0018C55C File Offset: 0x0018A75C
	private void Awake()
	{
		GameObject original = Resources.Load("Character_model") as GameObject;
		this.characterInterface = (UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.Euler(0f, 158.15f, 0f)) as GameObject).GetComponent<CharacterInterface>();
		this.characterInterface.GetComponent<CharacterInterface>().usePetFromStorager = false;
		this.characterInterface.transform.SetParent(this.previewPers.transform, false);
		this.characterInterface.SetCharacterType(true, true, true);
		this.characterInterface.usePetFromStorager = false;
		ShopNGUIController.DisableLightProbesRecursively(this.characterInterface.gameObject);
		Player_move_c.SetLayerRecursively(this.characterInterface.gameObject, this.previewPers.layer);
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x0018C61C File Offset: 0x0018A81C
	private void Start()
	{
		SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
		SkinEditorController.isEditingSkin = false;
		SkinEditorController.isEditingPartSkin = false;
		SkinEditorController.sharedController = this;
		MenuBackgroundMusic.sharedMusic.PlayCustomMusicFrom(base.gameObject);
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		SkinEditorController.colorForPaint = new Color(PlayerPrefs.GetFloat("ColorForPaintR", 0f), PlayerPrefs.GetFloat("ColorForPaintG", 1f), PlayerPrefs.GetFloat("ColorForPaintB", 0f), 1f);
		this.colorButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		for (int i = 0; i < this.colorOnBrashSprites.Length; i++)
		{
			this.colorOnBrashSprites[i].color = SkinEditorController.colorForPaint;
		}
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.SkinPers)
		{
			if (SkinEditorController.currentSkinName == null)
			{
				SkinEditorController.currentSkin = (Resources.Load("Clear_Skin") as Texture2D);
				SkinEditorController.currentSkin.filterMode = FilterMode.Point;
				SkinEditorController.currentSkin.Apply();
				this.skinNameInput.value = string.Empty;
			}
			else
			{
				SkinEditorController.currentSkin = SkinsController.skinsForPers[SkinEditorController.currentSkinName];
				if (SkinsController.skinsNamesForPers.ContainsKey(SkinEditorController.currentSkinName))
				{
					this.skinNameInput.value = SkinsController.skinsNamesForPers[SkinEditorController.currentSkinName];
				}
			}
			Debug.Log("modeEditor== ModeEditor.SkinPers");
			this.partPreviewPanel.SetActive(false);
			this.skinPreviewPanel.SetActive(true);
			this.editorPanel.SetActive(false);
			this.currentPreviewsSkin.Add(this.previewPers);
			this.currentPreviewsSkin.Add(this.previewPersShadow);
			this.ShowPreviewSkin();
		}
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape || SkinEditorController.modeEditor == SkinEditorController.ModeEditor.LogoClan)
		{
			Texture2D texture2D;
			if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape)
			{
				texture2D = SkinsController.capeUserTexture;
				if (texture2D == null)
				{
					texture2D = Resources.Load<Texture2D>("cape_CustomTexture");
				}
			}
			else
			{
				texture2D = SkinsController.logoClanUserTexture;
				if (texture2D == null)
				{
					texture2D = Resources.Load<Texture2D>("Clan_Previews/icon_clan_001");
				}
			}
			SkinEditorController.currentSkin = EditorTextures.CreateCopyTexture(texture2D);
			this.partPreviewPanel.SetActive(false);
			this.skinPreviewPanel.SetActive(false);
			this.editorPanel.SetActive(true);
			this.editorTexture.gameObject.GetComponent<EditorTextures>().SetStartCanvas(SkinEditorController.currentSkin);
		}
		this.savePanelInEditorTexture.SetActive(false);
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.SetPartsRect();
		this.UpdateTexturesPartsInDictionary();
		this.colorPanel.SetActive(false);
		this.saveChangesPanel.SetActive(false);
		this.saveSkinPanel.SetActive(false);
		this.leavePanel.SetActive(false);
		if (this.topPart != null)
		{
			ButtonHandler component = this.topPart.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandleSideClicked;
			}
		}
		if (this.downPart != null)
		{
			ButtonHandler component2 = this.downPart.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleSideClicked;
			}
		}
		if (this.leftPart != null)
		{
			ButtonHandler component3 = this.leftPart.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.HandleSideClicked;
			}
		}
		if (this.frontPart != null)
		{
			ButtonHandler component4 = this.frontPart.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += this.HandleSideClicked;
			}
		}
		if (this.rigthPart != null)
		{
			ButtonHandler component5 = this.rigthPart.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += this.HandleSideClicked;
			}
		}
		if (this.backPart != null)
		{
			ButtonHandler component6 = this.backPart.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += this.HandleSideClicked;
			}
		}
		if (this.saveButton != null)
		{
			this.saveButton.Clicked += this.HandleSaveButtonClicked;
		}
		if (this.backButton != null)
		{
			this.backButton.Clicked += this.HandleBackButtonClicked;
		}
		if (this.fillButton != null)
		{
			this.fillButton.Clicked += this.HandleSelectBrashClicked;
		}
		if (this.brashButton != null)
		{
			this.brashButton.Clicked += this.HandleSelectBrashClicked;
		}
		if (this.pencilButton != null)
		{
			this.pencilButton.Clicked += this.HandleSelectBrashClicked;
		}
		if (this.pipetteButton != null)
		{
			this.pipetteButton.Clicked += this.HandleSelectBrashClicked;
		}
		if (this.eraserButton != null)
		{
			this.eraserButton.Clicked += this.HandleSelectBrashClicked;
		}
		if (this.colorButton != null)
		{
			this.colorButton.Clicked += this.HandleSelectColorClicked;
		}
		if (this.setColorButton != null)
		{
			this.setColorButton.Clicked += this.HandleSetColorClicked;
		}
		if (this.saveChangesButton != null)
		{
			DialogEscape dialogEscape = this.saveChangesButton.GetComponent<DialogEscape>() ?? this.saveChangesButton.gameObject.AddComponent<DialogEscape>();
			dialogEscape.Context = "Save Skin Changes Dialog";
			this.saveChangesButton.Clicked += this.HandleSaveChangesButtonClicked;
		}
		if (this.cancelInSaveChangesButton != null)
		{
			this.cancelInSaveChangesButton.Clicked += this.HandleCancelInSaveChangesButtonClicked;
		}
		if (this.okInSaveSkin != null)
		{
			DialogEscape dialogEscape2 = this.okInSaveSkin.GetComponent<DialogEscape>() ?? this.okInSaveSkin.gameObject.AddComponent<DialogEscape>();
			dialogEscape2.Context = "Save Skin as... Dialog";
			this.okInSaveSkin.Clicked += this.HandleOkInSaveSkinClicked;
		}
		if (this.cancelInSaveSkin != null)
		{
			this.cancelInSaveSkin.Clicked += this.HandleCancelInSaveSkinClicked;
		}
		if (this.yesInLeaveSave != null)
		{
			DialogEscape dialogEscape3 = this.yesInLeaveSave.GetComponent<DialogEscape>() ?? this.yesInLeaveSave.gameObject.AddComponent<DialogEscape>();
			dialogEscape3.Context = "Save Skin Dialog";
			this.yesInLeaveSave.Clicked += this.HandleYesInLeaveSaveClicked;
		}
		if (this.noInLeaveSave != null)
		{
			this.noInLeaveSave.Clicked += this.HandleNoInLeaveSaveClicked;
		}
		if (this.yesSaveButtonInEdit != null)
		{
			DialogEscape dialogEscape4 = this.yesSaveButtonInEdit.GetComponent<DialogEscape>() ?? this.yesSaveButtonInEdit.gameObject.AddComponent<DialogEscape>();
			dialogEscape4.Context = "Save Cape Dialog";
			this.yesSaveButtonInEdit.Clicked += this.HandleYesSaveButtonInEditClicked;
		}
		if (this.noSaveButtonInEdit != null)
		{
			this.noSaveButtonInEdit.Clicked += this.HandleNoSaveButtonInEditClicked;
		}
		for (int j = 0; j < this.colorHistoryButtons.Length; j++)
		{
			this.colorHistoryButtons[j].Clicked += this.HandleSetHistoryColorClicked;
		}
		if (this.presetsButton != null)
		{
			this.presetsButton.Clicked += this.HandlePresetsButtonClicked;
		}
		if (this.closePresetPanelButton != null)
		{
			this.closePresetPanelButton.Clicked += this.HandleClosePresetClicked;
		}
		if (this.selectPresetButton != null)
		{
			this.selectPresetButton.Clicked += this.HandleSelectPresetClicked;
		}
		if (this.centeredPresetButton != null)
		{
			this.centeredPresetButton.Clicked += this.HandleSelectPresetClicked;
		}
		this.AddColor(SkinEditorController.colorForPaint);
		this.UpdateHistoryColors();
		this.GetPresetSkins();
		UIWrapContent uiwrapContent = this.skinPresentsWrap;
		uiwrapContent.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(uiwrapContent.onInitializeItem, new UIWrapContent.OnInitializeItem(this.WrapSkinPreset));
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x0018CF48 File Offset: 0x0018B148
	private void WrapSkinPreset(GameObject obj, int wrapIndex, int realIndex)
	{
		obj.GetComponent<WrapIndex>().myIndex = realIndex;
		SkinsController.SetTextureRecursivelyFrom(obj.transform.GetChild(0).GetChild(0).gameObject, SkinsController.skinsForPers[this.presentSkins[realIndex]], null);
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x0018CF94 File Offset: 0x0018B194
	private void GetPresetSkins()
	{
		this.presentSkins = SkinsController.GetSkinsIdList();
		this.skinPresentsWrap.maxIndex = this.presentSkins.Count - 1;
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x0018CFBC File Offset: 0x0018B1BC
	private void OpenPresetsWindow()
	{
		this.HidePreviewSkin();
		if (!this.presetPreviewInitialized)
		{
			this.presetPreviewInitialized = true;
			GameObject original = Resources.Load("Character_model") as GameObject;
			for (int i = 0; i < this.skinPresentsWrap.transform.childCount; i++)
			{
				CharacterInterface component = (UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<CharacterInterface>();
				component.usePetFromStorager = false;
				component.transform.SetParent(this.skinPresentsWrap.transform.GetChild(i).GetChild(0).GetChild(0), false);
				component.SetSimpleCharacter();
				ShopNGUIController.DisableLightProbesRecursively(component.gameObject);
				Player_move_c.SetLayerRecursively(component.gameObject, this.skinPresentsWrap.transform.GetChild(i).gameObject.layer);
			}
		}
		this.presetsPanel.SetActive(true);
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			this.HandleClosePresetClicked(this, EventArgs.Empty);
		}, "Skin Editor Presets Window");
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x0018D0D8 File Offset: 0x0018B2D8
	private void HandlePresetsButtonClicked(object sender, EventArgs e)
	{
		this.OpenPresetsWindow();
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x0018D0E0 File Offset: 0x0018B2E0
	private void HandleClosePresetClicked(object sender, EventArgs e)
	{
		this.ShowPreviewSkin();
		this.presetsPanel.SetActive(false);
		this.OnEnable();
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x0018D0FC File Offset: 0x0018B2FC
	private void HandleSelectPresetClicked(object sender, EventArgs e)
	{
		GameObject centeredObject = this.skinPresentsWrap.GetComponent<MyCenterOnChild>().centeredObject;
		if (centeredObject == null)
		{
			return;
		}
		this.SetPresetSkin(centeredObject);
		this.HandleClosePresetClicked(null, null);
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x0018D138 File Offset: 0x0018B338
	private void SetPresetSkin(GameObject obj)
	{
		WrapIndex component = obj.GetComponent<WrapIndex>();
		if (component == null)
		{
			return;
		}
		this.SetPresetSkin(component.myIndex);
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x0018D168 File Offset: 0x0018B368
	private void SetPresetSkin(int index)
	{
		SkinEditorController.currentSkin = SkinsController.skinsForPers[this.presentSkins[index]];
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.SetPartsRect();
		this.UpdateTexturesPartsInDictionary();
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x0018D1B0 File Offset: 0x0018B3B0
	private void HandleYesSaveButtonInEditClicked(object sender, EventArgs e)
	{
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.LogoClan)
		{
			Debug.Log("modeEditor==ModeEditor.LogoClan");
			SkinsController.logoClanUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.editorTexture.mainTexture);
		}
		if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape)
		{
			SkinsController.capeUserTexture = EditorTextures.CreateCopyTexture((Texture2D)this.editorTexture.mainTexture);
			string cape = SkinsController.StringFromTexture(SkinsController.capeUserTexture);
			long ticks = DateTime.UtcNow.Ticks;
			CapeMemento capeMemento = new CapeMemento(ticks, cape);
			PlayerPrefs.SetString("NewUserCape", JsonUtility.ToJson(capeMemento));
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				SkinsSynchronizer.Instance.Sync();
			}
			else
			{
				SkinsSynchronizer.Instance.Push();
			}
		}
		SkinEditorController.isEditingPartSkin = false;
		this.HandleBackButtonClicked(null, (SkinEditorController.modeEditor != SkinEditorController.ModeEditor.LogoClan) ? null : new EditorClosingEventArgs
		{
			ClanLogoSaved = true
		});
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x0018D2A0 File Offset: 0x0018B4A0
	private void HandleNoSaveButtonInEditClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingPartSkin = false;
		this.HandleBackButtonClicked(null, null);
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x0018D2B0 File Offset: 0x0018B4B0
	private void HandleOkInSaveSkinClicked(object sender, EventArgs e)
	{
		this.ShowPreviewSkin();
		string text = SkinEditorController.currentSkinName;
		string b = SkinsController.AddUserSkin(this.skinNameInput.value, SkinEditorController.currentSkin, text);
		this.newNameSkin = b;
		SkinsController.SetCurrentSkin(this.newNameSkin);
		SkinEditorController.isEditingSkin = false;
		this.saveSkinPanel.SetActive(false);
		this.HandleBackButtonClicked(null, null);
		if (text != b)
		{
			AnalyticsFacade.SendCustomEventToFacebook("create_custom_skin", null);
		}
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x0018D324 File Offset: 0x0018B524
	private void HandleCancelInSaveSkinClicked(object sender, EventArgs e)
	{
		if (this.isSaveAndExit)
		{
			this.saveSkinPanel.SetActive(false);
			this.leavePanel.SetActive(true);
		}
		else
		{
			this.ShowPreviewSkin();
			this.saveSkinPanel.SetActive(false);
		}
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x0018D36C File Offset: 0x0018B56C
	private void HandleYesInLeaveSaveClicked(object sender, EventArgs e)
	{
		this.leavePanel.SetActive(false);
		this.saveSkinPanel.SetActive(true);
		this.isSaveAndExit = true;
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x0018D390 File Offset: 0x0018B590
	private void HandleNoInLeaveSaveClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingSkin = false;
		this.ShowPreviewSkin();
		this.leavePanel.SetActive(false);
		this.HandleBackButtonClicked(null, null);
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x0018D3C0 File Offset: 0x0018B5C0
	private void ShowPreviewSkin()
	{
		foreach (GameObject gameObject in this.currentPreviewsSkin)
		{
			gameObject.SetActive(true);
		}
		this.backButton.gameObject.GetComponent<UIButton>().isEnabled = true;
		this.saveButton.gameObject.GetComponent<UIButton>().isEnabled = true;
		this.presetsButton.gameObject.GetComponent<UIButton>().isEnabled = true;
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x0018D468 File Offset: 0x0018B668
	private void HidePreviewSkin()
	{
		foreach (GameObject gameObject in this.currentPreviewsSkin)
		{
			gameObject.SetActive(false);
		}
		this.backButton.gameObject.GetComponent<UIButton>().isEnabled = false;
		this.saveButton.gameObject.GetComponent<UIButton>().isEnabled = false;
		this.presetsButton.gameObject.GetComponent<UIButton>().isEnabled = false;
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x0018D510 File Offset: 0x0018B710
	private void HandleSaveChangesButtonClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingPartSkin = false;
		SkinEditorController.isEditingSkin = true;
		this.saveChangesPanel.SetActive(false);
		this.SavePartInTexturesParts(this.selectedPartName);
		SkinEditorController.currentSkin = SkinEditorController.BuildSkin(SkinEditorController.texturesParts);
		SkinsController.SetTextureRecursivelyFrom(this.previewPers, SkinEditorController.currentSkin, null);
		this.UpdateTexturesPartsInDictionary();
		this.HandleBackButtonClicked(null, null);
	}

	// Token: 0x060047DD RID: 18397 RVA: 0x0018D570 File Offset: 0x0018B770
	private void HandleCancelInSaveChangesButtonClicked(object sender, EventArgs e)
	{
		SkinEditorController.isEditingPartSkin = false;
		this.saveChangesPanel.SetActive(false);
		this.HandleBackButtonClicked(null, null);
	}

	// Token: 0x060047DE RID: 18398 RVA: 0x0018D58C File Offset: 0x0018B78C
	private void HandleSelectColorClicked(object sender, EventArgs e)
	{
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			SkinEditorController.brashMode = SkinEditorController.brashModeOld;
			this.pencilButton.gameObject.GetComponent<UIToggle>().value = true;
		}
		this.editorPanel.SetActive(false);
		this.colorPanel.SetActive(true);
		this.oldColor.color = SkinEditorController.colorForPaint;
		this.newColor.color = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x0018D64C File Offset: 0x0018B84C
	private void SetCurrentColor(Color color)
	{
		SkinEditorController.colorForPaint = color;
		this.colorButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.colorButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().defaultColor = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().pressed = SkinEditorController.colorForPaint;
		this.okColorInPalitraButton.gameObject.GetComponent<UIButton>().hover = SkinEditorController.colorForPaint;
		PlayerPrefs.SetFloat("ColorForPaintR", SkinEditorController.colorForPaint.r);
		PlayerPrefs.SetFloat("ColorForPaintG", SkinEditorController.colorForPaint.g);
		PlayerPrefs.SetFloat("ColorForPaintB", SkinEditorController.colorForPaint.b);
		for (int i = 0; i < this.colorOnBrashSprites.Length; i++)
		{
			this.colorOnBrashSprites[i].color = SkinEditorController.colorForPaint;
		}
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x0018D764 File Offset: 0x0018B964
	private void UpdateHistoryColors()
	{
		for (int i = 0; i < SkinEditorController.colorHistory.Length; i++)
		{
			this.colorHistoryButtons[i].gameObject.SetActive(SkinEditorController.colorHistory[i] != Color.clear);
			this.colorHistorySprites[i].color = SkinEditorController.colorHistory[i];
			this.colorHistoryButtons[i].GetComponent<UIButton>().defaultColor = SkinEditorController.colorHistory[i];
			this.colorHistoryButtons[i].GetComponent<UIButton>().pressed = SkinEditorController.colorHistory[i];
			this.colorHistoryButtons[i].GetComponent<UIButton>().hover = SkinEditorController.colorHistory[i];
		}
		this.frameResizer.ResizeFrame();
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x0018D844 File Offset: 0x0018BA44
	private void AddColor(Color color)
	{
		this.SetCurrentColor(color);
		for (int i = 0; i < SkinEditorController.colorHistory.Length; i++)
		{
			if (SkinEditorController.colorHistory[i] == color)
			{
				return;
			}
		}
		for (int j = 1; j < SkinEditorController.colorHistory.Length; j++)
		{
			SkinEditorController.colorHistory[SkinEditorController.colorHistory.Length - j] = SkinEditorController.colorHistory[SkinEditorController.colorHistory.Length - j - 1];
		}
		SkinEditorController.colorHistory[0] = color;
		this.UpdateHistoryColors();
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x0018D8F0 File Offset: 0x0018BAF0
	public void HandleSetColorClicked(object sender, EventArgs e)
	{
		this.editorPanel.SetActive(true);
		this.colorPanel.SetActive(false);
		this.AddColor(this.newColor.color);
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x0018D928 File Offset: 0x0018BB28
	public void HandleSetHistoryColorClicked(object sender, EventArgs e)
	{
		for (int i = 0; i < this.colorHistoryButtons.Length; i++)
		{
			if (this.colorHistoryButtons[i].Equals(sender))
			{
				this.SetCurrentColor(SkinEditorController.colorHistory[i]);
			}
		}
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x0018D978 File Offset: 0x0018BB78
	public void SetColorClickedUp()
	{
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			SkinEditorController.brashMode = SkinEditorController.brashModeOld;
			switch (SkinEditorController.brashMode)
			{
			case SkinEditorController.BrashMode.Pencil:
				this.pencilButton.gameObject.GetComponent<UIToggle>().value = true;
				break;
			case SkinEditorController.BrashMode.Brash:
				this.brashButton.gameObject.GetComponent<UIToggle>().value = true;
				break;
			case SkinEditorController.BrashMode.Eraser:
				SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
				this.pencilButton.gameObject.GetComponent<UIToggle>().value = true;
				break;
			case SkinEditorController.BrashMode.Fill:
				this.fillButton.gameObject.GetComponent<UIToggle>().value = true;
				break;
			}
		}
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x0018DA30 File Offset: 0x0018BC30
	private void HandleSelectBrashClicked(object sender, EventArgs e)
	{
		GameObject gameObject = (sender as MonoBehaviour).gameObject;
		string name = gameObject.name;
		Debug.Log(name);
		if (name.Equals("Fill"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Fill;
		}
		if (name.Equals("Brash"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Brash;
		}
		if (name.Equals("Pencil"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Pencil;
		}
		if (name.Equals("Eraser"))
		{
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Eraser;
		}
		if (name.Equals("Pipette"))
		{
			if (SkinEditorController.brashMode != SkinEditorController.BrashMode.Pipette)
			{
				SkinEditorController.brashModeOld = SkinEditorController.brashMode;
			}
			SkinEditorController.brashMode = SkinEditorController.BrashMode.Pipette;
		}
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x0018DADC File Offset: 0x0018BCDC
	private void HandleSideClicked(object sender, EventArgs e)
	{
		this.selectedSide = (sender as MonoBehaviour).gameObject;
		this.editorPanel.SetActive(true);
		this.partPreviewPanel.SetActive(false);
		this.editorTexture.gameObject.GetComponent<EditorTextures>().SetStartCanvas((Texture2D)this.selectedSide.transform.GetChild(0).GetComponent<UITexture>().mainTexture);
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x0018DB48 File Offset: 0x0018BD48
	private void HandleSaveButtonClicked(object sender, EventArgs e)
	{
		this.isSaveAndExit = false;
		this.saveSkinPanel.SetActive(true);
		this.HidePreviewSkin();
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x0018DB64 File Offset: 0x0018BD64
	private void HandleBackButtonClicked(object sender, EventArgs e)
	{
		if (this.partPreviewPanel.activeSelf)
		{
			if (SkinEditorController.isEditingPartSkin)
			{
				this.saveChangesPanel.SetActive(true);
				this.backButton.gameObject.GetComponent<UIButton>().isEnabled = false;
				this.topPart.GetComponent<UIButton>().isEnabled = false;
				this.downPart.GetComponent<UIButton>().isEnabled = false;
				this.leftPart.GetComponent<UIButton>().isEnabled = false;
				this.frontPart.GetComponent<UIButton>().isEnabled = false;
				this.rigthPart.GetComponent<UIButton>().isEnabled = false;
				this.backPart.GetComponent<UIButton>().isEnabled = false;
			}
			else
			{
				this.partPreviewPanel.SetActive(false);
				this.skinPreviewPanel.SetActive(true);
				this.backButton.gameObject.GetComponent<UIButton>().isEnabled = true;
				this.topPart.GetComponent<UIButton>().isEnabled = true;
				this.downPart.GetComponent<UIButton>().isEnabled = true;
				this.leftPart.GetComponent<UIButton>().isEnabled = true;
				this.frontPart.GetComponent<UIButton>().isEnabled = true;
				this.rigthPart.GetComponent<UIButton>().isEnabled = true;
				this.backPart.GetComponent<UIButton>().isEnabled = true;
			}
			return;
		}
		if (this.editorPanel.activeSelf)
		{
			if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.SkinPers)
			{
				this.editorPanel.SetActive(false);
				this.partPreviewPanel.SetActive(true);
				this.selectedSide.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture((Texture2D)this.editorTexture.mainTexture);
			}
			else if (SkinEditorController.modeEditor == SkinEditorController.ModeEditor.Cape || SkinEditorController.modeEditor == SkinEditorController.ModeEditor.LogoClan)
			{
				if (SkinEditorController.isEditingPartSkin)
				{
					this.savePanelInEditorTexture.SetActive(true);
				}
				else
				{
					this.ExitFromScene(e);
				}
			}
			return;
		}
		if (this.colorPanel.activeSelf)
		{
			this.editorPanel.SetActive(true);
			this.colorPanel.SetActive(false);
			return;
		}
		if (this.skinPreviewPanel.activeSelf)
		{
			if (SkinEditorController.isEditingSkin)
			{
				this.leavePanel.SetActive(true);
				this.HidePreviewSkin();
			}
			else
			{
				this.ExitFromScene(e);
			}
			return;
		}
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x0018DDB0 File Offset: 0x0018BFB0
	private void SavePartInTexturesParts(string _partName)
	{
		Dictionary<string, Texture2D> dictionary = new Dictionary<string, Texture2D>();
		foreach (KeyValuePair<string, Texture2D> keyValuePair in SkinEditorController.texturesParts[_partName])
		{
			if (keyValuePair.Key.Equals("Top"))
			{
				dictionary.Add(keyValuePair.Key, EditorTextures.CreateCopyTexture((Texture2D)this.topPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (keyValuePair.Key.Equals("Down"))
			{
				dictionary.Add(keyValuePair.Key, EditorTextures.CreateCopyTexture((Texture2D)this.downPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (keyValuePair.Key.Equals("Left"))
			{
				dictionary.Add(keyValuePair.Key, EditorTextures.CreateCopyTexture((Texture2D)this.leftPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (keyValuePair.Key.Equals("Front"))
			{
				dictionary.Add(keyValuePair.Key, EditorTextures.CreateCopyTexture((Texture2D)this.frontPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (keyValuePair.Key.Equals("Right"))
			{
				dictionary.Add(keyValuePair.Key, EditorTextures.CreateCopyTexture((Texture2D)this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
			if (keyValuePair.Key.Equals("Back"))
			{
				dictionary.Add(keyValuePair.Key, EditorTextures.CreateCopyTexture((Texture2D)this.backPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture));
			}
		}
		if (_partName.Equals("Arm_right") || _partName.Equals("Arm_left"))
		{
			SkinEditorController.texturesParts.Remove("Arm_right");
			SkinEditorController.texturesParts.Add("Arm_right", dictionary);
			SkinEditorController.texturesParts.Remove("Arm_left");
			SkinEditorController.texturesParts.Add("Arm_left", dictionary);
		}
		if (_partName.Equals("Foot_right") || _partName.Equals("Foot_left"))
		{
			SkinEditorController.texturesParts.Remove("Foot_right");
			SkinEditorController.texturesParts.Add("Foot_right", dictionary);
			SkinEditorController.texturesParts.Remove("Foot_left");
			SkinEditorController.texturesParts.Add("Foot_left", dictionary);
		}
		SkinEditorController.texturesParts.Remove(_partName);
		SkinEditorController.texturesParts.Add(_partName, dictionary);
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x0018E0A4 File Offset: 0x0018C2A4
	private void SetPartsRect()
	{
		Dictionary<string, Rect> dictionary = new Dictionary<string, Rect>();
		SkinEditorController.rectsPartsInSkin.Clear();
		SkinEditorController.ModeEditor modeEditor = SkinEditorController.modeEditor;
		if (modeEditor == SkinEditorController.ModeEditor.SkinPers)
		{
			Dictionary<string, Rect> dictionary2 = new Dictionary<string, Rect>();
			dictionary2.Add("Top", new Rect(8f, 24f, 8f, 8f));
			dictionary2.Add("Down", new Rect(16f, 24f, 8f, 8f));
			dictionary2.Add("Left", new Rect(0f, 16f, 8f, 8f));
			dictionary2.Add("Front", new Rect(8f, 16f, 8f, 8f));
			dictionary2.Add("Right", new Rect(16f, 16f, 8f, 8f));
			dictionary2.Add("Back", new Rect(24f, 16f, 8f, 8f));
			SkinEditorController.rectsPartsInSkin.Add("Head", dictionary2);
			Dictionary<string, Rect> dictionary3 = new Dictionary<string, Rect>();
			dictionary3.Add("Top", new Rect(4f, 12f, 4f, 4f));
			dictionary3.Add("Down", new Rect(8f, 12f, 4f, 4f));
			dictionary3.Add("Left", new Rect(0f, 0f, 4f, 12f));
			dictionary3.Add("Front", new Rect(4f, 0f, 4f, 12f));
			dictionary3.Add("Right", new Rect(8f, 0f, 4f, 12f));
			dictionary3.Add("Back", new Rect(12f, 0f, 4f, 12f));
			SkinEditorController.rectsPartsInSkin.Add("Foot_left", dictionary3);
			Dictionary<string, Rect> dictionary4 = new Dictionary<string, Rect>();
			dictionary4.Add("Top", new Rect(4f, 12f, 4f, 4f));
			dictionary4.Add("Down", new Rect(8f, 12f, 4f, 4f));
			dictionary4.Add("Left", new Rect(0f, 0f, 4f, 12f));
			dictionary4.Add("Front", new Rect(4f, 0f, 4f, 12f));
			dictionary4.Add("Right", new Rect(8f, 0f, 4f, 12f));
			dictionary4.Add("Back", new Rect(12f, 0f, 4f, 12f));
			SkinEditorController.rectsPartsInSkin.Add("Foot_right", dictionary4);
			Dictionary<string, Rect> dictionary5 = new Dictionary<string, Rect>();
			dictionary5.Add("Top", new Rect(20f, 12f, 8f, 4f));
			dictionary5.Add("Down", new Rect(28f, 12f, 8f, 4f));
			dictionary5.Add("Left", new Rect(16f, 0f, 4f, 12f));
			dictionary5.Add("Front", new Rect(20f, 0f, 8f, 12f));
			dictionary5.Add("Right", new Rect(28f, 0f, 4f, 12f));
			dictionary5.Add("Back", new Rect(32f, 0f, 8f, 12f));
			SkinEditorController.rectsPartsInSkin.Add("Body", dictionary5);
			Dictionary<string, Rect> dictionary6 = new Dictionary<string, Rect>();
			dictionary6.Add("Top", new Rect(44f, 12f, 4f, 4f));
			dictionary6.Add("Down", new Rect(48f, 12f, 4f, 4f));
			dictionary6.Add("Left", new Rect(40f, 0f, 4f, 12f));
			dictionary6.Add("Front", new Rect(44f, 0f, 4f, 12f));
			dictionary6.Add("Right", new Rect(48f, 0f, 4f, 12f));
			dictionary6.Add("Back", new Rect(52f, 0f, 4f, 12f));
			SkinEditorController.rectsPartsInSkin.Add("Arm_right", dictionary6);
			Dictionary<string, Rect> dictionary7 = new Dictionary<string, Rect>();
			dictionary7.Add("Top", new Rect(44f, 12f, 4f, 4f));
			dictionary7.Add("Down", new Rect(48f, 12f, 4f, 4f));
			dictionary7.Add("Left", new Rect(40f, 0f, 4f, 12f));
			dictionary7.Add("Front", new Rect(44f, 0f, 4f, 12f));
			dictionary7.Add("Right", new Rect(48f, 0f, 4f, 12f));
			dictionary7.Add("Back", new Rect(52f, 0f, 4f, 12f));
			SkinEditorController.rectsPartsInSkin.Add("Arm_left", dictionary7);
		}
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x0018E68C File Offset: 0x0018C88C
	public void UpdateTexturesPartsInDictionary()
	{
		SkinEditorController.texturesParts.Clear();
		foreach (KeyValuePair<string, Dictionary<string, Rect>> keyValuePair in SkinEditorController.rectsPartsInSkin)
		{
			Dictionary<string, Texture2D> dictionary = new Dictionary<string, Texture2D>();
			foreach (KeyValuePair<string, Rect> keyValuePair2 in SkinEditorController.rectsPartsInSkin[keyValuePair.Key])
			{
				dictionary.Add(keyValuePair2.Key, this.TextureFromRect(SkinEditorController.currentSkin, SkinEditorController.rectsPartsInSkin[keyValuePair.Key][keyValuePair2.Key]));
			}
			SkinEditorController.texturesParts.Add(keyValuePair.Key, dictionary);
		}
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x0018E79C File Offset: 0x0018C99C
	public static Texture2D BuildSkin(Dictionary<string, Dictionary<string, Texture2D>> _texturesParts)
	{
		int width = SkinEditorController.currentSkin.width;
		int height = SkinEditorController.currentSkin.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
		Color clear = Color.clear;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				texture2D.SetPixel(j, i, clear);
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, Texture2D>> keyValuePair in _texturesParts)
		{
			foreach (KeyValuePair<string, Texture2D> keyValuePair2 in _texturesParts[keyValuePair.Key])
			{
				texture2D.SetPixels(Mathf.RoundToInt(SkinEditorController.rectsPartsInSkin[keyValuePair.Key][keyValuePair2.Key].x), Mathf.RoundToInt(SkinEditorController.rectsPartsInSkin[keyValuePair.Key][keyValuePair2.Key].y), Mathf.RoundToInt(SkinEditorController.rectsPartsInSkin[keyValuePair.Key][keyValuePair2.Key].width), Mathf.RoundToInt(SkinEditorController.rectsPartsInSkin[keyValuePair.Key][keyValuePair2.Key].height), _texturesParts[keyValuePair.Key][keyValuePair2.Key].GetPixels());
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x0018E98C File Offset: 0x0018CB8C
	public void SelectPart(string _partName)
	{
		if (!SkinEditorController.texturesParts.ContainsKey(_partName))
		{
			Debug.Log("texturesParts not contain key");
			return;
		}
		SkinEditorController.isEditingPartSkin = false;
		this.selectedPartName = _partName;
		this.topPart.SetActive(false);
		this.downPart.SetActive(false);
		this.leftPart.SetActive(false);
		this.frontPart.SetActive(false);
		this.rigthPart.SetActive(false);
		this.backPart.SetActive(false);
		int num = 22;
		foreach (KeyValuePair<string, Texture2D> keyValuePair in SkinEditorController.texturesParts[_partName])
		{
			if (keyValuePair.Key.Equals("Top"))
			{
				this.topPart.SetActive(true);
				this.topPart.GetComponent<BoxCollider>().size = new Vector3((float)(keyValuePair.Value.width * num), (float)(keyValuePair.Value.height * num), 0f);
				this.topPart.transform.GetChild(0).GetComponent<UITexture>().width = keyValuePair.Value.width * num;
				this.topPart.transform.GetChild(0).GetComponent<UITexture>().height = keyValuePair.Value.height * num;
				this.topPart.transform.localPosition = new Vector3((float)(-(float)keyValuePair.Value.width) * 0.5f * (float)num, (float)(SkinEditorController.texturesParts[_partName]["Front"].height + keyValuePair.Value.height) * 0.5f * (float)num, 0f);
				this.topPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(keyValuePair.Value);
			}
			if (keyValuePair.Key.Equals("Down"))
			{
				this.downPart.SetActive(true);
				this.downPart.GetComponent<BoxCollider>().size = new Vector3((float)(keyValuePair.Value.width * num), (float)(keyValuePair.Value.height * num), 0f);
				this.downPart.transform.GetChild(0).GetComponent<UITexture>().width = keyValuePair.Value.width * num;
				this.downPart.transform.GetChild(0).GetComponent<UITexture>().height = keyValuePair.Value.height * num;
				this.downPart.transform.localPosition = new Vector3((float)(-(float)keyValuePair.Value.width) * 0.5f * (float)num, (float)(-(float)(SkinEditorController.texturesParts[_partName]["Front"].height + keyValuePair.Value.height)) * 0.5f * (float)num, 0f);
				this.downPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(keyValuePair.Value);
			}
			if (keyValuePair.Key.Equals("Left"))
			{
				this.leftPart.SetActive(true);
				this.leftPart.GetComponent<BoxCollider>().size = new Vector3((float)(keyValuePair.Value.width * num), (float)(keyValuePair.Value.height * num), 0f);
				this.leftPart.transform.GetChild(0).GetComponent<UITexture>().width = keyValuePair.Value.width * num;
				this.leftPart.transform.GetChild(0).GetComponent<UITexture>().height = keyValuePair.Value.height * num;
				this.leftPart.transform.localPosition = new Vector3(-((float)keyValuePair.Value.width * 0.5f + (float)SkinEditorController.texturesParts[_partName]["Front"].width) * (float)num, 0f, 0f);
				this.leftPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(keyValuePair.Value);
			}
			if (keyValuePair.Key.Equals("Front"))
			{
				this.frontPart.SetActive(true);
				this.frontPart.GetComponent<BoxCollider>().size = new Vector3((float)(keyValuePair.Value.width * num), (float)(keyValuePair.Value.height * num), 0f);
				this.frontPart.transform.GetChild(0).GetComponent<UITexture>().width = keyValuePair.Value.width * num;
				this.frontPart.transform.GetChild(0).GetComponent<UITexture>().height = keyValuePair.Value.height * num;
				this.frontPart.transform.localPosition = new Vector3(-((float)keyValuePair.Value.width * 0.5f) * (float)num, 0f, 0f);
				this.frontPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(keyValuePair.Value);
			}
			if (keyValuePair.Key.Equals("Right"))
			{
				this.rigthPart.SetActive(true);
				this.rigthPart.GetComponent<BoxCollider>().size = new Vector3((float)(keyValuePair.Value.width * num), (float)(keyValuePair.Value.height * num), 0f);
				this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().width = keyValuePair.Value.width * num;
				this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().height = keyValuePair.Value.height * num;
				this.rigthPart.transform.localPosition = new Vector3((float)keyValuePair.Value.width * 0.5f * (float)num, 0f, 0f);
				this.rigthPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(keyValuePair.Value);
			}
			if (keyValuePair.Key.Equals("Back"))
			{
				this.backPart.SetActive(true);
				this.backPart.GetComponent<BoxCollider>().size = new Vector3((float)(keyValuePair.Value.width * num), (float)(keyValuePair.Value.height * num), 0f);
				this.backPart.transform.GetChild(0).GetComponent<UITexture>().width = keyValuePair.Value.width * num;
				this.backPart.transform.GetChild(0).GetComponent<UITexture>().height = keyValuePair.Value.height * num;
				this.backPart.transform.localPosition = new Vector3(((float)keyValuePair.Value.width * 0.5f + (float)SkinEditorController.texturesParts[_partName]["Right"].width) * (float)num, 0f, 0f);
				this.backPart.transform.GetChild(0).GetComponent<UITexture>().mainTexture = EditorTextures.CreateCopyTexture(keyValuePair.Value);
			}
		}
		this.partPreviewPanel.SetActive(true);
		this.skinPreviewPanel.SetActive(false);
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x0018F140 File Offset: 0x0018D340
	private Texture2D TextureFromRect(Texture2D texForCut, Rect rectForCut)
	{
		Color[] pixels = texForCut.GetPixels((int)rectForCut.x, (int)rectForCut.y, (int)rectForCut.width, (int)rectForCut.height);
		Texture2D texture2D = new Texture2D((int)rectForCut.width, (int)rectForCut.height);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x0018F1A0 File Offset: 0x0018D3A0
	private void ExitFromScene(EventArgs e = null)
	{
		if (SkinEditorController.ExitFromSkinEditor != null)
		{
			SkinEditorController.ExitFromSkinEditor((SkinEditorController.modeEditor != SkinEditorController.ModeEditor.LogoClan || e == null || !(e is EditorClosingEventArgs) || !(e as EditorClosingEventArgs).ClanLogoSaved) ? this.newNameSkin : "SAVED");
			SkinEditorController.currentSkinName = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		if (SkinEditorController.modeEditor != SkinEditorController.ModeEditor.LogoClan && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x0018F234 File Offset: 0x0018D434
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			this.HandleBackButtonClicked(this, EventArgs.Empty);
		}, "Skin Editor");
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x0018F270 File Offset: 0x0018D470
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x0018F290 File Offset: 0x0018D490
	private void OnDestroy()
	{
		SkinEditorController.sharedController = null;
	}

	// Token: 0x040034E0 RID: 13536
	public static Color colorForPaint = new Color(0f, 1f, 0f, 1f);

	// Token: 0x040034E1 RID: 13537
	public static Color[] colorHistory = new Color[]
	{
		Color.clear,
		Color.clear,
		Color.clear,
		Color.clear,
		Color.clear
	};

	// Token: 0x040034E2 RID: 13538
	public static SkinEditorController.BrashMode brashMode = SkinEditorController.BrashMode.Pencil;

	// Token: 0x040034E3 RID: 13539
	public static SkinEditorController.BrashMode brashModeOld = SkinEditorController.BrashMode.Pencil;

	// Token: 0x040034E4 RID: 13540
	public GameObject topPart;

	// Token: 0x040034E5 RID: 13541
	public GameObject downPart;

	// Token: 0x040034E6 RID: 13542
	public GameObject leftPart;

	// Token: 0x040034E7 RID: 13543
	public GameObject frontPart;

	// Token: 0x040034E8 RID: 13544
	public GameObject rigthPart;

	// Token: 0x040034E9 RID: 13545
	public GameObject backPart;

	// Token: 0x040034EA RID: 13546
	public static SkinEditorController.ModeEditor modeEditor = SkinEditorController.ModeEditor.SkinPers;

	// Token: 0x040034EB RID: 13547
	public static SkinEditorController sharedController = null;

	// Token: 0x040034EC RID: 13548
	public ButtonHandler saveButton;

	// Token: 0x040034ED RID: 13549
	public ButtonHandler backButton;

	// Token: 0x040034EE RID: 13550
	public ButtonHandler fillButton;

	// Token: 0x040034EF RID: 13551
	public ButtonHandler eraserButton;

	// Token: 0x040034F0 RID: 13552
	public ButtonHandler brashButton;

	// Token: 0x040034F1 RID: 13553
	public ButtonHandler pencilButton;

	// Token: 0x040034F2 RID: 13554
	public ButtonHandler pipetteButton;

	// Token: 0x040034F3 RID: 13555
	public ButtonHandler colorButton;

	// Token: 0x040034F4 RID: 13556
	public ButtonHandler okColorInPalitraButton;

	// Token: 0x040034F5 RID: 13557
	public ButtonHandler setColorButton;

	// Token: 0x040034F6 RID: 13558
	public ButtonHandler saveChangesButton;

	// Token: 0x040034F7 RID: 13559
	public ButtonHandler cancelInSaveChangesButton;

	// Token: 0x040034F8 RID: 13560
	public ButtonHandler okInSaveSkin;

	// Token: 0x040034F9 RID: 13561
	public ButtonHandler cancelInSaveSkin;

	// Token: 0x040034FA RID: 13562
	public ButtonHandler yesInLeaveSave;

	// Token: 0x040034FB RID: 13563
	public ButtonHandler noInLeaveSave;

	// Token: 0x040034FC RID: 13564
	public ButtonHandler prevHistoryButton;

	// Token: 0x040034FD RID: 13565
	public ButtonHandler nextHistoryButton;

	// Token: 0x040034FE RID: 13566
	public ButtonHandler yesSaveButtonInEdit;

	// Token: 0x040034FF RID: 13567
	public ButtonHandler noSaveButtonInEdit;

	// Token: 0x04003500 RID: 13568
	public ButtonHandler presetsButton;

	// Token: 0x04003501 RID: 13569
	public ButtonHandler closePresetPanelButton;

	// Token: 0x04003502 RID: 13570
	public ButtonHandler selectPresetButton;

	// Token: 0x04003503 RID: 13571
	public ButtonHandler centeredPresetButton;

	// Token: 0x04003504 RID: 13572
	public GameObject previewPers;

	// Token: 0x04003505 RID: 13573
	public GameObject previewPersShadow;

	// Token: 0x04003506 RID: 13574
	public GameObject skinPreviewPanel;

	// Token: 0x04003507 RID: 13575
	public GameObject partPreviewPanel;

	// Token: 0x04003508 RID: 13576
	public GameObject editorPanel;

	// Token: 0x04003509 RID: 13577
	public GameObject colorPanel;

	// Token: 0x0400350A RID: 13578
	public GameObject saveChangesPanel;

	// Token: 0x0400350B RID: 13579
	public GameObject saveSkinPanel;

	// Token: 0x0400350C RID: 13580
	public GameObject leavePanel;

	// Token: 0x0400350D RID: 13581
	public GameObject savePanelInEditorTexture;

	// Token: 0x0400350E RID: 13582
	public GameObject presetsPanel;

	// Token: 0x0400350F RID: 13583
	public string selectedPartName;

	// Token: 0x04003510 RID: 13584
	public GameObject selectedSide;

	// Token: 0x04003511 RID: 13585
	public static Texture2D currentSkin = null;

	// Token: 0x04003512 RID: 13586
	public static string currentSkinName = null;

	// Token: 0x04003513 RID: 13587
	public static Dictionary<string, Dictionary<string, Rect>> rectsPartsInSkin = new Dictionary<string, Dictionary<string, Rect>>();

	// Token: 0x04003514 RID: 13588
	public static Dictionary<string, Dictionary<string, Texture2D>> texturesParts = new Dictionary<string, Dictionary<string, Texture2D>>();

	// Token: 0x04003515 RID: 13589
	public UILabel pensilLabel;

	// Token: 0x04003516 RID: 13590
	public UILabel brashLabel;

	// Token: 0x04003517 RID: 13591
	public UILabel eraserLabel;

	// Token: 0x04003518 RID: 13592
	public UILabel fillLabel;

	// Token: 0x04003519 RID: 13593
	public UILabel pipetteLabel;

	// Token: 0x0400351A RID: 13594
	public UITexture editorTexture;

	// Token: 0x0400351B RID: 13595
	public UISprite oldColor;

	// Token: 0x0400351C RID: 13596
	public UISprite newColor;

	// Token: 0x0400351D RID: 13597
	public UISprite[] colorHistorySprites;

	// Token: 0x0400351E RID: 13598
	public ButtonHandler[] colorHistoryButtons;

	// Token: 0x0400351F RID: 13599
	public UIInput skinNameInput;

	// Token: 0x04003520 RID: 13600
	public static bool isEditingPartSkin = false;

	// Token: 0x04003521 RID: 13601
	public static bool isEditingSkin = false;

	// Token: 0x04003522 RID: 13602
	public bool isSaveAndExit;

	// Token: 0x04003523 RID: 13603
	private List<GameObject> currentPreviewsSkin = new List<GameObject>();

	// Token: 0x04003524 RID: 13604
	private string newNameSkin;

	// Token: 0x04003525 RID: 13605
	public UISprite[] colorOnBrashSprites;

	// Token: 0x04003526 RID: 13606
	public FrameResizer frameResizer;

	// Token: 0x04003527 RID: 13607
	public UIWrapContent skinPresentsWrap;

	// Token: 0x04003528 RID: 13608
	private List<string> presentSkins = SkinsController.GetSkinsIdList();

	// Token: 0x04003529 RID: 13609
	private CharacterInterface characterInterface;

	// Token: 0x0400352A RID: 13610
	private bool presetPreviewInitialized;

	// Token: 0x0400352B RID: 13611
	private IDisposable _backSubscription;

	// Token: 0x020007BF RID: 1983
	public enum ModeEditor
	{
		// Token: 0x0400352E RID: 13614
		SkinPers,
		// Token: 0x0400352F RID: 13615
		Cape,
		// Token: 0x04003530 RID: 13616
		LogoClan
	}

	// Token: 0x020007C0 RID: 1984
	public enum BrashMode
	{
		// Token: 0x04003532 RID: 13618
		Pencil,
		// Token: 0x04003533 RID: 13619
		Brash,
		// Token: 0x04003534 RID: 13620
		Eraser,
		// Token: 0x04003535 RID: 13621
		Fill,
		// Token: 0x04003536 RID: 13622
		Pipette
	}
}
