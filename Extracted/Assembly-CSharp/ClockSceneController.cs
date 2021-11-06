using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class ClockSceneController : MonoBehaviour
{
	// Token: 0x06000330 RID: 816 RVA: 0x0001B7C4 File Offset: 0x000199C4
	private void Start()
	{
		this.thisTransform = base.transform;
		this.UpdateAngle();
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0001B7D8 File Offset: 0x000199D8
	private void Update()
	{
		this.UpdateAngle();
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0001B7E0 File Offset: 0x000199E0
	private void UpdateAngle()
	{
		DateTime now = DateTime.Now;
		int num = (this.type != ClockSceneController.TypeHand.minutes) ? (now.Hour * 60 + now.Minute) : now.Minute;
		if (num == this.oldValue)
		{
			return;
		}
		if (this.bats != null && num < this.oldValue && num == 0)
		{
			this.bats.timer = 10f;
			this.bats.gameObject.SetActive(true);
		}
		this.oldValue = num;
		if (this.type == ClockSceneController.TypeHand.hour && num >= 720)
		{
			num -= 720;
		}
		float y = 360f * (float)num / ((this.type != ClockSceneController.TypeHand.minutes) ? 720f : 60f);
		this.thisTransform.localRotation = Quaternion.Euler(new Vector3(0f, y, 0f));
	}

	// Token: 0x0400036F RID: 879
	public ClockSceneController.TypeHand type;

	// Token: 0x04000370 RID: 880
	private Transform thisTransform;

	// Token: 0x04000371 RID: 881
	public DisableObjectFromTimer bats;

	// Token: 0x04000372 RID: 882
	private int oldValue = -1000;

	// Token: 0x0200006F RID: 111
	public enum TypeHand
	{
		// Token: 0x04000374 RID: 884
		minutes,
		// Token: 0x04000375 RID: 885
		hour
	}
}
