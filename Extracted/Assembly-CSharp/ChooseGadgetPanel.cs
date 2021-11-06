using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x020005BC RID: 1468
public class ChooseGadgetPanel : MonoBehaviour
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060032B3 RID: 12979 RVA: 0x00106BD4 File Offset: 0x00104DD4
	// (remove) Token: 0x060032B4 RID: 12980 RVA: 0x00106BEC File Offset: 0x00104DEC
	public static event Action OnDisablePanel;

	// Token: 0x060032B5 RID: 12981 RVA: 0x00106C04 File Offset: 0x00104E04
	private void Awake()
	{
		ChooseGadgetPanel.LoadDefaultGadget();
		InGameGadgetSet.Renew();
		this.UpdatePanel();
		ShopNGUIController.EquippedGadget += this.ShopNGUIController_EquippedGadget;
		try
		{
			this.swipeToOpenHint.SetActiveSafeSelf(false);
			this.chooseGadgetHint.SetActiveSafeSelf(false);
			this.m_hintsShown = (Storager.getInt("ChooseGadgetPanel.GADGET_PANEL_HINTS_SHOWN_KEY", false) == 1);
			if (!this.m_hintsShown && InGameGadgetSet.CurrentSet.Count > 1)
			{
				this.m_hintState = ChooseGadgetPanel.HintState.RunningFirstTimer;
				this.m_delayShowFirstHintStartTime = Time.time;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in initializing gadget hints: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x00106CC8 File Offset: 0x00104EC8
	private void Update()
	{
		if (this.m_hintState == ChooseGadgetPanel.HintState.RunningFirstTimer)
		{
			if (Time.time - 3f >= this.m_delayShowFirstHintStartTime)
			{
				this.m_hintState = ChooseGadgetPanel.HintState.SHowingFirstTip;
				this.m_hintsShown = true;
				this.swipeToOpenHint.SetActiveSafeSelf(true);
				this.chooseGadgetHint.SetActiveSafeSelf(false);
			}
		}
		else if (this.m_hintState == ChooseGadgetPanel.HintState.SHowingFirstTip && Time.time - 15f - 3f >= this.m_delayShowFirstHintStartTime)
		{
			this.m_hintState = ChooseGadgetPanel.HintState.Finished;
			this.swipeToOpenHint.SetActiveSafeSelf(false);
			this.chooseGadgetHint.SetActiveSafeSelf(false);
		}
		try
		{
			if (this._entries != null)
			{
				if (this._entries.Count > 0)
				{
					this.gadgetButtonScript.duration.fillAmount = InGameGadgetSet.CurrentSet[this._entries[0].Info.Category].ExpirationProgress;
					this.gadgetButtonScript.cooldown.fillAmount = InGameGadgetSet.CurrentSet[this._entries[0].Info.Category].CooldownProgress;
					bool canUse = InGameGadgetSet.CurrentSet[this._entries[0].Info.Category].CanUse;
					if (this.entrieCategory != -1 && this.entrieCategory != (int)this._entries[0].Info.Category)
					{
						this.isGadgetReady = canUse;
					}
					if (canUse && !this.isGadgetReady)
					{
						this.isGadgetReady = true;
						this.gadgetButtonScript.cooldownEnds.GetComponent<UITweener>().ResetToBeginning();
						this.gadgetButtonScript.cooldownEnds.GetComponent<UITweener>().PlayForward();
						if (Defs.isSoundFX)
						{
							this.gadgetButtonScript.cooldownEnds.GetComponent<AudioSource>().Play();
						}
					}
					this.isGadgetReady = canUse;
					this.entrieCategory = (int)this._entries[0].Info.Category;
				}
				if (this._entries.Count > 1)
				{
					this.gadgetButtonScript.duration1.fillAmount = InGameGadgetSet.CurrentSet[this._entries[1].Info.Category].ExpirationProgress;
					this.gadgetButtonScript.cooldown1.fillAmount = InGameGadgetSet.CurrentSet[this._entries[1].Info.Category].CooldownProgress;
				}
				if (this._entries.Count > 2)
				{
					this.gadgetButtonScript.duration2.fillAmount = InGameGadgetSet.CurrentSet[this._entries[2].Info.Category].ExpirationProgress;
					this.gadgetButtonScript.cooldown2.fillAmount = InGameGadgetSet.CurrentSet[this._entries[2].Info.Category].CooldownProgress;
				}
			}
			this.gadgetButtonScript.yazichok.SetActiveSafeSelf(this.CanExtend() && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.canUseGadgets);
		}
		catch (Exception ex)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogErrorFormat("Exception in ChooseGadgetPanel.Update: {0}", new object[]
				{
					ex
				});
			}
		}
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x0010704C File Offset: 0x0010524C
	private void ShopNGUIController_EquippedGadget(string arg1, string arg2, GadgetInfo.GadgetCategory arg3)
	{
		this.UpdatePanel();
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x00107054 File Offset: 0x00105254
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			ChooseGadgetPanel.SaveDefaultGadget();
			this.SaveHintsShown();
		}
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x00107068 File Offset: 0x00105268
	private void OnDisable()
	{
		Action onDisablePanel = ChooseGadgetPanel.OnDisablePanel;
		if (onDisablePanel != null)
		{
			onDisablePanel();
		}
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x00107088 File Offset: 0x00105288
	private void OnDestroy()
	{
		ChooseGadgetPanel.SaveDefaultGadget();
		ShopNGUIController.EquippedGadget -= this.ShopNGUIController_EquippedGadget;
		this.SaveHintsShown();
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x001070A8 File Offset: 0x001052A8
	private void SaveHintsShown()
	{
		if (this.m_hintsShown && Storager.getInt("ChooseGadgetPanel.GADGET_PANEL_HINTS_SHOWN_KEY", false) == 0)
		{
			Storager.setInt("ChooseGadgetPanel.GADGET_PANEL_HINTS_SHOWN_KEY", 1, false);
		}
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x001070D4 File Offset: 0x001052D4
	public void Show()
	{
		if (this.m_hintState == ChooseGadgetPanel.HintState.RunningFirstTimer || this.m_hintState == ChooseGadgetPanel.HintState.SHowingFirstTip)
		{
			this.m_hintState = ChooseGadgetPanel.HintState.ShowingSecondTip;
			this.m_hintsShown = true;
			this.swipeToOpenHint.SetActiveSafeSelf(false);
			this.chooseGadgetHint.SetActiveSafeSelf(true);
		}
		this.gadgetButtonScript.OpenGadgetPanel(true);
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x0010712C File Offset: 0x0010532C
	public void Hide()
	{
		if (this.m_hintState == ChooseGadgetPanel.HintState.ShowingSecondTip)
		{
			this.m_hintState = ChooseGadgetPanel.HintState.Finished;
			this.m_hintsShown = true;
			this.swipeToOpenHint.SetActiveSafeSelf(false);
			this.chooseGadgetHint.SetActiveSafeSelf(false);
		}
		this.gadgetButtonScript.OpenGadgetPanel(false);
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x00107178 File Offset: 0x00105378
	public void ChooseDefault()
	{
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x0010717C File Offset: 0x0010537C
	public void ChooseFirst()
	{
		if (this._entries.Count > 1)
		{
			GadgetsInfo.DefaultGadget = this._entries[1].Info.Category;
			this.UpdatePanel();
		}
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x001071BC File Offset: 0x001053BC
	public void ChooseSecond()
	{
		if (this._entries.Count > 2)
		{
			GadgetsInfo.DefaultGadget = this._entries[2].Info.Category;
			this.UpdatePanel();
		}
	}

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x060032C1 RID: 12993 RVA: 0x001071FC File Offset: 0x001053FC
	public GadgetButton gadgetButtonScript
	{
		get
		{
			if (this._cachedGadgetButton == null)
			{
				this._cachedGadgetButton = base.GetComponent<GadgetButton>();
			}
			return this._cachedGadgetButton;
		}
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x00107224 File Offset: 0x00105424
	public bool CanExtend()
	{
		return this._entries.Count > 1;
	}

	// Token: 0x060032C3 RID: 12995 RVA: 0x00107234 File Offset: 0x00105434
	private void UpdatePanel()
	{
		GadgetPanelEntry[] array = new GadgetPanelEntry[this._entries.Count];
		this._entries.CopyTo(array);
		this._entries.Clear();
		foreach (GadgetInfo.GadgetCategory gadgetCategory in Enum.GetValues(typeof(GadgetInfo.GadgetCategory)).OfType<GadgetInfo.GadgetCategory>())
		{
			Gadget gadget = null;
			InGameGadgetSet.CurrentSet.TryGetValue(gadgetCategory, out gadget);
			if (gadget != null)
			{
				this._entries.Add(new GadgetPanelEntry
				{
					Texture = ItemDb.GetItemIcon(gadget.Info.Id, (ShopNGUIController.CategoryNames)gadgetCategory, null, true),
					Info = gadget.Info
				});
			}
		}
		this._entries.Sort(delegate(GadgetPanelEntry entry1, GadgetPanelEntry entry2)
		{
			if (entry1.Info.Category == GadgetsInfo.DefaultGadget && entry2.Info.Category == GadgetsInfo.DefaultGadget)
			{
				return 0;
			}
			if (entry1.Info.Category == GadgetsInfo.DefaultGadget)
			{
				return -1;
			}
			if (entry2.Info.Category == GadgetsInfo.DefaultGadget)
			{
				return 1;
			}
			return entry1.Info.Category.CompareTo(entry2.Info.Category);
		});
		if (this._entries.Count > 0)
		{
			this.gadgetButtonScript.gadgetIcon.mainTexture = this._entries[0].Texture;
			if (this._entries.Count > 1)
			{
				this.gadgetButtonScript.gadgetIcon1.mainTexture = this._entries[1].Texture;
				if (this._entries.Count > 2)
				{
					this.gadgetButtonScript.gadgetIcon2.mainTexture = this._entries[2].Texture;
				}
				this.gadgetButtonScript.thirdAvailableGadgetCell.SetActiveSafeSelf(this._entries.Count > 2);
				this.gadgetButtonScript.thirdAvailableGadgetFrame.SetActiveSafeSelf(this._entries.Count > 2);
			}
		}
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x00107420 File Offset: 0x00105620
	private static void SaveDefaultGadget()
	{
		if (Storager.getInt("GadgetsInfo.DefaultGadgetKey", false) != (int)GadgetsInfo.DefaultGadget)
		{
			Storager.setInt("GadgetsInfo.DefaultGadgetKey", (int)GadgetsInfo.DefaultGadget, false);
		}
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x00107448 File Offset: 0x00105648
	private static void LoadDefaultGadget()
	{
		int num = Storager.getInt("GadgetsInfo.DefaultGadgetKey", false);
		if (num == 0)
		{
			num = 12500;
		}
		GadgetsInfo.DefaultGadget = (GadgetInfo.GadgetCategory)num;
	}

	// Token: 0x0400253C RID: 9532
	private const float DELAY_FIRST_TIP = 3f;

	// Token: 0x0400253D RID: 9533
	private const float TIME_SHOWING_FIRST_TIP = 15f;

	// Token: 0x0400253E RID: 9534
	private const string GADGET_PANEL_HINTS_SHOWN_KEY = "ChooseGadgetPanel.GADGET_PANEL_HINTS_SHOWN_KEY";

	// Token: 0x0400253F RID: 9535
	public GameObject swipeToOpenHint;

	// Token: 0x04002540 RID: 9536
	public GameObject chooseGadgetHint;

	// Token: 0x04002541 RID: 9537
	public List<Transform> objectsNoFilpXScale;

	// Token: 0x04002542 RID: 9538
	public GameObject hoverBackground1;

	// Token: 0x04002543 RID: 9539
	public GameObject hoverBackground2;

	// Token: 0x04002544 RID: 9540
	public GameObject hoverBackground3;

	// Token: 0x04002545 RID: 9541
	private bool isGadgetReady = true;

	// Token: 0x04002546 RID: 9542
	private int entrieCategory = -1;

	// Token: 0x04002547 RID: 9543
	private GadgetButton _cachedGadgetButton;

	// Token: 0x04002548 RID: 9544
	private List<GadgetPanelEntry> _entries = new List<GadgetPanelEntry>();

	// Token: 0x04002549 RID: 9545
	private ChooseGadgetPanel.HintState m_hintState;

	// Token: 0x0400254A RID: 9546
	private float m_delayShowFirstHintStartTime;

	// Token: 0x0400254B RID: 9547
	private bool m_hintsShown;

	// Token: 0x020005BD RID: 1469
	private enum HintState
	{
		// Token: 0x0400254F RID: 9551
		ShouldNotShow,
		// Token: 0x04002550 RID: 9552
		RunningFirstTimer,
		// Token: 0x04002551 RID: 9553
		SHowingFirstTip,
		// Token: 0x04002552 RID: 9554
		ShowingSecondTip,
		// Token: 0x04002553 RID: 9555
		Finished
	}
}
