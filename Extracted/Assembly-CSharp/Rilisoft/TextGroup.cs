using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200076E RID: 1902
	[ExecuteInEditMode]
	public class TextGroup : MonoBehaviour
	{
		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060042FF RID: 17151 RVA: 0x00166418 File Offset: 0x00164618
		// (set) Token: 0x06004300 RID: 17152 RVA: 0x00166420 File Offset: 0x00164620
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				if (this._labels == null || !this._labels.Any<UILabel>())
				{
					this.SetLabels();
				}
				this._labels.ForEach(delegate(UILabel l)
				{
					l.text = this._text;
				});
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x0016646C File Offset: 0x0016466C
		// (set) Token: 0x06004302 RID: 17154 RVA: 0x00166474 File Offset: 0x00164674
		public string LocalizationKey
		{
			get
			{
				return this._localizationKey;
			}
			set
			{
				this._localizationKey = value;
				if (!this._localizationKey.IsNullOrEmpty())
				{
					if (this.UseLocalizationComponents)
					{
						this.SetLocalizeComponents();
					}
					else
					{
						this.Text = LocalizationStore.Get(value);
					}
				}
				else
				{
					this.Text = this._text;
				}
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06004303 RID: 17155 RVA: 0x001664CC File Offset: 0x001646CC
		public bool UseLocalizationComponents
		{
			get
			{
				return base.GetComponent<Localize>() != null;
			}
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x001664DC File Offset: 0x001646DC
		private void OnEnable()
		{
			LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
			this.SetLabels();
			if (this.UseLocalizationComponents)
			{
				this.SetLocalizeComponents();
			}
			else if (!this.LocalizationKey.IsNullOrEmpty())
			{
				this.Text = LocalizationStore.Get(this.LocalizationKey);
			}
			else
			{
				this.Text = this._text;
			}
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00166548 File Offset: 0x00164748
		private void OnDisable()
		{
			LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0016655C File Offset: 0x0016475C
		private void SetLabels()
		{
			if (this._labels == null)
			{
				this._labels = new List<UILabel>();
			}
			else
			{
				this._labels.Clear();
			}
			this._labels.AddRange(base.GetComponentsInChildren<UILabel>(true));
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x001665A4 File Offset: 0x001647A4
		private void HandleLocalizationChanged()
		{
			if (!this.LocalizationKey.IsNullOrEmpty())
			{
				if (this.UseLocalizationComponents)
				{
					this.SetLocalizeComponents();
				}
				else
				{
					this.Text = LocalizationStore.Get(this.LocalizationKey);
				}
			}
			else
			{
				this.Text = this._text;
			}
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x001665FC File Offset: 0x001647FC
		private void SetLocalizeComponents()
		{
			foreach (UILabel uilabel in this._labels)
			{
				Localize localize = uilabel.gameObject.GetComponent<Localize>();
				if (localize == null)
				{
					localize = uilabel.gameObject.AddComponent<Localize>();
				}
				localize.Term = this.LocalizationKey;
			}
			this.Text = LocalizationStore.Get(this.LocalizationKey);
		}

		// Token: 0x04003115 RID: 12565
		[SerializeField]
		[ReadOnly]
		private List<UILabel> _labels = new List<UILabel>();

		// Token: 0x04003116 RID: 12566
		[SerializeField]
		private string _text;

		// Token: 0x04003117 RID: 12567
		[SerializeField]
		private string _localizationKey;
	}
}
