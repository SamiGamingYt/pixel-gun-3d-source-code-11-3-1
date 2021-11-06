using System;
using UnityEngine;

// Token: 0x02000895 RID: 2197
public class testMem : MonoBehaviour
{
	// Token: 0x06004EF4 RID: 20212 RVA: 0x001C9A74 File Offset: 0x001C7C74
	private void Start()
	{
		if (!meminfo.getMemInfo())
		{
			Debug.Log("Could not get Memory Info");
		}
	}

	// Token: 0x06004EF5 RID: 20213 RVA: 0x001C9A98 File Offset: 0x001C7C98
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	// Token: 0x06004EF6 RID: 20214 RVA: 0x001C9AAC File Offset: 0x001C7CAC
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, (float)(Screen.height - 50), 180f, 40f), "Get MemInfo"))
		{
			meminfo.getMemInfo();
		}
		if (GUI.Button(new Rect(200f, (float)(Screen.height - 50), 180f, 40f), "native Gc Collect"))
		{
			meminfo.gc_Collect();
		}
		GUI.Label(new Rect(50f, 10f, 250f, 40f), "memtotal: " + meminfo.minf.memtotal.ToString() + " kb");
		GUI.Label(new Rect(50f, 50f, 250f, 40f), "memfree: " + meminfo.minf.memfree.ToString() + " kb");
		GUI.Label(new Rect(50f, 90f, 250f, 40f), "active: " + meminfo.minf.active.ToString() + " kb");
		GUI.Label(new Rect(50f, 130f, 250f, 40f), "inactive: " + meminfo.minf.inactive.ToString() + " kb");
		GUI.Label(new Rect(50f, 170f, 250f, 40f), "cached: " + meminfo.minf.cached.ToString() + " kb");
		GUI.Label(new Rect(50f, 210f, 250f, 40f), "swapcached: " + meminfo.minf.swapcached.ToString() + " kb");
		GUI.Label(new Rect(50f, 250f, 250f, 40f), "swaptotal: " + meminfo.minf.swaptotal.ToString() + " kb");
		GUI.Label(new Rect(50f, 290f, 250f, 40f), "swapfree: " + meminfo.minf.swapfree.ToString() + " kb");
	}
}
