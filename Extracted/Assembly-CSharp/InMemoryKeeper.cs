using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class InMemoryKeeper : MonoBehaviour
{
	// Token: 0x06001913 RID: 6419 RVA: 0x00061B0C File Offset: 0x0005FD0C
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.objectsToKeepInMemory.Add(Resources.Load<GameObject>("Rocket"));
		this.objectsToKeepInMemory.AddRange(Resources.LoadAll<GameObject>("Rays/"));
		this.objectsToKeepInMemory.AddRange(Resources.LoadAll<GameObject>("Explosions/"));
	}

	// Token: 0x04000E24 RID: 3620
	public List<GameObject> objectsToKeepInMemory = new List<GameObject>();
}
