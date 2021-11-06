using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public class PhotonStream
{
	// Token: 0x06002501 RID: 9473 RVA: 0x000BA2C0 File Offset: 0x000B84C0
	public PhotonStream(bool write, object[] incomingData)
	{
		this.write = write;
		if (incomingData == null)
		{
			this.writeData = new Queue<object>(10);
		}
		else
		{
			this.readData = incomingData;
		}
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x000BA2FC File Offset: 0x000B84FC
	public void SetReadStream(object[] incomingData, byte pos = 0)
	{
		this.readData = incomingData;
		this.currentItem = pos;
		this.write = false;
	}

	// Token: 0x06002503 RID: 9475 RVA: 0x000BA314 File Offset: 0x000B8514
	internal void ResetWriteStream()
	{
		this.writeData.Clear();
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06002504 RID: 9476 RVA: 0x000BA324 File Offset: 0x000B8524
	public bool isWriting
	{
		get
		{
			return this.write;
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06002505 RID: 9477 RVA: 0x000BA32C File Offset: 0x000B852C
	public bool isReading
	{
		get
		{
			return !this.write;
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06002506 RID: 9478 RVA: 0x000BA338 File Offset: 0x000B8538
	public int Count
	{
		get
		{
			return (!this.isWriting) ? this.readData.Length : this.writeData.Count;
		}
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x000BA360 File Offset: 0x000B8560
	public object ReceiveNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		object result = this.readData[(int)this.currentItem];
		this.currentItem += 1;
		return result;
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x000BA3A4 File Offset: 0x000B85A4
	public object PeekNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		return this.readData[(int)this.currentItem];
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x000BA3D8 File Offset: 0x000B85D8
	public void SendNext(object obj)
	{
		if (!this.write)
		{
			Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
			return;
		}
		this.writeData.Enqueue(obj);
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000BA408 File Offset: 0x000B8608
	public object[] ToArray()
	{
		return (!this.isWriting) ? this.readData : this.writeData.ToArray();
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000BA42C File Offset: 0x000B862C
	public void Serialize(ref bool myBool)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myBool);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			myBool = (bool)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000BA494 File Offset: 0x000B8694
	public void Serialize(ref int myInt)
	{
		if (this.write)
		{
			this.writeData.Enqueue(myInt);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			myInt = (int)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000BA4FC File Offset: 0x000B86FC
	public void Serialize(ref string value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (string)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000BA55C File Offset: 0x000B875C
	public void Serialize(ref char value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (char)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000BA5C4 File Offset: 0x000B87C4
	public void Serialize(ref short value)
	{
		if (this.write)
		{
			this.writeData.Enqueue(value);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			value = (short)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000BA62C File Offset: 0x000B882C
	public void Serialize(ref float obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (float)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000BA694 File Offset: 0x000B8894
	public void Serialize(ref PhotonPlayer obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (PhotonPlayer)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000BA6F4 File Offset: 0x000B88F4
	public void Serialize(ref Vector3 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Vector3)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06002513 RID: 9491 RVA: 0x000BA764 File Offset: 0x000B8964
	public void Serialize(ref Vector2 obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Vector2)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000BA7D4 File Offset: 0x000B89D4
	public void Serialize(ref Quaternion obj)
	{
		if (this.write)
		{
			this.writeData.Enqueue(obj);
		}
		else if (this.readData.Length > (int)this.currentItem)
		{
			obj = (Quaternion)this.readData[(int)this.currentItem];
			this.currentItem += 1;
		}
	}

	// Token: 0x04001A03 RID: 6659
	private bool write;

	// Token: 0x04001A04 RID: 6660
	private Queue<object> writeData;

	// Token: 0x04001A05 RID: 6661
	private object[] readData;

	// Token: 0x04001A06 RID: 6662
	internal byte currentItem;
}
