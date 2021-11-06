using System;
using System.Text;

namespace Rilisoft
{
	// Token: 0x02000744 RID: 1860
	public class MailUrlBuilder
	{
		// Token: 0x06004171 RID: 16753 RVA: 0x0015CCE8 File Offset: 0x0015AEE8
		public string GetUrl()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("mailto:{0}", this.to);
			string arg = Uri.EscapeUriString(this.subject);
			stringBuilder.AppendFormat("?subject={0}", arg);
			string arg2 = Uri.EscapeUriString(this.body);
			stringBuilder.AppendFormat("&body={0}", arg2);
			return stringBuilder.ToString();
		}

		// Token: 0x04002FC6 RID: 12230
		public string to;

		// Token: 0x04002FC7 RID: 12231
		public string subject;

		// Token: 0x04002FC8 RID: 12232
		public string body;
	}
}
