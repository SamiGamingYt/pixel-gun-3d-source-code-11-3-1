using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// Token: 0x02000413 RID: 1043
public class WebRpcResponse
{
	// Token: 0x06002518 RID: 9496 RVA: 0x000BA884 File Offset: 0x000B8A84
	public WebRpcResponse(OperationResponse response)
	{
		object obj;
		response.Parameters.TryGetValue(209, out obj);
		this.Name = (obj as string);
		response.Parameters.TryGetValue(207, out obj);
		this.ReturnCode = ((obj == null) ? -1 : ((int)((byte)obj)));
		response.Parameters.TryGetValue(208, out obj);
		this.Parameters = (obj as Dictionary<string, object>);
		response.Parameters.TryGetValue(206, out obj);
		this.DebugMessage = (obj as string);
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06002519 RID: 9497 RVA: 0x000BA920 File Offset: 0x000B8B20
	// (set) Token: 0x0600251A RID: 9498 RVA: 0x000BA928 File Offset: 0x000B8B28
	public string Name { get; private set; }

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x0600251B RID: 9499 RVA: 0x000BA934 File Offset: 0x000B8B34
	// (set) Token: 0x0600251C RID: 9500 RVA: 0x000BA93C File Offset: 0x000B8B3C
	public int ReturnCode { get; private set; }

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x0600251D RID: 9501 RVA: 0x000BA948 File Offset: 0x000B8B48
	// (set) Token: 0x0600251E RID: 9502 RVA: 0x000BA950 File Offset: 0x000B8B50
	public string DebugMessage { get; private set; }

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x0600251F RID: 9503 RVA: 0x000BA95C File Offset: 0x000B8B5C
	// (set) Token: 0x06002520 RID: 9504 RVA: 0x000BA964 File Offset: 0x000B8B64
	public Dictionary<string, object> Parameters { get; private set; }

	// Token: 0x06002521 RID: 9505 RVA: 0x000BA970 File Offset: 0x000B8B70
	public string ToStringFull()
	{
		return string.Format("{0}={2}: {1} \"{3}\"", new object[]
		{
			this.Name,
			SupportClass.DictionaryToString(this.Parameters),
			this.ReturnCode,
			this.DebugMessage
		});
	}
}
