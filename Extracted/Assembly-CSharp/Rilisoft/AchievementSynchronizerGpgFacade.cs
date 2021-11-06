using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200082A RID: 2090
	internal struct AchievementSynchronizerGpgFacade
	{
		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x001B5BB4 File Offset: 0x001B3DB4
		private static ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient result;
				try
				{
					if (GpgFacade.Instance.SavedGame == null)
					{
						result = AchievementSynchronizerGpgFacade._dummy;
					}
					else
					{
						result = GpgFacade.Instance.SavedGame;
					}
				}
				catch (NullReferenceException)
				{
					result = AchievementSynchronizerGpgFacade._dummy;
				}
				return result;
			}
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x001B5C20 File Offset: 0x001B3E20
		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(AchievementProgressSyncObject achievementMemento)
		{
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			string format = "{0}.Push({1})";
			object[] array = new object[2];
			array[0] = base.GetType().Name;
			array[1] = achievementMemento.ProgressData.Map((List<AchievementProgressData> l) => l.Count);
			string text = string.Format(invariantCulture, format, array);
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild && !Application.isEditor))
				{
					AchievementSynchronizerGpgFacade.PushCallback @object = new AchievementSynchronizerGpgFacade.PushCallback(achievementMemento, taskCompletionSource);
					AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
				}
				task = taskCompletionSource.Task;
			}
			return task;
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x001B5D64 File Offset: 0x001B3F64
		public Task<GoogleSavedGameRequestResult<AchievementProgressSyncObject>> Pull()
		{
			string text = base.GetType().Name + ".Pull()";
			Task<GoogleSavedGameRequestResult<AchievementProgressSyncObject>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<AchievementProgressSyncObject>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<AchievementProgressSyncObject>>();
				using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild && !Application.isEditor))
				{
					AchievementSynchronizerGpgFacade.PullCallback @object = new AchievementSynchronizerGpgFacade.PullCallback(taskCompletionSource);
					AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
				}
				task = taskCompletionSource.Task;
			}
			return task;
		}

		// Token: 0x04003B22 RID: 15138
		public const string Filename = "Achievements";

		// Token: 0x04003B23 RID: 15139
		private static readonly DummySavedGameClient _dummy = new DummySavedGameClient("Achievements");

		// Token: 0x0200082B RID: 2091
		private abstract class Callback
		{
			// Token: 0x17000C79 RID: 3193
			// (get) Token: 0x06004C03 RID: 19459 RVA: 0x001B5E78 File Offset: 0x001B4078
			protected DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004C04 RID: 19460
			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			// Token: 0x06004C05 RID: 19461
			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			// Token: 0x06004C06 RID: 19462
			protected abstract void TrySetException(Exception ex);

			// Token: 0x06004C07 RID: 19463 RVA: 0x001B5E7C File Offset: 0x001B407C
			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					original.Description,
					unmerged.Description
				});
				using (new ScopeLogger(callee, false))
				{
					AchievementProgressSyncObject achievementProgressSyncObject = AchievementSynchronizerGpgFacade.Callback.Parse(originalData);
					AchievementProgressSyncObject achievementProgressSyncObject2 = AchievementSynchronizerGpgFacade.Callback.Parse(unmergedData);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Achievements] original: {0}, unmerged: {1}", new object[]
						{
							achievementProgressSyncObject,
							achievementProgressSyncObject2
						});
					}
					int count = achievementProgressSyncObject.ProgressData.Count;
					int count2 = achievementProgressSyncObject2.ProgressData.Count;
					ISavedGameMetadata chosenMetadata = (count < count2) ? unmerged : original;
					resolver.ChooseMetadata(chosenMetadata);
					AchievementProgressSyncObject other = AchievementProgressData.Merge(achievementProgressSyncObject, achievementProgressSyncObject2);
					this._resolved = this.MergeWithResolved(other, true);
					AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
				}
			}

			// Token: 0x06004C08 RID: 19464 RVA: 0x001B5FB8 File Offset: 0x001B41B8
			protected AchievementProgressSyncObject MergeWithResolved(AchievementProgressSyncObject other, bool forceConflicted)
			{
				AchievementProgressSyncObject achievementProgressSyncObject = (this._resolved == null) ? other : AchievementProgressData.Merge(this._resolved, other);
				if (forceConflicted)
				{
					achievementProgressSyncObject.SetConflicted();
				}
				return achievementProgressSyncObject;
			}

			// Token: 0x06004C09 RID: 19465 RVA: 0x001B5FF0 File Offset: 0x001B41F0
			protected static AchievementProgressSyncObject Parse(byte[] data)
			{
				if (data == null || data.Length <= 0)
				{
					return new AchievementProgressSyncObject();
				}
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (string.IsNullOrEmpty(@string))
				{
					return new AchievementProgressSyncObject();
				}
				AchievementProgressSyncObject result;
				try
				{
					AchievementProgressSyncObject achievementProgressSyncObject = AchievementProgressSyncObject.FromJson(@string);
					result = achievementProgressSyncObject;
				}
				catch (ArgumentException exception)
				{
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[]
					{
						typeof(AchievementProgressSyncObject).Name,
						@string
					});
					Debug.LogException(exception);
					result = new AchievementProgressSyncObject();
				}
				return result;
			}

			// Token: 0x04003B25 RID: 15141
			protected AchievementProgressSyncObject _resolved;
		}

		// Token: 0x0200082C RID: 2092
		private sealed class PushCallback : AchievementSynchronizerGpgFacade.Callback
		{
			// Token: 0x06004C0A RID: 19466 RVA: 0x001B60A0 File Offset: 0x001B42A0
			public PushCallback(AchievementProgressSyncObject memento, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				if (memento == null)
				{
					throw new ArgumentNullException("memento");
				}
				this._memento = memento;
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>());
			}

			// Token: 0x06004C0B RID: 19467 RVA: 0x001B60D4 File Offset: 0x001B42D4
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, false))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
					else
					{
						AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", base.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004C0C RID: 19468 RVA: 0x001B61A0 File Offset: 0x001B43A0
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004C0D RID: 19469 RVA: 0x001B61B0 File Offset: 0x001B43B0
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, false))
				{
					switch (requestStatus + 3)
					{
					case (SavedGameRequestStatus)0:
						GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
						goto IL_1A6;
					case (SavedGameRequestStatus)2:
						AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", base.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_1A6;
					case (SavedGameRequestStatus)4:
					{
						AchievementProgressSyncObject achievementProgressSyncObject = base.MergeWithResolved(this._memento, false);
						string text2 = (!achievementProgressSyncObject.Conflicted) ? ((this._resolved == null) ? "none" : "trivial") : "resolved";
						string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[]
						{
							SystemInfo.deviceModel,
							text2
						});
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						string s = AchievementProgressSyncObject.ToJson(achievementProgressSyncObject);
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						AchievementSynchronizerGpgFacade.SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
						goto IL_1A6;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
					IL_1A6:;
				}
			}

			// Token: 0x06004C0E RID: 19470 RVA: 0x001B638C File Offset: 0x001B458C
			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, false))
				{
					switch (requestStatus + 3)
					{
					case (SavedGameRequestStatus)0:
						GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
						goto IL_EC;
					case (SavedGameRequestStatus)2:
						AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", base.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_EC;
					}
					GoogleSavedGameRequestResult<ISavedGameMetadata> result = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
					this._promise.TrySetResult(result);
					IL_EC:;
				}
			}

			// Token: 0x04003B26 RID: 15142
			private readonly AchievementProgressSyncObject _memento;

			// Token: 0x04003B27 RID: 15143
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;
		}

		// Token: 0x0200082D RID: 2093
		private sealed class PullCallback : AchievementSynchronizerGpgFacade.Callback
		{
			// Token: 0x06004C0F RID: 19471 RVA: 0x001B64B0 File Offset: 0x001B46B0
			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<AchievementProgressSyncObject>> promise)
			{
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<AchievementProgressSyncObject>>());
			}

			// Token: 0x06004C10 RID: 19472 RVA: 0x001B64CC File Offset: 0x001B46CC
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, false))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<AchievementProgressSyncObject>(SavedGameRequestStatus.AuthenticationError, new AchievementProgressSyncObject()));
					}
					else
					{
						AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004C11 RID: 19473 RVA: 0x001B6598 File Offset: 0x001B4798
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004C12 RID: 19474 RVA: 0x001B65A8 File Offset: 0x001B47A8
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, false))
				{
					switch (requestStatus + 3)
					{
					case (SavedGameRequestStatus)0:
						GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
						goto IL_10A;
					case (SavedGameRequestStatus)2:
						AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_10A;
					case (SavedGameRequestStatus)4:
						AchievementSynchronizerGpgFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
						goto IL_10A;
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<AchievementProgressSyncObject>(requestStatus, new AchievementProgressSyncObject()));
					IL_10A:;
				}
			}

			// Token: 0x06004C13 RID: 19475 RVA: 0x001B66E8 File Offset: 0x001B48E8
			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleReadCompleted('{2}', {3})", new object[]
				{
					typeof(AchievementSynchronizerGpgFacade).Name,
					base.GetType().Name,
					requestStatus,
					(data == null) ? 0 : data.Length
				});
				using (new ScopeLogger(callee, false))
				{
					switch (requestStatus + 3)
					{
					case (SavedGameRequestStatus)0:
						GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
						goto IL_132;
					case (SavedGameRequestStatus)2:
						AchievementSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Achievements", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_132;
					case (SavedGameRequestStatus)4:
					{
						AchievementProgressSyncObject achievementProgressSyncObject = AchievementSynchronizerGpgFacade.Callback.Parse(data);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Achievements] Incoming: {0}", new object[]
							{
								achievementProgressSyncObject
							});
						}
						AchievementProgressSyncObject value = base.MergeWithResolved(achievementProgressSyncObject, false);
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<AchievementProgressSyncObject>(requestStatus, value));
						goto IL_132;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<AchievementProgressSyncObject>(requestStatus, new AchievementProgressSyncObject()));
					IL_132:;
				}
			}

			// Token: 0x04003B28 RID: 15144
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<AchievementProgressSyncObject>> _promise;
		}
	}
}
