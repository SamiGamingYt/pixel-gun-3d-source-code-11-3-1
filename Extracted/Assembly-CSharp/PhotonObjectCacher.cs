using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class PhotonObjectCacher : MonoBehaviour
{
	// Token: 0x060027D2 RID: 10194 RVA: 0x000C65C0 File Offset: 0x000C47C0
	public static void AddObject(GameObject obj)
	{
		if (PhotonNetwork.SendMonoMessageTargets == null)
		{
			PhotonNetwork.SendMonoMessageTargets = new HashSet<GameObject>();
		}
		if (obj != null && !PhotonNetwork.SendMonoMessageTargets.Contains(obj))
		{
			PhotonNetwork.SendMonoMessageTargets.Add(obj);
		}
	}

	// Token: 0x060027D3 RID: 10195 RVA: 0x000C660C File Offset: 0x000C480C
	public static void RemoveObject(GameObject obj)
	{
		if (PhotonNetwork.SendMonoMessageTargets == null)
		{
			return;
		}
		if (PhotonNetwork.SendMonoMessageTargets.Contains(obj))
		{
			PhotonNetwork.SendMonoMessageTargets.Remove(obj);
		}
	}
}
