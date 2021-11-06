using System;
using UnityEngine;

// Token: 0x0200042D RID: 1069
[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Transform View")]
public class PhotonTransformView : MonoBehaviour, IPunObservable
{
	// Token: 0x06002660 RID: 9824 RVA: 0x000C01E8 File Offset: 0x000BE3E8
	private void Awake()
	{
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_PositionControl = new PhotonTransformViewPositionControl(this.m_PositionModel);
		this.m_RotationControl = new PhotonTransformViewRotationControl(this.m_RotationModel);
		this.m_ScaleControl = new PhotonTransformViewScaleControl(this.m_ScaleModel);
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x000C0234 File Offset: 0x000BE434
	private void OnEnable()
	{
		this.m_firstTake = true;
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x000C0240 File Offset: 0x000BE440
	private void Update()
	{
		if (this.m_PhotonView == null || this.m_PhotonView.isMine || !PhotonNetwork.connected)
		{
			return;
		}
		this.UpdatePosition();
		this.UpdateRotation();
		this.UpdateScale();
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x000C028C File Offset: 0x000BE48C
	private void UpdatePosition()
	{
		if (!this.m_PositionModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localPosition = this.m_PositionControl.UpdatePosition(base.transform.localPosition);
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x000C02D8 File Offset: 0x000BE4D8
	private void UpdateRotation()
	{
		if (!this.m_RotationModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localRotation = this.m_RotationControl.GetRotation(base.transform.localRotation);
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x000C0324 File Offset: 0x000BE524
	private void UpdateScale()
	{
		if (!this.m_ScaleModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localScale = this.m_ScaleControl.GetScale(base.transform.localScale);
	}

	// Token: 0x06002666 RID: 9830 RVA: 0x000C0370 File Offset: 0x000BE570
	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_PositionControl.SetSynchronizedValues(speed, turnSpeed);
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x000C0380 File Offset: 0x000BE580
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.m_PositionControl.OnPhotonSerializeView(base.transform.localPosition, stream, info);
		this.m_RotationControl.OnPhotonSerializeView(base.transform.localRotation, stream, info);
		this.m_ScaleControl.OnPhotonSerializeView(base.transform.localScale, stream, info);
		if (!this.m_PhotonView.isMine && this.m_PositionModel.DrawErrorGizmo)
		{
			this.DoDrawEstimatedPositionError();
		}
		if (stream.isReading)
		{
			this.m_ReceivedNetworkUpdate = true;
			if (this.m_firstTake)
			{
				this.m_firstTake = false;
				base.transform.localPosition = this.m_PositionControl.GetNetworkPosition();
				base.transform.localRotation = this.m_RotationControl.GetNetworkRotation();
				base.transform.localScale = this.m_ScaleControl.GetNetworkScale();
			}
		}
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x000C0464 File Offset: 0x000BE664
	private void DoDrawEstimatedPositionError()
	{
		Vector3 vector = this.m_PositionControl.GetNetworkPosition();
		if (base.transform.parent != null)
		{
			vector = base.transform.parent.position + vector;
		}
		Debug.DrawLine(vector, base.transform.position, Color.red, 2f);
		Debug.DrawLine(base.transform.position, base.transform.position + Vector3.up, Color.green, 2f);
		Debug.DrawLine(vector, vector + Vector3.up, Color.red, 2f);
	}

	// Token: 0x04001AC4 RID: 6852
	[SerializeField]
	private PhotonTransformViewPositionModel m_PositionModel = new PhotonTransformViewPositionModel();

	// Token: 0x04001AC5 RID: 6853
	[SerializeField]
	private PhotonTransformViewRotationModel m_RotationModel = new PhotonTransformViewRotationModel();

	// Token: 0x04001AC6 RID: 6854
	[SerializeField]
	private PhotonTransformViewScaleModel m_ScaleModel = new PhotonTransformViewScaleModel();

	// Token: 0x04001AC7 RID: 6855
	private PhotonTransformViewPositionControl m_PositionControl;

	// Token: 0x04001AC8 RID: 6856
	private PhotonTransformViewRotationControl m_RotationControl;

	// Token: 0x04001AC9 RID: 6857
	private PhotonTransformViewScaleControl m_ScaleControl;

	// Token: 0x04001ACA RID: 6858
	private PhotonView m_PhotonView;

	// Token: 0x04001ACB RID: 6859
	private bool m_ReceivedNetworkUpdate;

	// Token: 0x04001ACC RID: 6860
	private bool m_firstTake;
}
