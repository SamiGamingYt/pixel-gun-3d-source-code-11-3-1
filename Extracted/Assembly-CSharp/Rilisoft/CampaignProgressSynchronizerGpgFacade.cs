using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000830 RID: 2096
	internal struct CampaignProgressSynchronizerGpgFacade
	{
		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x001B7260 File Offset: 0x001B5460
		private static ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient result;
				try
				{
					if (GpgFacade.Instance.SavedGame == null)
					{
						result = CampaignProgressSynchronizerGpgFacade._dummy;
					}
					else
					{
						result = GpgFacade.Instance.SavedGame;
					}
				}
				catch (NullReferenceException)
				{
					result = CampaignProgressSynchronizerGpgFacade._dummy;
				}
				return result;
			}
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x001B72CC File Offset: 0x001B54CC
		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(CampaignProgressMemento campaignProgress)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[]
			{
				base.GetType().Name,
				campaignProgress.Levels.Count
			});
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild && !Application.isEditor))
				{
					CampaignProgressSynchronizerGpgFacade.PushCallback @object = new CampaignProgressSynchronizerGpgFacade.PushCallback(campaignProgress, taskCompletionSource);
					CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
				}
				task = taskCompletionSource.Task;
			}
			return task;
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x001B73F4 File Offset: 0x001B55F4
		public Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> Pull()
		{
			string text = base.GetType().Name + ".Pull()";
			Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>>();
				using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild && !Application.isEditor))
				{
					CampaignProgressSynchronizerGpgFacade.PullCallback @object = new CampaignProgressSynchronizerGpgFacade.PullCallback(taskCompletionSource);
					CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
				}
				task = taskCompletionSource.Task;
			}
			return task;
		}

		// Token: 0x04003B34 RID: 15156
		public const string Filename = "CampaignProgress";

		// Token: 0x04003B35 RID: 15157
		private static readonly DummySavedGameClient _dummy = new DummySavedGameClient("CampaignProgress");

		// Token: 0x02000831 RID: 2097
		private abstract class Callback
		{
			// Token: 0x17000C7E RID: 3198
			// (get) Token: 0x06004C34 RID: 19508
			protected abstract DataSource DefaultDataSource { get; }

			// Token: 0x06004C35 RID: 19509
			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			// Token: 0x06004C36 RID: 19510
			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			// Token: 0x06004C37 RID: 19511
			protected abstract void TrySetException(Exception ex);

			// Token: 0x06004C38 RID: 19512 RVA: 0x001B7500 File Offset: 0x001B5700
			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
					base.GetType().Name,
					original.Description,
					unmerged.Description
				});
				using (new ScopeLogger(callee, false))
				{
					CampaignProgressMemento campaignProgressMemento = CampaignProgressSynchronizerGpgFacade.Callback.Parse(originalData);
					CampaignProgressMemento campaignProgressMemento2 = CampaignProgressSynchronizerGpgFacade.Callback.Parse(unmergedData);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[CampaignProgress] original: {0}, unmerged: {1}", new object[]
						{
							campaignProgressMemento,
							campaignProgressMemento2
						});
					}
					int num = campaignProgressMemento.Levels.Sum((LevelProgressMemento l) => l.CoinCount + l.GemCount);
					int num2 = campaignProgressMemento2.Levels.Sum((LevelProgressMemento l) => l.CoinCount + l.GemCount);
					ISavedGameMetadata chosenMetadata = (num < num2) ? unmerged : original;
					resolver.ChooseMetadata(chosenMetadata);
					CampaignProgressMemento other = CampaignProgressMemento.Merge(campaignProgressMemento, campaignProgressMemento2);
					this._resolved = new CampaignProgressMemento?(this.MergeWithResolved(other, true));
					CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
				}
			}

			// Token: 0x06004C39 RID: 19513 RVA: 0x001B7684 File Offset: 0x001B5884
			protected CampaignProgressMemento MergeWithResolved(CampaignProgressMemento other, bool forceConflicted)
			{
				CampaignProgressMemento result = (this._resolved == null) ? other : CampaignProgressMemento.Merge(this._resolved.Value, other);
				if (forceConflicted)
				{
					result.SetConflicted();
				}
				return result;
			}

			// Token: 0x06004C3A RID: 19514 RVA: 0x001B76C8 File Offset: 0x001B58C8
			protected static CampaignProgressMemento Parse(byte[] data)
			{
				if (data == null || data.Length <= 0)
				{
					return default(CampaignProgressMemento);
				}
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (string.IsNullOrEmpty(@string))
				{
					return default(CampaignProgressMemento);
				}
				CampaignProgressMemento result;
				try
				{
					CampaignProgressMemento campaignProgressMemento = JsonUtility.FromJson<CampaignProgressMemento>(@string);
					result = campaignProgressMemento;
				}
				catch (ArgumentException exception)
				{
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[]
					{
						typeof(CampaignProgressMemento).Name,
						@string
					});
					Debug.LogException(exception);
					result = default(CampaignProgressMemento);
				}
				return result;
			}

			// Token: 0x04003B36 RID: 15158
			protected CampaignProgressMemento? _resolved;
		}

		// Token: 0x02000832 RID: 2098
		private sealed class PushCallback : CampaignProgressSynchronizerGpgFacade.Callback
		{
			// Token: 0x06004C3D RID: 19517 RVA: 0x001B77AC File Offset: 0x001B59AC
			public PushCallback(CampaignProgressMemento campaignProgressMemento, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._campaignProgressMemento = campaignProgressMemento;
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>());
			}

			// Token: 0x17000C7F RID: 3199
			// (get) Token: 0x06004C3E RID: 19518 RVA: 0x001B77DC File Offset: 0x001B59DC
			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004C3F RID: 19519 RVA: 0x001B77E0 File Offset: 0x001B59E0
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
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
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004C40 RID: 19520 RVA: 0x001B78AC File Offset: 0x001B5AAC
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004C41 RID: 19521 RVA: 0x001B78BC File Offset: 0x001B5ABC
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
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
						goto IL_1B1;
					case (SavedGameRequestStatus)2:
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_1B1;
					case (SavedGameRequestStatus)4:
					{
						CampaignProgressMemento campaignProgressMemento = base.MergeWithResolved(this._campaignProgressMemento, false);
						string text2 = (!campaignProgressMemento.Conflicted) ? ((this._resolved == null) ? "none" : "trivial") : "resolved";
						string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[]
						{
							SystemInfo.deviceModel,
							text2
						});
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						string s = JsonUtility.ToJson(campaignProgressMemento);
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						CampaignProgressSynchronizerGpgFacade.SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
						goto IL_1B1;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
					IL_1B1:;
				}
			}

			// Token: 0x06004C42 RID: 19522 RVA: 0x001B7AA4 File Offset: 0x001B5CA4
			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
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
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_EC;
					}
					GoogleSavedGameRequestResult<ISavedGameMetadata> result = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
					this._promise.TrySetResult(result);
					IL_EC:;
				}
			}

			// Token: 0x04003B39 RID: 15161
			private readonly CampaignProgressMemento _campaignProgressMemento;

			// Token: 0x04003B3A RID: 15162
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;
		}

		// Token: 0x02000833 RID: 2099
		private sealed class PullCallback : CampaignProgressSynchronizerGpgFacade.Callback
		{
			// Token: 0x06004C43 RID: 19523 RVA: 0x001B7BC8 File Offset: 0x001B5DC8
			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> promise)
			{
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>>());
			}

			// Token: 0x17000C80 RID: 3200
			// (get) Token: 0x06004C44 RID: 19524 RVA: 0x001B7BE4 File Offset: 0x001B5DE4
			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004C45 RID: 19525 RVA: 0x001B7BE8 File Offset: 0x001B5DE8
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, false))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(SavedGameRequestStatus.AuthenticationError, default(CampaignProgressMemento)));
					}
					else
					{
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004C46 RID: 19526 RVA: 0x001B7CB8 File Offset: 0x001B5EB8
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004C47 RID: 19527 RVA: 0x001B7CC8 File Offset: 0x001B5EC8
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
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
						goto IL_10F;
					case (SavedGameRequestStatus)2:
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_10F;
					case (SavedGameRequestStatus)4:
						CampaignProgressSynchronizerGpgFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
						goto IL_10F;
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, default(CampaignProgressMemento)));
					IL_10F:;
				}
			}

			// Token: 0x06004C48 RID: 19528 RVA: 0x001B7E10 File Offset: 0x001B6010
			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleReadCompleted('{2}', {3})", new object[]
				{
					typeof(CampaignProgressSynchronizerGpgFacade).Name,
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
						goto IL_13C;
					case (SavedGameRequestStatus)2:
						CampaignProgressSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("CampaignProgress", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_13C;
					case (SavedGameRequestStatus)4:
					{
						CampaignProgressMemento campaignProgressMemento = CampaignProgressSynchronizerGpgFacade.Callback.Parse(data);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[CampaignProgress] Incoming: {0}", new object[]
							{
								campaignProgressMemento
							});
						}
						CampaignProgressMemento value = base.MergeWithResolved(campaignProgressMemento, false);
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, value));
						goto IL_13C;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<CampaignProgressMemento>(requestStatus, default(CampaignProgressMemento)));
					IL_13C:;
				}
			}

			// Token: 0x04003B3B RID: 15163
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<CampaignProgressMemento>> _promise;
		}
	}
}
