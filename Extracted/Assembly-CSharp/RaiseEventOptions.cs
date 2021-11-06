using System;

// Token: 0x020003FE RID: 1022
public class RaiseEventOptions
{
	// Token: 0x04001969 RID: 6505
	public static readonly RaiseEventOptions Default = new RaiseEventOptions();

	// Token: 0x0400196A RID: 6506
	public EventCaching CachingOption;

	// Token: 0x0400196B RID: 6507
	public byte InterestGroup;

	// Token: 0x0400196C RID: 6508
	public int[] TargetActors;

	// Token: 0x0400196D RID: 6509
	public ReceiverGroup Receivers;

	// Token: 0x0400196E RID: 6510
	public byte SequenceChannel;

	// Token: 0x0400196F RID: 6511
	public bool ForwardToWebhook;

	// Token: 0x04001970 RID: 6512
	public bool Encrypt;
}
