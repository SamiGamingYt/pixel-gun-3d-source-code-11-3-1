using System;
using UnityEngine;

// Token: 0x02000759 RID: 1881
internal sealed class Shoot : MonoBehaviour
{
	// Token: 0x0600420B RID: 16907 RVA: 0x0015F57C File Offset: 0x0015D77C
	private void Start()
	{
		this._bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
	}

	// Token: 0x0600420C RID: 16908 RVA: 0x0015F590 File Offset: 0x0015D790
	[PunRPC]
	[RPC]
	private void Popal(NetworkViewID Popal, NetworkMessageInfo info)
	{
		Debug.Log(string.Concat(new object[]
		{
			Popal,
			" ",
			base.gameObject.transform.GetComponent<NetworkView>().viewID,
			" ",
			info.sender
		}));
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x0015F5F4 File Offset: 0x0015D7F4
	public void shootS()
	{
		Debug.Log("Shot!!" + base.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, Player_move_c._ShootRaycastLayerMask))
		{
			Debug.Log("Hit!");
			if (raycastHit.collider.gameObject.transform.CompareTag("Enemy") && Defs.isMulti)
			{
				base.GetComponent<NetworkView>().RPC("Popal", RPCMode.All, new object[]
				{
					raycastHit.collider.gameObject.transform.GetComponent<NetworkView>().viewID
				});
			}
		}
	}

	// Token: 0x0400304E RID: 12366
	public float Range = 1000f;

	// Token: 0x0400304F RID: 12367
	public Transform _transform;

	// Token: 0x04003050 RID: 12368
	public GameObject bullet;

	// Token: 0x04003051 RID: 12369
	private GameObject _bulletSpawnPoint;

	// Token: 0x04003052 RID: 12370
	public int lives = 100;
}
