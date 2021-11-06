using System;

// Token: 0x020003EC RID: 1004
public class FriendInfo
{
	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x060023E1 RID: 9185 RVA: 0x000B2B84 File Offset: 0x000B0D84
	// (set) Token: 0x060023E2 RID: 9186 RVA: 0x000B2B8C File Offset: 0x000B0D8C
	public string Name { get; protected internal set; }

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x060023E3 RID: 9187 RVA: 0x000B2B98 File Offset: 0x000B0D98
	// (set) Token: 0x060023E4 RID: 9188 RVA: 0x000B2BA0 File Offset: 0x000B0DA0
	public bool IsOnline { get; protected internal set; }

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x060023E5 RID: 9189 RVA: 0x000B2BAC File Offset: 0x000B0DAC
	// (set) Token: 0x060023E6 RID: 9190 RVA: 0x000B2BB4 File Offset: 0x000B0DB4
	public string Room { get; protected internal set; }

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000B2BC0 File Offset: 0x000B0DC0
	public bool IsInRoom
	{
		get
		{
			return this.IsOnline && !string.IsNullOrEmpty(this.Room);
		}
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000B2BE0 File Offset: 0x000B0DE0
	public override string ToString()
	{
		return string.Format("{0}\t is: {1}", this.Name, this.IsOnline ? ((!this.IsInRoom) ? "on master" : "playing") : "offline");
	}
}
