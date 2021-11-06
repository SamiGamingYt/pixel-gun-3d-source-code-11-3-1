using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000835 RID: 2101
	internal sealed class DummySavedGameClient : ISavedGameClient
	{
		// Token: 0x06004C52 RID: 19538 RVA: 0x001B81C4 File Offset: 0x001B63C4
		public DummySavedGameClient(string filename)
		{
			this._filename = (filename ?? string.Empty);
			this._dummySavedGameMetadata = new DummySavedGameMetadata(this._filename);
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x001B81FC File Offset: 0x001B63FC
		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x001B8204 File Offset: 0x001B6404
		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Debug.LogFormat("{0}('{1}').CommitUpdate()", new object[]
			{
				base.GetType().Name,
				this.Filename
			});
			if (callback == null)
			{
				return;
			}
			Action action = delegate()
			{
				callback(SavedGameRequestStatus.Success, this._dummySavedGameMetadata);
			};
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x001B8278 File Offset: 0x001B6478
		public void Delete(ISavedGameMetadata metadata)
		{
			Debug.LogFormat("{0}('{1}').Delete()", new object[]
			{
				base.GetType().Name,
				this.Filename
			});
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x001B82AC File Offset: 0x001B64AC
		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			Debug.LogFormat("{0}('{1}').FetchAllSavedGames()", new object[]
			{
				base.GetType().Name,
				this.Filename
			});
			if (callback == null)
			{
				return;
			}
			Action action = delegate()
			{
				callback(SavedGameRequestStatus.Success, new List<ISavedGameMetadata>());
			};
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x001B8318 File Offset: 0x001B6518
		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Debug.LogFormat("{0}('{1}').OpenWithAutomaticConflictResolution()", new object[]
			{
				base.GetType().Name,
				this.Filename
			});
			if (callback == null)
			{
				return;
			}
			Action action = delegate()
			{
				callback(SavedGameRequestStatus.Success, this._dummySavedGameMetadata);
			};
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x001B838C File Offset: 0x001B658C
		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			bool flag = this._conflictCounter % 2 == 0;
			Debug.LogFormat("{0}('{1}', {2}).OpenWithManualConflictResolution()", new object[]
			{
				base.GetType().Name,
				this.Filename,
				flag
			});
			if (flag)
			{
				if (conflictCallback == null)
				{
					return;
				}
				byte[] data = Encoding.UTF8.GetBytes("{}");
				Action action = delegate()
				{
					conflictCallback(DummyConflictResolver.Instance, this._dummySavedGameMetadata, data, this._dummySavedGameMetadata, data);
				};
				CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
			}
			else
			{
				if (completedCallback == null)
				{
					return;
				}
				Action action2 = delegate()
				{
					completedCallback(SavedGameRequestStatus.Success, this._dummySavedGameMetadata);
				};
				CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action2));
			}
			this._conflictCounter++;
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x001B8490 File Offset: 0x001B6690
		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			Debug.LogFormat("{0}('{1}').ReadBinaryData()", new object[]
			{
				base.GetType().Name,
				this.Filename
			});
			if (completedCallback == null)
			{
				return;
			}
			byte[] binaryData = Encoding.UTF8.GetBytes("{}");
			Action action = delegate()
			{
				completedCallback(SavedGameRequestStatus.Success, binaryData);
			};
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x001B8514 File Offset: 0x001B6714
		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			Debug.LogFormat("{0}('{1}').ShowSelectSavedGameUI()", new object[]
			{
				base.GetType().Name,
				this.Filename
			});
			if (callback == null)
			{
				return;
			}
			Action action = delegate()
			{
				callback(SelectUIStatus.SavedGameSelected, this._dummySavedGameMetadata);
			};
			CoroutineRunner.Instance.StartCoroutine(this.WaitAndExecuteCoroutine(action));
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x001B8588 File Offset: 0x001B6788
		private IEnumerator WaitAndExecuteCoroutine(Action action)
		{
			yield return null;
			action();
			yield break;
		}

		// Token: 0x04003B3F RID: 15167
		private int _conflictCounter;

		// Token: 0x04003B40 RID: 15168
		private readonly string _filename;

		// Token: 0x04003B41 RID: 15169
		private readonly DummySavedGameMetadata _dummySavedGameMetadata;
	}
}
