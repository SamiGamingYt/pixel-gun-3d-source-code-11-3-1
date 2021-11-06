using System;

namespace Rilisoft
{
	// Token: 0x02000544 RID: 1348
	internal sealed class PerelivSettings
	{
		// Token: 0x06002EE8 RID: 12008 RVA: 0x000F5348 File Offset: 0x000F3548
		public PerelivSettings(bool enabled, string imageUrl, string redirectUrl, string text, bool showAlways, int minLevel, int maxLevel, bool buttonClose)
		{
			this._enabled = enabled;
			this._imageUrl = (imageUrl ?? string.Empty);
			this._redirectUrl = (redirectUrl ?? string.Empty);
			this._text = (text ?? string.Empty);
			this._showAlways = showAlways;
			this._minLevel = minLevel;
			this._maxLevel = maxLevel;
			this._buttonClose = buttonClose;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000F53E8 File Offset: 0x000F35E8
		public PerelivSettings(string error)
		{
			this._error = (error ?? string.Empty);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000F543C File Offset: 0x000F363C
		private PerelivSettings()
		{
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002EEC RID: 12012 RVA: 0x000F5488 File Offset: 0x000F3688
		public static PerelivSettings Default
		{
			get
			{
				return PerelivSettings.s_default;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x000F5490 File Offset: 0x000F3690
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x000F5498 File Offset: 0x000F3698
		public string ImageUrl
		{
			get
			{
				return this._imageUrl;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000F54A0 File Offset: 0x000F36A0
		public string RedirectUrl
		{
			get
			{
				return this._redirectUrl;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002EF0 RID: 12016 RVA: 0x000F54A8 File Offset: 0x000F36A8
		public string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x000F54B0 File Offset: 0x000F36B0
		public bool ShowAlways
		{
			get
			{
				return this._showAlways;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x000F54B8 File Offset: 0x000F36B8
		public int MinLevel
		{
			get
			{
				return this._minLevel;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x000F54C0 File Offset: 0x000F36C0
		public int MaxLevel
		{
			get
			{
				return this._maxLevel;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x000F54C8 File Offset: 0x000F36C8
		public bool ButtonClose
		{
			get
			{
				return this._buttonClose;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002EF5 RID: 12021 RVA: 0x000F54D0 File Offset: 0x000F36D0
		public string Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x040022A1 RID: 8865
		private readonly bool _enabled;

		// Token: 0x040022A2 RID: 8866
		private readonly string _imageUrl = string.Empty;

		// Token: 0x040022A3 RID: 8867
		private readonly string _redirectUrl = string.Empty;

		// Token: 0x040022A4 RID: 8868
		private readonly string _text = string.Empty;

		// Token: 0x040022A5 RID: 8869
		private readonly bool _showAlways;

		// Token: 0x040022A6 RID: 8870
		private readonly int _minLevel;

		// Token: 0x040022A7 RID: 8871
		private readonly int _maxLevel;

		// Token: 0x040022A8 RID: 8872
		private readonly bool _buttonClose;

		// Token: 0x040022A9 RID: 8873
		private readonly string _error = string.Empty;

		// Token: 0x040022AA RID: 8874
		private static readonly PerelivSettings s_default = new PerelivSettings();
	}
}
