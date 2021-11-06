using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000252 RID: 594
	internal class NativeQuestMilestone : BaseReferenceHolder, IQuestMilestone
	{
		// Token: 0x060012F2 RID: 4850 RVA: 0x0004EAAC File Offset: 0x0004CCAC
		internal NativeQuestMilestone(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0004EAB8 File Offset: 0x0004CCB8
		public string Id
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => QuestMilestone.QuestMilestone_Id(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x0004EACC File Offset: 0x0004CCCC
		public string EventId
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => QuestMilestone.QuestMilestone_EventId(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0004EAE0 File Offset: 0x0004CCE0
		public string QuestId
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => QuestMilestone.QuestMilestone_QuestId(base.SelfPtr(), out_string, out_size));
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x0004EAF4 File Offset: 0x0004CCF4
		public ulong CurrentCount
		{
			get
			{
				return QuestMilestone.QuestMilestone_CurrentCount(base.SelfPtr());
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0004EB04 File Offset: 0x0004CD04
		public ulong TargetCount
		{
			get
			{
				return QuestMilestone.QuestMilestone_TargetCount(base.SelfPtr());
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0004EB14 File Offset: 0x0004CD14
		public byte[] CompletionRewardData
		{
			get
			{
				return PInvokeUtilities.OutParamsToArray<byte>((byte[] out_bytes, UIntPtr out_size) => QuestMilestone.QuestMilestone_CompletionRewardData(base.SelfPtr(), out_bytes, out_size));
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0004EB28 File Offset: 0x0004CD28
		public MilestoneState State
		{
			get
			{
				Types.QuestMilestoneState questMilestoneState = QuestMilestone.QuestMilestone_State(base.SelfPtr());
				switch (questMilestoneState)
				{
				case Types.QuestMilestoneState.NOT_STARTED:
					return MilestoneState.NotStarted;
				case Types.QuestMilestoneState.NOT_COMPLETED:
					return MilestoneState.NotCompleted;
				case Types.QuestMilestoneState.COMPLETED_NOT_CLAIMED:
					return MilestoneState.CompletedNotClaimed;
				case Types.QuestMilestoneState.CLAIMED:
					return MilestoneState.Claimed;
				default:
					throw new InvalidOperationException("Unknown state: " + questMilestoneState);
				}
			}
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0004EB80 File Offset: 0x0004CD80
		internal bool Valid()
		{
			return QuestMilestone.QuestMilestone_Valid(base.SelfPtr());
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0004EB90 File Offset: 0x0004CD90
		protected override void CallDispose(HandleRef selfPointer)
		{
			QuestMilestone.QuestMilestone_Dispose(selfPointer);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0004EB98 File Offset: 0x0004CD98
		public override string ToString()
		{
			return string.Format("[NativeQuestMilestone: Id={0}, EventId={1}, QuestId={2}, CurrentCount={3}, TargetCount={4}, State={5}]", new object[]
			{
				this.Id,
				this.EventId,
				this.QuestId,
				this.CurrentCount,
				this.TargetCount,
				this.State
			});
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0004EBFC File Offset: 0x0004CDFC
		internal static NativeQuestMilestone FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeQuestMilestone(pointer);
		}
	}
}
