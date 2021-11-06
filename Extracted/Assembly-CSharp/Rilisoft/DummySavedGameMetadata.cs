using System;
using GooglePlayGames.BasicApi.SavedGame;

namespace Rilisoft
{
	// Token: 0x02000836 RID: 2102
	internal sealed class DummySavedGameMetadata : ISavedGameMetadata
	{
		// Token: 0x06004C5C RID: 19548 RVA: 0x001B85AC File Offset: 0x001B67AC
		public DummySavedGameMetadata(string filename)
		{
			this._filename = (filename ?? string.Empty);
			this.LastModifiedTimestamp = DateTime.Now;
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06004C5D RID: 19549 RVA: 0x001B85E0 File Offset: 0x001B67E0
		public string CoverImageURL
		{
			get
			{
				return "http://example.com";
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06004C5E RID: 19550 RVA: 0x001B85E8 File Offset: 0x001B67E8
		public string Description
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06004C5F RID: 19551 RVA: 0x001B85F8 File Offset: 0x001B67F8
		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x001B8600 File Offset: 0x001B6800
		public bool IsOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x001B8604 File Offset: 0x001B6804
		// (set) Token: 0x06004C62 RID: 19554 RVA: 0x001B860C File Offset: 0x001B680C
		public DateTime LastModifiedTimestamp { get; private set; }

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x001B8618 File Offset: 0x001B6818
		public TimeSpan TotalTimePlayed
		{
			get
			{
				return TimeSpan.Zero;
			}
		}

		// Token: 0x04003B42 RID: 15170
		private string _filename;
	}
}
