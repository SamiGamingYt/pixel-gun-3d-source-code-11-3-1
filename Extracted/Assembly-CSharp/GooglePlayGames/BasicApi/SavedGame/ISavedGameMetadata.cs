using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x0200019A RID: 410
	public interface ISavedGameMetadata
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000CFF RID: 3327
		bool IsOpen { get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000D00 RID: 3328
		string Filename { get; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000D01 RID: 3329
		string Description { get; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000D02 RID: 3330
		string CoverImageURL { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000D03 RID: 3331
		TimeSpan TotalTimePlayed { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000D04 RID: 3332
		DateTime LastModifiedTimestamp { get; }
	}
}
