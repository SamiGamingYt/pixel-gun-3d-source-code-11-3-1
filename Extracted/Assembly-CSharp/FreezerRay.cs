using System;
using UnityEngine;

// Token: 0x0200060E RID: 1550
public class FreezerRay : MonoBehaviour
{
	// Token: 0x170008D5 RID: 2261
	// (set) Token: 0x06003515 RID: 13589 RVA: 0x00112AB4 File Offset: 0x00110CB4
	public float Length
	{
		set
		{
			base.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, value));
		}
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x00112AE8 File Offset: 0x00110CE8
	private void OnEnable()
	{
		this.timeLeft += this.lifetime;
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x00112B00 File Offset: 0x00110D00
	private void Update()
	{
		this.timeLeft -= Time.deltaTime;
		if (this.timeLeft <= 0f)
		{
			base.GetComponent<RayAndExplosionsStackItem>().Deactivate();
			return;
		}
		if (this.mc != null && this.target != null && this.target.parent != null && this.target.parent.parent != null)
		{
			base.transform.position = this.target.position;
			base.transform.forward = this.target.parent.parent.forward;
		}
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x00112BC4 File Offset: 0x00110DC4
	public void Activate(Player_move_c move_c, Transform gunFlash)
	{
		this.mc = move_c;
		this.target = gunFlash;
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x00112BD4 File Offset: 0x00110DD4
	public void UpdatePosition(float length)
	{
		this.timeLeft += this.lifetime;
		this.Length = length;
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x00112BF0 File Offset: 0x00110DF0
	private void OnDisable()
	{
		if (this.mc != null)
		{
			this.mc.RemoveRay(this);
		}
	}

	// Token: 0x040026F4 RID: 9972
	private Player_move_c mc;

	// Token: 0x040026F5 RID: 9973
	public float lifetime = 0.1f;

	// Token: 0x040026F6 RID: 9974
	public float timeLeft;

	// Token: 0x040026F7 RID: 9975
	private Transform target;
}
