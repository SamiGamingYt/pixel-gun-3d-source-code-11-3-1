using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020007DB RID: 2011
internal sealed class Bullet : MonoBehaviour
{
	// Token: 0x060048DA RID: 18650 RVA: 0x00194B18 File Offset: 0x00192D18
	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x00194B28 File Offset: 0x00192D28
	public void StartBullet()
	{
		base.gameObject.SetActive(true);
		base.CancelInvoke("RemoveSelf");
		base.Invoke("RemoveSelf", this.LifeTime);
		base.transform.position = this.startPos;
		this.isUse = true;
		this.myRender.enabled = true;
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x00194B84 File Offset: 0x00192D84
	[Obfuscation(Exclude = true)]
	private void RemoveSelf()
	{
		base.transform.position = new Vector3(-10000f, -10000f, -10000f);
		this.myRender.enabled = false;
		this.isUse = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x00194BD0 File Offset: 0x00192DD0
	private void Update()
	{
		if (!this.isUse)
		{
			return;
		}
		base.transform.position += (this.endPos - this.startPos).normalized * this.bulletSpeed * Time.deltaTime;
		if (Vector3.SqrMagnitude(this.startPos - base.transform.position) >= this.lifeS * this.lifeS)
		{
			this.RemoveSelf();
		}
	}

	// Token: 0x040035F1 RID: 13809
	private float LifeTime = 0.5f;

	// Token: 0x040035F2 RID: 13810
	private float RespawnTime;

	// Token: 0x040035F3 RID: 13811
	public float bulletSpeed = 200f;

	// Token: 0x040035F4 RID: 13812
	public float lifeS = 100f;

	// Token: 0x040035F5 RID: 13813
	public Vector3 startPos;

	// Token: 0x040035F6 RID: 13814
	public Vector3 endPos;

	// Token: 0x040035F7 RID: 13815
	public bool isUse;

	// Token: 0x040035F8 RID: 13816
	public TrailRenderer myRender;
}
