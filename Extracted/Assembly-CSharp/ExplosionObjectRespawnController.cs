using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public class ExplosionObjectRespawnController : MonoBehaviour
{
	// Token: 0x060034A2 RID: 13474 RVA: 0x0010FF80 File Offset: 0x0010E180
	private void CreateExplosionObject()
	{
		if (this._isMultiplayerMode)
		{
			if (PhotonNetwork.isMasterClient)
			{
				string prefabName = string.Format("ExplosionObjects/{0}", this.explosionObjectPrefab.name);
				this._currentExplosionObject = PhotonNetwork.InstantiateSceneObject(prefabName, base.transform.position, base.transform.rotation, 0, null);
			}
			else
			{
				this._currentExplosionObject = null;
			}
		}
		else
		{
			this._currentExplosionObject = UnityEngine.Object.Instantiate<GameObject>(this.explosionObjectPrefab);
		}
		if (this._currentExplosionObject != null)
		{
			this._currentExplosionObject.transform.parent = base.transform;
			this._currentExplosionObject.transform.localPosition = Vector3.zero;
			this._currentExplosionObject.transform.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x00110050 File Offset: 0x0010E250
	private void Start()
	{
		this._isMultiplayerMode = Defs.isMulti;
		this.CreateExplosionObject();
		ExplosionObjectRespawnController.respawnList.Add(base.gameObject);
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x00110074 File Offset: 0x0010E274
	private void OnDestroy()
	{
		ExplosionObjectRespawnController.respawnList.Remove(base.gameObject);
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x00110088 File Offset: 0x0010E288
	public void StartProcessNewRespawn()
	{
		this._currentExplosionObject = null;
		base.Invoke("CreateExplosionObject", this.timeToNextRespawn);
	}

	// Token: 0x040026A0 RID: 9888
	[Header("Time settings")]
	public float timeToNextRespawn;

	// Token: 0x040026A1 RID: 9889
	[Header("Object settings")]
	public GameObject explosionObjectPrefab;

	// Token: 0x040026A2 RID: 9890
	private GameObject _currentExplosionObject;

	// Token: 0x040026A3 RID: 9891
	private bool _isMultiplayerMode;

	// Token: 0x040026A4 RID: 9892
	public static List<GameObject> respawnList = new List<GameObject>();
}
