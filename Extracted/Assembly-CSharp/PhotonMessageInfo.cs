using System;

// Token: 0x0200040F RID: 1039
public struct PhotonMessageInfo
{
	// Token: 0x060024FD RID: 9469 RVA: 0x000BA25C File Offset: 0x000B845C
	public PhotonMessageInfo(PhotonPlayer player, int timestamp, PhotonView view)
	{
		this.sender = player;
		this.timeInt = timestamp;
		this.photonView = view;
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x060024FE RID: 9470 RVA: 0x000BA274 File Offset: 0x000B8474
	public double timestamp
	{
		get
		{
			uint num = (uint)this.timeInt;
			double num2 = num;
			return num2 / 1000.0;
		}
	}

	// Token: 0x060024FF RID: 9471 RVA: 0x000BA298 File Offset: 0x000B8498
	public override string ToString()
	{
		return string.Format("[PhotonMessageInfo: Sender='{1}' Senttime={0}]", this.timestamp, this.sender);
	}

	// Token: 0x040019F4 RID: 6644
	private readonly int timeInt;

	// Token: 0x040019F5 RID: 6645
	public readonly PhotonPlayer sender;

	// Token: 0x040019F6 RID: 6646
	public readonly PhotonView photonView;
}
