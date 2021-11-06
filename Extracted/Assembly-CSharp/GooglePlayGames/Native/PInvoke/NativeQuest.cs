using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000251 RID: 593
	internal class NativeQuest : BaseReferenceHolder, IQuest
	{
		// Token: 0x060012DE RID: 4830 RVA: 0x0004E830 File Offset: 0x0004CA30
		internal NativeQuest(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0004E83C File Offset: 0x0004CA3C
		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_Id(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0004E850 File Offset: 0x0004CA50
		public string Name
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_Name(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0004E864 File Offset: 0x0004CA64
		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_Description(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0004E878 File Offset: 0x0004CA78
		public string BannerUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_BannerUrl(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x0004E88C File Offset: 0x0004CA8C
		public string IconUrl
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => Quest.Quest_IconUrl(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0004E8A0 File Offset: 0x0004CAA0
		public DateTime StartTime
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(Quest.Quest_StartTime(base.SelfPtr()));
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0004E8B4 File Offset: 0x0004CAB4
		public DateTime ExpirationTime
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(Quest.Quest_ExpirationTime(base.SelfPtr()));
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0004E8C8 File Offset: 0x0004CAC8
		public DateTime? AcceptedTime
		{
			get
			{
				long num = Quest.Quest_AcceptedTime(base.SelfPtr());
				if (num == 0L)
				{
					return null;
				}
				return new DateTime?(PInvokeUtilities.FromMillisSinceUnixEpoch(num));
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0004E8FC File Offset: 0x0004CAFC
		public IQuestMilestone Milestone
		{
			get
			{
				if (this.mCachedMilestone == null)
				{
					this.mCachedMilestone = NativeQuestMilestone.FromPointer(Quest.Quest_CurrentMilestone(base.SelfPtr()));
				}
				return this.mCachedMilestone;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0004E92C File Offset: 0x0004CB2C
		public QuestState State
		{
			get
			{
				Types.QuestState questState = Quest.Quest_State(base.SelfPtr());
				switch (questState)
				{
				case Types.QuestState.UPCOMING:
					return QuestState.Upcoming;
				case Types.QuestState.OPEN:
					return QuestState.Open;
				case Types.QuestState.ACCEPTED:
					return QuestState.Accepted;
				case Types.QuestState.COMPLETED:
					return QuestState.Completed;
				case Types.QuestState.EXPIRED:
					return QuestState.Expired;
				case Types.QuestState.FAILED:
					return QuestState.Failed;
				default:
					throw new InvalidOperationException("Unknown state: " + questState);
				}
			}
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0004E990 File Offset: 0x0004CB90
		internal bool Valid()
		{
			return Quest.Quest_Valid(base.SelfPtr());
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0004E9A0 File Offset: 0x0004CBA0
		protected override void CallDispose(HandleRef selfPointer)
		{
			Quest.Quest_Dispose(selfPointer);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0004E9A8 File Offset: 0x0004CBA8
		public override string ToString()
		{
			if (base.IsDisposed())
			{
				return "[NativeQuest: DELETED]";
			}
			return string.Format("[NativeQuest: Id={0}, Name={1}, Description={2}, BannerUrl={3}, IconUrl={4}, State={5}, StartTime={6}, ExpirationTime={7}, AcceptedTime={8}]", new object[]
			{
				this.Id,
				this.Name,
				this.Description,
				this.BannerUrl,
				this.IconUrl,
				this.State,
				this.StartTime,
				this.ExpirationTime,
				this.AcceptedTime
			});
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0004EA3C File Offset: 0x0004CC3C
		internal static NativeQuest FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeQuest(pointer);
		}

		// Token: 0x04000C03 RID: 3075
		private volatile NativeQuestMilestone mCachedMilestone;
	}
}
