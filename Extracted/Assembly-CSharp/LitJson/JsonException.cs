using System;

namespace LitJson
{
	// Token: 0x02000143 RID: 323
	public class JsonException : ApplicationException
	{
		// Token: 0x06000A5B RID: 2651 RVA: 0x0003B748 File Offset: 0x00039948
		public JsonException()
		{
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0003B750 File Offset: 0x00039950
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0003B768 File Offset: 0x00039968
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0003B784 File Offset: 0x00039984
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0003B7A0 File Offset: 0x000399A0
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0003B7BC File Offset: 0x000399BC
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0003B7C8 File Offset: 0x000399C8
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
