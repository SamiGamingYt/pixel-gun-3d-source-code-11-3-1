using System;
using UnityEngine;

// Token: 0x02000795 RID: 1941
internal sealed class crossHair : MonoBehaviour
{
	// Token: 0x060045B4 RID: 17844 RVA: 0x00178FE0 File Offset: 0x001771E0
	private void Start()
	{
		this.photonView = PhotonView.Get(this);
		if ((((!Defs.isInet && base.GetComponent<NetworkView>().isMine) || (Defs.isInet && this.photonView.isMine)) && Defs.isMulti) || !Defs.isMulti)
		{
			this.crossHairPosition = new Rect((float)((Screen.width - this.crossHairTexture.width * Screen.height / 640) / 2), (float)((Screen.height - this.crossHairTexture.height * Screen.height / 640) / 2), (float)(this.crossHairTexture.width * Screen.height / 640), (float)(this.crossHairTexture.height * Screen.height / 640));
			this.pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
			this.playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		}
	}

	// Token: 0x060045B5 RID: 17845 RVA: 0x001790E8 File Offset: 0x001772E8
	private void OnGUI()
	{
		if ((((!Defs.isInet && base.GetComponent<NetworkView>().isMine) || (Defs.isInet && this.photonView.isMine)) && Defs.isMulti) || !Defs.isMulti)
		{
			if (this.pauser.paused)
			{
				return;
			}
			GUI.DrawTexture(this.crossHairPosition, this.crossHairTexture);
		}
	}

	// Token: 0x0400331E RID: 13086
	public Texture2D crossHairTexture;

	// Token: 0x0400331F RID: 13087
	private Rect crossHairPosition;

	// Token: 0x04003320 RID: 13088
	private Pauser pauser;

	// Token: 0x04003321 RID: 13089
	private Player_move_c playerMoveC;

	// Token: 0x04003322 RID: 13090
	private PhotonView photonView;
}
