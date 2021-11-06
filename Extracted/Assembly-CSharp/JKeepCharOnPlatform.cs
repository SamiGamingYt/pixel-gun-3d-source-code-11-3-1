using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class JKeepCharOnPlatform : MonoBehaviour
{
	// Token: 0x06001974 RID: 6516 RVA: 0x000655CC File Offset: 0x000637CC
	private void OnTriggerEnter(Collider other)
	{
		CharacterController characterController = other.GetComponent(typeof(CharacterController)) as CharacterController;
		if (characterController == null)
		{
			return;
		}
		Transform transform = other.transform;
		float yOffset = characterController.height / 2f - characterController.center.y + this.verticalOffset;
		JKeepCharOnPlatform.Data data = new JKeepCharOnPlatform.Data(characterController, transform, yOffset);
		this.onPlatform.Add(other.transform, data);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x00065648 File Offset: 0x00063848
	private void OnTriggerExit(Collider other)
	{
		this.onPlatform.Remove(other.transform);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x0006565C File Offset: 0x0006385C
	private void Start()
	{
		this.lastPos = base.transform.position;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x00065670 File Offset: 0x00063870
	private void Update()
	{
		Vector3 position = base.transform.position;
		float y = position.y;
		Vector3 b = position - this.lastPos;
		float y2 = b.y;
		b.y = 0f;
		this.lastPos = position;
		foreach (object obj in this.onPlatform)
		{
			JKeepCharOnPlatform.Data data = (JKeepCharOnPlatform.Data)((DictionaryEntry)obj).Value;
			float y3 = data.ctrl.velocity.y;
			if (y3 <= 0f || y3 <= y2)
			{
				Vector3 vector = data.t.position;
				vector.y = y + data.yOffset;
				vector += b;
				data.t.position = vector;
			}
		}
	}

	// Token: 0x04000E97 RID: 3735
	public float verticalOffset = 0.5f;

	// Token: 0x04000E98 RID: 3736
	private Hashtable onPlatform = new Hashtable();

	// Token: 0x04000E99 RID: 3737
	private Vector3 lastPos;

	// Token: 0x020002D8 RID: 728
	public struct Data
	{
		// Token: 0x06001978 RID: 6520 RVA: 0x0006578C File Offset: 0x0006398C
		public Data(CharacterController ctrl, Transform t, float yOffset)
		{
			this.ctrl = ctrl;
			this.t = t;
			this.yOffset = yOffset;
		}

		// Token: 0x04000E9A RID: 3738
		public CharacterController ctrl;

		// Token: 0x04000E9B RID: 3739
		public Transform t;

		// Token: 0x04000E9C RID: 3740
		public float yOffset;
	}
}
