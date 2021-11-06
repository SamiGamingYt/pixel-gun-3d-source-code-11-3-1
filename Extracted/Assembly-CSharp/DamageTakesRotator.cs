using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
public sealed class DamageTakesRotator : MonoBehaviour
{
	// Token: 0x06000423 RID: 1059 RVA: 0x00023A98 File Offset: 0x00021C98
	private void Start()
	{
		this.thisTransform = base.transform;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00023AA8 File Offset: 0x00021CA8
	private void Update()
	{
		if (this.myPlayer == null)
		{
			if (Defs.isMulti)
			{
				this.myPlayer = WeaponManager.sharedManager.myPlayer;
			}
			else
			{
				this.myPlayer = GameObject.FindGameObjectWithTag("Player");
			}
		}
		if (this.myPlayer == null)
		{
			return;
		}
		this.thisTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.myPlayer.transform.localRotation.eulerAngles.y));
	}

	// Token: 0x040004B4 RID: 1204
	private Transform thisTransform;

	// Token: 0x040004B5 RID: 1205
	public InGameGUI inGameGUI;

	// Token: 0x040004B6 RID: 1206
	private GameObject myPlayer;
}
