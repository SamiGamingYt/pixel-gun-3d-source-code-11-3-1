using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200066A RID: 1642
	public class InAppBonusLobbyButtonView : InAppBonusLobbyViewBase
	{
		// Token: 0x0600392D RID: 14637 RVA: 0x00128328 File Offset: 0x00126528
		public override void UpdateView(bool force = false)
		{
			base.UpdateView(force);
			if (base.Data == null)
			{
				return;
			}
			if (this._data.IsTypePack)
			{
				if (force || this._prevData.Pack != this._data.Pack)
				{
					this._prevData.Pack = this._data.Pack;
					this._label.text = string.Format(LocalizationStore.Get("Key_2915"), this._data.Pack);
				}
			}
			else if (force || this._prevData.End != this._data.End)
			{
				this._prevData.End = this._data.End;
				this._label.text = RiliExtensions.GetTimeStringLocalizedShort((long)this._data.End);
			}
			if (this._data.Type == BonusData.BonusType.Weapons && (force || this._prevData.WeaponId != this._data.WeaponId))
			{
				this._weaponTexture.mainTexture = ItemDb.GetItemIcon(this._data.WeaponId, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(this._data.WeaponId), null, true);
			}
			else if (this._data.Type == BonusData.BonusType.Pets && (force || this._prevData.PetId != this._data.PetId))
			{
				PetInfo info = Singleton<PetsManager>.Instance.GetInfo(this._data.PetId);
				if (info != null)
				{
					this._petTexture.mainTexture = Resources.Load<Texture>("OfferIcons/MiniOfferIcons/offer_icon_" + info.IdWithoutUp);
				}
			}
			if (this._prevData.Type == this._data.Type && !force)
			{
				return;
			}
			this._currencyObj.SetActiveSafe(false);
			this._leprechauntObj.SetActiveSafe(false);
			this._weaponObj.SetActiveSafe(false);
			this._petObj.SetActiveSafe(false);
			this._gadgetObj.SetActiveSafe(false);
			if (this._data.Type == BonusData.BonusType.Currency)
			{
				this._currencyObj.SetActiveSafe(true);
			}
			else if (this._data.Type == BonusData.BonusType.Weapons)
			{
				this._weaponObj.SetActiveSafe(true);
			}
			else if (this._data.Type == BonusData.BonusType.Pets)
			{
				this._petObj.SetActiveSafe(true);
			}
			else if (this._data.Type == BonusData.BonusType.Leprechaunt)
			{
				this._leprechauntObj.SetActiveSafe(true);
			}
			else if (this._data.Type == BonusData.BonusType.Gadgets)
			{
				this._gadgetObj.SetActiveSafe(true);
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x001285F8 File Offset: 0x001267F8
		private void OnClick()
		{
			base.Interact();
		}

		// Token: 0x040029EB RID: 10731
		[SerializeField]
		[Header("[ base ]")]
		private UILabel _label;

		// Token: 0x040029EC RID: 10732
		[SerializeField]
		[Header("[ images objects ]")]
		private GameObject _currencyObj;

		// Token: 0x040029ED RID: 10733
		[SerializeField]
		private GameObject _leprechauntObj;

		// Token: 0x040029EE RID: 10734
		[SerializeField]
		private GameObject _weaponObj;

		// Token: 0x040029EF RID: 10735
		[SerializeField]
		private GameObject _petObj;

		// Token: 0x040029F0 RID: 10736
		[SerializeField]
		private GameObject _gadgetObj;

		// Token: 0x040029F1 RID: 10737
		[Header("[ textures ]")]
		[SerializeField]
		private UITexture _weaponTexture;

		// Token: 0x040029F2 RID: 10738
		[SerializeField]
		private UITexture _petTexture;

		// Token: 0x040029F3 RID: 10739
		[SerializeField]
		private UITexture _gadgetTexture;
	}
}
