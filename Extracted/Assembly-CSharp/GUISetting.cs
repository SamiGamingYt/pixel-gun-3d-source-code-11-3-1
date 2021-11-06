using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000152 RID: 338
public sealed class GUISetting : MonoBehaviour
{
	// Token: 0x06000B46 RID: 2886 RVA: 0x0003FCD0 File Offset: 0x0003DED0
	private void Start()
	{
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x0003FCD4 File Offset: 0x0003DED4
	private void Update()
	{
		ActivityIndicator.IsActiveIndicator = StoreKitEventListener.purchaseInProcess;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0003FCE0 File Offset: 0x0003DEE0
	private void OnGUI()
	{
		GUI.depth = 2;
		float num = (float)Screen.height / 768f;
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 683f * num, 0f, 1366f * num, (float)Screen.height), this.fon);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)this.settingPlashka.width * num * 0.5f, (float)Screen.height * 0.52f - (float)this.settingPlashka.height * num * 0.5f, (float)this.settingPlashka.width * num, (float)this.settingPlashka.height * num), this.settingPlashka);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)this.settingTitle.width / 2f * Defs.Coef, (float)Screen.height * 0.08f, (float)this.settingTitle.width * Defs.Coef, (float)this.settingTitle.height * Defs.Coef), this.settingTitle);
		Rect position = new Rect((float)Screen.width * 0.5f - (float)this.soundOnOff.normal.background.width * 0.5f * num, (float)Screen.height * 0.52f - (float)this.soundOnOff.normal.background.height * 0.5f * num, (float)this.soundOnOff.normal.background.width * num, (float)this.soundOnOff.normal.background.height * num);
		bool flag = PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true);
		flag = GUI.Toggle(position, flag, string.Empty, this.soundOnOff);
		AudioListener.volume = (float)((!flag) ? 0 : 1);
		PlayerPrefsX.SetBool(PlayerPrefsX.SndSetting, flag);
		PlayerPrefs.Save();
		Rect position2 = new Rect((float)Screen.width * 0.5f - (float)this.soundOnOff.normal.background.width * 0.5f * num, (float)Screen.height * 0.72f - (float)this.soundOnOff.normal.background.height * 0.5f * num, (float)this.soundOnOff.normal.background.width * num, (float)this.soundOnOff.normal.background.height * num);
		bool flag2 = Defs.IsChatOn;
		flag2 = GUI.Toggle(position2, flag2, string.Empty, this.soundOnOff);
		Defs.IsChatOn = flag2;
		PlayerPrefs.Save();
		if (GUI.Button(new Rect(21f * num, (float)Screen.height - (21f + (float)this.back.normal.background.height) * num, (float)this.back.normal.background.width * num, (float)this.back.normal.background.height * num), string.Empty, this.back))
		{
			Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene, LoadSceneMode.Single);
		}
		GUI.enabled = !StoreKitEventListener.purchaseInProcess;
		Rect position3 = new Rect((float)Screen.width / 2f - (float)this.restore.normal.background.width * num * 0.5f, (float)Screen.height - (21f + (float)this.restore.normal.background.height) * num, (float)this.restore.normal.background.width * num, (float)this.restore.normal.background.height * num);
		if (GUI.Button(position3, string.Empty, this.restore))
		{
			StoreKitEventListener.purchaseInProcess = true;
		}
		GUI.enabled = true;
		this.sliderStyle.fixedWidth = (float)this.slow_fast.width * num;
		this.sliderStyle.fixedHeight = (float)this.slow_fast.height * num;
		this.thumbStyle.fixedWidth = (float)this.polzunok.width * num;
		this.thumbStyle.fixedHeight = (float)this.polzunok.height * num;
		Rect position4 = new Rect((float)Screen.width * 0.5f - (float)this.slow_fast.width * 0.5f * num, (float)Screen.height * 0.81f - (float)this.slow_fast.height * 0.5f * num, (float)this.slow_fast.width * num, (float)this.slow_fast.height * num);
		this.mySens = GUI.HorizontalSlider(position4, Defs.Sensitivity, 6f, 18f, this.sliderStyle, this.thumbStyle);
		Defs.Sensitivity = this.mySens;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x000401C8 File Offset: 0x0003E3C8
	private void OnDestroy()
	{
		ActivityIndicator.IsActiveIndicator = false;
	}

	// Token: 0x040008CA RID: 2250
	public GUIStyle back;

	// Token: 0x040008CB RID: 2251
	public GUIStyle soundOnOff;

	// Token: 0x040008CC RID: 2252
	public GUIStyle restore;

	// Token: 0x040008CD RID: 2253
	public GUIStyle sliderStyle;

	// Token: 0x040008CE RID: 2254
	public GUIStyle thumbStyle;

	// Token: 0x040008CF RID: 2255
	public Texture settingPlashka;

	// Token: 0x040008D0 RID: 2256
	public Texture settingTitle;

	// Token: 0x040008D1 RID: 2257
	public Texture fon;

	// Token: 0x040008D2 RID: 2258
	public Texture slow_fast;

	// Token: 0x040008D3 RID: 2259
	public Texture polzunok;

	// Token: 0x040008D4 RID: 2260
	private float mySens;
}
