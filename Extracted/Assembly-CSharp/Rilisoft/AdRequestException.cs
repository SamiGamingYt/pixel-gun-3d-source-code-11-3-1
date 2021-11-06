using System;

namespace Rilisoft
{
	// Token: 0x02000624 RID: 1572
	public sealed class AdRequestException : Exception
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x00118C7C File Offset: 0x00116E7C
		public AdRequestException(string message) : base(message)
		{
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x00118C88 File Offset: 0x00116E88
		public AdRequestException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
