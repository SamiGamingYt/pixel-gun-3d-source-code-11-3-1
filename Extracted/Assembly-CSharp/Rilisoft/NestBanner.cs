using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005F0 RID: 1520
	public class NestBanner : MonoBehaviour
	{
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06003417 RID: 13335 RVA: 0x0010DAE4 File Offset: 0x0010BCE4
		// (remove) Token: 0x06003418 RID: 13336 RVA: 0x0010DB00 File Offset: 0x0010BD00
		public event Action OnClose;

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x0010DB1C File Offset: 0x0010BD1C
		public bool IsVisible
		{
			get
			{
				return this._window.activeInHierarchy;
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x0010DB2C File Offset: 0x0010BD2C
		public void Show(Egg egg)
		{
			if (this._backSubscription == null)
			{
				this._backSubscription = BackSystem.Instance.Register(new Action(this.Hide), "Nest Banner");
			}
			this._egg = egg;
			this._window.SetActiveSafe(true);
			this._eggRenderer.material.mainTexture = Resources.Load<Texture>(this._egg.GetRelativeMeshTexturePath());
			this._headerText.Text = LocalizationStore.Get("Key_2675");
			string term = string.Empty;
			switch (this._egg.Data.Rare)
			{
			case EggRarity.Simple:
				term = "Key_2534";
				break;
			case EggRarity.Ancient:
				term = "Key_2535";
				break;
			case EggRarity.Magical:
				term = "Key_2536";
				break;
			case EggRarity.Champion:
				term = "Key_2537";
				break;
			}
			this._eggNameText.Text = LocalizationStore.Get(term);
			string text;
			switch (this._egg.HatchedType)
			{
			case EggHatchedType.Time:
				this.UpdateTimedEggText();
				return;
			case EggHatchedType.Wins:
				text = string.Format("{0} {1}", LocalizationStore.Get("Key_2676"), this._egg.Data.Wins);
				goto IL_17E;
			case EggHatchedType.Rating:
				text = string.Format("{0} {1}", LocalizationStore.Get("Key_2731"), this._egg.Data.Rating);
				goto IL_17E;
			}
			text = string.Empty;
			IL_17E:
			this._conditionText.Text = text;
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x0010DCC4 File Offset: 0x0010BEC4
		private void Update()
		{
			this.UpdateTimedEggText();
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x0010DCCC File Offset: 0x0010BECC
		private void UpdateTimedEggText()
		{
			if (this._egg != null && this._egg.HatchedType == EggHatchedType.Time)
			{
				TextGroup conditionText = this._conditionText;
				long? incubationTimeLeft = this._egg.IncubationTimeLeft;
				conditionText.Text = ((incubationTimeLeft == null || incubationTimeLeft.Value <= 0L) ? LocalizationStore.Get("Key_2564") : string.Format("{0} {1}", LocalizationStore.Get("Key_2698"), EggHatchingConditionFormatter.TextForConditionOfEgg(this._egg)));
			}
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x0010DD58 File Offset: 0x0010BF58
		public void Hide()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
			this._egg = null;
			this._window.SetActiveSafe(false);
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x0010DDAC File Offset: 0x0010BFAC
		public void EnableTouchBlocker(bool enabled)
		{
			this._touchBlock.SetActiveSafe(enabled);
		}

		// Token: 0x0400264C RID: 9804
		[SerializeField]
		private Renderer _eggRenderer;

		// Token: 0x0400264D RID: 9805
		[SerializeField]
		private TextGroup _headerText;

		// Token: 0x0400264E RID: 9806
		[SerializeField]
		private TextGroup _eggNameText;

		// Token: 0x0400264F RID: 9807
		[SerializeField]
		private TextGroup _conditionText;

		// Token: 0x04002650 RID: 9808
		[SerializeField]
		private GameObject _window;

		// Token: 0x04002651 RID: 9809
		[SerializeField]
		private GameObject _touchBlock;

		// Token: 0x04002652 RID: 9810
		private Egg _egg;

		// Token: 0x04002653 RID: 9811
		private IDisposable _backSubscription;
	}
}
