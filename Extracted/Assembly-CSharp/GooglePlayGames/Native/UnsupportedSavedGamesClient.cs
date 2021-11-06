using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x02000285 RID: 645
	internal class UnsupportedSavedGamesClient : ISavedGameClient
	{
		// Token: 0x060014B4 RID: 5300 RVA: 0x00052048 File Offset: 0x00050248
		public UnsupportedSavedGamesClient(string message)
		{
			this.mMessage = Misc.CheckNotNull<string>(message);
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0005205C File Offset: 0x0005025C
		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0005206C File Offset: 0x0005026C
		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0005207C File Offset: 0x0005027C
		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0005208C File Offset: 0x0005028C
		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005209C File Offset: 0x0005029C
		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x000520AC File Offset: 0x000502AC
		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000520BC File Offset: 0x000502BC
		public void Delete(ISavedGameMetadata metadata)
		{
			throw new NotImplementedException(this.mMessage);
		}

		// Token: 0x04000C13 RID: 3091
		private readonly string mMessage;
	}
}
