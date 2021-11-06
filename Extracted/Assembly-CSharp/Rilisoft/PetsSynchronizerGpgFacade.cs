using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200083B RID: 2107
	internal struct PetsSynchronizerGpgFacade
	{
		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004C8F RID: 19599 RVA: 0x001B8E2C File Offset: 0x001B702C
		private static ISavedGameClient SavedGame
		{
			get
			{
				ISavedGameClient result;
				try
				{
					if (GpgFacade.Instance.SavedGame == null)
					{
						result = PetsSynchronizerGpgFacade._dummy;
					}
					else
					{
						result = GpgFacade.Instance.SavedGame;
					}
				}
				catch (NullReferenceException)
				{
					result = PetsSynchronizerGpgFacade._dummy;
				}
				return result;
			}
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x001B8E98 File Offset: 0x001B7098
		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(PlayerPets petsMemento)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[]
			{
				base.GetType().Name,
				petsMemento.Pets.Count
			});
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild && !Application.isEditor))
				{
					PetsSynchronizerGpgFacade.PushCallback @object = new PetsSynchronizerGpgFacade.PushCallback(petsMemento, taskCompletionSource);
					PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
				}
				task = taskCompletionSource.Task;
			}
			return task;
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x001B8FC0 File Offset: 0x001B71C0
		public Task<GoogleSavedGameRequestResult<PlayerPets>> Pull()
		{
			string text = base.GetType().Name + ".Pull()";
			Task<GoogleSavedGameRequestResult<PlayerPets>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<PlayerPets>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<PlayerPets>>();
				using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild && !Application.isEditor))
				{
					PetsSynchronizerGpgFacade.PullCallback @object = new PetsSynchronizerGpgFacade.PullCallback(taskCompletionSource);
					PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
				}
				task = taskCompletionSource.Task;
			}
			return task;
		}

		// Token: 0x04003B56 RID: 15190
		public const string Filename = "Pets.PlayerPets";

		// Token: 0x04003B57 RID: 15191
		private static readonly DummySavedGameClient _dummy = new DummySavedGameClient("Pets.PlayerPets");

		// Token: 0x0200083C RID: 2108
		private abstract class Callback
		{
			// Token: 0x17000C93 RID: 3219
			// (get) Token: 0x06004C93 RID: 19603
			protected abstract DataSource DefaultDataSource { get; }

			// Token: 0x06004C94 RID: 19604
			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			// Token: 0x06004C95 RID: 19605
			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			// Token: 0x06004C96 RID: 19606
			protected abstract void TrySetException(Exception ex);

			// Token: 0x06004C97 RID: 19607 RVA: 0x001B90CC File Offset: 0x001B72CC
			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
					base.GetType().Name,
					original.Description,
					unmerged.Description
				});
				using (new ScopeLogger(callee, false))
				{
					PlayerPets playerPets = PetsSynchronizerGpgFacade.Callback.Parse(originalData);
					PlayerPets playerPets2 = PetsSynchronizerGpgFacade.Callback.Parse(unmergedData);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Pets] original: {0}, unmerged: {1}", new object[]
						{
							playerPets,
							playerPets2
						});
					}
					int count = playerPets.Pets.Count;
					int count2 = playerPets2.Pets.Count;
					ISavedGameMetadata chosenMetadata = (count < count2) ? unmerged : original;
					resolver.ChooseMetadata(chosenMetadata);
					PlayerPets other = PlayerPets.Merge(playerPets, playerPets2);
					this._resolved = this.MergeWithResolved(other, true);
					PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
				}
			}

			// Token: 0x06004C98 RID: 19608 RVA: 0x001B9208 File Offset: 0x001B7408
			protected PlayerPets MergeWithResolved(PlayerPets other, bool forceConflicted)
			{
				PlayerPets playerPets = (this._resolved == null) ? other : PlayerPets.Merge(this._resolved, other);
				if (forceConflicted)
				{
					playerPets.Conflicted = true;
				}
				return playerPets;
			}

			// Token: 0x06004C99 RID: 19609 RVA: 0x001B9244 File Offset: 0x001B7444
			protected static PlayerPets Parse(byte[] data)
			{
				if (data == null || data.Length <= 0)
				{
					return new PlayerPets();
				}
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (string.IsNullOrEmpty(@string))
				{
					return new PlayerPets();
				}
				PlayerPets result;
				try
				{
					PlayerPets playerPets = JsonUtility.FromJson<PlayerPets>(@string);
					result = playerPets;
				}
				catch (ArgumentException exception)
				{
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[]
					{
						typeof(PlayerPets).Name,
						@string
					});
					Debug.LogException(exception);
					result = new PlayerPets();
				}
				return result;
			}

			// Token: 0x04003B58 RID: 15192
			protected PlayerPets _resolved;
		}

		// Token: 0x0200083D RID: 2109
		private sealed class PushCallback : PetsSynchronizerGpgFacade.Callback
		{
			// Token: 0x06004C9A RID: 19610 RVA: 0x001B92F4 File Offset: 0x001B74F4
			public PushCallback(PlayerPets petsMemento, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._petsMemento = petsMemento;
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>());
			}

			// Token: 0x17000C94 RID: 3220
			// (get) Token: 0x06004C9B RID: 19611 RVA: 0x001B9324 File Offset: 0x001B7524
			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004C9C RID: 19612 RVA: 0x001B9328 File Offset: 0x001B7528
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
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
						PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004C9D RID: 19613 RVA: 0x001B93F4 File Offset: 0x001B75F4
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004C9E RID: 19614 RVA: 0x001B9404 File Offset: 0x001B7604
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
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
						PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_1A6;
					case (SavedGameRequestStatus)4:
					{
						PlayerPets playerPets = base.MergeWithResolved(this._petsMemento, false);
						string text2 = (!playerPets.Conflicted) ? ((this._resolved == null) ? "none" : "trivial") : "resolved";
						string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[]
						{
							SystemInfo.deviceModel,
							text2
						});
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						string s = JsonUtility.ToJson(playerPets);
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						PetsSynchronizerGpgFacade.SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
						goto IL_1A6;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
					IL_1A6:;
				}
			}

			// Token: 0x06004C9F RID: 19615 RVA: 0x001B95E0 File Offset: 0x001B77E0
			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
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
						PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_EC;
					}
					GoogleSavedGameRequestResult<ISavedGameMetadata> result = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
					this._promise.TrySetResult(result);
					IL_EC:;
				}
			}

			// Token: 0x04003B59 RID: 15193
			private readonly PlayerPets _petsMemento;

			// Token: 0x04003B5A RID: 15194
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;
		}

		// Token: 0x0200083E RID: 2110
		private sealed class PullCallback : PetsSynchronizerGpgFacade.Callback
		{
			// Token: 0x06004CA0 RID: 19616 RVA: 0x001B9704 File Offset: 0x001B7904
			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<PlayerPets>> promise)
			{
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<PlayerPets>>());
			}

			// Token: 0x17000C95 RID: 3221
			// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x001B9720 File Offset: 0x001B7920
			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004CA2 RID: 19618 RVA: 0x001B9724 File Offset: 0x001B7924
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, false))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<PlayerPets>(SavedGameRequestStatus.AuthenticationError, new PlayerPets()));
					}
					else
					{
						PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004CA3 RID: 19619 RVA: 0x001B97F0 File Offset: 0x001B79F0
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004CA4 RID: 19620 RVA: 0x001B9800 File Offset: 0x001B7A00
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
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
						PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_10A;
					case (SavedGameRequestStatus)4:
						PetsSynchronizerGpgFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
						goto IL_10A;
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<PlayerPets>(requestStatus, new PlayerPets()));
					IL_10A:;
				}
			}

			// Token: 0x06004CA5 RID: 19621 RVA: 0x001B9940 File Offset: 0x001B7B40
			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleReadCompleted('{2}', {3})", new object[]
				{
					typeof(PetsSynchronizerGpgFacade).Name,
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
						PetsSynchronizerGpgFacade.SavedGame.OpenWithManualConflictResolution("Pets.PlayerPets", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_132;
					case (SavedGameRequestStatus)4:
					{
						PlayerPets playerPets = PetsSynchronizerGpgFacade.Callback.Parse(data);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Pets] Incoming: {0}", new object[]
							{
								playerPets
							});
						}
						PlayerPets value = base.MergeWithResolved(playerPets, false);
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<PlayerPets>(requestStatus, value));
						goto IL_132;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<PlayerPets>(requestStatus, new PlayerPets()));
					IL_132:;
				}
			}

			// Token: 0x04003B5B RID: 15195
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<PlayerPets>> _promise;
		}
	}
}
