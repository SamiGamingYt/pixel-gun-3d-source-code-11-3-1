using System;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class PhotonTransformViewRotationControl
{
	// Token: 0x06002673 RID: 9843 RVA: 0x000C0A84 File Offset: 0x000BEC84
	public PhotonTransformViewRotationControl(PhotonTransformViewRotationModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x06002674 RID: 9844 RVA: 0x000C0A94 File Offset: 0x000BEC94
	public Quaternion GetNetworkRotation()
	{
		return this.m_NetworkRotation;
	}

	// Token: 0x06002675 RID: 9845 RVA: 0x000C0A9C File Offset: 0x000BEC9C
	public Quaternion GetRotation(Quaternion currentRotation)
	{
		switch (this.m_Model.InterpolateOption)
		{
		default:
			return this.m_NetworkRotation;
		case PhotonTransformViewRotationModel.InterpolateOptions.RotateTowards:
			return Quaternion.RotateTowards(currentRotation, this.m_NetworkRotation, this.m_Model.InterpolateRotateTowardsSpeed * Time.deltaTime);
		case PhotonTransformViewRotationModel.InterpolateOptions.Lerp:
			return Quaternion.Lerp(currentRotation, this.m_NetworkRotation, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
		}
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x000C0B10 File Offset: 0x000BED10
	public void OnPhotonSerializeView(Quaternion currentRotation, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			stream.SendNext(currentRotation);
			this.m_NetworkRotation = currentRotation;
		}
		else
		{
			this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x04001AEE RID: 6894
	private PhotonTransformViewRotationModel m_Model;

	// Token: 0x04001AEF RID: 6895
	private Quaternion m_NetworkRotation;
}
