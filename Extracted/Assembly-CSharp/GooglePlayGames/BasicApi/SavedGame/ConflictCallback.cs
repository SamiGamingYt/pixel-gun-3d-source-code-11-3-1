using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x02000933 RID: 2355
	// (Invoke) Token: 0x0600516C RID: 20844
	public delegate void ConflictCallback(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData);
}
