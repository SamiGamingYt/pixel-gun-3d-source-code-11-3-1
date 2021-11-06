using System;
using UnityEngine;

// Token: 0x0200084D RID: 2125
public class TableGearController : MonoBehaviour
{
	// Token: 0x06004D18 RID: 19736 RVA: 0x001BCE14 File Offset: 0x001BB014
	private void Start()
	{
		TableGearController.sharedController = this;
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001BCE1C File Offset: 0x001BB01C
	private void OnDestroy()
	{
		TableGearController.sharedController = null;
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x001BCE24 File Offset: 0x001BB024
	private void Update()
	{
		if (this.timerShowLabel > 0f)
		{
			this.timerShowLabel -= Time.deltaTime;
			if (this.timerShowLabel < 0f)
			{
				this.activatePotionLabel.gameObject.SetActive(false);
			}
		}
		for (int i = 0; i < this.potionLables.Length; i++)
		{
			if (!PotionsController.sharedController.PotionIsActive(this.potionLables[i].myPotionName))
			{
				if (this.potionLables[i].gameObject.activeSelf)
				{
					this.potionLables[i].gameObject.SetActive(false);
					this.potionLables[i].myLabel.text = string.Empty;
					this.table.Reposition();
				}
			}
			else
			{
				if (!this.potionLables[i].gameObject.activeSelf)
				{
					this.potionLables[i].transform.GetChild(0).GetComponent<TweenScale>().enabled = true;
					this.potionLables[i].gameObject.SetActive(true);
					this.ReNameLabelObjects();
					this.table.Reposition();
					string myPotionName = this.potionLables[i].myPotionName;
					TableGearController.TypeGear typeGear = (TableGearController.TypeGear)((int)Enum.Parse(typeof(TableGearController.TypeGear), myPotionName));
					int num = (int)typeGear;
					this.activatePotionLabel.text = LocalizationStore.Get((!Defs.isDaterRegim) ? this.keysForLabel[num] : this.keysForLabelDater[num]);
					this.activatePotionLabel.gameObject.SetActive(true);
					this.timerShowLabel = 2f;
				}
				this.potionLables[i].timerUpdate -= Time.deltaTime;
				if (this.potionLables[i].timerUpdate < 0f)
				{
					this.potionLables[i].timerUpdate = 0.25f;
					this.potionLables[i].UpdateTime();
				}
			}
		}
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x001BD014 File Offset: 0x001BB214
	private void ReNameLabelObjects()
	{
		for (int i = 0; i < PotionsController.sharedController.activePotionsList.Count; i++)
		{
			string value = PotionsController.sharedController.activePotionsList[i];
			TableGearController.TypeGear typeGear = (TableGearController.TypeGear)((int)Enum.Parse(typeof(TableGearController.TypeGear), value));
			int num = (int)typeGear;
			this.potionLables[num].name = i.ToString();
		}
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x001BD080 File Offset: 0x001BB280
	public void ReactivatePotion(string _potion)
	{
		TableGearController.TypeGear typeGear = (TableGearController.TypeGear)((int)Enum.Parse(typeof(TableGearController.TypeGear), _potion));
		int num = (int)typeGear;
		this.potionLables[num].transform.GetChild(0).GetComponent<TweenScale>().enabled = true;
		this.ReNameLabelObjects();
		this.table.Reposition();
		this.activatePotionLabel.text = LocalizationStore.Get((!Defs.isDaterRegim) ? this.keysForLabel[num] : this.keysForLabelDater[num]);
		this.activatePotionLabel.gameObject.SetActive(true);
		this.timerShowLabel = 2f;
	}

	// Token: 0x04003B82 RID: 15234
	public static TableGearController sharedController;

	// Token: 0x04003B83 RID: 15235
	public TimePotionUpdate[] potionLables;

	// Token: 0x04003B84 RID: 15236
	public UITable table;

	// Token: 0x04003B85 RID: 15237
	public UILabel activatePotionLabel;

	// Token: 0x04003B86 RID: 15238
	private string[] keysForLabel = new string[]
	{
		"Key_1813",
		"Key_1810",
		"Key_1812",
		"Key_1811"
	};

	// Token: 0x04003B87 RID: 15239
	private string[] keysForLabelDater = new string[]
	{
		"Key_1853",
		"Key_1851",
		"Key_1854",
		"Key_1852"
	};

	// Token: 0x04003B88 RID: 15240
	private float timerShowLabel = -1f;

	// Token: 0x0200084E RID: 2126
	private enum TypeGear
	{
		// Token: 0x04003B8A RID: 15242
		Turret,
		// Token: 0x04003B8B RID: 15243
		Mech,
		// Token: 0x04003B8C RID: 15244
		Jetpack,
		// Token: 0x04003B8D RID: 15245
		InvisibilityPotion
	}
}
