using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000693 RID: 1683
	public class LeprechauntManager : Singleton<LeprechauntManager>
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x001301A0 File Offset: 0x0012E3A0
		// (set) Token: 0x06003AD5 RID: 15061 RVA: 0x001301B0 File Offset: 0x0012E3B0
		public int LifeTimeSeconds
		{
			get
			{
				return this._lifeTime.Value;
			}
			private set
			{
				this._lifeTime.Value = value;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x001301C0 File Offset: 0x0012E3C0
		// (set) Token: 0x06003AD7 RID: 15063 RVA: 0x001301D0 File Offset: 0x0012E3D0
		public int RewardCount
		{
			get
			{
				return this._rewardCount.Value;
			}
			private set
			{
				this._rewardCount.Value = value;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x001301E0 File Offset: 0x0012E3E0
		// (set) Token: 0x06003AD9 RID: 15065 RVA: 0x001301F0 File Offset: 0x0012E3F0
		public string RewardCurrency
		{
			get
			{
				return this._rewardCurrency.Value;
			}
			private set
			{
				this._rewardCurrency.Value = value;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x00130200 File Offset: 0x0012E400
		// (set) Token: 0x06003ADB RID: 15067 RVA: 0x00130210 File Offset: 0x0012E410
		public int DropDelaySeconds
		{
			get
			{
				return this._dropDelaySecs.Value;
			}
			private set
			{
				this._dropDelaySecs.Value = value;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x00130220 File Offset: 0x0012E420
		public long? CurrentTime
		{
			get
			{
				if (FriendsController.ServerTime < 1L)
				{
					return null;
				}
				return new long?(FriendsController.ServerTime);
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x00130250 File Offset: 0x0012E450
		public bool LeprechauntExists
		{
			get
			{
				return this._comeTimeSeconds.Value > 0;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x00130260 File Offset: 0x0012E460
		public int? LeprechauntEndTime
		{
			get
			{
				if (!this.LeprechauntExists)
				{
					return null;
				}
				return new int?(this._comeTimeSeconds.Value + this.LifeTimeSeconds);
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x0013029C File Offset: 0x0012E49C
		public int? LeprechauntLifeTimeLeft
		{
			get
			{
				if (this.CurrentTime == null || !this.LeprechauntExists)
				{
					return null;
				}
				int? leprechauntEndTime = this.LeprechauntEndTime;
				int? num = (leprechauntEndTime == null) ? null : new int?(leprechauntEndTime.Value - (int)this.CurrentTime.Value);
				int? result;
				if (num != null && num.Value > 0)
				{
					int? leprechauntEndTime2 = this.LeprechauntEndTime;
					result = ((leprechauntEndTime2 == null) ? null : new int?(leprechauntEndTime2.Value - (int)this.CurrentTime.Value));
				}
				else
				{
					result = new int?(0);
				}
				return result;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x00130374 File Offset: 0x0012E574
		public bool LeprechauntTimeOff
		{
			get
			{
				return (long)(this._comeTimeSeconds.Value + this.LifeTimeSeconds) < this.CurrentTime.Value;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x001303A4 File Offset: 0x0012E5A4
		public float? RewardDropSecsLeft
		{
			get
			{
				if (this.CurrentTime != null)
				{
					return new float?((float)((long)(this._lastDropTimeSeconds.Value + this.DropDelaySeconds) - this.CurrentTime.Value));
				}
				return null;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x001303F8 File Offset: 0x0012E5F8
		public bool RewardIsReadyToDrop
		{
			get
			{
				bool result;
				if (this.LeprechauntExists && this.RewardDropSecsLeft != null)
				{
					float? rewardDropSecsLeft = this.RewardDropSecsLeft;
					result = (rewardDropSecsLeft != null && rewardDropSecsLeft.Value <= 0f);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x00130450 File Offset: 0x0012E650
		public int RewardReadyToDrop
		{
			get
			{
				if (this.CurrentTime == null || this.LeprechauntEndTime == null)
				{
					return -1;
				}
				return (this.ElapsedDropIntervals <= 0) ? 0 : this.RewardCount;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x001304A0 File Offset: 0x0012E6A0
		private int ElapsedDropIntervals
		{
			get
			{
				if (this.CurrentTime == null || this.LeprechauntEndTime == null)
				{
					return -1;
				}
				if ((long)this.LeprechauntEndTime.Value < this.CurrentTime.Value)
				{
					return 1;
				}
				long num = this.CurrentTime.Value - (long)this._lastDropTimeSeconds.Value;
				return Mathf.CeilToInt((float)(num / (long)this.DropDelaySeconds));
			}
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0013052C File Offset: 0x0012E72C
		private void Update()
		{
			if (this._needReset.Value)
			{
				this.Reset();
			}
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00130544 File Offset: 0x0012E744
		public void SetLeprechaunt(int liveTimeSeconds, string rewardCurrency, int rewardCount, int rewardDropDelaySeconds = 86400)
		{
			Debug.Log(">>> L: set started");
			if (this.LeprechauntExists)
			{
				Debug.LogError("leprechaun allready exists");
				return;
			}
			Debug.Log(">>> L: exists pass");
			this.LifeTimeSeconds = liveTimeSeconds;
			this.RewardCurrency = rewardCurrency;
			this.RewardCount = rewardCount;
			this.DropDelaySeconds = rewardDropDelaySeconds;
			this.Reset();
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001305A0 File Offset: 0x0012E7A0
		private void Reset()
		{
			if (this.CurrentTime == null)
			{
				this._needReset.Value = true;
				return;
			}
			this._needReset.Value = false;
			Debug.Log(">>> L: reset");
			CachedProperty<int> comeTimeSeconds = this._comeTimeSeconds;
			long? currentTime = this.CurrentTime;
			comeTimeSeconds.Value = (int)((currentTime == null) ? null : new long?(currentTime.Value - (long)this.DropDelaySeconds)).Value;
			this._lastDropTimeSeconds.Value = (int)(this.CurrentTime.Value - (long)this.DropDelaySeconds);
			Debug.Log(string.Concat(new object[]
			{
				">>> L: reset to: LifeTimeSeconds: ",
				this.LifeTimeSeconds,
				" RewardCurrency: ",
				this.RewardCurrency,
				" RewardCount ",
				this.RewardCount,
				" DropDelaySeconds ",
				this.DropDelaySeconds
			}));
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001306B0 File Offset: 0x0012E8B0
		public void RemoveLeprechaunt()
		{
			if (this.CurrentTime == null)
			{
				return;
			}
			if (!this.LeprechauntExists)
			{
				return;
			}
			this._comeTimeSeconds.Value = -1;
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001306EC File Offset: 0x0012E8EC
		public void DropReward()
		{
			if (this.CurrentTime == null || this.LeprechauntEndTime == null)
			{
				return;
			}
			if (!this.RewardIsReadyToDrop)
			{
				return;
			}
			if (this.RewardCurrency == "GemsCurrency")
			{
				BankController.AddGems(this.RewardReadyToDrop, true, AnalyticsConstants.AccrualType.Earned);
			}
			else
			{
				BankController.AddCoins(this.RewardReadyToDrop, true, AnalyticsConstants.AccrualType.Earned);
			}
			if (!this.LeprechauntTimeOff)
			{
				this._lastDropTimeSeconds.Value = (int)this.CurrentTime.Value;
			}
			else
			{
				this.RemoveLeprechaunt();
			}
		}

		// Token: 0x04002B6E RID: 11118
		private readonly StoragerIntCachedProperty _comeTimeSeconds = new StoragerIntCachedProperty("leprechaunt_come_time", false);

		// Token: 0x04002B6F RID: 11119
		private readonly StoragerIntCachedProperty _lastDropTimeSeconds = new StoragerIntCachedProperty("leprechaunt_last_drop_time", false);

		// Token: 0x04002B70 RID: 11120
		private readonly StoragerIntCachedProperty _lifeTime = new StoragerIntCachedProperty("leprechaunt_lifeTime", false);

		// Token: 0x04002B71 RID: 11121
		private readonly StoragerIntCachedProperty _rewardCount = new StoragerIntCachedProperty("leprechaunt_rewardCount", false);

		// Token: 0x04002B72 RID: 11122
		private readonly StoragerStringCachedProperty _rewardCurrency = new StoragerStringCachedProperty("leprechaunt_rewardCurrency", false);

		// Token: 0x04002B73 RID: 11123
		private readonly StoragerIntCachedProperty _dropDelaySecs = new StoragerIntCachedProperty("leprechaunt_dropDelay", false);

		// Token: 0x04002B74 RID: 11124
		private readonly PrefsBoolCachedProperty _needReset = new PrefsBoolCachedProperty("leprechaunt_needReset");
	}
}
