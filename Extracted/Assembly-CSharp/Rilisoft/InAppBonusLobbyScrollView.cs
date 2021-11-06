using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200066D RID: 1645
	public class InAppBonusLobbyScrollView : InAppBonusLobbyViewBase
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x00128F14 File Offset: 0x00127114
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
					string term = (this._data.Type != BonusData.BonusType.Currency && this._data.Type != BonusData.BonusType.Gadgets) ? "Key_2896" : "Key_2864";
					this._packsLabel.text = string.Format(LocalizationStore.Get(term), this._data.Pack);
				}
			}
			else if (force || this._prevData.End != this._data.End)
			{
				this._prevData.End = this._data.End;
				string text = (this._data.End < 86400) ? RiliExtensions.GetTimeString((long)this._data.End, ":") : string.Format("{0} {1}", LocalizationStore.Get("Key_1125"), RiliExtensions.GetTimeStringDays((long)this._data.End));
				this._packsLabel.text = text;
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
					this._descriptionLabel.text = string.Format(LocalizationStore.Get("Key_2901"), LocalizationStore.Get(info.Lkey));
					this._petTexture.mainTexture = ItemDb.GetItemIcon(info.IdWithoutUp, ShopNGUIController.CategoryNames.PetsCategory, null, true);
				}
			}
			else if (this._data.Type == BonusData.BonusType.Gadgets && (force || (this._data.Gadgets.Any<string>() && this._gadgetsTextures.Any<UITexture>() && this._data.Gadgets.Any((string g) => this._prevData.Gadgets.All((string pg) => pg != g)))))
			{
				List<string> list = GadgetsInfo.AvailableForBuyGadgets(ExpController.GetOurTier()).ToList<string>();
				list = list.Intersect(this._data.Gadgets).ToList<string>();
				for (int i = 0; i < this._gadgetsTextures.Count; i++)
				{
					if (i < list.Count<string>())
					{
						string key = list[i];
						if (GadgetsInfo.info.ContainsKey(key))
						{
							GadgetInfo gadgetInfo = GadgetsInfo.info[key];
							this._gadgetsTextures[i].mainTexture = ItemDb.GetItemIcon(gadgetInfo.Id, (ShopNGUIController.CategoryNames)gadgetInfo.Category, null, true);
						}
					}
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
			this._gadgetsObj.SetActiveSafe(false);
			if (this._data.Type == BonusData.BonusType.Currency)
			{
				this._currencyObj.SetActiveSafe(true);
				this._headerLabel.text = LocalizationStore.Get("Key_2875");
				this._descriptionLabel.text = LocalizationStore.Get("Key_2906");
			}
			else if (this._data.Type == BonusData.BonusType.Weapons)
			{
				this._weaponObj.SetActiveSafe(true);
				this._headerLabel.text = LocalizationStore.Get("Key_2892");
				this._descriptionLabel.text = LocalizationStore.Get("Key_2893");
			}
			else if (this._data.Type == BonusData.BonusType.Pets)
			{
				this._petObj.SetActiveSafe(true);
				this._headerLabel.text = LocalizationStore.Get("Key_2882");
			}
			else if (this._data.Type == BonusData.BonusType.Leprechaunt)
			{
				this._leprechauntObj.SetActiveSafe(true);
				this._headerLabel.text = LocalizationStore.Get("Key_2900");
				this._descriptionLabel.text = LocalizationStore.Get("Key_2899");
			}
			else if (this._data.Type == BonusData.BonusType.Gadgets)
			{
				this._gadgetsObj.SetActiveSafe(true);
				this._headerLabel.text = LocalizationStore.Get("Key_2883");
				this._descriptionLabel.text = LocalizationStore.Get("Key_2897");
			}
		}

		// Token: 0x04002A07 RID: 10759
		[Header("[ base ]")]
		[SerializeField]
		private UILabel _packsLabel;

		// Token: 0x04002A08 RID: 10760
		[SerializeField]
		private UILabel _descriptionLabel;

		// Token: 0x04002A09 RID: 10761
		[SerializeField]
		private UILabel _headerLabel;

		// Token: 0x04002A0A RID: 10762
		[SerializeField]
		[Header("[ images objects ]")]
		private GameObject _currencyObj;

		// Token: 0x04002A0B RID: 10763
		[SerializeField]
		private GameObject _leprechauntObj;

		// Token: 0x04002A0C RID: 10764
		[SerializeField]
		private GameObject _weaponObj;

		// Token: 0x04002A0D RID: 10765
		[SerializeField]
		private GameObject _petObj;

		// Token: 0x04002A0E RID: 10766
		[SerializeField]
		private GameObject _gadgetsObj;

		// Token: 0x04002A0F RID: 10767
		[SerializeField]
		[Header("[ textures ]")]
		private UITexture _weaponTexture;

		// Token: 0x04002A10 RID: 10768
		[SerializeField]
		private UITexture _petTexture;

		// Token: 0x04002A11 RID: 10769
		[SerializeField]
		private List<UITexture> _gadgetsTextures;
	}
}
