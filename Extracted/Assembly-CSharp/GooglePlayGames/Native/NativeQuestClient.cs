﻿using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Quests;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x02000220 RID: 544
	internal class NativeQuestClient : IQuestsClient
	{
		// Token: 0x060010F7 RID: 4343 RVA: 0x00049374 File Offset: 0x00047574
		internal NativeQuestClient(GooglePlayGames.Native.PInvoke.QuestManager manager)
		{
			this.mManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.QuestManager>(manager);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00049388 File Offset: 0x00047588
		public void Fetch(DataSource source, string questId, Action<ResponseStatus, IQuest> callback)
		{
			Misc.CheckNotNull<string>(questId);
			Misc.CheckNotNull<Action<ResponseStatus, IQuest>>(callback);
			callback = CallbackUtils.ToOnGameThread<ResponseStatus, IQuest>(callback);
			this.mManager.Fetch(ConversionUtils.AsDataSource(source), questId, delegate(GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data());
				}
			});
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x000493E4 File Offset: 0x000475E4
		public void FetchMatchingState(DataSource source, QuestFetchFlags flags, Action<ResponseStatus, List<IQuest>> callback)
		{
			Misc.CheckNotNull<Action<ResponseStatus, List<IQuest>>>(callback);
			callback = CallbackUtils.ToOnGameThread<ResponseStatus, List<IQuest>>(callback);
			this.mManager.FetchList(ConversionUtils.AsDataSource(source), (int)flags, delegate(GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data().Cast<IQuest>().ToList<IQuest>());
				}
			});
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0004943C File Offset: 0x0004763C
		public void ShowAllQuestsUI(Action<QuestUiResult, IQuest, IQuestMilestone> callback)
		{
			Misc.CheckNotNull<Action<QuestUiResult, IQuest, IQuestMilestone>>(callback);
			callback = CallbackUtils.ToOnGameThread<QuestUiResult, IQuest, IQuestMilestone>(callback);
			this.mManager.ShowAllQuestUI(NativeQuestClient.FromQuestUICallback(callback));
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004946C File Offset: 0x0004766C
		public void ShowSpecificQuestUI(IQuest quest, Action<QuestUiResult, IQuest, IQuestMilestone> callback)
		{
			Misc.CheckNotNull<IQuest>(quest);
			Misc.CheckNotNull<Action<QuestUiResult, IQuest, IQuestMilestone>>(callback);
			callback = CallbackUtils.ToOnGameThread<QuestUiResult, IQuest, IQuestMilestone>(callback);
			NativeQuest nativeQuest = quest as NativeQuest;
			if (nativeQuest == null)
			{
				Logger.e("Encountered quest that was not generated by this IQuestClient");
				callback(QuestUiResult.BadInput, null, null);
				return;
			}
			this.mManager.ShowQuestUI(nativeQuest, NativeQuestClient.FromQuestUICallback(callback));
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x000494C4 File Offset: 0x000476C4
		private static QuestUiResult UiErrorToQuestUiResult(CommonErrorStatus.UIStatus status)
		{
			switch (status + 12)
			{
			case (CommonErrorStatus.UIStatus)0:
				return QuestUiResult.UiBusy;
			case (CommonErrorStatus.UIStatus)6:
				return QuestUiResult.UserCanceled;
			case (CommonErrorStatus.UIStatus)7:
				return QuestUiResult.Timeout;
			case (CommonErrorStatus.UIStatus)8:
				return QuestUiResult.VersionUpdateRequired;
			case (CommonErrorStatus.UIStatus)9:
				return QuestUiResult.NotAuthorized;
			case (CommonErrorStatus.UIStatus)10:
				return QuestUiResult.InternalError;
			}
			Logger.e("Unknown error status: " + status);
			return QuestUiResult.InternalError;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00049530 File Offset: 0x00047730
		private static Action<GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse> FromQuestUICallback(Action<QuestUiResult, IQuest, IQuestMilestone> callback)
		{
			return delegate(GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse response)
			{
				if (!response.RequestSucceeded())
				{
					callback(NativeQuestClient.UiErrorToQuestUiResult(response.RequestStatus()), null, null);
					return;
				}
				NativeQuest nativeQuest = response.AcceptedQuest();
				NativeQuestMilestone nativeQuestMilestone = response.MilestoneToClaim();
				if (nativeQuest != null)
				{
					callback(QuestUiResult.UserRequestsQuestAcceptance, nativeQuest, null);
					nativeQuestMilestone.Dispose();
				}
				else if (nativeQuestMilestone != null)
				{
					callback(QuestUiResult.UserRequestsMilestoneClaiming, null, response.MilestoneToClaim());
					nativeQuest.Dispose();
				}
				else
				{
					Logger.e("Quest UI succeeded without a quest acceptance or milestone claim.");
					nativeQuest.Dispose();
					nativeQuestMilestone.Dispose();
					callback(QuestUiResult.InternalError, null, null);
				}
			};
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00049558 File Offset: 0x00047758
		public void Accept(IQuest quest, Action<QuestAcceptStatus, IQuest> callback)
		{
			Misc.CheckNotNull<IQuest>(quest);
			Misc.CheckNotNull<Action<QuestAcceptStatus, IQuest>>(callback);
			callback = CallbackUtils.ToOnGameThread<QuestAcceptStatus, IQuest>(callback);
			NativeQuest nativeQuest = quest as NativeQuest;
			if (nativeQuest == null)
			{
				Logger.e("Encountered quest that was not generated by this IQuestClient");
				callback(QuestAcceptStatus.BadInput, null);
				return;
			}
			this.mManager.Accept(nativeQuest, delegate(GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse response)
			{
				if (response.RequestSucceeded())
				{
					callback(QuestAcceptStatus.Success, response.AcceptedQuest());
				}
				else
				{
					callback(NativeQuestClient.FromAcceptStatus(response.ResponseStatus()), null);
				}
			});
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000495D4 File Offset: 0x000477D4
		private static QuestAcceptStatus FromAcceptStatus(CommonErrorStatus.QuestAcceptStatus status)
		{
			switch (status + 5)
			{
			case (CommonErrorStatus.QuestAcceptStatus)0:
				return QuestAcceptStatus.Timeout;
			default:
				if (status == CommonErrorStatus.QuestAcceptStatus.ERROR_QUEST_NOT_STARTED)
				{
					return QuestAcceptStatus.QuestNotStarted;
				}
				if (status != CommonErrorStatus.QuestAcceptStatus.ERROR_QUEST_NO_LONGER_AVAILABLE)
				{
					Logger.e("Encountered unknown status: " + status);
					return QuestAcceptStatus.InternalError;
				}
				return QuestAcceptStatus.QuestNoLongerAvailable;
			case (CommonErrorStatus.QuestAcceptStatus)2:
				return QuestAcceptStatus.NotAuthorized;
			case (CommonErrorStatus.QuestAcceptStatus)3:
				return QuestAcceptStatus.InternalError;
			case (CommonErrorStatus.QuestAcceptStatus)6:
				return QuestAcceptStatus.Success;
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00049640 File Offset: 0x00047840
		public void ClaimMilestone(IQuestMilestone milestone, Action<QuestClaimMilestoneStatus, IQuest, IQuestMilestone> callback)
		{
			Misc.CheckNotNull<IQuestMilestone>(milestone);
			Misc.CheckNotNull<Action<QuestClaimMilestoneStatus, IQuest, IQuestMilestone>>(callback);
			callback = CallbackUtils.ToOnGameThread<QuestClaimMilestoneStatus, IQuest, IQuestMilestone>(callback);
			NativeQuestMilestone nativeQuestMilestone = milestone as NativeQuestMilestone;
			if (nativeQuestMilestone == null)
			{
				Logger.e("Encountered milestone that was not generated by this IQuestClient");
				callback(QuestClaimMilestoneStatus.BadInput, null, null);
				return;
			}
			this.mManager.ClaimMilestone(nativeQuestMilestone, delegate(GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse response)
			{
				if (response.RequestSucceeded())
				{
					callback(QuestClaimMilestoneStatus.Success, response.Quest(), response.ClaimedMilestone());
				}
				else
				{
					callback(NativeQuestClient.FromClaimStatus(response.ResponseStatus()), null, null);
				}
			});
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000496BC File Offset: 0x000478BC
		private static QuestClaimMilestoneStatus FromClaimStatus(CommonErrorStatus.QuestClaimMilestoneStatus status)
		{
			switch (status + 5)
			{
			case (CommonErrorStatus.QuestClaimMilestoneStatus)0:
				return QuestClaimMilestoneStatus.Timeout;
			default:
				if (status == CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_MILESTONE_CLAIM_FAILED)
				{
					return QuestClaimMilestoneStatus.MilestoneClaimFailed;
				}
				if (status != CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_MILESTONE_ALREADY_CLAIMED)
				{
					Logger.e("Encountered unknown status: " + status);
					return QuestClaimMilestoneStatus.InternalError;
				}
				return QuestClaimMilestoneStatus.MilestoneAlreadyClaimed;
			case (CommonErrorStatus.QuestClaimMilestoneStatus)2:
				return QuestClaimMilestoneStatus.NotAuthorized;
			case (CommonErrorStatus.QuestClaimMilestoneStatus)3:
				return QuestClaimMilestoneStatus.InternalError;
			case (CommonErrorStatus.QuestClaimMilestoneStatus)6:
				return QuestClaimMilestoneStatus.Success;
			}
		}

		// Token: 0x04000BBB RID: 3003
		private readonly GooglePlayGames.Native.PInvoke.QuestManager mManager;
	}
}