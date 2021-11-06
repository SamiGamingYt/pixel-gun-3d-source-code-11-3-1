using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
public class ButtonBannerHUD : MonoBehaviour
{
	// Token: 0x06003201 RID: 12801 RVA: 0x00103864 File Offset: 0x00101A64
	private void Awake()
	{
		ButtonBannerHUD.instance = this;
		this.LoadAllExistBanners();
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x00103874 File Offset: 0x00101A74
	private void Start()
	{
		this.centerScript = this.wrapBanners.GetComponent<MyCenterOnChild>();
		if (this.centerScript != null)
		{
			MyCenterOnChild myCenterOnChild = this.centerScript;
			myCenterOnChild.onFinished = (SpringPanel.OnFinished)Delegate.Combine(myCenterOnChild.onFinished, new SpringPanel.OnFinished(this.OnCenterBanner));
		}
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.LocalizeBanner));
		this.UpdateListBanners();
		this.ResetTimerNextBanner();
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x001038E8 File Offset: 0x00101AE8
	private void OnDestroy()
	{
		if (this.centerScript != null)
		{
			MyCenterOnChild myCenterOnChild = this.centerScript;
			myCenterOnChild.onFinished = (SpringPanel.OnFinished)Delegate.Remove(myCenterOnChild.onFinished, new SpringPanel.OnFinished(this.OnCenterBanner));
		}
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.LocalizeBanner));
		ButtonBannerHUD.instance = null;
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x00103944 File Offset: 0x00101B44
	private void LoadAllExistBanners()
	{
		this.listAllBanners.Clear();
		UnityEngine.Object[] array = Resources.LoadAll("ButtonBanners");
		foreach (UnityEngine.Object @object in array)
		{
			if (@object != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
				gameObject.transform.parent = this.objAnchorNoActiveBanners;
				gameObject.transform.localScale = Vector3.one;
				ButtonBannerBase component = gameObject.GetComponent<ButtonBannerBase>();
				if (component != null)
				{
					this.listAllBanners.Add(component);
				}
			}
		}
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x001039E0 File Offset: 0x00101BE0
	public static void OnUpdateBanners()
	{
		if (ButtonBannerHUD.instance != null)
		{
			ButtonBannerHUD.instance.UpdateListBanners();
		}
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x001039FC File Offset: 0x00101BFC
	private void UpdateListBanners()
	{
		this.RemoveAllNoActiveBanners();
		this.AddNewActiveBanners();
		this.SortByPriority();
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x00103A10 File Offset: 0x00101C10
	private void RemoveAllNoActiveBanners()
	{
		if (this.curShowBanner != null && !this.curShowBanner.BannerIsActive())
		{
			this.curShowBanner = this.GetNextActiveBanner();
		}
		List<ButtonBannerBase> list = new List<ButtonBannerBase>();
		foreach (ButtonBannerBase buttonBannerBase in this.listActiveBanners)
		{
			if (!buttonBannerBase.BannerIsActive())
			{
				list.Add(buttonBannerBase);
			}
		}
		foreach (ButtonBannerBase buttonBannerBase2 in list)
		{
			buttonBannerBase2.transform.parent = this.objAnchorNoActiveBanners;
			this.listActiveBanners.Remove(buttonBannerBase2);
		}
	}

	// Token: 0x06003208 RID: 12808 RVA: 0x00103B20 File Offset: 0x00101D20
	private bool IsExistActiveBanner(ButtonBannerBase needBanners)
	{
		return this.listActiveBanners.Contains(needBanners);
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x00103B30 File Offset: 0x00101D30
	private void AddNewActiveBanners()
	{
		foreach (ButtonBannerBase buttonBannerBase in this.listAllBanners)
		{
			if (!this.IsExistActiveBanner(buttonBannerBase))
			{
				if (buttonBannerBase.BannerIsActive())
				{
					buttonBannerBase.transform.parent = this.wrapBanners.transform;
					this.listActiveBanners.Add(buttonBannerBase);
				}
			}
		}
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x00103BD4 File Offset: 0x00101DD4
	private void SortByPriority()
	{
		this.listActiveBanners.Sort(delegate(ButtonBannerBase left, ButtonBannerBase right)
		{
			if (left == null && right == null)
			{
				return 0;
			}
			if (left == null)
			{
				return -1;
			}
			if (right == null)
			{
				return 1;
			}
			return left.priorityShow.CompareTo(right.priorityShow);
		});
		string text = string.Empty;
		for (int i = 0; i < this.listActiveBanners.Count; i++)
		{
			text = i.ToString();
			if (i < 10)
			{
				text = "0" + text;
			}
			this.listActiveBanners[i].gameObject.name = text;
			this.listActiveBanners[i].indexBut = i;
		}
		this.wrapBanners.SortAlphabetically();
		this.wrapBanners.WrapContent();
		this.ShowBanner(this.curShowBanner);
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x0600320B RID: 12811 RVA: 0x00103C94 File Offset: 0x00101E94
	// (set) Token: 0x0600320C RID: 12812 RVA: 0x00103CA0 File Offset: 0x00101EA0
	public int IndexShowBanner
	{
		get
		{
			return Load.LoadInt("keyLastShowIndex");
		}
		set
		{
			int num = value;
			if (num < 0)
			{
				num = 0;
			}
			if (num >= this.listActiveBanners.Count)
			{
				num = 0;
			}
			Save.SaveInt("keyLastShowIndex", num);
		}
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x00103CD8 File Offset: 0x00101ED8
	public void OnClickShowBanner()
	{
		if (this.curShowBanner != null)
		{
			this.curShowBanner.OnClickButton();
		}
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x00103CF8 File Offset: 0x00101EF8
	private ButtonBannerBase GetNextActiveBanner()
	{
		int num = this.listActiveBanners.Count;
		ButtonBannerBase buttonBannerBase = null;
		if (num > 0)
		{
			for (;;)
			{
				this.IndexShowBanner++;
				num--;
				buttonBannerBase = this.listActiveBanners[this.IndexShowBanner];
				if (buttonBannerBase.BannerIsActive())
				{
					break;
				}
				if (num <= 0)
				{
					goto Block_2;
				}
			}
			return buttonBannerBase;
			Block_2:
			Debug.LogWarning("No next banner for show");
		}
		return buttonBannerBase;
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x00103D6C File Offset: 0x00101F6C
	public void ShowNextBanner()
	{
		ButtonBannerBase nextActiveBanner = this.GetNextActiveBanner();
		this.ShowBanner(nextActiveBanner);
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x00103D88 File Offset: 0x00101F88
	public void ShowBanner(ButtonBannerBase needBanner)
	{
		if (needBanner == null || !needBanner.BannerIsActive())
		{
			return;
		}
		if (needBanner != null)
		{
			this.SetShowBanner(needBanner, true);
			this.centerScript.CenterOn(needBanner.transform);
		}
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x00103DD4 File Offset: 0x00101FD4
	public void OnCenterBanner()
	{
		if (this.centerScript != null)
		{
			ButtonBannerBase component = this.centerScript.centeredObject.GetComponent<ButtonBannerBase>();
			if (this.oldShowBanner != null)
			{
				this.oldShowBanner.OnHide();
			}
			this.SetShowBanner(component, false);
			if (component)
			{
				this.ResetTimerNextBanner();
			}
		}
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x00103E38 File Offset: 0x00102038
	private void SetShowBanner(ButtonBannerBase needBanner, bool auto = false)
	{
		if (this.curShowBanner != needBanner)
		{
			this.oldShowBanner = this.curShowBanner;
			this.curShowBanner = needBanner;
			if (this.curShowBanner != null)
			{
				this.IndexShowBanner = this.curShowBanner.indexBut;
				this.curShowBanner.OnShow();
				if (auto)
				{
					this.curShowBanner.OnUpdateParameter();
				}
			}
		}
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x00103EA8 File Offset: 0x001020A8
	public void StopTimerNextBanner()
	{
		base.CancelInvoke("ShowNextBanner");
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x00103EB8 File Offset: 0x001020B8
	public void ResetTimerNextBanner()
	{
		this.StopTimerNextBanner();
		base.InvokeRepeating("ShowNextBanner", 5f, 5f);
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x00103ED8 File Offset: 0x001020D8
	public void LocalizeBanner()
	{
		if (this.curShowBanner != null)
		{
			this.curShowBanner.OnChangeLocalize();
		}
	}

	// Token: 0x040024CE RID: 9422
	private const string keyLastShowIndex = "keyLastShowIndex";

	// Token: 0x040024CF RID: 9423
	private const string pathToBtnBanner = "ButtonBanners";

	// Token: 0x040024D0 RID: 9424
	private const float timeForNextScroll = 5f;

	// Token: 0x040024D1 RID: 9425
	public static ButtonBannerHUD instance;

	// Token: 0x040024D2 RID: 9426
	public UIScrollView scrollBanners;

	// Token: 0x040024D3 RID: 9427
	public UIWrapContent wrapBanners;

	// Token: 0x040024D4 RID: 9428
	public ButtonBannerBase curShowBanner;

	// Token: 0x040024D5 RID: 9429
	private ButtonBannerBase oldShowBanner;

	// Token: 0x040024D6 RID: 9430
	public Transform objAnchorNoActiveBanners;

	// Token: 0x040024D7 RID: 9431
	public List<ButtonBannerBase> listAllBanners = new List<ButtonBannerBase>();

	// Token: 0x040024D8 RID: 9432
	public List<ButtonBannerBase> listActiveBanners = new List<ButtonBannerBase>();

	// Token: 0x040024D9 RID: 9433
	[HideInInspector]
	public MyCenterOnChild centerScript;
}
