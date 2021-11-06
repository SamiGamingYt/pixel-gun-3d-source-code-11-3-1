using System;
using Photon;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class SettingBonus : Photon.MonoBehaviour
{
	// Token: 0x060041CB RID: 16843 RVA: 0x0015DF3C File Offset: 0x0015C13C
	private void Start()
	{
	}

	// Token: 0x060041CC RID: 16844 RVA: 0x0015DF40 File Offset: 0x0015C140
	public void SetNumberSpawnZone(int _number)
	{
		base.photonView.RPC("SynchNamberSpawnZoneRPC", PhotonTargets.AllBuffered, new object[]
		{
			_number
		});
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x0015DF70 File Offset: 0x0015C170
	[RPC]
	[PunRPC]
	public void SynchNamberSpawnZoneRPC(int _number)
	{
		this.numberSpawnZone = _number;
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x0015DF7C File Offset: 0x0015C17C
	private void Update()
	{
	}

	// Token: 0x04003014 RID: 12308
	public int typeOfMass;

	// Token: 0x04003015 RID: 12309
	public int numberSpawnZone = -1;
}
