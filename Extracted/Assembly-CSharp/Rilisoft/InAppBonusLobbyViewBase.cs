using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200066E RID: 1646
	public class InAppBonusLobbyViewBase : MonoBehaviour
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x001294B0 File Offset: 0x001276B0
		public BonusData Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x001294B8 File Offset: 0x001276B8
		private void Awake()
		{
			LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.OnLocalizationChanged));
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x001294CC File Offset: 0x001276CC
		private void OnDestroy()
		{
			LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.OnLocalizationChanged));
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x001294E0 File Offset: 0x001276E0
		private void OnLocalizationChanged()
		{
			this.UpdateView(true);
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x001294EC File Offset: 0x001276EC
		public void SetData(Dictionary<string, object> bonusData)
		{
			RiliExtensions.Swap<BonusData>(ref this._data, ref this._prevData);
			this._data.Set(bonusData);
			if (base.gameObject.name != this._data.Action)
			{
				base.gameObject.name = this._data.Action;
			}
			this.UpdateView(false);
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x00129554 File Offset: 0x00127754
		public virtual void UpdateView(bool force = false)
		{
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x00129558 File Offset: 0x00127758
		public void Interact()
		{
			if (this._data == null)
			{
				return;
			}
			EventHandler handleBackFromBank = null;
			handleBackFromBank = delegate(object sender_, EventArgs e_)
			{
				if (BankController.Instance.InterfaceEnabledCoroutineLocked)
				{
					Debug.LogWarning("InterfaceEnabledCoroutineLocked");
					return;
				}
				BankController.Instance.BackRequested -= handleBackFromBank;
				BankController.Instance.InterfaceEnabled = false;
			};
			BankController.Instance.BackRequested += handleBackFromBank;
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, (!this._data.IsGems) ? "Coins" : "GemsCurrency");
		}

		// Token: 0x04002A12 RID: 10770
		[ReadOnly]
		[Header("[ debug ]")]
		[SerializeField]
		protected BonusData _data = new BonusData();

		// Token: 0x04002A13 RID: 10771
		protected BonusData _prevData = new BonusData();
	}
}
