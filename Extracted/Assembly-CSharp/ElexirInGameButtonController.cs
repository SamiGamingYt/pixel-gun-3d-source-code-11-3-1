using System;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class ElexirInGameButtonController : MonoBehaviour
{
	// Token: 0x06000518 RID: 1304 RVA: 0x00029AAC File Offset: 0x00027CAC
	private void Awake()
	{
		string text = (!Defs.isDaterRegim) ? this.myPotion.name : this.idForPriceInDaterRegim;
		string name = this.myPotion.name;
		if (GearManager.Gear.Contains(text))
		{
			text = GearManager.OneItemIDForGear(text, GearManager.CurrentNumberOfUphradesForGear(text));
		}
		this.isKnifeMap = SceneLoader.ActiveSceneName.Equals("Knife");
		if (Defs.isHunger || this.isKnifeMap)
		{
			this.myButton.disabledSprite = "game_clear";
			this.myButton.isEnabled = false;
			this.lockSprite.SetActive(true);
		}
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(text);
		if (name != null && name.Equals(GearManager.Grenade))
		{
			itemPrice = new ItemPrice(itemPrice.Price * GearManager.ItemsInPackForGear(GearManager.Grenade), itemPrice.Currency);
		}
		this.priceLabel.GetComponent<UILabel>().text = itemPrice.Price.ToString();
		PotionsController.PotionDisactivated += this.HandlePotionDisactivated;
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00029BC0 File Offset: 0x00027DC0
	private void Start()
	{
		this.OnEnable();
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00029BC8 File Offset: 0x00027DC8
	private void HandlePotionDisactivated(string obj)
	{
		if (!obj.Equals(this.myPotion.name))
		{
			return;
		}
		this.myButton.isEnabled = true;
		this.myLabelTime.text = string.Empty;
		string key = (!Defs.isDaterRegim) ? obj : GearManager.HolderQuantityForID(this.idForPriceInDaterRegim);
		int @int = Storager.getInt(key, false);
		this.myLabelTime.enabled = false;
		this.isActivePotion = false;
		this.myLabelTime.gameObject.SetActive(base.gameObject.activeSelf);
		if (@int == 0)
		{
			this.SetStateBuy();
		}
		else
		{
			this.SetStateUse();
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00029C74 File Offset: 0x00027E74
	private void SetStateBuy()
	{
		this.myButton.normalSprite = "game_clear_yellow";
		this.myButton.pressedSprite = "game_clear_yellow_n";
		this.priceLabel.SetActive(true);
		this.myLabelCount.gameObject.SetActive(false);
		this.plusSprite.SetActive(true);
		this.myLabelTime.enabled = false;
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00029CD8 File Offset: 0x00027ED8
	private void SetStateUse()
	{
		this.myLabelCount.gameObject.SetActive(true);
		this.plusSprite.SetActive(false);
		this.myButton.normalSprite = "game_clear";
		this.myButton.pressedSprite = "game_clear_n";
		this.priceLabel.SetActive(false);
		if (!this.isActivePotion)
		{
			this.myLabelTime.enabled = false;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00029D4C File Offset: 0x00027F4C
	private void Update()
	{
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00029D50 File Offset: 0x00027F50
	private void OnDestroy()
	{
		PotionsController.PotionDisactivated -= this.HandlePotionDisactivated;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00029D64 File Offset: 0x00027F64
	private void OnEnable()
	{
		int @int = Storager.getInt((!Defs.isDaterRegim) ? this.myPotion.name : GearManager.HolderQuantityForID(this.idForPriceInDaterRegim), false);
		this.myLabelCount.text = @int.ToString();
		this.myLabelTime.gameObject.SetActive(true);
		if (@int == 0)
		{
			if (!this.isActivePotion)
			{
				this.SetStateBuy();
			}
		}
		else
		{
			this.SetStateUse();
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00029DE4 File Offset: 0x00027FE4
	private void OnDisable()
	{
		if (!this.isActivePotion)
		{
			this.myLabelTime.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400058F RID: 1423
	public bool isActivePotion;

	// Token: 0x04000590 RID: 1424
	public UIButton myButton;

	// Token: 0x04000591 RID: 1425
	public UILabel myLabelTime;

	// Token: 0x04000592 RID: 1426
	public UILabel myLabelCount;

	// Token: 0x04000593 RID: 1427
	public int price = 10;

	// Token: 0x04000594 RID: 1428
	public GameObject plusSprite;

	// Token: 0x04000595 RID: 1429
	public GameObject myPotion;

	// Token: 0x04000596 RID: 1430
	public GameObject priceLabel;

	// Token: 0x04000597 RID: 1431
	public GameObject lockSprite;

	// Token: 0x04000598 RID: 1432
	private bool isKnifeMap;

	// Token: 0x04000599 RID: 1433
	public string idForPriceInDaterRegim;
}
