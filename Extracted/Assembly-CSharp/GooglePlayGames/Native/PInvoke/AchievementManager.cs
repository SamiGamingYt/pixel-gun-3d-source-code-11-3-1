using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000232 RID: 562
	internal class AchievementManager
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x0004C460 File Offset: 0x0004A660
		internal AchievementManager(GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004C474 File Offset: 0x0004A674
		internal void ShowAllUI(Action<CommonErrorStatus.UIStatus> callback)
		{
			Misc.CheckNotNull<Action<CommonErrorStatus.UIStatus>>(callback);
			AchievementManager.AchievementManager_ShowAllUI(this.mServices.AsHandle(), new AchievementManager.ShowAllUICallback(Callbacks.InternalShowUICallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004C4A0 File Offset: 0x0004A6A0
		internal void FetchAll(Action<AchievementManager.FetchAllResponse> callback)
		{
			Misc.CheckNotNull<Action<AchievementManager.FetchAllResponse>>(callback);
			AchievementManager.AchievementManager_FetchAll(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new AchievementManager.FetchAllCallback(AchievementManager.InternalFetchAllCallback), Callbacks.ToIntPtr<AchievementManager.FetchAllResponse>(callback, new Func<IntPtr, AchievementManager.FetchAllResponse>(AchievementManager.FetchAllResponse.FromPointer)));
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0004C4E4 File Offset: 0x0004A6E4
		[MonoPInvokeCallback(typeof(AchievementManager.FetchAllCallback))]
		private static void InternalFetchAllCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("AchievementManager#InternalFetchAllCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0004C4F4 File Offset: 0x0004A6F4
		internal void Fetch(string achId, Action<AchievementManager.FetchResponse> callback)
		{
			Misc.CheckNotNull<string>(achId);
			Misc.CheckNotNull<Action<AchievementManager.FetchResponse>>(callback);
			AchievementManager.AchievementManager_Fetch(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, achId, new AchievementManager.FetchCallback(AchievementManager.InternalFetchCallback), Callbacks.ToIntPtr<AchievementManager.FetchResponse>(callback, new Func<IntPtr, AchievementManager.FetchResponse>(AchievementManager.FetchResponse.FromPointer)));
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0004C540 File Offset: 0x0004A740
		[MonoPInvokeCallback(typeof(AchievementManager.FetchCallback))]
		private static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("AchievementManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0004C550 File Offset: 0x0004A750
		internal void Increment(string achievementId, uint numSteps)
		{
			Misc.CheckNotNull<string>(achievementId);
			AchievementManager.AchievementManager_Increment(this.mServices.AsHandle(), achievementId, numSteps);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0004C56C File Offset: 0x0004A76C
		internal void SetStepsAtLeast(string achivementId, uint numSteps)
		{
			Misc.CheckNotNull<string>(achivementId);
			AchievementManager.AchievementManager_SetStepsAtLeast(this.mServices.AsHandle(), achivementId, numSteps);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004C588 File Offset: 0x0004A788
		internal void Reveal(string achievementId)
		{
			Misc.CheckNotNull<string>(achievementId);
			AchievementManager.AchievementManager_Reveal(this.mServices.AsHandle(), achievementId);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004C5A4 File Offset: 0x0004A7A4
		internal void Unlock(string achievementId)
		{
			Misc.CheckNotNull<string>(achievementId);
			AchievementManager.AchievementManager_Unlock(this.mServices.AsHandle(), achievementId);
		}

		// Token: 0x04000BF6 RID: 3062
		private readonly GameServices mServices;

		// Token: 0x02000233 RID: 563
		internal class FetchResponse : BaseReferenceHolder
		{
			// Token: 0x060011CE RID: 4558 RVA: 0x0004C5C0 File Offset: 0x0004A7C0
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060011CF RID: 4559 RVA: 0x0004C5CC File Offset: 0x0004A7CC
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return AchievementManager.AchievementManager_FetchResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060011D0 RID: 4560 RVA: 0x0004C5DC File Offset: 0x0004A7DC
			internal NativeAchievement Achievement()
			{
				IntPtr selfPointer = AchievementManager.AchievementManager_FetchResponse_GetData(base.SelfPtr());
				return new NativeAchievement(selfPointer);
			}

			// Token: 0x060011D1 RID: 4561 RVA: 0x0004C5FC File Offset: 0x0004A7FC
			protected override void CallDispose(HandleRef selfPointer)
			{
				AchievementManager.AchievementManager_FetchResponse_Dispose(selfPointer);
			}

			// Token: 0x060011D2 RID: 4562 RVA: 0x0004C604 File Offset: 0x0004A804
			internal static AchievementManager.FetchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new AchievementManager.FetchResponse(pointer);
			}
		}

		// Token: 0x02000234 RID: 564
		internal class FetchAllResponse : BaseReferenceHolder, IEnumerable, IEnumerable<NativeAchievement>
		{
			// Token: 0x060011D3 RID: 4563 RVA: 0x0004C624 File Offset: 0x0004A824
			internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x060011D4 RID: 4564 RVA: 0x0004C630 File Offset: 0x0004A830
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060011D5 RID: 4565 RVA: 0x0004C638 File Offset: 0x0004A838
			internal CommonErrorStatus.ResponseStatus Status()
			{
				return AchievementManager.AchievementManager_FetchAllResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x060011D6 RID: 4566 RVA: 0x0004C648 File Offset: 0x0004A848
			private UIntPtr Length()
			{
				return AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(base.SelfPtr());
			}

			// Token: 0x060011D7 RID: 4567 RVA: 0x0004C658 File Offset: 0x0004A858
			private NativeAchievement GetElement(UIntPtr index)
			{
				if (index.ToUInt64() >= this.Length().ToUInt64())
				{
					throw new ArgumentOutOfRangeException();
				}
				return new NativeAchievement(AchievementManager.AchievementManager_FetchAllResponse_GetData_GetElement(base.SelfPtr(), index));
			}

			// Token: 0x060011D8 RID: 4568 RVA: 0x0004C698 File Offset: 0x0004A898
			public IEnumerator<NativeAchievement> GetEnumerator()
			{
				return PInvokeUtilities.ToEnumerator<NativeAchievement>(AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(base.SelfPtr()), (UIntPtr index) => this.GetElement(index));
			}

			// Token: 0x060011D9 RID: 4569 RVA: 0x0004C6B8 File Offset: 0x0004A8B8
			protected override void CallDispose(HandleRef selfPointer)
			{
				AchievementManager.AchievementManager_FetchAllResponse_Dispose(selfPointer);
			}

			// Token: 0x060011DA RID: 4570 RVA: 0x0004C6C0 File Offset: 0x0004A8C0
			internal static AchievementManager.FetchAllResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new AchievementManager.FetchAllResponse(pointer);
			}
		}
	}
}
