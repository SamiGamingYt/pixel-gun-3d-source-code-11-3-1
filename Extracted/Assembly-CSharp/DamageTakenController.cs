using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class DamageTakenController : MonoBehaviour
{
	// Token: 0x0600041E RID: 1054 RVA: 0x00023968 File Offset: 0x00021B68
	public void reset(float alpha)
	{
		this.time = this.maxTime;
		base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -alpha));
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00023998 File Offset: 0x00021B98
	private void Start()
	{
		this.mySprite.color = new Color(1f, 1f, 1f, 0f);
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x000239CC File Offset: 0x00021BCC
	public void Remove()
	{
		this.time = -1f;
		this.mySprite.color = new Color(1f, 1f, 1f, 0f);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00023A00 File Offset: 0x00021C00
	private void Update()
	{
		if (this.time > 0f)
		{
			this.mySprite.color = new Color(1f, 1f, 1f, this.time / this.maxTime);
			this.time -= Time.deltaTime;
			if (this.time < 0f)
			{
				this.mySprite.color = new Color(1f, 1f, 1f, 0f);
			}
		}
	}

	// Token: 0x040004B1 RID: 1201
	private float time;

	// Token: 0x040004B2 RID: 1202
	private float maxTime = 3f;

	// Token: 0x040004B3 RID: 1203
	public UISprite mySprite;
}
