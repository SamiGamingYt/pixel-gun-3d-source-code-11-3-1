using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000579 RID: 1401
	internal sealed class BattleInviteListener
	{
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x000FCC2C File Offset: 0x000FAE2C
		public static BattleInviteListener Instance
		{
			get
			{
				return BattleInviteListener.s_instance;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000FCC34 File Offset: 0x000FAE34
		// (set) Token: 0x06003089 RID: 12425 RVA: 0x000FCC68 File Offset: 0x000FAE68
		internal TimeSpan OutgoingInviteTimeout
		{
			get
			{
				TimeSpan? outgoingInviteTimeout = this._outgoingInviteTimeout;
				return (outgoingInviteTimeout == null) ? BattleInviteListener.DefaultOutgoingInviteTimeout : outgoingInviteTimeout.Value;
			}
			set
			{
				this._outgoingInviteTimeout = new TimeSpan?(value);
			}
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000FCC78 File Offset: 0x000FAE78
		internal bool CallToFriendEnabled(string friendId)
		{
			if (string.IsNullOrEmpty(friendId))
			{
				return false;
			}
			float? outgoingInviteTimestamp = this.GetOutgoingInviteTimestamp(friendId);
			return outgoingInviteTimestamp == null || (double)(Time.realtimeSinceStartup - outgoingInviteTimestamp.Value) >= this.OutgoingInviteTimeout.TotalSeconds;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000FCCD0 File Offset: 0x000FAED0
		internal float? GetOutgoingInviteTimestamp(string friendId)
		{
			if (friendId == null)
			{
				return null;
			}
			float value;
			if (this._outgoingInviteTimestamps.TryGetValue(friendId, out value))
			{
				return new float?(value);
			}
			return null;
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x000FCD10 File Offset: 0x000FAF10
		internal void SetOutgoingInviteTimestamp(string friendId, float timestamp)
		{
			if (friendId == null)
			{
				return;
			}
			this._outgoingInviteTimestamps[friendId] = timestamp;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000FCD28 File Offset: 0x000FAF28
		internal IEnumerable<string> GetFriendIds()
		{
			return this._incomingInviteFriendIds;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000FCD30 File Offset: 0x000FAF30
		internal void ClearIncomingInvites()
		{
			this._incomingInviteFriendIds.Clear();
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000FCD40 File Offset: 0x000FAF40
		internal void NotifyBattleIncomingInvite(string friendId, string nickname)
		{
			if (string.IsNullOrEmpty(friendId))
			{
				return;
			}
			if (string.IsNullOrEmpty(nickname))
			{
				return;
			}
			this._incomingInviteFriendIds.Add(friendId);
			InfoWindowController.Instance.ShowBattleInvite(nickname);
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x000FCD80 File Offset: 0x000FAF80
		private static TimeSpan DefaultOutgoingInviteTimeout
		{
			get
			{
				return (!Application.isEditor) ? TimeSpan.FromMinutes(15.0) : TimeSpan.FromSeconds(15.0);
			}
		}

		// Token: 0x040023A6 RID: 9126
		private static readonly BattleInviteListener s_instance = new BattleInviteListener();

		// Token: 0x040023A7 RID: 9127
		private readonly HashSet<string> _incomingInviteFriendIds = new HashSet<string>();

		// Token: 0x040023A8 RID: 9128
		private readonly Dictionary<string, float> _outgoingInviteTimestamps = new Dictionary<string, float>();

		// Token: 0x040023A9 RID: 9129
		private TimeSpan? _outgoingInviteTimeout;
	}
}
