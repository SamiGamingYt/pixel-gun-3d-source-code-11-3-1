using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class DisableObjectFromTimer : MonoBehaviour
{
	// Token: 0x06000462 RID: 1122 RVA: 0x000250D8 File Offset: 0x000232D8
	private void Start()
	{
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000250DC File Offset: 0x000232DC
	private void Update()
	{
		if (this.timer >= 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				if (this.isDestroy)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x040004E8 RID: 1256
	public float timer = -1f;

	// Token: 0x040004E9 RID: 1257
	public bool isDestroy;
}
