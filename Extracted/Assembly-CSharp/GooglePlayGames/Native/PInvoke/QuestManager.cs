using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000269 RID: 617
	internal class QuestManager
	{
		// Token: 0x060013C2 RID: 5058 RVA: 0x0005037C File Offset: 0x0004E57C
		internal QuestManager(GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00050390 File Offset: 0x0004E590
		internal void Fetch(Types.DataSource source, string questId, Action<QuestManager.FetchResponse> callback)
		{
			QuestManager.QuestManager_Fetch(this.mServices.AsHandle(), source, questId, new QuestManager.FetchCallback(QuestManager.InternalFetchCallback), Callbacks.ToIntPtr<QuestManager.FetchResponse>(callback, new Func<IntPtr, QuestManager.FetchResponse>(QuestManager.FetchResponse.FromPointer)));
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x000503D0 File Offset: 0x0004E5D0
		[MonoPInvokeCallback(typeof(QuestManager.FetchCallback))]
		internal static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#FetchCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x000503E0 File Offset: 0x0004E5E0
		internal void FetchList(Types.DataSource source, int fetchFlags, Action<QuestManager.FetchListResponse> callback)
		{
			QuestManager.QuestManager_FetchList(this.mServices.AsHandle(), source, fetchFlags, new QuestManager.FetchListCallback(QuestManager.InternalFetchListCallback), Callbacks.ToIntPtr<QuestManager.FetchListResponse>(callback, new Func<IntPtr, QuestManager.FetchListResponse>(QuestManager.FetchListResponse.FromPointer)));
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00050420 File Offset: 0x0004E620
		[MonoPInvokeCallback(typeof(QuestManager.FetchListCallback))]
		internal static void InternalFetchListCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#FetchListCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00050430 File Offset: 0x0004E630
		internal void ShowAllQuestUI(Action<QuestManager.QuestUIResponse> callback)
		{
			QuestManager.QuestManager_ShowAllUI(this.mServices.AsHandle(), new QuestManager.QuestUICallback(QuestManager.InternalQuestUICallback), Callbacks.ToIntPtr<QuestManager.QuestUIResponse>(callback, new Func<IntPtr, QuestManager.QuestUIResponse>(QuestManager.QuestUIResponse.FromPointer)));
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0005046C File Offset: 0x0004E66C
		internal void ShowQuestUI(NativeQuest quest, Action<QuestManager.QuestUIResponse> callback)
		{
			QuestManager.QuestManager_ShowUI(this.mServices.AsHandle(), quest.AsPointer(), new QuestManager.QuestUICallback(QuestManager.InternalQuestUICallback), Callbacks.ToIntPtr<QuestManager.QuestUIResponse>(callback, new Func<IntPtr, QuestManager.QuestUIResponse>(QuestManager.QuestUIResponse.FromPointer)));
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x000504B0 File Offset: 0x0004E6B0
		[MonoPInvokeCallback(typeof(QuestManager.QuestUICallback))]
		internal static void InternalQuestUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#QuestUICallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000504C0 File Offset: 0x0004E6C0
		internal void Accept(NativeQuest quest, Action<QuestManager.AcceptResponse> callback)
		{
			QuestManager.QuestManager_Accept(this.mServices.AsHandle(), quest.AsPointer(), new QuestManager.AcceptCallback(QuestManager.InternalAcceptCallback), Callbacks.ToIntPtr<QuestManager.AcceptResponse>(callback, new Func<IntPtr, QuestManager.AcceptResponse>(QuestManager.AcceptResponse.FromPointer)));
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00050504 File Offset: 0x0004E704
		[MonoPInvokeCallback(typeof(QuestManager.AcceptCallback))]
		internal static void InternalAcceptCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#AcceptCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00050514 File Offset: 0x0004E714
		internal void ClaimMilestone(NativeQuestMilestone milestone, Action<QuestManager.ClaimMilestoneResponse> callback)
		{
			QuestManager.QuestManager_ClaimMilestone(this.mServices.AsHandle(), milestone.AsPointer(), new QuestManager.ClaimMilestoneCallback(QuestManager.InternalClaimMilestoneCallback), Callbacks.ToIntPtr<QuestManager.ClaimMilestoneResponse>(callback, new Func<IntPtr, QuestManager.ClaimMilestoneResponse>(QuestManager.ClaimMilestoneResponse.FromPointer)));
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00050558 File Offset: 0x0004E758
		[MonoPInvokeCallback(typeof(QuestManager.ClaimMilestoneCallback))]
		internal static void InternalClaimMilestoneCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("QuestManager#ClaimMilestoneCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x04000C0D RID: 3085
		private readonly GameServices mServices;

		// Token: 0x0200026A RID: 618
		internal class FetchResponse : BaseReferenceHolder
		{
			// Token: 0x060013CE RID: 5070 RVA: 0x00050568 File Offset: 0x0004E768
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013CF RID: 5071 RVA: 0x00050574 File Offset: 0x0004E774
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return QuestManager.QuestManager_FetchResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013D0 RID: 5072 RVA: 0x00050584 File Offset: 0x0004E784
			internal NativeQuest Data()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeQuest(QuestManager.QuestManager_FetchResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x060013D1 RID: 5073 RVA: 0x000505A4 File Offset: 0x0004E7A4
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x060013D2 RID: 5074 RVA: 0x000505B0 File Offset: 0x0004E7B0
			protected override void CallDispose(HandleRef selfPointer)
			{
				QuestManager.QuestManager_FetchResponse_Dispose(selfPointer);
			}

			// Token: 0x060013D3 RID: 5075 RVA: 0x000505B8 File Offset: 0x0004E7B8
			internal static QuestManager.FetchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new QuestManager.FetchResponse(pointer);
			}
		}

		// Token: 0x0200026B RID: 619
		internal class FetchListResponse : BaseReferenceHolder
		{
			// Token: 0x060013D4 RID: 5076 RVA: 0x000505D8 File Offset: 0x0004E7D8
			internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013D5 RID: 5077 RVA: 0x000505E4 File Offset: 0x0004E7E4
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return QuestManager.QuestManager_FetchListResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013D6 RID: 5078 RVA: 0x000505F4 File Offset: 0x0004E7F4
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x060013D7 RID: 5079 RVA: 0x00050600 File Offset: 0x0004E800
			internal IEnumerable<NativeQuest> Data()
			{
				return PInvokeUtilities.ToEnumerable<NativeQuest>(QuestManager.QuestManager_FetchListResponse_GetData_Length(base.SelfPtr()), (UIntPtr index) => new NativeQuest(QuestManager.QuestManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index)));
			}

			// Token: 0x060013D8 RID: 5080 RVA: 0x00050620 File Offset: 0x0004E820
			protected override void CallDispose(HandleRef selfPointer)
			{
				QuestManager.QuestManager_FetchListResponse_Dispose(selfPointer);
			}

			// Token: 0x060013D9 RID: 5081 RVA: 0x00050628 File Offset: 0x0004E828
			internal static QuestManager.FetchListResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new QuestManager.FetchListResponse(pointer);
			}
		}

		// Token: 0x0200026C RID: 620
		internal class ClaimMilestoneResponse : BaseReferenceHolder
		{
			// Token: 0x060013DB RID: 5083 RVA: 0x0005065C File Offset: 0x0004E85C
			internal ClaimMilestoneResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013DC RID: 5084 RVA: 0x00050668 File Offset: 0x0004E868
			internal CommonErrorStatus.QuestClaimMilestoneStatus ResponseStatus()
			{
				return QuestManager.QuestManager_ClaimMilestoneResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013DD RID: 5085 RVA: 0x00050678 File Offset: 0x0004E878
			internal NativeQuest Quest()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuest nativeQuest = new NativeQuest(QuestManager.QuestManager_ClaimMilestoneResponse_GetQuest(base.SelfPtr()));
				if (nativeQuest.Valid())
				{
					return nativeQuest;
				}
				nativeQuest.Dispose();
				return null;
			}

			// Token: 0x060013DE RID: 5086 RVA: 0x000506B8 File Offset: 0x0004E8B8
			internal NativeQuestMilestone ClaimedMilestone()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuestMilestone nativeQuestMilestone = new NativeQuestMilestone(QuestManager.QuestManager_ClaimMilestoneResponse_GetClaimedMilestone(base.SelfPtr()));
				if (nativeQuestMilestone.Valid())
				{
					return nativeQuestMilestone;
				}
				nativeQuestMilestone.Dispose();
				return null;
			}

			// Token: 0x060013DF RID: 5087 RVA: 0x000506F8 File Offset: 0x0004E8F8
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.QuestClaimMilestoneStatus)0;
			}

			// Token: 0x060013E0 RID: 5088 RVA: 0x00050704 File Offset: 0x0004E904
			protected override void CallDispose(HandleRef selfPointer)
			{
				QuestManager.QuestManager_ClaimMilestoneResponse_Dispose(selfPointer);
			}

			// Token: 0x060013E1 RID: 5089 RVA: 0x0005070C File Offset: 0x0004E90C
			internal static QuestManager.ClaimMilestoneResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new QuestManager.ClaimMilestoneResponse(pointer);
			}
		}

		// Token: 0x0200026D RID: 621
		internal class AcceptResponse : BaseReferenceHolder
		{
			// Token: 0x060013E2 RID: 5090 RVA: 0x0005072C File Offset: 0x0004E92C
			internal AcceptResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013E3 RID: 5091 RVA: 0x00050738 File Offset: 0x0004E938
			internal CommonErrorStatus.QuestAcceptStatus ResponseStatus()
			{
				return QuestManager.QuestManager_AcceptResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013E4 RID: 5092 RVA: 0x00050748 File Offset: 0x0004E948
			internal NativeQuest AcceptedQuest()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeQuest(QuestManager.QuestManager_AcceptResponse_GetAcceptedQuest(base.SelfPtr()));
			}

			// Token: 0x060013E5 RID: 5093 RVA: 0x00050768 File Offset: 0x0004E968
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.QuestAcceptStatus)0;
			}

			// Token: 0x060013E6 RID: 5094 RVA: 0x00050774 File Offset: 0x0004E974
			protected override void CallDispose(HandleRef selfPointer)
			{
				QuestManager.QuestManager_AcceptResponse_Dispose(selfPointer);
			}

			// Token: 0x060013E7 RID: 5095 RVA: 0x0005077C File Offset: 0x0004E97C
			internal static QuestManager.AcceptResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new QuestManager.AcceptResponse(pointer);
			}
		}

		// Token: 0x0200026E RID: 622
		internal class QuestUIResponse : BaseReferenceHolder
		{
			// Token: 0x060013E8 RID: 5096 RVA: 0x0005079C File Offset: 0x0004E99C
			internal QuestUIResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060013E9 RID: 5097 RVA: 0x000507A8 File Offset: 0x0004E9A8
			internal CommonErrorStatus.UIStatus RequestStatus()
			{
				return QuestManager.QuestManager_QuestUIResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060013EA RID: 5098 RVA: 0x000507B8 File Offset: 0x0004E9B8
			internal bool RequestSucceeded()
			{
				return this.RequestStatus() > (CommonErrorStatus.UIStatus)0;
			}

			// Token: 0x060013EB RID: 5099 RVA: 0x000507C4 File Offset: 0x0004E9C4
			internal NativeQuest AcceptedQuest()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuest nativeQuest = new NativeQuest(QuestManager.QuestManager_QuestUIResponse_GetAcceptedQuest(base.SelfPtr()));
				if (nativeQuest.Valid())
				{
					return nativeQuest;
				}
				nativeQuest.Dispose();
				return null;
			}

			// Token: 0x060013EC RID: 5100 RVA: 0x00050804 File Offset: 0x0004EA04
			internal NativeQuestMilestone MilestoneToClaim()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				NativeQuestMilestone nativeQuestMilestone = new NativeQuestMilestone(QuestManager.QuestManager_QuestUIResponse_GetMilestoneToClaim(base.SelfPtr()));
				if (nativeQuestMilestone.Valid())
				{
					return nativeQuestMilestone;
				}
				nativeQuestMilestone.Dispose();
				return null;
			}

			// Token: 0x060013ED RID: 5101 RVA: 0x00050844 File Offset: 0x0004EA44
			protected override void CallDispose(HandleRef selfPointer)
			{
				QuestManager.QuestManager_QuestUIResponse_Dispose(selfPointer);
			}

			// Token: 0x060013EE RID: 5102 RVA: 0x0005084C File Offset: 0x0004EA4C
			internal static QuestManager.QuestUIResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new QuestManager.QuestUIResponse(pointer);
			}
		}
	}
}
