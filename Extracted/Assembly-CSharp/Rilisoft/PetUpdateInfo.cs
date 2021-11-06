using System;

namespace Rilisoft
{
	// Token: 0x020006F9 RID: 1785
	public class PetUpdateInfo
	{
		// Token: 0x06003DE8 RID: 15848 RVA: 0x0014C5D0 File Offset: 0x0014A7D0
		public PetUpdateInfo()
		{
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0014C5D8 File Offset: 0x0014A7D8
		public PetUpdateInfo(PlayerPet petOld, PlayerPet petNew)
		{
			this.PetOld = petOld;
			this.PetNew = petNew;
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003DEA RID: 15850 RVA: 0x0014C5F0 File Offset: 0x0014A7F0
		public bool PetAdded
		{
			get
			{
				return this.PetOld == null;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06003DEB RID: 15851 RVA: 0x0014C5FC File Offset: 0x0014A7FC
		public bool Upgraded
		{
			get
			{
				return this.PetOld != null && this.PetNew != null && this.PetOld.InfoId != this.PetNew.InfoId;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x0014C640 File Offset: 0x0014A840
		public bool PetPointsAdded
		{
			get
			{
				return this.PetOld != null && this.PetNew != null && this.PetOld.Points != this.PetNew.Points;
			}
		}

		// Token: 0x04002DDA RID: 11738
		public PlayerPet PetOld;

		// Token: 0x04002DDB RID: 11739
		public PlayerPet PetNew;
	}
}
