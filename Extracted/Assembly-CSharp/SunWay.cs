using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class SunWay : MonoBehaviour
{
	// Token: 0x06004BBD RID: 19389 RVA: 0x001B41D4 File Offset: 0x001B23D4
	private void Start()
	{
		base.StartCoroutine(this.GetDistance());
		this.startScaleX = base.transform.GetChild(0).localScale.x;
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x001B4210 File Offset: 0x001B2410
	private void Update()
	{
		if (NickLabelController.currentCamera && this.startDistance > 0f)
		{
			this.distance = Vector2.Distance(new Vector2(NickLabelController.currentCamera.transform.position.x, NickLabelController.currentCamera.transform.position.z), new Vector2(base.transform.position.x, base.transform.position.z));
			base.transform.position = new Vector3(this.sun.position.x, this.waterLevel, this.sun.position.z);
			Vector3 worldPosition = NickLabelController.currentCamera.transform.position + NickLabelController.currentCamera.transform.forward * -0.5f;
			worldPosition.y = base.transform.position.y;
			base.transform.LookAt(worldPosition);
			base.transform.GetChild(0).localScale = new Vector3(this.startScaleX * (1f + Mathf.Clamp(this.sun.transform.position.y / 100f, 0f, 0.3f)), this.startScale * Mathf.Pow(this.distance / this.startDistance, this.multiplier), base.transform.GetChild(0).localScale.z);
			if (this.sun.position.y + 120f > this.waterLevel)
			{
				base.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f - Mathf.Clamp01((this.waterLevel + NickLabelController.currentCamera.transform.position.y + this.sun.transform.position.y / 100f) / (20f + this.waterLevel)));
				this.alpha = 1f;
			}
			else
			{
				this.alpha -= Time.deltaTime;
				base.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.alpha));
			}
		}
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x001B44CC File Offset: 0x001B26CC
	private IEnumerator GetDistance()
	{
		yield return new WaitForSeconds(1f);
		if (NickLabelController.currentCamera)
		{
			this.startDistance = Vector2.Distance(new Vector2(NickLabelController.currentCamera.transform.position.x, NickLabelController.currentCamera.transform.position.z), new Vector2(base.transform.position.x, base.transform.position.z));
			this.startScale = base.transform.GetChild(0).localScale.y;
		}
		yield break;
	}

	// Token: 0x04003AE1 RID: 15073
	public float waterLevel;

	// Token: 0x04003AE2 RID: 15074
	public Transform sun;

	// Token: 0x04003AE3 RID: 15075
	private Vector3 directionLoolAt;

	// Token: 0x04003AE4 RID: 15076
	private float distance;

	// Token: 0x04003AE5 RID: 15077
	private float startDistance;

	// Token: 0x04003AE6 RID: 15078
	private float startScale;

	// Token: 0x04003AE7 RID: 15079
	private float startScaleX;

	// Token: 0x04003AE8 RID: 15080
	public float multiplier = 1f;

	// Token: 0x04003AE9 RID: 15081
	private float alpha = 1f;
}
