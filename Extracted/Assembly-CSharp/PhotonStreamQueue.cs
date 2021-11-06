using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class PhotonStreamQueue
{
	// Token: 0x060025E3 RID: 9699 RVA: 0x000BDBEC File Offset: 0x000BBDEC
	public PhotonStreamQueue(int sampleRate)
	{
		this.m_SampleRate = sampleRate;
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x000BDC34 File Offset: 0x000BBE34
	private void BeginWritePackage()
	{
		if (Time.realtimeSinceStartup < this.m_LastSampleTime + 1f / (float)this.m_SampleRate)
		{
			this.m_IsWriting = false;
			return;
		}
		if (this.m_SampleCount == 1)
		{
			this.m_ObjectsPerSample = this.m_Objects.Count;
		}
		else if (this.m_SampleCount > 1 && this.m_Objects.Count / this.m_SampleCount != this.m_ObjectsPerSample)
		{
			Debug.LogWarning("The number of objects sent via a PhotonStreamQueue has to be the same each frame");
			Debug.LogWarning(string.Concat(new object[]
			{
				"Objects in List: ",
				this.m_Objects.Count,
				" / Sample Count: ",
				this.m_SampleCount,
				" = ",
				this.m_Objects.Count / this.m_SampleCount,
				" != ",
				this.m_ObjectsPerSample
			}));
		}
		this.m_IsWriting = true;
		this.m_SampleCount++;
		this.m_LastSampleTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x000BDD58 File Offset: 0x000BBF58
	public void Reset()
	{
		this.m_SampleCount = 0;
		this.m_ObjectsPerSample = -1;
		this.m_LastSampleTime = float.NegativeInfinity;
		this.m_LastFrameCount = -1;
		this.m_Objects.Clear();
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x000BDD88 File Offset: 0x000BBF88
	public void SendNext(object obj)
	{
		if (Time.frameCount != this.m_LastFrameCount)
		{
			this.BeginWritePackage();
		}
		this.m_LastFrameCount = Time.frameCount;
		if (!this.m_IsWriting)
		{
			return;
		}
		this.m_Objects.Add(obj);
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x000BDDC4 File Offset: 0x000BBFC4
	public bool HasQueuedObjects()
	{
		return this.m_NextObjectIndex != -1;
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x000BDDD4 File Offset: 0x000BBFD4
	public object ReceiveNext()
	{
		if (this.m_NextObjectIndex == -1)
		{
			return null;
		}
		if (this.m_NextObjectIndex >= this.m_Objects.Count)
		{
			this.m_NextObjectIndex -= this.m_ObjectsPerSample;
		}
		return this.m_Objects[this.m_NextObjectIndex++];
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x000BDE34 File Offset: 0x000BC034
	public void Serialize(PhotonStream stream)
	{
		if (this.m_Objects.Count > 0 && this.m_ObjectsPerSample < 0)
		{
			this.m_ObjectsPerSample = this.m_Objects.Count;
		}
		stream.SendNext(this.m_SampleCount);
		stream.SendNext(this.m_ObjectsPerSample);
		for (int i = 0; i < this.m_Objects.Count; i++)
		{
			stream.SendNext(this.m_Objects[i]);
		}
		this.m_Objects.Clear();
		this.m_SampleCount = 0;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x000BDED4 File Offset: 0x000BC0D4
	public void Deserialize(PhotonStream stream)
	{
		this.m_Objects.Clear();
		this.m_SampleCount = (int)stream.ReceiveNext();
		this.m_ObjectsPerSample = (int)stream.ReceiveNext();
		for (int i = 0; i < this.m_SampleCount * this.m_ObjectsPerSample; i++)
		{
			this.m_Objects.Add(stream.ReceiveNext());
		}
		if (this.m_Objects.Count > 0)
		{
			this.m_NextObjectIndex = 0;
		}
		else
		{
			this.m_NextObjectIndex = -1;
		}
	}

	// Token: 0x04001A4C RID: 6732
	private int m_SampleRate;

	// Token: 0x04001A4D RID: 6733
	private int m_SampleCount;

	// Token: 0x04001A4E RID: 6734
	private int m_ObjectsPerSample = -1;

	// Token: 0x04001A4F RID: 6735
	private float m_LastSampleTime = float.NegativeInfinity;

	// Token: 0x04001A50 RID: 6736
	private int m_LastFrameCount = -1;

	// Token: 0x04001A51 RID: 6737
	private int m_NextObjectIndex = -1;

	// Token: 0x04001A52 RID: 6738
	private List<object> m_Objects = new List<object>();

	// Token: 0x04001A53 RID: 6739
	private bool m_IsWriting;
}
