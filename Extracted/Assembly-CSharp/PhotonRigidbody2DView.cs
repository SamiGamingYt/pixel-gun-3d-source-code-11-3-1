using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
[AddComponentMenu("Photon Networking/Photon Rigidbody 2D View")]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody2D))]
public class PhotonRigidbody2DView : MonoBehaviour, IPunObservable
{
	// Token: 0x0600265A RID: 9818 RVA: 0x000C003C File Offset: 0x000BE23C
	private void Awake()
	{
		this.m_Body = base.GetComponent<Rigidbody2D>();
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x000C004C File Offset: 0x000BE24C
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.m_SynchronizeVelocity)
			{
				stream.SendNext(this.m_Body.velocity);
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				stream.SendNext(this.m_Body.angularVelocity);
			}
		}
		else
		{
			if (this.m_SynchronizeVelocity)
			{
				this.m_Body.velocity = (Vector2)stream.ReceiveNext();
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				this.m_Body.angularVelocity = (float)stream.ReceiveNext();
			}
		}
	}

	// Token: 0x04001ABE RID: 6846
	[SerializeField]
	private bool m_SynchronizeVelocity = true;

	// Token: 0x04001ABF RID: 6847
	[SerializeField]
	private bool m_SynchronizeAngularVelocity = true;

	// Token: 0x04001AC0 RID: 6848
	private Rigidbody2D m_Body;
}
