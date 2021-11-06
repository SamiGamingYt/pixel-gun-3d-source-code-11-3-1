using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000715 RID: 1813
	[Obsolete]
	internal sealed class CloudCleaner
	{
		// Token: 0x06003F3F RID: 16191 RVA: 0x001528B8 File Offset: 0x00150AB8
		public void CleanSavedGameFile(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentException("Filename should not be empty", filename);
			}
			if (GpgFacade.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				return;
			}
			Action<ISavedGameMetadata> commit = delegate(ISavedGameMetadata metadata)
			{
				byte[] updatedBinaryData = new byte[0];
				SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(string.Format("Cleaned by '{0}'", SystemInfo.deviceModel)).Build();
				GpgFacade.Instance.SavedGame.CommitUpdate(metadata, updateForMetadata, updatedBinaryData, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
				{
					Debug.LogFormat("------ Cleaned after conflict '{0}': {1} '{2}'", new object[]
					{
						filename,
						writeStatus,
						closeMetadata.GetDescription()
					});
				});
			};
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = delegate(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				Debug.LogFormat("------ Open '{0}': {1} '{2}'", new object[]
				{
					filename,
					openStatus,
					openMetadata.GetDescription()
				});
				if (openStatus != SavedGameRequestStatus.Success)
				{
					return;
				}
				commit(openMetadata);
			};
			ConflictCallback conflictCallback = delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				resolver.ChooseMetadata(unmerged);
				Debug.LogFormat("------ Partially resolved using unmerged metadata '{0}': '{1}'", new object[]
				{
					filename,
					unmerged.GetDescription()
				});
				commit(unmerged);
			};
			Debug.LogFormat("------ Trying to open '{0}'...", new object[]
			{
				filename
			});
			GpgFacade.Instance.SavedGame.OpenWithManualConflictResolution(filename, DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x0015296C File Offset: 0x00150B6C
		public static CloudCleaner Instance
		{
			get
			{
				if (CloudCleaner._instance == null)
				{
					CloudCleaner._instance = new CloudCleaner();
				}
				return CloudCleaner._instance;
			}
		}

		// Token: 0x04002E8F RID: 11919
		private static CloudCleaner _instance;
	}
}
