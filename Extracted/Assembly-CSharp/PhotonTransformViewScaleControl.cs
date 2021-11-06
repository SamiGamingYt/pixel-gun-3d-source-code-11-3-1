using System;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class PhotonTransformViewScaleControl
{
	// Token: 0x06002678 RID: 9848 RVA: 0x000C0B8C File Offset: 0x000BED8C
	public PhotonTransformViewScaleControl(PhotonTransformViewScaleModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x06002679 RID: 9849 RVA: 0x000C0BA8 File Offset: 0x000BEDA8
	public Vector3 GetNetworkScale()
	{
		return this.m_NetworkScale;
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000C0BB0 File Offset: 0x000BEDB0
	public Vector3 GetScale(Vector3 currentScale)
	{
		switch (this.m_Model.InterpolateOption)
		{
		default:
			return this.m_NetworkScale;
		case PhotonTransformViewScaleModel.InterpolateOptions.MoveTowards:
			return Vector3.MoveTowards(currentScale, this.m_NetworkScale, this.m_Model.InterpolateMoveTowardsSpeed * Time.deltaTime);
		case PhotonTransformViewScaleModel.InterpolateOptions.Lerp:
			return Vector3.Lerp(currentScale, this.m_NetworkScale, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
		}
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x000C0C24 File Offset: 0x000BEE24
	public void OnPhotonSerializeView(Vector3 currentScale, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			stream.SendNext(currentScale);
			this.m_NetworkScale = currentScale;
		}
		else
		{
			this.m_NetworkScale = (Vector3)stream.ReceiveNext();
		}
	}

	// Token: 0x04001AF8 RID: 6904
	private PhotonTransformViewScaleModel m_Model;

	// Token: 0x04001AF9 RID: 6905
	private Vector3 m_NetworkScale = Vector3.one;
}
