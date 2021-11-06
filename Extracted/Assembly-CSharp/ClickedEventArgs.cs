using System;

// Token: 0x02000682 RID: 1666
public sealed class ClickedEventArgs : EventArgs
{
	// Token: 0x060039EF RID: 14831 RVA: 0x0012C21C File Offset: 0x0012A41C
	public ClickedEventArgs(string id)
	{
		this._id = (id ?? string.Empty);
	}

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x060039F0 RID: 14832 RVA: 0x0012C238 File Offset: 0x0012A438
	public string Id
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x04002AA5 RID: 10917
	private readonly string _id;
}
