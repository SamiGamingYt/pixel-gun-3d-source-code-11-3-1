using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.SavedGame;

namespace Rilisoft
{
	// Token: 0x02000838 RID: 2104
	public struct GoogleSavedGameRequestResult<TValue> : IEquatable<GoogleSavedGameRequestResult<TValue>>
	{
		// Token: 0x06004C68 RID: 19560 RVA: 0x001B8684 File Offset: 0x001B6884
		public GoogleSavedGameRequestResult(SavedGameRequestStatus requestStatus, TValue value)
		{
			this._requestStatus = requestStatus;
			this._value = value;
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x001B8694 File Offset: 0x001B6894
		public SavedGameRequestStatus RequestStatus
		{
			get
			{
				return this._requestStatus;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x001B869C File Offset: 0x001B689C
		public TValue Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001B86A4 File Offset: 0x001B68A4
		public bool Equals(GoogleSavedGameRequestResult<TValue> other)
		{
			return this._requestStatus.Equals(other.RequestStatus) && EqualityComparer<TValue>.Default.Equals(other.Value);
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001B86F4 File Offset: 0x001B68F4
		public override bool Equals(object obj)
		{
			if (!(obj is GoogleSavedGameRequestResult<TValue>))
			{
				return false;
			}
			GoogleSavedGameRequestResult<TValue> other = (GoogleSavedGameRequestResult<TValue>)obj;
			return this.Equals(other);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x001B871C File Offset: 0x001B691C
		public override int GetHashCode()
		{
			int hashCode = this.RequestStatus.GetHashCode();
			TValue value = this.Value;
			return hashCode ^ value.GetHashCode();
		}

		// Token: 0x04003B45 RID: 15173
		private readonly SavedGameRequestStatus _requestStatus;

		// Token: 0x04003B46 RID: 15174
		private readonly TValue _value;
	}
}
