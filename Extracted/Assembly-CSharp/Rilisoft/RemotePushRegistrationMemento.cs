using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200073A RID: 1850
	[Serializable]
	internal struct RemotePushRegistrationMemento : IEquatable<RemotePushRegistrationMemento>
	{
		// Token: 0x0600410F RID: 16655 RVA: 0x0015B6F4 File Offset: 0x001598F4
		public RemotePushRegistrationMemento(string registrationId, DateTime registrationTime, string version)
		{
			this.registrationId = (registrationId ?? string.Empty);
			this.registrationTime = registrationTime.ToString("s", CultureInfo.InvariantCulture);
			this.version = (version ?? string.Empty);
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06004110 RID: 16656 RVA: 0x0015B734 File Offset: 0x00159934
		public string RegistrationId
		{
			get
			{
				return this.registrationId ?? string.Empty;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x0015B748 File Offset: 0x00159948
		public string RegistrationTime
		{
			get
			{
				return this.registrationTime ?? string.Empty;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06004112 RID: 16658 RVA: 0x0015B75C File Offset: 0x0015995C
		public string Version
		{
			get
			{
				return this.version ?? string.Empty;
			}
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0015B770 File Offset: 0x00159970
		public bool Equals(RemotePushRegistrationMemento other)
		{
			return !(this.RegistrationTime != other.RegistrationTime) && !(this.Version != other.Version) && !(this.RegistrationId != other.RegistrationId);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0015B7CC File Offset: 0x001599CC
		public override bool Equals(object obj)
		{
			if (!(obj is RemotePushRegistrationMemento))
			{
				return false;
			}
			RemotePushRegistrationMemento other = (RemotePushRegistrationMemento)obj;
			return this.Equals(other);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0015B7F4 File Offset: 0x001599F4
		public override int GetHashCode()
		{
			return this.RegistrationTime.GetHashCode() ^ this.Version.GetHashCode() ^ this.RegistrationId.GetHashCode();
		}

		// Token: 0x04002F8D RID: 12173
		[SerializeField]
		private string registrationId;

		// Token: 0x04002F8E RID: 12174
		[SerializeField]
		private string registrationTime;

		// Token: 0x04002F8F RID: 12175
		[SerializeField]
		private string version;
	}
}
