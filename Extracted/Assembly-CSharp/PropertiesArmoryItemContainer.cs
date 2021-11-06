using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class PropertiesArmoryItemContainer : MonoBehaviour
{
	// Token: 0x04001FE8 RID: 8168
	public GameObject needTierPet;

	// Token: 0x04001FE9 RID: 8169
	public UILabel needTierPetLabel;

	// Token: 0x04001FEA RID: 8170
	public GameObject needMoreTrophiesPanel;

	// Token: 0x04001FEB RID: 8171
	public GameObject renamePetButton;

	// Token: 0x04001FEC RID: 8172
	public UITable specialTable;

	// Token: 0x04001FED RID: 8173
	public UITable gadgetPropertyTable;

	// Token: 0x04001FEE RID: 8174
	public GameObject weaponProperties;

	// Token: 0x04001FEF RID: 8175
	public GameObject meleeProperties;

	// Token: 0x04001FF0 RID: 8176
	public GameObject specialParams;

	// Token: 0x04001FF1 RID: 8177
	public GameObject nonArmorWearProperties;

	// Token: 0x04001FF2 RID: 8178
	public GameObject armorWearProperties;

	// Token: 0x04001FF3 RID: 8179
	public GameObject skinProperties;

	// Token: 0x04001FF4 RID: 8180
	public GameObject gadgetProperties;

	// Token: 0x04001FF5 RID: 8181
	public UILabel fireRate;

	// Token: 0x04001FF6 RID: 8182
	public UILabel fireRateMElee;

	// Token: 0x04001FF7 RID: 8183
	public UILabel mobility;

	// Token: 0x04001FF8 RID: 8184
	public UILabel mobilityMelee;

	// Token: 0x04001FF9 RID: 8185
	public UILabel capacity;

	// Token: 0x04001FFA RID: 8186
	public UILabel damage;

	// Token: 0x04001FFB RID: 8187
	public UILabel damageMelee;

	// Token: 0x04001FFC RID: 8188
	public UILabel weaponsRarityLabel;

	// Token: 0x04001FFD RID: 8189
	public UILabel descriptionGadget;

	// Token: 0x04001FFE RID: 8190
	public UIButton upgradeButton;

	// Token: 0x04001FFF RID: 8191
	public UIButton trainButton;

	// Token: 0x04002000 RID: 8192
	public UIButton buyButton;

	// Token: 0x04002001 RID: 8193
	public UIButton equipButton;

	// Token: 0x04002002 RID: 8194
	public UIButton unequipButton;

	// Token: 0x04002003 RID: 8195
	public UIButton infoButton;

	// Token: 0x04002004 RID: 8196
	public UIButton editButton;

	// Token: 0x04002005 RID: 8197
	public UIButton deleteButton;

	// Token: 0x04002006 RID: 8198
	public UIButton enableButton;

	// Token: 0x04002007 RID: 8199
	public UIButton createButton;

	// Token: 0x04002008 RID: 8200
	public UIButton tryGun;

	// Token: 0x04002009 RID: 8201
	public GameObject equipped;

	// Token: 0x0400200A RID: 8202
	public GameObject needTier;

	// Token: 0x0400200B RID: 8203
	public UILabel needTierLabel;

	// Token: 0x0400200C RID: 8204
	public GameObject needBuyPrevious;

	// Token: 0x0400200D RID: 8205
	public UILabel needBuyPreviousLabel;

	// Token: 0x0400200E RID: 8206
	public List<UISprite> effectsSprites;

	// Token: 0x0400200F RID: 8207
	public List<UILabel> effectsLabels;

	// Token: 0x04002010 RID: 8208
	public List<GadgetPropertyItem> gadgetsPropertiesList;

	// Token: 0x04002011 RID: 8209
	public UILabel nonArmorWearDEscription;

	// Token: 0x04002012 RID: 8210
	public UILabel armorWearDescription;

	// Token: 0x04002013 RID: 8211
	public UILabel armorCountLabel;

	// Token: 0x04002014 RID: 8212
	public UILabel gadgetNameLabel;

	// Token: 0x04002015 RID: 8213
	public GameObject tryGunPanel;

	// Token: 0x04002016 RID: 8214
	public UILabel tryGunMatchesCount;

	// Token: 0x04002017 RID: 8215
	public UILabel tryGunDiscountTime;

	// Token: 0x04002018 RID: 8216
	public PriceContainer price;

	// Token: 0x04002019 RID: 8217
	public GameObject discountPanel;

	// Token: 0x0400201A RID: 8218
	public UILabel discountLabel;
}
