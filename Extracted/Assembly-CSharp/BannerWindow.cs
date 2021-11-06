using System;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public class BannerWindow : MonoBehaviour
{
	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06003001 RID: 12289 RVA: 0x000FA8E8 File Offset: 0x000F8AE8
	// (set) Token: 0x06003002 RID: 12290 RVA: 0x000FA8F0 File Offset: 0x000F8AF0
	public BannerWindowType type { get; set; }

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06003003 RID: 12291 RVA: 0x000FA8FC File Offset: 0x000F8AFC
	// (set) Token: 0x06003004 RID: 12292 RVA: 0x000FA904 File Offset: 0x000F8B04
	public bool IsShow
	{
		get
		{
			return this._isShow;
		}
		set
		{
			this._isShow = value;
		}
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x000FA910 File Offset: 0x000F8B10
	public void SetBackgroundImage(Texture2D image)
	{
		if (this.Background == null)
		{
			return;
		}
		this.Background.mainTexture = image;
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x000FA930 File Offset: 0x000F8B30
	public void SetEnableExitButton(bool enable)
	{
		if (this.ExitButton == null)
		{
			return;
		}
		this.ExitButton.gameObject.SetActive(enable);
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x000FA958 File Offset: 0x000F8B58
	protected virtual void SetActiveAndShow()
	{
		base.gameObject.SetActive(true);
		this.IsShow = true;
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x000FA970 File Offset: 0x000F8B70
	public virtual void Show()
	{
		this.SetActiveAndShow();
		AdmobPerelivWindow component = base.GetComponent<AdmobPerelivWindow>();
		if (component != null)
		{
			component.Show();
		}
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x000FA99C File Offset: 0x000F8B9C
	public void Hide()
	{
		AdmobPerelivWindow component = base.GetComponent<AdmobPerelivWindow>();
		if (component != null)
		{
			component.Hide();
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.IsShow = false;
	}

	// Token: 0x0600300A RID: 12298 RVA: 0x000FA9DC File Offset: 0x000F8BDC
	internal virtual void Submit()
	{
	}

	// Token: 0x04002340 RID: 9024
	public UITexture Background;

	// Token: 0x04002341 RID: 9025
	public UIButton ExitButton;

	// Token: 0x04002342 RID: 9026
	private bool _isShow;
}
