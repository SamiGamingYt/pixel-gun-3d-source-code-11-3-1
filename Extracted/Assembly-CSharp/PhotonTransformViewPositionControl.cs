using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class PhotonTransformViewPositionControl
{
	// Token: 0x06002669 RID: 9833 RVA: 0x000C0510 File Offset: 0x000BE710
	public PhotonTransformViewPositionControl(PhotonTransformViewPositionModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x0600266A RID: 9834 RVA: 0x000C0548 File Offset: 0x000BE748
	private Vector3 GetOldestStoredNetworkPosition()
	{
		Vector3 result = this.m_NetworkPosition;
		if (this.m_OldNetworkPositions.Count > 0)
		{
			result = this.m_OldNetworkPositions.Peek();
		}
		return result;
	}

	// Token: 0x0600266B RID: 9835 RVA: 0x000C057C File Offset: 0x000BE77C
	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_SynchronizedSpeed = speed;
		this.m_SynchronizedTurnSpeed = turnSpeed;
	}

	// Token: 0x0600266C RID: 9836 RVA: 0x000C058C File Offset: 0x000BE78C
	public Vector3 UpdatePosition(Vector3 currentPosition)
	{
		Vector3 vector = this.GetNetworkPosition() + this.GetExtrapolatedPositionOffset();
		switch (this.m_Model.InterpolateOption)
		{
		case PhotonTransformViewPositionModel.InterpolateOptions.Disabled:
			if (!this.m_UpdatedPositionAfterOnSerialize)
			{
				currentPosition = vector;
				this.m_UpdatedPositionAfterOnSerialize = true;
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.FixedSpeed:
			currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * this.m_Model.InterpolateMoveTowardsSpeed);
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed:
			if (this.m_OldNetworkPositions.Count != 0)
			{
				float num = Vector3.Distance(this.m_NetworkPosition, this.GetOldestStoredNetworkPosition()) / (float)this.m_OldNetworkPositions.Count * (float)PhotonNetwork.sendRateOnSerialize;
				currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * num);
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues:
			if (this.m_SynchronizedSpeed.magnitude == 0f)
			{
				currentPosition = vector;
			}
			else
			{
				currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * this.m_SynchronizedSpeed.magnitude);
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.Lerp:
			currentPosition = Vector3.Lerp(currentPosition, vector, Time.deltaTime * this.m_Model.InterpolateLerpSpeed);
			break;
		}
		if (this.m_Model.TeleportEnabled && Vector3.Distance(currentPosition, this.GetNetworkPosition()) > this.m_Model.TeleportIfDistanceGreaterThan)
		{
			currentPosition = this.GetNetworkPosition();
		}
		return currentPosition;
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x000C06F0 File Offset: 0x000BE8F0
	public Vector3 GetNetworkPosition()
	{
		return this.m_NetworkPosition;
	}

	// Token: 0x0600266E RID: 9838 RVA: 0x000C06F8 File Offset: 0x000BE8F8
	public Vector3 GetExtrapolatedPositionOffset()
	{
		float num = (float)(PhotonNetwork.time - this.m_LastSerializeTime);
		if (this.m_Model.ExtrapolateIncludingRoundTripTime)
		{
			num += (float)PhotonNetwork.GetPing() / 1000f;
		}
		Vector3 result = Vector3.zero;
		switch (this.m_Model.ExtrapolateOption)
		{
		case PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues:
		{
			Quaternion rotation = Quaternion.Euler(0f, this.m_SynchronizedTurnSpeed * num, 0f);
			result = rotation * (this.m_SynchronizedSpeed * num);
			break;
		}
		case PhotonTransformViewPositionModel.ExtrapolateOptions.EstimateSpeedAndTurn:
		{
			Vector3 a = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()) * (float)PhotonNetwork.sendRateOnSerialize;
			result = a * num;
			break;
		}
		case PhotonTransformViewPositionModel.ExtrapolateOptions.FixedSpeed:
		{
			Vector3 normalized = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()).normalized;
			result = normalized * this.m_Model.ExtrapolateSpeed * num;
			break;
		}
		}
		return result;
	}

	// Token: 0x0600266F RID: 9839 RVA: 0x000C07F4 File Offset: 0x000BE9F4
	public void OnPhotonSerializeView(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			this.SerializeData(currentPosition, stream, info);
		}
		else
		{
			this.DeserializeData(stream, info);
		}
		this.m_LastSerializeTime = PhotonNetwork.time;
		this.m_UpdatedPositionAfterOnSerialize = false;
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000C0848 File Offset: 0x000BEA48
	private void SerializeData(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(currentPosition);
		this.m_NetworkPosition = currentPosition;
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			stream.SendNext(this.m_SynchronizedSpeed);
			stream.SendNext(this.m_SynchronizedTurnSpeed);
		}
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x000C08AC File Offset: 0x000BEAAC
	private void DeserializeData(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 networkPosition = (Vector3)stream.ReceiveNext();
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			this.m_SynchronizedSpeed = (Vector3)stream.ReceiveNext();
			this.m_SynchronizedTurnSpeed = (float)stream.ReceiveNext();
		}
		if (this.m_OldNetworkPositions.Count == 0)
		{
			this.m_NetworkPosition = networkPosition;
		}
		this.m_OldNetworkPositions.Enqueue(this.m_NetworkPosition);
		this.m_NetworkPosition = networkPosition;
		while (this.m_OldNetworkPositions.Count > this.m_Model.ExtrapolateNumberOfStoredPositions)
		{
			this.m_OldNetworkPositions.Dequeue();
		}
	}

	// Token: 0x04001ACD RID: 6861
	private PhotonTransformViewPositionModel m_Model;

	// Token: 0x04001ACE RID: 6862
	private float m_CurrentSpeed;

	// Token: 0x04001ACF RID: 6863
	private double m_LastSerializeTime;

	// Token: 0x04001AD0 RID: 6864
	private Vector3 m_SynchronizedSpeed = Vector3.zero;

	// Token: 0x04001AD1 RID: 6865
	private float m_SynchronizedTurnSpeed;

	// Token: 0x04001AD2 RID: 6866
	private Vector3 m_NetworkPosition;

	// Token: 0x04001AD3 RID: 6867
	private Queue<Vector3> m_OldNetworkPositions = new Queue<Vector3>();

	// Token: 0x04001AD4 RID: 6868
	private bool m_UpdatedPositionAfterOnSerialize = true;
}
