using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000848 RID: 2120
	public struct TrophiesSynchronizerGoogleSavedGameFacade
	{
		// Token: 0x06004CFF RID: 19711 RVA: 0x001BBF88 File Offset: 0x001BA188
		public Task<GoogleSavedGameRequestResult<TrophiesMemento>> Pull()
		{
			string text = base.GetType().Name + ".Pull()";
			Task<GoogleSavedGameRequestResult<TrophiesMemento>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>>();
				TrophiesSynchronizerGoogleSavedGameFacade.PullCallback @object = new TrophiesSynchronizerGoogleSavedGameFacade.PullCallback(taskCompletionSource);
				if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
				{
					taskCompletionSource.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					task = taskCompletionSource.Task;
				}
				else
				{
					using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild))
					{
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
					}
					task = taskCompletionSource.Task;
				}
			}
			return task;
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x001BC0A0 File Offset: 0x001BA2A0
		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(TrophiesMemento trophies)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[]
			{
				base.GetType().Name,
				trophies
			});
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				TrophiesSynchronizerGoogleSavedGameFacade.PushCallback @object = new TrophiesSynchronizerGoogleSavedGameFacade.PushCallback(trophies, taskCompletionSource);
				if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
				{
					taskCompletionSource.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					task = taskCompletionSource.Task;
				}
				else
				{
					using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild))
					{
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
					}
					task = taskCompletionSource.Task;
				}
			}
			return task;
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x001BC1C0 File Offset: 0x001BA3C0
		private static ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient result;
				try
				{
					result = GpgFacade.Instance.SavedGame;
				}
				catch (NullReferenceException)
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x04003B78 RID: 15224
		public const string Filename = "Trophies";

		// Token: 0x04003B79 RID: 15225
		private const string SavedGameClientIsNullMessage = "SavedGameClient is null.";

		// Token: 0x02000849 RID: 2121
		private abstract class Callback
		{
			// Token: 0x06004D03 RID: 19715
			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			// Token: 0x06004D04 RID: 19716
			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			// Token: 0x06004D05 RID: 19717
			protected abstract void TrySetException(Exception ex);

			// Token: 0x06004D06 RID: 19718 RVA: 0x001BC214 File Offset: 0x001BA414
			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleOpenConflict('{1}', '{2}')", new object[]
				{
					base.GetType().Name,
					original.Description,
					unmerged.Description
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						TrophiesMemento trophiesMemento = TrophiesSynchronizerGoogleSavedGameFacade.Callback.ParseTrophies(originalData);
						TrophiesMemento trophiesMemento2 = TrophiesSynchronizerGoogleSavedGameFacade.Callback.ParseTrophies(unmergedData);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Trophies] Original: {0}, unmerged: {1}", new object[]
							{
								trophiesMemento,
								trophiesMemento2
							});
						}
						if (trophiesMemento.TrophiesNegative >= trophiesMemento2.TrophiesNegative && trophiesMemento.TrophiesPositive >= trophiesMemento2.TrophiesPositive)
						{
							resolver.ChooseMetadata(original);
							this._resolvedTrophies = new TrophiesMemento?(this.MergeWithResolved(trophiesMemento, false));
						}
						else if (trophiesMemento.TrophiesNegative <= trophiesMemento2.TrophiesNegative && trophiesMemento.TrophiesPositive <= trophiesMemento2.TrophiesPositive)
						{
							resolver.ChooseMetadata(unmerged);
							this._resolvedTrophies = new TrophiesMemento?(this.MergeWithResolved(trophiesMemento2, false));
						}
						else
						{
							ISavedGameMetadata chosenMetadata = (trophiesMemento.Trophies < trophiesMemento2.Trophies) ? unmerged : original;
							resolver.ChooseMetadata(chosenMetadata);
							TrophiesMemento trophies = TrophiesMemento.Merge(trophiesMemento, trophiesMemento2);
							this._resolvedTrophies = new TrophiesMemento?(this.MergeWithResolved(trophies, true));
						}
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004D07 RID: 19719 RVA: 0x001BC3E8 File Offset: 0x001BA5E8
			protected TrophiesMemento MergeWithResolved(TrophiesMemento trophies, bool forceConflicted)
			{
				TrophiesMemento result = (this._resolvedTrophies == null) ? trophies : TrophiesMemento.Merge(this._resolvedTrophies.Value, trophies);
				if (forceConflicted)
				{
					return new TrophiesMemento(result.TrophiesNegative, result.TrophiesPositive, result.Season, true);
				}
				return result;
			}

			// Token: 0x06004D08 RID: 19720 RVA: 0x001BC440 File Offset: 0x001BA640
			protected static TrophiesMemento ParseTrophies(byte[] data)
			{
				if (data != null && data.Length > 0)
				{
					string @string = Encoding.UTF8.GetString(data, 0, data.Length);
					if (string.IsNullOrEmpty(@string))
					{
						return default(TrophiesMemento);
					}
					try
					{
						return JsonUtility.FromJson<TrophiesMemento>(@string);
					}
					catch (ArgumentException exception)
					{
						Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[]
						{
							typeof(TrophiesMemento).Name,
							@string
						});
						Debug.LogException(exception);
						return default(TrophiesMemento);
					}
				}
				return default(TrophiesMemento);
			}

			// Token: 0x04003B7A RID: 15226
			protected TrophiesMemento? _resolvedTrophies;
		}

		// Token: 0x0200084A RID: 2122
		private sealed class PushCallback : TrophiesSynchronizerGoogleSavedGameFacade.Callback
		{
			// Token: 0x06004D09 RID: 19721 RVA: 0x001BC504 File Offset: 0x001BA704
			public PushCallback(TrophiesMemento trophies, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._trophies = trophies;
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>());
			}

			// Token: 0x06004D0A RID: 19722 RVA: 0x001BC534 File Offset: 0x001BA734
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleOpenCompleted('{1}', '{2}')", new object[]
				{
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						switch (requestStatus + 3)
						{
						case (SavedGameRequestStatus)0:
							GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
							goto IL_1BD;
						case (SavedGameRequestStatus)2:
							TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
							goto IL_1BD;
						case (SavedGameRequestStatus)4:
						{
							TrophiesMemento trophiesMemento = base.MergeWithResolved(this._trophies, false);
							string text2 = (!trophiesMemento.Conflicted) ? ((this._resolvedTrophies == null) ? "none" : "trivial") : "resolved";
							string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[]
							{
								SystemInfo.deviceModel,
								text2
							});
							SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
							string s = JsonUtility.ToJson(trophiesMemento);
							byte[] bytes = Encoding.UTF8.GetBytes(s);
							TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
							goto IL_1BD;
						}
						}
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
						IL_1BD:;
					}
				}
			}

			// Token: 0x06004D0B RID: 19723 RVA: 0x001BC728 File Offset: 0x001BA928
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleAuthenticationCompleted({1})", new object[]
				{
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
					else if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004D0C RID: 19724 RVA: 0x001BC800 File Offset: 0x001BAA00
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004D0D RID: 19725 RVA: 0x001BC810 File Offset: 0x001BAA10
			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleCommitCompleted('{1}', '{2}')", new object[]
				{
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					switch (requestStatus + 3)
					{
					case (SavedGameRequestStatus)0:
						GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
						goto IL_F8;
					case (SavedGameRequestStatus)2:
						if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
						{
							this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
							return;
						}
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_F8;
					}
					GoogleSavedGameRequestResult<ISavedGameMetadata> result = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
					this._promise.TrySetResult(result);
					IL_F8:;
				}
			}

			// Token: 0x04003B7B RID: 15227
			private readonly TrophiesMemento _trophies;

			// Token: 0x04003B7C RID: 15228
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;
		}

		// Token: 0x0200084B RID: 2123
		private sealed class PullCallback : TrophiesSynchronizerGoogleSavedGameFacade.Callback
		{
			// Token: 0x06004D0E RID: 19726 RVA: 0x001BC940 File Offset: 0x001BAB40
			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> promise)
			{
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>>());
			}

			// Token: 0x06004D0F RID: 19727 RVA: 0x001BC95C File Offset: 0x001BAB5C
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleOpenCompleted('{1}', '{2}')", new object[]
				{
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						switch (requestStatus + 3)
						{
						case (SavedGameRequestStatus)0:
							GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
							goto IL_120;
						case (SavedGameRequestStatus)2:
							TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
							goto IL_120;
						case (SavedGameRequestStatus)4:
							TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
							goto IL_120;
						}
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(requestStatus, default(TrophiesMemento)));
						IL_120:;
					}
				}
			}

			// Token: 0x06004D10 RID: 19728 RVA: 0x001BCAB4 File Offset: 0x001BACB4
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleAuthenticationCompleted({1})", new object[]
				{
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(SavedGameRequestStatus.AuthenticationError, default(TrophiesMemento)));
					}
					else if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004D11 RID: 19729 RVA: 0x001BCB94 File Offset: 0x001BAD94
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004D12 RID: 19730 RVA: 0x001BCBA4 File Offset: 0x001BADA4
			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandleReadCompleted('{1}', {2})", new object[]
				{
					base.GetType().Name,
					requestStatus,
					(data == null) ? 0 : data.Length
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					switch (requestStatus + 3)
					{
					case (SavedGameRequestStatus)0:
						GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
						goto IL_14D;
					case (SavedGameRequestStatus)2:
						if (TrophiesSynchronizerGoogleSavedGameFacade.SavedGame == null)
						{
							this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
							return;
						}
						TrophiesSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Trophies", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_14D;
					case (SavedGameRequestStatus)4:
					{
						TrophiesMemento trophiesMemento = TrophiesSynchronizerGoogleSavedGameFacade.Callback.ParseTrophies(data);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Trophies] Incoming: {0}", new object[]
							{
								trophiesMemento
							});
						}
						TrophiesMemento value = base.MergeWithResolved(trophiesMemento, false);
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(requestStatus, value));
						goto IL_14D;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<TrophiesMemento>(requestStatus, default(TrophiesMemento)));
					IL_14D:;
				}
			}

			// Token: 0x04003B7D RID: 15229
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<TrophiesMemento>> _promise;
		}
	}
}
