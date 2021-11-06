using System;

// Token: 0x02000058 RID: 88
public sealed class ResPath
{
	// Token: 0x06000238 RID: 568 RVA: 0x00013D9C File Offset: 0x00011F9C
	public static string Combine(string a, string b)
	{
		if (a == null || b == null)
		{
			return string.Empty;
		}
		return a + "/" + b;
	}
}
