using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200025A RID: 602
	internal class NativeSnapshotMetadataChange : BaseReferenceHolder
	{
		// Token: 0x0600133F RID: 4927 RVA: 0x0004F1E0 File Offset: 0x0004D3E0
		internal NativeSnapshotMetadataChange(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0004F1EC File Offset: 0x0004D3EC
		protected override void CallDispose(HandleRef selfPointer)
		{
			SnapshotMetadataChange.SnapshotMetadataChange_Dispose(selfPointer);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0004F1F4 File Offset: 0x0004D3F4
		internal static NativeSnapshotMetadataChange FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeSnapshotMetadataChange(pointer);
		}

		// Token: 0x0200025B RID: 603
		internal class Builder : BaseReferenceHolder
		{
			// Token: 0x06001342 RID: 4930 RVA: 0x0004F214 File Offset: 0x0004D414
			internal Builder() : base(SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Construct())
			{
			}

			// Token: 0x06001343 RID: 4931 RVA: 0x0004F224 File Offset: 0x0004D424
			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Dispose(selfPointer);
			}

			// Token: 0x06001344 RID: 4932 RVA: 0x0004F22C File Offset: 0x0004D42C
			internal NativeSnapshotMetadataChange.Builder SetDescription(string description)
			{
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetDescription(base.SelfPtr(), description);
				return this;
			}

			// Token: 0x06001345 RID: 4933 RVA: 0x0004F23C File Offset: 0x0004D43C
			internal NativeSnapshotMetadataChange.Builder SetPlayedTime(ulong playedTime)
			{
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetPlayedTime(base.SelfPtr(), playedTime);
				return this;
			}

			// Token: 0x06001346 RID: 4934 RVA: 0x0004F24C File Offset: 0x0004D44C
			internal NativeSnapshotMetadataChange.Builder SetCoverImageFromPngData(byte[] pngData)
			{
				Misc.CheckNotNull<byte[]>(pngData);
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetCoverImageFromPngData(base.SelfPtr(), pngData, new UIntPtr((ulong)pngData.LongLength));
				return this;
			}

			// Token: 0x06001347 RID: 4935 RVA: 0x0004F278 File Offset: 0x0004D478
			internal NativeSnapshotMetadataChange Build()
			{
				return NativeSnapshotMetadataChange.FromPointer(SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Create(base.SelfPtr()));
			}
		}
	}
}
