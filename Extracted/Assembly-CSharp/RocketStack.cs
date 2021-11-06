using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
public class RocketStack : MonoBehaviour
{
	// Token: 0x06002C33 RID: 11315 RVA: 0x000EA99C File Offset: 0x000E8B9C
	private void Awake()
	{
		RocketStack.sharedController = this;
		this.mytranform = base.transform;
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x000EA9B0 File Offset: 0x000E8BB0
	public GameObject GetRocket()
	{
		while (this.gameObjects.Count > 0 && this.gameObjects[0] == null)
		{
			this.gameObjects.RemoveAt(0);
		}
		GameObject gameObject;
		if (this.gameObjects.Count > 0)
		{
			gameObject = this.gameObjects[0];
			this.gameObjects.RemoveAt(0);
			gameObject.SetActive(true);
		}
		else
		{
			if (Defs.isMulti)
			{
				if (!Defs.isInet)
				{
					gameObject = (GameObject)Network.Instantiate(Resources.Load("Rocket") as GameObject, Vector3.down * 10000f, Quaternion.identity, 0);
				}
				else
				{
					gameObject = PhotonNetwork.Instantiate("Rocket", Vector3.down * 10000f, Quaternion.identity, 0);
				}
			}
			else
			{
				gameObject = (UnityEngine.Object.Instantiate(Resources.Load("Rocket") as GameObject, Vector3.down * 10000f, Quaternion.identity) as GameObject);
			}
			gameObject.transform.parent = this.mytranform;
		}
		return gameObject;
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x000EAADC File Offset: 0x000E8CDC
	public void ReturnRocket(GameObject returnObject)
	{
		Rigidbody component = returnObject.GetComponent<Rigidbody>();
		component.velocity = Vector3.zero;
		component.isKinematic = false;
		component.useGravity = false;
		component.angularVelocity = Vector3.zero;
		returnObject.transform.position = Vector3.down * 10000f;
		returnObject.SetActive(false);
		this.timeUseGameObjects = Time.realtimeSinceStartup;
		this.gameObjects.Add(returnObject);
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x000EAB4C File Offset: 0x000E8D4C
	private void OnDestroy()
	{
		RocketStack.sharedController = null;
	}

	// Token: 0x04002138 RID: 8504
	public static RocketStack sharedController;

	// Token: 0x04002139 RID: 8505
	private List<GameObject> gameObjects = new List<GameObject>();

	// Token: 0x0400213A RID: 8506
	private float timeUseGameObjects;

	// Token: 0x0400213B RID: 8507
	public Transform mytranform;
}
