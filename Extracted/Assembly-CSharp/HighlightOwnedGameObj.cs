using System;
using Photon;
using UnityEngine;

// Token: 0x0200043D RID: 1085
[RequireComponent(typeof(PhotonView))]
public class HighlightOwnedGameObj : Photon.MonoBehaviour
{
	// Token: 0x060026A1 RID: 9889 RVA: 0x000C1900 File Offset: 0x000BFB00
	private void Update()
	{
		if (base.photonView.isMine)
		{
			if (this.markerTransform == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PointerPrefab);
				gameObject.transform.parent = base.gameObject.transform;
				this.markerTransform = gameObject.transform;
			}
			Vector3 position = base.gameObject.transform.position;
			this.markerTransform.position = new Vector3(position.x, position.y + this.Offset, position.z);
			this.markerTransform.rotation = Quaternion.identity;
		}
		else if (this.markerTransform != null)
		{
			UnityEngine.Object.Destroy(this.markerTransform.gameObject);
			this.markerTransform = null;
		}
	}

	// Token: 0x04001B23 RID: 6947
	public GameObject PointerPrefab;

	// Token: 0x04001B24 RID: 6948
	public float Offset = 0.5f;

	// Token: 0x04001B25 RID: 6949
	private Transform markerTransform;
}
