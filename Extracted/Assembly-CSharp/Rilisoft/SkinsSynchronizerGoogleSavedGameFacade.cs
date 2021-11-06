using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000841 RID: 2113
	public struct SkinsSynchronizerGoogleSavedGameFacade
	{
		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x001BA050 File Offset: 0x001B8250
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

		// Token: 0x06004CBD RID: 19645 RVA: 0x001BA09C File Offset: 0x001B829C
		public Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> Push(SkinsMemento skins)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "{0}.Push({1})", new object[]
			{
				base.GetType().Name,
				skins
			});
			Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>();
				if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
				{
					taskCompletionSource.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					task = taskCompletionSource.Task;
				}
				else
				{
					using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild))
					{
						SkinsSynchronizerGoogleSavedGameFacade.PushCallback @object = new SkinsSynchronizerGoogleSavedGameFacade.PushCallback(skins, taskCompletionSource);
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
					}
					task = taskCompletionSource.Task;
				}
			}
			return task;
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x001BA1C0 File Offset: 0x001B83C0
		public Task<GoogleSavedGameRequestResult<SkinsMemento>> Pull()
		{
			string text = base.GetType().Name + ".Pull()";
			Task<GoogleSavedGameRequestResult<SkinsMemento>> task;
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> taskCompletionSource = new TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>>();
				if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
				{
					taskCompletionSource.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					task = taskCompletionSource.Task;
				}
				else
				{
					using (new ScopeLogger(text, "OpenWithManualConflictResolution", Defs.IsDeveloperBuild))
					{
						SkinsSynchronizerGoogleSavedGameFacade.PullCallback @object = new SkinsSynchronizerGoogleSavedGameFacade.PullCallback(taskCompletionSource);
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(@object.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(@object.HandleOpenCompleted));
					}
					task = taskCompletionSource.Task;
				}
			}
			return task;
		}

		// Token: 0x04003B65 RID: 15205
		public const string Filename = "Skins";

		// Token: 0x04003B66 RID: 15206
		private const string SavedGameClientIsNullMessage = "SavedGameClient is null.";

		// Token: 0x02000842 RID: 2114
		private abstract class Callback
		{
			// Token: 0x17000C9E RID: 3230
			// (get) Token: 0x06004CC0 RID: 19648
			protected abstract DataSource DefaultDataSource { get; }

			// Token: 0x06004CC1 RID: 19649
			internal abstract void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata);

			// Token: 0x06004CC2 RID: 19650
			protected abstract void HandleAuthenticationCompleted(bool succeeded);

			// Token: 0x06004CC3 RID: 19651
			protected abstract void TrySetException(Exception ex);

			// Token: 0x06004CC4 RID: 19652 RVA: 0x001BA2E4 File Offset: 0x001B84E4
			internal void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenConflict('{2}', '{3}')", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
					base.GetType().Name,
					original.Description,
					unmerged.Description
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						SkinsMemento skinsMemento = SkinsSynchronizerGoogleSavedGameFacade.Callback.Parse(originalData);
						SkinsMemento skinsMemento2 = SkinsSynchronizerGoogleSavedGameFacade.Callback.Parse(unmergedData);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Skins] Original: {0}, unmerged: {1}", new object[]
							{
								skinsMemento,
								skinsMemento2
							});
						}
						HashSet<string> hashSet = new HashSet<string>(from s in skinsMemento.Skins
						select s.Id);
						HashSet<string> hashSet2 = new HashSet<string>(from s in skinsMemento2.Skins
						select s.Id);
						if (hashSet.IsSupersetOf(hashSet2))
						{
							resolver.ChooseMetadata(original);
							this._resolved = new SkinsMemento?(this.MergeWithResolved(skinsMemento, false));
						}
						else if (hashSet.IsProperSubsetOf(hashSet2))
						{
							resolver.ChooseMetadata(unmerged);
							this._resolved = new SkinsMemento?(this.MergeWithResolved(skinsMemento2, false));
						}
						else
						{
							ISavedGameMetadata chosenMetadata = (hashSet.Count < hashSet2.Count) ? unmerged : original;
							resolver.ChooseMetadata(chosenMetadata);
							SkinsMemento skins = SkinsMemento.Merge(skinsMemento, skinsMemento2);
							this._resolved = new SkinsMemento?(this.MergeWithResolved(skins, true));
						}
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(this.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004CC5 RID: 19653 RVA: 0x001BA4FC File Offset: 0x001B86FC
			protected SkinsMemento MergeWithResolved(SkinsMemento skins, bool forceConflicted)
			{
				SkinsMemento result = (this._resolved == null) ? skins : SkinsMemento.Merge(this._resolved.Value, skins);
				if (forceConflicted)
				{
					return new SkinsMemento(result.Skins, result.DeletedSkins, result.Cape, true);
				}
				return result;
			}

			// Token: 0x06004CC6 RID: 19654 RVA: 0x001BA554 File Offset: 0x001B8754
			protected static SkinsMemento Parse(byte[] data)
			{
				if (data == null || data.Length <= 0)
				{
					return default(SkinsMemento);
				}
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (string.IsNullOrEmpty(@string))
				{
					return default(SkinsMemento);
				}
				SkinsMemento result;
				try
				{
					SkinsMemento skinsMemento = JsonUtility.FromJson<SkinsMemento>(@string);
					result = skinsMemento;
				}
				catch (ArgumentException exception)
				{
					Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[]
					{
						typeof(SkinsMemento).Name,
						@string
					});
					Debug.LogException(exception);
					result = default(SkinsMemento);
				}
				return result;
			}

			// Token: 0x04003B67 RID: 15207
			protected SkinsMemento? _resolved;
		}

		// Token: 0x02000843 RID: 2115
		private sealed class PushCallback : SkinsSynchronizerGoogleSavedGameFacade.Callback
		{
			// Token: 0x06004CC9 RID: 19657 RVA: 0x001BA630 File Offset: 0x001B8830
			public PushCallback(SkinsMemento skins, TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> promise)
			{
				this._skins = skins;
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>>());
			}

			// Token: 0x17000C9F RID: 3231
			// (get) Token: 0x06004CCA RID: 19658 RVA: 0x001BA660 File Offset: 0x001B8860
			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004CCB RID: 19659 RVA: 0x001BA664 File Offset: 0x001B8864
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(SavedGameRequestStatus.AuthenticationError, null));
					}
					else if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004CCC RID: 19660 RVA: 0x001BA754 File Offset: 0x001B8954
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004CCD RID: 19661 RVA: 0x001BA764 File Offset: 0x001B8964
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						switch (requestStatus + 3)
						{
						case (SavedGameRequestStatus)0:
							GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
							goto IL_1D4;
						case (SavedGameRequestStatus)2:
							SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
							goto IL_1D4;
						case (SavedGameRequestStatus)4:
						{
							SkinsMemento skinsMemento = base.MergeWithResolved(this._skins, false);
							string text2 = (!skinsMemento.Conflicted) ? ((this._resolved == null) ? "none" : "trivial") : "resolved";
							string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", new object[]
							{
								SystemInfo.deviceModel,
								text2
							});
							SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
							string s = JsonUtility.ToJson(skinsMemento);
							byte[] bytes = Encoding.UTF8.GetBytes(s);
							SkinsSynchronizerGoogleSavedGameFacade.SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleCommitCompleted));
							goto IL_1D4;
						}
						}
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata));
						IL_1D4:;
					}
				}
			}

			// Token: 0x06004CCE RID: 19662 RVA: 0x001BA970 File Offset: 0x001B8B70
			private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleCommitCompleted('{2}', '{3}')", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
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
						goto IL_10F;
					case (SavedGameRequestStatus)2:
						if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
						{
							this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
							return;
						}
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", this.DefaultDataSource, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_10F;
					}
					GoogleSavedGameRequestResult<ISavedGameMetadata> result = new GoogleSavedGameRequestResult<ISavedGameMetadata>(requestStatus, metadata);
					this._promise.TrySetResult(result);
					IL_10F:;
				}
			}

			// Token: 0x04003B6A RID: 15210
			private readonly SkinsMemento _skins;

			// Token: 0x04003B6B RID: 15211
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<ISavedGameMetadata>> _promise;
		}

		// Token: 0x02000844 RID: 2116
		private sealed class PullCallback : SkinsSynchronizerGoogleSavedGameFacade.Callback
		{
			// Token: 0x06004CCF RID: 19663 RVA: 0x001BAAB8 File Offset: 0x001B8CB8
			public PullCallback(TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> promise)
			{
				this._promise = (promise ?? new TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>>());
			}

			// Token: 0x17000CA0 RID: 3232
			// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x001BAAD4 File Offset: 0x001B8CD4
			protected override DataSource DefaultDataSource
			{
				get
				{
					return DataSource.ReadNetworkOnly;
				}
			}

			// Token: 0x06004CD1 RID: 19665 RVA: 0x001BAAD8 File Offset: 0x001B8CD8
			protected override void HandleAuthenticationCompleted(bool succeeded)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleAuthenticationCompleted({2})", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
					base.GetType().Name,
					succeeded
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (!succeeded)
					{
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(SavedGameRequestStatus.AuthenticationError, default(SkinsMemento)));
					}
					else if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
					}
				}
			}

			// Token: 0x06004CD2 RID: 19666 RVA: 0x001BABCC File Offset: 0x001B8DCC
			protected override void TrySetException(Exception ex)
			{
				this._promise.TrySetException(ex);
			}

			// Token: 0x06004CD3 RID: 19667 RVA: 0x001BABDC File Offset: 0x001B8DDC
			internal override void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
			{
				string text = (metadata == null) ? string.Empty : metadata.Description;
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleOpenCompleted('{2}', '{3}')", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
					base.GetType().Name,
					requestStatus,
					text
				});
				using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
				{
					if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
					{
						this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
					}
					else
					{
						switch (requestStatus + 3)
						{
						case (SavedGameRequestStatus)0:
							GpgFacade.Instance.Authenticate(new Action<bool>(this.HandleAuthenticationCompleted), true);
							goto IL_132;
						case (SavedGameRequestStatus)2:
							SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
							goto IL_132;
						case (SavedGameRequestStatus)4:
							SkinsSynchronizerGoogleSavedGameFacade.SavedGame.ReadBinaryData(metadata, new Action<SavedGameRequestStatus, byte[]>(this.HandleReadCompleted));
							goto IL_132;
						}
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(requestStatus, default(SkinsMemento)));
						IL_132:;
					}
				}
			}

			// Token: 0x06004CD4 RID: 19668 RVA: 0x001BAD44 File Offset: 0x001B8F44
			private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
			{
				string callee = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.HandleReadCompleted('{2}', {3})", new object[]
				{
					typeof(SkinsSynchronizerGoogleSavedGameFacade).Name,
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
						goto IL_15F;
					case (SavedGameRequestStatus)2:
						if (SkinsSynchronizerGoogleSavedGameFacade.SavedGame == null)
						{
							this.TrySetException(new InvalidOperationException("SavedGameClient is null."));
							return;
						}
						SkinsSynchronizerGoogleSavedGameFacade.SavedGame.OpenWithManualConflictResolution("Skins", DataSource.ReadNetworkOnly, true, new ConflictCallback(base.HandleOpenConflict), new Action<SavedGameRequestStatus, ISavedGameMetadata>(this.HandleOpenCompleted));
						goto IL_15F;
					case (SavedGameRequestStatus)4:
					{
						SkinsMemento skinsMemento = SkinsSynchronizerGoogleSavedGameFacade.Callback.Parse(data);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Skins] Incoming: {0}", new object[]
							{
								skinsMemento
							});
						}
						SkinsMemento value = base.MergeWithResolved(skinsMemento, false);
						this._promise.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(requestStatus, value));
						goto IL_15F;
					}
					}
					this._promise.TrySetResult(new GoogleSavedGameRequestResult<SkinsMemento>(requestStatus, default(SkinsMemento)));
					IL_15F:;
				}
			}

			// Token: 0x04003B6C RID: 15212
			private readonly TaskCompletionSource<GoogleSavedGameRequestResult<SkinsMemento>> _promise;
		}
	}
}
