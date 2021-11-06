using System;

namespace Rilisoft
{
	// Token: 0x0200069C RID: 1692
	public class VerificationEventArgs : EventArgs
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003B3C RID: 15164 RVA: 0x00133B68 File Offset: 0x00131D68
		// (set) Token: 0x06003B3D RID: 15165 RVA: 0x00133B70 File Offset: 0x00131D70
		public VerificationErrorCode ErrorCode { get; set; }

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003B3E RID: 15166 RVA: 0x00133B7C File Offset: 0x00131D7C
		// (set) Token: 0x06003B3F RID: 15167 RVA: 0x00133B84 File Offset: 0x00131D84
		public int SentNonce { get; set; }

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003B40 RID: 15168 RVA: 0x00133B90 File Offset: 0x00131D90
		// (set) Token: 0x06003B41 RID: 15169 RVA: 0x00133B98 File Offset: 0x00131D98
		public string SentPackageName { get; set; }

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003B42 RID: 15170 RVA: 0x00133BA4 File Offset: 0x00131DA4
		// (set) Token: 0x06003B43 RID: 15171 RVA: 0x00133BAC File Offset: 0x00131DAC
		public string ReceivedPackageName { get; set; }

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003B44 RID: 15172 RVA: 0x00133BB8 File Offset: 0x00131DB8
		// (set) Token: 0x06003B45 RID: 15173 RVA: 0x00133BC0 File Offset: 0x00131DC0
		public int ReceivedNonce { get; set; }

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003B46 RID: 15174 RVA: 0x00133BCC File Offset: 0x00131DCC
		// (set) Token: 0x06003B47 RID: 15175 RVA: 0x00133BD4 File Offset: 0x00131DD4
		public ResponseCode ReceivedResponseCode { get; set; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003B48 RID: 15176 RVA: 0x00133BE0 File Offset: 0x00131DE0
		// (set) Token: 0x06003B49 RID: 15177 RVA: 0x00133BE8 File Offset: 0x00131DE8
		public long ReceivedTimestamp { get; set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003B4A RID: 15178 RVA: 0x00133BF4 File Offset: 0x00131DF4
		// (set) Token: 0x06003B4B RID: 15179 RVA: 0x00133BFC File Offset: 0x00131DFC
		public string ReceivedUserId { get; set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x00133C08 File Offset: 0x00131E08
		// (set) Token: 0x06003B4D RID: 15181 RVA: 0x00133C10 File Offset: 0x00131E10
		public int ReceivedVersionCode { get; set; }
	}
}
