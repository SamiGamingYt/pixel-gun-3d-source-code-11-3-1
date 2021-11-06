using System;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class OnJoinedInstantiate : MonoBehaviour
{
	// Token: 0x060026DD RID: 9949 RVA: 0x000C2D78 File Offset: 0x000C0F78
	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate != null)
		{
			foreach (GameObject gameObject in this.PrefabsToInstantiate)
			{
				Debug.Log("Instantiating: " + gameObject.name);
				Vector3 a = Vector3.up;
				if (this.SpawnPosition != null)
				{
					a = this.SpawnPosition.position;
				}
				Vector3 a2 = UnityEngine.Random.insideUnitSphere;
				a2.y = 0f;
				a2 = a2.normalized;
				Vector3 position = a + this.PositionOffset * a2;
				PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, 0);
			}
		}
	}

	// Token: 0x04001B55 RID: 6997
	public Transform SpawnPosition;

	// Token: 0x04001B56 RID: 6998
	public float PositionOffset = 2f;

	// Token: 0x04001B57 RID: 6999
	public GameObject[] PrefabsToInstantiate;
}
