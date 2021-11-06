using System;
using System.Reflection;
using Rilisoft;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
internal sealed class SetParentWeapon : MonoBehaviour
{
	// Token: 0x060045D6 RID: 17878 RVA: 0x001797A4 File Offset: 0x001779A4
	private void Start()
	{
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
		{
			return;
		}
		this.isMulti = Defs.isMulti;
		if (!this.isMulti)
		{
			return;
		}
		this.isInet = Defs.isInet;
		this.photonView = PhotonView.Get(this);
		if (!this.isInet)
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		else
		{
			this.isMine = this.photonView.isMine;
		}
		this.SetParent();
	}

	// Token: 0x060045D7 RID: 17879 RVA: 0x0017982C File Offset: 0x00177A2C
	[Obfuscation(Exclude = true)]
	private void SetParent()
	{
		int num = -1;
		NetworkPlayer owner = base.GetComponent<NetworkView>().owner;
		if (this.isInet && this.photonView && this.photonView.owner != null)
		{
			num = this.photonView.owner.ID;
		}
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if ((this.isInet && player_move_c.mySkinName.photonView != null && player_move_c.mySkinName.photonView.owner != null && player_move_c.mySkinName.photonView.owner.ID == num) || (!this.isInet && player_move_c.mySkinName.GetComponent<NetworkView>().owner.Equals(owner)))
			{
				GameObject playerGameObject = player_move_c.mySkinName.playerGameObject;
				base.transform.position = Vector3.zero;
				if (!base.transform.GetComponent<WeaponSounds>().isMelee)
				{
					foreach (object obj in base.transform)
					{
						Transform transform = (Transform)obj;
						if (transform.gameObject.name.Equals("BulletSpawnPoint") && transform.childCount >= 0)
						{
							GameObject gameObject = transform.GetChild(0).gameObject;
							if (!this.isMine)
							{
								WeaponManager.SetGunFlashActive(gameObject, false);
							}
							break;
						}
					}
				}
				foreach (object obj2 in playerGameObject.transform)
				{
					Transform transform2 = (Transform)obj2;
					transform2.parent = null;
					transform2.position += -Vector3.up * 1000f;
				}
				base.transform.parent = playerGameObject.transform;
				if (base.transform.FindChild("BulletSpawnPoint") != null)
				{
					player_move_c._bulletSpawnPoint = base.transform.FindChild("BulletSpawnPoint").gameObject;
				}
				base.transform.localPosition = new Vector3(0f, -1.7f, 0f);
				base.transform.rotation = playerGameObject.transform.rotation;
				GameObject gameObject2 = playerGameObject.transform.parent.gameObject;
				if (gameObject2 != null)
				{
					player_move_c.SetTextureForBodyPlayer(player_move_c._skin);
				}
				return;
			}
		}
		base.Invoke("SetParent", 0.1f);
	}

	// Token: 0x04003331 RID: 13105
	private bool isMine;

	// Token: 0x04003332 RID: 13106
	private bool isInet;

	// Token: 0x04003333 RID: 13107
	private bool isMulti;

	// Token: 0x04003334 RID: 13108
	private PhotonView photonView;
}
