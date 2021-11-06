using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000685 RID: 1669
	public sealed class LeaderboardItemViewModel
	{
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x0012DCD0 File Offset: 0x0012BED0
		// (set) Token: 0x06003A5C RID: 14940 RVA: 0x0012DCD8 File Offset: 0x0012BED8
		public string Id { get; set; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x0012DCE4 File Offset: 0x0012BEE4
		// (set) Token: 0x06003A5E RID: 14942 RVA: 0x0012DCEC File Offset: 0x0012BEEC
		public int Rank { get; set; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06003A5F RID: 14943 RVA: 0x0012DCF8 File Offset: 0x0012BEF8
		// (set) Token: 0x06003A60 RID: 14944 RVA: 0x0012DD00 File Offset: 0x0012BF00
		public string Nickname { get; set; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06003A61 RID: 14945 RVA: 0x0012DD0C File Offset: 0x0012BF0C
		// (set) Token: 0x06003A62 RID: 14946 RVA: 0x0012DD14 File Offset: 0x0012BF14
		public int WinCount { get; set; }

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06003A63 RID: 14947 RVA: 0x0012DD20 File Offset: 0x0012BF20
		// (set) Token: 0x06003A64 RID: 14948 RVA: 0x0012DD28 File Offset: 0x0012BF28
		public int Place { get; set; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06003A65 RID: 14949 RVA: 0x0012DD34 File Offset: 0x0012BF34
		// (set) Token: 0x06003A66 RID: 14950 RVA: 0x0012DD3C File Offset: 0x0012BF3C
		public bool Highlight { get; set; }

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003A67 RID: 14951 RVA: 0x0012DD48 File Offset: 0x0012BF48
		// (set) Token: 0x06003A68 RID: 14952 RVA: 0x0012DD50 File Offset: 0x0012BF50
		public string ClanName { get; set; }

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06003A69 RID: 14953 RVA: 0x0012DD5C File Offset: 0x0012BF5C
		// (set) Token: 0x06003A6A RID: 14954 RVA: 0x0012DD64 File Offset: 0x0012BF64
		public string ClanLogo
		{
			get
			{
				return this._clanLogo;
			}
			set
			{
				if (value == this._clanLogo)
				{
					return;
				}
				this._clanLogo = value;
				this._clanLogoTexture = new Lazy<Texture2D>(() => LeaderboardItemViewModel.CreateLogoFromBase64String(value));
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003A6B RID: 14955 RVA: 0x0012DDB8 File Offset: 0x0012BFB8
		public Texture2D ClanLogoTexture
		{
			get
			{
				if (this._clanLogoTexture.Value == null)
				{
					string currentClanLogo = this.ClanLogo;
					this._clanLogoTexture = new Lazy<Texture2D>(() => LeaderboardItemViewModel.CreateLogoFromBase64String(currentClanLogo));
				}
				return this._clanLogoTexture.Value;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003A6C RID: 14956 RVA: 0x0012DE10 File Offset: 0x0012C010
		public static LeaderboardItemViewModel Empty
		{
			get
			{
				return LeaderboardItemViewModel._empty;
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x0012DE18 File Offset: 0x0012C018
		internal static Texture2D CreateLogoFromBase64String(string logo)
		{
			if (string.IsNullOrEmpty(logo))
			{
				return null;
			}
			Texture2D result;
			try
			{
				byte[] data = Convert.FromBase64String(logo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false)
				{
					filterMode = FilterMode.Point
				};
				texture2D.LoadImage(data);
				texture2D.Apply();
				result = texture2D;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				result = null;
			}
			return result;
		}

		// Token: 0x04002AEF RID: 10991
		private static LeaderboardItemViewModel _empty = new LeaderboardItemViewModel
		{
			Id = string.Empty,
			Nickname = string.Empty,
			ClanLogo = string.Empty
		};

		// Token: 0x04002AF0 RID: 10992
		private string _clanLogo = string.Empty;

		// Token: 0x04002AF1 RID: 10993
		private Lazy<Texture2D> _clanLogoTexture = new Lazy<Texture2D>(() => null);
	}
}
