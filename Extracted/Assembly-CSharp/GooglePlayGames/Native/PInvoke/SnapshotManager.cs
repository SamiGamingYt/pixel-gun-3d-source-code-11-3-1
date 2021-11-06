using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000277 RID: 631
	internal class SnapshotManager
	{
		// Token: 0x06001439 RID: 5177 RVA: 0x00051234 File Offset: 0x0004F434
		internal SnapshotManager(GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00051248 File Offset: 0x0004F448
		internal void FetchAll(Types.DataSource source, Action<SnapshotManager.FetchAllResponse> callback)
		{
			SnapshotManager.SnapshotManager_FetchAll(this.mServices.AsHandle(), source, new SnapshotManager.FetchAllCallback(SnapshotManager.InternalFetchAllCallback), Callbacks.ToIntPtr<SnapshotManager.FetchAllResponse>(callback, new Func<IntPtr, SnapshotManager.FetchAllResponse>(SnapshotManager.FetchAllResponse.FromPointer)));
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00051284 File Offset: 0x0004F484
		[MonoPInvokeCallback(typeof(SnapshotManager.FetchAllCallback))]
		internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("SnapshotManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x00051294 File Offset: 0x0004F494
		internal void SnapshotSelectUI(bool allowCreate, bool allowDelete, uint maxSnapshots, string uiTitle, Action<SnapshotManager.SnapshotSelectUIResponse> callback)
		{
			SnapshotManager.SnapshotManager_ShowSelectUIOperation(this.mServices.AsHandle(), allowCreate, allowDelete, maxSnapshots, uiTitle, new SnapshotManager.SnapshotSelectUICallback(SnapshotManager.InternalSnapshotSelectUICallback), Callbacks.ToIntPtr<SnapshotManager.SnapshotSelectUIResponse>(callback, new Func<IntPtr, SnapshotManager.SnapshotSelectUIResponse>(SnapshotManager.SnapshotSelectUIResponse.FromPointer)));
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x000512D8 File Offset: 0x0004F4D8
		[MonoPInvokeCallback(typeof(SnapshotManager.SnapshotSelectUICallback))]
		internal static void InternalSnapshotSelectUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("SnapshotManager#SnapshotSelectUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x000512E8 File Offset: 0x0004F4E8
		internal void Open(string fileName, Types.DataSource source, Types.SnapshotConflictPolicy conflictPolicy, Action<SnapshotManager.OpenResponse> callback)
		{
			Misc.CheckNotNull<string>(fileName);
			Misc.CheckNotNull<Action<SnapshotManager.OpenResponse>>(callback);
			SnapshotManager.SnapshotManager_Open(this.mServices.AsHandle(), source, fileName, conflictPolicy, new SnapshotManager.OpenCallback(SnapshotManager.InternalOpenCallback), Callbacks.ToIntPtr<SnapshotManager.OpenResponse>(callback, new Func<IntPtr, SnapshotManager.OpenResponse>(SnapshotManager.OpenResponse.FromPointer)));
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00051338 File Offset: 0x0004F538
		[MonoPInvokeCallback(typeof(SnapshotManager.OpenCallback))]
		internal static void InternalOpenCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("SnapshotManager#OpenCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00051348 File Offset: 0x0004F548
		internal void Commit(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, byte[] updatedData, Action<SnapshotManager.CommitResponse> callback)
		{
			Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
			Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
			SnapshotManager.SnapshotManager_Commit(this.mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), updatedData, new UIntPtr((ulong)((long)updatedData.Length)), new SnapshotManager.CommitCallback(SnapshotManager.InternalCommitCallback), Callbacks.ToIntPtr<SnapshotManager.CommitResponse>(callback, new Func<IntPtr, SnapshotManager.CommitResponse>(SnapshotManager.CommitResponse.FromPointer)));
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x000513A8 File Offset: 0x0004F5A8
		internal void Resolve(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, string conflictId, Action<SnapshotManager.CommitResponse> callback)
		{
			Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
			Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
			Misc.CheckNotNull<string>(conflictId);
			SnapshotManager.SnapshotManager_ResolveConflict(this.mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), conflictId, new SnapshotManager.CommitCallback(SnapshotManager.InternalCommitCallback), Callbacks.ToIntPtr<SnapshotManager.CommitResponse>(callback, new Func<IntPtr, SnapshotManager.CommitResponse>(SnapshotManager.CommitResponse.FromPointer)));
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00051408 File Offset: 0x0004F608
		[MonoPInvokeCallback(typeof(SnapshotManager.CommitCallback))]
		internal static void InternalCommitCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("SnapshotManager#CommitCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00051418 File Offset: 0x0004F618
		internal void Delete(NativeSnapshotMetadata metadata)
		{
			Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
			SnapshotManager.SnapshotManager_Delete(this.mServices.AsHandle(), metadata.AsPointer());
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00051438 File Offset: 0x0004F638
		internal void Read(NativeSnapshotMetadata metadata, Action<SnapshotManager.ReadResponse> callback)
		{
			Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
			Misc.CheckNotNull<Action<SnapshotManager.ReadResponse>>(callback);
			SnapshotManager.SnapshotManager_Read(this.mServices.AsHandle(), metadata.AsPointer(), new SnapshotManager.ReadCallback(SnapshotManager.InternalReadCallback), Callbacks.ToIntPtr<SnapshotManager.ReadResponse>(callback, new Func<IntPtr, SnapshotManager.ReadResponse>(SnapshotManager.ReadResponse.FromPointer)));
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00051488 File Offset: 0x0004F688
		[MonoPInvokeCallback(typeof(SnapshotManager.ReadCallback))]
		internal static void InternalReadCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("SnapshotManager#ReadCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x04000C10 RID: 3088
		private readonly GameServices mServices;

		// Token: 0x02000278 RID: 632
		internal class OpenResponse : BaseReferenceHolder
		{
			// Token: 0x06001446 RID: 5190 RVA: 0x00051498 File Offset: 0x0004F698
			internal OpenResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001447 RID: 5191 RVA: 0x000514A4 File Offset: 0x0004F6A4
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.SnapshotOpenStatus)0;
			}

			// Token: 0x06001448 RID: 5192 RVA: 0x000514B0 File Offset: 0x0004F6B0
			internal CommonErrorStatus.SnapshotOpenStatus ResponseStatus()
			{
				return SnapshotManager.SnapshotManager_OpenResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001449 RID: 5193 RVA: 0x000514C0 File Offset: 0x0004F6C0
			internal string ConflictId()
			{
				if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					throw new InvalidOperationException("OpenResponse did not have a conflict");
				}
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotManager.SnapshotManager_OpenResponse_GetConflictId(base.SelfPtr(), out_string, out_size));
			}

			// Token: 0x0600144A RID: 5194 RVA: 0x000514F8 File Offset: 0x0004F6F8
			internal NativeSnapshotMetadata Data()
			{
				if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID)
				{
					throw new InvalidOperationException("OpenResponse had a conflict");
				}
				return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_OpenResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x0600144B RID: 5195 RVA: 0x00051524 File Offset: 0x0004F724
			internal NativeSnapshotMetadata ConflictOriginal()
			{
				if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					throw new InvalidOperationException("OpenResponse did not have a conflict");
				}
				return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_OpenResponse_GetConflictOriginal(base.SelfPtr()));
			}

			// Token: 0x0600144C RID: 5196 RVA: 0x00051550 File Offset: 0x0004F750
			internal NativeSnapshotMetadata ConflictUnmerged()
			{
				if (this.ResponseStatus() != CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
				{
					throw new InvalidOperationException("OpenResponse did not have a conflict");
				}
				return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_OpenResponse_GetConflictUnmerged(base.SelfPtr()));
			}

			// Token: 0x0600144D RID: 5197 RVA: 0x0005157C File Offset: 0x0004F77C
			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotManager.SnapshotManager_OpenResponse_Dispose(selfPointer);
			}

			// Token: 0x0600144E RID: 5198 RVA: 0x00051584 File Offset: 0x0004F784
			internal static SnapshotManager.OpenResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new SnapshotManager.OpenResponse(pointer);
			}
		}

		// Token: 0x02000279 RID: 633
		internal class FetchAllResponse : BaseReferenceHolder
		{
			// Token: 0x06001450 RID: 5200 RVA: 0x000515B4 File Offset: 0x0004F7B4
			internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001451 RID: 5201 RVA: 0x000515C0 File Offset: 0x0004F7C0
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return SnapshotManager.SnapshotManager_FetchAllResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001452 RID: 5202 RVA: 0x000515D0 File Offset: 0x0004F7D0
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x06001453 RID: 5203 RVA: 0x000515DC File Offset: 0x0004F7DC
			internal IEnumerable<NativeSnapshotMetadata> Data()
			{
				return PInvokeUtilities.ToEnumerable<NativeSnapshotMetadata>(SnapshotManager.SnapshotManager_FetchAllResponse_GetData_Length(base.SelfPtr()), (UIntPtr index) => new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_FetchAllResponse_GetData_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x06001454 RID: 5204 RVA: 0x000515FC File Offset: 0x0004F7FC
			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotManager.SnapshotManager_FetchAllResponse_Dispose(selfPointer);
			}

			// Token: 0x06001455 RID: 5205 RVA: 0x00051604 File Offset: 0x0004F804
			internal static SnapshotManager.FetchAllResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new SnapshotManager.FetchAllResponse(pointer);
			}
		}

		// Token: 0x0200027A RID: 634
		internal class CommitResponse : BaseReferenceHolder
		{
			// Token: 0x06001457 RID: 5207 RVA: 0x00051638 File Offset: 0x0004F838
			internal CommitResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001458 RID: 5208 RVA: 0x00051644 File Offset: 0x0004F844
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return SnapshotManager.SnapshotManager_CommitResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001459 RID: 5209 RVA: 0x00051654 File Offset: 0x0004F854
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x0600145A RID: 5210 RVA: 0x00051660 File Offset: 0x0004F860
			internal NativeSnapshotMetadata Data()
			{
				if (!this.RequestSucceeded())
				{
					throw new InvalidOperationException("Request did not succeed");
				}
				return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_CommitResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x0600145B RID: 5211 RVA: 0x00051694 File Offset: 0x0004F894
			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotManager.SnapshotManager_CommitResponse_Dispose(selfPointer);
			}

			// Token: 0x0600145C RID: 5212 RVA: 0x0005169C File Offset: 0x0004F89C
			internal static SnapshotManager.CommitResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new SnapshotManager.CommitResponse(pointer);
			}
		}

		// Token: 0x0200027B RID: 635
		internal class ReadResponse : BaseReferenceHolder
		{
			// Token: 0x0600145D RID: 5213 RVA: 0x000516BC File Offset: 0x0004F8BC
			internal ReadResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x0600145E RID: 5214 RVA: 0x000516C8 File Offset: 0x0004F8C8
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return SnapshotManager.SnapshotManager_CommitResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x0600145F RID: 5215 RVA: 0x000516D8 File Offset: 0x0004F8D8
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x06001460 RID: 5216 RVA: 0x000516E4 File Offset: 0x0004F8E4
			internal byte[] Data()
			{
				if (!this.RequestSucceeded())
				{
					throw new InvalidOperationException("Request did not succeed");
				}
				return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_bytes, UIntPtr out_size) => SnapshotManager.SnapshotManager_ReadResponse_GetData(base.SelfPtr(), out_bytes, out_size));
			}

			// Token: 0x06001461 RID: 5217 RVA: 0x00051710 File Offset: 0x0004F910
			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotManager.SnapshotManager_ReadResponse_Dispose(selfPointer);
			}

			// Token: 0x06001462 RID: 5218 RVA: 0x00051718 File Offset: 0x0004F918
			internal static SnapshotManager.ReadResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new SnapshotManager.ReadResponse(pointer);
			}
		}

		// Token: 0x0200027C RID: 636
		internal class SnapshotSelectUIResponse : BaseReferenceHolder
		{
			// Token: 0x06001464 RID: 5220 RVA: 0x00051748 File Offset: 0x0004F948
			internal SnapshotSelectUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001465 RID: 5221 RVA: 0x00051754 File Offset: 0x0004F954
			internal CommonErrorStatus.UIStatus RequestStatus()
			{
				return SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001466 RID: 5222 RVA: 0x00051764 File Offset: 0x0004F964
			internal bool RequestSucceeded()
			{
				return this.RequestStatus() > (CommonErrorStatus.UIStatus)0;
			}

			// Token: 0x06001467 RID: 5223 RVA: 0x00051770 File Offset: 0x0004F970
			internal NativeSnapshotMetadata Data()
			{
				if (!this.RequestSucceeded())
				{
					throw new InvalidOperationException("Request did not succeed");
				}
				return new NativeSnapshotMetadata(SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x06001468 RID: 5224 RVA: 0x000517A4 File Offset: 0x0004F9A4
			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_Dispose(selfPointer);
			}

			// Token: 0x06001469 RID: 5225 RVA: 0x000517AC File Offset: 0x0004F9AC
			internal static SnapshotManager.SnapshotSelectUIResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new SnapshotManager.SnapshotSelectUIResponse(pointer);
			}
		}
	}
}
