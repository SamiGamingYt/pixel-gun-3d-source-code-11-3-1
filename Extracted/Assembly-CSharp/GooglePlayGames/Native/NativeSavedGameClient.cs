using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x0200022E RID: 558
	internal class NativeSavedGameClient : ISavedGameClient
	{
		// Token: 0x0600118D RID: 4493 RVA: 0x0004B398 File Offset: 0x00049598
		internal NativeSavedGameClient(GooglePlayGames.Native.PInvoke.SnapshotManager manager)
		{
			this.mSnapshotManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.SnapshotManager>(manager);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004B3C0 File Offset: 0x000495C0
		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Misc.CheckNotNull<string>(filename);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(callback);
			callback = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(callback);
			if (!NativeSavedGameClient.IsValidFilename(filename))
			{
				Logger.e("Received invalid filename: " + filename);
				callback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			this.OpenWithManualConflictResolution(filename, source, false, delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				switch (resolutionStrategy)
				{
				case ConflictResolutionStrategy.UseLongestPlaytime:
					if (original.TotalTimePlayed >= unmerged.TotalTimePlayed)
					{
						resolver.ChooseMetadata(original);
					}
					else
					{
						resolver.ChooseMetadata(unmerged);
					}
					return;
				case ConflictResolutionStrategy.UseOriginal:
					resolver.ChooseMetadata(original);
					return;
				case ConflictResolutionStrategy.UseUnmerged:
					resolver.ChooseMetadata(unmerged);
					return;
				default:
					Logger.e("Unhandled strategy " + resolutionStrategy);
					callback(SavedGameRequestStatus.InternalError, null);
					return;
				}
			}, callback);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0004B44C File Offset: 0x0004964C
		private ConflictCallback ToOnGameThread(ConflictCallback conflictCallback)
		{
			return delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				Logger.d("Invoking conflict callback");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					conflictCallback(resolver, original, originalData, unmerged, unmergedData);
				});
			};
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0004B474 File Offset: 0x00049674
		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			Misc.CheckNotNull<string>(filename);
			Misc.CheckNotNull<ConflictCallback>(conflictCallback);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
			conflictCallback = this.ToOnGameThread(conflictCallback);
			completedCallback = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(completedCallback);
			if (!NativeSavedGameClient.IsValidFilename(filename))
			{
				Logger.e("Received invalid filename: " + filename);
				completedCallback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			this.InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0004B4E0 File Offset: 0x000496E0
		private void InternalManualOpen(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			this.mSnapshotManager.Open(filename, NativeSavedGameClient.AsDataSource(source), Types.SnapshotConflictPolicy.MANUAL, delegate(GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse response)
			{
				if (!response.RequestSucceeded())
				{
					completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				}
				else if (response.ResponseStatus() == CommonErrorStatus.SnapshotOpenStatus.VALID)
				{
					completedCallback(SavedGameRequestStatus.Success, response.Data());
				}
				else if (response.ResponseStatus() == CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					NativeSnapshotMetadata original = response.ConflictOriginal();
					NativeSnapshotMetadata unmerged = response.ConflictUnmerged();
					NativeSavedGameClient.NativeConflictResolver resolver = new NativeSavedGameClient.NativeConflictResolver(this.mSnapshotManager, response.ConflictId(), original, unmerged, completedCallback, delegate()
					{
						this.InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback);
					});
					if (!prefetchDataOnConflict)
					{
						conflictCallback(resolver, original, null, unmerged, null);
						return;
					}
					NativeSavedGameClient.Prefetcher @object = new NativeSavedGameClient.Prefetcher(delegate(byte[] originalData, byte[] unmergedData)
					{
						conflictCallback(resolver, original, originalData, unmerged, unmergedData);
					}, completedCallback);
					this.mSnapshotManager.Read(original, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(@object.OnOriginalDataRead));
					this.mSnapshotManager.Read(unmerged, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(@object.OnUnmergedDataRead));
				}
				else
				{
					Logger.e("Unhandled response status");
					completedCallback(SavedGameRequestStatus.InternalError, null);
				}
			});
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004B548 File Offset: 0x00049748
		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			Misc.CheckNotNull<ISavedGameMetadata>(metadata);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, byte[]>>(completedCallback);
			completedCallback = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, byte[]>(completedCallback);
			NativeSnapshotMetadata nativeSnapshotMetadata = metadata as NativeSnapshotMetadata;
			if (nativeSnapshotMetadata == null)
			{
				Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				completedCallback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!nativeSnapshotMetadata.IsOpen)
			{
				Logger.e("This method requires an open ISavedGameMetadata.");
				completedCallback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			this.mSnapshotManager.Read(nativeSnapshotMetadata, delegate(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse response)
			{
				if (!response.RequestSucceeded())
				{
					completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				}
				else
				{
					completedCallback(SavedGameRequestStatus.Success, response.Data());
				}
			});
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0004B5E8 File Offset: 0x000497E8
		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			Misc.CheckNotNull<string>(uiTitle);
			Misc.CheckNotNull<Action<SelectUIStatus, ISavedGameMetadata>>(callback);
			callback = NativeSavedGameClient.ToOnGameThread<SelectUIStatus, ISavedGameMetadata>(callback);
			if (maxDisplayedSavedGames <= 0U)
			{
				Logger.e("maxDisplayedSavedGames must be greater than 0");
				callback(SelectUIStatus.BadInputError, null);
				return;
			}
			this.mSnapshotManager.SnapshotSelectUI(showCreateSaveUI, showDeleteSaveUI, maxDisplayedSavedGames, uiTitle, delegate(GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse response)
			{
				callback(NativeSavedGameClient.AsUIStatus(response.RequestStatus()), (!response.RequestSucceeded()) ? null : response.Data());
			});
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0004B664 File Offset: 0x00049864
		public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			Misc.CheckNotNull<ISavedGameMetadata>(metadata);
			Misc.CheckNotNull<byte[]>(updatedBinaryData);
			Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(callback);
			callback = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(callback);
			NativeSnapshotMetadata nativeSnapshotMetadata = metadata as NativeSnapshotMetadata;
			if (nativeSnapshotMetadata == null)
			{
				Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
				callback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			if (!nativeSnapshotMetadata.IsOpen)
			{
				Logger.e("This method requires an open ISavedGameMetadata.");
				callback(SavedGameRequestStatus.BadInputError, null);
				return;
			}
			this.mSnapshotManager.Commit(nativeSnapshotMetadata, NativeSavedGameClient.AsMetadataChange(updateForMetadata), updatedBinaryData, delegate(GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response)
			{
				if (!response.RequestSucceeded())
				{
					callback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
				}
				else
				{
					callback(SavedGameRequestStatus.Success, response.Data());
				}
			});
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0004B714 File Offset: 0x00049914
		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			Misc.CheckNotNull<Action<SavedGameRequestStatus, List<ISavedGameMetadata>>>(callback);
			callback = NativeSavedGameClient.ToOnGameThread<SavedGameRequestStatus, List<ISavedGameMetadata>>(callback);
			this.mSnapshotManager.FetchAll(NativeSavedGameClient.AsDataSource(source), delegate(GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse response)
			{
				if (!response.RequestSucceeded())
				{
					callback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), new List<ISavedGameMetadata>());
				}
				else
				{
					callback(SavedGameRequestStatus.Success, response.Data().Cast<ISavedGameMetadata>().ToList<ISavedGameMetadata>());
				}
			});
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0004B768 File Offset: 0x00049968
		public void Delete(ISavedGameMetadata metadata)
		{
			Misc.CheckNotNull<ISavedGameMetadata>(metadata);
			this.mSnapshotManager.Delete((NativeSnapshotMetadata)metadata);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0004B784 File Offset: 0x00049984
		internal static bool IsValidFilename(string filename)
		{
			return filename != null && NativeSavedGameClient.ValidFilenameRegex.IsMatch(filename);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0004B79C File Offset: 0x0004999C
		private static Types.SnapshotConflictPolicy AsConflictPolicy(ConflictResolutionStrategy strategy)
		{
			switch (strategy)
			{
			case ConflictResolutionStrategy.UseLongestPlaytime:
				return Types.SnapshotConflictPolicy.LONGEST_PLAYTIME;
			case ConflictResolutionStrategy.UseOriginal:
				return Types.SnapshotConflictPolicy.LAST_KNOWN_GOOD;
			case ConflictResolutionStrategy.UseUnmerged:
				return Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
			default:
				throw new InvalidOperationException("Found unhandled strategy: " + strategy);
			}
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0004B7E0 File Offset: 0x000499E0
		private static SavedGameRequestStatus AsRequestStatus(CommonErrorStatus.SnapshotOpenStatus status)
		{
			switch (status + 5)
			{
			case (CommonErrorStatus.SnapshotOpenStatus)0:
				return SavedGameRequestStatus.TimeoutError;
			default:
				if (status != CommonErrorStatus.SnapshotOpenStatus.VALID)
				{
					Logger.e("Encountered unknown status: " + status);
					return SavedGameRequestStatus.InternalError;
				}
				return SavedGameRequestStatus.Success;
			case (CommonErrorStatus.SnapshotOpenStatus)2:
				return SavedGameRequestStatus.AuthenticationError;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004B830 File Offset: 0x00049A30
		private static Types.DataSource AsDataSource(DataSource source)
		{
			if (source == DataSource.ReadCacheOrNetwork)
			{
				return Types.DataSource.CACHE_OR_NETWORK;
			}
			if (source != DataSource.ReadNetworkOnly)
			{
				throw new InvalidOperationException("Found unhandled DataSource: " + source);
			}
			return Types.DataSource.NETWORK_ONLY;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0004B86C File Offset: 0x00049A6C
		private static SavedGameRequestStatus AsRequestStatus(CommonErrorStatus.ResponseStatus status)
		{
			switch (status + 5)
			{
			case (CommonErrorStatus.ResponseStatus)0:
				return SavedGameRequestStatus.TimeoutError;
			case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				Logger.e("User was not authorized (they were probably not logged in).");
				return SavedGameRequestStatus.AuthenticationError;
			case (CommonErrorStatus.ResponseStatus)3:
				return SavedGameRequestStatus.InternalError;
			case (CommonErrorStatus.ResponseStatus)4:
				Logger.e("User attempted to use the game without a valid license.");
				return SavedGameRequestStatus.AuthenticationError;
			case (CommonErrorStatus.ResponseStatus)6:
			case (CommonErrorStatus.ResponseStatus)7:
				return SavedGameRequestStatus.Success;
			}
			Logger.e("Unknown status: " + status);
			return SavedGameRequestStatus.InternalError;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004B8E0 File Offset: 0x00049AE0
		private static SelectUIStatus AsUIStatus(CommonErrorStatus.UIStatus uiStatus)
		{
			switch (uiStatus + 6)
			{
			case (CommonErrorStatus.UIStatus)0:
				return SelectUIStatus.UserClosedUI;
			case CommonErrorStatus.UIStatus.VALID:
				return SelectUIStatus.TimeoutError;
			case (CommonErrorStatus.UIStatus)3:
				return SelectUIStatus.AuthenticationError;
			case (CommonErrorStatus.UIStatus)4:
				return SelectUIStatus.InternalError;
			case (CommonErrorStatus.UIStatus)7:
				return SelectUIStatus.SavedGameSelected;
			}
			Logger.e("Encountered unknown UI Status: " + uiStatus);
			return SelectUIStatus.InternalError;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004B940 File Offset: 0x00049B40
		private static NativeSnapshotMetadataChange AsMetadataChange(SavedGameMetadataUpdate update)
		{
			NativeSnapshotMetadataChange.Builder builder = new NativeSnapshotMetadataChange.Builder();
			if (update.IsCoverImageUpdated)
			{
				builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
			}
			if (update.IsDescriptionUpdated)
			{
				builder.SetDescription(update.UpdatedDescription);
			}
			if (update.IsPlayedTimeUpdated)
			{
				builder.SetPlayedTime((ulong)update.UpdatedPlayedTime.Value.TotalMilliseconds);
			}
			return builder.Build();
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004B9B8 File Offset: 0x00049BB8
		private static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
		{
			return delegate(T1 val1, T2 val2)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2);
				});
			};
		}

		// Token: 0x04000BE2 RID: 3042
		private static readonly Regex ValidFilenameRegex = new Regex("\\A[a-zA-Z0-9-._~]{1,100}\\Z");

		// Token: 0x04000BE3 RID: 3043
		private readonly GooglePlayGames.Native.PInvoke.SnapshotManager mSnapshotManager;

		// Token: 0x0200022F RID: 559
		private class NativeConflictResolver : IConflictResolver
		{
			// Token: 0x060011A0 RID: 4512 RVA: 0x0004B9E0 File Offset: 0x00049BE0
			internal NativeConflictResolver(GooglePlayGames.Native.PInvoke.SnapshotManager manager, string conflictId, NativeSnapshotMetadata original, NativeSnapshotMetadata unmerged, Action<SavedGameRequestStatus, ISavedGameMetadata> completeCallback, Action retryOpen)
			{
				this.mManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.SnapshotManager>(manager);
				this.mConflictId = Misc.CheckNotNull<string>(conflictId);
				this.mOriginal = Misc.CheckNotNull<NativeSnapshotMetadata>(original);
				this.mUnmerged = Misc.CheckNotNull<NativeSnapshotMetadata>(unmerged);
				this.mCompleteCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completeCallback);
				this.mRetryFileOpen = Misc.CheckNotNull<Action>(retryOpen);
			}

			// Token: 0x060011A1 RID: 4513 RVA: 0x0004BA40 File Offset: 0x00049C40
			public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
			{
				NativeSnapshotMetadata nativeSnapshotMetadata = chosenMetadata as NativeSnapshotMetadata;
				if (nativeSnapshotMetadata != this.mOriginal && nativeSnapshotMetadata != this.mUnmerged)
				{
					Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
					this.mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
					return;
				}
				this.mManager.Resolve(nativeSnapshotMetadata, new NativeSnapshotMetadataChange.Builder().Build(), this.mConflictId, delegate(GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response)
				{
					if (!response.RequestSucceeded())
					{
						this.mCompleteCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
						return;
					}
					this.mRetryFileOpen();
				});
			}

			// Token: 0x04000BE4 RID: 3044
			private readonly GooglePlayGames.Native.PInvoke.SnapshotManager mManager;

			// Token: 0x04000BE5 RID: 3045
			private readonly string mConflictId;

			// Token: 0x04000BE6 RID: 3046
			private readonly NativeSnapshotMetadata mOriginal;

			// Token: 0x04000BE7 RID: 3047
			private readonly NativeSnapshotMetadata mUnmerged;

			// Token: 0x04000BE8 RID: 3048
			private readonly Action<SavedGameRequestStatus, ISavedGameMetadata> mCompleteCallback;

			// Token: 0x04000BE9 RID: 3049
			private readonly Action mRetryFileOpen;
		}

		// Token: 0x02000230 RID: 560
		private class Prefetcher
		{
			// Token: 0x060011A3 RID: 4515 RVA: 0x0004BAEC File Offset: 0x00049CEC
			internal Prefetcher(Action<byte[], byte[]> dataFetchedCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
			{
				this.mDataFetchedCallback = Misc.CheckNotNull<Action<byte[], byte[]>>(dataFetchedCallback);
				this.completedCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
			}

			// Token: 0x060011A4 RID: 4516 RVA: 0x0004BB18 File Offset: 0x00049D18
			internal void OnOriginalDataRead(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
			{
				object obj = this.mLock;
				lock (obj)
				{
					if (!readResponse.RequestSucceeded())
					{
						Logger.e("Encountered error while prefetching original data.");
						this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
						this.completedCallback = delegate(SavedGameRequestStatus A_0, ISavedGameMetadata A_1)
						{
						};
					}
					else
					{
						Logger.d("Successfully fetched original data");
						this.mOriginalDataFetched = true;
						this.mOriginalData = readResponse.Data();
						this.MaybeProceed();
					}
				}
			}

			// Token: 0x060011A5 RID: 4517 RVA: 0x0004BBD4 File Offset: 0x00049DD4
			internal void OnUnmergedDataRead(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
			{
				object obj = this.mLock;
				lock (obj)
				{
					if (!readResponse.RequestSucceeded())
					{
						Logger.e("Encountered error while prefetching unmerged data.");
						this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
						this.completedCallback = delegate(SavedGameRequestStatus A_0, ISavedGameMetadata A_1)
						{
						};
					}
					else
					{
						Logger.d("Successfully fetched unmerged data");
						this.mUnmergedDataFetched = true;
						this.mUnmergedData = readResponse.Data();
						this.MaybeProceed();
					}
				}
			}

			// Token: 0x060011A6 RID: 4518 RVA: 0x0004BC90 File Offset: 0x00049E90
			private void MaybeProceed()
			{
				if (this.mOriginalDataFetched && this.mUnmergedDataFetched)
				{
					Logger.d("Fetched data for original and unmerged, proceeding");
					this.mDataFetchedCallback(this.mOriginalData, this.mUnmergedData);
				}
				else
				{
					Logger.d(string.Concat(new object[]
					{
						"Not all data fetched - original:",
						this.mOriginalDataFetched,
						" unmerged:",
						this.mUnmergedDataFetched
					}));
				}
			}

			// Token: 0x04000BEA RID: 3050
			private readonly object mLock = new object();

			// Token: 0x04000BEB RID: 3051
			private bool mOriginalDataFetched;

			// Token: 0x04000BEC RID: 3052
			private byte[] mOriginalData;

			// Token: 0x04000BED RID: 3053
			private bool mUnmergedDataFetched;

			// Token: 0x04000BEE RID: 3054
			private byte[] mUnmergedData;

			// Token: 0x04000BEF RID: 3055
			private Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;

			// Token: 0x04000BF0 RID: 3056
			private readonly Action<byte[], byte[]> mDataFetchedCallback;
		}
	}
}
