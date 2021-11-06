using System;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class MovingDummy : MonoBehaviour
{
	// Token: 0x06001B52 RID: 6994 RVA: 0x000701E4 File Offset: 0x0006E3E4
	private void Awake()
	{
		this.myDummy = base.GetComponent<BaseDummy>();
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000701F4 File Offset: 0x0006E3F4
	private void Start()
	{
		this.startPoint = base.transform.localPosition;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x00070208 File Offset: 0x0006E408
	public void ResetPath()
	{
		base.transform.localPosition = this.startPoint;
		this.moveVector = Vector3.zero;
		this.currentPoint = 0;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x00070230 File Offset: 0x0006E430
	private void Update()
	{
		if (this.pathPoints == null || this.pathPoints.Length == 0 || this.myDummy.isDead)
		{
			return;
		}
		if (this.currentPoint >= this.pathPoints.Length)
		{
			this.currentPoint = 0;
		}
		if ((base.transform.localPosition - (this.startPoint + this.pathPoints[this.currentPoint])).sqrMagnitude > 1f)
		{
			Vector3 normalized = (this.startPoint + this.pathPoints[this.currentPoint] - base.transform.localPosition).normalized;
			this.moveVector = Vector3.MoveTowards(this.moveVector, normalized, this.smooth * Time.deltaTime);
			base.transform.localPosition += this.moveVector * this.speed * Time.deltaTime;
		}
		else
		{
			this.currentPoint++;
		}
	}

	// Token: 0x0400107B RID: 4219
	private BaseDummy myDummy;

	// Token: 0x0400107C RID: 4220
	private Vector3 startPoint;

	// Token: 0x0400107D RID: 4221
	public Vector3[] pathPoints;

	// Token: 0x0400107E RID: 4222
	public float speed = 2f;

	// Token: 0x0400107F RID: 4223
	public float smooth = 1f;

	// Token: 0x04001080 RID: 4224
	private int currentPoint;

	// Token: 0x04001081 RID: 4225
	private Vector3 moveVector;
}
