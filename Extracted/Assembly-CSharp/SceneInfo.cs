using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000749 RID: 1865
[ExecuteInEditMode]
public sealed class SceneInfo : MonoBehaviour
{
	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x06004180 RID: 16768 RVA: 0x0015CEB8 File Offset: 0x0015B0B8
	public bool IsLoaded
	{
		get
		{
			if (this._isLoaded)
			{
				return true;
			}
			this.UpdateKeyLoaded();
			return this._isLoaded;
		}
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x0015CED4 File Offset: 0x0015B0D4
	public void UpdateKeyLoaded()
	{
		if (this.isPreloading)
		{
			this._isLoaded = false;
		}
		else
		{
			this._isLoaded = true;
		}
	}

	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x06004182 RID: 16770 RVA: 0x0015CEF4 File Offset: 0x0015B0F4
	public string NameScene
	{
		get
		{
			return base.gameObject.name;
		}
	}

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x06004183 RID: 16771 RVA: 0x0015CF04 File Offset: 0x0015B104
	public bool IsAvaliableVersion
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x0015CF08 File Offset: 0x0015B108
	public bool IsAvaliableForMode(TypeModeGame curMode)
	{
		if (this.IsAvaliableVersion && this.avaliableInModes != null && this.avaliableInModes.Count > 0)
		{
			for (int i = 0; i < this.avaliableInModes.Count; i++)
			{
				if (curMode == this.avaliableInModes[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x0015CF70 File Offset: 0x0015B170
	public void AddMode(TypeModeGame curMode)
	{
		for (int i = 0; i < this.avaliableInModes.Count; i++)
		{
			if (curMode == this.avaliableInModes[i])
			{
				return;
			}
		}
		this.avaliableInModes.Add(curMode);
	}

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x06004186 RID: 16774 RVA: 0x0015CFB8 File Offset: 0x0015B1B8
	public Sounds GetBackgroundSound
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x06004187 RID: 16775 RVA: 0x0015CFBC File Offset: 0x0015B1BC
	public string TranslateName
	{
		get
		{
			return this.transName;
		}
	}

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x06004188 RID: 16776 RVA: 0x0015CFC4 File Offset: 0x0015B1C4
	public string TranslatePreviewName
	{
		get
		{
			return this.transShortName;
		}
	}

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x06004189 RID: 16777 RVA: 0x0015CFCC File Offset: 0x0015B1CC
	public string TranslateEngShortName
	{
		get
		{
			return this.transShortName;
		}
	}

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x0600418A RID: 16778 RVA: 0x0015CFD4 File Offset: 0x0015B1D4
	public string KeyTranslateSizeMap
	{
		get
		{
			switch (this.sizeMap)
			{
			case InfoSizeMap.small:
				return "Key_0541";
			case InfoSizeMap.normal:
				return "Key_0539";
			case InfoSizeMap.big:
				return "Key_0538";
			case InfoSizeMap.veryBig:
				return "Key_0540";
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x0600418B RID: 16779 RVA: 0x0015D020 File Offset: 0x0015B220
	public string TranslateSizeMap
	{
		get
		{
			return this.transSizeMap;
		}
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x0015D028 File Offset: 0x0015B228
	public void UpdateLocalize()
	{
		this.transName = LocalizationStore.Get(this.keyTranslateName);
		this.transShortName = LocalizationStore.Get(this.keyTranslateShortName);
		this.transSizeMap = LocalizationStore.Get(this.KeyTranslateSizeMap);
		this.transEngShortName = LocalizationStore.GetByDefault(this.keyTranslateShortName);
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x0015D07C File Offset: 0x0015B27C
	public void SetStartPositionCamera(GameObject curCamObj)
	{
		if (curCamObj != null)
		{
			curCamObj.transform.position = this.positionCam;
			curCamObj.transform.eulerAngles = this.rotationCam;
		}
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x0015D0B8 File Offset: 0x0015B2B8
	[ContextMenu("Set Next Index")]
	private void DoSomething()
	{
		int num = 0;
		UnityEngine.Object[] array = Resources.LoadAll("SceneInfo");
		foreach (UnityEngine.Object @object in array)
		{
			SceneInfo component = (@object as GameObject).GetComponent<SceneInfo>();
			if (component.indexMap > num)
			{
				num = component.indexMap;
			}
		}
		this.indexMap = num + 1;
	}

	// Token: 0x04002FD8 RID: 12248
	[Header("Parametr map")]
	public int indexMap;

	// Token: 0x04002FD9 RID: 12249
	public ModeWeapon AvaliableWeapon;

	// Token: 0x04002FDA RID: 12250
	public List<TypeModeGame> avaliableInModes = new List<TypeModeGame>();

	// Token: 0x04002FDB RID: 12251
	public string minAvaliableVersion = "0.0.0.0";

	// Token: 0x04002FDC RID: 12252
	public string maxAvaliableVersion = "0.0.0.0";

	// Token: 0x04002FDD RID: 12253
	public bool isPremium;

	// Token: 0x04002FDE RID: 12254
	public bool isPreloading;

	// Token: 0x04002FDF RID: 12255
	private bool _isLoaded;

	// Token: 0x04002FE0 RID: 12256
	public InfoSizeMap sizeMap;

	// Token: 0x04002FE1 RID: 12257
	[Header("Number key for translate")]
	public string keyTranslateName = string.Empty;

	// Token: 0x04002FE2 RID: 12258
	private string transName = string.Empty;

	// Token: 0x04002FE3 RID: 12259
	public string keyTranslateShortName = string.Empty;

	// Token: 0x04002FE4 RID: 12260
	private string transShortName = string.Empty;

	// Token: 0x04002FE5 RID: 12261
	private string transEngShortName = string.Empty;

	// Token: 0x04002FE6 RID: 12262
	private string transSizeMap = string.Empty;

	// Token: 0x04002FE7 RID: 12263
	[Header("Camera on start")]
	public Vector3 positionCam;

	// Token: 0x04002FE8 RID: 12264
	public Vector3 rotationCam;
}
