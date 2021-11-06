using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x02000006 RID: 6
public sealed class ActivityIndicator : MonoBehaviour
{
	// Token: 0x0600000E RID: 14 RVA: 0x000028B0 File Offset: 0x00000AB0
	public void Awake()
	{
		ActivityIndicator.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.vectRotateSpeed = new Vector3(0f, this.rotSpeed, 0f);
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			base.gameObject.AddComponent<PurchasesSynchronizerListener>();
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002914 File Offset: 0x00000B14
	private void Start()
	{
		this.OnEnable();
		this.lbLoading.GetComponent<Localize>().enabled = true;
		if (Launcher.UsingNewLauncher)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002950 File Offset: 0x00000B50
	private void Update()
	{
		if (this.objIndicator != null)
		{
			this.objIndicator.transform.Rotate(this.vectRotateSpeed * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002984 File Offset: 0x00000B84
	private void OnDestroy()
	{
		ActivityIndicator.instance = null;
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000029A0 File Offset: 0x00000BA0
	private void HandleLocalizationChanged()
	{
		if (this.lbLoading != null)
		{
			this.text = LocalizationStore.Get("Key_0853");
			this.lbLoading.text = this.text;
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000029E0 File Offset: 0x00000BE0
	private void OnEnable()
	{
		this.HandleLocalizationChanged();
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000014 RID: 20 RVA: 0x000029E8 File Offset: 0x00000BE8
	// (set) Token: 0x06000015 RID: 21 RVA: 0x000029F0 File Offset: 0x00000BF0
	public static float LoadingProgress
	{
		get
		{
			return ActivityIndicator.curPers;
		}
		set
		{
			if (ActivityIndicator.instance != null)
			{
				ActivityIndicator.curPers = value;
				ActivityIndicator.curPers = Mathf.Clamp01(ActivityIndicator.curPers);
				if (ActivityIndicator.curPers < 0f)
				{
					ActivityIndicator.curPers = 0f;
				}
				if (ActivityIndicator.curPers > 1f)
				{
					ActivityIndicator.curPers = 1f;
				}
				if (ActivityIndicator.instance.txProgressBar != null)
				{
					ActivityIndicator.instance.txProgressBar.fillAmount = ActivityIndicator.curPers;
				}
				if (ActivityIndicator.instance.lbPercentLoading)
				{
					ActivityIndicator.instance.lbPercentLoading.text = string.Format("{0}%", Mathf.RoundToInt(ActivityIndicator.curPers * 100f));
				}
			}
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002AC0 File Offset: 0x00000CC0
	public static void SetLoadingFon(Texture needFon)
	{
		if (ActivityIndicator.instance == null)
		{
			return;
		}
		if (ActivityIndicator.instance.txFon[0] == null)
		{
			return;
		}
		ActivityIndicator.instance.txFon[0].mainTexture = needFon;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002B00 File Offset: 0x00000D00
	public IEnumerable<float> ReplaceLoadingFon(Texture needFon, float duration)
	{
		this.txFon[1].mainTexture = needFon;
		this.txFon[1].alpha = 0f;
		float _curDuration = 0f;
		yield return 0f;
		while (_curDuration < duration)
		{
			_curDuration += Time.deltaTime;
			float _alpha = _curDuration / duration;
			Mathf.Min(_alpha, 1f);
			this.txFon[1].alpha = _alpha;
			yield return _alpha;
		}
		this.txFon[1].mainTexture = null;
		this.txFon[0].mainTexture = needFon;
		yield break;
	}

	// Token: 0x17000002 RID: 2
	// (set) Token: 0x06000018 RID: 24 RVA: 0x00002B40 File Offset: 0x00000D40
	public static bool IsShowWindowLoading
	{
		set
		{
			if (ActivityIndicator.instance != null)
			{
				if (!value && ActivityIndicator.instance.txFon != null)
				{
					ActivityIndicator.instance.txFon[0].mainTexture = null;
				}
				if (ActivityIndicator.instance.panelWindowLoading != null)
				{
					ActivityIndicator.instance.panelWindowLoading.SetActive(value);
				}
			}
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000019 RID: 25 RVA: 0x00002BAC File Offset: 0x00000DAC
	// (set) Token: 0x0600001A RID: 26 RVA: 0x00002BF0 File Offset: 0x00000DF0
	public static bool IsActiveIndicator
	{
		get
		{
			return !(ActivityIndicator.instance == null) && !(ActivityIndicator.instance.panelIndicator == null) && ActivityIndicator.instance.panelIndicator.activeSelf;
		}
		set
		{
			if (ActivityIndicator.instance == null)
			{
				return;
			}
			if (ActivityIndicator.instance.panelIndicator != null)
			{
				ActivityIndicator.instance.panelIndicator.SetActive(value);
			}
			if (ActivityIndicator.instance.needCam != null)
			{
				ActivityIndicator.instance.needCam.Render();
			}
			if (!value)
			{
				ActivityIndicator.instance.HandleLocalizationChanged();
			}
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002C68 File Offset: 0x00000E68
	public static void SetActiveWithCaption(string caption)
	{
		if (ActivityIndicator.instance != null && ActivityIndicator.instance.lbLoading != null)
		{
			ActivityIndicator.instance.lbLoading.text = (caption ?? string.Empty);
		}
		ActivityIndicator.IsActiveIndicator = true;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002CBC File Offset: 0x00000EBC
	public static void ClearMemory()
	{
		if (ActivityIndicator.instance != null && ActivityIndicator.instance.canClearMemory)
		{
			ActivityIndicator.instance.StartCoroutine(ActivityIndicator.instance.Crt_ClearMemory());
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002D00 File Offset: 0x00000F00
	private IEnumerator Crt_ClearMemory()
	{
		if (this.canClearMemory)
		{
			this.canClearMemory = false;
			yield return null;
			meminfo.gc_Collect();
			yield return null;
			Resources.UnloadUnusedAssets();
			yield return null;
			this.canClearMemory = true;
		}
		yield break;
	}

	// Token: 0x0400000F RID: 15
	internal const string DefaultLegendLabel = "PLEASE REBOOT YOUR DEVICE IF FROZEN.";

	// Token: 0x04000010 RID: 16
	public static ActivityIndicator instance;

	// Token: 0x04000011 RID: 17
	public float rotSpeed = 180f;

	// Token: 0x04000012 RID: 18
	private Vector3 vectRotateSpeed;

	// Token: 0x04000013 RID: 19
	public string text;

	// Token: 0x04000014 RID: 20
	public Camera needCam;

	// Token: 0x04000015 RID: 21
	public GameObject panelWindowLoading;

	// Token: 0x04000016 RID: 22
	public GameObject panelIndicator;

	// Token: 0x04000017 RID: 23
	public GameObject objIndicator;

	// Token: 0x04000018 RID: 24
	public GameObject panelProgress;

	// Token: 0x04000019 RID: 25
	public UILabel lbLoading;

	// Token: 0x0400001A RID: 26
	public UILabel lbPercentLoading;

	// Token: 0x0400001B RID: 27
	public UILabel legendLabel;

	// Token: 0x0400001C RID: 28
	public UITexture[] txFon;

	// Token: 0x0400001D RID: 29
	public UITexture txProgressBar;

	// Token: 0x0400001E RID: 30
	private static float curPers;

	// Token: 0x0400001F RID: 31
	private bool canClearMemory = true;
}
