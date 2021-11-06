using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class LevelUpWithOffers : MonoBehaviour
{
	// Token: 0x06001A3A RID: 6714 RVA: 0x0006A090 File Offset: 0x00068290
	private IEnumerator UpdatePanelsAndAnchors()
	{
		yield return new WaitForEndOfFrame();
		Player_move_c.PerformActionRecurs(base.transform.parent.parent.parent.gameObject, delegate(Transform t)
		{
			UIPanel component = t.GetComponent<UIPanel>();
			if (component != null)
			{
				component.Refresh();
			}
		});
		Player_move_c.PerformActionRecurs(base.transform.parent.parent.parent.gameObject, delegate(Transform t)
		{
			UIRect component = t.GetComponent<UIRect>();
			if (component != null)
			{
				component.UpdateAnchors();
			}
		});
		yield break;
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x0006A0AC File Offset: 0x000682AC
	private void Awake()
	{
		if (!this.isTierLevelUp)
		{
			FacebookController.StoryPriority levelupPriority = FacebookController.StoryPriority.Red;
			this.shareScript.priority = levelupPriority;
			this.shareScript.shareAction = delegate()
			{
				FacebookController.PostOpenGraphStory("reach", "level", levelupPriority, new Dictionary<string, string>
				{
					{
						"level",
						ExperienceController.sharedController.currentLevel.ToString()
					}
				});
			};
			this.shareScript.HasReward = true;
			this.shareScript.twitterStatus = (() => string.Format("I've reached level {0} in @PixelGun3D! Come to the battle and try to defeat me! #pixelgun3d #pixelgun #3d #pg3d #fps http://goo.gl/8fzL9u", ExperienceController.sharedController.currentLevel));
			this.shareScript.EventTitle = "Level-up";
		}
		else
		{
			FacebookController.StoryPriority tierupPriority = FacebookController.StoryPriority.Green;
			this.shareScript.priority = tierupPriority;
			this.shareScript.shareAction = delegate()
			{
				FacebookController.PostOpenGraphStory("unlock", "new weapon", tierupPriority, new Dictionary<string, string>
				{
					{
						"new weapon",
						(ExpController.Instance.OurTier + 1).ToString()
					}
				});
			};
			this.shareScript.HasReward = true;
			this.shareScript.twitterStatus = (() => "I've unlocked cool new weapons in @PixelGun3D! Let’s try them out! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u");
			this.shareScript.EventTitle = "Tier-up";
		}
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x0006A1BC File Offset: 0x000683BC
	[ContextMenu("Update")]
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdatePanelsAndAnchors());
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x0006A1CC File Offset: 0x000683CC
	private void OnDisable()
	{
		this.ShowIndicationMoney();
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x0006A1D4 File Offset: 0x000683D4
	private void OnDestroy()
	{
		this.ShowIndicationMoney();
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x0006A1DC File Offset: 0x000683DC
	private void ShowIndicationMoney()
	{
		BankController.canShowIndication = true;
		BankController.UpdateAllIndicatorsMoney();
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0006A1EC File Offset: 0x000683EC
	public void SetCurrentRank(string currentRank)
	{
		for (int i = 0; i < this.currentRankLabel.Length; i++)
		{
			this.currentRankLabel[i].text = LocalizationStore.Get("Key_0226").ToUpper() + " " + currentRank + "!";
		}
		string text = string.Empty;
		switch (ProfileController.CurOrderCup)
		{
		case 0:
			text = ScriptLocalization.Get("Key_1938");
			break;
		case 1:
			text = ScriptLocalization.Get("Key_1939");
			break;
		case 2:
			text = ScriptLocalization.Get("Key_1940");
			break;
		case 3:
			text = ScriptLocalization.Get("Key_1941");
			break;
		case 4:
			text = ScriptLocalization.Get("Key_1942");
			break;
		case 5:
			text = ScriptLocalization.Get("Key_1943");
			break;
		}
		foreach (UILabel uilabel in this.youReachedLabels)
		{
			uilabel.text = text;
		}
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x0006A328 File Offset: 0x00068528
	public void SetRewardPrice(string rewardPrice)
	{
		for (int i = 0; i < this.rewardPriceLabel.Length; i++)
		{
			this.rewardPriceLabel[i].text = rewardPrice;
		}
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0006A35C File Offset: 0x0006855C
	public void SetGemsRewardPrice(string gemsReward)
	{
		for (int i = 0; i < this.rewardGemsPriceLabel.Length; i++)
		{
			this.rewardGemsPriceLabel[i].text = gemsReward;
		}
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x0006A390 File Offset: 0x00068590
	public void SetAddHealthCount(string count)
	{
		if (this.healthLabel != null)
		{
			for (int i = 0; i < this.healthLabel.Length; i++)
			{
				this.healthLabel[i].text = count;
			}
		}
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0006A3D0 File Offset: 0x000685D0
	private void SetGemsLabel(int value)
	{
		for (int i = 0; i < this.gemsStarterBank.Length; i++)
		{
			this.gemsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1531"), value);
		}
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0006A418 File Offset: 0x00068618
	private void SetCoinsLabel(int value)
	{
		for (int i = 0; i < this.coinsStarterBank.Length; i++)
		{
			this.coinsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1530"), value);
		}
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x0006A460 File Offset: 0x00068660
	public IEnumerator GemsStarterAnimation()
	{
		float seconds = 0f;
		this.SetGemsLabel(0);
		while (seconds < 1f)
		{
			for (int i = 0; i < this.gemsStarterBank.Length; i++)
			{
				this.SetGemsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, this.gemsStarterBankValue, seconds)));
			}
			seconds += Time.deltaTime;
			yield return null;
		}
		this.SetGemsLabel(Mathf.RoundToInt(this.gemsStarterBankValue));
		yield break;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0006A47C File Offset: 0x0006867C
	public IEnumerator CoinsStarterAnimation()
	{
		float seconds = 0f;
		this.SetCoinsLabel(0);
		while (seconds < 1f)
		{
			for (int i = 0; i < this.coinsStarterBank.Length; i++)
			{
				this.SetCoinsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, this.coinsStarterBankValue, seconds)));
			}
			seconds += Time.deltaTime;
			yield return null;
		}
		this.SetCoinsLabel(Mathf.RoundToInt(this.coinsStarterBankValue));
		yield break;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0006A498 File Offset: 0x00068698
	public void SetStarterBankValues(int gemsReward, int coinsReward)
	{
		this.gemsStarterBankValue = (float)gemsReward;
		this.coinsStarterBankValue = (float)coinsReward;
		this.SetGemsLabel(0);
		this.SetCoinsLabel(0);
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0006A4B8 File Offset: 0x000686B8
	public void SetItems(List<string> itemTags)
	{
		if (this.items == null || this.items.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < itemTags.Count; j++)
		{
			this.items[j].gameObject.SetActive(true);
			string text = itemTags[j];
			int itemCategory = ItemDb.GetItemCategory(text);
			this.items[j]._tag = text;
			this.items[j].category = (ShopNGUIController.CategoryNames)itemCategory;
			this.items[j].itemImage.mainTexture = ItemDb.GetItemIcon(text, (ShopNGUIController.CategoryNames)itemCategory, new int?(1), true);
			foreach (UILabel uilabel in this.items[j].itemName)
			{
				uilabel.text = ItemDb.GetItemName(text, (ShopNGUIController.CategoryNames)itemCategory);
			}
			this.items[j].GetComponent<UIButton>().isEnabled = (!Defs.isHunger || text == null || ItemDb.GetByTag(text) == null);
		}
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0006A620 File Offset: 0x00068820
	public void Close()
	{
		ExpController.Instance.HandleContinueButton(base.gameObject);
	}

	// Token: 0x04000F5C RID: 3932
	public RewardWindowBase shareScript;

	// Token: 0x04000F5D RID: 3933
	public UILabel[] rewardGemsPriceLabel;

	// Token: 0x04000F5E RID: 3934
	public UILabel[] currentRankLabel;

	// Token: 0x04000F5F RID: 3935
	public UILabel[] rewardPriceLabel;

	// Token: 0x04000F60 RID: 3936
	public UILabel[] healthLabel;

	// Token: 0x04000F61 RID: 3937
	public UILabel[] gemsStarterBank;

	// Token: 0x04000F62 RID: 3938
	public UILabel[] coinsStarterBank;

	// Token: 0x04000F63 RID: 3939
	public List<UILabel> youReachedLabels;

	// Token: 0x04000F64 RID: 3940
	public NewAvailableItemInShop[] items;

	// Token: 0x04000F65 RID: 3941
	public bool isTierLevelUp;

	// Token: 0x04000F66 RID: 3942
	private float gemsStarterBankValue;

	// Token: 0x04000F67 RID: 3943
	private float coinsStarterBankValue;

	// Token: 0x020002F2 RID: 754
	public struct ItemDesc
	{
		// Token: 0x04000F6A RID: 3946
		public string tag;

		// Token: 0x04000F6B RID: 3947
		public ShopNGUIController.CategoryNames category;
	}
}
