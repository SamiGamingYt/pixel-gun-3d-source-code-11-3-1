using System;

// Token: 0x02000400 RID: 1024
public class TypedLobby
{
	// Token: 0x06002426 RID: 9254 RVA: 0x000B3C80 File Offset: 0x000B1E80
	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000B3C9C File Offset: 0x000B1E9C
	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06002429 RID: 9257 RVA: 0x000B3CC0 File Offset: 0x000B1EC0
	public bool IsDefault
	{
		get
		{
			return this.Type == LobbyType.Default && string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x000B3CDC File Offset: 0x000B1EDC
	public override string ToString()
	{
		return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
	}

	// Token: 0x04001975 RID: 6517
	public string Name;

	// Token: 0x04001976 RID: 6518
	public LobbyType Type;

	// Token: 0x04001977 RID: 6519
	public static readonly TypedLobby Default = new TypedLobby();
}
