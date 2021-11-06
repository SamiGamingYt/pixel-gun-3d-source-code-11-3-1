using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000259 RID: 601
	internal class NativeSnapshotMetadata : BaseReferenceHolder, ISavedGameMetadata
	{
		// Token: 0x06001333 RID: 4915 RVA: 0x0004F088 File Offset: 0x0004D288
		internal NativeSnapshotMetadata(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0004F094 File Offset: 0x0004D294
		public bool IsOpen
		{
			get
			{
				return SnapshotMetadata.SnapshotMetadata_IsOpen(base.SelfPtr());
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0004F0A4 File Offset: 0x0004D2A4
		public string Filename
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_FileName(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x0004F0B8 File Offset: 0x0004D2B8
		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_Description(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0004F0CC File Offset: 0x0004D2CC
		public string CoverImageURL
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_CoverImageURL(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0004F0E0 File Offset: 0x0004D2E0
		public TimeSpan TotalTimePlayed
		{
			get
			{
				long num = SnapshotMetadata.SnapshotMetadata_PlayedTime(base.SelfPtr());
				if (num < 0L)
				{
					return TimeSpan.FromMilliseconds(0.0);
				}
				return TimeSpan.FromMilliseconds((double)num);
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x0004F118 File Offset: 0x0004D318
		public DateTime LastModifiedTimestamp
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(SnapshotMetadata.SnapshotMetadata_LastModifiedTime(base.SelfPtr()));
			}
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0004F12C File Offset: 0x0004D32C
		public override string ToString()
		{
			if (base.IsDisposed())
			{
				return "[NativeSnapshotMetadata: DELETED]";
			}
			return string.Format("[NativeSnapshotMetadata: IsOpen={0}, Filename={1}, Description={2}, CoverImageUrl={3}, TotalTimePlayed={4}, LastModifiedTimestamp={5}]", new object[]
			{
				this.IsOpen,
				this.Filename,
				this.Description,
				this.CoverImageURL,
				this.TotalTimePlayed,
				this.LastModifiedTimestamp
			});
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0004F1A0 File Offset: 0x0004D3A0
		protected override void CallDispose(HandleRef selfPointer)
		{
			SnapshotMetadata.SnapshotMetadata_Dispose(base.SelfPtr());
		}
	}
}
