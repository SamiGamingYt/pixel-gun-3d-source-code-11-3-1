using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200000D RID: 13
[DisallowMultipleComponent]
public sealed class AdmobPerelivWindow : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00003B84 File Offset: 0x00001D84
	// (set) Token: 0x0600002F RID: 47 RVA: 0x00003B8C File Offset: 0x00001D8C
	public static string Context { get; set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000030 RID: 48 RVA: 0x00003B94 File Offset: 0x00001D94
	private Transform myTransform
	{
		get
		{
			if (this._transform == null)
			{
				this._transform = base.transform;
			}
			return this._transform;
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003BBC File Offset: 0x00001DBC
	private void Awake()
	{
		this._transform = base.transform;
		this.closeSprite.gameObject.SetActive(BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer);
		this.closeSpriteAndr.gameObject.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003C08 File Offset: 0x00001E08
	private void Start()
	{
		if (this.closeAnchor != null)
		{
			this.closeAnchor.transform.localPosition = new Vector3((float)(-(float)Screen.width / 2) * 768f / (float)Screen.height, 384f, 0f);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003C5C File Offset: 0x00001E5C
	private void Update()
	{
		if (this.state == AdmobPerelivWindow.WinState.on && this.myTransform.localPosition.y < 0f)
		{
			float num = this.myTransform.localPosition.y;
			num += 770f / this.timeOn * Time.deltaTime;
			if (num > 0f)
			{
				num = 0f;
				this.state = AdmobPerelivWindow.WinState.show;
			}
			this.myTransform.localPosition = new Vector3(0f, num, 0f);
		}
		if (this.state == AdmobPerelivWindow.WinState.off && this.myTransform.localPosition.y > -770f)
		{
			float num2 = this.myTransform.localPosition.y;
			num2 -= 770f / this.timeOn * Time.deltaTime;
			if (num2 < -770f)
			{
				num2 = -770f;
				this.state = AdmobPerelivWindow.WinState.none;
				base.gameObject.SetActive(false);
				this.adTexture.mainTexture = null;
				if (AdmobPerelivWindow.admobTexture != null)
				{
					UnityEngine.Object.Destroy(AdmobPerelivWindow.admobTexture);
				}
				AdmobPerelivWindow.admobTexture = null;
				AdmobPerelivWindow.admobUrl = string.Empty;
			}
			this.myTransform.localPosition = new Vector3(0f, num2, 0f);
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003DB8 File Offset: 0x00001FB8
	public void Show()
	{
		if (this.state != AdmobPerelivWindow.WinState.none)
		{
			return;
		}
		if (AdmobPerelivWindow.admobTexture == null)
		{
			Debug.LogWarningFormat("AdmobTexture is null.", new object[0]);
			return;
		}
		float num = (float)AdmobPerelivWindow.admobTexture.width;
		float num2 = (float)AdmobPerelivWindow.admobTexture.height;
		if (num2 / num >= (float)Screen.height / (float)Screen.width)
		{
			num = num * 768f / num2;
			num2 = 768f;
		}
		else
		{
			num2 = num2 * (768f * (float)Screen.width) / ((float)Screen.height * num);
			num = 768f * (float)Screen.width / (float)Screen.height;
		}
		if (this.adTexture != null)
		{
			this.adTexture.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			this.adTexture.mainTexture = AdmobPerelivWindow.admobTexture;
			this.adTexture.width = Mathf.RoundToInt(num);
			this.adTexture.height = Mathf.RoundToInt(num2);
		}
		else
		{
			Debug.LogWarning("AdTexture is null.");
		}
		if (this.NeedSmoothShow)
		{
			this.state = AdmobPerelivWindow.WinState.on;
			this.myTransform.localPosition = new Vector3(0f, -770f, 0f);
			float num3 = 44f;
			float f = 242f;
			float num4 = 30f;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
			}
			num3 = num3 * 768f / (float)Screen.height;
			num4 = num4 * 768f / (float)Screen.height;
			this.closeSprite.width = Mathf.RoundToInt(num3);
			this.closeSprite.height = Mathf.RoundToInt(num3);
			this.closeSprite.transform.localPosition = new Vector3(num4, -num4, 0f);
			if (this.lightTexture != null)
			{
				this.lightTexture.width = Mathf.RoundToInt(f);
				this.lightTexture.height = Mathf.RoundToInt(f);
			}
		}
		else
		{
			this.myTransform.localPosition = new Vector3(0f, 0f, 0f);
			this.state = AdmobPerelivWindow.WinState.show;
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003FCC File Offset: 0x000021CC
	public void Hide()
	{
		if (this.state != AdmobPerelivWindow.WinState.show)
		{
			return;
		}
		if (this.NeedSmoothShow)
		{
			this.state = AdmobPerelivWindow.WinState.off;
		}
		else
		{
			this.adTexture.mainTexture = null;
			if (AdmobPerelivWindow.admobTexture != null)
			{
				UnityEngine.Object.Destroy(AdmobPerelivWindow.admobTexture);
			}
			AdmobPerelivWindow.admobTexture = null;
			AdmobPerelivWindow.admobUrl = string.Empty;
			this.state = AdmobPerelivWindow.WinState.none;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000036 RID: 54 RVA: 0x00004048 File Offset: 0x00002248
	private bool NeedSmoothShow
	{
		get
		{
			return false;
		}
	}

	// Token: 0x04000039 RID: 57
	public AdmobPerelivWindow.WinState state;

	// Token: 0x0400003A RID: 58
	private float timeOn = 0.2f;

	// Token: 0x0400003B RID: 59
	public static Texture admobTexture = null;

	// Token: 0x0400003C RID: 60
	public static string admobUrl = string.Empty;

	// Token: 0x0400003D RID: 61
	public UITexture adTexture;

	// Token: 0x0400003E RID: 62
	public GameObject closeAnchor;

	// Token: 0x0400003F RID: 63
	public UISprite closeSprite;

	// Token: 0x04000040 RID: 64
	public UITexture lightTexture;

	// Token: 0x04000041 RID: 65
	public UISprite closeSpriteAndr;

	// Token: 0x04000042 RID: 66
	private Transform _transform;

	// Token: 0x0200000E RID: 14
	public enum WinState
	{
		// Token: 0x04000045 RID: 69
		none,
		// Token: 0x04000046 RID: 70
		on,
		// Token: 0x04000047 RID: 71
		show,
		// Token: 0x04000048 RID: 72
		off
	}
}
