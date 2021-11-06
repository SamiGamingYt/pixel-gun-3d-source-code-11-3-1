using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x0200064B RID: 1611
public class GiftHUDItem : MonoBehaviour
{
	// Token: 0x06003802 RID: 14338 RVA: 0x00120D3C File Offset: 0x0011EF3C
	private void OnEnable()
	{
		if (this.colliderForDrag == null)
		{
			this.colliderForDrag = base.GetComponent<BoxCollider>();
		}
		if (!this.isInfo)
		{
			base.StartCoroutine(this.ActiveSkinAfterWait());
		}
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x00120D74 File Offset: 0x0011EF74
	public void SetInfoButton(SlotInfo curInfo)
	{
		this._data = curInfo;
		if (this._data == null)
		{
			Debug.LogError("SetInfoButton");
			return;
		}
		if (this.sprIcon)
		{
			this.sprIcon.gameObject.SetActive(false);
		}
		if (this.textureIcon)
		{
			this.textureIcon.gameObject.SetActive(false);
		}
		if (this.skinModelTransform != null)
		{
			UnityEngine.Object.Destroy(this.skinModelTransform.gameObject);
			this.skinModelTransform = null;
		}
		string text = (this._data.CountGift <= 1) ? string.Empty : (this._data.CountGift + " ");
		switch (this._data.category.Type)
		{
		case GiftCategoryType.Coins:
			this.nameAndCountGift = text + LocalizationStore.Get("Key_0275");
			break;
		case GiftCategoryType.Gems:
			this.nameAndCountGift = text + LocalizationStore.Get("Key_0951");
			break;
		case GiftCategoryType.Grenades:
		case GiftCategoryType.Gear:
		case GiftCategoryType.ArmorAndHat:
		case GiftCategoryType.Wear:
		case GiftCategoryType.Masks:
		case GiftCategoryType.Capes:
		case GiftCategoryType.Boots:
		case GiftCategoryType.Hats_random:
		case GiftCategoryType.Gun1:
		case GiftCategoryType.Gun2:
		case GiftCategoryType.Gun3:
		case GiftCategoryType.Gun4:
		case GiftCategoryType.Guns_gray:
		{
			string str = text;
			string id = this._data.gift.Id;
			ShopNGUIController.CategoryNames? typeShopCat = this._data.gift.TypeShopCat;
			this.nameAndCountGift = str + RespawnWindowItemToBuy.GetItemName(id, (typeShopCat == null) ? ShopNGUIController.CategoryNames.ArmorCategory : typeShopCat.Value, 0);
			break;
		}
		case GiftCategoryType.Skins:
			this.nameAndCountGift = SkinsController.skinsNamesForPers[this._data.gift.Id];
			break;
		case GiftCategoryType.Event_content:
			break;
		case GiftCategoryType.Editor:
			if (this._data.gift.Id == "editor_Cape")
			{
				this.nameAndCountGift = LocalizationStore.Get("Key_0746");
			}
			else if (this._data.gift.Id == "editor_Skin")
			{
				this.nameAndCountGift = LocalizationStore.Get("Key_0086");
			}
			else
			{
				Debug.LogError(string.Format("[GIFT] unknown gift id: '{0}'", this._data.gift.Id));
			}
			break;
		case GiftCategoryType.Stickers:
			if (this._data.gift.Id == "classic")
			{
				this.nameAndCountGift = LocalizationStore.Get("Key_1756");
			}
			else if (this._data.gift.Id == "christmas")
			{
				this.nameAndCountGift = LocalizationStore.Get("Key_1758");
			}
			break;
		case GiftCategoryType.Freespins:
			this.nameAndCountGift = string.Format(LocalizationStore.Get("Key_2196"), this._data.CountGift);
			break;
		case GiftCategoryType.WeaponSkin:
		{
			string lkey = WeaponSkinsManager.GetSkin(this._data.gift.Id).Lkey;
			this.nameAndCountGift = text + LocalizationStore.Get(lkey);
			break;
		}
		case GiftCategoryType.Gadgets:
			if (GadgetsInfo.info.ContainsKey(this._data.gift.Id))
			{
				GadgetInfo gadgetInfo = GadgetsInfo.info[this._data.gift.Id];
				this.nameAndCountGift = text + ItemDb.GetItemName(gadgetInfo.Id, (ShopNGUIController.CategoryNames)gadgetInfo.Category);
			}
			else
			{
				Debug.LogErrorFormat("not found gadget: '{0}'", new object[]
				{
					this._data.gift.Id
				});
			}
			break;
		default:
			this.nameAndCountGift = text + LocalizationStore.Get(this._data.gift.KeyTranslateInfo);
			break;
		}
		if (this.nameGift)
		{
			this.nameGift.text = this.nameAndCountGift;
		}
		if (this.lbInfoGift != null)
		{
			if (this._data.category.Type == GiftCategoryType.Gadgets)
			{
				if (GadgetsInfo.info.ContainsKey(this._data.gift.Id))
				{
					GadgetInfo gadgetInfo2 = GadgetsInfo.info[this._data.gift.Id];
					this.lbInfoGift.text = LocalizationStore.Get(gadgetInfo2.DescriptionLkey);
					this.lbInfoGift.gameObject.SetActive(true);
				}
				else
				{
					Debug.LogErrorFormat("not found gadget: '{0}'", new object[]
					{
						this._data.gift.Id
					});
				}
			}
			else if (!string.IsNullOrEmpty(this._data.gift.KeyTranslateInfo))
			{
				this.lbInfoGift.text = ScriptLocalization.Get(this._data.gift.KeyTranslateInfo);
				this.lbInfoGift.gameObject.SetActive(true);
			}
			else if (!string.IsNullOrEmpty(this._data.category.KeyTranslateInfoCommon))
			{
				this.lbInfoGift.text = ScriptLocalization.Get(this._data.category.KeyTranslateInfoCommon);
				this.lbInfoGift.gameObject.SetActive(true);
			}
			else
			{
				this.lbInfoGift.gameObject.SetActive(false);
			}
		}
		switch (this._data.category.Type)
		{
		case GiftCategoryType.Skins:
			if (this.parentForSkin)
			{
				if (!this.isInfo)
				{
					this.parentForSkin.layer = LayerMask.NameToLayer("FriendsWindowGUI");
				}
				this.skinModelTransform = SkinsController.SkinModel(this._data.gift.Id, 1, this.parentForSkin.transform, this.offsetSkin, this.scaleSkin);
			}
			return;
		case GiftCategoryType.Event_content:
			return;
		}
		this.SetImage();
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x00121394 File Offset: 0x0011F594
	private void SetImage()
	{
		Texture texture = null;
		string spriteName = null;
		switch (this._data.category.Type)
		{
		case GiftCategoryType.Coins:
			texture = Resources.Load<Texture>("OfferIcons/Marathon/bonus_coins");
			break;
		case GiftCategoryType.Gems:
			texture = Resources.Load<Texture>("OfferIcons/Marathon/bonus_gems");
			break;
		case GiftCategoryType.Grenades:
		{
			string str = string.Empty;
			string id = this._data.gift.Id;
			if (id != null)
			{
				if (GiftHUDItem.<>f__switch$mapC == null)
				{
					GiftHUDItem.<>f__switch$mapC = new Dictionary<string, int>(2)
					{
						{
							"GrenadeID",
							0
						},
						{
							"LikeID",
							1
						}
					};
				}
				int num;
				if (GiftHUDItem.<>f__switch$mapC.TryGetValue(id, out num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							str = "LikeID";
						}
					}
					else
					{
						str = "Marathon/bonus_grenade";
					}
				}
			}
			texture = Resources.Load<Texture>("OfferIcons/" + str);
			break;
		}
		case GiftCategoryType.Gear:
		{
			string str2 = string.Empty;
			if (this._data.gift.Id.Equals("MusicBox"))
			{
				str2 = "Dater_bonus_turret";
			}
			if (this._data.gift.Id.Equals("Wings"))
			{
				str2 = "Dater_bonus_jetpack";
			}
			if (this._data.gift.Id.Equals("Bear"))
			{
				str2 = "Dater_bonus_mech";
			}
			if (this._data.gift.Id.Equals("BigHeadPotion"))
			{
				str2 = "Dater_bonus_potion";
			}
			texture = Resources.Load<Texture>("OfferIcons/Marathon/" + str2);
			break;
		}
		case GiftCategoryType.Skins:
			return;
		case GiftCategoryType.ArmorAndHat:
		case GiftCategoryType.Wear:
		case GiftCategoryType.Masks:
		case GiftCategoryType.Capes:
		case GiftCategoryType.Boots:
		case GiftCategoryType.Hats_random:
		case GiftCategoryType.Gun1:
		case GiftCategoryType.Gun2:
		case GiftCategoryType.Gun3:
		case GiftCategoryType.Gun4:
		case GiftCategoryType.Guns_gray:
		{
			ShopNGUIController.CategoryNames category = (this._data.gift.TypeShopCat == null) ? ShopNGUIController.CategoryNames.ArmorCategory : this._data.gift.TypeShopCat.Value;
			texture = ItemDb.GetItemIcon(this._data.gift.Id, category, null, true);
			break;
		}
		case GiftCategoryType.Editor:
			if (this._data.gift.Id == "editor_Cape")
			{
				texture = Resources.Load<Texture>("OfferIcons/editor_win_cape");
			}
			else if (this._data.gift.Id == "editor_Skin")
			{
				texture = Resources.Load<Texture>("OfferIcons/editor_win_skin2");
			}
			else
			{
				Debug.LogError(string.Format("[GIFT] unknown gift id: '{0}'", this._data.gift.Id));
			}
			break;
		case GiftCategoryType.Stickers:
		{
			TypePackSticker value = this._data.gift.Id.ToEnum(null).Value;
			if (value != TypePackSticker.classic)
			{
				if (value == TypePackSticker.christmas)
				{
					texture = Resources.Load<Texture>("OfferIcons/free_christmas_smile");
				}
			}
			else
			{
				texture = Resources.Load<Texture>("OfferIcons/free_smile");
			}
			break;
		}
		case GiftCategoryType.Freespins:
			texture = Resources.Load<Texture>(string.Format("OfferIcons/free_spin_{0}", this._data.gift.Count.Value));
			break;
		case GiftCategoryType.WeaponSkin:
			texture = ItemDb.GetItemIcon(this._data.gift.Id, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory, null, true);
			break;
		case GiftCategoryType.Gadgets:
			if (GadgetsInfo.info.ContainsKey(this._data.gift.Id))
			{
				GadgetInfo gadgetInfo = GadgetsInfo.info[this._data.gift.Id];
				texture = ItemDb.GetItemIcon(gadgetInfo.Id, (ShopNGUIController.CategoryNames)gadgetInfo.Category, null, true);
			}
			else
			{
				Debug.LogErrorFormat("not found gadget: '{0}'", new object[]
				{
					this._data.gift.Id
				});
			}
			break;
		}
		if (texture != null)
		{
			this.textureIcon.mainTexture = texture;
			this.textureIcon.gameObject.SetActive(true);
			this.sprIcon.gameObject.SetActive(false);
		}
		else
		{
			this.sprIcon.spriteName = spriteName;
			this.sprIcon.gameObject.SetActive(true);
			this.textureIcon.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x00121830 File Offset: 0x0011FA30
	private IEnumerator ActiveSkinAfterWait()
	{
		while (this.skinModelTransform == null)
		{
			yield return null;
		}
		this.skinModelTransform.gameObject.SetActive(false);
		while (!(GiftBannerWindow.instance != null) || !GiftBannerWindow.instance.bannerObj.activeSelf)
		{
			yield return null;
		}
		yield return null;
		this.skinModelTransform.gameObject.SetActive(true);
		yield break;
		yield break;
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x0012184C File Offset: 0x0011FA4C
	public void InCenter(bool anim = false, int countBut = 1)
	{
		UIScrollView componentInParent = base.GetComponentInParent<UIScrollView>();
		if (componentInParent == null)
		{
			return;
		}
		Transform transform = base.transform;
		Vector3[] worldCorners = componentInParent.panel.worldCorners;
		Vector3 position = (worldCorners[2] + worldCorners[0]) * 0.5f;
		if (transform != null && componentInParent != null && componentInParent.panel != null)
		{
			Transform cachedTransform = componentInParent.panel.cachedTransform;
			GameObject gameObject = transform.gameObject;
			Vector3 a = cachedTransform.InverseTransformPoint(transform.position);
			Vector3 b = cachedTransform.InverseTransformPoint(position);
			Vector3 b2 = a - b;
			if (!componentInParent.canMoveHorizontally)
			{
				b2.x = 0f;
			}
			if (!componentInParent.canMoveVertically)
			{
				b2.y = 0f;
			}
			b2.z = 0f;
			if (anim)
			{
				Vector3 offset = cachedTransform.localPosition - b2;
				base.StartCoroutine(this.Crt_Anim_InCenter(componentInParent.panel.cachedGameObject, offset, (float)(countBut * 130)));
			}
			else
			{
				Vector3 zero = Vector3.zero;
				if (componentInParent.transform.localPosition.Equals(cachedTransform.localPosition - b2))
				{
					zero = new Vector3(1f, 0f, 0f);
				}
				SpringPanel.Begin(componentInParent.gameObject, cachedTransform.localPosition - b2 + zero, 10f);
			}
		}
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x001219F0 File Offset: 0x0011FBF0
	private void FastCenter(UIScrollView scroll, Vector3 needPos)
	{
		float deltaTime = RealTime.deltaTime;
		Vector3 localPosition = scroll.transform.localPosition;
		scroll.transform.localPosition = needPos;
		Vector3 vector = needPos - localPosition;
		Vector2 clipOffset = scroll.panel.clipOffset;
		clipOffset.x -= vector.x;
		clipOffset.y -= vector.y;
		scroll.panel.clipOffset = clipOffset;
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x00121A6C File Offset: 0x0011FC6C
	private IEnumerator Crt_Anim_InCenter(GameObject obj, Vector3 offset, float width)
	{
		base.StartCoroutine(this.Crt_TimerAnim());
		float speedAnim = 0f;
		Vector3 animOffset = new Vector3(width * 5f, 0f, 0f) + offset;
		while (!this.endAnim)
		{
			if (speedAnim < 1f)
			{
				speedAnim += 0.05f;
			}
			SpringPanel.Begin(obj, animOffset, speedAnim);
			yield return new WaitForEndOfFrame();
		}
		SpringPanel.Begin(obj, animOffset, 1f);
		yield break;
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x00121AB4 File Offset: 0x0011FCB4
	private IEnumerator Crt_TimerAnim()
	{
		this.endAnim = false;
		yield return new WaitForSeconds(1.5f);
		this.endAnim = true;
		yield break;
	}

	// Token: 0x040028CD RID: 10445
	public bool isInfo;

	// Token: 0x040028CE RID: 10446
	public UISprite sprIcon;

	// Token: 0x040028CF RID: 10447
	public UITexture textureIcon;

	// Token: 0x040028D0 RID: 10448
	public UILabel nameGift;

	// Token: 0x040028D1 RID: 10449
	public GameObject parentForSkin;

	// Token: 0x040028D2 RID: 10450
	public BoxCollider colliderForDrag;

	// Token: 0x040028D3 RID: 10451
	public UILabel lbInfoGift;

	// Token: 0x040028D4 RID: 10452
	private Transform skinModelTransform;

	// Token: 0x040028D5 RID: 10453
	[SerializeField]
	[ReadOnly]
	private string nameAndCountGift = string.Empty;

	// Token: 0x040028D6 RID: 10454
	private Vector3 offsetSkin = new Vector3(0f, -44.12f, 0f);

	// Token: 0x040028D7 RID: 10455
	private Vector3 scaleSkin = new Vector3(45f, 45f, 45f);

	// Token: 0x040028D8 RID: 10456
	private bool endAnim;

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	private SlotInfo _data;
}
