using System;
using UnityEngine;

// Token: 0x0200042C RID: 1068
[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Photon Networking/Photon Rigidbody View")]
[RequireComponent(typeof(PhotonView))]
public class PhotonRigidbodyView : MonoBehaviour, IPunObservable
{
	// Token: 0x0600265D RID: 9821 RVA: 0x000C0108 File Offset: 0x000BE308
	private void Awake()
	{
		this.m_Body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x000C0118 File Offset: 0x000BE318
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
				this.m_Body.velocity = (Vector3)stream.ReceiveNext();
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				this.m_Body.angularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
	}

	// Token: 0x04001AC1 RID: 6849
	[SerializeField]
	private bool m_SynchronizeVelocity = true;

	// Token: 0x04001AC2 RID: 6850
	[SerializeField]
	private bool m_SynchronizeAngularVelocity = true;

	// Token: 0x04001AC3 RID: 6851
	private Rigidbody m_Body;
}
