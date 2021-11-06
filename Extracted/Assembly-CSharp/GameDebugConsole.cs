using System;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class GameDebugConsole : MonoBehaviour
{
	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x0600373E RID: 14142 RVA: 0x0011D328 File Offset: 0x0011B528
	// (set) Token: 0x0600373D RID: 14141 RVA: 0x0011D320 File Offset: 0x0011B520
	public static GameDebugConsole Instance { get; private set; }

	// Token: 0x0600373F RID: 14143 RVA: 0x0011D330 File Offset: 0x0011B530
	public static void AddLogLine(string log)
	{
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x0011D334 File Offset: 0x0011B534
	public static void AddLogLine(string log, Color logColor)
	{
	}

	// Token: 0x06003741 RID: 14145 RVA: 0x0011D338 File Offset: 0x0011B538
	public static void AddLogLine(string log, string styleCode)
	{
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x0011D33C File Offset: 0x0011B53C
	public static void AddLogLine(string log, Color logColor, string styleCode)
	{
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x0011D340 File Offset: 0x0011B540
	public void Clear()
	{
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x0011D344 File Offset: 0x0011B544
	public void ChangeFontSize(int newSize)
	{
	}

	// Token: 0x04002837 RID: 10295
	public UILabel logTextLabel;

	// Token: 0x02000641 RID: 1601
	public class TextStyle
	{
		// Token: 0x04002839 RID: 10297
		public const string bold = "b";

		// Token: 0x0400283A RID: 10298
		public const string italic = "i";

		// Token: 0x0400283B RID: 10299
		public const string strike = "s";

		// Token: 0x0400283C RID: 10300
		public const string underline = "u";

		// Token: 0x0400283D RID: 10301
		public const string sup = "sup";

		// Token: 0x0400283E RID: 10302
		public const string sub = "sub";
	}
}
